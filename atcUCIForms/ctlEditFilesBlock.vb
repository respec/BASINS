Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditFilesBlock
    Implements ctlEdit

    Dim pHspfFilesBlk As HspfFilesBlk
    Dim pDataSource As atcGridSource

    Public Sub Add() Implements ctlEdit.Add

    End Sub

    Public Sub Help() Implements ctlEdit.Help

    End Sub

    Public Sub Remove() Implements ctlEdit.Remove

    End Sub

    Public Sub Save() Implements ctlEdit.Save
        With pDataSource
            Logger.Dbg("EditFilesBlocK:Save:RowCount:" & .Rows)
            For lInd As Integer = 1 To .Rows
                Dim lHspfFile As New HspfData.HspfFile
                lHspfFile.Typ = .CellValue(lInd, 0)
                lHspfFile.Unit = .CellValue(lInd, 1)
                lHspfFile.Name = .CellValue(lInd, 2)
                pHspfFilesBlk.Value(lInd) = lHspfFile
            Next
        End With
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfFilesBlk
        End Get
        Set(ByVal aHspfFilesBlk As Object)
            pHspfFilesBlk = aHspfFilesBlk
            With pDataSource
                .Rows = aHspfFilesBlk.count + 1
                For lInd As Integer = 1 To aHspfFilesBlk.count
                    .CellValue(lInd, 0) = pHspfFilesBlk.Value(lInd).Typ
                    .CellEditable(lInd, 0) = True
                    .CellValue(lInd, 1) = pHspfFilesBlk.Value(lInd).Unit
                    .CellEditable(lInd, 1) = True
                    .CellValue(lInd, 2) = pHspfFilesBlk.Value(lInd).Name
                    .CellEditable(lInd, 2) = True
                Next
            End With
            With grdEdit
                .Clear()
                .Source = pDataSource
                .SizeAllColumnsToContents()
                .Refresh()
            End With
        End Set
    End Property

    Public Sub New(ByVal aHspfFilesBlk As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pDataSource = New atcGridSource
        With pDataSource
            .Rows = 1
            .Columns = 3
            .CellValue(0, 0) = "Type"
            .CellValue(0, 1) = "Unit"
            .CellValue(0, 2) = "Name"
            .FixedRows = 1
        End With
        Data = aHspfFilesBlk
    End Sub
End Class
