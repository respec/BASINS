Imports atcMwGisUtility
Imports atcUtility
Imports System.Windows.Forms

Public Class frmCalculate

    Friend pFields As atcCollection
    Friend pDefaults As atcCollection
    Friend pWidths As atcCollection
    Friend pTypes As atcCollection
    Friend pLayerName As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(pLayerName)

        For lCheckedIndex As Integer = 0 To clbCalculate.CheckedItems.Count - 1
            Dim lCheckedName As String = clbCalculate.CheckedItems(lCheckedIndex)
            'update this item for each feature
            Dim lFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lLayerIndex, lCheckedName, pTypes(pFields.IndexOf(lCheckedName)), pWidths(pFields.IndexOf(lCheckedName)))
            For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
                Dim lValue As String = ""
                If lCheckedName = "Name" Then
                    lValue = pDefaults(pFields.IndexOf(lCheckedName)) & CStr(lFeatureIndex + 1)
                ElseIf lCheckedName = "OutNodeID" Then
                    'todo: find closest subcatchment outlet node id
                ElseIf lCheckedName = "Width" Then
                    'estimate width of subcatchment
                    lValue = Math.Sqrt(GisUtil.FeatureArea(lLayerIndex, lFeatureIndex)) * 3.281 'convert m to ft
                ElseIf lCheckedName = "Slope" Then
                    'todo: compute subcatchment slope
                ElseIf lCheckedName = "InletNode" Then
                    'todo: find closest conduit inlet node id
                ElseIf lCheckedName = "OutletNode" Then
                    'todo: find closest conduit outlet node id
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
                GisUtil.SetFeatureValue(lLayerIndex, lFieldIndex, lFeatureIndex, lValue)
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

    Public Sub InitializeForm(ByVal aLayerName As String, ByVal aFields As atcCollection, ByVal aDefaults As atcCollection, ByVal aWidths As atcCollection, ByVal aTypes As atcCollection)
        pLayerName = aLayerName
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
End Class