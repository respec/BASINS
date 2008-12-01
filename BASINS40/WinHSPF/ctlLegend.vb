Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Friend Class clsIcon
    Inherits Windows.Forms.Control

    Public Selected As Boolean
    Public pOperation As HspfOperation
    Public DownstreamIcons As New Generic.List(Of clsIcon)
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1
    Public Key As String = ""

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub

    Public Function Center() As Point
        Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
    End Function

    Public Property Operation() As HspfOperation
        Get
            Return pOperation
        End Get
        Set(ByVal newValue As HspfOperation)
            pOperation = newValue
            If pOperation Is Nothing Then
                Key = ""
            Else
                Key = OperationKey(pOperation)
            End If
        End Set
    End Property
End Class

Friend Class IconCollection
    Inherits KeyedCollection(Of String, clsIcon)
    Protected Overrides Function GetKeyForItem(ByVal item As clsIcon) As String
        Return item.Key
    End Function
End Class

Friend Class PanelDoubleBuffer
    Inherits Panel
    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub
End Class

Public Class ctlLegend

    Dim lIcons As Generic.List(Of clsIcon)

End Class
