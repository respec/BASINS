Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWMM
    Implements clsCatModel

    Public Event BaseModelSet(ByVal aBaseModel As String) Implements clsCatModel.BaseModelSet

    Private pBaseModel As String = ""
    Private pBaseProject As atcSWMM.atcSWMMProject = Nothing

    Public Property BaseModel() As String Implements clsCatModel.BaseModel
        Get
            Return pBaseModel
        End Get
        Set(ByVal newValue As String)
            OpenBaseModel(newValue)
        End Set
    End Property

    ''' <summary>
    ''' Open data files referred to in this INP file
    ''' </summary>
    ''' <param name="aFilename">Full path of INP file</param>
    ''' <remarks></remarks>
    Friend Sub OpenBaseModel(Optional ByVal aFilename As String = "")
        If Not aFilename Is Nothing And Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".inp") Then aFilename &= ".inp"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open INP file containing base model"
            cdlg.Filter = "INP files|*.inp|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If IO.File.Exists(aFilename) Then
            Dim lInpFolder As String = PathNameOnly(aFilename)
            ChDriveDir(lInpFolder)
            pBaseModel = aFilename
            RaiseEvent BaseModelSet(aFilename)
            pBaseProject = clsCat.OpenDataSource(aFilename)

            'Open the output file
            'Dim lBinOutFilename As String = IO.Path.Combine(IO.Path.GetFileNameWithoutExtension(aFilename), ".out")
            Dim lBinOutFilename As String = IO.Path.ChangeExtension(aFilename, ".out")
            clsCat.OpenDataSource(lBinOutFilename)
        End If
    End Sub

    Private Sub CreateModifiedINP(ByVal aNewModelName As String, ByVal aNewInpFilename As String, ByVal aModifiedData As atcData.atcTimeseriesGroup)
        Dim lPrevCurDir As String = CurDir()
        ChDriveDir(IO.Path.GetDirectoryName(aNewInpFilename))
        pBaseProject.Save(aNewInpFilename, atcDataSource.EnumExistAction.ExistReplace)
        pBaseProject.Specification = pBaseModel ' save changed specification for creating template, then switch back

        Dim lModifiedProject As New atcSWMM.atcSWMMProject
        Dim lModifiedRaingages As New Generic.List(Of atcSWMM.atcSWMMRainGage)
        Dim lModifiedTemperature As atcSWMM.atcSWMMTemperature = Nothing
        Dim lModifiedEvaporation As atcSWMM.atcSWMMEvaporation = Nothing

        lModifiedProject.Open(aNewInpFilename)

        For Each lTS As atcData.atcTimeseries In aModifiedData
            Dim lID As Integer = lTS.Attributes.GetValue("ID")
            Dim lOriginalTS As atcData.atcTimeseries = lModifiedProject.DataSets.ItemByKey(lID)
            If lOriginalTS IsNot Nothing Then
                lModifiedProject.DataSets.Remove(lOriginalTS)
                lModifiedProject.DataSets.Add(lID, lTS)
            End If
            If lModifiedProject.Temperature.Timeseries IsNot Nothing AndAlso _
               lModifiedProject.Temperature.Timeseries.Attributes.GetValue("ID") = lID Then
                lModifiedTemperature = lModifiedProject.Temperature
                lModifiedProject.Temperature.Timeseries = lTS
            ElseIf lModifiedProject.Evaporation.Timeseries IsNot Nothing AndAlso _
                   lModifiedProject.Evaporation.Timeseries.Attributes.GetValue("ID") = lID Then
                lModifiedEvaporation = lModifiedProject.Evaporation
                lModifiedProject.Evaporation.Timeseries = lTS
            Else
                For Each lRaingage As atcSWMM.atcSWMMRainGage In lModifiedProject.RainGages
                    If lRaingage.TimeSeries IsNot Nothing AndAlso _
                       lRaingage.TimeSeries.Attributes.GetValue("ID") = lID Then
                        lModifiedRaingages.Add(lRaingage)
                        lRaingage.TimeSeries = lTS
                        Exit For
                    End If
                Next
            End If
        Next

        'Save the project now that it contains all the modified data
        lModifiedProject.Save(aNewInpFilename, atcDataSource.EnumExistAction.ExistReplace)

        'Remove modified data before it is cleared by lModifiedProject being destroyed
        For Each lTS As atcData.atcTimeseries In aModifiedData
            lModifiedProject.DataSets.Remove(lTS)
        Next

        If lModifiedTemperature IsNot Nothing Then lModifiedTemperature.Timeseries = Nothing

        If lModifiedEvaporation IsNot Nothing Then lModifiedEvaporation.Timeseries = Nothing

        For Each lRaingage As atcSWMM.atcSWMMRainGage In lModifiedProject.RainGages
            lRaingage.TimeSeries = Nothing
        Next

        atcDataManager.RemoveDataSource(lModifiedProject)
        lModifiedProject = Nothing
        ChDriveDir(lPrevCurDir)
    End Sub

    ''' <summary>
    ''' Copy base INP and change model name within it
    ''' Change data to be modified in new INP
    ''' Run SWMM5 with the new INP
    ''' </summary>
    ''' <param name="aNewModelName"></param>
    ''' <param name="aModifiedData"></param>
    ''' <param name="aPreparedInput"></param>
    ''' <param name="aRunModel"></param>
    ''' <param name="aShowProgress"></param>
    ''' <param name="aKeepRunning"></param>
    ''' <returns>atcCollection of atcDataSource</returns>
    ''' <remarks></remarks>
    Public Function ModelRun(ByVal aNewModelName As String, _
                                ByVal aModifiedData As atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcCollection Implements clsCatModel.ModelRun

        Dim lModified As New atcCollection
        'Dim lCurrentTimeseries As atcTimeseries

        If aModifiedData Is Nothing Then
            aModifiedData = New atcTimeseriesGroup
        End If

        If IO.File.Exists(pBaseModel) Then
            Dim lNewBaseFilename As String = AbsolutePath(pBaseModel, CurDir)
            Dim lNewFolder As String = PathNameOnly(lNewBaseFilename) & g_PathChar
            lNewBaseFilename = lNewFolder & aNewModelName & "."
            Dim lNewINPfilename As String = ""

            'Get all external climate timeseries files, assuming they are named as *.DAT
            Select Case aNewModelName.ToLower
                Case "base"
                    lNewINPfilename = AbsolutePath(pBaseModel, CurDir)
                    lModified.Add(IO.Path.GetFileName(lNewINPfilename).ToLower.Trim, lNewINPfilename.Trim)
                Case "modifyoriginal"
                    lNewINPfilename = AbsolutePath(pBaseModel, CurDir)
                    lModified.Add(IO.Path.GetFileName(lNewINPfilename).ToLower.Trim, lNewINPfilename.Trim)

                    'TODO: needs work here to make sense: first add into, then clear???
                    'Dim lBinOutResults As atcTimeseriesSWMM5Output.atcDataSourceTimeseriesSWMM5Output = atcData.atcDataManager.DataSources(0) 'TODO: is it a good assumption that (0) is the results?

                    'For Each lDATfilename As String In lDATFilenames
                    '    lDATfilename = AbsolutePath(lDATfilename, CurDir).Trim()
                    '    For Each lCurrentTimeseries In aModifiedData
                    '        If Not lCurrentTimeseries Is Nothing _
                    '           AndAlso lCurrentTimeseries.Attributes.GetValue("History 1").ToString.ToLower.Equals("read from " & lDATfilename.ToLower) Then
                    '            lBinOutResults.AddDataSet(lCurrentTimeseries)
                    '            'Dim lmodifiedData As String = GetModifiedData(lType, aModifiedData)
                    '        End If
                    '    Next
                    '    lBinOutResults.DataSets.Clear()
                    'Next
                Case Else
                    lNewFolder = PathNameOnly(lNewBaseFilename) & g_PathChar & aNewModelName & g_PathChar
                    IO.Directory.CreateDirectory(lNewFolder)
                    lNewBaseFilename = lNewFolder & IO.Path.GetFileNameWithoutExtension(pBaseModel) & "."
                    lNewINPfilename = lNewBaseFilename & "inp"
                    'Copy base INP, changing base to new model name within it

                    CreateModifiedINP(aNewModelName, lNewINPfilename, aModifiedData)
                    lModified.Add(IO.Path.GetFileName(pBaseModel).ToLower.Trim, lNewINPfilename.Trim)
            End Select

            Dim lRunExitCode As Integer = 0
            If aRunModel Then
                'Run Model
                Dim lSWMMExeName As String = FindFile("Please locate swmm5.exe", g_PathChar & "Program Files\EPA SWMM 5.0\swmm5.exe")
                Dim lPrevCurDir As String = CurDir() 'save the base model folder
                ChDriveDir(IO.Path.GetDirectoryName(lNewINPfilename)) 'change curdir to modified model folder
                AppendFileString(lNewFolder & "SWMM5Error.Log", "Start log for " & lNewBaseFilename & vbCrLf)
                Dim lArgs As String = lNewINPfilename
                lArgs &= " " & IO.Path.ChangeExtension(lNewINPfilename, "rpt")
                lArgs &= " " & IO.Path.ChangeExtension(lNewINPfilename, "out")
                Logger.Dbg("Start " & lSWMMExeName & " with Arguments '" & lArgs & "'")
                Dim lSWMMProcess As New Diagnostics.Process
                With lSWMMProcess.StartInfo
                    .FileName = lSWMMExeName
                    .Arguments = lArgs
                    .CreateNoWindow = False
                    .UseShellExecute = True
                End With
                lSWMMProcess.Start()
                While Not lSWMMProcess.HasExited
                    If Not g_Running And Not aKeepRunning Then
                        lSWMMProcess.Kill()
                    End If
                    Windows.Forms.Application.DoEvents()
                    Threading.Thread.Sleep(50)
                End While
                lRunExitCode = lSWMMProcess.ExitCode
                Logger.Dbg("Model exit code " & lRunExitCode)
                If lRunExitCode <> 0 Then
                    Logger.Dbg("****************** Problem *********************")
                End If
                ChDriveDir(lPrevCurDir) 'Change curdir back to base model folder
            Else
                Logger.Dbg("Skipping running model")
            End If

            If g_Running Then
                If lRunExitCode <> 0 Then 'SWMM run failed, don't send any timeseries back to cat
                    lModified.Clear()
                Else
                    Dim lBinOutFilename As String = IO.Path.ChangeExtension(lNewINPfilename, "out")

                    If IO.File.Exists(lBinOutFilename) Then
                        'Dim lHBNResults As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                        'If lHBNResults.Open(lNewFilename) Then
                        lModified.Add(IO.Path.ChangeExtension(IO.Path.GetFileName(pBaseModel), "out").ToLower.Trim, lBinOutFilename.Trim)
                        'Else
                        '    Logger.Dbg("Could not open output file '" & lBinOutFilename & "'")
                        'End If
                    Else
                        Logger.Dbg("Could not find binary .OUT file '" & lBinOutFilename & "'")
                    End If
                End If
            End If
        Else
            Logger.Msg("Could not find base INP file '" & pBaseModel & "'" & vbCrLf & "Could not run model", "Model Run")
        End If
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<INP>" & vbCrLf
            lXML &= "  <FileName>" & pBaseModel & "</FileName>" & vbCrLf
            lXML &= "</INP>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
