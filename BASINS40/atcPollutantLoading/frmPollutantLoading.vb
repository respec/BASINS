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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboLanduse As System.Windows.Forms.ComboBox
    Friend WithEvents cboSubbasins As System.Windows.Forms.ComboBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cboLandUseLayer As System.Windows.Forms.ComboBox
    Friend WithEvents lblLandUseLayer As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents cmdAbout As System.Windows.Forms.Button
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents cmdEC As System.Windows.Forms.Button
    Friend WithEvents lblEC As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents AtcGridEC As atcControls.atcGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ofdEC As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelSetup))
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.cboSubbasins = New System.Windows.Forms.ComboBox
        Me.cboLanduse = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.cboLandUseLayer = New System.Windows.Forms.ComboBox
        Me.lblLandUseLayer = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdEC = New System.Windows.Forms.Button
        Me.lblEC = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.AtcGridEC = New atcControls.atcGrid
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdHelp = New System.Windows.Forms.Button
        Me.cmdAbout = New System.Windows.Forms.Button
        Me.ofdEC = New System.Windows.Forms.OpenFileDialog
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(60, 21)
        Me.TabControl1.Location = New System.Drawing.Point(16, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(528, 258)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.RadioButton2)
        Me.TabPage1.Controls.Add(Me.RadioButton1)
        Me.TabPage1.Controls.Add(Me.cboSubbasins)
        Me.TabPage1.Controls.Add(Me.cboLanduse)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(520, 229)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(27, 138)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(162, 21)
        Me.RadioButton2.TabIndex = 10
        Me.RadioButton2.Text = "Simple (EMC) Method"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Location = New System.Drawing.Point(27, 110)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(187, 21)
        Me.RadioButton1.TabIndex = 9
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Export Coefficient Method"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'cboSubbasins
        '
        Me.cboSubbasins.AllowDrop = True
        Me.cboSubbasins.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSubbasins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubbasins.Location = New System.Drawing.Point(200, 26)
        Me.cboSubbasins.Name = "cboSubbasins"
        Me.cboSubbasins.Size = New System.Drawing.Size(240, 24)
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
        Me.cboLanduse.Size = New System.Drawing.Size(240, 24)
        Me.cboLanduse.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(24, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 24)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Subbasins Layer:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(24, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(152, 24)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Land Use Type:"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.cboLandUseLayer)
        Me.TabPage2.Controls.Add(Me.lblLandUseLayer)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(520, 229)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Land Use"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'cboLandUseLayer
        '
        Me.cboLandUseLayer.AllowDrop = True
        Me.cboLandUseLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboLandUseLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLandUseLayer.Location = New System.Drawing.Point(200, 32)
        Me.cboLandUseLayer.Name = "cboLandUseLayer"
        Me.cboLandUseLayer.Size = New System.Drawing.Size(240, 24)
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
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(520, 229)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Point Sources"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 25)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(520, 229)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "BMPs"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Label1)
        Me.TabPage5.Controls.Add(Me.cmdEC)
        Me.TabPage5.Controls.Add(Me.lblEC)
        Me.TabPage5.Controls.Add(Me.Label4)
        Me.TabPage5.Controls.Add(Me.AtcGridEC)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(520, 229)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Export Coefficients"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 17)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "(lbs/ac/yr)"
        '
        'cmdEC
        '
        Me.cmdEC.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdEC.Location = New System.Drawing.Point(427, 12)
        Me.cmdEC.Name = "cmdEC"
        Me.cmdEC.Size = New System.Drawing.Size(73, 24)
        Me.cmdEC.TabIndex = 22
        Me.cmdEC.Text = "Change"
        '
        'lblEC
        '
        Me.lblEC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEC.Location = New System.Drawing.Point(176, 16)
        Me.lblEC.Name = "lblEC"
        Me.lblEC.Size = New System.Drawing.Size(243, 16)
        Me.lblEC.TabIndex = 21
        Me.lblEC.Text = "<none>"
        Me.lblEC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(17, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(153, 16)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Export Coefficient File:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AtcGridEC
        '
        Me.AtcGridEC.AllowHorizontalScrolling = True
        Me.AtcGridEC.AllowNewValidValues = False
        Me.AtcGridEC.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AtcGridEC.CellBackColor = System.Drawing.Color.Empty
        Me.AtcGridEC.LineColor = System.Drawing.Color.Empty
        Me.AtcGridEC.LineWidth = 0.0!
        Me.AtcGridEC.Location = New System.Drawing.Point(20, 54)
        Me.AtcGridEC.Name = "AtcGridEC"
        Me.AtcGridEC.Size = New System.Drawing.Size(480, 159)
        Me.AtcGridEC.Source = Nothing
        Me.AtcGridEC.TabIndex = 19
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(16, 292)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(103, 32)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "&Generate"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Location = New System.Drawing.Point(125, 292)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(88, 32)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdHelp.Location = New System.Drawing.Point(368, 292)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(80, 32)
        Me.cmdHelp.TabIndex = 6
        Me.cmdHelp.Text = "&Help"
        '
        'cmdAbout
        '
        Me.cmdAbout.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAbout.Location = New System.Drawing.Point(456, 292)
        Me.cmdAbout.Name = "cmdAbout"
        Me.cmdAbout.Size = New System.Drawing.Size(88, 32)
        Me.cmdAbout.TabIndex = 7
        Me.cmdAbout.Text = "&About"
        '
        'ofdEC
        '
        Me.ofdEC.DefaultExt = "dbf"
        Me.ofdEC.Filter = "DBF Files (*.dbf)|*.dbf"
        Me.ofdEC.Title = "Select Export Coefficient File"
        '
        'frmModelSetup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(560, 337)
        Me.Controls.Add(Me.cmdAbout)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmModelSetup"
        Me.Text = "BASINS Pollutant Loading Estimator"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboLanduse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLanduse.SelectedIndexChanged
        SetECGrid()
    End Sub

    Private Sub SetECGrid()
        Dim i As Long, k As Long
        Dim tmpDbf As IatcTable
        Dim lSorted As New atcCollection

        If AtcGridEC.Source Is Nothing Then Exit Sub

        If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
            lblEC.Text = "\BASINS\etc\ecgiras.dbf"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "Other Shapefile" Then
            lblEC.Text = "\BASINS\etc\ecgiras.dbf"
        ElseIf cboLanduse.Items(cboLanduse.SelectedIndex) = "NLCD Grid" Then
            lblEC.Text = "\BASINS\etc\ecnlcd.dbf"
        Else 'grid
            lblEC.Text = "\BASINS\etc\ecnlcd.dbf"
        End If
        AtcGridEC.Clear()

        If lblEC.Text <> "<none>" Then
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lblEC.Text)

            With AtcGridEC.Source
                .Rows = 1
                .Columns = tmpDbf.NumFields
                .ColorCells = True
                .FixedRows = 1
                .FixedColumns = 1
                For i = 1 To tmpDbf.NumFields
                    .CellValue(0, i) = tmpDbf.FieldName(i)
                    .CellColor(0, i) = SystemColors.ControlDark
                Next i

                For k = 1 To tmpDbf.NumRecords
                    tmpDbf.CurrentRecord = k
                    For i = 1 To tmpDbf.NumFields
                        .CellValue(k, i) = tmpDbf.Value(i)
                        If i > 2 Then
                            .CellEditable(k, i) = True
                        Else
                            .CellColor(k, i) = SystemColors.ControlDark
                        End If
                    Next i
                Next k
            End With

        End If
        AtcGridEC.SizeAllColumnsToContents()
        AtcGridEC.Refresh()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        MsgBox("BASINS Pollutant Loading Estimator for MapWindow" & vbCrLf & vbCrLf & "Version 1.0", , "BASINS")
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        'MsgBox("Help is not yet implemented.", MsgBoxStyle.Critical, "BASINS " & pModelName & " Problem")
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim i As Integer, j As Integer, k As Integer
        Dim lSubbasinLayerName As String
        Dim lSubbasinLayerIndex As Integer
        Dim lucode As Integer
        Dim lProblem As String

        If lblEC.Text <> "<none>" Then

            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            lSubbasinLayerName = cboSubbasins.Items(cboSubbasins.SelectedIndex)
            lSubbasinLayerIndex = GisUtil.LayerIndex(lSubbasinLayerName)

            'are any areas selected?
            Dim cSelectedAreaIndexes As New Collection
            For i = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
                'add selected areas to the collection
                cSelectedAreaIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSubbasinLayerIndex))
            Next
            If cSelectedAreaIndexes.Count = 0 Then
                'no areas selected, act as if all are selected
                For i = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                    cSelectedAreaIndexes.Add(i - 1)
                Next
            End If

            'build array for output area values (area of each landuse in each subbasin)
            Dim lmaxlu As Integer = AtcGridEC.Source.CellValue(AtcGridEC.Source.Rows - 1, 1)
            Dim aAreaLS(lmaxlu, cSelectedAreaIndexes.Count) As Double

            'build array of constituent names
            Dim aConsNames(AtcGridEC.Source.Columns - 4) As String
            For i = 0 To AtcGridEC.Source.Columns - 4
                aConsNames(i) = AtcGridEC.Source.CellValue(0, i + 3)
            Next i

            'build array for each export coeff for each land use type
            Dim aCoeffLC(lmaxlu, AtcGridEC.Source.Columns - 4) As Double
            For j = 1 To AtcGridEC.Source.Rows - 1
                lucode = AtcGridEC.Source.CellValue(j, 1)
                For i = 0 To AtcGridEC.Source.Columns - 4
                    aCoeffLC(lucode, i) = AtcGridEC.Source.CellValue(j, i + 3)
                Next i
            Next j

            'build array for output load values (load for each subbasin and constituent)
            Dim aLoadSC(cSelectedAreaIndexes.Count, aConsNames.GetUpperBound(0)) As Double
            'build array for area of each subbasin 
            Dim aAreaS(cSelectedAreaIndexes.Count) As Double
            'build array for per acre load values (load for each subbasin and constituent)
            Dim aLoadPerAcreSC(cSelectedAreaIndexes.Count, aConsNames.GetUpperBound(0)) As Double

            If RadioButton1.Checked = True Then 'Export Coefficients Method
                'calculate areas of each land use in each subbasin
                If cboLanduse.Items(cboLanduse.SelectedIndex) = "USGS GIRAS Shapefile" Then
                    GIRASAreas(lSubbasinLayerIndex, cSelectedAreaIndexes, aAreaLS)
                    ' / 4046.8564 to convert from m2 to acres
                End If
                'calculate loads
                For i = 0 To cSelectedAreaIndexes.Count - 1 'for each subbasin
                    For j = 0 To aConsNames.GetUpperBound(0)  'for each constituent
                        For k = 1 To lmaxlu
                            aLoadSC(i, j) = aLoadSC(i, j) + (aCoeffLC(k, j) * aAreaLS(k, i) / 4046.8564)
                        Next k
                    Next j
                Next i
                'calculate areas of each subbasin
                For i = 0 To cSelectedAreaIndexes.Count - 1 'for each subbasin
                    For k = 1 To lmaxlu
                        aAreaS(i) = aAreaS(i) + (aAreaLS(k, i) / 4046.8564)
                    Next k
                Next i
                'calculate loads per acre
                For i = 0 To cSelectedAreaIndexes.Count - 1 'for each subbasin
                    If aAreaS(i) > 0 Then
                        For j = 0 To aConsNames.GetUpperBound(0)  'for each constituent
                            aLoadPerAcreSC(i, j) = aLoadSC(i, j) / aAreaS(i)
                        Next j
                    End If
                Next i

                'add group to map for output 
                Dim lGroupName As String = "Estimated Annual Pollutant Loads"
                GisUtil.AddGroup(lGroupName)

                'now build the output shapefiles 
                Dim lOutputShapefileName As String
                Dim lLayerDesc As String
                For k = 1 To 2
                    lOutputShapefileName = ""
                    GisUtil.SaveSelectedFeatures(lSubbasinLayerIndex, cSelectedAreaIndexes, _
                                                 lOutputShapefileName)

                    'add the output shapefile to the map
                    If k = 1 Then
                        lLayerDesc = "Per Acre Load (lbs)"
                    Else
                        lLayerDesc = "Total Load (lbs)"
                    End If
                    Dim basename As String = lLayerDesc
                    i = 0
                    Do While GisUtil.IsLayer(lLayerDesc)
                        i = i + 1
                        lLayerDesc = basename & " " & i
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
                    For j = 0 To aConsNames.GetUpperBound(0)  'for each constituent
                        Try
                            lFieldIndex = GisUtil.FieldIndex(lOutputLayerIndex, aConsNames(j) & "_load")
                        Catch
                            lFieldIndex = GisUtil.AddField(lOutputLayerIndex, aConsNames(j) & "_load", 2, 10)
                        End Try
                        For i = 0 To cSelectedAreaIndexes.Count - 1 'for each subbasin
                            If k = 1 Then
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, aLoadPerAcreSC(i, j))
                            Else
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, aLoadSC(i, j))
                            End If
                        Next i
                    Next j
                    GisUtil.StopSetFeatureValue(lOutputLayerIndex)
                Next k

                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

            End If

            Me.Close()

        End If
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        cmdOK.Enabled = b
        cmdHelp.Enabled = b
        cmdCancel.Enabled = b
        cmdAbout.Enabled = b
        TabControl1.Enabled = b
    End Sub

    Public Sub InitializeUI()
        Dim ctemp As String

        cboLanduse.Items.Add("USGS GIRAS Shapefile")
        cboLanduse.Items.Add("NLCD Grid")
        cboLanduse.Items.Add("Other Shapefile")
        cboLanduse.Items.Add("User Grid")

        Dim lyr As Long
        For lyr = 0 To GisUtil.NumLayers() - 1
            ctemp = GisUtil.LayerName(lyr)
            If GisUtil.LayerType(lyr) = 3 Then
                'PolygonShapefile 
                cboSubbasins.Items.Add(ctemp)
                If UCase(ctemp) = "SUBBASINS" Or InStr(ctemp, "Watershed Shapefile") > 0 Then
                    cboSubbasins.SelectedIndex = cboSubbasins.Items.Count - 1
                End If
            ElseIf GisUtil.LayerType(lyr) = 1 Then
                'PointShapefile
                'cboOutlets.Items.Add(ctemp)
                'If UCase(ctemp) = "OUTLETS" Then
                '    cboOutlets.SelectedIndex = cboOutlets.Items.Count - 1
                'End If
            End If
        Next
        If cboSubbasins.Items.Count > 0 And cboSubbasins.SelectedIndex < 0 Then
            cboSubbasins.SelectedIndex = 0
        End If
        'If cboOutlets.Items.Count > 0 And cboOutlets.SelectedIndex < 0 Then
        '    cboOutlets.SelectedIndex = 0
        'End If

        With AtcGridEC
            .Source = New atcControls.atcGridSource
            .Font = New Font(.Font, FontStyle.Regular)
            .AllowHorizontalScrolling = True
        End With

        cboLanduse.SelectedIndex = 0
    End Sub

    Private Sub cmdEC_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ofdEC.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblEC.Text = ofdEC.FileName
            SetECGrid()
        End If
    End Sub

    Private Sub frmPollutantLoading_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPF.html")
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            TabPage5.Text = "Export Coefficients"
        Else
            TabPage5.Text = "Event Mean Concentrations"
        End If
    End Sub

    Private Sub GIRASAreas(ByVal aAreaLayerIndex As Integer, _
                           ByVal aSelectedAreaIndexes As Collection, _
                           ByRef aAreaLandSub(,) As Double)

        Dim lProblem As String = ""
        Dim lLanduseLayerIndex As Long
        Dim lLandUseFieldIndex As Long
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
                Dim lbfirst As Boolean = True
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