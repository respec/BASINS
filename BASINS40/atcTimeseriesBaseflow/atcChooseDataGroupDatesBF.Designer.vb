<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcChooseDataGroupDatesBF
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
        Me.chkYearly = New System.Windows.Forms.CheckBox
        Me.grpYears.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCommonStart
        '
        Me.lblCommonStart.AutoSize = True
        Me.lblCommonStart.Location = New System.Drawing.Point(128, 48)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonStart.TabIndex = 48
        Me.lblCommonStart.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblCommonStart, "First Common Date")
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(128, 71)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonEnd.TabIndex = 47
        Me.lblCommonEnd.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblCommonEnd, "Last Common Date")
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(50, 48)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(0, 13)
        Me.lblDataStart.TabIndex = 45
        Me.lblDataStart.Tag = "Data Starts"
        Me.ToolTip1.SetToolTip(Me.lblDataStart, "First Start Date")
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(50, 71)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Tag = ""
        Me.ToolTip1.SetToolTip(Me.lblDataEnd, "Last End Date")
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(64, 19)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(64, 20)
        Me.btnAll.TabIndex = 49
        Me.btnAll.Text = "All"
        Me.ToolTip1.SetToolTip(Me.btnAll, "Start of earliest dataset to end of latest")
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnCommon
        '
        Me.btnCommon.Location = New System.Drawing.Point(134, 19)
        Me.btnCommon.Name = "btnCommon"
        Me.btnCommon.Size = New System.Drawing.Size(64, 20)
        Me.btnCommon.TabIndex = 50
        Me.btnCommon.Text = "Common"
        Me.ToolTip1.SetToolTip(Me.btnCommon, "Overlapping period of all selected data")
        Me.btnCommon.UseVisualStyleBackColor = True
        '
        'grpYears
        '
        Me.grpYears.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grpYears.Controls.Add(Me.lblCommonStart)
        Me.grpYears.Controls.Add(Me.chkYearly)
        Me.grpYears.Controls.Add(Me.lblCommonEnd)
        Me.grpYears.Controls.Add(Me.lblDataStart)
        Me.grpYears.Controls.Add(Me.lblDataEnd)
        Me.grpYears.Controls.Add(Me.btnCommon)
        Me.grpYears.Controls.Add(Me.txtOmitAfter)
        Me.grpYears.Controls.Add(Me.btnAll)
        Me.grpYears.Controls.Add(Me.txtOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitBefore)
        Me.grpYears.Controls.Add(Me.lblOmitAfter)
        Me.grpYears.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grpYears.Location = New System.Drawing.Point(0, 0)
        Me.grpYears.Name = "grpYears"
        Me.grpYears.Size = New System.Drawing.Size(359, 123)
        Me.grpYears.TabIndex = 71
        Me.grpYears.TabStop = False
        Me.grpYears.Text = "Dates to Include"
        '
        'txtOmitBefore
        '
        Me.txtOmitBefore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitBefore.Location = New System.Drawing.Point(246, 45)
        Me.txtOmitBefore.Name = "txtOmitBefore"
        Me.txtOmitBefore.Size = New System.Drawing.Size(104, 20)
        Me.txtOmitBefore.TabIndex = 5
        Me.txtOmitBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(9, 48)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(29, 13)
        Me.lblOmitBefore.TabIndex = 40
        Me.lblOmitBefore.Text = "Start"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(12, 71)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(26, 13)
        Me.lblOmitAfter.TabIndex = 43
        Me.lblOmitAfter.Text = "End"
        '
        'txtOmitAfter
        '
        Me.txtOmitAfter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOmitAfter.Location = New System.Drawing.Point(246, 71)
        Me.txtOmitAfter.Name = "txtOmitAfter"
        Me.txtOmitAfter.Size = New System.Drawing.Size(104, 20)
        Me.txtOmitAfter.TabIndex = 6
        Me.txtOmitAfter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkYearly
        '
        Me.chkYearly.AutoSize = True
        Me.chkYearly.Location = New System.Drawing.Point(12, 98)
        Me.chkYearly.Name = "chkYearly"
        Me.chkYearly.Size = New System.Drawing.Size(208, 17)
        Me.chkYearly.TabIndex = 51
        Me.chkYearly.Text = "Baseflow separation one year at a time"
        Me.chkYearly.UseVisualStyleBackColor = True
        Me.chkYearly.Visible = False
        '
        'atcChooseDataGroupDatesBF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.grpYears)
        Me.Name = "atcChooseDataGroupDatesBF"
        Me.Size = New System.Drawing.Size(364, 126)
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
    Friend WithEvents btnCommon As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents chkYearly As System.Windows.Forms.CheckBox

End Class
