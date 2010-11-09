Imports atcData
Imports atcUtility
Imports atcWDM
Imports MapWinUtility

Module modSwatMetData

    'Private pWDMconstituents() As String = {"PREC", "ATEM", "SOLR", "WIND", "PEVT"}
    'Private pSWATconstituentFilenames() As String = {"pcp1.pcp", "tmp1.tmp", "slr.slr", "wnd.wnd", "pet1.pet"}

    Private pModifiedData As atcTimeseriesGroup

    Public Sub WriteSwatMetInput(ByVal aCacheFolder As String, _
                                 ByVal aProjectFolder As String, _
                                 ByVal aSaveInFolder As String, _
                                 ByVal aDateStart As Double, _
                                 ByVal aDateEnd As Double)
        'If aTextInput Then
        '    Dim lMetPcpFilename As String = IO.Path.Combine(aProjectFolder, "met\pcp1_new.pcp")
        '    Dim lMetTmpFilename As String = IO.Path.Combine(aProjectFolder, "met\tmp1_new.tmp")
        '    Dim lMetPcpToFilename As String = IO.Path.Combine(aSaveInFolder, "pcp1.pcp")
        '    Dim lMetTmpToFilename As String = IO.Path.Combine(aSaveInFolder, "tmp1.tmp")

        '    IO.File.Copy(lMetPcpFilename, lMetPcpToFilename, True)
        '    IO.File.Copy(lMetTmpFilename, lMetTmpToFilename, True)
        '    Exit Sub
        '    'Dim lSwatPcpData As New atcTimeseriesSWAT.atcTimeseriesSWAT
        '    'Dim lSwatTmpData As New atcTimeseriesSWAT.atcTimeseriesSWAT
        '    'lSwatPcpData.Open(lMetPcpFilename)
        '    'lSwatTmpData.Open(lMetTmpFilename)
        '    'lSwatPcpData.Save(lMetPcpFilename, atcDataSource.EnumExistAction.ExistReplace)
        '    'lSwatTmpData.Save(lMetTmpFilename, atcDataSource.EnumExistAction.ExistReplace)
        'End If
        Dim lMetWDMfilename As String = IO.Path.Combine(aProjectFolder, "met\met.wdm")
        Dim lMetWDM As New atcDataSourceWDM ' open met.wdm created by download
        If lMetWDM.Open(lMetWDMfilename) Then
            If lMetWDM.DataSets.Count > 0 Then
                WriteSwatMetInput(lMetWDM, Nothing, aProjectFolder, aSaveInFolder, aDateStart, aDateEnd)
                lMetWDM.Clear()
            Else
                MapWinUtility.Logger.Dbg("No data found in met wdm: " & lMetWDMfilename)
            End If
        Else
            MapWinUtility.Logger.Dbg("Could not open met wdm: " & lMetWDMfilename)
        End If
    End Sub

    Public Sub WriteSwatMetInput(ByVal aOriginalData As atcDataSource, ByVal aModifiedData As atcTimeseriesGroup, ByVal aSaveInFolder As String)
        'Separate Prec and Atem modified data from aModifiedData into two atcTimeseriesGroup(s)
        'Create two new atcTimeseriesSWAT, one for pcp and one for tmp
        'search to swap into the two new atctimeseriesSWAT Datasets the modified data
        'Then, call their Save function to save into aSaveInFolder
        Dim lNewSwatDataSource As New atcTimeseriesSWAT.atcTimeseriesSWAT
        lNewSwatDataSource.DataSets.AddRange(aOriginalData.DataSets)
        lNewSwatDataSource.DataType = CType(aOriginalData, atcTimeseriesSWAT.atcTimeseriesSWAT).DataType
        Dim lTargetSwatCons As String = lNewSwatDataSource.DataSets(0).Attributes.GetValue("Constituent")
        Dim lTargetSwatStnID As String = ""
        Dim lTargetSwatFldInd As String = ""

        ''Another way of merging modified and original datasets
        ''but it seems the original data should not be changed
        'lTargetSwatCons = aOriginalData.DataSets(0).Attributes.GetValue("Constituent")
        'Dim lFoundMatch As Boolean = False
        'For I As Integer = 0 To aOriginalData.DataSets.Count - 1
        '    lTargetSwatStnID = aOriginalData.DataSets(I).Attributes.GetValue("SWATSTNID")
        '    lTargetSwatFldInd = aOriginalData.DataSets(I).Attributes.GetValue("FieldIndex")
        '    lFoundMatch = False
        '    For J As Integer = 0 To aModifiedData.Count - 1
        '        If aModifiedData(J).Attributes.GetValue("Constituent") = lTargetSwatCons And _
        '           aModifiedData(J).Attributes.GetValue("SWATSTNID") = lTargetSwatStnID And _
        '           aModifiedData(J).Attributes.GetValue("FieldIndex") = lTargetSwatFldInd Then
        '            lNewSwatDataSource.DataSets.Add(aModifiedData(J))
        '            lFoundMatch = True
        '            Exit For
        '        End If
        '    Next
        '    If Not lFoundMatch Then
        '        lNewSwatDataSource.DataSets.Add(aOriginalData.DataSets(I))
        '    End If
        'Next

        For I As Integer = 0 To lNewSwatDataSource.DataSets.Count - 1
            lTargetSwatStnID = lNewSwatDataSource.DataSets(I).Attributes.GetValue("SWATSTNID")
            lTargetSwatFldInd = lNewSwatDataSource.DataSets(I).Attributes.GetValue("FieldIndex")
            For J As Integer = 0 To aModifiedData.Count - 1
                If aModifiedData(J).Attributes.GetValue("Constituent") = lTargetSwatCons And _
                   aModifiedData(J).Attributes.GetValue("SWATSTNID") = lTargetSwatStnID And _
                   aModifiedData(J).Attributes.GetValue("FieldIndex") = lTargetSwatFldInd Then
                    lNewSwatDataSource.DataSets(I) = aModifiedData(J)
                End If
            Next
        Next
        Dim lSavedInFilename As String = ""
        Select Case lTargetSwatCons.ToLower
            Case "prec"
                lSavedInFilename = IO.Path.Combine(aSaveInFolder, "pcp1.pcp")
            Case "atem"
                lSavedInFilename = IO.Path.Combine(aSaveInFolder, "tmp1.tmp")
        End Select
        lNewSwatDataSource.Save(lSavedInFilename, atcDataSource.EnumExistAction.ExistReplace)
    End Sub

    Public Sub WriteSwatMetInput(ByVal aOriginalData As atcDataSource, _
                                 ByVal aModifiedData As atcTimeseriesGroup, _
                                 ByVal aProjectFolder As String, _
                                 ByVal aSaveInFolder As String, _
                                 ByVal aDateStart As Double, _
                                 ByVal aDateEnd As Double)

        pModifiedData = aModifiedData

        'See if we can open original station WDM containing additional data
        'Dim lts As atcData.atcTimeseries = lMetWDM.DataSets(0)
        'Dim lStationID As String = lts.Attributes.GetValue("Location", "")
        'If lStationID.Length > 0 Then
        '    Dim lStationFilename As String = IO.Path.Combine(aCacheFolder, "clsBasins\met\" & lStationID & ".wdm")
        '    If IO.File.Exists(lStationFilename) Then
        '        Dim lStationWDM As New atcDataSourceWDM ' open original station wdm downloaded to cache
        '        If lStationWDM.Open(lStationFilename) Then
        '            lMetWDM = lStationWDM
        '        End If
        '    End If
        'End If

        Dim lD4EMSource As New atcTimeseriesD4EM.atcDataSourceTimeseriesD4EM
        lD4EMSource.DataSets.Add("pcp", WritePCP(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))
        Dim lTmp As atcTimeseries = WriteTMP(aOriginalData, aSaveInFolder, aDateStart, aDateEnd)
        lD4EMSource.DataSets.Add("tmean", lTmp)
        If lTmp.Attributes.ContainsAttribute("tmax") Then lD4EMSource.DataSets.Add("tmax", lTmp.Attributes.GetValue("tmax"))
        If lTmp.Attributes.ContainsAttribute("tmin") Then lD4EMSource.DataSets.Add("tmin", lTmp.Attributes.GetValue("tmin"))
        lD4EMSource.DataSets.Add("slr", WriteSLR(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))
        lD4EMSource.DataSets.Add("wnd", WriteWND(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))
        lD4EMSource.DataSets.Add("pet", WritePET(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))
        'lD4EMSource.DataSets.Add("hmd", WriteHMD(lMetWDM, aSaveInFolder, aDateStart, aDateEnd))
        lD4EMSource.DataSets.Add("dew", WriteDEW(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))
        lD4EMSource.DataSets.Add("clo", WriteCLO(aOriginalData, aSaveInFolder, aDateStart, aDateEnd))

        lD4EMSource.Save(IO.Path.Combine(aProjectFolder, "MetData.txt"))
        lD4EMSource.Clear()

        pModifiedData = Nothing
    End Sub

    'Write the SWAT precipitation file and return the timeseries used to write the file
    Private Function WritePCP(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries

        Dim lWDMconstituent As String = "PREC"
        Dim lTS As atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        'Dim lTSPREC As atcData.atcTimeseries = Nothing
        'Dim lTSPRCP As atcData.atcTimeseries = Nothing
        'Dim lTS As atcData.atcTimeseries

        '' prefer starting with daily PRCP timeseries or sub-daily PREC?
        'If lTSPRCP IsNot Nothing Then
        '    lTS = lTSPRCP
        '    lDaily = True
        'Else
        '    lTS = lTSPREC
        'End If

        Return WritePCP(lTS, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WritePCP(ByVal aTS As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWriteDaily As Boolean = True
        Dim lTS As atcTimeseries = Nothing
        If aTS IsNot Nothing Then
            lTS = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)
            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "pcp1.pcp")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(lTS)) 'Not processed by model
            lWriter.WriteLine(LatLonElev(lTS))

            'lTS.Attributes.SetValueIfMissing("Units", "in")
            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 25.4) 'in to mm
            lTS.Attributes.SetValue("Units", "mm/day")
            Dim lDate As Date
            Dim lValue As Double
            For lIndex As Integer = 1 To lTS.numValues
                Try
                    lDate = Date.FromOADate(lTS.Dates.Value(lIndex - 1))
                    lValue = lTS.Value(lIndex)
                    If Double.IsNaN(lValue) Then lValue = -99
                    If lWriteDaily Then 'Write daily format including just year and day
                        lWriter.WriteLine(YYYYddd(lDate) & f51(lValue))
                    Else 'Write sub-daily format including year, day and time
                        lWriter.WriteLine(YYYYddd(lDate) & Format(lDate, "HH:mm") & f51(lValue))
                    End If
                Catch e As Exception
                    lWriter.WriteLine(YYYYddd(lDate) & f51(-99))
                End Try
            Next
            lWriter.Close()
        End If
        Return lTS
    End Function

    Private Function WriteTMP(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "ATEM"
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)

        Return WriteTMP(lTS, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WriteTMP(ByVal aTS As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        If aTS IsNot Nothing Then
            aTS.EnsureValuesRead()
            'lTS = atcData.SubsetByDate(lTS, aDateStart, aDateEnd, Nothing)
            aTS = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", aTS)
            aTS.Attributes.SetValue("Units", "C")

            Dim lTsMax As atcTimeseries = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranMax)
            lTsMax = EnsureComplete(lTsMax, aDateStart, aDateEnd, Nothing)

            Dim lTsMin As atcTimeseries = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranMin)
            lTsMin = EnsureComplete(lTsMin, aDateStart, aDateEnd, Nothing)

            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "tmp1.tmp")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model
            lWriter.WriteLine(LatLonElev(aTS))

            'lTS.Attributes.SetValueIfMissing("Units", "F")

            For lIndex As Integer = 1 To lTsMax.numValues
                WriteTMPline(lWriter, _
                             Date.FromOADate(lTsMax.Dates.Value(lIndex - 1)), _
                             lTsMax.Value(lIndex), _
                             lTsMin.Value(lIndex))
            Next

            lWriter.Close()

            aTS = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            aTS = EnsureComplete(aTS, aDateStart, aDateEnd, Nothing)

            'Keep min and max timeseries for later use next to average
            aTS.Attributes.SetValue("tmax", lTsMax)
            aTS.Attributes.SetValue("tmin", lTsMin)
        End If
        Return aTS
    End Function

    Private Sub WriteTMPline(ByVal aWriter As IO.StreamWriter, ByVal aDate As Date, ByVal aMax As Double, ByVal aMin As Double)
        If aMax < aMin Then
            aMax = -99
            aMin = -99
        Else
            If Double.IsNaN(aMax) Then aMax = -99
            If Double.IsNaN(aMin) Then aMin = -99
        End If
        aWriter.WriteLine(YYYYddd(aDate) & f51(aMax) & f51(aMin))
    End Sub

    Private Function WriteSLR(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "SOLR" '"DSOL"
        'Dim lDaily As Boolean = True

        'Dim lTSSOLR As atcData.atcTimeseries = Nothing
        'Dim lTSDSOL As atcData.atcTimeseries = Nothing
        'For Each lTS In aWDM.DataSets
        '    Select Case lTS.Attributes.GetValue("Constituent", "")
        '        Case lWDMconstituent : lTSSOLR = lTS
        '            'Case "DSOL" : lTSDSOL = lTS
        '    End Select
        'Next

        '' prefer daily DSOL timeseries rather than compute from SOLR
        'If lTSDSOL IsNot Nothing Then
        '    lTS = lTSDSOL
        '    lDaily = True
        'Else
        '    lTS = lTSSOLR
        'End If
 
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        Return WriteSLR(lTS, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WriteSLR(ByVal aTS As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        If aTS IsNot Nothing Then
            aTS.EnsureValuesRead()
            aTS = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            aTS = EnsureComplete(aTS, aDateStart, aDateEnd, Nothing)

            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "slr.slr")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model

            Dim lDate As Date
            Dim lValue As Double
            'lTS.Attributes.SetValueIfMissing("Units", "Langley")
            aTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", aTS, 0.484 * 0.0864) 'convert Langley to MJ/m2
            aTS.Attributes.SetValue("Units", "MJ/m2/day")

            'If lDaily Then
            For lIndex As Integer = 1 To aTS.numValues
                lDate = Date.FromOADate(aTS.Dates.Value(lIndex - 1)) 'begin of interval
                lValue = aTS.Value(lIndex)
                If Double.IsNaN(lValue) Then lValue = -99
                lWriter.WriteLine(YYYYddd(lDate) & f83(lValue))
            Next
            'Else
            '    Dim lPrevDay As Integer = -1
            '    Dim lPrevDate As Date
            '    Dim lTotal As Double = 0
            '    For lIndex As Integer = 1 To lTS.numValues
            '        lDate = Date.FromOADate(lTS.Dates.Value(lIndex))
            '        Dim lDay As Integer = lDate.DayOfYear
            '        If lDay <> lPrevDay Then
            '            If lPrevDay > -1 Then
            '                lWriter.WriteLine(YYYYddd(lDate) & f83(lTotal))
            '            End If
            '            lTotal = 0
            '            lPrevDay = lDay
            '        End If

            '        lValue = lTS.Value(lIndex)
            '        If Not Double.IsNaN(lValue) Then lTotal += lValue

            '        lPrevDate = lDate
            '    Next
            'lWriter.WriteLine(YYYYddd(lDate) & f83(lTotal))
            'End If

            lWriter.Close()
        End If
        Return aTS
    End Function

    Private Function WriteWND(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "WIND"
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        Return WriteWND(lTS, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WriteWND(ByVal aTS As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        If aTS IsNot Nothing Then
            aTS.EnsureValuesRead()
            aTS = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

            aTS = EnsureComplete(aTS, aDateStart, aDateEnd, Nothing)

            'Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, lTSWIND.Attributes.GetValue("Location", "met") & ".wnd")
            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "wnd.wnd")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model

            'lTS.Attributes.SetValueIfMissing("Units", "mph")
            aTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", aTS, 0.44704) 'convert wind speed miles per hour to meters per second
            aTS.Attributes.SetValue("Units", "m/s")
            Dim lDate As Date
            Dim lValue As Double
            For lIndex As Integer = 1 To aTS.numValues
                lDate = Date.FromOADate(aTS.Dates.Value(lIndex - 1))
                lValue = aTS.Value(lIndex)
                If Double.IsNaN(lValue) Then lValue = -99
                lWriter.WriteLine(YYYYddd(lDate) & f83(lValue))
            Next
            lWriter.Close()
        End If
        Return aTS
    End Function

    'Write the SWAT Potential EvapoTranspiration file and return the timeseries used to write the file
    Private Function WritePET(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "PEVT"
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        Return WritePET(lTS, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WritePET(ByVal aTS As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        If aTS IsNot Nothing Then
            aTS.EnsureValuesRead()
            aTS = atcData.modTimeseriesMath.Aggregate(aTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)

            aTS = EnsureComplete(aTS, aDateStart, aDateEnd, Nothing)

            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "pet1.pet")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model
            lWriter.WriteLine(LatLonElev(aTS))

            'lTS.Attributes.SetValueIfMissing("Units", "in")
            aTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", aTS, 25.4)
            aTS.Attributes.SetValue("Units", "mm/day")
            Dim lDate As Date
            Dim lValue As Double
            For lIndex As Integer = 1 To aTS.numValues
                lDate = Date.FromOADate(aTS.Dates.Value(lIndex - 1))
                lValue = aTS.Value(lIndex)
                If Double.IsNaN(lValue) Then lValue = -99
                lWriter.WriteLine(YYYYddd(lDate) & f51(lValue))
            Next
            lWriter.Close()
        End If
        Return aTS
    End Function

    'Write the SWAT relative humidity file and return the timeseries used to write the file
    Private Function WriteHMD(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "DEWP"
        Dim lDew As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        Dim lTemp As atcData.atcTimeseries = GetWDMTimeseries(aWDM, "ATEM")

        Return WriteHMD(lDew, lTemp, aSaveInFolder, aDateStart, aDateEnd)
    End Function

    Private Function WriteHMD(ByVal lDew As atcTimeseries, _
                              ByVal lTemp As atcTimeseries, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lHMD As atcTimeseries = Nothing
        If lDew IsNot Nothing AndAlso lTemp IsNot Nothing Then
            lDew.EnsureValuesRead()
            lTemp.EnsureValuesRead()
            lTemp = atcData.modTimeseriesMath.Aggregate(lTemp, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            lTemp = EnsureComplete(lTemp, aDateStart, aDateEnd, Nothing)
            lTemp = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTemp)

            lDew = atcData.modTimeseriesMath.Aggregate(lDew, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            lDew = EnsureComplete(lDew, aDateStart, aDateEnd, Nothing)
            lDew = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lDew)

            Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "hmd1.hmd")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
            Dim lWriter As New IO.StreamWriter(lTextFilename)
            lWriter.WriteLine(MetHeader(lDew)) 'Not processed by model
            lWriter.WriteLine(LatLonElev(lDew))

            lHMD = lDew.Clone
            With lHMD.Attributes
                .SetValue("Constituent", "Relative Humidity")
                .SetValue("Units", "fraction")
            End With

            Dim lDate As Date
            Dim lDewValue As Double
            Dim lTempValue As Double
            Dim lValue As Double
            For lIndex As Integer = 1 To lDew.numValues
                lDate = Date.FromOADate(lDew.Dates.Value(lIndex - 1))

                lDewValue = lDew.Value(lIndex)
                lTempValue = lTemp.Value(lIndex)

                If lDewValue > lTempValue Then
                    Debug.WriteLine("dew > temp, " & DoubleToString(lDewValue) & " > " & DoubleToString(lTempValue))
                End If

                lValue = Math.Exp((17.269 * lDewValue) / (273.3 + lDewValue)) _
                       / Math.Exp((17.269 * lTempValue) / (273.3 + lTempValue))
                lHMD.Value(lIndex) = lValue
                If Double.IsNaN(lValue) Then lValue = -99
                lWriter.WriteLine(YYYYddd(lDate) & f83(lValue))
            Next
            lWriter.Close()
        End If
        Return lHMD
    End Function

    'returns the dewpoint timeseries in degrees C, does not write SWAT input
    Private Function WriteDEW(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "DEWP"
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        If lTS IsNot Nothing Then
            lTS.EnsureValuesRead()
            lTS = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

            lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)

            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTS)
            lTS.Attributes.SetValue("Units", "C")
        End If
        Return lTS
    End Function

    'return the filled cloud cover dataset, does not write SWAT input
    Private Function WriteCLO(ByVal aWDM As atcDataSourceWDM, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries
        Dim lWDMconstituent As String = "CLOU"
        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
        If lTS IsNot Nothing Then
            lTS = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

            lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)

            'lTS.Attributes.SetValueIfMissing("Units", "tenths")
            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 0.1)
            lTS.Attributes.SetValue("Units", "fraction")
        End If
        Return lTS
    End Function

    ''' <summary>
    ''' Return the modified data with this constituent if any, otherwise return the data from the given data source
    ''' </summary>
    ''' <param name="aWDM">Data source containing original data</param>
    ''' <param name="aWDMConstituentName">Name of constituent to search for</param>
    ''' <returns>uses module variable pModifiedData</returns>
    ''' <remarks>Does not yet support multiple met stations in one run</remarks>
    Private Function GetWDMTimeseries(ByVal aWDM As atcDataSource, ByVal aWDMConstituentName As String) As atcTimeseries
        ' Search modified data for given constituent
        If pModifiedData IsNot Nothing Then
            For Each lTSsearch As atcData.atcTimeseries In pModifiedData
                If lTSsearch.Attributes.GetValue("Constituent", "") = aWDMConstituentName Then Return lTSsearch
            Next
        End If
        ' Search for this data in original data source
        For Each lTSsearch As atcData.atcTimeseries In aWDM.DataSets
            If lTSsearch.Attributes.GetValue("Constituent", "") = aWDMConstituentName Then Return lTSsearch
        Next
        Logger.Dbg("No " & aWDMConstituentName & " available in " & aWDM.Specification)
        Return Nothing
    End Function

    Private Function MetHeader(ByVal aTimeseries As atcData.atcTimeseries) As String
        With aTimeseries.Attributes
            Return .GetValue("Constituent", "") & " at " _
                 & .GetValue("Location", "") & ", " _
                 & .GetValue("STANAM", "") _
                 & " data from BASINS via D4EM/EDDT"
        End With
    End Function

    Private Function LatLonElev(ByVal aTimeseries As atcData.atcTimeseries) As String
        With aTimeseries.Attributes
            'Lat and Lon not processed by model
            Return ("LatDeg " & DoubleToString(CDbl(.GetValue("Latitude", "0")), 5) & vbCrLf _
                  & "LonDeg " & DoubleToString(CDbl(.GetValue("Longitude", "0")), 5) & vbCrLf _
                  & "Elev(m)" & Format(CInt(.GetValue("Elevation", "0")), "00000"))
            'TODO: populate station list with elevation, add to WDM attributes, convert to meters
        End With
    End Function

    Private Function f51(ByVal aValue As Double) As String
        If aValue < 0 Then
            Return Format(aValue, "#0.0").PadLeft(5)
        Else
            Return Format(aValue, "##0.0").PadLeft(5)
        End If
    End Function

    Private Function f83(ByVal aValue As Double) As String
        If aValue < 0 Then
            Return Format(aValue, "##0.000").PadLeft(8)
        Else
            Return Format(aValue, "###0.000").PadLeft(8)
        End If
    End Function

    Private Function YYYYddd(ByVal aDate As Date) As String
        Return aDate.Year & Format(aDate.DayOfYear, "000")
    End Function

    Private Function EnsureComplete(ByVal aTimeseries As atcTimeseries, _
                                    ByVal aStartDate As Double, _
                                    ByVal aEndDate As Double, _
                                    ByVal aDataSource As atcDataSource) As atcTimeseries

        Dim lNewTimeseries As atcTimeseries
        Dim lReport As New Text.StringBuilder

        lReport.AppendLine("Complete date range from " & Date.FromOADate(aStartDate) & " to " & Date.FromOADate(aEndDate))

        Dim lOverlapTimeseries As atcTimeseries = SubsetByDate(aTimeseries, aStartDate, aEndDate, aDataSource)
        If lOverlapTimeseries.numValues > 0 Then
            If Math.Abs(lOverlapTimeseries.Dates.Value(0) - aStartDate) < JulianSecond AndAlso _
               Math.Abs(lOverlapTimeseries.Dates.Value(lOverlapTimeseries.numValues) - aEndDate) < JulianSecond Then
                'Requested time 
                lNewTimeseries = lOverlapTimeseries
            Else
                lNewTimeseries = NewTimeseries(aStartDate, aEndDate, atcTimeUnit.TUDay, 1, , GetNaN)
                'First copy overlapping values into new timeseries
                Dim lOverlapStart As Integer = FindDateAtOrAfter(lNewTimeseries.Dates.Values, lOverlapTimeseries.Dates.Value(1))
                System.Array.Copy(lOverlapTimeseries.Values, 1, lNewTimeseries.Values, lOverlapStart, lOverlapTimeseries.numValues)
            End If
        Else
            lNewTimeseries = NewTimeseries(aStartDate, aEndDate, atcTimeUnit.TUDay, 1, , GetNaN)
        End If

        'Fill in missing values from same date in another year of input data
        Dim lOldIndex As Integer = 1
        Dim lOldDateArray(5) As Integer
        Dim lNewDateArray(5) As Integer

        For lNewIndex As Integer = 1 To lNewTimeseries.numValues
            If Double.IsNaN(lNewTimeseries.Value(lNewIndex)) Then
                J2Date(lNewTimeseries.Dates.Value(lNewIndex), lNewDateArray)
                Dim lSearchOldIndex As Integer = lOldIndex
                While lSearchOldIndex <= aTimeseries.numValues
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
                            lReport.AppendLine("Missing value for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & ", copied from " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
                            GoTo FoundOldDate
                        End If
                    End If
                    lSearchOldIndex += 1
                End While
                lSearchOldIndex = 1
                While lSearchOldIndex < lOldIndex
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
                            GoTo FoundOldDate
                        End If
                    End If
                    lSearchOldIndex += 1
                End While

                lSearchOldIndex = lOldIndex
                Dim lNearestIndex As Integer = lOldIndex
                Dim lNearestDays As Integer = 500
                Dim lDays As Integer
                While lSearchOldIndex <= aTimeseries.numValues
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) Then
                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
                        End If
                    End If
                    lSearchOldIndex += 1
                End While
                lSearchOldIndex = 1
                While lSearchOldIndex < lOldIndex
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) Then
                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
                        End If
                    End If
                    lSearchOldIndex += 1
                End While

                lSearchOldIndex = lNearestIndex
                J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                lReport.AppendLine("Value not found for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & " using value from " & lNearestDays & " days away: " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
FoundOldDate:
                lNewTimeseries.Value(lNewIndex) = aTimeseries.Value(lSearchOldIndex)
                lNewTimeseries.ValueAttributes(lNewIndex).SetValue("Original Date", aTimeseries.Dates.Value(lSearchOldIndex))
            End If
        Next

        lNewTimeseries.Attributes.SetValue("Description", lReport.ToString)
        Return lNewTimeseries

    End Function

    '    Private Sub EnsureComplete(ByVal aStartDate As Double, _
    '                               ByVal aEndDate As Double, _
    '                               ByVal aDataSource As atcDataSource, _
    '                               ByVal aTimeseriesOut As atcDataGroup, _
    '                               ByVal ParamArray aTimeseriesIn() As atcTimeseries)

    '        Dim lLastTsIndex As Integer = aTimeseriesIn.GetUpperBound(0)
    '        Dim lTsIndex As Integer
    '        Dim lOverlapTimeseries(lLastTsIndex) As atcTimeseries
    '        Dim lNewTimeseries As atcTimeseries
    '        Dim lReport As New Text.StringBuilder

    '        lReport.AppendLine("Complete date range from " & DoubleToString(aStartDate) & " to " & DoubleToString(aEndDate) & " for " & lLastTsIndex - 1 & " timeseries")

    '        For lTsIndex = 0 To lLastTsIndex
    '            lOverlapTimeseries(lTsIndex) = SubsetByDate(aTimeseriesIn(lTsIndex), aStartDate, aEndDate, aDataSource)
    '        Next


    '        If Math.Abs(lOverlapTimeseries.Dates.Value(0) - aStartDate) < JulianSecond AndAlso _
    '           Math.Abs(lOverlapTimeseries.Dates.Value(lOverlapTimeseries.numValues) - aEndDate) < JulianSecond Then
    '            'Requested time 
    '            lNewTimeseries = lOverlapTimeseries
    '        Else
    '            lNewTimeseries = NewTimeseries(aStartDate, aEndDate, atcTimeUnit.TUDay, 1, , GetNaN)

    '            'First copy overlapping values into new timeseries
    '            Dim lOverlapStart As Integer = FindDateAtOrAfter(lNewTimeseries.Dates.Values, lOverlapTimeseries.Dates.Value(1))
    '            System.Array.Copy(lOverlapTimeseries.Values, 1, lNewTimeseries.Values, lOverlapStart, lOverlapTimeseries.numValues)
    '        End If

    '        'Fill in missing values from same date in another year of input data
    '        Dim lOldIndex As Integer = 1
    '        Dim lOldDateArray(5) As Integer
    '        Dim lNewDateArray(5) As Integer

    '        For lNewIndex As Integer = 1 To lNewTimeseries.numValues
    '            If Double.IsNaN(lNewTimeseries.Value(lNewIndex)) Then
    '                J2Date(lNewTimeseries.Dates.Value(lNewIndex), lNewDateArray)
    '                Dim lSearchOldIndex As Integer = lOldIndex
    '                While lSearchOldIndex <= aTimeseries.numValues
    '                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
    '                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
    '                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
    '                            lReport.AppendLine("Missing value for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & ", copied from " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
    '                            GoTo FoundOldDate
    '                        End If
    '                    End If
    '                    lSearchOldIndex += 1
    '                End While
    '                lSearchOldIndex = 1
    '                While lSearchOldIndex < lOldIndex
    '                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
    '                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
    '                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
    '                            GoTo FoundOldDate
    '                        End If
    '                    End If
    '                    lSearchOldIndex += 1
    '                End While

    '                lSearchOldIndex = lOldIndex
    '                Dim lNearestIndex As Integer = lOldIndex
    '                Dim lNearestDays As Integer = 500
    '                Dim lDays As Integer
    '                While lSearchOldIndex <= aTimeseries.numValues
    '                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
    '                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
    '                        If lOldDateArray(1) = lNewDateArray(1) Then
    '                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
    '                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
    '                        End If
    '                    End If
    '                    lSearchOldIndex += 1
    '                End While
    '                lSearchOldIndex = 1
    '                While lSearchOldIndex < lOldIndex
    '                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
    '                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
    '                        If lOldDateArray(1) = lNewDateArray(1) Then
    '                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
    '                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
    '                        End If
    '                    End If
    '                    lSearchOldIndex += 1
    '                End While

    '                lSearchOldIndex = lNearestIndex
    '                J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
    '                lReport.AppendLine("Value not found for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & " using value from " & lNearestDays & " days away: " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
    'FoundOldDate:
    '                lNewTimeseries.Value(lNewIndex) = aTimeseries.Value(lSearchOldIndex)
    '                lNewTimeseries.ValueAttributes(lNewIndex).SetValue("Original Date", aTimeseries.Dates.Value(lSearchOldIndex))
    '            End If
    '        Next

    '        lNewTimeseries.Attributes.SetValue("Description", lReport.ToString)
    '        Return lNewTimeseries

    '    End Sub

End Module
