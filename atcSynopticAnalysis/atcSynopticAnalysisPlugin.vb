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
    End Function

    Public Overrides Sub Save(ByVal aDataManager As atcDataManager, _
                    ByVal aDataGroup As atcDataGroup, _
                    ByVal aFileName As String, _
                    ByVal ParamArray aOptions() As String)

        Dim lForm As New frmSynoptic
        lForm.Initialize(aDataManager, aDataGroup)
        lForm.Hide()

        Dim lReport As New IO.StreamWriter(aFileName)

        If aOptions Is Nothing OrElse _
           aOptions.Length = 0 OrElse _
           aOptions(0) Is Nothing OrElse _
           aOptions(0).Length < 1 Then

            ReDim aOptions(0)
            aOptions(0) = lForm.cboGroupBy.Text
        End If

        For Each lOption As String In aOptions
            Dim lGroupByIndex As Integer = lForm.cboGroupBy.Items.IndexOf(lOption)
            If lGroupByIndex >= 0 Then
                lForm.cboGroupBy.SelectedIndex = lGroupByIndex
                lReport.WriteLine(lForm.ToString)
            End If
        Next

        lReport.Close()
        Logger.Dbg("SynopticAnalysis Complete, Results in file '" & aFileName & "'")
    End Sub

    'Private Function DateString(ByVal aJulianDay As Double) As String
    '    Dim d(5) As Integer
    '    J2Date(aJulianDay, d)
    '    Return Format(d(0), "0000") & "/" & _
    '           Format(d(1), "00") & "/" & _
    '           Format(d(2), "00")
    'End Function

    'Private Function DateTabHour(ByVal aJulianDay As Double) As String
    '    Dim d(5) As Integer
    '    J2Date(aJulianDay, d)
    '    Return Format(d(0), "0000") & "/" & _
    '           Format(d(1), "00") & "/" & _
    '           Format(d(2), "00") & vbTab & _
    '           Format(d(3), "00")
    'End Function

    Public Shared Function ComputeEvents(ByVal aDataGroup As atcDataGroup, ByVal aThreshold As Double, ByVal aDaysGapAllowed As Double, ByVal aHighEvents As Boolean) As atcDataGroup
        ComputeEvents = New atcDataGroup
        If Not aDataGroup Is Nothing Then
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            For Each lDataSet As atcTimeseries In aDataGroup
                Dim lEvents As atcDataGroup = atcEvents.EventSplit(lDataSet, Nothing, aThreshold, aDaysGapAllowed, aHighEvents)
                ComputeEvents.AddRange(lEvents)
            Next
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End If
    End Function

End Class
