Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System
Imports System.Data
Imports Microsoft.VisualBasic

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcTimeseriesBinary
Imports SwatObject

Module SWATRunner
    Private Const pRefreshDB As Boolean = False
    Private Const pOutputOnly As Boolean = True
    'Private Const pBasePath As String = "D:\Basins\data\SWATOutput\UM\baseline90"
    Private Const pBasePath As String = "C:\Project\UMRB\baseline90"
    'Private Const pInputPath As String = "D:\Basins\data\SWATOutput\UM\baseline90jack"
    Private Const pInputPath As String = "C:\Project\UMRB\baseline90\Scenarios\Test"
    'Private Const pSWATGDB As String = "SWAT2005.mdb"
    Private Const pSWATGDB As String = "C:\Program Files\SWAT\ArcSWAT\Databases\SWAT2005.mdb"
    Private Const pOutGDBPath As String = pInputPath & "\TablesIn"
    Private Const pOutGDB As String = "baseline90.mdb"
    Private Const pOutputFolder As String = pInputPath & "\TxtInOut"
    Private Const pReportsFolder As String = pInputPath & "\TablesOut"
    Private Const pSWATExe As String = pOutputFolder & "\swat2005.exe" 'local copy with input data
    'Private Const pSWATExe As String = "C:\Program Files\SWAT 2005 Editor\swat2005.exe"
    Private pSwatOutput As Text.StringBuilder
    Private pSwatError As Text.StringBuilder

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputPath)
        Dim lLogFileName As String = Logger.FileName

        'log for swat runner
        Logger.StartToFile(pInputPath & "\logs\SWATRunner.log", , , True)

        Dim lOutGDB As String = pOutGDBPath & "\" & pOutGDB

        If Not pOutputOnly Then
            If pRefreshDB Then 'copy the entire input parameter database for this new scenario
                If IO.File.Exists(lOutGDB) Then
                    Logger.Dbg("DeleteExisting " & lOutGDB)
                    IO.File.Delete(lOutGDB)
                End If
                IO.File.Copy(pBasePath & "\" & pOutGDB, lOutGDB)
            End If

            Logger.Dbg("InitializeSwatInput")
            SwatInput.Initialize(pSWATGDB, lOutGDB, pOutputFolder)

            SwatInput.Hru.TableCreate()

            Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
            For Each lString As String In LinesInFile("SWATParmChanges.txt")
                Dim lParms() As String = lString.Split(";")
                SwatInput.UpdateInputDB(lParms(0).Trim, lParms(1).Trim, lParms(2).Trim, lParms(3).Trim, lParms(4).Trim)
            Next

            Logger.Dbg("SWATSummarizeInput")
            Dim lStreamWriter As New IO.StreamWriter(pInputPath & "\logs\LandUses.txt")
            Dim lUniqueLandUses As DataTable = SwatInput.Hru.UniqueValues("LandUse")
            For Each lLandUse As DataRow In lUniqueLandUses.Rows
                lStreamWriter.WriteLine(lLandUse.Item(0).ToString)
            Next
            lStreamWriter.Close()

            Dim lLandUSeTable As DataTable = AggregateCrops(SwatInput.SubBasin.TableWithArea("LandUse"))
            SaveFileString(pInputPath & "\logs\AreaLandUseReport.txt", _
                           SWATArea.Report(lLandUSeTable))
            SaveFileString(pInputPath & "\logs\AreaSoilReport.txt", _
                           SWATArea.Report(SwatInput.SubBasin.TableWithArea("Soil")))
            SaveFileString(pInputPath & "\logs\AreaSlopeCodeReport.txt", _
                           SWATArea.Report(SwatInput.SubBasin.TableWithArea("Slope_Cd")))

            LaunchProgram(pSWATExe, pOutputFolder)
        End If

        MkDirPath(IO.Path.GetFullPath(pReportsFolder))
        Dim lOutputRch As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputRch
            .Open(pOutputFolder & "\output.rch")
            Logger.Dbg("OutputRchTimserCount " & .DataSets.Count)
            WriteDatasets(pReportsFolder & "\rch", .DataSets)
        End With

        Dim lOutputSub As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputSub
            .Open(pOutputFolder & "\output.sub")
            Logger.Dbg("OutputSubTimserCount " & .DataSets.Count)
            WriteDatasets(pReportsFolder & "\sub", .DataSets)
        End With

        Dim lOutputFields As New atcData.atcDataAttributes
        lOutputFields.SetValue("FieldName", "AREAkm2;YLDt/ha")
        Dim lOutputHru As New atcTimeseriesSWAT.atcTimeseriesSWAT
        With lOutputHru
            '.Open(pOutputFolder & "\tab.hru", lOutputFields)
            .Open(pOutputFolder & "\output.hru", lOutputFields)
            Logger.Dbg("OutputHruTimserCount " & .DataSets.Count)
            WriteDatasets(pReportsFolder & "\hru", .DataSets)
        End With

        Logger.Dbg("SwatSummaryReport")
        Dim lTimseriesGroup As New atcDataSourceTimeseriesBinary
        lTimseriesGroup.Open(pReportsFolder & "\hru.tsbin")
        WriteSummary(pReportsFolder, lTimseriesGroup.DataSets)

        Logger.Dbg("SwatPostProcessingDone")

        'back to basins log
        Logger.StartToFile(lLogFileName, True, False, True)
    End Sub

    Private Sub WriteDatasets(ByVal aFileName As String, ByVal aDatasets As atcDataGroup)
        Dim lDataTarget As New atcDataSourceTimeseriesBinary ' atcDataSourceWDM
        Dim lFileName As String = aFileName & ".tsbin" 'lDataTarget.Filter.?) Then
        TryDelete(lFileName)
        If lDataTarget.Open(lFileName) Then
            lDataTarget.AddDatasets(aDatasets)
        End If
    End Sub

    Private Sub WriteSummary(ByVal aOutputFolder As String, ByVal aTimeseriesGroup As atcDataGroup)
        Dim lCropIds As New atcCollection
        With lCropIds
            .Add("CORN") : .Add("CCCC") : .Add("CSC1") : .Add("CSS1") : .Add("CCS1")
        End With

        Dim lTab As String = vbTab
        Dim lFieldWidth As Integer = 12
        Dim lSigDigits As Integer = 8
        Dim lArea As Double = 0.0
        Dim lUnitYield As Double = 0.0

        Dim lSBAreaDebug As New Text.StringBuilder
        lSBAreaDebug.AppendLine("SubId" & lTab & _
                                "HruId" & lTab & _
                                "Crop" & lTab & _
                                "Area".PadLeft(lFieldWidth) & lTab & _
                                "Fraction".PadLeft(lFieldWidth))
        Dim lSBDebug As New Text.StringBuilder
        lSBDebug.AppendLine("SubId" & lTab & _
                            "Crop" & lTab & _
                            "Year" & lTab & _
                            "Area".PadLeft(lFieldWidth) & lTab & _
                            "UnitYield".PadLeft(lFieldWidth) & lTab & _
                            "Yield".PadLeft(lFieldWidth))
        Dim lSBAnnual As New Text.StringBuilder
        lSBAnnual.AppendLine("SubId" & lTab & _
                             "Year" & lTab & _
                             "Area".PadLeft(lFieldWidth) & lTab & _
                             "CornArea".PadLeft(lFieldWidth) & lTab & _
                             "%".PadLeft(lFieldWidth) & lTab & _
                             "UnitYield".PadLeft(lFieldWidth) & lTab & _
                             "Yield".PadLeft(lFieldWidth))
        Dim lSBAverage As New Text.StringBuilder
        lSBAverage.AppendLine("SubId" & lTab & _
                              "Area".PadLeft(lFieldWidth) & lTab & _
                              "CornArea".PadLeft(lFieldWidth) & lTab & _
                              "%".PadLeft(lFieldWidth) & lTab & _
                              "UnitYield".PadLeft(lFieldWidth) & lTab & _
                              "Yield".PadLeft(lFieldWidth))
        Dim lSBTotal As New Text.StringBuilder
        lSBTotal.AppendLine("Area".PadLeft(lFieldWidth) & lTab & _
                            "CornArea".PadLeft(lFieldWidth) & lTab & _
                            "%".PadLeft(lFieldWidth) & lTab & _
                            "UnitYield".PadLeft(lFieldWidth) & lTab & _
                            "Yield".PadLeft(lFieldWidth))

        Dim lAreaGroup As atcDataGroup = aTimeseriesGroup.FindData("Constituent", "AREA")
        Dim lSubIds As atcCollection = lAreaGroup.SortedAttributeValues("SubId")
        Dim lSubIdAreas As New atcCollection
        For Each lSubId As String In lSubIds
            Dim lAreaSubIdTotal As Double = 0.0
            Dim lSubIdDataGroup As atcDataGroup = lAreaGroup.FindData("SubId", lSubId)
            Dim lHruIds As atcCollection = lSubIdDataGroup.SortedAttributeValues("HruId")
            Dim lAreaStrings As New atcCollection
            For Each lHruId As String In lHruIds
                Dim lHruIdDataGroup As atcDataGroup = lSubIdDataGroup.FindData("HruId", lHruId)
                Dim lAreaUsed As Boolean = False
                For Each lAreaTimeseries As atcTimeseries In lHruIdDataGroup
                    lArea = lAreaTimeseries.Value(1)
                    If Double.IsNaN(lArea) Then
                        'skip
                    ElseIf Not lAreaUsed Then
                        lAreaStrings.Add(lHruId, lSubId & lTab & _
                                                 lAreaTimeseries.Attributes.GetValue("HruId") & lTab & _
                                                 lAreaTimeseries.Attributes.GetValue("CropId") & lTab & _
                                                 DecimalAlign(lArea, , , lSigDigits))
                        lAreaSubIdTotal += lArea
                        lAreaUsed = True
                    Else
                        Logger.Dbg("Problem " & lHruId)
                    End If
                Next
            Next
            For Each lAreaString As String In lAreaStrings
                lArea = lAreaString.Substring(lAreaString.LastIndexOf(lTab))
                lSBAreaDebug.AppendLine(lAreaString & lTab & DecimalAlign(lArea / lAreaSubIdTotal, , 10, lSigDigits))
            Next
            lSubIdAreas.Add(lSubId, lAreaSubIdTotal)
        Next
        SaveFileString(aOutputFolder & "\Area.txt", lSBAreaDebug.ToString)

        Dim lMatchingDataGroup As atcDataGroup = aTimeseriesGroup.FindData("CropId", lCropIds)
        Dim lTimserBase As atcTimeseries = lMatchingDataGroup.Item(0)
        Dim lDateBase(5) As Integer
        J2Date(lTimserBase.Dates.Value(0), lDateBase)
        Dim lNumValues As Integer = lTimserBase.numValues

        Dim lAreaAllTotal As Double = 0.0
        Dim lAreaTotal As Double = 0.0
        Dim lYieldTotal As Double = 0.0
        For Each lSubId As String In lSubIds
            Dim lSubIdDataGroup As atcDataGroup = lMatchingDataGroup.FindData("SubId", lSubId)
            Dim lLocationIdsInSub As atcCollection = lSubIdDataGroup.SortedAttributeValues("Location")
            Dim lYieldSum As Double = 0.0
            Dim lAreaSum As Double = 0.0
            Dim lYear As Integer = lDateBase(0)
            Dim lSubIdArea As Double = lSubIdAreas.ItemByKey(lSubId)
            lAreaAllTotal += lSubIdArea
            For lYearIndex As Integer = 1 To lNumValues
                Dim lAreaSub As Double = 0
                Dim lYieldSub As Double = 0
                For Each lLocationId As String In lLocationIdsInSub
                    Dim lLocationIdDataGroup As atcDataGroup = lSubIdDataGroup.FindData("Location", lLocationId)
                    If lLocationIdDataGroup.Count = 2 Then
                        Dim lAreaTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "Area").Item(0)
                        Dim lYieldTimser As atcTimeseries = lLocationIdDataGroup.FindData("Constituent", "Yld").Item(0)

                        If lYearIndex <= lAreaTimser.numValues Then
                            lArea = lAreaTimser.Value(lYearIndex)
                            lUnitYield = lYieldTimser.Value(lYearIndex)
                        Else
                            lArea = Double.NaN
                            lUnitYield = Double.NaN
                        End If
                        Dim lYield As Double = lUnitYield * lArea
                        lSBDebug.AppendLine(lSubId.Trim & lTab & _
                                            lLocationId & lTab & _
                                            lYear & lTab & _
                                            DecimalAlign(lArea) & lTab & _
                                            DecimalAlign(lUnitYield) & lTab & _
                                            DecimalAlign(lYield))
                        If Not Double.IsNaN(lArea) Then
                            lAreaSub += lArea
                            lYieldSub += lYield
                        End If
                    Else
                        Logger.Dbg("Problem:" & lLocationIdDataGroup.Count)
                    End If
                Next
                lSBAnnual.AppendLine(lSubId.Trim & lTab & _
                                     lYear & lTab & _
                                     DecimalAlign(lSubIdArea) & lTab & _
                                     DecimalAlign(lAreaSub) & lTab & _
                                     DecimalAlign(100 * lAreaSub / lSubIdArea, , 1) & lTab & _
                                     DecimalAlign(lYieldSub / lAreaSub) & lTab & _
                                     DecimalAlign(lYieldSub))
                lYieldSum += lYieldSub
                lAreaSum += lAreaSub
                lYear += 1
            Next
            Dim lAreaAvg As Double = lAreaSum / lNumValues
            Dim lYieldAvg As Double = lYieldSum / lNumValues
            lSBAverage.AppendLine(lSubId.Trim & lTab & _
                                  DecimalAlign(lSubIdArea) & lTab & _
                                  DecimalAlign(lAreaAvg) & lTab & _
                                  DecimalAlign(100 * lAreaAvg / lSubIdArea, , 1) & lTab & _
                                  DecimalAlign(lYieldAvg / lAreaAvg) & lTab & _
                                  DecimalAlign(lYieldAvg))
            lAreaTotal += lAreaAvg
            lYieldTotal += lYieldAvg
        Next
        lSBTotal.AppendLine(DecimalAlign(lAreaAllTotal) & lTab & _
                            DecimalAlign(lAreaTotal) & lTab & _
                            DecimalAlign(100 * lAreaTotal / lAreaAllTotal, , 1) & lTab & _
                            DecimalAlign(lYieldTotal / lAreaTotal) & lTab & _
                            DecimalAlign(lYieldTotal))
        SaveFileString(aOutputFolder & "\Debug.txt", lSBDebug.ToString)
        SaveFileString(aOutputFolder & "\Annual.txt", lSBAnnual.ToString)
        SaveFileString(aOutputFolder & "\Average.txt", lSBAverage.ToString)
        SaveFileString(aOutputFolder & "\Total.txt", lSBTotal.ToString)
    End Sub
