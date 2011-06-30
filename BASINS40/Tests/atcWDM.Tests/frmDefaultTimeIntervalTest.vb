Imports atcControls
Imports System.Windows.Forms
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcWDM

'''<summary>
'''This is a test class for frmDefaultTimeIntervalTest and is intended
'''to contain all frmDefaultTimeIntervalTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmDefaultTimeIntervalTest
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

    '''<summary>Test frmDefaultTimeInterval Constructor</summary>
    <TestMethod()> Public Sub frmDefaultTimeIntervalConstructorTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim aDataSetDescription As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aTu As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTuExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTs As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aTsExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAggr As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aAggrExpected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AskUser(aDataSetDescription, aTu, aTs, aAggr)
        Assert.AreEqual(aTuExpected, aTu)
        Assert.AreEqual(aTsExpected, aTs)
        Assert.AreEqual(aAggrExpected, aAggr)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub DisposeTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmDefaultTimeInterval_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAll_Click</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub btnAll_ClickTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnSkipAll_Click</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub btnSkipAll_ClickTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSkipAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnSkip_Click</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub btnSkip_ClickTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSkip_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test frmDefaultTimeInterval_KeyDown</summary>
    <TestMethod(), DeploymentItem("atcWDM.dll")> _
    Public Sub frmDefaultTimeInterval_KeyDownTest()
        Dim target As frmDefaultTimeInterval_Accessor = New frmDefaultTimeInterval_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmDefaultTimeInterval_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test atcTextTimeStep</summary>
    <TestMethod()> Public Sub atcTextTimeStepTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As atcText = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcText
        target.atcTextTimeStep = expected
        actual = target.atcTextTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnAll</summary>
    <TestMethod()> Public Sub btnAllTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAll = expected
        actual = target.btnAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnSkip</summary>
    <TestMethod()> Public Sub btnSkipTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSkip = expected
        actual = target.btnSkip
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnSkipAll</summary>
    <TestMethod()> Public Sub btnSkipAllTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSkipAll = expected
        actual = target.btnSkipAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblAggregation</summary>
    <TestMethod()> Public Sub lblAggregationTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblAggregation = expected
        actual = target.lblAggregation
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblInstructions</summary>
    <TestMethod()> Public Sub lblInstructionsTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblInstructions = expected
        actual = target.lblInstructions
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblTimeUnit</summary>
    <TestMethod()> Public Sub lblTimeUnitTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblTimeUnit = expected
        actual = target.lblTimeUnit
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblTimestep</summary>
    <TestMethod()> Public Sub lblTimestepTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblTimestep = expected
        actual = target.lblTimestep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lstAggregation</summary>
    <TestMethod()> Public Sub lstAggregationTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstAggregation = expected
        actual = target.lstAggregation
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lstTimeUnit</summary>
    <TestMethod()> Public Sub lstTimeUnitTest()
        Dim target As frmDefaultTimeInterval = New frmDefaultTimeInterval() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstTimeUnit = expected
        actual = target.lstTimeUnit
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
