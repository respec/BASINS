Imports atcControls

Imports System.Windows.Forms

Imports System

Imports atcData

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcWDM



'''<summary>
'''This is a test class for frmSaveTest and is intended
'''to contain all frmSaveTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSaveTest


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
    '''A test for frmSave Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmSaveConstructorTest()
        Dim target As frmSave = New frmSave()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AskUser
    '''</summary>
    <TestMethod()> _
    Public Sub AskUserTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aLabel As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aHighestDSN As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.AskUser(aDataGroup, aLabel, aHighestDSN)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSave_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCancel_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSave_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub btnSave_ClickTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSave_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSelectAttributes_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub btnSelectAttributes_ClickTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSelectAttributes_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for frmSave_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcWDM.dll")> _
    Public Sub frmSave_KeyDownTest()
        Dim target As frmSave_Accessor = New frmSave_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSave_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for agdData
    '''</summary>
    <TestMethod()> _
    Public Sub agdDataTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGrid
        target.agdData = expected
        actual = target.agdData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnCancel
    '''</summary>
    <TestMethod()> _
    Public Sub btnCancelTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSave
    '''</summary>
    <TestMethod()> _
    Public Sub btnSaveTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSave = expected
        actual = target.btnSave
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSelectAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub btnSelectAttributesTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSelectAttributes = expected
        actual = target.btnSelectAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblInstructions
    '''</summary>
    <TestMethod()> _
    Public Sub lblInstructionsTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblInstructions = expected
        actual = target.lblInstructions
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblStatus
    '''</summary>
    <TestMethod()> _
    Public Sub lblStatusTest()
        Dim target As frmSave = New frmSave() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblStatus = expected
        actual = target.lblStatus
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
