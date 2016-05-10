<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBatch))
        Me.txtSpecFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnDoBatch = New System.Windows.Forms.Button()
        Me.txtMsg = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtSpecFile
        '
        Me.txtSpecFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSpecFile.Location = New System.Drawing.Point(163, 19)
        Me.txtSpecFile.Name = "txtSpecFile"
        Me.txtSpecFile.Size = New System.Drawing.Size(277, 20)
        Me.txtSpecFile.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Batch Run Configuration File:"
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(446, 17)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 57)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(603, 23)
        Me.ProgressBar1.TabIndex = 3
        Me.ProgressBar1.Visible = False
        '
        'btnDoBatch
        '
        Me.btnDoBatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDoBatch.Location = New System.Drawing.Point(527, 17)
        Me.btnDoBatch.Name = "btnDoBatch"
        Me.btnDoBatch.Size = New System.Drawing.Size(90, 23)
        Me.btnDoBatch.TabIndex = 4
        Me.btnDoBatch.Text = "Do Batch Run"
        Me.btnDoBatch.UseVisualStyleBackColor = True
        '
        'txtMsg
        '
        Me.txtMsg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMsg.Location = New System.Drawing.Point(12, 87)
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMsg.Size = New System.Drawing.Size(603, 0)
        Me.txtMsg.TabIndex = 5
        Me.txtMsg.Visible = False
        '
        'frmBatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 56)
        Me.Controls.Add(Me.txtMsg)
        Me.Controls.Add(Me.btnDoBatch)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSpecFile)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBatch"
        Me.Text = "Base-Flow Analysis: Batch Run"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSpecFile As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents btnDoBatch As System.Windows.Forms.Button
    Friend WithEvents txtMsg As System.Windows.Forms.TextBox
End Class
