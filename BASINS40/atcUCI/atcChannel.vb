Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class Channels
    Inherits KeyedCollection(Of String, Channel)
    Protected Overrides Function GetKeyForItem(ByVal aChannel As Channel) As String
        Dim lKey As String
        lKey = aChannel.ChanId
        Return lKey
    End Function

    Public Function Open(ByVal aFileName As String) As Integer
        'read ptf file
        Dim lReturnCode As Integer = 0
        Dim lName As String = FilenameOnly(aFileName) & ".ptf"

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
                    Dim lChannel As New Channel
                    lChannel.ChanId = StrSplit(lCurrentRecord, lDelim, lQuote) 'reach id
                    lChannel.ChanL = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lChannel.ChanYm = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mean depth
                    lChannel.ChanWm = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mean width
                    lChannel.ChanN = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mann n
                    lChannel.ChanS = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'long slope
                    If lChannel.ChanS < 0.0001 Then
                        lChannel.ChanS = 0.0001
                    End If
                    Dim lTempString As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lChannel.ChanM31 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope upper left
                    lChannel.ChanM21 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope lower left
                    lChannel.ChanW11 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'zero slope width left
                    lChannel.ChanM11 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope chan left
                    lChannel.ChanM12 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope chan right
                    lChannel.ChanW12 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'zero slope width right
                    lChannel.ChanM22 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope lower right
                    lChannel.ChanM32 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope upper right
                    lChannel.ChanYc = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'channel depth
                    lChannel.ChanYt1 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'depth at slope change
                    lChannel.ChanYt2 = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'channel max depth
                    Me.Add(lChannel)
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

Public Class Channel
    Public ChanId As String   'reach id
    Public ChanL As Single    'length
    Public ChanYm As Single   'mean depth
    Public ChanWm As Single   'mean width
    Public ChanN As Single    'mann n
    Public ChanS As Single    'long slope
    Public ChanM11 As Single  'side slope chan left
    Public ChanM12 As Single  'side slope chan right
    Public ChanYc As Single   'channel depth
    Public ChanM21 As Single  'side slope lower left
    Public ChanM22 As Single  'side slope lower right
    Public ChanYt1 As Single  'depth at slope change
    Public ChanYt2 As Single  'channel max depth
    Public ChanM31 As Single  'side slope upper left
    Public ChanM32 As Single  'side slope upper right
    Public ChanW11 As Single  'zero slope width left
    Public ChanW12 As Single  'zero slope width right
End Class
