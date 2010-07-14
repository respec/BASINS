Imports System.Text
Imports System.Collections.Specialized

Imports MapWinUtility

Imports atcUtility

Public Class atcSWMMProject
    Private pIsMetric As Boolean

    Public Blocks As atcSWMMBlocks

    Public Options As atcSWMMOptions
    Public Catchments As atcSWMMCatchments
    Public Conduits As atcSWMMConduits
    Public Losses As atcSWMMBlock 'TODO: create class
    Public Report As atcSWMMBlock 'TODO: create class
    Public Tags As atcSWMMBlock 'TODO: create class
    Public Map As atcSWMMBlock 'TODO: create class
    Public Nodes As atcSWMMNodes
    Public Landuses As atcSWMMLanduses
    Public RainGages As atcSWMMRainGages
    Public Evaporation As atcSWMMEvaporation
    Public Temperature As atcSWMMTemperature

    Public Name As String = ""
    Public FileName As String = ""
    Public Title As String = ""

    Public BackdropFile As String = ""
    Public BackdropX1 As Double = 0.0
    Public BackdropY1 As Double = 0.0
    Public BackdropX2 As Double = 0.0
    Public BackdropY2 As Double = 0.0
    Public MapUnits As String = "METERS"

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Name = ""
        FileName = ""
        Title = ""
        BackdropFile = ""

        Options = New atcSWMMOptions
        Catchments = New atcSWMMCatchments(Me)
        Conduits = New atcSWMMConduits(Me)
        Losses = New atcSWMMBlock("[LOSSES]", _
                                  ";;Link           Inlet      Outlet     Average    Flap Gate " & vbCrLf _
                                & ";;-------------- ---------- ---------- ---------- ----------" & vbCrLf)
        Report = New atcSWMMBlock("[REPORT]", _
                                  "INPUT      NO" & vbCrLf _
                                & "CONTROLS   NO" & vbCrLf)
        Tags = New atcSWMMBlock("[TAGS]", vbCrLf)
        Map = New atcSWMMBlock("[MAP]", _
                               "UNITS      " & MapUnits & vbCrLf)
        Nodes = New atcSWMMNodes
        Landuses = New atcSWMMLanduses
        RainGages = New atcSWMMRainGages(Me)
        Evaporation = New atcSWMMEvaporation(Me)
        Temperature = New atcSWMMTemperature(Me)

        Blocks = New atcSWMMBlocks
        Blocks.Add(Options)
        Blocks.Add(Catchments)
        Blocks.Add(Conduits)
        Blocks.Add(Losses)
        Blocks.Add(Report)
        Blocks.Add(Tags)
        Blocks.Add(Map)
        Blocks.Add(Nodes)
        Blocks.Add(Landuses)
        Blocks.Add(RainGages)
        Blocks.Add(Evaporation)
        Blocks.Add(Temperature)
    End Sub

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
        Dim lSWMMExeName As String = GetSetting("BASINS4", "SWMM", "SimulationExeFileName", "")
        If Not IO.File.Exists(lSWMMExeName) Then
            lSWMMExeName = FindFile("Please locate SWMM5 Exe File", "swmm5.exe", "exe")
            If IO.File.Exists(lSWMMExeName) Then
                SaveSetting("BASINS4", "SWMM", "SimulationExeFileName", "")
            End If
        End If
        If IO.File.Exists(lSWMMExeName) Then
            Dim lProcess As New System.Diagnostics.Process
            With lProcess
                With .StartInfo
                    .FileName = lSWMMExeName
                    .WorkingDirectory = IO.Path.GetDirectoryName(FileName)
                    Dim lFileNameNoExt As String = IO.Path.GetFileNameWithoutExtension(FileName)
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

    Public Function Load(ByVal aFileName As String) As Boolean
        If Not IO.File.Exists(aFileName) Then
            Logger.Msg("File '" & aFileName & "' not found", "SWMMProjectLoadProblem")
            Return False
        Else
            Clear()
            FileName = aFileName
            Dim lSR As New IO.StreamReader(aFileName)
            While Not lSR.EndOfStream
                Dim lBlockName As String = lSR.ReadLine.ToUpper
                Dim lBlockContents As String = ReadBlockContents(lBlockName, lSR)

                Select Case lBlockName
                    Case "[TITLE]" : Title = lBlockContents
                    Case "[OPTIONS]" : Options.FromString(lBlockContents)
                        'Case "[EVAPORATION]" : Evaporation.FromString(lBlockContents)
                        'Case "[TEMPERATURE]" : Temperature.FromString(lBlockContents)
                        'Case "[TIMESERIES]"   'TODO:parse into Evaporation or Temperature block
                        'Case "[RAINGAGES]" : RainGages.FromString(lBlockContents)
                        'Case "[CONDUITS]" : Conduits.FromString(lBlockContents)
                    Case "[RAINFALL]", "[SUBCATCHMENTS]", "[SUBAREAS]", "[INFILTRATION]", _
                         "[CONDUITS]", "[XSECTIONS]", "[INFLOWS]", "[POLLUTANTS]", "[LOADINGS]", _
                         "[BUILDUP]", "[WASHOFF]", "[LOSSES]", "[LANDUSES]", "[COVERAGES]", _
                         "[REPORT]", "[TAGS]", "[MAP]", "[COORDINATES]", "[VERTICES]", _
                         "[POLYGONS]", "[SYMBOLS]", "[BACKDROP]", "[TAGS]", "[LABELS]", _
                         "[STORAGE]", "[PUMPS]", "[CONTROLS]", "[DWF]", "[CURVES]", "[PATTERNS]"
                        Blocks.Add(New atcSWMMBlock(lBlockName, lBlockContents))
                        'TODO: parse these into better objects!
                        'TODO: [JUNCTIONS] and [OUTFALLS] currently live inside Nodes
                        'TODO: [XSECTIONS] lives inside Conduits

                    Case Else
                        Logger.Dbg("'" & lBlockName & "' is not a known input block  ")
                End Select
            End While
        End If
    End Function

    Shared Function ReadBlockContents(ByVal aBlockName As String, ByVal aSR As IO.StreamReader) As String
        Logger.Dbg("LoadGenericDummy " & aBlockName)
        Dim lBlockComplete As Boolean = False
        Dim lContents As String = ""
        Dim lNextLine As String = aSR.ReadLine
        While Not lBlockComplete
            lContents &= (lNextLine & vbCrLf)
            If aSR.EndOfStream Then
                lBlockComplete = True
                lNextLine = ""
            Else
                lNextLine = aSR.ReadLine
                If lNextLine.Length = 0 Then
                    If aSR.Peek = Asc("[") Then
                        lBlockComplete = True
                    End If
                End If
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

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSW As New IO.StreamWriter(aFileName)
        Dim lBlocksWritten As New StringBuilder
        FileName = aFileName

        lBlocksWritten.Append("[TITLE]")
        lSW.WriteLine("[TITLE]")
        lSW.WriteLine(Title)
        lSW.WriteLine()

        lBlocksWritten.Append("[OPTIONS]")
        lSW.WriteLine(Options.ToString)

        lBlocksWritten.Append("[EVAPORATION]")
        lSW.WriteLine(Evaporation.ToString)

        lBlocksWritten.Append("[TEMPERATURE]")
        lSW.WriteLine(Temperature.ToString)

        lBlocksWritten.Append("[RAINGAGES]")
        lSW.WriteLine(RainGages.ToString)
        RainGages.TimeSeriesToFile()

        lBlocksWritten.Append("[SUBCATCHMENTS]")
        lSW.WriteLine(Catchments.ToString)

        lBlocksWritten.Append("[SUBAREAS]")
        lSW.WriteLine(Catchments.SubareasToString)

        lBlocksWritten.Append("[INFILTRATION]")
        lSW.WriteLine(Catchments.InfiltrationToString)

        lBlocksWritten.Append("[JUNCTIONS][OUTFALLS]")
        lSW.WriteLine(Nodes.ToString)

        lBlocksWritten.Append("[CONDUITS][XSECTIONS]")
        lSW.WriteLine(Conduits.ToString)

        lBlocksWritten.Append("[LOSSES]")
        lSW.WriteLine(Losses.ToString)

        lBlocksWritten.Append("[LANDUSES]")
        lSW.WriteLine(Landuses.ToString)

        lBlocksWritten.Append("[COVERAGES]")
        lSW.WriteLine(Landuses.CoveragesToString)

        lBlocksWritten.Append("[TIMESERIES]")
        lSW.WriteLine(atcSWMMEvaporation.TimeSeriesHeaderToString)
        If Options.EJDate - Options.SJDate < 30 Then
            Evaporation.TimeSeriesToStream(lSW)
            Temperature.TimeSeriesToStream(lSW)
        Else
            'more than 30 days, write to file
            lSW.WriteLine(Evaporation.TimeSeriesFileNamesToString)
            lSW.WriteLine(Temperature.TimeSeriesFileNamesToString)
            Evaporation.TimeSeriesToFile()
            Temperature.TimeSeriesToFile()
        End If
        lSW.WriteLine()

        lBlocksWritten.Append("[REPORT]")
        lSW.WriteLine(Report.ToString)

        lBlocksWritten.Append("[TAGS]")
        lSW.WriteLine(Tags.ToString)

        lBlocksWritten.Append("[MAP]")
        lSW.WriteLine(Map.ToString)

        lBlocksWritten.Append("[COORDINATES]")
        lSW.WriteLine(Nodes.CoordinatesToString)

        lBlocksWritten.Append("[VERTICES]")
        lSW.WriteLine(Conduits.VerticesToString)

        lBlocksWritten.Append("[POLYGONS]")
        lSW.WriteLine(Catchments.PolygonsToString)

        lBlocksWritten.Append("[SYMBOLS]")
        lSW.WriteLine(RainGages.CoordinatesToString)

        lBlocksWritten.Append("[BACKDROP]")
        If BackdropFile.Length > 0 Then
            lSW.WriteLine("")
            lSW.WriteLine("[BACKDROP]")
            lSW.WriteLine("FILE       " & """" & BackdropFile & """")
            lSW.WriteLine("DIMENSIONS " & Format(BackdropX1, "0.000") & " " & Format(BackdropY1, "0.000") & " " & Format(BackdropX2, "0.000") & " " & Format(BackdropY2, "0.000"))
        End If

        'Write any blocks not already written above
        For Each lBlock As IBlock In Blocks
            If Not lBlocksWritten.ToString.Contains(lBlock.Name.ToUpper) Then
                lSW.WriteLine(lBlock.ToString)
            End If
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
        For lIndex As Integer = lStartIndex To lEndIndex - 1
            If aType = "PREC" Then
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
            lSB.Append(StrPad(Format(aTimeSeries.Values(lIndex + 1), "0.000"), 10, " ", False))
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

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lSWMMexe As String = atcUtility.FindFile("Please locate the EPA SWMM 5.0 Executable", "\Program Files\EPA SWMM 5.0\epaswmm5.exe")
            If IO.File.Exists(lSWMMexe) Then
                LaunchProgram(lSWMMexe, IO.Path.GetDirectoryName(aInputFileName), "/f " & aInputFileName, False)
                Logger.Dbg("SWMM launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA SWMM 5.0 Executable", MsgBoxStyle.Critical, "BASINS SWMM Problem")
            End If
        Else
            Logger.Msg("Cannot find SWMM 5.0 Input File " & aInputFileName)
        End If
    End Sub
End Class
