Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMwGisUtility
Imports atcUtility
Imports atcWASP

Module WaspPlugInTester
    Private Const pBaseDrive As String = "D:"
    Private Const pBaseFolder As String = pBaseDrive & "\Basins\data\02060006\nhdplus02060006\hydrography\"
    Private pSegmentLayerName As String = pBaseFolder & "nhdflowline.shp"
    Private pSelectedFeatureIndicesArray() As Integer = {565, 570, 571, 581, 799}
    Private pSelectedFeatureIndices As New ArrayList(pSelectedFeatureIndicesArray)
    Private pMaxTravelTime As Double = 0.25 'days
    Private pMinTravelTime As Double = 0.1 'days

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)
        Logger.Dbg("StartWaspPlugInTester " & CurDir())

        If Not GisUtil.IsLayer(pSegmentLayerName) Then
            GisUtil.AddLayer(pSegmentLayerName, "FlowLines")
        End If
        Dim lSegmentLayerIndex As Integer = GisUtil.LayerIndex(pSegmentLayerName)
        GisUtil.ClearSelectedFeatures(lSegmentLayerIndex)
        For Each lSelectedFeatureIndex As Integer In pSelectedFeatureIndices
            GisUtil.SetSelectedFeature(lSegmentLayerIndex, lSelectedFeatureIndex)
        Next

        Dim lWasp As New atcWASPProject
        With lWasp
            .Name = "BatchWASPTest"
            .SJDate = Jday(2000, 1, 1, 0, 0, 0)
            .EJDate = Jday(2000, 12, 31, 0, 0, 0)

            '.GenerateSegments(lSegmentLayerIndex, pMaxTravelTime, pMinTravelTime)
            .Save("Junk" & pMinTravelTime & ".txt")

            pMinTravelTime = 0.05
            '.GenerateSegments(lSegmentLayerIndex, pMaxTravelTime, pMinTravelTime)
            .Save("Junk" & pMinTravelTime & ".txt")
        End With
    End Sub
End Module
