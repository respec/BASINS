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
    Private pTestPath As String
    Private pBaseName As String
    Private pOutputLocations As New atcCollection
    Private pGraphSaveFormat As String

    Private Sub Initialize()
        pOutputLocations.Clear()

        pGraphSaveFormat = ".png"
        'pGraphSaveFormat = ".emf"

        'Dim lTestName As String = "tinley"
        'Dim lTestName As String = "hspf"
        Dim lTestName As String = "hyd_man"
        'Dim lTestName As String = "calleguas_cat"
        'Dim lTestName As String = "calleguas_nocat"
        'Dim lTestName As String = "SantaClara"

        Select Case lTestName
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
                pTestPath = "C:\test\EXP_CAL\hyd_man.net_cat"
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
                pOutputLocations.Add("R:526")
                pOutputLocations.Add("R:410")
                pOutputLocations.Add("R:880")
        End Select
    End Sub

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Initialize()
        ChDriveDir(pTestPath)
        If FileExists(pBaseName & "Orig.uci") Then
            FileCopy(pBaseName & "Orig.uci", pBaseName & ".uci")
        End If

        'open uci file
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, pBaseName & ".uci")
        lHspfUci.Save()

        'open WDM file
        Dim lWdmFileName As String = pTestPath & "\" & pBaseName & ".wdm"
        Dim lWdmDataSource As New atcDataSourceWDM()
        lWdmDataSource.Open(lWdmFileName)

        Dim lOutFileName As String
        Dim lExpertSystemFileNames As New NameValueCollection
        AddFilesInDir(lExpertSystemFileNames, CurDir, False, "*.exs")
        Dim lExpertSystem As HspfSupport.ExpertSystem
        For Each lExpertSystemFileName As String In lExpertSystemFileNames
            Try
                Dim lFileCopied As Boolean = False
                If FilenameOnly(lExpertSystemFileName).ToLower <> pBaseName.ToLower Then
                    FileCopy(lExpertSystemFileName, pBaseName & ".exs")
                    lFileCopied = True
                End If
                lExpertSystem = New HspfSupport.ExpertSystem(lHspfUci, lWdmDataSource)
                Dim lStr As String = lExpertSystem.Report
                SaveFileString("outfiles\ExpertSysStats-" & FilenameOnly(lExpertSystemFileName) & ".txt", lStr)

                'lStr = lExpertSystem.AsString 'NOTE:just testing
                'SaveFileString(FilenameOnly(lHspfUci.Name) & ".exx", lStr)

                Dim lCons As String = "Flow"
                For lSiteIndex As Integer = 1 To lExpertSystem.Sites.Count
                    Dim lSite As String = lExpertSystem.Sites(lSiteIndex).Name
                    Dim lArea As Double = lExpertSystem.Sites(lSiteIndex).Area
                    Dim lSimTSer As atcTimeseries = InchesToCfs(lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(0)), lArea)
                    lSimTSer.Attributes.SetValue("Units", "Flow (cfs)")
                    lSimTSer.Attributes.SetValue("YAxis", "Left")
                    Dim lObsTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(1))
                    lObsTSer.Attributes.SetValue("YAxis", "Left")
                    lObsTSer.Attributes.SetValue("Units", "Flow (cfs)")
                    lStr = HspfSupport.DailyMonthlyCompareStats.Report(lHspfUci, _
                                                                       lCons, lSite, _
                                                                       lSimTSer, lObsTSer, _
                                                                       lExpertSystem.SDateJ, _
                                                                       lExpertSystem.EDateJ)
                    lOutFileName = "outfiles\DailyMonthly" & lCons & "Stats" & "-" & lSite & ".txt"
                    SaveFileString(lOutFileName, lStr)
                    Dim lDataGroup As New atcDataGroup
                    lDataGroup.Add(SubsetByDate(lSimTSer, _
                                                lExpertSystem.SDateJ, _
                                                lExpertSystem.EDateJ, Nothing))
                    lDataGroup.Add(SubsetByDate(lObsTSer, _
                                                lExpertSystem.SDateJ, _
                                                lExpertSystem.EDateJ, Nothing))
                    Dim lOutFileBase As String = "outfiles\" & lCons & "_" & lSite
                    Dim lZgc As ZedGraphControl
                    'duration plot
                    lZgc = CreateZgc()
                    Dim lGraphDur As New clsGraphProbability(lDataGroup, lZgc)
                    lZgc.SaveIn(lOutFileBase & "_dur" & pGraphSaveFormat)
                    lGraphDur.Dispose()
                    lZgc.Dispose()
                    'cummulative difference
                    lZgc = CreateZgc()
                    Dim lGraphCum As New clsGraphCumulativeDifference(lDataGroup, lZgc)
                    lZgc.SaveIn(lOutFileBase & "_cumDif" & pGraphSaveFormat)
                    lGraphCum.Dispose()
                    lZgc.Dispose()
                    'scatter
                    lZgc = CreateZgc()
                    lZgc.MasterPane.PaneList(0).YAxis.Type = ZedGraph.AxisType.Log
                    Dim lGraphScatter As New clsGraphScatter(lDataGroup, lZgc)
                    lZgc.SaveIn(lOutFileBase & "_scatDay" & pGraphSaveFormat)
                    lGraphScatter.Dispose()
                    lZgc.Dispose()
                    'scatter - LZS vs Error(cfs)
                    Dim lTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(9))
                    lZgc = CreateZgc()
                    If GraphScatterError(lZgc, lDataGroup, lExpertSystem.SDateJ, lExpertSystem.EDateJ, lTSer, "LZS (in)") Then
                        lZgc.SaveIn(lOutFileBase & "_Error_LZS" & pGraphSaveFormat)
                    End If
                    'scatter - UZS vs Error(cfs)
                    lTSer = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(8))
                    lZgc = CreateZgc()
                    If GraphScatterError(lZgc, lDataGroup, lExpertSystem.SDateJ, lExpertSystem.EDateJ, lTSer, "UZS (in)") Then
                        lZgc.SaveIn(lOutFileBase & "_Error_UZS" & pGraphSaveFormat)
                    End If
                    'scatter - Observed vs Error(cfs)    
                    lZgc = CreateZgc()
                    If GraphScatterError(lZgc, lDataGroup, lExpertSystem.SDateJ, lExpertSystem.EDateJ, lObsTSer, "Observed (cfs)", AxisType.Log) Then
                        lZgc.SaveIn(lOutFileBase & "_Error_ObsFlow" & pGraphSaveFormat)
                    End If
                    'add precip to aux axis
                    Dim lPrecTSer As atcTimeseries = lWdmDataSource.DataSets.ItemByKey(lExpertSystem.Sites(lSiteIndex).Dsn(5))
                    lPrecTSer.Attributes.SetValue("YAxis", "Aux")
                    lDataGroup.Add(SubsetByDate(lPrecTSer, _
                                                lExpertSystem.SDateJ, _
                                                lExpertSystem.EDateJ, Nothing))
                    'timeseries - arith
                    lZgc = CreateZgc()
                    Dim lGrapher As New clsGraphTime(lDataGroup, lZgc)
                    lZgc.MasterPane.PaneList(0).YAxis.Title.Text = "Precip (in)"
                    lZgc.SaveIn(lOutFileBase & pGraphSaveFormat)
                    With lZgc.MasterPane.PaneList(1) 'main pane, not aux
                        .YAxis.Type = ZedGraph.AxisType.Log
                        'ScaleAxis(lDataGroup, .YAxis)
                        .YAxis.Scale.Max *= 4 'wag!
                        .YAxis.Scale.MaxAuto = False
                        .YAxis.Scale.IsUseTenPower = False
                    End With
                    'timeseries - log
                    lZgc.SaveIn(lOutFileBase & "_log " & pGraphSaveFormat)
                    lZgc.Dispose()
                Next lSiteIndex
                lExpertSystem = Nothing
                If lFileCopied Then
                    Kill(pBaseName & ".exs")
                End If
            Catch lEx As ApplicationException
                Logger.Dbg(lEx.Message)
            End Try
        Next lExpertSystemFileName

        'open HBN file
        Dim lHspfBinFileName As String = pTestPath & "\" & pBaseName & ".hbn"
        Dim lHspfBinDataSource As New atcTimeseriesFileHspfBinOut()
        lHspfBinDataSource.Open(lHspfBinFileName)

        Dim lSummaryType As String = "Water"

        'watershed summary
        Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        Dim lString As Text.StringBuilder = HspfSupport.WatershedSummary.Report(lHspfUci, lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime, lSummaryType)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedSummary.txt"
        SaveFileString(lOutFileName, lString.ToString)
        lString = Nothing

        'build collection of operation types to report
        Dim lOperationTypes As New atcCollection
        lOperationTypes.Add("P:", "PERLND")
        lOperationTypes.Add("I:", "IMPLND")
        lOperationTypes.Add("R:", "RCHRES")
        Dim lLocations As atcCollection = lHspfBinDataSource.DataSets.SortedAttributeValues("Location")

        'constituent balance
        lString = HspfSupport.ConstituentBalance.Report _
           (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
            lHspfBinDataSource, lLocations, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "ConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

        'watershed constituent balance 
        lString = HspfSupport.WatershedConstituentBalance.Report _
           (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
            lHspfBinDataSource, lHspfBinFileInfo.LastWriteTime)
        lOutFileName = "outfiles\" & lSummaryType & "_" & "WatershedConstituentBalance.txt"
        SaveFileString(lOutFileName, lString.ToString)

        If pOutputLocations.Count > 0 Then 'subwatershed constituent balance 
            HspfSupport.WatershedConstituentBalance.ReportsToFiles _
               (lHspfUci, lSummaryType, lOperationTypes, pBaseName, _
                lHspfBinDataSource, pOutputLocations, lHspfBinFileInfo.LastWriteTime, _
                "outfiles\")
        End If
    End Sub

    Function GraphScatterError(ByVal aZgc As ZedGraphControl, ByVal aDataGroup As atcDataGroup, _
                               ByVal aSDateJ As Double, ByVal aEDateJ As Double, _
                               ByVal aXAxisTser As atcTimeseries, ByVal aXAxisTitle As String, _
                               Optional ByVal aXAxisType As ZedGraph.AxisType = AxisType.Linear) As Boolean
        Dim lMath As New atcTimeseriesMath.atcTimeseriesMath
        Dim lMathArgs As New atcDataAttributes
        lMathArgs.SetValue("timeseries", aDataGroup)
        If lMath.Open("subtract", lMathArgs) Then
            Dim lDataGroupError As New atcDataGroup
            lDataGroupError.Add(SubsetByDate(aXAxisTser, aSDateJ, aEDateJ, Nothing))
            lDataGroupError.Add(SubsetByDate(lMath.DataSets(0), aSDateJ, aEDateJ, Nothing))
            Dim lGraphScatter As clsGraphScatter = New clsGraphScatter(lDataGroupError, aZgc)
            With aZgc.MasterPane.PaneList(0)
                .XAxis.Title.Text = aXAxisTitle
                .XAxis.Type = aXAxisType
                If aXAxisType = AxisType.Linear Then
                    Scalit(aXAxisTser.Attributes.GetValue("Minimum"), _
                           aXAxisTser.Attributes.GetValue("Maximum"), _
                           False, .XAxis.Scale.Min, .XAxis.Scale.Max)
                Else
                    .XAxis.Scale.Min = 1
                    .XAxis.Scale.Max = aXAxisTser.Attributes.GetDefinedValue("Maximum").Value * 2
                    .XAxis.Scale.IsUseTenPower = False
                End If
                .YAxis.Title.Text = "Error (cfs)"
                If Math.Abs(.YAxis.Scale.Min) > .YAxis.Scale.Max Then
                    .YAxis.Scale.Max = -.YAxis.Scale.Min
                Else
                    .YAxis.Scale.Min = -.YAxis.Scale.Max
                End If
            End With
            lGraphScatter.Dispose()
            Return True
        Else 'TODO:need error message
            Return False
        End If
    End Function
End Module
