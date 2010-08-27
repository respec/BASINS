Imports atcData
Imports atcData.atcTimeseriesGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcClimateAssessmentTool

Imports Microsoft.VisualBasic
Imports System

Public Module CatSummary
    Private Const pFieldWidth As Integer = 12
    Private pTestPath As String = "C:\mono_luChange\output\lu2030a2\"
    Private Const pCatSummaryFileName As String = "CatSummary.txt"
    Private pCatXMLFile As String = "VaryPrecTempHbnPrepare.xml"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat
    Private pCatList As ArrayList


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

        'The commented out block below is the direct approach to extract summary result from output wdm and hbns
        Dim lScenList As New ArrayList
        lScenList.Add("C:\mono_luChange\output\lu2030a2\")
        'lScenList.Add("C:\mono_luChange\output\lu2030b2\")
        'lScenList.Add("C:\mono_luChange\output\lu2090a2\")
        'lScenList.Add("C:\mono_luChange\output\lu2090b2\")
        For Each pTestPath In lScenList

            'change to the directory of the current project
            ChDriveDir(pTestPath)
            Logger.Dbg(" CurDir:" & CurDir())

            'delete the output file if it already exists
            If FileExists(pCatSummaryFileName) Then
                Kill(pCatSummaryFileName)
            End If

            'read in the xml for the endpoints and parse out the variations
            Logger.Dbg("read in endpoints")
            With pCat
                .XML = IO.File.ReadAllText(pCatXMLFile) ' TODO: ZZ deal with no need for uci files
            End With

            'build collection of location::constituent<->operation list
            pCatList = New ArrayList
            Try
                For Each lVar As atcClimateAssessmentTool.atcVariation In pCat.Endpoints
                    'ATTENTION:
                    'Put the selected months here assuming there is only one dataset per variation
                    'If not, then the selected months for the first dataset within the current variation is added into all datasets
                    'So has to pay attention here
                    Dim lSelectedMonths As String = ""
                    If lVar.Seasons IsNot Nothing Then
                        For li As Integer = 1 To 12
                            If lVar.Seasons.SeasonSelected(li) Then lSelectedMonths &= li & ","
                        Next
                        'Rid of the last , at the end of the lSelectedMonths string
                        lSelectedMonths = lSelectedMonths.TrimEnd(",")
                    End If

                    For Each ldataset As atcDataSet In lVar.DataSets
                        Dim lCatTemp(4) As String
                        Dim lChkSeasons As Boolean = True
                        lCatTemp(0) = ldataset.Attributes.GetFormattedValue("location")  ' e.g. Seg1
                        lCatTemp(1) = ldataset.Attributes.GetFormattedValue("constituent") ' e.g. HPRC
                        lCatTemp(2) = lVar.Operation.ToString ' e.g. Sum
                        lCatTemp(3) = lSelectedMonths ' either this is empty string or a comma separated list of Months
                        lCatTemp(4) = ""
                        If lVar.Seasons IsNot Nothing Then
                            lCatTemp(4) = lVar.Seasons.Name.Substring(lVar.Seasons.Name.IndexOf("-") + 2) ' to get the 'Month' or 'Traditional' part
                        End If
                        If lCatTemp(1) = "HPRC" Or lCatTemp(1) = "ATMP" Or lCatTemp(1) = "EVAP" Then lChkSeasons = False
                        If Not foundMatchingEndpoint(pCatList, lCatTemp, lChkSeasons) Then
                            pCatList.Add(lCatTemp)
                        End If
                    Next
                Next
            Catch ex As Exception
                Logger.Msg("Cat structure failed: " & ex.ToString)
            End Try

            'build collection of scenarios (uci base names) to report
            Dim lUcis As New System.Collections.Specialized.NameValueCollection
            'do we want the 'base.uci'?
            AddFilesInDir(lUcis, pTestPath, False, "*.uci")
            Dim lScenarios As New atcCollection
            For Each lUci As String In lUcis
                lScenarios.Add(FilenameNoPath(FilenameNoExt(lUci)))
            Next

            'declare a new data manager to manage the hbn and wdm files
            'Dim lDataManager As New atcDataManager(aMapWin)

            'loop thru each scenario (uci name)
            For Each lScenario As String In lScenarios
                Dim lScenarioDataGroup As New atcTimeseriesGroup
                'If atcDataManager.DataSources.Count > 0 Then
                '    Logger.Dbg("Clearing " & atcDataManager.DataSources.Count & " datasources.")
                '    atcDataManager.DataSources.Clear()
                'End If

                'open the corresponding hbn file
                Dim lHspfBinFile As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
                Dim lHspfBinFileName As String = lScenario & ".hbn"
                Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
                If Not FileExists(lHspfBinFileName) Then
                    'if hbn doesnt exist, make a guess at what the name might be
                    lHspfBinFileName = lHspfBinFileName.Replace(".hbn", ".base.hbn")
                    Logger.Dbg("  NameUpdated " & lHspfBinFileName)
                End If
                Dim lHspfBinFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
                If lHspfBinFile.Open(lHspfBinFileName) Then
                    Logger.Dbg(" Adding " & lHspfBinFile.DataSets.Count)
                    lScenarioDataGroup.AddRange(lHspfBinFile.DataSets)
                End If
                Logger.Dbg(" DataSetCount " & lHspfBinFile.DataSets.Count)

                'open the corresponding wdm file
                Dim lHspfWdmFile As New atcWDM.atcDataSourceWDM
                Dim lHspfWdmFileName As String = lScenario & ".wdm"
                Logger.Dbg(" AboutToOpen " & lHspfWdmFileName)
                If Not FileExists(lHspfWdmFileName) Then
                    'if wdm doesnt exist, make a guess at what the name might be
                    lHspfWdmFileName = lHspfWdmFileName.Replace(".wdm", ".base.wdm")
                    Logger.Dbg("  NameUpdated " & lHspfWdmFileName)
                End If
                If lHspfWdmFile.Open(lHspfWdmFileName) Then
                    Logger.Dbg(" Adding " & lHspfWdmFile.DataSets.Count)
                    lScenarioDataGroup.AddRange(lHspfWdmFile.DataSets)
                End If
                Logger.Dbg(" DataSetCount " & lHspfWdmFile.DataSets.Count)

                'open the corresponding output wdm file
                Dim lHspfOutWdmFile As New atcWDM.atcDataSourceWDM
                Dim lHspfOutWdmFileName As String = lScenario & ".output.wdm"
                If IO.File.Exists(lHspfOutWdmFileName) Then
                    Logger.Dbg("  Opening " & lHspfOutWdmFileName)
                    If lHspfOutWdmFile.Open(lHspfOutWdmFileName) Then
                        Logger.Dbg(" DataSetCount " & lHspfOutWdmFile.DataSets.Count)
                        lScenarioDataGroup.AddRange(lHspfOutWdmFile.DataSets)
                    End If
                Else
                    Logger.Dbg("  Not Opening " & lHspfOutWdmFileName)
                End If

                'call main cat summary routine
                DoCatSummary(lScenarioDataGroup, lScenario)

                lScenarioDataGroup.Clear()

                atcDataManager.DataSources.Remove(lHspfBinFile)
                lHspfBinFile.DataSets.Clear()
                lHspfBinFile = Nothing

                atcDataManager.DataSources.Remove(lHspfWdmFile)
                lHspfWdmFile.DataSets.Clear()
                lHspfWdmFile = Nothing

                atcDataManager.DataSources.Remove(lHspfOutWdmFile)
                lHspfOutWdmFile.DataSets.Clear()
                lHspfOutWdmFile = Nothing

            Next lScenario

            With pCat
                .Inputs.Clear()
                .Endpoints.Clear()
                .PreparedInputs.Clear()
            End With

            pCatList.Clear()
            pCatList = Nothing

        Next
        Logger.Msg("Done summary of CAT run results")

    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        pCat.StartIterationMessage(aIteration)
    End Sub

    Private Sub UpdateResults(ByVal aResultsFilename As String) Handles pCat.UpdateResults
        Try
            Windows.Forms.Application.DoEvents()
        Catch
            'stop
        End Try
        SaveFileString(aResultsFilename, pCat.ResultsGrid.ToString)
    End Sub

    Public Function foundMatchingEndpoint(ByRef aVarList As ArrayList, ByRef aCat() As String, ByVal aChkSeasons As Boolean) As Boolean
        'This function loops through the Endpoints-Variation collection to search for the matching one as user specified entry
        'Found it, return True
        'Not found it, return False
        'This function is used to make sure there is only unique set of variations are used to do the cat summary from run results
        Dim foundMatch As Boolean = False
        If aVarList Is Nothing Then Return False
        For Each lCatTemp() As String In aVarList
            If lCatTemp(0) = aCat(0) And lCatTemp(1) = aCat(1) And lCatTemp(2) = aCat(2) Then
                foundMatch = True
                If aChkSeasons Then
                    If Not String.Compare(lCatTemp(3), aCat(3)) = 0 Then foundMatch = False
                End If
            End If
        Next
        Return foundMatch
    End Function

    Friend Sub DoCatSummary(ByVal aDataGroup As atcTimeseriesGroup, ByVal aScenario As String)
        Logger.Dbg("DoCatSummary for " & aScenario)

        Dim lString As New Text.StringBuilder
        lString.Append(aScenario)

        'Get this hard coded stuff from CAT endpoints/variations!
        Dim lMetDataGroup As atcTimeseriesGroup = aDataGroup.FindData("Location", "SEG1")
        Logger.Dbg("     MetMatchingDatasetCount " & lMetDataGroup.Count)
        lMetDataGroup.Add(aDataGroup.FindData("Location", "_A24013"))
        lMetDataGroup.Add(aDataGroup.FindData("Location", "A24013"))
        Logger.Dbg("     AdditionalMetMatchingDatasetCount " & lMetDataGroup.Count)

        Dim lRchDataGroupW As atcTimeseriesGroup = aDataGroup.FindData("Location", "RIV9")
        Dim lRchDataGroup As atcTimeseriesGroup = aDataGroup.FindData("Location", "R:9")

        For Each lCat() As String In pCatList
            If lCat(0) = "SEG1" Then
                If lMetDataGroup.Count > 0 Then
                    lString.Append(AnnualAndSeasonalValues(lMetDataGroup, lCat(1), lCat(2)))
                End If
            End If

            'For Each lCat() As String In pCatList
            If lCat(0) = "RIV9" Then
                If lRchDataGroupW.Count > 0 Then
                    lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, lCat(1), lCat(2), lCat(3), lCat(4)))
                Else
                    lString.Append(vbTab & "NODATA")
                End If
            End If
            'Next

            'For Each lCat() As String In pCatList
            If lCat(0) = "R:9" Then
                If lRchDataGroup.Count > 0 Then
                    If lCat(3).Length > 1 Then
                        lString.Append(AnnualValue(lRchDataGroup, lCat(1), lCat(2), lCat(3), True))
                    Else
                        lString.Append(AnnualValue(lRchDataGroup, lCat(1), lCat(2)))
                    End If
                End If
            End If
        Next

        '******* Original ********
        'Dim lMetDataGroup As atcDataGroup = atcDataManager.DataSets.FindData("Location", "SEG1")
        'Logger.Dbg("     MetMatchingDatasetCount " & lMetDataGroup.Count)
        'lMetDataGroup.Add(atcDataManager.DataSets.FindData("Location", "_A24013"))
        'lMetDataGroup.Add(atcDataManager.DataSets.FindData("Location", "A24013"))
        'Logger.Dbg("     AdditionalMetMatchingDatasetCount " & lMetDataGroup.Count)

        'If lMetDataGroup.Count > 0 Then
        '    lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "HPRC", "Sum"))
        '    lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "ATMP", "Mean"))
        '    lString.Append(AnnualAndSeasonalValues(lMetDataGroup, "EVAP", "Sum"))
        'End If

        'Dim lRchDataGroupW As atcDataGroup = atcDataManager.DataSets.FindData("Location", "RIV9")
        'If lRchDataGroupW.Count > 0 Then
        '    lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "WATR", "Mean"))
        '    lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "WATR", "1Hi100"))
        '    lString.Append(AnnualAndSeasonalValues(lRchDataGroupW, "FLOW", "7Q10"))
        'End If

        'Dim lRchDataGroup As atcDataGroup = atcDataManager.DataSets.FindData("Location", "R:9")
        'If lRchDataGroup.Count > 0 Then
        '    lString.Append(AnnualValue(lRchDataGroup, "RO", "Mean"))
        '    lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "SumAnnual"))
        '    lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "SumAnnual", True))
        '    lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "Mean"))
        '    lString.Append(AnnualValue(lRchDataGroup, "ROSED-TOT", "Mean", True))
        '    lString.Append(AnnualValue(lRchDataGroup, "P-TOT-OUT", "SumAnnual"))
        '    lString.Append(AnnualValue(lRchDataGroup, "N-TOT-OUT", "SumAnnual"))
        'End If

        lString.AppendLine()

        Logger.Dbg("CAT Report Add - " & lString.ToString)
        AppendFileString(pCatSummaryFileName, lString.ToString)
    End Sub

    Private Function AnnualValue(ByVal aDataGroup As atcTimeseriesGroup, ByVal aCons As String, ByVal aTrans As String, Optional ByVal aMonthsList As String = "", Optional ByVal aSummer As Boolean = False) As String
        Dim lString As String = ""

        Try
            Dim lConsDataGroup As atcTimeseriesGroup = aDataGroup.FindData("Constituent", aCons)
            Logger.Dbg("     " & aCons & "MatchingDatasetCount " & lConsDataGroup.Count)
            If lConsDataGroup.Count > 0 Then
                Dim lTempDataSet As atcDataSet = lConsDataGroup.Item(0)
                Dim lValue As Double = lTempDataSet.Attributes.GetDefinedValue(aTrans).Value
                If aMonthsList = "" Then
                    lString = vbTab & DecimalAlign(lValue)
                End If
                If aSummer Then ' if aSummer is true, then this also imply that aMonthList is not empty
                    Dim lSeasons As New atcSeasonsMonth
                    Dim lSeasonalAttributes As New atcDataAttributes
                    Dim lCalculatedAttributes As New atcDataAttributes
                    lSeasonalAttributes.SetValue(aTrans, 0) 'fluxes are summed from daily, monthly or annual to annual
                    lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)
                    lValue = 0

                    For Each lVar As atcVariation In pCat.Endpoints
                        If lVar.DataSets(0).Attributes.GetFormattedValue("Location") = "R:9" AndAlso _
                           lVar.DataSets(0).Attributes.GetFormattedValue("Constituent") = aCons AndAlso _
                           lVar.Operation = aTrans AndAlso _
                           lVar.Seasons IsNot Nothing Then
                            Dim lData As atcTimeseries = lTempDataSet
                            lData = lVar.Seasons.SplitBySelected(lData, Nothing).Item(0)
                            lValue = lData.Attributes.GetFormattedValue(aTrans)
                            Exit For
                        End If
                    Next

                    'Dim lMonthsStrArray() As String = aMonthsList.Split(",")
                    'For Each ls As String In lMonthsStrArray
                    '    lValue += lCalculatedAttributes(Integer.Parse(ls) - 1).Value
                    'Next
                    ''For lIndex As Integer = 3 To 9
                    ''    lValue += lCalculatedAttributes(lIndex).Value 'AMJJASO
                    ''Next lIndex
                    'If aTrans.StartsWith("Mean") Then
                    '    lValue = lValue / lMonthsStrArray.Length ' to get the Mean of the seasonal months involved
                    'End If

                    lString &= vbTab & DecimalAlign(lValue)
                End If
            End If
        Catch ex As Exception
            Logger.Msg("AnnualValue Error: " & ex.ToString)
        End Try

        Return lString
    End Function

    Private Function AnnualAndSeasonalValues(ByVal aDataGroup As atcTimeseriesGroup, ByVal aCons As String, ByVal aTrans As String, Optional ByVal aMonthList As String = "", Optional ByVal aSeasonType As String = "") As String
        Dim lString As String = ""
        Try
            Dim lConsDataGroup As atcTimeseriesGroup = aDataGroup.FindData("Constituent", aCons)
            Logger.Dbg("     " & aCons & "MatchingDatasetCount " & lConsDataGroup.Count)
            If lConsDataGroup.Count > 0 Then
                Dim lTempDataSet As atcDataSet = Nothing
                lTempDataSet = lConsDataGroup.Item(0)
                If lTempDataSet Is Nothing Then
                    Logger.Msg("AnnualAndSeasonalValues Error: The dataset: " & lConsDataGroup.Item(0).ToString & " is Empty")
                End If

                Dim lValue As Double
                If aMonthList = "" AndAlso aSeasonType = "" Then
                    Dim lAttributeDefinedValue As atcDefinedValue = lTempDataSet.Attributes.GetDefinedValue(aTrans)
                    If lAttributeDefinedValue Is Nothing Then
                        lValue = Double.NaN
                    Else
                        lValue = lAttributeDefinedValue.Value
                    End If
                    lString = vbTab & DecimalAlign(lValue)
                End If

                Dim lSeasons As New atcSeasonsMonth
                Dim lSeasonalAttributes As New atcDataAttributes
                Dim lCalculatedAttributes As New atcDataAttributes
                lSeasonalAttributes.SetValue(aTrans, 0) 'fluxes are summed from daily, monthly or annual to annual
                lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)

                If Not aCons.StartsWith("FLOW") Then 'The simulation result summary does not include 3-month means for any 'FLOW' constituents
                    Try
                        lValue = lCalculatedAttributes(0).Value + lCalculatedAttributes(1).Value + lCalculatedAttributes(11).Value   'DJF
                        If aTrans = "Mean" Then lValue = lValue / 3
                    Catch ex As Exception
                        lValue = Double.NaN
                    End Try
                    lString &= vbTab & DecimalAlign(lValue)

                    Try
                        lValue = lCalculatedAttributes(2).Value + lCalculatedAttributes(3).Value + lCalculatedAttributes(4).Value  'MAM
                        If aTrans = "Mean" Then lValue = lValue / 3
                    Catch ex As Exception
                        lValue = Double.NaN
                    End Try
                    lString &= vbTab & DecimalAlign(lValue)
                    Try
                        lValue = lCalculatedAttributes(5).Value + lCalculatedAttributes(6).Value + lCalculatedAttributes(7).Value  'JJA
                        If aTrans = "Mean" Then lValue = lValue / 3
                    Catch ex As Exception
                        lValue = Double.NaN
                    End Try
                    lString &= vbTab & DecimalAlign(lValue)
                    Try
                        lValue = lCalculatedAttributes(8).Value + lCalculatedAttributes(9).Value + lCalculatedAttributes(10).Value  'SON
                        If aTrans = "Mean" Then lValue = lValue / 3
                    Catch ex As Exception
                        lValue = Double.NaN
                    End Try
                    lString &= vbTab & DecimalAlign(lValue)
                Else 'This is for FLOW
                    lValue = 0.0
                    If Not (aMonthList = "" AndAlso aSeasonType = "") Then 'There is a seasonal list of month, either it is 'Month' or 'Traditional' Type
                        If aSeasonType = "Month" Then
                            'Loop through all the month as previously done
                        ElseIf aSeasonType = "Traditional" Then
                            For Each lVar As atcVariation In pCat.Endpoints
                                If lVar.Operation = aTrans AndAlso _
                                   lTempDataSet.Attributes.GetFormattedValue("Constituent") = aCons AndAlso _
                                   lVar.Seasons IsNot Nothing Then
                                    If lVar.Seasons.Name.Substring(lVar.Seasons.Name.IndexOf("-") + 2) = "Traditional" Then
                                        Dim lData As atcTimeseries = lTempDataSet
                                        lData = lVar.Seasons.SplitBySelected(lData, Nothing).Item(0)
                                        lValue = lData.Attributes.GetFormattedValue(aTrans)
                                        Exit For
                                    End If
                                End If
                            Next
                            'Dim lTriplet(3) As Integer
                            'Select Case aMonthList
                            '    Case "0"
                            '        lTriplet(0) = 0 ' Jan
                            '        lTriplet(1) = 1 ' feb
                            '        lTriplet(2) = 2 ' mar
                            '    Case "1"
                            '        lTriplet(0) = 3 ' Apr
                            '        lTriplet(1) = 4 ' May
                            '        lTriplet(2) = 5 ' June
                            '    Case "2"
                            '        lTriplet(0) = 6 ' July
                            '        lTriplet(1) = 7 ' August
                            '        lTriplet(2) = 8 ' September
                            '    Case "3"
                            '        lTriplet(0) = 9 ' Oct
                            '        lTriplet(1) = 10 ' Nov
                            '        lTriplet(2) = 11 ' Dec
                            'End Select
                            'For li As Integer = 0 To 2
                            '    lValue += lCalculatedAttributes(lTriplet(li)).Value
                            'Next
                            'If aTrans = "Mean" Then
                            '    lValue /= 3
                            'End If
                        End If
                        lString &= vbTab & DecimalAlign(lValue) ' if there is not month list, then doesnot append a value
                    End If
                End If
            End If
        Catch ex As Exception
            Logger.Msg("AnnualAndSeasonalValues Error: " & ex.ToString)
        End Try
        Return lString
    End Function
End Module
