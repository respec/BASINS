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
        Dim lNewShapefileFolder As String = ""
        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Original Shape File", lOriginalShapefileName)
            .Add("New Shape File Folder", lNewShapefileFolder)
            .Add("HUC Field Name", lHucFieldName)
            .Add("Downstream Field Name", lDownstreamFieldName)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("HUC Shapefile Specifications", lUserParms) Then
            With lUserParms
                lOriginalShapefileName = .ItemByKey("Original Shape File")
                lNewShapefileFolder = .ItemByKey("New Shape File Folder")
                lHucFieldName = .ItemByKey("HUC Field Name")
                lDownstreamFieldName = .ItemByKey("Downstream Field Name")
            End With
            Dim lNewShapeFileName As String = IO.Path.Combine(lNewShapefileFolder, IO.Path.GetFileNameWithoutExtension(lOriginalShapefileName)) & ".shp"
            Dim lCopyStatus As Boolean
            If IO.File.Exists(lNewShapeFileName) Then
                lCopyStatus = TryCopy(IO.Path.ChangeExtension(lOriginalShapefileName, "dbf"), _
                                      IO.Path.ChangeExtension(lNewShapeFileName, "dbf"))
            Else
                lCopyStatus = TryCopyShapefile(lOriginalShapefileName, lNewShapefileFolder)
            End If
            If lCopyStatus Then
                Dim lHucField As Integer = -1
                Dim lDownstreamField As Integer = -1
                Dim lNewShapefile As New MapWinGIS.Shapefile
                Dim lCircularExit As New SortedList(Of String, String)
                If lNewShapefile.Open(lNewShapeFileName) Then
                    lNewShapefile.StartEditingTable()
                    Dim lCurField As Integer
                    For lCurField = 0 To lNewShapefile.NumFields - 1
                        Select Case lNewShapefile.Field(lCurField).Name
                            Case lHucFieldName : lHucField = lCurField
                            Case lDownstreamFieldName : lDownstreamField = lCurField

                        End Select
                    Next
                    If lHucField < 0 Then
                        Logger.Msg("Could not find field '" & lHucFieldName & "'")
                    ElseIf lDownstreamField < 0 Then
                        Logger.Msg("Could not find field '" & lDownstreamFieldName & "'")
                    Else
                        Dim lHeadwaterField As New MapWinGIS.Field
                        lHeadwaterField.Name = "headwater"
                        lHeadwaterField.Width = 1
                        lHeadwaterField.Type = MapWinGIS.FieldType.STRING_FIELD
                        lNewShapefile.EditInsertField(lHeadwaterField, lNewShapefile.NumFields)
                        Dim lHeadwaterFieldIndex As Integer = lNewShapefile.NumFields - 1

                        Dim lUpstreamCountField As New MapWinGIS.Field
                        lUpstreamCountField.Name = "upstreamCnt"
                        lUpstreamCountField.Width = 1
                        lUpstreamCountField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        lNewShapefile.EditInsertField(lUpstreamCountField, lNewShapefile.NumFields)
                        Dim lUpstreamCountFieldIndex As Integer = lNewShapefile.NumFields - 1

                        Dim lDownstreamHUCs As New SortedList(Of String, String)
                        Dim lLastShape As Integer = lNewShapefile.NumShapes - 1
                        Logger.Status("Editing fields in shapefile " & lNewShapeFileName)
                        lNewShapefile.StartEditingTable()
                        For lShapeIndex As Integer = 0 To lLastShape
                            Dim lCellValue As Object = lNewShapefile.CellValue(lDownstreamField, lShapeIndex)
                            If Not lDownstreamHUCs.ContainsKey(lCellValue) Then
                                lDownstreamHUCs.Add(lCellValue, -1)
                            End If
                            lNewShapefile.EditCellValue(lHeadwaterFieldIndex, lShapeIndex, "Y")
                            lNewShapefile.EditCellValue(lUpstreamCountFieldIndex, lShapeIndex, 0)
                            If lDownstreamHUCs.Count Mod 100 = 0 Then
                                Logger.Progress("DSHucCnt " & lDownstreamHUCs.Count, lShapeIndex, lLastShape)
                            End If
                        Next
                        Logger.Status("Setting Indexes of DSHucs")
                        Dim lHuc12 As Object
                        Dim lHuc12DS As Object
                        Dim lDownstreamIndex As Integer = 0
                        For lShapeIndex As Integer = 0 To lLastShape
                            lHuc12 = lNewShapefile.CellValue(lHucField, lShapeIndex)
                            lDownstreamIndex = lDownstreamHUCs.IndexOfKey(lHuc12)
                            If lDownstreamIndex > -1 Then
                                lDownstreamHUCs.Item(lHuc12) = lShapeIndex
                            End If
                        Next

                        Logger.Status("Setting Headwater Field and Counting Downstream")
                        Dim lHuc12CountUpstream As Integer = 0
                        For lShapeIndex As Integer = 0 To lLastShape
                            lHuc12 = lNewShapefile.CellValue(lHucField, lShapeIndex)
                            If lDownstreamHUCs.ContainsKey(lHuc12) Then
                                lNewShapefile.EditCellValue(lHeadwaterFieldIndex, lShapeIndex, "N")
                            End If
                            lHuc12DS = lNewShapefile.CellValue(lDownstreamField, lShapeIndex)
                            lDownstreamIndex = lShapeIndex

                            Dim lHuc12Chain As New SortedList(Of String, Integer)
                            While lHuc12DS IsNot Nothing AndAlso lHuc12DS.ToString.Length > 0
                                If lHuc12Chain.ContainsKey(lHuc12DS) Then
                                    lNewShapefile.EditCellValue(lHeadwaterFieldIndex, lDownstreamIndex, "?")
                                    lNewShapefile.EditCellValue(lHeadwaterFieldIndex, lShapeIndex, "X")
                                    'Logger.Dbg("CircularExitFor " & lHuc12 & " at " & lHuc12DS & " count " & lHuc12Chain.Count)
                                    If Not lCircularExit.ContainsKey(lHuc12DS) Then
                                        lCircularExit.Add(lHuc12DS, lDownstreamIndex)
                                    End If
                                    Exit While
                                Else
                                    lHuc12Chain.Add(lHuc12DS, Nothing)
                                End If
                                lDownstreamIndex = lDownstreamHUCs.Item(lHuc12DS)
                                lHuc12CountUpstream = lNewShapefile.CellValue(lUpstreamCountFieldIndex, lDownstreamIndex) + 1
                                lNewShapefile.EditCellValue(lUpstreamCountFieldIndex, lDownstreamIndex, lHuc12CountUpstream)
                                lHuc12DS = lNewShapefile.CellValue(lDownstreamField, lDownstreamIndex)
                            End While
                            If lShapeIndex Mod 100 = 0 Then Logger.Progress(lShapeIndex, lLastShape)
                        Next
                        lNewShapefile.StopEditingTable()
                        lNewShapefile.Save()
                        If lCircularExit.Count > 0 Then
                            Logger.Dbg("CircularCount " & lCircularExit.Count)
                            Dim lCountTotal As Integer = 0
                            For Each lCircularItem As String In lCircularExit.Keys
                                Dim lCount As Integer = lNewShapefile.CellValue(lUpstreamCountFieldIndex, lCircularExit.Item(lCircularItem))
                                lCountTotal += lCount
                                Logger.Dbg(lCircularItem & " " & lCount)
                            Next
                            Logger.Dbg("TotalImpacted " & lCountTotal)
                        End If
                    End If
                    lNewShapefile.Close()
                End If
                Logger.Status("")
            End If
        End If
    End Sub
End Module
