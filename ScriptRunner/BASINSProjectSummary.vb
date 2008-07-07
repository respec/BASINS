Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports atcUtility.modFile
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports mapwinGIS

Imports Microsoft.VisualBasic
Imports System

Public Module BASINSProjectSummary
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lLayer As MapWindow.Interfaces.Layer

        Dim lString As New Text.StringBuilder
        lString.AppendLine("BASINS Project Summary" & vbCrLf & _
                              "Name" & vbTab & aMapWin.Project.FileName & vbCrLf & _
                              "Projection" & vbTab & aMapWin.Project.ProjectProjection & vbCrLf & _
                              "Units" & vbTab & aMapWin.Project.MapUnits)

        lString.AppendLine("Layer Count" & vbTab & aMapWin.Layers.NumLayers)

        Dim lLayerDetailString As New Text.StringBuilder
        Dim lOldHeaderHandle As Integer = 0
        For i As Integer = aMapWin.Layers.NumLayers - 1 To 0 Step -1
            lLayer = aMapWin.Layers(i)
            If lLayer.GroupHandle <> lOldHeaderHandle Then
                lOldHeaderHandle = lLayer.GroupHandle
                lLayerDetailString.AppendLine("HeaderHandle " & lOldHeaderHandle & vbCrLf & _
                                               vbTab & "Name" & vbTab & aMapWin.Layers.Groups(lOldHeaderHandle).Text)
            End If
            lLayerDetailString.AppendLine("Layer" & vbTab & i & vbCrLf & _
                                          vbTab & "Name" & vbTab & lLayer.Name & vbCrLf & _
                                          vbTab & "Type" & vbTab & lLayer.LayerType.ToString & vbCrLf & _
                                          vbTab & "File" & vbTab & lLayer.FileName)
            'vbTab & "Tag" & vbTab & lLayer.Tag & vbCrLf & _
            'vbTab & "String" & vbTab & lLayer.ToString & vbcrlf & _

            Dim lFileNameBase As String = IO.Path.GetFileNameWithoutExtension(lLayer.FileName)
            If lFileNameBase.ToLower = "cat" Then
                lString.AppendLine("CatalogingUnitCount" & vbTab & lLayer.Shapes.NumShapes)
                Dim lShapeFile As MapWinGIS.Shapefile = lLayer.GetObject
                Dim lShapeField As MapWinGIS.Field = lShapeFile.FieldByName("NAME")
                Dim lShapeFieldIndex As Integer = 6 'how to get this from field? Needs to be more generic!
                For l As Integer = 0 To lShapeFile.NumShapes - 1
                    lString.AppendLine(vbTab & "Name" & vbTab & lShapeFile.CellValue(lShapeFieldIndex, l) & vbCrLf & _
                                       vbTab & "Number" & vbTab & lShapeFile.CellValue(lShapeFieldIndex + 1, l))
                Next
            End If
        Next
        lString.AppendLine(vbCrLf & lLayerDetailString.ToString)

        SaveFileString("BASINSProjectSummary.txt", lString.ToString)
    End Sub
End Module
