
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
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable component for calculating retake test application fees, total test fees, and persisting retake sub-application records.", Module = "Tests Management", Version = "1.0")]
    public partial class ucRetakeTestInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier of the retake test application.
        /// </summary>
        /// <remarks>Default value is -1 to indicate an uninitialized or unset identifier.</remarks>
        private int _RetakeTestApplicationID = -1;
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the identifier for the retake test application.
        /// </summary>
        public int RetakeTestApplicationID => _RetakeTestApplicationID;

        /// <summary>
        /// Gets the application fee for a retake application.
        /// </summary>
        /// <remarks>Looks up application type ID 8 via ClsApplicationTypeBusiness.FindApplicationTypeByID
        /// and returns its ApplicationFees value; falls back to 5.00m if not found.</remarks>
        public decimal RetakeApplicationFee
        {
            get
            {
                var retakeTypeInfo = ClsApplicationTypeBusiness.FindApplicationTypeByID(8);
                return (retakeTypeInfo != null) ? retakeTypeInfo.ApplicationFees : 5.00m;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucRetakeTestInfo class.
        /// </summary>
        /// <remarks>Calls InitializeComponent to initialize the control's UI components.</remarks>
        public ucRetakeTestInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Workflow

        /// <summary>
        /// Resets retake test labels to default values and clears the internal retake test application ID.
        /// </summary>
        /// <remarks>Sets _RetakeTestApplicationID to -1 and updates lblReAppFess, lblTotalFees, and
        /// lblTestAppID to their default display values.</remarks>
        [DocInfo("Resets retake test labels to zero / default state and clears internal application ID.")]
        public void Reset()
        {
            _RetakeTestApplicationID = -1;

            lblReAppFess.Text = "0";

            lblTotalFees.Text = "0";

            lblTestAppID.Text = "N/A";
        }

        /// <summary>
        /// Sets UI labels for a first-time test attempt to indicate a zero retake fee.
        /// </summary>
        /// <remarks>Sets fee labels to "0" and the test application ID label to "N/A".</remarks>
        /// <param name="LocalDrivingLicenseAppID">Local driving license application identifier.</param>
        /// <param name="StandardFee">Standard test fee amount.</param>
        /// <returns>True if UI labels were configured successfully; otherwise false.</returns>
        [DocInfo("Configures UI labels for first-time test attempt with zero retake fee.")]
        public bool LoadRetakeTestInfo(int LocalDrivingLicenseAppID, decimal StandardFee)
        {
            lblReAppFess.Text = "0";

            lblTotalFees.Text = "0";

            lblTestAppID.Text = "N/A";

            return true;
        }

        /// <summary>
        /// Calculates and displays retake fees by adding the retake application fee to the standard test fee and
        /// updates UI labels.
        /// </summary>
        /// <remarks>When TrailCount equals zero, the overload that loads default retake information is
        /// invoked. The test application ID label remains "N/A" until the record is saved.</remarks>
        /// <param name="LocalDrivingLicenseAppID">Identifier of the local driving license application.</param>
        /// <param name="StandardFee">Standard test fee.</param>
        /// <param name="TrailCount">Number of prior trials; if zero, loads default retake information.</param>
        /// <returns>True when retake information is loaded and UI labels are updated.</returns>

        [DocInfo("Calculates total retake fees by adding retake application fee to standard test fee based on trial count.")]
        public bool LoadRetakeTestInfo(int LocalDrivingLicenseAppID, decimal StandardFee, int TrailCount)
        {
            if (TrailCount == 0)
            {
                LoadRetakeTestInfo(LocalDrivingLicenseAppID, StandardFee);
            }

            decimal RetakeAppFee = RetakeApplicationFee;

            lblReAppFess.Text = RetakeAppFee.ToString("F0");

            lblTotalFees.Text = (StandardFee + RetakeAppFee).ToString("F0");

            lblTestAppID.Text = "N/A"; // Stays N/A on screen until they click Save!

            return true;
        }

        /// <summary>
        /// Instantiate and persist a sub-application of type 8 (Retake Test) linked to the person from the specified
        /// local driving license application.
        /// </summary>
        /// <remarks>Sets ApplicationTypeID to 8, AppStatus to New, PaidFees to the retake fee, and
        /// timestamps ApplicationDate and LastStatus with the current time. Loads the master application record if
        /// needed. On success updates _RetakeTestApplicationID and lblTestAppID; on failure displays error message
        /// boxes.</remarks>
        /// <param name="LocalDrivingLicenseAppID">Local driving license application identifier used to locate the base application and associated person.</param>
        /// <param name="CurrentUserID">Identifier of the user creating the retake application; saved to CreatedByUserID.</param>
        /// <returns>True when the retake application is created and saved; otherwise false.</returns>

        [DocInfo("Instantiates and saves a sub-application record of Type 8 (Retake Test) linked to the person ID.")]
        public bool CreateRetakeApplication(int LocalDrivingLicenseAppID, int CurrentUserID)
        {
            ClsApplicationBusiness RetakeApplication = new ClsApplicationBusiness();

            var localDrivingLicenseApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(LocalDrivingLicenseAppID);

            if (localDrivingLicenseApp == null)
            {
                MessageBox.Show("System Error: Local application properties could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (localDrivingLicenseApp.AppInfo == null)
            {
                localDrivingLicenseApp.AppInfo = ClsApplicationBusiness.FindApplicationByID(localDrivingLicenseApp.ApplicationID);
            }

            if (localDrivingLicenseApp.AppInfo == null)
            {
                MessageBox.Show("System Error: Master base application record could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            RetakeApplication.ApplicationPersonID = localDrivingLicenseApp.AppInfo.ApplicationPersonID;

            RetakeApplication.ApplicationDate = DateTime.Now;

            RetakeApplication.ApplicationTypeID = 8;

            RetakeApplication.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;

            RetakeApplication.LastStatus = DateTime.Now;

            RetakeApplication.PaidFees = RetakeApplicationFee;

            RetakeApplication.CreatedByUserID = CurrentUserID;

            if (RetakeApplication.Save())
            {
                _RetakeTestApplicationID = RetakeApplication.ApplicationID;

                lblTestAppID.Text = _RetakeTestApplicationID.ToString();

                return true;
            }
            else
            {
                MessageBox.Show("Failed to create a side system application for the retake fee structure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Handles the control's Load event to initialize UI elements and component state.
        /// </summary>
        /// <remarks>Runs on the UI thread; avoid long-running work here. Use async patterns or background
        /// tasks for expensive operations.</remarks>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments for the Load event.</param>
        private void ucRetakeTestInfo_Load(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
