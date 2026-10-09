
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable UI component for displaying international driving license application info and initializing issue workflows.", Module = "International License Management", Version = "1.0")]
    public partial class ucInternationalDrivingLicenseAppInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier for an international license; -1 indicates unspecified.
        /// </summary>
        /// <remarks>Backing field for the InternationalLicenseID property.</remarks>
        private int _InternationalLicenseID = -1;

        /// <summary>
        /// Stores international license business information.
        /// </summary>
        private ClsInternationalLicenseBusiness _InternationalLicenseInfo;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets the international license identifier.
        /// </summary>
        /// <remarks>Read-only; backed by the _InternationalLicenseID field.</remarks>
        public int InternationalLicense
        {
            get { return _InternationalLicenseID;  }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucInternationalDrivingLicenseAppInfo class.
        /// </summary>
        /// <remarks>Initializes UI controls and layout by calling InitializeComponent.</remarks>
        public ucInternationalDrivingLicenseAppInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & State Management

       /// <summary>
       /// Resets internal and UI state to defaults: clears application labels and sets the international license
       /// identifier to -1.
       /// </summary>
       /// <remarks>Sets the backing field _InternationalLicenseID to -1 and assigns the placeholder ??? to
       /// all application label controls.</remarks>
        [DocInfo("Restores default state values and clears UI application labels.")]
        public void ResetDefaultValues()
        {
            _InternationalLicenseID = -1;

            lblInternationalApplicationID.Text = "???";
            lblAppDate.Text = "???";
            lblIssueDate.Text = "???";
            lblFees.Text = "???";
            lblInternationalLicenseID.Text = "???";
            lblLocalLicenseID.Text = "???";
            lblExpirationDate.Text = "???";
            lblCreatedBy.Text = "???";
        }

        /// <summary>
        /// Binds generated application and international license IDs to display labels after transaction persistence.
        /// </summary>
        /// <remarks>Updates the backing field _InternationalLicenseID and the Text properties of
        /// lblInternationalApplicationID and lblInternationalLicenseID.</remarks>
        /// <param name="ApplicationID">Generated application identifier to display.</param>
        /// <param name="InternationalLicenseID">Generated international license identifier to display.</param>
        [DocInfo("Binds generated application ID and international license ID to display labels after transaction persistence.")]
        public void UpdateGeneratedIDs(int ApplicationID, int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            lblInternationalApplicationID.Text = ApplicationID.ToString();
            lblInternationalLicenseID.Text = InternationalLicenseID.ToString();
        }

        /// <summary>
        /// Queries the international license by ID and populates UI labels with the retrieved information.
        /// </summary>
        /// <remarks>Sets internal fields and label text based on the found entity. On success, assigns
        /// _InternationalLicenseID and _InternationalLicenseInfo and updates labels such as
        /// lblInternationalApplicationID, lblAppDate, lblIssueDate, lblFees, lblInternationalLicenseID,
        /// lblLocalLicenseID, lblExpirationDate, and lblCreatedBy. On failure, displays an error MessageBox, calls
        /// ResetDefaultValues, and returns false.</remarks>
        /// <param name="InternationalLicenseID">Identifier of the international license to load.</param>
        /// <returns>True if the license was found and UI labels were populated; otherwise false.</returns>

        [DocInfo("Queries international license domain entity by ID and populates UI display labels.")]
        public bool LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;

            _InternationalLicenseInfo =  ClsInternationalLicenseBusiness.FindInternationalLicenseByID(InternationalLicenseID); 

            if(_InternationalLicenseInfo == null)
            {
                MessageBox.Show("Error: No International License found with ID = " + InternationalLicenseID,
                                "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDefaultValues();
                return false;
            }

            lblInternationalApplicationID.Text = "???";
            lblAppDate.Text = _InternationalLicenseInfo.Appinfo.ApplicationDate.ToShortDateString();
            lblIssueDate.Text = _InternationalLicenseInfo.IssueDate.ToLongDateString();
            lblFees.Text = _InternationalLicenseInfo.Appinfo.PaidFees.ToString();
            lblInternationalLicenseID.Text = "???";
            lblLocalLicenseID.Text = _InternationalLicenseInfo.IssuedUsingLocalLicenseID.ToString();
            lblExpirationDate.Text = _InternationalLicenseInfo.ExpirationDate.ToShortDateString();
            lblCreatedBy.Text = _InternationalLicenseInfo.Appinfo.UserInfo.UserName;

            return true; 
        }

        /// <summary>
        /// Loads application fee data and initializes default application, issue, and expiration dates and creator
        /// information for a new international license.
        /// </summary>
        /// <remarks>Updates UI labels for local license ID, application date, issue date, expiration date
        /// (one year from the current date), created-by, and fees retrieved from the international application type
        /// when available; leaves international application and license IDs as placeholders if not set.</remarks>
        /// <param name="LocalLicenseID">Local license identifier.</param>
        /// <param name="CurrentUserName">Name of the current user.</param>

        [DocInfo("Loads application fee structures and initializes default issue/expiration dates for a new international license.")]
        public void LoadDefaultApplicationData(int LocalLicenseID, string CurrentUserName)
        {
            int InternationalApplicationTypeID = 6;
            ClsApplicationTypeBusiness AppType = ClsApplicationTypeBusiness.FindApplicationTypeByID(InternationalApplicationTypeID);
            

            // Make sure these label variable names match your UI properties perfectly!
            lblLocalLicenseID.Text = LocalLicenseID.ToString();
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblCreatedBy.Text = CurrentUserName;

            if (AppType != null)
            {
               
                lblFees.Text = AppType.ApplicationFees.ToString("0.00");
            }

            lblInternationalApplicationID.Text = "???";
            lblInternationalLicenseID.Text = "???";
        }
        #endregion

        #region Private Event Handlers
        private void ucInternationalDrivingLicenseAppInfo_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}
