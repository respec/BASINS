<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcChooseDataGroupDates
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
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblCommonStart = New System.Windows.Forms.Label
        Me.lblCommonEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.radioAll = New System.Windows.Forms.RadioButton
        Me.radioCommon = New System.Windows.Forms.RadioButton
        Me.radioCustom = New System.Windows.Forms.RadioButton
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.txtOmitBefore = New System.Windows.Forms.TextBox
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.txtOmitAfter = New System.Windows.Forms.TextBox
        Me.grpYears.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCommonStart
        '
        Me.lblCommonStart.AutoSize = True
        Me.lblCommonStart.Location = New System.Drawing.Point(116, 36)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(65, 13)
        Me.lblCommonStart.TabIndex = 48
        Me.lblCommonStart.Tag = ""
        Me.lblCommonStart.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblCommonStart, "First Common Date")
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(116, 62)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(65, 13)
        Me.lblCommonEnd.TabIndex = 47
        Me.lblCommonEnd.Tag = ""
        Me.lblCommonEnd.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblCommonEnd, "Last Common Date")
        '
        'lblDataStart
        '
        Me.lblDataStart.Location = New System.Drawing.Point(38, 36)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(65, 13)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblDataStart, "First Start Date")
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(38, 62)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(65, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = ""
        Me.lblDataEnd.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblDataEnd, "Last End Date")
        '
        'radioAll
        '
        Me.radioAll.AutoSize = True
        Me.radioAll.Checked = True
        Me.radioAll.Location = New System.Drawing.Point(41, 16)
        Me.radioAll.Name = "radioAll"
        Me.radioAll.Size = New System.Drawing.Size(67, 17)
        Me.radioAll.TabIndex = 1
        Me.radioAll.TabStop = True
        Me.radioAll.Text = "All Dates"
        Me.ToolTip1.SetToolTip(Me.radioAll, "Start of earliest dataset to end of latest")
        Me.radioAll.UseVisualStyleBackColor = True
        '
        'radioCommon
        '
        Me.radioCommon.AutoSize = True
        Me.radioCommon.Location = New System.Drawing.Point(114, 16)
        Me.radioCommon.Name = "radioCommon"
        Me.radioCommon.Size = New System.Drawing.Size(66, 17)
        Me.radioCommon.TabIndex = 2
        Me.radioCommon.Text = "Common"
        Me.ToolTip1.SetToolTip(Me.radioCommon, "Overlapping period of all selected data")
        Me.radioCommon.UseVisualStyleBackColor = True
        '
        'radioCustom
        '
        Me.radioCustom.AutoSize = True
        Me.radioCustom.Location = New System.Drawing.Point(197, 16)
        Me.radioCustom.Name = "radioCustom"
        Me.radioCustom.Size = New System.Drawing.Size(60, 17)
        Me.radioCustom.TabIndex = 3
        Me.radioCustom.Text = "Custom"
        Me.ToolTip1.SetToolTip(Me.radioCustom, "Enter custom period of interest")
        Me.radioCustom.UseVisualStyleBackColor = True
        '
        'grpYears
        '
        Me.grpYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpYears.Controls.Add(Me.txtOmitBefore)
        Me.grpYears.Controls.Add(Me.radioCustom)
        Me.grpYears.Controls.Add(Me.radioCommon)
        Me.grpYears.Controls.Add(Me.radioAll)
        Me.grpYears.Controls.Add(Me.lblCommonStart)
        Me.grpYears.Controls.Add(Me.lblCommonEnd)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfter)
        Me.grpYears.Location = New System.Drawing.Point(0, 0)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(279, 85)
        Me.grpYears.TabIndex = 71
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Dates to Include"
        '
        'txtOmitBefore
        '
        Me.txtOmitBefore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitBefore.Location = New System.Drawing.Point(197, 33)
        Me.txtOmitBefore.Name = "txtOmitBefore"
        Me.txtOmitBefore.Size = New System.Drawing.Size(76, 20)
        Me.txtOmitBefore.TabIndex = 5
        Me.txtOmitBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(6, 36)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(29, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(6, 62)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(26, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End"
        '
        'txtOmitAfter
        '
        Me.txtOmitAfter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitAfter.Location = New System.Drawing.Point(197, 59)
        Me.txtOmitAfter.Name = "txtOmitAfter"
        Me.txtOmitAfter.Size = New System.Drawing.Size(76, 20)
        Me.txtOmitAfter.TabIndex = 6
        Me.txtOmitAfter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'atcChooseDataGroupDates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpYears)
        Me.Name = "atcChooseDataGroupDates"
        Me.Size = New System.Drawing.Size(279, 85)
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents grpYears As System.Windows.Forms.GroupBox
    Friend WithEvents lblCommonStart As System.Windows.Forms.Label
    Friend WithEvents lblCommonEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents txtOmitAfter As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitBefore As System.Windows.Forms.TextBox
    Friend WithEvents radioAll As System.Windows.Forms.RadioButton
    Friend WithEvents radioCustom As System.Windows.Forms.RadioButton
    Friend WithEvents radioCommon As System.Windows.Forms.RadioButton

End Class
