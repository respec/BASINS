<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.btnFindRechargeEvents = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSy = New System.Windows.Forms.TextBox
        Me.btnWriteOutput = New System.Windows.Forms.Button
        Me.gbChooseAntMethod = New System.Windows.Forms.GroupBox
        Me.btnAntMethodSpecifyParm = New System.Windows.Forms.Button
        Me.rdoAntMethodPower = New System.Windows.Forms.RadioButton
        Me.rdoAntMethodLinear = New System.Windows.Forms.RadioButton
        Me.rdoAntMethodFall = New System.Windows.Forms.RadioButton
        Me.gbChooseAntMethod.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFindRechargeEvents
        '
        Me.btnFindRechargeEvents.Location = New System.Drawing.Point(12, 178)
        Me.btnFindRechargeEvents.Name = "btnFindRechargeEvents"
        Me.btnFindRechargeEvents.Size = New System.Drawing.Size(265, 23)
        Me.btnFindRechargeEvents.TabIndex = 2
        Me.btnFindRechargeEvents.Text = "Find Recharge Events"
        Me.btnFindRechargeEvents.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 153)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Aquifer Specific Yield:"
        '
        'txtSy
        '
        Me.txtSy.Location = New System.Drawing.Point(127, 150)
        Me.txtSy.Name = "txtSy"
        Me.txtSy.Size = New System.Drawing.Size(150, 20)
        Me.txtSy.TabIndex = 4
        '
        'btnWriteOutput
        '
        Me.btnWriteOutput.Location = New System.Drawing.Point(12, 207)
        Me.btnWriteOutput.Name = "btnWriteOutput"
        Me.btnWriteOutput.Size = New System.Drawing.Size(265, 23)
        Me.btnWriteOutput.TabIndex = 5
        Me.btnWriteOutput.Text = "Calculate Recharge and Write Output"
        Me.btnWriteOutput.UseVisualStyleBackColor = True
        '
        'gbChooseAntMethod
        '
        Me.gbChooseAntMethod.Controls.Add(Me.btnAntMethodSpecifyParm)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodPower)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodLinear)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodFall)
        Me.gbChooseAntMethod.Location = New System.Drawing.Point(13, 13)
        Me.gbChooseAntMethod.Name = "gbChooseAntMethod"
        Me.gbChooseAntMethod.Size = New System.Drawing.Size(265, 127)
        Me.gbChooseAntMethod.TabIndex = 6
        Me.gbChooseAntMethod.TabStop = False
        Me.gbChooseAntMethod.Text = "Choose Antecedent GWL Estimate Method"
        '
        'btnAntMethodSpecifyParm
        '
        Me.btnAntMethodSpecifyParm.Location = New System.Drawing.Point(7, 92)
        Me.btnAntMethodSpecifyParm.Name = "btnAntMethodSpecifyParm"
        Me.btnAntMethodSpecifyParm.Size = New System.Drawing.Size(153, 23)
        Me.btnAntMethodSpecifyParm.TabIndex = 3
        Me.btnAntMethodSpecifyParm.Text = "Specify Method Parameters"
        Me.btnAntMethodSpecifyParm.UseVisualStyleBackColor = True
        '
        'rdoAntMethodPower
        '
        Me.rdoAntMethodPower.AutoSize = True
        Me.rdoAntMethodPower.Location = New System.Drawing.Point(7, 68)
        Me.rdoAntMethodPower.Name = "rdoAntMethodPower"
        Me.rdoAntMethodPower.Size = New System.Drawing.Size(153, 17)
        Me.rdoAntMethodPower.TabIndex = 2
        Me.rdoAntMethodPower.TabStop = True
        Me.rdoAntMethodPower.Text = "The Power Function Model"
        Me.rdoAntMethodPower.UseVisualStyleBackColor = True
        '
        'rdoAntMethodLinear
        '
        Me.rdoAntMethodLinear.AutoSize = True
        Me.rdoAntMethodLinear.Location = New System.Drawing.Point(7, 44)
        Me.rdoAntMethodLinear.Name = "rdoAntMethodLinear"
        Me.rdoAntMethodLinear.Size = New System.Drawing.Size(108, 17)
        Me.rdoAntMethodLinear.TabIndex = 1
        Me.rdoAntMethodLinear.TabStop = True
        Me.rdoAntMethodLinear.Text = "The Linear Model"
        Me.rdoAntMethodLinear.UseVisualStyleBackColor = True
        '
        'rdoAntMethodFall
        '
        Me.rdoAntMethodFall.AutoSize = True
        Me.rdoAntMethodFall.Location = New System.Drawing.Point(7, 20)
        Me.rdoAntMethodFall.Name = "rdoAntMethodFall"
        Me.rdoAntMethodFall.Size = New System.Drawing.Size(151, 17)
        Me.rdoAntMethodFall.TabIndex = 0
        Me.rdoAntMethodFall.TabStop = True
        Me.rdoAntMethodFall.Text = "Semi-log Regression/FALL"
        Me.rdoAntMethodFall.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(290, 362)
        Me.Controls.Add(Me.gbChooseAntMethod)
        Me.Controls.Add(Me.btnWriteOutput)
        Me.Controls.Add(Me.txtSy)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnFindRechargeEvents)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Water Table Fluctuation Method"
        Me.gbChooseAntMethod.ResumeLayout(False)
        Me.gbChooseAntMethod.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnFindRechargeEvents As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSy As System.Windows.Forms.TextBox
    Friend WithEvents btnWriteOutput As System.Windows.Forms.Button
    Friend WithEvents gbChooseAntMethod As System.Windows.Forms.GroupBox
    Friend WithEvents rdoAntMethodFall As System.Windows.Forms.RadioButton
    Friend WithEvents btnAntMethodSpecifyParm As System.Windows.Forms.Button
    Friend WithEvents rdoAntMethodPower As System.Windows.Forms.RadioButton
    Friend WithEvents rdoAntMethodLinear As System.Windows.Forms.RadioButton
End Class
