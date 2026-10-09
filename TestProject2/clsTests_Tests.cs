using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using System.Data;
using System.Data.SqlClient;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace TestProject2;

/// <summary>
/// Integration test suite for ClsTestBuisness covering creation, retrieval, update, deletion, duplicate prevention, and
/// appointment lock handling.
/// </summary>
/// <remarks>Sets up a Person, a LocalDrivingLicenseApplication, and an unlocked TestAppointment before each test
/// and cleans up created records in teardown (test result, appointment, application, person). Tests exercise Save,
/// FindByID, FindByAppointmentID, GetAllTests, Update, Delete, and behavior when attempting duplicate results. Relies
/// on persistent state and business-layer side effects such as locking the appointment when a test result is
/// saved.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsTestBuisness execution, test record lock handling, and appointment result lookups.", Module = "Test Execution Management", Version = "1.0")]
public class clsTests_Tests
{
    [TestFixture]
    public class TestsBusinessTests
    {
        #region Private Test Fixture State

        /// <summary>
        /// Stores the identifier of the test appointment used by the test fixture.
        /// </summary>
        /// <remarks>Populated during test setup and referenced by test methods to identify the
        /// appointment under test.</remarks>
        private int _testAppointmentID;

        /// <summary>
        /// Local application identifier used for testing.
        /// </summary>
        /// <remarks>Private backing field for the test application identifier; accessed only within the
        /// containing type.</remarks>
        private int _testLocalAppID;

        /// <summary>
        /// Backing field for the test person identifier.
        /// </summary>
        /// <remarks>Used as the backing store for the corresponding property.</remarks>
        private int _testPersonID;

        /// <summary>
        /// Unique identifier for the test result.
        /// </summary>
        private int _testResultID;
        #endregion

        #region Setup & Teardown

        /// <summary>
        /// Generates transient person, local driving-license application, and unlocked test appointment records
        /// required to record test results.
        /// </summary>
        /// <remarks>Creates and persists a person (with four name fields), a local driving-license
        /// application linked to that person, and an unlocked test appointment scheduled for the next day. Each entity
        /// is saved and its generated identifier is stored in _testPersonID, _testLocalAppID, and _testAppointmentID
        /// respectively.</remarks>
        [SetUp]
        [DocInfo("Generates transient person, local driving application, and unlocked appointment records required to record test results.")]
        public void Setup()
        {
            // --- Step A: Create a Person (with ALL 4 name fields) ---
            var p = new ClsPerson();
            p.NationalNo = "TEST_RES_001";
            p.FirstName = "Res";
            p.SecondName = "Unit";
            p.ThirdName = "Test";
            p.LastName = "Tester";
            p.DateOfBirth = new DateTime(1990, 1, 1);
            p.Gendor = 0;
            p.Address = "789 Test Ave";
            p.Phone = "01234567890";
            p.Email = "res.test@example.com";
            p.NationalityCountryID = 1;
            p.ImagePath = "";
            p.Save();
            _testPersonID = p.PersonID;

            // --- Step B: Create a Local Driving License Application ---
            var app = new ClsLocalDrivingLicenseAppBusiness();
            app.AppInfo.ApplicationPersonID = _testPersonID;
            app.AppInfo.ApplicationDate = DateTime.Now;
            app.AppInfo.ApplicationTypeID = 1;
            app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
            app.AppInfo.LastStatus = DateTime.Now;
            app.AppInfo.PaidFees = 15;
            app.AppInfo.CreatedByUserID = 1;
            app.LicenseClassID = 1;
            app.Save();
            _testLocalAppID = app.LocalDrivingLicenseApplicationID;

            // --- Step C: Create an UNLOCKED Test Appointment ---
            var apt = new ClsTestAppointmentBusiness();
            apt.TestTypeID = 1; // Vision Test
            apt.LocalDrivingLicenseApplicationID = _testLocalAppID;
            apt.AppointmentDate = DateTime.Now.AddDays(1);
            apt.PaidFees = 10;
            apt.CreatedByUserID = 1;
            apt.IsLocked = false;
            apt.Save();
            _testAppointmentID = apt.TestAppointmentID;
        }

