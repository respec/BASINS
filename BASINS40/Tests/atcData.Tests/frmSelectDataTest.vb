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

    '''<summary>Test frmSelectData Constructor</summary>
    <TestMethod()> Public Sub frmSelectDataConstructorTest()
        Dim target As frmSelectData = New frmSelectData()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>Test AddCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub AddCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aText As String = String.Empty ' TODO: Initialize to an appropriate value
        target.AddCriteria(aText)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AskUser</summary>
    <TestMethod()> Public Sub AskUserTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim aGroup As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim aModal As Boolean = False ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.AskUser(aGroup, aModal)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test CreateSelectedGroupWithTimeStep</summary>
    <TestMethod()> Public Sub CreateSelectedGroupWithTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        actual = target.CreateSelectedGroupWithTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test Dispose</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub DisposeTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim disposing As Boolean = False ' TODO: Initialize to an appropriate value
        target.Dispose(disposing)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test GetIndex</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub GetIndexTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim actual As Integer
        actual = target.GetIndex(aName)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test InitializeComponent</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub InitializeComponentTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.InitializeComponent()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test LoadFilterClick</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LoadFilterClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.LoadFilterClick(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test LoadFilters</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LoadFiltersTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aFilename As String = String.Empty ' TODO: Initialize to an appropriate value
        target.LoadFilters(aFilename)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test LoadFiltersMenu</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub LoadFiltersMenuTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.LoadFiltersMenu()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test OpenedData</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub OpenedDataTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aDataSource As atcTimeseriesSource = Nothing ' TODO: Initialize to an appropriate value
        target.OpenedData(aDataSource)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test Populate</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.Populate()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test PopulateCriteriaCombos</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateCriteriaCombosTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.PopulateCriteriaCombos()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test PopulateCriteriaList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateCriteriaListTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAttributeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim aList As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        target.PopulateCriteriaList(aAttributeName, aList)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test PopulateMatching</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub PopulateMatchingTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.PopulateMatching()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveAllCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemoveAllCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.RemoveAllCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test RemoveCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub RemoveCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim cbo As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim lst As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        target.RemoveCriteria(cbo, lst)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test ResizeOneCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub ResizeOneCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aCriteria As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.ResizeOneCriteria(aCriteria, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SelectMatchingRow</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SelectMatchingRowTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aSelect As Boolean = False ' TODO: Initialize to an appropriate value
        target.SelectMatchingRow(aRow, aSelect)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test SizeCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub SizeCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.SizeCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test TimeUnits_Changed</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub TimeUnits_ChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.TimeUnits_Changed(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test UpdateManagerSelectionAttributes</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub UpdateManagerSelectionAttributesTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.UpdateManagerSelectionAttributes()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test UpdatedCriteria</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub UpdatedCriteriaTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        target.UpdatedCriteria()
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test __ENCAddToList</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub __ENCAddToListTest()
        Dim value As Object = Nothing ' TODO: Initialize to an appropriate value
        frmSelectData_Accessor.__ENCAddToList(value)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnCancel_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnCancel_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnCancel_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test btnOk_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub btnOk_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.btnOk_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test cboCriteria_SelectedIndexChanged</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub cboCriteria_SelectedIndexChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.cboCriteria_SelectedIndexChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test frmSelectData_Closed</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_ClosedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_Closed(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test frmSelectData_KeyDown</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_KeyDownTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_KeyDown(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test frmSelectData_VisibleChanged</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub frmSelectData_VisibleChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.frmSelectData_VisibleChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test lstCriteria_GotFocus</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_GotFocusTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aSource As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstCriteria_GotFocus(aSource, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test lstCriteria_KeyDownGrid</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_KeyDownGridTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim e As KeyEventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.lstCriteria_KeyDownGrid(aGrid, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test lstCriteria_MouseDownCell</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub lstCriteria_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.lstCriteria_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuAttributesAdd_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuAttributesAdd_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuAttributesAdd_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuFileManage_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuFileManage_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuFileManage_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuHelp_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuHelp_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuHelp_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuLoadFilters_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuLoadFilters_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuLoadFilters_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuMove_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuMove_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuMove_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuOpenData_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuOpenData_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuOpenData_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuRemove_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuRemove_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuRemove_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSaveFilters_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSaveFilters_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSaveFilters_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSelectAllMatching_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectAllMatching_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectAllMatching_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSelectAll_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectAll_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectAll_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSelectClear_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectClear_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectClear_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSelectMap_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectMap_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectMap_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test mnuSelectNoMatching_Click</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub mnuSelectNoMatching_ClickTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.mnuSelectNoMatching_Click(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pMatchingGrid_MouseDownCell</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pMatchingGrid_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pMatchingGrid_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pMatchingGrid_UserResizedColumn</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pMatchingGrid_UserResizedColumnTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pMatchingGrid_UserResizedColumn(aGrid, aColumn, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pSelectedGrid_MouseDownCell</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGrid_MouseDownCellTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aRow As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSelectedGrid_MouseDownCell(aGrid, aRow, aColumn)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pSelectedGrid_UserResizedColumn</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGrid_UserResizedColumnTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aGrid As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim aColumn As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim aWidth As Integer = 0 ' TODO: Initialize to an appropriate value
        target.pSelectedGrid_UserResizedColumn(aGrid, aColumn, aWidth)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test pSelectedGroup_Changed</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGroup_ChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim aAdded As atcCollection = Nothing ' TODO: Initialize to an appropriate value
        target.pSelectedGroup_Changed(aAdded)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test panelCriteria_SizeChanged</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub panelCriteria_SizeChangedTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim sender As Object = Nothing ' TODO: Initialize to an appropriate value
        Dim e As EventArgs = Nothing ' TODO: Initialize to an appropriate value
        target.panelCriteria_SizeChanged(sender, e)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>Test AvailableData</summary>
    <TestMethod()> Public Sub AvailableDataTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcDataGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcDataGroup
        target.AvailableData = expected
        actual = target.AvailableData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test MenuStrip1</summary>
    <TestMethod()> Public Sub MenuStrip1Test()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As MenuStrip = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As MenuStrip
        target.MenuStrip1 = expected
        actual = target.MenuStrip1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test SelectedOk</summary>
    <TestMethod()> Public Sub SelectedOkTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        target.SelectedOk = expected
        actual = target.SelectedOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test atcSelectedDates</summary>
    <TestMethod()> Public Sub atcSelectedDatesTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcChooseDataGroupDates = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcChooseDataGroupDates
        target.atcSelectedDates = expected
        actual = target.atcSelectedDates
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnCancel</summary>
    <TestMethod()> Public Sub btnCancelTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnCancel = expected
        actual = target.btnCancel
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test btnOk</summary>
    <TestMethod()> Public Sub btnOkTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Button = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Button
        target.btnOk = expected
        actual = target.btnOk
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboAggregate</summary>
    <TestMethod()> Public Sub cboAggregateTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboAggregate = expected
        actual = target.cboAggregate
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test cboTimeUnits</summary>
    <TestMethod()> Public Sub cboTimeUnitsTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ComboBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ComboBox
        target.cboTimeUnits = expected
        actual = target.cboTimeUnits
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test chkTimeStep</summary>
    <TestMethod()> Public Sub chkTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As CheckBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CheckBox
        target.chkTimeStep = expected
        actual = target.chkTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test groupSelected</summary>
    <TestMethod()> Public Sub groupSelectedTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.groupSelected = expected
        actual = target.groupSelected
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test groupTop</summary>
    <TestMethod()> Public Sub groupTopTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As GroupBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As GroupBox
        target.groupTop = expected
        actual = target.groupTop
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test lblMatching</summary>
    <TestMethod()> Public Sub lblMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Label = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Label
        target.lblMatching = expected
        actual = target.lblMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuAttributes</summary>
    <TestMethod()> Public Sub mnuAttributesTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributes = expected
        actual = target.mnuAttributes
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuAttributesAdd</summary>
    <TestMethod()> Public Sub mnuAttributesAddTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesAdd = expected
        actual = target.mnuAttributesAdd
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuAttributesMove</summary>
    <TestMethod()> Public Sub mnuAttributesMoveTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesMove = expected
        actual = target.mnuAttributesMove
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuAttributesRemove</summary>
    <TestMethod()> Public Sub mnuAttributesRemoveTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuAttributesRemove = expected
        actual = target.mnuAttributesRemove
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuFile</summary>
    <TestMethod()> Public Sub mnuFileTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuFile = expected
        actual = target.mnuFile
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuFileManage</summary>
    <TestMethod()> Public Sub mnuFileManageTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuFileManage = expected
        actual = target.mnuFileManage
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuHelp</summary>
    <TestMethod()> Public Sub mnuHelpTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuHelp = expected
        actual = target.mnuHelp
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuLoadFilters</summary>
    <TestMethod()> Public Sub mnuLoadFiltersTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuLoadFilters = expected
        actual = target.mnuLoadFilters
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuOpenData</summary>
    <TestMethod()> Public Sub mnuOpenDataTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuOpenData = expected
        actual = target.mnuOpenData
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSaveFilters</summary>
    <TestMethod()> Public Sub mnuSaveFiltersTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSaveFilters = expected
        actual = target.mnuSaveFilters
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelect</summary>
    <TestMethod()> Public Sub mnuSelectTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelect = expected
        actual = target.mnuSelect
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectAll</summary>
    <TestMethod()> Public Sub mnuSelectAllTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectAll = expected
        actual = target.mnuSelectAll
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectAllMatching</summary>
    <TestMethod()> Public Sub mnuSelectAllMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectAllMatching = expected
        actual = target.mnuSelectAllMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectClear</summary>
    <TestMethod()> Public Sub mnuSelectClearTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectClear = expected
        actual = target.mnuSelectClear
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectMap</summary>
    <TestMethod()> Public Sub mnuSelectMapTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectMap = expected
        actual = target.mnuSelectMap
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectNoMatching</summary>
    <TestMethod()> Public Sub mnuSelectNoMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripMenuItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripMenuItem
        target.mnuSelectNoMatching = expected
        actual = target.mnuSelectNoMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test mnuSelectSeparator1</summary>
    <TestMethod()> Public Sub mnuSelectSeparator1Test()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As ToolStripSeparator = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As ToolStripSeparator
        target.mnuSelectSeparator1 = expected
        actual = target.mnuSelectSeparator1
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pMatchingGrid</summary>
    <TestMethod()> Public Sub pMatchingGridTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGrid
        target.pMatchingGrid = expected
        actual = target.pMatchingGrid
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pSelectedGrid</summary>
    <TestMethod()> Public Sub pSelectedGridTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As atcGrid = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcGrid
        target.pSelectedGrid = expected
        actual = target.pSelectedGrid
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pSelectedGroup</summary>
    <TestMethod(), DeploymentItem("atcData.dll")> _
    Public Sub pSelectedGroupTest()
        Dim target As frmSelectData_Accessor = New frmSelectData_Accessor() ' TODO: Initialize to an appropriate value
        Dim expected As atcTimeseriesGroup = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As atcTimeseriesGroup
        target.pSelectedGroup = expected
        actual = target.pSelectedGroup
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test panelCriteria</summary>
    <TestMethod()> Public Sub panelCriteriaTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.panelCriteria = expected
        actual = target.panelCriteria
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test pnlButtons</summary>
    <TestMethod()> Public Sub pnlButtonsTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Panel = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Panel
        target.pnlButtons = expected
        actual = target.pnlButtons
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test splitAboveMatching</summary>
    <TestMethod()> Public Sub splitAboveMatchingTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.splitAboveMatching = expected
        actual = target.splitAboveMatching
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test splitAboveSelected</summary>
    <TestMethod()> Public Sub splitAboveSelectedTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As Splitter = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As Splitter
        target.splitAboveSelected = expected
        actual = target.splitAboveSelected
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>Test txtTimeStep</summary>
    <TestMethod()> Public Sub txtTimeStepTest()
        Dim target As frmSelectData = New frmSelectData() ' TODO: Initialize to an appropriate value
        Dim expected As TextBox = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As TextBox
        target.txtTimeStep = expected
        actual = target.txtTimeStep
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
