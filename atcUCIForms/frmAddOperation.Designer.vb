<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddOperation
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtOperationNumber = New System.Windows.Forms.TextBox
        Me.txtOperationDescription = New System.Windows.Forms.TextBox
        Me.lstUp = New System.Windows.Forms.ListBox
        Me.lstDown = New System.Windows.Forms.ListBox
        Me.cboOperationType = New System.Windows.Forms.ComboBox
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 27)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Operation Type"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(293, 27)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Number"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(40, 65)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Description"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(21, 121)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(143, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Upstream Operations"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(253, 121)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(160, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Downstream Operations"
        '
        'txtOperationNumber
        '
        Me.txtOperationNumber.Location = New System.Drawing.Point(360, 22)
        Me.txtOperationNumber.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOperationNumber.Name = "txtOperationNumber"
        Me.txtOperationNumber.Size = New System.Drawing.Size(108, 22)
        Me.txtOperationNumber.TabIndex = 5
        Me.txtOperationNumber.Text = "1"
        '
        'txtOperationDescription
        '
        Me.txtOperationDescription.Location = New System.Drawing.Point(132, 62)
        Me.txtOperationDescription.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOperationDescription.Name = "txtOperationDescription"
        Me.txtOperationDescription.Size = New System.Drawing.Size(336, 22)
        Me.txtOperationDescription.TabIndex = 6
        '
        'lstUp
        '
        Me.lstUp.FormattingEnabled = True
        Me.lstUp.ItemHeight = 16
        Me.lstUp.Location = New System.Drawing.Point(25, 155)
        Me.lstUp.Margin = New System.Windows.Forms.Padding(4)
        Me.lstUp.Name = "lstUp"
        Me.lstUp.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstUp.Size = New System.Drawing.Size(209, 180)
        Me.lstUp.TabIndex = 7
        '
        'lstDown
        '
        Me.lstDown.FormattingEnabled = True
        Me.lstDown.ItemHeight = 16
        Me.lstDown.Location = New System.Drawing.Point(257, 155)
        Me.lstDown.Margin = New System.Windows.Forms.Padding(4)
        Me.lstDown.Name = "lstDown"
        Me.lstDown.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstDown.Size = New System.Drawing.Size(209, 180)
        Me.lstDown.TabIndex = 8
        '
        'cboOperationType
        '
        Me.cboOperationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOperationType.FormattingEnabled = True
        Me.cboOperationType.Location = New System.Drawing.Point(132, 22)
        Me.cboOperationType.Margin = New System.Windows.Forms.Padding(4)
        Me.cboOperationType.Name = "cboOperationType"
        Me.cboOperationType.Size = New System.Drawing.Size(125, 24)
        Me.cboOperationType.TabIndex = 9
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdClose.Location = New System.Drawing.Point(252, 354)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(136, 34)
        Me.cmdClose.TabIndex = 16
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(101, 354)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(136, 34)
        Me.cmdOK.TabIndex = 15
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'frmAddOperation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 404)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboOperationType)
        Me.Controls.Add(Me.lstDown)
        Me.Controls.Add(Me.lstUp)
        Me.Controls.Add(Me.txtOperationDescription)
        Me.Controls.Add(Me.txtOperationNumber)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAddOperation"
        Me.Text = "Add Operation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtOperationNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtOperationDescription As System.Windows.Forms.TextBox
    Friend WithEvents lstUp As System.Windows.Forms.ListBox
    Friend WithEvents lstDown As System.Windows.Forms.ListBox
    Friend WithEvents cboOperationType As System.Windows.Forms.ComboBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
