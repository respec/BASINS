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
    Public UciFileNames As New List(Of String)
    Private pUciFileName As String
    Private pUciFile As atcUCI.HspfUci
    Public WatershedImage As Image
    Public WatershedImageFilename As String
    Public WatershedName As String = ""

    Public DownstreamIcon As clsIcon
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
        Me.AllowDrop = True
        BackColor = Drawing.SystemColors.ButtonFace
    End Sub

    Public Overrides Function ToString() As String
        Return WatershedName
    End Function

    Public Function Key() As String
        Return WatershedName.ToLowerInvariant()
    End Function

    Public Function Center() As Point
        Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
    End Function

    Public Property UciFileName() As String
        Set(value As String)
            pUciFileName = value
            If Not Me.UciFileNames.Contains(pUciFileName) Then
                Me.UciFileNames.Add(pUciFileName)
            End If
            If FileExists(pUciFileName) Then
                Me.BackColor = Drawing.SystemColors.ButtonFace
            Else
                Me.BackColor = Drawing.Color.Salmon
            End If
        End Set
        Get
            Return pUciFileName
        End Get
    End Property

    Public Function UciFile() As atcUCI.HspfUci
        If pUciFile Is Nothing OrElse pUciFile.Name <> UciFileName Then
            pUciFile = OpenUCI(UciFileName)
        End If
        Return pUciFile
    End Function

End Class

Friend Class IconCollection
    Inherits KeyedCollection(Of String, clsIcon)
    Protected Overrides Function GetKeyForItem(ByVal item As clsIcon) As String
        Return item.Key
    End Function

    Friend Function FindOrAddIcon(ByVal aWatershedName As String) As clsIcon
        Dim lIcon As clsIcon
        If Contains(aWatershedName.ToLowerInvariant) Then
            lIcon = Item(aWatershedName.ToLowerInvariant)
        Else
            lIcon = New clsIcon
            lIcon.WatershedName = aWatershedName
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