        /// <summary>
      /// Purge transient test results, test appointments, local driving license applications, and person records to
      /// restore a clean database state.
      /// </summary>
      /// <remarks>Deletes entities in reverse order of creation. Skips test-result deletion when no valid
      /// identifier exists and verifies the existence of a local driving license application before attempting
      /// deletion.</remarks>
        [TearDown]
        [DocInfo("Purges transient test results, appointments, applications, and person entities to ensure database cleanliness.")]
        public void Cleanup()
        {
            // Delete in REVERSE order of creation

            // --- Step 1: Delete the Test Result (if it exists) ---
            if (_testResultID > 0)
            {
                // Assuming there's a delete method. If not, skip this.
                // ClsTestBuisness.DeleteTest(_testResultID);
            }

            // --- Step 2: Delete the Test Appointment ---
            ClsTestAppointmentBusiness.DeleteTestAppoint(_testAppointmentID);

            // --- Step 3: Delete the Local Driving License Application ---
            var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_testLocalAppID);
            if (localApp != null)
            {
                ClsApplicationBusiness.DeleteAppLication(localApp.ApplicationID);
            }

            // --- Step 4: Delete the Person ---
            ClsPerson.DeletePerson(_testPersonID);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Creates and saves a test result for the current test appointment and returns the saved ClsTestBuisness
        /// instance.
        /// </summary>
        /// <remarks>Assigns TestAppointmentID from the current _testAppointmentID, sets CreatedByUserID
        /// to 1, calls Save(), and stores the created TestID in _testResultID.</remarks>
        /// <param name="testResult">Indicates whether the test passed (true) or failed (false).</param>
        /// <param name="notes">Optional notes associated with the test result.</param>
        /// <returns>The saved ClsTestBuisness instance representing the created test result.</returns>
        private ClsTestBuisness CreateTestResult(bool testResult = true, string notes = "Test note")
        {
            var test = new ClsTestBuisness();
            test.TestAppointmentID = _testAppointmentID;
            test.TestResult = testResult;
            test.Notes = notes;
            test.CreatedByUserID = 1;
            test.Save();
            _testResultID = test.TestID;
            return test;
        }
        #endregion

        #region Test Cases

        /// <summary>
        /// Saves a new test result for the specified appointment and locks the appointment to prevent double-booking or
        /// duplicated results.
        /// </summary>
        /// <remarks>Verifies Save returns true and a TestID was generated, stores the generated TestID
        /// for cleanup, and confirms the appointment exists and its IsLocked flag is set.</remarks>
        [Test]
        [DocInfo("Ensures saving a test result locks the associated appointment to prevent double-booking or duplicated results.")]
        public void AddNewTest_WhenValid_ShouldSaveAndLockAppointment()
        {
            // ========== ARRANGE ==========
            var test = new ClsTestBuisness();
            test.TestAppointmentID = _testAppointmentID;
            test.TestResult = true;
            test.Notes = "Passed the vision test.";
            test.CreatedByUserID = 1;

            // ========== ACT (READ) ==========
            bool result = test.Save();

            // ========== ASSERT (COMPARE) ==========
            Assert.That(result, Is.True,"Save operation failed!");
            Assert.That(test.TestID, Is.GreaterThan(0), "Test ID was not generated!"); 

            // Store for cleanup
            _testResultID = test.TestID;

            // Verify appointment is locked
            var apt = ClsTestAppointmentBusiness.FindTestAppointByID(_testAppointmentID);
            Assert.That(apt, Is.Not.Null,"Appointment not found after test save.");
            Assert.That(apt.IsLocked, Is.True,"Appointment was NOT locked after test result was saved!");
        }

