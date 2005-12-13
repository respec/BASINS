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
  Friend WithEvents mnuSelectTimeseries As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributeRows As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributeColumns As System.Windows.Forms.MenuItem
  Friend WithEvents mnuView As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcListForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuSelectTimeseries = New System.Windows.Forms.MenuItem
    Me.mnuSelectAttributes = New System.Windows.Forms.MenuItem
    Me.mnuView = New System.Windows.Forms.MenuItem
    Me.mnuAttributeRows = New System.Windows.Forms.MenuItem
    Me.mnuAttributeColumns = New System.Windows.Forms.MenuItem
    Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.MenuItem1 = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuView, Me.mnuAnalysis})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSelectTimeseries, Me.mnuSelectAttributes})
    Me.mnuFile.Text = "File"
    '
    'mnuSelectTimeseries
    '
    Me.mnuSelectTimeseries.Index = 0
    Me.mnuSelectTimeseries.Text = "Select Timeseries"
    '
    'mnuSelectAttributes
    '
    Me.mnuSelectAttributes.Index = 1
    Me.mnuSelectAttributes.Text = "Select Attributes"
    '
    'mnuView
    '
    Me.mnuView.Index = 1
    Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributeRows, Me.mnuAttributeColumns, Me.MenuItem1, Me.mnuSizeColumnsToContents})
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
    'mnuSizeColumnsToContents
    '
    Me.mnuSizeColumnsToContents.Index = 3
    Me.mnuSizeColumnsToContents.Text = "Size Columns To Contents"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 2
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
    Me.agdMain.Size = New System.Drawing.Size(528, 545)
    Me.agdMain.Source = Nothing
    Me.agdMain.TabIndex = 0
    '
    'MenuItem1
    '
    Me.MenuItem1.Index = 2
    Me.MenuItem1.Text = "-"
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

  Private Sub mnuSelectTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectTimeseries.Click
    pDataManager.UserSelectData(, pDataGroup, False)
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

  Private Sub mnuSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAttributes.Click
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

  Private Sub mnuAttributeRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeRows.Click
    SwapRowsColumns = False
  End Sub

  Private Sub mnuAttributeColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributeColumns.Click
    SwapRowsColumns = True
  End Sub

  'True for attributes in columns, False for attributes in rows
  Public Property SwapRowsColumns() As Boolean
    Get
      Return pSwapperSource.SwapRowsColumns
    End Get
    Set(ByVal newValue As Boolean)
      If pSwapperSource.SwapRowsColumns <> newValue Then
        pSwapperSource.SwapRowsColumns = newValue
        agdMain.Refresh()
      End If
      mnuAttributeColumns.Checked = newValue
      mnuAttributeRows.Checked = Not newValue
    End Set
  End Property

  Private Sub mnuSizeColumnsToContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSizeColumnsToContents.Click
    agdMain.SizeAllColumnsToContents()
    agdMain.Refresh()
  End Sub
End Class