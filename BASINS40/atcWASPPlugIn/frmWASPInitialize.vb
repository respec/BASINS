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
        cboLowest.Items.Add("1")
        cboLowest.Items.Add("2")
        cboLowest.Items.Add("3")
        cboLowest.Items.Add("4")
        cboLowest.Items.Add("5")
        cboLowest.SelectedIndex = 0
        RefreshSelectionInfo()
    End Sub

    Private Sub RefreshSelectionInfo()
        pCurrentLayerIndex = GisUtil.CurrentLayer
        pCurrentLayerName = GisUtil.LayerName(GisUtil.CurrentLayer)
        If GisUtil.LayerType(pCurrentLayerIndex) = 2 Then
            pNumSelected = GisUtil.NumSelectedFeatures(pCurrentLayerIndex)
            pNumFeatures = GisUtil.NumFeatures(pCurrentLayerIndex)

            'save which features are selected
            pSelectedIndexes = New atcCollection
            For lIndex As Integer = 0 To GisUtil.NumSelectedFeatures(pCurrentLayerIndex) - 1
                pSelectedIndexes.Add(lIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex, pCurrentLayerIndex))
            Next
        End If

        'txtInfo.Text = pNumSelected.ToString & " features out of " & pNumFeatures.ToString & " in the '" & _
        '               pCurrentLayerName & "' layer are selected."
        txtInfo.Text = "Selection Layer: " & pCurrentLayerName & vbCrLf & vbCrLf & _
                       "Number of Selected Features: " & pNumSelected.ToString & " of " & pNumFeatures.ToString

        'does the current layer look like a nhdplus flowlines layer?
        If GisUtil.LayerType(pCurrentLayerIndex) = 2 AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "COMID") AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "TOCOMID") AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "MAVELU") AndAlso _
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
        Dim lStreamOrders = New atcCollection

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
            Dim lStreamOrderFieldIndex As Integer = 0
            If GisUtil.IsField(pCurrentLayerIndex, "STREAMORDE") Then
                lStreamOrderFieldIndex = GisUtil.FieldIndex(pCurrentLayerIndex, "STREAMORDE")
            End If
            For lIndex As Integer = 0 To GisUtil.NumFeatures(pCurrentLayerIndex) - 1
                lComids.Add(lIndex, GisUtil.FieldValue(pCurrentLayerIndex, lIndex, lComidFieldIndex))
                lToComids.Add(lIndex, GisUtil.FieldValue(pCurrentLayerIndex, lIndex, lToComidFieldIndex))
                lStreamOrders.Add(lIndex, GisUtil.FieldValue(pCurrentLayerIndex, lIndex, lStreamOrderFieldIndex))
            Next

            Dim lLowestStreamOrder As Integer = cboLowest.SelectedIndex + 1

            'loop through each selected segment, see if anything is upstream of it
            Dim lSegmentIndexesToCheck As New Collection
            For Each lSegIndex As Integer In pSelectedIndexes
                lSegmentIndexesToCheck.Add(lSegIndex)
            Next
            Dim lSegmentIndexesToAdd As New Collection
            lSegmentIndexesToAdd.Add(0, "X")  'just to prime the loop
            Do While lSegmentIndexesToAdd.Count > 0
                lSegmentIndexesToAdd.Clear()
                For Each lSegIndex As Integer In lSegmentIndexesToCheck
                    Dim lSelectedComid As String = lComids(lSegIndex)
                    For lindex As Integer = 0 To lToComids.count - 1
                        If lToComids(lindex) = lSelectedComid Then
                            'see if what is upstream of this one is in the selected index collection
                            If Not pSelectedIndexes.Contains(lindex) Then
                                If lStreamOrders(lindex) >= lLowestStreamOrder Then
                                    lSegmentIndexesToAdd.Add(lindex)
                                End If
                            End If
                        End If
                    Next
                Next
                Logger.Dbg("Adding " & lSegmentIndexesToAdd.Count & " more selected WASP segments")
                lSegmentIndexesToCheck.Clear()
                For Each lSegIndex As Integer In lSegmentIndexesToAdd
                    pSelectedIndexes.Add(pSelectedIndexes.Count, lSegIndex)
                    lSegmentIndexesToCheck.Add(lSegIndex)
                Next
                txtInfo.Text = "Selection Layer: " & pCurrentLayerName & vbCrLf & vbCrLf & _
                               "Number of Selected Features: " & pSelectedIndexes.Count.ToString & " of " & pNumFeatures.ToString
                Me.Refresh()
            Loop

            'now mark the segments as selected on the map
            GisUtil.ClearSelectedFeatures(pCurrentLayerIndex)
            pSelectedIndexes.SortByValue()
            For Each lSegIndex As Integer In pSelectedIndexes
                GisUtil.SetSelectedFeature(pCurrentLayerIndex, lSegIndex)
            Next

            RefreshSelectionInfo()
        End If
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub
End Class