
using DVLDManagePeople_PresentationLayer.Properties;
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
    public partial class frmScheduleTest : Form
    {

        private int _LocalDrivingLicenseAppID = -1;

        private ClsTestTypeBusiness.enTestType _TestType = ClsTestTypeBusiness.enTestType.Vision;

        private int _TestappointmentID = -1; 



        public frmScheduleTest(int LocalDrivingLicenseAppID, ClsTestTypeBusiness.enTestType TestType)
        {
            InitializeComponent();

            _LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;

            _TestType = TestType;

            _TestappointmentID = -1; 
        }

        public frmScheduleTest(int LocalDrivingLicenseAppID, ClsTestTypeBusiness.enTestType TestType,int TestappointmentID)
        {
            InitializeComponent();

            _LocalDrivingLicenseAppID = LocalDrivingLicenseAppID;

            _TestType = TestType;

            _TestappointmentID = TestappointmentID; 
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            _LoadScheduleData(); 
        }

        private void _ConfigureTestTypeUI()
        {
            switch(_TestType)
            {
                case ClsTestTypeBusiness.enTestType.Vision:
                    gbTestType.Text = "Vision Test";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Eye;
                    break;

                case ClsTestTypeBusiness.enTestType.Written:
                    gbTestType.Text = "Written Test";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Pen;
                    break;

                case ClsTestTypeBusiness.enTestType.Practical:
                    gbTestType.Text = "Street Test";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Car;
                    break;
            }
        }

        private void _LoadScheduleData()
        {
            _ConfigureTestTypeUI();

            lblDrivingLicenesAppID.Text = _LocalDrivingLicenseAppID.ToString();

            var TestTypeInfo = ClsTestTypeBusiness.FindTestTypeByID(_TestType);

            if (TestTypeInfo == null)
            {
                MessageBox.Show("System Error: Test type details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblFees.Text = TestTypeInfo.TestTypeFees.ToString();

            var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocalDrivingLicenseAppID);
            if (localApp == null)
            {
                MessageBox.Show("System Error: Local application details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblDrivinClass.Text = localApp.LicenseClassInfo.ClassName;

            lblName.Text = localApp.AppInfo.PersonInof.FullName;

            if (_TestappointmentID != -1)
            {
                gbTestType.Text = $"{gbTestType.Text} (Reschedule Mode)";

                var existingAppointment = ClsTestAppointmentBusiness.FindTestAppointByID(_TestappointmentID);
                if (existingAppointment != null)
                {
                    if (existingAppointment.AppointmentDate < DateTime.Now)
                    {
                        dtAppointmentDate.MinDate = existingAppointment.AppointmentDate;
                    }
                    else
                    {
                        dtAppointmentDate.MinDate = DateTime.Now;
                    }

                    dtAppointmentDate.Value = existingAppointment.AppointmentDate;
                }

                gbRetakeTestInfo.Visible = false;
                ucRetakeTestInfo1.Visible = false;
                gbRetakeTestInfo.Name = ""; 
                return;
            }


            dtAppointmentDate.MinDate = DateTime.Now;

            int trialCount = ClsTestAppointmentBusiness.TotalTrialsPerTestType(_LocalDrivingLicenseAppID, (int)_TestType);
            lblTrail.Text = trialCount.ToString();
            if (trialCount > 0)
            {
                gbTestType.Text = $"{gbTestType.Text} (Retake Test)";

                lblTitle.Text = " Schedule Retake Test";

                ucRetakeTestInfo1.Visible = true;
                gbRetakeTestInfo.Text = "Retake Test Info"; 

                if (!ucRetakeTestInfo1.LoadRetakeTestInfo(
                        _LocalDrivingLicenseAppID,
                        TestTypeInfo.TestTypeFees,
                        trialCount))
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                ucRetakeTestInfo1.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to schedule this test appointment?",
                        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ClsTestAppointmentBusiness appointment;

            //if(_TestappointmentID != -1)
            //{
            //    appointment = ClsTestAppointmentBusiness.FindTestAppointByID(_TestappointmentID); 
            //
            //    if(appointment == null)
            //    {
            //        MessageBox.Show("System Error: Could not locate target appointment entity record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}


            if (_TestappointmentID != -1)
            {
                // Use our laser-targeted helper method! 🎯
                if (ClsTestAppointmentBusiness.ModifyAppointmentDate(_TestappointmentID, dtAppointmentDate.Value))
                {
                    MessageBox.Show("Test Appointment date has been modified successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: Could not update the appointment date.\n\nHint: The appointment might be locked or no longer exists.",
                                    "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                appointment = new ClsTestAppointmentBusiness();

                appointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseAppID;
                appointment.TestTypeID = (int)_TestType;
                appointment.PaidFees = Convert.ToDecimal(lblFees.Text);
                appointment.CreatedByUserID = 1;
                appointment.IsLocked = false;

                // Check for an existing unlocked (active) appointment before trying to save.
                // This can happen when a previous scheduling attempt left an appointment open.
                if (ClsTestAppointmentBusiness.IsAnyActiveAppointment(_LocalDrivingLicenseAppID, (int)_TestType))
                {
                    MessageBox.Show(
                        "Cannot schedule a new appointment.\n\nThis application already has an active (unlocked) appointment for this test type.\n\nPlease lock or remove the existing appointment before scheduling again.",
                        "Active Appointment Exists",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                int trialCount = ClsTestAppointmentBusiness.TotalTrialsPerTestType(_LocalDrivingLicenseAppID, (int)_TestType);

                if (trialCount > 0)
                {
                    if (!ucRetakeTestInfo1.CreateRetakeApplication(_LocalDrivingLicenseAppID, 1))
                    {
                        return;
                    }
                }

                appointment.AppointmentDate = dtAppointmentDate.Value;

                if (appointment.Save())
                {
                    MessageBox.Show("Test Appointment has been scheduled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    dtAppointmentDate.Enabled = false;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("System Error: Failed to insert the test appointment into the database.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

           

           

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
