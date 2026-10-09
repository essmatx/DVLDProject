
using DVLD_Business; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmLocalDrivingLicenseApp : Form
    {
        private DataTable _dataTable;

        private ClsApplicationTypeBusiness _AppTypes; 



        private int _LocalDrivingLicenseApplicationID = -1;
        private int _GetSelectedLocalDrivingLicenseApplicationID()
        {
            if (dgvLocalDrivingLicenseApps.CurrentRow != null)
            {
                return (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;
            }

            return -1;
        }
        private void _RefreshList()
        {
            _dataTable = ClsLocalDrivingLicenseAppBusiness.GetAllLocalDrivingLicenseApplications();

            dgvLocalDrivingLicenseApps.DataSource = _dataTable;
        }
        public frmLocalDrivingLicenseApp()
        {

            InitializeComponent();


        }


        private void frmLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
           
            _RefreshList();

            cbFilter.Items.Add("LDLAPPID");
            cbFilter.Items.Add("Driving Class");
            cbFilter.Items.Add("Status"); 

           
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplication = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;

            string ApplicationStatus = dgvLocalDrivingLicenseApps.CurrentRow.Cells["Status"].Value.ToString();


            var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplication); 
            
            if(localApp == null)
            {
                return; 
            }

            bool PassedVision = ClsTestAppointmentBusiness.DoesPassTestType(localApp.LocalDrivingLicenseApplicationID, 1);

            bool PassedWritten = ClsTestAppointmentBusiness.DoesPassTestType(localApp.LocalDrivingLicenseApplicationID, 2);

            bool PassedStreet = ClsTestAppointmentBusiness.DoesPassTestType(localApp.LocalDrivingLicenseApplicationID, 3);

            bool passedAllTests = PassedVision && PassedWritten && PassedStreet;



            if (passedAllTests)
           {
                sechduleVisionTestToolStripMenuItem.Enabled = false;
                sechduleWriteTestToolStripMenuItem.Enabled = false;
                sechduleStreetTestToolStripMenuItem.Enabled  = false;
                
           }
           else if(ApplicationStatus == "Canceled" || ApplicationStatus == "Completed")
           {
                sechduleVisionTestToolStripMenuItem.Enabled = false;
                sechduleWriteTestToolStripMenuItem.Enabled = false;
                sechduleStreetTestToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = true;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = true; 
                return;
           }
           else
           {
                sechduleVisionTestToolStripMenuItem.Enabled = !PassedVision;

                sechduleWriteTestToolStripMenuItem.Enabled = PassedVision && !PassedWritten;

                sechduleStreetTestToolStripMenuItem.Enabled = PassedWritten && !PassedStreet;
                showLicenseToolStripMenuItem.Enabled = false;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = false;

            }

            bool isLicenseAlreadyIssued = ClsLicenseBusiness.DoesLicenseExistForApplication(localApp.ApplicationID);

            if (passedAllTests && !isLicenseAlreadyIssued && ApplicationStatus == "New")
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            }
            else
            {
                
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            }

            if(isLicenseAlreadyIssued == true)
            {
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                canceleToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showAppicationDetailsToolStripMenuItem.Enabled = false;
                sechdulToolStripMenuItem.Enabled = false;


                showLicenseToolStripMenuItem.Enabled = true;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = true; 

            }
            else
            {
                editApplicationToolStripMenuItem.Enabled = true;
                deleteApplicationToolStripMenuItem.Enabled = true;
                canceleToolStripMenuItem.Enabled = true;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                showAppicationDetailsToolStripMenuItem.Enabled = true;
                sechdulToolStripMenuItem.Enabled = true;
            }



            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void showAppicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = _GetSelectedLocalDrivingLicenseApplicationID();

            if (LocalDrivingLicenseApplicationID == -1) return;

            frmLocalDrivingLicenseAppInfo frmDrivingLicenseAppInfo = new frmLocalDrivingLicenseAppInfo(LocalDrivingLicenseApplicationID);

            

            frmDrivingLicenseAppInfo.ShowDialog();

            _RefreshList();
        }

        private void canceleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = _GetSelectedLocalDrivingLicenseApplicationID();
            if (LocalDrivingLicenseApplicationID == -1) return;


            if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ClsLocalDrivingLicenseAppBusiness licenseAppBusiness = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);

            if (licenseAppBusiness == null)
            {
                MessageBox.Show("System Error: This application could not be found in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (licenseAppBusiness.AppInfo.Cancel())
            {
                MessageBox.Show("Application canceled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshList();
            }
            else
            {
                MessageBox.Show("Database Error: Could not cancel the application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = _GetSelectedLocalDrivingLicenseApplicationID();
            if (LocalDrivingLicenseApplicationID == -1) return;

            if (MessageBox.Show("Are you sure you want to delete this application permanently?", "Confirm Permanent Deletion",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (ClsLocalDrivingLicenseAppBusiness.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID))
                {
                    MessageBox.Show("Application deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshList();
                }
                else
                {
                    MessageBox.Show("This application cannot be deleted because it has linked tracking logs or tests.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sechdulToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

          int localdrivinglicenseapp = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells[0].Value;
         
         frmManageTestAppointments frm = new frmManageTestAppointments(localdrivinglicenseapp, ClsTestTypeBusiness.enTestType.Vision);
         
          frm.ShowDialog(); 
        }

        private void sechduleWriteTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localdrivinglicenseapp = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells[0].Value;

            frmManageTestAppointments frm = new frmManageTestAppointments(localdrivinglicenseapp, ClsTestTypeBusiness.enTestType.Written);

            frm.ShowDialog();
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localdrivinglicenseapp = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells[0].Value;

            frmManageTestAppointments frm = new frmManageTestAppointments(localdrivinglicenseapp, ClsTestTypeBusiness.enTestType.Practical);

            frm.ShowDialog();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApp frm = new frmNewLocalDrivingLicenseApp();

            frm.ShowDialog(); 
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localdrivinglicenseid = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;

            ClsLocalDrivingLicenseAppBusiness localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByAppID(localdrivinglicenseid);

            if (localApp == null)
            {
                MessageBox.Show("System Error: Local application record could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!localApp.PassedAllTests())
            {
                MessageBox.Show("The applicant has not passed all 3 required tests yet (Vision, Written, and Practical).",
                        "Requirements Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmIssueDrivingLicenseFirstTime frm = new frmIssueDrivingLicenseFirstTime(localdrivinglicenseid);
            frm.ShowDialog();

            _RefreshList(); 

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLocalDrivingLicenseApps.CurrentRow == null)
            {
                MessageBox.Show("Please select a row first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Make sure "LocalDrivingLicenseApplicationID" matches the 'Name' property of the column in your DataGridView
            object cellValue = dgvLocalDrivingLicenseApps.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;

            if (cellValue == null || cellValue == DBNull.Value)
            {
                MessageBox.Show("Invalid ID found in this row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int localAppID = Convert.ToInt32(cellValue);

            // 3. Continue with your logic
            var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(localAppID);

            if (localApp == null)
            {
                MessageBox.Show("Error: Could not retrieve the application data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseID = ClsLicenseBusiness.GetActiveLicenseIDByApplicationID(localApp.ApplicationID);

            if (LicenseID == -1)
            {
                MessageBox.Show("No active license was found for this application.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }    

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLocalDrivingLicenseApps.CurrentRow == null)
            {
                MessageBox.Show("Please select an application from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Extract the Local Driving License Application ID from your grid row cell
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApps.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value;

            // 3. Look up the application object from the Business Layer
            ClsLocalDrivingLicenseAppBusiness localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);

            if (localApp == null)
            {
                MessageBox.Show("System Error: Could not find application details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Retrieve the PersonID via the base Application Information Info layer property mapping
            // 💡 Note: Verify if your class properties are named exactly like 'localApp.AppInfo.ApplicantPersonID' or 'localApp.ApplicantPersonID'
            int TargetPersonID = localApp.AppInfo.ApplicationPersonID;

            // 5. Initialize your non-user control history form passing the extracted PersonID directly into the constructor parameter!
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(TargetPersonID);

            // 6. Pop the history window up cleanly as a modal dialog box
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = (string)dgvLocalDrivingLicenseApps.CurrentRow.Cells["NationalNo"].Value;

            frmNewLocalDrivingLicenseApp frm = new frmNewLocalDrivingLicenseApp(NationalNo);
            frm.ShowDialog(); 
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilter.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";


            switch (cbFilter.Text)
            {
                case "LocalDrivingLicenseApplicationID": filterColumn = "LocalDrivingLicenseApplicationID"; break;
                case "Driving Class": filterColumn = "DrivingClass"; break;
                case "Status": filterColumn = "Status"; break;
            }

            // If "None" is selected or the search box is empty, show all records
            if (txtFilterValue.Text.Trim() == "" || filterColumn == "None")
            {
                ((DataTable)dgvLocalDrivingLicenseApps.DataSource).DefaultView.RowFilter = "";
                return;
            }

            if (filterColumn == "LocalDrivingLicenseApplicationID")
            {
                // LDLAPPID is a number: use '=' without single quotes
                if (int.TryParse(txtFilterValue.Text.Trim(), out int LDLAPPID))
                {
                    ((DataTable)dgvLocalDrivingLicenseApps.DataSource).DefaultView.RowFilter =
                   string.Format("[{0}] = {1}", filterColumn, LDLAPPID);
                }
                else
                {
                    ((DataTable)dgvLocalDrivingLicenseApps.DataSource).DefaultView.RowFilter = "";
                }

            }
            else
            {
                // Text columns: use 'LIKE' with single quotes
                ((DataTable)dgvLocalDrivingLicenseApps.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim());
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
