Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsDayOfMonthTest and is intended
'''to contain all atcSeasonsDayOfMonthTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsDayOfMonthTest
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
            Select Case Date.FromOADate(lTs.Dates.Value(lInd)).Day
                Case 1 : lTs.Value(lInd + 1) = 1
                Case 2 : lTs.Value(lInd + 1) = 2
                Case 3 : lTs.Value(lInd + 1) = 3
                Case 4 : lTs.Value(lInd + 1) = 4
                Case 5 : lTs.Value(lInd + 1) = 5
                Case 6 : lTs.Value(lInd + 1) = 6
                Case 7 : lTs.Value(lInd + 1) = 7
                Case 8 : lTs.Value(lInd + 1) = 8
                Case 9 : lTs.Value(lInd + 1) = 9
                Case 10 : lTs.Value(lInd + 1) = 10
                Case 11 : lTs.Value(lInd + 1) = 11
                Case 12 : lTs.Value(lInd + 1) = 12
                Case 13 : lTs.Value(lInd + 1) = 13
                Case 14 : lTs.Value(lInd + 1) = 14
                Case 15 : lTs.Value(lInd + 1) = 15
                Case 16 : lTs.Value(lInd + 1) = 16
                Case 17 : lTs.Value(lInd + 1) = 17
                Case 18 : lTs.Value(lInd + 1) = 18
                Case 19 : lTs.Value(lInd + 1) = 19
                Case 20 : lTs.Value(lInd + 1) = 20
                Case 21 : lTs.Value(lInd + 1) = 21
                Case 22 : lTs.Value(lInd + 1) = 22
                Case 23 : lTs.Value(lInd + 1) = 23
                Case 24 : lTs.Value(lInd + 1) = 24
                Case 25 : lTs.Value(lInd + 1) = 25
                Case 26 : lTs.Value(lInd + 1) = 26
                Case 27 : lTs.Value(lInd + 1) = 27
                Case 28 : lTs.Value(lInd + 1) = 28
                Case 29 : lTs.Value(lInd + 1) = 29
                Case 30 : lTs.Value(lInd + 1) = 30
                Case 31 : lTs.Value(lInd + 1) = 31
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

    '''<summary>Test atcSeasonsDayOfMonth Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsDayOfMonthConstructorTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsDayOfMonth))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(31, target.AllSeasons.Length)
        Assert.AreEqual(15, target.AllSeasons(14))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonsDayOfMonth = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(1999, 12, 15, 0, 0, 0)
        Assert.AreEqual(15, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("15", target.SeasonName(15))
        Assert.AreEqual(Nothing, target.SeasonName(0))
        Assert.AreEqual(Nothing, target.SeasonName(32))
    End Sub

    '''<summary>Test SeasonSelected</summary>
    <TestMethod()> Public Sub SeasonSelectedTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth() ' TODO: Initialize to an appropriate value
        target.SeasonSelected(15) = True
        target.SeasonSelected(16) = True
        Assert.AreEqual(True, target.SeasonSelected(15))
        Assert.AreEqual(False, target.SeasonSelected(17))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean DayOfMonth 01 1"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean DayOfMonth 02 2"))
        Assert.AreEqual(12.0, aCalculatedAttributes.GetValue("Mean DayOfMonth 12 12"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean DayOfMonth 01 1"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean DayOfMonth 02 2"))
        Assert.AreEqual(12.0, aTS.Attributes.GetValue("Mean DayOfMonth 12 12"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(31, actual.Count)
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(3.0, actual(2).Attributes.GetValue("Mean"))
        Assert.AreEqual(12.0, actual(11).Attributes.GetValue("Mean"))
        Assert.AreEqual(28.0, actual(27).Attributes.GetValue("Mean"))
        Assert.AreEqual(29.0, actual(28).Attributes.GetValue("Mean"))
        Assert.AreEqual(31.0, actual(30).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsDayOfMonth = New atcSeasonsDayOfMonth()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(2)  ' day 3
        lSeasonsSelected.Add(12) ' day 13
        lSeasonsSelected.Add(28) ' day 29
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(14.735849, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(15.839393, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
