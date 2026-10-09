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
    // <summary>
    /// Represents and manages business logic, country lookups, and administrative country management.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages country lookup data, insertions, updates, and global system location entities.", Module = "Countries Management", Version = "1.0")]
    public class clsCountry
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the country business object instance.
        /// </summary>
        enum enMode { _AddNew = 0, _Update = 1 };

        /// <summary>
        /// Tracks current object mode to determine whether to insert or update during persistence.
        /// </summary>
        enMode Mode = enMode._AddNew;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// </summary>
        public int CountryID { set; get; }

        /// <summary>
        /// Gets or sets the full official name of the country.
        /// </summary>
        public string CountryName { set; get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="clsCountry"/> in _AddNew mode with default initial values.
        /// </summary>
        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";

            Mode = enMode._AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing country domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="CountryID">The unique identifier of the country.</param>
        /// <param name="CountryName">The name of the country.</param>
        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;

            Mode = enMode._Update;
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Invokes DAL insertion logic to add a new country record and sets the generated CountryID.
        /// </summary>
        /// <returns>True if insertion succeeded; otherwise, false.</returns>
        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryData.InsertNewCountry(this.CountryName);
            return (this.CountryID != -1);
        }

        /// <summary>
        /// Invokes DAL update logic to modify an existing country record's name.
        /// </summary>
        /// <returns>True if the update succeeded; otherwise, false.</returns>
        private bool _UpdateCountry()
        {
            return clsCountryData.UpdateCountry(this.CountryID, this.CountryName);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a country domain instance by its name.
        /// </summary>
        /// <param name="CountryName">The country name to search for.</param>
        /// <returns>Populated <see cref="clsCountry"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a country record by country name.")]
        public static clsCountry _FindCountry(string CountryName)
        {
            int CountryID = -1;

            if (clsCountryData.FindCountry(ref CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates a country domain instance by its unique Country ID.
        /// </summary>
        /// <param name="CountryID">The unique country identifier.</param>
        /// <returns>Populated <see cref="clsCountry"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a country record by CountryID.")]
        public static clsCountry FindCountryByID(int CountryID)
        {
            string CountryName = "";

            if (clsCountryData.FindCountryByID(ref CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all registered country records for dropdown lists and UI presentation.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all country records.</returns>
        [DocInfo("Retrieves all countries records for lookup lists.")]
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        /// <summary>
        /// Deletes a country record by its unique identifier.
        /// </summary>
        /// <param name="CountryID">The unique country ID to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a country record by CountryID.")]
        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryData.DeleteCountry(CountryID);
        }
        #endregion

        #region Business Rules & Persistence

        /// <summary>
        /// Persists changes to the country record according to the object's current state (<see cref="Mode"/>).
        /// </summary>
        /// <returns>True if the save or update operation succeeded; otherwise, false.</returns>
        [DocInfo("Saves new country or updates existing record based on current mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode._AddNew:

                    if (_AddNewCountry())
                    {
                        Mode = enMode._Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode._Update:
                    return _UpdateCountry();
            }

            return false;

        }

        #endregion
    }
}
