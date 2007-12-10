Option Strict Off
Option Explicit On

Imports System.IO

Module modUCIRecords

    Private pUciRec As ArrayList

    Public Sub ReadUCIRecords(ByRef aFileName As String)
        Dim lCurrentRecord As String
        Dim lStreamReader As New StreamReader(aFileName)

        pUciRec = New ArrayList
        Do
            lCurrentRecord = lStreamReader.ReadLine
            If lCurrentRecord Is Nothing Then
                Exit Do
            End If
            pUciRec.Add(lCurrentRecord.TrimEnd)
        Loop
    End Sub

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
            For Each lUciRec As String In pUciRec
                lRecordIndex += 1
                If lUciRec.StartsWith(aBlockName) Then 'found start
                    aRecordIndex = lRecordIndex
                    Exit For
                End If
            Next lUciRec
            aReturnCode = 10
        End If

        'start at retkey+1
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
            ElseIf lUciRec.IndexOf("***") > 0 Then
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
        If aRecordIndex = pUciRec.Count - 1 Then
            aReturnCode = 0
        End If
    End Sub

    Public Sub StartingRecordofOperationTable(ByRef aOperationName As String, _
                                              ByRef aKeyword As String, _
                                              ByRef srec As Integer, _
                                              ByRef noccur As Integer)
        Dim ostart, i, oend As Integer
        Dim lOperationNameLength As Integer = aOperationName.Length
        srec = 0
        noccur = 0
        ostart = 0
        For i = 0 To pUciRec.Count - 1
            If Len(pUciRec(i)) >= lOperationNameLength Then
                If Left(pUciRec(i), lOperationNameLength) = aOperationName Then
                    'found start of this operation type block
                    ostart = i
                    Exit For
                End If
            End If
        Next i

        oend = 0
        If ostart > 0 Then
            For i = ostart + 1 To pUciRec.Count - 1
                lOperationNameLength = Len("END " & aOperationName)
                If Len(pUciRec(i)) >= lOperationNameLength Then
                    If Left(pUciRec(i), lOperationNameLength) = "END " & aOperationName Then
                        'found end of this operation type block
                        oend = i
                        Exit For
                    End If
                End If
            Next i
        End If

        If ostart > 0 And oend > 0 Then
            For i = ostart + 1 To oend
                lOperationNameLength = Len("  " & aKeyword)
                If Len(pUciRec(i)) >= lOperationNameLength And InStr(1, pUciRec(i), "***") = 0 Then
                    'If Left(uciRec(i), ilen) = "  " & kwd Then
                    'pbd -- distinguish between soil-data and soil-data2 for instance
                    If RTrim(Left(pUciRec(i), lOperationNameLength + 1)) = "  " & aKeyword Then
                        'found start of this table
                        If srec = 0 Then
                            srec = i
                        End If
                        noccur = noccur + 1
                    End If
                End If
            Next i
        End If
    End Sub

    Public Sub GetNextRecordFromTable(ByRef blockname As String, ByRef tablename As String, ByRef srec As Integer, ByRef initfg As Integer, ByRef noccur As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim i, ilen As Integer

        If noccur > 1 And initfg = 1 Then
            'first time in, need to find start of the next one of these tables
            For i = srec + 1 To pUciRec.Count - 1
                ilen = Len("  " & tablename)
                If Len(pUciRec(i)) >= ilen And InStr(1, pUciRec(i), "***") = 0 Then
                    If Left(pUciRec(i), ilen) = "  " & tablename Then
                        'found start of this table
                        'pbd 9/04 always want next occur
                        srec = i
                        Exit For
                    End If
                End If
            Next i
        End If

        For i = srec + 1 To pUciRec.Count - 1
            If RTrim(pUciRec(i)) = "  END " & tablename Then
                retcod = 10
                Exit For
            End If
            If InStr(1, pUciRec(i), "***") = 0 And Len(Trim(pUciRec(i))) > 0 Then
                'found a real line of this block
                srec = i
                cbuff = pUciRec(i)
                rectyp = 0
                retcod = 2
                Exit For
            ElseIf InStr(1, pUciRec(i), "***") > 0 Then
                'found comment
                srec = i
                cbuff = pUciRec(i)
                rectyp = -1
                retcod = 3
                Exit For
            End If
        Next i

        If srec = pUciRec.Count - 1 Then
            retcod = 0
        End If
    End Sub

    Public Function GetCommentBeforeBlock(ByRef aBlockName As String) As String
        Dim lStartRecordIndex As Integer = -1
        For lRecordIndex As Integer = 0 To pUciRec.Count - 1
            If pUciRec(lRecordIndex).StartsWith(aBlockName) Then 'found start of block
                lStartRecordIndex = lRecordIndex
                Exit For
            End If
        Next lRecordIndex

        Dim lComment As String = ""
        If lStartRecordIndex > 1 Then
            For lRecordIndex As Integer = lStartRecordIndex - 1 To 1 Step -1
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

        Dim lComment As String = ""
        Dim lRecordKey As Integer = aStartRecord
        Dim lOccurCount As Integer
        If aThisOccur > 1 Then
            lRecordKey = -1
            lOccurCount = 1
            For i As Integer = aStartRecord + 1 To pUciRec.Count - 1
                If aTableName.Trim = pUciRec(i).Trim Then
                    'found another occurence
                    lOccurCount += 1
                    If lOccurCount = aThisOccur Then
                        lRecordKey = i
                    End If
                    Exit For
                End If
            Next i
        End If

        'start at retkey+1
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

    Public Sub DefaultBlockOrder(ByRef aOrder As ArrayList)
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
        aOrder = lOrder
    End Sub

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
        Dim iprev As Integer = -1
        For Each lBlock As String In aOrder
            Dim lFoundAt As Integer = lOrder.IndexOf(lBlock)
            If lFoundAt >= 0 Then
                iprev = lFoundAt
            Else 'add this one so that myOrder has a complete set
                iprev += 1
                lOrder.Insert(iprev, lBlock)
            End If
        Next
        aOrder.Clear()
        aOrder = lOrder
    End Sub
End Module