'This project includes utilities to write WASP INP files; was designed to be used in conjunction with atcWASPPlugIn 
'which has UI for WASP model building tool.
'
'Tools originally written 2008 - 2010 by AquaTerra Corp (ATC) for Tim Wool, EPA.
'Significantly enhanced by Dr. Chris Wilson of Wilson Engineering, St. Louis at Tim Wool's request
'to include the following enhancements at a minimum:
'1. allow WASP user to select data from WRDB project tables (instead of or in addition to BASIN timeseries datasets)
'2. correct issues with creation of segments where NHD datasets have multi-line rivers
'3. correct for changed INP file format
'4. enhance to use SQLite tables maintained by WASP to discover available models, state variables, etc.
'5. enhance to allow model building specification to be saved and loaded
'6. change name of optional shape file so includes name of BASINS project
'7. fix UI so show loads expressed as Kg/Day (not mg/L)
'8. include means by which WRDB stations will be associated with WASP segments, and PCodes with state variables and time functions
'9. add ability to apply scale factors (and simple pre-defined unit conversions) to imported data
'10. change so segment information is written in segment number order
'11. other enhancements to UI

Imports MapWinUtility
Imports System.Text
Imports atcUtility
Imports atcData
Imports atcControls
Imports atcMwGisUtility

Public Class atcWASPProject

