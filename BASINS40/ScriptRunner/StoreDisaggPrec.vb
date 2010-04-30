Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
'Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
'Imports BASINS

Imports atcUtility
Imports atcData
'Imports atcDataTree
'Imports atcEvents

Public Module StoreDisaggPrec
    Private Const pStationPath As String = "C:\BasinsMet\Stations\"
    Private Const pInputPath As String = "C:\BASINSMet\DisaggPrec\"
    Private Const pOutputPath As String = "C:\BASINSMet\WDMFinal\"
    Private Const pMinNumHrly As Integer = 43830 '5 years of hourly values
    Private Const pAlreadyDone As String = "" '01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,50,51" ',66,67,ak,al,ar,az,ca,co,ct,de,fl,ga,hi,ia,id,il,in,ks,ky,la,ma,md,me,mi,mn,mo,ms,mt,nc,nd,ne,nh,nj,nm,nv,ny"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("StoreDisaggPrec:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lts As atcTimeseries
        Dim i As Integer
        Dim lLat As Double
        Dim lLng As Double
        Dim lStaName As String

        Dim lExistingWDM As String
        Dim lDBF As atcTableDBF = Nothing
        Dim lStation As String
        Dim lStatePath As String = ""
        Dim lNewLocn As String = ""
        Dim lWroteCount As Integer = 0
        Dim lPrimeCount As Integer = 0
        Dim lSecondCount As Integer = 0

        Dim lStates() As String = {"AL", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "ID", _
                                   "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", _
                                   "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", _
                                   "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", _
                                   "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "AK", "HI", "PR", "VI"}
        Dim lState As String = ""
        Dim lStIds As New atcCollection
        i = 1
        For Each lState In lStates
            If i < 10 Then
                lStIds.Add(lState, "0" & CStr(i))
            Else
                lStIds.Add(lState, CStr(i))
                If i = 48 Then i += 1 'skip unused 49
                If i = 51 Then i = 65 'skip to Puerto Rico (66)
            End If
            i += 1
        Next

        Dim lStationDBF As New atcTableDBF
        If lStationDBF.OpenFile(pStationPath & "StationLocs.dbf") Then
            Logger.Dbg("StoreDisaggPrec: Opened Station Location Master file " & pStationPath & "StationLocs.dbf")
        Else
            Logger.Dbg("StoreDisaggPrec: PROBLEM Opening Station Location Master file " & pStationPath & "StationLocs.dbf")
        End If

        Logger.Dbg("StoreDisaggPrec: Get all files in data directory " & pInputPath)
        Dim lFiles As NameValueCollection = Nothing
        AddFilesInDir(lFiles, pInputPath, True, "*.wdm")
        Logger.Dbg("StoreDisaggPrec: Found " & lFiles.Count & " data files")

        For Each lFile As String In lFiles
            Logger.Dbg("StoreDisaggPrec: Opening data file - " & lFile)
            lStatePath = Right(PathNameOnly(lFile), 2)
            If Not pAlreadyDone.Contains(lStatePath.Substring(0, 2)) Then
                Dim lWDMfile As New atcWDM.atcDataSourceWDM
                lWDMfile.Open(lFile)
                lStation = Left(FilenameNoExt(FilenameNoPath(lFile)), 6)
                If IsNumeric(lStatePath) Then
                    lState = lStIds.Keys(lStIds.IndexOf(lStatePath))
                Else 'just use state path for start of new location
                    lState = lStatePath
                End If
                lNewLocn = lState.ToUpper & lStation
                Logger.Dbg("StoreDisaggPrec: For station " & lStation)
                lts = Nothing
                lts = lWDMfile.DataSets(0)
                If Not lts Is Nothing AndAlso lts.numValues > pMinNumHrly Then
                    If lStationDBF.FindFirst(1, lStation) Then
                        lLat = lStationDBF.Value(4)
                        lLng = lStationDBF.Value(5)
                        lStaName = lStationDBF.Value(2)
                        lExistingWDM = pOutputPath & lNewLocn & ".wdm"
                        If FileExists(lExistingWDM) Then
                            Dim lExistingWDMFile As New atcWDM.atcDataSourceWDM
                            lExistingWDMFile.Open(lExistingWDM)
                            If lExistingWDMFile.DataSets.Keys.Contains(1) Then
                                'already have hourly precip in place on final WDM, store disagg on DSN 10
                                lts.Attributes.SetValue("ID", 10)
                                lSecondCount += 1
                            Else 'final WDM has no hourly precip yet, store in DSN 1
                                lts.Attributes.SetValue("ID", 1)
                                lPrimeCount += 1
                            End If
                            lts.Attributes.SetValue("LatDeg", lLat)
                            lts.Attributes.SetValue("LngDeg", lLng)
                            lts.Attributes.SetValue("STANAM", lStaName)
                            lts.Attributes.SetValue("Constituent", "PREC")
                            lts.Attributes.SetValue("TSTYPE", "PREC")
                            lts.Attributes.SetValue("Scenario", "COMPUTED")
                            lts.Attributes.SetValue("Location", lNewLocn)
                            If lExistingWDMFile.AddDataset(lts, atcDataSource.EnumExistAction.ExistReplace) Then
                                Logger.Dbg("StoreDisaggPrec: Wrote " & lts.ToString & " to WDM file")
                                lWroteCount += 1
                            Else
                                Logger.Dbg("StoreDisaggPrec: PROBLEM writing " & lts.ToString & " to WDM file")
                            End If

                        End If
                    Else
                        Logger.Dbg("StoreDisaggPrec: PROBLEM - couldn't find station on station file!")
                    End If
                Else
                    Logger.Dbg("StoreDisaggPrec: Not enough values to warrant saving data")
                End If
            End If
        Next
        Logger.Dbg("StoreDisaggPrec: Completed storing Disaggregated Precip on WDM files")
        Logger.Dbg("StoreDisaggPrec:   Wrote " & lWroteCount & " timeseries")
        Logger.Dbg("StoreDisaggPrec:   Wrote " & lPrimeCount & " timeseries to primary Precip dataset (DSN 1)")
        Logger.Dbg("StoreDisaggPrec:   Wrote " & lSecondCount & " timeseries to secondary Precip dataset (DSN 10)")

        'Application.Exit()

    End Sub

End Module
