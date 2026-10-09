using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace DVLD_Business
{
    /// <summary>
    /// Represents and manages driver profiles, inheriting core personal demographics from <see cref="ClsPerson"/> while handling driver lifecycle workflows and persistence.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages driver profiles, personal identity inheritance, driver search functions, and persistence.", Module = "Drivers Management", Version = "1.0")]
    public class ClsDriverBusiness : ClsPerson
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the driver business object instance.
        /// </summary>

        private enum enMode { _AddNew = 0 , _Update = 1};

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        enMode Mode = enMode._AddNew;
        #endregion

        #region Properties & Composition

        /// <summary>
        /// Gets the unique identifier for the driver profile.
        /// </summary>
        public int DriverID { get; private set; }

        /// <summary>
        /// Gets or sets the User ID of the system user who registered this driver.
        /// </summary>
        public int CreatedUserID { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the driver profile was created.
        /// </summary>

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the system user domain object instance who registered this driver.
        /// </summary>
        public ClsUserBuiness UserInfo { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ClsDriverBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsDriverBusiness()
        {
            this.DriverID = -1;
            this.CreatedUserID = -1;
            this.CreatedDate = DateTime.Now;

            this.Mode = enMode._AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing driver domain object instance along with its inherited personal demographics (Update Mode).
        /// </summary>
        /// <param name="DriverID">The unique identifier of the driver profile.</param>
        /// <param name="PersonID">The unique identifier of the associated person entity.</param>
        /// <param name="NationalNo">The national identity number of the person.</param>
        /// <param name="FirstName">The first name of the person.</param>
        /// <param name="SecondName">The second/middle name of the person.</param>
        /// <param name="ThirdName">The third name of the person.</param>
        /// <param name="LastName">The last/family name of the person.</param>
        /// <param name="DateOfBirth">The date of birth of the person.</param>
        /// <param name="Gendor">The gender indicator (0 for Male, 1 for Female).</param>
        /// <param name="Address">The residential address of the person.</param>
        /// <param name="Phone">The primary contact phone number.</param>
        /// <param name="Email">The contact email address.</param>
        /// <param name="NationalityCountryID">The unique country identifier for nationality.</param>
        /// <param name="ImagePath">The file system path or URI to the person's profile image.</param>
        /// <param name="CreatedUserID">The User ID of the system user who registered this driver.</param>
        /// <param name="CreatedDate">The date and time when the driver record was created.</param>
        private ClsDriverBusiness(int DriverID, int PersonID, string NationalNo, string FirstName,
                        string SecondName, string ThirdName, string LastName,
                        DateTime DateOfBirth, short Gendor, string Address,
                        string Phone, string Email, int NationalityCountryID,
                        string ImagePath, int CreatedUserID, DateTime CreatedDate)
            : base(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
           DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
        {
            this.DriverID = DriverID;
            this.CreatedUserID = CreatedUserID;
            this.CreatedDate = CreatedDate;

            this.Mode = enMode._Update;

            this.UserInfo = ClsUserBuiness.FindUserByID(CreatedUserID);
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion logic to add a new driver record and updates the internal DriverID state.
        /// </summary>
        /// <returns>True if insertion succeeded; otherwise, false.</returns>
        private bool AddNewDriver()
        {
            this.DriverID = clsDriverData.InsertNewDriver(this.PersonID, this.CreatedUserID, this.CreatedDate);

            return (this.DriverID > 0);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing driver record.
        /// </summary>
        /// <returns>True if update succeeded; otherwise, false.</returns>
        private bool UpdateDriver()
        {
            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedUserID, this.CreatedDate);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a driver business instance by unique Driver ID.
        /// </summary>
        /// <param name="DriverID">The unique driver identifier.</param>
        /// <returns>Populated <see cref="ClsDriverBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a driver profile by DriverID.")]
        public static ClsDriverBusiness FindDriverByID(int DriverID)
        {
            int PersonID = -1;
            int CreatedUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.FindDriverByID(ref DriverID, ref PersonID, ref CreatedUserID, ref CreatedDate))
            {
                ClsPerson Peroninfo = ClsPerson.FindPersonByID(PersonID);
                if (Peroninfo != null)
                {
                    ClsDriverBusiness Driver = new ClsDriverBusiness(DriverID, Peroninfo.PersonID, Peroninfo.NationalNo,
                        Peroninfo.FirstName, Peroninfo.SecondName, Peroninfo.ThirdName, Peroninfo.LastName,
                        Peroninfo.DateOfBirth, Peroninfo.Gendor, Peroninfo.Address, Peroninfo.Phone, Peroninfo.Email, Peroninfo.NationalityCountryID, Peroninfo.ImagePath, CreatedUserID, CreatedDate);

                    return Driver;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates a driver business instance associated with a specific Person ID.
        /// </summary>
        /// <param name="PersonID">The unique person identifier.</param>
        /// <returns>Populated <see cref="ClsDriverBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a driver profile by associated PersonID.")]
        public static ClsDriverBusiness FindDriverByPerson(int PersonID)
        {
            int DriverID = -1;
            int CreatedUserID = -1;
            DateTime CreatedDate = DateTime.Now;



            if (clsDriverData.FindDriverByPersonID(ref DriverID, ref PersonID, ref CreatedUserID, ref CreatedDate))
            {
                ClsPerson Peroninfo = ClsPerson.FindPersonByID(PersonID);
                if (Peroninfo != null)
                {
                    ClsDriverBusiness Driver = new ClsDriverBusiness(DriverID, Peroninfo.PersonID, Peroninfo.NationalNo,
                        Peroninfo.FirstName, Peroninfo.SecondName, Peroninfo.ThirdName, Peroninfo.LastName,
                        Peroninfo.DateOfBirth, Peroninfo.Gendor, Peroninfo.Address, Peroninfo.Phone, Peroninfo.Email, Peroninfo.NationalityCountryID, Peroninfo.ImagePath, CreatedUserID, CreatedDate);

                    Driver.UserInfo = ClsUserBuiness.FindUserByID(CreatedUserID);

                    return Driver;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all registered driver records from the database for grid displays.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all driver profiles.</returns>
        [DocInfo("Retrieves all registered driver records for administration grid display.")]
        public static DataTable GetAllDriveres()
        {
            return clsDriverData.GetAllDrivers();
        }

        /// <summary>
        /// Deletes a driver record by its unique Driver ID.
        /// </summary>
        /// <param name="DriverID">The unique driver identifier to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a driver record by DriverID.")]
        public static bool DeleteDiver(int DriverID)
        {
            return clsDriverData.DeleteDriver(DriverID);
        }
        #endregion

        #region Business Rules & Workflows
        /// <summary>
        /// Validates business rules and saves new or updated driver records based on the object's current state.
        /// Enforces the rule: One driver record per person.
        /// </summary>
        /// <returns>True if saving or updating succeeded; otherwise, false.</returns>
        [DocInfo("Saves new driver profile or updates existing record after enforcing unique person driver constraint.")]
        public bool Save()
        {
            // Basic validation
            if (this.PersonID <= 0)
                return false;

            // PATH A: Adding a brand new driver
            if (this.DriverID == -1 || this.DriverID == 0)
            {
                // Enforce the rule: One driver record per person
                if (FindDriverByPerson(this.PersonID) != null)
                    return false;

                this.Mode = enMode._AddNew; 
                return AddNewDriver();
            }
            // PATH B: Updating an existing driver
            else
            {
               this.Mode = enMode._Update;

                return UpdateDriver(); 
            }

        }

        #endregion

    }
}
