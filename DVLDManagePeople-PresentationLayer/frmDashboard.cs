using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmDashboard : Form
    {
        frmLogin _frmLogin; 
        public frmDashboard(frmLogin frmLogin)
        {
            InitializeComponent();

            _frmLogin = frmLogin; 
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople frm1 = new frmManagePeople();

            frm1.ShowDialog(); 
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers frmManageUsers = new frmManageUsers();

            frmManageUsers.ShowDialog(); 
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes manageApplicationTypes = new frmManageApplicationTypes();

            manageApplicationTypes.ShowDialog(); 
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frmManageTestTypes = new frmManageTestTypes();

            frmManageTestTypes.ShowDialog(); 
        }

        private void localDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApp frmNewLocalDrivingLicenseApp = new frmNewLocalDrivingLicenseApp();

            frmNewLocalDrivingLicenseApp.ShowDialog(); 
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frmLocalDrivingLicenseApp = new frmLocalDrivingLicenseApp();

            frmLocalDrivingLicenseApp.ShowDialog(); 
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDrivers frm = new frmManageDrivers();

            frm.ShowDialog(); 
        }

        private void internationalDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();

            frm.ShowDialog(); 
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReNewLocalDrivingLicense frm = new frmReNewLocalDrivingLicense();

            frm.ShowDialog(); 
        }

        private void replacementForLostOrDamgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseReplacementForLostOrDamaged frm = new frmLicenseReplacementForLostOrDamaged();

            frm.ShowDialog(); 
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            
            SetupAccountSettingContextMenu(cmsAccountSetting);
            btnAccountSetting.Click += btnAccountSetting_Click;

            _RefreshQueueStatusData();

            LoadTodayAppointmentsData();

            ToatlLicense();

            LoadExpiringSoonData();

            LoadDetainedLicensesData();
            
        }

        private void detainLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmDetainLicensApp frm = new frmDetainLicensApp();

            frm.ShowDialog(); 
        }

        private void realeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainLicense frm = new frmReleaseDetainLicense();

            frm.ShowDialog(); 
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm = new frmManageDetainedLicenses();

            frm.ShowDialog(); 
        }

        private void realseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainLicense frm = new frmReleaseDetainLicense();

            frm.ShowDialog(); 
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frm = new frmLocalDrivingLicenseApp();

            frm.ShowDialog(); 
        }

        private void internationalDrivingLicenseAppliationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageInternationalDrivingLicense frm = new frmManageInternationalDrivingLicense();

            frm.ShowDialog(); 
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog(); 
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close(); 
        }

        private void frmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void _RefreshQueueStatusData()
        {
            try
            {
                // 1. Fetch pending application counts from Business Layer
                // ApplicationType IDs: 1 = New Driving License, 2 = Renew, 3 = Replacement
                int newLicensesCount = ClsApplicationBusiness.GetPendingCountByApplicationType(1);
                int renewalsCount = ClsApplicationBusiness.GetPendingCountByApplicationType(2);
                int replacementsCount = ClsApplicationBusiness.GetPendingCountByApplicationType(3);

                // 2. Calculate Total Waiting
                int totalWaiting = newLicensesCount + renewalsCount + replacementsCount;

                // 3. Calculate Efficiency Rate (e.g. Completed / Total Applications * 100)
                double efficiencyRate = ClsApplicationBusiness.CalculateSystemEfficiencyRate();

                // 4. Update UI Labels (Formatted with leading zeros or percentages)
                lblNewLicensesCount.Text = newLicensesCount.ToString("D2");
                lblRenewalsCount.Text = renewalsCount.ToString("D2");
                lblReplacementsCount.Text = replacementsCount.ToString("D2");
                lblTotalWaiting.Text = totalWaiting.ToString();
                lblEfficiencyRate.Text = $"{efficiencyRate:F1}%";
            }
            catch (Exception ex)
            {
                // Fallback default values on error
                lblNewLicensesCount.Text = "00";
                lblRenewalsCount.Text = "00";
                lblReplacementsCount.Text = "00";
                lblTotalWaiting.Text = "0";
                lblEfficiencyRate.Text = "0.0%";
            }
        }

        private void LoadTodayAppointmentsData()
        {
            try
            {
                // 1. Fetch count per test type (1 = Vision, 2 = Written, 3 = Street)
                int visionCount = ClsTestAppointmentBusiness.GetTodayAppointmentsCountByTestType(1);
                int writtenCount = ClsTestAppointmentBusiness.GetTodayAppointmentsCountByTestType(2);
                int streetCount = ClsTestAppointmentBusiness.GetTodayAppointmentsCountByTestType(3);

                // 2. Total today
                int totalToday = visionCount + writtenCount + streetCount;

                // 3. Update UI Labels (formatted with leading zero if preferred)
                lblTodayVisionCount.Text = visionCount.ToString("D2");  // e.g., "05"
                lblTodayWrittenCount.Text = writtenCount.ToString("D2"); // e.g., "12"
                lblTodayStreetCount.Text = streetCount.ToString("D2");  // e.g., "03"

                lblTotalTodayAppointments.Text = totalToday.ToString();
            }
            catch (Exception ex)
            {
                lblTodayVisionCount.Text = "00";
                lblTodayWrittenCount.Text = "00";
                lblTodayStreetCount.Text = "00";
                lblTotalTodayAppointments.Text = "0";
            }
        }

        private void ToatlLicense()
        {
            int toatl = ClsLicenseBusiness.GetTotalLicensesCount();

            lblLicensesIssuedTrend.Text = toatl.ToString(); 
        }

        private void LoadExpiringSoonData()
        {
            try
            {
                // 1. Fetch count of active licenses expiring in the next 30 days
                int expiringCount = ClsLicenseBusiness.GetExpiringSoonLicensesCount(30);

                // 2. Update UI Count Label
                lblExpiringSoonCount.Text = expiringCount.ToString();

                // Optional: Highlight card if there are urgent expirations
                if (expiringCount > 0)
                {
                    lblExpiringSoonCount.ForeColor = Color.FromArgb(229, 192, 123); // Gold Alert
                }
            }
            catch (Exception ex)
            {
                lblExpiringSoonCount.Text = "0";
            }
        }

        private void LoadDetainedLicensesData()
        {
            try
            {
                // 1. Fetch count of currently detained licenses from DB
                int detainedCount = ClsDetainedLicenseBusiness.GetActiveDetainedLicensesCount();

                // 2. Update UI Count Label
                lblDetainedLicensesCount.Text = detainedCount.ToString();

                // Optional: Highlight number if there are active detentions
                if (detainedCount > 0)
                {
                    lblDetainedLicensesCount.ForeColor = Color.FromArgb(229, 192, 123); // Gold Alert
                }
            }
            catch (Exception ex)
            {
                lblDetainedLicensesCount.Text = "0";
            }
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            cmsApplications.Show(btnNewApplication, new Point(0, btnNewApplication.Height));
        }

        private void StyleContextMenu(ToolStripDropDown dropDown)
        {
            dropDown.BackColor = Color.FromArgb(20, 20, 20); // Sleek obsidian dark background
            dropDown.ForeColor = Color.FromArgb(240, 240, 240); // Elegant off-white text
            dropDown.Font = new Font("Georgia", 11.5f, FontStyle.Regular);

            if (dropDown is ToolStripDropDownMenu dropDownMenu)
            {
                dropDownMenu.ShowImageMargin = false;
            }

            if (dropDown is Guna2ContextMenuStrip gunaCms)
            {
                gunaCms.RenderStyle.SelectionBackColor = Color.FromArgb(37, 35, 31); // Dark charcoal gold hover background
                gunaCms.RenderStyle.SelectionForeColor = Color.FromArgb(197, 160, 89); // Warm Gold hover text
                gunaCms.RenderStyle.BorderColor = Color.FromArgb(197, 160, 89); // Gold border
                gunaCms.RenderStyle.ArrowColor = Color.FromArgb(197, 160, 89); // Gold submenu arrows
                gunaCms.RenderStyle.SeparatorColor = Color.FromArgb(60, 197, 160, 89); // Subtle gold separator line
                gunaCms.RenderStyle.RoundedEdges = true;
            }
            else
            {
                // Inherit renderer for submenus to draw gold arrows and selection styling
                dropDown.Renderer = cmsApplications.Renderer;
            }

            foreach (ToolStripItem item in dropDown.Items)
            {
                item.BackColor = Color.FromArgb(20, 20, 20);
                item.ForeColor = Color.FromArgb(240, 240, 240);
                item.Font = new Font("Georgia", 11.5f, FontStyle.Regular);

                if (item is ToolStripSeparator)
                {
                    item.Margin = new Padding(0, 5, 0, 5);
                }
                else
                {
                    // Spacious vertical and horizontal padding to match the high-end feel in the screenshot
                    item.Padding = new Padding(12, 10, 12, 10);
                }

                if (item is ToolStripMenuItem menuItem)
                {
                    if (menuItem.HasDropDownItems)
                    {
                        StyleContextMenu(menuItem.DropDown);
                    }
                }
            }
        }

      

        private void btnAccountSetting_Click(object sender, EventArgs e)
        {
            cmsAccountSetting.Show(btnAccountSetting, new Point(0, btnAccountSetting.Height));
        }

        private void SetupAccountSettingContextMenu(Guna.UI2.WinForms.Guna2ContextMenuStrip cms)
        {
            // --- 1. BUILD MENU ITEMS ---
            cms.Items.Clear();

            cms.Items.Add("Current User Info", null, currentUserInfoToolStripMenuItem_Click);
            cms.Items.Add("Change Password", null, changePasswordToolStripMenuItem_Click);
            cms.Items.Add("Sign Out", null, signOutToolStripMenuItem_Click);

            // --- 2. GLASS & GOLD THEME STYLING (Recursive) ---
            StyleContextMenu(cms);
        }

        private void newDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void renewDrivingLicenseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmReNewLocalDrivingLicense frm = new frmReNewLocalDrivingLicense();

            frm.ShowDialog(); 
        }

        private void replacementForLostOrDamageLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseReplacementForLostOrDamaged frm = new frmLicenseReplacementForLostOrDamaged();

            frm.ShowDialog(); 
        }

        private void localDrivinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frmLocalDrivingLicenseApp = new frmLocalDrivingLicenseApp();

            frmLocalDrivingLicenseApp.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageInternationalDrivingLicense frm = new frmManageInternationalDrivingLicense();

            frm.ShowDialog(); 
        }

        private void manageDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm = new frmManageDetainedLicenses();

            frm.ShowDialog(); 
        }

        private void detainLicenseToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmDetainLicensApp frm = new frmDetainLicensApp();

            frm.ShowDialog(); 
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainLicense frm = new frmReleaseDetainLicense();

            frm.ShowDialog(); 
        }

        private void manageApplicationTypesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmManageApplicationTypes manageApplicationTypes = new frmManageApplicationTypes();

            manageApplicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();

            frm.ShowDialog(); 
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            frm.ShowDialog(); 
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            frmNewLocalDrivingLicenseApp frm = new frmNewLocalDrivingLicenseApp();

            frm.ShowDialog(); 
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();
            frm.ShowDialog(); 
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm = new frmManageDetainedLicenses();

            frm.ShowDialog(); 
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmManagePeople frm = new frmManagePeople();

            frm.ShowDialog(); 
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmManageDrivers frm = new frmManageDrivers();

            frm.ShowDialog(); 
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            frmManageUsers frm = new frmManageUsers();

            frm.ShowDialog(); 
        }

        private void localDrivingLicenseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frm = new frmLocalDrivingLicenseApp();

            frm.ShowDialog(); 
        }

        private void internationalDrivingLicenseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();

            frm.ShowDialog(); 
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainLicense frm = new frmReleaseDetainLicense();

            frm.ShowDialog(); 
        }

        private void retakeTestToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frm = new frmLocalDrivingLicenseApp();

            frm.ShowDialog();
        }
    }
}
