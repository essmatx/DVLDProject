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
   /// Provides static data-access methods for user records, including retrieval by ID or username, credential
   /// validation, insertion, update, deletion, existence checks, password changes, and retrieving all users.
   /// </summary>
   /// <remarks>All members are static and execute stored procedures via System.Data.SqlClient using the
   /// connection string in clsDataAccessSettings.Connection. Exceptions are logged to clsEventLogger and methods return
   /// default values on failure (for example, -1, empty strings, false, or an empty DataTable). Callers are responsible
   /// for validating inputs and handling sensitive data such as passwords appropriately.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Data Access class responsible for all database operations related to Users.")]
    public static class clsUserData
    {
        /// <summary>
        /// Searches the database for a user record by its unique identifier and populates the output parameters.
        /// </summary>
        /// <param name="UserID">The unique primary key identifier of the user to search for.</param>
        /// <param name="PersonID">[Out] Receives the associated person ID (returns -1 if NULL).</param>
        /// <param name="UserName">[Out] Receives the username (returns empty string if NULL).</param>
        /// <param name="Password">[Out] Receives the account password (returns empty string if NULL).</param>
        /// <param name="IsActive">[Out] Receives the active status flag of the account (returns false if NULL).</param>
        /// <returns>
        /// <c>true</c> if a matching user record was found; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a single user record by its unique identifier.")]
        [StoredProcedure("SP_FindUserByID")]
        public static bool FindUserByID(ref int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool Isfound = false;

         
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindUserByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Isfound = true;
                                PersonID = (reader["PersonID"] != DBNull.Value) ? (int)reader["PersonID"] : (int)-1;
                                UserName = (reader["UserName"] != DBNull.Value) ? (string)reader["UserName"] : "";
                                Password = (reader["Password"] != DBNull.Value) ? (string)reader["Password"] : "";
                                IsActive = (reader["IsActive"] != DBNull.Value) ? (bool)reader["IsActive"] : false;
                            }
                        }
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
        /// Searches the database for a user record by username and populates the output parameters.
        /// </summary>
        /// <param name="UserName">The username to search for.</param>
        /// <param name="UserID">[Out] Receives the unique User ID (returns -1 if NULL).</param>
        /// <param name="PersonID">[Out] Receives the associated Person ID (returns -1 if NULL).</param>
        /// <param name="Password">[Out] Receives the account password (returns empty string if NULL).</param>
        /// <param name="IsActive">[Out] Receives the active status flag (returns false if NULL).</param>
        /// <returns>
        /// <c>true</c> if a matching user record was found; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a single user record by username.")]
        [StoredProcedure("SP_FindUserByUserName")]
        public static bool FindUserByUserName(ref int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindUserByUserName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@UserName", UserName);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                Isfound = true;
                                UserID = (reader["UserID"] != DBNull.Value) ? (int)reader["UserID"] : (int)-1;
                                PersonID = (reader["PersonID"] != DBNull.Value) ? (int)reader["PersonID"] : (int)-1;
                                Password = (reader["Password"] != DBNull.Value) ? (string)reader["Password"] : "";
                                IsActive = (reader["IsActive"] != DBNull.Value) ? (bool)reader["IsActive"] : false;
                            }
                        }
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
        /// Validates credentials by searching for a matching username and password combination.
        /// </summary>
        /// <param name="UserName">The username credentials to check.</param>
        /// <param name="Password">The password credentials to check.</param>
        /// <param name="UserID">[Out] Receives the unique User ID (returns -1 if NULL).</param>
        /// <param name="PersonID">[Out] Receives the associated Person ID (returns -1 if NULL).</param>
        /// <param name="IsActive">[Out] Receives the active status flag (returns false if NULL).</param>
        /// <returns>
        /// <c>true</c> if matching user credentials were found; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a single user record matching username and password.")]
        [StoredProcedure("SP_FindUserByUserNameAndPassword")]
        public static bool FindUserByUserNameAndPassowrd(ref int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool Isfound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindUserByUserNameAndPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Isfound = true;
                                UserID = (reader["UserID"] != DBNull.Value) ? (int)reader["UserID"] : (int)-1;
                                PersonID = (reader["PersonID"] != DBNull.Value) ? (int)reader["PersonID"] : (int)-1;
                                Password = (reader["Password"] != DBNull.Value) ? (string)reader["Password"] : "";
                                IsActive = (reader["IsActive"] != DBNull.Value) ? (bool)reader["IsActive"] : false;
                            }
                        }
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
        /// Inserts a new user record into the database and returns the generated User ID.
        /// </summary>
        /// <param name="PersonID">The person ID associated with the user account.</param>
        /// <param name="UserName">The username for the account.</param>
        /// <param name="Password">The account password.</param>
        /// <param name="IsActive">The active status of the account.</param>
        /// <returns>
        /// The newly generated identity integer (<c>UserID</c>) if inserted successfully; otherwise, <c>-1</c>.
        /// </returns>
        [DocInfo("Inserts a new user record into the database and returns the generated ID.")]
        [StoredProcedure("SP_InsertNewUser")]
        public static int InsertNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName.Trim());
                    command.Parameters.AddWithValue("@Password", Password.Trim());
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int Inserted))
                        {
                            UserID = Inserted;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error"); ;
                    }
                }
            }

            return UserID;
        }

        /// <summary>
        /// Updates person ID, username, password, and active status for an existing user record.
        /// </summary>
        /// <param name="UserID">The unique primary key identifier of the user to update.</param>
        /// <param name="PersonID">The updated person ID.</param>
        /// <param name="UserName">The updated username.</param>
        /// <param name="Password">The updated password.</param>
        /// <param name="IsActive">The updated active status flag.</param>
        /// <returns>
        /// <c>true</c> if the record was updated successfully; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Updates an existing user record in the database.")]
        [StoredProcedure("SP_UpdateUser")]
        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {

            int result = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateUser", connection))
                {

                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName.Trim());
                    command.Parameters.AddWithValue("@Password", Password.Trim());
                    command.Parameters.AddWithValue("@IsActive", IsActive);


                    try
                    {
                        connection.Open();
                        result = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }

            return (result > 0);
        }

        /// <summary>
        /// Deletes a user record from the database by its unique identifier.
        /// </summary>
        /// <param name="UserID">The unique primary key identifier of the user to delete.</param>
        /// <returns>
        /// <c>true</c> if the record was deleted successfully; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Deletes an existing user record from the database.")]
        [StoredProcedure("SP_DeleteUser")]
        public static bool DeleteUser(int UserID)
        {
            int rows = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@UserID", UserID);

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
        /// Retrieves all user records joined with person full names from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="DataTable"/> containing all user records; returns an empty table if no records exist or an error occurs.
        /// </returns>
        [DocInfo("Retrieves all user records from the database.")]
        [StoredProcedure("SP_GetAllUsers")]
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
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
        /// Checks whether a user record exists in the database by Username.
        /// </summary>
        /// <param name="UserName">The username string to check.</param>
        /// <returns>
        /// <c>true</c> if the user exists; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Checks if a user exists in the database by Username.")]
        [StoredProcedure("SP_IsUserExistByUserName")]
        public static bool IsUserExists(string Username)
        {
            bool Isfound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsUserExistByUserName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@Username", Username);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null)
                        {
                            Isfound = true;
                        }
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
        /// Updates the password for a specific user record.
        /// </summary>
        /// <param name="UserID">The unique primary key identifier of the user.</param>
        /// <param name="NewPassword">The new password value to assign.</param>
        /// <returns>
        /// <c>true</c> if the password was updated successfully; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Updates the password for a specific user record.")]
        [StoredProcedure("SP_ChangeUserPassword")]
        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = -1;
            
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using(SqlCommand command = new SqlCommand("SP_ChangeUserPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Password", NewPassword.Trim());

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }
           
            return (rowsAffected > 0);
        }
    }
}
