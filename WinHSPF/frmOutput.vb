Imports atcUCI
Imports atcData
Imports atcControls
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
            txtDesc.Text = "Output will be generated at each 'Hydrology Calibration' output location for " & _
              "total runoff, surface runoff, interflow, base flow, potential evapotranspiration, actual evapotranspiration, " & _
              "upper zone storage, and lower zone storage."
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
                            .Rows = .Rows + 1
                            .CellValue(.Rows, 0) = lHspfOperation.Name & " " & lHspfOperation.Id
                            .CellValue(.Rows, 1) = lHspfOperation.Description
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
                            ElseIf lHspfConnection.Source.Group = "HYDR" And lHspfConnection.Source.Member = "RO" And IsFlowLocation(lHspfOperation.Name, lHspfOperation.Id) Then
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

    Private Function WDMInd(ByVal aWDMId As String) As Long
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

    Public Function IsAQUATOXLocation(ByVal aName As String, ByVal aId As Integer) As Boolean
        'call it an aquatox loc if required sections are on and

        Dim lDSNObject As Object
        Dim lHspfTable As HspfTable
        Dim lHspfOperation As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim lOper As Integer
        Dim lFoundFlag(7) As Boolean
        Dim lDSNId, lWDMId As String

        IsAQUATOXLocation = False
        lHspfOperation = pUCI.OpnBlks(aName).OperFromID(aId)
        lHspfTable = lHspfOperation.Tables("ACTIVITY")
        If lHspfTable.Parms(1).Value = 1 And lHspfTable.Parms(4).Value = 1 And _
           lHspfTable.Parms(5).Value = 1 And lHspfTable.Parms(7).Value = 1 And _
           lHspfTable.Parms(8).Value = 1 Then
            'all required rchres sections are on
            '(hydr, htrch, sedtrn, oxrx, nutrx)
            For lOper = 1 To 7
                lFoundFlag(lOper) = False
            Next lOper
            For lOper = 1 To lHspfOperation.Targets.Count
                lHspfConnection = lHspfOperation.Targets(lOper)
                If Microsoft.VisualBasic.Left(lHspfConnection.Target.VolName, 3) = "WDM" Then
                    lDSNId = lHspfConnection.Target.VolId
                    lWDMId = lHspfConnection.Target.VolName
                    lDSNObject = pUCI.GetDataSetFromDsn(WDMInd(lWDMId), lDSNId)
                    If Trim(lHspfConnection.Source.Member) = "AVDEP" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(1) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "SAREA" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(2) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "IVOL" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(3) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "RO" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(4) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "TW" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(5) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "NUIF1" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(6) = True
                        End If
                    ElseIf Trim(lHspfConnection.Source.Member) = "OXIF" Then
                        If InStr(1, UCase(lDSNObject.Header.Desc), "AQUATOX") Then
                            lFoundFlag(7) = True
                        End If
                    End If
                End If
            Next lOper
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
        RefreshAll()
    End Sub

    Private Sub radio4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio4.CheckedChanged
        pCheckedRadioIndex = 4
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


    End Sub


    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click

        Dim i&, copyid&, irch&, nsel&, crch$, j&
        Dim vConn As Object, lConn As HspfConnection
        Dim vtarget As Object, ltarget As HspfConnection
        Dim lOpn As HspfOperation
        Dim k As Integer
        Dim colonpos&, Id&, WDMId$, idsn&, parpos&, commapos&, lparpos&
        Dim spacepos&, ctemp$, opname$, group$, member$, sub1&, sub2&
        Dim dsnObj As atcTimeseries
        Dim lDialogBoxResult As System.Windows.Forms.DialogResult

        MsgBox(pSelectedCell(0) & " " & pSelectedCell(1))

        If radio1.Checked Then 'calibration locations
            nsel = 0
            For i = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = i AndAlso pSelectedCell(1) = 0 Then
                    'something is selected
                    crch = agdOutput.Source.CellValue(i, 0)
                    'find copy operation associated with crch
                    irch = CLng(Mid(crch, 7))
                    copyid = Reach2Copy(irch)

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the WDM timeseries associated with this calibration location?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.No) = Windows.Forms.DialogResult.Yes

                    If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                        'remove the ext targets datasets for this copy
                        For Each vConn In pUCI.Connections
                            lConn = vConn
                            If lConn.Typ = 4 And Trim(lConn.Source.VolName) = "COPY" And _
                               lConn.Source.VolId = copyid Then 'this is one
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lConn.Target.VolName, lConn.Target.VolId)
                            End If
                        Next vConn
                    End If

                    'now delete this copy operation
                    If copyid > 0 Then
                        Call pUCI.DeleteOperation("COPY", copyid)
                    End If
                    'remove the ext targets entry for simq here
                    j = 1
                    For Each vConn In pUCI.Connections
                        lConn = vConn
                        If lConn.Typ = 4 And lConn.Source.VolName = "RCHRES" And _
                           lConn.Source.VolId = irch And Trim(lConn.Target.Member) = "SIMQ" Then 'this is the one
                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lConn.Target.VolName, lConn.Target.VolId)
                            End If
                            'remove the connection
                            pUCI.Connections.RemoveAt(j)
                            'also remove connection from operation
                            lOpn = pUCI.OpnBlks("RCHRES").OperFromID(irch)
                            k = 1
                            For Each vtarget In lOpn.Targets
                                ltarget = vtarget
                                If Mid(ltarget.Target.VolName, 1, 3) = "WDM" And _
                                   Trim(ltarget.Target.Member) = "SIMQ" Then
                                    lOpn.Targets.RemoveAt(k)
                                Else
                                    k = k + 1
                                End If
                            Next vtarget
                        Else
                            j = j + 1
                        End If
                    Next vConn
                    nsel = nsel + 1
                End If
            Next i
            If nsel > 0 Then
                RefreshAll()
            Else
                Call MsgBox("No location is selected to remove.", vbOKOnly, "Remove Problem")
            End If
        ElseIf radio2.Checked = True Then 'flow locations
            'remove
            nsel = 0
            For i = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = i AndAlso pSelectedCell(1) = 0 Then
                    'something is selected
                    crch = agdOutput.Source.CellValue(i, 0)
                    irch = CLng(Mid(crch, 7))

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the output WDM timeseries?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)

                    'remove the ext targets entry here
                    j = 1
                    For Each vConn In pUCI.Connections
                        lConn = vConn
                        If lConn.Typ = 4 And lConn.Source.VolName = "RCHRES" And _
                           lConn.Source.VolId = irch And lConn.Target.Member = "FLOW" Then 'this is the one
                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lConn.Target.VolName, lConn.Target.VolId)
                            End If
                            'remove the connection
                            pUCI.Connections.RemoveAt(j)
                            'also remove connection from operation
                            lOpn = pUCI.OpnBlks("RCHRES").OperFromID(irch)
                            k = 1
                            For Each vtarget In lOpn.Targets
                                ltarget = vtarget
                                If Mid(ltarget.Target.VolName, 1, 3) = "WDM" And _
                                   ltarget.Target.Member = "FLOW" Then
                                    lOpn.Targets.RemoveAt(k)
                                Else
                                    k = k + 1
                                End If
                            Next vtarget
                        Else
                            j = j + 1
                        End If
                    Next vConn
                    nsel = nsel + 1
                End If
            Next i
            If nsel > 0 Then
                RefreshAll()
            Else
                Call MsgBox("No location is selected to remove.", vbOKOnly, "Remove Problem")
            End If
        ElseIf radio4.Checked = True Then 'remove other
            nsel = 0
            For i = 1 To agdOutput.Source.Rows - 1
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = i AndAlso pSelectedCell(1) = 0 Then
                    'something is selected
                    ctemp = agdOutput.Source.CellValue(i, 0)
                    spacepos = InStr(1, ctemp, " ")
                    opname = Mid(ctemp, 1, spacepos - 1)
                    Id = CInt(Mid(ctemp, spacepos + 1))
                    ctemp = agdOutput.Source.CellValue(i, 2)
                    colonpos = InStr(1, ctemp, ":")
                    group = Mid(ctemp, 1, colonpos - 1)
                    member = Mid(ctemp, colonpos + 1)
                    parpos = InStr(1, member, "(")
                    sub1 = 1
                    sub2 = 1
                    If parpos > 0 Then
                        commapos = InStr(1, member, ",")
                        If commapos > 0 Then
                            sub1 = CInt(Mid(member, parpos + 1, commapos - parpos - 1))
                            lparpos = InStr(1, member, ")")
                            sub2 = CInt(Mid(member, commapos + 1, lparpos - commapos - 1))
                            member = Mid(member, 1, parpos - 1)
                        Else
                            lparpos = InStr(1, member, ")")
                            sub1 = CInt(Mid(member, parpos + 1, lparpos - parpos - 1))
                            sub2 = 1
                            member = Mid(member, 1, parpos - 1)
                        End If
                    End If

                    'remove the ext targets entry here

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the output WDM timeseries?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)

                    j = 1
                    For Each vConn In pUCI.Connections
                        lConn = vConn
                        If lConn.Typ = 4 And lConn.Source.VolName = opname And _
                           lConn.Source.VolId = Id And lConn.Source.Group = group And _
                           lConn.Source.Member = member And _
                           lConn.Source.MemSub1 = sub1 And lConn.Source.MemSub2 = sub2 Then 'this is the one
                            If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                'delete this dsn
                                pUCI.DeleteWDMDataSet(lConn.Target.VolName, lConn.Target.VolId)
                            End If
                            'remove the connection
                            pUCI.Connections.RemoveAt(j)
                            'also remove connection from operation
                            lOpn = pUCI.OpnBlks(opname).OperFromID(Id)
                            k = 1
                            For Each vtarget In lOpn.Targets
                                ltarget = vtarget
                                If Mid(ltarget.Target.VolName, 1, 3) = "WDM" And _
                                   ltarget.Source.Group = group And _
                                   ltarget.Source.Member = member And _
                                   ltarget.Source.MemSub1 = sub1 And ltarget.Source.MemSub2 = sub2 Then
                                    lOpn.Targets.RemoveAt(k - 1)
                                Else
                                    k = k + 1
                                End If
                            Next vtarget
                        Else
                            j = j + 1
                        End If
                    Next vConn
                    nsel = nsel + 1
                End If
            Next i
            If nsel > 0 Then
                RefreshAll()
            Else
                Call MsgBox("No output is selected to remove.", vbOKOnly, "Remove Problem")
            End If
        ElseIf radio3.Checked = True Then 'remove aquatox location
            nsel = 0
            For i = 1 To agdOutput.Source.Rows
                '.net conversion issue: atcGrid need more work for the next line to really work
                'If agdOutput.Selected(i, 0) And agdOutput.SelEndCol <> agdOutput.SelStartCol Then
                If pSelectedCell(0) = i AndAlso pSelectedCell(1) = 0 Then
                    'something is selected
                    crch = agdOutput.Source.CellValue(i, 0)
                    irch = CLng(Mid(crch, 7))

                    j = 0
                    For Each vConn In pUCI.Connections
                        lConn = vConn
                        If lConn.Typ = 4 And lConn.Source.VolName = "RCHRES" And _
                           lConn.Source.VolId = irch Then 'this is the one
                            WDMId = lConn.Target.VolName
                            idsn = lConn.Target.VolId
                            dsnObj = pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn)
                            If InStr(1, UCase(dsnObj.Attributes.GetValue("Description")), "AQUATOX") Then
                                j = j + 1
                            End If
                        End If
                    Next vConn

                    lDialogBoxResult = Logger.Message("Do you want to permanently delete the " & j & " WDM timeseries " & vbCrLf & "associated with this AQUATOX Linkage location?", "Delete Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.No)

                    'remove the ext targets and dsns
                    j = 1
                    For Each vConn In pUCI.Connections
                        lConn = vConn
                        If lConn.Typ = 4 And lConn.Source.VolName = "RCHRES" And _
                           lConn.Source.VolId = irch Then 'this is the one
                            WDMId = lConn.Target.VolName
                            idsn = lConn.Target.VolId
                            dsnObj = pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn)
                            If InStr(1, UCase(dsnObj.Attributes.GetValue("Description")), "AQUATOX") Then
                                'found aquatox dsn
                                If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                                    'delete this dsn
                                    pUCI.DeleteWDMDataSet(WDMId, idsn)
                                End If
                                'remove the connection
                                pUCI.Connections.RemoveAt(j)
                                'also remove connection from operation
                                lOpn = pUCI.OpnBlks("RCHRES").OperFromID(irch)
                                k = 1
                                For Each vtarget In lOpn.Targets
                                    ltarget = vtarget
                                    If ltarget.Target.VolName = WDMId And _
                                      ltarget.Target.VolId = idsn Then
                                        lOpn.Targets.RemoveAt(k)
                                    Else
                                        k = k + 1
                                    End If
                                Next vtarget
                            Else
                                j = j + 1
                            End If
                        Else
                            j = j + 1
                        End If
                    Next vConn
                    nsel = nsel + 1
                End If
            Next i
            If nsel > 0 Then
                RefreshAll()
            Else
                Logger.Message("No location is selected to remove.", "Remove Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        End If

        agdOutput.SizeAllColumnsToContents()
        agdOutput.Refresh()


    End Sub
End Class