Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports System.Collections.ObjectModel

Public Class ctlEditConnections
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pConnection As HspfConnection
    Dim pConnectionType As String 'ext sources, ext targets, schematic, or network
    Dim pConnections As Collection(Of HspfConnection)
    Dim pChanged As Boolean
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Private Sub grdTable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdEdit.Resize
        grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
    End Sub

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

            With grdEdit
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            '<<<<<<<<<< NETWORK >>>>>>>>>>
            With grdEdit.Source
                If pConnectionType = "NETWORK" Then
                    .FixedRows = 1
                    .Columns = 15

                    .Rows = 1
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
                    .CellValue(0, 14) = "Comment"



                    For i As Integer = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
                        lOper = pConnection.Uci.OpnSeqBlock.Opn(i)
                        For j As Integer = 0 To lOper.Sources.Count - 1
                            lConn = lOper.Sources(j)
                            If lConn.Typ = 2 Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 0) = lConn.Source.VolName
                                .CellValue(.Rows - 1, 1) = lConn.Source.VolId
                                .CellValue(.Rows - 1, 2) = lConn.Source.Group
                                .CellValue(.Rows - 1, 3) = lConn.Source.Member
                                .CellValue(.Rows - 1, 4) = lConn.Source.MemSub1
                                .CellValue(.Rows - 1, 5) = lConn.Source.MemSub2
                                .CellValue(.Rows - 1, 6) = lConn.MFact
                                .CellValue(.Rows - 1, 7) = lConn.Tran
                                .CellValue(.Rows - 1, 8) = lOper.Name
                                .CellValue(.Rows - 1, 9) = lOper.Id
                                .CellValue(.Rows - 1, 10) = lConn.Target.Group
                                .CellValue(.Rows - 1, 11) = lConn.Target.Member
                                .CellValue(.Rows - 1, 12) = lConn.Target.MemSub1
                                .CellValue(.Rows - 1, 13) = lConn.Target.MemSub2
                                .CellValue(.Rows - 1, 14) = lConn.Comment

                            End If
                        Next
                    Next

                    '<<<<<<<<<< SCHEMATIC >>>>>>>>>>

                ElseIf pConnectionType = "SCHEMATIC" Then
                    .FixedRows = 1
                    .Columns = 9

                    .Rows = 1
                    .CellValue(0, 0) = "VolName"
                    .CellValue(0, 1) = "VolID"
                    .CellValue(0, 2) = "Area Fact"
                    .CellValue(0, 3) = "VolName"
                    .CellValue(0, 4) = "VolId"
                    .CellValue(0, 5) = "MLId"
                    .CellValue(0, 6) = "Sub1"
                    .CellValue(0, 7) = "Sub2"
                    .CellValue(0, 8) = "Comment"

                    For lLoopVar1 As Integer = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
                        lOper = pConnection.Uci.OpnSeqBlock.Opn(lLoopVar1)
                        For lLoopVar2 As Integer = 0 To lOper.Sources.Count - 1
                            lConn = lOper.Sources(lLoopVar2)
                            If lConn.Typ = 3 Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 0) = lConn.Source.VolName
                                .CellValue(.Rows - 1, 1) = lConn.Source.VolId
                                .CellValue(.Rows - 1, 2) = lConn.MFact
                                .CellValue(.Rows - 1, 3) = lConn.Target.VolName
                                .CellValue(.Rows - 1, 4) = lConn.Target.VolId
                                .CellValue(.Rows - 1, 5) = lConn.MassLink
                                .CellValue(.Rows - 1, 6) = lConn.Target.MemSub1
                                .CellValue(.Rows - 1, 7) = lConn.Target.MemSub2
                                .CellValue(.Rows - 1, 8) = lOper.Comment
                            End If
                        Next
                    Next

                    '<<<<<<<<<< EXT SOURCES >>>>>>>>>>

                ElseIf pConnectionType = "EXT SOURCES" Then
                    .FixedRows = 1
                    .Columns = 15

                    .Rows = 1
                    .CellValue(0, 0) = "VolName"
                    .CellValue(0, 1) = "VolID"
                    .CellValue(0, 2) = "MemName"
                    .CellValue(0, 3) = "QFlag"
                    .CellValue(0, 4) = "SSystem"
                    .CellValue(0, 5) = "SgapStr"
                    .CellValue(0, 6) = "MultFact"
                    .CellValue(0, 7) = "Tran"
                    .CellValue(0, 8) = "VolName"
                    .CellValue(0, 9) = "VolId"
                    .CellValue(0, 10) = "Group"
                    .CellValue(0, 11) = "MemName"
                    .CellValue(0, 12) = "MemSub1"
                    .CellValue(0, 13) = "MemSub2"
                    .CellValue(0, 14) = "Comment"

                    For lLoopVar1 As Integer = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
                        lOper = pConnection.Uci.OpnSeqBlock.Opn(lLoopVar1)
                        For lLoopVar2 As Integer = 0 To lOper.Sources.Count - 1
                            lConn = lOper.Sources(lLoopVar2)
                            If lConn.Typ = 1 Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 0) = lConn.Source.VolName
                                .CellValue(.Rows - 1, 1) = lConn.Source.VolId
                                .CellValue(.Rows - 1, 2) = lConn.Source.Member
                                .CellValue(.Rows - 1, 3) = lConn.Source.MemSub1
                                .CellValue(.Rows - 1, 4) = lConn.Ssystem
                                .CellValue(.Rows - 1, 5) = lConn.Sgapstrg
                                .CellValue(.Rows - 1, 6) = lConn.MFact
                                .CellValue(.Rows - 1, 7) = lConn.Tran
                                .CellValue(.Rows - 1, 8) = lOper.Name
                                .CellValue(.Rows - 1, 9) = lOper.Id
                                .CellValue(.Rows - 1, 10) = lConn.Target.Group
                                .CellValue(.Rows - 1, 11) = lConn.Target.Member
                                .CellValue(.Rows - 1, 12) = lConn.Target.MemSub1
                                .CellValue(.Rows - 1, 13) = lConn.Target.MemSub2
                                .CellValue(.Rows - 1, 14) = lConn.Comment
                            End If
                        Next
                    Next

                    '<<<<<<<<<< EXT TARGETS >>>>>>>>>>

                ElseIf pConnectionType = "EXT TARGETS" Then
                    .FixedRows = 1
                    .Columns = 16

                    .Rows = 1
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
                    .CellValue(0, 10) = "MemName"
                    .CellValue(0, 11) = "QFlag"
                    .CellValue(0, 12) = "TSystem"
                    .CellValue(0, 13) = "AggrStr"
                    .CellValue(0, 14) = "AmdStr"
                    .CellValue(0, 15) = "Comment"

                    For lLoopVar1 As Integer = 1 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
                        lOper = pConnection.Uci.OpnSeqBlock.Opn(lLoopVar1)
                        For lLoopVar2 As Integer = 0 To lOper.Targets.Count - 1
                            lConn = lOper.Targets(lLoopVar2)
                            If lConn.Typ = 4 Then
                                .Rows += 1
                                .CellValue(.Rows - 1, 0) = lConn.Source.VolName
                                .CellValue(.Rows - 1, 1) = lConn.Source.VolId
                                .CellValue(.Rows - 1, 2) = lConn.Source.Group
                                .CellValue(.Rows - 1, 3) = lConn.Source.Member
                                .CellValue(.Rows - 1, 4) = lConn.Source.MemSub1
                                .CellValue(.Rows - 1, 5) = lConn.Source.MemSub2
                                .CellValue(.Rows - 1, 6) = lConn.MFact
                                .CellValue(.Rows - 1, 7) = lConn.Tran
                                .CellValue(.Rows - 1, 8) = lConn.Target.VolName
                                .CellValue(.Rows - 1, 9) = lConn.Target.VolId
                                .CellValue(.Rows - 1, 10) = lConn.Target.Member
                                .CellValue(.Rows - 1, 11) = lConn.Target.MemSub1
                                .CellValue(.Rows - 1, 12) = lConn.Ssystem
                                .CellValue(.Rows - 1, 13) = lConn.Sgapstrg
                                .CellValue(.Rows - 1, 14) = lConn.Amdstrg
                                .CellValue(.Rows - 1, 15) = lConn.Comment
                            End If
                        Next
                    Next

                End If

                For lCol As Integer = 0 To .Columns - 1
                    For lRow As Integer = 1 To .Rows - 1
                        .CellEditable(lRow, lCol) = True
                    Next
                Next

            End With

            grdEdit.SizeAllColumnsToContents(grdEdit.Width - pVScrollColumnOffset, True)
            grdEdit.Refresh()

        End Set
    End Property

    Private Sub grdTableClick(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.MouseDownCell
        Dim lBlockDef As New HspfBlockDef
        Dim lSelectedColumn As Integer = aColumn

        If Len(pConnectionType) > 0 Then
            lBlockDef = pConnection.Uci.Msg.BlockDefs(pConnectionType)
            lSelectedColumn = aColumn
            If pConnectionType = "EXT SOURCES" Then
                If lSelectedColumn < 10 Then
                    lSelectedColumn = aColumn
                Else
                    lSelectedColumn = aColumn + 1
                End If
            ElseIf pConnectionType = "EXT TARGETS" Then
                lSelectedColumn = aColumn
            ElseIf pConnectionType = "NETWORK" Then
                If lSelectedColumn < 10 Then
                    lSelectedColumn = aColumn
                Else
                    lSelectedColumn = aColumn + 1
                End If
            ElseIf pConnectionType = "SCHEMATIC" Then
                lSelectedColumn = aColumn
            End If

            If lSelectedColumn <= lBlockDef.TableDefs(0).ParmDefs.Count - 1 Then
                txtDefine.Text = lBlockDef.TableDefs(0).ParmDefs(lSelectedColumn).Name & ": " & lBlockDef.TableDefs(0).ParmDefs(lSelectedColumn).Define
            Else
                txtDefine.Clear()
            End If
        End If

    End Sub

    Public Sub New(ByVal aHspfConnection As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        pConnectionType = aTag
        Data = aHspfConnection
    End Sub
End Class
