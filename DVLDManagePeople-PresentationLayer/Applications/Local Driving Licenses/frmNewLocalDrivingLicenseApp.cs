
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
    public partial class frmNewLocalDrivingLicenseApp : Form
    {
    
        enum enMode { AddNew = 0 ,Update = 1};
        enMode Mode = enMode.AddNew;
        private int _PersonID = -1;
        private string _NationalNo = "";

        private ClsLocalDrivingLicenseAppBusiness _LocalDrivingLicenseApp;

 

        public frmNewLocalDrivingLicenseApp()
        {
            InitializeComponent();
            this.Mode = enMode.AddNew; 
            
        }

        public frmNewLocalDrivingLicenseApp(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            this.Mode = enMode.Update;

            this.Text = "Update Local Driving License Application"; 
        }

        public frmNewLocalDrivingLicenseApp(string NationalNo)
        {
            InitializeComponent();

            _NationalNo = NationalNo;

            this.Mode = enMode.Update;
            this.Text = "Update Local Driving License Application";

            frmTitle.Text = "Update Local Driving License Application";
        }

        private void _ResetDefaultValues()
        {
            if (Mode == enMode.AddNew)
            {



                // 1. Set static labels
                lblApplicationID.Text = "N/A";
                lblAppDate.Text = DateTime.Now.ToShortDateString();

                // 2. Populate License Classes ComboBox from Business Tier
                DataTable dtLicenseClasses = ClsLicensClassBusiness.GetAallLicenseClass();
                cbLicenseClasses.DataSource = dtLicenseClasses;
                cbLicenseClasses.DisplayMember = "ClassName";
                cbLicenseClasses.ValueMember = "LicenseClassID";

                // Default to standard ordinary driving license class if items exist (usually Class 3)
                if (cbLicenseClasses.Items.Count > 0)
                    cbLicenseClasses.SelectedIndex = 2;

                // 3. Load Application Type Fees dynamically from DB (ApplicationTypeID = 1 for New Local License)
                // Assuming you have a Find method in your ApplicationTypes Business Layer
                decimal ApplicationFees = ClsApplicationTypeBusiness.FindApplicationTypeByID(1).ApplicationFees;
                lblAppFees.Text = ApplicationFees.ToString("0.00");

                // 4. Set the logged-in system user (Using your Global login session class)
                // e.g., lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
                lblCreatedBy.Text = "Admin";

                btnSave.Enabled = true;
            }
        }
        private void ucPersonInfo1_Load(object sender, EventArgs e)
        {
           
        }

       
        private void frmNewLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {

            cbFilter.Items.Add("National No");
            cbFilter.Items.Add("Person ID");
            cbFilter.SelectedIndex = 1;
            btnNext.Enabled = false;

            _ResetDefaultValues();

            if (_PersonID != -1)
            {
                cbFilter.SelectedIndex = 1; // Person ID
                txtValue.Text = _PersonID.ToString();
                _LoadPersonData(_PersonID);
            }
            else if (!string.IsNullOrEmpty(_NationalNo))
            {
                cbFilter.SelectedIndex = 0; // National No
                txtValue.Text = _NationalNo;
                _LoadPersonDataByNationalNo(_NationalNo);
            }

        }

        private void _LoadPersonData(int PersonID)
        {
            ClsPerson person = ClsPerson.FindPersonByID(PersonID);
            if (person != null)
            {
                _PersonID = person.PersonID;
                ucPersonInfo1.LoadPersonInfo(_PersonID);
                btnNext.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Person details could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _PersonID = -1;
                btnNext.Enabled = false;
                btnSave.Enabled = false;
            }
        }


        private void _LoadPersonDataByNationalNo(string NationalNo)
        {
            ClsPerson person = ClsPerson.FindPersonByNationalNo(NationalNo);
            if (person != null)
            {
                _PersonID = person.PersonID;
                ucPersonInfo1.LoadPersonInfo(_PersonID);
                btnNext.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Person details could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _PersonID = -1;
                btnNext.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string FilterValue = txtValue.Text.Trim();

            if (string.IsNullOrEmpty(FilterValue))
            {
                MessageBox.Show("Please enter a search value.", "Blank Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbFilter.Text == "Person ID")
            {
                if (int.TryParse(FilterValue, out int PersonID))
                {
                    _LoadPersonData(PersonID);
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric Person ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _LoadPersonDataByNationalNo(FilterValue);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_PersonID == - 1)
            {
                MessageBox.Show("Please select a valid person first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            tabControl1.SelectedTab = tabPage2; 

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(cbLicenseClasses.SelectedValue == null)
            {
                return; 
            }

            if (!int.TryParse(cbLicenseClasses.SelectedValue.ToString(), out int _SelectedLicenseClassID))
            {
                MessageBox.Show("Invalid License Class selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            

            if (_PersonID == -1)
            {
                MessageBox.Show("Please select or add a person first on the first tab.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ClsLocalDrivingLicenseAppBusiness.DoesPersonHaveActiveApplication(_PersonID, _SelectedLicenseClassID))
            {

                MessageBox.Show("This person already has an active application for this license class. Please select a different class.",
            "Application Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnNext.Enabled = false;
                return;
            }
            else
            {
                btnNext.Enabled = true;
            }


            ClsLocalDrivingLicenseAppBusiness NewApp = new ClsLocalDrivingLicenseAppBusiness();
            
           
            NewApp.LicenseClassID = _SelectedLicenseClassID;

            NewApp.AppInfo.ApplicationPersonID = _PersonID;
            NewApp.AppInfo.ApplicationDate = DateTime.Now;
            NewApp.AppInfo.ApplicationTypeID = 1;
            NewApp.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
            NewApp.AppInfo.LastStatus = DateTime.Now;
            NewApp.AppInfo.PaidFees = Convert.ToDecimal(lblAppFees.Text);
            NewApp.AppInfo.CreatedByUserID = 1;
            NewApp.LicenseClassID = _SelectedLicenseClassID;

            if (ClsLocalDrivingLicenseAppBusiness.DoesPersonHaveActiveApplication(NewApp.AppInfo.ApplicationPersonID,NewApp.LicenseClassID))
            {
                MessageBox.Show("This person already has an active application for this license class.",
            "Application Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewApp.Save())
           {
               lblApplicationID.Text = NewApp.LocalDrivingLicenseApplicationID.ToString();
          
               MessageBox.Show($"Application Saved Successfully wit ID = {NewApp.LocalDrivingLicenseApplicationID}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
          
               
           }
           else
           {
               MessageBox.Show("An error occurred while trying to save.", "Database Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }

            
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAdd = new frmAddUpdatePerson();
            frmAdd.DataBack += FrmAdd_Databack; 
            frmAdd.ShowDialog(); 
        }

        private void FrmAdd_Databack(object sender , int PersonID)
        {
            if(PersonID != -1)
            {
                _PersonID = PersonID;
                cbFilter.Text = "Person ID";
                txtValue.Text = PersonID.ToString(); 
                ucPersonInfo1.LoadPersonInfo(_PersonID);

                btnNext.Enabled = true; 
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbLicenseClasses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                // Safety Check: Don't let them view Page 2 info if they haven't selected a person
                if (_PersonID == -1)
                {
                    MessageBox.Show("Please select a valid person first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabControl1.SelectedIndex = 0; // Force them back to Tab 1
                    return;
                }

                // Load the Page 2 data now that the controls are guaranteed to exist!
                _LoadApplicationDataOnTab2();
            }
        }


        private void _LoadApplicationDataOnTab2()
        {
            // 1. Set application status labels
            lblApplicationID.Text = "N/A";
            lblAppDate.Text = DateTime.Now.ToShortDateString();

            // 2. Populate License Classes ComboBox
            DataTable dtLicenseClasses = ClsLicensClassBusiness.GetAallLicenseClass();
            cbLicenseClasses.DataSource = dtLicenseClasses;
            cbLicenseClasses.DisplayMember = "ClassName";
            cbLicenseClasses.ValueMember = "LicenseClassID";

            // Default to standard ordinary driving license class if items exist (usually Index 2 / Class 3)
            if (cbLicenseClasses.Items.Count > 0)
                cbLicenseClasses.SelectedIndex = 2;

            // 3. Load Application Type Fees dynamically from DB (ApplicationTypeID = 1)
            var appType = ClsApplicationTypeBusiness.FindApplicationTypeByID(1);
            if (appType != null)
            {
                lblAppFees.Text = appType.ApplicationFees.ToString("0.00");
            }
            else
            {
                lblAppFees.Text = "0.00";
                MessageBox.Show("Error loading application fees from database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 4. Set the logged-in system user (using static 'Admin' for now)
            lblCreatedBy.Text = "Admin";

            btnSave.Enabled = true;
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {

        }
    }
}
