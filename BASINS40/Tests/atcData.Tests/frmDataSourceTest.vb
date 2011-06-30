Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for frmDataSourceTest and is intended
'''to contain all frmDataSourceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmDataSourceTest
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

    '''<summary>Test frmDataSource Constructor</summary>
    <TestMethod()> Public Sub frmDataSourceConstructorTest()
        Dim target As frmDataSource = New frmDataSource()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim aSelectedSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aSelectedSourceExpected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToOpenExpected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSaveExpected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aCategories As ArrayList = Nothing ' TODO: Initialize to an appropriate value
        target.AskUser(aSelectedSource, aNeedToOpen, aNeedToSave, aCategories)
        Assert.AreEqual(aSelectedSourceExpected, aSelectedSource)
        Assert.AreEqual(aNeedToOpenExpected, aNeedToOpen)
        Assert.AreEqual(aNeedToSaveExpected, aNeedToSave)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test FindOrCreateNode</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub FindOrCreateNodeTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNodes As TreeNodeCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aNodeText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As TreeNode = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TreeNode
        actual = target.FindOrCreateNode(aNodes, aNodeText)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test GetSource</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub GetSourceTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSourceName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aOperationName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.GetSource(aSourceName, aOperationName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test NodeHeight</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub NodeHeightTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNode As TreeNode = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.NodeHeight(aNode)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OnLoad</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OnLoadTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnLoad(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Populate</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToOpenExpected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSaveExpected As Boolean = False ' TODO: Initialize to an appropriate value
        target.Populate(aNeedToOpen, aNeedToSave)
        Assert.AreEqual(aNeedToOpenExpected, aNeedToOpen)
        Assert.AreEqual(aNeedToSaveExpected, aNeedToSave)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ResizeToShowBottomNode</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub ResizeToShowBottomNodeTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        target.ResizeToShowBottomNode()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmDataSource_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeSources_AfterCollapse</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeSources_AfterCollapseTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As TreeViewEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeSources_AfterCollapse(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeSources_AfterExpand</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeSources_AfterExpandTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As TreeViewEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeSources_AfterExpand(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test treeSources_DoubleClick</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub treeSources_DoubleClickTest()
        Dim target As frmDataSource_Accessor = New frmDataSource_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.treeSources_DoubleClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboDisplay</summary>
    <TestMethod()> Public Sub cboDisplayTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboDisplay = expected
        actual = target.cboDisplay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblDisplay</summary>
    <TestMethod()> Public Sub lblDisplayTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDisplay = expected
        actual = target.lblDisplay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pnlButtons</summary>
    <TestMethod()> Public Sub pnlButtonsTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.pnlButtons = expected
        actual = target.pnlButtons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test treeSources</summary>
    <TestMethod()> Public Sub treeSourcesTest()
        Dim target As frmDataSource = New frmDataSource() ' TODO: Initialize to an appropriate value
        Dim expected As TreeView = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TreeView
        target.treeSources = expected
        actual = target.treeSources
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
