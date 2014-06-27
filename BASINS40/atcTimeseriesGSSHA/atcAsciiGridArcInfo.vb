Public Class atcAsciiGridArcInfo
    Private Const Header_NCOLS As String = "NCOLS"
    Private Const Header_NROWS As String = "NROWS"
    Private Const Header_XLLCORNER As String = "XLLCORNER"
    Private Const Header_YLLCORNER As String = "YLLCORNER"
    Private Const Header_CELLSIZE As String = "CELLSIZE"
    Private Const Header_NODATA_VALUE As String = "NODATA_VALUE"

    Private Shared pNaN As Double = atcUtility.GetNaN()

    'Private pFileName As String

    'Public Sub New(aFileName As String)
    '    pFileName = aFileName
    'End Sub

    Public Shared Sub AddTimeStep(aFileName As String, aDate As Date, ByRef aBuilders As atcData.atcTimeseriesGroupBuilder)

        Using New MapWinUtility.ProgressLevel
            If aBuilders Is Nothing Then
                aBuilders = New atcData.atcTimeseriesGroupBuilder(Nothing)
            End If

            Dim lNCOLS As Integer = -1
            Dim lNROWS As Integer = -1
            Dim lXllCorner As Double = pNaN
            Dim lYllCorner As Double = pNaN
            Dim lCellSize As Double = pNaN
            Dim lNODATA_VALUE As Double = pNaN
            Dim lEpsilon As Double = 0.0000000001

            Dim lRow As Integer = 0
            Dim lCol As Integer = 0
            Dim lValuesThisLine() As String
            Dim lHeaderFinished As Boolean = False

            For Each lLine As String In atcUtility.LinesInFile(aFileName)
                lValuesThisLine = lLine.Split(" "c, vbTab)
                If Not lHeaderFinished Then
                    Select Case lValuesThisLine(0).ToUpper()
                        Case Header_NCOLS : lNCOLS = CInt(lValuesThisLine(1))
                        Case Header_NROWS : lNROWS = CInt(lValuesThisLine(1))
                        Case Header_XLLCORNER : lXllCorner = CDbl(lValuesThisLine(1))
                        Case Header_YLLCORNER : lYllCorner = CDbl(lValuesThisLine(1))
                        Case Header_CELLSIZE : lCellSize = CDbl(lValuesThisLine(1))
                        Case Header_NODATA_VALUE : lNODATA_VALUE = CDbl(lValuesThisLine(1))
                        Case Else
                            If IsNumeric(lValuesThisLine(0)) Then
                                lHeaderFinished = True
                                MapWinUtility.Logger.Status("Reading " & lNCOLS & " x " & lNROWS & " values for " & aDate & " from " & IO.Path.GetFileName(aFileName))
                                GoTo ProcessValues
                            End If
                    End Select
                Else
