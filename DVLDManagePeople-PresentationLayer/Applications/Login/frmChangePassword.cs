
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
using DVLDManagePeople_PresentationLayer.Global_Classes;

namespace DVLDManagePeople_PresentationLayer
{ 
    public partial class frmChangePassword : Form
    {
        private int _UserID;

        

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _UserID = UserID; 
        }

        private void ucLoginInfo1_Load(object sender, EventArgs e)
        {
             
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ucLoginInfo1.LoadByUserID(_UserID); 

            if(ucLoginInfo1.SelectedUserInfo != null)
            {
                ucPersonInfo1.LoadPersonInfo(ucLoginInfo1.SelectedUserInfo.PersonID);
            }
            else
            {
                MessageBox.Show("Failed to load user information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
           
        }

        private void txtCurrentPass_Validating(object sender, CancelEventArgs e)
        {
            string CurrentPassInput = txtCurrentPass.Text.Trim(); 

            if(string.IsNullOrEmpty(CurrentPassInput))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPass, "Please enter your current passowrd");

                return; 
            }
            else
            {
                errorProvider1.SetError(txtCurrentPass, ""); 
            }

            if(CurrentPassInput != ucLoginInfo1.SelectedUserInfo.Password)
            {
                e.Cancel = true;

                errorProvider1.SetError(txtCurrentPass, "The current passowrd enterd is incorrect"); 
            }
            else
            {
                errorProvider1.SetError(txtCurrentPass, ""); 
            }
        }

        private void txtNewPass_Validating(object sender, CancelEventArgs e)
        {
            string NewPassInput = txtNewPass.Text.Trim(); 

            if(string.IsNullOrEmpty(NewPassInput))
            {
                e.Cancel = true; 
                errorProvider1.SetError(txtNewPass, "New password field cannot be left blank");
            }
            else
            {
                errorProvider1.SetError(txtNewPass, ""); 
            }

        }

        private void txtConfirmPass_Validating(object sender, CancelEventArgs e)
        {

            string ConfirmpassInput = txtConfirmPass.Text.Trim();

            string NewPassword = txtNewPass.Text.Trim();


            if (string.IsNullOrEmpty(ConfirmpassInput) && string.IsNullOrEmpty(NewPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPass, "");
                return;
            }
           
            if(string.IsNullOrEmpty(ConfirmpassInput))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPass, "Confirmation Password cannot be left blank.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPass, "");
            }

           
            if (ConfirmpassInput != NewPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPass, "Password confirmation does not match your new password.");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPass, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are invalid. Please hover over the red icons to see the errors.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            ClsUserBuiness User = ucLoginInfo1.SelectedUserInfo;

            if (string.IsNullOrEmpty(txtNewPass.Text.Trim()))
            {
                User.Password = clsUtil.ComputeHash(txtNewPass.Text.Trim()); 
            }

            if(User.Save())
            {
                MessageBox.Show("Password changed successfully in the system!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("An unexpected database error occurred. Failed to update password.", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnClsoe_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
