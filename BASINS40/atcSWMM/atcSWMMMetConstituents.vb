Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class MetConstituents
    Inherits KeyedCollection(Of String, MetConstituent)
    Protected Overrides Function GetKeyForItem(ByVal aMetConstituent As MetConstituent) As String
        Dim lKey As String = aMetConstituent.Type
        Return lKey
    End Function
End Class

Public Class MetConstituent
    Public Type As String 'Evap or Temp
    Public TimeSeries As atcData.atcTimeseries
End Class
