
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
    public partial class frmUpdateTestType : Form
    {
        private int _ID = -1;
        private ClsTestTypeBusiness _TestTypes; 
        public frmUpdateTestType(int ID)
        {
            InitializeComponent();

            _ID = ID; 
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestTypes = ClsTestTypeBusiness.FindTestTypeByID((ClsTestTypeBusiness.enTestType)_ID);

            if (_TestTypes == null)
            {
                MessageBox.Show($"No test type found with ID = {_ID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return; 
            }


            lblID.Text = _TestTypes.TestTypeID.ToString();

            txtTitle.Text = _TestTypes.TestTypeTitle;

            txtDescription.Text = _TestTypes.TestTypeDescription;

            txtFees.Text = _TestTypes.TestTypeFees.ToString(); 
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            string TitleInput = txtTitle.Text.Trim(); 

            if(string.IsNullOrEmpty(TitleInput))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtTitle, "Test title cannot be left blank.");
                return; 
            }
            else
            {
                errorProvider1.SetError(txtTitle, ""); 
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            string DescriptionInput = txtDescription.Text.Trim(); 

            if(string.IsNullOrEmpty(DescriptionInput))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtDescription, "Test Description cannot be left blank.");
                return; 
            }
            else
            {
                errorProvider1.SetError(txtDescription, ""); 
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            string FeesInput = txtFees.Text.Trim();

            if (string.IsNullOrEmpty(FeesInput))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtFees, "Test fees cannot be left blank.");

                return; 
            }
            else
            {
                errorProvider1.SetError(txtFees, ""); 
            }


            if(!decimal.TryParse(FeesInput,out decimal Result))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Please enter a valid currency/numeric ammount."); 
            }
            else
            {
                errorProvider1.SetError(txtFees, ""); 
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the validation error before saving", "Validation Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);

                return; 
            }

            _TestTypes.TestTypeTitle = txtTitle.Text.Trim();

            _TestTypes.TestTypeDescription = txtDescription.Text.Trim();

            _TestTypes.TestTypeFees = Convert.ToDecimal(txtFees.Text.Trim()); 

            if(_TestTypes.Save())
            {
                MessageBox.Show("Test Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("An unexpected occured failed to update record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
    }
}