ProcessValues:
                    Dim lDoubleValue As Double
                    For Each lStringValue As String In lValuesThisLine
                        If Double.TryParse(lStringValue, lDoubleValue) Then
                            If Math.Abs(lDoubleValue - lNODATA_VALUE) < lEpsilon Then
                                lDoubleValue = pNaN
                            End If
                            If lCol >= lNCOLS Then
                                lCol = 0
                                lRow += 1
                            Else
                                lCol += 1
                            End If
                            aBuilders.Builder(lRow & "," & lCol).AddValue(aDate, lDoubleValue)
                        End If
                    Next
                End If
            Next
            'TODO: make this set shared attributes so we don't run out of memory
            aBuilders.SetAttributeForAll(Header_XLLCORNER, lXllCorner)
            aBuilders.SetAttributeForAll(Header_YLLCORNER, lYllCorner)
            aBuilders.SetAttributeForAll(Header_CELLSIZE, lCellSize)
        End Using
    End Sub

    Public Shared Sub ReadTimeseries(aDates As List(Of Double), _
                                     aGridFileNames As List(Of String), _
                                     aDataSource As atcData.atcDataSource)
        If aDates IsNot Nothing AndAlso aDates.Count > 0 AndAlso _
            aGridFileNames IsNot Nothing AndAlso aGridFileNames.Count > 0 Then

            Dim lNumDates As Integer = aDates.Count
            Dim lDates As New atcData.atcTimeseries(Nothing)
            lDates.numValues = lNumDates
            lDates.Value(0) = pNaN
            lDates.Attributes.SetValue("Shared", True)
            Dim lSharedAttributes As New atcData.atcDataAttributes
            Dim lKeys As ArrayList = Nothing
            Dim lTsList As ArrayList = Nothing
            Dim lTs(-1, -1) As atcData.atcTimeseries
            Dim lListIndex As Integer = 0
            For Each lGridFileName As String In aGridFileNames
                MapWinUtility.Logger.Progress(lListIndex, lNumDates)
                Using New MapWinUtility.ProgressLevel
                    Dim lDate As Double = aDates(lListIndex)
                    lDates.Value(lListIndex + 1) = lDate
                    Dim lNCOLS As Integer = -1
                    Dim lNROWS As Integer = -1
                    Dim lXllCorner As Double = pNaN
                    Dim lYllCorner As Double = pNaN
                    Dim lCellSize As Double = pNaN
                    Dim lNODATA_VALUE As Double = pNaN
                    Dim lEpsilon As Double = 0.0000000001

                    Dim lRow As Integer = 0
                    Dim lCol As Integer = 0
                    Dim lValuesThisLine() As String
                    Dim lHeaderFinished As Boolean = False

                    For Each lLine As String In atcUtility.LinesInFile(lGridFileName)
                        lValuesThisLine = lLine.Split(" "c, vbTab)
                        If Not lHeaderFinished Then
                            Select Case lValuesThisLine(0).ToUpper()
                                Case Header_NCOLS : lNCOLS = CInt(lValuesThisLine(1))
                                Case Header_NROWS : lNROWS = CInt(lValuesThisLine(1))
                                Case Header_XLLCORNER : lXllCorner = CDbl(lValuesThisLine(1))
                                Case Header_YLLCORNER : lYllCorner = CDbl(lValuesThisLine(1))
                                Case Header_CELLSIZE : lCellSize = CDbl(lValuesThisLine(1))
                                Case Header_NODATA_VALUE : lNODATA_VALUE = CDbl(lValuesThisLine(1))
                                Case Else 'If we have reached the values, then we are finished with the header, do some initialization
                                    If IsNumeric(lValuesThisLine(0)) Then
                                        lHeaderFinished = True
                                        Dim lTotalCells As Long = lNCOLS * lNROWS
                                        If lListIndex = 0 Then
                                            lKeys = New ArrayList(lTotalCells)
                                            lTsList = New ArrayList(lTotalCells)
                                            ReDim lTs(lNCOLS - 1, lNROWS - 1)
                                            lSharedAttributes.SetValue(Header_XLLCORNER, lXllCorner)
                                            lSharedAttributes.SetValue(Header_YLLCORNER, lYllCorner)
                                            lSharedAttributes.SetValue(Header_CELLSIZE, lCellSize)
                                            lSharedAttributes.SetValue("Data Source", aDataSource.Specification)
                                            lSharedAttributes.SetValue("Scenario", "Grid")
                                            lSharedAttributes.SetValue("Constituent", "Precipitation")
                                        End If
                                        MapWinUtility.Logger.Status("Reading " & lNCOLS & " x " & lNROWS & " values for " & atcUtility.DumpDate(lDate) & " from " & IO.Path.GetFileName(lGridFileName))
                                        GoTo ProcessValues
                                    End If
                            End Select
                        Else
ProcessValues:
                            Dim lDoubleValue As Double
                            For Each lStringValue As String In lValuesThisLine
                                If Double.TryParse(lStringValue, lDoubleValue) Then
                                    If Math.Abs(lDoubleValue - lNODATA_VALUE) < lEpsilon Then
                                        lDoubleValue = pNaN
                                    End If

                                    If lListIndex = 0 Then
                                        Dim lKey As String = lCol & "," & lRow
                                        lKeys.Add(lKey)
                                        lTs(lCol, lRow) = New atcData.atcTimeseries(aDataSource)
                                        With lTs(lCol, lRow)
                                            .Dates = lDates
                                            .numValues = lNumDates
                                            .Attributes.SetValue("Location", lKey)
                                            .Attributes.SharedAttributes = lSharedAttributes
                                        End With
                                        'aDataSource.DataSets.Add(lTs(lCol, lRow))
                                        lTsList.Add(lTs(lCol, lRow))
                                    End If

                                    lTs(lCol, lRow).Value(lListIndex + 1) = lDoubleValue

                                    lCol += 1
                                    If lCol >= lNCOLS Then
                                        lCol = 0
                                        lRow += 1
                                    End If
                                End If
                            Next
                        End If
                    Next
                End Using
                lListIndex += 1
            Next
            Dim lCollection As atcUtility.atcCollection = aDataSource.DataSets
            aDataSource.DataSets.AddRange(lKeys, lTsList)
            MapWinUtility.Logger.Progress(lNumDates, lNumDates)
        End If
    End Sub

End Class
