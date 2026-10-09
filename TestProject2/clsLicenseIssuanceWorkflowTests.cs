using DVLD_Business;
using DVLD_BusinessWorkflows; 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace TestProject2;

/// <summary>
/// Integration tests for the license issuance workflow, validating first-time license issuance, application completion
/// status updates, handling of incomplete or cancelled applications, and prevention of duplicate licenses.
/// </summary>
/// <remarks>Setup creates a test person and a local driving license application and records passing vision,
/// written, and street tests. TearDown removes test results, appointments, the local and parent applications, and the
/// person in reverse order. Individual tests cover successful issuance, failure when required tests are not passed,
/// rejection for cancelled applications, and prevention of issuing a duplicate license for the same person and
/// class.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsLicenseIssuanceWorkflow validating initial license issuance, test completion checks, cancellation safeguards, and duplicate prevention.", Module = "License Issuance Workflow", Version = "1.0")]
public class clsLicenseIssuanceWorkflowTests
{
    #region Private Test Fixture State

    /// <summary>
    /// Identifier of the test person used by the test fixture.
    /// </summary>
    /// <remarks>Assigned during test setup and referenced by test methods.</remarks>
    private int _testPersonID;

    /// <summary>
    /// The test application identifier.
    /// </summary>
    private int _testAppID;     
    
    /// <summary>
    /// Local test application identifier.
    /// </summary>
    private int _testLocalAppID;     
    private int _visionAppointmentID;

    /// <summary>
    /// The identifier of the appointment that was written.
    /// </summary>
    /// <remarks>A value of zero indicates no appointment has been written.</remarks>
    private int _writtenAppointmentID;

    /// <summary>
    /// Identifier of the associated street appointment.
    /// </summary>
    /// <remarks>Backing field for a street appointment identifier.</remarks>
    private int _streetAppointmentID;
    #endregion

    #region Setup & Teardown

    /// <summary>
   /// Creates a test person, a local driving license application, and three passed test records (Vision, Written,
   /// Street) for use by tests.
   /// </summary>
   /// <remarks>Creates and saves a ClsPerson with all name fields populated; creates and saves a
   /// ClsLocalDrivingLicenseAppBusiness configured as a new application; schedules and saves three test appointments
   /// and corresponding ClsTestBuisness records for Vision (TestTypeID 1), Written (TestTypeID 2), and Street
   /// (TestTypeID 3). Assigned identifiers are stored in _testPersonID, _testAppID, _testLocalAppID,
   /// _visionAppointmentID, _writtenAppointmentID, and _streetAppointmentID.</remarks>
    [SetUp]
    [DocInfo("Establishes a valid applicant, local application, and 3 passed test records (Vision, Written, Street) prior to test execution.")]
    public void Setup()
    {
        // ========== Step 1: Create a Person (ALL 4 name fields!) ==========
        var p = new ClsPerson();
        p.NationalNo = "WORKFLOW_001";
        p.FirstName = "Work";
        p.SecondName = "Flow";          // FIXED: Added SecondName
        p.ThirdName = "Unit";           // FIXED: Added ThirdName
        p.LastName = "Test";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "123 License St";
        p.Phone = "01000000000";
        p.Email = "workflow@test.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        _testPersonID = p.PersonID;

        // ========== Step 2: Create a Local Driving License Application ==========
        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = _testPersonID;
        app.AppInfo.ApplicationDate = DateTime.Now;
        app.AppInfo.ApplicationTypeID = 1; // New License
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.LastStatus = DateTime.Now;
        app.AppInfo.PaidFees = 15;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1; // Assume Class 1 exists
        app.Save();

        _testAppID = app.ApplicationID;
        _testLocalAppID = app.LocalDrivingLicenseApplicationID;

        // ========== Step 3: Add & Pass ALL 3 Tests (Vision, Written, Street) ==========

        // --- Vision Test (ID 1) ---
        var apt1 = new ClsTestAppointmentBusiness();
        apt1.TestTypeID = 1;
        apt1.LocalDrivingLicenseApplicationID = _testLocalAppID;
        apt1.AppointmentDate = DateTime.Now.AddDays(-1);
        apt1.PaidFees = 10;
        apt1.CreatedByUserID = 1;
        apt1.IsLocked = false;
        apt1.Save();
        _visionAppointmentID = apt1.TestAppointmentID;

        var test1 = new ClsTestBuisness();
        test1.TestAppointmentID = _visionAppointmentID;
        test1.TestResult = true;
        test1.Notes = "Passed Vision Test";
        test1.CreatedByUserID = 1;
        test1.Save();

        // --- Written Test (ID 2) ---
        var apt2 = new ClsTestAppointmentBusiness();
        apt2.TestTypeID = 2;
        apt2.LocalDrivingLicenseApplicationID = _testLocalAppID;
        apt2.AppointmentDate = DateTime.Now.AddDays(-1);
        apt2.PaidFees = 10;
        apt2.CreatedByUserID = 1;
        apt2.IsLocked = false;
        apt2.Save();
        _writtenAppointmentID = apt2.TestAppointmentID;

        var test2 = new ClsTestBuisness();
        test2.TestAppointmentID = _writtenAppointmentID;
        test2.TestResult = true;
        test2.Notes = "Passed Written Test";
        test2.CreatedByUserID = 1;
        test2.Save();

        // --- Street Test (ID 3) ---
        var apt3 = new ClsTestAppointmentBusiness();
        apt3.TestTypeID = 3;
        apt3.LocalDrivingLicenseApplicationID = _testLocalAppID;
        apt3.AppointmentDate = DateTime.Now.AddDays(-1);
        apt3.PaidFees = 10;
        apt3.CreatedByUserID = 1;
        apt3.IsLocked = false;
        apt3.Save();
        _streetAppointmentID = apt3.TestAppointmentID;

        var test3 = new ClsTestBuisness();
        test3.TestAppointmentID = _streetAppointmentID;
        test3.TestResult = true;
        test3.Notes = "Passed Street Test";
        test3.CreatedByUserID = 1;
        test3.Save();
    }

