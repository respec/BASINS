Imports System.Windows.Forms
Imports System
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcControls

'''<summary>
'''This is a test class for atcSelectListTest and is intended
'''to contain all atcSelectListTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcSelectListTest
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

    '''<summary>Test atcSelectList Constructor</summary>
    <TestMethod()> Public Sub atcSelectListConstructorTest()
        Dim target As atcSelectList = New atcSelectList()
        Assert.IsNotNull(target)
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim aAvailable As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim aSelected As List(Of String) = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AskUser(aAvailable, aSelected)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub ClearTest()
        Dim target As atcSelectList_Accessor = New atcSelectList_Accessor() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub DisposeTest()
        Dim target As atcSelectList_Accessor = New atcSelectList_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcSelectList_Accessor = New atcSelectList_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAttributesAll_Click</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub btnAttributesAll_ClickTest()
        Dim target As atcSelectList_Accessor = New atcSelectList_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAttributesAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAttributesNone_Click</summary>
    <TestMethod(), DeploymentItem("atcControls.dll")> _
    Public Sub btnAttributesNone_ClickTest()
        Dim target As atcSelectList_Accessor = New atcSelectList_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAttributesNone_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAttributesAll</summary>
    <TestMethod()> Public Sub btnAttributesAllTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAttributesAll = expected
        actual = target.btnAttributesAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnAttributesNone</summary>
    <TestMethod()> Public Sub btnAttributesNoneTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAttributesNone = expected
        actual = target.btnAttributesNone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lstAvailable</summary>
    <TestMethod()> Public Sub lstAvailableTest()
        Dim target As atcSelectList = New atcSelectList() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstAvailable = expected
        actual = target.lstAvailable
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
