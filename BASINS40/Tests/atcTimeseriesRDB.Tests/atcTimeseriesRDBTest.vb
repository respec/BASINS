Imports System.IO
Imports atcData
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcTimeseriesRDB

'''<summary>
'''This is a test class for atcTimeseriesRDBTest and is intended
'''to contain all atcTimeseriesRDBTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesRDBTest
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
            testContextInstance = value
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

    '''<summary>Test atcTimeseriesRDB Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesRDBConstructorTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test Open</summary>
    <TestMethod()> Public Sub OpenTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.Open(aFileName, aAttributes)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ProcessDailyValues</summary>
    <TestMethod()> Public Sub ProcessDailyValuesTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim aInputReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.ProcessDailyValues(aInputReader, aAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ProcessIdaValues</summary>
    <TestMethod()> Public Sub ProcessIdaValuesTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim aInputReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.ProcessIdaValues(aInputReader, aAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ProcessMeasurements</summary>
    <TestMethod()> Public Sub ProcessMeasurementsTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim aInputReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.ProcessMeasurements(aInputReader, aAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ProcessWaterQualityValues</summary>
    <TestMethod()> Public Sub ProcessWaterQualityValuesTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim aInputReader As BinaryReader = Nothing ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        target.ProcessWaterQualityValues(aInputReader, aAttributes)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CanOpen</summary>
    <TestMethod()> Public Sub CanOpenTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.CanOpen
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Category</summary>
    <TestMethod()> Public Sub CategoryTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Category
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Description</summary>
    <TestMethod()> Public Sub DescriptionTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Description
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Name</summary>
    <TestMethod()> Public Sub NameTest()
        Dim target As atcTimeseriesRDB = New atcTimeseriesRDB() ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.Name
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
