<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBLM
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBLM))
        Me.btnRun = New System.Windows.Forms.Button
        Me.atcGridLocation = New atcControls.atcGrid
        Me.lblSelect = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnRun
        '
        Me.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRun.Location = New System.Drawing.Point(134, 183)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(53, 32)
        Me.btnRun.TabIndex = 0
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'atcGridLocation
        '
        Me.atcGridLocation.AllowHorizontalScrolling = True
        Me.atcGridLocation.AllowNewValidValues = False
        Me.atcGridLocation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridLocation.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridLocation.LineColor = System.Drawing.Color.Empty
        Me.atcGridLocation.LineWidth = 0.0!
        Me.atcGridLocation.Location = New System.Drawing.Point(13, 26)
        Me.atcGridLocation.Name = "atcGridLocation"
        Me.atcGridLocation.Size = New System.Drawing.Size(298, 151)
        Me.atcGridLocation.Source = Nothing
        Me.atcGridLocation.TabIndex = 1
        '
        'lblSelect
        '
        Me.lblSelect.AutoSize = True
        Me.lblSelect.Location = New System.Drawing.Point(13, 6)
        Me.lblSelect.Name = "lblSelect"
        Me.lblSelect.Size = New System.Drawing.Size(117, 17)
        Me.lblSelect.TabIndex = 2
        Me.lblSelect.Text = "Select a Location"
        '
        'frmBLM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(323, 227)
        Me.Controls.Add(Me.lblSelect)
        Me.Controls.Add(Me.atcGridLocation)
        Me.Controls.Add(Me.btnRun)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBLM"
        Me.Text = "Biotic Ligand Model (BLM)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents atcGridLocation As atcControls.atcGrid
    Friend WithEvents lblSelect As System.Windows.Forms.Label
End Class
