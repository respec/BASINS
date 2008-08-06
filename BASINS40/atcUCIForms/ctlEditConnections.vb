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
            With grdEdit.Source
                If pConnectionType = "NETWORK" Then

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
