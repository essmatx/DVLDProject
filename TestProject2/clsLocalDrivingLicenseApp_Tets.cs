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
/// Integration test suite for ClsLocalDrivingLicenseAppBusiness covering CRUD operations, status transitions, and
/// catalog queries.
/// </summary>
/// <remarks>Creates a temporary test person in Setup and removes it in TearDown. Exercises saving a new local
/// driving license application (verifies saved identifier), finding by application ID, retrieving all local driving
/// license applications, and cancelling an application (verifies status becomes Cancelled). Tests interact with
/// persistent storage and assume required lookup data such as license classes and application types exist.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsLocalDrivingLicenseAppBusiness CRUD operations, status state transitions, and catalog queries.", Module = "Local Driving License Applications Management", Version = "1.0")]
public class clsLocalDrivingLicenseApp_Tets
{
    #region Private Test Fixture State

    /// <summary>
    /// Identifier of the test person used by the test fixture.
    /// </summary>
    /// <remarks>Set during test setup and reset between test cases.</remarks>
    private int _testPersonID;

    /// <summary>
    /// Identifier for the test application.
    /// </summary>
    private int _testAppID;
    #endregion

    #region Setup & Teardown

    /// <summary>
   /// Generates and persists a unique applicant person entity used as fixture state for local application tests and
   /// records its identifier in the test fixture.
   /// </summary>
   /// <remarks>Creates a ClsPerson with predefined fields (NationalNo, names, DateOfBirth, Gendor,
   /// NationalityCountryID), calls Save(), and assigns the resulting PersonID to the _testPersonID field. Uses
   /// DateTime.Now for DateOfBirth, producing non-deterministic data suitable only for local tests.</remarks>
    [SetUp]
    [DocInfo("Generates a unique applicant person entity to serve as fixture state for local application tests.")]
    public void Setup()
    {
        var p = new ClsPerson();
        p.NationalNo = "LOCAL_APP_001";
        p.FirstName = "Local";
        p.SecondName = "License";
        p.ThirdName = "App";
        p.LastName = "Test";
        p.DateOfBirth = DateTime.Now;
        p.Gendor = 0;
        p.NationalityCountryID = 1;
        p.Save();
        _testPersonID = p.PersonID;
    }

    /// <summary>
    /// Deletes the transient test person from the database to maintain test execution isolation.
    /// </summary>
    /// <remarks>Executed as a teardown after each test to remove the person identified by _testPersonID.
    /// Exceptions from the deletion call propagate to the test framework.</remarks>
    [TearDown]
    [DocInfo("Purges transient person entity from the database to maintain execution isolation.")]
    public void Cleanup()
    {
        ClsPerson.DeletePerson(_testPersonID);
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Saves a new local driving license application and verifies it is persisted and retrievable by its generated ID.
    /// </summary>
    /// <remarks>Prepares application fields (ApplicationPersonID, ApplicationDate, ApplicationTypeID,
    /// AppStatus, LastStatus, PaidFees, CreatedByUserID, LicenseClassID), calls Save, and asserts Save returns true,
    /// LocalDrivingLicenseApplicationID is greater than zero, and a lookup by the saved ID returns a non-null
    /// record.</remarks>
    [Test]
    [DocInfo("Ensures Save successfully persists a new local driving license application record and verifies post-save lookup by ID.")]
    public void AddLocalDrivingLicenseApp_WhenValid_ShouldSave()
    {
        // Arrange
        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = _testPersonID;
        app.AppInfo.ApplicationDate = DateTime.Now;
        app.AppInfo.ApplicationTypeID = 1; // New License
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.LastStatus = DateTime.Now;
        app.AppInfo.PaidFees = 15;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1; // Assume Class 1 exists

        // Act
        bool result = app.Save();

        // Assert
        Assert.That(result,Is.True);
        Assert.That(app.LocalDrivingLicenseApplicationID, Is.GreaterThan(0)); 
        _testAppID = app.ApplicationID;

        var found = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(app.LocalDrivingLicenseApplicationID);
        Assert.That(found,Is.Not.Null);
    }

    /// <summary>
    /// Verifies that FindLocalDrivingLicenseApplicationByAppID returns the saved ClsLocalDrivingLicenseAppBusiness when
    /// queried with a valid ApplicationID.
    /// </summary>
    /// <remarks>Creates and saves a new local driving license application with required fields to obtain an
    /// ApplicationID, calls FindLocalDrivingLicenseApplicationByAppID with that ID, and asserts the returned instance
    /// is not null and has a matching ApplicationID.</remarks>
    [Test]
    [DocInfo("Ensures FindLocalDrivingLicenseApplicationByAppID retrieves the expected record when provided a valid base ApplicationID.")]
    public void FindByAppID_WhenValid_ShouldReturnRecord()
    {
        // Arrange
        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = _testPersonID;
        app.AppInfo.ApplicationTypeID = 1;
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1;
        app.Save();
        int appID = app.ApplicationID;

        // Act
        var found = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByAppID(appID);

        // Assert
        Assert.That(found,Is.Not.Null);
        Assert.That(found.ApplicationID, Is.EqualTo(appID)); 
    }

    /// <summary>
    /// Verifies GetAllLocalDrivingLicenseApplications returns a non-null, populated DataTable of local driving license
    /// application records.
    /// </summary>
    /// <remarks>Asserts the returned DataTable is not null and contains at least one row.</remarks>
    [Test]
    [DocInfo("Verifies GetAllLocalDrivingLicenseApplications yields a populated DataTable containing local driving license application records.")]
    public void GetAllLocalDrivingLicenses_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsLocalDrivingLicenseAppBusiness.GetAllLocalDrivingLicenseApplications();

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }

    /// <summary>
    /// Verifies that calling AppInfo.Cancel transitions the application's status to Cancelled and persists the change.
    /// </summary>
    /// <remarks>Arranges a local driving license application with New status, saves it, invokes Cancel, and
    /// asserts the returned result and the persisted AppStatus.</remarks>
    [Test]
    [DocInfo("Ensures invoking Cancel on AppInfo transitions the application status to Cancelled and updates database state.")]
    public void CancelApplication_ShouldChangeStatus()
    {
        // Arrange
        var app = new ClsLocalDrivingLicenseAppBusiness();
        app.AppInfo.ApplicationPersonID = _testPersonID;
        app.AppInfo.ApplicationTypeID = 1;
        app.AppInfo.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
        app.AppInfo.CreatedByUserID = 1;
        app.LicenseClassID = 1;
        app.Save();

        // Act
        bool result = app.AppInfo.Cancel();

        // Assert
        Assert.That(result,Is.True);
        var verify = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByAppID(app.ApplicationID);
        Assert.That(verify.AppInfo.AppStatus,Is.EqualTo( ClsApplicationBusiness.enApplicationStatus.Cancelled));
    }

    #endregion
}
