Imports System.Collections
Imports atcUtility
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for modTimeseriesMathTest and is intended
'''to contain all modTimeseriesMathTest Unit Tests (Done, has some questions)
'''</summary>
<TestClass()> _
Public Class modTimeseriesMathTest
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

    Private Function CreateTestTimeseries(Optional ByVal aStartDate As Double = 0.0, Optional ByVal aEndDate As Double = 0.0, Optional ByVal aSetValue As Double = 0.0) As atcTimeseries
        Dim lStartDate As Double = aStartDate
        If lStartDate = 0.0 Then
            lStartDate = Jday(2010, 1, 1, 0, 0, 0)
        End If

        Dim lEndDate As Double = aEndDate
        If lEndDate = 0.0 Then
            lEndDate = Jday(2010, 12, 31, 24, 0, 0)
        End If

        Dim lDate() As Integer = Nothing
        Dim lTs As atcTimeseries = atcData.NewTimeseries(lStartDate, lEndDate, atcUtility.atcTimeUnit.TUHour, 1)
        Dim lYear As Integer = 0
        For lInd As Integer = 0 To lTs.numValues - 1
            If aSetValue <> 0.0 Then
                lTs.Value(lInd + 1) = aSetValue
            Else
                lTs.Value(lInd + 1) = 1.0
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

    '''<summary>Test Aggregate</summary>
    <TestMethod()> Public Sub AggregateTest()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries()
        Dim aTU As modDate.atcTimeUnit = atcTimeUnit.TUDay
        Dim aTS As Integer = 1
        Dim aTran As modDate.atcTran = atcTran.TranSumDiv
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = Aggregate(aTimeseries, aTU, aTS, aTran, aDataSource)
        Assert.AreEqual(24.0, actual.Attributes.GetValue("Mean"))
    End Sub

    '''<summary>Test BinarySearchFirstGreaterDoubleArrayList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub BinarySearchFirstGreaterDoubleArrayListTest()
        Dim lValues As IEnumerable = {0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0}
        Dim aArray As New ArrayList(lValues)
        Assert.AreEqual(6, modTimeseriesMath_Accessor.BinarySearchFirstGreaterDoubleArrayList(aArray, 6.0))
    End Sub

    '''<summary>Test CommonDates</summary>
    <TestMethod()> Public Sub CommonDatesTest()
        Dim aGroup As New atcTimeseriesGroup
        Dim lTs1 As atcTimeseries = CreateTestTimeseries()
        Dim lSD As Double = Jday(2010, 5, 1, 0, 0, 0)
        Dim lED As Double = Jday(2011, 5, 1, 0, 0, 0)
        Dim lTs2 As atcTimeseries = CreateTestTimeseries(lSD, lED)
        lSD = Jday(2010, 3, 1, 0, 0, 0)
        lED = Jday(2010, 9, 30, 0, 0, 0)
        Dim lTs3 As atcTimeseries = CreateTestTimeseries(lSD, lED)
        aGroup.Add(lTs1)
        aGroup.Add(lTs2)
        aGroup.Add(lTs3)

        Dim aFirstStart As Double = 0.0!
        Dim aFirstStartExpected As Double = lTs1.Dates.Value(1) '???need to ask???
        Dim aLastEnd As Double = 0.0!
        Dim aLastEndExpected As Double = lTs2.Dates.Value(lTs2.numValues)
        Dim aCommonStart As Double = 0.0!
        Dim aCommonStartExpected As Double = lTs2.Dates.Value(1) '???Need to ask???
        Dim aCommonEnd As Double = 0.0!
        Dim aCommonEndExpected As Double = lTs3.Dates.Value(lTs3.numValues)
        Dim expected As Boolean = True
        Dim actual As Boolean = modTimeseriesMath.CommonDates(aGroup, aFirstStart, aLastEnd, aCommonStart, aCommonEnd)
        Assert.AreEqual(aFirstStartExpected, aFirstStart)
        Assert.AreEqual(aLastEndExpected, aLastEnd)
        Assert.AreEqual(aCommonStartExpected, aCommonStart)
        Assert.AreEqual(aCommonEndExpected, aCommonEnd)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>Test ComputePercentile</summary>
    <TestMethod()> Public Sub ComputePercentileTest()
        Dim aTimeseries As New atcTimeseries(Nothing)
        Dim lvalues() As Double = {GetNaN(), 9.0, 4.0, 1.0, 3.0, 8.0, GetNaN(), 7.0, 6.0, 5.0, 2.0, 10.0}
        aTimeseries.Values = lvalues
        Dim aPercentile As Double = 10.0
        modTimeseriesMath.ComputePercentile(aTimeseries, aPercentile)
        Assert.AreEqual(2.0, aTimeseries.Attributes.GetValue("%20"))
    End Sub

    '''<summary>Test ComputePercentileSum</summary>
    <TestMethod()> Public Sub ComputePercentileSumTest()
        Dim aTimeseries As New atcTimeseries(Nothing)
        Dim lvalues() As Double = {9.0, 4.0, 1.0, 3.0, 8.0, 7.0, 6.0, 5.0, 2.0, 10.0}
        aTimeseries.Values = lvalues
        Dim aPercentile As Double = 30.0
        modTimeseriesMath.ComputePercentileSum(aTimeseries, aPercentile)
        Assert.AreEqual(3.0, aTimeseries.Attributes.GetValue("%Sum30"))
    End Sub

    '''<summary>Test ComputeRanks</summary>
    <TestMethod()> Public Sub ComputeRanksTest()
        Dim aTimeseries As New atcTimeseries(Nothing)
        Dim lvalues() As Double = {GetNaN(), 5.0, 5.0, 9.0, 7.0} 'must have the zero position filled with NaN
        aTimeseries.Values = lvalues

        'All ValueAttributes start from index 1, index 0 always holds nothing
        Dim aLowToHigh As Boolean = False
        Dim aAllowTies As Boolean = False
        modTimeseriesMath.ComputeRanks(aTimeseries, aLowToHigh, aAllowTies)
        Assert.AreEqual(4, aTimeseries.ValueAttributes(1).GetValue("Rank"))
        Assert.AreEqual(3, aTimeseries.ValueAttributes(2).GetValue("Rank"))
        Assert.AreEqual(1, aTimeseries.ValueAttributes(3).GetValue("Rank"))
        Assert.AreEqual(2, aTimeseries.ValueAttributes(4).GetValue("Rank"))

        aTimeseries = New atcTimeseries(Nothing)
        aTimeseries.Values = lvalues
        aAllowTies = True
        modTimeseriesMath.ComputeRanks(aTimeseries, aLowToHigh, aAllowTies)
        Assert.AreEqual(3, aTimeseries.ValueAttributes(1).GetValue("Rank"))
        Assert.AreEqual(3, aTimeseries.ValueAttributes(2).GetValue("Rank"))
        Assert.AreEqual(1, aTimeseries.ValueAttributes(3).GetValue("Rank"))
        Assert.AreEqual(2, aTimeseries.ValueAttributes(4).GetValue("Rank"))

        aAllowTies = False
        aLowToHigh = True
        aTimeseries = New atcTimeseries(Nothing)
        aTimeseries.Values = lvalues
        modTimeseriesMath.ComputeRanks(aTimeseries, aLowToHigh, aAllowTies)
        Assert.AreEqual(1, aTimeseries.ValueAttributes(1).GetValue("Rank"))
        Assert.AreEqual(2, aTimeseries.ValueAttributes(2).GetValue("Rank"))
        Assert.AreEqual(4, aTimeseries.ValueAttributes(3).GetValue("Rank"))
        Assert.AreEqual(3, aTimeseries.ValueAttributes(4).GetValue("Rank"))
    End Sub

    '''<summary>Test CopyBaseAttributes</summary>
    <TestMethod()> Public Sub CopyBaseAttributesTest()
        Dim aFromDataset As New atcTimeseries(Nothing)
        Dim lvalues() As Double = {GetNaN(), 5.0, 5.0, 9.0, 7.0}
        aFromDataset.Values = lvalues
        modTimeseriesMath.ComputeRanks(aFromDataset, True, False)
        Dim aToDataSet As New atcTimeseries(Nothing) 'Need to have values in the timeseries
        aToDataSet.Values = lvalues
        aToDataSet.numValues = lvalues.Length - 1

        Dim aNumValues As Integer = 5 ' Has to be the total number of value including the first NaN in the Timeseries or leave blank
        Dim aStartFrom As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartTo As Integer = 0 ' TODO: Initialize to an appropriate value
        modTimeseriesMath.CopyBaseAttributes(aFromDataset, aToDataSet, aNumValues, aStartFrom, aStartTo)
        Assert.AreEqual(Nothing, aToDataSet.ValueAttributes(0).GetValue("Rank"))  '???Need to ask???
        Assert.AreEqual(1, aToDataSet.ValueAttributes(1).GetValue("Rank"))
        Assert.AreEqual(2, aToDataSet.ValueAttributes(2).GetValue("Rank"))
        Assert.AreEqual(4, aToDataSet.ValueAttributes(3).GetValue("Rank"))
        Assert.AreEqual(3, aToDataSet.ValueAttributes(4).GetValue("Rank"))
    End Sub

    '''<summary>Test DatasetOrGroupToGroup</summary>
    <TestMethod()> Public Sub DatasetOrGroupToGroupTest()
        Dim aObject As Object = Nothing

        aObject = New atcTimeseries(Nothing)
        Assert.IsInstanceOfType(modTimeseriesMath.DatasetOrGroupToGroup(aObject), GetType(atcTimeseriesGroup))

        aObject = New atcTimeseriesGroup()
        Assert.IsInstanceOfType(modTimeseriesMath.DatasetOrGroupToGroup(aObject), GetType(atcTimeseriesGroup))

        aObject = New atcDataSet()
        Assert.IsInstanceOfType(modTimeseriesMath.DatasetOrGroupToGroup(aObject), GetType(atcDataGroup))

    End Sub

    '''<summary>Test DoMath</summary>
    <TestMethod()> Public Sub DoMathTest()
        Dim aOperationName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aArgs As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.DoMath(aOperationName, aArgs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FillMissingByInterpolation</summary>
    <TestMethod()> Public Sub FillMissingByInterpolationTest()
        Dim aOldTSer As atcTimeseries = CreateTestTimeseries()
        aOldTSer.Value(11) = Double.NaN
        aOldTSer.Value(12) = Double.NaN
        aOldTSer.Value(13) = Double.NaN

        aOldTSer.Value(41) = Double.NaN
        aOldTSer.Value(42) = Double.NaN
        aOldTSer.Value(43) = Double.NaN

        Dim aMaxFillLength As Double = 7.0
        Dim aFillInstances As ArrayList = Nothing
        Dim aMissingValue As Double = GetNaN()
        Dim expected As atcTimeseries = Nothing
        Dim actual As atcTimeseries = modTimeseriesMath.FillMissingByInterpolation(aOldTSer, aMaxFillLength, aFillInstances, aMissingValue)
        Assert.AreNotEqual(Double.NaN, actual.Value(11))
        Assert.AreNotEqual(Double.NaN, actual.Value(12))
        Assert.AreNotEqual(Double.NaN, actual.Value(13))
    End Sub

    '''<summary>Test FillValues</summary>
    <TestMethod()> Public Sub FillValuesTest()
        'TODO: Need to discuss how this routine is working
        'the replacing missing value with fill value is not working
        Dim aOldTSer As atcTimeseries = CreateTestTimeseries()
        Assert.AreEqual(1.0, aOldTSer.Attributes.GetValue("Mean"))

        aOldTSer.Value(1) = -999.0
        For I As Integer = 2 To 50
            aOldTSer.Value(I) = 0.0
        Next

        Dim aTU As modDate.atcTimeUnit = atcTimeUnit.TUHour
        Dim aTS As Long = 1
        Dim aFillVal As Double = 100.0
        Dim aMissVal As Double = -999.0
        Dim aAccumVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = modTimeseriesMath.FillValues(aOldTSer, aTU, aTS, aFillVal, aMissVal, aAccumVal, aDataSource)
        Assert.AreEqual(100.0, aOldTSer.Attributes.GetValue("Mean"))
        Assert.AreEqual("This test", "Failed")
    End Sub

    '''<summary>Test FindDateAtOrAfter</summary>
    <TestMethod()> Public Sub FindDateAtOrAfterTest()
        Dim lTs As atcTimeseries = CreateTestTimeseries()

        Dim aDatesToSearch() As Double = lTs.Dates.Values
        Dim aDate As Double = lTs.Dates.Value(100)
        Dim aStartAt As Integer = 0 ' TODO: Initialize to an appropriate value

        Dim actual As Integer = modTimeseriesMath.FindDateAtOrAfter(aDatesToSearch, aDate, aStartAt)
        Assert.AreEqual(100, actual)
    End Sub

    '''<summary>Test FindNextNotMissing</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub FindNextNotMissingTest()
        Dim aTser As atcTimeseries = CreateTestTimeseries()
        For I As Integer = 15 To 20
            aTser.Value(I) = -999.0
        Next
        Dim aInd As Integer = 15 ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Double = -999.0
        Dim actual As Integer = modTimeseriesMath_Accessor.FindNextNotMissing(aTser, aInd, aMissingValue)
        Assert.AreEqual(21, actual)
    End Sub

    '''<summary>Test FitLine</summary>
    <TestMethod()> Public Sub FitLineTest()

        Dim lSD As Double = Jday(2010, 1, 1, 0, 0, 0)
        Dim lED As Double = Jday(2010, 1, 9, 24, 0, 0)
        Dim lDates As New atcTimeseries(Nothing)
        Dim lDatesValues() As Double = NewTimeseries(lSD, lED, atcTimeUnit.TUDay, 1, Nothing).Dates.Values()
        lDates.Values = lDatesValues
        Dim aTSerX As New atcTimeseries(Nothing)
        Dim lValuesX() As Double = {GetNaN(), 1, 2, 3, 4, 5, 4, 3, 2, 1}

        aTSerX.Dates = lDates
        aTSerX.Values = lValuesX
        Dim aTSerY As New atcTimeseries(Nothing)
        Dim lValuesY() As Double = {GetNaN(), 3, 4, 5, 6, 7, 6, 5, 4, 3}
        aTSerY.Dates = lDates
        aTSerY.Values = lValuesY

        Dim aACoef As Double = 0.0!
        Dim aACoefExpected As Double = 1.0!
        Dim aBCoef As Double = 0.0!
        Dim aBCoefExpected As Double = -2.0!
        Dim aRSquare As Double = 0.0!
        Dim aRSquareExpected As Double = -1.764285!
        modTimeseriesMath.FitLine(aTSerX, aTSerY, aACoef, aBCoef, aRSquare) 'Not sure the logic is correct
        Assert.AreEqual(aACoefExpected, aACoef, 0.001)
        Assert.AreEqual(aBCoefExpected, aBCoef, 0.001)
        Assert.AreEqual(aRSquareExpected, aRSquare, 0.001)
    End Sub

    '''<summary>Test GetNextDateIndex</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub GetNextDateIndexTest()
        Dim aTs As atcTimeseries = CreateTestTimeseries()
        For I As Integer = 5 To 15
            aTs.Value(I) = GetNaN()
        Next

        Dim aFilterNoData As Boolean = False
        Dim aIndex As Integer = 5
        Dim aIndexExpected As Integer = 6
        Dim aNextDate As Double = 0.0!
        Dim aNextDateExpected As Double = aTs.Dates.Value(aIndexExpected)
        modTimeseriesMath_Accessor.GetNextDateIndex(aTs, aFilterNoData, aIndex, aNextDate)
        Assert.AreEqual(aIndexExpected, aIndex)
        Assert.AreEqual(aNextDateExpected, aNextDate)

        aFilterNoData = True
        aIndexExpected = 16
        aNextDateExpected = aTs.Dates.Value(aIndexExpected)
        modTimeseriesMath_Accessor.GetNextDateIndex(aTs, aFilterNoData, aIndex, aNextDate)
        Assert.AreEqual(aIndexExpected, aIndex)
        Assert.AreEqual(aNextDateExpected, aNextDate)
    End Sub

    '''<summary>Test MakeBins</summary>
    <TestMethod()> Public Sub MakeBinsTest()
        Dim aTS As New atcTimeseries(Nothing)
        Dim lSD As Double = Jday(2010, 1, 1, 0, 0, 0)
        Dim lED As Double = Jday(2010, 1, 20, 24, 0, 0)
        Dim lDates As New atcTimeseries(Nothing)
        Dim lDatesValues() As Double = NewTimeseries(lSD, lED, atcTimeUnit.TUDay, 1, Nothing).Dates.Values()
        lDates.Values = lDatesValues
        aTS.Dates = lDates

        Dim lValues() As Double = {GetNaN(), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}
        aTS.Values = lValues

        Dim aMaxBinSize As Integer = 0
        Dim actual As atcCollection = modTimeseriesMath.MakeBins(aTS, aMaxBinSize)
        Assert.AreEqual(3, actual.Count) 'Need to check logic further

        aMaxBinSize = 5
        actual = modTimeseriesMath.MakeBins(aTS, aMaxBinSize)
        Assert.AreEqual(6, actual.Count) 'Need to check logic further
    End Sub

    '''<summary>Test MergeAttributes</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub MergeAttributesTest()
        Dim aGroup As New atcTimeseriesGroup
        Dim aTarget As New atcTimeseries(Nothing)
        aGroup.Add(CreateTestTimeseries())
        aGroup.Add(CreateTestTimeseries())
        aGroup.Add(CreateTestTimeseries())
        aGroup(0).Attributes.SetValue("ID", 1) : aGroup(0).Attributes.SetValue("Constituent", "TestData")
        aGroup(1).Attributes.SetValue("ID", 2) : aGroup(1).Attributes.SetValue("Constituent", "TestData")
        aGroup(2).Attributes.SetValue("ID", 3) : aGroup(2).Attributes.SetValue("Constituent", "TestData")
        modTimeseriesMath_Accessor.MergeAttributes(aGroup, aTarget)
        Assert.AreEqual(1, aTarget.Attributes.Count)
        Assert.AreEqual("Constituent", aTarget.Attributes.Item(0).Definition.Name)
        Assert.AreEqual("TestData", aTarget.Attributes.Item(0).Value)
    End Sub

    '''<summary>Test MergeDates</summary>
    <TestMethod()> Public Sub MergeDatesTest()
        Dim aGroup As New atcTimeseriesGroup
        Dim lTs1 As atcTimeseries = CreateTestTimeseries()
        Dim lSD As Double = Jday(2010, 5, 1, 0, 0, 0)
        Dim lED As Double = Jday(2011, 5, 1, 0, 0, 0)
        Dim lTs2 As atcTimeseries = CreateTestTimeseries(lSD, lED)
        lSD = Jday(2010, 3, 1, 0, 0, 0)
        lED = Jday(2010, 9, 30, 0, 0, 0)
        Dim lTs3 As atcTimeseries = CreateTestTimeseries(lSD, lED)
        aGroup.Add(lTs1)
        aGroup.Add(lTs2)
        aGroup.Add(lTs3)

        Dim aFilterNoData As Boolean = False ' false to keep missing values
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries = modTimeseriesMath.MergeDates(aGroup, aFilterNoData)
        lSD = Jday(2010, 1, 1, 0, 0, 0)
        Assert.AreEqual(lSD, actual.Value(0))
        lED = Jday(2011, 5, 1, 0, 0, 0)
        Assert.AreEqual(lED, actual.Value(actual.numValues))
    End Sub

    '''<summary>Test MergeTimeseries</summary>
    <TestMethod()> Public Sub MergeTimeseriesTest()
        Dim aGroup As New atcTimeseriesGroup
        Dim lTs1 As atcTimeseries = CreateTestTimeseries()
        Dim lSD As Double = Jday(2010, 5, 1, 0, 0, 0)
        Dim lED As Double = Jday(2011, 5, 1, 0, 0, 0)
        Dim lTs2 As atcTimeseries = CreateTestTimeseries(lSD, lED, 2.0)
        lSD = Jday(2010, 3, 1, 0, 0, 0)
        lED = Jday(2010, 9, 30, 0, 0, 0)
        Dim lTs3 As atcTimeseries = CreateTestTimeseries(lSD, lED, 3.0)
        aGroup.Add(lTs1)
        aGroup.Add(lTs2)
        aGroup.Add(lTs3)

        Dim aFilterNoData As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.MergeTimeseries(aGroup, aFilterNoData)
        lSD = Jday(2010, 1, 1, 0, 0, 0)
        lED = Jday(2011, 5, 1, 0, 0, 0)
        Assert.AreEqual(lSD, actual.Dates.Value(0))
        Assert.AreEqual(lED, actual.Dates.Value(actual.numValues))
    End Sub

    '''<summary>Test NewDates</summary>
    <TestMethod()> Public Sub NewDatesTest()
        Dim aTSer As atcTimeseries = CreateTestTimeseries()
        Dim aTU As modDate.atcTimeUnit = atcTimeUnit.TUYear
        Dim aTS As Integer = 1
        Dim actual() As Double = modTimeseriesMath.NewDates(aTSer, aTU, aTS)
        Assert.AreEqual(2, actual.Length)

        aTU = atcTimeUnit.TUMonth
        actual = NewDates(aTSer, aTU, aTS)
        Assert.AreEqual(13, actual.Length)
    End Sub

    '''<summary>Test NewDates</summary>
    <TestMethod()> Public Sub NewDatesTest1()
        Dim aStartDate As Double = Jday(2010, 1, 1, 0, 0, 0)
        Dim aEndDate As Double = Jday(2010, 12, 31, 24, 0, 0)
        Dim aTU As modDate.atcTimeUnit = atcTimeUnit.TUYear
        Dim aTS As Integer = 1

        Dim actual() As Double = modTimeseriesMath.NewDates(aStartDate, aEndDate, aTU, aTS)
        Assert.AreEqual(2, actual.Length)
        Assert.AreEqual(aEndDate, actual(actual.Length - 1))

        aTU = atcTimeUnit.TUMonth
        actual = NewDates(aStartDate, aEndDate, aTU, aTS)
        Assert.AreEqual(13, actual.Length)
        Assert.AreEqual(aEndDate, actual(actual.Length - 1))
    End Sub

    '''<summary>Test NewTimeseries</summary>
    <TestMethod()> Public Sub NewTimeseriesTest()
        Dim aStartDate As Double = Jday(2010, 1, 1, 0, 0, 0)
        Dim aEndDate As Double = Jday(2011, 12, 31, 24, 0, 0)
        Dim aTU As modDate.atcTimeUnit = atcTimeUnit.TUHour
        Dim aTS As Integer = 1
        Dim aDataSource As atcTimeseriesSource = Nothing
        Dim aSetAllValues As Double = 1.0

        Dim actual As atcTimeseries = NewTimeseries(aStartDate, aEndDate, aTU, aTS, aDataSource, aSetAllValues)
        Assert.AreEqual(aStartDate, actual.Dates.Value(0))
        Assert.AreEqual(aEndDate, actual.Dates.Value(actual.numValues))
    End Sub

    '''<summary>Test SplitBin</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SplitBinTest()
        Dim aTS As New atcTimeseries(Nothing)
        Dim lSD As Double = Jday(2010, 1, 1, 0, 0, 0)
        Dim lED As Double = Jday(2010, 1, 20, 24, 0, 0)
        Dim lDates As New atcTimeseries(Nothing)
        Dim lDatesValues() As Double = NewTimeseries(lSD, lED, atcTimeUnit.TUDay, 1, Nothing).Dates.Values()
        lDates.Values = lDatesValues
        aTS.Dates = lDates

        Dim lValues() As Double = {GetNaN(), 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}
        aTS.Values = lValues

        Dim aMaxBinSize As Integer = 5
        Dim aBins As atcCollection = modTimeseriesMath.MakeBins(aTS, aMaxBinSize)
        Assert.AreEqual(6, aBins.Count)
        Dim aBin As ArrayList = aBins(2)
        Dim aBinIndex As Integer = 2
        modTimeseriesMath_Accessor.SplitBin(aBins, aBin, aBinIndex)
        Assert.AreEqual(7, aBins.Count)
    End Sub

    '''<summary>Test SubsetByDate</summary>
    <TestMethod()> Public Sub SubsetByDateTest()
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries()
        Dim aStartDate As Double = Jday(2010, 5, 5, 0, 0, 0)
        Dim aEndDate As Double = Jday(2010, 5, 31, 24, 0, 0)
        Dim aDataSource As atcTimeseriesSource = Nothing
        Dim actual As atcTimeseries = SubsetByDate(aTimeseries, aStartDate, aEndDate, aDataSource)
        Assert.AreEqual(aStartDate, actual.Dates.Value(0))
        Assert.AreEqual(aEndDate, actual.Dates.Value(actual.numValues))
    End Sub

    '''<summary>Test SubsetByDateBoundary</summary>
    <TestMethod()> Public Sub SubsetByDateBoundaryTest()
        Dim lSD As Double = Jday(1999, 5, 5, 0, 0, 0)
        Dim lED As Double = Jday(2003, 11, 15, 0, 0, 0)
        Dim aTimeseries As atcTimeseries = CreateTestTimeseries(lSD, lED)
        Dim aStartMonth As Integer = 1
        Dim aStartDay As Integer = 1
        Dim aDataSource As atcTimeseriesSource = Nothing
        Dim aFirstYear As Integer = 2000
        Dim aLastYear As Integer = 2002
        Dim aEndMonth As Integer = 12
        Dim aEndDay As Integer = 31
        Dim actual As atcTimeseries = modTimeseriesMath.SubsetByDateBoundary(aTimeseries, aStartMonth, aStartDay, aDataSource, aFirstYear, aLastYear, aEndMonth, aEndDay)
        lSD = Jday(2000, 1, 1, 0, 0, 0)
        lED = Jday(2002, 12, 31, 24, 0, 0)
        Assert.AreEqual(lSD, actual.Dates.Value(0))
        Assert.AreEqual(lED, actual.Dates.Value(actual.numValues))
    End Sub
End Class
