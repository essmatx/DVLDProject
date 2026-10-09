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
    /// Manages driving test appointments, enforces prerequisite rules and active-appointment checks, handles locking
    /// and mode-based create/update operations, and exposes persistence and reporting helpers.
    /// </summary>
    /// <remarks>Prerequisite logic: test type N requires passing test type N−1 (vision test type 1 has no
    /// prerequisite). Save validates required IDs, prevents duplicate active appointments, enforces prerequisites, and
    /// either inserts or updates an appointment depending on Mode (AddNew or Update). The class delegates persistence
    /// to clsTestAppointmentData and provides lookup, locking, trial-counting, and daily-reporting helpers. Related IDs
    /// link to user, application, and test-type information.</remarks>
    [DocInfo("Core domain object managing driving test appointments, prerequisite rules, active status checks, and scheduling metrics.", Module = "Test Management", Version = "1.0")]
    public class ClsTestAppointmentBusiness
    {
        #region Enums & Modes
        /// <summary>
        /// Indicates whether an operation adds a new item or updates an existing one.
        /// </summary>
        /// <remarks>Use for save/persist operations to distinguish insert (_AddNew = 0) from update
        /// (_Update = 1).</remarks>
        public enum enMode { _AddNew = 0, _Update = 1 };

        /// <summary>
        /// Gets or sets the current operation mode.
        /// </summary>
        public enMode Mode { get; set; }
        #endregion

        #region Scalar & Navigation Properties

        /// <summary>
        /// Gets or sets the unique identifier for the test appointment.
        /// </summary>
        /// <remarks>Represents the primary key for the TestAppointment entity and typically maps to the
        /// database identity column.</remarks>
        public int TestAppointmentID { get; set; }

        /// <summary>
       /// Gets or sets the identifier of the test type.
       /// </summary>
        public int TestTypeID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the local driving license application.
        /// </summary>
        /// <remarks>Represents the primary key assigned by the local system and used to correlate
        /// application records.</remarks>
        public int LocalDrivingLicenseApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the appointment.
        /// </summary>
        /// <remarks>Use DateTimeKind to indicate whether the value is UTC or local; prefer UTC for
        /// storage and comparison.</remarks>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Gets or sets the total amount of fees that have been paid.
        /// </summary>
        /// <remarks>Expressed in the application's currency and stored as a decimal for monetary
        /// precision; value is expected to be non-negative.</remarks>
        public decimal PaidFees { get; set; }

        /// <summary>
        /// Identifier of the user who created the entity.
        /// </summary>
        /// <remarks>Typically a foreign key to the Users table; set at creation and not modified
        /// afterward.</remarks>
        public int CreatedByUserID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the object is locked.
        /// </summary>
        /// <remarks>When true, modifying operations should be prevented until the property is
        /// cleared.</remarks>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the ClsUserBuiness instance that contains the user's business information.
        /// </summary>
        /// <remarks>May be null if no user information is available.</remarks>
        public ClsUserBuiness Userinfo { get; set; }

        /// <summary>
        /// Gets or sets the business information for the test type.
        /// </summary>
        /// <remarks>May be null if not initialized.</remarks>
        public ClsTestTypeBusiness TesTypeinfo { get; set; }

        /// <summary>
        /// Gets or sets the local driving license application business data.
        /// </summary>
        /// <remarks>May be null if the local driving license application has not been
        /// initialized.</remarks>
        public ClsLocalDrivingLicenseAppBusiness LocalLicensinfo { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClsTestAppointmentBusiness class with default property values.
        /// </summary>
        /// <remarks>Default values: TestAppointmentID = -1; TestTypeID = -1;
        /// LocalDrivingLicenseApplicationID = -1; AppointmentDate = DateTime.Now; PaidFees = -1; CreatedByUserID = -1;
        /// IsLocked = false; Userinfo = new ClsUserBuiness(); LocalLicensinfo = new
        /// ClsLocalDrivingLicenseAppBusiness(); Mode = enMode._AddNew.</remarks>
        public ClsTestAppointmentBusiness()
        {
            this.TestAppointmentID = -1;

            this.TestTypeID = -1;

            this.LocalDrivingLicenseApplicationID = -1;

            this.AppointmentDate = DateTime.Now;

            this.PaidFees = -1;

            this.CreatedByUserID = -1;

            this.IsLocked = false;

            this.Userinfo = new ClsUserBuiness();

            this.LocalLicensinfo = new ClsLocalDrivingLicenseAppBusiness();

            this.Mode = enMode._AddNew;
        }

        /// <summary>
        /// Initializes a ClsTestAppointmentBusiness with specified identifiers, appointment details, and related entity
        /// data.
        /// </summary>
        /// <remarks>Sets Mode to enMode._Update and populates Userinfo, LocalLicensinfo, and TesTypeinfo
        /// by loading the corresponding entities.</remarks>
        /// <param name="TestAppointmentID">Identifier of the test appointment.</param>
        /// <param name="TestTypeID">Identifier of the test type.</param>
        /// <param name="LocalDrivingLicenseApplicationID">Identifier of the local driving license application.</param>
        /// <param name="AppointmentDate">Date and time of the appointment.</param>
        /// <param name="PaidFees">Amount of fees paid for the appointment.</param>
        /// <param name="CreatedByUserID">Identifier of the user who created the appointment.</param>
        /// <param name="IsLocked">Whether the appointment is locked.</param>
        private ClsTestAppointmentBusiness(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {
            this.TestAppointmentID = TestAppointmentID;

            this.TestTypeID = TestTypeID;

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            this.AppointmentDate = AppointmentDate;

            this.PaidFees = PaidFees;

            this.CreatedByUserID = CreatedByUserID;

            this.IsLocked = IsLocked;

            this.Mode = enMode._Update;

            this.Userinfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.LocalLicensinfo = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);

            this.TesTypeinfo = ClsTestTypeBusiness.FindTestTypeByID((ClsTestTypeBusiness.enTestType)TestTypeID);
        }
        #endregion

        #region Private CRUD Helpers
        /// <summary>
        /// Inserts a new test appointment into the data store and assigns the resulting identifier to
        /// TestAppointmentID.
        /// </summary>
        /// <remarks>Calls clsTestAppointmentData.InserNewTestAppointment with TestTypeID,
        /// LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, and IsLocked to create the
        /// record.</remarks>
        /// <returns>True if the inserted TestAppointmentID is greater than zero; otherwise, false.</returns>
        private bool _AddNewTestAppoint()
        {
            this.TestAppointmentID = clsTestAppointmentData.InserNewTestAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);

            return (this.TestAppointmentID > 0);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Gets and hydrates the test appointment identified by TestAppointmentID.
        /// </summary>
        /// <remarks>Loads data using clsTestAppointmentData.FindTestAppointmentByID and constructs a
        /// ClsTestAppointmentBusiness instance.</remarks>
        /// <param name="TestAppointmentID">The primary key identifier of the test appointment to retrieve.</param>
        /// <returns>A ClsTestAppointmentBusiness populated with the appointment data, or null if no matching record is found.</returns>

        [DocInfo("Finds and hydrates a test appointment record by primary key TestAppointmentID.")]
        public static ClsTestAppointmentBusiness FindTestAppointByID(int TestAppointmentID)
        {

            int TestTypeID = -1;

            int LocalDrivingLicenseApplicationID = -1;

            DateTime AppointmentDate = DateTime.Now;

            decimal PaidFees = -1;

            int CreatedByUserID = -1;

            bool IsLocked = false;

            if (clsTestAppointmentData.FindTestAppointmentByID(ref TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked))
            {
                ClsTestAppointmentBusiness TestInfo = new ClsTestAppointmentBusiness(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);


                return TestInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves test appointments for the specified test type and local driving license application.
        /// </summary>
        /// <remarks>Delegates to clsTestAppointmentData.GetTestAppointmentsByLocalAppAndTestTyp; may
        /// return an empty DataTable if no records match.</remarks>
        /// <param name="TestTypeID">The identifier of the test type used to filter appointments.</param>
        /// <param name="LocalDrivingLicenseApplicationID">The identifier of the local driving license application used to filter appointments.</param>
        /// <returns>A DataTable containing the matching test appointment records.</returns>
        [DocInfo("Retrieves appointments filtered by TestType ID and Local Driving License Application ID.")]
        public static DataTable FindTestAppointByTesTypeID(int TestTypeID, int LocalDrivingLicenseApplicationID)
        {

            return clsTestAppointmentData.GetTestAppointmentsByLocalAppAndTestTyp(TestTypeID, LocalDrivingLicenseApplicationID);
        }

        /// <summary>
        /// Retrieves all test appointment records used to populate administration UI grids.
        /// </summary>
        /// <remarks>Obtains data from the data access layer; the DataTable schema is defined by the
        /// underlying data source and intended for UI consumption.</remarks>
        /// <returns>A DataTable containing the complete set of test appointment records.</returns>

        [DocInfo("Retrieves complete list of test appointments for administration UI grids.")]
        public static DataTable GetAllTestAppoint()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        /// <summary>
        /// Retrieves historical appointments for a specified local driving license application filtered by test type.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">Identifier of the local driving license application.</param>
        /// <param name="TestTypeID">Identifier of the test type used to filter appointments.</param>
        /// <returns>A DataTable containing appointment records for the specified application and test type.</returns>

        [DocInfo("Retrieves historical appointment list for a given application per test type.")]
        public static DataTable GetApplicationAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            return clsTestAppointmentData.GetApplicationAppointmentsPerTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        #endregion

        #region Business Rules & Validation

        /// <summary>
        /// Determines whether the specified application has an unlocked pending appointment for the given test type.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">Local driving license application identifier.</param>
        /// <param name="TestTypeID">Test type identifier to check.</param>
        /// <returns>True if an unlocked pending appointment exists for the application and test type; otherwise, false.</returns>
        [DocInfo("Validates whether an application has an unlocked pending appointment for a specific test type.")]
        public static bool IsAnyActiveAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.IsAnyActiveAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        /// <summary>
        /// Determines whether a specific test type has already been passed for a local driving license application.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">Identifier of the local driving license application.</param>
        /// <param name="TestTypeID">Identifier of the test type to check.</param>
        /// <returns>True if the specified test type has been passed for the application; otherwise, false.</returns>
        [DocInfo("Validates whether a specific test type has already been passed for an application.")]
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.DoesPassTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        /// <summary>
        /// Indicates whether the prerequisite test for the specified test type has not been passed for the given local
        /// driving license application.
        /// </summary>
        /// <remarks>Vision (1) has no prerequisite. Written (2) requires Vision (1) to be passed.
        /// Practical (3) requires Written (2) to be passed.</remarks>
        /// <param name="LocalDrivingLicenseApplicationID">Local driving license application identifier.</param>
        /// <param name="TestTypeID">Test type identifier (1 = Vision, 2 = Written, 3 = Practical).</param>
        /// <returns>True if a prerequisite test exists and has not been passed for the application; otherwise false.</returns>
        [DocInfo("Evaluates workflow sequential dependencies before permitting appointment scheduling.")]
        public static bool IsPrerequisiteMissing(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            // Vision test (1) has no prerequisite
            // Written test (2) requires Vision (1) to be passed
            // Practical test (3) requires Written (2) to be passed
            int prerequisiteTestTypeID = TestTypeID - 1;

            if (prerequisiteTestTypeID <= 0)
                return false; // Vision test has no prerequisite

            if (!DoesPassTestType(LocalDrivingLicenseApplicationID, prerequisiteTestTypeID))
            {
                return true; // Prerequisite IS missing
            }

            return false; // Prerequisite is satisfied
        }

        /// <summary>
        /// Retrieves the total number of trial attempts for the specified local driving license application and test
        /// type.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">Local driving license application identifier.</param>
        /// <param name="TestTypeID">Test type identifier.</param>
        /// <returns>Total number of trial attempts for the specified application and test type.</returns>
        [DocInfo("Retrieves trial attempt count for a specific application and test type.")]
        public static int TotalTrialsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.TotalTrialsPerTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        /// <summary>
        /// Gets the number of appointments scheduled for today for the specified test type.
        /// </summary>
        /// <remarks>Counts only appointments scheduled for the current date.</remarks>
        /// <param name="testTypeID">The identifier of the test type for which to count today's scheduled appointments.</param>
        /// <returns>The number of appointments scheduled for today for the specified test type.</returns>

        [DocInfo("Retrieves daily scheduled appointment count per test type for dashboard metrics.")]
        public static int GetTodayAppointmentsCountByTestType(int testTypeID)
        {
            return clsTestAppointmentData.GetTodayAppointmentsCountByTestType(testTypeID);
        }

        /// <summary>
        /// Gets the total number of scheduled appointments for the current calendar date.
        /// </summary>
        /// <returns>The total number of scheduled appointments for the current calendar date.</returns>

        [DocInfo("Retrieves total aggregate scheduled appointments for the current calendar date.")]
        public static int GetTotalTodayAppointmentsCount()
        {
            return clsTestAppointmentData.GetTotalTodayAppointmentsCount();
        }
        #endregion

        #region Persistence & Scheduling Operations

        /// <summary>
        /// Deletes the test appointment with the specified TestAppointmentID.
        /// </summary>
        /// <remarks>Delegates to clsTestAppointmentData.DeleteTestAppointment in the data-access
        /// layer.</remarks>
        /// <param name="TestAppointmentID">Primary key of the test appointment to delete.</param>
        /// <returns>True if the record was deleted; otherwise, false.</returns>
        [DocInfo("Deletes a test appointment record by primary key TestAppointmentID.")]
        public static bool DeleteTestAppoint(int TestAppointmentID)
        {
            return clsTestAppointmentData.DeleteTestAppointment(TestAppointmentID);
        }

        /// <summary>
        /// Sets a lock flag on a completed appointment to prevent further modifications.
        /// </summary>
        /// <remarks>Only applies to completed appointments. Operation delegates to the data
        /// layer.</remarks>
        /// <param name="TestAppointmentID">Identifier of the appointment to lock.</param>
        /// <returns>true if the appointment was successfully locked; otherwise, false.</returns>
        [DocInfo("Sets lock flag on completed appointment to freeze modifications.")]
        public static bool LockAppointment(int TestAppointmentID)
        {
            return clsTestAppointmentData.LockAppointment(TestAppointmentID);
        }

        /// <summary>
        /// Updates the scheduled date for an existing active test appointment.
        /// </summary>
        /// <remarks>Only active appointments are eligible for date modification.</remarks>
        /// <param name="TestAppointmentID">Identifier of the test appointment to update.</param>
        /// <param name="NewAppointmentDate">New scheduled date and time for the appointment.</param>
        /// <returns>True if the appointment was updated successfully; otherwise, false.</returns>

        [DocInfo("Updates scheduled date for an existing active test appointment.")]
        public static bool ModifyAppointmentDate(int TestAppointmentID, DateTime NewAppointmentDate)
        {
            return clsTestAppointmentData.UpdateTestAppointment(TestAppointmentID, NewAppointmentDate);

        }

        /// <summary>
        /// Validates business rules—active status, prerequisites, and locking—and persists a test appointment entity.
        /// </summary>
        /// <remarks>Validates LocalDrivingLicenseApplicationID and TestTypeID, ensures no existing active
        /// appointment for the same application and test type, enforces prerequisite test completion, and prevents
        /// updates on locked appointments. In AddNew mode, creates a new appointment and sets Mode to Update on
        /// success. In Update mode, updates the appointment date if not locked.</remarks>
        /// <returns>true if the appointment was created or updated successfully; otherwise, false.</returns>
        [DocInfo("Validates business rules (active status, prerequisites, locking) and persists test appointment entity.")]
        public bool Save()
        {
            if (this.LocalDrivingLicenseApplicationID == -1 || this.TestTypeID == -1)
            {
                Console.WriteLine("Validation Error: Invalid Application or Test Type ID.");
                return false;
            }

            if (IsAnyActiveAppointment(this.LocalDrivingLicenseApplicationID, this.TestTypeID))
            {
                Console.WriteLine("Validation Error: Person already has an active appointment!");
                return false;
            }



            if (IsPrerequisiteMissing(this.LocalDrivingLicenseApplicationID, this.TestTypeID))
            {
                string prevTest = (this.TestTypeID == 2) ? "Vision" : "Written";
                Console.WriteLine($"Validation Error: Cannot schedule this test. The {prevTest} test must be passed first!");
                return false;
            }


            switch (Mode)
            {
                case enMode._AddNew:
                    if (_AddNewTestAppoint())
                    {

                        Mode = enMode._Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode._Update:

                    if (this.IsLocked)
                    {
                        Console.WriteLine("Validation Error: Cannot update a locked appointment.");
                        return false;
                    }

                    // if (ClsTestAppointmentBusiness.LockAppointment(this.TestAppointmentID))
                    // {
                    //     Console.WriteLine("Validation Error: Cannot update a locked appointment.");
                    //     return false;
                    // }
                    return ModifyAppointmentDate(this.TestAppointmentID, this.AppointmentDate);
            }
            return false;
        }
        #endregion

    }
}
