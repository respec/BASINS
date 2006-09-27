Imports atcUCI
Imports atcControls

Public Class ctlEditFilesBlock
    Implements ctlEdit

    Dim pData As HspfFilesBlk
    Dim pDataSource As atcGridSource

    Public Sub Add() Implements ctlEdit.Add

    End Sub

    Public Sub Help() Implements ctlEdit.Help

    End Sub

    Public Sub Remove() Implements ctlEdit.Remove

    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pData
        End Get
        Set(ByVal value As Object)
            pData = value
            pDataSource = New atcGridSource
            With pDataSource
                .Rows = 2
                .Columns = 3
                .CellValue(0, 0) = "Type"
                .CellValue(0, 1) = "Unit"
                .CellValue(0, 2) = "Name"
            End With
            grdEdit.Clear()
            grdEdit.Source = pDataSource
            grdEdit.SizeAllColumnsToContents()
            grdEdit.Refresh()
        End Set
    End Property

    Public Sub New(ByVal aData As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aData
    End Sub
End Class
