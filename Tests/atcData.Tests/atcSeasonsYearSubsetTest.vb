Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsYearSubsetTest and is intended
'''to contain all atcSeasonsYearSubsetTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsYearSubsetTest
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

    Private pStartDate As Date = New Date(1900, 1, 1, 0, 0, 0)
    Private pEndDate As Date = New Date(1900, 6, 30, 23, 0, 0)
    Private pEndDateNextYear As Boolean = False

    Private Function CreateTestTimeseries() As atcTimeseries
        Dim lStartDate As Double = Jday(1999, 1, 1, 0, 0, 0)
        Dim lEndDate As Double = Jday(2002, 12, 31, 24, 0, 0)
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lDate As Date
        Dim lDateNoYear As Date
        For lInd As Integer = 0 To lTs.numValues - 1
            lDate = Date.FromOADate(lTs.Dates.Value(lInd))
            Dim lDay As Integer = lDate.Day
            If lDay = 29 AndAlso lDate.Month = 2 Then
                lDay = 28  'place leap day in same season as the 28th
            End If
            lDateNoYear = New Date(pStartDate.Year, lDate.Month, lDay, _
                                               lDate.Hour, lDate.Minute, lDate.Second, _
                                               lDate.Millisecond)
            If pEndDateNextYear Then
                If lDateNoYear >= pStartDate Then
                    lTs.Value(lInd + 1) = 1
                Else
                    lDateNoYear = New Date(pEndDate.Year, lDate.Month, lDay, _
                                           lDate.Hour, lDate.Minute, lDate.Second, _
                                           lDate.Millisecond)
                    If lDateNoYear < pEndDate Then
                        lTs.Value(lInd + 1) = 1
                    Else
                        lTs.Value(lInd + 1) = 0
                    End If
                End If
            Else
                If lDateNoYear >= pStartDate AndAlso lDateNoYear < pEndDate Then
                    lTs.Value(lInd + 1) = 1
                Else
                    lTs.Value(lInd + 1) = 0
                End If
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

    '''<summary>Test atcSeasonsYearSubset Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsYearSubsetConstructorTest()
        Dim aStartMonth As Integer = pStartDate.Month
        Dim aStartDay As Integer = pStartDate.Day
        Dim aEndMonth As Integer = pEndDate.Month
        Dim aEndDay As Integer = pEndDate.Day
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(aStartMonth, aStartDay, aEndMonth, aEndDay)
        Assert.IsInstanceOfType(target, GetType(atcSeasonsYearSubset))
    End Sub

    '''<summary>Test atcSeasonsYearSubset Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsYearSubsetConstructorTest1()
        Dim aStartDate As DateTime = pStartDate
        Dim aEndDate As DateTime = pEndDate
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(aStartDate, aEndDate)
        Assert.IsInstanceOfType(target, GetType(atcSeasonsYearSubset))
    End Sub

    '''<summary>Test atcSeasonsYearSubset Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsYearSubsetConstructorTest2()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsYearSubset))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate) ' TODO: Initialize to an appropriate value
        Assert.AreEqual(2, target.AllSeasons.Length)
        Assert.AreEqual(0, target.AllSeasons(0))
        Assert.AreEqual(1, target.AllSeasons(1))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate) ' TODO: Initialize to an appropriate value
        Dim actual As atcSeasonBase = target.Clone
        Assert.AreEqual(target.SeasonsSelected.Count, actual.SeasonsSelected.Count)
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate) ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(1999, 7, 15, 0, 0, 0)
        Assert.AreEqual(0, target.SeasonIndex(aDate))
        aDate = Jday(2000, 3, 3, 0, 0, 0)
        Assert.AreEqual(1, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate) ' TODO: Initialize to an appropriate value
        Assert.AreEqual("Inside", target.SeasonName(1))
        Assert.AreEqual("Outside", target.SeasonName(0))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate)
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean YearSubset 1 Inside"))
        Assert.AreEqual(0.0, aCalculatedAttributes.GetValue("Mean YearSubset 0 Outside"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean YearSubset 1 Inside"))
        Assert.AreEqual(0.0, aTS.Attributes.GetValue("Mean YearSubset 0 Outside"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate)
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(1.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(0.0, actual(1).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsYearSubset = New atcSeasonsYearSubset(pStartDate, pEndDate)

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(0) 'outside
        'lSeasonsSelected.Add(1) 'inside
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(0.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(1.0, actual(1).Attributes.GetValue("Mean"))
    End Sub
End Class
