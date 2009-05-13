Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcMetCmp
'Imports atcDataTree
'Imports atcEvents

Public Module GenPenmanMonteithET
    Private Const pOutputPath As String = "C:\TT_GCRP\WDM_Data\"
    Private Const pStationPath As String = "C:\BASINSMet\Stations\"
    Private pSWATStnFile As String = IO.Path.Combine(pOutputPath, "statwgn.txt")

    'subroutine pmpevt(idmet,istyrZ,istdyZ,nbyrZ,sub_elevZ,sub_latZ,
    '&                 CO2Z,numdata,PrecipZ,TmaxZ,TminZ,PevtPMZ)
    'The last four are 1-d array of real
    Private Declare Sub PMPEVT Lib "tt_met.dll" (ByRef idmet As Integer, _
                                                ByRef istyrZ As Integer, _
                                                ByRef istdyZ As Integer, _
                                                ByRef nbyrZ As Integer, _
                                                ByRef sub_elevZ As Single, _
                                                ByRef sub_latZ As Single, _
                                                ByRef CO2Z As Single, _
                                                ByRef numdata As Integer, _
                                                ByRef PrecipZ As Single, _
                                                ByRef TmaxZ As Single, _
                                                ByRef TminZ As Single, _
                                                ByRef PevtPMZ As Single)

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GenPenmanMonteith:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lStationDBF As New atcTableDBF
        Dim lSJD As Double
        Dim lEJD As Double
        Dim ltsPrec As atcTimeseries
        Dim ltsAtem As atcTimeseries
        Dim ltsAtemSub As atcTimeseries
        Dim ltsPMET As atcTimeseries = Nothing
        Dim lWDMFile As atcWDM.atcDataSourceWDM = Nothing
        Dim lPrecVals As Double() = Nothing
        Dim lTMinVals As Double() = Nothing
        Dim lTMaxVals As Double() = Nothing
        Dim lPMETValsHdl() As Double = Nothing
        Dim lswatStnKey As String = String.Empty

        Dim ltsTemp As atcTimeseries = Nothing

        'For report
        Dim lAtemVals() As Double = Nothing
        Dim lPrecValsMonthly() As Double = Nothing
        Dim lPrecValsYearly() As Double = Nothing
        Dim lAtemValsMonthly() As Double = Nothing
        Dim lAtemValsYearly() As Double = Nothing
        Dim ltsPEVT As atcTimeseries = Nothing
        Dim lPEVTVals() As Double = Nothing
        Dim lPEVTValsMonthly() As Double = Nothing
        Dim lPEVTValsYearly() As Double = Nothing
        Dim lPMETValsMonthly() As Double = Nothing
        Dim lPMETValsYearly() As Double = Nothing
        Dim lsw As System.IO.StreamWriter
        'lsw = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "PMETReport.txt"), False)
        lsw = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "test1.txt"), False)
        Dim ldoneReport As Boolean = False

        Dim line As String = String.Empty
        Dim lSJDText As String = String.Empty
        Dim lEJDText As String = String.Empty
        Dim ldailyDates() As Double = Nothing
        Dim lmonthDates() As Double = Nothing
        Dim lyearDates() As Double = Nothing
        'For report


        Dim lNVals As Integer
        Dim lDate(5) As Integer
        Dim lJDay As Integer
        Dim lLat As Double
        Dim lElev As Double
        Dim lNYrs As Integer
        Dim lCO2 As Double

        Dim lnumPairs As Integer
        Dim lPTPairList As Dictionary(Of String, String)

        Dim lCOOPIDs() As String = { _
        "OR350595", _
        "OR351735", _
        "OR352325", _
        "OR352348", _
        "OR352493", _
        "OR352693", _
        "OR352997", _
        "OR353908", _
        "OR355384", _
        "OR356151", _
        "OR356334", _
        "OR356749", _
        "OR356751", _
        "OR357127", _
        "OR357572", _
        "OR358466"}

        Dim lAllStnsList As Dictionary(Of String, String) = Nothing
        lAllStnsList = New Dictionary(Of String, String)
        Dim lsr As System.IO.StreamReader = Nothing
        lsr = New System.IO.StreamReader(IO.Path.Combine(pOutputPath, "AllStns.txt"))
        Dim lkey As String = String.Empty
        Dim larr() As String = Nothing
        While Not lsr.EndOfStream
            line = lsr.ReadLine()
            larr = line.Split(" ")
            'lkey = larr(0) & " " & larr(3) ' e.g. AL 1
            lkey = larr(3) ' e.g. 249, but as a string
            lAllStnsList.Add(lkey, line)
            larr = Nothing
        End While
        lsr.Close()

        Dim lFName As String = pStationPath & "StationLocs.dbf"
        If lStationDBF.OpenFile(lFName) Then
            Logger.Dbg("GenPET: Opened Station Location file " & lFName)
        End If

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("GenPenmanMonteith: Found " & lFiles.Count & " WDM data files")
        lsr = Nothing 'reuse this reader for the individual stnlist file below
        Dim lkupFile As String = String.Empty
        For Each lFile As String In lFiles

            'The current set up requires one StationList.txt be placed in the same folder as each wdm as look up guide
            'as wdm holds pretty much all stations data, this should be manageable
            lkupFile = IO.Path.Combine(IO.Path.GetDirectoryName(lFile), "StationList.txt")
            'if a stationlist.txt is not there with the met.wdm, then skip it
            If Not IO.File.Exists(lkupFile) Then
                Continue For
            End If

            lWDMFile = New atcWDM.atcDataSourceWDM
            lWDMFile.Open(lFile)
            Logger.Dbg("GenPenmanMonteith: Opening data file - " & lFile)

            lPTPairList = New Dictionary(Of String, String)
            'lnumPairs = getRainTempPairs(lPTPairList, lWDMFile)
            lnumPairs = getRainTempPairs(lPTPairList, lWDMFile, lkupFile)
            Logger.Dbg("GenPenmanMonteith: Found matching pair of rain and temp" & lnumPairs)

            For Each lStation As String In lPTPairList.Keys
                Dim ltsPrecID As Integer = Integer.Parse(lPTPairList.Item(lStation).Split(",")(0))
                Dim ltsAtemID As Integer = Integer.Parse(lPTPairList.Item(lStation).Split(",")(1))
                lswatStnKey = String.Empty
                lswatStnKey = lPTPairList.Item(lStation).Split(",")(2)

                Dim ltsPevtID As Integer = 0
                Try
                    ltsPevtID = Integer.Parse(lPTPairList.Item(lStation).Split(",")(3)) ' For report
                Catch ex As Exception
                    ltsPevtID = -9999
                End Try

                ltsPrec = lWDMFile.DataSets.ItemByKey(ltsPrecID)
                ltsAtem = lWDMFile.DataSets.ItemByKey(ltsAtemID)
                If ltsPevtID = -9999 Then
                    ltsPEVT = Nothing
                Else
                    ltsPEVT = lWDMFile.DataSets.ItemByKey(ltsPevtID) ' For report
                End If


                Dim lhasSWATParms As Boolean = False
                'lswatStnKey = String.Empty
                'lswatStnKey = getSWATStnKey(lkupFile, lStation)
                If lswatStnKey <> String.Empty Then
                    lhasSWATParms = True
                End If

                'For Each lCOOPID As String In lCOOPIDs
                '    If lStation = lCOOPID Then
                '        lhasSWATParms = True
                '        Exit For
                '    End If
                'Next

                If Not lhasSWATParms Then Continue For
                If Not setSWATStn(lAllStnsList, lswatStnKey) Then Continue For

                Logger.Dbg("GenPenmanMonteith:   For Station - " & lStation)

                'Find common period for precip/temp
                If ltsPrec.Attributes.GetValue("SJDay") > ltsAtem.Attributes.GetValue("SJDay") Then
                    lSJD = ltsPrec.Attributes.GetValue("SJDay")
                Else
                    lSJD = ltsAtem.Attributes.GetValue("SJDay")
                End If
                If ltsPrec.Attributes.GetValue("EJDay") < ltsAtem.Attributes.GetValue("EJDay") Then
                    lEJD = ltsPrec.Attributes.GetValue("EJDay")
                Else
                    lEJD = ltsAtem.Attributes.GetValue("EJDay")
                End If
                If lSJD < lEJD Then 'common period found
                    Logger.Dbg("GenPenmanMonteith:   Generating ET for period " & DumpDate(lSJD) & " - " & DumpDate(lEJD))
                    lPrecVals = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUDay, 1, atcTran.TranSumDiv).Values
                    ltsAtemSub = SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)
                    lTMinVals = Aggregate(ltsAtemSub, atcTimeUnit.TUDay, 1, atcTran.TranMin).Values
                    lTMaxVals = Aggregate(ltsAtemSub, atcTimeUnit.TUDay, 1, atcTran.TranMax).Values

                    'More TS for report:
                    lSJDText = ltsAtemSub.Attributes.GetFormattedValue("start date")
                    lEJDText = ltsAtemSub.Attributes.GetFormattedValue("end date")

                    lAtemVals = Aggregate((SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)), atcTimeUnit.TUDay, 1, atcTran.TranAverSame).Values
                    lPrecValsMonthly = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv).Values
                    lAtemValsMonthly = Aggregate((SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)), atcTimeUnit.TUMonth, 1, atcTran.TranAverSame).Values
                    lPrecValsYearly = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUYear, 1, atcTran.TranSumDiv).Values
                    lAtemValsYearly = Aggregate((SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)), atcTimeUnit.TUYear, 1, atcTran.TranAverSame).Values

                    ldailyDates = Aggregate((SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)), atcTimeUnit.TUDay, 1, atcTran.TranAverSame).Dates.Values
                    lmonthDates = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv).Dates.Values()
                    lyearDates = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUYear, 1, atcTran.TranSumDiv).Dates.Values()

                    If ltsPEVT IsNot Nothing Then
                        lPEVTVals = Aggregate((SubsetByDate(ltsPEVT, lSJD, lEJD, Nothing)), atcTimeUnit.TUDay, 1, atcTran.TranSumDiv).Values ' For report
                        lPEVTValsMonthly = Aggregate((SubsetByDate(ltsPEVT, lSJD, lEJD, Nothing)), atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv).Values
                        lPEVTValsYearly = Aggregate((SubsetByDate(ltsPEVT, lSJD, lEJD, Nothing)), atcTimeUnit.TUYear, 1, atcTran.TranSumDiv).Values
                    Else
                        lPEVTVals = Nothing ' For report
                        lPEVTValsMonthly = Nothing
                        lPEVTValsYearly = Nothing
                    End If

                    lNVals = lPrecVals.GetUpperBound(0)
                    J2Date(lSJD, lDate)
                    If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                        lDate(1) = 1
                        lDate(2) = 1
                        lJDay = lSJD - Date2J(lDate) + 1
                    Else
                        lJDay = 1
                    End If
                    lNYrs = timdifJ(lSJD, lEJD, atcTimeUnit.TUYear, 1)
                    lStationDBF.FindFirst(1, lStation)
                    lLat = lStationDBF.Value(4) 'in decimal degrees
                    lElev = lStationDBF.Value(6) 'in feet
                    Dim lPMETValsSingle(lNVals) As Single
                    Dim lPrecValsSingle(lNVals) As Single
                    Dim lTMinValsSingle(lNVals) As Single
                    Dim lTMaxValsSingle(lNVals) As Single

                    'Copy values into single array for calling fortran
                    For z As Integer = 0 To lPrecVals.GetUpperBound(0)
                        lPrecValsSingle(z) = lPrecVals(z)
                        lTMinValsSingle(z) = lTMinVals(z)
                        lTMaxValsSingle(z) = lTMaxVals(z)
                    Next

                    'TODO: need a look up mechanism to pairoff station name with SWAT station id
                    '      now, hard-coded to be 767
                    PMPEVT(Integer.Parse(lswatStnKey), lDate(0), lJDay, lNYrs + 1, lElev, lLat, lCO2, lNVals, lPrecValsSingle(1), lTMaxValsSingle(1), lTMinValsSingle(1), lPMETValsSingle(1))
                    ltsPMET = ltsAtemSub.Clone
                    ltsPMET.Attributes.SetValue("Constituent", "PMET")
                    ltsPMET.Attributes.SetValue("Location", lStation)
                    ltsPMET.Attributes.SetValue("ID", ltsPrecID + 8)
                    ltsPMET.Attributes.SetValue("TU", atcTimeUnit.TUDay)
                    ltsPMET.Attributes.SetValue("start date", lSJDText)
                    ltsPMET.Attributes.SetValue("end date", lEJDText)
                    ltsPMET.Attributes.SetValue("description", "Daily SWAT PM ET mm")
                    ltsPMET.Attributes.SetValue("interval", 1.0)
                    ltsPMET.Attributes.SetValue("TSTYPE", "PMET")
                    J2Date(lSJD, lDate)
                    ltsPMET.Attributes.SetValue("TSBYR", lDate(0))
                    ltsPMET.Dates.Values = NewDates(lSJD, lEJD, atcTimeUnit.TUDay, 1)

                    'Copy back from single pet array to double pet array
                    Dim lPMETVals(lPMETValsSingle.GetUpperBound(0)) As Double
                    For z As Integer = 0 To lPMETValsSingle.GetUpperBound(0)
                        lPMETVals(z) = Double.Parse(lPMETValsSingle(z).ToString)
                    Next
                    ltsPMET.Values = lPMETVals

                    If ltsPMET.Attributes.GetDefinedValue("start date").Value > ltsPMET.Attributes.GetDefinedValue("end date").Value Then
                        GoTo PMETTSPROBLEM
                    End If
                    If lWDMFile.AddDataset(ltsPMET, atcDataSource.EnumExistAction.ExistReplace) Then
                        Logger.Dbg("GenPenmanMonteith:   Wrote Penman-Monteith PET to DSN " & ltsPrecID + 8)
                        lPMETValsMonthly = Aggregate(ltsPMET, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv).Values ' For report
                        lPMETValsYearly = Aggregate(ltsPMET, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv).Values  ' For report
                    Else
