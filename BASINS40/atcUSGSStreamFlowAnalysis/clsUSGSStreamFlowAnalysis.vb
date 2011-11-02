Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Drawing

Public Class clsUSGSStreamFlowAnalysis
    Inherits atcData.atcDataDisplay

    Private pRequiredHelperPlugin As String = "Timeseries::Meteorologic Generation" 'atcMetCmp

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS Streamflow"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUSGSStreamFlowAnalysis))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Show = Nothing

        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then lTimeseriesGroup = New atcTimeseriesGroup
        If lTimeseriesGroup.Count = 0 Then 'ask user to specify some Data
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", lTimeseriesGroup)
        End If
        If lTimeseriesGroup.Count > 0 Then
            Dim lForm As New frmUSGSStreamFlowAnalysis
            ShowForm(lTimeseriesGroup, lForm)
            Return lForm
        Else
            Logger.Msg("Need to select at least one daily streamflow dataset", "USGS Streamflow Analysis")
        End If
    End Function

    Private Sub ShowForm(ByVal aTimeseriesGroup As atcData.atcDataGroup, ByVal aForm As Object)
        LoadPlugin(pRequiredHelperPlugin)
        Dim lBasicAttributes As New Generic.List(Of String)
        With lBasicAttributes
            .Add("ID")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("Count Missing")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Location")
        End With

        aForm.Initialize(aTimeseriesGroup, lBasicAttributes, True)
    End Sub

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            LoadPlugin(pRequiredHelperPlugin)
            Dim lForm As New frmUSGSStreamFlowAnalysis

            lForm.Initialize(aTimeseriesGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            'lForm.Dispose()
        End If
    End Sub

    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
    End Sub

    Private Sub LoadPlugin(ByVal aPluginName As String)
        Try
            Dim lKey As String = pMapWin.Plugins.GetPluginKey(aPluginName)
            'If Not g_MapWin.Plugins.PluginIsLoaded(lKey) Then 
            pMapWin.Plugins.StartPlugin(lKey)
        Catch e As Exception
            Logger.Dbg("Exception loading " & aPluginName & ": " & e.Message)
        End Try
    End Sub

    Public Shared Function ComputeRankedAnnualTimeseries(ByVal aTimeseriesGroup As atcTimeseriesGroup, _
                                                         ByVal aNDay() As Double, _
                                                         ByVal aHighFlag As Boolean, _
                                                         ByVal aFirstYear As Integer, _
                                                         ByVal aLastYear As Integer, _
                                                         ByVal aBoundaryMonth As Integer, _
                                                         ByVal aBoundaryDay As Integer, _
                                                         ByVal aEndMonth As Integer, _
                                                         ByVal aEndDay As Integer) As atcTimeseriesGroup
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", aTimeseriesGroup)

        lArgs.SetValue("NDay", aNDay)
        lArgs.SetValue("HighFlag", aHighFlag)

        lArgs.SetValue("FirstYear", aFirstYear)
        lArgs.SetValue("LastYear", aLastYear)

        lArgs.SetValue("BoundaryMonth", aBoundaryMonth)
        lArgs.SetValue("BoundaryDay", aBoundaryDay)

        lArgs.SetValue("EndMonth", aEndMonth)
        lArgs.SetValue("EndDay", aEndDay)

        Dim lHighLow As String = "low"
        If aHighFlag Then
            lHighLow = "high"
        End If

        'Dim lCalculator As New atcTimeseriesBaseflow.atcTimeseriesBaseflow
        'If lCalculator.Open("n-day " & lHighLow & " timeseries", lArgs) Then
        '    For Each lDataset As atcTimeseries In lCalculator.DataSets
        '        ComputeRanks(lDataset, Not aHighFlag, False)
        '    Next
        'End If
        'Return lCalculator.DataSets
        Return Nothing
    End Function
End Class
