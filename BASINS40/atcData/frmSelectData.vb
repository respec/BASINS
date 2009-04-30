Imports atcControls
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Friend Class frmSelectData
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        pMatchingGrid.AllowHorizontalScrolling = False
        pSelectedGrid.AllowHorizontalScrolling = False
        AddHandler atcDataManager.OpenedData, AddressOf OpenedData

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
    Friend WithEvents mnuAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelect As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributesAdd As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributesRemove As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributesMove As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectAllMatching As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectClear As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectNoMatching As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileManage As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuOpenData As System.Windows.Forms.MenuItem
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents atcSelectedDates As atcData.atcChooseDataGroupDates
    Friend WithEvents mnuSelectSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectMap As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectData))
        Me.groupTop = New System.Windows.Forms.GroupBox
        Me.pMatchingGrid = New atcControls.atcGrid
        Me.lblMatching = New System.Windows.Forms.Label
        Me.splitAboveMatching = New System.Windows.Forms.Splitter
        Me.panelCriteria = New System.Windows.Forms.Panel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.splitAboveSelected = New System.Windows.Forms.Splitter
        Me.groupSelected = New System.Windows.Forms.GroupBox
        Me.pSelectedGrid = New atcControls.atcGrid
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuOpenData = New System.Windows.Forms.MenuItem
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
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.pnlButtons = New System.Windows.Forms.Panel
        Me.atcSelectedDates = New atcData.atcChooseDataGroupDates
        Me.mnuSelectSep1 = New System.Windows.Forms.MenuItem
        Me.mnuSelectMap = New System.Windows.Forms.MenuItem
        Me.groupTop.SuspendLayout()
        Me.groupSelected.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupTop
        '
        Me.groupTop.Controls.Add(Me.pMatchingGrid)
        Me.groupTop.Controls.Add(Me.lblMatching)
        Me.groupTop.Controls.Add(Me.splitAboveMatching)
        Me.groupTop.Controls.Add(Me.panelCriteria)
        resources.ApplyResources(Me.groupTop, "groupTop")
        Me.groupTop.Name = "groupTop"
        Me.groupTop.TabStop = False
        '
        'pMatchingGrid
        '
        Me.pMatchingGrid.AllowHorizontalScrolling = True
        Me.pMatchingGrid.AllowNewValidValues = False
        Me.pMatchingGrid.CellBackColor = System.Drawing.Color.Empty
        resources.ApplyResources(Me.pMatchingGrid, "pMatchingGrid")
        Me.pMatchingGrid.LineColor = System.Drawing.Color.Empty
        Me.pMatchingGrid.LineWidth = 0.0!
        Me.pMatchingGrid.Name = "pMatchingGrid"
        Me.pMatchingGrid.Source = Nothing
        '
        'lblMatching
        '
        resources.ApplyResources(Me.lblMatching, "lblMatching")
        Me.lblMatching.Name = "lblMatching"
        '
        'splitAboveMatching
        '
        resources.ApplyResources(Me.splitAboveMatching, "splitAboveMatching")
        Me.splitAboveMatching.Name = "splitAboveMatching"
        Me.splitAboveMatching.TabStop = False
        '
        'panelCriteria
        '
        resources.ApplyResources(Me.panelCriteria, "panelCriteria")
        Me.panelCriteria.Name = "panelCriteria"
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Name = "btnCancel"
        '
        'btnOk
        '
        resources.ApplyResources(Me.btnOk, "btnOk")
        Me.btnOk.Name = "btnOk"
        '
        'splitAboveSelected
        '
        resources.ApplyResources(Me.splitAboveSelected, "splitAboveSelected")
        Me.splitAboveSelected.Name = "splitAboveSelected"
        Me.splitAboveSelected.TabStop = False
        '
        'groupSelected
        '
        Me.groupSelected.Controls.Add(Me.pSelectedGrid)
        resources.ApplyResources(Me.groupSelected, "groupSelected")
        Me.groupSelected.Name = "groupSelected"
        Me.groupSelected.TabStop = False
        '
        'pSelectedGrid
        '
        Me.pSelectedGrid.AllowHorizontalScrolling = True
        Me.pSelectedGrid.AllowNewValidValues = False
        Me.pSelectedGrid.CellBackColor = System.Drawing.Color.Empty
        resources.ApplyResources(Me.pSelectedGrid, "pSelectedGrid")
        Me.pSelectedGrid.LineColor = System.Drawing.Color.Empty
        Me.pSelectedGrid.LineWidth = 0.0!
        Me.pSelectedGrid.Name = "pSelectedGrid"
        Me.pSelectedGrid.Source = Nothing
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAttributes, Me.mnuSelect, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOpenData, Me.mnuFileManage})
        resources.ApplyResources(Me.mnuFile, "mnuFile")
        '
        'mnuOpenData
        '
        Me.mnuOpenData.Index = 0
        resources.ApplyResources(Me.mnuOpenData, "mnuOpenData")
        '
        'mnuFileManage
        '
        Me.mnuFileManage.Index = 1
        resources.ApplyResources(Me.mnuFileManage, "mnuFileManage")
        '
        'mnuAttributes
        '
        Me.mnuAttributes.Index = 1
        Me.mnuAttributes.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributesAdd, Me.mnuAttributesRemove, Me.mnuAttributesMove})
        resources.ApplyResources(Me.mnuAttributes, "mnuAttributes")
        '
        'mnuAttributesAdd
        '
        Me.mnuAttributesAdd.Index = 0
        resources.ApplyResources(Me.mnuAttributesAdd, "mnuAttributesAdd")
        '
        'mnuAttributesRemove
        '
        Me.mnuAttributesRemove.Index = 1
        resources.ApplyResources(Me.mnuAttributesRemove, "mnuAttributesRemove")
        '
        'mnuAttributesMove
        '
        Me.mnuAttributesMove.Index = 2
        resources.ApplyResources(Me.mnuAttributesMove, "mnuAttributesMove")
        '
        'mnuSelect
        '
        Me.mnuSelect.Index = 2
        Me.mnuSelect.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuSelectAll, Me.mnuSelectClear, Me.mnuSelectAllMatching, Me.mnuSelectNoMatching, Me.mnuSelectSep1, Me.mnuSelectMap})
        resources.ApplyResources(Me.mnuSelect, "mnuSelect")
        '
        'mnuSelectAll
        '
        Me.mnuSelectAll.Index = 0
        resources.ApplyResources(Me.mnuSelectAll, "mnuSelectAll")
        '
        'mnuSelectClear
        '
        Me.mnuSelectClear.Index = 1
        resources.ApplyResources(Me.mnuSelectClear, "mnuSelectClear")
        '
        'mnuSelectAllMatching
        '
        Me.mnuSelectAllMatching.Index = 2
        resources.ApplyResources(Me.mnuSelectAllMatching, "mnuSelectAllMatching")
        '
        'mnuSelectNoMatching
        '
        Me.mnuSelectNoMatching.Index = 3
        resources.ApplyResources(Me.mnuSelectNoMatching, "mnuSelectNoMatching")
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 3
        resources.ApplyResources(Me.mnuHelp, "mnuHelp")
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.atcSelectedDates)
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnOk)
        resources.ApplyResources(Me.pnlButtons, "pnlButtons")
        Me.pnlButtons.Name = "pnlButtons"
        '
        'atcSelectedDates
        '
        resources.ApplyResources(Me.atcSelectedDates, "atcSelectedDates")
        Me.atcSelectedDates.CommonEnd = 1.7976931348623157E+308
        Me.atcSelectedDates.CommonStart = -1.7976931348623157E+308
        Me.atcSelectedDates.DataGroup = Nothing
        Me.atcSelectedDates.FirstStart = 1.7976931348623157E+308
        Me.atcSelectedDates.LastEnd = -1.7976931348623157E+308
        Me.atcSelectedDates.Name = "atcSelectedDates"
        Me.atcSelectedDates.OmitAfter = 0
        Me.atcSelectedDates.OmitBefore = 0
        '
        'mnuSelectSep1
        '
        Me.mnuSelectSep1.Index = 4
        resources.ApplyResources(Me.mnuSelectSep1, "mnuSelectSep1")
        '
        'mnuSelectMap
        '
        Me.mnuSelectMap.Checked = True
        Me.mnuSelectMap.Index = 5
        resources.ApplyResources(Me.mnuSelectMap, "mnuSelectMap")
        '
        'frmSelectData
        '
        Me.AcceptButton = Me.btnOk
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.groupSelected)
        Me.Controls.Add(Me.splitAboveSelected)
        Me.Controls.Add(Me.pnlButtons)
        Me.Controls.Add(Me.groupTop)
        Me.KeyPreview = True
        Me.Menu = Me.MainMenu1
        Me.Name = "frmSelectData"
        Me.groupTop.ResumeLayout(False)
        Me.groupSelected.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Const LAYOUT_PADDING As Integer = 5
    Private Const NOTHING_VALUE As String = "~Missing~"
    Private Const CALCULATED_LABEL As String = "Calculated Attributes:"
    Private Const BLANK_LABEL As String = ""

    Private pcboCriteria() As Windows.Forms.ComboBox
    Private plstCriteria() As atcGrid
    Private pCriteriaFraction() As Single
    Private pCriteriaSearch As String = "" 'recently typed characters of incremental search in criteria list

    Private pMatchingGroup As atcTimeseriesGroup
    Private WithEvents pSelectedGroup As atcTimeseriesGroup
    Private pSaveGroup As atcTimeseriesGroup = Nothing

    Private pMatchingSource As GridSource
    Private pSelectedSource As GridSource

    Private pInitializing As Boolean = False
    Private pPopulatingMatching As Boolean = False
    Private pRestartPopulatingMatching As Boolean = False

    Private pSelectedOK As Boolean = False
    Private pRevertedToSaved As Boolean = False

    Private pTotalTS As Integer = 0

    Private pAvailableData As atcTimeseriesGroup = Nothing

    ''' <summary>
    ''' The datasets available for selection. 
    ''' Set this property before calling AskUser or by default all datasets in all open data sources will be available.
    ''' </summary>
    Public Property AvailableData() As atcTimeseriesGroup
        Get
            If pAvailableData Is Nothing Then
                pAvailableData = atcDataManager.DataSets
            End If
            Return pAvailableData
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            pAvailableData = newValue
        End Set
    End Property

    Public Function AskUser(Optional ByVal aGroup As atcTimeseriesGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcTimeseriesGroup
        mnuSelectMap.Checked = False

        If aGroup Is Nothing Then
            pSelectedGroup = New atcTimeseriesGroup
        Else
            pSaveGroup = aGroup.Clone
            pSelectedGroup = aGroup
            groupSelected.Text = "Selected Data (" & pSelectedGroup.Count & ")"
        End If

        atcSelectedDates.DataGroup = pSelectedGroup

        pMatchingGroup = New atcTimeseriesGroup
        pMatchingSource = New GridSource(pMatchingGroup)
        pMatchingSource.SelectedItems = pSelectedGroup
        pSelectedSource = New GridSource(pSelectedGroup)

        pMatchingGrid.Initialize(pMatchingSource)
        pSelectedGrid.Initialize(pSelectedSource)

        mnuSelectMap.Checked = GetSetting("BASINS", "Select Data", "SelectMap", "True").ToLower <> "false"

        Me.Show()
        Populate()

        Dim lCriteriaIndex As Integer = 0
        While Me.Visible AndAlso lCriteriaIndex < pcboCriteria(0).Items.Count
            Dim lAttributeName As String = pcboCriteria(0).Items(lCriteriaIndex)
            Select Case lAttributeName
                Case BLANK_LABEL, CALCULATED_LABEL
                    lCriteriaIndex = pcboCriteria(0).Items.Count
                Case Else
                    Dim lHasValues As Boolean = False
                    For Each ts As atcDataSet In AvailableData
                        Application.DoEvents()
                        If ts.Attributes.ContainsAttribute(lAttributeName) Then
                            lHasValues = True
                            Exit For
                        End If
                    Next

                    If Not lHasValues Then
                        Logger.Dbg("Removing " & lAttributeName)
                        For lComboIndex As Integer = 0 To pcboCriteria.GetUpperBound(0)
                            pcboCriteria(lComboIndex).Items.RemoveAt(lCriteriaIndex)
                        Next
                    Else
                        Logger.Dbg("Keeping " & lAttributeName)
                        lCriteriaIndex += 1
                    End If
            End Select
        End While

        If aModal Then
            While Me.Visible
                Application.DoEvents()
                Threading.Thread.Sleep(10)
            End While
            If pSelectedOK Then
                If Not atcSelectedDates.SelectedAll Then 'Change to date subset if needed
                    pSelectedGroup.ChangeTo(atcSelectedDates.CreateSelectedDataGroupSubset)
                End If
            Else 'User clicked Cancel or closed dialog
                If Not pRevertedToSaved Then pSelectedGroup.ChangeTo(pSaveGroup)
            End If
            Try
                Me.Close()
            Catch
            End Try
            Return pSelectedGroup
        Else
            Return Nothing
        End If
    End Function

    Private Sub Populate()
        pInitializing = True

        If Not pcboCriteria Is Nothing Then
            Try
                For iCriteria As Integer = pcboCriteria.GetUpperBound(0) To 0 Step -1
                    RemoveCriteria(pcboCriteria(iCriteria), plstCriteria(iCriteria))
                Next
            Catch ex As Exception
                'first time through there is nothing to remove, error is normal
            End Try
        End If

        ReDim pcboCriteria(0)
        ReDim plstCriteria(0)
        ReDim pCriteriaFraction(0)

        For Each lAttribName As String In atcDataManager.SelectionAttributes
            AddCriteria(lAttribName)
        Next

        pInitializing = False
        SizeCriteria()
        Application.DoEvents()
        UpdatedCriteria()
    End Sub

    Private Sub PopulateCriteriaCombos()
        Dim lCalculatedItems As New atcCollection
        Dim lNotCalculatedItems As New atcCollection

        Dim i As Integer
        For i = 0 To pcboCriteria.GetUpperBound(0)
            pcboCriteria(i).Items.Clear()
        Next
        Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
        Dim lName As String
        Dim lItemIndex As Integer
        If Not lAllDefinitions Is Nothing Then
            For Each def As atcAttributeDefinition In lAllDefinitions.Clone
                If atcDataAttributes.IsSimple(def) Then
                    lName = def.Name
                    Select Case lName.ToLower
                        Case "attributes", "bins", "compfg", "constant coefficient", "degrees f", "headercomplete", "highflag", "kendall tau", "number", "vbtime", "%*", "%sum*"
                        Case Else
                            If def.Calculated Then
                                lItemIndex = lCalculatedItems.BinarySearchForKey(lName)
                                If lItemIndex = lCalculatedItems.Count OrElse lCalculatedItems.Keys.Item(lItemIndex) <> lName Then
                                    lCalculatedItems.Insert(lItemIndex, lName, lName)
                                End If
                            Else
                                'Dim lAttributeValues As atcCollection = AvailableData.SortedAttributeValues(lName, NOTHING_VALUE)
                                'If lAttributeValues.Count > 1 OrElse (lAttributeValues.Count = 1 AndAlso Not lAttributeValues.Item(0).Equals(NOTHING_VALUE)) Then
                                lItemIndex = lNotCalculatedItems.BinarySearchForKey(lName)
                                If lItemIndex = lNotCalculatedItems.Count OrElse lNotCalculatedItems.Keys.Item(lItemIndex) <> lName Then
                                    lNotCalculatedItems.Insert(lItemIndex, lName, lName)
                                End If
                                'End If
                            End If
                    End Select
                End If
            Next
            For Each lName In lNotCalculatedItems
                For i = 0 To pcboCriteria.GetUpperBound(0)
                    pcboCriteria(i).Items.Add(lName)
                Next
            Next
            If lCalculatedItems.Count > 0 Then
                For i = 0 To pcboCriteria.GetUpperBound(0)
                    pcboCriteria(i).Items.Add(BLANK_LABEL)
                    pcboCriteria(i).Items.Add(CALCULATED_LABEL)
                Next
                For Each lName In lCalculatedItems
                    For i = 0 To pcboCriteria.GetUpperBound(0)
                        pcboCriteria(i).Items.Add(lName)
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub PopulateCriteriaList(ByVal aAttributeName As String, ByVal aList As atcGrid)
        'Logger.Dbg("Start Populating Criteria List for " & aAttributeName)
        Dim lDefinition As atcAttributeDefinition = atcDataAttributes.GetDefinition(aAttributeName)
        Dim lSortedItems As atcCollection
        Dim lNumeric As Boolean = False
        If lDefinition Is Nothing Then
            lSortedItems = New atcCollection
        Else
            lNumeric = atcDataAttributes.GetDefinition(aAttributeName).IsNumeric
            Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(aAttributeName)

            If lAttributeDef Is Nothing Then
                lSortedItems = New atcCollection
            Else
                aList.Visible = False
                lSortedItems = AvailableData.SortedAttributeValues(aAttributeName, NOTHING_VALUE)
            End If
        End If

        With aList
            .Initialize(New ListSource(lSortedItems))
            If lNumeric Then
                .Source.Alignment(0, 0) = atcAlignment.HAlignDecimal
            Else
                .Source.Alignment(0, 0) = atcAlignment.HAlignLeft
            End If
            .Visible = True
            .Refresh()
        End With

        'Logger.Dbg("Finished PopulateCriteriaList(" & aAttributeName & ")")
    End Sub

    Private Sub PopulateMatching()
        If pPopulatingMatching Then
            'Already have a thread doing this, tell it to start over
            pRestartPopulatingMatching = True
        Else
            Dim lSaveCursor As Cursor = Me.Cursor
            pPopulatingMatching = True
            Dim lAllDatasets As atcTimeseriesGroup = AvailableData
            pTotalTS = lAllDatasets.Count
            Logger.Status("Matching " & Format(pTotalTS, "#,###") & " timeseries")
Restart:
            Try
                Dim attrName As String
                Dim selectedValues As atcCollection
                Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
                Dim lCriteriaSelectedItems(iLastCriteria) As atcCollection
                Dim lCriteriaName(iLastCriteria) As String
                'Find attribute names that have selected values to match
                For iCriteria As Integer = 0 To iLastCriteria
                    attrName = pcboCriteria(iCriteria).SelectedItem
                    Select Case attrName
                        Case Nothing, CALCULATED_LABEL, BLANK_LABEL
                            'can't use this criteria
                        Case Else
                            selectedValues = CType(plstCriteria(iCriteria).Source, ListSource).SelectedItems
                            If selectedValues.Count > 0 Then 'none selected = all selected
                                lCriteriaName(iCriteria) = attrName
                                lCriteriaSelectedItems(iCriteria) = selectedValues
                            End If
                    End Select
                Next

                pRestartPopulatingMatching = False
                'Dim lTimeStart As Date = Date.Now
                Me.Cursor = Cursors.WaitCursor
                pMatchingGroup.Clear()
                Dim lCount As Integer = 0
                Dim lNextProgress As Integer = -1
                'Dim selectedValues As atcCollection = CType(plstCriteria(1).Source, ListSource).SelectedItems
                For Each ts As atcDataSet In lAllDatasets
                    For iCriteria As Integer = 0 To iLastCriteria
                        attrName = lCriteriaName(iCriteria)
                        If attrName IsNot Nothing Then
                            selectedValues = lCriteriaSelectedItems(iCriteria)
                            Dim attrValue As String = ts.Attributes.GetFormattedValue(attrName, NOTHING_VALUE)
                            If Not selectedValues.Contains(attrValue) Then 'Does not match this criteria
                                GoTo NextTS
                            End If
                        End If
                    Next
                    'Matched all criteria, add to matching table
                    pMatchingGroup.Add(ts)
                    SelectMatchingRow(pMatchingGroup.Count, pSelectedGroup.Contains(ts))
NextTS:
                    lCount += 1
                    If lCount = 20 Then pMatchingGrid.Refresh() 'Show the first few matches while finding the rest

                    If lCount > lNextProgress Then
                        Logger.Progress(lCount, pTotalTS)
                        lNextProgress += 10
                    End If
                    If pRestartPopulatingMatching Then
                        Logger.Dbg("Restarting PopulateMatching")
                        GoTo Restart
                    End If
                Next
                Logger.Progress("", pTotalTS, pTotalTS)
                lblMatching.Text = "Matching Data (" & pMatchingGroup.Count & " of " & pTotalTS & ")"
                pMatchingGrid.Refresh()
                'Logger.Dbg("PopulateMatching " & (Date.Now - lTimeStart).TotalSeconds)
            Catch ex As Exception 'Catch anything so we are sure to clear pPopulatingMatching and restore cursor
                Me.Cursor = lSaveCursor
                pPopulatingMatching = False
                'Throw New ApplicationException("Exception while populating matching: " & ex.Message, ex)
                Logger.Dbg("Exception while populating matching: " & ex.Message & vbCrLf & ex.StackTrace)
            End Try
            Me.Cursor = lSaveCursor
            pPopulatingMatching = False
        End If
    End Sub

    Private Function GetIndex(ByVal aName As String) As Integer
        Return CInt(Mid(aName, InStr(aName, "#") + 1))
    End Function

    Private Sub cboCriteria_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not sender.SelectedItem Is Nothing Then
            PopulateCriteriaList(sender.SelectedItem, plstCriteria(GetIndex(sender.name)))
            UpdatedCriteria()
        End If
    End Sub

    Private Sub lstCriteria_GotFocus(ByVal aSource As Object, ByVal e As System.EventArgs)
        pCriteriaSearch = ""
    End Sub

    ''' <summary>
    ''' Scroll list to show first item whose beginning matches what the user is typing
    ''' </summary>
    ''' <param name="aGrid">atcGrid to scroll</param>
    ''' <param name="e">KeyDown Event Args</param>
    ''' <remarks>Typing more than one character in a row is supported. 
    ''' If an item is not found that starts with the multiple typed characters, 
    ''' search is reset to find an item starting with just the most recently typed character.
    ''' Pressing a key for a non-printable character (for example Backspace) resets search.</remarks>
    Private Sub lstCriteria_KeyDownGrid(ByVal aGrid As atcGrid, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            Select Case e.KeyCode
                Case Keys.Space To Keys.Z
                    Dim lSource As ListSource = aGrid.Source
                    Dim lMatchRow As Integer = -1
                    pCriteriaSearch &= Chr(e.KeyCode)
                    While lMatchRow < 0 AndAlso pCriteriaSearch.Length > 0
                        For lRow As Integer = lSource.FixedRows To lSource.Rows
                            If lSource.CellValue(lRow, 0).ToUpper.StartsWith(pCriteriaSearch) Then
                                lMatchRow = lRow
                                Exit For
                            End If
                        Next
                        If lMatchRow >= 0 Then
                            aGrid.EnsureRowVisible(lMatchRow)
                        ElseIf pCriteriaSearch.Length > 1 Then
                            pCriteriaSearch = Chr(e.KeyCode) 'Start a new search with just most recently typed character
                        Else
                            pCriteriaSearch = ""
                        End If
                    End While

                Case Else
                    pCriteriaSearch = ""
            End Select
        Catch ex As Exception
            Logger.Dbg("Exception trying to ensure criteria visible: " & ex.Message)
        End Try
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
        RemoveHandler cbo.SelectedValueChanged, AddressOf cboCriteria_SelectedIndexChanged
        RemoveHandler lst.MouseDownCell, AddressOf lstCriteria_MouseDownCell
        RemoveHandler lst.KeyDownGrid, AddressOf lstCriteria_KeyDownGrid
        RemoveHandler lst.GotFocus, AddressOf lstCriteria_GotFocus


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
        AddHandler plstCriteria(iCriteria).KeyDownGrid, AddressOf lstCriteria_KeyDownGrid
        AddHandler plstCriteria(iCriteria).GotFocus, AddressOf lstCriteria_GotFocus

        With pcboCriteria(iCriteria)
            .Name = "cboCriteria#" & iCriteria
            .DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
            .MaxDropDownItems = 40
        End With

        With plstCriteria(iCriteria)
            .Name = "lstCriteria#" & iCriteria
            .AllowHorizontalScrolling = False
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
                If atcDataAttributes.GetDefinition(curName).Calculated Then GoTo NextName
                pcboCriteria(iCriteria).Text = curName
                Exit For
NextName:
            Next
        End If
    End Sub

    Private Sub panelCriteria_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles panelCriteria.SizeChanged
        SizeCriteria()
    End Sub

    Private Sub ResizeOneCriteria(ByVal aCriteria As Integer, ByVal aWidth As Integer)
        Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
        Dim lWidth As Integer = aWidth - LAYOUT_PADDING
        pcboCriteria(aCriteria).Width = lWidth
        pCriteriaFraction(aCriteria) = lWidth / (panelCriteria.Width - LAYOUT_PADDING)
        plstCriteria(aCriteria).Width = lWidth
        plstCriteria(aCriteria).ColumnWidth(0) = lWidth
        While aCriteria < iLastCriteria
            aCriteria += 1
            pcboCriteria(aCriteria).Left = pcboCriteria(aCriteria - 1).Left + pcboCriteria(aCriteria - 1).Width + LAYOUT_PADDING
            plstCriteria(aCriteria).Left = pcboCriteria(aCriteria).Left
        End While

        'Fit rightmost criteria to fill remaining space
        Dim availableWidth As Integer = panelCriteria.Width - LAYOUT_PADDING * 2
        If pcboCriteria(iLastCriteria).Left < availableWidth Then
            lWidth = availableWidth - pcboCriteria(iLastCriteria).Left
            pcboCriteria(iLastCriteria).Width = lWidth
            plstCriteria(iLastCriteria).Width = lWidth
            plstCriteria(iLastCriteria).ColumnWidth(0) = lWidth
        End If
    End Sub

    Private Sub SizeCriteria()
        If Visible AndAlso Not pcboCriteria Is Nothing Then
            Dim iLastCriteria As Integer = pcboCriteria.GetUpperBound(0)
            If iLastCriteria >= 0 Then
                Dim availableWidth As Integer = panelCriteria.Width
                'Dim perCriteriaWidth As Integer = (panelCriteria.Width - LAYOUT_PADDING) / (iLastCriteria + 1)
                Dim curLeft As Integer = 0

                pMatchingGrid.ColumnWidth(0) = 0
                pSelectedGrid.ColumnWidth(0) = 0

                For iCriteria As Integer = 0 To iLastCriteria
                    pcboCriteria(iCriteria).Top = LAYOUT_PADDING
                    pcboCriteria(iCriteria).Left = curLeft
                    If iCriteria = iLastCriteria AndAlso curLeft < availableWidth Then
                        pcboCriteria(iCriteria).Width = availableWidth - curLeft 'Rightmost criteria fills remaining space
                    Else
                        If availableWidth * pCriteriaFraction(iCriteria) > LAYOUT_PADDING * 2 Then
                            pcboCriteria(iCriteria).Width = availableWidth * pCriteriaFraction(iCriteria) - LAYOUT_PADDING
                        Else
                            pcboCriteria(iCriteria).Width = LAYOUT_PADDING
                        End If
                    End If

                    With plstCriteria(iCriteria)
                        .Top = pcboCriteria(iCriteria).Top + pcboCriteria(iCriteria).Height + LAYOUT_PADDING
                        .Left = curLeft
                        .Width = pcboCriteria(iCriteria).Width
                        .ColumnWidth(0) = .Width
                        .Height = panelCriteria.Height - .Top - LAYOUT_PADDING
                        .Visible = True
                        .BringToFront()
                        .Refresh()
                    End With

                    curLeft = pcboCriteria(iCriteria).Left + pcboCriteria(iCriteria).Width + LAYOUT_PADDING

                    pMatchingGrid.ColumnWidth(iCriteria + 1) = pcboCriteria(iCriteria).Width + LAYOUT_PADDING
                    pSelectedGrid.ColumnWidth(iCriteria + 1) = pMatchingGrid.ColumnWidth(iCriteria + 1)
                Next
                pMatchingGrid.Refresh()
                pSelectedGrid.Refresh()
            End If
        End If
    End Sub

    Public Property SelectedOk() As Boolean
        Get
            Return pSelectedOK
        End Get
        Set(ByVal newValue As Boolean)
            pSelectedOK = newValue
        End Set
    End Property

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'If user didn't select anything, 
        ' but either narrowed the matching group or there are not more than 10 datasets,
        ' assume they meant to select all the matching datasets
        If pSelectedGroup.Count = 0 AndAlso _
          (pMatchingGroup.Count < AvailableData.Count OrElse pMatchingGroup.Count < 11) Then
            pSelectedGroup.ChangeTo(pMatchingGroup)
        End If
        pSelectedOK = True
        Me.Visible = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pSelectedOK = False
        pRevertedToSaved = True
        pSelectedGroup.ChangeTo(pSaveGroup)
        Me.Visible = False
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
            atcDataManager.SelectionAttributes.Clear()
            atcDataManager.SelectionAttributes.AddRange(curAttributes)
        End If
    End Sub

    Private Sub SelectMatchingRow(ByVal aRow As Integer, ByVal aSelect As Boolean)
        If pMatchingSource IsNot Nothing Then
            For iColumn As Integer = 0 To pMatchingSource.Columns - 1
                pMatchingSource.CellSelected(aRow, iColumn) = aSelect
            Next
        End If
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
                    pSelectedGroup.Add(selTS)
                    SelectMatchingRow(aRow, True)
                End If
            End If
        End If
    End Sub

    Private Sub pSelectedGrid_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles pSelectedGrid.MouseDownCell
        If IsNumeric(pSelectedSource.CellValue(aRow, 0)) Then 'clicked a row containing a serial number
            Dim lSerial As Integer = CInt(pSelectedSource.CellValue(aRow, 0)) 'Serial number in row to be removed
            Dim iTS As Integer = pSelectedGroup.IndexOfSerial(lSerial)
            If iTS >= 0 Then 'Found matching serial number in pSelectedGroup
                pSelectedGroup.RemoveAt(iTS)
            Else
                'TODO: should never reach this line
            End If
        End If
    End Sub

    Private Sub OpenedData(ByVal aDataSource As atcTimeseriesSource)
        Populate()
    End Sub

    Private Sub pMatchingGrid_UserResizedColumn(ByVal aGrid As atcGrid, ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pMatchingGrid.UserResizedColumn
        pSelectedGrid.ColumnWidth(aColumn) = aWidth
        pSelectedGrid.Refresh()
        ResizeOneCriteria(aColumn - 1, aWidth)
    End Sub

    Private Sub pSelectedGrid_UserResizedColumn(ByVal aGrid As atcGrid, ByVal aColumn As Integer, ByVal aWidth As Integer) Handles pSelectedGrid.UserResizedColumn
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
        End If
    End Sub

    Private Sub mnuSelectAllMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAllMatching.Click
        'For Each ts As atcDataSet In pMatchingGroup
        '  If Not pSelectedGroup.Contains(ts) Then pSelectedGroup.Add(ts)
        'Next
        Dim lAdd As New atcCollection
        For Each ts As atcDataSet In pMatchingGroup
            If Not pSelectedGroup.Contains(ts) Then lAdd.Add(ts)
        Next
        If lAdd.Count > 0 Then
            pSelectedGroup.Add(lAdd)
        End If
    End Sub

    Private Sub mnuSelectNoMatching_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectNoMatching.Click
        Dim lRemove As New atcCollection
        For Each ts As atcDataSet In pMatchingGroup
            If pSelectedGroup.Contains(ts) Then lRemove.Add(ts)
        Next
        If lRemove.Count > 0 Then
            pSelectedGroup.Remove(lRemove)
        End If
    End Sub

    Private Sub mnuFileManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileManage.Click
        atcDataManager.UserManage() ' .OpenData("")
    End Sub

    Private Sub mnuRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mnu As MenuItem = sender
        Dim index As Integer = mnu.Index
        RemoveCriteria(pcboCriteria(index), plstCriteria(index))
    End Sub

    Private Sub mnuMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'TODO: re-order criteria
    End Sub

    Private Sub mnuOpenData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenData.Click
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcTimeseriesSource = atcDataManager.UserSelectDataSource(lFilesOnly)
        If Not lNewSource Is Nothing Then
            atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)
            pAvailableData.AddRange(lNewSource.DataSets)
            Me.Populate()
        End If
    End Sub

    Private Sub mnuSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSelectAll.Click
        pSelectedGroup.Clear()
        pSelectedGroup.Add(AvailableData)
    End Sub

    Private Sub frmSelectData_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Analysis\Time Series Functions.html")
        End If
    End Sub

    Private Sub frmSelectData_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Visible Then SizeCriteria()
    End Sub

    Private Sub frmSelectData_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        RemoveHandler atcDataManager.OpenedData, AddressOf OpenedData
        pMatchingGroup = Nothing
        pSaveGroup = Nothing
        pMatchingSource = Nothing
        pSelectedSource = Nothing
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions.html")
    End Sub

    Private Sub pSelectedGroup_Changed(ByVal aAdded As atcUtility.atcCollection) Handles pSelectedGroup.Added, pSelectedGroup.Removed
        pMatchingGrid.Refresh()
        pSelectedGrid.Refresh()
        If pTotalTS > 0 Then
            groupSelected.Text = "Selected Data (" & pSelectedGroup.Count & " of " & pTotalTS & ")"
        Else
            groupSelected.Text = "Selected Data (" & pSelectedGroup.Count & ")"
        End If
        Try
            If mnuSelectMap.Checked Then atcDataManager.SelectLocationsOnMap(pSelectedGroup.SortedAttributeValues("Location"), True)
        Catch
        End Try
    End Sub

    Private Sub mnuSelectMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuSelectMap.Click
        SaveSetting("BASINS", "Select Data", "SelectMap", CStr(mnuSelectMap.Checked))
    End Sub
End Class

Friend Class GridSource
    Inherits atcControls.atcGridSource

    ' 0 to label the columns in row 0
    '-1 to not label columns
    Private Const LabelRow As Integer = -1

    Private pDataGroup As atcTimeseriesGroup
    Private pSelected As atcCollection

    Public Property SelectedItems() As atcCollection
        Get
            Return pSelected
        End Get
        Set(ByVal newValue As atcCollection)
            pSelected = newValue
        End Set
    End Property

    Sub New(ByVal aDataGroup As atcData.atcTimeseriesGroup)
        pDataGroup = aDataGroup
    End Sub

    Overrides Property Columns() As Integer
        Get
            Return atcDataManager.SelectionAttributes.Count() + 1
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            Return pDataGroup.Count + LabelRow + 1
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If aRow = LabelRow Then
                If aColumn = 0 Then
                    Return ""
                Else
                    Return atcDataManager.SelectionAttributes(aColumn - 1)
                End If
            ElseIf aColumn = 0 Then
                Return pDataGroup(aRow - (LabelRow + 1)).Serial()
            Else
                Return pDataGroup(aRow - (LabelRow + 1)).Attributes.GetFormattedValue(atcDataManager.SelectionAttributes(aColumn - 1))
            End If
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            If aRow > LabelRow AndAlso aColumn > 0 Then
                Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(atcDataManager.SelectionAttributes(aColumn - 1))
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

    Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
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

    Overrides Property Columns() As Integer
        Get
            Return 1
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            If pValues Is Nothing Then Return 1
            Return pValues.Count
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
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

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            Return pAlignment
        End Get
        Set(ByVal newValue As atcControls.atcAlignment)
            pAlignment = newValue
        End Set
    End Property

    'Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
    '  Get
    '    If pSelected.Contains(CellValue(aRow, aColumn)) Then
    '      Return System.Drawing.SystemColors.Highlight
    '    Else
    '      Return System.Drawing.SystemColors.Window 'TODO: use grid's CellBackColor
    '    End If
    '  End Get
    '  Set(ByVal Value As System.Drawing.Color)
    '  End Set
    'End Property

    Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Return pSelected.Keys.Contains(aRow) ' & "," & aColumn)
        End Get
        Set(ByVal newValue As Boolean)
            If newValue Then
                If Not pSelected.Keys.Contains(aRow) Then
                    pSelected.Add(aRow, CellValue(aRow, aColumn))
                End If
            Else
                pSelected.RemoveByKey(aRow)
            End If
        End Set
    End Property
End Class