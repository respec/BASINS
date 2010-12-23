Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWAT
    Implements clsCatModel

    Public SWATProgramBase As String = "C:\Program Files\SWAT 2005 Editor" & g_PathChar

    Public Event BaseModelSet(ByVal aBaseModel As String) Implements clsCatModel.BaseModelSet

    Private pBaseModel As String = Nothing
    Private pSWATDatabaseName As String = Nothing
    Private pMetWDM As atcWDM.atcDataSourceWDM
    Private pMetPcp As atcTimeseriesSWAT.atcTimeseriesSWAT
    Private pMetTmp As atcTimeseriesSWAT.atcTimeseriesSWAT
    Private pBaseOutputHruFileName As String = Nothing
    Private pBaseOutputRchFileName As String = Nothing
    Private pBaseOutputSubFileName As String = Nothing
    Private pSWATEXE As String = "swat2005.exe"

    Public Property SwatExe() As String
        Get
            Return pSWATEXE
        End Get
        Set(ByVal value As String)
            pSWATEXE = value
        End Set
    End Property

    Public Property BaseModel() As String Implements clsCatModel.BaseModel
        Get
            Return pBaseModel
        End Get
        Set(ByVal newValue As String)
            OpenBaseModel(newValue)
        End Set
    End Property

    Friend Sub OpenBaseModel(Optional ByVal aFilename As String = "")
        If pBaseOutputHruFileName IsNot Nothing AndAlso _
           pBaseOutputRchFileName IsNot Nothing AndAlso _
           pBaseOutputSubFileName IsNot Nothing Then
            GoTo ALREADYSET
        End If

        pBaseOutputHruFileName = Nothing
        pBaseOutputRchFileName = Nothing
        pBaseOutputSubFileName = Nothing

        If aFilename IsNot Nothing AndAlso Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".mdb") Then aFilename &= ".mdb"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open SWAT file containing base model"
            cdlg.Filter = "SWAT mdb files|*.mdb|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Logger.Dbg("Could not find SWAT base model MDB.")
        Else
            If SWATDatabasePath Is Nothing OrElse Not IO.File.Exists(SWATDatabasePath) Then
                Logger.Dbg("Could not find SWAT database path.")
            End If

            If (pBaseModel Is Nothing OrElse aFilename.ToLower <> pBaseModel.ToLower) Then
                Dim lFolder As String = PathNameOnly(aFilename)
                ChDriveDir(lFolder)
                pBaseModel = aFilename
                RaiseEvent BaseModelSet(aFilename)

                'Find and open met data WDM file
                Dim lWDMfilename As String = IO.Path.GetDirectoryName(aFilename) & "\met.wdm"
                If Not IO.File.Exists(lWDMfilename) Then
                    lWDMfilename = IO.Path.Combine(IO.Path.GetDirectoryName(aFilename), "met\met.wdm")
                End If
OpenMetWDM:
                If IO.File.Exists(lWDMfilename) Then
                    pMetWDM = clsCat.OpenDataSource(lWDMfilename)
                Else
                    Select Case Logger.Msg("Did not find '" & lWDMfilename & "'" & vbCrLf _
                                & "Yes to browse for met data WDM" & vbCrLf _
                                & "No to use SWAT met data directly" & vbCrLf _
                                & "Cancel to stop opening SWAT base model", vbYesNoCancel, "SWAT Met WDM Not Found")
                        Case MsgBoxResult.Yes
                            Dim lFileDialog As New Windows.Forms.OpenFileDialog
                            With lFileDialog
                                .Title = "Please locate met WDM for SWAT"
                                .Filter = "*.wdm|*.wdm"
                                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                                    lWDMfilename = .FileName
                                    GoTo OpenMetWDM
                                End If
                            End With
                        Case MsgBoxResult.No
                            'Continue without opening WDM
                        Case MsgBoxResult.Cancel
                            Throw New ApplicationException("Cancel Opening Base Model")
                    End Select
                End If

