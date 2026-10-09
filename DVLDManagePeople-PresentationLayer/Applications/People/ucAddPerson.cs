
using DVLD_Business; 
using DVLDManagePeople_PresentationLayer.Global_Classes; 
using DVLDManagePeople_PresentationLayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer
{
    /// <summary>
    /// UserControl that provides a reusable form for creating and editing person demographic records, including name,
    /// contact details, nationality, date of birth, image handling, validation, and saving to persistence.
    /// </summary>
    /// <remarks>Designed for WinForms use. Operates in two modes (AddNew, Update); call LoadData(int) to
    /// populate an existing record or ResetDefaultValues() for a new entry. Validates required fields, email format,
    /// national number uniqueness, and date input using the format "yyyy-MM-dd". Manages selecting, copying, and
    /// removing person images and copies images into the project image folder when appropriate. Raises the
    /// OnSaveSuccess event with the saved PersonID after a successful save. Not thread-safe; interact with the control
    /// from the UI thread.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Reusable form component for managing person demographic records, country lookups, image handling, and validation.", Module = "People Management", Version = "1.0")]
    public partial class ucAddPerson : UserControl
    {
        #region Custom Delegates & Events

        /// <summary>
        /// Represents a method that handles an event that provides a person's identifier.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="PersonID">The identifier of the person associated with the event.</param>
        public delegate void DataBackEventHandler(object sender, int PersonID);

        /// <summary>
        /// Occurs when a save operation completes successfully.
        /// </summary>
        /// <remarks>Subscribers are notified with a DataBackEventHandler that provides information about
        /// the saved data and the operation result.</remarks>
        public event DataBackEventHandler OnSaveSuccess;
        #endregion

        #region Nested Enums
        /// <summary>
        /// Specifies whether an operation creates a new item or updates an existing one.
        /// </summary>
        /// <remarks>Indicates the mode for data operations: _AddNew creates a new item; _Update updates
        /// an existing item.</remarks>
        public enum enMode { _AddNew = 0, _Update = 1 };
        #endregion

        #region Private Fields

        /// <summary>
        /// Current operation mode.
        /// </summary>
        /// <remarks>Initialized to enMode._AddNew.</remarks>
         enMode _Mode = enMode._AddNew;

        /// <summary>
        /// Unique identifier for the person.
        /// </summary>
        private int _PersonID;

        /// <summary>
        /// The person associated with the object.
        /// </summary>
        /// <remarks>Typically used as the backing field for a Person property.</remarks>
        private ClsPerson _person;

        /// <summary>
        /// Backing field that stores the file path of the selected image.
        /// </summary>
        /// <remarks>Initialized to an empty string and serves as the backing store for the corresponding
        /// SelectedImagePath property.</remarks>
        private string _SelectedImagePath = "";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the date of birth displayed in the associated text box. The getter parses the text and returns
        /// DateTime.Today if the text is empty or invalid; the setter writes the date using the 'yyyy-MM-dd' format.
        /// </summary>
        /// <remarks>The getter uses DateTime.TryParse on the trimmed text. The setter overwrites the text
        /// box with a formatted date string and does not perform additional validation.</remarks>
        public DateTime DateOfBirthValue
        {
            get
            {
                // Try parsing whatever the user typed into a real DateTime object
                if (DateTime.TryParse(txtDateOfBirth.Text.Trim(), out DateTime parsedDate))
                {
                    return parsedDate;
                }

                // Fallback default date if the box is empty or invalid
                return DateTime.Today;
            }
            set
            {
                // Allows you to load an existing date from your database into the text box easily
                txtDateOfBirth.Text = value.ToString("yyyy-MM-dd");
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ucAddPerson class and its UI components.
        /// </summary>
        public ucAddPerson()
        {
            InitializeComponent();
        }
        #endregion

        #region Public UI & Persistence
        /// <summary>
        /// Load a person by ID, bind the person's data to UI controls, set update mode and internal identifiers, and
        /// load the person's image if present.
        /// </summary>
        /// <remarks>Shows an error message if no person is found; sets SelectedImagePath, _PersonID and
        /// update mode; populates name, contact, address, national number, internal ID and date of birth fields; sets
        /// national number field read-only; sets gender radio button and selects nationality; loads the picture into
        /// the picture box if the image file exists.</remarks>
        /// <param name="PersonID">Identifier of the person to load.</param>
        [DocInfo("Loads person record by ID, binds attributes to form controls, and configures update mode.")]
        public void LoadData(int PersonID)
        {
           
            _person = ClsPerson.FindPersonByID(PersonID);

            if(_person == null)
            {
                MessageBox.Show("No Person with ID " + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }

            _SelectedImagePath = _person.ImagePath;

            this._Mode = enMode._Update;

            _PersonID = PersonID;

            txtFIrstName.Text = _person.FirstName;
             txtSecondName.Text = _person.SecondName;
            textthirdName.Text = _person.ThirdName;
            textLastName.Text = _person.LastName;
            txtNationalNo.Text = _person.NationalNo;
            txtEmail.Text = _person.Email;
            txtPhone.Text = _person.Phone;
            txtAddress.Text = _person.Address;
            txtInternalPID.Text = _person.PersonID.ToString(); 
            //dtpDateOfBirth.Value = _person.DateOfBirth;

            DateOfBirthValue = _person.DateOfBirth;

            txtNationalNo.ReadOnly = true; 
            

            if (_person.Gendor == 0) rbMale.Checked = true; else rbFemale.Checked = true;

            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.FindCountryByID(_person.NationalityCountryID).CountryName); 

            if (_person.ImagePath != "" && File.Exists(_person.ImagePath))
            {
                pbPersonImage.Load(_person.ImagePath);
            }

       
        }

        /// <summary>
        /// Resets form inputs, restores default country and gender selections, sets mode to AddNew, and initializes
        /// person-related fields.
        /// </summary>
        /// <remarks>Sets _Mode to enMode._AddNew, sets _PersonID to -1, creates a new ClsPerson, clears
        /// text fields, checks the male radio button, sets DateOfBirthValue to 18 years before today, and selects
        /// "Egypt" in the country combo box.</remarks>
        [DocInfo("Resets form inputs, restores default country/gender selections, and sets mode to AddNew.")]
        public void ResetDefaultValues()
        {
            this._Mode = enMode._AddNew;

            _PersonID = -1;

            _person = new ClsPerson();

            txtFIrstName.Text = "";
            txtSecondName.Text = "";
            textthirdName.Text = "";
            textLastName.Text = "";
            txtNationalNo.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";

            rbMale.Checked = true;



            DateOfBirthValue = DateTime.Now.AddYears(-18);

            //dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;


            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");

            //pbPersonImage.ImageLocation = null; 
        }

        /// <summary>
        /// Validate form input, process the selected image, populate and persist the person entity, and raise
        /// OnSaveSuccess on successful save.
        /// </summary>
        /// <remarks>Shows message boxes for validation failures and save results, creates a new ClsPerson
        /// when in AddNew mode, updates _person properties from the form controls, updates _Mode to Update on success,
        /// and invokes OnSaveSuccess with the saved PersonID.</remarks>
        /// <returns>true if the person entity was saved successfully; otherwise false.</returns>

        [DocInfo("Executes form validation, image file handling, entity save, and triggers OnSaveSuccess event.")]
        public bool Save()
        {

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are invalid. Please hover over the red icons to check details.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (this._Mode == enMode._AddNew)
            {
                _person = new ClsPerson();
            }

            if (!_HandlePersonImage())
            {
                return false;
            }

            _person.FirstName = txtFIrstName.Text;
            _person.SecondName = txtSecondName.Text;
            _person.ThirdName = textthirdName.Text;
            _person.LastName = textLastName.Text;
            _person.NationalNo = txtNationalNo.Text;
            _person.Email = txtEmail.Text;
            _person.Phone = txtPhone.Text;
            _person.Address = txtAddress.Text;
            //_person.DateOfBirth = dtpDateOfBirth.Value;
            _person.DateOfBirth = DateOfBirthValue;

            _person.Gendor = rbMale.Checked ? (short)0 : (short)1;

            _person.NationalityCountryID = (int)cbCountry.SelectedValue;

            _person.ImagePath = _SelectedImagePath;

            if (_person.Save())
            {
                MessageBox.Show("Data Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this._Mode = enMode._Update;

                OnSaveSuccess?.Invoke(this, _person.PersonID);
                return true;
            }
            else
            {
                MessageBox.Show("Error: Data Faild to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

        }

        #endregion

        #region Private Data Loading & Image Management

        /// <summary>
        /// Populates the country ComboBox (cbCountry) by binding a DataTable of countries and setting DisplayMember to
        /// 'CountryName' and ValueMember to 'CountryID'.
        /// </summary>
        /// <remarks>Retrieves countries from clsCountry.GetAllCountries(), validates that the DataTable
        /// contains 'CountryName' and 'CountryID' columns before binding. If validation fails, clears the ComboBox
        /// binding and writes a diagnostic message listing available columns.</remarks>
        private void __FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            // Guard: ensure the table and expected columns exist
            if (dtCountries != null
                && dtCountries.Columns.Contains("CountryName")
                && dtCountries.Columns.Contains("CountryID"))
            {
                cbCountry.DisplayMember = ""; // clear first to avoid intermediate binding issues
                cbCountry.ValueMember = "";
                cbCountry.DataSource = dtCountries;
                cbCountry.DisplayMember = "CountryName";
                cbCountry.ValueMember = "CountryID";
            }
            else
            {
                // Safe fallback: unbind and optionally log diagnostic info
                cbCountry.DataSource = null;
                cbCountry.DisplayMember = "";
                cbCountry.ValueMember = "";

                // Example: replace with your logging mechanism
                System.Diagnostics.Debug.WriteLine("Warning: dtCountries is null or missing expected columns. Columns present: "
                    + (dtCountries == null ? "null" :
                       string.Join(", ", dtCountries.Columns.Cast<DataColumn>().Select(c => c.ColumnName))));
            }
        }

        /// <summary>
        /// Deletes an existing person image file if present, copies the current PictureBox image to the project's image
        /// folder, and updates the PictureBox image location.
        /// </summary>
        /// <remarks>IOExceptions raised during file deletion are ignored. On copy failure a message box
        /// is shown. The copy operation may modify the source path passed by reference.</remarks>
        /// <returns>true if the image was successfully handled or no action was required; false if copying the image failed.</returns>
        private bool _HandlePersonImage()
        {
            if (_person.ImagePath != "")
            {
                try
                {
                    File.Delete(_person.ImagePath);
                }
                catch (IOException)
                {

                }

                if (pbPersonImage.ImageLocation != null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImageFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Validation Handlers

        /// <summary>
        /// Validates that the TextBox specified by sender contains non-empty text; if empty, sets an error on
        /// errorProvider1 and sets e.Cancel to true.
        /// </summary>
        /// <param name="sender">The event source, expected to be a TextBox whose Text property is validated.</param>
        /// <param name="e">Provides the Cancel property; set to true when validation fails to prevent leaving the control.</param>
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox temp = (TextBox)sender;


            if (string.IsNullOrEmpty(temp.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }



        }

        /// <summary>
        /// Validate the first-name TextBox and cancel validation if its value is empty.
        /// </summary>
        /// <remarks>Delegates to ValidateEmptyTextBox to perform the empty-value check.</remarks>
        /// <param name="sender">The control that raised the validating event.</param>
        /// <param name="e">Provides data for the validating event; set Cancel to true to prevent the control from losing focus.</param>
        private void txtFIrstName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validates the national identification number input: requires a non-empty value, checks for an existing
        /// person when adding or when the value changed, sets ErrorProvider messages, and cancels validation on
        /// failure.
        /// </summary>
        /// <remarks>If the input is empty, sets the error "This field is required!" and cancels
        /// validation. If adding a new record or the value differs from the existing person's number, calls
        /// ClsPerson.DoesPersonExist to detect duplicates; if a duplicate is found, sets an error ("Wrong Number Please
        /// Try Again Again") and cancels validation; otherwise clears any error.</remarks>
        /// <param name="sender">The control that raised the Validating event.</param>
        /// <param name="e">CancelEventArgs instance used to cancel validation; set e.Cancel to true to prevent focus change when
        /// validation fails.</param>
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string enteredNationalNo = txtNationalNo.Text.Trim();

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (this._Mode == enMode._AddNew || _person.NationalNo != txtNationalNo.Text.Trim())
            {
                if (ClsPerson.DoesPersonExist(txtNationalNo.Text.Trim()))
                {
                    e.Cancel = true;

                    errorProvider1.SetError(txtNationalNo, "Wrong Number Please Try Again Again");
                }
                else
                {
                    errorProvider1.SetError(txtNationalNo, "");
                }
            }
        }

        /// <summary>
        /// Validates the second name text box and cancels validation if the field is empty.
        /// </summary>
        /// <remarks>Invokes ValidateEmptyTextBox to perform the actual validation.</remarks>
        /// <param name="sender">The control that raised the Validating event.</param>
        /// <param name="e">Provides the event data and allows cancellation by setting Cancel to true.</param>
        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validates the third-name text box and cancels the event when the value is empty.
        /// </summary>
        /// <remarks>Delegates the validation work to ValidateEmptyTextBox.</remarks>
        /// <param name="sender">The source of the event; typically the third-name TextBox control.</param>
        /// <param name="e">Provides validating event data and allows cancellation of the event by setting Cancel to true.</param>
        private void textthirdName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validates the last name text box on the Validating event and cancels validation if the field is empty.
        /// </summary>
        /// <remarks>Delegates to a shared helper to enforce non-empty input.</remarks>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">A CancelEventArgs instance that allows canceling the validation by setting Cancel to true.</param>
        private void textLastName_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validates that the email text box is not empty.
        /// </summary>
        /// <remarks>Delegates validation to ValidateEmptyTextBox.</remarks>
        /// <param name="sender">The control that raised the Validating event.</param>
        /// <param name="e">Event data for the validation operation; set Cancel to true to prevent focus change on validation failure.</param>
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validate the email address in txtEmail during the control's Validating event and set or clear an error on
        /// errorProvider1.
        /// </summary>
        /// <remarks>Calls ValidateEmptyTextBox to check for empty input, then validates txtEmail.Text
        /// against a regular-expression for standard email format and sets an error on errorProvider1 if validation
        /// fails (e.g., example@domain.com).</remarks>
        /// <param name="sender">The source of the event (the control being validated).</param>
        /// <param name="e">Provides event data and allows cancellation by setting Cancel to true when the email is invalid.</param>
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);

            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            var regex = new Regex(pattern);

            if (!regex.IsMatch(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email format! (e.g., example@domain.com)");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null); // Clear error if it passes matching rules
            }
        }

         /// <summary>
        /// Validates the selected country when the control is validating, allowing cancellation via the provided
        /// CancelEventArgs.
        /// </summary>
        /// <param name="sender">The control that raised the Validating event.</param>
        /// <param name="e">Provides data for the Validating event and allows cancellation of the operation.</param>
        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// Validates the address TextBox and cancels validation if it is empty.
        /// </summary>
        /// <remarks>Delegates to ValidateEmptyTextBox to check for an empty value and set e.Cancel when
        /// validation fails.</remarks>
        /// <param name="sender">The control that raised the Validating event.</param>
        /// <param name="e">Provides event data and allows cancellation of validation through the Cancel property.</param>
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
        }

        /// <summary>
        /// Validates the email entered in the associated control and updates the user interface to reflect the
        /// validation result.
        /// </summary>
        /// <remarks>Attach to the control's Validated event. Implement email format checks and update an
        /// ErrorProvider or other validation state as appropriate.</remarks>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">Event arguments for the validation event.</param>

        private void txtEmail_Validated(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Validates the date-of-birth text in txtDateOfBirth, ensuring it matches the 'yyyy-MM-dd' format and
        /// represents a plausible age (0-120). Displays an error message and cancels validation when invalid.
        /// </summary>
        /// <remarks>Empty input is allowed (no validation). Uses DateTime.TryParseExact with
        /// CultureInfo.InvariantCulture and DateTimeStyles.None. Calculates age and rejects values outside the 0-120
        /// range. Shows a MessageBox for format or logical errors.</remarks>
        /// <param name="sender">Source of the event, typically the TextBox control raising the Validating event.</param>
        /// <param name="e">Provides the ability to cancel validation; set Cancel to true to keep focus on the control when input is
        /// invalid.</param>
        private void guna2TextBox7_Validating(object sender, CancelEventArgs e)
        {
            string input = txtDateOfBirth.Text.Trim();

            // If the box is empty, handle it based on your form requirements
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // Define the exact format you want the user to type
            string format = "yyyy-MM-dd"; // Or "MM/dd/yyyy"

            // Attempt to parse the user's string into a real DateTime object
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Optional: Add logical restrictions (e.g., user must be at least 18 years old)
                int age = DateTime.Today.Year - parsedDate.Year;
                if (parsedDate.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 0 || age > 120)
                {
                    MessageBox.Show("Please enter a valid birth date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true; // Keeps the cursor in the textbox until fixed
                }
                else
                {
                    // Valid date! Store or use parsedDate here
                    // e.g., dbParameters.Add("@Dob", parsedDate);
                }
            }
            else
            {
                // Format was wrong (e.g., user typed text or wrong separators)
                MessageBox.Show($"Invalid format. Please use {format}.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // Prevents moving to the next field
            }
        }

        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Opens an OpenFileDialog filtered to common image formats and, if the user selects a file, loads the chosen
        /// image into the person picture box.
        /// </summary>
        /// <remarks>Accepts JPG, JPEG, PNG, GIF, and BMP files; stores the selected file path in
        /// _SelectedImagePath and loads the image into pbPersonImage when DialogResult is OK.</remarks>
        /// <param name="sender">The control that raised the event.</param>
        /// <param name="e">Event data for the link-click action.</param>
        private void linklblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedImagePath = openFileDialog.FileName;
                    pbPersonImage.Load(_SelectedImagePath); 
                }
            }
        }

        /// <summary>
        /// Handles text changes for the national number input, normalizes the value and triggers validation and related
        /// UI updates.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event data that enables cancelling the operation and provides validation context.</param>
        private void txtNationalNo_TextChanged(object sender, CancelEventArgs e)
        {
           
            
        }

        /// <summary>
        /// Processes command keys and invokes the Save action when Enter is pressed.
        /// </summary>
        /// <remarks>Pressing Enter anywhere in the control invokes the Save button and returns true to
        /// prevent the system alert sound. Otherwise the call is delegated to the base implementation.</remarks>
        /// <param name="msg">The Windows message to process.</param>
        /// <param name="keyData">The key data representing the pressed key(s).</param>
        /// <returns>true if the key was handled; otherwise false.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // If the user presses Enter anywhere inside this control, click Save
            if (keyData == Keys.Enter)
            {
                btnSave.PerformClick();
                return true; // This stops the annoying Windows 'ding' sound
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Clear the current person image and set the default image based on the selected gender.
        /// </summary>
        /// <remarks>Sets pbPersonImage.Image to null, then assigns the male or female default image
        /// resource according to rbMale.Checked.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The LinkLabelLinkClickedEventArgs that contains event data.</param>
        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            pbPersonImage.Image = null;

            // 2. Fall back to the correct default icon based on selected gender
            if (rbMale.Checked)
            {
                pbPersonImage.Image = Properties.Resources.Screenshot_2026_05_18_142413; // Change to your actual resource name
            }
            else
            {
                pbPersonImage.Image = Properties.Resources.Screenshot_2026_05_18_152210; // Change to your actual resource name
            }

            // 3. Hide the remove link since there's no custom image left to remove
           
        }

        /// <summary>
        /// Handles the Click event for iconDropDownButton1 and displays or toggles its associated dropdown menu.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">The event data for the click event.</param>
        private void iconDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the guna2Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles changes to the Date of Birth text input by parsing and validating the entered date and updating
        /// related UI or model state.
        /// </summary>
        /// <remarks>Parse using the application's expected date format and culture (prefer
        /// TryParse/TryParseExact). Update model or UI on successful parse and present validation feedback on failure;
        /// avoid long-running work on the UI thread.</remarks>
        /// <param name="sender">The source of the event, typically the TextBox whose Text changed.</param>
        /// <param name="e">Event arguments for the TextChanged event.</param>
        private void txtDateOfBirth_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Open a file dialog to select an image file and load the selected image into the person picture box.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void btnSetImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedImagePath = openFileDialog.FileName;
                    pbPersonImage.Load(_SelectedImagePath);
                }
            }
        }

        /// <summary>
        /// Clears the displayed person image and restores the appropriate default icon for the selected gender.
        /// </summary>
        /// <remarks>Resets the picture box image to null and applies a gender-specific fallback icon when
        /// available.</remarks>
        /// <param name="sender">The control that raised the click event.</param>
        /// <param name="e">The event data for the click event.</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            pbPersonImage.Image = null;

            // 2. Fall back to the correct default icon based on selected gender
            if (rbMale.Checked)
            {
                pbPersonImage.Image = null;
            }
            else
            {
                pbPersonImage.Image = null;
            }

        }

        /// <summary>
        /// Populate the countries combo box, apply default values for a new record, load person data for updates, and
        /// select the male radio button when the control loads.
        /// </summary>
        /// <remarks>Invokes __FillCountriesInComboBox; if _Mode is enMode._AddNew calls
        /// ResetDefaultValues; if _Mode is enMode._Update calls LoadData(_PersonID); then sets rbMale.Checked to
        /// true.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs instance that contains the event data.</param>
        private void UserControl1_Load(object sender, EventArgs e)
        {
            __FillCountriesInComboBox();

            if (this._Mode == enMode._AddNew)
            {
                ResetDefaultValues();
            }

            if (this._Mode == enMode._Update)
            {
                LoadData(_PersonID);
            }

            rbMale.Checked = true;
        }

        /// <summary>
        /// Saves current data by invoking Save.
        /// </summary>
        /// <remarks>Calls Save to persist changes.</remarks>
        /// <param name="sender">The control that raised the Click event.</param>
        /// <param name="e">Event arguments for the Click event.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Closes the containing form if one is found.
        /// </summary>
        /// <remarks>No action is taken if the control is not hosted in a Form.</remarks>
        /// <param name="sender">The control that raised the Click event.</param>
        /// <param name="e">Event data for the Click event.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }

        #endregion
    }
}