    /// <summary>
    /// Purges generated test workflow entities in reverse dependency order to preserve database consistency.
    /// </summary>
    /// <remarks>Deletes data in reverse creation order: test results are removed via appointment deletions,
    /// then the local driving license application and its parent application, and finally the person record.
    /// Appointment deletions rely on cascade delete for test results; if cascade delete is not enabled, test results
    /// must be removed before deleting appointments.</remarks>
    [TearDown]
    [DocInfo("Purges generated test workflow entities in reverse dependency order to preserve database consistency.")]
    public void Cleanup()
    {
        // IMPORTANT: Delete in REVERSE order of creation!
        // Test Results -> Appointments -> Local App -> Parent App -> Person

        // --- Step 1: Delete the Test Results (via deleting the Appointments) ---
        // We stored the Appointment IDs. Deleting the appointment should cascade to the Test Result.
        // If you do NOT have CASCADE DELETE, you must delete the Test Results manually first.

        // Option A: Delete the Appointments (which deletes Test Results if CASCADE is set)
        if (_visionAppointmentID > 0)
            ClsTestAppointmentBusiness.DeleteTestAppoint(_visionAppointmentID);

        if (_writtenAppointmentID > 0)
            ClsTestAppointmentBusiness.DeleteTestAppoint(_writtenAppointmentID);

        if (_streetAppointmentID > 0)
            ClsTestAppointmentBusiness.DeleteTestAppoint(_streetAppointmentID);

        // --- Step 2: Delete the Local Driving License Application ---
        var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_testLocalAppID);
        if (localApp != null)
        {
            // Delete the parent Application record
            ClsApplicationBusiness.DeleteAppLication(localApp.ApplicationID);
        }

        // --- Step 3: Delete the Person ---
        ClsPerson.DeletePerson(_testPersonID);
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Creates a license for the specified local application when all prerequisite tests have passed and updates the
    /// parent application's status to Completed.
    /// </summary>
    /// <remarks>Returns the newly created license ID (>0) on success and updates the parent application
    /// status to Completed. May throw on invalid input or persistence failures.</remarks>
    [Test]
    [DocInfo("Ensures IssueLicenseForTheFirstTime creates a valid license entity and updates the parent application status to Completed when prerequisites are fully met.")]
    public void IssueLicenseForFirstTime_WhenAllTestsPassed_ShouldCreateLicense()
    {
        // ========== ACT (READ) ==========
        int licenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(
            _testLocalAppID,
            "Issued via Unit Test (All Tests Passed)",
            1 // CreatedByUserID
        );

        // ========== ASSERT (COMPARE) ==========
        Assert.That(licenseID, Is.GreaterThan(0), "FAIL: License issuance failed even though all tests passed!"); 

        // Optional: Verify the application status changed to "Completed"
        var verifyApp = ClsApplicationBusiness.FindApplicationByID(_testAppID);
        Assert.That(verifyApp.AppStatus, Is.EqualTo(ClsApplicationBusiness.enApplicationStatus.Completed),
            "FAIL: Application status was not updated to Completed!");
    }

