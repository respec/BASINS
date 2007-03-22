Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptStatTest
    Private Const pTestPath As String = "C:\test\WaterBalance\"
    Private Const pBalanceType As String = "Water"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lScenario As String = "base"

        Dim lConstituents2Output As New atcCollection
        Select Case pBalanceType
            Case "Water"
                lConstituents2Output.Add("SUPY", "Rainfall")
                lConstituents2Output.Add("Header1", "Runoff")
                lConstituents2Output.Add("SURO", "    Surface")
                lConstituents2Output.Add("IFWO", "    Interflow")
                lConstituents2Output.Add("AGWO", "    Baseflow")
                lConstituents2Output.Add("PERO", "    Total")
                lConstituents2Output.Add("IGWI", "Deep Groundwater")
                lConstituents2Output.Add("Header2", "Evaporation")
                lConstituents2Output.Add("PET", "    Potential")
                lConstituents2Output.Add("CEPE", "    Intercep St")
                lConstituents2Output.Add("UZET", "    Upper Zone")
                lConstituents2Output.Add("LZET", "    Lower Zone")
                lConstituents2Output.Add("AGWET", "    Ground Water")
                lConstituents2Output.Add("BASET", "    Baseflow")
                lConstituents2Output.Add("TAET", "    Total")
        End Select

        Dim lString As New Text.StringBuilder
        lString.AppendLine(pBalanceType & " Balance Report For " & lScenario)

        Dim lArgs As Object() = Nothing
        Dim lErr As String = ""
        Dim lDataManager As New atcDataManager(aMapWin) ' = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
        Dim lHspfBinFileName As String = "Input\" & lScenario & ".hbn"
        Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        lString.AppendLine("   Run Made " & lFileDetails.CreationTime & vbCrLf)

        Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
        lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
        Logger.Dbg(" DataSetCount " & lHspfBinFile.Datasets.Count)

        Dim lLocations As atcCollection = lHspfBinFile.DataSets.SortedAttributeValues("Location")
        'Logger.Dbg(" LocationCount " & lLocations.ToString(lLocations.Count))

        Dim lConstituents As atcCollection = lHspfBinFile.DataSets.SortedAttributeValues("Constituent")
        Logger.Dbg(" ConstituentCount " & lConstituents.ToString(lConstituents.Count))

        Dim lMatchConstituentGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet

        For Each lLocation As String In lLocations
            If lLocation.StartsWith("P:") Then
                Logger.Dbg("   Perlnd " & lLocation)
                lString.AppendLine(pBalanceType & " Balance Report For " & lLocation)
                Dim lTempDataGroup As atcDataGroup = lHspfBinFile.Datasets.FindData("Location", lLocation)
                Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                Dim lNeedHeader As Boolean = True

                For lIndex As Integer = 0 To lConstituents2Output.Count - 1
                    Dim lConstituentKey As String = lConstituents2Output.Keys(lIndex)
                    Dim lConstituentName As String = lConstituents2Output(lIndex)
                    lMatchConstituentGroup = lTempDataGroup.FindData("Constituent", lConstituentKey)
                    If lMatchConstituentGroup.Count > 0 Then
                        lTempDataSet = lMatchConstituentGroup.Item(0)
                        Logger.Dbg("       Match " & lConstituentKey & " with " & lTempDataSet.ToString)

                        Dim lSeasons As New atcSeasons.atcSeasonsCalendarYear
                        Dim lSeasonalAttributes As New atcDataAttributes
                        Dim lCalculatedAttributes As New atcDataAttributes
                        lSeasonalAttributes.SetValue("Mean", 0)
                        lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)

                        If lNeedHeader Then
                            lString.Append("Date" & vbTab & "Mean")
                            For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                                lString.Append(vbTab & lAttribute.Definition.Name)
                            Next
                            lString.AppendLine()
                            lNeedHeader = False
                        End If

                        lString.Append(lConstituentName & vbTab & DF(lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value))
                        For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                            lString.Append(vbTab & DF(lAttribute.Value))
                        Next
                        lString.AppendLine()
                    Else
                        lString.AppendLine(lConstituentName)
                    End If

                Next
                lTempDataGroup = Nothing
                lString.AppendLine(vbCrLf)
            Else
                'Logger.Dbg("   SKIP " & lLocation)
            End If
        Next lLocation
        Dim lOutFileName As String = lScenario & "_" & pBalanceType & "_" & "Balance.txt"
        Logger.Dbg("  WriteReportTo " & lOutFileName)
        SaveFileString(lOutFileName, lString.ToString)

    End Sub
    Private Function DF(ByVal aValue As Double, Optional ByVal aDecimalPlaces As Integer = 3) As String
        Return Trim(Format(aValue, "##########0." & StrDup(aDecimalPlaces, "0")))
    End Function
End Module
