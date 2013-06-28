Imports atcUCI
Imports atcControls
Imports System.Drawing

Public Class ctlEditTable
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pHspfTable As HspfTable
    Dim pChanged As Boolean
    Dim pCurrentSelectedColumn As Integer
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Private Sub grdTable_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdTable.Resize
        grdTable.SizeAllColumnsToContents(grdTable.Width - pVScrollColumnOffset, True)
    End Sub

    Private Sub grdTableCellEdited(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdTable.CellEdited
        pChanged = True
        RaiseEvent Change(True)

        Dim lchkDescInteger As Integer
        If chkDesc.Checked = True Then
            lchkDescInteger = 1
        Else
            lchkDescInteger = 0
        End If
        Dim lUnitfg As Integer = pHspfTable.Opn.OpnBlk.Uci.GlobalBlock.EmFg

        Dim lMinValue As Single = -999
        Dim lMaxValue As Single = -999

        Dim lParm As HspfParm = pHspfTable.Parms(aColumn - lchkDescInteger - 1)

        If lParm.Def.Typ = 1 Or lParm.Def.Typ = 2 Then
            'this is a numeric field
            If lUnitfg = 1 Then 'english
                lMaxValue = lParm.Def.Max
                lMinValue = lParm.Def.Min
            ElseIf lUnitfg = 2 Then 'metric
                lMaxValue = lParm.Def.MetricMax
                lMinValue = lParm.Def.MetricMin
            End If

            If lMaxValue <> -999 Or lMinValue <> -999 Then
                Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
                Dim lNewValueNumeric As Double = -999
                If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
                Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
                If ((lNewValueNumeric >= lMinValue And lMinValue <> -999) Or lMinValue = -999) AndAlso _
                   ((lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Or lMaxValue = -999) Then
                    lNewColor = aGrid.CellBackColor
                Else
                    lNewColor = Color.Pink
                End If
                If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                    aGrid.Source.CellColor(aRow, aColumn) = lNewColor
                End If
            End If
        End If
    End Sub

    Private Sub grdTableClick(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdTable.MouseDownCell
        Dim lchkDescCheckedInteger As Integer
        Dim lUnitfg As Integer
        pCurrentSelectedColumn = aColumn

        If chkDesc.Checked = True Then
            lchkDescCheckedInteger = 1
        Else
            lchkDescCheckedInteger = 0
        End If

        If pCurrentSelectedColumn = 0 Then
            txtDefine.Text = "Table: " & pHspfTable.Name & ", " & pHspfTable.Def.Define & vbCrLf & "Parameter: Operation Number" & vbCrLf & vbCrLf
        ElseIf pCurrentSelectedColumn = lchkDescCheckedInteger Then
            txtDefine.Text = "Table: " & pHspfTable.Name & ", " & pHspfTable.Def.Define & vbCrLf & "Parameter: Description" & vbCrLf & vbCrLf
        Else
            txtDefine.Text = "Table: " & pHspfTable.Name & ", " & pHspfTable.Def.Define & vbCrLf & "Parameter: " & pHspfTable.Parms(pCurrentSelectedColumn - lchkDescCheckedInteger - 1).Def.Define & vbCrLf & vbCrLf
        End If
        lUnitfg = pHspfTable.Opn.OpnBlk.Uci.GlobalBlock.EmFg
        If lUnitfg = 1 Then 'english
            txtDefine.Text = txtDefine.Text & pHspfTable.Def.HeaderE
        ElseIf lUnitfg = 2 Then 'metric
            txtDefine.Text = txtDefine.Text & pHspfTable.Def.HeaderM
        End If

    End Sub

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Edit Table " & pHspfTable.Caption
        End Get
    End Property

    Public Property Changed() As Boolean Implements ctlEdit.Changed
        Get
            Return pChanged
        End Get
        Set(ByVal aChanged As Boolean)
            If aChanged <> pChanged Then
                pChanged = aChanged
                RaiseEvent Change(aChanged)
            End If
        End Set
    End Property

    Public Sub Add() Implements ctlEdit.Add
        pChanged = True
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfTable
        End Get

        Set(ByVal aHspfTable As Object)
            Dim lOper, lOccurIndex As Integer
            Dim lTempString As String

            With grdTable
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowNewValidValues = True
                .AllowHorizontalScrolling = False
                .Visible = True
                .Source.FixedRows = 1
            End With

            pHspfTable = aHspfTable

            If pHspfTable.Def.NumOccur > 1 Then
                cboOccur.Items.Clear()

                For lOper = 1 To pHspfTable.OccurCount 'how about later ones?
                    If pHspfTable.Def.OccurGroup > 0 Then
                        'this is part of an occurance group, add name of occurance to combo box
                        lTempString = ""
                        If pHspfTable.Opn.Name = "PERLND" Or pHspfTable.Opn.Name = "IMPLND" Then
                            lTempString = "QUAL-PROPS"
                        ElseIf pHspfTable.Opn.Name = "RCHRES" Then
                            lTempString = "GQ-QALDATA"
                        End If
                        If lOper > 1 Then
                            lOccurIndex = pHspfTable.Opn.OpnBlk.Ids(0).Tables(pHspfTable.Name & ":" & lOper).OccurIndex
                        Else
                            lOccurIndex = pHspfTable.Opn.OpnBlk.Ids(0).Tables(pHspfTable.Name).OccurIndex
                        End If
                        If lOccurIndex = 0 Then
                            lOccurIndex = lOper
                        End If
                        If lOccurIndex > 1 Then
                            lTempString = lTempString & ":" & lOccurIndex
                        End If
                        If pHspfTable.Opn.OpnBlk.Ids(0).TableExists(lTempString) Then
                            cboOccur.Items.Add(lOper & " - " & pHspfTable.Opn.OpnBlk.Ids(0).Tables(lTempString).Parms(0).Value)
                        Else
                            cboOccur.Items.Add(lOper)
                        End If
                    Else
                        cboOccur.Items.Add(lOper)
                    End If
                Next
                cboOccur.SelectedIndex = pHspfTable.OccurNum - 1
                lblOccur.Visible = True
                cboOccur.Visible = True
            Else
                lblOccur.Visible = False
                cboOccur.Visible = False
            End If

            If Len(pHspfTable.Opn.Description) > 0 And pHspfTable.Name <> "GEN-INFO" Then
                chkDesc.Checked = True
                refreshGrid()
            Else
                chkDesc.Checked = False
                chkDesc.Visible = False
                refreshGrid()
            End If

            txtDefine.Text = "Table: " & pHspfTable.Name & ", " & pHspfTable.Def.Define & vbCrLf & vbCrLf & vbCrLf
            Dim lUnitfg As Integer = pHspfTable.Opn.OpnBlk.Uci.GlobalBlock.EmFg
            If lUnitfg = 1 Then 'english
                txtDefine.Text = txtDefine.Text & pHspfTable.Def.HeaderE
            ElseIf lUnitfg = 2 Then 'metric
                txtDefine.Text = txtDefine.Text & pHspfTable.Def.HeaderM
            End If
        End Set
    End Property

    Private Sub RefreshGrid()
        Dim lParm As HspfParm
        Dim ltable As HspfTable = Nothing
        Dim tname As String = Nothing
        Dim unitfg As String = Nothing
        Dim more, skip As Boolean
        Dim i, j, lchkDescInteger As Integer
        Dim lStartEditCol, lCol As Integer
        Dim lParmIndex As Integer = -1

        If pHspfTable.Name = "CONDUIT-PARM" Then
            'special case 
            With grdTable.Source
                .Columns = 8
                .CellValue(0, 0) = "OpNum"
                If chkDesc.Checked = True Then
                    .CellValue(0, 1) = "Description"
                    lchkDescInteger = 1
                Else
                    lchkDescInteger = 0
                End If
                For i = 0 To 6
                    lParm = pHspfTable.Parms(i)
                    .CellValue(0, i + lchkDescInteger + 1) = lParm.Name
                Next i
                tname = pHspfTable.Name
                ltable = pHspfTable
                Dim nRows As Integer = 100
                If ltable.Opn.TableExists("DYNAMIC-WAVE") Then
                    nRows = ltable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NCOND")
                End If
                .Rows = 1
                For j = 1 To nRows
                    .Rows = .Rows + 1
                    .CellValue(.Rows - 1, 0) = ltable.Opn.Id
                    If chkDesc.Checked = True Then
                        .CellValue(.Rows - 1, 1) = ltable.Opn.Description
                    End If
                    For i = 0 To 6
                        lParmIndex += 1
                        .CellValue(.Rows - 1, i + lchkDescInteger + 1) = ltable.Parms(lParmIndex).Value
                    Next i
                Next j
            End With
        ElseIf pHspfTable.Name = "CONDUIT-XS" Then
            'special case 
            With grdTable.Source
                .Columns = 5
                .CellValue(0, 0) = "OpNum"
                If chkDesc.Checked = True Then
                    .CellValue(0, 1) = "Description"
                    lchkDescInteger = 1
                Else
                    lchkDescInteger = 0
                End If
                For i = 0 To 4
                    lParm = pHspfTable.Parms(i)
                    .CellValue(0, i + lchkDescInteger + 1) = lParm.Name
                Next i
                tname = pHspfTable.Name
                ltable = pHspfTable
                Dim nRows As Integer = 100
                If ltable.Opn.TableExists("DYNAMIC-WAVE") Then
                    nRows = ltable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NCOND")
                End If
                .Rows = 1
                For j = 1 To nRows
                    .Rows = .Rows + 1
                    .CellValue(.Rows - 1, 0) = ltable.Opn.Id
                    If chkDesc.Checked = True Then
                        .CellValue(.Rows - 1, 1) = ltable.Opn.Description
                    End If
                    For i = 0 To 4
                        lParmIndex += 1
                        .CellValue(.Rows - 1, i + lchkDescInteger + 1) = ltable.Parms(lParmIndex).Value
                    Next i
                Next j
            End With
        ElseIf pHspfTable.Name = "NODE-PARM" Then
            'special case 
            With grdTable.Source
                .Columns = 6
                .CellValue(0, 0) = "OpNum"
                If chkDesc.Checked = True Then
                    .CellValue(0, 1) = "Description"
                    lchkDescInteger = 1
                Else
                    lchkDescInteger = 0
                End If
                For i = 0 To 5
                    lParm = pHspfTable.Parms(i)
                    .CellValue(0, i + lchkDescInteger + 1) = lParm.Name
                Next i
                tname = pHspfTable.Name
                ltable = pHspfTable
                Dim nRows As Integer = 100
                If ltable.Opn.TableExists("DYNAMIC-WAVE") Then
                    nRows = ltable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NNODE")
                End If
                .Rows = 1
                For j = 1 To nRows
                    .Rows = .Rows + 1
                    .CellValue(.Rows - 1, 0) = ltable.Opn.Id
                    If chkDesc.Checked = True Then
                        .CellValue(.Rows - 1, 1) = ltable.Opn.Description
                    End If
                    For i = 0 To 5
                        lParmIndex += 1
                        .CellValue(.Rows - 1, i + lchkDescInteger + 1) = ltable.Parms(lParmIndex).Value
                    Next i
                Next j
            End With
        Else
            'normal case
            With grdTable.Source
                .Columns = pHspfTable.Parms.Count + 1
                .CellValue(0, 0) = "OpNum"
                If chkDesc.Checked = True Then
                    .CellValue(0, 1) = "Description"
                    lchkDescInteger = 1
                Else
                    lchkDescInteger = 0
                End If

                For i = 0 To pHspfTable.Parms.Count - 1
                    lParm = pHspfTable.Parms(i)
                    .CellValue(0, i + lchkDescInteger + 1) = lParm.Name
                Next i

                'may need index here
                tname = pHspfTable.Name
                If pHspfTable.OccurCount > 1 Then
                    If cboOccur.SelectedIndex > 0 Then
                        tname = tname & ":" & cboOccur.SelectedIndex + 1
                    End If
                End If

                more = True
                .Rows = 1
                j = 1
                Do While more = True
                    If pHspfTable.EditAllSimilar = True Then
                        If pHspfTable.Opn.OpnBlk.NthOper(j).TableExists(tname) Then
                            ltable = pHspfTable.Opn.OpnBlk.NthOper(j).Tables(tname)
                            skip = False
                        Else
                            skip = True
                        End If
                        j = j + 1
                        If j > pHspfTable.Opn.OpnBlk.Ids.Count Then
                            more = False
                        End If
                    ElseIf pHspfTable.OccurCount > 1 Then
                        ltable = pHspfTable.Opn.OpnBlk.OperFromID(pHspfTable.Opn.Id).Tables(tname)
                        skip = False
                        more = False
                    Else
                        ltable = pHspfTable
                        skip = False
                        more = False
                    End If

                    If skip = False Then
                        .Rows = .Rows + 1
                        .CellValue(.Rows - 1, 0) = ltable.Opn.Id
                        If chkDesc.Checked = True Then
                            .CellValue(.Rows - 1, 1) = ltable.Opn.Description
                        End If
                        For i = 0 To pHspfTable.Parms.Count - 1
                            .CellValue(.Rows - 1, i + lchkDescInteger + 1) = ltable.Parms(i).Value
                        Next i
                    End If
                Loop

            End With
        End If

        If chkDesc.Checked = True Then
            lStartEditCol = 2
        Else
            lStartEditCol = 1
        End If

        For lCol = lStartEditCol To grdTable.Source.Columns - 1
            For lRow As Integer = 1 To grdTable.Source.Rows - 1
                grdTable.Source.CellEditable(lRow, lCol) = True
            Next
        Next

        grdTable.SizeAllColumnsToContents(grdTable.Width, True)
        grdTable.Refresh()

    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove

    End Sub

    Public Sub Save() Implements ctlEdit.Save
        Dim lRow, lCol As Integer
        Dim lParm As HspfParm
        Dim lTable As HspfTable
        Dim lTname As String
        Dim lnRows As Integer = 0
        Dim lParmIndex As Integer = -1

        lTname = pHspfTable.Name
        If pHspfTable.OccurCount > 1 Then
            If cboOccur.SelectedIndex > 0 Then
                lTname = lTname & ":" & cboOccur.SelectedIndex + 1
            End If
        End If

        With grdTable.Source
            If pHspfTable.Name = "CONDUIT-PARM" Then
                'special case 
                lRow = 1
                lTable = pHspfTable
                lnRows = 100
                If lTable.Opn.TableExists("DYNAMIC-WAVE") Then
                    lnRows = lTable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NCOND")
                End If
                For lRow = 1 To lnRows
                    For lCol = 0 To 6
                        lParmIndex += 1
                        lParm = lTable.Parms(lParmIndex)
                        lParm.Value = .CellValue(lRow, lCol + chkDesc.CheckState + 1)
                    Next
                Next
            ElseIf pHspfTable.Name = "CONDUIT-XS" Then
                'special case 
                lRow = 1
                lTable = pHspfTable
                lnRows = 100
                If lTable.Opn.TableExists("DYNAMIC-WAVE") Then
                    lnRows = lTable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NCOND")
                End If
                For lRow = 1 To lnRows
                    For lCol = 0 To 4
                        lParmIndex += 1
                        lParm = lTable.Parms(lParmIndex)
                        lParm.Value = .CellValue(lRow, lCol + chkDesc.CheckState + 1)
                    Next
                Next
            ElseIf pHspfTable.Name = "NODE-PARM" Then
                'special case 
                lRow = 1
                lTable = pHspfTable
                lnRows = 100
                If lTable.Opn.TableExists("DYNAMIC-WAVE") Then
                    lnRows = lTable.Opn.Tables("DYNAMIC-WAVE").ParmValue("NNODE")
                End If
                For lRow = 1 To lnRows
                    For lCol = 0 To 5
                        lParmIndex += 1
                        lParm = lTable.Parms(lParmIndex)
                        lParm.Value = .CellValue(lRow, lCol + chkDesc.CheckState + 1)
                    Next
                Next
            Else
                'normal case
                For lRow = 1 To .Rows - 1
                    If .Rows = 1 Then
                        lTable = Nothing
                        lTable = pHspfTable.Opn.Tables(lTname)
                    Else
                        'Set ltable = pTable.Opn.OpnBlk.Ids(j).Tables(tname) 'changed for sort
                        lTable = Nothing
                        If Len(.CellValue(lRow, 0)) > 0 Then
                            If Not pHspfTable.Opn.OpnBlk.OperFromID(.CellValue(lRow, 0)) Is Nothing Then
                                'make sure there is an operation by this number
                                lTable = pHspfTable.Opn.OpnBlk.OperFromID(.CellValue(lRow, 0)).Tables(lTname)
                            End If
                        End If
                    End If
                    If Not lTable Is Nothing Then
                        For lCol = 0 To lTable.Parms.Count - 1
                            lParm = lTable.Parms(lCol)
                            lParm.Value = .CellValue(lRow, lCol + chkDesc.CheckState + 1)
                            If lTable.Name = "GEN-INFO" And lCol = 1 Then
                                lTable.Opn.Description = lParm.Value
                            End If
                        Next
                    End If
                    lTable.Edited = True
                Next
            End If
        End With

        pChanged = False
    End Sub

    Public Sub New(ByVal aHspfTable As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfTable
    End Sub

    Private Sub chkDesc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesc.CheckedChanged
        refreshGrid()
    End Sub

    Private Sub grdTable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdTable.Resize
        grdTable.SizeAllColumnsToContents(grdTable.Width, True)
    End Sub

    Private Sub cboOccur_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboOccur.SelectedIndexChanged
        RefreshGrid()
    End Sub
End Class
