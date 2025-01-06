﻿
Imports atcUtility
Imports atcData
Imports atcBasinsObsWQ
Imports MapWinUtility 'this has to be downloaded separately from http://svn.mapwindow.org/svnroot/MapWindow4Dev/Bin/
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Drawing



Public Module AutomatedGraphs
    Sub MakeAutomatedGraphs(ByVal lGraphStartJ As Double, ByVal lGraphEndJ As Double, ByVal aOutputDirectory As String,
                            Optional aTestPath As String = "")

        'Expect a comma separated file called *.csv
        'On 11/13/2015, Anurag decided that aany number of CSV file with *.csv extension could be added. 
        'In 2017, option to plot JSON files was added.
        Dim lGraphSpecificationFileNames As New NameValueCollection

        AddFilesInDir(lGraphSpecificationFileNames, IO.Directory.GetCurrentDirectory, False, "*.json")

        If Not System.IO.Directory.Exists(aOutputDirectory) Then
            System.IO.Directory.CreateDirectory(aOutputDirectory)
        End If

        Dim NumberOfJSonFiles As Integer = lGraphSpecificationFileNames.Count
        If NumberOfJSonFiles >= 1 Then '
            Logger.Dbg(Now & " Custom graphs will be produced from JSON Files.")
            For Each lGraphSpecificationFile As String In lGraphSpecificationFileNames
                Dim lOutPutGraphFileName As String = IO.Path.GetFileName(lGraphSpecificationFile)
                lOutPutGraphFileName = IO.Path.Combine(aOutputDirectory, lOutPutGraphFileName)
                lOutPutGraphFileName = IO.Path.ChangeExtension(lOutPutGraphFileName, ".png")
                atcGraph.GraphJsonToFile(lGraphSpecificationFile, lOutPutGraphFileName)
            Next
        End If

        lGraphSpecificationFileNames.Clear()

        AddFilesInDir(lGraphSpecificationFileNames, IO.Directory.GetCurrentDirectory, False, "*.csv")
        Dim NumberOfCSVFiles As Integer = lGraphSpecificationFileNames.Count

        If NumberOfCSVFiles >= 1 Then '
            Logger.Dbg(Now & " Custom graphs will be produced from CSV Files.")
        End If

        If NumberOfCSVFiles = 0 AndAlso NumberOfJSonFiles = 0 Then '
            '
            Throw New ApplicationException("No graph specification file found in directory " & IO.Directory.GetCurrentDirectory)
            Logger.Dbg(Now & " Custom graphs will not be produced.")
        End If

        Dim lGraphFilesCount As Integer = 0
        For Each lGraphSpecificationFile As String In lGraphSpecificationFileNames
            lGraphFilesCount += 1
            If IO.Path.GetFileName(lGraphSpecificationFile).ToLower = "res_tp_standard.csv" Then Continue For
            Dim lgraphRecordsNew As New ArrayList()
            Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(lGraphSpecificationFile)
                Dim lines() As String = {}
                If System.IO.File.Exists(lGraphSpecificationFile) Then


                    MyReader.TextFieldType = FileIO.FieldType.Delimited
                    MyReader.SetDelimiters(",")


                    Dim CurrentRow As String()

                    While Not MyReader.EndOfData
                        Try
                            If (MyReader.PeekChars(10000).Contains("***") Or
                                    Trim(MyReader.PeekChars(10000)) = "" Or
                                    Trim(MyReader.PeekChars(10000).ToLower.Contains("type of graph")) Or
                                    Trim(MyReader.PeekChars(10000).ToLower.Contains("axis for the curve")) Or
                                    Trim(MyReader.PeekChars(10000).ToLower.StartsWith(","))) Then
                                CurrentRow = MyReader.ReadFields
                            Else

                                CurrentRow = MyReader.ReadFields
                                Dim i As Integer = 0
                                For Each testtring As String In CurrentRow
                                    testtring = testtring.Replace("deg-", ChrW(186))
                                    testtring = testtring.Replace("mu-", ChrW(956))
                                    CurrentRow(i) = testtring
                                    i += 1
                                Next

                                lgraphRecordsNew.Add(CurrentRow)

                            End If


                        Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                            Logger.Dbg("Line " & ex.Message & " is not valid and will be skipped.")
                            Exit While
                        End Try
                    End While
                    Logger.Dbg(lGraphSpecificationFile & " was used as the Graph Specification File")
                ElseIf (lGraphSpecificationFileNames.Count = 0) Then
                    MsgBox("The Graph specification files were not found.", vbOKOnly)
                    Exit For
                Else
                    Logger.Dbg("The" & lGraphSpecificationFile & " file didn't exist or was blank. Reading next CSV file!")
                    Continue For
                End If

                Dim lRecordIndex As Integer = 0
                Dim ListTypeOfGraph() As String = {"timeseries", "frequency", "scatter", "cumulative probability"}
                Dim ListDatasetType() As String = {"left", "right", "aux", "add", "multiply", "divide", "subtract", "x-axis", "y-axis"}

                If lgraphRecordsNew.Count < 1 Then
                    Logger.Dbg("The" & lGraphSpecificationFile & " file didn't have any useful data. Reading next CSV file!")
                    Continue For
                End If

                Dim lDBFdatasource As New atcDataSourceBasinsObsWQ
                Do
                    Dim lTimeseriesGroup As New atcTimeseriesGroup
                    Dim lGraphInit() As String = lgraphRecordsNew(lRecordIndex)
                    Dim TypeOfGraph As String = Trim(lGraphInit(0)).ToLower
                    If Not (ListTypeOfGraph.Contains(TypeOfGraph)) Then
                        Logger.Dbg("Wrong type of graph specified. Aborting graphing from file " & lGraphSpecificationFile & " Reading next CSV file!")
                        Continue For
                    End If
                    Dim lNumberOfCurves As Integer = Trim(lGraphInit(2))
                    Dim lOutFileName As String = aOutputDirectory & Trim(lGraphInit(1))
                    If lNumberOfCurves < 1 Then
                        Logger.Dbg("The " & lOutFileName & " graph in " & lGraphSpecificationFile & " file didn't have any useful data. Reading next CSV file!")
                        Continue For
                    End If
                    Logger.Dbg("Started preparing graph " & lOutFileName)
                    Dim lGraphStartDateJ, lGraphEndDateJ As Double
                    'GraphInit has information about number of datasets, their color, symbol etc.


                    If Not Trim(lGraphInit(7)) = "" Then
                        Dim SDate As String() = Trim(lGraphInit(7)).Split("/")
                        lGraphStartDateJ = Date2J(SDate(2), SDate(0), SDate(1))

                    Else
                        lGraphStartDateJ = lGraphStartJ
                    End If
                    If Not Trim(lGraphInit(8)) = "" Then
                        Dim EDate As String() = Trim(lGraphInit(8)).Split("/")
                        lGraphEndDateJ = Date2J(EDate(2), EDate(0), EDate(1), 24, 0)

                    Else
                        lGraphEndDateJ = lGraphEndJ
                    End If

                    If TypeOfGraph = "scatter" Then lNumberOfCurves = 2
                    lRecordIndex += 1
                    Dim lGraphDataset() As String = lgraphRecordsNew(lRecordIndex)
                    Dim skipGraph As Boolean = False

                    Do While (ListDatasetType.Contains(lGraphDataset(0).ToLower) Or skipGraph)

                        'For CurveNumber As Integer = 1 To lNumberOfCurves

                        Dim lTimeSeries As atcTimeseries = Nothing
                        Dim lDataSourceFilename As String = AbsolutePath(Trim(lGraphDataset(1)), aTestPath)
                        If IO.File.Exists(lDataSourceFilename) Then
                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lDataSourceFilename)

                            If lDataSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lDataSourceFilename) Then
                                    lDataSource = atcDataManager.DataSourceBySpecification(lDataSourceFilename)
                                End If
                            End If
                            If lDataSource Is Nothing Then
                                Throw New ApplicationException("Could not open '" & lDataSourceFilename & "'")
                            End If
                            Select Case IO.Path.GetExtension(lDataSourceFilename)
                                Case ".wdm"
                                    lTimeSeries = lDataSource.DataSets.FindData("ID", Trim(lGraphDataset(2)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " &
                                                " DSN " & Trim(lGraphDataset(2)) & " to make " & IO.Path.GetFileName(lOutFileName) &
                                                " graph. Moving to next graph!", vbOKOnly, "Automated Graph: Time Series Issue")
                                        lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)

                                        skipGraph = True
                                        Exit Do

                                    End If

                                Case ".hbn", ".dbf"
                                    lTimeSeries = lDataSource.DataSets.FindData("Location", Trim(lGraphDataset(2))) _
                                                                      .FindData("Constituent", Trim(lGraphDataset(3)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " &
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) &
                                                " to make " & IO.Path.GetFileName(lOutFileName) & " graph. Moving to next graph!",
                                               vbOKOnly, "Automated Graph: Time Series Issue")
                                        lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)

                                        skipGraph = True
                                        Exit Do

                                    End If

                                Case ".filtered"
                                    lTimeSeries = lDataSource.DataSets.FindData("Location", Trim(lGraphDataset(2))) _
                                                                      .FindData("Constituent", Trim(lGraphDataset(3)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " &
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " & Trim(lGraphDataset(3)) &
                                                " to make " & IO.Path.GetFileName(lOutFileName) & " graph. Moving to next graph!",
                                               vbOKOnly, "Automated Graph: Time Series Issue")
                                        lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)

                                        skipGraph = True
                                        Exit Do

                                    End If

                                Case ".rdb"
                                    lTimeSeries = lDataSource.DataSets.FindData("ParmCode", Trim(lGraphDataset(2)))(0)
                                    lTimeSeries = SubsetByDate(lTimeSeries, lGraphStartDateJ, lGraphEndDateJ, Nothing)
                                    If lTimeSeries Is Nothing OrElse lTimeSeries.numValues < 1 Then
                                        MsgBox("No timeseries was available from " & lDataSourceFilename & " for " &
                                                " Location " & Trim(lGraphDataset(2)) & " Constituent " &
                                                Trim(lGraphDataset(3)) & " to make " & IO.Path.GetFileName(lOutFileName) &
                                                " graph. Moving to next graph!", vbOKOnly, "Automated Graph: Time Series Issue")
                                        lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)

                                        skipGraph = True
                                        Exit Do
                                    End If
                            End Select





                            If (lGraphDataset.GetUpperBound(0) > 10 AndAlso Not String.IsNullOrEmpty(Trim(lGraphDataset(10)))) Then
                                lTimeSeries = AggregateTS(lTimeSeries, Trim(lGraphDataset(10)).ToLower, Trim(lGraphDataset(11)).ToLower)
                            End If
                            If (lGraphDataset.GetUpperBound(0) > 11 AndAlso Not String.IsNullOrEmpty(Trim(lGraphDataset(12)))) Then
                                Dim Transformation As String = Trim(lGraphDataset(12)).ToLower
                                Select Case True
                                    Case Transformation.Contains("c to f")
                                        lTimeSeries = lTimeSeries * 1.8 + 32

                                    Case Transformation.Contains("f to c")
                                        lTimeSeries = (lTimeSeries - 32) * 0.56

                                    Case Transformation.Contains("sum")
                                        Dim Sum As Double = Convert.ToDouble(Transformation.Split(" ")(1))
                                        lTimeSeries = lTimeSeries + Sum

                                    Case Transformation.Contains("product")
                                        Dim Product As Double = Convert.ToDouble(Transformation.Split(" ")(1))
                                        lTimeSeries = lTimeSeries * Product
                                End Select
                            End If

                            If (lGraphInit.GetUpperBound(0) > 17 AndAlso Not String.IsNullOrEmpty(lGraphInit(18))) Then

                                Dim SeasonStart() As Integer = Array.ConvertAll(lGraphInit(18).Split("/"), Function(str) Int32.Parse(str))
                                Dim SeasonEnd() As Integer = Array.ConvertAll(lGraphInit(19).Split("/"), Function(str) Int32.Parse(str))
                                Dim lseasons As New atcSeasonsYearSubset(SeasonStart(0), SeasonStart(1), SeasonEnd(0), SeasonEnd(1))
                                lseasons.SeasonSelected(0) = True
                                lTimeSeries = lseasons.SplitBySelected(lTimeSeries, Nothing).ItemByIndex(1)

                            End If
                            Dim aTu As Integer = lTimeSeries.Attributes.GetValue("TimeUnit")
                            If (Trim(lGraphDataset(0)).ToLower = "multiply" Or Trim(lGraphDataset(0)).ToLower = "add" Or
                                    Trim(lGraphDataset(0)).ToLower = "subtract" Or Trim(lGraphDataset(0)).ToLower = "divide") AndAlso
                                                        lTimeseriesGroup.Count = 0 Then
                                'Checking if there is a time series before this operation
                                MsgBox("No timeseries was read for the graph " & IO.Path.GetFileName(lOutFileName) & " to add, subtract, multiply, or divide the time series." &
                                           " Skipping to the next graph.", vbOKOnly, "Automated Graph: Time Series Issue")

                                lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)

                                skipGraph = True
                                Exit Do

                            ElseIf (Trim(lGraphDataset(0)).ToLower = "multiply" Or Trim(lGraphDataset(0)).ToLower = "add" Or
                                    Trim(lGraphDataset(0)).ToLower = "subtract" Or Trim(lGraphDataset(0)).ToLower = "divide") AndAlso
                                                       lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                'problem doing math operation if we can't check this -- for instance if both are obs from different days
                                MsgBox("No time unit is defined, can't perform math operation for graph " & IO.Path.GetFileName(lOutFileName) & ". Mathematical operation will not take place.", vbOKOnly,
                                       "Automated Graph: Time Series Issue")
                                lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)
                                skipGraph = True
                                Exit Do

                            ElseIf (Trim(lGraphDataset(0)).ToLower = "multiply" Or Trim(lGraphDataset(0)).ToLower = "add" Or
                                    Trim(lGraphDataset(0)).ToLower = "subtract" Or Trim(lGraphDataset(0)).ToLower = "divide") AndAlso
                                                       lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.GetDefinedValue("TimeUnit").Value <> aTu Then
                                'Checking if the time series read before has the same time steps as the current timeseries
                                MsgBox("The time steps of timseries in the graph " & IO.Path.GetFileName(lOutFileName) & " are different. Mathematical operation will not take place.", vbOKOnly,
                                       "Automated Graph: Time Series Issue")
                                lRecordIndex = skipLines(lgraphRecordsNew, lRecordIndex, ListTypeOfGraph)
                                skipGraph = True
                                Exit Do

                            ElseIf Trim(lGraphDataset(0)) = "add" AndAlso lTimeseriesGroup.Count >= 1 Then

                                lTimeseriesGroup(lTimeseriesGroup.Count - 1) = lTimeseriesGroup(lTimeseriesGroup.Count - 1) + lTimeSeries
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("Constituent", "Sum")
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("ID", "")

                            ElseIf Trim(lGraphDataset(0)) = "multiply" AndAlso lTimeseriesGroup.Count >= 1 Then
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1) = lTimeseriesGroup(lTimeseriesGroup.Count - 1) * lTimeSeries
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("Constituent", "Product")
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("ID", "")

                            ElseIf Trim(lGraphDataset(0)) = "subtract" AndAlso lTimeseriesGroup.Count >= 1 Then
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1) = lTimeseriesGroup(lTimeseriesGroup.Count - 1) - lTimeSeries
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("Constituent", "Subtract")
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("ID", "")

                            ElseIf Trim(lGraphDataset(0)) = "divide" AndAlso lTimeseriesGroup.Count >= 1 Then
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1) = lTimeseriesGroup(lTimeseriesGroup.Count - 1) / lTimeSeries
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("Constituent", "divide")
                                lTimeseriesGroup(lTimeseriesGroup.Count - 1).Attributes.SetValue("ID", "")

                            Else
                                lTimeSeries.Attributes.SetValue("YAxis", Trim(lGraphDataset(0)))
                                lTimeseriesGroup.Add(lTimeSeries)
                            End If

                        Else
                            Logger.Msg("Could not open '" & lDataSourceFilename & "' Aborting Graphing.", MsgBoxStyle.OkOnly, "HSPEXP+")
                            Exit Do
                        End If

                        'Next CurveNumber
                        lRecordIndex += 1
                        If lRecordIndex >= lgraphRecordsNew.Count Then Exit Do
                        lGraphDataset = lgraphRecordsNew(lRecordIndex)
                    Loop
                    If Not skipGraph Then

                        If Trim(lGraphInit(0)).ToLower = "frequency" Or Trim(lGraphInit(0)).ToLower = "scatter" Then
                            'if time units not set for either timeseries, this is sporadic data and more work must be done to match points
                            'Dim lTimeseriesX As atcTimeseries = lTimeseriesGroup(0)
                            'Dim lTimeseriesY As atcTimeseries = lTimeseriesGroup(1)
                            Dim lNewTimeseries As New atcTimeseries
                            Dim lBaseTimeseries As New atcTimeseries
                            Dim lSporadicTimeseries As New atcTimeseries
                            If lTimeseriesGroup(0).Attributes.GetDefinedValue("TimeUnit") Is Nothing Or
                               lTimeseriesGroup(1).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                If lTimeseriesGroup(0).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                    '0(x) is sporadic
                                    lNewTimeseries = lTimeseriesGroup(0).Clone()
                                    lBaseTimeseries = lTimeseriesGroup(1)
                                    lSporadicTimeseries = lTimeseriesGroup(0)
                                ElseIf lTimeseriesGroup(1).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                    '1(y) is sporadic
                                    lNewTimeseries = lTimeseriesGroup(1).Clone()
                                    lBaseTimeseries = lTimeseriesGroup(0)
                                    lSporadicTimeseries = lTimeseriesGroup(1)
                                End If
                                For lIndex As Integer = 1 To lSporadicTimeseries.numValues
                                    Dim lTargetDate As Double = lSporadicTimeseries.Dates.Values(lIndex)
                                    'what value in base is closest in date?
                                    Dim lMinDiff As Double = 999999999.9
                                    Dim lDiff As Double = 0.0
                                    Dim lMinIndex As Integer = 0
                                    For lDateIndex As Integer = 1 To lBaseTimeseries.numValues
                                        lDiff = Math.Abs(lTargetDate - lBaseTimeseries.Dates.Values(lDateIndex))
                                        If lDiff < lMinDiff Then
                                            lMinDiff = lDiff
                                            lMinIndex = lDateIndex
                                        End If
                                    Next
                                    lNewTimeseries.Values(lIndex) = lBaseTimeseries.Values(lMinIndex)
                                Next
                                lNewTimeseries.Attributes.SetValue("Location", lBaseTimeseries.Attributes.GetValue("Location"))
                                lNewTimeseries.Attributes.SetValue("Scenario", lBaseTimeseries.Attributes.GetValue("Scenario"))
                                lNewTimeseries.Attributes.SetValue("Constituent", lBaseTimeseries.Attributes.GetValue("Constituent"))
                                lNewTimeseries.Attributes.SetValue("History 1", lBaseTimeseries.Attributes.GetValue("History 1"))
                                lTimeseriesGroup(1) = lNewTimeseries
                                If lTimeseriesGroup(0).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                    '0(x) is sporadic
                                    lTimeseriesGroup(1) = lNewTimeseries
                                ElseIf lTimeseriesGroup(1).Attributes.GetDefinedValue("TimeUnit") Is Nothing Then
                                    '1(y) is sporadic
                                    lTimeseriesGroup(0) = lNewTimeseries
                                End If
                            End If
                        End If

                        Dim lZgc As ZedGraphControl = CreateZgc(, 1024, 768)
                        Select Case Trim(lGraphInit(0)).ToLower
                            Case "timeseries"
                                TimeSeriesgraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "frequency"
                                FrequencyGraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "scatter"
                                lRecordIndex += 2
                                ScatterPlotGraph(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                            Case "cumulative probability"
                                CumulativeProbability(lTimeseriesGroup, lZgc, lGraphInit, lgraphRecordsNew, lRecordIndex)
                        End Select

                        Dim GraphDirectory As String = System.IO.Path.GetDirectoryName(lOutFileName)
                        If Not System.IO.Directory.Exists(GraphDirectory) Then
                            System.IO.Directory.CreateDirectory(GraphDirectory)
                        End If
                        lZgc.SaveIn(lOutFileName)

                        Dim newlistofattributes() As String = {"Location", "Constituent", "ID"}
                        atcData.atcDataManager.DisplayAttributesSet(newlistofattributes)
                        Dim lList As New atcList.atcListPlugin
                        lList.Save(lTimeseriesGroup, lOutFileName.Substring(0, Len(lOutFileName) - 4) & ".txt")
                        lZgc.Dispose()
                        lRecordIndex -= 1
                        lTimeseriesGroup = Nothing
                        lRecordIndex += 1
                    End If

                Loop While (lRecordIndex + 1) < lgraphRecordsNew.Count

                atcDataManager.DataSets.Clear()
                atcDataManager.Clear()
            End Using
        Next

    End Sub

    Private Function AggregateTS(ByVal aTimeseries As atcTimeseries, ByVal aTimeUnit As String, ByVal aTran As String) As atcTimeseries
        Select Case aTimeUnit
            Case "hourly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUHour, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select

            Case "daily"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select

            Case "monthly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select
            Case "yearly"
                Select Case aTran
                    Case "average"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
                    Case "max"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMax)
                    Case "min"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranMin)
                    Case "sum"
                        Return Aggregate(aTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                    Case Else
                        Return aTimeseries
                End Select
        End Select
        Return aTimeseries
    End Function
    Private Function TimeSeriesgraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object,
                                     ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer) As ZedGraphControl
        Dim lGrapher As New clsGraphTime(aTimeseriesgroup, aZgc)
        Dim lNumberofCurves As Integer = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberofAuxPaneCurves As Integer = 0
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing
        Dim lAuxPane As GraphPane = Nothing
        Dim lCurve As ZedGraph.LineItem = Nothing


        If aZgc.MasterPane.PaneList.Count > 1 Then
            lAuxPane = aZgc.MasterPane.PaneList(0)
            lPaneMain = aZgc.MasterPane.PaneList(1)
            If (aGraphInit.length > 11 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(11)))) Then
                lAuxPane.YAxis.Scale.Min = Trim(aGraphInit(11))
            End If
            If (aGraphInit.length > 12 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(12)))) Then
                lAuxPane.YAxis.Scale.Max = Trim(aGraphInit(12))
            End If
            If (aGraphInit.length > 16 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(16))) AndAlso Trim(aGraphInit(16)).ToLower = "yes") Then
                lAuxPane.YAxis.Type = AxisType.Log
            End If

        Else
            lPaneMain = aZgc.MasterPane.PaneList(0)
        End If


        If (aGraphInit.length > 9 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(9)))) Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If (aGraphInit.length > 10 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(10)))) Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        If (aGraphInit.length > 13 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(13)))) Then
            lPaneMain.Y2Axis.Scale.Min = Trim(aGraphInit(13))
        End If
        If (aGraphInit.length > 14 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(14)))) Then
            lPaneMain.Y2Axis.Scale.Max = Trim(aGraphInit(14))
        End If
        If (aGraphInit.length > 15 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(15))) AndAlso Trim(aGraphInit(15)).ToLower = "yes") Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If
        If (aGraphInit.length > 15 AndAlso Not String.IsNullOrEmpty(Trim(aGraphInit(17))) AndAlso Trim(aGraphInit(17)).ToLower = "yes") Then
            lPaneMain.Y2Axis.Type = AxisType.Log
        End If

        For CurveNumber As Integer = 1 To lNumberofCurves
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex)
            If Trim(lGraphDataset(0)).ToLower = "aux" Then
                lCurve = lAuxPane.CurveList.Item(lNumberofAuxPaneCurves)
                lNumberofAuxPaneCurves += 1
            ElseIf (Trim(lGraphDataset(0)).ToLower = "left" Or
                        Trim(lGraphDataset(0)).ToLower = "right") Then
                lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
                lNumberOfMainPaneCurves += 1
            ElseIf (Trim(lGraphDataset(0)).ToLower = "add" Or
                        Trim(lGraphDataset(0)).ToLower = "multiply" Or
                        Trim(lGraphDataset(0)).ToLower = "subtract" Or
                        Trim(lGraphDataset(0)).ToLower = "divide") Then
                aRecordIndex += 1
                Continue For

            End If

            If Trim(lGraphDataset(4)).ToLower = "line" Then
                lCurve.Symbol.Type = SymbolType.None
                lCurve.Line.IsVisible = True
                lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid

                If lGraphDataset(8).Length = 0 Then
                    lCurve.Line.Width = 1
                Else
                    lCurve.Line.Width = Math.Max(Convert.ToInt32(Trim(lGraphDataset(8))), 1)
                End If
            Else
                lCurve.Line.IsVisible = False
                Select Case Trim(lGraphDataset(7)).ToLower
                    Case "circle"
                        lCurve.Symbol.Type = SymbolType.Circle
                    Case "square"
                        lCurve.Symbol.Type = SymbolType.Square
                    Case "plus"
                        lCurve.Symbol.Type = SymbolType.Plus
                    Case "diamond"
                        lCurve.Symbol.Type = SymbolType.Diamond
                    Case "hdash"
                        lCurve.Symbol.Type = SymbolType.HDash
                    Case "triangle"
                        lCurve.Symbol.Type = SymbolType.Triangle
                    Case "triangledown"
                        lCurve.Symbol.Type = SymbolType.TriangleDown
                    Case "vdash"
                        lCurve.Symbol.Type = SymbolType.VDash
                    Case "xcross"
                        lCurve.Symbol.Type = SymbolType.XCross
                    Case "star"
                        lCurve.Symbol.Type = SymbolType.Star
                    Case Else
                        lCurve.Symbol.Type = SymbolType.Circle
                End Select
                lCurve.Symbol.Fill.IsVisible = True
                If lGraphDataset(8).Length = 0 Then
                    lCurve.Symbol.Size = 1
                Else
                    lCurve.Symbol.Size = Math.Max(Convert.ToInt32(Trim(lGraphDataset(8))), 1)
                End If
            End If

            lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

            If Trim(lGraphDataset(6)).ToLower.Contains("forward") Then
                lCurve.Line.StepType = StepType.ForwardStep
            ElseIf Trim(lGraphDataset(6)).ToLower.Contains("rear") Then
                lCurve.Line.StepType = StepType.RearwardStep
            Else
                lCurve.Line.StepType = StepType.NonStep
            End If

            If Not Trim(lGraphDataset(9)) = "" Then
                If Trim(lGraphDataset(9)).ToLower = "don't show" Then
                    lCurve.Label.IsVisible = False
                End If
                lCurve.Label.Text = Trim(lGraphDataset(9))
            End If
            aRecordIndex += 1
        Next CurveNumber

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))
        lPaneMain.Y2Axis.Title.Text = Trim(aGraphInit(6))
        lPaneMain.YAxis.Scale.Min = 0
        lPaneMain.Y2Axis.Scale.Min = 0

        lPaneMain.YAxis.Title.FontSpec.IsBold = True
        lPaneMain.YAxis.Title.FontSpec.Size += 8
        lPaneMain.YAxis.Scale.FontSpec.Size += 4
        lPaneMain.YAxis.Scale.FontSpec.IsBold = False

        lPaneMain.Y2Axis.Title.FontSpec.IsBold = True
        lPaneMain.Y2Axis.Title.FontSpec.Size += 8
        lPaneMain.Y2Axis.Scale.FontSpec.Size += 4
        lPaneMain.Y2Axis.Scale.FontSpec.IsBold = False
        lPaneMain.Y2Axis.Scale.Max *= 1.0

        lPaneMain.YAxis.Scale.IsUseTenPower = False
        If lPaneMain.YAxis.Scale.Max > 9999 Then
            lPaneMain.YAxis.Scale.IsUseTenPower = True
        End If

        lPaneMain.Legend.FontSpec.Size += 4
        lPaneMain.Legend.FontSpec.IsBold = True

        lPaneMain.XAxis.Title.FontSpec.IsBold = True
        lPaneMain.XAxis.Title.FontSpec.Size += 8
        lPaneMain.XAxis.Scale.FontSpec.Size += 4
        lPaneMain.XAxis.Scale.FontSpec.IsBold = False

        If aZgc.MasterPane.PaneList.Count > 1 Then
            lAuxPane.YAxis.Title.Text = Trim(aGraphInit(5))
            lAuxPane.YAxis.Title.FontSpec.IsBold = True
            lAuxPane.YAxis.Title.FontSpec.Size += 8
            lAuxPane.YAxis.Scale.FontSpec.Size += 4
            lAuxPane.YAxis.Scale.FontSpec.IsBold = False
            lAuxPane.Legend.FontSpec.Size += 4
            lAuxPane.Legend.FontSpec.IsBold = True

            Dim lChartWidthReduction As Single = 15

            lAuxPane.YAxis.Scale.IsUseTenPower = False
            If lAuxPane.YAxis.Scale.Max > 999 Then
                lAuxPane.YAxis.Scale.IsUseTenPower = True
                lChartWidthReduction = 25
                lAuxPane.Margin.All = 100
            End If
            'Somehow IsUseTenPower is not working for Auxiliary Axis and therefore I changed chart width, if numbers are big, and also increased margin. 
            'I am sure there might be a better way to accomplish this.

            Dim lChartWidth As Single = Math.Min(lAuxPane.Chart.Rect.Width, lPaneMain.Chart.Rect.Width) - lChartWidthReduction
            Dim lChartX As Single = Math.Max(lAuxPane.Chart.Rect.X, lPaneMain.Chart.Rect.X) + lChartWidthReduction
            lPaneMain.Chart.Rect = New RectangleF(lChartX, lPaneMain.Chart.Rect.Y, lChartWidth, lPaneMain.Chart.Rect.Height)
            lAuxPane.Chart.Rect = New RectangleF(lChartX, lAuxPane.Chart.Rect.Y, lChartWidth, lAuxPane.Chart.Rect.Height)
        End If

        aTimeseriesgroup = Nothing
        Return aZgc
    End Function
    Private Function FrequencyGraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl, ByVal aGraphInit As Object,
                                     ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer) As ZedGraphControl
        Dim lGrapher As New clsGraphProbability(aTimeseriesgroup, aZgc)

        Dim lNumberofCurves As Integer = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))
        lPaneMain.Legend.FontSpec.Size = 12

        If Trim(aGraphInit(15)) = "yes" Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If

        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.MinAuto = False
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.MaxAuto = False
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        lPaneMain.AxisChange()
        For CurveNumber As Integer = 1 To lNumberofCurves
            Dim lGraphDataset() As String = aGraphRecords(aRecordIndex)

            If (Trim(lGraphDataset(0)).ToLower = "add" Or
                        Trim(lGraphDataset(0)).ToLower = "multiply" Or
                        Trim(lGraphDataset(0)).ToLower = "subtract" Or
                        Trim(lGraphDataset(0)).ToLower = "divide") Then
                Continue For
            End If

            lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
            If Trim(lGraphDataset(4)).ToLower = "line" Then
                lCurve.Symbol.Type = SymbolType.None
                lCurve.Line.IsVisible = True
                lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid
                lCurve.Line.Width = Trim(lGraphDataset(8))
            End If

            If Trim(lGraphDataset(5)).ToLower = "nonstep" Then
                lCurve.Line.StepType = StepType.NonStep
            Else
                lCurve.Line.StepType = StepType.ForwardStep
            End If


            lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

            If Not Trim(lGraphDataset(9)) = "" Then
                If Trim(lGraphDataset(9)).ToLower = "don't show" Then
                    lCurve.Label.IsVisible = False
                End If
                lCurve.Label.Text = Trim(lGraphDataset(9))

            End If

            'End If
            aRecordIndex += 1
            lNumberOfMainPaneCurves += 1
        Next CurveNumber
        lPaneMain.YAxis.Title.FontSpec.IsBold = True
        lPaneMain.YAxis.Title.FontSpec.Size += 8
        lPaneMain.YAxis.Scale.FontSpec.Size += 4
        lPaneMain.YAxis.Scale.FontSpec.IsBold = False

        lPaneMain.Legend.FontSpec.Size += 4
        lPaneMain.Legend.FontSpec.IsBold = True

        lPaneMain.XAxis.Title.FontSpec.IsBold = True
        lPaneMain.XAxis.Title.FontSpec.Size += 8
        lPaneMain.XAxis.Scale.FontSpec.Size += 4
        lPaneMain.XAxis.Scale.FontSpec.IsBold = False


        Return aZgc
    End Function
    Private Function ScatterPlotGraph(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl,
                                      ByVal aGraphInit As Object, ByVal aGraphRecords As Object, ByVal aRecordIndex As Integer) As ZedGraphControl
        Dim lGrapher As New clsGraphScatter(aTimeseriesgroup, aZgc)

        Dim lNumberofCurves As Integer = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))


        lPaneMain.XAxis.Scale.Min = aTimeseriesgroup(0).Attributes.GetValue("Minimum")

        lPaneMain.XAxis.Scale.Max = aTimeseriesgroup(0).Attributes.GetValue("Maximum")
        lPaneMain.YAxis.Scale.Min = aTimeseriesgroup(1).Attributes.GetValue("Minimum")
        lPaneMain.YAxis.Scale.Max = aTimeseriesgroup(1).Attributes.GetValue("Maximum")
        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If
        lPaneMain.AxisChange()

        If Trim(aGraphInit(15)) = "yes" Then
            lPaneMain.YAxis.Type = AxisType.Log
        End If

        Dim lGraphDataset() As String = aGraphRecords(aRecordIndex)

        lCurve = lPaneMain.CurveList.Item(lNumberOfMainPaneCurves)
        If Trim(lGraphDataset(4)).ToLower = "line" Then
            lCurve.Symbol.Type = SymbolType.None
            lCurve.Line.IsVisible = True
            lCurve.Line.Style = Drawing.Drawing2D.DashStyle.Solid
            lCurve.Line.Width = Trim(lGraphDataset(8))
        Else
            lCurve.Line.IsVisible = False
            Select Case Trim(lGraphDataset(7)).ToLower
                Case "circle"
                    lCurve.Symbol.Type = SymbolType.Circle
                    lCurve.Symbol.Fill.IsVisible = True
                Case "square"
                    lCurve.Symbol.Type = SymbolType.Square
                    lCurve.Symbol.Fill.IsVisible = True
                Case "plus"
                    lCurve.Symbol.Type = SymbolType.Plus
                Case "diamond"
                    lCurve.Symbol.Type = SymbolType.Diamond
                    lCurve.Symbol.Fill.IsVisible = True
                Case "hdash"
                    lCurve.Symbol.Type = SymbolType.HDash
                Case "triangle"
                    lCurve.Symbol.Type = SymbolType.Triangle
                    lCurve.Symbol.Fill.IsVisible = True
                Case "triangledown"
                    lCurve.Symbol.Type = SymbolType.TriangleDown
                    lCurve.Symbol.Fill.IsVisible = True
                Case "vdash"
                    lCurve.Symbol.Type = SymbolType.VDash
                Case "xcross"
                    lCurve.Symbol.Type = SymbolType.XCross
                Case "star"
                    lCurve.Symbol.Type = SymbolType.Star
                    lCurve.Symbol.Fill.IsVisible = True
            End Select
            lCurve.Symbol.Size = Trim(lGraphDataset(8))
        End If



        If Trim(lGraphDataset(5)).ToLower = "nonstep" Then
            lCurve.Line.StepType = StepType.NonStep
        Else
            lCurve.Line.StepType = StepType.ForwardStep
        End If


        lCurve.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)

        If Not Trim(lGraphDataset(9)) = "" Then
            lCurve.Label.Text = Trim(lGraphDataset(9))
        End If

        For RecordIndex As Integer = 3 To lNumberofCurves
            lGraphDataset = aGraphRecords(RecordIndex)
            Select Case Trim(lGraphDataset(0)).ToLower
                Case "regression"
                    Dim lACoef As Double
                    Dim lBCoef As Double
                    Dim lRSquare As Double
                    Dim lSJDay As Double
                    Dim lEJDay As Double
                    Dim lTimeseriesX As atcTimeseries = aTimeseriesgroup(0)
                    Dim lTimeseriesY As atcTimeseries = aTimeseriesgroup(1)
                    If lTimeseriesX.Dates.Values(0) < lTimeseriesY.Dates.Values(0) Then
                        'y starts after x, use y start date
                        lSJDay = lTimeseriesY.Dates.Values(0)
                    Else 'use x start date
                        lSJDay = lTimeseriesX.Dates.Values(0)
                    End If
                    If lTimeseriesX.Dates.Values(lTimeseriesX.Dates.numValues) < lTimeseriesY.Dates.Values(lTimeseriesY.Dates.numValues) Then
                        'x ends before y, use x end date
                        lEJDay = lTimeseriesX.Dates.Values(lTimeseriesX.Dates.numValues)
                    Else 'use y end date
                        lEJDay = lTimeseriesY.Dates.Values(lTimeseriesY.Dates.numValues)
                    End If

                    Dim lSubsetTimeseriesX As atcTimeseries = SubsetByDate(lTimeseriesX, lSJDay, lEJDay, Nothing)
                    Dim lSubsetTimeseriesY As atcTimeseries = SubsetByDate(lTimeseriesY, lSJDay, lEJDay, Nothing)

                    FitLine(lSubsetTimeseriesX, lSubsetTimeseriesY, lACoef, lBCoef, lRSquare, "")
                    Dim lLine As ZedGraph.LineItem = AddLine(lPaneMain, lACoef, lBCoef, Drawing.Drawing2D.DashStyle.Solid)
                    lLine.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)
                    lLine.Line.Width = Trim(lGraphDataset(8))
                    lLine.Label.Text = Trim(lGraphDataset(9))
                    Dim lText As New TextObj
                    Dim lFmt As String = "###,##0.###"
                    lText.Text = "Y = " & DoubleToString(lACoef, , lFmt) & " X + " & DoubleToString(lBCoef, , lFmt) & Environment.NewLine &
                                 "R = " & DoubleToString(Math.Sqrt(lRSquare), , lFmt) & vbCrLf &
                                 "R Squared = " & DoubleToString(lRSquare, , lFmt)
                    lText.FontSpec.StringAlignment = Drawing.StringAlignment.Near

                    lText.Location = New Location(0.05, 0.15, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                    lText.FontSpec.Border.IsVisible = False
                    lPaneMain.GraphObjList.Add(lText)
                    lPaneMain.XAxis.Title.Text &= vbCrLf & vbCrLf & "Scatter Plot"

                Case "45-deg line"
                    Dim lLine As ZedGraph.LineItem = AddLine(lPaneMain, 1, 0, Drawing.Drawing2D.DashStyle.Dot)
                    lLine.Color = Drawing.Color.FromName(Trim(lGraphDataset(5)).ToLower)
                    lLine.Line.Width = Trim(lGraphDataset(8))
                    lLine.Label.Text = Trim(lGraphDataset(9))
            End Select
            lPaneMain.Legend.IsVisible = True

        Next

        lPaneMain.YAxis.Title.FontSpec.IsBold = True
        lPaneMain.YAxis.Title.FontSpec.Size += 8
        lPaneMain.YAxis.Scale.FontSpec.Size += 4
        lPaneMain.YAxis.Scale.FontSpec.IsBold = False

        lPaneMain.Legend.FontSpec.Size += 4
        lPaneMain.Legend.FontSpec.IsBold = True

        lPaneMain.XAxis.Title.FontSpec.IsBold = True
        lPaneMain.XAxis.Title.FontSpec.Size += 8
        lPaneMain.XAxis.Scale.FontSpec.Size += 4
        lPaneMain.XAxis.Scale.FontSpec.IsBold = False
        Return aZgc
    End Function
    Private Function CumulativeProbability(ByVal aTimeseriesgroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl,
                                           ByVal aGraphInit As Object, ByVal aGraphRecords As Object,
                                           ByVal aRecordIndex As Integer) As ZedGraphControl

        Dim lNumberofCurves As Integer = Trim(aGraphInit(2))
        aRecordIndex -= lNumberofCurves
        Dim lNumberOfMainPaneCurves As Integer = 0
        Dim lPaneMain As GraphPane = Nothing

        Dim lCurve As ZedGraph.LineItem = Nothing

        lPaneMain = aZgc.MasterPane.PaneList(0)

        lPaneMain.YAxis.Title.Text = Trim(aGraphInit(3))
        lPaneMain.XAxis.Title.Text = Trim(aGraphInit(4))

        If Not Trim(aGraphInit(9)) = "" Then
            lPaneMain.YAxis.Scale.Min = Trim(aGraphInit(9))
        End If
        If Not Trim(aGraphInit(10)) = "" Then
            lPaneMain.YAxis.Scale.Max = Trim(aGraphInit(10))
        End If

        Return aZgc

    End Function

    Private Function skipLines(ByRef aGraphRecords As ArrayList, ByVal aRecordIndex As Integer, ByVal aListTypeOfGraph As String()) As Integer

        aRecordIndex += 1

        Do Until (aRecordIndex + 1 > aGraphRecords.Count) OrElse
                aListTypeOfGraph.Contains(aGraphRecords(aRecordIndex)(0).ToLower)

            aRecordIndex += 1
        Loop
        Return aRecordIndex
    End Function
End Module
