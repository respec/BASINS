Imports atcUtility
Imports atcData
Imports atcBatchProcessing
Imports MapWinUtility

Public Class clsBatchSpecDFLOW
    Inherits clsBatch

    Public ListBatchOpnsMethods As atcCollection

    Public Sub New(ByVal aProgressbar As Windows.Forms.ProgressBar, ByVal aTextField As Windows.Forms.TextBox)
        gProgressBar = aProgressbar
        gTextStatus = aTextField
    End Sub

    Public Sub New()
    End Sub

    Public Overrides Sub PopulateScenarios()
        If IO.File.Exists(SpecFilename) Then
            If ListBatchOpns Is Nothing Then
                ListBatchOpns = New atcCollection()
            Else
                ListBatchOpns.Clear()
            End If
            If ListBatchOpnsMethods Is Nothing Then
                ListBatchOpnsMethods = New atcCollection()
            Else
                ListBatchOpnsMethods.Clear()
            End If
            If ListBatchUnitsData Is Nothing Then
                ListBatchUnitsData = New atcCollection()
            Else
                ListBatchUnitsData.Clear()
            End If
            If DownloadStations Is Nothing Then
                DownloadStations = New atcCollection()
            End If
            Dim lOneLine As String
            Dim lSR As New IO.StreamReader(SpecFilename)

            Dim lOpnCounter As Integer = 1
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Contains("***") Then Continue While 'bypass comments
                If lOneLine.Trim() = "" Then Continue While 'bypass blank lines

                If lOneLine.StartsWith("GLOBAL") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("METHOD") Then
                            Dim lMethodSetting As String = ""
                            While Not lSR.EndOfStream
                                lOneLine = lSR.ReadLine()
                                If lOneLine.Contains("***") Then Continue While
                                If lOneLine.Trim() = "" Then Continue While
                                If lOneLine.StartsWith("END METHOD") Then
                                    SpecifyMethod("GLOBAL", lMethodSetting)
                                    lOneLine = lSR.ReadLine()
                                    Exit While
                                End If
                                lMethodSetting &= lOneLine & ","
                            End While
                        End If
                        If lOneLine.StartsWith("END GLOBAL") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        SpecifyGlobal(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("DFLOW") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("METHOD") Then
                            Dim lMethodSetting As String = ""
                            While Not lSR.EndOfStream
                                lOneLine = lSR.ReadLine()
                                If lOneLine.Contains("***") Then Continue While
                                If lOneLine.Trim() = "" Then Continue While
                                If lOneLine.StartsWith("END METHOD") Then
                                    SpecifyMethod("GROUP" & lOpnCounter, lMethodSetting)
                                    lOneLine = lSR.ReadLine()
                                    Exit While
                                End If
                                lMethodSetting &= lOneLine & ","
                            End While
                        End If
                        If lOneLine.StartsWith("END DFLOW") Then
                            lReachedEnd = True
                            lOpnCounter += 1
                            Exit While
                        End If
                        SpecifyGroup(lOneLine, lOpnCounter)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("OTHER") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END OTHER") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        'SpecifyOther(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If
            End While
            lSR.Close()
            lSR = Nothing
        End If

        MergeSpecs()
    End Sub

    ''' <summary>
    ''' Merge global setting with individual station setting
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub MergeSpecs()
        For Each lBFOpn As atcCollection In ListBatchOpns
            For Each lStation As clsBatchUnitStation In lBFOpn
                If lStation.NeedToDownloadData Then
                    If Not DownloadStations.Keys.Contains(lStation.StationID) Then
                        DownloadStations.Add(lStation.StationID, lStation.StationID)
                    End If
                End If
                For Each lgDef As atcDefinedValue In GlobalSettings
                    With lStation.BFInputs
                        If .GetValue(lgDef.Definition.Name) Is Nothing Then
                            .SetValue(lgDef.Definition.Name, lgDef.Value)
                        End If
                    End With
                Next
            Next
        Next

        Dim lglobalMethods As atcCollection = GlobalSettings.GetValue(InputNamesDFLOW.Methods, Nothing)
        If lglobalMethods IsNot Nothing Then
            For Each lGroupKey As Integer In ListBatchOpns.Keys
                Dim lgroupMethods As atcCollection = ListBatchOpnsMethods.ItemByKey(lGroupKey)
                If lgroupMethods Is Nothing Then
                    lgroupMethods = New atcCollection()
                    ListBatchOpnsMethods.Add(lGroupKey, lgroupMethods)
                    For Each lMethodKey As String In lglobalMethods.Keys
                        Dim lglobalMethod As atcCollection = lglobalMethods.ItemByKey(lMethodKey)
                        lgroupMethods.Add(lMethodKey, lglobalMethod.Clone())
                    Next
                End If
            Next
        End If

        'Below is to download all stations data at once
        'Dim lContinueDataDownload As Boolean = True
        'clsBatchUtil.SiteInfoDir = GlobalSettings.GetValue("DataDir")
        'If Not IO.Directory.Exists(clsBatchUtil.SiteInfoDir) Then
        '    Dim lDirOpenDiag As New System.Windows.Forms.FolderBrowserDialog()
        '    lDirOpenDiag.Description = "Specify Directory To Save Downloaded Streamflow Data"
        '    If lDirOpenDiag.ShowDialog = Windows.Forms.DialogResult.OK Then
        '        clsBatchUtil.SiteInfoDir = lDirOpenDiag.SelectedPath
        '    Else
        '        Message = "Error: No data folder is specified for downloading data. Batch run stopped."
        '        UpdateStatus(Message, , True)
        '        lContinueDataDownload = False
        '    End If
        'End If
        'If lContinueDataDownload Then
        '    Try
        '        clsBatchUtil.DownloadData(DownloadStations)
        '    Catch ex As Exception

        '    End Try

        '    Dim lDownloadFailedStations As New atcCollection()
        '    For Each lBFOpn As atcCollection In ListBatchOpns
        '        For Each lStation As clsBatchUnitStation In lBFOpn
        '            If DownloadStations.Contains(lStation.StationID) Then
        '                For Each lStationId As String In DownloadStations
        '                    If lStation.StationID = lStationId Then
        '                        Dim lDataPath As String = IO.Path.Combine(clsBatchUtil.SiteInfoDir, "NWIS\NWIS_discharge_" & lStationId & ".rdb")
        '                        If IO.File.Exists(lDataPath) Then
        '                            lStation.StationDataFilename = lDataPath
        '                            lStation.NeedToDownloadData = False
        '                        Else
        '                            'Download failed for this station
        '                            lStation.StationDataFilename = ""
        '                            lStation.NeedToDownloadData = True
        '                            lDownloadFailedStations.Add(lStation.StationID, lStation.StationID)
        '                        End If
        '                    End If
        '                Next
        '            End If
        '        Next
        '    Next
        '    If lDownloadFailedStations.Count > 0 Then
        '        Message = "Error: Data Download failed for the following stations:" & vbCrLf
        '        For Each lStationID As String In lDownloadFailedStations
        '            Message &= lStationID & vbCrLf
        '        Next
        '        UpdateStatus(Message, , True)
        '    End If
        'End If
    End Sub

    Private Function GetMethod(ByVal aSpec As String) As atcCollection
        Dim lMethod As New atcCollection()
        Dim lSpecs() As String = aSpec.Split(",")
        Dim lMethodName As String = ""
        For Each lspec As String In lSpecs
            lspec = lspec.Trim()
            If Not String.IsNullOrEmpty(lspec) Then
                Dim lArr() As String = lspec.Split(vbTab)
                If lArr(0).ToLower().StartsWith("type_") Then
                    lMethodName = lArr(0)
                End If
                If Not lMethod.Keys.Contains(lArr(0)) Then
                    lMethod.Add(lArr(0), lArr(1))
                End If
            End If
        Next
        Return lMethod
    End Function

    Private Function AddGlobalMethod(ByVal aMethod As atcCollection) As String
        If GlobalSettings Is Nothing Then Return ""

        Dim lNewMethodKey As String = ""
        With GlobalSettings
            Dim lMethods As atcCollection = .GetValue(InputNamesDFLOW.Methods)
            If lMethods Is Nothing Then
                lMethods = New atcCollection()
                .SetValue(InputNamesDFLOW.Methods, lMethods)
            End If

            For Each lmKey As String In aMethod.Keys
                If lmKey.ToLower().StartsWith("type_") Then
                    lNewMethodKey = lmKey
                End If
            Next
            Dim lCount As Integer = 0
            For Each lmKey As String In lMethods.Keys
                If lmKey.StartsWith(lNewMethodKey) Then
                    lCount += 1
                End If
            Next
            lNewMethodKey &= "_" & lCount.ToString()
            lMethods.Add(lNewMethodKey, aMethod)
        End With
        Return lNewMethodKey
    End Function

    Private Function AddGroupMethod(ByVal aGroupId As Integer, ByVal aMethod As atcCollection) As String
        Dim lMethods As atcCollection = ListBatchOpnsMethods.ItemByKey(aGroupId)
        If lMethods Is Nothing Then
            lMethods = New atcCollection()
            ListBatchOpnsMethods.Add(aGroupId, lMethods)
        End If
        Dim lNewMethodKey As String = ""
        For Each lmKey As String In aMethod.Keys
            If lmKey.ToLower().StartsWith("type_") Then
                lNewMethodKey = lmKey
            End If
        Next
        Dim lCount As Integer = 0
        For Each lmKey As String In lMethods.Keys
            If lmKey.StartsWith(lNewMethodKey) Then
                lCount += 1
            End If
        Next
        lNewMethodKey &= "_" & lCount.ToString()
        lMethods.Add(lNewMethodKey, aMethod)
        Return lNewMethodKey
    End Function

    Private Sub SpecifyMethod(ByVal aTargetGroup As String, ByVal aSpec As String)
        Dim lMethod As atcCollection = GetMethod(aSpec)
        If aTargetGroup.StartsWith("GLOBAL") Then
            If GlobalSettings Is Nothing Then GlobalSettings = New atcDataAttributes()
            Dim lNewMethodName As String = AddGlobalMethod(lMethod)
        ElseIf aTargetGroup.StartsWith("GROUP") Then
            Dim lGroupIdStr As String = aTargetGroup.Substring("GROUP".Length)
            Dim lGroupId As Integer
            If Integer.TryParse(lGroupIdStr, lGroupId) Then
                AddGroupMethod(lGroupId, lMethod)
            End If
        End If
    End Sub
    ''' <summary>
    ''' This routine save a line of global setting
    ''' differentiate by first keyword on the line
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SpecifyGlobal(ByVal aSpec As String)
        If String.IsNullOrEmpty(aSpec) Then Exit Sub
        If aSpec.Contains("***") Then Exit Sub
        If GlobalSettings Is Nothing Then GlobalSettings = New atcDataAttributes()
        Dim lArr() As String = aSpec.Split(Delimiter)
        If String.IsNullOrEmpty(lArr(1).Trim()) Then Return
        Dim lAttribName As String = lArr(0).Trim()
        If GlobalSettings.GetValue(lAttribName) IsNot Nothing Then Return

        Select Case lAttribName.ToLower
            'Case atcTimeseriesBaseflow.BFInputNames.StartDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 0
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.StartDate, lDates)
            '    End If
            'Case atcTimeseriesBaseflow.BFInputNames.EndDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 24
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        GlobalSettings.Add(atcTimeseriesBaseflow.BFInputNames.EndDate, lDates)
            '    End If
            Case InputNamesDFLOW.Method.ToLower()
                Dim lMethods As atcCollection = GlobalSettings.GetValue(InputNamesDFLOW.Methods)
            'Dim lMethod As InputNames.ITAMethod = 0
            'Select Case lArr(1).ToUpper()
            '    Case InputNames.ITAMethod.FREQUECYGRID.ToString().ToUpper()
            '        lMethod = InputNames.ITAMethod.FREQUECYGRID
            '    Case InputNames.ITAMethod.FREQUENCYGRAPH.ToString().ToUpper()
            '        lMethod = InputNames.ITAMethod.FREQUENCYGRAPH
            '    Case InputNames.ITAMethod.NDAYTIMESERIES.ToString().ToUpper()
            '        lMethod = InputNames.ITAMethod.NDAYTIMESERIES
            '    Case InputNames.ITAMethod.TRENDLIST.ToString().ToUpper()
            '        lMethod = InputNames.ITAMethod.TRENDLIST
            'End Select
            'If lMethod > 0 Then
            '    If lMethods Is Nothing Then
            '        lMethods = New atcCollection()
            '        lMethods.Add(lMethod)
            '        GlobalSettings.Add(InputNames.Methods, lMethods)
            '    Else
            '        If Not lMethods.Contains(lMethod) Then
            '            lMethods.Add(lMethod)
            '        End If
            '    End If
            'End If
            Case InputNamesDFLOW.DataDir.ToLower
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso IO.Directory.Exists(lArr(1)) Then
                    DownloadDataDirectory = lArr(1)
                End If
                GlobalSettings.Add(InputNamesDFLOW.DataDir, lArr(1))
            'Case InputNames.UseCache.ToLower
            '    Dim lUseCache As Boolean = False
            '    If Not String.IsNullOrEmpty(lArr(1)) AndAlso lArr(1).ToLower = "yes" Then
            '        lUseCache = True
            '    End If
            '    GlobalSettings.Add(BatchInputNames.UseCache, lUseCache)
            Case Else
                If GlobalSettings.ContainsAttribute(lArr(0)) Then
                    GlobalSettings.SetValue(lArr(0), lArr(1))
                Else
                    GlobalSettings.Add(lArr(0), lArr(1))
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Create list of DFLOW Units
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SpecifyGroup(ByVal aSpec As String, ByVal aOpnCount As Integer)
        If String.IsNullOrEmpty(aSpec) Then Exit Sub
        If aSpec.Contains("***") Then Exit Sub

        Dim lArr() As String = aSpec.Split(Delimiter)

        If lArr.Length < 2 Then Return
        If String.IsNullOrEmpty(lArr(0)) Then Return

        Dim lListBatchUnits As atcCollection = ListBatchOpns.ItemByKey(aOpnCount)
        If lListBatchUnits Is Nothing Then
            lListBatchUnits = New atcCollection()
            ListBatchOpns.Add(aOpnCount, lListBatchUnits)
        End If

        Select Case lArr(0).ToLower
            Case "station"
                Dim lArrStation() As String = lArr(1).Split(",")
                Dim lStationId As String = lArrStation(0).Trim()
                Dim lStation As clsBatchUnitStation = lListBatchUnits.ItemByKey(lStationId)
                If lStation Is Nothing Then
                    lStation = New clsBatchUnitStation()
                    lStation.StationID = lStationId
                    lListBatchUnits.Add(lStationId, lStation)
                End If
                If lArrStation.Length >= 2 Then
                    Dim lStationDA As Double
                    If Double.TryParse(lArrStation(1).Trim(), lStationDA) AndAlso lStationDA > 0 Then
                        lStation.StationDrainageArea = lStationDA
                    End If
                End If
                If lArrStation.Length >= 3 Then
                    Dim lStationDatafilename As String = lArrStation(2).Trim()
                    If Not String.IsNullOrEmpty(lStationDatafilename) AndAlso IO.File.Exists(lStationDatafilename) Then
                        lStation.StationDataFilename = lStationDatafilename
                        lStation.NeedToDownloadData = False
                        If Not ListBatchUnitsData.Keys.Contains(lStationId) Then
                            ListBatchUnitsData.Add(lStationId, lStationDatafilename)
                        End If
                    End If
                End If
            'Case InputNamesDFLOW.StartDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 0
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        For Each lStation As clsBatchUnitStation In lListBatchUnits
            '            Dim lSDate() As Integer = lStation.BFInputs.GetValue(InputNames.StartDate)
            '            If lSDate Is Nothing Then
            '                lStation.BFInputs.Add(InputNames.StartDate, lDates)
            '            Else
            '                For I = 0 To 5
            '                    lSDate(I) = lDates(I)
            '                Next
            '            End If
            '        Next
            '    End If
            'Case InputNamesDFLOW.EndDate.ToLower
            '    Dim lDates(5) As Integer
            '    Dim lDate As DateTime
            '    If Date.TryParse(lArr(1), lDate) Then
            '        lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
            '        lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
            '        lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
            '        lDates(3) = 24
            '        lDates(4) = 0
            '        lDates(5) = 0
            '        For Each lStation As clsBatchUnitStation In lListBatchUnits
            '            Dim lEDate() As Integer = lStation.BFInputs.GetValue(InputNames.EndDate)
            '            If lEDate Is Nothing Then
            '                lStation.BFInputs.Add(InputNames.EndDate, lDates)
            '            Else
            '                For I = 0 To 5
            '                    lEDate(I) = lDates(I)
            '                Next
            '            End If
            '        Next
            '    End If
            Case InputNamesDFLOW.Method.ToLower
                'Dim lMethod As InputNames.ITAMethod = 0
                'Select Case lArr(1).Trim().ToUpper()
                '    Case InputNames.ITAMethod.FREQUECYGRID.ToString().ToUpper()
                '        lMethod = InputNames.ITAMethod.FREQUECYGRID
                '    Case InputNames.ITAMethod.FREQUENCYGRAPH.ToString().ToUpper()
                '        lMethod = InputNames.ITAMethod.FREQUENCYGRAPH
                '    Case InputNames.ITAMethod.NDAYTIMESERIES.ToString().ToUpper()
                '        lMethod = InputNames.ITAMethod.NDAYTIMESERIES
                '    Case InputNames.ITAMethod.TRENDLIST.ToString().ToUpper()
                '        lMethod = InputNames.ITAMethod.TRENDLIST
                'End Select
                'If lMethod > 0 Then
                '    For Each lStation As clsBatchUnitStation In lListBatchUnits
                '        Dim lMethods As atcCollection = lStation.BFInputs.GetValue(InputNames.Methods)
                '        If lMethods Is Nothing Then
                '            lMethods = New atcCollection()
                '            lMethods.Add(lMethod)
                '            lStation.BFInputs.Add(InputNames.Methods, lMethods)
                '        Else
                '            If Not lMethods.Contains(lMethod) Then
                '                lMethods.Add(lMethod)
                '            End If
                '        End If
                '    Next
                'End If
            Case InputNamesDFLOW.OutputDir.ToLower
                Dim lOutputDir As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputDir) AndAlso IO.Directory.Exists(lOutputDir) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(InputNamesDFLOW.OutputDir, lOutputDir)
                    Next
                End If
            Case InputNamesDFLOW.OutputPrefix.ToLower
                Dim lOutputPrefix As String = lArr(1).Trim()
                If Not String.IsNullOrEmpty(lOutputPrefix) Then
                    For Each lStation As clsBatchUnitStation In lListBatchUnits
                        lStation.BFInputs.SetValue(InputNamesDFLOW.OutputPrefix, lOutputPrefix)
                    Next
                End If
            Case Else
                For Each lStation As clsBatchUnitStation In lListBatchUnits
                    If lStation.BFInputs.ContainsAttribute(lArr(0)) Then
                        lStation.BFInputs.SetValue(lArr(0), lArr(1))
                    Else
                        lStation.BFInputs.Add(lArr(0), lArr(1))
                    End If
                Next
        End Select
    End Sub

    Private Function IsDirectoryWritable(ByVal aPath As String, Optional ByVal aCreate As Boolean = True) As Boolean
        If String.IsNullOrEmpty(aPath) Then Return False
        If Not IO.Directory.Exists(aPath) Then
            If aCreate Then
                Try
                    Dim lDirInfo As New IO.DirectoryInfo(aPath)
                    Dim ldSecurity As System.Security.AccessControl.DirectorySecurity = lDirInfo.GetAccessControl()
                    MkDirPath(aPath)
                Catch ex As Exception
                    'RaiseEvent StatusUpdate("0,0,Cannot create output directory: " & vbCrLf & lOutputDir)
                    UpdateStatus("Cannot create output directory: " & vbCrLf & aPath, , True)
                    Return False
                End Try
            Else
                Return False
            End If
        End If

        Dim lDirWritable As Boolean = True
        Try
            Dim lSW As IO.StreamWriter = Nothing
            Try
                lSW = New IO.StreamWriter(IO.Path.Combine(aPath, "z.txt"), False)
                lSW.WriteLine("1")
                lSW.Flush()
                lSW.Close()
                lSW = Nothing
                IO.File.Delete(IO.Path.Combine(aPath, "z.txt"))
            Catch ex As Exception
                If lSW IsNot Nothing Then
                    lSW.Close()
                    lSW = Nothing
                End If
                lDirWritable = False
            End Try
        Catch ex As Exception
            lDirWritable = False
        End Try
        Return lDirWritable
    End Function

    Public Overrides Sub DoBatch()
        If Not String.IsNullOrEmpty(Message) AndAlso Message.ToLower.StartsWith("error") Then
            Logger.Msg("Please address following issues before running batch:" & vbCrLf & Message,
                       "Batch Run DFLOW")
            Exit Sub
        Else
            Message = ""
        End If
        Dim lOutputDir As String = GlobalSettings.GetValue(InputNamesDFLOW.OutputDir, "")
        Dim lOutputDirWritable As Boolean = IsDirectoryWritable(lOutputDir)
        If Not lOutputDirWritable Then
            'RaiseEvent StatusUpdate("0,0,Can not write to output directory: " & vbCrLf & lOutputDir)
            'Windows.Forms.Application.DoEvents()
            UpdateStatus("Can not write to output directory: " & vbCrLf & lOutputDir, , True)
            Exit Sub
        End If

        Dim lTotalStations As Integer
        Dim lTotalBFOpn As Integer = ListBatchOpns.Count
        'For Each lKey As Integer In ListBatchOpns.Keys
        '    For Each lstation As clsBatchUnitStation In ListBatchOpns.ItemByKey(lKey)
        '        lTotalBFOpn += 1
        '    Next
        'Next
        gProgressBar.Minimum = 0
        gProgressBar.Maximum = lTotalBFOpn

        Dim lBFOpnCount As Integer = 0
        Dim lConfigFile As IO.StreamWriter = Nothing
        Dim lScenarios As New atcCollection()
        For Each lBFOpnId As Integer In ListBatchOpns.Keys
            Dim lBFOpn As atcCollection = ListBatchOpns.ItemByKey(lBFOpnId)
            Dim lBFOpnName As String = "DFLOW_Opn_" & lBFOpnId
            Dim lBFOpnDir As String = IO.Path.Combine(lOutputDir, lBFOpnName)
            MkDirPath(lBFOpnDir)
            'Retrieve parameters, at least one hydrologic group and one biological group
            Dim lParams As atcCollection = ListBatchOpnsMethods.ItemByKey(lBFOpnId)

            'Build a TserGroup to do ITFA
            Dim lDataGroup As New atcTimeseriesGroup()
            For Each lStation As clsBatchUnitStation In lBFOpn
                'Dim lOutputPrefix As String = lStation.BFInputs.GetValue(InputNames.OutputPrefix, "")
                'If String.IsNullOrEmpty(lOutputPrefix) Then lOutputPrefix = "ITFA_" & lStation.StationID
                Dim lTsFlow As atcTimeseries = GetStationStreamFlowData(lStation, ListBatchUnitsData)
                If lTsFlow IsNot Nothing Then
                    lDataGroup.Add(lTsFlow)
                Else
                    lStation.Message &= "Error: flow data is missing." & vbCrLf
                End If
            Next
            If lDataGroup.Count > 0 AndAlso lParams IsNot Nothing AndAlso lParams.Count >= 2 Then
                lTotalStations += lDataGroup.Count
                Dim lStation0 As clsBatchUnitStation = lBFOpn(0)
                Dim lInputArgs As atcDataAttributes = lStation0.BFInputs

                Dim lBioParamSet As New ArrayList()
                Dim lNBioParamSet As New ArrayList()
                For Each lParamSet As atcCollection In lParams
                    Dim lSetting As String = lParamSet.ItemByKey(InputNamesDFLOW.BiologicalDFLOW)
                    If lSetting.ToLower() = "yes" Then
                        lBioParamSet.Add(lParamSet)
                    Else
                        lNBioParamSet.Add(lParamSet)
                    End If
                Next
                lTotalBFOpn = lDataGroup.Count * lBioParamSet.Count * lNBioParamSet.Count
                gProgressBar.Minimum = lDataGroup.Count
                gProgressBar.Maximum = lTotalBFOpn
                gProgressBar.Step = lDataGroup.Count
                Dim lNBioName As String = ""
                Dim lBioName As String = ""
                'OutputFilenameRoot = lStation0.BFInputs.GetValue(InputNames.OutputPrefix, "DFLOW")
                Dim lSW As IO.StreamWriter = Nothing
                Try
                    lSW = New IO.StreamWriter(IO.Path.Combine(lBFOpnDir, "DFLOW_Report.txt"), False)
                    Dim lScenarioCount As Integer = 1
                    For Each lNBioParam As atcCollection In lNBioParamSet
                        lNBioName = InputNamesDFLOW.GetAbbrevParamSetName(lNBioParam)
                        For Each lBioParam As atcCollection In lBioParamSet
                            lBioName = InputNamesDFLOW.GetAbbrevParamSetName(lBioParam)
                            Try
                                Dim lScen As New clsInteractiveDFLOW()
                                With lScen
                                    .ScenarioID = lScenarioCount
                                    'remember Bio params
                                    .ParamBio1FlowAvgDays = lBioParam.ItemByKey(InputNamesDFLOW.BioAvgPeriod)
                                    .ParamBio2YearsBetweenExcursion = lBioParam.ItemByKey(InputNamesDFLOW.BioReturnYears)
                                    .ParamBio3ExcursionClusterDays = lBioParam.ItemByKey(InputNamesDFLOW.BioClusterDays)
                                    .ParamBio4ExcursionPerCluster = lBioParam.ItemByKey(InputNamesDFLOW.BioNumExcrsnPerCluster)
                                    For Each lKey As String In lBioParam.Keys
                                        If String.Compare(lKey, "type_" & InputNamesDFLOW.EBioDFlowType.Acute_maximum_concentration.ToString(), True) = 0 Then
                                            .TypeBio = clsInteractiveDFLOW.EDFLOWPARAM.BIOAcute
                                        ElseIf String.Compare(lKey, "type_" & InputNamesDFLOW.EBioDFlowType.Chronic_continuous_concentration.ToString(), True) = 0 Then
                                            .TypeBio = clsInteractiveDFLOW.EDFLOWPARAM.BIOChronic
                                        ElseIf String.Compare(lKey, "type_" & InputNamesDFLOW.EBioDFlowType.Ammonia.ToString(), True) = 0 Then
                                            .TypeBio = clsInteractiveDFLOW.EDFLOWPARAM.BIOAmmonia
                                        ElseIf String.Compare(lKey, "type_" & InputNamesDFLOW.EBioDFlowType.Custom.ToString(), True) = 0 Then
                                            .TypeBio = clsInteractiveDFLOW.EDFLOWPARAM.BIOCustom
                                        End If
                                    Next

                                    'remember Non-bio params
                                    For Each lKey As String In lNBioParam.Keys
                                        If String.Compare(lKey, "type_" & InputNamesDFLOW.EDFlowType.Hydrological.ToString(), True) = 0 Then
                                            .TypeNBio = clsInteractiveDFLOW.EDFLOWPARAM.NBIOAcute
                                            .ParamNBioNDay = lNBioParam.ItemByKey(InputNamesDFLOW.NBioAveragingPeriod)
                                            .ParamNBioReturn = lNBioParam.ItemByKey(InputNamesDFLOW.NBioReturnPeriod)
                                            Exit For
                                        ElseIf String.Compare(lKey, "type_" & InputNamesDFLOW.EDFlowType.Explicit_Flow_Value.ToString(), True) = 0 Then
                                            .TypeNBio = clsInteractiveDFLOW.EDFLOWPARAM.NBIOExplicitFlow
                                            .ParamNBioExpFlow = lNBioParam.ItemByKey(InputNamesDFLOW.NBioExplicitFlow)
                                            Exit For
                                        ElseIf String.Compare(lKey, "type_" & InputNamesDFLOW.EDFlowType.Flow_Percentile.ToString(), True) = 0 Then
                                            .TypeNBio = clsInteractiveDFLOW.EDFLOWPARAM.NBIOFlowPCT
                                            .ParamNBioFlowPct = lNBioParam.ItemByKey(InputNamesDFLOW.NBioFlowPercentile)
                                            Exit For
                                        ElseIf lKey.ToLower.Contains("type_" & InputNamesDFLOW.EDFlowType.Custom.ToString().ToLower()) Then
                                            .TypeNBio = clsInteractiveDFLOW.EDFLOWPARAM.NBIOCustom
                                            .ParamNBioNDay = lNBioParam.ItemByKey(InputNamesDFLOW.NBioAveragingPeriod)
                                            .ParamNBioReturn = lNBioParam.ItemByKey(InputNamesDFLOW.NBioReturnPeriod)
                                            Exit For
                                        End If
                                    Next

                                    With lInputArgs
                                        .SetValue("ExcursionCountArray", lScen.ExcursionCountArray)
                                        .SetValue("ExcursionsArray", lScen.ExcursionsArray)
                                        .SetValue("ClustersArray", lScen.ClustersArray)
                                    End With
                                    .DataGroup = lDataGroup
                                    .ReportSrc = DFLOWCalcs.DFLOWToGrid(lDataGroup, lBioParam, lNBioParam, lInputArgs, True)
                                    Dim lAugReportSrc As atcControls.atcGridSource = .AugmentReport(lNBioName & "-" & lBioName, lScenarioCount)
                                    lSW.WriteLine("DFLOW :: " & lBFOpnName & " :: NBio(" & lNBioName & ") by Bio(" & lBioName & ")")
                                    lSW.WriteLine(lAugReportSrc.ToString())
                                    lSW.Flush()
                                    lAugReportSrc.Rows = 0
                                    lAugReportSrc.Columns = 0
                                    lAugReportSrc = Nothing
                                    UpdateStatus(lBFOpnName & "-Done NBio(" & lNBioName & ") by Bio(" & lBioName & ")", True)
                                End With
                            Catch ex As Exception
                                UpdateStatus(lBFOpnName & "-Failed NBio(" & lNBioName & ") by Bio(" & lBioName & ")", True)
                            End Try
                        Next 'lBioParam
                    Next 'lNBioParam
                Catch ex As Exception

                Finally
                    If lSW IsNot Nothing Then
                        lSW.Flush()
                        lSW.Close()
                        lSW = Nothing
                    End If
                End Try
                lConfigFile = New IO.StreamWriter(IO.Path.Combine(lBFOpnDir, "Config.txt"), False)
                For Each lStation As clsBatchUnitStation In lBFOpn
                    Dim lDataFilename As String = ListBatchUnitsData.ItemByKey(lStation.StationID)
                    If String.IsNullOrEmpty(lDataFilename) Then
                        lDataFilename = lStation.StationDataFilename
                        If IO.File.Exists(lDataFilename) Then
                            ListBatchUnitsData.Add(lStation.StationID, lDataFilename)
                        End If
                    End If
                    lConfigFile.WriteLine("Station " & lStation.StationID & ", " & lStation.StationDrainageArea & ", " & lDataFilename)
                Next
                For Each lAttrib As atcDefinedValue In CType(lBFOpn.ItemByIndex(0), clsBatchUnitStation).BFInputs
                    Dim lName As String = lAttrib.Definition.Name
                    Select Case lName.ToLower()
                        Case "startdate", "enddate"
                            Dim lDates(5) As Integer
                            atcUtility.J2Date(lAttrib.Value, lDates)
                            If lName.ToLower() = "enddate" Then
                                timcnv(lDates)
                            End If
                            lConfigFile.WriteLine(lName & ", " & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
                        Case Else
                            lConfigFile.WriteLine(lName & ", " & lAttrib.Value.ToString())
                    End Select
                Next
                lConfigFile.Flush()
                lConfigFile.Close()
                lConfigFile = Nothing

                If DFLOWCalcs.DFLOWMessage IsNot Nothing AndAlso DFLOWCalcs.DFLOWMessage.Length > 0 Then
                    Dim log As New IO.StreamWriter(IO.Path.Combine(lBFOpnDir, "DFLOW_Log_" & SafeFilename(DateTime.Now()) & ".txt"), False)
                    log.WriteLine(DFLOWCalcs.DFLOWMessage.ToString())
                    log.Flush() : log.Close() : log = Nothing
                    DFLOWCalcs.DFLOWMessage.Length = 0
                End If
            End If

            'RaiseEvent StatusUpdate(lBFOpnCount & "," & lTotalBFOpn & "," & "Base-flow Separation for station: " & lStation.StationID & " (" & lBFOpnCount & " out of " & lTotalBFOpn & ")")
            lBFOpnCount = lTotalBFOpn / lDataGroup.Count
            UpdateStatus("DFLOW Batch Run Group " & lBFOpnId & " (" & lBFOpnCount & " out of " & lBFOpnCount & " analysis on " & lDataGroup.Count & " stations)", True, True)
        Next

        'Dim lSummary As New IO.StreamWriter(IO.Path.Combine(lOutputDir, "DFLOW_Log_" & SafeFilename(DateTime.Now()) & ".txt"), False)
        'For Each lBFOpnId As Integer In ListBatchOpns.Keys
        '    Dim lBFOpn As atcCollection = ListBatchOpns.ItemByKey(lBFOpnId)
        '    lSummary.WriteLine("Batch Run Group ***  " & lBFOpnId & "  ***")
        '    Dim lHasError As Boolean = False
        '    For Each lStation As clsBatchUnitStation In lBFOpn
        '        If Not String.IsNullOrEmpty(lStation.Message) Then
        '            lSummary.WriteLine("---- Station: " & lStation.StationID & "----")
        '            lSummary.WriteLine(lStation.Message)
        '            lSummary.WriteLine("---------------------------------------------")
        '            If lStation.Message.ToLower().Contains("error") Then lHasError = True
        '        End If
        '    Next
        '    If lHasError Then
        '        lSummary.WriteLine("End Batch Run Group " & lBFOpnId & ", Has errors.")
        '    Else
        '        lSummary.WriteLine("End Batch Run Group " & lBFOpnId & ", Successful")
        '    End If
        'Next
        'lSummary.Flush()
        'lSummary.Close()
        'lSummary = Nothing
        UpdateStatus("Batch Run Complete for " & lTotalStations & " Stations in " & ListBatchOpns.Count & " groups.", True)
    End Sub
End Class
