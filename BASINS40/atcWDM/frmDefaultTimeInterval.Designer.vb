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
        Me.lblTimestep.Location = New System.Drawing.Point(23, 31)
        Me.lblTimestep.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTimestep.Name = "lblTimestep"
        Me.lblTimestep.Size = New System.Drawing.Size(53, 13)
        Me.lblTimestep.TabIndex = 0
        Me.lblTimestep.Text = "Time step"
        Me.lblTimestep.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'atcTextTimeStep
        '
        Me.atcTextTimeStep.Alignment = System.Windows.Forms.HorizontalAlignment.Right
        Me.atcTextTimeStep.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.atcTextTimeStep.DefaultValue = "1"
        Me.atcTextTimeStep.HardMax = 60
        Me.atcTextTimeStep.HardMin = 1
        Me.atcTextTimeStep.InsideLimitsBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.Location = New System.Drawing.Point(91, 28)
        Me.atcTextTimeStep.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.atcTextTimeStep.MaxWidth = 0
        Me.atcTextTimeStep.Name = "atcTextTimeStep"
        Me.atcTextTimeStep.NumericFormat = "0.#####"
        Me.atcTextTimeStep.OutsideHardLimitBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.OutsideSoftLimitBackground = System.Drawing.Color.Empty
        Me.atcTextTimeStep.SelLength = 1
        Me.atcTextTimeStep.SelStart = 0
        Me.atcTextTimeStep.Size = New System.Drawing.Size(42, 16)
        Me.atcTextTimeStep.SoftMax = 0
        Me.atcTextTimeStep.SoftMin = 0
        Me.atcTextTimeStep.TabIndex = 1
        Me.atcTextTimeStep.TabStop = False
        Me.atcTextTimeStep.ValueDouble = 1
        Me.atcTextTimeStep.ValueInteger = 1
        '
        'lstTimeUnits
        '
        Me.lstTimeUnits.ColumnWidth = 10
        Me.lstTimeUnits.FormattingEnabled = True
        Me.lstTimeUnits.Items.AddRange(New Object() {"Minutes", "Hours", "Days", "Months"})
        Me.lstTimeUnits.Location = New System.Drawing.Point(91, 48)
        Me.lstTimeUnits.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.lstTimeUnits.Name = "lstTimeUnits"
        Me.lstTimeUnits.Size = New System.Drawing.Size(74, 56)
        Me.lstTimeUnits.TabIndex = 3
        '
        'lblTimeUnits
        '
        Me.lblTimeUnits.AutoSize = True
        Me.lblTimeUnits.Location = New System.Drawing.Point(23, 48)
        Me.lblTimeUnits.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTimeUnits.Name = "lblTimeUnits"
        Me.lblTimeUnits.Size = New System.Drawing.Size(55, 13)
        Me.lblTimeUnits.TabIndex = 2
        Me.lblTimeUnits.Text = "Time units"
        Me.lblTimeUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblInstructions
        '
        Me.lblInstructions.AutoSize = True
        Me.lblInstructions.Location = New System.Drawing.Point(11, 9)
        Me.lblInstructions.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(145, 13)
        Me.lblInstructions.TabIndex = 13
        Me.lblInstructions.Text = "Set Time Interval for Dataset "
        Me.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(11, 209)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnAll
        '
        Me.btnAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAll.Location = New System.Drawing.Point(101, 209)
        Me.btnAll.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(75, 23)
        Me.btnAll.TabIndex = 7
        Me.btnAll.Text = "Ok for All"
        Me.btnAll.UseVisualStyleBackColor = True
        '
        'btnSkip
        '
        Me.btnSkip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSkip.Location = New System.Drawing.Point(191, 209)
        Me.btnSkip.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnSkip.Name = "btnSkip"
        Me.btnSkip.Size = New System.Drawing.Size(75, 23)
        Me.btnSkip.TabIndex = 8
        Me.btnSkip.Text = "Skip Adding"
        Me.btnSkip.UseVisualStyleBackColor = True
        '
        'btnSkipAll
        '
        Me.btnSkipAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSkipAll.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSkipAll.Location = New System.Drawing.Point(281, 209)
        Me.btnSkipAll.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnSkipAll.Name = "btnSkipAll"
        Me.btnSkipAll.Size = New System.Drawing.Size(75, 23)
        Me.btnSkipAll.TabIndex = 9
        Me.btnSkipAll.Text = "Skip All"
        Me.btnSkipAll.UseVisualStyleBackColor = True
        '
        'lstAggregation
        '
        Me.lstAggregation.FormattingEnabled = True
        Me.lstAggregation.Items.AddRange(New Object() {"Average", "Sum", "Minimum", "Maximum", "First", "Last"})
        Me.lstAggregation.Location = New System.Drawing.Point(91, 108)
        Me.lstAggregation.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.lstAggregation.Name = "lstAggregation"
        Me.lstAggregation.Size = New System.Drawing.Size(72, 82)
        Me.lstAggregation.TabIndex = 5
        '
        'lblAggregation
        '
        Me.lblAggregation.AutoSize = True
        Me.lblAggregation.Location = New System.Drawing.Point(23, 108)
        Me.lblAggregation.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblAggregation.Name = "lblAggregation"
        Me.lblAggregation.Size = New System.Drawing.Size(64, 13)
        Me.lblAggregation.TabIndex = 4
        Me.lblAggregation.Text = "Aggregation"
        Me.lblAggregation.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmDefaultTimeInterval
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSkipAll
        Me.ClientSize = New System.Drawing.Size(367, 243)
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
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
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
