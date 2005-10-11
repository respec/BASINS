Imports atcControls
Imports atcUtility

Imports System.Windows.Forms

Friend Class frmSelectData
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    InitializeComponent()

    pMatchingGrid.AllowHorizontalScrolling = False
    pSelectedGrid.AllowHorizontalScrolling = False

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
  Friend WithEvents groupTop As System.Windows.Forms.GroupBox
  Friend WithEvents pnlButtons As System.Windows.Forms.Panel
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  Friend WithEvents splitAboveSelected As System.Windows.Forms.Splitter
  Friend WithEvents groupSelected As System.Windows.Forms.GroupBox
  Friend WithEvents panelCriteria As System.Windows.Forms.Panel
  Friend WithEvents splitAboveMatching As System.Windows.Forms.Splitter
  Friend WithEvents lblMatching As System.Windows.Forms.Label
  Friend WithEvents pMatchingGrid As atcControls.atcGrid
  Friend WithEvents pSelectedGrid As atcControls.atcGrid
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributes As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelect As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesRemove As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAttributesMove As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAllMatching As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectClear As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectNoMatching As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileManage As System.Windows.Forms.MenuItem
  Friend WithEvents mnuAddData As System.Windows.Forms.MenuItem
  Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectData))
    Me.groupTop = New System.Windows.Forms.GroupBox
    Me.pMatchingGrid = New atcControls.atcGrid
    Me.lblMatching = New System.Windows.Forms.Label
    Me.splitAboveMatching = New System.Windows.Forms.Splitter
    Me.panelCriteria = New System.Windows.Forms.Panel
    Me.pnlButtons = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.splitAboveSelected = New System.Windows.Forms.Splitter
    Me.groupSelected = New System.Windows.Forms.GroupBox
    Me.pSelectedGrid = New atcControls.atcGrid
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.MenuItem1 = New System.Windows.Forms.MenuItem
    Me.mnuAddData = New System.Windows.Forms.MenuItem
    Me.mnuFileManage = New System.Windows.Forms.MenuItem
    Me.mnuAttributes = New System.Windows.Forms.MenuItem
    Me.mnuAttributesAdd = New System.Windows.Forms.MenuItem
    Me.mnuAttributesRemove = New System.Windows.Forms.MenuItem
    Me.mnuAttributesMove = New System.Windows.Forms.MenuItem
    Me.mnuSelect = New System.Windows.Forms.MenuItem
    Me.mnuSelectAll = New System.Windows.Forms.MenuItem
    Me.mnuSelectClear = New System.Windows.Forms.MenuItem
    Me.mnuSelectAllMatching = New System.Windows.Forms.MenuItem
    Me.mnuSelectNoMatching = New System.Windows.Forms.MenuItem
    Me.groupTop.SuspendLayout()
    Me.pnlButtons.SuspendLayout()
    Me.groupSelected.SuspendLayout()
    Me.SuspendLayout()
    '
    'groupTop
    '
    Me.groupTop.Controls.Add(Me.pMatchingGrid)
    Me.groupTop.Controls.Add(Me.lblMatching)
    Me.groupTop.Controls.Add(Me.splitAboveMatching)
    Me.groupTop.Controls.Add(Me.panelCriteria)
    Me.groupTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.groupTop.Location = New System.Drawing.Point(0, 0)
    Me.groupTop.Name = "groupTop"
    Me.groupTop.Size = New System.Drawing.Size(633, 406)
    Me.groupTop.TabIndex = 10
    Me.groupTop.TabStop = False
    Me.groupTop.Text = "Select Attribute Values to Filter Available Data"
    '
    'pMatchingGrid
    '
    Me.pMatchingGrid.AllowHorizontalScrolling = True
    Me.pMatchingGrid.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pMatchingGrid.LineColor = System.Drawing.Color.Empty
    Me.pMatchingGrid.LineWidth = 0.0!
    Me.pMatchingGrid.Location = New System.Drawing.Point(3, 212)
    Me.pMatchingGrid.Name = "pMatchingGrid"
    Me.pMatchingGrid.Size = New System.Drawing.Size(627, 191)
    Me.pMatchingGrid.TabIndex = 15
    '
    'lblMatching
    '
    Me.lblMatching.Dock = System.Windows.Forms.DockStyle.Top
    Me.lblMatching.Location = New System.Drawing.Point(3, 194)
    Me.lblMatching.Name = "lblMatching"
    Me.lblMatching.Size = New System.Drawing.Size(627, 18)
    Me.lblMatching.TabIndex = 14
    Me.lblMatching.Text = "Matching Data (click to select)"
    '
    'splitAboveMatching
    '
    Me.splitAboveMatching.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitAboveMatching.Location = New System.Drawing.Point(3, 185)
    Me.splitAboveMatching.Name = "splitAboveMatching"
    Me.splitAboveMatching.Size = New System.Drawing.Size(627, 9)
    Me.splitAboveMatching.TabIndex = 12
    Me.splitAboveMatching.TabStop = False
    '
    'panelCriteria
    '
    Me.panelCriteria.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelCriteria.Location = New System.Drawing.Point(3, 18)
    Me.panelCriteria.Name = "panelCriteria"
    Me.panelCriteria.Size = New System.Drawing.Size(627, 167)
    Me.panelCriteria.TabIndex = 11
    '
    'pnlButtons
    '
    Me.pnlButtons.Controls.Add(Me.btnCancel)
    Me.pnlButtons.Controls.Add(Me.btnOk)
    Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlButtons.Location = New System.Drawing.Point(0, 559)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.Size = New System.Drawing.Size(633, 46)
    Me.pnlButtons.TabIndex = 12
    '
    'btnCancel
    '
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Location = New System.Drawing.Point(125, 9)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(96, 28)
    Me.btnCancel.TabIndex = 4
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(10, 9)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(96, 28)
    Me.btnOk.TabIndex = 3
    Me.btnOk.Text = "Ok"
    '
    'splitAboveSelected
    '
    Me.splitAboveSelected.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitAboveSelected.Location = New System.Drawing.Point(0, 406)
    Me.splitAboveSelected.Name = "splitAboveSelected"
    Me.splitAboveSelected.Size = New System.Drawing.Size(633, 9)
    Me.splitAboveSelected.TabIndex = 11
    Me.splitAboveSelected.TabStop = False
    '
    'groupSelected
    '
    Me.groupSelected.Controls.Add(Me.pSelectedGrid)
    Me.groupSelected.Dock = System.Windows.Forms.DockStyle.Fill
    Me.groupSelected.Location = New System.Drawing.Point(0, 415)
    Me.groupSelected.Name = "groupSelected"
    Me.groupSelected.Size = New System.Drawing.Size(633, 144)
    Me.groupSelected.TabIndex = 14
    Me.groupSelected.TabStop = False
    Me.groupSelected.Text = "Selected Data"
    '
    'pSelectedGrid
    '
    Me.pSelectedGrid.AllowHorizontalScrolling = True
    Me.pSelectedGrid.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pSelectedGrid.LineColor = System.Drawing.Color.Empty
    Me.pSelectedGrid.LineWidth = 0.0!
    Me.pSelectedGrid.Location = New System.Drawing.Point(3, 18)
    Me.pSelectedGrid.Name = "pSelectedGrid"
    Me.pSelectedGrid.Size = New System.Drawing.Size(627, 123)
    Me.pSelectedGrid.TabIndex = 0
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.mnuAttributes, Me.mnuSelect})
    '
    'MenuItem1
    '
    Me.MenuItem1.Index = 0
    Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddData, Me.mnuFileManage})
    Me.MenuItem1.Text = "&File"
    '
    'mnuAddData
    '
    Me.mnuAddData.Index = 0
    Me.mnuAddData.Text = "&Add Data"
    '
    'mnuFileManage
    '
    Me.mnuFileManage.Index = 1
    Me.mnuFileManage.Text = "&Manage Data Sources"
    '
    'mnuAttributes
    '
    Me.mnuAttributes.Index = 1
    Me.mnuAttributes.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributesAdd, Me.mnuAttributesRemove, Me.mnuAttributesMove})
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
    'mnuAttributesMove
    '
    Me.mnuAttributesMove.Index = 2
    Me.mnuAttributesMove.Text = "&Move"
    '
    'mnuSelect
    '
    Me.mnuSelect.Index = 2
    Me.mnuSelect.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSelectAll, Me.mnuSelectClear, Me.mnuSelectAllMatching, Me.mnuSelectNoMatching})
    Me.mnuSelect.Text = "&Select"
    '
    'mnuSelectAll
    '
    Me.mnuSelectAll.Index = 0
    Me.mnuSelectAll.Text = "Select &All"
    '
    'mnuSelectClear
    '
    Me.mnuSelectClear.Index = 1
    Me.mnuSelectClear.Text = "&Un-select All"
    '
    'mnuSelectAllMatching
    '
    Me.mnuSelectAllMatching.Index = 2
    Me.mnuSelectAllMatching.Text = "Select &Matching"
    '
    'mnuSelectNoMatching
    '
    Me.mnuSelectNoMatching.Index = 3
    Me.mnuSelectNoMatching.Text = "&Un-select Matching"
    '
    'frmSelectData
    '
    Me.AcceptButton = Me.btnOk
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.CancelButton = Me.btnCancel
    Me.ClientSize = New System.Drawing.Size(633, 605)
    Me.Controls.Add(Me.groupSelected)
    Me.Controls.Add(Me.splitAboveSelected)
    Me.Controls.Add(Me.pnlButtons)
    Me.Controls.Add(Me.groupTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "frmSelectData"
    Me.Text = "Select Data"
    Me.groupTop.ResumeLayout(False)
    Me.pnlButtons.ResumeLayout(False)
    Me.groupSelected.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const PADDING As Integer = 5
  'Private Const REMOVE_VALUE = "~Remove~"
  Private Const NOTHING_VALUE = "~Missing~"

  Private pcboCriteria() As Windows.Forms.ComboBox
  Private plstCriteria() As atcGrid
  Private pCriteriaFraction() As Single

  Private WithEvents pDataManager As atcDataManager

  Private pMatchingGroup As atcDataGroup
  Private pSelectedGroup As atcDataGroup
  Private pSaveGroup As atcDataGroup = Nothing

  Private pMatchingSource As GridSource
  Private pSelectedSource As GridSource

  Private pInitializing As Boolean
  Private pSelectedOK As Boolean

  Private pTotalTS As Integer

  Public Function AskUser(ByVal aDataManager As atcDataManager, Optional ByVal aGroup As atcDataGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcDataGroup
    If aGroup Is Nothing Then
      pSelectedGroup = New atcDataGroup
    Else
      pSaveGroup = aGroup.Clone
      pSelectedGroup = aGroup
    End If

    pDataManager = aDataManager

    'If pDataManager.DataSources.Count = 1 Then
    '  pDataManager.UserManage()
    '  While pDataManager.DataSources.Count = 1
    '    Application.DoEvents()
    '  End While
    'End If

    pMatchingGroup = New atcDataGroup
    pMatchingSource = New GridSource(pDataManager, pMatchingGroup)
    pMatchingSource.SelectedItems = pSelectedGroup
    pSelectedSource = New GridSource(pDataManager, pSelectedGroup)

    pMatchingGrid.Initialize(pMatchingSource)
    pSelectedGrid.Initialize(pSelectedSource)

    Populate()
    If aModal Then
      Me.ShowDialog()
      If Not pSelectedOK Then 'User clicked Cancel or closed dialog
        pSelectedGroup.ChangeTo(pSaveGroup)
      End If
    Else
      Me.Show()
    End If

    pDataManager = Nothing
    pMatchingGroup = Nothing
    pSaveGroup = Nothing
    pMatchingSource = Nothing
    pSelectedSource = Nothing
    Return pSelectedGroup
  End Function

  Private Sub Populate()
    pInitializing = True

    Try
      For iCriteria As Integer = pcboCriteria.GetUpperBound(0) To 0 Step -1
        RemoveCriteria(pcboCriteria(iCriteria), plstCriteria(iCriteria))
      Next
    Catch ex As Exception
      'first time through there is nothing to remove, error is normal
    End Try

    ReDim pcboCriteria(0)
    ReDim plstCriteria(0)
    ReDim pCriteriaFraction(0)

    For Each lAttribName As String In pDataManager.SelectionAttributes
      AddCriteria(lAttribName)
    Next

    PopulateMatching()
    pInitializing = False
    UpdatedCriteria()
    SizeCriteria()
  End Sub

  Private Sub PopulateCriteriaCombos()
    Dim i As Integer
    For i = 0 To pcboCriteria.GetUpperBound(0)
      pcboCriteria(i).Items.Clear()
    Next
    For Each def As atcAttributeDefinition In atcDataAttributes.AllDefinitions
      If Not pcboCriteria(0).Items.Contains(def.Name) _
       AndAlso atcDataAttributes.IsSimple(def) Then
        For i = 0 To pcboCriteria.GetUpperBound(0)
          pcboCriteria(i).Items.Add(def.Name)
        Next
      End If
    Next
  End Sub

  Private Sub PopulateCriteriaList(ByVal aAttributeName As String, ByVal aList As atcGrid)
    Dim lNumeric As Boolean = False
    Dim lSortedItems As New atcCollection
    Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(aAttributeName)
    Dim lTsIndex As Integer = 0
    Dim lTsLastIndex As Integer = pDataManager.DataSets.Count - 1

    LogDbg("Start PopulateCriteriaList(" & aAttributeName & ")")

    If Not lAttributeDef Is Nothing Then
      Select Case lAttributeDef.TypeString.ToLower
        Case "integer", "single", "double"
          lNumeric = True
      End Select
    End If

    With aList
      .Visible = False
      For Each ts As atcDataSet In pDataManager.DataSets
        Dim lVal As String = ts.Attributes.GetFormattedValue(aAttributeName, NOTHING_VALUE)
        Dim lIndex As Integer = 0
        If Not lSortedItems.Contains(lVal) Then
          If lNumeric Then
            Dim lKey As Double = ts.Attributes.GetValue(aAttributeName, Double.NegativeInfinity)
            lIndex = BinarySearchNumeric(lKey, lSortedItems)
            lSortedItems.Insert(lIndex, lKey, lVal)
          Else
            Dim lKey As String = ts.Attributes.GetValue(aAttributeName, NOTHING_VALUE)
            lIndex = BinarySearchString(lKey, lSortedItems)
            lSortedItems.Insert(lIndex, lKey, lVal)
          End If
        End If
        lTsIndex += 1
        LogProgress("PopulateCriteriaList ", lTsIndex, lTsLastIndex)
      Next
      .Initialize(New ListSource(lSortedItems))
      If lNumeric Then
        .Source.Alignment(0, 0) = atcAlignment.HAlignDecimal
      Else
        .Source.Alignment(0, 0) = atcAlignment.HAlignLeft
      End If
      .Visible = True
      .Refresh()
    End With

    LogDbg("Finished PopulateCriteriaList(" & aAttributeName & ")")
  End Sub

  'Returns first index of a key equal to or higher than aKey
  Private Function BinarySearchString(ByVal aKey As String, ByVal aKeys As atcCollection) As Integer
    Dim lHigher As Integer = aKeys.Count
    Dim lLower As Integer = -1
    Dim lProbe As Integer
    While (lHigher - lLower > 1)
      lProbe = (lHigher + lLower) / 2
      If (aKeys.Item(lProbe) < aKey) Then
        lLower = lProbe
      Else
        lHigher = lProbe
      End If
    End While
    Return lHigher
  End Function

  'Returns first index of a key equal to or higher than aKey
  Private Function BinarySearchNumeric(ByVal aKey As Double, ByVal aKeys As ArrayList) As Integer
    Dim lHigher As Integer = aKeys.Count
    Dim lLower As Integer = -1
    Dim lProbe As Integer
    While (lHigher - lLower > 1)
      lProbe = (lHigher + lLower) / 2
      If (CDbl(aKeys.Item(lProbe)) < aKey) Then
        lLower = lProbe
      Else
        lHigher = lProbe
      End If
    End While
    Return lHigher
  End Function

  Private Sub PopulateMatching()
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    pMatchingGroup.Clear()
    pTotalTS = 0
    For Each ts As atcDataSet In pDataManager.DataSets
      pTotalTS += 1
      For iCriteria As Integer = 0 To iLastCriteria
        Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
        If Not attrName Is Nothing Then
          Dim selectedValues As atcCollection = CType(plstCriteria(iCriteria).Source, ListSource).SelectedItems
          If selectedValues.Count > 0 Then 'none selected = all selected
            Dim attrValue As String = ts.Attributes.GetFormattedValue(attrName, NOTHING_VALUE)
            If Not selectedValues.Contains(attrValue) Then 'Does not match this criteria
              GoTo NextTS
            End If
          End If
        End If
      Next
      'Matched all criteria, add to matching table
      pMatchingGroup.Add(ts)
      SelectMatchingRow(pMatchingGroup.Count, pSelectedGroup.Contains(ts))
NextTS:
    Next
    lblMatching.Text = "Matching Data (" & pMatchingGroup.Count & " of " & pTotalTS & ")"
    pMatchingGrid.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub cboCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Not sender.SelectedItem Is Nothing Then
      'If sender.SelectedItem = REMOVE_VALUE Then
      '  If plstCriteria.GetUpperBound(0) > 0 Then
      '    RemoveCriteria(sender, plstCriteria(GetIndex(sender.name)))
      '  End If
      'Else
      PopulateCriteriaList(sender.SelectedItem, plstCriteria(GetIndex(sender.name)))
      UpdatedCriteria()
      'End If
    End If
  End Sub

  Private Sub lstCriteria_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer)
    Dim lSource As ListSource = aGrid.Source
    Dim lIndex As Integer = lSource.SelectedItems.IndexFromKey(aRow)
    If lIndex >= 0 Then
      lSource.SelectedItems.RemoveAt(lIndex)
    Else
      lSource.SelectedItems.Add(aRow, lSource.CellValue(aRow, aColumn))
    End If
    aGrid.Refresh()
    PopulateMatching()
  End Sub

  Private Sub UpdatedCriteria()
    If Not pInitializing Then
      Dim mnu As MenuItem
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)

      UpdateManagerSelectionAttributes()
      PopulateMatching()
      RefreshSelected()

      For Each mnu In mnuAttributesRemove.MenuItems
        RemoveHandler mnu.Click, AddressOf mnuRemove_Click
      Next
      For Each mnu In mnuAttributesMove.MenuItems
        RemoveHandler mnu.Click, AddressOf mnuMove_Click
      Next

      mnuAttributesRemove.MenuItems.Clear()
      mnuAttributesMove.MenuItems.Clear()

      If iLastCriteria > 0 Then 'Only allow moving/removing if more than one exists
        For iCriteria As Integer = 0 To iLastCriteria
          mnu = mnuAttributesRemove.MenuItems.Add("&" & iCriteria + 1 & " " & pcboCriteria(iCriteria).SelectedItem)
          AddHandler mnu.Click, AddressOf mnuRemove_Click
          mnu = mnuAttributesMove.MenuItems.Add("&" & iCriteria + 1 & " " & pcboCriteria(iCriteria).SelectedItem)
          AddHandler mnu.Click, AddressOf mnuMove_Click
        Next
      End If
    End If
  End Sub

  Private Sub RemoveCriteria(ByVal cbo As Windows.Forms.ComboBox, ByVal lst As atcGrid)
    Dim iRemoving As Integer = GetIndex(cbo.Name)
    Dim newLastCriteria As Integer = pcboCriteria.GetUpperBound(0) - 1
    Dim OldToNew As Single = 1 / (1 - pCriteriaFraction(iRemoving))
    Dim mnu As MenuItem
    RemoveHandler cbo.SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    RemoveHandler lst.MouseDownCell, AddressOf lstCriteria_MouseDownCell
    panelCriteria.Controls.Remove(cbo)
    panelCriteria.Controls.Remove(lst)

    For iMoving As Integer = iRemoving To pcboCriteria.GetUpperBound(0) - 1
      pcboCriteria(iMoving) = pcboCriteria(iMoving + 1)
      plstCriteria(iMoving) = plstCriteria(iMoving + 1)
      pcboCriteria(iMoving).Name = "cboCriteria#" & iMoving
      plstCriteria(iMoving).Name = "lstCriteria#" & iMoving
      pCriteriaFraction(iMoving) = pCriteriaFraction(iMoving + 1)
    Next

    ReDim Preserve pcboCriteria(newLastCriteria)
    ReDim Preserve plstCriteria(newLastCriteria)
    ReDim Preserve pCriteriaFraction(newLastCriteria)

    'Expand remaining criteria proportionally to fill space
    For iScanCriteria As Integer = 0 To newLastCriteria
      pCriteriaFraction(iScanCriteria) *= OldToNew
    Next

    'If pcboCriteria.GetUpperBound(0) = 0 Then
    '  pcboCriteria(0).Items.Remove(REMOVE_VALUE)
    'End If

    SizeCriteria()
    UpdatedCriteria()
  End Sub

  Private Sub AddCriteria(Optional ByVal aText As String = "")
    Dim iCriteria As Integer = pcboCriteria.GetUpperBound(0)

    If Not pcboCriteria(iCriteria) Is Nothing Then 'If we already populated this index, move to next one
      iCriteria += 1                               'This happens every time except for the first one
      ReDim Preserve pcboCriteria(iCriteria)
      ReDim Preserve plstCriteria(iCriteria)
      ReDim Preserve pCriteriaFraction(iCriteria)
    End If

    Dim fractionInUse As Single = 0
    For iScanCriteria As Integer = 0 To iCriteria - 1
      fractionInUse += pCriteriaFraction(iScanCriteria)
    Next

    Dim newEqualPortion As Single = 1 / (iCriteria + 1)
    Dim totalShrinkingNeeded As Single = fractionInUse + newEqualPortion - 1

    'Default to give new one an equal portion of the width
    pCriteriaFraction(iCriteria) = newEqualPortion

    If totalShrinkingNeeded > 0 Then 'Not enough extra unused space
      'Shrink existing criteria proportionally to fit the new one in
      For iScanCriteria As Integer = 0 To iCriteria - 1
        pCriteriaFraction(iScanCriteria) *= (1 - totalShrinkingNeeded)
      Next
    End If

    pcboCriteria(iCriteria) = New Windows.Forms.ComboBox
    plstCriteria(iCriteria) = New atcGrid

    panelCriteria.Controls.Add(pcboCriteria(iCriteria))
    panelCriteria.Controls.Add(plstCriteria(iCriteria))

    AddHandler pcboCriteria(iCriteria).SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    AddHandler plstCriteria(iCriteria).MouseDownCell, AddressOf lstCriteria_MouseDownCell

    With pcboCriteria(iCriteria)
      .Name = "cboCriteria#" & iCriteria
      .DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
      .MaxDropDownItems = 40
      .Sorted = True
    End With

    With plstCriteria(iCriteria)
      .Name = "lstCriteria#" & iCriteria
      .AllowHorizontalScrolling = False
    End With

    If iCriteria = 0 Then
      PopulateCriteriaCombos()
    Else 'populate from first combo box
      'If Not pcboCriteria(0).Items.Contains(REMOVE_VALUE) Then
      '  pcboCriteria(0).Items.Add(REMOVE_VALUE)
      'End If

      For iItem As Integer = 0 To pcboCriteria(0).Items.Count - 1
        pcboCriteria(iCriteria).Items.Add(pcboCriteria(0).Items.Item(iItem))
      Next
    End If
    If aText.Length > 0 Then
      pcboCriteria(iCriteria).Text = aText
    Else 'Find next criteria that is not yet in use
      For Each curName As String In pcboCriteria(iCriteria).Items
        'If curName <> REMOVE_VALUE Then
        For iOtherCriteria As Integer = 0 To iCriteria - 1
          If curName.Equals(pcboCriteria(iOtherCriteria).SelectedItem) Then GoTo NextName
        Next
        pcboCriteria(iCriteria).Text = curName
        Exit For
        'End If
