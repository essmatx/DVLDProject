using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_BusinessWorkflows
{
    /// <summary>
    /// Provides business-workflow operations for issuing and managing international driver licenses.
    /// </summary>
    /// <remarks>Validates driver and local license presence, prevents duplicate international licenses,
    /// creates and persists an international license with a one-year expiration, and returns the new license identifier
    /// or negative error codes for failure cases (−1: driver not found, −2: no active local license, −3: existing
    /// international license, −4: save failed). Depends on ClsDriverBusiness, ClsLicenseBusiness, and
    /// ClsInternationalLicenseBusiness.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_BusinessWorkflows)]
    [DocInfo(" International License Management", Version = "1.0")]
    public partial class ClsInternationalLicenseWorkflow
    {
        /// <summary>
        /// Creates and activates an international driving license for the specified driver when prerequisites are met.
        /// </summary>
        /// <remarks>Requires an active local (regular car) license and that no active international
        /// license already exists. Sets IssueDate to the current time and ExpirationDate to one year later, then saves
        /// and activates the record.</remarks>
        /// <param name="driverID">ID of the driver for whom the international license will be issued.</param>
        /// <param name="applicationID">ID of the application associated with the international license.</param>
        /// <param name="createdByUserID">ID of the user creating the international license record.</param>
        /// <returns>The newly created international license ID on success; negative values indicate failure: -1 = driver not
        /// found; -2 = no active local license; -3 = an active international license already exists; -4 = failed to
        /// save the license.</returns>
        public static int IssueInternationalLicense(int driverID,int applicationID,int createdByUserID)
        {
            ClsDriverBusiness Driver = ClsDriverBusiness.FindDriverByID(driverID);

            if (Driver == null)
            {
                return -1; 
            }

            ClsLicenseBusiness localLicense = ClsLicenseBusiness.FindActiveLicensByPersonandClass(Driver.PersonID,(int)ClsLicensClassBusiness.enLicenseClass.RegularCar); 

            if(localLicense == null)
            {
                return -2; 
            }

            ClsInternationalLicenseBusiness InternationalLicense = ClsInternationalLicenseBusiness.FindActiveInternationalLicenseByDriverID(driverID);

            if(InternationalLicense != null)
            {
                return -3; 
            }

            ClsInternationalLicenseBusiness intLicense = new ClsInternationalLicenseBusiness();
            intLicense.ApplicationID = applicationID;
            intLicense.DriverID = driverID;
            intLicense.IssuedUsingLocalLicenseID = localLicense.LicenseID;
            intLicense.IssueDate = DateTime.Now;
            intLicense.ExpirationDate = DateTime.Now.AddYears(1);
            intLicense.IsActive = true;
            intLicense.CreatedByUserID = createdByUserID;

            if(!intLicense.Save())
            {
                return -4; 
            }
            return intLicense.InternationalLicenseID;
        }
    }
}