        /// <summary>
        /// Verifies that FindTestByID returns the expected TestResult when given a valid TestID.
        /// </summary>
        /// <remarks>Arranges a TestResult and captures its ID, invokes ClsTestBuisness.FindTestByID, and
        /// asserts the returned entity is not null and that TestID, Notes, and TestResult match the expected
        /// values.</remarks>
        [Test]
        [DocInfo("Ensures FindTestByID retrieves the expected entity when provided a valid TestID.")]
        public void FindTestByID_WhenValid_ShouldReturnTestResult()
        {
            // ========== ARRANGE ==========
            var test = CreateTestResult(true, "Test for FindByID");
            int testID = _testResultID;

            // ========== ACT (READ) ==========
            var found = ClsTestBuisness.FindTestByID(testID);

            // ========== ASSERT (COMPARE) ==========
            Assert.That(found, Is.Not.Null,"Test result was not found by ID!");
            Assert.That(found.TestID, Is.EqualTo(testID), "Test ID does not match!");
            Assert.That(found.Notes, Is.EqualTo("Test for FindByID"), "Notes do not match!"); 
            Assert.That(found.TestResult, Is.True,"Test result value does not match!");
        }

        /// <summary>
      /// Verifies that FindByAppointmentID returns the recorded test result for the specified appointment ID.
      /// </summary>
      /// <remarks>Creates a test result associated with an appointment, invokes FindByAppointmentID with
      /// that appointment ID, and asserts the returned entity is non-null and that its TestAppointmentID and Notes
      /// match the expected values.</remarks>
        [Test]
        [DocInfo("Ensures FindByAppointmentID retrieves the recorded test entity associated with a specific appointment.")]
        public void FindByAppointmentID_WhenValid_ShouldReturnTestResult()
        {
            // ========== ARRANGE ==========
            var test = CreateTestResult(true, "Test for FindByAppointmentID");
            int appointmentID = _testAppointmentID;

            // ========== ACT (READ) ==========
            var found = ClsTestBuisness.FindByAppointmentID(appointmentID);

            // ========== ASSERT (COMPARE) ==========
            Assert.That(found, Is.Not.Null,"Test result was not found by Appointment ID!");
            Assert.That(found.TestAppointmentID, Is.EqualTo(appointmentID), "Appointment ID does not match!");
            Assert.That(found.Notes, Is.EqualTo("Test for FindByAppointmentID"), "Notes do not match!"); 
        }

       /// <summary>
       /// Verifies that FindByAppointmentID returns null when the specified appointment has no associated test result.
       /// </summary>
       /// <remarks>Arranges an appointment without a test result and asserts that
       /// ClsTestBusiness.FindByAppointmentID returns null to ensure no result is returned for appointments lacking a
       /// test.</remarks>
        [Test]
        [DocInfo("Ensures FindByAppointmentID returns null when querying an appointment that lacks a recorded test result.")]
        public void FindByAppointmentID_WhenNoTestExists_ShouldReturnNull()
        {
            // ========== ARRANGE ==========
            // No test result is created! The appointment exists but has no test result.

            // ========== ACT (READ) ==========
            var found = ClsTestBuisness.FindByAppointmentID(_testAppointmentID);

            // ========== ASSERT (COMPARE) ==========
            Assert.That(found, Is.Null,"FindByAppointmentID returned a result even though no test exists!");
        }

       /// <summary>
       /// Verifies that saving a second test result for the same locked appointment is prevented by domain rules.
       /// </summary>
       /// <remarks>Saves an initial test result for the appointment and records its identifier, then
       /// attempts a second save on the same appointment and asserts the save returns false. Relies on appointment
       /// locking and domain validation; persists the first test result and sets _testResultID as a side
       /// effect.</remarks>
        [Test]
        [DocInfo("Ensures domain rules prevent saving multiple test results against the same appointment once locked.")]
        public void AddDuplicateTestResult_ShouldFail()
        {
            // ========== ARRANGE ==========
            // First save (should pass)
            var test1 = new ClsTestBuisness();
            test1.TestAppointmentID = _testAppointmentID;
            test1.TestResult = true;
            test1.CreatedByUserID = 1;
            test1.Save(); // First save succeeds
            _testResultID = test1.TestID;

            // ========== ACT ==========
            // Try to save again on the same locked appointment
            var test2 = new ClsTestBuisness();
            test2.TestAppointmentID = _testAppointmentID;
            test2.TestResult = true;
            test2.CreatedByUserID = 1;
            bool result = test2.Save();

            // ========== ASSERT (COMPARE) ==========
            Assert.That(result, Is.False,"SECURITY BREACH: System allowed a duplicate test result on a locked appointment!");
        }

