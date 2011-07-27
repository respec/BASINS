Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsCalendarYearTest and is intended
'''to contain all atcSeasonsCalendarYearTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsCalendarYearTest
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
            Select Case Date.FromOADate(lTs.Dates.Value(lInd)).Year
                Case 1999 : lTs.Value(lInd + 1) = 1
                Case 2000 : lTs.Value(lInd + 1) = 2
                Case 2001 : lTs.Value(lInd + 1) = 3
            End Select
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

    '''<summary>Test atcSeasonsCalendarYear Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsCalendarYearConstructorTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsCalendarYear))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Assert.AreEqual(target.AllSeasons().Length, 0)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Dim lClone As atcSeasonsCalendarYear = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Dim aDate As Double = Jday(1999, 5, 30, 0, 0, 0)
        Assert.AreEqual(1999, target.SeasonIndex(aDate))
        aDate = Jday(2001, 5, 30, 0, 0, 0)
        Assert.AreEqual(2001, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SetAllSeasons</summary>
    <TestMethod()> Public Sub SetAllSeasonsTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Dim aAllSeasons() As Integer = {1999, 2000, 2001}
        target.SetAllSeasons(aAllSeasons)
        Assert.AreEqual(1999, target.AllSeasons(0))
        Assert.AreEqual(2000, target.AllSeasons(1))
        Assert.AreEqual(2001, target.AllSeasons(2))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean CalendarYear 1999 1999"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean CalendarYear 2000 2000"))
        Assert.AreEqual(3.0, aCalculatedAttributes.GetValue("Mean CalendarYear 2001 2001"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean CalendarYear 1999 1999"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean CalendarYear 2000 2000"))
        Assert.AreEqual(3.0, aTS.Attributes.GetValue("Mean CalendarYear 2001 2001"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(3, actual.Count)
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(3.0, actual(2).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsCalendarYear = New atcSeasonsCalendarYear()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(1999)
        lSeasonsSelected.Add(2000)
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(1.5, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(3.0, actual(1).Attributes.GetValue("Mean"))
    End Sub
End Class