#Region "Public Variables and class instantiation..."
    Public Filename As String
    Public MinTravelTime As Double, MaxTravelTime As Double
    Public Segments As atcWASPSegments
    'Public InputTimeseriesCollection As atcWASPTimeseriesCollection
    Public SDate As Date = Date.MinValue
    Public EDate As Date = Date.MaxValue
    Public MetLayer As String = ""
    Public ModelType As Integer = 2
    Public Version As Integer = 2
    Public Name As String = ""
    Public INPFileName As String = ""
    Public SegmentFieldMap As New atcCollection
    'Public WASPConstituents As New atcCollection
    'Public WASPTimeFunctions As New atcCollection
    'Public WASPTimeFunctionIds As New atcCollection
    Public FlowStationCandidates As New atcWASPTimeseriesCollection
    Public AllStationCandidates As New atcWASPTimeseriesCollection

    'pointers to timeseries inputs as implemented for version 2.0 (create list for each flow, bound, & load and each segment (or reach) and each constituent)
    'each entry or array element is a string which indicates where the data are to be retrieved from
    'Public FlowTimeSeries As Generic.List(Of String)
    'Public BoundTimeSeries As Generic.List(Of String())
    'Public LoadTimeSeries As Generic.List(Of String())

    'dictionaries containing segment/station and constituent/pcode mappings
    Public StationMapping As Generic.Dictionary(Of String, String)
    Public PCodeMapping As Generic.Dictionary(Of String, clsPCodeMapping)

    'list of time series pointers associated with each time function for version 2.0
    Public TimeFunctionSeries() As clsTimeSeriesSelection

    Public WASPConstituents As Generic.List(Of clsWASPConstituent)
    Public WASPTimeFunctions As Generic.List(Of clsWASPTimeFunction)

    Public WriteErrors As String = ""

    Public Enum enumDataSourceType
        WRDB
        Basins
    End Enum

    'the following are used in creating the wasp inp file
    Friend NumFlowFunc(5) As Integer
    Friend FlowPathList As New List(Of Integer) ' subset of Headwaters that actually have flowpath defined, zero-based

    ''' <summary>
    ''' Instantiate WaspProject class
    ''' </summary>
    Public Sub New()
        Name = ""
        Segments = New atcWASPSegments
        Segments.WASPProject = Me
        'set field mapping for segments based on NHDPlus
        SegmentFieldMap.Clear()
        SegmentFieldMap.Add("GNIS_NAME", "Name")
        SegmentFieldMap.Add("COMID", "ID")
        SegmentFieldMap.Add("LINKNO", "ID")
        SegmentFieldMap.Add("LENGTHKM", "Length")
        SegmentFieldMap.Add("MeanWidth", "Width")
        SegmentFieldMap.Add("DSCOMID", "DownID")
        SegmentFieldMap.Add("DSLINKNO", "DownID")
        SegmentFieldMap.Add("TOCOMID", "DownID")
        SegmentFieldMap.Add("MAVELU", "Velocity")
        SegmentFieldMap.Add("MAFLOWU", "MeanAnnualFlow")
        SegmentFieldMap.Add("SLOPE", "Slope")
        SegmentFieldMap.Add("CUMDRAINAG", "CumulativeDrainageArea")
        SegmentFieldMap.Add("DIVERGENCE", "Divergence")

        'InputTimeseriesCollection = New atcWASPTimeseriesCollection

        'FlowTimeSeries = New Generic.List(Of String)
        'BoundTimeSeries = New Generic.List(Of String())
        'LoadTimeSeries = New Generic.List(Of String())
        'TimeFunctionSeries = New Generic.List(Of String)

        StationMapping = New Generic.Dictionary(Of String, String)
        PCodeMapping = New Generic.Dictionary(Of String, clsPCodeMapping)
    End Sub

#End Region

#Region "Load and Save WaspBuilder project files..."

    ''' <summary>
    ''' Save all project settings in a single .waspbuilder file which can be reloaded
    ''' </summary>
    ''' <returns>True if successful</returns>
    ''' <remarks>Save function not suitable because it creates multiple files; want all input in a single file (no time series data)</remarks>
    Public Function SaveProject(ByVal aFilename As String) As Boolean
        If String.IsNullOrEmpty(aFilename) Then Return False
        Dim sw As New IO.StreamWriter(aFilename)
        WriteLine(sw, "WASP Model Builder Header info: Version, Name, ModelType, MetLayer, Start/End Date-time, Min/Max Travel time")
        WriteLine(sw, My.Application.Info.Version.ToString(2)) 'so future versions of file can be discerned (e.g., 1.2)
        WriteLine(sw, Name)
        WriteLine(sw, ModelType)
        WriteLine(sw, MetLayer)

        WriteLine(sw, "{0}\t{1}", SDate.ToString("MM/dd/yyyy HH:mm"), EDate.ToString("MM/dd/yyyy HH:mm"))
        WriteLine(sw, "{0}\t{1}", MinTravelTime, MaxTravelTime)

        If Not Segments.Save(sw) Then Return False

        WriteLine(sw, "Station & PCode Mapping")

        With StationMapping
            WriteLine(sw, StationMapping.Count)
            For Each kv As KeyValuePair(Of String, String) In StationMapping
                WriteLine(sw, "{0}\t{1}", kv.Key, kv.Value)
            Next
        End With

        With PCodeMapping
            WriteLine(sw, PCodeMapping.Count)
            For Each kv As KeyValuePair(Of String, clsPCodeMapping) In PCodeMapping
                With kv.Value
                    WriteLine(sw, "{0}\t{1}\t{2}\t{3}", kv.Key, .PCode, clsTimeSeriesSelection.GetConversionName(.ConversionType), .ScaleFactor)
                End With
            Next
        End With

        'WriteLine(sw, "Flow Time Series")

        'With FlowTimeSeries
        '    WriteLine(sw, FlowTimeSeries.Count)
        '    For i As Integer = 0 To .Count - 1
        '        WriteLine(sw, .Item(i))
        '    Next
        'End With

        'WriteLine(sw, "Boundary Time Series")

        'With BoundTimeSeries
        '    WriteLine(sw, .Count)
        '    For i As Integer = 0 To .Count - 1
        '        Dim s As String = ""
        '        For j As Integer = 0 To .Item(i).Length - 1
        '            s &= IIf(j = 0, "", "\t") & .Item(i)(j)
        '        Next
        '        WriteLine(sw, s)
        '    Next
        'End With

        'WriteLine(sw, "Load Time Series")

        'With LoadTimeSeries
        '    WriteLine(sw, .Count)
        '    For i As Integer = 0 To .Count - 1
        '        Dim s As String = ""
        '        For j As Integer = 0 To .Item(i).Length - 1
        '            s &= IIf(j = 0, "", "\t") & .Item(i)(j)
        '        Next
        '        WriteLine(sw, s)
        '    Next
        'End With

        WriteLine(sw, "Time Function Series")

        WriteLine(sw, TimeFunctionSeries.Length)
        For i As Integer = 0 To TimeFunctionSeries.Length - 1
            WriteLine(sw, TimeFunctionSeries(i).ToFullString)
        Next

        sw.Close()
        sw.Dispose()
        Filename = aFilename

        Return True
    End Function

    ''' <summary>
    ''' Load project data from file
    ''' </summary>
    ''' <param name="aFilename">Name of project file (.waspbuilder)</param>
    ''' <returns>True if successfule</returns>
    Public Function LoadProject(ByVal aFilename As String) As Boolean

        Dim lWASPFolder As String = TestNull(My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\USEPA\WASP\8.0", "DatabaseDir", "C:\WASP8"), "C:\WASP8").Trim(New Char() {"\"}) & "\.."

        If Not My.Computer.FileSystem.DirectoryExists(lWASPFolder) Then
            'Logger.Message("Database directory for the WASP 8 program could not be found: " & pWASPFolder, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, Windows.Forms.DialogResult.OK)
            'WASP 8 is not ready to go, so in that case just use the epa.sqlite database being distributed with the plugin
            lWASPFolder = IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location)
        End If

        Segments.WASPProject = Me
        Dim sr As New IO.StreamReader(aFilename)
        Dim s As String = sr.ReadLine
        If Not s.StartsWith("WASP") Then Throw New Exception("Invalid file read: " & aFilename & "; first line was: " & s)
        Dim Version As String = sr.ReadLine
        Name = sr.ReadLine
        ModelType = sr.ReadLine

        For Each mdl As clsWASPModel In GetWASPModels(lWASPFolder)
            If mdl.ModelID = ModelType Then
                ReadWASPdb(lWASPFolder, mdl.Description) 'will redimension arrays based on number of constituents
                Exit For
            End If
        Next

        MetLayer = sr.ReadLine
        Dim ar() As String = sr.ReadLine.Split(vbTab)
        SDate = ar(0)
        EDate = ar(1)

        ar = sr.ReadLine.Split(vbTab)
        MinTravelTime = ar(0)
        MaxTravelTime = ar(1)

        Segments.Clear()
        If Not Segments.Load(sr) Then Return False

        sr.ReadLine() 'skip header line

        With StationMapping
            .Clear()
            Dim numsta As Integer = sr.ReadLine
            For i As Integer = 0 To numsta - 1
                Dim sm() As String = sr.ReadLine.Split(vbTab)
                .Add(sm(0), sm(1))
            Next
        End With

        With PCodeMapping
            .Clear()
            Dim numsta As Integer = sr.ReadLine
            For i As Integer = 0 To numsta - 1
                Dim sm() As String = sr.ReadLine.Split(vbTab)
                .Add(sm(0), New clsPCodeMapping(sm(1), sm(2), Val(sm(3))))
            Next
        End With

        sr.ReadLine() 'skip header line

        Dim cnt As Integer = sr.ReadLine
        ReDim TimeFunctionSeries(cnt - 1)
        For i As Integer = 0 To cnt - 1
            TimeFunctionSeries(i) = New clsTimeSeriesSelection(sr.ReadLine)
        Next

        sr.Close()
        sr.Dispose()

        Filename = aFilename

        Return True
    End Function

#End Region

#Region "Utility routines for dividing and combining Wasp segments..."

    ''' <summary>
    ''' Determine whether specified segment is a boundary segment
    ''' </summary>
    ''' <param name="aSegment">Wasp segment to check</param>
    ''' <returns>True if is boundary segment</returns>
    Public Function IsBoundary(ByVal aSegment As atcWASPSegment) As Boolean
        Dim lBoundary As Boolean = False

        Dim lDownBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If aSegment.DownID = lSegment.ID Then
                'this segment connects to one downstream
                lDownBoundary = False
                Exit For
            End If
        Next

        Dim lUpBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = aSegment.ID Then
                'an upstream segment connects to this one
                lUpBoundary = False
                Exit For
            End If
        Next

        If lUpBoundary Or lDownBoundary Then
            lBoundary = True
        End If

        Return lBoundary
    End Function

    ''' <summary>
    ''' Determine whether specified segment is a boundary segment
    ''' </summary>
    ''' <param name="aSegNum">Wasp segment number to check</param>
    ''' <returns>True if is boundary segment</returns>
    Public Function IsBoundary(ByVal aSegNum As Integer) As Boolean
        Return IsBoundary(Segments(aSegNum))
    End Function

    Public Function TravelTime(ByVal aLengthKM As Double, ByVal aVelocityMPS As Double) As Double
        Dim lTravelTime As Double = 0.0
        If aVelocityMPS > 0 Then
            lTravelTime = aLengthKM * 1000 / aVelocityMPS / (60 * 60 * 24)  'computes in days
        End If
        Return lTravelTime
    End Function

    Public Function GenerateSegments(ByVal aSegmentLayerIndex As Integer, ByVal aSelectedIndexes As atcCollection, ByVal aMaxTravelTime As Double, ByVal aMinTravelTime As Double) As Boolean
        With Me
            Try
                'populate the WASP classes from the shapefiles
                If .Segments.Count = 0 Then   'dont read these in again if we already have them, we might have already combined/divided them
                    .Segments.Clear()
                    Dim lTable As New atcUtility.atcTableDBF

                    'add only selected segments
                    Dim lSegmentShapefileName As String = GisUtil.LayerFileName(aSegmentLayerIndex)
                    If lTable.OpenFile(FilenameSetExt(lSegmentShapefileName, "dbf")) Then
                        Logger.Dbg("Add " & aSelectedIndexes.Count & " SegmentsFrom " & lSegmentShapefileName)
                        Dim lSegmentsSelected As New ArrayList '(Of atcWASP.atcWASPSegment)
                        For Each lShapeIndex As Integer In aSelectedIndexes
                            lTable.CurrentRecord = lShapeIndex + 1
                            Dim lSeg As New atcWASP.atcWASPSegment(WASPConstituents.Count)
                            lTable.PopulateObject(lSeg, .SegmentFieldMap)
                            Dim lTimeseriesCollection As New atcWASP.atcWASPTimeseriesCollection
                            'lSeg.InputTimeseriesCollection = lTimeseriesCollection
                            lSeg.BaseID = lSeg.ID   'store segment id before breaking up
                            GisUtil.LineCentroid(aSegmentLayerIndex, lShapeIndex, lSeg.CentroidX, lSeg.CentroidY) 'store centroid 
                            GisUtil.PointsOfLine(aSegmentLayerIndex, lShapeIndex, lSeg.PtsX, lSeg.PtsY)  'store point coordinates of vertices
                            lSegmentsSelected.Add(lSeg)
                        Next
                        .Segments.AddRange(NumberObjects(lSegmentsSelected, "Name"))
                    End If
                    Logger.Dbg("SegmentsCount " & .Segments.Count)

                    'calculate depth and width from mean annual flow and mean annual velocity
                    'Depth (ft)= a*DA^b (english):  a= 1.5; b=0.284    -- assumption from GBMM
                    For Each lSegment As atcWASPSegment In .Segments
                        lSegment.Depth = 1.5 * (lSegment.CumulativeDrainageArea ^ 0.284)   'gives depth in ft
                        lSegment.Width = (lSegment.MeanAnnualFlow / lSegment.Velocity) / lSegment.Depth  'gives width in ft
                    Next

                    'do unit conversions from NHDPlus units to WASP assumed units
                    For Each lSegment As atcWASPSegment In .Segments
                        lSegment.Velocity = SignificantDigits(lSegment.Velocity / 3.281, 3)  'convert ft/s to m/s
                        lSegment.MeanAnnualFlow = SignificantDigits(lSegment.MeanAnnualFlow / (3.281 ^ 3), 3) 'convert cfs to cms
                        'lSegment.DrainageArea = lSegment.DrainageArea  'already in sq km
                        lSegment.Depth = SignificantDigits(lSegment.Depth / 3.281, 3)  'convert ft to m
                        lSegment.Width = SignificantDigits(lSegment.Width / 3.281, 3)  'convert ft to m
                        lSegment.WaspName = lSegment.ID & IIf(IsNumeric(lSegment.Name), "", ":" & lSegment.Name) 'initialize to same as ID & name (unless name is number--drop it)
                    Next
                End If

                Dim lProblem As String = ""
                If aMinTravelTime > 0 Then 'minimum travel time has been set, combine the segments as needed
                    CombineSegments(aMinTravelTime, lProblem)
                End If

                If aMaxTravelTime > 0 Then 'maximum travel time has been set, divide the segments as needed
                    DivideSegments(aMaxTravelTime)
                End If

                lProblem = .Segments.RemoveBraidedSegments
                If lProblem.Length > 0 Then
                    Logger.Dbg("ProblemInGenerateSegmentsRemoveBraidedSegments " & lProblem)
                    Return False
                End If

                lProblem = .Segments.AssignWaspIds()
                If lProblem.Length > 0 Then
                    Logger.Dbg("ProblemInGenerateSegmentsAssignWaspIds " & lProblem)
                    Return False
                Else
                    Return True
                End If
            Catch lEX As Exception
                Logger.Dbg(lEX.Message)
                Return False
            End Try
        End With
    End Function

    ''' <summary>
    ''' Force two segments to be combined; secondary is assumed to be upstream of primary (and continguous of course)
    ''' </summary>
    ''' <param name="aSegmentPrimary"></param>
    ''' <param name="aSegmentSecondary"></param>
    ''' <remarks></remarks>
    Public Sub CombineSegments(ByVal aSegmentPrimary As atcWASPSegment, ByVal aSegmentSecondary As atcWASPSegment, ByVal aSecondaryUpstream As Boolean)
        Dim lNewSegments As New atcWASPSegments
        Dim lNewSegment As atcWASPSegment = CombineSegment(aSegmentPrimary, aSegmentSecondary, aSecondaryUpstream)

        'fix up all pointers to the segment being subsumed
        For Each seg As atcWASPSegment In Segments
            If aSecondaryUpstream Then
                If seg.DownID = aSegmentSecondary.ID Then seg.DownID = aSegmentPrimary.ID
            Else
                If seg.DownID = aSegmentPrimary.ID Then seg.DownID = aSegmentSecondary.ID
            End If
        Next

        For Each seg As atcWASPSegment In Segments
            If seg Is aSegmentPrimary Then 'substitue with combined segment
                lNewSegments.Add(lNewSegment)
            ElseIf seg Is aSegmentSecondary Then 'skip this one
            Else

                lNewSegments.Add(seg)
            End If
        Next
        Segments.Clear()
        Segments = lNewSegments
        Segments.WASPProject = Me
        Segments.AssignWaspIds()
    End Sub

    Private Sub CombineSegments(ByVal aMinTravelTime As Double, ByRef aProblem As String)
        Dim lDownstreamKey As String = Segments.DownstreamKey(aProblem)
        If aProblem.Length = 0 Then
            Dim lShortSegmentCount As Integer = DetermineShortSegments(aMinTravelTime, lDownstreamKey)
            Logger.Dbg("ShortSegmentCount " & lShortSegmentCount)
            If lShortSegmentCount > 0 Then 'fix them
                Dim lNewSegments As New atcWASPSegments
                CombineSegmentsDetail(lDownstreamKey, aMinTravelTime, lNewSegments)
                Segments.Clear()
                Segments = lNewSegments
                Segments.WASPProject = Me
            End If
        End If
    End Sub

    Private Function DetermineShortSegments(ByVal aMinTravelTime As Double, ByVal aDownstreamKey As String) As Integer
        Dim lShortSegmentCount As Integer = 0
        Dim lDownstreamSegment As atcWASPSegment = Segments(aDownstreamKey)
        'If aMinTravelTime > TravelTime(lDownstreamSegment.Length, lDownstreamSegment.Velocity) Then
        '    lShortSegmentCount += 1
        '    lDownstreamSegment.TooShort = True
        'End If
        lDownstreamSegment.CountAbove = 0
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = lDownstreamSegment.ID Then
                If aMinTravelTime > TravelTime(lSegment.Length, lSegment.Velocity) Then
                    lShortSegmentCount += 1
                    lSegment.TooShort = True
                End If
                lDownstreamSegment.CountAbove += 1
                lShortSegmentCount += DetermineShortSegments(aMinTravelTime, lSegment.ID)
            End If
        Next
        Return lShortSegmentCount
    End Function

    Private Sub CombineSegmentsDetail(ByVal aSegmentKey As String, ByVal aMinTravelTime As Double, ByRef aNewSegments As atcWASPSegments)
        Dim lSegment As atcWASPSegment = Segments(aSegmentKey)
        If Not lSegment.Removed Then
            Logger.Dbg("Combine " & aSegmentKey & " " & lSegment.TooShort)
            If lSegment.TooShort Then 'too short - combine with segment up or down
                Dim lSegmentCombined As atcWASPSegment = Nothing
                Dim lSegmentRemoved As atcWASPSegment = Nothing
                Dim lUpStreamSegment As atcWASPSegment = UpstreamMainSegment(lSegment.ID)
                If lUpStreamSegment IsNot Nothing Then 'combine with upstream
                    lSegmentCombined = CombineSegment(lSegment, lUpStreamSegment, True)
                    lSegmentRemoved = lUpStreamSegment
                    lUpStreamSegment.Removed = True
                Else 'combine with downstream (if possible)
                    Dim lDownStreamSegment As atcWASPSegment = Segments(lSegment.DownID)

                    Dim lDownStreamUpMainSegment As atcWASPSegment = UpstreamMainSegment(lDownStreamSegment.ID)
                    If lDownStreamUpMainSegment.Removed Then
                        'can't combine with a downstream segment that has already been removed, just keep this one for now
                        aNewSegments.Add(lSegment)

                        ''combine with new downstream segment (LCW) for case where segments are all in a series
                        'lDownStreamSegment = aNewSegments(lSegment.DownID) 'downstream segment, possibly already combined
                        'lSegmentCombined = CombineSegment(lSegment, lDownStreamSegment, False)

                    Else
                        If lDownStreamUpMainSegment.ID = lSegment.ID Then
                            lSegmentCombined = CombineSegment(lSegment, lDownStreamSegment, False)
                        Else
                            Logger.Dbg("Skip " & lSegment.ID & " Nothing up and not MainChannel")
                        End If
                    End If
                End If
                If lSegmentCombined IsNot Nothing Then
                    If aMinTravelTime - 0.001 > TravelTime(lSegmentCombined.Length, lSegmentCombined.Velocity) Then
                        Logger.Dbg("StillTooShort " & lSegmentCombined.ID & " " & TravelTime(lSegmentCombined.Length, lSegmentCombined.Velocity))
                        lSegmentCombined.TooShort = True
                        'TODO: what should we do now?
                    End If
                    If aNewSegments.Contains(lSegmentCombined.ID) Then
                        aNewSegments.Remove(lSegmentCombined.ID)
                    End If
                    aNewSegments.Add(lSegmentCombined)
                    'fix DownIds for upstream segments
                    If Not lSegmentRemoved Is Nothing Then
                        For Each lSegment In Segments
                            If lSegment.DownID = lSegmentRemoved.ID Then
                                lSegment.DownID = lSegmentCombined.ID
                            End If
                        Next
                    End If
                End If
            Else 'no problem, use as is
                aNewSegments.Add(lSegment)
            End If
            Logger.Dbg("NewSegmentCount " & aNewSegments.Count)
            'move on upstream
            For Each lSegment In Segments
                If lSegment.DownID = aSegmentKey And Not lSegment.Removed Then
                    CombineSegmentsDetail(lSegment.ID, aMinTravelTime, aNewSegments)
                End If
            Next
        Else
            Logger.Dbg("SkipRemovedSegment " & lSegment.ID)
        End If
    End Sub

    Private Function UpstreamMainSegment(ByVal aSegmentId As String) As atcWASPSegment
        Dim lUpstreamMainSegment As atcWASPSegment = Nothing
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = aSegmentId Then
                If lUpstreamMainSegment Is Nothing OrElse lUpstreamMainSegment.CumulativeDrainageArea < lSegment.CumulativeDrainageArea Then
                    lUpstreamMainSegment = lSegment
                End If
            End If
        Next
        Return lUpstreamMainSegment
    End Function

    Private Function CombineSegment(ByVal aSegmentPrimary As atcWASPSegment, _
                                    ByVal aSegmentSecondary As atcWASPSegment, _
                                    ByVal aSecondaryUpstream As Boolean) As atcWASPSegment
        Dim lSegment As atcWASPSegment
        If aSecondaryUpstream Then 'primary downstream
            lSegment = aSegmentPrimary.Clone
        Else 'secondary downstream
            lSegment = aSegmentSecondary.Clone
            'lSegment.DownID = aSegmentPrimary.DownID  'I think this is a bug
            'lSegment.ID = aSegmentPrimary.ID
        End If

        Try
            With lSegment
                Dim lLengthP As Double = aSegmentPrimary.Length
                Dim lLengthS As Double = aSegmentSecondary.Length
                .Length = lLengthP + lLengthS
                .Depth = ((aSegmentPrimary.Depth * lLengthP) + (aSegmentSecondary.Depth * lLengthS)) / .Length
                '.Name 
                .Roughness = ((aSegmentPrimary.Roughness * lLengthP) + (aSegmentSecondary.Roughness * lLengthS)) / .Length
                .Slope = ((aSegmentPrimary.Slope * lLengthP) + (aSegmentSecondary.Slope * lLengthS)) / .Length
                .Velocity = ((aSegmentPrimary.Velocity * lLengthP) + (aSegmentSecondary.Velocity * lLengthS)) / .Length
                .Width = ((aSegmentPrimary.Width * lLengthP) + (aSegmentSecondary.Width * lLengthS)) / .Length
                '.BaseID 
                .CentroidX = (aSegmentPrimary.CentroidX + aSegmentSecondary.CentroidX) / 2
                .CentroidY = (aSegmentPrimary.CentroidY + aSegmentSecondary.CentroidY) / 2
                Dim lPointCount As Integer = aSegmentPrimary.PtsX.GetLength(0) + aSegmentSecondary.PtsX.GetLength(0) - 1
                ReDim .PtsX(lPointCount)
                ReDim .PtsY(lPointCount)
                'TODO: assumes nhdplus convention - up to down, needs to be robust
                If aSecondaryUpstream Then
                    For lIndex As Integer = 0 To aSegmentSecondary.PtsX.GetLength(0) - 1
                        .PtsX(lIndex) = aSegmentSecondary.PtsX(lIndex)
                        .PtsY(lIndex) = aSegmentSecondary.PtsY(lIndex)
                    Next
                    Dim lBasePoint As Integer = aSegmentSecondary.PtsX.GetLength(0)
                    For lIndex As Integer = 0 To aSegmentPrimary.PtsX.GetLength(0) - 1
                        .PtsX(lBasePoint) = aSegmentPrimary.PtsX(lIndex)
                        .PtsY(lBasePoint) = aSegmentPrimary.PtsY(lIndex)
                        lBasePoint += 1
                    Next
                Else
                    For lIndex As Integer = 0 To aSegmentPrimary.PtsX.GetLength(0) - 1
                        .PtsX(lIndex) = aSegmentPrimary.PtsX(lIndex)
                        .PtsY(lIndex) = aSegmentPrimary.PtsY(lIndex)
                    Next
                    Dim lBasePoint As Integer = aSegmentPrimary.PtsX.GetLength(0) - 1
                    For lIndex As Integer = 0 To aSegmentSecondary.PtsX.GetLength(0) - 1
                        lBasePoint += 1
                        .PtsX(lBasePoint) = aSegmentSecondary.PtsX(lIndex)
                        .PtsY(lBasePoint) = aSegmentSecondary.PtsY(lIndex)
                    Next
                End If
                '.MeanAnnualFlow
                '.WASPID 
            End With
        Catch lEx As Exception
            Logger.Dbg(lEx.Message)
        End Try
        Return lSegment
    End Function

    ''' <summary>
    ''' Divide all or single segment into smaller parts based on desired maximum travel time or number of parts
    ''' </summary>
    ''' <param name="aMaxTravelTime">Maximum allowable travel time (days)</param>
    ''' <param name="aSegment">Optional segment to split (leave blank for all segments)</param>
    Public Sub DivideSegments(ByVal aMaxTravelTime As Double, Optional ByVal aSegment As atcWASPSegment = Nothing)
        Dim lNewSegments As New atcWASPSegments
        Dim lNewSegmentPositions As New atcCollection
        For lIndex As Integer = 1 To Segments.Count
            Dim lSegment As atcWASPSegment = Segments(lIndex - 1)
            If aSegment Is Nothing OrElse aSegment Is lSegment Then
                If TravelTime(lSegment.Length, lSegment.Velocity) > aMaxTravelTime + 0.001 Then
                    'need to break this segment into multiple
                    Dim lBreakNumber As Integer = Int(TravelTime(lSegment.Length, lSegment.Velocity) / aMaxTravelTime) + 1
                    'find cumulative drainage area above this segment
                    Dim lCumAbove As Double = CumulativeAreaAboveSegment(lSegment.ID)
                    'create the new pieces
                    For lBreakIndex As Integer = 2 To lBreakNumber
                        Dim lNewSegment As New atcWASPSegment(WASPConstituents.Count)
                        lNewSegment = lSegment.Clone
                        lNewSegment.ID = lSegment.ID & IntegerToAlphabet(lBreakIndex - 1)
                        If lBreakIndex < lBreakNumber Then
                            lNewSegment.DownID = lSegment.ID & IntegerToAlphabet(lBreakIndex)
                        Else
                            lNewSegment.DownID = lSegment.DownID
                        End If
                        lNewSegment.Length = lSegment.Length / lBreakNumber
                        lNewSegment.CumulativeDrainageArea = lCumAbove + ((lSegment.CumulativeDrainageArea - lCumAbove) * lBreakIndex / lBreakNumber)
                        BreakLineIntoNthPart(lSegment.PtsX, lSegment.PtsY, lBreakIndex, lBreakNumber, lNewSegment.PtsX, lNewSegment.PtsY)
                        lNewSegments.Add(lNewSegment)
                        lNewSegmentPositions.Add(lNewSegment.ID, lIndex)
                    Next
                    'reset length and id for the original segment 
                    Dim lOldID As String = lSegment.ID
                    'TODO: should this be hardcoded?
                    lSegment.ID = lOldID & "A"
                    'if this segment id shows up as a downid anywhere else, change it
                    For Each lTempSeg As atcWASPSegment In Segments
                        If lTempSeg.DownID = lOldID Then
                            lTempSeg.DownID = lSegment.ID
                        End If
                    Next
                    'TODO: should this be hardcoded?
                    lSegment.DownID = lOldID & "B"
                    lSegment.Length = lSegment.Length / lBreakNumber
                    lSegment.CumulativeDrainageArea = lCumAbove + ((lSegment.CumulativeDrainageArea - lCumAbove) / lBreakNumber)
                    Dim lPtsX(0) As Double
                    Dim lPtsY(0) As Double
                    BreakLineIntoNthPart(lSegment.PtsX, lSegment.PtsY, 1, lBreakNumber, lPtsX, lPtsY)
                    lSegment.PtsX = lPtsX
                    lSegment.PtsY = lPtsY
                End If
            End If
        Next
        'if any new segments, add them now to the segments collection
        For lIndex As Integer = lNewSegments.Count To 1 Step -1
            Segments.Insert(lNewSegmentPositions(lIndex - 1), lNewSegments(lIndex - 1))
        Next
        'because some keys have changes, clear all out and add back in
        lNewSegments.Clear()
        For Each lSegment As atcWASPSegment In Segments
            lNewSegments.Add(lSegment)
        Next
        Segments.Clear()
        Segments = lNewSegments
    End Sub

    Private Function CumulativeAreaAboveSegment(ByVal aSegmentID As String) As Double
        'find the area above this segment id
        Dim lArea As Double = 0
        With Me
            For Each lSegment As atcWASPSegment In .Segments
                If lSegment.DownID = aSegmentID Then
                    lArea += lSegment.CumulativeDrainageArea
                End If
            Next
        End With
        Return lArea
    End Function

