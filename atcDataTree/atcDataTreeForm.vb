Imports System.Windows.Forms
Imports atcData
Imports atcUtility
Imports MapWinUtility

Friend Class atcDataTreeForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(Optional ByRef aDataGroup As atcData.atcTimeseriesGroup = Nothing)
        MyBase.New()
        InitializeComponent() 'required by Windows Form Designer

        If aDataGroup Is Nothing Then aDataGroup = New atcTimeseriesGroup

        If aDataGroup.Count = 0 Then 'ask user to specify some Data
            aDataGroup = atcDataManager.UserSelectData(, aDataGroup)
        End If

        If aDataGroup.Count > 0 Then
            pTimeseriesGroup = aDataGroup 'Don't assign to pDataGroup too soon or it may slow down UserSelectData
            PopulateTree()

            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each ldisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = ldisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        Else 'user declined to specify Data
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
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExpand As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCollapse As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDefault As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents mnuDataCount As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShowMore As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShowLess As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShowAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(atcDataTreeForm))
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuExpand = New System.Windows.Forms.MenuItem
        Me.mnuCollapse = New System.Windows.Forms.MenuItem
        Me.mnuDefault = New System.Windows.Forms.MenuItem
        Me.mnuDataCount = New System.Windows.Forms.MenuItem
        Me.mnuShowMore = New System.Windows.Forms.MenuItem
        Me.mnuShowLess = New System.Windows.Forms.MenuItem
        Me.mnuShowAll = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuMain = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSave})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select &Data"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 1
        Me.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSave.Text = "Save"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Index = 0
        Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.mnuEditCopy.Text = "Copy"
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuExpand, Me.mnuCollapse, Me.mnuDefault, Me.mnuDataCount})
        Me.mnuView.Text = "View"
        '
        'mnuExpand
        '
        Me.mnuExpand.Index = 0
        Me.mnuExpand.Text = "Expand"
        '
        'mnuCollapse
        '
        Me.mnuCollapse.Index = 1
        Me.mnuCollapse.Text = "Collapse"
        '
        'mnuDefault
        '
        Me.mnuDefault.Index = 2
        Me.mnuDefault.Text = "Default"
        '
        'mnuDataCount
        '
        Me.mnuDataCount.Index = 3
        Me.mnuDataCount.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuShowMore, Me.mnuShowLess, Me.mnuShowAll})
        Me.mnuDataCount.Text = "Data to Show"
        '
        'mnuShowMore
        '
        Me.mnuShowMore.Index = 0
        Me.mnuShowMore.Text = "More"
        '
        'mnuShowLess
        '
        Me.mnuShowLess.Index = 1
        Me.mnuShowLess.Text = "Less"
        '
        'mnuShowAll
        '
        Me.mnuShowAll.Index = 2
        Me.mnuShowAll.Text = "All"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.MenuItem1, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 1
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopy})
        Me.MenuItem1.Text = "Edit"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 4
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.ShowShortcut = False
        Me.mnuHelp.Text = "Help"
        '
        'atcDataTreeForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(527, 544)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.mnuMain
        Me.Name = "atcDataTreeForm"
        Me.Text = "Data Tree"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents pTreeViewMain As TreeView   'tree control
    Private WithEvents pTimeseriesGroup As atcTimeseriesGroup
    Private pNumValuesShowDefault As Integer = 8
    Private pNumValuesShow As Integer = pNumValuesShowDefault

    Private Sub PopulateTree()
        If Not pTreeViewMain Is Nothing Then
            Me.Controls.Remove(pTreeViewMain)
        End If

        pTreeViewMain = New TreeView
        With pTreeViewMain
            .Visible = False
            '.Font = System.Drawing.Font.  try for courier or other not proportional font here
            .Location = New System.Drawing.Point(0, 0)
            .Name = "atrMain"
            .Size = Me.ClientSize
            .TabIndex = 14
            .Anchor = AnchorStyles.Top _
                   Or AnchorStyles.Bottom _
                   Or AnchorStyles.Left _
                   Or AnchorStyles.Right
            Me.Controls.Add(pTreeViewMain)
            .Refresh()

            For Each lTimeseries As atcTimeseries In pTimeseriesGroup
                lTimeseries.Attributes.CalculateAll() 'be sure to get everything
                Dim lNode As TreeNode = .Nodes.Add(lTimeseries.ToString)

                Dim lAttributeNode As TreeNode = lNode.Nodes.Add("Attributes")
                Dim lComputedNode As TreeNode = lNode.Nodes.Add("Computed")
                Dim lAttributes As SortedList = lTimeseries.Attributes.ValuesSortedByName
                For lAttributeIndex As Integer = 0 To lAttributes.Count - 1
                    Dim lAttributeName As String = lAttributes.GetKey(lAttributeIndex)
                    Dim lAttributeValue As String = lTimeseries.Attributes.GetFormattedValue(lAttributeName)
                    If lTimeseries.Attributes.GetDefinedValue(lAttributeName).Definition.Calculated Then
                        lComputedNode.Nodes.Add(lAttributeName & " : " & lAttributeValue)
                    Else
                        lAttributeNode.Nodes.Add(lAttributeName & " : " & lAttributeValue)
                    End If
                Next
                lAttributeNode.ExpandAll()
                lAttributeNode.EnsureVisible()
                lComputedNode.ExpandAll()

                Dim lInternalNode As TreeNode = lNode.Nodes.Add("Internal")
                Dim lNumValues As Integer = lTimeseries.numValues
                lInternalNode.Nodes.Add("NumValues : " & lNumValues)
                lInternalNode.ExpandAll()
                lInternalNode.EnsureVisible()

                Dim lDataNode As TreeNode = lNode.Nodes.Add("Data")
                Dim lNumValuesNow As Integer
                If lNumValues > pNumValuesShow AndAlso pNumValuesShow <> -1 Then
                    lNumValuesNow = pNumValuesShow
                Else
                    lNumValuesNow = lNumValues
                End If
                Logger.Dbg("NumValuesNow " & lNumValuesNow & " Show " & pNumValuesShow)

                Dim lDateOffset As Integer 'Mean data is labeled with previous date value (lDateOffset = -1)
                If lTimeseries.Attributes.GetValue("Point", False) Then
                    lDateOffset = 0
                Else
                    lDateOffset = -1
                End If
                Logger.Dbg("DateOffset " & lDateOffset)

                Logger.Dbg("AtStart ValueStart 1 NumValues " & lNumValuesNow)
                For lValueIndex As Integer = 1 To lNumValuesNow
                    'data starts at 1
                    ' for non point data, date display is from prev value which is start of interval
                    lDataNode.Nodes.Add(ValueWithAttributes(lTimeseries, lValueIndex, lDateOffset))
                Next

                If lNumValues > pNumValuesShow AndAlso pNumValuesShow <> -1 Then  'some from end too
                    Dim lValueStart As Integer
                    If lNumValues - pNumValuesShow + 1 > pNumValuesShow Then
                        lDataNode.Nodes.Add("  <" & lNumValues - (2 * pNumValuesShow) & " values skipped>")
                        lValueStart = lNumValues - pNumValuesShow + 1
                    Else
                        lValueStart = lNumValuesNow
                    End If
                    Logger.Dbg("AtEnd ValueStart " & lValueStart & " NumValues " & lTimeseries.numValues)
                    For lValueIndex As Integer = lValueStart To lTimeseries.numValues
                        lDataNode.Nodes.Add(ValueWithAttributes(lTimeseries, lValueIndex, lDateOffset))
                    Next
                End If
            Next
            .Visible = True
        End With
    End Sub

    Private Function ValueWithAttributes(ByVal aData As atcTimeseries, _
                                         ByVal aValueIndex As Integer, _
                                         ByVal aDateOffset As Integer) As String
        Dim lJDate As Double = aData.Dates.Value(aValueIndex + aDateOffset)
        Dim lDateString() As String = DumpDate(lJDate).Split(" ")

        Dim lValueAttributes As atcDataAttributes = Nothing
        If aData.ValueAttributesExist Then
            lValueAttributes = aData.ValueAttributes(aValueIndex)
        End If

        Dim lConditional As String = ""
        If lValueAttributes Is Nothing Then
            lConditional = ""
        Else
            lConditional = lValueAttributes.GetValue("Conditional", "")
        End If
        Dim lString As String = lDateString(0) & " " & _
                                lDateString(1) & " : " & _
                                lConditional & DoubleToString(aData.Value(aValueIndex)) & " : " & _
                                DoubleToString(lJDate)

        If Not lValueAttributes Is Nothing Then
            'TODO: add display of any attributes other than conditional - as nodes?
        End If
        Return lString
    End Function

    Friend Sub Save(ByVal aFileName As String)
        If aFileName.Length = 0 Then 'prompt user
            Dim lCdlg As New Windows.Forms.SaveFileDialog
            With lCdlg
                .Title = "Select File to Save Into"
                .Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
                .FilterIndex = 1
                .DefaultExt = "txt"
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    aFileName = AbsolutePath(.FileName, CurDir)
                Else 'Return empty string if user clicked Cancel
                    aFileName = ""
                End If
            End With
        End If

        If aFileName.Length > 0 Then
            SaveFileString(aFileName, ToString)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New Text.StringBuilder

        For lIndexOuter As Integer = 0 To pTreeViewMain.GetNodeCount(False) - 1
            With pTreeViewMain.Nodes(lIndexOuter)
                lSB.Append(.Text)
                If Not .IsExpanded And .GetNodeCount(False) > 0 Then
                    lSB.AppendLine(" ...")
                Else
                    lSB.AppendLine()
                    For lIndexMiddle As Integer = 0 To .GetNodeCount(False) - 1
                        With .Nodes(lIndexMiddle)
                            lSB.Append(vbTab & .Text)
                            If Not .IsExpanded And .GetNodeCount(False) > 0 Then
                                lSB.AppendLine(" ...")
                            Else
                                lSB.AppendLine()
                                For lIndexInner As Integer = 0 To .GetNodeCount(False) - 1
                                    Dim lS As String = ""
                                    Dim lT As String = .Nodes(lIndexInner).Text.Replace(" : ", vbTab)
                                    If lT.IndexOf(vbTab) > 0 Then
                                        Dim lTa() As String = lT.Split(vbTab)
                                        lS &= vbTab & vbTab & lTa(0).PadRight(24) & vbTab & lTa(1).PadRight(16)
                                        If lTa.GetUpperBound(0) > 1 Then
                                            lS &= vbTab & lTa(2)
                                        End If
                                        lSB.AppendLine(lS)
                                    Else
                                        lSB.AppendLine(vbTab & vbTab & .Nodes(lIndexInner).Text)
                                    End If
                                Next
                            End If
                        End With
                    Next
                End If
            End With
        Next
        Return lSB.ToString
    End Function

    Friend Sub TreeAction(ByVal ParamArray aAction() As String)
        Logger.Dbg("TreeActionCount " & aAction.GetUpperBound(0))
        For Each lAction As String In aAction
            Dim lCurAction() As String = lAction.Split(" ")
            Logger.Dbg("Action " & lAction)
            Select Case lCurAction(0)
                Case "Expand"
                    With pTreeViewMain
                        .Visible = False
                        .ExpandAll()
                        .Visible = True
                    End With
                Case "Collapse"
                    With pTreeViewMain
                        .Visible = False
                        .CollapseAll()
                        .Visible = True
                    End With
                Case "Default" : PopulateTree()
                Case "Display"
                    If IsNumeric(lCurAction(1)) Then
                        pNumValuesShow = lCurAction(1)
                        Logger.Dbg("NewNumValuesShow " & pNumValuesShow)
                        PopulateTree()
                    End If
            End Select
        Next
    End Sub

    Private Function GetIndex(ByVal aName As String) As Integer
        Return CInt(Mid(aName, InStr(aName, "#") + 1))
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pTimeseriesGroup)
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData(, pTimeseriesGroup)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pTimeseriesGroup.Added
        PopulateTree()
        'TODO: could efficiently insert newly added item(s)
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pTimeseriesGroup.Removed
        PopulateTree()
        'TODO: could efficiently remove by serial number
    End Sub

    Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
        Save("")
    End Sub

    Private Sub mnuCollapse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCollapse.Click
        TreeAction("Collapse")
    End Sub

    Private Sub mnuExpand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExpand.Click
        TreeAction("Expand")
    End Sub

    Private Sub mnuDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDefault.Click
        TreeAction("Default")
    End Sub

    Private Sub mnuEditCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditCopy.Click
        Clipboard.SetDataObject(ToString)
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pTimeseriesGroup = Nothing
        pTreeViewMain = Nothing
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details\Analysis\Time Series Functions\Data Tree.html")
    End Sub

    Private Sub mnuShowMore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowMore.Click
        If pNumValuesShow <> -1 Then
            pNumValuesShow *= 3
        End If
        ReviseDataCount()
    End Sub

    Private Sub mnuShowLess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuShowLess.Click
        If pNumValuesShow <> -1 Then
            pNumValuesShow /= 3
        Else
            pNumValuesShow = pNumValuesShowDefault
        End If
        ReviseDataCount()
    End Sub

    Private Sub mnuShowAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuShowAll.Click
        pNumValuesShow = -1
        ReviseDataCount()
    End Sub

    Private Sub ReviseDataCount()
        PopulateTree()
        If pNumValuesShow = -1 Then
            mnuDataCount.Text = "Show All Data"
        Else
            mnuDataCount.Text = "Data to Show " & pNumValuesShow
        End If
        TreeAction("Expand")
    End Sub
End Class
