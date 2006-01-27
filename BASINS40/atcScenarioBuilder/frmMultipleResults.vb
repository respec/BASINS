Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmMultipleResults
  Inherits System.Windows.Forms.Form

  Private pDataManager As atcDataManager
  Private pDataToVary As atcDataGroup
  Private pResultsToTrack As New atcDataGroup
  Private pComputation As atcDataSource

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
  Friend WithEvents agdRowPerRun As atcControls.atcGrid
  Friend WithEvents cboRows As System.Windows.Forms.ComboBox
  Friend WithEvents lblRows As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents cboColumns As System.Windows.Forms.ComboBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents cboCells As System.Windows.Forms.ComboBox
  Friend WithEvents tabSpecify As System.Windows.Forms.TabPage
  Friend WithEvents txtResultAttribute As System.Windows.Forms.TextBox
  Friend WithEvents lblResultAttribute As System.Windows.Forms.Label
  Friend WithEvents txtResultData As System.Windows.Forms.TextBox
  Friend WithEvents lblResultData As System.Windows.Forms.Label
  Friend WithEvents txtFunction As System.Windows.Forms.TextBox
  Friend WithEvents txtVaryData As System.Windows.Forms.TextBox
  Friend WithEvents grpParameter As System.Windows.Forms.GroupBox
  Friend WithEvents txtMax As System.Windows.Forms.TextBox
  Friend WithEvents txtMin As System.Windows.Forms.TextBox
  Friend WithEvents rdoRandom As System.Windows.Forms.RadioButton
  Friend WithEvents rdoLog As System.Windows.Forms.RadioButton
  Friend WithEvents rdoLinear As System.Windows.Forms.RadioButton
  Friend WithEvents lblPattern As System.Windows.Forms.Label
  Friend WithEvents lblMaximum As System.Windows.Forms.Label
  Friend WithEvents lblMinimum As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents btnStart As System.Windows.Forms.Button
  Friend WithEvents Tabs As System.Windows.Forms.TabControl
  Friend WithEvents agdPivot As atcControls.atcGrid
  Friend WithEvents txtIncrement As System.Windows.Forms.TextBox
  Friend WithEvents lblIncrement As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMultipleResults))
    Me.Tabs = New System.Windows.Forms.TabControl
    Me.tabSpecify = New System.Windows.Forms.TabPage
    Me.btnStart = New System.Windows.Forms.Button
    Me.txtResultAttribute = New System.Windows.Forms.TextBox
    Me.lblResultAttribute = New System.Windows.Forms.Label
    Me.txtResultData = New System.Windows.Forms.TextBox
    Me.lblResultData = New System.Windows.Forms.Label
    Me.txtFunction = New System.Windows.Forms.TextBox
    Me.txtVaryData = New System.Windows.Forms.TextBox
    Me.grpParameter = New System.Windows.Forms.GroupBox
    Me.txtIncrement = New System.Windows.Forms.TextBox
    Me.lblIncrement = New System.Windows.Forms.Label
    Me.txtMax = New System.Windows.Forms.TextBox
    Me.txtMin = New System.Windows.Forms.TextBox
    Me.rdoRandom = New System.Windows.Forms.RadioButton
    Me.rdoLog = New System.Windows.Forms.RadioButton
    Me.rdoLinear = New System.Windows.Forms.RadioButton
    Me.lblPattern = New System.Windows.Forms.Label
    Me.lblMaximum = New System.Windows.Forms.Label
    Me.lblMinimum = New System.Windows.Forms.Label
    Me.Label4 = New System.Windows.Forms.Label
    Me.Label5 = New System.Windows.Forms.Label
    Me.tabRowPerRun = New System.Windows.Forms.TabPage
    Me.agdRowPerRun = New atcControls.atcGrid
    Me.tabPivot = New System.Windows.Forms.TabPage
    Me.Label2 = New System.Windows.Forms.Label
    Me.cboCells = New System.Windows.Forms.ComboBox
    Me.Label1 = New System.Windows.Forms.Label
    Me.cboColumns = New System.Windows.Forms.ComboBox
    Me.lblRows = New System.Windows.Forms.Label
    Me.cboRows = New System.Windows.Forms.ComboBox
    Me.agdPivot = New atcControls.atcGrid
    Me.Tabs.SuspendLayout()
    Me.tabSpecify.SuspendLayout()
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
    Me.Tabs.Location = New System.Drawing.Point(8, 8)
    Me.Tabs.Name = "Tabs"
    Me.Tabs.SelectedIndex = 0
    Me.Tabs.Size = New System.Drawing.Size(552, 472)
    Me.Tabs.TabIndex = 0
    '
    'tabSpecify
    '
    Me.tabSpecify.Controls.Add(Me.btnStart)
    Me.tabSpecify.Controls.Add(Me.txtResultAttribute)
    Me.tabSpecify.Controls.Add(Me.lblResultAttribute)
    Me.tabSpecify.Controls.Add(Me.txtResultData)
    Me.tabSpecify.Controls.Add(Me.lblResultData)
    Me.tabSpecify.Controls.Add(Me.txtFunction)
    Me.tabSpecify.Controls.Add(Me.txtVaryData)
    Me.tabSpecify.Controls.Add(Me.grpParameter)
    Me.tabSpecify.Controls.Add(Me.Label4)
    Me.tabSpecify.Controls.Add(Me.Label5)
    Me.tabSpecify.Location = New System.Drawing.Point(4, 22)
    Me.tabSpecify.Name = "tabSpecify"
    Me.tabSpecify.Size = New System.Drawing.Size(544, 446)
    Me.tabSpecify.TabIndex = 2
    Me.tabSpecify.Text = "Specify Runs"
    '
    'btnStart
    '
    Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.btnStart.Location = New System.Drawing.Point(128, 280)
    Me.btnStart.Name = "btnStart"
    Me.btnStart.Size = New System.Drawing.Size(88, 24)
    Me.btnStart.TabIndex = 26
    Me.btnStart.Text = "Start"
    '
    'txtResultAttribute
    '
    Me.txtResultAttribute.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtResultAttribute.Location = New System.Drawing.Point(128, 240)
    Me.txtResultAttribute.Name = "txtResultAttribute"
    Me.txtResultAttribute.Size = New System.Drawing.Size(200, 20)
    Me.txtResultAttribute.TabIndex = 25
    Me.txtResultAttribute.Text = ""
    '
    'lblResultAttribute
    '
    Me.lblResultAttribute.BackColor = System.Drawing.Color.Transparent
    Me.lblResultAttribute.Location = New System.Drawing.Point(16, 240)
    Me.lblResultAttribute.Name = "lblResultAttribute"
    Me.lblResultAttribute.Size = New System.Drawing.Size(96, 18)
    Me.lblResultAttribute.TabIndex = 24
    Me.lblResultAttribute.Text = "Result Attribute:"
    '
    'txtResultData
    '
    Me.txtResultData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtResultData.Location = New System.Drawing.Point(128, 208)
    Me.txtResultData.Name = "txtResultData"
    Me.txtResultData.Size = New System.Drawing.Size(200, 20)
    Me.txtResultData.TabIndex = 23
    Me.txtResultData.Text = ""
    '
    'lblResultData
    '
    Me.lblResultData.BackColor = System.Drawing.Color.Transparent
    Me.lblResultData.Location = New System.Drawing.Point(16, 216)
    Me.lblResultData.Name = "lblResultData"
    Me.lblResultData.Size = New System.Drawing.Size(96, 18)
    Me.lblResultData.TabIndex = 22
    Me.lblResultData.Text = "Result Data:"
    '
    'txtFunction
    '
    Me.txtFunction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFunction.Location = New System.Drawing.Point(128, 40)
    Me.txtFunction.Name = "txtFunction"
    Me.txtFunction.Size = New System.Drawing.Size(200, 20)
    Me.txtFunction.TabIndex = 21
    Me.txtFunction.Text = "Multiply"
    '
    'txtVaryData
    '
    Me.txtVaryData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtVaryData.Location = New System.Drawing.Point(128, 8)
    Me.txtVaryData.Name = "txtVaryData"
    Me.txtVaryData.Size = New System.Drawing.Size(200, 20)
    Me.txtVaryData.TabIndex = 20
    Me.txtVaryData.Text = ""
    '
    'grpParameter
    '
    Me.grpParameter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.grpParameter.Controls.Add(Me.txtIncrement)
    Me.grpParameter.Controls.Add(Me.lblIncrement)
    Me.grpParameter.Controls.Add(Me.txtMax)
    Me.grpParameter.Controls.Add(Me.txtMin)
    Me.grpParameter.Controls.Add(Me.rdoRandom)
    Me.grpParameter.Controls.Add(Me.rdoLog)
    Me.grpParameter.Controls.Add(Me.rdoLinear)
    Me.grpParameter.Controls.Add(Me.lblPattern)
    Me.grpParameter.Controls.Add(Me.lblMaximum)
    Me.grpParameter.Controls.Add(Me.lblMinimum)
    Me.grpParameter.Location = New System.Drawing.Point(16, 64)
    Me.grpParameter.Name = "grpParameter"
    Me.grpParameter.Size = New System.Drawing.Size(312, 128)
    Me.grpParameter.TabIndex = 19
    Me.grpParameter.TabStop = False
    Me.grpParameter.Text = "Variation Parameter"
    '
    'txtIncrement
    '
    Me.txtIncrement.Location = New System.Drawing.Point(80, 72)
    Me.txtIncrement.Name = "txtIncrement"
    Me.txtIncrement.Size = New System.Drawing.Size(72, 20)
    Me.txtIncrement.TabIndex = 17
    Me.txtIncrement.Text = "0.05"
    '
    'lblIncrement
    '
    Me.lblIncrement.BackColor = System.Drawing.Color.Transparent
    Me.lblIncrement.Location = New System.Drawing.Point(16, 76)
    Me.lblIncrement.Name = "lblIncrement"
    Me.lblIncrement.Size = New System.Drawing.Size(64, 18)
    Me.lblIncrement.TabIndex = 16
    Me.lblIncrement.Text = "Increment:"
    '
    'txtMax
    '
    Me.txtMax.Location = New System.Drawing.Point(80, 48)
    Me.txtMax.Name = "txtMax"
    Me.txtMax.Size = New System.Drawing.Size(72, 20)
    Me.txtMax.TabIndex = 15
    Me.txtMax.Text = "1.1"
    '
    'txtMin
    '
    Me.txtMin.Location = New System.Drawing.Point(80, 24)
    Me.txtMin.Name = "txtMin"
    Me.txtMin.Size = New System.Drawing.Size(72, 20)
    Me.txtMin.TabIndex = 14
    Me.txtMin.Text = "0.9"
    '
    'rdoRandom
    '
    Me.rdoRandom.BackColor = System.Drawing.Color.Transparent
    Me.rdoRandom.Enabled = False
    Me.rdoRandom.Location = New System.Drawing.Point(232, 96)
    Me.rdoRandom.Name = "rdoRandom"
    Me.rdoRandom.Size = New System.Drawing.Size(72, 16)
    Me.rdoRandom.TabIndex = 13
    Me.rdoRandom.Text = "Random"
    '
    'rdoLog
    '
    Me.rdoLog.BackColor = System.Drawing.Color.Transparent
    Me.rdoLog.Enabled = False
    Me.rdoLog.Location = New System.Drawing.Point(160, 96)
    Me.rdoLog.Name = "rdoLog"
    Me.rdoLog.Size = New System.Drawing.Size(56, 16)
    Me.rdoLog.TabIndex = 12
    Me.rdoLog.Text = "Log"
    '
    'rdoLinear
    '
    Me.rdoLinear.BackColor = System.Drawing.Color.Transparent
    Me.rdoLinear.Checked = True
    Me.rdoLinear.Enabled = False
    Me.rdoLinear.Location = New System.Drawing.Point(88, 96)
    Me.rdoLinear.Name = "rdoLinear"
    Me.rdoLinear.Size = New System.Drawing.Size(56, 16)
    Me.rdoLinear.TabIndex = 11
    Me.rdoLinear.TabStop = True
    Me.rdoLinear.Text = "Linear"
    '
    'lblPattern
    '
    Me.lblPattern.BackColor = System.Drawing.Color.Transparent
    Me.lblPattern.Enabled = False
    Me.lblPattern.Location = New System.Drawing.Point(16, 100)
    Me.lblPattern.Name = "lblPattern"
    Me.lblPattern.Size = New System.Drawing.Size(64, 18)
    Me.lblPattern.TabIndex = 10
    Me.lblPattern.Text = "Pattern:"
    '
    'lblMaximum
    '
    Me.lblMaximum.BackColor = System.Drawing.Color.Transparent
    Me.lblMaximum.Location = New System.Drawing.Point(16, 52)
    Me.lblMaximum.Name = "lblMaximum"
    Me.lblMaximum.Size = New System.Drawing.Size(64, 18)
    Me.lblMaximum.TabIndex = 9
    Me.lblMaximum.Text = "Maximum:"
    '
    'lblMinimum
    '
    Me.lblMinimum.BackColor = System.Drawing.Color.Transparent
    Me.lblMinimum.Location = New System.Drawing.Point(16, 28)
    Me.lblMinimum.Name = "lblMinimum"
    Me.lblMinimum.Size = New System.Drawing.Size(64, 18)
    Me.lblMinimum.TabIndex = 8
    Me.lblMinimum.Text = "Minimum:"
    '
    'Label4
    '
    Me.Label4.BackColor = System.Drawing.Color.Transparent
    Me.Label4.Location = New System.Drawing.Point(16, 40)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(128, 18)
    Me.Label4.TabIndex = 18
    Me.Label4.Text = "Variation Function:"
    '
    'Label5
    '
    Me.Label5.BackColor = System.Drawing.Color.Transparent
    Me.Label5.Location = New System.Drawing.Point(16, 16)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(128, 18)
    Me.Label5.TabIndex = 17
    Me.Label5.Text = "Data to Vary:"
    '
    'tabRowPerRun
    '
    Me.tabRowPerRun.Controls.Add(Me.agdRowPerRun)
    Me.tabRowPerRun.Location = New System.Drawing.Point(4, 22)
    Me.tabRowPerRun.Name = "tabRowPerRun"
    Me.tabRowPerRun.Size = New System.Drawing.Size(544, 446)
    Me.tabRowPerRun.TabIndex = 0
    Me.tabRowPerRun.Text = "Row Per Run"
    '
    'agdRowPerRun
    '
    Me.agdRowPerRun.AllowHorizontalScrolling = True
    Me.agdRowPerRun.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdRowPerRun.CellBackColor = System.Drawing.Color.Empty
    Me.agdRowPerRun.LineColor = System.Drawing.Color.Empty
    Me.agdRowPerRun.LineWidth = 0.0!
    Me.agdRowPerRun.Location = New System.Drawing.Point(8, 8)
    Me.agdRowPerRun.Name = "agdRowPerRun"
    Me.agdRowPerRun.Size = New System.Drawing.Size(528, 432)
    Me.agdRowPerRun.Source = Nothing
    Me.agdRowPerRun.TabIndex = 0
    '
    'tabPivot
    '
    Me.tabPivot.Controls.Add(Me.Label2)
    Me.tabPivot.Controls.Add(Me.cboCells)
    Me.tabPivot.Controls.Add(Me.Label1)
    Me.tabPivot.Controls.Add(Me.cboColumns)
    Me.tabPivot.Controls.Add(Me.lblRows)
    Me.tabPivot.Controls.Add(Me.cboRows)
    Me.tabPivot.Controls.Add(Me.agdPivot)
    Me.tabPivot.Location = New System.Drawing.Point(4, 22)
    Me.tabPivot.Name = "tabPivot"
    Me.tabPivot.Size = New System.Drawing.Size(544, 446)
    Me.tabPivot.TabIndex = 1
    Me.tabPivot.Text = "Pivot"
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(8, 68)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(56, 16)
    Me.Label2.TabIndex = 6
    Me.Label2.Text = "Cells"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboCells
    '
    Me.cboCells.Location = New System.Drawing.Point(72, 64)
    Me.cboCells.Name = "cboCells"
    Me.cboCells.Size = New System.Drawing.Size(112, 21)
    Me.cboCells.TabIndex = 5
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(8, 44)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(56, 16)
    Me.Label1.TabIndex = 4
    Me.Label1.Text = "Columns"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboColumns
    '
    Me.cboColumns.Location = New System.Drawing.Point(72, 40)
    Me.cboColumns.Name = "cboColumns"
    Me.cboColumns.Size = New System.Drawing.Size(112, 21)
    Me.cboColumns.TabIndex = 3
    '
    'lblRows
    '
    Me.lblRows.Location = New System.Drawing.Point(8, 20)
    Me.lblRows.Name = "lblRows"
    Me.lblRows.Size = New System.Drawing.Size(56, 16)
    Me.lblRows.TabIndex = 2
    Me.lblRows.Text = "Rows"
    Me.lblRows.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'cboRows
    '
    Me.cboRows.Location = New System.Drawing.Point(72, 16)
    Me.cboRows.Name = "cboRows"
    Me.cboRows.Size = New System.Drawing.Size(112, 21)
    Me.cboRows.TabIndex = 1
    '
    'agdPivot
    '
    Me.agdPivot.AllowHorizontalScrolling = True
    Me.agdPivot.CellBackColor = System.Drawing.Color.Empty
    Me.agdPivot.LineColor = System.Drawing.Color.Empty
    Me.agdPivot.LineWidth = 0.0!
    Me.agdPivot.Location = New System.Drawing.Point(8, 112)
    Me.agdPivot.Name = "agdPivot"
    Me.agdPivot.Size = New System.Drawing.Size(528, 328)
    Me.agdPivot.Source = Nothing
    Me.agdPivot.TabIndex = 0
    '
    'frmMultipleResults
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(568, 485)
    Me.Controls.Add(Me.Tabs)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmMultipleResults"
    Me.Text = "Multiple Runs"
    Me.Tabs.ResumeLayout(False)
    Me.tabSpecify.ResumeLayout(False)
    Me.grpParameter.ResumeLayout(False)
    Me.tabRowPerRun.ResumeLayout(False)
    Me.tabPivot.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub Initialize(ByVal aDataManager As atcDataManager, _
           Optional ByVal aTimeseriesGroup As atcDataGroup = Nothing)
    pDataManager = aDataManager
    pDataToVary = aTimeseriesGroup
    If pDataToVary Is Nothing Then pDataToVary = New atcDataGroup
    UpdateDataText(txtVaryData, pDataToVary)
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
    txtFunction.Text = pComputation.ToString
  End Sub

  Private Sub txtResultData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtResultData.Click
    pResultsToTrack = pDataManager.UserSelectData("Select results of interest", pResultsToTrack)
    UpdateDataText(txtResultData, pResultsToTrack)
  End Sub

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
    agdRowPerRun.Source = New atcGridSource
    With agdRowPerRun.Source
      .FixedColumns = 1
      .Columns = 6
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
    agdRowPerRun.Initialize(agdRowPerRun.Source)
    agdRowPerRun.Refresh()
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

  Private Sub Run(ByVal aOrigPrecip As atcDataSet, _
                  ByVal aMin As Double, _
                  ByVal aMax As Double, _
                  ByVal aIncrement As Double, _
                  ByVal aOrigAirTmp As atcDataSet, _
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

    For lAirTmpAdd As Double = aMin2 To aMax2 Step aIncrement2
      'build new temp and evap
      lTsMath.DataSets.Clear()
      lArgsMath.Clear()
      lArgsMath.SetValue("timeseries", aOrigAirTmp)
      lArgsMath.SetValue("Number", lAirTmpAdd) 'Adjust to change all values
      pDataManager.OpenDataSource(lTsMath, "add", lArgsMath)
      Dim lAirTmpMean As String = Format(lTsMath.DataSets(0).Attributes.GetValue("Mean"), "#.00")
      lArgsMath.Clear()
      lNewEvap = atcMetCmp.CmpHamX(lTsMath.DataSets(0), Nothing, True, 39, lCTS)
      lNewEvap.Attributes.SetValue("Constituent", "PET")
      lNewEvap.Attributes.SetValue("Id", 111)
      lNewEvap.Attributes.SetValue("Scenario", "modified")
      Dim lEvapMean As String = Format(lNewEvap.Attributes.GetValue("Mean") * 365.25, "#.00")
      'lSummary.Save(pDataManager, New atcDataGroup(lNewEvap), "DataTree_Evap" & lAirTmpAdd & ".txt")

      For lPrecMult As Double = aMin To aMax Step aIncrement
        Logger.Dbg(" Step:" & lAirTmpAdd & ":" & lPrecMult)
        lRow += 1
        With agdRowPerRun.Source
          .CellValue(lRow, 0) = lAirTmpAdd
          .CellValue(lRow, 1) = lAirTmpMean
          .CellValue(lRow, 2) = lEvapMean
          .CellValue(lRow, 3) = lPrecMult
        End With
        agdRowPerRun.Refresh()

        lNewPrec = Nothing
        lNewPrec = aOrigPrecip.Clone
        For iValue As Integer = 1 To lNewPrec.numValues
          Dim lDate As Date = Date.FromOADate(lNewPrec.Dates.Value(iValue))
          Select Case lDate.Month
            Case 8, 9 : lNewPrec.Value(iValue) *= lPrecMult
          End Select
        Next
        lNewPrec.Attributes.CalculateAll()
        'TODO: REPLACE HARD CODED 10 YEARS WITH ACTUAL SPAN CALC!
        agdRowPerRun.Source.CellValue(lRow, 4) = Format(lNewPrec.Attributes.GetValue("Sum") / 10, "#.00")
        'lSummary.Save(pDataManager, New atcDataGroup(lNewPrec), "DataTree_Precip" & lPrecMult & ".txt")

        Dim lScenarioResults = New atcDataSource
        Dim lModifiedData As New atcDataGroup
        lModifiedData.Add(lNewPrec)
        lModifiedData.Add(lNewEvap)
        lScenarioResults = ScenarioRun(aWDMFileName, "modified", lModifiedData)

        lOutputFlow = lScenarioResults.DataSets.FindData("ID", "1004", 0)
        'lSummary.Save(pDataManager, lOutputFlow, "DataTree_FlowB&M" & lTempMult & ":" & lPrecMult & ".txt")
        For lIndex = 0 To aOutputAttributes.GetUpperBound(0)
          agdRowPerRun.Source.CellValue(lRow, 5 + lIndex) = Format(lOutputFlow.Item(0).Attributes.GetValue(aOutputAttributes(lIndex)), "#.0")
        Next
        agdRowPerRun.Refresh()
        Windows.Forms.Application.DoEvents()
      Next
    Next
  End Sub

  Private Function Pattern() As Integer
    If rdoLinear.Checked Then Return 0
    If rdoLog.Checked Then Return 1
    If rdoRandom.Checked Then Return 2
    Logger.Msg("No pattern was selected for varying parameter")
  End Function

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

    If Not agdRowPerRun.Source Is Nothing Then
      For iColumn As Integer = 0 To agdRowPerRun.Source.Columns - 1
        Dim lColumnTitle As String = agdRowPerRun.Source.CellValue(0, iColumn)
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
      Dim lRuns As Integer = agdRowPerRun.Source.Rows - 1
      Dim lRunRow As Integer = 0
      Dim lRunColumn As Integer = 0
      Dim lRunCell As Integer = 0

      While Not agdRowPerRun.Source.CellValue(0, lRunRow).Equals(cboRows.Text)
        lRunRow += 1
      End While
      While Not agdRowPerRun.Source.CellValue(0, lRunColumn).Equals(cboColumns.Text)
        lRunColumn += 1
      End While
      While Not agdRowPerRun.Source.CellValue(0, lRunCell).Equals(cboCells.Text)
        lRunCell += 1
      End While

      With lPivotData
        Dim lRowValues As ArrayList = UniqueValuesInColumn(agdRowPerRun.Source, lRunRow)
        Dim lColumnValues As ArrayList = UniqueValuesInColumn(agdRowPerRun.Source, lRunColumn)
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
            lMatchRow = FindMatchingRow(agdRowPerRun.Source, lRunRow, lRowValue, lRunColumn, lColumnValue)
            If lMatchRow > 0 Then
              .CellValue(lRow, lColumn) = agdRowPerRun.Source.CellValue(lMatchRow, lRunCell)
            End If
          Next
        Next
        ' agdRowPerRun.Source.CellValue(lRow, lRunRow)
      End With

      agdPivot.Initialize(lPivotData)
      agdPivot.Visible = True
      agdPivot.Refresh()
    Else
      agdPivot.Visible = False
    End If
  End Sub

End Class
