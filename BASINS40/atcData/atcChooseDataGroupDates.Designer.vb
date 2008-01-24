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
        Me.btnAll = New System.Windows.Forms.Button
        Me.btnCommon = New System.Windows.Forms.Button
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
        Me.lblCommonStart.Location = New System.Drawing.Point(155, 48)
        Me.lblCommonStart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(0, 17)
        Me.lblCommonStart.TabIndex = 48
        Me.lblCommonStart.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblCommonStart, "First Common Date")
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(155, 76)
        Me.lblCommonEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(0, 17)
        Me.lblCommonEnd.TabIndex = 47
        Me.lblCommonEnd.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblCommonEnd, "Last Common Date")
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(51, 48)
        Me.lblDataStart.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(0, 17)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.ToolTip1.SetToolTip(Me.lblDataStart, "First Start Date")
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(51, 76)
        Me.lblDataEnd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(0, 17)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblDataEnd, "Last End Date")
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(55, 20)
        Me.btnAll.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(85, 25)
        Me.btnAll.TabIndex = 49
        Me.btnAll.Text = "All"
        Me.ToolTip1.SetToolTip(Me.btnAll, "Start of earliest dataset to end of latest")
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnCommon
        '
        Me.btnCommon.Location = New System.Drawing.Point(159, 20)
        Me.btnCommon.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCommon.Name = "btnCommon"
        Me.btnCommon.Size = New System.Drawing.Size(85, 25)
        Me.btnCommon.TabIndex = 50
        Me.btnCommon.Text = "Common"
        Me.ToolTip1.SetToolTip(Me.btnCommon, "Overlapping period of all selected data")
        Me.btnCommon.UseVisualStyleBackColor = True
        '
        'grpYears
        '
        Me.grpYears.AutoSize = True
        Me.grpYears.Controls.Add(Me.btnCommon)
        Me.grpYears.Controls.Add(Me.btnAll)
        Me.grpYears.Controls.Add(Me.txtOmitBefore)
        Me.grpYears.Controls.Add(Me.lblCommonStart)
        Me.grpYears.Controls.Add(Me.lblCommonEnd)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfter)
        Me.grpYears.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpYears.Location = New System.Drawing.Point(0, 0)
        Me.grpYears.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpYears.Size = New System.Drawing.Size(372, 108)
        Me.grpYears.TabIndex = 71
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Dates to Include"
        '
        'txtOmitBefore
        '
        Me.txtOmitBefore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitBefore.Location = New System.Drawing.Point(263, 44)
        Me.txtOmitBefore.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtOmitBefore.Name = "txtOmitBefore"
        Me.txtOmitBefore.Size = New System.Drawing.Size(100, 22)
        Me.txtOmitBefore.TabIndex = 5
        Me.txtOmitBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(8, 48)
        Me.lblOmitBefore.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(38, 17)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(8, 76)
        Me.lblOmitAfter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(33, 17)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End"
        '
        'txtOmitAfter
        '
        Me.txtOmitAfter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitAfter.Location = New System.Drawing.Point(263, 73)
        Me.txtOmitAfter.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtOmitAfter.Name = "txtOmitAfter"
        Me.txtOmitAfter.Size = New System.Drawing.Size(100, 22)
        Me.txtOmitAfter.TabIndex = 6
        Me.txtOmitAfter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'atcChooseDataGroupDates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpYears)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "atcChooseDataGroupDates"
        Me.Size = New System.Drawing.Size(372, 108)
        Me.grpYears.ResumeLayout(False)
        Me.grpYears.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    Friend WithEvents btnCommon As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button

End Class
