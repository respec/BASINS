Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Friend Class frmDisplaySWStats
  Inherits System.Windows.Forms.Form
  Private pInitializing As Boolean

#Region " Windows Form Designer generated code "

  Public Sub New(ByVal aDataManager As atcData.atcDataManager, _
        Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
    MyBase.New()
    pInitializing = True
    pDataManager = aDataManager
    If aDataGroup Is Nothing Then
      pDataGroup = New atcDataGroup
    Else
      pDataGroup = aDataGroup
    End If
    InitializeComponent() 'required by Windows Form Designer

    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each ldisp As atcDataDisplay In DisplayPlugins
      mnuAnalysis.MenuItems.Add(ldisp.Name, New EventHandler(AddressOf mnuAnalysis_Click))
    Next

    If pDataGroup.Count = 0 Then 'ask user to specify some Data
      pDataManager.UserSelectData("Select Timeseries for SWSTAT Reports", pDataGroup, True)
    End If

    pInitializing = False
    If pDataGroup.Count > 0 Then
      PopulateGrid()
    Else 'user declined to specify Data
      Me.Close()
    End If
    'agdMain.AllowHorizontalScrolling = False
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
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAddAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  Friend WithEvents mnuReport As System.Windows.Forms.MenuItem
  Friend WithEvents mnuRptFreq As System.Windows.Forms.MenuItem
  Friend WithEvents mnuRptNDay As System.Windows.Forms.MenuItem
  Friend WithEvents mnuRptTrend As System.Windows.Forms.MenuItem
  Friend WithEvents txtReport As System.Windows.Forms.TextBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDisplaySWStats))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuAddAttributes = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditCopy = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.mnuReport = New System.Windows.Forms.MenuItem
    Me.mnuRptFreq = New System.Windows.Forms.MenuItem
    Me.mnuRptNDay = New System.Windows.Forms.MenuItem
    Me.mnuRptTrend = New System.Windows.Forms.MenuItem
    Me.txtReport = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuAnalysis, Me.mnuReport})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd, Me.mnuAddAttributes, Me.mnuFileSave})
    Me.mnuFile.Text = "File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "Add Timeseries"
    '
    'mnuAddAttributes
    '
    Me.mnuAddAttributes.Index = 1
    Me.mnuAddAttributes.Text = "Add Attributes"
    '
    'mnuFileSave
    '
    Me.mnuFileSave.Index = 2
    Me.mnuFileSave.Text = "Save"
    '
    'mnuEdit
    '
    Me.mnuEdit.Index = 1
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopy})
    Me.mnuEdit.Text = "&Edit"
    '
    'mnuEditCopy
    '
    Me.mnuEditCopy.Index = 0
    Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
    Me.mnuEditCopy.Text = "Copy"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 3
    Me.mnuAnalysis.Text = "Analysis"
    '
    'mnuReport
    '
    Me.mnuReport.Index = 4
    Me.mnuReport.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRptFreq, Me.mnuRptNDay, Me.mnuRptTrend})
    Me.mnuReport.Text = "Report"
    '
    'mnuRptFreq
    '
    Me.mnuRptFreq.Index = 0
    Me.mnuRptFreq.Text = "Frequency"
    '
    'mnuRptNDay
    '
    Me.mnuRptNDay.Index = 1
    Me.mnuRptNDay.Text = "N-Day"
    '
    'mnuRptTrend
    '
    Me.mnuRptTrend.Index = 2
    Me.mnuRptTrend.Text = "Trend"
    '
    'txtReport
    '
    Me.txtReport.Dock = System.Windows.Forms.DockStyle.Fill
    Me.txtReport.Location = New System.Drawing.Point(0, 0)
    Me.txtReport.Multiline = True
    Me.txtReport.Name = "txtReport"
    Me.txtReport.Size = New System.Drawing.Size(720, 545)
    Me.txtReport.TabIndex = 0
    Me.txtReport.Text = ""
    '
    'frmDisplaySWStats
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(7, 13)
    Me.ClientSize = New System.Drawing.Size(720, 545)
    Me.Controls.Add(Me.txtReport)
    Me.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmDisplaySWStats"
    Me.Text = "High Values"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager

  'The group of atcTimeseries displayed
  Private WithEvents pDataGroup As atcDataGroup

  Private Sub PopulateGrid()
  End Sub

  Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
    Dim newDisplay As atcDataDisplay
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each atf As atcDataDisplay In DisplayPlugins
      If atf.Name = sender.Text Then
        Dim typ As System.Type = atf.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        newDisplay = asm.CreateInstance(typ.FullName)
        newDisplay.Show(pDataManager, pDataGroup)
        Exit Sub
      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pDataGroup, False)
  End Sub

  Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
    If Not pInitializing Then PopulateGrid()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
    If Not pInitializing Then PopulateGrid()
    'TODO: could efficiently remove by serial number
  End Sub

  Private Sub mnuAddAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddAttributes.Click
    UserSpecifyAttributes()
    PopulateGrid()
  End Sub

  Private Sub UserSpecifyAttributes()
    Dim lForm As New frmSpecifyFrequency
    Dim lChoseHigh As Boolean
    If lForm.AskUser(pDataGroup, lChoseHigh) Then
    End If
  End Sub

  Public Overrides Function ToString() As String
    Return Me.Text & vbCrLf & txtReport.Text
  End Function

  Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
    Clipboard.SetDataObject(Me.ToString)
  End Sub

  Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
    Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
    With lSaveDialog
      .Title = "Save Grid As"
      .DefaultExt = ".txt"
      .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
      If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        SaveFileString(.FileName, Me.ToString)
      End If
    End With
  End Sub

  Private Sub mnuRptFreq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptFreq.Click
    Dim lForm As New frmSpecifyFrequency
    'lform.AskUser(pdatagroup,
    mnuRptFreq.Checked = True
    mnuRptNDay.Checked = False
    mnuRptTrend.Checked = False
    txtReport.Text = ""
    'loop through timeseries in group
    For Each lts As atcTimeseries In pDataGroup
      'txtReport.Text &= GenFreqReport(lts, )
    Next
  End Sub

  Private Sub mnuRptNDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptNDay.Click
    mnuRptNDay.Checked = True
    mnuRptFreq.Checked = False
    mnuRptTrend.Checked = False
    txtReport.Text = GenNDayReport()

  End Sub

  Private Sub mnuRptTrend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRptTrend.Click
    mnuRptTrend.Checked = True
    mnuRptFreq.Checked = False
    mnuRptNDay.Checked = False
    txtReport.Text = ""
    For Each lts As atcTimeseries In pDataGroup
      txtReport.Text &= GenTrendReport(lts)
    Next
  End Sub

End Class
