Option Strict Off
Option Explicit On

Imports System.IO
Imports atcUtility
Imports MapWinUtility

Module modUCIRecords

    Private pUciRec As ArrayList
    Private pBlocks As New atcCollection

    Public Function ReadUCIRecords(ByRef aFileName As String) As Integer
        If Not pUciRec Is Nothing Then
            pUciRec = Nothing
        End If
        pUciRec = New ArrayList

        For lBlockIndex As Integer = 0 To pBlocks.Count - 1
            pBlocks(lBlockIndex) = -1
        Next
        Dim lIndex As Integer = 0
        Logger.Dbg("Reading " & aFileName)
        For Each lCurrentRecord As String In LinesInFile(aFileName)
            pUciRec.Add(lCurrentRecord.TrimEnd)
            If pBlocks.Keys.Contains(lCurrentRecord.Trim) Then
                pBlocks.ItemByKey(lCurrentRecord.Trim) = lIndex
                Logger.Dbg("Reading " & lCurrentRecord.Trim & " at " & lIndex)
            End If
            lIndex += 1
        Next
        Return lIndex
    End Function

    Public Function GetUCIRecord(ByRef aIndex As Integer) As String
        If aIndex >= 0 And aIndex < pUciRec.Count Then
            Return pUciRec(aIndex)
        Else
            Return Nothing
        End If
    End Function

    Public Sub GetNextRecordFromBlock(ByRef aBlockName As String, _
                                      ByRef aRecordIndex As Integer, _
                                      ByRef aRecord As String, _
                                      ByRef aRecordType As Integer, _
                                      ByRef aReturnCode As Integer)
        Dim lRecordIndex As Integer = -1
        If aRecordIndex = -1 Then 'find first record of block
            If pBlocks.Keys.Contains(aBlockName) Then
                aRecordIndex = pBlocks.ItemByKey(aBlockName)
            End If
            If aRecordIndex = -1 Then
                aReturnCode = 10
            End If
        End If

        'start at record after input aRecordIndex
        If aRecordIndex > -1 Then
            aRecordIndex += 1
            Dim lUciRec As String = pUciRec(aRecordIndex)
            If lUciRec = "END " & aBlockName Then
                aRecordIndex = 0
                aReturnCode = 10
            ElseIf lUciRec.Length = 0 And _
                   aBlockName <> "FTABLES" Then
                'found blank line
                aRecord = ""
                aRecordType = -2
                aReturnCode = 2
            ElseIf lUciRec.IndexOf("***") > -1 Then
                'found comment
                aRecord = pUciRec(aRecordIndex)
                aRecordType = -1
                aReturnCode = 2
            Else 'found a real line of this block
                aRecord = lUciRec
                aRecordType = 0
                aReturnCode = 2
            End If
        End If

        If Not aRecord Is Nothing And aRecordType <> -1 Then
            aRecord = aRecord.PadRight(80) 'ensure full record if not a comment 
        End If
        If aRecordIndex = pUciRec.Count - 1 Then
            aReturnCode = 0
        End If
    End Sub

    Public Sub StartingRecordOfOperationTable(ByRef aOperationName As String, _
                                              ByRef aKeyword As String, _
                                              ByRef aRecIndex As Integer, _
                                              ByRef aOccurNumber As Integer)
        Dim lRecIndexStart As Integer = -1
        If pBlocks.Keys.Contains(aOperationName) Then
            lRecIndexStart = pBlocks.ItemByKey(aOperationName)
        End If

        Dim lRecIndexEnd As Integer = -1
        If lRecIndexStart > 0 Then
            For lRecIndex As Integer = lRecIndexStart + 1 To pUciRec.Count - 1
                Dim lRec As String = pUciRec(lRecIndex)
                If lRec.StartsWith("END " & aOperationName) Then
                    'found end of this operation type block
                    lRecIndexEnd = lRecIndex
                    Exit For
                End If
            Next lRecIndex
        End If

        aRecIndex = 0
        aOccurNumber = 0
        Dim lRecSearch As String = "  " & aKeyword
        If lRecIndexStart > -1 And lRecIndexEnd > -1 Then
            For lRecIndex As Integer = lRecIndexStart + 1 To lRecIndexEnd
                Dim lRec As String = pUciRec(lRecIndex)
                If lRec.StartsWith(lRecSearch) Then 'found start of this table 
                    'pbd -- distinguish between soil-data and soil-data2 for instance
                    If lRec.Length = lRecSearch.Length Then
                        If aRecIndex = 0 Then
                            aRecIndex = lRecIndex
                        End If
                        aOccurNumber += 1
                    End If
                End If
            Next lRecIndex
        End If
    End Sub

    Public Sub GetNextRecordFromTable(ByRef aBlockName As String, _
                                      ByRef aTableName As String, _
                                      ByRef aRecIndex As Integer, _
                                      ByRef aInitflag As Integer, _
                                      ByRef aOccurNumber As Integer, _
                                      ByRef aRecord As String, _
                                      ByRef aRecType As Integer, _
                                      ByRef aReturnCode As Integer)
        Dim lRecord As String
        If aOccurNumber > 1 And aInitflag = 1 Then
            'first time in, need to find start of the next one of these tables
            For lRecIndex As Integer = aRecIndex + 1 To pUciRec.Count - 1
                lRecord = pUciRec(lRecIndex)
                If lRecord.StartsWith("  " & aTableName) Then
                    'found start of this table
                    'pbd 9/04 always want next occur
                    aRecIndex = lRecIndex
                    Exit For
                End If
            Next lRecIndex
        End If

        aRecIndex += 1
        lRecord = pUciRec(aRecIndex)
        If lRecord.StartsWith("  END " & aTableName) Then
            aReturnCode = 10
        ElseIf lRecord.IndexOf("***") > -1 Then 'found comment
            aRecord = lRecord
            aRecType = -1
            aReturnCode = 3
        ElseIf lRecord.Trim.Length = 0 And _
               aBlockName <> "FTABLES" Then
            'found blank line
            aRecord = ""
            aRecType = -2
            aReturnCode = 3 'treat like comment
        Else 'found a real line of this block
            aRecord = lRecord
            aRecType = 0
            aReturnCode = 2
        End If
        If aRecIndex = pUciRec.Count - 1 Then
            aReturnCode = 0
        End If
    End Sub

    Public Function GetCommentBeforeBlock(ByRef aBlockName As String) As String
        Dim lStartRecordIndex As Integer = -1
        If pBlocks.Keys.Contains(aBlockName) Then
            lStartRecordIndex = pBlocks.ItemByKey(aBlockName)
        End If

        Dim lComment As String = ""
        If lStartRecordIndex > 0 Then
            For lRecordIndex As Integer = lStartRecordIndex - 1 To 0 Step -1
                If pUciRec(lRecordIndex).Trim.Length = 0 Then 'found blank line
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment = vbCrLf & lComment
                    End If
                ElseIf pUciRec(lRecordIndex).IndexOf("***") > -1 Then 'found comment
                    If lComment.Length = 0 Then
                        lComment = pUciRec(lRecordIndex)
                    Else
                        lComment = pUciRec(lRecordIndex) & vbCrLf & lComment
                    End If
                Else 'something on this line and its not a comment
                    Exit For
                End If
            Next lRecordIndex
        End If
        Return lComment.TrimEnd
    End Function

    Public Function GetTableComment(ByRef aStartRecord As Integer, _
                                    ByRef aTableName As String, _
                                    ByRef aThisOccur As Integer) As String

        Dim lRecordKey As Integer = aStartRecord
        Dim lOccurCount As Integer
        If aThisOccur > 1 Then
            lRecordKey = -1
            lOccurCount = 1
            For lRecordIndex As Integer = aStartRecord + 1 To pUciRec.Count - 1
                If aTableName.Trim = pUciRec(lRecordIndex).Trim Then
                    'found another occurence
                    lOccurCount += 1
                    If lOccurCount = aThisOccur Then
                        lRecordKey = lRecordIndex
                    End If
                    Exit For
                End If
            Next lRecordIndex
        End If

        'start at retkey+1
        Dim lComment As String = ""
        If lRecordKey > 0 Then
            For lRecordIndex As Integer = lRecordKey - 1 To 0 Step -1
                If pUciRec(lRecordIndex).Trim.Length = 0 Then 'found blank line
                    'dont tack it on if the preceeding line is real
                    If lComment.Length = 0 Then
                        lComment = " "
                    Else
                        lComment = vbCrLf & lComment
                    End If
                ElseIf pUciRec(lRecordIndex).IndexOf("***") = -1 Then
                    'something on this line and its not a comment
                    Exit For
                Else 'found comment
                    If lComment.Length = 0 Then
                        lComment = pUciRec(lRecordIndex)
                    Else
                        lComment = pUciRec(lRecordIndex) & vbCrLf & lComment
                    End If
                End If
            Next lRecordIndex
        End If
        Return lComment.TrimEnd
    End Function

    Public Function DefaultBlockOrder() As ArrayList
        Dim lOrder As New ArrayList(20)
        With lOrder
            .Add("GLOBAL")
            .Add("FILES")
            .Add("OPN SEQUENCE")
            .Add("MONTH DATA")
            .Add("CATEGORY")
            .Add("PERLND")
            .Add("IMPLND")
            .Add("RCHRES")
            .Add("FTABLES")
            .Add("COPY")
            .Add("PLTGEN")
            .Add("DISPLY")
            .Add("DURANL")
            .Add("GENER")
            .Add("MUTSIN")
            .Add("BMPRAC")
            .Add("REPORT")
            .Add("CONNECTIONS")
            .Add("MASSLINKS")
            .Add("SPECIAL ACTIONS")
        End With

        pBlocks.Clear()
        pBlocks.Add("RUN", -1)
        For Each lBlockName As String In lOrder
            pBlocks.Add(lBlockName, -1)
        Next
        pBlocks.Add("CATEGORY", -1)
        pBlocks.Add("MONTH-DATA", -1)
        pBlocks.Add("MASS-LINK", -1)
        pBlocks.Add("SPEC-ACTIONS", -1)
        pBlocks.Add("EXT SOURCES", -1)
        pBlocks.Add("EXT TARGETS", -1)
        pBlocks.Add("SCHEMATIC", -1)
        pBlocks.Add("NETWORK", -1)

        Return lOrder
    End Function

    Public Sub SaveBlockOrder(ByRef aOrder As ArrayList)
        Dim lOrder As New ArrayList(20)
        Dim lConnectionFound As Boolean
        For lRecordIndex As Integer = 0 To pUciRec.Count - 1
            Dim lBlock As String = pUciRec(lRecordIndex).Trim
            Select Case lBlock
                Case "GLOBAL", "FILES", "OPN SEQUENCE", "PERLND", "IMPLND", _
                     "RCHRES", "FTABLES", "COPY", "PLTGEN", "DISPLY", _
                     "DURANL", "GENER", "MUTSIN", "BMPRAC", "REPORT", _
                     "CATEGORY"
                    lOrder.Add(lBlock)
                Case "MONTH-DATA"
                    lOrder.Add("MONTH DATA")
                Case "MASS-LINK"
                    lOrder.Add("MASSLINKS")
                Case "SPEC-ACTIONS"
                    lOrder.Add("SPECIAL ACTIONS")
                Case "EXT SOURCES", "EXT TARGETS", "SCHEMATIC", "NETWORK"
                    If Not lConnectionFound Then
                        lOrder.Add("CONNECTIONS")
                        lConnectionFound = True
                    End If
            End Select
        Next

        'add any blocks in pOrder that aren't in myOrder
        Dim lPrev As Integer = -1
        For Each lBlock As String In aOrder
            Dim lFoundAt As Integer = lOrder.IndexOf(lBlock)
            If lFoundAt >= 0 Then
                lPrev = lFoundAt
            Else 'add this one so that myOrder has a complete set
                lPrev += 1
                lOrder.Insert(lPrev, lBlock)
            End If
        Next
        aOrder.Clear()
        aOrder = lOrder
    End Sub
End Module