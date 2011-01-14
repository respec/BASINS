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

Public Module SWCPMET
    Private Const pReportNanAsZero As Boolean = True
    Private pWorkingDir As String = "G:\Data\BasinsMet\"
    Private pOutputPath As String = pWorkingDir & "WDMFinal\" '<<<Change here to your own WDM folder, lDataSource
    'Private pOutputPath As String = pWorkingDir & "WDMDebug\" '<<<Change here to your own WDM folder, lDataSource
    Private pBSNsAllStnFile As String = pWorkingDir & "Stations\StationLocs.dbf" '<<<Need to locate your BASINS weather station DBF folder
    Private pSWATAllStnFile As String = pWorkingDir & "Stations\AllStns.txt" '<<< do not change this file name as it is the storage of all stations
    Private pSWATStnFile As String = pOutputPath & "statwgn.txt" '<<< do not change this file name as it is required by the FORTRAN code
    Private pAllSWATStnsList As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Private pdoPET As Boolean = True ' Switch to do the actual PM PET calculation
    Private pdoReport As Boolean = False

    Private pStartYear As Integer = 1970
    Private pEndYear As Integer = 2010
    Private Const pFormatLL As String = "###0.00###"
    Private Const pFormat As String = "##,###,##0.00"

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
        Logger.Dbg("SWCPMET:Start")
        ChDriveDir(pOutputPath)
        Logger.Dbg("SWCPMET:CurDir:" & CurDir())

        Dim lFiles As New NameValueCollection
        AddFilesInDir(lFiles, pOutputPath, True, "*.wdm")
        Logger.Dbg("SWCPMET: Found " & lFiles.Count & " WDM data files")
        If pdoReport Then
            SummarizePMET(pOutputPath, lFiles)
            Exit Sub
        End If

        Dim lStationDBF As New atcTableDBF
        Dim lSJD As Double
        Dim lEJD As Double

        Dim lSJDSWCPMET As Double = Date2J(1970, 1, 1, 0, 0, 0)
        Dim lEJDSWCPMET As Double = Date2J(2010, 12, 31, 24, 0, 0)

        Dim lsw As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "PMETReport.txt"), False)
        Dim ldoneReport As Boolean = False

        Dim line As String = String.Empty

        Dim lNVals As Integer
        Dim lDate(5) As Integer
        Dim lJDay As Integer
        Dim lLat As Double
        Dim lElev As Double
        Dim lNYrs As Integer
        Dim lCO2 As Double

        'Create a station_number-keyed list of SWAT WGN parameters
        'TODO: this could be replaced with the PointLocations class

        Dim lSR As System.IO.StreamReader = New System.IO.StreamReader(pSWATAllStnFile)
        Dim lKey As String = String.Empty
        Dim lArr() As String = Nothing
        While Not lSR.EndOfStream
            line = lSR.ReadLine()
            lArr = line.Split(" ")
            lKey = lArr(3) ' e.g. 249, but as a string
            pAllSWATStnsList.Add(lKey, line)
            lArr = Nothing
        End While
        lSR.Close()
        lSR = Nothing 'reuse this reader later

        If lStationDBF.OpenFile(pBSNsAllStnFile) Then
            Logger.Dbg("SWCPMET: Opened Basins station location file: " & pBSNsAllStnFile)
        End If

        For Each lFile As String In lFiles
            ''The current set up requires one StationList.txt be placed in the 
            ''same folder as each WDM file as look up guide
            'lkupFile = IO.Path.Combine(IO.Path.GetDirectoryName(lFile), "StationList.txt")
            ''if a stationlist.txt is not there with a particular met.wdm, then skip it
            'If Not IO.File.Exists(lkupFile) Then
            '    Continue For
            'End If

            Logger.Dbg("SWCPMET: Opening data file - " & lFile)
            If pdoPET Then
                'Open WDM data file
                Dim lWDMFile As atcWDM.atcDataSourceWDM = New atcWDM.atcDataSourceWDM
                lWDMFile.Open(lFile)

                Dim lPTPairList As New Dictionary(Of String, String)
                Dim lnumPairs As Integer = getRainTempPairs(lPTPairList, lWDMFile)
                Logger.Dbg("SWCPMET: Found " & lnumPairs & " matching pairs of rain and temp.")

                For Each lStation As String In lPTPairList.Keys 'Keys are station name
                    Dim ltsPrecID As Integer = Integer.Parse(lPTPairList.Item(lStation).Split(",")(0))
                    Dim ltsAtemID As Integer = Integer.Parse(lPTPairList.Item(lStation).Split(",")(1))

                    Dim ltsPMET As atcTimeseries = Nothing
                    Dim ltsPMETHour As atcTimeseries = Nothing
                    Dim ltsPrec As atcTimeseries
                    Dim ltsAtem As atcTimeseries
                    Dim ltsAtemSub As atcTimeseries = Nothing

                    Dim lPrecVals() As Double = Nothing
                    Dim lTMinVals() As Double = Nothing
                    Dim lTMaxVals() As Double = Nothing
                    Dim lPMETValsHdl() As Double = Nothing

                    Dim ltsTemp As atcTimeseries = Nothing

                    'Find a matching SWAT stn or closest SWAT stn
                    'TODO: might need to have a closest distance threshold beyond which just don't do it
                    Dim lswatStnKey As String = ""
                    Dim LatDeg As Double
                    Dim LngDeg As Double
                    With lPTPairList.Item(lStation)
                        LatDeg = .Split(",")(2)
                        LngDeg = .Split(",")(3)
                    End With
                    'find the closest SWAT Stn's id
                    lswatStnKey = getTargetSWATStnKeys(LatDeg, LngDeg)
                    'if there is no SWAT stn found, then bypass this WDM wthr station
                    If lswatStnKey = "" Then
                        Continue For
                    End If
                    'write out SWAT parameter for SWAT fortran PMET routine to read
                    'if fails, then bypass this station
                    If Not setSWATStn(pAllSWATStnsList, lswatStnKey) Then
                        Continue For
                    End If

                    'Now, prepare the precip and temperature data
                    ltsPrec = TrimTimeseries(lWDMFile.DataSets.ItemByKey(ltsPrecID))
                    If ltsPrec Is Nothing Then
                        Logger.Msg("SWCPMET:Precip data trim problem: " & lWDMFile.DataSets.ItemByKey(ltsPrecID).Attributes.GetFormattedValue("Location"), "Problem")
                        Throw New ApplicationException()
                    End If
                    ltsAtem = TrimTimeseries(lWDMFile.DataSets.ItemByKey(ltsAtemID))
                    If ltsAtem Is Nothing Then
                        Logger.Msg("SWCPMET:ATMP data trim problem: " & lWDMFile.DataSets.ItemByKey(ltsAtemID).Attributes.GetFormattedValue("Location"), "Problem")
                        Throw New ApplicationException()
                    End If

                    Logger.Dbg("SWCPMET:Start PM calculation for station - " & lStation)
                    'Find common period for precip/temp
                    lSJD = Math.Max(ltsPrec.Attributes.GetValue("SJDay"), _
                                    ltsAtem.Attributes.GetValue("SJDay"))
                    lEJD = Math.Min(ltsPrec.Attributes.GetValue("EJDay"), _
                                    ltsAtem.Attributes.GetValue("EJDay"))
                    If lSJD < lEJD Then 'common period found

                        'Coordinate with SWC desired date boundaries
                        If lSJD <= lSJDSWCPMET Then
                            lSJD = lSJDSWCPMET
                        End If
                        If lEJD >= lEJDSWCPMET Then
                            lEJD = lEJDSWCPMET
                        End If

                        If lSJD >= lEJD Then
                            Logger.Dbg("SWCPMET:No common period available for Precip and Air Temp data during " & DumpDate(lSJDSWCPMET) & " ~ " & DumpDate(lEJDSWCPMET))
                            GoTo EndCleanUp
                        End If

                        Logger.Dbg("SWCPMET:Generating ET for period " & DumpDate(lSJD) & " - " & DumpDate(lEJD))

                        'SubsetByDate, Aggregate to daily
                        'TODO: might need to add a test to see what the original timeseries' tu is instead of assuming hourly
                        lPrecVals = Aggregate((SubsetByDate(ltsPrec, lSJD, lEJD, Nothing)), atcTimeUnit.TUDay, 1, atcTran.TranSumDiv).Values
                        ltsAtemSub = SubsetByDate(ltsAtem, lSJD, lEJD, Nothing)
                        lTMinVals = Aggregate(ltsAtemSub, atcTimeUnit.TUDay, 1, atcTran.TranMin).Values
                        lTMaxVals = Aggregate(ltsAtemSub, atcTimeUnit.TUDay, 1, atcTran.TranMax).Values

                        lNVals = lPrecVals.GetUpperBound(0)

                        'Figure out the starting day for calling fortran PMET function below
                        J2Date(lSJD, lDate)
                        If lDate(1) > 1 OrElse lDate(2) > 1 Then 'start is not jan 1, figure out julian date for the year
                            lDate(1) = 1
                            lDate(2) = 1
                            lJDay = lSJD - Date2J(lDate) + 1
                        Else
                            lJDay = 1
                        End If
                        lNYrs = timdifJ(lSJD, lEJD, atcTimeUnit.TUYear, 1)
                        lStationDBF.FindFirst(1, lStation.Substring(2))
                        lLat = lStationDBF.Value(4) 'in decimal degrees
                        lElev = lStationDBF.Value(6) 'in feet
                        lElev = lElev * 0.3048 ' turn into meter

                        Dim lPMETValsSingle(1) As Single
                        Dim lPrecValsSingle(1) As Single
                        Dim lTMinValsSingle(1) As Single
                        Dim lTMaxValsSingle(1) As Single

                        ReDim lPMETValsSingle(lNVals)
                        ReDim lPrecValsSingle(lNVals)
                        ReDim lTMinValsSingle(lNVals)
                        ReDim lTMaxValsSingle(lNVals)

                        'Copy values into arrays of single for calling fortran
                        For z As Integer = 0 To lPrecVals.GetUpperBound(0)
                            lPrecValsSingle(z) = lPrecVals(z)
                            lTMinValsSingle(z) = lTMinVals(z)
                            lTMaxValsSingle(z) = lTMaxVals(z)
                        Next
                        'Call fortran PMET code
                        Logger.Dbg("SWCPMET:Fortran PMET starts")
                        PMPEVT(Integer.Parse(lswatStnKey), lDate(0), lJDay, lNYrs + 1, lElev, lLat, lCO2, lNVals, lPrecValsSingle(1), lTMaxValsSingle(1), lTMinValsSingle(1), lPMETValsSingle(1))
                        Logger.Dbg("SWCPMET:Fortran PMET ends")
                        ltsPMET = ltsAtemSub.Clone
                        ltsPMET.Attributes.SetValue("Constituent", "PMET")
                        ltsPMET.Attributes.SetValue("ID", ltsAtemID + 6)
                        ltsPMET.Attributes.SetValue("TU", atcTimeUnit.TUDay)
                        ltsPMET.Attributes.SetValue("description", "Daily SWAT PM ET mm")
                        ltsPMET.Attributes.SetValue("interval", 1.0)
                        ltsPMET.Attributes.SetValue("TSTYPE", "PMET")
                        'J2Date(lSJD, lDate)
                        ltsPMET.Attributes.SetValue("TSBYR", lDate(0))

                        'These are the same as the ltsAtemSub's
                        'ltsPMET.Attributes.SetValue("Location", lStation)
                        'ltsPMET.Attributes.SetValue("start date", lSJD)
                        'ltsPMET.Attributes.SetValue("end date", lEJD)
                        'ltsPMET.Attributes.SetValue("Latitude", lLat)

                        ltsPMET.Dates.Values = NewDates(lSJD, lEJD, atcTimeUnit.TUDay, 1)

                        'Copy back from single PMET array to double array
                        Dim lPMETVals(lPMETValsSingle.GetUpperBound(0)) As Double
                        Dim lValue As Double = 0.0
                        For z As Integer = 0 To lPMETValsSingle.GetUpperBound(0)
                            lValue = Double.Parse(lPMETValsSingle(z).ToString)
                            If lValue < 0.00000001 Then
                                lValue = 0.0
                            ElseIf lValue > 40.0 Then
                                Logger.Dbg("SWCPMET:Extremely large PMET daily value, i.e. " & lValue & " in " & lStation)
                            Else
                                lValue = CInt(lValue * 1000000.0 + 0.5) / 1000000.0
                            End If
                            lPMETVals(z) = lValue
                        Next
                        ltsPMET.Values = lPMETVals

                        If ltsPMET.Attributes.GetDefinedValue("start date").Value > ltsPMET.Attributes.GetDefinedValue("end date").Value Then
                            GoTo PMETTSPROBLEM
                        End If
                        Dim lLatitudeDefinedValue As atcDefinedValue = ltsPMET.Attributes.GetDefinedValue("Latitude")
                        Dim lLatitude As Double
                        If lLatitudeDefinedValue IsNot Nothing Then
                            lLatitude = lLatitudeDefinedValue.Value
                            Logger.Dbg("Latitude " & lLatitude)
                        Else
                            Logger.Dbg("DEFAULT Latitiude")
                            lLatitude = 45
                        End If

                        'Disaggragate the daily PMET timeseries into hourly timeseries
                        ltsPMETHour = atcMetCmp.DisSolPet(ltsPMET, Nothing, 2, lLatitude)
                        ltsPMETHour.Attributes.SetValue("Constituent", ltsPMET.Attributes.GetDefinedValue("Constituent").Value)
                        ltsPMETHour.Attributes.SetValue("TSTYPE", ltsPMET.Attributes.GetDefinedValue("TSTYPE").Value)
                        ltsPMETHour.Attributes.SetValue("ID", ltsPMET.Attributes.GetDefinedValue("ID").Value)

                        'Add the newly calculated hourly PMET timeseries back into the current WDM, overwrite if already exists.
                        If lWDMFile.AddDataset(ltsPMETHour, atcDataSource.EnumExistAction.ExistReplace) Then
                            Logger.Dbg("SWCPMET:Wrote Penman-Monteith PET to DSN " & ltsPMETHour.Attributes.GetValue("ID").ToString & ", SumAnnual PMET=" & ltsPMETHour.Attributes.GetValue("SumAnnual").ToString)
                        Else
