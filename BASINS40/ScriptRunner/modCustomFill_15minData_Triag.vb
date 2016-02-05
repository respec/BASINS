Imports System.Collections.Specialized
Imports atcUtility
Imports atcData
Imports atcWDM
Imports MapWinUtility
Imports System.Text.RegularExpressions
Imports Microsoft.Office.Interop.Excel

Module modCustom
    'Custom module for filling 15-minutes rain fall data
    'processing daily PRISM temperature data (dail min and max)
    'calculation of Hamon PET
    'this module was run in the TimeseriesUtility program to save memory

    'Private Sub btnCustom_Click(sender As System.Object, e As System.EventArgs) Handles btnCustom.Click
        'functions to call to do the job
        'DataFill_SetPriority:
        '  specify weather station by name to search for nearby stations
        '  and then fill rain data with triangular redistribution
        'DataFill_SetPriority()

        'the following merging function will merge the two WDMs (each contains
        'a portion of the total 29 stations in San Diego region of CA)
        'into one final WDM file,
        '1 for merging WDMs of original raw data
        '2 for merging WDMs of filled data
        'modCustom.MergeDatasetsIntoOneWDM(2)

        'the following function will read in 30yr daily PRISM temperatures
        'for the 29 stations, then put them into WDMs 
        'GetTempAndPET()

        'the following function will merge temperature WDMs into one WDM
        'MergeTempIntoOneWDM()
    'End Sub

    Private pMissingValue As Integer = -999
    Private pAccumulValue As Integer = -998
    Private pTimeStep As Integer = 15
    Private Enum EVQualALERT
        Good1 = 1
        Good2 = 2
        Accum = 80
        Miss = 151
    End Enum
    Public Structure DataFile
        Public Location As String
        Public BeginYear As Integer
        Public EndYear As Integer
    End Structure

    Public Sub GetTempAndPET()
        Dim lPattern As String = "PRISM_(tm\w{2})_early_4kmD1_19810101_20151209_(\d+\.?\d+)_(-\d+\.?\d+)_Interp_((\w+\s?)+)\.csv"
        Dim lDataDir As String = "C:\Projects\DataUpdate\PRISM\Temperature\"
        Dim lWDMName As String = IO.Path.Combine(lDataDir, "Met_PRISM_new.wdm")
        Dim lWDM As New atcWDM.atcDataSourceWDM()
        If Not lWDM.Open(lWDMName) Then Exit Sub
        Dim lDir As New IO.DirectoryInfo(lDataDir)
        Dim lFileInfos As IO.FileInfo() = lDir.GetFiles("*.csv")
        Dim lFileInfo As IO.FileInfo
        Dim lDataListing As New Generic.Dictionary(Of String, SortedDictionary(Of Integer, DataFile))
        Dim lTmaxFiles As New atcCollection()
        Dim lTminFiles As New atcCollection()
        Dim lFileTotalCount As Integer = lFileInfos.Length
        Dim lFileCount As Integer = 1
        For Each lFileInfo In lFileInfos
            Dim lFilename As String = IO.Path.GetFileName(lFileInfo.FullName)
            Dim lMatches As MatchCollection = Nothing
            Dim lFoundMatch As Boolean = True
            Try
                lMatches = Regex.Matches(lFilename, lPattern, RegexOptions.IgnoreCase)
            Catch ex As Exception
                lFoundMatch = False
            End Try
            If Not lFoundMatch Then Continue For
            Dim lStationName As String = lMatches(0).Groups(4).Value
            If Not String.IsNullOrEmpty(lStationName) AndAlso Not String.IsNullOrEmpty(lStationName.Trim()) Then
                Dim ltype As String = lMatches(0).Groups(1).Value
                If ltype = "tmax" Then
                    lTmaxFiles.Add(lStationName, lFilename)
                ElseIf ltype = "tmin" Then
                    lTminFiles.Add(lStationName, lFilename)
                End If
            End If
        Next
        Dim lDefStartDate As Double = Date2J(1981, 1, 1, 0, 0, 0)
        Dim lDefEndDate As Double = Date2J(2015, 12, 9, 24, 0, 0)
        Dim lStartDate As Double = -1
        Dim lEndDate As Double = -1
        Dim lID As Integer = 1
        Dim lMetCmpSrc As New atcMetCmp.atcMetCmpPlugin()
        Dim lCTS() As Double = {0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}
        For Each lStnName As String In lTmaxFiles.Keys
            Logger.Progress("Process Station: " & lStnName, lID, lTmaxFiles.Count * 3)
            Dim lTmaxFName As String = lTmaxFiles.ItemByKey(lStnName)
            Dim lTminFName As String = lTminFiles.ItemByKey(lStnName)
            If String.IsNullOrEmpty(lTminFName) Then Continue For

            'Construct a new timeseries
            Dim lTsMax As atcTimeseries = ReadPRISMTemp(IO.Path.Combine(lDataDir, lTmaxFName), lDefStartDate, lDefEndDate, lID)
            lTsMax.Attributes.SetValue("Location", lStnName)
            lID += 1
            Dim lTsMin As atcTimeseries = ReadPRISMTemp(IO.Path.Combine(lDataDir, lTminFName), lDefStartDate, lDefEndDate, lID)
            lTsMin.Attributes.SetValue("Location", lStnName)
            lID += 1
            lWDM.AddDataset(lTsMax)
            lWDM.AddDataset(lTsMin)

            Dim lat As Double = lTsMax.Attributes.GetValue("Latitude")
            Dim lArg As New atcDataAttributes()
            With lArg
                .SetValue("TMIN", lTsMin)
                .SetValue("TMAX", lTsMax)
                .SetValue("Degrees F", True)
                .SetValue("Latitude", lat)
                .SetValue("Hamon Monthly Coefficients", lCTS)
            End With
            If lMetCmpSrc.Open("Hamon PET", lArg) Then
                Dim lTsPET As atcTimeseries = lMetCmpSrc.DataSets(0)
                Dim lTsPETHr As atcTimeseries = atcMetCmp.DisSolPet(lTsPET, Nothing, 2, lat)
                With lTsPETHr.Attributes
                    .SetValue("Location", lStnName)
                    .SetValue("ID", lID)
                    .SetValue("Constituent", "PET")
                    .SetValue("Description", "Hamon PET from PRISM Temp")
                End With
                lWDM.AddDataset(lTsPETHr)
                lID += 1
                lTsPET.Clear() : lTsPET = Nothing
                lTsPETHr.Clear() : lTsPETHr = Nothing
            Else
                Logger.Msg("Problem calculating PET for station: " & lStnName)
            End If
            lTsMax.Clear() : lTsMax = Nothing
            lTsMin.Clear() : lTsMin = Nothing
        Next
        Logger.Progress(lID, lTmaxFiles.Count * 3)
        Logger.Msg("Done reading PRISM temperature and PET generation.")
    End Sub

    Public Function ReadPRISMTemp(ByVal aFName As String, ByVal aStart As Double, ByVal aEnd As Double, ByVal aID As Integer) As atcTimeseries
        Dim lTable As New atcTableDelimited()
        lTable.NumHeaderRows = 6
        lTable.NumFields = 2
        If Not lTable.OpenFile(aFName) Then
            Return Nothing
        End If

        Dim lHeaders() As String = lTable.Header.Split(Environment.NewLine)
        Dim lat As Double
        Dim longi As Double
        Dim lelev As Double
        Dim lCons As String = ""
        Dim lStart As Double
        Dim lEnd As Double
        Dim lArr() As String = Nothing
        For Each line As String In lHeaders
            lArr = line.Trim().Split(":")
            If line.Trim().StartsWith("Location") Then
                lat = Double.Parse(lArr(2).Substring(0, lArr(2).LastIndexOf(" ")))
                longi = Double.Parse(lArr(3).Substring(0, lArr(3).LastIndexOf(" ")))
                lelev = Double.Parse(lArr(4).Substring(0, lArr(4).LastIndexOf("f")))
            ElseIf line.Trim().StartsWith("Climate variable") Then
                lCons = lArr(1).Trim()
            ElseIf line.Trim().StartsWith("Period") Then
                Dim lDateStr() As String = lArr(1).Split("-")
                'Dim lPattern0 As String = "1981-01-01 - 2015-12-09"
                Dim lPattern As String = "(\d{4})-(\d{2})-(\d{2}) - (\d{4})-(\d{2})-(\d{2})"
                Dim lMatches As MatchCollection = Nothing
                Dim lFoundMatch As Boolean = True
                Try
                    lMatches = Regex.Matches(lArr(1), lPattern, RegexOptions.IgnoreCase)
                Catch ex As Exception
                    lFoundMatch = False
                End Try
                If lFoundMatch Then
                    Dim lcStartYear As Integer = Integer.Parse(lMatches(0).Groups(1).Value)
                    Dim lcStartMonth As Integer = Integer.Parse(lMatches(0).Groups(2).Value)
                    Dim lcStartDay As Integer = Integer.Parse(lMatches(0).Groups(3).Value)
                    Dim lcEndYear As Integer = Integer.Parse(lMatches(0).Groups(4).Value)
                    Dim lcEndMonth As Integer = Integer.Parse(lMatches(0).Groups(5).Value)
                    Dim lcEndDay As Integer = Integer.Parse(lMatches(0).Groups(6).Value)

                    lStart = Date2J(lcStartYear, lcStartMonth, lcStartDay, 0, 0, 0)
                    lEnd = Date2J(lcEndYear, lcEndMonth, lcEndDay, 24, 0, 0)
                End If
            End If
        Next

        Dim lTs As atcTimeseries = Nothing
        If Math.Abs(aStart - lStart) > 0.001 OrElse Math.Abs(aEnd - lEnd) > 0.001 Then
            lTs = NewTimeseries(lStart, lEnd, atcTimeUnit.TUDay, 1)
        Else
            lTs = NewTimeseries(aStart, aEnd, atcTimeUnit.TUDay, 1)
        End If

        With lTs.Attributes
            .SetValue("ID", aID)
            .SetValue("Constituent", lCons)
            .SetValue("Scenario", "PRISM")
            .SetValue("latitude", lat)
            .SetValue("longitude", longi)
            .SetValue("Elev", lelev)
            .SetValue("Description", "PRISM Daily 4km Interpolated degF")
        End With

        Dim lYear, lMonth, lDay As Integer
        Dim lValue As Double
        lTable.CurrentRecord = 1
        For I As Integer = 1 To lTs.numValues
            With lTable
                .CurrentRecord = I
                lArr = .Value(1).Split("-")
                Try
                    lYear = Integer.Parse(lArr(0))
                    lMonth = Integer.Parse(lArr(1))
                    lDay = Integer.Parse(lArr(2))
                    Dim lDatePrism As Double = Date2J(lYear, lMonth, lDay, 24, 0, 0)
                    If Math.Abs(lDatePrism - lTs.Dates.Value(I)) < 0.001 Then
                        lValue = Double.Parse(.Value(2))
                        lTs.Value(I) = lValue
                    Else
                        Logger.Msg("Found a missing data @date: " & lYear & "-" & lMonth & "-" & lDay)
                    End If
                Catch ex As Exception
                    Logger.Msg("Parsing Data Problem.")
                End Try
            End With
        Next
        lTs.Value(0) = GetNaN()
        lTable.Clear()
        lTable = Nothing
        Return lTs
    End Function

    ''' <summary>
    ''' As new temperature station data are read in and PET generated, they are saved in the Temperature folder
    ''' in the Met_PRISM_new.wdm file
    ''' These newly created datasets needs to be incorporated into the master overall temperature wdm file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub MergeTempIntoOneWDM()
        'new data
        Dim lWDMnewDir As String = "C:\Projects\DataUpdate\PRISM\Temperature\"
        Dim lWDMnew As String = "Met_PRISM_new.wdm"
        Dim lWDMFilenew As New atcWDM.atcDataSourceWDM()
        If Not lWDMFilenew.Open(IO.Path.Combine(lWDMnewDir, lWDMnew)) Then Exit Sub

        'master datasets
        Dim lWDMDir As String = "C:\Projects\DataUpdate\PRISM\Temperature_Done\"
        Dim lWDM As String = "Met_PRISM.wdm"
        Dim lWDMFile As New atcWDM.atcDataSourceWDM()
        If Not lWDMFile.Open(IO.Path.Combine(lWDMDir, lWDM)) Then Exit Sub

        'append to the end of the list of datasets sequentially
        Dim lID As Integer = lWDMFile.DataSets.Count + 1
        For Each lTserNew As atcTimeseries In lWDMFilenew.DataSets
            lTserNew.EnsureValuesRead()
            lTserNew.Attributes.SetValue("ID", lID)
            If Not lWDMFile.AddDataset(lTserNew) Then
                Logger.Dbg("Failed writing " & lTserNew.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            lID += 1
        Next
    End Sub

    Public Sub MergeDatasetsIntoOneWDM(ByVal aMode As Integer)
        Dim lWDMFile2 As New atcWDM.atcDataSourceWDM()
        Dim lWDMFile3 As New atcWDM.atcDataSourceWDM()
        Dim lWDMFileAll As New atcWDM.atcDataSourceWDM()
        If aMode = 1 Then
            'merge original
            Dim lWDMDir As String = "C:\Projects\DataUpdate\Deliver\"
            Dim lWDM2 As String = "Met_Haines2.wdm"
            Dim lWDM3 As String = "Met_Haines3.wdm"
            Dim lWDMAll As String = "Met_Haines.wdm"
            If Not lWDMFile2.Open(IO.Path.Combine(lWDMDir, lWDM2)) Then Exit Sub
            If Not lWDMFile3.Open(IO.Path.Combine(lWDMDir, lWDM3)) Then Exit Sub
            If Not lWDMFileAll.Open(IO.Path.Combine(lWDMDir, lWDMAll)) Then Exit Sub
        ElseIf aMode = 2 Then
            'merge filled
            Dim lWDMFilledDir As String = "C:\Projects\DataUpdate\Deliver\Filled\"
            Dim lWDM2filled As String = "Met_Haines2_Filled.wdm"
            Dim lWDM3filled As String = "Met_Haines3_Filled.wdm"
            Dim lWDMAll As String = "Met_Haines_Filled.wdm"
            If Not lWDMFile2.Open(IO.Path.Combine(lWDMFilledDir, lWDM2filled)) Then Exit Sub
            If Not lWDMFile3.Open(IO.Path.Combine(lWDMFilledDir, lWDM3filled)) Then Exit Sub
            If Not lWDMFileAll.Open(IO.Path.Combine(lWDMFilledDir, lWDMAll)) Then Exit Sub
        End If
        Dim lDatasetID As Integer = 1
        For Each lDataset As atcTimeseries In lWDMFile2.DataSets
            lDataset.EnsureValuesRead()
            lDataset.Attributes.SetValue("ID", lDatasetID)
            If Not lWDMFileAll.AddDataset(lDataset) Then
                Logger.Dbg("Failed writing " & lDataset.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            'lDataset.Clear()
            lDatasetID += 1
        Next

        For Each lDataset As atcTimeseries In lWDMFile3.DataSets
            lDataset.EnsureValuesRead()
            lDataset.Attributes.SetValue("ID", lDatasetID)
            If Not lWDMFileAll.AddDataset(lDataset) Then
                Logger.Dbg("Failed writing " & lDataset.Attributes.GetValue("Location") & " into the 1 WDM.")
            End If
            'lDataset.Clear()
            lDatasetID += 1
        Next
        'lWDMFile2.Clear()
        'lWDMFile3.Clear()
        'lWDMFileAll.Clear()
        'lWDMFile2 = Nothing
        'lWDMFile3 = Nothing
        'lWDMFileAll = Nothing
        Logger.Msg("Merge Precip Data Done.")
    End Sub

    Public Sub DataFill_SetPriority()
        Dim lSelectedStations As New atcCollection()
        With lSelectedStations
            '.Add("Bonita")
            '.Add("Descanso")
            '.Add("Encinitas")
            '.Add("Fallbrook AP")
            '.Add("Fashion Valley")
            '.Add("Granite Hills HS")
            '.Add("Kearny Mesa")
            '.Add("Lake Henshaw")
            '.Add("Lake Wohlford")
            '.Add("Oceanside")
            '.Add("Poway")
            '.Add("Ramona")
            '.Add("Morena Lake")
            '.Add("Santa Ysabel")
            .Add("Tierra del Sol")
        End With

        Dim lXLApp As New Application()
        Dim lXLBook As Workbook = Nothing
        Dim lXLSheet As Worksheet = Nothing

        'lXLBook = lXLApp.Workbooks.Open("C:\Projects\DataUpdate\PriorityTable_11302015.xlsx")
        'lXLSheet = lXLBook.Worksheets(1)
        lXLBook = lXLApp.Workbooks.Open("C:\Projects\DataUpdate\PriorityTable_01152016.xlsx")
        lXLSheet = lXLBook.Sheets("AdditionalStations")

        Dim lRows As Integer = lXLSheet.UsedRange.Rows.Count
        Dim lColStaName As Integer = 1
        Dim lColStaPriority As Integer = 4
        Dim lColRank As Integer = 8
        Dim lColMultiplier As Integer = 9
        Dim lColHistParamTarget As Integer = 2
        Dim lColHistParamNeighbor As Integer = 5

        Dim lStaName As String = ""
        Dim lStaNeighbor As String = ""
        Dim lRank As Integer = 0
        Dim lMultiplier As Double = 1.0
        Dim lHistParamTarget As Double = -999
        Dim lHistParamNeighbor As Double = -999

        Logger.Status("Setup Neighboring Gage Network...")
        Dim lTargetStations As New atcCollection()
        For lRow As Integer = 2 To lRows
            Logger.Progress(lRow, lRows)
            With lXLSheet
                lStaName = .Cells(lRow, lColStaName).Value
                If Not lSelectedStations.Contains(lStaName) Then
                    Continue For
                End If
                lStaNeighbor = .Cells(lRow, lColStaPriority).Value
                lRank = Integer.Parse(.Cells(lRow, lColRank).Value)
                lMultiplier = Double.Parse(.Cells(lRow, lColMultiplier).Value)
                lHistParamTarget = Double.Parse(.Cells(lRow, lColHistParamTarget).Value)
                lHistParamNeighbor = Double.Parse(.Cells(lRow, lColHistParamNeighbor).Value)
                Dim lTargetStation As Gage = lTargetStations.ItemByKey(lStaName)
                If lTargetStation Is Nothing Then
                    lTargetStation = New Gage()
                    lTargetStation.Name = lStaName
                    lTargetStation.DataPrecipHistParamValue = lHistParamTarget
                    lTargetStation.Neighbors = New SortedList(Of Integer, Gage)
                    lTargetStations.Add(lStaName, lTargetStation)
                End If
                If Not lTargetStation.Neighbors.ContainsKey(lRank) Then
                    Dim lnewNeighborGage As New Gage()
                    lnewNeighborGage.Name = lStaNeighbor
                    lnewNeighborGage.RankPrecip = lRank
                    lnewNeighborGage.DataPrecipMultiplier = lMultiplier
                    lnewNeighborGage.DataPrecipHistParamValue = lHistParamNeighbor
                    lTargetStation.Neighbors.Add(lRank, lnewNeighborGage)
                End If
            End With
        Next
        lXLBook.Close()
        lXLApp.Quit()
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLSheet)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLBook)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(lXLApp)
        GC.Collect()

        Logger.Status("Locate Rainfall Data for gaging stations...")
        Dim lWDMDir As String = "C:\Projects\DataUpdate\Deliver\"
        Dim lWDMFilledDir As String = "C:\Projects\DataUpdate\Deliver\Filled\"

        Dim lWDM2 As String = "Met_Haines2.wdm"
        Dim lWDM3 As String = "Met_Haines3.wdm"
        Dim lWDM2filled As String = "Met_Haines2_Filled.wdm"
        Dim lWDM3filled As String = "Met_Haines3_Filled.wdm"

        Dim lWDMFile2 As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMFile2.Open(IO.Path.Combine(lWDMDir, lWDM2)) Then Exit Sub
        Dim lWDMFile3 As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM()
        If Not lWDMFile3.Open(IO.Path.Combine(lWDMDir, lWDM3)) Then Exit Sub

        For Each lGageTarget As Gage In lTargetStations
            If SetDataSpec(lGageTarget, lWDMFile2, lWDMFile3) Then
                If lGageTarget.DataPrecipWDM.Contains("2") Then
                    lGageTarget.DataPrecipWDMFilled = IO.Path.Combine(lWDMFilledDir, lWDM2filled)
                Else
                    lGageTarget.DataPrecipWDMFilled = IO.Path.Combine(lWDMFilledDir, lWDM3filled)
                End If
            End If
            For Each lGageNeighbor As Gage In lGageTarget.Neighbors.Values
                SetDataSpec(lGageNeighbor, lWDMFile2, lWDMFile3)
            Next
        Next

        DataFill_Fill(lTargetStations)
        Logger.Status("HIDE")
    End Sub

    Private Function SetDataSpec(ByRef aGage As Gage, ByVal aWDM1 As atcWDM.atcDataSourceWDM, ByVal aWDM2 As atcWDM.atcDataSourceWDM)
        Dim gageLoc As String = aGage.Name
        Dim lFound As Boolean = False
        For Each lTs As atcTimeseries In aWDM1.DataSets
            Dim loc As String = lTs.Attributes.GetValue("Location")
            If gageLoc.ToLower.StartsWith(loc.ToLower) Then
                aGage.DataPrecipWDM = aWDM1.Specification
                aGage.DataPrecipDatasetID = lTs.Attributes.GetValue("ID")
                lFound = True
                Exit For
            End If
        Next

        If Not lFound Then
            For Each lTs As atcTimeseries In aWDM2.DataSets
                Dim loc As String = lTs.Attributes.GetValue("Location")
                If gageLoc.ToLower.StartsWith(loc.ToLower) Then
                    aGage.DataPrecipWDM = aWDM2.Specification
                    aGage.DataPrecipDatasetID = lTs.Attributes.GetValue("ID")
                    lFound = True
                    Exit For
                End If
            Next
        End If

        Return lFound
    End Function

    Private Sub DataFill_Fill(ByVal aTargetStations As atcCollection)
        Logger.Dbg("TongZhai: ESA Fill Data Starts")
        Logger.Status("Filling Station Precipitation Data...")

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format

        Dim pMaxNearStas As Integer = 28
        Dim pMaxFillLength As Integer = 11 * 4 'any span < max time shift (10 hrs for HI)
        'Dim pMinNumHrly As Integer = 43830 '5 years of hourly values
        'Dim pMinNumDly As Integer = 1830 '5 years of daily
        Dim pMinNum As Integer = 1830 * 24 * 4 '5 years of daily
        Dim pMaxPctMiss As Integer = 50 '20

        Dim lStationCount As Integer = 1
        For Each lGage As Gage In aTargetStations
            Logger.Progress("Fill Station " & lGage.Name, lStationCount, aTargetStations.Count)
            Dim lTargetTser As atcTimeseries = lGage.GetData("PREC")
            If lTargetTser Is Nothing Then
                Continue For
            Else
                lTargetTser.Attributes.SetValue(lGage.DataPrecipHistParam, lGage.DataPrecipHistParamValue)
            End If
            Dim lAddMe As Boolean = False
            Dim lStr As String = MissingDataSummary(lTargetTser, lMVal, lMAcc, lFMin, lFMax, lRepType)
            Dim lPctMiss As Double = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
            If Not (lPctMiss > 0) Then Continue For
            If lPctMiss < pMaxPctMiss Then '% missing OK
                If lTargetTser.numValues > pMinNum Then
                    Logger.Dbg("Filling data for " & lGage.Name)
                    lAddMe = True
                Else
                    Logger.Dbg("Not enough values (" & lTargetTser.numValues & ") for " & lGage.Name & " - need at least " & pMinNum)
                End If
            Else
                Logger.Dbg("For " & lGage.Name & ", percent Missing (" & lPctMiss & ") too large (> " & pMaxPctMiss & ")")
            End If
            If Not lAddMe Then Continue For

            'Actual filling
            Dim lFillers As New atcCollection()
            Dim lNeighborStnCount As Integer = 0
            For Each i As Integer In lGage.Neighbors.Keys
                Dim lNGage As Gage = lGage.Neighbors.Item(i)
                Dim lNTser As atcTimeseries = lNGage.GetData("PREC")
                If lNTser IsNot Nothing Then
                    lNTser.Attributes.SetValue(lNGage.DataPrecipHistParam, lNGage.DataPrecipHistParamValue)
                    'lNTser.Attributes.SetValue("ModifierAttributeName", lNGage.DataPrecipModifierAttributeName)
                    'lNTser.Attributes.SetValue(lNGage.DataPrecipModifierAttributeName, lNGage.DataPrecipMultiplier)
                    lFillers.Add(lNGage.RankPrecip, lNTser)
                    lNeighborStnCount += 1
                End If
                If lNeighborStnCount = pMaxNearStas Then
                    Exit For
                End If
            Next

            Dim lFilledTS As atcTimeseries = Nothing
            If lFillers.Count > 0 Then
                Logger.Dbg("FillMissing:  Found " & lFillers.Count & " nearby stations for filling")
                Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                'lFilledTS = lGage.GetData("PREC", True) 'lTargetTser.Clone()
                'If lFilledTS IsNot Nothing Then
                '    lFilledTS.Attributes.SetValue(lGage.DataPrecipHistParam, lGage.DataPrecipHistParamValue)
                'End If
                lFilledTS = lTargetTser.Clone()
                If lTargetTser.Attributes.GetValue("TU") = atcTimeUnit.TUHour OrElse
                   lTargetTser.Attributes.GetValue("TU") = atcTimeUnit.TUMinute Then
                    If lPctMiss > 0 Then
                        Dim lArgs As New atcDataAttributes()
                        lArgs.Add("AvailableRanked", True)
                        lArgs.Add("Roundoff", 0.01)
                        lArgs.SetValue("UseTriagDistribution", True)
                        'lArgs.Add("ModifierAttributeName", CType(lFillers.ItemByIndex(0), atcTimeseries).Attributes.GetValue("ModifierAttributeName", ""))
                        FillHourlyTser(lFilledTS, lFillers, lMVal, lMAcc, 90, lArgs)
                    Else
                        Logger.Dbg("FillMissing:  All Missing periods filled via interpolation")
                    End If
                Else 'daily tser
                    FillDailyTser(lFilledTS, Nothing, lFillers, Nothing, lMVal, lMAcc, 90)
                End If
                lStr = MissingDataSummary(lFilledTS, lMVal, lMAcc, lFMin, lFMax, lRepType)
                lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
            Else
                Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
            End If

            'write filled data set to new WDM file
            lGage.SaveData(lFilledTS)
            lFilledTS.Clear()
            lTargetTser.Clear()
            For Each lTs As atcTimeseries In lFillers
                lTs.Clear()
            Next
            'If lFilledTS IsNot Nothing Then
            '    If lNewWDMfile.AddDataset(lFilledTS) Then
            '        Logger.Dbg("FillMissing:  Added " & lCons & " dataset to WDM file for station " & lStation)
            '    Else
            '        Logger.Dbg("FillMissing:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
            '    End If
            '    lFilledTS.Clear()
            'End If
            'lts.ValuesNeedToBeRead = True
            lStationCount += 1
        Next
        Logger.Status("HIDE")
        Logger.Msg("Finished Fill Missing Data.")
    End Sub

    Public Class Gage
        Public Name As String = ""
        Public Latitude As Double = 0.0
        Public Longitude As Double = 0.0
        Public RankPrecip As Integer = 1
        Public DataPrecip As atcTimeseries = Nothing
        Public Neighbors As SortedList(Of Integer, Gage)
        Public DataPrecipWDM As String = ""
        Public DataPrecipDatasetID As Integer = -99
        Public DataPrecipWDMFilled As String = ""
        Public DataPrecipHistParam As String = "PRECIP"
        Public DataPrecipHistParamValue As Double = -999
        Public DataPrecipModifierAttributeName As String = "Multiplier"
        Public DataPrecipMultiplier As Double = 1.0

        Public Function GetData(ByVal aCons As String, Optional ByVal aGetFromFilled As Boolean = False) As atcTimeseries
            If String.IsNullOrEmpty(aCons) Then Return Nothing

            Dim lDataset As atcTimeseries = Nothing
            Dim lWDMFilename As String = DataPrecipWDM
            If aGetFromFilled Then lWDMFilename = DataPrecipWDMFilled
            Select Case aCons
                Case "PREC"
                    If Not String.IsNullOrEmpty(lWDMFilename) AndAlso DataPrecipDatasetID > 0 Then
                        Dim lWDM As atcWDM.atcDataSourceWDM = atcDataManager.DataSourceBySpecification(lWDMFilename)
                        Dim lNeedToReopen As Boolean = False
                        If lWDM Is Nothing OrElse lWDM.DataSets Is Nothing Then
                            lNeedToReopen = True
                        Else
                            With lWDM
                                For I As Integer = 0 To .DataSets.Count - 1
                                    If .DataSets(I).Attributes.GetValue("ID") Is Nothing Then
                                        lWDM.Clear()
                                        atcDataManager.RemoveDataSource(lWDM)
                                        lNeedToReopen = True
                                        Exit For
                                    End If
                                Next
                            End With
                        End If
                        If lNeedToReopen Then
                            lWDM = New atcWDM.atcDataSourceWDM()
                            If lWDM.Open(lWDMFilename) Then
                                atcDataManager.DataSources.Add(lWDM)
                            Else
                                Return Nothing
                            End If
                        End If
                        lDataset = lWDM.DataSets.FindData("ID", DataPrecipDatasetID)(0)
                    End If
            End Select
            Return lDataset
        End Function

        Public Function SaveData(ByVal aTser As atcTimeseries, Optional ByVal aSaveToFilled As Boolean = True) As Boolean
            If aTser Is Nothing Then Return False

            Dim lDataset As atcTimeseries = Nothing
            Dim lWDMFilename As String = DataPrecipWDMFilled
            If Not aSaveToFilled Then lWDMFilename = DataPrecipWDM
            If Not String.IsNullOrEmpty(lWDMFilename) AndAlso DataPrecipDatasetID > 0 Then
                Dim lWDM As atcWDM.atcDataSourceWDM = atcDataManager.DataSourceBySpecification(lWDMFilename)
                If lWDM Is Nothing Then
                    lWDM = New atcWDM.atcDataSourceWDM()
                    If lWDM.Open(lWDMFilename) Then
                        atcDataManager.DataSources.Add(lWDM)
                    Else
                        Return False
                    End If
                End If
                Dim lMsg As String = ""
                If lWDM.AddDataset(aTser) Then
                    If aSaveToFilled Then
                        lMsg = "FillMissing: Done for station: " & Name
                    Else
                        lMsg = "Saved Original Data for station: " & Name
                    End If
                    Logger.Dbg(lMsg)
                    Return True
                Else
                    If aSaveToFilled Then
                        lMsg = "FillMissing: failed for station: " & Name
                    Else
                        lMsg = "Saved Original Data failed for station: " & Name
                    End If
                    Logger.Dbg(lMsg)
                    Return False
                End If
                'aTser.Clear()
            Else
                Return False
            End If
        End Function
    End Class
End Module
