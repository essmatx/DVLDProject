using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_DataAccess
{
    /// <summary>
    /// Provides data-access operations for license classes, including create, read, update, and delete functionality.
    /// </summary>
    /// <remarks>Performs database operations via ADO.NET using stored procedures and
    /// clsDataAccessSettings.Connection. Methods execute SQL commands and may return identifiers or success flags;
    /// callers should validate inputs and handle failures.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for License Classes.", Module = "License Classes Management", Version = "1.0")]
    public class clsLicenseClassData
    {
        /// <summary>
        /// Retrieves license class details using its unique license class ID.
        /// </summary>
        /// <param name="LicenseClassID">The unique identifier of the license class (input/output).</param>
        /// <param name="ClassName">Output parameter for the class name.</param>
        /// <param name="ClassDescription">Output parameter for the class description.</param>
        /// <param name="MinimumAllowedAge">Output parameter for the minimum allowed age.</param>
        /// <param name="DefaultValidityLength">Output parameter for the default validity length in years.</param>
        /// <param name="ClassFees">Output parameter for the class fees.</param>
        /// <returns>Returns true if the license class record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a license class record by its license class ID.")]
        [StoredProcedure("SP_FindLicenseClassByID")]
        public static bool FindLicensClassByID(ref int LicenseClassID, ref string ClassName, ref string ClassDescription, ref short MinimumAllowedAge
         , ref short DefaultValidityLength, ref decimal ClassFees)
        {
            bool Isfound = false;
          
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindLicenseClassByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LicenseClassID = (reader["LicenseClassID"] != DBNull.Value) ? (int)reader["LicenseClassID"] : -1;
                            ClassName = (reader["ClassName"] != DBNull.Value) ? (string)reader["ClassName"] : "";
                            ClassDescription = (reader["ClassDescription"] != DBNull.Value) ? (string)reader["ClassDescription"] : "";
                            MinimumAllowedAge = (reader["MinimumAllowedAge"] != DBNull.Value) ? Convert.ToInt16(reader["MinimumAllowedAge"]) : (short)-1;
                            DefaultValidityLength = (reader["DefaultValidityLength"] != DBNull.Value) ? Convert.ToInt16(reader["DefaultValidityLength"]) : (short)-1;
                            ClassFees = (reader["ClassFees"] != DBNull.Value) ? Convert.ToDecimal(reader["ClassFees"]) : (decimal)-1;
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
        /// Retrieves license class details using its class name.
        /// </summary>
        /// <param name="LicenseClassID">Output parameter for the license class ID.</param>
        /// <param name="ClassName">The class name to search for (input/output).</param>
        /// <param name="ClassDescription">Output parameter for the class description.</param>
        /// <param name="MinimumAllowedAge">Output parameter for the minimum allowed age.</param>
        /// <param name="DefaultValidityLength">Output parameter for the default validity length in years.</param>
        /// <param name="ClassFees">Output parameter for the class fees.</param>
        /// <returns>Returns true if the license class record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a license class record by its class name.")]
        [StoredProcedure("SP_FindLicenseClassByClassName")]
        public static bool FindLicensClassByClassName(ref int LicenseClassID, ref string ClassName, ref string ClassDescription, ref short MinimumAllowedAge
            , ref short DefaultValidityLength, ref decimal ClassFees)
        {
            bool Isfound = false;
        
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_FindLicenseClassByClassName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ClassName", ClassName);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            LicenseClassID = (reader["LicenseClassID"] != DBNull.Value) ? (int)reader["LicenseClassID"] : -1;
                            ClassName = (reader["ClassName"] != DBNull.Value) ? (string)reader["ClassName"] : "";
                            ClassDescription = (reader["ClassDescription"] != DBNull.Value) ? (string)reader["ClassDescription"] : "";
                            MinimumAllowedAge = (reader["MinimumAllowedAge"] != DBNull.Value) ? Convert.ToInt16(reader["MinimumAllowedAge"]) : (short)-1;
                            DefaultValidityLength = (reader["DefaultValidityLength"] != DBNull.Value) ? Convert.ToInt16(reader["DefaultValidityLength"]) : (short)-1;
                            ClassFees = (reader["ClassFees"] != DBNull.Value) ? Convert.ToDecimal(reader["ClassFees"]) : (decimal)-1;
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
        /// Updates an existing license class record in the database.
        /// </summary>
        /// <param name="LicenseClassID">The unique identifier of the license class to update.</param>
        /// <param name="ClassName">The updated class name.</param>
        /// <param name="ClassDescription">The updated class description.</param>
        /// <param name="MinimumAllowedAge">The updated minimum allowed age.</param>
        /// <param name="DefaultValidityLength">The updated default validity length in years.</param>
        /// <param name="ClassFees">The updated class fees.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing license class record.")]
        [StoredProcedure("SP_UpdateLicenseClass")]
        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, short MinimumAllowedAge
            , short DefaultValidityLength, decimal ClassFees)
        {
            int rows = 0;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateLicenseClass", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@ClassName", (object)ClassName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ClassDescription", (object)ClassDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", ClassFees);

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
        /// Inserts a new license class record into the database.
        /// </summary>
        /// <param name="ClassName">The class name.</param>
        /// <param name="ClassDescription">The class description.</param>
        /// <param name="MinimumAllowedAge">The minimum allowed age for the license class.</param>
        /// <param name="DefaultValidityLength">The default validity length in years.</param>
        /// <param name="ClassFees">The fees associated with the license class.</param>
        /// <returns>Returns the newly generated LicenseClassID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new license class record and returns its ID.")]
        [StoredProcedure("SP_InsertNewLicenseClass")]
        public static int InsertNewLicenseClass(string ClassName, string ClassDescription, short MinimumAllowedAge, short DefaultValidityLength, decimal ClassFees)
        {
            int NewClassID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewLicenseClass", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassName", ClassName);
                    command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", ClassFees);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            NewClassID = insertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        NewClassID = -1;
                    }
                } // Closes command
            } // Closes connection

            return NewClassID; // Returns the newly generated ID
        }

        /// <summary>
        /// Deletes a license class record from the database by its license class ID.
        /// </summary>
        /// <param name="LicenseClassID">The unique identifier of the license class record to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a license class record by its ID.")]
        [StoredProcedure("SP_DeleteLicenseClass")]
        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            int rowsAffected = 0;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteLicenseClass", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        return false;
                    }
                }
            }

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Retrieves all license class records from the database.
        /// </summary>
        /// <returns>A DataTable containing all license class records.</returns>
        [DocInfo("Retrieves all license classes.")]
        [StoredProcedure("SP_GetAllLicenseClasses")]
        public static DataTable GetAllLicenseClass()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllLicenseClasses", connection))
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

    }
}
