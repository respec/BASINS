<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RunMultiWeppForm1
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtRunCount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRunStatus = New System.Windows.Forms.Label()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSlopePath = New System.Windows.Forms.Button()
        Me.txtPathSlope = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnOutPath = New System.Windows.Forms.Button()
        Me.txtPathOutput = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnWeppPath = New System.Windows.Forms.Button()
        Me.txtPathWepp = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBasePath = New System.Windows.Forms.Button()
        Me.txtPathBase = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSlopeStart = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtSlopeStop = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSlopeDelta = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtLengthStart = New System.Windows.Forms.TextBox()
        Me.txtLengthStop = New System.Windows.Forms.TextBox()
        Me.txtLengthDelta = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.txtSlopeSteps = New System.Windows.Forms.TextBox()
        Me.txtLengthSteps = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnPathPlot = New System.Windows.Forms.Button()
        Me.txtPathPlot = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(27, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(689, 567)
        Me.TabControl1.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(681, 541)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Road (Slope vs. Length)"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtRunCount)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtRunStatus)
        Me.GroupBox3.Controls.Add(Me.btnExecute)
        Me.GroupBox3.Controls.Add(Me.ProgressBar1)
        Me.GroupBox3.Location = New System.Drawing.Point(35, 426)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(620, 99)
        Me.GroupBox3.TabIndex = 27
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Execute"
        '
        'txtRunCount
        '
        Me.txtRunCount.AutoSize = True
        Me.txtRunCount.Location = New System.Drawing.Point(78, 60)
        Me.txtRunCount.Name = "txtRunCount"
        Me.txtRunCount.Size = New System.Drawing.Size(13, 13)
        Me.txtRunCount.TabIndex = 36
        Me.txtRunCount.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Total Runs: "
        '
        'txtRunStatus
        '
        Me.txtRunStatus.AutoSize = True
        Me.txtRunStatus.Location = New System.Drawing.Point(162, 60)
        Me.txtRunStatus.Name = "txtRunStatus"
        Me.txtRunStatus.Size = New System.Drawing.Size(62, 13)
        Me.txtRunStatus.TabIndex = 34
        Me.txtRunStatus.Text = "<Press Go>"
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(5, 24)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(114, 28)
        Me.btnExecute.TabIndex = 25
        Me.btnExecute.Text = "Go!"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(165, 29)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(433, 19)
        Me.ProgressBar1.TabIndex = 26
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.btnPathPlot)
        Me.GroupBox2.Controls.Add(Me.txtPathPlot)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.btnSlopePath)
        Me.GroupBox2.Controls.Add(Me.txtPathSlope)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnOutPath)
        Me.GroupBox2.Controls.Add(Me.txtPathOutput)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnWeppPath)
        Me.GroupBox2.Controls.Add(Me.txtPathWepp)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.btnBasePath)
        Me.GroupBox2.Controls.Add(Me.txtPathBase)
        Me.GroupBox2.Location = New System.Drawing.Point(30, 16)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(620, 202)
        Me.GroupBox2.TabIndex = 24
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Paths"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(25, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(125, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Modified Slope Out Path:"
        '
        'btnSlopePath
        '
        Me.btnSlopePath.Location = New System.Drawing.Point(165, 59)
        Me.btnSlopePath.Name = "btnSlopePath"
        Me.btnSlopePath.Size = New System.Drawing.Size(28, 25)
        Me.btnSlopePath.TabIndex = 20
        Me.btnSlopePath.Text = "..."
        Me.btnSlopePath.UseVisualStyleBackColor = True
        '
        'txtPathSlope
        '
        Me.txtPathSlope.Location = New System.Drawing.Point(204, 61)
        Me.txtPathSlope.Name = "txtPathSlope"
        Me.txtPathSlope.Size = New System.Drawing.Size(394, 20)
        Me.txtPathSlope.TabIndex = 21
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(25, 155)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Output Folder:"
        '
        'btnOutPath
        '
        Me.btnOutPath.Location = New System.Drawing.Point(165, 149)
        Me.btnOutPath.Name = "btnOutPath"
        Me.btnOutPath.Size = New System.Drawing.Size(28, 25)
        Me.btnOutPath.TabIndex = 17
        Me.btnOutPath.Text = "..."
        Me.btnOutPath.UseVisualStyleBackColor = True
        '
        'txtPathOutput
        '
        Me.txtPathOutput.Location = New System.Drawing.Point(204, 151)
        Me.txtPathOutput.Name = "txtPathOutput"
        Me.txtPathOutput.Size = New System.Drawing.Size(394, 20)
        Me.txtPathOutput.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(124, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Wepp Executable Folder"
        '
        'btnWeppPath
        '
        Me.btnWeppPath.Location = New System.Drawing.Point(165, 88)
        Me.btnWeppPath.Name = "btnWeppPath"
        Me.btnWeppPath.Size = New System.Drawing.Size(28, 25)
        Me.btnWeppPath.TabIndex = 14
        Me.btnWeppPath.Text = "..."
        Me.btnWeppPath.UseVisualStyleBackColor = True
        '
        'txtPathWepp
        '
        Me.txtPathWepp.Location = New System.Drawing.Point(204, 90)
        Me.txtPathWepp.Name = "txtPathWepp"
        Me.txtPathWepp.Size = New System.Drawing.Size(394, 20)
        Me.txtPathWepp.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(108, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Base Slope File Path:"
        '
        'btnBasePath
        '
        Me.btnBasePath.Location = New System.Drawing.Point(165, 30)
        Me.btnBasePath.Name = "btnBasePath"
        Me.btnBasePath.Size = New System.Drawing.Size(28, 25)
        Me.btnBasePath.TabIndex = 11
        Me.btnBasePath.Text = "..."
        Me.btnBasePath.UseVisualStyleBackColor = True
        '
        'txtPathBase
        '
        Me.txtPathBase.Location = New System.Drawing.Point(204, 32)
        Me.txtPathBase.Name = "txtPathBase"
        Me.txtPathBase.Size = New System.Drawing.Size(394, 20)
        Me.txtPathBase.TabIndex = 12
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtLengthSteps)
        Me.GroupBox1.Controls.Add(Me.txtSlopeSteps)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtSlopeStart)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.txtSlopeStop)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtSlopeDelta)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.txtLengthStart)
        Me.GroupBox1.Controls.Add(Me.txtLengthStop)
        Me.GroupBox1.Controls.Add(Me.txtLengthDelta)
        Me.GroupBox1.Location = New System.Drawing.Point(40, 283)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(620, 137)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Variables"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2, 33)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 13)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Slope Values (%)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(336, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 13)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Delta"
        '
        'txtSlopeStart
        '
        Me.txtSlopeStart.Location = New System.Drawing.Point(131, 33)
        Me.txtSlopeStart.Name = "txtSlopeStart"
        Me.txtSlopeStart.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeStart.TabIndex = 22
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(247, 17)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 13)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "Stop"
        '
        'txtSlopeStop
        '
        Me.txtSlopeStop.Location = New System.Drawing.Point(222, 33)
        Me.txtSlopeStop.Name = "txtSlopeStop"
        Me.txtSlopeStop.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeStop.TabIndex = 23
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(156, 17)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(29, 13)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Start"
        '
        'txtSlopeDelta
        '
        Me.txtSlopeDelta.Location = New System.Drawing.Point(313, 33)
        Me.txtSlopeDelta.Name = "txtSlopeDelta"
        Me.txtSlopeDelta.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeDelta.TabIndex = 24
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(2, 72)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(83, 13)
        Me.Label13.TabIndex = 29
        Me.Label13.Text = "Road length (ft.)"
        '
        'txtLengthStart
        '
        Me.txtLengthStart.Location = New System.Drawing.Point(131, 69)
        Me.txtLengthStart.Name = "txtLengthStart"
        Me.txtLengthStart.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthStart.TabIndex = 25
        '
        'txtLengthStop
        '
        Me.txtLengthStop.Location = New System.Drawing.Point(222, 69)
        Me.txtLengthStop.Name = "txtLengthStop"
        Me.txtLengthStop.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthStop.TabIndex = 26
        '
        'txtLengthDelta
        '
        Me.txtLengthDelta.Location = New System.Drawing.Point(313, 69)
        Me.txtLengthDelta.Name = "txtLengthDelta"
        Me.txtLengthDelta.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthDelta.TabIndex = 27
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'txtSlopeSteps
        '
        Me.txtSlopeSteps.Enabled = False
        Me.txtSlopeSteps.Location = New System.Drawing.Point(398, 33)
        Me.txtSlopeSteps.Name = "txtSlopeSteps"
        Me.txtSlopeSteps.Size = New System.Drawing.Size(79, 20)
        Me.txtSlopeSteps.TabIndex = 33
        '
        'txtLengthSteps
        '
        Me.txtLengthSteps.Enabled = False
        Me.txtLengthSteps.Location = New System.Drawing.Point(398, 69)
        Me.txtLengthSteps.Name = "txtLengthSteps"
        Me.txtLengthSteps.Size = New System.Drawing.Size(79, 20)
        Me.txtLengthSteps.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(421, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Steps"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(25, 124)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Plot File Path:"
        '
        'btnPathPlot
        '
        Me.btnPathPlot.Location = New System.Drawing.Point(165, 118)
        Me.btnPathPlot.Name = "btnPathPlot"
        Me.btnPathPlot.Size = New System.Drawing.Size(28, 25)
        Me.btnPathPlot.TabIndex = 23
        Me.btnPathPlot.Text = "..."
        Me.btnPathPlot.UseVisualStyleBackColor = True
        '
        'txtPathPlot
        '
        Me.txtPathPlot.Location = New System.Drawing.Point(204, 120)
        Me.txtPathPlot.Name = "txtPathPlot"
        Me.txtPathPlot.Size = New System.Drawing.Size(394, 20)
        Me.txtPathPlot.TabIndex = 24
        '
        'RunMultiWeppForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(753, 591)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RunMultiWeppForm1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Run Multi Wepp Tool"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeStart As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeStop As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSlopeDelta As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtLengthStart As System.Windows.Forms.TextBox
    Friend WithEvents txtLengthStop As System.Windows.Forms.TextBox
    Friend WithEvents txtLengthDelta As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtRunStatus As System.Windows.Forms.Label
    Friend WithEvents btnExecute As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOutPath As System.Windows.Forms.Button
    Friend WithEvents txtPathOutput As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnWeppPath As System.Windows.Forms.Button
    Friend WithEvents txtPathWepp As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBasePath As System.Windows.Forms.Button
    Friend WithEvents txtPathBase As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtRunCount As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSlopePath As System.Windows.Forms.Button
    Friend WithEvents txtPathSlope As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtLengthSteps As System.Windows.Forms.TextBox
    Friend WithEvents txtSlopeSteps As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnPathPlot As System.Windows.Forms.Button
    Friend WithEvents txtPathPlot As System.Windows.Forms.TextBox
End Class
