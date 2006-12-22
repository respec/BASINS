Imports atcData
Imports atcUtility

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
        Dim lFilename As String = IO.Path.GetTempPath() & "SynopticAnalysis.txt"
        Save(aDataManager, aDataGroup, lFilename)
        Process.Start(lFilename)
        Return Nothing
    End Function

    Public Overrides Sub Save(ByVal aDataManager As atcDataManager, _
                    ByVal aDataGroup As atcDataGroup, _
                    ByVal aFileName As String, _
                    ByVal ParamArray aOption() As String)

        If aDataGroup Is Nothing OrElse aDataGroup.Count = 0 Then
            SaveFileString(aFileName, "No data sets selected for synoptic analysis")
        Else
            Dim lThreshold As Double = 0.01
            Dim lHighEvents As Boolean = True
            Dim lReport As New IO.StreamWriter(aFileName)
            lReport.WriteLine("Synoptic Analysis of " & aDataGroup.Count & " Data Sets")
            For Each lDataSet As atcTimeseries In aDataGroup
                lReport.WriteLine()
                lReport.WriteLine("Location:" & lDataSet.Attributes.GetValue("Location"))
                lReport.WriteLine("Time Span:" & DateString(lDataSet.Dates.Value(1)) & " to " & DateString(lDataSet.Dates.Value(lDataSet.Dates.numValues)))

                lReport.WriteLine(vbCrLf & "Storm Events" & vbCrLf)

                lReport.WriteLine("Storm #" & vbTab & "Date      " & vbTab & "Hour" & vbTab & "Hours" & vbTab & "Volume")
                Dim lStorms As atcDataGroup = atcEvents.EventSplit(lDataSet, Nothing, lThreshold, lHighEvents)
                Dim lStormIndex As Integer = 0
                For Each lStorm As atcTimeseries In lStorms
                    lStormIndex += 1
                    'Storm Number, Date, Hour
                    lReport.Write(lStormIndex & vbTab & DateTabHour(lStorm.Dates.Value(1)))
                    'Duration
                    lReport.Write(vbTab & CInt((lStorm.Dates.Value(lStorm.Dates.numValues) - lStorm.Dates.Value(1)) * 24))
                    'Volume
                    lReport.Write(vbTab & Format(lStorm.Attributes.GetValue("Sum"), "0.00"))
                    lReport.WriteLine()
                Next
            Next
            lReport.Close()
        End If
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

End Class
