
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmManageDetainedLicenses : Form
    {

        private static DataTable _dtAllDetainedLicenses;

        private ClsLicenseBusiness License; 
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private string ResolveColumnName(params string[] candidates)
        {
            if (_dtAllDetainedLicenses == null) return null;
            foreach (var c in candidates)
            {
                if (string.IsNullOrEmpty(c)) continue;
                if (_dtAllDetainedLicenses.Columns.Contains(c)) return c;
            }
            return null;
        }

        private bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
                || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong)
                || type == typeof(decimal) || type == typeof(float) || type == typeof(double);
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbFilter.Items.Add("None");

            cbFilter.Items.Add("Detain ID");

            cbFilter.Items.Add("License ID");

            cbFilter.Items.Add("Is Released");

            cbFilter.Items.Add("National No");

            cbFilter.Items.Add("Full Name");

            cbFilter.Items.Add("Release Application ID");

            _RefreshDetainedLicensesList(); 


        }



        private void _RefreshDetainedLicensesList()
        {
            _dtAllDetainedLicenses = ClsDetainedLicenseBusiness.GetAllDetainedLicenses();

            dgDetainLicense.DataSource = _dtAllDetainedLicenses;

            if (dgDetainLicense.Rows.Count > 0)
            {
                // Safe check for the column name (with or without space)
                string columnName = dgDetainLicense.Columns.Contains("FineFees") ? "FineFees" : "Fine Fees";

                if (dgDetainLicense.Columns.Contains(columnName))
                {
                    dgDetainLicense.Columns[columnName].DefaultCellStyle.Format = "0.00";
                }

            }
        }  
        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgDetainLicense.CurrentRow == null) return;
            string licenseCol = ResolveColumnName("L.ID", "License ID", "LicenseID", "LID", "ID");
            if (string.IsNullOrEmpty(licenseCol))
            {
                MessageBox.Show("License ID column not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedLicenseID = Convert.ToInt32(dgDetainLicense.CurrentRow.Cells[licenseCol].Value);

            frmLicenseInfo frm = new frmLicenseInfo(selectedLicenseID);
            frm.ShowDialog();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = ""; 

            if(cbFilter.Text == "None")
            {
                txtFilterValue.Visible = false; 
                if(_dtAllDetainedLicenses != null)
                {
                    _dtAllDetainedLicenses.DefaultView.RowFilter = ""; 
                }
            }
            else
            {
                txtFilterValue.Visible = true;

                txtFilterValue.Focus(); 
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_dtAllDetainedLicenses == null)
            {
                return;
            }

            string filterColumn = null;
            switch (cbFilter.Text)
            {
                case "Detain ID":
                    filterColumn = ResolveColumnName("D.ID", "Detain ID", "DetainID", "DetainId", "ID");
                    break;
                case "License ID":
                    filterColumn = ResolveColumnName("L.ID", "License ID", "LicenseID", "LID", "ID");
                    break;
                case "National No":
                    filterColumn = ResolveColumnName("N.No", "National No", "NationalNo", "No");
                    break;
                case "Full Name":
                    filterColumn = ResolveColumnName("Full Name", "FullName", "Name", "PersonName");
                    break;
                case "Is Released":
                    filterColumn = ResolveColumnName("Is Released", "IsReleased", "Released");
                    break;
            }

            if (string.IsNullOrEmpty(filterColumn) || string.IsNullOrEmpty(txtFilterValue.Text))
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                return;
            }

            var col = _dtAllDetainedLicenses.Columns[filterColumn];

            // numeric column handling
            if (col != null && IsNumericType(col.DataType))
            {
                if (int.TryParse(txtFilterValue.Text.Trim(), out int IDValue))
                {
                    _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, IDValue);
                }
                else
                {
                    _dtAllDetainedLicenses.DefaultView.RowFilter = "1=0";
                }
                return;
            }

            // boolean column handling
            if (col != null && col.DataType == typeof(bool))
            {
                string filterText = txtFilterValue.Text.Trim().ToLower();
                if (filterText == "yes" || filterText == "true" || filterText == "1")
                {
                    _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = true", filterColumn);
                }
                else if (filterText == "no" || filterText == "false" || filterText == "0")
                {
                    _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = false", filterColumn);
                }
                return;
            }

            // default: string LIKE
            _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim().Replace("'", "''"));
        }

        private void cmsDetainedLicenses_Opening(object sender, CancelEventArgs e)
        {
            if(dgDetainLicense.CurrentRow == null)
            {
                return; 
            }

            string isReleasedCol = ResolveColumnName("Is Released", "IsReleased", "Released");
            if (string.IsNullOrEmpty(isReleasedCol))
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;
                return;
            }

            bool IsReleased = Convert.ToBoolean(dgDetainLicense.CurrentRow.Cells[isReleasedCol].Value);
            releaseDetainedLicenseToolStripMenuItem.Enabled = !IsReleased;


        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgDetainLicense.CurrentRow == null) return;

            string nationalNoCol = ResolveColumnName("N.No", "National No", "NationalNo", "No");
            if (string.IsNullOrEmpty(nationalNoCol))
            {
                MessageBox.Show("National number column not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nationalNo = dgDetainLicense.CurrentRow.Cells[nationalNoCol].Value.ToString();

            ClsPerson person = ClsPerson.FindPersonByNationalNo(nationalNo);

            if(person != null)
            {
                frmShowPersonInfo frm = new frmShowPersonInfo(person.PersonID);

                frm.ShowDialog();

                _RefreshDetainedLicensesList(); 
            }
            else
            {
                MessageBox.Show("Unable to find person record details associated with national tracking ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgDetainLicense.CurrentRow == null) return;

            string nationalNoCol = ResolveColumnName("N.No", "National No", "NationalNo", "No");
            if (string.IsNullOrEmpty(nationalNoCol))
            {
                MessageBox.Show("National number column not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nationalNo = dgDetainLicense.CurrentRow.Cells[nationalNoCol].Value.ToString();

            ClsPerson person = ClsPerson.FindPersonByNationalNo(nationalNo);

            if(person != null)
            {
                frmPersonLicenseHistory frm = new frmPersonLicenseHistory(person.PersonID);
                frm.ShowDialog();
            }
            else
            {
                    MessageBox.Show("Unable to find person record details associated with national tracking ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgDetainLicense.CurrentRow == null) return;

            string licenseCol = ResolveColumnName("L.ID", "License ID", "LicenseID", "LID", "ID");
            if (string.IsNullOrEmpty(licenseCol))
            {
                MessageBox.Show("License ID column not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedLicenseID = Convert.ToInt32(dgDetainLicense.CurrentRow.Cells[licenseCol].Value);

            frmReleaseDetainLicense frm = new frmReleaseDetainLicense(selectedLicenseID);
            frm.ShowDialog();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicensApp frm = new frmDetainLicensApp();
            frm.ShowDialog();

            _RefreshDetainedLicensesList();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainLicense frm = new frmReleaseDetainLicense();
            frm.ShowDialog();

            _RefreshDetainedLicensesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgDetainLicense_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
