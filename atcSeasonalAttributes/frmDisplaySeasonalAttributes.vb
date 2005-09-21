Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Friend Class frmDisplaySeasonalAttributes
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
  Friend WithEvents mnuViewSeasonColumns As System.Windows.Forms.MenuItem
  Friend WithEvents mnuViewSeasonRows As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAddAttributes As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDisplaySeasonalAttributes))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuView = New System.Windows.Forms.MenuItem
    Me.mnuViewSeasonColumns = New System.Windows.Forms.MenuItem
    Me.mnuViewSeasonRows = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.mnuAddAttributes = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuView, Me.mnuAnalysis})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd, Me.mnuAddAttributes})
    Me.mnuFile.Text = "File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "Add Timeseries"
    '
    'mnuView
    '
    Me.mnuView.Index = 1
    Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuViewSeasonColumns, Me.mnuViewSeasonRows})
    Me.mnuView.Text = "&View"
    '
    'mnuViewSeasonColumns
    '
    Me.mnuViewSeasonColumns.Index = 0
    Me.mnuViewSeasonColumns.Text = "Season Columns"
    '
    'mnuViewSeasonRows
    '
    Me.mnuViewSeasonRows.Index = 1
    Me.mnuViewSeasonRows.Text = "Season Rows"
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
    Me.agdMain.TabIndex = 0
    '
    'mnuAddAttributes
    '
    Me.mnuAddAttributes.Index = 1
    Me.mnuAddAttributes.Text = "Add Attributes"
    '
    'frmDisplaySeasonalAttributes
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 545)
    Me.Controls.Add(Me.agdMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmDisplaySeasonalAttributes"
    Me.Text = "Seasonal Attributes"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager

  'The group of atcTimeseries displayed
  Private WithEvents pDataGroup As atcDataGroup

  'Translator class between pDataGroup and agdMain
  Private pSource As atcSeasonalAttributesGridSource

  Private Sub PopulateGrid()
    Dim lWasSwapped As Boolean = Not pSource Is Nothing AndAlso pSource.SwapRowsColumns
    pSource = New atcSeasonalAttributesGridSource(pDataManager, pDataGroup)
    If pSource.Columns < 3 Then
      UserSpecifyAttributes()
      pSource = New atcSeasonalAttributesGridSource(pDataManager, pDataGroup)
    End If
    If lWasSwapped Then pSource.SwapRowsColumns = True
    agdMain.Initialize(pSource)
    agdMain.SizeAllColumnsToContents()
    agdMain.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

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

  Private Sub mnuViewSeasonColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSeasonColumns.Click
    If pSource.SwapRowsColumns Then
      pSource.SwapRowsColumns = False
      agdMain.Refresh()
    End If
  End Sub

  Private Sub mnuViewSeasonRows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSeasonRows.Click
    If Not pSource.SwapRowsColumns Then
      pSource.SwapRowsColumns = True
      agdMain.Refresh()
    End If
  End Sub

  Private Sub mnuAddAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddAttributes.Click
    UserSpecifyAttributes()
  End Sub

  Private Sub UserSpecifyAttributes()
    For Each lPlugin As atcDataPlugin In pDataManager.GetPlugins(GetType(atcDataSource))
      If (lPlugin.Name = "Timeseries::Seasonal") Then
        Dim typ As System.Type = lPlugin.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        Dim newSource As atcDataSource = asm.CreateInstance(typ.FullName)
        Dim newArguments As New atcDataAttributes
        newArguments.SetValue("Timeseries", pDataGroup)
        newSource.Open("SeasonalAttributes", newArguments)
        Exit For
      End If
    Next
    PopulateGrid()
  End Sub

  Public Overrides Function ToString() As String
    Return agdMain.ToString
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


End Class
