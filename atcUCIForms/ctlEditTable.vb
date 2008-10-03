Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditTable
    Implements ctlEdit

    Dim pHspfTable As HspfTable
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

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
                            lOccurIndex = pHspfTable.Opn.OpnBlk.Ids(1).Tables(pHspfTable.Name & ":" & lOper).OccurIndex
                        Else
                            lOccurIndex = pHspfTable.Opn.OpnBlk.Ids(1).Tables(pHspfTable.Name).OccurIndex
                        End If
                        If lOccurIndex = 0 Then
                            lOccurIndex = lOper
                        End If
                        If lOccurIndex > 1 Then
                            lTempString = lTempString & ":" & lOccurIndex
                        End If
                        If pHspfTable.Opn.OpnBlk.Ids(1).TableExists(lTempString) Then
                            cboOccur.Items.Add(lOper & " - " & pHspfTable.Opn.OpnBlk.Ids(1).Tables(lTempString).Parms(1).Value)
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
        End Set
    End Property
    'Private Sub grdTable_RowColChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdTable.Change
    '    Dim unitfg&
    '    If grdTable.col = 0 Then
    '        txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
    '                    "Parameter: Operation Number" & vbCrLf & _
    '                    vbCrLf
    '    ElseIf grdTable.col = chkDesc Then
    '        txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
    '                    "Parameter: Description" & vbCrLf & _
    '                    vbCrLf
    '    Else
    '        txtDefine = "Table: " & pTable.Name & ", " & pTable.Def.Define & vbCrLf & _
    '                    "Parameter: " & pTable.Parms(grdTable.col - chkDesc).Def.Define & vbCrLf & _
    '                    vbCrLf
    '    End If
    '    unitfg = pTable.Opn.OpnBlk.Uci.GlobalBlock.emfg
    '    If unitfg = 1 Then 'english
    '        txtDefine = txtDefine & pTable.Def.HeaderE
    '    ElseIf unitfg = 2 Then 'metric
    '        txtDefine = txtDefine & pTable.Def.HeaderM
    '    End If
    'End Sub

    Private Sub refreshGrid()
        Dim lParm As HspfParm
        Dim ltable As HspfTable = Nothing
        Dim tname As String = Nothing
        Dim unitfg As String = Nothing
        Dim more, skip As Boolean
        Dim i, j, lchkDescInteger As Integer
        Dim lStartEditCol, lCol As Integer

        With grdTable.Source
            .Columns = pHspfTable.Parms.Count + 1
            .CellValue(0, 0) = "OpNum"
            If chkDesc.Checked = True Then
                .CellValue(0, 1) = "Description"
                lchkDescInteger = 1
            Else
                .Columns += 1
                lchkDescInteger = 0
            End If

            For i = 0 To pHspfTable.Parms.Count - 1
                lParm = pHspfTable.Parms(i)
                .CellValue(0, i + lchkDescInteger + 1) = lParm.Name
                'The below code is commented because the .ColType, .ColMax, .ColMin was not defined yet.
                'If lParm.Def.Typ = 2 Then
                '    '.ColType(i + lchkDescInteger) = ATCoSng  'causes formatting problems
                'End If
                'If lParm.Def.Typ = 1 Then
                '    '.ColType(i + lchkDescInteger) = ATCoInt0
                'End If
                'If lParm.Def.Typ = 0 Then
                '    '.ColType(i + lchkDescInteger) = ATCoTxt
                'End If
                'unitfg = pHspfTable.Opn.OpnBlk.Uci.GlobalBlock.EmFg
                'If unitfg = 1 Then 'english
                '    '.ColMax(i + lchkDescInteger) = lParm.Def.Max
                '    '.ColMin(i + lchkDescInteger) = lParm.Def.Min
                'ElseIf unitfg = 2 Then 'metric
                '    '.ColMax(i + lchkDescInteger) = lParm.Def.MetricMax
                '    '.ColMin(i + lchkDescInteger) = lParm.Def.MetricMin
                'End If
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

                .Rows = .Rows + 1
                If skip = False Then
                    .CellValue(.Rows - 1, 0) = ltable.Opn.Id
                    If chkDesc.Checked = True Then
                        .CellValue(.Rows - 1, 1) = ltable.Opn.Description
                    End If
                    For i = 0 To pHspfTable.Parms.Count - 1
                        .CellValue(.Rows - 1, i + lchkDescInteger + 1) = ltable.Parms(i).Value
                    Next i
                End If
            Loop

            If chkDesc.Checked = True Then
                lStartEditCol = 2
            Else
                lStartEditCol = 1
            End If

            For lCol = lStartEditCol To .Columns - 1
                For lRow As Integer = 1 To .Rows - 1
                    .CellEditable(lRow, lCol) = True
                Next
            Next

            For lCol = 0 To .Columns - 1
                .CellColor(0, lCol) = SystemColors.ControlLight
            Next

        End With

        grdTable.SizeAllColumnsToContents(grdTable.Width, True)
        grdTable.Refresh()

    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove

    End Sub

    Public Sub Save() Implements ctlEdit.Save
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
End Class
