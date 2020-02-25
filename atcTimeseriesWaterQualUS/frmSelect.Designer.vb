<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelect
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
        Me.lstLocations = New System.Windows.Forms.CheckedListBox()
        Me.lstConstituents = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.spaneMain = New System.Windows.Forms.SplitContainer()
        Me.btnSelectAllCons = New System.Windows.Forms.Button()
        Me.btnClearCons = New System.Windows.Forms.Button()
        Me.btnSelectAllLoc = New System.Windows.Forms.Button()
        Me.btnClearLoc = New System.Windows.Forms.Button()
        Me.txtMsgCons = New System.Windows.Forms.TextBox()
        Me.btnDoTser = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cbxSave = New System.Windows.Forms.CheckBox()
        Me.spaneMain.Panel1.SuspendLayout()
        Me.spaneMain.Panel2.SuspendLayout()
        Me.spaneMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstLocations
        '
        Me.lstLocations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstLocations.FormattingEnabled = True
        Me.lstLocations.Location = New System.Drawing.Point(6, 20)
        Me.lstLocations.Name = "lstLocations"
        Me.lstLocations.Size = New System.Drawing.Size(313, 259)
        Me.lstLocations.TabIndex = 0
        '
        'lstConstituents
        '
        Me.lstConstituents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstConstituents.FormattingEnabled = True
        Me.lstConstituents.Location = New System.Drawing.Point(3, 19)
        Me.lstConstituents.Name = "lstConstituents"
        Me.lstConstituents.Size = New System.Drawing.Size(317, 259)
        Me.lstConstituents.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Locations"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Constituents"
        '
        'spaneMain
        '
        Me.spaneMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.spaneMain.Location = New System.Drawing.Point(12, 12)
        Me.spaneMain.Name = "spaneMain"
        '
        'spaneMain.Panel1
        '
        Me.spaneMain.Panel1.Controls.Add(Me.btnSelectAllCons)
        Me.spaneMain.Panel1.Controls.Add(Me.lstConstituents)
        Me.spaneMain.Panel1.Controls.Add(Me.Label2)
        Me.spaneMain.Panel1.Controls.Add(Me.btnClearCons)
        '
        'spaneMain.Panel2
        '
        Me.spaneMain.Panel2.Controls.Add(Me.btnSelectAllLoc)
        Me.spaneMain.Panel2.Controls.Add(Me.btnClearLoc)
        Me.spaneMain.Panel2.Controls.Add(Me.txtMsgCons)
        Me.spaneMain.Panel2.Controls.Add(Me.Label1)
        Me.spaneMain.Panel2.Controls.Add(Me.lstLocations)
        Me.spaneMain.Size = New System.Drawing.Size(658, 398)
        Me.spaneMain.SplitterDistance = 325
        Me.spaneMain.TabIndex = 4
        '
        'btnSelectAllCons
        '
        Me.btnSelectAllCons.Location = New System.Drawing.Point(6, 311)
        Me.btnSelectAllCons.Name = "btnSelectAllCons"
        Me.btnSelectAllCons.Size = New System.Drawing.Size(50, 43)
        Me.btnSelectAllCons.TabIndex = 5
        Me.btnSelectAllCons.Text = "Select All"
        Me.btnSelectAllCons.UseVisualStyleBackColor = True
        '
        'btnClearCons
        '
        Me.btnClearCons.Location = New System.Drawing.Point(6, 282)
        Me.btnClearCons.Name = "btnClearCons"
        Me.btnClearCons.Size = New System.Drawing.Size(50, 23)
        Me.btnClearCons.TabIndex = 4
        Me.btnClearCons.Text = "Clear"
        Me.btnClearCons.UseVisualStyleBackColor = True
        '
        'btnSelectAllLoc
        '
        Me.btnSelectAllLoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectAllLoc.Location = New System.Drawing.Point(6, 314)
        Me.btnSelectAllLoc.Name = "btnSelectAllLoc"
        Me.btnSelectAllLoc.Size = New System.Drawing.Size(50, 40)
        Me.btnSelectAllLoc.TabIndex = 6
        Me.btnSelectAllLoc.Text = "Select All"
        Me.btnSelectAllLoc.UseVisualStyleBackColor = True
        '
        'btnClearLoc
        '
        Me.btnClearLoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearLoc.Location = New System.Drawing.Point(6, 285)
        Me.btnClearLoc.Name = "btnClearLoc"
        Me.btnClearLoc.Size = New System.Drawing.Size(50, 23)
        Me.btnClearLoc.TabIndex = 3
        Me.btnClearLoc.Text = "Clear"
        Me.btnClearLoc.UseVisualStyleBackColor = True
        '
        'txtMsgCons
        '
        Me.txtMsgCons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMsgCons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMsgCons.Location = New System.Drawing.Point(59, 285)
        Me.txtMsgCons.Multiline = True
        Me.txtMsgCons.Name = "txtMsgCons"
        Me.txtMsgCons.ReadOnly = True
        Me.txtMsgCons.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMsgCons.Size = New System.Drawing.Size(264, 110)
        Me.txtMsgCons.TabIndex = 5
        '
        'btnDoTser
        '
        Me.btnDoTser.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDoTser.Location = New System.Drawing.Point(491, 416)
        Me.btnDoTser.Name = "btnDoTser"
        Me.btnDoTser.Size = New System.Drawing.Size(98, 23)
        Me.btnDoTser.TabIndex = 5
        Me.btnDoTser.Text = "Make Timeseries"
        Me.btnDoTser.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(595, 416)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cbxSave
        '
        Me.cbxSave.AutoSize = True
        Me.cbxSave.Location = New System.Drawing.Point(357, 420)
        Me.cbxSave.Name = "cbxSave"
        Me.cbxSave.Size = New System.Drawing.Size(104, 19)
        Me.cbxSave.TabIndex = 8
        Me.cbxSave.Text = "Save Selected"
        Me.cbxSave.UseVisualStyleBackColor = True
        '
        'frmSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(682, 451)
        Me.Controls.Add(Me.cbxSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDoTser)
        Me.Controls.Add(Me.spaneMain)
        Me.Name = "frmSelect"
        Me.Text = "Select Water Quality Data"
        Me.spaneMain.Panel1.ResumeLayout(False)
        Me.spaneMain.Panel1.PerformLayout()
        Me.spaneMain.Panel2.ResumeLayout(False)
        Me.spaneMain.Panel2.PerformLayout()
        Me.spaneMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstConstituents As Windows.Forms.CheckedListBox
    Friend WithEvents lstLocations As Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents spaneMain As Windows.Forms.SplitContainer
    Friend WithEvents btnClearLoc As Windows.Forms.Button
    Friend WithEvents btnClearCons As Windows.Forms.Button
    Friend WithEvents btnDoTser As Windows.Forms.Button
    Friend WithEvents btnCancel As Windows.Forms.Button
    Friend WithEvents txtMsgCons As Windows.Forms.TextBox
    Friend WithEvents btnSelectAllCons As Windows.Forms.Button
    Friend WithEvents btnSelectAllLoc As Windows.Forms.Button
    Friend WithEvents cbxSave As Windows.Forms.CheckBox
End Class