PMETTSPROBLEM:
                        Logger.Dbg("GenPenmanMonteith:   PROBLEM writing Penman-Monteith PET to DSN " & ltsPrecID + 8)
                    End If
                Else
                    Logger.Dbg("GenPenmanMonteith:   No common period available for Precip and Air Temp data")
                End If

                'Do report for just the Sherburn 3WSW station
                If lStation = "MN217602" Then
                    ldoneReport = False
                Else
                    ldoneReport = True
                End If
                If ldoneReport Then GoTo clearEnd
                lsw.WriteLine("============================================================")
                lsw.WriteLine("==============  " & lStation & " Starts ====================")
                lsw.WriteLine("============================================================")
                lsw.WriteLine("StationID,TimeUnit,Date,PREC,ATEM,PMET,PEVT")

                'Daily first
                Dim ldateStr As String = String.Empty
                For i As Integer = 1 To lPrecVals.GetUpperBound(0)
                    J2Date(ldailyDates(i), lDate)
                    ldateStr = lDate(1) & "/" & lDate(2) & "/" & lDate(0)
                    ldateStr = ldailyDates(i).ToString
                    line = lStation & ",Daily," & ldateStr & "," & lPrecVals(i).ToString & "," & lAtemVals(i).ToString & "," & ltsPMET.Values(i).ToString
                    If lPEVTVals IsNot Nothing Then
                        line &= "," & lPEVTVals(i).ToString
                    End If
                    lsw.WriteLine(line)
                Next
                lsw.Flush()

                lsw.WriteLine("----------------------------------")
                lsw.WriteLine("StationID,TimeUnit,Date,PREC,ATEM,PMET,PEVT")

                For i As Integer = 1 To lPrecValsMonthly.GetUpperBound(0)
                    J2Date(lmonthDates(i), lDate)
                    ldateStr = lDate(0) & "-" & lDate(1)
                    ldateStr = lmonthDates(i).ToString
                    line = lStation & ",Monthly," & ldateStr & "," & lPrecValsMonthly(i).ToString & "," & lAtemValsMonthly(i).ToString & "," & lPMETValsMonthly(i).ToString
                    If lPEVTValsMonthly IsNot Nothing Then
                        line &= "," & lPEVTValsMonthly(i).ToString
                    End If
                    lsw.WriteLine(line)
                Next
                lsw.Flush()

                lsw.WriteLine("----------------------------------")
                lsw.WriteLine("StationID,TimeUnit,Date,PREC,ATEM,PMET,PEVT")

                For i As Integer = 1 To lPrecValsYearly.GetUpperBound(0)
                    J2Date(lyearDates(i), lDate)
                    ldateStr = lDate(0).ToString
                    ldateStr = lyearDates(i).ToString
                    line = lStation & ",Yearly," & ldateStr & "," & lPrecValsYearly(i).ToString & "," & lAtemValsYearly(i).ToString & "," & lPMETValsYearly(i).ToString
                    If lPEVTValsYearly IsNot Nothing Then
                        line &= "," & lPEVTValsYearly(i).ToString
                    End If
                    lsw.WriteLine(line)
                Next

                lsw.WriteLine("============================================================")
                lsw.WriteLine("==============  " & lStation & " Ends ======================")
                lsw.WriteLine("============================================================")
                lsw.Flush()

                ldoneReport = True

                'Write out SWAT met data for individual station
                'Dim lprojectFolder As String = IO.Path.GetDirectoryName(lFile)
                'Dim lmetFolder As String = IO.Path.Combine(lprojectFolder, "met")
                'Dim lsaveInFolder As String = IO.Path.Combine(lprojectFolder, lStation)
                ''Dim lblankWDMFile As String = IO.Path.Combine(pOutputPath, "metBlank.wdm")
                'If Not IO.Directory.Exists(lmetFolder) Then
                '    MkDir(lmetFolder)
                'End If
                'If Not IO.Directory.Exists(lsaveInFolder) Then
                '    MkDir(lsaveInFolder)
                'End If

                ''If IO.File.Exists(IO.Path.Combine(lFile, IO.Path.Combine(lmetFolder, "met.wdm"))) Then
                '' IO.File.Delete(IO.Path.Combine(lFile, IO.Path.Combine(lmetFolder, "met.wdm")))
                ''End If
                ''IO.File.Copy(lFile, IO.Path.Combine(lmetFolder, "met.wdm"))
                ''IO.File.Copy(lblankWDMFile, IO.Path.Combine(lmetFolder, "met.wdm")) ' make a blank copy

                ''Add data into that wdm
                'Dim lstationWDMFile As New atcDataSource
                'lstationWDMFile.Specification = "In-memory"

                ''Dim lstationWDMFile As New atcWDM.atcDataSourceWDM
                ''lstationWDMFile.Open(IO.Path.Combine(lmetFolder, "met.wdm"))
                'lstationWDMFile.DataSets.AddRange(lWDMFile.DataSets.FindData("Location", lStation))

                'If IO.File.Exists(IO.Path.Combine(lsaveInFolder, "pcp1.pcp")) Then IO.File.Delete(IO.Path.Combine(lsaveInFolder, "pcp1.pcp"))
                'If IO.File.Exists(IO.Path.Combine(lsaveInFolder, "pet1.pet")) Then IO.File.Delete(IO.Path.Combine(lsaveInFolder, "pet1.pet"))
                'If IO.File.Exists(IO.Path.Combine(lsaveInFolder, "slr.slr")) Then IO.File.Delete(IO.Path.Combine(lsaveInFolder, "slr.slr"))
                'If IO.File.Exists(IO.Path.Combine(lsaveInFolder, "tmp1.tmp")) Then IO.File.Delete(IO.Path.Combine(lsaveInFolder, "tmp1.tmp"))
                'If IO.File.Exists(IO.Path.Combine(lsaveInFolder, "wnd.wnd")) Then IO.File.Delete(IO.Path.Combine(lsaveInFolder, "wnd.wnd"))

                'Try
                '    WriteSwatMetInput(lstationWDMFile, Nothing, lprojectFolder, lsaveInFolder, lSJD, lEJD)
                'Catch lEx As Exception
                '    Logger.Dbg("WriteSwatMetInput Exception: " & lEx.InnerException.Message & vbCrLf & lEx.InnerException.StackTrace)
                '    Logger.Flush()
                'End Try

