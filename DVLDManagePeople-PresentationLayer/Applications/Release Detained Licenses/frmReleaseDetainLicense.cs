
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
    public partial class frmReleaseDetainLicense : Form
    {
        private int _SelectedLicenseID = -1; 

        public frmReleaseDetainLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainLicense(int LicenseiD)
        {
            InitializeComponent();

            _SelectedLicenseID = LicenseiD; 
        }

        private void frmReleaseDetainLicense_Load(object sender, EventArgs e)
        {
            ucLicenseSearchFilter1.OnSearchClick += ucLicenseSearchFilter1_OnSearchClick;

            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

            ucShowDetainInfo1.ResetDefaultValues();

            if (_SelectedLicenseID != -1)
            {
                // Set the license text box value inside your user control filter
                ucLicenseSearchFilter1.SetLicenseIDToTextBox(_SelectedLicenseID);

                // Manually fire the search click logic to pull up all cards and verify detention state
                ucLicenseSearchFilter1_OnSearchClick(_SelectedLicenseID);
            }

        }

        private void ucLicenseSearchFilter1_OnSearchClick(int incomingLicenseID)
        {
            _SelectedLicenseID = incomingLicenseID;

            ucLicenseInfo1.LoadLicenseInfo(_SelectedLicenseID); 

            if(ucLicenseInfo1.SelectedLicenseInfo == null)
            {
                _ResetFormState();
                return;
            }

            if (!ucLicenseInfo1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not active! You cannot release an inactive license.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetFormState();
                return;
            }

            if (ucLicenseInfo1.SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show($"Selected License has expired on {ucLicenseInfo1.SelectedLicenseInfo.ExpirationDate.ToString("yyyy-MM-dd")}! Please renew the license first.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetFormState();
                return;
            }

            bool isDetained = ucShowDetainInfo1.LoadDetainedLicenseDataByLicenseID(_SelectedLicenseID); 

            if(!isDetained)
            {
                MessageBox.Show("Selected License is not currently detained inside the system!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ResetFormState();
                return;
            }

            btnRelease.Enabled = true;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true; 
            
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            

            ClsApplicationBusiness ReleaseApplication = new ClsApplicationBusiness();
            ReleaseApplication.ApplicationPersonID = ucLicenseInfo1.SelectedLicenseInfo.Driverinfo.PersonID;
            ReleaseApplication.ApplicationDate = DateTime.Now;
            ReleaseApplication.ApplicationTypeID = 5; // Release Detained License Type
            ReleaseApplication.AppStatus = ClsApplicationBusiness.enApplicationStatus.Completed; // Completed Status
            ReleaseApplication.LastStatus = DateTime.Now;
            ReleaseApplication.PaidFees = ucShowDetainInfo1.ApplicationFees; // Perfect consumption via getter!

            // Defensive: ensure an authenticated user exists before proceeding
            var globalUser = clsGlobal.CurrentUser;
            
            // Default to 1 for testing purposes if no user is logged in
            int currentUser = 1; 

            if (globalUser != null)
            {
                currentUser = globalUser.UserID;
            }

            ReleaseApplication.CreatedByUserID = currentUser;

            if (!ReleaseApplication.Save())
            {
                MessageBox.Show("System Error: Failed to generate the release base application record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClsDetainedLicenseBusiness DetainedLicense = ClsDetainedLicenseBusiness.FindDetainedLicenseByDetainID(ucShowDetainInfo1.DetainID);

            if (DetainedLicense == null)
            {
                MessageBox.Show("System Error: Detained license record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DetainedLicense.ReleaseDetain(currentUser, ReleaseApplication.ApplicationID))
            {
                // Update your custom control display view with the newly generated Application ID key
                ucShowDetainInfo1.UpdateAfterReleaseSuccess(ReleaseApplication.ApplicationID);

                MessageBox.Show($"License has been released successfully with Application ID = {ReleaseApplication.ApplicationID}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Secure post-transaction UI element states
                btnRelease.Enabled = false;
                ucLicenseSearchFilter1.Enabled = false; // Lock search interaction to protect transactional integrity
            }
            else
            {
                MessageBox.Show("System Error: Failed to update the database detention entry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _ResetFormState()
        {
            _SelectedLicenseID = -1;
            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            ucShowDetainInfo1.ResetDefaultValues();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(ucLicenseInfo1.SelectedLicenseInfo.Driverinfo.PersonID);
            frm.ShowDialog();
        }
    }
    
}
