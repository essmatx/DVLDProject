using static DVLD_Shared.Attributes.clsDocAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
namespace DVLD_Business
{
    /// <summary>
    /// Represents and manages the base business logic, status lifecycles, and analytical metrics for all system applications.
    /// </summary>

    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages general application base business logic, status transitions, and analytical metrics.", Module = "Applications Management", Version = "1.0")]
    public class ClsApplicationBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the business object instance.
        /// </summary>
        public enum enMode { _AddNewApp = 0, _UpdateApp = 1 };

        /// <summary>
        /// Tracks current object mode to determine whether to insert or update during persistence.
        /// </summary>
        public enMode Mode { get; set; }

        /// <summary>
        /// Represents the execution and lifecycle status of an application.
        /// </summary>
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

        /// <summary>
        /// Gets or sets the current application status enum value.
        /// </summary>
        public enApplicationStatus AppStatus { get; set; }

        #endregion

        #region Composition Properties

        /// <summary>
        /// Gets or sets the associated applicant person object instance
        /// </summary>
        public ClsPerson PersonInof { get; set; }

        /// <summary>
        /// Gets or sets the application type domain model instance
        /// </summary>
        public ClsApplicationTypeBusiness applicationType { get; set; }

        /// <summary>
        /// Gets or sets the system user object instance who created the application.
        /// </summary>
        public ClsUserBuiness UserInfo { get; set; }

        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the unique identifier for the application
        /// </summary>

