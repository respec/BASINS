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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSpecifySplit))
        Me.txtGroupSeasons = New System.Windows.Forms.TextBox
        Me.radioSeasonsGroup = New System.Windows.Forms.RadioButton
        Me.radioSeasonsCombine = New System.Windows.Forms.RadioButton
        Me.radioSeasonsSeparate = New System.Windows.Forms.RadioButton
        Me.lblSeasons = New System.Windows.Forms.Label
        Me.lblSeasonType = New System.Windows.Forms.Label
        Me.btnSeasonsNone = New System.Windows.Forms.Button
        Me.btnSeasonsAll = New System.Windows.Forms.Button
        Me.lstSeasons = New System.Windows.Forms.ListBox
        Me.cboSeasons = New System.Windows.Forms.ComboBox
        Me.lblGroupSeasons = New System.Windows.Forms.Label
        Me.btnSplit = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtGroupSeasons
        '
        Me.txtGroupSeasons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtGroupSeasons.Location = New System.Drawing.Point(224, 333)
        Me.txtGroupSeasons.Name = "txtGroupSeasons"
        Me.txtGroupSeasons.Size = New System.Drawing.Size(33, 20)
        Me.txtGroupSeasons.TabIndex = 8
        '
        'radioSeasonsGroup
        '
        Me.radioSeasonsGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsGroup.AutoSize = True
        Me.radioSeasonsGroup.Location = New System.Drawing.Point(11, 334)
        Me.radioSeasonsGroup.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsGroup.Name = "radioSeasonsGroup"
        Me.radioSeasonsGroup.Size = New System.Drawing.Size(207, 17)
        Me.radioSeasonsGroup.TabIndex = 7
        Me.radioSeasonsGroup.Text = "Separate time series for each group of "
        Me.radioSeasonsGroup.UseVisualStyleBackColor = True
        '
        'radioSeasonsCombine
        '
        Me.radioSeasonsCombine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsCombine.AutoSize = True
        Me.radioSeasonsCombine.Location = New System.Drawing.Point(11, 292)
        Me.radioSeasonsCombine.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsCombine.Name = "radioSeasonsCombine"
        Me.radioSeasonsCombine.Size = New System.Drawing.Size(304, 17)
        Me.radioSeasonsCombine.TabIndex = 5
        Me.radioSeasonsCombine.Text = "One time series containing all values from selected seasons"
        Me.radioSeasonsCombine.UseVisualStyleBackColor = True
        '
        'radioSeasonsSeparate
        '
        Me.radioSeasonsSeparate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.radioSeasonsSeparate.AutoSize = True
        Me.radioSeasonsSeparate.Checked = True
        Me.radioSeasonsSeparate.Location = New System.Drawing.Point(11, 313)
        Me.radioSeasonsSeparate.Margin = New System.Windows.Forms.Padding(2)
        Me.radioSeasonsSeparate.Name = "radioSeasonsSeparate"
        Me.radioSeasonsSeparate.Size = New System.Drawing.Size(242, 17)
        Me.radioSeasonsSeparate.TabIndex = 6
        Me.radioSeasonsSeparate.TabStop = True
        Me.radioSeasonsSeparate.Text = "Separate time series for each selected season"
        Me.radioSeasonsSeparate.UseVisualStyleBackColor = True
        '
        'lblSeasons
        '
        Me.lblSeasons.AutoSize = True
        Me.lblSeasons.Location = New System.Drawing.Point(8, 47)
        Me.lblSeasons.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSeasons.Name = "lblSeasons"
        Me.lblSeasons.Size = New System.Drawing.Size(100, 13)
        Me.lblSeasons.TabIndex = 28
        Me.lblSeasons.Text = "Seasons to include:"
        '
        'lblSeasonType
        '
        Me.lblSeasonType.AutoSize = True
        Me.lblSeasonType.Location = New System.Drawing.Point(8, 9)
        Me.lblSeasonType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSeasonType.Name = "lblSeasonType"
        Me.lblSeasonType.Size = New System.Drawing.Size(141, 13)
        Me.lblSeasonType.TabIndex = 27
        Me.lblSeasonType.Text = "Type of seasons to split into:"
        '
        'btnSeasonsNone
        '
        Me.btnSeasonsNone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsNone.Location = New System.Drawing.Point(270, 266)
        Me.btnSeasonsNone.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSeasonsNone.Name = "btnSeasonsNone"
        Me.btnSeasonsNone.Size = New System.Drawing.Size(57, 22)
        Me.btnSeasonsNone.TabIndex = 4
        Me.btnSeasonsNone.Text = "None"
        '
        'btnSeasonsAll
        '
        Me.btnSeasonsAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSeasonsAll.Location = New System.Drawing.Point(11, 266)
        Me.btnSeasonsAll.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSeasonsAll.Name = "btnSeasonsAll"
        Me.btnSeasonsAll.Size = New System.Drawing.Size(57, 22)
        Me.btnSeasonsAll.TabIndex = 3
        Me.btnSeasonsAll.Text = "All"
        '
        'lstSeasons
        '
        Me.lstSeasons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstSeasons.IntegralHeight = False
        Me.lstSeasons.Location = New System.Drawing.Point(11, 62)
        Me.lstSeasons.Margin = New System.Windows.Forms.Padding(2)
        Me.lstSeasons.Name = "lstSeasons"
        Me.lstSeasons.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstSeasons.Size = New System.Drawing.Size(316, 200)
        Me.lstSeasons.TabIndex = 2
        Me.lstSeasons.Tag = "Seasons"
        '
        'cboSeasons
        '
        Me.cboSeasons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSeasons.Location = New System.Drawing.Point(11, 24)
        Me.cboSeasons.Margin = New System.Windows.Forms.Padding(2)
        Me.cboSeasons.MaxDropDownItems = 20
        Me.cboSeasons.Name = "cboSeasons"
        Me.cboSeasons.Size = New System.Drawing.Size(316, 21)
        Me.cboSeasons.TabIndex = 1
        Me.cboSeasons.Tag = "SeasonType"
        '
        'lblGroupSeasons
        '
        Me.lblGroupSeasons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblGroupSeasons.AutoSize = True
        Me.lblGroupSeasons.Location = New System.Drawing.Point(267, 336)
        Me.lblGroupSeasons.Name = "lblGroupSeasons"
        Me.lblGroupSeasons.Size = New System.Drawing.Size(46, 13)
        Me.lblGroupSeasons.TabIndex = 33
        Me.lblGroupSeasons.Text = "seasons"
        '
        'btnSplit
        '
        Me.btnSplit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSplit.Location = New System.Drawing.Point(270, 362)
        Me.btnSplit.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSplit.Name = "btnSplit"
        Me.btnSplit.Size = New System.Drawing.Size(57, 22)
        Me.btnSplit.TabIndex = 10
        Me.btnSplit.Text = "Split"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(200, 362)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(57, 22)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Cancel"
        '
        'frmSpecifySplit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 395)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSplit)
        Me.Controls.Add(Me.lblGroupSeasons)
        Me.Controls.Add(Me.txtGroupSeasons)
        Me.Controls.Add(Me.radioSeasonsGroup)
        Me.Controls.Add(Me.radioSeasonsCombine)
        Me.Controls.Add(Me.radioSeasonsSeparate)
        Me.Controls.Add(Me.lblSeasons)
        Me.Controls.Add(Me.lblSeasonType)
        Me.Controls.Add(Me.btnSeasonsNone)
        Me.Controls.Add(Me.btnSeasonsAll)
        Me.Controls.Add(Me.lstSeasons)
        Me.Controls.Add(Me.cboSeasons)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmSpecifySplit"
        Me.Text = "Split Time Series"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtGroupSeasons As System.Windows.Forms.TextBox
    Friend WithEvents radioSeasonsGroup As System.Windows.Forms.RadioButton
    Friend WithEvents radioSeasonsCombine As System.Windows.Forms.RadioButton
    Friend WithEvents radioSeasonsSeparate As System.Windows.Forms.RadioButton
    Friend WithEvents lblSeasons As System.Windows.Forms.Label
    Friend WithEvents lblSeasonType As System.Windows.Forms.Label
    Friend WithEvents btnSeasonsNone As System.Windows.Forms.Button
    Friend WithEvents btnSeasonsAll As System.Windows.Forms.Button
    Friend WithEvents lstSeasons As System.Windows.Forms.ListBox
    Friend WithEvents cboSeasons As System.Windows.Forms.ComboBox
    Friend WithEvents lblGroupSeasons As System.Windows.Forms.Label
    Friend WithEvents btnSplit As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
