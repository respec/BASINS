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
  Friend WithEvents mnuScenariosAddFromScript As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopyInputs As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopyResults As System.Windows.Forms.MenuItem
  Friend WithEvents mnuEditCopyBoth As System.Windows.Forms.MenuItem
  Friend WithEvents mnuScenariosAddFromBuiltInScript As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcScenarioBuilderForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuFileAddResults = New System.Windows.Forms.MenuItem
    Me.mnuEdit = New System.Windows.Forms.MenuItem
    Me.mnuEditCopyBoth = New System.Windows.Forms.MenuItem
    Me.mnuEditCopyInputs = New System.Windows.Forms.MenuItem
    Me.mnuEditCopyResults = New System.Windows.Forms.MenuItem
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.mnuDisplay = New System.Windows.Forms.MenuItem
    Me.mnuScenarios = New System.Windows.Forms.MenuItem
    Me.mnuScenariosAdd = New System.Windows.Forms.MenuItem
    Me.mnuScenariosAddFromScript = New System.Windows.Forms.MenuItem
    Me.agdMain = New atcControls.atcGrid
    Me.splitHoriz = New System.Windows.Forms.Splitter
    Me.panelMiddle = New System.Windows.Forms.Panel
    Me.agdResults = New atcControls.atcGrid
    Me.mnuScenariosAddFromBuiltInScript = New System.Windows.Forms.MenuItem
    Me.SuspendLayout()
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuAttributes, Me.mnuDisplay, Me.mnuScenarios})
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
    'mnuFileAddResults
    '
    Me.mnuFileAddResults.Index = 1
    Me.mnuFileAddResults.Text = "Add Results"
    '
    'mnuEdit
    '
    Me.mnuEdit.Index = 1
    Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopyBoth, Me.mnuEditCopyInputs, Me.mnuEditCopyResults})
    Me.mnuEdit.Text = "&Edit"
    '
    'mnuEditCopyBoth
    '
    Me.mnuEditCopyBoth.Index = 0
    Me.mnuEditCopyBoth.Text = "&Copy Both"
    '
    'mnuEditCopyInputs
    '
    Me.mnuEditCopyInputs.Index = 1
    Me.mnuEditCopyInputs.Text = "Copy Inputs"
    '
    'mnuEditCopyResults
    '
    Me.mnuEditCopyResults.Index = 2
    Me.mnuEditCopyResults.Text = "Copy Results"
    '
    'mnuAttributes
    '
    Me.mnuAttributes.Index = 2
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
    Me.mnuDisplay.Index = 3
    Me.mnuDisplay.Text = "&Display"
    '
    'mnuScenarios
    '
    Me.mnuScenarios.Index = 4
    Me.mnuScenarios.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuScenariosAdd, Me.mnuScenariosAddFromScript, Me.mnuScenariosAddFromBuiltInScript})
    Me.mnuScenarios.Text = "&Scenarios"
    '
    'mnuScenariosAdd
    '
    Me.mnuScenariosAdd.Index = 0
    Me.mnuScenariosAdd.Text = "&Add"
    '
    'mnuScenariosAddFromScript
    '
    Me.mnuScenariosAddFromScript.Index = 1
    Me.mnuScenariosAddFromScript.Text = "Add From Script"
    '
    'agdMain
    '
    Me.agdMain.AllowHorizontalScrolling = True
    Me.agdMain.Dock = System.Windows.Forms.DockStyle.Top
    Me.agdMain.LineColor = System.Drawing.Color.Empty
    Me.agdMain.LineWidth = 0.0!
    Me.agdMain.Location = New System.Drawing.Point(0, 0)
    Me.agdMain.Name = "agdMain"
    Me.agdMain.Size = New System.Drawing.Size(535, 249)
    Me.agdMain.Source = Nothing
    Me.agdMain.TabIndex = 0
    '
    'splitHoriz
    '
    Me.splitHoriz.BackColor = System.Drawing.SystemColors.ControlDark
    Me.splitHoriz.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitHoriz.Location = New System.Drawing.Point(0, 249)
    Me.splitHoriz.Name = "splitHoriz"
    Me.splitHoriz.Size = New System.Drawing.Size(535, 8)
    Me.splitHoriz.TabIndex = 1
    Me.splitHoriz.TabStop = False
    '
    'panelMiddle
    '
    Me.panelMiddle.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelMiddle.Location = New System.Drawing.Point(0, 257)
    Me.panelMiddle.Name = "panelMiddle"
    Me.panelMiddle.Size = New System.Drawing.Size(535, 47)
    Me.panelMiddle.TabIndex = 2
    '
    'agdResults
    '
    Me.agdResults.AllowHorizontalScrolling = True
    Me.agdResults.Dock = System.Windows.Forms.DockStyle.Fill
    Me.agdResults.LineColor = System.Drawing.Color.Empty
    Me.agdResults.LineWidth = 0.0!
    Me.agdResults.Location = New System.Drawing.Point(0, 304)
    Me.agdResults.Name = "agdResults"
    Me.agdResults.Size = New System.Drawing.Size(535, 200)
    Me.agdResults.Source = Nothing
    Me.agdResults.TabIndex = 3
    '
    'mnuScenariosAddFromBuiltInScript
    '
    Me.mnuScenariosAddFromBuiltInScript.Index = 2
    Me.mnuScenariosAddFromBuiltInScript.Text = "Add From Built-In Script"
    '
    'atcScenarioBuilderForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(535, 504)
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
  Private pMapWin As MapWindow.Interfaces.IMapWin

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
                        ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
               Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    pInitializing = True
    pDataManager = aDataManager
    pMapWin = aMapWin
    pBaseScenario = New atcDataSource
    If Not aTimeseriesGroup Is Nothing Then
      pBaseScenario.DataSets.AddRange(aTimeseriesGroup) 'TODO: want to share events with aTimeseriesGroup
    End If

    mnuDisplay.MenuItems.Clear()
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each lDisp As atcDataDisplay In DisplayPlugins
      Dim lMenuDispType As MenuItem = mnuDisplay.MenuItems.Add(lDisp.Name, New EventHandler(AddressOf mnuDisplay_Click))
      lMenuDispType.MenuItems.Add("All Inputs", New EventHandler(AddressOf mnuDisplay_Click))
      lMenuDispType.MenuItems.Add("Base Inputs", New EventHandler(AddressOf mnuDisplay_Click))
      lMenuDispType.MenuItems.Add("Modified Inputs", New EventHandler(AddressOf mnuDisplay_Click))

      lMenuDispType.MenuItems.Add("-")

      lMenuDispType.MenuItems.Add("All Results", New EventHandler(AddressOf mnuDisplay_Click))
      lMenuDispType.MenuItems.Add("Base Results", New EventHandler(AddressOf mnuDisplay_Click))
      lMenuDispType.MenuItems.Add("Modified Results", New EventHandler(AddressOf mnuDisplay_Click))
    Next

    If pBaseScenario.DataSets.Count = 0 Then
      'By default add EVAP and PREC if none were given
      For Each lDataSet As atcDataSet In pDataManager.DataSets
        Select Case lDataSet.Attributes.GetValue("Constituent")
          Case "EVAP", "PREC", "TMIN", "TMAX", "PET", "HPRECIP" : pBaseScenario.DataSets.Add(lDataSet)
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
        Dim lScenario As atcDataSource
        Dim lScenarioName As String = lDataSet.Attributes.GetValue("Scenario")
        If lScenarioName.StartsWith("Modified") Then
          If pModifiedScenarios.Keys.Contains(lScenarioName) Then
            lScenario = pModifiedScenarios.ItemByKey(lScenarioName)
          Else
            AddScenario(lScenarioName)
            lScenario = pModifiedScenarios.ItemByIndex(pModifiedScenarios.Count - 1)
          End If
          If Not lScenario Is Nothing AndAlso Not lScenario.DataSets.Contains(lDataSet) Then
            lScenario.AddDataSet(lDataSet)
          End If
        End If
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

    If pInitializing Then
      Dim lMaxGridHeight As Integer
      lMaxGridHeight = (pSource.Rows + 1) * agdMain.RowHeight(0)
      If lMaxGridHeight < agdMain.Height Then
        agdMain.Height = lMaxGridHeight
        'splitHoriz.Top = agdMain.Top + lMaxGridHeight
      End If
      lMaxGridHeight = (pResultSource.Rows + 3) * agdResults.RowHeight(0)
      If lMaxGridHeight < agdResults.Height Then
        Me.Height = agdResults.Top + lMaxGridHeight
      End If
    Else
      agdMain.Refresh()
      agdResults.Refresh()
    End If
  End Sub

  Private Sub mnuEditCopyBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopyBoth.Click
    Clipboard.SetDataObject(pSource.ToString & vbCrLf & pResultSource.ToString)
  End Sub

  Private Sub mnuEditCopyInputs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopyInputs.Click
    Clipboard.SetDataObject(pSource.ToString)
  End Sub

  Private Sub mnuEditCopyResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopyResults.Click
    Clipboard.SetDataObject(pResultSource.ToString)
  End Sub

  Private Sub mnuDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDisplay.Click
    Dim lMenuClicked As MenuItem = sender
    Dim lNewDisplay As atcDataDisplay
    Dim lDisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    Dim lShowBaseInputs As Boolean = False
    Dim lShowModifiedInputs As Boolean = False
    Dim lShowBaseResults As Boolean = False
    Dim lShowModifiedResults As Boolean = False
    Dim lShowThese As New atcDataGroup
    Dim lScenario As atcDataSource
    Dim lDataSet As atcDataSet

    Select Case lMenuClicked.Text
      Case "All Inputs"
        lShowBaseInputs = True
        lShowModifiedInputs = True
      Case "Base Inputs" : lShowBaseInputs = True
      Case "Modified Inputs" : lShowModifiedInputs = True
      Case "All Results"
        lShowBaseResults = True
        lShowModifiedResults = True
      Case "Base Results" : lShowBaseResults = True
      Case "Modified Results" : lShowModifiedResults = True
      Case Else
        lShowBaseInputs = True
        lShowModifiedInputs = True
        lShowBaseResults = True
        lShowModifiedResults = True
        For Each atf As atcDataDisplay In lDisplayPlugins
          If atf.Name = lMenuClicked.Text Then
            Dim typ As System.Type = atf.GetType()
            Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
            lNewDisplay = asm.CreateInstance(typ.FullName)
          End If
        Next
    End Select

    If lNewDisplay Is Nothing Then
      Dim mnuDisplayType As MenuItem = lMenuClicked.Parent
      For Each atf As atcDataDisplay In lDisplayPlugins
        If atf.Name = mnuDisplayType.Text Then
          Dim typ As System.Type = atf.GetType()
          Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
          lNewDisplay = asm.CreateInstance(typ.FullName)
        End If
      Next
    End If

    If Not lNewDisplay Is Nothing Then
      If lShowBaseInputs Then
        For Each lDataSet In pBaseScenario.DataSets
          lShowThese.Add(lDataSet)
        Next
      End If
      If lShowModifiedInputs Then
        For Each lScenario In pModifiedScenarios
          For Each lDataSet In lScenario.DataSets
            lShowThese.Add(lDataSet)
          Next
        Next
      End If

      If lShowBaseResults Then
        For Each lDataSet In pBaseResults.DataSets
          lShowThese.Add(lDataSet)
        Next
      End If

      If lShowModifiedResults Then
        For Each lScenario In pModifiedResults
          For Each lDataSet In lScenario.DataSets
            lShowThese.Add(lDataSet)
          Next
        Next
      End If

      lNewDisplay.Show(pDataManager, lShowThese)
    End If
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pBaseScenario.DataSets, False)
    UpdatedAttributes()
  End Sub

  Private Sub mnuFileAddResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAddResults.Click
    pDataManager.UserSelectData(, pBaseResults.DataSets, False)
    UpdatedAttributes()
  End Sub

  Private Sub mnuAttributesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnuAttribute As MenuItem = sender
    Dim mnuText As String = mnuAttribute.Text.Trim
    Dim mnuConstituent As MenuItem = mnuAttribute.Parent
    Dim allInputs As Boolean = mnuConstituent.Text.Equals(MenuText_AllInputs)
    Dim allResults As Boolean = mnuConstituent.Text.Equals(MenuText_AllResults)
    Dim lNeedMainRefresh As Boolean = False
    Dim lNeedResultsRefresh As Boolean = False
    Dim lFromScript As Boolean = mnuText.Equals("From Script...")
    'MsgBox("Add " & mnuConstituent.Text, MsgBoxStyle.Information, mnuAttribute.Text)

    If lFromScript Then 'Add attribute(s) from a script
      Dim lScriptFileName As String = FindFile("Please locate attribute script to run", "", "vb", "VB.net Files (*.vb)|*.vb|All files (*.*)|*.*", True)

      'Save the set of all known attribute definitions before running the script
      Dim lOldAttributes As atcCollection = atcDataAttributes.AllDefinitions.Clone
      If allInputs Then
        AddAttributeFromScript(pBaseScenario.DataSets, lScriptFileName)
        For Each lModifiedScenario As atcDataSource In pModifiedScenarios
          AddAttributeFromScript(lModifiedScenario.DataSets, lScriptFileName)
        Next

        For Each lNewAttribute As atcAttributeDefinition In atcDataAttributes.AllDefinitions
          'Add newly created attributes to the list to display in the top grid
          If Not lOldAttributes.Contains(lNewAttribute) Then
            For Each lDataSet As atcDataSet In pBaseScenario.DataSets
              GetScenarioAttributes(lDataSet).SetValue(lNewAttribute.Name, "")
            Next
            lNeedMainRefresh = True
          End If
        Next
      End If

      If allResults Then
        AddAttributeFromScript(pBaseResults.DataSets, lScriptFileName)
        For Each lModifiedScenario As atcDataSource In pModifiedResults
          AddAttributeFromScript(lModifiedScenario.DataSets, lScriptFileName)
        Next
        For Each lNewAttribute As atcAttributeDefinition In atcDataAttributes.AllDefinitions
          'Add newly created attributes to the list to display in the bottom grid
          If Not lOldAttributes.Contains(lNewAttribute) Then
            For Each lDataSet As atcDataSet In pBaseResults.DataSets
              GetScenarioAttributes(lDataSet).SetValue(lNewAttribute.Name, "")
              lNeedResultsRefresh = True
            Next
          End If
        Next
      End If

    Else 'Add a regular attribute

      For Each lDataSet As atcDataSet In pBaseScenario.DataSets
        If allInputs OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
          GetScenarioAttributes(lDataSet).SetValue(mnuText, "")
          lNeedMainRefresh = True
        End If
      Next
      For Each lDataSet As atcDataSet In pBaseResults.DataSets
        If allResults OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
          GetScenarioAttributes(lDataSet).SetValue(mnuText, "")
          lNeedResultsRefresh = True
        End If
      Next
    End If
    If lNeedMainRefresh Then agdMain.Refresh()
    If lNeedResultsRefresh Then agdResults.Refresh()
    UpdatedAttributes()
  End Sub

  Private Sub AddAttributeFromScript(ByVal aDatasets As atcDataGroup, ByVal aScriptFileName As String)
    If FileExists(aScriptFileName) Then
      Dim args() As Object = New Object() {aDatasets}
      Dim errors As String
      'Dim lBasinsPlugin As Object = pDataManager.Basins
      'lBasinsPlugin.RunBasinsScript(FileExt(aScriptFileName), WholeFileString(aScriptFileName), errors, args)
      If Not errors Is Nothing Then
        LogMsg(aScriptFileName & vbCrLf & vbCrLf & errors, "Attribute Script Error")
      End If
    Else
      LogMsg("Unable to find script " & aScriptFileName, "Attribute Script")
    End If
  End Sub

  Private Sub mnuAttributesRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnuAttribute As MenuItem = sender
    Dim mnuText As String = mnuAttribute.Text.Trim
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
          lAttributes.RemoveByKey(mnuText.ToLower)
          lNeedMainRefresh = True
        End If
      End If
    Next

    For Each lDataSet As atcDataSet In pBaseResults.DataSets
      If allResults OrElse lDataSet.Attributes.GetValue("constituent") = mnuConstituent.Text Then
        Dim lAttributes As atcDataAttributes = lDataSet.Attributes.GetValue("Scenario Attributes")
        If Not lAttributes Is Nothing Then
          lAttributes.RemoveByKey(mnuText.ToLower)
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

  Private Sub agdMain_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMain.MouseDownCell
    If aRow > 0 AndAlso aColumn > 2 Then
      Dim pAttributeIndex As Integer
      Dim lBaseDataSet As atcDataSet = pSource.BaseDataSetInRow(aRow, pAttributeIndex) '(aRow - 1) \ pDataManager.DisplayAttributes.Count
      UserModify(lBaseDataSet, pModifiedScenarios.ItemByIndex(aColumn - 3))
    End If
  End Sub

  Public Shared Function GetScenarioAttributes(ByVal aDataSet As atcDataSet) As atcDataAttributes
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

  Private Sub PopulateAddRemoveAttributesMenus(ByVal aDataSet As atcDataSet, _
                                               ByRef aAddedConstituents As String, _
                                               ByRef lAttributeRemovable As String)
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

      'Add items available to add to a sorted, categorized list
      Dim lAddToAdd As New SortedList
      Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
      For Each lAttributeDef As atcAttributeDefinition In lAllDefinitions
        If lAddedAttributes.IndexOf("++" & lAttributeDef.Name & "++") = -1 AndAlso _
          atcDataAttributes.IsSimple(lAttributeDef) Then
          Dim lCategory As SortedList
          If lAddToAdd.ContainsKey(lAttributeDef.Category) Then
            lCategory = lAddToAdd.Item(lAttributeDef.Category)
          Else
            lCategory = New SortedList
            lAddToAdd.Add(lAttributeDef.Category, lCategory)
          End If
          lCategory.Add(lAttributeDef.Name, lAttributeDef.Name)
        End If
      Next
      'Add sorted items to menu
      For Each lCategoryEntry As DictionaryEntry In lAddToAdd
        If mnuConstituentAdd.MenuItems.Count > 0 Then
          mnuConstituentAdd.MenuItems.Add("-")
        End If
        If Len(lCategoryEntry.Key) > 0 Then
          mnuConstituentAdd.MenuItems.Add(lCategoryEntry.Key)
        End If
        Dim lCategory As SortedList = lCategoryEntry.Value
        For Each lAttributeEntry As DictionaryEntry In lCategory
          mnuAttribute = mnuConstituentAdd.MenuItems.Add("  " & lAttributeEntry.Value)
          AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click
        Next
      Next
      lAttributeRemovable &= lAddedAttributes
    End If

  End Sub

  Private Sub UpdatedAttributes()
    'If Not pInitializing Then
    Dim lAddedConstituents As String = "++"
    Dim lAttributeRemovable As String = "++"
    Dim lAttributeEntry As DictionaryEntry
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

    Dim lAddToAdd As New SortedList
    Dim lAddToRemove As New SortedList

    Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
    For Each lAttributeDef As atcAttributeDefinition In lAllDefinitions
      If atcDataAttributes.IsSimple(lAttributeDef) Then
        Dim lCategory As SortedList
        If lAddToAdd.ContainsKey(lAttributeDef.Category) Then
          lCategory = lAddToAdd.Item(lAttributeDef.Category)
        Else
          lCategory = New SortedList
          lAddToAdd.Add(lAttributeDef.Category, lCategory)
        End If
        lCategory.Add(lAttributeDef.Name, lAttributeDef.Name)
      End If

      'offer removal of attributes currently in use by any constituent
      If lAttributeRemovable.IndexOf("++" & lAttributeDef.Name & "++") > -1 Then
        lAddToRemove.Add(lAttributeDef.Name, lAttributeDef.Name)
      End If
    Next

    For Each lCategoryEntry As DictionaryEntry In lAddToAdd
      If mnuConstituentAddInputs.MenuItems.Count > 0 Then
        mnuConstituentAddInputs.MenuItems.Add("-")
        mnuConstituentAddResults.MenuItems.Add("-")
      End If
      If Len(lCategoryEntry.Key) > 0 Then
        mnuConstituentAddInputs.MenuItems.Add(lCategoryEntry.Key)
        mnuConstituentAddResults.MenuItems.Add(lCategoryEntry.Key)
      End If
      Dim lCategory As SortedList = lCategoryEntry.Value
      For Each lAttributeEntry In lCategory
        mnuAttribute = mnuConstituentAddInputs.MenuItems.Add("  " & lAttributeEntry.Value)
        AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click

        mnuAttribute = mnuConstituentAddResults.MenuItems.Add("  " & lAttributeEntry.Value)
        AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click
      Next
    Next

    mnuAttribute = mnuConstituentAddInputs.MenuItems.Add("From Script...")
    AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click

    mnuAttribute = mnuConstituentAddResults.MenuItems.Add("From Script...")
    AddHandler mnuAttribute.Click, AddressOf mnuAttributesAdd_Click

    For Each lAttributeEntry In lAddToRemove
      mnuAttribute = mnuConstituentRemoveInputs.MenuItems.Add(lAttributeEntry.Value)
      AddHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click

      mnuAttribute = mnuConstituentRemoveResults.MenuItems.Add(lAttributeEntry.Value)
      AddHandler mnuAttribute.Click, AddressOf mnuAttributesRemove_Click
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
        lNewWDM = lOldWDM
        ScenarioRun(lCurrentWDMfilename, "base", Nothing)
      Else 'Run a modified scenario
        Dim lModifiedScenario As atcDataSource = pModifiedScenarios.ItemByIndex(lModifiedIndex)
        lNewWDM = ScenarioRun(lCurrentWDMfilename, _
                              lModifiedScenario.Specification, _
                              lModifiedScenario.DataSets)
      End If

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
    Try
      Dim iLastButton As Integer = pRunButton.GetUpperBound(0)
      Dim iButton As Integer

      For iButton = 0 To iLastButton
        With pRunButton(iButton)
          .Visible = False
          .Top = (panelMiddle.Height - .Height) / 2
          .Left = 0
        End With
      Next

      For iColumn As Integer = 0 To pSource.Columns - 1
        For iButton = 0 To iLastButton
          If iButton > iColumn - 2 Then
            pRunButton(iButton).Left += agdResults.ColumnWidth(iColumn)
          ElseIf iButton = iColumn - 2 Then
            pRunButton(iButton).Left += (agdResults.ColumnWidth(iColumn) - pRunButton(iButton).Width) / 2
          End If
        Next
      Next

      For iButton = 0 To iLastButton
        pRunButton(iButton).Visible = True
      Next
    Catch 'quietly skip repositioning the buttons if there is a problem (e.g. can't do before buttons are created)
    End Try
  End Sub

  Private Sub mnuScenariosAddFromScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScenariosAddFromScript.Click
    Dim lScriptFileName As String = FindFile("Please locate scenario builder script to run", "", "vb", "VB.net Files (*.vb)|*.vb|All files (*.*)|*.*", True)
    If FileExists(lScriptFileName) Then
      Dim lNewScenario As atcDataSource = pModifiedScenarios.ItemByIndex(pModifiedScenarios.Count - 1)
      If lNewScenario.DataSets.Count > 0 Then 'This scenario is already in use
        AddScenario()                         'Create a new scenario to populate
        lNewScenario = pModifiedScenarios.ItemByIndex(pModifiedScenarios.Count - 1)
      End If
      Dim errors As String
      RunScript(FileExt(lScriptFileName), MakeScriptName, lScriptFileName, errors, pDataManager, _
                pBaseScenario, lNewScenario)
      If Not errors Is Nothing Then
        LogMsg(lScriptFileName & vbCrLf & vbCrLf & errors, "Scenario Script Error")
      End If
    Else
      LogMsg("Unable to find script " & lScriptFileName, "Scenario Script")
    End If
    agdMain.Refresh()
  End Sub

  Private Sub mnuScenariosAddFromBuiltInScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScenariosAddFromBuiltInScript.Click
    Dim lNewScenario As atcDataSource = pModifiedScenarios.ItemByIndex(pModifiedScenarios.Count - 1)
    If lNewScenario.DataSets.Count > 0 Then 'This scenario is already in use
      AddScenario()                         'Create a new scenario to populate
      lNewScenario = pModifiedScenarios.ItemByIndex(pModifiedScenarios.Count - 1)
    End If
    atcScriptTest.Main(pDataManager, pBaseScenario, lNewScenario)
    agdMain.Refresh()
  End Sub

  Private Function MakeScriptName() As String
    Dim tryName As String
    Dim iTry As Integer = 1

    Do
      tryName = pMapWin.Plugins.PluginFolder & _
                "\Basins\RemoveMe-Script-" & iTry & ".dll"
      iTry += 1
    Loop While FileExists(tryName)
    Return tryName
  End Function

  Private Sub mnuScenariosAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuScenariosAdd.Click
    AddScenario()
  End Sub

  Private Sub AddScenario(Optional ByVal aScenarioName As String = "")
    Dim lNewModified As New atcDataSource
    Dim lNewResults As New atcDataSource

    If aScenarioName.Length = 0 Then aScenarioName = "Modified_" & pModifiedScenarios.Count

    pModifiedScenarios.Add(aScenarioName, lNewModified)
    pModifiedResults.Add(aScenarioName, lNewResults)

    lNewModified.Specification = aScenarioName
    lNewResults.Specification = aScenarioName

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

  Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
    MyBase.OnResize(e)
    PositionRunButtons()
  End Sub

  Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    pDataManager = Nothing
    pBaseScenario = Nothing
    pModifiedScenarios = Nothing
    pBaseResults = Nothing
    pModifiedResults = Nothing
    pSource = Nothing
    pResultSource = Nothing
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

  Protected Overrides Property ProtectedColumns() As Integer
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

  Protected Overrides Property ProtectedRows() As Integer
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
      Dim lScenarioAttributes As atcDataAttributes = atcScenarioBuilderForm.GetScenarioAttributes(lDataSet)
      If lRow + lScenarioAttributes.Count > aRow Then
        aAttributeIndex = aRow - lRow
        Return lDataSet
      End If
      lRow += lScenarioAttributes.Count
    Next
    Return Nothing
  End Function

  Protected Overrides Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      Select Case aRow
        Case Is >= Rows
          Return ""
        Case pLabelRow
          Select Case aColumn
            Case 0
              Return "Constituent"
            Case 1
              Return "Attribute"
            Case 2
              Return "Base"
            Case Is >= Columns
              Return ""
            Case Is > 2
              Dim lModifiedSource As atcDataSource = pModifiedScenarios.ItemByIndex(aColumn - 3)
              Return lModifiedSource.Specification
          End Select
        Case Is > 0
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
            Case Is >= Columns
              Return ""
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
                      Dim lPercentDifference As Double = (lNewValue - lOldValue) * 100 / lOldValue
                      If Not Double.IsNaN(lPercentDifference) AndAlso _
                         Not Double.IsInfinity(lPercentDifference) AndAlso _
                         Math.Abs(lPercentDifference) > 0.005 Then
                        Dim lPercentDifferenceString As String = DoubleToString(lPercentDifference, 5, "+#,##0.##;-#,##0.##", , "", 2)
                        If lPercentDifferenceString.Length > 0 Then
                          Return lModifiedData.Attributes.GetFormattedValue(lAttributeName) & vbTab & " (" _
                              & lPercentDifferenceString & "%)"
                        Else
                          Return lModifiedData.Attributes.GetFormattedValue(lAttributeName)
                        End If
                      End If
                    End If
                  End If
                  Return lModifiedData.Attributes.GetFormattedValue(lAttributeName)
                End If
              Next
          End Select
          Return "" '    "(click to modify)"
      End Select
    End Get
    Set(ByVal newValue As String)
    End Set
  End Property

  Protected Overrides Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
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