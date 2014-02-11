<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MakeEXSFile
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtUCIFilename = New System.Windows.Forms.TextBox
        Me.lblLatLeft = New System.Windows.Forms.Label
        Me.txtLat1 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.lblLat = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.lblLong1 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.lblLong2 = New System.Windows.Forms.Label
        Me.lblSimulationPeriod = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblNoOfSites = New System.Windows.Forms.Label
        Me.cmbNumberOfSites = New System.Windows.Forms.ComboBox
        Me.DTStartDate = New System.Windows.Forms.DateTimePicker
        Me.DTEndDate = New System.Windows.Forms.DateTimePicker
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtReachNumber = New System.Windows.Forms.TextBox
        Me.txtSIMQ = New System.Windows.Forms.TextBox
        Me.lblSIMQ = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtLZSX = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtUZSX = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtSAET = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtPET = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtSUPY = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtAGWO = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtIFWO = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtSURO = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtObsFlow = New System.Windows.Forms.TextBox
        Me.pLocations = New System.Windows.Forms.Panel
        Me.VScrollBar1 = New System.Windows.Forms.VScrollBar
        Me.pLocations.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Prefix of the UCI File"
        '
        'txtUCIFilename
        '
        Me.txtUCIFilename.Location = New System.Drawing.Point(139, 23)
        Me.txtUCIFilename.MaxLength = 8
        Me.txtUCIFilename.Name = "txtUCIFilename"
        Me.txtUCIFilename.Size = New System.Drawing.Size(79, 20)
        Me.txtUCIFilename.TabIndex = 1
        '
        'lblLatLeft
        '
        Me.lblLatLeft.AutoSize = True
        Me.lblLatLeft.Location = New System.Drawing.Point(224, 26)
        Me.lblLatLeft.Name = "lblLatLeft"
        Me.lblLatLeft.Size = New System.Drawing.Size(51, 13)
        Me.lblLatLeft.TabIndex = 2
        Me.lblLatLeft.Text = "Latitude1"
        '
        'txtLat1
        '
        Me.txtLat1.Location = New System.Drawing.Point(281, 23)
        Me.txtLat1.Name = "txtLat1"
        Me.txtLat1.Size = New System.Drawing.Size(65, 20)
        Me.txtLat1.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(409, 23)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(65, 20)
        Me.TextBox1.TabIndex = 5
        '
        'lblLat
        '
        Me.lblLat.AutoSize = True
        Me.lblLat.Location = New System.Drawing.Point(352, 26)
        Me.lblLat.Name = "lblLat"
        Me.lblLat.Size = New System.Drawing.Size(51, 13)
        Me.lblLat.TabIndex = 4
        Me.lblLat.Text = "Latitude2"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(537, 23)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(65, 20)
        Me.TextBox2.TabIndex = 7
        '
        'lblLong1
        '
        Me.lblLong1.AutoSize = True
        Me.lblLong1.Location = New System.Drawing.Point(480, 26)
        Me.lblLong1.Name = "lblLong1"
        Me.lblLong1.Size = New System.Drawing.Size(60, 13)
        Me.lblLong1.TabIndex = 6
        Me.lblLong1.Text = "Longitude1"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(671, 26)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(65, 20)
        Me.TextBox3.TabIndex = 9
        '
        'lblLong2
        '
        Me.lblLong2.AutoSize = True
        Me.lblLong2.Location = New System.Drawing.Point(608, 26)
        Me.lblLong2.Name = "lblLong2"
        Me.lblLong2.Size = New System.Drawing.Size(60, 13)
        Me.lblLong2.TabIndex = 8
        Me.lblLong2.Text = "Longitude2"
        '
        'lblSimulationPeriod
        '
        Me.lblSimulationPeriod.AutoSize = True
        Me.lblSimulationPeriod.Location = New System.Drawing.Point(30, 59)
        Me.lblSimulationPeriod.Name = "lblSimulationPeriod"
        Me.lblSimulationPeriod.Size = New System.Drawing.Size(393, 13)
        Me.lblSimulationPeriod.TabIndex = 10
        Me.lblSimulationPeriod.Text = "You may specify a different period of analysis than the simulation period of UCI " & _
            "file."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Start Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(330, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "End Date"
        '
        'lblNoOfSites
        '
        Me.lblNoOfSites.AutoSize = True
        Me.lblNoOfSites.Location = New System.Drawing.Point(30, 133)
        Me.lblNoOfSites.Name = "lblNoOfSites"
        Me.lblNoOfSites.Size = New System.Drawing.Size(62, 13)
        Me.lblNoOfSites.TabIndex = 15
        Me.lblNoOfSites.Text = "No. of Sites"
        '
        'cmbNumberOfSites
        '
        Me.cmbNumberOfSites.AllowDrop = True
        Me.cmbNumberOfSites.AutoCompleteCustomSource.AddRange(New String() {"1", "2", "3", "4", "5"})
        Me.cmbNumberOfSites.FormatString = "N0"
        Me.cmbNumberOfSites.FormattingEnabled = True
        Me.cmbNumberOfSites.Location = New System.Drawing.Point(102, 133)
        Me.cmbNumberOfSites.Name = "cmbNumberOfSites"
        Me.cmbNumberOfSites.Size = New System.Drawing.Size(121, 21)
        Me.cmbNumberOfSites.TabIndex = 16
        '
        'DTStartDate
        '
        Me.DTStartDate.Location = New System.Drawing.Point(102, 93)
        Me.DTStartDate.Name = "DTStartDate"
        Me.DTStartDate.Size = New System.Drawing.Size(200, 20)
        Me.DTStartDate.TabIndex = 23
        '
        'DTEndDate
        '
        Me.DTEndDate.Location = New System.Drawing.Point(389, 93)
        Me.DTEndDate.Name = "DTEndDate"
        Me.DTEndDate.Size = New System.Drawing.Size(200, 20)
        Me.DTEndDate.TabIndex = 24
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(30, 169)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(293, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Provide the DSN of following Constituents for each Location."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(0, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(130, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Reach Number in UCI File"
        '
        'txtReachNumber
        '
        Me.txtReachNumber.Location = New System.Drawing.Point(12, 25)
        Me.txtReachNumber.Name = "txtReachNumber"
        Me.txtReachNumber.Size = New System.Drawing.Size(100, 20)
        Me.txtReachNumber.TabIndex = 27
        '
        'txtSIMQ
        '
        Me.txtSIMQ.Location = New System.Drawing.Point(136, 25)
        Me.txtSIMQ.Name = "txtSIMQ"
        Me.txtSIMQ.Size = New System.Drawing.Size(49, 20)
        Me.txtSIMQ.TabIndex = 28
        '
        'lblSIMQ
        '
        Me.lblSIMQ.AutoSize = True
        Me.lblSIMQ.Location = New System.Drawing.Point(144, 9)
        Me.lblSIMQ.Name = "lblSIMQ"
        Me.lblSIMQ.Size = New System.Drawing.Size(34, 13)
        Me.lblSIMQ.TabIndex = 29
        Me.lblSIMQ.Text = "SIMQ"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(681, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "LZSX"
        '
        'txtLZSX
        '
        Me.txtLZSX.Location = New System.Drawing.Point(675, 25)
        Me.txtLZSX.Name = "txtLZSX"
        Me.txtLZSX.Size = New System.Drawing.Size(49, 20)
        Me.txtLZSX.TabIndex = 30
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(628, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 13)
        Me.Label7.TabIndex = 33
        Me.Label7.Text = "UZSX"
        '
        'txtUZSX
        '
        Me.txtUZSX.Location = New System.Drawing.Point(620, 25)
        Me.txtUZSX.Name = "txtUZSX"
        Me.txtUZSX.Size = New System.Drawing.Size(49, 20)
        Me.txtUZSX.TabIndex = 32
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(572, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 35
        Me.Label8.Text = "SAET"
        '
        'txtSAET
        '
        Me.txtSAET.Location = New System.Drawing.Point(565, 25)
        Me.txtSAET.Name = "txtSAET"
        Me.txtSAET.Size = New System.Drawing.Size(49, 20)
        Me.txtSAET.TabIndex = 34
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(516, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 37
        Me.Label9.Text = "PETX"
        '
        'txtPET
        '
        Me.txtPET.Location = New System.Drawing.Point(510, 25)
        Me.txtPET.Name = "txtPET"
        Me.txtPET.Size = New System.Drawing.Size(49, 20)
        Me.txtPET.TabIndex = 36
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(462, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(36, 13)
        Me.Label10.TabIndex = 39
        Me.Label10.Text = "SUPY"
        '
        'txtSUPY
        '
        Me.txtSUPY.Location = New System.Drawing.Point(455, 25)
        Me.txtSUPY.Name = "txtSUPY"
        Me.txtSUPY.Size = New System.Drawing.Size(49, 20)
        Me.txtSUPY.TabIndex = 38
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(407, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(41, 13)
        Me.Label11.TabIndex = 41
        Me.Label11.Text = "AGWO"
        '
        'txtAGWO
        '
        Me.txtAGWO.Location = New System.Drawing.Point(400, 25)
        Me.txtAGWO.Name = "txtAGWO"
        Me.txtAGWO.Size = New System.Drawing.Size(49, 20)
        Me.txtAGWO.TabIndex = 40
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(353, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(35, 13)
        Me.Label12.TabIndex = 43
        Me.Label12.Text = "IFWO"
        '
        'txtIFWO
        '
        Me.txtIFWO.Location = New System.Drawing.Point(346, 25)
        Me.txtIFWO.Name = "txtIFWO"
        Me.txtIFWO.Size = New System.Drawing.Size(49, 20)
        Me.txtIFWO.TabIndex = 42
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(298, 9)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(38, 13)
        Me.Label13.TabIndex = 45
        Me.Label13.Text = "SURO"
        '
        'txtSURO
        '
        Me.txtSURO.Location = New System.Drawing.Point(291, 25)
        Me.txtSURO.Name = "txtSURO"
        Me.txtSURO.Size = New System.Drawing.Size(49, 20)
        Me.txtSURO.TabIndex = 44
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(191, 9)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(101, 13)
        Me.Label14.TabIndex = 47
        Me.Label14.Text = "Observed Flow (cfs)"
        '
        'txtObsFlow
        '
        Me.txtObsFlow.Location = New System.Drawing.Point(213, 25)
        Me.txtObsFlow.Name = "txtObsFlow"
        Me.txtObsFlow.Size = New System.Drawing.Size(49, 20)
        Me.txtObsFlow.TabIndex = 46
        '
        'pLocations
        '
        Me.pLocations.Controls.Add(Me.VScrollBar1)
        Me.pLocations.Controls.Add(Me.Label14)
        Me.pLocations.Controls.Add(Me.Label4)
        Me.pLocations.Controls.Add(Me.txtObsFlow)
        Me.pLocations.Controls.Add(Me.txtReachNumber)
        Me.pLocations.Controls.Add(Me.Label13)
        Me.pLocations.Controls.Add(Me.txtSIMQ)
        Me.pLocations.Controls.Add(Me.txtSURO)
        Me.pLocations.Controls.Add(Me.lblSIMQ)
        Me.pLocations.Controls.Add(Me.Label12)
        Me.pLocations.Controls.Add(Me.txtLZSX)
        Me.pLocations.Controls.Add(Me.txtIFWO)
        Me.pLocations.Controls.Add(Me.Label6)
        Me.pLocations.Controls.Add(Me.Label11)
        Me.pLocations.Controls.Add(Me.txtUZSX)
        Me.pLocations.Controls.Add(Me.txtAGWO)
        Me.pLocations.Controls.Add(Me.Label7)
        Me.pLocations.Controls.Add(Me.Label10)
        Me.pLocations.Controls.Add(Me.txtSAET)
        Me.pLocations.Controls.Add(Me.txtSUPY)
        Me.pLocations.Controls.Add(Me.Label8)
        Me.pLocations.Controls.Add(Me.Label9)
        Me.pLocations.Controls.Add(Me.txtPET)
        Me.pLocations.Location = New System.Drawing.Point(33, 198)
        Me.pLocations.Name = "pLocations"
        Me.pLocations.Size = New System.Drawing.Size(772, 57)
        Me.pLocations.TabIndex = 48
        '
        'VScrollBar1
        '
        Me.VScrollBar1.Location = New System.Drawing.Point(745, 0)
        Me.VScrollBar1.Name = "VScrollBar1"
        Me.VScrollBar1.Size = New System.Drawing.Size(27, 57)
        Me.VScrollBar1.TabIndex = 48
        '
        'MakeEXSFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(817, 525)
        Me.Controls.Add(Me.pLocations)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.DTEndDate)
        Me.Controls.Add(Me.DTStartDate)
        Me.Controls.Add(Me.cmbNumberOfSites)
        Me.Controls.Add(Me.lblNoOfSites)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblSimulationPeriod)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.lblLong2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.lblLong1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.lblLat)
        Me.Controls.Add(Me.txtLat1)
        Me.Controls.Add(Me.lblLatLeft)
        Me.Controls.Add(Me.txtUCIFilename)
        Me.Controls.Add(Me.Label1)
        Me.Name = "MakeEXSFile"
        Me.Text = "MakeEXSFile"
        Me.pLocations.ResumeLayout(False)
        Me.pLocations.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUCIFilename As System.Windows.Forms.TextBox
    Friend WithEvents lblLatLeft As System.Windows.Forms.Label
    Friend WithEvents txtLat1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lblLat As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents lblLong1 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents lblLong2 As System.Windows.Forms.Label
    Friend WithEvents lblSimulationPeriod As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblNoOfSites As System.Windows.Forms.Label
    Friend WithEvents cmbNumberOfSites As System.Windows.Forms.ComboBox
    Friend WithEvents DTStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents DTEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtReachNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtSIMQ As System.Windows.Forms.TextBox
    Friend WithEvents lblSIMQ As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtLZSX As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtUZSX As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtSAET As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPET As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSUPY As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtAGWO As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtIFWO As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtSURO As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtObsFlow As System.Windows.Forms.TextBox
    Friend WithEvents pLocations As System.Windows.Forms.Panel
    Friend WithEvents VScrollBar1 As System.Windows.Forms.VScrollBar
End Class
