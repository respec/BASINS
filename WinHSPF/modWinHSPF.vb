Imports MapWinUtility
Imports atcUCI
Imports atcUCIForms
Imports atcUtility

Public Module WinHSPF
    Friend pUCI As HspfUci
    Friend pMsg As HspfMsg
    Friend pIcon As Icon
    Friend pDefUCI As HspfUci
    Friend pPollutantList As Collection

    'Variableize each form to prevent multiple open and facilitate BringToFront if already open
    Friend pfrmReach As frmReach
    Friend pfrmAbout As frmAbout
    Friend pfrmActivityAll As frmActivityAll
    Friend pfrmAddExpert As frmAddExpert
    Friend pfrmAddMet As frmAddMet
    Friend pfrmControl As frmControl
    Friend pfrmInputDataEditor As frmInputDataEditor
    Friend pfrmLand As frmLand
    Friend pfrmOutput As frmOutput
    Friend pfrmPoint As frmPoint
    Friend pfrmAddPoint As frmAddPoint
    Friend pfrmImportPoint As frmImportPoint
    Friend pfrmTSnew As frmTSnew
    Friend pfrmPointScenario As frmPointScenario
    Friend pfrmPollutant As frmPollutant
    Friend pfrmTime As frmTime
    Friend pfrmAQUATOX As frmAQUATOX
    Friend pfrmBMP As frmBMP
    Friend pfrmBMPEffic As frmBMPEffic
    Friend pfrmHspfParm As frmHspfParm
    Friend pfrmSaveAs As frmSaveAs
    Friend pfrmStarter As frmStarter
    Friend pfrmXSect As frmXSect
    Friend pWinHSPF As frmWinHSPF

    'Friend pIPC As ATCoIPC

    Sub Main()
        Logger.StartToFile("C:\dev\basins40\logs\WinHSPF.log")

        'open hspf message mdb
        pMsg = New HspfMsg
        pMsg.Open("hspfmsg.mdb")
        Logger.Dbg("WinHSPF:Opened:hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not FileExists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not FileExists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        pDefUCI = New HspfUci
        pDefUCI.FastReadUciForStarter(pMsg, lStarterPath)

        'show main form
        pWinHSPF = New frmWinHSPF
        pIcon = pWinHSPF.Icon
        OpenUCI("D:\BASINS\modelout\sediment\sed_riv.uci")
        pWinHSPF.ShowDialog()
    End Sub

    'Open an existing uci file
    Sub OpenUCI(Optional ByVal aFileName As String = "")
        CloseUCI()
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not IO.File.Exists(aFileName) Then
            Dim lOpenDialog As New OpenFileDialog
            With lOpenDialog
                .Title = "Locate UCI file to open"
                .Filter = "UCI files|*.uci"
                .FilterIndex = 1
                .DefaultExt = ".uci"
                If .ShowDialog <> DialogResult.OK Then
                    Exit Sub
                End If
                aFileName = .FileName
            End With
        End If
        Dim lWorkingDir As String = IO.Path.GetDirectoryName(aFileName)
        ChDriveDir(lWorkingDir)
        Logger.Dbg("WinHSPF:WorkingDir:" & lWorkingDir & ":" & CurDir())

        pUCI = New HspfUci
        Dim lUCIName As String = IO.Path.GetFileName(aFileName)
        pUCI.FastReadUciForStarter(pMsg, lUCIName)
        'Dim lFilesOK As Boolean = True
        'Dim lEchoFile As String = ""
        'pUCI.ReadUci(pMsg, lUCIName, 1, lFilesOK, lEchoFile)
        'set UCI name in caption
        If pWinHSPF IsNot Nothing Then
            pWinHSPF.Text = pWinHSPF.Tag & ": " & pUCI.Name
            pWinHSPF.SchematicDiagram.UCI = pUCI
        End If
        Logger.Dbg("WinHSPF:FastReadUci:Done:" & lUCIName)
    End Sub

    Sub SaveUCI()
        pUCI.Save()
        Logger.Dbg("WinHSPF:SaveUci:Done:" & pUCI.Name)
    End Sub

    Sub CloseUCI()
        'TODO: confirm close if UCI has been edited
        pUCI = Nothing
        If pWinHSPF IsNot Nothing Then
            pWinHSPF.Text = pWinHSPF.Tag
            pWinHSPF.SchematicDiagram.UCI = pUCI
        End If
    End Sub

    Sub ReachEditor()

        If pUCI.OpnBlks("RCHRES").Count > 0 Then

            If IsNothing(pfrmReach) Then
                pfrmReach = New frmReach
                pfrmReach.Show()
            Else
                If pfrmReach.IsDisposed Then
                    pfrmReach = New frmReach
                    pfrmReach.Show()
                Else
                    pfrmReach.WindowState = FormWindowState.Normal
                    pfrmReach.BringToFront()
                End If
            End If

            With pWinHSPF.SchematicDiagram
                .UCI = pUCI
                'TODO: .UpdateLegend()
            End With

        Else
            Logger.Message("The current project contains no reaches.", "Reach Editor Problem", _
                           MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK)
        End If
    End Sub

    Sub PollutantSelectorCheck()
        If pUCI.Pollutants.Count > 0 Then
            frmPollutant.Show()
        Else
            '.net conversion issue: verify this is the right message to display.
            'Logger.Message("The current project contains no pollutant sources.", "Pollutant Selector Problem", _
            '               MessageBoxButtons.OK, MessageBoxIcon.Information, DialogResult.OK)
            frmPollutant.Show()
        End If
    End Sub

    Sub EditBlock(ByVal aParent As Windows.Forms.Form, ByVal aTableName As String)
        If aTableName = "GLOBAL" Then
            UCIForms.Edit(aParent, pUCI.GlobalBlock)
        ElseIf aTableName = "OPN SEQUENCE" Then
            UCIForms.Edit(aParent, pUCI.OpnSeqBlock)
        ElseIf aTableName = "FILES" Then
            UCIForms.Edit(aParent, pUCI.FilesBlock)
        ElseIf aTableName = "CATEGORY" Then
            UCIForms.Edit(aParent, pUCI.CategoryBlock)
        ElseIf aTableName = "FTABLES" Then
            If pUCI.OpnBlks("RCHRES").Count > 0 Then
                UCIForms.Edit(aParent, pUCI.OpnBlks("RCHRES").Ids(0).FTable)
            Else
                Logger.Message("The current project contains no reaches.", "FTable Editor Problem", MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
            End If
        ElseIf aTableName = "MONTH-DATA" Then
            UCIForms.Edit(aParent, pUCI.MonthData)
        ElseIf aTableName = "EXT SOURCES" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "NETWORK" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "SCHEMATIC" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "EXT TARGETS" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName)
        ElseIf aTableName = "MASS-LINK" Then
            UCIForms.Edit(aParent, pUCI.MassLinks(0), aTableName)
        ElseIf aTableName = "SPEC-ACTIONS" Then
            UCIForms.Edit(aParent, pUCI.SpecialActionBlk, aTableName)
        Else
            Logger.Msg("Table/Block " & aTableName & " not found.", MsgBoxStyle.OkOnly, "Edit Problem")
        End If
    End Sub

    Function OperationKey(ByVal aOperation As HspfOperation) As String
        Return aOperation.Name & " " & aOperation.Id
    End Function
End Module
