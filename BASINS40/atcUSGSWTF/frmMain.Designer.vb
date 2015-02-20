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
        Me.btnWriteOutput = New System.Windows.Forms.Button
        Me.gbChooseAntMethod = New System.Windows.Forms.GroupBox
        Me.btnAntMethodSpecifyParm = New System.Windows.Forms.Button
        Me.gbFindRecharge = New System.Windows.Forms.GroupBox
        Me.gbEstimateRecharge = New System.Windows.Forms.GroupBox
        Me.gbChooseAntMethod.SuspendLayout()
        Me.gbFindRecharge.SuspendLayout()
        Me.gbEstimateRecharge.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnFindRechargeEvents
        '
        Me.btnFindRechargeEvents.Location = New System.Drawing.Point(6, 21)
        Me.btnFindRechargeEvents.Name = "btnFindRechargeEvents"
        Me.btnFindRechargeEvents.Size = New System.Drawing.Size(271, 23)
        Me.btnFindRechargeEvents.TabIndex = 2
        Me.btnFindRechargeEvents.Text = "Find Recharge Events"
        Me.btnFindRechargeEvents.UseVisualStyleBackColor = True
        '
        'btnWriteOutput
        '
        Me.btnWriteOutput.Location = New System.Drawing.Point(7, 18)
        Me.btnWriteOutput.Name = "btnWriteOutput"
        Me.btnWriteOutput.Size = New System.Drawing.Size(268, 23)
        Me.btnWriteOutput.TabIndex = 5
        Me.btnWriteOutput.Text = "Estimate Recharge"
        Me.btnWriteOutput.UseVisualStyleBackColor = True
        '
        'gbChooseAntMethod
        '
        Me.gbChooseAntMethod.Controls.Add(Me.btnAntMethodSpecifyParm)
        Me.gbChooseAntMethod.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbChooseAntMethod.Location = New System.Drawing.Point(0, 0)
        Me.gbChooseAntMethod.Name = "gbChooseAntMethod"
        Me.gbChooseAntMethod.Size = New System.Drawing.Size(290, 59)
        Me.gbChooseAntMethod.TabIndex = 6
        Me.gbChooseAntMethod.TabStop = False
        Me.gbChooseAntMethod.Text = "Step 1."
        '
        'btnAntMethodSpecifyParm
        '
        Me.btnAntMethodSpecifyParm.Location = New System.Drawing.Point(6, 19)
        Me.btnAntMethodSpecifyParm.Name = "btnAntMethodSpecifyParm"
        Me.btnAntMethodSpecifyParm.Size = New System.Drawing.Size(269, 23)
        Me.btnAntMethodSpecifyParm.TabIndex = 3
        Me.btnAntMethodSpecifyParm.Text = "Estimate Groundwater Recession Parameters"
        Me.btnAntMethodSpecifyParm.UseVisualStyleBackColor = True
        '
        'gbFindRecharge
        '
        Me.gbFindRecharge.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFindRecharge.Controls.Add(Me.btnFindRechargeEvents)
        Me.gbFindRecharge.Location = New System.Drawing.Point(0, 64)
        Me.gbFindRecharge.Margin = New System.Windows.Forms.Padding(2)
        Me.gbFindRecharge.Name = "gbFindRecharge"
        Me.gbFindRecharge.Padding = New System.Windows.Forms.Padding(2)
        Me.gbFindRecharge.Size = New System.Drawing.Size(290, 65)
        Me.gbFindRecharge.TabIndex = 7
        Me.gbFindRecharge.TabStop = False
        Me.gbFindRecharge.Text = "Step 2."
        '
        'gbEstimateRecharge
        '
        Me.gbEstimateRecharge.Controls.Add(Me.btnWriteOutput)
        Me.gbEstimateRecharge.Location = New System.Drawing.Point(0, 133)
        Me.gbEstimateRecharge.Margin = New System.Windows.Forms.Padding(2)
        Me.gbEstimateRecharge.Name = "gbEstimateRecharge"
        Me.gbEstimateRecharge.Padding = New System.Windows.Forms.Padding(2)
        Me.gbEstimateRecharge.Size = New System.Drawing.Size(290, 62)
        Me.gbEstimateRecharge.TabIndex = 8
        Me.gbEstimateRecharge.TabStop = False
        Me.gbEstimateRecharge.Text = "Step 3."
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(290, 203)
        Me.Controls.Add(Me.gbEstimateRecharge)
        Me.Controls.Add(Me.gbFindRecharge)
        Me.Controls.Add(Me.gbChooseAntMethod)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Water-Table Fluctuation Method"
        Me.gbChooseAntMethod.ResumeLayout(False)
        Me.gbFindRecharge.ResumeLayout(False)
        Me.gbEstimateRecharge.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnFindRechargeEvents As System.Windows.Forms.Button
    Friend WithEvents btnWriteOutput As System.Windows.Forms.Button
    Friend WithEvents gbChooseAntMethod As System.Windows.Forms.GroupBox
    Friend WithEvents btnAntMethodSpecifyParm As System.Windows.Forms.Button
    Friend WithEvents gbFindRecharge As System.Windows.Forms.GroupBox
    Friend WithEvents gbEstimateRecharge As System.Windows.Forms.GroupBox
End Class
