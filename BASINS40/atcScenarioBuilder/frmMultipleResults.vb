Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Public Class frmMultipleResults
  Inherits System.Windows.Forms.Form

  Private pDataManager As atcDataManager
  Private pDataToVary As atcDataGroup
  Private pResultsToTrack As New atcDataGroup
  Private pComputation As atcDataSource
  Private pSeasonalPlugin As New atcSeasons.atcSeasonPlugin
  Private pSeasonsAvailable As atcDataAttributes

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
  Friend WithEvents tabRowPerRun As System.Windows.Forms.TabPage
  Friend WithEvents tabPivot As System.Windows.Forms.TabPage
  Friend WithEvents cboRows As System.Windows.Forms.ComboBox
  Friend WithEvents lblRows As System.Windows.Forms.Label
  Friend WithEvents cboColumns As System.Windows.Forms.ComboBox
  Friend WithEvents cboCells As System.Windows.Forms.ComboBox
  Friend WithEvents tabSpecify As System.Windows.Forms.TabPage
  Friend WithEvents txtFunction As System.Windows.Forms.TextBox
  Friend WithEvents txtVaryData As System.Windows.Forms.TextBox
  Friend WithEvents grpParameter As System.Windows.Forms.GroupBox
  Friend WithEvents txtMax As System.Windows.Forms.TextBox
  Friend WithEvents txtMin As System.Windows.Forms.TextBox
  Friend WithEvents lblMaximum As System.Windows.Forms.Label
  Friend WithEvents lblMinimum As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents btnStart As System.Windows.Forms.Button
  Friend WithEvents Tabs As System.Windows.Forms.TabControl
  Friend WithEvents agdPivot As atcControls.atcGrid
  Friend WithEvents txtIncrement As System.Windows.Forms.TextBox
  Friend WithEvents lblIncrement As System.Windows.Forms.Label
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
  Friend WithEvents mnuOpenResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSaveResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSavePivot As System.Windows.Forms.MenuItem
  Friend WithEvents mnuCopyResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuCopyPivot As System.Windows.Forms.MenuItem
  Friend WithEvents mnuPasteResults As System.Windows.Forms.MenuItem
  Friend WithEvents grpSeasons As System.Windows.Forms.GroupBox
  Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
  Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
  Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
  Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
  Friend WithEvents lblColumns As System.Windows.Forms.Label
  Friend WithEvents lblCells As System.Windows.Forms.Label
  Friend WithEvents agdResults As atcControls.atcGrid
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMultipleResults))
    Me.Tabs = New System.Windows.Forms.TabControl
    Me.tabSpecify = New System.Windows.Forms.TabPage
    Me.grpSeasons = New System.Windows.Forms.GroupBox
    Me.btnSeasonsNone = New System.Windows.Forms.Button
    Me.btnSeasonsAll = New System.Windows.Forms.Button
    Me.lstSeasons = New System.Windows.Forms.ListBox
    Me.cboSeasons = New System.Windows.Forms.ComboBox
    Me.btnStart = New System.Windows.Forms.Button
    Me.txtFunction = New System.Windows.Forms.TextBox
    Me.txtVaryData = New System.Windows.Forms.TextBox
    Me.grpParameter = New System.Windows.Forms.GroupBox
    Me.txtIncrement = New System.Windows.Forms.TextBox
    Me.lblIncrement = New System.Windows.Forms.Label
    Me.txtMax = New System.Windows.Forms.TextBox
    Me.txtMin = New System.Windows.Forms.TextBox
    Me.lblMaximum = New System.Windows.Forms.Label
    Me.lblMinimum = New System.Windows.Forms.Label
    Me.Label4 = New System.Windows.Forms.Label
    Me.Label5 = New System.Windows.Forms.Label
    Me.tabRowPerRun = New System.Windows.Forms.TabPage
    Me.agdResults = New atcControls.atcGrid
    Me.tabPivot = New System.Windows.Forms.TabPage
    Me.lblCells = New System.Windows.Forms.Label
    Me.cboCells = New System.Windows.Forms.ComboBox
    Me.lblColumns = New System.Windows.Forms.Label
    Me.cboColumns = New System.Windows.Forms.ComboBox
    Me.lblRows = New System.Windows.Forms.Label
    Me.cboRows = New System.Windows.Forms.ComboBox
    Me.agdPivot = New atcControls.atcGrid
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuOpenResults = New System.Windows.Forms.MenuItem
    Me.mnuSaveResults = New System.Windows.Forms.MenuItem
    Me.mnuSavePivot = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuCopyResults = New System.Windows.Forms.MenuItem
    Me.mnuCopyPivot = New System.Windows.Forms.MenuItem
    Me.mnuPasteResults = New System.Windows.Forms.MenuItem
    Me.mnuHelp = New System.Windows.Forms.MenuItem
    Me.Tabs.SuspendLayout()
    Me.tabSpecify.SuspendLayout()
    Me.grpSeasons.SuspendLayout()
    Me.grpParameter.SuspendLayout()
    Me.tabRowPerRun.SuspendLayout()
    Me.tabPivot.SuspendLayout()
    Me.SuspendLayout()
    '
    'Tabs
    '
    Me.Tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.Tabs.Controls.Add(Me.tabSpecify)
    Me.Tabs.Controls.Add(Me.tabRowPerRun)
    Me.Tabs.Controls.Add(Me.tabPivot)
    Me.Tabs.Location = New System.Drawing.Point(10, 9)
    Me.Tabs.Name = "Tabs"
    Me.Tabs.SelectedIndex = 0
    Me.Tabs.Size = New System.Drawing.Size(662, 545)
    Me.Tabs.TabIndex = 0
    '
    'tabSpecify
    '
    Me.tabSpecify.Controls.Add(Me.grpSeasons)
    Me.tabSpecify.Controls.Add(Me.btnStart)
    Me.tabSpecify.Controls.Add(Me.txtFunction)
    Me.tabSpecify.Controls.Add(Me.txtVaryData)
    Me.tabSpecify.Controls.Add(Me.grpParameter)
    Me.tabSpecify.Controls.Add(Me.Label4)
    Me.tabSpecify.Controls.Add(Me.Label5)
    Me.tabSpecify.Location = New System.Drawing.Point(4, 25)
    Me.tabSpecify.Name = "tabSpecify"
    Me.tabSpecify.Size = New System.Drawing.Size(654, 516)
    Me.tabSpecify.TabIndex = 2
    Me.tabSpecify.Text = "Specify Runs"
    '
    'grpSeasons
    '
    Me.grpSeasons.Controls.Add(Me.btnSeasonsNone)
    Me.grpSeasons.Controls.Add(Me.btnSeasonsAll)
    Me.grpSeasons.Controls.Add(Me.lstSeasons)
    Me.grpSeasons.Controls.Add(Me.cboSeasons)
    Me.grpSeasons.Location = New System.Drawing.Point(288, 83)
    Me.grpSeasons.Name = "grpSeasons"
    Me.grpSeasons.Size = New System.Drawing.Size(240, 379)
    Me.grpSeasons.TabIndex = 27
    Me.grpSeasons.TabStop = False
    Me.grpSeasons.Text = "Seasons"
    '
    'btnSeasonsNone
    '
    Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsNone.Location = New System.Drawing.Point(154, 336)
    Me.btnSeasonsNone.Name = "btnSeasonsNone"
    Me.btnSeasonsNone.Size = New System.Drawing.Size(76, 26)
    Me.btnSeasonsNone.TabIndex = 12
    Me.btnSeasonsNone.Text = "None"
    '
    'btnSeasonsAll
    '
    Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnSeasonsAll.Location = New System.Drawing.Point(10, 336)
    Me.btnSeasonsAll.Name = "btnSeasonsAll"
    Me.btnSeasonsAll.Size = New System.Drawing.Size(76, 27)
    Me.btnSeasonsAll.TabIndex = 11
    Me.btnSeasonsAll.Text = "All"
    '
    'lstSeasons
    '
    Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstSeasons.IntegralHeight = False
    Me.lstSeasons.ItemHeight = 16
    Me.lstSeasons.Location = New System.Drawing.Point(10, 46)
    Me.lstSeasons.Name = "lstSeasons"
    Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
    Me.lstSeasons.Size = New System.Drawing.Size(220, 281)
    Me.lstSeasons.TabIndex = 7
    '
    'cboSeasons
    '
    Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.cboSeasons.Location = New System.Drawing.Point(10, 18)
    Me.cboSeasons.MaxDropDownItems = 20
    Me.cboSeasons.Name = "cboSeasons"
    Me.cboSeasons.Size = New System.Drawing.Size(220, 24)
    Me.cboSeasons.TabIndex = 6
    '
    'btnStart
    '
    Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnStart.Location = New System.Drawing.Point(67, 435)
    Me.btnStart.Name = "btnStart"
    Me.btnStart.Size = New System.Drawing.Size(106, 28)
    Me.btnStart.TabIndex = 26
    Me.btnStart.Text = "Start"
    '
    'txtFunction
    '
    Me.txtFunction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFunction.Location = New System.Drawing.Point(154, 46)
    Me.txtFunction.Name = "txtFunction"
    Me.txtFunction.Size = New System.Drawing.Size(490, 22)
    Me.txtFunction.TabIndex = 21
    Me.txtFunction.Text = "Multiply"
    '
    'txtVaryData
    '
    Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVaryData.Location = New System.Drawing.Point(154, 9)
    Me.txtVaryData.Name = "txtVaryData"
    Me.txtVaryData.Size = New System.Drawing.Size(490, 22)
    Me.txtVaryData.TabIndex = 20
    Me.txtVaryData.Text = ""
    '
    'grpParameter
    '
    Me.grpParameter.Controls.Add(Me.txtIncrement)
    Me.grpParameter.Controls.Add(Me.lblIncrement)
    Me.grpParameter.Controls.Add(Me.txtMax)
    Me.grpParameter.Controls.Add(Me.txtMin)
    Me.grpParameter.Controls.Add(Me.lblMaximum)
    Me.grpParameter.Controls.Add(Me.lblMinimum)
    Me.grpParameter.Location = New System.Drawing.Point(58, 83)
    Me.grpParameter.Name = "grpParameter"
    Me.grpParameter.Size = New System.Drawing.Size(201, 120)
    Me.grpParameter.TabIndex = 19
    Me.grpParameter.TabStop = False
    Me.grpParameter.Text = "Variation Parameter"
    '
    'txtIncrement
    '
    Me.txtIncrement.Location = New System.Drawing.Point(96, 83)
    Me.txtIncrement.Name = "txtIncrement"
    Me.txtIncrement.Size = New System.Drawing.Size(86, 22)
    Me.txtIncrement.TabIndex = 17
    Me.txtIncrement.Text = "0.05"
    '
    'lblIncrement
    '
    Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
    Me.lblIncrement.ImageAlign = System.Drawing.ContentAlignment.BottomRight
    Me.lblIncrement.Location = New System.Drawing.Point(10, 88)
    Me.lblIncrement.Name = "lblIncrement"
    Me.lblIncrement.Size = New System.Drawing.Size(76, 20)
    Me.lblIncrement.TabIndex = 16
    Me.lblIncrement.Text = "Increment:"
    Me.lblIncrement.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'txtMax
    '
    Me.txtMax.Location = New System.Drawing.Point(96, 55)
    Me.txtMax.Name = "txtMax"
    Me.txtMax.Size = New System.Drawing.Size(86, 22)
    Me.txtMax.TabIndex = 15
    Me.txtMax.Text = "1.1"
    '
    'txtMin
    '
    Me.txtMin.Location = New System.Drawing.Point(96, 28)
    Me.txtMin.Name = "txtMin"
    Me.txtMin.Size = New System.Drawing.Size(86, 22)
    Me.txtMin.TabIndex = 14
    Me.txtMin.Text = "0.9"
    '
    'lblMaximum
    '
    Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
    Me.lblMaximum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblMaximum.Location = New System.Drawing.Point(10, 60)
    Me.lblMaximum.Name = "lblMaximum"
    Me.lblMaximum.Size = New System.Drawing.Size(76, 21)
    Me.lblMaximum.TabIndex = 9
    Me.lblMaximum.Text = "Maximum:"
    Me.lblMaximum.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'lblMinimum
    '
    Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
    Me.lblMinimum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
    Me.lblMinimum.Location = New System.Drawing.Point(10, 32)
    Me.lblMinimum.Name = "lblMinimum"
    Me.lblMinimum.Size = New System.Drawing.Size(76, 21)
    Me.lblMinimum.TabIndex = 8
    Me.lblMinimum.Text = "Minimum:"
    Me.lblMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Label4
    '
    Me.Label4.BackColor = System.Drawing.Color.Transparent
    Me.Label4.Location = New System.Drawing.Point(19, 51)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(125, 21)
    Me.Label4.TabIndex = 18
    Me.Label4.Text = "Variation Function:"
    Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'Label5
    '
    Me.Label5.BackColor = System.Drawing.Color.Transparent
    Me.Label5.Location = New System.Drawing.Point(19, 18)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(125, 21)
    Me.Label5.TabIndex = 17
    Me.Label5.Text = "Data to Vary:"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'tabRowPerRun
    '
    Me.tabRowPerRun.Controls.Add(Me.agdResults)
    Me.tabRowPerRun.Location = New System.Drawing.Point(4, 25)
    Me.tabRowPerRun.Name = "tabRowPerRun"
    Me.tabRowPerRun.Size = New System.Drawing.Size(654, 516)
    Me.tabRowPerRun.TabIndex = 0
    Me.tabRowPerRun.Text = "Results"
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
    Me.agdResults.Location = New System.Drawing.Point(10, 9)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(633, 495)
    Me.agdResults.Source = Nothing
    Me.agdResults.TabIndex = 0
    '
    'tabPivot
    '
    Me.tabPivot.Controls.Add(Me.lblCells)
    Me.tabPivot.Controls.Add(Me.cboCells)
    Me.tabPivot.Controls.Add(Me.lblColumns)
    Me.tabPivot.Controls.Add(Me.cboColumns)
    Me.tabPivot.Controls.Add(Me.lblRows)
    Me.tabPivot.Controls.Add(Me.cboRows)
    Me.tabPivot.Controls.Add(Me.agdPivot)
    Me.tabPivot.Location = New System.Drawing.Point(4, 25)
    Me.tabPivot.Name = "tabPivot"
    Me.tabPivot.Size = New System.Drawing.Size(654, 516)
    Me.tabPivot.TabIndex = 1
    Me.tabPivot.Text = "Pivot"
    '
    'lblCells
    '
    Me.lblCells.Location = New System.Drawing.Point(10, 78)
    Me.lblCells.Name = "lblCells"
    Me.lblCells.Size = New System.Drawing.Size(67, 19)
    Me.lblCells.TabIndex = 6
    Me.lblCells.Text = "Cells"
    Me.lblCells.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboCells
    '
    Me.cboCells.Location = New System.Drawing.Point(86, 74)
    Me.cboCells.Name = "cboCells"
    Me.cboCells.Size = New System.Drawing.Size(135, 22)
    Me.cboCells.TabIndex = 5
    '
    'lblColumns
    '
    Me.lblColumns.Location = New System.Drawing.Point(10, 51)
    Me.lblColumns.Name = "lblColumns"
    Me.lblColumns.Size = New System.Drawing.Size(67, 18)
    Me.lblColumns.TabIndex = 4
    Me.lblColumns.Text = "Columns"
    Me.lblColumns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboColumns
    '
    Me.cboColumns.Location = New System.Drawing.Point(86, 46)
    Me.cboColumns.Name = "cboColumns"
    Me.cboColumns.Size = New System.Drawing.Size(135, 22)
    Me.cboColumns.TabIndex = 3
    '
    'lblRows
    '
    Me.lblRows.Location = New System.Drawing.Point(10, 23)
    Me.lblRows.Name = "lblRows"
    Me.lblRows.Size = New System.Drawing.Size(67, 19)
    Me.lblRows.TabIndex = 2
    Me.lblRows.Text = "Rows"
    Me.lblRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboRows
    '
    Me.cboRows.Location = New System.Drawing.Point(86, 18)
    Me.cboRows.Name = "cboRows"
    Me.cboRows.Size = New System.Drawing.Size(135, 22)
    Me.cboRows.TabIndex = 1
    '
    'agdPivot
    '
    Me.agdPivot.AllowHorizontalScrolling = True
    Me.agdPivot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdPivot.CellBackColor = System.Drawing.Color.Empty
    Me.agdPivot.LineColor = System.Drawing.Color.Empty
    Me.agdPivot.LineWidth = 0.0!
    Me.agdPivot.Location = New System.Drawing.Point(10, 129)
    Me.agdPivot.Name = "agdPivot"
    Me.agdPivot.Size = New System.Drawing.Size(633, 379)
    Me.agdPivot.Source = Nothing
    Me.agdPivot.TabIndex = 0
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuHelp})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpenResults, Me.mnuSaveResults, Me.mnuSavePivot})
    Me.mnuFile.Text = "File"
    '
    'mnuOpenResults
    '
    Me.mnuOpenResults.Index = 0
    Me.mnuOpenResults.Text = "Open Results"
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
    'mnuHelp
    '
    Me.mnuHelp.Index = 2
    Me.mnuHelp.Text = "Help"
    '
    'frmMultipleResults
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(680, 558)
    Me.Controls.Add(Me.Tabs)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmMultipleResults"
    Me.Text = "Multiple Runs"
    Me.Tabs.ResumeLayout(False)
    Me.tabSpecify.ResumeLayout(False)
    Me.grpSeasons.ResumeLayout(False)
    Me.grpParameter.ResumeLayout(False)
    Me.tabRowPerRun.ResumeLayout(False)
    Me.tabPivot.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub Initialize(ByVal aDataManager As atcDataManager, _
           Optional ByVal aTimeseriesGroup As atcDataGroup = Nothing)
    pSeasonsAvailable = pSeasonalPlugin.AvailableOperations(True, False)
    pDataManager = aDataManager
    pDataToVary = aTimeseriesGroup
    If pDataToVary Is Nothing Then pDataToVary = New atcDataGroup
    UpdateDataText(txtVaryData, pDataToVary)
    cboSeasons.Items.Add("No seasons")
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      cboSeasons.Items.Add(lSeason.Definition.Name.Substring(0, lSeason.Definition.Name.IndexOf("::")))
    Next

    Me.Show()

  End Sub

  Private Sub txtVaryData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVaryData.Click
    pDataToVary = pDataManager.UserSelectData("Select data to vary", pDataToVary)
    UpdateDataText(txtVaryData, pDataToVary)
  End Sub

  Private Sub txtFunction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFunction.Click
    Dim aCategory As New ArrayList(1)
    aCategory.Add("Compute")
    pComputation = pDataManager.UserSelectDataSource(aCategory, "Select Function for Varying Input Data")
    If Not pComputation Is Nothing Then
      txtFunction.Text = pComputation.ToString
    End If
  End Sub

  'Private Sub txtResultData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  pResultsToTrack = pDataManager.UserSelectData("Select results of interest", pResultsToTrack)
  '  UpdateDataText(txtResultData, pResultsToTrack)
  'End Sub

  Private Sub UpdateDataText(ByVal aTextBox As Windows.Forms.TextBox, _
                             ByVal aGroup As atcDataGroup)
    If aGroup.Count > 0 Then
      aTextBox.Text = aGroup.ItemByIndex(0).ToString
      If aGroup.Count > 1 Then aTextBox.Text &= " (and " & aGroup.Count - 1 & " more)"
    Else
      aTextBox.Text = "Click to select data"
    End If
  End Sub

  Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
    Dim lOutputAttributes As String() = {"Mean", "Min", "Max", "1Hi100", "7Q10"}
    Dim lOutputConsName() As String = {"Flow", "Flow", "Flow", "Flow", "Flow"}
    Dim lWDMFileName As String = "base.wdm"

    'group containing dsn 122 (hourly AirTemp) from base.wdm
    Dim lOrigAirTmp As New atcDataGroup
    lOrigAirTmp = pDataManager.DataSets.FindData("ID", "122", 0)
    If lOrigAirTmp.Count = 0 Then
      Dim lWDMfile As atcDataSource = New atcWDM.atcDataSourceWDM
      pDataManager.OpenDataSource(lWDMfile, lWDMFileName, Nothing)
      lOrigAirTmp = lWDMfile.DataSets.FindData("ID", "122", 0)
    End If
    'lSummary.Save(pDataManager, lOrigAirTmp, "DataTree_OrigAirTmp.txt")

    'header for attributes
    agdResults.Source = New atcGridSource
    With agdResults.Source
      .FixedRows = 1
      .Columns = 5 + lOutputAttributes.GetUpperBound(0) + 1
      .Rows = 2
      .CellValue(0, 0) = "Add AirTmp"
      .CellValue(0, 1) = "MeanDay AirTmp"
      .CellValue(0, 2) = "MeanAnn Evap"
      .CellValue(0, 3) = "Mult8&9 Precip"
      .CellValue(0, 4) = "AveAnn Precip"
      For lIndex As Integer = 0 To lOutputAttributes.GetUpperBound(0)
        .CellValue(0, 5 + lIndex) = lOutputAttributes(lIndex) & " " & lOutputConsName(lIndex)
      Next
    End With
    agdResults.Initialize(agdResults.Source)
    agdResults.Refresh()
    PopulatePivotCombos()

    Tabs.SelectedIndex = 1

    Run(pDataToVary.ItemByIndex(0), CDbl(txtMin.Text), CDbl(txtMax.Text), CDbl(txtIncrement.Text), _
        lOrigAirTmp.ItemByIndex(0), 0, 2, 0.5, _
        lOutputAttributes, lOutputConsName, lWDMFileName)

    'group containing dsn 105 (hourly Precip) from base.wdm
    'Dim lOrigPrecip As New atcDataGroup
    'lOrigPrecip = pDataManager.DataSets.FindData("ID", "105", 0)
    'lSummary.Save(pDataManager, lOrigPrecip, "DataTree_OrigPrecip.txt")
  End Sub

  Private Sub Run(ByVal aBase1 As atcDataSet, _
                  ByVal aMin1 As Double, _
                  ByVal aMax1 As Double, _
                  ByVal aIncrement1 As Double, _
                  ByVal aBase2 As atcDataSet, _
                  ByVal aMin2 As Double, _
                  ByVal aMax2 As Double, _
                  ByVal aIncrement2 As Double, _
                  ByVal aOutputAttributes() As String, _
                  ByVal aOutputConsName() As String, _
                  ByVal aWDMFileName As String)
    Dim pTestPath As String = "c:\test\climate\"
    Logger.Dbg("Start")
    ChDriveDir(pTestPath)
    Logger.Dbg(" CurDir:" & CurDir())

    Dim lArgs As Object()
    Dim lErr As String

    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
    'Dim lSummary As atcDataTreePlugin = New atcDataTree.atcDataTreePlugin
    Dim lMetCmp As New atcMetCmp.atcMetCmpPlugin
    Dim lCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
    Dim lArgsMath As New atcDataAttributes

    Dim lIndex As Integer
    Dim lRow As Integer = 0

    Dim lNewPrec As atcTimeseries
    Dim lNewEvap As atcTimeseries
    Dim lOutputFlow As New atcDataGroup

    For lCurValue2 As Double = aMin2 To aMax2 Step aIncrement2
      'build new temp and evap
      lTsMath.DataSets.Clear()
      lArgsMath.Clear()
      lArgsMath.SetValue("timeseries", aBase2)
      lArgsMath.SetValue("Number", lCurValue2)
      pDataManager.OpenDataSource(lTsMath, "add", lArgsMath)
      Dim lAirTmpMean As String = Format(lTsMath.DataSets(0).Attributes.GetValue("Mean"), "#.00")
      lArgsMath.Clear()
      lNewEvap = atcMetCmp.CmpHamX(lTsMath.DataSets(0), Nothing, True, 39, lCTS)
      lNewEvap.Attributes.SetValue("Constituent", "PET")
      lNewEvap.Attributes.SetValue("Id", 111)
      lNewEvap.Attributes.SetValue("Scenario", "modified")
      Dim lEvapMean As String = Format(lNewEvap.Attributes.GetValue("Mean") * 365.25, "#.00")
      'lSummary.Save(pDataManager, New atcDataGroup(lNewEvap), "DataTree_Evap" & lCurValue2 & ".txt")

      For lCurValue1 As Double = aMin1 To aMax1 Step aIncrement1
        Logger.Dbg(" Step:" & lCurValue2 & ":" & lCurValue1)
        lRow += 1
        With agdResults.Source
          .CellValue(lRow, 0) = lCurValue2
          .CellValue(lRow, 1) = lAirTmpMean
          .CellValue(lRow, 2) = lEvapMean
          .CellValue(lRow, 3) = lCurValue1
        End With
        agdResults.Refresh()

        lNewPrec = Nothing
        lNewPrec = aBase1.Clone
        For iValue As Integer = 1 To lNewPrec.numValues
          Dim lDate As Date = Date.FromOADate(lNewPrec.Dates.Value(iValue))
          Select Case lDate.Month
            Case 8, 9 : lNewPrec.Value(iValue) *= lCurValue1
          End Select
        Next
        lNewPrec.Attributes.CalculateAll()
        'TODO: REPLACE HARD CODED 10 YEARS WITH ACTUAL SPAN CALC!
        agdResults.Source.CellValue(lRow, 4) = Format(lNewPrec.Attributes.GetValue("Sum") / 10, "#.00")
        'lSummary.Save(pDataManager, New atcDataGroup(lNewPrec), "DataTree_Precip" & lCurValue1 & ".txt")

        Dim lScenarioResults = New atcDataSource
        Dim lModifiedData As New atcDataGroup
        lModifiedData.Add(lNewPrec)
        lModifiedData.Add(lNewEvap)
        lScenarioResults = ScenarioRun(aWDMFileName, "modified", lModifiedData)

        lOutputFlow = lScenarioResults.DataSets.FindData("ID", "1004", 0)
        'lSummary.Save(pDataManager, lOutputFlow, "DataTree_FlowB&M" & lTempMult & ":" & lCurValue1 & ".txt")
        For lIndex = 0 To aOutputAttributes.GetUpperBound(0)
          agdResults.Source.CellValue(lRow, 5 + lIndex) = Format(lOutputFlow.Item(0).Attributes.GetValue(aOutputAttributes(lIndex)), "#.0")
        Next
        agdResults.Refresh()
        Windows.Forms.Application.DoEvents()
      Next
    Next
  End Sub

  Private Sub cboRows_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRows.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub cboColumns_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboColumns.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub cboCells_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCells.SelectedIndexChanged
    PopulatePivotTable()
  End Sub

  Private Sub PopulatePivotCombos()
    cboRows.Items.Clear()
    cboColumns.Items.Clear()
    cboCells.Items.Clear()

    If Not agdResults.Source Is Nothing Then
      For iColumn As Integer = 0 To agdResults.Source.Columns - 1
        Dim lColumnTitle As String = agdResults.Source.CellValue(0, iColumn)
        cboRows.Items.Add(lColumnTitle)
        cboColumns.Items.Add(lColumnTitle)
        cboCells.Items.Add(lColumnTitle)
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
    If cboRows.Text.Length > 0 AndAlso cboColumns.Text.Length > 0 AndAlso cboCells.Text.Length > 0 Then
      Dim lPivotData As New atcGridSource
      Dim lRuns As Integer = agdResults.Source.Rows - 1
      Dim lRunRow As Integer = 0
      Dim lRunColumn As Integer = 0
      Dim lRunCell As Integer = 0

      While Not agdResults.Source.CellValue(0, lRunRow).Equals(cboRows.Text)
        lRunRow += 1
      End While
      While Not agdResults.Source.CellValue(0, lRunColumn).Equals(cboColumns.Text)
        lRunColumn += 1
      End While
      While Not agdResults.Source.CellValue(0, lRunCell).Equals(cboCells.Text)
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

        For lRow = 1 To .Rows - 1
          lRowValue = lRowValues(lRow - 1)
          .CellValue(lRow, 0) = lRowValue
          For lColumn = 1 To .Columns - 1
            lColumnValue = lColumnValues(lColumn - 1)
            .CellValue(0, lColumn) = lColumnValue
            lMatchRow = FindMatchingRow(agdResults.Source, lRunRow, lRowValue, lRunColumn, lColumnValue)
            If lMatchRow > 0 Then
              .CellValue(lRow, lColumn) = agdResults.Source.CellValue(lMatchRow, lRunCell)
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

  Private Sub mnuOpenResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenResults.Click
    Dim lOpenDialog As New Windows.Forms.OpenFileDialog
    With lOpenDialog
      .FileName = "results.txt"
      .Filter = "Text files (*.txt)|*.txt|All files|*.*"
      .FilterIndex = 1
      .Title = "Scenario Builder - Open Results"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        'read file into grid
        If FileExists(.FileName) Then
          PopulateGrid(agdResults, WholeFileString(.FileName))
          Tabs.SelectedIndex = 1
        End If
      End If
    End With
  End Sub

  Private Sub PopulateGrid(ByVal aGrid As atcGrid, ByVal aNewContentsOfGrid As String)
    With aGrid
      .Source = New atcGridSource
      .Initialize(agdResults.Source)
      .Source.FromString(aNewContentsOfGrid)
      .SizeAllColumnsToContents(.Width, True)
      .Refresh()
      PopulatePivotCombos()
    End With

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
        SaveFileString(.FileName, lblRows.Text & vbTab & cboRows.Text & vbCrLf _
                                & lblColumns.Text & vbTab & cboColumns.Text & vbCrLf _
                                & lblCells.Text & vbTab & cboCells.Text & vbCrLf _
                                & agdPivot.Source.ToString)
      End If
    End With
  End Sub

  Private Sub mnuPasteResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPasteResults.Click
    Dim iData As IDataObject = Clipboard.GetDataObject()
    If iData.GetDataPresent(DataFormats.Text) Then
      PopulateGrid(agdResults, CType(iData.GetData(DataFormats.Text), String))
    Else
      Logger.Msg("No text on clipboard to paste into grid", "Paste")
    End If
    Tabs.SelectedIndex = 1
  End Sub

  Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
    'lstSeasons.Items.Clear()
    'Dim lSeasonSource As atcSeasons.atcSeasonBase = CurrentSeason()
    'If Not lSeasonSource Is Nothing Then
    '  Dim lSeasonName As String = SeasonIndex lSeasonalAttribute.Arguments.GetValue("SeasonName") 'Definition.Name
    '    If Not lstSeasons.Items.Contains(lSeasonName) Then
    '      lstSeasons.Items.Add(lSeasonName)
    '      lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
    '    End If
    '  Next
    'End If
  End Sub

  Private Function CurrentSeason() As atcDataSource
    For Each lSeason As atcDefinedValue In pSeasonsAvailable
      If lSeason.Definition.Name.Equals(cboSeasons.Text & "::SeasonalAttributes") Then
        Return lSeason.Definition.Calculator
      End If
    Next
    Return Nothing
  End Function

End Class
