Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Windows.Forms
Imports System.Drawing

Public Class ctlEditOpnSeqBlock
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pHspfOpnSeqBlk As HspfOpnSeqBlk
    Dim pfrmAddOperation As frmAddOperation
    Dim pfrmRenumberOperation As frmRenumberOperation

    Dim pChanged As Boolean
    Dim pCurrentSelectedColumn As Integer
    Dim pCurrentSelectedRow As Integer
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Private Sub grdEdit_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.MouseDownCell
        pCurrentSelectedColumn = aColumn
        pCurrentSelectedRow = aRow
        DoLimits()
    End Sub

    Private Sub grdEdit_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdEdit.Resize
        grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
    End Sub

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Operation Sequence Block"
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

        If IsNothing(pfrmAddOperation) Then
            pfrmAddOperation = New frmAddOperation
            pfrmAddOperation.Init(pHspfOpnSeqBlk, Me.Parent.Parent, pCurrentSelectedRow - 1)
            pfrmAddOperation.ShowDialog()
            Data = pHspfOpnSeqBlk
        Else
            If pfrmAddOperation.IsDisposed Then
                pfrmAddOperation = New frmAddOperation
                pfrmAddOperation.Init(pHspfOpnSeqBlk, Me.Parent.Parent, pCurrentSelectedRow - 1)
                pfrmAddOperation.ShowDialog()
                Data = pHspfOpnSeqBlk
            Else
                pfrmAddOperation.WindowState = FormWindowState.Normal
                pfrmAddOperation.BringToFront()
            End If
        End If

    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfOpnSeqBlk
        End Get
        Set(ByVal aHspfOpnSeqBlk As Object)
            pHspfOpnSeqBlk = aHspfOpnSeqBlk
            txtIndelt.ValueInteger = pHspfOpnSeqBlk.Delt

            With grdEdit
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
                .Source.FixedRows = 1
            End With

            With grdEdit.Source
                .Columns = 2
                .Rows = pHspfOpnSeqBlk.Opns.Count
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Number"

                For lRow As Integer = 1 To .Rows
                    .CellValue(lRow, 0) = pHspfOpnSeqBlk.Opns(lRow - 1).Name
                    .CellValue(lRow, 1) = pHspfOpnSeqBlk.Opns(lRow - 1).Id
                Next

                For lCol As Integer = 0 To .Columns - 1
                    For lRow As Integer = 1 To .Rows - 1
                        .CellEditable(lRow, lCol) = True
                    Next
                Next

            End With

            grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
            grdEdit.Refresh()

        End Set
    End Property

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        With grdEdit.Source
            If pCurrentSelectedRow > 0 Then
                For lRow As Integer = pCurrentSelectedRow To .Rows - 1
                    For lColumn As Integer = 0 To .Columns - 1
                        .CellValue(lRow, lColumn) = .CellValue(lRow + 1, lColumn)
                    Next
                Next
                .Rows -= 1
                Changed = True
            End If
        End With
    End Sub

    Public Sub Save() Implements ctlEdit.Save

        pHspfOpnSeqBlk.Delt = txtIndelt.ValueInteger

        'find out if any operations have been deleted
        Dim lOpnsToDelete As New Collection
        For Each lOpn As HspfOperation In pHspfOpnSeqBlk.Opns
            Dim lInList As Boolean = False
            For lRow As Integer = 1 To grdEdit.Source.Rows
                If Len(grdEdit.Source.CellValue(lRow, 1)) > 0 And _
                   Len(grdEdit.Source.CellValue(lRow, 0)) > 0 Then
                    If lOpn.Id = grdEdit.Source.CellValue(lRow, 1) And _
                        lOpn.Name = grdEdit.Source.CellValue(lRow, 0) Then
                        lInList = True
                    End If
                End If
            Next
            If Not lInList Then
                'delete this operation
                lOpnsToDelete.Add(lOpn)
            End If
        Next
        For Each lOpn As HspfOperation In lOpnsToDelete
            pHspfOpnSeqBlk.Uci.DeleteOperation(lOpn.Name, lOpn.Id)
        Next

        'find out if any operations have been added
        For lRow As Integer = 1 To grdEdit.Source.Rows
            Dim lInList As Boolean = False
            For Each lOpn As HspfOperation In pHspfOpnSeqBlk.Opns
                If grdEdit.Source.CellValue(lRow, 1).Length > 0 And _
                   grdEdit.Source.CellValue(lRow, 0).Length > 0 Then
                    If lOpn.Id = grdEdit.Source.CellValue(lRow, 1) And _
                       lOpn.Name = grdEdit.Source.CellValue(lRow, 0) Then
                        lInList = True
                    End If
                End If
            Next
            If Not lInList And grdEdit.Source.CellValue(lRow, 1).Length > 0 And _
                 grdEdit.Source.CellValue(lRow, 0).Length > 0 Then
                'add this operation
                Dim lOpn As New HspfOperation
                lOpn.Name = grdEdit.Source.CellValue(lRow, 0)
                lOpn.Id = grdEdit.Source.CellValue(lRow, 1)
                pHspfOpnSeqBlk.Uci.AddOperation(lOpn.Name, lOpn.Id)
                pHspfOpnSeqBlk.Uci.AddOperationToOpnSeqBlock(lOpn.Name, lOpn.Id, lRow)
            End If
        Next

        With grdEdit.Source
            For lRow As Integer = 1 To .Rows - 1
                Dim lOpnBlk As HspfOpnBlk = pHspfOpnSeqBlk.Uci.OpnBlks(.CellValue(lRow, 0))
                If lOpnBlk IsNot Nothing Then
                    pHspfOpnSeqBlk.Opns(lRow - 1) = lOpnBlk.OperFromID(.CellValue(lRow, 1))
                End If
            Next
        End With

    End Sub

    Public Sub New(ByVal aHspfOpnSeqBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdEdit.Source = New atcGridSource
        Data = aHspfOpnSeqBlk

    End Sub

    Public Sub DoLimits()
        With grdEdit
            Dim lValidValues As New Collection
            If pCurrentSelectedColumn = 0 Then 'opername
                For lIndex As Integer = 0 To pHspfOpnSeqBlk.Uci.OpnBlks.Count - 1
                    lValidValues.Add(pHspfOpnSeqBlk.Uci.OpnBlks(lIndex).Name)
                Next
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            Else 'oper id column
                .ValidValues = lValidValues
                .Refresh()
            End If
        End With
    End Sub

    Private Sub AtcGridPervious_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.CellEdited
        If aColumn = 1 Then
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)

            'operid should be between 1 and 999
            If lNewValueNumeric > 0 AndAlso lNewValueNumeric < 1000 Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If

            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

    Private Sub cmdRenumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRenumber.Click
        If IsNothing(pfrmRenumberOperation) Then
            pfrmRenumberOperation = New frmRenumberOperation
            pfrmRenumberOperation.Init(pHspfOpnSeqBlk, Me.Parent.Parent)
            pfrmRenumberOperation.ShowDialog()
            Data = pHspfOpnSeqBlk
        Else
            If pfrmRenumberOperation.IsDisposed Then
                pfrmRenumberOperation = New frmRenumberOperation
                pfrmRenumberOperation.Init(pHspfOpnSeqBlk, Me.Parent.Parent)
                pfrmRenumberOperation.ShowDialog()
                Data = pHspfOpnSeqBlk
            Else
                pfrmRenumberOperation.WindowState = FormWindowState.Normal
                pfrmRenumberOperation.BringToFront()
            End If
        End If
    End Sub
End Class
