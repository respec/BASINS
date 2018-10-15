Imports atcUCI
Imports atcData
Imports System.IO
Imports System.Data
Imports HspfSupport
'Imports atcWASPInpWriter
Imports System.Collections.Specialized
''' <summary>
''' This module prepares a WASP input file. It assumes that the HSPF model is run using English Units
''' </summary>
Module WASP
    Sub WASPInputFile(ByVal aHSPFUCI As HspfUci, ByVal aBinaryData As atcDataSource,
                         ByVal aSDateJ As Double, ByVal aEDateJ As Double, ByVal aReachId As Integer,
                         ByVal aOutputfolder As String)

        'Dim lWaspProject As New atcWASPProject
        'Dim lFileName As String = System.IO.Path.Combine(aOutputfolder, "WASP_R" & aReachId.ToString & ".inp")
        'lWaspProject.SDate = Date.FromOADate(aSDateJ)
        'lWaspProject.EDate = Date.FromOADate(aEDateJ)

        ''assuming eutrophication model
        'lWaspProject.WASPConstituents = New Generic.List(Of clsWASPConstituent)
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

        ''need segments -- will make some assumptions here but for now we'll add three
        ''lWaspProject.Segments.Add(New atcWASPSegment(lWaspProject.WASPConstituents.Count))

        'Dim lSeg As New atcWASPSegment(lWaspProject.WASPConstituents.Count)
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

        ''initialize time series for each segment and all time functions
        'For i As Integer = 0 To lWaspProject.Segments.Count - 1
        '    With lWaspProject.Segments(i)
        '        .FlowTimeSeries = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
        '        ReDim .LoadTimeSeries(lWaspProject.WASPConstituents.Count - 1)
        '        ReDim .BoundTimeSeries(lWaspProject.WASPConstituents.Count - 1)
        '        For j As Integer = 0 To lWaspProject.WASPConstituents.Count - 1
        '            .LoadTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
        '            .BoundTimeSeries(j) = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.None)
        '        Next
        '    End With
        'Next

        ''now ready to write
        'lWaspProject.WriteINP(lFileName)
    End Sub
End Module

