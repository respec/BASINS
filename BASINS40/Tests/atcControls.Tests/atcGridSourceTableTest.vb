Imports atcUtility

Imports System.Drawing

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcControls



'''<summary>
'''This is a test class for atcGridSourceTableTest and is intended
'''to contain all atcGridSourceTableTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcGridSourceTableTest


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
    '''A test for atcGridSourceTable Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcGridSourceTableConstructorTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for ExpandRowsColumns
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ExpandRowsColumnsTest()
        Dim target As atcGridSourceTable_Accessor = New atcGridSourceTable_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.ExpandRowsColumns(aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcGridSourceTable_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Alignment
    '''</summary>
    <TestMethod()> _
    Public Sub AlignmentTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcAlignment = New atcAlignment() ' TODO: Initialize to an appropriate value
        Dim actual As atcAlignment
        target.Alignment(aRow, aColumn) = expected
        actual = target.Alignment(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellColor
    '''</summary>
    <TestMethod()> _
    Public Sub CellColorTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.CellColor(aRow, aColumn) = expected
        actual = target.CellColor(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellEditable
    '''</summary>
    <TestMethod()> _
    Public Sub CellEditableTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.CellEditable(aRow, aColumn) = expected
        actual = target.CellEditable(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellSelected
    '''</summary>
    <TestMethod()> _
    Public Sub CellSelectedTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.CellSelected(aRow, aColumn) = expected
        actual = target.CellSelected(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellValue
    '''</summary>
    <TestMethod()> _
    Public Sub CellValueTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.CellValue(aRow, aColumn) = expected
        actual = target.CellValue(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Columns
    '''</summary>
    <TestMethod()> _
    Public Sub ColumnsTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Columns = expected
        actual = target.Columns
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FixedColumns
    '''</summary>
    <TestMethod()> _
    Public Sub FixedColumnsTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FixedColumns = expected
        actual = target.FixedColumns
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for FixedRows
    '''</summary>
    <TestMethod()> _
    Public Sub FixedRowsTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.FixedRows = expected
        actual = target.FixedRows
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Rows
    '''</summary>
    <TestMethod()> _
    Public Sub RowsTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.Rows = expected
        actual = target.Rows
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Table
    '''</summary>
    <TestMethod()> _
    Public Sub TableTest()
        Dim target As atcGridSourceTable = New atcGridSourceTable() ' TODO: Initialize to an appropriate value
        Dim expected As atcTable = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTable
        target.Table = expected
        actual = target.Table
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
