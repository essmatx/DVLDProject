
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessWorkflows;
using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmNewInternationalLicenseApp : Form
    {
        private int _SelectedLocalLicenseID = -1;
        private int _CreatedInternationalLicenseID = -1;
        private ClsLicenseBusiness _LocalLicense;

        public frmNewInternationalLicenseApp()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void frmNewInternationalLicenseApp_Load(object sender, EventArgs e)
        {
            ucLicenseSearchFilter1.OnSearchClick += ucLicenseSearchFilter1_OnSearchClick;

            ucInternationalDrivingLicenseAppInfo1.ResetDefaultValues();

            btnIssue.Enabled = false;
          

        }

      

        private void _ResetFormState()
        {
            _SelectedLocalLicenseID = -1;
            btnIssue.Enabled = false;
            ucInternationalDrivingLicenseAppInfo1.ResetDefaultValues();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue this International License?", "Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ClsApplicationBusiness Application = new ClsApplicationBusiness();

            Application.ApplicationPersonID = _LocalLicense.Driverinfo.PersonID; 
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = 6;
            Application.AppStatus = ClsApplicationBusiness.enApplicationStatus.Completed;
            Application.LastStatus = DateTime.Now;
            Application.PaidFees = 51;
            Application.CreatedByUserID = clsGlobal.CurrentUser.UserID; 

            if(!Application.Save())
            {
                MessageBox.Show("System Error: Failed to create the base license application infrastructure.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int CurrentUserID = clsGlobal.CurrentUser.UserID; 

            int Result = ClsInternationalLicenseWorkflow.IssueInternationalLicense(_LocalLicense.DriverID, Application.ApplicationID, CurrentUserID);

            switch (Result)
            {
                case -1:
                    MessageBox.Show("Error: Driver record not found in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case -2:
                    MessageBox.Show("Error: No active regular local license found for this driver.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case -3:
                    MessageBox.Show("Error: This driver already holds an active International License!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case -4:
                    MessageBox.Show("System Error: Failed to save the final international license card record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default: // SUCCESS: It returned the newly generated InternationalLicenseID!
                    MessageBox.Show($"International Driving License Issued Successfully!\n\nIL License ID: {Result}",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _CreatedInternationalLicenseID = Result;

                    // 🎯 Instantly update your custom summary user control labels!
                    ucInternationalDrivingLicenseAppInfo1.UpdateGeneratedIDs(Application.ApplicationID, Result);

                    // Secure UI states
                    btnIssue.Enabled = false;
                    ucLicenseSearchFilter1.Enabled = false;
                    
                    break;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void ucLicenseSearchFilter1_OnSearchClick(int incomingLicenseID)
        {
            ucLicenseInfo1.LoadLicenseInfo(incomingLicenseID);

            _LocalLicense = ClsLicenseBusiness.FindLicenseByID(incomingLicenseID); 

            if(_LocalLicense == null)
            {
                _ResetFormState();
                return; 
            }

            _SelectedLocalLicenseID = incomingLicenseID;

           

            if(!_LocalLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Active. Cannot issue an International License against an inactive document.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnIssue.Enabled = false;
                return;
            }

            if(_LocalLicense.LicenseClassID != 3)
            {
                MessageBox.Show("Selected License must be a Class 3 (Ordinary driving license). International licenses cannot be issued for this vehicle class.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnIssue.Enabled = false;
                return;
            }

            ClsInternationalLicenseBusiness ActiveInternationalLicense = ClsInternationalLicenseBusiness.FindActiveInternationalLicenseByDriverID(_LocalLicense.DriverID);

            if(ActiveInternationalLicense != null)
            {
                MessageBox.Show($"Person already has an active International License with ID = {ActiveInternationalLicense.InternationalLicenseID}!",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                _CreatedInternationalLicenseID = ActiveInternationalLicense.InternationalLicenseID;

               
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
            ucInternationalDrivingLicenseAppInfo1.LoadDefaultApplicationData(incomingLicenseID,clsGlobal.CurrentUser.UserName);
            

        }
        private void ucLicenseSearchFilter1_Load(object sender, EventArgs e)
        {

        }

        private void ucInternationalDrivingLicenseAppInfo1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            frmInternationalDrivingLicenseInfo frm = new frmInternationalDrivingLicenseInfo(_CreatedInternationalLicenseID);
            frm.ShowDialog();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(_LocalLicense.Driverinfo.PersonID);

            frm.ShowDialog();
        }
    }
}
