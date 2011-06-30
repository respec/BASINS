Imports System.Collections

Imports System

Imports System.Drawing

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcControls



'''<summary>
'''This is a test class for atcGridTest and is intended
'''to contain all atcGridTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcGridTest


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
    '''A test for atcGrid Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcGridConstructorTest()
        Dim target As atcGrid = New atcGrid()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for ATCgrid_Paint
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ATCgrid_PaintTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As PaintEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.ATCgrid_Paint(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CellBounds
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CellBoundsTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Rectangle = New Rectangle() ' TODO: Initialize to an appropriate value
        Dim actual As Rectangle
        actual = target.CellBounds(aRow, aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellComboBox_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CellComboBox_KeyDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CellComboBox_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CellComboBox_LostFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CellComboBox_LostFocusTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CellComboBox_LostFocus(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CellEditBox_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CellEditBox_KeyDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CellEditBox_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CellEditBox_LostFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CellEditBox_LostFocusTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CellEditBox_LostFocus(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ChangeEditingValues
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ChangeEditingValuesTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNewValue As String = String.Empty ' TODO: Initialize to an appropriate value
        target.ChangeEditingValues(aNewValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Clear
    '''</summary>
    <TestMethod()> _
    Public Sub ClearTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ColumnDecimalToDrag
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ColumnDecimalToDragTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim X As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim Y As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.ColumnDecimalToDrag(X, Y)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ColumnEdgeToDrag
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ColumnEdgeToDragTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim X As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.ColumnEdgeToDrag(X)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ComputeCurrentRowColumn
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub ComputeCurrentRowColumnTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aRowExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumnExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        target.ComputeCurrentRowColumn(e, aRow, aColumn)
        Assert.AreEqual(aRowExpected, aRow)
        Assert.AreEqual(aColumnExpected, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CopyAllToolStripMenuItem_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CopyAllToolStripMenuItem_ClickTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CopyAllToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CopyToClipboard
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CopyToClipboardTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aCopyAll As Boolean = False ' TODO: Initialize to an appropriate value
        target.CopyToClipboard(aCopyAll)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CopyToolStripMenuItem_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub CopyToolStripMenuItem_ClickTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CopyToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub DisposeTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for EditCell
    '''</summary>
    <TestMethod()> _
    Public Sub EditCellTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aOverrideEditable As Boolean = False ' TODO: Initialize to an appropriate value
        target.EditCell(aRow, aColumn, aOverrideEditable)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for EditCellFinished
    '''</summary>
    <TestMethod()> _
    Public Sub EditCellFinishedTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        target.EditCellFinished()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for EnsureRowVisible
    '''</summary>
    <TestMethod()> _
    Public Sub EnsureRowVisibleTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        target.EnsureRowVisible(aRow)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetPreferredSize
    '''</summary>
    <TestMethod()> _
    Public Sub GetPreferredSizeTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aProposedSize As Size = New Size() ' TODO: Initialize to an appropriate value
        Dim expected As Size = New Size() ' TODO: Initialize to an appropriate value
        Dim actual As Size
        actual = target.GetPreferredSize(aProposedSize)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for HScroll_ValueChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub HScroll_ValueChangedTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.HScroll_ValueChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for HScroller_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub HScroller_KeyDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.HScroller_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for HScroller_MouseEnter
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub HScroller_MouseEnterTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.HScroller_MouseEnter(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Initialize
    '''</summary>
    <TestMethod()> _
    Public Sub InitializeTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aSource As atcGridSource = Nothing ' TODO: Initialize to an appropriate value
        target.Initialize(aSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for MoveToCell
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub MoveToCellTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.MoveToCell(aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OnDoubleClick
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub OnDoubleClickTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnDoubleClick(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OnMouseDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub OnMouseDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnMouseDown(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OnMouseMove
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub OnMouseMoveTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnMouseMove(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OnMouseUp
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub OnMouseUpTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnMouseUp(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OnMouseWheel
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub OnMouseWheelTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnMouseWheel(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for PasteFromClipboard
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub PasteFromClipboardTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        target.PasteFromClipboard()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for PasteToolStripMenuItem_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub PasteToolStripMenuItem_ClickTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.PasteToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Render
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub RenderTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim g As Graphics = Nothing ' TODO: Initialize to an appropriate value
        target.Render(g)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetEndSelectedRange
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SetEndSelectedRangeTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.SetEndSelectedRange(aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetHScroller
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SetHScrollerTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.SetHScroller
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SetStartSelectedRange
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SetStartSelectedRangeTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.SetStartSelectedRange(aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SizeAllColumnsToContents
    '''</summary>
    <TestMethod()> _
    Public Sub SizeAllColumnsToContentsTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aTotalWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aShrinkToTotalWidth As Boolean = False ' TODO: Initialize to an appropriate value
        target.SizeAllColumnsToContents(aTotalWidth, aShrinkToTotalWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SizeColumnToContents
    '''</summary>
    <TestMethod()> _
    Public Sub SizeColumnToContentsTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.SizeColumnToContents(aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SizeColumnToString
    '''</summary>
    <TestMethod()> _
    Public Sub SizeColumnToStringTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aString As String = String.Empty ' TODO: Initialize to an appropriate value
        target.SizeColumnToString(aColumn, aString)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SizeScrollers
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub SizeScrollersTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        target.SizeScrollers()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ToString
    '''</summary>
    <TestMethod()> _
    Public Sub ToStringTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.ToString
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for VScroll_ValueChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub VScroll_ValueChangedTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.VScroll_ValueChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for VScroller_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub VScroller_KeyDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.VScroller_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for VScroller_MouseEnter
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub VScroller_MouseEnterTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.VScroller_MouseEnter(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for VScroller_MouseWheel
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub VScroller_MouseWheelTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As MouseEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.VScroller_MouseWheel(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for WidthLeftOfDecimal
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub WidthLeftOfDecimalTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim g As Graphics = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.WidthLeftOfDecimal(aText, g)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for WidthRightOfDecimal
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub WidthRightOfDecimalTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim g As Graphics = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.WidthRightOfDecimal(aText, g)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcGrid_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for atcGrid_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub atcGrid_KeyDownTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.atcGrid_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for atcGrid_Resize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub atcGrid_ResizeTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.atcGrid_Resize(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSource_ChangedColumns
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub pSource_ChangedColumnsTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aColumns As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSource_ChangedColumns(aColumns)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSource_ChangedRows
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub pSource_ChangedRowsTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRows As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSource_ChangedRows(aRows)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSource_ChangedValue
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub pSource_ChangedValueTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSource_ChangedValue(aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AllowHorizontalScrolling
    '''</summary>
    <TestMethod()> _
    Public Sub AllowHorizontalScrollingTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.AllowHorizontalScrolling = expected
        actual = target.AllowHorizontalScrolling
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for AllowNewValidValues
    '''</summary>
    <TestMethod()> _
    Public Sub AllowNewValidValuesTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.AllowNewValidValues = expected
        actual = target.AllowNewValidValues
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellBackColor
    '''</summary>
    <TestMethod()> _
    Public Sub CellBackColorTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.CellBackColor = expected
        actual = target.CellBackColor
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellComboBox
    '''</summary>
    <TestMethod()> _
    Public Sub CellComboBoxTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.CellComboBox = expected
        actual = target.CellComboBox
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CellEditBox
    '''</summary>
    <TestMethod()> _
    Public Sub CellEditBoxTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.CellEditBox = expected
        actual = target.CellEditBox
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ColumnWidth
    '''</summary>
    <TestMethod()> _
    Public Sub ColumnWidthTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.ColumnWidth(aColumn) = expected
        actual = target.ColumnWidth(aColumn)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CopyAllToolStripMenuItem
    '''</summary>
    <TestMethod()> _
    Public Sub CopyAllToolStripMenuItemTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.CopyAllToolStripMenuItem = expected
        actual = target.CopyAllToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CopyToolStripMenuItem
    '''</summary>
    <TestMethod()> _
    Public Sub CopyToolStripMenuItemTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.CopyToolStripMenuItem = expected
        actual = target.CopyToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Fixed3D
    '''</summary>
    <TestMethod()> _
    Public Sub Fixed3DTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.Fixed3D = expected
        actual = target.Fixed3D
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Font
    '''</summary>
    <TestMethod()> _
    Public Sub FontTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Font = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Font
        target.Font = expected
        actual = target.Font
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GridContextMenuStrip
    '''</summary>
    <TestMethod()> _
    Public Sub GridContextMenuStripTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ContextMenuStrip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ContextMenuStrip
        target.GridContextMenuStrip = expected
        actual = target.GridContextMenuStrip
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for HScroller
    '''</summary>
    <TestMethod()> _
    Public Sub HScrollerTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As HScrollBar = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As HScrollBar
        target.HScroller = expected
        actual = target.HScroller
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for LineColor
    '''</summary>
    <TestMethod()> _
    Public Sub LineColorTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Color = New Color() ' TODO: Initialize to an appropriate value
        Dim actual As Color
        target.LineColor = expected
        actual = target.LineColor
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for LineWidth
    '''</summary>
    <TestMethod()> _
    Public Sub LineWidthTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Single = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Single
        target.LineWidth = expected
        actual = target.LineWidth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for PasteToolStripMenuItem
    '''</summary>
    <TestMethod()> _
    Public Sub PasteToolStripMenuItemTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.PasteToolStripMenuItem = expected
        actual = target.PasteToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for RowHeight
    '''</summary>
    <TestMethod()> _
    Public Sub RowHeightTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.RowHeight(aRow) = expected
        actual = target.RowHeight(aRow)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Source
    '''</summary>
    <TestMethod()> _
    Public Sub SourceTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As atcGridSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGridSource
        target.Source = expected
        actual = target.Source
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for VScroller
    '''</summary>
    <TestMethod()> _
    Public Sub VScrollerTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As VScrollBar = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As VScrollBar
        target.VScroller = expected
        actual = target.VScroller
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ValidValues
    '''</summary>
    <TestMethod()> _
    Public Sub ValidValuesTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As ICollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ICollection
        target.ValidValues = expected
        actual = target.ValidValues
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pSource
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub pSourceTest()
        Dim target As atcGrid_Accessor = New atcGrid_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcGridSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGridSource
        target.pSource = expected
        actual = target.pSource
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for scrollCorner
    '''</summary>
    <TestMethod()> _
    Public Sub scrollCornerTest()
        Dim target As atcGrid = New atcGrid() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.scrollCorner = expected
        actual = target.scrollCorner
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
