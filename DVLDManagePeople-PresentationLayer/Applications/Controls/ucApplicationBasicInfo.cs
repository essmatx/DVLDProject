
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
    /// <summary>
    /// Composite UserControl that displays basic application information, status metrics, fees, and a link to the
    /// applicant's person record.
    /// </summary>
    /// <remarks>Populate the control by calling LoadApplicantData(int); call Reset() to clear displayed
    /// values. Exposes read-only LocalDrivingLicenseApplicationID and PersonID. Raises OnViewPersonInfoClick when the
    /// user requests to view applicant details. Intended for use on the UI thread.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable composite control rendering basic application details, status metrics, and applicant person linkage.", Module = "Application Management", Version = "1.0")]
    public partial class ucApplicationBasicInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// Identifier for the local driving license application.
        /// </summary>
        /// <remarks>Default value is -1 to indicate an uninitialized or absent application.</remarks>
        private int _LocalDrivingLicenseApplicationID = -1;

        /// <summary>
        /// Backing field that stores the person's identifier.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unassigned identifier.</remarks>
        private int _PersonID = -1;
        #endregion

        #region Custom Events
        /// <summary>
        /// Raised when a request to view a person's information occurs; the handler receives the person's identifier.
        /// </summary>
        /// <remarks>Consider using EventHandler<TEventArgs> to follow the .NET event pattern instead of a
        /// raw Action<int>.</remarks>
        public event Action<int> OnViewPersonInfoClick;
        #endregion

        #region Scalar & Navigation Properties

        /// <summary>
        /// Gets the identifier of the local driving license application.
        /// </summary>
        /// <remarks>Read-only value backed by a private field; assigned elsewhere and used to correlate
        /// with application records.</remarks>
        public int LocalDrivingLicenseApplicationID => _LocalDrivingLicenseApplicationID;

        /// <summary>
        /// Gets the person's unique identifier.
        /// </summary>
        /// <remarks>Read-only. Backed by the _PersonID backing field.</remarks>
        public int PersonID => _PersonID;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of ucApplicationBasicInfo and its UI components.
        /// </summary>
        /// <remarks>Calls InitializeComponent to configure the control's UI elements; intended for
        /// designer support.</remarks>
        public ucApplicationBasicInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & Data Loading

        /// <summary>
        /// Resets internal identifiers and application info labels to their default placeholder values.
        /// </summary>
        /// <remarks>Sets _PersonID and _LocalDrivingLicenseApplicationID to -1 and restores placeholder
        /// text ("???:") for labels such as lblApplicant, lblCreatedBy, lblDate, lblFees,
        /// lblLocalDrivingLicenseApplicationID, lblStatus, lblStatusDate, and lblType.</remarks>
        [DocInfo("Clears state properties and restores default placeholder text across all application info labels.")]
        public void Reset()
        {
            _PersonID = -1;

          

            _LocalDrivingLicenseApplicationID = -1; 

            lblApplicant.Text = "???"; 

            lblCreatedBy.Text = "???";

            lblDate.Text = "???";

            lblFees.Text = "???";

            lblLocalDrivingLicenseApplicationID.Text = "???";

            lblStatus.Text = "???"; ; 

            lblStatusDate.Text = "???";

            lblType.Text = "???";


        }

        /// <summary>
        /// Queries the application domain for the specified ID and populates UI controls with the application's date,
        /// status, fees, type, creator, and applicant name.
        /// </summary>
        /// <remarks>Sets internal fields (_LocalDrivingLicenseApplicationID and _PersonID), updates UI
        /// labels, and displays an error MessageBox if no application is found. If the ID is -1 the UI is reset without
        /// querying the data source.</remarks>
        /// <param name="LocalDrivingLicenseApplicationID">Local driving license application identifier; use -1 to clear the UI and reset state.</param>
        [DocInfo("Queries application domain entity and populates UI controls with application and person details.")]
        public void LoadApplicantData(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            if(LocalDrivingLicenseApplicationID == - 1)
            {
                Reset();
                return; 
            }

            ClsLocalDrivingLicenseAppBusiness LocalDrivingLicenseApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApp == null)
            {
                Reset();
                MessageBox.Show("No Application found with ID = " + _LocalDrivingLicenseApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            lblLocalDrivingLicenseApplicationID.Text = LocalDrivingLicenseApp.AppInfo.ApplicationID.ToString();

            lblDate.Text = LocalDrivingLicenseApp.AppInfo.ApplicationDate.ToShortDateString();

            lblStatusDate.Text = LocalDrivingLicenseApp.AppInfo.LastStatus.ToShortDateString();

            lblFees.Text = LocalDrivingLicenseApp.AppInfo.PaidFees.ToString("F2");

            _PersonID = LocalDrivingLicenseApp.AppInfo.ApplicationPersonID;

          

             var person = LocalDrivingLicenseApp.AppInfo.PersonInof;
            if (person != null)
            {
                string thirdName = string.IsNullOrEmpty(person.ThirdName) ? "" : person.ThirdName + " ";
                lblApplicant.Text = $"{person.FirstName} {person.SecondName} {thirdName}{person.LastName}";
            }

            lblType.Text = "New Local Driving License Application";

            lblCreatedBy.Text = LocalDrivingLicenseApp.AppInfo.UserInfo.UserName;  



            switch (LocalDrivingLicenseApp.AppInfo.AppStatus)
            {
                case ClsApplicationBusiness.enApplicationStatus.New:
                    lblStatus.Text = "New";
                    break;
                case ClsApplicationBusiness.enApplicationStatus.Cancelled:
                    lblStatus.Text = "Canceled";
                    break;
                case ClsApplicationBusiness.enApplicationStatus.Completed:
                    lblStatus.Text = "Completed";
                    break;
            }
        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Opens a modal person-details dialog for the current person and raises the OnViewPersonInfoClick event.
        /// </summary>
        /// <remarks>Invokes OnViewPersonInfoClick with the current person ID, creates a frmShowPersonInfo
        /// initialized with that ID, and displays it modally.</remarks>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            OnViewPersonInfoClick?.Invoke(_PersonID);

            frmShowPersonInfo frm = new frmShowPersonInfo(_PersonID);

            frm.ShowDialog();
        }
        private void ucApplicationBasicInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void lnklblPersoninfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion
    }
}
