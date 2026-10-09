using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class ScheduleTest : Form
    {
        enum enMode { AddNewAppointment = 0 , UpdateAppointment = 1};
        enMode Mode = enMode.AddNewAppointment; 

        private int _TestTypeID = -1;

        private int _LocalDrivingLicenseApplicationID = -1;

        private int TestAppointmentID = -1; 

        public void LoadTestAppointmentDate()
        {

        }

        private ClsLocalDrivingLicenseAppBusiness LocallicenseApp;
        public ScheduleTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            _TestTypeID = TestTypeID;

            this.Mode = enMode.AddNewAppointment; 
        }

        private void LoadDefaultData()
        {
            LocallicenseApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByAppID(_LocalDrivingLicenseApplicationID); 

            if(LocallicenseApp == null)
            {
                MessageBox.Show("System Error; Application record not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return; 
            }

            lblDrivingLicenesAppID.Text = LocallicenseApp.LocalDrivingLicenseApplicationID.ToString();

            lblDrivinClass.Text = LocallicenseApp.LicenseClassInfo.ClassName;

            string FullName = LocallicenseApp.AppInfo.PersonInof.FirstName + "" + LocallicenseApp.AppInfo.PersonInof.SecondName + "" + LocallicenseApp.AppInfo.PersonInof.ThirdName
                + "" + LocallicenseApp.AppInfo.PersonInof.LastName;
            lblName.Text = $"{FullName}"; 

            int TrailCount =  ClsTestAppointmentBusiness.TotalTrialsPerTestType(_LocalDrivingLicenseApplicationID,_TestTypeID);
            lblTrail.Text = TrailCount.ToString();

            decimal StandardFees = 10.00m;

            lblFees.Text = StandardFees.ToString("F0"); 

            if(TrailCount > 0)
            {
                gbRetakeTestInfo.Enabled = true;

                ucRetakeTestInfo1.LoadRetakeTestInfo(_LocalDrivingLicenseApplicationID, StandardFees, TrailCount); 
            }
            else
            {
                ucRetakeTestInfo1.Reset(); 
            }
        }
        private void VisionTest_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

           ClsTestAppointmentBusiness appointment = new ClsTestAppointmentBusiness();

            appointment.TestTypeID = _TestTypeID;
            appointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
            appointment.AppointmentDate = dtAppointmentDate.Value;
            appointment.PaidFees = Convert.ToDecimal(lblFees.Text);
            appointment.CreatedByUserID = 1;
            appointment.IsLocked = false;

            if (appointment.Save())
            {
                MessageBox.Show("Appointment scheduled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Database Error: Could not save the appointment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
