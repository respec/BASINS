Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsMonthTest and is intended
'''to contain all atcSeasonsMonthTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsMonthTest
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
        Dim lEndDate As Double = Jday(2002, 12, 31, 24, 0, 0)
        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lYear As Integer = 0
        For lInd As Integer = 0 To lTs.numValues - 1
            Select Case Date.FromOADate(lTs.Dates.Value(lInd)).Month
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

    '''<summary>Test atcSeasonsMonth Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsMonthConstructorTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsMonth))
    End Sub

    '''<summary>Test AllSeasonNames</summary>
    <TestMethod()> Public Sub AllSeasonNamesTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Dim expected() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As String
        actual = target.AllSeasonNames
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Dim expected() As Integer = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}
        Dim actual() As Integer = target.AllSeasons
        Assert.AreEqual(expected.Length, actual.Length)
        Assert.AreEqual(expected(5), actual(5))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonsMonth = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(1999, 5, 30, 0, 0, 0)
        Assert.AreEqual(5, target.SeasonIndex(aDate))
        aDate = Jday(2001, 7, 31, 0, 0, 0)
        Assert.AreEqual(7, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("Jan", target.SeasonName(1))
        Assert.AreEqual("Dec", target.SeasonName(12))
    End Sub

    '''<summary>Test SeasonYearFraction</summary>
    <TestMethod()> Public Sub SeasonYearFractionTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(31.0 / JulianYear, target.SeasonYearFraction(1))
        Assert.AreEqual(28.25 / JulianYear, target.SeasonYearFraction(2))
        Assert.AreEqual(31.0 / JulianYear, target.SeasonYearFraction(12))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean Month 01 Jan"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean Month 02 Feb"))
        Assert.AreEqual(12.0, aCalculatedAttributes.GetValue("Mean Month 12 Dec"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean Month 01 Jan"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean Month 02 Feb"))
        Assert.AreEqual(12.0, aTS.Attributes.GetValue("Mean Month 12 Dec"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(12, actual.Count)
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(3.0, actual(2).Attributes.GetValue("Mean"))
        Assert.AreEqual(12.0, actual(11).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsMonth = New atcSeasonsMonth()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(2)
        lSeasonsSelected.Add(12)
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(7.232, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(6.3856, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
