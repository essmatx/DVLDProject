using DVLD_BusinessWorkflows;
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
    public partial class frmLicenseReplacementForLostOrDamaged : Form
    {
        private int _OldLicneseID = -1;
        private int _NewLicenseID = -1;
        private int _ApplicationTypeID = 4;
        private ClsLicenseBusiness _OldLicense = null; 
        public frmLicenseReplacementForLostOrDamaged()
        {
            InitializeComponent();
        }

        private void UpdateReplacementMode()
        {
            if(rbDamagedLicense.Checked)
            {
                lblTitle.Text = "Replacement for Damaged License";
                this.Text = "Replacement for Damaged License";
                _ApplicationTypeID = 4; 
            }
            else
            {
                lblTitle.Text = "Replacement for Lost License";
                this.Text = "Replacement for Lost License";
                _ApplicationTypeID = 3; 
            }

            if(_OldLicense != null)
            {
                _LoadApplicationInfo();
            }
        }

        private void frmLicenseReplacementForLostOrDamaged_Load(object sender, EventArgs e)
        {
            ucLicenseSearchFilter1.OnSearchClick += ucLicenseSearchFilter1_OnSearchClick;

            btnIssueReplacement.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

           
            rbDamagedLicense.Checked = true;
            UpdateReplacementMode();
        }

        private void ucLicenseSearchFilter1_OnSearchClick(int incomingLicenseID)
        {
            _OldLicneseID = incomingLicenseID;

            ucLicenseInfo1.LoadLicenseInfo(_OldLicneseID);

            _OldLicense = ucLicenseInfo1.SelectedLicenseInfo; 

            if(_OldLicense == null)
            {
                _ResetFormState();
                return;
            }

            if(!_OldLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Active.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetFormState();
                return;
            }

            if(_OldLicense.IsExpired())
            {
                MessageBox.Show("Selected License is Expired. You should renew it instead of replacing it.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetFormState();
                return;
            }

            btnIssueReplacement.Enabled = true;
            llShowLicenseHistory.Enabled = true;
            _LoadApplicationInfo();
        }

        private void _ResetFormState()
        {
            _OldLicense = null;
            btnIssueReplacement.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }

        

        private void _LoadApplicationInfo()
        {

            ucApplicationInforLicenseReplacement1.LoadApplicationInfoForReplacement(_OldLicense, _ApplicationTypeID);
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No) return;

            ClsLicenseBusiness.enIssueReason issueReason = rbDamagedLicense.Checked
                ? ClsLicenseBusiness.enIssueReason.ReplacemnetDamged
                : ClsLicenseBusiness.enIssueReason.ReplacmentLost;

            ClsLicenseBusiness NewLicense = ClsLicenseReplacementWorkflow.ReplaceLicense(_OldLicneseID, issueReason, clsGlobal.CurrentUser.UserID); 

            if(NewLicense != null)
            {
                _NewLicenseID = NewLicense.LicenseID;

                MessageBox.Show($"License Replaced Successfully with ID = {_NewLicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssueReplacement.Enabled = false;
                llShowLicenseInfo.Enabled = true;
                ucLicenseSearchFilter1.Enabled = false;
                ucApplicationInforLicenseReplacement1.UpdateReplacementApplicationInfo(NewLicense.ApplicationID, _NewLicenseID);
            }
            else
            {
                MessageBox.Show("Failed to complete license replacement processing.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_OldLicense.DriverID);

          
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);

            frm.ShowDialog(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamagedLicense.Checked)
            {
                _ApplicationTypeID = 3; 
                UpdateReplacementMode();
            }
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLostLicense.Checked)
            {
                _ApplicationTypeID = 4;
                UpdateReplacementMode();
            }
        }
    }
}
