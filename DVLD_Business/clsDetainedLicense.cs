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
    /// Represents and manages business rules, detention lifecycles, fine calculations, and release workflows for driver licenses.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages license detention records, fines, release application workflows, and detention metrics.", Module = "Licenses Management", Version = "1.0")]
    public class ClsDetainedLicenseBusiness
    {
        #region Enums & Modes
        /// <summary>
        /// Defines the state mode of the detained license business object instance.
        /// </summary>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        public enMode Mode { get; set; }
        #endregion

        #region Composition Properties

        /// <summary>
        /// Gets or sets the release application domain object instance associated with this detention.
        /// </summary>
        public ClsApplicationBusiness Appinfo { get; set; }

        /// <summary>
        /// Gets or sets the user domain object instance who created the detention record.
        /// </summary>
        public ClsUserBuiness Userinfo { get; set; }

        /// <summary>
        /// /// <summary>Gets or sets the license domain object instance associated with this detention.
        /// </summary>
        public ClsLicenseBusiness Licensinfo { get; set; }
        #endregion

        #region Scalar Properties
        /// <summary>
        /// Gets or sets the unique identifier for the detention record.
        /// </summary>
        public int DetainID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the detained license.
        /// </summary>
        public int LicenseID { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the license was detained.
        /// </summary>
        public DateTime DetainDate { get; set; }

        /// <summary>
        /// Gets or sets the fine fee amount imposed for the detention.
        /// </summary>
        public decimal FineFees { get; set; }

        /// <summary>
        /// Gets or sets the User ID of the system user who created the detention.
        /// </summary>
        public int CreatedByUserID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the license has been released from detention.
        /// </summary>
        public bool IsReleased { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the license was released, or null if still detained.
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the User ID of the system user who authorized the release, or null if still detained.
        /// </summary>
        public int? ReleasedByUserID { get; set; }

        /// <summary>
        /// Gets or sets the Release Application ID associated with the release workflow, or null if still detained.
        /// </summary>
        public int? ReleaseApplicationID { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="ClsDetainedLicenseBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsDetainedLicenseBusiness()
        {
            this.DetainID = -1;

            this.LicenseID = -1;

            this.DetainDate = DateTime.Now;

            this.FineFees = -1;

            this.CreatedByUserID = -1;

            this.IsReleased = false;

            this.ReleaseDate = null;

            this.ReleasedByUserID = null;

            this.ReleaseApplicationID = null;

            this.Mode = enMode.AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing detained license domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="DetainID">The unique identifier of the detention record.</param>
        /// <param name="LicenseID">The unique identifier of the detained license.</param>
        /// <param name="DetainDate">The date and time when the license was detained.</param>
        /// <param name="FineFees">The fine fee amount imposed.</param>
        /// <param name="CreatedByUserID">The ID of the user who created the detention.</param>
        /// <param name="IsReleased">Indicates whether the license is released.</param>
        /// <param name="ReleaseDate">The date and time of release.</param>
        /// <param name="ReleasedByUserID">The ID of the user who authorized the release.</param>
        /// <param name="ReleaseApplicationID">The Application ID associated with the release.</param>
        private ClsDetainedLicenseBusiness(int DetainID, int LicenseID, DateTime DetainDate,
           decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;

            this.LicenseID = LicenseID;

            this.DetainDate = DetainDate;

            this.FineFees = FineFees;

            this.CreatedByUserID = CreatedByUserID;

            this.IsReleased = IsReleased;

            this.ReleaseDate = ReleaseDate;

            this.ReleaseApplicationID = ReleaseApplicationID;

            this.ReleasedByUserID = ReleasedByUserID;

            this.Userinfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.Licensinfo = ClsLicenseBusiness.FindLicenseByID(LicenseID);

            if (this.IsReleased)
            {
                this.Appinfo = ClsApplicationBusiness.FindApplicationByID(ReleaseApplicationID);
            }
            else
            {
                this.Appinfo = null;
            }

            this.Mode = enMode.Update;
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion logic to add a new license detention record and updates the internal DetainID state.
        /// </summary>
        /// <returns>True if insertion succeeded; otherwise, false.</returns>
        private bool AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.InsertDetainedLicense(this.LicenseID, this.DetainDate,
           this.FineFees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);

            return (DetainID > 0);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing detained license record.
        /// </summary>
        /// <returns>True if update succeeded; otherwise, false.</returns>
        private bool UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate,
           this.FineFees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a detained license business instance by its unique Detain ID.
        /// </summary>
        /// <param name="DetainID">The unique detention identifier.</param>
        /// <returns>Populated <see cref="ClsDetainedLicenseBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a detained license record by DetainID.")]
        public static ClsDetainedLicenseBusiness FindDetainedLicenseByDetainID(int DetainID)
        {
            int LicenseID = -1;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = -1;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.FindDetainedLicenseByDetainID(ref DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                ClsDetainedLicenseBusiness Detaininfo = new ClsDetainedLicenseBusiness(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
                return Detaininfo;
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// Finds and hydrates the active detained license business instance for a given License ID.
        /// </summary>
        /// <param name="LicenseID">The unique license identifier.</param>
        /// <returns>Populated <see cref="DataTable"/> instance if an active detention exists; otherwise, null.</returns>
        [DocInfo("Finds active detained license details for a specific LicenseID.")]
        public static DataTable FindDetainedLicenseByLicenseID(int LicenseID)
        {
            return clsDetainedLicenseData.GetDetainedLicensesByLicenseID(LicenseID);
        }

        /// <summary>
        /// Finds and hydrates the active detained license business instance for a given License ID.
        /// </summary>
        /// <param name="LicenseID">The unique license identifier.</param>
        /// <returns>Populated <see cref="ClsDetainedLicenseBusiness"/> instance if an active detention exists; otherwise, null.</returns>
        [DocInfo("Finds active detained license details for a specific LicenseID.")]
        public static ClsDetainedLicenseBusiness FindDetainedLicenseByLicenseiD(int DetainID)
        {
            int LicenseID = -1;
            DateTime DetainDate = DateTime.Now;
            decimal FineFees = -1;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.Now;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.FindDetainedLicenseByLicenseID(ref DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                ClsDetainedLicenseBusiness Detaininfo = new ClsDetainedLicenseBusiness(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
                return Detaininfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves detention history logs for a specific License ID.
        /// </summary>
        /// <param name="LicenseID">The target license ID.</param>
        /// <returns>A <see cref="DataTable"/> containing all detention records for the specified license.</returns>
        [DocInfo("Retrieves detention history table for a specific LicenseID.")]
        public static int GetDetainedLicenseByLicenseID(int LicenseID)
        {
            return clsDetainedLicenseData.GetActiveDetainedLicenseByLicenseID(LicenseID);
        }

        /// <summary>
        /// Retrieves all detained license records across the system for administration grid display.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all detained license records.</returns>
        [DocInfo("Retrieves all detained licenses records for display grid.")]
        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        /// <summary>
        /// Deletes a detained license record by its unique identifier.
        /// </summary>
        /// <param name="DetainID">The unique Detain ID to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a detained license record by ID.")]
        public static bool DeleteDetainedLicense(int DetainID)
        {
            return clsDetainedLicenseData.DeleteDetainedLicense(DetainID);
        }
        #endregion

        #region Business Rules & Workflows
        /// <summary>
        /// Executes the release workflow for this detained license instance.
        /// </summary>
        /// <param name="ReleasedByUserID">User ID authorizing the release.</param>
        /// <param name="ReleaseApplicationID">Release Application ID associated with payment/processing.</param>
        /// <returns>True if the release state was successfully updated; otherwise, false.</returns>
        [DocInfo("Executes release workflow for a detained license record.")]
        public bool ReleaseDetain(int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.IsReleased = true;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;

            return clsDetainedLicenseData.ReleaseDetain(this.DetainID, this.ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        }

        /// <summary>
        /// Persists changes to the detained license record according to the object's current state (<see cref="Mode"/>).
        /// </summary>
        /// <returns>True if save or update succeeded; otherwise, false.</returns>
        [DocInfo("Saves new license detention or updates existing record based on current mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return this.UpdateDetainedLicense();
            }

            return false;
        }
        #endregion

        #region Metrics & Analytics

        /// <summary>
        /// Checks whether a specific license is currently in an active (unreleased) detained status.
        /// </summary>
        /// <param name="LicenseID">The unique identifier of the license to check.</param>
        /// <returns>True if the license is currently detained and unreleased; otherwise, false.</returns>
        [DocInfo("Checks whether a license is currently in active detention status.")]
        public static bool IsLicenseDetaind(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetaind(LicenseID);
        }

        /// <summary>
        /// Retrieves the total count of currently active (unreleased) detained licenses across the system.
        /// </summary>
        /// <returns>Total count of active detained license records.</returns>
        [DocInfo("Retrieves total count of currently active detained licenses.")]
        public static int GetActiveDetainedLicensesCount()
        {
            return clsDetainedLicenseData.GetActiveDetainedLicensesCount();
        }

        #endregion
    }
}
