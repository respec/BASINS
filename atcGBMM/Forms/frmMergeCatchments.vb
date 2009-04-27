Imports atcMwGisUtility

Public Class frmMergeCatchments

    Private Sub frmMergeCatchments_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MergeForm = Nothing
        SaveWindowPos(REGAPP, Me)
    End Sub

    ''' <summary>
    ''' Initialize form: mainly fill combo boxes with lists of polygon shape files
    ''' </summary>
    Private Sub frmMergeCatchments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPP, Me)
        For i As Integer = 0 To GisUtil.NumLayers - 1
            Dim ln As String = GisUtil.LayerName(i)
            Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(i)
            If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                cboCatchmentsLayer.Items.Add(ln)
                cboSubbasinLayer.Items.Add(ln)
                If ln.ToLower.Contains("catchment") Then cboCatchmentsLayer.Text = ln
                If ln = MercuryForm.cboSubbasinLayer.Text Then cboSubbasinLayer.Text = ln
            End If
        Next
        lblNumShapes.Text = ""
        AddHandler Project.ShapesSelected, AddressOf ShapesSelected
        MergeForm = Me
    End Sub

    ''' <summary>
    ''' Select multiply polygons on catchment layer
    ''' </summary>
    ''' <remarks>If subbasin layer isn't loaded, create new shapefile and load it</remarks>
    Private Sub btnSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        If Not GisUtil.IsLayer(cboSubbasinLayer.Text) Then
            Dim fn As String = String.Format("{0}\{1}.shp", Project.Folders.ProjectFolder, cboSubbasinLayer.Text)
            Dim sfSubbasin As New MapWinGIS.Shapefile
            Dim sfCatchment As New MapWinGIS.Shapefile
            sfCatchment.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(cboCatchmentsLayer.Text)))
            If My.Computer.FileSystem.FileExists(fn) AndAlso Not MapWinGeoProc.DataManagement.DeleteShapefile(fn) Then WarningMsg("Unable to delete " & fn) : Exit Sub
            If Not sfSubbasin.CreateNew(fn, MapWinGIS.ShpfileType.SHP_POLYGON) Then WarningMsg("Unable to create " & fn) : Exit Sub
            sfSubbasin.StartEditingTable()
            For i As Integer = 0 To sfCatchment.NumFields - 1
                If Not sfSubbasin.EditInsertField(sfCatchment.Field(i), i) Then WarningMsg("Unable to edit " & cboSubbasinLayer.Text) : Exit Sub
            Next
            If Not sfSubbasin.StopEditingShapes(True, True) Then WarningMsg("Unable to stop editting " & cboSubbasinLayer.Text) : Exit Sub
            sfSubbasin.Projection = GisUtil.ProjectProjection
            If Not sfSubbasin.Close() Then WarningMsg("Unable to close " & cboSubbasinLayer.Text) : Exit Sub
            GisUtil.AddGroup(GroupName)
            If Not GisUtil.AddLayerToGroup(fn, cboSubbasinLayer.Text, GroupName) Then WarningMsg("Unable to add layer " & cboSubbasinLayer.Text) : Exit Sub
        End If

        lblNumShapes.Text = "Select catchments on map then right-click (or click OK button)..."
        Me.TopMost = True

        If GisUtil.IsLayer(cboCatchmentsLayer.Text) Then
            GisUtil.CurrentLayer = GisUtil.LayerIndex(cboCatchmentsLayer.Text)
            GisUtil.LayerVisible(cboCatchmentsLayer.Text) = True
            GisUtil.ClearSelectedFeatures(GisUtil.CurrentLayer)
        End If
        With gMapWin.View
            .SelectMethod = MapWinGIS.SelectMode.INTERSECTION
            .CursorMode = MapWinGIS.tkCursorMode.cmSelection
        End With
        MapWindowForm.Activate()

    End Sub

    ''' <summary>
    ''' Event handler when clsPlugin reports that shapes have been selected (via clsProject event)
    ''' </summary>
    ''' <param name="NumSelected">Number of selected shapes</param>
    Private Sub ShapesSelected(ByVal NumSelected As Integer)
        lblNumShapes.Text = NumSelected & " shapes selected"
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    ''' <summary>
    ''' Merge all selected shapes on the catchment layer and add to subbasin layer
    ''' </summary>
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            If cboSubbasinLayer.Text = "" Then WarningMsg("You must specify the layer to place the merged shapes on.") : Exit Sub
            Dim idxCat As Integer = GisUtil.LayerIndex(cboCatchmentsLayer.Text)
            If GisUtil.NumSelectedFeatures(idxCat) = 0 Then WarningMsg("No shapes were selected.") : Exit Sub
            If Not GisUtil.IsLayer(cboSubbasinLayer.Text) Then WarningMsg("Subbasin layer doesn't exist.") : Exit Sub
            Dim idxSub As Integer = GisUtil.LayerIndex(cboSubbasinLayer.Text)

            Cursor = Cursors.WaitCursor
            UseWaitCursor = True
            For Each cntl As Control In Controls
                cntl.Enabled = False
            Next
            lblNumShapes.Text = "Processing, please stand by..."
            Application.DoEvents()

            If Not GisUtil.MergeSelectedShapes(idxCat, idxSub) Then WarningMsg("Unable to merge shapes.") : Exit Sub

            For Each cntl As Control In Controls
                cntl.Enabled = True
            Next
            lblNumShapes.Text = ""
            UseWaitCursor = False
            Cursor = Cursors.Default

            'set layer color and opacity
            Dim mwLayer As MapWindow.Interfaces.Layer
            mwLayer = gMapWin.Layers(gMapWin.Layers.GetHandle(GisUtil.LayerIndex(cboSubbasinLayer.Text)))
            With mwLayer
                .ShapeLayerFillTransparency = 0.1
                .Color = Drawing.Color.Blue
                .OutlineColor = Drawing.Color.Blue
                .LineStipple = MapWinGIS.tkLineStipple.lsDashDotDash
                .LineOrPointSize = 3
                .Visible = True
            End With

            With MercuryForm.cboSubbasinLayer
                If Not .Items.Contains(cboSubbasinLayer.Text) Then .Items.Add(cboSubbasinLayer.Text)
                .Text = cboSubbasinLayer.Text
            End With

            Close()
            MercuryForm.Focus()
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            lblNumShapes.Text = ""
            TopMost = False
        End Try
    End Sub
End Class