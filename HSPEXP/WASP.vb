Imports atcUCI
Imports atcData
Imports atcUtility
Imports System.IO
Imports System.Data
Imports HspfSupport
Imports atcWASPInpWriter
Imports System.Collections.Specialized
''' <summary>
''' This module prepares a WASP input file. It assumes that the HSPF model is run using English Units
''' </summary>
Module WASP
    Sub WASPInputFile(ByVal aHSPFUCI As HspfUci, ByVal aBinaryData As atcDataSource,
                         ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal aReachId As Integer,
                         ByVal aOutputfolder As String)

        Dim lWaspProject As New atcWASPProject
        Dim lFileName As String = System.IO.Path.Combine(aOutputfolder, "WASP_R" & aReachId.ToString & ".inp")
        lWaspProject.SDate = Date.FromOADate(aSDateJ)
        lWaspProject.EDate = Date.FromOADate(aEDateJ)

        'assuming eutrophication model
        lWaspProject.WASPConstituents = New Generic.List(Of clsWASPConstituent)
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Ammonia (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Nitrate (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Organic Nitrogen (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Orthophosphate (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Organic Phosphorus (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Phytoplankton Chla (ug/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 1 (Ultimate) (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 2 (Ultimate) (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 3 (Ultimate) (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Dissolved Oxygen (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Carbon (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Nitrogen (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Phosphorus (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Salinity (ppt)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Solids (mg/L)", "", ""))

        'need segments -- will make some assumptions here 
        Dim lNSegs As Integer = 1
        Dim lLength_km As Double = 1.0
        Dim lSlope As Double = 0.01
        Dim lDepth_m As Double = 1.0
        Dim lWidth_m As Double = 10.0
        Dim lVolume_m3 As Double = 100.0
        Dim lSegName As String = "Segment"
        If aHSPFUCI.OperationExists("RCHRES", aReachId) Then
            lLength_km = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).Tables("HYDR-PARM2").Parms("LEN").Value * 1.60934 'Converting stream length in miles to km
            lSlope = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).Tables("HYDR-PARM2").Parms("DELTH").Value * 0.3048 / (lLength_km * 1000.0)  'Converting delth in ft to m
            lSegName = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).Tables("GEN-INFO").Parms("RCHID").Value
            'Depth (ft)= a*DrainageArea^b (english):  a= 1.5; b=0.284    -- assumption from GBMM used in BASINS WASP plugin
            '   drainage area appears to be in sq km
            Dim lOperationTypes As New atcCollection
            lOperationTypes.Add("P:", "PERLND")
            lOperationTypes.Add("I:", "IMPLND")
            lOperationTypes.Add("R:", "RCHRES")
            lOperationTypes.Add("B:", "BMPRAC")
            Dim lContributingLandUseAreas As atcCollection = ContributingLandUseAreas(aHSPFUCI, lOperationTypes, "R:" & aReachId.ToString)
            Dim lDrainageArea As Double = 0.0
            For Each lArea In lContributingLandUseAreas
                lDrainageArea += StrRetRem(lArea)
                If lArea.Length > 0 Then
                    lDrainageArea += lArea
                End If
            Next
            lDrainageArea = lDrainageArea / 247.105   'to convert acres to sq km
            Dim lDepth = 1.5 * (lDrainageArea ^ 0.284)   'gives depth in ft
            lDepth_m = lDepth / 3.281   'depth in m

            'basins uses these formulae for depth and width from drainage area 
            Dim lDepthFromBASINS As Double = (0.13) * ((lDrainageArea) ^ (0.4))   'meters
            Dim lWidthFromBASINS As Double = (1.29) * ((lDrainageArea) ^ (0.6))   'meters

            'from FTABLE can get surface area, volume, and discharge at this depth
            Dim lFlowThru_days As Double = 0.0
            Dim lHspfFtable As HspfFtable = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).FTable
            For lRow As Integer = 1 To lHspfFtable.Nrows
                If lHspfFtable.Depth(lRow) > lDepth Then
                    'use this as approximation
                    lWidth_m = lHspfFtable.Area(lRow) * 4046.856 / (lLength_km * 1000.0)   'converting acres to m2
                    lVolume_m3 = lHspfFtable.Volume(lRow) * 1233.48            'converting acft to m3
                    If lHspfFtable.Outflow1(lRow) > 0.0 Then
                        lFlowThru_days = (lHspfFtable.Volume(lRow) * 43560 / lHspfFtable.Outflow1(lRow)) / (60 * 60 * 24)  'assuming first exit is the main one
                    End If
                    Exit For
                End If
            Next
            Dim lMinFlowThruDays As Double = 0.1  '0.1?
            If lFlowThru_days > lMinFlowThruDays Then
                'break up to keep less than 0.1 days of flow-thru time
                lNSegs = CInt((lFlowThru_days / lMinFlowThruDays) + 0.5)
                lLength_km = lLength_km / lNSegs
                lVolume_m3 = lVolume_m3 / lNSegs
            End If
        End If

        For i As Integer = 1 To lNSegs
            Dim lSeg As New atcWASPSegment(lWaspProject.WASPConstituents.Count)
            lSeg.BaseID = Str(i)
            lSeg.Depth = lDepth_m
            If i = 1 Then
                lSeg.DownID = ""
            Else
                lSeg.DownID = Str(i - 1)
            End If
            lSeg.ID = Str(i)
            lSeg.Length = lLength_km
            lSeg.Name = lSegName & Str(i)
            lSeg.Slope = lSlope
            lSeg.WaspID = i
            lSeg.WaspName = Str(i)
            lSeg.Width = lWidth_m
            lWaspProject.Segments.Add(lSeg)
        Next

        'Dim lSeg As New atcWASPSegment(lWaspProject.WASPConstituents.Count)    'leaving these here for testing
        'lSeg.BaseID = "1"
        'lSeg.BoundTimeSeries = Nothing
        'lSeg.CentroidX = 0.0
        'lSeg.CentroidY = 0.0
        'lSeg.CumulativeDrainageArea = 0.0
        'lSeg.Depth = 0.0
        'lSeg.Divergence = 1
        'lSeg.DownID = ""
        'lSeg.FlowTimeSeries = Nothing
        'lSeg.ID = "1"
        'lSeg.Length = 1.0
        'lSeg.LoadTimeSeries = Nothing
        'lSeg.MeanAnnualFlow = 1.0
        'lSeg.Name = "1"
        'lSeg.Roughness = 0.05
        'lSeg.Slope = 0.01
        'lSeg.Velocity = 1.0
        'lSeg.WaspID = 1
        'lSeg.WaspName = "1"
        'lSeg.Width = 1.0
        'lWaspProject.Segments.Add(lSeg)

        'Dim lSeg2 As New atcWASPSegment(lWaspProject.WASPConstituents.Count)
        'lSeg2.BaseID = "2"
        'lSeg2.BoundTimeSeries = Nothing
        'lSeg2.CentroidX = 0.0
        'lSeg2.CentroidY = 0.0
        'lSeg2.CumulativeDrainageArea = 0.0
        'lSeg2.Depth = 0.0
        'lSeg2.Divergence = 1
        'lSeg2.DownID = "1"
        'lSeg2.FlowTimeSeries = Nothing
        'lSeg2.ID = "2"
        'lSeg2.Length = 1.0
        'lSeg2.LoadTimeSeries = Nothing
        'lSeg2.MeanAnnualFlow = 1.0
        'lSeg2.Name = "2"
        'lSeg2.Roughness = 0.05
        'lSeg2.Slope = 0.01
        'lSeg2.Velocity = 1.0
        'lSeg2.WaspID = 2
        'lSeg2.WaspName = "2"
        'lSeg2.Width = 1.0
        'lWaspProject.Segments.Add(lSeg2)

        'Dim lSeg3 As New atcWASPSegment(lWaspProject.WASPConstituents.Count)
        'lSeg3.BaseID = "3"
        'lSeg3.BoundTimeSeries = Nothing
        'lSeg3.CentroidX = 0.0
        'lSeg3.CentroidY = 0.0
        'lSeg3.CumulativeDrainageArea = 0.0
        'lSeg3.Depth = 0.0
        'lSeg3.Divergence = 1
        'lSeg3.DownID = "1"
        'lSeg3.FlowTimeSeries = Nothing
        'lSeg3.ID = "3"
        'lSeg3.Length = 1.0
        'lSeg3.LoadTimeSeries = Nothing
        'lSeg3.MeanAnnualFlow = 1.0
        'lSeg3.Name = "3"
        'lSeg3.Roughness = 0.05
        'lSeg3.Slope = 0.01
        'lSeg3.Velocity = 1.0
        'lSeg3.WaspID = 3
        'lSeg3.WaspName = "3"
        'lSeg3.Name = "3"
        'lSeg3.Width = 1.0
        'lWaspProject.Segments.Add(lSeg3)

        'initialize time series for each segment and all time functions
        For i As Integer = 0 To lWaspProject.Segments.Count - 1
            With lWaspProject.Segments(i)
                .FlowTimeSeries = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                ReDim .LoadTimeSeries(lWaspProject.WASPConstituents.Count - 1)
                ReDim .BoundTimeSeries(lWaspProject.WASPConstituents.Count - 1)
                For j As Integer = 0 To lWaspProject.WASPConstituents.Count - 1
                    .LoadTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                    .BoundTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
                Next
            End With
        Next

        'One way to find input timeseries is to look in WDM file for upstream contributors.
        'The concern with this is that all tribs have to be added together, plus we need local inflows.
        'Look for timeseries from contributing reaches 
        Dim lReach As HspfOperation = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId)
        Dim lTimeseries As atcTimeseries = Nothing
        'Dim lCompositeTimeseries As atcTimeseries = Nothing
        'Dim lCountTimeseries As Integer = 0
        'For Each lSource As HspfConnection In lReach.Sources
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
        '        'here's a contributing reach
        '        Dim lTimeSeriesIsInWDM As Boolean = False
        '        Dim lTimeseries As atcTimeseries = Nothing
        '        lTimeseries = LocateTheTimeSeries(aHSPFUCI, lSource.Source.VolId, "HYDR", "RO", 1, 1, lTimeSeriesIsInWDM)
        '        If lTimeseries IsNot Nothing Then
        '            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) / 35.315 'Converting flow cfs to cms
        '        End If
        '        If lTimeseries IsNot Nothing Then
        '            If lCountTimeseries = 0 Then
        '                lCompositeTimeseries = lTimeseries
        '            Else
        '                lCompositeTimeseries = lCompositeTimeseries + lTimeseries
        '            End If
        '            lCountTimeseries += 1
        '        End If
        '    End If
        'Next

        'a lot more straightforward is just to use the ivol term from the bino file
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "IVOL")(0)
        If lTimeseries IsNot Nothing Then
            'convert from ivol in ac.ft/ivld to cms
            Dim lConvFact As Double = 1233.48 / (24 * 60 * 60)
            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv) * lConvFact
            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
        End If
        If lTimeseries IsNot Nothing Then
            lWaspProject.Segments(lNSegs - 1).FlowTimeSeries = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Database)
            lWaspProject.Segments(lNSegs - 1).FlowTimeSeries.ts = lTimeseries
        End If

        'also add loadings at the upstream boundary (needs to be kg/day)

        'Ammonia              TAM-INTOT (lbs)
        Dim lConvFactP As Double = 1 / 2.205
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "TAM-INTOT", lConvFactP, 0, aSDateJ, aEDateJ)

        'Nitrate              NO3-INTOT (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "NO3-INTOT", lConvFactP, 1, aSDateJ, aEDateJ)

        'Organic Nitrogen     N-TOTORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "N-TOTORG-IN", lConvFactP, 2, aSDateJ, aEDateJ)

        'Orthophosphate       PO4-INTOT (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "PO4-INTOT", lConvFactP, 3, aSDateJ, aEDateJ)

        'Organic Phosphorus   P-TOTORG-IN (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "P-TOTORG-IN", lConvFactP, 4, aSDateJ, aEDateJ)

        'Phytoplankton Chla   PHYTO-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "PHYTO-IN", lConvFactP, 5, aSDateJ, aEDateJ)

        'CBOD 1(Ultimate)     BODIN (lbs) 
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "BODIN", lConvFactP, 6, aSDateJ, aEDateJ)

        'CBOD 2(Ultimate)     *** 
        'CBOD 3(Ultimate)     *** 

        'Dissolved Oxygen     DOXIN (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "DOXIN", lConvFactP, 9, aSDateJ, aEDateJ)

        'Detrital Carbon      C-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "C-REFORG-IN", lConvFactP, 10, aSDateJ, aEDateJ)

        'Detrital Nitrogen    N-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "N-REFORG-IN", lConvFactP, 11, aSDateJ, aEDateJ)

        'Detrital Phosphorus  P-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "P-REFORG-IN", lConvFactP, 12, aSDateJ, aEDateJ)

        'Salinity             ***

        'Solids               ISED-TOT (tons)
        Dim lConvFactT As Double = 907.185  'tons to kg
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "ISED-TOT", lConvFactT, 14, aSDateJ, aEDateJ)

        'now ready to write
        lWaspProject.WriteINP(lFileName)
    End Sub

    Sub LinkBinoTimeseriesToWASPLoadTimeseries(ByRef aWaspProject As atcWASPProject, ByVal aBinaryData As atcDataSource,
                                               ByVal aReachId As Integer, ByVal aConstituent As String,
                                               ByVal aConvFact As Double, ByVal aLoadID As Integer,
                                               ByVal aSDateJ As Double, ByVal aEDateJ As Double)
        Dim lTimeseries As atcTimeseries = Nothing
        lTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", aConstituent)(0)
        If lTimeseries IsNot Nothing Then
            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv) * aConvFact
            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
        End If
        If lTimeseries IsNot Nothing Then
            aWaspProject.Segments(aWaspProject.Segments.Count - 1).LoadTimeSeries(aLoadID) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Database)
            aWaspProject.Segments(aWaspProject.Segments.Count - 1).LoadTimeSeries(aLoadID).ts = lTimeseries
        End If
    End Sub
End Module

