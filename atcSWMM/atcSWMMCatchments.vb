Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports atcMwGisUtility
Imports System.Text

Public Class Catchments
    Inherits KeyedCollection(Of String, Catchment)
    Protected Overrides Function GetKeyForItem(ByVal aCatchment As Catchment) As String
        Dim lKey As String = aCatchment.Name
        Return lKey
    End Function

    Public LayerFileName As String
    Public SWMMProject As SWMMProject

    Public Function CreateFromShapefile(ByVal aShapefileName As String, ByVal aSubbasinFieldName As String) As Boolean
        Me.ClearItems()

        LayerFileName = aShapefileName

        If Not GisUtil.IsLayerByFileName(LayerFileName) Then
            GisUtil.AddLayer(LayerFileName, "Catchments")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(LayerFileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lCatchment As New Catchment
            Dim lSubbasinID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lCatchment.Name = "S" & lSubbasinID
            'lCatchment.RainGage()

            'find associated conduit
            For Each lConduit As Conduit In Me.SWMMProject.Conduits
                If lConduit.Name.Substring(1) = lSubbasinID Then
                    lCatchment.Conduit = lConduit
                    Exit For
                End If
            Next

            lCatchment.FeatureIndex = lFeatureIndex
            lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            'lCatchment.PercentImpervious()
            'lCatchment.Width()
            'lCatchment.Slope()
            Me.Add(lCatchment)
        Next

    End Function

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder

        lString.Append("[SUBCATCHMENTS]" & vbCrLf & _
                       ";;                                                 Total    Pcnt.             Pcnt.    Curb     Snow    " & vbCrLf & _
                       ";;Name           Raingage         Outlet           Area     Imperv   Width    Slope    Length   Pack    " & vbCrLf & _
                       ";;-------------- ---------------- ---------------- -------- -------- -------- -------- -------- --------" & vbCrLf)

        For Each lCatchment As Catchment In Me
            With lCatchment
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                If Not .RainGage Is Nothing Then
                    lString.Append(StrPad(.RainGage.Name, 16, " ", False))
                Else
                    lString.Append(StrPad("R1", 16, " ", False))
                End If
                lString.Append(" ")
                If Not .Conduit Is Nothing Then
                    lString.Append(StrPad(.Conduit.OutletID, 16, " ", False))
                Else
                    lString.Append(StrPad("J1", 16, " ", False))
                End If
                lString.Append(" ")
                lString.Append(StrPad(Format(.Area, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.PercentImpervious, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Width, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Slope, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.CurbLength, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(.SnowPackName)
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function

    Public Function PolygonsToString() As String
        Dim lString As New StringBuilder

        Dim lLayerIndex As Integer = GisUtil.LayerIndex(LayerFileName)

        lString.Append("[Polygons]" & vbCrLf & _
                       ";;Subcatchment   X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf) 

        Dim lX() As Double = Nothing
        Dim lY() As Double = Nothing
        For Each lCatchment As Catchment In Me
            With lCatchment
                GisUtil.PointsOfLine(lLayerIndex, .FeatureIndex, lX, lY)
                For lIndex As Integer = 0 To lX.GetUpperBound(0) - 1
                    lString.Append(StrPad(.Name, 16, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(lX(lIndex), "0.000"), 18, " ", False))
                    lString.Append(" ")
                    lString.Append(StrPad(Format(lY(lIndex), "0.000"), 18, " ", False))
                    lString.Append(vbCrLf)
                Next
            End With
        Next

        Return lString.ToString
    End Function
End Class

Public Class Catchment
    Public Name As String
    Public RainGage As RainGage
    Public Conduit As Conduit
    Public FeatureIndex As Integer
    Public Area As Double = 0.0 'in acres or hectares
    Public PercentImpervious As Double = 0.0
    Public Width As Double = 0.0 'in feet or meters
    Public Slope As Double = 0.0 'percent
    Public CurbLength As Double = 0.0
    Public SnowPackName As String = "" 'blank if none
End Class

