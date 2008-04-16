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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnDisplay As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents treeFiles As System.Windows.Forms.TreeView
    'Friend WithEvents panelOpening As System.Windows.Forms.Panel
    'Friend WithEvents lstDataSourceType As System.Windows.Forms.ListBox
    'Friend WithEvents lblDataSourceType As System.Windows.Forms.Label
    'Friend WithEvents btnOpen As System.Windows.Forms.Button
    'Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManager))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnDisplay = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.treeFiles = New System.Windows.Forms.TreeView
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
        Me.txtDetails.Size = New System.Drawing.Size(504, 84)
        Me.txtDetails.TabIndex = 2
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(7, 7)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(90, 26)
        Me.btnOpen.TabIndex = 4
        Me.btnOpen.Text = "Open File..."
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(199, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(106, 26)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close Selected"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnDisplay
        '
        Me.btnDisplay.Location = New System.Drawing.Point(311, 7)
        Me.btnDisplay.Name = "btnDisplay"
        Me.btnDisplay.Size = New System.Drawing.Size(119, 26)
        Me.btnDisplay.TabIndex = 6
        Me.btnDisplay.Text = "Display Selected"
        Me.btnDisplay.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(103, 8)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 26)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New File..."
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'treeFiles
        '
        Me.treeFiles.AllowDrop = True
        Me.treeFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treeFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.treeFiles.Location = New System.Drawing.Point(0, 40)
        Me.treeFiles.Name = "treeFiles"
        Me.treeFiles.Size = New System.Drawing.Size(504, 180)
        Me.treeFiles.TabIndex = 3
        '
        'frmManager
        '
        Me.AllowDrop = True
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(504, 309)
        Me.Controls.Add(Me.treeFiles)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnDisplay)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.txtDetails)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmManager"
        Me.Text = "Data Sources"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Sub Edit(Optional ByVal aNodeKey As Integer = -1)
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
        Me.Show()
        Windows.Forms.Application.DoEvents()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
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

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
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

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If treeFiles.SelectedNode IsNot Nothing Then
            If IsNumeric(treeFiles.SelectedNode.Name) Then
                Dim lDataSourceIndex As Integer = treeFiles.SelectedNode.Name
                If lDataSourceIndex > -1 AndAlso _
                       lDataSourceIndex < atcDataManager.DataSources.Count Then
                    Logger.Dbg("Close:" & CType(atcDataManager.DataSources.Item(lDataSourceIndex), atcDataSource).Specification)
                    atcDataManager.RemoveDataSource (lDataSourceIndex)
                    lDataSourceIndex -= 1
                    If lDataSourceIndex = -1 And atcDataManager.DataSources.Count > 0 Then
                        lDataSourceIndex = 0
                    End If
                End If
            Else 'TODO: close all of this type?
                Logger.Msg("Choose a specific data source to close, not a data type or file format")
            End If
        Else 'TODO: close all of this type?
            Logger.Msg("Choose a specific data source to close")
        End If
    End Sub

    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If treeFiles.SelectedNode IsNot Nothing Then
            If IsNumeric(treeFiles.SelectedNode.Name) Then
                Dim lDataSourceIndex As Integer = treeFiles.SelectedNode.Name
                If lDataSourceIndex > -1 AndAlso _
                       lDataSourceIndex < atcDataManager.DataSources.Count Then
                    Dim lDataSource As atcDataSource = atcDataManager.DataSources.Item(lDataSourceIndex)
                    Logger.Dbg("Display:" & lDataSource.Specification)
                    atcDataManager.UserSelectDisplay(lDataSource.Specification, lDataSource.DataSets.Clone)
                End If
            Else
                Logger.Msg("Choose a specific data source to display, not a data type or file format")
            End If
        Else
            Logger.Msg("Choose a specific data source to display")
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
            For Each lFileName As String In lFileNames
                Logger.Dbg("DroppedFile:" & lFileName)
                atcDataManager.OpenDataSource(lFileName)
            Next
        End If
    End Sub

    Private Sub treeFiles_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles treeFiles.NodeMouseClick
        Dim lSelectedDataSourceIndex As String = e.Node.Name
        If IsNumeric(lSelectedDataSourceIndex) Then
            RefreshDetails(lSelectedDataSourceIndex)
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
End Class


