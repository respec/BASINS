Imports atcData
Imports atcUCI
Imports atcUtility
Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class frmTime

    Dim pCurrentSelectedRow As Integer

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        With agdMet
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.FixedRows = 1
        End With

        With pUCI.GlobalBlock

            txtStartYear.Text = .SDate(0)
            txtStartMonth.Text = .SDate(1)
            txtStartDay.Text = .SDate(2)
            txtStartHour.Text = .SDate(3)
            txtStartMinute.Text = .SDate(4)

            txtEndYear.Text = .EDate(0)
            txtEndMonth.Text = .EDate(1)
            txtEndDay.Text = .EDate(2)
            txtEndHour.Text = .EDate(3)
            txtEndMinute.Text = .EDate(4)

        End With

        RefreshGrid()

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        If IsNothing(pfrmAddMet) Then
            pfrmAddMet = New frmAddMet
            pfrmAddMet.Init("", 0)
            pfrmAddMet.Show()
        Else
            If pfrmAddMet.IsDisposed Then
                pfrmAddMet = New frmAddMet
                pfrmAddMet.Init("", 0)
                pfrmAddMet.Show()
            Else
                pfrmAddMet.WindowState = FormWindowState.Normal
                pfrmAddMet.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click

        If pCurrentSelectedRow > 0 Then
            If agdMet.Source.CellValue(pCurrentSelectedRow, 0) = "<none>" Then
                Logger.Msg("Unable to Edit Met Segment <none>.", MsgBoxStyle.OkOnly, "Edit Met Segment Problem")
            Else
                If IsNothing(pfrmEditMet) Then
                    pfrmEditMet = New frmAddMet
                    pfrmEditMet.Init(agdMet.Source.CellValue(pCurrentSelectedRow, 0), 1)
                    pfrmEditMet.Show()
                Else
                    If pfrmEditMet.IsDisposed Then
                        pfrmEditMet = New frmAddMet
                        pfrmEditMet.Init(agdMet.Source.CellValue(pCurrentSelectedRow, 0), 1)
                        pfrmEditMet.Show()
                    Else
                        pfrmEditMet.WindowState = FormWindowState.Normal
                        pfrmEditMet.BringToFront()
                    End If
                End If
            End If
        Else
            Logger.Msg("Select a Met Segment from the list below.", MsgBoxStyle.OkOnly, "Edit Met Segment Problem")
        End If

    End Sub

    Private Sub RefreshGrid()

        With agdMet.Source
            .Rows = 0
            .Columns = 2
            .CellValue(0, 0) = "Met Seg ID"
            .CellValue(0, 1) = "Operation"
            For Each lHSPFOperation As HspfOperation In pUCI.OpnSeqBlock.Opns
                If lHSPFOperation.Name = "PERLND" Or lHSPFOperation.Name = "IMPLND" Or lHSPFOperation.Name = "RCHRES" Then
                    Dim lRow As Integer = .Rows
                    Dim lmetseg As HspfMetSeg = lHSPFOperation.MetSeg
                    If lmetseg Is Nothing Then
                        .CellValue(lRow, 0) = "<none>"
                    Else
                        .CellValue(lRow, 0) = lmetseg.Name
                    End If
                    .CellValue(lRow, 1) = lHSPFOperation.Name & " " & lHSPFOperation.Id
                    .CellEditable(lRow, 0) = True
                End If
            Next
        End With

        agdMet.SizeAllColumnsToContents()
        agdMet.Refresh()

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'okay
        Dim lGlobal As HspfGlobalBlk = pUCI.GlobalBlock
        For i As Integer = 0 To 4
            'put dates back
            lGlobal.SDate(0) = txtStartYear.Text
            lGlobal.SDate(1) = txtStartMonth.Text
            lGlobal.SDate(2) = txtStartDay.Text
            lGlobal.SDate(3) = txtStartHour.Text
            lGlobal.SDate(4) = txtStartMinute.Text

            lGlobal.EDate(0) = txtEndYear.Text
            lGlobal.EDate(1) = txtEndMonth.Text
            lGlobal.EDate(2) = txtEndDay.Text
            lGlobal.EDate(3) = txtEndHour.Text
            lGlobal.EDate(4) = txtEndMinute.Text
        Next i

        For i As Integer = 1 To agdMet.Source.Rows - 1
            'put met segs back
            Dim lTemp As String = agdMet.Source.CellValue(i, 1)
            Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(Mid(lTemp, 1, 6))
            Dim Id As Integer = CInt(Mid(lTemp, 8))
            Dim lOper As HspfOperation = lOpnBlk.OperFromID(Id)
            If agdMet.Source.CellValue(i, 0) = "<none>" Then
                lOper.MetSeg = Nothing
            Else
                For Each lMetseg As HspfMetSeg In pUCI.MetSegs
                    If lMetseg.Name = agdMet.Source.CellValue(i, 0) Then
                        lOper.MetSeg = lMetseg
                    End If
                Next
            End If
        Next i

        'remove unused met segments
        For Each lMetseg As HspfMetSeg In pUCI.MetSegs
            Dim lInuse As Boolean = False
            For i As Integer = 1 To agdMet.Source.Rows - 1
                If agdMet.Source.CellValue(i, 0) = lMetseg.Name Then
                    lInuse = True
                End If
            Next i
            If Not lInuse Then
                pUCI.MetSegs.Remove(lMetseg)
            End If
        Next

        pWinHSPF.SchematicDiagram.ClearTree()
        pWinHSPF.SchematicDiagram.BuildTree()
        pWinHSPF.SchematicDiagram.UpdateLegend()
        pWinHSPF.SchematicDiagram.UpdateDetails()
        Me.Dispose()
    End Sub

    Private Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click

        Dim lUpLandOp As New Collection(Of HspfOperation)

        If agdMet.Source.CellValue(pCurrentSelectedRow, 0) = "<none>" Then
            Logger.Msg("Unable to apply Met Segment <none> to contributing land area.", MsgBoxStyle.OkOnly, "Apply Met Segment Problem")
        Else
            'check if we can apply it to the existing perlnd/implnds, or create new operations
            Dim lId As Integer = CInt(Mid(agdMet.Source.CellValue(pCurrentSelectedRow, 1), 8))
            Dim lOper As HspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lId)
            Dim lSelectedMetSeg As String = agdMet.Source.CellValue(pCurrentSelectedRow, 0)
            'find operations contributing to this rchres
            For Each lConn As HspfConnection In lOper.Sources
                If lConn.Source.Opn.Name = "PERLND" Or lConn.Source.Opn.Name = "IMPLND" Then
                    'found a contributing land area
                    lUpLandOp.Add(lConn.Source.Opn)
                End If
            Next
            If lUpLandOp.Count = 0 Then
                'no upstream land areas
                Logger.Msg("There is no contributing land area to which to apply this Met Segment.", MsgBoxStyle.OkOnly, "Apply Met Segment Problem")
            Else
                'do these land areas contribute to other reaches?
                Dim lGrouped As Boolean = False
                For Each lupOper As HspfOperation In lUpLandOp
                    For Each lConn As HspfConnection In lupOper.Targets
                        If (lConn.Target.Opn.Name = "RCHRES" And lConn.Target.Opn.Id <> lId) _
                          Or lConn.Target.Opn.Name = "BMPRAC" Then
                            'this oper goes to another reach or a bmprac
                            lGrouped = True
                        End If
                    Next
                Next
                Dim lChange As Integer = 0
                For Each lupOper As HspfOperation In lUpLandOp
                    If lupOper.MetSeg.Name <> lSelectedMetSeg Then
                        'need to change
                        lChange = lChange + 1
                    End If
                Next
                If lChange = 0 Then
                    'nothing to change
                    Logger.Msg("The contributing land area is already associated with this Met Segment.", MsgBoxStyle.OkOnly, "Apply Met Segment")
                Else
                    If Not lGrouped Then
                        'no - apply to existing
                        For Each lupOper As HspfOperation In lUpLandOp
                            'find in list
                            For lRow As Integer = 1 To agdMet.Source.Rows
                                Dim lTempOperName As String = Mid(agdMet.Source.CellValue(lRow, 1), 1, 6)
                                Dim lTempOperId As Integer = CInt(Mid(agdMet.Source.CellValue(lRow, 1), 8))
                                If lupOper.Name = lTempOperName And lupOper.Id = lTempOperId Then
                                    agdMet.Source.CellValue(lRow, 0) = agdMet.Source.CellValue(pCurrentSelectedRow, 0)
                                End If
                            Next
                        Next
                    Else
                        'we are using grouped option
                        'do we have an existing p/i set for this met seg?
                        Dim lUpOperCount As Integer = 0
                        Dim lDesc As String
                        For Each lupOper As HspfOperation In lUpLandOp
                            Dim lMoved As Boolean = False
                            For lRow As Integer = 1 To agdMet.Source.Rows
                                If agdMet.Source.CellValue(lRow, 0) = agdMet.Source.CellValue(pCurrentSelectedRow, 0) Then
                                    'this line has the met seg we're looking for
                                    Dim lTempOperName As String = Mid(agdMet.Source.CellValue(lRow, 1), 1, 6)
                                    Dim lTempOperId As Integer = CInt(Mid(agdMet.Source.CellValue(lRow, 1), 8))
                                    lDesc = pUCI.OpnBlks(lTempOperName).OperFromID(lTempOperId).Description
                                    If lupOper.Name = lTempOperName And lupOper.Description = lDesc Then
                                        'this looks like the one we want
                                        lUpOperCount += 1
                                    End If
                                End If
                            Next
                        Next
                        'do we want to move to existing p/i set?
                        Dim lResp As MsgBoxResult
                        If lUpOperCount > 0 Then
                            lResp = Logger.Msg("Do you want to group the contributing area for this reach" & _
                                       vbCrLf & "with the corresponding existing met segment?", _
                                       MsgBoxStyle.YesNo, "Group Met Segment")
                        End If
                        If lResp = MsgBoxResult.Yes Then
                            'do grouping to existing met seg
                            For Each lupOper As HspfOperation In lUpLandOp
                                Dim lMoved As Boolean = False
                                For lRow As Integer = 1 To agdMet.Source.Rows
                                    If agdMet.Source.CellValue(lRow, 0) = agdMet.Source.CellValue(pCurrentSelectedRow, 0) Then
                                        'this line has the met seg we're looking for
                                        Dim lTempOperName As String = Mid(agdMet.Source.CellValue(lRow, 1), 1, 6)
                                        Dim lTempOperId As Integer = CInt(Mid(agdMet.Source.CellValue(lRow, 1), 8))
                                        lDesc = pUCI.OpnBlks(lTempOperName).OperFromID(lTempOperId).Description
                                        If lupOper.Name = lTempOperName And lupOper.Description = lDesc Then
                                            'this looks like the one we want
                                            Dim lTempOper As HspfOperation = pUCI.OpnBlks(lTempOperName).OperFromID(lTempOperId)
                                            'move land area from one set to the other
                                            For Each lConn As HspfConnection In lOper.Sources
                                                If lConn.Source.VolName = lupOper.Name And _
                                                   lConn.Source.VolId = lupOper.Id Then
                                                    lConn.Source.Opn = lTempOper
                                                    lConn.Source.VolId = lTempOper.Id
                                                End If
                                            Next
                                            For Each lConn As HspfConnection In lupOper.Targets
                                                If lConn.Target.VolName = lOper.Name And _
                                                   lConn.Target.VolId = lOper.Id Then
                                                    'remove this target
                                                    lupOper.Targets.Remove(lConn)
                                                    'set this as a target from the new opn
                                                    lTempOper.Targets.Add(lConn)
                                                End If
                                            Next
                                            lMoved = True
                                        End If
                                    End If
                                Next
                                If lMoved Then
                                    lUpLandOp.Remove(lupOper)
                                End If
                            Next
                        End If
                        If lUpLandOp.Count > 0 Then
                            'create new
                            lResp = Logger.Msg(lUpLandOp.Count & " new PERLNDs/IMPLNDs will be created." & vbCrLf & vbCrLf & _
                                               "Do you want to add these new operations?", MsgBoxStyle.YesNo, _
                                               "Apply Met Segment")
                            If lResp = MsgBoxResult.Yes Then
                                'create new operations
                                For Each lupOper As HspfOperation In lUpLandOp
                                    Dim lTempOperName As String = lupOper.Name
                                    Dim lInc As Integer
                                    If lupOper.Id < 900 Then
                                        lInc = 100
                                    Else
                                        lInc = 1
                                    End If
                                    Dim lTempOperId As Integer = lupOper.Id + lInc
                                    'figure out which to put it after in the opn seq block
                                    Dim lAfterId As Integer = 1
                                    For lIndex As Integer = 1 To pUCI.OpnSeqBlock.Opns.Count
                                        If pUCI.OpnSeqBlock.Opn(lIndex).Name = lupOper.Name And _
                                           pUCI.OpnSeqBlock.Opn(lIndex).Id = lupOper.Id Then
                                            lAfterId = lIndex
                                        End If
                                    Next
                                    Do While Not pUCI.OpnBlks(lTempOperName).OperFromID(lTempOperId) Is Nothing
                                        lTempOperId = lTempOperId + lInc
                                    Loop

                                    'add the operation to the uci object
                                    pUCI.AddOperation(lTempOperName, lTempOperId)
                                    Dim lTempOper As HspfOperation = pUCI.OpnBlks(lTempOperName).OperFromID(lTempOperId)
                                    lTempOper.Description = lupOper.Description
                                    lTempOper.MetSeg = lupOper.MetSeg
                                    'add the operation to the opn seq block
                                    pUCI.OpnSeqBlock.AddAfter(lTempOper, lAfterId)
                                    'copy tables from the existing operation
                                    Dim lTabName As String
                                    For Each lTable As HspfTable In lupOper.Tables
                                        If lTable.OccurCount > 1 And lTable.OccurNum > 1 Then
                                            lTabName = lTable.Name & ":" & lTable.OccurNum
                                        Else
                                            lTabName = lTable.Name
                                        End If
                                        pUCI.AddTable(lTempOperName, lTempOperId, lTabName)
                                        For lIndex As Integer = 1 To lTable.Parms.Count
                                            lTempOper.Tables(lTabName).Parms(lIndex).Value = lTable.Parms(lIndex).Value
                                        Next
                                    Next
                                    'add to list
                                    For lRow As Integer = 1 To agdMet.Source.Rows
                                        If lupOper.Name = Mid(agdMet.Source.CellValue(lRow, 1), 1, 6) And _
                                           lupOper.Id = CInt(Mid(agdMet.Source.CellValue(lRow, 1), 8)) Then
                                            'agdMet.InsertRow(j) TODO need equivalent
                                            agdMet.Source.CellValue(lRow + 1, 0) = lSelectedMetSeg
                                            agdMet.Source.CellValue(lRow + 1, 1) = lTempOperName & " " & lTempOperId
                                            Exit For
                                        End If
                                    Next
                                    'move land area from one set to the other
                                    For Each lConn As HspfConnection In lOper.Sources
                                        If lConn.Source.VolName = lupOper.Name And _
                                           lConn.Source.VolId = lupOper.Id Then
                                            lConn.Source.Opn = lTempOper
                                            lConn.Source.VolId = lTempOper.Id
                                        End If
                                    Next lConn

                                    For Each lConn2 As HspfConnection In lupOper.Targets
                                        If lConn2.Target.VolName = lOper.Name And _
                                           lConn2.Target.VolId = lOper.Id Then
                                            'remove this target
                                            lupOper.Targets.Remove(lConn2)
                                            'set this as a target from the new opn
                                            lTempOper.Targets.Add(lConn2)
                                        End If
                                    Next
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub agdMet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles agdMet.Click
        DoLimits()
    End Sub

    Private Sub agdMet_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMet.MouseDownCell
        pCurrentSelectedRow = aRow
        DoLimits()
    End Sub

    Private Sub DoLimits()
        Dim lValidValues As New Collection

        lValidValues.Add("<none>")
        For Each lMetseg As HspfMetSeg In pUCI.MetSegs
            lValidValues.Add(lMetseg.Name)
        Next
        With agdMet
            .ValidValues = lValidValues
            .AllowNewValidValues = False
            .Refresh()
        End With

        If agdMet.Source.CellValue(pCurrentSelectedRow, 1).StartsWith("RCHRES") Then
            cmdApply.Enabled = True
        Else
            cmdApply.Enabled = False
        End If
    End Sub

    Private Sub frmTime_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Simulation Time.html")
        End If
    End Sub
End Class