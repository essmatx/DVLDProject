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
    /// Provides CRUD and query operations for local driving license applications.
    /// </summary>
    /// <remarks>Static data-access helper that executes stored procedures to find, insert, update, delete,
    /// and query local driving license application records. Methods return primitives or a DataTable and indicate
    /// failures with false, -1, or null; exceptions are written to the console.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for Local Driving License Applications.", Module = "Local Driving License Applications Management", Version = "1.0")]
    public static class clsLocalDrivingLicenseApplicationData
    {
        /// <summary>
        /// Retrieves local driving license application details by its unique LocalDrivingLicenseApplicationID.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The unique identifier of the local driving license application (input/output).</param>
        /// <param name="ApplicationID">Output parameter for the associated general application ID.</param>
        /// <param name="LicenseClassID">Output parameter for the target license class ID.</param>
        /// <returns>Returns true if the record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a local driving license application by its unique ID.")]
        [StoredProcedure("SP_FindLocalDrivingLicenseAppByID")]
        public static bool FindLocalDrivingLicenseAppByID(ref int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindLocalDrivingLicenseAppByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LocalDrivingLicenseApplicationID = (reader["LocalDrivingLicenseApplicationID"] != DBNull.Value) ? (int)reader["LocalDrivingLicenseApplicationID"] : -1;

                            ApplicationID = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : -1;

                            LicenseClassID = (reader["LicenseClassID"] != DBNull.Value) ? (int)reader["LicenseClassID"] : -1;

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
        /// Retrieves local driving license application details using its associated general ApplicationID.
        /// </summary>
        /// <param name="ApplicationID">The general application ID to search by.</param>
        /// <param name="LocalDrivingLicenseApplicationID">Output parameter for the found local driving license application ID.</param>
        /// <param name="LicenseClassID">Output parameter for the target license class ID.</param>
        /// <returns>Returns true if the record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a local driving license application by its general Application ID.")]
        [StoredProcedure("SP_FindLocalDrivingLicenseAppByAppID")]
        public static bool FindLocalDrivingLicenseAppByAppID(ref int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool Isfound = false;
          
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindLocalDrivingLicenseAppByAppID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LocalDrivingLicenseApplicationID = (reader["LocalDrivingLicenseApplicationID"] != DBNull.Value) ? (int)reader["LocalDrivingLicenseApplicationID"] : -1;

                            ApplicationID = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : -1;

                            LicenseClassID = (reader["LicenseClassID"] != DBNull.Value) ? (int)reader["LicenseClassID"] : -1;

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
        /// Inserts a new local driving license application record into the database.
        /// </summary>
        /// <param name="ApplicationID">The general application ID associated with this record.</param>
        /// <param name="LicenseClassID">The requested license class ID.</param>
        /// <returns>Returns the newly generated LocalDrivingLicenseApplicationID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new local driving license application record and returns its ID.")]
        [StoredProcedure("SP_InsertNewLocalDrivingLicenseApp")]
        public static int InsertNewLocalDrivingLicenseApp(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewLocalDrivingLicenseApp", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();

                        object Results = command.ExecuteScalar();

                        if (Results != null && int.TryParse(Results.ToString(), out int rows))
                        {
                            LocalDrivingLicenseApplicationID = rows;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    catch (Exception ex)
                    {

                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }

            return LocalDrivingLicenseApplicationID;
        }

        /// <summary>
        /// Checks if there is an active (New / Status = 1) local driving license application for a person and license class.
        /// </summary>
        /// <param name="PersonID">The applicant's person ID.</param>
        /// <param name="LicenseClassID">The requested license class ID.</param>
        /// <returns>Returns true if an active application exists; otherwise, false.</returns>
        [DocInfo("Checks whether an active local driving license application exists for a person and license class.")]
        [StoredProcedure("SP_IsThereAnActiveApplication")]
        public static bool IsThereAnActiveApplication(int PersonID, int LicenseClassID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsThereAnActiveApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        /// <summary>
        /// Updates an existing local driving license application record.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The ID of the local driving license application to update.</param>
        /// <param name="ApplicationID">The associated general application ID.</param>
        /// <param name="LicenseClassID">The updated license class ID.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing local driving license application record.")]
        [StoredProcedure("SP_UpdateLocalDrivingLicenseApplication")]
        public static bool UpdateLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rows = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateLocalDrivingLicenseApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        return false;
                    }
                    
                }
            }

           
            return (rows > 0);
        }

        /// <summary>
        /// Deletes a local driving license application record by its unique ID.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The ID of the application record to delete.</param>
        /// <returns>Returns true if deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a local driving license application record by ID.")]
        [StoredProcedure("SP_DeleteLocalDrivingLicenseApplication")]
        public static bool DeleteLocalDrivingLicenseApp(int LocalDrivingLicenseApplicationID)
        {
            int rows = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteLocalDrivingLicenseApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        return false;
                    }

                }
            }


            return (rows > 0);
        }

        /// <summary>
        /// Retrieves all local driving license applications summary view data.
        /// </summary>
        /// <returns>A DataTable containing all local driving license applications.</returns>
        [DocInfo("Retrieves all local driving license application view records.")]
        [StoredProcedure("SP_GetAllLocalDrivingLicenseApplications")]
        public static DataTable GetAllLocalDrivingLicenseApps()
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllLocalDrivingLicenseApplications", connection))
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
                        else
                        {
                            return null;
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
        /// Calculates the total count of passed tests for a given local driving license application.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The unique identifier of the local driving license application.</param>
        /// <returns>Returns the count of passed tests (0-3).</returns>
        [DocInfo("Gets total count of passed tests for an application.")]
        [StoredProcedure("SP_GetPassedTestCount")]
        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {

            int passedTest = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPassedTestCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null && int.TryParse(results.ToString(), out int count))
                        {
                            passedTest = count;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }


          
            return passedTest;
        }
    }
}
