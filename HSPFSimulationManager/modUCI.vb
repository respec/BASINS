Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Module modUCI

    Public MessageFile As atcUCI.HspfMsg

    Function OpenUCI(aUciFileName As String) As atcUCI.HspfUci
        If Not FileExists(aUciFileName) Then
            Return Nothing
        End If

        If MessageFile Is Nothing Then
            MessageFile = New atcUCI.HspfMsg()
            Dim lMsgFile As String = FindFile("Locate Message WDM", "hspfmsg.wdm")
            If Not IO.File.Exists(lMsgFile) Then
FindMsg:        lMsgFile = FindFile("Locate Message WDM", lMsgFile, "wdm", aUserVerifyFileName:=True)
            End If
            If Not IO.File.Exists(lMsgFile) OrElse Not lMsgFile.ToLower().EndsWith("hspfmsg.wdm") Then
                Select Case Logger.Msg("HSPF message file (hspfmsg.wdm) not found.", MsgBoxStyle.AbortRetryIgnore)
                    Case MsgBoxResult.Abort
                        Throw New ApplicationException("HSPF message file (hspfmsg.wdm) not found.")
                    Case MsgBoxResult.Retry
                        GoTo FindMsg
                    Case MsgBoxResult.Ignore
                        Return Nothing
                End Select
            End If
            MessageFile.Open(lMsgFile)
        End If

        Dim lUCI As New atcUCI.HspfUci
        lUCI.FastReadUciForStarter(MessageFile, aUciFileName)
        Return lUCI
    End Function

    Function GetEchoFileName(aUCI As atcUCI.HspfUci) As String
        Dim lFilesBlock As HspfFilesBlk = aUCI.FilesBlock
        For lIndex As Integer = 0 To lFilesBlock.Count - 1
            If lFilesBlock.Value(lIndex).Typ = "MESSU" Then
                Dim lUCIFileDirectory As String = IO.Path.GetDirectoryName(aUCI.Name)
                Return AbsolutePath(lFilesBlock.Value(lIndex).Name.Trim(), lUCIFileDirectory)
                Exit For
            End If
        Next
        Return String.Empty
    End Function

    ''' <summary>
    ''' True if echo file contains "End of Job"
    ''' </summary>
    Function RunComplete(aUCI As atcUCI.HspfUci) As Boolean
        Dim lEchName As String = GetEchoFileName(aUCI)

        If Not IO.File.Exists(lEchName) Then
            Return False
        End If
        Dim lEchoFile As IO.FileStream = Nothing
        Try
            'Open up the ech file
            lEchoFile = New IO.FileStream(lEchName, IO.FileMode.Open, IO.FileAccess.Read)
            Dim lFileLength As Long = lEchoFile.Length
            Dim lStartReading As Long = Math.Max(0, lFileLength - 1000)
            Dim lReadLength As Long = lFileLength - lStartReading
            lEchoFile.Position = lStartReading
            Dim lStreamReader As New IO.StreamReader(lEchoFile, System.Text.Encoding.ASCII)
            Dim lLastPartOfEchoFile As String = lStreamReader.ReadToEnd()
            Return lLastPartOfEchoFile.Contains("End of Job")
        Catch
            Return False
        Finally
            If lEchoFile IsNot Nothing Then
                Try
                    lEchoFile.Close()
                Catch
                End Try
            End If
        End Try
    End Function

    Public Function ConnectionSummary(ByVal aSourceUCI As HspfUci, ByVal aTargetUCI As HspfUci) As List(Of String)
        Dim lConnections As New List(Of String)
        Dim lFileName As String = ""

        'build collection of wdms used by source uci 
        Dim lSourceWDMs As New atcCollection
        For lIndex As Integer = 1 To aSourceUCI.FilesBlock.Count
            Dim lFile As HspfFile = aSourceUCI.FilesBlock.Value(lIndex)
            Dim lFileTyp As String = lFile.Typ
            If lFileTyp.StartsWith("WDM") Then
                If lFileTyp = "WDM" Then
                    lFileTyp = "WDM1"
                End If
                lFileName = AbsolutePath(lFile.Name, aSourceUCI.Name).ToLower
                lSourceWDMs.Add(lFileTyp, lFileName) 'todo: make standard full path 
            End If
        Next

        'build collection of wdms used by target uci
        Dim lTargetWDMs As New atcCollection
        For lIndex As Integer = 1 To aTargetUCI.FilesBlock.Count
            Dim lFile As HspfFile = aTargetUCI.FilesBlock.Value(lIndex)
            Dim lFileTyp As String = lFile.Typ
            If lFileTyp.StartsWith("WDM") Then
                If lFileTyp = "WDM" Then
                    lFileTyp = "WDM1"
                End If
                lFileName = AbsolutePath(lFile.Name, aTargetUCI.Name).ToLower
                lTargetWDMs.Add(lFileTyp, lFileName) 'todo: make standard full path 
            End If
        Next

        For Each lSConn As HspfConnection In aSourceUCI.Connections
            Dim lOutputVol As String = lSConn.Target.VolName
            If lOutputVol.StartsWith("WDM") Then
                If lOutputVol = "WDM" Then
                    lOutputVol = "WDM1"
                End If
                'translate wdm id into file name
                Dim lOutputWDMFileName As String = lSourceWDMs.ItemByKey(lOutputVol)
                Dim lOutputDsn As Integer = lSConn.Target.VolId
                Dim lInputWDMFileName As String = ""
                Dim lInputDsn As Integer = 0
                'now see if this wdm/dsn combination shows up as input to the target uci
                For Each lTConn As HspfConnection In aTargetUCI.Connections
                    Dim lInputVol As String = lTConn.Source.VolName
                    If lInputVol.StartsWith("WDM") Then
                        If lInputVol = "WDM" Then
                            lInputVol = "WDM1"
                        End If
                        'translate wdm id into file name
                        lInputWDMFileName = lTargetWDMs.ItemByKey(lInputVol)
                        lInputDsn = lTConn.Source.VolId
                        'do the check to see if these match
                        If lInputWDMFileName = lOutputWDMFileName AndAlso lInputDsn = lOutputDsn Then
                            'we have a match!
                            lConnections.Add(lInputWDMFileName & "|" & lInputDsn.ToString)
                        End If
                    End If
                Next
            End If
        Next

        Return lConnections
    End Function

End Module
