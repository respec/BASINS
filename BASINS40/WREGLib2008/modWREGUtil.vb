Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.IO
Imports Microsoft.VisualBasic
Public Module modWREGUtil
    Private pDefaultProjection As String = "+proj=latlong +datum=NAD83"

    Public Function MakeStationShapefile(ByVal aAllStations As atcTableDelimited, _
                                     ByVal aSaveAs As String, _
                                     ByVal aFieldNonZero As String, _
                                     ByVal aDesiredProjection As String) As String
        Try
            Dim lLongField As Integer = aAllStations.FieldNumber("Long")
            Dim lLatField As Integer = aAllStations.FieldNumber("Lat")
            If lLongField < 1 Then
                Logger.Msg("Station table missing Long field, cannot create shape file", MsgBoxStyle.Information, "Stop Mapping WREG Stations")
            ElseIf lLatField < 1 Then
                Logger.Msg("Station table missing Lat field, cannot create shape file", MsgBoxStyle.Information, "Stop Mapping WREG Stations")
            Else
                Dim lFieldNonZero As Integer = aAllStations.FieldNumber(aFieldNonZero)
                Dim lLastField As Integer = aAllStations.NumFields - 1
                Dim lCurField As Integer
                Dim lCurFieldValue As String
                Dim lCurShape As MapWinGIS.Shape
                MkDirPath(PathNameOnly(aSaveAs))
                TryDeleteShapefile(aSaveAs)

                Dim lNewShapefile As New MapWinGIS.Shapefile
                lNewShapefile.CreateNew(aSaveAs, MapWinGIS.ShpfileType.SHP_POINT)
                lNewShapefile.Projection = pDefaultProjection
                lNewShapefile.StartEditingTable()
                For lCurField = 0 To lLastField
                    Dim lNewField As New MapWinGIS.Field
                    lNewField.Name = aAllStations.FieldName(lCurField + 1)
                    lNewField.Width = aAllStations.FieldLength(lCurField + 1)
                    Select Case lCurField
                        Case 0
                            lNewField.Type = MapWinGIS.FieldType.STRING_FIELD
                        Case Else
                            lNewField.Type = MapWinGIS.FieldType.DOUBLE_FIELD
                    End Select
                    lNewShapefile.EditInsertField(lNewField, lCurField)
                Next

                lNewShapefile.StartEditingShapes()
                Dim lShapeIndex As Integer = -1
                Dim lLastStation As Integer = aAllStations.NumRecords - 1

                'Make sure there is at least one station with the desired nonzero field
                'If not, skip checking for nonzero field
                If lFieldNonZero > 0 Then
                    Dim lFoundStation As Boolean = False
                    For iStation As Integer = 1 To lLastStation
                        aAllStations.CurrentRecord = iStation
                        lCurFieldValue = aAllStations.Value(lFieldNonZero)
                        If IsNumeric(lCurFieldValue) AndAlso CInt(lCurFieldValue) > 0 Then
                            lFoundStation = True
                            Exit For
                        End If
                    Next
                    If Not lFoundStation Then
                        Logger.Dbg("No stations found with nonzero field " & aFieldNonZero & ", including all stations")
                        lFieldNonZero = 0
                    End If
                End If

                'Include all stations if we don't have a field to check
                Dim lIncludeThisStation As Boolean = (lFieldNonZero < 1)

                Logger.Status("Scanning " & String.Format(lLastStation, "#,###") & " stations for " & Path.GetFileNameWithoutExtension(aSaveAs), True)
                For iStation As Integer = 1 To lLastStation
                    aAllStations.CurrentRecord = iStation
                    If lFieldNonZero > 0 Then
                        lCurFieldValue = aAllStations.Value(lFieldNonZero)
                        lIncludeThisStation = IsNumeric(lCurFieldValue) AndAlso CInt(lCurFieldValue) > 0
                    End If
                    If lIncludeThisStation Then
                        Dim lPoint As New MapWinGIS.Point
                        lPoint.x = aAllStations.Value(lLongField)
                        lPoint.y = aAllStations.Value(lLatField)
                        lCurShape = New MapWinGIS.Shape
                        lCurShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
                        lCurShape.InsertPoint(lPoint, 0)
                        lShapeIndex += 1
                        lNewShapefile.EditInsertShape(lCurShape, lShapeIndex)
                        For lCurField = 0 To lLastField
                            'Debug.Print(lCurField & " " & lNewShapefile.Field(lCurField).Name & " " & lNewShapefile.Field(lCurField).Width & " " & aAllStations.Value(lCurField + 1).Length & " " & aAllStations.Value(lCurField + 1))
                            lNewShapefile.EditCellValue(lCurField, lShapeIndex, aAllStations.Value(lCurField + 1))
                        Next
                    End If
                    Logger.Progress(iStation, lLastStation)
                Next
                lNewShapefile.StopEditingShapes()
                lNewShapefile.StopEditingTable()
                lNewShapefile.Close()
                Logger.Status("")
                Return "<shape_created>" & aSaveAs & "</shape_created>"
                'If FileExists(aSaveAs) Then
                '    SpatialOperations.CopyProcStepsFromCachedFile(aAllStations.FileName, aSaveAs)
                '    SpatialOperations.ProjectAndClipShapeLayer(aSaveAs, SpatialOperations.GeographicProjection, aDesiredProjection)
                '    Return "<add_shape>" & aSaveAs & "</add_shape>" & vbCrLf
                'Else
                '    Logger.Dbg("No shape file created for '" & aSaveAs & "'")
                'End If
            End If
        Catch e As System.Exception
            Logger.Msg("Exception creating NWIS shape file '" & aSaveAs & "' " & e.Message & vbCrLf & e.StackTrace)
        End Try
        Return ""
    End Function
End Module
