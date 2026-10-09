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
/// Integration test suite for ClsLicensClassBusiness covering CRUD operations, fee adjustments, and catalog queries.
/// </summary>
/// <remarks>Runs integration tests against a database using ClsLicensClassBusiness. Tests perform inserts,
/// updates, reads, and deletions and include rollback/cleanup; run against an isolated test database to avoid affecting
/// production data.</remarks>
[DocInfo("Integration test suite for ClsLicensClassBusiness CRUD operations, class fee adjustments, and catalog queries.", Module = "License Classes Management", Version = "1.0")]
[TestFixture]
public class clsLicenseClasses
{
    #region Test Cases

    /// <summary>
    /// Verifies FindLicenseClassByID returns the license class entity for a valid identifier and that the returned
    /// entity's LicenseClassID matches the requested ID.
    /// </summary>
    /// <remarks>Invokes FindLicenseClassByID with ID 1 and asserts the result is not null and that
    /// LicenseClassID equals 1.</remarks>
    [Test]
    [DocInfo("Ensures FindLicenseClassByID returns the expected license class entity for a valid ID.")]
    public void FindLicenseClassByID_WhenValid_ShouldReturnClass()
    {
        // Act
        var result = ClsLicensClassBusiness.FindLicenseClassByID(1);

        // Assert
        Assert.That(result,Is.Not.Null);
        Assert.That(result.LicenseClassID,Is.EqualTo( 1));
    }

    /// <summary>
    /// Returns the license class that matches the specified class title.
    /// </summary>
    /// <remarks>Performs a lookup by class title and returns the corresponding record when a matching title
    /// exists; otherwise returns null.</remarks>
    [Test]
    [DocInfo("Ensures FindLicenseClassByClassName returns the corresponding record when provided a valid class title string.")]
    public void FindLicenseClassByClassName_WhenValid_ShouldReturnClass()
    {
        // Act
        var result = ClsLicensClassBusiness.FindLicenseClassByClassName("Class 2 - Heavy Motorcycle License");

        // Assert
        Assert.That(result,Is.Not.Null);
        Assert.That(result.ClassName,Is.EqualTo( "Class 2 - Heavy Motorcycle License"));
    }

    /// <summary>
    /// Verifies that updating the ClassFees of an existing license class is persisted to the database and that the
    /// original fee value is restored after the test.
    /// </summary>
    /// <remarks>Uses license class with ID 1, increases the fee by 5, asserts that Save returns true and the
    /// persisted ClassFees equals the new value, then restores the original fee. Modifies persistent state; run in an
    /// isolated or transactional test environment to avoid side effects.</remarks>
    [Test]
    [DocInfo("Ensures updated fee amounts are correctly saved to the database for an existing license class and restored after testing.")]
    public void UpdateLicenseClassFees_ShouldModifyRecord()
    {
        // Arrange
        var cls = ClsLicensClassBusiness.FindLicenseClassByID(1);
        decimal originalFees = cls.ClassFees;
        decimal newFees = originalFees + 5;

        // Act
        cls.ClassFees = newFees;
        bool result = cls.Save();

        // Assert
        Assert.That(result,Is.True);
        var verify = ClsLicensClassBusiness.FindLicenseClassByID(1);
        Assert.That(verify.ClassFees, Is.EqualTo(newFees));

        // Rollback
        verify.ClassFees = originalFees;
        verify.Save();
    }

    /// <summary>
    /// Creates a license class with a unique name, inserts it into the database, updates its fees, verifies the
    /// persisted changes, and removes the test record.
    /// </summary>
    /// <remarks>Appends a GUID-based suffix to avoid duplicate-name constraints, performs insert and update
    /// operations with assertions, validates the updated record by reloading it, and ensures deletion in a finally
    /// block to maintain database integrity.</remarks>
    [Test]
    [DocInfo("Ensures dynamic insertion, subsequent fee updates, and database deletion of a new license class entity.")]
    public void AddNewLicenseClassFees_ShouldModifyRecord()
    {
        ClsLicensClassBusiness licenseClass = new ClsLicensClassBusiness();

        // Append a unique suffix to avoid "Duplicate Class Name" constraints
        string uniqueClassName = "Test Class " + Guid.NewGuid().ToString().Substring(0, 5);

        licenseClass.ClassName = uniqueClassName;
        licenseClass.ClassDescription = "Temporary integration test description.";
        licenseClass.MiniMumAllowedAge = 18;
        licenseClass.DefaultValidityLingth = 10;
        licenseClass.ClassFees = 150.00m;

        try
        {
            // 1. ACT & ASSERT: Save (Insert) to the database
            bool isInserted = licenseClass.Save();
            Assert.That(isInserted, Is.True, "Initial database insert failed.");
            Assert.That(licenseClass.LicenseClassID, Is.Not.EqualTo(-1), "LicenseClassID was not updated from -1 after saving.");

            // 2. ACT & ASSERT: Modify the fees and Save again (Update)
            decimal updatedFees = 275.50m;
            licenseClass.ClassFees = updatedFees;
            bool isUpdated = licenseClass.Save();
            Assert.That(isUpdated, Is.True, "The update database command failed.");

            // 3. ASSERT: Deep Validation
            ClsLicensClassBusiness fetchedClass = ClsLicensClassBusiness.FindLicenseClassByID(licenseClass.LicenseClassID);
            Assert.That(fetchedClass, Is.Not.Null, "Failed to retrieve the updated record from the database.");
            Assert.That(fetchedClass.ClassFees, Is.EqualTo(updatedFees), "The retrieved fees do not match the updated value.");
            Assert.That(fetchedClass.ClassName, Is.EqualTo(uniqueClassName), "Class name was unexpectedly altered during the update.");
        }
        finally
        {
            // ==========================================
            // 🧹 CLEANUP: Wipe the record from the DB!
            // ==========================================
            if (licenseClass.LicenseClassID != -1)
            {
                bool isDeleted = ClsLicensClassBusiness.DeleteLicenseClass(licenseClass.LicenseClassID);

                // Optional: Assert that the deletion was successful to ensure database integrity
                Assert.That(isDeleted, Is.True, "Failed to clean up and delete the test license class from the database.");
            }
        }
    }

    /// <summary>
    /// Verifies that GetAallLicenseClass returns a non-null, non-empty DataTable of license class catalog records.
    /// </summary>
    /// <remarks>Calls ClsLicensClassBusiness.GetAallLicenseClass and asserts the DataTable is not null and
    /// contains at least one DataRow.</remarks>
    [DocInfo("Verifies GetAallLicenseClass yields a populated DataTable containing license class catalog records.")]
    [Test]
    public void GetAllLicenseClasses_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsLicensClassBusiness.GetAallLicenseClass();

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }

    #endregion
}
