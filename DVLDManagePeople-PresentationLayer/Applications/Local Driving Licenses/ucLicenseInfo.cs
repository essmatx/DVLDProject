
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessWorkflows;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// Composite UserControl that displays local driving license information, including license class, driver name and
    /// identifiers, issue and expiration dates, issue reason and notes, active and detention status, and the driver
    /// profile picture.
    /// </summary>
    /// <remarks>Call LoadLicenseInfo(int LicenseID) to populate the control; invalid or missing IDs reset the
    /// control and show an error message. ResetLicenseInfo clears all displayed values. The control exposes LicenseID,
    /// SelectedLicenseInfo, and IsDetained to retrieve the current state. The driver image is loaded from
    /// Driverinfo.ImagePath when present; a built-in placeholder is used otherwise. Methods update UI elements and
    /// should be invoked on the UI thread.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable composite control displaying local driving license details, issue reasons, detention status, and driver profile picture.", Module = "Local License Management", Version = "1.0")]
    public partial class ucLicenseInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Backing field that stores the license identifier.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unset or unknown license; used internally as the
        /// storage for the public property or methods that expose license information.</remarks>
        private int _LicenseID = -1;

        /// <summary>
        /// Instance of ClsLicenseBusiness used to access and manage licensing operations.
        /// </summary>
        /// <remarks>May be null until initialized; initialized and managed by the declaring
        /// class.</remarks>
        private ClsLicenseBusiness _LicenseInfo;

        /// <summary>
        /// Indicates whether the instance is currently detained.
        /// </summary>
        /// <remarks>Backing field for the IsDetained property. Defaults to false.</remarks>
        private bool _IsDetained = false;
        #endregion 

        #region Scalar & Entity Properties

        /// <summary>
        /// Gets the unique identifier for the license.
        /// </summary>
        public int LicenseID => _LicenseID;

        /// <summary>
        /// Gets the selected license information.
        /// </summary>
        /// <remarks>Read-only; returns the backing _LicenseInfo field.</remarks>
        public ClsLicenseBusiness SelectedLicenseInfo => _LicenseInfo;

        /// <summary>
        /// Gets a value indicating whether the instance is detained.
        /// </summary>
        /// <remarks>Read-only. Backed by a private field that reflects the current detained
        /// status.</remarks>
        public bool IsDetained => _IsDetained;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of ucLicenseInfo and configures its user interface components.
        /// </summary>
        /// <remarks>Called by the Windows Forms designer; InitializeComponent constructs and configures
        /// controls, layout, and event hookups.</remarks>
        public ucLicenseInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & Data Loading

        /// <summary>
        /// Queries the license entity by identifier, determines detention status, and populates UI controls with
        /// license and driver details.
        /// </summary>
        /// <remarks>If the identifier is invalid or the license is not found, resets UI and displays an
        /// error. Populates labels for class, driver name and identifiers, dates, gender, active/detained status,
        /// reason, and notes; loads the driver's image if the path exists, otherwise sets a default
        /// placeholder.</remarks>
        /// <param name="LicenseID">License identifier to query; expected to be greater than zero.</param>
        [DocInfo("Queries license domain entity by ID, evaluates detention status, and populates UI controls.")]
        public void LoadLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID; 
            _LicenseInfo = ClsLicenseBusiness.FindLicenseByID(_LicenseID);



            if (LicenseID <= 0)
            {
                ResetLicenseInfo();
                MessageBox.Show("Invalid License ID provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_LicenseInfo == null)
            {
                ResetLicenseInfo(); 
                MessageBox.Show($"No License with ID = {LicenseID} found in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _IsDetained = ClsDetainedLicenseBusiness.IsLicenseDetaind(_LicenseID);


            lblLicenseClass.Text = _LicenseInfo.Classinfo.ClassName;

            lblName.Text = _LicenseInfo.Driverinfo.FullName;

            lblLicenseID.Text = _LicenseInfo.LicenseID.ToString();

            lblGendor.Text = (_LicenseInfo.Driverinfo.Gendor == 0) ? "Male" :"Female";

            lblNationalNo.Text = _LicenseInfo.Driverinfo.NationalNo;

            lblIssueDate.Text = _LicenseInfo.IssueDate.ToString();

            lblDrivierID.Text = _LicenseInfo.DriverID.ToString();


            lblReason.Text = _GetIssueReasonText(_LicenseInfo.IssueReason);
            lblNotes.Text = string.IsNullOrEmpty(_LicenseInfo.Notes) ? "No Notes Provided" : _LicenseInfo.Notes;


            lblExpiration.Text = _LicenseInfo.ExpirationDate.ToShortDateString();

            lblDateOfbirth.Text = _LicenseInfo.Driverinfo.DateOfBirth.ToString();

            lblActive.Text = (_LicenseInfo.IsActive) ? "Yes" : "No";

            lblDetained.Text = (_IsDetained)? "Yes" : "No";

            if(!string.IsNullOrEmpty(_LicenseInfo.Driverinfo.ImagePath))
            {
                if(System.IO.File.Exists(_LicenseInfo.Driverinfo.ImagePath))
                {
                    pbDriverImage.ImageLocation = _LicenseInfo.Driverinfo.ImagePath;
                }
                else
                    pbDriverImage.Image = Properties.Resources.Screenshot_2026_05_18_1424131;
            }
           
        }

        /// <summary>
        /// Clears license-related UI fields and restores placeholder text for all license display labels.
        /// </summary>
        /// <remarks>Resets visible label text to default placeholders and clears related state; invoke on
        /// the UI thread to avoid cross-thread access issues.</remarks>
        [DocInfo("Clears state properties and restores default placeholder text across all license display labels.")]
        public void ResetLicenseInfo()
        {
            lblLicenseClass.Text = "???"; 

            lblName.Text = "???";
            lblLicenseID.Text = "???";

            lblGendor.Text = "???";

            lblNationalNo.Text = "???";

            lblIssueDate.Text = "???";


            lblReason.Text = "???";
            lblNotes.Text = "???";


            lblExpiration.Text = "???";

            lblDateOfbirth.Text = "???";

            lblActive.Text = "???";

            lblDetained.Text = "???";
        }
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets the text representation for a license issue reason.
        /// </summary>
        /// <remarks>Returns 'Unknown' for values not explicitly handled.</remarks>
        /// <param name="IssueReason">The issue reason value.</param>
        /// <returns>The text representation of the specified issue reason.</returns>
        private string _GetIssueReasonText(ClsLicenseBusiness.enIssueReason IssueReason)
        {
            switch ((byte)IssueReason)
            {
                case 1: return "First Time";
                case 2: return "Renew";
                case 3: return "Replacement for Damaged";
                case 4: return "Replacement for Lost";
                default: return "Unknown";
            }
        }
       
        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pbDriverImage_Click(object sender, EventArgs e)
        {

        }

        private void ucLicenseInfo_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