    /// <summary>
    /// Verifies that IssueLicenseForTheFirstTime returns a non-positive identifier when required tests are not passed.
    /// </summary>
    /// <remarks>Creates a new person and application without any passed tests, invokes
    /// IssueLicenseForTheFirstTime for that application, asserts the returned license ID is less than or equal to zero,
    /// then cleans up the created records.</remarks>
    [Test]
    [DocInfo("Ensures IssueLicenseForTheFirstTime returns a negative/invalid ID when test prerequisites are incomplete.")]
    public void IssueLicense_WhenTestsNotPassed_ShouldReturnNegative()
    {
        // ========== ARRANGE ==========
        // Create a completely NEW person and app specifically for this test
        var p = new ClsPerson();
        p.NationalNo = "WORKFLOW_FAIL_001";
        p.FirstName = "Fail";
        p.SecondName = "Test";
        p.ThirdName = "Case";
        p.LastName = "User";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "555 Fail St";
        p.Phone = "01555555555";
        p.Email = "fail@test.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        int failPersonID = p.PersonID;

        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = failPersonID;
        app.AppInfo.ApplicationDate = DateTime.Now;
        app.AppInfo.ApplicationTypeID = 1;
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.LastStatus = DateTime.Now;
        app.AppInfo.PaidFees = 15;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1;
        app.Save();
        int badAppID = app.LocalDrivingLicenseApplicationID;

        // ========== ACT (READ) ==========
        // NO tests were added for this app!
        int licenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(
            badAppID,
            "Should fail (No tests passed)",
            1
        );

        // ========== ASSERT (COMPARE) ==========
        Assert.That(licenseID, Is.LessThanOrEqualTo(0), "SECURITY BREACH: License was issued WITHOUT passing all tests!"); 

        // ========== CLEANUP for this specific test ==========
        ClsApplicationBusiness.DeleteAppLication(app.ApplicationID);
        ClsPerson.DeletePerson(failPersonID);
    }

    /// <summary>
    /// Verifies that issuing a license for a cancelled application is blocked and returns a non-positive identifier.
    /// </summary>
    /// <remarks>Creates a new person and application, cancels the application, attempts first-time license
    /// issuance via ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime, asserts the returned license ID is less
    /// than or equal to zero, and cleans up created entities.</remarks>
    [Test]
    [DocInfo("Ensures IssueLicenseForTheFirstTime blocks license generation for cancelled applications.")]
    public void IssueLicense_WhenApplicationCancelled_ShouldReturnNegative()
    {
        // ========== ARRANGE ==========
        // Create a NEW person and app
        var p = new ClsPerson();
        p.NationalNo = "WORKFLOW_CANCEL_001";
        p.FirstName = "Cancel";
        p.SecondName = "Test";
        p.ThirdName = "Case";
        p.LastName = "User";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "777 Cancel St";
        p.Phone = "01777777777";
        p.Email = "cancel@test.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        int cancelPersonID = p.PersonID;

        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = cancelPersonID;
        app.AppInfo.ApplicationDate = DateTime.Now;
        app.AppInfo.ApplicationTypeID = 1;
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.LastStatus = DateTime.Now;
        app.AppInfo.PaidFees = 15;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1;
        app.Save();
        int cancelAppID = app.LocalDrivingLicenseApplicationID;

        // Cancel the application first
        bool cancelResult = app.AppInfo.Cancel();
        Assert.That(cancelResult, Is.True,"Setup failed: Could not cancel the application.");

        // ========== ACT (READ) ==========
        int licenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(
            cancelAppID,
            "Should fail (App is cancelled)",
            1
        );

        // ========== ASSERT (COMPARE) ==========
        Assert.That(licenseID, Is.LessThanOrEqualTo(0), "SECURITY BREACH: License was issued on a CANCELLED application!"); 

        // ========== CLEANUP for this specific test ==========
        ClsApplicationBusiness.DeleteAppLication(app.ApplicationID);
        ClsPerson.DeletePerson(cancelPersonID);
    }

    /// <summary>
    /// Verifies that issuing a license for an applicant who already holds a license in the same license class fails and
    /// returns a non-positive identifier.
    /// </summary>
    /// <remarks>Arranges by issuing an initial license, then attempts to issue a second license for the same
    /// person and class. Asserts the first issuance returns an id greater than zero and the second returns a value less
    /// than or equal to zero. Cleanup relies on the test teardown to remove created records.</remarks>
    [Test]
    [DocInfo("Ensures IssueLicenseForTheFirstTime prevents generating duplicate licenses for an applicant within the same license class.")]
    public void IssueLicense_WhenPersonAlreadyHasLicense_ShouldReturnNegative()
    {
        // ========== ARRANGE ==========
        // First, issue a license successfully to create the "already has" scenario.
        int firstLicenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(
            _testLocalAppID,
            "First License (To create duplicate scenario)",
            1
        );
        Assert.That(firstLicenseID, Is.GreaterThan(0), "Setup failed: Could not issue first license."); 

        // ========== ACT (READ) ==========
        // Try to issue ANOTHER license for the SAME person and SAME class
        int secondLicenseID = ClsLicenseIssuanceWorkflow.IssueLicenseForTheFirstTime(
            _testLocalAppID,
            "Second License (Should fail)",
            1
        );

        // ========== ASSERT (COMPARE) ==========
        Assert.That(secondLicenseID, Is.LessThanOrEqualTo(0),
            "SECURITY BREACH: System issued a DUPLICATE license to the same person for the same class!"); 

        // Cleanup the first license (if you have a DeleteLicense method, call it here)
        // Otherwise, we rely on the TearDown to delete the parent records.
        // Note: If you have a License table, you might need to delete the license record manually.
        // For now, we just rely on the [TearDown] to remove the parent Application and Person.
    }

    #endregion
}
