Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesGroupBuilderTest and is intended
'''to contain all atcTimeseriesGroupBuilderTest Unit Tests (Done)
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

    '''<summary>Test atcTimeseriesGroupBuilder Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGroupBuilderConstructorTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource)
        Assert.IsInstanceOfType(target, GetType(atcTimeseriesGroupBuilder))
    End Sub

    '''<summary>Test AddValues</summary>
    <TestMethod()> Public Sub AddValuesTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim lKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(lKeys)

        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim aValues() As Double = {1, 2, 3}
        target.AddValues(aDate, aValues)
        Assert.AreEqual(2, target.Builder("1").NumValues)
    End Sub

    '''<summary>Test Builder</summary>
    <TestMethod()> Public Sub BuilderTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim lKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(lKeys)

        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim aValues() As Double = {1, 2, 3}
        target.AddValues(aDate, aValues)
        Assert.AreEqual(2, target.Builder("1").NumValues)
        Assert.AreEqual(2, target.Builder("2").NumValues)
        Assert.AreEqual(2, target.Builder("3").NumValues)
    End Sub

    '''<summary>Test Count</summary>
    <TestMethod()> Public Sub CountTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Assert.AreEqual(0, target.Count)
        Dim lKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(lKeys)

        Dim aDate As DateTime = New DateTime() ' TODO: Initialize to an appropriate value
        Dim aValues() As Double = {1, 2, 3}
        target.AddValues(aDate, aValues)
        Assert.AreEqual(3, target.Count)
    End Sub

    '''<summary>Test CreateBuilders</summary>
    <TestMethod()> Public Sub CreateBuildersTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(aKeys)
        Assert.AreEqual(3, target.Count)
    End Sub

    '''<summary>Test CreateTimeseriesAddToGroup</summary>
    <TestMethod()> Public Sub CreateTimeseriesAddToGroupTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim aKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(aKeys)
        Dim aGroup As New atcTimeseriesGroup
        target.CreateTimeseriesAddToGroup(aGroup)
        Assert.AreEqual(aKeys.Length, aGroup.Count)
    End Sub

    '''<summary>Test CreateTimeseriesGroup</summary>
    <TestMethod()> Public Sub CreateTimeseriesGroupTest()
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroupBuilder = New atcTimeseriesGroupBuilder(aDataSource) ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup = target.CreateTimeseriesGroup
        Assert.AreEqual(0, actual.Count)
        Dim aKeys() As String = {"1", "2", "3"}
        target.CreateBuilders(aKeys)
        actual = target.CreateTimeseriesGroup
        Assert.AreEqual(3, actual.Count)
    End Sub
End Class
