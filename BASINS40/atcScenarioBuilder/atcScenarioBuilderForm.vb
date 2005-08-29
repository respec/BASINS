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
  Friend WithEvents agdResults As atcControls.atcGrid
  Friend WithEvents splitHoriz As System.Windows.Forms.Splitter
  Friend WithEvents panelMiddle As System.Windows.Forms.Panel
  Friend WithEvents mnuScenarios As System.Windows.Forms.MenuItem
  Friend WithEvents mnuScenariosAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAddResults As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcScenarioBuilderForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.mnuDisplay = New System.Windows.Forms.MenuItem
    Me.mnuScenarios = New System.Windows.Forms.MenuItem
    Me.mnuScenariosAdd = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.splitHoriz = New System.Windows.Forms.Splitter
    Me.panelMiddle = New System.Windows.Forms.Panel
    Me.agdResults = New atcControls.atcGrid
    Me.mnuFileAddResults = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAttributes, Me.mnuDisplay, Me.mnuScenarios})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd, Me.mnuFileAddResults})
    Me.mnuFile.Text = "File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "Add Input"
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
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Top
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(643, 287)
    Me.agdMain.TabIndex = 0
    '
    'splitHoriz
    '
    Me.splitHoriz.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitHoriz.Location = New System.Drawing.Point(0, 287)
    Me.splitHoriz.Name = "splitHoriz"
    Me.splitHoriz.Size = New System.Drawing.Size(643, 10)
    Me.splitHoriz.TabIndex = 1
    Me.splitHoriz.TabStop = False
    '
    'panelMiddle
    '
    Me.panelMiddle.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelMiddle.Location = New System.Drawing.Point(0, 297)
    Me.panelMiddle.Name = "panelMiddle"
    Me.panelMiddle.Size = New System.Drawing.Size(643, 54)
    Me.panelMiddle.TabIndex = 2
    '
    'agdResults
    '
    Me.agdResults.AllowHorizontalScrolling = True
    Me.agdResults.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdResults.LineColor = System.Drawing.Color.Empty
    Me.agdResults.LineWidth = 0.0!
    Me.agdResults.Location = New System.Drawing.Point(0, 351)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(643, 231)
    Me.agdResults.TabIndex = 3
    '
    'mnuFileAddResults
    '
    Me.mnuFileAddResults.Index = 1
    Me.mnuFileAddResults.Text = "Add Results"
    '
    'atcScenarioBuilderForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(643, 582)
    Me.Controls.Add(Me.agdResults)
    Me.Controls.Add(Me.panelMiddle)
    Me.Controls.Add(Me.splitHoriz)
    Me.Controls.Add(Me.agdMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcScenarioBuilderForm"
    Me.Text = "Climate Change Scenario Builder"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const MenuText_AllInputs = "All Input"
  Private Const MenuText_AllResults = "All Results"

  Private pDataManager As atcDataManager

  'The group of atcTimeseries containing base conditions
  Private pBaseScenario As atcDataSource

  'a collection of atcDataGroups, one for each modified scenario
  Private pModifiedScenarios As atcCollection

  'The group of atcTimeseries containing model output from base conditions
  Private pBaseResults As atcDataSource

  'a collection of atcDataGroups, one for each modified scenario
  Private pModifiedResults As atcCollection

  'Translator class between pBaseScenario/pNewScenarios and agdMain
  Private pSource As GridSource

  'Translator class between pBaseResults and agdMain
  Private pResultSource As atcGridSource

  Private pRunButton() As Windows.Forms.Button

  Public Sub Initialize(ByVal aDataManager As atcData.atcDataManager, _
               Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    pInitializing = True

    pDataManager = aDataManager
    pBaseScenario = New atcDataSource
    If Not aTimeseriesGroup Is Nothing Then
      pBaseScenario.DataSets.AddRange(aTimeseriesGroup) 'TODO: want to share events with aTimeseriesGroup
    End If

    mnuDisplay.MenuItems.Clear()
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      mnuDisplay.MenuItems.Add(lDisp.Name, New EventHandler(AddressOf mnuDisplay_Click))
    Next

    If pBaseScenario.DataSets.Count = 0 Then
      'By default add EVAP and PREC if none were given
      For Each lDataSet As atcDataSet In pDataManager.DataSets
        Select Case lDataSet.Attributes.GetValue("Constituent")
          Case "EVAP", "PREC", "PET", "HPRECIP" : pBaseScenario.DataSets.Add(lDataSet)
        End Select
      Next
      'If we didn't find any to add, ask user to select some data
      If pBaseScenario.DataSets.Count = 0 Then
        pDataManager.UserSelectData(, pBaseScenario.DataSets, True)
      End If
    End If

    If pBaseScenario.DataSets.Count > 0 Then
      pModifiedScenarios = New atcCollection

      pBaseResults = New atcDataSource
      For Each lDataSet As atcDataSet In pDataManager.DataSets
        If lDataSet.Attributes.GetValue("Constituent").ToLower = "flow" _
         AndAlso lDataSet.Attributes.GetValue("Scenario").ToLower = "base" Then
          pBaseResults.DataSets.Add(lDataSet.Attributes.GetValue("id"), lDataSet)
        End If
      Next

      pModifiedResults = New atcCollection

      ReDim pRunButton(0)
      pRunButton(0) = New Windows.Forms.Button
      panelMiddle.Controls.Add(pRunButton(0))
      AddHandler pRunButton(0).Click, AddressOf RunButton_Click
      With pRunButton(0)
        .Name = "Run-1"
        .Tag = -1
        .Text = "Run"
        .Width = 55
      End With

      Me.Show()
      PopulateGrid()
      pInitializing = False
      UpdatedAttributes()
      Application.DoEvents()
      AddScenario()
    Else 'user declined to specify timeseries
      Me.Close()
    End If

  End Sub

  Private Sub PopulateGrid()
    pSource = New GridSource(pBaseScenario, pModifiedScenarios)
    pResultSource = New GridSource(pBaseResults, pModifiedResults)

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

        For Each lDataSet As atcDataSet In pBaseScenario.DataSets
          lShowThese.Add(lDataSet)
        Next
        lNewDisplay.Show(pDataManager, lShowThese)
        Exit Sub

      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pBaseScenario.DataSets, False)
  End Sub

  Private Sub mnuFileAddResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAddResults.Click
    pDataManager.UserSelectData(, pBaseResults.DataSets, False)
  End Sub

  Private Sub mnuAttributesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnuAttribute As MenuItem = sender
    Dim mnuConstituent As MenuItem = mnuAttribute.Parent
    Dim allInputs As Boolean = mnuConstituent.Text.Equals(MenuText_AllInputs)
    Dim allResults As Boolean = mnuConstituent.Text.Equals(MenuText_AllResults)
    Dim lNeedMainRefresh As Boolean = False
    Dim lNeedResultsRefresh As Boolean = False
    'MsgBox("Add " & mnuConstituent.Text, MsgBoxStyle.Information, mnuAttribute.Text)

    For Each lDataSet As atcDataSet In pBaseScenario.DataSets
      If allInputs OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
        Dim lAttributes As atcDataAttributes = GetScenarioAttributes(lDataSet)
        lAttributes.SetValue(mnuAttribute.Text, "")
        lNeedMainRefresh = True
      End If
    Next
    For Each lDataSet As atcDataSet In pBaseResults.DataSets
      If allResults OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
        Dim lAttributes As atcDataAttributes = GetScenarioAttributes(lDataSet)
        lAttributes.SetValue(mnuAttribute.Text, "")
        lNeedResultsRefresh = True
      End If
    Next
    If lNeedMainRefresh Then agdMain.Refresh()
    If lNeedResultsRefresh Then agdResults.Refresh()
    UpdatedAttributes()
  End Sub

  Private Sub mnuAttributesRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnuAttribute As MenuItem = sender
    Dim mnuConstituent As MenuItem = mnuAttribute.Parent
    Dim allInputs As Boolean = mnuConstituent.Text.Equals(MenuText_AllInputs)
    Dim allResults As Boolean = mnuConstituent.Text.Equals(MenuText_AllResults)
    Dim lNeedMainRefresh As Boolean = False
    Dim lNeedResultsRefresh As Boolean = False
    'MsgBox("Remove " & mnuConstituent.Text, MsgBoxStyle.Information, mnuAttribute.Text)

    For Each lDataSet As atcDataSet In pBaseScenario.DataSets
      If allInputs OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
        Dim lAttributes As atcDataAttributes = lDataSet.Attributes.GetValue("Scenario Attributes")
        If Not lAttributes Is Nothing Then
          lAttributes.RemoveByKey(mnuAttribute.Text.ToLower)
          lNeedMainRefresh = True
        End If
      End If
    Next

    For Each lDataSet As atcDataSet In pBaseResults.DataSets
      If allResults OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
        Dim lAttributes As atcDataAttributes = lDataSet.Attributes.GetValue("Scenario Attributes")
        If Not lAttributes Is Nothing Then
          lAttributes.RemoveByKey(mnuAttribute.Text.ToLower)
          lNeedResultsRefresh = True
        End If
      End If
    Next
    If lNeedMainRefresh Then agdMain.Refresh()
    If lNeedResultsRefresh Then agdResults.Refresh()
    UpdatedAttributes()
  End Sub

  'Private Sub pBaseScenario_Added(ByVal aAdded As atcCollection) Handles pBaseScenario.Added
  '  If Not pInitializing Then agdMain.Refresh()
  '  'PopulateGrid()
  '  'TODO: could efficiently insert newly added item(s)
  'End Sub

  'Private Sub pBaseScenario_Removed(ByVal aRemoved As atcCollection) Handles pBaseScenario.Removed
  '  If Not pInitializing Then agdMain.Refresh()
  '  'PopulateGrid()
  '  'TODO: could efficiently remove by serial number
  'End Sub

  Private Sub agdMain_MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMain.MouseDownCell
    If aColumn > 2 Then
      Dim pAttributeIndex As Integer
      Dim lBaseDataSet As atcDataSet = pSource.BaseDataSetInRow(aRow, pAttributeIndex) '(aRow - 1) \ pDataManager.DisplayAttributes.Count
      UserModify(lBaseDataSet, pModifiedScenarios.ItemByIndex(aColumn - 3))
    End If
  End Sub

  Private Function GetScenarioAttributes(ByVal aDataSet As atcDataSet) As atcDataAttributes
    Dim lConstituent As String
    Dim lAttributes As atcDataAttributes = aDataSet.Attributes.GetValue("Scenario Attributes")

    If lAttributes Is Nothing Then
      lAttributes = New atcDataAttributes
      lAttributes.Add("Min", "")
      lAttributes.Add("Max", "")
      lAttributes.Add("Mean", "")
      lConstituent = aDataSet.Attributes.GetValue("constituent")
      If lConstituent.ToLower = "flow" Then
        lAttributes.Add("7Q10", "")
        lAttributes.Add("1Hi100", "")
      End If
      aDataSet.Attributes.SetValue("Scenario Attributes", lAttributes)
    End If
    Return lAttributes
  End Function

  Private Sub PopulateAddRemoveAttributesMenus(ByVal aDataSet As atcDataSet, ByRef aAddedConstituents As String, ByRef lAttributeRemovable As String)
    Dim lConstituent As String
    Dim mnuConstituentAdd As MenuItem
    Dim mnuConstituentRemove As MenuItem
    Dim mnuAttribute As MenuItem
    lConstituent = aDataSet.Attributes.GetValue("constituent")
    If lConstituent.Length > 0 AndAlso aAddedConstituents.IndexOf("++" & lConstituent & "++") = -1 Then
      Dim lAddedAttributes As String = "++"
      aAddedConstituents &= lConstituent & "++"
      mnuConstituentAdd = mnuAttributesAdd.MenuItems.Add(lConstituent)
      mnuConstituentRemove = mnuAttributesRemove.MenuItems.Add(lConstituent)
      'AddHandler mnu.Click, AddressOf mnuAttributesRemove_Click
      Dim lAttributes As atcDataAttributes = GetScenarioAttributes(aDataSet)
      For Each lAttribute As atcDefinedValue In lAttributes
        lAddedAttributes &= lAttribute.Definition.Name & "++"
        mnuAttribute = mnuConstituentRemove.MenuItems.Add(lAttribute.Definition.Name)
        AddHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click
      Next
      Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
      For Each lAttributeDef As atcAttributeDefinition In lAllDefinitions
        If lAddedAttributes.IndexOf("++" & lAttributeDef.Name & "++") = -1 Then
          mnuAttribute = mnuConstituentAdd.MenuItems.Add(lAttributeDef.Name)
          AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click
        End If
      Next
      lAttributeRemovable &= lAddedAttributes
    End If

  End Sub

  Private Sub UpdatedAttributes()
    'If Not pInitializing Then
    Dim lAddedConstituents As String = "++"
    Dim lAttributeRemovable As String = "++"
    Dim mnuConstituentAddInputs As MenuItem
    Dim mnuConstituentRemoveInputs As MenuItem
    Dim mnuConstituentAddResults As MenuItem
    Dim mnuConstituentRemoveResults As MenuItem
    Dim mnuAttribute As MenuItem
    Dim iLastAttribute As Integer
    Dim iAttribute As Integer
    Dim lDataSet As atcDataSet

    For Each mnuConstituentAdd As MenuItem In mnuAttributesAdd.MenuItems
      For Each mnuAttribute In mnuConstituentAdd.MenuItems
        RemoveHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click
      Next
      'RemoveHandler mnuConstituentAdd.Click, AddressOf mnuAttributesAdd_Click
    Next

    For Each mnuConstituentRemove As MenuItem In mnuAttributesRemove.MenuItems
      For Each mnuAttribute In mnuConstituentRemove.MenuItems
        RemoveHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click
      Next
      'RemoveHandler mnuConstituentRemove.Click, AddressOf mnuAttributesRemove_Click
    Next

    mnuAttributesRemove.MenuItems.Clear()
    mnuAttributesAdd.MenuItems.Clear()

    mnuConstituentAddInputs = mnuAttributesAdd.MenuItems.Add(MenuText_AllInputs)
    'AddHandler mnuAttributesAdd.Click, AddressOf mnuAttributesRemove_Click
    mnuConstituentAddResults = mnuAttributesAdd.MenuItems.Add(MenuText_AllResults)
    'AddHandler mnuAttributesAdd.Click, AddressOf mnuAttributesRemove_Click

    mnuConstituentRemoveInputs = mnuAttributesRemove.MenuItems.Add(MenuText_AllInputs)
    'AddHandler mnuAttributesAdd.Click, AddressOf mnuAttributesRemove_Click
    mnuConstituentRemoveResults = mnuAttributesRemove.MenuItems.Add(MenuText_AllResults)
    'AddHandler mnuAttributesAdd.Click, AddressOf mnuAttributesRemove_Click

    For Each lDataSet In pBaseScenario.DataSets
      PopulateAddRemoveAttributesMenus(lDataSet, lAddedConstituents, lAttributeRemovable)
    Next

    For Each lDataSet In pBaseResults.DataSets
      PopulateAddRemoveAttributesMenus(lDataSet, lAddedConstituents, lAttributeRemovable)
    Next

    Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
    For Each lAttributeDef As atcAttributeDefinition In lAllDefinitions
      'TODO: any items that are already on for all constituents don't need to be addable
      mnuAttribute = mnuConstituentAddInputs.MenuItems.Add(lAttributeDef.Name)
      AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click

      mnuAttribute = mnuConstituentAddResults.MenuItems.Add(lAttributeDef.Name)
      AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click

      'offer removal of attributes currently in use by any constituent
      If lAttributeRemovable.IndexOf("++" & lAttributeDef.Name & "++") > -1 Then
        mnuAttribute = mnuConstituentRemoveInputs.MenuItems.Add(lAttributeDef.Name)
        AddHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click

        mnuAttribute = mnuConstituentRemoveResults.MenuItems.Add(lAttributeDef.Name)
        AddHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click
      End If
    Next

    If Not pInitializing Then
      agdMain.Refresh()
      agdResults.Refresh()
    End If
    'End If
  End Sub

  Private Sub UserModify(ByVal aBaseTimeseries As atcTimeseries, ByVal aModifiedScenario As atcDataSource)
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
        aModifiedScenario.DataSets.RemoveByKey(lID)
        aModifiedScenario.DataSets.Add(lID, lNewTimeseries)
      Next
    End If
    agdMain.Refresh()
    agdResults.Refresh()

  End Sub

  Private Sub RunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'Copy base UCI and change scenario name within it
    'Copy WDM
    'Change data to be modified in new WDM
    'Change scenario attribute in new WDM
    'Run WinHSPFlt with the new UCI
    'Reload datasets in Results grid

    Dim btn As Windows.Forms.Button = sender

    Dim lModifiedIndex As Integer = 0
    Dim lNewScenarioName As String
    Dim lNewFilename As String
    Dim lNewWDM As atcWDM.atcDataSourceWDM
    Dim lOldWDM As atcWDM.atcDataSourceWDM
    Dim lCurrentWDMfilename As String
    Dim lNewResults As atcDataSource

    For Each lDataSource As atcDataSource In pDataManager.DataSources
      If lDataSource.DataSets.Contains(pBaseScenario.DataSets.ItemByIndex(0)) Then
        lOldWDM = lDataSource
        lCurrentWDMfilename = lDataSource.Specification
      End If
    Next
    If FileExists(lCurrentWDMfilename) Then
      If IsNumeric(btn.Tag) Then lModifiedIndex = btn.Tag

      If lModifiedIndex = -1 Then 'Run base scenario
        lNewFilename = FilenameNoExt(lCurrentWDMfilename) & "."
        lNewWDM = lOldWDM
      Else 'Run a modified scenario
        lNewScenarioName = "modified"
        If lModifiedIndex > 0 Then lNewScenarioName &= (lModifiedIndex + 1)
        lNewFilename = PathNameOnly(lCurrentWDMfilename) & "\" & lNewScenarioName & "."
        'Copy base UCI and change scenario name within it
        ReplaceStringToFile(WholeFileString(FilenameSetExt(lCurrentWDMfilename, "uci")), "base.", lNewScenarioName & ".", lNewFilename & "uci")

        'Copy base WDM to new WDM
        FileCopy(lCurrentWDMfilename, lNewFilename & "wdm")
        lNewWDM = New atcWDM.atcDataSourceWDM
        If Not lNewWDM.Open(lNewFilename & "wdm") Then
          MsgBox("Could not open new scenario WDM file '" & lNewFilename & "wdm'", MsgBoxStyle.Critical, "Could not run model")
          Exit Sub
        End If
        Dim lCurrentTimeseries As atcTimeseries
        For Each lCurrentTimeseries In pModifiedScenarios.ItemByIndex(lModifiedIndex).DataSets
          lCurrentTimeseries.Attributes.SetValue("scenario", lNewScenarioName)
          lNewWDM.AddDataSet(lCurrentTimeseries)
        Next

        For Each lCurrentTimeseries In lNewWDM.DataSets
          If lCurrentTimeseries.Attributes.GetValue("scenario").ToLower = "base" Then
            lCurrentTimeseries.Attributes.SetValue("scenario", lNewScenarioName)
            lNewWDM.AddDataSet(lCurrentTimeseries) 'TODO: Would be nice to just update this attribute, not rewrite all data values
          End If
        Next
      End If

      Shell("D:\BASINS\models\hspf\bin\WinHspfLt.exe " & lNewFilename & "uci", AppWinStyle.NormalFocus, True)

      If lModifiedIndex >= 0 Then 'ran modified scenario
        lNewResults = pModifiedResults.ItemByIndex(lModifiedIndex)
        lNewResults.DataSets.Clear()
      Else 'ran base scenario, can't clear so just use new group
        lNewResults = New atcDataSource
      End If

      For Each lModifiedData As atcDataSet In lNewWDM.DataSets
        If pBaseResults.DataSets.Keys.Contains(lModifiedData.Attributes.GetValue("id")) Then
          'Found data matching one of the base results
          'Re-read this dataset since it has just been written by HSPF

          'First, discard any calculated attributes
          Dim lRemoveThese As New ArrayList
          'Step in reverse so we can remove by index in next loop without high indexes changing before they are removed
          For iAttribute As Integer = lModifiedData.Attributes.Count - 1 To 0 Step -1
            If lModifiedData.Attributes.ItemByIndex(iAttribute).Definition.Calculated Then
              lRemoveThese.Add(iAttribute)
            End If
          Next
          For Each iAttribute As Integer In lRemoveThese
            lModifiedData.Attributes.RemoveAt(iAttribute)
          Next
          lModifiedData.Attributes.SetValue("HeaderComplete", False)
          lModifiedData.Attributes.SetValue("HeaderOnly", False)
          lNewWDM.ReadData(lModifiedData)

          lNewResults.DataSets.Add(lModifiedData)
        End If
      Next
      agdResults.Refresh()
    Else
      MsgBox("Could not find base WDM file '" & lCurrentWDMfilename & "'", MsgBoxStyle.Critical, "Could not run model")
    End If
    Dim lNewWDMfile As atcDataSource

  End Sub

  Private Sub agdMain_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles agdMain.UserResizedColumn
    agdResults.ColumnWidth(aColumn) = aWidth
    agdResults.Refresh()
    PositionRunButtons()
  End Sub

  Private Sub agdResults_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles agdResults.UserResizedColumn
    agdMain.ColumnWidth(aColumn) = aWidth
    agdMain.Refresh()
    PositionRunButtons()
  End Sub

  Private Sub PositionRunButtons()
    Dim iLastButton As Integer = pRunButton.GetUpperBound(0)
    Dim iButton As Integer

    For iButton = 0 To iLastButton
      With pRunButton(iButton)
        .Visible = False
        .Top = (panelMiddle.Height - .Height) / 2
        .Left = 0
      End With
    Next

    For iColumn As Integer = 0 To pSource.Columns - 2
      For iButton = 0 To iLastButton
        If iButton > iColumn - 2 Then
          pRunButton(iButton).Left += agdResults.ColumnWidth(iColumn)
        End If
      Next
    Next

    For iButton = 0 To iLastButton
      pRunButton(iButton).Visible = True
    Next
  End Sub

  Private Sub mnuScenariosAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScenariosAdd.Click
    AddScenario()
  End Sub

  Private Sub AddScenario()
    Dim lNewModified As New atcDataSource
    Dim lNewResults As New atcDataSource

    pModifiedScenarios.Add(lNewModified)
    pModifiedResults.Add(lNewResults)

    lNewModified.Specification = "Modified_" & pModifiedScenarios.Count
    lNewResults.Specification = "Modified_" & pModifiedResults.Count

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

    Dim lNewButtonIndex As Integer = pRunButton.GetUpperBound(0) + 1
    ReDim Preserve pRunButton(lNewButtonIndex)
    pRunButton(lNewButtonIndex) = New Windows.Forms.Button
    panelMiddle.Controls.Add(pRunButton(lNewButtonIndex))
    AddHandler pRunButton(lNewButtonIndex).Click, AddressOf RunButton_Click
    With pRunButton(lNewButtonIndex)
      .Tag = lNewButtonIndex - 1
      .Name = "Run" & .Tag
      .Text = "Run" ' & .Tag
      .Width = pRunButton(0).Width
    End With
    PositionRunButtons()
  End Sub
End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const pLabelRow As Integer = 0

  Private pBaseScenario As atcDataSource
  Private pModifiedScenarios As atcCollection

  Sub New(ByVal aBaseScenario As atcDataSource, _
          ByVal aModifiedScenarios As atcCollection)
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
      If pBaseScenario Is Nothing Then
        Return pLabelRow + 1
      Else
        Dim lRows As Integer = pLabelRow + 1
        For Each lDataSet As atcDataSet In pBaseScenario.DataSets
          Dim lScenarioAttributes As atcDataAttributes = lDataSet.Attributes.GetValue("Scenario Attributes", Nothing)
          If lScenarioAttributes Is Nothing Then
            lRows += 3
          Else
            lRows += lScenarioAttributes.Count
          End If
        Next
        Return lRows
      End If
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Function BaseDataSetInRow(ByVal aRow As Integer, ByRef aAttributeIndex As Integer) As atcDataSet
    Dim lRow As Integer = pLabelRow + 1
    For Each lDataSet As atcDataSet In pBaseScenario.DataSets
      Dim lScenarioAttributes As atcDataAttributes = lDataSet.Attributes.GetValue("Scenario Attributes", Nothing)
      If lScenarioAttributes Is Nothing Then
        lScenarioAttributes = New atcDataAttributes
        lScenarioAttributes.Add("Min", "")
        lScenarioAttributes.Add("Max", "")
        lScenarioAttributes.Add("Mean", "")
        lDataSet.Attributes.SetValue("Scenario Attributes", lScenarioAttributes)
      End If
      If lRow + lScenarioAttributes.Count > aRow Then
        aAttributeIndex = aRow - lRow
        Return lDataSet
      End If
      lRow += lScenarioAttributes.Count
    Next
    Return Nothing
  End Function

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = pLabelRow Then
        Select Case aColumn
          Case 0
            Return "Constituent"
          Case 1
            Return "Attribute"
          Case 2
            Return "Base"
          Case Is > 2
            Dim lModifiedSource As atcDataSource = pModifiedScenarios.ItemByIndex(aColumn - 3)
            Return lModifiedSource.Specification
        End Select
      Else
        Dim lAttributeIndex As Integer
        Dim lDataSet As atcDataSet = BaseDataSetInRow(aRow, lAttributeIndex)
        Dim lAttributeName As String = ""
        Try 'If we fail to find a name, it will stay blank as assigned above
          lAttributeName = lDataSet.Attributes.GetValue("Scenario Attributes").ItemByIndex(lAttributeIndex).Definition.Name()
        Catch
        End Try
        Select Case aColumn
          Case 0
            If lAttributeIndex = 0 Then
              Return lDataSet.Attributes.GetFormattedValue("Constituent")
            Else
              Return ""
            End If
          Case 1
            Return lAttributeName
          Case 2
            Return lDataSet.Attributes.GetFormattedValue(lAttributeName)
          Case Is > 2
            Dim lModifiedSource As atcDataSource = pModifiedScenarios.ItemByIndex(aColumn - 3)
            Dim lModifiedGroup As atcDataGroup = lModifiedSource.DataSets
            Dim lBaseID As String = lDataSet.Attributes.GetValue("id")
            For Each lModifiedData As atcDataSet In lModifiedGroup
              If lModifiedData.Attributes.GetValue("id") = lBaseID Then 'Found modified dataset for this cell
                'If aColumn Mod 2 = 1 Then 'odd columns contain relationships
                'Return lModifiedData.Attributes.GetValue("History 1")
                'Else 'Even columns contain values
                Dim lNewValue As Object = lModifiedData.Attributes.GetValue(lAttributeName)
                If IsNumeric(lNewValue) Then 'See if we can provide %difference from base
                  Dim lOldValue As Object = lDataSet.Attributes.GetValue(lAttributeName)
                  If IsNumeric(lOldValue) Then
                    Dim lPercentDifference As Double = (lNewValue - lOldValue) / lOldValue
                    If Not Double.IsNaN(lPercentDifference) AndAlso _
                       Not Double.IsInfinity(lPercentDifference) AndAlso _
                       Math.Abs(lPercentDifference) > 0.005 Then
                      Return lModifiedData.Attributes.GetFormattedValue(lAttributeName) & vbTab & " (" _
                           & Format(lPercentDifference, "+#,##0.##%;-#,##0.##%") & ")"
                    End If
                  End If
                End If
                Return lModifiedData.Attributes.GetFormattedValue(lAttributeName)
              End If
            Next
        End Select
        Return "" '    "(click to modify)"
      End If
    End Get
    Set(ByVal newValue As String)
    End Set
  End Property

  Public Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      '      If aRow = pLabelRow Then
      'Return atcControls.atcAlignment.HAlignLeft
      '     Else
      Select Case aColumn
        Case 0
          Return atcControls.atcAlignment.HAlignLeft
        Case 1
          Return atcControls.atcAlignment.HAlignLeft
        Case Else
          Return atcControls.atcAlignment.HAlignDecimal
      End Select
      '      End If
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property

End Class