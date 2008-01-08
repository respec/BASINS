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
        Me.btnCommon = New System.Windows.Forms.Button
        Me.lblCommonStart = New System.Windows.Forms.Label
        Me.lblCommonEnd = New System.Windows.Forms.Label
        Me.btnAllDates = New System.Windows.Forms.Button
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.grpYears = New System.Windows.Forms.GroupBox
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.txtOmitAfter = New System.Windows.Forms.TextBox
        Me.txtOmitBefore = New System.Windows.Forms.TextBox
        Me.grpYears.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCommon
        '
        Me.btnCommon.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCommon.Location = New System.Drawing.Point(204, 10)
        Me.btnCommon.Name = "btnCommon"
        Me.btnCommon.Size = New System.Drawing.Size(62, 23)
        Me.btnCommon.TabIndex = 49
        Me.btnCommon.Text = "Common"
        Me.ToolTip1.SetToolTip(Me.btnCommon, "Choose the dates common to all selected data")
        Me.btnCommon.UseVisualStyleBackColor = True
        '
        'lblCommonStart
        '
        Me.lblCommonStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCommonStart.AutoSize = True
        Me.lblCommonStart.Location = New System.Drawing.Point(201, 36)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(65, 13)
        Me.lblCommonStart.TabIndex = 48
        Me.lblCommonStart.Tag = ""
        Me.lblCommonStart.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblCommonStart, "First Common Date")
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(201, 62)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(65, 13)
        Me.lblCommonEnd.TabIndex = 47
        Me.lblCommonEnd.Tag = ""
        Me.lblCommonEnd.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblCommonEnd, "Last Common Date")
        '
        'btnAllDates
        '
        Me.btnAllDates.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAllDates.Location = New System.Drawing.Point(126, 10)
        Me.btnAllDates.Name = "btnAllDates"
        Me.btnAllDates.Size = New System.Drawing.Size(62, 23)
        Me.btnAllDates.TabIndex = 46
        Me.btnAllDates.Text = "All Dates"
        Me.ToolTip1.SetToolTip(Me.btnAllDates, "Choose widest date range with any data")
        Me.btnAllDates.UseVisualStyleBackColor = True
        '
        'lblDataStart
        '
        Me.lblDataStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(123, 36)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(65, 13)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.lblDataStart.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblDataStart, "First Start Date")
        '
        'lblDataEnd
        '
        Me.lblDataEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(123, 62)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(65, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = ""
        Me.lblDataEnd.Text = "11/22/1934"
        Me.ToolTip1.SetToolTip(Me.lblDataEnd, "Last End Date")
        '
        'grpYears
        '
        Me.grpYears.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpYears.Controls.Add(Me.btnCommon)
        Me.grpYears.Controls.Add(Me.lblCommonStart)
        Me.grpYears.Controls.Add(Me.lblCommonEnd)
        Me.grpYears.Controls.Add(Me.btnAllDates)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitAfter)
        Me.grpYears.Controls.Add(Me.txtOmitBefore)
        Me.grpYears.Location = New System.Drawing.Point(0, 0)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(279, 85)
        Me.grpYears.TabIndex = 71
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Dates to Include"
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
        Me.txtOmitAfter.Location = New System.Drawing.Point(41, 59)
        Me.txtOmitAfter.Name = "txtOmitAfter"
        Me.txtOmitAfter.Size = New System.Drawing.Size(76, 20)
        Me.txtOmitAfter.TabIndex = 6
        Me.txtOmitAfter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtOmitBefore
        '
        Me.txtOmitBefore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitBefore.Location = New System.Drawing.Point(41, 33)
        Me.txtOmitBefore.Name = "txtOmitBefore"
        Me.txtOmitBefore.Size = New System.Drawing.Size(76, 20)
        Me.txtOmitBefore.TabIndex = 5
        Me.txtOmitBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
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
    Friend WithEvents btnCommon As System.Windows.Forms.Button
    Friend WithEvents lblCommonStart As System.Windows.Forms.Label
    Friend WithEvents lblCommonEnd As System.Windows.Forms.Label
    Friend WithEvents btnAllDates As System.Windows.Forms.Button
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents txtOmitAfter As System.Windows.Forms.TextBox
    Friend WithEvents txtOmitBefore As System.Windows.Forms.TextBox

End Class
