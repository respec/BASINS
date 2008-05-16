Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class Landuses
    Inherits KeyedCollection(Of String, Landuse)
    Protected Overrides Function GetKeyForItem(ByVal aLanduse As Landuse) As String
        Dim lKey As String = aLanduse.Name
        Return lKey
    End Function
End Class

Public Class Landuse
    Public Name As String
    Public Area As Double
    Public Catchment As Catchment
End Class
