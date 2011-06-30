Imports System.Windows.Forms

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcUtility



'''<summary>
'''This is a test class for frmFeedbackTest and is intended
'''to contain all frmFeedbackTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmFeedbackTest


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
    '''A test for frmFeedback Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmFeedbackConstructorTest()
        Dim target As frmFeedback = New frmFeedback()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub DisposeTest()
        Dim target As frmFeedback_Accessor = New frmFeedback_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetDebugInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetDebugInfoTest()
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = frmFeedback.GetDebugInfo
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetModuleInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetModuleInfoTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetModuleInfo
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetSystemInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetSystemInfoTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.GetSystemInfo
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmFeedback_Accessor = New frmFeedback_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ShowFeedback
    '''</summary>
    <TestMethod()> _
    Public Sub ShowFeedbackTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aNameExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aEmail As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aEmailExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMessage As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMessageExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSystemInformation As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aSystemInformationExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aAddGenericSystemInfo As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aAddDebugInfo As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aAddModuleInfo As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aAddFilesInFolder As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.ShowFeedback(aName, aEmail, aMessage, aSystemInformation, aAddGenericSystemInfo, aAddDebugInfo, aAddModuleInfo, aAddFilesInFolder)
        Assert.AreEqual(aNameExpected, aName)
        Assert.AreEqual(aEmailExpected, aEmail)
        Assert.AreEqual(aMessageExpected, aMessage)
        Assert.AreEqual(aSystemInformationExpected, aSystemInformation)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmFeedback_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCancel_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmFeedback_Accessor = New frmFeedback_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCopy_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub btnCopy_ClickTest()
        Dim target As frmFeedback_Accessor = New frmFeedback_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCopy_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSend_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcUtility.dll")> _
    Public Sub btnSend_ClickTest()
        Dim target As frmFeedback_Accessor = New frmFeedback_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSend_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCancel
    '''</summary>
    <TestMethod()> _
    Public Sub btnCancelTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnCopy
    '''</summary>
    <TestMethod()> _
    Public Sub btnCopyTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCopy = expected
        actual = target.btnCopy
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSend
    '''</summary>
    <TestMethod()> _
    Public Sub btnSendTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSend = expected
        actual = target.btnSend
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for chkSendSystemInformation
    '''</summary>
    <TestMethod()> _
    Public Sub chkSendSystemInformationTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As CheckBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CheckBox
        target.chkSendSystemInformation = expected
        actual = target.chkSendSystemInformation
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblEmail
    '''</summary>
    <TestMethod()> _
    Public Sub lblEmailTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblEmail = expected
        actual = target.lblEmail
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblEnterAmessage
    '''</summary>
    <TestMethod()> _
    Public Sub lblEnterAmessageTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblEnterAmessage = expected
        actual = target.lblEnterAmessage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblName
    '''</summary>
    <TestMethod()> _
    Public Sub lblNameTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblName = expected
        actual = target.lblName
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtEmail
    '''</summary>
    <TestMethod()> _
    Public Sub txtEmailTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtEmail = expected
        actual = target.txtEmail
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtMessage
    '''</summary>
    <TestMethod()> _
    Public Sub txtMessageTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtMessage = expected
        actual = target.txtMessage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtName
    '''</summary>
    <TestMethod()> _
    Public Sub txtNameTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtName = expected
        actual = target.txtName
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtSystemInformation
    '''</summary>
    <TestMethod()> _
    Public Sub txtSystemInformationTest()
        Dim target As frmFeedback = New frmFeedback() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtSystemInformation = expected
        actual = target.txtSystemInformation
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
