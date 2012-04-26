Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Data

Public Class frmAddWatershed
    Friend pWatershedIDs As atcCollection
    Friend pCurrentIndex As Integer
    Friend pGrid As atcControls.atcGrid
    <CLSCompliant(False)> Friend Database As atcUtility.atcMDB

    Public Sub InitializeUI(ByVal aDatabase As atcUtility.atcMDB, ByVal aGrid As atcControls.atcGrid)
        Database = aDatabase
        Dim lX As Double = ((GisUtil.MapExtentXmax - GisUtil.MapExtentXmin) / 2) + GisUtil.MapExtentXmin
        Dim lY As Double = ((GisUtil.MapExtentYmax - GisUtil.MapExtentYmin) / 2) + GisUtil.MapExtentYmin
        GisUtil.ProjectPoint(lX, lY, GisUtil.ProjectProjection, "+proj=longlat +datum=NAD83")
        txtLatitude.Text = Math.Round(lY, 5)
        txtLongitude.Text = Math.Round(lX, 5)
        pGrid = aGrid
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lProject As String = txtProject.Text
        Dim lHuc As String = txtHuc.Text
        Dim lArea As String = txtArea.Text
        Dim lComments As String = txtComments.Text
        If lComments.Length = 0 Then lComments = " "
        Dim lLocation As String = txtLocation.Text
        Dim lSetting As String = txtSetting.Text
        Dim lWeather As String = txtWeather.Text
        Dim lLatitude As String = txtLatitude.Text
        Dim lLongitude As String = txtLongitude.Text
        'check values
        If lProject.Trim.Length = 0 Or lHuc.Trim.Length = 0 Or lArea.Trim.Length = 0 Or lLocation.Trim.Length = 0 Or lSetting.Trim.Length = 0 Or lWeather.Trim.Length = 0 Then
            Logger.Msg("One or more required fields does not have a value.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If
        If Not IsNumeric(lLatitude) Or Not IsNumeric(lLongitude) Then
            Logger.Msg("Latitude and Longitude fields must contain numeric values.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If
        Dim lX As Double = CDbl(lLongitude)
        Dim lY As Double = CDbl(lLatitude)
        GisUtil.ProjectPoint(lX, lY, "+proj=longlat +datum=NAD83", GisUtil.ProjectProjection)
        'find next available id number 
        Dim lTable As DataTable = Database.GetTable("WatershedData")
        Dim lMaxId As Integer = 0
        Dim lTmpId As Integer = 0
        For lRow As Integer = 0 To lTable.Rows.Count - 1
            lTmpId = lTable.Rows(lRow).Item(0).ToString
            If lTmpId > lMaxId Then
                lMaxId = lTmpId
            End If
        Next
        'save to database
        Dim lValues As New Collection
        lValues.Add(lMaxId + 1)
        lValues.Add("'" & lProject & "'")
        lValues.Add("'" & lLocation & "'")
        lValues.Add("'" & lSetting & "'")
        lValues.Add("'" & lWeather & "'")
        lValues.Add("'" & lArea & "'")
        lValues.Add("'" & lHuc & "'")
        lValues.Add(lLatitude)
        lValues.Add(lLongitude)
        lValues.Add(lX)
        lValues.Add(lY)
        lValues.Add("'" & lComments & "'")
        Database.InsertRowIntoTable("WatershedData", lValues)
        'save to shapefile
        If GisUtil.IsLayer("Watershed") Then
            Dim lLayerIndex As Integer = GisUtil.LayerIndex("Watershed")
            GisUtil.AddPoint(lLayerIndex, lX, lY)
            Dim lNumFeature As Integer = GisUtil.NumFeatures(lLayerIndex) - 1
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "ID"), lNumFeature, lMaxId + 1)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WATERSNAME"), lNumFeature, lProject)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "HUC"), lNumFeature, lHuc)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "LOCATION"), lNumFeature, lLocation)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "DRAINAGEAR"), lNumFeature, lArea)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "COMMENTS"), lNumFeature, lComments)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "PHYS"), lNumFeature, lSetting)
            GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WEATHER"), lNumFeature, lWeather)
            With pGrid.Source
                Dim lRow As Integer = .Rows
                .CellValue(lRow, 0) = lMaxId + 1
                .CellValue(lRow, 1) = lProject
                .CellValue(lRow, 2) = lHuc
                .CellValue(lRow, 3) = lLocation
                .CellValue(lRow, 4) = lArea
                .CellValue(lRow, 5) = lComments
                .CellValue(lRow, 6) = lSetting
                .CellValue(lRow, 7) = lWeather
            End With
            pGrid.Refresh()
        End If
        Me.Close()
    End Sub
End Class