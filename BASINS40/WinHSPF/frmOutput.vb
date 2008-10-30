Imports atcUtility
Imports atcUCIForms
Imports atcUCI
Imports atcControls
Imports System


Public Class frmOutput
    ' ==============================================================
    '.net Conversion issue: Imported VB6 code. Made modifications to array and collection
    'idices i.e. starting from 0. In VB6 code there was variable named dsnObj Dimensioned as ATCclsTserData.
    'At time of writing this comment, there was no clear implementation or replacement of ATCclsTserData class.
    'Actions for radio buttons 3 and 4 in Refreshall() are commented out. Many of the included functions come 
    'from various other classes in old VB6 code. Indices were changed to avoid errors and 
    '
    '  *  Indices on for-loops of pUCI.OpnSeqBlock.Opns.Count are {1, .Count-1} (formerly {1, .Count}
    '  *  Indices on for-loops of loper2.Targets.Count are {0, .Count-1} (formerly {1, .Count}
    '
    ' 
    'Written 29 October 2008 by Brandon G.
    ' ==============================================================

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

        With agdOutput
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        radio3.Checked = True
        cmdCopy.Enabled = False
        RefreshAll()

    End Sub

    Public Sub RefreshAll()

        ' Commented Dims that are unused for now to avoid warning messages

        'Dim loper As HspfOperation
        'Dim s As String
        'Dim i, j As Integer
        'Dim vConn As Object, lConn As HspfConnection
        'Dim dsnObj As ATCclsTserData
        'Dim WDMId$, idsn&, ctemp$
        'Dim lRow As Integer

        Dim loper As HspfOperation
        Dim i As Integer
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

                For i = 1 To pUCI.OpnSeqBlock.Opns.Count - 1
                    loper = pUCI.OpnSeqBlock.Opn(i)
                    If loper.Name = "RCHRES" Then
                        If IsCalibLocation(loper.Name, loper.Id) Then
                            'this is an expert system output location
                            lRow = .Rows
                            .CellValue(lRow, 0) = loper.Name & " " & loper.Id
                            .CellValue(lRow, 1) = loper.Description
                        End If
                    End If
                Next i
                agdOutput.SizeAllColumnsToContents()
            End With
            cmdCopy.Enabled = False

        ElseIf radio2.Checked Then
            txtDesc.Text = "Streamflow output will be generated at each 'Flow' output location."
            With agdOutput.Source
                .Rows = 0
                .Columns = 2
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Description"

                For i = 1 To pUCI.OpnSeqBlock.Opns.Count - 1
                    loper = pUCI.OpnSeqBlock.Opn(i)
                    If loper.Name = "RCHRES" Then
                        If IsFlowLocation(loper.Name, loper.Id) Then
                            'this is an output flow location
                            lRow = .Rows
                            .CellValue(lRow, 0) = loper.Name & " " & loper.Id
                            .CellValue(lRow, 1) = loper.Description
                        End If
                    End If
                Next i
                agdOutput.SizeAllColumnsToContents()
            End With
            cmdCopy.Visible = False
        ElseIf radio3.Checked Then
            txtDesc.Text = "Output will be generated at each 'Other' output location " & "for the specified constituents."
            '    With agdOutput.Source
            '        .Rows = 0
            '        .Columns = 3
            '        .CellValue(0, 0) = "Name"
            '        .CellValue(0, 1) = "Description"
            '        For i = 1 To pUCI.OpnSeqBlock.Opns.Count - 1
            '            loper = pUCI.OpnSeqBlock.Opn(i)
            '            'look for any output from here in ext targets
            '            For Each vConn In loper.Targets
            '                lConn = vConn
            '                lRow = .Rows
            '                If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
            '                    If lConn.Source.VolName = "COPY" Then
            '                        'assume this is a calibration location, skip it
            '                    ElseIf lConn.Source.Group = "ROFLOW" And lConn.Source.Member = "ROVOL" Then
            '                        'this is part of the calibration location
            '                    ElseIf lConn.Source.Group = "HYDR" And lConn.Source.Member = "RO" And IsFlowLocation(loper.Name, loper.Id) Then
            '                        'this is an output flow location
            '                    Else
            '                        idsn = lConn.Target.VolId
            '                        WDMId = lConn.Target.VolName

            '                        MsgBox(pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn).ToString)

            '                        dsnObj = pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn)
            '                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
            '                            'this an an aquatox output location
            '                        Else
            '                            'this is an other output location

            '                            .CellValue(lRow, 0) = loper.Name & " " & loper.Id
            '                            .CellValue(lRow, 1) = loper.Description
            '                            ctemp = lConn.Source.Group & ":" & lConn.Source.Member
            '                            If TSMaxSubscript(1, lConn.Source.Group, lConn.Source.Member) > 1 Then
            '                                ctemp = ctemp & "(" & lConn.Source.MemSub1
            '                                If TSMaxSubscript(2, lConn.Source.Group, lConn.Source.Member) > 1 Then
            '                                    ctemp = ctemp & "," & lConn.Source.MemSub2
            '                                End If
            '                                ctemp = ctemp & ")"
            '                            End If
            '                            .CellValue(lRow, 2) = ctemp
            '                        End If
            '                    End If
            '                End If
            '            Next vConn
            '        Next i
            '        agdOutput.SizeAllColumnsToContents()
            '    End With
            '    cmdCopy.Visible = True
        ElseIf radio4.Checked Then
            txtDesc.Text = "Output will be generated at each 'AQUATOX Linkage' output location for " & _
              "inflow, discharge, surface area, mean depth, water temperature, suspended sediment, " & _
              "organic chemicals (if available), and inflows of nutrients, " & _
              "DO, BOD, refractory organic carbon, and sediment."
            '    With agdOutput.Source
            '        .Rows = 0
            '        .Columns = 2
            '        .CellValue(0, 0) = "Name"
            '        .CellValue(0, 1) = "Description"
            '        For i = 1 To pUCI.OpnSeqBlock.Opns.Count
            '            loper = pUCI.OpnSeqBlock.Opn(i)
            '            If loper.Name = "RCHRES" Then
            '                If IsAQUATOXLocation(loper.Name, loper.Id) Then
            '                    'this is an expert system output location
            '                    .Rows = .Rows + 1
            '                    .CellValue(.Rows, 0) = loper.Name & " " & loper.Id
            '                    .CellValue(.Rows, 1) = loper.Description
            '                End If
            '            End If
            '        Next i
            '        agdOutput.SizeAllColumnsToContents()
            '    End With
        End If
    End Sub

    Public Function IsCalibLocation(ByVal Name As String, ByVal Id As Integer) As Boolean
        'call it a calib loc if there are copy ops to wdm and
        '  this reach is associated with a copy ifwo dataset,
        '  there may be a better way

        ' Commented Dims that are unused for now to avoid warning messages

        'Dim lTable As HspfTable
        'Dim loper2 As HspfOperation
        'Dim lConn As HspfConnection
        'Dim expertflag As Boolean
        'Dim s As String
        'Dim copyid, i, j As Integer


        Dim loper2 As HspfOperation
        Dim lConn As HspfConnection
        Dim expertflag As Boolean
        Dim copyid, i, j As Integer

        IsCalibLocation = False
        expertflag = False

        For i = 1 To pUCI.OpnSeqBlock.Opns.Count - 1
            loper2 = pUCI.OpnSeqBlock.Opn(i)

            If loper2.Name = "COPY" Then

                For j = 0 To loper2.Targets.Count - 1
                    lConn = loper2.Targets(j)

                    If Microsoft.VisualBasic.Left(lConn.Target.VolName, 3) = "WDM" And Trim(lConn.Target.Member) = "IFWO" Then
                        'looks like we have some expert system output locations
                        expertflag = True
                    End If
                Next

            End If

        Next i

        copyid = Reach2Copy(Id)

        If copyid > 0 Then
            loper2 = pUCI.OpnBlks("COPY").OperFromID(copyid)
            For j = 0 To loper2.Targets.Count - 1
                lConn = loper2.Targets(j)
                If Microsoft.VisualBasic.Left(lConn.Target.VolName, 3) = "WDM" And _
                  Trim(lConn.Target.Member) = "IFWO" And expertflag Then
                    'this is an expert system output location
                    IsCalibLocation = True
                End If
            Next j
        End If

    End Function

    Public Function IsFlowLocation(ByVal Name As String, ByVal Id As Integer) As Boolean

        ' Commented Dims that are unused for now to avoid warning messages

        'Dim lTable As HspfTable
        'Dim loper4 As HspfOperation
        'Dim lConn7 As HspfConnection
        'Dim expertflag As Boolean
        'Dim copyid, i, j As Integer
        'Dim s As String

        Dim j As Integer
        Dim loper4 As HspfOperation
        Dim lConn7 As HspfConnection

        IsFlowLocation = False

        If Id > 0 Then
            loper4 = pUCI.OpnBlks(Name).OperFromID(Id)

            For j = 0 To loper4.Targets.Count - 1
                lConn7 = loper4.Targets(j)
                If Microsoft.VisualBasic.Left(lConn7.Target.VolName, 3) = "WDM" And _
                  Trim(lConn7.Target.Member) = "FLOW" Then
                    'this is an output flow location
                    IsFlowLocation = True
                End If
            Next j
        End If

    End Function

    Private Function WDMInd(ByVal WDMId$) As Long
        Dim w$

        If Len(WDMId) > 3 Then
            w = Mid(WDMId, 4, 1)
            If w = " " Then w = "1"
        Else
            w = "1"
        End If
        WDMInd = w
    End Function

    Public Function TSMaxSubscript(ByVal subno&, ByVal group$, ByVal member$)
        Dim i, j As Integer

        TSMaxSubscript = 0

        MsgBox(pMsg.TSGroupDefs.Count)

        For i = 0 To pMsg.TSGroupDefs.Count - 1

            If pMsg.TSGroupDefs.Item(i).Name = "group" Then
                For j = 1 To pMsg.TSGroupDefs(i).MemberDefs.Count
                    If pMsg.TSGroupDefs(i).MemberDefs(j).Name = member Then
                        If subno = 1 Then
                            TSMaxSubscript = pMsg.TSGroupDefs(i).MemberDefs(j).Maxsb1
                        Else
                            TSMaxSubscript = pMsg.TSGroupDefs(i).MemberDefs(j).Maxsb2
                        End If
                        Exit For
                    End If
                Next j
                Exit For
            End If
        Next i
    End Function

    Public Function IsAQUATOXLocation(ByVal Name$, ByVal Id&) As Boolean
        'call it an aquatox loc if required sections are on and
        '  this reach has required output
        'Dim dsnObj As ATCclsTserData

        ' Commented Dims that are unused for now to avoid warning messages

        'Dim dsnObj As Object
        'Dim lTable As HspfTable
        'Dim loper As HspfOperation
        'Dim lConn As HspfConnection
        'Dim expertflag As Boolean
        'Dim copyid, i, j As Integer
        'Dim s As String
        'Dim ifound(7) As Boolean, idsn&, WDMId$

        Dim dsnObj As Object
        Dim lTable As HspfTable
        Dim loper As HspfOperation
        Dim lConn As HspfConnection
        Dim j As Integer
        Dim ifound(7) As Boolean, idsn&, WDMId$

        IsAQUATOXLocation = False
        loper = pUCI.OpnBlks(Name).OperFromID(Id)
        lTable = loper.tables("ACTIVITY")
        If lTable.Parms(1).Value = 1 And lTable.Parms(4).Value = 1 And _
           lTable.Parms(5).Value = 1 And lTable.Parms(7).Value = 1 And _
           lTable.Parms(8).Value = 1 Then
            'all required rchres sections are on
            '(hydr, htrch, sedtrn, oxrx, nutrx)
            For j = 1 To 7
                ifound(j) = False
            Next j
            For j = 1 To loper.targets.Count
                lConn = loper.targets(j)
                If Microsoft.VisualBasic.Left(lConn.Target.VolName, 3) = "WDM" Then
                    idsn = lConn.Target.VolId
                    WDMId = lConn.Target.VolName
                    dsnObj = pUCI.GetDataSetFromDsn(WDMInd(WDMId), idsn)
                    If Trim(lConn.Source.Member) = "AVDEP" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(1) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "SAREA" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(2) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "IVOL" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(3) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "RO" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(4) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "TW" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(5) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "NUIF1" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(6) = True
                        End If
                    ElseIf Trim(lConn.Source.Member) = "OXIF" Then
                        If InStr(1, UCase(dsnObj.Header.Desc), "AQUATOX") Then
                            ifound(7) = True
                        End If
                    End If
                End If
            Next j
            If ifound(1) And ifound(2) And ifound(3) And ifound(4) And _
               ifound(5) And ifound(6) And ifound(7) Then
                'this is an aquatox output location
                IsAQUATOXLocation = True
            End If
        End If

    End Function


    Private Function Reach2Copy(ByVal rchid&) As Long
        'given a reach id, find its associated copy for expert system datasets

        Dim loper3 As HspfOperation
        Dim lConn2 As HspfConnection
        Dim i, j As Integer
        Dim larea!, ldsn!, copyid&
        'Dim s As String

        copyid = 0
        loper3 = pUCI.OpnBlks("RCHRES").OperFromID(rchid)
        For j = 1 To loper3.Targets.Count - 1
            lConn2 = loper3.Targets(j)
            If Microsoft.VisualBasic.Left(lConn2.Target.VolName, 3) = "WDM" And _
              Trim(lConn2.Target.Member) = "SIMQ" Then
                'this is an expert system output locn, save area and dsn
                ldsn = lConn2.Target.VolId
                larea = lConn2.MFact
            End If
        Next j

        For i = 1 To pUCI.OpnSeqBlock.Opns.Count - 2
            loper3 = pUCI.OpnSeqBlock.Opn(i)
            If loper3.Name = "COPY" Then
                For j = 1 To loper3.Targets.Count
                    lConn2 = loper3.Targets(j)
                    If Microsoft.VisualBasic.Left(lConn2.Target.VolName, 3) = "WDM" And _
                      Trim(lConn2.Target.Member) = "IFWO" Then
                        If Math.Abs(larea - (lConn2.MFact * 12)) < 0.000001 Then
                            'this appears to be the associated copy
                            copyid = loper3.Id
                        End If
                    End If
                Next j
            End If
        Next i
        Reach2Copy = copyid
    End Function

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub radio1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio1.CheckedChanged
        RefreshAll()
    End Sub

    Private Sub radio2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio2.CheckedChanged
        RefreshAll()
    End Sub

    Private Sub radio3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio3.CheckedChanged
        RefreshAll()
    End Sub

    Private Sub radio4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radio4.CheckedChanged
        RefreshAll()
    End Sub

End Class