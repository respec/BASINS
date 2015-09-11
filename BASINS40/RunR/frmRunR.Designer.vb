<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRunR
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRunR))
        Me.txtR = New System.Windows.Forms.TextBox()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.txtYears = New System.Windows.Forms.TextBox()
        Me.lblYears = New System.Windows.Forms.Label()
        Me.lblValues = New System.Windows.Forms.Label()
        Me.txtValues = New System.Windows.Forms.TextBox()
        Me.lblScript = New System.Windows.Forms.Label()
        Me.lblConfidence = New System.Windows.Forms.Label()
        Me.txtConfidence = New System.Windows.Forms.TextBox()
        Me.lblPercent = New System.Windows.Forms.Label()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.ComboResult = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'txtR
        '
        Me.txtR.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtR.Location = New System.Drawing.Point(12, 143)
        Me.txtR.Multiline = True
        Me.txtR.Name = "txtR"
        Me.txtR.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtR.Size = New System.Drawing.Size(625, 263)
        Me.txtR.TabIndex = 0
        Me.txtR.Text = resources.GetString("txtR.Text")
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(562, 412)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 1
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'txtYears
        '
        Me.txtYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtYears.Location = New System.Drawing.Point(57, 12)
        Me.txtYears.Name = "txtYears"
        Me.txtYears.Size = New System.Drawing.Size(580, 20)
        Me.txtYears.TabIndex = 2
        Me.txtYears.Text = resources.GetString("txtYears.Text")
        '
        'lblYears
        '
        Me.lblYears.AutoSize = True
        Me.lblYears.Location = New System.Drawing.Point(12, 15)
        Me.lblYears.Name = "lblYears"
        Me.lblYears.Size = New System.Drawing.Size(34, 13)
        Me.lblYears.TabIndex = 3
        Me.lblYears.Text = "Years"
        '
        'lblValues
        '
        Me.lblValues.AutoSize = True
        Me.lblValues.Location = New System.Drawing.Point(12, 41)
        Me.lblValues.Name = "lblValues"
        Me.lblValues.Size = New System.Drawing.Size(39, 13)
        Me.lblValues.TabIndex = 5
        Me.lblValues.Text = "Values"
        '
        'txtValues
        '
        Me.txtValues.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtValues.Location = New System.Drawing.Point(57, 38)
        Me.txtValues.Name = "txtValues"
        Me.txtValues.Size = New System.Drawing.Size(580, 20)
        Me.txtValues.TabIndex = 4
        Me.txtValues.Text = "3, 6, 6, 7, 7, 9, 9, 9, 9, 10, 10, 10, 10, 10, 11, 14, 14, 16, 16, 16, 16, 18, 19" &
    ", 20, 21, 22, 23, 26, 27, 28, 28, 28, 30, 33, 33, 34, 37, 42, 45"
        '
        'lblScript
        '
        Me.lblScript.AutoSize = True
        Me.lblScript.Location = New System.Drawing.Point(12, 127)
        Me.lblScript.Name = "lblScript"
        Me.lblScript.Size = New System.Drawing.Size(102, 13)
        Me.lblScript.TabIndex = 6
        Me.lblScript.Text = "Computational Code"
        '
        'lblConfidence
        '
        Me.lblConfidence.AutoSize = True
        Me.lblConfidence.Location = New System.Drawing.Point(12, 67)
        Me.lblConfidence.Name = "lblConfidence"
        Me.lblConfidence.Size = New System.Drawing.Size(99, 13)
        Me.lblConfidence.TabIndex = 8
        Me.lblConfidence.Text = "Confidence Interval"
        '
        'txtConfidence
        '
        Me.txtConfidence.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConfidence.Location = New System.Drawing.Point(157, 64)
        Me.txtConfidence.Name = "txtConfidence"
        Me.txtConfidence.Size = New System.Drawing.Size(56, 20)
        Me.txtConfidence.TabIndex = 7
        Me.txtConfidence.Text = "95"
        '
        'lblPercent
        '
        Me.lblPercent.AutoSize = True
        Me.lblPercent.Location = New System.Drawing.Point(219, 67)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.Size = New System.Drawing.Size(15, 13)
        Me.lblPercent.TabIndex = 9
        Me.lblPercent.Text = "%"
        '
        'lblResult
        '
        Me.lblResult.AutoSize = True
        Me.lblResult.Location = New System.Drawing.Point(12, 93)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(64, 13)
        Me.lblResult.TabIndex = 10
        Me.lblResult.Text = "Result Type"
        '
        'ComboResult
        '
        Me.ComboResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboResult.FormattingEnabled = True
        Me.ComboResult.Items.AddRange(New Object() {"TRUE: there is a trend FALSE: No trend", "rho", "rho, P", "rho, P, TRUE/FALSE", "rho, P, confidence intervals", "rho, P, confidence intervals, TRUE/FALSE", "rho, P, VB/VBA integer for TRUE/FALSE", "rho, P, VB/VBA integer for TRUE/FALSE", "rho, P, confidence intervals, VB/VBA integer for TRUE/FALSE", "VB/VBA integer for TRUE/FALSE"})
        Me.ComboResult.Location = New System.Drawing.Point(157, 90)
        Me.ComboResult.Name = "ComboResult"
        Me.ComboResult.Size = New System.Drawing.Size(480, 21)
        Me.ComboResult.TabIndex = 11
        '
        'frmRunR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(649, 447)
        Me.Controls.Add(Me.ComboResult)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.lblPercent)
        Me.Controls.Add(Me.lblConfidence)
        Me.Controls.Add(Me.txtConfidence)
        Me.Controls.Add(Me.lblScript)
        Me.Controls.Add(Me.lblValues)
        Me.Controls.Add(Me.txtValues)
        Me.Controls.Add(Me.lblYears)
        Me.Controls.Add(Me.txtYears)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.txtR)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRunR"
        Me.Text = "Run R"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtR As TextBox
    Friend WithEvents btnRun As Button
    Friend WithEvents txtYears As TextBox
    Friend WithEvents lblYears As Label
    Friend WithEvents lblValues As Label
    Friend WithEvents txtValues As TextBox
    Friend WithEvents lblScript As Label
    Friend WithEvents lblConfidence As Label
    Friend WithEvents txtConfidence As TextBox
    Friend WithEvents lblPercent As Label
    Friend WithEvents lblResult As Label
    Friend WithEvents ComboResult As ComboBox
End Class
