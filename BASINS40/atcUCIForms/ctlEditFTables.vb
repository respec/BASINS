Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms

Public Class ctlEditFTables
    Implements ctlEdit

    Dim pHspfFtable As HspfFtable
    Dim pDataSource As atcGridSource
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
        With pDataSource
            Changed = False
        End With
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

    Private Sub RefreshFtables()
        Dim i, j, units As Integer
        units = pHspfFtable.Operation.OpnBlk.Uci.GlobalBlock.emfg

        With grdEdit
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdEdit.Source
            .Rows = txtNRows.Value
            .Columns = txtNCols.Value
            For j = 0 To .Columns - 1
                .CellEditable(0, j) = False
            Next
            If units = 1 Then
                .CellValue(0, 0) = "Depth (ft)"
                .CellValue(0, 1) = "Area (acres)"
                .CellValue(0, 2) = "Volume (acre-ft)"
                For j = 3 To .Columns - 1
                    .CellValue(0, j) = "Outflow" & j - 2 & "(ft3/s)"
                Next
            Else 'metric
                .CellValue(0, 0) = "Depth (m)"
                .CellValue(0, 1) = "Area (ha)"
                .CellValue(0, 2) = "Volume (Mm3)"
                For j = 3 To .Columns - 1
                    .CellValue(0, j) = "Outflow" & j - 2 & "(m3/s)"
                Next
            End If

            For lCol As Integer = 0 To .Columns - 1
                .CellColor(0, lCol) = SystemColors.ControlLight
            Next

            For m As Integer = 0 To .Columns - 1
                For k As Integer = 1 To .Rows - 1
                    .CellEditable(k, m) = True
                Next
            Next

            For i = 1 To .Rows
                .CellValue(i, 0) = pHspfFtable.Depth(i)
                .CellValue(i, 1) = pHspfFtable.Area(i)
                .CellValue(i, 2) = pHspfFtable.Volume(i)
                For j = 3 To .Columns - 1
                    If j = 3 Then
                        .CellValue(i, j) = pHspfFtable.Outflow1(i)
                    End If
                    If j = 4 Then
                        .CellValue(i, j) = pHspfFtable.Outflow2(i)
                    End If
                    If j = 5 Then
                        .CellValue(i, j) = pHspfFtable.Outflow3(i)
                    End If
                    If j = 6 Then
                        .CellValue(i, j) = pHspfFtable.Outflow4(i)
                    End If
                    If j = 7 Then
                        .CellValue(i, j) = pHspfFtable.Outflow5(i)
                    End If

                Next
            Next
        End With

        grdEdit.Refresh()
        grdEdit.SizeAllColumnsToContents()

    End Sub
    Private Sub refreshGrid()
        Dim i, j, units As Integer
        units = pHspfFtable.Operation.OpnBlk.Uci.GlobalBlock.emfg

        With grdEdit.Source
            .Rows = txtNRows.Value
            .Columns = txtNCols.Value
            For j = 3 To .Columns - 1
                If units = 1 Then
                    .CellValue(0, j) = "Outflow" & j - 2 & " (ft3/s)"
                Else
                    .CellValue(0, j) = "Outflow" & j - 2 & " (m3/s)"
                End If
                For m As Integer = 0 To .Columns - 1
                    For k As Integer = 1 To .Rows - 1
                        .CellEditable(k, m) = True
                    Next
                Next
            Next
            For j = 0 To .Columns - 1
                For i = 1 To .Rows
                    If Len(.CellValue(i, j)) = 0 Then
                        .CellValue(i, j) = 0
                    End If
                Next
            Next
            grdEdit.Refresh()
            grdEdit.SizeAllColumnsToContents()
        End With
    End Sub
    Public Sub New(ByVal aHspfFtables As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pDataSource = New atcGridSource

        Data = aHspfFtables
    End Sub

    Private Sub grdEdit_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.CellEdited
        Changed = True
    End Sub

    Private Sub cmdImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImport.Click
        Dim frmXSect As New frmXSect
        frmXSect.Show()
    End Sub

    Private Sub cmdCompute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCompute.Click
        Dim frmNewFTable As New frmNewFTable
        frmNewFTable.Show()
    End Sub

    'Private Sub cboID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboID.SelectedIndexChanged
    '    Dim discard, tempID, i As Integer

    '    i = InStr(1, cboID, "-")
    '    If i > 0 Then
    '        tempID = CInt(Mid(cboID, 1, i - 2))

    '    End If
    'End Sub
End Class

