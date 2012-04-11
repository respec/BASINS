Imports atcUtility
Imports MapWinUtility
Imports atcUCI

Public Class frmReport

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub cmdAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAll.Click
        'Dim i&, Tid&

        'If frmMain.fraSeg.Visible Then
        '    f = lblFile.Caption
        '    If f = "<none>" Then
        '        MsgBox("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
        '    Else
        '        MousePointer = vbHourglass
        '        'can write either tables or parms
        '        'suggest writing tables since less space req
        '        For i = 1 To frmMain.agdSeg.Rows
        '            frmMain.agdSeg.Selected(i, 1) = True
        '        Next i
        '        For i = 1 To frmMain.agdTab.Rows
        '            ctmp = frmMain.agdTab.TextMatrix(i, 0)
        '            If ctmp <> "NQUALS" And Mid(ctmp, 1, 5) <> "QUAL-" Then 'kludge for performance
        '                frmMain.agdTab.Selected(i, 0) = True
        '                Tid = frmMain.agdTab.ItemData(i)
        '                Call frmMain.ViewTable(Tid)
        '                PrintSave(1)
        '            End If
        '        Next i

        '        'clear main by selecting scenario again
        '        frmMain.RefreshSegment()

        'Close #u
        '        MousePointer = vbDefault
        '    End If
        'Else
        '    MsgBox("One scenario must be selected on the main form.", vbOKOnly, "Write Problem")
        'End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub cbxSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxUCI.CheckedChanged
        
    End Sub

    Private Sub frmReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPFParm.html")
        End If
    End Sub

    Private Sub cmdWrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWrite.Click
        PrintSave(1)
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        PrintSave(0)
    End Sub

    Sub PrintSave(ByVal opt%)
        'opt = 0 , printer
        'opt = 1 , file
        'Dim crit$, TabNam$, OpTypNam$, i&, OpTyp&, curop&, desc$, curseg$
        'Dim pd(23) As ParmDetail
        'Dim Min(23) As Single
        'Dim Max(23) As Single
        'Dim sum(23) As Single, rtemp!, wid&

        'f = lblFile.Caption
        'If frmMain.fraView.Visible Then
        '    If f = "<none>" And opt = 1 Then
        '        MsgBox("The Report file must be set using the Set File button.", vbOKOnly, "Write Problem")
        '    Else
        '        If opt = 1 Then
        '            u = FreeFile(0)
        '            If Len(Dir(f)) > 0 Then
        '                'already exists
        '    Open f For Append As #u
        '            Else
        '                'open output file
        '    Open f For Output As #u
        '            End If
        '        Else 'print to printer
        '            Dim fp&, tp&
        '            fp = 1
        '            tp = 1
        '            Call ShowPrinterX(Me, fp, tp, 1, 1, PD_NOSELECTION + PD_NOPAGENUMS + PD_DISABLEPRINTTOFILE)
        '            tFont = lblDummy.Font
        '            Printer.Font = tFont
        '            Printer.FontBold = False
        '        End If
        '        ltxt = frmMain.agdView.Header
        '        If Mid(ltxt, 5, 1) <> "T" Or (Mid(ltxt, 5, 1) = "T" And chkUCI = 0) Then
        '            'output header if not doing uci tables
        '            If opt = 1 Then
        '    Print #u, ltxt
        '            Else
        '                Printer.Print(ltxt)
        '            End If
        '        End If

        '        'is this by table or parameter?
        '        If Mid(ltxt, 5, 1) = "T" Then
        '            'table
        '            'get field widths
        '            For i = 1 To frmMain.agdTab.Rows
        '                If frmMain.agdTab.Selected(i, 0) Then
        '                    TabNam = frmMain.agdTab.TextMatrix(i, 0)
        '                    OpTypNam = frmMain.agdTab.TextMatrix(i, 1)
        '                    If OpTypNam = "PERLND" Then  'KLUDGE
        '                        OpTyp = 1
        '                    ElseIf OpTypNam = "IMPLND" Then
        '                        OpTyp = 2
        '                    ElseIf OpTypNam = "RCHRES" Then
        '                        OpTyp = 3
        '                    End If
        '                    Exit For
        '                End If
        '            Next i
        '            If chkUCI.Value = 0 Then
        '                ltxt = "  Op Type " & OpTypNam
        '                If opt = 1 Then
        '      Print #u, ltxt
        '      Print #u,
        '                Else
        '                    Printer.Print(ltxt)
        '                    Printer.Print()
        '                End If
        '            End If
        '            myParmDefn = myDB.OpenRecordset("ParmTableList", dbOpenDynaset)
        '            crit = "TabName = '" & TabNam & "' AND OpnTypID = " & CStr(OpTyp)
        '            With myParmDefn
        '                .FindFirst(crit)
        '                i = 0
        '                Do Until .NoMatch
        '                    i = i + 1
        '                    pd(i).Name = !Name
        '                    pd(i).StartCol = !StartCol
        '                    pd(i).Width = !Width
        '                    .FindNext(crit)
        '                Loop
        '            End With
        '            myParmDefn.Close()

        '            If chkUCI.Value = 0 Then
        '                ltxt = "Op Num    Scenario  "
        '                If frmMain.agdView.ColWidth(2) > 0 Then
        '                    ltxt = ltxt & frmMain.agdView.ColTitle(2)
        '                    k = Len(ltxt)
        '                    For i = k + 1 To 30
        '                        ltxt = ltxt & " "
        '                    Next i
        '                End If
        '                If frmMain.agdView.ColWidth(3) > 0 Then
        '                    ltxt = ltxt & frmMain.agdView.ColTitle(3)
        '                    k = Len(ltxt)
        '                    If k > 30 Then
        '                        For i = k + 1 To 40
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    Else
        '                        For i = k + 1 To 30
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    End If
        '                End If
        '                ltxt = ltxt & "Desc                "
        '                For j = 3 To frmMain.agdView.Cols
        '                    k = Len(pd(j - 2).Name)
        '                    w = pd(j - 2).Width
        '                    If w > k Then
        '                        ltxt = ltxt & pd(j - 2).Name
        '                        For n = 1 To (w - k)
        '                            ltxt = ltxt & " "
        '                        Next n
        '                    ElseIf w <= k Then
        '                        ltxt = ltxt & Mid(pd(j - 2).Name, 1, w)
        '                    End If
        '                Next j
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '                'initialize min, max, sum for each column
        '                For j = 1 To frmMain.agdView.Cols
        '                    Min(j) = 999999
        '                    Max(j) = -999999
        '                    sum(j) = 0.0#
        '                Next j
        '                'fill in values from table
        '                For j = 1 To frmMain.agdView.Rows
        '                    frmMain.agdView.Row = j
        '                    frmMain.agdView.Col = 0
        '                    ltxt = Trim(frmMain.agdView.Text)
        '                    curop = CInt(ltxt)
        '                    curseg = OpTypNam & frmMain.agdView.Text
        '                    k = Len(ltxt)
        '                    For i = k To 9
        '                        ltxt = ltxt & " "
        '                    Next i
        '                    frmMain.agdView.Col = 1
        '                    ltxt = ltxt & Left(frmMain.agdView.Text, 10)
        '                    k = Len(ltxt)
        '                    For i = k To 19
        '                        ltxt = ltxt & " "
        '                    Next i
        '                    If frmMain.agdView.ColWidth(2) > 0 Then
        '                        frmMain.agdView.Col = 2
        '                        ltxt = ltxt & frmMain.agdView.Text
        '                        k = Len(ltxt)
        '                        For i = k To 29
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    End If
        '                    If frmMain.agdView.ColWidth(3) > 0 Then
        '                        frmMain.agdView.Col = 3
        '                        ltxt = ltxt & frmMain.agdView.Text
        '                        k = Len(ltxt)
        '                        If k > 30 Then
        '                            For i = k To 39
        '                                ltxt = ltxt & " "
        '                            Next i
        '                        Else
        '                            For i = k To 29
        '                                ltxt = ltxt & " "
        '                            Next i
        '                        End If
        '                    End If
        '                    For i = 1 To frmMain.agdSeg.Rows
        '                        If frmMain.agdSeg.TextMatrix(i, 0) = curseg Then
        '                            desc = frmMain.agdSeg.TextMatrix(i, 1)  'add description
        '                            Exit For
        '                        End If
        '                    Next i
        '                    ltxt = ltxt & desc
        '                    k = Len(desc)
        '                    If k < 20 Then
        '                        For i = k To 19
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    End If
        '                    For k = 5 To frmMain.agdView.Cols
        '                        frmMain.agdView.Col = k - 1
        '                        n = Len(frmMain.agdView.Text)
        '                        ltxt = ltxt & frmMain.agdView.Text
        '                        'keep sum, min, max
        '                        rtemp = val(frmMain.agdView.Text)
        '                        sum(k) = sum(k) + rtemp
        '                        If rtemp > Max(k) Then
        '                            Max(k) = rtemp
        '                        End If
        '                        If rtemp < Min(k) Then
        '                            Min(k) = rtemp
        '                        End If
        '                        For i = n + 1 To pd(k - 4).Width
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    Next k
        '                    If opt = 1 Then
        '        Print #u, ltxt
        '                    Else
        '                        Printer.Print(ltxt)
        '                    End If
        '                Next j
        '                If opt = 1 Then
        '      Print #u,
        '                Else
        '                    Printer.Print()
        '                End If
        '                'add lines for min, max, mean
        '                ltxt = "Min                "
        '                ltxt = ltxt & "                    "
        '                If frmMain.agdView.ColWidth(2) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                If frmMain.agdView.ColWidth(3) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                For k = 5 To frmMain.agdView.Cols
        '                    frmMain.agdView.Col = k - 1
        '                    txtval = str(Min(k))
        '                    n = Len(txtval)
        '                    ltxt = ltxt & txtval
        '                    For i = n + 1 To pd(k - 4).Width
        '                        ltxt = ltxt & " "
        '                    Next i
        '                Next k
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '                'output max
        '                ltxt = "Max                "
        '                ltxt = ltxt & "                    "
        '                If frmMain.agdView.ColWidth(2) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                If frmMain.agdView.ColWidth(3) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                For k = 5 To frmMain.agdView.Cols
        '                    frmMain.agdView.Col = k - 1
        '                    txtval = str(Max(k))
        '                    n = Len(txtval)
        '                    ltxt = ltxt & txtval
        '                    For i = n + 1 To pd(k - 4).Width
        '                        ltxt = ltxt & " "
        '                    Next i
        '                Next k
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '                ltxt = "Mean               "
        '                ltxt = ltxt & "                    "
        '                If frmMain.agdView.ColWidth(2) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                If frmMain.agdView.ColWidth(3) > 0 Then
        '                    ltxt = ltxt & "          "
        '                End If
        '                For k = 5 To frmMain.agdView.Cols
        '                    frmMain.agdView.Col = k - 1
        '                    rtemp = sum(k) / frmMain.agdView.Rows
        '                    wid = pd(k - 4).Width
        '                    'txtval = NumFmted(rtemp, wid, 4)
        '                    txtval = str(rtemp)
        '                    n = Len(txtval)
        '                    ltxt = ltxt & txtval
        '                    For i = n + 1 To pd(k - 4).Width
        '                        ltxt = ltxt & " "
        '                    Next i
        '                Next k
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '                If opt = 1 Then
        '      Print #u,
        '      Print #u,
        '                Else
        '                    Printer.Print()
        '                    Printer.Print()
        '                End If
        '            Else 'want table in uci form
        '                If opt = 1 Then
        '      Print #u, TabNam
        '                Else
        '                    Printer.Print(TabNam)
        '                End If
        '                ltxt = OpTypNam & " ***"
        '                For j = 1 To 80
        '                    ltxt = ltxt & " "
        '                Next j
        '                For j = 5 To frmMain.agdView.Cols
        '                    k = pd(j - 4).StartCol
        '                    Mid(ltxt, k) = pd(j - 4).Name
        '                Next j
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '                'fill in each row
        '                For j = 1 To frmMain.agdView.Rows
        '                    frmMain.agdView.Row = j
        '                    frmMain.agdView.Col = 0
        '                    ltxt = Trim(frmMain.agdView.Text)
        '                    For i = 1 To 80
        '                        ltxt = ltxt & " "
        '                    Next i
        '                    For k = 5 To frmMain.agdView.Cols
        '                        frmMain.agdView.Col = k - 1
        '                        i = pd(k - 4).StartCol
        '                        Mid(ltxt, i) = frmMain.agdView.Text
        '                    Next k
        '                    If opt = 1 Then
        '        Print #u, ltxt
        '                    Else
        '                        Printer.Print(ltxt)
        '                    End If
        '                Next j
        '                If opt = 1 Then
        '      Print #u, "END " & TabNam
        '      Print #u,
        '      Print #u,
        '                Else
        '                    Printer.Print("END " & TabNam)
        '                    Printer.Print()
        '                    Printer.Print()
        '                End If
        '            End If
        '        Else
        '            'parameter
        '            ltxt = "Name      Value     Segment             Scenario            Desc"
        '            If frmMain.agdView.Cols > 3 Then
        '                If frmMain.agdView.ColWidth(4) > 0 Then
        '                    ltxt = ltxt & frmMain.agdView.ColTitle(4)
        '                    k = Len(ltxt)
        '                    For i = k + 1 To 60
        '                        ltxt = ltxt & " "
        '                    Next i
        '                End If
        '            End If
        '            If frmMain.agdView.Cols > 4 Then
        '                If frmMain.agdView.ColWidth(5) > 0 Then
        '                    ltxt = ltxt & frmMain.agdView.ColTitle(5)
        '                    k = Len(ltxt)
        '                    For i = k + 1 To 70
        '                        ltxt = ltxt & " "
        '                    Next i
        '                End If
        '            End If
        '            If opt = 1 Then
        '    Print #u,
        '    Print #u, ltxt
        '            Else
        '                Printer.Print()
        '                Printer.Print(ltxt)
        '            End If
        '            For j = 1 To frmMain.agdView.Rows
        '                ltxt = ""
        '                frmMain.agdView.Row = j
        '                frmMain.agdView.Col = 0
        '                ltxt = ltxt & frmMain.agdView.Text
        '                k = Len(ltxt)
        '                For i = k + 1 To 10
        '                    ltxt = ltxt & " "
        '                Next i
        '                frmMain.agdView.Col = 1
        '                ltxt = ltxt & frmMain.agdView.Text
        '                k = Len(ltxt)
        '                For i = k + 1 To 20
        '                    ltxt = ltxt & " "
        '                Next i
        '                frmMain.agdView.Col = 2
        '                ltxt = ltxt & frmMain.agdView.Text
        '                curseg = frmMain.agdView.Text
        '                k = Len(ltxt)
        '                For i = k + 1 To 40
        '                    ltxt = ltxt & " "
        '                Next i
        '                frmMain.agdView.Col = 3
        '                ltxt = ltxt & frmMain.agdView.Text  'scenario
        '                k = Len(ltxt)
        '                For i = k + 1 To 60
        '                    ltxt = ltxt & " "
        '                Next i
        '                If Len(ltxt) > 60 Then
        '                    ltxt = Mid(ltxt, 1, 60)
        '                End If
        '                For i = 1 To frmMain.agdSeg.Rows
        '                    If frmMain.agdSeg.TextMatrix(i, 0) = curseg Then
        '                        desc = frmMain.agdSeg.TextMatrix(i, 1)  'add description
        '                        Exit For
        '                    End If
        '                Next i
        '                ltxt = ltxt & desc
        '                k = Len(desc)
        '                If k < 20 Then
        '                    For i = k To 19
        '                        ltxt = ltxt & " "
        '                    Next i
        '                End If
        '                'optional fields
        '                If frmMain.agdView.Cols > 3 Then
        '                    frmMain.agdView.Col = 4
        '                    If frmMain.agdView.ColWidth(4) > 0 Then
        '                        ltxt = ltxt & frmMain.agdView.Text
        '                        k = Len(ltxt)
        '                        For i = k + 1 To 90
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    End If
        '                End If
        '                If frmMain.agdView.Cols > 4 Then
        '                    frmMain.agdView.Col = 5
        '                    If frmMain.agdView.ColWidth(5) > 0 Then
        '                        ltxt = ltxt & frmMain.agdView.Text
        '                        k = Len(ltxt)
        '                        For i = k + 1 To 100
        '                            ltxt = ltxt & " "
        '                        Next i
        '                    End If
        '                End If
        '                If opt = 1 Then
        '      Print #u, ltxt
        '                Else
        '                    Printer.Print(ltxt)
        '                End If
        '            Next j
        '            If opt = 1 Then
        '    Print #u,
        '    Print #u,
        '            Else
        '                Printer.Print()
        '                Printer.Print()
        '            End If
        '        End If
        '        If opt = 1 Then
        '  Close #u
        '        Else
        '            Printer.EndDoc()
        '        End If
        '    End If
        'Else
        '    MsgBox("Some tables or parameters must be selected on the main form.", vbOKOnly, "Write Problem")
        'End If
    End Sub

    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click
        If cdSetOut.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lblFile.Text = cdSetOut.FileName
        End If
    End Sub
End Class