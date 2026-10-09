
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
    public partial class frmDetainLicensApp : Form
    {
        private int _SelectedLicenseID = -1;

        private ClsLicenseBusiness _SelectedLicense;

        private int _DetainID = -1; 


        public frmDetainLicensApp()
        {
            InitializeComponent();
        }

        private void frmDetainLicensApp_Load(object sender, EventArgs e)
        {
            ucLicenseSearchFilter1.OnSearchClick += ucLicenseSearchFilter1_OnSearchClick;

            _ResetFormState();
        }

        private void ucLicenseSearchFilter1_OnSearchClick(int incomingLicenseID)
        {
            _SelectedLicenseID = incomingLicenseID;

            ucLicenseInfo1.LoadLicenseInfo(_SelectedLicenseID);

            _SelectedLicense = ucLicenseInfo1.SelectedLicenseInfo;

            if (_SelectedLicense == null)
            {
                _ResetFormState();
                return;
            }

            if(ucLicenseInfo1.IsDetained)
            {
                MessageBox.Show("Selected License is already Detained!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ResetFormState();
                return;
            }

            if(!_SelectedLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Active. Cannot detain an inactive document.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ResetFormState();
                return;
            }

            ucDetainLicenseInfo1.LoadDefaultDetainData(_SelectedLicenseID, clsGlobal.CurrentUser.UserName);

            btnDetain.Enabled = true;
            llShowLicenseHistory.Enabled = true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            decimal appliedFine = ucDetainLicenseInfo1.FineFees; 

            if(appliedFine <= 0 )
            {
                MessageBox.Show("Please enter a valid fine fee amount inside the Detain Info panel.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ClsDetainedLicenseBusiness DetainedLicense = new ClsDetainedLicenseBusiness();
            DetainedLicense.LicenseID = _SelectedLicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = appliedFine;
            DetainedLicense.CreatedByUserID = 1; 
            DetainedLicense.IsReleased = false;

            if(DetainedLicense.Save())
            {
                ucDetainLicenseInfo1.UpdateAfterDetainSuccess(DetainedLicense.DetainID);

                _DetainID = ucDetainLicenseInfo1.DetainID;

                MessageBox.Show($"License Detained Successfully with ID = {_DetainID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnDetain.Enabled = false;
                ucLicenseSearchFilter1.Enabled = false; // Lock filter interaction
                llShowLicenseinfo.Enabled = true;

            }
            else
            {
                MessageBox.Show("System Error: Failed to execute database detention transaction row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void _ResetFormState()
        {
            _SelectedLicense = null;
            _SelectedLicenseID = -1;
            _DetainID = -1;

            btnDetain.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseinfo.Enabled = false;

            ucDetainLicenseInfo1.ResetDefaultValues();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(_SelectedLicense.Driverinfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseinfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ucLicenseSearchFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}
