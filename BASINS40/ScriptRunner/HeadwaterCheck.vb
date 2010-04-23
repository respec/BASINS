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
        Dim lNumUpstreamFieldName As String = "HU_12_UPCNT"
        Dim lNewShapefileName As String = ""
        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Original Shape File", lOriginalShapefileName)
            .Add("New Shape File", lNewShapefileName)
            .Add("HUC Field Name", lHucFieldName)
            .Add("Downstream Field Name", lDownstreamFieldName)
            .Add("New Upstream Count Field Name", lNumUpstreamFieldName)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Specify full path of text file to import and WDM file to write into", lUserParms) Then
            With lUserParms
                lOriginalShapefileName = .ItemByKey("Original Shape File")
                lNewShapefileName = .ItemByKey("New Shape File")
                lHucFieldName = .ItemByKey("HUC Field Name")
                lDownstreamFieldName = .ItemByKey("Downstream Field Name")
                lNumUpstreamFieldName = .ItemByKey("New Upstream Count Field Name")
            End With
            Dim lOriginalDBF As New atcTableDBF
            If lOriginalDBF.OpenFile(IO.Path.ChangeExtension(lOriginalShapefileName, ".dbf")) Then
                Dim lLastOriginalField As Integer = lOriginalDBF.NumFields
                Dim lHucField As Integer = lOriginalDBF.FieldNumber(lHucFieldName)
                Dim lDownstreamField As Integer = lOriginalDBF.FieldNumber(lDownstreamFieldName)
                If lHucField < 1 Then
                    lOriginalDBF.Clear()
                    Logger.Msg("Could not find field '" & lHucFieldName & "'")
                ElseIf lDownstreamField < 1 Then
                    lOriginalDBF.Clear()
                    Logger.Msg("Could not find field '" & lDownstreamFieldName & "'")
                Else
                    Dim lNumRecords As Integer = lOriginalDBF.NumRecords
                    Dim lNewDBF As New atcTableDBF
                    lNewDBF.NumFields = lOriginalDBF.NumFields + 1
                    Dim lCurField As Integer
                    For lCurField = 1 To lLastOriginalField
                        lNewDBF.FieldName(lCurField) = lOriginalDBF.FieldName(lCurField)
                        lNewDBF.FieldLength(lCurField) = lOriginalDBF.FieldLength(lCurField)
                        lNewDBF.FieldType(lCurField) = lOriginalDBF.FieldType(lCurField)
                        lNewDBF.FieldLength(lCurField) = lOriginalDBF.FieldLength(lCurField)
                        lNewDBF.FieldDecimalCount(lCurField) = lOriginalDBF.FieldDecimalCount(lCurField)
                    Next
                    Dim lNumUpstreamField As Integer = lLastOriginalField + 1
                    lNewDBF.FieldName(lNumUpstreamField) = lNumUpstreamFieldName
                    lNewDBF.FieldLength(lNumUpstreamField) = 3
                    lNewDBF.FieldType(lNumUpstreamField) = "N"
                    lNewDBF.NumRecords = lNumRecords
                    lNewDBF.InitData()

                    Dim lDownstreamHUCs As New atcCollection
                    Logger.Status("Creating new DBF")
                    Dim lCurrentRecord As Integer
                    For lCurrentRecord = 1 To lNumRecords
                        lOriginalDBF.CurrentRecord = lCurrentRecord
                        lNewDBF.CurrentRecord = lCurrentRecord
                        For lCurField = 0 To lLastOriginalField
                            Dim lCellValue As String = lOriginalDBF.Value(lCurField)
                            lNewDBF.Value(lCurField) = lCellValue
                            If lCurField = lDownstreamField Then
                                lDownstreamHUCs.Increment(lCellValue)
                            End If
                        Next
                        'lNewDBF.Value(lCurField) = "0"
                        'Logger.Progress("DSHucCnt " & lDownstreamHUCs.Count, lCurrentRecord, lNumRecords)
                        Logger.Progress(lCurrentRecord, lNumRecords)
                    Next

                    lOriginalDBF.Clear()
                    Logger.Status("Setting " & lNumUpstreamFieldName)
                    For lCurrentRecord = 1 To lNumRecords
                        lNewDBF.CurrentRecord = lCurrentRecord
                        Dim lUpstreamCountIndex As Integer = lDownstreamHUCs.IndexFromKey(lNewDBF.Value(lHucField))
                        If lUpstreamCountIndex < 0 Then
                            lNewDBF.Value(lNumUpstreamField) = "0"
                        Else
                            lNewDBF.Value(lNumUpstreamField) = lDownstreamHUCs.ItemByIndex(lUpstreamCountIndex)
                        End If
                        Logger.Progress(lCurrentRecord, lNumRecords)
                    Next
                    'TryCopyShapefile(lOriginalShapefileName, lNewShapefileName)
                    lNewDBF.WriteFile(IO.Path.ChangeExtension(lNewShapefileName, ".dbf"))
                End If
                Logger.Status("")
            End If
        End If
    End Sub

End Module
