Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcUtility

'''<summary>
'''This is a test class for frmArgsTest and is intended
'''to contain all frmArgsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmArgsTest
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

    '''<summary>Test frmArgs Constructor</summary>
    <TestMethod()> Public Sub frmArgsConstructorTest()
        Dim target As frmArgs = New frmArgs()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AddCheckbox</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub AddCheckboxTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim aChecked As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As CheckBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CheckBox
        actual = target.AddCheckbox(aChecked, aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AddLabel</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub AddLabelTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        actual = target.AddLabel(aText, aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AddTextbox</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub AddTextboxTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        actual = target.AddTextbox(aText, aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmArgs = New frmArgs() ' TODO: Initialize to an appropriate value
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aArgs As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim aArgsExpected As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AskUser(aTitle, aArgs)
        Assert.AreEqual(aArgsExpected, aArgs)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub DisposeTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel_Click</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcUtility.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmArgs_Accessor = New frmArgs_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As frmArgs = New frmArgs() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmArgs = New frmArgs() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
