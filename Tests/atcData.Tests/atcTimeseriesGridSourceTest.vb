Imports System.Drawing
Imports atcControls
Imports atcUtility
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcTimeseriesGridSourceTest and is intended
'''to contain all atcTimeseriesGridSourceTest Unit Tests (User control, not tested)
'''</summary>
<TestClass()> _
Public Class atcTimeseriesGridSourceTest
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

    '''<summary>Test atcTimeseriesGridSource Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGridSourceConstructorTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test atcTimeseriesGridSource Constructor</summary>
    <TestMethod()> Public Sub atcTimeseriesGridSourceConstructorTest1()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aFilterNoData As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues, aFilterNoData)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test CellDataset</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub CellDatasetTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource_Accessor = New atcTimeseriesGridSource_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTimeseries As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aTimeseriesExpected As atcTimeseries = Nothing ' TODO: Initialize to an appropriate value
        Dim aIsValue As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aIsValueExpected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aValueAttDef As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        Dim aValueAttDefExpected As atcAttributeDefinition = Nothing ' TODO: Initialize to an appropriate value
        target.CellDataset(aColumn, aTimeseries, aIsValue, aValueAttDef)
        Assert.AreEqual(aTimeseriesExpected, aTimeseries)
        Assert.AreEqual(aIsValueExpected, aIsValue)
        Assert.AreEqual(aValueAttDefExpected, aValueAttDef)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RefreshAllDates</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RefreshAllDatesTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource_Accessor = New atcTimeseriesGridSource_Accessor(param0) ' TODO: Initialize to an appropriate value
        target.RefreshAllDates()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ValueFormat</summary>
    <TestMethod()> Public Sub ValueFormatTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim aMaxWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aExpFormat As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aCantFit As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSignificantDigits As Integer = 0 ' TODO: Initialize to an appropriate value
        target.ValueFormat(aMaxWidth, aFormat, aExpFormat, aCantFit, aSignificantDigits)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcTimeseriesGridSource_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pDataGroup_Added</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pDataGroup_AddedTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource_Accessor = New atcTimeseriesGridSource_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim aAdded As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pDataGroup_Added(aAdded)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pDataGroup_Removed</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pDataGroup_RemovedTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource_Accessor = New atcTimeseriesGridSource_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim aRemoved As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pDataGroup_Removed(aRemoved)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Alignment</summary>
    <TestMethod()> Public Sub AlignmentTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcAlignment = New atcAlignment() ' TODO: Initialize to an appropriate value
        Dim actual As atcAlignment
        target.Alignment(aRow, aColumn) = expected
        actual = target.Alignment(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CellColor</summary>
    <TestMethod()> Public Sub CellColorTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.CellColor(aRow, aColumn) = expected
        actual = target.CellColor(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CellEditable</summary>
    <TestMethod()> Public Sub CellEditableTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.CellEditable(aRow, aColumn) = expected
        actual = target.CellEditable(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CellValue</summary>
    <TestMethod()> Public Sub CellValueTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.CellValue(aRow, aColumn) = expected
        actual = target.CellValue(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Columns</summary>
    <TestMethod()> Public Sub ColumnsTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Columns = expected
        actual = target.Columns
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DateFormat</summary>
    <TestMethod()> Public Sub DateFormatTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim expected As atcDateFormat = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDateFormat
        target.DateFormat = expected
        actual = target.DateFormat
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DisplayValueAttributes</summary>
    <TestMethod()> Public Sub DisplayValueAttributesTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.DisplayValueAttributes = expected
        actual = target.DisplayValueAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FixedRows</summary>
    <TestMethod()> Public Sub FixedRowsTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FixedRows = expected
        actual = target.FixedRows
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Rows</summary>
    <TestMethod()> Public Sub RowsTest()
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayAttributes As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aDisplayValues As Boolean = False ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource = New atcTimeseriesGridSource(aDataGroup, aDisplayAttributes, aDisplayValues) ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Rows = expected
        actual = target.Rows
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pTimeseriesGroup</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pTimeseriesGroupTest()
        Dim param0 As PrivateObject = Nothing ' TODO: Initialize to an appropriate value
        Dim target As atcTimeseriesGridSource_Accessor = New atcTimeseriesGridSource_Accessor(param0) ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pTimeseriesGroup = expected
        actual = target.pTimeseriesGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
