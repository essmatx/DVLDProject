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
    /// Represents and manages business operations, lookups, and fee updates for system Application Types.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages lookup, search, and fee structure modifications for application types.", Module = "Application Types", Version = "1.0")]
    public class ClsApplicationTypeBusiness
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the application type.
        /// </summary>
        public int ApplicationTypeID { get; set; }

        /// <summary>
        /// Gets or sets the descriptive title of the application type.
        /// </summary>
        public string ApplicationTypeTitle { get; set; }

        /// <summary>
        /// Gets or sets the financial fee required for this application type.
        /// </summary>
        public decimal ApplicationFees { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing application type object instance from the database layer.
        /// </summary>
        /// <param name="ApplicationTypeID">The unique identifier of the application type.</param>
        /// <param name="ApplicationTypeTitle">The descriptive title or name of the application type.</param>
        /// <param name="ApplicationFees">The monetary fee required for this application type.</param>
        private ClsApplicationTypeBusiness(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees = ApplicationFees;
        }

        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Invokes DAL update logic to persist changes to application type title and fees.
        /// </summary>
        /// <returns>True if the update operation succeeded; otherwise, false.</returns>
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }

        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates an application type domain instance by its unique ID.
        /// </summary>
        /// <param name="ApplicationTypeID">The target application type ID.</param>
        /// <returns>Populated <see cref="ClsApplicationTypeBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates an application type by ApplicationTypeID.")]
        public static ClsApplicationTypeBusiness FindApplicationTypeByID(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";

            decimal ApplicationFees = -1;

            if (clsApplicationTypeData.FindApplicationTypeByID(ref ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new ClsApplicationTypeBusiness(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates an application type domain instance by its title.
        /// </summary>
        /// <param name="ApplicationTypeTitle">The exact or partial application type title to search for.</param>
        /// <returns>Populated <see cref="ClsApplicationTypeBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates an application type by its title.")]
        public static ClsApplicationTypeBusiness _FindApplicationTypeByTitle(string ApplicationTypeTitle)
        {
            int ApplicationTypeID = -1;

            decimal ApplicationFees = -1;

            if (clsApplicationTypeData.FindApplicationTypeByTitle(ref ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new ClsApplicationTypeBusiness(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all application types for system-wide lookup and UI grid presentation.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all registered application types.</returns>
        [DocInfo("Retrieves all application types records.")]
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        #endregion

        #region Business Rules & Persistence

        /// <summary>
        /// Persists modifications made to the current application type object.
        /// </summary>
        /// <returns>True if the save operation succeeded; otherwise, false.</returns>
        [DocInfo("Saves modifications for the current application type instance.")]
        public bool Save()
        {
            return _UpdateApplicationType();
        }

        #endregion

    }
}
