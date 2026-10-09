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
    /// Provides a reusable UserControl for driving license lookup that accepts numeric-only license ID input and raises
    /// the OnSearchClick event with the parsed license ID.
    /// </summary>
    /// <remarks>Enforces digit-only input in the license ID textbox, ignores empty or whitespace input, and
    /// invokes OnSearchClick when a valid integer license ID is submitted. Use SetLicenseIDToTextBox to
    /// programmatically populate the filter.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable search filter control for driving license lookup operations with numeric input constraints.", Module = "License Management", Version = "1.0")]
    public partial class ucLicenseSearchFilter : UserControl
    {
        #region Custom Events

        /// <summary>
        /// Occurs when a search is requested and provides an int that identifies the search or conveys a related
        /// parameter.
        /// </summary>
        /// <remarks>The integer argument typically represents a search identifier or a search-related
        /// parameter such as a page index.</remarks>
        public event Action<int>OnSearchClick;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucLicenseSearchFilter class and initializes its user-interface components.
        /// </summary>
        /// <remarks>Required for Windows Forms designer support.</remarks>
        public ucLicenseSearchFilter()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI  & Helper Methods

        /// <summary>
        /// Populates the filter textbox with the specified license ID.
        /// </summary>
        /// <remarks>Converts the value to its string representation and assigns it to the filter
        /// textbox's Text. Must be called on the UI thread.</remarks>
        /// <param name="licenseID">License identifier to assign to the filter textbox.</param>

        [DocInfo("Populates the filter textbox with a specified License ID value.")]
        public void SetLicenseIDToTextBox(int licenseID)
        {
            txtLicenseIDFilter.Text = licenseID.ToString();
        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Handles the search button Click event and raises OnSearchClick when the license ID input contains a valid
        /// integer.
        /// </summary>
        /// <remarks>Ignores empty or whitespace input; trims the input before parsing and only invokes
        /// OnSearchClick when parsing succeeds.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtLicenseIDFilter.Text)) return;


            if (int.TryParse(txtLicenseIDFilter.Text.Trim(), out int licenseID))
            {

                OnSearchClick?.Invoke(licenseID);
            }
        }

        /// <summary>
        /// Prevents non-digit and non-control characters from being entered into the control.
        /// </summary>
        /// <remarks>Allows only numeric input and control keys (for example, Backspace) by marking all
        /// other characters as handled.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">KeyPressEventArgs containing the key character; Handled is set to true for non-digit and non-control
        /// characters.</param>
        private void txtLicenseIDFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ucLicenseSearchFilter_Load(object sender, EventArgs e)
        {

        }

         private void gbFilter_Enter(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
