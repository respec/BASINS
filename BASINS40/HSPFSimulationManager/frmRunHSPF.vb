Imports atcUtility
Imports MapWinUtility

Public Class frmRunHSPF
    Friend SchematicDiagram As HSPFSimulationManager.ctlSchematic

    Private Sub frmRunHSPF_Load(sender As Object, e As EventArgs) Handles Me.Load
        If SchematicDiagram IsNot Nothing Then
            Dim lModels As New List(Of clsIcon)
            Dim lIcon As clsIcon
            For Each lIcon In SchematicDiagram.AllIcons
                lModels.Add(lIcon)
            Next
            Dim lIconIndex As Integer
SortDownstream:
            For lIconIndex = 0 To lModels.Count - 1
                lIcon = lModels(lIconIndex)
                If lIcon.DownstreamIcon IsNot Nothing Then
                    Dim lDownstreamIndex As Integer = lModels.IndexOf(lIcon.DownstreamIcon)
                    If lDownstreamIndex < lIconIndex Then
                        lModels.RemoveAt(lDownstreamIndex)
                        'After removing downstream icon from earlier in the list, lIconIndex now points at location just after lIcon
                        lModels.Insert(lIconIndex, lIcon.DownstreamIcon)
                        GoTo SortDownstream
                    End If
                End If
            Next
            For Each lIcon In lModels
                lstModels.Items.Add(lIcon, lIcon.Selected)
            Next
        End If
    End Sub

    Public Shared Sub RunUCI(ByVal aExeName As String, ByVal aUCIFilename As String)
        Dim lHspfExe As String = atcUtility.FindFile("Please locate " & aExeName, aExeName)
        If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
            lHspfExe = atcUtility.FindFile("Please locate " & aExeName, aExeName, , , True)
            If Not IO.File.Exists(lHspfExe) OrElse Not lHspfExe.ToLower.EndsWith(aExeName.ToLower) Then
                Logger.Msg("Unable to locate " & aExeName & ", not running.", aExeName)
                Exit Sub
            End If
        End If

        'MsgBox("Run: " & aExeName & " on " & aUCIFilename, MsgBoxStyle.OkOnly, "Skipping RunUCI")
        'Exit Sub

        Logger.Status("Running " & aUCIFilename & " (" & lHspfExe & ")")
        MapWinUtility.LaunchProgram(lHspfExe, IO.Path.GetDirectoryName(aUCIFilename), aUCIFilename)
    End Sub

    'Private Sub btnWinHSPF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWinHSPF.Click
    '    RunUCI("WinHSPF.exe", cboUciFiles.Text)
    'End Sub

    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        If lstModels.CheckedItems.Count = 0 Then
            Logger.Msg("No models selected to run", frmHspfSimulationManager.g_AppNameLong)
        Else
