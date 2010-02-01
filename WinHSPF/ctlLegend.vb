Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

Friend Class ctlLegend

    Public Shared IconMargin As Integer = 3

    Public LegendType As EnumLegendType
    Public Icons As New Generic.List(Of clsIcon)
    Public TopIconIndex As Integer = 0

    Public Event Resized()

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Clear()
    End Sub

    Public Sub Clear()
        btnScrollLegendUp.Left = IconMargin
        btnScrollLegendDown.Left = IconMargin
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
        aIcon.Top = lY
        aIcon.Left = IconMargin
        aIcon.Width = IconWidth()
        Icons.Add(aIcon)
        pnlLegend.Controls.Add(aIcon)
    End Sub

    Public Function Icon(ByVal aKey As String) As clsIcon
        For Each lIcon As clsIcon In Icons
            If lIcon.Key = aKey Then Return lIcon
        Next
        Return Nothing
    End Function

    Public Function IconWidth() As Integer
        Return Math.Max(Me.Width - IconMargin - IconMargin, 20)
    End Function

    Private Sub btnScrollLegendUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendUp.Click
        If TopIconIndex > 0 Then
            TopIconIndex -= 1
            PlaceIcons()
        End If
    End Sub

    Private Sub btnScrollLegendDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendDown.Click
        If TopIconIndex < Icons.Count - 1 Then
            TopIconIndex += 1
            PlaceIcons()
        End If
    End Sub

    ''' <summary>
    ''' After scrolling or selecting, make sure icons are in correct positions and selections are indicated
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PlaceIcons()
        If pnlLegend.Height > IconMargin AndAlso pnlLegend.Width > IconMargin Then
            Dim lY As Integer = IconMargin
            Dim lIndex As Integer = 0
            Dim lBackground As Bitmap = New Bitmap(pnlLegend.Width, pnlLegend.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim lGraphics As Graphics = Graphics.FromImage(lBackground)

            btnScrollLegendUp.Visible = (TopIconIndex > 0)

            For Each lIcon As clsIcon In Icons
                If lIndex < TopIconIndex Then
                    lIcon.Top = -lIcon.Height
                Else
                    lIcon.Top = lY
                    lIcon.Width = IconWidth()
                    If lIcon.Selected Then
                        lGraphics.FillRectangle(SystemBrushes.Highlight, 0, lY - IconMargin, Me.Width, lIcon.Height + IconMargin + IconMargin)
                    End If
                    lY += lIcon.Height + IconMargin * 2
                End If
                lIndex += 1
            Next

            btnScrollLegendDown.Visible = (lY > pnlLegend.Height)

            lGraphics.Dispose()
            pnlLegend.BackgroundImage = lBackground
        End If
    End Sub

    Private Sub ctlLegend_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        btnScrollLegendUp.Width = Me.Width - IconMargin * 2
        btnScrollLegendDown.Width = Me.Width - IconMargin * 2
        PlaceIcons()
        RaiseEvent Resized()
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
    Public Label As String = ""

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

    Public ReachOrBMP As HspfOperation
    Public Implnd As HspfOperation
    Public Perlnd As HspfOperation
    Public MetSeg As HspfMetSeg
    Public PointSource As HspfPointSource
    Public DownstreamIcons As New Generic.List(Of clsIcon)
    Public UpstreamIcons As New Generic.List(Of clsIcon)
    Public DistanceFromOutlet As Integer = -1

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