Imports atcUCI
Imports MapWinUtility
Imports atcUtility

Public Class frmAddMet

    Dim pSegmentName As String
    Dim pMetSeg As HspfMetSeg
    Dim pAddEdit As Integer

    Dim pMetSegNames As New Collection
    Dim pMetSegBaseDsns As New Collection
    Dim pMetSegWDMIds As New Collection
    Dim pMetSegDescs As New Collection

    Dim pSelectedRow As Integer
    Dim pSelectedColumn As Integer

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
            For lRow As Integer = 1 To 7
                .CellValue(lRow, 1) = ""
                .CellValue(lRow, 2) = ""
                .CellValue(lRow, 3) = ""
                .CellValue(lRow, 4) = ""
                .CellValue(lRow, 5) = ""
            Next

            If pAddEdit = 1 Then
                pMetSeg = Nothing
                For Each lTempMetSeg As HspfMetSeg In pUCI.MetSegs  'find which met seg this is
                    If lTempMetSeg.Name = pSegmentName Then
                        pMetSeg = lTempMetSeg
                    End If
                Next
                If Not pMetSeg Is Nothing Then
                    For Each lMetSegRec As HspfMetSegRecord In pMetSeg.MetSegRecs
                        Dim lRow As Integer = 0
                        Select Case lMetSegRec.Name
                            Case "PREC" : lRow = 1
                            Case "ATEM" : lRow = 2
                            Case "DEWP" : lRow = 3
                            Case "WIND" : lRow = 4
                            Case "SOLR" : lRow = 5
                            Case "CLOU" : lRow = 6
                            Case "PEVT" : lRow = 7
                        End Select
                        Dim lId As Integer
                        If Len(lMetSegRec.Source.VolName) < 4 Then
                            lId = 1
                        Else
                            lId = CInt(Mid(lMetSegRec.Source.VolName, 4, 1))
                        End If
                        Dim ldsn As Integer = lMetSegRec.Source.VolId
                        .CellValue(lRow, 1) = lMetSegRec.Source.VolName
                        .CellValue(lRow, 2) = lMetSegRec.Source.Member
                        .CellValue(lRow, 3) = ldsn & LocFromDsn(lId, ldsn)
                        .CellValue(lRow, 4) = lMetSegRec.MFactP
                        .CellValue(lRow, 5) = lMetSegRec.MFactR
                    Next
                End If
            End If
            For lRow As Integer = 1 To 7
                .CellEditable(lRow, 1) = True
                .CellEditable(lRow, 2) = True
                .CellEditable(lRow, 3) = True
                .CellEditable(lRow, 4) = True
                .CellEditable(lRow, 5) = True
            Next
        End With

        agdMet.SizeAllColumnsToContents()
        agdMet.Refresh()

        If cboName.Items.Count > 0 Then
            cboName.SelectedIndex = 0
        End If
    End Sub

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Private Sub cboName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboName.SelectedIndexChanged

        'fill in grid from the met data details
        If Not agdMet.Source Is Nothing Then
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
        End If
    End Sub

    Private Sub DoLimits()
        
        With agdMet
            Dim lValidValues As New Collection
            If pSelectedColumn = 1 Then 'valid wdm ids
                Dim lFiles As HspfFilesBlk = pUCI.FilesBlock
                For lIndex As Integer = 1 To pUCI.FilesBlock.Count
                    Dim lFile As HspfFile = pUCI.FilesBlock.Value(lIndex)
                    If lFile.Typ.Length > 2 Then
                        If lFile.Typ.Substring(0, 3) = "WDM" Then
                            lValidValues.Add(lFile.Typ)
                        End If
                    End If
                Next
            ElseIf pSelectedColumn = 2 Then  'valid tstypes
                Dim lWDMid As String = .Source.CellValue(pSelectedRow, pSelectedColumn - 1)
                If Not lWDMid Is Nothing AndAlso lWDMid.Length > 0 Then
                    Dim lId As Integer
                    If lWDMid.Length > 3 Then
                        lId = CInt(Mid(lWDMid, 4, 1))
                    Else
                        lId = 1
                    End If
                    Dim lTsFile As New atcData.atcTimeseriesSource
                    Dim lTsTypeCol As New atcCollection
                    If IsNumeric(lId) Then
                        lTsFile = pUCI.GetWDMObj(CInt(lId))
                        If Not lTsFile Is Nothing Then
                            lTsTypeCol = uniqueAttributeValues("TSTYPE", lTsFile)
                        End If
                    End If
                    For Each lString As String In lTsTypeCol
                        lValidValues.Add(lString)
                    Next
                End If
            ElseIf pSelectedColumn = 3 Then 'valid dsns
                Dim lWDMid As String = .Source.CellValue(pSelectedRow, pSelectedColumn - 2)
                Dim lId As Integer
                If IsNumeric(Mid(lWDMid, 4, 1)) Then
                    lId = CInt(Mid(lWDMid, 4, 1))
                Else
                    lId = 1
                End If
                Dim lTsFile As New atcData.atcTimeseriesSource
                lTsFile = pUCI.GetWDMObj(CInt(lId))
                If Not lTsFile Is Nothing Then
                    For Each lts As atcData.atcTimeseries In lTsFile.DataSets
                        If lts.Attributes.GetValue("TSTYPE") = .Source.CellValue(pSelectedRow, pSelectedColumn - 1) Then
                            Dim lDsn As String = lts.Attributes.GetValue("ID")
                            lValidValues.Add(lDsn & LocFromDsn(lId, CInt(lDsn)))
                        End If
                    Next
                End If
            End If

            .ValidValues = lValidValues
            .AllowNewValidValues = False
            .Refresh()
        End With
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
                    If AllFieldsPopulated(lRow) Then
                        'every field is populated
                        Dim lName As String = ""
                        Select Case lRow
                            Case 1 : lName = "PREC"
                            Case 2 : lName = "ATEM"
                            Case 3 : lName = "DEWP"
                            Case 4 : lName = "WIND"
                            Case 5 : lName = "SOLR"
                            Case 6 : lName = "CLOU"
                            Case 7 : lName = "PEVT"
                        End Select
                        'does this type of record exist in the metsegrecs?
                        Dim lFoundIndex As Integer = -1
                        For lIndex As Integer = 0 To pMetSeg.MetSegRecs.Count - 1
                            If pMetSeg.MetSegRecs(lIndex).Name = lName Then
                                lFoundIndex = lIndex
                            End If
                        Next
                        If lFoundIndex > -1 Then
                            'already exists, just overwrite the properties
                            pMetSeg.MetSegRecs(lFoundIndex).Source.VolName = agdMet.Source.CellValue(lRow, 1)
                            pMetSeg.MetSegRecs(lFoundIndex).Source.Member = agdMet.Source.CellValue(lRow, 2)
                            pMetSeg.MetSegRecs(lFoundIndex).Source.VolId = DsnOnly(agdMet.Source.CellValue(lRow, 3))
                            pMetSeg.MetSegRecs(lFoundIndex).MFactP = agdMet.Source.CellValue(lRow, 4)
                            pMetSeg.MetSegRecs(lFoundIndex).MFactR = agdMet.Source.CellValue(lRow, 5)
                        Else
                            'does not exist yet, add it now
                            Dim lMetSegRec As New HspfMetSegRecord
                            lMetSegRec.Source.VolName = agdMet.Source.CellValue(lRow, 1)
                            lMetSegRec.Source.Member = agdMet.Source.CellValue(lRow, 2)
                            lMetSegRec.Source.VolId = DsnOnly(agdMet.Source.CellValue(lRow, 3))
                            lMetSegRec.MFactP = agdMet.Source.CellValue(lRow, 4)
                            lMetSegRec.MFactR = agdMet.Source.CellValue(lRow, 5)
                            lMetSegRec.Name = lName
                            pMetSeg.MetSegRecs.Add(lMetSegRec)
                        End If
                    End If
                Next lRow
                pMetSeg.ExpandMetSegName(agdMet.Source.CellValue(1, 1), DsnOnly(agdMet.Source.CellValue(1, 3)))
            End If
        ElseIf pAddEdit = 0 And Len(pSegmentName) > 0 Then 'add new met seg
            pMetSeg = New HspfMetSeg
            pMetSeg.Uci = pUCI
            With agdMet.Source
                For lRow = 1 To 7
                    If AllFieldsPopulated(lRow) Then
                        'every field is populated
                        Dim lMetSegRec As New HspfMetSegRecord
                        lMetSegRec.Source.VolName = .CellValue(lRow, 1)
                        lMetSegRec.Source.Member = .CellValue(lRow, 2)
                        lMetSegRec.Source.VolId = DsnOnly(.CellValue(lRow, 3))
                        If Len(Trim(.CellValue(lRow, 4))) = 0 Then
                            lMetSegRec.MFactP = 0.0#
                        Else
                            lMetSegRec.MFactP = .CellValue(lRow, 4)
                        End If
                        If Len(Trim(.CellValue(lRow, 5))) = 0 Then
                            lMetSegRec.MFactR = 0.0#
                        Else
                            lMetSegRec.MFactR = .CellValue(lRow, 5)
                        End If
                        lMetSegRec.Sgapstrg = ""
                        lMetSegRec.Ssystem = "ENGL"
                        lMetSegRec.Tran = "SAME"
                        lMetSegRec.Name = ""
                        Select Case lRow
                            Case 1 : lMetSegRec.Name = "PREC"
                            Case 2 : lMetSegRec.Name = "ATEM"
                            Case 3 : lMetSegRec.Name = "DEWP"
                            Case 4 : lMetSegRec.Name = "WIND"
                            Case 5 : lMetSegRec.Name = "SOLR"
                            Case 6 : lMetSegRec.Name = "CLOU"
                            Case 7 : lMetSegRec.Name = "PEVT"
                        End Select
                        pMetSeg.MetSegRecs.Add(lMetSegRec)
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

    Private Function AllFieldsPopulated(ByVal aRow As Integer) As Boolean
        Dim lReturnVal As Boolean = True
        For lCol As Integer = 1 To 5
            If agdMet.Source.CellValue(aRow, lCol) Is Nothing Then
                lReturnVal = False
            ElseIf agdMet.Source.CellValue(aRow, lCol).Length = 0 Then
                lReturnVal = False
            End If
        Next
        Return lReturnVal
    End Function

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

    Private Sub agdMet_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMet.CellEdited

        DoLimits()
        'If curtext <> agdMet.TextMatrix(agdMet.row, agdMet.col) Then
        If pSelectedRow = 1 And (pSelectedColumn = 1 Or pSelectedColumn = 2 Or pSelectedColumn = 3) And _
           pAddEdit = 1 Then
            'cant change these, set it back
            Logger.Msg("The Precip Data Set defines the met segment and thus cannot be changed." & vbCrLf & _
                   "Use 'Add' to add a new met segment.", MsgBoxStyle.OkOnly, "Edit Met Segment Problem")
            'agdMet.TextMatrix(pSelectedRow, pSelectedColumn) = curtext
        Else
            ClearRestofRow()
        End If
        'End If
        If pAddEdit = 0 Then
            'adding, check for precip data set
            If pSelectedRow = 1 And (pSelectedColumn = 1 Or pSelectedColumn = 2 Or pSelectedColumn = 3) Then
                With agdMet.Source
                    If Len(Trim(.CellValue(1, 1))) > 0 And _
                       Len(Trim(.CellValue(1, 2))) > 0 And _
                       Len(Trim(.CellValue(1, 3))) > 0 Then
                        lblName.Text = pUCI.GetWDMAttr(.CellValue(1, 1), DsnOnly(.CellValue(1, 3)), "LOC")
                        pSegmentName = lblName.Text
                    End If
                End With
            End If
        End If

        If aColumn = 4 Or aColumn = 5 Then
            'handle limits for mfactrs
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)

            'value should be greater than zero
            If lNewValueNumeric > 0 Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If

            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If

    End Sub

    Private Sub ClearRestofRow()
        With agdMet.Source
            If pSelectedColumn = 1 Then  'wdm ids, clear tstypes and dsns
                .CellValue(pSelectedRow, 2) = ""
                .CellValue(pSelectedRow, 3) = ""
            ElseIf pSelectedColumn = 2 Then  'tstypes, clear dsns
                .CellValue(pSelectedRow, 3) = ""
            End If
        End With
    End Sub

    Private Sub agdMet_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdMet.MouseDownCell
        pSelectedColumn = aColumn
        pSelectedRow = aRow

        DoLimits()
    End Sub

    Private Sub frmAddMet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Simulation Time.html")
        End If
    End Sub
End Class