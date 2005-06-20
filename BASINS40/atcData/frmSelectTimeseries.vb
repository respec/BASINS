Imports atcData

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
  Friend WithEvents gridMatching As System.Windows.Forms.DataGrid
  Friend WithEvents gridSelected As System.Windows.Forms.DataGrid
  Friend WithEvents btnAddAttribute As System.Windows.Forms.Button
  Friend WithEvents groupTop As System.Windows.Forms.GroupBox
  Friend WithEvents lblMatching As System.Windows.Forms.Label
  Friend WithEvents groupSelected As System.Windows.Forms.GroupBox
  Friend WithEvents pnlButtons As System.Windows.Forms.Panel
  Friend WithEvents btnOk As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSelectTimeseries))
    Me.groupTop = New System.Windows.Forms.GroupBox
    Me.btnAddAttribute = New System.Windows.Forms.Button
    Me.gridMatching = New System.Windows.Forms.DataGrid
    Me.lblMatching = New System.Windows.Forms.Label
    Me.groupSelected = New System.Windows.Forms.GroupBox
    Me.gridSelected = New System.Windows.Forms.DataGrid
    Me.pnlButtons = New System.Windows.Forms.Panel
    Me.btnCancel = New System.Windows.Forms.Button
    Me.btnOk = New System.Windows.Forms.Button
    Me.groupTop.SuspendLayout()
    CType(Me.gridMatching, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.groupSelected.SuspendLayout()
    CType(Me.gridSelected, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlButtons.SuspendLayout()
    Me.SuspendLayout()
    '
    'groupTop
    '
    Me.groupTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.groupTop.Controls.Add(Me.btnAddAttribute)
    Me.groupTop.Controls.Add(Me.gridMatching)
    Me.groupTop.Controls.Add(Me.lblMatching)
    Me.groupTop.Location = New System.Drawing.Point(8, 8)
    Me.groupTop.Name = "groupTop"
    Me.groupTop.Size = New System.Drawing.Size(512, 352)
    Me.groupTop.TabIndex = 10
    Me.groupTop.TabStop = False
    Me.groupTop.Text = "Select Attribute Values to Filter Available Data"
    '
    'btnAddAttribute
    '
    Me.btnAddAttribute.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnAddAttribute.Location = New System.Drawing.Point(480, 24)
    Me.btnAddAttribute.Name = "btnAddAttribute"
    Me.btnAddAttribute.Size = New System.Drawing.Size(16, 16)
    Me.btnAddAttribute.TabIndex = 1
    Me.btnAddAttribute.Text = "+"
    '
    'gridMatching
    '
    Me.gridMatching.AllowNavigation = False
    Me.gridMatching.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.gridMatching.CaptionVisible = False
    Me.gridMatching.DataMember = ""
    Me.gridMatching.HeaderForeColor = System.Drawing.SystemColors.ControlText
    Me.gridMatching.Location = New System.Drawing.Point(8, 200)
    Me.gridMatching.Name = "gridMatching"
    Me.gridMatching.ReadOnly = True
    Me.gridMatching.RowHeadersVisible = False
    Me.gridMatching.Size = New System.Drawing.Size(496, 144)
    Me.gridMatching.TabIndex = 1
    '
    'lblMatching
    '
    Me.lblMatching.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.lblMatching.Location = New System.Drawing.Point(8, 176)
    Me.lblMatching.Name = "lblMatching"
    Me.lblMatching.Size = New System.Drawing.Size(216, 16)
    Me.lblMatching.TabIndex = 10
    Me.lblMatching.Text = "Matching Timeseries (click to select)"
    '
    'groupSelected
    '
    Me.groupSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.groupSelected.Controls.Add(Me.gridSelected)
    Me.groupSelected.Location = New System.Drawing.Point(8, 368)
    Me.groupSelected.Name = "groupSelected"
    Me.groupSelected.Size = New System.Drawing.Size(512, 112)
    Me.groupSelected.TabIndex = 11
    Me.groupSelected.TabStop = False
    Me.groupSelected.Text = "Selected Timeseries"
    '
    'gridSelected
    '
    Me.gridSelected.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.gridSelected.CaptionVisible = False
    Me.gridSelected.CausesValidation = False
    Me.gridSelected.DataMember = ""
    Me.gridSelected.HeaderForeColor = System.Drawing.SystemColors.ControlText
    Me.gridSelected.Location = New System.Drawing.Point(8, 40)
    Me.gridSelected.Name = "gridSelected"
    Me.gridSelected.ReadOnly = True
    Me.gridSelected.RowHeadersVisible = False
    Me.gridSelected.Size = New System.Drawing.Size(496, 64)
    Me.gridSelected.TabIndex = 2
    '
    'pnlButtons
    '
    Me.pnlButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.pnlButtons.Controls.Add(Me.btnCancel)
    Me.pnlButtons.Controls.Add(Me.btnOk)
    Me.pnlButtons.Location = New System.Drawing.Point(8, 488)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.Size = New System.Drawing.Size(184, 24)
    Me.pnlButtons.TabIndex = 12
    '
    'btnCancel
    '
    Me.btnCancel.Location = New System.Drawing.Point(104, 0)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(80, 24)
    Me.btnCancel.TabIndex = 4
    Me.btnCancel.Text = "Cancel"
    '
    'btnOk
    '
    Me.btnOk.Location = New System.Drawing.Point(0, 0)
    Me.btnOk.Name = "btnOk"
    Me.btnOk.Size = New System.Drawing.Size(80, 24)
    Me.btnOk.TabIndex = 3
    Me.btnOk.Text = "Ok"
    '
    'frmSelectTimeseries
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(528, 525)
    Me.Controls.Add(Me.pnlButtons)
    Me.Controls.Add(Me.groupSelected)
    Me.Controls.Add(Me.groupTop)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmSelectTimeseries"
    Me.Text = "Select Timeseries"
    Me.groupTop.ResumeLayout(False)
    CType(Me.gridMatching, System.ComponentModel.ISupportInitialize).EndInit()
    Me.groupSelected.ResumeLayout(False)
    CType(Me.gridSelected, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Const pPad As Integer = 15

  Private pcboCriteria() As Windows.Forms.ComboBox
  Private plstCriteria() As Windows.Forms.ListBox

  Private pTimeseriesManager As atcDataManager

  Private pMatchingTable As System.Data.DataTable
  Private pSelectedTable As System.Data.DataTable

  Private pMatchingTS As atcTimeseriesGroup
  Private pSelectedTS As atcTimeseriesGroup

  Private pSelectedOK As Boolean

  Private pTotalTS As Integer

  Public Function AskUser(ByVal aManager As atcDataManager, Optional ByVal aGroup As atcTimeseriesGroup = Nothing) As atcTimeseriesGroup
    Dim pSaveGroup As atcTimeseriesGroup = Nothing
    If aGroup Is Nothing Then
      pSelectedTS = New atcTimeseriesGroup
    Else
      pSaveGroup = aGroup.Clone
      pSelectedTS = aGroup
    End If
    Populate(aManager)
    Me.ShowDialog()
    If Not pSelectedOK Then 'User clicked Cancel or closed dialog
      pSelectedTS.ChangeTo(pSaveGroup)
    End If
    Return pSelectedTS
  End Function

  Private Sub Populate(ByVal aTimeseriesManager As atcDataManager)
    pMatchingTS = New atcTimeseriesGroup

    pTimeseriesManager = aTimeseriesManager
    ReDim pcboCriteria(0)
    ReDim plstCriteria(0)
    Dim tblSty As Windows.Forms.DataGridTableStyle

    For Each lAttribName As String In pTimeseriesManager.SelectionAttributes
      AddCriteria(lAttribName)
    Next

    pMatchingTable = New System.Data.DataTable
    With pMatchingTable.Columns
      .Add(New System.Data.DataColumn("atcTimeseries"))
      .Item(0).DataType = GetType(atcTimeseries)
      pMatchingTable.PrimaryKey = New Data.DataColumn() {.Item(0)}

      For Each lAttribName As String In pTimeseriesManager.DisplayAttributes
        .Add(New System.Data.DataColumn(lAttribName))
      Next
    End With

    For Each lColumn As System.Data.DataColumn In pMatchingTable.Columns
      lColumn.DataType = GetType(String)
      lColumn.ReadOnly = True
    Next

    gridMatching.SetDataBinding(pMatchingTable, "")
    pSelectedTable = pMatchingTable.Clone
    gridSelected.SetDataBinding(pSelectedTable, "")

    ' Hide the first column of each grid (holds serial #)
    tblSty = New Windows.Forms.DataGridTableStyle
    tblSty.RowHeadersVisible = False
    gridMatching.TableStyles.Add(tblSty)
    gridMatching.TableStyles(0).GridColumnStyles(0).Width = 0

    tblSty = New Windows.Forms.DataGridTableStyle
    tblSty.RowHeadersVisible = False
    gridSelected.TableStyles.Add(tblSty)
    gridSelected.TableStyles(0).GridColumnStyles(0).Width = 0

    PopulateMatching()

    'Populate pSelectedTable from selected group
    For Each ts As atcTimeseries In pSelectedTS
      AddTStoTable(ts, pSelectedTable)
    Next
  End Sub

  'Private Sub PopulateOperations()
  '  Dim lNode As System.Windows.Forms.TreeNode
  '  For Each curPlugin As atcDataPlugin In pManager.TimeseriesDataPlugins
  '    lNode = treeOperations.Nodes.Add(curPlugin.Name)
  '    lNode.Expand()
  '    For Each def As DictionaryEntry In curPlugin.AvailableTimeseriesOperations
  '      lNode.Nodes.Add(def.Value.Name)
  '    Next
  '  Next
  'End Sub

  Private Sub PopulateCriteriaCombos()
    Dim i As Integer
    For i = 0 To pcboCriteria.GetUpperBound(0)
      pcboCriteria(i).Items.Clear()
    Next
    For Each tsFile As atcDataSource In pTimeseriesManager.Files
      For Each ts As atcTimeseries In tsFile.Timeseries
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
    For Each tsFile As atcDataSource In pTimeseriesManager.Files
      For Each ts As atcTimeseries In tsFile.Timeseries
        Dim val As Object = ts.Attributes.GetValue(aAttributeName, Nothing)
        If Not val Is Nothing Then
          If Not aList.Items.Contains(val) Then
            aList.Items.Add(val)
          End If
        End If
      Next
    Next
  End Sub

  Private Sub PopulateMatching()
    Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
    pMatchingTable.Rows.Clear()
    pMatchingTS.Clear()
    pTotalTS = 0
    For Each tsFile As atcDataSource In pTimeseriesManager.Files
      For Each ts As atcTimeseries In tsFile.Timeseries
        pTotalTS += 1
        For iCriteria As Integer = 0 To iLastCriteria
          Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
          If Not attrName Is Nothing Then
            Dim selectedValues As Windows.Forms.ListBox.SelectedObjectCollection = plstCriteria(iCriteria).SelectedItems
            If selectedValues.Count > 0 Then 'none selected = all selected
              Dim attrValue As Object = ts.Attributes.GetValue(attrName)
              If Not selectedValues.Contains(attrValue) Then 'Does not match this criteria
                GoTo NextTS
              End If
            End If
          End If
        Next
        'Matched all criteria, add to matching table
        pMatchingTS.Add(ts)
        AddTStoTable(ts, pMatchingTable)
NextTS:
      Next
    Next
    lblMatching.Text = "Matching Timeseries (" & pMatchingTable.Rows.Count & " of " & pTotalTS & ")"
    gridMatching.Refresh()
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

  Private Sub cboCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Not sender.SelectedItem Is Nothing Then
      PopulateCriteriaList(sender.SelectedItem, plstCriteria(GetIndex(sender.name)))
    End If
  End Sub

  Private Sub lstCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    PopulateMatching()
  End Sub

  Private Sub btnAddAttribute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAttribute.Click
    AddCriteria()
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

    groupTop.Controls.Add(pcboCriteria(iCriteria))
    groupTop.Controls.Add(plstCriteria(iCriteria))

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
        For iOtherCriteria As Integer = 0 To iCriteria - 1
          If curName.Equals(pcboCriteria(iOtherCriteria).SelectedItem) Then GoTo NextName
        Next
        pcboCriteria(iCriteria).Text = curName
        Exit For
NextName:
      Next
    End If

    frmSelectTimeseries_Resize(Nothing, Nothing)

  End Sub

  Private Sub frmSelectTimeseries_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
    If Not pcboCriteria Is Nothing Then
      Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
      Dim perCriteriaWidth As Integer = (btnAddAttribute.Left - pPad) / (iLastCriteria + 1)
      Dim curLeft As Integer = pPad
      For iCriteria As Integer = 0 To iLastCriteria
        pcboCriteria(iCriteria).Top = btnAddAttribute.Top
        pcboCriteria(iCriteria).Left = curLeft
        pcboCriteria(iCriteria).Width = perCriteriaWidth - pPad

        plstCriteria(iCriteria).Top = pcboCriteria(iCriteria).Top + pcboCriteria(iCriteria).Height + pPad
        plstCriteria(iCriteria).Left = curLeft
        plstCriteria(iCriteria).Width = pcboCriteria(iCriteria).Width
        plstCriteria(iCriteria).Height = lblMatching.Top - plstCriteria(iCriteria).Top - pPad

        curLeft += perCriteriaWidth
      Next
    End If
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    pSelectedOK = True

    'Update SelectionAttributes from current set of pcboCriteria
    pTimeseriesManager.SelectionAttributes.Clear()
    For iCriteria As Integer = 0 To pcboCriteria.GetUpperBound(0)
      Dim attrName As String = pcboCriteria(iCriteria).SelectedItem
      If Not attrName Is Nothing Then
        pTimeseriesManager.SelectionAttributes.Add(attrName)
      End If
    Next
    Me.Close()
  End Sub

  Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    pSelectedOK = False
    Me.Close()
  End Sub

  Private Sub gridMatching_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridMatching.MouseDown
    Dim lRow As Integer = gridMatching.HitTest(e.X, e.Y).Row
    If IsNumeric(gridMatching.Item(lRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(gridMatching.Item(lRow, 0)) 'Serial number in clicked row
      Dim iTS As Integer = pSelectedTS.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Already selected, unselect
        pSelectedTS.Remove(iTS)
        pSelectedTable.Rows.Remove(pSelectedTable.Rows.Find(lSerial))
      Else 'Not already selected, select it now
        iTS = pMatchingTS.IndexOfSerial(lSerial)
        If iTS >= 0 Then 'Found matching serial number in pMatchingTS
          Dim selTS As atcData.atcTimeseries = pMatchingTS(iTS)
          pSelectedTS.Add(selTS)
          AddTStoTable(selTS, pSelectedTable)
        End If
      End If
    End If

    groupSelected.Text = "Selected Timeseries (" & pSelectedTable.Rows.Count & " of " & pTotalTS & ")"

  End Sub

  Private Sub gridSelected_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridSelected.MouseDown
    Dim lRow As Integer = gridSelected.HitTest(e.X, e.Y).Row
    If IsNumeric(gridSelected.Item(lRow, 0)) Then 'clicked a row containing a serial number
      Dim lSerial As Integer = CInt(gridSelected.Item(lRow, 0)) 'Serial number in row to be removed
      Dim iTS As Integer = pSelectedTS.IndexOfSerial(lSerial)
      If iTS >= 0 Then 'Found matching serial number in pSelectedTS
        pSelectedTS.Remove(iTS)
        pSelectedTable.Rows.Remove(pSelectedTable.Rows.Find(lSerial))
        groupSelected.Text = "Selected Timeseries (" & pSelectedTable.Rows.Count & " of " & pTotalTS & ")"
      Else
        'TODO: should never reach this line
      End If
    End If
  End Sub

End Class
