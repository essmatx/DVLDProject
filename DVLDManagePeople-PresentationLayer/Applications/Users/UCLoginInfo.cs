
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
    /// Represents a reusable Windows Forms UserControl that displays a user’s login metadata, active status, and
    /// primary key identifiers.
    /// </summary>
    /// <remarks>Call LoadByUserID(int) to load and display a user; ResetUserInfo clears the control and
    /// internal state. SelectedUserInfo exposes the loaded business object and UserID exposes the current user
    /// identifier. The control shows an error MessageBox when a user cannot be found.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable presentation control for displaying user account login metadata, active status, and primary key identifiers.", Module = "User Management", Version = "1.0")]
    public partial class UCLoginInfo : UserControl
    {
        #region Private Fields

        /// <summary>
        /// User identifier.
        /// </summary>
        /// <remarks>Initialized to -1 to represent an unset or unknown user.</remarks>
        private int _UserID = -1;

        /// <summary>
        /// Cached instance of ClsUserBuiness used for user-related business logic.
        /// </summary>
        /// <remarks>Access restricted to the declaring type.</remarks>
        private ClsUserBuiness _User ;
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the user's identifier.
        /// </summary>
        /// <remarks>Read-only; returns the underlying backing field.</remarks>
        public int UserID => _UserID;

        /// <summary>
        /// Gets the currently selected user's business object.
        /// </summary>
        /// <remarks>May be null if no user is selected.</remarks>
        public ClsUserBuiness SelectedUserInfo => _User;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UCLoginInfo class.
        /// </summary>
        public UCLoginInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Data Loading

        /// <summary>
        /// Loads the user with the specified ID, populates account labels and active status, and raises the
        /// OnUserSelected event.
        /// </summary>
        /// <remarks>If no user is found, displays an error message. When a user is found, populates UI
        /// account labels and active status and raises the OnUserSelected event.</remarks>
        /// <param name="UserID">The user identifier to load.</param>
        [DocInfo("Fetches user by ID, populates UI account labels, active status, and raises OnUserSelected event.")]
        public void LoadByUserID(int UserID)
        {
            _UserID = UserID;

            _User = ClsUserBuiness.FindUserByID(_UserID);

            if (_User == null)
            {
                MessageBox.Show($"No user found with ID{UserID}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            _FileUserInfo(); 
        }

        /// <summary>
        /// Resets stored user information by clearing the internal user ID and user reference, and setting the
        /// associated UI labels to default placeholder values.
        /// </summary>
        /// <remarks>Clears _UserID to -1 and _User to null, and sets lblUserID.Text, lblUsername.Text,
        /// and lblIsActive.Text to '???'. Does not persist changes; should be invoked on the UI thread.</remarks>

        [DocInfo("Clears label values to default indicators and releases internal user entity references.")]
        public void ResetUserInfo()
        {
            _UserID = -1;
            _User = null;

            lblUserID.Text = "???";
            lblUsername.Text = "???";
            lblIsActive.Text = "???";
        }

        #endregion

        #region Private Visual & Loading Helpers

        /// <summary>
        /// Binds the user's UserID, UserName, and IsActive properties to corresponding display labels.
        /// </summary>
        /// <remarks>Converts UserID to a string and formats IsActive as 'Yes' or 'No'. Assumes _User and
        /// the label controls are initialized and not null.</remarks>
        [DocInfo("Binds user object properties (UserID, Username, IsActive) to display labels.")]
        private void _FileUserInfo()
        {
            lblUserID.Text = _User.UserID.ToString();

            lblUsername.Text = _User.UserName;

            if (_User.IsActive)
            {
                lblIsActive.Text = "Yes"; 
            }
            else
            {
                lblIsActive.Text = "No"; 
            }
        }

        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Initializes the UCLoginInfo control when it is loaded.
        /// </summary>
        /// <remarks>Initialize control state, load data, and attach event handlers as needed.</remarks>
        /// <param name="sender">The source of the load event.</param>
        /// <param name="e">Event data for the load event.</param>
        private void UCLoginInfo_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
