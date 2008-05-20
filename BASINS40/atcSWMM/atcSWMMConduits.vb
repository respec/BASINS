Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class Conduits
    Inherits KeyedCollection(Of String, Conduit)
    Protected Overrides Function GetKeyForItem(ByVal aConduit As Conduit) As String
        Dim lKey As String = aConduit.Name
        Return lKey
    End Function

    Public LayerFileName As Integer
End Class

Public Class Conduit
    Public Name As String
    Public FeatureIndex As Integer
    Public InletID As String 'name of inlet node
    Public OutletID As String 'name of outlet node
    Public Length As Double 'in feet or meters
    Public ManningsN As Double '0.05
    Public InletOffset As Double '0.0
    Public OutletOffset As Double '0.0
    Public InitialFlow As Double '0.0
    Public MaxFlow As Double '0.0
End Class
