<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownload
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDownload))
        Me.grpBASINS = New System.Windows.Forms.GroupBox
        Me.chkBASINS_Met = New System.Windows.Forms.CheckBox
        Me.chkBASINS_PCS3 = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NHD = New System.Windows.Forms.CheckBox
        Me.chkBASINS_NED = New System.Windows.Forms.CheckBox
        Me.chkBASINS_LSTORET = New System.Windows.Forms.CheckBox
        Me.chkBASINS_GIRAS = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEMG = New System.Windows.Forms.CheckBox
        Me.chkBASINS_DEM = New System.Windows.Forms.CheckBox
        Me.chkBASINS_Census = New System.Windows.Forms.CheckBox
        Me.grpNWIS = New System.Windows.Forms.GroupBox
        Me.chkNWIS_Measurements = New System.Windows.Forms.CheckBox
        Me.chkNWIS_WQ = New System.Windows.Forms.CheckBox
        Me.chkNWIS_DailyFlow = New System.Windows.Forms.CheckBox
        Me.chkTerraServer_Urban = New System.Windows.Forms.CheckBox
        Me.grpTerraServer = New System.Windows.Forms.GroupBox
        Me.chkTerraServer_DRG = New System.Windows.Forms.CheckBox
        Me.chkTerraServer_DOQ = New System.Windows.Forms.CheckBox
        Me.chkNLCD_1992 = New System.Windows.Forms.CheckBox
        Me.grpNLCD = New System.Windows.Forms.GroupBox
        Me.chkNLCD_2001 = New System.Windows.Forms.CheckBox
        Me.grpNHDplus = New System.Windows.Forms.GroupBox
        Me.chkNHDplus_All = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDownload = New System.Windows.Forms.Button
        Me.cboRegion = New System.Windows.Forms.ComboBox
        Me.lblRegion = New System.Windows.Forms.Label
        Me.chkClip = New System.Windows.Forms.CheckBox
        Me.lblClip = New System.Windows.Forms.Label
        Me.lblMerge = New System.Windows.Forms.Label
        Me.chkMerge = New System.Windows.Forms.CheckBox
        Me.grpBASINS.SuspendLayout()
        Me.grpNWIS.SuspendLayout()
        Me.grpTerraServer.SuspendLayout()
        Me.grpNLCD.SuspendLayout()
        Me.grpNHDplus.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpBASINS
        '
        Me.grpBASINS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Met)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_PCS3)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NHD)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_NED)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_LSTORET)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_GIRAS)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEMG)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_DEM)
        Me.grpBASINS.Controls.Add(Me.chkBASINS_Census)
        Me.grpBASINS.Location = New System.Drawing.Point(12, 12)
        Me.grpBASINS.Name = "grpBASINS"
        Me.grpBASINS.Size = New System.Drawing.Size(351, 88)
        Me.grpBASINS.TabIndex = 0
        Me.grpBASINS.TabStop = False
        Me.grpBASINS.Text = "Basins"
        '
        'chkBASINS_Met
        '
        Me.chkBASINS_Met.AutoSize = True
        Me.chkBASINS_Met.Location = New System.Drawing.Point(112, 65)
        Me.chkBASINS_Met.Name = "chkBASINS_Met"
        Me.chkBASINS_Met.Size = New System.Drawing.Size(89, 17)
        Me.chkBASINS_Met.TabIndex = 5
        Me.chkBASINS_Met.Text = "Meterological"
        Me.chkBASINS_Met.UseVisualStyleBackColor = True
        '
        'chkBASINS_PCS3
        '
        Me.chkBASINS_PCS3.AutoSize = True
        Me.chkBASINS_PCS3.Location = New System.Drawing.Point(229, 65)
        Me.chkBASINS_PCS3.Name = "chkBASINS_PCS3"
        Me.chkBASINS_PCS3.Size = New System.Drawing.Size(53, 17)
        Me.chkBASINS_PCS3.TabIndex = 8
        Me.chkBASINS_PCS3.Text = "PCS3"
        Me.chkBASINS_PCS3.UseVisualStyleBackColor = True
        '
        'chkBASINS_NHD
        '
        Me.chkBASINS_NHD.AutoSize = True
        Me.chkBASINS_NHD.Location = New System.Drawing.Point(229, 42)
        Me.chkBASINS_NHD.Name = "chkBASINS_NHD"
        Me.chkBASINS_NHD.Size = New System.Drawing.Size(50, 17)
        Me.chkBASINS_NHD.TabIndex = 7
        Me.chkBASINS_NHD.Text = "NHD"
        Me.chkBASINS_NHD.UseVisualStyleBackColor = True
        '
        'chkBASINS_NED
        '
        Me.chkBASINS_NED.AutoSize = True
        Me.chkBASINS_NED.Location = New System.Drawing.Point(229, 19)
        Me.chkBASINS_NED.Name = "chkBASINS_NED"
        Me.chkBASINS_NED.Size = New System.Drawing.Size(49, 17)
        Me.chkBASINS_NED.TabIndex = 6
        Me.chkBASINS_NED.Text = "NED"
        Me.chkBASINS_NED.UseVisualStyleBackColor = True
        '
        'chkBASINS_LSTORET
        '
        Me.chkBASINS_LSTORET.AutoSize = True
        Me.chkBASINS_LSTORET.Location = New System.Drawing.Point(112, 42)
        Me.chkBASINS_LSTORET.Name = "chkBASINS_LSTORET"
        Me.chkBASINS_LSTORET.Size = New System.Drawing.Size(108, 17)
        Me.chkBASINS_LSTORET.TabIndex = 4
        Me.chkBASINS_LSTORET.Text = "Legacy STORET"
        Me.chkBASINS_LSTORET.UseVisualStyleBackColor = True
        '
        'chkBASINS_GIRAS
        '
        Me.chkBASINS_GIRAS.AutoSize = True
        Me.chkBASINS_GIRAS.Location = New System.Drawing.Point(112, 19)
        Me.chkBASINS_GIRAS.Name = "chkBASINS_GIRAS"
        Me.chkBASINS_GIRAS.Size = New System.Drawing.Size(59, 17)
        Me.chkBASINS_GIRAS.TabIndex = 3
        Me.chkBASINS_GIRAS.Text = "GIRAS"
        Me.chkBASINS_GIRAS.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEMG
        '
        Me.chkBASINS_DEMG.AutoSize = True
        Me.chkBASINS_DEMG.Location = New System.Drawing.Point(6, 65)
        Me.chkBASINS_DEMG.Name = "chkBASINS_DEMG"
        Me.chkBASINS_DEMG.Size = New System.Drawing.Size(72, 17)
        Me.chkBASINS_DEMG.TabIndex = 2
        Me.chkBASINS_DEMG.Text = "DEM Grid"
        Me.chkBASINS_DEMG.UseVisualStyleBackColor = True
        '
        'chkBASINS_DEM
        '
        Me.chkBASINS_DEM.AutoSize = True
        Me.chkBASINS_DEM.Location = New System.Drawing.Point(6, 42)
        Me.chkBASINS_DEM.Name = "chkBASINS_DEM"
        Me.chkBASINS_DEM.Size = New System.Drawing.Size(84, 17)
        Me.chkBASINS_DEM.TabIndex = 1
        Me.chkBASINS_DEM.Text = "DEM Shape"
        Me.chkBASINS_DEM.UseVisualStyleBackColor = True
        '
        'chkBASINS_Census
        '
        Me.chkBASINS_Census.AutoSize = True
        Me.chkBASINS_Census.Location = New System.Drawing.Point(6, 19)
        Me.chkBASINS_Census.Name = "chkBASINS_Census"
        Me.chkBASINS_Census.Size = New System.Drawing.Size(61, 17)
        Me.chkBASINS_Census.TabIndex = 0
        Me.chkBASINS_Census.Text = "Census"
        Me.chkBASINS_Census.UseVisualStyleBackColor = True
        '
        'grpNWIS
        '
        Me.grpNWIS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_Measurements)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_WQ)
        Me.grpNWIS.Controls.Add(Me.chkNWIS_DailyFlow)
        Me.grpNWIS.Location = New System.Drawing.Point(12, 106)
        Me.grpNWIS.Name = "grpNWIS"
        Me.grpNWIS.Size = New System.Drawing.Size(351, 42)
        Me.grpNWIS.TabIndex = 1
        Me.grpNWIS.TabStop = False
        Me.grpNWIS.Text = "NWIS"
        '
        'chkNWIS_Measurements
        '
        Me.chkNWIS_Measurements.AutoSize = True
        Me.chkNWIS_Measurements.Location = New System.Drawing.Point(229, 19)
        Me.chkNWIS_Measurements.Name = "chkNWIS_Measurements"
        Me.chkNWIS_Measurements.Size = New System.Drawing.Size(120, 17)
        Me.chkNWIS_Measurements.TabIndex = 11
        Me.chkNWIS_Measurements.Text = "Flow Measurements"
        Me.chkNWIS_Measurements.UseVisualStyleBackColor = True
        '
        'chkNWIS_WQ
        '
        Me.chkNWIS_WQ.AutoSize = True
        Me.chkNWIS_WQ.Location = New System.Drawing.Point(112, 19)
        Me.chkNWIS_WQ.Name = "chkNWIS_WQ"
        Me.chkNWIS_WQ.Size = New System.Drawing.Size(90, 17)
        Me.chkNWIS_WQ.TabIndex = 10
        Me.chkNWIS_WQ.Text = "Water Quality"
        Me.chkNWIS_WQ.UseVisualStyleBackColor = True
        '
        'chkNWIS_DailyFlow
        '
        Me.chkNWIS_DailyFlow.AutoSize = True
        Me.chkNWIS_DailyFlow.Location = New System.Drawing.Point(6, 19)
        Me.chkNWIS_DailyFlow.Name = "chkNWIS_DailyFlow"
        Me.chkNWIS_DailyFlow.Size = New System.Drawing.Size(100, 17)
        Me.chkNWIS_DailyFlow.TabIndex = 9
        Me.chkNWIS_DailyFlow.Text = "Daily Discharge"
        Me.chkNWIS_DailyFlow.UseVisualStyleBackColor = True
        '
        'chkTerraServer_Urban
        '
        Me.chkTerraServer_Urban.AutoSize = True
        Me.chkTerraServer_Urban.Location = New System.Drawing.Point(6, 19)
        Me.chkTerraServer_Urban.Name = "chkTerraServer_Urban"
        Me.chkTerraServer_Urban.Size = New System.Drawing.Size(80, 17)
        Me.chkTerraServer_Urban.TabIndex = 12
        Me.chkTerraServer_Urban.Text = "Urban Area"
        Me.chkTerraServer_Urban.UseVisualStyleBackColor = True
        '
        'grpTerraServer
        '
        Me.grpTerraServer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_DRG)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_DOQ)
        Me.grpTerraServer.Controls.Add(Me.chkTerraServer_Urban)
        Me.grpTerraServer.Location = New System.Drawing.Point(12, 154)
        Me.grpTerraServer.Name = "grpTerraServer"
        Me.grpTerraServer.Size = New System.Drawing.Size(351, 42)
        Me.grpTerraServer.TabIndex = 9
        Me.grpTerraServer.TabStop = False
        Me.grpTerraServer.Text = "TerraServer Images"
        '
        'chkTerraServer_DRG
        '
        Me.chkTerraServer_DRG.AutoSize = True
        Me.chkTerraServer_DRG.Location = New System.Drawing.Point(229, 19)
        Me.chkTerraServer_DRG.Name = "chkTerraServer_DRG"
        Me.chkTerraServer_DRG.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServer_DRG.TabIndex = 14
        Me.chkTerraServer_DRG.Text = "DRG"
        Me.chkTerraServer_DRG.UseVisualStyleBackColor = True
        '
        'chkTerraServer_DOQ
        '
        Me.chkTerraServer_DOQ.AutoSize = True
        Me.chkTerraServer_DOQ.Location = New System.Drawing.Point(112, 19)
        Me.chkTerraServer_DOQ.Name = "chkTerraServer_DOQ"
        Me.chkTerraServer_DOQ.Size = New System.Drawing.Size(50, 17)
        Me.chkTerraServer_DOQ.TabIndex = 13
        Me.chkTerraServer_DOQ.Text = "DOQ"
        Me.chkTerraServer_DOQ.UseVisualStyleBackColor = True
        '
        'chkNLCD_1992
        '
        Me.chkNLCD_1992.AutoSize = True
        Me.chkNLCD_1992.Location = New System.Drawing.Point(6, 19)
        Me.chkNLCD_1992.Name = "chkNLCD_1992"
        Me.chkNLCD_1992.Size = New System.Drawing.Size(50, 17)
        Me.chkNLCD_1992.TabIndex = 15
        Me.chkNLCD_1992.Text = "1992"
        Me.chkNLCD_1992.UseVisualStyleBackColor = True
        '
        'grpNLCD
        '
        Me.grpNLCD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNLCD.Controls.Add(Me.chkNLCD_2001)
        Me.grpNLCD.Controls.Add(Me.chkNLCD_1992)
        Me.grpNLCD.Location = New System.Drawing.Point(12, 202)
        Me.grpNLCD.Name = "grpNLCD"
        Me.grpNLCD.Size = New System.Drawing.Size(351, 42)
        Me.grpNLCD.TabIndex = 11
        Me.grpNLCD.TabStop = False
        Me.grpNLCD.Text = "NLCD"
        '
        'chkNLCD_2001
        '
        Me.chkNLCD_2001.AutoSize = True
        Me.chkNLCD_2001.Location = New System.Drawing.Point(112, 19)
        Me.chkNLCD_2001.Name = "chkNLCD_2001"
        Me.chkNLCD_2001.Size = New System.Drawing.Size(50, 17)
        Me.chkNLCD_2001.TabIndex = 16
        Me.chkNLCD_2001.Text = "2001"
        Me.chkNLCD_2001.UseVisualStyleBackColor = True
        '
        'grpNHDplus
        '
        Me.grpNHDplus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNHDplus.Controls.Add(Me.chkNHDplus_All)
        Me.grpNHDplus.Location = New System.Drawing.Point(12, 250)
        Me.grpNHDplus.Name = "grpNHDplus"
        Me.grpNHDplus.Size = New System.Drawing.Size(351, 45)
        Me.grpNHDplus.TabIndex = 12
        Me.grpNHDplus.TabStop = False
        Me.grpNHDplus.Text = "NHD Plus"
        '
        'chkNHDplus_All
        '
        Me.chkNHDplus_All.AutoSize = True
        Me.chkNHDplus_All.Location = New System.Drawing.Point(6, 19)
        Me.chkNHDplus_All.Name = "chkNHDplus_All"
        Me.chkNHDplus_All.Size = New System.Drawing.Size(37, 17)
        Me.chkNHDplus_All.TabIndex = 17
        Me.chkNHDplus_All.Text = "All"
        Me.chkNHDplus_All.UseVisualStyleBackColor = True
        '
        'btnDownload
        '
        Me.btnDownload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDownload.Location = New System.Drawing.Point(288, 348)
        Me.btnDownload.Name = "btnDownload"
        Me.btnDownload.Size = New System.Drawing.Size(75, 23)
        Me.btnDownload.TabIndex = 29
        Me.btnDownload.Text = "Download"
        Me.btnDownload.UseVisualStyleBackColor = True
        '
        'cboRegion
        '
        Me.cboRegion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegion.FormattingEnabled = True
        Me.cboRegion.Items.AddRange(New Object() {"View Rectangle", "Extent of Selected Layer"})
        Me.cboRegion.Location = New System.Drawing.Point(124, 305)
        Me.cboRegion.Name = "cboRegion"
        Me.cboRegion.Size = New System.Drawing.Size(239, 21)
        Me.cboRegion.TabIndex = 30
        '
        'lblRegion
        '
        Me.lblRegion.AutoSize = True
        Me.lblRegion.Location = New System.Drawing.Point(12, 308)
        Me.lblRegion.Name = "lblRegion"
        Me.lblRegion.Size = New System.Drawing.Size(104, 13)
        Me.lblRegion.TabIndex = 31
        Me.lblRegion.Text = "Region to Download"
        '
        'chkClip
        '
        Me.chkClip.AutoSize = True
        Me.chkClip.Location = New System.Drawing.Point(124, 332)
        Me.chkClip.Name = "chkClip"
        Me.chkClip.Size = New System.Drawing.Size(15, 14)
        Me.chkClip.TabIndex = 35
        Me.chkClip.UseVisualStyleBackColor = True
        '
        'lblClip
        '
        Me.lblClip.AutoSize = True
        Me.lblClip.Location = New System.Drawing.Point(12, 332)
        Me.lblClip.Name = "lblClip"
        Me.lblClip.Size = New System.Drawing.Size(73, 13)
        Me.lblClip.TabIndex = 36
        Me.lblClip.Text = "Clip to Region"
        Me.ToolTip1.SetToolTip(Me.lblClip, "Remove downloaded portions that do not intersect the region")
        '
        'lblMerge
        '
        Me.lblMerge.AutoSize = True
        Me.lblMerge.Location = New System.Drawing.Point(12, 352)
        Me.lblMerge.Name = "lblMerge"
        Me.lblMerge.Size = New System.Drawing.Size(37, 13)
        Me.lblMerge.TabIndex = 38
        Me.lblMerge.Text = "Merge"
        Me.ToolTip1.SetToolTip(Me.lblMerge, "Merge separate parts of same coverage")
        '
        'chkMerge
        '
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Location = New System.Drawing.Point(124, 352)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(15, 14)
        Me.chkMerge.TabIndex = 37
        Me.chkMerge.UseVisualStyleBackColor = True
        '
        'frmDownload
        '
        Me.AcceptButton = Me.btnDownload
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 382)
        Me.Controls.Add(Me.lblMerge)
        Me.Controls.Add(Me.chkMerge)
        Me.Controls.Add(Me.lblClip)
        Me.Controls.Add(Me.chkClip)
        Me.Controls.Add(Me.lblRegion)
        Me.Controls.Add(Me.cboRegion)
        Me.Controls.Add(Me.btnDownload)
        Me.Controls.Add(Me.grpNHDplus)
        Me.Controls.Add(Me.grpNLCD)
        Me.Controls.Add(Me.grpTerraServer)
        Me.Controls.Add(Me.grpNWIS)
        Me.Controls.Add(Me.grpBASINS)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDownload"
        Me.Text = "Download Data"
        Me.grpBASINS.ResumeLayout(False)
        Me.grpBASINS.PerformLayout()
        Me.grpNWIS.ResumeLayout(False)
        Me.grpNWIS.PerformLayout()
        Me.grpTerraServer.ResumeLayout(False)
        Me.grpTerraServer.PerformLayout()
        Me.grpNLCD.ResumeLayout(False)
        Me.grpNLCD.PerformLayout()
        Me.grpNHDplus.ResumeLayout(False)
        Me.grpNHDplus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpBASINS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_Census As System.Windows.Forms.CheckBox
    Friend WithEvents grpNWIS As System.Windows.Forms.GroupBox
    Friend WithEvents chkBASINS_PCS3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_NHD As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_NED As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_LSTORET As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_GIRAS As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEMG As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_DEM As System.Windows.Forms.CheckBox
    Friend WithEvents chkBASINS_Met As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_Measurements As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_WQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkNWIS_DailyFlow As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServer_Urban As System.Windows.Forms.CheckBox
    Friend WithEvents grpTerraServer As System.Windows.Forms.GroupBox
    Friend WithEvents chkTerraServer_DRG As System.Windows.Forms.CheckBox
    Friend WithEvents chkTerraServer_DOQ As System.Windows.Forms.CheckBox
    Friend WithEvents chkNLCD_1992 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNLCD As System.Windows.Forms.GroupBox
    Friend WithEvents chkNLCD_2001 As System.Windows.Forms.CheckBox
    Friend WithEvents grpNHDplus As System.Windows.Forms.GroupBox
    Friend WithEvents chkNHDplus_All As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDownload As System.Windows.Forms.Button
    Friend WithEvents cboRegion As System.Windows.Forms.ComboBox
    Friend WithEvents lblRegion As System.Windows.Forms.Label
    Friend WithEvents chkClip As System.Windows.Forms.CheckBox
    Friend WithEvents lblClip As System.Windows.Forms.Label
    Friend WithEvents lblMerge As System.Windows.Forms.Label
    Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
End Class
