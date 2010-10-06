Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports MapWinUtility.Strings
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
                    While lCurrentRecord.Length > 0
                        Dim lDataType As New DataType
                        With lDataType
                            .WdmID = StrSplit(lCurrentRecord, lDelim, lQuote)
                            .Dsn = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                            .Name = StrSplit(lCurrentRecord, lDelim, lQuote)
                            .MFactPI = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                            .MFactR = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        End With
                        lMetSeg.DataTypes.Add(lDataType)
                        lCurrentRecord = lCurrentRecord.Trim
                    End While
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
    Public DataTypes As New DataTypes
    'Public WdmId(7) As String
    'Public Dsn(7) As Integer
    'Public Tstype(7) As String
    'Public MfactPI(7) As Double
    'Public MfactR(7) As Double
End Class

Public Class DataTypes
    Inherits KeyedCollection(Of String, DataType)
    Protected Overrides Function GetKeyForItem(ByVal aDataType As DataType) As String
        Return aDataType.Name
    End Function
End Class

Public Class DataType
    Public WdmID As String
    Public Dsn As Integer
    Public Name As String
    Public MFactPI As Double
    Public MFactR As Double
End Class
