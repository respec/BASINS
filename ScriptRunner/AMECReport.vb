Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcBasinsObsWQ
Imports atcSeasons

Imports MapWindow.Interfaces
Imports MapWinUtility
Imports mapwinGIS

Public Module AMECReport
    Private gProjectDir As String = "G:\Projects\AMEC\Phenol\"
    Private gOutputDir As String = gProjectDir & "Outfiles\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(gProjectDir)
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lSDate As Double = Date.Parse("1/1/1973").ToOADate
        Dim lEDate As Double = Date.Parse("1/1/2003").ToOADate
        Dim lLocnArray() As String = {"LOW", "UP"}
        Dim lSimDsnArray() As String = {"834", "813"}
        Dim lCurObsCons As String = "Phenols_mg/L"

        Dim lSeasonStartArray() As String = {"11/1", "4/1", "6/1", "9/1", "1/1"}
        Dim lSeasonEndArray() As String = {"4/1", "6/1", "9/1", "11/1", "12/31"}
        Dim lSeasonNameArray() As String = {"Winter", "Spring", "Summer", "Fall", "Annual"}

        'open WDM file
        Dim lWdmFileName As String = gProjectDir & "Phenol-Mus85.wdm"
        Dim lWdmDataSource As New atcDataSourceWDM()
        lWdmDataSource.Open(lWdmFileName)
        'open DBF file
        Dim lDbfFileName As String = gProjectDir & "Phenol.dbf"
        Dim lDbfDataSource As New atcDataSourceBasinsObsWQ()
        lDbfDataSource.Open(lDbfFileName)

        Dim lConcentrationsTable As New atcTableDelimited
        With lConcentrationsTable
            .Delimiter = vbTab
            .NumFields = (lLocnArray.Length * lSeasonStartArray.Length * 4) + 1
            .NumHeaderRows = 3
            For lLocationIndex As Integer = 0 To lLocnArray.GetUpperBound(0)
                If lLocationIndex > 0 Then
                    .Header(1) &= vbTab
                    .Header(2) &= vbTab
                    .Header(3) &= vbTab
                End If
                .Header(1) &= lLocnArray(lLocationIndex)
                For lSeasonIndex As Integer = 0 To lSeasonNameArray.GetUpperBound(0)
                    .Header(1) &= StrDup(4, vbTab)
                    .Header(2) &= lSeasonNameArray(lSeasonIndex) & " (" & lSeasonStartArray(lSeasonIndex) & "-" & lSeasonEndArray(lSeasonIndex) & ")" & StrDup(4, vbTab)
                    .Header(3) &= "Simulated" & vbTab & vbTab & "Observed" & vbTab & vbTab
                Next
            Next
            Dim lFieldIndex As Integer = 1
            For lIndex As Integer = 0 To lLocnArray.GetUpperBound(0)
                'this will need a better search if obs data contains multiple constituents
                Dim lObsData As atcTimeseries = lDbfDataSource.DataSets.FindData("LOCN", lLocnArray(lIndex))(0)
                Dim lSimData As atcTimeseries = lWdmDataSource.DataSets.FindData("ID", lSimDsnArray(lIndex))(0)
                Dim lColumnStart As Integer = lIndex * lSeasonStartArray.Length * 4
                If lIndex > 0 Then lColumnStart += 1 'TODO: more than 2 locations
                For lSeasonIndex As Integer = 0 To lSeasonStartArray.GetUpperBound(0)
                    PutTimeseriesInGrid(lSimData, lConcentrationsTable, lFieldIndex, _
                                        lSeasonNameArray(lSeasonIndex), _
                                        lSeasonStartArray(lSeasonIndex), _
                                        lSeasonEndArray(lSeasonIndex), True)
                    lFieldIndex += 2

                    PutTimeseriesInGrid(lObsData, lConcentrationsTable, lFieldIndex, _
                                        lSeasonNameArray(lSeasonIndex), _
                                        lSeasonStartArray(lSeasonIndex), _
                                        lSeasonEndArray(lSeasonIndex), False)
                    lFieldIndex += 2
                Next
                lFieldIndex += 1
            Next
            Dim lWdmFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lWdmFileName)
            Dim lMsg As New atcUCI.HspfMsg
            lMsg.Open("hspfmsg.mdb")
            'Dim lHspfUci As New atcUCI.HspfUci
            'lHspfUci.FastReadUciForStarter(lMsg, "Phenol-Mus85" & ".uci")

            'Dim lReport As String = "Observed Concentrations for " & lCurObsCons & vbCrLf _
            '                      & "Dates: " & lHspfUci.GlobalBlock.RunPeriod & vbCrLf _
            '                      & "   Run Made: " & lWdmFileInfo.LastWriteTime & vbCrLf _
            '                      & "   Run Title: " & lHspfUci.GlobalBlock.RunInf.Value & vbCrLf & vbCrLf _
            '                      & .ToString

            Dim lReport As String = "Observed Concentrations for " & lCurObsCons & vbCrLf _
                                  & "Dates: " & vbCrLf _
                                  & "   Run Made: " & lWdmFileInfo.LastWriteTime & vbCrLf _
                                  & "   Run Title: " & vbCrLf & vbCrLf _
                                  & .ToString

            SaveFileString(gOutputDir & "Seasonal_Phenols_Summary.txt", lReport)
        End With
    End Sub

    Private Sub PutTimeseriesInGrid(ByVal aTimeseries As atcTimeseries, _
                                    ByVal aGrid As atcTableDelimited, _
                                    ByVal aGridColumn As Integer, _
                                    ByVal aSeasonName As String, _
                                    ByVal aSeasonStart As String, _
                                    ByVal aSeasonEnd As String, _
                                    ByVal aSubtractSmall As Boolean)
        Dim lValues() As Double
        Dim lValueIndex As Integer
        If aSeasonName = "Annual" Then
            lValues = aTimeseries.Values.Clone
        Else
            Dim lSeasonData As atcSeasonsYearSubset
            Dim lStartMonthDay() As String = aSeasonStart.Split("/")
            Dim lEndMonthDay() As String = aSeasonEnd.Split("/")
            lSeasonData = New atcSeasonsYearSubset(lStartMonthDay(0), lStartMonthDay(1), lEndMonthDay(0), lEndMonthDay(1))
            Dim lSimTimser As atcTimeseries = lSeasonData.Split(aTimeseries, Nothing).ItemByKey(1)
            lValues = lSimTimser.Values
        End If
        Dim lNumNonMissingValues As Integer = 0
        Dim lNonMissingValues(lValues.GetUpperBound(0)) As Double
        For lValueIndex = 0 To lValues.GetUpperBound(0)
            If Not Double.IsNaN(lValues(lValueIndex)) Then
                lNonMissingValues(lNumNonMissingValues) = lValues(lValueIndex)
                lNumNonMissingValues += 1
            End If
        Next
        ReDim Preserve lNonMissingValues(lNumNonMissingValues - 1)
        Array.Sort(lNonMissingValues)
        With aGrid
            .CurrentRecord = 1
            .FieldName(aGridColumn) = "Value"
            .FieldName(aGridColumn + 1) = "Prob"
            Dim lDivideBy As Integer = lNumNonMissingValues + 1
            If aSubtractSmall Then
                Dim lSmallValues As Integer = 0
                For lValueIndex = 0 To lNumNonMissingValues - 1
                    If lNonMissingValues(lValueIndex) > 0.00005 Then
                        .Value(aGridColumn) = DoubleToString(lNonMissingValues(lValueIndex), , "#0.0000")
                        .Value(aGridColumn + 1) = DoubleToString((lValueIndex + 1 - lSmallValues) / (lDivideBy - lSmallValues), , "#0.0000")
                        .CurrentRecord += 1
                    Else
                        lSmallValues += 1
                    End If
                Next
            Else
                For lValueIndex = 0 To lNumNonMissingValues - 1
                    If lNonMissingValues(lValueIndex) > 0.00005 Then
                        .Value(aGridColumn) = DoubleToString(lNonMissingValues(lValueIndex), , "#0.0000")
                        .Value(aGridColumn + 1) = DoubleToString((lValueIndex) / (lNumNonMissingValues), , "#0.0000")
                        .CurrentRecord += 1
                    End If
                Next
            End If
        End With
    End Sub

