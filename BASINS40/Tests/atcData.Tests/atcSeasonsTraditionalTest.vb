Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData
Imports atcUtility

'''<summary>
'''This is a test class for atcSeasonsTraditionalTest and is intended
'''to contain all atcSeasonsTraditionalTest Unit Tests (Done)
'''</summary>
<TestClass()> _
Public Class atcSeasonsTraditionalTest
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
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lDate As Date
        For lInd As Integer = 0 To lTs.numValues - 1
            lDate = Date.FromOADate(lTs.Dates.Value(lInd))
            'NOTE: The exact starting day of seasons is not always agreed on
            'and can change from year to year. 
            'Here we assume March 20, June 21, September 22 and December 21 
            'are always the first days of the seasons
            Select Case lDate.Month
                Case 1, 2 : lTs.Value(lInd + 1) = 0
                Case 3 : If lDate.Day < 20 Then lTs.Value(lInd + 1) = 0 Else lTs.Value(lInd + 1) = 1
                Case 4, 5 : lTs.Value(lInd + 1) = 1
                Case 6 : If lDate.Day < 21 Then lTs.Value(lInd + 1) = 1 Else lTs.Value(lInd + 1) = 2
                Case 7, 8 : lTs.Value(lInd + 1) = 2
                Case 9 : If lDate.Day < 22 Then lTs.Value(lInd + 1) = 2 Else lTs.Value(lInd + 1) = 3
                Case 10, 11 : lTs.Value(lInd + 1) = 3
                Case 12 : If lDate.Day < 21 Then lTs.Value(lInd + 1) = 3 Else lTs.Value(lInd + 1) = 0
                Case Else : lTs.Value(lInd + 1) = -1
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

    '''<summary>Test atcSeasonsTraditional Constructor</summary>
    <TestMethod()> Public Sub atcSeasonsTraditionalConstructorTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional()
        Assert.IsInstanceOfType(target, GetType(atcSeasonsTraditional))
    End Sub

    '''<summary>Test AllSeasonNames</summary>
    <TestMethod()> Public Sub AllSeasonNamesTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(4, target.AllSeasonNames.Length)
        Assert.AreEqual("Spring", target.AllSeasonNames(1))
    End Sub

    '''<summary>Test AllSeasons</summary>
    <TestMethod()> Public Sub AllSeasonsTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional() ' TODO: Initialize to an appropriate value
        Assert.AreEqual(4, target.AllSeasons.Length)
        Assert.AreEqual(1, target.AllSeasons(1))
    End Sub

    '''<summary>Test Clone</summary>
    <TestMethod()> Public Sub CloneTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional() ' TODO: Initialize to an appropriate value
        Dim actual As atcSeasonBase = target.Clone
        Assert.AreEqual(target.AllSeasonNames.Length, actual.AllSeasonNames.Length)
        Assert.AreEqual(target.AllSeasonNames(1), actual.AllSeasonNames(1))
    End Sub

    '''<summary>Test SeasonIndex</summary>
    <TestMethod()> Public Sub SeasonIndexTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional() ' TODO: Initialize to an appropriate value
        Dim aDate As Double = Jday(2010, 2, 15, 0, 0, 0)
        Assert.AreEqual(0, target.SeasonIndex(aDate))
        aDate = Jday(2010, 3, 20, 0, 0, 0)
        Assert.AreEqual(1, target.SeasonIndex(aDate))
        aDate = Jday(2010, 7, 20, 0, 0, 0)
        Assert.AreEqual(2, target.SeasonIndex(aDate))
        aDate = Jday(2010, 10, 20, 0, 0, 0)
        Assert.AreEqual(3, target.SeasonIndex(aDate))
    End Sub

    '''<summary>Test SeasonName</summary>
    <TestMethod()> Public Sub SeasonNameTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional() ' TODO: Initialize to an appropriate value
        Assert.AreEqual("Winter", target.SeasonName(0))
        Assert.AreEqual("Spring", target.SeasonName(1))
        Assert.AreEqual("Summer", target.SeasonName(2))
        Assert.AreEqual("Autumn", target.SeasonName(3))
    End Sub

    '''<summary>Test SetSeasonalAttributes</summary>
    <TestMethod()> Public Sub SetSeasonalAttributesTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aAttributes As New atcDataAttributes
        aAttributes.Add("Mean", 0)

        Dim aCalculatedAttributes As New atcDataAttributes
        target.SetSeasonalAttributes(aTS, aAttributes, aCalculatedAttributes)
        Assert.AreEqual(0.0, aCalculatedAttributes.GetValue("Mean Traditional 0 Winter"))
        Assert.AreEqual(1.0, aCalculatedAttributes.GetValue("Mean Traditional 1 Spring"))
        Assert.AreEqual(2.0, aCalculatedAttributes.GetValue("Mean Traditional 2 Summer"))
        Assert.AreEqual(3.0, aCalculatedAttributes.GetValue("Mean Traditional 3 Autumn"))

        target.SetSeasonalAttributes(aTS, aAttributes)
        Assert.AreEqual(0.0, aTS.Attributes.GetValue("Mean Traditional 0 Winter"))
        Assert.AreEqual(1.0, aTS.Attributes.GetValue("Mean Traditional 1 Spring"))
        Assert.AreEqual(2.0, aTS.Attributes.GetValue("Mean Traditional 2 Summer"))
        Assert.AreEqual(3.0, aTS.Attributes.GetValue("Mean Traditional 3 Autumn"))
    End Sub

    '''<summary>Test Split</summary>
    <TestMethod()> Public Sub SplitTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional()
        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.Split(aTS, aSource)
        Assert.AreEqual(4, actual.Count)
        Assert.AreEqual(0.0, actual(0).Attributes.GetValue("Mean"))
        Assert.AreEqual(1.0, actual(1).Attributes.GetValue("Mean"))
        Assert.AreEqual(2.0, actual(2).Attributes.GetValue("Mean"))
        Assert.AreEqual(3.0, actual(3).Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test SplitBySelected</summary>
    <TestMethod()> Public Sub SplitBySelectedTest()
        Dim target As atcSeasonsTraditional = New atcSeasonsTraditional()

        Dim aTS As atcTimeseries = CreateTestTimeseries()
        Dim lSeasonsSelected As New ArrayList
        lSeasonsSelected.Add(0) ' winter
        lSeasonsSelected.Add(1) ' spring
        target.SeasonsSelected = lSeasonsSelected

        Dim aSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseriesGroup = Nothing
        actual = target.SplitBySelected(aTS, aSource)
        Assert.AreEqual(2, actual.Count)
        Assert.AreEqual(0.51, actual(0).Attributes.GetValue("Mean"), 0.001)
        Assert.AreEqual(2.4918, actual(1).Attributes.GetValue("Mean"), 0.001)
    End Sub
End Class
