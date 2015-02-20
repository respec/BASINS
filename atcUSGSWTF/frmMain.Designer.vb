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
        Me.gbFindRecharge = New System.Windows.Forms.GroupBox
        Me.gbEstimateRecharge = New System.Windows.Forms.GroupBox
        Me.gbChooseAntMethod.SuspendLayout()
        Me.gbFindRecharge.SuspendLayout()
        Me.gbEstimateRecharge.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFindRechargeEvents
        '
        Me.btnFindRechargeEvents.Location = New System.Drawing.Point(8, 26)
        Me.btnFindRechargeEvents.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnFindRechargeEvents.Name = "btnFindRechargeEvents"
        Me.btnFindRechargeEvents.Size = New System.Drawing.Size(361, 28)
        Me.btnFindRechargeEvents.TabIndex = 2
        Me.btnFindRechargeEvents.Text = "Find Recharge Events"
        Me.btnFindRechargeEvents.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 341)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Aquifer Specific Yield:"
        Me.Label1.Visible = False
        '
        'txtSy
        '
        Me.txtSy.Location = New System.Drawing.Point(170, 338)
        Me.txtSy.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtSy.Name = "txtSy"
        Me.txtSy.Size = New System.Drawing.Size(199, 22)
        Me.txtSy.TabIndex = 4
        Me.txtSy.Visible = False
        '
        'btnWriteOutput
        '
        Me.btnWriteOutput.Location = New System.Drawing.Point(9, 22)
        Me.btnWriteOutput.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnWriteOutput.Name = "btnWriteOutput"
        Me.btnWriteOutput.Size = New System.Drawing.Size(358, 28)
        Me.btnWriteOutput.TabIndex = 5
        Me.btnWriteOutput.Text = "Estimate Recharge"
        Me.btnWriteOutput.UseVisualStyleBackColor = True
        '
        'gbChooseAntMethod
        '
        Me.gbChooseAntMethod.Controls.Add(Me.btnAntMethodSpecifyParm)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodPower)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodLinear)
        Me.gbChooseAntMethod.Controls.Add(Me.rdoAntMethodFall)
        Me.gbChooseAntMethod.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbChooseAntMethod.Location = New System.Drawing.Point(0, 0)
        Me.gbChooseAntMethod.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbChooseAntMethod.Name = "gbChooseAntMethod"
        Me.gbChooseAntMethod.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbChooseAntMethod.Size = New System.Drawing.Size(387, 79)
        Me.gbChooseAntMethod.TabIndex = 6
        Me.gbChooseAntMethod.TabStop = False
        Me.gbChooseAntMethod.Text = "Step 1."
        '
        'btnAntMethodSpecifyParm
        '
        Me.btnAntMethodSpecifyParm.Location = New System.Drawing.Point(8, 23)
        Me.btnAntMethodSpecifyParm.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAntMethodSpecifyParm.Name = "btnAntMethodSpecifyParm"
        Me.btnAntMethodSpecifyParm.Size = New System.Drawing.Size(359, 28)
        Me.btnAntMethodSpecifyParm.TabIndex = 3
        Me.btnAntMethodSpecifyParm.Text = "Estimate Groundwater Recession Parameters"
        Me.btnAntMethodSpecifyParm.UseVisualStyleBackColor = True
        '
        'rdoAntMethodPower
        '
        Me.rdoAntMethodPower.AutoSize = True
        Me.rdoAntMethodPower.Location = New System.Drawing.Point(9, 84)
        Me.rdoAntMethodPower.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoAntMethodPower.Name = "rdoAntMethodPower"
        Me.rdoAntMethodPower.Size = New System.Drawing.Size(197, 21)
        Me.rdoAntMethodPower.TabIndex = 2
        Me.rdoAntMethodPower.Text = "The Power Function Model"
        Me.rdoAntMethodPower.UseVisualStyleBackColor = True
        Me.rdoAntMethodPower.Visible = False
        '
        'rdoAntMethodLinear
        '
        Me.rdoAntMethodLinear.AutoSize = True
        Me.rdoAntMethodLinear.Location = New System.Drawing.Point(9, 54)
        Me.rdoAntMethodLinear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoAntMethodLinear.Name = "rdoAntMethodLinear"
        Me.rdoAntMethodLinear.Size = New System.Drawing.Size(140, 21)
        Me.rdoAntMethodLinear.TabIndex = 1
        Me.rdoAntMethodLinear.Text = "The Linear Model"
        Me.rdoAntMethodLinear.UseVisualStyleBackColor = True
        Me.rdoAntMethodLinear.Visible = False
        '
        'rdoAntMethodFall
        '
        Me.rdoAntMethodFall.AutoSize = True
        Me.rdoAntMethodFall.Checked = True
        Me.rdoAntMethodFall.Location = New System.Drawing.Point(9, 25)
        Me.rdoAntMethodFall.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rdoAntMethodFall.Name = "rdoAntMethodFall"
        Me.rdoAntMethodFall.Size = New System.Drawing.Size(197, 21)
        Me.rdoAntMethodFall.TabIndex = 0
        Me.rdoAntMethodFall.TabStop = True
        Me.rdoAntMethodFall.Text = "Semi-log Regression/FALL"
        Me.rdoAntMethodFall.UseVisualStyleBackColor = True
        Me.rdoAntMethodFall.Visible = False
        '
        'gbFindRecharge
        '
        Me.gbFindRecharge.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFindRecharge.Controls.Add(Me.btnFindRechargeEvents)
        Me.gbFindRecharge.Location = New System.Drawing.Point(0, 87)
        Me.gbFindRecharge.Name = "gbFindRecharge"
        Me.gbFindRecharge.Size = New System.Drawing.Size(387, 80)
        Me.gbFindRecharge.TabIndex = 7
        Me.gbFindRecharge.TabStop = False
        Me.gbFindRecharge.Text = "Step 2."
        '
        'gbEstimateRecharge
        '
        Me.gbEstimateRecharge.Controls.Add(Me.btnWriteOutput)
        Me.gbEstimateRecharge.Location = New System.Drawing.Point(0, 173)
        Me.gbEstimateRecharge.Name = "gbEstimateRecharge"
        Me.gbEstimateRecharge.Size = New System.Drawing.Size(387, 76)
        Me.gbEstimateRecharge.TabIndex = 8
        Me.gbEstimateRecharge.TabStop = False
        Me.gbEstimateRecharge.Text = "Step 3."
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(387, 267)
        Me.Controls.Add(Me.gbEstimateRecharge)
        Me.Controls.Add(Me.gbFindRecharge)
        Me.Controls.Add(Me.gbChooseAntMethod)
        Me.Controls.Add(Me.txtSy)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmMain"
        Me.Text = "Water-Table Fluctuation Method"
        Me.gbChooseAntMethod.ResumeLayout(False)
        Me.gbChooseAntMethod.PerformLayout()
        Me.gbFindRecharge.ResumeLayout(False)
        Me.gbEstimateRecharge.ResumeLayout(False)
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
    Friend WithEvents gbFindRecharge As System.Windows.Forms.GroupBox
    Friend WithEvents gbEstimateRecharge As System.Windows.Forms.GroupBox
End Class
