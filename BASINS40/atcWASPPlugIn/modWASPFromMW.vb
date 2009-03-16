Imports atcMwGisUtility
Imports atcWASP
Imports atcData
Imports MapWinUtility
Imports atcUtility

Module modWASPFromMW
    Friend Sub BuildListofValidStationNames(ByRef aConstituent As String, _
                                            ByVal aStationCandidates As WASPTimeseriesCollection)

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            BuildListofValidStationNamesFromDataSource(lDataSource, aConstituent, aStationCandidates)
        Next

    End Sub

    Friend Sub BuildListofValidStationNamesFromDataSource(ByVal aDataSource As atcTimeseriesSource, _
                                                          ByRef aConstituent As String, _
                                                          ByVal aStationCandidates As WASPTimeseriesCollection)

        Dim lTotalCount As Integer = aDataSource.DataSets.Count
        Dim lCounter As Integer = 0

        For Each lDataSet As atcData.atcTimeseries In aDataSource.DataSets
            lCounter += 1
            Logger.Progress(lCounter, lTotalCount)

            If lDataSet.Attributes.GetValue("Constituent") = aConstituent Then
                Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                Dim lDataSourceName As String = lDataSet.Attributes.GetValue("Data Source")
                Dim lSJDay As Double
                Dim lEJDay As Double
                lSJDay = lDataSet.Attributes.GetValue("Start Date", 0)
                lEJDay = lDataSet.Attributes.GetValue("End Date", 0)
                If lSJDay = 0 Then
                    lSJDay = lDataSet.Dates.Value(0)
                End If
                If lEJDay = 0 Then
                    lEJDay = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                End If

                Dim lSdate(6) As Integer
                Dim lEdate(6) As Integer
                J2Date(lSJDay, lSdate)
                J2Date(lEJDay, lEdate)
                Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                Dim lCandidateTimeseries As New WASPTimeseries
                lCandidateTimeseries.Identifier = lLoc
                lCandidateTimeseries.SDate = lSJDay
                lCandidateTimeseries.EDate = lEJDay
                lCandidateTimeseries.Description = lLoc & ":" & lStanam & " " & lDateString
                lCandidateTimeseries.Type = aConstituent
                lCandidateTimeseries.ID = lDsn
                lCandidateTimeseries.DataSourceName = lDataSourceName
                Try
                    aStationCandidates.Add(lCandidateTimeseries)
                Catch
                    Logger.Dbg("Problem adding " & lCandidateTimeseries.Description)
                End Try
            End If
            If aDataSource.Name = "Timeseries::WDM" Then
                'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                lDataSet.ValuesNeedToBeRead = True
            End If
        Next

        Logger.Dbg("Found " & aStationCandidates.Count & " Stations")
    End Sub

    Friend Sub AddSelectedTimeseriesToWASPSegment(ByVal aKeyString As String, _
                                                  ByRef aStationCandidates As WASPTimeseriesCollection, _
                                                  ByRef aWASPProject As WASPProject, _
                                                  ByRef aSegment As Segment)

        'need to make sure this timeseries is in the class structure
        If aStationCandidates.Contains(aKeyString) Then
            If aWASPProject.InputTimeseriesCollection.Contains(aKeyString) Then
                'already in the project, just reference it from this segment
                aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
            Else
                'not yet in the project, add it
                Dim lTimeseries As atcTimeseries = GetTimeseries(aStationCandidates(aKeyString).DataSourceName, aStationCandidates(aKeyString).ID)
                If lTimeseries Is Nothing Then
                    Logger.Dbg("Coud not find timeseries " & aKeyString)
                Else
                    aStationCandidates(aKeyString).TimeSeries = lTimeseries
                    aWASPProject.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
                    aSegment.InputTimeseriesCollection.Add(aStationCandidates(aKeyString))
                End If
            End If
        End If
    End Sub

    Friend Function GetTimeseries(ByVal aDataSourceName As String, _
                                  ByVal aID As Integer) As atcData.atcTimeseries
        Dim lGetTimeseries As atcData.atcTimeseries = Nothing

        For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
            lGetTimeseries = GetTimeseriesFromDataSource(lDataSource, aDataSourceName, aID)
            If Not lGetTimeseries Is Nothing Then Exit For
        Next

        Return lGetTimeseries
    End Function

    Friend Function GetTimeseriesFromDataSource(ByVal aDataSource As atcTimeseriesSource, _
                                                ByVal aDataSourceName As String, _
                                                ByVal aID As Integer) As atcData.atcTimeseries

        Dim lGetTimeseries As atcData.atcTimeseries = Nothing

        For Each lDataSet As atcData.atcTimeseries In aDataSource.DataSets
            If (lDataSet.Attributes.GetValue("Data Source") = aDataSourceName And _
                            lDataSet.Attributes.GetValue("ID") = aID) Then
                lGetTimeseries = lDataSet
                Exit For
            End If
        Next

        Return lGetTimeseries
    End Function

    Friend Function TravelTime(ByVal aLengthKM As Double, ByVal aVelocityFPS As Double) As Double
        Dim lTravelTime As Double
        lTravelTime = aLengthKM / aVelocityFPS * 3281 / 60  'computes in minutes
        Return lTravelTime
    End Function

    Public Function IntegerToAlphabet(ByVal aNumber As Integer) As String
        'given 1 returns 'A'
        'given 26 returns 'Z'
        'given 27 returns 'AA'
        'given 28 returns 'AB'
        Dim lResult As String = ""

        If (26 > aNumber) Then
            lResult = Chr(aNumber + 65)
        Else
            Dim lColumn As Integer
            Do
                lColumn = aNumber Mod 26
                aNumber = (aNumber \ 26) - 1
                lResult = Chr(lColumn + 65) + lResult
            Loop Until (aNumber < 0)
        End If

        Return lResult
    End Function

    Public Sub BreakLine(ByVal aXOrig() As Double, ByVal aYOrig() As Double, ByVal aNumPieces As Integer, ByRef aXNew() As Double, ByRef aYNew() As Double, ByRef aLineEndIndexes() As Integer)
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

    Friend Sub GetMetStationCoordinates(ByVal aMetLayerIndex As Integer, ByVal aStationCandidates As WASPTimeseriesCollection)
        Dim lFieldIndex As Integer = 1
        If GisUtil.IsField(aMetLayerIndex, "LOCATION") Then
            lFieldIndex = GisUtil.FieldIndex(aMetLayerIndex, "LOCATION")
        End If

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(aMetLayerIndex) - 1
            Dim lTempID As String = GisUtil.FieldValue(aMetLayerIndex, lFeatureIndex, lFieldIndex)
            For Each lStationCandidate As WASPTimeseries In aStationCandidates
                If lStationCandidate.Identifier = lTempID Then
                    'found a timeseries, add the coordinates
                    GisUtil.PointXY(aMetLayerIndex, lFeatureIndex, lStationCandidate.LocationX, lStationCandidate.LocationY)
                End If
            Next
        Next
    End Sub

    Friend Function CalculateDistance(ByVal aX1 As Double, ByVal aY1 As Double, ByVal aX2 As Double, ByVal aY2 As Double) As Double
        Dim lDistance As Double
        lDistance = Math.Sqrt(((aX2 - aX1) ^ 2) + ((aY2 - aY1) ^ 2))
        Return lDistance
    End Function

End Module
