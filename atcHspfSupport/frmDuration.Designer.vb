<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDuration
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
        Me.lblTSDir = New System.Windows.Forms.Label
        Me.txtTSDir = New System.Windows.Forms.TextBox
        Me.btnTSDir = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.DateChooser = New atcData.atcChooseDataGroupDates
        Me.btnDuration = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblTSDir
        '
        Me.lblTSDir.AutoSize = True
        Me.lblTSDir.Location = New System.Drawing.Point(27, 66)
        Me.lblTSDir.Name = "lblTSDir"
        Me.lblTSDir.Size = New System.Drawing.Size(62, 13)
        Me.lblTSDir.TabIndex = 0
        Me.lblTSDir.Text = "Time Series"
        '
        'txtTSDir
        '
        Me.txtTSDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTSDir.Location = New System.Drawing.Point(103, 63)
        Me.txtTSDir.Name = "txtTSDir"
        Me.txtTSDir.Size = New System.Drawing.Size(232, 20)
        Me.txtTSDir.TabIndex = 1
        '
        'btnTSDir
        '
        Me.btnTSDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTSDir.Location = New System.Drawing.Point(355, 63)
        Me.btnTSDir.Name = "btnTSDir"
        Me.btnTSDir.Size = New System.Drawing.Size(32, 23)
        Me.btnTSDir.TabIndex = 2
        Me.btnTSDir.Text = "..."
        Me.btnTSDir.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(27, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Report Title"
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.Location = New System.Drawing.Point(103, 22)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(232, 20)
        Me.txtTitle.TabIndex = 4
        '
        'DateChooser
        '
        Me.DateChooser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateChooser.CommonEnd = 1.7976931348623157E+308
        Me.DateChooser.CommonStart = -1.7976931348623157E+308
        Me.DateChooser.DataGroup = Nothing
        Me.DateChooser.FirstStart = Double.NaN
        Me.DateChooser.LastEnd = Double.NaN
        Me.DateChooser.Location = New System.Drawing.Point(30, 109)
        Me.DateChooser.Name = "DateChooser"
        Me.DateChooser.OmitAfter = 0
        Me.DateChooser.OmitBefore = 0
        Me.DateChooser.Size = New System.Drawing.Size(370, 88)
        Me.DateChooser.TabIndex = 7
        '
        'btnDuration
        '
        Me.btnDuration.Location = New System.Drawing.Point(30, 232)
        Me.btnDuration.Name = "btnDuration"
        Me.btnDuration.Size = New System.Drawing.Size(75, 23)
        Me.btnDuration.TabIndex = 8
        Me.btnDuration.Text = "Analyze"
        Me.btnDuration.UseVisualStyleBackColor = True
        '
        'frmDuration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(429, 306)
        Me.Controls.Add(Me.btnDuration)
        Me.Controls.Add(Me.DateChooser)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnTSDir)
        Me.Controls.Add(Me.txtTSDir)
        Me.Controls.Add(Me.lblTSDir)
        Me.Name = "frmDuration"
        Me.Text = "Duration Analysis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTSDir As System.Windows.Forms.Label
    Friend WithEvents txtTSDir As System.Windows.Forms.TextBox
    Friend WithEvents btnTSDir As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents DateChooser As atcData.atcChooseDataGroupDates
    Friend WithEvents btnDuration As System.Windows.Forms.Button
End Class
