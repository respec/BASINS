Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports atcMwGisUtility
Imports System.Text

Public Class Conduits
    Inherits KeyedCollection(Of String, Conduit)
    Protected Overrides Function GetKeyForItem(ByVal aConduit As Conduit) As String
        Dim lKey As String = aConduit.Name
        Return lKey
    End Function

    Public LayerFileName As String

    Public Function CreateFromShapefile(ByVal aShapefileName As String, ByVal aSubbasinFieldName As String, ByVal aDownSubbasinFieldName As String) As Boolean
        Me.ClearItems()

        LayerFileName = aShapefileName

        If Not GisUtil.IsLayerByFileName(LayerFileName) Then
            GisUtil.AddLayer(LayerFileName, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(LayerFileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)
        Dim lDownSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aDownSubbasinFieldName)

        'create all conduits
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As New Conduit
            lConduit.Name = "C" & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lConduit.FeatureIndex = lFeatureIndex
            lConduit.InletID = lConduit.Name & "U"
            lConduit.OutletID = lConduit.Name & "D"
            lConduit.Length = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex)
            'lConduit.ManningsN()
            'lConduit.InletOffset()
            'lConduit.OutletOffset()
            'lConduit.InitialFlow()
            'lConduit.MaxFlow()
            Me.Add(lConduit)
        Next

    End Function

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder

        lString.Append("[CONDUITS]" & vbCrLf & _
                       ";;               Inlet            Outlet                      Manning    Inlet      Outlet     Init.      Max.      " & vbCrLf & _
                       ";;Name           Node             Node             Length     N          Offset     Offset     Flow       Flow      " & vbCrLf & _
                       ";;-------------- ---------------- ---------------- ---------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lConduit As Conduit In Me
            With lConduit
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.InletID, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.OutletID, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Length, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.ManningsN, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InletOffset, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.OutletOffset, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InitialFlow, "0.0"), 8, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.MaxFlow, "0.0"), 8, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function
End Class

Public Class Conduit
    Public Name As String
    Public FeatureIndex As Integer
    Public InletID As String 'name of inlet node
    Public OutletID As String 'name of outlet node
    Public Length As Double 'in feet or meters
    Public ManningsN As Double = 0.05
    Public InletOffset As Double = 0.0
    Public OutletOffset As Double = 0.0
    Public InitialFlow As Double = 0.0
    Public MaxFlow As Double = 0.0
End Class
