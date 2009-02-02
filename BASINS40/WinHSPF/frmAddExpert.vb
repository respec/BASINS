Imports atcUCI
Imports atcData
Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel


Public Class frmAddExpert

    Dim pCheckedRadioIndex As Integer
    Dim pListBox1DataItems As New Collection

    Sub New(ByVal aCheckedRadioIndex As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        pCheckedRadioIndex = aCheckedRadioIndex

        Select Case pCheckedRadioIndex
            Case 1 'Hydrology Calibration
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDescription.Visible = False
                optH.Visible = False
                optD.Visible = False
                Label2.Visible = False
            Case 2 'Flow
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDescription.Visible = False
                Label2.Visible = False
            Case 3 'AQUATOX Linkage
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDescription.Visible = False
                Label2.Visible = False
            Case 5 'Copy All
                Label1.Text = "From Operation"
                Label2.Text = "To Operation"
                ListBox2.Items.Add("<select origin operation>")
                Me.Text = "WinHSPF - Copy Output"
                txtDescription.Visible = False
        End Select

        Dim lHspfOperation As HspfOperation
        Dim i, lIndex2 As Integer
        Dim S As String
        Dim lFoundFlag As Boolean

        pListBox1DataItems.Clear()

        If pCheckedRadioIndex = 4 Then 'other types
            ListBox2.Enabled = False
            Label2.Enabled = False
            For i = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                lHspfOperation = pUCI.OpnSeqBlock.Opn(i)
                If lHspfOperation.Name = "RCHRES" Or lHspfOperation.Name = "PERLND" Or lHspfOperation.Name = "IMPLND" Or lHspfOperation.Name = "BMPRAC" Then
                    S = lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")"
                    ListBox1.Items.Add(S)
                    pListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next i
        ElseIf pCheckedRadioIndex = 1 Or pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Then 'calib or flow or aquatox
            ListBox2.Enabled = False
            For i = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                lHspfOperation = pUCI.OpnSeqBlock.Opn(i)
                If lHspfOperation.Name = "RCHRES" Then
                    S = lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")"
                    ListBox1.Items.Add(S)
                    pListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next i
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            ListBox1.Items.Clear()
            ListBox2.Enabled = False

            For i = 1 To pfrmOutput.agdOutput.Source.Rows - 1

                lFoundFlag = False
                For lIndex2 = 0 To ListBox1.Items.Count - 1
                    If pfrmOutput.agdOutput.Source.CellValue(i, 0) = ListBox1.Items.Item(lIndex2) Then
                        lFoundFlag = True
                        Exit For
                    End If
                Next

                If Not lFoundFlag Then
                    ListBox1.Items.Add(pfrmOutput.agdOutput.Source.CellValue(i, 0))
                End If

            Next

        End If

        If pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Or pCheckedRadioIndex = 5 Then
            'give option of output timeunits if hourly run, otherwise always daily
            If pUCI.OpnSeqBlock.Delt <= 60 Then
                optH.Enabled = True
                optD.Enabled = True
            End If
        End If

        txtLoc.Text = "<none>"

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim ostr$(28), copyid&, ContribArea!
        Dim lTable As HspfTable, WDMId&, newdsn&, spacepos&, parenpos&
        Dim opname$, tmem$, group$, mem$, colonpos&, crem$, commapos&, sub1&, sub2&
        Dim found As Boolean, gqualfg&(3), tempmem$
        Dim fromOper As HspfOperation
        Dim toOper As HspfOperation
        Dim lOper As HspfOperation
        Dim member(28), tgroup(28), ctemp As String
        Dim vConn As Object, vToConn As Object
        Dim lConn As HspfConnection, lToConn As HspfConnection
        Dim fromcalib As Boolean, outtu As Long
        Dim Id, lTrans As String
        Dim msub1(28), adsn(28) As Integer
        Dim lDialogResult As Windows.Forms.DialogResult = Nothing

        If optH.Checked Then
            outtu = 3  'hourly
        End If

        If optD.Checked Then
            outtu = 4  'daily
        End If

        If pCheckedRadioIndex = 1 Then 'add calib location
            If ListBox1.SelectedIndex > -1 Then
                Id = pListBox1DataItems.Item(ListBox1.SelectedIndex + 1)
                If Not pfrmOutput.IsCalibLocation("RCHRES", (Id)) Then
                    Me.Cursor = Cursors.WaitCursor
                    'add data sets
                    pUCI.AddExpertDsns(Id, Label1.Text, atxBase.Text, adsn, ostr)
                    'add to copy block
                    copyid = 1
                    pUCI.AddOperation("COPY", copyid)
                    pUCI.AddTable("COPY", copyid, "TIMESERIES")
                    lTable = pUCI.OpnBlks("COPY").OperFromID(copyid).Tables("TIMESERIES")
                    lTable.Parms("NMN").Value = 7
                    'add to opn seq block
                    pUCI.OpnSeqBlock.Add(pUCI.OpnBlks("COPY").OperFromID(copyid))
                    'add to ext targets block
                    ContribArea = pUCI.UpstreamArea(pUCI.OpnBlks.Item("RCHRES").OperFromID(Id))
                    pUCI.AddExpertExtTargets(Id, copyid, ContribArea, adsn, ostr)
                    'add mass-link and schematic copy records
                    pUCI.AddExpertSchematic(Id, copyid)
                    pUCI.Edited = True
                    Me.Cursor = Cursors.Arrow
                End If
            End If
        ElseIf pCheckedRadioIndex = 2 Then 'add flow location
            If ListBox1.SelectedIndex > -1 Then
                Id = pListBox1DataItems.Item(ListBox1.SelectedIndex + 1)
                pUCI.AddOutputWDMDataSetExt(Label1.Text, "FLOW", atxBase.Text, WDMId, outtu, "", newdsn)
                pUCI.AddExtTarget("RCHRES", Id, "HYDR", "RO", 1, 1, 1.0#, "AVER", _
                       "WDM" & CStr(WDMId), newdsn, "FLOW", 1, "ENGL", "AGGR", "REPL")
                pUCI.Edited = True
            End If
        ElseIf pCheckedRadioIndex = 4 Then 'add this other output
            If ListBox1.SelectedIndex > -1 And ListBox2.SelectedIndex > -1 Then
                spacepos = InStr(1, ListBox1.Items(ListBox1.SelectedIndex), " ")
                parenpos = InStr(1, ListBox1.Items(ListBox1.SelectedIndex), "(")
                If spacepos > 0 And parenpos > 0 Then
                    'parse operation name and id
                    Id = CInt(Mid(ListBox1.Items(ListBox1.SelectedIndex), spacepos, parenpos - spacepos))
                    opname = Mid(ListBox1.Items(ListBox1.SelectedIndex), 1, spacepos - 1)
                    If Id > 0 And Len(opname) > 0 Then
                        'parse member name
                        tmem = ListBox2.Items(ListBox2.SelectedIndex)
                        parenpos = InStr(1, tmem, "(")
                        colonpos = InStr(1, tmem, ":")
                        If parenpos > 0 Then 'has subscripts
                            crem = Mid(tmem, parenpos)
                            commapos = InStr(1, crem, ",")
                            If commapos > 0 Then
                                sub1 = CInt(Mid(crem, 2, commapos - 2))
                                sub2 = CInt(Mid(crem, commapos + 1, Len(crem) - commapos - 1))
                            Else
                                sub1 = CInt(Mid(crem, 2, Len(crem) - 1))
                                sub2 = 1
                            End If
                            mem = Mid(tmem, colonpos + 1, parenpos - colonpos - 1)
                            group = Mid(tmem, 1, colonpos - 1)
                            tempmem = mem & CStr(sub1)

                            '.net conversion note: This is implemented with the assumption that pfrmAddExpert is opened with pfrmOutput as its parent.
                            If pfrmOutput.TSMaxSubscript(2, group, mem) > 1 Then
                                tempmem = tempmem & CStr(sub2)
                            End If
                        Else
                            sub1 = 1
                            sub2 = 1
                            mem = Mid(tmem, colonpos + 1)
                            tempmem = mem
                        End If
                        group = Mid(tmem, 1, colonpos - 1)
                        'now add the data set
                        If outtu = 3 Then
                            'hourly, use blank transform
                            lTrans = "    "
                        Else
                            lTrans = "AVER"
                        End If
                        pUCI.AddOutputWDMDataSetExt(Label1.Text, tempmem, atxBase.Text, WDMId, outtu, "", newdsn)
                        pUCI.AddExtTarget(opname, Id, group, mem, sub1, sub2, 1.0#, lTrans, _
                             "WDM" & CStr(WDMId), newdsn, tempmem, 1, "ENGL", "AGGR", "REPL")
                        pUCI.Edited = True
                    End If
                End If
            Else
                lDialogResult = Logger.Message("An operation and group/member must be selected.", "Add Output Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            If ListBox2.Items.Count > 0 And ListBox2.SelectedIndex > -1 Then
                'something selected in both lists
                ctemp = ListBox1.Items(ListBox1.SelectedIndex)
                spacepos = InStr(1, ctemp, " ")
                opname = Mid(ctemp, 1, spacepos - 1)
                Id = CInt(Mid(ctemp, spacepos + 1))
                fromOper = pUCI.OpnBlks(opname).OperFromID(Id)
                ctemp = ListBox2.Items(ListBox1.SelectedIndex)
                spacepos = InStr(1, ctemp, " ")
                opname = Mid(ctemp, 1, spacepos - 1)
                Id = CInt(Mid(ctemp, spacepos + 1))
                toOper = pUCI.OpnBlks(opname).OperFromID(Id)
                If fromOper.Name = "RCHRES" Then
                    fromcalib = pfrmOutput.IsCalibLocation(fromOper.Name, fromOper.Id)
                Else
                    fromcalib = False
                End If
                For Each vConn In fromOper.Targets
                    lConn = vConn
                    If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
                        If lConn.Source.VolName = "COPY" Then
                            'assume this is a calibration location, skip it
                        Else
                            'this is an output from this operation, copy it
                            If fromcalib And lConn.Source.Group = "ROFLOW" And lConn.Source.Member = "ROVOL" Then
                            Else
                                'make sure we do not already have this output
                                found = False
                                For Each vToConn In toOper.Targets
                                    lToConn = vToConn
                                    If lToConn.Source.Group = lConn.Source.Group And _
                                       lToConn.Source.Member = lConn.Source.Member Then
                                        found = True
                                    End If
                                Next vToConn
                                If found = False Then
                                    'now add it
                                    Call pUCI.AddOutputWDMDataSet(Label1.Text, lConn.Target.Member, atxBase.Text, WDMId, newdsn)
                                    pUCI.AddExtTarget(toOper.Name, toOper.Id, lConn.Source.Group, lConn.Source.Member, _
                                       lConn.Source.MemSub1, lConn.Source.MemSub2, lConn.MFact, lConn.Tran, _
                                       "WDM" & CStr(WDMId), newdsn, lConn.Target.Member, lConn.Target.MemSub1, _
                                       lConn.Ssystem, lConn.Sgapstrg, lConn.Amdstrg)
                                    pUCI.Edited = True
                                End If
                            End If
                        End If
                    End If
                Next vConn
            Else
                lDialogResult = Logger.Message("An operation must be selected in the 'To Operation' list.", "Output Manager Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        ElseIf pCheckedRadioIndex = 3 Then 'add aquatox linkage
            If ListBox1.SelectedIndex > -1 Then
                Id = pListBox1DataItems.Item(ListBox1.SelectedIndex + 1)
                If Not pfrmOutput.IsAQUATOXLocation("RCHRES", (Id)) Then
                    'make sure required sections are on  
                    lOper = pUCI.OpnBlks("RCHRES").OperFromID(Id)
                    lTable = lOper.Tables("ACTIVITY")
                    If lTable.Parms(1).Value = 1 AndAlso lTable.Parms(4).Value = 1 AndAlso lTable.Parms(5).Value = 1 AndAlso lTable.Parms(7).Value = 1 AndAlso lTable.Parms(8).Value = 1 Then
                        'all required rchres sections are on
                        '(hydr, htrch, sedtrn, oxrx, nutrx)
                        CheckGQUALs(Id, gqualfg)
                        Me.Cursor = Cursors.WaitCursor
                        'add data sets
                        'pUCI.AddAQUATOXDsnsExt(Id, Label1.Text, atxBase.Text, lTable.Parms(9).Value, gqualfg, WDMId, member, msub1, tgroup, adsn, ostr, outtu)
                        pUCI.AddExpertDsns(Id, Label1.Text, atxBase.Text, adsn, ostr)
                        'add to ext targets block
                        pUCI.AddAQUATOXExtTargetsExt(Id, WDMId, member, msub1, tgroup, adsn, ostr, outtu)
                        pUCI.Edited = True
                        Me.Cursor = Cursors.Arrow
                    Else
                        ctemp = ""
                        If lTable.Parms(1).Value = 0 Then
                            ctemp = ctemp & "HYDR "
                        End If
                        If lTable.Parms(4).Value = 0 Then
                            ctemp = ctemp & "HTRCH "
                        End If
                        If lTable.Parms(5).Value = 0 Then
                            ctemp = ctemp & "SEDTRN "
                        End If
                        If lTable.Parms(7).Value = 0 Then
                            ctemp = ctemp & "OXRX "
                        End If
                        If lTable.Parms(8).Value = 0 Then
                            ctemp = ctemp & "NUTRX "
                        End If
                        lDialogResult = Logger.Message("The following required sections are not on: " & ctemp, "AQUATOX Linkage Problem", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                    End If
                End If
            End If
        End If

        If lDialogResult = Nothing Then
            pfrmOutput.RefreshAll()
            Me.Dispose()
        End If
        
    End Sub

    Private Sub CheckGQUALs(ByVal Id&, ByVal gqualfg&())
        Dim lTable As HspfTable
        Dim lOper As HspfOperation
        Dim ngqual&, i&, tname$
        Dim qname() As String
        Dim itemp As Integer
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim lDialogBoxResult As Windows.Forms.DialogResult

        lOper = pUCI.OpnBlks("RCHRES").OperFromID(Id)
        lTable = lOper.tables("ACTIVITY")
        If lTable.Parms(6).Value = 1 Then
            'gqual on
            'figure out how many gquals
            ngqual = 0
            For Each vOpn In pUCI.OpnBlks("RCHRES").Ids
                lOpn = vOpn
                If lOpn.TableExists("GQ-QALDATA") Then
                    If lOpn.TableExists("GQ-GENDATA") Then
                        itemp = lOpn.Tables("GQ-GENDATA").Parms("NGQUAL").Value
                    Else
                        itemp = 1
                    End If
                    If itemp > ngqual Then
                        ngqual = itemp
                    End If
                End If
            Next vOpn

            ReDim qname(ngqual)
            For i = 1 To ngqual
                If i = 1 Then
                    tname = "GQ-QALDATA"
                Else
                    tname = "GQ-QALDATA:" & i
                End If
                qname(i) = ""
                For Each vOpn In pUCI.OpnBlks("RCHRES").Ids
                    lOpn = vOpn
                    If lOpn.TableExists(tname) And Len(qname(i)) = 0 Then
                        qname(i) = Trim(lOpn.Tables(tname).Parms("GQID").Value)
                    End If
                Next vOpn
                lDialogBoxResult = Logger.Message("Do you want to include the total inflow of GQUAL " & i & " (" & qname(i) & ") in the AQUATOX Linkage?", "AQUATOX Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)


                If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                    gqualfg(i) = 1
                ElseIf lDialogBoxResult = Windows.Forms.DialogResult.No Then
                    gqualfg(i) = 0
                End If
            Next i
        Else
            gqualfg(1) = 0
            gqualfg(2) = 0
            gqualfg(3) = 0
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        Dim spacepos&, parenpos&, Id&, opname$, colonpos&
        Dim lOper As HspfOperation, group$, mem$
        Dim i, j, lGetMemberDefAtIndex As Integer
        Dim lGroupDef As HspfTSGroupDef

        spacepos = InStr(1, ListBox2.Items.Item(ListBox2.SelectedIndex), " ")
        parenpos = InStr(1, ListBox2.Items.Item(ListBox2.SelectedIndex), "(")
        If parenpos = 0 Then parenpos = Len(ListBox2.Items.Item(ListBox2.SelectedIndex)) + 1
        If spacepos > 0 And parenpos > 0 Then
            Id = CInt(Mid(ListBox2.Items.Item(ListBox2.SelectedIndex), spacepos, parenpos - spacepos))
            opname = Mid(ListBox2.Items.Item(ListBox2.SelectedIndex), 1, spacepos - 1)
            If Id > 0 And Len(opname) > 0 Then
                If opname = "PERLND" Then
                    txtLoc.Text = "P" & CStr(Id)
                ElseIf opname = "IMPLND" Then
                    txtLoc.Text = "I" & CStr(Id)
                ElseIf opname = "RCHRES" Then
                    txtLoc.Text = "RCH" & CStr(Id)
                ElseIf opname = "BMPRAC" Then
                    txtLoc.Text = "BMP" & CStr(Id)
                End If
            End If
        End If
        'set desc text
        spacepos = InStr(1, ListBox1.Items.Item(ListBox1.SelectedIndex), " ")
        parenpos = InStr(1, ListBox1.Items.Item(ListBox1.SelectedIndex), "(")
        If spacepos > 0 And parenpos > 0 Then
            Id = CInt(Mid(ListBox1.Items.Item(ListBox1.SelectedIndex), spacepos, parenpos - spacepos))
            opname = Mid(ListBox1.Items.Item(ListBox1.SelectedIndex), 1, spacepos - 1)
            If Id > 0 And Len(opname) > 0 Then
                lOper = pUCI.OpnBlks(opname).OperFromID(Id)
                colonpos = InStr(1, ListBox2.Items.Item(ListBox2.SelectedIndex), ":")
                If colonpos > 0 Then
                    group = Mid(ListBox2.Items.Item(ListBox2.SelectedIndex), 1, colonpos - 1)
                    parenpos = InStr(1, ListBox2.Items.Item(ListBox2.SelectedIndex), "(")
                    If parenpos > 0 Then
                        mem = Mid(ListBox2.Items.Item(ListBox2.SelectedIndex), colonpos + 1, parenpos - colonpos - 1)
                    Else
                        mem = Mid(ListBox2.Items.Item(ListBox2.SelectedIndex), colonpos + 1)
                    End If

                    For i = 0 To pUCI.Msg.TSGroupDefs.Count - 1
                        If pUCI.Msg.TSGroupDefs.Item(i).Name = group Then
                            lGroupDef = pUCI.Msg.TSGroupDefs.Item(i)

                            '.net conversion issue: Becuase no method to fetch via key in lGroupDef.MemberDefs was found, this loop does it manually.
                            For j = 0 To lGroupDef.MemberDefs.Count - 1
                                If lGroupDef.MemberDefs.Item(j).Name = mem Then
                                    lGetMemberDefAtIndex = j
                                    Exit For
                                End If
                            Next

                            'now set the text
                            txtDescription.Text = group & ":" & mem & " - " & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).Defn
                            If pUCI.GlobalBlock.EmFg = 1 Then
                                txtDescription.Text = txtDescription.Text & " (" & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).EUnits & ")"
                            Else
                                txtDescription.Text = txtDescription.Text & " (" & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).MUnits & ")"
                            End If
                            Exit For
                        End If


                    Next i
                End If
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim Id&, spacepos&, parenpos&, opname$
        Dim lOper As HspfOperation, lsub$
        Dim ctimser As Collection(Of HspfStatusType)
        Dim vTimser As Object, lTimser As HspfStatusType
        Dim ctemp As String
        Dim i As Integer
        Dim exttarget As Boolean
        Dim lOpnBlk As HspfOpnBlk
        Dim vConn As Object, lConn As HspfConnection

        If pCheckedRadioIndex = 1 Or pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Then
            Id = pListBox1DataItems.Item(ListBox1.SelectedIndex + 1)
            txtLoc.Text = "RCH" & CStr(Id)
        ElseIf pCheckedRadioIndex = 4 Then
            txtDescription.Text = ""
            spacepos = InStr(1, ListBox1.Items.Item(ListBox1.SelectedIndex), " ")
            parenpos = InStr(1, ListBox1.Items.Item(ListBox1.SelectedIndex), "(")
            If spacepos > 0 And parenpos > 0 Then
                Id = CInt(Mid(ListBox1.Items.Item(ListBox1.SelectedIndex), spacepos, parenpos - spacepos))
                opname = Mid(ListBox1.Items.Item(ListBox1.SelectedIndex), 1, spacepos - 1)
                If Id > 0 And Len(opname) > 0 Then
                    lOper = pUCI.OpnBlks(opname).OperFromID(Id)
                    If Not lOper Is Nothing Then
                        If opname = "PERLND" Then
                            txtLoc.Text = "P" & CStr(Id)
                        ElseIf opname = "IMPLND" Then
                            txtLoc.Text = "I" & CStr(Id)
                        ElseIf opname = "RCHRES" Then
                            txtLoc.Text = "RCH" & CStr(Id)
                        ElseIf opname = "BMPRAC" Then
                            txtLoc.Text = "BMP" & CStr(Id)
                        End If
                        'get possible timsers from edit operation method
                        lOper.OutputTimeseriesStatus.UpdateExtTargetsOutputs()
                        ctimser = lOper.OutputTimeseriesStatus.GetOutputInfo(2)
                        ListBox2.Items.Clear()
                        For Each vTimser In ctimser
                            lTimser = vTimser
                            exttarget = False
                            If lTimser.Present Then
                                'see if used as external target
                                For Each vConn In lOper.Targets
                                    lConn = vConn
                                    If UCase(Mid(lConn.Target.VolName, 1, 3)) = "WDM" Then
                                        'used as ext target, don't add
                                        i = InStr(1, lTimser.Name, ":")
                                        If i > 0 Then
                                            If lConn.Source.Group = Mid(lTimser.Name, 1, i - 1) Then
                                                If lConn.Source.Member = Mid(lTimser.Name, i + 1) Then
                                                    If lTimser.Max > 1 Then
                                                        If lConn.Source.MemSub1 = lTimser.Occur Mod 1000 Then
                                                            If lConn.Source.MemSub2 = CLng((1 + (lTimser.Occur) / 1000)) Then
                                                                exttarget = True
                                                            End If
                                                        End If
                                                    Else
                                                        exttarget = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next vConn
                            End If
                            If Not exttarget Then
                                If lTimser.Max = 1 Then
                                    ListBox2.Items.Add(lTimser.Name)
                                Else
                                    lsub = "(" & lTimser.Occur Mod 1000 & "," & CLng(1 + (lTimser.Occur) / 1000) & ")"
                                    ListBox2.Items.Add(lTimser.Name & lsub)
                                End If
                            End If
                        Next vTimser
                        ListBox2.Enabled = True
                        Label2.Enabled = True
                    End If
                End If
            End If
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            ctemp = ListBox1.Items.Item(ListBox1.SelectedIndex)
            spacepos = InStr(1, ctemp, " ")
            opname = Mid(ctemp, 1, spacepos - 1)
            Id = CInt(Mid(ctemp, spacepos + 1))
            ListBox2.Items.Clear()
            lOpnBlk = pUCI.OpnBlks(opname)
            For i = 1 To lOpnBlk.Count
                lOper = lOpnBlk.NthOper(i)
                If lOper.Id <> Id Then
                    ListBox2.Items.Add(lOper.Name & " " & lOper.Id)
                End If
            Next i
            If ListBox2.Items.Count > 0 Then
                ListBox2.Enabled = True
            Else
                ListBox2.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

End Class