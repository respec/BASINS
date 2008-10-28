Imports atcUtility
Imports atcUCIForms
Imports atcUCI
Imports atcControls

Public Class frmOutput
    Dim pVScrollColumnOffset As Integer = 16

    Dim pPChanged As Boolean
    Dim pIChanged As Boolean
    Dim pRChanged As Boolean
    Dim pPGrid, pIGrid, pRGrid As atcGrid

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size

        Me.Text = "WinHSPF - Output Manager"
        Me.Icon = pIcon

        cmdCopy.Enabled = False



    End Sub

    'Public Sub RefreshAll()

    '    Dim loper As HspfOperation
    '    Dim i&, s$, j&
    '    Dim vConn As Object, lConn As HspfConnection
    '    'Dim dsnObj As ATCclsTserData
    '    Dim dsnObj As Object
    '    Dim WDMId$, idsn&, ctemp$

    '    If radio1.Checked Then
    '        txtDesc.Text = "Output will be generated at each 'Hydrology Calibration' output location for " & _
    '          "total runoff, surface runoff, interflow, base flow, potential evapotranspiration, actual evapotranspiration, " & _
    '          "upper zone storage, and lower zone storage."
    '        With agdOutput.Source
    '            .Rows = 0
    '            .Columns = 2
    '            .CellValue(0, 0) = "Name"
    '            .CellValue(0, 1) = "Description"
    '            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
    '                loper = pUCI.OpnSeqBlock.Opn(i)
    '                If loper.Name = "RCHRES" Then
    '                    If IsCalibLocation(loper.Name, loper.Id) Then
    '                        'this is an expert system output location
    '                        .Rows = .Rows + 1
    '                        .CellValue(.Rows, 0) = loper.Name & " " & loper.Id
    '                        .CellValue(.Rows, 1) = loper.Description
    '                    End If
    '                End If
    '            Next i
    '            agdOutput.SizeAllColumnsToContents()
    '        End With
    '        cmdCopy.Enabled = False
    '    ElseIf radio2.Checked Then
    '        txtDesc.Text = "Streamflow output will be generated at each 'Flow' output location."
    '        With agdOutput.Source
    '            .Rows = 0
    '            .Columns = 2
    '            .CellValue(0, 0) = "Name"
    '            .CellValue(0, 1) = "Description"
    '            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
    '                loper = pUCI.OpnSeqBlock.Opn(i)
    '                If loper.Name = "RCHRES" Then
    '                    If IsFlowLocation(loper.Name, loper.Id) Then
    '                        'this is an output flow location
    '                        .Rows = .Rows + 1
    '                        .CellValue(.Rows, 0) = loper.Name & " " & loper.Id
    '                        .CellValue(.Rows, 1) = loper.Description
    '                    End If
    '                End If
    '            Next i
    '            agdOutput.SizeAllColumnsToContents()
    '        End With
    '        cmdCopy.Visible = False
    '    ElseIf radio3.Checked Then
    '        txtDesc.Text = "Output will be generated at each 'Other' output location " & _
    '          "for the specified constituents."
    '        With agdOutput.Source
    '            .Rows = 0
    '            .Columns = 3
    '            .CellValue(0, 0) = "Name"
    '            .CellValue(0, 1) = "Description"
    '            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
    '                loper = pUCI.OpnSeqBlock.Opn(i)
    '                'look for any output from here in ext targets
    '                For Each vConn In loper.Targets
    '                    lConn = vConn
    '                    If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
    '                        If lConn.Source.VolName = "COPY" Then
    '                            'assume this is a calibration location, skip it
    '                        ElseIf lConn.Source.Group = "ROFLOW" And lConn.Source.Member = "ROVOL" Then
    '                            'this is part of the calibration location
    '                        ElseIf lConn.Source.Group = "HYDR" And lConn.Source.Member = "RO" And IsFlowLocation(loper.Name, loper.Id) Then
    '                            'this is an output flow location
    '                        Else
    '                            idsn = lConn.Target.VolId
    '                            WDMId = lConn.Target.VolName
    '                            dsnObj = pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn)
    '                            If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                                'this an an aquatox output location
    '                            Else
    '                                'this is an other output location
    '                                .Rows = .Rows + 1
    '                                .CellValue(.Rows, 0) = loper.Name & " " & loper.Id
    '                                .CellValue(.Rows, 1) = loper.Description
    '                                ctemp = lConn.Source.Group & ":" & lConn.Source.Member
    '                                If TSMaxSubscript(1, lConn.Source.Group, lConn.Source.Member) > 1 Then
    '                                    ctemp = ctemp & "(" & lConn.Source.MemSub1
    '                                    If TSMaxSubscript(2, lConn.Source.Group, lConn.Source.Member) > 1 Then
    '                                        ctemp = ctemp & "," & lConn.Source.MemSub2
    '                                    End If
    '                                    ctemp = ctemp & ")"
    '                                End If
    '                                .CellValue(.Rows, 2) = ctemp
    '                            End If
    '                        End If
    '                    End If
    '                Next vConn
    '            Next i
    '            agdOutput.SizeAllColumnsToContents()
    '        End With
    '        cmdCopy.Visible = True
    '    ElseIf radio4.Checked Then
    '        txtDesc.Text = "Output will be generated at each 'AQUATOX Linkage' output location for " & _
    '          "inflow, discharge, surface area, mean depth, water temperature, suspended sediment, " & _
    '          "organic chemicals (if available), and inflows of nutrients, " & _
    '          "DO, BOD, refractory organic carbon, and sediment."
    '        With agdOutput.Source
    '            .Rows = 0
    '            .Columns = 2
    '            .CellValue(0, 0) = "Name"
    '            .CellValue(0, 1) = "Description"
    '            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
    '                loper = pUCI.OpnSeqBlock.Opn(i)
    '                If loper.Name = "RCHRES" Then
    '                    If IsAQUATOXLocation(loper.Name, loper.Id) Then
    '                        'this is an expert system output location
    '                        .Rows = .Rows + 1
    '                        .CellValue(.Rows, 0) = loper.Name & " " & loper.Id
    '                        .CellValue(.Rows, 1) = loper.Description
    '                    End If
    '                End If
    '            Next i
    '            agdOutput.SizeAllColumnsToContents()
    '        End With
    '    End If
    'End Sub
    'Public Function IsCalibLocation(ByVal Name$, ByVal Id&) As Boolean
    '    'call it a calib loc if there are copy ops to wdm and
    '    '  this reach is associated with a copy ifwo dataset,
    '    '  there may be a better way

    '    Dim lTable As HspfTable
    '    Dim loper As HspfOperation
    '    Dim lConn As HspfConnection
    '    Dim i&, s$, expertflag As Boolean, copyid&
    '    Dim j As Integer

    '    IsCalibLocation = False
    '    expertflag = False
    '    For i = 1 To pUCI.OpnSeqBlock.Opns.Count
    '        loper = pUCI.OpnSeqBlock.Opn(i)
    '        If loper.Name = "COPY" Then
    '            For j = 1 To loper.Targets.Count
    '                lConn = loper.Targets(j)
    '                If Microsoft.VisualBasic.Left(lConn.Target.VolName, 3) = "WDM" And Trim(lConn.Target.Member) = "IFWO" Then
    '                    'looks like we have some expert system output locations
    '                    expertflag = True
    '                End If
    '            Next j
    '        End If
    '    Next i
    '    copyid = Reach2Copy(Id)
    '    If copyid > 0 Then
    '        loper = myUci.OpnBlks("COPY").operfromid(copyid)
    '        For j = 1 To loper.targets.Count
    '            lConn = loper.targets(j)
    '            If Left(lConn.Target.volname, 3) = "WDM" And _
    '              Trim(lConn.Target.member) = "IFWO" And expertflag Then
    '                'this is an expert system output location
    '                IsCalibLocation = True
    '            End If
    '        Next j
    '    End If

    'End Function

    'Public Function IsFlowLocation(ByVal Name$, ByVal Id&) As Boolean

    '    Dim lTable As HspfTable
    '    Dim loper As HspfOperation
    '    Dim lConn As HspfConnection
    '    Dim i&, s$, j&, expertflag As Boolean, copyid&

    '    IsFlowLocation = False

    '    If Id > 0 Then
    '        loper = myUci.OpnBlks(Name).operfromid(Id)
    '        For j = 1 To loper.targets.Count
    '            lConn = loper.targets(j)
    '            If Left(lConn.Target.volname, 3) = "WDM" And _
    '              Trim(lConn.Target.member) = "FLOW" Then
    '                'this is an output flow location
    '                IsFlowLocation = True
    '            End If
    '        Next j
    '    End If

    'End Function

    'Private Function WDMInd(ByVal WDMId$) As Long
    '    Dim w$

    '    If Len(WDMId) > 3 Then
    '        w = Mid(WDMId, 4, 1)
    '        If w = " " Then w = "1"
    '    Else
    '        w = "1"
    '    End If
    '    WDMInd = w
    'End Function

    'Public Function TSMaxSubscript(ByVal subno&, ByVal group$, ByVal member$)
    '    Dim i, j As Integer

    '    TSMaxSubscript = 0
    '    For i = 1 To pMsg.TSGroupDefs.Count

    '        If pMsg.TSGroupDefs.Item(i).Name = "group" Then
    '            For j = 1 To pMsg.TSGroupDefs(i).MemberDefs.Count
    '                If pMsg.TSGroupDefs(i).MemberDefs(j).Name = member Then
    '                    If subno = 1 Then
    '                        TSMaxSubscript = pMsg.TSGroupDefs(i).MemberDefs(j).Maxsb1
    '                    Else
    '                        TSMaxSubscript = pMsg.TSGroupDefs(i).MemberDefs(j).Maxsb2
    '                    End If
    '                    Exit For
    '                End If
    '            Next j
    '            Exit For
    '        End If
    '    Next i
    'End Function

    'Public Function IsAQUATOXLocation(ByVal Name$, ByVal Id&) As Boolean
    '    'call it an aquatox loc if required sections are on and
    '    '  this reach has required output
    '    Dim dsnObj As ATCclsTserData
    '    Dim lTable As HspfTable
    '    Dim loper As HspfOperation
    '    Dim lConn As HspfConnection
    '    Dim i&, s$, j&, expertflag As Boolean, copyid&
    '    Dim ifound(7) As Boolean, idsn&, WDMId$

    '    IsAQUATOXLocation = False
    '    loper = myUci.OpnBlks(Name).operfromid(Id)
    '    lTable = loper.tables("ACTIVITY")
    '    If lTable.Parms(1).Value = 1 And lTable.Parms(4).Value = 1 And _
    '       lTable.Parms(5).Value = 1 And lTable.Parms(7).Value = 1 And _
    '       lTable.Parms(8).Value = 1 Then
    '        'all required rchres sections are on
    '        '(hydr, htrch, sedtrn, oxrx, nutrx)
    '        For j = 1 To 7
    '            ifound(j) = False
    '        Next j
    '        For j = 1 To loper.targets.Count
    '            lConn = loper.targets(j)
    '            If Left(lConn.Target.volname, 3) = "WDM" Then
    '                idsn = lConn.Target.volid
    '                WDMId = lConn.Target.volname
    '                dsnObj = myUci.GetDataSetFromDsn(WDMInd(WDMId), idsn)
    '                If Trim(lConn.Source.member) = "AVDEP" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(1) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "SAREA" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(2) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "IVOL" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(3) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "RO" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(4) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "TW" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(5) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "NUIF1" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(6) = True
    '                    End If
    '                ElseIf Trim(lConn.Source.member) = "OXIF" Then
    '                    If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
    '                        ifound(7) = True
    '                    End If
    '                End If
    '            End If
    '        Next j
    '        If ifound(1) And ifound(2) And ifound(3) And ifound(4) And _
    '           ifound(5) And ifound(6) And ifound(7) Then
    '            'this is an aquatox output location
    '            IsAQUATOXLocation = True
    '        End If
    '    End If

    'End Function

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

End Class