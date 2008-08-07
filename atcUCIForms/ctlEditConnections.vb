Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports System.Collections.ObjectModel

Public Class ctlEditConnections
    Implements ctlEdit

    Dim pConnection As HspfConnection
    Dim pConnectionType As String 'ext sources, ext targets, schematic, or network
    Dim pConnections As Collection(Of HspfConnection)
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return pConnectionType
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
        With grdEdit.Source
            'TODO: need selected rows
            'Dim lRow, lCol As Integer
            'Dim lTmp As Boolean = .CellSelected(lRow, lCol)
        End With
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        With grdEdit.Source

        End With
    End Sub

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pConnection
        End Get
        Set(ByVal aConnection As Object)
            pConnection = aConnection
            pConnections = pConnection.Uci.Connections
            Dim lOper As New HspfOperation
            Dim lConn As New HspfConnection

            With grdEdit.Source
                If pConnectionType = "NETWORK" Then
                    .Columns = 15
                    .CellValue(0, 0) = "VolName"
                    .CellValue(0, 1) = "VolID"
                    .CellValue(0, 2) = "Group"
                    .CellValue(0, 3) = "MemName"
                    .CellValue(0, 4) = "MemSub1"
                    .CellValue(0, 5) = "MemSub2"
                    .CellValue(0, 6) = "MultFact"
                    .CellValue(0, 7) = "Tran"
                    .CellValue(0, 8) = "VolName"
                    .CellValue(0, 9) = "VolId"
                    .CellValue(0, 10) = "Group"
                    .CellValue(0, 11) = "MemName"
                    .CellValue(0, 12) = "MemSub1"
                    .CellValue(0, 13) = "MemSub2"
                    .CellValue(0, 14) = "HIDE"

                    For j As Integer = 1 To .Columns - 1
                        For k As Integer = 1 To .Rows - 1
                            .CellEditable(k, j) = True
                        Next
                    Next

                    For lCol As Integer = 0 To .Columns
                        .CellColor(0, lCol) = SystemColors.ControlLight
                    Next

                    For i As Integer = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1 'NOTE: Index values may need adjusting
                        lOper = pConnection.Uci.OpnSeqBlock.Opn(i)
                        For j As Integer = 1 To lOper.Sources.Count - 1 'NOTE: Index values may need adjusting
                            lConn = lOper.Sources(j)
                            If lConn.Typ = 2 Then
                                .Rows += 1
                                .CellValue(.Rows, 0) = lConn.Source.VolName
                                .CellValue(.Rows, 1) = lConn.Source.VolId
                                .CellValue(.Rows, 2) = lConn.Source.Group
                                .CellValue(.Rows, 3) = lConn.Source.Member
                                .CellValue(.Rows, 4) = lConn.Source.MemSub1
                                .CellValue(.Rows, 5) = lConn.Source.MemSub2
                                .CellValue(.Rows, 6) = lConn.MFact
                                .CellValue(.Rows, 7) = lConn.Tran
                                .CellValue(.Rows, 8) = lOper.Name
                                .CellValue(.Rows, 9) = lOper.Id
                                .CellValue(.Rows, 10) = lConn.Target.Group
                                .CellValue(.Rows, 11) = lConn.Target.Member
                                .CellValue(.Rows, 12) = lConn.Target.MemSub1
                                .CellValue(.Rows, 13) = lConn.Target.MemSub2
                                .CellValue(.Rows, 14) = lConn.Comment

                            End If
                        Next
                    Next
                End If
            End With
            With grdEdit
                .Clear()
                .SizeAllColumnsToContents()
                .Refresh()
            End With
        End Set
    End Property

    Public Sub New(ByVal aHspfConnection As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pConnectionType = aTag
        grdEdit.Source = New atcGridSource

        Data = aHspfConnection
    End Sub
End Class
