Imports atcData
Imports atcData.atcTimeseriesGroup
Imports atcUtility
Imports atcUtility.modFile
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGIS

Imports Microsoft.VisualBasic
Imports System

'WBD HUC 12 field names: 
'HUC_8	HUC_10	HUC_12	ACRES	NCONTRB_A	
'HU_10_GNIS	HU_12_GNIS	
'HU_10_DS	HU_10_NAME	HU_10_MOD	HU_10_TYPE	
'HU_12_DS	HU_12_NAME	HU_12_MOD	HU_12_TYPE	META_ID	STATES

Public Module HeadwaterCheck
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalShapefileName As String = ""
        Dim lHucFieldName As String = "HUC_12"
        Dim lDownstreamFieldName As String = "HU_12_DS"
        Dim lNewShapefileName As String = ""
        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Original Shape File", lOriginalShapefileName)
            .Add("New Shape File", lNewShapefileName)
            .Add("HUC Field Name", lHucFieldName)
            .Add("Downstream Field Name", lDownstreamFieldName)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Specify full path of text file to import and WDM file to write into", lUserParms) Then
            With lUserParms
                lOriginalShapefileName = .ItemByKey("Original Shape File")
                lNewShapefileName = .ItemByKey("New Shape File")
                lHucFieldName = .ItemByKey("HUC Field Name")
                lDownstreamFieldName = .ItemByKey("Downstream Field Name")
            End With
            Dim lOriginalShapefile As New MapWinGIS.Shapefile
            If lOriginalShapefile.Open(lOriginalShapefileName) Then
                Dim lLastField As Integer = lOriginalShapefile.NumFields - 1
                Dim lHucField As Integer = -1
                Dim lDownstreamField As Integer = -1
                Dim lNewShapefile As New MapWinGIS.Shapefile
                lNewShapefile.CreateNewWithShapeID(lNewShapefileName, lOriginalShapefile.ShapefileType)
                lNewShapefile.StartEditingTable()
                Dim lCurField As Integer
                For lCurField = 0 To lLastField
                    lNewShapefile.EditInsertField(lOriginalShapefile.Field(lCurField), lCurField)
                    Select Case lOriginalShapefile.Field(lCurField).Name
                        Case lHucFieldName : lHucField = lCurField
                        Case lDownstreamFieldName : lDownstreamField = lCurField

                    End Select
                Next
                If lHucField < 0 Then
                    lOriginalShapefile.Close()
                    Logger.Msg("Could not find field '" & lHucFieldName & "'")
                ElseIf lDownstreamField < 0 Then
                    lOriginalShapefile.Close()
                    Logger.Msg("Could not find field '" & lDownstreamFieldName & "'")
                Else
                    Dim lNewField As New MapWinGIS.Field
                    lNewField.Name = "headwater"
                    lNewField.Width = 1
                    lNewField.Type = MapWinGIS.FieldType.STRING_FIELD
                    lNewShapefile.EditInsertField(lNewField, lCurField)
                    Dim lDownstreamHUCs As New SortedList(Of String, String)
                    Dim lLastShape As Integer = lOriginalShapefile.NumShapes - 1
                    Logger.Status("Creating new shape file")
                    lNewShapefile.StartEditingShapes()
                    For lShapeIndex As Integer = 0 To lLastShape
                        lNewShapefile.EditInsertShape(lOriginalShapefile.Shape(lShapeIndex), lShapeIndex)
                        For lCurField = 0 To lLastField
                            Dim lCellValue As Object = lOriginalShapefile.CellValue(lCurField, lShapeIndex)
                            lNewShapefile.EditCellValue(lCurField, lShapeIndex, lCellValue)
                            If lCurField = lDownstreamField AndAlso Not lDownstreamHUCs.ContainsKey(lCellValue) Then
                                lDownstreamHUCs.Add(lOriginalShapefile.CellValue(lDownstreamField, lShapeIndex), Nothing)
                            End If
                        Next
                        lNewShapefile.EditCellValue(lCurField, lShapeIndex, "Y")
                        Logger.Progress("DSHucCnt " & lDownstreamHUCs.Count, lShapeIndex, lLastShape)
                    Next
                    lNewShapefile.StopEditingShapes()

                    lOriginalShapefile.Close()
                    Logger.Status("Setting Headwater Field")
                    For lShapeIndex As Integer = 0 To lLastShape
                        If lDownstreamHUCs.ContainsKey(lNewShapefile.CellValue(lHucField, lShapeIndex)) Then
                            lNewShapefile.EditCellValue(lLastField, lShapeIndex, "N")
                        End If
                        If CInt(lShapeIndex / 10) * 10 = lShapeIndex Then Logger.Progress(lShapeIndex, lLastShape)
                    Next
                End If
                lNewShapefile.StopEditingTable()
                lNewShapefile.Save()
                lNewShapefile.Close()
                Logger.Status("")
            End If
        End If
    End Sub

End Module
