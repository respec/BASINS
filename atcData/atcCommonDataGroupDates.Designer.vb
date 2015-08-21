<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class atcCommonDataGroupDates
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
        Me.lblCommonStart = New System.Windows.Forms.Label
        Me.lblCommonEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblOmitBefore = New System.Windows.Forms.Label
        Me.lblOmitAfter = New System.Windows.Forms.Label
        Me.lblCommon = New System.Windows.Forms.Label
        Me.lblAll = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblCommonStart
        '
        Me.lblCommonStart.AutoSize = True
        Me.lblCommonStart.Location = New System.Drawing.Point(110, 22)
        Me.lblCommonStart.Name = "lblCommonStart"
        Me.lblCommonStart.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonStart.TabIndex = 54
        Me.lblCommonStart.Tag = ""
        '
        'lblCommonEnd
        '
        Me.lblCommonEnd.AutoSize = True
        Me.lblCommonEnd.Location = New System.Drawing.Point(110, 45)
        Me.lblCommonEnd.Name = "lblCommonEnd"
        Me.lblCommonEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblCommonEnd.TabIndex = 53
        Me.lblCommonEnd.Tag = ""
        '
        'lblDataStart
        '
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(32, 22)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(0, 13)
        Me.lblDataStart.TabIndex = 52
        Me.lblDataStart.Tag = "Data Starts"
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(32, 45)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(0, 13)
        Me.lblDataEnd.TabIndex = 49
        Me.lblDataEnd.Tag = ""
        '
        'lblOmitBefore
        '
        Me.lblOmitBefore.AutoSize = True
        Me.lblOmitBefore.Location = New System.Drawing.Point(0, 22)
        Me.lblOmitBefore.Name = "lblOmitBefore"
        Me.lblOmitBefore.Size = New System.Drawing.Size(29, 13)
        Me.lblOmitBefore.TabIndex = 50
        Me.lblOmitBefore.Text = "Start"
        '
        'lblOmitAfter
        '
        Me.lblOmitAfter.AutoSize = True
        Me.lblOmitAfter.Location = New System.Drawing.Point(0, 45)
        Me.lblOmitAfter.Name = "lblOmitAfter"
        Me.lblOmitAfter.Size = New System.Drawing.Size(26, 13)
        Me.lblOmitAfter.TabIndex = 51
        Me.lblOmitAfter.Text = "End"
        '
        'lblCommon
        '
        Me.lblCommon.AutoSize = True
        Me.lblCommon.Location = New System.Drawing.Point(121, 0)
        Me.lblCommon.Name = "lblCommon"
        Me.lblCommon.Size = New System.Drawing.Size(48, 13)
        Me.lblCommon.TabIndex = 55
        Me.lblCommon.Text = "Common"
        '
        'lblAll
        '
        Me.lblAll.AutoSize = True
        Me.lblAll.Location = New System.Drawing.Point(47, 0)
        Me.lblAll.Name = "lblAll"
        Me.lblAll.Size = New System.Drawing.Size(18, 13)
        Me.lblAll.TabIndex = 56
        Me.lblAll.Text = "All"
        '
        'atcCommonDataGroupDates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblAll)
        Me.Controls.Add(Me.lblCommon)
        Me.Controls.Add(Me.lblCommonStart)
        Me.Controls.Add(Me.lblCommonEnd)
        Me.Controls.Add(Me.lblDataStart)
        Me.Controls.Add(Me.lblDataEnd)
        Me.Controls.Add(Me.lblOmitBefore)
        Me.Controls.Add(Me.lblOmitAfter)
        Me.Name = "atcCommonDataGroupDates"
        Me.Size = New System.Drawing.Size(197, 63)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCommonStart As System.Windows.Forms.Label
    Friend WithEvents lblCommonEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblOmitBefore As System.Windows.Forms.Label
    Friend WithEvents lblOmitAfter As System.Windows.Forms.Label
    Friend WithEvents lblCommon As System.Windows.Forms.Label
    Friend WithEvents lblAll As System.Windows.Forms.Label

End Class
