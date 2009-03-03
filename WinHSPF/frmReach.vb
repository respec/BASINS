Imports atcUCI
Imports atcUCIForms

Public Class frmReach

    Dim pVScrollColumnOffset As Integer = 16

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
        UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").NthOper(1).FTable)  'todo: use selected row
    End Sub

    Private Sub grdReach_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdReach.CellEdited
        'todo: add limits 
        'units = myUci.GlobalBlock.emfg
        'lOpnBlk = myUci.OpnBlks("RCHRES")
        'lTable = lOpnBlk.tables("HYDR-PARM2")

        'If units = 1 Then
        '    .TextMatrix(0, 2) = "Length (mi)"
        '    .ColMin(2) = lTable.Parms("LEN").Def.Min
        '    .ColMax(2) = lTable.Parms("LEN").Def.Max
        'Else
        '    .TextMatrix(0, 2) = "Length (km)"
        '    .ColMin(2) = lTable.Parms("LEN").Def.MetricMin
        '    .ColMax(2) = lTable.Parms("LEN").Def.MetricMax
        'End If

        'If units = 1 Then
        '    .TextMatrix(0, 3) = "Delta H (ft)"
        '    .ColMin(3) = lTable.Parms("DELTH").Def.Min
        '    .ColMax(3) = lTable.Parms("DELTH").Def.Max
        'Else
        '    .TextMatrix(0, 3) = "Delta H (m)"
        '    .ColMin(3) = lTable.Parms("DELTH").Def.MetricMin
        '    .ColMax(3) = lTable.Parms("DELTH").Def.MetricMax
        'End If

        '.ColMin(4) = 0
        '.ColMax(4) = 999

        'lTable = lOpnBlk.tables("GEN-INFO")

        '.ColMin(5) = lTable.Parms("NEXITS").Def.Min
        '.ColMax(5) = lTable.Parms("NEXITS").Def.Max

        '.TextMatrix(0, 6) = "Lake Flag"
        '.ColMin(6) = lTable.Parms("LKFG").Def.Min
        '.ColMax(6) = lTable.Parms("LKFG").Def.Max

    End Sub

    Private Sub cmdOK_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'changednetwork = False
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
                'TODO -- put downoper back into puci structure
                'If lOper.DownOper("RCHRES") <> .TextMatrix(i, 4) Then
                '    'changed downstream id
                '    changednetwork = True
                '    oldDownId = lOper.DownOper("RCHRES")
                '    'check to make sure new one is ok
                '    newDownId = .TextMatrix(i, 4)
                '    tOper = lOper.OpnBlk.operfromid(newDownId)
                '    If tOper Is Nothing And newDownId <> 0 Then
                '        'invalid oper id
                '        errorfg = True
                '        myMsgBox.Show("RCHRES Operation ID " & newDownId & " is invalid.", "Reach Editor Problem", "OK")
                '        Exit For
                '    End If
                '    If Not errorfg Then
                '        'remove old connection from uci
                '        If oldDownId > 0 Then
                '            For j = 1 To myUci.Connections.Count
                '                lConn = myUci.Connections(j)
                '                If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                '                   And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                '                    myUci.Connections.Remove(j)
                '                    tempConn = lConn
                '                    Exit For
                '                End If
                '            Next j
                '        Else
                '            'no down id, used to go to 0
                '            tempConn = New HspfConnection
                '            For j = 1 To myUci.Connections.Count
                '                lConn = myUci.Connections(j)
                '                If lConn.Source.volname = "RCHRES" _
                '                   And lConn.Target.volname = "RCHRES" Then
                '                    'make like this one
                '                    tempConn.Amdstrg = lConn.Amdstrg
                '                    tempConn.MassLink = lConn.MassLink
                '                    tempConn.MFact = lConn.MFact
                '                    tempConn.Sgapstrg = lConn.Sgapstrg
                '                    tempConn.Ssystem = lConn.Ssystem
                '                    tempConn.Tran = lConn.Tran
                '                    tempConn.Typ = lConn.Typ
                '                    tempConn.Uci = lConn.Uci
                '                    tempConn.Source.Opn = lOper
                '                    tempConn.Source.volid = lOper.Id
                '                    tempConn.Source.volname = lOper.Name
                '                    Exit For
                '                End If
                '            Next j
                '        End If
                '        'remove old connection from source and target ops
                '        For j = 1 To lOper.targets.Count
                '            lConn = lOper.targets(j)
                '            If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                '               And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                '                lOper.targets.Remove(j)
                '                Exit For
                '            End If
                '        Next j
                '        If oldDownId > 0 Then
                '            OldOper = lOper.OpnBlk.operfromid(oldDownId)
                '            For j = 1 To OldOper.Sources.Count
                '                lConn = OldOper.Sources(j)
                '                If lConn.Source.volname = "RCHRES" And lConn.Source.volid = lOper.Id _
                '                   And lConn.Target.volname = "RCHRES" And lConn.Target.volid = oldDownId Then
                '                    OldOper.Sources.Remove(j)
                '                    Exit For
                '                End If
                '            Next j
                '        End If
                '        'add new connection to uci
                '        If newDownId > 0 Then
                '            newOper = lOper.OpnBlk.operfromid(newDownId)
                '            tempConn.Target.Opn = newOper
                '            tempConn.Target.volid = newDownId
                '            myUci.Connections.Add(tempConn)
                '            'add new connection to source and target ops
                '            lOper.targets.Add(tempConn)
                '            newOper.Sources.Add(tempConn)
                '        End If
                '    End If
                'End If
            Next
            'If changednetwork Then
            '    'update opn sequence if necessary
            '    switched = True
            '    changecount = 0
            '    Do Until switched = False
            '        switched = False
            '        For j = 1 To myUci.Connections.Count
            '            lConn = myUci.Connections(j)
            '            If lConn.Source.volname = "RCHRES" _
            '               And lConn.Target.volname = "RCHRES" Then
            '                sourcepos = 0
            '                targetpos = 0
            '                For k = 1 To myUci.OpnSeqBlock.Opns.Count
            '                    tOper = myUci.OpnSeqBlock.Opns(k)
            '                    If tOper.Name = lConn.Source.volname And tOper.Id = lConn.Source.volid Then
            '                        sourcepos = k
            '                    End If
            '                    If tOper.Name = lConn.Target.volname And tOper.Id = lConn.Target.volid Then
            '                        targetpos = k
            '                    End If
            '                Next k
            '                If sourcepos > 0 And targetpos > 0 And sourcepos > targetpos Then
            '                    'need to switch these 2
            '                    switched = True
            '                    tOper = myUci.OpnSeqBlock.Opns(targetpos)
            '                    myUci.OpnSeqBlock.Opns.Remove(targetpos)
            '                    myUci.OpnSeqBlock.Opns.Add(tOper, after:=sourcepos - 1)
            '                    Exit For
            '                End If
            '            End If
            '        Next j
            '        changecount = changecount + 1
            '        If changecount > 2000 Then
            '            'must have infinite loop
            '            myMsgBox.Show("This reach network is not resolving." & vbCrLf & "Check for circular connections.", "Reach Editor Problem", "OK")
            '            switched = False
            '        End If
            '    Loop
            'End If
        End With
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub
End Class
