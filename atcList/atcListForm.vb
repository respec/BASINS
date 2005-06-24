Imports atcData

Imports System.Windows.Forms

Friend Class atcListForm
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New(ByVal aDataManager As atcData.atcDataManager, _
        Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing)
    MyBase.New()
    pDataManager = aDataManager
    If aTimeseriesGroup Is Nothing Then
      pTimeseriesGroup = New atcTimeseriesGroup
    Else
      pTimeseriesGroup = aTimeseriesGroup
    End If
    InitializeComponent() 'required by Windows Form Designer

    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcTimeseriesDisplay))
    For Each atf As atcTimeseriesDisplay In DisplayPlugins
      mnuAnalysis.MenuItems.Add(atf.Name, New EventHandler(AddressOf mnuAnalysis_Click))
    Next

    If pTimeseriesGroup.Count = 0 Then 'ask user to specify some timeseries
      mnuFileAdd_Click(Nothing, Nothing)
    End If

    If pTimeseriesGroup.Count > 0 Then
      Me.Show()
      PopulateGrid()
    Else 'use declined to specify timeseries
      Me.Close()
    End If
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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcListForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAnalysis})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd})
    Me.mnuFile.Text = "File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "Add Timeseries"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 1
    Me.mnuAnalysis.Text = "Analysis"
    '
    'atcListForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 545)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcListForm"
    Me.Text = "Timeseries List"

  End Sub

#End Region

  Private pDataManager As atcDataManager

  'The grid control
  Private WithEvents agdMain As atcControls.atcGrid

  'The group of atcTimeseries displayed
  Private WithEvents pTimeseriesGroup As atcTimeseriesGroup

  'Translator class between pTimeseriesGroup and agdMain
  Private pSource As ListGridSource

  Private Sub PopulateGrid()
    pSource = New ListGridSource(pDataManager, pTimeseriesGroup)

    If Not agdMain Is Nothing Then
      Me.Controls.Remove(agdMain)
    End If

    agdMain = New atcControls.atcGrid(pSource)
    With agdMain
      .Location = New System.Drawing.Point(0, 0)
      .Name = "agdMain"
      .Size = Me.ClientSize
      .TabIndex = 14
      .Anchor = AnchorStyles.Top _
             Or AnchorStyles.Bottom _
             Or AnchorStyles.Left _
             Or AnchorStyles.Right
      Me.Controls.Add(agdMain)
      .Refresh()
    End With
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
    Dim newDisplay As atcTimeseriesDisplay
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcTimeseriesDisplay))
    For Each atf As atcTimeseriesDisplay In DisplayPlugins
      If atf.Name = sender.Text Then
        Dim typ As System.Type = atf.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        newDisplay = asm.CreateInstance(typ.FullName)
        newDisplay.Show(pDataManager, pTimeseriesGroup)
        Exit Sub
      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectTimeseries(, pTimeseriesGroup)
  End Sub

  Private Sub pTimeseriesGroup_Added(ByVal aAdded As Collections.ArrayList) Handles pTimeseriesGroup.Added
    PopulateGrid()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pTimeseriesGroup_Removed(ByVal aRemoved As System.Collections.ArrayList) Handles pTimeseriesGroup.Removed
    PopulateGrid()
    'TODO: could efficiently remove by serial number
  End Sub

End Class

Friend Class ListGridSource
  Inherits atcControls.atcGridSource

  Private pDataManager As atcDataManager
  Private pTimeseriesGroup As atcTimeseriesGroup

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup)
    pDataManager = aDataManager
    pTimeseriesGroup = aTimeseriesGroup
  End Sub

  Public Overrides Property Columns() As Integer
    Get
      Return pTimeseriesGroup.Count + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property Rows() As Integer
    Get
      Return pDataManager.DisplayAttributes.Count()
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aColumn = 0 Then
        Return pDataManager.DisplayAttributes(aRow)
      Else
        Return pTimeseriesGroup(aColumn - 1).Attributes.GetValue(pDataManager.DisplayAttributes(aRow))
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property
End Class