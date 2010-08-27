Imports MapWinUtility
Imports atcData
Imports atcSegmentation
Imports atcUCI
Imports atcUCIForms
Imports atcUtility
Imports System.Collections.ObjectModel

Public Module WinHSPF
    Friend pUCI As HspfUci
    Friend pMsg As HspfMsg
    Friend pIcon As Icon
    Friend pDefUCI As HspfUci
    Friend pPollutantList As Collection
    Friend pUCIFullFileName As String

    'Variableize each form to prevent multiple open and facilitate BringToFront if already open
    Friend pfrmReach As frmReach
    Friend pfrmAbout As frmAbout
    Friend pfrmActivityAll As frmActivityAll
    Friend pfrmAddExpert As frmAddExpert
    Friend pfrmAddMet As frmAddMet
    Friend pfrmEditMet As frmAddMet
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

    Friend pWinHSPFManualName As String
    Friend pHSPFManualName As String
    'Friend pIPC As ATCoIPC

    Sub Main()
        pDefUCI = New HspfUci
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        If Not FileExists(Logger.FileName) Then
            Dim lLogFolder As String = lBasinsBinLoc
            If lLogFolder.EndsWith("Debug") Then lLogFolder = PathNameOnly(lLogFolder)
            lLogFolder = PathNameOnly(lLogFolder)
            Logger.StartToFile(IO.Path.Combine(IO.Path.Combine(lLogFolder, "logs"), "WinHSPF.log"))
        End If

        'set WinHSPF manual location
        pWinHSPFManualName = lBasinsBinLoc & "\..\..\..\docs\WinHSPF30.chm"
        If Not FileExists(pWinHSPFManualName) Then
            pWinHSPFManualName = lBasinsFolder & "\docs\WinHSPF30.chm"
            If Not FileExists(pWinHSPFManualName) Then
                pWinHSPFManualName = FindFile("Please locate WinHSPF30.chm", "WinHSPF30.chm")
            End If
        End If
        ShowHelp(pWinHSPFManualName)

        'set HSPF manual location
        pHSPFManualName = lBasinsBinLoc & "\..\..\..\docs\HSPF.chm"
        If Not FileExists(pHSPFManualName) Then
            pHSPFManualName = lBasinsFolder & "\docs\HSPF.chm"
            If Not FileExists(pHSPFManualName) Then
                pHSPFManualName = FindFile("Please locate HSPF.chm", "HSPF.chm")
            End If
        End If

        'open hspf message mdb
        pMsg = New HspfMsg
        pMsg.Open("hspfmsg.mdb")
        Logger.Dbg("WinHSPF:Opened:hspfmsg.mdb")

        'get starter uci ready
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not FileExists(lStarterPath) Then
            lStarterPath = lBasinsFolder & "\models\hspf\bin\starter\" & lStarterUciName
            If Not FileExists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        pDefUCI.FastReadUciForStarter(pMsg, lStarterPath)

        'read pollutant list
        pPollutantList = New Collection
        ReadPollutantList()

        'show main form
        pWinHSPF = New frmWinHSPF
        pIcon = pWinHSPF.Icon

        'handle command line
        Dim lCommand As String = Command()
        If lCommand.Length > 0 Then
            If UCase(Right(lCommand, 4)) = ".UCI" Then
                OpenUCI(lCommand)
            ElseIf lCommand = "open" Then 'go to open uci prompt
                OpenUCI()
            End If
        End If
        pWinHSPF.ShowDialog()

    End Sub

    'Open an existing uci file
    Sub OpenUCI(Optional ByVal aFileName As String = "")
        If Not pUCI Is Nothing AndAlso pUCI.Name.Length > 0 Then
            'already have an active uci, warn user
            If Logger.Msg("Only one UCI can be active at a time." & vbCrLf & _
                          "Continuing will deactivate the current UCI." & vbCrLf & _
                          "Are you sure you want to continue?", MessageBoxButtons.OKCancel, "UCI Open Warning") = DialogResult.Cancel Then
                Exit Sub
            End If
        End If
        CloseUCI()
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not IO.File.Exists(aFileName) Then
            Dim lOpenDialog As New OpenFileDialog
            With lOpenDialog
                .Title = "Locate UCI file to open"
                .Filter = "UCI files|*.uci"
                .FilterIndex = 1
                .DefaultExt = ".uci"
                If .ShowDialog <> DialogResult.OK Then
                    pUCI = New HspfUci
                    Exit Sub
                End If
                aFileName = .FileName
            End With
        End If
        Dim lWorkingDir As String = IO.Path.GetDirectoryName(aFileName)
        ChDriveDir(lWorkingDir)
        Logger.Dbg("WinHSPF:WorkingDir:" & lWorkingDir & ":" & CurDir())
        pUCIFullFileName = aFileName

        pUCI = New HspfUci
        Dim lUCIName As String = IO.Path.GetFileName(aFileName)
        pUCI.FastReadUci(pMsg, lUCIName)
        'Dim lFilesOK As Boolean
        'Dim lFullFg As Integer = -3
        'Dim lEchoFile As String = ""
        'pUCI.ReadUci(pMsg, lUCIName, lFullFg, lFilesOK, lEchoFile)
        'set UCI name in caption
        If pWinHSPF IsNot Nothing Then
            pWinHSPF.Text = pWinHSPF.Tag & ": " & pUCI.Name
            pWinHSPF.SchematicDiagram.UCI = pUCI
            pWinHSPF.ResizeDiagram()
        End If
        Logger.Dbg("WinHSPF:FastReadUci:Done:" & lUCIName)
    End Sub

    'Create a new uci file
    Sub NewUCI()

        If Not pUCI Is Nothing AndAlso pUCI.Name.Length > 0 Then
            'already have an active uci, warn user
            If Logger.Msg("Only one UCI can be active at a time." & vbCrLf & _
                          "Continuing will deactivate the current UCI." & vbCrLf & _
                          "Are you sure you want to continue?", MessageBoxButtons.OKCancel, "New UCI Warning") = DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        CloseUCI()

        'get name of wsd file
        Dim lWatershedFileName As String = ""
        Dim lOpenDialog As New OpenFileDialog
        With lOpenDialog
            .Title = "Locate BASINS Watershed File to open"
            .Filter = "BASINS Watershed Files|*.wsd"
            .FilterIndex = 1
            .DefaultExt = ".wsd"
            If .ShowDialog <> DialogResult.OK Then
                Exit Sub
            End If
            lWatershedFileName = .FileName
        End With

        'get name of met wdm file
        Dim lMetWDMFileName As String = ""
        With lOpenDialog
            .Title = "Locate Met WDM File to open"
            .Filter = "Meteorologic WDM Files|*.wdm"
            .FilterIndex = 1
            .DefaultExt = ".wdm"
            .FileName = ""
            If .ShowDialog <> DialogResult.OK Then
                Exit Sub
            End If
            lMetWDMFileName = .FileName
        End With

        'open project wdm
        Dim lDataSources As New Collection(Of atcData.atcTimeseriesSource)
        Dim lProjectDataSource As New atcWDM.atcDataSourceWDM
        Dim lProjectWDMName As String = PathNameOnly(lWatershedFileName) & "\" & IO.Path.GetFileNameWithoutExtension(lWatershedFileName) & ".wdm"
        lProjectDataSource.Open(lProjectWDMName)
        lDataSources.Add(lProjectDataSource)

        'open met wdm
        Dim lMetDataSource As New atcWDM.atcDataSourceWDM
        lMetDataSource.Open(lMetWDMFileName)
        lDataSources.Add(lMetDataSource)

        'build new UCI from BASINS files
        Dim lUCIFileName As String = IO.Path.GetFileNameWithoutExtension(lWatershedFileName) & ".uci"
        ChDriveDir(PathNameOnly(lWatershedFileName))
        Dim lWatershedName As String = IO.Path.GetFileNameWithoutExtension(lUCIFileName)
        Dim lWatershed As New Watershed
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            pUCI = New HspfUci
            pUCI.Msg = pMsg
            pUCI.CreateUciFromBASINS(lWatershed, _
                                     lDataSources, _
                                     pDefUCI)
            pUCI.Save()
            pUCIFullFileName = lUCIFileName
            If pWinHSPF IsNot Nothing Then
                pWinHSPF.Text = pWinHSPF.Tag & ": " & pUCI.Name
                pWinHSPF.SchematicDiagram.UCI = pUCI
            End If
        End If

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

    Sub SimulationTimeMetDataEditor()
        If IsNothing(pfrmTime) Then
            pfrmTime = New frmTime
            pfrmTime.Show()
        Else
            If pfrmTime.IsDisposed Then
                pfrmTime = New frmTime
                pfrmTime.Show()
            Else
                pfrmTime.WindowState = FormWindowState.Normal
                pfrmTime.BringToFront()
            End If
        End If
    End Sub

    Sub LandUseEditor()
        If IsNothing(pfrmLand) Then
            pfrmLand = New frmLand
            pfrmLand.Show()
        Else
            If pfrmLand.IsDisposed Then
                pfrmLand = New frmLand
                pfrmLand.Show()
            Else
                pfrmLand.WindowState = FormWindowState.Normal
                pfrmLand.BringToFront()
            End If
        End If
    End Sub

    Sub EditControlCardsWithTables()
        If IsNothing(pfrmActivityAll) Then
            pfrmActivityAll = New frmActivityAll
            pfrmActivityAll.Show()
        Else
            If pfrmActivityAll.IsDisposed Then
                pfrmActivityAll = New frmActivityAll
                pfrmActivityAll.Show()
            Else
                pfrmActivityAll.WindowState = FormWindowState.Normal
                pfrmActivityAll.BringToFront()
            End If
        End If
    End Sub

    Sub EditControlCardsWithDescriptions()
        If IsNothing(pfrmControl) Then
            pfrmControl = New frmControl
            pfrmControl.Show()
        Else
            If pfrmControl.IsDisposed Then
                pfrmControl = New frmControl
                pfrmControl.Show()
            Else
                pfrmControl.WindowState = FormWindowState.Normal
                pfrmControl.BringToFront()
            End If
        End If
    End Sub

    Sub PollutantSelector()
        If IsNothing(pfrmPollutant) Then
            pUCI.PollutantsBuild()
            pfrmPollutant = New frmPollutant
            PollutantSelectorShow()
        Else
            If pfrmPollutant.IsDisposed Then
                pUCI.PollutantsBuild()
                pfrmPollutant = New frmPollutant
                PollutantSelectorShow()
            Else
                pfrmPollutant.WindowState = FormWindowState.Normal
                pfrmPollutant.BringToFront()
            End If
        End If
    End Sub

    Sub PollutantSelectorShow()
        pfrmPollutant.ShowDialog()
        pUCI.PollutantsUnBuild()
        CheckAndAddMissingTables("PERLND")
        CheckAndAddMissingTables("IMPLND")
        CheckAndAddMissingTables("RCHRES")
        UpdateFlagDependencies("RCHRES")
        SetMissingValuesToDefaults(pUCI, pDefUCI)
    End Sub

    Sub PointSourceEditor()
        If IsNothing(pfrmPoint) Then
            pfrmPoint = New frmPoint
            pfrmPoint.Show()
        Else
            If pfrmPoint.IsDisposed Then
                pfrmPoint = New frmPoint
                pfrmPoint.Show()
            Else
                pfrmPoint.WindowState = FormWindowState.Normal
                pfrmPoint.BringToFront()
            End If
        End If
    End Sub

    Sub InputDataEditor()
        If IsNothing(pfrmInputDataEditor) Then
            pfrmInputDataEditor = New frmInputDataEditor
            pfrmInputDataEditor.Show()
        Else
            If pfrmInputDataEditor.IsDisposed Then
                pfrmInputDataEditor = New frmInputDataEditor
                pfrmInputDataEditor.Show()
            Else
                pfrmInputDataEditor.WindowState = FormWindowState.Normal
                pfrmInputDataEditor.BringToFront()
            End If
        End If
    End Sub

    Sub OutputManager()
        If IsNothing(pfrmOutput) Then
            pfrmOutput = New frmOutput
            pfrmOutput.Show()
        Else
            If pfrmOutput.IsDisposed Then
                pfrmOutput = New frmOutput
                pfrmOutput.Show()
            Else
                pfrmOutput.WindowState = FormWindowState.Normal
                pfrmOutput.BringToFront()
            End If
        End If
    End Sub

    Sub RunHSPF()
        If pUCI.Edited Then
            If Logger.Msg("Changes have been made since your last Save." & vbCrLf & vbCrLf & _
                          "WinHSPF will save the changes before running.", MsgBoxStyle.OkCancel, _
                          "Confirm Save UCI") = MsgBoxResult.Cancel Then
                Exit Sub
            End If
            pUCI.Save()
        End If

        'DisableAll(True)
        pUCI.ClearAllOutputDsns()
        Dim lRetcod As Integer
        pUCI.RunUci(lRetcod)   'now activate and run
        'If lRetcod = -99 Then StartHSPFEngine()
        Dim lMsg As String = pUCI.ErrorDescription
        If lMsg.Trim.Length > 0 Then
            If Logger.Msg(lMsg & vbCrLf & vbCrLf & _
                          "Do you want to view the Echo file?", MsgBoxStyle.YesNo, _
                          "HSPF Problem") = MsgBoxResult.Yes Then
                Try
                    Process.Start(pUCI.EchoFileName.Trim)
                Catch
                    Logger.Msg("No application is associated with the Echo file.  It may be opened in a text editor.", vbOKOnly, "HSPF Problem")
                End Try
            End If
        End If
        'DisableAll(False)

    End Sub

    Sub ViewOutput()
        'Logger.Msg("'View Output' Feature not yet implemented.", vbOKOnly, "WinHSPF Problem")

        'If a mapwindow project exists (with the same base name), open it.

        'If there is no mapwindow project, create a new one, add the wdm files from this HSPF 
        'project to that new mapwindow project, and open that MapWindow project.

        Dim myProcesses() As Process
        myProcesses = Process.GetProcessesByName("MapWindow")
        If myProcesses.Length = 0 Then
            'open the mwprj if it exists, create it if not
            Dim lWinHSPFBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            Dim lMapWindowBinLoc As String = lWinHSPFBinLoc & "\..\..\..\bin\"
            Dim lMapWindowProjectName As String = FilenameSetExt(pUCIFullFileName, "mwprj")
            Dim lMapWindowExeName As String = lMapWindowBinLoc & "MapWindow.exe"

            If Not FileExists(lMapWindowExeName) Then
                lMapWindowExeName = FindFile("Please locate MapWindow.exe", "MapWindow.exe")
            End If

            If FileExists(lMapWindowProjectName) Then
                If FileExists(lMapWindowExeName) Then
                    Process.Start(lMapWindowExeName, lMapWindowProjectName)
                End If
            Else
                'create a script to run from the mapwindow command line
                If FileExists(lMapWindowExeName) Then
                    Dim lStr As String = "Imports MapWindow.Interfaces" & vbCrLf & _
                                         "Imports atcData" & vbCrLf & _
                                         "Imports atcUtility" & vbCrLf & vbCrLf & _
                                         "Module AutoLoad" & vbCrLf & vbCrLf & _
                                         "    Public Sub ScriptMain(ByRef aMapWin As IMapWin)" & vbCrLf & _
                                         "        Dim lFolder As String = " & """" & PathNameOnly(lMapWindowProjectName) & """" & vbCrLf & _
                                         "        ChDriveDir(lFolder)" & vbCrLf & vbCrLf

                    For lIndex As Integer = 0 To pUCI.FilesBlock.Count
                        Dim lFile As HspfFile = pUCI.FilesBlock.Value(lIndex)
                        If lFile.Typ.StartsWith("WDM") Then
                            lStr = lStr & "        atcDataManager.OpenDataSource(" & """" & lFile.Name.Trim & """" & ")" & vbCrLf
                        End If
                    Next
                    lStr = lStr & vbCrLf & "        aMapWin.Project.Save(" & """" & lMapWindowProjectName & """" & ")" & vbCrLf

                    lStr = lStr & vbCrLf & "    End Sub" & vbCrLf & _
                                           "End Module"

                    Dim lScriptName As String = PathNameOnly(lMapWindowProjectName) & "\AutoLoad.vb"

                    IO.File.WriteAllText(lScriptName, lStr)

                    Process.Start(lMapWindowExeName, lScriptName)
                End If
            End If
        Else
                Logger.Msg("A BASINS 4.0 MapWindow project is already open.", vbOKOnly, "WinHSPF View Output")
        End If

    End Sub

    Sub EditBlock(ByVal aParent As Windows.Forms.Form, ByVal aTableName As String)
        If aTableName = "GLOBAL" Then
            UCIForms.Edit(aParent, pUCI.GlobalBlock, aTableName, pHSPFManualName)
        ElseIf aTableName = "OPN SEQUENCE" Then
            UCIForms.Edit(aParent, pUCI.OpnSeqBlock, aTableName, pHSPFManualName)
        ElseIf aTableName = "FILES" Then
            UCIForms.Edit(aParent, pUCI.FilesBlock, aTableName, pHSPFManualName)
        ElseIf aTableName = "CATEGORY" Then
            UCIForms.Edit(aParent, pUCI.CategoryBlock, aTableName, pHSPFManualName)
        ElseIf aTableName = "FTABLES" Then
            If pUCI.OpnBlks("RCHRES").Count > 0 Then
                UCIForms.Edit(aParent, pUCI.OpnBlks("RCHRES").Ids(0).FTable, aTableName, pHSPFManualName)
            Else
                Logger.Message("The current project contains no reaches.", "FTable Editor Problem", MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
            End If
        ElseIf aTableName = "MONTH-DATA" Then
            UCIForms.Edit(aParent, pUCI.MonthData, aTableName, pHSPFManualName)
        ElseIf aTableName = "EXT SOURCES" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName, pHSPFManualName)
        ElseIf aTableName = "NETWORK" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName, pHSPFManualName)
        ElseIf aTableName = "SCHEMATIC" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName, pHSPFManualName)
        ElseIf aTableName = "EXT TARGETS" Then
            UCIForms.Edit(aParent, pUCI.Connections(0), aTableName, pHSPFManualName)
        ElseIf aTableName = "MASS-LINK" Then
            UCIForms.Edit(aParent, pUCI.MassLinks(0), aTableName, pHSPFManualName)
        ElseIf aTableName = "SPEC-ACTIONS" Then
            UCIForms.Edit(aParent, pUCI.SpecialActionBlk, aTableName, pHSPFManualName)
        Else
            Logger.Msg("Table/Block " & aTableName & " not found.", MsgBoxStyle.OkOnly, "Edit Problem")
        End If
    End Sub

    Function OperationKey(ByVal aOperation As HspfOperation) As String
        If aOperation Is Nothing Then
            Return ""
        Else
            Return aOperation.Name & " " & aOperation.Id
        End If
    End Function

    Public Sub CheckAndAddMissingTables(ByVal aOpName As String)

        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(aOpName)

        Dim lTablesRequiredMissing As System.Collections.ObjectModel.Collection(Of HspfStatusType)
        For Each lOper As HspfOperation In lOpnBlk.Ids
            'setting the collection forces build of tablestatus
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)
            lOper.TableStatus.Update() 'need to update in case we just changed flags
        Next

        Dim lTabname As String
        For Each lOper As HspfOperation In lOpnBlk.Ids
            lTablesRequiredMissing = lOper.TableStatus.GetInfo(1, False)

            For Each lStatus As HspfStatusType In lTablesRequiredMissing
                If lStatus.Occur > 1 Then
                    lTabname = lStatus.Name & ":" & lStatus.Occur
                Else
                    lTabname = lStatus.Name
                End If
                If lOpnBlk.Count > 0 Then
                    'double check to see if this table exists
                    If Not lOpnBlk.TableExists(lTabname) Then
                        lOpnBlk.AddTableForAll(lTabname, aOpName)
                        SetDefaultsForTable(pUCI, pDefUCI, aOpName, lTabname)
                    End If
                End If
            Next
        Next
        For Each lOper As HspfOperation In lOpnBlk.Ids
            lOper.TableStatus.Update()
        Next
    End Sub

    Public Sub SetDefaultsForTable(ByVal aUCI As HspfUci, ByVal aDefUCI As HspfUci, ByVal aOpName As String, ByVal TableName As String)

        If aUCI.OpnBlks(aOpName).Count > 0 Then
            Dim lOptyp As HspfOpnBlk = aUCI.OpnBlks(aOpName)
            For Each lOpn As HspfOperation In lOptyp.Ids
                Dim lId As Integer = aUCI.DefaultOpnId(lOpn, aDefUCI)
                If lId > 0 Then
                    Dim lDOpn As HspfOperation = aDefUCI.OpnBlks(lOpn.Name).OperFromID(lId)
                    If Not lDOpn Is Nothing Then
                        If lOpn.TableExists(TableName) Then
                            Dim lTab As HspfTable = lOpn.Tables(TableName)
                            If DefaultThisTable(lOptyp.Name, lTab.Name) Then
                                If lDOpn.TableExists(lTab.Name) Then
                                    Dim lDTab As HspfTable = lDOpn.Tables(lTab.Name)
                                    For Each lPar As HspfParm In lTab.Parms
                                        If DefaultThisParameter(lOptyp.Name, lTab.Name, lPar.Name) Then
                                            If lPar.Value <> lPar.Name Then
                                                lPar.Value = lDTab.Parms(lPar.Name).Value
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Function DefaultThisTable(ByVal aOperName As String, ByVal aTableName As String) As Boolean
        If aOperName = "PERLND" Or aOperName = "IMPLND" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "PWAT-PARM5" Then
                DefaultThisTable = False
            ElseIf Microsoft.VisualBasic.Left(aTableName, 4) = "QUAL" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        ElseIf aOperName = "RCHRES" Then
            If aTableName = "ACTIVITY" Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "HYDR-PARM1" Then
                DefaultThisTable = False
            ElseIf Microsoft.VisualBasic.Left(aTableName, 3) = "GQ-" Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        Else
            DefaultThisTable = False
        End If
    End Function

    Private Function DefaultThisParameter(ByVal aOperName As String, ByVal aTableName As String, ByVal aParmName As String) As Boolean
        DefaultThisParameter = True
        If aOperName = "PERLND" Then
            If aTableName = "PWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf aOperName = "IMPLND" Then
            If aTableName = "IWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf aOperName = "RCHRES" Then
            If aTableName = "HYDR-PARM2" Then
                If aParmName = "LEN" Or _
                   aParmName = "DELTH" Or _
                   aParmName = "FTBUCI" Then
                    DefaultThisParameter = False
                End If
            ElseIf aTableName = "GQ-GENDATA" Then
                If aParmName = "NGQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        End If
    End Function

    Public Sub UpdateFlagDependencies(ByVal aOpName As String)

        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(aOpName)
        For Each lOper As HspfOperation In lOpnBlk.Ids
            If lOper.TableExists("ACTIVITY") Then
                If lOper.Tables("ACTIVITY").Parms("SEDFG").Value = 1 Then
                    If lOper.TableExists("HYDR-PARM1") Then
                        'change aux flags
                        lOper.Tables("HYDR-PARM1").Parms("AUX1FG").Value = 1
                        lOper.Tables("HYDR-PARM1").Parms("AUX2FG").Value = 1
                        lOper.Tables("HYDR-PARM1").Parms("AUX3FG").Value = 1
                    End If
                End If
                If lOper.Tables("ACTIVITY").Parms("PLKFG").Value = 1 Then
                    If lOper.TableExists("NUT-FLAGS") Then
                        'change po4 flag
                        lOper.Tables("NUT-FLAGS").Parms("PO4FG").Value = 1
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub SetMissingValuesToDefaults(ByVal aUCI As HspfUci, ByVal aDefUCI As HspfUci)

        Dim lOpTyps() As String = {"PERLND", "IMPLND", "RCHRES"}

        For Each lOpTypName As String In lOpTyps
            If aUCI.OpnBlks(lOpTypName).Count > 0 Then
                Dim lOptyp As HspfOpnBlk = aUCI.OpnBlks(lOpTypName)
                For Each lOpn As HspfOperation In lOptyp.Ids
                    Dim lId = aUCI.DefaultOpnId(lOpn, aDefUCI)
                    If lId > 0 Then
                        Dim lDOpn As HspfOperation = aDefUCI.OpnBlks(lOpn.Name).OperFromID(lId)
                        If Not lDOpn Is Nothing Then
                            For Each lTab As HspfTable In lOpn.Tables
                                If lDOpn.TableExists(lTab.Name) Then
                                    Dim lDTab As HspfTable = lDOpn.Tables(lTab.Name)
                                    For Each lPar As HspfParm In lTab.Parms
                                        If lPar.Value.GetType.Name = "Double" AndAlso lPar.Value = -999.0# Then
                                            lPar.Value = lDTab.Parms(lPar.Name).Value
                                        End If
                                    Next lPar
                                End If
                            Next lTab
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Public Sub CheckAndAddMassLinks()

        'find the number of quals to be in the masslink block

        'pquals, npest
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("PERLND")
        Dim lPQuals As Integer = 0
        Dim lNPest As Integer = 0
        Dim i As Integer
        For Each lId As HspfOperation In lOpnBlk.Ids
            If lId.TableExists("ACTIVITY") Then
                Dim lTable As HspfTable = lId.Tables("ACTIVITY")
                If lTable.Parms("PQALFG").Value = 1 Then
                    lPQuals = 1
                End If
                If lTable.Parms("PESTFG").Value = 1 Then
                    lNPest = 1
                End If
            End If
            If lId.TableExists("NQUALS") Then
                i = lId.Tables("NQUALS").Parms("NQUAL").Value
                If i > lPQuals Then
                    lPQuals = i
                End If
            End If
            If lId.TableExists("PEST-FLAGS") Then
                i = lId.Tables("PEST-FLAGS").Parms("NPST").Value
                If i > lNPest Then
                    lNPest = i
                End If
            End If
        Next

        'iquals
        lOpnBlk = pUCI.OpnBlks("IMPLND")
        Dim lIQuals As Integer = 0
        For Each lId As HspfOperation In lOpnBlk.Ids
            If lId.TableExists("ACTIVITY") Then
                Dim lTable As HspfTable = lId.Tables("ACTIVITY")
                If lTable.Parms("IQALFG").Value = 1 Then
                    lIQuals = 1
                End If
            End If
            If lId.TableExists("NQUALS") Then
                i = lId.Tables("NQUALS").Parms("NQUAL").Value
                If i > lIQuals Then
                    lIQuals = i
                End If
            End If
        Next
        lOpnBlk = pUCI.OpnBlks("RCHRES")

        'gquals
        Dim lGQuals As Integer = 0
        For Each lId As HspfOperation In lOpnBlk.Ids
            If lId.TableExists("ACTIVITY") Then
                Dim lTable As HspfTable = lId.Tables("ACTIVITY")
                If lTable.Parms("GQALFG").Value = 1 Then
                    lGQuals = 1
                End If
            End If
            If lId.TableExists("GQ-GENDATA") Then
                i = lId.Tables("GQ-GENDATA").Parms("NGQUAL").Value
                If i > lGQuals Then
                    lGQuals = i
                End If
            End If
        Next

        Dim lNQuals As Integer = 0
        If lPQuals > 1 And lGQuals > 1 Then
            If lPQuals >= lGQuals Then
                lNQuals = lGQuals
            Else
                lNQuals = lPQuals
            End If
            'make sure we have the masslinks for these p to r connections
            CopyMLsForAdditionalQuals("PERLND", "PQUAL", lNQuals)
        End If

        If lIQuals > 1 And lGQuals > 1 Then
            If lIQuals >= lGQuals Then
                lNQuals = lGQuals
            Else
                lNQuals = lIQuals
            End If
            'make sure we have the masslinks for these i to r connections
            CopyMLsForAdditionalQuals("IMPLND", "IQUAL", lNQuals)
        End If

        If lNPest > 1 And lGQuals > 1 Then
            If lNPest >= lGQuals Then
                lNQuals = lGQuals
            Else
                lNQuals = lNPest
            End If
            'make sure we have the masslinks for these p to r connections
            CopyMLsForAdditionalQuals("PERLND", "PEST", lNQuals)
        End If

    End Sub

    Private Sub CopyMLsForAdditionalQuals(ByVal aOpName As String, ByVal aOpGroup As String, ByVal lNQuals As Integer)

        'copy the masslinks for 1 pqual/iqual to all additional pqual/iquals
        For Each lML As HspfMassLink In pUCI.MassLinks
            If lML.Source.VolName = aOpName And lML.Source.Group = aOpGroup And _
               lML.Target.VolName = "RCHRES" Then
                If (lML.Source.MemSub2 = 0 And lML.Source.MemSub1 = 1) Or _
                   lML.Source.MemSub2 = 1 Then
                    'need this record for each nqual
                    For i As Integer = 2 To lNQuals
                        Dim lFound As Boolean = False
                        For Each lML2 As HspfMassLink In pUCI.MassLinks 'see if we already have it
                            If lML2.Source.VolName = lML.Source.VolName And _
                               lML2.Source.Group = lML.Source.Group And _
                               lML2.Source.Member = lML.Source.Member And _
                               lML2.MFact = lML.MFact And _
                               lML2.Target.VolName = lML.Target.VolName And _
                               lML2.Target.Group = lML.Target.Group And _
                               lML2.Target.Member = lML.Target.Member And _
                               lML2.Source.MemSub1 = i Then
                                If lML.Target.MemSub2 > 0 Then
                                    If lML2.Target.MemSub1 = lML.Target.MemSub1 And _
                                      lML2.Target.MemSub2 = i Then
                                        lFound = True
                                    End If
                                Else
                                    If lML2.Target.MemSub1 = i Then
                                        lFound = True
                                    End If
                                End If
                            End If
                        Next
                        If lFound = False Then  'dont have it yet, so add it
                            Dim lNewML = New HspfMassLink
                            lNewML.MassLinkId = lML.MassLinkId
                            lNewML.MFact = lML.MFact
                            lNewML.Tran = lML.Tran
                            lNewML.Uci = lML.Uci
                            lNewML.Source.VolName = lML.Source.VolName
                            lNewML.Source.Group = lML.Source.Group
                            lNewML.Source.Member = lML.Source.Member
                            lNewML.Target.VolName = lML.Target.VolName
                            lNewML.Target.Group = lML.Target.Group
                            lNewML.Target.Member = lML.Target.Member
                            lNewML.Target.MemSub1 = lML.Target.MemSub1
                            lNewML.Source.MemSub1 = i
                            If lML.Target.MemSub2 > 0 Then
                                lNewML.Target.MemSub2 = i
                            Else
                                lNewML.Target.MemSub1 = i
                            End If
                            pUCI.MassLinks.Add(lNewML)
                        End If
                    Next i
                End If
            End If
        Next
    End Sub

    Private Sub ReadPollutantList()
        Dim lPollutantFileName As String = Nothing
        Dim lLineNumber As Integer = 0

        Try
            lPollutantFileName = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location) & "\Poltnt_2.prn"

            If Not FileExists(lPollutantFileName) Then
                lPollutantFileName = FindFile("Please locate Poltnt_2.prn", "Poltnt_2.prn")
            End If

            For Each lString As String In LinesInFile(lPollutantFileName)
                If lLineNumber = 0 Then
                    'skip the first line which is assumed to be a header
                    If InStr(lString, "PARM_CODE    PARM_NAME") < 1 Then
                        If Logger.Message("The header of this pollutant file: " & vbCrLf & lPollutantFileName & vbCrLf & "Does not match common fomating standards. Do you want to continue?", "Pollutant File Suspicious", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.Yes) = Windows.Forms.DialogResult.No Then
                            Exit Try
                        End If
                    End If
                Else
                    If lString.Length > 1 Then
                        pPollutantList.Add(lString)
                    End If
                End If
                lLineNumber += 1
            Next
        Catch ex As Exception
            pPollutantList.Clear()
            Logger.Message("There was an error reading the selected pollutant list." & vbCrLf & "Ensure that the pollutant file selected is formatted properly.", "Error Reading the pollutant file", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub
End Module
