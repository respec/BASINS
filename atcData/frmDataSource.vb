Imports System.Windows
Imports System.Windows.Forms
Imports atcUtility

Friend Class frmDataSource
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.CancelButton = Me.btnCancel
        Me.AcceptButton = Me.btnOk

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
    Friend WithEvents treeSources As System.Windows.Forms.TreeView
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents cboDisplay As System.Windows.Forms.ComboBox
    Friend WithEvents lblDisplay As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataSource))
        Me.treeSources = New System.Windows.Forms.TreeView
        Me.pnlButtons = New System.Windows.Forms.Panel
        Me.lblDisplay = New System.Windows.Forms.Label
        Me.cboDisplay = New System.Windows.Forms.ComboBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'treeSources
        '
        Me.treeSources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treeSources.Location = New System.Drawing.Point(0, 0)
        Me.treeSources.Name = "treeSources"
        Me.treeSources.Size = New System.Drawing.Size(369, 389)
        Me.treeSources.TabIndex = 0
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.lblDisplay)
        Me.pnlButtons.Controls.Add(Me.cboDisplay)
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnOk)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtons.Location = New System.Drawing.Point(0, 389)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(368, 40)
        Me.pnlButtons.TabIndex = 1
        '
        'lblDisplay
        '
        Me.lblDisplay.Location = New System.Drawing.Point(8, 12)
        Me.lblDisplay.Name = "lblDisplay"
        Me.lblDisplay.Size = New System.Drawing.Size(72, 20)
        Me.lblDisplay.TabIndex = 2
        Me.lblDisplay.Text = "Display with:"
        Me.lblDisplay.Visible = False
        '
        'cboDisplay
        '
        Me.cboDisplay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDisplay.Location = New System.Drawing.Point(88, 8)
        Me.cboDisplay.Name = "cboDisplay"
        Me.cboDisplay.Size = New System.Drawing.Size(96, 21)
        Me.cboDisplay.TabIndex = 3
        Me.cboDisplay.Text = "No Display"
        Me.cboDisplay.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(280, 8)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(192, 8)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "Ok"
        '
        'frmDataSource
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(368, 429)
        Me.Controls.Add(Me.treeSources)
        Me.Controls.Add(Me.pnlButtons)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDataSource"
        Me.Text = "Select a Data Source"
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pCategories As ArrayList
    Private pSelectedSource As atcTimeseriesSource
    Private pSpecification As String
    'Private Const NO_DISPLAY As String = "No Display"
    'Private pDisplayPlugins As ICollection

    Public Sub AskUser(ByRef aSelectedSource As atcTimeseriesSource, _
                       ByRef aNeedToOpen As Boolean, _
                       ByRef aNeedToSave As Boolean, _
              Optional ByVal aCategories As ArrayList = Nothing)
        pSelectedSource = aSelectedSource
        If Not aSelectedSource Is Nothing Then pSpecification = aSelectedSource.Specification
        pCategories = aCategories
        'PopulateDisplays()
        Populate(aNeedToOpen, aNeedToSave)
        If treeSources.Nodes.Count = 1 AndAlso treeSources.Nodes(0).Nodes.Count = 1 Then
            treeSources.SelectedNode = treeSources.Nodes(0).Nodes(0)
            GetSource(treeSources.SelectedNode.Tag, treeSources.SelectedNode.Text)
        Else
            Me.ShowDialog() 'Block until form closes
        End If

        aSelectedSource = pSelectedSource
        If Not aSelectedSource Is Nothing Then
            aSelectedSource.Specification = pSpecification
        End If
        pCategories = Nothing
        pSelectedSource = Nothing
    End Sub

    'Private Sub PopulateDisplays()
    '  pDisplayPlugins = atcDataManager.GetPlugins(GetType(atcDataDisplay))
    '  cboDisplay.Items.Clear()
    '  cboDisplay.Items.Add(NO_DISPLAY)
    '  For Each lDisp As atcDataDisplay In pDisplayPlugins
    '    Dim iColon As Integer = lDisp.Name.IndexOf("::")
    '    If iColon > 0 Then
    '      cboDisplay.Items.Add(lDisp.Name.Substring(iColon + 2))
    '    Else
    '      cboDisplay.Items.Add(lDisp.Name)
    '    End If
    '  Next
    '  cboDisplay.Text = GetSetting("atcData", "DataSource", "Display", NO_DISPLAY)
    'End Sub

    Private Sub Populate(ByRef aNeedToOpen As Boolean, _
                         ByRef aNeedToSave As Boolean)
        Dim lNode As Forms.TreeNode
        Dim lDataSources As atcCollection = atcDataManager.GetPlugins(GetType(atcDataSource))
        If lDataSources.Count = 0 Then
            treeSources.Nodes.Add("No data source plugins are loaded")
        Else
            For Each ds As atcDataSource In lDataSources
                If (Not aNeedToOpen OrElse ds.CanOpen) AndAlso _
                   (Not aNeedToSave OrElse ds.CanSave) Then
                    Dim lOperations As atcDataAttributes = ds.AvailableOperations
                    Dim lCategory As String = ds.Category
                    If lCategory.Length = 0 Then lCategory = ds.Description
                    'If either no category was specified or
                    'this DataSource has one of the specified categories
                    If pCategories Is Nothing OrElse pCategories.Contains(lCategory) Then
                        Dim lCategoryNode As Forms.TreeNode = FindOrCreateNode(treeSources.Nodes, lCategory)
                        If lCategory.Equals("File") OrElse _
                           GetSetting("BASINS4", "Data Source Categories", lCategory, "Expanded") = "Expanded" Then
                            lCategoryNode.ExpandAll()
                        End If
                        If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                            For Each lOperation As atcDefinedValue In lOperations
                                Select Case lOperation.Definition.TypeString
                                    Case "atcTimeseries", "atcDataGroup", "atcTimeseriesGroup"
                                        'Operations might have categories to further divide them
                                        If lOperation.Definition.Category.Length > 0 Then
                                            Dim lSubCategoryNode As Forms.TreeNode = FindOrCreateNode(lCategoryNode.Nodes, lOperation.Definition.Category)
                                            lNode = FindOrCreateNode(lSubCategoryNode.Nodes, lOperation.Definition.Name)
                                            lSubCategoryNode.ExpandAll()
                                        Else
                                            lNode = FindOrCreateNode(lCategoryNode.Nodes, lOperation.Definition.Name)
                                        End If

                                        lNode.Tag = ds.Name
                                        If ds.Equals(pSelectedSource) AndAlso lNode.Text = pSpecification Then
                                            treeSources.SelectedNode = lNode
                                        End If
                                End Select
                            Next
                        Else
                            lNode = FindOrCreateNode(lCategoryNode.Nodes, ds.Description)
                            lNode.Tag = ds.Name
                            If ds.Equals(pSelectedSource) Then
                                treeSources.SelectedNode = lNode
                            End If
                        End If
                    End If
                End If
            Next
            If treeSources.Nodes.Count = 0 Then
                treeSources.Nodes.Add("No appropriate plugins are loaded")
            End If
        End If
        treeSources.Nodes(0).EnsureVisible()
        ResizeToShowBottomNode()
        'These were only set for use during Populate and should not still be set if user closes window
        pSelectedSource = Nothing
        pSpecification = ""
    End Sub

    Private Sub ResizeToShowBottomNode()
        Dim lTotalHeight As Integer = treeSources.Top + pnlButtons.Height + Me.Height - Me.ClientSize.Height + 10

        For Each lNode As Forms.TreeNode In treeSources.Nodes
            lTotalHeight += NodeHeight(lNode)
        Next

        'Dim lBottomNode As TreeNode = BottomNode(treeSources.Nodes(treeSources.Nodes.Count - 1))
        Me.Top = 0
        'Me.Height = lBottomNode.Bounds.Top + lBottomNode.Bounds.Height
        Me.Height = lTotalHeight
    End Sub

    Private Function NodeHeight(ByVal aNode As Forms.TreeNode) As Integer
        NodeHeight = aNode.Bounds.Height
        If aNode.IsExpanded Then
            For Each lNode As Forms.TreeNode In aNode.Nodes
                NodeHeight += NodeHeight(lNode)
            Next
        End If
    End Function

    Private Function FindOrCreateNode(ByVal aNodes As TreeNodeCollection, ByVal aNodeText As String) As Forms.TreeNode
        Dim newNode As Forms.TreeNode = Nothing
        For Each newNode In aNodes
            If newNode.Text = aNodeText Then
                Exit For
            End If
            newNode = Nothing
        Next
        If newNode Is Nothing Then
            newNode = New Forms.TreeNode(aNodeText)
            Dim lIndex As Integer
            For lIndex = 0 To aNodes.Count - 1
                If aNodes(lIndex).Text > aNodeText Then Exit For
            Next
            aNodes.Insert(lIndex, newNode)
        End If
        Return newNode
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If GetSource(treeSources.SelectedNode.Tag, treeSources.SelectedNode.Text) Then
            Me.Close()
        End If
    End Sub

    Private Function GetSource(ByVal aSourceName As String, ByVal aOperationName As String) As Boolean
        For Each ds As atcDataSource In atcDataManager.GetPlugins(GetType(atcDataSource))
            If ds.Name = aSourceName Then
                Dim lOperations As atcDataAttributes = ds.AvailableOperations
                If lOperations.Count > 0 Then
                    For Each lOperation As atcDefinedValue In lOperations
                        If lOperation.Definition.Name = aOperationName Then
                            pSelectedSource = ds.NewOne
                            pSpecification = aOperationName
                            Return True
                        End If
                    Next
                ElseIf ds.Description = aOperationName Then
                    pSelectedSource = ds.NewOne
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Private Sub treeSources_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treeSources.DoubleClick
        btnOk_Click(sender, e)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Dim lHeight As Integer = Me.Height
        MyBase.OnLoad(e)
        Me.Top = 0
        Me.Height = lHeight
    End Sub

    Private Sub treeSources_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeSources.AfterCollapse
        If Not e.Node Is Nothing AndAlso e.Node.Parent Is Nothing Then
            SaveSetting("BASINS4", "Data Source Categories", e.Node.Text, "Closed")
        End If
    End Sub

    Private Sub treeSources_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeSources.AfterExpand
        If Not e.Node Is Nothing AndAlso e.Node.Parent Is Nothing Then
            SaveSetting("BASINS4", "Data Source Categories", e.Node.Text, "Expanded")
            ResizeToShowBottomNode()
        End If
    End Sub

    'Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
    '  SaveSetting("atcData", "DataSource", "Display", cboDisplay.Text)
    'End Sub
End Class
