
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
    /// UserControl that displays and manages preview and metadata for lost or damaged license replacement applications,
    /// including old license details, replacement IDs, application date, creator, and fees.
    /// </summary>
    /// <remarks>Reusable presentation-layer component for Driving Licenses (Module) version 1.0. Populate
    /// displayed data via LoadApplicationInfoForReplacement(ClsLicenseBusiness, applicationTypeID) to set the old
    /// license and fees, and call UpdateReplacementApplicationInfo(lrApplicationID, replacedLicenseID) to record
    /// replacement identifiers. Use ResetApplicationInfo() to clear state. Intended for use on the UI thread.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable component for displaying and managing application preview details for lost/damaged license replacement operations.", Module = "Driving Licenses", Version = "1.0")]
    public partial class ucApplicationInforLicenseReplacement : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier of the LR application.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unset or unknown identifier.</remarks>
        private int _LRApplicationID = -1;

        /// <summary>
        /// Identifier of the license that replaces the original license.
        /// </summary>
        /// <remarks>Default value is -1 to indicate no replacement has been set.</remarks>
        private int _ReplacedLicenseID = -1;

        /// <summary>
        /// Previous license information used for comparison and rollback.
        /// </summary>
        /// <remarks>Initialized to null; assigned when license data is loaded or updated.</remarks>
        private ClsLicenseBusiness _OldLicenseInfo = null;
        #endregion

        #region Public Properties

        /// <summary>
        /// Identifier of the previous license, or -1 if no previous license information is available.
        /// </summary>
        /// <remarks>Reads LicenseID from the backing _OldLicenseInfo; returns -1 when the backing
        /// instance is null.</remarks>
        public int OldLicenseID { get { return _OldLicenseInfo?.LicenseID ?? -1; } }

        /// <summary>
        /// Gets the LR application identifier.
        /// </summary>
        public int LRApplicationID { get { return _LRApplicationID; } }

        /// <summary>
        /// Identifier of the license that was replaced.
        /// </summary>
        public int ReplacedLicenseID { get { return _ReplacedLicenseID; } }

        /// <summary>
        /// Gets the previous license information associated with the instance.
        /// </summary>
        /// <remarks>May be null if no previous license information is available.</remarks>
        public ClsLicenseBusiness OldLicenseInfo { get { return _OldLicenseInfo; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucApplicationInforLicenseReplacement class.
        /// </summary>
        /// <remarks>Sets up the control's UI components by calling InitializeComponent.</remarks>
        public ucApplicationInforLicenseReplacement()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Replacement Workflow 

        /// <summary>
        /// Retrieve application type details from the business layer and update the application fees label.
        /// </summary>
        /// <remarks>Updates lblApplicationFees.Text with the application fees formatted to two decimal
        /// places ("0.00"); sets the label to "N/A" if no matching application type is found.</remarks>
        /// <param name="applicationTypeID">Identifier of the application type to retrieve.</param>
        [DocInfo("Retrieves application type details from business layer and updates application fees label.")]
        public void UpdateFees(int applicationTypeID)
        {
            ClsApplicationTypeBusiness appType = ClsApplicationTypeBusiness.FindApplicationTypeByID(applicationTypeID); 

            if(appType != null)
            {
                lblApplicationFees.Text = appType.ApplicationFees.ToString("0.00");
            }
            else
            {
                lblApplicationFees.Text = "N/A"; 
            }
        }


        /// <summary>
        /// Populate original license details, user tracking information, application date, and fee metadata for a
        /// replacement application.
        /// </summary>
        /// <remarks>Updates UI labels (old license ID, application date, created by) and invokes
        /// UpdateFees. On null input it calls ResetApplicationInfo and displays an error MessageBox.</remarks>
        /// <param name="oldLicense">Original license information to use as the source for populating the replacement. If null, the method resets
        /// application state and shows an error message.</param>
        /// <param name="applicationTypeID">Application type identifier used to update associated fee metadata.</param>
        [DocInfo("Populates original license details, user tracking info, application date, and fee metadata.")]
        public void LoadApplicationInfoForReplacement(ClsLicenseBusiness oldLicense, int applicationTypeID)
        {
            if(oldLicense == null)
            {
                MessageBox.Show("Failed to load license details: Data is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetApplicationInfo();
                return;
            }

            _OldLicenseInfo = oldLicense;

            lblOldLicenseID.Text = _OldLicenseInfo.LicenseID.ToString();
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = _OldLicenseInfo.Userinfo.UserName;

            UpdateFees(applicationTypeID);
        }

        /// <summary>
        /// Binds the replacement application ID and replaced license ID to their corresponding display labels and
        /// updates the backing fields.
        /// </summary>
        /// <param name="lrApplicationID">Replacement application primary key.</param>
        /// <param name="replacedLicenseID">Replaced license primary key.</param>
        [DocInfo("Binds post-replacement transaction primary keys (Application ID and Replaced License ID) to display labels.")]
        public void UpdateReplacementApplicationInfo(int lrApplicationID, int replacedLicenseID)
        {
            _LRApplicationID = lrApplicationID;
            _ReplacedLicenseID = replacedLicenseID;

            lblLicenseReplacementAppID.Text = _LRApplicationID.ToString();

            lblReplacedLicneseID.Text = _ReplacedLicenseID.ToString(); 
        }

        /// <summary>
        /// Resets form input controls, clears application fee indicators, and releases license-related entity
        /// references.
        /// </summary>
        /// <remarks>Sets internal license identifiers to sentinel values, clears the cached previous
        /// license info, and updates UI labels to placeholder values.</remarks>
        [DocInfo("Resets form input controls, application fee indicators, and releases license entity references.")]
        public void ResetApplicationInfo()
        {
            _LRApplicationID = -1;
            _ReplacedLicenseID = -1;
            _OldLicenseInfo = null;

            lblLicenseReplacementAppID.Text = "[???]";
            lblApplicationDate.Text = "[???]";
            lblApplicationFees.Text = "[???]";
            lblReplacedLicneseID.Text = "[???]";
            lblOldLicenseID.Text = "[???]";
            lblCreatedBy.Text = "[???]";
        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Initialize control state and perform setup when the control is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void ucApplicationInforLicenseReplacement_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Performs custom rendering for the guna2GradientPanel1 control during its Paint event.
        /// </summary>
        /// <remarks>Avoid expensive work inside the Paint handler; use e.ClipRectangle to limit drawing
        /// to the invalidated area and dispose any created GDI+ objects.</remarks>
        /// <param name="sender">The source of the paint event.</param>
        /// <param name="e">PaintEventArgs that provides the Graphics surface and clip rectangle for drawing.</param>
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}
