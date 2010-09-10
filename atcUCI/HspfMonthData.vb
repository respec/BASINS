'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Text
Imports atcUtility

Public Class HspfMonthData
    Private pMonthDataTables As Collection(Of HspfMonthDataTable)
    Private pComment As String

    Public ReadOnly Caption As String = "Month Data"
    Public Comment As String
    Public Uci As HspfUci
    Public ReadOnly EditControlName As String = "ATCoHspf.ctlMonthDataEdit"

    Public ReadOnly Property MonthDataTables() As Collection(Of HspfMonthDataTable)
        Get
            MonthDataTables = pMonthDataTables
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        pMonthDataTables = New Collection(Of HspfMonthDataTable)
    End Sub

    Public Sub ReadUciFile()
        pComment = GetCommentBeforeBlock("MONTH-DATA")

        Dim lOmCode As Integer = HspfOmCode("MONTH-DATA")

        Dim lInit As Integer = 1
        Dim lRetkey As Integer = -1
        Dim lBuff As String = Nothing
        Dim lRectyp As Integer
        Dim lRetcod As Integer

        Dim lDone As Boolean = False
        Do Until lDone
            GetNextRecordFromBlock("MONTH-DATA", lRetkey, lBuff, lRectyp, lRetcod)
           
            lInit = 0
            If lBuff Is Nothing Then
                lDone = True
            ElseIf lBuff.Contains("END") Then
                'skip this
            ElseIf lBuff.Contains("MONTH-DATA") Then  'another one
                Dim lMonthDataTable As New HspfMonthDataTable
                lMonthDataTable.Id = CInt(Right(Trim(lBuff), 3))
                lMonthDataTable.Block = Me
                Dim lComment As String = ""
                Do
                    GetNextRecordFromBlock("MONTH-DATA", lRetkey, lBuff, lRectyp, lRetcod)
                    
                    If lRectyp = -1 Then 'this is a comment
                        If lComment.Length = 0 Then
                            lComment = lBuff
                        Else
                            lComment &= vbCrLf & lBuff
                        End If
                    ElseIf lBuff.Contains("END") Then
                        Exit Do
                    Else 'this is a regular record
                        lMonthDataTable.Comment = lComment
                        Dim lTmpStr As String
                        For lMonthIndex As Integer = 1 To 12
                            lTmpStr = Mid(lBuff, 1 + (lMonthIndex - 1) * 6, 6)
                            If IsNumeric(lTmpStr) Then
                                lMonthDataTable.MonthValue(lMonthIndex) = CSng(lTmpStr)
                            ElseIf lTmpStr.Trim.Length = 0 Then
                                lMonthDataTable.MonthValue(lMonthIndex) = 0.0
                            End If
                        Next lMonthIndex
                        pMonthDataTables.Add(lMonthDataTable)
                    End If
                Loop
            End If
            If lRetcod <> 2 Then
                lDone = True
            End If
        Loop
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        If pMonthDataTables.Count > 0 Then 'something to write
            If pComment.Length > 0 Then
                lSB.AppendLine(pComment)
            End If
            lSB.AppendLine("MONTH-DATA")
            lSB.AppendLine(" ")
            For Each lMonthDataTable As HspfMonthDataTable In pMonthDataTables
                lSB.AppendLine("  MONTH-DATA     " & myFormatI(lMonthDataTable.Id, 3))
                If lMonthDataTable.Comment.Length > 0 Then
                    lSB.AppendLine(lMonthDataTable.Comment)
                End If
                Dim lStr As String = ""
                For lMonthIndex As Integer = 1 To 12
                    lStr &= DoubleToString(lMonthDataTable.MonthValue(lMonthIndex), 6).Replace(",", "").PadLeft(6)
                    'lStr &= CStr(lMonthDataTable.MonthValue(lMonthIndex)).PadLeft(6)
                Next lMonthIndex
                lSB.AppendLine(lStr)
                lSB.AppendLine("  END MONTH-DATA " & myFormatI(lMonthDataTable.Id, 3))
                lSB.AppendLine(" ")
            Next lMonthDataTable
            lSB.AppendLine("END MONTH-DATA")
        End If
        Return lSB.ToString
    End Function
End Class