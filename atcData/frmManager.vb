Imports System.Windows.Forms

Imports atcUtility

Friend Class frmManager
  Inherits System.Windows.Forms.Form

  Private WithEvents pManager As atcTimeseriesManager

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
  Friend WithEvents treeFiles As System.Windows.Forms.TreeView
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents tbbOpen As System.Windows.Forms.ToolBarButton
  Friend WithEvents tbbClose As System.Windows.Forms.ToolBarButton
  Friend WithEvents txtDetails As System.Windows.Forms.TextBox
  Friend WithEvents toolbarTop As System.Windows.Forms.ToolBar
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmManager))
    Me.treeFiles = New System.Windows.Forms.TreeView
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.toolbarTop = New System.Windows.Forms.ToolBar
    Me.tbbOpen = New System.Windows.Forms.ToolBarButton
    Me.tbbClose = New System.Windows.Forms.ToolBarButton
    Me.txtDetails = New System.Windows.Forms.TextBox
    Me.SuspendLayout()
    '
    'treeFiles
    '
    Me.treeFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.treeFiles.ImageIndex = -1
    Me.treeFiles.Location = New System.Drawing.Point(0, 40)
    Me.treeFiles.Name = "treeFiles"
    Me.treeFiles.SelectedImageIndex = -1
    Me.treeFiles.Size = New System.Drawing.Size(296, 288)
    Me.treeFiles.TabIndex = 0
    '
    'toolbarTop
    '
    Me.toolbarTop.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.tbbOpen, Me.tbbClose})
    Me.toolbarTop.DropDownArrows = True
    Me.toolbarTop.Location = New System.Drawing.Point(0, 0)
    Me.toolbarTop.Name = "toolbarTop"
    Me.toolbarTop.ShowToolTips = True
    Me.toolbarTop.Size = New System.Drawing.Size(296, 42)
    Me.toolbarTop.TabIndex = 1
    '
    'tbbOpen
    '
    Me.tbbOpen.Text = "Open"
    '
    'tbbClose
    '
    Me.tbbClose.Text = "Close"
    '
    'txtDetails
    '
    Me.txtDetails.BackColor = System.Drawing.SystemColors.ControlLight
    Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.txtDetails.Location = New System.Drawing.Point(0, 333)
    Me.txtDetails.Multiline = True
    Me.txtDetails.Name = "txtDetails"
    Me.txtDetails.Size = New System.Drawing.Size(296, 56)
    Me.txtDetails.TabIndex = 2
    Me.txtDetails.Text = ""
    '
    'frmManager
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(296, 389)
    Me.Controls.Add(Me.txtDetails)
    Me.Controls.Add(Me.toolbarTop)
    Me.Controls.Add(Me.treeFiles)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmManager"
    Me.Text = "Timeseries Files"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub Edit(ByVal aManager As atcTimeseriesManager)
    pManager = aManager
    treeFiles.Nodes.Clear()
    For Each lFile As atcTimeseriesFile In pManager.Files
      AddFileToTree(treeFiles.Nodes, lFile)
    Next
    Me.Show()
  End Sub

  Private Sub AddFileToTree(ByVal aAddTo As TreeNodeCollection, ByVal aFile As atcTimeseriesFile)
    Dim fileNode As TreeNode = aAddTo.Add(aFile.Name & " " & aFile.FileName & " (" & aFile.Timeseries.Count & ")")
    fileNode.Tag = aFile
    Dim ts As atcTimeseries
    Dim tsNode As TreeNode
    For i As Integer = 0 To aFile.Timeseries.Count - 1
      ts = aFile.Timeseries.Item(i)
      tsNode = fileNode.Nodes.Add(ts.ToString)
      tsNode.Tag = ts
    Next
  End Sub

  Private Sub treeFiles_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeFiles.AfterSelect
    Dim obj As Object = treeFiles.SelectedNode.Tag

    If obj.GetType.IsSubclassOf(GetType(atcTimeseriesFile)) Then
      Dim tsf As atcTimeseriesFile = obj
      txtDetails.Text = tsf.Name & " File '" & tsf.FileName & "'"
      txtDetails.Text &= vbCrLf & Format(tsf.Timeseries.Count, "#,###") & " Timeseries"
      If FileExists(tsf.FileName) Then
        txtDetails.Text &= vbCrLf & Format(FileLen(tsf.FileName), "#,###") & " bytes"
        txtDetails.Text &= vbCrLf & "Modified " & System.IO.File.GetLastWriteTime(tsf.FileName)
      End If

    ElseIf obj.GetType.Equals(GetType(atcTimeseries)) Then
      Dim ts As atcTimeseries = obj
      txtDetails.Text = "Timeseries '" & ts.ToString & "'"
      txtDetails.Text &= vbCrLf & Format(ts.numValues, "#,###") & " values"

    Else
      txtDetails.Text = obj.GetType.Name '""

    End If

  End Sub

  Private Sub toolbarTop_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles toolbarTop.ButtonClick
    Select Case e.Button.Text
      Case "Open" : pManager.Open("")
      Case "Close"
        'TODO: how do we remove a file from pManager.Files?
    End Select
  End Sub

  Private Sub pManager_OpenedFile(ByVal aTimeseriesFile As atcTimeseriesFile) Handles pManager.OpenedFile
    Edit(pManager)
  End Sub
End Class
