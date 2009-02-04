Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWAT
    Implements clsCatModel

    Public SWATProgramBase As String = "C:\Program Files\SWAT 2005 Editor\"

    Public Event BaseScenarioSet(ByVal aBaseScenario As String) Implements clsCatModel.BaseScenarioSet

    Private pBaseScenario As String = ""
    Private pSWATDatabaseName As String = ""
    Private pMetWDM As atcWDM.atcDataSourceWDM
    Private pBaseOutputHruFileName As String
    Private pBaseOutputRchFileName As String
    Private pBaseOutputSubFileName As String

    Public Property BaseScenario() As String Implements clsCatModel.BaseScenario
        Get
            Return pBaseScenario
        End Get
        Set(ByVal newValue As String)
            OpenBaseScenario(newValue)
        End Set
    End Property

    Friend Sub OpenBaseScenario(Optional ByVal aFilename As String = "")
        If Not aFilename Is Nothing AndAlso Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".mdb") Then aFilename &= ".mdb"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open SWAT file containing base scenario"
            cdlg.Filter = "SWAT mdb files|*.mdb|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If aFilename.ToLower <> pBaseScenario.ToLower AndAlso IO.File.Exists(aFilename) Then
            Dim lFolder As String = PathNameOnly(aFilename)
            ChDriveDir(lFolder)
            pBaseScenario = aFilename
            RaiseEvent BaseScenarioSet(aFilename)

            'Find and open met data WDM file
            Dim lWDMfilename As String = IO.Path.GetDirectoryName(aFilename) & "\met.wdm"
            If Not IO.File.Exists(lWDMfilename) Then
                lWDMfilename = IO.Path.GetDirectoryName(aFilename) & "\met\met.wdm"
            End If
OpenMetWDM:
            If IO.File.Exists(lWDMfilename) Then
                pMetWDM = clsCat.OpenDataSource(lWDMfilename)
            Else
                If Logger.Msg("Did not find '" & lWDMfilename & "'" & vbCrLf & "Browse for met data?", vbOKCancel, "SWAT Met Data Not Found") = MsgBoxResult.Ok Then
                    Dim lFileDialog As New Windows.Forms.OpenFileDialog
                    With lFileDialog
                        .Title = "Please locate met WDM for SWAT"
                        .Filter = "*.wdm|*.wdm"
                        If .ShowDialog = Windows.Forms.DialogResult.OK Then
                            lWDMfilename = .FileName
                            GoTo OpenMetWDM
                        End If
                    End With
                End If
            End If

            If pMetWDM IsNot Nothing Then
                'Find and open output from base run, TODO: offer to browse for base or run base case
                Dim lTxtInOutFolder As String = IO.Path.GetDirectoryName(aFilename) & "\Scenarios\base\TxtInOut\" ' trailing directory separator
                pBaseOutputHruFileName = lTxtInOutFolder & "output.hru"
                If FileExists(pBaseOutputHruFileName) Then
                    Dim lOutputHru As New atcTimeseriesSWAT.atcTimeseriesSWAT
                    Dim lOutputFields As New atcData.atcDataAttributes
                    With lOutputHru
                        lOutputFields.SetValue("FieldName", "AREAkm2;YLDt/ha")
                        If atcDataManager.OpenDataSource(lOutputHru, pBaseOutputHruFileName, lOutputFields) Then
                            Logger.Dbg("OutputHruTimserCount " & .DataSets.Count)
                        End If
                    End With
                Else
                    Logger.Dbg("MissingHruOutput " & pBaseOutputHruFileName)
                End If

                pBaseOutputRchFileName = lTxtInOutFolder & "output.rch"
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

                pBaseOutputSubFileName = lTxtInOutFolder & "output.sub"
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
    End Sub

    Public Property SWATDatabasePath() As String
        Get
            If Not FileExists(pSWATDatabaseName) Then
                pSWATDatabaseName = FindFile("Please locate SWAT 2005 database", SWATProgramBase & "Databases\SWAT2005.mdb")
            End If
            Return pSWATDatabaseName
        End Get
        Set(ByVal newValue As String)
            pSWATDatabaseName = newValue
        End Set
    End Property

    Public Function ScenarioRun(ByVal aNewScenarioName As String, _
                                ByVal aModifiedData As atcData.atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcUtility.atcCollection _
                                                        Implements clsCatModel.ScenarioRun
        Dim lSaveDir As String = CurDir()
        Dim lProjectFolder As String = IO.Path.GetTempPath & aNewScenarioName
        Dim lTxtInOutFolder As String = lProjectFolder & "\Scenarios\" & aNewScenarioName & "\TxtInOut\" ' trailing directory separator
        Dim lSwatInput As New SwatObject.SwatInput(SWATDatabasePath, pBaseScenario, lProjectFolder, aNewScenarioName)
        lSwatInput.SaveAllTextInput()
        Dim lFigFilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(pBaseScenario), "fig.fig")
        If IO.File.Exists(lFigFilename) Then
            If TryDelete(lTxtInOutFolder & "fig.fig") Then
                TryCopy(lFigFilename, lTxtInOutFolder & "fig.fig")
            End If
        End If
        'write met data
        Dim lCioItem As SwatObject.SwatInput.clsCIOItem = lSwatInput.CIO.Item
        modSwatMetData.WriteSwatMetInput(pMetWDM, aModifiedData, lProjectFolder, lTxtInOutFolder, _
                                         atcUtility.Jday(lCioItem.IYR, 1, 1, 0, 0, 0), _
                                         atcUtility.Jday(lCioItem.IYR + lCioItem.NBYR, 1, 1, 0, 0, 0))
        If aRunModel Then
            ChDir(lTxtInOutFolder)
            Dim lSWATexeTargetPath As String = lTxtInOutFolder & "Swat2005.exe"
            If Not IO.File.Exists(lSWATexeTargetPath) Then
                Dim lSWATexePath As String = IO.Path.Combine(SWATProgramBase, "Swat2005.exe")
                If Not IO.File.Exists(lSWATexePath) Then
                    lSWATexePath = FindFile("Please Locate Swat2005.exe", lSWATexePath)
                    If IO.File.Exists(lSWATexePath) Then
                        SWATProgramBase = IO.Path.GetDirectoryName(lSWATexePath)
                        IO.File.Copy(lSWATexePath, lSWATexeTargetPath)
                    End If
                End If
            End If
            If IO.File.Exists(lSWATexeTargetPath) Then
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
            lModified.Add(IO.Path.GetFileName(pBaseOutputHruFileName).ToLower.Trim, lOutputHruFileName.Trim)
        Else
            Logger.Dbg("MissingHruOutput " & lOutputHruFileName)
        End If

        Dim lOutputRchFileName As String = lTxtInOutFolder & "output.rch"
        If FileExists(lOutputRchFileName) Then
            lModified.Add(IO.Path.GetFileName(pBaseOutputRchFileName).ToLower.Trim, lOutputRchFileName.Trim)
        Else
            Logger.Dbg("MissingRchOutput " & lOutputRchFileName)
        End If

        Dim lOutputSubFileName As String = lTxtInOutFolder & "output.sub"
        If FileExists(lOutputSubFileName) Then
            lModified.Add(IO.Path.GetFileName(pBaseOutputSubFileName).ToLower.Trim, lOutputSubFileName.Trim)
        Else
            Logger.Dbg("MissingSubOutput " & lOutputSubFileName)
        End If

        ChDir(lSaveDir)
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<SWAT>" & vbCrLf
            lXML &= "  <FileName>" & pBaseScenario & "</FileName>" & vbCrLf
            lXML &= "</SWAT>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
