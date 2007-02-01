Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcSynopticAnalysisPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Synoptic"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataManager As atcDataManager, _
                     Optional ByVal aDataGroup As atcDataGroup = Nothing) _
                     As Object 'System.Windows.Forms.Form

        Dim lForm As New frmSynoptic
        lForm.Initialize(aDataManager, aDataGroup)
        Return lForm

        'Dim lFilename As String = IO.Path.GetTempPath() & "SynopticAnalysis.txt"
        'Save(aDataManager, aDataGroup, lFilename)
        'Process.Start(lFilename)
        'Return Nothing
    End Function

    Public Overrides Sub Save(ByVal aDataManager As atcDataManager, _
                    ByVal aDataGroup As atcDataGroup, _
                    ByVal aFileName As String, _
                    ByVal ParamArray aOption() As String)
        If aDataGroup Is Nothing Then
            aDataGroup = New atcDataGroup
        End If
        If aDataGroup.Count = 0 Then
            aDataManager.UserSelectData("Select data for Synoptic Analysis", aDataGroup)
            If aDataGroup.Count = 0 Then
                Dim lMsg As String = "No data sets selected for synoptic analysis"
                SaveFileString(aFileName, lMsg)
                Logger.Dbg(lMsg)
                Exit Sub
            End If
        End If

        Dim lThreshold As Double = 0 '0.01 'TODO: allow this to be an option
        Dim lDaysGapAllowed As Double = JulianSecond + 4 / 24  'TODO: allow this to be an option
        Dim lHighEvents As Boolean = True
        Dim lDuration As Double
        Dim lVolume As Double
        Dim lTimeSince As Double = 0
        Dim lReport As New IO.StreamWriter(aFileName)

        lReport.WriteLine("Synoptic Analysis of " & aDataGroup.Count & " Data Sets")
        For Each lDataSet As atcTimeseries In aDataGroup
            lDataSet.EnsureValuesRead()
            lReport.WriteLine()
            lReport.WriteLine("Location:" & lDataSet.Attributes.GetValue("Location"))
            lReport.WriteLine("Time Span:" & DateString(lDataSet.Dates.Value(1)) & _
                                    " to " & DateString(lDataSet.Dates.Value(lDataSet.Dates.numValues)))

            lReport.WriteLine(vbCrLf & "Storm Events" & vbCrLf)

            lReport.WriteLine("Storm " & vbTab & "          " & vbTab & "    " & vbTab & "Duration" & vbTab & "Total " & vbTab & "Average  " & vbTab & "Maximum  " & vbTab & "Time Since Last")
            lReport.WriteLine("Number" & vbTab & "Date      " & vbTab & "Hour" & vbTab & "(Hours) " & vbTab & "Volume" & vbTab & "Intensity" & vbTab & "Intensity" & vbTab & "(Hours)        ")

            Dim lStorms As atcDataGroup = atcEvents.EventSplit(lDataSet, Nothing, lThreshold, lDaysGapAllowed, lHighEvents)
            Dim lStormIndex As Integer = 0
            For Each lStorm As atcTimeseries In lStorms
                lStormIndex += 1

                'Storm Number, Date, Hour
                lReport.Write(lStormIndex & vbTab & DateTabHour(lStorm.Dates.Value(1)))

                'Duration (hours) 'TODO: dont hard code hours?
                'TODO: 1 + should be 1 time unit, not 1 hour
                lDuration = 1 + 24 * (lStorm.Dates.Value(lStorm.Dates.numValues) - lStorm.Dates.Value(1))
                lReport.Write(vbTab & StrPad(CInt(lDuration), 8))

                'Volume
                lVolume = lStorm.Attributes.GetValue("Sum")
                lReport.Write(vbTab & StrPad(Format(lStorm.Attributes.GetValue("Sum"), "0.00"), 6))

                'Average Intensity
                lReport.Write(vbTab & StrPad(Format(lVolume / lDuration, "0.00"), 9))

                'Maximum Intensity
                lReport.Write(vbTab & StrPad(Format(lStorm.Attributes.GetValue("Max"), "0.00"), 9))

                'Time since previous event
                lTimeSince = lStorm.Attributes.GetValue("EventTimeSincePrevious", 0)
                If lTimeSince > 0 Then
                    lReport.Write(vbTab & StrPad(Format(lTimeSince * 24, "#,###"), 9))
                End If

                lReport.WriteLine()
            Next
        Next
        lReport.Close()
        Logger.Dbg("SynopticAnalysis Complete, Results in file '" & aFileName & "'")
    End Sub

    Private Function DateString(ByVal aJulianDay As Double) As String
        Dim d(5) As Integer
        J2Date(aJulianDay, d)
        Return Format(d(0), "0000") & "/" & _
               Format(d(1), "00") & "/" & _
               Format(d(2), "00")
    End Function

    Private Function DateTabHour(ByVal aJulianDay As Double) As String
        Dim d(5) As Integer
        J2Date(aJulianDay, d)
        Return Format(d(0), "0000") & "/" & _
               Format(d(1), "00") & "/" & _
               Format(d(2), "00") & vbTab & _
               Format(d(3), "00")
    End Function

    Public Shared Function ComputeEvents(ByVal aDataGroup As atcDataGroup, ByVal aThreshold As Double, ByVal aDaysGapAllowed As Double, ByVal aHighEvents As Boolean) As atcDataGroup
        ComputeEvents = New atcDataGroup
        If Not aDataGroup Is Nothing Then
            For Each lDataSet As atcTimeseries In aDataGroup
                Dim lEvents As atcDataGroup = atcEvents.EventSplit(lDataSet, Nothing, aThreshold, aDaysGapAllowed, aHighEvents)
                ComputeEvents.AddRange(lEvents)
            Next
        End If
    End Function

End Class
