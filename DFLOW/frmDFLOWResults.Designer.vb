<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDFLOWResults
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDFLOWResults))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.SepcifyInputsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveinputsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveGridToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AttributesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblYears = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.lblSeasons = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.DFLOWHelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutDFLOWToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.agrResults = New atcControls.atcGrid
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.AttributesToolStripMenuItem, Me.ViewToolStripMenuItem, Me.SelectionToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(892, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectDataToolStripMenuItem, Me.ToolStripMenuItem1, Me.SepcifyInputsToolStripMenuItem, Me.SaveinputsToolStripMenuItem, Me.ToolStripMenuItem2, Me.SaveGridToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'SelectDataToolStripMenuItem
        '
        Me.SelectDataToolStripMenuItem.Name = "SelectDataToolStripMenuItem"
        Me.SelectDataToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SelectDataToolStripMenuItem.Text = "Select Data"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(140, 6)
        '
        'SepcifyInputsToolStripMenuItem
        '
        Me.SepcifyInputsToolStripMenuItem.Name = "SepcifyInputsToolStripMenuItem"
        Me.SepcifyInputsToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SepcifyInputsToolStripMenuItem.Text = "Specify Inputs"
        '
        'SaveinputsToolStripMenuItem
        '
        Me.SaveinputsToolStripMenuItem.Name = "SaveinputsToolStripMenuItem"
        Me.SaveinputsToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SaveinputsToolStripMenuItem.Text = "Save [inputs]"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(140, 6)
        '
        'SaveGridToolStripMenuItem
        '
        Me.SaveGridToolStripMenuItem.Name = "SaveGridToolStripMenuItem"
        Me.SaveGridToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.SaveGridToolStripMenuItem.Text = "Save Grid"
        '
        'AttributesToolStripMenuItem
        '
        Me.AttributesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem, Me.CopyAllToolStripMenuItem})
        Me.AttributesToolStripMenuItem.Name = "AttributesToolStripMenuItem"
        Me.AttributesToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.AttributesToolStripMenuItem.Text = "Edit"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.CopyToolStripMenuItem.Text = "Copy Base"
        '
        'CopyAllToolStripMenuItem
        '
        Me.CopyAllToolStripMenuItem.Name = "CopyAllToolStripMenuItem"
        Me.CopyAllToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.CopyAllToolStripMenuItem.Text = "Copy All"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'SelectionToolStripMenuItem
        '
        Me.SelectionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DFLOWHelpToolStripMenuItem, Me.ToolStripMenuItem4, Me.AboutDFLOWToolStripMenuItem})
        Me.SelectionToolStripMenuItem.Name = "SelectionToolStripMenuItem"
        Me.SelectionToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.SelectionToolStripMenuItem.Text = "Help"
        '
        'lblYears
        '
        Me.lblYears.AutoSize = True
        Me.lblYears.Location = New System.Drawing.Point(12, 34)
        Me.lblYears.Name = "lblYears"
        Me.lblYears.Size = New System.Drawing.Size(58, 13)
        Me.lblYears.TabIndex = 8
        Me.lblYears.Text = "lblSeasons"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(772, 37)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(108, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Copy to Clipboard"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblSeasons
        '
        Me.lblSeasons.AutoSize = True
        Me.lblSeasons.Location = New System.Drawing.Point(12, 55)
        Me.lblSeasons.Name = "lblSeasons"
        Me.lblSeasons.Size = New System.Drawing.Size(58, 13)
        Me.lblSeasons.TabIndex = 10
        Me.lblSeasons.Text = "lblSeasons"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 262)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(282, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Double-click on biological flow value for excursion analysis"
        '
        'DFLOWHelpToolStripMenuItem
        '
        Me.DFLOWHelpToolStripMenuItem.Name = "DFLOWHelpToolStripMenuItem"
        Me.DFLOWHelpToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DFLOWHelpToolStripMenuItem.Text = "DFLOW &Help"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(149, 6)
        '
        'AboutDFLOWToolStripMenuItem
        '
        Me.AboutDFLOWToolStripMenuItem.Name = "AboutDFLOWToolStripMenuItem"
        Me.AboutDFLOWToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AboutDFLOWToolStripMenuItem.Text = "&About DFLOW"
        '
        'agrResults
        '
        Me.agrResults.AllowHorizontalScrolling = True
        Me.agrResults.AllowNewValidValues = False
        Me.agrResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agrResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.agrResults.CellBackColor = System.Drawing.Color.Empty
        Me.agrResults.LineColor = System.Drawing.Color.Empty
        Me.agrResults.LineWidth = 0.0!
        Me.agrResults.Location = New System.Drawing.Point(0, 73)
        Me.agrResults.Name = "agrResults"
        Me.agrResults.Size = New System.Drawing.Size(892, 183)
        Me.agrResults.Source = Nothing
        Me.agrResults.TabIndex = 7
        '
        'frmDFLOWResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 283)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblSeasons)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblYears)
        Me.Controls.Add(Me.agrResults)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmDFLOWResults"
        Me.Text = "DFLOW Results"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AttributesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectDataToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SepcifyInputsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveinputsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents agrResults As atcControls.atcGrid
    Friend WithEvents lblYears As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblSeasons As System.Windows.Forms.Label
    Friend WithEvents SaveGridToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DFLOWHelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutDFLOWToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
