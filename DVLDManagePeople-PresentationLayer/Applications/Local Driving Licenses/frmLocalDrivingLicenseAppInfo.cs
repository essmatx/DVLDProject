
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
    public partial class frmLocalDrivingLicenseAppInfo : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1; 
        public frmLocalDrivingLicenseAppInfo(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

        }

        private void LoadApplicationData()
        {
            ClsLocalDrivingLicenseAppBusiness LocalDrivingLicenseApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApp == null)
            {
                MessageBox.Show("Application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ucApplicationBasicInfo1.LoadApplicantData(LocalDrivingLicenseApp.LocalDrivingLicenseApplicationID);
            ucPersonInfo1.LoadPersonInfo(LocalDrivingLicenseApp.AppInfo.ApplicationPersonID);
        }
        private void frmLocalDrivingLicenseAppInfo_Load(object sender, EventArgs e)
        {
            LoadApplicationData();
        }

        private void ucApplicationBasicInfo1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
