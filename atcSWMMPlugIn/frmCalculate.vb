Imports atcMwGisUtility
Imports atcUtility
Imports System.Windows.Forms

Public Class frmCalculate

    Friend pFields As atcCollection
    Friend pDefaults As atcCollection
    Friend pWidths As atcCollection
    Friend pTypes As atcCollection
    Friend pCalculateLayerName As String
    Friend pNodeLayerName As String
    Friend pGridLayerIndex As Integer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(pCalculateLayerName)

        For lCheckedIndex As Integer = 0 To clbCalculate.CheckedItems.Count - 1
            Dim lCheckedName As String = clbCalculate.CheckedItems(lCheckedIndex)

            If lCheckedName = "Slope" Then
                'special case for slope, need grid to calculate slope
                Dim lfrmSelectGrid As New frmSelectGrid
                lfrmSelectGrid.InitializeForm(Me)
                pGridLayerIndex = -1
                lfrmSelectGrid.ShowDialog()
            End If

            'update this item for each feature
            Dim lFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lLayerIndex, lCheckedName, pTypes(pFields.IndexOf(lCheckedName)), pWidths(pFields.IndexOf(lCheckedName)))
            For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
                Dim lValue As String = ""
                Dim lUpdateMe As Boolean = True
                If lCheckedName = "Name" Then
                    lValue = pDefaults(pFields.IndexOf(lCheckedName)) & CStr(lFeatureIndex + 1)
                ElseIf lCheckedName = "OutNodeID" Then
                    'find closest subcatchment outlet node id
                    If pNodeLayerName <> "<none>" Then
                        Dim lCentroidX As Double
                        Dim lCentroidY As Double
                        GisUtil.ShapeCentroid(lLayerIndex, lFeatureIndex, lCentroidX, lCentroidY)
                        lValue = FindClosestNodeToXY(lCentroidX, lCentroidY)
                    End If
                ElseIf lCheckedName = "Width" Then
                    'estimate width of subcatchment
                    lValue = Math.Sqrt(GisUtil.FeatureArea(lLayerIndex, lFeatureIndex)) * 3.281 'convert m to ft
                ElseIf lCheckedName = "Slope" Then
                    'compute subcatchment slope in percent
                    If pGridLayerIndex > -1 Then
                        lValue = GisUtil.GridSlopeInPolygon(pGridLayerIndex, lLayerIndex, lFeatureIndex) * 100
                    Else
                        lUpdateMe = False
                    End If
                ElseIf lCheckedName = "InletNode" Then
                    'find closest conduit inlet node id
                    If pNodeLayerName <> "<none>" Then
                        Dim lX1 As Double
                        Dim lY1 As Double
                        Dim lX2 As Double
                        Dim lY2 As Double
                        GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lX1, lY1, lX2, lY2)
                        lValue = FindClosestNodeToXY(lX1, lY1)
                    End If
                ElseIf lCheckedName = "OutletNode" Then
                    'find closest conduit outlet node id
                    If pNodeLayerName <> "<none>" Then
                        Dim lX1 As Double
                        Dim lY1 As Double
                        Dim lX2 As Double
                        Dim lY2 As Double
                        GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lX1, lY1, lX2, lY2)
                        lValue = FindClosestNodeToXY(lX2, lY2)
                    End If
                ElseIf lCheckedName = "Geometry1" Then
                    'estimate full height from mean depth of conduit
                    lValue = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, GisUtil.FieldIndex(lLayerIndex, "MeanDepth")) * 1.25
                ElseIf lCheckedName = "Geometry2" Then
                    'estimate base width from mean width and mean depth of conduit
                    lValue = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, GisUtil.FieldIndex(lLayerIndex, "MeanWidth")) - (2 * GisUtil.FieldValue(lLayerIndex, lFeatureIndex, GisUtil.FieldIndex(lLayerIndex, "MeanDepth")))
                Else
                    'regular case
                    lValue = pDefaults(pFields.IndexOf(lCheckedName))
                End If
                If lUpdateMe Then GisUtil.SetFeatureValue(lLayerIndex, lFieldIndex, lFeatureIndex, lValue)
            Next
        Next
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cbxSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSelect.CheckedChanged
        For lRow As Integer = 0 To clbCalculate.Items.Count - 1
            clbCalculate.SetItemChecked(lRow, cbxSelect.Checked)
        Next
    End Sub

    Public Sub InitializeForm(ByVal aCalculateLayerName As String, ByVal aNodeLayerName As String, ByVal aFields As atcCollection, ByVal aDefaults As atcCollection, ByVal aWidths As atcCollection, ByVal aTypes As atcCollection)
        pCalculateLayerName = aCalculateLayerName
        pNodeLayerName = aNodeLayerName
        pFields = aFields
        pDefaults = aDefaults
        pWidths = aWidths
        pTypes = aTypes

        clbCalculate.Items.Clear()
        If Not pFields Is Nothing Then
            For Each lField As String In pFields
                clbCalculate.Items.Add(lField, True)
            Next
        End If
    End Sub

    Private Function FindClosestNodeToXY(ByVal aInputX As Double, ByVal aInputY As Double) As String
        Dim lMinimumDistance As Double = 1.0E+20
        Dim lClosestFeature As Integer = -1
        Dim lValue As String = ""
        Dim lNodeLayerIndex As Integer = GisUtil.LayerIndex(pNodeLayerName)
        For lNodeFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lNodeLayerIndex) - 1
            Dim lX As Double
            Dim lY As Double
            GisUtil.PointXY(lNodeLayerIndex, lNodeFeatureIndex, lX, lY)
            If (lX - aInputX) ^ 2 + (lY - aInputY) ^ 2 < lMinimumDistance Then
                lMinimumDistance = (lX - aInputX) ^ 2 + (lY - aInputY) ^ 2
                lClosestFeature = lNodeFeatureIndex
            End If
        Next
        If lClosestFeature > -1 And GisUtil.IsField(lNodeLayerIndex, "Name") Then
            Dim lName As String = ""
            lName = GisUtil.FieldValue(lNodeLayerIndex, lClosestFeature, GisUtil.FieldIndex(lNodeLayerIndex, "Name"))
            If lName.Length > 0 Then
                lValue = lName
            Else
                lValue = "N" & CStr(lClosestFeature + 1)
            End If
        End If
        Return lValue
    End Function

    Public Sub SetGridLayerIndexForSlope(ByVal aGridLayerIndex As Integer)
        pGridLayerIndex = aGridLayerIndex
    End Sub
End Class