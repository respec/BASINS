Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptGridValues
    Private pTestPath As String = "D:\GisData\SERDP"
    Private pStepSize As Integer = 2000 'meters

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())
        Dim lString As New Text.StringBuilder
        Dim lPointCollection As atcCollection
        Dim lRatio As Double

        'sample and save points from grid
        For lX As Double = 688000 To 724000 Step pStepSize
            For lY As Double = 3568000 To 3604000 Step pStepSize
                lPointCollection = XY2Z(aMapWin, lX, lY)
                If lPointCollection.Count > 1 Then
                    lString.Append(lX.ToString & vbTab & lY.ToString & vbTab & lPointCollection(0) & vbTab & lPointCollection(1))
                    lRatio = lPointCollection(0) / lPointCollection(1)
                    lString.Append(vbTab & DoubleToString(lRatio))
                    For lIndex As Integer = 3 To lPointCollection.Count
                        lString.Append(vbTab & lPointCollection(lIndex - 1) & vbTab & "???")
                    Next lIndex
                    lString.AppendLine()
                End If
            Next lY
        Next lX
        SaveFileString("GridSample.txt", lString.ToString)

        'sample points going up Upatoi Creek
        Dim lLayer As MapWinGIS.Shapefile = Nothing
        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.LineShapefile Then
                lLayer = aMapWin.Layers(lLayerIndex).GetObject
                If FilenameNoPath(lLayer.Filename) = "UpatoiNHDFlowline.shp" Then
                    'we need to do better than this!!!!!!!
                    Logger.Dbg("Found Upatoi:ShapeCount:" & lLayer.NumShapes)
                    Exit For
                End If
            End If
        Next

        Dim lComIdField As MapWinGIS.Field = lLayer.FieldByName("COMID")
        Dim lToComIdField As MapWinGIS.Field = lLayer.FieldByName("TOCOMID")
        Dim lLength As Double = 0
        Dim lComId As Integer = 3432490 'downstream COMID for Upitoi Creek from visual inspection
        Do While lComId > 0

            lComId = 0
        Loop
    End Sub

    Function XY2Z(ByRef aMapWin As IMapWin, ByVal aX As Double, ByVal aY As Double) As atcCollection
        Dim lZ As Double
        Dim lR, lC As Integer
        Dim lOutCollection As New atcCollection

        For lLayerIndex As Integer = 0 To aMapWin.Layers.NumLayers - 1
            If aMapWin.Layers(lLayerIndex).LayerType = eLayerType.Grid Then
                Dim lGrid As MapWinGIS.Grid = aMapWin.Layers(lLayerIndex).GetGridObject
                lGrid.ProjToCell(aX, aY, lC, lR)
                lZ = lGrid.Value(lC, lR)
                If lZ > 0 Then
                    lOutCollection.Add(lLayerIndex, lZ)
                End If
            End If
        Next
        Return lOutCollection
    End Function
End Module