End Module



'      (Set curRow 1)
'      (+= j 2)
'      (If (= lSeasonCnt 1)
'        (Grid j curRow curLocn)
'      )
'      (++ curRow)
'      (Grid j curRow (+ curSeasonName " (" curSeasonDates ")"))
'      (Grid j (++ curRow)  "Simulated")
'      (Grid j (++ curRow)  "Value")
'      (Grid (+ j 1) curRow "Prob")
'      (Set valCnt (+ (Len lSimVals) 1))
'      (Set m 0)
'      (For k = 1 to (Len lSimVals)
'        (Set val (ArrayItem lSimVals k))
'        (If (> val 0.00005)
'          (Grid j (++ curRow) (Format val "#0.0000"))
'          (Grid (+ j 1) curRow (Format (/ (- k m) (- valCnt m)) "#0.0000"))
'          (Else (++ m))
'        )
'      )
'      (Set curRow 2)
'      (+= j 2)
'      (Grid j (++ curRow) "Observed")
'      (Grid j (++ curRow)  "Value")
'      (Grid (+ j 1) curRow "Prob")
'      (Set valCnt (+ (Len lObsVals) 1))
'      (For k = 1 to (Len lObsVals)
'        (Set val (ArrayItem lObsVals k))
'        (If (> val 0.00005)
'          (Grid j (++ curRow) (Format val "#0.0000"))
'          (Grid (+ j 1) curRow (Format (/ k valCnt) "#0.0000"))        
'        )
'      )
'      (Unset lSimSubSet)
'      (Unset lObsSubSet)
'    )
'  )
'  (+= ReportString (Grid AsText (Chr 9) CRLF))
'  (Set curObsCons (Mid curObsCons 1 (InStr curObsCons "_")))
'  (SaveFile (+ gOutputDir "Seasonal_" curObsCons "Summary.txt") ReportString)
')

