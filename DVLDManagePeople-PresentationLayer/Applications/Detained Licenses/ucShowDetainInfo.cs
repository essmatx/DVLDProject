
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Shared.Attributes.clsDocAttributes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// Reusable WinForms UserControl that displays detained license details (detain ID, license ID, detain date,
    /// created-by) and calculates financial totals (fine, application, and total fees) used during license release
    /// workflows.
    /// </summary>
    /// <remarks>Loads detained license data by license ID, retrieves the release application fee from
    /// application-type data, updates visible labels, records the release application ID after a successful release,
    /// and resets displayed values when no active detain is found. Intended for the presentation layer; data retrieval
    /// and business logic are delegated to underlying business classes.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable UI component for displaying detained license details and calculating release financial totals during license release workflows.", Module = "License Management", Version = "1.0")]
    public partial class ucShowDetainInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Stores the license identifier; a value of -1 indicates unassigned.
        /// </summary>
        /// <remarks>Used internally to represent the license state; negative values denote uninitialized
        /// or absent licenses.</remarks>
        private int _LicenseID = -1;

        /// <summary>
        /// Identifier for a detain record.
        /// </summary>
        /// <remarks>Defaults to -1 to indicate an uninitialized or unset identifier.</remarks>
        private int _DetainID = -1;

        /// <summary>
        /// Identifier for the release application.
        /// </summary>
        /// <remarks>A value of -1 indicates the identifier is unspecified.</remarks>
        private int _ReleaseApplicationID = -1;

        /// <summary>
        /// Total amount of accrued fines and fees.
        /// </summary>
        /// <remarks>Represents a monetary value stored as a decimal for accurate financial calculations.
        /// Initialized to zero.</remarks>
        private decimal _FineFees = 0; 

        /// <summary>
        /// Total application fees in the account currency.
        /// </summary>
        /// <remarks>Initial value is 0. Stored as a decimal to preserve precision for monetary
        /// amounts.</remarks>
        private decimal _ApplicationFees = 0;
        #endregion

        #region Scalar & Financial Properties

        /// <summary>
        /// Gets the identifier for the detain.
        /// </summary>
        /// <remarks>Backed by the _DetainID field.</remarks>
        public int DetainID => _DetainID;

        /// <summary>
        /// Gets the license identifier.
        /// </summary>
        /// <remarks>Backed by the _LicenseID field.</remarks>
        public int LicenseID => _LicenseID;

        /// <summary>
        /// Gets the total amount of fine fees.
        /// </summary>
        /// <remarks>Represents a monetary amount in the application's currency. Use culture-specific
        /// formatting when displaying.</remarks>
        public decimal FineFees => _FineFees;

        /// <summary>
        /// Gets the application fees.
        /// </summary>
        public decimal ApplicationFees => _ApplicationFees;

        /// <summary>
        /// Gets the total fees as the sum of ApplicationFees and FineFees.
        /// </summary>
        /// <remarks>Computed, read-only value derived from ApplicationFees and FineFees.</remarks>
        public decimal TotalFees => _ApplicationFees + _FineFees;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ucShowDetainInfo control and initializes its UI components.
        /// </summary>
        public ucShowDetainInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & State Management

        /// <summary>
        /// Queries the active detention record and release application fees for the specified license ID and populates
        /// UI display labels.
        /// </summary>
        /// <remarks>Sets internal fields (_LicenseID, _DetainID, _FineFees, _ApplicationFees), retrieves
        /// the release application type (ID 5) to obtain application fees, formats and assigns label texts for detain
        /// ID, license ID, detain date, created by, fine, application, and total fees. Calls ResetDefaultValues and
        /// returns false if no detained record or record lookup fails.</remarks>
        /// <param name="licenseID">License identifier to query.</param>
        /// <returns>True when an active detained license is found and UI labels are populated; false when no active record
        /// exists and default values are applied.</returns>
        [DocInfo("Queries active detention record and release application fees by License ID, populating UI display labels.")]
        public bool LoadDetainedLicenseDataByLicenseID(int licenseID)
        {
            _LicenseID = licenseID;

            int activeDetainID = ClsDetainedLicenseBusiness.GetDetainedLicenseByLicenseID(_LicenseID);

            if (activeDetainID == -1)
            {
                ResetDefaultValues();
                return false;
            }

            ClsDetainedLicenseBusiness detainedLicense = ClsDetainedLicenseBusiness.FindDetainedLicenseByDetainID(activeDetainID);

            if (detainedLicense == null)
            {
                ResetDefaultValues();
                return false;
            }

            _DetainID = detainedLicense.DetainID;
            _FineFees = detainedLicense.FineFees;

           

            ClsApplicationTypeBusiness releaseAppType = ClsApplicationTypeBusiness.FindApplicationTypeByID(5); 

            if(releaseAppType != null)
            {
                _ApplicationFees = releaseAppType.ApplicationFees; 
            }
            else
            {
                _ApplicationFees = 0; 
            }


            lblDetainID.Text = _DetainID.ToString();
            lblLicenseID.Text = _LicenseID.ToString();
            lblDetainDate.Text = detainedLicense.DetainDate.ToString("yyyy-MM-dd");
            lblCreatedBy.Text = detainedLicense.Userinfo.UserName;


            lblFineFees.Text = _FineFees.ToString("0.00");
            lblAppFees.Text = _ApplicationFees.ToString("0.00");
            lblTotalFees.Text = TotalFees.ToString("0.00");

            return true;
        }

        /// <summary>
        /// Binds the generated release application identifier to the UI label after a successful license release.
        /// </summary>
        /// <remarks>Updates the stored release application identifier and sets the label text to its
        /// string representation.</remarks>
        /// <param name="releaseApplicationID">Identifier of the released application to display in the UI.</param>
        [DocInfo("Binds generated release application ID to UI label after su" +
            "ccessful license release.")]
        public void UpdateAfterReleaseSuccess(int releaseApplicationID)
        {
            _ReleaseApplicationID = releaseApplicationID;
            lblReleaseID.Text = _ReleaseApplicationID.ToString();
        }

        /// <summary>
        /// Restores default state values and resets UI labels for license, detain, release, and fee information.
        /// </summary>
        /// <remarks>Sets identifier fields to -1, fee fields to 0, the date label to the current date
        /// ("yyyy-MM-dd"), numeric fee labels to "0.00", and text labels to "???".</remarks>
        [DocInfo("Restores default state values and resets UI financial labels.")]
        public void ResetDefaultValues()
        {
            _LicenseID = -1;
            _DetainID = -1;
            _ReleaseApplicationID = -1;
            _FineFees = 0;
            _ApplicationFees = 0;

            lblDetainID.Text = "???";
            lblLicenseID.Text = "???";
            lblDetainDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblCreatedBy.Text = "???";
            lblFineFees.Text = "0.00";
            lblAppFees.Text = "0.00";
            lblTotalFees.Text = "0.00";
            lblReleaseID.Text = "???";
        }
        #endregion
    }
}
