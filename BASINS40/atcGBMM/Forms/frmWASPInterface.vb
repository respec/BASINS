Friend Class frmWASPInterface
    Inherits System.Windows.Forms.Form

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Try
            pSegmentTravelTime = 0
            pSegmentLengthMeters = 0
            Dim pMinSegmentLen As Double
            Dim pMaxSegmentLen As Double
            Dim pMinSegmentTime As Double
            Dim pMaxSegmentTime As Double
            Dim pMinVelocity As Double
            Dim minTravelTime As Double
            If (optionTime.Checked = True) Then
                If (txtTravelTime.Text = "") Then
                    MsgBox("Enter Travel Time ")
                    Exit Sub
                End If
                pSegmentTravelTime = CDbl(txtTravelTime.Text)

                'Add check for minimum travel time to match 2 * Cellsize
                'Call module to compute time and length range
                'modGIStoWasp.FindSegmentationLengthAndTimeRange(pMinSegmentLen, pMaxSegmentLen, pMinSegmentTime, pMaxSegmentTime, pMinVelocity)

                If pMinVelocity > 0 Then
                    minTravelTime = 2 * gCellSize / pMinVelocity / 3600 'Conversion from second to hour

                    If (pSegmentTravelTime < minTravelTime) Then
                        MsgBox("Enter Segment Travel Time greater than " & FormatNumber(minTravelTime, 3, TriState.True))
                        Exit Sub
                    End If
                End If

                If (pSegmentTravelTime <= 0) Then
                    MsgBox("Segment Travel Time cannot be less or equal to zero.")
                    Exit Sub
                End If
            ElseIf (optionLength.Checked = True) Then
                If (txtLength.Text = "") Then
                    MsgBox("Enter Segment Length ")
                    Exit Sub
                End If

                If CDbl(txtLength.Text) < 2 * gCellSize Then
                    MsgBox("Enter Segment Length greater than " & FormatNumber(2 * gCellSize, 1, TriState.True))
                    Exit Sub
                End If


                pSegmentLengthMeters = CDbl(txtLength.Text)
                If (pSegmentLengthMeters <= 0) Then
                    MsgBox("Segment Length cannot be less or equal to zero.")
                    Exit Sub
                End If
            End If

            '?? Dim pContentsView As IContentsView
            Dim pAssessFeatLyr As MapWindow.Interfaces.Layer
            Dim pBranchFeatLyr As MapWindow.Interfaces.Layer
            Dim pDrainFeatLyr As MapWindow.Interfaces.Layer
            If (pSegmentLengthMeters > 0 Or pSegmentTravelTime > 0) Then
                Me.Close()
                '* Call the subroutine to create new tables

                Dim frm As New frmProcess

                frm.Show()

                modGIStoWasp.Step4CreateLinkageToWASPmodel()

                frm.Close()
                frm.Dispose()

                Initialize()

                pDrainFeatLyr = GetInputLayer("Drain")
                If Not pDrainFeatLyr Is Nothing Then pDrainFeatLyr.Visible = False

                pBranchFeatLyr = GetInputLayer("Branches")
                If Not pBranchFeatLyr Is Nothing Then
                    pBranchFeatLyr.Visible = True
                    gMapWin.Layers.MoveLayer(GisUtil.LayerIndex("Branches"), 0, GisUtil.GroupIndex("GBMM"))
                End If

                pAssessFeatLyr = GetInputLayer("AssessPoints")
                If Not pAssessFeatLyr Is Nothing Then
                    pAssessFeatLyr.Visible = True
                    gMapWin.Layers.MoveLayer(GisUtil.LayerIndex("AssessPoints"), 0, GisUtil.GroupIndex("GBMM"))
                End If

                gMapWin.Refresh()

                MsgBox("WASP segmentation completed.")

            Else
                WarningMsg("Select Travel Time or Segment Length in meters.")
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    Private Sub FrmWASPInterface_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'Call the subroutine to open the branches table and find the range
        Dim pMinSegmentLen As Double
        Dim pMaxSegmentLen As Double
        Dim pMinSegmentTime As Double
        Dim pMaxSegmentTime As Double
        Dim pMinVelocity As Double
        'Call module to compute time and length range
        'modGIStoWasp.FindSegmentationLengthAndTimeRange(pMinSegmentLen, pMaxSegmentLen, pMinSegmentTime, pMaxSegmentTime, pMinVelocity)

        Dim minTravelTime As Double
        minTravelTime = pMinSegmentTime
        If (pMinVelocity > 0) Then
            minTravelTime = 2 * gCellSize / pMinVelocity / 3600 'Conversion from second to hours

            If minTravelTime > pMinSegmentTime Then pMinSegmentTime = minTravelTime
        End If
        pMinSegmentLen = 2 * gCellSize
        'Display the time and length range labels
        optionLength.Text = "Enter Segment Length (" & FormatNumber(pMinSegmentLen, 1, TriState.True) & " - " & FormatNumber(pMaxSegmentLen, 1, TriState.True) & "  meters) :"
        optionTime.Text = "Enter Segment Travel Time (" & FormatNumber(minTravelTime, 3, TriState.True) & " - " & FormatNumber(pMaxSegmentTime, 3, TriState.True) & "  hours) :"
    End Sub

    Private Sub optionLength_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionLength.CheckedChanged
        If eventSender.Checked Then
            txtLength.Focus()
        End If
    End Sub

    Private Sub optionTime_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionTime.CheckedChanged
        If eventSender.Checked Then
            txtTravelTime.Focus()
        End If
    End Sub

    Private Sub txtLength_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles txtLength.KeyDown
        Dim keyCode As Short = eventArgs.KeyCode
        Dim Shift As Short = eventArgs.KeyData \ &H10000
        optionLength.Checked = True
    End Sub

    Private Sub txtTravelTime_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles txtTravelTime.KeyDown
        Dim keyCode As Short = eventArgs.KeyCode
        Dim Shift As Short = eventArgs.KeyData \ &H10000
        optionTime.Checked = True
    End Sub
End Class