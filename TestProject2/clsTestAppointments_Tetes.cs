using DVLD_Business;
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

[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsTestAppointmentBusiness lifecycle, schedule queries, and record lookup.", Module = "Test Appointments Management", Version = "1.0")]
public class clsTestAppointments_Tetes
{
    #region Private Test Fixture State

    /// <summary>
    /// Identifier for the test application used by the containing test fixture.
    /// </summary>
    /// <remarks>Used only to track test-specific application state within the fixture.</remarks>
    private int _testAppID;

    /// <summary>
    /// Identifier for a test person.
    /// </summary>
    /// <remarks>Private backing field used to store the person's identifier for testing purposes.</remarks>
    private int _testPersonID;
    #endregion

    #region Setup & Teardown

    /// <summary>
    /// Creates transient person and local driving license application entities required to host test appointments.
    /// </summary>
    /// <remarks>Creates a temporary ClsPerson with sample data and saves it; creates a
    /// ClsLocalDrivingLicenseAppBusiness, associates the person, sets ApplicationTypeID=1, AppStatus=New,
    /// CreatedByUserID=1 and LicenseClassID=1, saves the application, and stores its ID in _testAppID.</remarks>
    [SetUp]
    [DocInfo("Generates transient person and local driving license application entities required to host test appointments.")]
    public void Setup()
    {
        // Create a Local License App first (Simplified)
        var p = new ClsPerson();
        p.NationalNo = "APPT_001";
        p.FirstName = "Appt";
        p.SecondName = "Appointment";
        p.ThirdName = "Framework";
        p.LastName = "Test";
        p.DateOfBirth = DateTime.Now;
        p.Gendor = 0;
        p.NationalityCountryID = 1;
        p.Save();

        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = p.PersonID;
        app.AppInfo.ApplicationTypeID = 1;
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1;
        app.Save();
        _testAppID = app.LocalDrivingLicenseApplicationID;
    }

    /// <summary>
    /// Purge transient test appointments, local driving license applications, and person entities created during the
    /// test to prevent database foreign key constraint conflicts.
    /// </summary>
    /// <remarks>Delete in reverse order of creation (child records first, then parents). Find and delete test
    /// appointments by local application ID, remove the local driving license application or its parent application
    /// record, and then delete the person record.</remarks>
    [TearDown]
    [DocInfo("Purges transient test appointments, driving license applications, and person entities to prevent database foreign key constraint conflicts.")]
    public void Cleanup()
    {
        // IMPORTANT: Delete in REVERSE order of creation!
        // (Child records first, then parent records)

        // --- Step 1: Delete any Appointments created during the test ---
        // We don't have a specific ID stored for the appointment created in the test,
        // but we can find and delete them by the Local App ID.
        DataTable appointments = ClsTestAppointmentBusiness.FindTestAppointByTesTypeID(1, _testAppID);
        if (appointments != null)
        {
            foreach (DataRow row in appointments.Rows)
            {
                int aptID = Convert.ToInt32(row["TestAppointmentID"]);
                ClsTestAppointmentBusiness.DeleteTestAppoint(aptID);
            }
        }

        // --- Step 2: Delete the Local Driving License Application ---
        // (Assuming you have a delete method. If not, you might need to add one.
        //  Alternatively, just leave it, but then the Person cleanup will fail due to FK.
        //  Let's assume ClsLocalDrivingLicenseAppBusiness has a Delete method)
        var localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(_testAppID);
        if (localApp != null)
        {
            // Option A: If you have a Delete method in that class:
            // ClsLocalDrivingLicenseAppBusiness.DeleteLocalDrivingLicenseApplication(_testAppID);

            // Option B: Delete the parent Application record directly (since it's a 1-to-1)
            ClsApplicationBusiness.DeleteAppLication(localApp.ApplicationID);
        }

        // --- Step 3: Delete the Person ---
        ClsPerson.DeletePerson(_testPersonID);
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Saves a valid test appointment, returns true when the record is persisted, and assigns the generated
    /// TestAppointmentID.
    /// </summary>
    /// <remarks>Arranges a ClsTestAppointmentBusiness with valid properties, calls Save(), and asserts that
    /// Save returns true and TestAppointmentID is greater than zero.</remarks>
    [Test]
    [DocInfo("Ensures Save persists a valid test appointment record and returns a generated TestAppointmentID.")]
    public void AddAppointment_WhenValid_ShouldSave()
    {
        // Arrange
        var apt = new ClsTestAppointmentBusiness();
        apt.TestTypeID = 1;
        apt.LocalDrivingLicenseApplicationID = _testAppID;
        apt.AppointmentDate = DateTime.Now.AddDays(1);
        apt.PaidFees = 10;
        apt.CreatedByUserID = 1;
        apt.IsLocked = false;

        // Act
        bool result = apt.Save();

        // Assert
        Assert.That(result,Is.True);
        Assert.That(apt.TestAppointmentID, Is.GreaterThan(0)); 
    }

    /// <summary>
    /// Verifies that FindTestAppointByID returns the expected TestAppointment for a valid TestAppointmentID.
    /// </summary>
    /// <remarks>Creates and persists a TestAppointment, invokes FindTestAppointByID with its ID, and asserts
    /// the result is non-null and has the same TestAppointmentID.</remarks>
    [Test]
    [DocInfo("Ensures FindTestAppointByID retrieves the expected entity when provided a valid TestAppointmentID.")]
    public void FindByID_WhenValid_ShouldReturn()
    {
        // Arrange
        var apt = new ClsTestAppointmentBusiness();
        apt.TestTypeID = 1;
        apt.LocalDrivingLicenseApplicationID = _testAppID;
        apt.AppointmentDate = DateTime.Now.AddDays(1);
        apt.PaidFees = 10;
        apt.CreatedByUserID = 1;
        apt.Save();
        int id = apt.TestAppointmentID;

        // Act
        var found = ClsTestAppointmentBusiness.FindTestAppointByID(id);

        // Assert
        Assert.That(found,Is.Not.Null);
        Assert.That(found.TestAppointmentID,Is.EqualTo( id));
    }

    /// <summary>
    /// Asserts that GetAllTestAppoint returns a non-null DataTable containing one or more appointment records.
    /// </summary>
    /// <remarks>Unit test that calls ClsTestAppointmentBusiness.GetAllTestAppoint and verifies the returned
    /// DataTable is populated (Rows.Count > 0).</remarks>
    [Test]
    [DocInfo("Ensures GetAllTestAppoint yields a populated DataTable containing test appointment records.")]
    public void GetAllAppointments_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsTestAppointmentBusiness.GetAllTestAppoint();

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }

    #endregion
}
