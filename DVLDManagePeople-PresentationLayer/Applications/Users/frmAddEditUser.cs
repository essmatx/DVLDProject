
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmAddEditUser : Form
    {
        private int _SelectedPersonID = -1;
        private int _UserID; 

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; 

      private ClsUserBuiness _User; 
        public frmAddEditUser()
        {
            InitializeComponent();
            this.Mode = enMode.AddNew; 
        }

        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            this.Mode = enMode.Update;
            _UserID = UserID; 
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.Clear();

            cbFilterBy.Items.Add("Person ID");

            cbFilterBy.Items.Add("National No.");

            cbFilterBy.SelectedIndex = 1;

            tabControl1.Appearance = TabAppearance.Buttons;

            tabControl1.ItemSize = new Size(0, 1);

            tabControl1.SizeMode = TabSizeMode.Fixed;

            btnNext.Enabled = false;

            if (this.Mode == enMode.AddNew)
            {
                lblFormTitle.Text = "Add New User";
                _User = new ClsUserBuiness();
                btnNext.Enabled = false;

                tabControl1.SelectedTab = tpPersonalInfo; 
            }

         

            else
            {
                lblFormTitle2.Text = "Update User Info";

                _User = ClsUserBuiness.FindUserByID(_UserID);


                if(_User == null)
                {
                    MessageBox.Show($"No user found with ID = {_UserID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                _SelectedPersonID = _User.PersonID;

                lblUserIDValue.Text = _User.UserID.ToString(); 
                txtUserName.Text = _User.UserName;
                
                chkIsActive.Checked = _User.IsActive;

                ucPersonInfo1.LoadPersonInfo(_User.PersonID);

                btnNext.Enabled = true; 

                tabControl1.SelectedTab = tpLoginInfo; 
            }
            
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";

            txtFilterValue.Focus(); 
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "Person ID")
            {
                if(!char.IsDigit(e.KeyChar)&& !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; 
                }
            }
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            string FilterValue = txtFilterValue.Text.Trim(); 

            if(string.IsNullOrEmpty(FilterValue))
            {
                MessageBox.Show("Please enter a search value.", "Blank Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            ClsPerson person; 

            if(cbFilterBy.Text == "Person ID")
            {
                int PersonID = Convert.ToInt32(FilterValue);

                person = ClsPerson.FindPersonByID(PersonID); 
            }
            else
            {
                person = ClsPerson.FindPersonByNationalNo(FilterValue); 
            }

            if(person != null)
            {
               if(ClsPerson.DoesUserExist(person.PersonID))
                {
                    MessageBox.Show("This Person is already assigend to a user account.Please choose a different Person."
                        , "Account already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _SelectedPersonID = -1;
                    btnNext.Enabled = false;
                    return;
                }

                _SelectedPersonID = person.PersonID;

                ucPersonInfo1.LoadPersonInfo(_SelectedPersonID);

                btnNext.Enabled = true; 
            }
            else
            {
                MessageBox.Show($"No person found with {cbFilterBy.Text} = {FilterValue}", 
                    "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _SelectedPersonID = -1;
                btnNext.Enabled = false; 
            }
        }

        private void FrmAddUpdatePerson_DataBack(object sender, int PersonID)
        {
            _SelectedPersonID = PersonID;

            ucPersonInfo1.LoadPersonInfo(_SelectedPersonID);

            btnNext.Enabled = true; 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           

           if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are invalid. please hover over the red icon to see the error.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return; 
            }

           if(this.Mode == enMode.AddNew)
            {
                if (_SelectedPersonID == -1)
                {
                    MessageBox.Show("Please select or add a valid person on the first bage be saving.", "Missing Person"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

            }



            _User.PersonID = _SelectedPersonID;

            _User.UserName = txtUserName.Text.Trim();

            _User.IsActive = chkIsActive.Checked;

            if(string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                _User.Password = clsUtil.ComputeHash(txtPassword.Text.Trim()); 
            }


            if(_User.Save())
            {
                MessageBox.Show("User Account created successfully in the system!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Mode = enMode.Update;

                lblFormTitle.Text = "Update User Info";

                lblUserIDValue.Text = _User.UserID.ToString(); 
            }
            else
            {
                MessageBox.Show("An unexpected database error occurred. Failed to save user account.", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            frm.DataBack += FrmAddUpdatePerson_DataBack;

            frm.ShowDialog(); 
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            string InputUsername = txtUserName.Text.Trim(); 

            if(string.IsNullOrEmpty(InputUsername))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtUserName, "Username cannot be left blank:");
                return; 
            }
            else
            {
                errorProvider1.SetError(txtUserName, ""); 
            }

            if(this.Mode == enMode.AddNew || (this.Mode == enMode.Update && InputUsername != _User.UserName))
            {
                if(ClsUserBuiness.DoesUserExists(InputUsername))
                {
                    e.Cancel = true;

                    errorProvider1.SetError(txtUserName, "This Username is already taken please choose another one");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, "");
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            string InputPassword = txtPassword.Text.Trim();

            if (this.Mode == enMode.AddNew && string.IsNullOrEmpty(InputPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password field cannot be left blank.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtConfirmation_Validating(object sender, CancelEventArgs e)
        {
            string InputConfirmationPassword = txtConfirmation.Text.Trim();
            string InputPassword = txtPassword.Text.Trim(); // Grab the password text too

            // 1. If updating and both are empty, that is fine! Skip the rest.
            if (this.Mode == enMode.Update && string.IsNullOrEmpty(InputPassword) && string.IsNullOrEmpty(InputConfirmationPassword))
            {
                errorProvider1.SetError(txtConfirmation, "");
                return;
            }

            // 2. Otherwise, make sure it isn't blank
            if (string.IsNullOrEmpty(InputConfirmationPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmation, "Confirmation Password cannot be left blank.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmation, "");
            }

            // 3. Make sure they match
            if (InputConfirmationPassword != InputPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmation, "Password confirmation does not match. Please re-enter your password.");
            }
            else
            {
                errorProvider1.SetError(txtConfirmation, "");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tpLoginInfo;
        }

        private void ucPersonInfo1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
