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
''' This module prepares a WASP input file. It assumes that the HSPF model is run using English Units
''' </summary>
Module WASP
    Sub WASPInputFile(ByVal aHSPFUCI As HspfUci, ByVal aBinaryData As atcDataSource,
                         ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal aReachId As Integer,
                         ByVal aOutputfolder As String)
        'Dim BATHTUBInputFile As New Text.StringBuilder
        'BATHTUBInputFile.AppendLine("Vers 6.14f (04/28/2015)") 'First line of BATHTUB Output
        'BATHTUBInputFile.AppendLine("BATHTUB Model Developed Using HSPEXP+")

        ''GLobal Parameters in four lines
        'BATHTUBInputFile.AppendLine("4,""Global Parmameters""")
        'Dim lYears As Integer = YearCount(aSDateJ, aEDateJ)
        'BATHTUBInputFile.AppendLine("1,""AVERAGING PERIOD (YRS)"",1,0")
        'Dim lTimeseries As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "PRSUPY")(0)
        'lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'Dim lAverage As Double = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        'Dim lStdEv As Double = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        'Dim lCov As Double = lStdEv / lAverage
        'BATHTUBInputFile.AppendLine("2,""PRECIPITATION (METERS)""," & Format(lAverage * 0.0254, "0.00") & "," & Format(lCov, "0.00"))
        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "VOLEV")(0)
        'lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        'lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        'lCov = lStdEv / lAverage
        'BATHTUBInputFile.AppendLine("3,""EVAPORATION (METERS)""," & Format(lAverage * 0.0254, "0.00") & "," & Format(lCov, "0.00"))
        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "AVDEP")(0)
        'Dim lChangeInDepth_m As Double = (lTimeseries.Value(1) - lTimeseries.Value(lTimeseries.numValues)) * 0.0254 / lYears
        'BATHTUBInputFile.AppendLine("4,""INCREASE IN STORAGE (METERS)""," & Format(lChangeInDepth_m, "0.00") & ",0")

        ''Model options in 12 lines. Pretty much copied from an example file.
        'Dim lString As String = "12,""Model Options""" & vbCrLf &
        '"1,""CONSERVATIVE SUBSTANCE"",0" & vbCrLf &
        '"2,""PHOSPHORUS BALANCE"",1" & vbCrLf &
        '"3,""NITROGEN BALANCE"",0" & vbCrLf &
        '"4,""CHLOROPHYLL-A"",2" & vbCrLf &
        '"5,""SECCHI DEPTH"",1" & vbCrLf &
        '"6,""DISPERSION"",1" & vbCrLf &
        '"7,""PHOSPHORUS CALIBRATION"",1" & vbCrLf &
        '"8,""NITROGEN CALIBRATION"",1" & vbCrLf &
        '"9,""ERROR ANALYSIS"",1" & vbCrLf &
        '"10,""AVAILABILITY FACTORS"",0" & vbCrLf &
        '"11,""MASS-BALANCE TABLES"",1" & vbCrLf &
        '"12,""OUTPUT DESTINATION"",2"
        'BATHTUBInputFile.AppendLine(lString)

        ''Model coefficients in 17 lines. Copied from an example file.
        'lString = "17,""Model Coefficients""" & vbCrLf &
        '"1,""DISPERSION RATE"",1,.7" & vbCrLf &
        '"2,""P DECAY RATE"",1,.45" & vbCrLf &
        '"3,""N DECAY RATE"",1,.55" & vbCrLf &
        '"4,""CHL-A MODEL"",1,.26" & vbCrLf &
        '"5,""SECCHI MODEL"",1,.1" & vbCrLf &
        '"6,""ORGANIC N MODEL"",1,.12" & vbCrLf &
        '"7,""TP-OP MODEL"",1,.15" & vbCrLf &
        '"8,""HODV MODEL"",1,.15" & vbCrLf &
        '"9,""MODV MODEL"",1,.22" & vbCrLf &
        '"10,""BETA  M2/MG"",.025,0" & vbCrLf &
        '"11,""MINIMUM QS"",.1,0" & vbCrLf &
        '"12,""FLUSHING EFFECT"",1,0" & vbCrLf &
        '"13,""CHLOROPHYLL-A CV"",.62,0" & vbCrLf &
        '"14,""Avail Factor - TP"",.33,0" & vbCrLf &
        '"15,""Avail Factor - Ortho P"",1.93,0" & vbCrLf &
        '"16,""Avail Factor - TN"",.59,0" & vbCrLf &
        '"17,""Avail Factor - Inorganic N"",.79,0"
        'BATHTUBInputFile.AppendLine(lString)

        ''Atmospheric deposition in 2 lines. Not sure if other atmispheric deposition information is required.
        'BATHTUBInputFile.AppendLine("5,""Atmospheric Loads""")
        'Dim lSurfaceArea_km2 As Double = 0
        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "SAREA")(0)
        'lSurfaceArea_km2 = lTimeseries.Attributes.GetDefinedValue("Mean").Value * 0.00404686
        'BATHTUBInputFile.AppendLine("1, ""CONSERVATIVE SUBST."", 0,0")
        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "PO4-ATMDEPTOT")(0)
        'Dim lORTHOPAverage As Double = 0
        'Dim lORTHOPCov As Double = 0
        'If Not lTimeseries Is Nothing Then
        '    lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '    lTimeseries *= 453592 'Converting lbs to mg
        '    lTimeseries /= (lSurfaceArea_km2 * 1000000) 'Changing load from mg to mg/m2 using average surface area
        '    lORTHOPAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        '    If lORTHOPAverage <> 0 Then
        '        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lORTHOPCov = lStdEv / lORTHOPAverage
        '        BATHTUBInputFile.AppendLine("2,""TOTAL P""," & Format(lORTHOPAverage, "0.00") & "," & Format(lORTHOPCov, "0.00"))
        '    Else
        '        BATHTUBInputFile.AppendLine("2,""TOTAL P"",0,0")
        '    End If
        'End If

        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "TAM-ATMDEPTOT")(0)
        'If Not lTimeseries Is Nothing Then
        '    lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        'End If
        'Dim lNO3_ATMDEPTOT As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO3-ATMDEPTOT")(0)
        'If Not lNO3_ATMDEPTOT Is Nothing Then
        '    lNO3_ATMDEPTOT = Aggregate(lNO3_ATMDEPTOT, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '    lTimeseries += lNO3_ATMDEPTOT
        'End If
        'Dim lNO2_ATMDEPTOT As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO2-ATMDEPTOT")(0)
        'If Not lNO2_ATMDEPTOT Is Nothing Then
        '    lNO2_ATMDEPTOT = Aggregate(lNO2_ATMDEPTOT, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
        '    lTimeseries += lNO2_ATMDEPTOT
        'End If
        'lTimeseries *= 453592 'Converting lbs to mg
        'lTimeseries /= (lSurfaceArea_km2 * 1000000) 'Changing load from mg to mg/m2 using average surface area
        'lAverage = 0
        'lCov = 0
        'lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        'If lAverage <> 0 Then
        '    lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '    lCov = lStdEv / lAverage
        '    BATHTUBInputFile.AppendLine("3,""TOTAL N""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))
        'Else
        '    BATHTUBInputFile.AppendLine("3,""TOTAL N"",0,0")
        'End If

        'BATHTUBInputFile.AppendLine("4,""ORTHO P""," & Format(lORTHOPAverage, "0.00") & "," & Format(lORTHOPCov, "0.00"))
        'BATHTUBInputFile.AppendLine("5,""INORGANIC N""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))

        ''Working on segments line. Right now, assuming only one segment
        'Dim lMeanDepth_m As Double = 0
        'lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "AVDEP")(0)
        'lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        'lTimeseries *= 0.0254
        'lMeanDepth_m = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        'Dim lDepth_COV As Double = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value / lMeanDepth_m

        'Dim lLength_km As Double = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).Tables("HYDR-PARM2").Parms("LEN").Value * 1.60934 'COnverting stream length in miles to km
        'BATHTUBInputFile.AppendLine("1,""Segments""")
        ''Properties of the first segment
        'BATHTUBInputFile.AppendLine("1,""MainPool"",0,1," & Format(lSurfaceArea_km2, "0.00") & "," &
        '                            Format(lMeanDepth_m, "0.00") & "," & Format(lLength_km, "0.00") & "," & Format(lMeanDepth_m, "0.00") & "," &
        '                            Format(lDepth_COV, "0.00") & "," & Format(lMeanDepth_m, "0.00") & "," & Format(lDepth_COV, "0.00") & ",1,0,0,0")
        ''Lines for pollutants in the first segment
        'lString = "1,""CONSERVATIVE SUBST."",0,0" & vbCrLf &
        '"1,""TOTAL P"",0,0" & vbCrLf &
        '"1,""TOTAL N"",0,0" & vbCrLf &
        '"1,""CONSERVATIVE SUB"",0,0,1,0" & vbCrLf &
        '"1,""TOTAL P    MG/M3"",75,.2,1,0" & vbCrLf &
        '"1,""TOTAL N    MG/M3"",0,0,1,0" & vbCrLf &
        '"1,""CHL-A      MG/M3"",5,.2,1,0" & vbCrLf &
        '"1,""SECCHI         M"",.8,.3,1,0" & vbCrLf &
        '"1,""ORGANIC N  MG/M3"",0,0,1,0" & vbCrLf &
        '"1,""TP-ORTHO-P MG/M3"",0,0,1,0" & vbCrLf &
        '"1,""HOD-V  MG/M3-DAY"",50,.2,1,0" & vbCrLf &
        '"1,""MOD-V  MG/M3-DAY"",0,0,1,0" & vbCrLf


        'BATHTUBInputFile.Append(lString)

        ''Information about the tributaries

        'Dim lRCHRESOperation As HspfOperation = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId)
        'Dim lCountNumberOfTributaries As Integer = 0
        'For Each lSource As HspfConnection In lRCHRESOperation.Sources
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
        '        lCountNumberOfTributaries += 1
        '    End If
        'Next

        ''An extra tributary is the non-point loading
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""Tributaries""")
        'lCountNumberOfTributaries = 0
        'Dim lOperationTypes As New atcCollection
        'lOperationTypes.Add("P:", "PERLND")
        'lOperationTypes.Add("I:", "IMPLND")
        'lOperationTypes.Add("R:", "RCHRES")

        ''Going through each reach that contributes to the main reach (lake)
        'For Each lSource As HspfConnection In lRCHRESOperation.Sources
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
        '        lCountNumberOfTributaries += 1

        '        Dim lLocationID As String = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.VolId
        '        Dim lAreaTable As DataTable = AreaReportInTableFormat(aHSPFUCI, lOperationTypes, lLocationID)
        '        Dim lselectExpression As String = "Landuse='Total'"
        '        Dim lTotalRows() As DataRow = lAreaTable.Select("Landuse='Total'")
        '        Dim lDrainageAreaKM2 As Double = lTotalRows(0)("TotalArea") * 0.00404686 'Converting areas from ac to km2

        '        Dim lTimeSeriesIsInWDM As Boolean = False

        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "ROFLOW", "ROVOL", 1, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "ROVOL")(0)
        '        End If

        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 0.00123348185532 'Converting flow in ac-ft to hm3
        '        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value
        '        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lCov = lStdEv / lAverage
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""" & lSource.Source.Opn.Description & """,1,1," & Format(lDrainageAreaKM2, "0.00") & "," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00") & ",0")

        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""CONSERVATIVE SUBST."",0,0")
        '        'Get the total P concentration data
        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "PLANK", "PKST4", 2, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "P-TOT-CONC")(0)
        '        End If
        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        '        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
        '        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lCov = lStdEv / lAverage
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""TOTAL P""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))

        '        'Get the total N concentration
        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "PLANK", "PKST4", 1, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "N-TOT-CONC")(0)
        '        End If
        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        '        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
        '        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lCov = lStdEv / lAverage
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""TOTAL N""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))

        '        'Get the ortho P concentration
        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "NUTRX", "DNUST", 4, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "PO4-CONCDIS")(0)
        '        End If
        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)

        '        lAverage = lTimeseries.Attributes.GetDefinedValue("Mean").Value * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
        '        lStdEv = lTimeseries.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lCov = lStdEv / lAverage
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""ORTHO P""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))

        '        'Get the inorganic N concentration (sum of NO3, NO2, and TAM)

        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "NUTRX", "DNUST", 1, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO3-CONCDIS")(0).Attributes.GetDefinedValue("Average").Value * 1000
        '        End If
        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        '        Dim lInorgN As atcTimeseries = lTimeseries

        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "NUTRX", "DNUST", 2, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "TAM-CONCDIS")(0)
        '        End If
        '        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        '        lInorgN += lTimeseries

        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, aReachId, "NUTRX", "DNUST", 3, 1, lTimeSeriesIsInWDM)
        '        If lTimeSeriesIsInWDM = True Then
        '            lTimeSeriesIsInWDM = False
        '        Else
        '            lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "NO2-CONCDIS")(0)
        '        End If
        '        If Not lTimeseries Is Nothing Then
        '            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUYear, 1, atcTran.TranAverSame)
        '            lInorgN += lTimeseries
        '        End If
        '        lAverage = lInorgN.Attributes.GetDefinedValue("Mean").Value * 1000 'Original concentration is in mg/l or ppm. Multiplying it by 1000, makes it ppb
        '        lStdEv = lInorgN.Attributes.GetDefinedValue("Standard Deviation").Value
        '        lCov = lStdEv / lAverage
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""INORGANIC N""," & Format(lAverage, "0.00") & "," & Format(lCov, "0.00"))
        '        BATHTUBInputFile.AppendLine(lCountNumberOfTributaries & ",""LandUses"",0,0,0,0,0,0,0,0")
        '    End If
        'Next

        ''Information about the local lakeshed

        'Dim lNPSTable As New DataTable
        'Dim lColumn As DataColumn
        'lColumn = New DataColumn
        'lColumn.ColumnName = "LandUseName"
        'lColumn.DataType = Type.GetType("System.String")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "Area_km2"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "Water_Vol_m"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "Water_Vol_COV"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "PO4_mg_m3"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "PO4_COV"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "TP_mg_m3"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "TP_COV"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "InorgN_mg_m3"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "InorgN_COV"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "TN_mg_m3"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'lColumn = New DataColumn
        'lColumn.ColumnName = "TN_COV"
        'lColumn.DataType = Type.GetType("System.Double")
        'lNPSTable.Columns.Add(lColumn)

        'Dim lRow As DataRow
        'For Each lSource As HspfConnection In lRCHRESOperation.Sources
        '    If Not lSource.Source.Opn Is Nothing AndAlso (lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND") Then
        '        Dim lLocationID As String = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.VolId
        '        Dim lMassLinkID As Integer = lSource.MassLink
        '        lRow = lNPSTable.NewRow
        '        lRow("LandUseName") = lSource.Source.VolName.Substring(0, 1) & ":" & lSource.Source.Opn.Description
        '        lRow("Area_km2") = lSource.MFact * 0.00404686 'Converting area from ac to km2
        '        Dim WaterVolumeInMAndCov As Double() = CalculateVolumeInMeters(lSource.Source.Opn, aBinaryData, aSDateJ, aEDateJ, lLocationID)
        '        lRow("Water_Vol_m") = WaterVolumeInMAndCov(0)
        '        lRow("Water_Vol_COV") = WaterVolumeInMAndCov(1)
        '        'The loading rate from the function is in lbs/ac. Multiplying it by 112.085 converts to mg/m2. Dividing it by water depth, converts to mg/m3
        '        Dim NutrientLoadAndCOV As Double() = CalculatePhosphorus(lSource.Source.Opn, aBinaryData, lRCHRESOperation, lMassLinkID, aSDateJ, aEDateJ, aHSPFUCI)
        '        lRow("PO4_mg_m3") = NutrientLoadAndCOV(0) / WaterVolumeInMAndCov(0)
        '        lRow("PO4_COV") = NutrientLoadAndCOV(1)
        '        lRow("TP_mg_m3") = NutrientLoadAndCOV(2) / WaterVolumeInMAndCov(0)
        '        lRow("TP_COV") = NutrientLoadAndCOV(3)
        '        NutrientLoadAndCOV = CalculateNitrogen(lSource.Source.Opn, aBinaryData, lRCHRESOperation, lMassLinkID, aSDateJ, aEDateJ, aHSPFUCI)
        '        lRow("InorgN_mg_m3") = NutrientLoadAndCOV(0) / WaterVolumeInMAndCov(0)
        '        lRow("InorgN_COV") = NutrientLoadAndCOV(1)
        '        lRow("TN_mg_m3") = NutrientLoadAndCOV(2) / WaterVolumeInMAndCov(0)
        '        lRow("TN_COV") = NutrientLoadAndCOV(3)
        '        lNPSTable.Rows.Add(lRow)
        '    End If
        'Next
        'Dim lakeshedAreaKm2 As Double = lNPSTable.Compute("SUM(Area_km2)", "")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""LakeShed"",1,2," & Format(lakeshedAreaKm2, "0.00") & ",0,0,0") ' & Format(lAverage, "0.00") & "," & Format(lCov, "0.00") & ",0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""CONSERVATIVE SUBST."",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL P"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""TOTAL N"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""ORTHO P"",0,0")
        'BATHTUBInputFile.AppendLine(lCountNumberOfTributaries + 1 & ",""INORGANIC N"",0,0")
        'Dim landuseAreakm2 As String = lCountNumberOfTributaries + 1 & ",""LandUses"","
        'Dim lNumberOfLandUses As Integer = lNPSTable.Rows.Count
        'For Each lNPSTableRow As DataRow In lNPSTable.Rows
        '    landuseAreakm2 &= Format(lNPSTableRow("Area_km2"), "0.00") & ","
        'Next
        'If lNumberOfLandUses < 8 Then
        '    For i As Integer = 1 To 8 - lNumberOfLandUses
        '        landuseAreakm2 &= "0,"
        '    Next
        'End If
        'landuseAreakm2 = landuseAreakm2.Substring(0, landuseAreakm2.Length - 1)

        'BATHTUBInputFile.AppendLine(landuseAreakm2)
        'BATHTUBInputFile.AppendLine("0,""Channels""")
        'BATHTUBInputFile.AppendLine("8,""Land Use Export Categories""")
        'Dim LandUseNumber As Integer = 0
        'For Each lNPSTableRow As DataRow In lNPSTable.Rows
        '    LandUseNumber += 1
        '    BATHTUBInputFile.AppendLine(LandUseNumber & "," & """" & lNPSTableRow("LandUseName") & """")
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""Runoff""," & Format(lNPSTableRow("Water_Vol_m"), "0.00") & "," & Format(lNPSTableRow("Water_Vol_COV"), "0.00"))
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""CONSERVATIVE SUBST."",0,0")
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""TOTAL P""," & Format(lNPSTableRow("TP_mg_m3"), "0.00") & "," & Format(lNPSTableRow("TP_COV"), "0.00"))
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""TOTAL N""," & Format(lNPSTableRow("TN_mg_m3"), "0.00") & "," & Format(lNPSTableRow("TN_COV"), "0.00"))
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""ORTHO P""," & Format(lNPSTableRow("PO4_mg_m3"), "0.00") & "," & Format(lNPSTableRow("PO4_COV"), "0.00"))
        '    BATHTUBInputFile.AppendLine(LandUseNumber & ",""INORGANIC N""," & Format(lNPSTableRow("InorgN_mg_m3"), "0.00") & "," & Format(lNPSTableRow("InorgN_COV"), "0.00"))
        'Next

        'If lNumberOfLandUses < 8 Then
        '    For i As Integer = 1 To 8 - lNumberOfLandUses
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & "," & """""")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""Runoff"",0,0")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""CONSERVATIVE SUBST."",0,0")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""TOTAL P"",0,0")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""TOTAL N"",0,0")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""ORTHO P"",0,0")
        '        BATHTUBInputFile.AppendLine(i + lNumberOfLandUses & ",""INORGANIC N"",0,0")
        '    Next
        'End If


        'BATHTUBInputFile.AppendLine("""Notes""")
        'BATHTUBInputFile.AppendLine("Write Whatever you Want!!!")
        'For i As Integer = 0 To 8
        '    BATHTUBInputFile.AppendLine()
        'Next



        'File.WriteAllText(aOutputfolder & "BATHTUB_" & aReachId & ".btb", BATHTUBInputFile.ToString)

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
        Dim lAverageTS As atcTimeseries = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
        lOrthoPAndTP(0) = lAverageTS.Attributes.GetDefinedValue("SumAnnual").Value
        lOrthoPAndTP(1) = lAverageTS.Attributes.GetDefinedValue("Standard Deviation").Value / lOrthoPAndTP(0)

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
            lTotalTS += lTS * lMassLinkFactor
        Next
        lTotalTS = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
        lOrthoPAndTP(2) = lTotalTS.Attributes.GetDefinedValue("SumAnnual").Value
        lOrthoPAndTP(3) = lTotalTS.Attributes.GetDefinedValue("Standard Deviation").Value / lOrthoPAndTP(2)

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
            lTotalTS += lTS * lMassLinkFactor
        Next
        Dim lAverageTS As atcTimeseries = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2

        lInorNAndTotalN(0) = lAverageTS.Attributes.GetDefinedValue("SumAnnual").Value
        lInorNAndTotalN(1) = lAverageTS.Attributes.GetDefinedValue("Standard Deviation").Value / lInorNAndTotalN(0)

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
            lTotalTS += lTS * lMassLinkFactor
        Next
        lTotalTS = Aggregate(lTotalTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv) * 112.085 'Converting lbs/ac to mg/m2
        lInorNAndTotalN(2) = lTotalTS.Attributes.GetDefinedValue("SumAnnual").Value
        lInorNAndTotalN(3) = lTotalTS.Attributes.GetDefinedValue("Standard Deviation").Value / lInorNAndTotalN(2)


        Return lInorNAndTotalN

    End Function
End Module

