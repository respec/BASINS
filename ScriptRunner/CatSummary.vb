Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptCatSummary
    Private Const pFieldWidth As Integer = 12
    Private Const pTestPath As String = "C:\test\SegmentBalance\"
    Private Const pCatSummaryFileName As String = "CatSummary.txt"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'For each UCI file in the specified (pTestPath) folder, 
        'do a Cat Summary Report 

        'Requirements:
        'UCI, WDM and HBN must reside in the same folder,
        'base name of UCI = base name of HBN file = base name of WDM file

        'Output:
        'writes to the pTestPath folder, a file named "CatSummary.txt",
        'appending to the file for each UCI (scenario)

        Logger.Dbg("Start")

        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'delete the output file if it already exists
        If FileExists(pCatSummaryFileName) Then Kill(pCatSummaryFileName)

        'build collection of scenarios (uci base names) to report
        Dim lUcis As New System.Collections.Specialized.NameValueCollection
        AddFilesInDir(lUcis, pTestPath, False, "*.uci")
        Dim lScenarios As New atcCollection
        For Each lUci As String In lUcis
            lScenarios.Add(FilenameNoPath(FilenameNoExt(lUci)))
        Next

        'declare a new data manager to manage the hbn and wdm files
        Dim lDataManager As New atcDataManager(aMapWin)

        'loop thru each scenario (uci name)
        For Each lScenario As String In lScenarios

            'open the corresponding hbn file
            Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
            Dim lHspfBinFileName As String = lScenario & ".hbn"
            Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
            If Not FileExists(lHspfBinFileName) Then
                'if hbn doesnt exist, make a guess at what the name might be
                lHspfBinFileName = lHspfBinFileName.Replace(".hbn", ".base.hbn")
                Logger.Dbg("  NameUpdated " & lHspfBinFileName)
            End If
            Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
            lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
            Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)

            'open the corresponding hbn file
            Dim lHspfWdmFile As atcDataSource = New atcWDM.atcDataSourceWDM
            Dim lHspfWdmFileName As String = lScenario & ".wdm"
            Logger.Dbg(" AboutToOpen " & lHspfWdmFileName)
            If Not FileExists(lHspfWdmFileName) Then
                'if hbn doesnt exist, make a guess at what the name might be
                lHspfWdmFileName = lHspfWdmFileName.Replace(".wdm", ".base.wdm")
                Logger.Dbg("  NameUpdated " & lHspfWdmFileName)
            End If
            lDataManager.OpenDataSource(lHspfWdmFile, lHspfWdmFileName, Nothing)
            Logger.Dbg(" DataSetCount " & lHspfWdmFile.DataSets.Count)

            'call main cat summary routine
            DoCatSummary(lScenario, lDataManager)

            lDataManager.DataSources.Remove(lHspfBinFile)
            lHspfBinFile.DataSets.Clear()
            lHspfBinFile = Nothing
            lDataManager.DataSources.Remove(lHspfWdmFile)
            lHspfWdmFile.DataSets.Clear()
            lHspfWdmFile = Nothing
        Next lScenario
    End Sub

    Friend Sub DoCatSummary(ByVal aScenario As String, ByVal aDataManager As atcDataManager)
        Logger.Dbg("DoCatSummary for " & aScenario)

        Dim lString As New Text.StringBuilder
        lString.Append(aScenario)

        'Get this hard coded stuff from CAT endpoints/variations!

        Dim lMetDataGroup As atcDataGroup = aDataManager.DataSets.FindData("Location", "SEG1")
        Logger.Dbg("     MetMatchingDatasetCount " & lMetDataGroup.Count)
        lMetDataGroup.Add(aDataManager.DataSets.FindData("Location", "_A24013"))
        lMetDataGroup.Add(aDataManager.DataSets.FindData("Location", "A24013"))
        Logger.Dbg("     AdditionalMetMatchingDatasetCount " & lMetDataGroup.Count)

        If lMetDataGroup.Count > 0 Then
            lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "HPRC", "Sum"))
            lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "ATMP", "Mean"))
            lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "EVAP", "Sum"))
        End If

        Dim lRchDataGroupW As atcDataGroup = aDataManager.DataSets.FindData("Location", "RIV9")
        If lRchDataGroupW.Count > 0 Then
            lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "WATR", "Mean"))
            lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "WATR", "1Hi100"))
            lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "FLOW", "7Q10"))
        End If

        Dim lRchDataGroup As atcDataGroup = aDataManager.DataSets.FindData("Location", "R:9")
        If lRchDataGroup.Count > 0 Then
            lString.Append(AnnualValue(lRchDataGroup, "RO", "Mean"))
            lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "SumAnnual"))
            lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "SumAnnual", True))
            lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "Mean"))
            lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "Mean", True))
            lString.Append(AnnualValue(lRchDataGroup, "P-TOT-OUT", "SumAnnual"))
            lString.Append(AnnualValue(lRchDataGroup, "N-TOT-OUT", "SumAnnual"))
        End If
        lString.AppendLine()

        Logger.Dbg("CAT Report Add - " & lString.ToString)
        AppendFileString(pCatSummaryFileName, lString.ToString)
    End Sub

    Private Function AnnualValue(ByVal aDataGroup As atcDataGroup, ByVal aCons As String, ByVal aTrans As String, Optional ByVal aSummer As Boolean = False) As String
        Dim lConsDataGroup As atcDataGroup = aDataGroup.FindData("Constituent", aCons)
        Logger.Dbg("     " & aCons & "MatchingDatasetCount " & lConsDataGroup.Count)
        Dim lString As String = ""
        If lConsDataGroup.Count > 0 Then
            Dim lTempDataSet As atcDataSet = lConsDataGroup.Item(0)
            Dim lValue As Double = lTempDataSet.Attributes.GetDefinedValue(aTrans).Value
            lString = vbTab & DF(lValue)
            If aSummer Then
                Dim lSeasons As New atcSeasons.atcSeasonsMonth
                Dim lSeasonalAttributes As New atcDataAttributes
                Dim lCalculatedAttributes As New atcDataAttributes
                lSeasonalAttributes.SetValue(aTrans, 0) 'fluxes are summed from daily, monthly or annual to annual
                lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)
                lValue = 0
                For lIndex As Integer = 3 To 9
                    lValue += lCalculatedAttributes(lIndex).Value 'AMJJASO
                Next lIndex
                lString &= vbTab & DF(lValue)
            End If
        End If
        Return lString
    End Function

    Private Function AnnualAndSeasonalValues(ByVal aDataGroup As atcDataGroup, ByVal aCons As String, ByVal aTrans As String) As String
        Dim lConsDataGroup As atcDataGroup = aDataGroup.FindData("Constituent", aCons)
        Logger.Dbg("     " & aCons & "MatchingDatasetCount " & lConsDataGroup.Count)
        Dim lString As String = ""
        If lConsDataGroup.Count > 0 Then
            Dim lTempDataSet As atcDataSet = lConsDataGroup.Item(0)
            Dim lValue As Double = lTempDataSet.Attributes.GetDefinedValue(aTrans).Value
            lString = vbTab & DF(lValue)
            Dim lSeasons As New atcSeasons.atcSeasonsMonth
            Dim lSeasonalAttributes As New atcDataAttributes
            Dim lCalculatedAttributes As New atcDataAttributes
            lSeasonalAttributes.SetValue(aTrans, 0) 'fluxes are summed from daily, monthly or annual to annual
            lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)
            lValue = lCalculatedAttributes(0).Value + lCalculatedAttributes(1).Value + lCalculatedAttributes(11).Value   'DJF
            If aTrans = "Mean" Then lValue = lValue / 3
            lString &= vbTab & DF(lValue)
            lValue = lCalculatedAttributes(2).Value + lCalculatedAttributes(3).Value + lCalculatedAttributes(4).Value  'MAM
            If aTrans = "Mean" Then lValue = lValue / 3
            lString &= vbTab & DF(lValue)
            lValue = lCalculatedAttributes(5).Value + lCalculatedAttributes(6).Value + lCalculatedAttributes(7).Value  'JJA
            If aTrans = "Mean" Then lValue = lValue / 3
            lString &= vbTab & DF(lValue)
            lValue = lCalculatedAttributes(8).Value + lCalculatedAttributes(9).Value + lCalculatedAttributes(10).Value  'SON
            If aTrans = "Mean" Then lValue = lValue / 3
            lString &= vbTab & DF(lValue)
        End If
        Return lString

    End Function

    Private Function DF(ByVal aValue As Double, Optional ByVal aDecimalPlaces As Integer = 3) As String
        Dim lFormat As String
        If aDecimalPlaces > 1 Then
            lFormat = "###,##0.0" & StrDup(aDecimalPlaces - 1, "#")
        Else
            lFormat = "###,##0.0"
        End If
        Dim lString As String = DoubleToString(aValue, , lFormat, , , 5)
        Dim dp As Integer = lString.IndexOf("."c)
        If dp >= 0 Then
            Dim laddLeft As Integer = pFieldWidth - 5 - dp
            If laddLeft > 0 Then lString = Space(laddLeft) & lString
        End If
        Return lString.PadRight(pFieldWidth)
        'Return Trim(Format(aValue, "##########0." & StrDup(aDecimalPlaces, "0")))
    End Function

End Module
