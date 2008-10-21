Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditFilesBlock
    Implements ctlEdit

    Dim pHspfFilesBlk As HspfFilesBlk
    Dim pDataSource As atcGridSource
    Dim pChanged As Boolean
    Dim pVScrollColumnOffset As Integer = 16
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Private Sub grdEdit_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdEdit.Resize
        grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
    End Sub

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Files Block"
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
        With pDataSource
            .Rows += 1
            .CellEditable(.Rows - 1, 0) = True
            .CellEditable(.Rows - 1, 1) = True
            .CellEditable(.Rows - 1, 2) = True
            .CellEditable(.Rows - 1, 3) = True
        End With
        Changed = True
    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'TODO: add this code
        With pDataSource
            'TODO: need selected rows
            'Dim lRow, lCol As Integer
            'Dim lTmp As Boolean = .CellSelected(lRow, lCol)
        End With
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        With pDataSource
            Logger.Dbg("EditFilesBlocK:Save:RowCount:" & .Rows)
            pHspfFilesBlk.Clear()
            For lInd As Integer = 1 To .Rows - 1
                Dim lHspfFile As New HspfData.HspfFile
                lHspfFile.Typ = .CellValue(lInd, 0)
                lHspfFile.Unit = .CellValue(lInd, 1)
                lHspfFile.Name = .CellValue(lInd, 2)
                lHspfFile.Comment = .CellValue(lInd, 3)
                pHspfFilesBlk.Value(lInd) = lHspfFile
            Next
            Changed = False
        End With
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfFilesBlk
        End Get
        Set(ByVal aHspfFilesBlk As Object)
            pHspfFilesBlk = aHspfFilesBlk

            Me.MinimumSize = New System.Drawing.Size(640, 320)

            With pDataSource
                .Rows = aHspfFilesBlk.count
                For lInd As Integer = 1 To aHspfFilesBlk.count
                    .CellValue(lInd, 0) = pHspfFilesBlk.Value(lInd).Typ
                    .CellEditable(lInd, 0) = True
                    .CellValue(lInd, 1) = pHspfFilesBlk.Value(lInd).Unit
                    .CellEditable(lInd, 1) = True
                    .CellValue(lInd, 2) = pHspfFilesBlk.Value(lInd).Name
                    .CellEditable(lInd, 2) = True
                    .CellValue(lInd, 3) = pHspfFilesBlk.Value(lInd).Comment
                    .CellEditable(lInd, 3) = True
                Next
            End With
            With grdEdit
                .Clear()
                .Source = pDataSource
                grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
                .Refresh()
            End With
        End Set
    End Property

    Public Sub New(ByVal aHspfFilesBlk As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pDataSource = New atcGridSource
        With pDataSource
            .Rows = 1
            .Columns = 4
            .ColorCells = True
            .CellValue(0, 0) = "Type"
            .CellColor(0, 0) = aParent.BackColor
            .CellValue(0, 1) = "Unit"
            .CellColor(0, 1) = aParent.BackColor
            .CellValue(0, 2) = "Name"
            .CellColor(0, 2) = aParent.BackColor
            .CellValue(0, 3) = "Preceeding Comment"
            .CellColor(0, 3) = aParent.BackColor
            .FixedRows = 1
        End With
        Data = aHspfFilesBlk
    End Sub

    Private Sub grdEdit_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.CellEdited
        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
        Dim lNewValueNumeric As Double = Double.NaN

        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

        Select Case aColumn
            Case 1 'Unit should be between 21 and 99, must be between 1 and 99
                If lNewValueNumeric < 1 AndAlso lNewValueNumeric > 99 Or _
                   lNewValueNumeric = Double.NaN Then
                    lNewColor = Color.Pink
                ElseIf lNewValueNumeric < 21 Then
                    lNewColor = Color.Yellow
                Else
                    lNewColor = aGrid.CellBackColor
                End If
        End Select

        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If
        Changed = True
    End Sub

End Class
