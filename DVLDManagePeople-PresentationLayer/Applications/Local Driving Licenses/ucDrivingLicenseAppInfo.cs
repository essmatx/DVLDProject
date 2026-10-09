
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// Reusable UserControl that displays a local driving license application's summary information, including
    /// application ID, license class, and passed-test progress.
    /// </summary>
    /// <remarks>Populate the control by calling _LoadDrvingLicenseAppInfo(int LocalDrvingLicenseAppID). Call
    /// Reset() to clear displayed values. Intended for use on the UI thread within the presentation layer.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable UI component displaying local driving license application summary info, license class, and passed test progress.", Module = "Local License Application", Version = "1.0")]
    public partial class ucDrivingLicenseAppInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier for the local driving license application.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unassigned or invalid identifier.</remarks>
        private int _LocalDrvingLicenseAppID = -1;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets the identifier of the local driving license application.
        /// </summary>
        /// <remarks>Read-only integer backed by a private field.</remarks>
        public int LocalDrvingLicenseAppID => _LocalDrvingLicenseAppID;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucDrivingLicenseAppInfo user control.
        /// </summary>
        /// <remarks>Initializes component-level UI elements and resources by calling
        /// InitializeComponent.</remarks>
        public ucDrivingLicenseAppInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & Data Loading

        /// <summary>
        /// Clears internal state and restores UI labels to their initial default values.
        /// </summary>
        /// <remarks>Sets _LocalDrvingLicenseAppID to -1 and resets lblAppID to "???", lblLicenseClass to
        /// "???", and lblTests to "0/3".</remarks>

        [DocInfo("Clears internal state properties and resets UI application labels to initial default values.")]
        public void Reset()
        {
            _LocalDrvingLicenseAppID = -1; 

            lblAppID.Text = "???";

            lblLicenseClass.Text = "???";

            lblTests.Text = "0/3"; 
        }


        /// <summary>
        /// Loads a local driving license application by ID and updates UI labels for application ID, license class, and
        /// passed tests; resets the UI when the sentinel ID is provided or when no entity is found.
        /// </summary>
        /// <remarks>Retrieves the entity via
        /// ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID, updates lblAppID, lblLicenseClass,
        /// and lblTests with the entity data, and displays an error MessageBox if the application is not
        /// found.</remarks>
        /// <param name="LocalDrvingLicenseAppID">ID of the local driving license application to load; pass -1 to reset the UI.</param>
        [DocInfo("Queries local driving license application domain entity by ID and populates UI labels and test counters.")]
        public void _LoadDrvingLicenseAppInfo(int LocalDrvingLicenseAppID)
        {
            _LocalDrvingLicenseAppID = LocalDrvingLicenseAppID;

            if(_LocalDrvingLicenseAppID == -1)
            {
                Reset();

                return; 
            }



            ClsLocalDrivingLicenseAppBusiness clsLocalDriving = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocalDrvingLicenseAppID);

            if (clsLocalDriving == null)
            {
                Reset();

                MessageBox.Show("No Local Driving License Application found with ID" + _LocalDrvingLicenseAppID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            lblAppID.Text = clsLocalDriving.LocalDrivingLicenseApplicationID.ToString(); 

            lblLicenseClass.Text = clsLocalDriving.LicenseClassInfo.ClassName;

            int PassedTest = clsLocalDriving.GetPassedTestsCount();

            lblTests.Text = $"{PassedTest}/3"; 
        }
        #endregion

        #region Private Event Handlers
        private void DrivingLicenseAppInfo_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
