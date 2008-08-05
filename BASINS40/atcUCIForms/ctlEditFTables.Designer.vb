<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditFTables
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
        Me.cmdImport = New System.Windows.Forms.Button
        Me.cmdCompute = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.cboID = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNCols = New atcControls.atcText
        Me.txtNRows = New atcControls.atcText
        Me.grdEdit = New atcControls.atcGrid
        Me.SuspendLayout()
        '
        'cmdImport
        '
        Me.cmdImport.Location = New System.Drawing.Point(793, 133)
        Me.cmdImport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.Size = New System.Drawing.Size(131, 47)
        Me.cmdImport.TabIndex = 1
        Me.cmdImport.Text = "Import From Cross Section"
        Me.cmdImport.UseVisualStyleBackColor = True
        '
        'cmdCompute
        '
        Me.cmdCompute.Location = New System.Drawing.Point(793, 185)
        Me.cmdCompute.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cmdCompute.Name = "cmdCompute"
        Me.cmdCompute.Size = New System.Drawing.Size(129, 47)
        Me.cmdCompute.TabIndex = 2
        Me.cmdCompute.Text = "Compute New"
        Me.cmdCompute.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(793, 236)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(129, 47)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "F-Curve"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cboID
        '
        Me.cboID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboID.FormattingEnabled = True
        Me.cboID.Location = New System.Drawing.Point(80, 4)
        Me.cboID.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboID.Name = "cboID"
        Me.cboID.Size = New System.Drawing.Size(404, 24)
        Me.cboID.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(757, 63)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "NRows:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(757, 98)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "NCols:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 7)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "FTable:"
        '
        'txtNCols
        '
        Me.txtNCols.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtNCols.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtNCols.DefaultValue = 4
        Me.txtNCols.HardMax = 8
        Me.txtNCols.HardMin = 4
        Me.txtNCols.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtNCols.Location = New System.Drawing.Point(825, 92)
        Me.txtNCols.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtNCols.MaxDecimal = 0
        Me.txtNCols.maxWidth = 0
        Me.txtNCols.Name = "txtNCols"
        Me.txtNCols.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtNCols.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtNCols.SelLength = 0
        Me.txtNCols.SelStart = 1
        Me.txtNCols.Size = New System.Drawing.Size(108, 28)
        Me.txtNCols.SoftMax = -999
        Me.txtNCols.SoftMin = -999
        Me.txtNCols.TabIndex = 6
        Me.txtNCols.Value = CType(4, Long)
        '
        'txtNRows
        '
        Me.txtNRows.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtNRows.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtNRows.DefaultValue = 1
        Me.txtNRows.HardMax = 25
        Me.txtNRows.HardMin = 1
        Me.txtNRows.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.txtNRows.Location = New System.Drawing.Point(825, 57)
        Me.txtNRows.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtNRows.MaxDecimal = 0
        Me.txtNRows.maxWidth = 0
        Me.txtNRows.Name = "txtNRows"
        Me.txtNRows.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.txtNRows.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.txtNRows.SelLength = 0
        Me.txtNRows.SelStart = 1
        Me.txtNRows.Size = New System.Drawing.Size(108, 28)
        Me.txtNRows.SoftMax = -999
        Me.txtNRows.SoftMin = -999
        Me.txtNRows.TabIndex = 5
        Me.txtNRows.Value = CType(1, Long)
        '
        'grdEdit
        '
        Me.grdEdit.AllowHorizontalScrolling = True
        Me.grdEdit.AllowNewValidValues = False
        Me.grdEdit.CellBackColor = System.Drawing.Color.Empty
        Me.grdEdit.LineColor = System.Drawing.Color.Empty
        Me.grdEdit.LineWidth = 0.0!
        Me.grdEdit.Location = New System.Drawing.Point(16, 57)
        Me.grdEdit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.grdEdit.Name = "grdEdit"
        Me.grdEdit.Size = New System.Drawing.Size(735, 439)
        Me.grdEdit.Source = Nothing
        Me.grdEdit.TabIndex = 0
        '
        'ctlEditFTables
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNCols)
        Me.Controls.Add(Me.txtNRows)
        Me.Controls.Add(Me.cboID)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cmdCompute)
        Me.Controls.Add(Me.cmdImport)
        Me.Controls.Add(Me.grdEdit)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "ctlEditFTables"
        Me.Size = New System.Drawing.Size(951, 498)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdEdit As atcControls.atcGrid
    Friend WithEvents cmdImport As System.Windows.Forms.Button
    Friend WithEvents cmdCompute As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cboID As System.Windows.Forms.ComboBox
    Friend WithEvents txtNRows As atcControls.atcText
    Friend WithEvents txtNCols As atcControls.atcText
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class