        public int ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the Person ID of the applican
        /// </summary>
        public int ApplicationPersonID { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the application was submitted
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the application type
        /// </summary>
        public int ApplicationTypeID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last status update
        /// </summary>
        public DateTime LastStatus { get; set; }

        /// <summary>
        /// Gets or sets the total paid application fees
        /// </summary>
        public decimal PaidFees { get; set; }

        /// <summary>
        /// Gets or sets the User ID of the creator.
        /// </summary>
        public int CreatedByUserID { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ClsApplicationBusiness"/> in _AddNewApp mode with default initial values.
        /// </summary>
        public ClsApplicationBusiness()
        {
            this.ApplicationID = -1;
            this.ApplicationPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.AppStatus = enApplicationStatus.New;
            this.LastStatus = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            this.Mode = enMode._AddNewApp;

        }

        /// <summary>
        /// Protected parameterized constructor used by derived application classes and factory methods to hydrate an existing application record (Update Mode).
        /// </summary>
        /// <param name="ApplicationID">The unique identifier of the existing application.</param>
        /// <param name="ApplicationPersonID">The Person ID of the applicant associated with this application.</param>
        /// <param name="ApplicationDate">The original date and time when the application was created.</param>
        /// <param name="ApplicationTypeID">The ID representing the type of application.</param>
        /// <param name="ApplicationStatus">The numeric status code (1 = New, 2 = Cancelled, 3 = Completed).</param>
        /// <param name="LastStatus">The date and time of the last status transition.</param>
        /// <param name="PaidFees">The total fees paid for this application.</param>
        /// <param name="CreatedByUserID">The ID of the user who originally created this application record.</param>
        protected ClsApplicationBusiness(int ApplicationID, int ApplicationPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, short ApplicationStatus, DateTime LastStatus, decimal PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicationPersonID = ApplicationPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.AppStatus = (enApplicationStatus)ApplicationStatus;
            this.LastStatus = LastStatus;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            this.PersonInof = ClsPerson.FindPersonByID(ApplicationPersonID);

            this.applicationType = ClsApplicationTypeBusiness.FindApplicationTypeByID(ApplicationTypeID);

            this.UserInfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.Mode = enMode._UpdateApp;
        }

        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion procedure and updates the internal ApplicationID state.
        /// </summary>
        /// <returns>True if insertion was successful; otherwise, false.</returns>
        private bool AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.InsertNewApplication(this.ApplicationPersonID, this.ApplicationDate, this.ApplicationTypeID, (short)this.AppStatus, this.LastStatus, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID > 0);
        }

        /// <summary>
        /// Calls DAL update procedure to persist modifications on an existing application record.
        /// </summary>
        /// <returns>True if update succeeded; otherwise, false.</returns>
        private bool UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicationPersonID, this.ApplicationDate, this.ApplicationTypeID, (short)this.AppStatus, this.LastStatus, this.PaidFees, this.CreatedByUserID);
        }

        #endregion

        #region Factory & Search Methods#region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates an application business instance by its unique ApplicationID.
        /// </summary>
        /// <param name="ApplicationID">The unique application identifier.</param>
        /// <returns>Populated <see cref="ClsApplicationBusiness"/> instance if found; otherwise null.</returns>
        [DocInfo("Finds and hydrates an application by Application ID.")]
        public static ClsApplicationBusiness FindApplicationByID(int ApplicationID)
        {
            int ApplicationPersonID = -1;
            DateTime ApplicationDate = DateTime.Now;
            int ApplicationTypeID = -1;
            short ApplicationStatus = -1;
            DateTime LastStatus = DateTime.Now;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;


            if (clsApplicationData.FindApplicationByID(ref ApplicationID, ref ApplicationPersonID, ref ApplicationDate, ref ApplicationTypeID, ref ApplicationStatus, ref LastStatus, ref PaidFees, ref CreatedByUserID))
            {
                ClsApplicationBusiness App = new ClsApplicationBusiness(ApplicationID, ApplicationPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatus, PaidFees, CreatedByUserID);

                return App;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all applications associated with a specific Person ID.
        /// </summary>
        /// <param name="ApplicationPersonID">The applicant's Person ID.</param>
        /// <returns>A <see cref="DataTable"/> containing application records for the specified person.</returns>
        [DocInfo("Retrieves applications for a specific applicant by Person ID.")]
        public static DataTable FindApplicationByPersonID(int ApplicationPersonID)
        {
            return clsApplicationData.FindApplicationByPersonID(ApplicationPersonID);
        }

        /// <summary>
        /// Checks whether a person currently has an active application of a specific type.
        /// </summary>
        /// <param name="ApplicationPersonID">The applicant Person ID.</param>
        /// <param name="ApplicationTypeID">Target Application Type ID.</param>
        /// <returns>True if an active application exists; otherwise, false.</returns>
        [DocInfo("Checks if an active application exists for a person and application type.")]
        public static bool DoesPersonHaveActiveApplication(int ApplicationPersonID, int ApplicationTypeID)
        {
            return clsApplicationData.FindApplicationByPersonIDAndApplicationTypeID(ApplicationPersonID, ApplicationTypeID);
        }


        /// <summary>
        /// Retrieves all application records across the system for presentation views.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing summary view of all applications.</returns>
        [DocInfo("Retrieves all application records.")]
        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }

        #endregion

        #region Business Rules & Lifecycle Workflows

        /// <summary>
        /// Persists the application object state into the database based on the current <see cref="Mode"/>.
        /// </summary>
        /// <returns>True if save or update succeeded; otherwise, false.</returns>
        [DocInfo("Saves new application or updates existing record according to current mode.")]
        public bool Save()
        {
            switch (Mode)
            {

                case enMode._AddNewApp:

                    // NOTE: Duplicate-application validation is intentionally NOT done here.
                    // Each specialized workflow (e.g. ClsLocalDrivingLicenseAppBusiness)
                    // performs its own finer-grained check before calling AppInfo.Save().
                    // A check here on (PersonID + ApplicationTypeID) is too broad —
                    // it would wrongly block a person from applying for a different license class
                    // because all local license apps share the same ApplicationTypeID.

                    if (AddNewApplication())
                    {

                        Mode = enMode._UpdateApp;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode._UpdateApp:
                    return UpdateApplication();
            }

            return false;
        }

        /// <summary>
        /// Cancels the current application and updates its status timestamp.
        /// </summary>
        /// <returns>True if cancellation status was successfully saved; otherwise, false.</returns>
        [DocInfo("Executes application cancellation workflow.")]
        public bool Cancel()
        {
            this.AppStatus = enApplicationStatus.Cancelled;
            this.LastStatus = DateTime.Now;
            return this.Save();

        }

        /// <summary>
        /// Marks the current application as completed and updates its status timestamp.
        /// </summary>
        /// <returns>True if completion status was successfully saved; otherwise, false.</returns>
        [DocInfo("Executes application completion workflow.")]
        public bool Completed()
        {
            this.AppStatus = enApplicationStatus.Completed;
            this.LastStatus = DateTime.Now;
            return this.Save();
        }

        /// <summary>
        /// Deletes an application record by its unique identifier.
        /// </summary>
        /// <param name="ApplicationID">The unique Application ID to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes an application record by ID.")]
        public static bool DeleteAppLication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }

        #endregion

        #region Metrics & Analytics


        /// <summary>
        /// Gets the total count of pending applications for a given application type.
        /// </summary>
        /// <param name="applicationTypeID">Target Application Type ID.</param>
        /// <returns>Count of pending applications.</returns>
        [DocInfo("Gets count of pending applications by application type.")]
        public static int GetPendingCountByApplicationType(int applicationTypeID)
        {
            return clsApplicationData.GetPendingCountByApplicationType(applicationTypeID);
        }

        /// <summary>
        /// Gets total applications count across the system.
        /// </summary>
        /// <returns>Total application count.</returns>
        [DocInfo("Retrieves total system applications count.")]
        public static int GetTotalApplicationsCount()
        {
            return clsApplicationData.GetTotalApplicationsCount();
        }

        /// <summary>
        /// Gets total completed applications count across the system.
        /// </summary>
        /// <returns>Total completed applications count.</returns>
        [DocInfo("Retrieves total completed applications count.")]
        public static int GetCompletedApplicationsCount()
        {
            return clsApplicationData.GetCompletedApplicationsCount();
        }

        /// <summary>
        /// Calculates system efficiency rate as a percentage of completed applications over total applications.
        /// </summary>
        /// <returns>System efficiency rate percentage (0.0 to 100.0).</returns>
        [DocInfo("Calculates completion efficiency rate percentage across system applications.")]
        public static double CalculateSystemEfficiencyRate()
        {
            int totalApps = GetTotalApplicationsCount();
            int completedApps = GetCompletedApplicationsCount();

            if (totalApps == 0)
                return 0.0;

            return ((double)completedApps / totalApps) * 100.0;
        }

        /// <summary>
        /// Retrieves daily application stats for processed and total applications.
        /// </summary>
        /// <param name="processedCount">Output parameter for daily processed applications count.</param>
        /// <param name="totalCount">Output parameter for daily total applications count.</param>
        /// <returns>True if stats were successfully retrieved; otherwise, false.</returns>
        [DocInfo("Retrieves daily application processing statistics.")]
        public static bool GetDailyStats(ref int processedCount, ref int totalCount)
        {
            return clsApplicationData.GetDailyApplicationShortStats(ref processedCount, ref totalCount);
        }

        #endregion
    }
}