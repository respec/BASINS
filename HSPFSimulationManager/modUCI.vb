Imports atcData
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
                                If Not lInputVol.StartsWith("WDMT") Then
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
                                                ElseIf lUsesTransfer <> lInputWDMFileName
                                                    lUsesTransfer = "MULTIPLE"
                                                End If
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

    Public Sub ConnectionsToTransferWDM(ByVal aTransferWDMName As String, ByVal aUCIs As atcCollection)

        'create transfer wdm if it does not already exist
        Dim lTransferWDMName As String = aTransferWDMName
        Dim lTransferWDM As atcWDM.atcDataSourceWDM
        lTransferWDM = atcDataManager.DataSourceBySpecification(IO.Path.GetFullPath(lTransferWDMName))
        If lTransferWDM Is Nothing Then 'need to open it here
            lTransferWDM = New atcWDM.atcDataSourceWDM
            If Not lTransferWDM.Open(lTransferWDMName) Then
                lTransferWDM = Nothing
            End If
        End If

        'now find connecting data sets and create copies in transfer wdm
        For Each lTargetUCI As HspfUci In aUCIs
            For Each lSourceUCI As HspfUci In aUCIs
                If lSourceUCI.Name <> lTargetUCI.Name Then
                    Dim lConnections As New atcCollection
                    'Do a check on just these 2 UCIs to see if they already use this transfer WDM
                    Dim lUcis As New atcCollection
                    lUcis.Add(lSourceUCI)
                    lUcis.Add(lTargetUCI)
                    Dim lTrans As String = UsesTransfer(lUcis)
                    If Not lTrans.ToLower = aTransferWDMName.ToLower Then
                        'they don't use the transfer wdm yet, so update them
                        lConnections = FindConnections(lSourceUCI, lTargetUCI, lTransferWDM)
                    End If
                End If
            Next
        Next
    End Sub

    Private Function FindConnections(ByVal aSourceUCI As HspfUci, ByVal aTargetUCI As HspfUci, ByVal aTransferWDM As atcDataSource) As atcCollection
        Dim lConnections As New atcCollection

        'build collection of wdms used by source uci 
        Dim lSourceWDMs As New atcCollection
        For lIndex As Integer = 1 To aSourceUCI.FilesBlock.Count
            Dim lFile As HspfFile = aSourceUCI.FilesBlock.Value(lIndex)
            Dim lFileTyp As String = lFile.Typ
            If lFileTyp.StartsWith("WDM") Then
                If lFileTyp = "WDM" Then
                    lFileTyp = "WDM1"
                End If
                lSourceWDMs.Add(lFileTyp, AbsolutePath(lFile.Name.Trim, PathNameOnly(aSourceUCI.Name)).ToLower)
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
                lTargetWDMs.Add(lFileTyp, AbsolutePath(lFile.Name.Trim, PathNameOnly(aTargetUCI.Name)).ToLower)
            End If
        Next

        Dim lUpdateCounter As Integer = 0
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
                        If lInputDsn = lOutputDsn Then
                            If lInputWDMFileName = lOutputWDMFileName Then
                                'we have a match!
                                lConnections.Add(lOutputVol & "|" & lInputVol & "|" & lInputWDMFileName & "|" & lInputDsn.ToString)

                                If aTransferWDM IsNot Nothing Then
                                    'make this connection through a tranfer wdm
                                    lUpdateCounter += 1
                                    lSConn.Target.VolName = "WDMT"
                                    lTConn.Source.VolName = "WDMT"

                                    Dim lSourceDataSource As New atcWDM.atcDataSourceWDM
                                    Dim lSourceDataSet As atcTimeseries = Nothing
                                    If lSourceDataSource.Open(lInputWDMFileName) Then
                                        Dim lSourceDataSetIndex As Integer = lSourceDataSource.DataSets.IndexFromKey(lInputDsn)
                                        If lSourceDataSetIndex >= 0 Then
                                            lSourceDataSet = lSourceDataSource.DataSets.ItemByIndex(lSourceDataSetIndex)
                                            lSourceDataSource.ReadData(lSourceDataSet)
                                        End If
                                        If lSourceDataSet Is Nothing Then
                                            Logger.Dbg("CopyDataSet:FailedToOpenSourceId " & lInputDsn & " from " & lInputWDMFileName)
                                        End If
                                    Else
                                        Logger.Dbg("CopyDataSet:FailedToOpenSource " & lInputWDMFileName)
                                    End If

                                    If lSourceDataSet IsNot Nothing Then
                                        'is this dsn already taken?
                                        Dim lFreeDsn As Integer = CopyDataSet(lSourceDataSet, aTransferWDM, lOutputDsn, atcDataSource.EnumExistAction.ExistRenumber)

                                        If lFreeDsn <> lOutputDsn Then
                                            'we needed to change the dsn because the original one is in use in the transfer wdm
                                            lSConn.Target.VolId = lFreeDsn
                                            lTConn.Source.VolId = lFreeDsn
                                        End If

                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        Next

        Return lConnections
    End Function

    Private Function CopyDataSet(ByVal aSourceDataSet As atcDataSet,
                            ByVal aTargetSource As atcDataSource,
                            ByVal aTargetId As Integer,
                            Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Integer
        Dim lCopyDataSet As Integer = 0
        Dim lNewDsn As Integer = aTargetId

        'just change id in the copy
        aSourceDataSet.Attributes.SetValue("id", aTargetId)

        Dim lDsnExists As Boolean = aTargetSource.DataSets.Keys.Contains(aTargetId)
        If lDsnExists Then
            Dim lDatasets As atcTimeseriesGroup = aTargetSource.DataSets
            While lDatasets.Keys.Contains(lNewDsn)
                lNewDsn += 1
                If lNewDsn > 9999 Then
                    lNewDsn = 1
                End If
            End While
            aSourceDataSet.Attributes.SetValue("Id", lNewDsn)
        End If

        Dim lResult As Boolean = aTargetSource.AddDataSet(aSourceDataSet, aExistAction)
        If lResult Then
            lCopyDataSet = lNewDsn
        End If

        Logger.Dbg("CopyDataSet:Add " & aSourceDataSet.ToString & " as ID " & lCopyDataSet & " Result:" & lResult)
        Return lCopyDataSet
    End Function

    Public Sub RemoveUnusedWDMs(ByVal aUCIs As atcCollection)
        For Each lTargetUCI As HspfUci In aUCIs
            Dim lIndex As Integer = 1
            Do While lIndex < lTargetUCI.FilesBlock.Count
                Dim lFile As HspfFile = lTargetUCI.FilesBlock.Value(lIndex)
                Dim lFileTyp As String = lFile.Typ
                If lFileTyp.StartsWith("WDM") Then
                    If lFileTyp = "WDM" Then
                        lFileTyp = "WDM1"
                    End If
                    'is this wdm actually in use?
                    Dim lInUse As Boolean = False
                    For Each lConn As HspfConnection In lTargetUCI.Connections
                        Dim lVol As String = lConn.Source.VolName
                        If lVol.StartsWith("WDM") Then
                            If lVol = "WDM" Then
                                lVol = "WDM1"
                            End If
                        End If
                        If lVol = lFileTyp Then
                            lInUse = True
                        End If
                        lVol = lConn.Target.VolName
                        If lVol.StartsWith("WDM") Then
                            If lVol = "WDM" Then
                                lVol = "WDM1"
                            End If
                        End If
                        If lVol = lFileTyp Then
                            lInUse = True
                        End If
                    Next
                    If lInUse Then
                        lIndex += 1
                    Else
                        lTargetUCI.FilesBlock.Remove(lIndex - 1)
                    End If
                Else
                    lIndex += 1
                End If
            Loop
        Next
    End Sub

    Public Sub AddTransferWDMtoFilesBlock(ByVal aTransferWDMName As String, ByVal aUCIs As atcCollection)
        For Each lUCI As HspfUci In aUCIs

            Dim lNextWDM As String = ""
            Dim lWDMUnit As Integer = 0

            'see if this transfer WDM is already in the files block
            Dim lInFilesBlock As Boolean = False
            For lIndex As Integer = 1 To lUCI.FilesBlock.Count
                Dim lFile As HspfFile = lUCI.FilesBlock.Value(lIndex)
                If Trim(RelativeFilename(lFile.Name, PathNameOnly(lUCI.Name))).ToLower = Trim(RelativeFilename(aTransferWDMName, PathNameOnly(lUCI.Name))).ToLower Then
                    lInFilesBlock = True
                    lNextWDM = lFile.Typ
                    lWDMUnit = lFile.Unit
                End If
            Next

            If Not lInFilesBlock Then
                'figure out the next free WDM position (wdm1, wdm2, wdm3 or wdm4)
                Dim lWDMinUse(4) As Boolean

                lWDMinUse(1) = False
                lWDMinUse(2) = False
                lWDMinUse(3) = False
                lWDMinUse(4) = False
                For lIndex As Integer = 1 To lUCI.FilesBlock.Count
                    Dim lFile As HspfFile = lUCI.FilesBlock.Value(lIndex)
                    Dim lFileTyp As String = lFile.Typ
                    If lFileTyp.StartsWith("WDM") Then
                        If lFileTyp = "WDM" Then
                            lFileTyp = "WDM1"
                        End If
                        If lFileTyp = "WDM1" Then
                            lWDMinUse(1) = True
                            lWDMUnit = lFile.Unit
                        End If
                        If lFileTyp = "WDM2" Then
                            lWDMinUse(2) = True
                            lWDMUnit = lFile.Unit
                        End If
                        If lFileTyp = "WDM3" Then
                            lWDMinUse(3) = True
                            lWDMUnit = lFile.Unit
                        End If
                        If lFileTyp = "WDM4" Then
                            lWDMinUse(4) = True
                            lWDMUnit = lFile.Unit
                        End If
                    End If
                Next

                If Not lWDMinUse(1) Then
                    lNextWDM = "WDM1"
                ElseIf Not lWDMinUse(2) Then
                    lNextWDM = "WDM2"
                ElseIf Not lWDMinUse(3) Then
                    lNextWDM = "WDM3"
                ElseIf Not lWDMinUse(4) Then
                    lNextWDM = "WDM4"
                End If
                If lNextWDM.Length = 0 Then
                    'problem, no place to put another wdm file
                End If

                'figure out what unit number to use
                Dim lFoundUnit As Boolean = True
                Do Until Not lFoundUnit
                    lFoundUnit = False
                    lWDMUnit += 1
                    'is this unit in use?
                    For lIndex As Integer = 1 To lUCI.FilesBlock.Count
                        Dim lFile As HspfFile = lUCI.FilesBlock.Value(lIndex)
                        If lFile.Unit = lWDMUnit Then
                            lFoundUnit = True
                        End If
                    Next
                Loop

                If lWDMUnit > 0 Then
                    'add this line to the files block
                    Dim lNewFile As New HspfFile
                    lNewFile.Name = aTransferWDMName
                    lNewFile.Typ = lNextWDM
                    lNewFile.Unit = lWDMUnit
                    lUCI.FilesBlock.Add(lNewFile)
                End If
            End If

            'now change every occurance of 'WDMT' to lNextWDM
            For Each lConn As HspfConnection In lUCI.Connections
                If lConn.Target.VolName = "WDMT" Then
                    lConn.Target.VolName = lNextWDM
                End If
                If lConn.Source.VolName = "WDMT" Then
                    lConn.Source.VolName = lNextWDM
                End If
            Next

        Next
    End Sub

End Module
