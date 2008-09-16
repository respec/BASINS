<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDefaultTimeInterval
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDefaultTimeInterval))
        Me.lblTimestep = New System.Windows.Forms.Label
        Me.atcTextTimeStep = New atcControls.atcText
        Me.lstTimeUnits = New System.Windows.Forms.ListBox
        Me.lblTimeUnits = New System.Windows.Forms.Label
        Me.lblInstructions = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnAll = New System.Windows.Forms.Button
        Me.btnSkip = New System.Windows.Forms.Button
        Me.btnSkipAll = New System.Windows.Forms.Button
        Me.lstAggregation = New System.Windows.Forms.ListBox
        Me.lblAggregation = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblTimestep
        '
        Me.lblTimestep.AutoSize = True
        Me.lblTimestep.Location = New System.Drawing.Point(62, 36)
        Me.lblTimestep.Name = "lblTimestep"
        Me.lblTimestep.Size = New System.Drawing.Size(70, 17)
        Me.lblTimestep.TabIndex = 11
        Me.lblTimestep.Text = "Time step"
        Me.lblTimestep.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'atcTextTimeStep
        '
        Me.atcTextTimeStep.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.atcTextTimeStep.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atcTextTimeStep.DefaultValue = 1
        Me.atcTextTimeStep.HardMax = 60
        Me.atcTextTimeStep.HardMin = 1
        Me.atcTextTimeStep.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.Location = New System.Drawing.Point(150, 33)
        Me.atcTextTimeStep.MaxDecimal = 0
        Me.atcTextTimeStep.maxWidth = 0
        Me.atcTextTimeStep.Name = "atcTextTimeStep"
        Me.atcTextTimeStep.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.SelLength = 0
        Me.atcTextTimeStep.SelStart = 1
        Me.atcTextTimeStep.Size = New System.Drawing.Size(56, 20)
        Me.atcTextTimeStep.SoftMax = 0
        Me.atcTextTimeStep.SoftMin = 0
        Me.atcTextTimeStep.TabIndex = 10
        Me.atcTextTimeStep.TabStop = False
        Me.atcTextTimeStep.Value = CType(1, Long)
        '
        'lstTimeUnits
        '
        Me.lstTimeUnits.ColumnWidth = 10
        Me.lstTimeUnits.FormattingEnabled = True
        Me.lstTimeUnits.ItemHeight = 16
        Me.lstTimeUnits.Items.AddRange(New Object() {"Minutes", "Hours", "Days", "Months"})
        Me.lstTimeUnits.Location = New System.Drawing.Point(150, 59)
        Me.lstTimeUnits.Name = "lstTimeUnits"
        Me.lstTimeUnits.Size = New System.Drawing.Size(98, 52)
        Me.lstTimeUnits.TabIndex = 9
        '
        'lblTimeUnits
        '
        Me.lblTimeUnits.AutoSize = True
        Me.lblTimeUnits.Location = New System.Drawing.Point(62, 59)
        Me.lblTimeUnits.Name = "lblTimeUnits"
        Me.lblTimeUnits.Size = New System.Drawing.Size(73, 17)
        Me.lblTimeUnits.TabIndex = 12
        Me.lblTimeUnits.Text = "Time units"
        Me.lblTimeUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblInstructions
        '
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.Location = New System.Drawing.Point(42, 9)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(192, 17)
        Me.lblInstructions.TabIndex = 13
        Me.lblInstructions.Text = "Set Time Interval for Dataset "
        Me.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(45, 234)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 14
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(150, 234)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(98, 23)
        Me.btnAll.TabIndex = 15
        Me.btnAll.Text = "Use for all"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnSkip
        '
        Me.btnSkip.Location = New System.Drawing.Point(45, 263)
        Me.btnSkip.Name = "btnSkip"
        Me.btnSkip.Size = New System.Drawing.Size(75, 23)
        Me.btnSkip.TabIndex = 16
        Me.btnSkip.Text = "Skip"
        Me.btnSkip.UseVisualStyleBackColor = True
        '
        'btnSkipAll
        '
        Me.btnSkipAll.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSkipAll.Location = New System.Drawing.Point(150, 263)
        Me.btnSkipAll.Name = "btnSkipAll"
        Me.btnSkipAll.Size = New System.Drawing.Size(98, 23)
        Me.btnSkipAll.TabIndex = 17
        Me.btnSkipAll.Text = "Skip All"
        Me.btnSkipAll.UseVisualStyleBackColor = True
        '
        'lstAggregation
        '
        Me.lstAggregation.FormattingEnabled = True
        Me.lstAggregation.ItemHeight = 16
        Me.lstAggregation.Items.AddRange(New Object() {"Average", "Sum", "Minimum", "Maximum", "First", "Last"})
        Me.lstAggregation.Location = New System.Drawing.Point(153, 119)
        Me.lstAggregation.Name = "lstAggregation"
        Me.lstAggregation.Size = New System.Drawing.Size(94, 100)
        Me.lstAggregation.TabIndex = 18
        '
        'lblAggregation
        '
        Me.lblAggregation.AutoSize = True
        Me.lblAggregation.Location = New System.Drawing.Point(62, 119)
        Me.lblAggregation.Name = "lblAggregation"
        Me.lblAggregation.Size = New System.Drawing.Size(85, 17)
        Me.lblAggregation.TabIndex = 19
        Me.lblAggregation.Text = "Aggregation"
        Me.lblAggregation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmDefaultTimeInterval
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSkipAll
        Me.ClientSize = New System.Drawing.Size(291, 302)
        Me.Controls.Add(Me.lblAggregation)
        Me.Controls.Add(Me.lstAggregation)
        Me.Controls.Add(Me.btnSkipAll)
        Me.Controls.Add(Me.btnSkip)
        Me.Controls.Add(Me.btnAll)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblInstructions)
        Me.Controls.Add(Me.lblTimeUnits)
        Me.Controls.Add(Me.lblTimestep)
        Me.Controls.Add(Me.atcTextTimeStep)
        Me.Controls.Add(Me.lstTimeUnits)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmDefaultTimeInterval"
        Me.Text = "Set Missing Time Interval"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTimestep As System.Windows.Forms.Label
    Friend WithEvents atcTextTimeStep As atcControls.atcText
    Friend WithEvents lstTimeUnits As System.Windows.Forms.ListBox
    Friend WithEvents lblTimeUnits As System.Windows.Forms.Label
    Friend WithEvents lblInstructions As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnAll As System.Windows.Forms.Button
    Friend WithEvents btnSkip As System.Windows.Forms.Button
    Friend WithEvents btnSkipAll As System.Windows.Forms.Button
    Friend WithEvents lstAggregation As System.Windows.Forms.ListBox
    Friend WithEvents lblAggregation As System.Windows.Forms.Label
End Class
