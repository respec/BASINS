Imports System.Windows.Forms

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for frmSaveDataTest and is intended
'''to contain all frmSaveDataTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSaveDataTest


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
    '''A test for frmSaveData Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmSaveDataConstructorTest()
        Dim target As frmSaveData = New frmSaveData()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AskUser
    '''</summary>
    <TestMethod()> _
    Public Sub AskUserTest()
        Dim target As frmSaveData = New frmSaveData() ' TODO: Initialize to an appropriate value
        Dim aDataGroup As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As atcDataSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataSource
        actual = target.AskUser(aDataGroup)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSaveData_Accessor = New frmSaveData_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSaveData_Accessor = New frmSaveData_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSaveData_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for lstDataSources_DoubleClick
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub lstDataSources_DoubleClickTest()
        Dim target As frmSaveData_Accessor = New frmSaveData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstDataSources_DoubleClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCancel
    '''</summary>
    <TestMethod()> _
    Public Sub btnCancelTest()
        Dim target As frmSaveData = New frmSaveData() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnOk
    '''</summary>
    <TestMethod()> _
    Public Sub btnOkTest()
        Dim target As frmSaveData = New frmSaveData() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lstDataSources
    '''</summary>
    <TestMethod()> _
    Public Sub lstDataSourcesTest()
        Dim target As frmSaveData = New frmSaveData() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstDataSources = expected
        actual = target.lstDataSources
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pnlButtons
    '''</summary>
    <TestMethod()> _
    Public Sub pnlButtonsTest()
        Dim target As frmSaveData = New frmSaveData() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.pnlButtons = expected
        actual = target.pnlButtons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
