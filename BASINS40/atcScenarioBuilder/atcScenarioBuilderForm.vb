Imports atcControls
Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Friend Class atcScenarioBuilderForm
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Private pInitializing As Boolean

  Public Sub New()
    MyBase.New()
    InitializeComponent() 'required by Windows Form Designer
    agdMain.AllowHorizontalScrolling = False
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
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  Friend WithEvents agdMain As atcControls.atcGrid
  Friend WithEvents mnuDisplay As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesRemove As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcScenarioBuilderForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuDisplay = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAttributes, Me.mnuDisplay})
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
    'mnuDisplay
    '
    Me.mnuDisplay.Index = 2
    Me.mnuDisplay.Text = "&Display"
    '
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(536, 249)
    Me.agdMain.TabIndex = 0
    '
    'mnuAttributes
    '
    Me.mnuAttributes.Index = 1
    Me.mnuAttributes.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributesAdd, Me.mnuAttributesRemove})
    Me.mnuAttributes.Text = "&Attributes"
    '
    'mnuAttributesAdd
    '
    Me.mnuAttributesAdd.Index = 0
    Me.mnuAttributesAdd.Text = "A&dd"
    '
    'mnuAttributesRemove
    '
    Me.mnuAttributesRemove.Index = 1
    Me.mnuAttributesRemove.Text = "&Remove"
    '
    'atcScenarioBuilderForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(536, 249)
    Me.Controls.Add(Me.agdMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcScenarioBuilderForm"
    Me.Text = "Climate Change Scenario Builder"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager
  Public Const ModifiedAttrName As String = "Modified Version"

  'The group of atcTimeseries displayed
  Private WithEvents pDataGroup As atcDataGroup

  'Translator class between pDataGroup and agdMain
  Private pSource As atcGridSource

  Public Sub Initialize(ByVal aDataManager As atcData.atcDataManager, _
               Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    pDataManager = aDataManager
    If aTimeseriesGroup Is Nothing Then
      pDataGroup = New atcDataGroup
    Else
      pDataGroup = aTimeseriesGroup
    End If

    mnuDisplay.MenuItems.Clear()
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      mnuDisplay.MenuItems.Add(lDisp.Name, New EventHandler(AddressOf mnuDisplay_Click))
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
    pSource = New GridSource(pDataManager, pDataGroup)
    agdMain.Initialize(pSource)
    agdMain.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub mnuDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisplay.Click
    Dim lNewDisplay As atcDataDisplay
    Dim lDisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    Dim lShowThese As New atcDataGroup
    For Each atf As atcDataDisplay In lDisplayPlugins
      If atf.Name = sender.Text Then
        Dim typ As System.Type = atf.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        lNewDisplay = asm.CreateInstance(typ.FullName)
        For Each lDataSet As atcDataSet In pDataGroup
          lShowThese.Add(lDataSet)
          lDataSet = lDataSet.Attributes.GetValue(ModifiedAttrName, Nothing)
          If Not lDataSet Is Nothing Then lShowThese.Add(lDataSet)
        Next
        lNewDisplay.Show(pDataManager, lShowThese)
        Exit Sub
      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pDataGroup, False)
  End Sub

  Private Sub mnuRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnu As MenuItem = sender
    Dim index As Integer = mnu.Index
    pDataManager.DisplayAttributes.RemoveAt(index)
    UpdatedCriteria()
    'RemoveCriteria(pcboCriteria(index), plstCriteria(index))
  End Sub

  Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
    PopulateGrid()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
    PopulateGrid()
    'TODO: could efficiently remove by serial number
  End Sub

  Private Sub agdMain_MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMain.MouseDownCell
    If aColumn = 3 Then
      Dim lDataIndex As Integer = (aRow - 1) \ pDataManager.DisplayAttributes.Count
      UserModify(pDataGroup.ItemByIndex(lDataIndex))
    End If
  End Sub

  Private Sub UpdatedCriteria()
    If Not pInitializing Then
      Dim mnu As MenuItem
      Dim iLastCriteria As Integer = pDataManager.DisplayAttributes.Count + 1

      'UpdateManagerSelectionAttributes()

      For Each mnu In mnuAttributesRemove.MenuItems
        RemoveHandler mnu.Click, AddressOf mnuRemove_Click
      Next
      'For Each mnu In mnuAttributesMove.MenuItems
      '  RemoveHandler mnu.Click, AddressOf mnuMove_Click
      'Next

      mnuAttributesRemove.MenuItems.Clear()
      'mnuAttributesMove.MenuItems.Clear()

      If iLastCriteria > 0 Then 'Only allow moving/removing if more than one exists
        For iCriteria As Integer = 0 To iLastCriteria
          mnu = mnuAttributesRemove.MenuItems.Add("&" & iCriteria + 1 & " " & pDataManager.DisplayAttributes.Item(iCriteria))
          AddHandler mnu.Click, AddressOf mnuRemove_Click
          'mnu = mnuAttributesMove.MenuItems.Add("&" & iCriteria + 1 & " " & pcboCriteria(iCriteria).SelectedItem)
          'AddHandler mnu.Click, AddressOf mnuMove_Click
        Next
      End If
      agdMain.Refresh()
    End If
  End Sub

  Private Sub UserModify(ByVal aBaseTimeseries As atcTimeseries)
    Dim lGenerateArguments As New atcDataAttributes
    Dim lNewTimeseries As atcTimeseries
    Dim lNewDataSource As atcDataSource
    Dim lCategories As New ArrayList
    lCategories.Add("Generate Timeseries")

    lGenerateArguments.SetValue("Timeseries", New atcDataGroup(aBaseTimeseries))
    lNewDataSource = pDataManager.UserSelectDataSource(lCategories)
    If Not lNewDataSource Is Nothing Then
      pDataManager.OpenDataSource(lNewDataSource, lNewDataSource.Specification, lGenerateArguments)
      For Each lNewTimeseries In lNewDataSource.DataSets
        For Each lAttribute As atcDefinedValue In aBaseTimeseries.Attributes
          If Not (lAttribute.Definition.Calculated) Then
            Select Case lAttribute.Definition.Name.ToLower
              Case "data source", "id"
              Case Else
                lNewTimeseries.Attributes.Add(lAttribute)
            End Select
          End If
        Next
      Next
      Select Case lNewDataSource.DataSets.Count
        Case 0 'didn't acutally compute anything, ignore
        Case 1
          lNewTimeseries = lNewDataSource.DataSets.ItemByIndex(0)
          aBaseTimeseries.Attributes.SetValue(ModifiedAttrName, lNewTimeseries)
          'pSource.CellValue(aRow, pSource.Columns - 1) = lNewDataSource.Specification
        Case Else
          MsgBox("Need to deal with multiple results in scenario builder", MsgBoxStyle.Critical, "UserModify")
      End Select
    End If
    agdMain.Refresh()
  End Sub
End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const pLabelRow As Integer = 0

  Private pDataManager As atcDataManager
  Private pDataGroup As atcDataGroup

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataManager = aDataManager
    pDataGroup = aDataGroup
  End Sub

  Public Overrides Property Columns() As Integer
    Get
      Return 4
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property Rows() As Integer
    Get
      Return pDataGroup.Count * pDataManager.DisplayAttributes.Count + pLabelRow + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = pLabelRow Then
        Select Case aColumn
          Case 0
            Return "Constituent"
          Case 1
            Return "Attribute"
          Case 2
            Return "Original"
          Case 3
            Return "Modified"
        End Select
      Else
        Dim lDataIndex As Integer = (aRow - pLabelRow - 1) \ pDataManager.DisplayAttributes.Count
        Dim lDataSet As atcDataSet = pDataGroup(lDataIndex)
        Dim lAttributeIndex As Integer = (aRow - pLabelRow - 1) Mod pDataManager.DisplayAttributes.Count
        Select Case aColumn
          Case 0
            If lAttributeIndex = 0 Then
              Return lDataSet.Attributes.GetFormattedValue("Constituent")
            Else
              Return """"
            End If
          Case 1
            Return pDataManager.DisplayAttributes(lAttributeIndex)
          Case 2
            Return lDataSet.Attributes.GetFormattedValue(pDataManager.DisplayAttributes(lAttributeIndex))
          Case 3
            Dim lModified As atcDataSet = lDataSet.Attributes.GetValue(atcScenarioBuilderForm.ModifiedAttrName, Nothing)
            If lModified Is Nothing Then
              Return "" '"(click to modify)"
            Else
              Return lModified.Attributes.GetFormattedValue(pDataManager.DisplayAttributes(lAttributeIndex))
              'Return lDataset.Attributes.GetValue("Data Source", lDataset.ToString)
            End If
        End Select
        'Select Case aColumn
        '  Case Is < 0
        '    Return ""
        '  Case Is > pDataManager.DisplayAttributes.Count
        '    Return ""
        '  Case pDataManager.DisplayAttributes.Count
        '    Dim lDataset As atcDataSet = pDataGroup(aRow - (pLabelRow + 1)).Attributes.GetValue(atcScenarioBuilderForm.ModifiedAttrName, Nothing)
        '    If lDataset Is Nothing Then
        '      Return "(click to modify)"
        '    Else
        '      Return lDataset.Attributes.GetValue("Data Source", lDataset.ToString)
        '    End If
        '  Case Else
        '    Return pDataGroup(aRow - (pLabelRow + 1)).Attributes.GetFormattedValue(pDataManager.DisplayAttributes(aColumn))
        'End Select
      End If
    End Get
    Set(ByVal newValue As String)
    End Set
  End Property

  Public Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      Return atcControls.atcAlignment.HAlignLeft
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property
End Class