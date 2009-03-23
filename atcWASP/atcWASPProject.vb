Imports MapWinUtility
Imports System.Text
Imports atcUtility
Imports atcMwGisUtility

Public Class WASPProject
    Public Segments As Segments
    Public InputTimeseriesCollection As WASPTimeseriesCollection
    Public SJDate As Double = 0.0
    Public EJDate As Double = 0.0
    Public Name As String = ""
    Public WNFFileName As String = ""
    Public SegmentFieldMap As New atcCollection

    Public Sub New()
        Name = ""
        WNFFileName = ""
        Segments = New Segments
        Segments.WASPProject = Me
        'set field mapping for segments
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

        InputTimeseriesCollection = New WASPTimeseriesCollection
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

    Sub GenerateSegments(ByVal aMaxTravelTime As Double)
        With Me
            'calculate depth and width from mean annual flow and mean annual velocity
            'Depth (ft)= a*DA^b (english):  a= 1.5; b=0.284
            For Each lSegment As Segment In .Segments
                lSegment.Depth = 1.5 * (lSegment.CumulativeDrainageArea ^ 0.284)   'gives depth in ft
                lSegment.Width = (lSegment.MeanAnnualFlow / lSegment.Velocity) / lSegment.Depth  'gives width in ft
            Next

            'do unit conversions from NHDPlus units to WASP assumed units
            For Each lSegment As Segment In .Segments
                lSegment.Velocity = SignificantDigits(lSegment.Velocity / 3.281, 3)  'convert ft/s to m/s
                lSegment.MeanAnnualFlow = SignificantDigits(lSegment.MeanAnnualFlow / (3.281 ^ 3), 3) 'convert cfs to cms
                'lSegment.DrainageArea = lSegment.DrainageArea  'already in sq km
                lSegment.Depth = SignificantDigits(lSegment.Depth / 3.281, 3)  'convert ft to m
                lSegment.Width = SignificantDigits(lSegment.Width / 3.281, 3)  'convert ft to m
            Next

            'if a maximum travel time has been set, divide the segments as needed
            Dim lMaxTravelTime As Double = aMaxTravelTime
            If lMaxTravelTime > 0 Then
                Dim lNewSegments As New Segments
                Dim lNewSegmentPositions As New atcCollection
                For lIndex As Integer = 1 To .Segments.Count
                    Dim lSegment As Segment = .Segments(lIndex - 1)
                    If TravelTime(lSegment.Length, lSegment.Velocity) > lMaxTravelTime Then
                        'need to break this segment into multiple
                        Dim lBreakNumber As Integer = Int(TravelTime(lSegment.Length, lSegment.Velocity) / lMaxTravelTime) + 1
                        'find cumulative drainage area above this segment
                        Dim lCumAbove As Double = CumulativeAreaAboveSegment(lSegment.ID)
                        'create the new pieces
                        For lBreakIndex As Integer = 2 To lBreakNumber
                            Dim lNewSegment As New Segment
                            lNewSegment = lSegment.Clone
                            lNewSegment.ID = lSegment.ID & IntegerToAlphabet(lBreakIndex - 1)
                            If lBreakIndex < lBreakNumber Then
                                lNewSegment.DownID = lSegment.ID & IntegerToAlphabet(lBreakIndex)
                            Else
                                lNewSegment.DownID = lSegment.DownID
                            End If
                            lNewSegment.Length = lSegment.Length / lBreakNumber
                            lNewSegment.CumulativeDrainageArea = lCumAbove + ((lSegment.CumulativeDrainageArea - lCumAbove) * lBreakIndex / lBreakNumber)
                            lNewSegments.Add(lNewSegment)
                            lNewSegmentPositions.Add(lNewSegment.ID, lIndex)
                        Next
                        'reset length and id for the original segment 
                        Dim lOldID As String = lSegment.ID
                        lSegment.ID = lOldID & "A"
                        'if this segment id shows up as a downid anywhere else, change it
                        For Each lTempSeg As Segment In .Segments
                            If lTempSeg.DownID = lOldID Then
                                lTempSeg.DownID = lSegment.ID
                            End If
                        Next
                        lSegment.DownID = lOldID & "B"
                        lSegment.Length = lSegment.Length / lBreakNumber
                        lSegment.CumulativeDrainageArea = lCumAbove + ((lSegment.CumulativeDrainageArea - lCumAbove) / lBreakNumber)
                    End If
                Next
                'if any new segments, add them now to the segments collection
                For lIndex As Integer = lNewSegments.Count To 1 Step -1
                    .Segments.Insert(lNewSegmentPositions(lIndex - 1), lNewSegments(lIndex - 1))
                Next
                'because some keys have changes, clear all out and add back in
                lNewSegments.Clear()
                For Each lSegment As Segment In .Segments
                    lNewSegments.Add(lSegment)
                Next
                .Segments = lNewSegments
            End If

            Dim lProblem As String = .Segments.AssignWaspIds()
        End With
    End Sub

    Private Function CumulativeAreaAboveSegment(ByVal aSegmentID As String) As Double
        'find the area above this segment id
        Dim lArea As Double = 0
        With Me
            For Each lSegment As Segment In .Segments
                If lSegment.DownID = aSegmentID Then
                    lArea += lSegment.CumulativeDrainageArea
                End If
            Next
        End With
        Return lArea
    End Function

    Public Sub CreateSegmentShapeFile(ByVal lSegmentLayerIndex As Integer)
        'come up with name of new shapefile
        Dim lSegmentShapefileName As String = GisUtil.LayerFileName(lSegmentLayerIndex)
        Dim lOutputPath As String = PathNameOnly(lSegmentShapefileName)
        Dim lIndex As Integer = 1
        Dim lWASPShapefileName As String = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Do While FileExists(lWASPShapefileName)
            lIndex += 1
            lWASPShapefileName = lOutputPath & "\WASPSegments" & lIndex & ".shp"
        Loop

        'figure out which shapes we want from old shapefile
        Dim lShapeIds As New atcCollection
        For Each lSegment As Segment In Me.Segments
            If Not lShapeIds.Contains(lSegment.BaseID) Then
                lShapeIds.Add(lSegment.BaseID)
            End If
        Next

        'which field is mapped to the id?
        Dim lIDFieldIndex As Integer
        Dim lIDFieldName As String = "ID"
        For lIndex = 0 To SegmentFieldMap.Count - 1
            If SegmentFieldMap.ItemByIndex(lIndex) = "ID" Then
                Dim lKey As String = SegmentFieldMap.Keys(lIndex)
                If GisUtil.IsField(lSegmentLayerIndex, lKey) Then
                    lIDFieldIndex = GisUtil.FieldIndex(lSegmentLayerIndex, lKey)
                    lIDFieldName = lKey
                End If
            End If
        Next

        'create the new empty shapefile
        GisUtil.CreateEmptyShapefile(lWASPShapefileName, "", "line")
        GisUtil.AddLayer(lWASPShapefileName, "WASP Segments")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(lWASPShapefileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True
        'add an id field to the new shapefile
        Dim lNewIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, lIDFieldName, 0, 20)

        'find each desired shape
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lSegmentLayerIndex) - 1
            For Each lShapeId As String In lShapeIds
                If GisUtil.FieldValue(lSegmentLayerIndex, lFeatureIndex, lIDFieldIndex) = lShapeId Then
                    'this is one of the shapes we want

                    'how many shapes do we want to break this one into?
                    Dim lCount As Integer = 0
                    Dim lPieceIDs As New atcCollection
                    For Each lSegment As Segment In Me.Segments
                        If lSegment.BaseID = lShapeId Then
                            lCount = lCount + 1
                            lPieceIDs.Add(lSegment.ID)
                        End If
                    Next

                    Dim lX() As Double = Nothing
                    Dim lY() As Double = Nothing
                    GisUtil.PointsOfLine(lSegmentLayerIndex, lFeatureIndex, lX, lY)

                    If lCount = 1 Then
                        'create line from these points in the new shapefile
                        GisUtil.AddLine(lNewLayerIndex, lX, lY)
                        GisUtil.SetFeatureValue(lNewLayerIndex, lNewIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lShapeId)
                    Else
                        'break this line into lcount pieces
                        Dim lX2() As Double = Nothing
                        Dim lY2() As Double = Nothing
                        Dim lLineEndIndexes(lCount) As Integer
                        BreakLine(lX, lY, lCount, lX2, lY2, lLineEndIndexes)
                        For lLineIndex As Integer = 1 To lCount
                            Dim lXTemp(lLineEndIndexes(lLineIndex) - lLineEndIndexes(lLineIndex - 1)) As Double
                            Dim lYTemp(lLineEndIndexes(lLineIndex) - lLineEndIndexes(lLineIndex - 1)) As Double
                            Dim lPointCounter As Integer = -1
                            For lPoints As Integer = lLineEndIndexes(lLineIndex - 1) To lLineEndIndexes(lLineIndex)
                                lPointCounter += 1
                                lXTemp(lPointCounter) = lX2(lPoints)
                                lYTemp(lPointCounter) = lY2(lPoints)
                            Next
                            GisUtil.AddLine(lNewLayerIndex, lXTemp, lYTemp)
                            GisUtil.SetFeatureValue(lNewLayerIndex, lNewIDFieldIndex, GisUtil.NumFeatures(lNewLayerIndex) - 1, lPieceIDs(lLineIndex - 1))
                        Next
                    End If

                End If
            Next
        Next
    End Sub

    Private Sub BreakLine(ByVal aXOrig() As Double, ByVal aYOrig() As Double, ByVal aNumPieces As Integer, _
                         ByRef aXNew() As Double, ByRef aYNew() As Double, ByRef aLineEndIndexes() As Integer)
        'break a line into specified number of segments
        'given XY coords of vertices, return XY coords of new vertices and index of endpoints within new xy points

        'first compute total length and distance between points
        Dim lTotalLength As Double = 0
        Dim lDistance(aXOrig.GetUpperBound(0)) As Double
        For lIndex As Integer = 1 To aXOrig.GetUpperBound(0)
            lDistance(lIndex) = System.Math.Sqrt(((aXOrig(lIndex) - aXOrig(lIndex - 1)) ^ 2) + ((aYOrig(lIndex) - aYOrig(lIndex - 1)) ^ 2))
            lTotalLength = lTotalLength + lDistance(lIndex)
        Next

        'find desired length
        Dim lDesiredLength As Double = lTotalLength / aNumPieces

        'build arrays to store all points including new points
        ReDim aXNew(aXOrig.GetUpperBound(0) + aNumPieces - 1)
        ReDim aYNew(aYOrig.GetUpperBound(0) + aNumPieces - 1)

        'fill new array of points
        Dim lPiece As Integer = 0
        Dim lCumDist As Double = 0.0
        For lIndex As Integer = 1 To lDistance.GetUpperBound(0)
            If (lCumDist + lDistance(lIndex) > lDesiredLength) Then
                'would be too much, need to calculate end point for this piece
                aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 2) + ((aXOrig(lIndex - 1) - aXOrig(lIndex - 2)) * ((lDesiredLength - lCumDist) / lDistance(lIndex)))
                aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 2) + ((aYOrig(lIndex - 1) - aYOrig(lIndex - 2)) * ((lDesiredLength - lCumDist) / lDistance(lIndex)))
                aLineEndIndexes(lPiece + 1) = lIndex - 1 + lPiece  'save the index of this endpoint
                lPiece += 1
                lCumDist = lDistance(lIndex) * (1 - ((lDesiredLength - lCumDist) / lDistance(lIndex)))
                aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 1)
                aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 1)
            Else
                'not long enough yet, just add point
                aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 1)
                aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 1)
                lCumDist = lCumDist + lDistance(lIndex)
            End If
        Next
        'close out the last piece
        aXNew(aXNew.GetUpperBound(0)) = aXOrig(aXOrig.GetUpperBound(0))
        aYNew(aYNew.GetUpperBound(0)) = aYOrig(aYOrig.GetUpperBound(0))
        aLineEndIndexes(aNumPieces) = aXNew.GetUpperBound(0)
    End Sub

End Class
