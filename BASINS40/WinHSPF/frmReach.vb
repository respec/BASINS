Imports atcUCI
Imports atcUCIForms
Imports atcUtility
Imports MapWinUtility

Public Class frmReach

    Dim pVScrollColumnOffset As Integer = 16
    Dim pSelectedRow As Integer = 1

    Private Sub grdReach_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdReach.MouseDownCell
        pSelectedRow = aRow
    End Sub

    Private Sub grdReach_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdReach.Resize
        grdReach.SizeAllColumnsToContents(grdReach.Width - pVScrollColumnOffset, True)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Size = New Size(grdReach.Width + 40, grdReach.Height + 50)
        Me.MinimumSize = Me.Size

        ' Add any initialization after the InitializeComponent() call.
        Dim lUnits As Integer = pUCI.GlobalBlock.emfg

        With grdReach
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdReach.Source
            .FixedRows = 1
            .Columns = 6
            .CellValue(0, 0) = "ID"
            .CellValue(0, 1) = "Description"
            If lUnits = 1 Then
                .CellValue(0, 2) = "Length (mi)"
                .CellValue(0, 3) = "Delta H (ft)"
            Else
                .CellValue(0, 2) = "Length (km)"
                .CellValue(0, 3) = "Delta H (m)"
            End If
            .CellValue(0, 4) = "DownstreamID"
            .CellValue(0, 5) = "N Exits"
            .CellValue(0, 6) = "Lake Flag"

            Dim lOperBlock As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            .Rows = lOperBlock.Count

            'loop through each operation and populate the table
            For lRow As Integer = 1 To lOperBlock.Count
                Dim lOperation As HspfOperation = lOperBlock.NthOper(lRow)
                .CellValue(lRow, 0) = lOperation.Id
                Dim lTable As HspfTable = lOperation.Tables("GEN-INFO")
                .CellValue(lRow, 1) = lTable.Parms("RCHID").Value
                .CellValue(lRow, 5) = lTable.Parms("NEXITS").Value
                .CellValue(lRow, 6) = lTable.Parms("LKFG").Value
                lTable = lOperation.Tables("HYDR-PARM2")
                .CellValue(lRow, 2) = lTable.Parms("LEN").Value
                .CellValue(lRow, 3) = lTable.Parms("DELTH").Value
                .CellValue(lRow, 4) = lOperation.DownOper("RCHRES")
            Next

            For j As Integer = 1 To .Columns - 1
                For k As Integer = 1 To .Rows - 1
                    .CellEditable(k, j) = True
                Next
            Next

        End With

        grdReach.SizeAllColumnsToContents(grdReach.Width - pVScrollColumnOffset, True)
        grdReach.Refresh()

        Me.Icon = pIcon

    End Sub

    Private Sub FTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTables.Click
        If pSelectedRow > 0 Then
            UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").NthOper(1).FTable, "FTABLES", pHSPFManualName)
        Else
            UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").NthOper(pSelectedRow).FTable, "FTABLES", pHSPFManualName)
        End If
    End Sub

    Private Sub grdReach_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdReach.CellEdited
        Dim lUnitfg As Integer = pUCI.GlobalBlock.EmFg

        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999

        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
        Dim lTable As HspfTable = Nothing

        If aColumn = 2 Or aColumn = 3 Then
            lTable = lOpnBlk.Tables("HYDR-PARM2")
        ElseIf aColumn = 4 Then
            lMinValue = 0
            lMaxValue = 999
        ElseIf aColumn = 5 Or aColumn = 6 Then
            lTable = lOpnBlk.Tables("GEN-INFO")
        End If

        Dim lParmName As String = ""
        If aColumn = 2 Then
            lParmName = "LEN"
        ElseIf aColumn = 3 Then
            lParmName = "DELTH"
        ElseIf aColumn = 5 Then
            lParmName = "NEXITS"
        ElseIf aColumn = 6 Then
            lParmName = "LKFG"
        End If

        If Not lTable Is Nothing Then
            Dim lParm As HspfParm = lTable.Parms(lParmName)

            If lParm.Def.Typ = 1 Or lParm.Def.Typ = 2 Then
                'this is a numeric field
                If lUnitfg = 1 Then 'english
                    lMaxValue = lParm.Def.Max
                    lMinValue = lParm.Def.Min
                ElseIf lUnitfg = 2 Then 'metric
                    lMaxValue = lParm.Def.MetricMax
                    lMinValue = lParm.Def.MetricMin
                End If
            End If
        End If

        If lMaxValue <> -999 Or lMinValue <> -999 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
            If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
               ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If
            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lChangedNetwork As Boolean = False
        Dim lErrorFg As Boolean = False

        With grdReach.Source
            Dim lOperBlock As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            For lRow As Integer = 1 To lOperBlock.Count
                Dim lOperation As HspfOperation = lOperBlock.NthOper(lRow)
                Dim lTable As HspfTable = lOperation.Tables("GEN-INFO")
                lTable.Parms("RCHID").Value = .CellValue(lRow, 1)
                lTable.Parms("NEXITS").Value = .CellValue(lRow, 5)
                lTable.Parms("LKFG").Value = .CellValue(lRow, 6)
                lTable = lOperation.Tables("HYDR-PARM2")
                lTable.Parms("LEN").Value = .CellValue(lRow, 2)
                lTable.Parms("DELTH").Value = .CellValue(lRow, 3)

                If lOperation.DownOper("RCHRES") <> .CellValue(lRow, 4) Then
                    'changed downstream id
                    lChangedNetwork = True
                    Dim lOldDownId As Integer = lOperation.DownOper("RCHRES")
                    'check to make sure new one is ok
                    Dim lNewDownId As Integer = .CellValue(lRow, 4)
                    Dim tOper As HspfOperation = lOperation.OpnBlk.OperFromID(lNewDownId)
                    If tOper Is Nothing And lNewDownId <> 0 Then
                        'invalid oper id
                        lErrorFg = True
                        Logger.Msg("RCHRES Operation ID " & lNewDownId & " is invalid.", "Reach Editor Problem")
                        Exit For
                    End If
                    If Not lErrorFg Then
                        'remove old connection from uci
                        Dim lTempConn As New HspfConnection
                        If lOldDownId > 0 Then
                            For Each lConn As HspfConnection In pUCI.Connections
                                If lConn.Source.VolName = "RCHRES" And lConn.Source.VolId = lOperation.Id _
                                   And lConn.Target.VolName = "RCHRES" And lConn.Target.VolId = lOldDownId Then
                                    lTempConn = lConn
                                    pUCI.Connections.Remove(lConn)
                                    Exit For
                                End If
                            Next
                        Else
                            'no down id, used to go to 0
                            For Each lConn As HspfConnection In pUCI.Connections
                                If lConn.Source.VolName = "RCHRES" _
                                   And lConn.Target.VolName = "RCHRES" Then
                                    'make like this one
                                    lTempConn.Amdstrg = lConn.Amdstrg
                                    lTempConn.MassLink = lConn.MassLink
                                    lTempConn.MFact = lConn.MFact
                                    lTempConn.Sgapstrg = lConn.Sgapstrg
                                    lTempConn.Ssystem = lConn.Ssystem
                                    lTempConn.Tran = lConn.Tran
                                    lTempConn.Typ = lConn.Typ
                                    lTempConn.Uci = lConn.Uci
                                    lTempConn.Source.Opn = lOperation
                                    lTempConn.Source.VolId = lOperation.Id
                                    lTempConn.Source.VolName = lOperation.Name
                                    Exit For
                                End If
                            Next
                        End If
                        'remove old connection from source and target ops
                        For Each lConn As HspfConnection In lOperation.Targets
                            If lConn.Source.VolName = "RCHRES" And lConn.Source.VolId = lOperation.Id _
                               And lConn.Target.VolName = "RCHRES" And lConn.Target.VolId = lOldDownId Then
                                lOperation.Targets.Remove(lConn)
                                Exit For
                            End If
                        Next
                        If lOldDownId > 0 Then
                            Dim lOldOper As HspfOperation = lOperation.OpnBlk.OperFromID(lOldDownId)
                            For Each lConn As HspfConnection In lOldOper.Sources
                                If lConn.Source.VolName = "RCHRES" And lConn.Source.VolId = lOldOper.Id _
                                   And lConn.Target.VolName = "RCHRES" And lConn.Target.VolId = lOldDownId Then
                                    lOldOper.Sources.Remove(lConn)
                                    Exit For
                                End If
                            Next
                        End If
                        'add new connection to uci
                        If lNewDownId > 0 Then
                            Dim lNewOper As HspfOperation = lOperation.OpnBlk.OperFromID(lNewDownId)
                            lTempConn.Target.Opn = lNewOper
                            lTempConn.Target.VolId = lNewDownId
                            pUCI.Connections.Add(lTempConn)
                            'add new connection to source and target ops
                            lOperation.Targets.Add(lTempConn)
                            lNewOper.Sources.Add(lTempConn)
                        End If
                    End If
                End If
            Next
            If lChangedNetwork Then
                'update opn sequence if necessary
                Dim lSwitched As Boolean = True
                Dim lChangeCount = 0
                Do Until lSwitched = False
                    lSwitched = False
                    For Each lConn As HspfConnection In pUCI.Connections
                        If lConn.Source.VolName = "RCHRES" _
                           And lConn.Target.VolName = "RCHRES" Then
                            Dim lSourcePos As Integer = 0
                            Dim lTargetPos As Integer = 0
                            For lIndex As Integer = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                                Dim lOper As HspfOperation = pUCI.OpnSeqBlock.Opns(lIndex)
                                If lOper.Name = lConn.Source.VolName And lOper.Id = lConn.Source.VolId Then
                                    lSourcePos = lIndex
                                End If
                                If lOper.Name = lConn.Target.VolName And lOper.Id = lConn.Target.VolId Then
                                    lTargetPos = lIndex
                                End If
                            Next lIndex
                            If lSourcePos > 0 And lTargetPos > 0 And lSourcePos > lTargetPos Then
                                'need to switch these 2
                                lSwitched = True
                                Dim lOper As HspfOperation = pUCI.OpnSeqBlock.Opns(lTargetPos)
                                pUCI.OpnSeqBlock.Opns.Remove(lOper)
                                pUCI.OpnSeqBlock.Opns.Insert(lSourcePos - 1, lOper)
                                Exit For
                            End If
                        End If
                    Next
                    lChangeCount = lChangeCount + 1
                    If lChangeCount > 2000 Then
                        'must have infinite loop
                        Logger.Msg("This reach network is not resolving." & vbCrLf & "Check for circular connections.", "Reach Editor Problem")
                        lSwitched = False
                    End If
                Loop
            End If
        End With
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub frmReach_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Reach Editor.html")
        End If
    End Sub
End Class
