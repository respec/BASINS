Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Module modUCI

    'Global variables, not just for UCI. TODO: move to new global module
    Friend Const g_AppNameShort As String = "HspfSimulationManager"
    Friend Const g_AppNameLong As String = "SARA HSPF Simulation Manager"
    Friend g_ProgramDir As String = ""
    Friend g_StatusMonitor As MonitorProgressStatus

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

    Public Function SimulationPeriodString(aUci As atcUCI.HspfUci) As String
        Dim lSJDate As Double = aUci.GlobalBlock.SDateJ
        Dim lEJDate As Double = aUci.GlobalBlock.EdateJ
        Dim lSDate(6) As Integer
        Dim lEDate(6) As Integer
        J2Date(lSJDate, lSDate)
        J2Date(lEJDate, lEDate)
        atcUtility.modDate.timcnv(lEDate)
        Return lSDate(0) & "/" & lSDate(1) & "/" & lSDate(2) & " to " & lEDate(0) & "/" & lEDate(1) & "/" & lEDate(2)
    End Function

    Public Function ConnectionReport(aUpstreamUCI As atcUCI.HspfUci, aDownstreamUCI As atcUCI.HspfUci) As String
        Dim lReport As String = String.Empty
        If aUpstreamUCI Is Nothing Then
            lReport &= "Upstream UCI file not found" & vbCrLf
        Else
            lReport &= "Upstream UCI file: " & vbCrLf & aUpstreamUCI.Name & vbCrLf & vbCrLf
            If aDownstreamUCI Is Nothing Then
                lReport &= "Downstream UCI file not found" & vbCrLf
            Else
                lReport &= "Downstream UCI file:" & vbCrLf & aDownstreamUCI.Name & vbCrLf & vbCrLf
                lReport &= "Upstream Simulation Period:    " & vbTab & SimulationPeriodString(aUpstreamUCI) & vbCrLf
                lReport &= "Downstream Simulation Period: " & vbTab & SimulationPeriodString(aDownstreamUCI) & vbCrLf & vbCrLf
                Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(aUpstreamUCI, aDownstreamUCI)
                If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                    lReport &= "No connecting datasets found." & vbCrLf
                Else
                    Dim lWDMFileName As String = String.Empty
                    For Each lReportLine In lConnCheck
                        Dim lFields() As String = lReportLine.Split("|"c)
                        If lFields(0) <> lWDMFileName Then
                            lWDMFileName = lFields(0)
                            lReport &= "WDM file: " & vbCrLf & lWDMFileName & vbCrLf
                        End If
                        For lField = 1 To lFields.Length - 1
                            lReport &= vbTab & lFields(lField)
                        Next
                        lReport &= vbCrLf
                    Next
                End If
            End If
        End If
        Return lReport
    End Function

    Public Function ConnectionSummary(ByVal aSourceUCI As HspfUci, ByVal aTargetUCI As HspfUci) As List(Of String)
        Dim lSourceWDMs As atcCollection = AllWDMs(aSourceUCI)
        Dim lTargetWDMs As atcCollection = AllWDMs(aTargetUCI)

        Dim lConnections As New List(Of String)

        For Each lSConn As HspfConnection In aSourceUCI.Connections
            Dim lOutputVol As String = GetVolName(lSConn.Target.VolName)
            If lOutputVol.StartsWith("WDM") Then
                'translate wdm id into file name
                Dim lOutputWDMFileName As String = lSourceWDMs.ItemByKey(lOutputVol)
                Dim lOutputDsn As Integer = lSConn.Target.VolId
                Dim lInputWDMFileName As String = ""
                Dim lInputDsn As Integer = 0
                'now see if this wdm/dsn combination shows up as input to the target uci
                For Each lTConn As HspfConnection In aTargetUCI.Connections
                    Dim lInputVol As String = GetVolName(lTConn.Source.VolName)
                    If lInputVol.StartsWith("WDM") Then
                        'translate wdm id into file name
                        lInputWDMFileName = lTargetWDMs.ItemByKey(lInputVol)
                        lInputDsn = lTConn.Source.VolId
                        'do the check to see if these match
                        If lInputWDMFileName.ToLowerInvariant = lOutputWDMFileName.ToLowerInvariant AndAlso lInputDsn = lOutputDsn Then
                            'we have a match!
                            lConnections.Add(lInputWDMFileName & "|" & lInputDsn.ToString & "|" & _
                                             lSConn.Source.VolName & ":" & lSConn.Source.VolId & ":" & lSConn.Target.Member & " -> " & _
                                             lTConn.Target.VolName & ":" & lTConn.Target.VolId & ":" & lTConn.Source.Member)
                        End If
                    End If
                Next
            End If
        Next

        Return lConnections
    End Function

    Private Function GetVolName(aUciVolName As String) As String
        If aUciVolName = "WDM" Then
            Return "WDM1"
        Else
            Return aUciVolName
        End If
    End Function

    Public Function WDMsWritten(ByVal aUCI As HspfUci) As List(Of String)
        Dim lAllWDMs As atcCollection = AllWDMs(aUCI)
        Dim lWDMsWritten As New List(Of String)

        For Each lSConn As HspfConnection In aUCI.Connections
            Dim lOutputVol As String = GetVolName(lSConn.Target.VolName)
            If lOutputVol.StartsWith("WDM") Then
                'translate wdm id into file name
                Dim lOutputWDMFileName As String = lAllWDMs.ItemByKey(lOutputVol)
                If Not lWDMsWritten.Contains(lOutputWDMFileName) Then
                    lWDMsWritten.Add(lOutputWDMFileName)
                End If
            End If
        Next

        Return lWDMsWritten
    End Function

    Private Function AllWDMs(aUCI As HspfUci) As atcCollection
        Dim lAllWDMs As New atcCollection
        Dim lFileName As String
        For lIndex As Integer = 1 To aUCI.FilesBlock.Count
            Dim lFile As HspfFile = aUCI.FilesBlock.Value(lIndex)
            Dim lFileTyp As String = GetVolName(lFile.Typ)
            If lFileTyp.StartsWith("WDM") Then
                lFileName = AbsolutePath(Trim(lFile.Name), PathNameOnly(aUCI.Name))
                lAllWDMs.Add(lFileTyp, lFileName)
            End If
        Next
        Return lAllWDMs
    End Function

    Public Function UsesTransfer(ByVal aUCIs As atcCollection) As String
        'return blank if models are not connected
        'return name of transfer wdm if models are connected using a single transfer wdm
        'return 'MULTIPLE' if models are connected but connections use multiple wdms 
        Dim lUsesTransfer As String = ""

        For Each lTargetUCI As HspfUci In aUCIs
            For Each lSourceUCI As HspfUci In aUCIs
                If lSourceUCI.Name <> lTargetUCI.Name Then

                    'build collection of wdms used by source uci 
                    Dim lSourceWDMs As New atcCollection
                    For lIndex As Integer = 1 To lSourceUCI.FilesBlock.Count
                        Dim lFile As HspfFile = lSourceUCI.FilesBlock.Value(lIndex)
                        Dim lFileTyp As String = lFile.Typ
                        If lFileTyp.StartsWith("WDM") Then
                            If lFileTyp = "WDM" Then
                                lFileTyp = "WDM1"
                            End If
                            lSourceWDMs.Add(lFileTyp, AbsolutePath(lFile.Name.Trim, PathNameOnly(lSourceUCI.Name)).ToLower)
                        End If
                    Next

                    'build collection of wdms used by target uci
                    Dim lTargetWDMs As New atcCollection
                    For lIndex As Integer = 1 To lTargetUCI.FilesBlock.Count
                        Dim lFile As HspfFile = lTargetUCI.FilesBlock.Value(lIndex)
                        Dim lFileTyp As String = lFile.Typ
                        If lFileTyp.StartsWith("WDM") Then
                            If lFileTyp = "WDM" Then
                                lFileTyp = "WDM1"
                            End If
                            lTargetWDMs.Add(lFileTyp, AbsolutePath(lFile.Name.Trim, PathNameOnly(lTargetUCI.Name)).ToLower)
                        End If
                    Next

                    Dim lUpdateCounter As Integer = 0
                    For Each lSConn As HspfConnection In lSourceUCI.Connections
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
                            For Each lTConn As HspfConnection In lTargetUCI.Connections
                                Dim lInputVol As String = lTConn.Source.VolName
                                If lInputVol.StartsWith("WDM") Then
                                    If lInputVol = "WDM" Then
                                        lInputVol = "WDM1"
                                    End If
                                    'translate wdm id into file name
                                    lInputWDMFileName = lTargetWDMs.ItemByKey(lInputVol)
                                    lInputDsn = lTConn.Source.VolId
                                    'do the check to see if these match
                                    If lInputDsn = lOutputDsn Then
                                        If lInputWDMFileName = lOutputWDMFileName Then
                                            'we have a match!
                                            If lUsesTransfer.Length = 0 Then
                                                lUsesTransfer = lInputWDMFileName
                                            Else
                                                lUsesTransfer = "MULTIPLE"
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next

                End If
            Next
        Next
        Return lUsesTransfer
    End Function

End Module
