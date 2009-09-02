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
    Public ModelType As Integer = 2
    Public Name As String = ""
    Public WNFFileName As String = ""
    Public INPFileName As String = String.Empty
    Public SegmentFieldMap As New atcCollection
    Public WASPConstituents As New atcCollection
    Public WASPTimeFunctions As New atcCollection
    Public WASPTimeFunctionIds As New atcCollection

    Public FlowStationCandidates As New atcWASPTimeseriesCollection
    Public AllStationCandidates As New atcWASPTimeseriesCollection

    'the following are used in creating the wasp inp file
    Friend NumFlowFunc(5) As Integer
    Friend FlowPathList As New List(Of Integer) ' subset of Headwaters that actually have flowpath defined, zero-based

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
        Segments.WASPProject = Me

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
                LaunchProgram(lWASPexe, IO.Path.GetDirectoryName(aInputFileName), "-import " & """" & aInputFileName & """", False)
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

    Sub GenerateSegments(ByVal aSegmentLayerIndex As Integer, ByVal aSelectedIndexes As atcCollection, ByVal aMaxTravelTime As Double, ByVal aMinTravelTime As Double)
        With Me
            Try
                'populate the WASP classes from the shapefiles
                .Segments.Clear()
                Dim lTable As New atcUtility.atcTableDBF

                'add only selected segments
                Dim lSegmentShapefileName As String = GisUtil.LayerFileName(aSegmentLayerIndex)
                If lTable.OpenFile(FilenameSetExt(lSegmentShapefileName, "dbf")) Then
                    Logger.Dbg("Add " & aSelectedIndexes.Count & " SegmentsFrom " & lSegmentShapefileName)
                    Dim lSegmentsSelected As New ArrayList '(Of atcWASP.atcWASPSegment)
                    For Each lShapeIndex As Integer In aSelectedIndexes
                        lTable.CurrentRecord = lShapeIndex + 1
                        Dim lSeg As New atcWASP.atcWASPSegment
                        lTable.PopulateObject(lSeg, .SegmentFieldMap)
                        Dim lTimeseriesCollection As New atcWASP.atcWASPTimeseriesCollection
                        lSeg.InputTimeseriesCollection = lTimeseriesCollection
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
                Segments.WASPProject = Me
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
                If lSegment.DownID = aSegmentKey Then
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
            lSegment.DownID = aSegmentPrimary.DownID
            lSegment.ID = aSegmentPrimary.ID
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

    Public Sub CreateFlowlineShapeFile(ByVal aSegmentLayerIndex As Integer, ByRef aSelectedIndexes As atcCollection)
        Dim lNewFileName As String = ""
        Dim lSelectedIndexCollection As New Collection
        For Each lSelectedIndex As Object In aSelectedIndexes
            lSelectedIndexCollection.Add(lSelectedIndex)
        Next
        GisUtil.SaveSelectedFeatures(aSegmentLayerIndex, lSelectedIndexCollection, lNewFileName, "polylinez")
        If GisUtil.IsLayer("Flowlines for WASP Project") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Flowlines for WASP Project"))
        End If
        GisUtil.AddLayer(lNewFileName, "Flowlines for WASP Project")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(lNewFileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True
        GisUtil.SetLayerLineSize(lNewLayerIndex, 2)
    End Sub

    Public Sub CreateSegmentShapeFile(ByVal aSegmentLayerIndex As Integer, ByRef aWASPShapefileName As String)
        'come up with name of new shapefile
        Dim lSegmentShapefileName As String = GisUtil.LayerFileName(aSegmentLayerIndex)
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
        Dim lNewWASPIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "SEGID", 0, 20)

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
        GisUtil.UniqueValuesRenderer(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "SEGID"))
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

    Public Sub RebuildTimeseriesCollections(ByVal aGridTimeSource As atcGridSource, _
                                            ByVal aGridFlowSource As atcGridSource, ByVal aGridLoadSource As atcGridSource, ByVal aGridBoundSource As atcGridSource)
        'clear out collections of timeseries prior to rebuilding
        InputTimeseriesCollection.Clear()
        For lIndex As Integer = 1 To Segments.Count
            Segments(lIndex - 1).InputTimeseriesCollection.Clear()
        Next

        'build collections of timeseries 
        Dim lKeyString As String = ""
        Dim lRow As Integer = 0
        For Each lSegment As atcWASPSegment In Segments
            If IsBoundary(lSegment) Then
                lRow = lRow + 1
                'input flows 
                lKeyString = "FLOW:" & aGridFlowSource.CellValue(lRow, 3)
                If aGridFlowSource.CellValue(lRow, 3) <> "<none>" Then
                    AddSelectedTimeseriesToWASPSegment(lKeyString, FlowStationCandidates, Me, lSegment)
                End If
            End If
        Next

        lKeyString = ""
        For lIndex As Integer = 1 To Segments.Count
            For lColumn As Integer = 1 To Me.WASPConstituents.Count
                'wq loads
                'build key string, type is the first part before the colon.
                Dim lColonPos As Integer = InStr(1, aGridLoadSource.CellValue(lIndex, lColumn), ":")
                If lColonPos > 0 Then
                    lKeyString = Mid(aGridLoadSource.CellValue(lIndex, lColumn), 1, lColonPos) & aGridLoadSource.CellValue(lIndex, lColumn)
                Else
                    lKeyString = ""
                End If
                If aGridLoadSource.CellValue(lIndex, lColumn) <> "<none>" Then
                    If lKeyString.Length > 0 Then
                        AllStationCandidates(lKeyString).BoundaryOrLoad = "Load"
                        AllStationCandidates(lKeyString).WASPSystem = WASPConstituents(lColumn - 1)
                        AddSelectedTimeseriesToWASPSegment(lKeyString, AllStationCandidates, Me, Segments(lIndex - 1))
                    End If
                End If
            Next
        Next

        'check to see if each segment is a boundary
        lRow = 0
        For Each lSegment As atcWASPSegment In Segments
            If IsBoundary(lSegment) Then
                lRow = lRow + 1
                For lColumn As Integer = 1 To Me.WASPConstituents.Count
                    'boundaries
                    'build key string, type is the first part before the colon.
                    Dim lColonPos As Integer = InStr(1, aGridBoundSource.CellValue(lRow, lColumn), ":")
                    If lColonPos > 0 Then
                        lKeyString = Mid(aGridBoundSource.CellValue(lRow, lColumn), 1, lColonPos) & aGridBoundSource.CellValue(lRow, lColumn)
                    Else
                        lKeyString = ""
                    End If
                    If aGridBoundSource.CellValue(lRow, lColumn) <> "<none>" Then
                        If lKeyString.Length > 0 Then
                            AllStationCandidates(lKeyString).BoundaryOrLoad = "Boundary"
                            AllStationCandidates(lKeyString).WASPSystem = WASPConstituents(lColumn - 1)
                            AddSelectedTimeseriesToWASPSegment(lKeyString, AllStationCandidates, Me, lSegment)
                        End If
                    End If
                Next
            End If
        Next

        'aGridTimeSource timeseries are not segment-specific
        For lRow = 1 To aGridTimeSource.Rows - 1
            If aGridTimeSource.CellValue(lRow, 1) <> "<none>" Then
                'build key string, type is the first part before the colon.
                Dim lColonPos As Integer = InStr(1, aGridTimeSource.CellValue(lRow, 1), ":")
                If lColonPos > 0 Then
                    lKeyString = Mid(aGridTimeSource.CellValue(lRow, 1), 1, lColonPos) & aGridTimeSource.CellValue(lRow, 1)
                    AllStationCandidates(lKeyString).BoundaryOrLoad = "TimeFunction"
                    AllStationCandidates(lKeyString).WASPSystem = WASPTimeFunctions(lRow - 1)
                    AddSelectedTimeseriesToWASPProject(lKeyString, AllStationCandidates, Me)
                End If
            End If
        Next
    End Sub

    Public Function IsBoundary(ByVal aSegment As atcWASPSegment) As Boolean
        Dim lBoundary As Boolean = False

        Dim lDownBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If aSegment.DownID = lSegment.ID Then
                'this segment connects to one downstream
                lDownBoundary = False
            End If
        Next

        Dim lUpBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = aSegment.ID Then
                'an upstream segment connects to this one
                lUpBoundary = False
            End If
        Next

        If lUpBoundary Or lDownBoundary Then
            lBoundary = True
        End If

        Return lBoundary
    End Function

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

    Public Function WriteINP(ByVal aFileName As String) As Boolean
        'set inp file name
        INPFileName = aFileName

        'First get rid of the old existing .inp file
        'because this is going to be an appendable file
        'so need to start fresh
        If IO.File.Exists(INPFileName) Then
            IO.File.Delete(INPFileName)
        End If

        'Declare a brandnew appendable .inp file
        Dim lSW As New IO.StreamWriter(INPFileName, True)

        writeInpIntro(lSW)
        writeInpSegs(lSW)
        writeInpPath(lSW)
        writeInpFlowFile(lSW)
        writeInpDispFile(lSW)
        writeInpBoundFile(lSW)
        writeInpLoadFile(lSW)
        writeInpTFuncFile(lSW)
        writeInpParamInfoFile(lSW)
        writeInpConstFile(lSW)
        writeInpIcFile(lSW)

        'final flush and close it
        lSW.Flush()
        lSW.Close()

        Return True
    End Function

    Private Function writeInpIntro(ByRef aSW As IO.StreamWriter) As Boolean

        Dim lSDate(6) As Integer
        J2Date(SJDate, lSDate)
        Dim lStartDateString As String = lSDate(1).ToString.PadLeft(5) & lSDate(2).ToString.PadLeft(5) & lSDate(0).ToString.PadLeft(5)
        Dim lJulianEnd As String = (EJDate - SJDate).ToString.PadLeft(10)

        Dim lIntroText As New System.Text.StringBuilder
        lIntroText.AppendLine(Me.ModelType.ToString.PadLeft(5) & "               Module type               SYSFILE")
        lIntroText.AppendLine(lStartDateString & "    0    0    0     Start date and time")
        lIntroText.AppendLine(lStartDateString & "    0    0    0     Skip date and time")
        lIntroText.AppendLine(lJulianEnd & "          Julian end time")
        lIntroText.AppendLine(Me.WASPConstituents.Count.ToString.PadLeft(5) & "               Number of Systems")
        lIntroText.AppendLine("    0               Mass Balance Table Output")
        lIntroText.AppendLine("    1               Solution Technique Option")
        lIntroText.AppendLine("    0               Negative Solution Option")
        lIntroText.AppendLine("    0               Restart Option")
        lIntroText.AppendLine("    1               Time Optimization Option")
        lIntroText.AppendLine("    1               WQ Module Linkage Option")
        lIntroText.AppendLine("    0.9000          TOPT Factor")
        lIntroText.AppendLine("0.003000  1.000     Min and Max Timestep")
        lIntroText.AppendLine("    2               Number print intervals")
        lIntroText.AppendLine("      0.00  1.000   Time and Print Interval")
        lIntroText.AppendLine(lJulianEnd & "  1.000   Time and Print Interval")
        Dim lTemp As String = " "
        For lIndex As Integer = 1 To Me.WASPConstituents.Count
            lTemp = lTemp & "  0"
        Next
        lIntroText.AppendLine(lTemp)
        lIntroText.AppendLine("    0               Number output variables")
        aSW.Write(lIntroText.ToString) 'WriteLine would add an additional \n
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpSegs(ByRef aSW As IO.StreamWriter) As Boolean
        Dim lSegText As New System.Text.StringBuilder
        lSegText.AppendLine(Segments.Count.ToString.PadLeft(5) & "               Number of Segments                            SEGFILE")
        lSegText.AppendLine("    0               Bed Volume Option")
        lSegText.AppendLine("     0.000          Bed Compaction Time Step")
        lSegText.AppendLine("   1.000   1.000    Volume Scale & Conversion Factor")
        lSegText.AppendLine("  Segment   SegName")
        'Write out the segment id number and their names in format: FORMAT(I5,5X,A40)
        Dim line As String = String.Empty
        For Each lSegment As atcWASPSegment In Segments
            line = lSegment.WASPID.ToString.PadLeft(5) & Space(5) & lSegment.Name.Substring(0, lSegment.Name.Length)
            lSegText.AppendLine(line)
        Next

        'Write out segment geometry information
        line = " Segment  BotSeg   iType     Volume  VMult  Vexp    DMult   Dexp    Length   Slope  Width   Rough  Depth_Q0"
        lSegText.AppendLine(line)

        Dim lsegParams(12) As String
        For Each lSegment As atcWASPSegment In Segments
            lsegParams(0) = Space(3) & lSegment.WASPID
            lsegParams(1) = " 0" ' BotSeg
            lsegParams(2) = " 1" ' iType
            lsegParams(3) = " " & String.Format("{0:0.00}", lSegment.Length * 1000.0 * lSegment.Width * lSegment.Depth * 0.5) ' crude assumption for Volume in m3
            lsegParams(4) = " 0.000000" 'VMult    'need default here
            lsegParams(5) = " 0.000000" 'Vexp     'need default here
            lsegParams(6) = " 1.000000" 'DMult    'need default here
            lsegParams(7) = " 0.000000" 'Dexp     'need default here
            lsegParams(8) = " " & String.Format("{0:0.00}", lSegment.Length * 1000.0) ' Length 2, output in meters 
            lsegParams(9) = " " & String.Format("{0:0.00000}", lSegment.Slope) ' slope 6
            lsegParams(10) = " " & String.Format("{0:0.0000}", lSegment.Width) 'Width 4
            lsegParams(11) = " " & String.Format("{0:0.0000}", lSegment.Roughness) ' Rough 6
            lsegParams(12) = " " & String.Format("{0:0.0000}", lSegment.Depth) ' Depth_Q0 6

            line = String.Join(" ", lsegParams)
            lSegText.AppendLine(line)
        Next
        aSW.Write(lSegText.ToString)
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
            loutletSegWASPID = Segments.Item(loutletSegID).WASPID
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
        ltemp.AppendLine("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(0) - 1).WASPID.ToString.PadLeft(4) & Space(11) & lflowfraction)
        ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(0) - 1).WASPID.ToString.PadLeft(4))
        lnumflowroutes += 1

        Dim lend As Boolean = False
        Dim lthisSeg As atcWASPSegment = Segments.Item(lHeadwatersIndexes(0) - 1)
        Dim lthisPair As String = String.Empty
        While Not lend
            If lthisSeg.ID = loutletSegID Then
                lthisPair = lthisSeg.WASPID.ToString.PadLeft(4) & "0".PadLeft(4)
                ltemp.Append(lthisPair & Space(11) & lflowfraction)
                ldoneFlowpaths.Add(lthisPair)
                lnumflowroutes += 1
                lend = True
            Else
                Dim ldownID As String = lthisSeg.DownID
                lthisPair = lthisSeg.WASPID.ToString.PadLeft(4) & Segments.Item(ldownID).WASPID.ToString.PadLeft(4)
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
            ltemp.Append("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WASPID.ToString.PadLeft(4) & Space(11) & lflowfraction)
            ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WASPID.ToString.PadLeft(4))
            lnumflowroutes += 1
            lthisFlowfunction = New System.Text.StringBuilder
            While Not lend
                If lthisSeg.ID = loutletSegID Then
                    lthisPair = lthisSeg.WASPID.ToString.PadLeft(4) & "0".PadLeft(4)
                    If ldoneFlowpaths.Contains(lthisPair) Then
                        lend = True
                        Continue While
                    Else
                        ldoneFlowpaths.Add(lthisPair)
                    End If
                    ltemp.Append(lthisPair & Space(11) & lflowfraction)
                    ldoneFlowpaths.Add(lthisSeg.WASPID.ToString.PadLeft(4) & "0".PadLeft(4))
                    lnumflowroutes += 1
                    lend = True
                Else
                    Dim ldownID As String = lthisSeg.DownID
                    lthisPair = lthisSeg.WASPID.ToString.PadLeft(4) & Segments.Item(ldownID).WASPID.ToString.PadLeft(4)
                    If ldoneFlowpaths.Contains(lthisPair) Then
                        lend = True
                        Continue While
                    Else
                        ldoneFlowpaths.Add(lthisPair)
                    End If
                    ltemp.Append(vbCrLf)
                    ltemp.Append(lthisSeg.WASPID.ToString.PadLeft(4) & Segments.Item(ldownID).WASPID.ToString.PadLeft(4) & Space(11) & lflowfraction)
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
        Dim lPathText As New System.Text.StringBuilder

        lPathText.AppendLine("    4               Flow Pathways                                 PATHFILE")
        lPathText.AppendLine("    6               Number of flow fields") ' this can be hardcoded here as only 6 constant fields
        lPathText.AppendLine("     Flow Field   1")

        'Figure out the flowpaths:
        'this flow path is the base flow network that is used later for other things
        Dim lflowfuncfield1 As Generic.Dictionary(Of Integer, String)
        lflowfuncfield1 = GenerateFlowPaths()
        lPathText.AppendLine(lflowfuncfield1.Count.ToString.PadLeft(5) & Space(15) & "Number of Flow Functions for Flow Field")
        For Each lString As String In lflowfuncfield1.Values
            lPathText.AppendLine(lString)
        Next

        'For now the 2-6 fields' flow func can be hard-coded here, later done by functions
        For lflowField As Integer = 2 To 6
            lPathText.AppendLine("     Flow Field   " & lflowField.ToString)
            lPathText.AppendLine("    0               Number of Flow Functions for Flow Field")
            NumFlowFunc(lflowField - 1) = 0 'TODO: NumFlowFunc when this is done dynamically, this needs to be changed
        Next
        aSW.Write(lPathText.ToString)
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpFlowFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("    6               Number of Flow Fields                          FLOWFILE")
        Dim linflowCat(5) As String
        linflowCat(0) = " Surface Water Flow Field"
        linflowCat(1) = " Porewater Flow Field"
        linflowCat(2) = " Solids - 1"
        linflowCat(3) = " Solids - 2"
        linflowCat(4) = " Solids - 3"
        linflowCat(5) = " Evap/Precip Flow Field"

        Dim lflowTS As atcWASPTimeseries = Nothing
        For i As Integer = 0 To 5  ' Loop through the 6 flow fields
            aSW.WriteLine(linflowCat(i))
            'Assuming inflow time-value correspond to the set up for flowpath
            'so 'NumFlowFunc' array can be used for these different, yet related, sections
            aSW.WriteLine(NumFlowFunc(i).ToString.PadLeft(5) & Space(15) & "Number of inflows")
            If NumFlowFunc(i) = 0 Then
                Continue For
            End If
            aSW.WriteLine("   1.000   1.000    Flow Scale & Conversion Factors")

            'The loop below needs to be expanded to write out different 'type' of flow
            'right now, only the water flow type is assumed to have values
            'a switch on 'linflowCat' name would be needed here
            For j As Integer = 0 To FlowPathList.Count - 1
                aSW.WriteLine(Segments.Item(FlowPathList(j)).Name)
                'Write out this segment's flow timeseries here
                lflowTS = Nothing
                For Each lts As atcWASPTimeseries In Segments.Item(FlowPathList(j)).InputTimeseriesCollection
                    If lts.Type.StartsWith("FLOW") Then
                        lflowTS = lts
                        Exit For
                    End If
                Next
                If lflowTS IsNot Nothing AndAlso lflowTS.TimeSeries IsNot Nothing Then
                    'have a full timeseries to write
                    Dim lValCount As Integer = 0
                    For lindex As Integer = 1 To lflowTS.TimeSeries.Values.Length - 1
                        If lflowTS.TimeSeries.Dates.Values(lindex) >= SJDate And lflowTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                            lValCount = lValCount + 1
                        End If
                    Next
                    aSW.WriteLine(Space(2) & lValCount & Space(15) & "Number of time-flow values")
                    For lindex As Integer = 1 To lflowTS.TimeSeries.Values.Length - 1
                        If lflowTS.TimeSeries.Dates.Values(lindex) >= SJDate And lflowTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                            Dim lFlowVal As Single = lflowTS.TimeSeries.Values(lindex)
                            lFlowVal = lFlowVal / (3.281 ^ 3)  'convert cfs to cms
                            aSW.WriteLine(Space(3) & String.Format("{0:0.000}", lflowTS.TimeSeries.Dates.Values(lindex) - SJDate) & Space(2) & String.Format("{0:0.000}", lFlowVal))
                        End If
                    Next
                ElseIf lflowTS IsNot Nothing Then
                    'have mean annual flow to write
                    'the mean annual flow is stored in the Me.DataSourceName
                    Dim lValue As Double = 0.0
                    If IsNumeric(lflowTS.DataSourceName) Then
                        lValue = CDbl(lflowTS.DataSourceName)
                    End If
                    Dim lJulianEnd As String = String.Format("{0:0.000}", EJDate - SJDate).PadLeft(10)
                    aSW.WriteLine("    2               Number of time-flow values")
                    aSW.WriteLine("     0.000" & "  " & String.Format("{0:0.0000}", lValue))
                    aSW.WriteLine(lJulianEnd & "  " & String.Format("{0:0.0000}", lValue))
                Else
                    aSW.WriteLine("    0               Number of time-flow values")
                End If
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpDispFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("    0               Number of Exchange Fields                     DISPFILE")
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpBoundFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine(WASPConstituents.Count.ToString.PadLeft(5) & "               Number of Systems                             BOUNDFILE")
        'figure out the number of boundaries
        Dim lBoundCount As Integer = 0
        For Each lSegment As atcWASPSegment In Segments
            If IsBoundary(lSegment) Then
                lBoundCount = lBoundCount + 1
            End If
        Next
        For Each lCons As String In WASPConstituents
            aSW.WriteLine("  " & lCons)
            aSW.WriteLine(lBoundCount.ToString.PadLeft(5) & Space(15) & "Number of boundaries")
            aSW.WriteLine("   1.000   1.000    Boundary Scale & Conversion Factors")
            For Each lSegment As atcWASPSegment In Segments
                If IsBoundary(lSegment) Then
                    aSW.WriteLine(lSegment.WASPID.ToString.PadLeft(5) & Space(15) & "Boundary Segment Number")
                    Dim lBoundTS As atcWASPTimeseries = Nothing
                    For Each lts As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                        If lts.BoundaryOrLoad = "Boundary" And lts.WASPSystem = lCons Then
                            lBoundTS = lts
                            Exit For
                        End If
                    Next
                    If Not lBoundTS Is Nothing Then
                        'have a full timeseries to write
                        Dim lValCount As Integer = 0
                        For lindex As Integer = 1 To lBoundTS.TimeSeries.Values.Length - 1
                            If lBoundTS.TimeSeries.Dates.Values(lindex) >= SJDate And lBoundTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                lValCount = lValCount + 1
                            End If
                        Next
                        aSW.WriteLine(Space(2) & lValCount & Space(15) & "Number of time-concentration values")
                        For lindex As Integer = 1 To lBoundTS.TimeSeries.Values.Length - 1
                            If lBoundTS.TimeSeries.Dates.Values(lindex) >= SJDate And lBoundTS.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                aSW.WriteLine(Space(3) & String.Format("{0:0.000}", lBoundTS.TimeSeries.Dates.Values(lindex) - SJDate) & Space(2) & String.Format("{0:0.000}", lBoundTS.TimeSeries.Values(lindex)))
                            End If
                        Next
                    Else
                        Dim lJulianEnd As String = String.Format("{0:0.000}", EJDate - SJDate).PadLeft(10)
                        aSW.WriteLine("    2               Number of time-concentration values")
                        aSW.WriteLine("     0.000" & "  " & String.Format("{0:0.0000}", 0.0))
                        aSW.WriteLine(lJulianEnd & "  " & String.Format("{0:0.0000}", 0.0))
                    End If
                End If
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpLoadFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("    0               NPS Input Option (0=No, 1=Yes)                LOADFILE")
        aSW.WriteLine(WASPConstituents.Count.ToString.PadLeft(5) & "               Number of Systems")
        For Each lCons As String In WASPConstituents
            aSW.WriteLine("  " & lCons)
            'figure out the number of loadings
            Dim lLoadCount As Integer = 0
            For Each lSegment As atcWASPSegment In Segments
                For Each lts As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                    If lts.BoundaryOrLoad = "Load" And lts.WASPSystem = lCons Then
                        lLoadCount = lLoadCount + 1
                    End If
                Next
            Next
            aSW.WriteLine(lLoadCount.ToString.PadLeft(5) & Space(15) & "Number of Loadings")
            If lLoadCount > 0 Then
                aSW.WriteLine("   1.000   1.000    Loading Scale & Conversion Factors")
                For Each lSegment As atcWASPSegment In Segments
                    For Each lts As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                        If lts.BoundaryOrLoad = "Load" And lts.WASPSystem = lCons Then
                            aSW.WriteLine(lSegment.WASPID.ToString.PadLeft(5) & Space(15) & "Loading Segment Number")
                            Dim lValCount As Integer = 0
                            For lindex As Integer = 1 To lts.TimeSeries.Values.Length - 1
                                If lts.TimeSeries.Dates.Values(lindex) >= SJDate And lts.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                    lValCount = lValCount + 1
                                End If
                            Next
                            aSW.WriteLine(Space(2) & lValCount & Space(15) & "Number of time-loading values")
                            For lindex As Integer = 1 To lts.TimeSeries.Values.Length - 1
                                If lts.TimeSeries.Dates.Values(lindex) >= SJDate And lts.TimeSeries.Dates.Values(lindex) <= EJDate Then
                                    aSW.WriteLine(Space(3) & String.Format("{0:0.000}", lts.TimeSeries.Dates.Values(lindex) - SJDate) & Space(2) & String.Format("{0:0.000}", lts.TimeSeries.Values(lindex)))
                                End If
                            Next
                        End If
                    Next
                Next
            End If
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpTFuncFile(ByRef aSW As IO.StreamWriter) As Boolean
        'figure out the number of time functions
        Dim lTimeCount As Integer = 0
        For Each lts As atcWASPTimeseries In Me.InputTimeseriesCollection
            If lts.BoundaryOrLoad = "TimeFunction" Then
                lTimeCount = lTimeCount + 1
            End If
        Next
        aSW.WriteLine(lTimeCount.ToString.PadLeft(5) & Space(15) & "Number of Time Functions                      TFUNCFILE")
        Dim lTimeFunctionId As Integer = 0
        For Each lts As atcWASPTimeseries In Me.InputTimeseriesCollection
            If lts.BoundaryOrLoad = "TimeFunction" Then
                'look through time function names to find the associated id
                For lIndex As Integer = 1 To WASPTimeFunctions.Count
                    If WASPTimeFunctions(lIndex - 1) = lts.WASPSystem Then
                        lTimeFunctionId = WASPTimeFunctionIds(lIndex - 1)
                    End If
                Next
                aSW.WriteLine(lTimeFunctionId.ToString.PadLeft(5) & Space(15) & "Time Function ID Number")
                aSW.WriteLine("  " & lts.WASPSystem)  'use to write time function name
                Dim lValCount As Integer = 0
                For lindex As Integer = 1 To lts.TimeSeries.Values.Length - 1
                    If lts.TimeSeries.Dates.Values(lindex) >= SJDate And lts.TimeSeries.Dates.Values(lindex) <= EJDate Then
                        lValCount = lValCount + 1
                    End If
                Next
                aSW.WriteLine(lValCount.ToString.PadLeft(5) & Space(15) & "Number of Time Pairs in Function")
                For lindex As Integer = 1 To lts.TimeSeries.Values.Length - 1
                    If lts.TimeSeries.Dates.Values(lindex) >= SJDate And lts.TimeSeries.Dates.Values(lindex) <= EJDate Then
                        aSW.WriteLine(Space(3) & String.Format("{0:0.000}", lts.TimeSeries.Dates.Values(lindex) - SJDate) & Space(2) & String.Format("{0:0.000}", lts.TimeSeries.Values(lindex)))
                    End If
                Next
            End If
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpParamInfoFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine(Segments.Count.ToString.PadLeft(5) & Space(15) & "Number of Segments" & Space(28) & "PARAMINFO")
        aSW.WriteLine("    0               Number of Segment Parameters")
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
        aSW.WriteLine("    0               Number of Constants                           CONSTFILE")
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
        aSW.WriteLine(WASPConstituents.Count.ToString.PadLeft(5) & "               Number of Systems                             ICFILE")
        aSW.WriteLine(Segments.Count.ToString.PadLeft(5) & Space(15) & "Number of Segments")
        For lIndex As Integer = 1 To WASPConstituents.Count
            aSW.WriteLine("     Initial conditions for system" & lIndex.ToString.PadLeft(3) & " " & WASPConstituents(lIndex - 1))
            aSW.WriteLine("    0               Solids Transport Field")
            aSW.WriteLine("     1.000          Solids Density, g/mL")
            aSW.WriteLine("     10000.0000     Maximum Allowed Concentration")
            aSW.WriteLine("  Seg   Conc   DissF")
            For lSegIndex As Integer = 1 To Segments.Count
                aSW.WriteLine("   " & lSegIndex.ToString & "  0.000000  1.00000")
            Next
        Next
        aSW.Flush()
        Return True
    End Function

End Class
