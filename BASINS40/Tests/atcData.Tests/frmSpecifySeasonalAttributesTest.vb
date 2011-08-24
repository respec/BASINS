Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports atcData

'''<summary>
'''This is a test class for frmSpecifySeasonalAttributesTest and is intended
'''to contain all frmSpecifySeasonalAttributesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSpecifySeasonalAttributesTest
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

    '''<summary>Test frmSpecifySeasonalAttributes Constructor</summary>
    <TestMethod()> Public Sub frmSpecifySeasonalAttributesConstructorTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aSeasonsAvailable As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.AskUser(aTimeseriesGroup, aSeasonsAvailable)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CalculateAttributes</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub CalculateAttributesTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAttributes As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim aSetInTimeseries As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcDataAttributes = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataAttributes
        actual = target.CalculateAttributes(aAttributes, aSetInTimeseries)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Clear</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub ClearTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        target.Clear()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test CurrentSeason</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub CurrentSeasonTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = target.CurrentSeason
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test LoadListSelected</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LoadListSelectedTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim aListbox As ListBox = Nothing ' TODO: Initialize to an appropriate value
        target.LoadListSelected(aListbox)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SaveListSelected</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SaveListSelectedTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim aListbox As ListBox = Nothing ' TODO: Initialize to an appropriate value
        target.SaveListSelected(aListbox)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAttributesAll_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnAttributesAll_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAttributesAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnAttributesNone_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnAttributesNone_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnAttributesNone_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnSeasonsAll_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnSeasonsAll_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSeasonsAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnSeasonsNone_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnSeasonsNone_ClickTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSeasonsNone_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test cboSeasons_SelectedIndexChanged</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub cboSeasons_SelectedIndexChangedTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.cboSeasons_SelectedIndexChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test lstAttributes_SelectedIndexChanged</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub lstAttributes_SelectedIndexChangedTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstAttributes_SelectedIndexChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Splitter1</summary>
    <TestMethod()> Public Sub Splitter1Test()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.Splitter1 = expected
        actual = target.Splitter1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnAttributesAll</summary>
    <TestMethod()> Public Sub btnAttributesAllTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAttributesAll = expected
        actual = target.btnAttributesAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnAttributesNone</summary>
    <TestMethod()> Public Sub btnAttributesNoneTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnAttributesNone = expected
        actual = target.btnAttributesNone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnSeasonsAll</summary>
    <TestMethod()> Public Sub btnSeasonsAllTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSeasonsAll = expected
        actual = target.btnSeasonsAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnSeasonsNone</summary>
    <TestMethod()> Public Sub btnSeasonsNoneTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSeasonsNone = expected
        actual = target.btnSeasonsNone
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboSeasons</summary>
    <TestMethod()> Public Sub cboSeasonsTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboSeasons = expected
        actual = target.cboSeasons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test grpAttributes</summary>
    <TestMethod()> Public Sub grpAttributesTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.grpAttributes = expected
        actual = target.grpAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test grpSeasons</summary>
    <TestMethod()> Public Sub grpSeasonsTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.grpSeasons = expected
        actual = target.grpSeasons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lstAttributes</summary>
    <TestMethod()> Public Sub lstAttributesTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstAttributes = expected
        actual = target.lstAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lstSeasons</summary>
    <TestMethod()> Public Sub lstSeasonsTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As ListBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ListBox
        target.lstSeasons = expected
        actual = target.lstSeasons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pTimseriesGroup</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pTimseriesGroupTest()
        Dim target As frmSpecifySeasonalAttributes_Accessor = New frmSpecifySeasonalAttributes_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pTimseriesGroup = expected
        actual = target.pTimseriesGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test panelBottom</summary>
    <TestMethod()> Public Sub panelBottomTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.panelBottom = expected
        actual = target.panelBottom
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test panelTop</summary>
    <TestMethod()> Public Sub panelTopTest()
        Dim target As frmSpecifySeasonalAttributes = New frmSpecifySeasonalAttributes() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.panelTop = expected
        actual = target.panelTop
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
