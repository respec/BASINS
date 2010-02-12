<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRenumberOperation
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
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cboOperationType = New System.Windows.Forms.ComboBox
        Me.txtOperationNumber = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdClose1 = New System.Windows.Forms.Button
        Me.cmdOK1 = New System.Windows.Forms.Button
        Me.cboOperation = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.atxNew = New atcControls.atcText
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdClose.Location = New System.Drawing.Point(305, 179)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(136, 34)
        Me.cmdClose.TabIndex = 22
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK.Location = New System.Drawing.Point(154, 179)
        Me.cmdOK.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(136, 34)
        Me.cmdOK.TabIndex = 21
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'cboOperationType
        '
        Me.cboOperationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOperationType.FormattingEnabled = True
        Me.cboOperationType.Location = New System.Drawing.Point(38, -49)
        Me.cboOperationType.Margin = New System.Windows.Forms.Padding(4)
        Me.cboOperationType.Name = "cboOperationType"
        Me.cboOperationType.Size = New System.Drawing.Size(125, 24)
        Me.cboOperationType.TabIndex = 20
        '
        'txtOperationNumber
        '
        Me.txtOperationNumber.Location = New System.Drawing.Point(266, -49)
        Me.txtOperationNumber.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOperationNumber.Name = "txtOperationNumber"
        Me.txtOperationNumber.Size = New System.Drawing.Size(108, 22)
        Me.txtOperationNumber.TabIndex = 19
        Me.txtOperationNumber.Text = "1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(199, -44)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 17)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Number"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(-81, -44)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 17)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Operation Type"
        '
        'cmdClose1
        '
        Me.cmdClose1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdClose1.Location = New System.Drawing.Point(312, 117)
        Me.cmdClose1.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose1.Name = "cmdClose1"
        Me.cmdClose1.Size = New System.Drawing.Size(136, 34)
        Me.cmdClose1.TabIndex = 28
        Me.cmdClose1.Text = "Close"
        Me.cmdClose1.UseVisualStyleBackColor = True
        '
        'cmdOK1
        '
        Me.cmdOK1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdOK1.Location = New System.Drawing.Point(154, 117)
        Me.cmdOK1.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdOK1.Name = "cmdOK1"
        Me.cmdOK1.Size = New System.Drawing.Size(136, 34)
        Me.cmdOK1.TabIndex = 27
        Me.cmdOK1.Text = "OK"
        Me.cmdOK1.UseVisualStyleBackColor = True
        '
        'cboOperation
        '
        Me.cboOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOperation.FormattingEnabled = True
        Me.cboOperation.Location = New System.Drawing.Point(69, 59)
        Me.cboOperation.Margin = New System.Windows.Forms.Padding(4)
        Me.cboOperation.Name = "cboOperation"
        Me.cboOperation.Size = New System.Drawing.Size(221, 24)
        Me.cboOperation.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(346, 38)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 17)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "New Number"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(66, 38)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 17)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Operation to Renumber"
        '
        'atxNew
        '
        Me.atxNew.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atxNew.DefaultValue = ""
        Me.atxNew.HardMax = 999
        Me.atxNew.HardMin = 0
        Me.atxNew.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxNew.Location = New System.Drawing.Point(347, 58)
        Me.atxNew.MaxWidth = 20
        Me.atxNew.Name = "atxNew"
        Me.atxNew.NumericFormat = "0.#####"
        Me.atxNew.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxNew.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxNew.SelLength = 0
        Me.atxNew.SelStart = 0
        Me.atxNew.Size = New System.Drawing.Size(123, 25)
        Me.atxNew.SoftMax = -999
        Me.atxNew.SoftMin = -999
        Me.atxNew.TabIndex = 29
        Me.atxNew.ValueDouble = 1
        Me.atxNew.ValueInteger = 1
        '
        'frmRenumberOperation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(586, 164)
        Me.Controls.Add(Me.atxNew)
        Me.Controls.Add(Me.cmdClose1)
        Me.Controls.Add(Me.cmdOK1)
        Me.Controls.Add(Me.cboOperation)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cboOperationType)
        Me.Controls.Add(Me.txtOperationNumber)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmRenumberOperation"
        Me.Text = "Renumber Operation"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cboOperationType As System.Windows.Forms.ComboBox
    Friend WithEvents txtOperationNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdClose1 As System.Windows.Forms.Button
    Friend WithEvents cmdOK1 As System.Windows.Forms.Button
    Friend WithEvents cboOperation As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents atxNew As atcControls.atcText
End Class
