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
    public partial class frmPersonLicenseHistory : Form
    {
        private int _PersonID = -1;

        public frmPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        private void frmPersonLicenseHistory_Load(object sender, EventArgs e)
        {


            

            cbFilterBy.Items.Add("Person ID");
            tabPage1.Text = "Local";
            tabPage2.Text = "International";

            if (_PersonID != -1)
            {
                txtFilterValue.Text = _PersonID.ToString();
                _LoadPersonLicenseHistoryData(_PersonID);
            }
            else
            {
                txtFilterValue.Focus();
            }
            cbFilterBy.SelectedIndex = 0;


        }

        private void _LoadPersonLicenseHistoryData(int PersonID)
        {
            ucPersonInfo1.LoadPersonInfo(PersonID);

            DataTable dtLocal = ClsPerson.GetPersonLocalLicensesHistory(PersonID);
            dgvLocalLicensesHistory.DataSource = dtLocal;


            DataTable dtInternational = ClsPerson.GetPersonInternationalLicensesHistory(PersonID);
            dgvInternationalLicensesHistory.DataSource = dtInternational;
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string FilterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(FilterValue))
            {
                MessageBox.Show("Please type a search query value first.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int FoundPersonID = -1;

            if (cbFilterBy.Text == "Person ID")
            {
                if (!int.TryParse(FilterValue, out FoundPersonID))
                {
                    MessageBox.Show("Person ID must be a valid numeric integer value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!ClsPerson.DoesPersonExist(FoundPersonID))
            {
                MessageBox.Show("No person record matches this ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ClearHistoryDataViews();
                return;
            }

            _PersonID = FoundPersonID;
            _LoadPersonLicenseHistoryData(_PersonID);

        }


        private void _ClearHistoryDataViews()
        {
           

            dgvLocalLicensesHistory.DataSource = null;
            dgvInternationalLicensesHistory.DataSource = null;

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ucPersonInfo1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}