CheckUCIExists:
            Dim lNumFinishedRunning As Integer = 0
            Try
                Me.Visible = False
                Dim lNumToRun As Integer = 0
                Dim lAllUcisMissing As String = String.Empty
                Dim lAllWDMsWritten As New List(Of String)
                For Each lIcon As clsIcon In lstModels.CheckedItems
                    If IO.File.Exists(lIcon.UciFileName) Then
                        lNumToRun += 1
                        lAllWDMsWritten.AddRange(WDMsWritten(lIcon.UciFile))
                    Else
                        lAllUcisMissing &= lIcon.UciFileName & vbCrLf
                    End If
                Next
                'If lAllUcisMissing.Length > 0 Then
                '    Select Case Logger.Msg("UCI File Not Found: " & vbCrLf & lAllUcisMissing, _
                '                           MsgBoxStyle.AbortRetryIgnore, _
                '                           frmHspfSimulationManager.g_AppNameLong)
                '        Case MsgBoxResult.Abort : Exit Sub
                '        Case MsgBoxResult.Retry : GoTo CheckUCIExists
                '    End Select
                'End If

                Dim lConnectionsMissing As String = CheckSelectedConnections()
                If lConnectionsMissing.Length > 0 Then
                    Select Case Logger.Msg(lConnectionsMissing, _
                                           MsgBoxStyle.AbortRetryIgnore, _
                                           frmHspfSimulationManager.g_AppNameLong)
                        Case MsgBoxResult.Abort : Exit Sub
                        Case MsgBoxResult.Retry : GoTo CheckUCIExists
                    End Select
                End If

                Dim lMsg As String = "No output WDM files found"
                If lAllWDMsWritten.Count > 0 Then
                    lMsg = String.Join(vbCrLf, lAllWDMsWritten.ToArray())
                End If
                Select Case Logger.Msg(lMsg, MsgBoxStyle.OkCancel, "WDM Files To Be Written")
                    Case MsgBoxResult.Cancel : Exit Sub
                End Select

                For Each lIcon As clsIcon In lstModels.CheckedItems
                    If IO.File.Exists(lIcon.UciFileName) Then
                        RunUCI("WinHSPFlt.exe", lIcon.UciFileName)
                        If Not RunComplete(lIcon.UciFile) Then
                            Logger.Msg("HSPF Run Did Not Complete" & vbCrLf & lIcon.ToString(), _
                                       MsgBoxStyle.Critical, frmHspfSimulationManager.g_AppNameLong)
                            Exit Sub
                        End If
                        lNumFinishedRunning += 1
                        Logger.Progress(lNumFinishedRunning, lNumToRun)
                    End If
                Next
            Catch ex As Exception
                Logger.Msg("Error running HSPF: " & e.ToString, MsgBoxStyle.Critical, frmHspfSimulationManager.g_AppNameLong)
            Finally
                If lNumFinishedRunning > 0 Then
                    Me.Close()
                Else
                    Me.Visible = True
                End If
            End Try
            Dim lPlural As String
            If lNumFinishedRunning = 1 Then
                lPlural = ""
            Else
                lPlural = "s"
            End If
            Logger.Msg("Finished " & lNumFinishedRunning & " HSPF run" & lPlural, MsgBoxStyle.OkOnly, frmHspfSimulationManager.g_AppNameLong)
        End If
    End Sub

    Private Function CheckSelectedConnections() As String
        Dim lReport As String = ""
        Dim lReportLine As String
        Dim lIconIndex As Integer = 0
        For Each lIcon As clsIcon In SchematicDiagram.AllIcons
            If lstModels.CheckedItems.Contains(lIcon) OrElse lIcon.DownstreamIcon IsNot Nothing AndAlso lstModels.CheckedItems.Contains(lIcon.DownstreamIcon) Then
                Logger.Progress("Checking " & lIcon.WatershedName, lIconIndex, SchematicDiagram.AllIcons.Count)
                Dim lUpstreamUCI As atcUCI.HspfUci = lIcon.UciFile
                If lUpstreamUCI Is Nothing Then
                    lReportLine = "UCI file not found: " & lIcon.UciFileName
                    If Not lReport.Contains(lReportLine) Then
                        lReport &= lReportLine & vbCrLf
                    End If
                ElseIf lIcon.DownstreamIcon IsNot Nothing Then
                    Dim lDownstreamUCI As atcUCI.HspfUci = lIcon.DownstreamIcon.UciFile
                    If lDownstreamUCI IsNot Nothing Then
                        Dim lConnCheck As List(Of String) = modUCI.ConnectionSummary(lUpstreamUCI, lDownstreamUCI)
                        If lConnCheck Is Nothing OrElse lConnCheck.Count = 0 Then
                            lReport &= "No datasets found connecting " & vbCrLf & lIcon.UciFileName & vbCrLf & "to" & vbCrLf & lIcon.DownstreamIcon.UciFileName & vbCrLf
                        End If
                    End If
                End If
            End If
            lIconIndex += 1
        Next
        Logger.Progress(lIconIndex, lIconIndex)
        Return lReport
        'If lReport.Length > 0 Then
        '    MsgBox(lReport, MsgBoxStyle.OkOnly, "Connection Report")
        'End If

    End Function
End Class