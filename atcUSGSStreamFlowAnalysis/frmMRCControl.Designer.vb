<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMRCControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMRCControl))
        Me.tabMRCMain = New System.Windows.Forms.TabControl
        Me.tabConstructMRC = New System.Windows.Forms.TabPage
        Me.gbMRCEquations = New System.Windows.Forms.GroupBox
        Me.txtSeason = New System.Windows.Forms.TextBox
        Me.lblSeason = New System.Windows.Forms.Label
        Me.txtLogQMax = New System.Windows.Forms.TextBox
        Me.lblMaxLogQ = New System.Windows.Forms.Label
        Me.txtLogQMin = New System.Windows.Forms.TextBox
        Me.lblMinLogQ = New System.Windows.Forms.Label
        Me.txtStation = New System.Windows.Forms.TextBox
        Me.lblStation = New System.Windows.Forms.Label
        Me.chkAutoMatching = New System.Windows.Forms.CheckBox
        Me.btnMRCPlot = New System.Windows.Forms.Button
        Me.lstEquations = New System.Windows.Forms.CheckedListBox
        Me.btnMRCAdd = New System.Windows.Forms.Button
        Me.btnMRCDelete = New System.Windows.Forms.Button
        Me.txtDA = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtCoefA = New System.Windows.Forms.TextBox
        Me.txtCoefB = New System.Windows.Forms.TextBox
        Me.txtCoefC = New System.Windows.Forms.TextBox
        Me.gbDA = New System.Windows.Forms.GroupBox
        Me.lstStations = New System.Windows.Forms.ListBox
        Me.btnStations = New System.Windows.Forms.Button
        Me.gbRecSum = New System.Windows.Forms.GroupBox
        Me.btnRecSum = New System.Windows.Forms.Button
        Me.lstRecSum = New System.Windows.Forms.ListBox
        Me.tabPlotMRC = New System.Windows.Forms.TabPage
        Me.panelGraph = New System.Windows.Forms.Panel
        Me.tabMRCTable = New System.Windows.Forms.TabPage
        Me.txtMRCTable = New System.Windows.Forms.TextBox
        Me.tabMRCMain.SuspendLayout()
        Me.tabConstructMRC.SuspendLayout()
        Me.gbMRCEquations.SuspendLayout()
        Me.gbDA.SuspendLayout()
        Me.gbRecSum.SuspendLayout()
        Me.tabPlotMRC.SuspendLayout()
        Me.tabMRCTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMRCMain
        '
        Me.tabMRCMain.Controls.Add(Me.tabConstructMRC)
        Me.tabMRCMain.Controls.Add(Me.tabPlotMRC)
        Me.tabMRCMain.Controls.Add(Me.tabMRCTable)
        Me.tabMRCMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMRCMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMRCMain.Name = "tabMRCMain"
        Me.tabMRCMain.SelectedIndex = 0
        Me.tabMRCMain.Size = New System.Drawing.Size(660, 466)
        Me.tabMRCMain.TabIndex = 0
        '
        'tabConstructMRC
        '
        Me.tabConstructMRC.Controls.Add(Me.gbMRCEquations)
        Me.tabConstructMRC.Controls.Add(Me.gbDA)
        Me.tabConstructMRC.Controls.Add(Me.gbRecSum)
        Me.tabConstructMRC.Location = New System.Drawing.Point(4, 22)
        Me.tabConstructMRC.Name = "tabConstructMRC"
        Me.tabConstructMRC.Padding = New System.Windows.Forms.Padding(3)
        Me.tabConstructMRC.Size = New System.Drawing.Size(652, 440)
        Me.tabConstructMRC.TabIndex = 0
        Me.tabConstructMRC.Text = "Construct MRC"
        Me.tabConstructMRC.UseVisualStyleBackColor = True
        '
        'gbMRCEquations
        '
        Me.gbMRCEquations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMRCEquations.Controls.Add(Me.txtSeason)
        Me.gbMRCEquations.Controls.Add(Me.lblSeason)
        Me.gbMRCEquations.Controls.Add(Me.txtLogQMax)
        Me.gbMRCEquations.Controls.Add(Me.lblMaxLogQ)
        Me.gbMRCEquations.Controls.Add(Me.txtLogQMin)
        Me.gbMRCEquations.Controls.Add(Me.lblMinLogQ)
        Me.gbMRCEquations.Controls.Add(Me.txtStation)
        Me.gbMRCEquations.Controls.Add(Me.lblStation)
        Me.gbMRCEquations.Controls.Add(Me.chkAutoMatching)
        Me.gbMRCEquations.Controls.Add(Me.btnMRCPlot)
        Me.gbMRCEquations.Controls.Add(Me.lstEquations)
        Me.gbMRCEquations.Controls.Add(Me.btnMRCAdd)
        Me.gbMRCEquations.Controls.Add(Me.btnMRCDelete)
        Me.gbMRCEquations.Controls.Add(Me.txtDA)
        Me.gbMRCEquations.Controls.Add(Me.Label1)
        Me.gbMRCEquations.Controls.Add(Me.Label2)
        Me.gbMRCEquations.Controls.Add(Me.Label3)
        Me.gbMRCEquations.Controls.Add(Me.Label4)
        Me.gbMRCEquations.Controls.Add(Me.txtCoefA)
        Me.gbMRCEquations.Controls.Add(Me.txtCoefB)
        Me.gbMRCEquations.Controls.Add(Me.txtCoefC)
        Me.gbMRCEquations.Location = New System.Drawing.Point(8, 276)
        Me.gbMRCEquations.Name = "gbMRCEquations"
        Me.gbMRCEquations.Size = New System.Drawing.Size(638, 158)
        Me.gbMRCEquations.TabIndex = 37
        Me.gbMRCEquations.TabStop = False
        Me.gbMRCEquations.Text = "MRC Polynomial Equations (T= Coeff.A * LogQ^2 + Coeff.B * LogQ + Coeff.C)"
        '
        'txtSeason
        '
        Me.txtSeason.Location = New System.Drawing.Point(488, 131)
        Me.txtSeason.Name = "txtSeason"
        Me.txtSeason.Size = New System.Drawing.Size(20, 20)
        Me.txtSeason.TabIndex = 32
        '
        'lblSeason
        '
        Me.lblSeason.AutoSize = True
        Me.lblSeason.Location = New System.Drawing.Point(485, 115)
        Me.lblSeason.Name = "lblSeason"
        Me.lblSeason.Size = New System.Drawing.Size(43, 13)
        Me.lblSeason.TabIndex = 31
        Me.lblSeason.Text = "Season"
        '
        'txtLogQMax
        '
        Me.txtLogQMax.Location = New System.Drawing.Point(224, 131)
        Me.txtLogQMax.Name = "txtLogQMax"
        Me.txtLogQMax.Size = New System.Drawing.Size(60, 20)
        Me.txtLogQMax.TabIndex = 30
        '
        'lblMaxLogQ
        '
        Me.lblMaxLogQ.AutoSize = True
        Me.lblMaxLogQ.Location = New System.Drawing.Point(221, 115)
        Me.lblMaxLogQ.Name = "lblMaxLogQ"
        Me.lblMaxLogQ.Size = New System.Drawing.Size(53, 13)
        Me.lblMaxLogQ.TabIndex = 29
        Me.lblMaxLogQ.Text = "MaxLogQ"
        '
        'txtLogQMin
        '
        Me.txtLogQMin.Location = New System.Drawing.Point(158, 131)
        Me.txtLogQMin.Name = "txtLogQMin"
        Me.txtLogQMin.Size = New System.Drawing.Size(60, 20)
        Me.txtLogQMin.TabIndex = 28
        '
        'lblMinLogQ
        '
        Me.lblMinLogQ.AutoSize = True
        Me.lblMinLogQ.Location = New System.Drawing.Point(155, 116)
        Me.lblMinLogQ.Name = "lblMinLogQ"
        Me.lblMinLogQ.Size = New System.Drawing.Size(50, 13)
        Me.lblMinLogQ.TabIndex = 27
        Me.lblMinLogQ.Text = "MinLogQ"
        '
        'txtStation
        '
        Me.txtStation.Location = New System.Drawing.Point(6, 131)
        Me.txtStation.Name = "txtStation"
        Me.txtStation.Size = New System.Drawing.Size(80, 20)
        Me.txtStation.TabIndex = 26
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Location = New System.Drawing.Point(7, 115)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(40, 13)
        Me.lblStation.TabIndex = 25
        Me.lblStation.Text = "Station"
        '
        'chkAutoMatching
        '
        Me.chkAutoMatching.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAutoMatching.AutoSize = True
        Me.chkAutoMatching.Location = New System.Drawing.Point(532, 106)
        Me.chkAutoMatching.Name = "chkAutoMatching"
        Me.chkAutoMatching.Size = New System.Drawing.Size(94, 17)
        Me.chkAutoMatching.TabIndex = 7
        Me.chkAutoMatching.Text = "Auto matching"
        Me.chkAutoMatching.UseVisualStyleBackColor = True
        '
        'btnMRCPlot
        '
        Me.btnMRCPlot.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMRCPlot.Location = New System.Drawing.Point(532, 77)
        Me.btnMRCPlot.Name = "btnMRCPlot"
        Me.btnMRCPlot.Size = New System.Drawing.Size(75, 23)
        Me.btnMRCPlot.TabIndex = 6
        Me.btnMRCPlot.Text = "Plot MRC"
        Me.btnMRCPlot.UseVisualStyleBackColor = True
        '
        'lstEquations
        '
        Me.lstEquations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstEquations.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstEquations.FormattingEnabled = True
        Me.lstEquations.Location = New System.Drawing.Point(6, 19)
        Me.lstEquations.Name = "lstEquations"
        Me.lstEquations.Size = New System.Drawing.Size(520, 94)
        Me.lstEquations.TabIndex = 8
        '
        'btnMRCAdd
        '
        Me.btnMRCAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMRCAdd.Location = New System.Drawing.Point(532, 19)
        Me.btnMRCAdd.Name = "btnMRCAdd"
        Me.btnMRCAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnMRCAdd.TabIndex = 4
        Me.btnMRCAdd.Text = "Add MRC"
        Me.btnMRCAdd.UseVisualStyleBackColor = True
        '
        'btnMRCDelete
        '
        Me.btnMRCDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMRCDelete.Location = New System.Drawing.Point(532, 48)
        Me.btnMRCDelete.Name = "btnMRCDelete"
        Me.btnMRCDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnMRCDelete.TabIndex = 5
        Me.btnMRCDelete.Text = "Delete MRC"
        Me.btnMRCDelete.UseVisualStyleBackColor = True
        '
        'txtDA
        '
        Me.txtDA.Location = New System.Drawing.Point(92, 131)
        Me.txtDA.Name = "txtDA"
        Me.txtDA.Size = New System.Drawing.Size(60, 20)
        Me.txtDA.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(89, 116)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "DA (sq mi)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(287, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Coeff. A"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(353, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Coeff. B"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(419, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Coeff. C"
        '
        'txtCoefA
        '
        Me.txtCoefA.Location = New System.Drawing.Point(290, 131)
        Me.txtCoefA.Name = "txtCoefA"
        Me.txtCoefA.Size = New System.Drawing.Size(60, 20)
        Me.txtCoefA.TabIndex = 1
        '
        'txtCoefB
        '
        Me.txtCoefB.Location = New System.Drawing.Point(356, 131)
        Me.txtCoefB.Name = "txtCoefB"
        Me.txtCoefB.Size = New System.Drawing.Size(60, 20)
        Me.txtCoefB.TabIndex = 2
        '
        'txtCoefC
        '
        Me.txtCoefC.Location = New System.Drawing.Point(422, 131)
        Me.txtCoefC.Name = "txtCoefC"
        Me.txtCoefC.Size = New System.Drawing.Size(60, 20)
        Me.txtCoefC.TabIndex = 3
        '
        'gbDA
        '
        Me.gbDA.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDA.Controls.Add(Me.lstStations)
        Me.gbDA.Controls.Add(Me.btnStations)
        Me.gbDA.Location = New System.Drawing.Point(8, 134)
        Me.gbDA.Name = "gbDA"
        Me.gbDA.Size = New System.Drawing.Size(635, 136)
        Me.gbDA.TabIndex = 38
        Me.gbDA.TabStop = False
        Me.gbDA.Text = "Specify Drainage Area (DA)"
        '
        'lstStations
        '
        Me.lstStations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstStations.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstStations.FormattingEnabled = True
        Me.lstStations.Location = New System.Drawing.Point(6, 19)
        Me.lstStations.Name = "lstStations"
        Me.lstStations.Size = New System.Drawing.Size(517, 108)
        Me.lstStations.TabIndex = 10
        '
        'btnStations
        '
        Me.btnStations.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStations.Location = New System.Drawing.Point(532, 19)
        Me.btnStations.Name = "btnStations"
        Me.btnStations.Size = New System.Drawing.Size(97, 23)
        Me.btnStations.TabIndex = 9
        Me.btnStations.Text = "Browse Stations"
        Me.btnStations.UseVisualStyleBackColor = True
        '
        'gbRecSum
        '
        Me.gbRecSum.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbRecSum.Controls.Add(Me.btnRecSum)
        Me.gbRecSum.Controls.Add(Me.lstRecSum)
        Me.gbRecSum.Location = New System.Drawing.Point(8, 6)
        Me.gbRecSum.Name = "gbRecSum"
        Me.gbRecSum.Size = New System.Drawing.Size(635, 122)
        Me.gbRecSum.TabIndex = 39
        Me.gbRecSum.TabStop = False
        Me.gbRecSum.Text = "Specify polynomial coefficients"
        '
        'btnRecSum
        '
        Me.btnRecSum.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecSum.Location = New System.Drawing.Point(529, 15)
        Me.btnRecSum.Name = "btnRecSum"
        Me.btnRecSum.Size = New System.Drawing.Size(100, 23)
        Me.btnRecSum.TabIndex = 11
        Me.btnRecSum.Text = "Browse RecSum"
        Me.btnRecSum.UseVisualStyleBackColor = True
        '
        'lstRecSum
        '
        Me.lstRecSum.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstRecSum.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstRecSum.FormattingEnabled = True
        Me.lstRecSum.Location = New System.Drawing.Point(6, 15)
        Me.lstRecSum.Name = "lstRecSum"
        Me.lstRecSum.Size = New System.Drawing.Size(517, 95)
        Me.lstRecSum.TabIndex = 12
        '
        'tabPlotMRC
        '
        Me.tabPlotMRC.Controls.Add(Me.panelGraph)
        Me.tabPlotMRC.Location = New System.Drawing.Point(4, 22)
        Me.tabPlotMRC.Name = "tabPlotMRC"
        Me.tabPlotMRC.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPlotMRC.Size = New System.Drawing.Size(652, 440)
        Me.tabPlotMRC.TabIndex = 1
        Me.tabPlotMRC.Text = "MRC Plot"
        Me.tabPlotMRC.UseVisualStyleBackColor = True
        '
        'panelGraph
        '
        Me.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelGraph.Location = New System.Drawing.Point(3, 3)
        Me.panelGraph.Name = "panelGraph"
        Me.panelGraph.Size = New System.Drawing.Size(646, 434)
        Me.panelGraph.TabIndex = 0
        '
        'tabMRCTable
        '
        Me.tabMRCTable.Controls.Add(Me.txtMRCTable)
        Me.tabMRCTable.Location = New System.Drawing.Point(4, 22)
        Me.tabMRCTable.Name = "tabMRCTable"
        Me.tabMRCTable.Size = New System.Drawing.Size(652, 440)
        Me.tabMRCTable.TabIndex = 2
        Me.tabMRCTable.Text = "MRC Table"
        Me.tabMRCTable.UseVisualStyleBackColor = True
        '
        'txtMRCTable
        '
        Me.txtMRCTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMRCTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMRCTable.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMRCTable.Location = New System.Drawing.Point(0, 0)
        Me.txtMRCTable.Multiline = True
        Me.txtMRCTable.Name = "txtMRCTable"
        Me.txtMRCTable.ReadOnly = True
        Me.txtMRCTable.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMRCTable.Size = New System.Drawing.Size(652, 440)
        Me.txtMRCTable.TabIndex = 0
        '
        'frmMRCControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 466)
        Me.Controls.Add(Me.tabMRCMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMRCControl"
        Me.Text = "View Master Recession Curve (MRC)"
        Me.tabMRCMain.ResumeLayout(False)
        Me.tabConstructMRC.ResumeLayout(False)
        Me.gbMRCEquations.ResumeLayout(False)
        Me.gbMRCEquations.PerformLayout()
        Me.gbDA.ResumeLayout(False)
        Me.gbRecSum.ResumeLayout(False)
        Me.tabPlotMRC.ResumeLayout(False)
        Me.tabMRCTable.ResumeLayout(False)
        Me.tabMRCTable.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMRCMain As System.Windows.Forms.TabControl
    Friend WithEvents tabConstructMRC As System.Windows.Forms.TabPage
    Friend WithEvents btnRecSum As System.Windows.Forms.Button
    Friend WithEvents btnMRCDelete As System.Windows.Forms.Button
    Friend WithEvents btnMRCAdd As System.Windows.Forms.Button
    Friend WithEvents btnStations As System.Windows.Forms.Button
    Friend WithEvents lstRecSum As System.Windows.Forms.ListBox
    Friend WithEvents lstStations As System.Windows.Forms.ListBox
    Friend WithEvents txtCoefC As System.Windows.Forms.TextBox
    Friend WithEvents txtCoefB As System.Windows.Forms.TextBox
    Friend WithEvents txtCoefA As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDA As System.Windows.Forms.TextBox
    Friend WithEvents lstEquations As System.Windows.Forms.CheckedListBox
    Friend WithEvents tabPlotMRC As System.Windows.Forms.TabPage
    Friend WithEvents btnMRCPlot As System.Windows.Forms.Button
    Friend WithEvents gbMRCEquations As System.Windows.Forms.GroupBox
    Friend WithEvents gbDA As System.Windows.Forms.GroupBox
    Friend WithEvents gbRecSum As System.Windows.Forms.GroupBox
    Friend WithEvents chkAutoMatching As System.Windows.Forms.CheckBox
    Friend WithEvents panelGraph As System.Windows.Forms.Panel
    Friend WithEvents txtStation As System.Windows.Forms.TextBox
    Friend WithEvents lblStation As System.Windows.Forms.Label
    Friend WithEvents lblMaxLogQ As System.Windows.Forms.Label
    Friend WithEvents txtLogQMin As System.Windows.Forms.TextBox
    Friend WithEvents lblMinLogQ As System.Windows.Forms.Label
    Friend WithEvents txtLogQMax As System.Windows.Forms.TextBox
    Friend WithEvents txtSeason As System.Windows.Forms.TextBox
    Friend WithEvents lblSeason As System.Windows.Forms.Label
    Friend WithEvents tabMRCTable As System.Windows.Forms.TabPage
    Friend WithEvents txtMRCTable As System.Windows.Forms.TextBox
End Class
