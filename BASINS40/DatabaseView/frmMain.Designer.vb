<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOpen = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem
        Me.atcGrid = New atcControls.atcGrid
        Me.lblTable = New System.Windows.Forms.Label
        Me.lstTableName = New System.Windows.Forms.ListBox
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(580, 26)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOpen, Me.mnuSave, Me.mnuFileSep1, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(40, 22)
        Me.mnuFile.Text = "File"
        '
        'mnuOpen
        '
        Me.mnuOpen.Name = "mnuOpen"
        Me.mnuOpen.Size = New System.Drawing.Size(152, 22)
        Me.mnuOpen.Text = "Open"
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(152, 22)
        Me.mnuSave.Text = "Save"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Name = "mnuFileSep1"
        Me.mnuFileSep1.Size = New System.Drawing.Size(149, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(152, 22)
        Me.mnuExit.Text = "Exit"
        '
        'atcGrid
        '
        Me.atcGrid.AllowHorizontalScrolling = True
        Me.atcGrid.AllowNewValidValues = False
        Me.atcGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGrid.AutoSize = True
        Me.atcGrid.CellBackColor = System.Drawing.Color.Empty
        Me.atcGrid.LineColor = System.Drawing.Color.Empty
        Me.atcGrid.LineWidth = 0.0!
        Me.atcGrid.Location = New System.Drawing.Point(0, 93)
        Me.atcGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.atcGrid.Name = "atcGrid"
        Me.atcGrid.Size = New System.Drawing.Size(580, 248)
        Me.atcGrid.Source = Nothing
        Me.atcGrid.TabIndex = 0
        '
        'lblTable
        '
        Me.lblTable.Location = New System.Drawing.Point(12, 33)
        Me.lblTable.Name = "lblTable"
        Me.lblTable.Size = New System.Drawing.Size(66, 17)
        Me.lblTable.TabIndex = 4
        Me.lblTable.Text = "Table:"
        Me.lblTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lstTableName
        '
        Me.lstTableName.FormattingEnabled = True
        Me.lstTableName.ItemHeight = 16
        Me.lstTableName.Location = New System.Drawing.Point(84, 34)
        Me.lstTableName.Name = "lstTableName"
        Me.lstTableName.Size = New System.Drawing.Size(237, 52)
        Me.lstTableName.TabIndex = 6
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(580, 336)
        Me.Controls.Add(Me.lstTableName)
        Me.Controls.Add(Me.lblTable)
        Me.Controls.Add(Me.atcGrid)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmMain"
        Me.Text = "Database Viewer"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents atcGrid As atcControls.atcGrid
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOpen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTable As System.Windows.Forms.Label
    Friend WithEvents lstTableName As System.Windows.Forms.ListBox

End Class
