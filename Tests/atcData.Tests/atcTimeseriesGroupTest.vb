Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcTimeseriesGroupTest and is intended
'''to contain all atcTimeseriesGroupTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcTimeseriesGroupTest


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
    '''A test for atcTimeseriesGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesGroupConstructorTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aTimeseries)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for atcTimeseriesGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesGroupConstructorTest1()
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aDataGroup)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for atcTimeseriesGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesGroupConstructorTest2()
        Dim aTimeseries() As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup(aTimeseries)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for atcTimeseriesGroup Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcTimeseriesGroupConstructorTest3()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Clone
    '''</summary>
    <TestMethod()> _
    Public Sub CloneTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.Clone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindData
    '''</summary>
    <TestMethod()> _
    Public Sub FindDataTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValue As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.FindData(aAttributeName, aValue, aLimit)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindData
    '''</summary>
    <TestMethod()> _
    Public Sub FindDataTest1()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aValues As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aLimit As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.FindData(aAttributeName, aValues, aLimit)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcTimeseriesGroup_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Item
    '''</summary>
    <TestMethod()> _
    Public Sub ItemTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        target(aIndex) = expected
        actual = target(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ItemByIndex
    '''</summary>
    <TestMethod()> _
    Public Sub ItemByIndexTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        target.ItemByIndex(aIndex) = expected
        actual = target.ItemByIndex(aIndex)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ItemByKey
    '''</summary>
    <TestMethod()> _
    Public Sub ItemByKeyTest()
        Dim target As atcTimeseriesGroup = New atcTimeseriesGroup() ' TODO: Initialize to an appropriate value
        Dim aKey As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        target.ItemByKey(aKey) = expected
        actual = target.ItemByKey(aKey)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
