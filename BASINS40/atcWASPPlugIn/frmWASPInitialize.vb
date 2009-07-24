Imports MapWinUtility
Imports atcMwGisUtility
Imports atcUtility

Public Class frmWASPInitialize

    Friend pPlugIn As PlugIn
    Friend pCurrentLayerIndex As Integer
    Friend pCurrentLayerName As String
    Friend pNumSelected As Integer
    Friend pNumFeatures As Integer
    Friend pSelectedIndexes As atcCollection

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lfrmWASPSetup As New frmWASPSetup
        With lfrmWASPSetup
            .InitializeUI(pPlugIn, pSelectedIndexes, pCurrentLayerIndex)
            Logger.Dbg("WASPSetup Initialized")
            .Show()
            Logger.Dbg("WASPSetup Shown")
            Me.Close()
            .GenerateSegments()
            .InitializeStationLists()
            .EnableControls(True)
        End With
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        pPlugIn = aPlugIn
        RefreshSelectionInfo()
    End Sub

    Private Sub RefreshSelectionInfo()
        pCurrentLayerIndex = GisUtil.CurrentLayer
        pCurrentLayerName = GisUtil.LayerName(GisUtil.CurrentLayer)
        pNumSelected = GisUtil.NumSelectedFeatures(pCurrentLayerIndex)
        pNumFeatures = GisUtil.NumFeatures(pCurrentLayerIndex)

        'save which features are selected
        pSelectedIndexes = New atcCollection
        For lIndex As Integer = 0 To GisUtil.NumSelectedFeatures(pCurrentLayerIndex) - 1
            pSelectedIndexes.Add(lIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex, pCurrentLayerIndex))
        Next

        txtInfo.Text = pNumSelected.ToString & " features out of " & pNumFeatures.ToString & " in the '" & _
                       pCurrentLayerName & "' layer are selected."

        'does the current layer look like a nhdplus flowlines layer?
        If GisUtil.LayerType(pCurrentLayerIndex) = 2 And _
           GisUtil.IsField(pCurrentLayerIndex, "COMID") And _
           GisUtil.IsField(pCurrentLayerIndex, "TOCOMID") And _
           GisUtil.IsField(pCurrentLayerIndex, "MAVELU") And _
           GisUtil.IsField(pCurrentLayerIndex, "CUMDRAINAG") Then
            'yes, looks like nhdplus flowlines
            txtWarning.Text = ""
        Else
            'no, problem 
            txtWarning.Text = "Warning: The current layer does not appear to be a valid NHDPlus Flowline layer!"
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        RefreshSelectionInfo()
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        'select upstream segments
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lComids = New atcCollection
        Dim lToComids = New atcCollection

        If pNumSelected > 0 Then
            'build collection of comid, tocomid
            Dim lComidFieldIndex As Integer = 0
            If GisUtil.IsField(pCurrentLayerIndex, "COMID") Then
                lComidFieldIndex = GisUtil.FieldIndex(pCurrentLayerIndex, "COMID")
            End If
            Dim lToComidFieldIndex As Integer = 0
            If GisUtil.IsField(pCurrentLayerIndex, "TOCOMID") Then
                lToComidFieldIndex = GisUtil.FieldIndex(pCurrentLayerIndex, "TOCOMID")
            End If
            For lIndex As Integer = 0 To GisUtil.NumFeatures(pCurrentLayerIndex) - 1
                lComids.Add(lIndex, GisUtil.FieldValue(pCurrentLayerIndex, lIndex, lComidFieldIndex))
                lToComids.Add(lIndex, GisUtil.FieldValue(pCurrentLayerIndex, lIndex, lToComidFieldIndex))
            Next

            'loop through each selected segment, see if anything is upstream of it
            Dim pSegmentIndexesToAdd As New Collection
            pSegmentIndexesToAdd.Add(0, "X")  'just to prime the loop
            Do While pSegmentIndexesToAdd.Count > 0
                pSegmentIndexesToAdd.Clear()
                For Each lSegIndex As Integer In pSelectedIndexes
                    Dim lSelectedComid As String = lComids(lSegIndex)
                    For lindex As Integer = 0 To lToComids.count - 1
                        If lToComids(lindex) = lSelectedComid Then
                            'see if what is upstream of this one is in the selected index collection
                            If Not pSelectedIndexes.Contains(lindex) Then
                                pSegmentIndexesToAdd.Add(lindex)
                            End If
                        End If
                    Next
                Next
                For Each lSegIndex As Integer In pSegmentIndexesToAdd
                    pSelectedIndexes.Add(pSelectedIndexes.Count, lSegIndex)
                Next
            Loop

            'now mark the segments as selected on the map
            GisUtil.ClearSelectedFeatures(pCurrentLayerIndex)
            For Each lSegIndex As Integer In pSelectedIndexes
                GisUtil.SetSelectedFeature(pCurrentLayerIndex, lSegIndex)
            Next

            RefreshSelectionInfo()
        End If
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub
End Class