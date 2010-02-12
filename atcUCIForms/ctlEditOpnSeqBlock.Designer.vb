<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditOpnSeqBlock
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtIndelt = New atcControls.atcText
        Me.grdEdit = New atcControls.atcGrid
        Me.cmdRenumber = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 63)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Operations"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 17)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Indelt:"
        '
        'txtIndelt
        '
        Me.txtIndelt.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtIndelt.DefaultValue = "0"
        Me.txtIndelt.HardMax = 99999
        Me.txtIndelt.HardMin = 0
        Me.txtIndelt.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtIndelt.Location = New System.Drawing.Point(76, 17)
        Me.txtIndelt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtIndelt.MaxWidth = 0
        Me.txtIndelt.Name = "txtIndelt"
        Me.txtIndelt.NumericFormat = "0.#####"
        Me.txtIndelt.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtIndelt.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtIndelt.SelLength = 1
        Me.txtIndelt.SelStart = 0
        Me.txtIndelt.Size = New System.Drawing.Size(131, 34)
        Me.txtIndelt.SoftMax = 0
        Me.txtIndelt.SoftMin = 0
        Me.txtIndelt.TabIndex = 3
        Me.txtIndelt.ValueDouble = 0
        Me.txtIndelt.ValueInteger = 0
        '
        'grdEdit
        '
        Me.grdEdit.AllowHorizontalScrolling = True
        Me.grdEdit.AllowNewValidValues = False
        Me.grdEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdEdit.CellBackColor = System.Drawing.Color.Empty
        Me.grdEdit.Fixed3D = False
        Me.grdEdit.LineColor = System.Drawing.Color.Empty
        Me.grdEdit.LineWidth = 0.0!
        Me.grdEdit.Location = New System.Drawing.Point(35, 87)
        Me.grdEdit.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grdEdit.Name = "grdEdit"
        Me.grdEdit.Size = New System.Drawing.Size(585, 327)
        Me.grdEdit.Source = Nothing
        Me.grdEdit.TabIndex = 0
        '
        'cmdRenumber
        '
        Me.cmdRenumber.Location = New System.Drawing.Point(439, 21)
        Me.cmdRenumber.Name = "cmdRenumber"
        Me.cmdRenumber.Size = New System.Drawing.Size(171, 29)
        Me.cmdRenumber.TabIndex = 4
        Me.cmdRenumber.Text = "Renumber Operation"
        Me.cmdRenumber.UseVisualStyleBackColor = True
        '
        'ctlEditOpnSeqBlock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmdRenumber)
        Me.Controls.Add(Me.txtIndelt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grdEdit)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "ctlEditOpnSeqBlock"
        Me.Size = New System.Drawing.Size(653, 437)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdEdit As atcControls.atcGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtIndelt As atcControls.atcText
    Friend WithEvents cmdRenumber As System.Windows.Forms.Button

End Class
