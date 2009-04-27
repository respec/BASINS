<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmWASPInterface
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
    Public WithEvents optionTime As System.Windows.Forms.RadioButton
    Public WithEvents txtTravelTime As System.Windows.Forms.TextBox
    Public WithEvents txtLength As System.Windows.Forms.TextBox
    Public WithEvents optionLength As System.Windows.Forms.RadioButton
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWASPInterface))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.optionTime = New System.Windows.Forms.RadioButton
        Me.txtTravelTime = New System.Windows.Forms.TextBox
        Me.txtLength = New System.Windows.Forms.TextBox
        Me.optionLength = New System.Windows.Forms.RadioButton
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Frame1
        '
        Me.Frame1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.optionTime)
        Me.Frame1.Controls.Add(Me.txtTravelTime)
        Me.Frame1.Controls.Add(Me.txtLength)
        Me.Frame1.Controls.Add(Me.optionLength)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(16, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(287, 88)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Segmentation Options"
        '
        'optionTime
        '
        Me.optionTime.AutoSize = True
        Me.optionTime.BackColor = System.Drawing.SystemColors.Control
        Me.optionTime.Checked = True
        Me.optionTime.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionTime.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionTime.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionTime.Location = New System.Drawing.Point(16, 24)
        Me.optionTime.Name = "optionTime"
        Me.optionTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionTime.Size = New System.Drawing.Size(150, 18)
        Me.optionTime.TabIndex = 0
        Me.optionTime.TabStop = True
        Me.optionTime.Text = "Enter Travel Time (hours):"
        Me.optionTime.UseVisualStyleBackColor = False
        '
        'txtTravelTime
        '
        Me.txtTravelTime.AcceptsReturn = True
        Me.txtTravelTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTravelTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtTravelTime.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTravelTime.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTravelTime.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTravelTime.Location = New System.Drawing.Point(199, 24)
        Me.txtTravelTime.MaxLength = 0
        Me.txtTravelTime.Name = "txtTravelTime"
        Me.txtTravelTime.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTravelTime.Size = New System.Drawing.Size(72, 20)
        Me.txtTravelTime.TabIndex = 1
        '
        'txtLength
        '
        Me.txtLength.AcceptsReturn = True
        Me.txtLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLength.BackColor = System.Drawing.SystemColors.Window
        Me.txtLength.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLength.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLength.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLength.Location = New System.Drawing.Point(199, 56)
        Me.txtLength.MaxLength = 0
        Me.txtLength.Name = "txtLength"
        Me.txtLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLength.Size = New System.Drawing.Size(72, 20)
        Me.txtLength.TabIndex = 3
        '
        'optionLength
        '
        Me.optionLength.AutoSize = True
        Me.optionLength.BackColor = System.Drawing.SystemColors.Control
        Me.optionLength.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionLength.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionLength.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionLength.Location = New System.Drawing.Point(16, 56)
        Me.optionLength.Name = "optionLength"
        Me.optionLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionLength.Size = New System.Drawing.Size(178, 18)
        Me.optionLength.TabIndex = 2
        Me.optionLength.TabStop = True
        Me.optionLength.Text = "Enter Segment Length (meters):"
        Me.optionLength.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(169, 112)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(64, 24)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(239, 112)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(64, 24)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'FrmWASPInterface
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(322, 148)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmWASPInterface"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WASP Stream Network Preprocessor"
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class