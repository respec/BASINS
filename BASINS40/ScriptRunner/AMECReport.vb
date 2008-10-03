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
            .NumFields = (lLocnArray.Length * lSeasonStartArray.Length * 4) + 1
            .NumHeaderRows = 3
            For lLocationIndex As Integer = 0 To lLocnArray.Length - 1
                If lLocationIndex > 0 Then
                    .Header(1) &= StrDup((lSeasonStartArray.Length * 2) + 1, vbTab)
                    .Header(2) &= vbTab
                    .Header(3) &= vbTab
                End If
                .Header(1) &= lLocnArray(lLocationIndex)
                For lSeasonIndex As Integer = 0 To lSeasonNameArray.Length - 1
                    .Header(2) &= lSeasonNameArray(lSeasonIndex) & StrDup(3, vbTab)
                    .Header(3) &= "Simulated" & vbTab & "Observed" & vbTab
                Next
            Next
            .Delimiter = vbTab
            Dim lFieldIndex As Integer = 1
            For lIndex As Integer = 0 To lLocnArray.Length - 1
                'this will need a better search if obs data contains multiple constituents
                Dim lObsData As atcTimeseries = lDbfDataSource.DataSets.FindData("LOCN", lLocnArray(lIndex))(0)
                Dim lSimData As atcTimeseries = lWdmDataSource.DataSets.FindData("ID", lSimDsnArray(lIndex))(0)
                Dim lColumnStart As Integer = lIndex * lSeasonStartArray.Length * 4
                If lIndex > 0 Then lColumnStart += 1 'TODO: more than 2 locations
                For lSeasonIndex As Integer = 0 To lSeasonStartArray.Length - 1
                    Dim lSeasonName As String = lSeasonNameArray(lSeasonIndex)
                    Dim lSeasonStart As String = lSeasonStartArray(lSeasonIndex)
                    Dim lSeasonEnd As String = lSeasonEndArray(lSeasonIndex)
                    Dim lSimValues() As Double
                    Dim lObsValues() As Double
                    If lSeasonName = "Annual" Then
                        lSimValues = lSimData.Values.Clone
                        lObsValues = lObsData.Values.Clone
                    Else
                        Dim lSeasonData As atcSeasonsYearSubset
                        Dim lStartMonthDay() As String = lSeasonStart.Split("/")
                        Dim lEndMonthDay() As String = lSeasonEnd.Split("/")
                        lSeasonData = New atcSeasonsYearSubset(lStartMonthDay(0), lStartMonthDay(1), lEndMonthDay(0), lEndMonthDay(1))
                        Dim lSimTimser As atcTimeseries = lSeasonData.Split(lSimData, Nothing)(1)
                        lSimValues = lSimTimser.Values
                        Dim lObsTimser As atcTimeseries = lSeasonData.Split(lObsData, Nothing)(1)
                        lObsValues = lObsTimser.Values
                    End If
                    Array.Sort(lSimValues)
                    .CurrentRecord = 1
                    .FieldName(lFieldIndex) = "Value"
                    .FieldName(lFieldIndex + 1) = "Prob"
                    For lValueIndex As Integer = 0 To lSimValues.Length - 1
                        If lSimValues(lValueIndex) > 0.00005 Then
                            .Value(lFieldIndex) = DoubleToString(lSimValues(lValueIndex), , "#0.0000")
                            .Value(lFieldIndex + 1) = DoubleToString((lValueIndex) / (lSimValues.Length), , "#0.0000")
                            .CurrentRecord += 1
                        End If
                    Next
                    lFieldIndex += 2
                    Array.Sort(lObsValues)
                    .CurrentRecord = 1
                    .FieldName(lFieldIndex) = "Value"
                    .FieldName(lFieldIndex + 1) = "Prob"
                    For lValueIndex As Integer = 0 To lObsValues.Length - 1
                        If lObsValues(lValueIndex) > 0.00005 Then
                            .Value(lFieldIndex) = DoubleToString(lObsValues(lValueIndex), , "#0.0000")
                            .Value(lFieldIndex + 1) = DoubleToString((lValueIndex) / (lObsValues.Length), , "#0.0000")
                            .CurrentRecord += 1
                        End If
                    Next
                    lFieldIndex += 2
                Next
                lFieldIndex += 1
            Next
            SaveFileString(gOutputDir & "Seasonal_Phenols_Summary.txt", .ToString)
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

