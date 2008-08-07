Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditCategory
    Implements ctlEdit

    Dim pHspfCategoryBlk As HspfCategoryBlk
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Edit Category Block"
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
        With grdEdit.Source
            .Rows += 1
            .CellEditable(.Rows - 1, 0) = True
            .CellEditable(.Rows - 1, 1) = True
        End With
        pChanged = True
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfCategoryBlk
        End Get
        Set(ByVal aHspfCategoryBlk As Object)
            pHspfCategoryBlk = aHspfCategoryBlk

            With grdEdit
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
                .ColumnWidth(0) = 175
                .ColumnWidth(1) = 225
            End With

            With grdEdit.Source
                .Columns = 2
                .Rows = pHspfCategoryBlk.Categories.Count
                .CellValue(0, 0) = "Tag"
                .CellValue(0, 1) = "Name"
                For lRow As Integer = 1 To .Rows
                    .CellValue(lRow, 0) = pHspfCategoryBlk.Categories(lRow - 1).Name
                    .CellValue(lRow, 1) = pHspfCategoryBlk.Categories(lRow - 1).Tag
                Next

                For lCol As Integer = 0 To .Columns - 1
                    .CellColor(0, lCol) = SystemColors.ControlLight
                Next

                For m As Integer = 0 To .Columns - 1
                    For k As Integer = 1 To .Rows - 1
                        .CellEditable(k, m) = True
                    Next
                Next

            End With
            grdEdit.Refresh()
        End Set
    End Property

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'not be needed
    End Sub

    Public Sub Save() Implements ctlEdit.Save

        'TODO: Open the Add Dialog
        'With grdEdit.Source
        '    Logger.Dbg("EditOpnSeqBlocK:Save:RowCount:" & .Rows)
        '    For i As Integer = 1 To .Rows - 1
        '        pHspfOpnSeqBlk.Opn(i).Name = .CellValue(i, 0)
        '        pHspfOpnSeqBlk.Opn(i).Id = .CellValue(i, 1)
        '    Next
        'End With
        'pChanged = False
    End Sub

    Public Sub New(ByVal aHspfCategoryBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdEdit.Source = New atcGridSource

        Data = aHspfCategoryBlk
    End Sub
End Class