OpenOutput:
                'Find and open output from base run, TODO: offer to run base case
                Dim lTxtInOutFolder As String = IO.Path.GetDirectoryName(aFilename) & g_PathChar & "Scenarios\base\TxtInOut" & g_PathChar ' trailing directory separator
                If Not FileExists(pBaseOutputHruFileName) Then
                    pBaseOutputHruFileName = lTxtInOutFolder & "output.hru"
                    If Not FileExists(pBaseOutputHruFileName) Then
                        'In BatchSWAT we name the base scenario folder the same as the project folder, so check there for output
                        pBaseOutputHruFileName = IO.Path.GetDirectoryName(aFilename) & g_PathChar & "Scenarios" & g_PathChar & IO.Path.GetFileName(IO.Path.GetDirectoryName(aFilename)) & g_PathChar & "TxtInOut\output.hru"
                    End If
                    If Not FileExists(pBaseOutputHruFileName) Then
                        Dim lScenarios As String() = IO.Directory.GetDirectories(IO.Path.GetDirectoryName(aFilename) & g_PathChar & "Scenarios" & g_PathChar)
                        If lScenarios.Length = 1 AndAlso Not lScenarios(0).Contains("Modified") AndAlso IO.File.Exists(lScenarios(0) & g_PathChar & "TxtInOut\output.hru") Then
                            pBaseOutputHruFileName = lScenarios(0) & g_PathChar & "TxtInOut\output.hru"
                        Else
                            Dim cdlg As New Windows.Forms.OpenFileDialog
                            With cdlg
                                .Title = "Please locate 'output.hru' from base SWAT run"
                                .FileName = IO.Path.GetDirectoryName(aFilename) & g_PathChar & "Scenarios\output.hru"
                                .Filter = "output.hru|output.hru"
                                .FilterIndex = 1
                                .DefaultExt = "hru"
                                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    pBaseOutputHruFileName = .FileName
                                    Logger.Dbg("User specified output.hru '" & pBaseOutputHruFileName & "'")
                                Else 'user clicked Cancel
                                    pBaseOutputHruFileName = ""
                                End If
                            End With
                        End If
                    End If
                End If
                If FileExists(pBaseOutputHruFileName) Then
                    lTxtInOutFolder = IO.Path.GetDirectoryName(pBaseOutputHruFileName) & g_PathChar
                    Dim lOutputHru As New atcTimeseriesSWAT.atcTimeseriesSWAT
                    Dim lOutputFields As New atcData.atcDataAttributes
                    With lOutputHru
                        lOutputFields.SetValue("FieldName", "AREAkm2;YLDt/ha")
                        If atcDataManager.OpenDataSource(lOutputHru, pBaseOutputHruFileName, lOutputFields) Then
                            Logger.Dbg("OutputHruTimserCount " & .DataSets.Count)
                        End If
                    End With

                    'open ascii Met data source
                    Dim lPcpFile As String = IO.Path.Combine(lTxtInOutFolder, "pcp1.pcp")
                    Dim lTmpFile As String = IO.Path.Combine(lTxtInOutFolder, "tmp1.tmp")
                    If IO.File.Exists(lPcpFile) AndAlso IO.File.Exists(lTmpFile) Then
                        pMetPcp = clsCat.OpenDataSource(lPcpFile)
                        pMetTmp = clsCat.OpenDataSource(lTmpFile)
                    Else
                        Logger.Dbg("Cannot find SWAT Base case Met data pcp1.pcp and tmp1.tmp in " & lTxtInOutFolder)
                    End If
                Else
                    If Logger.Msg("Run base model now?", vbYesNo, "SWAT output.hru not found") = MsgBoxResult.Yes Then
                        pBaseOutputHruFileName = Nothing
                        ModelRun("Base", Nothing, "", True, True, False)
                        GoTo OpenOutput
                    Else
                        Logger.Dbg("MissingHruOutput " & pBaseOutputHruFileName)
                    End If
                End If

                If Not FileExists(pBaseOutputRchFileName) Then
                    pBaseOutputRchFileName = lTxtInOutFolder & "output.rch"
                End If
                If FileExists(pBaseOutputRchFileName) Then
                    Dim lOutputRch As New atcTimeseriesSWAT.atcTimeseriesSWAT
                    With lOutputRch
                        If atcDataManager.OpenDataSource(lOutputRch, pBaseOutputRchFileName, Nothing) Then
                            Logger.Dbg("OutputRchTimserCount " & .DataSets.Count)
                        End If
                    End With
                Else
                    Logger.Dbg("MissingRchOutput " & pBaseOutputRchFileName)
                End If

                If Not FileExists(pBaseOutputSubFileName) Then
                    pBaseOutputSubFileName = lTxtInOutFolder & "output.sub"
                End If
                If FileExists(pBaseOutputSubFileName) Then
                    Dim lOutputSub As New atcTimeseriesSWAT.atcTimeseriesSWAT
                    With lOutputSub
                        If atcDataManager.OpenDataSource(lOutputSub, pBaseOutputSubFileName, Nothing) Then
                            Logger.Dbg("OutputSubTimserCount " & .DataSets.Count)
                        End If
                    End With
                Else
                    Logger.Dbg("MissingSubOutput " & pBaseOutputSubFileName)
                End If

            End If
        End If
