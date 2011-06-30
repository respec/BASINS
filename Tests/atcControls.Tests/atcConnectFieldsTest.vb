Imports System.Windows.Forms

Imports System

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcControls



'''<summary>
'''This is a test class for atcConnectFieldsTest and is intended
'''to contain all atcConnectFieldsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcConnectFieldsTest


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
    '''A test for atcConnectFields Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcConnectFieldsConstructorTest()
        Dim target As atcConnectFields = New atcConnectFields()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddConnection
    '''</summary>
    <TestMethod()> _
    Public Sub AddConnectionTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim aConnectionString As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aQuiet As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AddConnection(aConnectionString, aQuiet)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Connections
    '''</summary>
    <TestMethod()> _
    Public Sub ConnectionsTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcCollection
        actual = target.Connections
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub DisposeTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for LoadConnections
    '''</summary>
    <TestMethod()> _
    Public Sub LoadConnectionsTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim aFileName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.LoadConnections(aFileName)
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
        atcConnectFields_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for atcConnectFields_Resize
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub atcConnectFields_ResizeTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.atcConnectFields_Resize(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnAdd_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub btnAdd_ClickTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAdd_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnClear_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub btnClear_ClickTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnClear_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnDelete_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub btnDelete_ClickTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnDelete_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnLoad_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub btnLoad_ClickTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnLoad_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSave_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub btnSave_ClickTest()
        Dim target As atcConnectFields_Accessor = New atcConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSave_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnAdd
    '''</summary>
    <TestMethod()> _
    Public Sub btnAddTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAdd = expected
        actual = target.btnAdd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnClear
    '''</summary>
    <TestMethod()> _
    Public Sub btnClearTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnClear = expected
        actual = target.btnClear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnDelete
    '''</summary>
    <TestMethod()> _
    Public Sub btnDeleteTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnDelete = expected
        actual = target.btnDelete
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnLoad
    '''</summary>
    <TestMethod()> _
    Public Sub btnLoadTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnLoad = expected
        actual = target.btnLoad
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSave
    '''</summary>
    <TestMethod()> _
    Public Sub btnSaveTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSave = expected
        actual = target.btnSave
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblConnections
    '''</summary>
    <TestMethod()> _
    Public Sub lblConnectionsTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblConnections = expected
        actual = target.lblConnections
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblHeader
    '''</summary>
    <TestMethod()> _
    Public Sub lblHeaderTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblHeader = expected
        actual = target.lblHeader
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblSource
    '''</summary>
    <TestMethod()> _
    Public Sub lblSourceTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblSource = expected
        actual = target.lblSource
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblTarget
    '''</summary>
    <TestMethod()> _
    Public Sub lblTargetTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblTarget = expected
        actual = target.lblTarget
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lstConnections
    '''</summary>
    <TestMethod()> _
    Public Sub lstConnectionsTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstConnections = expected
        actual = target.lstConnections
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lstSource
    '''</summary>
    <TestMethod()> _
    Public Sub lstSourceTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstSource = expected
        actual = target.lstSource
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lstTarget
    '''</summary>
    <TestMethod()> _
    Public Sub lstTargetTest()
        Dim target As atcConnectFields = New atcConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstTarget = expected
        actual = target.lstTarget
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
