Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports System.Math
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcWDM

Public Module FillMissingMPCAPrecip
    Private Const pInputPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pStationPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pOutputPath As String = "D:\BasinsMet\MPCA\Combined\"
    Private Const pMaxNearStas As Integer = 10
    Private Const pMaxFillLength As Integer = 11 'any span < max time shift (10 hrs for HI)
    Private Const pMinNumHrly As Integer = 43830 '5 years of hourly values
    Private Const pMinNumDly As Integer = 1830 '5 years of daily
    Private Const pMaxPctMiss As Integer = 50 '20
    Declare Sub F90_MSG Lib "hass_ent.dll" _
        (ByVal aMsg As String, ByVal aMsgLen As Short)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.StartToFile("FillMissingMPCA.log")
        Logger.Dbg("FillMissing:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationFile As New atcTableDelimited
        If lStationFile.OpenFile(pStationPath & "MetStationTable.csv") Then
            Logger.Dbg("FillMissing: Opened Station Location Master file")
        End If

        Dim X2(lStationFile.NumRecords) As Double
        Dim Y2(lStationFile.NumRecords) As Double
        Dim lDist(lStationFile.NumRecords) As Double
        Dim lPos(lStationFile.NumRecords) As Integer
        Dim lRank(lStationFile.NumRecords) As Integer

        Logger.Dbg("FillMissing: Read all lat/lng values")
        lStationFile.CurrentRecord = 1
        While Not lStationFile.EOF
            X2(lStationFile.CurrentRecord) = lStationFile.Value(2)
            Y2(lStationFile.CurrentRecord) = lStationFile.Value(1)
            lStationFile.CurrentRecord += 1
        End While

        Dim lFillers As atcCollection = Nothing
        Dim lFillerOTs As atcCollection = Nothing

        Dim lMVal As Double = -999.0
        Dim lMAcc As Double = -998.0
        Dim lFMin As Double = -100.0
        Dim lFMax As Double = 10000.0
        Dim lRepType As Integer = 1 'DBF parsing output format

        Dim lCurWDM As String = pOutputPath & "MNRawMet.wdm"
        Dim lWDMfile As New atcWDM.atcDataSourceWDM
        lWDMfile.Open(lCurWDM)

        Dim lBasinsMetWDM As String = pOutputPath & "MN_Met.wdm"
        Dim lBasinsMetWDMfile As New atcWDM.atcDataSourceWDM
        lBasinsMetWDMfile.Open(lBasinsMetWDM)

        Dim lNewWDM As String = pOutputPath & "MNFilledMet.wdm"
        Dim lNewWDMfile As New atcWDM.atcDataSourceWDM
        lNewWDMfile.Open(lNewWDM)

        Dim lAddMe As Boolean = True
        Dim lCons As String = ""
        Dim lStr As String = ""
        Dim lPctMiss As Double
        Dim i As Integer
        Dim j As Integer
        Dim X1 As Double
        Dim Y1 As Double
        Dim lStation As String = ""
        Dim lStaFill As String = ""
        Dim lFillTsers As atcCollection
        Dim lFillTS As atcTimeseries = Nothing
        For Each lts As atcTimeseries In lWDMfile.DataSets
            lAddMe = False
            lCons = lts.Attributes.GetValue("Constituent")
            If lCons = "PRCP" Then
                lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                If lPctMiss < pMaxPctMiss Then '% missing OK
                    If (lts.Attributes.GetValue("tu") = atcTimeUnit.TUHour AndAlso lts.numValues > pMinNumHrly) OrElse _
                       (lts.Attributes.GetValue("tu") = atcTimeUnit.TUDay AndAlso lts.numValues > pMinNumDly) Then 'want a significant time span
                        ExtendISHTSer(lts)
                        Logger.Dbg("FillMissing:  Filling data for " & lts.ToString & ", " & lts.Attributes.GetValue("Description"))
                        lAddMe = True
                    Else
                        Logger.Dbg("FillMissing:  Not enough values (" & lts.numValues & ") for " & lts.ToString & _
                                   " - need at least " & pMinNumHrly)
                    End If
                Else
                    Logger.Dbg("FillMissing:  For " & lts.ToString & ", percent Missing (" & lPctMiss & ") too large (> " & pMaxPctMiss & ")")
                End If
            End If
            If lAddMe Then
                If lPctMiss > 0 Then
                    lStation = lts.Attributes.GetValue("Stanam")
                    If lStationFile.FindFirst(3, lStation) Then
                        X1 = lStationFile.Value(2)
                        Y1 = lStationFile.Value(1)
                        Logger.Dbg("FillMissing: For Station " & lStation & ",  at Lat/Lng " & lStationFile.Value(2) & " / " & lStationFile.Value(1))
                        For i = 1 To lStationFile.NumRecords - 1
                            lDist(i) = System.Math.Sqrt((X1 - X2(i)) ^ 2 + (Y1 - Y2(i)) ^ 2)
                        Next
                        SortRealArray(0, lStationFile.NumRecords - 1, lDist, lPos)
                        Logger.Dbg("FillMissing: Sorted stations by distance")
                    End If
                    Logger.Dbg("FillMissing:    Nearby Stations:")
                    lFillers = New atcCollection

                    i = 2
                    j = 0
                    While j < pMaxNearStas AndAlso i < lStationFile.NumRecords
                        'look through stations, in order of proximity, that can be used to fill
                        lStationFile.CurrentRecord = lPos(i)
                        lStaFill = lStationFile.Value(3)
                        lFillTsers = FindFillTSers(lts, lCons, lStaFill, lWDMfile)
                        If lFillTsers.Count = 0 Then
                            lFillTsers = FindFillTSers(lts, "PREC", lStaFill, lBasinsMetWDMfile)
                        End If
                        For k As Integer = 0 To lFillTsers.Count - 1
                            lFillTS = lFillTsers(k)
                            If Not lFillTS Is Nothing Then
                                'contains data for time period being filled
                                lFillers.Add(lDist(lPos(i)), lFillTS)
                                j += 1
                                Logger.Dbg("FillMissing:  Using " & _
                                           lFillTS.Attributes.GetValue("Constituent") & " from " & _
                                           lFillTS.Attributes.GetValue("Location") & " " & _
                                           lFillTS.Attributes.GetValue("STANAM") & " at Lat/Lng " & _
                                           lStationFile.Value(2) & "/" & lStationFile.Value(1))
                            End If
                        Next
                        i += 1
                    End While

                    If j > 0 Then
                        Logger.Dbg("FillMissing:  Found " & j & " nearby stations for filling")
                        Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                        If lts.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then
                            If lPctMiss > 0 Then
                                FillHourlyTser(lts, lFillers, lMVal, lMAcc, 90)
                            Else
                                Logger.Dbg("FillMissing:  All Missing periods filled via interpolation")
                            End If
                        Else 'daily tser
                            FillDailyTser(lts, Nothing, lFillers, Nothing, lMVal, lMAcc, 90)
                        End If
                        lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                        lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                        Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
                    Else
                        Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
                    End If
                End If
            Else
                Logger.Dbg("FillMissing:  Not going to try to fill this dataset!")
            End If
            'write filled data set to new WDM file
            If lAddMe Then
                If lNewWDMfile.AddDataset(lts) Then
                    Logger.Dbg("FillMissing:  Added " & lCons & " dataset to WDM file for station " & lStation)
                Else
                    Logger.Dbg("FillMissing:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
                End If
            End If
        Next

        Logger.StartToFile("FillMissEnd.log", , , True)
        Logger.Dbg("FillMissing:Completed Filling")
    End Sub

    Private Function FindFillTSers(ByVal aCurTS As atcTimeseries, ByVal aCons As String, ByVal aLocn As String, ByVal aWDMFile As atcWDM.atcDataSourceWDM) As atcCollection
        Dim lSJD As Double = aCurTS.Attributes.GetValue("SJDay")
        Dim lEJD As Double = aCurTS.Attributes.GetValue("EJDay")
        Dim lCons As String = ""
        Dim lLocn As String = ""
        Dim lChkDates As Boolean = False
        Dim lTSers As New atcCollection

        For Each lts As atcTimeseries In aWDMFile.DataSets
            lChkDates = False
            lCons = lts.Attributes.GetValue("Constituent")
            If aLocn.Length <= 8 Then
                lLocn = lts.Attributes.GetValue("Location")
            Else
                lLocn = lts.Attributes.GetValue("StaNam")
            End If
            If lCons = aCons And lLocn = aLocn Then
                lChkDates = True
            End If
            If lChkDates Then 'got right constituent and location, check quality (UBC200) and period of record
                If lts.Attributes.GetValue("SJDay") < lEJD AndAlso _
                   lts.Attributes.GetValue("EJDay") > lSJD Then 'some portion falls within filling period
                    lTSers.Add(lts)
                End If
            End If
        Next
        Return lTSers
    End Function

    Private Sub ExtendISHTSer(ByRef aTSer As atcTimeseries)
        Dim lEJDay As Double = aTSer.Attributes.GetValue("EJDay")
        If lEJDay > 0 Then
            Dim lNewEJDay As Double = System.Math.Round(lEJDay)
            If lNewEJDay > lEJDay Then
                Dim lNumOldVals As Integer = aTSer.numValues
                Dim lNumNewVals As Integer = System.Math.Round((lNewEJDay - lEJDay) * 24)
                aTSer.numValues = lNumOldVals + lNumNewVals
                For i As Integer = 1 To lNumNewVals
                    aTSer.Dates.Values(lNumOldVals + i) = aTSer.Dates.Values(lNumOldVals) + i / 24
                    aTSer.Values(lNumOldVals + i) = Double.NaN 'aTSer.Attributes.GetValue("TSFILL")
                Next
                aTSer.Attributes.DiscardCalculated()
            End If
        End If
    End Sub

End Module
