using DVLD_DataAccess; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Business
{
    /// <summary>
    /// Represents and manages central Person identity records, personal demographics, contact details, and historical driving license associations.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Core identity domain object encapsulating demographic records, personal contact data, and license history.", Module = "People Management", Version = "1.0")]
    public class ClsPerson
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the person object instance.
        /// </summary>
        public enum enMode { _AddNew = 0, _Update = 1 };

        /// <summary>
        /// Gets or sets the current state mode of the object instance.
        /// </summary>
        public enMode Mode = enMode._AddNew;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the unique database identifier for the person.
        /// </summary>
        public int PersonID { set; get; }

        /// <summary>
        /// Gets or sets the unique national identification number.
        /// </summary>
        public string NationalNo { set; get; }

        /// <summary>
        /// Gets or sets the first name of the person.
        /// </summary>
        public string FirstName { set; get; }

        /// <summary>
        /// Gets or sets the optional second name of the person.
        /// </summary>
        public string SecondName { set; get; }

        /// <summary>
        /// Gets or sets the optional third name of the person.
        /// </summary>
        public string ThirdName { set; get; }

        /// <summary>
        /// Gets or sets the family/last name of the person.
        /// </summary>
        public string LastName { set; get; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { set; get; }

        /// <summary>
        /// Gets or sets the gender designation (0 for Male, 1 for Female).
        /// </summary>
        public short Gendor { set; get; }

        /// <summary>
        /// Gets or sets the residential address.
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// Gets or sets the primary contact phone number.
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// Gets or sets the associated foreign country identifier for nationality.
        /// </summary>
        public int NationalityCountryID { set; get; }

        /// <summary>
        /// Gets or sets the file path to the person's profile photograph.
        /// </summary>
        public string ImagePath { set; get; }

        /// <summary>
        /// Gets the full name composed of FirstName, optional SecondName and ThirdName, and LastName, with each part
        /// trimmed and separated by single spaces.
        /// </summary>
        /// <remarks>SecondName and ThirdName are included only if not null or whitespace. All parts are
        /// trimmed before concatenation. FirstName and LastName are assumed non-null; calling Trim on a null value will
        /// throw.</remarks>
        public string FullName
        {
            get
            {

                string secondNamePart = string.IsNullOrWhiteSpace(this.SecondName) ? "" : this.SecondName.Trim() + " ";


                string thirdNamePart = string.IsNullOrWhiteSpace(this.ThirdName) ? "" : this.ThirdName.Trim() + " ";


                return $"{this.FirstName.Trim()} {secondNamePart}{thirdNamePart}{this.LastName.Trim()}";
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of ClsPerson with default property values.
        /// </summary>
        /// <remarks>Sets PersonID to -1; sets NationalNo, FirstName, SecondName, ThirdName, LastName,
        /// Address, Phone, Email, and ImagePath to empty strings; sets DateOfBirth to DateTime.Now; sets Gendor and
        /// NationalityCountryID to -1; sets Mode to enMode._AddNew.</remarks>
        public ClsPerson()
        {
            this.PersonID = -1;

            this.NationalNo = "";

            this.FirstName = "";

            this.SecondName = "";

            this.ThirdName = "";

            this.LastName = "";

            this.DateOfBirth = DateTime.Now;

            this.Gendor = -1;

            this.Address = "";

            this.Phone = "";

            this.Email = "";

            this.NationalityCountryID = -1;

            this.ImagePath = "";

            Mode = enMode._AddNew;
        }

       /// <summary>
       /// Initializes a ClsPerson with the specified identifier, personal details, contact information, nationality,
       /// and image path, and sets Mode to enMode._Update.
       /// </summary>
       /// <param name="PersonID">Unique identifier for the person.</param>
       /// <param name="NationalNo">National identification number.</param>
       /// <param name="FirstName">First (given) name.</param>
       /// <param name="SecondName">Second (middle) name.</param>
       /// <param name="ThirdName">Third given name.</param>
       /// <param name="LastName">Family (last) name.</param>
       /// <param name="DateOfBirth">Person's date of birth.</param>
       /// <param name="Gendor">Gender code.</param>
       /// <param name="Address">Residential address.</param>
       /// <param name="Phone">Contact phone number.</param>
       /// <param name="Email">Contact email address.</param>
       /// <param name="NationalityCountryID">Identifier of the person's nationality country.</param>
       /// <param name="ImagePath">Path to the person's image file.</param>
        protected ClsPerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;

            this.NationalNo = NationalNo;

            this.FirstName = FirstName;

            this.SecondName = SecondName;

            this.ThirdName = ThirdName;

            this.LastName = LastName;

            this.DateOfBirth = DateOfBirth;

            this.Gendor = Gendor;

            this.Address = Address;

            this.Phone = Phone;

            this.Email = Email;

            this.NationalityCountryID = NationalityCountryID;

            this.ImagePath = ImagePath;

            Mode = enMode._Update;
        }
        #endregion

        #region Private CRUD Helpers

      /// <summary>
    /// Inserts a new person record using the instance's properties and assigns the generated PersonID.
    /// </summary>
    /// <remarks>Sets the instance's PersonID to the value returned by clsPersonData.InsertNewPerson. A
    /// PersonID of -1 indicates failure.</remarks>
    /// <returns>true if the insert succeeded and PersonID was assigned (PersonID != -1); otherwise, false.</returns>
        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.InsertNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
            return (this.PersonID != -1);
        }

       /// <summary>
        /// Updates the person record using the instance's properties by calling the data-layer update routine.
        /// </summary>
        /// <remarks>Delegates to clsPersonData.Updateperson.</remarks>
        /// <returns>True if the update succeeded; otherwise, false.</returns>
        private bool _UpdatPerson()
        {
            return clsPersonData.Updateperson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and returns a ClsPerson populated from the data source for the specified PersonID.
        /// </summary>
        /// <remarks>Queries the data layer and hydrates a new ClsPerson instance when a matching record
        /// is found.</remarks>
        /// <param name="PersonID">Primary key of the person to find.</param>
        /// <returns>A ClsPerson populated with the person's data if a matching record exists; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a person record by primary key PersonID.")]
        public static ClsPerson FindPersonByID(int PersonID)
        {
            int NationalityCountryID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalNo = "";
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;


            if (clsPersonData.GetPersonInfoByID(ref PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new ClsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and returns a populated ClsPerson for the specified national identification number.
        /// </summary>
        /// <remarks>The NationalNo value may be normalized or modified by the underlying data access
        /// call. Uses clsPersonData.GetPersonInfoByNationalNo to retrieve and populate person fields.</remarks>
        /// <param name="NationalNo">The national identification number of the person to locate.</param>
        /// <returns>A ClsPerson populated with the person's data if a matching record is found; otherwise null.</returns>

        [DocInfo("Finds and hydrates a person record by unique National ID.")]
        public static ClsPerson FindPersonByNationalNo(string NationalNo)
        {
            int NationalityCountryID = -1;
            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;


            if (clsPersonData.GetPersonInfoByNationalNo(ref PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new ClsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and returns the person that matches the specified nationality country identifier.
        /// </summary>
        /// <remarks>Calls clsPersonData.GetPersonInfoByNationality to retrieve the person record and
        /// returns the first match.</remarks>
        /// <param name="NationalityCountryID">Nationality country identifier to use when searching for a person.</param>
        /// <returns>A ClsPerson instance that matches the nationality country identifier, or null if no matching person is
        /// found.</returns>
        [DocInfo("Finds a person record by nationality country ID.")]
        public static ClsPerson FindPerosnByNationality(int NationalityCountryID)
        {
            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalNo = "";
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;

            if (clsPersonData.GetPersonInfoByNationality(ref PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
                ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new ClsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a person record by gender designation.
        /// </summary>
        /// <remarks>Delegates retrieval to clsPersonData.GetPersonInfoByGender and constructs a ClsPerson
        /// from the retrieved fields; returns null on failure or when no match exists.</remarks>
        /// <param name="Gendor">Gender designation used to locate the person record.</param>
        /// <returns>A ClsPerson representing the matched record, or null if no matching record is found.</returns>
        [DocInfo("Finds a person record by gender designation.")]
        public static ClsPerson FinPersonByGender(short Gendor)
        {
            int PersonID = -1;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalNo = "";
            string Address = "";
            string Phone = "";
            string Email = "";
            string ImagePath = "";
            int NationalityCountryID = -1;

            DateTime DateOfBirth = DateTime.Now;

            if (clsPersonData.GetPersonInfoByGender(ref PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName
                , ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new ClsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a DataTable of all people for administration data grids.
        /// </summary>
        /// <remarks>Delegates to clsPersonData.GetAllPeople.</remarks>
        /// <returns>A DataTable containing all person records; empty if no records exist.</returns>
        [DocInfo("Retrieves complete list of people for administration data grids.")]
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        /// <summary>
        /// Retrieves people records filtered by the specified column and value.
        /// </summary>
        /// <remarks>Delegates to the data layer; callers must ensure FilterColumn is valid and
        /// FilterValue is properly sanitized to avoid injection risks.</remarks>
        /// <param name="FilterColumn">Column name to filter by; must be a valid people column.</param>
        /// <param name="FilterValue">Value to match against the specified column.</param>
        /// <returns>A DataTable containing the matching people records; empty if no matches.</returns>
        [DocInfo("Retrieves dynamically filtered people records based on selected grid criteria.")]
        public static DataTable FilterPeop(string FilterColumn, string FilterValue)
        {


            return clsPersonData.GetFilteredPeople(FilterColumn, FilterValue);
        }

        /// <summary>
        /// Retrieves people records filtered by the selected grid criteria.
        /// </summary>
        /// <remarks>Delegates to clsPersonData.GetPeople.</remarks>
        /// <returns>A DataTable containing the people records that match the selected grid criteria.</returns>
        [DocInfo("Retrieves people records for display and filtering grids.")]
        public static DataTable FindPeople()
        {
            return clsPersonData.GetPeople();
        }
        #endregion

        #region Existence Checks & Deletion

        /// <summary>
        /// Deletes the person with the specified identifier.
        /// </summary>
        /// <remarks>Delegates to clsPersonData.DeletePerson.</remarks>
        /// <param name="PersonID">Unique identifier of the person to delete.</param>
        /// <returns>True if the person was deleted; otherwise, false.</returns>
        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        /// <summary>
        /// Determines whether a person with the specified identifier exists.
        /// </summary>
        /// <param name="PersonID">The identifier of the person to check.</param>
        /// <returns>true if a person with the specified identifier exists; otherwise, false.</returns>
        [DocInfo("Validates existence of a person by PersonID.")]
        public static bool DoesPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        /// <summary>
        /// Determines whether a person with the specified national identification number exists.
        /// </summary>
        /// <param name="NationalNo">National identification number to check.</param>
        /// <returns>True if a person with the specified national identification number exists; otherwise, false.</returns>
        [DocInfo("Validates existence of a person by NationalNo.")]
        public static bool DoesPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

        /// <summary>
        /// Determines whether the person with the specified identifier is registered as an active user.
        /// </summary>
        /// <param name="PersonID">The identifier of the person to check.</param>
        /// <returns>True if an active user account exists for the specified person; otherwise, false.</returns>
        [DocInfo("Checks if the person is registered as an active user account.")]
        public static bool DoesUserExist(int PersonID)
        {
            return clsPersonData.IsUserExist(PersonID);
        }
        #endregion

        #region Persistence & History Queries

       /// <summary>
       /// Persists the person domain entity to storage based on the current Mode state.
       /// </summary>
       /// <remarks>When Mode is enMode._AddNew, attempts to add a new person and sets Mode to
       /// enMode._Update on success. When Mode is enMode._Update, attempts to update the existing person. Returns false
       /// for unhandled Mode values or when the underlying add/update operation fails.</remarks>
       /// <returns>True if the entity was added or updated successfully; otherwise, false.</returns>

        [DocInfo("Persists person domain entity to storage based on current Mode state.")]
        public bool Save()
        {


            switch (Mode)
            {
                case enMode._AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode._Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode._Update:
                    return _UpdatPerson();
            }
            return false;
        }

        /// <summary>
        /// Retrieves the local license issuance history for the specified person.
        /// </summary>
        /// <param name="PersonID">Identifier of the person whose local license issuance history is retrieved.</param>
        /// <returns>A DataTable containing local license issuance records for the specified person.</returns>
        [DocInfo("Retrieves local license issuance history for the target person.")]
        public static DataTable GetPersonLocalLicensesHistory(int PersonID)
        {
            return clsPersonData.GetPersonLocalLicensesHistory(PersonID);
        }

        /// <summary>
        /// Retrieves international license issuance history for the specified person.
        /// </summary>
        /// <param name="PersonID">Identifier of the person whose international license issuance history is returned.</param>
        /// <returns>A DataTable containing the person's international license issuance history; may be empty if no records
        /// exist.</returns>

        [DocInfo("Retrieves international license issuance history for the target person.")]
        public static DataTable GetPersonInternationalLicensesHistory(int PersonID)
        {
            return clsPersonData.GetPersonInternationalLicensesHistory(PersonID);
        }

        #endregion
    }
}
