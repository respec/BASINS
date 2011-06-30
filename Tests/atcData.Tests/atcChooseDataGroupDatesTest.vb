Imports System.Windows.Forms
Imports atcUtility
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for atcChooseDataGroupDatesTest and is intended
'''to contain all atcChooseDataGroupDatesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcChooseDataGroupDatesTest
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

    '''<summary>Test atcChooseDataGroupDates Constructor</summary>
    <TestMethod()> Public Sub atcChooseDataGroupDatesConstructorTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test CreateSelectedDataGroupSubset</summary>
    <TestMethod()> Public Sub CreateSelectedDataGroupSubsetTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.CreateSelectedDataGroupSubset
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Reset</summary>
    <TestMethod()> Public Sub ResetTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        target.Reset()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcChooseDataGroupDates_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAll_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnAll_ClickTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCommon_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnCommon_ClickTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCommon_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pDataGroup_Added</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pDataGroup_AddedTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAdded As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pDataGroup_Added(aAdded)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pDataGroup_Removed</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pDataGroup_RemovedTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRemoved As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pDataGroup_Removed(aRemoved)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CommonEnd</summary>
    <TestMethod()> Public Sub CommonEndTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.CommonEnd = expected
        actual = target.CommonEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CommonStart</summary>
    <TestMethod()> Public Sub CommonStartTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.CommonStart = expected
        actual = target.CommonStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test DataGroup</summary>
    <TestMethod()> Public Sub DataGroupTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.DataGroup = expected
        actual = target.DataGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test FirstStart</summary>
    <TestMethod()> Public Sub FirstStartTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.FirstStart = expected
        actual = target.FirstStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test LastEnd</summary>
    <TestMethod()> Public Sub LastEndTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.LastEnd = expected
        actual = target.LastEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OmitAfter</summary>
    <TestMethod()> Public Sub OmitAfterTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.OmitAfter = expected
        actual = target.OmitAfter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test OmitBefore</summary>
    <TestMethod()> Public Sub OmitBeforeTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.OmitBefore = expected
        actual = target.OmitBefore
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectedAll</summary>
    <TestMethod()> Public Sub SelectedAllTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.SelectedAll
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Text</summary>
    <TestMethod()> Public Sub TextTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim actual As String
        target.Text = expected
        actual = target.Text
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test ToolTip1</summary>
    <TestMethod()> Public Sub ToolTip1Test()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As ToolTip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolTip
        target.ToolTip1 = expected
        actual = target.ToolTip1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnAll</summary>
    <TestMethod()> Public Sub btnAllTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAll = expected
        actual = target.btnAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnCommon</summary>
    <TestMethod()> Public Sub btnCommonTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCommon = expected
        actual = target.btnCommon
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test chkYearly</summary>
    <TestMethod()> Public Sub chkYearlyTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As CheckBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CheckBox
        target.chkYearly = expected
        actual = target.chkYearly
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test grpYears</summary>
    <TestMethod()> Public Sub grpYearsTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.grpYears = expected
        actual = target.grpYears
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblCommonEnd</summary>
    <TestMethod()> Public Sub lblCommonEndTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblCommonEnd = expected
        actual = target.lblCommonEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblCommonStart</summary>
    <TestMethod()> Public Sub lblCommonStartTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblCommonStart = expected
        actual = target.lblCommonStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblDataEnd</summary>
    <TestMethod()> Public Sub lblDataEndTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDataEnd = expected
        actual = target.lblDataEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblDataStart</summary>
    <TestMethod()> Public Sub lblDataStartTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDataStart = expected
        actual = target.lblDataStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblOmitAfter</summary>
    <TestMethod()> Public Sub lblOmitAfterTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblOmitAfter = expected
        actual = target.lblOmitAfter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblOmitBefore</summary>
    <TestMethod()> Public Sub lblOmitBeforeTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblOmitBefore = expected
        actual = target.lblOmitBefore
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pDataGroup</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pDataGroupTest()
        Dim target As atcChooseDataGroupDates_Accessor = New atcChooseDataGroupDates_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pDataGroup = expected
        actual = target.pDataGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtOmitAfter</summary>
    <TestMethod()> Public Sub txtOmitAfterTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtOmitAfter = expected
        actual = target.txtOmitAfter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtOmitBefore</summary>
    <TestMethod()> Public Sub txtOmitBeforeTest()
        Dim target As atcChooseDataGroupDates = New atcChooseDataGroupDates() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtOmitBefore = expected
        actual = target.txtOmitBefore
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
