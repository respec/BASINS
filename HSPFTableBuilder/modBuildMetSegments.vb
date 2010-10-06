Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Module modBuildMetSegments
    Sub FixMetSegments(ByVal aUci As HspfUci)
        Logger.Dbg("FixMetSegments")
        Dim lMetSegmentPath As String = g_BaseFolder & "MetSegments\"
        If g_SubProject.Length > 0 Then
            lMetSegmentPath &= g_SubProject & "\"
        End If
        Dim lMetDataTypes() As String = {"PREC", "ATEM", "PEVT"}

        'be sure correct number of met inputs available in each segment
        Dim lMetDataTypesCount As New atcCollection
        For Each lMetDataType As String In lMetDataTypes
            lMetDataTypesCount.Add(lMetDataType, 0)
        Next
        For lMetSegIndex As Integer = 0 To aUci.MetSegs.Count - 1
            Dim lMetSeg As HspfMetSeg = aUci.MetSegs(lMetSegIndex)
            For Each lMetDataTypesKey As String In lMetDataTypesCount.Keys
                lMetDataTypesCount.ItemByKey(lMetDataTypesKey) = 0
            Next

            For lMetSegRecIndex As Integer = lMetSeg.MetSegRecs.Count - 1 To 0 Step -1
                Dim lMetSegRecord As HspfMetSegRecord = lMetSeg.MetSegRecs(lMetSegRecIndex)
                Dim lMetType As String = lMetSegRecord.Source.Member
                If lMetDataTypes.Contains(lMetType) Then
                    lMetDataTypesCount.Increment(lMetType, 1)
                    If lMetDataTypesCount.ItemByKey(lMetType) > 1 Then
                        lMetSeg.MetSegRecs.RemoveAt(lMetSegRecIndex)
                    End If
                Else
                    lMetSeg.MetSegRecs.RemoveAt(lMetSegRecIndex)
                End If
            Next
            For Each lMetDataTypesKey As String In lMetDataTypesCount.Keys
                If lMetDataTypesCount.ItemByKey(lMetDataTypesKey) = 0 Then
                    Dim lMetSegRecord As HspfMetSegRecord = lMetSeg.MetSegRecs(0).Clone
                    lMetSegRecord.Name = lMetDataTypesKey
                    lMetSegRecord.Source.Member = lMetDataTypesKey
                    lMetSegRecord.Sgapstrg = ""
                    lMetSeg.MetSegRecs.Add(lMetSegRecord)
                End If
            Next
            If lMetSeg.MetSegRecs.Count <> lMetDataTypesCount.Count Then
                Logger.Dbg("MisssingRecord!")
            End If
        Next

        'update dsns on info in files from TT
        Dim lStationDsnTable As New atcTableDelimited
        With lStationDsnTable
            .Delimiter = vbTab
            .OpenFile(lMetSegmentPath & "station-dsn.txt")
        End With
        For lMetSegIndex As Integer = 0 To aUci.MetSegs.Count - 1
            Dim lMetSeg As HspfMetSeg = aUci.MetSegs(lMetSegIndex)
            Dim lMetSegComment As String = lMetSeg.Comment
            Dim lMetSetLocation As String = lMetSegComment.Substring(lMetSegComment.Length - 8)
            If lStationDsnTable.FindFirst(1, lMetSetLocation) Then
                For Each lMetSegRec As HspfMetSegRecord In lMetSeg.MetSegRecs
                    lMetSegRec.Source.VolId = lStationDsnTable.Value(Array.IndexOf(lMetDataTypes, lMetSegRec.Source.Member) + 2)
                    'TODO: if VolId is for a dataset not at same location as PREC, make note in comment
                Next
            End If
            For lMetSegRecIndex As Integer = 0 To lMetSeg.MetSegRecs.Count - 1
                Dim lMetSegRecord As HspfMetSegRecord = lMetSeg.MetSegRecs(lMetSegRecIndex)
                Dim lMetType As String = lMetSegRecord.Source.Member
                If lMetType = "PEVT" Then
                    lMetSegRecord.Source.Member = "PMET"
                    If g_PmetMFact <> 1.0 Then
                        lMetSegRecord.MFactR = g_PmetMFact
                        lMetSegRecord.MFactP = g_PmetMFact
                    End If
                End If
            Next
        Next

        'check reach met segments
        Dim lReachStationCollection As New atcCollection
        For Each lRecord As String In LinesInFile(lMetSegmentPath & "reach-station.txt")
            Dim lFields() As String = lRecord.Split(vbTab)
            lReachStationCollection.Add(lFields(0), lFields(1))
        Next
        For Each lOperation As HspfOperation In aUci.OpnBlks("RCHRES").Ids
            Dim lReachMetSegmentName As String = lReachStationCollection.ItemByKey(lOperation.Id.ToString)
            If lReachMetSegmentName.Length = 0 Then
                Logger.Dbg("RCHRES " & lOperation.Id & " MissingMetSegment")
            ElseIf Not lOperation.MetSeg.Comment.Contains(lReachMetSegmentName) Then
                Logger.Dbg("RCHRES " & lOperation.Id & " WrongMetSegment")
            End If
        Next

        'TODO: need any checks here?
        Dim lStationOrderTable As New atcTableDelimited
        lStationOrderTable.OpenFile(lMetSegmentPath & "station-order.txt")
    End Sub
End Module
