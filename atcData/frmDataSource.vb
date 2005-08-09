Imports System.Windows.Forms
Imports atcUtility

Public Class frmDataSource
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
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDataSource))
    Me.treeSources = New System.Windows.Forms.TreeView
    Me.pnlButtons = New System.Windows.Forms.Panel
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
    Me.treeSources.ImageIndex = -1
    Me.treeSources.Location = New System.Drawing.Point(0, 0)
    Me.treeSources.Name = "treeSources"
    Me.treeSources.SelectedImageIndex = -1
    Me.treeSources.Size = New System.Drawing.Size(355, 577)
    Me.treeSources.TabIndex = 0
    '
    'pnlButtons
    '
    Me.pnlButtons.Controls.Add(Me.btnCancel)
    Me.pnlButtons.Controls.Add(Me.btnOk)
    Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.pnlButtons.Location = New System.Drawing.Point(0, 568)
    Me.pnlButtons.Name = "pnlButtons"
    Me.pnlButtons.Size = New System.Drawing.Size(355, 46)
    Me.pnlButtons.TabIndex = 13
    '
    'btnCancel
    '
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
    'frmDataSource
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(355, 614)
    Me.Controls.Add(Me.pnlButtons)
    Me.Controls.Add(Me.treeSources)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmDataSource"
    Me.Text = "Select a Data Source"
    Me.pnlButtons.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pDataManager As atcDataManager
  Private pCategories As ArrayList
  Private pSelectedSource As atcDataSource
  Private pSpecification As String

  Public Sub AskUser(ByVal aDataManager As atcDataManager, _
                     ByRef aSelectedSource As atcDataSource, _
                     ByRef aSpecification As String, _
                     ByRef aNeedToOpen As Boolean, _
                     ByRef aNeedToSave As Boolean, _
            Optional ByVal aCategories As ArrayList = Nothing)
    pDataManager = aDataManager
    pSelectedSource = aSelectedSource
    pSpecification = aSpecification
    pCategories = aCategories
    Populate(aNeedToOpen, aNeedToSave)
    Me.ShowDialog() 'Block until form closes
    aSelectedSource = pSelectedSource
    aSpecification = pSpecification
  End Sub

  Private Sub Populate(ByRef aNeedToOpen As Boolean, _
                       ByRef aNeedToSave As Boolean)
    Dim lNode As TreeNode
    Dim lDataSources As atcCollection = pDataManager.GetPlugins(GetType(atcDataSource))
    For Each ds As atcDataSource In lDataSources
      If (Not aNeedToOpen OrElse ds.CanOpen) AndAlso _
         (Not aNeedToSave OrElse ds.CanSave) Then
        Dim lOperations As atcDataGroup = ds.AvailableOperations
        Dim lCategory As String = ds.Category
        If lCategory.Length = 0 Then lCategory = ds.Description
        'If either no category was specified or
        'this DataSource has one of the specified categories
        If pCategories Is Nothing OrElse pCategories.Contains(lCategory) Then
          Dim lCategoryNode As TreeNode = FindOrCreateNode(treeSources.Nodes, lCategory)
          lCategoryNode.ExpandAll()
          If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
            For Each lOperation As atcDataSet In lOperations
              Dim lName As String = lOperation.Attributes.GetValue("Name")
              If lName.Length = 0 Then
                lName = lOperation.Attributes.ItemByIndex(0).Definition.Name
              End If

              'Operations might have categories to further divide them
              Dim lSubCategory As String = lOperation.Attributes.GetValue("Category")
              If lSubCategory.Length > 0 Then
                Dim lSubCategoryNode As TreeNode = FindOrCreateNode(lCategoryNode.Nodes, lSubCategory)
                lNode = lSubCategoryNode.Nodes.Add(lName)
                lSubCategoryNode.ExpandAll()
              Else
                lNode = lCategoryNode.Nodes.Add(lName)
              End If

              lNode.Tag = ds.Name
              If ds.Equals(pSelectedSource) AndAlso lNode.Text = pSpecification Then
                treeSources.SelectedNode = lNode
              End If
            Next
          Else
            lNode = lCategoryNode.Nodes.Add(ds.Description)
            lNode.Tag = ds.Name
            If ds.Equals(pSelectedSource) Then
              treeSources.SelectedNode = lNode
            End If
          End If
        End If
      End If
    Next
    'These were only set for use during Populate and should not still be set if user closes window
    pSelectedSource = Nothing
    pSpecification = ""
  End Sub

  Private Function FindOrCreateNode(ByVal aNodes As TreeNodeCollection, ByVal aNodeText As String) As TreeNode
    Dim newNode As TreeNode
    For Each newNode In aNodes
      If newNode.Text = aNodeText Then
        Exit For
      End If
      newNode = Nothing
    Next
    If newNode Is Nothing Then
      newNode = New TreeNode
      newNode.Text = aNodeText
      aNodes.Add(newNode)
    End If
    Return newNode
  End Function

  Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    Me.Close()
  End Sub

  Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
    Dim lSourceName As String = treeSources.SelectedNode.Tag
    Dim lOperationName As String = treeSources.SelectedNode.Text
    For Each ds As atcDataSource In pDataManager.GetPlugins(GetType(atcDataSource))
      If ds.Name = lSourceName Then
        Dim lOperations As atcDataGroup = ds.AvailableOperations
        If lOperations.Count > 0 Then
          For Each lOperation As atcDataSet In lOperations
            If lOperation.Attributes.GetValue("Name") = lOperationName OrElse _
               lOperation.Attributes.ItemByIndex(0).Definition.Name = lOperationName Then
              pSelectedSource = ds
              pSpecification = lOperationName
              Me.Close()
            End If
          Next
        ElseIf ds.Description = lOperationName Then
          pSelectedSource = ds
          Me.Close()
        End If
      End If
    Next
  End Sub

  Private Sub treeSources_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treeSources.DoubleClick
    btnOk_Click(sender, e)
  End Sub
End Class
