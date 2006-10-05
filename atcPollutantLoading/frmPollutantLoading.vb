Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Drawing
Imports atcControls

Public Class frmModelSetup
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents tabPLOAD As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabLanduse As System.Windows.Forms.TabPage
    Friend WithEvents lblLanduseType As System.Windows.Forms.Label
    Friend WithEvents lblSubbasinsLayer As System.Windows.Forms.Label
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents tabPointSources As System.Windows.Forms.TabPage
    Friend WithEvents tabBMPs As System.Windows.Forms.TabPage
    Friend WithEvents tabValues As System.Windows.Forms.TabPage
    Friend WithEvents rbSimpleMethod As System.Windows.Forms.RadioButton
    Friend WithEvents rbExportCoefficientMethod As System.Windows.Forms.RadioButton
    Friend WithEvents cmdChangeFile As System.Windows.Forms.Button
    Friend WithEvents lblValueFileName As System.Windows.Forms.Label
    Friend WithEvents lblValueFile As System.Windows.Forms.Label
    Friend WithEvents atcGridValues As atcControls.atcGrid
    Friend WithEvents lblValueUnits As System.Windows.Forms.Label
    Friend WithEvents lstConstituents As System.Windows.Forms.ListBox
    Friend WithEvents lblMethod As System.Windows.Forms.Label
    Friend WithEvents lblPollutants As System.Windows.Forms.Label
    Friend WithEvents ofdValues As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSetup))
        Me.tabPLOAD = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.rbSimpleMethod = New System.Windows.Forms.RadioButton
        Me.rbExportCoefficientMethod = New System.Windows.Forms.RadioButton
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.lblSubbasinsLayer = New System.Windows.Forms.Label
        Me.lblLanduseType = New System.Windows.Forms.Label
        Me.tabLanduse = New System.Windows.Forms.TabPage
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.tabValues = New System.Windows.Forms.TabPage
        Me.lblValueUnits = New System.Windows.Forms.Label
        Me.cmdChangeFile = New System.Windows.Forms.Button
        Me.lblValueFileName = New System.Windows.Forms.Label
        Me.lblValueFile = New System.Windows.Forms.Label
        Me.atcGridValues = New atcControls.atcGrid
        Me.tabPointSources = New System.Windows.Forms.TabPage
        Me.tabBMPs = New System.Windows.Forms.TabPage
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdValues = New System.Windows.Forms.OpenFileDialog
        Me.lstConstituents = New System.Windows.Forms.ListBox
        Me.lblPollutants = New System.Windows.Forms.Label
        Me.lblMethod = New System.Windows.Forms.Label
        Me.tabPLOAD.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabLanduse.SuspendLayout()
        Me.tabValues.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPLOAD
        '
        Me.tabPLOAD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPLOAD.Controls.Add(Me.tabGeneral)
        Me.tabPLOAD.Controls.Add(Me.tabLanduse)
        Me.tabPLOAD.Controls.Add(Me.tabValues)
        Me.tabPLOAD.Controls.Add(Me.tabPointSources)
        Me.tabPLOAD.Controls.Add(Me.tabBMPs)
        Me.tabPLOAD.Cursor = System.Windows.Forms.Cursors.Default
        Me.tabPLOAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPLOAD.ItemSize = New System.Drawing.Size(60, 21)
        Me.tabPLOAD.Location = New System.Drawing.Point(16, 16)
        Me.tabPLOAD.Name = "tabPLOAD"
        Me.tabPLOAD.SelectedIndex = 0
        Me.tabPLOAD.Size = New System.Drawing.Size(540, 276)
        Me.tabPLOAD.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.SystemColors.Control
        Me.tabGeneral.Controls.Add(Me.lblMethod)
        Me.tabGeneral.Controls.Add(Me.lblPollutants)
        Me.tabGeneral.Controls.Add(Me.lstConstituents)
        Me.tabGeneral.Controls.Add(Me.rbSimpleMethod)
        Me.tabGeneral.Controls.Add(Me.rbExportCoefficientMethod)
        Me.tabGeneral.Controls.Add(Me.cboSubbasins)
        Me.tabGeneral.Controls.Add(Me.cboLanduse)
        Me.tabGeneral.Controls.Add(Me.lblSubbasinsLayer)
        Me.tabGeneral.Controls.Add(Me.lblLanduseType)
        Me.tabGeneral.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(532, 247)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'rbSimpleMethod
        '
        Me.rbSimpleMethod.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbSimpleMethod.AutoSize = True
        Me.rbSimpleMethod.Location = New System.Drawing.Point(312, 154)
        Me.rbSimpleMethod.Name = "rbSimpleMethod"
        Me.rbSimpleMethod.Size = New System.Drawing.Size(111, 21)
        Me.rbSimpleMethod.TabIndex = 10
        Me.rbSimpleMethod.Text = "Simple (EMC)"
        Me.rbSimpleMethod.UseVisualStyleBackColor = True
        '
        'rbExportCoefficientMethod
        '
        Me.rbExportCoefficientMethod.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbExportCoefficientMethod.AutoSize = True
        Me.rbExportCoefficientMethod.Checked = True
        Me.rbExportCoefficientMethod.Location = New System.Drawing.Point(312, 126)
        Me.rbExportCoefficientMethod.Name = "rbExportCoefficientMethod"
        Me.rbExportCoefficientMethod.Size = New System.Drawing.Size(140, 21)
        Me.rbExportCoefficientMethod.TabIndex = 9
        Me.rbExportCoefficientMethod.TabStop = True
        Me.rbExportCoefficientMethod.Text = "Export Coefficient "
        Me.rbExportCoefficientMethod.UseVisualStyleBackColor = True
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(200, 26)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(252, 24)
        Me.cboSubbasins.TabIndex = 8
        '
        'cboLanduse
        '
        Me.cboLanduse.AllowDrop = True
        Me.cboLanduse.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLanduse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLanduse.Location = New System.Drawing.Point(200, 63)
        Me.cboLanduse.Name = "cboLanduse"
        Me.cboLanduse.Size = New System.Drawing.Size(252, 24)
        Me.cboLanduse.TabIndex = 7
        '
        'lblSubbasinsLayer
        '
        Me.lblSubbasinsLayer.Location = New System.Drawing.Point(24, 26)
        Me.lblSubbasinsLayer.Name = "lblSubbasinsLayer"
        Me.lblSubbasinsLayer.Size = New System.Drawing.Size(152, 24)
        Me.lblSubbasinsLayer.TabIndex = 2
        Me.lblSubbasinsLayer.Text = "Subbasins Layer:"
        '
        'lblLanduseType
        '
        Me.lblLanduseType.Location = New System.Drawing.Point(24, 63)
        Me.lblLanduseType.Name = "lblLanduseType"
        Me.lblLanduseType.Size = New System.Drawing.Size(152, 24)
        Me.lblLanduseType.TabIndex = 1
        Me.lblLanduseType.Text = "Land Use Type:"
        '
        'tabLanduse
        '
        Me.tabLanduse.Controls.Add(Me.cboLandUseLayer)
        Me.tabLanduse.Controls.Add(Me.lblLandUseLayer)
        Me.tabLanduse.Location = New System.Drawing.Point(4, 25)
        Me.tabLanduse.Name = "tabLanduse"
        Me.tabLanduse.Size = New System.Drawing.Size(519, 229)
        Me.tabLanduse.TabIndex = 1
        Me.tabLanduse.Text = "Land Use"
        Me.tabLanduse.UseVisualStyleBackColor = True
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(200, 32)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(239, 24)
        Me.cboLandUseLayer.TabIndex = 10
        '
        'lblLandUseLayer
        '
        Me.lblLandUseLayer.Location = New System.Drawing.Point(40, 32)
        Me.lblLandUseLayer.Name = "lblLandUseLayer"
        Me.lblLandUseLayer.Size = New System.Drawing.Size(152, 24)
        Me.lblLandUseLayer.TabIndex = 9
        Me.lblLandUseLayer.Text = "Land Use Layer:"
        '
        'tabValues
        '
        Me.tabValues.Controls.Add(Me.lblValueUnits)
        Me.tabValues.Controls.Add(Me.cmdChangeFile)
        Me.tabValues.Controls.Add(Me.lblValueFileName)
        Me.tabValues.Controls.Add(Me.lblValueFile)
        Me.tabValues.Controls.Add(Me.atcGridValues)
        Me.tabValues.Location = New System.Drawing.Point(4, 25)
        Me.tabValues.Name = "tabValues"
        Me.tabValues.Size = New System.Drawing.Size(519, 229)
        Me.tabValues.TabIndex = 4
        Me.tabValues.Text = "Export Coefficients"
        Me.tabValues.UseVisualStyleBackColor = True
        '
        'lblValueUnits
        '
        Me.lblValueUnits.AutoSize = True
        Me.lblValueUnits.Location = New System.Drawing.Point(17, 32)
        Me.lblValueUnits.Name = "lblValueUnits"
        Me.lblValueUnits.Size = New System.Drawing.Size(71, 17)
        Me.lblValueUnits.TabIndex = 23
        Me.lblValueUnits.Text = "(lbs/ac/yr)"
        '
        'cmdChangeFile
        '
        Me.cmdChangeFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdChangeFile.Location = New System.Drawing.Point(426, 12)
        Me.cmdChangeFile.Name = "cmdChangeFile"
        Me.cmdChangeFile.Size = New System.Drawing.Size(73, 24)
        Me.cmdChangeFile.TabIndex = 22
        Me.cmdChangeFile.Text = "Change"
        '
        'lblValueFileName
        '
        Me.lblValueFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblValueFileName.Location = New System.Drawing.Point(176, 16)
        Me.lblValueFileName.Name = "lblValueFileName"
        Me.lblValueFileName.Size = New System.Drawing.Size(242, 16)
        Me.lblValueFileName.TabIndex = 21
        Me.lblValueFileName.Text = "<none>"
        Me.lblValueFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblValueFile
        '
        Me.lblValueFile.Location = New System.Drawing.Point(17, 16)
        Me.lblValueFile.Name = "lblValueFile"
        Me.lblValueFile.Size = New System.Drawing.Size(153, 16)
        Me.lblValueFile.TabIndex = 20
        Me.lblValueFile.Text = "Export Coefficient File:"
        Me.lblValueFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'atcGridValues
        '
        Me.atcGridValues.AllowHorizontalScrolling = True
        Me.atcGridValues.AllowNewValidValues = False
        Me.atcGridValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.atcGridValues.CellBackColor = System.Drawing.Color.Empty
        Me.atcGridValues.LineColor = System.Drawing.Color.Empty
        Me.atcGridValues.LineWidth = 0.0!
        Me.atcGridValues.Location = New System.Drawing.Point(20, 54)
        Me.atcGridValues.Name = "atcGridValues"
        Me.atcGridValues.Size = New System.Drawing.Size(479, 159)
        Me.atcGridValues.Source = Nothing
        Me.atcGridValues.TabIndex = 19
        '
        'tabPointSources
        '
        Me.tabPointSources.Location = New System.Drawing.Point(4, 25)
        Me.tabPointSources.Name = "tabPointSources"
        Me.tabPointSources.Size = New System.Drawing.Size(519, 229)
        Me.tabPointSources.TabIndex = 2
        Me.tabPointSources.Text = "Point Sources"
        Me.tabPointSources.UseVisualStyleBackColor = True
        '
        'tabBMPs
        '
        Me.tabBMPs.Location = New System.Drawing.Point(4, 25)
        Me.tabBMPs.Name = "tabBMPs"
        Me.tabBMPs.Size = New System.Drawing.Size(519, 229)
        Me.tabBMPs.TabIndex = 3
        Me.tabBMPs.Text = "BMPs"
        Me.tabBMPs.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 310)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(103, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Generate"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(125, 310)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(380, 310)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(80, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(468, 310)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(88, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "&About"
        '
        'ofdValues
        '
        Me.ofdValues.DefaultExt = "dbf"
        Me.ofdValues.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdValues.Title = "Select File"
        '
        'lstConstituents
        '
        Me.lstConstituents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstConstituents.FormattingEnabled = True
        Me.lstConstituents.ItemHeight = 16
        Me.lstConstituents.Location = New System.Drawing.Point(48, 126)
        Me.lstConstituents.Name = "lstConstituents"
        Me.lstConstituents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstConstituents.Size = New System.Drawing.Size(189, 100)
        Me.lstConstituents.TabIndex = 11
        '
        'lblPollutants
        '
        Me.lblPollutants.AutoSize = True
        Me.lblPollutants.Location = New System.Drawing.Point(26, 104)
        Me.lblPollutants.Name = "lblPollutants"
        Me.lblPollutants.Size = New System.Drawing.Size(74, 17)
        Me.lblPollutants.TabIndex = 12
        Me.lblPollutants.Text = "Pollutants:"
        '
        'lblMethod
        '
        Me.lblMethod.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMethod.AutoSize = True
        Me.lblMethod.Location = New System.Drawing.Point(296, 106)
        Me.lblMethod.Name = "lblMethod"
        Me.lblMethod.Size = New System.Drawing.Size(59, 17)
        Me.lblMethod.TabIndex = 13
        Me.lblMethod.Text = "Method:"
        '
        'frmModelSetup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(572, 355)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.tabPLOAD)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmModelSetup"
        Me.Text = "BASINS Pollutant Loading Estimator"
        Me.tabPLOAD.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabLanduse.ResumeLayout(False)
        Me.tabValues.ResumeLayout(False)
        Me.tabValues.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        SetGridValues()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        MsgBox("BASINS Pollutant Loading Estimator for MapWindow" & vbCrLf & vbCrLf & "Version 1.0", , "BASINS")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        'MsgBox("Help is not yet implemented.", MsgBoxStyle.Critical, "BASINS " & pModelName & " Problem")
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If lstConstituents.SelectedItems.Count = 0 Then
            Logger.Message("At least one pollutant must be selected", "Pollutant Loading Estimator Problem", _
                           Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Stop, Windows.Forms.MessageBoxDefaultButton.Button1)
        Else
            GenerateLoads()
        End If
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        cmdOK.Enabled = b
        cmdHelp.Enabled = b
        cmdCancel.Enabled = b
        cmdAbout.Enabled = b
        tabPLOAD.Enabled = b
    End Sub

    Public Sub InitializeUI()
        Dim lTemp As String

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")

        Dim lLyr As Integer
        Dim lSelectedLayer As Integer = -1
        For lLyr = 0 To GisUtil.NumLayers() - 1
            lTemp = GisUtil.LayerName(lLyr)
            If GisUtil.LayerType(lLyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(lTemp)
                If UCase(lTemp) = "SUBBASINS" Or InStr(lTemp, "Watershed Shapefile") > 0 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
                If GisUtil.CurrentLayer = lLyr Then
                    lSelectedLayer = cboSubbasins.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLyr) = 1 Then
                'PointShapefile
                'cboOutlets.Items.Add(ctemp)
                'If UCase(ctemp) = "OUTLETS" Then
                '    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
                'End If
            End If
        Next
        If lSelectedLayer > -1 Then
            cboSubbasins.SelectedIndex = lSelectedLayer
        End If
        'if all else fails set it to the first one
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            cboSubbasins.SelectedIndex = 0
        End If
        'If cboOutlets.Items.Count > 0 And cboOutlets.SelectedIndex < 0 Then
        '    cboOutlets.SelectedIndex = 0
        'End If

        With atcGridValues
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With

        cboLanduse.SelectedIndex = 0
    End Sub

    Private Sub frmPollutantLoading_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
        End If
    End Sub

    Private Sub rbExportCoefficientMethod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbExportCoefficientMethod.CheckedChanged
        If rbExportCoefficientMethod.Checked = True Then
            tabValues.Text = "Export Coefficients"
        Else
            tabValues.Text = "Event Mean Concentrations"
        End If
    End Sub

    Private Sub cmdChangeFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeFile.Click
        If rbExportCoefficientMethod.Checked Then
            ofdValues.Title = "Set Export Coefficient File"
        Else
            ofdValues.Title = "Set Event Mean Concentration File"
        End If
        If ofdValues.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblValueFileName.Text = ofdValues.FileName
            SetGridValues()
        End If
    End Sub

    Private Sub SetGridValues()
        Dim i As Integer, k As Integer
        Dim lDbf As IatcTable
        Dim lSorted As New atcCollection

        If atcGridValues.Source Is Nothing Then Exit Sub

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            lblValueFileName.Text = "\BASINS\etc\ecgiras.dbf"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            lblValueFileName.Text = "\BASINS\etc\ecgiras.dbf"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
            lblValueFileName.Text = "\BASINS\etc\ecnlcd.dbf"
        Else 'grid
            lblValueFileName.Text = "\BASINS\etc\ecnlcd.dbf"
        End If
        atcGridValues.Clear()

        If lblValueFileName.Text <> "<none>" Then
            lDbf = atcUtility.atcTableOpener.OpenAnyTable(lblValueFileName.Text)

            With atcGridValues.Source
                .Rows = 1
                .Columns = lDbf.NumFields
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                For i = 1 To lDbf.NumFields
                    .CellValue(0, i) = lDbf.FieldName(i)
                    .CellColor(0, i) = SystemColors.ControlDark
                Next i

                For k = 1 To lDbf.NumRecords
                    lDbf.CurrentRecord = k
                    For i = 1 To lDbf.NumFields
                        .CellValue(k, i) = lDbf.Value(i)
                        If i > 2 Then
                            .CellEditable(k, i) = True
                        Else
                            .CellColor(k, i) = SystemColors.ControlDark
                        End If
                    Next i
                Next k
            End With

        End If
        atcGridValues.SizeAllColumnsToContents()
        atcGridValues.Refresh()
        SetPollutantList()
    End Sub

    Private Sub SetPollutantList()
        Dim i As Integer

        lstConstituents.Items.Clear()
        For i = 0 To atcGridValues.Source.Columns - 4
            lstConstituents.Items.Add(atcGridValues.Source.CellValue(0, i + 3))
        Next i
        lstConstituents.SelectedItems.Clear()
        lstConstituents.SelectedItems.Add(lstConstituents.Items(0))
    End Sub

    Private Sub GenerateLoads()
        Dim i As Integer, j As Integer, k As Integer
        Dim lSubbasinLayerName As String
        Dim lSubbasinLayerIndex As Integer
        Dim lLucode As Integer
        Dim lProblem As String

        If lblValueFileName.Text <> "<none>" Then

            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            lSubbasinLayerName = cboSubbasins.Items(cboSubbasins.SelectedIndex)
            lSubbasinLayerIndex = GisUtil.LayerIndex(lSubbasinLayerName)

            'are any areas selected?
            Dim lSelectedAreaIndexes As New Collection
            For i = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
                'add selected areas to the collection
                lSelectedAreaIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSubbasinLayerIndex))
            Next
            If lSelectedAreaIndexes.Count = 0 Then
                'no areas selected, act as if all are selected
                For i = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    lSelectedAreaIndexes.Add(i - 1)
                Next
            End If

            'build array for output area values (area of each landuse in each subbasin)
            Dim lMaxlu As Integer = atcGridValues.Source.CellValue(atcGridValues.Source.Rows - 1, 1)
            Dim lAreasLS(lMaxlu, lSelectedAreaIndexes.Count) As Double

            'build array of constituent names
            Dim lConsNames(atcGridValues.Source.Columns - 4) As String
            For i = 0 To atcGridValues.Source.Columns - 4
                lConsNames(i) = atcGridValues.Source.CellValue(0, i + 3)
            Next i

            'build array for each export coeff for each land use type
            Dim lCoeffsLC(lMaxlu, atcGridValues.Source.Columns - 4) As Double
            For j = 1 To atcGridValues.Source.Rows - 1
                lLucode = atcGridValues.Source.CellValue(j, 1)
                For i = 0 To atcGridValues.Source.Columns - 4
                    lCoeffsLC(lLucode, i) = atcGridValues.Source.CellValue(j, i + 3)
                Next i
            Next j

            'build array for output load values (load for each subbasin and constituent)
            Dim lLoadsSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double
            'build array for area of each subbasin 
            Dim lAreasS(lSelectedAreaIndexes.Count) As Double
            'build array for per acre load values (load for each subbasin and constituent)
            Dim lLoadsPerAcreSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double

            If rbExportCoefficientMethod.Checked Then 'Export Coefficients Method
                'calculate areas of each land use in each subbasin
                If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                    CalculateGIRASAreas(lSubbasinLayerIndex, lSelectedAreaIndexes, lAreasLS)
                    'areas will be returned in square meters
                End If
                'calculate loads
                For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                    For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                        For k = 1 To lMaxlu
                            lLoadsSC(i, j) = lLoadsSC(i, j) + (lCoeffsLC(k, j) * lAreasLS(k, i) / 4046.8564)
                            ' / 4046.8564 to convert from m2 to acres
                        Next k
                    Next j
                Next i
                'calculate areas of each subbasin
                For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                    For k = 1 To lMaxlu
                        lAreasS(i) = lAreasS(i) + (lAreasLS(k, i) / 4046.8564)
                        ' / 4046.8564 to convert from m2 to acres
                    Next k
                Next i
                'calculate loads per acre
                For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                    If lAreasS(i) > 0 Then
                        For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                            lLoadsPerAcreSC(i, j) = lLoadsSC(i, j) / lAreasS(i)
                        Next j
                    End If
                Next i

                'add group to map for output 
                Dim lGroupName As String = "Estimated Annual Pollutant Loads"
                GisUtil.AddGroup(lGroupName)

                'now build the output shapefiles 
                Dim lOutputShapefileName As String
                Dim lLayerDesc As String
                For j = 0 To lConsNames.GetUpperBound(0)
                    'for each constituent
                    If IsConstituentSelected(lConsNames(j)) Then
                        For k = 1 To 2
                            lOutputShapefileName = ""
                            GisUtil.SaveSelectedFeatures(lSubbasinLayerIndex, lSelectedAreaIndexes, _
                                                         lOutputShapefileName)

                            'add the output shapefile to the map
                            If k = 1 Then
                                lLayerDesc = lConsNames(j) & " Load Per Acre (lbs)"
                            Else
                                lLayerDesc = lConsNames(j) & " Load (lbs)"
                            End If
                            Dim lBasename As String = lLayerDesc
                            i = 0
                            Do While GisUtil.IsLayer(lLayerDesc)
                                i = i + 1
                                lLayerDesc = lBasename & " " & i
                            Loop
                            If Not GisUtil.AddLayerToGroup(lOutputShapefileName, lLayerDesc, lGroupName) Then
                                lProblem = "Cant load layer " & lOutputShapefileName
                                Logger.Dbg(lProblem)
                            End If

                            'add fields to the output shapefile
                            Dim lOutputLayerIndex As String = GisUtil.LayerIndex(lLayerDesc)
                            Dim lFieldIndex As Integer
                            GisUtil.StartSetFeatureValue(lOutputLayerIndex)
                            'add loads
                            Try
                                lFieldIndex = GisUtil.FieldIndex(lOutputLayerIndex, lConsNames(j) & "_load")
                            Catch
                                lFieldIndex = GisUtil.AddField(lOutputLayerIndex, lConsNames(j) & "_load", 2, 10)
                            End Try
                            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                                If k = 1 Then
                                    GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, lLoadsPerAcreSC(i, j))
                                Else
                                    GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, lLoadsSC(i, j))
                                End If
                            Next i
                            GisUtil.StopSetFeatureValue(lOutputLayerIndex)

                            'set renderer for this layer
                            GisUtil.SetLayerRendererUniqueValues(lLayerDesc, lConsNames(j) & "_load")
                        Next k
                    End If
                Next j

                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

            Me.Close()

        End If
    End Sub

    Private Function IsConstituentSelected(ByVal aName As String) As Boolean
        Dim i As Integer

        IsConstituentSelected = False
        For i = 1 To lstConstituents.SelectedItems.Count
            If aName = lstConstituents.SelectedItems(i - 1) Then
                IsConstituentSelected = True
            End If
        Next
    End Function

    Private Sub CalculateGIRASAreas(ByVal aAreaLayerIndex As Integer, _
                                    ByVal aSelectedAreaIndexes As Collection, _
                                    ByRef aAreaLandSub(,) As Double)

        Dim lProblem As String = ""
        Dim lLanduseLayerIndex As Integer
        Dim lLandUseFieldIndex As Integer
        Dim lGridSource As New atcGridSource

        'set land use index layer
        Try
            lLanduseLayerIndex = GisUtil.LayerIndex("Land Use Index")
        Catch
            lProblem = "Missing Land Use Index Layer"
            Logger.Dbg(lProblem)
        End Try

        If Len(lProblem) = 0 Then

            Try
                lLandUseFieldIndex = GisUtil.FieldIndex(lLanduseLayerIndex, "COVNAME")
            Catch
                lProblem = "Expected field missing from Land Use Index Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                'figure out which land use tiles to list
                Dim lcluTiles As New Collection
                Dim lNewFileName As String
                Dim j As Integer
                Dim i As Integer
                For i = 1 To GisUtil.NumFeatures(lLanduseLayerIndex)
                    'loop thru each shape of land use index shapefile
                    lNewFileName = GisUtil.FieldValue(lLanduseLayerIndex, i - 1, lLandUseFieldIndex)
                    For j = 1 To aSelectedAreaIndexes.Count
                        If GisUtil.OverlappingPolygons(lLanduseLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
                            'yes, add it
                            lcluTiles.Add(lNewFileName)
                            Exit For
                        End If
                    Next j
                Next i

                Dim lPathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex)) & "\landuse"
                Dim lLandUseFieldName As String
                For j = 1 To lcluTiles.Count
                    'loop thru each land use tile
                    lNewFileName = lPathName & "\" & lcluTiles(j) & ".shp"
                    If Not GisUtil.AddLayer(lNewFileName, lcluTiles(j)) Then
                        lProblem = "Missing GIRAS Land Use Layer " & lcluTiles(j)
                        Logger.Dbg(lProblem)
                    End If

                    If Len(lProblem) = 0 Then
                        lLandUseFieldName = "LUCODE"
                        If GisUtil.LayerName(aAreaLayerIndex) <> "<none>" Then
                            'do overlay
                            Dim lLayer1Index As Integer = GisUtil.LayerIndex(lcluTiles(j))
                            GisUtil.TabulatePolygonAreas(lLayer1Index, _
                                                         GisUtil.FieldIndex(lLayer1Index, lLandUseFieldName), _
                                                         aAreaLayerIndex, aSelectedAreaIndexes, _
                                                         aAreaLandSub)
                        End If
                    End If
                Next j
            End If
        End If

    End Sub
End Class