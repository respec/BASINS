Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class Reaches
    Inherits KeyedCollection(Of String, Reach)
    Protected Overrides Function GetKeyForItem(ByVal aReach As Reach) As String
        Return aReach.Id
    End Function

    Public Function Open(ByVal aFileName As String) As Integer
        'read rch file
        Dim lReturnCode As Integer = 0
        Dim lName As String = FilenameOnly(aFileName) & ".rch"

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
                    Dim lReach As New Reach
                    lReach.Id = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.Name = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.WsId = StrSplit(lCurrentRecord, lDelim, lQuote)
                    Dim lTempString As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.NExits = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.Type = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.Length = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lReach.DeltH = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lReach.Elev = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.DownID = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.Depth = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lReach.Width = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lTempString = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lReach.Manning = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lReach.Order = Me.Count
                    Me.Add(lReach)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class Reach
    Public Id As String        'Reach id
    Public Name As String      'Reach name (ie Peachtree Creek)
    Public WsId As String      'Watershed id (same as reach id)
    Public NExits As Integer   'Number of exits
    Public Type As String      'Stream(S) or Reservior(R)
    Public Length As Double    'Stream length
    Public DeltH As Double     'Stream delta h
    Public Elev As Double      'Stream elevation
    Public DownID As String    'Downstream reach id
    Public Depth As Double     'Mean depth
    Public Width As Double     'Mean width
    Public Manning As Double   'Mannings n
    Public Order As Integer    'Order of reaches upstream to downstream
End Class
