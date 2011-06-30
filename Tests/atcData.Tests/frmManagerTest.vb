Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for frmManagerTest and is intended
'''to contain all frmManagerTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmManagerTest
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

    '''<summary>Test frmManager Constructor</summary>
    <TestMethod()> Public Sub frmManagerConstructorTest()
        Dim target As frmManager = New frmManager()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AddDatasetsToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub AddDatasetsToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.AddDatasetsToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AnalysisToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub AnalysisToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.AnalysisToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ChangedData</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub ChangedDataTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        target.ChangedData(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CloseAllToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub CloseAllToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CloseAllToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CloseToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub CloseToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.CloseToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DisplayToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisplayToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.DisplayToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DoAction</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DoActionTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAction As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aMultipleSelected As Boolean = False ' TODO: Initialize to an appropriate value
        target.DoAction(aAction, aDataSource, aMultipleSelected)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test DoDisplay</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DoDisplayTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        target.DoDisplay()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Edit</summary>
    <TestMethod()> Public Sub EditTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim aNodeKey As Integer = 0 ' TODO: Initialize to an appropriate value
        target.Edit(aNodeKey)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ExitToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub ExitToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.ExitToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Form_DragDrop</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub Form_DragDropTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As DragEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.Form_DragDrop(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Form_DragEnter</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub Form_DragEnterTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As DragEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.Form_DragEnter(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test HelpToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub HelpToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.HelpToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test NativeToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub NativeToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.NativeToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test NewToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub NewToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.NewToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test NodeBounds</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub NodeBoundsTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As TreeNode = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Rectangle = New Rectangle() ' TODO: Initialize to an appropriate value
        Dim actual As Rectangle
        actual = target.NodeBounds(e)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OnAfterSelect</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OnAfterSelectTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As TreeViewEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnAfterSelect(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test OnBeforeSelect</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OnBeforeSelectTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As TreeViewCancelEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnBeforeSelect(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test OnClosing</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OnClosingTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As CancelEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnClosing(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test OpenToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OpenToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OpenToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Populate</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNodeKey As Integer = 0 ' TODO: Initialize to an appropriate value
        target.Populate(aNodeKey)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RefreshDetails</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RefreshDetailsTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSourceIndex As Integer = 0 ' TODO: Initialize to an appropriate value
        target.RefreshDetails(aDataSourceIndex)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveDatasetsToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemoveDatasetsToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.RemoveDatasetsToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RenumberDatasetsToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RenumberDatasetsToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.RenumberDatasetsToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SaveInHandler</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SaveInHandlerTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.SaveInHandler(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SaveInToolStripMenuItem_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SaveInToolStripMenuItem_ClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.SaveInToolStripMenuItem_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SelectedSources</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SelectedSourcesTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As List(Of atcTimeseriesSource) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of atcTimeseriesSource)
        actual = target.SelectedSources
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectedTimeseries</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SelectedTimeseriesTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.SelectedTimeseries
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectionAction</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SelectionActionTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAction As String = String.Empty ' TODO: Initialize to an appropriate value
        target.SelectionAction(aAction)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmManager_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test frmManager_KeyDown</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub frmManager_KeyDownTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmManager_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test isParent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub isParentTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim aCheckParent As TreeNode = Nothing ' TODO: Initialize to an appropriate value
        Dim aDescendant As TreeNode = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.isParent(aCheckParent, aDescendant)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test paintSelectedNodes</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub paintSelectedNodesTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        target.paintSelectedNodes()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test removePaintFromNodes</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub removePaintFromNodesTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        target.removePaintFromNodes()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeFiles_AfterSelect</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeFiles_AfterSelectTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As TreeViewEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeFiles_AfterSelect(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeFiles_DoubleClick</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeFiles_DoubleClickTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeFiles_DoubleClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeFiles_DrawNode</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeFiles_DrawNodeTest()
        Dim target As frmManager_Accessor = New frmManager_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As DrawTreeNodeEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeFiles_DrawNode(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AddDatasetsToolStripMenuItem</summary>
    <TestMethod()> Public Sub AddDatasetsToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.AddDatasetsToolStripMenuItem = expected
        actual = target.AddDatasetsToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AnalysisToolStripMenuItem</summary>
    <TestMethod()> Public Sub AnalysisToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.AnalysisToolStripMenuItem = expected
        actual = target.AnalysisToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CloseAllToolStripMenuItem</summary>
    <TestMethod()> Public Sub CloseAllToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.CloseAllToolStripMenuItem = expected
        actual = target.CloseAllToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CloseToolStripMenuItem</summary>
    <TestMethod()> Public Sub CloseToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.CloseToolStripMenuItem = expected
        actual = target.CloseToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DisplayToolStripMenuItem</summary>
    <TestMethod()> Public Sub DisplayToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.DisplayToolStripMenuItem = expected
        actual = target.DisplayToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test EditToolStripMenuItem</summary>
    <TestMethod()> Public Sub EditToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.EditToolStripMenuItem = expected
        actual = target.EditToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ExitToolStripMenuItem</summary>
    <TestMethod()> Public Sub ExitToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.ExitToolStripMenuItem = expected
        actual = target.ExitToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FileToolStripMenuItem</summary>
    <TestMethod()> Public Sub FileToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.FileToolStripMenuItem = expected
        actual = target.FileToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test HelpToolStripMenuItem</summary>
    <TestMethod()> Public Sub HelpToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.HelpToolStripMenuItem = expected
        actual = target.HelpToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test MenuStrip1</summary>
    <TestMethod()> Public Sub MenuStrip1Test()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As MenuStrip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As MenuStrip
        target.MenuStrip1 = expected
        actual = target.MenuStrip1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NativeToolStripMenuItem</summary>
    <TestMethod()> Public Sub NativeToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.NativeToolStripMenuItem = expected
        actual = target.NativeToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test NewToolStripMenuItem</summary>
    <TestMethod()> Public Sub NewToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.NewToolStripMenuItem = expected
        actual = target.NewToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OpenToolStripMenuItem</summary>
    <TestMethod()> Public Sub OpenToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.OpenToolStripMenuItem = expected
        actual = target.OpenToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test RemoveDatasetsToolStripMenuItem</summary>
    <TestMethod()> Public Sub RemoveDatasetsToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.RemoveDatasetsToolStripMenuItem = expected
        actual = target.RemoveDatasetsToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test RenumberDatasetsToolStripMenuItem</summary>
    <TestMethod()> Public Sub RenumberDatasetsToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.RenumberDatasetsToolStripMenuItem = expected
        actual = target.RenumberDatasetsToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SaveInToolStripMenuItem</summary>
    <TestMethod()> Public Sub SaveInToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.SaveInToolStripMenuItem = expected
        actual = target.SaveInToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectedNodes</summary>
    <TestMethod()> Public Sub SelectedNodesTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ArrayList
        target.SelectedNodes = expected
        actual = target.SelectedNodes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ViewToolStripMenuItem</summary>
    <TestMethod()> Public Sub ViewToolStripMenuItemTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.ViewToolStripMenuItem = expected
        actual = target.ViewToolStripMenuItem
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test toolStripSeparator</summary>
    <TestMethod()> Public Sub toolStripSeparatorTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripSeparator = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripSeparator
        target.toolStripSeparator = expected
        actual = target.toolStripSeparator
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test toolStripSeparator1</summary>
    <TestMethod()> Public Sub toolStripSeparator1Test()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripSeparator = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripSeparator
        target.toolStripSeparator1 = expected
        actual = target.toolStripSeparator1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test toolStripSeparator2</summary>
    <TestMethod()> Public Sub toolStripSeparator2Test()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripSeparator = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripSeparator
        target.toolStripSeparator2 = expected
        actual = target.toolStripSeparator2
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test treeFiles</summary>
    <TestMethod()> Public Sub treeFilesTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As TreeView = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TreeView
        target.treeFiles = expected
        actual = target.treeFiles
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtDetails</summary>
    <TestMethod()> Public Sub txtDetailsTest()
        Dim target As frmManager = New frmManager() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtDetails = expected
        actual = target.txtDetails
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