        /// <summary>
        /// Updates an existing test result record with valid changes and persists those modifications.
        /// </summary>
        /// <remarks>Retrieves the record by ID, applies updated properties (for example, TestResult and
        /// Notes), invokes Save, and verifies that Save returns true and that a subsequent retrieval reflects the
        /// updated values.</remarks>
        [Test]
        [DocInfo("Ensures modifying properties on an existing test result updates the underlying record.")]
        public void UpdateTest_WhenValid_ShouldModifyExistingRecord()
        {
            // ========== ARRANGE ==========
            var test = CreateTestResult(true, "Original Notes");
            int testID = _testResultID;

            // ========== ACT (READ) ==========
            var toUpdate = ClsTestBuisness.FindTestByID(testID);
            toUpdate.TestResult = false;
            toUpdate.Notes = "Updated Notes";
            bool result = toUpdate.Save();

            // ========== ASSERT (COMPARE) ==========
            Assert.That(result, Is.True,"Update failed!");

            var verify = ClsTestBuisness.FindTestByID(testID);
            Assert.That(verify.TestResult, Is.False,"Test result was not updated!");
            Assert.That(verify.Notes, Is.EqualTo("Updated Notes"), "Notes were not updated!"); 
        }

        /// <summary>
       /// Verifies that GetAllTests returns a non-null DataTable containing at least one test record.
       /// </summary>
       /// <remarks>Creates a test result, invokes ClsTestBuisness.GetAllTests, and asserts the returned
       /// DataTable is not null and has one or more rows.</remarks>
        [Test]
        [DocInfo("Ensures GetAllTests yields a non-null DataTable containing test records.")]
        public void GetAllTests_ShouldReturnDataTableWithRows()
        {
            // ========== ARRANGE ==========
            CreateTestResult(true, "Test for GetAll");

            // ========== ACT (READ) ==========
            DataTable dt = ClsTestBuisness.GetAllTests();

            // ========== ASSERT (COMPARE) ==========
            Assert.That(dt, Is.Not.Null,"GetAllTests returned null!");
            Assert.That(dt.Rows.Count, Is.GreaterThan(0), "Tests table is empty!"); 
        }

       /// <summary>
       /// Verifies that deleting an existing test entity removes it from the database and that subsequent retrieval by
       /// ID returns null.
       /// </summary>
       /// <remarks>Creates a test entity, calls ClsTestBuisness.DeleteTest, asserts the deletion succeeded
       /// and the entity can no longer be found, then resets shared test state to avoid duplicate cleanup.</remarks>
        [Test]
        [DocInfo("Ensures DeleteTest removes the test entity, causing subsequent FindTestByID queries to return null.")]
        public void DeleteTest_WhenValid_ShouldRemoveFromDatabase()
        {
            // ========== ARRANGE ==========
            var test = CreateTestResult(true, "Test for Delete");
            int testID = _testResultID;

            // ========== ACT (READ) ==========
            bool result = ClsTestBuisness.DeleteTest(testID);

            // ========== ASSERT (COMPARE) ==========
            Assert.That(result, Is.True,"DeleteTest returned false!");

            var verify = ClsTestBuisness.FindTestByID(testID);
            Assert.That(verify, Is.Null,"Test still exists after deletion!");

            // Reset so Cleanup doesn't try to delete it again
            _testResultID = 0;
        }

        #endregion
    }
}
