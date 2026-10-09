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
    /// Provides create, read, update, and delete operations and query helpers for international license records.
    /// </summary>
    /// <remarks>Implements data access using ADO.NET and stored procedures, obtaining the connection string
    /// from clsDataAccessSettings.Connection. All operations are exposed as static methods that may return sentinel
    /// values or defaults on failure; callers should validate results and handle errors appropriately.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for International Licenses.", Module = "International Licenses Management", Version = "1.0")]
    public class clsInternationalLicenseData
    {
        /// <summary>
        /// Retrieves International License details using its unique international license ID.
        /// </summary>
        /// <param name="InternationalLicenseID">The unique identifier of the international license record.</param>
        /// <param name="ApplicationID">Output parameter for the associated application ID.</param>
        /// <param name="DriverID">Output parameter for the associated driver ID.</param>
        /// <param name="IssuedUsingLocalLicenseID">Output parameter for the local license ID used for issuance.</param>
        /// <param name="IssueDate">Output parameter for the issue date.</param>
        /// <param name="ExpirationDate">Output parameter for the expiration date.</param>
        /// <param name="IsActive">Output parameter indicating whether the international license is active.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <returns>Returns true if the international license record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds an international license record by its international license ID.")]
        [StoredProcedure("SP_FindInternationalLicenseByID")]
        public static bool FindInternationalLicenseByID(ref int InternationalLicenseID, ref int ApplicationID,
           ref int DriverID, ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindInternationalLicenseByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            ApplicationID = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : -1;

                            DriverID = (reader["DriverID"] != DBNull.Value) ? (int)reader["DriverID"] : -1;

                            IssuedUsingLocalLicenseID = (reader["IssuedUsingLocalLicenseID"] != DBNull.Value) ? (int)reader["IssuedUsingLocalLicenseID"] : -1;

                            IssueDate = (reader["IssueDate"] != DBNull.Value) ? (DateTime)reader["IssueDate"] : DateTime.Now;

                            ExpirationDate = (reader["ExpirationDate"] != DBNull.Value) ? (DateTime)reader["ExpirationDate"] : DateTime.Now;

                            IsActive = (reader["IsActive"] != DBNull.Value) ? (bool)reader["IsActive"] : false;

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
        /// Retrieves the active international license ID for a specified driver ID.
        /// </summary>
        /// <param name="DriverID">The driver ID to search for.</param>
        /// <returns>Returns the active InternationalLicenseID if found; otherwise, -1.</returns>
        [DocInfo("Gets an active international license ID for a given driver ID.")]
        [StoredProcedure("SP_GetActiveInternationalLicenseByDriverID")]
        public static int GetActiveInternationalLicenseByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_GetActiveInternationalLicenseByDriverID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null && int.TryParse(results.ToString(), out int ID))
                        {
                            InternationalLicenseID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }
              
            return InternationalLicenseID;
        }

        /// <summary>
        /// Updates an existing international license record in the database.
        /// </summary>
        /// <param name="InternationalLicenseID">The unique identifier of the international license record to update.</param>
        /// <param name="ApplicationID">The updated application ID.</param>
        /// <param name="DriverID">The updated driver ID.</param>
        /// <param name="IssuedUsingLocalLicenseID">The updated local license ID.</param>
        /// <param name="IssueDate">The updated issue date.</param>
        /// <param name="ExpirationDate">The updated expiration date.</param>
        /// <param name="IsActive">The updated active status.</param>
        /// <param name="CreatedByUserID">The user ID who created the record.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing international license record.")]
        [StoredProcedure("SP_UpdateInternationalLicense")]
        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID,
            int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int rows = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateInternationalLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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
        /// Inserts a new international license record into the database and deactivates any existing active licenses for the same driver.
        /// </summary>
        /// <param name="ApplicationID">The application ID associated with the international license.</param>
        /// <param name="DriverID">The driver ID holding the license.</param>
        /// <param name="IssuedUsingLocalLicenseID">The local license ID used to issue this international license.</param>
        /// <param name="IssueDate">The date and time of issue.</param>
        /// <param name="ExpirationDate">The expiration date and time.</param>
        /// <param name="IsActive">Indicates whether the new license is active.</param>
        /// <param name="CreatedByUserID">The user ID who created the record.</param>
        /// <returns>Returns the newly generated InternationalLicenseID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new international license record and returns its ID.")]
        [StoredProcedure("SP_InsertNewInternationalLicense")]
        public static int InsertNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewInternationalLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        object Results = command.ExecuteScalar();

                        if (Results != null && int.TryParse(Results.ToString(), out int ID))
                        {
                            InternationalLicenseID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                    
                }
            }


            return InternationalLicenseID;
        }

        /// <summary>
        /// Retrieves all international license records ordered by ID in descending order.
        /// </summary>
        /// <returns>A DataTable containing all international license records.</returns>
        [DocInfo("Retrieves all international licenses.")]
        [StoredProcedure("SP_GetAllInternationalLicenses")]
        public static DataTable GetAllInternationalLicense()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllInternationalLicenses", connection))
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
        /// Deletes an international license record from the database by its international license ID.
        /// </summary>
        /// <param name="InternationalLicenseID">The unique identifier of the international license record to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes an international license record by its ID.")]
        [StoredProcedure("SP_DeleteInternationalLicense")]
        public static bool DeleteInternationalLicense(int InternationalLicenseID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_DeleteInternationalLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
                  
            }
            return (rowsAffected > 0);
        }

    }
}
