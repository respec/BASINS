Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class MetSegments
    Inherits KeyedCollection(Of String, MetSegment)
    Private pWatershed As Watershed
    Protected Overrides Function GetKeyForItem(ByVal aMetSegment As MetSegment) As String
        Return aMetSegment.Id
    End Function

    Friend Function Open(ByVal aWatershed As Watershed) As Integer
        'read seg file
        pWatershed = aWatershed
        Dim lReturnCode As Integer = 0
        Dim lName As String = pWatershed.Name & ".seg"

        Try
            Dim lDelim As String = " "
            Dim lQuote As String = """"
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lName)
            lCurrentRecord = lStreamReader.ReadLine 'first line is header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    Dim lMetSeg As New MetSegment
                    lMetSeg.Id = StrSplit(lCurrentRecord, lDelim, lQuote)
                    For lIndex As Integer = 1 To 7
                        lMetSeg.WdmId(lIndex) = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lMetSeg.Dsn(lIndex) = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lMetSeg.Tstype(lIndex) = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lMetSeg.MfactPI(lIndex) = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lMetSeg.MfactR(lIndex) = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    Next
                    Me.Add(lMetSeg)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class MetSegment
    Public Id As Integer 'met segment id
    '1-prec, 2-atem, 3-dewp, 4-wind, 5-solr, 6-clou, 7-pevt
    Public WdmId(7) As String
    Public Dsn(7) As Integer
    Public Tstype(7) As String
    Public MfactPI(7) As Double
    Public MfactR(7) As Double
End Class
