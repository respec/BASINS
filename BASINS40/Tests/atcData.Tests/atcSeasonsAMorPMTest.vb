Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsAMorPMTest and is intended
'''to contain all atcSeasonsAMorPMTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsAMorPMTest
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
        Dim lEndDate As Double = Jday(1999, 1, 31, 24, 0, 0)
        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        For lInd As Integer = 0 To lTs.numValues - 1
            If Date.FromOADate(lTs.Dates.Value(lInd)).Hour < 12 Then
                lTs.Value(lInd + 1) = 1
            Else
                lTs.Value(lInd + 1) = 2
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

    '''<summary>Test atcSeasonsAMorPM Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsAMorPMConstructorTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsAMorPM))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(2, target.AllSeasons().Length)
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM() ' TODO: Initialize to an appropriate value
        Dim lClone As atcSeasonsAMorPM = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, lClone.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM()
        Dim aDate As Double = Jday(1999, 1, 1, 11, 0, 0)
        Dim expected As Integer = 0
        Dim actual As Integer = target.SeasonIndex(aDate)
        Assert.AreEqual(expected, actual)
        aDate = Jday(1999, 1, 1, 12, 0, 0)
        expected = 1
        actual = target.SeasonIndex(aDate)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("AM", target.SeasonName(0))
        Assert.AreEqual("PM", target.SeasonName(1))
        Assert.AreEqual(Nothing, target.SeasonName(2))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM() ' TODO: Initialize to an appropriate value
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean AMorPM 0 AM"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean AMorPM 1 PM"))
        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean AMorPM 0 AM"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean AMorPM 1 PM"))

    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Dim DataAttInitial As atcDataAttributes = New atcDataAttributes()
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(1).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsAMorPM = New atcSeasonsAMorPM()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        target.SeasonSelected(0) = True
        target.SeasonSelected(1) = True
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(1.5, actual(0).Attributes.GetValue("Mean"), 0.001)
        Dim lExpectedCount As Int64 = 0
        Assert.AreEqual(lExpectedCount, actual(1).numValues)
    End Sub
End Class
