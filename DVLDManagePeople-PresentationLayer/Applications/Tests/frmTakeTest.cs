
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
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID = 1;

        private ClsTestAppointmentBusiness _AppointmentInfo;

        private ClsLocalDrivingLicenseAppBusiness _LocalAppInfo;

        private ClsTestTypeBusiness.enTestType _TestType;
        public frmTakeTest(int TestAppointmenID, ClsTestTypeBusiness.enTestType TestType)
        {
            InitializeComponent();

            _TestAppointmentID = TestAppointmenID;

            _TestType = TestType;
        }

        private void _ConfigureTestTypeUI()
        {
            switch (_TestType)
            {
                case ClsTestTypeBusiness.enTestType.Vision:
                    lblTestType.Text = "Vision Test";
                    picShiel.IconChar = FontAwesome.Sharp.IconChar.Eye;
                    break;

                case ClsTestTypeBusiness.enTestType.Written:
                    lblTestType.Text = "Written Test";
                    picShiel.IconChar = FontAwesome.Sharp.IconChar.Pen;
                    break;

                case ClsTestTypeBusiness.enTestType.Practical:
                    lblTestType.Text = "Street / Practical Test";
                    picShiel.IconChar = FontAwesome.Sharp.IconChar.Car;
                    break;
            }
        }

        private void _LoadTestData()
        {
            _AppointmentInfo = ClsTestAppointmentBusiness.FindTestAppointByID(_TestAppointmentID);

            if (_AppointmentInfo == null)
            {
                MessageBox.Show("Error: Appontment data not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }



            lblDrivingLicenesAppID.Text = _AppointmentInfo.LocalDrivingLicenseApplicationID.ToString();

            lblFees.Text = _AppointmentInfo.PaidFees.ToString("F0");

            lblDate.Text = _AppointmentInfo.AppointmentDate.ToString("dd/MM/yyyy");




            _LocalAppInfo = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_AppointmentInfo.LocalDrivingLicenseApplicationID);


            if (_LocalAppInfo == null)
            {
                MessageBox.Show("System Error: Local application details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }



            lblDrivinClass.Text = _LocalAppInfo.LicenseClassInfo.ClassName;

            lblName.Text = _LocalAppInfo.AppInfo.PersonInof.FullName;

            lblTrail.Text = ClsTestAppointmentBusiness.TotalTrialsPerTestType(_LocalAppInfo.LocalDrivingLicenseApplicationID, (int)_TestType).ToString();


        }

        private void TakeTest_Load(object sender, EventArgs e)
        {
            _LoadTestData();

            _ConfigureTestTypeUI();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save this test result? Once saved, it cannot be modified.",
                        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            ClsTestBuisness test = new ClsTestBuisness();

            test.TestAppointmentID = _TestAppointmentID;

            test.TestResult = rbPass.Checked;

            test.Notes = textNotes.Text;

            test.CreatedByUserID = 1;

            if (rbPass.Checked == true)
            {
                test.TestResult = true;
            }
            else if (rbFail.Checked == true)
            {
                test.TestResult = false;
            }

            if (test.Save())
            {
                lblTestID.Text = test.TestID.ToString();

                btnSave.Enabled = false;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
                textNotes.Enabled = false;

                if (test.TestResult == true)
                {
                    MessageBox.Show("Test saved as PASSED!\n\nThe appointment is now locked. The applicant can now proceed to the next stage.",
                                  "Test Result: Pass", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (test.TestResult == false)
                {
                    MessageBox.Show("Test saved as FAILED.\n\nThe appointment is now locked. The applicant must schedule a Retake Test appointment to attempt this test type again.",
                         "Test Result: Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Database Error: Could not save the test result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbTestIcon_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
