Imports atcUtility

Imports System.Windows.Forms

Imports atcControls

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports atcData



'''<summary>
'''This is a test class for frmSelectDataTest and is intended
'''to contain all frmSelectDataTest Unit Tests
'''</summary>
<TestClass()> _
Public Class frmSelectDataTest


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
    '''A test for frmSelectData Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub frmSelectDataConstructorTest()
        Dim target As frmSelectData = New frmSelectData()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub AddCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        target.AddCriteria(aText)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AskUser
    '''</summary>
    <TestMethod()> _
    Public Sub AskUserTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aModal As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.AskUser(aGroup, aModal)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for CreateSelectedGroupWithTimeStep
    '''</summary>
    <TestMethod()> _
    Public Sub CreateSelectedGroupWithTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.CreateSelectedGroupWithTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for Dispose
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetIndex
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub GetIndexTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.GetIndex(aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for InitializeComponent
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for LoadFilterClick
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub LoadFilterClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.LoadFilterClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for LoadFilters
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub LoadFiltersTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        target.LoadFilters(aFilename)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for LoadFiltersMenu
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub LoadFiltersMenuTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.LoadFiltersMenu()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for OpenedData
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub OpenedDataTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        target.OpenedData(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for Populate
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub PopulateTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.Populate()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for PopulateCriteriaCombos
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub PopulateCriteriaCombosTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.PopulateCriteriaCombos()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for PopulateCriteriaList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub PopulateCriteriaListTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aList As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        target.PopulateCriteriaList(aAttributeName, aList)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for PopulateMatching
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub PopulateMatchingTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.PopulateMatching()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveAllCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RemoveAllCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.RemoveAllCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for RemoveCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub RemoveCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim cbo As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim lst As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        target.RemoveCriteria(cbo, lst)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for ResizeOneCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub ResizeOneCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aCriteria As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.ResizeOneCriteria(aCriteria, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SelectMatchingRow
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub SelectMatchingRowTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSelect As Boolean = False ' TODO: Initialize to an appropriate value
        target.SelectMatchingRow(aRow, aSelect)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for SizeCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub SizeCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.SizeCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for TimeUnits_Changed
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub TimeUnits_ChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.TimeUnits_Changed(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UpdateManagerSelectionAttributes
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub UpdateManagerSelectionAttributesTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.UpdateManagerSelectionAttributes()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for UpdatedCriteria
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub UpdatedCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.UpdatedCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for __ENCAddToList
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSelectData_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnCancel_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for btnOk_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for cboCriteria_SelectedIndexChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub cboCriteria_SelectedIndexChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.cboCriteria_SelectedIndexChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for frmSelectData_Closed
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_ClosedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_Closed(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for frmSelectData_KeyDown
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_KeyDownTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for frmSelectData_VisibleChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_VisibleChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_VisibleChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for lstCriteria_GotFocus
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_GotFocusTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSource As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstCriteria_GotFocus(aSource, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for lstCriteria_KeyDownGrid
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_KeyDownGridTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstCriteria_KeyDownGrid(aGrid, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for lstCriteria_MouseDownCell
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.lstCriteria_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuAttributesAdd_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuAttributesAdd_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuAttributesAdd_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuFileManage_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuFileManage_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuFileManage_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuHelp_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuHelp_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuHelp_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuLoadFilters_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuLoadFilters_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuLoadFilters_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuMove_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuMove_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuMove_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuOpenData_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuOpenData_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuOpenData_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuRemove_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuRemove_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuRemove_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSaveFilters_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSaveFilters_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSaveFilters_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSelectAllMatching_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectAllMatching_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectAllMatching_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSelectAll_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectAll_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSelectClear_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectClear_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectClear_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSelectMap_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectMap_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectMap_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for mnuSelectNoMatching_Click
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectNoMatching_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectNoMatching_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pMatchingGrid_MouseDownCell
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pMatchingGrid_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pMatchingGrid_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pMatchingGrid_UserResizedColumn
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pMatchingGrid_UserResizedColumnTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pMatchingGrid_UserResizedColumn(aGrid, aColumn, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSelectedGrid_MouseDownCell
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGrid_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSelectedGrid_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSelectedGrid_UserResizedColumn
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGrid_UserResizedColumnTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSelectedGrid_UserResizedColumn(aGrid, aColumn, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for pSelectedGroup_Changed
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGroup_ChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAdded As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pSelectedGroup_Changed(aAdded)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for panelCriteria_SizeChanged
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub panelCriteria_SizeChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.panelCriteria_SizeChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for AvailableData
    '''</summary>
    <TestMethod()> _
    Public Sub AvailableDataTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        target.AvailableData = expected
        actual = target.AvailableData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for MenuStrip1
    '''</summary>
    <TestMethod()> _
    Public Sub MenuStrip1Test()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As MenuStrip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As MenuStrip
        target.MenuStrip1 = expected
        actual = target.MenuStrip1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for SelectedOk
    '''</summary>
    <TestMethod()> _
    Public Sub SelectedOkTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.SelectedOk = expected
        actual = target.SelectedOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for atcSelectedDates
    '''</summary>
    <TestMethod()> _
    Public Sub atcSelectedDatesTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcChooseDataGroupDates = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcChooseDataGroupDates
        target.atcSelectedDates = expected
        actual = target.atcSelectedDates
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for btnCancel
    '''</summary>
    <TestMethod()> _
    Public Sub btnCancelTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
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
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cboAggregate
    '''</summary>
    <TestMethod()> _
    Public Sub cboAggregateTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboAggregate = expected
        actual = target.cboAggregate
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for cboTimeUnits
    '''</summary>
    <TestMethod()> _
    Public Sub cboTimeUnitsTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboTimeUnits = expected
        actual = target.cboTimeUnits
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for chkTimeStep
    '''</summary>
    <TestMethod()> _
    Public Sub chkTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As CheckBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CheckBox
        target.chkTimeStep = expected
        actual = target.chkTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for groupSelected
    '''</summary>
    <TestMethod()> _
    Public Sub groupSelectedTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.groupSelected = expected
        actual = target.groupSelected
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for groupTop
    '''</summary>
    <TestMethod()> _
    Public Sub groupTopTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.groupTop = expected
        actual = target.groupTop
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for lblMatching
    '''</summary>
    <TestMethod()> _
    Public Sub lblMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblMatching = expected
        actual = target.lblMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuAttributes
    '''</summary>
    <TestMethod()> _
    Public Sub mnuAttributesTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributes = expected
        actual = target.mnuAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuAttributesAdd
    '''</summary>
    <TestMethod()> _
    Public Sub mnuAttributesAddTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesAdd = expected
        actual = target.mnuAttributesAdd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuAttributesMove
    '''</summary>
    <TestMethod()> _
    Public Sub mnuAttributesMoveTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesMove = expected
        actual = target.mnuAttributesMove
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuAttributesRemove
    '''</summary>
    <TestMethod()> _
    Public Sub mnuAttributesRemoveTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesRemove = expected
        actual = target.mnuAttributesRemove
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuFile
    '''</summary>
    <TestMethod()> _
    Public Sub mnuFileTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuFile = expected
        actual = target.mnuFile
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuFileManage
    '''</summary>
    <TestMethod()> _
    Public Sub mnuFileManageTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuFileManage = expected
        actual = target.mnuFileManage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuHelp
    '''</summary>
    <TestMethod()> _
    Public Sub mnuHelpTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuHelp = expected
        actual = target.mnuHelp
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuLoadFilters
    '''</summary>
    <TestMethod()> _
    Public Sub mnuLoadFiltersTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuLoadFilters = expected
        actual = target.mnuLoadFilters
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuOpenData
    '''</summary>
    <TestMethod()> _
    Public Sub mnuOpenDataTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuOpenData = expected
        actual = target.mnuOpenData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSaveFilters
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSaveFiltersTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSaveFilters = expected
        actual = target.mnuSaveFilters
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelect
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelect = expected
        actual = target.mnuSelect
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectAll
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectAllTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectAll = expected
        actual = target.mnuSelectAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectAllMatching
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectAllMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectAllMatching = expected
        actual = target.mnuSelectAllMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectClear
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectClearTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectClear = expected
        actual = target.mnuSelectClear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectMap
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectMapTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectMap = expected
        actual = target.mnuSelectMap
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectNoMatching
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectNoMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectNoMatching = expected
        actual = target.mnuSelectNoMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for mnuSelectSeparator1
    '''</summary>
    <TestMethod()> _
    Public Sub mnuSelectSeparator1Test()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripSeparator = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripSeparator
        target.mnuSelectSeparator1 = expected
        actual = target.mnuSelectSeparator1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pMatchingGrid
    '''</summary>
    <TestMethod()> _
    Public Sub pMatchingGridTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGrid
        target.pMatchingGrid = expected
        actual = target.pMatchingGrid
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pSelectedGrid
    '''</summary>
    <TestMethod()> _
    Public Sub pSelectedGridTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGrid
        target.pSelectedGrid = expected
        actual = target.pSelectedGrid
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pSelectedGroup
    '''</summary>
    <TestMethod(), _
     DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGroupTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pSelectedGroup = expected
        actual = target.pSelectedGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for panelCriteria
    '''</summary>
    <TestMethod()> _
    Public Sub panelCriteriaTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.panelCriteria = expected
        actual = target.panelCriteria
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for pnlButtons
    '''</summary>
    <TestMethod()> _
    Public Sub pnlButtonsTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.pnlButtons = expected
        actual = target.pnlButtons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for splitAboveMatching
    '''</summary>
    <TestMethod()> _
    Public Sub splitAboveMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.splitAboveMatching = expected
        actual = target.splitAboveMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for splitAboveSelected
    '''</summary>
    <TestMethod()> _
    Public Sub splitAboveSelectedTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.splitAboveSelected = expected
        actual = target.splitAboveSelected
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for txtTimeStep
    '''</summary>
    <TestMethod()> _
    Public Sub txtTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtTimeStep = expected
        actual = target.txtTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
