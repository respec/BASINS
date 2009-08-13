Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmManDelin
    Inherits System.Windows.Forms.Form
    'Dim pProjectFileName As String
    Private pStartDrawing As Boolean
    Private pXPts As Collection
    Private pYPts As Collection
    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pOrigCursor As MapWinGIS.tkCursor
    Private pOperatingShapefileName As String
    Private pProgressStatus As New clsProgressStatus
    Private pProgressStatusOriginal As IProgressStatus
    Private pStatusShowOriginal As Boolean

    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDefine As System.Windows.Forms.Label
    Friend WithEvents cbxPCS As System.Windows.Forms.CheckBox
    Friend WithEvents cmdDefine As System.Windows.Forms.Button
    Friend WithEvents cboReach As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCalc As System.Windows.Forms.Label
    Friend WithEvents cmdCalculate As System.Windows.Forms.Button
    Friend WithEvents cboDEM As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDelin As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdCommit As System.Windows.Forms.Button
    Friend WithEvents cboLayer As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdDelineate As System.Windows.Forms.Button
    Friend WithEvents cbxCombine As System.Windows.Forms.CheckBox
    Friend WithEvents cmdCombine As System.Windows.Forms.Button
    Friend WithEvents cboUnits As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Dim pPrevHandle As Integer

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        pStartDrawing = False

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
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManDelin))
        Me.cmdClose = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cbxCombine = New System.Windows.Forms.CheckBox
        Me.lblDefine = New System.Windows.Forms.Label
        Me.cbxPCS = New System.Windows.Forms.CheckBox
        Me.cmdDefine = New System.Windows.Forms.Button
        Me.cboReach = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblCalc = New System.Windows.Forms.Label
        Me.cmdCalculate = New System.Windows.Forms.Button
        Me.cboDEM = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cmdCombine = New System.Windows.Forms.Button
        Me.cmdDelineate = New System.Windows.Forms.Button
        Me.lblDelin = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdCommit = New System.Windows.Forms.Button
        Me.cboLayer = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cboUnits = New System.Windows.Forms.ComboBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Location = New System.Drawing.Point(159, 514)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(104, 25)
        Me.cmdClose.TabIndex = 0
        Me.cmdClose.Text = "&Close"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.cbxCombine)
        Me.GroupBox1.Controls.Add(Me.lblDefine)
        Me.GroupBox1.Controls.Add(Me.cbxPCS)
        Me.GroupBox1.Controls.Add(Me.cmdDefine)
        Me.GroupBox1.Controls.Add(Me.cboReach)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(2, 358)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(423, 150)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Stream Network"
        '
        'cbxCombine
        '
        Me.cbxCombine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxCombine.Location = New System.Drawing.Point(198, 107)
        Me.cbxCombine.Name = "cbxCombine"
        Me.cbxCombine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbxCombine.Size = New System.Drawing.Size(210, 25)
        Me.cbxCombine.TabIndex = 21
        Me.cbxCombine.Text = "Force continuous flow path"
        '
        'lblDefine
        '
        Me.lblDefine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDefine.Location = New System.Drawing.Point(175, 55)
        Me.lblDefine.Name = "lblDefine"
        Me.lblDefine.Size = New System.Drawing.Size(239, 46)
        Me.lblDefine.TabIndex = 20
        Me.lblDefine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblDefine.Visible = False
        '
        'cbxPCS
        '
        Me.cbxPCS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxPCS.Location = New System.Drawing.Point(7, 107)
        Me.cbxPCS.Name = "cbxPCS"
        Me.cbxPCS.Size = New System.Drawing.Size(184, 25)
        Me.cbxPCS.TabIndex = 19
        Me.cbxPCS.Text = "Include PCS as Outlets"
        '
        'cmdDefine
        '
        Me.cmdDefine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDefine.Location = New System.Drawing.Point(7, 52)
        Me.cmdDefine.Name = "cmdDefine"
        Me.cmdDefine.Size = New System.Drawing.Size(163, 49)
        Me.cmdDefine.TabIndex = 18
        Me.cmdDefine.Text = "Define Stream Network and Outlets"
        '
        'cboReach
        '
        Me.cboReach.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReach.Location = New System.Drawing.Point(127, 21)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(288, 25)
        Me.cboReach.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 17)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Reach Layer:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.cboUnits)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.lblCalc)
        Me.GroupBox2.Controls.Add(Me.cmdCalculate)
        Me.GroupBox2.Controls.Add(Me.cboDEM)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(2, 205)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(423, 147)
        Me.GroupBox2.TabIndex = 18
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Subbasin Parameters"
        '
        'lblCalc
        '
        Me.lblCalc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCalc.Location = New System.Drawing.Point(175, 95)
        Me.lblCalc.Name = "lblCalc"
        Me.lblCalc.Size = New System.Drawing.Size(238, 46)
        Me.lblCalc.TabIndex = 16
        Me.lblCalc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblCalc.Visible = False
        '
        'cmdCalculate
        '
        Me.cmdCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCalculate.Location = New System.Drawing.Point(6, 95)
        Me.cmdCalculate.Name = "cmdCalculate"
        Me.cmdCalculate.Size = New System.Drawing.Size(163, 46)
        Me.cmdCalculate.TabIndex = 15
        Me.cmdCalculate.Text = "Calculate Subbasin Parameters"
        '
        'cboDEM
        '
        Me.cboDEM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDEM.Location = New System.Drawing.Point(127, 27)
        Me.cboDEM.Name = "cboDEM"
        Me.cboDEM.Size = New System.Drawing.Size(288, 25)
        Me.cboDEM.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 17)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Elevation Layer:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.cmdCombine)
        Me.GroupBox3.Controls.Add(Me.cmdDelineate)
        Me.GroupBox3.Controls.Add(Me.lblDelin)
        Me.GroupBox3.Controls.Add(Me.cmdCancel)
        Me.GroupBox3.Controls.Add(Me.cmdCommit)
        Me.GroupBox3.Controls.Add(Me.cboLayer)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(2, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(423, 187)
        Me.GroupBox3.TabIndex = 19
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Manual Delineation"
        '
        'cmdCombine
        '
        Me.cmdCombine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCombine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCombine.Location = New System.Drawing.Point(81, 147)
        Me.cmdCombine.Name = "cmdCombine"
        Me.cmdCombine.Size = New System.Drawing.Size(256, 27)
        Me.cmdCombine.TabIndex = 24
        Me.cmdCombine.Text = "Combine Selected Subbasins"
        '
        'cmdDelineate
        '
        Me.cmdDelineate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelineate.Location = New System.Drawing.Point(7, 57)
        Me.cmdDelineate.Name = "cmdDelineate"
        Me.cmdDelineate.Size = New System.Drawing.Size(163, 40)
        Me.cmdDelineate.TabIndex = 23
        Me.cmdDelineate.Text = "Delineate Subbasin"
        '
        'lblDelin
        '
        Me.lblDelin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDelin.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelin.Location = New System.Drawing.Point(12, 108)
        Me.lblDelin.Name = "lblDelin"
        Me.lblDelin.Size = New System.Drawing.Size(402, 36)
        Me.lblDelin.TabIndex = 22
        Me.lblDelin.Text = "Click points on the map to delineate a new subbasin boundary.  When completed cli" & _
            "ck 'Commit' or right click on the map."
        Me.lblDelin.Visible = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Enabled = False
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(296, 65)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(72, 24)
        Me.cmdCancel.TabIndex = 21
        Me.cmdCancel.Text = "Cancel"
        '
        'cmdCommit
        '
        Me.cmdCommit.Enabled = False
        Me.cmdCommit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCommit.Location = New System.Drawing.Point(198, 65)
        Me.cmdCommit.Name = "cmdCommit"
        Me.cmdCommit.Size = New System.Drawing.Size(81, 24)
        Me.cmdCommit.TabIndex = 20
        Me.cmdCommit.Text = "Commit"
        '
        'cboLayer
        '
        Me.cboLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboLayer.Location = New System.Drawing.Point(127, 25)
        Me.cboLayer.Name = "cboLayer"
        Me.cboLayer.Size = New System.Drawing.Size(288, 25)
        Me.cboLayer.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 17)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Subbasin Layer:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(156, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 17)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Vertical Units:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboUnits
        '
        Me.cboUnits.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUnits.Location = New System.Drawing.Point(257, 62)
        Me.cboUnits.Name = "cboUnits"
        Me.cboUnits.Size = New System.Drawing.Size(125, 25)
        Me.cboUnits.TabIndex = 18
        '
        'frmManDelin
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(429, 542)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmManDelin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Manual Watershed Delineator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        pMapWin.View.MapCursor = pOrigCursor
        pStartDrawing = False
        pMapWin.View.Draw.ClearDrawing(0)
        Me.Close()
    End Sub

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin)
        pMapWin = aMapWin
        pStatusShowOriginal = GisUtil.StatusShow
        GisUtil.MappingObject = aMapWin

        pProgressStatusOriginal = Logger.ProgressStatus
        pProgressStatus.ProgressStatusOther = Logger.ProgressStatus
        Logger.ProgressStatus = pProgressStatus

        'set delineation layer
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                cboLayer.Items.Add(GisUtil.LayerName(lLayerIndex))
                If GisUtil.CurrentLayer = lLayerIndex Then 'this is the current layer
                    cboLayer.SelectedIndex = cboLayer.Items.Count - 1
                End If
            End If
        Next lLayerIndex
        If cboLayer.SelectedIndex = -1 Then 'make a guess
            cboLayer.SelectedIndex = cboLayer.Items.IndexOf("Cataloging Unit Boundaries")
        End If

        'fill choices of units
        cboUnits.Items.Add("Feet")
        cboUnits.Items.Add("Meters")
        cboUnits.Items.Add("Centimeters")
        'set dem layer
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                cboDEM.Items.Add(lLayerName)
                If GisUtil.LayerFileName(lLayerIndex).IndexOf("\dem\") >= 0 And cboDEM.SelectedIndex = -1 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.Grid Then
                cboDEM.Items.Add(lLayerName)
                If GisUtil.LayerFileName(lLayerIndex).IndexOf("\demg\") >= 0 Or GisUtil.LayerFileName(lLayerIndex).IndexOf("\dem\") >= 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                ElseIf GisUtil.LayerFileName(lLayerIndex).IndexOf("\ned\") >= 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                End If
            End If
        Next lLayerIndex
        If cboDEM.SelectedIndex = -1 Then
            cmdCalculate.Enabled = False
            cmdDefine.Enabled = False
        End If

        'set reach layer
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = MapWindow.Interfaces.eLayerType.LineShapefile Then
                cboReach.Items.Add(GisUtil.LayerName(lLayerIndex))
                If GisUtil.LayerFileName(lLayerIndex).IndexOf("\nhd\") >= 0 Then
                    cboReach.SelectedIndex = cboReach.Items.Count - 1
                ElseIf GisUtil.LayerFileName(lLayerIndex).EndsWith("rf1.shp") Then
                    cboReach.SelectedIndex = cboReach.Items.Count - 1
                End If
            End If
        Next lLayerIndex

        pOrigCursor = pMapWin.View.MapCursor
        pOperatingShapefileName = ""
    End Sub

    Private Sub cmdDelineate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelineate.Click
        pMapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        pMapWin.View.MapCursor = MapWinGIS.tkCursor.crsrCross
        pStartDrawing = True
        pXpts = New Collection
        pYPts = New Collection
        cmdDelineate.Enabled = False
        cmdCombine.Enabled = False
        cboLayer.Enabled = False
        cmdCommit.Enabled = True
        cmdCancel.Enabled = True
        lblDelin.Visible = True
        If pOperatingShapefileName.Length = 0 Then
            'first time to delineate
            pOperatingShapefileName = ChangeNameCurrentOperatingShapefile(False)
        End If
    End Sub

    Public Sub MouseButtonClickUp(ByVal aX As Double, ByVal aY As Double, ByVal aButton As Integer)
        If pStartDrawing Then
            pXPts.Add(aX)
            pYPts.Add(aY)

            If pXPts.Count > 1 Then
                Dim lDraw_hndl As Integer = pMapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList)
                pMapWin.View.Draw.DrawLine(pXPts(pXPts.Count - 1), pYPts(pXPts.Count - 1), pXPts(pXPts.Count), pYPts(pXPts.Count), 1, System.Drawing.Color.Red)
            End If

            If aButton = 2 Then 'right click commits
                pPrevHandle = 0
                CommitLine()
            End If
        End If
    End Sub

    Public Sub MouseDrawingMove(ByVal aX As Double, ByVal aY As Double)
        If pStartDrawing Then
            If pXPts.Count > 0 Then
                pMapWin.View.Draw.ClearDrawing(pPrevHandle)
                Dim lDraw_hndl As Integer = pMapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList)
                pPrevHandle = lDraw_hndl
                pMapWin.View.Draw.DrawLine(pXPts(pXPts.Count), pYPts(pXPts.Count), aX, aY, 1, System.Drawing.Color.Red)
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        pStartDrawing = False
        cmdDelineate.Enabled = True
        cmdCombine.Enabled = True
        'cboLayer.Enabled = True
        cmdCommit.Enabled = False
        cmdCancel.Enabled = False
        lblDelin.Visible = False
        For lPointIndex As Integer = 0 To pXPts.Count
            'pMapWin.View.Draw.ClearDrawing(i - 2)
            pMapWin.View.Draw.ClearDrawing(lPointIndex)
        Next lPointIndex
        Do While pXpts.Count > 0
            pXpts.Remove(1)
            pYPts.Remove(1)
        Loop
    End Sub

    Private Sub cmdCommit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCommit.Click
        CommitLine()
    End Sub

    Private Sub CommitLine()
        pStartDrawing = False
        cmdDelineate.Enabled = True
        cmdCombine.Enabled = True
        'cboLayer.Enabled = True
        cmdCommit.Enabled = False
        cmdCancel.Enabled = False
        lblDelin.Visible = False

        pMapWin.View.Draw.ClearDrawing(pPrevHandle)

        'Create a new polyline shape
        Dim lClipShape As New MapWinGIS.Shape
        Dim lSuccess As Boolean = lClipShape.Create(MapWinGIS.ShpfileType.SHP_POLYLINE)
        For lPointIndex As Integer = 1 To pXPts.Count
            Dim lPoint As New MapWinGIS.Point
            lPoint.x = pXPts(lPointIndex)
            lPoint.y = pYPts(lPointIndex)
            lSuccess = lClipShape.InsertPoint(lPoint, lPointIndex)
        Next

        Dim lShapefile As New MapWinGIS.Shapefile
        lShapefile.Open(pOperatingShapefileName)

        Dim lShapefileOutput As New MapWinGIS.Shapefile

        'try to clip each polygon with this line
        Dim lShapes As New Collection
        For lShapeIndex As Integer = 1 To lShapefile.NumShapes
            Dim lFileIndex As Integer = 1
            Dim lFileNameTemp As String = PathNameOnly(pOperatingShapefileName) & "\temp" & lFileIndex & ".shp"
            Do While FileExists(lFileNameTemp)
                lFileIndex += 1
                lFileNameTemp = PathNameOnly(pOperatingShapefileName) & "\temp" & lFileIndex & ".shp"
            Loop

            lSuccess = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(lShapefile.Shape(lShapeIndex - 1), lClipShape, lFileNameTemp)
            If lSuccess Then
                lShapefileOutput.Open(lFileNameTemp)
                If lShapefileOutput.NumShapes > 0 Then
                    'this did clip a polygon, add the clipped shapes
                    For lShapeIndexOutput As Integer = 1 To lShapefileOutput.NumShapes
                        lShapes.Add(lShapefileOutput.Shape(lShapeIndexOutput - 1))
                    Next lShapeIndexOutput
                Else 'did not clip a polygon, add the original shape
                    lShapes.Add(lShapefile.Shape(lShapeIndex - 1))
                End If
                lShapefileOutput.Close()
            Else 'add the original shape too
                lShapes.Add(lShapefile.Shape(lShapeIndex - 1))
            End If
        Next lShapeIndex

        lShapefile.Close()

        'is this layer already in the view?
        Dim lInView As Integer = -1
        For lLayerIndex As Integer = 1 To GisUtil.NumLayers()
            If GisUtil.LayerFileName(lLayerIndex - 1) = pOperatingShapefileName Then
                'already in the view
                lInview = lLayerIndex
            End If
        Next lLayerIndex
        If lInview > -1 Then 'remove it so we can re-add it
            GisUtil.RemoveLayer(lInview - 1)
        Else 'add it to the cbolayers
            cboLayer.Items.Add("Subbasins")
            cboLayer.SelectedIndex = cboLayer.Items.Count - 1
        End If

        'create the new version of this shapefile
        Dim lOutputFileIndex As Integer = 1
        Dim lOutputPath As String = PathNameOnly(pOperatingShapefileName)
        Dim lInputProjectionFileName As String = FilenameSetExt(pOperatingShapefileName, "prj")
        pOperatingShapefileName = lOutputPath & "\subbasin" & lOutputFileIndex & ".shp"
        Do While FileExists(pOperatingShapefileName)
            lOutputFileIndex += 1
            pOperatingShapefileName = lOutputPath & "\subbasin" & lOutputFileIndex & ".shp"
        Loop
        'add shapes to the shapefile
        lSuccess = lShapefile.CreateNew(pOperatingShapefileName, MapWinGIS.ShpfileType.SHP_POLYGON)
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(pOperatingShapefileName, "prj"))
        End If

        lSuccess = lShapefile.StartEditingShapes(True)
        For Each lShape As MapWinGIS.Shape In lShapes
            lSuccess = lShapefile.EditInsertShape(lShape, 0)
        Next lShape
        'Add ID Field 
        Dim [of] As New MapWinGIS.Field
        [of].Name = "SUBBASIN"
        [of].Type = MapWinGIS.FieldType.INTEGER_FIELD
        [of].Width = 10
        lSuccess = lShapefile.EditInsertField([of], lShapefile.NumFields)
        For lShapeIndex As Integer = 1 To lShapefile.NumShapes
            lSuccess = lShapefile.EditCellValue(0, lShapeIndex - 1, lShapeIndex)
        Next lShapeIndex
        lSuccess = lShapefile.StopEditingShapes(True, True)
        lShapefile = Nothing

        'add output layer to the view
        Dim lOperatingShapeFile As New MapWinGIS.Shapefile
        lOperatingShapeFile.Open(pOperatingShapefileName)
        pMapWin.Layers.Add(lOperatingShapeFile, "Subbasins")
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Transparent
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Red
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).DrawFill = False

        'remove old points
        For lPointIndex As Integer = 0 To pXPts.Count
            'pMapWin.View.Draw.ClearDrawing(lPointIndex - 2)
            pMapWin.View.Draw.ClearDrawing(lPointIndex)
        Next lPointIndex
        Do While pXPts.Count > 0
            pXPts.Remove(1)
            pYPts.Remove(1)
        Loop

        Me.BringToFront()
    End Sub

    Private Sub cboDEM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDEM.SelectedIndexChanged
        If cboDEM.SelectedIndex = -1 Then
            cmdCalculate.Enabled = False
            cmdDefine.Enabled = False
        Else
            cmdCalculate.Enabled = True
            If cboReach.SelectedIndex = -1 Then
                cmdDefine.Enabled = False
            Else
                cmdDefine.Enabled = True
            End If
            'default units 
            Dim lElevationThemeName As String = cboDEM.Items(cboDEM.SelectedIndex)
            Dim lElevationLayerIndex As Integer = GisUtil.LayerIndex(lElevationThemeName)
            If (GisUtil.LayerFileName(lElevationLayerIndex).IndexOf("\ned\") > -1 Or GisUtil.LayerFileName(lElevationLayerIndex).IndexOf("\elev_cm") > -1) Then
                cboUnits.SelectedIndex = 2
            Else
                cboUnits.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub cmdCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalculate.Click
        CalculateSubbasinParametersUI()
    End Sub

    Private Sub CalculateSubbasinParametersUI()
        If cboLayer.SelectedIndex > -1 Then
            Dim lSubbasinThemeName As String = cboLayer.Items(cboLayer.SelectedIndex)
            Dim lElevationThemeName As String = cboDEM.Items(cboDEM.SelectedIndex)
            Dim lElevationUnitsName As String = cboUnits.Items(cboUnits.SelectedIndex)
            pProgressStatus.ProgressLabel = lblCalc
            CalculateSubbasinParameters(lSubbasinThemeName, lElevationThemeName, lElevationUnitsName)
        Else
            Logger.Msg("No Subbasin Layer to Calculate Paramters for")
        End If
    End Sub

    Private Sub cmdDefine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDefine.Click
        Dim lSubbasinThemeName As String = cboLayer.Items(cboLayer.SelectedIndex)
        Dim lReachThemeName As String = cboReach.Items(cboReach.SelectedIndex)
        pProgressStatus.ProgressLabel = lblDefine

        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinThemeName)
        If Not GisUtil.IsField(lSubbasinLayerIndex, "SUBBASIN") Or _
           Not GisUtil.IsField(lSubbasinLayerIndex, "SLO1") Then
            'we need to calculate the subbasin parameters first
            CalculateSubbasinParametersUI()
        End If

        Dim lOutletShapeFileName As String = ""
        Dim lElevationUnitsName As String = cboUnits.Items(cboUnits.SelectedIndex)
        CalculateReaches(lSubbasinThemeName, lReachThemeName, cboDEM.Items(cboDEM.SelectedIndex), _
                         cbxPCS.Checked, cbxCombine.Checked, lOutletShapeFileName, lElevationUnitsName)

        'add outlets layer to the map
        If GisUtil.IsLayer("Outlets") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Outlets"))
        End If
        GisUtil.AddLayer(lOutletShapeFileName, "Outlets")
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Cyan
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Cyan
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).LineOrPointSize = 5
    End Sub

    Private Sub frmManDelin_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Logger.ProgressStatus = pProgressStatusOriginal
        GisUtil.StatusShow = pStatusShowOriginal
    End Sub

    Private Sub frmManDelin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed Delineation\Manual Watershed Delineation.html")
        End If
    End Sub

    Private Sub cboReach_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReach.SelectedIndexChanged
        If cboReach.SelectedIndex = -1 Then
            cmdDefine.Enabled = False
        Else
            If cboDEM.SelectedIndex = -1 Then
                cmdDefine.Enabled = False
            Else
                cmdDefine.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmdCombine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCombine.Click
        'combine selected subbasins

        Dim lMergeLayerIndex As Integer

        If pOperatingShapefileName.Length = 0 Then
            'change name of operating shapefile
            'get index of current subbasin layer 
            For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerName(lLayerIndex).Trim = cboLayer.SelectedItem.ToString.Trim Then
                    lMergeLayerIndex = lLayerIndex
                End If
            Next lLayerIndex
            'build collection of selected shape indexes
            Dim lSelectedShapeIndexes As New atcCollection
            For lIndex As Integer = 1 To GisUtil.NumSelectedFeatures(lMergeLayerIndex)
                lSelectedShapeIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lMergeLayerIndex))
            Next
            'change name of operating shapefile
            pOperatingShapefileName = ChangeNameCurrentOperatingShapefile(True)
            'get index of new subbasin layer 
            For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
                If GisUtil.LayerName(lLayerIndex).Trim = cboLayer.SelectedItem.ToString.Trim Then
                    lMergeLayerIndex = lLayerIndex
                End If
            Next lLayerIndex
            'set selected features in new shapefile
            For lIndex As Integer = 1 To lSelectedShapeIndexes.Count
                GisUtil.SetSelectedFeature(lMergeLayerIndex, lSelectedShapeIndexes(lIndex - 1))
            Next
        End If

        'get index of current subbasin layer 
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            If GisUtil.LayerName(lLayerIndex).Trim = cboLayer.SelectedItem.ToString.Trim Then
                lMergeLayerIndex = lLayerIndex
            End If
        Next lLayerIndex

        'merge all selected shapes together
        GisUtil.MergeSelectedShapes(lMergeLayerIndex)

    End Sub

    Private Function ChangeNameCurrentOperatingShapefile(ByVal aAddtoView As Boolean) As String
        'get name of current subbasin layer from combo box
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            If GisUtil.LayerName(lLayerIndex).Trim = cboLayer.SelectedItem.ToString.Trim Then
                pOperatingShapefileName = GisUtil.LayerFileName(lLayerIndex)
            End If
        Next lLayerIndex
        'now open new file for output
        Dim pOutputPath As String = PathNameOnly(pOperatingShapefileName) & "\Watershed"
        If Not FileExists(pOutputPath, True, False) Then
            MkDirPath(pOutputPath)
        End If
        Dim lIndex As Integer = 1
        Dim lOutputFileName As String = pOutputPath & "\subbasin" & lIndex & ".shp"
        Do While FileExists(lOutputFileName)
            lIndex += 1
            lOutputFileName = pOutputPath & "\subbasin" & lIndex & ".shp"
        Loop
        'copy the base shapefile to the new name
        System.IO.File.Copy(pOperatingShapefileName, lOutputFileName)
        Dim ilen As Integer = pOperatingShapefileName.Length
        Dim jlen As Integer = lOutputFileName.Length
        System.IO.File.Copy(Mid(pOperatingShapefileName, 1, ilen - 3) & "dbf", Mid(lOutputFileName, 1, jlen - 3) & "dbf")
        System.IO.File.Copy(Mid(pOperatingShapefileName, 1, ilen - 3) & "shx", Mid(lOutputFileName, 1, jlen - 3) & "shx")
        Dim lInputProjectionFileName As String = FilenameSetExt(pOperatingShapefileName, "prj")
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(lOutputFileName, "prj"))
        End If

        If aAddtoView Then
            'is this layer already in the view?
            Dim lInView As Boolean = False
            For lLayerIndex As Integer = 1 To GisUtil.NumLayers()
                If GisUtil.LayerFileName(lLayerIndex - 1) = lOutputFileName Then
                    'already in the view
                    lInView = True
                End If
            Next lLayerIndex
            If Not lInView Then
                'add it to the cbolayers
                cboLayer.Items.Add("Subbasins")
                cboLayer.SelectedIndex = cboLayer.Items.Count - 1
                'add output layer to the view
                Dim lOperatingShapeFile As New MapWinGIS.Shapefile
                lOperatingShapeFile.Open(lOutputFileName)
                pMapWin.Layers.Add(lOperatingShapeFile, "Subbasins")
                pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Transparent
                pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Red
                pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).DrawFill = False
            End If
        End If

        Return lOutputFileName
    End Function
End Class
