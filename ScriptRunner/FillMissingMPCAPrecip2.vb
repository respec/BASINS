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
    Private Const pDataPath As String = "C:\Temp\"
    Private Const pMaxNearStas As Integer = 10
    Private Const pMaxFillLength As Integer = 11 'any span < max time shift (10 hrs for HI)
    Private Const pMinNumHrly As Integer = 43830 '5 years of hourly values
    Private Const pMinNumDly As Integer = 1830 '5 years of daily
    Private Const pMaxPctMiss As Integer = 50 '20

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Try
            Dim lCurWDM As String = pDataPath & "precip-2014-11-03.wdm"
            Logger.StartToFile(IO.Path.ChangeExtension(lCurWDM, ".log"), , , True)
            Logger.Dbg("FillMissing:Start")
            ChDriveDir(pDataPath)
            Logger.Dbg(" CurDir:" & CurDir())

            Dim lFillers As atcCollection = Nothing
            Dim lFillerOTs As atcCollection = Nothing

            Dim lMVal As Double = -999.0
            Dim lMAcc As Double = -998.0
            Dim lFMin As Double = -100.0
            Dim lFMax As Double = 10000.0
            Dim lRepType As Integer = 1 'DBF parsing output format

            Dim lWDMfile As New atcWDM.atcDataSourceWDM
            lWDMfile.Open(lCurWDM)

            Dim lNewWDM As String = atcUtility.GetTemporaryFileName(IO.Path.ChangeExtension(lCurWDM, ".Filled"), ".wdm")
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

            Dim lStationLocations As New atcCollection
            For Each lts As atcTimeseries In lWDMfile.DataSets
                Dim lLocation As String = lts.Attributes.GetValue("Location")
                Logger.Dbg(lLocation)
                If Not lStationLocations.Keys.Contains(lLocation) Then
                    lStationLocations.Add(lLocation, lts.Attributes)
                End If
            Next

            For Each lts As atcTimeseries In lWDMfile.DataSets
                'If lts.Attributes.GetValue("Location") <> "PREC0001" Then Continue For
                Dim lFilledTS As atcTimeseries = Nothing
                lAddMe = False
                lCons = lts.Attributes.GetValue("Constituent")
                If lCons = "PREC" Then
                    lStr = MissingDataSummary(lts, lMVal, lMAcc, lFMin, lFMax, lRepType)
                    lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                    If lPctMiss < pMaxPctMiss Then '% missing OK
                        If (lts.Attributes.GetValue("tu") = atcTimeUnit.TUHour AndAlso lts.numValues > pMinNumHrly) OrElse _
                           (lts.Attributes.GetValue("tu") = atcTimeUnit.TUDay AndAlso lts.numValues > pMinNumDly) Then 'want a significant time span
                            'ExtendISHTSer(lts)
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
                        lStation = lts.Attributes.GetValue("Location")

                        X1 = lts.Attributes.GetValue("Longitude")
                        Y1 = lts.Attributes.GetValue("Latitude")
                        Logger.Dbg("FillMissing: For Station " & lStation & ",  at Lat/Lng " & Y1 & " / " & X1)

                        Dim lOtherDataSets As New atcCollection ' SortedList(Of Double, atcDataAttributes)
                        For lStationIndex As Integer = 0 To lStationLocations.Count - 1
                            If lStationLocations.Keys(lStationIndex) <> lStation Then
                                Dim lOtherAtt As atcDataAttributes = lStationLocations.ItemByIndex(lStationIndex)
                                Dim lDistance As Double = System.Math.Sqrt((X1 - lOtherAtt.GetValue("Longitude")) ^ 2 + _
                                                                           (Y1 - lOtherAtt.GetValue("Latitude")) ^ 2)
                                While lOtherDataSets.Keys.Contains(lDistance)
                                    lDistance += 0.00001
                                End While
                                lOtherDataSets.Add(lDistance, lStationLocations.ItemByIndex(lStationIndex))
                            End If
                        Next
                        lOtherDataSets.Sort()
                        Logger.Dbg("FillMissing: Sorted stations by distance")
                        Logger.Dbg("FillMissing:    Nearby Stations:")
                        lFillers = New atcCollection

                        j = 0
                        For i = 0 To lOtherDataSets.Count - 1
                            If j < pMaxNearStas Then
                                'look through stations, in order of proximity, that can be used to fill
                                Dim lStationAttribues As atcDataAttributes = lOtherDataSets.ItemByIndex(i)
                                lStaFill = lStationAttribues.GetValue("Location")
                                lFillTsers = FindFillTSers(lts, lCons, lStaFill, lWDMfile)
                                'If lFillTsers.Count = 0 Then
                                '    lFillTsers = FindFillTSers(lts, "PREC", lStaFill, lBasinsMetWDMfile)
                                'End If
                                For k As Integer = 0 To lFillTsers.Count - 1
                                    lFillTS = lFillTsers(k)
                                    If lFillTS IsNot Nothing Then
                                        'contains data for time period being filled
                                        Dim lDistance As Double = lOtherDataSets.Keys(i)
                                        While lFillers.Keys.Contains(lDistance)
                                            lDistance += 0.00001
                                        End While

                                        lFillers.Add(lDistance, lFillTS)
                                        j += 1
                                        Logger.Dbg("FillMissing:  Using " & _
                                                   lFillTS.Attributes.GetValue("Constituent") & " from " & _
                                                   lFillTS.Attributes.GetValue("Location") & " at Lat/Lng " & _
                                                   lFillTS.Attributes.GetValue("Latitude") & "/" & _
                                                   lFillTS.Attributes.GetValue("Longitude") & " dist " & lOtherDataSets.Keys(i))
                                    End If
                                Next
                                i += 1
                            End If
                        Next

                        If j > 0 Then
                            Logger.Dbg("FillMissing:  Found " & j & " nearby stations for filling")
                            Logger.Dbg("FillMissing:  Before Filling, % Missing:  " & lPctMiss)
                            lFilledTS = lts.Clone()
                            If lts.Attributes.GetValue("TU") = atcTimeUnit.TUHour Then
                                If lPctMiss > 0 Then
                                    FillHourlyTser(lFilledTS, lFillers, lMVal, lMAcc, 90)
                                Else
                                    Logger.Dbg("FillMissing:  All Missing periods filled via interpolation")
                                End If
                            Else 'daily tser
                                FillDailyTser(lFilledTS, Nothing, lFillers, Nothing, lMVal, lMAcc, 90)
                            End If
                            lStr = MissingDataSummary(lFilledTS, lMVal, lMAcc, lFMin, lFMax, lRepType)
                            lPctMiss = CDbl(lStr.Substring(lStr.LastIndexOf(",") + 1))
                            Logger.Dbg("FillMissing:  After Filling, % Missing:  " & lPctMiss)
                        Else
                            Logger.Dbg("FillMissing:  PROBLEM - Could not find any nearby stations for filling")
                        End If
                    End If
                Else
                    Logger.Dbg("FillMissing:  Not going to try to fill this dataset from nearby stations.")
                End If
                'write filled data set to new WDM file
                If lFilledTS IsNot Nothing Then
                    If lNewWDMfile.AddDataset(lFilledTS) Then
                        Logger.Dbg("FillMissing:  Added " & lCons & " dataset to WDM file for station " & lStation)
                    Else
                        Logger.Dbg("FillMissing:  PROBLEM adding " & lCons & " dataset to WDM file for station " & lStation)
                    End If
                    lFilledTS.Clear()
                End If
                lts.ValuesNeedToBeRead = True
            Next
            Logger.Msg(lNewWDM, MsgBoxStyle.OkOnly, "FillMissing:Completed Filling")
        Catch ex As Exception
            Logger.Msg(ex.ToString, MsgBoxStyle.Critical, "FillMissing:Exception")
        End Try
    End Sub

    Private Function FindFillTSers(ByVal aCurTS As atcTimeseries, _
                                   ByVal aCons As String, _
                                   ByVal aLocn As String, _
                                   ByVal aWDMFile As atcWDM.atcDataSourceWDM) As atcCollection
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

End Module
