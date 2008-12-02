Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Public Class ctlLegend

    Public LegendType As EnumLegendType
    Public Shared IconMargin As Integer = 3
    Public Icons As New Generic.List(Of clsIcon)

    Public Sub Clear()
        With pnlLegend
            .SuspendLayout()
            If .BackgroundImage IsNot Nothing Then
                .BackgroundImage.Dispose()
                .BackgroundImage = Nothing 'New Bitmap(0, 0, Drawing.Imaging.PixelFormat.Format32bppArgb)
            End If
            .Controls.Clear()
            For Each lControl As Control In Icons
                lControl.Dispose()
            Next
            Icons.Clear()
            .ResumeLayout()
        End With
    End Sub

    Public Sub Add(ByVal aIcon As clsIcon)
        Dim lY As Integer = IconMargin
        If Icons.Count > 0 Then lY = Icons(Icons.Count - 1).Bottom + IconMargin * 2
        Icons.Add(aIcon)        
        aIcon.Top = lY
        aIcon.Width = Me.Width
        pnlLegend.Controls.Add(aIcon)
    End Sub

    Public Function Icon(ByVal aKey As String) As clsIcon
        For Each lIcon As clsIcon In Icons
            If lIcon.Tag = aKey Then Return lIcon
        Next
        Return Nothing
    End Function

    Private Sub btnScrollLegendUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendUp.Click

    End Sub

    Private Sub btnScrollLegendDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendDown.Click

    End Sub

End Class

Public Enum EnumLegendType
    LegLand = 0
    LegMet = 1
    LegPoint = 2
End Enum

Public Class clsIcon
    Inherits Windows.Forms.Control

    Public Selected As Boolean
    Public Key As String = ""

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub

    Public Function Center() As Point
        Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
    End Function

End Class

Public Class clsSchematicIcon
    Inherits clsIcon

    Public pOperation As HspfOperation
    Public DownstreamIcons As New Generic.List(Of clsIcon)
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1

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

Public Class PanelDoubleBuffer
    Inherits Panel
    Sub New()
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()
    End Sub
End Class