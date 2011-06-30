Imports System.Collections

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for modTimeseriesMathTest and is intended
'''to contain all modTimeseriesMathTest Unit Tests
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

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
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


    '''<summary>
    '''A test for Aggregate
    '''</summary>
    <TestMethod()> _
    Public Sub AggregateTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTran As modDate.atcTran = New modDate.atcTran() ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.Aggregate(aTimeseries, aTU, aTS, aTran, aDataSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for BinarySearchFirstGreaterDoubleArrayList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub BinarySearchFirstGreaterDoubleArrayListTest()
        Dim aArray As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modTimeseriesMath_Accessor.BinarySearchFirstGreaterDoubleArrayList(aArray, aValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CommonDates
    '''</summary>
    <TestMethod()> _
    Public Sub CommonDatesTest()
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aFirstStart As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFirstStartExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLastEnd As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aLastEndExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aCommonStart As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aCommonStartExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aCommonEnd As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aCommonEndExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = modTimeseriesMath.CommonDates(aGroup, aFirstStart, aLastEnd, aCommonStart, aCommonEnd)
        Assert.AreEqual(aFirstStartExpected, aFirstStart)
        Assert.AreEqual(aLastEndExpected, aLastEnd)
        Assert.AreEqual(aCommonStartExpected, aCommonStart)
        Assert.AreEqual(aCommonEndExpected, aCommonEnd)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ComputePercentile
    '''</summary>
    <TestMethod()> _
    Public Sub ComputePercentileTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aPercentile As Double = 0.0! ' TODO: Initialize to an appropriate value
        modTimeseriesMath.ComputePercentile(aTimeseries, aPercentile)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ComputePercentileSum
    '''</summary>
    <TestMethod()> _
    Public Sub ComputePercentileSumTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aPercentile As Double = 0.0! ' TODO: Initialize to an appropriate value
        modTimeseriesMath.ComputePercentileSum(aTimeseries, aPercentile)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ComputeRanks
    '''</summary>
    <TestMethod()> _
    Public Sub ComputeRanksTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aLowToHigh As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aAllowTies As Boolean = False ' TODO: Initialize to an appropriate value
        modTimeseriesMath.ComputeRanks(aTimeseries, aLowToHigh, aAllowTies)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CopyBaseAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub CopyBaseAttributesTest()
        Dim aFromDataset As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aToDataSet As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aNumValues As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartFrom As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartTo As Integer = 0 ' TODO: Initialize to an appropriate value
        modTimeseriesMath.CopyBaseAttributes(aFromDataset, aToDataSet, aNumValues, aStartFrom, aStartTo)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for DatasetOrGroupToGroup
    '''</summary>
    <TestMethod()> _
    Public Sub DatasetOrGroupToGroupTest()
        Dim aObject As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        actual = modTimeseriesMath.DatasetOrGroupToGroup(aObject)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DoMath
    '''</summary>
    <TestMethod()> _
    Public Sub DoMathTest()
        Dim aOperationName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aArgs As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.DoMath(aOperationName, aArgs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FillMissingByInterpolation
    '''</summary>
    <TestMethod()> _
    Public Sub FillMissingByInterpolationTest()
        Dim aOldTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aMaxFillLength As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFillInstances As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.FillMissingByInterpolation(aOldTSer, aMaxFillLength, aFillInstances, aMissingValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FillValues
    '''</summary>
    <TestMethod()> _
    Public Sub FillValuesTest()
        Dim aOldTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTS As Long = 0 ' TODO: Initialize to an appropriate value
        Dim aFillVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMissVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aAccumVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.FillValues(aOldTSer, aTU, aTS, aFillVal, aMissVal, aAccumVal, aDataSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindDateAtOrAfter
    '''</summary>
    <TestMethod()> _
    Public Sub FindDateAtOrAfterTest()
        Dim aDatesToSearch() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aStartAt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modTimeseriesMath.FindDateAtOrAfter(aDatesToSearch, aDate, aStartAt)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FindNextNotMissing
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub FindNextNotMissingTest()
        Dim aTser As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aInd As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMissingValue As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modTimeseriesMath_Accessor.FindNextNotMissing(aTser, aInd, aMissingValue)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FitLine
    '''</summary>
    <TestMethod()> _
    Public Sub FitLineTest()
        Dim aTSerX As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTSerY As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aACoef As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aACoefExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aBCoef As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aBCoefExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aRSquare As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aRSquareExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        modTimeseriesMath.FitLine(aTSerX, aTSerY, aACoef, aBCoef, aRSquare)
        Assert.AreEqual(aACoefExpected, aACoef)
        Assert.AreEqual(aBCoefExpected, aBCoef)
        Assert.AreEqual(aRSquareExpected, aRSquare)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetNextDateIndex
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub GetNextDateIndexTest()
        Dim aTs As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aFilterNoData As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aIndexExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNextDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aNextDateExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        modTimeseriesMath_Accessor.GetNextDateIndex(aTs, aFilterNoData, aIndex, aNextDate)
        Assert.AreEqual(aIndexExpected, aIndex)
        Assert.AreEqual(aNextDateExpected, aNextDate)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for MakeBins
    '''</summary>
    <TestMethod()> _
    Public Sub MakeBinsTest()
        Dim aTS As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aMaxBinSize As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = modTimeseriesMath.MakeBins(aTS, aMaxBinSize)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MergeAttributes
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub MergeAttributesTest()
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aTarget As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        modTimeseriesMath_Accessor.MergeAttributes(aGroup, aTarget)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for MergeDates
    '''</summary>
    <TestMethod()> _
    Public Sub MergeDatesTest()
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aFilterNoData As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.MergeDates(aGroup, aFilterNoData)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MergeTimeseries
    '''</summary>
    <TestMethod()> _
    Public Sub MergeTimeseriesTest()
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aFilterNoData As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.MergeTimeseries(aGroup, aFilterNoData)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NewDates
    '''</summary>
    <TestMethod()> _
    Public Sub NewDatesTest()
        Dim aTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Double
        actual = modTimeseriesMath.NewDates(aTSer, aTU, aTS)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NewDates
    '''</summary>
    <TestMethod()> _
    Public Sub NewDatesTest1()
        Dim aStartDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEndDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim actual() As Double
        actual = modTimeseriesMath.NewDates(aStartDate, aEndDate, aTU, aTS)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for NewTimeseries
    '''</summary>
    <TestMethod()> _
    Public Sub NewTimeseriesTest()
        Dim aStartDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEndDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTU As modDate.atcTimeUnit = New modDate.atcTimeUnit() ' TODO: Initialize to an appropriate value
        Dim aTS As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aSetAllValues As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.NewTimeseries(aStartDate, aEndDate, aTU, aTS, aDataSource, aSetAllValues)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SplitBin
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub SplitBinTest()
        Dim aBins As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aBin As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim aBinIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        modTimeseriesMath_Accessor.SplitBin(aBins, aBin, aBinIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SubsetByDate
    '''</summary>
    <TestMethod()> _
    Public Sub SubsetByDateTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aStartDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEndDate As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.SubsetByDate(aTimeseries, aStartDate, aEndDate, aDataSource)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SubsetByDateBoundary
    '''</summary>
    <TestMethod()> _
    Public Sub SubsetByDateBoundaryTest()
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aStartMonth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartDay As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aFirstYear As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aLastYear As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndMonth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndDay As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modTimeseriesMath.SubsetByDateBoundary(aTimeseries, aStartMonth, aStartDay, aDataSource, aFirstYear, aLastYear, aEndMonth, aEndDay)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
