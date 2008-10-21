Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms

Public Class frmReach

    Dim pVScrollColumnOffset As Integer = 16

    Private Sub grdReach_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdReach.Resize
        grdReach.SizeAllColumnsToContents(grdReach.Width - pVScrollColumnOffset, True)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Size = New Size(580, 320)
        Me.MinimumSize = Me.Size

        ' Add any initialization after the InitializeComponent() call.
        Dim lUnits As Integer = pUCI.GlobalBlock.emfg

        With grdReach
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdReach.Source
            .FixedRows = 1
            .Columns = 6
            .CellValue(0, 0) = "ID"
            .CellValue(0, 1) = "Description"
            If lUnits = 1 Then
                .CellValue(0, 2) = "Length (mi)"
                .CellValue(0, 3) = "Delta H (ft)"
            Else
                .CellValue(0, 2) = "Length (km)"
                .CellValue(0, 3) = "Delta H (m)"
            End If
            .CellValue(0, 4) = "DownstreamID"
            .CellValue(0, 5) = "N Exits"
            .CellValue(0, 6) = "Lake Flag"

            Dim lOperBlock As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            .Rows = lOperBlock.Count

            'loop through each operation and populate the table
            For lRow As Integer = 1 To lOperBlock.Count
                Dim lOperation As HspfOperation = lOperBlock.NthOper(lRow)
                .CellValue(lRow, 0) = lOperation.Id
                Dim lTable As HspfTable = lOperation.Tables("GEN-INFO")
                .CellValue(lRow, 1) = lTable.Parms("RCHID").Value
                .CellValue(lRow, 5) = lTable.Parms("NEXITS").Value
                .CellValue(lRow, 6) = lTable.Parms("LKFG").Value
                lTable = lOperation.Tables("HYDR-PARM2")
                .CellValue(lRow, 2) = lTable.Parms("LEN").Value
                .CellValue(lRow, 3) = lTable.Parms("DELTH").Value
                .CellValue(lRow, 4) = lOperation.DownOper("RCHRES")
            Next

            For j As Integer = 1 To .Columns - 1
                For k As Integer = 1 To .Rows - 1
                    .CellEditable(k, j) = True
                Next
            Next

        End With

        grdReach.SizeAllColumnsToContents(grdReach.Width - pVScrollColumnOffset, True)
        grdReach.Refresh()

        Me.Icon = pIcon

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        With grdReach.Source
            Dim lOperBlock As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            For lRow As Integer = 1 To lOperBlock.Count
                Dim lOperation As HspfOperation = lOperBlock.NthOper(lRow)
                Dim lTable As HspfTable = lOperation.Tables("GEN-INFO")
                lTable.Parms("RCHID").Value = .CellValue(lRow, 1)
                lTable.Parms("NEXITS").Value = .CellValue(lRow, 5)
                lTable.Parms("LKFG").Value = .CellValue(lRow, 6)
                lTable = lOperation.Tables("HYDR-PARM2")
                lTable.Parms("LEN").Value = .CellValue(lRow, 2)
                lTable.Parms("DELTH").Value = .CellValue(lRow, 3)
                'TODO -- put downoper back into puci structure
            Next
        End With
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'pEditControl.Save()
        Me.Dispose()
    End Sub

    Private Sub FTables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FTables.Click
        UCIForms.Edit(Me, pUCI.OpnBlks("RCHRES").NthOper(1).FTable)  'todo: use selected row
    End Sub

    Private Sub grdReach_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdReach.CellEdited
        'todo: add limits 
    End Sub

End Class