End Module

Module SWATArea
    Public Function Report(ByVal aReportTable As DataTable) As String
        Dim lFormat As String = "###,##0.00"

        With aReportTable
            Dim lAreaTotals(.Columns.Count) As Double
            Dim lSb As New Text.StringBuilder
            Dim lStr As String = ""
            For lColumnIndex As Integer = 0 To .Columns.Count - 1
                lStr &= .Columns(lColumnIndex).ColumnName.PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            For lRowIndex As Integer = 0 To .Rows.Count - 1
                Dim lReportRow As DataRow = .Rows(lRowIndex)
                With lReportRow
                    lStr = .Item(0).ToString.PadLeft(12) & vbTab
                    For lColumnIndex As Integer = 1 To .ItemArray.GetUpperBound(0)
                        lStr &= DoubleToString(.Item(lColumnIndex), 12, lFormat, , , 10).PadLeft(12) & vbTab
                        lAreaTotals(lColumnIndex) += .Item(lColumnIndex)
                    Next
                End With
                lSb.AppendLine(lStr.TrimEnd(vbTab))
            Next
            lStr = "Totals".PadLeft(12) & vbTab
            For lColumnIndex As Integer = 1 To .Columns.Count - 1
                lStr &= DoubleToString(lAreaTotals(lColumnIndex), 12, lFormat, , , 10).PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            Logger.Dbg("AreaTotalReportComplete " & lAreaTotals(1))
            Return lSb.ToString
        End With
    End Function

    Public Function AggregateCrops(ByVal aInputTable As DataTable) As DataTable
        Dim lArea As Double = 0.0
        Dim lCornFraction As New atcCollection
        lCornFraction.Add("CCCC", 1.0)
        lCornFraction.Add("CCS1", 0.66667)
        lCornFraction.Add("CSC1", 0.5)
        lCornFraction.Add("CSS1", 0.33333)
        lCornFraction.Add("SCC1", 0.66667)
        lCornFraction.Add("SCS1", 0.5)
        lCornFraction.Add("SSC1", 0.33333)
        lCornFraction.Add("SSSC", 0.0)

        Dim lOutputTable As DataTable = aInputTable.Copy
        Dim lCornColumnIndex As Integer = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("CORN")
        Dim lSoybColumnIndex As Integer = lOutputTable.Columns.Count
        lOutputTable.Columns.Add("SOYB")

        For Each lRow As DataRow In lOutputTable.Rows
            lRow(lCornColumnIndex) = 0.0
            lRow(lSoybColumnIndex) = 0.0
            For lColumnIndex As Integer = 2 To lOutputTable.Columns.Count - 2
                Dim lColumnName As String = lOutputTable.Columns(lColumnIndex).ColumnName
                Dim lColumnKeyIndex As Integer = lCornFraction.IndexFromKey(lColumnName)
                If lColumnKeyIndex >= 0 Then
                    lArea = lRow(lColumnIndex)
                    lRow(lCornColumnIndex) += lArea * lCornFraction(lColumnKeyIndex)
                    lRow(lSoybColumnIndex) += lArea * (1 - lCornFraction(lColumnKeyIndex))
                End If
            Next
        Next
        Return lOutputTable
    End Function
End Module
