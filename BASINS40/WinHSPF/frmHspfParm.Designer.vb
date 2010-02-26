<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHspfParm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHspfParm))
        Me.cmdStart = New System.Windows.Forms.Button
        Me.fraResults = New System.Windows.Forms.GroupBox
        Me.fraParameter = New System.Windows.Forms.GroupBox
        Me.lstStarter = New System.Windows.Forms.ListBox
        Me.lstParms = New System.Windows.Forms.ListBox
        Me.lblStarter = New System.Windows.Forms.Label
        Me.lblParm = New System.Windows.Forms.Label
        Me.agdStarter = New atcControls.atcGrid
        Me.cmdApply = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.optLandUse = New System.Windows.Forms.RadioButton
        Me.optParameter = New System.Windows.Forms.RadioButton
        Me.lblFile = New System.Windows.Forms.Label
        Me.cmdFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdClose = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.fraResults.SuspendLayout()
        Me.fraParameter.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdStart
        '
        Me.cmdStart.Location = New System.Drawing.Point(247, 31)
        Me.cmdStart.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(177, 39)
        Me.cmdStart.TabIndex = 0
        Me.cmdStart.Text = "Start &HSPFParm"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'fraResults
        '
        Me.fraResults.Controls.Add(Me.fraParameter)
        Me.fraResults.Controls.Add(Me.agdStarter)
        Me.fraResults.Controls.Add(Me.cmdApply)
        Me.fraResults.Controls.Add(Me.TextBox2)
        Me.fraResults.Controls.Add(Me.optLandUse)
        Me.fraResults.Controls.Add(Me.optParameter)
        Me.fraResults.Controls.Add(Me.lblFile)
        Me.fraResults.Controls.Add(Me.cmdFile)
        Me.fraResults.Location = New System.Drawing.Point(12, 96)
        Me.fraResults.Margin = New System.Windows.Forms.Padding(4)
        Me.fraResults.Name = "fraResults"
        Me.fraResults.Padding = New System.Windows.Forms.Padding(4)
        Me.fraResults.Size = New System.Drawing.Size(645, 377)
        Me.fraResults.TabIndex = 1
        Me.fraResults.TabStop = False
        Me.fraResults.Text = "Assign Values From HSPFParm Report file To Starter"
        '
        'fraParameter
        '
        Me.fraParameter.BackColor = System.Drawing.SystemColors.Control
        Me.fraParameter.Controls.Add(Me.lstStarter)
        Me.fraParameter.Controls.Add(Me.lstParms)
        Me.fraParameter.Controls.Add(Me.lblStarter)
        Me.fraParameter.Controls.Add(Me.lblParm)
        Me.fraParameter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.fraParameter.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.fraParameter.Location = New System.Drawing.Point(20, 104)
        Me.fraParameter.Name = "fraParameter"
        Me.fraParameter.Size = New System.Drawing.Size(603, 212)
        Me.fraParameter.TabIndex = 14
        Me.fraParameter.TabStop = False
        '
        'lstStarter
        '
        Me.lstStarter.FormattingEnabled = True
        Me.lstStarter.ItemHeight = 16
        Me.lstStarter.Location = New System.Drawing.Point(324, 34)
        Me.lstStarter.Name = "lstStarter"
        Me.lstStarter.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstStarter.Size = New System.Drawing.Size(265, 164)
        Me.lstStarter.TabIndex = 17
        '
        'lstParms
        '
        Me.lstParms.FormattingEnabled = True
        Me.lstParms.ItemHeight = 16
        Me.lstParms.Location = New System.Drawing.Point(17, 34)
        Me.lstParms.Name = "lstParms"
        Me.lstParms.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstParms.Size = New System.Drawing.Size(265, 164)
        Me.lstParms.TabIndex = 16
        '
        'lblStarter
        '
        Me.lblStarter.AutoSize = True
        Me.lblStarter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStarter.Location = New System.Drawing.Point(321, 14)
        Me.lblStarter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStarter.Name = "lblStarter"
        Me.lblStarter.Size = New System.Drawing.Size(144, 17)
        Me.lblStarter.TabIndex = 15
        Me.lblStarter.Text = "To Starter Land Uses"
        '
        'lblParm
        '
        Me.lblParm.AutoSize = True
        Me.lblParm.BackColor = System.Drawing.SystemColors.Control
        Me.lblParm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblParm.Location = New System.Drawing.Point(13, 14)
        Me.lblParm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParm.Name = "lblParm"
        Me.lblParm.Size = New System.Drawing.Size(186, 17)
        Me.lblParm.TabIndex = 14
        Me.lblParm.Text = "Parameters from HSPFParm"
        '
        'agdStarter
        '
        Me.agdStarter.AllowHorizontalScrolling = True
        Me.agdStarter.AllowNewValidValues = False
        Me.agdStarter.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdStarter.Fixed3D = False
        Me.agdStarter.LineColor = System.Drawing.SystemColors.Control
        Me.agdStarter.LineWidth = 1.0!
        Me.agdStarter.Location = New System.Drawing.Point(41, 134)
        Me.agdStarter.Margin = New System.Windows.Forms.Padding(4)
        Me.agdStarter.Name = "agdStarter"
        Me.agdStarter.Size = New System.Drawing.Size(568, 194)
        Me.agdStarter.Source = Nothing
        Me.agdStarter.TabIndex = 3
        '
        'cmdApply
        '
        Me.cmdApply.Location = New System.Drawing.Point(41, 336)
        Me.cmdApply.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(177, 27)
        Me.cmdApply.TabIndex = 2
        Me.cmdApply.Text = "&Apply to Starter"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(349, 134)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(259, 182)
        Me.TextBox2.TabIndex = 11
        '
        'optLandUse
        '
        Me.optLandUse.AutoSize = True
        Me.optLandUse.Location = New System.Drawing.Point(349, 76)
        Me.optLandUse.Margin = New System.Windows.Forms.Padding(4)
        Me.optLandUse.Name = "optLandUse"
        Me.optLandUse.Size = New System.Drawing.Size(153, 21)
        Me.optLandUse.TabIndex = 7
        Me.optLandUse.TabStop = True
        Me.optLandUse.Text = "Assign By Land Use"
        Me.optLandUse.UseVisualStyleBackColor = True
        '
        'optParameter
        '
        Me.optParameter.AutoSize = True
        Me.optParameter.Location = New System.Drawing.Point(41, 76)
        Me.optParameter.Margin = New System.Windows.Forms.Padding(4)
        Me.optParameter.Name = "optParameter"
        Me.optParameter.Size = New System.Drawing.Size(158, 21)
        Me.optParameter.TabIndex = 6
        Me.optParameter.TabStop = True
        Me.optParameter.Text = "Assign By Parameter"
        Me.optParameter.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(147, 41)
        Me.lblFile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(56, 17)
        Me.lblFile.TabIndex = 5
        Me.lblFile.Text = "<none>"
        '
        'cmdFile
        '
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.ImageIndex = 0
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(20, 37)
        Me.cmdFile.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(104, 23)
        Me.cmdFile.TabIndex = 4
        Me.cmdFile.Text = "Open File"
        Me.cmdFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdFile.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "toolbar-openicon.png")
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(272, 486)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(125, 28)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmHspfParm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(669, 530)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.fraResults)
        Me.Controls.Add(Me.cmdStart)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmHspfParm"
        Me.Text = "WinHSPF - HSPFParm Linkage"
        Me.fraResults.ResumeLayout(False)
        Me.fraResults.PerformLayout()
        Me.fraParameter.ResumeLayout(False)
        Me.fraParameter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents fraResults As System.Windows.Forms.GroupBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents optLandUse As System.Windows.Forms.RadioButton
    Friend WithEvents optParameter As System.Windows.Forms.RadioButton
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents agdStarter As atcControls.atcGrid
    Friend WithEvents fraParameter As System.Windows.Forms.GroupBox
    Friend WithEvents lstStarter As System.Windows.Forms.ListBox
    Friend WithEvents lstParms As System.Windows.Forms.ListBox
    Friend WithEvents lblStarter As System.Windows.Forms.Label
    Friend WithEvents lblParm As System.Windows.Forms.Label
End Class
