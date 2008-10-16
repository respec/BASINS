Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcBasinsObsWQ
Imports atcSeasons

Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module AMECReport
    Private gProjectDir As String = "C:\test\Phenol\"
    Private gOutputDir As String = gProjectDir & "Outfiles\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lTab As String = Microsoft.VisualBasic.vbTab
        Dim lCrLf As String = Microsoft.VisualBasic.vbCrLf ' Chr(13) & Chr(10)
        Dim lDateRange As String = "1/1/1973 - 1/1/2003" 'Edit this to match observed and simulated data, TODO: automatically extract from UCI file
        Dim lLocnArray() As String = {"LOW", "UP"}
        Dim lSimDsnArray() As String = {"834", "813"}
        Dim lCurObsCons As String = "Phenols_mg/L"
        Dim lWdmFileName As String = gProjectDir & "Phenol-Mus85.wdm"
        Dim lDbfFileName As String = gProjectDir & "Phenol.dbf"
        Dim lReportFilename As String = gOutputDir & "Seasonal_Phenols_Summary.txt"

        Dim lSeasonStartArray() As String = {"11/1", "4/1", "6/1", "9/1", "1/1"}
        Dim lSeasonEndArray() As String = {"4/1", "6/1", "9/1", "11/1", "12/31"}
        Dim lSeasonNameArray() As String = {"Winter", "Spring", "Summer", "Fall", "Annual"}

        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Project Dir", gProjectDir)
            .Add("Output Dir", gOutputDir)
            .Add("Date Range", lDateRange)
            .Add("Constituent", lCurObsCons)
            .Add("WDM file name", lWdmFileName)
            .Add("Simulated DSNs", String.Join(",", lSimDsnArray))
            .Add("DBF file name", lDbfFileName)
            .Add("Locations", String.Join(",", lLocnArray))
            .Add("Save Report As", lReportFilename)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("User Specified Parameters", lUserParms) Then
            With lUserParms
                gProjectDir = .ItemByKey("Project Dir")
                gOutputDir = .ItemByKey("Output Dir")
                lDateRange = .ItemByKey("Date Range")
                lCurObsCons = .ItemByKey("Constituent")
                lWdmFileName = .ItemByKey("WDM file name")
                lSimDsnArray = .ItemByKey("Simulated DSNs").ToString.Split(",")
                lDbfFileName = .ItemByKey("DBF file name")
                lLocnArray = .ItemByKey("Locations").ToString.Split(",")
                lReportFilename = .ItemByKey("Save Report As")
            End With

            ChDriveDir(gProjectDir)

            'open WDM file
            Dim lWdmDataSource As New atcDataSourceWDM()
            lWdmDataSource.Open(lWdmFileName)
            'open DBF file
            Dim lDbfDataSource As New atcDataSourceBasinsObsWQ()
            lDbfDataSource.Open(lDbfFileName)

            Dim lConcentrationsTable As New atcTableDelimited
            With lConcentrationsTable
                .Delimiter = lTab
                .NumFields = (lLocnArray.Length * lSeasonStartArray.Length * 4) + 1
                .NumHeaderRows = 3
                For lLocationIndex As Integer = 0 To lLocnArray.GetUpperBound(0)
                    If lLocationIndex > 0 Then
                        .Header(1) &= lTab
                        .Header(2) &= lTab
                        .Header(3) &= lTab
                    End If
                    .Header(1) &= lLocnArray(lLocationIndex)
                    For lSeasonIndex As Integer = 0 To lSeasonNameArray.GetUpperBound(0)
                        .Header(1) &= lTab & lTab & lTab & lTab
                        .Header(2) &= lSeasonNameArray(lSeasonIndex) & " (" & lSeasonStartArray(lSeasonIndex) & "-" & lSeasonEndArray(lSeasonIndex) & ")" & lTab & lTab & lTab & lTab
                        .Header(3) &= "Simulated" & lTab & lTab & "Observed" & lTab & lTab
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
                                            lSeasonEndArray(lSeasonIndex))
                        lFieldIndex += 2

                        PutTimeseriesInGrid(lObsData, lConcentrationsTable, lFieldIndex, _
                                            lSeasonNameArray(lSeasonIndex), _
                                            lSeasonStartArray(lSeasonIndex), _
                                            lSeasonEndArray(lSeasonIndex))
                        lFieldIndex += 2
                    Next
                    lFieldIndex += 1
                Next

                Dim lWdmFileInfo As System.IO.FileInfo = New System.IO.FileInfo(lWdmFileName)
                'Dim lMsg As New atcUCI.HspfMsg
                'lMsg.Open("hspfmsg.mdb")
                'Dim lHspfUci As New atcUCI.HspfUci
                'lHspfUci.FastReadUciForStarter(lMsg, IO.Path.ChangeExtension(lWdmFileName, ".uci"))
                'Dim lReport As String = "Observed Concentrations for " & lCurObsCons & vbCrLf _
                '                      & "Dates: " & lHspfUci.GlobalBlock.RunPeriod & vbCrLf _
                '                      & "   Run Made: " & lWdmFileInfo.LastWriteTime & vbCrLf _
                '                      & "   Run Title: " & lHspfUci.GlobalBlock.RunInf.Value & vbCrLf & vbCrLf _
                '                      & .ToString

                Dim lReport As String = "Observed Concentrations for " & lCurObsCons & lCrLf _
                                      & "Dates: " & lDateRange & lCrLf _
                                      & "   Run Made: " & lWdmFileInfo.LastWriteTime & lCrLf _
                                      & .ToString

                SaveFileString(lReportFilename, lReport)
                Logger.Message("Wrote " & lReportFilename, "AMEC Report", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.None,Windows.Forms.DialogResult.OK)
            End With
        End If
    End Sub

    Private Sub PutTimeseriesInGrid(ByVal aTimeseries As atcTimeseries, _
                                    ByVal aGrid As atcTableDelimited, _
                                    ByVal aGridColumn As Integer, _
                                    ByVal aSeasonName As String, _
                                    ByVal aSeasonStart As String, _
                                    ByVal aSeasonEnd As String)
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
            If Not Double.IsNaN(lValues(lValueIndex)) AndAlso lValues(lValueIndex) > 0.00005 Then
                lNonMissingValues(lNumNonMissingValues) = lValues(lValueIndex)
                lNumNonMissingValues += 1
            End If
        Next
        ReDim Preserve lNonMissingValues(lNumNonMissingValues - 1)
        System.Array.Sort(lNonMissingValues)
        With aGrid
            .CurrentRecord = 1
            .FieldName(aGridColumn) = "Value"
            .FieldName(aGridColumn + 1) = "Prob"
            Dim lNonMissingIndex As Integer = 1
            Dim lDivideBy As Integer = lNumNonMissingValues + 1
            For lValueIndex = 0 To lNumNonMissingValues - 1
                .Value(aGridColumn) = DoubleToString(lNonMissingValues(lValueIndex), , "#0.0000")
                .Value(aGridColumn + 1) = DoubleToString(lNonMissingIndex / lDivideBy, , "#0.0000")
                .CurrentRecord += 1
                lNonMissingIndex += 1
            Next
        End With
    End Sub
End Module