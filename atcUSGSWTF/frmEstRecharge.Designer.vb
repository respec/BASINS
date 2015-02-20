<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEstRecharge
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEstRecharge))
        Me.txtSy = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkAntMethodFall = New System.Windows.Forms.CheckBox
        Me.chkAntMethodLinear = New System.Windows.Forms.CheckBox
        Me.chkAntMethodPower = New System.Windows.Forms.CheckBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.gbFall = New System.Windows.Forms.GroupBox
        Me.lblSemiLogEqu = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtFallKgw = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtFallD = New System.Windows.Forms.TextBox
        Me.gbLinear = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtLinearB = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtLinearA = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.gbPower = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPowerExp = New System.Windows.Forms.TextBox
        Me.txtPowerDatum = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtPowerMultiplier = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtPowerIntercept = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.gbDates = New System.Windows.Forms.GroupBox
        Me.lblAnalysisDates = New System.Windows.Forms.Label
        Me.lblPeriodOfRecord = New System.Windows.Forms.Label
        Me.txtEndDateUser = New System.Windows.Forms.TextBox
        Me.txtStartDateUser = New System.Windows.Forms.TextBox
        Me.btnExamineData = New System.Windows.Forms.Button
        Me.txtDataEnd = New System.Windows.Forms.TextBox
        Me.txtDataStart = New System.Windows.Forms.TextBox
        Me.lblDataEnd = New System.Windows.Forms.Label
        Me.lblDataStart = New System.Windows.Forms.Label
        Me.gbTextOutput = New System.Windows.Forms.GroupBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnWriteASCIIOutput = New System.Windows.Forms.Button
        Me.txtOutputDir = New System.Windows.Forms.TextBox
        Me.lblOutputDir = New System.Windows.Forms.Label
        Me.txtOutputRootName = New System.Windows.Forms.TextBox
        Me.lblBaseFilename = New System.Windows.Forms.Label
        Me.gbFall.SuspendLayout()
        Me.gbLinear.SuspendLayout()
        Me.gbPower.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.gbDates.SuspendLayout()
        Me.gbTextOutput.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSy
        '
        Me.txtSy.Location = New System.Drawing.Point(12, 171)
        Me.txtSy.Name = "txtSy"
        Me.txtSy.Size = New System.Drawing.Size(96, 20)
        Me.txtSy.TabIndex = 6
        Me.txtSy.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 142)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 26)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Aquifer Specific Yield:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(0 ~ 1)"
        Me.Label1.Visible = False
        '
        'chkAntMethodFall
        '
        Me.chkAntMethodFall.AutoSize = True
        Me.chkAntMethodFall.Location = New System.Drawing.Point(12, 28)
        Me.chkAntMethodFall.Name = "chkAntMethodFall"
        Me.chkAntMethodFall.Size = New System.Drawing.Size(96, 17)
        Me.chkAntMethodFall.TabIndex = 10
        Me.chkAntMethodFall.Text = "Semi-logrithmic"
        Me.chkAntMethodFall.UseVisualStyleBackColor = True
        '
        'chkAntMethodLinear
        '
        Me.chkAntMethodLinear.AutoSize = True
        Me.chkAntMethodLinear.Location = New System.Drawing.Point(12, 51)
        Me.chkAntMethodLinear.Name = "chkAntMethodLinear"
        Me.chkAntMethodLinear.Size = New System.Drawing.Size(55, 17)
        Me.chkAntMethodLinear.TabIndex = 11
        Me.chkAntMethodLinear.Text = "Linear"
        Me.chkAntMethodLinear.UseVisualStyleBackColor = True
        '
        'chkAntMethodPower
        '
        Me.chkAntMethodPower.AutoSize = True
        Me.chkAntMethodPower.Location = New System.Drawing.Point(12, 74)
        Me.chkAntMethodPower.Name = "chkAntMethodPower"
        Me.chkAntMethodPower.Size = New System.Drawing.Size(100, 17)
        Me.chkAntMethodPower.TabIndex = 12
        Me.chkAntMethodPower.Text = "Power Function"
        Me.chkAntMethodPower.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(255, 13)
        Me.lblTitle.TabIndex = 14
        Me.lblTitle.Text = "Select Methods of Antecedent Recession Estimation"
        '
        'gbFall
        '
        Me.gbFall.Controls.Add(Me.lblSemiLogEqu)
        Me.gbFall.Controls.Add(Me.Label2)
        Me.gbFall.Controls.Add(Me.txtFallKgw)
        Me.gbFall.Controls.Add(Me.Label3)
        Me.gbFall.Controls.Add(Me.txtFallD)
        Me.gbFall.Location = New System.Drawing.Point(0, 0)
        Me.gbFall.Name = "gbFall"
        Me.gbFall.Size = New System.Drawing.Size(238, 114)
        Me.gbFall.TabIndex = 16
        Me.gbFall.TabStop = False
        Me.gbFall.Text = "Semi-logrithmic Method"
        '
        'lblSemiLogEqu
        '
        Me.lblSemiLogEqu.AutoSize = True
        Me.lblSemiLogEqu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSemiLogEqu.Location = New System.Drawing.Point(6, 16)
        Me.lblSemiLogEqu.Name = "lblSemiLogEqu"
        Me.lblSemiLogEqu.Size = New System.Drawing.Size(226, 15)
        Me.lblSemiLogEqu.TabIndex = 9
        Me.lblSemiLogEqu.Text = "Ant.GWL = (H0 - d) * e^(Kgw(Tpeak - T0)) + d"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 39)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "                         Kgw," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "GWL recession index " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       (ln(GWL-d)/day)"
        '
        'txtFallKgw
        '
        Me.txtFallKgw.Location = New System.Drawing.Point(149, 82)
        Me.txtFallKgw.Name = "txtFallKgw"
        Me.txtFallKgw.Size = New System.Drawing.Size(83, 20)
        Me.txtFallKgw.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "d, asymptotic GWL (ft):"
        '
        'txtFallD
        '
        Me.txtFallD.Location = New System.Drawing.Point(150, 37)
        Me.txtFallD.Name = "txtFallD"
        Me.txtFallD.Size = New System.Drawing.Size(83, 20)
        Me.txtFallD.TabIndex = 1
        '
        'gbLinear
        '
        Me.gbLinear.Controls.Add(Me.Label10)
        Me.gbLinear.Controls.Add(Me.txtLinearB)
        Me.gbLinear.Controls.Add(Me.Label4)
        Me.gbLinear.Controls.Add(Me.txtLinearA)
        Me.gbLinear.Controls.Add(Me.Label5)
        Me.gbLinear.Location = New System.Drawing.Point(4, 4)
        Me.gbLinear.Name = "gbLinear"
        Me.gbLinear.Size = New System.Drawing.Size(238, 115)
        Me.gbLinear.TabIndex = 17
        Me.gbLinear.TabStop = False
        Me.gbLinear.Text = "Linear Model Parameters"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label10.Location = New System.Drawing.Point(6, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(176, 15)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Decline rate = dZwt/dt = a(Zwt) + b"
        '
        'txtLinearB
        '
        Me.txtLinearB.Location = New System.Drawing.Point(146, 65)
        Me.txtLinearB.Name = "txtLinearB"
        Me.txtLinearB.Size = New System.Drawing.Size(86, 20)
        Me.txtLinearB.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Rate Decline Intercept, b:"
        '
        'txtLinearA
        '
        Me.txtLinearA.Location = New System.Drawing.Point(146, 39)
        Me.txtLinearA.Name = "txtLinearA"
        Me.txtLinearA.Size = New System.Drawing.Size(86, 20)
        Me.txtLinearA.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(114, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Rate Decline Slope, a:"
        '
        'gbPower
        '
        Me.gbPower.Controls.Add(Me.Label9)
        Me.gbPower.Controls.Add(Me.Label8)
        Me.gbPower.Controls.Add(Me.gbFall)
        Me.gbPower.Controls.Add(Me.txtPowerExp)
        Me.gbPower.Controls.Add(Me.txtPowerDatum)
        Me.gbPower.Controls.Add(Me.Label7)
        Me.gbPower.Controls.Add(Me.txtPowerMultiplier)
        Me.gbPower.Controls.Add(Me.Label6)
        Me.gbPower.Controls.Add(Me.txtPowerIntercept)
        Me.gbPower.Controls.Add(Me.Label11)
        Me.gbPower.Location = New System.Drawing.Point(4, 9)
        Me.gbPower.Name = "gbPower"
        Me.gbPower.Size = New System.Drawing.Size(238, 150)
        Me.gbPower.TabIndex = 18
        Me.gbPower.TabStop = False
        Me.gbPower.Text = "Power Model Parameters"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label9.Location = New System.Drawing.Point(6, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(209, 15)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Decline rate = dZwt/dt = -(c + d(Zwt - e)^f)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Exponent, f:"
        '
        'txtPowerExp
        '
        Me.txtPowerExp.Location = New System.Drawing.Point(90, 121)
        Me.txtPowerExp.Name = "txtPowerExp"
        Me.txtPowerExp.Size = New System.Drawing.Size(142, 20)
        Me.txtPowerExp.TabIndex = 6
        '
        'txtPowerDatum
        '
        Me.txtPowerDatum.Location = New System.Drawing.Point(90, 94)
        Me.txtPowerDatum.Name = "txtPowerDatum"
        Me.txtPowerDatum.Size = New System.Drawing.Size(142, 20)
        Me.txtPowerDatum.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 97)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(53, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Datum, e:"
        '
        'txtPowerMultiplier
        '
        Me.txtPowerMultiplier.Location = New System.Drawing.Point(90, 68)
        Me.txtPowerMultiplier.Name = "txtPowerMultiplier"
        Me.txtPowerMultiplier.Size = New System.Drawing.Size(142, 20)
        Me.txtPowerMultiplier.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 71)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Multiplier, d:"
        '
        'txtPowerIntercept
        '
        Me.txtPowerIntercept.Location = New System.Drawing.Point(90, 42)
        Me.txtPowerIntercept.Name = "txtPowerIntercept"
        Me.txtPowerIntercept.Size = New System.Drawing.Size(142, 20)
        Me.txtPowerIntercept.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 45)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Intercept, c:"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gbPower)
        Me.Panel1.Controls.Add(Me.gbLinear)
        Me.Panel1.Location = New System.Drawing.Point(135, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(248, 163)
        Me.Panel1.TabIndex = 19
        '
        'gbDates
        '
        Me.gbDates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDates.Controls.Add(Me.lblAnalysisDates)
        Me.gbDates.Controls.Add(Me.lblPeriodOfRecord)
        Me.gbDates.Controls.Add(Me.txtEndDateUser)
        Me.gbDates.Controls.Add(Me.txtStartDateUser)
        Me.gbDates.Controls.Add(Me.btnExamineData)
        Me.gbDates.Controls.Add(Me.txtDataEnd)
        Me.gbDates.Controls.Add(Me.txtDataStart)
        Me.gbDates.Controls.Add(Me.lblDataEnd)
        Me.gbDates.Controls.Add(Me.lblDataStart)
        Me.gbDates.Location = New System.Drawing.Point(3, 206)
        Me.gbDates.Name = "gbDates"
        Me.gbDates.Size = New System.Drawing.Size(375, 114)
        Me.gbDates.TabIndex = 20
        Me.gbDates.TabStop = False
        Me.gbDates.Text = "Define Analysis Dates"
        '
        'lblAnalysisDates
        '
        Me.lblAnalysisDates.AutoSize = True
        Me.lblAnalysisDates.Location = New System.Drawing.Point(191, 15)
        Me.lblAnalysisDates.Name = "lblAnalysisDates"
        Me.lblAnalysisDates.Size = New System.Drawing.Size(76, 13)
        Me.lblAnalysisDates.TabIndex = 12
        Me.lblAnalysisDates.Text = "Analysis Dates"
        '
        'lblPeriodOfRecord
        '
        Me.lblPeriodOfRecord.AutoSize = True
        Me.lblPeriodOfRecord.Location = New System.Drawing.Point(65, 15)
        Me.lblPeriodOfRecord.Name = "lblPeriodOfRecord"
        Me.lblPeriodOfRecord.Size = New System.Drawing.Size(87, 13)
        Me.lblPeriodOfRecord.TabIndex = 11
        Me.lblPeriodOfRecord.Text = "Period of Record"
        '
        'txtEndDateUser
        '
        Me.txtEndDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDateUser.Location = New System.Drawing.Point(194, 58)
        Me.txtEndDateUser.Name = "txtEndDateUser"
        Me.txtEndDateUser.Size = New System.Drawing.Size(175, 20)
        Me.txtEndDateUser.TabIndex = 9
        '
        'txtStartDateUser
        '
        Me.txtStartDateUser.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDateUser.Location = New System.Drawing.Point(194, 31)
        Me.txtStartDateUser.Name = "txtStartDateUser"
        Me.txtStartDateUser.Size = New System.Drawing.Size(175, 20)
        Me.txtStartDateUser.TabIndex = 8
        '
        'btnExamineData
        '
        Me.btnExamineData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExamineData.Location = New System.Drawing.Point(68, 85)
        Me.btnExamineData.Name = "btnExamineData"
        Me.btnExamineData.Size = New System.Drawing.Size(94, 23)
        Me.btnExamineData.TabIndex = 10
        Me.btnExamineData.Text = "Examine Data"
        Me.btnExamineData.UseVisualStyleBackColor = True
        '
        'txtDataEnd
        '
        Me.txtDataEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataEnd.Location = New System.Drawing.Point(68, 59)
        Me.txtDataEnd.Name = "txtDataEnd"
        Me.txtDataEnd.ReadOnly = True
        Me.txtDataEnd.Size = New System.Drawing.Size(119, 20)
        Me.txtDataEnd.TabIndex = 3
        '
        'txtDataStart
        '
        Me.txtDataStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataStart.Location = New System.Drawing.Point(68, 32)
        Me.txtDataStart.Name = "txtDataStart"
        Me.txtDataStart.ReadOnly = True
        Me.txtDataStart.Size = New System.Drawing.Size(119, 20)
        Me.txtDataStart.TabIndex = 2
        '
        'lblDataEnd
        '
        Me.lblDataEnd.AutoSize = True
        Me.lblDataEnd.Location = New System.Drawing.Point(6, 58)
        Me.lblDataEnd.Name = "lblDataEnd"
        Me.lblDataEnd.Size = New System.Drawing.Size(52, 13)
        Me.lblDataEnd.TabIndex = 1
        Me.lblDataEnd.Text = "Data End"
        '
        'lblDataStart
        '
        Me.lblDataStart.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDataStart.AutoSize = True
        Me.lblDataStart.Location = New System.Drawing.Point(6, 32)
        Me.lblDataStart.Name = "lblDataStart"
        Me.lblDataStart.Size = New System.Drawing.Size(55, 13)
        Me.lblDataStart.TabIndex = 0
        Me.lblDataStart.Text = "Data Start"
        '
        'gbTextOutput
        '
        Me.gbTextOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextOutput.Controls.Add(Me.btnSave)
        Me.gbTextOutput.Controls.Add(Me.btnWriteASCIIOutput)
        Me.gbTextOutput.Controls.Add(Me.txtOutputDir)
        Me.gbTextOutput.Controls.Add(Me.lblOutputDir)
        Me.gbTextOutput.Controls.Add(Me.txtOutputRootName)
        Me.gbTextOutput.Controls.Add(Me.lblBaseFilename)
        Me.gbTextOutput.Location = New System.Drawing.Point(3, 326)
        Me.gbTextOutput.Name = "gbTextOutput"
        Me.gbTextOutput.Size = New System.Drawing.Size(375, 111)
        Me.gbTextOutput.TabIndex = 21
        Me.gbTextOutput.TabStop = False
        Me.gbTextOutput.Text = "Text Output"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(125, 82)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(127, 23)
        Me.btnSave.TabIndex = 32
        Me.btnSave.Text = "Plot Monthly Recharge"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnWriteASCIIOutput
        '
        Me.btnWriteASCIIOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWriteASCIIOutput.Location = New System.Drawing.Point(9, 82)
        Me.btnWriteASCIIOutput.Name = "btnWriteASCIIOutput"
        Me.btnWriteASCIIOutput.Size = New System.Drawing.Size(110, 23)
        Me.btnWriteASCIIOutput.TabIndex = 15
        Me.btnWriteASCIIOutput.Text = "Calculate Recharge"
        Me.btnWriteASCIIOutput.UseVisualStyleBackColor = True
        '
        'txtOutputDir
        '
        Me.txtOutputDir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutputDir.Location = New System.Drawing.Point(78, 20)
        Me.txtOutputDir.Name = "txtOutputDir"
        Me.txtOutputDir.Size = New System.Drawing.Size(291, 20)
        Me.txtOutputDir.TabIndex = 12
        '
        'lblOutputDir
        '
        Me.lblOutputDir.AutoSize = True
        Me.lblOutputDir.Location = New System.Drawing.Point(8, 23)
        Me.lblOutputDir.Name = "lblOutputDir"
        Me.lblOutputDir.Size = New System.Drawing.Size(68, 13)
        Me.lblOutputDir.TabIndex = 31
        Me.lblOutputDir.Text = "Output folder"
        '
        'txtOutputRootName
        '
        Me.txtOutputRootName.Location = New System.Drawing.Point(114, 49)
        Me.txtOutputRootName.Name = "txtOutputRootName"
        Me.txtOutputRootName.Size = New System.Drawing.Size(121, 20)
        Me.txtOutputRootName.TabIndex = 13
        '
        'lblBaseFilename
        '
        Me.lblBaseFilename.AutoSize = True
        Me.lblBaseFilename.Location = New System.Drawing.Point(6, 52)
        Me.lblBaseFilename.Name = "lblBaseFilename"
        Me.lblBaseFilename.Size = New System.Drawing.Size(106, 13)
        Me.lblBaseFilename.TabIndex = 30
        Me.lblBaseFilename.Text = "Base output filename"
        '
        'frmEstRecharge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 445)
        Me.Controls.Add(Me.gbDates)
        Me.Controls.Add(Me.gbTextOutput)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.chkAntMethodFall)
        Me.Controls.Add(Me.chkAntMethodLinear)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.chkAntMethodPower)
        Me.Controls.Add(Me.txtSy)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmEstRecharge"
        Me.Text = "WTF Estimate Recharge"
        Me.gbFall.ResumeLayout(False)
        Me.gbFall.PerformLayout()
        Me.gbLinear.ResumeLayout(False)
        Me.gbLinear.PerformLayout()
        Me.gbPower.ResumeLayout(False)
        Me.gbPower.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.gbDates.ResumeLayout(False)
        Me.gbDates.PerformLayout()
        Me.gbTextOutput.ResumeLayout(False)
        Me.gbTextOutput.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSy As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAntMethodFall As System.Windows.Forms.CheckBox
    Friend WithEvents chkAntMethodLinear As System.Windows.Forms.CheckBox
    Friend WithEvents chkAntMethodPower As System.Windows.Forms.CheckBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gbFall As System.Windows.Forms.GroupBox
    Friend WithEvents lblSemiLogEqu As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFallKgw As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFallD As System.Windows.Forms.TextBox
    Friend WithEvents gbLinear As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtLinearB As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtLinearA As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents gbPower As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPowerExp As System.Windows.Forms.TextBox
    Friend WithEvents txtPowerDatum As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtPowerMultiplier As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPowerIntercept As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gbDates As System.Windows.Forms.GroupBox
    Friend WithEvents lblAnalysisDates As System.Windows.Forms.Label
    Friend WithEvents lblPeriodOfRecord As System.Windows.Forms.Label
    Friend WithEvents txtEndDateUser As System.Windows.Forms.TextBox
    Friend WithEvents txtStartDateUser As System.Windows.Forms.TextBox
    Friend WithEvents btnExamineData As System.Windows.Forms.Button
    Friend WithEvents txtDataEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtDataStart As System.Windows.Forms.TextBox
    Friend WithEvents lblDataEnd As System.Windows.Forms.Label
    Friend WithEvents lblDataStart As System.Windows.Forms.Label
    Friend WithEvents gbTextOutput As System.Windows.Forms.GroupBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnWriteASCIIOutput As System.Windows.Forms.Button
    Friend WithEvents txtOutputDir As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputDir As System.Windows.Forms.Label
    Friend WithEvents txtOutputRootName As System.Windows.Forms.TextBox
    Friend WithEvents lblBaseFilename As System.Windows.Forms.Label
End Class
