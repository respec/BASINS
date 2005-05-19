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
  Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmGenerateTimeseries))
    Me.TreeView1 = New System.Windows.Forms.TreeView
    Me.SuspendLayout()
    '
    'TreeView1
    '
    Me.TreeView1.ImageIndex = -1
    Me.TreeView1.Location = New System.Drawing.Point(8, 8)
    Me.TreeView1.Name = "TreeView1"
    Me.TreeView1.SelectedImageIndex = -1
    Me.TreeView1.Size = New System.Drawing.Size(152, 256)
    Me.TreeView1.TabIndex = 0
    '
    'frmGenerateTimeseries
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(292, 273)
    Me.Controls.Add(Me.TreeView1)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmGenerateTimeseries"
    Me.Text = "frmGenerateTimeseries"
    Me.ResumeLayout(False)

  End Sub

#End Region

End Class
