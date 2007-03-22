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
                lConstituents2Output.Add("SUPY")
                lConstituents2Output.Add("PERO")
                lConstituents2Output.Add("IFWO")
                lConstituents2Output.Add("AGWO")
                lConstituents2Output.Add("IGWI")
                lConstituents2Output.Add("PET")
                lConstituents2Output.Add("CEPE")
                lConstituents2Output.Add("SURET")
                lConstituents2Output.Add("UZET")
                lConstituents2Output.Add("LZET")
                lConstituents2Output.Add("AGWET")
                lConstituents2Output.Add("BASET")
                lConstituents2Output.Add("TAET")
        End Select

        Dim lString As New Text.StringBuilder
        lString.AppendLine(pBalanceType & " Balance Report For " & lScenario)

        Dim lArgs As Object() = Nothing
        Dim lErr As String = ""
        Dim lDataManager As New atcDataManager(aMapWin) ' = Scripting.Run("vb", "", "subFindDataManager.vb", lErr, False, aMapWin, aMapWin)

        Dim lHspfBinFile As atcDataSource = New atcHspfBinOut.atcTimeseriesFileHspfBinOut
        Dim lHspfBinFileName As String = "Input\" & lScenario & ".hbn"
        Logger.Dbg(" AboutToOpen " & lHspfBinFileName)
        lDataManager.OpenDataSource(lHspfBinFile, lHspfBinFileName, Nothing)
        Logger.Dbg(" DataSetCount " & lHspfBinFile.Datasets.Count)
        Dim lFileDetails As System.IO.FileInfo = New System.IO.FileInfo(lHspfBinFileName)
        lString.AppendLine("   Run Made " & lFileDetails.CreationTime & vbcrlf)

        Dim lLocations As atcCollection = lHspfBinFile.DataSets.SortedAttributeValues("Location")
        'Logger.Dbg(" LocationCount " & lLocations.ToString(lLocations.Count))

        Dim lConstituents As atcCollection = lHspfBinFile.DataSets.SortedAttributeValues("Constituent")
        'Logger.Dbg(" ConstituentCount " & lConstituents.ToString(lConstituents.Count))

        For Each lLocation As String In lLocations
            If lLocation.StartsWith("P:") Then
                Logger.Dbg("   Perlnd " & lLocation)
                lString.AppendLine(pBalanceType & " Balance Report For " & lLocation)
                Dim lTempDataGroup As atcDataGroup = lHspfBinFile.Datasets.FindData("Location", lLocation)
                Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                For Each lConstituent as String In lConstituents2Output
                    Dim lTempDataSet As atcDataSet = lTempDataGroup.FindData("Constituent", lConstituent).Item(0)
                    Logger.Dbg("       Match " & lConstituent & " with " & lTempDataSet.ToString)
                    lString.AppendLine(Space(6) & lConstituent & "  " & lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value)
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
End Module
