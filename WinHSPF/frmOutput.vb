Imports atcUCI
Imports atcData
Imports atcControls
Imports atcUtility
Imports MapWinUtility

Public Class frmOutput
    Dim pCheckedRadioIndex As Integer
    Dim pSelectedCell() As Integer = {0, 0}

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        With agdOutput
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Source.FixedRows = 1
            .Visible = True
        End With

        radio1.Checked = True
        cmdCopy.Enabled = False
        RefreshAll()

    End Sub

    Public Sub RefreshAll()
        Dim lObject As Object
        Dim lHspfConnection As HspfConnection
        Dim lHspfOperation As HspfOperation
        Dim lDSNObject As atcTimeseries
        Dim lWDMId, lDSNCoutner, lTempString As String
        Dim lOper As Integer
        Dim lRow As Integer

        If radio1.Checked Then
            txtDesc.Text = "Output will be generated at each 'Hydrology Calibration' output location for " &
              "simulated flow, surface runoff, interflow, base flow, potential evapotranspiration, actual evapotranspiration, " &
              "upper zone storage, lower zone storage, and total moisture supply in default project units at daily interval." & vbCrLf &
            "A Basins Specification (EXS) file will also be generated for the output location, that can used with HSPEXP+ to generate " &
            "calibration statistics and graphs."
            With agdOutput.Source
                .Rows = 0
                .Columns = 2
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Description"

                For lOper = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                    lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper)
                    If lHspfOperation.Name = "RCHRES" Then
                        If IsCalibLocation(lHspfOperation.Name, lHspfOperation.Id) Then
                            'this is an expert system output location
                            lRow = .Rows
                            .CellValue(lRow, 0) = lHspfOperation.Name & " " & lHspfOperation.Id
                            .CellValue(lRow, 1) = lHspfOperation.Description
                        End If
                    End If
                Next lOper
            End With
            cmdCopy.Enabled = False

        ElseIf radio2.Checked Then
            txtDesc.Text = "Streamflow output will be generated at each 'Flow' output location."
            With agdOutput.Source
                .Rows = 0
                .Columns = 2
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Description"

                For lOper = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                    lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper)
                    If lHspfOperation.Name = "RCHRES" Then
                        If IsFlowLocation(lHspfOperation.Name, lHspfOperation.Id) Then
                            'this is an output flow location
                            lRow = .Rows
                            .CellValue(lRow, 0) = lHspfOperation.Name & " " & lHspfOperation.Id
                            .CellValue(lRow, 1) = lHspfOperation.Description
                        End If
                    End If
                Next lOper

            End With
            cmdCopy.Visible = False

        ElseIf radio3.Checked Then
            txtDesc.Text = "Output will be generated at each 'AQUATOX Linkage' output location for " & _
             "inflow, discharge, surface area, mean depth, water temperature, suspended sediment, " & _
             "organic chemicals (if available), and inflows of nutrients, " & _
             "DO, BOD, refractory organic carbon, and sediment."
            With agdOutput.Source
                .Rows = 0
                .Columns = 2
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Description"
                For lOper = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                    lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper)
                    If lHspfOperation.Name = "RCHRES" Then
                        If IsAQUATOXLocation(lHspfOperation.Name, lHspfOperation.Id) Then
                            'this is an expert system output location
                            lRow = .Rows
                            .CellValue(lRow, 0) = lHspfOperation.Name & " " & lHspfOperation.Id
                            .CellValue(lRow, 1) = lHspfOperation.Description
                        End If
                    End If
                Next lOper
            End With
        ElseIf radio4.Checked Then
            txtDesc.Text = "Output will be generated at each 'Other' output location " & "for the specified constituents."
            With agdOutput.Source
                .Rows = 0
                .Columns = 2
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Description"
                .CellValue(0, 2) = "Group/Member"
                For lOper = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                    lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper)
                    'look for any output from here in ext targets
                    For Each lObject In lHspfOperation.Targets
                        lHspfConnection = lObject

                        If Mid(lHspfConnection.Target.VolName, 1, 3) = "WDM" Then
                            If lHspfConnection.Source.VolName = "COPY" Then
                                'assume this is a calibration location, skip it
                            ElseIf lHspfConnection.Source.Group = "ROFLOW" And lHspfConnection.Source.Member = "ROVOL" Then
                                'this is part of the calibration location
                            ElseIf lHspfConnection.Source.Group = "HYDR" And lHspfConnection.Source.Member = "RO" _
                            And IsFlowLocation(lHspfOperation.Name, lHspfOperation.Id) Then
                                'this is an output flow location
                            Else
                                lDSNCoutner = lHspfConnection.Target.VolId
                                lWDMId = lHspfConnection.Target.VolName

                                lDSNObject = pUCI.GetDataSetFromDsn(WDMInd(lWDMId), lDSNCoutner)

                                '.net conversion issue: There is no 'Description' attrib. seen in loaded TS.
                                'At time of writing, lDSNObject could not be set as type ATCclsTserData as class was converted to .net.

                                If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                                    'this an an aquatox output location
                                Else
                                    'this is an other output location
                                    lRow = .Rows
                                    .CellValue(lRow, 0) = lHspfOperation.Name & " " & lHspfOperation.Id
                                    .CellValue(lRow, 1) = lHspfOperation.Description
                                    lTempString = lHspfConnection.Source.Group & ":" & lHspfConnection.Source.Member
                                    If TSMaxSubscript(1, lHspfConnection.Source.Group, lHspfConnection.Source.Member) > 1 Then
                                        lTempString = lTempString & "(" & lHspfConnection.Source.MemSub1
                                        If TSMaxSubscript(2, lHspfConnection.Source.Group, lHspfConnection.Source.Member) > 1 Then
                                            lTempString = lTempString & "," & lHspfConnection.Source.MemSub2
                                        End If
                                        lTempString = lTempString & ")"
                                    End If
                                    .CellValue(lRow, 2) = lTempString
                                End If
                            End If
                        End If
                    Next lObject
                Next lOper
            End With
            cmdCopy.Visible = True
        End If

        agdOutput.SizeAllColumnsToContents()
        agdOutput.Refresh()

    End Sub

    Public Function IsCalibLocation(ByVal aName As String, ByVal aID As Integer) As Boolean
        Dim lHspfOperation As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim lExpertFlag As Boolean
        Dim lCopyID, lOper1, lOper2 As Integer

        IsCalibLocation = False
        lExpertFlag = False

        For lOper1 = 1 To pUCI.OpnSeqBlock.Opns.Count - 1
            lHspfOperation = pUCI.OpnSeqBlock.Opn(lOper1)

            If lHspfOperation.Name = "COPY" Then

                For lOper2 = 0 To lHspfOperation.Targets.Count - 1
                    lHspfConnection = lHspfOperation.Targets(lOper2)

                    If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" And Trim(lHspfConnection.Target.Member) = "IFWO" Then
                        'looks like we have some expert system output locations
                        lExpertFlag = True
                    End If
                Next

            End If

        Next lOper1

        lCopyID = Reach2Copy(aID)

        If lCopyID > 0 Then
            lHspfOperation = pUCI.OpnBlks("COPY").OperFromID(lCopyID)
            For lOper2 = 0 To lHspfOperation.Targets.Count - 1
                lHspfConnection = lHspfOperation.Targets(lOper2)
                If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" And _
                  Trim(lHspfConnection.Target.Member) = "IFWO" And lExpertFlag Then
                    'this is an expert system output location
                    IsCalibLocation = True
                End If
            Next lOper2
        End If

    End Function

    Public Function IsFlowLocation(ByVal aName As String, ByVal aId As Integer) As Boolean
        Dim lOper As Integer
        Dim lHspfOperation As HspfOperation
        Dim lHspfConnection As HspfConnection

        IsFlowLocation = False

        If aId > 0 Then
            lHspfOperation = pUCI.OpnBlks(aName).OperFromID(aId)

            For lOper = 0 To lHspfOperation.Targets.Count - 1
                lHspfConnection = lHspfOperation.Targets(lOper)
                If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" And _
                  Trim(lHspfConnection.Target.Member) = "FLOW" Then
                    'this is an output flow location
                    IsFlowLocation = True
                End If
            Next lOper
        End If

    End Function

    Private Shared Function WDMInd(ByVal aWDMId As String) As Long
        Dim lString As String

        If Len(aWDMId) > 3 Then
            lString = Mid(aWDMId, 4, 1)
            If lString = " " Then lString = "1"
        Else
            lString = "1"
        End If
        WDMInd = lString
    End Function

    Public Function TSMaxSubscript(ByVal aSubNumber As Integer, ByVal aGroup As String, ByVal aMember As String)
        Dim lOper1, lOper2 As Integer

        TSMaxSubscript = 0

        For lOper1 = 0 To pMsg.TSGroupDefs.Count - 1

            If pMsg.TSGroupDefs.Item(lOper1).Name = "group" Then
                For lOper2 = 1 To pMsg.TSGroupDefs(lOper1).MemberDefs.Count
                    If pMsg.TSGroupDefs(lOper1).MemberDefs(lOper2).Name = aMember Then
                        If aSubNumber = 1 Then
                            TSMaxSubscript = pMsg.TSGroupDefs(lOper1).MemberDefs(lOper2).Maxsb1
                        Else
                            TSMaxSubscript = pMsg.TSGroupDefs(lOper1).MemberDefs(lOper2).Maxsb2
                        End If
                        Exit For
                    End If
                Next lOper2
                Exit For
            End If
        Next lOper1
    End Function

    Public Shared Function IsAQUATOXLocation(ByVal aName As String, ByVal aId As Integer) As Boolean
        'call it an aquatox loc if required sections are on and

        Dim lHspfTable As HspfTable
        Dim lHspfOperation As HspfOperation
        Dim lOper As Integer
        Dim lFoundFlag(7) As Boolean
        Dim lDSNId, lWDMId As String

        IsAQUATOXLocation = False
        lHspfOperation = pUCI.OpnBlks(aName).OperFromID(aId)
        lHspfTable = lHspfOperation.Tables("ACTIVITY")
        If lHspfTable.Parms(0).Value = 1 And lHspfTable.Parms(3).Value = 1 And _
           lHspfTable.Parms(4).Value = 1 And lHspfTable.Parms(6).Value = 1 And _
           lHspfTable.Parms(7).Value = 1 Then
            'all required rchres sections are on
            '(hydr, htrch, sedtrn, oxrx, nutrx)
            For lOper = 1 To 7
                lFoundFlag(lOper) = False
            Next lOper
            For Each lHspfConnection As HspfConnection In lHspfOperation.Targets
                If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" Then
                    lDSNId = lHspfConnection.Target.VolId
                    lWDMId = lHspfConnection.Target.VolName
                    Dim lDSNObject As atcData.atcTimeseries = pUCI.GetDataSetFromDsn(WDMInd(lWDMId), lDSNId)
                    If Trim(lHspfConnection.Source.Member) = "AVDEP" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(1) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "SAREA" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(2) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "IVOL" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(3) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "RO" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(4) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "TW" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(5) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "NUIF1" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(6) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "OXIF" Then
                        If InStr(1, UCase(lDSNObject.Attributes.GetValue("Description")), "AQUATOX") Then
                            lFoundFlag(7) = True
                        End If
                    End If
                End If
            Next
            If lFoundFlag(1) And lFoundFlag(2) And lFoundFlag(3) And lFoundFlag(4) And _
               lFoundFlag(5) And lFoundFlag(6) And lFoundFlag(7) Then
                'this is an aquatox output location
                IsAQUATOXLocation = True
            End If
        End If

    End Function

    Private Function Reach2Copy(ByVal aReachId As Integer) As Integer
        'given a reach id, find its associated copy for expert system datasets

        Dim lHspfOperation As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim lOper1, lOper2, lCopyID As Integer
        Dim lArea, ldsn As Double

        lCopyID = 0
        lHspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(aReachId)
        For lOper2 = 1 To lHspfOperation.Targets.Count - 1
            lHspfConnection = lHspfOperation.Targets(lOper2)
            If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" And _
              Trim(lHspfConnection.Target.Member) = "SIMQ" Then
                'this is an expert system output locn, save area and dsn
                ldsn = lHspfConnection.Target.VolId
                lArea = lHspfConnection.MFact
            End If
        Next lOper2

        Dim Dummy As Object = pUCI.OpnSeqBlock.Opns

        For lOper1 = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
            lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper1)
            If lHspfOperation.Name = "COPY" Then
                For lOper2 = 0 To lHspfOperation.Targets.Count - 1
                    lHspfConnection = lHspfOperation.Targets(lOper2)
                    If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" And _
                      Trim(lHspfConnection.Target.Member) = "IFWO" Then
                        If Math.Abs(lArea - (lHspfConnection.MFact * 12)) < 0.000001 Then
                            'this appears to be the associated copy
                            lCopyID = lHspfOperation.Id
                        End If
                    End If
                Next lOper2
            End If
        Next lOper1
        Reach2Copy = lCopyID
    End Function

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub radio1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio1.CheckedChanged
        pCheckedRadioIndex = 1
        RefreshAll()
    End Sub

    Private Sub radio2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio2.CheckedChanged
        pCheckedRadioIndex = 2
        RefreshAll()
    End Sub

    Private Sub radio3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio3.CheckedChanged
        pCheckedRadioIndex = 3
        cmdCopy.Enabled = True
        RefreshAll()
    End Sub

    Private Sub radio4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio4.CheckedChanged
        pCheckedRadioIndex = 4
        cmdCopy.Enabled = True
        RefreshAll()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        If IsNothing(pfrmAddExpert) Then
            pfrmAddExpert = New frmAddExpert(pCheckedRadioIndex)
            pfrmAddExpert.Show()
        Else
            If pfrmAddExpert.IsDisposed Then
                pfrmAddExpert = New frmAddExpert(pCheckedRadioIndex)
                pfrmAddExpert.Show()
            Else
                pfrmAddExpert.WindowState = FormWindowState.Normal
                pfrmAddExpert.BringToFront()
            End If
        End If
    End Sub

    Private Sub agdOutput_Click(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdOutput.MouseDownCell
        pSelectedCell(0) = aRow
        pSelectedCell(1) = aColumn

        For lRow As Integer = 1 To agdOutput.Source.Rows - 1
            For lCol As Integer = 0 To agdOutput.Source.Columns - 1
                agdOutput.Source.CellSelected(lRow, lCol) = False
                agdOutput.Source.CellSelected(lRow, lCol) = False
            Next
        Next
        For lCol As Integer = 0 To agdOutput.Source.Columns - 1
            agdOutput.Source.CellSelected(aRow, lCol) = True
        Next
        Refresh()

    End Sub

    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click

        Dim lRowIndex, lCopyId, lReachIndex, lSelectedCount, lIndex1, lIndex2 As Integer
        Dim lHspfConnection As HspfConnection
        Dim lTargetHspfConnection As HspfConnection
        Dim lHspfOperation As HspfOperation
        Dim lColonPosition, lId, lDsnIndex, lLeftParenthPosition, lCommaPosition, lRightParenthPosition As Integer
        Dim lWdmId As String
        Dim lSpacePosition, lSub1, lSub2, lOffsetAfterDeleteIndex As Integer
        Dim lTempString, lOperName, lGroup, lMember, lCurrentReachString As String
        Dim lDsnObject As atcTimeseries
        Dim lDialogBoxResult As System.Windows.Forms.DialogResult
        Dim lHspfConnectionIndex, lTargetIndex As Integer
        Dim lRemoveUciConnectionAtIndex As New Collection
        Dim lRemoveWdmDataSetVolName, lRemoveWdmDataSetVolId, lRemoveDsnAtIndex As New Collection
        Dim lRemoveTargetVolName, lRemoveTargetVolId, lRemoveTargetAtIndex As New Collection

        '.net conversion issue: The structure of this subroutine has been changed such that removal of WdmDataSets, Targets, UCI entries
        'are done OUTSIDE of loops which locate them. There was a problem with vb6 code deleting collection items while looping through
        'that collection. This was fixed by adding the collection vectors to keep track of which items to delete once outside the loop(s).
        'Additionally, atcGrid had not implemented multicell select, so no testing was done to verify removal of more than one row.
        '
        'Written by Brandon G. on 2009.01.19 


        If radio1.Checked Then 'calibration locations
            lSelectedCount = 0
            For lRowIndex = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = lRowIndex Then
                    'something is selected
                    lCurrentReachString = agdOutput.Source.CellValue(lRowIndex, 0)
                    'find copy operation associated with crch
                    lReachIndex = CLng(Mid(lCurrentReachString, 7))
                    lCopyId = Reach2Copy(lReachIndex)

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the WDM timeseries associated with this calibration location?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.No)

                    If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then

                        lRemoveTargetVolName.Clear()
                        lRemoveTargetVolId.Clear()

                        'build collectino of indexes of ext targets datasets to be removed for this copy
                        For lIndex2 = 0 To pUCI.Connections.Count - 1
                            lHspfConnection = pUCI.Connections.Item(lIndex2)
                            If lHspfConnection.Typ = 4 AndAlso Trim(lHspfConnection.Source.VolName) = "COPY" AndAlso lHspfConnection.Source.VolId = lCopyId Then 'this is one
                                'delete this dsn
                                lRemoveTargetVolName.Add(lHspfConnection.Target.VolName)
                                lRemoveTargetVolId.Add(lHspfConnection.Target.VolId)
                            End If
                        Next
                    End If

                    For lIndex2 = 1 To lRemoveTargetVolName.Count
                        pUCI.DeleteWDMDataSet(lRemoveTargetVolName.Item(lIndex2), lRemoveTargetVolId.Item(lIndex2))
                    Next

                    'now delete this copy operation
                    If lCopyId > 0 Then
                        Call pUCI.DeleteOperation("COPY", lCopyId)
                    End If

                    lRemoveTargetVolName.Clear()
                    lRemoveTargetVolId.Clear()
                    lRemoveTargetAtIndex.Clear()

                    For lHspfConnectionIndex = 0 To pUCI.Connections.Count - 1
                        lHspfConnection = pUCI.Connections(lHspfConnectionIndex)

                        If lHspfConnection.Typ = 4 AndAlso lHspfConnection.Source.VolName = "RCHRES" AndAlso lHspfConnection.Source.VolId = lReachIndex AndAlso Trim(lHspfConnection.Target.Member) = "SIMQ" Then 'this is the one

                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lHspfConnection.Target.VolName, lHspfConnection.Target.VolId)

                            End If
                            'remove the connection

                            'also remove connection from operation
                            lHspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lReachIndex)

                            For lTargetIndex = 0 To lHspfOperation.Targets.Count - 1
                                lTargetHspfConnection = lHspfOperation.Targets.Item(lTargetIndex)
                                If Mid(lTargetHspfConnection.Target.VolName, 1, 3) = "WDM" AndAlso Trim(lTargetHspfConnection.Target.Member) = "SIMQ" Then
                                    lRemoveTargetAtIndex.Add(lTargetIndex)
                                End If
                            Next lTargetIndex

                            lOffsetAfterDeleteIndex = 0
                            For lIndex2 = 1 To lRemoveTargetAtIndex.Count
                                lHspfOperation.Targets.RemoveAt(lRemoveTargetAtIndex(lIndex2) - lOffsetAfterDeleteIndex)
                                lOffsetAfterDeleteIndex += 1
                            Next

                        End If
                    Next

                    lSelectedCount += 1
                End If

            Next lRowIndex
            If lSelectedCount > 0 Then
                RefreshAll()
            Else
                Logger.Message("No location is selected to remove.", "Remove Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If

        ElseIf radio2.Checked = True Then 'flow locations
            'remove
            lSelectedCount = 0
            For lRowIndex = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = lRowIndex Then
                    'something is selected
                    lCurrentReachString = agdOutput.Source.CellValue(lRowIndex, 0)
                    lReachIndex = CLng(Mid(lCurrentReachString, 7))

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the output WDM timeseries?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)

                    lRemoveTargetAtIndex.Clear()
                    lRemoveUciConnectionAtIndex.Clear()

                    lHspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lReachIndex)

                    For lHspfConnectionIndex = 0 To pUCI.Connections.Count - 1
                        lHspfConnection = pUCI.Connections(lHspfConnectionIndex)
                        If lHspfConnection.Typ = 4 AndAlso lHspfConnection.Source.VolName = "RCHRES" AndAlso lHspfConnection.Source.VolId = lReachIndex AndAlso lHspfConnection.Target.Member = "FLOW" Then 'this is the one
                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lHspfConnection.Target.VolName, lHspfConnection.Target.VolId)
                               
                            End If
                            'remove the connection
                            lRemoveUciConnectionAtIndex.Add(lHspfConnectionIndex)
                            'also remove connection from operation
                        End If
                    Next

                    lOffsetAfterDeleteIndex = 0
                    For lIndex2 = 1 To lRemoveUciConnectionAtIndex.Count
                        pUCI.Connections.RemoveAt(lRemoveUciConnectionAtIndex(lIndex2) - lOffsetAfterDeleteIndex)
                        lOffsetAfterDeleteIndex += 1
                    Next

                    lRemoveTargetAtIndex.Clear()

                    For lTargetIndex = 0 To lHspfOperation.Targets.Count - 1
                        lTargetHspfConnection = lHspfOperation.Targets.Item(lTargetIndex)
                        If Mid(lTargetHspfConnection.Target.VolName, 1, 3) = "WDM" AndAlso lTargetHspfConnection.Target.Member = "FLOW" Then
                            lRemoveTargetAtIndex.Add(lTargetIndex)
                        End If
                    Next

                    lOffsetAfterDeleteIndex = 0
                    For lIndex2 = 1 To lRemoveTargetAtIndex.Count
                        lHspfOperation.Targets.RemoveAt(lRemoveTargetAtIndex(lIndex2) - lOffsetAfterDeleteIndex)
                        lOffsetAfterDeleteIndex += 1
                    Next


                    lSelectedCount += 1
                End If
            Next lRowIndex
            If lSelectedCount > 0 Then
                RefreshAll()
            Else
                Logger.Message("No location is selected to remove.", "Remove Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If

        ElseIf radio4.Checked = True Then 'remove other
            lSelectedCount = 0
            For lRowIndex = 1 To agdOutput.Source.Rows - 1
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = lRowIndex Then
                    'something is selected
                    lTempString = agdOutput.Source.CellValue(lRowIndex, 0)
                    lSpacePosition = InStr(1, lTempString, " ")
                    lOperName = Mid(lTempString, 1, lSpacePosition - 1)
                    lId = CInt(Mid(lTempString, lSpacePosition + 1))
                    lTempString = agdOutput.Source.CellValue(lRowIndex, 2)
                    lColonPosition = InStr(1, lTempString, ":")
                    lGroup = Mid(lTempString, 1, lColonPosition - 1)
                    lMember = Mid(lTempString, lColonPosition + 1)
                    lLeftParenthPosition = InStr(1, lMember, "(")
                    lSub1 = 1
                    lSub2 = 1

                    If lLeftParenthPosition > 0 Then
                        lCommaPosition = InStr(1, lMember, ",")
                        If lCommaPosition > 0 Then
                            lSub1 = CInt(Mid(lMember, lLeftParenthPosition + 1, lCommaPosition - lLeftParenthPosition - 1))
                            lRightParenthPosition = InStr(1, lMember, ")")
                            lSub2 = CInt(Mid(lMember, lCommaPosition + 1, lRightParenthPosition - lCommaPosition - 1))
                            lMember = Mid(lMember, 1, lLeftParenthPosition - 1)
                        Else
                            lRightParenthPosition = InStr(1, lMember, ")")
                            lSub1 = CInt(Mid(lMember, lLeftParenthPosition + 1, lRightParenthPosition - lLeftParenthPosition - 1))
                            lSub2 = 1
                            lMember = Mid(lMember, 1, lLeftParenthPosition - 1)
                        End If
                    End If

                    'remove the ext targets entry here

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the output WDM timeseries?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)
                    For lHspfConnectionIndex = 0 To pUCI.Connections.Count - 1
                        lHspfConnection = pUCI.Connections(lHspfConnectionIndex)
                        If lHspfConnection.Typ = 4 And lHspfConnection.Source.VolName = lOperName AndAlso lHspfConnection.Source.VolId = lId AndAlso lHspfConnection.Source.Group = lGroup AndAlso lHspfConnection.Source.Member = lMember AndAlso lHspfConnection.Source.MemSub1 = lSub1 And lHspfConnection.Source.MemSub2 = lSub2 Then 'this is the one

                            lRemoveWdmDataSetVolName.Clear()
                            lRemoveWdmDataSetVolId.Clear()
                            lRemoveUciConnectionAtIndex.Clear()
                            lRemoveTargetAtIndex.Clear()

                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lHspfConnection.Target.VolName, lHspfConnection.Target.VolId)
                            End If

                            'remove the connection by adding the index to the list 
                            lRemoveUciConnectionAtIndex.Add(lHspfConnectionIndex)

                            'also remove connection from operation
                            lHspfOperation = pUCI.OpnBlks(lOperName).OperFromID(lId)
                            For lTargetIndex = 0 To lHspfOperation.Targets.Count - 1
                                lTargetHspfConnection = lHspfOperation.Targets.Item(lTargetIndex)
                                If Mid(lTargetHspfConnection.Target.VolName, 1, 3) = "WDM" AndAlso lTargetHspfConnection.Source.Group = lGroup AndAlso lTargetHspfConnection.Source.Member = lMember AndAlso lTargetHspfConnection.Source.MemSub1 = lSub1 AndAlso lTargetHspfConnection.Source.MemSub2 = lSub2 Then
                                    lRemoveTargetAtIndex.Add(lTargetIndex)
                                End If
                            Next lTargetIndex

                            lOffsetAfterDeleteIndex = 0
                            For lIndex2 = 1 To lRemoveTargetAtIndex.Count
                                lHspfOperation.Targets.RemoveAt(lRemoveTargetAtIndex(lIndex2) - lOffsetAfterDeleteIndex)
                                lOffsetAfterDeleteIndex += 1
                            Next

                        End If
                    Next

                    lOffsetAfterDeleteIndex = 0
                    For lIndex2 = 1 To lRemoveUciConnectionAtIndex.Count
                        pUCI.Connections.RemoveAt(lRemoveUciConnectionAtIndex(lIndex2) - lOffsetAfterDeleteIndex)
                        lOffsetAfterDeleteIndex += 1
                    Next

                    lSelectedCount += 1
                End If
            Next lRowIndex
            If lSelectedCount > 0 Then
                RefreshAll()
            Else
                Logger.Message("No location is selected to remove.", "Remove Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If

        ElseIf radio3.Checked = True Then 'remove aquatox location
            lSelectedCount = 0
            For lRowIndex = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selectedl(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = lRowIndex Then
                    'something is selected
                    lCurrentReachString = agdOutput.Source.CellValue(lRowIndex, 0)
                    lReachIndex = CLng(Mid(lCurrentReachString, 7))

                    lIndex1 = 0
                    For lHspfConnectionIndex = 0 To pUCI.Connections.Count - 1
                        lHspfConnection = pUCI.Connections(lHspfConnectionIndex)
                        If lHspfConnection.Typ = 4 AndAlso lHspfConnection.Source.VolName = "RCHRES" AndAlso lHspfConnection.Source.VolId = lReachIndex Then 'this is the one
                            lWdmId = lHspfConnection.Target.VolName
                            lDsnIndex = lHspfConnection.Target.VolId
                            lDsnObject = pUCI.GetDataSetFromDsn(WDMInd(lWdmId), lDsnIndex)
                            If InStr(1, UCase(lDsnObject.Attributes.GetValue("Description")), "AQUATOX") Then
                                lIndex1 += 1
                            End If
                        End If
                    Next

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the " & lIndex1 & " WDM timeseries " & vbCrLf & "associated with this AQUATOX Linkage location?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)

                    'remove the ext targets and dsns

                    lRemoveWdmDataSetVolId.Clear()
                    lRemoveDsnAtIndex.Clear()
                    lRemoveUciConnectionAtIndex.Clear()

                    For lHspfConnectionIndex = 0 To pUCI.Connections.Count - 1
                        lHspfConnection = pUCI.Connections(lHspfConnectionIndex)

                        If lHspfConnection.Typ = 4 AndAlso lHspfConnection.Source.VolName = "RCHRES" AndAlso lHspfConnection.Source.VolId = lReachIndex Then 'this is the one
                            lWdmId = lHspfConnection.Target.VolName
                            lDsnIndex = lHspfConnection.Target.VolId
                            lDsnObject = pUCI.GetDataSetFromDsn(WDMInd(lWdmId), lDsnIndex)
                            If InStr(1, UCase(lDsnObject.Attributes.GetValue("Description")), "AQUATOX") Then
                                'found aquatox dsn
                                If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                    'delete this dsn
                                    pUCI.DeleteWDMDataSet(lWdmId, lDsnIndex)
                                End If
                                'remove the connection

                                lRemoveUciConnectionAtIndex.Add(lHspfConnectionIndex)

                                'also remove connection from operation
                                lHspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lReachIndex)

                                For lTargetIndex = 0 To lHspfOperation.Targets.Count - 1
                                    lTargetHspfConnection = lHspfOperation.Targets.Item(lTargetIndex)

                                    If lTargetHspfConnection.Target.VolName = lWdmId AndAlso lTargetHspfConnection.Target.VolId = lDsnIndex Then
                                        lRemoveTargetAtIndex.Add(lTargetIndex)
                                    End If
                                Next

                                lOffsetAfterDeleteIndex = 0
                                For lIndex2 = 1 To lRemoveTargetAtIndex.Count
                                    lHspfOperation.Targets.RemoveAt(lRemoveTargetAtIndex.Item(lIndex2) - lOffsetAfterDeleteIndex)
                                    lOffsetAfterDeleteIndex += 1
                                Next

                            End If
                        End If
                    Next

                    lOffsetAfterDeleteIndex = 0
                    For lIndex2 = 1 To lRemoveUciConnectionAtIndex.Count
                        pUCI.Connections.RemoveAt(lRemoveUciConnectionAtIndex.Item(lIndex2) - lOffsetAfterDeleteIndex)
                        lOffsetAfterDeleteIndex += 1
                    Next

                    lSelectedCount += 1
                End If
            Next lRowIndex
            If lSelectedCount > 0 Then
                RefreshAll()
            Else
                Logger.Message("No location is selected to remove.", "Remove Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        End If

        agdOutput.SizeAllColumnsToContents()
        agdOutput.Refresh()

    End Sub

    Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click

        If IsNothing(pfrmAddExpert) Then
            pfrmAddExpert = New frmAddExpert(5)
            pfrmAddExpert.Show()
        Else
            If pfrmAddExpert.IsDisposed Then
                pfrmAddExpert = New frmAddExpert(5)
                pfrmAddExpert.Show()
            Else
                pfrmAddExpert.WindowState = FormWindowState.Normal
                pfrmAddExpert.BringToFront()
            End If
        End If

    End Sub

    Private Sub frmOutput_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Output Manager.html")
        End If
    End Sub
End Class