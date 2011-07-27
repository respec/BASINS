Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsWaterYearTest and is intended
'''to contain all atcSeasonsWaterYearTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsWaterYearTest
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
        Dim lStartDate As Double = Jday(1998, 10, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2001, 9, 30, 24, 0, 0)
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lDate As Date
        For lInd As Integer = 0 To lTs.numValues - 1
            lDate = Date.FromOADate(lTs.Dates.Value(lInd))
            If lDate.Month < 10 Then
                lTs.Value(lInd + 1) = lDate.Year
            Else
                lTs.Value(lInd + 1) = lDate.Year + 1
            End If
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

    '''<summary>Test atcSeasonsWaterYear Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsWaterYearConstructorTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsWaterYear))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(0, target.AllSeasons.Length)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear() ' TODO: Initialize to an appropriate value
        Dim actual As atcSeasonBase = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, actual.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(1998, 9, 30, 0, 0, 0)
        Assert.AreEqual(1998, target.SeasonIndex(aDate))
        aDate = Jday(1998, 10, 1, 0, 0, 0)
        Assert.AreEqual(1999, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SetAllSeasons</summary>
    <TestMethod()> Public Sub SetAllSeasonsTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(0, target.AllSeasons.Length)
        Dim aAllSeasons() As Integer = {1999, 2000, 2001}
        target.SetAllSeasons(aAllSeasons)
        Assert.AreEqual(3, target.AllSeasons.Length)
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1999.0, aCalculatedAttributes.GetValue("Mean WaterYear 1999 1999"))
        Assert.AreEqual(2000.0, aCalculatedAttributes.GetValue("Mean WaterYear 2000 2000"))
        Assert.AreEqual(2001.0, aCalculatedAttributes.GetValue("Mean WaterYear 2001 2001"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1999.0, aTS.Attributes.GetValue("Mean WaterYear 1999 1999"))
        Assert.AreEqual(2000.0, aTS.Attributes.GetValue("Mean WaterYear 2000 2000"))
        Assert.AreEqual(2001.0, aTS.Attributes.GetValue("Mean WaterYear 2001 2001"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(3, actual.Count)
        Assert.AreEqual(1999.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2000.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(2001.0, actual(2).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsWaterYear = New atcSeasonsWaterYear()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(1)
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(2001, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(2000, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
