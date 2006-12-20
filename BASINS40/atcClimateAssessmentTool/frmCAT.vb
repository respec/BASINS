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
    Friend WithEvents chkShowEachRunProgress As System.Windows.Forms.CheckBox
    Friend WithEvents btnEndpointCopy As System.Windows.Forms.Button
    Friend WithEvents mnuOpenUCI As System.Windows.Forms.MenuItem
    Friend WithEvents txtBaseScenario As System.Windows.Forms.TextBox
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCAT))
        Me.myTabs = New System.Windows.Forms.TabControl
        Me.tabInputs = New System.Windows.Forms.TabPage
        Me.txtBaseScenario = New System.Windows.Forms.TextBox
        Me.btnInputAddCligen = New System.Windows.Forms.Button
        Me.lstInputs = New System.Windows.Forms.CheckedListBox
        Me.btnInputDown = New System.Windows.Forms.Button
        Me.btnInputUp = New System.Windows.Forms.Button
        Me.btnInputRemove = New System.Windows.Forms.Button
        Me.btnInputModify = New System.Windows.Forms.Button
        Me.btnInputAdd = New System.Windows.Forms.Button
        Me.txtModifiedScenarioName = New System.Windows.Forms.TextBox
        Me.lblBaseScenarioName = New System.Windows.Forms.Label
        Me.lblNewScenarioName = New System.Windows.Forms.Label
        Me.tabEndpoints = New System.Windows.Forms.TabPage
        Me.btnEndpointCopy = New System.Windows.Forms.Button
        Me.chkShowEachRunProgress = New System.Windows.Forms.CheckBox
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
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuOpenUCI = New System.Windows.Forms.MenuItem
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
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.lblTop = New System.Windows.Forms.Label
        Me.btnStop = New System.Windows.Forms.Button
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
        Me.myTabs.Location = New System.Drawing.Point(0, 3)
        Me.myTabs.Name = "myTabs"
        Me.myTabs.SelectedIndex = 0
        Me.myTabs.Size = New System.Drawing.Size(459, 310)
        Me.myTabs.TabIndex = 1
        '
        'tabInputs
        '
        Me.tabInputs.Controls.Add(Me.txtBaseScenario)
        Me.tabInputs.Controls.Add(Me.btnInputAddCligen)
        Me.tabInputs.Controls.Add(Me.lstInputs)
        Me.tabInputs.Controls.Add(Me.btnInputDown)
        Me.tabInputs.Controls.Add(Me.btnInputUp)
        Me.tabInputs.Controls.Add(Me.btnInputRemove)
        Me.tabInputs.Controls.Add(Me.btnInputModify)
        Me.tabInputs.Controls.Add(Me.btnInputAdd)
        Me.tabInputs.Controls.Add(Me.txtModifiedScenarioName)
        Me.tabInputs.Controls.Add(Me.lblBaseScenarioName)
        Me.tabInputs.Controls.Add(Me.lblNewScenarioName)
        Me.tabInputs.Location = New System.Drawing.Point(4, 22)
        Me.tabInputs.Name = "tabInputs"
        Me.tabInputs.Size = New System.Drawing.Size(451, 284)
        Me.tabInputs.TabIndex = 0
        Me.tabInputs.Text = "Inputs"
        '
        'txtBaseScenario
        '
        Me.txtBaseScenario.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBaseScenario.Location = New System.Drawing.Point(96, 8)
        Me.txtBaseScenario.Name = "txtBaseScenario"
        Me.txtBaseScenario.Size = New System.Drawing.Size(346, 20)
        Me.txtBaseScenario.TabIndex = 3
        Me.txtBaseScenario.Text = "<click to select>"
        '
        'btnInputAddCligen
        '
        Me.btnInputAddCligen.Location = New System.Drawing.Point(178, 64)
        Me.btnInputAddCligen.Name = "btnInputAddCligen"
        Me.btnInputAddCligen.Size = New System.Drawing.Size(48, 24)
        Me.btnInputAddCligen.TabIndex = 9
        Me.btnInputAddCligen.Text = "Cligen"
        '
        'lstInputs
        '
        Me.lstInputs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstInputs.IntegralHeight = False
        Me.lstInputs.Location = New System.Drawing.Point(8, 94)
        Me.lstInputs.Name = "lstInputs"
        Me.lstInputs.Size = New System.Drawing.Size(434, 181)
        Me.lstInputs.TabIndex = 12
        '
        'btnInputDown
        '
        Me.btnInputDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInputDown.Location = New System.Drawing.Point(418, 64)
        Me.btnInputDown.Name = "btnInputDown"
        Me.btnInputDown.Size = New System.Drawing.Size(24, 24)
        Me.btnInputDown.TabIndex = 11
        Me.btnInputDown.Text = "v"
        '
        'btnInputUp
        '
        Me.btnInputUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInputUp.Location = New System.Drawing.Point(388, 64)
        Me.btnInputUp.Name = "btnInputUp"
        Me.btnInputUp.Size = New System.Drawing.Size(24, 24)
        Me.btnInputUp.TabIndex = 10
        Me.btnInputUp.Text = "^"
        '
        'btnInputRemove
        '
        Me.btnInputRemove.Location = New System.Drawing.Point(62, 64)
        Me.btnInputRemove.Name = "btnInputRemove"
        Me.btnInputRemove.Size = New System.Drawing.Size(56, 24)
        Me.btnInputRemove.TabIndex = 7
        Me.btnInputRemove.Text = "Remove"
        '
        'btnInputModify
        '
        Me.btnInputModify.Location = New System.Drawing.Point(124, 64)
        Me.btnInputModify.Name = "btnInputModify"
        Me.btnInputModify.Size = New System.Drawing.Size(48, 24)
        Me.btnInputModify.TabIndex = 8
        Me.btnInputModify.Text = "Edit"
        '
        'btnInputAdd
        '
        Me.btnInputAdd.Location = New System.Drawing.Point(8, 64)
        Me.btnInputAdd.Name = "btnInputAdd"
        Me.btnInputAdd.Size = New System.Drawing.Size(48, 24)
        Me.btnInputAdd.TabIndex = 6
        Me.btnInputAdd.Text = "Add"
        '
        'txtModifiedScenarioName
        '
        Me.txtModifiedScenarioName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtModifiedScenarioName.Location = New System.Drawing.Point(96, 34)
        Me.txtModifiedScenarioName.Name = "txtModifiedScenarioName"
        Me.txtModifiedScenarioName.Size = New System.Drawing.Size(346, 20)
        Me.txtModifiedScenarioName.TabIndex = 5
        Me.txtModifiedScenarioName.Text = "Modified"
        '
        'lblBaseScenarioName
        '
        Me.lblBaseScenarioName.AutoSize = True
        Me.lblBaseScenarioName.Location = New System.Drawing.Point(8, 11)
        Me.lblBaseScenarioName.Name = "lblBaseScenarioName"
        Me.lblBaseScenarioName.Size = New System.Drawing.Size(76, 13)
        Me.lblBaseScenarioName.TabIndex = 2
        Me.lblBaseScenarioName.Text = "Base Scenario"
        '
        'lblNewScenarioName
        '
        Me.lblNewScenarioName.AutoSize = True
        Me.lblNewScenarioName.Location = New System.Drawing.Point(8, 37)
        Me.lblNewScenarioName.Name = "lblNewScenarioName"
        Me.lblNewScenarioName.Size = New System.Drawing.Size(74, 13)
        Me.lblNewScenarioName.TabIndex = 4
        Me.lblNewScenarioName.Text = "New Scenario"
        '
        'tabEndpoints
        '
        Me.tabEndpoints.Controls.Add(Me.btnEndpointCopy)
        Me.tabEndpoints.Controls.Add(Me.chkShowEachRunProgress)
        Me.tabEndpoints.Controls.Add(Me.lblAllResults)
        Me.tabEndpoints.Controls.Add(Me.chkSaveAll)
        Me.tabEndpoints.Controls.Add(Me.lstEndpoints)
        Me.tabEndpoints.Controls.Add(Me.btnEndpointDown)
        Me.tabEndpoints.Controls.Add(Me.btnEndpointUp)
        Me.tabEndpoints.Controls.Add(Me.btnEndpointRemove)
        Me.tabEndpoints.Controls.Add(Me.btnEndpointModify)
        Me.tabEndpoints.Controls.Add(Me.btnEndpointAdd)
        Me.tabEndpoints.Location = New System.Drawing.Point(4, 22)
        Me.tabEndpoints.Name = "tabEndpoints"
        Me.tabEndpoints.Size = New System.Drawing.Size(451, 284)
        Me.tabEndpoints.TabIndex = 1
        Me.tabEndpoints.Text = "Endpoints"
        '
        'btnEndpointCopy
        '
        Me.btnEndpointCopy.Location = New System.Drawing.Point(178, 64)
        Me.btnEndpointCopy.Name = "btnEndpointCopy"
        Me.btnEndpointCopy.Size = New System.Drawing.Size(48, 24)
        Me.btnEndpointCopy.TabIndex = 16
        Me.btnEndpointCopy.Text = "Copy"
        '
        'chkShowEachRunProgress
        '
        Me.chkShowEachRunProgress.AutoSize = True
        Me.chkShowEachRunProgress.Location = New System.Drawing.Point(16, 38)
        Me.chkShowEachRunProgress.Name = "chkShowEachRunProgress"
        Me.chkShowEachRunProgress.Size = New System.Drawing.Size(160, 17)
        Me.chkShowEachRunProgress.TabIndex = 12
        Me.chkShowEachRunProgress.Text = "Show Progress of Each Run"
        '
        'lblAllResults
        '
        Me.lblAllResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAllResults.Location = New System.Drawing.Point(128, 16)
        Me.lblAllResults.Name = "lblAllResults"
        Me.lblAllResults.Size = New System.Drawing.Size(307, 16)
        Me.lblAllResults.TabIndex = 18
        '
        'chkSaveAll
        '
        Me.chkSaveAll.AutoSize = True
        Me.chkSaveAll.Location = New System.Drawing.Point(16, 16)
        Me.chkSaveAll.Name = "chkSaveAll"
        Me.chkSaveAll.Size = New System.Drawing.Size(103, 17)
        Me.chkSaveAll.TabIndex = 11
        Me.chkSaveAll.Text = "Save All Results"
        '
        'lstEndpoints
        '
        Me.lstEndpoints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstEndpoints.IntegralHeight = False
        Me.lstEndpoints.Location = New System.Drawing.Point(8, 94)
        Me.lstEndpoints.Name = "lstEndpoints"
        Me.lstEndpoints.Size = New System.Drawing.Size(434, 181)
        Me.lstEndpoints.TabIndex = 20
        '
        'btnEndpointDown
        '
        Me.btnEndpointDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEndpointDown.Location = New System.Drawing.Point(417, 64)
        Me.btnEndpointDown.Name = "btnEndpointDown"
        Me.btnEndpointDown.Size = New System.Drawing.Size(24, 24)
        Me.btnEndpointDown.TabIndex = 19
        Me.btnEndpointDown.Text = "v"
        '
        'btnEndpointUp
        '
        Me.btnEndpointUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEndpointUp.Location = New System.Drawing.Point(387, 64)
        Me.btnEndpointUp.Name = "btnEndpointUp"
        Me.btnEndpointUp.Size = New System.Drawing.Size(24, 24)
        Me.btnEndpointUp.TabIndex = 18
        Me.btnEndpointUp.Text = "^"
        '
        'btnEndpointRemove
        '
        Me.btnEndpointRemove.Location = New System.Drawing.Point(62, 64)
        Me.btnEndpointRemove.Name = "btnEndpointRemove"
        Me.btnEndpointRemove.Size = New System.Drawing.Size(56, 24)
        Me.btnEndpointRemove.TabIndex = 14
        Me.btnEndpointRemove.Text = "Remove"
        '
        'btnEndpointModify
        '
        Me.btnEndpointModify.Location = New System.Drawing.Point(124, 64)
        Me.btnEndpointModify.Name = "btnEndpointModify"
        Me.btnEndpointModify.Size = New System.Drawing.Size(48, 24)
        Me.btnEndpointModify.TabIndex = 15
        Me.btnEndpointModify.Text = "Edit"
        '
        'btnEndpointAdd
        '
        Me.btnEndpointAdd.Location = New System.Drawing.Point(8, 64)
        Me.btnEndpointAdd.Name = "btnEndpointAdd"
        Me.btnEndpointAdd.Size = New System.Drawing.Size(48, 24)
        Me.btnEndpointAdd.TabIndex = 13
        Me.btnEndpointAdd.Text = "Add"
        '
        'tabResults
        '
        Me.tabResults.Controls.Add(Me.agdResults)
        Me.tabResults.Location = New System.Drawing.Point(4, 22)
        Me.tabResults.Name = "tabResults"
        Me.tabResults.Size = New System.Drawing.Size(451, 284)
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
        Me.agdResults.Location = New System.Drawing.Point(8, 8)
        Me.agdResults.Name = "agdResults"
        Me.agdResults.Size = New System.Drawing.Size(434, 267)
        Me.agdResults.Source = Nothing
        Me.agdResults.TabIndex = 21
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
        Me.tabPivot.Size = New System.Drawing.Size(451, 284)
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
        Me.agdPivot.Location = New System.Drawing.Point(8, 88)
        Me.agdPivot.Name = "agdPivot"
        Me.agdPivot.Size = New System.Drawing.Size(434, 187)
        Me.agdPivot.Source = Nothing
        Me.agdPivot.TabIndex = 28
        '
        'lblPivotColumns
        '
        Me.lblPivotColumns.AutoSize = True
        Me.lblPivotColumns.Location = New System.Drawing.Point(27, 37)
        Me.lblPivotColumns.Name = "lblPivotColumns"
        Me.lblPivotColumns.Size = New System.Drawing.Size(47, 13)
        Me.lblPivotColumns.TabIndex = 24
        Me.lblPivotColumns.Text = "Columns"
        Me.lblPivotColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPivotCells
        '
        Me.lblPivotCells.AutoSize = True
        Me.lblPivotCells.Location = New System.Drawing.Point(45, 64)
        Me.lblPivotCells.Name = "lblPivotCells"
        Me.lblPivotCells.Size = New System.Drawing.Size(29, 13)
        Me.lblPivotCells.TabIndex = 26
        Me.lblPivotCells.Text = "Cells"
        Me.lblPivotCells.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPivotRows
        '
        Me.lblPivotRows.AutoSize = True
        Me.lblPivotRows.Location = New System.Drawing.Point(40, 10)
        Me.lblPivotRows.Name = "lblPivotRows"
        Me.lblPivotRows.Size = New System.Drawing.Size(34, 13)
        Me.lblPivotRows.TabIndex = 22
        Me.lblPivotRows.Text = "Rows"
        Me.lblPivotRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboPivotCells
        '
        Me.cboPivotCells.Location = New System.Drawing.Point(80, 61)
        Me.cboPivotCells.Name = "cboPivotCells"
        Me.cboPivotCells.Size = New System.Drawing.Size(128, 21)
        Me.cboPivotCells.TabIndex = 27
        '
        'cboPivotColumns
        '
        Me.cboPivotColumns.Location = New System.Drawing.Point(80, 34)
        Me.cboPivotColumns.Name = "cboPivotColumns"
        Me.cboPivotColumns.Size = New System.Drawing.Size(128, 21)
        Me.cboPivotColumns.TabIndex = 25
        '
        'cboPivotRows
        '
        Me.cboPivotRows.Location = New System.Drawing.Point(80, 7)
        Me.cboPivotRows.Name = "cboPivotRows"
        Me.cboPivotRows.Size = New System.Drawing.Size(128, 21)
        Me.cboPivotRows.TabIndex = 23
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(12, 319)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(56, 24)
        Me.btnStart.TabIndex = 0
        Me.btnStart.Text = "Start"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuOptions, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpenUCI, Me.mnuLoadVariations, Me.mnuSaveVariations, Me.mnuFileSep1, Me.mnuLoadResults, Me.mnuSaveResults, Me.mnuFileSep2, Me.mnuSavePivot})
        Me.mnuFile.Text = "File"
        '
        'mnuOpenUCI
        '
        Me.mnuOpenUCI.Index = 0
        Me.mnuOpenUCI.Text = "Open UCI file"
        '
        'mnuLoadVariations
        '
        Me.mnuLoadVariations.Index = 1
        Me.mnuLoadVariations.Text = "Load Inputs and Endpoints"
        '
        'mnuSaveVariations
        '
        Me.mnuSaveVariations.Index = 2
        Me.mnuSaveVariations.Text = "Save Inputs and Endpoints"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 3
        Me.mnuFileSep1.Text = "-"
        '
        'mnuLoadResults
        '
        Me.mnuLoadResults.Index = 4
        Me.mnuLoadResults.Text = "Load Results"
        '
        'mnuSaveResults
        '
        Me.mnuSaveResults.Index = 5
        Me.mnuSaveResults.Text = "Save Results"
        '
        'mnuFileSep2
        '
        Me.mnuFileSep2.Index = 6
        Me.mnuFileSep2.Text = "-"
        '
        'mnuSavePivot
        '
        Me.mnuSavePivot.Index = 7
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
        'mnuHelp
        '
        Me.mnuHelp.Index = 3
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'lblTop
        '
        Me.lblTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTop.Location = New System.Drawing.Point(74, 319)
        Me.lblTop.Name = "lblTop"
        Me.lblTop.Size = New System.Drawing.Size(372, 24)
        Me.lblTop.TabIndex = 2
        Me.lblTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Location = New System.Drawing.Point(12, 319)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(56, 24)
        Me.btnStop.TabIndex = 3
        Me.btnStop.Text = "Stop"
        Me.btnStop.Visible = False
        '
        'frmCAT
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(458, 351)
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
        Me.tabInputs.PerformLayout()
        Me.tabEndpoints.ResumeLayout(False)
        Me.tabEndpoints.PerformLayout()
        Me.tabResults.ResumeLayout(False)
        Me.tabPivot.ResumeLayout(False)
        Me.tabPivot.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Const CLIGEN_NAME As String = "Cligen"
    Private Const StartFolderVariable As String = "{StartFolder}"

    Private pPlugin As atcClimateAssessmentToolPlugin

    'all the variations listed in the Input tab
    Private pInputs As atcCollection
    Private pInputFiles As atcCollection

    'all the endpoints listed in the Endpoints tab
    Private pEndpoints As atcCollection

    Private pUciFile As String = ""

    Private InputArgumentPrefix As String = "Current Modifier for "
    Private ResultsTabIndex As Integer = 2
    Private TotalIterations As Integer = 0
    Private TimePerRun As Double = 0 'Time each run takes in seconds

    Public Sub Initialize(ByRef aPlugin As atcClimateAssessmentToolPlugin)
        mnuPivotHeaders.Checked = GetSetting("BasinsCAT", "Settings", "PivotHeaders", "Yes").Equals("Yes")
        TimePerRun = CDbl(GetSetting("BasinsCAT", "Settings", "TimePerRun", "0"))
        pInputs = New atcCollection
        pEndpoints = New atcCollection
        Me.Show()

        pPlugin = aPlugin
        If Not aPlugin.XML Is Nothing AndAlso aPlugin.XML.Length > 0 Then
            XML = aPlugin.XML
        End If
    End Sub

    Private Function OpenDataSource(ByVal aFilename As String) As atcDataSource
        Dim lAddSource As Boolean = True
        For Each lDataSource As atcDataSource In g_DataManager.DataSources
            If lDataSource.Specification.ToLower = aFilename.ToLower Then 'already open
                Return lDataSource
            End If
        Next
        If lAddSource AndAlso FileExists(aFilename) Then
            Dim lDataSource As atcDataSource
            If aFilename.ToLower.EndsWith("wdm") Then
                lDataSource = New atcWDM.atcDataSourceWDM
            ElseIf aFilename.ToLower.EndsWith("hbn") Then
                lDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Else
                Throw New ApplicationException("Could not open '" & aFilename & "' in frmCAT:OpenDataSource")
            End If
            lDataSource.Specification = aFilename
            g_DataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing)
            Return lDataSource
        End If
        Return Nothing
    End Function

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim lSelectedVariations As atcCollection = New atcCollection
        Dim lRuns As Integer = 0
        Dim lEndpoint As Variation

        g_running = True
        btnStart.Visible = False
        btnStop.Visible = True

        UpdateStatusLabel("Setting up to run")

        RefreshTotalIterations()

        'Make a collection of the variations that are selected/checked in lstInputs
        For Each lVariation As Variation In pInputs
            If lVariation.Selected Then lSelectedVariations.Add(lVariation)
        Next

        'header for attributes
        agdResults.Source = New atcGridSource
        With agdResults.Source
            Dim lUsingSeasons As Boolean = False
            Dim lColumn As Integer = 1
            Dim lRow As Integer
            .FixedRows = 4
            .FixedColumns = 1
            .Columns = 1
            .CellValue(0, 0) = "Run"
            .ColorCells = True

            For Each lEndpoint In pEndpoints
                If Not Double.IsNaN(lEndpoint.CurrentValue) Then
                    .Columns += 1
                Else
                    .Columns += lEndpoint.DataSets.Count
                End If
            Next

            .Rows = 5
            lColumn = 1

            For lRow = 0 To .FixedRows - 1
                .CellColor(lRow, 0) = Drawing.SystemColors.Control
            Next

            For Each lEndpoint In pEndpoints
                If Not Double.IsNaN(lEndpoint.CurrentValue) Then
                    .CellValue(0, lColumn) = lEndpoint.Name
                    .CellValue(1, lColumn) = lEndpoint.Operation
                    .CellValue(2, lColumn) = "Current Value"
                    If Not lEndpoint.Seasons Is Nothing Then
                        .CellValue(3, lColumn) = lEndpoint.Seasons.ToString
                    End If
                    For lRow = 0 To .FixedRows - 1
                        .CellColor(lRow, lColumn) = Drawing.SystemColors.Control
                    Next
                    lColumn += 1
                Else
                    For Each lDataset As atcDataSet In lEndpoint.DataSets
                        .CellValue(0, lColumn) = lEndpoint.Name
                        If Not lEndpoint.Operation Is Nothing Then
                            .CellValue(1, lColumn) = lEndpoint.Operation
                        End If
                        .CellValue(2, lColumn) = lDataset.ToString
                        If Not lEndpoint.Seasons Is Nothing Then
                            .CellValue(3, lColumn) = lEndpoint.Seasons.ToString
                        End If
                        For lRow = 0 To .FixedRows - 1
                            .CellColor(lRow, lColumn) = Drawing.SystemColors.Control
                        Next
                        lColumn += 1
                    Next
                End If
            Next
        End With

        agdResults.Initialize(agdResults.Source)
        agdResults.Refresh()

        myTabs.SelectedIndex = ResultsTabIndex

        Run(txtModifiedScenarioName.Text, _
            lSelectedVariations, _
            txtBaseScenario.Text, _
            lRuns, 0, Nothing)

        SaveSetting("BasinsCAT", "Settings", "TimePerRun", TimePerRun)

        PopulatePivotCombos()
        UpdateStatusLabel("Finished with " & lRuns & " runs")
        g_running = False
        btnStart.Visible = True
        btnStop.Visible = False
    End Sub

    Private Sub Run(ByVal aModifiedScenarioName As String, _
                    ByVal aVariations As atcCollection, _
                    ByVal aBaseFileName As String, _
                    ByRef aIteration As Integer, _
                    ByRef aStartVariation As Integer, _
                    ByRef aModifiedData As atcDataGroup)

        If Not g_running Then
            UpdateStatusLabel("Stopping Run")
        Else
            Logger.Dbg("Run")
            ChDriveDir(PathNameOnly(aBaseFileName))

            If aStartVariation >= aVariations.Count Then 'All variations have values, do a model run
                UpdateStatusLabel(aIteration)
                If chkSaveAll.Checked Then aModifiedScenarioName &= "-" & aIteration + 1
                TimePerRun = Now.ToOADate
                Dim lResults As atcCollection = ScenarioRun(aBaseFileName, aModifiedScenarioName, aModifiedData, chkShowEachRunProgress.Checked)
                If lResults Is Nothing Then
                    Logger.Dbg("Null scenario results from ScenarioRun")
                    Exit Sub
                End If
                TimePerRun = (Now.ToOADate - TimePerRun) * 24 * 60 * 60 'Convert days to seconds

                UpdateResults(aIteration, lResults, PathNameOnly(aBaseFileName) & "\" & aModifiedScenarioName & ".results.txt")

                'Close any open results
                For Each lSpecification As String In lResults
                    lSpecification = lSpecification.ToLower
                    Dim lMatchDataSource As atcDataSource = Nothing
                    For Each lDataSource As atcDataSource In g_DataManager.DataSources
                        If lDataSource.Specification.ToLower = lSpecification Then
                            lMatchDataSource = lDataSource
                            Exit For
                        End If
                    Next
                    If Not lMatchDataSource Is Nothing Then
                        'lMatchDataSource.clear 'TODO: want to make sure we don't have a memory leak here
                        g_DataManager.DataSources.Remove(lMatchDataSource)
                    End If
                Next

                aIteration += 1

            Else 'Need to loop through values for next variation
                Dim lVariation As Variation = aVariations.ItemByIndex(aStartVariation)
                With lVariation
                    Dim lModifiedGroup As atcDataGroup = .StartIteration
                    While g_running And Not lModifiedGroup Is Nothing
                        If aModifiedData Is Nothing Then aModifiedData = New atcDataGroup

                        aModifiedData.Add(lModifiedGroup)

                        'We have handled a variation, now handle more input variations or run the model
                        Run(aModifiedScenarioName, _
                            aVariations, _
                            aBaseFileName, _
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

    Private Sub UpdateResults(ByVal aIteration As Integer, ByVal aResults As atcCollection, ByVal aResultsFilename As String)
        With agdResults.Source
            Dim lRow As Integer = aIteration + .FixedRows
            Dim lColumn As Integer = .FixedColumns
            Dim lEndpoint As Variation

            .CellValue(lRow, 0) = aIteration + 1
            .CellColor(lRow, 0) = Drawing.SystemColors.Control

            For Each lEndpoint In pEndpoints
                If Not Double.IsNaN(lEndpoint.CurrentValue) Then 'This is an input, display current value
                    .CellValue(lRow, lColumn) = Format(lEndpoint.CurrentValue, "0.####")
                    lColumn += 1
                Else
                    For Each lOldData As atcDataSet In lEndpoint.DataSets
                        Dim lGroup As atcDataGroup = Nothing
                        Dim lOriginalDataSpec As String = lOldData.Attributes.GetValue("History 1", "").Substring(10)
                        Dim lResultDataSpec As String = aResults.ItemByKey(lOriginalDataSpec)
                        If Not lResultDataSpec Is Nothing Then
                            Dim lResultDataSource As atcDataSource = OpenDataSource(lResultDataSpec)
                            If Not lResultDataSource Is Nothing Then
                                lGroup = lResultDataSource.DataSets.FindData("ID", lOldData.Attributes.GetValue("ID"), 1)
                                If Not lGroup Is Nothing AndAlso lGroup.Count > 0 Then
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
                            End If
                        End If
                    Next
                End If
            Next
        End With
        agdResults.Refresh()
        Try
            Windows.Forms.Application.DoEvents()
        Catch
            'stop
        End Try
        SaveFileString(aResultsFilename, agdResults.Source.ToString)
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
                If Not lColumnTitle Is Nothing AndAlso lColumnTitle.Length > 0 Then
                    cboPivotRows.Items.Add(lColumnTitle)
                    cboPivotColumns.Items.Add(lColumnTitle)
                    cboPivotCells.Items.Add(lColumnTitle)
                    'TODO: default to last setting for each pivot 
                    If lColumnTitle.ToLower.StartsWith("prec") Then
                        cboPivotColumns.Text = lColumnTitle
                    ElseIf lColumnTitle.ToLower.StartsWith("air") Then
                        cboPivotRows.Text = lColumnTitle
                    ElseIf lColumnTitle.ToLower.StartsWith("flow") Then
                        cboPivotCells.Text = lColumnTitle
                    End If
                End If
            Next
        End If
    End Sub

    Private Function UniqueValuesInColumn(ByVal aSource As atcGridSource, ByVal aColumn As Integer) As ArrayList
        Dim lLastRow As Integer = aSource.Rows - 1
        Dim lValues As New ArrayList
        Dim lCheckValue As String
        Dim lMatch As Boolean
        For lrow As Integer = aSource.FixedRows To lLastRow
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
        For lRow As Integer = aSource.FixedRows To aSource.Rows - 1
            Try
                If aSource.CellValue(lRow, aCol1) = aCol1Value AndAlso _
                   aSource.CellValue(lRow, aCol2) = aCol2Value Then
                    Return lRow
                End If
            Catch 'error indicates match not found
            End Try
        Next
        Return -1
    End Function

    Private Sub PopulatePivotTable()
        If cboPivotRows.Text.Length > 0 AndAlso cboPivotColumns.Text.Length > 0 AndAlso cboPivotCells.Text.Length > 0 Then
            Dim lPivotData As New atcGridSource
            Dim lRuns As Integer = agdResults.Source.Rows - 1
            Dim lRunRow As Integer = agdResults.Source.FixedRows
            Dim lRunColumn As Integer = agdResults.Source.FixedColumns
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

    Private Sub frmCAT_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not pPlugin Is Nothing Then
            pPlugin.XML = Me.XML
        End If
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

    Private Sub UpdateStatusLabel(ByVal aText As String)
        Logger.Dbg(aText)
        lblTop.Text = aText
        lblTop.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer)
        Dim lLabelText As String = "Running # " & aIteration + 1 & " of " & TotalIterations
        If TimePerRun > 0 Then
            lLabelText &= " (" & FormatTime(TimePerRun * (TotalIterations - aIteration)) & " remaining)"
        End If
        UpdateStatusLabel(lLabelText)
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

            'Dim lEndpoint As Variation = lVariation.Clone
            'lEndpoint.Operation = "Current Value"
            pEndpoints.Add(lVariation)
            RefreshEndpointList()
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

    Private Sub MoveItem(ByVal aGroup As atcCollection, ByVal aList As CheckedListBox, ByVal aMoveTo As Integer)
        Dim lMoveFrom As Integer = aList.SelectedIndex
        If lMoveFrom >= 0 AndAlso aMoveTo >= 0 AndAlso lMoveFrom < aGroup.Count AndAlso aMoveTo < aGroup.Count Then
            Dim lWasChecked As Boolean = aList.CheckedIndices.Contains(lMoveFrom)
            Dim lMoveMe As Variation = aGroup.ItemByIndex(lMoveFrom)
            aGroup.RemoveAt(lMoveFrom)
            aList.Items.RemoveAt(lMoveFrom)
            aGroup.Insert(aMoveTo, lMoveMe)
            aList.Items.Insert(aMoveTo, lMoveMe.ToString)
            If lWasChecked Then aList.SetItemChecked(aMoveTo, True)
            aList.SelectedIndex = aMoveTo
        End If
    End Sub

    Private Sub btnInputUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputUp.Click
        MoveItem(pInputs, lstInputs, lstInputs.SelectedIndex - 1)
    End Sub

    Private Sub btnInputUp_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputUp.DoubleClick
        MoveItem(pInputs, lstInputs, 0)
    End Sub

    Private Sub btnInputDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputDown.Click
        MoveItem(pInputs, lstInputs, lstInputs.SelectedIndex + 1)
    End Sub

    Private Sub btnInputDown_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInputDown.DoubleClick
        MoveItem(pInputs, lstInputs, lstInputs.Items.Count - 1)
    End Sub

    Private Sub btnEndpointUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointUp.Click
        MoveItem(pEndpoints, lstEndpoints, lstEndpoints.SelectedIndex - 1)
    End Sub

    Private Sub btnEndpointUp_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointUp.DoubleClick
        MoveItem(pEndpoints, lstEndpoints, 0)
    End Sub

    Private Sub btnEndpointDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointDown.Click
        MoveItem(pEndpoints, lstEndpoints, lstEndpoints.SelectedIndex + 1)
    End Sub

    Private Sub btnEndpointDown_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointDown.DoubleClick
        MoveItem(pEndpoints, lstEndpoints, lstEndpoints.Items.Count - 1)
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
        TotalIterations = 1

        For Each lVariation As Variation In pInputs
            If lVariation.Selected Then
                TotalIterations *= lVariation.Iterations
            End If
        Next

        lLabelText = "Total iterations selected = " & TotalIterations
        If TimePerRun > 0 Then
            lLabelText &= " (" & FormatTime(TimePerRun * TotalIterations) & ")"
        End If
        UpdateStatusLabel(lLabelText)

        'TODO: calculate size of all inputs/outputs for display
        'Try
        '    lblAllResults.Text = "(" & Format((FileLen(cboBaseScenarioName.Text) * TotalIterations) / 1048576, "#,##0.#") & " Meg)"
        'Catch
        '    lblAllResults.Text = ""
        'End Try
    End Sub

    Private Sub RefreshInputList()
        RefreshList(lstInputs, pInputs)
    End Sub

    Private Sub RefreshEndpointList()
        RefreshList(lstEndpoints, pEndpoints)
    End Sub

    Private Sub RefreshList(ByVal aList As System.Windows.Forms.CheckedListBox, ByVal aVariations As atcCollection)
        aList.Items.Clear()
        For Each lVariation As Variation In aVariations
            aList.Items.Add(lVariation.ToString)
            aList.SetItemChecked(aList.Items.Count - 1, lVariation.Selected)
        Next
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
                RefreshEndpointList()
            End If
        End If
    End Sub

    Private Sub btnEndpointRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEndpointRemove.Click
        For lIndex As Integer = pEndpoints.Count - 1 To 0 Step -1
            If lstEndpoints.GetSelected(lIndex) Then
                pEndpoints.RemoveAt(lIndex)
            End If
        Next
        RefreshEndpointList()
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
            Dim lXML As String = ""

            lXML &= "<SaveAll>" & chkSaveAll.Checked & "</SaveAll>" & vbCrLf

            lXML &= "<ShowEachRun>" & chkShowEachRunProgress.Checked & "</ShowEachRun>" & vbCrLf

            lXML &= "<UCI>" & vbCrLf
            lXML &= "  <FileName>" & txtBaseScenario.Text & "</FileName>" & vbCrLf
            lXML &= "</UCI>" & vbCrLf

            Dim lVariation As Variation
            lXML &= "<Variations>" & vbCrLf
            For Each lVariation In pInputs
                lXML &= lVariation.XML
            Next
            lXML &= "</Variations>" & vbCrLf

            lXML &= "<Endpoints>" & vbCrLf
            For Each lVariation In pEndpoints
                lXML &= lVariation.XML
            Next
            lXML &= "</Endpoints>" & vbCrLf

            Dim lStartFolder As String = CurDir()
            lXML = ReplaceStringNoCase(lXML, lStartFolder, StartFolderVariable)
            If lXML.Contains(StartFolderVariable) Then
                lXML = "<StartFolder>" & lStartFolder & "</StartFolder>" & vbCrLf & lXML
            End If

            lXML = "<BasinsCAT>" & vbCrLf & lXML & "</BasinsCAT>" & vbCrLf
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

                'Replace start folder in all XML if present
                If lXML.Tag.ToLower = "startfolder" Then
                    Dim lStartFolder As String = lXML.Content
                    aValue = ReplaceString(aValue, StartFolderVariable, lStartFolder)
                    lXML.LoadXml(aValue)
                    lXML.FirstChild2()
                End If

                Do
                    Dim lVariation As Variation
                    Dim lChild As Chilkat.Xml = lXML.FirstChild
                    Select Case lXML.Tag.ToLower
                        Case "saveall"
                            chkSaveAll.Checked = (lXML.Content.ToLower = "true")
                        Case "showeachrun"
                            chkShowEachRunProgress.Checked = (lXML.Content.ToLower = "true")
                        Case "uci"
                            OpenUCI(AbsolutePath(lChild.Content, CurDir))
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
            .Title = Me.Text & " - Load Variations"
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
        g_running = False
        UpdateStatusLabel("Stopping")
    End Sub

    Private Sub lstInputs_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lstInputs.ItemCheck
        Dim lVariation As Variation = pInputs.ItemByIndex(e.Index)
        lVariation.Selected = (e.NewValue = CheckState.Checked)
        RefreshTotalIterations()
    End Sub

    Private Sub lstEndpoints_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lstEndpoints.ItemCheck
        Dim lVariation As Variation = pEndpoints.ItemByIndex(e.Index)
        lVariation.Selected = (e.NewValue = CheckState.Checked)
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Climate Assessment Tool.html")
    End Sub

    ''' <summary>
    ''' Add a copy of the currently selected endpoint(s) to the list
    ''' </summary>
    Private Sub btnEndpointCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEndpointCopy.Click
        Dim lCopyNumber As Integer
        Dim lCopyText As String = " copy "
        For Each lIndex As Integer In lstEndpoints.SelectedIndices
            Dim lNewEndpoint As Variation = pEndpoints(lIndex).Clone
            lNewEndpoint.CurrentValue = Double.NaN
            Dim lCopyTextPosition As Integer = lNewEndpoint.Name.LastIndexOf(lCopyText)
            lCopyNumber = 1
            If (lCopyTextPosition > 0) Then
                If IsNumeric(lNewEndpoint.Name.Substring(lCopyTextPosition + 6)) Then
                    lCopyNumber = CInt(lNewEndpoint.Name.Substring(lCopyTextPosition + lCopyText.Length)) + 1
                    lNewEndpoint.Name = lNewEndpoint.Name.Substring(0, lCopyTextPosition) 'remove " copy 1" from name
                End If
            End If
            While lstEndpoints.Items.Contains(lNewEndpoint.Name & lCopyText & lCopyNumber)
                lCopyNumber += 1
            End While
            lNewEndpoint.Name &= lCopyText & lCopyNumber
            pEndpoints.Add(lNewEndpoint)
        Next
        RefreshEndpointList()
    End Sub

    Private Sub mnuOpenUCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenUCI.Click
        OpenUCI()
    End Sub

    Private Sub txtBaseScenario_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtBaseScenario.MouseClick
        OpenUCI()
    End Sub

    ''' <summary>
    ''' Open data files referred to in this UCI file
    ''' </summary>
    ''' <param name="aUCIfilename">Full path of UCI file</param>
    ''' <remarks></remarks>
    Private Sub OpenUCI(Optional ByVal aUCIfilename As String = "")

        If Not aUCIfilename Is Nothing And Not FileExists(aUCIfilename) Then
            If FileExists(aUCIfilename & ".uci") Then aUCIfilename &= ".uci"
        End If

        If aUCIfilename Is Nothing OrElse Not FileExists(aUCIfilename) Then
            Dim cdlg As New OpenFileDialog
            cdlg.Title = "Open UCI file containing base scenario"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aUCIfilename = cdlg.FileName
            End If
        End If

        If FileExists(aUCIfilename) Then
            pUciFile = aUCIfilename
            txtBaseScenario.Text = pUciFile
            Dim lUciFolder As String = PathNameOnly(pUciFile)
            ChDriveDir(lUciFolder)

            Dim lFullText As String = WholeFileString(pUciFile)
            For Each lWDMfilename As String In UCIFilesBlockFilenames(lFullText, "WDM")
                lWDMfilename = AbsolutePath(lWDMfilename, lUciFolder)
                OpenDataSource(lWDMfilename)
            Next
            For Each lBinOutFilename As String In UCIFilesBlockFilenames(lFullText, "BINO")
                lBinOutFilename = AbsolutePath(lBinOutFilename, lUciFolder)
                OpenDataSource(lBinOutFilename)
            Next
        End If
    End Sub

End Class
