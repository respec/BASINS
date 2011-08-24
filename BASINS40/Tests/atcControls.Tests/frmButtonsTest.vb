Imports System.Windows.Forms
Imports System
Imports System.ComponentModel
Imports System.Collections
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcControls

'''<summary>
'''This is a test class for frmButtonsTest and is intended
'''to contain all frmButtonsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmButtonsTest
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

    '''<summary>Test frmButtons Constructor</summary>
    <TestMethod()> Public Sub frmButtonsConstructorTest()
        Dim target As frmButtons = New frmButtons()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmButtons = New frmButtons() ' TODO: Initialize to an appropriate value
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMessage As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aButtonLabels As IEnumerable = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.AskUser(aTitle, aMessage, aButtonLabels)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest1()
        Dim target As frmButtons = New frmButtons() ' TODO: Initialize to an appropriate value
        Dim aTitle As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aMessage As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aButtonLabels() As String = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = target.AskUser(aTitle, aMessage, aButtonLabels)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub DisposeTest()
        Dim target As frmButtons_Accessor = New frmButtons_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmButtons_Accessor = New frmButtons_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test OnClosing</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub OnClosingTest()
        Dim target As frmButtons_Accessor = New frmButtons_Accessor() ' TODO: Initialize to an appropriate value
        Dim e As CancelEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.OnClosing(e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnClick</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub btnClickTest()
        Dim target As frmButtons_Accessor = New frmButtons_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test lblMessage</summary>
    <TestMethod()> Public Sub lblMessageTest()
        Dim target As frmButtons = New frmButtons() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblMessage = expected
        actual = target.lblMessage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