PMETTSPROBLEM:
                            Logger.Dbg("SWCPMET:PROBLEM writing Penman-Monteith PET to DSN " & ltsPMETHour.Attributes.GetValue("ID").ToString)
                        End If

                        'throw away temporary arrays
                        ReDim lPMETValsSingle(0) : lPMETValsSingle = Nothing
                        ReDim lPrecValsSingle(0) : lPrecValsSingle = Nothing
                        ReDim lTMinValsSingle(0) : lTMinValsSingle = Nothing
                        ReDim lTMaxValsSingle(0) : lTMaxValsSingle = Nothing

                        ReDim lPrecVals(0) : lPrecVals = Nothing
                        ReDim lTMinVals(0) : lTMinVals = Nothing
                        ReDim lTMaxVals(0) : lTMaxVals = Nothing
                        ReDim lPMETValsHdl(0) : lPMETValsHdl = Nothing
                    Else
                        Logger.Dbg("SWCPMET:No common period available for Precip and Air Temp data")
                    End If
EndCleanUp:
                    If ltsPrec IsNot Nothing Then ltsPrec.Clear()
                    If ltsAtem IsNot Nothing Then ltsAtem.Clear()
                    If ltsAtemSub IsNot Nothing Then ltsAtemSub.Clear()
                    If ltsPMET IsNot Nothing Then ltsPMET.Clear()
                    If ltsPMETHour IsNot Nothing Then ltsPMETHour.Clear()

                    ltsPrec = Nothing
                    ltsAtem = Nothing
                    ltsAtemSub = Nothing
                    ltsPMET = Nothing
                    ltsPMETHour = Nothing
                    GC.Collect()
                    System.Threading.Thread.Sleep(30)
                    'Logger.Dbg(MemUsage)
                Next ' lPTPairList
                'Close it to let the added dataset finalize
                lWDMFile.Clear()
                lWDMFile = Nothing
                GC.Collect()
                System.Threading.Thread.Sleep(30)
                Logger.Dbg("SWCPMET:Done PMET Calculation for " & lFile)
            End If
        Next ' lFiles
        lsw.Close()
        lStationDBF.Clear()
        lStationDBF = Nothing
    End Sub

    Public Sub SummarizePMET(ByVal aDataSource As String, ByVal aWdmFileNames As NameValueCollection)
        Dim lConstituentNames() As String = {"PMET"}
        SummarizeData(aWdmFileNames, aDataSource)
        SummarizeDataByConstituentDetails(aDataSource, lConstituentNames, aWdmFileNames)
    End Sub

    '
    'This function finds all pair of PREC and ATEM for any given WDM files
    ' return the total number of pairs found, as well as populating the list holding the pair
    ' with station name as key
    '
    Public Function getRainTempPairs(ByVal aPTPairList As Dictionary(Of String, String), ByVal aWDMFile As atcWDM.atcDataSourceWDM) As Integer
        Dim locPrec As String = String.Empty
        Dim locAtem As String = String.Empty
        Dim LatDeg As Double
        Dim LngDeg As Double

        Dim lDSNPrec As Integer = 0
        Dim lDSNAtem As Integer = 0

        With aWDMFile.DataSets
            For i As Integer = 0 To .Count - 1
                With .Item(i).Attributes
                    If .GetValue("Constituent") = "PREC" Then
                        locPrec = .GetValue("Location")
                        lDSNPrec = .GetValue("ID")
                        LatDeg = .GetValue("LatDeg")
                        LngDeg = .GetValue("LngDeg")
                        lDSNAtem = lDSNPrec + 2
                        If aWDMFile.DataSets.ItemByKey(lDSNAtem) Is Nothing Then
                            Continue For
                        End If
                        If aWDMFile.DataSets.ItemByKey(lDSNAtem).Attributes.GetValue("Constituent") = "ATEM" Then
                            locAtem = aWDMFile.DataSets.ItemByKey(lDSNAtem).Attributes.GetValue("Location")
                            If locPrec = locAtem Then
                                'if found a matching pair, then add an entry, e.g.
                                'IN000001 ->1,3,31.4,-87.9
                                aPTPairList.Add(locPrec, lDSNPrec.ToString & "," & lDSNAtem.ToString & "," & LatDeg.ToString & "," & LngDeg.ToString)
                            End If
                        End If
                    End If
                End With
            Next
        End With
        Return aPTPairList.Count
    End Function

    ''' <summary>
    ''' This routine is derived from the 'Closest' function
    ''' </summary>
    ''' <param name="aLatitude"></param>
    ''' <param name="aLongitude"></param>
    ''' <param name="aMaxCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getTargetSWATStnKeys(ByVal aLatitude As Double, ByVal aLongitude As Double, _
                                        Optional ByVal aMaxCount As Integer = 5) As String
        Dim lClosestPoints As New SortedList(Of Double, String)
        For Each lPointLocation As String In pAllSWATStnsList.Keys
            Dim LatDeg As Double = Double.Parse(pAllSWATStnsList.Item(lPointLocation).Split(" ")(4))
            Dim LngDeg As Double = Double.Parse(pAllSWATStnsList.Item(lPointLocation).Split(" ")(5))
            Dim lCalcDistance As Double = Spatial.GreatCircleDistance(aLongitude, aLatitude, LngDeg, LatDeg)
            Dim lTempDistance As Double = lCalcDistance
            While lClosestPoints.ContainsKey(lTempDistance)
                lTempDistance *= 1.00000001
            End While
            lClosestPoints.Add(lTempDistance, lPointLocation)
        Next
        'TODO: could do return closest aMaxCount stn for averaging later
        'Dim lClosestPointsToReturn As New SortedList(Of Double, PointLocation)
        'Dim lCount As Integer = 0
        'For Each lItem As KeyValuePair(Of Double, PointLocation) In lClosestPoints
        '    lClosestPointsToReturn.Add(lItem.Key, lItem.Value)
        '    lCount += 1
        '    If lCount >= aMaxCount Then Exit For
        'Next
        'Return lClosestPointsToReturn

        Return lClosestPoints.Item(lClosestPoints.Keys.Item(0)) 'return the closest SWAT stn id
    End Function

    ''' <summary>
    ''' This function extract the SWAT parameters for a given station
    ''' and write it out to a file, which is to be read by SWAT FORTRAN PMET routine
    ''' </summary>
    ''' <param name="aDictionary">The dictionary of SWAT WGN stations and its parameters</param>
    ''' <param name="aKey">a SWAT station id</param>
    ''' <returns>True, if process is successful; False, if failed during process</returns>
    ''' <remarks></remarks>
    Public Function setSWATStn(ByVal aDictionary As Dictionary(Of String, String), ByVal aKey As String) As Boolean
        If aDictionary.ContainsKey(aKey) Then
            Dim lSW As System.IO.StreamWriter = New System.IO.StreamWriter(pSWATStnFile, False)
            lSW.WriteLine(aDictionary.Item(aKey))
            lSW.Flush()
            lSW.Close()
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>Wrapper for distance calculations</summary>
    ''' <remarks></remarks>
    Public Class Spatial
        Private Const DegreesToRadians As Double = 0.01745329252

        Public Shared Function GreatCircleDistance(ByVal aLong1 As Double, ByVal aLat1 As Double, ByVal aLong2 As Double, ByVal aLat2 As Double) As Double
            Dim lLat1 As Double = DegreesToRadians * aLat1
            Dim lLat2 As Double = DegreesToRadians * aLat2
            Dim lLong1 As Double = DegreesToRadians * aLong1
            Dim lLong2 As Double = DegreesToRadians * aLong2

            Dim lDistance As Double = 2 * Math.Asin(Math.Sqrt((Math.Sin((lLat1 - lLat2) / 2)) ^ 2 + _
                     Math.Cos(lLat1) * Math.Cos(lLat2) * (Math.Sin((lLong1 - lLong2) / 2)) ^ 2))
            lDistance *= 6366.71 'radians to km
            lDistance *= 1000  'km to m
            Return lDistance
        End Function
    End Class


    ''' <summary>Make a summary of data (with monthly values) in a collection of WDM files for a specified set of constituents</summary>
    ''' <param name="aDataSource">Name of original collection of WDM files</param>
    ''' <param name="aConstituentNames">Constituent names to summarize</param>
    ''' <param name="aWdmFileNames">Collection of WDM file names to summarize</param>
    ''' <remarks></remarks>
    Sub SummarizeDataByConstituentDetails(ByVal aDataSource As String, _
                                          ByVal aConstituentNames() As String, _
                                          ByVal aWdmFileNames As NameValueCollection)
        Dim lWdmFileNames As New NameValueCollection
        For Each lWdmFileName As String In aWdmFileNames
            Dim lWdmFileNameShort As String = IO.Path.GetFileName(lWdmFileName)
            lWdmFileNames.Add(lWdmFileNameShort, lWdmFileName)

            'If lWdmFileNames.AllKeys.Contains(lWdmFileName) Then
            '    Logger.Dbg("DuplicateKeyFor " & lWdmFileName)
            'Else
            '    lWdmFileNames.Add(lWdmFileNameShort, lWdmFileName)
            'End If
        Next

        Dim lSeasonsMonth As New atcSeasonsMonth
        Dim lDatasetTable As New atcTableDelimited
        Dim lSummaryFileName As String = ""
        For Each lConstituentName As String In aConstituentNames
            Logger.Dbg("SWCPMET:Process " & lConstituentName & " in " & aDataSource)
            With lDatasetTable
                .Delimiter = vbTab
                lSummaryFileName = IO.Path.Combine(aDataSource, lConstituentName & "_Summary.txt")
                'If .OpenFile(aDataSource & "_" & lConstituentName & "_Summary.txt") Then
                If .OpenFile(lSummaryFileName) Then
                    Logger.Dbg("Fields " & .NumFields & " Records " & .NumRecords)
                    Dim lDatasetTableWithDetails As atcTableDelimited = .Cousin
                    With lDatasetTableWithDetails
                        .NumFields += 13
                        .FieldName(.NumFields - 13) = "Sum"
                        Dim lMonthIndex As Integer = 1
                        For lFieldNameIndex As Integer = .NumFields - 12 To .NumFields - 1
                            .FieldName(lFieldNameIndex) = MonthName(lMonthIndex, True)
                            lMonthIndex += 1
                        Next
                        .NumRecords = 1
                    End With
                    'need to do the next 4 lines because changing the number of fields clears all fieldnames
                    For lFieldIndex As Integer = 1 To .NumFields - 1
                        lDatasetTableWithDetails.FieldName(lFieldIndex) = .FieldName(lFieldIndex)
                    Next
                    lDatasetTableWithDetails.FieldName(lDatasetTableWithDetails.NumFields) = .FieldName(.NumFields)

                    Dim lWdmFileNameFieldIndex As Integer = .FieldNumber("FileName")
                    Dim lIdFieldIndex As Integer = .FieldNumber("Id")
                    While Not .EOF
                        Dim lWdmFileNameWanted As String = .Value(lWdmFileNameFieldIndex)
                        Dim lWdmFileName As String = lWdmFileNames.Item(lWdmFileNameWanted)
                        'Dim lWDMFile As New atcWDMfile
                        Dim lWDMFile As New atcWDM.atcDataSourceWDM
                        If lWDMFile.Open(lWdmFileName) Then
                            Dim lTimeseries As atcTimeseries = lWDMFile.DataSets.FindData("Id", .Value(lIdFieldIndex)).Item(0)
                            If lTimeseries.Attributes.GetValue("Min") = -998.0 Then
                                UpdateAccumulated(lTimeseries)
                            End If
                            Dim lMonthValues(12) As Double
                            For Each lTimeseriesMonth As atcTimeseries In lSeasonsMonth.Split(lTimeseries, Nothing)
                                Dim lMonthValue As Double = lTimeseriesMonth.Attributes.GetValue("SumAnnual")
                                If pReportNanAsZero AndAlso Double.IsNaN(lMonthValue) Then
                                    lMonthValue = 0.0
                                ElseIf lMonthValue < 0.01 Then
                                    lMonthValue = 0.0
                                End If
                                lMonthValues(lTimeseriesMonth.Attributes.GetValue("SeasonIndex")) = lMonthValue
                            Next
                            lDatasetTableWithDetails.MoveNext()
                            For lFieldIndex As Integer = 1 To .NumFields - 1
                                lDatasetTableWithDetails.Value(lFieldIndex) = .Value(lFieldIndex)
                            Next
                            Dim lSumMonths As Double = 0.0
                            Dim lMonthIndex As Integer = 1
                            For lFieldIndex As Integer = .NumFields + 1 To .NumFields + 12
                                lSumMonths += lMonthValues(lMonthIndex)
                                lDatasetTableWithDetails.Value(lFieldIndex) = DoubleToString(lMonthValues(lMonthIndex), , pFormat)
                                lMonthIndex += 1
                            Next
                            lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields - 13) = DoubleToString(lSumMonths, , pFormat)
                            lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields) = .Value(.NumFields)
                            lDatasetTableWithDetails.NumRecords += 1
                            Logger.Dbg("Found " & lTimeseries.Attributes.GetValue("Id").ToString.PadLeft(4) & " value " & DoubleToString(lTimeseries.Attributes.GetValue("SumAnnual"), , pFormat).PadLeft(10) & _
                                       DoubleToString(lSumMonths, , pFormat).PadLeft(10) & " in " & lWdmFileName)
                            lWDMFile.DataSets.Clear()
                            lWDMFile = Nothing
                        Else
                            Logger.Dbg("ProblemOpening " & lWdmFileName)
                        End If

                        .MoveNext()
                    End While
                    lDatasetTableWithDetails.WriteFile(IO.Path.Combine(aDataSource, lConstituentName & "_Details.txt"))
                    .Clear()
                Else
                    Logger.Dbg(lConstituentName & " NotFound")
                End If
            End With
        Next 'constituent
    End Sub

    ''' <summary>Make a summary of data found in a collection of WDM files</summary>
    ''' <param name="aWdmFileNames">Collection of WDM file names to summarize</param>
    ''' <param name="aDataSource">Name of collection of WDM files</param>
    ''' <remarks></remarks>
    Sub SummarizeData(ByVal aWdmFileNames As NameValueCollection, ByVal aDataSource As String)
        Dim lD2SStart As New atcDateFormat
        lD2SStart.IncludeHours = False
        lD2SStart.IncludeMinutes = False
        lD2SStart.Midnight24 = False
        Dim lD2SEnd As New atcDateFormat
        lD2SEnd.IncludeHours = False
        lD2SEnd.IncludeMinutes = False

        Dim lTargetConstituent As String = "PMET"

        Dim lConstituentCounts As New atcCollection
        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("StationId" & vbTab & "DataType" & vbTab & "FileName" & vbTab & "Id" & vbTab & "Lat" & vbTab & "Long" & vbTab & _
                       "Scenario" & vbTab & "Constituent" & vbTab & _
                       "SDate" & vbTab & "EDate" & vbTab & "YrCount" & vbTab & "Value" & vbTab & "StaNam")
        Dim lWdmCnt As Integer = 0
        For Each lWdmFileName As String In aWdmFileNames
            lWdmCnt += 1
            Dim lWDMFile As New atcWDM.atcDataSourceWDM
            If lWDMFile.Open(lWdmFileName) Then
                Dim lWDMName As String = IO.Path.GetFileName(lWdmFileName)
                Dim latcTimeSeriesGroup As atcTimeseriesGroup = lWDMFile.DataSets.FindData("Constituent", lTargetConstituent)
                Dim lDataSource As String = IO.Path.GetDirectoryName(lWdmFileName).Trim("\")
                lDataSource = lDataSource.Substring(lDataSource.LastIndexOf("\") + 1)
                Dim lLatitudeBase As Double = lWDMFile.DataSets(0).Attributes.GetValue("Latitude")
                Dim lLongitudeBase As Double = lWDMFile.DataSets(0).Attributes.GetValue("Longitude")
                Dim lStationId As String = lWDMName.Substring(0, 6)
                If Not IsNumeric(lStationId.Substring(0, 1)) Then
                    lStationId = lWDMName.Substring(2, 6)
                End If

                For Each lTimeseries As atcTimeseries In latcTimeSeriesGroup
                    'lTimeseries = SubsetByDate(lTimeseries, Date2J(pStartYear, 1, 1), Date2J(pEndYear, 12, 31), Nothing)
                    Dim lCons As String = lTimeseries.Attributes.GetValue("Constituent")
                    lConstituentCounts.Increment(lCons)
                    Dim lScenario As String = lTimeseries.Attributes.GetValue("Scenario")
                    If lTimeseries.Attributes.GetValue("Min") = -998.0 Then
                        UpdateAccumulated(lTimeseries)
                    End If
                    Dim lValue As String = ""
                    Select Case lCons 'dont include constituents dont want to summarize
                        Case "PMET"
                            lValue = DoubleToString(lTimeseries.Attributes.GetValue("SumAnnual"), , pFormat)
                            'Case "DEWP", "WNDH", "RADH", "ATMP", "CLDC"
                            '    lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), , pFormat)
                            'Case Else
                            '    lValue = DoubleToString(lTimeseries.Attributes.GetValue("Mean"), 16, pFormat) '"?"
                    End Select
                    If lValue <> "?" Or lValue <> "" Then
                        With lTimeseries.Attributes
                            Dim lSJDay As Double = .GetValue("SJDay")
                            Dim lEJDay As Double = .GetValue("EJDay")
                            lSB.AppendLine(lStationId.PadLeft(8) & vbTab & _
                                           lDataSource.PadLeft(8) & vbTab & _
                                           lWDMName.PadLeft(12) & vbTab & _
                                           .GetValue("ID").ToString.PadLeft(12) & vbTab & _
                                           DoubleToString(.GetValue("Latitude", lLatitudeBase), , pFormatLL).PadLeft(12) & vbTab & _
                                           DoubleToString(.GetValue("Longitude", lLongitudeBase), , pFormatLL).PadLeft(12) & vbTab & _
                                           lScenario.PadLeft(12) & vbTab & _
                                           lCons.PadLeft(10) & vbTab & _
                                           ("'" & lD2SStart.JDateToString(lSJDay) & "'").PadLeft(12) & vbTab & _
                                           ("'" & lD2SEnd.JDateToString(lEJDay) & "'").PadLeft(12) & vbTab & _
                                           DoubleToString((lEJDay - lSJDay) / 365.25, , pFormat).PadLeft(6) & vbTab & _
                                           lValue.PadLeft(10) & vbTab & _
                                           .GetValue("StaNam".ToString))
                        End With
                    Else
                        'TODO: first time skip message
                    End If
                Next
                lWDMFile.DataSets.Clear()
                lWDMFile = Nothing
                latcTimeSeriesGroup.Clear()
                latcTimeSeriesGroup = Nothing
            Else
                Logger.Dbg("ProblemOpening " & lWdmFileName)
            End If
            Dim lPercent As String = "(" & DoubleToString((100 * lWdmCnt) / aWdmFileNames.Count, , pFormat) & "%)"
            Logger.Dbg("Done " & lWdmCnt & lPercent & lWdmFileName & " MemUsage " & MemUsage())
        Next
        SaveFileString(IO.Path.Combine(aDataSource, lTargetConstituent & "_Summary.txt"), lSB.ToString)
        lSB = New Text.StringBuilder 'use .Clear in 4.0
        For lConstituentIndex As Integer = 0 To lConstituentCounts.Count - 1
            lSB.AppendLine(lConstituentCounts.Keys(lConstituentIndex).ToString.PadLeft(12) & vbTab & lConstituentCounts.Item(lConstituentIndex))
        Next
        SaveFileString(IO.Path.Combine(aDataSource, "ConstituentCounts.txt"), lSB.ToString)
        Logger.Dbg("All Done " & lWdmCnt & " WDMs")
    End Sub

    ''' <summary>Distributes accumulated data within a timeseries</summary>
    ''' <param name="aTimeseries">Timeseries to distribute accumulated data in</param>
    ''' <param name="aAccumulatedValue">Accumulated data value</param>
    ''' <remarks></remarks>
    Private Sub UpdateAccumulated(ByVal aTimeseries As atcTimeseries, Optional ByVal aAccumulatedValue As Double = -998.0)
        With aTimeseries
            Dim lCons As String = .Attributes.GetValue("Constituent")
            Dim lFunction As String = "Div"
            If lCons = "ATMP" Then  'TODo - add more as needed
                lFunction = "Same"
            End If

            Dim lAccumIndexStart As Integer = 0
            Dim lAccum As Boolean = False
            For lValueIndex As Integer = 1 To .numValues
                If .Value(lValueIndex) = aAccumulatedValue Then
                    If Not lAccum Then
                        lAccumIndexStart = lValueIndex
                        lAccum = True
                    End If
                ElseIf lAccum Then
                    Dim lNewValue As Double = .Value(lValueIndex)
                    If lFunction = "Div" Then
                        lNewValue /= CDbl(1 + lValueIndex - lAccumIndexStart)
                    End If
                    For lTempIndex As Integer = lAccumIndexStart To lValueIndex
                        .Value(lTempIndex) = lNewValue
                    Next
                    lAccum = False
                End If
            Next
            .Attributes.CalculateAll()
        End With
    End Sub
End Module

