Imports System

Imports System.Windows.Forms

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for atcSeasonYearsTest and is intended
'''to contain all atcSeasonYearsTest Unit Tests
'''</summary>
<TestClass()> _
Public Class atcSeasonYearsTest


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
    '''A test for atcSeasonYears Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub atcSeasonYearsConstructorTest()
        Dim target As atcSeasonYears = New atcSeasonYears()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetTextbox
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub GetTextboxTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim aTextbox As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.GetTextbox(aTextbox)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Reset
    '''</summary>
    <TestMethod()> _
    Public Sub ResetTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        target.Reset()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SetTextbox
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub SetTextboxTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim aTextbox As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim aValue As Integer = 0 ' TODO: Initialize to an appropriate value
        target.SetTextbox(aTextbox, aValue)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        atcSeasonYears_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnAllYears_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnAllYears_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAllYears_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnAll_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnAll_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCalendarYear_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnCalendarYear_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCalendarYear_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCommonYears_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnCommonYears_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCommonYears_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCommon_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnCommon_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCommon_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnWaterYear_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnWaterYear_ClickTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnWaterYear_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for CommonEnd
    '''</summary>
    <TestMethod()> _
    Public Sub CommonEndTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.CommonEnd = expected
        actual = target.CommonEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CommonStart
    '''</summary>
    <TestMethod()> _
    Public Sub CommonStartTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Double = 0.0! ' TODO: Initialize to an appropriate value
        Dim actual As Double
        target.CommonStart = expected
        actual = target.CommonStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for DataGroup
    '''</summary>
    <TestMethod()> _
    Public Sub DataGroupTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.DataGroup = expected
        actual = target.DataGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for EndDay
    '''</summary>
    <TestMethod()> _
    Public Sub EndDayTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.EndDay = expected
        actual = target.EndDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for EndMonth
    '''</summary>
    <TestMethod()> _
    Public Sub EndMonthTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.EndMonth = expected
        actual = target.EndMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OmitAfterYear
    '''</summary>
    <TestMethod()> _
    Public Sub OmitAfterYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.OmitAfterYear = expected
        actual = target.OmitAfterYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for OmitBeforeYear
    '''</summary>
    <TestMethod()> _
    Public Sub OmitBeforeYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.OmitBeforeYear = expected
        actual = target.OmitBeforeYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for ShowBoundaries
    '''</summary>
    <TestMethod()> _
    Public Sub ShowBoundariesTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.ShowBoundaries = expected
        actual = target.ShowBoundaries
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Splitter1
    '''</summary>
    <TestMethod()> _
    Public Sub Splitter1Test()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.Splitter1 = expected
        actual = target.Splitter1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for StartDay
    '''</summary>
    <TestMethod()> _
    Public Sub StartDayTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.StartDay = expected
        actual = target.StartDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for StartMonth
    '''</summary>
    <TestMethod()> _
    Public Sub StartMonthTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        target.StartMonth = expected
        actual = target.StartMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnAll
    '''</summary>
    <TestMethod()> _
    Public Sub btnAllTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAll = expected
        actual = target.btnAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnCalendarYear
    '''</summary>
    <TestMethod()> _
    Public Sub btnCalendarYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCalendarYear = expected
        actual = target.btnCalendarYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnCommon
    '''</summary>
    <TestMethod()> _
    Public Sub btnCommonTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCommon = expected
        actual = target.btnCommon
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnWaterYear
    '''</summary>
    <TestMethod()> _
    Public Sub btnWaterYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnWaterYear = expected
        actual = target.btnWaterYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cboEndMonth
    '''</summary>
    <TestMethod()> _
    Public Sub cboEndMonthTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboEndMonth = expected
        actual = target.cboEndMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cboStartMonth
    '''</summary>
    <TestMethod()> _
    Public Sub cboStartMonthTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboStartMonth = expected
        actual = target.cboStartMonth
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for grpBoundaries
    '''</summary>
    <TestMethod()> _
    Public Sub grpBoundariesTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.grpBoundaries = expected
        actual = target.grpBoundaries
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for grpYears
    '''</summary>
    <TestMethod()> _
    Public Sub grpYearsTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.grpYears = expected
        actual = target.grpYears
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblCommonEnd
    '''</summary>
    <TestMethod()> _
    Public Sub lblCommonEndTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblCommonEnd = expected
        actual = target.lblCommonEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblCommonStart
    '''</summary>
    <TestMethod()> _
    Public Sub lblCommonStartTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblCommonStart = expected
        actual = target.lblCommonStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblDataEnd
    '''</summary>
    <TestMethod()> _
    Public Sub lblDataEndTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDataEnd = expected
        actual = target.lblDataEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblDataStart
    '''</summary>
    <TestMethod()> _
    Public Sub lblDataStartTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDataStart = expected
        actual = target.lblDataStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblOmitAfter
    '''</summary>
    <TestMethod()> _
    Public Sub lblOmitAfterTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblOmitAfter = expected
        actual = target.lblOmitAfter
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblOmitBefore
    '''</summary>
    <TestMethod()> _
    Public Sub lblOmitBeforeTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblOmitBefore = expected
        actual = target.lblOmitBefore
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblYearEnd
    '''</summary>
    <TestMethod()> _
    Public Sub lblYearEndTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblYearEnd = expected
        actual = target.lblYearEnd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblYearStart
    '''</summary>
    <TestMethod()> _
    Public Sub lblYearStartTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblYearStart = expected
        actual = target.lblYearStart
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pDataGroup
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pDataGroupTest()
        Dim target As atcSeasonYears_Accessor = New atcSeasonYears_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pDataGroup = expected
        actual = target.pDataGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtEndDay
    '''</summary>
    <TestMethod()> _
    Public Sub txtEndDayTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtEndDay = expected
        actual = target.txtEndDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtOmitAfterYear
    '''</summary>
    <TestMethod()> _
    Public Sub txtOmitAfterYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtOmitAfterYear = expected
        actual = target.txtOmitAfterYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtOmitBeforeYear
    '''</summary>
    <TestMethod()> _
    Public Sub txtOmitBeforeYearTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtOmitBeforeYear = expected
        actual = target.txtOmitBeforeYear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtStartDay
    '''</summary>
    <TestMethod()> _
    Public Sub txtStartDayTest()
        Dim target As atcSeasonYears = New atcSeasonYears() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtStartDay = expected
        actual = target.txtStartDay
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
