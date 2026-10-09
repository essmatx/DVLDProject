using DVLD_Business;
using DVLD_BusinessWorkflows;
using DVLDManagePeople_PresentationLayer.Global_Classes;
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

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmReNewLocalDrivingLicense : Form
    {
        private int _OldLicenseID = -1;
        private int _NewLicenseID = -1;

        private ClsLicenseBusiness _OldLicense; 
        public frmReNewLocalDrivingLicense()
        {
            InitializeComponent();
        }

        private void frmReNewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            ucLicenseSearchFilter1.OnSearchClick += ucLicenseSearchFilter1_OnSearchClick;
            btnRenew.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowNewLicenseInfo.Enabled = false;

           
        }

        private void ucLicenseSearchFilter1_OnSearchClick(int incomingLicenseID)
        {
            _OldLicenseID = incomingLicenseID;

            ucLicenseInfo1.LoadLicenseInfo(_OldLicenseID); 

            if(ucLicenseInfo1.SelectedLicenseInfo == null)
            {
                _ResetFormState();
                return; 
            }

            if(ClsDetainedLicenseBusiness.IsLicenseDetaind(_OldLicenseID))
            {
                MessageBox.Show("This license is currently detained! You must release the detention and settle penalties before it can be renewed.",
                                "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                _ResetFormState();
                return;
            }

            bool isReadyForRenewal = ucRenewLocalDrivingLicense1.LoadRenewalPreviewData(_OldLicenseID);

            if (isReadyForRenewal)
            {
                // assign only when preview indicates readiness
                _OldLicense = ucLicenseInfo1.SelectedLicenseInfo;

                btnRenew.Enabled = true;
                llShowLicenseHistory.Enabled = true;
            }
            else
            {
                _OldLicense = null;

                btnRenew.Enabled = false;

                llShowLicenseHistory.Enabled = false;
            }
        }
       


        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew this license?", "Confirm Renewal",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (_OldLicense == null || !_OldLicense.IsActive)
            {
                MessageBox.Show("No active license loaded. Please search and select an active license before renewing.",
                                "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnRenew.Enabled = false;
                return;
            }

            if (_OldLicense.ApplicationID <= 0 || _OldLicense.CreatedByUserID <= 0)
            {
                MessageBox.Show("License data incomplete: missing application or creator information.",
                                "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int result = ClsLicenseRenewalWorkflow.RenewLicense(
                _OldLicenseID,
                _OldLicense.ApplicationID,
                ucRenewLocalDrivingLicense1.GetNotes(),
                clsGlobal.CurrentUser.UserID);

            if (result <= 0)
            {
                string msg;
                switch (result)
                {
                    case -1:
                        msg = "Selected license not found or not active.";
                        break;
                    case -2:
                        msg = "License class rules are missing. Contact support.";
                        break;
                    case -3:
                        msg = "Failed to deactivate the old license. Please try again.";
                        break;
                    case -4:
                        msg = "Failed to save new license; the operation was rolled back.";
                        break;
                    default:
                        msg = "Unexpected error during renewal.";
                        break;
                }

                MessageBox.Show(msg, "Renewal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Refresh the currently loaded license state to avoid stale data
                ucLicenseInfo1.LoadLicenseInfo(_OldLicenseID);
                _OldLicense = ClsLicenseBusiness.FindLicenseByID(_OldLicenseID);
                btnRenew.Enabled = false;
                return;
            }

            _NewLicenseID = result;

            MessageBox.Show($"License Renewed Successfully! New License ID is: {_NewLicenseID}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClsLicenseBusiness newLicenseRecord = ClsLicenseBusiness.FindLicenseByID(_NewLicenseID);

            if (newLicenseRecord != null)
            {
               
                ucRenewLocalDrivingLicense1.InitializePostRenewalData(newLicenseRecord.ApplicationID, _NewLicenseID);
            }

            ucLicenseInfo1.LoadLicenseInfo(_NewLicenseID);

            btnRenew.Enabled = false;
            ucLicenseSearchFilter1.Enabled = false;
            llShowNewLicenseInfo.Enabled = true;
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(ucLicenseInfo1.SelectedLicenseInfo != null)
            {
                ClsDriverBusiness driver = ClsDriverBusiness.FindDriverByID(ucLicenseInfo1.SelectedLicenseInfo.DriverID);

                frmPersonLicenseHistory frm = new frmPersonLicenseHistory(driver.PersonID);
                frm.ShowDialog();
            }
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_NewLicenseID != -1)
            {
                frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
                frm.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void _ResetFormState()
        {
            _OldLicense = null;

            
            ucRenewLocalDrivingLicense1.ResetControlValues();

        
            btnRenew.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowNewLicenseInfo.Enabled = false;
        }

        private void ucLicenseSearchFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}
