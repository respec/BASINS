Imports atcData
Imports atcUtility

Public Class clsBatchUnitStation
    Public StationID As String
    Public StreamFlowData As atcTimeseries
    Public BFInputs As atcDataAttributes
    Public CalcBF As atcTimeseriesBaseflow.clsBaseflow
    Public Message As String = ""
    Public NeedToDownloadData As Boolean = True
    Public StationDrainageArea As Double = -99
    Public StationDataFilename As String = ""

    Public Sub clsBatchUnitStation()
    End Sub

    Public Sub clsBatchUnitStation(ByVal aArgs As atcDataAttributes)
        If aArgs IsNot Nothing Then BFInputs = aArgs
    End Sub

    Public Function DoBaseFlowSeparation() As Boolean
        Dim lSuccess As Boolean = True

        If lSuccess Then
            Message = "SUCCESS"
        End If

        Return lSuccess
    End Function

End Class
