Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Friend Class atcListForm
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()
    InitializeComponent() 'required by Windows Form Designer
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
  Friend WithEvents agdMain As atcControls.atcGrid
  Friend WithEvents mnuAttributeRows As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributeColumns As System.Windows.Forms.MenuItem
  Friend WithEvents mnuView As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSelectAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
  Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcListForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
    Me.mnuFileSelectAttributes = New System.Windows.Forms.MenuItem
    Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditCopy = New System.Windows.Forms.MenuItem
    Me.mnuView = New System.Windows.Forms.MenuItem
    Me.mnuAttributeRows = New System.Windows.Forms.MenuItem
    Me.mnuAttributeColumns = New System.Windows.Forms.MenuItem
    Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
    Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.mnuHelp = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSelectAttributes, Me.mnuFileSep1, Me.mnuFileSave})
    Me.mnuFile.Text = "File"
    '
    'mnuFileSelectData
    '
    Me.mnuFileSelectData.Index = 0
    Me.mnuFileSelectData.Text = "Select &Data"
    '
    'mnuFileSelectAttributes
    '
    Me.mnuFileSelectAttributes.Index = 1
    Me.mnuFileSelectAttributes.Text = "Select &Attributes"
    '
    'mnuFileSep1
    '
    Me.mnuFileSep1.Index = 2
    Me.mnuFileSep1.Text = "-"
    '
    'mnuFileSave
    '
    Me.mnuFileSave.Index = 3
    Me.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
    Me.mnuFileSave.Text = "Save"
    '
    'mnuEdit
    '
    Me.mnuEdit.Index = 1
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopy})
    Me.mnuEdit.Text = "Edit"
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
    Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributeRows, Me.mnuAttributeColumns, Me.mnuViewSep1, Me.mnuSizeColumnsToContents})
    Me.mnuView.Text = "View"
    '
    'mnuAttributeRows
    '
    Me.mnuAttributeRows.Checked = True
    Me.mnuAttributeRows.Index = 0
    Me.mnuAttributeRows.Text = "Attribute Rows"
    '
    'mnuAttributeColumns
    '
    Me.mnuAttributeColumns.Index = 1
    Me.mnuAttributeColumns.Text = "Attribute Columns"
    '
    'mnuViewSep1
    '
    Me.mnuViewSep1.Index = 2
    Me.mnuViewSep1.Text = "-"
    '
    'mnuSizeColumnsToContents
    '
    Me.mnuSizeColumnsToContents.Index = 3
    Me.mnuSizeColumnsToContents.Text = "Size Columns To Contents"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 3
    Me.mnuAnalysis.Text = "Analysis"
    '
    'mnuHelp
    '
    Me.mnuHelp.Index = 4
    Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
    Me.mnuHelp.Text = "Help"
    '
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.CellBackColor = System.Drawing.Color.Empty
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(528, 545)
    Me.agdMain.Source = Nothing
    Me.agdMain.TabIndex = 0
    '
    'atcListForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 545)
    Me.Controls.Add(Me.agdMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcListForm"
    Me.Text = "Timeseries List"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager

  'The group of atcTimeseries displayed
  Private WithEvents pDataGroup As atcDataGroup

  'Translator class between pDataGroup and agdMain
  Private pSource As atcListGridSource
  Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

  Public Sub Initialize(ByVal aDataManager As atcData.atcDataManager, _
               Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    pDataManager = aDataManager
    If aTimeseriesGroup Is Nothing Then
      pDataGroup = New atcDataGroup
    Else
      pDataGroup = aTimeseriesGroup
    End If

    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      Dim lMenuText As String = lDisp.Name
      If lMenuText.StartsWith("Tools::") Then lMenuText = lMenuText.Substring(7)
      mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
    Next

    If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
      pDataManager.UserSelectData(, pDataGroup, True)
    End If

    If pDataGroup.Count > 0 Then
      Me.Show()
      PopulateGrid()
    Else 'user declined to specify timeseries
      Me.Close()
    End If

  End Sub

  Private Sub PopulateGrid()
    Dim lTotalWidth As Integer = 10
    pSource = New atcListGridSource(pDataManager, pDataGroup)
    pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(pSource)
    pSwapperSource.SwapRowsColumns = mnuAttributeColumns.Checked
    agdMain.Initialize(pSwapperSource)
    agdMain.SizeAllColumnsToContents()
    For iColumn As Integer = 0 To pSource.Columns
      lTotalWidth += agdMain.ColumnWidth(iColumn)
    Next
    Try
      If lTotalWidth < Screen.PrimaryScreen.Bounds.Width Then
        Me.Width = lTotalWidth
      End If
    Catch 'Ignore error if we can't tell how large to make it, or can't rezise
    End Try
    agdMain.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
    pDataManager.ShowDisplay(sender.Text, pDataGroup)
  End Sub

  Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
    If Me.Visible Then PopulateGrid()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
    If Me.Visible Then PopulateGrid()
    'TODO: could efficiently remove by serial number
  End Sub

  Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    pDataManager = Nothing
    pDataGroup = Nothing
    pSource = Nothing
  End Sub

  Private Sub mnuAttributeRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeRows.Click
    SwapRowsColumns = False
  End Sub

  Private Sub mnuAttributeColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeColumns.Click
    SwapRowsColumns = True
  End Sub

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

  Private Sub mnuFileSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectAttributes.Click
    Dim lst As New atcControls.atcSelectList
    Dim lAvailable As New ArrayList
    For Each lAttrDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions
      Select Case lAttrDef.TypeString.ToLower
        Case "double", "integer", "boolean", "string"
          lAvailable.Add(lAttrDef.Name)
      End Select
    Next
    lAvailable.Sort()
    If lst.AskUser(lAvailable, pDataManager.DisplayAttributes) Then
      PopulateGrid()
    End If
  End Sub

  Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
    pDataManager.UserSelectData(, pDataGroup, False)
  End Sub

  Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
    agdMain.SizeAllColumnsToContents()
    agdMain.Refresh()
  End Sub

  'True for attributes in columns, False for attributes in rows
  Public Property SwapRowsColumns() As Boolean
    Get
      Return pSwapperSource.SwapRowsColumns
    End Get
    Set(ByVal newValue As Boolean)
      If pSwapperSource.SwapRowsColumns <> newValue Then
        pSwapperSource.SwapRowsColumns = newValue
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
      End If
      mnuAttributeColumns.Checked = newValue
      mnuAttributeRows.Checked = Not newValue
    End Set
  End Property

  Public Overrides Function ToString() As String
    Return Me.Text & vbCrLf & agdMain.ToString
  End Function

  Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
    ShowHelp("BASINS Details\Analysis\Time Series Functions\List.html")
  End Sub
End Class