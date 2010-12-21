Imports atcData
Imports atcutility

Public Class DFLOWAnalysisPlugin

    '
    ' Notes on behavior:
    '
    ' 1) Save method invokes batch operation
    '
    ' 2) Show method invokes interactive operation
    '
    ' 3) Show method logic:
    '    - Create instance of frmDFLOWResults
    '    - frmDFLOWResults.New method invokes atcDataManager.UserSelectData to get time series
    '    - frmDFLOWResults.New method then invokes frmDFLOWResults.UserSpecifyDFLOWArgs to get season, period, xQy, and xBy parameters
    '    - frmDFLOWResults.UserSpecifyDFLOWArgs creates an instance of frmDFLOWArgs and invokes frmDFLOWArgs.AskUser
    '
    ' 4) Transitions:
    '   "File|Select Data" brings up atcDataManager.UserSelectData
    '   "File|Specify Inputs" brings up frmDFLOWResults.UserSpecifyDFLOWArgs [showmodal?]

    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::DFLOW"
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return "LimnoTech"
        End Get
    End Property
    
    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)


        ' "BATCH" VERSION

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            ' Do computation driven by contents of paramarry

            If UBound(aOption) > 0 Then fBioPeriod = aOption(0) Else fBioPeriod = 1
            If UBound(aOption) > 1 Then fBioYears = aOption(1) Else fBioYears = 3
            If UBound(aOption) > 2 Then fBioCluster = aOption(2) Else fBioCluster = 120
            If UBound(aOption) > 3 Then fBioExcursions = aOption(3) Else fBioExcursions = 5
            If UBound(aOption) > 4 Then fStartMonth = aOption(4) Else fStartMonth = 4
            If UBound(aOption) > 5 Then fStartDay = aOption(5) Else fStartDay = 1
            If UBound(aOption) > 6 Then fFirstYear = aOption(6) Else fFirstYear = 0
            If UBound(aOption) > 7 Then fLastYear = aOption(7) Else fLastYear = 0

            fEndDay = fStartDay - 1
            If fEndDay = 0 Then
                fEndMonth = fStartMonth - 1
                If fStartMonth = 0 Then fEndMonth = 12
                Dim pLastDayOfMonth() As Integer = {99, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
                fEndDay = pLastDayOfMonth(fEndMonth)
            End If

            fNonBioType = 0
            fAveragingPeriod = 7
            fReturnPeriod = 10

            Dim lForm As New frmDFLOWResults(aTimeseriesGroup)
            If lForm.DialogResult <> Windows.Forms.DialogResult.Cancel Then
                SaveFileString(aFileName, lForm.ToString)
            End If
        End If
    End Sub

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup

        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup.AddRange(atcDataManager.UserSelectData("Select Data For DFLOW Analysis", lTimeseriesGroup, Nothing, True))
        End If

        If lTimeseriesGroup.Count > 0 Then
            Dim lForm As New frmDFLOWResults(lTimeseriesGroup)
            If lForm.DialogResult <> Windows.Forms.DialogResult.Cancel Then
                lForm.Show()
            End If
        End If
        Return Nothing
    End Function

    Public Sub ComputexBy(ByVal aTimeseriesGroup As atcTimeseriesGroup, ByVal x As Integer, ByVal y As Integer)
        For Each lTimeSeries As atcTimeseries In aTimeseriesGroup

            Dim lNTimeSeries As atcTimeseries = lTimeSeries.Clone
            Dim n As Integer = lTimeSeries.numValues ' option base 1
            Debug.Print(lTimeSeries.Value(1), lTimeSeries.Value(n))
            If Double.IsNaN(lTimeSeries.Value(n)) Then

            End If

            ' N-day average
            ' Iterate

            lNTimeSeries.Clear()

        Next
    End Sub
End Class

