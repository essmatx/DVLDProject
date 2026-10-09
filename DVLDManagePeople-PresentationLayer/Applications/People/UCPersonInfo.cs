using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
   /// <summary>
   /// Reusable WinForms UserControl that displays a person’s demographic profile, nationality and profile image, and
   /// provides UI affordances to load, reset and edit the displayed person.
   /// </summary>
   /// <remarks>LoadPersonInfo looks up a person by ID via ClsPerson.FindPersonByID; if no person is found the
   /// control shows an error message and resets its display. The control displays 'Unknown' when the country lookup
   /// fails. If ImagePath is set and the file exists, the image is shown. ResetPersonInfo clears displayed fields and
   /// resets the internal person identifier to -1. Editing the person opens frmAddUpdatePerson.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable presentation control for displaying person demographic profiles, nationality lookup, and image media.", Module = "People Management", Version = "1.0")]
    public partial class UCPersonInfo : UserControl
    {
        #region Private Fields
        /// <summary>
        /// Person instance used internally by the class.
        /// </summary>
        private ClsPerson _person;

        /// <summary>
        /// Backing field that stores the person's identifier.
        /// </summary>
        /// <remarks>Initialized to -1 to indicate an unassigned identifier.</remarks>
        private int _PersonID = -1;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the person's identifier.
        /// </summary>
        /// <remarks>Read-only; backed by the _PersonID field.</remarks>
        public int PerosnID
        {
            get { return _PersonID; }

        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UCPersonInfo control and configures its UI components.
        /// </summary>
        /// <remarks>Performs designer-generated component initialization.</remarks>
        public UCPersonInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Data Loading & Reset API

        /// <summary>
        /// Fetches a person by ID and populates UI labels with the person's details, resolves the country name, and
        /// sets the picture box image when an existing image path is available.
        /// </summary>
        /// <remarks>Shows an error message and calls ResetPersonInfo when no matching person is found.
        /// Maps stored gender value to "Male" or "Female". Uses clsCountry.FindCountryByID to resolve the nationality
        /// name and falls back to "Unknown" if not found. Sets pbPersonImage.ImageLocation only when ImagePath is not
        /// empty and the file exists.</remarks>
        /// <param name="PersonID">Person identifier to load.</param>
        [DocInfo("Fetches person by ID, populates UI labels, maps country entity, handles image rendering, and raises OnPersonSelected event.")]
        public void LoadPersonInfo(int PersonID)
        {
            _person = ClsPerson.FindPersonByID(PersonID);

            if (_person == null)
            {
                MessageBox.Show($"No Person with ID = {PerosnID} found in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();
                return;
            }

            lblPersonID.Text = _person.PersonID.ToString();
            lblName.Text = $"{_person.FirstName} {_person.SecondName} {_person.ThirdName} {_person.LastName}";
            lblNationalNo.Text = _person.NationalNo;
            lblGendor.Text = (_person.Gendor == 0) ? "Male" : "Female";
            lblEmail.Text = _person.Email;
            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _person.Phone;
            lblCountry.Text = _person.NationalityCountryID.ToString();
            lblAddress.Text = _person.Address;

            clsCountry Country = clsCountry.FindCountryByID(_person.NationalityCountryID);

            if (Country != null)
            {
                lblCountry.Text = Country.CountryName;
            }
            else
            {
                lblCountry.Text = "Unknown";
            }


            if (!string.IsNullOrEmpty(_person.ImagePath) && File.Exists(_person.ImagePath))
            {
                pbPersonImage.ImageLocation = _person.ImagePath;
            }
            else
            {
            }
        }

        /// <summary>
        /// Reset person-related UI labels to default placeholders and clear the stored person identifier.
        /// </summary>
        /// <remarks>Sets the underlying identifier field to -1 and updates all person display labels to
        /// the "???" placeholder. Changes affect in-memory fields and the UI only and are not persisted.</remarks>
        [DocInfo("Clears label values to default indicators and releases person entity references.")]
        public void ResetPersonInfo()
        {
            _PersonID = -1;


            lblPersonID.Text = "???";
            lblName.Text = "???";
            lblEmail.Text = "???";
            lblGendor.Text = "???";
            lblNationalNo.Text = "???";
            lblPhone.Text = "???";
            lblDateOfBirth.Text = "???";
            lblCountry.Text = "???";


        }
        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Initialize control state on load; reset person information when no person is selected.
        /// </summary>
        /// <remarks>If _PersonID equals -1, calls ResetPersonInfo to clear the displayed person
        /// data.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void UCPersonInfo_Load(object sender, EventArgs e)
        {
            if(_PersonID == -1)
            {
                ResetPersonInfo(); 
            }
        }

        /// <summary>
        /// Handles the Enter event of the GroupBox control.
        /// </summary>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Shows a modal Add/Update Person dialog for the current person.
        /// </summary>
        /// <remarks>Passes the current person's ID to the dialog and displays it modally.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A LinkLabelLinkClickedEventArgs that contains the event data.</param>
        private void linklblEditPersoninfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_person.PersonID);

            frm.ShowDialog(); 
        }

        /// <summary>
        /// Handles the Paint event for guna2GradientPanel1 and performs custom drawing using the provided
        /// PaintEventArgs.
        /// </summary>
        /// <remarks>Use e.Graphics for all rendering and dispose any created GDI+ objects. Keep drawing
        /// operations lightweight to avoid blocking the UI thread.</remarks>
        /// <param name="sender">The source of the Paint event.</param>
        /// <param name="e">Provides paint data, including the Graphics object and clip rectangle.</param>
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Opens the Add/Update Person form for the current person and displays it modally.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnEditPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_person.PersonID);

            frm.ShowDialog();
        }

        #endregion
    }
}
