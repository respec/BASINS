Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System
Imports System.Data
Imports System.Collections.ObjectModel
Imports Microsoft.VisualBasic

Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcTimeseriesBinary
Imports SwatObject

Module SWATRunner
    Private pRefreshDB As Boolean = True ' make a copy of the SWATInput database
    Private pUpdateMDB As Boolean = False ' save changes to table values in the SWATInput database
    Private pOutputSummarize As Boolean = True
    Private pInputSummarize As Boolean = True
    Private pRunModel As Boolean = True
    Private pScenario As String = "RevCrop"
    Private pDrive As String = "G:"
    Private pBaseFolder As String = pDrive & "\Project\UMRB\baseline90"
    Private pSWATGDB As String = "c:\Program Files\SWAT 2005 Editor\Databases\SWAT2005.mdb"
    Private pInputFolder As String = pBaseFolder & "\Scenarios\" & pScenario
    Private pOutGDBFolder As String = pInputFolder & "\TablesIn"
    Private pOutGDB As String = "baseline90.mdb"
    Private pOutputFolder As String = pInputFolder & "\TxtInOut"
    Private pReportsFolder As String = pInputFolder & "\TablesOut"
    Private pSWATExe As String = pOutputFolder & "\swat2005.exe" 'local copy with input data
    Private pParmChangesTextfile As String = "SWATParmChanges.txt"
    Friend pFormat As String = "###,##0.00"
    'Private Const pSWATExe As String = "C:\Program Files\SWAT 2005 Editor\swat2005.exe"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pInputFolder)
        Dim lLogFileName As String = Logger.FileName

        'log for swat runner
        Logger.StartToFile(pInputFolder & "\logs\SWATRunner.log", , , True)

        Dim lOutGDB As String = pOutGDBFolder & "\" & pOutGDB
        If pRefreshDB OrElse Not IO.File.Exists(lOutGDB) Then 'copy the entire input parameter database for this new scenario
            If IO.File.Exists(lOutGDB) Then
                Logger.Dbg("DeleteExisting " & lOutGDB)
                IO.File.Delete(lOutGDB)
            End If
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lOutGDB))
            IO.File.Copy(pBaseFolder & "\" & pOutGDB, lOutGDB)
            Logger.Dbg("Copied " & lOutGDB & " from " & pBaseFolder)
        End If

        Logger.Dbg("InitializeSwatInput")
        Dim lSwatInput As New SwatInput(pSWATGDB, lOutGDB, pBaseFolder, pScenario)

        Dim lCIOTable As DataTable = lSwatInput.CIO.Table

        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Start Year", lCIOTable.Rows(0).Item("IYR"))
            .Add("Number of Years", lCIOTable.Rows(0).Item("NBYR"))
            .Add("Run Model", pRunModel)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("User Specified Parameters", lUserParms) Then
            If pUpdateMDB Then
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "IYR", lUserParms.ItemByKey("Start Year"))
                lSwatInput.UpdateInputDB("CIO", "OBJECTID", 1, "NBYR", lUserParms.ItemByKey("Number of Years"))
            End If
            lCIOTable.Rows(0).Item("IYR") = lUserParms.ItemByKey("Start Year")
            lCIOTable.Rows(0).Item("NBYR") = lUserParms.ItemByKey("Number of Years")
            lSwatInput.CIO.Save(False, lCIOTable)

            Dim lUniqueLandUses As DataTable = lSwatInput.Hru.UniqueValues("LandUse")
            If pInputSummarize Then
                'TODO: why do we need to make a new HRU table?
                'lSwatInput.Hru.TableCreate()

                Logger.Dbg("SWATSummarizeInput")
                Dim lStreamWriter As New IO.StreamWriter(pInputFolder & "\logs\LandUses.txt")
                For Each lLandUse As DataRow In lUniqueLandUses.Rows
                    lStreamWriter.WriteLine(lLandUse.Item(0).ToString)
                Next
                lStreamWriter.Close()

                Dim lLandUSeTable As DataTable = AggregateCrops(lSwatInput.SubBsn.TableWithArea("LandUse"))
                SaveFileString(pInputFolder & "\logs\AreaLandUseReport.txt", _
                               SWATArea.Report(lLandUSeTable))
                SaveFileString(pInputFolder & "\logs\AreaSoilReport.txt", _
                               SWATArea.Report(lSwatInput.SubBsn.TableWithArea("Soil")))
                SaveFileString(pInputFolder & "\logs\AreaSlopeCodeReport.txt", _
                               SWATArea.Report(lSwatInput.SubBsn.TableWithArea("Slope_Cd")))
            End If

            Dim lTotalArea As Double = 0.0
            Dim lTotalAreaNotConverted As Double = 0.0
            Dim lTotalAreaConverted As Double = 0.0
            Dim lTotalAreaCornFut As Double = 0.0
            Dim lTotalAreaCornNow As Double = 0.0
            Dim lCornConversions As New CornConversions
            Dim lSummaryWriter As New IO.StreamWriter(pInputFolder & "\logs\CornChanges.txt")
            lSummaryWriter.WriteLine("FrmCrp" & vbTab & "ToCrp" & vbTab _
                                   & "FrcChg".PadLeft(12) & vbTab _
                                   & "Area".PadLeft(12) & vbTab _
                                   & "AreaCornNow".PadLeft(12) & vbTab _
                                   & "AreaChg".PadLeft(12) & vbTab _
                                   & "AreaSkip".PadLeft(12) & vbTab _
                                   & "AreaCornFut".PadLeft(12) & vbTab _
                                   & "CntPot" & vbTab & "CntAct")
            Dim lHruWriter As New IO.StreamWriter(pInputFolder & "\logs\CornHruChanges.txt")
            lHruWriter.WriteLine("FrmCrp" & vbTab & "ToCrp" & vbTab _
                               & "SubId" & vbTab & "Soil" & vbTab & "Slope" & vbTab _
                               & "FrcChg".PadLeft(12) & vbTab _
                               & "Area".PadLeft(12) & vbTab _
                               & "AreaCornNow".PadLeft(12) & vbTab _
                               & "AreaChg".PadLeft(12) & vbTab _
                               & "AreaSkip".PadLeft(12) & vbTab _
                               & "AreaCornFut".PadLeft(12) & vbTab)

            For Each lLandUse As DataRow In lUniqueLandUses.Rows
                Dim lLandUseName As String = lLandUse.Item(0).ToString
                Logger.Dbg("Process " & lLandUseName)
                Dim lLandUSeConvertsTo As String = ""
                Dim lPotentialChangedHrus As DataTable = lSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lLandUseName & "';")
                Dim lConvertFractionNet As Double = 0
                Dim lCornFractionAfter As Double = 0
                Dim lCornFractionBefore As Double = 0
                If lCornConversions.Contains(lLandUseName) Then
                    Dim lCornConversion As CornConversion = lCornConversions.Item(lLandUseName)
                    Dim lCornConvertTo As CornConversion = lCornConversions.Item(lCornConversion.NameConvertsTo)
                    If lCornConvertTo.Fraction > lCornConversion.Fraction Then
                        lConvertFractionNet = lCornConvertTo.Fraction - lCornConversion.Fraction
                    End If
                    lLandUSeConvertsTo = lCornConversion.NameConvertsTo
                    lCornFractionAfter = lCornConvertTo.Fraction
                    lCornFractionBefore = lCornConversion.Fraction
                End If
                Dim lChangedHruCount As Integer = 0
                Dim lCropArea As Double = 0.0
                Dim lCropAreaNotConverted As Double = 0.0
                Dim lCropAreaConverted As Double = 0.0
                Dim lCropAreaCornNow As Double = 0.0
                Dim lCropAreaCornFut As Double = 0.0
                For Each lPotentialChangedHru As DataRow In lPotentialChangedHrus.Rows
                    Dim lHruItem As New SwatInput.clsHruItem(lPotentialChangedHru)
                    With lHruItem
                        Dim lAreaSubBasin As Double = lSwatInput.QueryInputDB("Select SUB_KM FROM(sub) WHERE SUBBASIN=" & .SUBBASIN & ";").Rows(0).Item(0)
                        Dim lHruChangeTo As DataTable = lSwatInput.QueryInputDB("Select * FROM(hru) WHERE LANDUSE='" & lLandUSeConvertsTo & "' AND SOIL='" & .SOIL & "' AND SLOPE_CD='" & .SLOPE_CD & "' AND SUBBASIN=" & .SUBBASIN & ";")
                        Dim lHruArea As Double = lAreaSubBasin * .HRU_FR
                        Dim lAreaPotentialConvert As Double = lHruArea * lConvertFractionNet
                        Dim lHruAreaNotConverted As Double = 0.0
                        Dim lHruAreaConverted As Double = 0.0
                        Dim lHruAreaCornNow As Double = lHruArea * lCornFractionBefore
                        Dim lHruAreaCornFut As Double = 0.0
                        If lHruChangeTo.Rows.Count > 0 Then
                            lHruAreaConverted = lAreaPotentialConvert
                            lChangedHruCount += 1
                            lHruAreaCornFut = lCornFractionAfter * lHruArea
                        Else 'no conversion
                            lHruAreaNotConverted = lAreaPotentialConvert
                            lHruAreaCornFut = lCornFractionBefore * lHruArea
                        End If
                        lHruWriter.WriteLine(lLandUseName & vbTab & lLandUSeConvertsTo & vbTab _
                                           & .SUBBASIN & vbTab & .SOIL & vbTab & .SLOPE_CD & vbTab _
                                           & DoubleToString(lConvertFractionNet, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                           & DoubleToString(lHruArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                           & DoubleToString(lHruAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                           & DoubleToString(lHruAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                           & DoubleToString(lHruAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                           & DoubleToString(lHruAreaCornFut, 12, pFormat, , , 10).PadLeft(12))
                        lCropArea += lHruArea
                        lCropAreaConverted += lHruAreaConverted
                        lCropAreaNotConverted += lHruAreaNotConverted
                        lCropAreaCornNow += lHruAreaCornNow
                        lCropAreaCornFut += lHruAreaCornFut
                    End With
                Next
                lSummaryWriter.WriteLine(lLandUseName & vbTab & lLandUSeConvertsTo & vbTab _
                                       & DoubleToString(lConvertFractionNet, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lCropArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lCropAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lCropAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lCropAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & DoubleToString(lCropAreaCornFut, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                                       & lPotentialChangedHrus.Rows.Count & vbTab _
                                       & lChangedHruCount)
                lTotalArea += lCropArea
                lTotalAreaConverted += lCropAreaConverted
                lTotalAreaNotConverted += lCropAreaNotConverted
                lTotalAreaCornNow += lCropAreaCornNow
                lTotalAreaCornFut += lCropAreaCornFut
                lHruWriter.Flush()
            Next
            lSummaryWriter.WriteLine("Total" & vbTab & Space(6) & vbTab _
                          & Space(12) & vbTab _
                          & DoubleToString(lTotalArea, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                          & DoubleToString(lTotalAreaCornNow, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                          & DoubleToString(lTotalAreaConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                          & DoubleToString(lTotalAreaNotConverted, 12, pFormat, , , 10).PadLeft(12) & vbTab _
                          & DoubleToString(lTotalAreaCornFut, 12, pFormat, , , 10).PadLeft(12))
            lSummaryWriter.Flush()
            Logger.Dbg("AreaTotal " & lTotalArea & " Converted " & lTotalAreaConverted & " NotTotal " & lTotalAreaNotConverted & " CornTotal " & lTotalAreaCornFut)

            'update areas 
            'lSwatInput.UpdateInputDB("HRU", "OID", .oi, "IYR", lUserParms.ItemByKey("Start Year"))

            Logger.Dbg("SWATPreprocess-UpdateParametersAsRequested")
            For Each lString As String In LinesInFile(pParmChangesTextfile)
                Dim lParms() As String = lString.Split(";")
                lSwatInput.UpdateInputDB(lParms(0).Trim, lParms(1).Trim, lParms(2).Trim, lParms(3).Trim, lParms(4).Trim)
            Next

            pRunModel = lUserParms.ItemByKey("Run Model")
            If pRunModel Then
                LaunchProgram(pSWATExe, pOutputFolder)
            End If

            If pOutputSummarize Then
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
            End If
        End If
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
                        lStr &= DoubleToString(.Item(lColumnIndex), 12, pFormat, , , 10).PadLeft(12) & vbTab
                        lAreaTotals(lColumnIndex) += .Item(lColumnIndex)
                    Next
                End With
                lSb.AppendLine(lStr.TrimEnd(vbTab))
            Next
            lStr = "Totals".PadLeft(12) & vbTab
            For lColumnIndex As Integer = 1 To .Columns.Count - 1
                lStr &= DoubleToString(lAreaTotals(lColumnIndex), 12, pFormat, , , 10).PadLeft(12) & vbTab
            Next
            lSb.AppendLine(lStr.TrimEnd(vbTab))
            Logger.Dbg("AreaTotalReportComplete " & lAreaTotals(1))
            Return lSb.ToString
        End With
    End Function

    Public Function AggregateCrops(ByVal aInputTable As DataTable) As DataTable
        Dim lCornConversions As New CornConversions
        Dim lArea As Double = 0.0

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
                If lCornConversions.Contains(lColumnName) Then
                    Dim lCornConversion As CornConversion = lCornConversions.Item(lColumnName)
                    lArea = lRow(lColumnIndex)
                    lRow(lCornColumnIndex) += lArea * lCornConversion.Fraction
                    lRow(lSoybColumnIndex) += lArea * (1 - lCornConversion.Fraction)
                End If
            Next
        Next
        Return lOutputTable
    End Function

    Friend Class CornConversions
        Inherits KeyedCollection(Of String, CornConversion)
        Protected Overrides Function GetKeyForItem(ByVal aParm As CornConversion) As String
            Return aParm.Name
        End Function

        Public Sub New()
            Me.Add(New CornConversion("CCCC", 1.0, "CCCC"))
            Me.Add(New CornConversion("CCS1", 0.66667, "CCCC"))
            Me.Add(New CornConversion("CSC1", 0.5, "CCCC")) 'TODO: check
            Me.Add(New CornConversion("CSS1", 0.33333, "CCCC"))
            Me.Add(New CornConversion("SCC1", 0.66667, "CCCC"))
            Me.Add(New CornConversion("SCS1", 0.5, "CCCC"))  'TODO: check
            Me.Add(New CornConversion("SSC1", 0.33333, "CCCC"))
            Me.Add(New CornConversion("SSSC", 0.0, "CCCC"))
            Me.Add(New CornConversion("AGRR", 0.0, "CCCC"))
            Me.Add(New CornConversion("CRP", 0.0, "CCCC"))
        End Sub
    End Class

    Friend Class CornConversion
        Public Name As String
        Public Fraction As Double
        Public NameConvertsTo As String

        Public Sub New(ByVal aName As String, ByVal aFraction As Double, ByVal aNameConvertsTo As String)
            Name = aName
            Fraction = aFraction
            NameConvertsTo = aNameConvertsTo
        End Sub
    End Class
End Module
