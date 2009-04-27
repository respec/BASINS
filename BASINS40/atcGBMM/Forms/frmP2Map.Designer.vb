<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmP2Map
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents cmdClose As System.Windows.Forms.Button
    Public WithEvents PictureP2Map As System.Windows.Forms.PictureBox
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmP2Map))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.PictureP2Map = New System.Windows.Forms.PictureBox
        CType(Me.PictureP2Map, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(825, 661)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(81, 25)
        Me.cmdClose.TabIndex = 0
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'PictureP2Map
        '
        Me.PictureP2Map.BackColor = System.Drawing.SystemColors.Control
        Me.PictureP2Map.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureP2Map.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureP2Map.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PictureP2Map.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PictureP2Map.Image = CType(resources.GetObject("PictureP2Map.Image"), System.Drawing.Image)
        Me.PictureP2Map.Location = New System.Drawing.Point(12, 12)
        Me.PictureP2Map.Name = "PictureP2Map"
        Me.PictureP2Map.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PictureP2Map.Size = New System.Drawing.Size(895, 636)
        Me.PictureP2Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureP2Map.TabIndex = 0
        Me.PictureP2Map.TabStop = False
        '
        'frmP2Map
        '
        Me.AcceptButton = Me.cmdClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(918, 698)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.PictureP2Map)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmP2Map"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "2-Year 24-Hour Rainfall Map"
        CType(Me.PictureP2Map, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class