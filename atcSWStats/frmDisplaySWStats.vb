Imports atcData
Imports atcUtility

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
      pDataManager.UserSelectData(, pDataGroup, True)
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
  Friend WithEvents agdMain As atcControls.atcGrid
  Friend WithEvents mnuView As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewColumns As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewRows As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAddAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewHigh As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewLow As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDisplaySWStats))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuAddAttributes = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditCopy = New System.Windows.Forms.MenuItem
    Me.mnuView = New System.Windows.Forms.MenuItem
    Me.mnuViewColumns = New System.Windows.Forms.MenuItem
    Me.mnuViewRows = New System.Windows.Forms.MenuItem
    Me.MenuItem1 = New System.Windows.Forms.MenuItem
    Me.mnuViewHigh = New System.Windows.Forms.MenuItem
    Me.mnuViewLow = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis})
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
    'mnuView
    '
    Me.mnuView.Index = 2
    Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewColumns, Me.mnuViewRows, Me.MenuItem1, Me.mnuViewHigh, Me.mnuViewLow})
    Me.mnuView.Text = "&View"
    '
    'mnuViewColumns
    '
    Me.mnuViewColumns.Checked = True
    Me.mnuViewColumns.Index = 0
    Me.mnuViewColumns.Text = "Columns"
    '
    'mnuViewRows
    '
    Me.mnuViewRows.Index = 1
    Me.mnuViewRows.Text = "Rows"
    '
    'MenuItem1
    '
    Me.MenuItem1.Index = 2
    Me.MenuItem1.Text = "-"
    '
    'mnuViewHigh
    '
    Me.mnuViewHigh.Checked = True
    Me.mnuViewHigh.Index = 3
    Me.mnuViewHigh.Text = "High"
    '
    'mnuViewLow
    '
    Me.mnuViewLow.Index = 4
    Me.mnuViewLow.Text = "Low"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 3
    Me.mnuAnalysis.Text = "Analysis"
    '
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(720, 545)
    Me.agdMain.Source = Nothing
    Me.agdMain.TabIndex = 0
    '
    'frmDisplaySWStats
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(720, 545)
    Me.Controls.Add(Me.agdMain)
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

  'Translator class between pDataGroup and agdMain
  Private pSource As atcSWStatsSource

  Private Sub PopulateGrid()
    pSource = New atcSWStatsSource(pDataGroup)
    If pSource.Columns < 3 Then
      UserSpecifyAttributes()
      pSource = New atcSWStatsSource(pDataGroup)
    End If

    pSource.SwapRowsColumns = mnuViewRows.Checked
    pSource.High = mnuViewHigh.Checked
    agdMain.Initialize(pSource)
    agdMain.SizeAllColumnsToContents()

    Dim lRequestedHeight As Single = Me.Height - agdMain.Top - agdMain.Height + pSource.Rows * agdMain.RowHeight(0)
    Dim lRequestedWidth As Single = Me.Width - agdMain.Left - agdMain.Width
    For lColumn As Integer = 0 To pSource.Columns - 1
      lRequestedWidth += agdMain.ColumnWidth(lColumn)
    Next
    Me.Height = lRequestedHeight
    Me.Width = lRequestedWidth
    agdMain.Refresh()
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

  Private Sub mnuViewColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewColumns.Click
    mnuViewColumns.Checked = True
    mnuViewRows.Checked = False
    pSource.SwapRowsColumns = False
    agdMain.Refresh()
  End Sub

  Private Sub mnuViewRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewRows.Click
    mnuViewColumns.Checked = False
    mnuViewRows.Checked = True
    pSource.SwapRowsColumns = True
    agdMain.Refresh()
  End Sub

  Private Sub mnuViewHigh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewHigh.Click
    Me.HighDisplay = True
  End Sub

  Private Sub mnuViewLow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuViewLow.Click
    Me.HighDisplay = False
  End Sub

  Private Sub mnuAddAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddAttributes.Click
    UserSpecifyAttributes()
    PopulateGrid()
  End Sub

  Private Sub UserSpecifyAttributes()
    Dim lForm As New frmSpecifyFrequency
    Dim lChoseHigh As Boolean
    If lForm.AskUser(pDataGroup, lChoseHigh) Then
      Me.HighDisplay = lChoseHigh
    End If
  End Sub

  Public Overrides Function ToString() As String
    Return Me.Text & vbCrLf & agdMain.ToString
  End Function

  'True for rows and columns to be swapped, false for normal orientation
  Public Property SwapRowsColumns() As Boolean
    Get
      Return pSource.SwapRowsColumns
    End Get
    Set(ByVal newValue As Boolean)
      pSource.SwapRowsColumns = newValue
    End Set
  End Property

  Public Property HighDisplay() As Boolean
    Get
      Return pSource.High
    End Get
    Set(ByVal newValue As Boolean)
      pSource.High = newValue
      If newValue Then
        Me.Text = "High Values"
        mnuViewHigh.Checked = True
        mnuViewLow.Checked = False
      Else
        Me.Text = "Low Values"
        mnuViewHigh.Checked = False
        mnuViewLow.Checked = True
      End If
      agdMain.Refresh()
    End Set
  End Property

  Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
    Clipboard.SetDataObject(Me.ToString)
  End Sub

  Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
    Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
    With lSaveDialog
      .Title = "Save Grid As"
      .DefaultExt = ".txt"
      .FileName = ReplaceString(Me.Text, " ", "_") & ".txt"
      If .ShowDialog(Me) = DialogResult.OK Then
        SaveFileString(.FileName, Me.ToString)
      End If
    End With
  End Sub

  Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    pDataManager = Nothing
    pDataGroup = Nothing
  End Sub
End Class
