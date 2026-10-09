using ClsPersonBusiness;
using DVLDCountry_BusinessTier;
using DVLDManagePeople_PresentationLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class UserControl1 : UserControl
    {

       private string _SelectedImagePath = ""; 
        public enum enMode { _AddNew = 0 , _Update = 1};
        enMode Mode = enMode._AddNew;

        private int _PersonID;
        private ClsPerson _person; 

        public void LoadData(int PersonID)
        {
           
            _person = ClsPerson.FindPersonByID(PersonID);

            if(_person == null)
            {
                MessageBox.Show("No Person with ID " + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            _SelectedImagePath = _person.ImagePath;

            this.Mode = enMode._Update;

            _PersonID = PersonID;

            txtFIrstName.Text = _person.FirstName;
             txtSecondName.Text = _person.SecondName;
            textthirdName.Text = _person.ThirdName;
            textLastName.Text = _person.LastName;
            txtNationalNo.Text = _person.NationalNo;
            txtEmail.Text = _person.Email;
            txtPhone.Text = _person.Phone;
            txtAddress.Text = _person.Address;
            dtpDateOfBirth.Value = _person.DateOfBirth;

            txtNationalNo.ReadOnly = true; 
            

            if (_person.Gendor == 0) rbMale.Checked = true; else rbFemale.Checked = true;

            cbCountry.SelectedIndex = cbCountry.FindString(ClsCountryBusiness.FindCountryByID(_person.NationalityCountryID).CountryName); 

            if (_person.ImagePath != "" && File.Exists(_person.ImagePath))
            {
                pbPersonImage.Load(_person.ImagePath);
            }
            else
            {
                string defaultImagepath = (_person.Gendor == 0) ?
                     @"C:\Users\Essmat Tarek\Downloads\windows10_people_free\WINDOWS10\people\png\72\person_man.png"
                   : @"C:\Users\Essmat Tarek\Downloads\windows10_people_free\WINDOWS10\people\png\72\person_woman.png";

                if(File.Exists(defaultImagepath))
                {
                    pbPersonImage.Image = System.Drawing.Image.FromFile(defaultImagepath);
                }
            }
        }

        public bool Save()
        {
           if( this.Mode == enMode._AddNew)
            {
                _person = new ClsPerson();
            } 

            _person.FirstName = txtFIrstName.Text; 
            _person.SecondName = txtSecondName.Text;
            _person.ThirdName = textthirdName.Text;
            _person.LastName = textLastName.Text;
            _person.NationalNo = txtNationalNo.Text;
            _person.Email = txtEmail.Text;
            _person.Phone = txtPhone.Text;
            _person.Address = txtAddress.Text;
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.Gendor = rbMale.Checked ? (short)0 : (short)1;

            _person.NationalityCountryID = (int)cbCountry.SelectedValue;

            _person.ImagePath = _SelectedImagePath;

            if (_person._Save())
            {
                MessageBox.Show("Data Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Mode = enMode._Update;

                return true;
            }
            else
            {
                MessageBox.Show("Error: Data Faild to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false; 
            }
        }
        private void __FillCountriesInComboBox()
        {
            DataTable dtCountries = ClsCountryBusiness.GetAllCountries();
            cbCountry.DataSource = dtCountries;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            __FillCountriesInComboBox();

            if(this.Mode == enMode._AddNew)
            {
                ResetDefaultValues(); 
            }

            if(this.Mode == enMode._Update)
            {
                LoadData(_PersonID); 
            }

            rbMale.Checked = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close(); 
        }

        public void ResetDefaultValues()
        {
            this.Mode = enMode._AddNew;

            _PersonID = -1;

            _person = new ClsPerson();

            txtFIrstName.Text = "";
            txtSecondName.Text = "";
            textthirdName.Text = "";
            textLastName.Text = "";
            txtNationalNo.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";

            rbMale.Checked = true;

            dtpDateOfBirth.Value = DateTime.Now.AddYears(-18);

            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");

            pbPersonImage.ImageLocation = null; 
        }

        private void linklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedImagePath = openFileDialog.FileName;
                    pbPersonImage.Load(_SelectedImagePath); 
                }
            }
        }

        private void txtNationalNo_TextChanged(object sender, CancelEventArgs e)
        {
           
            
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string enteredNationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (this.Mode == enMode._AddNew || _person.NationalNo != txtNationalNo.Text.Trim())
            {
                if (ClsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
                {
                    e.Cancel = true;

                    errorProvider1.SetError(txtNationalNo, "Wrong Number Please Try Again Again");
                }
                else
                {
                    errorProvider1.SetError(txtNationalNo, "");
                }
            }
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox temp = (TextBox)sender; 

            if(string.IsNullOrEmpty(temp.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(temp, null); 
            }
        }

        private void txtFIrstName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void textthirdName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void textLastName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }
    }
}
