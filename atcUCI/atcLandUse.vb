Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class LandUses
    Inherits KeyedCollection(Of String, LandUse)
    Protected Overrides Function GetKeyForItem(ByVal aLandUse As LandUse) As String
        Dim lKey As String
        lKey = aLandUse.Description & ":" & aLandUse.Type & ":" & aLandUse.Reach
        Return lKey
    End Function

    Public Function Open(ByVal aFileName As String) As Integer
        'read wsd file
        Dim lReturnCode As Integer = 0
        Dim lName As String = FilenameOnly(aFileName) & ".wsd"
        Try
            Dim lDelim As String = " "
            Dim lQuote As String = """"
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(aFileName)
            lCurrentRecord = lStreamReader.ReadLine 'first line is header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    Dim lLandUse As New LandUse
                    'count the number of fields in this string
                    Dim lStrTemp As String = lCurrentRecord
                    Dim lFieldCount As Integer = 0
                    Do While StrSplit(lStrTemp, lDelim, lQuote).Length > 0
                        lFieldCount += 1
                    Loop
                    If lFieldCount = 6 Then
                        'this is the normal way
                        lLandUse.Description = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lLandUse.Type = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Reach = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lLandUse.Area = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Slope = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Distance = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    Else
                        'if coming from old delineator might not be space delimited
                        lLandUse.Description = StrSplit(lCurrentRecord, lDelim, lQuote)
                        If lCurrentRecord.Length > 23 Then
                            lLandUse.Distance = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 7, 8))
                            lLandUse.Slope = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 15, 8))
                            lLandUse.Area = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 23, 8))
                        End If
                        lLandUse.Type = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Reach = StrSplit(lCurrentRecord, lDelim, lQuote)
                    End If
                    Select Case lLandUse.Type
                        Case 1 : lLandUse.Type = "IMPLND"
                        Case 2 : lLandUse.Type = "PERLND"
                        Case Else : lLandUse.Type = "Unknown"
                    End Select
                    Me.Add(lLandUse)
                End If
            Loop
            Return lReturnCode
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class LandUse
    Public Description As String
    Public Type As String  'PERLND(2) or IMPLND(1)
    Public Reach As String
    Public Area As Double
    Public Slope As Double
    Public Distance As Double
    Public Id As Integer
    Public Oper As String
End Class
