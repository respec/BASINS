Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcTimeseriesGroupBuilderTest and is intended
'''to contain all atcTimeseriesGroupBuilderTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesGroupBuilderTest


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


    '''<summary>
    '''A test for atcTimeseriesGroupBuilder Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesGroupBuilderConstructorTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddValues
    '''</summary>
    <TestMethod()> _
    Public Sub AddValuesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim aValues() As Double = Nothing ' TODO: Initialize to an appropriate value
        target.AddValues(aDate, aValues)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Builder
    '''</summary>
    <TestMethod()> _
    Public Sub BuilderTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aDataSetKey As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesBuilder = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesBuilder
        actual = target.Builder(aDataSetKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Count
    '''</summary>
    <TestMethod()> _
    Public Sub CountTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.Count
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CreateBuilders
    '''</summary>
    <TestMethod()> _
    Public Sub CreateBuildersTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aKeys() As String = Nothing ' TODO: Initialize to an appropriate value
        target.CreateBuilders(aKeys)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CreateTimeseriesAddToGroup
    '''</summary>
    <TestMethod()> _
    Public Sub CreateTimeseriesAddToGroupTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        target.CreateTimeseriesAddToGroup(aGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CreateTimeseriesGroup
    '''</summary>
    <TestMethod()> _
    Public Sub CreateTimeseriesGroupTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.CreateTimeseriesGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
