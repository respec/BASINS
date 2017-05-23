<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddConnection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddConnection))
        Me.cboUpstream = New System.Windows.Forms.ComboBox()
        Me.lblUpstreamReach = New System.Windows.Forms.Label()
        Me.lblUpstream = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cboDownstream = New System.Windows.Forms.ComboBox()
        Me.lblDownstream = New System.Windows.Forms.Label()
        Me.lblDownstreamReach = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cboUpstream
        '
        Me.cboUpstream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUpstream.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cboUpstream.Location = New System.Drawing.Point(115, 38)
        Me.cboUpstream.Name = "cboUpstream"
        Me.cboUpstream.Size = New System.Drawing.Size(122, 21)
        Me.cboUpstream.TabIndex = 2
        '
        'lblUpstreamReach
        '
        Me.lblUpstreamReach.AutoSize = True
        Me.lblUpstreamReach.Location = New System.Drawing.Point(12, 41)
        Me.lblUpstreamReach.Name = "lblUpstreamReach"
        Me.lblUpstreamReach.Size = New System.Drawing.Size(90, 13)
        Me.lblUpstreamReach.TabIndex = 27
        Me.lblUpstreamReach.Text = "Upstream Reach:"
        '
        'lblUpstream
        '
        Me.lblUpstream.AutoSize = True
        Me.lblUpstream.Location = New System.Drawing.Point(12, 15)
        Me.lblUpstream.Name = "lblUpstream"
        Me.lblUpstream.Size = New System.Drawing.Size(76, 13)
        Me.lblUpstream.TabIndex = 29
        Me.lblUpstream.Text = "Upstream UCI:"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.AutoSize = True
        Me.btnOk.Location = New System.Drawing.Point(15, 148)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(84, 23)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.AutoSize = True
        Me.btnCancel.Location = New System.Drawing.Point(105, 148)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(84, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboDownstream
        '
        Me.cboDownstream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDownstream.FormattingEnabled = True
        Me.cboDownstream.Location = New System.Drawing.Point(115, 99)
        Me.cboDownstream.Name = "cboDownstream"
        Me.cboDownstream.Size = New System.Drawing.Size(122, 21)
        Me.cboDownstream.TabIndex = 30
        '
        'lblDownstream
        '
        Me.lblDownstream.AutoSize = True
        Me.lblDownstream.Location = New System.Drawing.Point(12, 76)
        Me.lblDownstream.Name = "lblDownstream"
        Me.lblDownstream.Size = New System.Drawing.Size(90, 13)
        Me.lblDownstream.TabIndex = 32
        Me.lblDownstream.Text = "Downstream UCI:"
        '
        'lblDownstreamReach
        '
        Me.lblDownstreamReach.AutoSize = True
        Me.lblDownstreamReach.Location = New System.Drawing.Point(12, 102)
        Me.lblDownstreamReach.Name = "lblDownstreamReach"
        Me.lblDownstreamReach.Size = New System.Drawing.Size(104, 13)
        Me.lblDownstreamReach.TabIndex = 31
        Me.lblDownstreamReach.Text = "Downstream Reach:"
        '
        'frmAddConnection
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(254, 183)
        Me.Controls.Add(Me.cboDownstream)
        Me.Controls.Add(Me.lblDownstream)
        Me.Controls.Add(Me.lblDownstreamReach)
        Me.Controls.Add(Me.cboUpstream)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblUpstream)
        Me.Controls.Add(Me.lblUpstreamReach)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAddConnection"
        Me.Text = "Add Connection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUpstreamReach As System.Windows.Forms.Label
    Friend WithEvents lblUpstream As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboDownstream As ComboBox
    Friend WithEvents lblDownstream As Label
    Friend WithEvents lblDownstreamReach As Label
    Friend WithEvents cboUpstream As ComboBox
End Class
