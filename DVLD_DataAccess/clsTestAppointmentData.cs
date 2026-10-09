 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DVLD_Shared;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_DataAccess

{
    /// <summary>
    /// Provides CRUD and query operations for test appointments.
    /// </summary>
    /// <remarks>Data-access layer for the Test Appointment Management module. Uses ADO.NET and stored
    /// procedures to perform create, read, update, delete, and query operations; methods are static and use the
    /// connection defined in clsDataAccessSettings.Connection. Errors are written to the console.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for Test Appointments.", Module = "Test Appointment Management", Version = "1.0")]
    public class clsTestAppointmentData
    {
        /// <summary>
        /// Retrieves a single test appointment record by its unique identifier.
        /// </summary>
        /// <param name="TestAppointmentID">The primary key ID of the test appointment to find.</param>
        /// <param name="TestTypeID">Receives the associated test type ID.</param>
        /// <param name="LocalDrivingLicenseApplicationID">Receives the associated local driving license application ID.</param>
        /// <param name="AppointmentDate">Receives the scheduled appointment date and time.</param>
        /// <param name="PaidFees">Receives the fees paid for the appointment.</param>
        /// <param name="CreatedByUserID">Receives the ID of the user who created the record.</param>
        /// <param name="IsLocked">Receives a boolean value indicating if the appointment is locked.</param>
        /// <returns>True if the test appointment was found; otherwise, false.</returns>
        [DocInfo("Searches the database for a test appointment by ID.")]
        [StoredProcedure("SP_FindTestAppointmentByID")]
        public static bool FindTestAppointmentByID(ref int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindTestAppointmentByID", connection))
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

                            TestAppointmentID = (reader["TestAppointmentID"] != DBNull.Value) ? (int)reader["TestAppointmentID"] : -1;

                            TestTypeID = (reader["TestTypeID"] != DBNull.Value) ? (int)reader["TestTypeID"] : -1;

                            LocalDrivingLicenseApplicationID = (reader["LocalDrivingLicenseApplicationID"] != DBNull.Value) ? (int)reader["LocalDrivingLicenseApplicationID"] : -1;

                            AppointmentDate = (reader["AppointmentDate"] != DBNull.Value) ? (DateTime)reader["AppointmentDate"] : DateTime.Now;

                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : (decimal)-1;

                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;

                            IsLocked = (reader["IsLocked"] != DBNull.Value) ? (bool)reader["IsLocked"] : false;
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
        /// Retrieves all test appointments for a specific application and test type.
        /// </summary>
        /// <param name="TestTypeID">The ID of the test type (e.g., Vision, Written, Street).</param>
        /// <param name="LocalDrivingLicenseApplicationID">The local driving license application ID.</param>
        /// <returns>A DataTable containing matching test appointment records.</returns>
        [DocInfo( "Fetches test appointments filtered by Application ID and Test Type ID.")]
        [StoredProcedure("SP_GetTestAppointmentsByLocalAppAndTestType")]
        public static DataTable GetTestAppointmentsByLocalAppAndTestTyp(int TestTypeID, int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTestAppointmentsByLocalAppAndTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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
        /// Inserts a new test appointment record into the database.
        /// </summary>
        /// <param name="TestTypeID">The test type ID.</param>
        /// <param name="LocalDrivingLicenseApplicationID">The local driving license application ID.</param>
        /// <param name="AppointmentDate">The date and time of the appointment.</param>
        /// <param name="PaidFees">The fee amount paid for the appointment.</param>
        /// <param name="CreatedByUserID">The user ID creating the appointment.</param>
        /// <param name="IsLocked">Initial lock status (typically false when newly scheduled).</param>
        /// <returns>The newly generated TestAppointmentID, or -1 if insertion fails.</returns>
        [DocInfo("Creates a new test appointment and returns the generated ID.")]
        [StoredProcedure("SP_InserNewTestAppointment")]
        public static int InserNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {
            int TestAppointmentID = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InserNewTestAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

                    command.Parameters.AddWithValue("@PaidFees", PaidFees);

                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    command.Parameters.AddWithValue("@IsLocked", IsLocked);

                    try
                    {
                        connection.Open();

                        object Results = command.ExecuteScalar();

                        if (Results != null && int.TryParse(Results.ToString(), out int rows))
                        {
                            TestAppointmentID = rows;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   

                }

            }

            return TestAppointmentID;
        }

        /// <summary>
        /// Checks whether an unlocked (active) appointment already exists for a given application and test type.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The local application ID.</param>
        /// <param name="TestTypeID">The test type ID.</param>
        /// <returns>True if an active (unlocked) appointment exists; otherwise, false.</returns>
        [DocInfo("Verifies if there is an open appointment for an application and test type.")]
        [StoredProcedure("SP_IsAnyActiveAppointment")]
        public static bool IsAnyActiveAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsAnyActiveAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();
                        object results = command.ExecuteScalar();


                        if (results != null)
                        {
                            Isfound = true;
                        }
                        else
                        {
                            return false;
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
        /// Updates the date of an existing, unlocked test appointment.
        /// </summary>
        /// <param name="TestAppointmentID">The ID of the test appointment to reschedule.</param>
        /// <param name="NewAppointmentDate">The new appointment date and time.</param>
        /// <returns>True if the appointment was successfully updated; otherwise, false.</returns>
        [DocInfo("Reschedules an unlocked test appointment.")]
        [StoredProcedure("SP_UpdateTestAppointment")]
        public static bool UpdateTestAppointment(int TestAppointmentID, DateTime NewAppointmentDate)
        {
            int rows = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateTestAppointment", connection))
                {


                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@AppointmentDate", NewAppointmentDate);


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
        /// Deletes a test appointment record by ID.
        /// </summary>
        /// <param name="TestAppointmentID">The ID of the appointment to delete.</param>
        /// <returns>True if deleted successfully; otherwise, false.</returns>
        [DocInfo("Removes a test appointment from the database.")]
        [StoredProcedure("SP_DeleteTestAppointment")]
        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int rows = -1;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteTestAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
        /// Locks a test appointment after a test result is recorded.
        /// </summary>
        /// <param name="TestAppointmentID">The ID of the test appointment to lock.</param>
        /// <returns>True if locked successfully; otherwise, false.</returns>
        [DocInfo("Sets IsLocked to true for a test appointment.")]
        [StoredProcedure("SP_LockTestAppointment")]
        public static bool LockAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_LockTestAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
        /// Retrieves all test appointments using the DB view <c>TestAppointments_View</c>.
        /// </summary>
        /// <returns>A DataTable containing all test appointment records.</returns>
        [DocInfo("Retrieves a comprehensive list of test appointments from the view.")]
        [StoredProcedure("SP_GetAllTestAppointments")]
        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
          
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllTestAppointments", connection))
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
        /// Checks whether an applicant has passed a specific test type.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The local application ID.</param>
        /// <param name="TestTypeID">The test type ID.</param>
        /// <returns>True if a passing test result exists; otherwise, false.</returns>
        [DocInfo("Determines if an application has a passed test for a specific test type.")]
        [StoredProcedure("SP_DoesPassTestType")]
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Isfound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
                using (SqlCommand command = new SqlCommand("SP_DoesPassTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    connection.Open();

                    object Results = command.ExecuteScalar();

                    if (Results != null)
                    {
                        Isfound = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException(ex, "Data Access Error");
            }

            return Isfound;
        }

        /// <summary>
        /// Counts the total number of test attempts made for a given test type under an application.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The local application ID.</param>
        /// <param name="TestTypeID">The test type ID.</param>
        /// <returns>The total trial count, or -1 on error.</returns>
        [DocInfo("Calculates how many times an applicant took a specific test.")]
        [StoredProcedure("SP_GetTotalTrialsPerTestType")]
        public static int TotalTrialsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int Trailcount = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetTotalTrialsPerTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null && int.TryParse(results.ToString(), out int Count))
                        {
                            Trailcount = Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }
            }
            
            return Trailcount;

        }

        /// <summary>
        /// Retrieves key appointment details associated with a specific application and test type, ordered descending by date.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The local application ID.</param>
        /// <param name="TestTypeID">The test type ID.</param>
        /// <returns>A DataTable with appointment records containing ID, date, fees, and lock status.</returns>
        [DocInfo( "Lists appointment history per test type for a specific application.")]
        [StoredProcedure("SP_GetApplicationAppointmentsPerTestType")]
        public static DataTable GetApplicationAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                SqlCommand command = new SqlCommand("SP_GetApplicationAppointmentsPerTestType", connection);

                command.CommandType = CommandType.StoredProcedure; 

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

            return dt;
        }

        /// <summary>
        /// Counts how many appointments are scheduled for today for a given test type.
        /// </summary>
        /// <param name="testTypeID">The test type ID.</param>
        /// <returns>Number of appointments scheduled today for the test type.</returns>
        [DocInfo("Returns today's appointment count filtered by test type.")]
        [StoredProcedure("SP_GetTodayAppointmentsCountByTestType")]
        public static int GetTodayAppointmentsCountByTestType(int testTypeID)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                
                using (SqlCommand command = new SqlCommand("SP_GetTodayAppointmentsCountByTestType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@TestTypeID", testTypeID);

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
                        if(count == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Counts total test appointments scheduled across all test types for today.
        /// </summary>
        /// <returns>Total number of today's appointments.</returns>
        [DocInfo("Returns the grand total of today's test appointments.")]
        [StoredProcedure("SP_GetTotalTodayAppointmentsCount")]
        public static int GetTotalTodayAppointmentsCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using (SqlCommand command = new SqlCommand("SP_GetTotalTodayAppointmentsCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
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
                        if(count  == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return count;
        }
    }

}
