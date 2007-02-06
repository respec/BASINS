<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseColumns
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChooseColumns))
        Me.lstVolume = New System.Windows.Forms.CheckedListBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnRevert = New System.Windows.Forms.Button
        Me.chkEvents = New System.Windows.Forms.CheckBox
        Me.chkMeasurements = New System.Windows.Forms.CheckBox
        Me.chkStartTime = New System.Windows.Forms.CheckBox
        Me.chkStartDate = New System.Windows.Forms.CheckBox
        Me.lstDuration = New System.Windows.Forms.CheckedListBox
        Me.lstIntensity = New System.Windows.Forms.CheckedListBox
        Me.lstTimeSinceLast = New System.Windows.Forms.CheckedListBox
        Me.btnAddAttribute = New System.Windows.Forms.Button
        Me.btnAllVolume = New System.Windows.Forms.Button
        Me.btnNoneVolume = New System.Windows.Forms.Button
        Me.lblVolume = New System.Windows.Forms.Label
        Me.lblDuration = New System.Windows.Forms.Label
        Me.lblIntensity = New System.Windows.Forms.Label
        Me.lblTimeSinceLast = New System.Windows.Forms.Label
        Me.btnNoneDuration = New System.Windows.Forms.Button
        Me.btnAllDuration = New System.Windows.Forms.Button
        Me.btnNoneIntensity = New System.Windows.Forms.Button
        Me.btnAllIntensity = New System.Windows.Forms.Button
        Me.btnNoneTimeSinceLast = New System.Windows.Forms.Button
        Me.btnAllTimeSinceLast = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lstVolume
        '
        Me.lstVolume.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstVolume.CheckOnClick = True
        Me.lstVolume.FormattingEnabled = True
        Me.lstVolume.IntegralHeight = False
        Me.lstVolume.Location = New System.Drawing.Point(12, 25)
        Me.lstVolume.Name = "lstVolume"
        Me.lstVolume.Size = New System.Drawing.Size(102, 193)
        Me.lstVolume.TabIndex = 1
        Me.lstVolume.Tag = "Volume"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCancel.Location = New System.Drawing.Point(359, 284)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOk.Location = New System.Drawing.Point(273, 284)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(80, 24)
        Me.btnOk.TabIndex = 14
        Me.btnOk.Text = "Ok"
        '
        'btnRevert
        '
        Me.btnRevert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevert.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnRevert.Location = New System.Drawing.Point(187, 284)
        Me.btnRevert.Name = "btnRevert"
        Me.btnRevert.Size = New System.Drawing.Size(80, 24)
        Me.btnRevert.TabIndex = 13
        Me.btnRevert.Text = "Revert"
        '
        'chkEvents
        '
        Me.chkEvents.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkEvents.AutoSize = True
        Me.chkEvents.Location = New System.Drawing.Point(12, 251)
        Me.chkEvents.Name = "chkEvents"
        Me.chkEvents.Size = New System.Drawing.Size(59, 17)
        Me.chkEvents.TabIndex = 8
        Me.chkEvents.Text = "Events"
        Me.chkEvents.UseVisualStyleBackColor = True
        '
        'chkMeasurements
        '
        Me.chkMeasurements.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkMeasurements.AutoSize = True
        Me.chkMeasurements.Location = New System.Drawing.Point(77, 251)
        Me.chkMeasurements.Name = "chkMeasurements"
        Me.chkMeasurements.Size = New System.Drawing.Size(95, 17)
        Me.chkMeasurements.TabIndex = 9
        Me.chkMeasurements.Text = "Measurements"
        Me.chkMeasurements.UseVisualStyleBackColor = True
        '
        'chkStartTime
        '
        Me.chkStartTime.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkStartTime.AutoSize = True
        Me.chkStartTime.Location = New System.Drawing.Point(265, 251)
        Me.chkStartTime.Name = "chkStartTime"
        Me.chkStartTime.Size = New System.Drawing.Size(74, 17)
        Me.chkStartTime.TabIndex = 11
        Me.chkStartTime.Text = "Start Time"
        Me.chkStartTime.UseVisualStyleBackColor = True
        '
        'chkStartDate
        '
        Me.chkStartDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkStartDate.AutoSize = True
        Me.chkStartDate.Location = New System.Drawing.Point(185, 251)
        Me.chkStartDate.Name = "chkStartDate"
        Me.chkStartDate.Size = New System.Drawing.Size(74, 17)
        Me.chkStartDate.TabIndex = 10
        Me.chkStartDate.Text = "Start Date"
        Me.chkStartDate.UseVisualStyleBackColor = True
        '
        'lstDuration
        '
        Me.lstDuration.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstDuration.CheckOnClick = True
        Me.lstDuration.FormattingEnabled = True
        Me.lstDuration.IntegralHeight = False
        Me.lstDuration.Location = New System.Drawing.Point(120, 25)
        Me.lstDuration.Name = "lstDuration"
        Me.lstDuration.Size = New System.Drawing.Size(102, 193)
        Me.lstDuration.TabIndex = 3
        Me.lstDuration.Tag = "Duration"
        '
        'lstIntensity
        '
        Me.lstIntensity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstIntensity.CheckOnClick = True
        Me.lstIntensity.FormattingEnabled = True
        Me.lstIntensity.IntegralHeight = False
        Me.lstIntensity.Location = New System.Drawing.Point(228, 25)
        Me.lstIntensity.Name = "lstIntensity"
        Me.lstIntensity.Size = New System.Drawing.Size(102, 193)
        Me.lstIntensity.TabIndex = 5
        Me.lstIntensity.Tag = "Intensity"
        '
        'lstTimeSinceLast
        '
        Me.lstTimeSinceLast.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstTimeSinceLast.CheckOnClick = True
        Me.lstTimeSinceLast.FormattingEnabled = True
        Me.lstTimeSinceLast.IntegralHeight = False
        Me.lstTimeSinceLast.Location = New System.Drawing.Point(336, 25)
        Me.lstTimeSinceLast.Name = "lstTimeSinceLast"
        Me.lstTimeSinceLast.Size = New System.Drawing.Size(102, 193)
        Me.lstTimeSinceLast.TabIndex = 7
        Me.lstTimeSinceLast.Tag = "Time Since Last"
        '
        'btnAddAttribute
        '
        Me.btnAddAttribute.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddAttribute.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddAttribute.Location = New System.Drawing.Point(101, 284)
        Me.btnAddAttribute.Name = "btnAddAttribute"
        Me.btnAddAttribute.Size = New System.Drawing.Size(80, 24)
        Me.btnAddAttribute.TabIndex = 12
        Me.btnAddAttribute.Text = "Add Attribute"
        Me.btnAddAttribute.Visible = False
        '
        'btnAllVolume
        '
        Me.btnAllVolume.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAllVolume.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAllVolume.Location = New System.Drawing.Point(12, 221)
        Me.btnAllVolume.Name = "btnAllVolume"
        Me.btnAllVolume.Size = New System.Drawing.Size(38, 24)
        Me.btnAllVolume.TabIndex = 16
        Me.btnAllVolume.Text = "All"
        '
        'btnNoneVolume
        '
        Me.btnNoneVolume.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNoneVolume.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnNoneVolume.Location = New System.Drawing.Point(56, 221)
        Me.btnNoneVolume.Name = "btnNoneVolume"
        Me.btnNoneVolume.Size = New System.Drawing.Size(58, 24)
        Me.btnNoneVolume.TabIndex = 17
        Me.btnNoneVolume.Text = "None"
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.Location = New System.Drawing.Point(12, 9)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(42, 13)
        Me.lblVolume.TabIndex = 18
        Me.lblVolume.Text = "Volume"
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Location = New System.Drawing.Point(117, 9)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(47, 13)
        Me.lblDuration.TabIndex = 19
        Me.lblDuration.Text = "Duration"
        '
        'lblIntensity
        '
        Me.lblIntensity.AutoSize = True
        Me.lblIntensity.Location = New System.Drawing.Point(225, 9)
        Me.lblIntensity.Name = "lblIntensity"
        Me.lblIntensity.Size = New System.Drawing.Size(46, 13)
        Me.lblIntensity.TabIndex = 20
        Me.lblIntensity.Text = "Intensity"
        '
        'lblTimeSinceLast
        '
        Me.lblTimeSinceLast.AutoSize = True
        Me.lblTimeSinceLast.Location = New System.Drawing.Point(333, 9)
        Me.lblTimeSinceLast.Name = "lblTimeSinceLast"
        Me.lblTimeSinceLast.Size = New System.Drawing.Size(83, 13)
        Me.lblTimeSinceLast.TabIndex = 21
        Me.lblTimeSinceLast.Text = "Time Since Last"
        '
        'btnNoneDuration
        '
        Me.btnNoneDuration.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNoneDuration.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnNoneDuration.Location = New System.Drawing.Point(164, 221)
        Me.btnNoneDuration.Name = "btnNoneDuration"
        Me.btnNoneDuration.Size = New System.Drawing.Size(58, 24)
        Me.btnNoneDuration.TabIndex = 23
        Me.btnNoneDuration.Text = "None"
        '
        'btnAllDuration
        '
        Me.btnAllDuration.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAllDuration.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAllDuration.Location = New System.Drawing.Point(120, 221)
        Me.btnAllDuration.Name = "btnAllDuration"
        Me.btnAllDuration.Size = New System.Drawing.Size(38, 24)
        Me.btnAllDuration.TabIndex = 22
        Me.btnAllDuration.Text = "All"
        '
        'btnNoneIntensity
        '
        Me.btnNoneIntensity.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNoneIntensity.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnNoneIntensity.Location = New System.Drawing.Point(272, 221)
        Me.btnNoneIntensity.Name = "btnNoneIntensity"
        Me.btnNoneIntensity.Size = New System.Drawing.Size(58, 24)
        Me.btnNoneIntensity.TabIndex = 25
        Me.btnNoneIntensity.Text = "None"
        '
        'btnAllIntensity
        '
        Me.btnAllIntensity.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAllIntensity.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAllIntensity.Location = New System.Drawing.Point(228, 221)
        Me.btnAllIntensity.Name = "btnAllIntensity"
        Me.btnAllIntensity.Size = New System.Drawing.Size(38, 24)
        Me.btnAllIntensity.TabIndex = 24
        Me.btnAllIntensity.Text = "All"
        '
        'btnNoneTimeSinceLast
        '
        Me.btnNoneTimeSinceLast.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNoneTimeSinceLast.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnNoneTimeSinceLast.Location = New System.Drawing.Point(380, 221)
        Me.btnNoneTimeSinceLast.Name = "btnNoneTimeSinceLast"
        Me.btnNoneTimeSinceLast.Size = New System.Drawing.Size(58, 24)
        Me.btnNoneTimeSinceLast.TabIndex = 27
        Me.btnNoneTimeSinceLast.Text = "None"
        '
        'btnAllTimeSinceLast
        '
        Me.btnAllTimeSinceLast.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAllTimeSinceLast.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAllTimeSinceLast.Location = New System.Drawing.Point(336, 221)
        Me.btnAllTimeSinceLast.Name = "btnAllTimeSinceLast"
        Me.btnAllTimeSinceLast.Size = New System.Drawing.Size(38, 24)
        Me.btnAllTimeSinceLast.TabIndex = 26
        Me.btnAllTimeSinceLast.Text = "All"
        '
        'frmChooseColumns
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 320)
        Me.Controls.Add(Me.btnNoneTimeSinceLast)
        Me.Controls.Add(Me.btnAllTimeSinceLast)
        Me.Controls.Add(Me.btnNoneIntensity)
        Me.Controls.Add(Me.btnAllIntensity)
        Me.Controls.Add(Me.btnNoneDuration)
        Me.Controls.Add(Me.btnAllDuration)
        Me.Controls.Add(Me.lblTimeSinceLast)
        Me.Controls.Add(Me.lblIntensity)
        Me.Controls.Add(Me.lblDuration)
        Me.Controls.Add(Me.lblVolume)
        Me.Controls.Add(Me.btnNoneVolume)
        Me.Controls.Add(Me.btnAllVolume)
        Me.Controls.Add(Me.btnAddAttribute)
        Me.Controls.Add(Me.lstTimeSinceLast)
        Me.Controls.Add(Me.lstIntensity)
        Me.Controls.Add(Me.lstDuration)
        Me.Controls.Add(Me.chkStartTime)
        Me.Controls.Add(Me.chkStartDate)
        Me.Controls.Add(Me.chkMeasurements)
        Me.Controls.Add(Me.chkEvents)
        Me.Controls.Add(Me.btnRevert)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lstVolume)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChooseColumns"
        Me.Text = "Choose Columns"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstVolume As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnRevert As System.Windows.Forms.Button
    Friend WithEvents chkEvents As System.Windows.Forms.CheckBox
    Friend WithEvents chkMeasurements As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartTime As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartDate As System.Windows.Forms.CheckBox
    Friend WithEvents lstDuration As System.Windows.Forms.CheckedListBox
    Friend WithEvents lstIntensity As System.Windows.Forms.CheckedListBox
    Friend WithEvents lstTimeSinceLast As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnAddAttribute As System.Windows.Forms.Button
    Friend WithEvents btnAllVolume As System.Windows.Forms.Button
    Friend WithEvents btnNoneVolume As System.Windows.Forms.Button
    Friend WithEvents lblVolume As System.Windows.Forms.Label
    Friend WithEvents lblDuration As System.Windows.Forms.Label
    Friend WithEvents lblIntensity As System.Windows.Forms.Label
    Friend WithEvents lblTimeSinceLast As System.Windows.Forms.Label
    Friend WithEvents btnNoneDuration As System.Windows.Forms.Button
    Friend WithEvents btnAllDuration As System.Windows.Forms.Button
    Friend WithEvents btnNoneIntensity As System.Windows.Forms.Button
    Friend WithEvents btnAllIntensity As System.Windows.Forms.Button
    Friend WithEvents btnNoneTimeSinceLast As System.Windows.Forms.Button
    Friend WithEvents btnAllTimeSinceLast As System.Windows.Forms.Button
End Class
