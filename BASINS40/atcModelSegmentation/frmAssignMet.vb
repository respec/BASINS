Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmAssignMet

    Dim gSubbasinLayerName As String = ""

    Public Sub SetSubbasinsLayer(ByVal aSubbasinLayerName As String)
        gSubbasinLayerName = aSubbasinLayerName
    End Sub

    Private Sub frmAssignMet_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim lLayerIndex As Integer
        Dim lDefaultLayerIndex As Integer = -1
        Dim lLayerName As String

        For lLayerIndex = 0 To GisUtil.NumLayers() - 1
            lLayerName = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 1 Then
                'This is a Point Shapefile 
                cboMetStations.Items.Add(lLayerName)
                If GisUtil.CurrentLayer = lLayerIndex Then
                    'this layer is selected, default to it
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                ElseIf (InStr(lLayerName, "Weather Station Sites") > 0 Or InStr(lLayerName, "Met Stations") > 0) And lDefaultLayerIndex < 0 Then
                    'this looks like a reasonable default layer
                    lDefaultLayerIndex = cboMetStations.Items.Count - 1
                End If
            End If
        Next

        If lDefaultLayerIndex > -1 Then
            'have a default layer
            cboMetStations.SelectedIndex = lDefaultLayerIndex
        End If
        If cboMetStations.Items.Count > 0 And cboMetStations.SelectedIndex < 0 Then
            'default to first layer if nothing more fitting has been found
            cboMetStations.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssign.Click

        Dim lMetLayerName As String = cboMetStations.Items(cboMetStations.SelectedIndex)
        Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(lMetLayerName)

        Dim lSubbasinLayerName As String = gSubbasinLayerName
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(lSubbasinLayerName)

        If cbxUseSelected.Checked And GisUtil.NumSelectedFeatures(lMetLayerIndex) = 0 Then
            'nothing selected in specified layer, let user know this is a problem
            MsgBox("Nothing is selected in layer '" & lMetLayerName & "'.", MsgBoxStyle.Information, "Assign Met Segments Problem")
            Exit Sub
        End If

        'change to hourglass cursor
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        'check to see if modelseg field is on subbasins layer, add if not 
        Dim lModelSegFieldIndex As Integer = -1
        If GisUtil.IsField(lSubbasinLayerIndex, "ModelSeg") Then
            lModelSegFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "ModelSeg")
        Else
            'need to add it
            lModelSegFieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "ModelSeg", 0, 40)
        End If

        'build local collection of selected features in met station layer
        Dim cSelectedMetStations As New Collection
        Dim lIndex As Integer
        If cbxUseSelected.Checked Then
            For lIndex = 1 To GisUtil.NumSelectedFeatures(lMetLayerIndex)
                cSelectedMetStations.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lMetLayerIndex))
            Next
        End If
        If cSelectedMetStations.Count = 0 Then
            'no met stations selected, act as if all are selected
            For lIndex = 1 To GisUtil.NumFeatures(lMetLayerIndex)
                cSelectedMetStations.Add(lIndex - 1)
            Next
        End If

        'loop through each subbasin and assign to nearest met station
        Dim lSubbasinIndex As Integer = 0
        Dim lMetStationIndex As Integer = 0
        Dim lSubbasinX As Double, lSubbasinY As Double
        Dim lPointX As Double, lPointY As Double
        GisUtil.StartSetFeatureValue(lSubbasinLayerIndex)
        For lSubbasinIndex = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            'do progress
            Logger.Progress(lSubbasinIndex, GisUtil.NumFeatures(lSubbasinLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            'get centroid of this subbasin
            GisUtil.ShapeCentroid(lSubbasinLayerIndex, lSubbasinIndex - 1, lSubbasinX, lSubbasinY)
            'now find the closest met station
            Dim lShortestDistance As Double = Double.MaxValue
            Dim lClosestMetStationIndex As Integer = -1
            Dim lDistance As Double = 0.0
            For lMetStationIndex = 1 To cSelectedMetStations.Count
                GisUtil.PointXY(lMetLayerIndex, cSelectedMetStations(lMetStationIndex), lPointX, lPointY)
                'calculate the distance
                lDistance = Math.Sqrt(((Math.Abs(lSubbasinX) - Math.Abs(lPointX)) ^ 2) + ((Math.Abs(lSubbasinY) - Math.Abs(lPointY)) ^ 2))
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestMetStationIndex = cSelectedMetStations(lMetStationIndex)
                End If
            Next
            If lClosestMetStationIndex > -1 Then
                'set ModelSeg attribute
                Dim lModelSegText As String = ""
                Dim lLocationFieldIndex As Integer = -1
                If GisUtil.IsField(lMetLayerIndex, "Location") Then
                    lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Location")
                    lModelSegText = GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                End If
                If GisUtil.IsField(lMetLayerIndex, "Stanam") Then
                    lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Stanam")
                    If Len(lModelSegText) > 0 Then
                        lModelSegText = lModelSegText & ":"
                    End If
                    lModelSegText = lModelSegText & GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                End If
                If Len(lModelSegText) = 0 Then
                    lModelSegText = CStr(lClosestMetStationIndex)
                End If
                GisUtil.SetFeatureValueNoStartStop(lSubbasinLayerIndex, lModelSegFieldIndex, lSubbasinIndex - 1, lModelSegText)
            End If
        Next
        GisUtil.StopSetFeatureValue(lSubbasinLayerIndex)

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Close()

    End Sub
End Class