#End Region

#Region "Mostly deprecated WDM timeseries tools..."

    ''' <summary>
    ''' Upon initialization, get list of WDM stations for loaded project
    ''' </summary>
    ''' <param name="aConstituent"></param>
    ''' <param name="aStationCandidates"></param>
    Public Sub BuildListofValidStationNames(ByRef aConstituent As String, _
                                            ByVal aStationCandidates As atcWASPTimeseriesCollection)

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            BuildListofValidStationNamesFromDataSource(lDataSource, aConstituent, aStationCandidates)
        Next

    End Sub

    'Public Sub AddSelectedTimeseriesToWASPSegment(ByVal aKeyString As String, _
    '                                                  ByRef aStationCandidates As atcWASPTimeseriesCollection, _
    '                                                  ByRef aWASPProject As atcWASPProject, _
    '                                                  ByRef aSegment As atcWASPSegment)

    '    'need to make sure this timeseries is in the class structure
    '    If aStationCandidates.Contains(aKeyString) Then
    '        If aWASPProject.InputTimeseriesCollection.Contains(aKeyString) Then
    '            'already in the project, just reference it from this segment
    '            'aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
    '        Else
    '            'not yet in the project, add it
    '            Dim lTimeseries As atcTimeseries = GetTimeseries(aStationCandidates(aKeyString).DataSourceName, aStationCandidates(aKeyString).ID)
    '            If lTimeseries Is Nothing Then
    '                Logger.Dbg("Could not find timeseries " & aKeyString)
    '            Else
    '                aStationCandidates(aKeyString).TimeSeries = lTimeseries
    '                aWASPProject.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
    '                'aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
    '            End If
    '        End If
    '    ElseIf aKeyString = "FLOW:<mean annual flow>" Then
    '        'user wants to use the mean annual flow.
    '        'the fact that there is no timeseries will be the clue that the mean annual flow should be used.
    '        Dim lStationCandidate As New atcWASPTimeseries
    '        With lStationCandidate
    '            .Type = "FLOW"
    '            .Description = "<mean annual flow> for segment " & aSegment.WaspID
    '            .SDate = 1
    '            .EDate = 100000
    '            .Identifier = aSegment.ID
    '            .DataSourceName = aSegment.MeanAnnualFlow.ToString  'kludge, but have to store this somewhere
    '        End With
    '        aWASPProject.InputTimeseriesCollection.Add(lStationCandidate)
    '        'aSegment.InputTimeseriesCollection.Add(lStationCandidate)
    '    End If
    'End Sub

    'Public Sub AddSelectedTimeseriesToWASPProject(ByVal aKeyString As String, _
    '                                              ByRef aStationCandidates As atcWASPTimeseriesCollection, _
    '                                              ByRef aWASPProject As atcWASPProject)

    '    'need to make sure this timeseries is in the class structure
    '    If aStationCandidates.Contains(aKeyString) Then
    '        If aWASPProject.InputTimeseriesCollection.Contains(aKeyString) Then
    '            'already in the project, just reference it from this segment
    '        Else
    '            'not yet in the project, add it
    '            Dim lTimeseries As atcTimeseries = GetTimeseries(aStationCandidates(aKeyString).DataSourceName, aStationCandidates(aKeyString).ID)
    '            If lTimeseries Is Nothing Then
    '                Logger.Dbg("Could not find timeseries " & aKeyString)
    '            Else
    '                aStationCandidates(aKeyString).TimeSeries = lTimeseries
    '                aWASPProject.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
    '            End If
    '        End If
    '    End If
    'End Sub

    'Public Sub RebuildTimeseriesCollections(ByVal aGridTimeSource As atcGridSource, _
    '                                        ByVal aGridFlowSource As atcGridSource, ByVal aGridLoadSource As atcGridSource, ByVal aGridBoundSource As atcGridSource)
    '    'clear out collections of timeseries prior to rebuilding
    '    InputTimeseriesCollection.Clear()
    '    For lIndex As Integer = 1 To Segments.Count
    '        Segments(lIndex - 1).InputTimeseriesCollection = New atcWASP.atcWASPTimeseriesCollection
    '    Next

    '    'build collections of timeseries 
    '    Dim lKeyString As String = ""
    '    Dim lRow As Integer = 0
    '    For Each lSegment As atcWASPSegment In Segments
    '        If IsBoundary(lSegment) Then
    '            lRow = lRow + 1
    '            'input flows 
    '            lKeyString = "FLOW:" & aGridFlowSource.CellValue(lRow, 3)
    '            If aGridFlowSource.CellValue(lRow, 3) <> "<none>" Then
    '                AddSelectedTimeseriesToWASPSegment(lKeyString, FlowStationCandidates, Me, lSegment)
    '            End If
    '        End If
    '    Next

    '    lKeyString = ""
    '    For lIndex As Integer = 1 To Segments.Count
    '        For lColumn As Integer = 1 To Me.WASPConstituents.Count
    '            'wq loads
    '            'build key string, type is the first part before the colon.
    '            Dim lColonPos As Integer = InStr(1, aGridLoadSource.CellValue(lIndex, lColumn), ":")
    '            If lColonPos > 0 Then
    '                lKeyString = Mid(aGridLoadSource.CellValue(lIndex, lColumn), 1, lColonPos) & aGridLoadSource.CellValue(lIndex, lColumn)
    '            Else
    '                lKeyString = ""
    '            End If
    '            If aGridLoadSource.CellValue(lIndex, lColumn) <> "<none>" Then
    '                If lKeyString.Length > 0 Then
    '                    AllStationCandidates(lKeyString).BoundaryOrLoad = "Load"
    '                    AllStationCandidates(lKeyString).WASPSystem = WASPConstituents(lColumn - 1)
    '                    AddSelectedTimeseriesToWASPSegment(lKeyString, AllStationCandidates, Me, Segments(lIndex - 1))
    '                End If
    '            End If
    '        Next
    '    Next

    '    'check to see if each segment is a boundary
    '    lRow = 0
    '    For Each lSegment As atcWASPSegment In Segments
    '        If IsBoundary(lSegment) Then
    '            lRow = lRow + 1
    '            For lColumn As Integer = 1 To Me.WASPConstituents.Count
    '                'boundaries
    '                'build key string, type is the first part before the colon.
    '                Dim lColonPos As Integer = InStr(1, aGridBoundSource.CellValue(lRow, lColumn), ":")
    '                If lColonPos > 0 Then
    '                    lKeyString = Mid(aGridBoundSource.CellValue(lRow, lColumn), 1, lColonPos) & aGridBoundSource.CellValue(lRow, lColumn)
    '                Else
    '                    lKeyString = ""
    '                End If
    '                If aGridBoundSource.CellValue(lRow, lColumn) <> "<none>" Then
    '                    If lKeyString.Length > 0 Then
    '                        AllStationCandidates(lKeyString).BoundaryOrLoad = "Boundary"
    '                        AllStationCandidates(lKeyString).WASPSystem = WASPConstituents(lColumn - 1)
    '                        AddSelectedTimeseriesToWASPSegment(lKeyString, AllStationCandidates, Me, lSegment)
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Next

    '    'aGridTimeSource timeseries are not segment-specific
    '    For lRow = 1 To aGridTimeSource.Rows - 1
    '        If aGridTimeSource.CellValue(lRow, 1) <> "<none>" Then
    '            'build key string, type is the first part before the colon.
    '            Dim lColonPos As Integer = InStr(1, aGridTimeSource.CellValue(lRow, 1), ":")
    '            If lColonPos > 0 Then
    '                lKeyString = Mid(aGridTimeSource.CellValue(lRow, 1), 1, lColonPos) & aGridTimeSource.CellValue(lRow, 1)
    '                AllStationCandidates(lKeyString).BoundaryOrLoad = "TimeFunction"
    '                AllStationCandidates(lKeyString).WASPSystem = WASPTimeFunctions(lRow - 1)
    '                AddSelectedTimeseriesToWASPProject(lKeyString, AllStationCandidates, Me)
    '            End If
    '        End If
    '    Next
    'End Sub

#End Region

#Region "Retrieve Met Station coordinates..."

    Public Sub GetMetStationCoordinates(ByVal aMetLayerIndex As Integer, ByVal aStationCandidates As atcWASPTimeseriesCollection)
        Dim lFieldIndex As Integer = 1
        If GisUtil.IsField(aMetLayerIndex, "LOCATION") Then
            lFieldIndex = GisUtil.FieldIndex(aMetLayerIndex, "LOCATION")
        End If

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(aMetLayerIndex) - 1
            Dim lTempID As String = GisUtil.FieldValue(aMetLayerIndex, lFeatureIndex, lFieldIndex)
            For Each lStationCandidate As atcWASPTimeseries In aStationCandidates
                If lStationCandidate.Identifier = lTempID Then
                    'found a timeseries, add the coordinates
                    GisUtil.PointXY(aMetLayerIndex, lFeatureIndex, lStationCandidate.LocationX, lStationCandidate.LocationY)
                End If
            Next
        Next
    End Sub

    Public Sub DefaultClosestMetStation(ByRef aAirIndex As Integer, ByRef aSolarIndex As Integer, ByRef aWindIndex As Integer)
        'default met stations based on distance
        Dim lXSum As Double = 0
        Dim lYSum As Double = 0
        For Each lSegment As atcWASPSegment In Segments
            'find average segment centroid 
            lXSum = lXSum + lSegment.CentroidX
            lYSum = lYSum + lSegment.CentroidY
        Next
        Dim lXAvg As Double = 0
        Dim lYAvg As Double = 0
        If Segments.Count > 0 Then
            lXAvg = lXSum / Segments.Count
            lYAvg = lYSum / Segments.Count
            aAirIndex = FindClosestMetStation(AllStationCandidates, lXAvg, lYAvg, "ATEM")
            aSolarIndex = FindClosestMetStation(AllStationCandidates, lXAvg, lYAvg, "SOLR")
            aWindIndex = FindClosestMetStation(AllStationCandidates, lXAvg, lYAvg, "WIND")
        Else
            aAirIndex = 0
            aSolarIndex = 0
            aWindIndex = 0
        End If
    End Sub

    Private Function FindClosestMetStation(ByVal aStationList As atcWASPTimeseriesCollection, ByVal aXAvg As Double, ByVal aYAvg As Double, ByVal aConstit As String) As Integer
        'for each valid value, find distance
        Dim lShortestDistance As Double = 1.0E+28
        Dim lDistance As Double = 0.0
        Dim lClosestIndex As Integer = 0
        Dim lStationIndex As Integer = 0
        For Each lStationCandidate As atcWASPTimeseries In aStationList
            lStationIndex += 1
            If lStationCandidate.Type = aConstit Then
                lDistance = CalculateDistance(aXAvg, aYAvg, lStationCandidate.LocationX, lStationCandidate.LocationY)
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestIndex = lStationIndex
                End If
            End If
        Next
        Return lClosestIndex
    End Function

#End Region

#Region "Wasp INP file writing routines..."

    ''' <summary>
    ''' Write Wasp INP file that contains all input data
    ''' </summary>
    ''' <param name="aFileName">Name of INP file to write</param>
    Public Function WriteINP(ByVal aFileName As String) As Boolean
        Dim lSW As IO.StreamWriter = Nothing

        Try
            'set inp file name
            INPFileName = aFileName
            WriteErrors = ""

            'Overwrite .inp file if it exists

            lSW = New IO.StreamWriter(INPFileName, False)

            Return writeInpIntro(lSW) AndAlso _
                   writeInpSegs(lSW) AndAlso _
                   writeInpPath(lSW) AndAlso _
                   writeInpFlowFile(lSW) AndAlso _
                   writeInpDispFile(lSW) AndAlso _
                   writeInpBoundFile(lSW) AndAlso _
                   writeInpLoadFile(lSW) AndAlso _
                   writeInpTFuncFile(lSW) AndAlso _
                   writeInpParamInfoFile(lSW) AndAlso _
                   writeInpConstFile(lSW) AndAlso _
                   writeInpIcFile(lSW)

        Catch ex As Exception
            Throw
            Return False
        Finally
            'final flush and close it
            If lSW IsNot Nothing Then
                lSW.Flush()
                lSW.Close()
            End If
        End Try
    End Function

    Private Function writeInpIntro(ByRef aSW As IO.StreamWriter) As Boolean

        'Dim lStartDateString As String = SDate.Month.ToString.PadLeft(5) & SDate.Day.ToString.PadLeft(5) & SDate.Year.ToString.PadLeft(5)
        Dim lJulianEnd As String = EDate.Subtract(SDate).TotalDays.ToString.PadLeft(10)

        aSW.WriteLine("{0,5}{1,7}               Module type               SYSFILE", ModelType, Version)
        'aSW.WriteLine(lStartDateString & "    0    0    0     Start date and time")
        'aSW.WriteLine(lStartDateString & "    0    0    0     Skip date and time")
        aSW.WriteLine("{0,5:MM}{0,5:dd}{0,5:yyyy}    0    0    0     Start date and time", SDate)
        aSW.WriteLine("{0,5:MM}{0,5:dd}{0,5:yyyy}    0    0    0     Skip date and time", SDate)
        aSW.WriteLine("{0,10:0.00}          Julian end time", EDate.Subtract(SDate).TotalDays)
        aSW.WriteLine("{0,5}               Number of Systems", WASPConstituents.Count)
        aSW.WriteLine("    0               Mass Balance Table Output")
        aSW.WriteLine("    1               Solution Technique Option")
        aSW.WriteLine("    0               Negative Solution Option")
        aSW.WriteLine("    0               Restart Option")
        aSW.WriteLine("    1               Time Optimization Option")
        aSW.WriteLine("    0               WQ Module Linkage Option")
        aSW.WriteLine("    0.9000          TOPT Factor")
        aSW.WriteLine("0.003000  1.000     Min and Max Timestep")
        aSW.WriteLine("    2               Number print intervals")
        aSW.WriteLine("      0.00  1.000   Time and Print Interval")
        aSW.WriteLine(lJulianEnd & "  1.000   Time and Print Interval")
        Dim lTemp As String = " "
        For lIndex As Integer = 1 To Me.WASPConstituents.Count
            lTemp = lTemp & "  0"
        Next
        aSW.WriteLine(lTemp)
        aSW.WriteLine("    0               Number output variables")
        aSW.WriteLine("    0               Number csv files")   'new entry in INP file per Tim Wool for version 1.2
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpSegs(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine(Segments.Count.ToString.PadLeft(5) & "               Number of Segments                            SEGFILE")
        aSW.WriteLine("    0               Bed Volume Option")
        aSW.WriteLine("     0.000          Bed Compaction Time Step")
        aSW.WriteLine("   1.000   1.000    Volume Scale & Conversion Factor")
        aSW.WriteLine("Segment   SegName")
        'Write out the segment id number and their names in format: FORMAT(I5,5X,A40)
        For i As Integer = 1 To Segments.Count
            For Each lSegment As atcWASPSegment In Segments
                With lSegment
                    If .WaspID = i Then
                        aSW.WriteLine("{0,5}     {1,-40}", .WaspID, .Name)
                        Exit For
                    End If
                End With
            Next
        Next

        'Write out segment geometry information
        aSW.WriteLine("   Segment    BotSeg     iType    Volume     VMult      Vexp     DMult      Dexp    Length     Slope     Width     Rough  Depth_Q0")

        'print out segments in order of ID
        Dim lsegParams(12) As String
        For i As Integer = 1 To Segments.Count
            For Each lSegment As atcWASPSegment In Segments
                With lSegment
                    If .WaspID = i Then
                        Dim vol As Double = .Length * 1000.0 * .Width * .Depth * 0.5 ' crude assumption for Volume in m3
                        aSW.WriteLine("{0,10}{1,10}{2,10}{3,10:0.00}{4,10:0.000000}{5,10:0.000000}{6,10:0.000000}" & _
                                      "{7,10:0.000000}{8,10:0.00}{9,10:0.00000}{10,10:0.0000}{11,10:0.0000}{12,10:0.0000}", _
                                      .WaspID, 0, 1, vol, 0, 0, .Depth, _
                                      0, .Length * 1000, .Slope, .Width, .Roughness, 0.001)
                        Exit For
                    End If
                End With
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function UpstreamKey(ByVal aSeg As Integer) As String
        Dim lUpstreamKey As String = String.Empty
        Dim ltargetSegID As String = Segments.Item(aSeg).ID
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = ltargetSegID Then
                lUpstreamKey = lSegment.ID
                Exit For
            End If
        Next
        Return lUpstreamKey
    End Function

    Private Function GenerateFlowPaths() As Generic.Dictionary(Of Integer, String)
        FlowPathList.Clear() 'start anew
        Dim lflowpaths As New Generic.Dictionary(Of Integer, String)

        'Build a list of headwater segments
        Dim lHeadwatersIndexes As New List(Of Integer)
        For i As Integer = 0 To Segments.Count - 1
            If UpstreamKey(i) = String.Empty Then
                lHeadwatersIndexes.Add(i + 1)
            End If
        Next

        'sort the list in assending order
        lHeadwatersIndexes.Sort() ' assuming by default it is in ascending order

        'Set up a collection to hold the from-to pairs
        Dim ldoneFlowpaths As New List(Of String)
        Dim lProblem As String = String.Empty
        Dim loutletSegID As String = Segments.DownstreamKey(lProblem)
        Dim loutletSegWASPID As Integer
        If lProblem = String.Empty Then ' getting the outlet seg succeed
            loutletSegWASPID = Segments.Item(loutletSegID).WaspID
        Else
            loutletSegWASPID = 1
        End If

        'Construct the main flowpath
        Dim lnumFlowFunc As Integer = 0
        Dim lnumflowroutes As Integer = 0
        Dim lflowfraction As String = "1.00"

        Dim lthisFlowfunction As New System.Text.StringBuilder
        lthisFlowfunction.AppendLine(Space(2) & Segments.Item(lHeadwatersIndexes(0) - 1).Name)
        Dim ltemp As New System.Text.StringBuilder
        ltemp.AppendLine("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(0) - 1).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
        ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(0) - 1).WaspID.ToString.PadLeft(4))
        lnumflowroutes += 1

        Dim lend As Boolean = False
        Dim lthisSeg As atcWASPSegment = Segments.Item(lHeadwatersIndexes(0) - 1)
        Dim lthisPair As String = String.Empty
        While Not lend
            If lthisSeg.ID = loutletSegID Then
                lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4)
                ltemp.Append(lthisPair & Space(11) & lflowfraction)
                ldoneFlowpaths.Add(lthisPair)
                lnumflowroutes += 1
                lend = True
            Else
                Dim ldownID As String = lthisSeg.DownID
                lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4)
                ltemp.AppendLine(lthisPair & Space(11) & lflowfraction)
                lthisSeg = Segments.Item(ldownID)
                ldoneFlowpaths.Add(lthisPair)
                lnumflowroutes += 1
                lend = False
            End If
        End While

        If Not ltemp.ToString = "" Then
            'Increment the total flow function count
            'write up routes count and info
            'clear the ltemp content for subsequent flow functions, reset lnumflowroutes
            'Add this flow function to the overall list
            lnumFlowFunc += 1
            lthisFlowfunction.AppendLine(Space(3) & lnumflowroutes.ToString)
            lthisFlowfunction.Append(ltemp.ToString)
            lflowpaths.Add(lHeadwatersIndexes(0), lthisFlowfunction.ToString)
            FlowPathList.Add(lHeadwatersIndexes(0) - 1)
            lnumflowroutes = 0
            ltemp = New System.Text.StringBuilder
        End If

        'Do the rest of the headwaters
        For i As Integer = 1 To lHeadwatersIndexes.Count - 1
            lend = False
            lthisSeg = Segments.Item(lHeadwatersIndexes(i) - 1)
            ltemp.Append("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
            ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WaspID.ToString.PadLeft(4))
            lnumflowroutes += 1
            lthisFlowfunction = New System.Text.StringBuilder
            While Not lend
                If lthisSeg.ID = loutletSegID Then
                    lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4)
                    If ldoneFlowpaths.Contains(lthisPair) Then
                        lend = True
                        Continue While
                    Else
                        ldoneFlowpaths.Add(lthisPair)
                    End If
                    ltemp.Append(lthisPair & Space(11) & lflowfraction)
                    ldoneFlowpaths.Add(lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4))
                    lnumflowroutes += 1
                    lend = True
                Else
                    Dim ldownID As String = lthisSeg.DownID
                    lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4)
                    If ldoneFlowpaths.Contains(lthisPair) Then
                        lend = True
                        Continue While
                    Else
                        ldoneFlowpaths.Add(lthisPair)
                    End If
                    ltemp.Append(vbCrLf)
                    ltemp.Append(lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
                    lnumflowroutes += 1
                    lthisSeg = Segments.Item(ldownID)
                    lend = False
                End If
            End While

            If Not ltemp.ToString = "" OrElse Not lnumflowroutes = 0 Then
                'Increment the total flow function count
                'write up routes count and info
                'clear the ltemp content for subsequent flow functions
                'Add this flow function to the overall list
                lnumFlowFunc += 1
                Dim lPathName As String = Segments.Item(lHeadwatersIndexes(i) - 1).Name
                If IsNumeric(lPathName) Then
                    lPathName = "FlowPath " & lnumFlowFunc.ToString
                End If
                lthisFlowfunction.AppendLine(Space(2) & lPathName)
                lthisFlowfunction.AppendLine(Space(3) & lnumflowroutes.ToString)
                lthisFlowfunction.Append(ltemp.ToString)
                lflowpaths.Add(lHeadwatersIndexes(i), lthisFlowfunction.ToString)
                FlowPathList.Add(lHeadwatersIndexes(i) - 1)
                lnumflowroutes = 0
                ltemp = New System.Text.StringBuilder
            End If
        Next
        NumFlowFunc(0) = lnumFlowFunc ' The first flow field is always for water flow
        Return lflowpaths
    End Function

    Private Function writeInpPath(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: flow field information, need interface

        aSW.WriteLine("{0,5}               Flow Pathways                                 PATHFILE", 4)
        aSW.WriteLine("{0,5}               Number of flow fields", 6) ' this can be hardcoded here as only 6 constant fields
        aSW.WriteLine("     Flow Field   1")

        'Figure out the flowpaths:
        'this flow path is the base flow network that is used later for other things
        Dim lflowfuncfield1 As Generic.Dictionary(Of Integer, String)
        lflowfuncfield1 = GenerateFlowPaths()
        aSW.WriteLine("{0,5}               Number of Flow Functions for Flow Field", lflowfuncfield1.Count)
        For Each lString As String In lflowfuncfield1.Values
            aSW.WriteLine(lString)
        Next

        'For now the 2-6 fields' flow func can be hard-coded here, later done by functions
        For lflowField As Integer = 2 To 6
            aSW.WriteLine("     Flow Field   {0}", lflowField.ToString)
            aSW.WriteLine("{0,5}               Number of Flow Functions for Flow Field", 0)
            NumFlowFunc(lflowField - 1) = 0 'TODO: NumFlowFunc when this is done dynamically, this needs to be changed
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpFlowFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Flow Fields                          FLOWFILE", 6)
        Dim linflowCat(5) As String
        linflowCat(0) = " Surface Water Flow Field      "
        linflowCat(1) = " Porewater Flow Field          "
        linflowCat(2) = " Solids - 1                    "
        linflowCat(3) = " Solids - 2                    "
        linflowCat(4) = " Solids - 3                    "
        linflowCat(5) = " Evap/Precip Flow Field        "

        For i As Integer = 0 To 5  ' Loop through the 6 flow fields
            aSW.WriteLine(linflowCat(i))
            'Assuming inflow time-value correspond to the set up for flowpath
            'so 'NumFlowFunc' array can be used for these different, yet related, sections
            aSW.WriteLine("{0,5}               Number of inflows", NumFlowFunc(i))
            If NumFlowFunc(i) = 0 Then
                Continue For
            End If
            aSW.WriteLine("   1.000   1.000    Flow Scale & Conversion Factors")

            For j As Integer = 0 To FlowPathList.Count - 1
                Dim seg As atcWASPSegment = Segments.Item(FlowPathList(j))
                aSW.WriteLine(seg.Name.PadRight(30))
                With seg.FlowTimeSeries.GetTimeSeries(Me, seg.ID_Name, "Input Flows")
                    If seg.FlowTimeSeries.SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                        WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", seg.Name, "flow", seg.FlowTimeSeries.ToFullString) & vbCr
                    End If
                    aSW.WriteLine("{0,5}               Number of time-flow values in {1}", .Count, seg.FlowTimeSeries.ToFullString)
                    For t As Integer = 0 To .Count - 1
                        aSW.WriteLine(String.Format("{0,8:0.000} {1,9:0.00000}", .Keys(t).Subtract(SDate).TotalDays, .Values(t)))
                    Next
                End With
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpDispFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Exchange Fields                     DISPFILE", 0)
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpBoundFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Systems                             BOUNDFILE", WASPConstituents.Count)
        'figure out the number of boundaries
        Dim NumBound As Integer = 0
        For Each lSegment As atcWASPSegment In Segments
            If IsBoundary(lSegment) Then
                NumBound += 1
            End If
        Next
        For c As Integer = 0 To WASPConstituents.Count - 1
            aSW.WriteLine("  {0,-30}", WASPConstituents(c).Description)
            aSW.WriteLine("{0,5}               Number of boundaries", NumBound)
            aSW.WriteLine("   1.000   1.000    Boundary Scale & Conversion Factors")
            For i As Integer = 1 To Segments.Count
                For Each Seg As atcWASPSegment In Segments
                    With Seg
                        If .WaspID = i Then
                            If IsBoundary(Seg) Then
                                aSW.WriteLine("{0,5}               Boundary Segment Number", .WaspID)
                                With Seg.BoundTimeSeries(c).GetTimeSeries(Me, Seg.ID_Name, WASPConstituents(c).Description)
                                    If Seg.BoundTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                                        WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", Seg.Name, WASPConstituents(c), Seg.BoundTimeSeries(c).ToFullString) & vbCr
                                    End If
                                    If .Count = 0 Then
                                        aSW.WriteLine("{0,5}               Number of time-concentration values in {1}", 2, "--Empty Time Series--")
                                        aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", 0, 0)
                                        aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", EDate.Subtract(SDate).TotalDays, 0)
                                    Else
                                        aSW.WriteLine("{0,5}               Number of time-concentration values in {1}", .Count, Seg.FlowTimeSeries.ToFullString)
                                        For t As Integer = 0 To .Count - 1
                                            aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t).Subtract(SDate).TotalDays, .Values(t))
                                        Next
                                    End If
                                End With
                                Exit For
                                'Dim lBoundTS As atcWASPTimeseries = Nothing
                                'For Each lts As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                                '    If lts.BoundaryOrLoad = "Boundary" And lts.WASPSystem = lCons Then
                                '        lBoundTS = lts
                                '        Exit For
                                '    End If
                                'Next
                                'If Not lBoundTS Is Nothing Then
                                '    'have a full timeseries to write
                                '    Dim lValCount As Integer = 0
                                '    For lindex As Integer = 1 To lBoundTS.TimeSeries.Values.Length - 1
                                '        If lBoundTS.TimeSeries.Dates.Values(lindex) >= SJDate And lBoundTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                '            lValCount = lValCount + 1
                                '        End If
                                '    Next
                                '    aSW.WriteLine(Space(2) & lValCount & Space(15) & "Number of time-concentration values")
                                '    For lindex As Integer = 1 To lBoundTS.TimeSeries.Values.Length - 1
                                '        If lBoundTS.TimeSeries.Dates.Values(lindex) >= SJDate And lBoundTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                '            aSW.WriteLine(Space(3) & String.Format("{0:0.000}", lBoundTS.TimeSeries.Dates.Values(lindex) - SJDate) & Space(2) & String.Format("{0:0.000}", lBoundTS.TimeSeries.Values(lindex)))
                                '        End If
                                '    Next
                                'Else
                                '    Dim lJulianEnd As String = String.Format("{0:0.000}", EJDate - SJDate).PadLeft(10)
                                '    aSW.WriteLine("    2               Number of time-concentration values")
                                '    aSW.WriteLine("     0.000" & "  " & String.Format("{0:0.0000}", 0.0))
                                '    aSW.WriteLine(lJulianEnd & "  " & String.Format("{0:0.0000}", 0.0))
                                'End If
                            End If
                        End If
                    End With
                Next
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpLoadFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               NPS Input Option (0=No, 1=Yes)                LOADFILE", 0)
        aSW.WriteLine("{0,5}               Number of Systems", WASPConstituents.Count)
        For c As Integer = 0 To WASPConstituents.Count - 1
            aSW.WriteLine("  {0,-30}", WASPConstituents(c).Description)
            'figure out the number of loadings
            Dim NumLoads As Integer = 0
            For Each Seg As atcWASPSegment In Segments
                If Seg.LoadTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then NumLoads += 1
            Next
            aSW.WriteLine("{0,5}               Number of Loadings", NumLoads)
            If NumLoads > 0 Then
                aSW.WriteLine("   1.000   1.000    Loading Scale & Conversion Factors")
                For s As Integer = 0 To Segments.Count - 1
                    For Each Seg As atcWASPSegment In Segments
                        With Seg
                            If .WaspID = s Then
                                aSW.WriteLine("{0,5}               Loading Segment Number", .WaspID)
                                With Seg.LoadTimeSeries(c).GetTimeSeries(Me, Seg.ID_Name, WASPConstituents(c).Description)
                                    If Seg.LoadTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                                        WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", Seg.Name, WASPConstituents(c), Seg.LoadTimeSeries(c).ToFullString) & vbCr
                                    End If
                                    aSW.WriteLine("{0,5}               Number of time-loading values in {1}", .Count, Seg.FlowTimeSeries.ToFullString)
                                    For t As Integer = 0 To .Count - 1
                                        aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t).Subtract(SDate).TotalDays, .Values(t))
                                    Next
                                End With
                            End If
                        End With
                    Next
                Next
            End If
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpTFuncFile(ByRef aSW As IO.StreamWriter) As Boolean
        Dim NumTimeFunc As Integer = 0
        For i As Integer = 0 To WASPTimeFunctions.Count - 1
            If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then NumTimeFunc += 1
        Next

        aSW.WriteLine("{0,5}               Number of Time Functions                      TFUNCFILE", NumTimeFunc)

        For i As Integer = 0 To WASPTimeFunctions.Count - 1
            With WASPTimeFunctions(i)
                If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then
                    aSW.WriteLine("{0,5}                Time Function ID Number", .FunctionID)
                    aSW.WriteLine("  {0}", .Description)  'use to write time function name
                    With TimeFunctionSeries(i).GetTimeSeries(Me, "Time Functions", .Description)
                        If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                            WriteErrors &= String.Format("Empty time series was returned for {0}; specification was: {1}", WASPTimeFunctions(i).Description, TimeFunctionSeries(i).ToFullString) & vbCr
                        End If
                        aSW.WriteLine("{0,5}               Number of time-function values in {1}", .Count, TimeFunctionSeries(i).ToFullString)
                        For t As Integer = 0 To .Count - 1
                            aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t).Subtract(SDate).TotalDays, .Values(t))
                        Next
                    End With
                End If
            End With
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpParamInfoFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Segments                             PARAMINFO", Segments.Count)
        aSW.WriteLine("{0,5}               Number of Segment Parameters", 0)
        aSW.Flush()
        Return True

        'Need a structure in the classes to hold the parameter list
        'so HC here
        'aSW.WriteLine("    2               Number of Segment Parameters")
        'aSW.WriteLine("    9       1.000000     ID and scale factor for: Sediment Oxygen Demand (g/m2/day)")
        'aSW.WriteLine("   12       1.000000     ID and scale factor for: Sediment Oxygen Demand Temperature Correction Factor")

        'For i As Integer = 0 To Segments.Count - 1
        '    aSW.WriteLine((i + 1).ToString.PadLeft(5) & Space(15) & "Segment number")
        '    aSW.WriteLine("   9  1.00000")
        '    aSW.WriteLine("   12  1.08000")
        'Next
        'aSW.Flush()
        'Return True
    End Function

    Private Function writeInpConstFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Constants                            CONSTFILE", 0)
        aSW.Flush()
        Return True

        'Need a structure to hold the constant info
        'HC here
        'aSW.WriteLine("   32               Number of Constants                           CONSTFILE")
        'aSW.WriteLine("   11  0.400000 Nitrification Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   12  1.04000 Nitrification Temperature Coefficient")
        'aSW.WriteLine("   13  2.00000 Half Saturation Constant for Nitrification Oxygen Limit (mg O/L)")
        'aSW.WriteLine("   14  4.00000 Minimum Temperature for Nitrification Reaction, deg C")
        'aSW.WriteLine("   21  0.690000 Denitrification Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   22  1.04000 Denitrification Temperature Coefficient")
        'aSW.WriteLine("   23  1.00000 Half Saturation Constant for Denitrification Oxygen Limit (mg O/L)")
        'aSW.WriteLine("   91  0.100000 Dissolved Organic Nitrogen Mineralization Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   92  1.07000 Dissolved Organic Nitrogen Mineralization Temperature Coefficient")
        'aSW.WriteLine("   100  0.180000 Mineralization Rate Constant for Dissolved Organic P @20 °C (per day)")
        'aSW.WriteLine("   101  1.07000 Dissolved Organic Phosphorus Mineralization Temperature Coefficient")
        'aSW.WriteLine("   41  3.00000 Phytoplankton Maximum Growth Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   42  1.05000 Phytoplankton Growth Temperature Coefficient")
        'aSW.WriteLine("   46  50.0000 Phytoplankton Carbon to Chlorophyll Ratio")
        'aSW.WriteLine("   48  5.000000E-02 Phytoplankton Half-Saturation Constant for Nitrogen Uptake (mg N/L)")
        'aSW.WriteLine("   49  5.000000E-03 Phytoplankton Half-Saturation Constant for Phosphorus Uptake (mg P/L)")
        'aSW.WriteLine("   50  0.100000 Phytoplankton Endogenous Respiration Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   51  1.07000 Phytoplankton Respiration Temperature Coefficient")
        'aSW.WriteLine("   52  0.000000 Phytoplankton Death Rate Constant (Non-Zooplankton Predation) (per day)")
        'aSW.WriteLine("   57  0.240000 Phytoplankton Phosphorus to Carbon Ratio")
        'aSW.WriteLine("   58  0.430000 Phytoplankton Nitrogen to Carbon Ratio")
        'aSW.WriteLine("   43  1.00000 Light Option (1 uses input light;  2 uses calculated diel light)")
        'aSW.WriteLine("   85  4.00000 Calc Reaeration Option (0=Covar, 1=O'Connor, 2=Owens, 3=Churchill, 4=Tsivoglou)")
        'aSW.WriteLine("   86  1.000000E-02 Minimum Reaeration Rate, per day")
        'aSW.WriteLine("   87  1.04700 Theta -- Reaeration Temperature Correction")
        'aSW.WriteLine("   81  2.67000 Oxygen to Carbon Stoichiometric Ratio")
        'aSW.WriteLine("   71  0.400000 BOD (1) Decay Rate Constant @20 °C (per day)")
        'aSW.WriteLine("   72  1.04700 BOD (1) Decay Rate Temperature Correction Coefficient")
        'aSW.WriteLine("   76  1.00000 BOD (2) Decay Rate @20 °C (per day)")
        'aSW.WriteLine("   77  1.04700 BOD (2) Decay Rate Temperature Correction Coefficient")
        'aSW.WriteLine("   131  0.200000 Detritus Dissolution Rate (1/day)")
        'aSW.WriteLine("   132  1.04700 Temperature Correction for detritus dissolution")
        'aSW.Flush()
        'Return True
    End Function

    Private Function writeInpIcFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Systems                             ICFILE", WASPConstituents.Count)
        aSW.WriteLine("{0,5}               Number of Segments", Segments.Count)
        For lIndex As Integer = 1 To WASPConstituents.Count
            aSW.WriteLine("     Initial conditions for system" & lIndex.ToString.PadLeft(3) & " " & WASPConstituents(lIndex - 1).Description)
            aSW.WriteLine("    0               Solids Transport Field")
            aSW.WriteLine("     1.000          Solids Density, g/mL")
            aSW.WriteLine("     10000.0000     Maximum Allowed Concentration")
            aSW.WriteLine("  Seg   Conc   DissF")
            For lSegIndex As Integer = 1 To Segments.Count
                aSW.WriteLine("   {0}  0.000000  1.00000", lSegIndex)
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    ''' <summary>
    ''' Launch Wasp 8 program
    ''' </summary>
    ''' <param name="aInputFileName">Name of INP file to import upon running WASP</param>
    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lWASPDir As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\USEPA\WASP\7.0", "DatabaseDir", "") & "..\wasp.exe"
            Dim lWASPexe As String = atcUtility.FindFile("Please locate the EPA WASP Executable", lWASPDir)
            If IO.File.Exists(lWASPexe) Then
                Process.Start(lWASPexe, "-import " & """" & aInputFileName & """")
                Logger.Dbg("WASP launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA WASP Executable", MsgBoxStyle.Critical, "BASINS WASP Problem")
            End If
        Else
            Logger.Msg("Cannot find WASP Input File " & aInputFileName)
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Return 
    ''' </summary>
    ''' <param name="aPathName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWASPModels(ByVal aPathName As String) As Generic.List(Of clsWASPModel)
        Dim con As SQLite.SQLiteConnection = Nothing
        Dim cmd As SQLite.SQLiteCommand = Nothing
        Dim dr As SQLite.SQLiteDataReader = Nothing

        Dim lFileName As String = aPathName & "\epa.sqlite"
        If Not My.Computer.FileSystem.FileExists(lFileName) Then
            lFileName = atcUtility.FindFile("Please locate the EPA WASP database file", lFileName)
            If Not My.Computer.FileSystem.FileExists(lFileName) Then Throw New System.IO.FileNotFoundException("WASP database file not found: " & lFileName)
        End If

        'open database connection
        Dim csb As New SQLite.SQLiteConnectionStringBuilder
        With csb
            .DataSource = lFileName
        End With
        con = New SQLite.SQLiteConnection(csb.ConnectionString)
        con.Open()

        Dim lstWASPModels As New Generic.List(Of clsWASPModel)

        cmd = New SQLite.SQLiteCommand("SELECT Model_name, Model_ID FROM models ORDER BY Sort_key", con)
        dr = cmd.ExecuteReader()
        While dr IsNot Nothing AndAlso dr.Read
            lstWASPModels.Add(New clsWASPModel(dr.GetString(0), TestNull(dr.GetValue(1), 0)))
        End While
        dr.Close()
        Return lstWASPModels
    End Function

    ''' <summary>
    ''' Given model type, retrieve associated constituents, functions, etc.
    ''' </summary>
    ''' <param name="aPathName">Path to SQLite database (no trailing slash)</param>
    ''' <param name="ModelName">Name of WASP model selected</param>
    Public Sub ReadWASPdb(ByVal aPathName As String, ByVal ModelName As String)
        Dim con As SQLite.SQLiteConnection = Nothing
        Dim cmd As SQLite.SQLiteCommand = Nothing
        Dim dr As SQLite.SQLiteDataReader = Nothing

        Try
            'LCW 11/28/10: read this information from WASP SQLite database tables found in the install directory rather than csv files

            Dim lFileName As String = aPathName & "\epa.sqlite"
            If Not My.Computer.FileSystem.FileExists(lFileName) Then
                lFileName = atcUtility.FindFile("Please locate the EPA WASP database file", lFileName)
                If Not My.Computer.FileSystem.FileExists(lFileName) Then Throw New System.IO.FileNotFoundException("WASP database file not found: " & lFileName)
            End If

            'open database connection
            Dim csb As New SQLite.SQLiteConnectionStringBuilder
            With csb
                .DataSource = lFileName
            End With
            con = New SQLite.SQLiteConnection(csb.ConnectionString)
            con.Open()

            WASPConstituents = New Generic.List(Of clsWASPConstituent)

            cmd = New SQLite.SQLiteCommand(String.Format("SELECT Description,Conc_Units,Load_Units FROM models,systems " & _
                                                         "WHERE models.Model_id=systems.Model_id AND Model_name='{0}' ORDER BY systems.Sort_key", ModelName), con)
            dr = cmd.ExecuteReader()
            While dr IsNot Nothing AndAlso dr.Read
                WASPConstituents.Add(New clsWASPConstituent(dr.GetString(0), dr.GetString(1), dr.GetString(2)))
            End While
            dr.Close()

            WASPTimeFunctions = New Generic.List(Of clsWASPTimeFunction)

            cmd = New SQLite.SQLiteCommand(String.Format("SELECT Description,Time_function_id FROM models,time_functions " & _
                                                         "WHERE models.Model_id=time_functions.Model_id AND Model_name='{0}' ORDER BY time_functions.Sort_key", ModelName), con)
            dr = cmd.ExecuteReader()
            While dr IsNot Nothing AndAlso dr.Read
                WASPTimeFunctions.Add(New clsWASPTimeFunction(dr.GetString(0), TestNull(dr.GetValue(1), 0)))
            End While
            dr.Close()

            'initialize time series for each segment and all time functions

            For i As Integer = 0 To Segments.Count - 1
                With Segments(i)
                    .FlowTimeSeries = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                    ReDim .LoadTimeSeries(WASPConstituents.Count - 1)
                    ReDim .BoundTimeSeries(WASPConstituents.Count - 1)
                    For j As Integer = 0 To WASPConstituents.Count - 1
                        .LoadTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                        .BoundTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                    Next
                End With
            Next

            ReDim TimeFunctionSeries(WASPTimeFunctions.Count - 1)

            For i As Integer = 0 To WASPTimeFunctions.Count - 1
                TimeFunctionSeries(i) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            If con IsNot Nothing Then con.Close() : con.Dispose()
            If cmd IsNot Nothing Then cmd.Dispose()
            If dr IsNot Nothing Then dr.Close()
        End Try
    End Sub

