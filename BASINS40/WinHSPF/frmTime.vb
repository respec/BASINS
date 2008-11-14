Imports atcData
Imports atcUCI

Public Class frmTime

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
            pfrmAddMet.Show()
        Else
            If pfrmAddMet.IsDisposed Then
                pfrmAddMet = New frmAddMet
                pfrmAddMet.Show()
            Else
                pfrmAddMet.WindowState = FormWindowState.Normal
                pfrmAddMet.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click

        If IsNothing(pfrmAddMet) Then
            pfrmAddMet = New frmAddMet
            pfrmAddMet.Show()
        Else
            If pfrmAddMet.IsDisposed Then
                pfrmAddMet = New frmAddMet
                pfrmAddMet.Show()
            Else
                pfrmAddMet.WindowState = FormWindowState.Normal
                pfrmAddMet.BringToFront()
            End If
        End If

    End Sub

    Private Sub RefreshGrid()

        Dim lHspfOperation As HspfOperation
        Dim lmetseg As HspfMetSeg
        Dim lRow, lOper As Integer

        With agdMet.Source
            .Rows = 0
            .Columns = 2
            .CellValue(0, 0) = "Met Seg ID"
            .CellValue(0, 1) = "Operation"
            For lOper = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                lHspfOperation = pUCI.OpnSeqBlock.Opns(lOper)
                If lHspfOperation.Name = "PERLND" Or lHspfOperation.Name = "IMPLND" Or lHspfOperation.Name = "RCHRES" Then
                    lmetseg = lHspfOperation.MetSeg
                    lRow = .Rows
                    If lmetseg Is Nothing Then
                        .CellValue(lRow, 0) = "<none>"
                    Else
                        .CellValue(lRow, 0) = lmetseg.Name
                    End If
                    .CellValue(lRow, 1) = lHspfOperation.Name & " " & lHspfOperation.Id
                End If
                .CellEditable(1, 0) = True
            Next lOper
        End With

        agdMet.SizeAllColumnsToContents()
        agdMet.Refresh()

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim myglobal As HspfGlobalBlk
        Dim lOpnBlk As HspfOpnBlk
        Dim loper As HspfOperation
        Dim inuse As Boolean
        Dim i, j, Id As Integer
        Dim ctemp As String

        'okay
        myglobal = pUCI.GlobalBlock
        For i = 0 To 4
            'put dates back
            myglobal.SDate(0) = txtStartYear.Text
            myglobal.SDate(1) = txtStartMonth.Text
            myglobal.SDate(2) = txtStartDay.Text
            myglobal.SDate(3) = txtStartHour.Text
            myglobal.SDate(4) = txtStartMinute.Text

            myglobal.EDate(0) = txtEndYear.Text
            myglobal.EDate(1) = txtEndMonth.Text
            myglobal.EDate(2) = txtEndDay.Text
            myglobal.EDate(3) = txtEndHour.Text
            myglobal.EDate(4) = txtEndMinute.Text

        Next i
        For i = 1 To agdMet.Source.Rows - 1
            'put met segs back
            ctemp = agdMet.Source.CellValue(i, 1)
            lOpnBlk = pUCI.OpnBlks(Mid(ctemp, 1, 6))
            Id = CInt(Mid(ctemp, 8))
            loper = lOpnBlk.OperFromID(Id)
            If agdMet.Source.CellValue(i, 0) = "<none>" Then
                loper.MetSeg = Nothing
            Else
                For j = 0 To pUCI.MetSegs.Count - 1
                    If pUCI.MetSegs(j).Name = agdMet.Source.CellValue(i, 0) Then
                        loper.MetSeg = pUCI.MetSegs(j)
                    End If
                Next j
            End If
        Next i
        'remove unused met segments
        j = 0
        Do While j <= pUCI.MetSegs.Count - 1
            inuse = False
            For i = 1 To agdMet.Source.Rows - 1
                If agdMet.Source.CellValue(i, 0) = pUCI.MetSegs(j).Name Then
                    inuse = True
                End If
            Next i
            If Not inuse Then
                'pUCI.MetSegs.Remove(loper.MetSeg)
            Else
                pUCI.MetSegs(j).Id = j
                j = j + 1
            End If
        Loop

        Me.Dispose()
    End Sub
End Class