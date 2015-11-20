Imports MapWinUtility
Imports atcMwGisUtility
Imports atcUtility

''' <summary>
''' This form is shown when the plugin is first activated. It allows the user to select one or more stream segments from a NHDPlus flowline layer that will be used to
''' build a Wasp input file. The form is always shown on top so won't disappear when the MapWindow application is selected. It changes dynamically as segments are selected
''' and also includes functionality to select upstream segments for you. After accepting this form, the WaspBuilder form is displayed.
''' </summary>
Public Class frmWASPInitialize

    Friend pPlugIn As PlugIn
    Friend pCurrentLayerIndex As Integer
    Friend pCurrentLayerName As String
    Friend pNumSelected As Integer
    Friend pNumFeatures As Integer
    Friend pSelectedIndexes As atcCollection

    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        pPlugIn.pfrmWASPSetup = New frmWASPSetup
        With pPlugIn.pfrmWASPSetup
            .InitializeUI(pPlugIn, pSelectedIndexes, pCurrentLayerIndex)
            Logger.Dbg("WASPSetup Initialized")
            .Show()
            Logger.Dbg("WASPSetup Shown")
            Me.Close()
            .GenerateSegments()
            .InitializeStationLists()
            .btnSave.PerformClick()
            .EnableControls(True)
        End With
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn)
        pPlugIn = aPlugIn
        cboLowest.Items.AddRange(New String() {"1", "2", "3", "4", "5"})
        cboLowest.SelectedIndex = 0
        RefreshSelectionInfo()
    End Sub

    Friend Sub RefreshSelectionInfo()
        pCurrentLayerIndex = GisUtil.CurrentLayer
        If pCurrentLayerIndex < 0 Then Exit Sub

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
        lblInfo.Text = "Selection Layer: " & pCurrentLayerName & vbCrLf & vbCrLf & _
                       "Number of Selected Features: " & pNumSelected.ToString & " of " & pNumFeatures.ToString

        'does the current layer look like a nhdplus flowlines layer?
        If GisUtil.LayerType(pCurrentLayerIndex) = 2 AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "COMID") AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "TOCOMID") AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "MAVELU") AndAlso _
           GisUtil.IsField(pCurrentLayerIndex, "CUMDRAINAG") Then
            'yes, looks like nhdplus flowlines
            lblWarning.Text = ""
        Else
            'no, problem 
            lblWarning.Text = "Warning: The current layer does not appear to be a valid NHDPlus Flowline layer!"
        End If

        grpSelect.Enabled = pNumSelected > 0
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshSelectionInfo()
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        'select upstream segments
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        lblWarning.Text = "Selecting upstream segments..."
        System.Windows.Forms.Application.DoEvents()
        Dim lComids As New atcCollection
        Dim lToComids As New atcCollection
        Dim lStreamOrders As New atcCollection

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
                lblInfo.Text = "Selection Layer: " & pCurrentLayerName & vbCrLf & vbCrLf & _
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
        lblWarning.Text = ""
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub frmWASPInitialize_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveWindowPos(REGAPPNAME, Me)
    End Sub

    Private Sub frmWASPInitialize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPPNAME, Me)
    End Sub
End Class