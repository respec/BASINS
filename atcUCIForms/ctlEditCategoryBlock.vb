Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditCategory
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
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

    Private Sub grdEdit_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdEdit.Resize
        grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
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
            End With

            With grdEdit.Source
                .FixedRows = 1
                .Columns = 2
                .CellValue(0, 0) = "Tag"
                .CellValue(0, 1) = "Name"

                If pHspfCategoryBlk.Categories.Count = 0 Then
                    .Rows = 2
                ElseIf pHspfCategoryBlk.Categories.Count > 0 Then
                    For lRow As Integer = 1 To .Rows
                        .CellValue(lRow, 0) = pHspfCategoryBlk.Categories(lRow - 1).Tag
                        .CellValue(lRow, 1) = pHspfCategoryBlk.Categories(lRow - 1).Name
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

            End With

            grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
            grdEdit.Refresh()

        End Set
    End Property

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove

    End Sub

    Public Sub Save() Implements ctlEdit.Save
        pHspfCategoryBlk.Clear()
        With grdEdit.Source
            Logger.Dbg("EditOpnSeqBlocK:Save:RowCount:" & .Rows)
            For i As Integer = 1 To .Rows - 1
                Dim lCategory As New HspfCategory
                lCategory.Tag = .CellValue(i, 0)
                lCategory.Name = .CellValue(i, 1)
                pHspfCategoryBlk.Add(lCategory)
            Next
        End With
        pChanged = False
    End Sub

    Public Sub New(ByVal aHspfCategoryBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdEdit.Source = New atcGridSource

        Data = aHspfCategoryBlk
    End Sub
End Class
