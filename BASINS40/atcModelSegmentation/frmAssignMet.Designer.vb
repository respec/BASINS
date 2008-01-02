<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAssignMet
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAssignMet))
        Me.cboMetStations = New System.Windows.Forms.ComboBox
        Me.fraMetStations = New System.Windows.Forms.GroupBox
        Me.cbxUseSelected = New System.Windows.Forms.CheckBox
        Me.cmdAssign = New System.Windows.Forms.Button
        Me.fraMetStations.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboMetStations
        '
        Me.cboMetStations.AllowDrop = True
        Me.cboMetStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMetStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMetStations.Location = New System.Drawing.Point(18, 30)
        Me.cboMetStations.Name = "cboMetStations"
        Me.cboMetStations.Size = New System.Drawing.Size(276, 24)
        Me.cboMetStations.TabIndex = 11
        '
        'fraMetStations
        '
        Me.fraMetStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMetStations.Controls.Add(Me.cbxUseSelected)
        Me.fraMetStations.Controls.Add(Me.cboMetStations)
        Me.fraMetStations.Location = New System.Drawing.Point(12, 12)
        Me.fraMetStations.Name = "fraMetStations"
        Me.fraMetStations.Size = New System.Drawing.Size(314, 94)
        Me.fraMetStations.TabIndex = 12
        Me.fraMetStations.TabStop = False
        Me.fraMetStations.Text = "Met Stations Layer"
        '
        'cbxUseSelected
        '
        Me.cbxUseSelected.AutoSize = True
        Me.cbxUseSelected.Location = New System.Drawing.Point(34, 60)
        Me.cbxUseSelected.Name = "cbxUseSelected"
        Me.cbxUseSelected.Size = New System.Drawing.Size(171, 21)
        Me.cbxUseSelected.TabIndex = 14
        Me.cbxUseSelected.Text = "Use Selected Features"
        Me.cbxUseSelected.UseVisualStyleBackColor = True
        '
        'cmdAssign
        '
        Me.cmdAssign.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAssign.Location = New System.Drawing.Point(115, 124)
        Me.cmdAssign.Name = "cmdAssign"
        Me.cmdAssign.Size = New System.Drawing.Size(112, 32)
        Me.cmdAssign.TabIndex = 13
        Me.cmdAssign.Text = "Assign"
        Me.cmdAssign.UseVisualStyleBackColor = True
        '
        'frmAssignMet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 168)
        Me.Controls.Add(Me.cmdAssign)
        Me.Controls.Add(Me.fraMetStations)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAssignMet"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Assign Met Segments by Proximity"
        Me.fraMetStations.ResumeLayout(False)
        Me.fraMetStations.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboMetStations As System.Windows.Forms.ComboBox
    Friend WithEvents fraMetStations As System.Windows.Forms.GroupBox
    Friend WithEvents cbxUseSelected As System.Windows.Forms.CheckBox
    Friend WithEvents cmdAssign As System.Windows.Forms.Button
End Class
