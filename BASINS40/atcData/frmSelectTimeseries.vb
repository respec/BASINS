Imports atcData

Imports System.Windows.Forms

Friend Class frmSelectTimeseries
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

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
  Friend WithEvents btnAddAttribute As System.Windows.Forms.Button
  Friend WithEvents splitAboveMatching As System.Windows.Forms.Splitter
  Friend WithEvents lblMatching As System.Windows.Forms.Label
  Friend WithEvents pMatchingGrid As atcControls.atcGrid
  Friend WithEvents pSelectedGrid As atcControls.atcGrid
  Friend WithEvents btnOpen As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectTimeseries))
    Me.groupTop = New System.Windows.Forms.GroupBox
    Me.pMatchingGrid = New atcControls.atcGrid
    Me.lblMatching = New System.Windows.Forms.Label
    Me.splitAboveMatching = New System.Windows.Forms.Splitter
    Me.panelCriteria = New System.Windows.Forms.Panel
    Me.btnAddAttribute = New System.Windows.Forms.Button
    Me.pnlButtons = New System.Windows.Forms.Panel
    Me.btnOpen = New System.Windows.Forms.Button
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.splitAboveSelected = New System.Windows.Forms.Splitter
    Me.groupSelected = New System.Windows.Forms.GroupBox
    Me.pSelectedGrid = New atcControls.atcGrid
    Me.groupTop.SuspendLayout()
    Me.panelCriteria.SuspendLayout()
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
    Me.groupTop.Size = New System.Drawing.Size(528, 352)
    Me.groupTop.TabIndex = 10
    Me.groupTop.TabStop = False
    Me.groupTop.Text = "Select Attribute Values to Filter Available Data"
    '
    'pMatchingGrid
    '
    Me.pMatchingGrid.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pMatchingGrid.LineColor = System.Drawing.Color.Empty
    Me.pMatchingGrid.LineWidth = 0.0!
    Me.pMatchingGrid.Location = New System.Drawing.Point(3, 184)
    Me.pMatchingGrid.Name = "pMatchingGrid"
    Me.pMatchingGrid.Size = New System.Drawing.Size(522, 165)
    Me.pMatchingGrid.TabIndex = 15
    '
    'lblMatching
    '
    Me.lblMatching.Dock = System.Windows.Forms.DockStyle.Top
    Me.lblMatching.Location = New System.Drawing.Point(3, 168)
    Me.lblMatching.Name = "lblMatching"
    Me.lblMatching.Size = New System.Drawing.Size(522, 16)
    Me.lblMatching.TabIndex = 14
    Me.lblMatching.Text = "Matching Timeseries (click to select)"
    '
    'splitAboveMatching
    '
    Me.splitAboveMatching.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitAboveMatching.Location = New System.Drawing.Point(3, 160)
    Me.splitAboveMatching.Name = "splitAboveMatching"
    Me.splitAboveMatching.Size = New System.Drawing.Size(522, 8)
    Me.splitAboveMatching.TabIndex = 12
    Me.splitAboveMatching.TabStop = False
    '
    'panelCriteria
    '
    Me.panelCriteria.Controls.Add(Me.btnAddAttribute)
    Me.panelCriteria.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelCriteria.Location = New System.Drawing.Point(3, 16)
    Me.panelCriteria.Name = "panelCriteria"
    Me.panelCriteria.Size = New System.Drawing.Size(522, 144)
    Me.panelCriteria.TabIndex = 11
    '
    'btnAddAttribute
    '
    Me.btnAddAttribute.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAddAttribute.Location = New System.Drawing.Point(496, 8)
    Me.btnAddAttribute.Name = "btnAddAttribute"
    Me.btnAddAttribute.Size = New System.Drawing.Size(16, 16)
    Me.btnAddAttribute.TabIndex = 2
    Me.btnAddAttribute.Text = "+"
    '
    'pnlButtons
    '
    Me.pnlButtons.Controls.Add(Me.btnOpen)
    Me.pnlButtons.Controls.Add(Me.btnCancel)
    Me.pnlButtons.Controls.Add(Me.btnOk)
    Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlButtons.Location = New System.Drawing.Point(0, 485)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.Size = New System.Drawing.Size(528, 40)
    Me.pnlButtons.TabIndex = 12
    '
    'btnOpen
    '
    Me.btnOpen.Location = New System.Drawing.Point(200, 8)
    Me.btnOpen.Name = "btnOpen"
    Me.btnOpen.Size = New System.Drawing.Size(136, 24)
    Me.btnOpen.TabIndex = 5
    Me.btnOpen.Text = "Manage Data Sources"
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(104, 8)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(80, 24)
    Me.btnCancel.TabIndex = 4
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(8, 8)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(80, 24)
    Me.btnOk.TabIndex = 3
    Me.btnOk.Text = "Ok"
    '
    'splitAboveSelected
    '
    Me.splitAboveSelected.Dock = System.Windows.Forms.DockStyle.Top
    Me.splitAboveSelected.Location = New System.Drawing.Point(0, 352)
    Me.splitAboveSelected.Name = "splitAboveSelected"
    Me.splitAboveSelected.Size = New System.Drawing.Size(528, 8)
    Me.splitAboveSelected.TabIndex = 11
    Me.splitAboveSelected.TabStop = False
    '
    'groupSelected
    '
    Me.groupSelected.Controls.Add(Me.pSelectedGrid)
    Me.groupSelected.Dock = System.Windows.Forms.DockStyle.Fill
    Me.groupSelected.Location = New System.Drawing.Point(0, 360)
    Me.groupSelected.Name = "groupSelected"
    Me.groupSelected.Size = New System.Drawing.Size(528, 125)
    Me.groupSelected.TabIndex = 14
    Me.groupSelected.TabStop = False
    Me.groupSelected.Text = "Selected Timeseries"
    '
    'pSelectedGrid
    '
    Me.pSelectedGrid.Dock = System.Windows.Forms.DockStyle.Fill
    Me.pSelectedGrid.LineColor = System.Drawing.Color.Empty
    Me.pSelectedGrid.LineWidth = 0.0!
    Me.pSelectedGrid.Location = New System.Drawing.Point(3, 16)
    Me.pSelectedGrid.Name = "pSelectedGrid"
    Me.pSelectedGrid.Size = New System.Drawing.Size(522, 106)
    Me.pSelectedGrid.TabIndex = 0
    '
    'frmSelectTimeseries
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 525)
    Me.Controls.Add(Me.groupSelected)
    Me.Controls.Add(Me.splitAboveSelected)
    Me.Controls.Add(Me.pnlButtons)
    Me.Controls.Add(Me.groupTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmSelectTimeseries"
    Me.Text = "Select Timeseries"
    Me.groupTop.ResumeLayout(False)
    Me.panelCriteria.ResumeLayout(False)
    Me.pnlButtons.ResumeLayout(False)
    Me.groupSelected.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const PADDING As Integer = 15
  Private Const REMOVE_VALUE = "~Remove~"
  Private Const NOTHING_VALUE = "~Missing~"

  Private pcboCriteria() As Windows.Forms.ComboBox
  Private plstCriteria() As Windows.Forms.ListBox

  Private WithEvents pDataManager As atcDataManager

  Private pMatchingTS As atcTimeseriesGroup
  Private pSelectedTS As atcTimeseriesGroup

  Private pMatchingSource As GridSource
  Private pSelectedSource As GridSource

  Private pInitializing As Boolean
  Private pSelectedOK As Boolean

  Private pTotalTS As Integer

  Public Function AskUser(ByVal aDataManager As atcDataManager, Optional ByVal aGroup As atcTimeseriesGroup = Nothing) As atcTimeseriesGroup
    Dim pSaveGroup As atcTimeseriesGroup = Nothing
    If aGroup Is Nothing Then
      pSelectedTS = New atcTimeseriesGroup
    Else
      pSaveGroup = aGroup.Clone
      pSelectedTS = aGroup
    End If

    pDataManager = aDataManager

    'If pDataManager.DataSources.Count = 1 Then
    '  pDataManager.UserManage()
    '  While pDataManager.DataSources.Count = 1
    '    Application.DoEvents()
    '  End While
    'End If

    pMatchingTS = New atcTimeseriesGroup
    pMatchingSource = New GridSource(pDataManager, pMatchingTS)
    pSelectedSource = New GridSource(pDataManager, pSelectedTS)

    pMatchingGrid.Initialize(pMatchingSource)
    pSelectedGrid.Initialize(pSelectedSource)

    Populate()
    Me.ShowDialog()
    If Not pSelectedOK Then 'User clicked Cancel or closed dialog
      pSelectedTS.ChangeTo(pSaveGroup)
    End If
    Return pSelectedTS
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

    For Each lAttribName As String In pDataManager.SelectionAttributes
      AddCriteria(lAttribName)
    Next

    PopulateMatching()
    pInitializing = False
  End Sub

  Private Sub PopulateCriteriaCombos()
    Dim i As Integer
    For i = 0 To pcboCriteria.GetUpperBound(0)
      pcboCriteria(i).Items.Clear()
      pcboCriteria(i).Items.Add(REMOVE_VALUE)
    Next
    For Each source As atcDataSource In pDataManager.DataSources
      For Each ts As atcTimeseries In source.Timeseries
        For Each de As DictionaryEntry In ts.Attributes.GetAll
          If Not pcboCriteria(0).Items.Contains(de.Key) Then
            For i = 0 To pcboCriteria.GetUpperBound(0)
              pcboCriteria(i).Items.Add(de.Key)
            Next
          End If
        Next
      Next
    Next
  End Sub

  Private Sub PopulateCriteriaList(ByVal aAttributeName As String, ByVal aList As Windows.Forms.ListBox)
    aList.Items.Clear()
    For Each source As atcDataSource In pDataManager.DataSources
      For Each ts As atcTimeseries In source.Timeseries
        Dim val As Object = ts.Attributes.GetValue(aAttributeName, Nothing)
        If val Is Nothing Then val = NOTHING_VALUE
        If Not aList.Items.Contains(val) Then
          aList.Items.Add(val)
        End If
      Next
    Next
  End Sub

  Private Sub PopulateMatching()
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    pMatchingTS.Clear()
    pTotalTS = 0
    For Each source As atcDataSource In pDataManager.DataSources
      For Each ts As atcTimeseries In source.Timeseries
        pTotalTS += 1
        For iCriteria As Integer = 0 To iLastCriteria
          Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
          If Not attrName Is Nothing Then
            Dim selectedValues As Windows.Forms.ListBox.SelectedObjectCollection = plstCriteria(iCriteria).SelectedItems
            If selectedValues.Count > 0 Then 'none selected = all selected
              Dim attrValue As Object = ts.Attributes.GetValue(attrName, Nothing)
              If attrValue Is Nothing Then attrValue = NOTHING_VALUE
              If Not selectedValues.Contains(attrValue) Then 'Does not match this criteria
                GoTo NextTS
              End If
            End If
          End If
        Next
        'Matched all criteria, add to matching table
        pMatchingTS.Add(ts)
NextTS:
      Next
    Next
    lblMatching.Text = "Matching Timeseries (" & pMatchingTS.Count & " of " & pTotalTS & ")"
    pMatchingGrid.Refresh()
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub cboCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Not sender.SelectedItem Is Nothing Then
      If sender.SelectedItem = REMOVE_VALUE Then
        If plstCriteria.GetUpperBound(0) > 0 Then
          RemoveCriteria(sender, plstCriteria(GetIndex(sender.name)))
        End If
      Else
        PopulateCriteriaList(sender.SelectedItem, plstCriteria(GetIndex(sender.name)))
        UpdatedCriteria()
      End If
    End If
  End Sub

  Private Sub lstCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    UpdatedCriteria()
  End Sub

  Private Sub UpdatedCriteria()
    If Not pInitializing Then
      UpdateManagerSelectionAttributes()
      PopulateMatching()
      pSelectedGrid.Refresh()
    End If
  End Sub

  Private Sub btnAddAttribute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAttribute.Click
    AddCriteria()
  End Sub

  Private Sub RemoveCriteria(ByVal cbo As Windows.Forms.ComboBox, ByVal lst As Windows.Forms.ListBox)
    Dim iRemoving As Integer = GetIndex(cbo.Name)
    RemoveHandler cbo.SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    RemoveHandler lst.SelectedValueChanged, AddressOf lstCriteria_SelectedIndexChanged
    panelCriteria.Controls.Remove(cbo)
    panelCriteria.Controls.Remove(lst)
    For iMoving As Integer = iRemoving To pcboCriteria.GetUpperBound(0) - 1
      pcboCriteria(iMoving) = pcboCriteria(iMoving + 1)
      plstCriteria(iMoving) = plstCriteria(iMoving + 1)
      pcboCriteria(iMoving).Name = "cboCriteria#" & iMoving
      plstCriteria(iMoving).Name = "lstCriteria#" & iMoving
    Next
    ReDim Preserve pcboCriteria(pcboCriteria.GetUpperBound(0) - 1)
    ReDim Preserve plstCriteria(plstCriteria.GetUpperBound(0) - 1)
    SizeCriteria()
    UpdatedCriteria()
  End Sub

  Private Sub AddCriteria(Optional ByVal aText As String = "")
    Dim iCriteria As Integer = pcboCriteria.GetUpperBound(0)

    If Not pcboCriteria(iCriteria) Is Nothing Then 'If we already populated this index, move to next one
      iCriteria += 1                               'This happens every time except for the first one
      ReDim Preserve pcboCriteria(iCriteria)
      ReDim Preserve plstCriteria(iCriteria)
    End If
    pcboCriteria(iCriteria) = New Windows.Forms.ComboBox
    plstCriteria(iCriteria) = New Windows.Forms.ListBox

    panelCriteria.Controls.Add(pcboCriteria(iCriteria))
    panelCriteria.Controls.Add(plstCriteria(iCriteria))

    AddHandler pcboCriteria(iCriteria).SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
    AddHandler plstCriteria(iCriteria).SelectedValueChanged, AddressOf lstCriteria_SelectedIndexChanged

    With pcboCriteria(iCriteria)
      .Name = "cboCriteria#" & iCriteria
      .DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
      .MaxDropDownItems = 30
      .Sorted = True
    End With

    With plstCriteria(iCriteria)
      .Name = "lstCriteria#" & iCriteria
      .IntegralHeight = False
      .SelectionMode = Windows.Forms.SelectionMode.MultiSimple
      .Sorted = True
      '.Dock = Windows.Forms.DockStyle.Left
    End With

    If iCriteria = 0 Then
      PopulateCriteriaCombos()
    Else 'populate from first combo box
      For iItem As Integer = 0 To pcboCriteria(0).Items.Count - 1
        pcboCriteria(iCriteria).Items.Add(pcboCriteria(0).Items.Item(iItem))
      Next
    End If
    If aText.Length > 0 Then
      pcboCriteria(iCriteria).Text = aText
    Else 'Find next criteria that is not yet in use
      For Each curName As String In pcboCriteria(iCriteria).Items
        If curName <> REMOVE_VALUE Then
          For iOtherCriteria As Integer = 0 To iCriteria - 1
            If curName.Equals(pcboCriteria(iOtherCriteria).SelectedItem) Then GoTo NextName
          Next
          pcboCriteria(iCriteria).Text = curName
          Exit For
        End If
NextName:
      Next
    End If

    SizeCriteria()
    UpdatedCriteria()
  End Sub

  Private Sub frmSelectTimeseries_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    SizeCriteria()
  End Sub

  Private Sub panelCriteria_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCriteria.SizeChanged
    SizeCriteria()
  End Sub

  Private Sub SizeCriteria()
    If Not pcboCriteria Is Nothing Then
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
      If iLastCriteria >= 0 Then
        Dim perCriteriaWidth As Integer = (btnAddAttribute.Left - PADDING) / (iLastCriteria + 1)
        Dim curLeft As Integer = 3

        pMatchingGrid.ColumnWidth(0) = 0
        pSelectedGrid.ColumnWidth(0) = 0

        For iCriteria As Integer = 0 To iLastCriteria
          pcboCriteria(iCriteria).Top = btnAddAttribute.Top
          pcboCriteria(iCriteria).Left = curLeft
          pcboCriteria(iCriteria).Width = perCriteriaWidth - PADDING

          plstCriteria(iCriteria).Top = pcboCriteria(iCriteria).Top + pcboCriteria(iCriteria).Height + PADDING
          plstCriteria(iCriteria).Left = curLeft
          plstCriteria(iCriteria).Width = pcboCriteria(iCriteria).Width
          plstCriteria(iCriteria).Height = panelCriteria.Height - plstCriteria(iCriteria).Top - PADDING

          curLeft += perCriteriaWidth

          pMatchingGrid.ColumnWidth(iCriteria + 1) = perCriteriaWidth 'curLeft - pMatchingGrid.ColumnWidth(iCriteria)
          pSelectedGrid.ColumnWidth(iCriteria + 1) = pMatchingGrid.ColumnWidth(iCriteria + 1)
        Next
        pMatchingGrid.Refresh()
        pSelectedGrid.Refresh()
      End If
    End If
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    pSelectedOK = True
    Me.Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    pSelectedOK = False
    Me.Close()
  End Sub

  'Update SelectionAttributes from current set of pcboCriteria
  Private Sub UpdateManagerSelectionAttributes()
    pDataManager.SelectionAttributes.Clear()
    For iCriteria As Integer = 0 To pcboCriteria.GetUpperBound(0)
      Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
      If Not attrName Is Nothing Then
        pDataManager.SelectionAttributes.Add(attrName)
      End If
    Next
  End Sub

  Private Sub pMatchingGrid_MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer) Handles pMatchingGrid.MouseDownCell
    If IsNumeric(pMatchingSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pMatchingSource.CellValue(aRow, 0)) 'Serial number in clicked row
      Dim iTS As Integer = pSelectedTS.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Already selected, unselect
        pSelectedTS.Remove(iTS)
      Else 'Not already selected, select it now
        iTS = pMatchingTS.IndexOfSerial(lSerial)
        If iTS >= 0 Then 'Found matching serial number in pMatchingTS
          Dim selTS As atcData.atcTimeseries = pMatchingTS(iTS)
          pSelectedTS.Add(selTS)
        End If
      End If
      pSelectedGrid.Refresh()
    End If
    groupSelected.Text = "Selected Timeseries (" & pSelectedTS.Count & " of " & pTotalTS & ")"
  End Sub

  Private Sub pSelectedGrid_MouseDownCell(ByVal aRow As Integer, ByVal aColumn As Integer) Handles pSelectedGrid.MouseDownCell
    If IsNumeric(pSelectedSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(pSelectedSource.CellValue(aRow, 0)) 'Serial number in row to be removed
      Dim iTS As Integer = pSelectedTS.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Found matching serial number in pSelectedTS
        pSelectedTS.Remove(iTS)
        groupSelected.Text = "Selected Timeseries (" & pSelectedTS.Count & " of " & pTotalTS & ")"
        pSelectedGrid.Refresh()
      Else
        'TODO: should never reach this line
      End If
    End If
  End Sub

  Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
    pDataManager.UserManage() ' .OpenData("")
  End Sub

  Private Sub pDataManager_OpenedData(ByVal aTimeseriesFile As atcDataSource) Handles pDataManager.OpenedData
    Populate()
  End Sub
End Class

Friend Class GridSource
  Inherits atcControls.atcGridSource

  ' 0 to label the columns in row 0
  '-1 to not label columns
  Private Const LabelRow As Integer = -1

  Private pDataManager As atcDataManager
  Private pTimeseriesGroup As atcTimeseriesGroup

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup)
    pDataManager = aDataManager
    pTimeseriesGroup = aTimeseriesGroup
  End Sub

  Public Overrides Property Columns() As Integer
    Get
      Return pDataManager.SelectionAttributes.Count() + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property Rows() As Integer
    Get
      Return pTimeseriesGroup.Count + LabelRow + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = LabelRow Then
        If aColumn = 0 Then
          Return ""
        Else
          Return pDataManager.SelectionAttributes(aColumn - 1)
        End If
      ElseIf aColumn = 0 Then
        Return pTimeseriesGroup(aRow - (LabelRow + 1)).Serial()
      Else
        Return pTimeseriesGroup(aRow - (LabelRow + 1)).Attributes.GetValue(pDataManager.SelectionAttributes(aColumn - 1))
      End If
    End Get
    Set(ByVal Value As String)
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