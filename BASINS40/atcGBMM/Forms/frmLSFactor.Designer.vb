<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class FrmLSFactor
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents txtMaxSlope As System.Windows.Forms.TextBox
	Public WithEvents chkMaxSlope As System.Windows.Forms.CheckBox
	Public WithEvents optionExisting As System.Windows.Forms.RadioButton
	Public WithEvents optionDefaultLSFactor As System.Windows.Forms.RadioButton
	Public WithEvents txtCSL As System.Windows.Forms.TextBox
	Public WithEvents optionDefineSlopeLength As System.Windows.Forms.RadioButton
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdOK As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLSFactor))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.Frame1 = New System.Windows.Forms.GroupBox
        Me.txtMaxSlope = New System.Windows.Forms.TextBox
        Me.chkMaxSlope = New System.Windows.Forms.CheckBox
        Me.optionExisting = New System.Windows.Forms.RadioButton
        Me.optionDefaultLSFactor = New System.Windows.Forms.RadioButton
        Me.txtCSL = New System.Windows.Forms.TextBox
        Me.optionDefineSlopeLength = New System.Windows.Forms.RadioButton
        Me.cmdOK = New System.Windows.Forms.Button
        Me.Frame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(288, 176)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(54, 24)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.txtMaxSlope)
        Me.Frame1.Controls.Add(Me.chkMaxSlope)
        Me.Frame1.Controls.Add(Me.optionExisting)
        Me.Frame1.Controls.Add(Me.optionDefaultLSFactor)
        Me.Frame1.Controls.Add(Me.txtCSL)
        Me.Frame1.Controls.Add(Me.optionDefineSlopeLength)
        Me.Frame1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Location = New System.Drawing.Point(8, 8)
        Me.Frame1.Name = "Frame1"
        Me.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Frame1.Size = New System.Drawing.Size(337, 161)
        Me.Frame1.TabIndex = 0
        Me.Frame1.TabStop = False
        Me.Frame1.Text = "Define Slope Length"
        '
        'txtMaxSlope
        '
        Me.txtMaxSlope.AcceptsReturn = True
        Me.txtMaxSlope.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxSlope.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxSlope.Enabled = False
        Me.txtMaxSlope.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxSlope.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxSlope.Location = New System.Drawing.Point(280, 54)
        Me.txtMaxSlope.MaxLength = 0
        Me.txtMaxSlope.Name = "txtMaxSlope"
        Me.txtMaxSlope.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxSlope.Size = New System.Drawing.Size(41, 20)
        Me.txtMaxSlope.TabIndex = 2
        Me.txtMaxSlope.Text = "100"
        '
        'chkMaxSlope
        '
        Me.chkMaxSlope.BackColor = System.Drawing.SystemColors.Control
        Me.chkMaxSlope.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkMaxSlope.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMaxSlope.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMaxSlope.Location = New System.Drawing.Point(72, 56)
        Me.chkMaxSlope.Name = "chkMaxSlope"
        Me.chkMaxSlope.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMaxSlope.Size = New System.Drawing.Size(185, 17)
        Me.chkMaxSlope.TabIndex = 1
        Me.chkMaxSlope.Text = "Maximum Slope Length (m)"
        Me.chkMaxSlope.UseVisualStyleBackColor = False
        '
        'optionExisting
        '
        Me.optionExisting.BackColor = System.Drawing.SystemColors.Control
        Me.optionExisting.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionExisting.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionExisting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionExisting.Location = New System.Drawing.Point(16, 112)
        Me.optionExisting.Name = "optionExisting"
        Me.optionExisting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionExisting.Size = New System.Drawing.Size(225, 25)
        Me.optionExisting.TabIndex = 5
        Me.optionExisting.TabStop = True
        Me.optionExisting.Text = "Use Existing Slope Length and LSFactor"
        Me.optionExisting.UseVisualStyleBackColor = False
        '
        'optionDefaultLSFactor
        '
        Me.optionDefaultLSFactor.BackColor = System.Drawing.SystemColors.Control
        Me.optionDefaultLSFactor.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionDefaultLSFactor.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionDefaultLSFactor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionDefaultLSFactor.Location = New System.Drawing.Point(16, 24)
        Me.optionDefaultLSFactor.Name = "optionDefaultLSFactor"
        Me.optionDefaultLSFactor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionDefaultLSFactor.Size = New System.Drawing.Size(201, 25)
        Me.optionDefaultLSFactor.TabIndex = 0
        Me.optionDefaultLSFactor.TabStop = True
        Me.optionDefaultLSFactor.Text = "Use DEM based Slope Length"
        Me.optionDefaultLSFactor.UseVisualStyleBackColor = False
        '
        'txtCSL
        '
        Me.txtCSL.AcceptsReturn = True
        Me.txtCSL.BackColor = System.Drawing.SystemColors.Window
        Me.txtCSL.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCSL.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCSL.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCSL.Location = New System.Drawing.Point(280, 80)
        Me.txtCSL.MaxLength = 0
        Me.txtCSL.Name = "txtCSL"
        Me.txtCSL.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCSL.Size = New System.Drawing.Size(41, 20)
        Me.txtCSL.TabIndex = 4
        Me.txtCSL.Text = "30"
        '
        'optionDefineSlopeLength
        '
        Me.optionDefineSlopeLength.BackColor = System.Drawing.SystemColors.Control
        Me.optionDefineSlopeLength.Checked = True
        Me.optionDefineSlopeLength.Cursor = System.Windows.Forms.Cursors.Default
        Me.optionDefineSlopeLength.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optionDefineSlopeLength.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optionDefineSlopeLength.Location = New System.Drawing.Point(16, 80)
        Me.optionDefineSlopeLength.Name = "optionDefineSlopeLength"
        Me.optionDefineSlopeLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.optionDefineSlopeLength.Size = New System.Drawing.Size(273, 25)
        Me.optionDefineSlopeLength.TabIndex = 3
        Me.optionDefineSlopeLength.TabStop = True
        Me.optionDefineSlopeLength.Text = "Define a Constant Slope Length (m)  (default):"
        Me.optionDefineSlopeLength.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(228, 176)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(54, 24)
        Me.cmdOK.TabIndex = 1
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'FrmLSFactor
        '
        Me.AcceptButton = Me.cmdCancel
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(358, 206)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdOK)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Tahoma", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmLSFactor"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LS Factor"
        Me.Frame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class