Imports atcUtility
Imports MapWinUtility
Imports atcUCI
Imports atcData
Imports System.IO
Imports System.Data
Imports HspfSupport
Imports ZedGraph 'this is coming from a DLL as the original project was a C# project and not a VB project
Imports atcGraph
Imports System.Collections.Specialized
''' <summary>
''' This module prepares a BATHTUB input file. It assumes that the HSPF model is run using English Units
''' </summary>
Module BATHTUB
    Sub BATHTUBInputFile(ByVal aHSPFUCI As HspfUci, ByVal aBinaryData As atcDataSource,
                         ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal aReachId As Integer,
                         ByVal aOutputfolder As String)
        Dim BATHTUBInputFile As New Text.StringBuilder
        BATHTUBInputFile.AppendLine("Vers 6.14f (04/28/2015)") 'First line of BATHTUB Output
        BATHTUBInputFile.AppendLine("HSPF Reach " & aReachId & " BATHTUB Model Developed Using HSPEXP+")

        'GLobal Parameters in four lines
        BATHTUBInputFile.AppendLine("4,""Global Parmameters""")
        Dim lYears As Integer = YearCount(aSDateJ, aEDateJ)
        BATHTUBInputFile.AppendLine("1,""AVERAGING PERIOD (YRS)"",1,0")
        Dim lTimeseries_precip As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "PRSUPY")(0)
        Dim lTimeseries_sarea As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "SAREA")(0)
        Dim lTimeseries As atcTimeseries = lTimeseries_precip / lTimeseries_sarea
        lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        Dim lAverage As Double = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        Dim lStdEv As Double = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        Dim lCVmean As Double = lStdEv / (lTimeseries.numValues) ^ (0.5) / lAverage
        BATHTUBInputFile.AppendLine("2,""PRECIPITATION (METERS)""," & Format(lAverage / 3.28084, "0.00") & "," & Format(lCVmean, "0.00"))
        Dim lTimeseries_volev = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "VOLEV")(0)
        lTimeseries = lTimeseries_volev / lTimeseries_sarea
        lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        lCVmean = lStdEv / (lTimeseries.numValues) ^ (0.5) / lAverage
        BATHTUBInputFile.AppendLine("3,""EVAPORATION (METERS)""," & Format(lAverage / 3.28084, "0.00") & "," & Format(lCVmean, "0.00"))
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "AVDEP")(0)
        Dim lChangeInDepth_m As Double = (lTimeseries.Value(1) - lTimeseries.Value(lTimeseries.numValues)) * 0.0254 / lYears
        BATHTUBInputFile.AppendLine("4,""INCREASE IN STORAGE (METERS)""," & Format(lChangeInDepth_m, "0.00") & ",0")

        'Model options in 12 lines; mostly copied from the default btb file
        Dim lString As String = "12,""Model Options""" & vbCrLf &
        "1,""CONSERVATIVE SUBSTANCE"",0" & vbCrLf &
        "2,""PHOSPHORUS BALANCE"",8" & vbCrLf &
        "3,""NITROGEN BALANCE"",0" & vbCrLf &
        "4,""CHLOROPHYLL-A"",2" & vbCrLf &
        "5,""SECCHI DEPTH"",1" & vbCrLf &
        "6,""DISPERSION"",1" & vbCrLf &
        "7,""PHOSPHORUS CALIBRATION"",1" & vbCrLf &
        "8,""NITROGEN CALIBRATION"",1" & vbCrLf &
        "9,""ERROR ANALYSIS"",1" & vbCrLf &
        "10,""AVAILABILITY FACTORS"",0" & vbCrLf &
        "11,""MASS-BALANCE TABLES"",1" & vbCrLf &
        "12,""OUTPUT DESTINATION"",2"
        BATHTUBInputFile.AppendLine(lString)

        'Model coefficients in 17 lines. Copied from an example file.
        lString = "17,""Model Coefficients""" & vbCrLf &
        "1,""DISPERSION RATE"",1,.7" & vbCrLf &
        "2,""P DECAY RATE"",1,.45" & vbCrLf &
        "3,""N DECAY RATE"",1,.55" & vbCrLf &
        "4,""CHL-A MODEL"",1,.26" & vbCrLf &
        "5,""SECCHI MODEL"",1,.1" & vbCrLf &
        "6,""ORGANIC N MODEL"",1,.12" & vbCrLf &
        "7,""TP-OP MODEL"",1,.15" & vbCrLf &
        "8,""HODV MODEL"",1,.15" & vbCrLf &
        "9,""MODV MODEL"",1,.22" & vbCrLf &
        "10,""BETA  M2/MG"",.025,0" & vbCrLf &
        "11,""MINIMUM QS"",.1,0" & vbCrLf &
        "12,""FLUSHING EFFECT"",1,0" & vbCrLf &
        "13,""CHLOROPHYLL-A CV"",.62,0" & vbCrLf &
        "14,""Avail Factor - TP"",.33,0" & vbCrLf &
        "15,""Avail Factor - Ortho P"",1.93,0" & vbCrLf &
        "16,""Avail Factor - TN"",.59,0" & vbCrLf &
        "17,""Avail Factor - Inorganic N"",.79,0"
        BATHTUBInputFile.AppendLine(lString)

        'Atmospheric deposition in 2 lines. Not sure if other atmospheric deposition information is required.
        BATHTUBInputFile.AppendLine("5,""Atmospheric Loads""")
        Dim lSurfaceArea_km2 As Double = 0
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "SAREA")(0)
        lSurfaceArea_km2 = lTimeseries.Attributes.GetDefinedValue("Mean").Value * 0.00404686
        BATHTUBInputFile.AppendLine("1,""CONSERVATIVE SUBST."",0,0")
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "PO4-ATMDEPTOT")(0)
        Dim lTPAverage As Double = 0
        Dim lTPCVmean As Double = 0
        If Not lTimeseries Is Nothing Then
            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
            lTimeseries *= 453592 'Converting lbs to mg
            lTimeseries /= (lSurfaceArea_km2 * 1000000) 'Changing load from mg to mg/m2 using average surface area
            lTPAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
            If lTPAverage <> 0 Then
                lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
                lTPCVmean = lStdEv / (lTimeseries.numValues) ^ (0.5) / lTPAverage
                BATHTUBInputFile.AppendLine("2,""TOTAL P""," & Format(lTPAverage, "0.00") & "," & Format(lTPCVmean, "0.00"))
            Else
                BATHTUBInputFile.AppendLine("2,""TOTAL P"",0,0")
            End If
        End If

        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "TAM-ATMDEPTOT")(0)
        If Not lTimeseries Is Nothing Then
            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        End If
        Dim lNO3_ATMDEPTOT As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO3-ATMDEPTOT")(0)
        If Not lNO3_ATMDEPTOT Is Nothing Then
            lNO3_ATMDEPTOT = SubsetByDate(lNO3_ATMDEPTOT, aSDateJ, aEDateJ, Nothing)
            lNO3_ATMDEPTOT = Aggregate(lNO3_ATMDEPTOT, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
            lTimeseries += lNO3_ATMDEPTOT
        End If
        Dim lNO2_ATMDEPTOT As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO2-ATMDEPTOT")(0)
        If Not lNO2_ATMDEPTOT Is Nothing Then
            lNO2_ATMDEPTOT = SubsetByDate(lNO2_ATMDEPTOT, aSDateJ, aEDateJ, Nothing)
            lNO2_ATMDEPTOT = Aggregate(lNO2_ATMDEPTOT, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
            lTimeseries += lNO2_ATMDEPTOT
        End If
        lTimeseries *= 453592 'Converting lbs to mg
        lTimeseries /= (lSurfaceArea_km2 * 1000000) 'Changing load from mg to mg/m2 using average surface area
        lAverage = 0
        lCVmean = 0
        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        If lAverage <> 0 Then
            lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
            lCVmean = lStdEv / (lTimeseries.numValues) ^ (0.5) / lAverage
            BATHTUBInputFile.AppendLine("3,""TOTAL N""," & Format(lAverage, "0.00") & "," & Format(lCVmean, "0.00"))
        Else
            BATHTUBInputFile.AppendLine("3,""TOTAL N"",0,0")
        End If

        BATHTUBInputFile.AppendLine("4,""ORTHO P""," & Format(lTPAverage / 2, "0.00") & "," & Format(lTPCVmean, "0.00"))
        BATHTUBInputFile.AppendLine("5,""INORGANIC N""," & Format(lAverage / 2, "0.00") & "," & Format(lCVmean, "0.00"))

        'Lake segments; assumes only one segment
        Dim lMeanDepth_m As Double = 0
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "AVDEP")(0)
        lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        lTimeseries /= 3.28084
        lMeanDepth_m = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        Dim CVMeanSampleSizeBasedOnHourly As Double = 0
        CVMeanSampleSizeBasedOnHourly = ((aEDateJ - aSDateJ) / 365)
        'Dim lDepth_CVmean As Double = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value / (lTimeseries.numValues) ^ (0.5) / lMeanDepth_m
        Dim lDepth_CVmean As Double = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value / (CVMeanSampleSizeBasedOnHourly) ^ (0.5) / lMeanDepth_m

        Dim lLength_km As Double = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).Tables("HYDR-PARM2").Parms("LEN").Value * 1.60934 'COnverting stream length in miles to km
        BATHTUBInputFile.AppendLine("1,""Segments""")
        'Properties of the first segment
        BATHTUBInputFile.AppendLine("1,""Main Pool"",0,1," & Format(lSurfaceArea_km2, "0.00") & "," &
                                    Format(lMeanDepth_m, "0.00") & "," & Format(lLength_km, "0.00") & "," & Format(lMeanDepth_m, "0.00") & "," &
                                    Format(lDepth_CVmean, "0.00") & ",0,0,0.08,0.05,0,0")
        'Lines for pollutants in the first segment; all written as zero and user must input in Bathtub; placeholder values are included for non-algal turbidity mean and CVmean to allow model to run
        lString = "1,""CONSERVATIVE SUBST."",0,0" & vbCrLf &
        "1,""TOTAL P"",0,0" & vbCrLf &
        "1,""TOTAL N"",0,0" & vbCrLf &
        "1,""CONSERVATIVE SUB"",0,0,1,0" & vbCrLf &
        "1,""TOTAL P    MG/M3"",0,0,1,0" & vbCrLf &
        "1,""TOTAL N    MG/M3"",0,0,1,0" & vbCrLf &
        "1,""CHL-A      MG/M3"",0,0,1,0" & vbCrLf &
        "1,""SECCHI         M"",0,0,1,0" & vbCrLf &
        "1,""ORGANIC N  MG/M3"",0,0,1,0" & vbCrLf &
        "1,""TP-ORTHO P MG/M3"",0,0,1,0" & vbCrLf &
        "1,""HOD-V  MG/M3-DAY"",0,0,1,0" & vbCrLf &
        "1,""MOD-V  MG/M3-DAY"",0,0,1,0" & vbCrLf


        BATHTUBInputFile.Append(lString)

        'Information about the tributaries

        Dim lRCHRESOperation As HspfOperation = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId)
        Dim lCountNumberOfTributaries As Integer = 0
        For Each lSource As HspfConnection In lRCHRESOperation.Sources
            If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
                lCountNumberOfTributaries += 1
            End If
        Next

        'An extra tributary is created for lakeshed loading
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""Tributaries""")
        lCountNumberOfTributaries = 0
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")

        CVMeanSampleSizeBasedOnHourly = 0
        Dim lCVmeanROVOL As Double = 0
        Dim lCVmeanTP As Double = 0
        Dim lCVmeanOP As Double = 0
        Dim lCVmeanTN As Double = 0
        Dim lCVmeanIN As Double = 0

        'Going through each reach that contributes to the main reach (lake)
        For Each lSource As HspfConnection In lRCHRESOperation.Sources
            If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
                lCountNumberOfTributaries += 1

                Dim lLocationID As String = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.VolId
                Dim lTribID As Integer = lSource.Source.VolId
                Dim lAreaTable As DataTable = AreaReportInTableFormat(aHSPFUCI, lOperationTypes, lLocationID)
                Dim lselectExpression As String = "Landuse='Total'"
                Dim lTotalRows() As DataRow = lAreaTable.Select("Landuse='Total'")
                Dim lDrainageAreaKM2 As Double = lTotalRows(0)("TotalArea") * 0.00404686 'Converting areas from ac to km2

                Dim lTimeSeriesIsInWDM As Boolean = False

                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "ROFLOW", "ROVOL", 1, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "ROVOL")(0)
                End If

                'save the original ROVOL timeseries for computing the flow weighted mean concentration
                Dim lROVOLTimeseries As atcTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)

                lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 0.00123348185532 'Converting flow in ac-ft to cubic hectometers
                lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
                lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
                lCVmeanROVOL = lStdEv / (lTimeseries.numValues) ^ (0.5) / lAverage
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""HSPF Reach " & lTribID & """,1,1," & Format(lDrainageAreaKM2, "0.00") & "," & Format(lAverage, "0.00") & "," & Format(lCVmeanROVOL, "0.00") & ",0")

                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""CONSERVATIVE SUBST."",0,0")

                'Get the total P concentration data
                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "PLANK", "TPKCF1", 5, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "P-TOT-OUT")(0)
                End If
                'convert to flow weighted concentration
                lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                'create concentration time series in micrograms per liter
                Dim lConcTimeseries As atcTimeseries = lTimeseries / lROVOLTimeseries
                lConcTimeseries *= 453592000
                lConcTimeseries /= 1233480
                lAverage = FlowWeightedConc(lConcTimeseries, lROVOLTimeseries)
                lStdEv = lConcTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
                CVMeanSampleSizeBasedOnHourly = ((aEDateJ - aSDateJ) / 365)
                lCVmeanTP = lStdEv / (CVMeanSampleSizeBasedOnHourly) ^ (0.5) / lAverage
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""TOTAL P""," & Format(lAverage, "0.00") & "," & Format(lCVmeanTP, "0.00"))

                'Get the total N concentration
                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "PLANK", "TPKCF1", 4, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "N-TOT-OUT")(0)
                End If
                'convert to flow weighted concentration
                lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                'create concentration time series in micrograms per liter
                lConcTimeseries = lTimeseries / lROVOLTimeseries
                lConcTimeseries *= 453592000
                lConcTimeseries /= 1233480
                lAverage = FlowWeightedConc(lConcTimeseries, lROVOLTimeseries)
                lStdEv = lConcTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
                lCVmeanTN = lStdEv / (CVMeanSampleSizeBasedOnHourly) ^ (0.5) / lAverage
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""TOTAL N""," & Format(lAverage, "0.00") & "," & Format(lCVmeanTN, "0.00"))

                'Get the ortho P concentration
                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "NUTRX", "DNUST", 4, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "PO4-CONCDIS")(0)
                End If
                'convert to flow weighted concentration
                lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                lAverage = FlowWeightedConc(lTimeseries, lROVOLTimeseries) * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
                lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
                lCVmeanOP = lStdEv / (CVMeanSampleSizeBasedOnHourly) ^ (0.5) / lAverage
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""ORTHO P""," & Format(lAverage, "0.00") & "," & Format(lCVmeanOP, "0.00"))

                'Get the inorganic N concentration (sum of NO3, NO2, and TAM)
                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "NUTRX", "DNUST", 1, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "NO3-CONCDIS")(0)
                End If
                Dim lInorgN As atcTimeseries = lTimeseries

                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "NUTRX", "DNUST", 2, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "TAM-CONCDIS")(0)
                End If
                lInorgN += lTimeseries

                lTimeseries = LocateTheTimeSeries(aHSPFUCI, lTribID, "NUTRX", "DNUST", 3, 1, lTimeSeriesIsInWDM)
                If lTimeSeriesIsInWDM = True Then
                    lTimeSeriesIsInWDM = False
                Else
                    lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & lTribID).FindData("Constituent", "NO2-CONCDIS")(0)
                End If
                If Not lTimeseries Is Nothing Then
                    lInorgN += lTimeseries
                End If

                'convert to flow weighted concentration
                lInorgN = SubsetByDate(lInorgN, aSDateJ, aEDateJ, Nothing)
                lAverage = FlowWeightedConc(lInorgN, lROVOLTimeseries) * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
                lStdEv = lInorgN.Attributes.GetDefinedValue("Standard Deviation").Value
                lCVmeanIN = lStdEv / (CVMeanSampleSizeBasedOnHourly) ^ (0.5) / lAverage
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""INORGANIC N""," & Format(lAverage, "0.00") & "," & Format(lCVmeanIN, "0.00"))
                BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""LandUses"",0,0,0,0,0,0,0,0")
            End If
        Next

        'Information about the local lakeshed

        Dim lNPSTable As New DataTable
        Dim lColumn As DataColumn
        lColumn = New DataColumn
        lColumn.ColumnName = "LandUseName"
        lColumn.DataType = Type.GetType("System.String")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "Area_km2"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "Water_Vol_m"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "Water_Vol_COV"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "PO4_mg_m3"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "PO4_COV"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "TP_mg_m3"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "TP_COV"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "InorgN_mg_m3"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "InorgN_COV"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "TN_mg_m3"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        lColumn = New DataColumn
        lColumn.ColumnName = "TN_COV"
        lColumn.DataType = Type.GetType("System.Double")
        lNPSTable.Columns.Add(lColumn)

        Dim lRow As DataRow
        For Each lSource As HspfConnection In lRCHRESOperation.Sources
            If Not lSource.Source.Opn Is Nothing AndAlso (lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND") Then
                Dim lLocationID As String = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.VolId
                Dim lMassLinkID As Integer = lSource.MassLink
                lRow = lNPSTable.NewRow
                lRow("LandUseName") = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.Opn.Description
                lRow("Area_km2") = lSource.MFact * 0.00404686 'Converting area from ac to km2
                Dim WaterVolumeInMAndCov As Double() = CalculateVolumeInMeters(lSource.Source.Opn, aBinaryData, aSDateJ, aEDateJ, lLocationID)
                lRow("Water_Vol_m") = WaterVolumeInMAndCov(0)
                lRow("Water_Vol_COV") = WaterVolumeInMAndCov(1) * ((lTimeseries.numValues) ^ (0.5)) / ((CVMeanSampleSizeBasedOnHourly) ^ (0.5))
                'The loading rate from the function is in lbs/ac. Multiplying it by 112.085 converts to mg/m2. Dividing it by water depth, converts to mg/m3
                Dim NutrientLoadAndCOV As Double() = CalculatePhosphorus(lSource.Source.Opn, aBinaryData, lRCHRESOperation, lMassLinkID, aSDateJ, aEDateJ, aHSPFUCI)
                lRow("PO4_mg_m3") = NutrientLoadAndCOV(0) / WaterVolumeInMAndCov(0)
                lRow("PO4_COV") = NutrientLoadAndCOV(1) * ((lTimeseries.numValues) ^ (0.5)) / ((CVMeanSampleSizeBasedOnHourly) ^ (0.5))
                lRow("TP_mg_m3") = NutrientLoadAndCOV(2) / WaterVolumeInMAndCov(0)
                lRow("TP_COV") = NutrientLoadAndCOV(3) * ((lTimeseries.numValues) ^ (0.5)) / ((CVMeanSampleSizeBasedOnHourly) ^ (0.5))
                NutrientLoadAndCOV = CalculateNitrogen(lSource.Source.Opn, aBinaryData, lRCHRESOperation, lMassLinkID, aSDateJ, aEDateJ, aHSPFUCI)
                lRow("InorgN_mg_m3") = NutrientLoadAndCOV(0) / WaterVolumeInMAndCov(0)
                lRow("InorgN_COV") = NutrientLoadAndCOV(1) * ((lTimeseries.numValues) ^ (0.5)) / ((CVMeanSampleSizeBasedOnHourly) ^ (0.5))
                lRow("TN_mg_m3") = NutrientLoadAndCOV(2) / WaterVolumeInMAndCov(0)
                lRow("TN_COV") = NutrientLoadAndCOV(3) * ((lTimeseries.numValues) ^ (0.5)) / ((CVMeanSampleSizeBasedOnHourly) ^ (0.5))
                lNPSTable.Rows.Add(lRow)
            End If
        Next
        Dim lakeshedAreaKm2 As Double = lNPSTable.Compute("SUM(Area_km2)", "")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""LakeShed"",1,2," & Format(lakeshedAreaKm2, "0.00") & ",0,0,0") ' & Format(lAverage, "0.00") & "," & Format(lCov, "0.00") & ",0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""CONSERVATIVE SUBST."",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL P"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL N"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""ORTHO P"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""INORGANIC N"",0,0")
        Dim landuseAreakm2 As String = lCountNumberOfTributaries + 1 & ",""LandUses"","
        Dim lNumberOfLandUses As Integer = lNPSTable.Rows.Count
        For Each lNPSTableRow As DataRow In lNPSTable.Rows
            landuseAreakm2 &= Format(lNPSTableRow("Area_km2"), "0.00") & ","
        Next
        If lNumberOfLandUses < 8 Then
            For i As Integer = 1 To 8 - lNumberOfLandUses
                landuseAreakm2 &= "0,"
            Next
        End If
        landuseAreakm2 = landuseAreakm2.Substring(0, landuseAreakm2.Length - 1)

        'BATHTUBInputFile.AppendLine(landuseAreakm2)
        'BATHTUBInputFile.AppendLine("0,""Channels""")
        'BATHTUBInputFile.AppendLine("8,""Land Use Export Categories""")
        Dim CumArea As Double = 0
        Dim CumRunoff As Double = 0
        Dim CumTPLoad As Double = 0
        Dim CumTNLoad As Double = 0
        Dim CumOPLoad As Double = 0
        Dim CumINLoad As Double = 0
        Dim LandUseNumber As Integer = 0
        For Each lNPSTableRow As DataRow In lNPSTable.Rows
            LandUseNumber += 1
            CumArea += lNPSTableRow("Area_km2")
            CumRunoff += lNPSTableRow("Water_Vol_m") * lNPSTableRow("Area_km2")
            CumTPLoad += lNPSTableRow("TP_mg_m3") * lNPSTableRow("Water_Vol_m") * lNPSTableRow("Area_km2")
            CumTNLoad += lNPSTableRow("TN_mg_m3") * lNPSTableRow("Water_Vol_m") * lNPSTableRow("Area_km2")
            CumOPLoad += lNPSTableRow("PO4_mg_m3") * lNPSTableRow("Water_Vol_m") * lNPSTableRow("Area_km2")
            CumINLoad += lNPSTableRow("InorgN_mg_m3") * lNPSTableRow("Water_Vol_m") * lNPSTableRow("Area_km2")
            'BATHTUBInputFile.AppendLine(LandUseNumber & "," & """" & lNPSTableRow("LandUseName") & """")
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""Runoff""," & Format(lNPSTableRow("Water_Vol_m"), "0.00") & "," & Format(lNPSTableRow("Water_Vol_COV"), "0.00"))
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""CONSERVATIVE SUBST."",0,0")
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""TOTAL P""," & Format(lNPSTableRow("TP_mg_m3"), "0.00") & "," & Format(lNPSTableRow("TP_COV"), "0.00"))
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""TOTAL N""," & Format(lNPSTableRow("TN_mg_m3"), "0.00") & "," & Format(lNPSTableRow("TN_COV"), "0.00"))
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""ORTHO P""," & Format(lNPSTableRow("PO4_mg_m3"), "0.00") & "," & Format(lNPSTableRow("PO4_COV"), "0.00"))
            'BATHTUBInputFile.AppendLine(LandUseNumber & ",""INORGANIC N""," & Format(lNPSTableRow("InorgN_mg_m3"), "0.00") & "," & Format(lNPSTableRow("InorgN_COV"), "0.00"))
        Next

        Dim CumTPConc As Double = 0
        Dim CumTNConc As Double = 0
        Dim CumOPConc As Double = 0
        Dim CumINConc As Double = 0

        If CumRunoff > 0 Then
            CumTPConc = CumTPLoad / CumRunoff
            CumTNConc = CumTNLoad / CumRunoff
            CumOPConc = CumOPLoad / CumRunoff
            CumINConc = CumINLoad / CumRunoff
        End If

        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""Lakeshed: HSPF Reach " & aReachId & """,1,1," & Format(CumArea, "0.00") & "," & Format(CumRunoff, "0.00") & "," & Format(lCVmeanROVOL, "0.00") & ",0")
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""CONSERVATIVE SUBST."",0,0")
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL P""," & Format(CumTPConc, "0.00") & "," & Format(lCVmeanTP, "0.00"))
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL N""," & Format(CumTNConc, "0.00") & "," & Format(lCVmeanTN, "0.00"))
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""ORTHO P""," & Format(CumOPConc, "0.00") & "," & Format(lCVmeanOP, "0.00"))
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""INORGANIC N""," & Format(CumINConc, "0.00") & "," & Format(lCVmeanIN, "0.00"))
        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""LandUses"",0,0,0,0,0,0,0,0")

        'BATHTUBInputFile.AppendLine(landuseAreakm2)
        BATHTUBInputFile.AppendLine("0,""Channels""")
        BATHTUBInputFile.AppendLine("8,""Land Use Export Categories""")

        'If lNumberOfLandUses < 8 Then
        For i As Integer = 1 To 8 '- lNumberOfLandUses
            BATHTUBInputFile.AppendLine(i & "," & """landuse" & i & """")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & "," & """""")
            BATHTUBInputFile.AppendLine(i & ",""Runoff"",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""Runoff"",0,0")
            BATHTUBInputFile.AppendLine(i & ",""CONSERVATIVE SUBST."",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""CONSERVATIVE SUBST."",0,0")
            BATHTUBInputFile.AppendLine(i & ",""TOTAL P"",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""TOTAL P"",0,0")
            BATHTUBInputFile.AppendLine(i & ",""TOTAL N"",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""TOTAL N"",0,0")
            BATHTUBInputFile.AppendLine(i & ",""ORTHO P"",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""ORTHO P"",0,0")
            BATHTUBInputFile.AppendLine(i & ",""INORGANIC N"",0,0")
            'BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""INORGANIC N"",0,0")
        Next
        'End If


        BATHTUBInputFile.AppendLine("""Notes""")
        'BATHTUBInputFile.AppendLine("Write Whatever you Want!!!")
        For i As Integer = 0 To 9
            BATHTUBInputFile.AppendLine()
        Next



        File.WriteAllText(aOutputfolder & "BATHTUB_" & aReachId & ".btb", BATHTUBInputFile.ToString)

    End Sub
    Private Function CalculateVolumeInMeters(ByVal aHSPFOpn As HspfOperation, ByVal aBinaryData As atcDataSource, ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                              ByVal aLocationID As String) As Double()
        Dim lOutput As Double() = {0, 0}
        Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aLocationID).FindData("Constituent", "SURO")(0)
        Dim lTemplTS As New atcTimeseries(Nothing)
        lTemplTS = aBinaryData.DataSets.FindData("Location", aLocationID).FindData("Constituent", "IFWO")(0)
        If Not lTemplTS Is Nothing Then
            lTS += lTemplTS
        End If
        lTemplTS = aBinaryData.DataSets.FindData("Location", aLocationID).FindData("Constituent", "AGWO")(0)
        If Not lTemplTS Is Nothing Then
            lTS += lTemplTS
        End If
        lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
        lTS = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        lTS *= 0.0254 'Converting water from inches to meters
        lOutput(0) = lTS.Attributes.GetDefinedValue("Mean").Value
        lOutput(1) = lTS.Attributes.GetDefinedValue("Standard Deviation").Value / lOutput(0)
        Return lOutput


    End Function

    Private Function CalculatePhosphorus(ByVal aHSPFSourceOpn As HspfOperation, ByVal aBinaryData As atcDataSource,
                                         ByVal aRCHRESOperation As HspfOperation, ByVal aMassLink As Integer,
                                         ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                         ByVal aHSPFUCI As HspfUci) As Double()
        Dim lOrthoPAndTP As Double() = {0, 0, 0, 0} 'OrthoP in mg/m3 then COV, TP in mg/m3 then COV
        Dim lConstituentList As Dictionary(Of String, String) = ConstituentList("TP", "ORTHO P",,, aHSPFSourceOpn.Name)
        Dim lTotal As Double = 0
        Dim lConversionFactor As Double = ConversionFactorfromOxygen(aHSPFUCI, "TP", aRCHRESOperation)
        Dim lTotalTS As New atcTimeseries(Nothing)
        For Each lOutflowDataType As String In lConstituentList.Values
            Dim lMassLinkFactor As Double = 1.0
            Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aHSPFSourceOpn.Name.Substring(0, 1) & ":" & aHSPFSourceOpn.Id).FindData("Constituent", lOutflowDataType)(0)
            If lTS Is Nothing Then Continue For
            If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
            If Not aMassLink = 0 Then
                lMassLinkFactor = FindMassLinkFactor(aHSPFUCI, aMassLink, lOutflowDataType,
                                                "TP", lConversionFactor, 0)
            End If
            If lTotalTS.numValues = 0 Then
                lTotalTS = lTS * lMassLinkFactor
            Else
                lTotalTS += lTS * lMassLinkFactor
            End If

        Next
        If lTotalTS.numValues = 0 Then
            lOrthoPAndTP(0) = 0.0
            lOrthoPAndTP(1) = 0.0
        Else
            lTotalTS = SubsetByDate(lTotalTS, aSDateJ, aEDateJ, Nothing)
            Dim lAverageTS As atcTimeseries = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
            lOrthoPAndTP(0) = lAverageTS.Attributes.GetDefinedValue("SumAnnual").Value
            lOrthoPAndTP(1) = lAverageTS.Attributes.GetDefinedValue("Standard Deviation").Value / lOrthoPAndTP(0)
        End If

        lConstituentList = ConstituentList("TP", "BOD",,, aHSPFSourceOpn.Name)

        For Each lOutflowDataType As String In lConstituentList.Values
            Dim lMassLinkFactor As Double = 1.0
            Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aHSPFSourceOpn.Name.Substring(0, 1) & ":" & aHSPFSourceOpn.Id).FindData("Constituent", lOutflowDataType)(0)
            If lTS Is Nothing Then Continue For
            If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
            If Not aMassLink = 0 Then
                lMassLinkFactor = FindMassLinkFactor(aHSPFUCI, aMassLink, lOutflowDataType,
                                                 "TP", lConversionFactor, 0)
            End If
            If lTotalTS.numValues = 0 Then
                lTotalTS = lTS * lMassLinkFactor
            Else
                lTotalTS += lTS * lMassLinkFactor
            End If
        Next
        If lTotalTS.numValues = 0 Then
            lOrthoPAndTP(2) = 0.0
            lOrthoPAndTP(3) = 0.0
        Else
            lTotalTS = SubsetByDate(lTotalTS, aSDateJ, aEDateJ, Nothing)
            lTotalTS = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
            lOrthoPAndTP(2) = lTotalTS.Attributes.GetDefinedValue("SumAnnual").Value
            lOrthoPAndTP(3) = lTotalTS.Attributes.GetDefinedValue("Standard Deviation").Value / lOrthoPAndTP(2)
        End If

        Return lOrthoPAndTP

    End Function

    Private Function CalculateNitrogen(ByVal aHSPFSourceOpn As HspfOperation, ByVal aBinaryData As atcDataSource,
                                         ByVal aRCHRESOperation As HspfOperation, ByVal aMassLink As Integer,
                                         ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                         ByVal aHSPFUCI As HspfUci) As Double()
        Dim lInorNAndTotalN As Double() = {0, 0, 0, 0}
        Dim lConstituentList As Dictionary(Of String, String) = ConstituentList("TN", "NH3+NH4",,, aHSPFSourceOpn.Name)
        Dim lTotalTS As New atcTimeseries(Nothing)
        Dim lConversionFactor As Double = ConversionFactorfromOxygen(aHSPFUCI, "TP", aRCHRESOperation)
        For Each lOutflowDataType As String In lConstituentList.Values
            Dim lMassLinkFactor As Double = 1.0
            Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aHSPFSourceOpn.Name.Substring(0, 1) & ":" & aHSPFSourceOpn.Id).FindData("Constituent", lOutflowDataType)(0)
            If lTS Is Nothing Then Continue For
            If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
            If Not aMassLink = 0 Then
                lMassLinkFactor = FindMassLinkFactor(aHSPFUCI, aMassLink, lOutflowDataType,
                                                 "TN", lConversionFactor, 0)
            End If
            If lTotalTS.numValues = 0 Then
                lTotalTS = lTS * lMassLinkFactor
            Else
                lTotalTS += lTS * lMassLinkFactor
            End If
        Next

        lConstituentList = ConstituentList("TN", "NO3",,, aHSPFSourceOpn.Name)
        For Each lOutflowDataType As String In lConstituentList.Values
            Dim lMassLinkFactor As Double = 1.0
            Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aHSPFSourceOpn.Name.Substring(0, 1) & ":" & aHSPFSourceOpn.Id).FindData("Constituent", lOutflowDataType)(0)
            If lTS Is Nothing Then Continue For
            If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
            If Not aMassLink = 0 Then
                lMassLinkFactor = FindMassLinkFactor(aHSPFUCI, aMassLink, lOutflowDataType,
                                                 "TN", lConversionFactor, 0)
            End If
            If lTotalTS.numValues = 0 Then
                lTotalTS = lTS * lMassLinkFactor
            Else
                lTotalTS += lTS * lMassLinkFactor
            End If
        Next
        If lTotalTS.numValues = 0 Then
            lInorNAndTotalN(0) = 0.0
            lInorNAndTotalN(1) = 0.0
        Else
            lTotalTS = SubsetByDate(lTotalTS, aSDateJ, aEDateJ, Nothing)
            Dim lAverageTS As atcTimeseries = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2

            lInorNAndTotalN(0) = lAverageTS.Attributes.GetDefinedValue("SumAnnual").Value
            lInorNAndTotalN(1) = lAverageTS.Attributes.GetDefinedValue("Standard Deviation").Value / lInorNAndTotalN(0)
        End If


        lConstituentList = ConstituentList("TN", "BOD",,, aHSPFSourceOpn.Name)
        For Each lOutflowDataType As String In lConstituentList.Values
            Dim lMassLinkFactor As Double = 1.0
            Dim lTS As atcTimeseries = aBinaryData.DataSets.FindData("Location", aHSPFSourceOpn.Name.Substring(0, 1) & ":" & aHSPFSourceOpn.Id).FindData("Constituent", lOutflowDataType)(0)
            If lTS Is Nothing Then Continue For
            If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
            If Not aMassLink = 0 Then
                lMassLinkFactor = FindMassLinkFactor(aHSPFUCI, aMassLink, lOutflowDataType,
                                                 "TN", lConversionFactor, 0)
            End If
            If lTotalTS.numValues = 0 Then
                lTotalTS = lTS * lMassLinkFactor
            Else
                lTotalTS += lTS * lMassLinkFactor
            End If
        Next
        If lTotalTS.numValues = 0 Then
            lInorNAndTotalN(2) = 0.0
            lInorNAndTotalN(3) = 0.0
        Else
            lTotalTS = SubsetByDate(lTotalTS, aSDateJ, aEDateJ, Nothing)
            lTotalTS = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
            lInorNAndTotalN(2) = lTotalTS.Attributes.GetDefinedValue("SumAnnual").Value
            lInorNAndTotalN(3) = lTotalTS.Attributes.GetDefinedValue("Standard Deviation").Value / lInorNAndTotalN(2)
        End If

        Return lInorNAndTotalN

    End Function

    Function FlowWeightedConc(ByVal aWQts As atcTimeseries, ByVal aFlowts As atcTimeseries, Optional ByVal aSpec As atcDataAttributes = Nothing) As Double
        Dim lFWC As Double = 0.0

        Dim lSumFC As Double = 0.0
        Dim lSumF As Double = 0.0
        Dim lWQVal As Double
        Dim lFlowVal As Double

        Dim lLogTransform As Boolean = False
        Dim lStartDate As Double = -99
        Dim lEndDate As Double = -99
        Dim lSigDigit As Integer = -99

        If aSpec IsNot Nothing Then
            With aSpec
                lLogTransform = .GetValue("Log", False)
                lStartDate = .GetValue("Start", -99)
                lEndDate = .GetValue("End", -99)
                lSigDigit = .GetValue("SigDigit", -99)
            End With
            If lStartDate >= lEndDate Then
                lStartDate = -99
                lEndDate = -99
            End If
        End If

        If lStartDate > 0 AndAlso lEndDate > 0 Then
            aWQts = SubsetByDate(aWQts, lStartDate, lEndDate, Nothing)
            aFlowts = SubsetByDate(aFlowts, lStartDate, lEndDate, Nothing)
        Else
            Dim lGroup As New atcTimeseriesGroup()
            lGroup.Add(aWQts)
            lGroup.Add(aFlowts)
            Dim lFirstStart As Double
            Dim lLastEnd As Double
            Dim lCommonStart As Double
            Dim lCommonEnd As Double
            If CommonDates(lGroup, lFirstStart, lLastEnd, lCommonStart, lCommonEnd) Then
                If lFirstStart <> lCommonStart OrElse lLastEnd <> lCommonEnd Then
                    aWQts = SubsetByDate(aWQts, lCommonStart, lCommonEnd, Nothing)
                    aFlowts = SubsetByDate(aFlowts, lCommonStart, lCommonEnd, Nothing)
                End If
            End If
            lGroup.Clear()
        End If

        Dim lIncludeFullFlowRange As Boolean = True
        For lIndex As Integer = 1 To aWQts.numValues
            'assume for now the flow and wq timeseries have same start/end dates and time step
            lWQVal = aWQts.Values(lIndex)
            lFlowVal = aFlowts.Values(lIndex)

            'bypass negative Qual values
            If lWQVal < 0 OrElse lFlowVal < 0 OrElse
                Double.IsNaN(lWQVal) OrElse Double.IsNaN(lFlowVal) OrElse
                Double.IsInfinity(lWQVal) OrElse Double.IsInfinity(lFlowVal) Then
                Continue For
            End If

            If lSigDigit > 0 Then
                lWQVal = Math.Round(lWQVal, lSigDigit)
            End If

            If lLogTransform Then
                If lWQVal > 0 Then
                    lWQVal = Math.Log10(lWQVal)
                Else
                    lWQVal = 0.0
                End If
            End If
            lSumFC += lWQVal * lFlowVal
            lSumF += lFlowVal
        Next
        If lSumF > 0 Then
            If lLogTransform Then
                lFWC = Math.Pow(10.0, lSumFC / lSumF)
            Else
                lFWC = lSumFC / lSumF
            End If
        End If
        Return lFWC
    End Function
End Module

