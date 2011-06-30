Imports System.Windows.Forms

Imports atcUtility

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcControls



'''<summary>
'''This is a test class for frmConnectFieldsTest and is intended
'''to contain all frmConnectFieldsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmConnectFieldsTest


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
    '''A test for frmConnectFields Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmConnectFieldsConstructorTest()
        Dim target As frmConnectFields = New frmConnectFields()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Connections
    '''</summary>
    <TestMethod()> _
    Public Sub ConnectionsTest()
        Dim target As frmConnectFields = New frmConnectFields() ' TODO: Initialize to an appropriate value
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
        Dim target As frmConnectFields_Accessor = New frmConnectFields_Accessor() ' TODO: Initialize to an appropriate value
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
        Dim target As frmConnectFields_Accessor = New frmConnectFields_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcControls.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmConnectFields_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for cmdCancel
    '''</summary>
    <TestMethod()> _
    Public Sub cmdCancelTest()
        Dim target As frmConnectFields = New frmConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.cmdCancel = expected
        actual = target.cmdCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cmdOK
    '''</summary>
    <TestMethod()> _
    Public Sub cmdOKTest()
        Dim target As frmConnectFields = New frmConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.cmdOK = expected
        actual = target.cmdOK
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ctlConnectFields
    '''</summary>
    <TestMethod()> _
    Public Sub ctlConnectFieldsTest()
        Dim target As frmConnectFields = New frmConnectFields() ' TODO: Initialize to an appropriate value
        Dim expected As atcConnectFields = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcConnectFields
        target.ctlConnectFields = expected
        actual = target.ctlConnectFields
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
