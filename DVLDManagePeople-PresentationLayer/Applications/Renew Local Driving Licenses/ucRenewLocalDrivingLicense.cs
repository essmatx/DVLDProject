
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
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// User control that calculates renewal fees, previews validity extensions, and displays post-renewal application
    /// and license information.
    /// </summary>
    /// <remarks>Intended for presentation-layer use. Loads a renewal preview only for expired licenses,
    /// computes application and class fees, and projects a new expiration date using the license class
    /// DefaultValidityLingth. Depends on ClsLicenseBusiness, ClsLicensClassBusiness, and ClsApplicationTypeBusiness and
    /// expects an application type with ID 2 for renewal. Presents MessageBox dialogs for missing data or validation
    /// failures. Exposes SelectedLicenseInfo, GetNotes, InitializePostRenewalData, and ResetControlValues for
    /// integration with hosting forms.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable component for calculating license renewal fees, validity extension preview, and rendering post-renewal application records.", Module = "Driving Licenses", Version = "1.0")]
    public partial class ucRenewLocalDrivingLicense : UserControl
    {
        #region Private Fields
        /// <summary>
        /// Previous ClsLicenseBusiness instance retained for comparison or rollback.
        /// </summary>
        /// <remarks>May be null when no prior license has been recorded.</remarks>
        private ClsLicenseBusiness _OldLicense;

        /// <summary>
        /// A ClsLicenseBusiness instance used to create or manage a new license.
        /// </summary>
        /// <remarks>May be null until initialized.</remarks>
        private ClsLicenseBusiness _NewLicense;

        /// <summary>
        /// Instance of ClsApplicationBusiness providing application business logic.
        /// </summary>
        private ClsApplicationBusiness _Application;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the currently selected license information.
        /// </summary>
        /// <remarks>Read-only. May be null if no license is selected.</remarks>
        public ClsLicenseBusiness SelectedLicenseInfo => _OldLicense;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ucRenewLocalDrivingLicense user control and initializes its UI components.
        /// </summary>
        /// <remarks>Performs component initialization required for the control's visual layout and
        /// behavior.</remarks>
        public ucRenewLocalDrivingLicense()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Renewal Workflow

        /// <summary>
        /// Validate the original license expiration, retrieve related fees, and populate UI preview fields for renewal.
        /// </summary>
        /// <remarks>Displays error or warning message boxes on failure and resets UI control values when
        /// validation fails. Looks up the license via ClsLicenseBusiness.FindLicenseByID and compares its
        /// ExpirationDate to the current DateTime before invoking PopulatePreviewFields.</remarks>
        /// <param name="OldLicenseID">ID of the original license to validate and load preview data for.</param>
        /// <returns>True if preview data was successfully loaded and UI fields populated; false if the license was not found or
        /// not expired.</returns>
        [DocInfo("Validates original license expiration state, fetches class and application fees, and populates UI preview fields.")]
        public bool LoadRenewalPreviewData(int OldLicenseID)
        {
            _OldLicense = ClsLicenseBusiness.FindLicenseByID(OldLicenseID); 

            if(_OldLicense == null)
            {
                MessageBox.Show("License not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetControlValues();
                return false;
            }

            if(_OldLicense.ExpirationDate > DateTime.Now)
            {
                MessageBox.Show($"Selected License is not yet expired. It will expire on: {_OldLicense.ExpirationDate.ToShortDateString()}",
                        "Not Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetControlValues();
                return false;
            }

            PopulatePreviewFields();

            return true; 
        }

        /// <summary>
        /// Gets the user-entered notes text from the UI, trimmed of leading and trailing whitespace.
        /// </summary>
        /// <remarks>Reads the Text property of the txtNotes control. Call on the UI thread to avoid
        /// cross-thread access issues.</remarks>
        /// <returns>A string containing the notes text with surrounding whitespace removed; empty if no text is entered.</returns>
        [DocInfo("Returns trimmed notes input text from UI control.")]
        public string GetNotes()
        {

            return txtNotes.Text.Trim();
        }

       /// <summary>
       /// Binds post-renewal transaction IDs (new Application ID and new License ID) to UI labels.
       /// </summary>
       /// <remarks>Retrieves the application and license via business-layer lookup and updates the labels
       /// only if both are found.</remarks>
       /// <param name="NewApplicationID">ID of the new application to retrieve and display.</param>
       /// <param name="NewLicenseID">ID of the new license to retrieve and display.</param>

        [DocInfo("Binds post-renewal transaction IDs (new Application ID and new License ID) to the UI.")]
        public void InitializePostRenewalData(int NewApplicationID, int NewLicenseID)
        {
            _Application = ClsApplicationBusiness.FindApplicationByID(NewApplicationID);

            _NewLicense = ClsLicenseBusiness.FindLicenseByID(NewLicenseID);

            if (_Application != null && _NewLicense != null)
            {
                lblRenewLicenseAppID.Text = _Application.ApplicationID.ToString();

                lblNewLicenseID.Text = _NewLicense.LicenseID.ToString();
            }
        }

        /// <summary>
        /// Resets form input controls and fee displays, clears notes, and releases references to the current license
        /// and application.
        /// </summary>
        /// <remarks>Clears label and textbox values and sets internal license and application fields to
        /// null; does not dispose controls or persist changes to underlying data.</remarks>

        [DocInfo("Resets form input controls, fee counters, and releases internal entity references.")]
        public void ResetControlValues()
        {
            _OldLicense = null;
            _NewLicense = null;
            _Application = null;

            lblRenewLicenseAppID.Text = "???";
            lblNewLicenseID.Text = "???";
            lblExLicenseID.Text = "???";
            lblExpirationDate.Text = "???";
            lblAppFees.Text = "???";
            lblLicenseFees.Text = "???";
            lblTotalFees.Text = "???";
            txtNotes.Text = "";
        }
        #endregion

        #region Private Preview & UI Helpers

        /// <summary>
        /// Populate preview labels with the license ID, current dates, creator, application and license fees, total
        /// fees, and expiration date.
        /// </summary>
        /// <remarks>Fetches the license class using _OldLicense.LicenseClassID and the application type
        /// for renewal (ID 2). Displays an error MessageBox and returns if either lookup fails. Formats fee labels to
        /// two decimal places, computes total as application fees plus class fees, and sets expiration by adding
        /// LicenseClass.DefaultValidityLingth years to the current date.</remarks>
        [DocInfo("Fetches license class and renewal application fees from database and updates display labels.")]
        private void PopulatePreviewFields()
        {
            lblExLicenseID.Text = _OldLicense.LicenseID.ToString();

            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = _OldLicense.Userinfo.UserName;
           

            ClsLicensClassBusiness LicenseClass = ClsLicensClassBusiness.FindLicenseClassByID(_OldLicense.LicenseClassID);

            if(LicenseClass == null)
            {
                MessageBox.Show("System Error: License Class rules could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClsApplicationTypeBusiness AppType = ClsApplicationTypeBusiness.FindApplicationTypeByID(2);

            if(AppType == null)
            {
                MessageBox.Show("System Error: Application Type for 'Renewal' (ID 2) is missing from the database.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblAppFees.Text = AppType.ApplicationFees.ToString("0.00");

            lblLicenseFees.Text = LicenseClass.ClassFees.ToString("0.00");

            decimal TotalFees = AppType.ApplicationFees + LicenseClass.ClassFees;

            lblTotalFees.Text = TotalFees.ToString("0.00");

            lblExpirationDate.Text = DateTime.Now.AddYears(LicenseClass.DefaultValidityLingth).ToShortDateString(); 

        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Handles the control's Load event.
        /// </summary>
        /// <remarks>Used to initialize control state, load data, and attach event handlers as
        /// needed.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void ucRenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
