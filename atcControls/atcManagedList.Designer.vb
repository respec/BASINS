<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcManagedList
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.btnDefault = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.lstValues = New System.Windows.Forms.ListBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.txtAdd = New System.Windows.Forms.TextBox
        Me.btnNone = New System.Windows.Forms.Button
        Me.btnAll = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnDefault
        '
        Me.btnDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDefault.Location = New System.Drawing.Point(146, 419)
        Me.btnDefault.Name = "btnDefault"
        Me.btnDefault.Size = New System.Drawing.Size(56, 20)
        Me.btnDefault.TabIndex = 35
        Me.btnDefault.Text = "Default"
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Location = New System.Drawing.Point(113, 419)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(27, 20)
        Me.btnRemove.TabIndex = 34
        Me.btnRemove.Text = "-"
        '
        'lstValues
        '
        Me.lstValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstValues.IntegralHeight = False
        Me.lstValues.Location = New System.Drawing.Point(0, 0)
        Me.lstValues.Name = "lstValues"
        Me.lstValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstValues.Size = New System.Drawing.Size(202, 413)
        Me.lstValues.TabIndex = 29
        Me.lstValues.Tag = ""
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Location = New System.Drawing.Point(80, 419)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(27, 20)
        Me.btnAdd.TabIndex = 31
        Me.btnAdd.Text = "+"
        '
        'txtAdd
        '
        Me.txtAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd.Location = New System.Drawing.Point(0, 419)
        Me.txtAdd.Name = "txtAdd"
        Me.txtAdd.Size = New System.Drawing.Size(74, 20)
        Me.txtAdd.TabIndex = 30
        '
        'btnNone
        '
        Me.btnNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNone.Location = New System.Drawing.Point(138, 448)
        Me.btnNone.Name = "btnNone"
        Me.btnNone.Size = New System.Drawing.Size(64, 24)
        Me.btnNone.TabIndex = 33
        Me.btnNone.Text = "None"
        '
        'btnAll
        '
        Me.btnAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAll.Location = New System.Drawing.Point(0, 448)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(64, 24)
        Me.btnAll.TabIndex = 32
        Me.btnAll.Text = "All"
        '
        'atcManagedList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnDefault)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.lstValues)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.txtAdd)
        Me.Controls.Add(Me.btnNone)
        Me.Controls.Add(Me.btnAll)
        Me.Name = "atcManagedList"
        Me.Size = New System.Drawing.Size(202, 472)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnDefault As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents lstValues As System.Windows.Forms.ListBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents txtAdd As System.Windows.Forms.TextBox
    Friend WithEvents btnNone As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button

End Class
