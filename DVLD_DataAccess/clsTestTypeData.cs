using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_DataAccess
{
    /// <summary>
    /// Data Access class responsible for all database operations related to Test Types.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Data Access class responsible for all database operations related to Test Types.")]

    public static class clsTestTypeData
    {
        /// <summary>
        /// Searches the database for a test type record by its unique identifier and populates the output parameters.
        /// </summary>
        /// <param name="TestTypeID">The unique primary key identifier of the test type to search for.</param>
        /// <param name="TestTypeTitle">[Out] Receives the title/name of the test type (returns empty string if NULL).</param>
        /// <param name="TestTypeDescription">[Out] Receives the detailed description of the test type (returns empty string if NULL).</param>
        /// <param name="TestTypeFees">[Out] Receives the fee amount for the test type (returns -1 if NULL).</param>
        /// <returns>
        /// <c>true</c> if a matching test type record was found; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a single test type record by its unique identifier.")]
        [StoredProcedure("SP_FindTestTypeByID")]
        public static bool FindTestTypeByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindTestTypeByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            TestTypeID = (reader["TestTypeID"] != DBNull.Value) ? (int)reader["TestTypeID"] : -1;

                            TestTypeTitle = (reader["TestTypeTitle"] != DBNull.Value) ? (string)reader["TestTypeTitle"] : "";

                            TestTypeDescription = (reader["TestTypeDescription"] != DBNull.Value) ? (string)reader["TestTypeDescription"] : "";

                            TestTypeFees = (reader["TestTypeFees"] != DBNull.Value) ? Convert.ToDecimal(reader["TestTypeFees"]) : -1;
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
        /// Updates the title, description, and fees for an existing test type record.
        /// </summary>
        /// <param name="TestTypeID">The unique primary key identifier of the test type to update.</param>
        /// <param name="TestTypeTitle">The updated title of the test type.</param>
        /// <param name="TestTypeDescription">The updated description of the test type.</param>
        /// <param name="TestTypeFees">The updated fee amount for the test type.</param>
        /// <returns>
        /// <c>true</c> if the record was updated successfully; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Updates an existing test type record in the database.")]
        [StoredProcedure("SP_UpdateTestType")]
        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@TestTypeTitle", (object)TestTypeTitle ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TestTypeDescription", (object)TestTypeDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return (rowsAffected > 0);
                    }
                    catch (Exception ex)
                    {
                        // This will print to your Test Output, so you can see if the error persists
                        clsEventLogger.LogException(ex, "Data Access Error");
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Inserts a new test type record into the database and returns the generated Test Type ID.
        /// </summary>
        /// <param name="Title">The title of the new test type.</param>
        /// <param name="Description">The detailed description of the new test type.</param>
        /// <param name="Fees">The fee amount associated with the new test type.</param>
        /// <returns>
        /// The newly generated identity integer (<c>TestTypeID</c>) if inserted successfully; otherwise, <c>-1</c>.
        /// </returns>
        [DocInfo("Inserts a new test type record into the database and returns the generated ID.")]
        [StoredProcedure("SP_AddNewTestType")]
        public static int AddNewTestType(string Title, string Description, decimal Fees)
        {
            int TestTypeID = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_AddNewTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@TestTypeTitle", Title);
                    command.Parameters.AddWithValue("@TestTypeDescription", Description);
                    command.Parameters.AddWithValue("@ApplicationFees", Fees);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestTypeID = insertedID;
                        }
                    }

                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");

                    }


                }


            }
            return TestTypeID;

        }

        /// <summary>
        /// Retrieves all test type records from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="DataTable"/> containing all test types; returns an empty table if no records exist or an error occurs.
        /// </returns>
        [DocInfo("Retrieves all test type records from the database.")]
        [StoredProcedure("SP_GetAllTestTypes")]
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllTestTypes", connection))
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
                        clsEventLogger.LogException(ex, "Data Access Error"); ;
                    }
                   
                }

            }
            return dt;
        }
    }
}
