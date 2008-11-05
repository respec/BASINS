<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPoint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPoint))
        Me.chkAllSources = New System.Windows.Forms.CheckBox
        Me.grpSources = New System.Windows.Forms.GroupBox
        Me.lstSources = New System.Windows.Forms.CheckedListBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.grpDetails = New System.Windows.Forms.GroupBox
        Me.AtcGrid1 = New atcControls.atcGrid
        Me.Button9 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.cmdDetailsShow = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmdDetailsHide = New System.Windows.Forms.Button
        Me.grpSources.SuspendLayout()
        Me.grpDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkAllSources
        '
        Me.chkAllSources.AutoSize = True
        Me.chkAllSources.Location = New System.Drawing.Point(15, 19)
        Me.chkAllSources.Name = "chkAllSources"
        Me.chkAllSources.Size = New System.Drawing.Size(70, 17)
        Me.chkAllSources.TabIndex = 7
        Me.chkAllSources.Text = "Select All"
        Me.chkAllSources.UseVisualStyleBackColor = True
        '
        'grpSources
        '
        Me.grpSources.Controls.Add(Me.lstSources)
        Me.grpSources.Controls.Add(Me.chkAllSources)
        Me.grpSources.Controls.Add(Me.Button2)
        Me.grpSources.Controls.Add(Me.Button1)
        Me.grpSources.Location = New System.Drawing.Point(12, 12)
        Me.grpSources.Name = "grpSources"
        Me.grpSources.Size = New System.Drawing.Size(245, 297)
        Me.grpSources.TabIndex = 14
        Me.grpSources.TabStop = False
        Me.grpSources.Text = "Point Sources"
        '
        'lstSources
        '
        Me.lstSources.FormattingEnabled = True
        Me.lstSources.Location = New System.Drawing.Point(17, 46)
        Me.lstSources.Name = "lstSources"
        Me.lstSources.Size = New System.Drawing.Size(211, 199)
        Me.lstSources.TabIndex = 17
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(126, 265)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(101, 26)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "Create Scenario"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(17, 265)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(101, 26)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = " Add New"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(137, 376)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(101, 26)
        Me.cmdCancel.TabIndex = 18
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(28, 376)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(101, 26)
        Me.cmdOK.TabIndex = 17
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.AtcGrid1)
        Me.grpDetails.Controls.Add(Me.Button9)
        Me.grpDetails.Controls.Add(Me.Button7)
        Me.grpDetails.Controls.Add(Me.Button8)
        Me.grpDetails.Location = New System.Drawing.Point(283, 12)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(478, 297)
        Me.grpDetails.TabIndex = 19
        Me.grpDetails.TabStop = False
        Me.grpDetails.Text = "Details of <    >"
        '
        'AtcGrid1
        '
        Me.AtcGrid1.AllowHorizontalScrolling = True
        Me.AtcGrid1.AllowNewValidValues = False
        Me.AtcGrid1.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGrid1.Fixed3D = False
        Me.AtcGrid1.LineColor = System.Drawing.Color.Empty
        Me.AtcGrid1.LineWidth = 0.0!
        Me.AtcGrid1.Location = New System.Drawing.Point(57, 17)
        Me.AtcGrid1.Name = "AtcGrid1"
        Me.AtcGrid1.Size = New System.Drawing.Size(406, 267)
        Me.AtcGrid1.Source = Nothing
        Me.AtcGrid1.TabIndex = 25
        '
        'Button9
        '
        Me.Button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button9.Image = CType(resources.GetObject("Button9.Image"), System.Drawing.Image)
        Me.Button9.Location = New System.Drawing.Point(6, 112)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(40, 35)
        Me.Button9.TabIndex = 24
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button7.Image = CType(resources.GetObject("Button7.Image"), System.Drawing.Image)
        Me.Button7.Location = New System.Drawing.Point(6, 71)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(40, 35)
        Me.Button7.TabIndex = 23
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.BackgroundImage = CType(resources.GetObject("Button8.BackgroundImage"), System.Drawing.Image)
        Me.Button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button8.Location = New System.Drawing.Point(6, 30)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(40, 35)
        Me.Button8.TabIndex = 22
        Me.Button8.UseVisualStyleBackColor = True
        '
        'cmdDetailsShow
        '
        Me.cmdDetailsShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdDetailsShow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsShow.ImageIndex = 1
        Me.cmdDetailsShow.ImageList = Me.ImageList1
        Me.cmdDetailsShow.Location = New System.Drawing.Point(157, 329)
        Me.cmdDetailsShow.Name = "cmdDetailsShow"
        Me.cmdDetailsShow.Size = New System.Drawing.Size(100, 21)
        Me.cmdDetailsShow.TabIndex = 17
        Me.cmdDetailsShow.Text = "Show Details"
        Me.cmdDetailsShow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDetailsShow.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "l.png")
        Me.ImageList1.Images.SetKeyName(1, "r.png")
        '
        'cmdDetailsHide
        '
        Me.cmdDetailsHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdDetailsHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDetailsHide.ImageIndex = 0
        Me.cmdDetailsHide.ImageList = Me.ImageList1
        Me.cmdDetailsHide.Location = New System.Drawing.Point(283, 329)
        Me.cmdDetailsHide.Name = "cmdDetailsHide"
        Me.cmdDetailsHide.Size = New System.Drawing.Size(100, 21)
        Me.cmdDetailsHide.TabIndex = 20
        Me.cmdDetailsHide.Text = "Hide Details"
        Me.cmdDetailsHide.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdDetailsHide.UseVisualStyleBackColor = True
        '
        'frmPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 423)
        Me.Controls.Add(Me.cmdDetailsHide)
        Me.Controls.Add(Me.cmdDetailsShow)
        Me.Controls.Add(Me.grpDetails)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.grpSources)
        Me.Name = "frmPoint"
        Me.Text = "WinHSPF - Point Sources"
        Me.grpSources.ResumeLayout(False)
        Me.grpSources.PerformLayout()
        Me.grpDetails.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkAllSources As System.Windows.Forms.CheckBox
    Friend WithEvents grpSources As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents AtcGrid1 As atcControls.atcGrid
    Friend WithEvents cmdDetailsHide As System.Windows.Forms.Button
    Friend WithEvents cmdDetailsShow As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lstSources As System.Windows.Forms.CheckedListBox
End Class
