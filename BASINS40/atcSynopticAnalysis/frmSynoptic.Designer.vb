<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSynoptic
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSynoptic))
        Me.lblGroupBy = New System.Windows.Forms.Label
        Me.cboGroupBy = New System.Windows.Forms.ComboBox
        Me.agdMain = New atcControls.atcGrid
        Me.txtThreshold = New System.Windows.Forms.TextBox
        Me.lblThreshold = New System.Windows.Forms.Label
        Me.radioAbove = New System.Windows.Forms.RadioButton
        Me.radioBelow = New System.Windows.Forms.RadioButton
        Me.cboGapUnits = New System.Windows.Forms.ComboBox
        Me.lblGap = New System.Windows.Forms.Label
        Me.txtGap = New System.Windows.Forms.TextBox
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectData = New System.Windows.Forms.MenuItem
        Me.mnuFileSelectAttributes = New System.Windows.Forms.MenuItem
        Me.mnuFileSep1 = New System.Windows.Forms.MenuItem
        Me.mnuFileSave = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveAll = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.mnuEditCopy = New System.Windows.Forms.MenuItem
        Me.mnuView = New System.Windows.Forms.MenuItem
        Me.mnuAttributeRows = New System.Windows.Forms.MenuItem
        Me.mnuAttributeColumns = New System.Windows.Forms.MenuItem
        Me.mnuViewSep1 = New System.Windows.Forms.MenuItem
        Me.mnuSizeColumnsToContents = New System.Windows.Forms.MenuItem
        Me.mnuChooseColumns = New System.Windows.Forms.MenuItem
        Me.mnuAnalysis = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.lblDuringEvent = New System.Windows.Forms.Label
        Me.btnComputeEvents = New System.Windows.Forms.Button
        Me.mnuGraph = New System.Windows.Forms.MenuItem
        Me.SuspendLayout()
        '
        'lblGroupBy
        '
        Me.lblGroupBy.AutoSize = True
        Me.lblGroupBy.Location = New System.Drawing.Point(12, 68)
        Me.lblGroupBy.Name = "lblGroupBy"
        Me.lblGroupBy.Size = New System.Drawing.Size(51, 13)
        Me.lblGroupBy.TabIndex = 0
        Me.lblGroupBy.Text = "Group By"
        '
        'cboGroupBy
        '
        Me.cboGroupBy.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboGroupBy.FormattingEnabled = True
        Me.cboGroupBy.Location = New System.Drawing.Point(117, 65)
        Me.cboGroupBy.Name = "cboGroupBy"
        Me.cboGroupBy.Size = New System.Drawing.Size(405, 21)
        Me.cboGroupBy.TabIndex = 12
        '
        'agdMain
        '
        Me.agdMain.AllowHorizontalScrolling = True
        Me.agdMain.AllowNewValidValues = False
        Me.agdMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdMain.CellBackColor = System.Drawing.Color.Empty
        Me.agdMain.LineColor = System.Drawing.Color.Empty
        Me.agdMain.LineWidth = 0.0!
        Me.agdMain.Location = New System.Drawing.Point(12, 92)
        Me.agdMain.Name = "agdMain"
        Me.agdMain.Size = New System.Drawing.Size(513, 307)
        Me.agdMain.Source = Nothing
        Me.agdMain.TabIndex = 13
        '
        'txtThreshold
        '
        Me.txtThreshold.Location = New System.Drawing.Point(117, 12)
        Me.txtThreshold.Name = "txtThreshold"
        Me.txtThreshold.Size = New System.Drawing.Size(82, 20)
        Me.txtThreshold.TabIndex = 6
        Me.txtThreshold.Text = "0"
        '
        'lblThreshold
        '
        Me.lblThreshold.AutoSize = True
        Me.lblThreshold.Location = New System.Drawing.Point(12, 15)
        Me.lblThreshold.Name = "lblThreshold"
        Me.lblThreshold.Size = New System.Drawing.Size(85, 13)
        Me.lblThreshold.TabIndex = 5
        Me.lblThreshold.Text = "Event Threshold"
        '
        'radioAbove
        '
        Me.radioAbove.AutoSize = True
        Me.radioAbove.Checked = True
        Me.radioAbove.Location = New System.Drawing.Point(205, 13)
        Me.radioAbove.Name = "radioAbove"
        Me.radioAbove.Size = New System.Drawing.Size(160, 17)
        Me.radioAbove.TabIndex = 7
        Me.radioAbove.TabStop = True
        Me.radioAbove.Text = "Events are Above Threshold"
        Me.radioAbove.UseVisualStyleBackColor = True
        '
        'radioBelow
        '
        Me.radioBelow.AutoSize = True
        Me.radioBelow.Location = New System.Drawing.Point(371, 13)
        Me.radioBelow.Name = "radioBelow"
        Me.radioBelow.Size = New System.Drawing.Size(158, 17)
        Me.radioBelow.TabIndex = 8
        Me.radioBelow.Text = "Events are Below Threshold"
        Me.radioBelow.UseVisualStyleBackColor = True
        '
        'cboGapUnits
        '
        Me.cboGapUnits.FormattingEnabled = True
        Me.cboGapUnits.Location = New System.Drawing.Point(205, 39)
        Me.cboGapUnits.Name = "cboGapUnits"
        Me.cboGapUnits.Size = New System.Drawing.Size(108, 21)
        Me.cboGapUnits.TabIndex = 11
        '
        'lblGap
        '
        Me.lblGap.AutoSize = True
        Me.lblGap.Location = New System.Drawing.Point(12, 42)
        Me.lblGap.Name = "lblGap"
        Me.lblGap.Size = New System.Drawing.Size(99, 13)
        Me.lblGap.TabIndex = 9
        Me.lblGap.Text = "Allow Gaps of up to"
        '
        'txtGap
        '
        Me.txtGap.Location = New System.Drawing.Point(117, 39)
        Me.txtGap.Name = "txtGap"
        Me.txtGap.Size = New System.Drawing.Size(82, 20)
        Me.txtGap.TabIndex = 10
        Me.txtGap.Text = "0"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuView, Me.mnuAnalysis, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSelectData, Me.mnuFileSelectAttributes, Me.mnuFileSep1, Me.mnuFileSave, Me.mnuFileSaveAll})
        Me.mnuFile.Text = "File"
        '
        'mnuFileSelectData
        '
        Me.mnuFileSelectData.Index = 0
        Me.mnuFileSelectData.Text = "Select &Data"
        '
        'mnuFileSelectAttributes
        '
        Me.mnuFileSelectAttributes.Index = 1
        Me.mnuFileSelectAttributes.Text = "Select &Attributes"
        '
        'mnuFileSep1
        '
        Me.mnuFileSep1.Index = 2
        Me.mnuFileSep1.Text = "-"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Index = 3
        Me.mnuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSave.Text = "Save"
        '
        'mnuFileSaveAll
        '
        Me.mnuFileSaveAll.Index = 4
        Me.mnuFileSaveAll.Text = "Save All Groupings"
        '
        'mnuEdit
        '
        Me.mnuEdit.Index = 1
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuEditCopy})
        Me.mnuEdit.Text = "Edit"
        '
        'mnuEditCopy
        '
        Me.mnuEditCopy.Index = 0
        Me.mnuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC
        Me.mnuEditCopy.Text = "Copy"
        '
        'mnuView
        '
        Me.mnuView.Index = 2
        Me.mnuView.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAttributeRows, Me.mnuAttributeColumns, Me.mnuViewSep1, Me.mnuSizeColumnsToContents, Me.mnuChooseColumns, Me.mnuGraph})
        Me.mnuView.Text = "View"
        '
        'mnuAttributeRows
        '
        Me.mnuAttributeRows.Checked = True
        Me.mnuAttributeRows.Index = 0
        Me.mnuAttributeRows.Text = "Attribute Rows"
        '
        'mnuAttributeColumns
        '
        Me.mnuAttributeColumns.Index = 1
        Me.mnuAttributeColumns.Text = "Attribute Columns"
        '
        'mnuViewSep1
        '
        Me.mnuViewSep1.Index = 2
        Me.mnuViewSep1.Text = "-"
        '
        'mnuSizeColumnsToContents
        '
        Me.mnuSizeColumnsToContents.Index = 3
        Me.mnuSizeColumnsToContents.Text = "Size Columns To Contents"
        '
        'mnuChooseColumns
        '
        Me.mnuChooseColumns.Index = 4
        Me.mnuChooseColumns.Text = "Choose Columns"
        '
        'mnuAnalysis
        '
        Me.mnuAnalysis.Index = 3
        Me.mnuAnalysis.Text = "Analysis"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 4
        Me.mnuHelp.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.mnuHelp.Text = "Help"
        '
        'lblDuringEvent
        '
        Me.lblDuringEvent.AutoSize = True
        Me.lblDuringEvent.Location = New System.Drawing.Point(319, 42)
        Me.lblDuringEvent.Name = "lblDuringEvent"
        Me.lblDuringEvent.Size = New System.Drawing.Size(81, 13)
        Me.lblDuringEvent.TabIndex = 14
        Me.lblDuringEvent.Text = "during an event"
        '
        'btnComputeEvents
        '
        Me.btnComputeEvents.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnComputeEvents.Location = New System.Drawing.Point(422, 37)
        Me.btnComputeEvents.Name = "btnComputeEvents"
        Me.btnComputeEvents.Size = New System.Drawing.Size(100, 23)
        Me.btnComputeEvents.TabIndex = 15
        Me.btnComputeEvents.Text = "Compute Events"
        Me.btnComputeEvents.UseVisualStyleBackColor = True
        '
        'mnuGraph
        '
        Me.mnuGraph.Index = 5
        Me.mnuGraph.Text = "Graph Synoptic Results"
        '
        'frmSynoptic
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(537, 411)
        Me.Controls.Add(Me.btnComputeEvents)
        Me.Controls.Add(Me.lblDuringEvent)
        Me.Controls.Add(Me.txtGap)
        Me.Controls.Add(Me.cboGapUnits)
        Me.Controls.Add(Me.lblGap)
        Me.Controls.Add(Me.radioBelow)
        Me.Controls.Add(Me.radioAbove)
        Me.Controls.Add(Me.txtThreshold)
        Me.Controls.Add(Me.lblThreshold)
        Me.Controls.Add(Me.agdMain)
        Me.Controls.Add(Me.cboGroupBy)
        Me.Controls.Add(Me.lblGroupBy)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmSynoptic"
        Me.Text = "Synoptic Analysis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblGroupBy As System.Windows.Forms.Label
    Friend WithEvents cboGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents agdMain As atcControls.atcGrid
    Friend WithEvents txtThreshold As System.Windows.Forms.TextBox
    Friend WithEvents lblThreshold As System.Windows.Forms.Label
    Friend WithEvents radioAbove As System.Windows.Forms.RadioButton
    Friend WithEvents radioBelow As System.Windows.Forms.RadioButton
    Friend WithEvents cboGapUnits As System.Windows.Forms.ComboBox
    Friend WithEvents lblGap As System.Windows.Forms.Label
    Friend WithEvents txtGap As System.Windows.Forms.TextBox
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectData As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSelectAttributes As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEditCopy As System.Windows.Forms.MenuItem
    Friend WithEvents mnuView As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributeRows As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAttributeColumns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuViewSep1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSizeColumnsToContents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    Friend WithEvents lblDuringEvent As System.Windows.Forms.Label
    Friend WithEvents btnComputeEvents As System.Windows.Forms.Button
    Friend WithEvents mnuFileSaveAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuChooseColumns As System.Windows.Forms.MenuItem
    Friend WithEvents mnuGraph As System.Windows.Forms.MenuItem
End Class
