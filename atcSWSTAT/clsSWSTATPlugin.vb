Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class clsSWSTATPlugin
    Inherits atcData.atcDataDisplay

    Private pTrendName As String = "Trend"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS Surface Water Statistics (SWSTAT)::Integrated Frequency Analysis"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSWSTAT))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lForm As New frmSWSTAT
        ShowForm(aTimeseriesGroup, lForm)
        Return lForm
    End Function

    Private Sub ShowForm(ByVal aTimeseriesGroup As atcData.atcDataGroup, ByVal aForm As Object)
        Dim lBasicAttributes As New ArrayList
        With lBasicAttributes
            .Add("ID")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("Count Missing")
        End With

        Dim lNDayAttributes As New ArrayList
        With lNDayAttributes
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
        End With

        Dim lTrendAttributes As New ArrayList
        With lTrendAttributes
            .Add("Original ID")
            .Add("KENTAU")
            .Add("KENPLV")
            .Add("KENSLPL")
            .Add("From")
            .Add("To")
            .Add("Count")
            .Add("Not Used")
            .Add("Min")
            .Add("Max")
            .Add("Constituent")
            .Add("STAID")
        End With
        aForm.Initialize(aTimeseriesGroup, lBasicAttributes, lNDayAttributes, lTrendAttributes)
    End Sub

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            Dim lForm As New frmSWSTAT

            lForm.Initialize(aTimeseriesGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub

    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)_" & pTrendName, _
                                                       atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)", pTrendName, Me.Icon, , , True))
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        MyBase.ItemClicked(aItemName, aHandled)
        If Not aHandled AndAlso aItemName.Equals(atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)_" & pTrendName) Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = _
              atcDataManager.UserSelectData("Select Data For Trend Analysis", _
                                            Nothing, Nothing, True, True, Me.Icon)
            If lTimeseriesGroup.Count > 0 Then
                Dim lForm As New frmTrend
                ShowForm(lTimeseriesGroup, lForm)
            End If
        End If
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

        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        If lCalculator.Open("n-day " & lHighLow & " timeseries", lArgs) Then
            For Each lDataset As atcTimeseries In lCalculator.DataSets
                ComputeRanks(lDataset, Not aHighFlag, False)
            Next
        End If
        Return lCalculator.DataSets
    End Function

    Public Shared Function ListDefaultArray(ByVal aListTag As String) As Double()
        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        Dim lNDayHi As atcDefinedValue = lCalculator.AvailableOperations.GetDefinedValue("n-day high value")
        Dim lArgs As atcDataAttributes = lNDayHi.Arguments
        Dim lDefault As Object = lArgs.GetDefinedValue(aListTag).Definition.DefaultValue
        If IsArray(lDefault) Then
            Return lDefault
        Else
            Return Nothing
        End If
    End Function

End Class