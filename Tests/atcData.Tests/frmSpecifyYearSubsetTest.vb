Imports System.Windows.Forms
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for frmSpecifyYearSubsetTest and is intended
'''to contain all frmSpecifyYearSubsetTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSpecifyYearSubsetTest
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

    '''<summary>Test frmSpecifyYearSubset Constructor</summary>
    <TestMethod()> Public Sub frmSpecifyYearSubsetConstructorTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim aStartMonth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartMonthExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartDay As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aStartDayExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndMonth As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndMonthExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndDay As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aEndDayExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AskUser(aStartMonth, aStartDay, aEndMonth, aEndDay)
        Assert.AreEqual(aStartMonthExpected, aStartMonth)
        Assert.AreEqual(aStartDayExpected, aStartDay)
        Assert.AreEqual(aEndMonthExpected, aEndMonth)
        Assert.AreEqual(aEndDayExpected, aEndDay)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSpecifyYearSubset_Accessor = New frmSpecifyYearSubset_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSpecifyYearSubset_Accessor = New frmSpecifyYearSubset_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSpecifyYearSubset_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmSpecifyYearSubset_Accessor = New frmSpecifyYearSubset_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Label1</summary>
    <TestMethod()> Public Sub Label1Test()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.Label1 = expected
        actual = target.Label1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Label2</summary>
    <TestMethod()> Public Sub Label2Test()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.Label2 = expected
        actual = target.Label2
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboEndMonth</summary>
    <TestMethod()> Public Sub cboEndMonthTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboEndMonth = expected
        actual = target.cboEndMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboStartMonth</summary>
    <TestMethod()> Public Sub cboStartMonthTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboStartMonth = expected
        actual = target.cboStartMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtEndDay</summary>
    <TestMethod()> Public Sub txtEndDayTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtEndDay = expected
        actual = target.txtEndDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtStartDay</summary>
    <TestMethod()> Public Sub txtStartDayTest()
        Dim target As frmSpecifyYearSubset = New frmSpecifyYearSubset() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtStartDay = expected
        actual = target.txtStartDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
