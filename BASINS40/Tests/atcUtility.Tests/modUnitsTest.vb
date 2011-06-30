Imports System.Collections
Imports System.Xml
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for modUnitsTest and is intended
'''to contain all modUnitsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class modUnitsTest
    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region

    '''<summary>Test ExtractChildByName</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub ExtractChildByNameTest()
        Dim aParent As XmlNode = Nothing ' TODO: Initialize to an appropriate value
        Dim aTag As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttValue As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As XmlNode = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As XmlNode
        actual = modUnits_Accessor.ExtractChildByName(aParent, aTag, aAttName, aAttValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetAllUnitCategories</summary>
    <TestMethod()> Public Sub GetAllUnitCategoriesTest()
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        actual = modUnits.GetAllUnitCategories
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetAllUnitCategoriesHelper</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetAllUnitCategoriesHelperTest()
        Dim aList As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aListExpected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aXML As XmlNode = Nothing ' TODO: Initialize to an appropriate value
        modUnits_Accessor.GetAllUnitCategoriesHelper(aList, aXML)
        Assert.AreEqual(aListExpected, aList)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test GetAllUnitsInCategory</summary>
    <TestMethod()> Public Sub GetAllUnitsInCategoryTest()
        Dim Category As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        actual = modUnits.GetAllUnitsInCategory(Category)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetConversionFactor</summary>
    <TestMethod()> Public Sub GetConversionFactorTest()
        Dim fromUnits As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim toUnits As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        actual = modUnits.GetConversionFactor(fromUnits, toUnits)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetParameterUnits</summary>
    <TestMethod()> Public Sub GetParameterUnitsTest()
        Dim ParameterName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim FileType As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modUnits.GetParameterUnits(ParameterName, FileType)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetTable</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub GetTableTest()
        Dim aTableName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As XmlNode = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As XmlNode
        actual = modUnits_Accessor.GetTable(aTableName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetUnitCategory</summary>
    <TestMethod()> Public Sub GetUnitCategoryTest()
        Dim unitsName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modUnits.GetUnitCategory(unitsName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetUnitDescription</summary>
    <TestMethod()> Public Sub GetUnitDescriptionTest()
        Dim unitsName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modUnits.GetUnitDescription(unitsName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetUnitID</summary>
    <TestMethod()> Public Sub GetUnitIDTest()
        Dim unitsName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modUnits.GetUnitID(unitsName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetUnitName</summary>
    <TestMethod()> Public Sub GetUnitNameTest()
        Dim unitsID As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modUnits.GetUnitName(unitsID)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test UnitSystem</summary>
    <TestMethod()> Public Sub UnitSystemTest()
        Dim aUnitSystem As atcUnitSystem = New atcUnitSystem() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modUnits.UnitSystem(aUnitSystem)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test unitsDB</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub unitsDBTest()
        Dim expected As XmlNode = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As XmlNode
        actual = modUnits_Accessor.unitsDB
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
