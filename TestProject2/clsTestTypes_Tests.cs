using DVLD_Business;
using DVLD_DataAccess;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;
using static DVLD_Business.ClsTestTypeBusiness;

namespace TestProject2;

/// <summary>
/// Integration tests for ClsTestTypeBusiness that validate retrieval of test type metadata, updating of test type fees,
/// and retrieval of all test types.
/// </summary>
/// <remarks>Tests interact with the persistent data store and assume a TestType record for enTestType.Vision
/// exists. Tests modify test type fees and capture/restore original fees in the teardown; run these tests against an
/// isolated test database to avoid impacting production data.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsTestTypeBusiness metadata retrieval and fee update operations.", Module = "Test Types Management", Version = "1.0")]
public class clsTestTypes_Tests
{
    #region Private Test Fixture State

    /// <summary>
    /// Original fees value used by the test fixture.
    /// </summary>
    /// <remarks>Captured during test setup and used to restore pre-test state during teardown.</remarks>
    private decimal _originalFees;

    /// <summary>
    /// Title identifying the unit test type.
    /// </summary>
    private const string TestTitle = "UNIT_TEST_TYPE";
    #endregion

    #region Teardown

    /// <summary>
/// Restore original Vision test type fees to preserve database state across test runs.
/// </summary>
/// <remarks>If a Vision test type exists, retrieve its TestTypeFees and assign them to the _originalFees
/// field.</remarks>
    [TearDown]
    [DocInfo("Restores original Vision test type fees to maintain database state integrity across test runs.")]
    public void Cleanup()
    {
        var existing = ClsTestTypeBusiness.FindTestTypeByID(ClsTestTypeBusiness.enTestType.Vision);
        if (existing != null)
        {
            _originalFees = existing.TestTypeFees;
        }
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Verifies that FindTestTypeByID returns the expected test type for a valid enum ID.
    /// </summary>
    /// <remarks>Assumes the database contains an entry for ClsTestTypeBusiness.enTestType.Vision. Asserts
    /// that the returned TestTypeID equals the requested enum value and fails if the result is null.</remarks>
    [Test]
    [DocInfo("Ensures FindTestTypeByID retrieves the correct test type record matching the requested enum ID.")]
    public void FindTestTypeByID_WhenValid_ShouldReturnType()
    {
        // Arrange: Assuming ID 1 exists in your DB.
        // Act
        var result = ClsTestTypeBusiness.FindTestTypeByID(ClsTestTypeBusiness.enTestType.Vision);

        // Assert
        if (result != null)
        {
            Assert.That(result.TestTypeID, Is.EqualTo(ClsTestTypeBusiness.enTestType.Vision), $"I expected 1, but the database returned {result.TestTypeID}");
        }
        else
        {
            Assert.Fail("The result was null. The database likely didn't find the record.");
        }
    }

    /// <summary>
    /// Updates the Vision test type fee, saves the change, and verifies the updated amount is persisted to the
    /// database.
    /// </summary>
    /// <remarks>Locates the test type via ClsTestTypeBusiness.FindTestTypeByID, increments TestTypeFees by 5,
    /// calls Save and asserts it returns true, then reloads the record and asserts the fee matches the expected value.
    /// Writes expected and actual values to TestContext.Out for diagnostics.</remarks>
    [Test]
    [DocInfo("Ensures Save updates test type fee amounts and persists the change to the database.")]
    public void UpdateTestTypeFees_ShouldModifyRecord()
    {

        var test = ClsTestTypeBusiness.FindTestTypeByID(enTestType.Vision);
        decimal newFees = test.TestTypeFees + 5;

        // Act
        test.TestTypeFees = newFees;
        bool result = test.Save();

        // Assert
        Assert.That(result, Is.True,"Result was not save");

        var verify = ClsTestTypeBusiness.FindTestTypeByID(ClsTestTypeBusiness.enTestType.Vision);

        // DEBUG: This writes to the Test Explorer "Output" window
        TestContext.Out.WriteLine($"Expected: {newFees}");
        TestContext.Out.WriteLine($"Actual from DB: {verify.TestTypeFees}");

        Assert.That(verify.TestTypeFees, Is.EqualTo(newFees));
    }

    /// <summary>
    /// Verifies that ClsTestTypeBusiness.GetAllTestTypess returns a non-null, populated DataTable containing all
    /// configured test types.
    /// </summary>
    /// <remarks>Asserts the returned DataTable is not null and contains at least one row; requires configured
    /// test types in the test environment.</remarks>
    [Test]
    [DocInfo("Ensures GetAllTestTypess returns a populated DataTable containing all configured test types.")]
    public void GetAllTestTypes_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsTestTypeBusiness.GetAllTestTypess();
        
        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }
    #endregion
}
