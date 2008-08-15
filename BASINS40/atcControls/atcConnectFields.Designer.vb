<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcConnectFields
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
        Me.lstSource = New System.Windows.Forms.ListBox
        Me.lstTarget = New System.Windows.Forms.ListBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.lblHeader = New System.Windows.Forms.Label
        Me.lblSource = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnLoad = New System.Windows.Forms.Button
        Me.lblConnections = New System.Windows.Forms.Label
        Me.lstConnections = New System.Windows.Forms.ListBox
        Me.btnClear = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lstSource
        '
        Me.lstSource.FormattingEnabled = True
        Me.lstSource.ItemHeight = 16
        Me.lstSource.Location = New System.Drawing.Point(18, 70)
        Me.lstSource.Name = "lstSource"
        Me.lstSource.Size = New System.Drawing.Size(173, 212)
        Me.lstSource.TabIndex = 0
        '
        'lstTarget
        '
        Me.lstTarget.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstTarget.FormattingEnabled = True
        Me.lstTarget.ItemHeight = 16
        Me.lstTarget.Location = New System.Drawing.Point(213, 70)
        Me.lstTarget.Name = "lstTarget"
        Me.lstTarget.Size = New System.Drawing.Size(173, 212)
        Me.lstTarget.TabIndex = 1
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(18, 297)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(59, 29)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(83, 297)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(57, 29)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Location = New System.Drawing.Point(15, 9)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(308, 17)
        Me.lblHeader.TabIndex = 5
        Me.lblHeader.Text = "Select a Source and Target field, then click Add"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(17, 45)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(87, 17)
        Me.lblSource.TabIndex = 6
        Me.lblSource.Text = "Source Field"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(210, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Target Field"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(295, 297)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(57, 29)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(232, 297)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(57, 29)
        Me.btnLoad.TabIndex = 9
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'lblConnections
        '
        Me.lblConnections.AutoSize = True
        Me.lblConnections.Location = New System.Drawing.Point(22, 333)
        Me.lblConnections.Name = "lblConnections"
        Me.lblConnections.Size = New System.Drawing.Size(86, 17)
        Me.lblConnections.TabIndex = 10
        Me.lblConnections.Text = "Connections"
        '
        'lstConnections
        '
        Me.lstConnections.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstConnections.FormattingEnabled = True
        Me.lstConnections.ItemHeight = 16
        Me.lstConnections.Location = New System.Drawing.Point(21, 365)
        Me.lstConnections.Name = "lstConnections"
        Me.lstConnections.Size = New System.Drawing.Size(365, 148)
        Me.lstConnections.TabIndex = 11
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(146, 297)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(57, 29)
        Me.btnClear.TabIndex = 12
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'atcConnectFields
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lstConnections)
        Me.Controls.Add(Me.lblConnections)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblSource)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstTarget)
        Me.Controls.Add(Me.lstSource)
        Me.Name = "atcConnectFields"
        Me.Size = New System.Drawing.Size(406, 525)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents lblConnections As System.Windows.Forms.Label
    Public WithEvents lstSource As System.Windows.Forms.ListBox
    Public WithEvents lstTarget As System.Windows.Forms.ListBox
    Public WithEvents lstConnections As System.Windows.Forms.ListBox
    Friend WithEvents btnClear As System.Windows.Forms.Button

End Class
