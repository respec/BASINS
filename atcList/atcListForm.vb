Imports atcData

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
      Populate()
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
  Friend WithEvents gridMain As System.Windows.Forms.DataGrid
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcListForm))
    Me.gridMain = New System.Windows.Forms.DataGrid
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    CType(Me.gridMain, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'gridMain
    '
    Me.gridMain.AllowNavigation = False
    Me.gridMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.gridMain.CaptionVisible = False
    Me.gridMain.DataMember = ""
    Me.gridMain.HeaderForeColor = System.Drawing.SystemColors.ControlText
    Me.gridMain.Location = New System.Drawing.Point(0, -16)
    Me.gridMain.Name = "gridMain"
    Me.gridMain.ReadOnly = True
    Me.gridMain.RowHeadersVisible = False
    Me.gridMain.Size = New System.Drawing.Size(528, 560)
    Me.gridMain.TabIndex = 13
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAnalysis})
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 1
    Me.mnuAnalysis.Text = "Analysis"
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
    'atcListForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 541)
    Me.Controls.Add(Me.gridMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcListForm"
    Me.Text = "Timeseries List"
    CType(Me.gridMain, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager

  Private pTable As System.Data.DataTable

  'The group of atcTimeseries displayed
  Private WithEvents pTimeseriesGroup As atcTimeseriesGroup

  Private Sub Populate()
    Dim tblSty As Windows.Forms.DataGridTableStyle

    pTable = New System.Data.DataTable
    With pTable.Columns
      .Add(New System.Data.DataColumn("atcTimeseries"))
      .Item(0).DataType = GetType(atcTimeseries)
      pTable.PrimaryKey = New Data.DataColumn() {.Item(0)}

      For Each lAttribName As String In pDataManager.DisplayAttributes
        .Add(New System.Data.DataColumn(lAttribName))
      Next
    End With

    For Each lColumn As System.Data.DataColumn In pTable.Columns
      lColumn.DataType = GetType(String)
      lColumn.ReadOnly = True
    Next

    gridMain.SetDataBinding(pTable, "")

    ' Hide the first column of each grid (holds serial #)
    tblSty = New Windows.Forms.DataGridTableStyle
    tblSty.RowHeadersVisible = False
    gridMain.TableStyles.Add(tblSty)
    gridMain.TableStyles(0).GridColumnStyles(0).Width = 0

    'Populate pTable from selected group
    For Each ts As atcTimeseries In pTimeseriesGroup
      AddTStoTable(ts, pTable)
    Next
  End Sub

  Private Sub AddTStoTable(ByVal aTS As atcData.atcTimeseries, ByVal aTable As System.Data.DataTable)
    Dim row() As Object
    ReDim row(aTable.Columns.Count - 1)
    row(0) = aTS.Serial
    For iColumn As Integer = 1 To aTable.Columns.Count - 1
      row(iColumn) = aTS.Attributes.GetValue(aTable.Columns(iColumn).ColumnName)
    Next
    aTable.Rows.Add(row)
  End Sub
  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub gridMatching_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    Dim lRow As Integer = gridMain.HitTest(e.X, e.Y).Row
    If IsNumeric(gridMain.Item(lRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(gridMain.Item(lRow, 0)) 'Serial number in clicked row
      Dim iTS As Integer = pTimeseriesGroup.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Already selected, unselect
        'TODO
      Else 'Not already selected, select it now
        'TODO
      End If
    End If
  End Sub

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
    Populate()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pTimeseriesGroup_Removed(ByVal aRemoved As System.Collections.ArrayList) Handles pTimeseriesGroup.Removed
    Populate()
    'TODO: could efficiently remove by serial number
  End Sub

End Class
