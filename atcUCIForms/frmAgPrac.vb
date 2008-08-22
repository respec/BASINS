Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Collections.ObjectModel

'***************IMPORTANT NOTES:***************
'Check definition of pCtl 
'Check starter path - hardcoded for now!!!
'**********************************************


Public Class frmAgPrac

    Dim pUci As HspfUci
    'in line below, may need to change to ctlEditSpecialAction
    Dim pCtl As ctlEditSpecialAction

    Dim cPracIDs As New Collection
    Dim cPracRecs As New Collection

    Private Sub Form_Load()

        Dim vOper As Object
        Dim lOper As HspfOperation
        Dim i, pcount As Integer
        Dim ctmp, tname, lstr, ilen, tstr As String
        Dim cend As Boolean

        txtNA.Visible = False
        txtNA.Enabled = False
        GroupLayers.Visible = True
        GroupPar.Visible = True
        GroupProps.Visible = True
        Height = 555

        comboRep.Items.Clear()
        comboRep.Items.Add("None")
        comboRep.Items.Add("YR")
        comboRep.Items.Add("MO")
        comboRep.Items.Add("DY")
        comboRep.Items.Add("HR")
        comboRep.Items.Add("MI")
        'read database
        i = FreeFile()
        'On Error GoTo ErrHandler
        tname = "C:\Basins\models\HSPF\bin\starter" & "\" & "agpractice.txt"
        FileOpen(i, tname, OpenMode.Input)
        pcount = 0

        Do Until EOF(i)
            lstr = LineInput(i)
            ilen = Len(lstr)
            If ilen > 6 Then
                If Microsoft.VisualBasic.Left(lstr, 7) = "PRACTIC" Then
                    'found start of a practice
                    ctmp = StrRetRem(lstr)
                    lstPrac.Items.Add(lstr)
                    pcount = pcount + 1
                    cend = False
                    Do While Not cend
                        lstr = LineInput(i)
                        tstr = Trim(lstr)
                        ilen = Len(tstr)
                        If Microsoft.VisualBasic.Left(tstr, ilen) = "END PRACTICE" Then
                            'found end of practice
                            cend = True
                        Else
                            cPracIDs.Add(pcount)
                            cPracRecs.Add(lstr)
                        End If
                    Loop
                End If
            End If
        Loop
        FileClose()
        'formerly goto FillLists
        For Each vOper In pUci.OpnSeqBlock.Opns
            lOper = vOper
            If lOper.Name = "PERLND" Then
                lstSeg.Items.Add(lOper.Name & " " & lOper.Id & " (" & lOper.Description & ")")
            End If
        Next

        atxYear.HardMax = pUci.GlobalBlock.EDate(0)
        atxYear.HardMin = pUci.GlobalBlock.SDate(0)

        lstPrac.SetSelected(0, True)
        lstSeg.SetSelected(0, True)
        ShowDetails()

        'ErrHandler:
        '        If Err.Number = 53 Then
        '            MsgBox("File " & tname & " not found.", vbOKOnly, "Read Ag Practices Problem")
        '        Else
        '            MsgBox(Err.Description & vbCrLf & vbCrLf & lstr, _
        '              vbOKOnly, "Read Ag Practices Problem")
        'End If

    End Sub

    Private Sub Form_Unload(ByVal Cancel As Integer)
        Do Until cPracIDs.Count = 0
            cPracIDs.Remove(1)
        Loop
        Do Until cPracRecs.Count = 0
            cPracRecs.Remove(1)
        Loop
    End Sub

    Private Sub frmAgPrac_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Form_Load()
    End Sub

    Friend Sub Init(ByVal aUci As HspfUci, ByVal aCtl As ctlEditSpecialAction)
        pUci = aUci
        pCtl = aCtl
        Me.Icon = aCtl.ParentForm.Icon
    End Sub



    Private Sub ShowDetails()
        Dim i&, cbuff$, itype&, ctmp$
        Dim rDate(5) As String
        Dim SDate(5) As String
        Dim ActionCount As Integer
        Dim LayerFlag As Boolean
        Dim Id As Long


        ParGrid.Source = New atcControls.atcGridSource
        With ParGrid
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.Columns = 2
        End With

        Id = lstPrac.SelectedIndex + 1

        LayerFlag = False
        For i = 1 To cPracIDs.Count
            If cPracIDs(i) = Id Then
                If Mid(cPracRecs(i), 3, 6) = "UVNAME" Then
                    LayerFlag = True
                End If
            End If
        Next i

        If GroupProps.Visible = False Then
            GroupProps.Visible = True
        End If
        If LayerFlag Then
            atxSurface.Visible = True
            atxUpper.Visible = True
            txtSurface.Visible = True
            txtUpper.Visible = True
            txtNA.Visible = False
        Else
            atxSurface.Visible = False
            atxUpper.Visible = False
            txtSurface.Visible = False
            txtUpper.Visible = False
            txtNA.Visible = True

        End If

        ActionCount = 0

        For i = 1 To cPracIDs.Count
            If cPracIDs(i) = Id Then
                'identify the type of record
                cbuff = cPracRecs(i)
                If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
                    itype = 6 ' hComment
                ElseIf Microsoft.VisualBasic.Left(Trim(cbuff), 3) = "IF " Or _
                       Microsoft.VisualBasic.Left(Trim(cbuff), 4) = "ELSE" Or _
                       Microsoft.VisualBasic.Left(Trim(cbuff), 6) = "END IF" Then
                    itype = 5 'hCondition
                ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
                    itype = 2 'hDistribute
                ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
                    itype = 3 'hUserDefineName
                ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
                    itype = 4 'hUserDefineQuan
                Else
                    itype = 1 'hAction
                End If
                'extract info as needed
                If itype = 3 Then 'hUserDefineName
                    'assume that the uvname will have surface/upper distrib
                    atxSurface.Value = Mid(cbuff, 37, 5)
                    atxUpper.Value = Mid(cbuff, 67, 5)
                ElseIf itype = 1 Then
                    ActionCount = ActionCount + 1
                    'check if repeating
                    ctmp = Mid(cbuff, 72, 2)
                    If ctmp = "YR" Then
                        comboRep.SelectedIndex = 1
                    ElseIf ctmp = "MO" Then
                        comboRep.SelectedIndex = 2
                    ElseIf ctmp = "DY" Then
                        comboRep.SelectedIndex = 3
                    ElseIf ctmp = "HR" Then
                        comboRep.SelectedIndex = 4
                    ElseIf ctmp = "MI" Then
                        comboRep.SelectedIndex = 5
                    Else
                        comboRep.SelectedIndex = 0
                    End If
                    'check for dated action
                    If Len(Trim(Mid(cbuff, 21, 4))) > 0 Then
                        rDate(0) = CInt(Mid(cbuff, 21, 4))
                        If Mid(cbuff, 25, 3) = "   " Then
                            rDate(1) = 0
                        Else
                            rDate(1) = CInt(Mid(cbuff, 25, 3))
                        End If
                        If Mid(cbuff, 28, 3) = "   " Then
                            rDate(2) = 0
                        Else
                            rDate(2) = CInt(Mid(cbuff, 28, 3))
                        End If
                        If Mid(cbuff, 31, 3) = "   " Then
                            rDate(3) = 0
                        Else
                            rDate(3) = CInt(Mid(cbuff, 31, 3))
                        End If
                        If Mid(cbuff, 34, 3) = "   " Then
                            rDate(4) = 0
                        Else
                            rDate(4) = CInt(Mid(cbuff, 34, 3))
                        End If
                        SDate(0) = pUci.GlobalBlock.SDate(0)
                        SDate(1) = pUci.GlobalBlock.SDate(1)
                        SDate(2) = pUci.GlobalBlock.SDate(2)
                        SDate(3) = pUci.GlobalBlock.SDate(3)
                        SDate(4) = pUci.GlobalBlock.SDate(4)
                        If Mid(cbuff, 72, 2) = "DY" Then
                            'set date to start of run
                            atxYear.Value = SDate(0)
                            atxMo.Value = SDate(1)
                            atxDay.Value = SDate(2)
                            atxHr.Value = SDate(3)
                            atxMin.Value = SDate(4)
                        Else
                            'yearly repeating or other
                            rDate(0) = SDate(0)
                            'If Date2J(rDate) < Date2J(SDate) Then
                            '    'change to following year
                            '    rDate(0) = rDate(0) + 1
                            'End If
                            atxYear.Value = rDate(0)
                            atxMo.Value = rDate(1)
                            atxDay.Value = rDate(2)
                            atxHr.Value = rDate(3)
                            atxMin.Value = rDate(4)
                        End If
                    End If
                    'fill names and values
                    If LayerFlag Then
                        ParGrid.Source.CellValue(ActionCount - 1, 0) = Mid(cbuff, 43, 3)
                    Else
                        ParGrid.Source.CellValue(ActionCount - 1, 0) = Mid(cbuff, 43, 6)
                    End If
                    ParGrid.Source.CellValue(ActionCount - 1, 1) = Mid(cbuff, 61, 10)
                    If Len(Mid(cbuff, 16, 2)) > 0 Then
                        'defer by default
                        chkDelay.Checked = True
                    End If
                End If
            End If
        Next i

        For lRow As Integer = 0 To ParGrid.Source.Rows - 1
            ParGrid.Source.CellColor(lRow, 0) = SystemColors.ControlLight
        Next

        For lCol As Integer = 0 To ParGrid.Source.Rows - 1
            ParGrid.Source.CellEditable(lCol, 1) = True
        Next

        ParGrid.SizeAllColumnsToContents()
        ParGrid.Refresh()

    End Sub

    Private Sub lstPrac_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPrac.SelectedIndexChanged
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then
            ShowDetails()
        End If
    End Sub

    Private Sub lstSeg_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSeg.SelectedIndexChanged
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then
            ShowDetails()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim Id&, i&, perlndid&, cbuff$, ctmp$, itype&, uvq$
        Dim nrepeat&, RecordCount&
        Dim LayerFlag As Boolean, j&, Sub1$, Sub2$, DistribID&
        Dim FirstAction As Boolean, WroteAction As Boolean
        Dim rDate(5), SDate(5), EDate(5) As Integer '<<<< This Dim statement was added (changed from Long to String)

        Dim strid As String

        'if one practice and one land use selected, apply
        If lstPrac.SelectedItems.Count = 1 And lstSeg.SelectedItems.Count = 1 Then

            ctmp = Mid(lstSeg.Items.Item(lstSeg.SelectedIndex), 8)
            perlndid = StrRetRem(ctmp)

            If chkDelay.Checked = True Then
                'deferring, add uvquan and conditional
                uvq = pCtl.UVQuanInUse("PERLND", perlndid)
                If Len(uvq) = 0 Then
                    'find an unused uvquan name
                    uvq = pCtl.NextUVQuanName("prec")
                    If Len(uvq) = 5 Then
                        uvq = uvq & " "
                    End If
                    'write this one to spec act records
                    strid = RSet(CStr(perlndid), 3)
                    cbuff = "  UVQUAN " & uvq & " PERLND " & strid & " PREC             3                 DY  1 SUM"
                    pCtl.AddToBeginning(cbuff, 4)
                End If
            End If

            LayerFlag = False
            FirstAction = True
            WroteAction = False
            DistribID = 0
            Id = lstPrac.Items.Count '<<<<<<<<<<<<<<<<<<<<<<<<<<<<+ 1
            RecordCount = 0
            For i = 1 To cPracIDs.Count
                If cPracIDs(i) = Id Then
                    'identify the type of record
                    cbuff = cPracRecs(i)
                    If Len(cbuff) = 0 Or InStr(cbuff, "***") > 0 Then
                        itype = 6
                    ElseIf Microsoft.VisualBasic.Left(Trim(cbuff), 3) = "IF " Or _
                           Microsoft.VisualBasic.Left(Trim(cbuff), 4) = "ELSE" Or _
                           Microsoft.VisualBasic.Left(Trim(cbuff), 6) = "END IF" Then
                        itype = 5 'hcondition
                    ElseIf Mid(cbuff, 3, 6) = "DISTRB" Then
                        itype = 2 'hDistribute
                        DistribID = pCtl.NextDistribNumber
                    ElseIf Mid(cbuff, 3, 6) = "UVNAME" Then
                        itype = 3 'hUserDefineName
                    ElseIf Mid(cbuff, 3, 6) = "UVQUAN" Then
                        itype = 4 'hUserDefineQuan
                    Else
                        itype = 1 'hAction
                    End If
                    'modify this record as appropriate
                    If itype = 4 Then
                        'set operation number
                        strid = RSet(CStr(perlndid), 3)
                        cbuff = Mid(cbuff, 1, 23) & strid & Mid(cbuff, 27)
                    ElseIf itype = 3 Then
                        'create var name from upper/surface split
                        LayerFlag = True
                        strid = CStr(atxSurface.Value)
                        j = InStr(1, strid, ".")
                        Sub1 = " "
                        If j > 0 And Len(strid) >= j + 1 Then
                            Sub1 = Mid(strid, j + 1, 1)
                        End If
                        strid = CStr(atxUpper.Value)
                        j = InStr(1, strid, ".")
                        Sub2 = " "
                        If j > 0 And Len(strid) >= j + 1 Then
                            Sub2 = Mid(strid, j + 1, 1)
                        End If
                        cbuff = Mid(cbuff, 1, 13) & Sub1 & Sub2 & Mid(cbuff, 16)
                        strid = RSet(CStr(atxSurface.Value), 5)
                        cbuff = Mid(cbuff, 1, 36) & strid & Mid(cbuff, 42)
                        strid = RSet(CStr(atxUpper.Value), 5)
                        cbuff = Mid(cbuff, 1, 66) & strid & Mid(cbuff, 72)
                    ElseIf itype = 2 Then
                        'change distrib id
                        strid = RSet(CStr(DistribID), 3)
                        cbuff = Mid(cbuff, 1, 8) & strid & Mid(cbuff, 12)
                    ElseIf itype = 1 Then
                        RecordCount = RecordCount + 1

                        If FirstAction And chkDelay.Checked = True Then
                            'need to open conditional
                            pCtl.AddToEnd("IF (" & Trim(uvq) & " < 0.11) THEN", 5)
                            FirstAction = False
                        End If

                        'set operation number
                        strid = RSet(CStr(perlndid), 3)
                        cbuff = Mid(cbuff, 1, 8) & strid & Mid(cbuff, 12)
                        'check if defering
                        If chkDelay.Checked = True Then
                            cbuff = Mid(cbuff, 1, 15) & "DY  1" & Mid(cbuff, 21)
                        Else
                            cbuff = Mid(cbuff, 1, 15) & "     " & Mid(cbuff, 21)
                        End If
                        'start year
                        RSet(CStr(atxYear.Value), 4)
                        cbuff = Mid(cbuff, 1, 20) & strid & Mid(cbuff, 25)
                        'start mon
                        strid = RSet(CStr(atxMo.Value), 3)
                        If atxMo.Value <> 0 Then
                            cbuff = Mid(cbuff, 1, 24) & strid & Mid(cbuff, 28)
                        Else
                            cbuff = Mid(cbuff, 1, 24) & "   " & Mid(cbuff, 28)
                        End If
                        'start day
                        strid = RSet(CStr(atxDay.Value), 3)
                        If atxDay.Value <> 0 Then
                            cbuff = Mid(cbuff, 1, 27) & strid & Mid(cbuff, 31)
                        Else
                            cbuff = Mid(cbuff, 1, 27) & "   " & Mid(cbuff, 31)
                        End If
                        'start hr
                        strid = RSet(CStr(atxHr.Value), 3)
                        If atxMin.Value <> 0 Then
                            cbuff = Mid(cbuff, 1, 30) & strid & Mid(cbuff, 34)
                        Else
                            cbuff = Mid(cbuff, 1, 30) & "   " & Mid(cbuff, 34)
                        End If
                        'start min
                        strid = RSet(CStr(atxMin.Value), 3)
                        If atxMin.Value <> 0 Then
                            cbuff = Mid(cbuff, 1, 33) & strid & Mid(cbuff, 37)
                        Else
                            cbuff = Mid(cbuff, 1, 33) & "   " & Mid(cbuff, 37)
                        End If
                        'distrib id
                        If IsNumeric(Mid(cbuff, 39, 3)) And DistribID > 0 Then
                            RSet(CStr(DistribID), 3)
                            cbuff = Mid(cbuff, 1, 35) & strid & Mid(cbuff, 39)
                        End If
                        If LayerFlag Then
                            'update uvname
                            cbuff = Mid(cbuff, 1, 45) & Sub1 & Sub2 & Mid(cbuff, 48)
                        End If
                        'value
                        strid = RSet(CStr(ParGrid.Source.CellValue(RecordCount - 1, 1)), 10)
                        cbuff = Mid(cbuff, 1, 60) & strid & Mid(cbuff, 71)
                        'check if repeating
                        If comboRep.Items.Item(comboRep.SelectedIndex) <> "None" Then '<<<<<<<DONT TRUST THIS!!!!!
                            'repeating
                            If Len(Mid(cbuff, 75, 3)) = 0 Then
                                cbuff = Mid(cbuff, 1, 71) & comboRep.Items.Item(comboRep.SelectedIndex) & "   1" & Mid(cbuff, 78)
                            Else
                                cbuff = Mid(cbuff, 1, 71) & comboRep.Items.Item(comboRep.SelectedIndex) & Mid(cbuff, 74)
                            End If
                        Else
                            cbuff = Mid(cbuff, 1, 71) & "      " & Mid(cbuff, 77)
                        End If
                        If Mid(cbuff, 72, 2) = "YR" Then
                            'yearly repeating
                            'check number of repeats
                            EDate(0) = pUci.GlobalBlock.EDate(0)
                            EDate(1) = pUci.GlobalBlock.EDate(1)
                            EDate(2) = pUci.GlobalBlock.EDate(2)
                            EDate(3) = pUci.GlobalBlock.EDate(3)
                            EDate(4) = pUci.GlobalBlock.EDate(4)
                            SDate(0) = atxYear.Value
                            SDate(1) = atxMo.Value
                            SDate(2) = atxDay.Value
                            SDate(3) = atxHr.Value
                            SDate(4) = atxMin.Value
                            nrepeat = Int((Date2J(EDate) - Date2J(SDate)) / 365)
                            strid = RSet(CStr(nrepeat), 3)
                            cbuff = Mid(cbuff, 1, 77) & strid
                        End If
                        WroteAction = True
                    End If
                    'add to spec act list
                    If itype = 3 Then
                        'check to make sure this name hasn't already been used
                        ctmp = Mid(cbuff, 11, 5)
                        If Not pCtl.UVNameInUse(ctmp) Then
                            pCtl.AddToEnd(cbuff, itype)
                        End If
                    Else
                        pCtl.AddToEnd(cbuff, itype)
                    End If
                End If
            Next i
            If WroteAction And chkDelay.Checked = 1 Then
                'need to close conditional
                pCtl.AddToEnd("END IF", 5)
            End If
        Else
            Logger.Msg("One Practice and One Land Segment must be selected.", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Add Ag Practice Problem")
        End If
    End Sub

End Class