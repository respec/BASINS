<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchRun
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
        Me.lblApplication = New System.Windows.Forms.Label()
        Me.txtApplication = New System.Windows.Forms.TextBox()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.txtDirectory = New System.Windows.Forms.TextBox()
        Me.lblDirectory = New System.Windows.Forms.Label()
        Me.txtParallel = New System.Windows.Forms.TextBox()
        Me.lblParallel = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lblArguments = New System.Windows.Forms.Label()
        Me.txtArguments = New System.Windows.Forms.TextBox()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.txtProgress = New System.Windows.Forms.TextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblApplication
        '
        Me.lblApplication.AutoSize = True
        Me.lblApplication.Location = New System.Drawing.Point(12, 15)
        Me.lblApplication.Name = "lblApplication"
        Me.lblApplication.Size = New System.Drawing.Size(59, 13)
        Me.lblApplication.TabIndex = 0
        Me.lblApplication.Text = "Application"
        '
        'txtApplication
        '
        Me.txtApplication.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtApplication.Location = New System.Drawing.Point(77, 12)
        Me.txtApplication.Name = "txtApplication"
        Me.txtApplication.Size = New System.Drawing.Size(641, 20)
        Me.txtApplication.TabIndex = 1
        '
        'btnRun
        '
        Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRun.Location = New System.Drawing.Point(643, 332)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 8
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'txtDirectory
        '
        Me.txtDirectory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDirectory.Location = New System.Drawing.Point(77, 38)
        Me.txtDirectory.Name = "txtDirectory"
        Me.txtDirectory.Size = New System.Drawing.Size(641, 20)
        Me.txtDirectory.TabIndex = 3
        '
        'lblDirectory
        '
        Me.lblDirectory.AutoSize = True
        Me.lblDirectory.Location = New System.Drawing.Point(12, 41)
        Me.lblDirectory.Name = "lblDirectory"
        Me.lblDirectory.Size = New System.Drawing.Size(49, 13)
        Me.lblDirectory.TabIndex = 2
        Me.lblDirectory.Text = "Directory"
        '
        'txtParallel
        '
        Me.txtParallel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtParallel.Location = New System.Drawing.Point(77, 332)
        Me.txtParallel.Name = "txtParallel"
        Me.txtParallel.Size = New System.Drawing.Size(57, 20)
        Me.txtParallel.TabIndex = 7
        Me.txtParallel.Text = "1"
        Me.txtParallel.Visible = False
        '
        'lblParallel
        '
        Me.lblParallel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblParallel.AutoSize = True
        Me.lblParallel.Location = New System.Drawing.Point(12, 335)
        Me.lblParallel.Name = "lblParallel"
        Me.lblParallel.Size = New System.Drawing.Size(53, 13)
        Me.lblParallel.TabIndex = 6
        Me.lblParallel.Text = "In Parallel"
        Me.lblParallel.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 66)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblArguments)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtArguments)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblProgress)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtProgress)
        Me.SplitContainer1.Size = New System.Drawing.Size(729, 260)
        Me.SplitContainer1.SplitterDistance = 130
        Me.SplitContainer1.TabIndex = 9
        '
        'lblArguments
        '
        Me.lblArguments.AutoSize = True
        Me.lblArguments.Location = New System.Drawing.Point(12, 6)
        Me.lblArguments.Name = "lblArguments"
        Me.lblArguments.Size = New System.Drawing.Size(57, 13)
        Me.lblArguments.TabIndex = 6
        Me.lblArguments.Text = "Arguments"
        '
        'txtArguments
        '
        Me.txtArguments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtArguments.Location = New System.Drawing.Point(77, 3)
        Me.txtArguments.Multiline = True
        Me.txtArguments.Name = "txtArguments"
        Me.txtArguments.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtArguments.Size = New System.Drawing.Size(641, 124)
        Me.txtArguments.TabIndex = 7
        Me.txtArguments.WordWrap = False
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(11, 4)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(48, 13)
        Me.lblProgress.TabIndex = 8
        Me.lblProgress.Text = "Progress"
        '
        'txtProgress
        '
        Me.txtProgress.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProgress.Location = New System.Drawing.Point(76, 1)
        Me.txtProgress.Multiline = True
        Me.txtProgress.Name = "txtProgress"
        Me.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtProgress.Size = New System.Drawing.Size(641, 124)
        Me.txtProgress.TabIndex = 9
        Me.txtProgress.WordWrap = False
        '
        'frmBatchRun
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 367)
        Me.Controls.Add(Me.txtParallel)
        Me.Controls.Add(Me.lblParallel)
        Me.Controls.Add(Me.txtDirectory)
        Me.Controls.Add(Me.lblDirectory)
        Me.Controls.Add(Me.btnRun)
        Me.Controls.Add(Me.txtApplication)
        Me.Controls.Add(Me.lblApplication)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmBatchRun"
        Me.Text = "Batch Run"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblApplication As System.Windows.Forms.Label
    Friend WithEvents txtApplication As System.Windows.Forms.TextBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents txtDirectory As System.Windows.Forms.TextBox
    Friend WithEvents lblDirectory As System.Windows.Forms.Label
    Friend WithEvents txtParallel As System.Windows.Forms.TextBox
    Friend WithEvents lblParallel As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lblArguments As System.Windows.Forms.Label
    Friend WithEvents txtArguments As System.Windows.Forms.TextBox
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents txtProgress As System.Windows.Forms.TextBox

End Class
