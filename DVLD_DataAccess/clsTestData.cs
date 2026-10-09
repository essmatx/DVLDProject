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
    /// Provides data access methods for CRUD operations and queries against Test records.
    /// </summary>
    /// <remarks>Uses SqlClient to execute stored procedures (named SP_*) and manages connections internally;
    /// methods map DBNull to sensible defaults and indicate success via return values. Relies on
    /// clsDataAccessSettings.Connection for the connection string.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Data Access class responsible for all database operations related to Tests.")]

    public class clsTestData
    {
        /// <summary>
        /// Searches the database for a test record by its unique identifier and populates the output parameters with the test details.
        /// </summary>
        /// <param name="TestID">
        /// [In/Out] On input, specifies the unique ID of the test to search for. On output, holds the validated Test ID.
        /// </param>
        /// <param name="TestAppointmentID">
        /// [Out] Receives the unique identifier of the associated test appointment (returns -1 if NULL).
        /// </param>
        /// <param name="TestResult">
        /// [Out] Receives the result status of the test (true = Passed, false = Failed / default).
        /// </param>
        /// <param name="Notes">
        /// [Out] Receives any descriptive notes or comments recorded during the test (returns empty string if NULL).
        /// </param>
        /// <param name="CreatedByUserID">
        /// [Out] Receives the ID of the system user who created/recorded the test (returns -1 if NULL).
        /// </param>
        /// <returns>
        /// <c>true</c> if a matching test record was found in the database; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a single test record by its unique test identifier.")]
        [StoredProcedure("SP_FindTestByID")]
        public static bool FindTestByID(ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool Isfound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindTestByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TestID", TestID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            TestID = (reader["TestID"] != DBNull.Value) ? (int)reader["TestID"] : -1;

                            TestAppointmentID = (reader["TestAppointmentID"] != DBNull.Value) ? (int)reader["TestAppointmentID"] : -1;

                            TestResult = (reader["TestResult"] != DBNull.Value) ? (bool)reader["TestResult"] : false;

                            Notes = (reader["Notes"] != DBNull.Value) ? (string)reader["Notes"] : "";

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
        /// Searches the database for a test record linked to a specific test appointment ID and populates the output parameters with the test details.
        /// </summary>
        /// <param name="TestID">
        /// [Out] Receives the unique primary key identifier of the matching test record (returns -1 if NULL).
        /// </param>
        /// <param name="TestAppointmentID">
        /// [In/Out] On input, specifies the appointment ID to search for. On output, holds the validated appointment ID.
        /// </param>
        /// <param name="TestResult">
        /// [Out] Receives the result status of the test (true = Passed, false = Failed / default).
        /// </param>
        /// <param name="Notes">
        /// [Out] Receives any evaluator comments or additional notes recorded during the test (returns empty string if NULL).
        /// </param>
        /// <param name="CreatedByUserID">
        /// [Out] Receives the unique ID of the system user who conducted and logged the test (returns -1 if NULL).
        /// </param>
        /// <returns>
        /// <c>true</c> if a test record exists for the specified appointment ID; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Retrieves a test record associated with a specific test appointment identifier.")]
        [StoredProcedure("SP_FindTestByAppointmentID")]
        public static bool FindTestByAppointmentID(ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool Isfound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindTestByAppointmentID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            TestID = (reader["TestID"] != DBNull.Value) ? (int)reader["TestID"] : -1;

                            TestAppointmentID = (reader["TestAppointmentID"] != DBNull.Value) ? (int)reader["TestAppointmentID"] : -1;

                            TestResult = (reader["TestResult"] != DBNull.Value) ? (bool)reader["TestResult"] : false;

                            Notes = (reader["Notes"] != DBNull.Value) ? (string)reader["Notes"] : "";

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
        /// Inserts a new test result record into the database and retrieves the newly generated auto-incremented primary key ID.
        /// </summary>
        /// <param name="TestAppointmentID">
        /// The unique identifier of the scheduled appointment being fulfilled by this test.
        /// </param>
        /// <param name="TestResult">
        /// The evaluation result of the test (<c>true</c> for Passed, <c>false</c> for Failed).
        /// </param>
        /// <param name="Notes">
        /// Optional remarks or evaluator comments regarding the test attempt (accepts null or empty strings).
        /// </param>
        /// <param name="CreatedByUserID">
        /// The unique ID of the system user/examiner creating and auditing this test record.
        /// </param>
        /// <returns>
        /// The auto-generated identity integer (<c>TestID</c>) if the record is inserted successfully; otherwise, <c>-1</c>.
        /// </returns>
        [DocInfo("Inserts a new test record into the database and returns the generated ID.")]
        [StoredProcedure("SP_InsertNewTest")]
        public static int InsertNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewTest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    command.Parameters.AddWithValue("@TestResult", TestResult);

                    command.Parameters.AddWithValue("@Notes", (object)Notes ?? DBNull.Value);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null && int.TryParse(results.ToString(), out int ID))
                        {
                            TestID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }
            
            return TestID;
        }

        /// <summary>
        /// Updates all field values of an existing test record in the database using its unique Test ID.
        /// </summary>
        /// <param name="TestID">
        /// The unique primary key identifier of the test record to update.
        /// </param>
        /// <param name="TestAppointmentID">
        /// The unique identifier of the associated test appointment.
        /// </param>
        /// <param name="TestResult">
        /// The updated evaluation result of the test (<c>true</c> for Passed, <c>false</c> for Failed).
        /// </param>
        /// <param name="Notes">
        /// Optional updated remarks or evaluator comments (passed as <c>DBNull.Value</c> if null).
        /// </param>
        /// <param name="CreatedByUserID">
        /// The unique ID of the system user/examiner updating or managing the record.
        /// </param>
        /// <returns>
        /// <c>true</c> if the test record was found and updated successfully; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Updates an existing test record in the database.")]
        [StoredProcedure("SP_UpdateTest")]
        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int Iseffected = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateTest", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TestID", TestID);

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    command.Parameters.AddWithValue("@TestResult", TestResult);

                    command.Parameters.AddWithValue("@Notes", (object)Notes ?? DBNull.Value);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        Iseffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }

            return (Iseffected > 0);
        }

        /// <summary>
        /// Deletes a specific test record from the database based on its unique primary key identifier.
        /// </summary>
        /// <param name="TestID">
        /// The unique primary key identifier of the test record to be deleted.
        /// </param>
        /// <returns>
        /// <c>true</c> if the test record was found and successfully deleted; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Deletes a test record from the database by its ID.")]
        [StoredProcedure("SP_DeleteTest")]
        public static bool DeleteTest(int TestID)
        {
            int rows = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteTest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestID", TestID);

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
        /// Checks whether a test record has already been recorded for a specific test appointment ID in the database.
        /// </summary>
        /// <param name="TestAppointmentID">
        /// The unique identifier of the scheduled test appointment to verify.
        /// </param>
        /// <returns>
        /// <c>true</c> if a test record exists for the given appointment ID; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Checks whether a test record already exists for a specific test appointment ID.")]
        [StoredProcedure("SP_DoesAppointmentHaveTest")]
        public static bool DoesAppointmentHaveTest(int TestAppointmentID)
        {
            bool Isfound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DoesAppointmentHaveTest", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
        /// Retrieves all test records from the database.
        /// </summary>
        /// <returns>
        /// A <see cref="DataTable"/> containing all rows from the <c>Tests</c> table; returns an empty table if no records exist or an error occurs.
        /// </returns>
        [DocInfo("Retrieves all test records from the database.")]
        [StoredProcedure("SP_GetAllTests")]
        public static DataTable GetAllTest()
        {
            DataTable dt = new DataTable();


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllTests", connection))
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
