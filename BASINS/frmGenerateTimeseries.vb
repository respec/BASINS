Imports atcData
Public Class frmGenerateTimeseries
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
  Friend WithEvents treeOperations As System.Windows.Forms.TreeView
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lstArgs As System.Windows.Forms.ListBox
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents treeTimeseries As System.Windows.Forms.TreeView
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmGenerateTimeseries))
    Me.treeOperations = New System.Windows.Forms.TreeView
    Me.Label1 = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.lstArgs = New System.Windows.Forms.ListBox
    Me.Label3 = New System.Windows.Forms.Label
    Me.treeTimeseries = New System.Windows.Forms.TreeView
    Me.SuspendLayout()
    '
    'treeOperations
    '
    Me.treeOperations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.treeOperations.FullRowSelect = True
    Me.treeOperations.ImageIndex = -1
    Me.treeOperations.Location = New System.Drawing.Point(8, 32)
    Me.treeOperations.Name = "treeOperations"
    Me.treeOperations.SelectedImageIndex = -1
    Me.treeOperations.Size = New System.Drawing.Size(152, 232)
    Me.treeOperations.TabIndex = 0
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(8, 8)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(128, 16)
    Me.Label1.TabIndex = 2
    Me.Label1.Text = "Operation"
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(168, 136)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(128, 16)
    Me.Label2.TabIndex = 3
    Me.Label2.Text = "Arguments"
    '
    'lstArgs
    '
    Me.lstArgs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lstArgs.IntegralHeight = False
    Me.lstArgs.Location = New System.Drawing.Point(168, 152)
    Me.lstArgs.Name = "lstArgs"
    Me.lstArgs.Size = New System.Drawing.Size(200, 112)
    Me.lstArgs.TabIndex = 4
    '
    'Label3
    '
    Me.Label3.Location = New System.Drawing.Point(168, 8)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(128, 16)
    Me.Label3.TabIndex = 5
    Me.Label3.Text = "Available Timeseries"
    '
    'treeTimeseries
    '
    Me.treeTimeseries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.treeTimeseries.FullRowSelect = True
    Me.treeTimeseries.ImageIndex = -1
    Me.treeTimeseries.Location = New System.Drawing.Point(168, 32)
    Me.treeTimeseries.Name = "treeTimeseries"
    Me.treeTimeseries.SelectedImageIndex = -1
    Me.treeTimeseries.Size = New System.Drawing.Size(200, 96)
    Me.treeTimeseries.TabIndex = 6
    '
    'frmGenerateTimeseries
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(376, 273)
    Me.Controls.Add(Me.treeTimeseries)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.lstArgs)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.treeOperations)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmGenerateTimeseries"
    Me.Text = "frmGenerateTimeseries"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private pPlugins As ICollection

  Public Sub Populate(ByVal aManager As atcTimeseriesManager)
    pPlugins = aManager.TimeseriesDataPlugins
    PopulateOperations()
    PopulateTimeseries(aManager)
  End Sub

  Private Sub PopulateOperations()
    Dim lNode As System.Windows.Forms.TreeNode
    For Each curPlugin As atcDataPlugin In pPlugins
      lNode = treeOperations.Nodes.Add(curPlugin.Name)
      lNode.Expand()
      For Each def As DictionaryEntry In curPlugin.AvailableTimeseriesOperations
        lNode.Nodes.Add(def.Value.Name)
      Next
    Next
  End Sub

  Private Sub PopulateTimeseries(ByVal aManager As atcTimeseriesManager)
    Dim lNode As System.Windows.Forms.TreeNode
    For Each tsFile As atcTimeseriesFile In aManager.Files
      lNode = treeTimeseries.Nodes.Add(tsFile.FileName)
      lNode.Expand()
      For Each ts As atcTimeseries In tsFile.Timeseries
        lNode.Nodes.Add(ts.ToString)
      Next
    Next
  End Sub

  Private Sub treeTimeseries_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeTimeseries.AfterSelect
    lstArgs.Items.Add(treeTimeseries.SelectedNode.Text)
  End Sub

  Private Sub lstArgs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstArgs.DoubleClick
    lstArgs.Items.RemoveAt(lstArgs.SelectedIndex)
  End Sub
End Class
