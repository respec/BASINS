<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParams
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParams))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtFallD = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtFallKgw = New System.Windows.Forms.TextBox
        Me.gbFall = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.gbLinear = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtLinearB = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtLinearA = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.gbPower = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPowerExp = New System.Windows.Forms.TextBox
        Me.txtPowerDatum = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtPowerMultiplier = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtPowerIntercept = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.gbFall.SuspendLayout()
        Me.gbLinear.SuspendLayout()
        Me.gbPower.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Asymptotic GWL (d, ft):"
        '
        'txtFallD
        '
        Me.txtFallD.Location = New System.Drawing.Point(4, 59)
        Me.txtFallD.Name = "txtFallD"
        Me.txtFallD.Size = New System.Drawing.Size(233, 20)
        Me.txtFallD.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 95)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(224, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "GWL recession constant (Kgw, ln(GWL)/day):"
        '
        'txtFallKgw
        '
        Me.txtFallKgw.Location = New System.Drawing.Point(6, 111)
        Me.txtFallKgw.Name = "txtFallKgw"
        Me.txtFallKgw.Size = New System.Drawing.Size(231, 20)
        Me.txtFallKgw.TabIndex = 3
        '
        'gbFall
        '
        Me.gbFall.Controls.Add(Me.Label11)
        Me.gbFall.Controls.Add(Me.Label2)
        Me.gbFall.Controls.Add(Me.txtFallKgw)
        Me.gbFall.Controls.Add(Me.Label1)
        Me.gbFall.Controls.Add(Me.txtFallD)
        Me.gbFall.Location = New System.Drawing.Point(1, 1)
        Me.gbFall.Name = "gbFall"
        Me.gbFall.Size = New System.Drawing.Size(244, 150)
        Me.gbFall.TabIndex = 4
        Me.gbFall.TabStop = False
        Me.gbFall.Text = "Semi-log Equation Parameters"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Location = New System.Drawing.Point(6, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(192, 15)
        Me.Label11.TabIndex = 9
        Me.Label11.Text = "Ant.GWL = (H0 - d)^(Kgw(Tp - T0)) + d"
        '
        'gbLinear
        '
        Me.gbLinear.Controls.Add(Me.Label10)
        Me.gbLinear.Controls.Add(Me.txtLinearB)
        Me.gbLinear.Controls.Add(Me.Label4)
        Me.gbLinear.Controls.Add(Me.txtLinearA)
        Me.gbLinear.Controls.Add(Me.Label3)
        Me.gbLinear.Location = New System.Drawing.Point(1, 181)
        Me.gbLinear.Name = "gbLinear"
        Me.gbLinear.Size = New System.Drawing.Size(244, 150)
        Me.gbLinear.TabIndex = 5
        Me.gbLinear.TabStop = False
        Me.gbLinear.Text = "Linear Model Parameters"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Location = New System.Drawing.Point(6, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(176, 15)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Decline rate = dZwt/dt = a(Zwt) + b"
        '
        'txtLinearB
        '
        Me.txtLinearB.Location = New System.Drawing.Point(5, 108)
        Me.txtLinearB.Name = "txtLinearB"
        Me.txtLinearB.Size = New System.Drawing.Size(232, 20)
        Me.txtLinearB.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 92)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Rate Decline Intercept, b:"
        '
        'txtLinearA
        '
        Me.txtLinearA.Location = New System.Drawing.Point(6, 58)
        Me.txtLinearA.Name = "txtLinearA"
        Me.txtLinearA.Size = New System.Drawing.Size(232, 20)
        Me.txtLinearA.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 42)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(114, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Rate Decline Slope, a:"
        '
        'gbPower
        '
        Me.gbPower.Controls.Add(Me.Label9)
        Me.gbPower.Controls.Add(Me.Label8)
        Me.gbPower.Controls.Add(Me.txtPowerExp)
        Me.gbPower.Controls.Add(Me.txtPowerDatum)
        Me.gbPower.Controls.Add(Me.Label7)
        Me.gbPower.Controls.Add(Me.txtPowerMultiplier)
        Me.gbPower.Controls.Add(Me.Label6)
        Me.gbPower.Controls.Add(Me.txtPowerIntercept)
        Me.gbPower.Controls.Add(Me.Label5)
        Me.gbPower.Location = New System.Drawing.Point(1, 337)
        Me.gbPower.Name = "gbPower"
        Me.gbPower.Size = New System.Drawing.Size(244, 150)
        Me.gbPower.TabIndex = 6
        Me.gbPower.TabStop = False
        Me.gbPower.Text = "Power Model Parameters"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Location = New System.Drawing.Point(6, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(209, 15)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Decline rate = dZwt/dt = -(c + d(Zwt - e)^f)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Exponent, f:"
        '
        'txtPowerExp
        '
        Me.txtPowerExp.Location = New System.Drawing.Point(90, 121)
        Me.txtPowerExp.Name = "txtPowerExp"
        Me.txtPowerExp.Size = New System.Drawing.Size(148, 20)
        Me.txtPowerExp.TabIndex = 6
        '
        'txtPowerDatum
        '
        Me.txtPowerDatum.Location = New System.Drawing.Point(90, 94)
        Me.txtPowerDatum.Name = "txtPowerDatum"
        Me.txtPowerDatum.Size = New System.Drawing.Size(148, 20)
        Me.txtPowerDatum.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 97)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(53, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Datum, e:"
        '
        'txtPowerMultiplier
        '
        Me.txtPowerMultiplier.Location = New System.Drawing.Point(90, 68)
        Me.txtPowerMultiplier.Name = "txtPowerMultiplier"
        Me.txtPowerMultiplier.Size = New System.Drawing.Size(148, 20)
        Me.txtPowerMultiplier.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 71)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Multiplier, d:"
        '
        'txtPowerIntercept
        '
        Me.txtPowerIntercept.Location = New System.Drawing.Point(90, 42)
        Me.txtPowerIntercept.Name = "txtPowerIntercept"
        Me.txtPowerIntercept.Size = New System.Drawing.Size(148, 20)
        Me.txtPowerIntercept.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 45)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Intercept, c:"
        '
        'frmParams
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(246, 475)
        Me.Controls.Add(Me.gbPower)
        Me.Controls.Add(Me.gbLinear)
        Me.Controls.Add(Me.gbFall)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmParams"
        Me.Text = "Specify Parameters"
        Me.gbFall.ResumeLayout(False)
        Me.gbFall.PerformLayout()
        Me.gbLinear.ResumeLayout(False)
        Me.gbLinear.PerformLayout()
        Me.gbPower.ResumeLayout(False)
        Me.gbPower.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFallD As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFallKgw As System.Windows.Forms.TextBox
    Friend WithEvents gbFall As System.Windows.Forms.GroupBox
    Friend WithEvents gbLinear As System.Windows.Forms.GroupBox
    Friend WithEvents txtLinearA As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtLinearB As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents gbPower As System.Windows.Forms.GroupBox
    Friend WithEvents txtPowerDatum As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtPowerMultiplier As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPowerIntercept As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPowerExp As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
End Class
