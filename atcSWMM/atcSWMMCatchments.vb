Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class Catchments
    Inherits KeyedCollection(Of String, Catchment)
    Protected Overrides Function GetKeyForItem(ByVal aCatchment As Catchment) As String
        Dim lKey As String = aCatchment.Name
        Return lKey
    End Function

    Public LayerIndex As Integer
End Class

Public Class Catchment
    Public Name As String
    Public RainGage As RainGage
    Public Conduit As Conduit
    Public FeatureIndex As Integer
    'Public Area As Double 'in acres or hectares
    'Public PercentImpervious As Double
    'Public Width As Double 'in feet or meters
    Public Slope As Double 'percent
    'Public CurbLength As Double '0.0
    'Public SnowPackName As String 'blank if none
End Class

