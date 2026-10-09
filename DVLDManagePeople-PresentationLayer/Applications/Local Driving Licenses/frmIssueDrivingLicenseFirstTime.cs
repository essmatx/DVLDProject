using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessWorkflows;
using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;
namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private ClsLocalDrivingLicenseAppBusiness _LDLA_Application;
        public frmIssueDrivingLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID; 
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            string LicenseNotes = txtNotes.Text.Trim();

            if (string.IsNullOrEmpty(LicenseNotes))
            {
                LicenseNotes = "No Notes Provided";
            }

            int ApplicationID = _LocalDrivingLicenseApplicationID;
            int CreatedByUserID = clsGlobal.CurrentUser.UserID; 


            int NewLicenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(ApplicationID, LicenseNotes, CreatedByUserID);

            if(NewLicenseID > 0)
            {
                MessageBox.Show($"License Issued Successfully with ID = {NewLicenseID}!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnIssue.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to issue the license. Please verify application requirements.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmIssueDrivingLicenseFirstTime_Load(object sender, EventArgs e)
        {
            if (_LocalDrivingLicenseApplicationID == -1)
            {
                MessageBox.Show("Error: No valid Application ID passed to this screen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _LDLA_Application = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocalDrivingLicenseApplicationID); 

            if(_LDLA_Application == null)
            {
                MessageBox.Show($"Error: Application with ID = {_LocalDrivingLicenseApplicationID} does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ucDrivingLicenseAppInfo1._LoadDrvingLicenseAppInfo(_LocalDrivingLicenseApplicationID);

            ucApplicationBasicInfo1.LoadApplicantData(this._LocalDrivingLicenseApplicationID); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
