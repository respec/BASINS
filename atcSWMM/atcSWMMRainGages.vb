Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class RainGages
    Inherits KeyedCollection(Of String, RainGage)
    Protected Overrides Function GetKeyForItem(ByVal aRainGage As RainGage) As String
        Dim lKey As String = aRainGage.Name
        Return lKey
    End Function

    Public LayerFileName As Integer
End Class

Public Class RainGage
    Public Name As String
    Public FeatureIndex As Integer
    Public Form As String 'intensity (or volume or cumulative)
    Public Interval As Double '1.0
    Public SnowCatchFactor As Double '1.0
    Public Type As String 'timeseries (or file)
    Public TimeSeries As atcData.atcTimeseries
    Public Units As String 'in (or mm)
End Class
