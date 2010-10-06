Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports MapWinUtility.Strings

Public Class Channels
    Inherits KeyedCollection(Of String, Channel)
    Private pWatershed As Watershed
    Protected Overrides Function GetKeyForItem(ByVal aChannel As Channel) As String
        Return aChannel.Reach.Id
    End Function

    Friend Function Open(ByVal aWatershed As Watershed) As Integer
        'read ptf file
        pWatershed = aWatershed
        Dim lReturnCode As Integer = 0
        Dim lName As String = pWatershed.Name & ".ptf"

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
                    Dim lReachId As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lChannel.Reach = aWatershed.Reaches(lReachId)
                    lChannel.Length = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                    lChannel.DepthMean = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mean depth
                    lChannel.WidthMean = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mean width
                    lChannel.ManningN = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'mann n
                    lChannel.SlopeProfile = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'long slope
                    If lChannel.SlopeProfile < 0.0001 Then
                        lChannel.SlopeProfile = 0.0001
                    End If
                    Dim lTempString As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lChannel.SlopeSideUpperFPLeft = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope upper left
                    lChannel.SlopeSideLowerFPLeft = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope lower left
                    lChannel.WidthZeroSlopeLeft = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'zero slope width left
                    lChannel.SlopeSideLeft = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope chan left
                    lChannel.SlopeSideRight = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope chan right
                    lChannel.WidthZeroSlopeRight = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'zero slope width right
                    lChannel.SlopeSideLowerFPRight = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope lower right
                    lChannel.SlopeSideUpperFPRight = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'side slope upper right
                    lChannel.DepthChannel = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'channel depth
                    lChannel.DepthSlopeChange = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'depth at slope change
                    lChannel.DepthMax = CSng(StrSplit(lCurrentRecord, lDelim, lQuote)) 'channel max depth
                    Me.Add(lChannel)
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class Channel
    Public Reach As Reach    'reach id
    Public Length As Single    'length
    Public DepthMean As Single   'mean depth
    Public WidthMean As Single   'mean width
    Public ManningN As Single    'mann n
    Public SlopeProfile As Single    'long slope
    Public SlopeSideLeft As Single  'side slope chan left
    Public SlopeSideRight As Single  'side slope chan right
    Public DepthChannel As Single   'channel depth
    Public SlopeSideLowerFPLeft As Single  'side slope lower left
    Public SlopeSideLowerFPRight As Single  'side slope lower right
    Public DepthSlopeChange As Single  'depth at slope change
    Public DepthMax As Single  'channel max depth
    Public SlopeSideUpperFPLeft As Single  'side slope upper left
    Public SlopeSideUpperFPRight As Single  'side slope upper right
    Public WidthZeroSlopeLeft As Single  'zero slope width left
    Public WidthZeroSlopeRight As Single  'zero slope width right
End Class
