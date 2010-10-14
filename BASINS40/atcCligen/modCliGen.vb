Option Strict Off
Option Explicit On
Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Module modCliGen

    Public Function ReadParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As atcTableFixed, ByRef aFooter As String) As Boolean
        Dim lStr As String = WholeFileString(aFileName)
        Dim lpos As Integer = InStr(lStr, " MEAN P ")
        If lpos > 0 Then 'start of monthly data found, save headers
            aHeader = Mid(lStr, 1, lpos - 2)
            lStr = Mid(lStr, lpos)
            lpos = InStr(lStr, "CALM")
            If lpos > 0 Then 'last record of data found
                aFooter = Mid(lStr, lpos + 82)
                lStr = Mid(lStr, 1, lpos + 79)
            End If
            If Len(lStr) > 0 Then 'only editable table parameters left
                aTable = New atcTableFixed
                If aTable.OpenString(lStr) Then 'load table field info
                    Dim lSCol() As Integer = {0, 1, 9, 15, 21, 27, 33, 39, 45, 51, 57, 63, 69, 75, 81}
                    Dim lFLen() As Integer = {0, 8, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6}
                    Dim lFldNames() As String = {"", "Cons", "Jan", "Feb", "Mar", "Apr", "May", _
                                                 "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"}
                    With aTable
                        .NumFields = 13
                        For i As Integer = 1 To .NumFields
                            .FieldName(i) = lFldNames(i)
                            .FieldLength(i) = lFLen(i)
                            .FieldStart(i) = lSCol(i)
                        Next
                    End With
                    Return True
                Else
                    Logger.Dbg("Problem reading CliGen parameters into table in file " & aFileName & "." & vbCrLf & _
                               "Check format of specified CliGen file.")
                    Return False
                End If
            End If
        Else
            Logger.Msg("CliGen parameters not found in file " & aFileName & vbCrLf & _
                       "Expecting to find parameters starting with 'MEAN P'", "CliGen Problem")
            Return False
        End If
    End Function

    Public Sub UpdateParmTable(ByRef aTable As atcTableFixed, ByVal aParm As String, ByVal aIMon As Integer, ByVal aNewVal As Double)
        With aTable
            If .FindFirst(1, aParm) Then
                Select Case aParm
                    Case "SOL.RAD", "SD SOL"
                        .Value(aIMon) = RightJustify(DoubleToString(aNewVal, , "####.0"), .FieldLength(aIMon))
                    Case "TIME PK"
                        .Value(aIMon) = RightJustify(DoubleToString(aNewVal, , "#.000"), .FieldLength(aIMon))
                    Case Else
                        .Value(aIMon) = RightJustify(DoubleToString(aNewVal, , "###.00"), .FieldLength(aIMon))
                End Select
            End If
        End With
    End Sub

    Public Sub WriteParmFile(ByVal aFileName As String, ByRef aHeader As String, ByRef aTable As atcTableFixed, ByRef aFooter As String)
        Dim lStr As String = aHeader & vbCrLf
        With aTable
            .MoveFirst()
            For i As Integer = 1 To .NumRecords
                lStr += .CurrentRecordAsDelimitedString("") & vbCrLf
                .MoveNext()
            Next i
        End With
        lStr += aFooter
        SaveFileString(aFileName, lStr)
    End Sub

End Module
