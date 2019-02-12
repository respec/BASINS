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
        Dim lOutputFolder As String = System.IO.Path.Combine(aOutputfolder, "WASP")
        FileIO.FileSystem.CreateDirectory(lOutputFolder)
        Dim lFileName As String = System.IO.Path.Combine(lOutputFolder, "WASP_R" & aReachId.ToString & ".inp")
        lWaspProject.SDate = Date.FromOADate(aSDateJ)
        lWaspProject.EDate = Date.FromOADate(aEDateJ)

        'assuming eutrophication model
        lWaspProject.WASPConstituents = New Generic.List(Of clsWASPConstituent)
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Ammonia (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Nitrate (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Organic Nitrogen (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Orthophosphate (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Organic Phosphorus (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Phytoplankton Chla (ug/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 1 (Ultimate) (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 2 (Ultimate) (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD 3 (Ultimate) (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Dissolved Oxygen (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Carbon (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Nitrogen (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Phosphorus (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Salinity (ppt)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Solids (mg/L)", "", ""))
        'need to use the list below for wasp 8 advanced eutrophication
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Ammonia Nitrogen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Nitrate Nitrogen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Diss Organic Nitrogen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Inorganic Phosphate", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Diss Organic Phosphorus", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Inorganic Silica", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Diss Organic Silica", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD1(ultimate)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD2(ultimate)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("CBOD3(ultimate)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Dissolved Oxygen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Carbon", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Nitrogen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Phosphorus", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Detrital Silica", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Total Detritus", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Salinity(PSU) Or TDS (mg/L)", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Benthic Algae", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Periphyton Cell Quota Nitrogen", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Periphyton Cell Quota Phosphorous", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Inorganic Solids 1", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Inorganic Solids 2", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Inorganic Solids 3", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Phytoplankton 1", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Phytoplankton 2", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Phytoplankton 3", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Total Inorganic C", "", ""))
        lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Alkalinity", "", ""))

        'if model type is heat
        'lWaspProject.WASPConstituents = New Generic.List(Of clsWASPConstituent)
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Temperature (°C)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Salinity (ppt)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Bacteria (#/100 ml)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Silts And Fines (mg/L)", "", ""))
        'lWaspProject.WASPConstituents.Add(New clsWASPConstituent("Sands (mg/L)", "", ""))

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

            'we need depth, width, and volume -- we may be able to get these from the HBN
            Dim lDepth As Double = -999
            Dim lTopWidth As Double = -999
            Dim lVol As Double = -999
            Dim lFlow As Double = -999
            Dim lFlowThru_days As Double = 0.0
            Dim lDepthTimeseries As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "DEP")(0)
            If lDepthTimeseries IsNot Nothing Then
                lDepth = lDepthTimeseries.Attributes.GetDefinedValue("Mean").Value
            End If
            Dim lWidthTimeseries As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "TWID")(0)
            If lWidthTimeseries IsNot Nothing Then
                lTopWidth = lWidthTimeseries.Attributes.GetDefinedValue("Mean").Value
            End If
            Dim lVolTimeseries As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "VOL")(0)
            If lVolTimeseries IsNot Nothing Then
                lVol = lVolTimeseries.Attributes.GetDefinedValue("Mean").Value
            End If
            Dim lFlowTimeseries As atcTimeseries = aBinaryData.DataSets.FindData("Location", "R:" & aReachId).FindData("Constituent", "RO")(0)
            If lFlowTimeseries IsNot Nothing Then
                lFlow = lFlowTimeseries.Attributes.GetDefinedValue("Mean").Value
            End If
            lDepth_m = lDepth / 3.281      'depth in m
            lWidth_m = lTopWidth / 3.281   'width in m
            lVolume_m3 = lVol * 1233.48            'converting acft to m3
            If lFlow > 0.0 Then
                lFlowThru_days = (lVol * 43560 / lFlow) / (60 * 60 * 24)
            End If

            If lDepth < 0 Then
                'Alternate method of getting depth, width, volume if HBN is not available 
                Dim lOperationTypes As New atcCollection
                Dim lContributingLandUseAreas As New atcCollection
                Dim lDrainageArea As Double = 0.0
                'Depth (ft)= a*DrainageArea^b (english):  a= 1.5; b=0.284    -- assumption from GBMM used in BASINS WASP plugin
                '   drainage area appears to be in sq km
                lOperationTypes.Add("P:", "PERLND")
                lOperationTypes.Add("I:", "IMPLND")
                lOperationTypes.Add("R:", "RCHRES")
                lOperationTypes.Add("B:", "BMPRAC")
                lContributingLandUseAreas = ContributingLandUseAreas(aHSPFUCI, lOperationTypes, "R:" & aReachId.ToString)
                For Each lArea In lContributingLandUseAreas
                    lDrainageArea += StrRetRem(lArea)
                    If lArea.Length > 0 Then
                        lDrainageArea += lArea
                    End If
                Next
                lDrainageArea = lDrainageArea / 247.105   'to convert acres to sq km
                lDepth = 1.5 * (lDrainageArea ^ 0.284)   'gives depth in ft
                lDepth_m = lDepth / 3.281   'depth in m
                'basins uses these formulae for depth and width from drainage area 
                'Dim lDepthFromBASINS As Double = (0.13) * ((lDrainageArea) ^ (0.4))   'meters
                'Dim lWidthFromBASINS As Double = (1.29) * ((lDrainageArea) ^ (0.6))   'meters
                'from FTABLE can get surface area, volume, and discharge at this depth
                Dim lHspfFtable As HspfFtable = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId).FTable
                For lRow As Integer = 1 To lHspfFtable.Nrows
                    If lHspfFtable.Depth(lRow) > lDepth Then
                        'use this as approximation
                        lWidth_m = lHspfFtable.Area(lRow) * 4046.856 / (lLength_km * 1000.0)   'converting acres to m2
                        lVolume_m3 = lHspfFtable.Volume(lRow) * 1233.48            'converting acft to m3
                        If lHspfFtable.Outflow1(lRow) > 0.0 Then
                            lFlowThru_days = (lHspfFtable.Volume(lRow) * 43560 / lHspfFtable.Outflow1(lRow)) / (60 * 60 * 24)  'assuming first exit is the main one
                            Exit For
                        End If
                    End If
                Next
            End If

            Dim lMinFlowThruDays As Double = 0.1  '0.1?
            If lFlowThru_days > lMinFlowThruDays Then
                'break up to keep less than 0.1 days of flow-thru time
                lNSegs = CInt((lFlowThru_days / lMinFlowThruDays) + 0.5)
                'with this many segments, is the width much larger than the length?
                If lWidth_m > lLength_km * 1000 * 10 / lNSegs Then
                    'make the segments about as long as they are wide
                    lNSegs = ((lLength_km * 1000) / lWidth_m) + 1
                End If
                If lNSegs = 0 Then lNSegs = 1
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

        'One way to find input timeseries is to look for upstream contributors.
        Dim lReach As HspfOperation = aHSPFUCI.OpnBlks("RCHRES").OperFromID(aReachId)
        Dim lConvFactP As Double = 1 / 2.205    'from pounds to kg
        Dim lConvFactT As Double = 907.185      'tons to kg
        Dim lConvFactF As Double = 1 / 35.315   'from cfs to cms
        Dim lConvFactV As Double = 102.79 / (24 * 60 * 60)   'from ac.in/ivld To cms
        'Look for timeseries from contributing reaches -- write them out
        'Look for local inflows -- write them out
        Dim lTimeseries As atcTimeseries = Nothing
        'For Each lSource As HspfConnection In lReach.Sources
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" Then
        '        'here's a contributing reach
        '        Dim lContributingReachId As String = "R:" & lSource.Source.VolId
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "RO", lConvFactF, aSDateJ, aEDateJ, aOutputfolder)          'Flow
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "TAM-INTOT", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)   'Ammonia Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "NO3-INTOT", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)   'Nitrate Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "N-TOTORG-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder) 'Dissolved Organic Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "PO4-INTOT", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)   'Inorganic Phosphate
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "P-TOTORG-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder) 'Dissolved Organic Phosphorus
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "PHYTO-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)    'Phytoplankton Chla
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "BODIN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)       'CBOD 1(Ultimate)
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "DOXIN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder)       'Dissolved Oxygen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "C-REFORG-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder) 'Detrital Carbon
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "N-REFORG-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder) 'Detrital Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "P-REFORG-IN", lConvFactP, aSDateJ, aEDateJ, aOutputfolder) 'Detrital Phosphorus
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingReachId, "R:" & aReachId, "ISED-TOT", lConvFactT, aSDateJ, aEDateJ, aOutputfolder)    'Solids
        '    End If
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "PERLND" Then
        '        'here's a contributing perlnd
        '        Dim lContributingId As String = "P:" & lSource.Source.VolId
        '        Dim lMult As Double = lSource.MFact
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "PERO", lConvFactV * lMult, aSDateJ, aEDateJ, aOutputfolder)            'Flow
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOSED", lConvFactT * lMult, aSDateJ, aEDateJ, aOutputfolder)           'Solids
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "PODOXM", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)          'Dissolved Oxygen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "POQUAL-NH4", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'Ammonia Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "POQUAL-NO3", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'Nitrate Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "POQUAL-ORTHO P", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)  'Inorganic Phosphate
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "POQUAL-BOD", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'BOD for Total Organic Nitrogen, Phosphorus, and BOD
        '    End If
        '    If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "IMPLND" Then
        '        'here's a contributing implnd
        '        Dim lContributingId As String = "I:" & lSource.Source.VolId
        '        Dim lMult As Double = lSource.MFact
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SURO", lConvFactV * lMult, aSDateJ, aEDateJ, aOutputfolder)            'Flow
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOSLD", lConvFactT * lMult, aSDateJ, aEDateJ, aOutputfolder)           'Solids
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SODOXM", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)          'Dissolved Oxygen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOQUAL-NH4", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'Ammonia Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOQUAL-NO3", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'Nitrate Nitrogen
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOQUAL-ORTHO P", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)  'Inorganic Phosphate
        '        WriteBinoTimeseriesForWASP(aBinaryData, lContributingId, "R:" & aReachId, "SOQUAL-BOD", lConvFactP * lMult, aSDateJ, aEDateJ, aOutputfolder)      'BOD for Total Organic Nitrogen, Phosphorus, and BOD
        '    End If
        'Next
        'alternate scheme to write individual and composite timeseries
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "RO", lConvFactF, aSDateJ, aEDateJ, lOutputFolder)           'Flow
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "TAM-OUTTOT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)   'Ammonia Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "NO3-OUTTOT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)   'Nitrate Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "N-TOTORG-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder) 'Dissolved Organic Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "PO4-OUTTOT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)   'Inorganic Phosphate
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "P-TOTORG-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder) 'Dissolved Organic Phosphorus
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "PHYTO-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)    'Phytoplankton Chla
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "BODOUTTOT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)    'CBOD 1(Ultimate)
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "DOXOUTTOT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)    'Dissolved Oxygen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "C-REFORG-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder) 'Detrital Carbon
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "N-REFORG-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder) 'Detrital Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "P-REFORG-OUT", lConvFactP, aSDateJ, aEDateJ, lOutputFolder) 'Detrital Phosphorus
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "R", "ROSED-TOT", lConvFactT, aSDateJ, aEDateJ, lOutputFolder)    'Solids
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "PERO", lConvFactV, aSDateJ, aEDateJ, lOutputFolder)            'Flow
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "SOSED", lConvFactT, aSDateJ, aEDateJ, lOutputFolder)           'Solids
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "PODOXM", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)          'Dissolved Oxygen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "POQUAL-NH4", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)      'Ammonia Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "POQUAL-NO3", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)      'Nitrate Nitrogen
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "POQUAL-ORTHO P", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)  'Inorganic Phosphate
        WriteHSPFTimeseriesForWASP(aBinaryData, lReach, "L", "POQUAL-BOD", lConvFactP, aSDateJ, aEDateJ, lOutputFolder)      'BOD for Total Organic Nitrogen, Phosphorus, and BOD

        'Or from WDM
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

        'Ammonia Nitrogen               TAM-INTOT (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "TAM-INTOT", lConvFactP, 0, aSDateJ, aEDateJ)

        'Nitrate Nitrogen               NO3-INTOT (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "NO3-INTOT", lConvFactP, 1, aSDateJ, aEDateJ)

        'Dissolved Organic Nitrogen     N-TOTORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "N-TOTORG-IN", lConvFactP, 2, aSDateJ, aEDateJ)

        'Inorganic Phosphate            PO4-INTOT (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "PO4-INTOT", lConvFactP, 3, aSDateJ, aEDateJ)

        'Dissolved Organic Phosphorus   P-TOTORG-IN (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "P-TOTORG-IN", lConvFactP, 4, aSDateJ, aEDateJ)

        'Inorganic Silica
        'Diss Organic Silica

        'CBOD 1(Ultimate)               BODIN (lbs) 
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "BODIN", lConvFactP, 7, aSDateJ, aEDateJ)

        'CBOD 2(Ultimate)               *** 
        'CBOD 3(Ultimate)               *** 

        'Dissolved Oxygen               DOXIN (lbs)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "DOXIN", lConvFactP, 10, aSDateJ, aEDateJ)

        'Detrital Carbon                C-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "C-REFORG-IN", lConvFactP, 11, aSDateJ, aEDateJ)

        'Detrital Nitrogen              N-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "N-REFORG-IN", lConvFactP, 12, aSDateJ, aEDateJ)

        'Detrital Phosphorus            P-REFORG-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "P-REFORG-IN", lConvFactP, 13, aSDateJ, aEDateJ)

        'Detrital Silica
        'Total Detritus
        'Salinity                       ***
        'Benthic Algae
        'Periphyton Cell Quota Nitrogen
        'Periphyton Cell Quota Phosphorous

        'Solids                         ISED-TOT (tons)
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "ISED-TOT", lConvFactT, 20, aSDateJ, aEDateJ)

        'Inorganic Solids 2
        'Inorganic Solids 3

        'Phytoplankton Chla             PHYTO-IN (lbs)  
        LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "PHYTO-IN", lConvFactP, 23, aSDateJ, aEDateJ)

        'Phytoplankton 2
        'Phytoplankton 3

        'Total Inorganic C
        'Alkalinity

        ''if using heat model
        ''Temperature (°C)     
        'Dim lConvFactD As Double = 5 / 9  ' F to C conversion needed
        'LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "TW", lConvFactD, 0, aSDateJ, aEDateJ)
        ''Salinity             ***
        ''Bacteria (#/100 ml)  ***  map from gqual?
        ''Silts and Fines
        'LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "ISED-SILT", lConvFactT, 3, aSDateJ, aEDateJ)
        'LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "ISED-CLAY", lConvFactT, 3, aSDateJ, aEDateJ)
        ''Sands 
        'LinkBinoTimeseriesToWASPLoadTimeseries(lWaspProject, aBinaryData, aReachId, "ISED-SAND", lConvFactT, 4, aSDateJ, aEDateJ)

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

    Sub WriteBinoTimeseriesForWASP(ByVal aBinaryData As atcDataSource,
                                   ByVal aOpnId As String, ByVal aDownStreamReachId As String,
                                   ByVal aConstituent As String,
                                   ByVal aConvFact As Double,
                                   ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                   ByVal aOutputfolder As String)
        Dim lFileName As String = System.IO.Path.Combine(aOutputfolder, "WASP_" & aOpnId.Replace(":", "") & "to" & aDownStreamReachId.Replace(":", "") & "_" & aConstituent & ".txt")
        Dim lTimeseries As atcTimeseries = Nothing
        lTimeseries = aBinaryData.DataSets.FindData("Location", aOpnId).FindData("Constituent", aConstituent)(0)
        If lTimeseries IsNot Nothing Then
            If aConstituent = "RO" Then
                lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) * aConvFact
            Else
                lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv) * aConvFact
            End If
            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
            Dim lSW As IO.StreamWriter = Nothing
            lSW = New IO.StreamWriter(lFileName, False)
            For t As Integer = 1 To lTimeseries.Values.Count - 1
                lSW.WriteLine("{0,8:0.000} {1,9:0.00000}", t, lTimeseries.Values(t))
            Next
            lSW.Flush()
            lSW.Close()
        End If
    End Sub

    Sub WriteHSPFTimeseriesForWASP(ByVal aBinaryData As atcDataSource,
                                   ByVal aTargetReach As HspfOperation,
                                   ByVal aType As String,
                                   ByVal aConstituent As String,
                                   ByVal aConvFact As Double,
                                   ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                   ByVal aOutputfolder As String)

        Dim lCompositeTimeseries As atcTimeseries = Nothing
        Dim lDownstreamReachId As String = "R:" & aTargetReach.Id
        For Each lSource As HspfConnection In aTargetReach.Sources
            If Not lSource.Source.Opn Is Nothing AndAlso lSource.Source.VolName = "RCHRES" AndAlso aType = "R" Then
                'here's a contributing reach
                Dim lContributingReachId As String = "R:" & lSource.Source.VolId
                Dim lFileName As String = System.IO.Path.Combine(aOutputfolder, "WASP_" & lContributingReachId.Replace(":", "") & "to" & lDownstreamReachId.Replace(":", "") & "_" & aConstituent & ".txt")
                Dim lTimeseries As atcTimeseries = Nothing
                lTimeseries = aBinaryData.DataSets.FindData("Location", lContributingReachId).FindData("Constituent", aConstituent)(0)
                If lTimeseries IsNot Nothing Then
                    If aConstituent = "RO" Then
                        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranAverSame) * aConvFact
                    Else
                        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv) * aConvFact
                    End If
                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                    Dim lSW As IO.StreamWriter = Nothing
                    lSW = New IO.StreamWriter(lFileName, False)
                    For t As Integer = 1 To lTimeseries.Values.Count - 1
                        lSW.WriteLine("{0,8:0.000},{1,9:0.00000}", t, lTimeseries.Values(t))
                    Next
                    lSW.Flush()
                    lSW.Close()
                    If lCompositeTimeseries Is Nothing Then
                        lCompositeTimeseries = lTimeseries
                    Else
                        lCompositeTimeseries = lCompositeTimeseries + lTimeseries
                    End If
                End If
            End If
            If Not lSource.Source.Opn Is Nothing AndAlso (lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND") AndAlso aType = "L" Then
                'here's a contributing perlnd/implnd
                Dim lContributingLandId As String = "P:" & lSource.Source.VolId
                Dim lConstituent As String = aConstituent
                Dim lMult As Double = lSource.MFact
                If lSource.Source.VolName = "IMPLND" Then
                    'set equivalent implnd names
                    lContributingLandId = "I:" & lSource.Source.VolId
                    If aConstituent = "PERO" Then
                        lConstituent = "SURO"
                    ElseIf aConstituent = "SOSED" Then
                        lConstituent = "SOSLD"
                    ElseIf aConstituent = "PODOXM" Then
                        lConstituent = "SODOXM"
                    ElseIf aConstituent = "POQUAL-NH4" Then
                        lConstituent = "SOQUAL-NH4"
                    ElseIf aConstituent = "POQUAL-NO3" Then
                        lConstituent = "SOQUAL-NO3"
                    ElseIf aConstituent = "POQUAL-ORTHO P" Then
                        lConstituent = "SOQUAL-ORTHO P"
                    ElseIf aConstituent = "POQUAL-BOD" Then
                        lConstituent = "SOQUAL-BOD"
                    End If
                End If
                Dim lFileName As String = System.IO.Path.Combine(aOutputfolder, "WASP_" & lContributingLandId.Replace(":", "") & "to" & lDownstreamReachId.Replace(":", "") & "_" & lConstituent & ".txt")
                Dim lTimeseries As atcTimeseries = Nothing
                lTimeseries = aBinaryData.DataSets.FindData("Location", lContributingLandId).FindData("Constituent", lConstituent)(0)
                If lTimeseries IsNot Nothing Then
                    lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv) * aConvFact * lMult
                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                    Dim lSW As IO.StreamWriter = Nothing
                    lSW = New IO.StreamWriter(lFileName, False)
                    For t As Integer = 1 To lTimeseries.Values.Count - 1
                        lSW.WriteLine("{0,8:0.000},{1,9:0.00000}", t, lTimeseries.Values(t))
                    Next
                    lSW.Flush()
                    lSW.Close()
                    If lCompositeTimeseries Is Nothing Then
                        lCompositeTimeseries = lTimeseries
                    Else
                        lCompositeTimeseries = lCompositeTimeseries + lTimeseries
                    End If
                End If
            End If
        Next
        If lCompositeTimeseries IsNot Nothing Then
            'write out the composite timeseries as well
            Dim lFileName As String = ""
            If aType = "R" Then
                lFileName = System.IO.Path.Combine(aOutputfolder, "WASP_Reach_Sum_to" & lDownstreamReachId.Replace(":", "") & "_" & aConstituent & ".txt")
            Else
                lFileName = System.IO.Path.Combine(aOutputfolder, "WASP_Land_Sum_to" & lDownstreamReachId.Replace(":", "") & "_" & aConstituent & ".txt")
            End If
            Dim lSW As IO.StreamWriter = Nothing
            lSW = New IO.StreamWriter(lFileName, False)
            For t As Integer = 1 To lCompositeTimeseries.Values.Count - 1
                lSW.WriteLine("{0,8:0.000},{1,9:0.00000}", t, lCompositeTimeseries.Values(t))
            Next
            lSW.Flush()
            lSW.Close()
        End If

    End Sub
End Module

