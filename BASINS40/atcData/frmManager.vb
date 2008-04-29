Imports System.Windows.Forms
Imports System.Drawing
Imports atcUtility
Imports MapWinUtility

Friend Class frmManager
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        AddHandler atcDataManager.OpenedData, AddressOf ChangedData
        AddHandler atcDataManager.ClosedData, AddressOf ChangedData
        'treeFiles.DrawMode = TreeViewDrawMode.Normal
        treeFiles.DrawMode = TreeViewDrawMode.OwnerDrawText

        Dim lDisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDataDisplay As atcDataDisplay In lDisplayPlugins
            Dim lMenuText As String = lDataDisplay.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            AnalysisToolStripMenuItem.DropDownItems.Add(lMenuText, Nothing, New EventHandler(AddressOf AnalysisToolStripMenuItem_Click))
        Next
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            RemoveHandler atcDataManager.OpenedData, AddressOf ChangedData
            RemoveHandler atcDataManager.ClosedData, AddressOf ChangedData
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
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IndexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnalysisToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisplayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NativeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddDatasetsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RenumberDatasetsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveDatasetsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents treeFiles As System.Windows.Forms.TreeView
    'Friend WithEvents panelOpening As System.Windows.Forms.Panel
    'Friend WithEvents lstDataSourceType As System.Windows.Forms.ListBox
    'Friend WithEvents lblDataSourceType As System.Windows.Forms.Label
    'Friend WithEvents btnOpen As System.Windows.Forms.Button
    'Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManager))
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.treeFiles = New System.Windows.Forms.TreeView
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NativeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AnalysisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddDatasetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RenumberDatasetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveDatasetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDetails
        '
        Me.txtDetails.AllowDrop = True
        Me.txtDetails.BackColor = System.Drawing.SystemColors.ControlLight
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtDetails.Location = New System.Drawing.Point(0, 225)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.Size = New System.Drawing.Size(658, 84)
        Me.txtDetails.TabIndex = 1
        '
        'treeFiles
        '
        Me.treeFiles.AllowDrop = True
        Me.treeFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treeFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treeFiles.Location = New System.Drawing.Point(0, 29)
        Me.treeFiles.Name = "treeFiles"
        Me.treeFiles.Size = New System.Drawing.Size(658, 190)
        Me.treeFiles.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.AnalysisToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(658, 26)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.toolStripSeparator, Me.SaveAsToolStripMenuItem, Me.toolStripSeparator1, Me.CloseToolStripMenuItem, Me.toolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(40, 22)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(189, 6)
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Image = CType(resources.GetObject("SaveAsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveAsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(189, 6)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'toolStripSeparator2
        '
        Me.toolStripSeparator2.Name = "toolStripSeparator2"
        Me.toolStripSeparator2.Size = New System.Drawing.Size(189, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDatasetsToolStripMenuItem, Me.RenumberDatasetsToolStripMenuItem, Me.RemoveDatasetsToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(43, 22)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayToolStripMenuItem, Me.NativeToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(49, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'DisplayToolStripMenuItem
        '
        Me.DisplayToolStripMenuItem.Name = "DisplayToolStripMenuItem"
        Me.DisplayToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.DisplayToolStripMenuItem.Text = "Display"
        '
        'NativeToolStripMenuItem
        '
        Me.NativeToolStripMenuItem.Name = "NativeToolStripMenuItem"
        Me.NativeToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.NativeToolStripMenuItem.Text = "Native"
        '
        'AnalysisToolStripMenuItem
        '
        Me.AnalysisToolStripMenuItem.Name = "AnalysisToolStripMenuItem"
        Me.AnalysisToolStripMenuItem.Size = New System.Drawing.Size(71, 22)
        Me.AnalysisToolStripMenuItem.Text = "Analysis"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.IndexToolStripMenuItem, Me.SearchToolStripMenuItem, Me.toolStripSeparator5, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(48, 22)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'ContentsToolStripMenuItem
        '
        Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
        Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ContentsToolStripMenuItem.Text = "Contents"
        '
        'IndexToolStripMenuItem
        '
        Me.IndexToolStripMenuItem.Name = "IndexToolStripMenuItem"
        Me.IndexToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.IndexToolStripMenuItem.Text = "Index"
        '
        'SearchToolStripMenuItem
        '
        Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
        Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.SearchToolStripMenuItem.Text = "Search"
        '
        'toolStripSeparator5
        '
        Me.toolStripSeparator5.Name = "toolStripSeparator5"
        Me.toolStripSeparator5.Size = New System.Drawing.Size(146, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'AddDatasetsToolStripMenuItem
        '
        Me.AddDatasetsToolStripMenuItem.Name = "AddDatasetsToolStripMenuItem"
        Me.AddDatasetsToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.AddDatasetsToolStripMenuItem.Text = "Add Datasets"
        '
        'RenumberDatasetsToolStripMenuItem
        '
        Me.RenumberDatasetsToolStripMenuItem.Name = "RenumberDatasetsToolStripMenuItem"
        Me.RenumberDatasetsToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.RenumberDatasetsToolStripMenuItem.Text = "Renumber Datasets"
        '
        'RemoveDatasetsToolStripMenuItem
        '
        Me.RemoveDatasetsToolStripMenuItem.Name = "RemoveDatasetsToolStripMenuItem"
        Me.RemoveDatasetsToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.RemoveDatasetsToolStripMenuItem.Text = "Remove Datasets"
        '
        'frmManager
        '
        Me.AllowDrop = True
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(658, 309)
        Me.Controls.Add(Me.treeFiles)
        Me.Controls.Add(Me.txtDetails)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmManager"
        Me.Text = "Data Sources"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub Edit(Optional ByVal aNodeKey As Integer = -1)
        Populate(aNodeKey)
        Me.Show()
        Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub Populate(ByVal aNodeKey As Integer)
        treeFiles.Nodes.Clear()
        txtDetails.Text = ""
        Dim lName As String = ""
        Dim lCount As String = 0
        For Each lDataSource As atcDataSource In atcDataManager.DataSources
            Dim lTypeAndSource() As String = lDataSource.Name.Split(":")

            'TODO: leave code for lTypeAndSource(0) - now only Timeseries, might want to add Model or Table later

            'If Not treeFiles.Nodes.ContainsKey(lTypeAndSource(0)) Then
            '    treeFiles.Nodes.Add(lTypeAndSource(0), lTypeAndSource(0))
            'End If
            'Dim lSourceNodes As TreeNodeCollection = treeFiles.Nodes(lTypeAndSource(0)).Nodes
            'If Not lSourceNodes.ContainsKey(lTypeAndSource(2)) Then
            '    lSourceNodes.Add(lTypeAndSource(2), lTypeAndSource(2))
            'End If
            If Not treeFiles.Nodes.ContainsKey(lTypeAndSource(2)) Then
                treeFiles.Nodes.Add(lTypeAndSource(2), lTypeAndSource(2))
            End If

            lName = lDataSource.Specification & " (" & lDataSource.DataSets.Count & ")"
            'treeFiles.Nodes(lTypeAndSource(0)).Nodes(lTypeAndSource(2)).Nodes.Add(lCount, lName)
            treeFiles.Nodes(lTypeAndSource(2)).Nodes.Add(lCount, lName)
            If lCount = aNodeKey Then
                'treeFiles.SelectedNode = treeFiles.Nodes(lTypeAndSource(0)).Nodes(lTypeAndSource(2)).Nodes(lCount)
                treeFiles.SelectedNode = treeFiles.Nodes(lTypeAndSource(2)).Nodes(lCount)
                treeFiles.SelectedNode.EnsureVisible()
                RefreshDetails(aNodeKey)
            End If
            lCount += 1
        Next
        treeFiles.ExpandAll()
    End Sub

    Private Sub SelectionAction(ByVal aAction As String)
        If treeFiles.SelectedNode IsNot Nothing Then
            If IsNumeric(treeFiles.SelectedNode.Name) Then
                Dim lDataSourceIndex As Integer = treeFiles.SelectedNode.Name
                If lDataSourceIndex > -1 AndAlso _
                       lDataSourceIndex < atcDataManager.DataSources.Count Then
                    Dim lDataSource As atcDataSource = atcDataManager.DataSources.Item(lDataSourceIndex)
                    With lDataSource
                        Logger.Dbg(aAction & ":" & .Specification)
                        Dim lActionArgs() As String = aAction.Split(":")
                        Select Case lActionArgs(0)
                            Case "Close"
                                atcDataManager.RemoveDataSource(lDataSourceIndex)
                                lDataSourceIndex -= 1
                                If lDataSourceIndex = -1 And atcDataManager.DataSources.Count > 0 Then
                                    lDataSourceIndex = 0
                                End If
                            Case "View"
                                .View()
                            Case "Display"
                                atcDataManager.UserSelectDisplay(.Specification, .DataSets.Clone)
                            Case "Analysis"
                                atcDataManager.ShowDisplay(lActionArgs(1), .DataSets)
                            Case "RemoveDatasets"
                                If .CanRemoveDataset Then
                                    For Each lDataSet As atcDataSet In atcDataManager.UserSelectData( _
                                      "Select Datasets to remove from " & .Specification, , .DataSets)
                                        .RemoveDataset(lDataSet)
                                    Next
                                    Populate(treeFiles.SelectedNode.Name)
                                End If
                        End Select
                    End With
                End If
            Else
                Logger.Msg("Choose a specific data source to " & aAction & ", not a data type or file format", aAction & " Problem")
            End If
        ElseIf treeFiles.Nodes.Count = 0 Then
            Logger.Msg("No data sources to " & aAction, aAction & " Problem")
        Else
            Logger.Msg("Choose a specific data source to " & aAction, aAction & " Problem")
        End If
    End Sub

    Private Sub ChangedData(ByVal aDataSource As atcDataSource)
        Edit(atcDataManager.DataSources.Count - 1)
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        Debug.Write("Closing frmManager")
    End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter, treeFiles.DragEnter, txtDetails.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub Form_DragDrop( _
        ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragDrop, treeFiles.DragDrop, txtDetails.DragDrop

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            Dim lIndex As Integer = 0
            For Each lFileName As String In lFileNames
                lIndex += 1
                Logger.Progress("Opening " & lFileName, lIndex, lFileNames.Length)
                atcDataManager.OpenDataSource(lFileName)
            Next
            Logger.Status("")
        End If
    End Sub

    Private Sub treeFiles_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeFiles.AfterSelect
        Dim lSelectedDataSourceIndex As String = e.Node.Name
        If IsNumeric(lSelectedDataSourceIndex) Then
            RefreshDetails(lSelectedDataSourceIndex)
        Else
            txtDetails.Text = ""
        End If
    End Sub

    Private Sub RefreshDetails(ByVal aDataSourceIndex As Integer)
        If aDataSourceIndex > -1 AndAlso _
           aDataSourceIndex < atcDataManager.DataSources.Count Then
            Dim lDataSource As atcDataSource = atcDataManager.DataSources.Item(aDataSourceIndex)
            txtDetails.Text = lDataSource.Name
            If Not lDataSource.Specification Is Nothing AndAlso lDataSource.Specification.Length > 0 Then
                txtDetails.Text &= vbCrLf & lDataSource.Specification
            End If
            If lDataSource.DataSets.Count > 0 Then
                txtDetails.Text &= vbCrLf & Format(lDataSource.DataSets.Count, "#,###") & " Timeseries"
            End If
            If FileExists(lDataSource.Specification) Then
                txtDetails.Text &= vbCrLf & Format(FileLen(lDataSource.Specification), "#,###") & " bytes"
                txtDetails.Text &= vbCrLf & "Modified " & System.IO.File.GetLastWriteTime(lDataSource.Specification)
            End If
        Else
            txtDetails.Text = ""
        End If
    End Sub

    ' Draws a node.
    Private Sub treeFiles_DrawNode(ByVal sender As Object, _
        ByVal e As DrawTreeNodeEventArgs) Handles treeFiles.DrawNode

        ' Draw the background and node text for a selected node.
        If (e.Node.IsSelected) Then 'e.State And TreeNodeStates.Selected) <> 0 Then

            ' Draw the background of the selected node. The NodeBounds
            ' method makes the highlight rectangle large enough to
            ' include the text of a node tag, if one is present.
            e.Graphics.FillRectangle(SystemBrushes.Highlight, NodeBounds(e.Node))

            ' Retrieve the node font. If the node font has not been set,
            ' use the TreeView font.
            Dim nodeFont As Font = e.Node.NodeFont
            If nodeFont Is Nothing Then
                nodeFont = CType(sender, TreeView).Font
            End If

            ' Draw the node text.
            e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, _
                e.Bounds.Left - 2, e.Bounds.Top)

            ' Use the default background and node text.
        Else
            e.DrawDefault = True
        End If

        ' If a node tag is present, draw its string representation 
        ' to the right of the label text.
        If Not (e.Node.Tag Is Nothing) Then
            e.Graphics.DrawString(e.Node.Tag.ToString(), e.Node.NodeFont, _
                Brushes.Yellow, e.Bounds.Right + 2, e.Bounds.Top)
        End If

        ' If the node has focus, draw the focus rectangle large, making
        ' it large enough to include the text of the node tag, if present.
        If (e.State And TreeNodeStates.Focused) <> 0 Then
            Dim focusPen As New Pen(Color.Black)
            Try
                focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                Dim focusBounds As Rectangle = NodeBounds(e.Node)
                focusBounds.Size = New Size(focusBounds.Width - 1, _
                    focusBounds.Height - 1)
                e.Graphics.DrawRectangle(focusPen, focusBounds)
            Finally
                focusPen.Dispose()
            End Try
        End If
    End Sub
    Private Function NodeBounds(ByVal e As System.Windows.Forms.TreeNode) As Rectangle
        Return New Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height)
    End Function

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Dim lFilesOnly As New ArrayList(1)
        lFilesOnly.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lFilesOnly, "Select a File Type", False, True)
        If Not lNewSource Is Nothing Then 'user did not cancel
            If Not atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data New Problem")
                End If
            End If
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Dim lCollection As New ArrayList
        lCollection.Add("File")
        Dim lNewSource As atcDataSource = atcDataManager.UserSelectDataSource(lCollection)
        If Not lNewSource Is Nothing Then
            If Not (atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data Open Problem")
                End If
            End If
        End If
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        SelectionAction("Close")
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub DisplayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayToolStripMenuItem.Click
        SelectionAction("Display")
    End Sub

    Private Sub NativeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NativeToolStripMenuItem.Click
        SelectionAction("View")
    End Sub

    Private Sub AnalysisToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnalysisToolStripMenuItem.Click
        If sender.text <> "Analysis" Then
            SelectionAction("Analysis:" & sender.Text)
        End If
    End Sub

    Private Sub RemoveDatasetsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RemoveDatasetsToolStripMenuItem.Click
        SelectionAction("RemoveDatasets")
    End Sub

    Private Sub RenumberDatasetsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RenumberDatasetsToolStripMenuItem.Click
        'TODO: implement this
    End Sub

    Private Sub AddDatasetsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddDatasetsToolStripMenuItem.Click
        'TODO: implement this
    End Sub

    Private Sub frmManager_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Project Creation and Management\GIS and Time-Series Data\Time-Series Management.html")
        End If
    End Sub

End Class