NextName:
      Next
    End If
  End Sub

  Private Sub panelCriteria_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCriteria.SizeChanged
    SizeCriteria()
  End Sub

  Private Sub ResizeOneCriteria(ByVal aCriteria As Integer, ByVal aWidth As Integer)
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    Dim lWidth As Integer = aWidth - PADDING
    pcboCriteria(aCriteria).Width = lWidth
    pCriteriaFraction(aCriteria) = lWidth / (panelCriteria.Width - PADDING)
    plstCriteria(aCriteria).Width = lWidth
    plstCriteria(aCriteria).ColumnWidth(0) = lWidth
    While aCriteria < iLastCriteria
      aCriteria += 1
      pcboCriteria(aCriteria).Left = pcboCriteria(aCriteria - 1).Left + pcboCriteria(aCriteria - 1).Width + PADDING
      plstCriteria(aCriteria).Left = pcboCriteria(aCriteria).Left
    End While

    'Fit rightmost criteria to fill remaining space
    Dim availableWidth As Integer = panelCriteria.Width - PADDING * 2
    If pcboCriteria(iLastCriteria).Left < availableWidth Then
      lWidth = availableWidth - pcboCriteria(iLastCriteria).Left
      pcboCriteria(iLastCriteria).Width = lWidth
      plstCriteria(iLastCriteria).Width = lWidth
      plstCriteria(iLastCriteria).ColumnWidth(0) = lWidth
    End If
  End Sub

  Private Sub SizeCriteria()
    If Not pcboCriteria Is Nothing Then
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
      If iLastCriteria >= 0 Then
        Dim eachCriteriaPortion() As Integer
        Dim availableWidth As Integer = panelCriteria.Width - PADDING
        'Dim perCriteriaWidth As Integer = (panelCriteria.Width - PADDING) / (iLastCriteria + 1)
        Dim curLeft As Integer = PADDING

        pMatchingGrid.ColumnWidth(0) = 0
        pSelectedGrid.ColumnWidth(0) = 0

        For iCriteria As Integer = 0 To iLastCriteria
          pcboCriteria(iCriteria).Top = PADDING
          pcboCriteria(iCriteria).Left = curLeft
          If iCriteria = iLastCriteria AndAlso curLeft < availableWidth Then 'Fit rightmost criteria to fill remaining space
            pcboCriteria(iCriteria).Width = availableWidth - curLeft
          Else
            pcboCriteria(iCriteria).Width = availableWidth * pCriteriaFraction(iCriteria)
          End If
          If pcboCriteria(iCriteria).Width > PADDING Then pcboCriteria(iCriteria).Width -= PADDING

          With plstCriteria(iCriteria)
            .Top = pcboCriteria(iCriteria).Top + pcboCriteria(iCriteria).Height + PADDING
            .Left = curLeft
            .Width = pcboCriteria(iCriteria).Width
            .ColumnWidth(0) = .Width
            .Height = panelCriteria.Height - .Top - PADDING
            .Visible = True
            .BringToFront()
            .Refresh()
          End With

          curLeft = pcboCriteria(iCriteria).Left + pcboCriteria(iCriteria).Width + PADDING

          pMatchingGrid.ColumnWidth(iCriteria + 1) = pcboCriteria(iCriteria).Width + PADDING
          pSelectedGrid.ColumnWidth(iCriteria + 1) = pMatchingGrid.ColumnWidth(iCriteria + 1)
        Next
        pMatchingGrid.Refresh()
        pSelectedGrid.Refresh()
      End If
    End If
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    'If user didn't select anything, 
    ' but either narrowed the matching group or there are not more than 10 datasets,
    ' assume they meant to select the matching ones
    If pSelectedGroup.Count = 0 AndAlso _
      (pMatchingGroup.Count < pDataManager.DataSets.Count OrElse pMatchingGroup.Count < 11) Then
      pSelectedGroup.ChangeTo(pMatchingGroup)
    End If
    pSelectedOK = True
    Me.Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    pSelectedOK = False
    pSelectedGroup.ChangeTo(pSaveGroup)
    Me.Close()
  End Sub

  'Update SelectionAttributes from current set of pcboCriteria
  Private Sub UpdateManagerSelectionAttributes()
    Dim curAttributes As New ArrayList
    For iCriteria As Integer = 0 To pcboCriteria.GetUpperBound(0)
      Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
      If Not attrName Is Nothing Then
        curAttributes.Add(attrName)
      End If
    Next
    If curAttributes.Count > 0 Then
      pDataManager.SelectionAttributes.Clear()
      pDataManager.SelectionAttributes.AddRange(curAttributes)
    End If
  End Sub

  Private Sub SelectMatchingRow(ByVal aRow As Integer, ByVal aSelect As Boolean)
    For iColumn As Integer = 0 To pMatchingSource.Columns - 1
      pMatchingSource.CellSelected(aRow, iColumn) = aSelect
    Next
  End Sub

  Private Sub pMatchingGrid_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles pMatchingGrid.MouseDownCell
    If IsNumeric(pMatchingSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pMatchingSource.CellValue(aRow, 0)) 'Serial number in clicked row
      Dim iTS As Integer = pSelectedGroup.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Already selected, unselect
        pSelectedGroup.RemoveAt(iTS)
        SelectMatchingRow(aRow, False)
      Else 'Not already selected, select it now
        iTS = pMatchingGroup.IndexOfSerial(lSerial)
        If iTS >= 0 Then 'Found matching serial number in pMatchingGroup
          Dim selTS As atcData.atcDataSet = pMatchingGroup(iTS)
          pSelectedGroup.Add(selTS.Attributes.GetValue("id"), selTS)
          SelectMatchingRow(aRow, True)
        End If
      End If
    End If
    RefreshSelected()
  End Sub

  Private Sub RefreshSelected()
    pMatchingGrid.Refresh()
    pSelectedGrid.Refresh()
    groupSelected.Text = "Selected Data (" & pSelectedGroup.Count & " of " & pTotalTS & ")"
  End Sub

  Private Sub pSelectedGrid_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles pSelectedGrid.MouseDownCell
    If IsNumeric(pSelectedSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pSelectedSource.CellValue(aRow, 0)) 'Serial number in row to be removed
      Dim iTS As Integer = pSelectedGroup.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Found matching serial number in pSelectedGroup
        pSelectedGroup.RemoveAt(iTS)
        RefreshSelected()
      Else
        'TODO: should never reach this line
      End If
    End If
  End Sub

  Private Sub pDataManager_OpenedData(ByVal aDataSource As atcDataSource) Handles pDataManager.OpenedData
    Populate()
  End Sub

  Private Sub pMatchingGrid_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pMatchingGrid.UserResizedColumn
    pSelectedGrid.ColumnWidth(aColumn) = aWidth
    pSelectedGrid.Refresh()
    ResizeOneCriteria(aColumn - 1, aWidth)
  End Sub

  Private Sub pSelectedGrid_UserResizedColumn(ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pSelectedGrid.UserResizedColumn
    pMatchingGrid.ColumnWidth(aColumn) = aWidth
    pMatchingGrid.Refresh()
    ResizeOneCriteria(aColumn - 1, aWidth)
  End Sub

  Private Sub mnuAttributesAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAttributesAdd.Click
    AddCriteria()
    UpdatedCriteria()
    SizeCriteria()
  End Sub

  Private Sub mnuSelectClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectClear.Click
    If pSelectedGroup.Count > 0 Then
      pSelectedGroup.Clear()
      RefreshSelected()
    End If
  End Sub

  Private Sub mnuSelectAllMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAllMatching.Click
    For Each ts As atcDataSet In pMatchingGroup
      If Not pSelectedGroup.Contains(ts) Then pSelectedGroup.Add(ts.Attributes.GetValue("id"), ts)
    Next
    RefreshSelected()
    'Dim lAdd As New atcCollection
    'For Each ts As atcDataSet In pMatchingGroup
    '  If Not pSelectedGroup.Contains(ts) Then lAdd.Add(ts)
    'Next
    'If lAdd.Count > 0 Then
    '  pSelectedGroup.Add(lAdd)
    '  RefreshSelected()
    'End If
  End Sub

  Private Sub mnuSelectNoMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectNoMatching.Click
    Dim lRemove As New atcCollection
    For Each ts As atcDataSet In pMatchingGroup
      If pSelectedGroup.Contains(ts) Then lRemove.Add(ts)
    Next
    If lRemove.Count > 0 Then
      pSelectedGroup.Remove(lRemove)
      RefreshSelected()
    End If
  End Sub

  Private Sub mnuFileManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileManage.Click
    pDataManager.UserManage() ' .OpenData("")
  End Sub

  Private Sub mnuRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim mnu As MenuItem = sender
    Dim index As Integer = mnu.Index
    RemoveCriteria(pcboCriteria(index), plstCriteria(index))
  End Sub

  Private Sub mnuMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    'TODO: re-order criteria
  End Sub

  Private Sub mnuAddData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddData.Click
    Dim lNewSource As atcDataSource = pDataManager.UserSelectDataSource
    pDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
    pSelectedGroup.AddRange(lNewSource.DataSets) 'TODO: add with IDs as keys
  End Sub

  Private Sub mnuSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAll.Click
    Dim lAdd As New atcCollection
    For Each ts As atcDataSet In pDataManager.DataSets
      If Not pSelectedGroup.Contains(ts) Then lAdd.Add(ts)
    Next
    If lAdd.Count > 0 Then
      pSelectedGroup.Add(lAdd) 'TODO: add with IDs as keys
      RefreshSelected()
    End If
  End Sub

End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const LabelRow As Integer = -1

  Private pDataManager As atcDataManager
  Private pDataGroup As atcDataGroup
  Private pSelected As atcCollection

  Public Property SelectedItems() As atcCollection
    Get
      Return pSelected
    End Get
    Set(ByVal newValue As atcCollection)
      pSelected = newValue
    End Set
  End Property

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataManager = aDataManager
    pDataGroup = aDataGroup
  End Sub

  Protected Overrides Property ProtectedColumns() As Integer
    Get
      Return pDataManager.SelectionAttributes.Count() + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedRows() As Integer
    Get
      Return pDataGroup.Count + LabelRow + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = LabelRow Then
        If aColumn = 0 Then
          Return ""
        Else
          Return pDataManager.SelectionAttributes(aColumn - 1)
        End If
      ElseIf aColumn = 0 Then
        Return pDataGroup(aRow - (LabelRow + 1)).Serial()
      Else
        Return pDataGroup(aRow - (LabelRow + 1)).Attributes.GetFormattedValue(pDataManager.SelectionAttributes(aColumn - 1))
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Protected Overrides Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      If aRow > LabelRow AndAlso aColumn > 0 Then
        Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(pDataManager.SelectionAttributes(aColumn - 1))
        If Not lAttributeDef Is Nothing Then
          Select Case lAttributeDef.TypeString.ToLower
            Case "integer", "single", "double"
              Return atcAlignment.HAlignDecimal
          End Select
        End If
      End If
      Return atcControls.atcAlignment.HAlignLeft
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property

  Protected Overrides Property ProtectedCellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      If Not pSelected Is Nothing Then
        If aRow = LabelRow Then
          Return False
        Else
          Return pSelected.Contains(pDataGroup(aRow - (LabelRow + 1)))
        End If
      End If
      Return False
    End Get
    Set(ByVal newValue As Boolean)
    End Set
  End Property

End Class

Friend Class ListSource
  Inherits atcControls.atcGridSource

  Private pAlignment As atcAlignment = atcAlignment.HAlignDecimal
  Private pValues As atcCollection
  Private pSelected As atcCollection

  Public Property SelectedItems() As atcCollection
    Get
      Return pSelected
    End Get
    Set(ByVal newValue As atcCollection)
      pSelected = newValue
    End Set
  End Property

  Sub New(ByVal aValues As atcCollection, Optional ByVal aSelected As atcCollection = Nothing)
    pValues = aValues
    If aSelected Is Nothing Then
      pSelected = New atcCollection
    Else
      pSelected = aSelected
    End If
  End Sub

  Protected Overrides Property ProtectedColumns() As Integer
    Get
      Return 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedRows() As Integer
    Get
      If pValues Is Nothing Then Return 1
      Return pValues.Count
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      Try
        Return pValues.ItemByIndex(aRow)
      Catch
        Return ""
      End Try
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Protected Overrides Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      Return pAlignment
    End Get
    Set(ByVal newValue As atcControls.atcAlignment)
      pAlignment = newValue
    End Set
  End Property

  'Protected Overrides Property ProtectedCellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
  '  Get
  '    If pSelected.Contains(ProtectedCellValue(aRow, aColumn)) Then
  '      Return System.Drawing.SystemColors.Highlight
  '    Else
  '      Return System.Drawing.SystemColors.Window 'TODO: use grid's CellBackColor
  '    End If
  '  End Get
  '  Set(ByVal Value As System.Drawing.Color)
  '  End Set
  'End Property

  Protected Overrides Property ProtectedCellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    Get
      Return pSelected.Keys.Contains(aRow) ' & "," & aColumn)
    End Get
    Set(ByVal newValue As Boolean)
      If newValue Then
        If Not pSelected.Keys.Contains(aRow) Then
          pSelected.Add(aRow, ProtectedCellValue(aRow, aColumn))
        End If
      Else
        pSelected.RemoveByKey(aRow)
      End If
    End Set
  End Property
End Class