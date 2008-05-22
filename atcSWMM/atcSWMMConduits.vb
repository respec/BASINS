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
    Public SWMMProject As SWMMProject

    Public Function CreateFromShapefile(ByVal aShapefileName As String, _
                                        ByVal aSubbasinFieldName As String, _
                                        ByVal aDownSubbasinFieldName As String, _
                                        ByVal aElevHighFieldName As String, _
                                        ByVal aElevLowFieldName As String) As Boolean
        Me.ClearItems()

        LayerFileName = aShapefileName

        If Not GisUtil.IsLayerByFileName(LayerFileName) Then
            GisUtil.AddLayer(LayerFileName, "Conduits")
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(LayerFileName)
        Dim lSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aSubbasinFieldName)
        Dim lDownSubbasinFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aDownSubbasinFieldName)
        Dim lElevHighFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aElevHighFieldName)
        Dim lElevLowFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, aElevLowFieldName)

        'create all conduits
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As New Conduit
            lConduit.Name = "C" & GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lConduit.FeatureIndex = lFeatureIndex

            lConduit.Length = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex)
            'lConduit.ManningsN()
            'lConduit.InletOffset()
            'lConduit.OutletOffset()
            'lConduit.InitialFlow()
            'lConduit.MaxFlow()

            Dim lElevHigh As Double = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevHighFieldIndex)
            Dim lElevLow As Double = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lElevLowFieldIndex)
            If lElevHigh < lElevLow Then
                'something is wrong, switch places
                Dim lTemp As Double = lElevHigh
                lElevHigh = lElevLow
                lElevLow = lTemp
            End If
            Dim lXup As Double
            Dim lYup As Double
            Dim lXdown As Double
            Dim lYdown As Double
            GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lXup, lYup, lXdown, lYdown)
            'todo: may need to verify which way the line is digitized

            'create node at upstream end
            Dim lUpNode As New Node
            Dim lSubID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lSubbasinFieldIndex)
            lUpNode.Name = "J" & lSubID
            lUpNode.Type = "JUNCTION"
            lUpNode.InvertElevation = lElevHigh
            lUpNode.XPos = lXup
            lUpNode.YPos = lYup
            If Not Me.SWMMProject.Nodes.Contains(lUpNode.Name) Then
                Me.SWMMProject.Nodes.Add(lUpNode)
                lConduit.InletNode = lUpNode
            Else
                lConduit.InletNode = Me.SWMMProject.Nodes(lUpNode.Name)
            End If

            'create node at downstream end
            Dim lNode As New Node
            Dim lDownID As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lDownSubbasinFieldIndex)
            If CInt(lDownID) > 0 Then
                lNode.Name = "J" & lDownID
                lNode.Type = "JUNCTION"
            Else
                lNode.Name = "O1"
                lNode.Type = "OUTFALL"
            End If
            lNode.InvertElevation = lElevLow
            lNode.XPos = lXdown
            lNode.YPos = lYdown
            If Not Me.SWMMProject.Nodes.Contains(lNode.Name) Then
                Me.SWMMProject.Nodes.Add(lNode)
                lConduit.OutletNode = lNode
            Else
                lConduit.OutletNode = Me.SWMMProject.Nodes(lNode.Name)
                'make sure coordinates correspond with downstream end
                lConduit.OutletNode.XPos = lNode.XPos
                lConduit.OutletNode.YPos = lNode.YPos
            End If

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
                lString.Append(StrPad(.InletNode.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.OutletNode.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Length, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.ManningsN, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.OutletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InitialFlow, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.MaxFlow, "0.0"), 10, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function
End Class

Public Class Conduit
    Public Name As String
    Public FeatureIndex As Integer
    Public InletNode As Node
    Public OutletNode As Node
    Public Length As Double 'in feet or meters
    Public ManningsN As Double = 0.05
    Public InletOffset As Double = 0.0
    Public OutletOffset As Double = 0.0
    Public InitialFlow As Double = 0.0
    Public MaxFlow As Double = 0.0
End Class
