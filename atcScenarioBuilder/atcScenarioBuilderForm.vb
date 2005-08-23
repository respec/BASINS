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
    agdResults.AllowHorizontalScrolling = False
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
  Friend WithEvents btnRun As System.Windows.Forms.Button
  Friend WithEvents agdResults As atcControls.atcGrid
  Friend WithEvents splitHoriz As System.Windows.Forms.Splitter
  Friend WithEvents panelMiddle As System.Windows.Forms.Panel
  Friend WithEvents mnuScenarios As System.Windows.Forms.MenuItem
  Friend WithEvents mnuScenariosAdd As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcScenarioBuilderForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.mnuDisplay = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.splitHoriz = New System.Windows.Forms.Splitter
    Me.panelMiddle = New System.Windows.Forms.Panel
    Me.btnRun = New System.Windows.Forms.Button
    Me.agdResults = New atcControls.atcGrid
    Me.mnuScenarios = New System.Windows.Forms.MenuItem
    Me.mnuScenariosAdd = New System.Windows.Forms.MenuItem
    Me.panelMiddle.SuspendLayout()
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAttributes, Me.mnuDisplay, Me.mnuScenarios})
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
    'mnuDisplay
    '
    Me.mnuDisplay.Index = 2
    Me.mnuDisplay.Text = "&Display"
    '
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Top
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(536, 249)
    Me.agdMain.TabIndex = 0
    '
    'splitHoriz
    '
    Me.splitHoriz.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitHoriz.Location = New System.Drawing.Point(0, 249)
    Me.splitHoriz.Name = "splitHoriz"
    Me.splitHoriz.Size = New System.Drawing.Size(536, 8)
    Me.splitHoriz.TabIndex = 1
    Me.splitHoriz.TabStop = False
    '
    'panelMiddle
    '
    Me.panelMiddle.Controls.Add(Me.btnRun)
    Me.panelMiddle.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelMiddle.Location = New System.Drawing.Point(0, 257)
    Me.panelMiddle.Name = "panelMiddle"
    Me.panelMiddle.Size = New System.Drawing.Size(536, 47)
    Me.panelMiddle.TabIndex = 2
    '
    'btnRun
    '
    Me.btnRun.Location = New System.Drawing.Point(16, 8)
    Me.btnRun.Name = "btnRun"
    Me.btnRun.Size = New System.Drawing.Size(96, 24)
    Me.btnRun.TabIndex = 0
    Me.btnRun.Text = "Run Model"
    '
    'agdResults
    '
    Me.agdResults.AllowHorizontalScrolling = True
    Me.agdResults.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdResults.LineColor = System.Drawing.Color.Empty
    Me.agdResults.LineWidth = 0.0!
    Me.agdResults.Location = New System.Drawing.Point(0, 304)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(536, 201)
    Me.agdResults.TabIndex = 3
    '
    'mnuScenarios
    '
    Me.mnuScenarios.Index = 3
    Me.mnuScenarios.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuScenariosAdd})
    Me.mnuScenarios.Text = "&Scenarios"
    '
    'mnuScenariosAdd
    '
    Me.mnuScenariosAdd.Index = 0
    Me.mnuScenariosAdd.Text = "&Add"
    '
    'atcScenarioBuilderForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(536, 505)
    Me.Controls.Add(Me.agdResults)
    Me.Controls.Add(Me.panelMiddle)
    Me.Controls.Add(Me.splitHoriz)
    Me.Controls.Add(Me.agdMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcScenarioBuilderForm"
    Me.Text = "Climate Change Scenario Builder"
    Me.panelMiddle.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager
  Public Const ModifiedAttrName As String = "Modified Version"

  'The group of atcTimeseries containing base conditions
  Private WithEvents pBaseScenario As atcDataGroup

  'a collection of atcDataGroups, one for each modified scenario
  Private pModifiedScenarios As atcCollection

  'The group of atcTimeseries containing model output from base conditions
  Private WithEvents pBaseResults As atcDataGroup

  'a collection of atcDataGroups, one for each modified scenario
  Private pModifiedResults As atcCollection

  'Translator class between pBaseScenario/pNewScenarios and agdMain
  Private pSource As atcGridSource

  'Translator class between pBaseResults and agdMain
  Private pResultSource As atcGridSource

  Public Sub Initialize(ByVal aDataManager As atcData.atcDataManager, _
               Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    pInitializing = True

    pDataManager = aDataManager
    If aTimeseriesGroup Is Nothing Then
      pBaseScenario = New atcDataGroup
    Else
      pBaseScenario = aTimeseriesGroup
    End If

    mnuDisplay.MenuItems.Clear()
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      mnuDisplay.MenuItems.Add(lDisp.Name, New EventHandler(AddressOf mnuDisplay_Click))
    Next

    If pBaseScenario.Count = 0 Then
      'By default add EVAP and PREC if none were given
      For Each lDataSet As atcDataSet In pDataManager.DataSets
        Select Case lDataSet.Attributes.GetValue("Constituent")
          Case "PET", "HPRECIP" : pBaseScenario.Add(lDataSet)
        End Select
      Next
      'If we didn't find any to add, ask user to select some data
      If pBaseScenario.Count = 0 Then
        pDataManager.UserSelectData(, pBaseScenario, True)
      End If
    End If

    If pBaseScenario.Count > 0 Then
      pModifiedScenarios = New atcCollection
      'Dim lExistingModified As New atcDataGroup
      'For Each lDataSet As atcDataSet In pBaseScenario
      '  Dim lModifiedDataSet As atcDataSet = lDataSet.Attributes.GetValue(ModifiedAttrName, Nothing)
      '  If Not lModifiedDataSet Is Nothing Then
      '    lExistingModified.Add(lModifiedDataSet)
      '  End If
      'Next
      'If lExistingModified.Count > 0 Then
      '  pModifiedScenarios.Add(lExistingModified)
      'End If

      pBaseResults = New atcDataGroup
      For Each lDataSet As atcDataSet In pDataManager.DataSets
        If lDataSet.Attributes.GetValue("Constituent").ToLower = "flow" _
         AndAlso lDataSet.Attributes.GetValue("Scenario").ToLower = "base" Then
          pBaseResults.Add(lDataSet.Attributes.GetValue("id"), lDataSet)
        End If
      Next

      pModifiedResults = New atcCollection

      Me.Show()
      PopulateGrid()
      pInitializing = False
      UpdatedAttributes()
    Else 'user declined to specify timeseries
      Me.Close()
    End If

  End Sub

  Private Sub PopulateGrid()
    pSource = New GridSource(pDataManager, pBaseScenario, pModifiedScenarios)
    pResultSource = New GridSource(pDataManager, pBaseResults, pModifiedResults)

    agdMain.Initialize(pSource)
    agdResults.Initialize(pResultSource)

    If Not pInitializing Then
      agdMain.Refresh()
      agdResults.Refresh()
    End If
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
        For Each lDataSet As atcDataSet In pBaseScenario
          lShowThese.Add(lDataSet)
          'lDataSet = lDataSet.Attributes.GetValue(ModifiedAttrName, Nothing)
          'If Not lDataSet Is Nothing Then lShowThese.Add(lDataSet)
        Next
        lNewDisplay.Show(pDataManager, lShowThese)
        Exit Sub
      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pBaseScenario, False)
  End Sub

  Private Sub mnuAttributesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnu As MenuItem = sender
    'pDataManager.DisplayAttributes.Add(mnu.Text.Substring(mnu.Text.IndexOf(" ") + 1))
    pDataManager.DisplayAttributes.Add(mnu.Text)
    UpdatedAttributes()
  End Sub

  Private Sub mnuAttributesRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnu As MenuItem = sender
    Dim index As Integer = mnu.Index
    pDataManager.DisplayAttributes.RemoveAt(index)
    UpdatedAttributes()
  End Sub

  Private Sub pBaseScenario_Added(ByVal aAdded As atcCollection) Handles pBaseScenario.Added
    If Not pInitializing Then agdMain.Refresh()
    'PopulateGrid()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pBaseScenario_Removed(ByVal aRemoved As atcCollection) Handles pBaseScenario.Removed
    If Not pInitializing Then agdMain.Refresh()
    'PopulateGrid()
    'TODO: could efficiently remove by serial number
  End Sub

  Private Sub agdMain_MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMain.MouseDownCell
    If aColumn > 2 Then
      Dim lDataIndex As Integer = (aRow - 1) \ pDataManager.DisplayAttributes.Count
      UserModify(pBaseScenario.ItemByIndex(lDataIndex), pModifiedScenarios.ItemByIndex(aColumn - 3))
    End If
  End Sub

  Private Sub UpdatedAttributes()
    'If Not pInitializing Then
    Dim mnu As MenuItem
    Dim iLastAttribute As Integer
    Dim iAttribute As Integer

    'UpdateManagerSelectionAttributes()

    For Each mnu In mnuAttributesRemove.MenuItems
      RemoveHandler mnu.Click, AddressOf mnuAttributesRemove_Click
    Next
    'For Each mnu In mnuAttributesMove.MenuItems
    '  RemoveHandler mnu.Click, AddressOf mnuMove_Click
    'Next
    For Each mnu In mnuAttributesAdd.MenuItems
      RemoveHandler mnu.Click, AddressOf mnuAttributesAdd_Click
    Next

    mnuAttributesRemove.MenuItems.Clear()
    'mnuAttributesMove.MenuItems.Clear()
    mnuAttributesAdd.MenuItems.Clear()

    iLastAttribute = pDataManager.DisplayAttributes.Count - 1
    If iLastAttribute > 0 Then 'Only allow moving/removing if more than one exists
      For iAttribute = 0 To iLastAttribute
        mnu = mnuAttributesRemove.MenuItems.Add("&" & iAttribute + 1 & " " & pDataManager.DisplayAttributes.Item(iAttribute))
        AddHandler mnu.Click, AddressOf mnuAttributesRemove_Click
        'mnu = mnuAttributesMove.MenuItems.Add("&" & iAttribute + 1 & " " & pcboAttribute(iAttribute).SelectedItem)
        'AddHandler mnu.Click, AddressOf mnuMove_Click
      Next
    End If

    Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
    iLastAttribute = lAllDefinitions.Count - 1
    For iAttribute = 0 To iLastAttribute
      Dim def As atcAttributeDefinition = lAllDefinitions.ItemByIndex(iAttribute)
      If Not pDataManager.DisplayAttributes.Contains(def.Name) _
       AndAlso def.TypeString <> "atcTimeseries" Then
        'mnu = mnuAttributesAdd.MenuItems.Add("&" & iAttribute + 1 & " " & def.Name)
        mnu = mnuAttributesAdd.MenuItems.Add(def.Name)
        AddHandler mnu.Click, AddressOf mnuAttributesAdd_Click
      End If
    Next

    If Not pInitializing Then
      agdMain.Refresh()
      agdResults.Refresh()
    End If
    'End If
  End Sub

  Private Sub UserModify(ByVal aBaseTimeseries As atcTimeseries, ByVal aModifiedScenario As atcDataGroup)
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
        'For Each lAttribute As atcDefinedValue In aBaseTimeseries.Attributes
        '  If Not (lAttribute.Definition.Calculated) Then
        '    Select Case lAttribute.Definition.Name.ToLower
        '      Case "data source" ', "id"
        '      Case Else
        '        lNewTimeseries.Attributes.Add(lAttribute)
        '    End Select
        '  End If
        'Next
        Dim lID As String = lNewTimeseries.Attributes.GetValue("id")
        aModifiedScenario.RemoveByKey(lID)
        aModifiedScenario.Add(lID, lNewTimeseries)
      Next
      'Select Case lNewDataSource.DataSets.Count
      '  Case 0 'didn't acutally compute anything, ignore
      '  Case 1
      '    lNewTimeseries = lNewDataSource.DataSets.ItemByIndex(0)

      '    'aBaseTimeseries.Attributes.SetValue(ModifiedAttrName, lNewTimeseries)
      '    'pSource.CellValue(aRow, pSource.Columns - 1) = lNewDataSource.Specification
      '  Case Else
      '    MsgBox("Need to deal with multiple results in scenario builder", MsgBoxStyle.Critical, "UserModify")
      'End Select
    End If
    agdMain.Refresh()
    agdResults.Refresh()

  End Sub

  Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
    'Copy base UCI and change scenario name within it
    'Copy WDM
    'Change data to be modified in new WDM
    'Change scenario attribute in new WDM
    'Run WinHSPFlt with the new UCI
    'Reload datasets in Results grid

    Dim lModifiedIndex As Integer = 0 'TODO: use a selected modified version, not just the first one
    Dim lNewScenarioName As String = "modified"
    Dim lCurrentWDMfilename As String
    For Each lDataSource As atcDataSource In pDataManager.DataSources
      If lDataSource.DataSets.Contains(pBaseScenario.ItemByIndex(0)) Then
        lCurrentWDMfilename = lDataSource.Specification
      End If
    Next
    If FileExists(lCurrentWDMfilename) Then
      Dim lNewFilename As String = PathNameOnly(lCurrentWDMfilename) & "\" & lNewScenarioName & "."
      'Copy base UCI and change scenario name within it
      ReplaceStringToFile(WholeFileString(FilenameSetExt(lCurrentWDMfilename, "uci")), "base.", lNewScenarioName & ".", lNewFilename & "uci")

      'Copy base WDM to new WDM
      FileCopy(lCurrentWDMfilename, lNewFilename & "wdm")
      Dim lNewWDM As New atcWDM.atcDataSourceWDM
      If lNewWDM.Open(lNewFilename & "wdm") Then
        Dim lCurrentTimeseries As atcTimeseries
        For Each lCurrentTimeseries In pModifiedScenarios.ItemByIndex(lModifiedIndex)
          lCurrentTimeseries.Attributes.SetValue("Scenario", lNewScenarioName)
          lNewWDM.AddDataSet(lCurrentTimeseries)
        Next

        For Each lCurrentTimeseries In lNewWDM.DataSets
          If lCurrentTimeseries.Attributes.GetValue("Scenario") = "BASE" Then
            lCurrentTimeseries.Attributes.SetValue("Scenario", lNewScenarioName)
            lNewWDM.AddDataSet(lCurrentTimeseries) 'TODO: Would be nice to just update this attribute, not rewrite all data values
          End If
        Next

        Shell("D:\BASINS\models\hspf\bin\WinHspfLt.exe " & lNewFilename & "uci", AppWinStyle.NormalNoFocus, True)

        Dim lModifiedResults As atcDataGroup = pModifiedResults.ItemByIndex(lModifiedIndex)
        lModifiedResults.Clear()
        For Each lModifiedData As atcDataSet In lNewWDM.DataSets
          If pBaseResults.Keys.Contains(lModifiedData.Attributes.GetValue("id")) Then
            'Found data matching one of the base results
            lModifiedResults.Add(lModifiedData)
          End If
        Next
        agdResults.Refresh()
      Else
        MsgBox("Could not open new scenario WDM file '" & lNewFilename & "wdm'", MsgBoxStyle.Critical, "Could not run model")
      End If
    End If
    Dim lNewWDMfile As atcDataSource

  End Sub

  Private Sub agdMain_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles agdMain.UserResizedColumn
    agdResults.ColumnWidth(aColumn) = aWidth
    agdResults.Refresh()
  End Sub

  Private Sub agdResults_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles agdResults.UserResizedColumn
    agdMain.ColumnWidth(aColumn) = aWidth
    agdMain.Refresh()
  End Sub

  Private Sub mnuScenariosAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScenariosAdd.Click
    Dim lNewModified As New atcDataGroup
    Dim lNewResults As New atcDataGroup

    pModifiedScenarios.Add(lNewModified)
    pModifiedResults.Add(lNewResults)

    For iColumn As Integer = 0 To pSource.Columns - 1
      agdMain.SizeColumnToContents(iColumn)
      agdResults.SizeColumnToContents(iColumn)
      If agdMain.ColumnWidth(iColumn) > agdResults.ColumnWidth(iColumn) Then
        agdResults.ColumnWidth(iColumn) = agdMain.ColumnWidth(iColumn)
      Else
        agdMain.ColumnWidth(iColumn) = agdResults.ColumnWidth(iColumn)
      End If
    Next
    agdMain.Refresh()
    agdResults.Refresh()
  End Sub
End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const pLabelRow As Integer = 0

  Private pDataManager As atcDataManager
  Private pBaseScenario As atcDataGroup
  Private pModifiedScenarios As atcCollection

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aBaseScenario As atcData.atcDataGroup, _
          ByVal aModifiedScenarios As atcCollection)
    pDataManager = aDataManager
    pBaseScenario = aBaseScenario
    pModifiedScenarios = aModifiedScenarios
  End Sub

  Public Overrides Property Columns() As Integer
    Get
      If pModifiedScenarios Is Nothing Then
        Return 3
      Else
        Return 3 + pModifiedScenarios.Count
      End If

    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property Rows() As Integer
    Get
      If pBaseScenario Is Nothing OrElse pDataManager Is Nothing Then
        Return pLabelRow + 1
      Else
        Return pBaseScenario.Count * pDataManager.DisplayAttributes.Count + pLabelRow + 1
      End If
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
            Return "Modification "
          Case Is > 3
            'If aColumn Mod 2 = 1 Then 'odd columns contain relationships
            Return "Modification #" & (aColumn - 2)
            'Else
            '  Return "New Value #" & (aColumn - 2) / 2
            'End If
        End Select
      Else
        Dim lDataIndex As Integer = (aRow - pLabelRow - 1) \ pDataManager.DisplayAttributes.Count
        Dim lAttributeIndex As Integer = (aRow - pLabelRow - 1) Mod pDataManager.DisplayAttributes.Count
        Select Case aColumn
          Case 0
            If lAttributeIndex = 0 Then
              Return pBaseScenario(lDataIndex).Attributes.GetFormattedValue("Constituent")
            Else
              Return """"
            End If
          Case 1
            Return pDataManager.DisplayAttributes(lAttributeIndex)
          Case 2
            Return pBaseScenario(lDataIndex).Attributes.GetFormattedValue(pDataManager.DisplayAttributes(lAttributeIndex))
          Case Is > 2
            Dim lModifiedGroup As atcDataGroup = pModifiedScenarios.ItemByIndex(aColumn - 3) 'lDataSet.Attributes.GetValue(atcScenarioBuilderForm.ModifiedAttrName, Nothing)
            Dim lBaseID As String = pBaseScenario(lDataIndex).Attributes.GetValue("id")
            For Each lModifiedData As atcDataSet In lModifiedGroup
              If lModifiedData.Attributes.GetValue("id") = lBaseID Then 'Found modified dataset for this cell
                'If aColumn Mod 2 = 1 Then 'odd columns contain relationships
                'Return lModifiedData.Attributes.GetValue("History 1")
                'Else 'Even columns contain values
                Dim lAttributeName As String = pDataManager.DisplayAttributes(lAttributeIndex)
                Dim lNewValue As Object = lModifiedData.Attributes.GetValue(lAttributeName)
                If IsNumeric(lNewValue) Then
                  Dim lOldValue As Object = pBaseScenario(lDataIndex).Attributes.GetValue(lAttributeName)
                  If IsNumeric(lOldValue) Then
                    Dim lPercentDifference As Double = (lNewValue - lOldValue) / lOldValue
                    If Not Double.IsNaN(lPercentDifference) AndAlso Math.Abs(lPercentDifference) > 0.005 Then
                      Return Format(lNewValue, "#,##0.#####") & " (" _
                           & Format(lPercentDifference, "+#,##0.##%;-#,##0.##%") & ")"
                    End If
                  End If
                End If
                Return lNewValue
              End If
            Next
            'No modified dataset matching that ID yet
            Return "" '    "(click to modify)"

        End Select
        'Select Case aColumn
        '  Case Is < 0
        '    Return ""
        '  Case Is > pDataManager.DisplayAttributes.Count
        '    Return ""
        '  Case pDataManager.DisplayAttributes.Count
        '    Dim lDataset As atcDataSet = pBaseScenario(aRow - (pLabelRow + 1)).Attributes.GetValue(atcScenarioBuilderForm.ModifiedAttrName, Nothing)
        '    If lDataset Is Nothing Then
        '      Return "(click to modify)"
        '    Else
        '      Return lDataset.Attributes.GetValue("Data Source", lDataset.ToString)
        '    End If
        '  Case Else
        '    Return pBaseScenario(aRow - (pLabelRow + 1)).Attributes.GetFormattedValue(pDataManager.DisplayAttributes(aColumn))
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