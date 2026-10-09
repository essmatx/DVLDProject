using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_DataAccess
{
    /// <summary>
    /// Handles CRUD and query operations for licenses.
    /// </summary>
    /// <remarks>Data-access operations for license entities in the Licenses Management module (Version 1.0);
    /// implemented in the data access layer and uses stored procedures.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for Licenses.", Module = "Licenses Management", Version = "1.0")]
    public class clsLicenseData
    {
        /// <summary>
        /// Retrieves license details using its unique license ID.
        /// </summary>
        /// <param name="LicenseID">The unique identifier of the license (input/output).</param>
        /// <param name="ApplicationID">Output parameter for the associated application ID.</param>
        /// <param name="DriverID">Output parameter for the associated driver ID.</param>
        /// <param name="LicenseClass">Output parameter for the license class ID.</param>
        /// <param name="IssueDate">Output parameter for the issue date.</param>
        /// <param name="ExpirationDate">Output parameter for the expiration date.</param>
        /// <param name="Notes">Output parameter for license notes.</param>
        /// <param name="PaidFees">Output parameter for the paid fees.</param>
        /// <param name="IsActive">Output parameter for active status flag.</param>
        /// <param name="IssueReason">Output parameter for the issue reason code.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <returns>Returns true if the license record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a license record by its License ID.")]
        [StoredProcedure("SP_FindLicenseByID")]
        public static bool FindLicenseByID(ref int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
           ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees,
           ref bool IsActive, ref short IssueReason, ref int CreatedByUserID)
        {
            bool Isfound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindLicenseByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LicenseID = (reader["LicenseID"] != DBNull.Value) ? (int)reader["LicenseID"] : -1;

                            ApplicationID = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : -1;

                            DriverID = (reader["DriverID"] != DBNull.Value) ? (int)reader["DriverID"] : -1;

                            LicenseClass = (reader["LicenseClass"] != DBNull.Value) ? (int)reader["LicenseClass"] : -1;

                            IssueDate = (reader["IssueDate"] != DBNull.Value) ? (DateTime)reader["IssueDate"] : DateTime.Now;

                            ExpirationDate = (reader["ExpirationDate"] != DBNull.Value) ? (DateTime)reader["ExpirationDate"] : DateTime.Now;

                            Notes = (reader["Notes"] != DBNull.Value) ? (string)reader["Notes"] : "";

                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : -1;

                            IsActive = (reader["IsActive"] != DBNull.Value) ? (bool)reader["IsActive"] : false;

                            IssueReason = (reader["IssueReason"] != DBNull.Value) ? Convert.ToInt16(reader["IssueReason"]) : (short)-1;

                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;
                        }
                        else
                        {
                            return false;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        Isfound = false;
                    }

                }
            }
            return Isfound;
        }

        /// <summary>
        /// Retrieves all license records associated with a specific Driver ID.
        /// </summary>
        /// <param name="DriverID">The unique identifier of the driver.</param>
        /// <returns>A DataTable containing all matching driver licenses.</returns>
        [DocInfo("Retrieves all licenses for a specific Driver ID.")]
        [StoredProcedure("SP_FindLicenseByDriverID")]
        public static DataTable FindLicenseByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindLicenseByDriverID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }

            return dt;
        }

        /// <summary>
        /// Finds the active license ID for a given Person ID and License Class ID.
        /// </summary>
        /// <param name="LicenseID">Output parameter for the active license ID.</param>
        /// <param name="PersonID">The unique identifier of the person.</param>
        /// <param name="LicenseClassID">The unique identifier of the license class.</param>
        /// <returns>Returns true if an active license record was found; otherwise, false.</returns>
        [DocInfo("Finds an active license ID by Person ID and License Class ID.")]
        [StoredProcedure("SP_FindActiveLicenseByPersonAndClass")]
        public static bool FindActiveLicensByPersonandClass(ref int LicenseID, int PersonID, int LicenseClassID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindActiveLicenseByPersonAndClass", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    command.Parameters.AddWithValue("@IsActive", true);


                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LicenseID = (reader["LicenseID"] != DBNull.Value) ? (int)reader["LicenseID"] : -1;


                        }
                        else
                        {
                            return false;
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        Isfound = false;
                    }

                }
            }
            return Isfound;
        }

        /// <summary>
        /// Inserts a new license record into the database.
        /// </summary>
        /// <param name="ApplicationID">The associated application ID.</param>
        /// <param name="DriverID">The associated driver ID.</param>
        /// <param name="LicenseClass">The license class ID.</param>
        /// <param name="IssueDate">The date of license issuance.</param>
        /// <param name="ExpirationDate">The date of license expiration.</param>
        /// <param name="Notes">Optional notes for the license.</param>
        /// <param name="PaidFees">The fee amount paid for the license.</param>
        /// <param name="IsActive">Status indicating whether the license is active.</param>
        /// <param name="IssueReason">The reason code for issuing the license.</param>
        /// <param name="CreatedByUserID">The ID of the user creating the record.</param>
        /// <returns>Returns the newly generated LicenseID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new license record and returns its ID.")]
        [StoredProcedure("SP_InsertNewLicense")]
        public static int InsertNewLicense(int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
           bool IsActive, short IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        object Results = command.ExecuteScalar();

                        if (Results != null && int.TryParse(Results.ToString(), out int ID))
                        {
                            LicenseID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }


            return LicenseID;
        }

        /// <summary>
        /// Updates an existing license record in the database.
        /// </summary>
        /// <param name="LicenseID">The unique identifier of the license to update.</param>
        /// <param name="ApplicationID">The associated application ID.</param>
        /// <param name="DriverID">The associated driver ID.</param>
        /// <param name="LicenseClass">The license class ID.</param>
        /// <param name="IssueDate">The updated issue date.</param>
        /// <param name="ExpirationDate">The updated expiration date.</param>
        /// <param name="Notes">The updated notes.</param>
        /// <param name="PaidFees">The updated paid fees.</param>
        /// <param name="IsActive">The updated active status.</param>
        /// <param name="IssueReason">The updated issue reason code.</param>
        /// <param name="CreatedByUserID">The user ID modifying/creating the record.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing license record.")]
        [StoredProcedure("SP_UpdateLicense")]
        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
            bool IsActive, short IssueReason, int CreatedByUserID)
        {
            int row = -1;
          
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(Notes) ? (object)DBNull.Value : Notes);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        row = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }

            return (row > 0);
        }

        /// <summary>
        /// Deletes a license record from the database by its unique license ID.
        /// </summary>
        /// <param name="LicenseID">The unique identifier of the license to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a license record by its ID.")]
        [StoredProcedure("SP_DeleteLicense")]
        public static bool DeleteLicense(int LicenseID)
        {
            int rows = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }


                }
            }

            return (rows > 0);
        }

        /// <summary>
        /// Deactivates a license record by setting its IsActive status flag to false.
        /// </summary>
        /// <param name="LicenseID">The unique identifier of the license to deactivate.</param>
        /// <returns>Returns true if the license was successfully deactivated; otherwise, false.</returns>
        [DocInfo("Deactivates an active license by setting IsActive to false.")]
        [StoredProcedure("SP_DeactivateLicense")]
        public static bool DeactivateLicense(int LicenseID)
        {
            int Deactivated = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeactivateLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        Deactivated = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");

                    }


                }
            }

            return (Deactivated > 0);
        }

        /// <summary>
        /// Retrieves all license records from the database.
        /// </summary>
        /// <returns>A DataTable containing all license records.</returns>
        [DocInfo("Retrieves all license records.")]
        [StoredProcedure("SP_GetAllLicenses")]
        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllLicenses", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }

            }
            return dt;
        }

        /// <summary>
        /// Checks whether a license record exists for a given Application ID.
        /// </summary>
        /// <param name="ApplicationID">The unique identifier of the application.</param>
        /// <returns>Returns true if a license exists for the application; otherwise, false.</returns>
        [DocInfo("Checks if a license exists for a given Application ID.")]
        [StoredProcedure("SP_DoesLicenseExistForApplication")]
        public static bool DoesLicenseExistForApplication(int ApplicationID)
        {
            bool isFound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesLicenseExistForApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            isFound = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }

                return isFound;
            }
        }

        /// <summary>
        /// Gets the active license ID associated with a given Application ID.
        /// </summary>
        /// <param name="ApplicationID">The unique identifier of the application.</param>
        /// <returns>Returns the LicenseID if found; otherwise, -1.</returns>
        [DocInfo("Retrieves active license ID associated with a given Application ID.")]
        [StoredProcedure("SP_GetActiveLicenseIDByApplicationID")]
        public static int GetActiveLicenseIDByApplicationID(int ApplicationID)
        {
            int LicenseID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using (SqlCommand command = new SqlCommand("SP_GetActiveLicenseIDByApplicationID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                   

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                    return LicenseID;
                }
            }
        }

        /// <summary>
        /// Retrieves total count of active licenses in the database.
        /// </summary>
        /// <returns>Returns count of active licenses.</returns>
        [DocInfo("Gets total count of active licenses.")]
        [StoredProcedure("SP_GetActualActiveLicensesCount")]
        public static int GetActualActiveLicensesCount()
        {
            int activeCount = 0;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetActualActiveLicensesCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open(); // Opened cleanly ONCE
                        object result = command.ExecuteScalar();

                        // Clean 'if' statement with a proper string conversion
                        if (result != null && int.TryParse(result.ToString(), out int count))
                        {
                            activeCount = count;
                        }
                    }
                    catch (Exception ex)
                    {

                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }
            return activeCount;
        }

        /// <summary>
        /// Retrieves total count of all license records in the database.
        /// </summary>
        /// <returns>Returns total count of license records.</returns>
        [DocInfo("Gets total count of all licenses.")]
        [StoredProcedure("SP_GetTotalLicensesCount")]
        public static int GetTotalLicensesCount()
        {
            int totalCount = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTotalLicensesCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int count))
                        {
                            totalCount = count;
                        }
                    }
                    catch (Exception ex)
                    {
                        if(totalCount == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return totalCount;
        }

        /// <summary>
        /// Gets the total count of active licenses expiring within a given number of days.
        /// </summary>
        /// <param name="days">Days threshold to check for expiration (default 30 days).</param>
        /// <returns>Returns count of licenses expiring within the specified days.</returns>
        [DocInfo("Gets count of active licenses expiring within specified days.")]
        [StoredProcedure("SP_GetExpiringSoonLicensesCount")]
        public static int GetExpiringSoonLicensesCount(int days = 30)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetExpiringSoonLicensesCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Days", days);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int parsedCount))
                        {
                            count = parsedCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }

            return count;
        }
    }

}
