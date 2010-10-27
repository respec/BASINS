Imports atcUCI
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmAddDataSet

    Dim pUci As HspfUci
    Dim pEditControl As Object 'the parent control
    Dim pVScrollColumnOffset As Integer = 16

    Public Sub InitializeForm(ByVal aUci As HspfUci, ByVal aEditControl As Object, ByVal aNoDsns As Collection, ByVal aWDMIds As Collection, ByVal aScens As Collection, ByVal aLocs As Collection, ByVal aCons As Collection)
        pEditControl = aEditControl
        pUci = aUci
        Me.Icon = pEditControl.ParentForm.Icon

        With agdAdd
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With agdAdd.Source
            .FixedRows = 1
            .FixedColumns = 2
            .Columns = 7
            .Rows = 1
            .CellValue(0, 0) = "WDM ID"
            .CellValue(0, 1) = "DSN"
            .CellValue(0, 2) = "Scenario"
            .CellValue(0, 3) = "Location"
            .CellValue(0, 4) = "Constituent"
            .CellValue(0, 5) = "Time Units"
            .CellValue(0, 6) = "Time Step"
            For lDsnIndex As Integer = 1 To aNoDsns.Count
                .Rows += 1
                .CellValue(.Rows - 1, 0) = aWDMIds(lDsnIndex)
                .CellValue(.Rows - 1, 1) = aNoDsns(lDsnIndex)
                .CellValue(.Rows - 1, 2) = aScens(lDsnIndex)
                .CellValue(.Rows - 1, 3) = aLocs(lDsnIndex)
                .CellValue(.Rows - 1, 4) = aCons(lDsnIndex)
                .CellValue(.Rows - 1, 5) = 4
                .CellValue(.Rows - 1, 6) = 1
                .CellEditable(.Rows - 1, 2) = True
                .CellEditable(.Rows - 1, 3) = True
                .CellEditable(.Rows - 1, 4) = True
                .CellEditable(.Rows - 1, 5) = True
                .CellEditable(.Rows - 1, 6) = True
            Next
        End With

        agdAdd.SizeAllColumnsToContents(agdAdd.Width - pVScrollColumnOffset, True)
        agdAdd.Refresh()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        With agdAdd.Source
            For lRow As Integer = 1 To .Rows - 1
                Dim lWdmid As Integer = WDMInd(.CellValue(lRow, 0))
                Dim lDsn As Integer = .CellValue(lRow, 1)
                Dim lScen As String = .CellValue(lRow, 2)
                Dim lLocn As String = .CellValue(lRow, 3)
                Dim lCons As String = .CellValue(lRow, 4)
                Dim lTu As Integer = .CellValue(lRow, 5)
                Dim lTs As Integer = .CellValue(lRow, 6)
                pUci.AddWDMDataSet(lWdmid, lDsn, lScen, lLocn, lCons, lTu, lTs)
            Next
        End With

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Dispose()
    End Sub

    Private Sub agdAdd_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdAdd.CellEdited
        Dim lMinValue As Integer = -999
        Dim lMaxValue As Integer = -999
        If aColumn = 5 Then
            lMaxValue = 6
            lMinValue = 1
        ElseIf aColumn = 6 Then
            lMinValue = 1
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
    End Sub
End Class