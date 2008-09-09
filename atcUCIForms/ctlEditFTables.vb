Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms

Public Class ctlEditFTables
    Implements ctlEdit

    Dim pHspfFtable As HspfFtable
    Dim pChanged As Boolean
    Private PrevListIndex As Long
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "FTables Block"
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
        Changed = True
    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'not needed
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        Dim lRow, lCol As Integer

        pHspfFtable.Nrows = txtNRows.Value
        pHspfFtable.Ncols = txtNCols.Value
        With grdEdit.Source
            For lRow = 1 To .Rows - 1
                pHspfFtable.Depth(lRow) = .CellValue(lRow, 0)
                pHspfFtable.Area(lRow) = .CellValue(lRow, 1)
                pHspfFtable.Volume(lRow) = .CellValue(lRow, 2)
                For lCol = 3 To .Columns - 1
                    If lCol = 3 Then
                        pHspfFtable.Outflow1(lRow) = .CellValue(lRow, lCol)
                    End If
                    If lCol = 4 Then
                        pHspfFtable.Outflow2(lRow) = .CellValue(lRow, lCol)
                    End If
                    If lCol = 5 Then
                        pHspfFtable.Outflow3(lRow) = .CellValue(lRow, lCol)
                    End If
                    If lCol = 6 Then
                        pHspfFtable.Outflow4(lRow) = .CellValue(lRow, lCol)
                    End If
                    If lCol = 7 Then
                        pHspfFtable.Outflow5(lRow) = .CellValue(lRow, lCol)
                    End If
                Next
            Next
        End With
        pHspfFtable.Edited()
        pChanged = False
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfFtable
        End Get

        Set(ByVal aHspfFtables As Object)
            pHspfFtable = aHspfFtables
            Dim lOper As HspfOperation
            For Each lOper In pHspfFtable.Operation.OpnBlk.Ids
                cboID.Items.Add(lOper.Id & " - " & lOper.Description)
                If pHspfFtable.Operation.Tables("HYDR-PARM2").ParmValue("FTBUCI") = cboID.Items.Count Then
                    cboID.SelectedIndex = 0
                    PrevListIndex = cboID.SelectedIndex
                End If
            Next
            txtNRows.Value = pHspfFtable.Nrows
            txtNCols.Value = pHspfFtable.Ncols
            RefreshFtables()
        End Set


    End Property

    Public Sub RefreshFtables()
        Dim lRow, lCol, units As Integer
        units = pHspfFtable.Operation.OpnBlk.Uci.GlobalBlock.EmFg

        With grdEdit
            .Clear()
            .AllowHorizontalScrolling = True
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdEdit.Source
            .Rows = txtNRows.Value + 1
            .Columns = txtNCols.Value
            For lCol = 0 To .Columns - 1
                .CellEditable(0, lCol) = False
            Next
            If units = 1 Then
                .CellValue(0, 0) = "Depth (ft)"
                .CellValue(0, 1) = "Area (acres)"
                .CellValue(0, 2) = "Volume (acre-ft)"
                For lCol = 3 To .Columns - 1
                    .CellValue(0, lCol) = "Outflow" & lCol - 2 & "(ft3/s)"
                Next
            Else 'metric
                .CellValue(0, 0) = "Depth (m)"
                .CellValue(0, 1) = "Area (ha)"
                .CellValue(0, 2) = "Volume (Mm3)"
                For lCol = 3 To .Columns - 1
                    .CellValue(0, lCol) = "Outflow" & lCol - 2 & "(m3/s)"
                Next
            End If

            For m As Integer = 0 To .Columns - 1
                For k As Integer = 1 To .Rows - 1
                    .CellEditable(k, m) = True
                Next
            Next

            For lCol2 As Integer = 0 To .Columns - 1
                .CellColor(0, lCol2) = SystemColors.ControlLight
            Next

            For lRow = 1 To .Rows - 1
                .CellValue(lRow, 0) = pHspfFtable.Depth(lRow)
                .CellValue(lRow, 1) = pHspfFtable.Area(lRow)
                .CellValue(lRow, 2) = pHspfFtable.Volume(lRow)
                For lCol = 3 To .Columns - 1
                    If lCol = 3 Then
                        .CellValue(lRow, lCol) = pHspfFtable.Outflow1(lRow)
                    End If
                    If lCol = 4 Then
                        .CellValue(lRow, lCol) = pHspfFtable.Outflow2(lRow)
                    End If
                    If lCol = 5 Then
                        .CellValue(lRow, lCol) = pHspfFtable.Outflow3(lRow)
                    End If
                    If lCol = 6 Then
                        .CellValue(lRow, lCol) = pHspfFtable.Outflow4(lRow)
                    End If
                    If lCol = 7 Then
                        .CellValue(lRow, lCol) = pHspfFtable.Outflow5(lRow)
                    End If

                Next
            Next
        End With
        pChanged = False
        grdEdit.Refresh()
        grdEdit.SizeAllColumnsToContents()

    End Sub

    Private Sub RefreshGrid()
        Dim lRow, lCol, units As Integer
        units = pHspfFtable.Operation.OpnBlk.Uci.GlobalBlock.EmFg

        With grdEdit.Source
            .Rows = txtNRows.Value + 1
            .Columns = txtNCols.Value
            For lCol = 3 To .Columns - 1
                If units = 1 Then
                    .CellValue(0, lCol) = "Outflow" & lCol - 2 & " (ft3/s)"
                Else
                    .CellValue(0, lCol) = "Outflow" & lCol - 2 & " (m3/s)"
                End If
                For m As Integer = 0 To .Columns - 1
                    For k As Integer = 1 To .Rows - 1
                        .CellEditable(k, m) = True
                    Next
                Next
            Next
            For lCol = 0 To .Columns - 1
                For lRow = 1 To .Rows - 1
                    If Len(.CellValue(lRow, lCol)) = 0 Then
                        .CellValue(lRow, lCol) = 0
                    End If
                Next
            Next

            For lCol2 As Integer = 0 To .Columns - 1
                .CellColor(0, lCol2) = SystemColors.ControlLight
            Next

            grdEdit.Refresh()
            grdEdit.SizeAllColumnsToContents()
        End With
    End Sub

    Public Sub New(ByVal aHspfFtables As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdEdit.Source = New atcGridSource

        Data = aHspfFtables
    End Sub

    Private Sub grdEdit_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.CellEdited
        Changed = True
    End Sub

    Private Sub cboID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboID.SelectedIndexChanged
        Dim discard, tempID, lOper As Integer
        lOper = InStr(1, cboID.SelectedItem, "-")
        If lOper > 0 Then
            tempID = CInt(Mid(cboID.SelectedItem, 1, lOper - 2))
        Else
            tempID = CInt(cboID.SelectedItem)
        End If
        If tempID <> pHspfFtable.Id Then
            If pChanged Then
                discard = Logger.Msg("Changes have been made.  Discard them?", Microsoft.VisualBasic.MsgBoxStyle.YesNo, "Discard Changes")
            Else
                discard = 6 'no changes
            End If
            If discard = 6 Then 'change table
                pHspfFtable = pHspfFtable.Operation.OpnBlk.Ids("K" & tempID).FTable
                pChanged = False
                txtNRows.Value = pHspfFtable.Nrows
                txtNCols.Value = pHspfFtable.Ncols
                RefreshFtables()
                RefreshGrid()
            Else 'dont discard set back to previous listindex
                cboID.SelectedIndex = PrevListIndex
            End If
        End If
        PrevListIndex = cboID.SelectedIndex
    End Sub

    Private Sub txtNRows_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNRows.Leave
        If txtNRows.Value > 1 And txtNCols.Value < 26 Then
            RefreshGrid()
            pChanged = True
        End If
    End Sub

    Private Sub txtNCols_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNCols.Leave
        If txtNCols.Value > 3 And txtNCols.Value < 9 Then
            RefreshGrid()
            pChanged = True
        End If
    End Sub

    Private Sub cmdImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImport.Click
        Dim frmXSect As New frmXSect
        frmXSect.CurrentReach(pHspfFtable.Operation.Id, pHspfFtable.Operation.FTable)
        txtNRows.Value = pHspfFtable.Nrows
        txtNCols.Value = pHspfFtable.Ncols
        frmXSect.Init(pHspfFtable, Me)
        frmXSect.Show()
    End Sub

    Private Sub cmdCompute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCompute.Click
        Dim frmNewFTable As New frmNewFTable
        Dim newFTable As New HspfFtable
        newFTable.Operation = pHspfFtable.Operation
        newFTable.Id = pHspfFtable.Id
        frmNewFTable.SetCurrentFTable(newFTable, Me)
        frmNewFTable.Init()
        frmNewFTable.Show()

    End Sub

    Public Sub UpdateFTABLE(ByVal aFtab As HspfFtable)
        Dim i As Long
        Dim j As Long

        txtNRows.Value = aFtab.Nrows
        txtNCols.Value = aFtab.Ncols
        With grdEdit.Source
            .Rows = txtNRows.Value
            .Columns = txtNCols.Value
            For j = 0 To .Columns - 1
                For i = 1 To .Rows - 1
                    .CellEditable(i, j) = True
                Next
            Next
            For i = 1 To .Rows
                .CellValue(i, 0) = aFtab.Depth(i)
                .CellValue(i, 1) = aFtab.Area(i)
                .CellValue(i, 2) = aFtab.Volume(i)
                .CellValue(i, 3) = aFtab.Outflow1(i)
            Next
        End With
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFcurve.Click
        'Set data for plot
        'ReDim XYD(0)
        'XYD(0).NVal = pHspfFtable.Nrows
        'ReDim XYD(0).Var(0).Vals(XYD(0).NVal)
        'ReDim XYD(0).Var(1).Vals(XYD(0).NVal)
        'XYD(0).Var(0).Trans = 1
        '      XYD(0).Var(0).Trans = 1
        '      XYD(0).Var(1).Trans = 1
        '      For j = 0 To 1
        '          XYD(0).Var(j).Min = 1.0E+30
        '          XYD(0).Var(j).Max = -1.0E+30
        '      Next j
        '      For i = 0 To XYD(0).NVal - 1
        '          XYD(0).Var(0).Vals(i) = grdEdit.TextMatrix(i + 1, 3)
        '          XYD(0).Var(1).Vals(i) = grdEdit.TextMatrix(i + 1, 0)
        '          For j = 0 To 1
        '              If XYD(0).Var(j).Vals(i) < XYD(0).Var(j).Min Then
        '                  XYD(0).Var(j).Min = XYD(0).Var(j).Vals(i)
        '              End If
        '              If XYD(0).Var(j).Vals(i) > XYD(0).Var(j).Max Then
        '                  XYD(0).Var(j).Max = XYD(0).Var(j).Vals(i)
        '              End If
        '          Next j
        '      Next i
        '      Call GLInit(1, g, 1, 2)
        '      capt = "F-Curve for Reach " & CStr(pFTable.Operation.Id) & " (" & pFTable.Operation.Description & ")"
        '      Call GLTitl("", capt)
        '      Call GLAxLab("Depth (ft)", "Outflow (cfs)", "", "")
        '      Call GLDoXY(g, 1, XYD(), 1)
        'Else
        '      Call MsgBox("This option is not yet implemented.", , "FTable Problem")
        'End If
    End Sub
End Class

