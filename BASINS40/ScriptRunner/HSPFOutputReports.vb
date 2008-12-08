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

Module HSPFOutputReports
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

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"
        pGraphSaveWidth = 1024
        pGraphSaveHeight = 768

        'Dim lTestName As String = "tinley"
        'Dim lTestName As String = "hspf"
        'Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "shena"
        'Dim lTestName As String = "mono_lu2030a2_base"
        Dim lTestName As String = "upatoi"
        'Dim lTestName As String = "housatonic"
        'Dim lTestName As String = "beaver"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        'Dim lTestName As String = "SantaClara"


        'Add scenario directories
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2bmp")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2bmp")

        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2bmp")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2bmp")

        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_10")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono10bmp")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_70")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmp")

        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmpDbg")

        pSummaryTypes.Add("Water")
        'pSummaryTypes.Add("Sediment")

        Select Case lTestName
            Case "mono"
                pTestPath = "d:\mono_base"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
            Case "mono_lu2030a2_base"
                pTestPath = "D:\mono_luChange\output\lu2030a2"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
            Case "mono_lu2030a2_a_10_cccm_F10"
                pTestPath = "D:\mono_luChange\output\lu2030a2"
                pBaseName = "a_10_cccm_F10.base"
                pOutputLocations.Add("R:9")
            Case "housatonic"
                pTestPath = "d:\projects\housatonic\jackBin"
                pBaseName = "base"
                pOutputLocations.Add("R:9")
                'pOutputLocations.Add("R:110")
                'pOutputLocations.Add("R:400")
                'pOutputLocations.Add("R:820")
                'pOutputLocations.Add("R:540")
                'pOutputLocations.Add("R:600")
                'pOutputLocations.Add("R:900")
            Case "beaver"
                pTestPath = "g:\projects\beaver\tds"
                pBaseName = "beaver-TDS-Run01"
                pOutputLocations.Add("R:360")
                pCurveStepType = "NonStep"
                If Not pSummaryTypes.Contains("Sediment") Then
                    pSummaryTypes.Add("Sediment")
                End If
            Case "shena"
                pTestPath = "c:\test\genscn"
                pBaseName = "base"
                pOutputLocations.Add("Lynnwood")
            Case "upatoi"
                pTestPath = "D:\Basins\modelout\Upatoi"
                'pTestPath = "C:\Basins\data\20710-01\Upatoi"
                pBaseName = "Upatoi"
                pOutputLocations.Add("R:14")
                pOutputLocations.Add("R:34")
                pOutputLocations.Add("R:46")
                pOutputLocations.Add("R:62")
                pOutputLocations.Add("R:74")
                pGraphAnnual = True
                pCurveStepType = "NonStep" 'Tony's convention 
                Dim pUpatoiPerlndSegmentStarts() As Integer = {101, 201, 301, 401, 501, 601, 701, 801, 901, 951} '{101, 201, 301, 401, 501, 601, 701, 801, 901, 924}
                pPerlndSegmentStarts = pUpatoiPerlndSegmentStarts
                Dim pUpatoiImplndSegmentStarts() As Integer = {102, 202, 302, 402, 502, 602, 702, 802, 902, 952} '{101, 201, 301, 401, 501, 601, 701, 801, 901, 905}
                pImplndSegmentStarts = pUpatoiImplndSegmentStarts
            Case "tinley"
                pTestPath = "c:\test\tinley"
                pBaseName = "tinley"
                pOutputLocations.Add("R:850")
            Case "calleguas_cat"
                pTestPath = "D:\MountainViewData\Calleguas\cat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "calleguas_nocat"
                pTestPath = "D:\MountainViewData\Calleguas\nocat"
                pBaseName = "Calleg"
                pOutputLocations.Add("R:408")
                pOutputLocations.Add("R:10")
                pOutputLocations.Add("R:307")
            Case "hyd_man"
                pTestPath = "C:\test\EXP_CAL\hyd_man.net"
                pBaseName = "hyd_man"
                pOutputLocations.Add("R:5")
                pOutputLocations.Add("R:4")
            Case "hspf"
                pTestPath = "C:\test\HSPF"
                pBaseName = "test10"
            Case "SantaClara"
                pTestPath = "D:\MountainViewData\SantaClara\nocat"
                pBaseName = "SCR10"
                pOutputLocations.Add("R:70")
                pOutputLocations.Add("R:180")
                pOutputLocations.Add("R:320")
                pOutputLocations.Add("R:410")
                pOutputLocations.Add("R:880")
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        If FileExists(pBaseName & "Orig.uci") Then
            IO.File.Copy(pBaseName & "Orig.uci", pBaseName & ".uci")
        End If

        '************  Graphing for WQ duration curve plots ******************
        ' The WQ results for SED, N, and P are in the *.output.wdm files
        ' Location: RCHRES 9
        ' Flow Volume: 911, OVOL, -> ac.ft / hour
        ' Sed Load: 921 - 923, Sand + Silt + Clay,  -> tons/hour
        ' Total N: 931-936, N -> lb/hour
        ' Total P: 941-945, P -> lb/hour
        ' The above information is obtained from the .uci file, which is the same across scenarios
        '

        Dim lGraphSaveFormat As String = ".png"
        Dim lWDMNameCollection As New atcCollection
        Dim lOutputWdmDataSource As New atcDataSourceWDM()
        Dim lScenName As String = ""
        For Each lfld As String In pBaseFolders ' loop through all land use scenario folders

            Dim lDataGroup4Plot As New atcDataGroup
            'build collection of scenarios (uci base names) to report
            Dim lOutputWDMs As New System.Collections.Specialized.NameValueCollection
            'do we want the 'base.uci'?
            AddFilesInDir(lOutputWDMs, lfld, False, "*.output.wdm")
            For Each lOWDM As String In lOutputWDMs
                lOutputWdmDataSource.Open(lOWDM)

                'Flow rate (911, ac.ft/hour -> liter/s)
                Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
                Dim lMathArgs As New atcDataAttributes
                lMathArgs.Add("Timeseries", lOutputWdmDataSource.DataSets.ItemByKey(911))
                lMathArgs.Add("Number", 342.633844) '1 ((acre foot) per hour) = 342.633844 liter per second

                Dim lFlow As atcTimeseries = Nothing
                If lMath.Open("Multiply", lMathArgs) Then
                    lFlow = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Flow extraction problem")
                    Exit Sub
                End If


                '****** SEDIMENT *****

                ' Sediment loading
                Dim lDataGroup As New atcDataGroup
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(921)) 'Sand
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(922)) 'Silt
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(923)) 'Clay

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                Dim lLoadSum As atcTimeseries ' Intermediate 
                If lMath.Open("Add", lMathArgs) Then
                    lLoadSum = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Adding sediment for " & lOWDM & " problem.")
                    Exit Sub
                End If

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lLoadSum)
                lMathArgs.SetValue("Number", 0.251995761 / 1000.0) ' coversion: ton/h -> kg/s * 1000
                Dim lLoadSumNewUnit As atcTimeseries = Nothing
                If lMath.Open("Multiply", lMathArgs) Then
                    lLoadSumNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Change sediment unit problem.")
                    Exit Sub
                End If

                ' Sediment concentration
                lDataGroup.Clear()
                lDataGroup.Add("sedload", lLoadSumNewUnit)
                lDataGroup.Add("flow", lFlow)
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                Dim lConc As atcTimeseries  ' Intermediate 
                If lMath.Open("Divide", lMathArgs) Then
                    lConc = lMath.DataSets(0) ' here unit is: number of 1000 kg / L
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc sediment concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If

                'Change sediment concentration to mg/L by multiplying 1000 * 1000 * 1000
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lConc)
                lMathArgs.SetValue("Number", 1000 * 1000 * 1000)
                Dim lConcNewUnit As atcTimeseries
                If lMath.Open("Multiply", lMathArgs) Then
                    lConcNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc final sediment concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If

                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSumNewUnit.Attributes.SetValue("Constituent", "LDC (Sediment)")
                lDataGroup.Add(lLoadSumNewUnit)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Sediment)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph
                Dim lscen As String = IO.Path.GetFileNameWithoutExtension(lOWDM)
                Dim lGraphFilename As String = IO.Path.Combine(lfld, lscen) & "_dur_sed" & lGraphSaveFormat
                Dim lZgc As ZedGraphControl

                lZgc = CreateZgc()
                Dim lGraphSaveWidth As Integer = 1000
                Dim lGraphSaveHeight As Integer = 600
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane

                    .XAxis.Title.Text = "Normal Percentile (% greater than): Sediment : " & lfld.Substring(lfld.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec (x1000)"
                        .IsVisible = True
                    End With
                    If .YAxis.Scale.Min < 1 Then
                        .YAxis.Scale.MinAuto = False
                        .YAxis.Scale.Min = 1
                        .AxisChange()
                        .YAxis.MajorGrid.IsVisible = False
                        .YAxis.MinorGrid.IsVisible = False
                        .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                    End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Sediment)").Color = Drawing.Color.Brown
                    .CurveList("CDC (Sediment)").Color = Drawing.Color.BurlyWood
                    .CurveList("LDC (Sediment)").IsY2Axis = True
                    .CurveList("CDC (Sediment)").IsY2Axis = True

                End With

                lZgc.SaveIn(lGraphFilename)
                lGraphDur.Dispose()
                lZgc.Dispose()

                '******* NITROGEN *******
                '******* NITROGEN Begins *******
                '******* NITROGEN *******
                ' Nitrogen loading
                lDataGroup.Clear()
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(931)) 'NUCF9, NO3D
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(932)) 'NUCF9, NH3D
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(933)) 'OSNH4, NH3A
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(934)) 'OSNH4, NH3I
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(935)) 'OSNH4, NH3C
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(936)) 'PKCF2, RORN

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                lLoadSum.Clear()
                If lMath.Open("Add", lMathArgs) Then
                    lLoadSum = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Adding Nitrogen for " & lOWDM & " problem.")
                    Exit Sub
                End If

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lLoadSum)
                lMathArgs.SetValue("Number", 0.000125997881 / 1000.0) ' coversion: lb/h -> kg/s * 1000,  1 (pound per hour) = 0.000125997881 kilogram per second
                lLoadSumNewUnit.Clear()
                If lMath.Open("Multiply", lMathArgs) Then
                    lLoadSumNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Change Nitrogen unit problem.")
                    Exit Sub
                End If

                ' Get Nitrogen concentration
                lDataGroup.Clear()
                lDataGroup.Add("nitroload", lLoadSumNewUnit)
                lDataGroup.Add("flow", lFlow)
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                lConc.Clear()
                If lMath.Open("Divide", lMathArgs) Then
                    lConc = lMath.DataSets(0) ' here unit is: number of 1000 kg / L
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc Nitrogen concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If

                'Change Nitrogen concentration to mg/L by multiplying 1000 * 1000 * 1000
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lConc)
                lMathArgs.SetValue("Number", 1000 * 1000 * 1000)
                lConcNewUnit.Clear()
                If lMath.Open("Multiply", lMathArgs) Then
                    lConcNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc final nitrogen concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If


                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSumNewUnit.Attributes.SetValue("Constituent", "LDC (Nitrogen)")
                lDataGroup.Add(lLoadSumNewUnit)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Nitrogen)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph for nitrogen
                'lscen = IO.Path.GetFileNameWithoutExtension(lOWDM) ' use from above
                lGraphFilename = IO.Path.Combine(lfld, lscen) & "_dur_nitro" & lGraphSaveFormat

                lZgc = CreateZgc()
                'Dim lGraphSaveWidth As Integer = 800  ' use from above
                'Dim lGraphSaveHeight As Integer = 600 ' use from above
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                lGraphDur = New clsGraphProbability(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane

                    .XAxis.Title.Text = "Normal Percentile (% greater than): Nitrogen : " & lfld.Substring(lfld.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec (x1000)"
                        .IsVisible = True
                    End With
                    If .YAxis.Scale.Min < 1 Then
                        .YAxis.Scale.MinAuto = False
                        .YAxis.Scale.Min = 1
                        .AxisChange()
                        .YAxis.MajorGrid.IsVisible = False
                        .YAxis.MinorGrid.IsVisible = False
                        .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                    End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Nitrogen)").Color = Drawing.Color.DarkGoldenrod
                    .CurveList("CDC (Nitrogen)").Color = Drawing.Color.DarkKhaki
                    .CurveList("LDC (Nitrogen)").IsY2Axis = True
                    .CurveList("CDC (Nitrogen)").IsY2Axis = True

                End With

                lZgc.SaveIn(lGraphFilename)
                lGraphDur.Dispose()
                lZgc.Dispose()


                '******* PO4 *******
                '******* PO4 Begins *******
                '******* PO4 *******
                ' Phosphorus loading
                lDataGroup.Clear()
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(941)) 'NUCF9, PO4D
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(942)) 'OSPO4, PO4A
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(943)) 'OSPO4, PO4I
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(944)) 'OSPO4, PO4C
                lDataGroup.Add(lOutputWdmDataSource.DataSets.ItemByKey(945)) 'PKCF2, RORP

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                lLoadSum.Clear()
                If lMath.Open("Add", lMathArgs) Then
                    lLoadSum = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Adding Phosphorus for " & lOWDM & " problem.")
                    Exit Sub
                End If

                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lLoadSum)
                lMathArgs.SetValue("Number", 0.000125997881 / 1000.0) ' coversion: lb/h -> kg/s * 1000,  1 (pound per hour) = 0.000125997881 kilogram per second
                lLoadSumNewUnit.Clear()
                If lMath.Open("Multiply", lMathArgs) Then
                    lLoadSumNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Change Nitrogen unit problem.")
                    Exit Sub
                End If

                ' Get Nitrogen concentration
                lDataGroup.Clear()
                lDataGroup.Add("po4load", lLoadSumNewUnit)
                lDataGroup.Add("flow", lFlow)
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lDataGroup)

                lConc.Clear()
                If lMath.Open("Divide", lMathArgs) Then
                    lConc = lMath.DataSets(0) ' here unit is: number of 1000 kg / L
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc Phosphorus concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If

                'Change Phosphorus concentration to mg/L by multiplying 1000 * 1000 * 1000
                lMath.Clear()
                lMathArgs.Clear()
                lMathArgs.SetValue("Timeseries", lConc)
                lMathArgs.SetValue("Number", 1000 * 1000 * 1000)
                lConcNewUnit.Clear()
                If lMath.Open("Multiply", lMathArgs) Then
                    lConcNewUnit = lMath.DataSets(0)
                    lMath.DataSets.RemoveAt(0)
                Else
                    Logger.Msg("Calc final phosphorus concentration for " & lOWDM & " problem.")
                    Exit Sub
                End If


                lDataGroup.Clear()
                lFlow.Attributes.SetValue("Constituent", "FDC")
                lDataGroup.Add(lFlow)
                lLoadSumNewUnit.Attributes.SetValue("Constituent", "LDC (Phosphorus)")
                lDataGroup.Add(lLoadSumNewUnit)
                lConcNewUnit.Attributes.SetValue("Constituent", "CDC (Phosphorus)")
                lDataGroup.Add(lConcNewUnit)

                'Do the duration graph for nitrogen
                'lscen = IO.Path.GetFileNameWithoutExtension(lOWDM) ' use from above
                lGraphFilename = IO.Path.Combine(lfld, lscen) & "_dur_phos" & lGraphSaveFormat

                lZgc = CreateZgc()
                'Dim lGraphSaveWidth As Integer = 800  ' use from above
                'Dim lGraphSaveHeight As Integer = 600 ' use from above
                lZgc.Width = lGraphSaveWidth
                lZgc.Height = lGraphSaveHeight
                lGraphDur = New clsGraphProbability(lDataGroup, lZgc)

                With lGraphDur.ZedGraphCtrl.GraphPane

                    .XAxis.Title.Text = "Normal Percentile (% greater than): Phosphorus : " & lfld.Substring(lfld.LastIndexOf("\") + 1) & " : " & lscen.Substring(0, lscen.Length - lscen.LastIndexOf(".base.output") + 1)
                    With .XAxis
                        .Scale.Min = 0.0000001
                    End With
                    With .Y2Axis
                        .Type = AxisType.Log
                        .Scale.IsUseTenPower = False
                        .MajorGrid.IsVisible = False
                        .MinorGrid.IsVisible = False
                        .Title.Text = "Concentration (CDC) mg/L and" & vbCrLf & "Load Rate (LDC) kg/sec (x1000)"
                        .IsVisible = True
                    End With
                    If .YAxis.Scale.Min < 1 Then
                        .YAxis.Scale.MinAuto = False
                        .YAxis.Scale.Min = 1
                        .AxisChange()
                        .YAxis.MajorGrid.IsVisible = False
                        .YAxis.MinorGrid.IsVisible = False
                        .YAxis.Title.Text = "Flow Rate (FDC) L/sec"
                    End If
                    .CurveList("FDC").Color = Drawing.Color.BlueViolet
                    .CurveList("LDC (Phosphorus)").Color = Drawing.Color.Crimson
                    .CurveList("CDC (Phosphorus)").Color = Drawing.Color.DarkSalmon
                    .CurveList("LDC (Phosphorus)").IsY2Axis = True
                    .CurveList("CDC (Phosphorus)").IsY2Axis = True

                End With

                lZgc.SaveIn(lGraphFilename)
                lGraphDur.Dispose()
                lZgc.Dispose()

            Next ' lOWDM in lOutputWDMS within a given scen lfld
            Exit Sub
        Next ' lfld in pBaseFolders

        '
        '************  End graphing for WQ Duration curve plots **************
        If pGraphWQOnly Then Exit Sub ' early end here if just doing it for WQ graphs

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        If pOutputLocations.Contains("Lynnwood") Then 'special case to check GenScn examples
            With lHspfUci.GlobalBlock
                .SDate(0) = 1986
                .SDate(1) = 10
                .SDate(2) = 1
                .EDate(0) = 1987
                .EDate(1) = 10
                .EDate(2) = 1
            End With
        End If
        'lHspfUci.Save()

        'open HBN file
        'TODO: need to allow additional binary output files!
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lRunMade As String = lHspfBinFileInfo.LastWriteTime.ToString

        If pSummaryTypes.Contains("Water") Then
            'open WDM file
            Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)

            Dim lExpertSystemFileNames As New NameValueCollection
            AddFilesInDir(lExpertSystemFileNames, IO.Directory.GetCurrentDirectory, False, "*.exs")
            Dim lExpertSystem As HspfSupport.atcExpertSystem
            For Each lExpertSystemFileName As String In lExpertSystemFileNames
                Try
                    Dim lFileCopied As Boolean = False
                    If IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                        lFileCopied = TryCopy(lExpertSystemFileName, pBaseName & ".exs")
                    End If
                    lExpertSystem = New HspfSupport.atcExpertSystem(lHspfUci, lWdmDataSource)
                    Dim lStr As String = lExpertSystem.Report
                    SaveFileString("outfiles\ExpertSysStats-" & IO.Path.GetFileNameWithoutExtension(lExpertSystemFileName) & ".txt", lStr)

                    'lStr = lExpertSystem.AsString 'NOTE:just testing
                    'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                    Dim lCons As String = "Flow"
                    For Each lSite As hexSite In lExpertSystem.Sites
                        Dim lSiteName As String = lSite.Name
                        Dim lArea As Double = lSite.Area
                        Dim lSimTSerInches As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(0))
                        lSimTSerInches.Attributes.SetValue("Units", "Flow (inches)")
                        Dim lSimTSer As atcTimeseries = InchesToCfs(lSimTSerInches, lArea)
                        lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lSimTSer.Attributes.SetValue("YAxis", "Left")
                        lSimTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSer As atcTimeseries = SubsetByDate(lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(1)), lExpertSystem.SDateJ, lExpertSystem.EDateJ, Nothing)
                        lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                        lObsTSer.Attributes.SetValue("YAxis", "Left")
                        lObsTSer.Attributes.SetValue("StepType", pCurveStepType)
                        Dim lObsTSerInches As atcTimeseries = CfsToInches(lObsTSer, lArea)
                        lObsTSerInches.Attributes.SetValue("Units", "Flow (inches)")

                        Dim lRchId As Integer
                        If lSite.Name.StartsWith("RCH") Then
                            lRchId = lSiteName.Substring(3)
                        Else
                            lRchId = lSite.Name
                        End If
                        Dim lOperation As atcUCI.HspfOperation = lHspfUci.OpnBlks("RCHRES").OperFromID(lRchId)

                        Dim lPrecSourceCollection As New atcCollection
                        Dim lAreaFromWeight As Double = lHspfUci.WeightedSourceArea(lOperation, "PREC", lPrecSourceCollection)
                        Dim lPrecTser As atcTimeseries = Nothing
                        Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
                        Dim lMathArgs As New atcDataAttributes
                        For lSourceIndex As Integer = 0 To lPrecSourceCollection.Count - 1
                            Dim lPrecDataGroup As atcDataGroup = lWdmDataSource.DataSets.FindData("ID", lPrecSourceCollection.Keys(lSourceIndex))
                            If lPrecDataGroup.Count = 0 Then
                                Dim lPrecWdmDataSource As New atcDataSourceWDM()
                                lPrecWdmDataSource.Open(pTestPath & "\FBMet.wdm")
                                lPrecDataGroup = lPrecWdmDataSource.DataSets.FindData("ID", lPrecSourceCollection.Keys(lSourceIndex))
                            End If
                            lMathArgs.SetValue("Timeseries", lPrecDataGroup)
                            lMathArgs.SetValue("Number", lPrecSourceCollection.Item(lSourceIndex))
                            If lMath.Open("Multiply", lMathArgs) Then
                                If lSourceIndex = 0 Then
                                    lPrecTser = lMath.DataSets(0).Clone
                                Else
                                    Dim lMathAdd As New atcTimeseriesMath.atcTimeseriesMath
                                    Dim lMathAddArgs As New atcDataAttributes
                                    Dim lDataGroup As New atcDataGroup
                                    lDataGroup.Add(lPrecTser)
                                    lDataGroup.Add(lMath.DataSets(0))
                                    lMathAddArgs.SetValue("Timeseries", lDataGroup)
                                    If lMathAdd.Open("Add", lMathAddArgs) Then
                                        lPrecTser = lMathAdd.DataSets(0).Clone
                                    Else
                                        Logger.Dbg("Problem")
                                    End If
                                End If
                            Else
                                Logger.Dbg("Problem!")
                            End If
                            lMath.Clear()
                            lMathArgs.Clear()
                        Next
                        lMathArgs.SetValue("Timeseries", lPrecTser)
                        'lMathArgs.SetValue("Number", lSite.Area)
                        lMathArgs.SetValue("Number", lAreaFromWeight)
                        If Not lMath.Open("Divide", lMathArgs) Then
                            Logger.Dbg("Problem")
                        End If
                        lPrecTser = lMath.DataSets(0)
                        lPrecTser.Attributes.SetValue("Location", "Weighted Average")

                        'Dim lPrecTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(5))
                        lPrecTser.Attributes.SetValue("Units", "inches")

                        lStr = HspfSupport.MonthlyAverageCompareStats.Report(lHspfUci, _
                                                                             lCons, lSiteName, _
                                                                             "inches", _
                                                                             lSimTSerInches, lObsTSerInches, _
                                                                             lRunMade, _
                                                                             lExpertSystem.SDateJ, _
                                                                             lExpertSystem.EDateJ)
                        Dim lOutFileName As String = "outfiles\MonthlyAverage" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.AnnualCompareStats.Report(lHspfUci, _
                                                                     lCons, lSiteName, _
                                                                     "inches", _
                                                                     lPrecTser, lSimTSerInches, lObsTSerInches, _
                                                                     lRunMade, _
                                                                     lExpertSystem.SDateJ, _
                                                                     lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\Annual" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                           lCons, lSiteName, _
                                                                           lSimTSer, lObsTSer, _
                                                                           lRunMade, _
                                                                           lExpertSystem.SDateJ, _
                                                                           lExpertSystem.EDateJ)
                        lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats-" & lSiteName & ".txt"
                        SaveFileString(lOutFileName, lStr)

                        Dim lTimeSeries As New atcDataGroup
                        lTimeSeries.Add("Observed", lObsTSer)
                        lTimeSeries.Add("Simulated", lSimTSer)
                        lTimeSeries.Add("Precipitation", lPrecTser)
                        lTimeSeries.Add("LZS", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(9)))
                        lTimeSeries.Add("UZS", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(8)))
                        lTimeSeries.Add("PotET", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(6)))
                        lTimeSeries.Add("ActET", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(7)))
                        lTimeSeries.Add("Baseflow", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(4)))
                        lTimeSeries.Add("Interflow", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(3)))
                        lTimeSeries.Add("Surface", lWdmDataSource.DataSets.ItemByKey(lSite.Dsn(2)))
                        GraphAll(lExpertSystem.SDateJ, lExpertSystem.EDateJ, _
                                 lCons, lSiteName, _
                                 lTimeSeries, _
                                 pGraphSaveFormat, _
                                 pGraphSaveWidth, _
                                 pGraphSaveHeight, _
                                 pGraphAnnual)
                        lTimeSeries.Clear()
                        lTimeSeries.Add("Observed", Aggregate(lWdmDataSource.DataSets.ItemByKey(5), atcTimeUnit.TUHour, 1, atcTran.TranAverSame))
                        'lTimeSeries.Add("Observed", lWdmDataSource.DataSets.ItemByKey(5))
                        lTimeSeries.Add("Simulated", lWdmDataSource.DataSets.ItemByKey(1109))
                        lTimeSeries.Add("Prec", lWdmDataSource.DataSets.ItemByKey(1010))
                        lTimeSeries(0).Attributes.SetValue("Units", "cfs")
                        lTimeSeries(0).Attributes.SetValue("StepType", pCurveStepType)
                        lTimeSeries(1).Attributes.SetValue("Units", "cfs")
                        lTimeSeries(1).Attributes.SetValue("StepType", pCurveStepType)
                        GraphStorms(lTimeSeries, 2, "outfiles\Storm", pGraphSaveFormat, pGraphSaveWidth, pGraphSaveHeight, lExpertSystem)
                        lTimeSeries.Dispose()
                    Next

                    lExpertSystem = Nothing
                    If lFileCopied Then
                        IO.File.Delete(pBaseName & ".exs")
                    End If
                Catch lEx As ApplicationException
                    Logger.Dbg(lEx.Message)
                End Try
            Next lExpertSystemFileName
        End If

        For Each lSummaryType As String In pSummaryTypes
            Dim lString As String
            Dim lOutFileName As String
            'build collection of operation types to report
            Dim lOperationTypes As New atcCollection
            lOperationTypes.Add("P:", "PERLND")
            lOperationTypes.Add("I:", "IMPLND")
            lOperationTypes.Add("R:", "RCHRES")

            lString = HspfSupport.WatershedSummaryOverland.Report(lHspfUci, lSummaryType, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade, pPerlndSegmentStarts, pImplndSegmentStarts).ToString
            lOutFileName = "outfiles\" & pBaseName & "_" & lSummaryType & "_WatershedOverland.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            lString = HspfSupport.ConstituentBudget.Report(lHspfUci, lSummaryType, lOperationTypes, pBaseName, lHspfBinDataSource, lRunMade).ToString
            lOutFileName = "outfiles\" & pBaseName & "_" & lSummaryType & "_Budget.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            lString = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lRunMade, lSummaryType).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedSummary.txt"
            SaveFileString(lOutFileName, lString)
            lString = Nothing

            Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

            'constituent balance
            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.ConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lLocations, lRunMade, True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_ConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            'watershed constituent balance 
            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalance.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivot.txt"
            SaveFileString(lOutFileName, lString)

            lString = HspfSupport.WatershedConstituentBalance.Report _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, lRunMade, , , , True, 2, 5, 8).ToString
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivotNarrowTab.txt"
            SaveFileString(lOutFileName, lString)
            lOutFileName = "outfiles\" & lSummaryType & "_WatershedConstituentBalancePivotNarrowSpace.txt"
            SaveFileString(lOutFileName, lString.Replace(vbTab, " "))

            If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    "outfiles\", True)
                HspfSupport.WatershedConstituentBalance.ReportsToFiles _
                   (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                    lHspfBinDataSource, pOutputLocations, lRunMade, _
                    "outfiles\", True, True)
            End If
        Next
        Logger.Dbg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
        'Logger.Msg("Reports Written in " & IO.Path.Combine(pTestPath, "outfiles"), "HSPFOutputReports")
    End Sub
End Module
