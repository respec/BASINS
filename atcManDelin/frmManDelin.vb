Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmManDelin
    Inherits System.Windows.Forms.Form
    'Dim pProjectFileName As String
    Dim startdrawing As Boolean
    Dim xpts As Collection
    Dim ypts As Collection
    Dim pMapWin As MapWindow.Interfaces.IMapWin
    Dim OrigCursor As MapWinGIS.tkCursor
    Dim OperatingShapefile As String
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
    Dim prevHandle As Integer

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        startdrawing = False

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
        Me.cmdDelineate = New System.Windows.Forms.Button
        Me.lblDelin = New System.Windows.Forms.Label
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdCommit = New System.Windows.Forms.Button
        Me.cboLayer = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
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
        Me.cmdClose.Location = New System.Drawing.Point(161, 432)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(102, 25)
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
        Me.GroupBox1.Location = New System.Drawing.Point(2, 283)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(421, 140)
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
        Me.lblDefine.Location = New System.Drawing.Point(166, 58)
        Me.lblDefine.Name = "lblDefine"
        Me.lblDefine.Size = New System.Drawing.Size(247, 27)
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
        Me.cmdDefine.Size = New System.Drawing.Size(151, 40)
        Me.cmdDefine.TabIndex = 18
        Me.cmdDefine.Text = "Define &Stream Network and Outlets"
        '
        'cboReach
        '
        Me.cboReach.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboReach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboReach.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboReach.Location = New System.Drawing.Point(127, 21)
        Me.cboReach.Name = "cboReach"
        Me.cboReach.Size = New System.Drawing.Size(286, 25)
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
        Me.GroupBox2.Controls.Add(Me.lblCalc)
        Me.GroupBox2.Controls.Add(Me.cmdCalculate)
        Me.GroupBox2.Controls.Add(Me.cboDEM)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(2, 166)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(421, 110)
        Me.GroupBox2.TabIndex = 18
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Subbasin Parameters"
        '
        'lblCalc
        '
        Me.lblCalc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCalc.Location = New System.Drawing.Point(166, 66)
        Me.lblCalc.Name = "lblCalc"
        Me.lblCalc.Size = New System.Drawing.Size(247, 24)
        Me.lblCalc.TabIndex = 16
        Me.lblCalc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblCalc.Visible = False
        '
        'cmdCalculate
        '
        Me.cmdCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCalculate.Location = New System.Drawing.Point(7, 58)
        Me.cmdCalculate.Name = "cmdCalculate"
        Me.cmdCalculate.Size = New System.Drawing.Size(151, 40)
        Me.cmdCalculate.TabIndex = 15
        Me.cmdCalculate.Text = "Calculate Subbasin &Parameters"
        '
        'cboDEM
        '
        Me.cboDEM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDEM.Location = New System.Drawing.Point(127, 27)
        Me.cboDEM.Name = "cboDEM"
        Me.cboDEM.Size = New System.Drawing.Size(286, 25)
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
        Me.GroupBox3.Controls.Add(Me.cmdDelineate)
        Me.GroupBox3.Controls.Add(Me.lblDelin)
        Me.GroupBox3.Controls.Add(Me.cmdCancel)
        Me.GroupBox3.Controls.Add(Me.cmdCommit)
        Me.GroupBox3.Controls.Add(Me.cboLayer)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(2, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(421, 147)
        Me.GroupBox3.TabIndex = 19
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Manual Delineation"
        '
        'cmdDelineate
        '
        Me.cmdDelineate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelineate.Location = New System.Drawing.Point(7, 57)
        Me.cmdDelineate.Name = "cmdDelineate"
        Me.cmdDelineate.Size = New System.Drawing.Size(151, 40)
        Me.cmdDelineate.TabIndex = 23
        Me.cmdDelineate.Text = "&Delineate Subbasin"
        '
        'lblDelin
        '
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
        Me.cmdCommit.Location = New System.Drawing.Point(188, 65)
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
        Me.cboLayer.Size = New System.Drawing.Size(286, 25)
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
        'frmManDelin
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(427, 471)
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
        pMapWin.View.MapCursor = OrigCursor
        startdrawing = False
        pMapWin.View.Draw.ClearDrawing(0)
        Me.Close()
    End Sub

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal m As MapWindow.Interfaces.IMapWin)
        Dim ctemp As String
        Dim lyr As Long
        Dim i As Integer

        pMapWin = m
        GisUtil.MappingObject = m

        'set delineation layer
        For lyr = 0 To GisUtil.NumLayers - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                'PolygonShapefile 
                cboLayer.Items.Add(ctemp)
                If GisUtil.CurrentLayer = lyr Then
                    'this is the current layer
                    cboLayer.SelectedIndex = cboLayer.Items.Count - 1
                End If
            End If
        Next lyr
        If cboLayer.SelectedIndex = -1 Then
            For i = 1 To cboLayer.Items.Count
                If cboLayer.Items(i - 1) = "Cataloging Unit Boundaries" Then
                    cboLayer.SelectedIndex = i - 1
                End If
            Next
        End If

        'set dem layer
        For lyr = 0 To GisUtil.NumLayers - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                'PolygonShapefile 
                cboDEM.Items.Add(ctemp)
                If InStr(GisUtil.LayerFileName(lyr), "\dem\") > 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lyr) = MapWindow.Interfaces.eLayerType.Grid Then
                'grid
                cboDEM.Items.Add(ctemp)
                If InStr(GisUtil.LayerFileName(lyr), "\demg\") > 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                ElseIf InStr(GisUtil.LayerFileName(lyr), "\ned\") > 0 Then
                    cboDEM.SelectedIndex = cboDEM.Items.Count - 1
                End If
            End If
        Next lyr
        If cboDEM.SelectedIndex = -1 Then
            cmdCalculate.Enabled = False
            cmdDefine.Enabled = False
        End If

        'set reach layer
        For lyr = 0 To GisUtil.NumLayers - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = MapWindow.Interfaces.eLayerType.LineShapefile Then
                'LineShapefile 
                cboReach.Items.Add(ctemp)
                If InStr(GisUtil.LayerFileName(lyr), "\nhd\") > 0 Then
                    cboReach.SelectedIndex = cboReach.Items.Count - 1
                ElseIf Microsoft.VisualBasic.Right(GisUtil.LayerFileName(lyr), 7) = "rf1.shp" Then
                    cboReach.SelectedIndex = cboReach.Items.Count - 1
                End If
            End If
        Next lyr

        OrigCursor = pMapWin.View.MapCursor
        OperatingShapefile = ""
    End Sub

    Private Sub cmdDelineate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelineate.Click
        Dim lyr As Long
        Dim outputpath As String
        Dim newfile As String
        Dim i As Integer
        Dim ilen As Integer
        Dim jlen As Integer

        pMapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        pMapWin.View.MapCursor = MapWinGIS.tkCursor.crsrCross
        startdrawing = True
        xpts = New Collection
        ypts = New Collection
        cmdDelineate.Enabled = False
        cboLayer.Enabled = False
        cmdCommit.Enabled = True
        cmdCancel.Enabled = True
        lblDelin.Visible = True
        If Len(OperatingShapefile) = 0 Then
            'first time to delineate
            'get name of current subbasin layer from combo box
            For lyr = 0 To GisUtil.NumLayers() - 1
                If Trim(GisUtil.LayerName(lyr)) = Trim(cboLayer.SelectedItem) Then
                    OperatingShapefile = GisUtil.LayerFileName(lyr)
                End If
            Next lyr
            'now open new file for output
            outputpath = PathNameOnly(OperatingShapefile) & "\Watershed"
            If Not FileExists(outputpath, True, False) Then
                MkDirPath(outputpath)
            End If
            i = 1
            newfile = outputpath & "\subbasin" & i & ".shp"
            Do While FileExists(newfile)
                i = i + 1
                newfile = outputpath & "\subbasin" & i & ".shp"
            Loop
            'copy the base shapefile to the new name
            System.IO.File.Copy(OperatingShapefile, newfile)
            ilen = Len(OperatingShapefile)
            jlen = Len(newfile)
            System.IO.File.Copy(Mid(OperatingShapefile, 1, ilen - 3) & "dbf", Mid(newfile, 1, jlen - 3) & "dbf")
            System.IO.File.Copy(Mid(OperatingShapefile, 1, ilen - 3) & "shx", Mid(newfile, 1, jlen - 3) & "shx")
            OperatingShapefile = newfile

            'clear out old fields
            'Dim newOperatingShapefile As New MapWinGIS.Shapefile
            'newOperatingShapefile.Open(OperatingShapefile)
            'newOperatingShapefile.StartEditingTable()
            'Do While newOperatingShapefile.NumFields > 1
            '  newOperatingShapefile.EditDeleteField(0)
            'Loop
            'newOperatingShapefile.StopEditingTable(True)
            'newOperatingShapefile.Close()
        End If
    End Sub

    Public Sub MouseButtonClickUp(ByVal x As Double, ByVal y As Double, ByVal button As Integer)
        Dim draw_hndl As Integer

        If startdrawing Then
            xpts.Add(x)
            ypts.Add(y)

            If xpts.Count > 1 Then
                draw_hndl = pMapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList)
                pMapWin.View.Draw.DrawLine(xpts(xpts.Count - 1), ypts(xpts.Count - 1), xpts(xpts.Count), ypts(xpts.Count), 1, System.Drawing.Color.Red)
            End If

            If button = 2 Then
                'right click commits
                prevHandle = 0
                CommitLine()
            End If
        End If
    End Sub

    Public Sub MouseDrawingMove(ByVal x As Double, ByVal y As Double)
        Dim draw_hndl As Integer
        If startdrawing Then
            If xpts.Count > 0 Then
                pMapWin.View.Draw.ClearDrawing(prevHandle)
                draw_hndl = pMapWin.View.Draw.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList)
                prevHandle = draw_hndl
                pMapWin.View.Draw.DrawLine(xpts(xpts.Count), ypts(xpts.Count), x, y, 1, System.Drawing.Color.Red)
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim i As Integer

        startdrawing = False
        cmdDelineate.Enabled = True
        'cboLayer.Enabled = True
        cmdCommit.Enabled = False
        cmdCancel.Enabled = False
        lblDelin.Visible = False
        For i = 0 To xpts.Count
            'pMapWin.View.Draw.ClearDrawing(i - 2)
            pMapWin.View.Draw.ClearDrawing(i)
        Next i
        Do While xpts.Count > 0
            xpts.Remove(1)
            ypts.Remove(1)
        Loop
    End Sub

    Private Sub cmdCommit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCommit.Click
        CommitLine()
    End Sub

    Private Sub CommitLine()
        Dim j As Integer
        Dim k As Integer
        Dim ltempfile As String

        startdrawing = False
        cmdDelineate.Enabled = True
        'cboLayer.Enabled = True
        cmdCommit.Enabled = False
        cmdCancel.Enabled = False
        lblDelin.Visible = False

        pMapWin.View.Draw.ClearDrawing(prevHandle)

        Dim clipshape As New MapWinGIS.Shape
        Dim pointx As New MapWinGIS.Point
        Dim success As Boolean
        Dim i As Integer
        'Create a new polyline shape
        success = clipshape.Create(MapWinGIS.ShpfileType.SHP_POLYLINE)
        For i = 1 To xpts.Count
            pointx = New MapWinGIS.Point
            pointx.x = xpts(i)
            pointx.y = ypts(i)
            success = clipshape.InsertPoint(pointx, i)
        Next

        Dim lShapefile As New MapWinGIS.Shapefile
        lShapefile.Open(OperatingShapefile)

        Dim lOutputShapefile As New MapWinGIS.Shapefile
        'try to clip each polygon with this line
        Dim cShapes As New Collection
        For i = 1 To lShapefile.NumShapes

            k = 1
            ltempfile = PathNameOnly(OperatingShapefile) & "\temp" & k & ".shp"
            Do While FileExists(ltempfile)
                k = k + 1
                ltempfile = PathNameOnly(OperatingShapefile) & "\temp" & k & ".shp"
            Loop

            success = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(lShapefile.Shape(i - 1), clipshape, ltempfile)
            If success Then
                lOutputShapefile.Open(ltempfile)
                If lOutputShapefile.NumShapes > 0 Then
                    'this did clip a polygon, add the clipped shapes
                    For j = 1 To lOutputShapefile.NumShapes
                        cShapes.Add(lOutputShapefile.Shape(j - 1))
                    Next j
                Else
                    'did not clip a polygon, add the original shape
                    cShapes.Add(lShapefile.Shape(i - 1))
                End If
                lOutputShapefile.Close()
            Else
                'add the original shape too
                cShapes.Add(lShapefile.Shape(i - 1))
            End If
        Next i

        lShapefile.Close()

        'is this layer already in the view?
        Dim inview As Integer
        inview = -1
        For i = 1 To GisUtil.NumLayers()
            If GisUtil.LayerFileName(i - 1) = OperatingShapefile Then
                'already in the view
                inview = i
            End If
        Next i
        If inview > -1 Then
            'remove it so we can re-add it
            GisUtil.RemoveLayer(inview - 1)
        Else
            'add it to the cbolayers
            cboLayer.Items.Add("Subbasins")
            cboLayer.SelectedIndex = cboLayer.Items.Count - 1
        End If

        'create the new version of this shapefile
        Dim csh As MapWinGIS.Shape
        i = 1
        Dim outputpath As String
        outputpath = PathNameOnly(OperatingShapefile)
        OperatingShapefile = outputpath & "\subbasin" & i & ".shp"
        Do While FileExists(OperatingShapefile)
            i = i + 1
            OperatingShapefile = outputpath & "\subbasin" & i & ".shp"
        Loop
        'add shapes to the shapefile
        success = lShapefile.CreateNew(OperatingShapefile, MapWinGIS.ShpfileType.SHP_POLYGON)
        success = lShapefile.StartEditingShapes(True)
        For Each csh In cShapes
            success = lShapefile.EditInsertShape(csh, 0)
        Next csh
        'Add ID Field 
        Dim [of] As New MapWinGIS.Field
        [of].Name = "SUBBASIN"
        [of].Type = MapWinGIS.FieldType.INTEGER_FIELD
        [of].Width = 10
        success = lShapefile.EditInsertField([of], lShapefile.NumFields)
        For i = 1 To lShapefile.NumShapes
            success = lShapefile.EditCellValue(0, i - 1, i)
        Next i
        success = lShapefile.StopEditingShapes(True, True)
        lShapefile = Nothing

        'add output layer to the view
        Dim newOperatingShapefile As New MapWinGIS.Shapefile
        newOperatingShapefile.Open(OperatingShapefile)
        pMapWin.Layers.Add(newOperatingShapefile, "Subbasins")
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Transparent
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Red
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).DrawFill = False

        'remove old points
        For i = 0 To xpts.Count
            'pMapWin.View.Draw.ClearDrawing(i - 2)
            pMapWin.View.Draw.ClearDrawing(i)
        Next i
        Do While xpts.Count > 0
            xpts.Remove(1)
            ypts.Remove(1)
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
        End If
    End Sub

    Private Sub cmdCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalculate.Click
        CalculateSubbasinParameters()
    End Sub

    Private Sub CalculateSubbasinParameters()
        Dim SubbasinThemeName As String
        Dim SubbasinLayerIndex As Integer
        Dim ElevationThemeName As String
        Dim ElevationLayerIndex As Integer
        Dim ElevationFieldIndex As Integer
        Dim i As Integer
        Dim j As Integer
        Dim SlopeFieldIndex As Integer
        Dim SubbasinFieldIndex As Integer
        Dim nsub As Integer
        Dim nelev As Integer

        If cboLayer.SelectedIndex > -1 Then

            'change to hourglass cursor
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            lblCalc.Visible = True
            lblCalc.Text = "Calculating..."
            Me.Refresh()

            SubbasinThemeName = cboLayer.Items(cboLayer.SelectedIndex)
            SubbasinLayerIndex = GisUtil.LayerIndex(SubbasinThemeName)
            ElevationThemeName = cboDEM.Items(cboDEM.SelectedIndex)
            ElevationLayerIndex = GisUtil.LayerIndex(ElevationThemeName)

            'calculate average elev
            'does mean elev field exist on subbasin shapefile?
            'MeanElevationFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "MEANELEV")
            'If MeanElevationFieldIndex = -1 Then
            '  'need to add it
            '  MeanElevationFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "MEANELEV", 2, 10)
            'End If
            'If GisUtil.LayerType(ElevationLayerIndex) = 3 Then
            '  'shapefile
            '  ElevationFieldIndex = GisUtil.FieldIndex(ElevationLayerIndex, "ELEV_M")
            '  For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
            '    subbasinArea = 0
            '    weightedElev = 0
            '    For j = 1 To GisUtil.NumFeatures(ElevationLayerIndex)
            '      If GisUtil.OverlappingPolygons(ElevationLayerIndex, j - 1, SubbasinLayerIndex, i - 1) Then
            '        subbasinArea = subbasinArea + GisUtil.AreaNthFeatureInLayer(ElevationLayerIndex, j - 1)
            '        weightedElev = weightedElev + (subbasinArea * GisUtil.FieldValue(ElevationLayerIndex, ElevationFieldIndex, j - 1))
            '      End If
            '    Next j
            '    weightedElev = weightedElev / subbasinArea
            '    'store in mean elevation field
            '    GisUtil.SetFeatureValue(SubbasinLayerIndex, MeanElevationFieldIndex, i - 1, weightedElev)
            '  Next i
            'Else
            '  'grid
            'End If

            'assign subbasin numbers
            If GisUtil.IsField(SubbasinLayerIndex, "SUBBASIN") Then
                SubbasinFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "SUBBASIN")
            Else
                'need to add it
                SubbasinFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "SUBBASIN", 0, 10)
                For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                    GisUtil.SetFeatureValue(SubbasinLayerIndex, SubbasinFieldIndex, i - 1, i)
                Next i
            End If

            'calculate slope
            Dim minelev As Double
            Dim maxelev As Double
            Dim elev As Double
            Dim slope As Double
            Dim aIndex() As Integer
            If GisUtil.IsField(SubbasinLayerIndex, "SLO1") Then
                SlopeFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "SLO1")
            Else
                'need to add it
                SlopeFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "SLO1", 2, 10)
            End If
            If GisUtil.LayerType(ElevationLayerIndex) = 3 Then
                'shapefile
                nsub = GisUtil.NumFeatures(SubbasinLayerIndex)
                nelev = GisUtil.NumFeatures(ElevationLayerIndex)
                ElevationFieldIndex = GisUtil.FieldIndex(ElevationLayerIndex, "ELEV_M")
                ReDim aIndex(nelev)
                GisUtil.AssignContainingPolygons(ElevationLayerIndex, SubbasinLayerIndex, aIndex)
                For i = 1 To nsub
                    minelev = 99999999
                    maxelev = -99999999
                    For j = 1 To nelev
                        'npercent = 100 * i * j / ntot
                        'If npercent > lastpercent Then
                        '  lblCalc.Text = "Calculating (" & npercent & "%)"
                        '  lastpercent = npercent
                        '  Me.Refresh()
                        'End If
                        If aIndex(j) = i - 1 Then
                            elev = GisUtil.FieldValue(ElevationLayerIndex, j - 1, ElevationFieldIndex)
                            If elev > maxelev Then
                                maxelev = elev
                            End If
                            If elev < minelev Then
                                minelev = elev
                            End If
                        End If
                    Next j
                    'store in slope field as percent
                    'estimate slope as the difference between max and min elevations / square root of subbasin area -- better approx?
                    slope = 100 * (maxelev - minelev) / ((GisUtil.FeatureArea(SubbasinLayerIndex, i - 1)) ^ 0.5)
                    GisUtil.SetFeatureValue(SubbasinLayerIndex, SlopeFieldIndex, i - 1, slope)
                Next i
            Else
                'grid
                nsub = GisUtil.NumFeatures(SubbasinLayerIndex)
                For i = 1 To nsub
                    'GisUtil.GridMinMaxInPolygon(ElevationLayerIndex, SubbasinLayerIndex, i - 1, minelev, maxelev)
                    'store in slope field as percent
                    'slope = 100 * (maxelev - minelev) / ((GisUtil.FeatureArea(SubbasinLayerIndex, i - 1)) ^ 0.5)
                    If GisUtil.FieldValue(SubbasinLayerIndex, i - 1, SlopeFieldIndex) <= 0 Then
                        If InStr(GisUtil.LayerFileName(ElevationLayerIndex), "\ned\") > 0 Then
                            slope = GisUtil.GridSlopeInPolygon(ElevationLayerIndex, SubbasinLayerIndex, i - 1)
                        Else
                            slope = 100 * GisUtil.GridSlopeInPolygon(ElevationLayerIndex, SubbasinLayerIndex, i - 1)
                        End If
                        GisUtil.SetFeatureValue(SubbasinLayerIndex, SlopeFieldIndex, i - 1, slope)
                    End If
                Next i
            End If

            'calculate length of overland flow plane
            'this is computed in WinHSPF based on slope, no need to compute here
            'Dim sl As Double

            'If GisUtil.IsField(SubbasinLayerIndex, "LEN1") Then
            '  LengthFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "LEN1")
            'Else
            '  'need to add it
            '  LengthFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "LEN1", 2, 10)
            'End If
            'For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
            '  slope = GisUtil.FieldValue(SubbasinLayerIndex, i - 1, SlopeFieldIndex)
            '  'Slope Length from old autodelin
            '  If ((slope > 0) And (slope < 2.0)) Then
            '    sl = 400 / 3.28
            '  ElseIf ((slope >= 2.0) And (slope < 5.0)) Then
            '    sl = 300 / 3.28
            '  ElseIf ((slope >= 5.0) And (slope < 8.0)) Then
            '    sl = 200 / 3.28
            '  ElseIf ((slope >= 8) And (slope < 10.0)) Then
            '    sl = 200 / 3.28
            '  ElseIf ((slope >= 10) And (slope < 12.0)) Then
            '    sl = 120.0 / 3.28
            '  ElseIf ((slope >= 12) And (slope < 16.0)) Then
            '    sl = 80.0 / 3.28
            '  ElseIf ((slope >= 16) And (slope < 20.0)) Then
            '    sl = 60.0 / 3.28
            '  ElseIf ((slope >= 20) And (slope < 25.0)) Then
            '    sl = 50.0 / 3.28
            '  Else
            '    sl = 0.05  '30.0/3.28      
            '  End If
            '  GisUtil.SetFeatureValue(SubbasinLayerIndex, LengthFieldIndex, i - 1, sl)
            'Next i

            'set area of each subbasin
            Dim r As Double
            Dim AreaAcresFieldIndex As Integer
            If GisUtil.IsField(SubbasinLayerIndex, "AREAACRES") Then
                AreaAcresFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "AREAACRES")
            Else
                'need to add it
                AreaAcresFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "AREAACRES", 2, 10)
            End If
            Dim AreaMi2FieldIndex As Integer
            If GisUtil.IsField(SubbasinLayerIndex, "AREAMI2") Then
                AreaMi2FieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "AREAMI2")
            Else
                'need to add it
                AreaMi2FieldIndex = GisUtil.AddField(SubbasinLayerIndex, "AREAMI2", 2, 10)
            End If
            For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                r = GisUtil.FeatureArea(SubbasinLayerIndex, i - 1)
                GisUtil.SetFeatureValue(SubbasinLayerIndex, AreaAcresFieldIndex, i - 1, r / 4046.86)
                GisUtil.SetFeatureValue(SubbasinLayerIndex, AreaMi2FieldIndex, i - 1, r / 2589988)
            Next i

            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            lblCalc.Text = ""
            lblCalc.Visible = False
            Me.Refresh()

        Else
            'cant do if we don't have a subbasin layer
        End If
    End Sub

    Private Sub cmdDefine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDefine.Click
        Dim SubbasinThemeName As String
        Dim SubbasinLayerIndex As Integer
        Dim ReachThemeName As String
        Dim ReachLayerIndex As Integer
        Dim OutputReachShapefileName As String
        Dim LevelFieldIndex As Integer
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim lowestlevel As Integer
        Dim StreamsLayerIndex As Integer

        SubbasinThemeName = cboLayer.Items(cboLayer.SelectedIndex)
        SubbasinLayerIndex = GisUtil.LayerIndex(SubbasinThemeName)

        If Not GisUtil.IsField(SubbasinLayerIndex, "SUBBASIN") Or _
           Not GisUtil.IsField(SubbasinLayerIndex, "SLO1") Then
            'we need to calculate the subbasin parameters first
            CalculateSubbasinParameters()
        End If

        'change to hourglass cursor
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        lblDefine.Visible = True
        lblDefine.Text = "Calculating..."
        Me.Refresh()

        'find the level field
        ReachThemeName = cboReach.Items(cboReach.SelectedIndex)
        ReachLayerIndex = GisUtil.LayerIndex(ReachThemeName)
        LevelFieldIndex = -1
        If Microsoft.VisualBasic.Right(GisUtil.LayerFileName(ReachLayerIndex), 7) = "rf1.shp" Then
            If GisUtil.IsField(ReachLayerIndex, "LEV") Then
                LevelFieldIndex = GisUtil.FieldIndex(ReachLayerIndex, "LEV")
            End If
        Else
            If GisUtil.IsField(ReachLayerIndex, "LEVEL") Then
                LevelFieldIndex = GisUtil.FieldIndex(ReachLayerIndex, "LEVEL")
            End If
            If LevelFieldIndex = -1 Then
                If GisUtil.IsField(ReachLayerIndex, "STREAMLEVE") Then
                    LevelFieldIndex = GisUtil.FieldIndex(ReachLayerIndex, "STREAMLEVE")
                End If
            End If
        End If
        If LevelFieldIndex = -1 Then
            MsgBox("Cannot find field 'Level' in the streams layer", MsgBoxStyle.OkOnly, "Stream Network Problem")
            Exit Sub
        End If

        lblDefine.Text = "Clipping..."
        Me.Refresh()
        'clip reach layer to subbasin boundaries
        OutputReachShapefileName = GisUtil.ClipShapesWithPolygon(ReachLayerIndex, SubbasinLayerIndex)

        'add output reach shapefile to the view
        If GisUtil.IsLayer("Streams") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Streams"))
            'if layer removed, need to obtain new indexes 
            SubbasinLayerIndex = GisUtil.LayerIndex(SubbasinThemeName)
            ReachLayerIndex = GisUtil.LayerIndex(ReachThemeName)
        End If
        GisUtil.AddLayer(OutputReachShapefileName, "Streams")
        StreamsLayerIndex = GisUtil.LayerIndex("Streams")
        GisUtil.LayerVisible(StreamsLayerIndex) = True

        Dim minfield As Integer
        minfield = 9999

        lblDefine.Text = "Indexing..."
        Me.Refresh()

        'identify which fields contain the upstream and downstream reach ids
        Dim rfield As Integer
        Dim dfield As Integer
        If GisUtil.IsField(StreamsLayerIndex, "RIVRCH") Then
            rfield = GisUtil.FieldIndex(StreamsLayerIndex, "RIVRCH")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "RCHID") Then
            rfield = GisUtil.FieldIndex(StreamsLayerIndex, "RCHID")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "COMID") Then
            rfield = GisUtil.FieldIndex(StreamsLayerIndex, "COMID")
        End If
        If GisUtil.IsField(StreamsLayerIndex, "DSCSM") Then
            dfield = GisUtil.FieldIndex(StreamsLayerIndex, "DSCSM")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "DSRCHID") Then
            dfield = GisUtil.FieldIndex(StreamsLayerIndex, "DSRCHID")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "TOCOMID") Then
            dfield = GisUtil.FieldIndex(StreamsLayerIndex, "TOCOMID")
        End If

        'temporarily flag segments that have been clipped -- we'll want to know this later
        Dim len1 As Double
        Dim len2 As Double
        Dim ClippedFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "CLIPPED") Then
            ClippedFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "CLIPPED")
        Else
            'need to add it
            ClippedFieldIndex = GisUtil.AddField(StreamsLayerIndex, "CLIPPED", 0, 10)
        End If
        GisUtil.StartSetFeatureValue(StreamsLayerIndex)
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            Dim lSearchVal As String = GisUtil.FieldValue(StreamsLayerIndex, i - 1, rfield)
            For j = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                If GisUtil.FieldValue(StreamsLayerIndex, j - 1, rfield) = lSearchVal And i <> j Then
                    'this record has the same reach id, mark as a clipped segment
                    GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, ClippedFieldIndex, i - 1, "clipped")
                    GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, ClippedFieldIndex, j - 1, "clipped")
                    len1 = GisUtil.FeatureLength(StreamsLayerIndex, i - 1)
                    len2 = GisUtil.FeatureLength(StreamsLayerIndex, j - 1)
                    If len1 < 0.05 * len2 Then
                        'this is just a nub, mark it for deletion
                        GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, LevelFieldIndex, i - 1, 998)
                    End If
                    If len2 < 0.05 * len1 Then
                        'this is just a nub, mark it for deletion
                        GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, LevelFieldIndex, j - 1, 998)
                    End If
                End If
            Next
        Next i
        GisUtil.StopSetFeatureValue(StreamsLayerIndex)

        'assign subbasin numbers to each reach segment
        Dim aIndex(GisUtil.NumFeatures(StreamsLayerIndex)) As Integer
        GisUtil.AssignContainingPolygons(StreamsLayerIndex, SubbasinLayerIndex, aIndex)
        Dim ReachSubbasinFieldIndex As Integer
        Dim SubbasinFieldIndex As Integer
        SubbasinFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "SUBBASIN")
        If GisUtil.IsField(StreamsLayerIndex, "SUBBASIN") Then
            ReachSubbasinFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "SUBBASIN")
        Else
            'need to add it
            ReachSubbasinFieldIndex = GisUtil.AddField(StreamsLayerIndex, "SUBBASIN", 1, 10)
        End If
        If ReachSubbasinFieldIndex < minfield Then minfield = ReachSubbasinFieldIndex
        GisUtil.StartSetFeatureValue(StreamsLayerIndex)
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            If aIndex(i) > -1 Then
                j = GisUtil.FieldValue(SubbasinLayerIndex, aIndex(i), SubbasinFieldIndex)
            Else
                j = aIndex(i)
            End If
            GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, ReachSubbasinFieldIndex, i - 1, j)
        Next i
        GisUtil.StopSetFeatureValue(StreamsLayerIndex)

        'clean out segments that are not within any subbasin, fix to clean up outliers in containing polygons
        GisUtil.StartRemoveFeature(StreamsLayerIndex)
        i = 0
        Do While i < GisUtil.NumFeatures(StreamsLayerIndex)
            If GisUtil.FieldValue(StreamsLayerIndex, i, ReachSubbasinFieldIndex) < 0 Then
                'remove this feature
                GisUtil.RemoveFeatureNoStartStop(StreamsLayerIndex, i)
            Else
                i = i + 1
            End If
        Loop
        GisUtil.StopRemoveFeature(StreamsLayerIndex)

        lblDefine.Text = "Filtering..."
        Me.Refresh()

        'find lowest reach level in each subbasin
        For k = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
            Logger.Progress(k, GisUtil.NumFeatures(SubbasinLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            lowestlevel = 999999
            For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                If GisUtil.FieldValue(StreamsLayerIndex, i - 1, ReachSubbasinFieldIndex) = GisUtil.FieldValue(SubbasinLayerIndex, k - 1, SubbasinFieldIndex) Then
                    'this is in the subbasin of interest
                    j = GisUtil.FieldValue(StreamsLayerIndex, i - 1, LevelFieldIndex)
                    If j < lowestlevel And j > 0 Then
                        lowestlevel = j
                    End If
                End If
            Next i

            'save only segments of the lowest level in this subbasin
            GisUtil.StartRemoveFeature(StreamsLayerIndex)
            i = 0
            Do While i < GisUtil.NumFeatures(StreamsLayerIndex)
                If GisUtil.FieldValue(StreamsLayerIndex, i, ReachSubbasinFieldIndex) = GisUtil.FieldValue(SubbasinLayerIndex, k - 1, SubbasinFieldIndex) Then
                    'this is in the subbasin of interest
                    j = GisUtil.FieldValue(StreamsLayerIndex, i, LevelFieldIndex)
                    If j <> lowestlevel Then
                        'remove this feature
                        GisUtil.RemoveFeatureNoStartStop(StreamsLayerIndex, i)
                    Else
                        i = i + 1
                    End If
                Else
                    i = i + 1
                End If
            Loop
            GisUtil.StopRemoveFeature(StreamsLayerIndex)
        Next k

        lblDefine.Text = "Merging..."
        Me.Refresh()

        'add downstream subbasin ids
        Dim DownstreamFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "SUBBASINR") Then
            DownstreamFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "SUBBASINR")
        Else
            'need to add it
            DownstreamFieldIndex = GisUtil.AddField(StreamsLayerIndex, "SUBBASINR", 1, 10)
        End If
        If DownstreamFieldIndex < minfield Then minfield = DownstreamFieldIndex

        Dim rval As String
        Dim dval As String
        Dim dsubbasin As String
        Dim rsubbasin As String
        GisUtil.StartSetFeatureValue(StreamsLayerIndex)
        'populate the downstream subbasin ids
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(StreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            dval = GisUtil.FieldValue(StreamsLayerIndex, i - 1, dfield)
            'find what is downstream of rval
            For j = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                rval = GisUtil.FieldValue(StreamsLayerIndex, j - 1, rfield)
                If rval = dval Then
                    'this is the downstream segment
                    dsubbasin = GisUtil.FieldValue(StreamsLayerIndex, j - 1, ReachSubbasinFieldIndex)
                    rsubbasin = GisUtil.FieldValue(StreamsLayerIndex, i - 1, ReachSubbasinFieldIndex)
                    'if the downstream subbasin id is different that this subbasin id
                    'set it, and make the same change to all segments of this subbasin id
                    If dsubbasin <> rsubbasin Then
                        GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, DownstreamFieldIndex, i - 1, dsubbasin)
                        'make another pass to set each stream within a subbasin to the same subbasinr
                        For k = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                            If GisUtil.FieldValue(StreamsLayerIndex, k - 1, ReachSubbasinFieldIndex) = rsubbasin Then
                                GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, DownstreamFieldIndex, k - 1, dsubbasin)
                            End If
                        Next k
                    End If
                    'exit once we found what is downstream of this segment
                    Exit For
                End If
            Next j
        Next i
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(StreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            dval = GisUtil.FieldValue(StreamsLayerIndex, i - 1, DownstreamFieldIndex)
            If dval = 0 Then
                GisUtil.SetFeatureValueNoStartStop(StreamsLayerIndex, DownstreamFieldIndex, i - 1, -999)
            End If
        Next i
        Logger.Progress(GisUtil.NumFeatures(StreamsLayerIndex), GisUtil.NumFeatures(StreamsLayerIndex))
        GisUtil.StopSetFeatureValue(StreamsLayerIndex)

        'merge reach segments together within subbasin
        GisUtil.MergeFeaturesBasedOnAttribute(StreamsLayerIndex, ReachSubbasinFieldIndex, cbxCombine.Checked)

        'create and populate fields
        lblDefine.Text = "Calculating attributes..."
        Me.Refresh()
        Dim TempFieldIndex As Integer

        'set length of stream reach
        Dim LengthFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "LEN2") Then
            LengthFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "LEN2")
        Else
            'need to add it
            LengthFieldIndex = GisUtil.AddField(StreamsLayerIndex, "LEN2", 2, 10)
        End If
        If LengthFieldIndex < minfield Then minfield = LengthFieldIndex
        Dim r As Double
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            r = GisUtil.FeatureLength(StreamsLayerIndex, i - 1)
            GisUtil.SetFeatureValue(StreamsLayerIndex, LengthFieldIndex, i - 1, r)
        Next i

        'set local contributing area of stream reach
        Dim AreaFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "LAREA") Then
            AreaFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "LAREA")
        Else
            'need to add it
            AreaFieldIndex = GisUtil.AddField(StreamsLayerIndex, "LAREA", 2, 10)
        End If
        If AreaFieldIndex < minfield Then minfield = AreaFieldIndex
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            rval = GisUtil.FieldValue(StreamsLayerIndex, i - 1, ReachSubbasinFieldIndex)
            For j = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
                dval = GisUtil.FieldValue(SubbasinLayerIndex, j - 1, SubbasinFieldIndex)
                If dval = rval Then
                    r = GisUtil.FeatureArea(SubbasinLayerIndex, j - 1)
                    GisUtil.SetFeatureValue(StreamsLayerIndex, AreaFieldIndex, i - 1, r)
                    Exit For
                End If
            Next j
        Next i

        'set total contributing area of stream reach
        Dim bfound As Boolean
        Dim r2 As Double
        Dim tAreaFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "TAREA") Then
            tAreaFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "TAREA")
        Else
            'need to add it
            tAreaFieldIndex = GisUtil.AddField(StreamsLayerIndex, "TAREA", 2, 10)
        End If
        If tAreaFieldIndex < minfield Then minfield = tAreaFieldIndex
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            r = GisUtil.FieldValue(StreamsLayerIndex, i - 1, AreaFieldIndex)
            GisUtil.SetFeatureValue(StreamsLayerIndex, tAreaFieldIndex, i - 1, r)
        Next i
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(StreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            'is there anything downstream of this one?
            dval = GisUtil.FieldValue(StreamsLayerIndex, i - 1, DownstreamFieldIndex)
            Do While dval > 0
                bfound = False
                For j = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                    rval = GisUtil.FieldValue(StreamsLayerIndex, j - 1, ReachSubbasinFieldIndex)
                    If rval = dval Then
                        'this is the one
                        r = GisUtil.FieldValue(StreamsLayerIndex, j - 1, tAreaFieldIndex)
                        r2 = GisUtil.FieldValue(StreamsLayerIndex, i - 1, tAreaFieldIndex)
                        GisUtil.SetFeatureValue(StreamsLayerIndex, tAreaFieldIndex, j - 1, r + r2)
                        dval = GisUtil.FieldValue(StreamsLayerIndex, j - 1, DownstreamFieldIndex)
                        Logger.Dbg("ManDelin:" & dval & " downstream of " & rval)
                        bfound = True
                        Exit For
                    End If
                Next j
                If Not bfound Then
                    dval = 0
                End If
            Loop
        Next i
        'add total contributing area in acres and square miles
        Dim AreaAcresFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "TAREAACRES") Then
            AreaAcresFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "TAREAACRES")
        Else
            'need to add it
            AreaAcresFieldIndex = GisUtil.AddField(StreamsLayerIndex, "TAREAACRES", 2, 10)
        End If
        Dim AreaMi2FieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "TAREAMI2") Then
            AreaMi2FieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "TAREAMI2")
        Else
            'need to add it
            AreaMi2FieldIndex = GisUtil.AddField(StreamsLayerIndex, "TAREAMI2", 2, 10)
        End If
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            r = GisUtil.FieldValue(StreamsLayerIndex, i - 1, tAreaFieldIndex)
            GisUtil.SetFeatureValue(StreamsLayerIndex, AreaAcresFieldIndex, i - 1, r / 4046.86)
            GisUtil.SetFeatureValue(StreamsLayerIndex, AreaMi2FieldIndex, i - 1, r / 2589988)
        Next i

        'set stream width based on upstream area
        If GisUtil.IsField(StreamsLayerIndex, "WID2") Then
            TempFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "WID2")
        Else
            'need to add it
            TempFieldIndex = GisUtil.AddField(StreamsLayerIndex, "WID2", 2, 10)
        End If
        If TempFieldIndex < minfield Then minfield = TempFieldIndex
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            r = GisUtil.FieldValue(StreamsLayerIndex, i - 1, tAreaFieldIndex)
            r2 = (1.29) * ((r / 1000000) ^ (0.6))
            GisUtil.SetFeatureValue(StreamsLayerIndex, TempFieldIndex, i - 1, r2)
        Next i

        'set depth based on upstream area
        If GisUtil.IsField(StreamsLayerIndex, "DEP2") Then
            TempFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "DEP2")
        Else
            'need to add it
            TempFieldIndex = GisUtil.AddField(StreamsLayerIndex, "DEP2", 2, 10)
        End If
        If TempFieldIndex < minfield Then minfield = TempFieldIndex
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            r = GisUtil.FieldValue(StreamsLayerIndex, i - 1, tAreaFieldIndex)
            r2 = (0.13) * ((r / 1000000) ^ (0.4))
            GisUtil.SetFeatureValue(StreamsLayerIndex, TempFieldIndex, i - 1, r2)
        Next i

        'set min elev
        Dim MinFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "MINEL") Then
            MinFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "MINEL")
        Else
            'need to add it
            MinFieldIndex = GisUtil.AddField(StreamsLayerIndex, "MINEL", 1, 10)
        End If
        If MinFieldIndex < minfield Then minfield = MinFieldIndex
        'set max elev
        Dim MaxFieldIndex As Integer
        If GisUtil.IsField(StreamsLayerIndex, "MAXEL") Then
            MaxFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "MAXEL")
        Else
            'need to add it
            MaxFieldIndex = GisUtil.AddField(StreamsLayerIndex, "MAXEL", 1, 10)
        End If
        If MaxFieldIndex < minfield Then minfield = MaxFieldIndex
        Dim ElevationThemeName As String
        Dim ElevationLayerIndex As Integer
        Dim ElevationFieldIndex As Integer
        Dim x1 As Double
        Dim x2 As Double
        Dim y1 As Double
        Dim y2 As Double
        Dim gmin As Integer
        Dim gmax As Integer
        Dim gtemp As Integer
        ElevationThemeName = cboDEM.Items(cboDEM.SelectedIndex)
        ElevationLayerIndex = GisUtil.LayerIndex(ElevationThemeName)
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            'return end points of stream segment
            GisUtil.EndPointsOfLine(StreamsLayerIndex, i - 1, x1, y1, x2, y2)
            If GisUtil.LayerType(ElevationLayerIndex) = 3 Then
                'get shapefile value at point
                j = GisUtil.PointInPolygonXY(x1, y1, ElevationLayerIndex)
                ElevationFieldIndex = GisUtil.FieldIndex(ElevationLayerIndex, "ELEV_M")
                gmin = GisUtil.FieldValue(ElevationLayerIndex, j, ElevationFieldIndex)
                j = GisUtil.PointInPolygonXY(x2, y2, ElevationLayerIndex)
                gmax = GisUtil.FieldValue(ElevationLayerIndex, j, ElevationFieldIndex)
            Else
                'get grid value at point
                gmin = GisUtil.GridValueAtPoint(ElevationLayerIndex, x1, y1)
                gmax = GisUtil.GridValueAtPoint(ElevationLayerIndex, x2, y2)
                If InStr(GisUtil.LayerFileName(ElevationLayerIndex), "\ned\") > 0 Then
                    'this is an ned grid (in cm), convert to meters
                    gmin = gmin / 100
                    gmax = gmax / 100
                End If
            End If
            If gmax < gmin Then
                gtemp = gmin
                gmin = gmax
                gmax = gtemp
            End If
            GisUtil.SetFeatureValue(StreamsLayerIndex, MinFieldIndex, i - 1, gmin)
            GisUtil.SetFeatureValue(StreamsLayerIndex, MaxFieldIndex, i - 1, gmax)
        Next i

        'set slope of stream reach
        If GisUtil.IsField(StreamsLayerIndex, "SLO2") Then
            TempFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "SLO2")
        Else
            'need to add it
            TempFieldIndex = GisUtil.AddField(StreamsLayerIndex, "SLO2", 2, 10)
        End If
        If TempFieldIndex < minfield Then minfield = TempFieldIndex
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            gmin = GisUtil.FieldValue(StreamsLayerIndex, i - 1, MinFieldIndex)
            gmax = GisUtil.FieldValue(StreamsLayerIndex, i - 1, MaxFieldIndex)
            gtemp = GisUtil.FieldValue(StreamsLayerIndex, i - 1, LengthFieldIndex)
            GisUtil.SetFeatureValue(StreamsLayerIndex, TempFieldIndex, i - 1, (gmax - gmin) * 100 / gtemp)
        Next i

        'set name of each stream reach
        If GisUtil.IsField(StreamsLayerIndex, "SNAME") Then
            TempFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "SNAME")
        Else
            'need to add it
            TempFieldIndex = GisUtil.AddField(StreamsLayerIndex, "SNAME", 0, 20)
        End If
        If TempFieldIndex < minfield Then minfield = TempFieldIndex
        Dim NameFieldIndex As Integer
        Dim Name As String
        If GisUtil.IsField(StreamsLayerIndex, "PNAME") Then
            NameFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "PNAME")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "NAME") Then
            NameFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "NAME")
        ElseIf GisUtil.IsField(StreamsLayerIndex, "GNIS_NAME") Then
            NameFieldIndex = GisUtil.FieldIndex(StreamsLayerIndex, "GNIS_NAME")
        End If
        If NameFieldIndex > -1 Then
            For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                Name = GisUtil.FieldValue(StreamsLayerIndex, i - 1, NameFieldIndex)
                GisUtil.SetFeatureValue(StreamsLayerIndex, TempFieldIndex, i - 1, Name)
            Next i
        End If
        'add name to subbasin layer as well
        If GisUtil.IsField(SubbasinLayerIndex, "BNAME") Then
            NameFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "BNAME")
        Else
            'need to add it
            NameFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "BNAME", 0, 20)
        End If
        For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
            dval = GisUtil.FieldValue(SubbasinLayerIndex, i - 1, SubbasinFieldIndex)
            For j = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
                rval = GisUtil.FieldValue(StreamsLayerIndex, j - 1, ReachSubbasinFieldIndex)
                If rval = dval Then
                    'this is the one
                    If Len(Trim(GisUtil.FieldValue(SubbasinLayerIndex, i - 1, NameFieldIndex))) = 0 Then
                        GisUtil.SetFeatureValue(SubbasinLayerIndex, NameFieldIndex, i - 1, GisUtil.FieldValue(StreamsLayerIndex, j - 1, TempFieldIndex))
                        Exit For
                    End If
                End If
            Next j
        Next i

        'remove unwanted fields
        For i = 1 To minfield
            GisUtil.RemoveField(StreamsLayerIndex, 0)
        Next i

        'now add outlets
        lblDefine.Text = "Creating outlets..."
        Me.Refresh()

        'create new outlets shapefile
        i = 1
        Dim outputpath As String
        Dim OutletShapefile As String
        Dim success As Boolean
        outputpath = PathNameOnly(GisUtil.LayerFileName(StreamsLayerIndex))
        OutletShapefile = outputpath & "\outlets" & i & ".shp"
        Do While FileExists(OutletShapefile)
            i = i + 1
            OutletShapefile = outputpath & "\outlets" & i & ".shp"
        Loop
        'add points to the shapefile
        Dim lShapefile As New MapWinGIS.Shapefile
        success = lShapefile.CreateNew(OutletShapefile, MapWinGIS.ShpfileType.SHP_POINT)
        success = lShapefile.StartEditingShapes(True)

        'Add ID Field 
        Dim [of] As New MapWinGIS.Field
        [of].Name = "ID"
        [of].Type = MapWinGIS.FieldType.INTEGER_FIELD
        [of].Width = 10
        success = lShapefile.EditInsertField([of], lShapefile.NumFields)
        [of] = Nothing
        'Add PCSID Field 
        Dim of2 As New MapWinGIS.Field
        of2.Name = "PCSID"
        of2.Type = MapWinGIS.FieldType.STRING_FIELD
        of2.Width = 10
        success = lShapefile.EditInsertField(of2, lShapefile.NumFields)
        'Add Xpr Field 
        Dim of3 As New MapWinGIS.Field
        of3.Name = "Xpr"
        of3.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        of3.Width = 10
        success = lShapefile.EditInsertField(of3, lShapefile.NumFields)
        'Add Ypr Field 
        Dim of4 As New MapWinGIS.Field
        of4.Name = "Ypr"
        of4.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        of4.Width = 10
        success = lShapefile.EditInsertField(of4, lShapefile.NumFields)

        'add points at each stream outlet
        For i = 1 To GisUtil.NumFeatures(StreamsLayerIndex)
            Dim lShape As New MapWinGIS.Shape
            Dim lPoint As New MapWinGIS.Point
            lShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
            GisUtil.EndPointsOfLine(StreamsLayerIndex, i - 1, x1, y1, x2, y2)
            If GisUtil.FieldName(rfield, ReachLayerIndex) = "COMID" Then
                'cheap way to see if this is nhdplus way of digitizing
                lPoint.x = x2
                lPoint.y = y2
            Else
                lPoint.x = x1
                lPoint.y = y1
            End If
            lShape.InsertPoint(lPoint, 0)
            success = lShapefile.EditInsertShape(lShape, lShapefile.NumShapes)
            success = lShapefile.EditCellValue(2, lShapefile.NumShapes - 1, lPoint.x)
            success = lShapefile.EditCellValue(3, lShapefile.NumShapes - 1, lPoint.y)
            lPoint = Nothing
            lShape = Nothing
        Next i

        'add pcs points if checked
        Dim pcsLayerIndex As Integer
        Dim pcsFieldindex As Integer
        Dim pcsid As String
        If cbxPCS.Checked = True Then
            pcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
            pcsFieldindex = GisUtil.FieldIndex(pcsLayerIndex, "NPDES")
            For i = 1 To GisUtil.NumFeatures(pcsLayerIndex)
                GisUtil.PointXY(pcsLayerIndex, i - 1, x1, y1)
                If GisUtil.PointInPolygonXY(x1, y1, SubbasinLayerIndex) > -1 Then
                    pcsid = GisUtil.FieldValue(pcsLayerIndex, i - 1, pcsFieldindex)
                    Dim lShape As New MapWinGIS.Shape
                    Dim lPoint As New MapWinGIS.Point
                    lShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
                    lPoint.x = x1
                    lPoint.y = y1
                    lShape.InsertPoint(lPoint, 0)
                    success = lShapefile.EditInsertShape(lShape, lShapefile.NumShapes)
                    success = lShapefile.EditCellValue(1, lShapefile.NumShapes - 1, pcsid)
                    success = lShapefile.EditCellValue(2, lShapefile.NumShapes - 1, x1)
                    success = lShapefile.EditCellValue(3, lShapefile.NumShapes - 1, y1)
                    lPoint = Nothing
                    lShape = Nothing
                End If
            Next i
        End If

        'Populate ID field
        For i = 1 To lShapefile.NumShapes
            success = lShapefile.EditCellValue(0, i - 1, i)
        Next i
        success = lShapefile.StopEditingShapes(True, True)
        success = lShapefile.Close()

        'add outlets layer to the map
        If GisUtil.IsLayer("Outlets") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Outlets"))
        End If
        GisUtil.AddLayer(OutletShapefile, "Outlets")
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).Color = System.Drawing.Color.Cyan
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).OutlineColor = System.Drawing.Color.Cyan
        pMapWin.Layers(pMapWin.Layers.GetHandle(pMapWin.Layers.NumLayers - 1)).LineOrPointSize = 5

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        lblDefine.Text = ""
        lblDefine.Visible = False
        Me.Refresh()

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

End Class
