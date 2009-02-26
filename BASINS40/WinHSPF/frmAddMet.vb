Imports atcUCI
Imports MapWinUtility
Imports atcUtility

Public Class frmAddMet

    'Dim Segname As String
    'Dim curtext As String
    'Dim lmetseg As HspfMetSeg
    'Dim AddEdit As Integer
    Dim pSegmentName As String
    Dim pMetSeg As HspfMetSeg
    Dim pAddEdit As Integer

    Dim pMetSegNames As New Collection
    Dim pMetSegBaseDsns As New Collection
    Dim pMetSegWDMIds As New Collection
    Dim pMetSegDescs As New Collection

    Public Sub Init(ByVal aSegmentName As String, ByVal aAddEditOption As Integer)
        pSegmentName = aSegmentName
        pAddEdit = aAddEditOption

        If pAddEdit = 1 Then
            Me.Text = "WinHSPF - Edit Met Segment"
            cboName.Visible = False
        Else
            Me.Text = "WinHSPF - Add Met Segment"
            lblName.Visible = False
            'add candidate met seg names to list
            cboName.Items.Clear()

            pMetSegNames.Clear()
            pMetSegBaseDsns.Clear()
            pMetSegWDMIds.Clear()
            pMetSegDescs.Clear()
            pUCI.GetMetSegNames(pMetSegNames, pMetSegBaseDsns, pMetSegWDMIds, pMetSegDescs)

            If pMetSegNames.Count > 0 Then
                For lIndex As Integer = 1 To pMetSegNames.Count
                    cboName.Items.Add(pMetSegNames(lIndex) & ":" & pMetSegDescs(lIndex))
                Next lIndex
                cboName.SelectedIndex = 0
            End If
        End If

        lblName.Text = pSegmentName

        With agdMet
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.FixedRows = 1
        End With

        With agdMet.Source
            .Rows = 0
            .Columns = 6
            .CellValue(0, 0) = "Constituent"
            .CellValue(0, 1) = "WDM ID"
            .CellValue(0, 2) = "TSTYPE"
            .CellValue(0, 3) = "Data Set"
            .CellValue(0, 4) = "Mfact P/I"
            .CellValue(0, 5) = "Mfact R"
            .CellValue(1, 0) = "Precip"
            .CellValue(2, 0) = "Air Temp"
            .CellValue(3, 0) = "Dew Point"
            .CellValue(4, 0) = "Wind"
            .CellValue(5, 0) = "Solar Rad"
            .CellValue(6, 0) = "Cloud"
            .CellValue(7, 0) = "Pot Evap"

            If pAddEdit = 1 Then
                pMetSeg = Nothing
                For Each lTempMetSeg As HspfMetSeg In pUCI.MetSegs  'find which met seg this is
                    If lTempMetSeg.Name = pSegmentName Then
                        pMetSeg = lTempMetSeg
                    End If
                Next
                If Not pMetSeg Is Nothing Then
                    For lRow As Integer = 1 To 7
                        Dim lId As Integer
                        If Len(pMetSeg.MetSegRecs(lRow).Source.VolName) < 4 Then
                            lId = 1
                        Else
                            lId = CInt(Mid(pMetSeg.MetSegRecs(lRow).Source.VolName, 4, 1))
                        End If
                        Dim ldsn As Integer = pMetSeg.MetSegRecs(lRow).Source.VolId
                        .CellValue(lRow, 1) = pMetSeg.MetSegRecs(lRow).Source.VolName
                        .CellValue(lRow, 2) = pMetSeg.MetSegRecs(lRow).Source.Member
                        .CellValue(lRow, 3) = ldsn & LocFromDsn(lId, ldsn)
                        .CellValue(lRow, 4) = pMetSeg.MetSegRecs(lRow).MFactP
                        .CellValue(lRow, 5) = pMetSeg.MetSegRecs(lRow).MFactR
                        .CellEditable(lRow, 1) = True
                        .CellEditable(lRow, 2) = True
                        .CellEditable(lRow, 3) = True
                        .CellEditable(lRow, 4) = True
                        .CellEditable(lRow, 5) = True
                    Next lRow
                End If
            End If
        End With

        agdMet.SizeAllColumnsToContents()
        agdMet.Refresh()
    End Sub

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Private Sub agdMet_CommitChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)
        'DoLimits(agdMet)
        'If curtext <> agdMet.TextMatrix(agdMet.row, agdMet.col) Then
        '    If agdMet.row = 1 And (agdMet.col = 1 Or agdMet.col = 2 Or agdMet.col = 3) And _
        '       AddEdit = 1 Then
        '        'cant change these, set it back
        '        MsgBox("The Precip Data Set defines the met segment and thus cannot be changed." & vbCrLf & _
        '               "Use 'Add' to add a new met segment.", vbOKOnly, "Edit Met Segment Problem")
        '        agdMet.TextMatrix(agdMet.row, agdMet.col) = curtext
        '    Else
        '        ClearRestofRow()
        '    End If
        'End If
        'If AddEdit = 0 Then
        '    'adding, check for precip data set
        '    If agdMet.row = 1 And (agdMet.col = 1 Or agdMet.col = 2 Or agdMet.col = 3) Then
        '        If Len(Trim(agdMet.TextMatrix(1, 1))) > 0 And _
        '           Len(Trim(agdMet.TextMatrix(1, 2))) > 0 And _
        '           Len(Trim(agdMet.TextMatrix(1, 3))) > 0 Then
        '            SegName = myUci.GetWDMAttr(agdMet.TextMatrix(1, 1), DsnOnly(agdMet.TextMatrix(1, 3)), "LOC")
        '            lblName.Caption = SegName
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub ClearRestofRow()
        'With agdMet
        '    If .col = 1 Then  'wdm ids, clear tstypes and dsns
        '        .TextMatrix(.row, 2) = ""
        '        .TextMatrix(.row, 3) = ""
        '    ElseIf .col = 2 Then  'tstypes, clear dsns
        '        .TextMatrix(.row, 3) = ""
        '    End If
        'End With
    End Sub

    Private Sub agdMet_RowColChange()
        'curtext = agdMet.TextMatrix(agdMet.row, agdMet.col)
        'DoLimits(agdMet)
    End Sub

    Private Sub agdMet_TextChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)
        'DoLimits(agdMet)
    End Sub

    Private Sub cboName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboName.Click

        'fill in grid from the met data details
        Dim lBaseDsn As Integer = pMetSegBaseDsns(cboName.SelectedIndex + 1)
        Dim lMetWdmId As String = pMetSegWDMIds(cboName.SelectedIndex + 1)
        With agdMet.Source
            For lRowIndex As Integer = 1 To 7
                .CellValue(lRowIndex, 1) = lMetWdmId
                Dim Id As Integer = CInt(Mid(lMetWdmId, 4, 1))
                Dim lTsFile As New atcData.atcTimeseriesSource
                Dim lTsTypeCol As New atcCollection
                If IsNumeric(Id) Then
                    lTsFile = pUCI.GetWDMObj(CInt(Id))
                    If Not lTsFile Is Nothing Then
                        lTsTypeCol = uniqueAttributeValues("TSTYPE", lTsFile)
                    End If
                End If
                Id = CInt(Mid(lMetWdmId, 4, 1))
                Dim ldsn&
                Select Case lRowIndex
                    Case 1
                        .CellValue(lRowIndex, 3) = lBaseDsn & LocFromDsn(Id, lBaseDsn)
                        .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, lBaseDsn).Attributes.GetValue("TSTYPE")
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                    Case 2
                        ldsn = lBaseDsn + 2
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            If lTsTypeCol.ItemByKey("ATEM") = "ATEM" Then
                                .CellValue(lRowIndex, 2) = "ATEM"
                            ElseIf lTsTypeCol.ItemByKey("ATMP") = "ATMP" Then
                                .CellValue(lRowIndex, 2) = "ATMP"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                    Case 3
                        ldsn = lBaseDsn + 6
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            .CellValue(lRowIndex, 2) = ""
                            If lTsTypeCol.ItemByKey("DEWP") = "DEWP" Then
                                .CellValue(lRowIndex, 2) = "DEWP"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                    Case 4
                        ldsn = lBaseDsn + 3
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            .CellValue(lRowIndex, 2) = ""
                            If lTsTypeCol.ItemByKey("WIND") = "WIND" Then
                                .CellValue(lRowIndex, 2) = "WIND"
                            ElseIf lTsTypeCol.ItemByKey("WNDH") = "WNDH" Then
                                .CellValue(lRowIndex, 2) = "WNDH"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                    Case 5
                        ldsn = lBaseDsn + 4
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            .CellValue(lRowIndex, 2) = ""
                            If lTsTypeCol.ItemByKey("SOLR") = "SOLR" Then
                                .CellValue(lRowIndex, 2) = "SOLR"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                    Case 6
                        ldsn = lBaseDsn + 7
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            .CellValue(lRowIndex, 2) = ""
                            If lTsTypeCol.ItemByKey("CLOU") = "CLOU" Then
                                .CellValue(lRowIndex, 2) = "CLOU"
                            ElseIf lTsTypeCol.ItemByKey("CLDC") = "CLDC" Then
                                .CellValue(lRowIndex, 2) = "CLDC"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 0
                        .CellValue(lRowIndex, 5) = 1
                    Case 7
                        ldsn = lBaseDsn + 5
                        If Not pUCI.GetDataSetFromDsn(Id, ldsn) Is Nothing Then
                            .CellValue(lRowIndex, 3) = ldsn & LocFromDsn(Id, ldsn)
                            .CellValue(lRowIndex, 2) = pUCI.GetDataSetFromDsn(Id, ldsn).Attributes.GetValue("TSTYPE")
                        Else
                            .CellValue(lRowIndex, 3) = 0
                            .CellValue(lRowIndex, 2) = ""
                            If lTsTypeCol.ItemByKey("PEVT") = "PEVT" Then
                                .CellValue(lRowIndex, 2) = "PEVT"
                            ElseIf lTsTypeCol.ItemByKey("EVAP") = "EVAP" Then
                                .CellValue(lRowIndex, 2) = "EVAP"
                            End If
                        End If
                        .CellValue(lRowIndex, 4) = 1
                        .CellValue(lRowIndex, 5) = 1
                End Select
                'default dsns if possible
                If Not lTsFile Is Nothing Then
                    If Len(.CellValue(lRowIndex, 2)) > 0 And .CellValue(lRowIndex, 3) = "0" Then
                        For Each lTser As atcData.atcTimeseries In lTsFile.DataSets
                            If lTser.Attributes.GetValue("TSTYPE") = .CellValue(lRowIndex, 2) Then
                                .CellValue(lRowIndex, 3) = lTser.Attributes.GetValue("ID") & LocFromDsn(Id, lTser.Attributes.GetValue("ID"))
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next lRowIndex
        End With
        pSegmentName = pUCI.GetWDMAttr(agdMet.Source.CellValue(1, 1), DsnOnly(agdMet.Source.CellValue(1, 3)), "LOC")
        agdMet.SizeAllColumnsToContents()
        agdMet.Refresh()
    End Sub

    Private Sub DoLimits(ByVal g As Object)
        'Dim i&, S$
        'Dim tstypecol As CollString
        'Dim tsfile As ATCclsTserFile
        'Dim vname As Object

        'g.ClearValues()
        'If g.col = 1 Then  'valid wdm ids
        '    If myUci.wdmcount = 1 Then
        '        g.addValue("WDM1")
        '    Else
        '        For i = 2 To myUci.wdmcount
        '            g.addValue("WDM" & CStr(i))
        '        Next i
        '    End If
        '    'For i = 1 To myUci.filesblock.Count
        '    '  If Mid(myUci.filesblock.Value(i).typ, 1, 3) = "WDM" Then
        '    '    g.AddValue myUci.filesblock.Value(i).typ
        '    '  End If
        '    'Next i
        'ElseIf g.col = 2 Then  'valid tstypes
        '    S = Mid(g.TextMatrix(g.row, 1), 4, 1)
        '    If IsNumeric(S) Then
        '        i = CInt(S)
        '        tsfile = myUci.GetWDMObj(i)
        '        If Not tsfile Is Nothing Then
        '            tstypecol = uniqueAttributeValues("TSTYPE", tsfile.DataCollection)
        '            For Each vname In tstypecol
        '                g.addValue(vname)
        '            Next
        '        End If
        '    End If
        'ElseIf g.col = 3 Then 'valid dsns
        '    S = Mid(g.TextMatrix(g.row, 1), 4, 1)
        '    If IsNumeric(S) Then
        '        i = CInt(S)
        '        tsfile = myUci.GetWDMObj(i)
        '        If Not tsfile Is Nothing Then
        '            For i = 1 To tsfile.DataCount
        '                If tsfile.Data(i).Attrib("TSTYPE") = g.TextMatrix(g.row, 2) Then
        '                    g.addValue(tsfile.Data(i).Header.Id & LocFromDsn(CInt(S), tsfile.Data(i).Header.Id))
        '                End If
        '            Next i
        '        End If
        '    End If
        'End If
    End Sub

    Private Function DsnOnly(ByVal aDsnString As String) As String
        Dim lParenPos As Integer
        DsnOnly = aDsnString
        lParenPos = InStr(1, aDsnString, "(")
        If lParenPos > 0 Then
            DsnOnly = Mid(aDsnString, 1, lParenPos - 2)
        End If
    End Function

    Private Function LocFromDsn(ByVal aId As Integer, ByVal aBaseDsn As Integer) As String
        LocFromDsn = ""
        If Not pUCI.GetDataSetFromDsn(aId, aBaseDsn) Is Nothing Then
            LocFromDsn = " (" & pUCI.GetDataSetFromDsn(aId, aBaseDsn).Attributes.GetValue("Location") & ")"
        End If
    End Function

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lRow As Integer
        If pAddEdit = 1 Then 'editing
            If Not pMetSeg Is Nothing Then
                For lRow = 1 To 7
                    pMetSeg.MetSegRecs(lRow).Source.VolName = agdMet.Source.CellValue(lRow, 1)
                    pMetSeg.MetSegRecs(lRow).Source.Member = agdMet.Source.CellValue(lRow, 2)
                    pMetSeg.MetSegRecs(lRow).Source.VolId = DsnOnly(agdMet.Source.CellValue(lRow, 3))
                    pMetSeg.MetSegRecs(lRow).MFactP = agdMet.Source.CellValue(lRow, 4)
                    pMetSeg.MetSegRecs(lRow).MFactR = agdMet.Source.CellValue(lRow, 5)
                Next lRow
                pMetSeg.ExpandMetSegName(agdMet.Source.CellValue(1, 1), DsnOnly(agdMet.Source.CellValue(1, 3)))
            End If
        ElseIf pAddEdit = 0 And Len(pSegmentName) > 0 Then 'add new met seg
            pMetSeg = New HspfMetSeg
            pMetSeg.Uci = pUCI
            With agdMet.Source
                For lRow = 1 To 7
                    If Len(.CellValue(lRow, 1)) > 0 And _
                       Len(.CellValue(lRow, 2)) > 0 And _
                       Len(.CellValue(lRow, 3)) > 0 Then
                        pMetSeg.MetSegRecs(lRow).Source.VolName = .CellValue(lRow, 1)
                        pMetSeg.MetSegRecs(lRow).Source.Member = .CellValue(lRow, 2)
                        pMetSeg.MetSegRecs(lRow).Source.VolId = DsnOnly(.CellValue(lRow, 3))
                        If Len(Trim(.CellValue(lRow, 4))) = 0 Then
                            pMetSeg.MetSegRecs(lRow).MFactP = 0.0#
                        Else
                            pMetSeg.MetSegRecs(lRow).MFactP = .CellValue(lRow, 4)
                        End If
                        If Len(Trim(.CellValue(lRow, 5))) = 0 Then
                            pMetSeg.MetSegRecs(lRow).MFactR = 0.0#
                        Else
                            pMetSeg.MetSegRecs(lRow).MFactR = .CellValue(lRow, 5)
                        End If
                        pMetSeg.MetSegRecs(lRow).Sgapstrg = ""
                        pMetSeg.MetSegRecs(lRow).Ssystem = "ENGL"
                        pMetSeg.MetSegRecs(lRow).Tran = "SAME"
                        pMetSeg.MetSegRecs(lRow).Name = ""
                        Select Case lRow
                            Case 1 : pMetSeg.MetSegRecs(lRow).Name = "PREC"
                            Case 2 : pMetSeg.MetSegRecs(lRow).Name = "ATEM"
                            Case 3 : pMetSeg.MetSegRecs(lRow).Name = "DEWP"
                            Case 4 : pMetSeg.MetSegRecs(lRow).Name = "WIND"
                            Case 5 : pMetSeg.MetSegRecs(lRow).Name = "SOLR"
                            Case 6 : pMetSeg.MetSegRecs(lRow).Name = "CLOU"
                            Case 7 : pMetSeg.MetSegRecs(lRow).Name = "PEVT"
                        End Select
                    End If
                Next lRow

                pMetSeg.ExpandMetSegName(.CellValue(1, 1), DsnOnly(.CellValue(1, 3)))
            End With

            Dim lFound As Boolean = False
            For Each lTempMetSeg As HspfMetSeg In pUCI.MetSegs
                If lTempMetSeg.Compare(pMetSeg, "PERLND") And lTempMetSeg.Compare(pMetSeg, "RCHRES") Then
                    'already exists
                    lFound = True
                End If
            Next
            If Not lFound Then
                pMetSeg.Id = pUCI.MetSegs.Count + 1
                pUCI.MetSegs.Add(pMetSeg)
            End If
        End If

        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Function uniqueAttributeValues(ByVal aAttrName As String, ByVal aTs As atcData.atcTimeseriesSource) As atcCollection
        Dim lRetval As New atcCollection
        Dim lStr As String

        For Each lts As atcData.atcTimeseries In aTs.DataSets
            lStr = lts.Attributes.GetValue(aAttrName)
            lRetval.Add(lStr, lStr)
        Next
        uniqueAttributeValues = lRetval
    End Function

End Class