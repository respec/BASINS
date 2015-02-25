Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Public Enum EnumLegendType
    LegLand = 0
    LegMet = 1
    LegPoint = 2
End Enum

Public Class clsIcon
    Inherits Windows.Forms.Control

    Public Selected As Boolean
    Public Label As String = ""
    Public UciFileName As String = ""
    Public WatershedImageFilename As String
    Public OrigImage As Image

    Public DownstreamIcon As clsIcon
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub

    Public Overrides Function ToString() As String
        Return UciFileName
    End Function

    Public Function Key() As String
        Return UciFileName.ToLowerInvariant()
    End Function

    Public Function Center() As Point
        Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
    End Function

End Class

Friend Class IconCollection
    Inherits KeyedCollection(Of String, clsIcon)
    Protected Overrides Function GetKeyForItem(ByVal item As clsIcon) As String
        Return item.Key
    End Function

    Friend Function FindOrAddIcon(ByVal aUciFilename As String) As clsIcon
        Dim lIcon As clsIcon
        If Contains(aUciFilename.ToLowerInvariant) Then
            lIcon = Item(aUciFilename.ToLowerInvariant)
        Else
            lIcon = New clsIcon
            lIcon.UciFileName = aUciFilename
            Add(lIcon)
        End If
        Return lIcon
    End Function
End Class

Public Class PanelDoubleBuffer
    Inherits Panel
    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        Me.BackgroundImageLayout = ImageLayout.None
        UpdateStyles()
    End Sub
End Class
