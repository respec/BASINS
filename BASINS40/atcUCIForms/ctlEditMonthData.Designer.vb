<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditMonthData
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
        Me.cboID = New System.Windows.Forms.ComboBox
        Me.lblRefCount = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.grdMonthValues = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Month Data Table Number:"
        '
        'cboID
        '
        Me.cboID.FormattingEnabled = True
        Me.cboID.Location = New System.Drawing.Point(161, 9)
        Me.cboID.Name = "cboID"
        Me.cboID.Size = New System.Drawing.Size(95, 21)
        Me.cboID.TabIndex = 1
        '
        'lblRefCount
        '
        Me.lblRefCount.AutoSize = True
        Me.lblRefCount.Location = New System.Drawing.Point(262, 13)
        Me.lblRefCount.Name = "lblRefCount"
        Me.lblRefCount.Size = New System.Drawing.Size(107, 13)
        Me.lblRefCount.TabIndex = 2
        Me.lblRefCount.Text = "is referenced 0 times."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Monthly Values"
        '
        'grdMonthValues
        '
        Me.grdMonthValues.AllowHorizontalScrolling = True
        Me.grdMonthValues.AllowNewValidValues = False
        Me.grdMonthValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdMonthValues.CellBackColor = System.Drawing.Color.Empty
        Me.grdMonthValues.Fixed3D = False
        Me.grdMonthValues.LineColor = System.Drawing.Color.Empty
        Me.grdMonthValues.LineWidth = 0.0!
        Me.grdMonthValues.Location = New System.Drawing.Point(13, 83)
        Me.grdMonthValues.Name = "grdMonthValues"
        Me.grdMonthValues.Size = New System.Drawing.Size(593, 74)
        Me.grdMonthValues.Source = Nothing
        Me.grdMonthValues.TabIndex = 4
        '
        'ctlEditMonthData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grdMonthValues)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblRefCount)
        Me.Controls.Add(Me.cboID)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ctlEditMonthData"
        Me.Size = New System.Drawing.Size(624, 172)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboID As System.Windows.Forms.ComboBox
    Friend WithEvents lblRefCount As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grdMonthValues As atcControls.atcGrid

End Class
