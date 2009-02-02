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
        Me.Button1 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.agdStarter = New atcControls.atcGrid
        Me.cmdApply = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.radioLandUse = New System.Windows.Forms.RadioButton
        Me.radioParameter = New System.Windows.Forms.RadioButton
        Me.lblFile = New System.Windows.Forms.Label
        Me.cmdFile = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Button3 = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(185, 25)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(133, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Start &HSPFParm"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.agdStarter)
        Me.GroupBox1.Controls.Add(Me.cmdApply)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.radioLandUse)
        Me.GroupBox1.Controls.Add(Me.radioParameter)
        Me.GroupBox1.Controls.Add(Me.lblFile)
        Me.GroupBox1.Controls.Add(Me.cmdFile)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 78)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(484, 306)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Assign Values From HSPFParm Report file To Starter"
        '
        'agdStarter
        '
        Me.agdStarter.AllowHorizontalScrolling = True
        Me.agdStarter.AllowNewValidValues = False
        Me.agdStarter.CellBackColor = System.Drawing.SystemColors.Window
        Me.agdStarter.Fixed3D = False
        Me.agdStarter.LineColor = System.Drawing.SystemColors.Control
        Me.agdStarter.LineWidth = 1.0!
        Me.agdStarter.Location = New System.Drawing.Point(31, 109)
        Me.agdStarter.Name = "agdStarter"
        Me.agdStarter.Size = New System.Drawing.Size(426, 158)
        Me.agdStarter.Source = Nothing
        Me.agdStarter.TabIndex = 3
        '
        'cmdApply
        '
        Me.cmdApply.Location = New System.Drawing.Point(31, 273)
        Me.cmdApply.Name = "cmdApply"
        Me.cmdApply.Size = New System.Drawing.Size(133, 22)
        Me.cmdApply.TabIndex = 2
        Me.cmdApply.Text = "&Apply to Starter"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(262, 109)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(195, 149)
        Me.TextBox2.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(259, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(108, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "To Starter Land Uses"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(31, 109)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(195, 149)
        Me.TextBox1.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Parameters from HSPFParm"
        '
        'radioLandUse
        '
        Me.radioLandUse.AutoSize = True
        Me.radioLandUse.Location = New System.Drawing.Point(262, 62)
        Me.radioLandUse.Name = "radioLandUse"
        Me.radioLandUse.Size = New System.Drawing.Size(120, 17)
        Me.radioLandUse.TabIndex = 7
        Me.radioLandUse.TabStop = True
        Me.radioLandUse.Text = "Assign By Land Use"
        Me.radioLandUse.UseVisualStyleBackColor = True
        '
        'radioParameter
        '
        Me.radioParameter.AutoSize = True
        Me.radioParameter.Location = New System.Drawing.Point(31, 62)
        Me.radioParameter.Name = "radioParameter"
        Me.radioParameter.Size = New System.Drawing.Size(122, 17)
        Me.radioParameter.TabIndex = 6
        Me.radioParameter.TabStop = True
        Me.radioParameter.Text = "Assign By Parameter"
        Me.radioParameter.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(110, 33)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(43, 13)
        Me.lblFile.TabIndex = 5
        Me.lblFile.Text = "<none>"
        '
        'cmdFile
        '
        Me.cmdFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdFile.ImageIndex = 0
        Me.cmdFile.ImageList = Me.ImageList1
        Me.cmdFile.Location = New System.Drawing.Point(15, 30)
        Me.cmdFile.Name = "cmdFile"
        Me.cmdFile.Size = New System.Drawing.Size(78, 19)
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
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(204, 395)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(94, 23)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "&Close"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmHspfParm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(502, 431)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "frmHspfParm"
        Me.Text = "WinHSPF - HSPFParm Linkage"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents cmdFile As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents radioLandUse As System.Windows.Forms.RadioButton
    Friend WithEvents radioParameter As System.Windows.Forms.RadioButton
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents agdStarter As atcControls.atcGrid
End Class
