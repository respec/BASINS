Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class frmReach

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With grdReach
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .Visible = True
        End With

        With grdReach.Source
            .Columns = 5
            .CellValue(0, 0) = "ID"
            .CellValue(0, 1) = "Description"
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            .CellColor(0, 0) = SystemColors.ControlLight

            Dim lOperBlock As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            .Rows = lOperBlock.Count

            'loop through each operation and populate the table
            For lRow As Integer = 1 To lOperBlock.Count
                Dim lOperation As HspfOperation = lOperBlock.NthOper(lRow)
                .CellValue(lRow, 0) = lOperation.Id
                Dim lTable As HspfTable = lOperation.Tables("GEN-INFO")
                .CellValue(lRow, 1) = lTable.Parms("RCHID").Value
            Next
        End With

        grdReach.Refresh()
        grdReach.SizeAllColumnsToContents()

        Me.Icon = pIcon

    End Sub
End Class