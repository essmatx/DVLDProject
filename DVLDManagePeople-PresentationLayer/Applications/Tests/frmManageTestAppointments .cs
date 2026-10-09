
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
    public partial class frmManageTestAppointments : Form
    {
        private int _LocaldrivinglicenseAppID = -1;

        private ClsTestTypeBusiness.enTestType _TestType;

        private ClsLocalDrivingLicenseAppBusiness _LocalAppInfo;

        public frmManageTestAppointments(int LocaldrivinglicenseAppID, ClsTestTypeBusiness.enTestType TestType)
        {
            InitializeComponent();

            _LocaldrivinglicenseAppID = LocaldrivinglicenseAppID;

            _TestType = TestType;
        }

        private void frmManageTestAppointments_Load(object sender, EventArgs e)
        {
            _ConfigureTestTypeUI();
            _LoadTestData();
        }


        private void _ConfigureTestTypeUI()
        {
            switch (_TestType)
            {
                case ClsTestTypeBusiness.enTestType.Vision:
                    lblTitle.Text = "Vision Test Appointments";
                    this.Text = "Vision Test Appointments";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Eye; 
                    break;

                case ClsTestTypeBusiness.enTestType.Written:
                    lblTitle.Text = "Written Test Appointments";
                    this.Text = "Written Test Appointments";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Pen; 
                    break;

                case ClsTestTypeBusiness.enTestType.Practical:
                    lblTitle.Text = "Street Test Appointments";
                    this.Text = "Street Test Appointments";
                    picShield.IconChar = FontAwesome.Sharp.IconChar.Car;
                    break;
            }
        }


        private void _LoadTestData()
        {
            _LocalAppInfo = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocaldrivinglicenseAppID);

            if (_LocalAppInfo == null)
            {
                MessageBox.Show("Error: Application data not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ucDrivingLicenseAppInfo1._LoadDrvingLicenseAppInfo(_LocalAppInfo.LocalDrivingLicenseApplicationID);

            ucApplicationBasicInfo1.LoadApplicantData(_LocalAppInfo.LocalDrivingLicenseApplicationID);

            dgvAppointments.DataSource = ClsTestAppointmentBusiness.GetApplicationAppointmentsPerTestType(_LocaldrivinglicenseAppID, (int)_TestType);


        }
        private void button2_Click(object sender, EventArgs e)
        {
            frmScheduleTest frm = new frmScheduleTest(_LocaldrivinglicenseAppID, _TestType, -1);

            frm.ShowDialog(); 
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvAppointments.CurrentRow == null) return;

            // 2. Extract the exact TestAppointmentID from the first column of the selected row 🎯
            int appointmentID = (int)dgvAppointments.CurrentRow.Cells["TestAppointmentID"].Value;

            // 3. Pass the specific Appointment ID and your unmodified TestType enum straight through
            frmTakeTest frm = new frmTakeTest(appointmentID, _TestType);
            frm.ShowDialog();

            // 4. Refresh the appointments grid immediately to reflect if the slot became locked (IsLocked = 1)
            dgvAppointments.DataSource = ClsTestAppointmentBusiness.GetApplicationAppointmentsPerTestType(_LocaldrivinglicenseAppID, (int)_TestType);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null) return;

            // 2. Extract the unique TestAppointmentID from the selected row 🎯
            // Note: If you haven't explicitly named your grid column keys, use index fallback: dgvAppointments.CurrentRow.Cells[0].Value
            int appointmentID = (int)dgvAppointments.CurrentRow.Cells["TestAppointmentID"].Value;

            // 3. Open the scheduling form passing the real appointment ID to tell it to load in EDIT mode
            frmScheduleTest frm = new frmScheduleTest(_LocaldrivinglicenseAppID, _TestType, appointmentID);
            frm.ShowDialog();

            // 4. Refresh: Instantly pull the updated date/time back into your data grid view 🔄
            dgvAppointments.DataSource = ClsTestAppointmentBusiness.GetApplicationAppointmentsPerTestType(_LocaldrivinglicenseAppID, (int)_TestType);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
