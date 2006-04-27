Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Public Class frmCAT
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
  Friend WithEvents btnStop As System.Windows.Forms.Button
  Friend WithEvents lblAllResults As System.Windows.Forms.Label
  Friend WithEvents chkSaveAll As System.Windows.Forms.CheckBox
  Friend WithEvents mnuPivotHeaders As System.Windows.Forms.MenuItem
  Friend WithEvents mnuOptions As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSep2 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents btnInputAddCligen As System.Windows.Forms.Button
  Friend WithEvents btnEndpointAddCligen As System.Windows.Forms.Button
  Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmCAT))
    Me.myTabs = New System.Windows.Forms.TabControl
    Me.tabInputs = New System.Windows.Forms.TabPage
    Me.btnInputAddCligen = New System.Windows.Forms.Button
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
    Me.btnEndpointAddCligen = New System.Windows.Forms.Button
    Me.lblAllResults = New System.Windows.Forms.Label
    Me.chkSaveAll = New System.Windows.Forms.CheckBox
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
    Me.mnuLoadVariations = New System.Windows.Forms.MenuItem
    Me.mnuSaveVariations = New System.Windows.Forms.MenuItem
    Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
    Me.mnuLoadResults = New System.Windows.Forms.MenuItem
    Me.mnuSaveResults = New System.Windows.Forms.MenuItem
    Me.mnuFileSep2 = New System.Windows.Forms.MenuItem
    Me.mnuSavePivot = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuCopyResults = New System.Windows.Forms.MenuItem
    Me.mnuCopyPivot = New System.Windows.Forms.MenuItem
    Me.mnuPasteResults = New System.Windows.Forms.MenuItem
    Me.mnuOptions = New System.Windows.Forms.MenuItem
    Me.mnuPivotHeaders = New System.Windows.Forms.MenuItem
    Me.lblTop = New System.Windows.Forms.Label
    Me.btnStop = New System.Windows.Forms.Button
    Me.mnuHelp = New System.Windows.Forms.MenuItem
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
    Me.myTabs.Location = New System.Drawing.Point(0, 46)
    Me.myTabs.Name = "myTabs"
    Me.myTabs.SelectedIndex = 0
    Me.myTabs.Size = New System.Drawing.Size(394, 305)
    Me.myTabs.TabIndex = 0
    '
    'tabInputs
    '
    Me.tabInputs.Controls.Add(Me.btnInputAddCligen)
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
    Me.tabInputs.Location = New System.Drawing.Point(4, 25)
    Me.tabInputs.Name = "tabInputs"
    Me.tabInputs.Size = New System.Drawing.Size(386, 276)
    Me.tabInputs.TabIndex = 0
    Me.tabInputs.Text = "Inputs"
    '
    'btnInputAddCligen
    '
    Me.btnInputAddCligen.Location = New System.Drawing.Point(77, 74)
    Me.btnInputAddCligen.Name = "btnInputAddCligen"
    Me.btnInputAddCligen.Size = New System.Drawing.Size(57, 28)
    Me.btnInputAddCligen.TabIndex = 11
    Me.btnInputAddCligen.Text = "Cligen"
    '
    'lstInputs
    '
    Me.lstInputs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstInputs.Location = New System.Drawing.Point(10, 111)
    Me.lstInputs.Name = "lstInputs"
    Me.lstInputs.Size = New System.Drawing.Size(444, 191)
    Me.lstInputs.TabIndex = 10
    '
    'btnInputDown
    '
    Me.btnInputDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnInputDown.Location = New System.Drawing.Point(425, 74)
    Me.btnInputDown.Name = "btnInputDown"
    Me.btnInputDown.Size = New System.Drawing.Size(29, 28)
    Me.btnInputDown.TabIndex = 9
    Me.btnInputDown.Text = "v"
    '
    'btnInputUp
    '
    Me.btnInputUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnInputUp.Location = New System.Drawing.Point(386, 74)
    Me.btnInputUp.Name = "btnInputUp"
    Me.btnInputUp.Size = New System.Drawing.Size(29, 28)
    Me.btnInputUp.TabIndex = 8
    Me.btnInputUp.Text = "^"
    '
    'btnInputRemove
    '
    Me.btnInputRemove.Location = New System.Drawing.Point(144, 74)
    Me.btnInputRemove.Name = "btnInputRemove"
    Me.btnInputRemove.Size = New System.Drawing.Size(67, 28)
    Me.btnInputRemove.TabIndex = 6
    Me.btnInputRemove.Text = "Remove"
    '
    'btnInputModify
    '
    Me.btnInputModify.Location = New System.Drawing.Point(221, 74)
    Me.btnInputModify.Name = "btnInputModify"
    Me.btnInputModify.Size = New System.Drawing.Size(57, 28)
    Me.btnInputModify.TabIndex = 7
    Me.btnInputModify.Text = "Edit"
    '
    'btnInputAdd
    '
    Me.btnInputAdd.Location = New System.Drawing.Point(10, 74)
    Me.btnInputAdd.Name = "btnInputAdd"
    Me.btnInputAdd.Size = New System.Drawing.Size(57, 28)
    Me.btnInputAdd.TabIndex = 5
    Me.btnInputAdd.Text = "Add"
    '
    'txtModifiedScenarioName
    '
    Me.txtModifiedScenarioName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtModifiedScenarioName.Location = New System.Drawing.Point(115, 37)
    Me.txtModifiedScenarioName.Name = "txtModifiedScenarioName"
    Me.txtModifiedScenarioName.Size = New System.Drawing.Size(339, 22)
    Me.txtModifiedScenarioName.TabIndex = 4
    Me.txtModifiedScenarioName.Text = "Modified"
    '
    'cboBaseScenarioName
    '
    Me.cboBaseScenarioName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboBaseScenarioName.Location = New System.Drawing.Point(115, 9)
    Me.cboBaseScenarioName.Name = "cboBaseScenarioName"
    Me.cboBaseScenarioName.Size = New System.Drawing.Size(339, 24)
    Me.cboBaseScenarioName.TabIndex = 2
    '
    'lblBaseScenarioName
    '
    Me.lblBaseScenarioName.Location = New System.Drawing.Point(10, 9)
    Me.lblBaseScenarioName.Name = "lblBaseScenarioName"
    Me.lblBaseScenarioName.Size = New System.Drawing.Size(96, 19)
    Me.lblBaseScenarioName.TabIndex = 0
    Me.lblBaseScenarioName.Text = "Base Scenario"
    '
    'lblNewScenarioName
    '
    Me.lblNewScenarioName.Location = New System.Drawing.Point(10, 37)
    Me.lblNewScenarioName.Name = "lblNewScenarioName"
    Me.lblNewScenarioName.Size = New System.Drawing.Size(96, 18)
    Me.lblNewScenarioName.TabIndex = 3
    Me.lblNewScenarioName.Text = "New Scenario"
    '
    'tabEndpoints
    '
    Me.tabEndpoints.Controls.Add(Me.btnEndpointAddCligen)
    Me.tabEndpoints.Controls.Add(Me.lblAllResults)
    Me.tabEndpoints.Controls.Add(Me.chkSaveAll)
    Me.tabEndpoints.Controls.Add(Me.lstEndpoints)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointDown)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointUp)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointRemove)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointModify)
    Me.tabEndpoints.Controls.Add(Me.btnEndpointAdd)
    Me.tabEndpoints.Location = New System.Drawing.Point(4, 25)
    Me.tabEndpoints.Name = "tabEndpoints"
    Me.tabEndpoints.Size = New System.Drawing.Size(386, 276)
    Me.tabEndpoints.TabIndex = 1
    Me.tabEndpoints.Text = "Endpoints"
    '
    'btnEndpointAddCligen
    '
    Me.btnEndpointAddCligen.Location = New System.Drawing.Point(77, 74)
    Me.btnEndpointAddCligen.Name = "btnEndpointAddCligen"
    Me.btnEndpointAddCligen.Size = New System.Drawing.Size(57, 28)
    Me.btnEndpointAddCligen.TabIndex = 19
    Me.btnEndpointAddCligen.Text = "Cligen"
    Me.btnEndpointAddCligen.Visible = False
    '
    'lblAllResults
    '
    Me.lblAllResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblAllResults.Location = New System.Drawing.Point(154, 18)
    Me.lblAllResults.Name = "lblAllResults"
    Me.lblAllResults.Size = New System.Drawing.Size(211, 19)
    Me.lblAllResults.TabIndex = 18
    '
    'chkSaveAll
    '
    Me.chkSaveAll.Location = New System.Drawing.Point(19, 18)
    Me.chkSaveAll.Name = "chkSaveAll"
    Me.chkSaveAll.Size = New System.Drawing.Size(135, 19)
    Me.chkSaveAll.TabIndex = 11
    Me.chkSaveAll.Text = "Save All Results"
    '
    'lstEndpoints
    '
    Me.lstEndpoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstEndpoints.Location = New System.Drawing.Point(10, 111)
    Me.lstEndpoints.Name = "lstEndpoints"
    Me.lstEndpoints.Size = New System.Drawing.Size(364, 157)
    Me.lstEndpoints.TabIndex = 17
    '
    'btnEndpointDown
    '
    Me.btnEndpointDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnEndpointDown.Location = New System.Drawing.Point(346, 74)
    Me.btnEndpointDown.Name = "btnEndpointDown"
    Me.btnEndpointDown.Size = New System.Drawing.Size(28, 28)
    Me.btnEndpointDown.TabIndex = 16
    Me.btnEndpointDown.Text = "v"
    '
    'btnEndpointUp
    '
    Me.btnEndpointUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnEndpointUp.Location = New System.Drawing.Point(307, 74)
    Me.btnEndpointUp.Name = "btnEndpointUp"
    Me.btnEndpointUp.Size = New System.Drawing.Size(29, 28)
    Me.btnEndpointUp.TabIndex = 15
    Me.btnEndpointUp.Text = "^"
    '
    'btnEndpointRemove
    '
    Me.btnEndpointRemove.Location = New System.Drawing.Point(144, 74)
    Me.btnEndpointRemove.Name = "btnEndpointRemove"
    Me.btnEndpointRemove.Size = New System.Drawing.Size(67, 28)
    Me.btnEndpointRemove.TabIndex = 13
    Me.btnEndpointRemove.Text = "Remove"
    '
    'btnEndpointModify
    '
    Me.btnEndpointModify.Location = New System.Drawing.Point(221, 74)
    Me.btnEndpointModify.Name = "btnEndpointModify"
    Me.btnEndpointModify.Size = New System.Drawing.Size(57, 28)
    Me.btnEndpointModify.TabIndex = 14
    Me.btnEndpointModify.Text = "Edit"
    '
    'btnEndpointAdd
    '
    Me.btnEndpointAdd.Location = New System.Drawing.Point(10, 74)
    Me.btnEndpointAdd.Name = "btnEndpointAdd"
    Me.btnEndpointAdd.Size = New System.Drawing.Size(57, 28)
    Me.btnEndpointAdd.TabIndex = 12
    Me.btnEndpointAdd.Text = "Add"
    '
    'tabResults
    '
    Me.tabResults.Controls.Add(Me.agdResults)
    Me.tabResults.Location = New System.Drawing.Point(4, 25)
    Me.tabResults.Name = "tabResults"
    Me.tabResults.Size = New System.Drawing.Size(386, 276)
    Me.tabResults.TabIndex = 2
    Me.tabResults.Text = "Results"
    '
    'agdResults
    '
    Me.agdResults.AllowHorizontalScrolling = True
    Me.agdResults.AllowNewValidValues = False
    Me.agdResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdResults.BackColor = System.Drawing.SystemColors.Control
    Me.agdResults.CellBackColor = System.Drawing.Color.Empty
    Me.agdResults.LineColor = System.Drawing.Color.Empty
    Me.agdResults.LineWidth = 0.0!
    Me.agdResults.Location = New System.Drawing.Point(10, 9)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(364, 255)
    Me.agdResults.Source = Nothing
    Me.agdResults.TabIndex = 18
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
    Me.tabPivot.Location = New System.Drawing.Point(4, 25)
    Me.tabPivot.Name = "tabPivot"
    Me.tabPivot.Size = New System.Drawing.Size(386, 276)
    Me.tabPivot.TabIndex = 3
    Me.tabPivot.Text = "Pivot"
    '
    'agdPivot
    '
    Me.agdPivot.AllowHorizontalScrolling = True
    Me.agdPivot.AllowNewValidValues = False
    Me.agdPivot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdPivot.CellBackColor = System.Drawing.Color.Empty
    Me.agdPivot.LineColor = System.Drawing.Color.Empty
    Me.agdPivot.LineWidth = 0.0!
    Me.agdPivot.Location = New System.Drawing.Point(10, 92)
    Me.agdPivot.Name = "agdPivot"
    Me.agdPivot.Size = New System.Drawing.Size(364, 172)
    Me.agdPivot.Source = Nothing
    Me.agdPivot.TabIndex = 25
    '
    'lblPivotColumns
    '
    Me.lblPivotColumns.Location = New System.Drawing.Point(10, 37)
    Me.lblPivotColumns.Name = "lblPivotColumns"
    Me.lblPivotColumns.Size = New System.Drawing.Size(76, 18)
    Me.lblPivotColumns.TabIndex = 21
    Me.lblPivotColumns.Text = "Columns"
    Me.lblPivotColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblPivotCells
    '
    Me.lblPivotCells.Location = New System.Drawing.Point(10, 65)
    Me.lblPivotCells.Name = "lblPivotCells"
    Me.lblPivotCells.Size = New System.Drawing.Size(76, 18)
    Me.lblPivotCells.TabIndex = 23
    Me.lblPivotCells.Text = "Cells"
    Me.lblPivotCells.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'lblPivotRows
    '
    Me.lblPivotRows.Location = New System.Drawing.Point(10, 9)
    Me.lblPivotRows.Name = "lblPivotRows"
    Me.lblPivotRows.Size = New System.Drawing.Size(76, 19)
    Me.lblPivotRows.TabIndex = 19
    Me.lblPivotRows.Text = "Rows"
    Me.lblPivotRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboPivotCells
    '
    Me.cboPivotCells.Location = New System.Drawing.Point(96, 65)
    Me.cboPivotCells.Name = "cboPivotCells"
    Me.cboPivotCells.Size = New System.Drawing.Size(154, 22)
    Me.cboPivotCells.TabIndex = 24
    '
    'cboPivotColumns
    '
    Me.cboPivotColumns.Location = New System.Drawing.Point(96, 37)
    Me.cboPivotColumns.Name = "cboPivotColumns"
    Me.cboPivotColumns.Size = New System.Drawing.Size(154, 22)
    Me.cboPivotColumns.TabIndex = 22
    '
    'cboPivotRows
    '
    Me.cboPivotRows.Location = New System.Drawing.Point(96, 9)
    Me.cboPivotRows.Name = "cboPivotRows"
    Me.cboPivotRows.Size = New System.Drawing.Size(154, 22)
    Me.cboPivotRows.TabIndex = 20
    '
    'btnStart
    '
    Me.btnStart.Location = New System.Drawing.Point(10, 9)
    Me.btnStart.Name = "btnStart"
    Me.btnStart.Size = New System.Drawing.Size(67, 28)
    Me.btnStart.TabIndex = 1
    Me.btnStart.Text = "Start"
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuOptions, Me.mnuHelp})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLoadVariations, Me.mnuSaveVariations, Me.mnuFileSep1, Me.mnuLoadResults, Me.mnuSaveResults, Me.mnuFileSep2, Me.mnuSavePivot})
    Me.mnuFile.Text = "File"
    '
    'mnuLoadVariations
    '
    Me.mnuLoadVariations.Index = 0
    Me.mnuLoadVariations.Text = "Load Inputs and Endpoints"
    '
    'mnuSaveVariations
    '
    Me.mnuSaveVariations.Index = 1
    Me.mnuSaveVariations.Text = "Save Inputs and Endpoints"
    '
    'mnuFileSep1
    '
    Me.mnuFileSep1.Index = 2
    Me.mnuFileSep1.Text = "-"
    '
    'mnuLoadResults
    '
    Me.mnuLoadResults.Index = 3
    Me.mnuLoadResults.Text = "Load Results"
    '
    'mnuSaveResults
    '
    Me.mnuSaveResults.Index = 4
    Me.mnuSaveResults.Text = "Save Results"
    '
    'mnuFileSep2
    '
    Me.mnuFileSep2.Index = 5
    Me.mnuFileSep2.Text = "-"
    '
    'mnuSavePivot
    '
    Me.mnuSavePivot.Index = 6
    Me.mnuSavePivot.Text = "Save Pivot"
    '
    'mnuEdit
    '
    Me.mnuEdit.Index = 1
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuCopyResults, Me.mnuCopyPivot, Me.mnuPasteResults})
    Me.mnuEdit.Text = "Edit"
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
    'mnuOptions
    '
    Me.mnuOptions.Index = 2
    Me.mnuOptions.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuPivotHeaders})
    Me.mnuOptions.Text = "Options"
    '
    'mnuPivotHeaders
    '
    Me.mnuPivotHeaders.Checked = True
    Me.mnuPivotHeaders.Index = 0
    Me.mnuPivotHeaders.Text = "Save/Copy Headers With Pivot"
    '
    'lblTop
    '
    Me.lblTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lblTop.Location = New System.Drawing.Point(86, 9)
    Me.lblTop.Name = "lblTop"
    Me.lblTop.Size = New System.Drawing.Size(298, 28)
    Me.lblTop.TabIndex = 2
    Me.lblTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'btnStop
    '
    Me.btnStop.Location = New System.Drawing.Point(10, 9)
    Me.btnStop.Name = "btnStop"
    Me.btnStop.Size = New System.Drawing.Size(67, 28)
    Me.btnStop.TabIndex = 3
    Me.btnStop.Text = "Stop"
    Me.btnStop.Visible = False
    '
    'mnuHelp
    '
    Me.mnuHelp.Index = 3
    Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
    Me.mnuHelp.ShowShortcut = False
    Me.mnuHelp.Text = "Help"
    '
    'frmCAT
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(393, 347)
    Me.Controls.Add(Me.lblTop)
    Me.Controls.Add(Me.btnStart)
    Me.Controls.Add(Me.myTabs)
    Me.Controls.Add(Me.btnStop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmCAT"
    Me.Text = "Climate Assessment Tool"
    Me.myTabs.ResumeLayout(False)
    Me.tabInputs.ResumeLayout(False)
    Me.tabEndpoints.ResumeLayout(False)
    Me.tabResults.ResumeLayout(False)
    Me.tabPivot.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const CLIGEN_NAME As String = "Cligen"

  'all the variations listed in the Input tab
  Private pInputs As atcCollection
  Private pRunning As Boolean = False

  'all the endpoints listed in the Endpoints tab
  Private pEndpoints As atcCollection

  Private InputArgumentPrefix As String = "Current Modifier for "
  Private ResultsTabIndex As Integer = 2
  Private TotalIterations As Integer = 0
  Private TimePerRun As Double = 0 'Time each run takes in seconds

  Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcDataGroup = Nothing)
    mnuPivotHeaders.Checked = GetSetting("BasinsCAT", "Settings", "PivotHeaders", "Yes").Equals("Yes")
    TimePerRun = CDbl(GetSetting("BasinsCAT", "Settings", "TimePerRun", "0"))
    pInputs = New atcCollection
    pEndpoints = New atcCollection
    For Each lDataSource As atcDataSource In g_DataManager.DataSources
      If lDataSource.Specification.EndsWith(".wdm") Then
        cboBaseScenarioName.Items.Add(lDataSource.Specification)
        cboBaseScenarioName.SelectedIndex = cboBaseScenarioName.Items.Count - 1
      End If
    Next
    Me.Show()
  End Sub

  Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
    Dim lWDMFileName As String = cboBaseScenarioName.Text
    Dim lVariations As atcCollection = New atcCollection
    Dim lRuns As Integer = 0

    pRunning = True
    btnStart.Visible = False
    btnStop.Visible = True

    UpdateTopLabel("Setting up to run")

    RefreshTotalIterations()

    'Make a collection of the variations that are selected/checked in lstInputs
    For Each lVariation As Variation In pInputs
      If lVariation.Selected Then lVariations.Add(lVariation)
    Next

    'header for attributes
    agdResults.Source = New atcGridSource
    With agdResults.Source
      Dim lColumn As Integer = 1
      .FixedRows = 1
      .FixedColumns = 1
      .Columns = pEndpoints.Count + pInputs.Count + 1
      .Rows = 2
      .CellValue(0, 0) = "Run"
      .CellColor(0, 0) = Drawing.SystemColors.Control
      .ColorCells = True
      For Each lEndpoint As Variation In pInputs
        If lEndpoint.Operation Is Nothing Then
          .CellValue(0, lColumn) = lEndpoint.Name
        Else
          .CellValue(0, lColumn) = lEndpoint.Name & " " & lEndpoint.Operation
        End If
        .CellColor(0, lColumn) = Drawing.SystemColors.Control
        lColumn += 1
      Next
      For Each lEndpoint As Variation In pEndpoints
        If lEndpoint.Operation Is Nothing Then
          .CellValue(0, lColumn) = lEndpoint.Name
        Else
          .CellValue(0, lColumn) = lEndpoint.Name & " " & lEndpoint.Operation
        End If
        .CellColor(0, lColumn) = Drawing.SystemColors.Control
        lColumn += 1
      Next
    End With
    agdResults.Initialize(agdResults.Source)
    agdResults.Refresh()

    myTabs.SelectedIndex = ResultsTabIndex

    Run(txtModifiedScenarioName.Text, _
        lVariations, _
        lWDMFileName, _
        lRuns, 0, Nothing)

    SaveSetting("BasinsCAT", "Settings", "TimePerRun", TimePerRun)

    PopulatePivotCombos()
    UpdateTopLabel("Finished with " & lRuns & " runs")
    pRunning = False
    btnStart.Visible = True
    btnStop.Visible = False
  End Sub

  Private Sub Run(ByVal aModifiedScenarioName As String, _
                  ByVal aVariations As atcCollection, _
                  ByVal aBaseWDMFileName As String, _
                  ByRef aIteration As Integer, _
                  ByRef aStartVariation As Integer, _
                  ByRef aModifiedData As atcDataGroup)

    If Not pRunning Then
      UpdateTopLabel("Stopping Run")
    Else
      Logger.Dbg("Run")
      ChDriveDir(PathNameOnly(aBaseWDMFileName))

      If aStartVariation >= aVariations.Count Then 'All variations have values, do a model run
        UpdateTopLabel(aIteration)
        If chkSaveAll.Checked Then aModifiedScenarioName &= "-" & aIteration + 1
                Dim lWDMResults As atcDataSource = Nothing
                Dim lHBNResults As atcDataSource = Nothing
        TimePerRun = Now.ToOADate
        ScenarioRun(aBaseWDMFileName, aModifiedScenarioName, aModifiedData, lWDMResults, lHBNResults)
        If lWDMResults Is Nothing Then
          Logger.Dbg("Null scenario results from ScenarioRun")
          Exit Sub
        End If
        TimePerRun = (Now.ToOADate - TimePerRun) * 24 * 60 * 60 'Convert days to seconds

        UpdateResults(aIteration, lWDMResults.DataSets, lHBNResults.DataSets)

        aIteration += 1

      Else 'Need to loop through values for next variation
        Dim lVariation As Variation = aVariations.ItemByIndex(aStartVariation)
        With lVariation
          Dim lModifiedGroup As atcDataGroup = .StartIteration
          While pRunning And Not lModifiedGroup Is Nothing
            If aModifiedData Is Nothing Then aModifiedData = New atcDataGroup

            aModifiedData.Add(lModifiedGroup)

            'We have handled a variation, now handle more input variations or run the model
            Run(aModifiedScenarioName, _
                aVariations, _
                aBaseWDMFileName, _
                aIteration, _
                aStartVariation + 1, _
                aModifiedData)

            aModifiedData.Remove(lModifiedGroup)

            lModifiedGroup = .NextIteration
          End While
        End With
      End If
    End If
  End Sub

  Private Sub UpdateResults(ByVal aIteration As Integer, ByVal aWDMData As atcDataGroup, ByVal aHBNData As atcDataGroup)
    With agdResults.Source
      Dim lRow As Integer = aIteration + .FixedRows
      Dim lColumn As Integer = .FixedColumns
      Dim lEndpoint As Variation
      .CellValue(lRow, 0) = aIteration + 1
      For Each lEndpoint In pInputs
        .CellValue(lRow, lColumn) = Format(lEndpoint.CurrentValue, "0.####")
        lColumn += 1
      Next
      For Each lEndpoint In pEndpoints
        For Each lOldData As atcDataSet In lEndpoint.DataSets
          Dim lGroup As atcDataGroup
          If CStr(lOldData.Attributes.GetValue("History 1", "")).ToLower.EndsWith("wdm") Then
            lGroup = aWDMData.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
          Else
            lGroup = aHBNData.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
          End If
          If lGroup.Count > 0 Then
            Dim lData As atcTimeseries = lGroup.Item(0)
            If Not lEndpoint.Seasons Is Nothing Then
              lData = lEndpoint.Seasons.SplitBySelected(lData, Nothing).Item(0)
            End If
            .CellValue(lRow, lColumn) = lData.Attributes.GetFormattedValue(lEndpoint.Operation)
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
      Next
    End With
    agdResults.Refresh()
    Try
      Windows.Forms.Application.DoEvents()
    Catch
      'stop
    End Try
    SaveFileString("ResultsIntermediate.txt", agdResults.Source.ToString)
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
      If Not lValue Is Nothing Then
        lMatch = False
        For Each lCheckValue In lValues
          If lCheckValue.Equals(lValue) Then lMatch = True : Exit For
        Next
        If Not lMatch Then
          lValues.Add(lValue)
        End If
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
      .FileName = GetSetting("BasinsCAT", "Settings", "LastResults", "Results.txt")
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Scenario Builder - Load Results"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If FileExists(.FileName) Then 'read file into grid
          PopulateResultsGrid(WholeFileString(.FileName))
          myTabs.SelectedIndex = ResultsTabIndex
          SaveSetting("BasinsCAT", "Settings", "LastResults", .FileName)
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
    agdResults.Refresh()
    agdPivot.Refresh()
  End Sub

  Private Sub mnuCopyResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuCopyResults.Click
    Clipboard.SetDataObject(agdResults.ToString)
  End Sub

  Private Sub mnuCopyPivot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuCopyPivot.Click
    Clipboard.SetDataObject(PivotText)
  End Sub

  Private Sub mnuPivotHeaders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPivotHeaders.Click
    mnuPivotHeaders.Checked = Not mnuPivotHeaders.Checked
    If mnuPivotHeaders.Checked Then
      DeleteSetting("BasinsCAT", "Settings", "PivotHeaders")
    Else
      SaveSetting("BasinsCAT", "Settings", "PivotHeaders", "No")
    End If
  End Sub

  Private Function PivotText() As String
    If mnuPivotHeaders.Checked Then 'Include info about columns, rows, cells
      Dim lFieldsSource As New atcGridSource
      lFieldsSource.FromString(lblPivotColumns.Text & vbTab & cboPivotColumns.Text & vbCrLf _
                             & lblPivotRows.Text & vbTab & cboPivotRows.Text & vbCrLf _
                             & lblPivotCells.Text & vbTab & cboPivotCells.Text & vbCrLf)
      Return lFieldsSource.AppendColumns(agdPivot.Source).ToString
    Else
      Return agdPivot.Source.ToString
    End If
  End Function

  Private Sub mnuSaveResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSaveResults.Click
    If agdResults.Source Is Nothing Then
      Logger.Msg("No results are available yet to save", "Save Results")
    Else
      Dim lSaveDialog As New Windows.Forms.SaveFileDialog
      With lSaveDialog
        .FileName = GetSetting("BasinsCAT", "Settings", "LastResults", "Results.txt")
        .Filter = "Text files (*.txt)|*.txt|All files|*.*"
        .FilterIndex = 1
        .Title = "Save Results as Tab-Delimited Text"
        .OverwritePrompt = True
        If .ShowDialog() = Windows.Forms.DialogResult.OK Then
          'write file from grid contents
          SaveFileString(.FileName, agdResults.Source.ToString)
          SaveSetting("BasinsCAT", "Settings", "LastResults", .FileName)
        End If
      End With
    End If
  End Sub

  Private Sub mnuSavePivot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSavePivot.Click
    Dim lSaveDialog As New Windows.Forms.SaveFileDialog
    With lSaveDialog
      .FileName = GetSetting("BasinsCAT", "Settings", "LastPivot", "Pivot.txt")
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Save Pivot Table as Tab-Delimited Text"
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'write file from grid contents
        SaveFileString(.FileName, PivotText)
        SaveSetting("BasinsCAT", "Settings", "LastPivot", .FileName)
      End If
    End With
  End Sub

  Private Sub UpdateTopLabel(ByVal aText As String)
    Logger.Dbg(aText)
    lblTop.Text = aText
    lblTop.Refresh()
    Application.DoEvents()
  End Sub

  Private Sub UpdateTopLabel(ByVal aIteration As Integer)
    Dim lLabelText As String = "Running # " & aIteration + 1 & " of " & TotalIterations
    If TimePerRun > 0 Then
      lLabelText &= " (" & FormatTime(TimePerRun * (TotalIterations - aIteration)) & " remaining)"
    End If
    UpdateTopLabel(lLabelText)
  End Sub

  Private Function FormatTime(ByVal aSeconds As Double) As String
    Dim lFormat As String = "0"
    Dim lFormatted As String = ""
    Dim lDays As Integer = Int(aSeconds / 86400)
    If lDays > 0 Then 'More than a day left
      lFormatted &= Format(lDays, lFormat) & ":"
      aSeconds -= 86400 * lDays
      lFormat = "00"
    End If
    Dim lHours As Integer = Int(aSeconds / 3600)
    If lHours > 0 OrElse lDays > 0 Then 'More than an hour left
      lFormatted &= Format(lHours, lFormat) & ":"
      aSeconds -= 3600 * lHours
      lFormat = "00"
    End If
    'Always include minutes:
    Dim lMinutes As Integer = Int(aSeconds / 60)
    lFormatted &= Format(lMinutes, lFormat) & ":"
    aSeconds -= 60 * lMinutes
    lFormat = "00"
    Return lFormatted & Format(aSeconds, lFormat)
  End Function

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
      lVariation.Selected = True
      lVariation.CurrentValue = lVariation.Min
      pInputs.Add(lVariation)
      RefreshInputList()
      RefreshTotalIterations()
    End If
  End Sub

  Private Sub btnInputAddCligen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputAddCligen.Click
    Dim frmVary As New frmVariationCligen
    Dim lVariation As New VariationCligen
    With lVariation
      .Name = CLIGEN_NAME
      .ComputationSource = New atcTimeseriesMath.atcTimeseriesMath
      .Operation = "Multiply"
      .Min = 0.9
      .Max = 1.1
      .Increment = 0.1
    End With
    lVariation = frmVary.AskUser(lVariation)
    If Not lVariation Is Nothing Then
      If lVariation.Name.IndexOf(CLIGEN_NAME) < 0 Then lVariation.Name = CLIGEN_NAME & " " & lVariation.Name
      lVariation.Selected = True
      lVariation.CurrentValue = lVariation.Min
      pInputs.Add(lVariation)
      RefreshInputList()
      RefreshTotalIterations()
    End If
  End Sub

  Private Sub btnInputModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputModify.Click
    Dim lIndex As Integer = lstInputs.SelectedIndex
    If lIndex >= 0 And lIndex < pInputs.Count Then
      Dim lVariation As Variation = pInputs.ItemByIndex(lIndex)
      If lVariation.GetType.Name.EndsWith("Cligen") Then
        Dim frmVaryCligen As New frmVariationCligen
        lVariation = frmVaryCligen.AskUser(lVariation)
      Else
        Dim frmVary As New frmVariation
        lVariation = frmVary.AskUser(lVariation)
      End If
      If Not lVariation Is Nothing Then
        pInputs.RemoveAt(lIndex)
        pInputs.Insert(lIndex, lVariation)
        RefreshInputList()
        RefreshTotalIterations()
      End If
    End If
  End Sub

  Private Sub btnInputRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputRemove.Click
    For Each lIndex As Integer In lstInputs.SelectedIndices
      pInputs.RemoveAt(lIndex)
      RefreshInputList()
    Next
    RefreshTotalIterations()
  End Sub

  Private Sub MoveItem(ByVal aGroup As atcCollection, ByVal aList As CheckedListBox, ByVal aMoveFrom As Integer, ByVal aMoveTo As Integer)
    If aMoveFrom >= 0 AndAlso aMoveTo >= 0 AndAlso aMoveFrom < aGroup.Count AndAlso aMoveTo < aGroup.Count Then
      Dim lWasChecked As Boolean = aList.CheckedIndices.Contains(aMoveFrom)
      Dim lMoveMe As Variation = aGroup.ItemByIndex(aMoveFrom)
      aGroup.RemoveAt(aMoveFrom)
      aList.Items.RemoveAt(aMoveFrom)
      aGroup.Insert(aMoveTo, lMoveMe)
      aList.Items.Insert(aMoveTo, lMoveMe.ToString)
      If lWasChecked Then aList.SetItemChecked(aMoveTo, True)
      aList.SelectedIndex = aMoveTo
    End If
  End Sub

  Private Sub btnInputUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputUp.Click
    MoveItem(pInputs, lstInputs, lstInputs.SelectedIndex, lstInputs.SelectedIndex - 1)
  End Sub

  Private Sub btnInputDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputDown.Click
    MoveItem(pInputs, lstInputs, lstInputs.SelectedIndex, lstInputs.SelectedIndex + 1)
  End Sub

  Private Sub btnEndpointUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointUp.Click
    MoveItem(pEndpoints, lstEndpoints, lstEndpoints.SelectedIndex, lstEndpoints.SelectedIndex - 1)
  End Sub

  Private Sub btnEndpointDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointDown.Click
    MoveItem(pEndpoints, lstEndpoints, lstEndpoints.SelectedIndex, lstEndpoints.SelectedIndex + 1)
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
      lVariation.Selected = True
      pEndpoints.Add(lVariation)
      RefreshEndpointList()
    End If
  End Sub

  Private Sub RefreshTotalIterations()
    Dim lLabelText As String
    Dim iVariation As Integer = 0
    TotalIterations = 1
    'Make a collection of the variations that are selected/checked in lstInputs
    For Each lVariation As Variation In pInputs
      lVariation.Selected = lstInputs.CheckedIndices.Contains(iVariation)
      If lVariation.Selected Then
        TotalIterations *= lVariation.Iterations
      End If
      iVariation += 1
    Next
    lLabelText = "Total iterations selected = " & TotalIterations
    If TimePerRun > 0 Then
      lLabelText &= " (" & FormatTime(TimePerRun * TotalIterations) & ")"
    End If
    UpdateTopLabel(lLabelText)
    Try
      lblAllResults.Text = "(" & Format((FileLen(cboBaseScenarioName.Text) * TotalIterations) / 1048576, "#,##0.#") & " Meg)"
    Catch
      lblAllResults.Text = ""
    End Try
  End Sub

  Private Sub RefreshInputList()
    lstInputs.Items.Clear()
    For Each lVariation As Variation In pInputs
      lstInputs.Items.Add(lVariation.ToString)
      lstInputs.SetItemChecked(lstInputs.Items.Count - 1, lVariation.Selected)
    Next
    RefreshEndpointList()
  End Sub

  Private Sub RefreshEndpointList()
    Dim lVariation As Variation
    lstEndpoints.Items.Clear()

    'Add endpoint references to inputs
    For Each lVariation In pInputs
      lstEndpoints.Items.Add(InputArgumentPrefix & lVariation.Name)
      lstEndpoints.SetItemChecked(lstEndpoints.Items.Count - 1, lVariation.Selected)
    Next

    For Each lVariation In pEndpoints
      lstEndpoints.Items.Add(lVariation.ToString)
      lstEndpoints.SetItemChecked(lstEndpoints.Items.Count - 1, lVariation.Selected)
    Next

  End Sub

  Private Sub EndpointSelectionFromList()
    Dim lListIndex As Integer = pInputs.Count
    For Each lVariation As Variation In pEndpoints
      lVariation.Selected = lstEndpoints.GetItemChecked(lListIndex)
      lListIndex += 1
    Next
  End Sub

  Private Sub btnEndpointModify_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointModify.Click
    Dim lIndex As Integer = lstEndpoints.SelectedIndex - pInputs.Count
    If lIndex >= 0 And lIndex < pEndpoints.Count Then
      Dim lVariation As Variation = pEndpoints.ItemByIndex(lIndex)
      Dim frmEnd As New frmEndpoint
      lVariation = frmEnd.AskUser(lVariation)
      If Not lVariation Is Nothing Then
        pEndpoints.RemoveAt(lIndex)
        pEndpoints.Insert(lIndex, lVariation)
        RefreshEndpointList()
      End If
    End If
  End Sub

  Private Sub btnEndpointRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointRemove.Click
    For Each lIndex As Integer In lstEndpoints.SelectedIndices
      pEndpoints.RemoveAt(lIndex)
      lstEndpoints.Items.RemoveAt(lIndex)
    Next
  End Sub

  Private Sub mnuSaveVariations_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSaveVariations.Click
    Dim lSaveDialog As New Windows.Forms.SaveFileDialog
    With lSaveDialog
      .FileName = GetSetting("BasinsCAT", "Settings", "LastSetup", "CAT.xml")
      .Filter = "XML files (*.xml)|*.xml|All files|*.*"
      .FilterIndex = 1
      .Title = "Save Variations as XML Text"
      .OverwritePrompt = True
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'write file from form contents
        SaveFileString(.FileName, (XML))
        SaveSetting("BasinsCAT", "Settings", "LastSetup", .FileName)
      End If
    End With
  End Sub

  Public Property XML() As String
    Get
      Dim lXML As String = "<BasinsCAT>" & vbCrLf

      lXML &= "<Wdm>" & vbCrLf
      For Each lScenario As String In cboBaseScenarioName.Items
        lXML &= "  <FileName>" & lScenario & "</FileName>" & vbCrLf
      Next
      lXML &= "</Wdm>" & vbCrLf

      Dim lVariation As Variation
      Dim lVariationIndex As Integer = 0
      lXML &= "<Variations>" & vbCrLf
      For Each lVariation In pInputs
        lXML &= lVariation.XML
        lVariationIndex += 1
      Next
      lXML &= "</Variations>" & vbCrLf

      lXML &= "<Endpoints>" & vbCrLf
      lVariationIndex = 0
      For Each lVariation In pEndpoints
        If Double.IsNaN(lVariation.CurrentValue) Then
          Dim lOneXML As New Chilkat.Xml
          lOneXML.LoadXml(lVariation.XML)
          If lstInputs.SelectedIndices.Contains(lVariationIndex) Then
            lOneXML.AddAttribute("Selected", "True")
          End If
          lXML &= lOneXML.GetXml
        End If
        lVariationIndex += 1
      Next
      lXML &= "</Endpoints>" & vbCrLf

      lXML &= "</BasinsCAT>" & vbCrLf

      'Dim lCXML As New Chilkat.Xml
      'If lCXML.LoadXml(lXML) Then
      '  Return lCXML.GetXml
      'Else
      '  Logger.Dbg("Could not parse new XML")
      Return lXML
      'End If
    End Get

    Set(ByVal aValue As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(aValue) AndAlso lXML.Tag.ToLower.Equals("basinscat") AndAlso lXML.FirstChild2 Then
        Do
          Dim lVariation As Variation
          Dim lChild As Chilkat.Xml = lXML.FirstChild
          Select Case lXML.Tag.ToLower
            Case "wdm"
              Dim lFileName As String = lChild.Content
              Dim lAddSource As Boolean = True
              For Each lDataSource As atcDataSource In g_DataManager.DataSources
                If lDataSource.Specification = lFileName Then 'already open
                  lAddSource = False
                End If
              Next
              If lAddSource Then
                Dim lDataSource As New atcWDM.atcDataSourceWDM
                lDataSource.Specification = lFileName
                g_DataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing)
                cboBaseScenarioName.Items.Add(lDataSource.Specification)
                cboBaseScenarioName.SelectedIndex = cboBaseScenarioName.Items.Count - 1
              End If

            Case "variations"
              pInputs.Clear()

              If Not lChild Is Nothing Then
                Do
                  If lChild.GetChildWithTag("Name").Content.IndexOf(CLIGEN_NAME) >= 0 Then
                    lVariation = New VariationCligen
                  Else
                    lVariation = New Variation
                  End If
                  lVariation.XML = lChild.GetXml
                  pInputs.Add(lVariation)
                Loop While lChild.NextSibling2
              End If
              RefreshInputList()
              RefreshTotalIterations()
            Case "endpoints"
              pEndpoints.Clear()
              If Not lChild Is Nothing Then
                Do
                  lVariation = New Variation
                  lVariation.XML = lChild.GetXml
                  pEndpoints.Add(lVariation)
                Loop While lChild.NextSibling2
              End If
              RefreshEndpointList()
          End Select
        Loop While lXML.NextSibling2
        'RefreshInputList()
      Else
        Logger.Msg("Could not parse variations " & vbCrLf & lXML.LastErrorText, "Load Variations")
      End If
    End Set
  End Property

  Private Sub mnuLoadVariations_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuLoadVariations.Click
    Dim lOpenDialog As New Windows.Forms.OpenFileDialog
    With lOpenDialog
      .FileName = GetSetting("BasinsCAT", "Settings", "LastSetup", "CAT.xml")
      .Filter = "XML files (*.xml)|*.xml|All files|*.*"
      .FilterIndex = 1
      .Title = "Scenario Builder - Load Variations"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If FileExists(.FileName) Then
          XML = WholeFileString(.FileName)
          SaveSetting("BasinsCAT", "Settings", "LastSetup", .FileName)
        End If
      End If
    End With
  End Sub

  Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
    btnStart.Visible = True
    btnStop.Visible = False
    pRunning = False
    UpdateTopLabel("Stopping")
  End Sub

  Private Sub lstInputs_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstInputs.MouseUp
    RefreshTotalIterations()
  End Sub

  Private Sub lstEndpoints_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstEndpoints.MouseUp
    EndpointSelectionFromList()
    End Sub

  Private Sub mnuHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
    ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
  End Sub
End Class
