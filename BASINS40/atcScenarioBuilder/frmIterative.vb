Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Public Class frmIterative
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents myTabs As System.Windows.Forms.TabControl
  Friend WithEvents tabInputs As System.Windows.Forms.TabPage
  Friend WithEvents tabEndpoints As System.Windows.Forms.TabPage
  Friend WithEvents tabResults As System.Windows.Forms.TabPage
  Friend WithEvents tabPivot As System.Windows.Forms.TabPage
  Friend WithEvents btnInputAdd As System.Windows.Forms.Button
  Friend WithEvents btnInputModify As System.Windows.Forms.Button
  Friend WithEvents btnInputRemove As System.Windows.Forms.Button
  Friend WithEvents btnInputUp As System.Windows.Forms.Button
  Friend WithEvents btnInputDown As System.Windows.Forms.Button
  Friend WithEvents cboPivotRows As System.Windows.Forms.ComboBox
  Friend WithEvents cboPivotColumns As System.Windows.Forms.ComboBox
  Friend WithEvents cboPivotCells As System.Windows.Forms.ComboBox
  Friend WithEvents lblPivotRows As System.Windows.Forms.Label
  Friend WithEvents lblPivotCells As System.Windows.Forms.Label
  Friend WithEvents lblPivotColumns As System.Windows.Forms.Label
  Friend WithEvents btnStart As System.Windows.Forms.Button
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSaveResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSavePivot As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuCopyResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuCopyPivot As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPasteResults As System.Windows.Forms.MenuItem
  Friend WithEvents agdPivot As atcControls.atcGrid
  Friend WithEvents lstInputs As System.Windows.Forms.CheckedListBox
  Friend WithEvents btnEndpointDown As System.Windows.Forms.Button
  Friend WithEvents btnEndpointUp As System.Windows.Forms.Button
  Friend WithEvents btnEndpointRemove As System.Windows.Forms.Button
  Friend WithEvents btnEndpointModify As System.Windows.Forms.Button
  Friend WithEvents btnEndpointAdd As System.Windows.Forms.Button
  Friend WithEvents lstEndpoints As System.Windows.Forms.CheckedListBox
  Friend WithEvents txtModifiedScenarioName As System.Windows.Forms.TextBox
  Friend WithEvents lblBaseScenarioName As System.Windows.Forms.Label
  Friend WithEvents lblNewScenarioName As System.Windows.Forms.Label
  Friend WithEvents cboBaseScenarioName As System.Windows.Forms.ComboBox
  Friend WithEvents agdResults As atcControls.atcGrid
  Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLoadResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLoadVariations As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSaveVariations As System.Windows.Forms.MenuItem
  Friend WithEvents lblTop As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmIterative))
    Me.myTabs = New System.Windows.Forms.TabControl
    Me.tabInputs = New System.Windows.Forms.TabPage
    Me.lstInputs = New System.Windows.Forms.CheckedListBox
    Me.btnInputDown = New System.Windows.Forms.Button
    Me.btnInputUp = New System.Windows.Forms.Button
    Me.btnInputRemove = New System.Windows.Forms.Button
    Me.btnInputModify = New System.Windows.Forms.Button
    Me.btnInputAdd = New System.Windows.Forms.Button
    Me.txtModifiedScenarioName = New System.Windows.Forms.TextBox
    Me.cboBaseScenarioName = New System.Windows.Forms.ComboBox
    Me.lblBaseScenarioName = New System.Windows.Forms.Label
    Me.lblNewScenarioName = New System.Windows.Forms.Label
    Me.tabEndpoints = New System.Windows.Forms.TabPage
    Me.lstEndpoints = New System.Windows.Forms.CheckedListBox
    Me.btnEndpointDown = New System.Windows.Forms.Button
    Me.btnEndpointUp = New System.Windows.Forms.Button
    Me.btnEndpointRemove = New System.Windows.Forms.Button
    Me.btnEndpointModify = New System.Windows.Forms.Button
    Me.btnEndpointAdd = New System.Windows.Forms.Button
    Me.tabResults = New System.Windows.Forms.TabPage
    Me.agdResults = New atcControls.atcGrid
    Me.tabPivot = New System.Windows.Forms.TabPage
    Me.agdPivot = New atcControls.atcGrid
    Me.lblPivotColumns = New System.Windows.Forms.Label
    Me.lblPivotCells = New System.Windows.Forms.Label
    Me.lblPivotRows = New System.Windows.Forms.Label
    Me.cboPivotCells = New System.Windows.Forms.ComboBox
    Me.cboPivotColumns = New System.Windows.Forms.ComboBox
    Me.cboPivotRows = New System.Windows.Forms.ComboBox
    Me.btnStart = New System.Windows.Forms.Button
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuLoadResults = New System.Windows.Forms.MenuItem
    Me.mnuSaveResults = New System.Windows.Forms.MenuItem
    Me.mnuSavePivot = New System.Windows.Forms.MenuItem
    Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
    Me.mnuLoadVariations = New System.Windows.Forms.MenuItem
    Me.mnuSaveVariations = New System.Windows.Forms.MenuItem
    Me.MenuItem1 = New System.Windows.Forms.MenuItem
    Me.mnuCopyResults = New System.Windows.Forms.MenuItem
    Me.mnuCopyPivot = New System.Windows.Forms.MenuItem
    Me.mnuPasteResults = New System.Windows.Forms.MenuItem
    Me.lblTop = New System.Windows.Forms.Label
    Me.myTabs.SuspendLayout()
    Me.tabInputs.SuspendLayout()
    Me.tabEndpoints.SuspendLayout()
    Me.tabResults.SuspendLayout()
    Me.tabPivot.SuspendLayout()
    Me.SuspendLayout()
    '
    'myTabs
    '
    Me.myTabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.myTabs.Controls.Add(Me.tabInputs)
    Me.myTabs.Controls.Add(Me.tabEndpoints)
    Me.myTabs.Controls.Add(Me.tabResults)
    Me.myTabs.Controls.Add(Me.tabPivot)
    Me.myTabs.Location = New System.Drawing.Point(0, 40)
    Me.myTabs.Name = "myTabs"
    Me.myTabs.SelectedIndex = 0
    Me.myTabs.Size = New System.Drawing.Size(296, 264)
    Me.myTabs.TabIndex = 0
    '
    'tabInputs
    '
    Me.tabInputs.Controls.Add(Me.lstInputs)
    Me.tabInputs.Controls.Add(Me.btnInputDown)
    Me.tabInputs.Controls.Add(Me.btnInputUp)
    Me.tabInputs.Controls.Add(Me.btnInputRemove)
    Me.tabInputs.Controls.Add(Me.btnInputModify)
    Me.tabInputs.Controls.Add(Me.btnInputAdd)
    Me.tabInputs.Controls.Add(Me.txtModifiedScenarioName)
    Me.tabInputs.Controls.Add(Me.cboBaseScenarioName)
    Me.tabInputs.Controls.Add(Me.lblBaseScenarioName)
    Me.tabInputs.Controls.Add(Me.lblNewScenarioName)
    Me.tabInputs.Location = New System.Drawing.Point(4, 22)
    Me.tabInputs.Name = "tabInputs"
    Me.tabInputs.Size = New System.Drawing.Size(288, 238)
    Me.tabInputs.TabIndex = 0
    Me.tabInputs.Text = "Inputs"
    '
    'lstInputs
    '
    Me.lstInputs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstInputs.CheckOnClick = True
    Me.lstInputs.Location = New System.Drawing.Point(8, 96)
    Me.lstInputs.Name = "lstInputs"
    Me.lstInputs.Size = New System.Drawing.Size(272, 139)
    Me.lstInputs.TabIndex = 10
    '
    'btnInputDown
    '
    Me.btnInputDown.Location = New System.Drawing.Point(248, 64)
    Me.btnInputDown.Name = "btnInputDown"
    Me.btnInputDown.Size = New System.Drawing.Size(24, 24)
    Me.btnInputDown.TabIndex = 9
    Me.btnInputDown.Text = "v"
    '
    'btnInputUp
    '
    Me.btnInputUp.Location = New System.Drawing.Point(216, 64)
    Me.btnInputUp.Name = "btnInputUp"
    Me.btnInputUp.Size = New System.Drawing.Size(24, 24)
    Me.btnInputUp.TabIndex = 8
    Me.btnInputUp.Text = "^"
    '
    'btnInputRemove
    '
    Me.btnInputRemove.Location = New System.Drawing.Point(80, 64)
    Me.btnInputRemove.Name = "btnInputRemove"
    Me.btnInputRemove.Size = New System.Drawing.Size(56, 24)
    Me.btnInputRemove.TabIndex = 7
    Me.btnInputRemove.Text = "Remove"
    '
    'btnInputModify
    '
    Me.btnInputModify.Location = New System.Drawing.Point(152, 64)
    Me.btnInputModify.Name = "btnInputModify"
    Me.btnInputModify.Size = New System.Drawing.Size(48, 24)
    Me.btnInputModify.TabIndex = 6
    Me.btnInputModify.Text = "Edit"
    '
    'btnInputAdd
    '
    Me.btnInputAdd.Location = New System.Drawing.Point(16, 64)
    Me.btnInputAdd.Name = "btnInputAdd"
    Me.btnInputAdd.Size = New System.Drawing.Size(48, 24)
    Me.btnInputAdd.TabIndex = 5
    Me.btnInputAdd.Text = "Add"
    '
    'txtModifiedScenarioName
    '
    Me.txtModifiedScenarioName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtModifiedScenarioName.Location = New System.Drawing.Point(96, 32)
    Me.txtModifiedScenarioName.Name = "txtModifiedScenarioName"
    Me.txtModifiedScenarioName.Size = New System.Drawing.Size(184, 20)
    Me.txtModifiedScenarioName.TabIndex = 3
    Me.txtModifiedScenarioName.Text = "Modified"
    '
    'cboBaseScenarioName
    '
    Me.cboBaseScenarioName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboBaseScenarioName.Location = New System.Drawing.Point(96, 8)
    Me.cboBaseScenarioName.Name = "cboBaseScenarioName"
    Me.cboBaseScenarioName.Size = New System.Drawing.Size(184, 21)
    Me.cboBaseScenarioName.TabIndex = 2
    '
    'lblBaseScenarioName
    '
    Me.lblBaseScenarioName.Location = New System.Drawing.Point(8, 8)
    Me.lblBaseScenarioName.Name = "lblBaseScenarioName"
    Me.lblBaseScenarioName.Size = New System.Drawing.Size(80, 16)
    Me.lblBaseScenarioName.TabIndex = 0
    Me.lblBaseScenarioName.Text = "Base Scenario"
    '
    'lblNewScenarioName
    '
    Me.lblNewScenarioName.Location = New System.Drawing.Point(8, 32)
    Me.lblNewScenarioName.Name = "lblNewScenarioName"
    Me.lblNewScenarioName.Size = New System.Drawing.Size(80, 16)
    Me.lblNewScenarioName.TabIndex = 11
    Me.lblNewScenarioName.Text = "New Scenario"
    '
    'tabEndpoints
    '
    Me.tabEndpoints.Controls.Add(Me.lstEndpoints)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointDown)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointUp)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointRemove)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointModify)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointAdd)
    Me.tabEndpoints.Location = New System.Drawing.Point(4, 22)
    Me.tabEndpoints.Name = "tabEndpoints"
    Me.tabEndpoints.Size = New System.Drawing.Size(288, 238)
    Me.tabEndpoints.TabIndex = 1
    Me.tabEndpoints.Text = "Endpoints"
    '
    'lstEndpoints
    '
    Me.lstEndpoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstEndpoints.CheckOnClick = True
    Me.lstEndpoints.Location = New System.Drawing.Point(8, 96)
    Me.lstEndpoints.Name = "lstEndpoints"
    Me.lstEndpoints.Size = New System.Drawing.Size(272, 139)
    Me.lstEndpoints.TabIndex = 16
    '
    'btnEndpointDown
    '
    Me.btnEndpointDown.Location = New System.Drawing.Point(248, 64)
    Me.btnEndpointDown.Name = "btnEndpointDown"
    Me.btnEndpointDown.Size = New System.Drawing.Size(24, 24)
    Me.btnEndpointDown.TabIndex = 15
    Me.btnEndpointDown.Text = "v"
    '
    'btnEndpointUp
    '
    Me.btnEndpointUp.Location = New System.Drawing.Point(216, 64)
    Me.btnEndpointUp.Name = "btnEndpointUp"
    Me.btnEndpointUp.Size = New System.Drawing.Size(24, 24)
    Me.btnEndpointUp.TabIndex = 14
    Me.btnEndpointUp.Text = "^"
    '
    'btnEndpointRemove
    '
    Me.btnEndpointRemove.Location = New System.Drawing.Point(80, 64)
    Me.btnEndpointRemove.Name = "btnEndpointRemove"
    Me.btnEndpointRemove.Size = New System.Drawing.Size(56, 24)
    Me.btnEndpointRemove.TabIndex = 13
    Me.btnEndpointRemove.Text = "Remove"
    '
    'btnEndpointModify
    '
    Me.btnEndpointModify.Location = New System.Drawing.Point(152, 64)
    Me.btnEndpointModify.Name = "btnEndpointModify"
    Me.btnEndpointModify.Size = New System.Drawing.Size(48, 24)
    Me.btnEndpointModify.TabIndex = 12
    Me.btnEndpointModify.Text = "Edit"
    '
    'btnEndpointAdd
    '
    Me.btnEndpointAdd.Location = New System.Drawing.Point(16, 64)
    Me.btnEndpointAdd.Name = "btnEndpointAdd"
    Me.btnEndpointAdd.Size = New System.Drawing.Size(48, 24)
    Me.btnEndpointAdd.TabIndex = 11
    Me.btnEndpointAdd.Text = "Add"
    '
    'tabResults
    '
    Me.tabResults.Controls.Add(Me.agdResults)
    Me.tabResults.Location = New System.Drawing.Point(4, 22)
    Me.tabResults.Name = "tabResults"
    Me.tabResults.Size = New System.Drawing.Size(288, 238)
    Me.tabResults.TabIndex = 2
    Me.tabResults.Text = "Results"
    '
    'agdResults
    '
    Me.agdResults.AllowHorizontalScrolling = True
    Me.agdResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdResults.CellBackColor = System.Drawing.Color.Empty
    Me.agdResults.LineColor = System.Drawing.Color.Empty
    Me.agdResults.LineWidth = 0.0!
    Me.agdResults.Location = New System.Drawing.Point(8, 43)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(272, 189)
    Me.agdResults.Source = Nothing
    Me.agdResults.TabIndex = 8
    '
    'tabPivot
    '
    Me.tabPivot.Controls.Add(Me.agdPivot)
    Me.tabPivot.Controls.Add(Me.lblPivotColumns)
    Me.tabPivot.Controls.Add(Me.lblPivotCells)
    Me.tabPivot.Controls.Add(Me.lblPivotRows)
    Me.tabPivot.Controls.Add(Me.cboPivotCells)
    Me.tabPivot.Controls.Add(Me.cboPivotColumns)
    Me.tabPivot.Controls.Add(Me.cboPivotRows)
    Me.tabPivot.Location = New System.Drawing.Point(4, 22)
    Me.tabPivot.Name = "tabPivot"
    Me.tabPivot.Size = New System.Drawing.Size(288, 238)
    Me.tabPivot.TabIndex = 3
    Me.tabPivot.Text = "Pivot"
    '
    'agdPivot
    '
    Me.agdPivot.AllowHorizontalScrolling = True
    Me.agdPivot.CellBackColor = System.Drawing.Color.Empty
    Me.agdPivot.LineColor = System.Drawing.Color.Empty
    Me.agdPivot.LineWidth = 0.0!
    Me.agdPivot.Location = New System.Drawing.Point(8, 80)
    Me.agdPivot.Name = "agdPivot"
    Me.agdPivot.Size = New System.Drawing.Size(272, 152)
    Me.agdPivot.Source = Nothing
    Me.agdPivot.TabIndex = 7
    '
    'lblPivotColumns
    '
    Me.lblPivotColumns.Location = New System.Drawing.Point(8, 32)
    Me.lblPivotColumns.Name = "lblPivotColumns"
    Me.lblPivotColumns.Size = New System.Drawing.Size(64, 16)
    Me.lblPivotColumns.TabIndex = 6
    Me.lblPivotColumns.Text = "Columns"
    Me.lblPivotColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblPivotCells
    '
    Me.lblPivotCells.Location = New System.Drawing.Point(8, 56)
    Me.lblPivotCells.Name = "lblPivotCells"
    Me.lblPivotCells.Size = New System.Drawing.Size(64, 16)
    Me.lblPivotCells.TabIndex = 5
    Me.lblPivotCells.Text = "Cells"
    Me.lblPivotCells.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblPivotRows
    '
    Me.lblPivotRows.Location = New System.Drawing.Point(8, 8)
    Me.lblPivotRows.Name = "lblPivotRows"
    Me.lblPivotRows.Size = New System.Drawing.Size(64, 16)
    Me.lblPivotRows.TabIndex = 4
    Me.lblPivotRows.Text = "Rows"
    Me.lblPivotRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboPivotCells
    '
    Me.cboPivotCells.Location = New System.Drawing.Point(80, 56)
    Me.cboPivotCells.Name = "cboPivotCells"
    Me.cboPivotCells.Size = New System.Drawing.Size(128, 21)
    Me.cboPivotCells.TabIndex = 3
    '
    'cboPivotColumns
    '
    Me.cboPivotColumns.Location = New System.Drawing.Point(80, 32)
    Me.cboPivotColumns.Name = "cboPivotColumns"
    Me.cboPivotColumns.Size = New System.Drawing.Size(128, 21)
    Me.cboPivotColumns.TabIndex = 2
    '
    'cboPivotRows
    '
    Me.cboPivotRows.Location = New System.Drawing.Point(80, 8)
    Me.cboPivotRows.Name = "cboPivotRows"
    Me.cboPivotRows.Size = New System.Drawing.Size(128, 21)
    Me.cboPivotRows.TabIndex = 1
    '
    'btnStart
    '
    Me.btnStart.Location = New System.Drawing.Point(8, 8)
    Me.btnStart.Name = "btnStart"
    Me.btnStart.Size = New System.Drawing.Size(56, 24)
    Me.btnStart.TabIndex = 1
    Me.btnStart.Text = "Start"
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.MenuItem1})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLoadResults, Me.mnuSaveResults, Me.mnuSavePivot, Me.mnuFileSep1, Me.mnuLoadVariations, Me.mnuSaveVariations})
    Me.mnuFile.Text = "File"
    '
    'mnuLoadResults
    '
    Me.mnuLoadResults.Index = 0
    Me.mnuLoadResults.Text = "Load Results"
    '
    'mnuSaveResults
    '
    Me.mnuSaveResults.Index = 1
    Me.mnuSaveResults.Text = "Save Results"
    '
    'mnuSavePivot
    '
    Me.mnuSavePivot.Index = 2
    Me.mnuSavePivot.Text = "Save Pivot"
    '
    'mnuFileSep1
    '
    Me.mnuFileSep1.Index = 3
    Me.mnuFileSep1.Text = "-"
    '
    'mnuLoadVariations
    '
    Me.mnuLoadVariations.Index = 4
    Me.mnuLoadVariations.Text = "Load Variations"
    '
    'mnuSaveVariations
    '
    Me.mnuSaveVariations.Index = 5
    Me.mnuSaveVariations.Text = "Save Variations"
    '
    'MenuItem1
    '
    Me.MenuItem1.Index = 1
    Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCopyResults, Me.mnuCopyPivot, Me.mnuPasteResults})
    Me.MenuItem1.Text = "Edit"
    '
    'mnuCopyResults
    '
    Me.mnuCopyResults.Index = 0
    Me.mnuCopyResults.Text = "Copy Results"
    '
    'mnuCopyPivot
    '
    Me.mnuCopyPivot.Index = 1
    Me.mnuCopyPivot.Text = "Copy Pivot"
    '
    'mnuPasteResults
    '
    Me.mnuPasteResults.Index = 2
    Me.mnuPasteResults.Text = "Paste Results"
    '
    'lblTop
    '
    Me.lblTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblTop.Location = New System.Drawing.Point(72, 8)
    Me.lblTop.Name = "lblTop"
    Me.lblTop.Size = New System.Drawing.Size(216, 24)
    Me.lblTop.TabIndex = 2
    Me.lblTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'frmIterative
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(296, 301)
    Me.Controls.Add(Me.lblTop)
    Me.Controls.Add(Me.btnStart)
    Me.Controls.Add(Me.myTabs)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmIterative"
    Me.Text = "Iterative Analysis"
    Me.myTabs.ResumeLayout(False)
    Me.tabInputs.ResumeLayout(False)
    Me.tabEndpoints.ResumeLayout(False)
    Me.tabResults.ResumeLayout(False)
    Me.tabPivot.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  'all the variations listed in the Input tab
  Private pInputs As atcCollection

  'all the endpoints listed in the Endpoints tab
  Private pEndpoints As atcCollection

  Private InputArgumentPrefix As String = "Current Value for "
  Private ResultsTabIndex As Integer = 2

  'Names of attributes to show in Results grid
  'Private pOutputAttributes As String() = {"Mean", "Mean", "Mean", "Min", "Max", "1Hi100", "7Q10"}
  'Names of constituents to show attributes of in Results grid
  'Private pOutputConsName() As String = {"ATMP", "HPREC", "Flow", "Flow", "Flow", "Flow", "Flow"}

  'Parameters for Hammond - TODO: don't hard code these
  Private pDegF As Boolean = True
  Private pLatDeg As Double = 39
  Private pCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}

  Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcDataGroup = Nothing)
    pInputs = New atcCollection
    pEndpoints = New atcCollection
    Me.Show()
  End Sub

  Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
    Dim lWDMFileName As String = "base.wdm"
    Dim lVariations As atcCollection = New atcCollection
    Dim lRuns As Integer = 0

    UpdateTopLabel("Setting up to run")

    'group containing dsn 122 (hourly AirTemp) from base.wdm
    'Dim lOrigAirTmp As New atcDataGroup
    'lOrigAirTmp = g_DataManager.DataSets.FindData("ID", "122", 0)
    'If lOrigAirTmp.Count = 0 Then
    '  Dim lWDMfile As atcDataSource = New atcWDM.atcDataSourceWDM
    '  g_DataManager.OpenDataSource(lWDMfile, lWDMFileName, Nothing)
    '  lOrigAirTmp = lWDMfile.DataSets.FindData("ID", "122", 0)
    'End If
    'lSummary.Save(g_DataManager, lOrigAirTmp, "DataTree_OrigAirTmp.txt")

    'Make a collection of the variations that are selected/checked in lstInputs
    For Each iVariation As Integer In lstInputs.CheckedIndices
      lVariations.Add(pInputs.ItemByIndex(iVariation))
    Next

    'header for attributes
    agdResults.Source = New atcGridSource
    With agdResults.Source
      .FixedRows = 1
      .Columns = pEndpoints.Count + 1
      .Rows = 2
      .CellValue(0, 0) = "Run"
      .CellColor(0, 0) = Drawing.SystemColors.Control
      .ColorCells = True
      For lIndex As Integer = 0 To pEndpoints.Count - 1
        Dim lEndpoint As Variation = pEndpoints.ItemByIndex(lIndex)
        .CellValue(0, lIndex + 1) = lEndpoint.Name
        .CellColor(0, lIndex + 1) = Drawing.SystemColors.Control
      Next
    End With
    agdResults.Initialize(agdResults.Source)
    agdResults.Refresh()
    PopulatePivotCombos()

    myTabs.SelectedIndex = ResultsTabIndex

    Run(txtModifiedScenarioName.Text, _
        lVariations, _
        lWDMFileName, _
        lRuns, 0, Nothing)

    UpdateTopLabel("Finished with " & lRuns & " runs")
  End Sub

  Private Sub Run(ByVal aModifiedScenarioName As String, _
                  ByVal aVariations As atcCollection, _
                  ByVal aBaseWDMFileName As String, _
                  ByRef aIteration As Integer, _
                  ByRef aStartVariation As Integer, _
                  ByRef aModifiedData As atcDataGroup)
    Dim pTestPath As String = "c:\test\climate\"
    Logger.Dbg("Start")
    ChDriveDir(pTestPath)
    Logger.Dbg(" CurDir:" & CurDir())

    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
    Dim lMetCmp As New atcMetCmp.atcMetCmpPlugin
    Dim lArgsMath As New atcDataAttributes

    Dim lRow As Integer = 0

    If aStartVariation >= aVariations.Count Then 'All variations have values, do a model run
      UpdateTopLabel(aIteration)
      Dim lScenarioResults As atcDataSource
      lScenarioResults = ScenarioRun(aBaseWDMFileName, aModifiedScenarioName, aModifiedData)

      UpdateResults(aIteration, lScenarioResults.DataSets)

      aIteration += 1

    Else 'Need to loop through values for next variation
      Dim lVariation As Variation = aVariations.ItemByIndex(aStartVariation)
      With lVariation
        For .CurrentValue = .Min To .Max Step .Increment
          If aModifiedData Is Nothing Then aModifiedData = New atcDataGroup
          Dim lModifiedTS As atcTimeseries
          Dim lLocalModifiedTS As New atcDataGroup
          For Each lOriginalData As atcDataSet In .DataSets
            lTsMath.DataSets.Clear()
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lOriginalData)
            lArgsMath.SetValue("Number", .CurrentValue)
            g_DataManager.OpenDataSource(lTsMath, .Operation, lArgsMath)

            lModifiedTS = lTsMath.DataSets(0)
            lLocalModifiedTS.Add(lModifiedTS)

            Select Case .DataSets.ItemByIndex(0).Attributes.GetValue("Constituent").ToString.ToUpper
              Case "ATMP", "AIRTMP", "AIRTEMP" 'recompute PET when ATMP is changed - TODO: don't hard code ATMP
                'Dim lAirTmpMean As String = Format(lModifiedTS.Attributes.GetValue("Mean"), "#.00")
                lModifiedTS = atcMetCmp.CmpHamX(lModifiedTS, Nothing, pDegF, pLatDeg, pCTS)
                lLocalModifiedTS.Add(lModifiedTS)
                'Dim lEvapMean As String = Format(lModifiedTS.Attributes.GetValue("Mean") * 365.25, "#.00")
                With lModifiedTS.Attributes
                  .SetValue("Constituent", "PET")
                  .SetValue("Id", 111)
                  .SetValue("Scenario", aModifiedScenarioName)
                End With
            End Select

            aModifiedData.Add(lLocalModifiedTS)

            'We have handled a variation, now handle more input variations or to run the model
            Run(aModifiedScenarioName, _
                aVariations, _
                aBaseWDMFileName, _
                aIteration, _
                aStartVariation + 1, _
                aModifiedData)

            aModifiedData.Remove(lLocalModifiedTS)

            '  lNewPrec = aBase1.Clone
            '  For iValue As Integer = 1 To lNewPrec.numValues
            '    Dim lDate As Date = Date.FromOADate(lNewPrec.Dates.Value(iValue))
            '    Select Case lDate.Month
            '      Case 8, 9 : lNewPrec.Value(iValue) *= lCurValue1
            '    End Select
            '  Next
            '  lNewPrec.Attributes.CalculateAll()
            '  'TODO: REPLACE HARD CODED 10 YEARS WITH ACTUAL SPAN CALC!
            '  agdResults.Source.CellValue(lRow, 4) = Format(lNewPrec.Attributes.GetValue("Sum") / 10, "#.00")
            '  'lSummary.Save(g_DataManager, New atcDataGroup(lNewPrec), "DataTree_Precip" & lCurValue1 & ".txt")

            '  If aModifiedData Is Nothing Then aModifiedData = New atcDataGroup
            '  aModifiedData.Add(lNewPrec)
            '  lModifiedData.Add(lNewEvap)
            '  Windows.Forms.Application.DoEvents()
            'Next

          Next
        Next
      End With
    End If
  End Sub

  Private Sub UpdateResults(ByVal aIteration As Integer, ByVal aData As atcDataGroup)
    With agdResults.Source
      Dim lRow As Integer = aIteration + .FixedRows
      Dim lColumn As Integer = .FixedRows
      .CellValue(lRow, 0) = aIteration + 1
      For Each lEndpoint As Variation In pEndpoints
        If Not Double.IsNaN(lEndpoint.CurrentValue) Then 'This is an input variation, put current value in results
          .CellValue(lRow, lColumn) = Format(lEndpoint.CurrentValue, "0.####")
          lColumn += 1
        Else
          For Each lOldData As atcDataSet In lEndpoint.DataSets
            Dim lGroup As atcDataGroup = aData.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
            If lGroup.Count > 0 Then
              .CellValue(lRow, lColumn) = lGroup.Item(0).Attributes.GetFormattedValue(lEndpoint.Operation)
              If .ColorCells Then
                If Not IsNumeric(.CellValue(lRow, lColumn)) Then
                  .CellColor(lRow, lColumn) = lEndpoint.ColorDefault
                Else
                  Dim lValue As Double = lGroup.Item(0).Attributes.GetValue(lEndpoint.Operation)
                  If Not Double.IsNaN(lEndpoint.Min) AndAlso lValue < lEndpoint.Min Then
                    .CellColor(lRow, lColumn) = lEndpoint.ColorBelowMin
                  ElseIf Not Double.IsNaN(lEndpoint.Max) AndAlso lValue > lEndpoint.Max Then
                    .CellColor(lRow, lColumn) = lEndpoint.ColorAboveMax
                  Else
                    .CellColor(lRow, lColumn) = lEndpoint.ColorDefault
                  End If
                End If
              End If
            Else
              .CellValue(lRow, lColumn) = ""
            End If
            lColumn += 1
          Next
        End If
      Next
    End With
    agdResults.Refresh()
    Windows.Forms.Application.DoEvents()
  End Sub

  Private Sub cboPivotRows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPivotRows.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub cboPivotColumns_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPivotColumns.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub cboPivotCells_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPivotCells.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub PopulatePivotCombos()
    cboPivotRows.Items.Clear()
    cboPivotColumns.Items.Clear()
    cboPivotCells.Items.Clear()

    If Not agdResults.Source Is Nothing Then
      For iColumn As Integer = 0 To agdResults.Source.Columns - 1
        Dim lColumnTitle As String = agdResults.Source.CellValue(0, iColumn)
        cboPivotRows.Items.Add(lColumnTitle)
        cboPivotColumns.Items.Add(lColumnTitle)
        cboPivotCells.Items.Add(lColumnTitle)
        'default to last entry for a constituent 
        'TODO: make smarter
        If lColumnTitle.ToLower.StartsWith("prec") Then
          cboPivotColumns.Text = lColumnTitle
        ElseIf lColumnTitle.ToLower.StartsWith("air") Then
          cboPivotRows.Text = lColumnTitle
        ElseIf lColumnTitle.ToLower.StartsWith("flow") Then
          cboPivotCells.Text = lColumnTitle
        End If
      Next
    End If
  End Sub

  Private Function UniqueValuesInColumn(ByVal aSource As atcGridSource, ByVal aColumn As Integer) As ArrayList
    Dim lRows As Integer = aSource.Rows - 1
    Dim lValues As New ArrayList
    Dim lCheckValue As String
    Dim lMatch As Boolean
    For lrow As Integer = 1 To lRows
      Dim lValue As String = aSource.CellValue(lrow, aColumn)
      lMatch = False
      For Each lCheckValue In lValues
        If lCheckValue.Equals(lValue) Then lMatch = True : Exit For
      Next
      If Not lMatch Then
        lValues.Add(lValue)
      End If
    Next
    Return lValues
  End Function

  'Find the row of the table containing the specified values in the specified columns
  Private Function FindMatchingRow(ByVal aSource As atcGridSource, _
                                   ByVal aCol1 As Integer, ByVal aCol1Value As String, _
                                   ByVal aCol2 As Integer, ByVal aCol2Value As String) As Integer
    For lRow As Integer = 1 To aSource.Rows - 1
      If aSource.CellValue(lRow, aCol1).Equals(aCol1Value) AndAlso _
         aSource.CellValue(lRow, aCol2).Equals(aCol2Value) Then
        Return lRow
      End If
    Next
    Return -1
  End Function

  Private Sub PopulatePivotTable()
    If cboPivotRows.Text.Length > 0 AndAlso cboPivotColumns.Text.Length > 0 AndAlso cboPivotCells.Text.Length > 0 Then
      Dim lPivotData As New atcGridSource
      Dim lRuns As Integer = agdResults.Source.Rows - 1
      Dim lRunRow As Integer = 0
      Dim lRunColumn As Integer = 0
      Dim lRunCell As Integer = 0

      While Not agdResults.Source.CellValue(0, lRunRow).Equals(cboPivotRows.Text)
        lRunRow += 1
      End While
      While Not agdResults.Source.CellValue(0, lRunColumn).Equals(cboPivotColumns.Text)
        lRunColumn += 1
      End While
      While Not agdResults.Source.CellValue(0, lRunCell).Equals(cboPivotCells.Text)
        lRunCell += 1
      End While

      With lPivotData
        Dim lRowValues As ArrayList = UniqueValuesInColumn(agdResults.Source, lRunRow)
        Dim lColumnValues As ArrayList = UniqueValuesInColumn(agdResults.Source, lRunColumn)
        Dim lRow As Integer
        Dim lColumn As Integer
        Dim lRowValue As String
        Dim lColumnValue As String
        Dim lMatchRow As Integer

        .Rows = lRowValues.Count + 1
        .Columns = lColumnValues.Count + 1
        .FixedRows = 1
        .FixedColumns = 1
        .ColorCells = True
        For lRow = 0 To .Rows - 1
          .CellColor(lRow, 0) = System.Drawing.SystemColors.Control
        Next
        For lColumn = 1 To .Columns - 1
          .CellColor(0, lColumn) = System.Drawing.SystemColors.Control
        Next

        For lRow = 1 To .Rows - 1
          lRowValue = lRowValues(lRow - 1)
          .CellValue(lRow, 0) = lRowValue
          For lColumn = 1 To .Columns - 1
            lColumnValue = lColumnValues(lColumn - 1)
            .CellValue(0, lColumn) = lColumnValue
            lMatchRow = FindMatchingRow(agdResults.Source, lRunRow, lRowValue, lRunColumn, lColumnValue)
            If lMatchRow > 0 Then
              .CellValue(lRow, lColumn) = agdResults.Source.CellValue(lMatchRow, lRunCell)
              .CellColor(lRow, lColumn) = agdResults.Source.CellColor(lMatchRow, lRunCell)
            End If
          Next
        Next
        ' agdResults.Source.CellValue(lRow, lRunRow)
      End With

      agdPivot.Initialize(lPivotData)
      agdPivot.Visible = True
      agdPivot.Refresh()
    Else
      agdPivot.Visible = False
    End If
  End Sub

  Private Sub PopulateResultsGrid(ByVal aNewContentsOfGrid As String)
    With agdResults
      .Source = Nothing
      .Source = New atcGridSource
      .Source.FromString(aNewContentsOfGrid)
      .Initialize(.Source)
      .SizeAllColumnsToContents(.Width, True)
      .Refresh()
    End With
    PopulatePivotCombos()
  End Sub

  Private Sub mnuLoadResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLoadResults.Click
    Dim lOpenDialog As New Windows.Forms.OpenFileDialog
    With lOpenDialog
      .FileName = "results.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Scenario Builder - Load Results"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If FileExists(.FileName) Then 'read file into grid
          PopulateResultsGrid(WholeFileString(.FileName))
          myTabs.SelectedIndex = ResultsTabIndex
        End If
      End If
    End With
  End Sub

  Private Sub mnuPasteResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPasteResults.Click
    Dim iData As IDataObject = Clipboard.GetDataObject()
    If iData.GetDataPresent(DataFormats.Text) Then
      PopulateResultsGrid(CType(iData.GetData(DataFormats.Text), String))
    Else
      Logger.Msg("No text on clipboard to paste into grid", "Paste")
    End If
    myTabs.SelectedIndex = ResultsTabIndex
  End Sub

  Private Sub frmMultipleResults_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    agdResults.SizeAllColumnsToContents(agdResults.Width, True)
    agdPivot.SizeAllColumnsToContents(agdPivot.Width, True)
  End Sub

  Private Sub mnuCopyResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuCopyResults.Click
    Clipboard.SetDataObject(agdResults.ToString)
  End Sub

  Private Sub mnuCopyPivot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuCopyPivot.Click
    Clipboard.SetDataObject(agdPivot.ToString)
  End Sub

  Private Sub mnuSaveResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSaveResults.Click
    Dim lSaveDialog As New Windows.Forms.SaveFileDialog
    With lSaveDialog
      .FileName = "results.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Save Results as Tab-Delimited Text"
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'write file from grid contents
        SaveFileString(.FileName, agdResults.Source.ToString)
      End If
    End With
  End Sub

  Private Sub mnuSavePivot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSavePivot.Click
    Dim lSaveDialog As New Windows.Forms.SaveFileDialog
    With lSaveDialog
      .FileName = "pivot.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Save Pivot Table as Tab-Delimited Text"
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'write file from grid contents
        SaveFileString(.FileName, lblPivotRows.Text & vbTab & cboPivotRows.Text & vbCrLf _
                                & lblPivotColumns.Text & vbTab & cboPivotColumns.Text & vbCrLf _
                                & lblPivotCells.Text & vbTab & cboPivotCells.Text & vbCrLf _
                                & agdPivot.Source.ToString)
      End If
    End With
  End Sub

  Private Sub UpdateTopLabel(ByVal aIteration As Integer)
    UpdateTopLabel("Run # " & aIteration + 1)
  End Sub

  Private Sub UpdateTopLabel(ByVal aText As String)
    lblTop.Text = aText
    lblTop.Refresh()
    Application.DoEvents()
  End Sub

  Private Sub btnInputAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputAdd.Click
    Dim frmVary As New frmVariation
    Dim lVariation As New Variation
    With lVariation
      .Name = ""
      .ComputationSource = New atcTimeseriesMath.atcTimeseriesMath
      .Operation = "Multiply"
      .Min = 0.9
      .Max = 1.1
      .Increment = 0.1
    End With
    lVariation = frmVary.AskUser(lVariation)
    If Not lVariation Is Nothing Then
      pInputs.Add(lVariation)
      lstInputs.Items.Add(lVariation.ToString)
      lstInputs.SetItemChecked(lstInputs.Items.Count - 1, True)

      pEndpoints.Add(lVariation)
      lstEndpoints.Items.Add(InputArgumentPrefix & lVariation.Name)
    End If
  End Sub

  Private Sub btnInputModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputModify.Click
    Dim lIndex As Integer = lstInputs.SelectedIndex
    If lIndex >= 0 And lIndex < pInputs.Count Then
      Dim lVariation As Variation = pInputs.ItemByIndex(lIndex)
      Dim frmVary As New frmVariation
      lVariation = frmVary.AskUser(lVariation)
      If Not lVariation Is Nothing Then
        pInputs.RemoveAt(lIndex)
        pInputs.Insert(lIndex, lVariation)
        lstInputs.Items.RemoveAt(lIndex)
        lstInputs.Items.Insert(lIndex, lVariation.ToString)
      End If
    End If
  End Sub

  Private Sub btnInputRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputRemove.Click
    For Each lIndex As Integer In lstInputs.SelectedIndices
      pInputs.RemoveAt(lIndex)
      lstInputs.Items.RemoveAt(lIndex)
    Next
  End Sub

  Private Sub btnEndpointAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEndpointAdd.Click
    Dim frmEnd As New frmEndpoint
    Dim lVariation As New Variation
    With lVariation
      .Name = ""
      .Operation = "Mean"
      .Min = Double.NaN
      .Max = Double.NaN
      .Increment = Double.NaN
    End With
    lVariation = frmEnd.AskUser(lVariation)
    If Not lVariation Is Nothing Then
      pEndpoints.Add(lVariation)
      lstEndpoints.Items.Add(lVariation.ToString)
      lstEndpoints.SetItemChecked(lstEndpoints.Items.Count - 1, True)
    End If
  End Sub

  Private Sub btnEndpointModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointModify.Click
    Dim lIndex As Integer = lstEndpoints.SelectedIndex
    If lIndex >= 0 And lIndex < pEndpoints.Count Then
      Dim lVariation As Variation = pEndpoints.ItemByIndex(lIndex)
      Dim frmEnd As New frmEndpoint
      lVariation = frmEnd.AskUser(lVariation)
      If Not lVariation Is Nothing Then
        pEndpoints.RemoveAt(lIndex)
        pEndpoints.Insert(lIndex, lVariation)
        lstEndpoints.Items.RemoveAt(lIndex)
        lstEndpoints.Items.Insert(lIndex, lVariation.ToString)
      End If
    End If
  End Sub

  Private Sub btnEndpointRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointRemove.Click
    For Each lIndex As Integer In lstEndpoints.SelectedIndices
      pEndpoints.RemoveAt(lIndex)
      lstEndpoints.Items.RemoveAt(lIndex)
    Next
  End Sub

  Private Sub UpdateList(ByVal aContents As atcCollection, ByVal aList As Windows.Forms.ListBox)
    aList.Items.Clear()
    For Each lVariation As Variation In aContents
      aList.Items.Add(lVariation.ToString)
    Next
  End Sub

  Private Sub mnuSaveVariations_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSaveVariations.Click
    Dim lSaveDialog As New Windows.Forms.SaveFileDialog
    With lSaveDialog
      .FileName = "Variations.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Save Variations as XML Text"
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'write file from grid contents
        Dim lXML As String = "<Variations>" & vbCrLf
        For Each lVariation As Variation In pInputs
          lXML &= lVariation.XML
        Next
        lXML &= "</Variations>" & vbCrLf
        SaveFileString(.FileName, lXML)
      End If
    End With
  End Sub

  Private Sub mnuLoadVariations_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuLoadVariations.Click
    Dim lOpenDialog As New Windows.Forms.OpenFileDialog
    With lOpenDialog
      .FileName = "Variations.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Scenario Builder - Load Variations"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If FileExists(.FileName) Then
          Dim lXML As New Chilkat.Xml
          If lXML.LoadXml(WholeFileString(.FileName)) Then
            If lXML.Tag.ToLower.Equals("variations") Then
              If lXML.FirstChild2 Then
                Dim lVariation As Variation
                pInputs.Clear()
                lstInputs.Items.Clear()
                Do
                  lVariation = New Variation
                  lVariation.XML = lXML.GetXml
                  pInputs.Add(lVariation)
                  lstInputs.Items.Add(lVariation.ToString)
                Loop While lXML.NextSibling2

                'Remove endpoint references to old inputs
                Dim iEndpoint As Integer = 0
                While iEndpoint < lstEndpoints.Items.Count
                  If lstEndpoints.Items(iEndpoint).ToString.StartsWith(InputArgumentPrefix) Then
                    lstEndpoints.Items.RemoveAt(iEndpoint)
                    pEndpoints.RemoveAt(iEndpoint)
                  Else
                    iEndpoint += 1
                  End If
                End While

                'Add endpoint references to new inputs
                For Each lVariation In pInputs
                  pEndpoints.Add(lVariation)
                  lstEndpoints.Items.Add(InputArgumentPrefix & lVariation.Name)
                Next

              End If
            Else
              Logger.Msg("Variations not found in '" & .FileName & "'" & vbCrLf & lXML.LastErrorText, "Load Variations")
            End If
          Else
            Logger.Msg("Could not parse variations from '" & .FileName & "'" & vbCrLf & lXML.LastErrorText, "Load Variations")
          End If
        End If
      End If
    End With
  End Sub

End Class
