Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class WASPTimeseriesCollection
    Inherits KeyedCollection(Of String, WASPTimeseries)
    Protected Overrides Function GetKeyForItem(ByVal aWASPTimeseries As WASPTimeseries) As String
        Dim lKey As String = aWASPTimeseries.Identifier
        Return lKey
    End Function

    Public WASPProject As WASPProject
End Class

Public Class WASPTimeseries
    Public Identifier As String
    Public Type As String 'flow, air temp, solrad, water temp, etc.
    Public SDate As Double
    Public EDate As Double
    Public TimeSeries As atcData.atcTimeseries
    Public Description As String
End Class
