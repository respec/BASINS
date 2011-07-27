Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsHourTest and is intended
'''to contain all atcSeasonsHourTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsHourTest
    Private testContextInstance As TestContext

    Public Function CreateTestTimeseries() As atcTimeseries
        Dim lStartDate As Double = Jday(2010, 10, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2010, 10, 31, 0, 0, 0)
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcTimeUnit.TUHour, 1)
        For lInd As Integer = 0 To lTs.numValues - 1
            lTs.Value(lInd + 1) = Date.FromOADate(lTs.Dates.Value(lInd)).Hour
        Next
        Return lTs
    End Function

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

    '''<summary>Test atcSeasonsHour Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsHourConstructorTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsHour))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(24, target.AllSeasons.Count)
        Assert.AreEqual(0, target.AllSeasons(0))
        Assert.AreEqual(23, target.AllSeasons(23))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonBase = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(2010, 1, 1, 0, 0, 0)
        Assert.AreEqual(0, target.SeasonIndex(aDate))
        aDate = Jday(2010, 1, 1, 1, 0, 0)
        Assert.AreEqual(1, target.SeasonIndex(aDate))
        aDate = Jday(2010, 1, 1, 24, 0, 0)
        Assert.AreEqual(0, target.SeasonIndex(aDate))
        aDate = Jday(2010, 1, 1, 23, 0, 0)
        Assert.AreEqual(23, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(Nothing, target.SeasonName(-1))
        Assert.AreEqual("0", target.SeasonName(0))
        Assert.AreEqual("23", target.SeasonName(23))
        Assert.AreEqual(Nothing, target.SeasonName(24))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(0.0, aCalculatedAttributes.GetValue("Mean Hour 00 0"))
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean Hour 01 1"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean Hour 02 2"))
        Assert.AreEqual(12.0, aCalculatedAttributes.GetValue("Mean Hour 12 12"))
        Assert.AreEqual(23.0, aCalculatedAttributes.GetValue("Mean Hour 23 23"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean Hour 01 1"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean Hour 02 2"))
        Assert.AreEqual(12.0, aTS.Attributes.GetValue("Mean Hour 12 12"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(24, actual.Count)
        Assert.AreEqual(0.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(1.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(2).Attributes.GetValue("Mean"))
        Assert.AreEqual(11.0, actual(11).Attributes.GetValue("Mean"))
        Assert.AreEqual(23.0, actual(23).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsHour = New atcSeasonsHour()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(1)  ' hour 1
        lSeasonsSelected.Add(23) ' hour 23

        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(12.0, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(11.45454, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
