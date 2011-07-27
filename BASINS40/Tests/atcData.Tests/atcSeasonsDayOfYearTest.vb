Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsDayOfYearTest and is intended
'''to contain all atcSeasonsDayOfYearTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsDayOfYearTest
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

    Private Function CreateTestTimeseries() As atcTimeseries
        Dim lStartDate As Double = Jday(1999, 1, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2001, 12, 31, 24, 0, 0)
        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lYear As Integer = 0
        For lInd As Integer = 0 To lTs.numValues - 1
            lTs.Value(lInd + 1) = Date.FromOADate(lTs.Dates.Value(lInd)).DayOfYear
        Next
        Return lTs
    End Function

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    <ClassInitialize()> _
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        Dim lHelper As New atcTimeseriesStatistics.atcTimeseriesStatistics
        lHelper.Initialize()
    End Sub
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

    '''<summary>Test atcSeasonsDayOfYear Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsDayOfYearConstructorTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsDayOfYear))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(366, target.AllSeasons.Length)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonsDayOfYear = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(2000, 2, 29, 0, 0, 0)
        Assert.AreEqual(60, target.SeasonIndex(aDate))
        aDate = Jday(2001, 3, 1, 0, 0, 0)
        Assert.AreEqual(60, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("60", target.SeasonName(60))
        Assert.AreEqual(Nothing, target.SeasonName(0))
        Assert.AreEqual(Nothing, target.SeasonName(367))
    End Sub

    '''<summary>Test SeasonSelected</summary>
    <TestMethod()> Public Sub SeasonSelectedTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear() ' TODO: Initialize to an appropriate value
        target.SeasonSelected(1) = True

        Assert.AreEqual(True, target.SeasonSelected(1))
        Assert.AreEqual(False, target.SeasonSelected(2))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean DayOfYear 001 1"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean DayOfYear 002 2"))
        Assert.AreEqual(12.0, aCalculatedAttributes.GetValue("Mean DayOfYear 012 12"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean DayOfYear 001 1"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean DayOfYear 002 2"))
        Assert.AreEqual(12.0, aTS.Attributes.GetValue("Mean DayOfYear 012 12"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(366, actual.Count)
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(28.0, actual(27).Attributes.GetValue("Mean"))
        Assert.AreEqual(29.0, actual(28).Attributes.GetValue("Mean"))
        Assert.AreEqual(31.0, actual(30).Attributes.GetValue("Mean"))
        Assert.AreEqual(365.0, actual(364).Attributes.GetValue("Mean"))
        Assert.AreEqual(366.0, actual(365).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsDayOfYear = New atcSeasonsDayOfYear()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(59)  ' day 29 of Feb
        'lSeasonsSelected.Add(12) ' day 13
        'lSeasonsSelected.Add(28) ' day 29
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(60.0, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(183.505, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
