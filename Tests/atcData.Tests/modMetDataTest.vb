Imports atcUtility
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for modMetDataTest and is intended
'''to contain all modMetDataTest Unit Tests
'''</summary>
<TestClass()> _
Public Class modMetDataTest
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

    '''<summary>Test BuildDBFSummary</summary>
    <TestMethod()> Public Sub BuildDBFSummaryTest()
        Dim aSummStr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aFName As String = String.Empty ' TODO: Initialize to an appropriate value
        modMetData.BuildDBFSummary(aSummStr, aFName)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CalcMetDistances</summary>
    <TestMethod()> Public Sub CalcMetDistancesTest()
        Dim aTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aStations As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aAdjustAttribute As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = modMetData.CalcMetDistances(aTSer, aStations, aAdjustAttribute)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ClosestPrecip</summary>
    <TestMethod()> Public Sub ClosestPrecipTest()
        Dim aTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aStations As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aDV As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aSDt As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aEDt As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTol As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aObsTime As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseries
        actual = modMetData.ClosestPrecip(aTSer, aStations, aDV, aSDt, aEDt, aTol, aObsTime)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CurrentObsTime</summary>
    <TestMethod()> Public Sub CurrentObsTimeTest()
        Dim aTS As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aPos As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = modMetData.CurrentObsTime(aTS, aPos)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FillDailyTser</summary>
    <TestMethod()> Public Sub FillDailyTserTest()
        Dim aTS2Fill As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTS2FillExpected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTS2FillOT As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTSAvail As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aTSAvailOT As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aMVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMAcc As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTol As Double = 0.0! ' TODO: Initialize to an appropriate value
        modMetData.FillDailyTser(aTS2Fill, aTS2FillOT, aTSAvail, aTSAvailOT, aMVal, aMAcc, aTol)
        Assert.AreEqual(aTS2FillExpected, aTS2Fill)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test FillHourlyTser</summary>
    <TestMethod()> Public Sub FillHourlyTserTest()
        Dim aTS2Fill As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTSAvail As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aMVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMAcc As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aTol As Double = 0.0! ' TODO: Initialize to an appropriate value
        modMetData.FillHourlyTser(aTS2Fill, aTSAvail, aMVal, aMAcc, aTol)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test GeoDist</summary>
    <TestMethod()> Public Sub GeoDistTest()
        Dim aTS1 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTS2 As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Single
        actual = modMetData.GeoDist(aTS1, aTS2)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetAdjustingAttribute</summary>
    <TestMethod()> Public Sub GetAdjustingAttributeTest()
        Dim aTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modMetData.GetAdjustingAttribute(aTSer)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test MissingDataSummary</summary>
    <TestMethod()> Public Sub MissingDataSummaryTest()
        Dim aTSer As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aMVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMAcc As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFMin As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFMax As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aRepTyp As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = modMetData.MissingDataSummary(aTSer, aMVal, aMAcc, aFMin, aFMax, aRepTyp)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NxtMis</summary>
    <TestMethod()> Public Sub NxtMisTest()
        Dim aBufPos As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aDBuff() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aValMis As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aValAcc As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFaultMin As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aFaultMax As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMisCod As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMisCodExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMisPos As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMisPosExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNVals As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aNValsExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aMVal As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim aMValExpected As Double = 0.0! ' TODO: Initialize to an appropriate value
        modMetData.NxtMis(aBufPos, aDBuff, aValMis, aValAcc, aFaultMin, aFaultMax, aMisCod, aMisPos, aNVals, aMVal)
        Assert.AreEqual(aMisCodExpected, aMisCod)
        Assert.AreEqual(aMisPosExpected, aMisPos)
        Assert.AreEqual(aNValsExpected, aNVals)
        Assert.AreEqual(aMValExpected, aMVal)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ReadNOAAAttributes</summary>
    <TestMethod()> Public Sub ReadNOAAAttributesTest()
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = modMetData.ReadNOAAAttributes(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ReadNOAAHPDAttributes</summary>
    <TestMethod()> Public Sub ReadNOAAHPDAttributesTest()
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = modMetData.ReadNOAAHPDAttributes(aFilename)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SortIntegerArray</summary>
    <TestMethod()> Public Sub SortIntegerArrayTest()
        Dim aOpt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCnt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aIVal() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aIValExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aPos() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aPosExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modMetData.SortIntegerArray(aOpt, aCnt, aIVal, aPos)
        Assert.AreEqual(aIValExpected, aIVal)
        Assert.AreEqual(aPosExpected, aPos)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SortRealArray</summary>
    <TestMethod()> Public Sub SortRealArrayTest()
        Dim aOpt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aCnt As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRVal() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aRValExpected() As Double = Nothing ' TODO: Initialize to an appropriate value
        Dim aPos() As Integer = Nothing ' TODO: Initialize to an appropriate value
        Dim aPosExpected() As Integer = Nothing ' TODO: Initialize to an appropriate value
        modMetData.SortRealArray(aOpt, aCnt, aRVal, aPos)
        Assert.AreEqual(aRValExpected, aRVal)
        Assert.AreEqual(aPosExpected, aPos)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub
End Class
