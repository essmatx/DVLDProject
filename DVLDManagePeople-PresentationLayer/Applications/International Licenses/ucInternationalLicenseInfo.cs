
using DVLDManagePeople_PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// Reusable WinForms UserControl that displays complete international driving license details and the holder's
    /// profile card.
    /// </summary>
    /// <remarks>Populate the control by calling LoadInternationalLicenseInfo(int InternationalLicenseID). The
    /// control formats dates, loads a profile image when available and falls back to embedded defaults, shows
    /// placeholder values when data is missing, and notifies the user if a license is not found. Intended for the
    /// presentation layer and must be hosted on the UI thread; it depends on the business-layer lookup
    /// ClsInternationalLicenseBusiness.FindInternationalLicenseByID.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable composite control displaying complete international driving license details and holder profile card.", Module = "International License Management", Version = "1.0")]
    public partial class ucInternationalLicenseInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier for the international license.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unset or invalid identifier.</remarks>
        private int _InternationalLicenseID = -1;

        /// <summary>
        /// International license business information for the current instance.
        /// </summary>
        private ClsInternationalLicenseBusiness _InternationalLicenseInfo;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Unique identifier for an international license.
        /// </summary>
        /// <remarks>Read-only; assigned when the instance is created and intended to remain
        /// immutable.</remarks>
        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucInternationalLicenseInfo control and sets up its UI components.
        /// </summary>
        /// <remarks>Invoked by the Windows Forms designer to initialize control layout and
        /// resources.</remarks>
        public ucInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & Data Loading

        /// <summary>
        /// Resets internal state and restores placeholder text for all license and driver information labels.
        /// </summary>
        /// <remarks>Sets _InternationalLicenseID to -1 and assigns the placeholder ??? to each UI label
        /// for application, license, driver, and personal details.</remarks>
        [DocInfo("Clears state properties and restores default placeholder text across all license and driver info labels.")]
        public void ResetDefaultValues()
        {
            _InternationalLicenseID = -1;

            lblAppID.Text = "???";
            lblIssueDate.Text = "???";
            lblInternationalLicenseID.Text = "???";
            lblLocalLicenseID.Text = "???";
            lblExpirationDate.Text = "???";
            lblGendor.Text = "???"; 
            lblName.Text = "???";
            lblNationalNo.Text = "???";
            lblIsActive.Text = "???";
            lblDriverID.Text = "???";
            lblDateOfBirth.Text = "???";

        }

        /// <summary>
        /// Loads international license data by ID and populates UI labels and the applicant profile card.
        /// </summary>
        /// <remarks>If no license is found, displays an error, resets default values, and returns false.
        /// If the person image path is missing or the file does not exist, assigns a gender-specific default
        /// image.</remarks>
        /// <param name="InternationalLicenseID">Identifier of the international license to load.</param>
        /// <returns>true if an international license with the specified ID was found and the UI was populated; otherwise, false.</returns>
        [DocInfo("Queries international license domain entity by ID and populates UI labels and applicant profile card.")]
        public bool LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;

            _InternationalLicenseInfo = ClsInternationalLicenseBusiness.FindInternationalLicenseByID(InternationalLicenseID);

            if (_InternationalLicenseInfo == null)
            {
                MessageBox.Show("Error: No International License found with ID = " + InternationalLicenseID,
                                "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaultValues();
                return false;
            }

            lblAppID.Text = _InternationalLicenseInfo.ApplicationID.ToString();
            lblIssueDate.Text = _InternationalLicenseInfo.IssueDate.ToLongDateString();
            lblInternationalLicenseID.Text = _InternationalLicenseInfo.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicenseInfo.IssuedUsingLocalLicenseID.ToString();
            lblExpirationDate.Text = _InternationalLicenseInfo.ExpirationDate.ToShortDateString();
            lblDateOfBirth.Text = _InternationalLicenseInfo.Driverinfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = _InternationalLicenseInfo.DriverID.ToString();

            var PersonInfo = _InternationalLicenseInfo.Appinfo.PersonInof;

            lblIsActive.Text = _InternationalLicenseInfo.IsActive ? "Yes" : "No";
            lblGendor.Text = (PersonInfo.Gendor == 0) ? "Male" : "Female";
            lblNationalNo.Text = PersonInfo.NationalNo;
            lblName.Text = PersonInfo.FullName;

            if (!string.IsNullOrEmpty(PersonInfo.ImagePath) && File.Exists(PersonInfo.ImagePath))
            {
                pbPersonImage.ImageLocation = PersonInfo.ImagePath;
            }
            else
            {
                
                pbPersonImage.Image = (PersonInfo.Gendor == 0) ? Resources.Screenshot_2026_05_18_1424131 : Resources.Screenshot_2026_05_18_152210;
            }


            return true;
        }
        #endregion

        #region Private Event Handlers
        private void ucInternationalLicenseInfo_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
