Imports System.Windows.Forms

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for frmSelectDisplayTest and is intended
'''to contain all frmSelectDisplayTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSelectDisplayTest


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
    '''A test for frmSelectDisplay Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmSelectDisplayConstructorTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AskUser
    '''</summary>
    <TestMethod()> _
    Public Sub AskUserTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim aTimeseriesGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        target.AskUser(aTimeseriesGroup)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for FormFromGroup
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub FormFromGroupTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        target.FormFromGroup()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SaveData
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub SaveDataTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = target.SaveData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for UserOpenDataFile
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub UserOpenDataFileTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim aNeedToOpen As Boolean = False ' TODO: Initialize to an appropriate value
        Dim aNeedToSave As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesSource
        actual = target.UserOpenDataFile(aNeedToOpen, aNeedToSave)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSelectDisplay_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnDiscard_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnDiscard_ClickTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnDiscard_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSave_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnSave_ClickTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSave_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnSelect_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnSelect_ClickTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnSelect_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btn_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btn_ClickTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btn_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for frmSelectDisplay_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub frmSelectDisplay_KeyDownTest()
        Dim target As frmSelectDisplay_Accessor = New frmSelectDisplay_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectDisplay_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GroupBox1
    '''</summary>
    <TestMethod()> _
    Public Sub GroupBox1Test()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.GroupBox1 = expected
        actual = target.GroupBox1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Label2
    '''</summary>
    <TestMethod()> _
    Public Sub Label2Test()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.Label2 = expected
        actual = target.Label2
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnDiscard
    '''</summary>
    <TestMethod()> _
    Public Sub btnDiscardTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnDiscard = expected
        actual = target.btnDiscard
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnFrequency
    '''</summary>
    <TestMethod()> _
    Public Sub btnFrequencyTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnFrequency = expected
        actual = target.btnFrequency
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnGraph
    '''</summary>
    <TestMethod()> _
    Public Sub btnGraphTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnGraph = expected
        actual = target.btnGraph
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnList
    '''</summary>
    <TestMethod()> _
    Public Sub btnListTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnList = expected
        actual = target.btnList
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSave
    '''</summary>
    <TestMethod()> _
    Public Sub btnSaveTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSave = expected
        actual = target.btnSave
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSeasonal
    '''</summary>
    <TestMethod()> _
    Public Sub btnSeasonalTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSeasonal = expected
        actual = target.btnSeasonal
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnSelect
    '''</summary>
    <TestMethod()> _
    Public Sub btnSelectTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnSelect = expected
        actual = target.btnSelect
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnTree
    '''</summary>
    <TestMethod()> _
    Public Sub btnTreeTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnTree = expected
        actual = target.btnTree
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblDescribeDatasets
    '''</summary>
    <TestMethod()> _
    Public Sub lblDescribeDatasetsTest()
        Dim target As frmSelectDisplay = New frmSelectDisplay() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblDescribeDatasets = expected
        actual = target.lblDescribeDatasets
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
