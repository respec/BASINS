Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized

Imports MapWinUtility
Imports atcData
Imports atcUtility

Public Class atcSWMMProject
    Inherits atcData.atcTimeseriesSource

    Private pIsMetric As Boolean
    Private pDatasetId As Integer
    Public Blocks As atcSWMMBlocks

    Public Options As atcSWMMOptions
    Public Catchments As atcSWMMCatchments
    Public Conduits As atcSWMMConduits
    Public Losses As atcSWMMBlock 'TODO: create class
    Public Report As atcSWMMBlock 'TODO: create class
    Public Tags As atcSWMMBlock 'TODO: create class
    Public Map As atcSWMMBlock 'TODO: create class
    Public Nodes As atcSWMMNodes

    Public RainGages As atcSWMMRainGages
    Public Evaporation As atcSWMMEvaporation
    Public Temperature As atcSWMMTemperature
    'Public Pollutants As atcSWMMPollutants
    'Public Pumps As atcSWMMPumps
    'Public Controls As atcSWMMControls

    Public Title As atcSWMMBlock
    Public Timeseries As atcSWMMBlock
    Public Coordinates As atcSWMMBlock
    Public Vertices As atcSWMMBlock
    Public Polygons As atcSWMMBlock
    Public Symbols As atcSWMMBlock
    Public Backdrop As atcSWMMBlock

    Public BackdropFile As String = ""
    Public BackdropX1 As Double = 0.0
    Public BackdropY1 As Double = 0.0
    Public BackdropX2 As Double = 0.0
    Public BackdropY2 As Double = 0.0
    Public MapUnits As String = "METERS"

    Public IsFileData As Boolean = False

    Public Sub New()
        Me.Clear()
    End Sub

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SWMM Input"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::SWMM Input"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Sub Clear()
        MyBase.Clear()
        Blocks = New atcSWMMBlocks
        pDatasetId = 0
        With Blocks

            Title = New atcSWMMBlock("[TITLE]", "")
            .Add(Title)

            Options = New atcSWMMOptions
            .Add(Options)

            Evaporation = New atcSWMMEvaporation(Me)
            .Add(Evaporation)

            Temperature = New atcSWMMTemperature(Me)
            .Add(Temperature)

            RainGages = New atcSWMMRainGages(Me)
            .Add(RainGages)

            Catchments = New atcSWMMCatchments(Me)
            .Add(Catchments)

            Nodes = New atcSWMMNodes
            .Add(Nodes)

            Conduits = New atcSWMMConduits(Me)
            .Add(Conduits)

            Losses = New atcSWMMBlock("[LOSSES]", _
                                      ";;Link           Inlet      Outlet     Average    Flap Gate " & vbCrLf _
                                    & ";;-------------- ---------- ---------- ---------- ----------")
            .Add(Losses)

            Landuses = New atcSWMMLanduses(Me)

            Timeseries = New atcSWMMBlock("[TIMESERIES]", "")
            .Add(Timeseries)

            Report = New atcSWMMBlock("[REPORT]", _
                                      "INPUT      NO" & vbCrLf _
                                    & "CONTROLS   NO")
            .Add(Report)

            Tags = New atcSWMMBlock("[TAGS]", "")
            .Add(Tags)

            Map = New atcSWMMBlock("[MAP]", _
                                   "UNITS      " & MapUnits)
            .Add(Map)
            
            Coordinates = New atcSWMMBlock("[COORDINATES]", "")
            .Add(Coordinates)

            Vertices = New atcSWMMBlock("[VERTICES]", "")
            .Add(Vertices)

            Polygons = New atcSWMMBlock("[Polygons]", "")
            .Add(Polygons)

            Symbols = New atcSWMMBlock("[SYMBOLS]", "")
            .Add(Symbols)

            Backdrop = New atcSWMMBlock("[BACKDROP]", "")
            BackdropFile = ""
            .Add(Backdrop)
        End With

    End Sub

    Public Property DatasetId(Optional ByVal aIncrement As Boolean = False) As Integer
        Get
            If aIncrement Then
                pDatasetId += 1
                Return pDatasetId
            Else
                Return pDatasetId
            End If
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 1
            pDatasetId = value
        End Set
    End Property

    Public Property Landuses() As atcSWMMLanduses
        Get
            Return Blocks("[LANDUSES]")
        End Get
        Set(ByVal value As atcSWMMLanduses)
            Dim lLanduseIndex As Integer
            If Blocks.Contains("[LANDUSES]") Then
                Dim lLanduses As atcSWMMLanduses = Blocks("[LANDUSES]")
                lLanduseIndex = Blocks.IndexOf(lLanduses)
                Blocks.RemoveAt(lLanduseIndex)
            Else
                lLanduseIndex = Blocks.Count
            End If
            Blocks.Insert(lLanduseIndex, value)
        End Set
    End Property

    Public Property IsMetric() As Boolean
        Get
            Return pIsMetric
        End Get
        Set(ByVal aIsMetric As Boolean)
            pIsMetric = aIsMetric
            atcSWMMConduit.IsMetric = aIsMetric
        End Set
    End Property

    Public Function RunSimulation() As Boolean
        Dim lRunSimulation As Boolean = False
        Dim lSWMMExeName As String = GetSetting("BASINS41", "SWMM", "SimulationExeFileName", "")
        If Not IO.File.Exists(lSWMMExeName) Then
            lSWMMExeName = FindFile("Please locate SWMM5 Exe File", "swmm5.exe", "exe")
            If IO.File.Exists(lSWMMExeName) Then
                SaveSetting("BASINS41", "SWMM", "SimulationExeFileName", "")
            End If
        End If
        If IO.File.Exists(lSWMMExeName) Then
            Dim lProcess As New System.Diagnostics.Process
            With lProcess
                With .StartInfo
                    .FileName = lSWMMExeName
                    .WorkingDirectory = IO.Path.GetDirectoryName(Specification)
                    Dim lFileNameNoExt As String = IO.Path.GetFileNameWithoutExtension(Specification)
                    .Arguments = lFileNameNoExt & ".inp " & lFileNameNoExt & ".rpt " & lFileNameNoExt & ".out"
                    .CreateNoWindow = True
                    .UseShellExecute = False
                    .RedirectStandardInput = True
                    .RedirectStandardOutput = True
                    AddHandler lProcess.OutputDataReceived, AddressOf ProcessMessageHandler
                    .RedirectStandardError = True
                    AddHandler lProcess.ErrorDataReceived, AddressOf ProcessMessageHandler
                End With
                .Start()
                .BeginErrorReadLine()
                .BeginOutputReadLine()
                Logger.Dbg("SWMM Started")
                .WaitForExit()
                If .ExitCode = 0 Then
                    lRunSimulation = True
                End If
            End With
        End If
        Return lRunSimulation
    End Function

    Private Sub ProcessMessageHandler(ByVal aSendingProcess As Object, _
                                      ByVal aOutLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(aOutLine.Data) Then
            Dim lMessage As String = aOutLine.Data.ToString
            Logger.Dbg("MsgFromSWMM5 " & lMessage)
        End If
    End Sub

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If Not MyBase.Open(aFileName, aAttributes) Then
            Return False
        Else
            Blocks.Clear()
            Dim lPrevDir As String = CurDir()
            ChDriveDir(IO.Path.GetDirectoryName(Me.Specification))
            Dim lSR As New IO.StreamReader(aFileName)
            'First set up a lookup using Blocks
            While Not lSR.EndOfStream
                Dim lBlockName As String = lSR.ReadLine.ToUpper
                Dim lBlockContents As String = ReadBlockContents(lBlockName, lSR)
                Blocks.Add(New atcSWMMBlock(lBlockName, lBlockContents))
            End While
            lSR.Close()

            'Then create classes using their contents
            For Each lBlock As atcSWMMBlock In Blocks
                Select Case lBlock.Name
                    Case "[TITLE]" : Title = lBlock
                    Case "[OPTIONS]" : Options.FromString(lBlock.Content)
                    Case "[EVAPORATION]" : Evaporation.FromString(lBlock.Content)
                    Case "[TEMPERATURE]" : Temperature.FromString(lBlock.Content)
                    Case "[TIMESERIES]"
                        'TODO:parse into Evaporation or Temperature block
                        Dim lLines() As String = lBlock.Content.Split(vbCrLf)
                        Dim laTSFile As String = String.Empty
                        'Dim lScenarioPrev As String = Regex.Split(lLines(0).Trim(), "\s+")(0)
                        Dim lScenarioPrev As String = String.Empty
                        For I As Integer = 0 To lLines.Length - 1
                            If Not lLines(I).Trim().StartsWith(";") And lLines(I).Trim().Length > 0 Then
                                lScenarioPrev = atcSWMMProject.SplitSpaceDelimitedWithQuotes(lLines(I).Trim())(0).Trim()
                                Exit For
                            End If
                        Next
                        For I As Integer = 0 To lLines.Length - 1
                            If Not lLines(I).Trim().StartsWith(";") And lLines(I).Trim().Length > 0 Then

                                Dim lItems As Generic.List(Of String) = atcSWMMProject.SplitSpaceDelimitedWithQuotes(lLines(I).Trim())
                                Dim lScenario As String = lItems(0).Trim()
                                Dim lBlockWithTS As Object = Nothing
                                Dim lRainGageName As String = String.Empty

                                If Temperature.Timeseries IsNot Nothing AndAlso Temperature.Timeseries.Attributes.GetValue("Scenario") = lScenario Then
                                    lBlockWithTS = Temperature
                                ElseIf Evaporation.Timeseries IsNot Nothing AndAlso Evaporation.Timeseries.Attributes.GetValue("Scenario") = lScenario Then
                                    lBlockWithTS = Evaporation
                                Else
                                    For Each lRaingage As atcSWMMRainGage In RainGages
                                        If lRaingage.TimeSeries.Attributes.GetValue("Scenario") = lScenario Then
                                            lBlockWithTS = RainGages
                                            lRainGageName = lRaingage.Name
                                            Exit For
                                        End If
                                    Next
                                    'perhaps for each of other type of objects
                                End If

                                If lLines(I).Contains("FILE") Then
                                    IsFileData = True
                                    'TODO: need to anticipate multiple TS of any block type
                                    If lBlockWithTS IsNot Nothing Then
                                        lBlockWithTS.TimeseriesFromFile(lItems(2).Trim().Trim(""""), lBlockWithTS.Timeseries)
                                        lBlockWithTS.Timeseries.ValuesNeedToBeRead = False
                                    End If
                                Else
                                    'assuming only raingage use infile timeseries as in Example1.inp
                                    'assuming dates is not give, only time (continuous?) as in Example1.inp
                                    If lBlockWithTS.GetType.Name = "atcSWMMRainGages" Then
                                        If lBlockWithTS(lRainGageName).TimeSeries.ValuesNeedToBeRead Then
                                            If lScenarioPrev = lScenario Then
                                                lBlockWithTS.AddValue(lLines(I), lRainGageName, False)
                                                If I = lLines.Length - 1 Then
                                                    lBlockWithTS.AddValue("", lRainGageName, True)
                                                End If
                                            Else
                                                lBlockWithTS.AddValue(lLines(I), lRainGageName, True)
                                                lScenarioPrev = lScenario
                                            End If
                                        End If
                                    Else

                                    End If
                                End If
                            End If
                        Next

                        'By now, if there is data to be read, then they are read already
                        'so close them down
                        If Temperature.Timeseries IsNot Nothing Then Temperature.Timeseries.ValuesNeedToBeRead = False
                        If Evaporation.Timeseries IsNot Nothing Then Evaporation.Timeseries.ValuesNeedToBeRead = False
                        If RainGages IsNot Nothing Then
                            For Each lRaingage As atcSWMMRainGage In RainGages
                                If lRaingage.TimeSeries IsNot Nothing Then
                                    lRaingage.TimeSeries.ValuesNeedToBeRead = False
                                End If
                            Next
                        End If
                    Case "[RAINGAGES]", "[SYMBOLS]"
                        RainGages.FromString(lBlock.Name & vbCrLf & lBlock.Content)
                        If Temperature.Timeseries IsNot Nothing Then
                            Temperature.Timeseries.Attributes.SetValue("interval", RainGages(0).TimeSeries.Attributes.GetValue("interval"))
                        End If
                        If Evaporation.Timeseries IsNot Nothing Then
                            Evaporation.Timeseries.Attributes.SetValue("interval", RainGages(0).TimeSeries.Attributes.GetValue("interval"))
                        End If
                        'any time multiple sections are used to build a single object type, the block's name is needed
                        'The block's name is not part of the "content" during ReadBlockContents above

                        'Case "[JUNCTIONS]", "[OUTFALLS]", "[STORAGE]", "[COORDINATES]", "[INFLOWS]" : Nodes.FromString(lBlock.Content)
                        'Case "[CONDUITS]", "[XSECTIONS]", "[LOSSES]", "[VERTICES]" : Conduits.FromString(lBlock.Content)
                        'Case "[SUBCATCHMENTS]", "[SUBAREAS]", "[INFILTRATION]", "[COVERAGES]", "[LOADINGS]", "[Polygons]" : Catchments.FromString(lBlock.Content)
                        'Case "[LANDUSES]" : Landuses.FromString(lBlock.Content)
                        'Case "[POLLUTANTS]" : Pollutants.FromString(lBlock.Content)
                        'Case "[PUMPS]" : Pumps.FromString(lBlock.Content)
                        'Case "[CONTROLS]" : Controls.FromString(lBlock.Content)
                        'Case "[REPORT]" : Report.FromString(lBlock.Content)
                        'Case "[TAGS]" : Tags.FromString(lBlock.Content)
                        'Case "[MAP]" : Map.FromString(lBlock.Content)
                        'Case "[BACKDROP]"
                        'Case "[LABELS]"
                        'Case "[CURVES]"
                        'Case "[DWF]"
                        'Case "[PATTERNS]"
                        'Case Else
                        '    'Logger.Dbg("'" & lBlock.Name & "' is not a known input block  ")
                End Select
            Next

            If RainGages IsNot Nothing Then
                For Each lRaingage As atcSWMMRainGage In RainGages
                    If lRaingage.TimeSeries IsNot Nothing Then
                        DataSets.Add(lRaingage.TimeSeries.Attributes.GetValue("ID"), lRaingage.TimeSeries)
                    End If
                Next
            End If
            If Temperature IsNot Nothing AndAlso Temperature.Timeseries IsNot Nothing Then
                DataSets.Add(Temperature.Timeseries.Attributes.GetValue("ID"), Temperature.Timeseries)
            End If
            If Evaporation IsNot Nothing AndAlso Evaporation.Timeseries IsNot Nothing Then
                'Setting more attributes for evaporation's timeseries 
                'Setting Evap's latitude using Temperature gage's latitude
                'need to consider getting this from the climate file
                For Each lParamKey As String In Temperature.AuxiParms.Keys
                    If lParamKey = "SNOWMELT" Then
                        Dim lParams As String = Temperature.AuxiParms.Item(lParamKey).Trim()
                        Dim lItems As Generic.List(Of String) = atcSWMMProject.SplitSpaceDelimitedWithQuotes(lParams)
                        Dim lLatDeg As Double = "40.0"
                        If Double.TryParse(lItems(4), lLatDeg) Then
                            Evaporation.Timeseries.Attributes.SetValue("LatDeg", lLatDeg)
                        End If
                        lItems.Clear()
                        Exit For
                    End If
                Next
                If RainGages.Count > 0 Then
                    Dim lInterval As Double = RainGages(0).TimeSeries.Attributes.GetValue("interval")
                    With Evaporation.Timeseries.Attributes
                        .SetValue("interval", lInterval) ' this is to go with the raingages' interval
                        If lInterval Mod JulianHour = 0 Then
                            .SetValue("tu", atcTimeUnit.TUHour)
                            .SetValue("ts", CInt(lInterval / JulianHour))
                        ElseIf lInterval Mod JulianMinute = 0 Then
                            .SetValue("tu", atcTimeUnit.TUMinute)
                            .SetValue("ts", CInt(lInterval / JulianMinute))
                        ElseIf lInterval Mod JulianSecond = 0 Then
                            .SetValue("tu", atcTimeUnit.TUSecond)
                            .SetValue("ts", CInt(lInterval / JulianSecond))
                        End If
                    End With
                End If
                DataSets.Add(Evaporation.Timeseries.Attributes.GetValue("ID"), Evaporation.Timeseries)
            End If

            'Save(IO.Path.Combine(IO.Path.GetDirectoryName(aFileName), IO.Path.GetFileNameWithoutExtension(aFileName) & "_test.inp"), EnumExistAction.ExistReplace)
            ChDriveDir(lPrevDir)
            Return True
        End If
    End Function

    Public Overrides Sub ReadData(ByVal aData As atcData.atcDataSet)
        'MyBase.ReadData(aData)
    End Sub

    Shared Function ReadBlockContents(ByVal aBlockName As String, ByVal aSR As IO.StreamReader) As String
        'Logger.Dbg("LoadGenericDummy " & aBlockName)
        If aBlockName = "[TIMESERIES]" Then
            Logger.Dbg("Timeseries")
        End If
        Dim lContents As String = ""
        Dim lNextLine As String = aSR.ReadLine
        While True
            If lNextLine.Length = 0 Then
                If aSR.Peek = Asc("[") Then
                    Exit While
                End If
            End If

            lContents &= (lNextLine & vbCrLf)
            If aSR.EndOfStream Then
                Exit While
            Else
                lNextLine = aSR.ReadLine
            End If
        End While
        If lContents.Length < 2 Then
            Logger.Dbg("  EmptyBlockFor '" & aBlockName & "'")
            lContents = vbCrLf
        Else
            lContents = lContents.Substring(0, lContents.Length - 2)
        End If
        Return lContents
    End Function

    Public Overrides Function Save(ByVal aFileName As String, _
                          Optional ByVal ExistAction As EnumExistAction = EnumExistAction.ExistReplace) As Boolean
        If IO.File.Exists(aFileName) Then
            Select Case ExistAction
                Case EnumExistAction.ExistReplace : IO.File.Delete(aFileName)
                Case EnumExistAction.ExistAskUser
                    If MsgBox("Replace Existing " & aFileName, MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        IO.File.Delete(aFileName)
                    Else
                        Return False
                    End If
                Case Else : Return False
            End Select
        End If

        Dim lSW As New IO.StreamWriter(aFileName)
        'Dim lBlocksWritten As New StringBuilder
        Specification = aFileName

        For Each lBlock As IBlock In Blocks
            Select Case lBlock.Name
                Case "[TITLE]" : lSW.WriteLine(Title.ToString)
                Case "[OPTIONS]" : lSW.WriteLine(Options.ToString)
                Case "[EVAPORATION]"
                    If Evaporation.Timeseries Is Nothing Then
                        lSW.WriteLine(lBlock.ToString)
                    Else
                        lSW.WriteLine(Evaporation.ToString)
                    End If
                Case "[TEMPERATURE]"
                    If Temperature.Timeseries Is Nothing Then
                        lSW.WriteLine(lBlock.ToString)
                    Else
                        lSW.WriteLine(Temperature.ToString)
                    End If
                Case "[RAINGAGES]"
                    lSW.WriteLine(RainGages.ToString)
                    RainGages.TimeSeriesToFile()
                Case "[SUBCATCHMENTS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[SUBAREAS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[INFILTRATION]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[JUNCTIONS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[OUTFALLS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[CONDUITS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[XSECTIONS]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[LOSSES]"
                    'lSW.WriteLine(Losses.ToString)
                    lSW.WriteLine(lBlock.ToString)
                Case "[LANDUSES]"
                    lSW.WriteLine(lBlock.ToString)
                Case "[COVERAGES]"
                    'lSW.WriteLine(Landuses.CoveragesToString)
                    lSW.WriteLine(lBlock.ToString)
                Case "[TIMESERIES]"
                    If Not RainGages.UseBlockText Then lSW.WriteLine(atcSWMMRainGages.TimeSeriesHeaderToString)
                    If IsFileData OrElse Options.EJDate - Options.SJDate >= 30 Then
                        'more than 30 days, write to a separate data file
                        If Temperature.Timeseries IsNot Nothing Then
                            lSW.WriteLine(Temperature.TimeSeriesFileNamesToString)
                        End If
                        If Evaporation.Timeseries IsNot Nothing Then
                            lSW.WriteLine(Evaporation.TimeSeriesFileNamesToString)
                        End If

                        If Temperature.Timeseries IsNot Nothing Then Temperature.TimeSeriesToFile()
                        If Evaporation.Timeseries IsNot Nothing Then Evaporation.TimeSeriesToFile()
                    Else 'write data inline in project file
                        If Temperature.Timeseries IsNot Nothing Then Temperature.TimeSeriesToStream(lSW)
                        If Evaporation.Timeseries IsNot Nothing Then Evaporation.TimeSeriesToStream(lSW)
                        If RainGages.Count > 0 Then
                            If RainGages.UseBlockText Then
                                lSW.WriteLine(lBlock.ToString)
                            Else
                                RainGages.TimeSeriesToStream(lSW)
                            End If
                        End If
                    End If
                Case "[REPORT]"
                    'lSW.WriteLine(Report.ToString)
                    lSW.WriteLine(lBlock.ToString)
                Case "[TAGS]"
                    'lSW.WriteLine(Tags.ToString)
                    lSW.WriteLine(lBlock.ToString)
                Case "[MAP]"
                    'lSW.WriteLine(Map.ToString)
                    lSW.WriteLine(lBlock.ToString)
                    lSW.WriteLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
                Case "[COORDINATES]"
                    If lBlock.ToString.Trim = lBlock.Name Then
                        lSW.WriteLine(Nodes.CoordinatesToString)
                    Else
                        lSW.WriteLine(lBlock.ToString)
                    End If
                Case "[VERTICES]"
                    If lBlock.ToString.Trim = lBlock.Name Then
                        lSW.WriteLine(Conduits.VerticesToString)
                    Else
                        lSW.WriteLine(lBlock.ToString)
                    End If
                Case "[Polygons]"
                    If lBlock.ToString.Trim = lBlock.Name Then
                        lSW.WriteLine(Catchments.PolygonsToString)
                    Else
                        lSW.WriteLine(lBlock.ToString)
                    End If
                Case "[SYMBOLS]"
                    If lBlock.ToString.Trim = lBlock.Name Then
                        lSW.WriteLine(RainGages.CoordinatesToString)
                    Else
                        lSW.WriteLine(lBlock.ToString)
                    End If
                Case "[BACKDROP]"
                    If lBlock.ToString.Trim = lBlock.Name Then
                        If BackdropFile.Length > 0 Then
                            lSW.WriteLine("")
                            lSW.WriteLine("[BACKDROP]")
                            lSW.WriteLine("FILE       " & """" & BackdropFile & """")
                            lSW.WriteLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
                        End If
                    Else
                        lSW.WriteLine(lBlock.ToString)
                    End If
                Case Else
                    'Write any blocks not already written above
                    lSW.WriteLine(lBlock.ToString)
            End Select
        Next

        lSW.Close()

        Return True
    End Function

    Public Function TimeSeriesToString(ByVal aTimeSeries As atcData.atcTimeseries, _
                                       ByVal aTimeseriesTag As String, _
                                       Optional ByVal aType As String = "PREC") As String
        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.SJDate, True)
        If Options.SJDate = aTimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.EJDate, True)
        Dim lSB As New StringBuilder
        Dim lValue As String = ""
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            If aType = "PREC" Then
                If Not (aTimeSeries.Values(lIndex + 1) > 0.0) Then
                    Continue For
                End If
                lSB.Append(StrPad(aTimeseriesTag, 16, " ", False))
                lSB.Append(" ")
            End If
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String
            If aType = "PREC" Then
                lDateString = lDate(0) & "  " & lDate(1) & "  " & lDate(2) & "  " & lDate(3) & "  " & lDate(4)
            Else
                lDateString = lDate(1) & "/" & lDate(2) & "/" & lDate(0) & " " & lDate(3) & ":" & lDate(4)
            End If
            lSB.Append(StrPad(lDateString, 20, " ", False))
            lSB.Append(" ")

            If aType = "PEVT" Then
                lValue = Format(aTimeSeries.Values(lIndex + 1), "0.0000000000")
            Else
                lValue = Format(aTimeSeries.Values(lIndex + 1), "0.000")
            End If
            lSB.Append(StrPad(lValue, 10, " ", False))
            lSB.Append(vbCrLf)
        Next
        Return lSB.ToString
    End Function

    Public Sub TimeSeriesToStream(ByVal aTimeSeries As atcData.atcTimeseries, _
                                  ByVal aTimeseriesTag As String, _
                                  ByVal aSW As IO.StreamWriter)

        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.SJDate, True)
        If Options.SJDate = aTimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.EJDate, True)
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            aSW.Write(StrPad(aTimeseriesTag, 16, " ", False))
            aSW.Write(" ")
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
            Dim lTimeString As String = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
            aSW.Write(StrPad(lDateString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(lTimeString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(Format(aTimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
            aSW.Write(vbCrLf)
        Next
    End Sub

    Public Sub RainTSToStream(ByVal aTimeSeries As atcData.atcTimeseries, _
                                  ByVal aTimeseriesTag As String, _
                                  ByVal aSW As IO.StreamWriter)
        Dim lStartIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.SJDate, True)
        If Options.SJDate = aTimeSeries.Dates.Values(0) Or lStartIndex < 0 Then
            lStartIndex = 0
        End If
        Dim lEndIndex As Integer = aTimeSeries.Dates.IndexOfValue(Options.EJDate, True)
        If lEndIndex < 0 Then lEndIndex = aTimeSeries.Dates.numValues
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            If lIndex >= 1 Then
                If Not aTimeSeries.Value(lIndex + 1) > 0 Then
                    Continue For
                End If
            End If
            aSW.Write(StrPad(aTimeseriesTag, 16, " ", False))
            aSW.Write(" ")
            Dim lJDate As Double = aTimeSeries.Dates.Values(lIndex)
            Dim lDate(6) As Integer
            J2Date(lJDate, lDate)
            Dim lDateString As String = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
            Dim lTimeString As String = lDate(3).ToString.PadLeft(2, "0") & ":" & lDate(4).ToString.PadLeft(2, "0")
            aSW.Write(StrPad(lDateString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(lTimeString, 10, " ", False))
            aSW.Write(" ")
            aSW.Write(StrPad(Format(aTimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
            aSW.Write(vbCrLf)
        Next

    End Sub

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lSWMMexe As String = atcUtility.FindFile("Please locate the EPA SWMM 5.0 Executable", "\Program Files\EPA SWMM 5.0\epaswmm5.exe")
            If IO.File.Exists(lSWMMexe) Then
                LaunchProgram(lSWMMexe, IO.Path.GetDirectoryName(aInputFileName), "/f " & aInputFileName, False)
                Logger.Dbg("SWMM launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA SWMM 5.0 Executable to open " & aInputFileName, MsgBoxStyle.Critical, "BASINS SWMM Problem")
            End If
        Else
            Logger.Msg("Cannot find SWMM 5.0 Input File " & aInputFileName)
        End If
    End Sub

    ''' <summary>
    ''' Recursive, only use on short string!
    ''' </summary>
    ''' <param name="aFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FilterFileName(ByVal aFileName As String) As String
        Dim lInValidChars() As Char = {"/"c, "\"c, ":"c, "*"c, "?"c, """", "|"c, "<"c, ">"c}
        Dim lNewName As String = aFileName
        For Each lInvalidChar As Char In lInValidChars
            If aFileName.IndexOf(lInvalidChar) >= 0 Then
                lNewName = aFileName.Replace(lInvalidChar, "")
                lNewName = FilterFileName(lNewName)
            End If
        Next
        Return lNewName
    End Function

    Public Shared Function SplitSpaceDelimitedWithQuotes(ByVal aSplitThis As String) As Generic.List(Of String)
        Dim lItems As New Generic.List(Of String) ' = Regex.Split(lLines(I).Trim(), "\s+")
        Dim lCurrItem As String = ""
        For lCharIndex As Integer = 0 To aSplitThis.Length - 1
            Dim lChar As String = aSplitThis.Substring(lCharIndex, 1)
            Select Case lChar
                Case " ", vbTab
                    If lCurrItem.Length > 0 Then
                        lItems.Add(lCurrItem)
                        lCurrItem = ""
                    End If
                Case """"
                    Dim lEndQuote As Integer = aSplitThis.IndexOf("""", lCharIndex + 1)
                    If lEndQuote > lCharIndex Then
                        lCurrItem = aSplitThis.Substring(lCharIndex + 1, lEndQuote - lCharIndex - 1)
                        lCharIndex = lEndQuote
                    Else
                        lCurrItem &= lChar
                    End If
                Case Else
                    lCurrItem &= lChar
            End Select
        Next
        If lCurrItem.Length > 0 Then
            lItems.Add(lCurrItem)
            lCurrItem = ""
        End If
        Return lItems
    End Function
End Class
