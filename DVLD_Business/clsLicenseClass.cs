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
    /// Represents and manages license classes, including driving qualification prerequisites, minimum allowed age, validity durations, fee schedules, and persistence logic.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages license class definitions, age limits, validity durations, fee schedules, and persistence.", Module = "Licenses Management", Version = "1.0")]
    public class ClsLicensClassBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the license class business object instance.
        /// </summary>
        public enum enMod { AddNew  = 0 ,  Update = 1};

        /// <summary>
        /// Standard enumeration for predefined license class categories in the system.
        /// </summary>
        public enum enLicenseClass
        {
            /// <summary>
            /// Small Motorcycle (Class 1)
            /// </summary>
            SmallMotorcycle = 1,

            /// <summary>
            /// Heavy Motorcycle (Class 2)
            /// </summary>
            HeavyMotorcycle = 2,

            /// <summary>
            /// Regular Ordinary Passenger Car (Class 3)
            /// </summary>
            RegularCar = 3,

            /// <summary>
            /// Commercial Transport Vehicle (Class 4)<
            /// </summary>
            Commercial = 4,

            /// <summary>
            /// Agricultural Tractor & Equipment (Class 5)
            /// </summary>
            Agricultural = 5,

            /// <summary>
            /// Small to Medium Passenger Bus (Class 6)
            /// </summary>
            SmallMediumBus = 6,

            /// <summary>
            /// Heavy Large Bus (Class 7)
            /// </summary>
            HeavyBus = 7
        };

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        enMod Mode = enMod.AddNew;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets the unique identifier for the license class.
        /// </summary>
        public int LicenseClassID { get; private set; }

        /// <summary>
        /// Gets or sets the formal name of the license class.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the detailed description of vehicles covered under this class.
        /// </summary>
        public string ClassDescription { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed driver age required to apply for this license class.
        /// </summary>
        public short MiniMumAllowedAge { get;  set; }

        /// <summary>
        /// Gets or sets the default validity duration in years for licenses issued under this class.
        /// </summary>
        public short DefaultValidityLingth { get;  set; }

        /// <summary>
        /// Gets or sets the standard fee amount charged for issuing or renewing this license class.
        /// </summary>
        
        public decimal ClassFees { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ClsLicensClassBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsLicensClassBusiness()
        {
            // Corrected: Assign directly to the class properties using 'this.'
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MiniMumAllowedAge = 0; // Standard default minimum age
            this.DefaultValidityLingth = 0;
            this.ClassFees = 0.0m;

            this.Mode = enMod.AddNew;

        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing license class domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="LicenseClassID">The unique license class identifier.</param>
        /// <param name="ClassName">The formal name of the license class.</param>
        /// <param name="ClassDescription">The description of the license class.</param>
        /// <param name="MiniMumAllowedAge">The minimum age required for applicants.</param>
        /// <param name="DefaultValidityLingth">The standard validity length in years.</param>
        /// <param name="ClassFees">The required fee for this license class.</param>
        private ClsLicensClassBusiness(int LicenseClassID, string ClassName, string ClassDescription,
            short MiniMumAllowedAge, short DefaultValidityLingth, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MiniMumAllowedAge = MiniMumAllowedAge;
            this.DefaultValidityLingth = DefaultValidityLingth;
            this.ClassFees = ClassFees;
            this.Mode = enMod.Update; 


        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion logic to add a new license class and updates internal LicenseClassID state.
        /// </summary>
        /// <returns>True if insertion succeeded; otherwise, false.</returns>
        private bool AddNewLicenseClass()
        {
            // 1. Call the DAL and capture the returned ID (int)
            int InsertedID = clsLicenseClassData.InsertNewLicenseClass(
                this.ClassName,
                this.ClassDescription,
                this.MiniMumAllowedAge,       // Kept your casing 'MiniMumAllowedAge'
                this.DefaultValidityLingth,   // Kept your spelling 'DefaultValidityLingth'
                this.ClassFees
            );

            // 2. Assign the newly generated ID to this object's ID property
            this.LicenseClassID = InsertedID;

            // 3. Return true if the ID is valid (not -1), meaning the insert succeeded!
            return (this.LicenseClassID != -1);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing license class record.
        /// </summary>
        /// <returns>True if update succeeded; otherwise, false.</returns>
        private bool UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName,
                this.ClassDescription, this.MiniMumAllowedAge, this.DefaultValidityLingth, this.ClassFees);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a license class business instance by its unique License Class ID.
        /// </summary>
        /// <param name="LicenseClassID">The unique license class identifier.</param>
        /// <returns>Populated <see cref="ClsLicensClassBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a license class definition by LicenseClassID.")]
        public static ClsLicensClassBusiness FindLicenseClassByID(int LicenseClassID)
        {
            string ClassName = "";
            string ClassDescription = "";
            short MiniMumAllowedAge = -1;
            short DefaultValidityLingth = -1;
            decimal ClassFees = -1;

            if (clsLicenseClassData.FindLicensClassByID(ref LicenseClassID, ref ClassName,
                ref ClassDescription, ref MiniMumAllowedAge, ref DefaultValidityLingth, ref ClassFees))
            {
                return new ClsLicensClassBusiness(LicenseClassID, ClassName, ClassDescription, MiniMumAllowedAge, DefaultValidityLingth, ClassFees);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Finds and hydrates a license class business instance by its formal class name.
        /// </summary>
        /// <param name="ClassName">The formal name of the license class to search for.</param>
        /// <returns>Populated <see cref="ClsLicensClassBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a license class definition by ClassName.")]
        public static ClsLicensClassBusiness FindLicenseClassByClassName(string ClassName)
        {
            int LicenseClassID = -1;
            string ClassDescription = "";
            short MiniMumAllowedAge = -1;
            short DefaultValidityLingth = -1;
            decimal ClassFees = -1;

            if (clsLicenseClassData.FindLicensClassByClassName(ref LicenseClassID, ref ClassName,
                ref ClassDescription, ref MiniMumAllowedAge, ref DefaultValidityLingth, ref ClassFees))
            {
                return new ClsLicensClassBusiness(LicenseClassID, ClassName, ClassDescription, MiniMumAllowedAge, DefaultValidityLingth, ClassFees);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Retrieves all license class definitions from the system for dropdowns and administration grids.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all license class records.</returns>
        [DocInfo("Retrieves all license class records for dropdown selections and administration display.")]
        public static DataTable GetAallLicenseClass()
        {
            return clsLicenseClassData.GetAllLicenseClass();
        }

        /// <summary>
        /// Deletes a license class definition by its unique identifier.
        /// </summary>
        /// <param name="LicenseClassID">The unique license class identifier to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a license class definition by LicenseClassID.")]
        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            return clsLicenseClassData.DeleteLicenseClass(LicenseClassID);
        }
        #endregion

        #region Business Rules & Workflows

        /// <summary>
        /// Persists changes to the license class record according to the object's current state (<see cref="Mode"/>).
        /// </summary>
        /// <returns>True if save or update succeeded; otherwise, false.</returns>
        [DocInfo("Saves new license class definition or updates existing record based on current mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMod.AddNew:
                    // 1. Try to add the new license class
                    if (AddNewLicenseClass())
                    {
                        // 2. If successful, switch the mode to Update so next saves edit this same record
                        Mode = enMod.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMod.Update:
                    // 3. Run the update query and return whether it succeeded or failed
                    return UpdateLicenseClass();
            }

            return false;
        }
        #endregion

    }
}
