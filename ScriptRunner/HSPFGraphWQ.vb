Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized

Module HSPFGraphWQ

    Private pBaseDrive As String = "C:"
    Private pBaseFolders As New ArrayList
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String
    Private pGraphSaveWidth As Integer
    Private pGraphSaveHeight As Integer
    Private pGraphAnnual As Boolean = False
    Private pCurveStepType As String = "RearwardStep"
    Private pSummaryTypes As New atcCollection
    Private pPerlndSegmentStarts() As Integer
    Private pImplndSegmentStarts() As Integer
    Private pGraphWQOnly As Boolean = True
    Private pModels As New ArrayList
    Private pIntensities As New ArrayList
    Private pErrLog As String = "C:\mono_luChange\output\graphWQlog1.txt"


    Private Sub Initialize()
        pOutputLocations.Clear()

        pModels.Add("cccm")
        pModels.Add("ccsr")
        pModels.Add("csir")
        pModels.Add("echm")
        pModels.Add("gfdl")
        pModels.Add("hadc")
        pModels.Add("ncar")

        pIntensities.Clear()
        pIntensities.Add("_F10")
        pIntensities.Add("_F30")
        pIntensities.Add("_M")

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768
        'Add scenario directories
        pBaseFolders.Clear()
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2bmp")

        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2bmp")

        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_10")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono10bmp")

        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_70")

        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmp")
    End Sub
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)

        '************  Graphing for WQ duration curve plots ******************
        ' The WQ results for SED, N, and P are in the *.output.wdm files
        ' Location: RCHRES 9
        ' Flow Volume: 911, OVOL, -> ac.ft / hour
        ' Sed Load: 921 - 923, Sand + Silt + Clay,  -> tons/hour
        ' Total N: 931-936, N -> lb/hour
        ' Total P: 941-945, P -> lb/hour
        ' The above information is obtained from the .uci file, which is the same across scenarios
        '
        Dim lGraphSaveFormat As String = pGraphSaveFormat
        Dim lWDMNameCollection As New atcCollection
        Dim lOutputWdmDataSource As New atcDataSourceWDM()
        Dim lScenName As String = ""
        'build collection of scenarios (uci base names) to report
        Dim lOutputWDMNames As New System.Collections.Specialized.NameValueCollection

        Dim lp As String = ""

        Dim lDatagroupLoadSed As New atcTimeseriesGroup
        Dim lDatagroupLoadNitro As New atcTimeseriesGroup
        Dim lDatagroupLoadPhos As New atcTimeseriesGroup

        Dim lDatagroupConcSed As New atcTimeseriesGroup
        Dim lDatagroupConcNitro As New atcTimeseriesGroup
        Dim lDatagroupConcPhos As New atcTimeseriesGroup

        Dim lemissionScen As String = ""
        Dim lCurrentWDMSet As New atcCollection
        For Each lBaseFolder As String In pBaseFolders ' loop through all land use scenario folders
            lp = ""

            'Decide on emission scenario
            If lBaseFolder.EndsWith("a2") Then
                lemissionScen = "a_"
            ElseIf lBaseFolder.EndsWith("b2") Then
                lemissionScen = "b_"
            ElseIf lBaseFolder.StartsWith("Mono") Then
                lemissionScen = ""
            End If

            For Each lIntensity As String In pIntensities

                'Construct file name filter according to landuse scenario's emission storylines
                AddFilesInDir(lOutputWDMNames, lBaseFolder, False, "*.output.wdm")

                Dim lbaseScen As String = ""
                If lemissionScen.StartsWith("a_") OrElse lemissionScen.StartsWith("b_") Then
                    Try
                        For Each lkey As String In lOutputWDMNames.Keys
                            If IO.Path.GetFileNameWithoutExtension(lkey).StartsWith(lemissionScen) OrElse IO.Path.GetFileNameWithoutExtension(lkey).StartsWith("base") Then
                                If lkey.Contains(lIntensity.ToLower()) OrElse IO.Path.GetFileNameWithoutExtension(lkey).StartsWith("base") Then
                                    lCurrentWDMSet.Add(lkey, lkey)
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        Stop
                    End Try
                Else
                    For Each lkey As String In lOutputWDMNames.Keys
                        If lkey.Contains(lIntensity.ToLower()) OrElse IO.Path.GetFileNameWithoutExtension(lkey).StartsWith("base") Then
                            lCurrentWDMSet.Add(lkey, lkey)
                        End If
                    Next
                End If

                Dim lFlow As atcTimeseries = Nothing
                Dim lLoadSum As atcTimeseries = Nothing
                Dim lLoadSumNewUnit As atcTimeseries = Nothing
                Dim lConc As atcTimeseries = Nothing
                Dim lConcNewUnit As atcTimeseries = Nothing

                Dim lscenid As String = ""
                For Each lOutputWDMName As String In lCurrentWDMSet
                    lp = ""

                    Dim lthisWDMName As String = IO.Path.GetFileNameWithoutExtension(lOutputWDMName)
                    If lthisWDMName.StartsWith("base") Then
                        lscenid = "base"
                    Else
                        lscenid = lthisWDMName.Split("_")(2) ' the model name e.g. cccm in b_70_cccm_F10.base.output.wdm
                    End If
                    lOutputWdmDataSource.Open(lOutputWDMName)

                    'Flow rate (911, ac.ft/hour -> liter/s)
                    lFlow = _
                      lOutputWdmDataSource.DataSets.ItemByKey(911) * 342.633844 '1 ((acre foot) per hour) = 342.633844 liter per second

                    '****** SEDIMENT *****
                    '****** SEDIMENT Begins *****
                    '****** SEDIMENT
                    ' Sediment loading (921, sand; 922, silt; 923, clay in tons per hour)
                    lLoadSum = _
                       lOutputWdmDataSource.DataSets.ItemByKey(921) + _
                       lOutputWdmDataSource.DataSets.ItemByKey(922) + _
                       lOutputWdmDataSource.DataSets.ItemByKey(923)
                    lLoadSumNewUnit = lLoadSum * 0.251995761 '1 (ton per hour) = 0.251995761 kilogram per second

                    ' Sediment concentration
                    lConc = lLoadSumNewUnit / lFlow ' Intermediate - kg/l
                    lConcNewUnit = lConc * (1000 * 1000) 'kg/l to mg/L

                    'Rid of Infinitys from TSers
                    For i As Integer = 0 To lFlow.numValues - 1
                        If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                        If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                        If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                    Next

                    lLoadSumNewUnit.Attributes.SetValue("Constituent", "LDC Sediment " & lscenid)
                    lConcNewUnit.Attributes.SetValue("Constituent", "CDC Sediment " & lscenid)
                    lDatagroupLoadSed.Add(lLoadSumNewUnit) ' kg/s
                    lDatagroupConcSed.Add(lConcNewUnit) ' mg/L/s

                    '******* NITROGEN *******
                    '******* NITROGEN Begins *******
                    '******* NITROGEN *******
                    ' Nitrogen loading
                    'Get Tot-N
                    ' Need 931 - NUCF9, NO3D
                    ' Need 932 - NUCF9, NH3D
                    ' Need 933 - OSNH4, NH3A
                    ' Need 934 - OSNH4, NH3I
                    ' Need 935 - OSNH4, NH3C
                    ' Need 936 - PKCF2, RORN
                    ' Need OXCF2 - 951 - BODA; multiplier: 0.043580459 (i.e. (14*16*49)/(1200*106*1.98))
                    ' Need PKCF2 - 953 - PHYT; multiplier: 0.086289308 (i.e. (14*16*49)/(1200*106))

                    Try
                        lLoadSum = lOutputWdmDataSource.DataSets.ItemByKey(931) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(932) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(933) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(933) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(934) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(935) + _
                        lOutputWdmDataSource.DataSets.ItemByKey(936) + _
                        (lOutputWdmDataSource.DataSets.ItemByKey(951) * 0.043580459) + _
                        (lOutputWdmDataSource.DataSets.ItemByKey(953) * 0.086289308)
                    Catch ex As Exception
                        Logger.Msg("Adding Nitrogen load problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    Try
                        lLoadSumNewUnit = lLoadSum * 0.000125997881 ' coversion: lb/h -> kg/s,  1 (pound per hour) = 0.000125997881 kilogram per second
                    Catch ex As Exception
                        Logger.Msg("Change Nitrogen load unit problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    Try
                        lConc = lLoadSum / lFlow ' here unit is: number of kg / L /s
                    Catch ex As Exception
                        Logger.Msg("Calc Nitrogen concentration problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    'Change Nitrogen concentration to mg/L by multiplying 1000 * 1000
                    Try
                        lConcNewUnit = lConc * (1000 * 1000)
                    Catch ex As Exception
                        Logger.Msg("Calc final nitrogen concentration problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    'Rid of Infinitys from TSers
                    For i As Integer = 0 To lFlow.numValues - 1
                        If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                        If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                        If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                    Next

                    lLoadSum.Attributes.SetValue("Constituent", "LDC Nitrogen " & lscenid)
                    lConcNewUnit.Attributes.SetValue("Constituent", "CDC Nitrogen " & lscenid)
                    lDatagroupLoadNitro.Add(lLoadSum) 'lb /h
                    lDatagroupConcNitro.Add(lConcNewUnit) ' mg/L/s

                    '******* PO4 *******
                    '******* PO4 Begins *******
                    '******* PO4 *******
                    ' Phosphorus loading
                    'Get Tot-P
                    ' Need 941 - 'NUCF9, PO4D
                    ' Need 942 - 'OSPO4, PO4A
                    ' Need 943 - 'OSPO4, PO4I
                    ' Need 944 - 'OSPO4, PO4C
                    ' Need 945 - 'PKCF2, RORP
                    ' Need OXCF2 - 951 - BODA; multiplier: 0.006031224 (i.e. (31*49)/(1200*106)/1.98)
                    ' Need PKCF2 - 953 - PHYT; multiplier: 0.011941824 (i.e. (31*49)/(1200*106))
                    Dim lPO4D As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(941)
                    Dim lPO4A As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(942)
                    Dim lPO4I As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(943)
                    Dim lPO4C As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(944)
                    Dim lRORP As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(945)
                    Dim lBODA As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(951) * 0.006031224
                    Dim lPHYT As atcTimeseries = lOutputWdmDataSource.DataSets.ItemByKey(953) * 0.011941824
                    Try
                        lLoadSum = lPO4D
                        lLoadSum = lLoadSum + lPO4A
                        lLoadSum = lLoadSum + lPO4I
                        lLoadSum = lLoadSum + lPO4C
                        lLoadSum = lLoadSum + lRORP
                        lLoadSum = lLoadSum + lBODA
                        lLoadSum = lLoadSum + lPHYT
                    Catch ex As Exception
                        Logger.Msg("Adding Phosphorus problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    Try
                        lLoadSumNewUnit = lLoadSum * 0.000125997881 ' coversion: lb/h -> kg/s,  1 (pound per hour) = 0.000125997881 kilogram per second
                    Catch ex As Exception
                        Logger.Msg("Changen unit for Phosphorus load problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    Try
                        lConc = lLoadSumNewUnit / lFlow ' here unit is: number of kg / L
                    Catch ex As Exception
                        Logger.Msg("Calc Phosphorus concentration problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    Try
                        lConcNewUnit = lConc * (1000 * 1000) ' convert unit to mg /L
                    Catch ex As Exception
                        Logger.Msg("Change Phosphorus concentration unit problem for: " & lOutputWDMName)
                        Exit Sub
                    End Try

                    'Rid of Infinitys from TSers
                    For i As Integer = 0 To lFlow.numValues - 1
                        If Double.IsInfinity(lFlow.Value(i)) Then lFlow.Value(i) = Double.NaN
                        If Double.IsInfinity(lLoadSumNewUnit.Value(i)) Then lLoadSumNewUnit.Value(i) = Double.NaN
                        If Double.IsInfinity(lConcNewUnit.Value(i)) Then lConcNewUnit.Value(i) = Double.NaN
                    Next

                    lLoadSum.Attributes.SetValue("Constituent", "LDC Phos " & lscenid)
                    lConcNewUnit.Attributes.SetValue("Constituent", "CDC Phos " & lscenid)
                    lDatagroupLoadPhos.Add(lLoadSum) ' lb/h
                    lDatagroupConcPhos.Add(lConcNewUnit) ' mg/L/s

                Next ' lOWDM in lOutputWDMS within a given scen lfld

                'Do the duration graphs for sediment load
                Dim lprefix As String = "zLDur_"
                Dim lscen As String = lprefix & "Sed" & lIntensity
                Dim lGraphFilename As String = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                Dim lPlotTitle As String = "Normal Percentile (% greater than): Sediment Load : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & " : " & lIntensity
                Dim lYTitle As String = "Sediment (LDC) kg/sec"
                doWQDurPlot(lDatagroupLoadSed, lGraphFilename, lPlotTitle, lYTitle)

                'Do the duration graph for sediment concentration
                lscen = lprefix & "SedConc" & lIntensity
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                lPlotTitle = "Normal Percentile (% greater than): Sediment Concentration : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & lIntensity
                lYTitle = "Sediment Concentration (CDC) mg/L/sec"
                doWQDurPlot(lDatagroupConcSed, lGraphFilename, lPlotTitle, lYTitle)

                'Do the duration graph for nitrogen load
                lscen = lprefix & "Nitro" & lIntensity
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                lPlotTitle = "Normal Percentile (% greater than): Nitrogen Load : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & lIntensity
                lYTitle = "Nitrogen Load (LDC) lb/h"
                doWQDurPlot(lDatagroupLoadNitro, lGraphFilename, lPlotTitle, lYTitle)

                'Do the duration graph for nitrogen Concentration
                lscen = lprefix & "NitroConc" & lIntensity
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                lPlotTitle = "Normal Percentile (% greater than): Nitrogen Concentration : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & lIntensity
                lYTitle = "Nitrogen Concentration (CDC) mg/L/sec"
                doWQDurPlot(lDatagroupConcNitro, lGraphFilename, lPlotTitle, lYTitle)

                'Do the duration graph for phosphorus load
                lscen = lprefix & "Phos" & lIntensity
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                lPlotTitle = "Normal Percentile (% greater than): Phosphorus Load : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & lIntensity
                lYTitle = "Phosphorus Load (LDC) lb/h"
                doWQDurPlot(lDatagroupLoadPhos, lGraphFilename, lPlotTitle, lYTitle)

                'Do the duration graph for phosphorus Concentration
                lscen = lprefix & "PhosConc" & lIntensity
                lGraphFilename = IO.Path.Combine(lBaseFolder, lscen) & lGraphSaveFormat
                lPlotTitle = "Normal Percentile (% greater than): Phosphorus Concentration : " & lBaseFolder.Substring(lBaseFolder.LastIndexOf("\") + 1) & lIntensity
                lYTitle = "Phosphorus Concentration (CDC) mg/L/sec"
                doWQDurPlot(lDatagroupConcPhos, lGraphFilename, lPlotTitle, lYTitle)

                If lFlow IsNot Nothing Then lFlow.Clear()
                If lLoadSum IsNot Nothing Then lLoadSum.Clear()
                If lLoadSumNewUnit IsNot Nothing Then lLoadSumNewUnit.Clear()
                If lConc IsNot Nothing Then lConc.Clear()
                If lConcNewUnit IsNot Nothing Then lConcNewUnit.Clear()

                lDatagroupLoadSed.Clear()
                lDatagroupLoadNitro.Clear()
                lDatagroupLoadPhos.Clear()

                lDatagroupConcSed.Clear()
                lDatagroupConcNitro.Clear()
                lDatagroupConcPhos.Clear()
                lOutputWDMNames.Clear()
                lCurrentWDMSet.Clear()

            Next ' lIntensity in pIntensities
        Next ' lfld in pBaseFolders
        '
    End Sub

    Private Function doWQDurPlot(ByRef aDatagroup As atcDataGroup, ByVal aGraphFilename As String, ByVal aPlotTitle As String, ByVal aYTitle As String) As Boolean
        Dim doneIt As Boolean = True
        Dim lp As String = ""

        Dim lZgc As ZedGraphControl
        lZgc = CreateZgc()
        Dim lGraphSaveWidth As Integer = 1000
        Dim lGraphSaveHeight As Integer = 600
        lZgc.Width = lGraphSaveWidth
        lZgc.Height = lGraphSaveHeight

        Dim lGraphDur As New clsGraphProbability(aDatagroup, lZgc)

        With lGraphDur.ZedGraphCtrl.GraphPane
            .XAxis.Title.Text = aPlotTitle
            With .XAxis
                .Scale.MaxAuto = False
                .Scale.MinAuto = False
                '.Scale.Min = 0.0001
                '.Scale.Max = 0.5
            End With
            .YAxis.Title.Text = aYTitle
            .YAxis.Type = AxisType.Linear
            If .YAxis.Scale.Min < 1 Then
                '.YAxis.Scale.MinAuto = True
                '.YAxis.Scale.MaxAuto = True
                '.YAxis.Scale.Min = 0.0000001
                '.YAxis.Scale.Min = 1
                '.YAxis.Scale.Max = 1000000
                .YAxis.MajorGrid.IsVisible = False
                .YAxis.MinorGrid.IsVisible = False
                .AxisChange()
            End If
            .CurveList.Item(0).Color = Drawing.Color.Firebrick
            .CurveList.Item(1).Color = Drawing.Color.Aqua
            .CurveList.Item(2).Color = Drawing.ColorTranslator.FromHtml("#ff3366")
            .CurveList.Item(3).Color = Drawing.ColorTranslator.FromHtml("#cc9933")
            .CurveList.Item(4).Color = Drawing.ColorTranslator.FromHtml("#99FF22")
            .CurveList.Item(5).Color = Drawing.Color.Black
            .CurveList.Item(6).Color = Drawing.Color.DarkGreen
            .CurveList.Item(7).Color = Drawing.Color.Moccasin
            If .CurveList.Count > 8 Then
                .CurveList.Item(8).Color = Drawing.ColorTranslator.FromHtml("#C8D981")
                .CurveList.Item(9).Color = Drawing.ColorTranslator.FromHtml("#9999FF")
                .CurveList.Item(10).Color = Drawing.ColorTranslator.FromHtml("#0099CC")
                .CurveList.Item(11).Color = Drawing.ColorTranslator.FromHtml("#FF00FF")
                .CurveList.Item(12).Color = Drawing.ColorTranslator.FromHtml("#9900FF")
                .CurveList.Item(13).Color = Drawing.ColorTranslator.FromHtml("#00CC99")
                .CurveList.Item(14).Color = Drawing.ColorTranslator.FromHtml("#FFFF00")
            End If
        End With

        Try
            lZgc.SaveIn(aGraphFilename)
        Catch ex As Exception
            lp = aGraphFilename
            doneIt = False
            'Stop
        End Try
        lGraphDur.Dispose()
        lZgc.Dispose()

        If lp <> "" Then
            IO.File.AppendAllText(pErrLog, lp & vbCrLf)
        End If

        Return doneIt
    End Function
End Module
