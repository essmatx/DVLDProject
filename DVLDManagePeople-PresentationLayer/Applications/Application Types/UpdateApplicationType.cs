
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
    public partial class frmUpdateApplicationType : Form
    {
        private int _ID = -1;

        private ClsApplicationTypeBusiness _AppTypes;
        public frmUpdateApplicationType(int ID)
        {
            InitializeComponent();

            _ID = ID;
        }

        private void UpdateApplicationType_Load(object sender, EventArgs e)
        {

            _AppTypes = ClsApplicationTypeBusiness.FindApplicationTypeByID(_ID);

            if (_AppTypes == null)
            {
                MessageBox.Show($"No Application type found with ID = {_ID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();

                return;
            }


            lblID.Text = _AppTypes.ApplicationTypeID.ToString();

            txtTitle.Text = _AppTypes.ApplicationTypeTitle;

            txtFees.Text = _AppTypes.ApplicationFees.ToString();
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            string TitleInput = txtTitle.Text.Trim();

            if (string.IsNullOrEmpty(TitleInput))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Application title cannot be left blank.");
            }
            else
            {
                errorProvider1.SetError(txtTitle, "");
            }
        }

        private void pictureBox2_Validating(object sender, CancelEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the validation error before saving.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _AppTypes.ApplicationTypeTitle = txtTitle.Text.Trim();

            _AppTypes.ApplicationFees = Convert.ToDecimal(txtFees.Text.Trim());

            if (_AppTypes.Save())
            {
                MessageBox.Show("Application Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("An unexpected occurred.Failed to update record.", "Errro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            string feesInput = txtFees.Text.Trim();

            if (string.IsNullOrEmpty(feesInput))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtFees, "Fees cannot be left blank.");

                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, "");
            }

            if (!decimal.TryParse(feesInput, out decimal result))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtFees, "Please enter a valid currency/numeric ammount");
            }
            else
            {
                errorProvider1.SetError(txtFees, "");
            }
        }
    }
}
