<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditTable
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
        Me.txtDefine = New System.Windows.Forms.TextBox
        Me.chkDesc = New System.Windows.Forms.CheckBox
        Me.cboOccur = New System.Windows.Forms.ComboBox
        Me.lblOccur = New System.Windows.Forms.Label
        Me.grdTable = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'txtDefine
        '
        Me.txtDefine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDefine.BackColor = System.Drawing.SystemColors.Control
        Me.txtDefine.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefine.Location = New System.Drawing.Point(9, 278)
        Me.txtDefine.Multiline = True
        Me.txtDefine.Name = "txtDefine"
        Me.txtDefine.Size = New System.Drawing.Size(644, 111)
        Me.txtDefine.TabIndex = 0
        '
        'chkDesc
        '
        Me.chkDesc.AutoSize = True
        Me.chkDesc.Location = New System.Drawing.Point(13, 10)
        Me.chkDesc.Name = "chkDesc"
        Me.chkDesc.Size = New System.Drawing.Size(107, 17)
        Me.chkDesc.TabIndex = 2
        Me.chkDesc.Text = "Show description"
        Me.chkDesc.UseVisualStyleBackColor = True
        '
        'cboOccur
        '
        Me.cboOccur.FormattingEnabled = True
        Me.cboOccur.Location = New System.Drawing.Point(213, 8)
        Me.cboOccur.Name = "cboOccur"
        Me.cboOccur.Size = New System.Drawing.Size(215, 21)
        Me.cboOccur.TabIndex = 3
        '
        'lblOccur
        '
        Me.lblOccur.AutoSize = True
        Me.lblOccur.Location = New System.Drawing.Point(158, 12)
        Me.lblOccur.Name = "lblOccur"
        Me.lblOccur.Size = New System.Drawing.Size(39, 13)
        Me.lblOccur.TabIndex = 4
        Me.lblOccur.Text = "Label1"
        '
        'grdTable
        '
        Me.grdTable.AllowHorizontalScrolling = True
        Me.grdTable.AllowNewValidValues = False
        Me.grdTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdTable.CellBackColor = System.Drawing.Color.Empty
        Me.grdTable.Fixed3D = True
        Me.grdTable.LineColor = System.Drawing.Color.Empty
        Me.grdTable.LineWidth = 0.0!
        Me.grdTable.Location = New System.Drawing.Point(12, 60)
        Me.grdTable.Name = "grdTable"
        Me.grdTable.Size = New System.Drawing.Size(638, 198)
        Me.grdTable.Source = Nothing
        Me.grdTable.TabIndex = 5
        '
        'ctlEditTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grdTable)
        Me.Controls.Add(Me.lblOccur)
        Me.Controls.Add(Me.cboOccur)
        Me.Controls.Add(Me.chkDesc)
        Me.Controls.Add(Me.txtDefine)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ctlEditTable"
        Me.Size = New System.Drawing.Size(662, 395)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDefine As System.Windows.Forms.TextBox
    Friend WithEvents chkDesc As System.Windows.Forms.CheckBox
    Friend WithEvents cboOccur As System.Windows.Forms.ComboBox
    Friend WithEvents lblOccur As System.Windows.Forms.Label
    Friend WithEvents grdTable As atcControls.atcGrid

End Class
