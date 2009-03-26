Imports MapWinUtility
Imports System.Text
Imports atcUtility
Imports atcData
Imports atcControls
Imports atcMwGisUtility

Public Class atcWASPProject
    Public Segments As atcWASPSegments
    Public InputTimeseriesCollection As atcWASPTimeseriesCollection
    Public SJDate As Double = 0.0
    Public EJDate As Double = 0.0
    Public Name As String = ""
    Public WNFFileName As String = ""
    Public SegmentFieldMap As New atcCollection

    Public FlowStationCandidates As New atcWASPTimeseriesCollection
    Public AirTempStationCandidates As New atcWASPTimeseriesCollection
    Public SolRadStationCandidates As New atcWASPTimeseriesCollection
    Public WindStationCandidates As New atcWASPTimeseriesCollection
    Public WaterTempStationCandidates As New atcWASPTimeseriesCollection

    Public Sub New()
        Name = ""
        WNFFileName = ""
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

        InputTimeseriesCollection = New atcWASPTimeseriesCollection
    End Sub

    Public Function Save(ByVal aFileName As String) As Boolean
        'set file names
        WNFFileName = aFileName
        Dim lSegmentFileName As String = FilenameSetExt(WNFFileName, "seg")
        Dim lDirectoryFileName As String = FilenameSetExt(WNFFileName, "tim")

        Dim lSDate(6) As Integer
        J2Date(SJDate, lSDate)
        Dim lEDate(6) As Integer
        J2Date(EJDate, lEDate)
        Dim lStartDateString As String = lSDate(1) & "/" & lSDate(2) & "/" & lSDate(0)
        Dim lEndDateString As String = lEDate(1) & "/" & lEDate(2) & "/" & lEDate(0)

        'write WASP network file first
        Dim lSW As New IO.StreamWriter(aFileName)
        lSW.WriteLine(lStartDateString)
        lSW.WriteLine(lEndDateString)
        lSW.WriteLine(lSegmentFileName)
        lSW.WriteLine(lDirectoryFileName)
        lSW.Close()

        'write segments file
        lSW = New IO.StreamWriter(lSegmentFileName)
        lSW.WriteLine(Segments.ToString)
        lSW.Close()

        'write timeseries directory file
        lSW = New IO.StreamWriter(lDirectoryFileName)
        lSW.WriteLine(Segments.TimeseriesDirectoryToString)
        lSW.Close()

        'write timeseries files
        Me.InputTimeseriesCollection.TimeSeriesToFile(lDirectoryFileName, Me.SJDate, Me.EJDate)

        Return True
    End Function

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lWASPDir As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\USEPA\WASP\7.0", "DatabaseDir", "") & "..\wasp.exe"
            Dim lWASPexe As String = atcUtility.FindFile("Please locate the EPA WASP Executable", lWASPDir)
            If IO.File.Exists(lWASPexe) Then
                LaunchProgram(lWASPexe, IO.Path.GetDirectoryName(aInputFileName), aInputFileName, False)
                Logger.Dbg("WASP launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA WASP Executable", MsgBoxStyle.Critical, "BASINS WASP Problem")
            End If
        Else
            Logger.Msg("Cannot find WASP Input File " & aInputFileName)
        End If
    End Sub

    Public Function TravelTime(ByVal aLengthKM As Double, ByVal aVelocityMPS As Double) As Double
        Dim lTravelTime As Double = 0.0
        If aVelocityMPS > 0 Then
            lTravelTime = aLengthKM * 1000 / aVelocityMPS / (60 * 60 * 24)  'computes in days
        End If
        Return lTravelTime
    End Function

    Sub GenerateSegments(ByVal lSegmentLayerIndex As Integer, ByVal aMaxTravelTime As Double, ByVal aMinTravelTime As Double)
        With Me
            Try
                'populate the WASP classes from the shapefiles
                .Segments.Clear()
                Dim lTable As New atcUtility.atcTableDBF

                'add only selected segments
                Dim lTempSegments As New atcWASPSegments
                Dim lSegmentShapefileName As String = GisUtil.LayerFileName(lSegmentLayerIndex)
                If lTable.OpenFile(FilenameSetExt(lSegmentShapefileName, "dbf")) Then
                    Logger.Dbg("Add " & lTable.NumRecords & " SegmentsFrom " & lSegmentShapefileName)
                    lTempSegments.AddRange(NumberObjects(lTable.PopulateObjects((New atcWASP.atcWASPSegment).GetType, .SegmentFieldMap), "Name"))
                End If
                Logger.Dbg("SegmentsCount " & lTempSegments.Count)

                For Each lSegment As atcWASP.atcWASPSegment In lTempSegments
                    Dim lTimeseriesCollection As New atcWASP.atcWASPTimeseriesCollection
                    lSegment.InputTimeseriesCollection = lTimeseriesCollection
                    lSegment.BaseID = lSegment.ID   'store segment id before breaking up
                Next

                'after reading the attribute table, see if any are selected
                If GisUtil.NumSelectedFeatures(lSegmentLayerIndex) > 0 Then
                    'put only selected segments in .segments 
                    For lIndex As Integer = 0 To GisUtil.NumSelectedFeatures(lSegmentLayerIndex) - 1
                        Dim lShapeIndex As Integer = GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex, lSegmentLayerIndex)
                        Dim lSegment As atcWASP.atcWASPSegment = lTempSegments(lShapeIndex)
                        GisUtil.LineCentroid(lSegmentLayerIndex, lShapeIndex, lSegment.CentroidX, lSegment.CentroidY) 'store centroid 
                        GisUtil.PointsOfLine(lSegmentLayerIndex, lShapeIndex, lSegment.PtsX, lSegment.PtsY)  'store point coordinates of vertices
                        Logger.Dbg("Add " & lSegment.ID)
                        .Segments.Add(lSegment)
                    Next
                Else 'add all 
                    .Segments = lTempSegments
                    Dim lShapeIndex As Integer = -1
                    For Each lSegment As atcWASP.atcWASPSegment In lTempSegments
                        lShapeIndex += 1
                        GisUtil.LineCentroid(lSegmentLayerIndex, lShapeIndex, lSegment.CentroidX, lSegment.CentroidY) 'store centroid 
                        GisUtil.PointsOfLine(lSegmentLayerIndex, lShapeIndex, lSegment.PtsX, lSegment.PtsY)  'store point coordinates of vertices
                    Next
                End If
                Logger.Dbg("SegmentsCentroidsAndPointsFor " & .Segments.Count & " SegmentsOf " & lTempSegments.Count)

                'calculate depth and width from mean annual flow and mean annual velocity
                'Depth (ft)= a*DA^b (english):  a= 1.5; b=0.284
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
                Next

                Dim lProblem As String = ""
                If aMinTravelTime > 0 Then 'minimum travel time has been set, combine the segments as needed
                    CombineSegments(aMinTravelTime, lProblem)
                End If

                If aMaxTravelTime > 0 Then 'maximum travel time has been set, divide the segments as needed
                    DivideSegments(aMaxTravelTime)
                End If

                lProblem = .Segments.AssignWaspIds()
                If lProblem.Length > 0 Then
                    Logger.Dbg("ProblemInGenerateSegmentsAssignWaspIds " & lProblem)
                End If
            Catch lEX As Exception
                Logger.Dbg(lEX.Message)
            End Try
        End With
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
            End If
        End If
    End Sub

    Private Function DetermineShortSegments(ByVal aMinTravelTime As Double, ByVal aDownstreamKey As String) As Integer
        Dim lShortSegmentCount As Integer = 0
        Dim lDownstreamSegment As atcWASPSegment = Segments(aDownstreamKey)
        If aMinTravelTime > TravelTime(lDownstreamSegment.Length, lDownstreamSegment.Velocity) Then
            lShortSegmentCount += 1
            lDownstreamSegment.TooShort = True
        End If
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
        Logger.Dbg("Combine " & aSegmentKey & " " & lSegment.TooShort)
        If lSegment.TooShort Then 'too short - combine with segment up or down
            Dim lSegmentCombined As atcWASPSegment = Nothing
            Dim lSegmentRemoved As atcWASPSegment = Nothing
            Dim lUpStreamSegment As atcWASPSegment = UpstreamMainSegment(lSegment.ID)
            If lUpStreamSegment IsNot Nothing Then 'combine with upstream
                lSegmentCombined = CombineSegment(lSegment, lUpStreamSegment, True)
                lSegmentRemoved = lUpStreamSegment
            Else 'combine with downstream (if possible)
                Dim lDownStreamSegment As atcWASPSegment = Segments(lSegment.DownID)
                Dim lDownStreamUpMainSegment As atcWASPSegment = UpstreamMainSegment(lDownStreamSegment.ID)
                If lDownStreamUpMainSegment.ID = lSegment.ID Then
                    lSegmentCombined = CombineSegment(lSegment, lDownStreamSegment, False)
                Else
                    Logger.Dbg("Skip " & lSegment.ID & " Nothing up and not MainChannel")
                End If
            End If
            If lSegmentCombined IsNot Nothing Then
                If aMinTravelTime > TravelTime(lSegmentCombined.Length, lSegmentCombined.Velocity) Then
                    Logger.Dbg("StillToShort " & lSegmentCombined.ID & " " & TravelTime(lSegmentCombined.Length, lSegmentCombined.Velocity))
                    lSegmentCombined.TooShort = True
                    'TODO: what should we do now?
                End If
                aNewSegments.Add(lSegmentCombined)
                'fix DownIds for upstream segments
                For Each lSegment In Segments
                    If lSegment.DownID = lSegmentRemoved.ID Then
                        lSegment.DownID = lSegmentCombined.ID
                    End If
                Next
            End If
        Else 'no problem, use as is
            aNewSegments.Add(lSegment)
        End If
        Logger.Dbg("NewSegmentCount " & aNewSegments.Count)
        'move on upstream
        For Each lSegment In Segments
            If lSegment.DownID = aSegmentKey Then
                CombineSegmentsDetail(lSegment.ID, aMinTravelTime, aNewSegments)
            End If
        Next
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
            lSegment.DownID = aSegmentPrimary.DownID
            lSegment.ID = aSegmentPrimary.ID
        End If

        Try
            With lSegment
                'TODO: better algorithm - weighted?
                .Depth = (aSegmentPrimary.Depth + aSegmentSecondary.Depth) / 2
                .Length = aSegmentPrimary.Length + aSegmentSecondary.Length
                '.Name 
                .Roughness = (aSegmentPrimary.Roughness + aSegmentSecondary.Roughness) / 2
                .Slope = (aSegmentPrimary.Slope + aSegmentSecondary.Slope) / 2
                .Velocity = (aSegmentPrimary.Velocity + aSegmentSecondary.Velocity) / 2
                .Width = (aSegmentPrimary.Width + aSegmentSecondary.Width) / 2
                '.BaseID 
                .CentroidX = (aSegmentPrimary.CentroidX + aSegmentSecondary.CentroidX) / 2
                .CentroidY = (aSegmentPrimary.CentroidY + aSegmentSecondary.CentroidY) / 2
                Dim lPointCount As Integer = aSegmentPrimary.PtsX.GetLength(0) + aSegmentSecondary.PtsX.GetLength(0)
                ReDim .PtsX(lPointCount)
                ReDim .PtsY(lPointCount)
                'TODO: assumes nhdplus convention - up to down, needs to be robust
                If aSecondaryUpstream Then
                    For lIndex As Integer = 0 To aSegmentSecondary.PtsX.GetLength(0) - 1
                        .PtsX(lIndex) = aSegmentSecondary.PtsX(lIndex)
                        .PtsY(lIndex) = aSegmentSecondary.PtsX(lIndex)
                    Next
                    Dim lBasePoint As Integer = aSegmentSecondary.PtsX.GetLength(0)
                    For lIndex As Integer = 0 To aSegmentPrimary.PtsX.GetLength(0) - 1
                        .PtsX(lBasePoint) = aSegmentPrimary.PtsX(lIndex)
                        .PtsY(lBasePoint) = aSegmentPrimary.PtsY(lIndex)
                        lBasePoint += 1
                    Next
                Else
                    For lIndex As Integer = 0 To aSegmentPrimary.PtsX.GetLength(0)
                        .PtsX(lIndex) = aSegmentPrimary.PtsX(lIndex)
                        .PtsY(lIndex) = aSegmentPrimary.PtsX(lIndex)
                    Next
                    Dim lBasePoint As Integer = aSegmentPrimary.PtsX.GetLength(0)
                    For lIndex As Integer = 0 To aSegmentSecondary.PtsX.GetLength(0)
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

    Private Sub DivideSegments(ByVal aMaxTravelTime As Double)
        Dim lNewSegments As New atcWASPSegments
        Dim lNewSegmentPositions As New atcCollection
        For lIndex As Integer = 1 To Segments.Count
            Dim lSegment As atcWASPSegment = Segments(lIndex - 1)
            If TravelTime(lSegment.Length, lSegment.Velocity) > aMaxTravelTime Then
                'need to break this segment into multiple
                Dim lBreakNumber As Integer = Int(TravelTime(lSegment.Length, lSegment.Velocity) / aMaxTravelTime) + 1
                'find cumulative drainage area above this segment
                Dim lCumAbove As Double = CumulativeAreaAboveSegment(lSegment.ID)
                'create the new pieces
                For lBreakIndex As Integer = 2 To lBreakNumber
                    Dim lNewSegment As New atcWASPSegment
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

    Public Sub CreateSegmentShapeFile(ByVal lSegmentLayerIndex As Integer, ByRef aWASPShapefileName As String)
        'come up with name of new shapefile
        Dim lSegmentShapefileName As String = GisUtil.LayerFileName(lSegmentLayerIndex)
        Dim lOutputPath As String = PathNameOnly(lSegmentShapefileName)
        Dim lIndex As Integer = 1
        aWASPShapefileName = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Do While FileExists(aWASPShapefileName)
            lIndex += 1
            aWASPShapefileName = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Loop

        'if there is already a layer by this name on the map, remove it
        If GisUtil.IsLayer("WASP Segments") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("WASP Segments"))
        End If

        'create the new empty shapefile
        GisUtil.CreateEmptyShapefile(aWASPShapefileName, "", "line")
        GisUtil.AddLayer(aWASPShapefileName, "WASP Segments")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(aWASPShapefileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True
        'add an id field to the new shapefile
        Dim lNewIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "ID", 0, 20)
        Dim lNewWASPIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "WASPID", 0, 20)

        'now add segments to the shapefile
        For Each lSegment As atcWASPSegment In Me.Segments
            GisUtil.AddLine(lNewLayerIndex, lSegment.PtsX, lSegment.PtsY)
            GisUtil.SetFeatureValue(lNewLayerIndex, lNewIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lSegment.ID)
            GisUtil.SetFeatureValue(lNewLayerIndex, lNewWASPIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lSegment.WASPID)
        Next
    End Sub

    Public Sub CreateBufferedSegmentShapeFile(ByVal aWASPShapefileFilename As String)
        'come up with name of new shapefile
        Dim lBufferedShapefileFilename As String = ""
        Dim lOutputPath As String = PathNameOnly(aWASPShapefileFilename)
        Dim lIndex As Integer = 1
        lBufferedShapefileFilename = lOutputPath & "\WASPSegmentsBuffered" & lIndex & ".shp"
        Do While FileExists(lBufferedShapefileFilename)
            lIndex += 1
            lBufferedShapefileFilename = lOutputPath & "\WASPSegmentsBuffered" & lIndex & ".shp"
        Loop
        GisUtil.BufferLayer(aWASPShapefileFilename, lBufferedShapefileFilename, 100)

        GisUtil.RemoveLayer(GisUtil.LayerIndex("WASP Segments"))
        GisUtil.AddLayer(lBufferedShapefileFilename, "WASP Segments")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(lBufferedShapefileFilename)
        GisUtil.LayerVisible(lNewLayerIndex) = True

        'add rendering
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(lBufferedShapefileFilename)
        GisUtil.UniqueValuesRenderer(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WASPID"))
        GisUtil.SetLayerLineSize(lLayerIndex, 0)
    End Sub

    Public Sub AddSelectedTimeseriesToWASPSegment(ByVal aKeyString As String, _
                                                  ByRef aStationCandidates As atcWASPTimeseriesCollection, _
                                                  ByRef aWASPProject As atcWASPProject, _
                                                  ByRef aSegment As atcWASPSegment)

        'need to make sure this timeseries is in the class structure
        If aStationCandidates.Contains(aKeyString) Then
            If aWASPProject.InputTimeseriesCollection.Contains(aKeyString) Then
                'already in the project, just reference it from this segment
                aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
            Else
                'not yet in the project, add it
                Dim lTimeseries As atcTimeseries = GetTimeseries(aStationCandidates(aKeyString).DataSourceName, aStationCandidates(aKeyString).ID)
                If lTimeseries Is Nothing Then
                    Logger.Dbg("Could not find timeseries " & aKeyString)
                Else
                    aStationCandidates(aKeyString).TimeSeries = lTimeseries
                    aWASPProject.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
                    aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
                End If
            End If
        ElseIf aKeyString = "FLOW:<mean annual flow>" Then
            'user wants to use the mean annual flow.
            'the fact that there is no timeseries will be the clue that the mean annual flow should be used.
            Dim lStationCandidate As New atcWASPTimeseries
            With lStationCandidate
                .Type = "FLOW"
                .Description = "<mean annual flow> for segment " & aSegment.WASPID
                .SDate = 1
                .EDate = 100000
                .Identifier = aSegment.ID
                .DataSourceName = aSegment.MeanAnnualFlow.ToString  'kludge, but have to store this somewhere
            End With
            aWASPProject.InputTimeseriesCollection.Add(lStationCandidate)
            aSegment.InputTimeseriesCollection.Add(lStationCandidate)
        End If
    End Sub

    Public Sub AddSelectedTimeseriesToWASPProject(ByVal aKeyString As String, _
                                                  ByRef aStationCandidates As atcWASPTimeseriesCollection, _
                                                  ByRef aWASPProject As atcWASPProject)

        'need to make sure this timeseries is in the class structure
        If aStationCandidates.Contains(aKeyString) Then
            If aWASPProject.InputTimeseriesCollection.Contains(aKeyString) Then
                'already in the project, just reference it from this segment
            Else
                'not yet in the project, add it
                Dim lTimeseries As atcTimeseries = GetTimeseries(aStationCandidates(aKeyString).DataSourceName, aStationCandidates(aKeyString).ID)
                If lTimeseries Is Nothing Then
                    Logger.Dbg("Could not find timeseries " & aKeyString)
                Else
                    aStationCandidates(aKeyString).TimeSeries = lTimeseries
                    aWASPProject.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
                End If
            End If
        End If
    End Sub

    Public Sub RebuildTimeseriesCollections(ByVal aAirName As String, ByVal aSolarName As String, ByVal aWindName As String, _
                                            ByVal aGridFlowSource As atcGridSource, ByVal aGridLoadSource As atcGridSource)
        'clear out collections of timeseries prior to rebuilding
        InputTimeseriesCollection.Clear()
        For lIndex As Integer = 1 To Segments.Count
            Segments(lIndex - 1).InputTimeseriesCollection.Clear()
        Next

        'build collections of timeseries 
        Dim lKeyString As String = ""
        For lIndex As Integer = 1 To Segments.Count
            'input flows 
            lKeyString = "FLOW:" & aGridFlowSource.CellValue(lIndex, 3)
            If aGridFlowSource.CellValue(lIndex, 3) <> "<none>" Then
                AddSelectedTimeseriesToWASPSegment(lKeyString, FlowStationCandidates, Me, Segments(lIndex - 1))
            End If
            'need to add other wq loads
            lKeyString = "WTMP:" & aGridLoadSource.CellValue(lIndex, 1)
            If aGridLoadSource.CellValue(lIndex, 1) <> "<none>" Then
                AddSelectedTimeseriesToWASPSegment(lKeyString, WaterTempStationCandidates, Me, Segments(lIndex - 1))
            End If
        Next
        'met timeseries are not segment-specific
        'air temp
        If aAirName <> "<none>" Then
            lKeyString = "ATMP:" & aAirName
            AddSelectedTimeseriesToWASPProject(lKeyString, AirTempStationCandidates, Me)
            lKeyString = "ATEM:" & aAirName
            AddSelectedTimeseriesToWASPProject(lKeyString, AirTempStationCandidates, Me)
        End If
        'sol rad
        If aSolarName <> "<none>" Then
            lKeyString = "SOLR:" & aSolarName
            AddSelectedTimeseriesToWASPProject(lKeyString, SolRadStationCandidates, Me)
            lKeyString = "SOLRAD:" & aSolarName
            AddSelectedTimeseriesToWASPProject(lKeyString, SolRadStationCandidates, Me)
        End If
        'wind 
        If aWindName <> "<none>" Then
            lKeyString = "WIND:" & aWindName
            AddSelectedTimeseriesToWASPProject(lKeyString, WindStationCandidates, Me)
        End If
    End Sub

    Public Sub BuildListofValidStationNames(ByRef aConstituent As String, _
                                            ByVal aStationCandidates As atcWASPTimeseriesCollection)

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            BuildListofValidStationNamesFromDataSource(lDataSource, aConstituent, aStationCandidates)
        Next

    End Sub

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
            aAirIndex = FindClosestMetStation(AirTempStationCandidates, lXAvg, lYAvg)
            aSolarIndex = FindClosestMetStation(SolRadStationCandidates, lXAvg, lYAvg)
            aWindIndex = FindClosestMetStation(WindStationCandidates, lXAvg, lYAvg)
        Else
            aAirIndex = 0
            aSolarIndex = 0
            aWindIndex = 0
        End If
    End Sub

    Private Function FindClosestMetStation(ByVal aStationList As atcWASPTimeseriesCollection, ByVal aXAvg As Double, ByVal aYAvg As Double) As Integer
        'for each valid value, find distance
        Dim lShortestDistance As Double = 1.0E+28
        Dim lDistance As Double = 0.0
        Dim lClosestIndex As Integer = 0
        Dim lStationIndex As Integer = 0
        For Each lStationCandidate As atcWASPTimeseries In aStationList
            lStationIndex += 1
            lDistance = CalculateDistance(aXAvg, aYAvg, lStationCandidate.LocationX, lStationCandidate.LocationY)
            If lDistance < lShortestDistance Then
                lShortestDistance = lDistance
                lClosestIndex = lStationIndex
            End If
        Next
        Return lClosestIndex
    End Function
End Class