End Class

''' <summary>
''' Class to hold contents of "Systems" table in WASP database which identifies all constituents for each model type
''' Use in dictionary and select by ModelName
''' </summary>
Public Class clsWASPModel
    Public Description As String
    Public ModelID As String
    Sub New(ByVal aDescription As String, ByVal aModelID As String)
        Description = aDescription
        ModelID = aModelID
    End Sub
End Class

''' <summary>
''' Class to hold contents of "Systems" table in WASP database which identifies all constituents for each model type
''' Use in dictionary and select by ModelName
''' </summary>
Public Class clsWASPConstituent
    Public Description As String
    Public ConcUnits As String
    Public LoadUnits As String
    Sub New(ByVal aDescription As String, ByVal aConcUnits As String, ByVal aLoadUnits As String)
        Description = aDescription
        ConcUnits = aConcUnits
        LoadUnits = aLoadUnits
    End Sub
End Class

''' <summary>
''' Class to hold contents of "Time_Functions" table in WASP database which identifies all time functions for each model type
''' Use in dictionary and select by ModelName
''' </summary>
Public Class clsWASPTimeFunction
    Public Description As String
    Public FunctionID As Integer
    Sub New(ByVal aDescription As String, ByVal aFunctionID As Integer)
        Description = aDescription
        FunctionID = aFunctionID
    End Sub
End Class

Public Class clsPCodeMapping
    Public PCode As String
    Public ConversionType As clsTimeSeriesSelection.enumConversion
    Public ScaleFactor As Double
    Sub New(ByVal aPCode As String, ByVal aConversionType As clsTimeSeriesSelection.enumConversion, ByVal aScaleFactor As Double)
        PCode = aPCode
        ConversionType = aConversionType
        ScaleFactor = aScaleFactor
    End Sub
    Sub New(ByVal aPCode As String, ByVal aConversionName As String, ByVal aScaleFactor As Double)
        PCode = aPCode
        ConversionType = clsTimeSeriesSelection.GetConversionType(aConversionName)
        ScaleFactor = aScaleFactor
    End Sub
End Class