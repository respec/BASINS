<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpecifySplit
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
        Me.tabs = New System.Windows.Forms.TabControl
        Me.tabSeasonal = New System.Windows.Forms.TabPage
        Me.tabCustom = New System.Windows.Forms.TabPage
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.radioSeasonsSeparate = New System.Windows.Forms.RadioButton
        Me.radioSeasonsCombine = New System.Windows.Forms.RadioButton
        Me.tabs.SuspendLayout()
        Me.tabSeasonal.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabs
        '
        Me.tabs.Controls.Add(Me.tabSeasonal)
        Me.tabs.Controls.Add(Me.tabCustom)
        Me.tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabs.Location = New System.Drawing.Point(0, 0)
        Me.tabs.Name = "tabs"
        Me.tabs.SelectedIndex = 0
        Me.tabs.Size = New System.Drawing.Size(450, 412)
        Me.tabs.TabIndex = 0
        '
        'tabSeasonal
        '
        Me.tabSeasonal.Controls.Add(Me.radioSeasonsCombine)
        Me.tabSeasonal.Controls.Add(Me.radioSeasonsSeparate)
        Me.tabSeasonal.Controls.Add(Me.Label2)
        Me.tabSeasonal.Controls.Add(Me.Label1)
        Me.tabSeasonal.Controls.Add(Me.btnSeasonsNone)
        Me.tabSeasonal.Controls.Add(Me.btnSeasonsAll)
        Me.tabSeasonal.Controls.Add(Me.lstSeasons)
        Me.tabSeasonal.Controls.Add(Me.cboSeasons)
        Me.tabSeasonal.Location = New System.Drawing.Point(4, 25)
        Me.tabSeasonal.Name = "tabSeasonal"
        Me.tabSeasonal.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSeasonal.Size = New System.Drawing.Size(442, 383)
        Me.tabSeasonal.TabIndex = 0
        Me.tabSeasonal.Text = "Seasonal"
        Me.tabSeasonal.UseVisualStyleBackColor = True
        '
        'tabCustom
        '
        Me.tabCustom.Location = New System.Drawing.Point(4, 25)
        Me.tabCustom.Name = "tabCustom"
        Me.tabCustom.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCustom.Size = New System.Drawing.Size(400, 362)
        Me.tabCustom.TabIndex = 1
        Me.tabCustom.Text = "Custom"
        Me.tabCustom.UseVisualStyleBackColor = True
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(353, 336)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(76, 27)
        Me.btnSeasonsNone.TabIndex = 16
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(13, 336)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(76, 27)
        Me.btnSeasonsAll.TabIndex = 15
        Me.btnSeasonsAll.Text = "All"
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.ItemHeight = 16
        Me.lstSeasons.Location = New System.Drawing.Point(13, 70)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(416, 203)
        Me.lstSeasons.TabIndex = 14
        Me.lstSeasons.Tag = "Seasons"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.Enabled = False
        Me.cboSeasons.Location = New System.Drawing.Point(13, 23)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(416, 24)
        Me.cboSeasons.TabIndex = 13
        Me.cboSeasons.Tag = "SeasonType"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(198, 17)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Type of Seasons To Split Into:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(132, 17)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Seasons to Include:"
        '
        'radioSeasonsSeparate
        '
        Me.radioSeasonsSeparate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsSeparate.AutoSize = True
        Me.radioSeasonsSeparate.Checked = True
        Me.radioSeasonsSeparate.Location = New System.Drawing.Point(13, 279)
        Me.radioSeasonsSeparate.Name = "radioSeasonsSeparate"
        Me.radioSeasonsSeparate.Size = New System.Drawing.Size(315, 21)
        Me.radioSeasonsSeparate.TabIndex = 19
        Me.radioSeasonsSeparate.TabStop = True
        Me.radioSeasonsSeparate.Text = "Separate timeseries for each included season"
        Me.radioSeasonsSeparate.UseVisualStyleBackColor = True
        '
        'radioSeasonsCombine
        '
        Me.radioSeasonsCombine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsCombine.AutoSize = True
        Me.radioSeasonsCombine.Location = New System.Drawing.Point(13, 306)
        Me.radioSeasonsCombine.Name = "radioSeasonsCombine"
        Me.radioSeasonsCombine.Size = New System.Drawing.Size(399, 21)
        Me.radioSeasonsCombine.TabIndex = 20
        Me.radioSeasonsCombine.Text = "One timeseries containing all values from included seasons"
        Me.radioSeasonsCombine.UseVisualStyleBackColor = True
        '
        'frmSpecifySplit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(450, 412)
        Me.Controls.Add(Me.tabs)
        Me.Name = "frmSpecifySplit"
        Me.Text = "Split Timeseries"
        Me.tabs.ResumeLayout(False)
        Me.tabSeasonal.ResumeLayout(False)
        Me.tabSeasonal.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabs As System.Windows.Forms.TabControl
    Friend WithEvents tabSeasonal As System.Windows.Forms.TabPage
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents tabCustom As System.Windows.Forms.TabPage
    Friend WithEvents radioSeasonsCombine As System.Windows.Forms.RadioButton
    Friend WithEvents radioSeasonsSeparate As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
