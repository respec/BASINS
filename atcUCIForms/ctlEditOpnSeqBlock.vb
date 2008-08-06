Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditOpnSeqBlock
    Implements ctlEdit

    Dim pHspfOpnSeqBlk As HspfOpnSeqBlk
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Open Sequence Block"
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
        'not needed 
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfOpnSeqBlk
        End Get
        Set(ByVal aHspfOpnSeqBlk As Object)
            pHspfOpnSeqBlk = aHspfOpnSeqBlk
            txtIndelt.Value = pHspfOpnSeqBlk.Delt

            With grdEdit
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
                .ColumnWidth(0) = 200
                .ColumnWidth(1) = 200
            End With

            With grdEdit.Source
                .Columns = 2
                .Rows = pHspfOpnSeqBlk.Opns.Count
                .CellValue(0, 0) = "Name"
                .CellValue(0, 1) = "Number"

                For lRow As Integer = 1 To .Rows - 1
                    .CellValue(lRow, 0) = pHspfOpnSeqBlk.Opn(lRow).Name
                    .CellValue(lRow, 1) = pHspfOpnSeqBlk.Opn(lRow).Id
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
        Dim i As Integer

        pHspfOpnSeqBlk.Delt = txtIndelt.Value
        With grdEdit.Source
            For i = 1 To .Rows - 1
                pHspfOpnSeqBlk.Opn(i).Name = .CellValue(i, 0)
                pHspfOpnSeqBlk.Opn(i).Id = .CellValue(i, 1)
            Next
        End With
        pChanged = False
    End Sub

    Public Sub New(ByVal aHspfGOpnSeqBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        grdEdit.Source = New atcGridSource

        Data = aHspfGOpnSeqBlk
    End Sub
End Class