ALREADYSET:
    End Sub

    Public Property SWATDatabasePath() As String
        Get
            If Not FileExists(pSWATDatabaseName) Then
                pSWATDatabaseName = FindFile("Please locate SWAT 2005 database", SWATProgramBase & "Databases\SWAT2005.mdb", , , True)
            End If
            Return pSWATDatabaseName
        End Get
        Set(ByVal newValue As String)
            pSWATDatabaseName = newValue
        End Set
    End Property

    Public Function ModelRun(ByVal aNewModelName As String, _
                                ByVal aModifiedData As atcData.atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcUtility.atcCollection _
                                                        Implements clsCatModel.ModelRun
        Dim lSaveDir As String = CurDir()
        Dim lProjectFolder As String = IO.Path.GetDirectoryName(pBaseModel)
        Dim lModelFolder As String = lProjectFolder & "\Scenarios" & g_PathChar & aNewModelName
        If aRunModel Then
            TryDelete(lModelFolder)
        End If
        Dim lTxtInOutFolder As String = lModelFolder & "\TxtInOut" & g_PathChar ' trailing directory separator
        Dim lSwatInput As New SwatObject.SwatInput(SWATDatabasePath, pBaseModel, lProjectFolder, aNewModelName)
        lSwatInput.SaveAllTextInput()
        Dim lFigFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(pBaseModel), "fig.fig")
        If IO.File.Exists(lFigFilename) Then
            If TryDelete(lTxtInOutFolder & "fig.fig") Then
                TryCopy(lFigFilename, lTxtInOutFolder & "fig.fig")
            End If
        End If

        'write met data
        Dim lCioItem As SwatObject.SwatInput.clsCIOItem = lSwatInput.CIO.Item
        If pMetWDM IsNot Nothing AndAlso pMetWDM.DataSets.Count > 0 Then
            modSwatMetData.WriteSwatMetInput(pMetWDM, aModifiedData, lProjectFolder, lTxtInOutFolder, _
                                             atcUtility.Jday(lCioItem.IYR, 1, 1, 0, 0, 0), _
                                             atcUtility.Jday(lCioItem.IYR + lCioItem.NBYR, 1, 1, 0, 0, 0))
        Else
            If pMetPcp.DataSets.Count > 0 Then modSwatMetData.WriteSwatMetInput(pMetPcp, aModifiedData, lTxtInOutFolder)
            If pMetTmp.DataSets.Count > 0 Then modSwatMetData.WriteSwatMetInput(pMetTmp, aModifiedData, lTxtInOutFolder)
        End If
        If aRunModel Then
            ChDir(lTxtInOutFolder)
            'Dim lSWATexeTargetPath As String = lTxtInOutFolder & "Swat2005.exe"
            'If Not IO.File.Exists(lSWATexeTargetPath) Then
            '    Dim lSWATexePath As String = IO.Path.Combine(SWATProgramBase, "Swat2005.exe")
            '    If Not IO.File.Exists(lSWATexePath) Then
            '        lSWATexePath = FindFile("Please Locate Swat2005.exe", lSWATexePath)
            '        If IO.File.Exists(lSWATexePath) Then
            '            SWATProgramBase = IO.Path.GetDirectoryName(lSWATexePath)
            '            IO.File.Copy(lSWATexePath, lSWATexeTargetPath)
            '        End If
            '    End If
            'End If
            'If IO.File.Exists(lSWATexeTargetPath) Then
            '    Logger.Dbg("StartModel")
            '    LaunchProgram(lSWATexeTargetPath, lTxtInOutFolder)
            '    Logger.Dbg("DoneModelRun")
            'Else
            '    Logger.Dbg("SWAT exe not found, skipping model run")
            'End If

            Dim lSWATExe As String = IO.Path.Combine(IO.Path.GetDirectoryName(pBaseModel), SwatExe())
            Dim lSWATexeTargetPath As String = String.Empty
            If IO.File.Exists(lSWATExe) Then
                If TryDelete(lTxtInOutFolder & SwatExe()) Then
                    TryCopy(lSWATExe, lTxtInOutFolder & SwatExe())
                End If
                lSWATexeTargetPath = IO.Path.Combine(lTxtInOutFolder, SwatExe())
                Logger.Dbg("StartModel")
                LaunchProgram(lSWATexeTargetPath, lTxtInOutFolder)
                Logger.Dbg("DoneModelRun")
            Else
                Logger.Dbg("SWAT exe not found, skipping model run")
            End If
        End If
        Dim lModified As New atcCollection

        ' Read written data for endpoints

        Dim lOutputHruFileName As String = lTxtInOutFolder & "output.hru"
        If FileExists(lOutputHruFileName) Then
            If pBaseOutputHruFileName Is Nothing Then 'running base
                pBaseOutputHruFileName = lOutputHruFileName
            Else
                lModified.Add(IO.Path.GetFileName(pBaseOutputHruFileName).ToLower.Trim, lOutputHruFileName.Trim)
            End If
        Else
            Logger.Dbg("MissingHruOutput " & lOutputHruFileName)
        End If

        Dim lOutputRchFileName As String = lTxtInOutFolder & "output.rch"
        If FileExists(lOutputRchFileName) Then
            If pBaseOutputRchFileName Is Nothing Then 'running base
                pBaseOutputRchFileName = lOutputRchFileName
            Else
                lModified.Add(IO.Path.GetFileName(pBaseOutputRchFileName).ToLower.Trim, lOutputRchFileName.Trim)
            End If
        Else
            Logger.Dbg("MissingRchOutput " & lOutputRchFileName)
        End If

        Dim lOutputSubFileName As String = lTxtInOutFolder & "output.sub"
        If FileExists(lOutputSubFileName) Then
            If pBaseOutputSubFileName Is Nothing Then 'running base
                pBaseOutputSubFileName = lOutputRchFileName
            Else
                lModified.Add(IO.Path.GetFileName(pBaseOutputSubFileName).ToLower.Trim, lOutputSubFileName.Trim)
            End If
        Else
            Logger.Dbg("MissingSubOutput " & lOutputSubFileName)
        End If

        Dim lPcpFileName As String = lTxtInOutFolder & "pcp1.pcp"
        If FileExists(lPcpFileName) Then
            If pMetPcp Is Nothing Then 'running base
                pMetPcp = clsCat.OpenDataSource(lPcpFileName)
            Else
                lModified.Add(IO.Path.GetFileName(pMetPcp.Specification).ToLower.Trim, lPcpFileName.Trim)
            End If
        Else
            Logger.Dbg("MissingPcp " & lPcpFileName)
        End If

        Dim lTmpFileName As String = lTxtInOutFolder & "tmp1.tmp"
        If FileExists(lTmpFileName) Then
            If pMetTmp Is Nothing Then 'running base
                pMetTmp = clsCat.OpenDataSource(lTmpFileName)
            Else
                lModified.Add(IO.Path.GetFileName(pMetTmp.Specification).ToLower.Trim, lTmpFileName.Trim)
            End If
        Else
            Logger.Dbg("MissingTmp " & lTmpFileName)
        End If


        ChDir(lSaveDir)
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<SWAT>" & vbCrLf
            lXML &= "  <FileName>" & pBaseModel & "</FileName>" & vbCrLf
            lXML &= "  <SWATDatabase>" & SWATDatabasePath & "</SWATDatabase>" & vbCrLf
            lXML &= "</SWAT>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
