Imports MapWinUtility
Imports atcMwGisUtility
Imports atcUtility
Imports atcData

Module modWaspUtil

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

    Public Sub BreakLineIntoNthPart(ByVal aOrigX() As Double, ByVal aOrigY() As Double, ByVal aPieceIndex As Integer, ByVal aNumPieces As Integer, ByRef aNewX() As Double, ByRef aNewY() As Double)
        'break a line into specified number of segments, return the nth part 
        'given XY coords of vertices, return XY coords of new vertices 

        'first compute total length and distance between points
        Dim lTotalLength As Double = 0
        Dim lDistance(aOrigX.GetUpperBound(0)) As Double
        For lIndex As Integer = 0 To aOrigX.GetUpperBound(0) - 1
            lDistance(lIndex) = System.Math.Sqrt(((aOrigX(lIndex + 1) - aOrigX(lIndex)) ^ 2) + ((aOrigY(lIndex + 1) - aOrigY(lIndex)) ^ 2))
            lTotalLength = lTotalLength + lDistance(lIndex)
        Next

        'find desired length
        Dim lDesiredLength As Double = lTotalLength / aNumPieces
        Dim lStartingDistance As Double = lTotalLength * (aPieceIndex - 1) / aNumPieces
        Dim lEndingDistance As Double = lStartingDistance + lDesiredLength

        'build arrays to store all points including new points
        Dim lXNew(aOrigX.GetUpperBound(0) + aNumPieces) As Double
        Dim lYNew(aOrigY.GetUpperBound(0) + aNumPieces) As Double

        Dim lCumDist As Double = 0.0
        Dim lPtCount As Integer = 0
        lXNew(0) = aOrigX(0)
        lYNew(0) = aOrigY(0)
        Dim lStartTrimDistance As Double = 0
        Dim lDistanceToNextPoint As Double = 0
        Dim lEndingTrimDistance As Double = 0
        Dim lDistanceToThisPoint As Double = 0
        For lIndex As Integer = 0 To lDistance.GetUpperBound(0) - 1
            lCumDist = lCumDist + lDistance(lIndex)
            If (lStartingDistance = 0 And lIndex = 0) Then
                'want the very beginning of the line, store this point in first position for future use
                lXNew(0) = aOrigX(lIndex)
                lYNew(0) = aOrigY(lIndex)
            ElseIf lCumDist < lStartingDistance Then
                'store this point in first position for future use
                lXNew(0) = aOrigX(lIndex)
                lYNew(0) = aOrigY(lIndex)
                lStartTrimDistance = lStartingDistance - lCumDist
                lDistanceToNextPoint = lDistance(lIndex + 1)
            ElseIf lCumDist > lStartingDistance And lCumDist < lEndingDistance Then
                'store this point and keep going
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex)
                lYNew(lPtCount) = aOrigY(lIndex)
            ElseIf lCumDist > lEndingDistance Then
                'would be too much, store this point and stop
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex)
                lYNew(lPtCount) = aOrigY(lIndex)
                lEndingTrimDistance = lCumDist - lEndingDistance
                lDistanceToThisPoint = lDistance(lIndex)
                Exit For
            ElseIf lCumDist = lEndingDistance Then
                'exactly the right amount, store this last line segment and stop
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex)
                lYNew(lPtCount) = aOrigY(lIndex)
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex + 1)
                lYNew(lPtCount) = aOrigY(lIndex + 1)
                lEndingTrimDistance = 0
                Exit For
            End If
        Next

        'pts 0 and lptcount need to be trimmed down to size
        If lStartTrimDistance > 0 And lDistanceToNextPoint > 0 Then
            'trim starting point
            lXNew(0) = lXNew(0) + ((lXNew(1) - lXNew(0)) * ((lStartTrimDistance) / lDistanceToNextPoint))
            lYNew(0) = lYNew(0) + ((lYNew(1) - lYNew(0)) * ((lStartTrimDistance) / lDistanceToNextPoint))
        End If
        If lEndingTrimDistance > 0 And lDistanceToThisPoint > 0 Then
            'trim ending point
            lXNew(lPtCount) = lXNew(lPtCount) - ((lXNew(lPtCount) - lXNew(lPtCount - 1)) * (lEndingTrimDistance / lDistanceToThisPoint))
            lYNew(lPtCount) = lYNew(lPtCount) - ((lYNew(lPtCount) - lYNew(lPtCount - 1)) * (lEndingTrimDistance / lDistanceToThisPoint))
        End If

        'return an array of the proper size
        ReDim aNewX(lPtCount)
        ReDim aNewY(lPtCount)
        For lIndex As Integer = 0 To lPtCount
            aNewX(lIndex) = lXNew(lIndex)
            aNewY(lIndex) = lYNew(lIndex)
        Next

    End Sub

    Friend Sub BuildListofValidStationNamesFromDataSource(ByVal aDataSource As atcTimeseriesSource, _
                                                          ByRef aConstituent As String, _
                                                          ByVal aStationCandidates As atcWASPTimeseriesCollection)

        Dim lTotalCount As Integer = aDataSource.DataSets.Count
        Dim lCounter As Integer = 0

        For Each lDataSet As atcData.atcTimeseries In aDataSource.DataSets
            lCounter += 1
            Logger.Progress(lCounter, lTotalCount)

            If lDataSet.Attributes.GetValue("Constituent") = aConstituent Or aConstituent = "" Then
                Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                Dim lDataSourceName As String = lDataSet.Attributes.GetValue("Data Source")
                Dim lSJDay As Double
                Dim lEJDay As Double
                Dim lConstituent As String = lDataSet.Attributes.GetValue("Constituent")
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
                Dim lCandidateTimeseries As New atcWASPTimeseries
                lCandidateTimeseries.Identifier = lLoc
                lCandidateTimeseries.SDate = lSJDay
                lCandidateTimeseries.EDate = lEJDay
                If aConstituent.Length > 0 Then
                    lCandidateTimeseries.Description = lLoc & ":" & lStanam & " " & lDateString
                Else
                    lCandidateTimeseries.Description = lConstituent & ":" & lLoc & ":" & lStanam & " " & lDateString
                End If
                lCandidateTimeseries.Type = lConstituent
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

    Friend Function CalculateDistance(ByVal aX1 As Double, ByVal aY1 As Double, ByVal aX2 As Double, ByVal aY2 As Double) As Double
        Dim lDistance As Double
        lDistance = Math.Sqrt(((aX2 - aX1) ^ 2) + ((aY2 - aY1) ^ 2))
        Return lDistance
    End Function

    ''' <summary>
    ''' Write line to file, substituting TAB character for \t in format string
    ''' </summary>
    Friend Sub WriteLine(ByRef sw As IO.StreamWriter, ByVal FormatString As String, ByVal ParamArray Args() As Object)
        If String.IsNullOrEmpty(FormatString) Then FormatString = ""
        sw.WriteLine(String.Format(FormatString, Args).Replace("\t", vbTab))
    End Sub

    ''' <summary>
    ''' Format string using standard String.Format, except substitute \t and \n with tab and newline characters
    ''' </summary>
    Friend Function StringFormat(ByVal Format As String, ByVal ParamArray Args() As Object) As String
        Return String.Format(Format, Args).Replace("\t", vbTab).Replace("\n", vbNewLine)
    End Function

    ''' <summary>
    ''' Test object; if dbnull, nothing or empty string, return default value
    ''' </summary>
    Friend Function TestNull(ByVal Value As Object, ByVal DefaultValue As Object) As Object
        If IsDBNull(Value) OrElse IsNothing(Value) OrElse (TypeOf Value Is String AndAlso Value = "") Then Return DefaultValue Else Return Value
    End Function

End Module
