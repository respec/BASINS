Imports atcUCI
Imports WinHSPF
Imports atcData


Public Class frmAddMet

    Dim Segname, curtext As String
    Dim lmetseg As HspfMetSeg
    Dim AddEdit As Integer
    Dim aMetDetails() As String


    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        'Dim i&, r&, nwdm&, aunits&(), j&, WDMId$
        'Dim numMetSeg&, arrayMetSegs$(), cntMetSeg&
        'Dim lMetDetails$(), lMetDescs$(), lId&, ldsn&

        'If AddEdit = 1 Then
        '    cboName.Visible = False
        'Else
        '    If AddEdit = 2 Then
        '        Me.Name = "WinHSPF - Initial Met Segment"
        '    Else
        '        Me.Name = "WinHSPF - Add Met Segment"
        '    End If
        '    lblName.Visible = False
        '    'add candidate met seg names to list
        '    pUCI.GetWDMUnits(nwdm, aunits)
        '    cntMetSeg = 0
        '    cboName.Items.Clear()
        '    For i = 1 To nwdm

        '        pUCI.GetMetSegNames(aunits(i), numMetSeg, arrayMetSegs, lMetDetails, lMetDescs)

        '        pUCI.GetWDMIDFromUnit(aunits(i), WDMId)
        '        If numMetSeg > 0 Then
        '            cntMetSeg = cntMetSeg + numMetSeg
        '            ReDim Preserve aMetDetails(cntMetSeg)
        '            For j = 1 To numMetSeg
        '                cboName.Items.Add(arrayMetSegs(j - 1) & ":" & lMetDescs(j - 1))
        '                aMetDetails(cntMetSeg - numMetSeg + j) = lMetDetails(j - 1) & ", " & WDMId
        '                If AddEdit = 2 Then
        '                    If InStr(1, Segname, arrayMetSegs(j - 1)) > 0 Then
        '                        cboName.SelectedIndex = j - 1  'set to what is selected on create form
        '                    End If
        '                End If
        '            Next j
        '        End If
        '    Next i
        '    If numMetSeg > 0 And cboName.SelectedIndex < 0 Then
        '        cboName.SelectedIndex = 0
        '    End If
        'End If

        'lblName.Text = Segname
        'With agdMet.Source
        '    .CellValue(0, 0) = "Constituent"
        '    .CellValue(0, 1) = "WDM ID"
        '    .CellValue(0, 2) = "TSTYPE"
        '    .CellValue(0, 3) = "Data Set"
        '    .CellValue(0, 4) = "Mfact P/I"
        '    .CellValue(0, 5) = "Mfact R"
        '    .CellValue(1, 0) = "Precip"
        '    .CellValue(2, 0) = "Air Temp"
        '    .CellValue(3, 0) = "Dew Point"
        '    .CellValue(4, 0) = "Wind"
        '    .CellValue(5, 0) = "Solar Rad"
        '    .CellValue(6, 0) = "Cloud"
        '    .CellValue(7, 0) = "Pot Evap"
        '    If AddEdit = 1 Then
        '        lmetseg = Nothing
        '        For i = 1 To pUCI.MetSegs.Count  'find which met seg this is
        '            If pUCI.MetSegs(i).Name = Segname Then
        '                lmetseg = pUCI.MetSegs(i)
        '            End If
        '        Next i

        '        If Not lmetseg Is Nothing Then
        '            For r = 1 To 7
        '                If Len(lmetseg.MetSegRec(r).Source.volname) < 4 Then
        '                    lId = 1
        '                Else
        '                    lId = CInt(Mid(lmetseg.MetSegRec(r).Source.volname, 4, 1))
        '                End If
        '                ldsn = lmetseg.MetSegRec(r).Source.volid
        '                .CellValue(r, 1) = lmetseg.MetSegRec(r).Source.volname
        '                .CellValue(r, 2) = lmetseg.MetSegRec(r).Source.member
        '                .CellValue(r, 3) = ldsn & LocFromDsn(lId, ldsn)
        '                .CellValue(r, 4) = lmetseg.MetSegRec(r).MFactP
        '                .CellValue(r, 5) = lmetseg.MetSegRec(r).MFactR
        '            Next r
        '        End If
        '    End If
        'End With

        'agdMet.SizeAllColumnsToContents()


    End Sub

End Class