clearEnd:
                'lstationWDMFile = Nothing
                ltsPrec = Nothing
                ltsAtem = Nothing
                ltsPEVT = Nothing 'for report
                ltsPMET = Nothing
                lAtemVals = Nothing
                lPrecValsMonthly = Nothing
                lPrecValsYearly = Nothing
                lAtemValsMonthly = Nothing
                lAtemValsYearly = Nothing
                lPEVTVals = Nothing
                lPEVTValsMonthly = Nothing
                lPEVTValsYearly = Nothing
                lPMETValsMonthly = Nothing
                lPMETValsYearly = Nothing
            Next ' lPTPairList


            'Write out SWAT met data for a group of stations in a given WDM
            Dim lprojectFolder As String = IO.Path.GetDirectoryName(lFile)
            Dim lsaveInFolder As String = lprojectFolder

            Try
                WriteSwatMetInput(lWDMFile, lprojectFolder, lsaveInFolder, lSJD, lEJD)
            Catch lEx As Exception
                Logger.Dbg("WriteSwatMetInput Exception: " & lEx.InnerException.Message & vbCrLf & lEx.InnerException.StackTrace)
                Logger.Flush()
            End Try

            lWDMFile.DataSets.Clear()
            lWDMFile = Nothing

        Next ' lFiles
        lsw.Close()

        'Application.Exit()
    End Sub

    '
    'This function finds all pair of PREC and ATEM for any given WDM files
    ' return the total number of pairs found, as well as populating the list holding the pair
    ' with station name as key
    '
    Public Function getRainTempPairs(ByVal aPTPairList As Dictionary(Of String, String), ByVal aWDMFile As atcWDM.atcDataSourceWDM) As Integer
        Dim locPrec As String = String.Empty
        Dim locAtem As String = String.Empty
        Dim locPevt As String = String.Empty ' For report
        Dim lDSNPrec As Integer = 0
        Dim lDSNAtem As Integer = 0
        Dim lDSNPevt As Integer = 0 'For report
        For i As Integer = 0 To aWDMFile.DataSets.Count - 1
            If aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("Constituent") = "PREC" Then
                locPrec = aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("Location")
                lDSNPrec = aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("ID")
                lDSNAtem = lDSNPrec + 2
                lDSNPevt = lDSNPrec + 5 ' For report
                If aWDMFile.DataSets.ItemByKey(lDSNAtem) Is Nothing Then
                    Continue For
                End If
                If aWDMFile.DataSets.ItemByKey(lDSNAtem).Attributes.GetFormattedValue("Constituent") = "ATEM" Then
                    locAtem = aWDMFile.DataSets.ItemByKey(lDSNAtem).Attributes.GetFormattedValue("Location")
                    If locPrec = locAtem Then 'found a matching pair
                        aPTPairList.Add(locPrec, lDSNPrec.ToString & "," & lDSNAtem.ToString & "," & lDSNPevt.ToString)
                    End If
                End If
            End If
        Next
        Return aPTPairList.Count
    End Function

    Public Function getRainTempPairs(ByRef aPTPairList As Dictionary(Of String, String), ByVal aWDMFile As atcWDM.atcDataSourceWDM, ByVal alkupFile As String) As Integer
        Dim lsr As System.IO.StreamReader = Nothing
        Dim locPrec As String = String.Empty
        Dim locAtem As String = String.Empty
        Dim locPevt As String = String.Empty ' For report
        Dim lDSNPrec As Integer = 0
        Dim lDSNAtem As Integer = 0
        Dim lDSNPevt As Integer = 0 'For report

        lsr = New System.IO.StreamReader(alkupFile)
        Dim larr() As String = Nothing
        Dim line As String = String.Empty
        While Not lsr.EndOfStream
            line = lsr.ReadLine()
            If line.StartsWith("LOCATION") Then Continue While
            If line.StartsWith("location") Then Continue While
            If line.StartsWith("Location") Then Continue While
            larr = line.Split(",")
            aPTPairList.Add(larr(0), larr(2) & "," & larr(3) & "," & larr(1)) ' stationID -> PREC_DSN,ATEM_DSN,SWAT_ID
            larr = Nothing
        End While
        lsr.Close()
        lsr = Nothing

        'For report, look for matching PEVT values
        line = String.Empty
        Dim ldictionary As New Dictionary(Of String, String)

        For Each lkey As String In aPTPairList.Keys
            ldictionary.Add(lkey, "")
            line = aPTPairList.Item(lkey)
            For i As Integer = 0 To aWDMFile.DataSets.Count - 1
                If aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("Location") = lkey Then
                    If aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("Constituent") = "PEVT" Then
                        line = line & "," & aWDMFile.DataSets.Item(i).Attributes.GetFormattedValue("ID").ToString
                        Exit For
                    End If
                End If
            Next ' item i
            ldictionary.Item(lkey) = line
            line = String.Empty
        Next ' lkey

        aPTPairList.Clear()
        aPTPairList = Nothing

        aPTPairList = ldictionary
        Return aPTPairList.Count
    End Function


    Public Function getSWATStnKey(ByVal alkupFile As String, ByVal aStation As String) As String
        Dim lsr As System.IO.StreamReader
        lsr = New System.IO.StreamReader(alkupFile)
        Dim line As String = String.Empty
        Dim lswatKey As String = String.Empty
        Dim larr() As String = Nothing
        While Not lsr.EndOfStream
            line = lsr.ReadLine()
            larr = line.Split(",")
            If larr(0) = "LOCATION" OrElse larr(0) = "location" OrElse larr(0) = "Location" Then
                larr = Nothing
                Continue While
            End If
            If larr(0) = aStation Then
                lswatKey = larr(0).Substring(0, 2) & " " & larr(1)
                Exit While
            End If
            larr = Nothing
        End While
        lsr.Close()
        Return lswatKey
    End Function
    Public Function setSWATStn(ByRef aDictionary As Dictionary(Of String, String), ByVal aKey As String) As Boolean
        Dim lhasStats As Boolean = False
        For Each lkey As String In aDictionary.Keys
            If lkey = aKey Then
                lhasStats = True
                Exit For
            End If
        Next
        If lhasStats Then
            Dim lsw As System.IO.StreamWriter = Nothing
            lsw = New System.IO.StreamWriter(pSWATStnFile, False)
            lsw.WriteLine(aDictionary.Item(aKey))
            lsw.Flush()
            lsw.Close()
            Return True
        Else
            Return False
        End If
    End Function

End Module
