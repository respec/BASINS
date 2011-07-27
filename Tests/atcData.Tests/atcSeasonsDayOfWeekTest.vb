Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsDayOfWeekTest and is intended
'''to contain all atcSeasonsDayOfWeekTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsDayOfWeekTest
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
        Dim lStartDate As Double = Jday(2011, 1, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2011, 1, 31, 24, 0, 0)
        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lYear As Integer = 0
        For lInd As Integer = 0 To lTs.numValues - 1
            Select Case Date.FromOADate(lTs.Dates.Value(lInd)).DayOfWeek + 1
                Case 1 : lTs.Value(lInd + 1) = 1
                Case 2 : lTs.Value(lInd + 1) = 2
                Case 3 : lTs.Value(lInd + 1) = 3
                Case 4 : lTs.Value(lInd + 1) = 4
                Case 5 : lTs.Value(lInd + 1) = 5
                Case 6 : lTs.Value(lInd + 1) = 6
                Case 7 : lTs.Value(lInd + 1) = 7
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

    '''<summary>Test atcSeasonsDayOfWeek Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsDayOfWeekConstructorTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsDayOfWeek))
    End Sub

    '''<summary>Test AllSeasonNames</summary>
    <TestMethod()> Public Sub AllSeasonNamesTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(7, target.AllSeasonNames.Length)
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(7, target.AllSeasons.Length)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonsDayOfWeek = target.Clone()
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        Dim lDay As Double = Jday(2011, 1, 17, 0, 0, 0)
        Assert.AreEqual(1, target.SeasonIndex(lDay))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("Sun", target.SeasonName(1))
        Assert.AreEqual(Nothing, target.SeasonName(0))
        Assert.AreEqual(Nothing, target.SeasonName(8))
    End Sub

    '''<summary>Test SeasonSelected</summary>
    <TestMethod()> Public Sub SeasonSelectedTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek() ' TODO: Initialize to an appropriate value
        target.SeasonSelected(1) = True
        Assert.AreEqual(True, target.SeasonSelected(1))
        Assert.AreEqual(False, target.SeasonSelected(2))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(7.0, aCalculatedAttributes.GetValue("Mean DayOfWeek 7 Sat"))
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean DayOfWeek 1 Sun"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean DayOfWeek 2 Mon"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(7.0, aTS.Attributes.GetValue("Mean DayOfWeek 7 Sat"))
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean DayOfWeek 1 Sun"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean DayOfWeek 2 Mon"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(7, actual.Count)
        Assert.AreEqual(7.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(1.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(2).Attributes.GetValue("Mean"))
        Assert.AreEqual(3.0, actual(3).Attributes.GetValue("Mean"))
        Assert.AreEqual(4.0, actual(4).Attributes.GetValue("Mean"))
        Assert.AreEqual(5.0, actual(5).Attributes.GetValue("Mean"))
        Assert.AreEqual(6.0, actual(6).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsDayOfWeek = New atcSeasonsDayOfWeek()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(6)  ' 6 Sat ' easy to be confused here with the other season index
        lSeasonsSelected.Add(0) ' 0 Sun ' easy to be confused here with the other season index
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(4.0, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(3.90476, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
