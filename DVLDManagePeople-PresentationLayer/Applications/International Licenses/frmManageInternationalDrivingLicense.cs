
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business; 
namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmManageInternationalDrivingLicense : Form
    {
        private static DataTable _dtAllInternationalLicenses;
        public frmManageInternationalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmManageInternationalDrivingLicense_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.Clear();
            cbFilterBy.Items.Add("None");
            cbFilterBy.Items.Add("Int. License ID");
            cbFilterBy.Items.Add("Application ID");
            cbFilterBy.Items.Add("Driver ID");
            cbFilterBy.Items.Add("L.ID"); // Local License ID
            cbFilterBy.Items.Add("Is Active");

            cbFilterBy.SelectedIndex = 0; // Default selection to "None"
            txtFilter.Visible = false;

            _RefreshInternationalLicensesList();
        }

        private void _RefreshInternationalLicensesList()
        {
            _dtAllInternationalLicenses = ClsInternationalLicenseBusiness.GetAllInternationalLicenses();
            dgvInternationalLicense.DataSource = _dtAllInternationalLicenses;
        }


        private void cbFilterBy_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            txtFilter.Text = "";

            if (cbFilterBy.Text == "None")
            {
                txtFilter.Visible = false;
                if (_dtAllInternationalLicenses != null)
                {
                    _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                }
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Focus();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilterBy.Text)
            {
                case "Int. License ID":
                    FilterColumn = "Int.LicenseID";
                    break;
                case "Application ID":
                    FilterColumn = "Application ID";
                    break;
                case "Driver ID":
                    FilterColumn = "Driver ID";
                    break;
                case "L.ID":
                    FilterColumn = "L.ID"; 
                    break;
                case "Is Active":
                    FilterColumn = "Is Active";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (FilterColumn == "None" || string.IsNullOrEmpty(txtFilter.Text.Trim()))
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                return;
            }

            if (FilterColumn == "Int.LicenseID" || FilterColumn == "Application ID" || FilterColumn == "Driver ID" || FilterColumn == "L.ID")
            {
                if (int.TryParse(txtFilter.Text.Trim(), out int IDValue))
                {
                    _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, IDValue);
                }
                else
                {
                    _dtAllInternationalLicenses.DefaultView.RowFilter = "1=0"; // Render empty grid if letters are typed
                }
            }
            else if (FilterColumn == "Is Active")
            {
                string filterText = txtFilter.Text.Trim().ToLower();

                if (filterText == "yes" || filterText == "true" || filterText == "1")
                {
                    _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = true", FilterColumn);
                }
                else if (filterText == "no" || filterText == "false" || filterText == "0")
                {
                    _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = false", FilterColumn);
                }
            }
            // Fallback String Rule
            else
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }



        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicense.CurrentRow == null) return;

            // Pull the DriverID from grid, then get PersonID
            int driverID = Convert.ToInt32(dgvInternationalLicense.CurrentRow.Cells["Driver ID"].Value);
            ClsDriverBusiness driver = ClsDriverBusiness.FindDriverByID(driverID);
            if (driver == null) return;
            
            int personID = driver.PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(personID);
            frm.ShowDialog();
            _RefreshInternationalLicensesList();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicense.CurrentRow == null) return;

            int selectedInternationalLicenseID = Convert.ToInt32(dgvInternationalLicense.CurrentRow.Cells["Int.LicenseID"].Value);

            // Opens the specialized international card viewer info form
            frmInternationalDrivingLicenseInfo frm = new frmInternationalDrivingLicenseInfo(selectedInternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicensHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInternationalLicense.CurrentRow == null) return;

            int driverID = Convert.ToInt32(dgvInternationalLicense.CurrentRow.Cells["Driver ID"].Value);
            ClsDriverBusiness driver = ClsDriverBusiness.FindDriverByID(driverID);
            if (driver == null) return;
            
            int personID = driver.PersonID;

            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(personID);
            frm.ShowDialog();
        }

        private void cmInternationalLicense_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();
            frm.ShowDialog();

            _RefreshInternationalLicensesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
