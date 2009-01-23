Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports atcControls
Imports System.Collections.ObjectModel
Imports atcData
Imports System.Drawing

Public Class ctlEditConnections
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pConnection As HspfConnection
    Dim pConnectionType As String 'ext sources, ext targets, schematic, or network
    Dim pConnections As Collection(Of HspfConnection)
    Dim pChanged As Boolean
    Dim pCurrentSelectedRow As Integer
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
        End With
        Changed = True
    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        With grdEdit.Source
            If pCurrentSelectedRow > 0 Then
                For lRow As Integer = pCurrentSelectedRow To .Rows - 1
                    For lColumn As Integer = 0 To .Columns - 1
                        .CellValue(lRow, lColumn) = .CellValue(lRow + 1, lColumn)
                    Next
                Next
                .Rows -= 1
                Changed = True
            End If
        End With
    End Sub

    Public Sub Save() Implements ctlEdit.Save
        With grdEdit.Source

            If pConnectionType = "NETWORK" Then
                'put network block back
                RemoveSourcesFromOperations()
                RemoveTargetsFromOperations()
                pConnection.Uci.RemoveConnectionsFromCollection(2)
                'create new connections
                For lRow As Integer = 2 To .Rows
                    Dim lRowComplete As Boolean = True
                    For lColumn As Integer = 1 To .Columns - 1
                        If .CellValue(lRow, lColumn - 1).Length < 1 And lColumn <> 8 And lColumn <> 4 And lColumn <> 12 Then
                            'no data entered for this field, dont do this row
                            lRowComplete = False
                        End If
                    Next lColumn
                    If Not lRowComplete Then
                        Logger.Msg("Some required fields on row " & lRow - 1 & " are empty." & vbCrLf & _
                                   "This row will be ignored.", MsgBoxStyle.OkOnly, _
                                   pConnection.Caption & " Edit Problem")
                    Else
                        Dim lConn As New HspfConnection
                        lConn.Uci = pConnection.Uci
                        lConn.Typ = 2
                        lConn.Source.VolName = .CellValue(lRow, 0)
                        lConn.Source.VolId = .CellValue(lRow, 1)
                        lConn.Source.Group = .CellValue(lRow, 2)
                        lConn.Source.Member = .CellValue(lRow, 3)
                        lConn.Source.MemSub1 = .CellValue(lRow, 4)
                        lConn.Source.MemSub2 = .CellValue(lRow, 5)
                        lConn.MFact = .CellValue(lRow, 6)
                        lConn.Tran = .CellValue(lRow, 7)
                        lConn.Target.VolName = .CellValue(lRow, 8)
                        lConn.Target.VolId = .CellValue(lRow, 9)
                        lConn.Target.Group = .CellValue(lRow, 10)
                        lConn.Target.Member = .CellValue(lRow, 11)
                        lConn.Target.MemSub1 = .CellValue(lRow, 12)
                        lConn.Target.MemSub2 = .CellValue(lRow, 13)
                        lConn.Comment = .CellValue(lRow, 14)
                        pConnection.Uci.Connections.Add(lConn)
                    End If
                Next
                'set timser connections
                For Each lOpn As HspfOperation In pConnection.Uci.OpnSeqBlock.Opns
                    lOpn.SetTimSerConnections()
                Next
                pConnection.Uci.Edited = True
            ElseIf pConnectionType = "SCHEMATIC" Then
                'put schematic block back
                RemoveSourcesFromOperations()
                RemoveTargetsFromOperations()
                pConnection.Uci.RemoveConnectionsFromCollection(3)
                'create new connections
                For lRow As Integer = 2 To .Rows
                    Dim lRowComplete As Boolean = True
                    For lColumn As Integer = 1 To .Columns - 1
                        If .CellValue(lRow, lColumn - 1).Length < 1 Then
                            'no data entered for this field, dont do this row
                            lRowComplete = False
                        End If
                    Next lColumn
                    If Not lRowComplete Then
                        Logger.Msg("Some required fields on row " & lRow - 1 & " are empty." & vbCrLf & _
                                   "This row will be ignored.", MsgBoxStyle.OkOnly, _
                                   pConnection.Caption & " Edit Problem")
                    Else
                        Dim lConn As New HspfConnection
                        lConn.Uci = pConnection.Uci
                        lConn.Typ = 3
                        lConn.Source.VolName = .CellValue(lRow, 0)
                        lConn.Source.VolId = .CellValue(lRow, 1)
                        lConn.MFact = .CellValue(lRow, 2)
                        lConn.Target.VolName = .CellValue(lRow, 3)
                        lConn.Target.VolId = .CellValue(lRow, 4)
                        lConn.MassLink = .CellValue(lRow, 5)
                        lConn.Target.MemSub1 = .CellValue(lRow, 6)
                        lConn.Target.MemSub2 = .CellValue(lRow, 7)
                        lConn.Comment = .CellValue(lRow, 8)
                        pConnection.Uci.Connections.Add(lConn)
                    End If
                Next
                'set timser connections
                For Each lOpn As HspfOperation In pConnection.Uci.OpnSeqBlock.Opns
                    lOpn.SetTimSerConnections()
                Next
                pConnection.Uci.Edited = True
            ElseIf pConnectionType = "EXT SOURCES" Then
                'put external sources back
                RemoveSourcesFromOperations()
                pConnection.Uci.RemoveConnectionsFromCollection(1)
                'create new connections
                For lRow As Integer = 2 To .Rows
                    Dim lRowComplete As Boolean = True
                    For lColumn As Integer = 1 To .Columns - 1
                        If .CellValue(lRow, lColumn - 1).Length < 1 And lColumn <> 6 And lColumn <> 8 Then
                            'no data entered for this field, dont do this row
                            lRowComplete = False
                        End If
                    Next lColumn
                    If Not lRowComplete Then
                        Logger.Msg("Some required fields on row " & lRow - 1 & " are empty." & vbCrLf & _
                                   "This row will be ignored.", MsgBoxStyle.OkOnly, _
                                   pConnection.Caption & " Edit Problem")
                    Else
                        Dim lConn As New HspfConnection
                        lConn.Uci = pConnection.Uci
                        lConn.Typ = 1
                        lConn.Source.VolName = .CellValue(lRow, 0)
                        lConn.Source.VolId = .CellValue(lRow, 1)
                        lConn.Source.Member = .CellValue(lRow, 2)
                        lConn.Source.MemSub1 = .CellValue(lRow, 3)
                        lConn.Ssystem = .CellValue(lRow, 4)
                        lConn.Sgapstrg = .CellValue(lRow, 5)
                        lConn.MFact = .CellValue(lRow, 6)
                        lConn.Tran = .CellValue(lRow, 7)
                        lConn.Target.VolName = .CellValue(lRow, 8)
                        lConn.Target.VolId = .CellValue(lRow, 9)
                        lConn.Target.Group = .CellValue(lRow, 10)
                        lConn.Target.Member = .CellValue(lRow, 11)
                        lConn.Target.MemSub1 = .CellValue(lRow, 12)
                        lConn.Target.MemSub2 = .CellValue(lRow, 13)
                        lConn.Comment = .CellValue(lRow, 14)
                        pConnection.Uci.Connections.Add(lConn)
                    End If
                Next
                'set timser connections
                For Each lOpn As HspfOperation In pConnection.Uci.OpnSeqBlock.Opns
                    lOpn.SetTimSerConnections()
                Next
                pConnection.Uci.Edited = True
            ElseIf pConnectionType = "EXT TARGETS" Then
                'put ext targets back
                If CheckDataSetExistance(pConnection.Uci) Then
                    'data sets have all been created, go ahead and refresh
                    RemoveTargetsFromOperations()
                    pConnection.Uci.RemoveConnectionsFromCollection(4)
                    'create new connections
                    For lRow As Integer = 2 To .Rows
                        Dim lRowComplete As Boolean = True
                        For lColumn As Integer = 1 To .Columns - 1
                            If .CellValue(lRow, lColumn - 1).Length < 1 And lColumn <> 8 And lColumn <> 14 Then
                                'no data entered for this field, dont do this row
                                lRowComplete = False
                            End If
                        Next lColumn
                        If Not lRowComplete Then
                            Logger.Msg("Some required fields on row " & lRow - 1 & " are empty." & vbCrLf & _
                                       "This row will be ignored.", MsgBoxStyle.OkOnly, _
                                       pConnection.Caption & " Edit Problem")
                        Else
                            Dim lConn As New HspfConnection
                            lConn.Uci = pConnection.Uci
                            lConn.Typ = 4
                            lConn.Source.VolName = .CellValue(lRow, 0)
                            lConn.Source.VolId = .CellValue(lRow, 1)
                            lConn.Source.Group = .CellValue(lRow, 2)
                            lConn.Source.Member = .CellValue(lRow, 3)
                            lConn.Source.MemSub1 = .CellValue(lRow, 4)
                            lConn.Source.MemSub2 = .CellValue(lRow, 5)
                            lConn.MFact = .CellValue(lRow, 6)
                            lConn.Tran = .CellValue(lRow, 7)
                            lConn.Target.VolName = .CellValue(lRow, 8)
                            lConn.Target.VolId = .CellValue(lRow, 9)
                            lConn.Target.Member = .CellValue(lRow, 10)
                            If .CellValue(lRow, 11).Length > 0 Then
                                lConn.Target.MemSub1 = .CellValue(lRow, 11)
                            End If
                            lConn.Ssystem = .CellValue(lRow, 12)
                            lConn.Sgapstrg = .CellValue(lRow, 13)
                            lConn.Amdstrg = .CellValue(lRow, 14)
                            lConn.Comment = .CellValue(lRow, 15)
                            pConnection.Uci.Connections.Add(lConn)
                        End If
                    Next
                    'set timser connections
                    For Each lOpn As HspfOperation In pConnection.Uci.OpnSeqBlock.Opns
                        lOpn.SetTimSerConnections()
                    Next
                    pConnection.Uci.Edited = True
                End If
            End If
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
            grdEdit.ColumnWidth(grdEdit.Source.Columns - 1) = 0   'make last column, comment, not visible
            grdEdit.Refresh()

        End Set
    End Property

    Private Sub grdTableClick(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.MouseDownCell
        Dim lBlockDef As New HspfBlockDef
        Dim lSelectedColumn As Integer = aColumn

        pCurrentSelectedRow = aRow
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

            DoLimits(pConnectionType, aColumn, aRow)

        End If

    End Sub

    Private Sub DoLimits(ByVal aConnectionType As String, ByVal aSelectedColumn As Integer, ByVal aSelectedRow As Integer)
        If aConnectionType = "EXT SOURCES" Then
            With grdEdit
                Dim lValidValues As New Collection
                If aSelectedColumn = 0 Then 'svol
                    SetLimitsWDM(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 1 Then 'svolno
                    Me.Cursor = Windows.Forms.Cursors.WaitCursor
                    CheckValidDsn(lValidValues, .Source.CellValue(aSelectedRow, aSelectedColumn - 1), pConnection.Uci)
                    Me.Cursor = Windows.Forms.Cursors.Default
                ElseIf aSelectedColumn = 2 Then 'smemn
                    CheckValidMemberName(lValidValues, .Source.CellValue(aSelectedRow, aSelectedColumn - 1), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), pConnection.Uci)
                ElseIf aSelectedColumn = 3 Then 'qflg
                    For lQflg As Integer = 0 To 31
                        lValidValues.Add(lQflg)
                    Next
                ElseIf aSelectedColumn = 4 Then 'ssyst
                    lValidValues.Add("ENGL")
                    lValidValues.Add("METR")
                ElseIf aSelectedColumn = 5 Then 'sgapst
                    lValidValues.Add(" ") 'allow default blank
                    lValidValues.Add("UNDF")
                    lValidValues.Add("ZERO")
                ElseIf aSelectedColumn = 6 Then 'mfactr
                ElseIf aSelectedColumn = 7 Then 'tran
                    SetValidTrans(lValidValues)
                ElseIf aSelectedColumn = 8 Then 'tvol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 9 Then 'topfst
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 10 Then 'group
                    SetGroupNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 2))
                ElseIf aSelectedColumn = 11 Then 'tmemn
                    SetMemberNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 12 Then 'tmems1
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 4), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), True)
                ElseIf aSelectedColumn = 13 Then 'tmems2
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 5), .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), False)
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aConnectionType = "EXT TARGETS" Then
            With grdEdit
                Dim lValidValues As New Collection
                If aSelectedColumn = 0 Then 'svol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 1 Then 'svolno
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 2 Then 'sgrpn
                    SetGroupNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 2))
                ElseIf aSelectedColumn = 3 Then 'smemn
                    SetMemberNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 4 Then 'smems1
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 4), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), True)
                ElseIf aSelectedColumn = 5 Then 'smems2
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 5), .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), False)
                ElseIf aSelectedColumn = 6 Then 'mfactr
                ElseIf aSelectedColumn = 7 Then 'tran
                    SetValidTrans(lValidValues)
                ElseIf aSelectedColumn = 8 Then 'tvol:wdm and what else
                    SetLimitsWDM(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 9 Then 'tvolno
                    'handled in celledited
                ElseIf aSelectedColumn = 10 Then 'tmemn
                    CheckValidMemberName(lValidValues, .Source.CellValue(aSelectedRow, aSelectedColumn - 1), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), pConnection.Uci)
                ElseIf aSelectedColumn = 11 Then 'qflg
                    For lQflg As Integer = 0 To 31
                        lValidValues.Add(lQflg)
                    Next
                ElseIf aSelectedColumn = 12 Then 'tsyst
                    lValidValues.Add("ENGL")
                    lValidValues.Add("METR")
                ElseIf aSelectedColumn = 13 Then 'aggst
                    lValidValues.Add(" ") 'allow default blank
                    lValidValues.Add("AGGR")
                ElseIf aSelectedColumn = 14 Then 'amdst
                    lValidValues.Add("ADD ")
                    lValidValues.Add("REPL")
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aConnectionType = "NETWORK" Then
            With grdEdit
                Dim lValidValues As New Collection
                If aSelectedColumn = 0 Then 'svol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 1 Then 'svolno
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 2 Then 'sgrpn
                    SetGroupNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 2))
                ElseIf aSelectedColumn = 3 Then 'smemn
                    SetMemberNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 4 Then 'smems1
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 4), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), True)
                ElseIf aSelectedColumn = 5 Then 'smems2
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 5), .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), False)
                ElseIf aSelectedColumn = 6 Then 'mfactr
                ElseIf aSelectedColumn = 7 Then 'tran
                    SetValidTrans(lValidValues)
                ElseIf aSelectedColumn = 8 Then 'tvol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 9 Then 'topfst
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 10 Then 'tgrpn
                    SetGroupNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 2))
                ElseIf aSelectedColumn = 11 Then 'tmem
                    SetMemberNames(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 12 Then 'tmems1
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 4), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), .Source.CellValue(aSelectedRow, aSelectedColumn - 1), True)
                ElseIf aSelectedColumn = 13 Then 'tmems2
                    SetMemberSubscript(lValidValues, pConnection.Uci.Msg, .Source.CellValue(aSelectedRow, aSelectedColumn - 5), .Source.CellValue(aSelectedRow, aSelectedColumn - 3), .Source.CellValue(aSelectedRow, aSelectedColumn - 2), False)
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        ElseIf aConnectionType = "SCHEMATIC" Then
            With grdEdit
                Dim lValidValues As New Collection
                If aSelectedColumn = 0 Then 'svol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 1 Then 'svolno
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 2 Then 'mfactor
                ElseIf aSelectedColumn = 3 Then 'tvol
                    SetValidOperations(lValidValues, pConnection.Uci)
                ElseIf aSelectedColumn = 4 Then 'tvolno
                    SetOperationMinMax(lValidValues, pConnection.Uci, .Source.CellValue(aSelectedRow, aSelectedColumn - 1))
                ElseIf aSelectedColumn = 5 Then 'mslkno
                    SetValidMassLinks(lValidValues, pConnection.Uci)
                End If
                .ValidValues = lValidValues
                .AllowNewValidValues = False
                .Refresh()
            End With
        End If
    End Sub

    Private Sub grdEdit_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles grdEdit.CellEdited
        If aColumn = 9 Then
            'handle target dsn for ext targets block
            Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
            Dim lNewValueNumeric As Double = -999
            If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)

            Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)

            'dsn should be between 1 and 9999
            If lNewValueNumeric > 0 AndAlso lNewValueNumeric < 10000 Then
                lNewColor = aGrid.CellBackColor
            Else
                lNewColor = Color.Pink
            End If

            If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
                aGrid.Source.CellColor(aRow, aColumn) = lNewColor
            End If
        End If
    End Sub

    Private Sub SetLimitsWDM(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        Dim lFiles As HspfFilesBlk = aUci.FilesBlock

        For lIndex As Integer = 1 To aUci.FilesBlock.Count
            Dim lFile As HspfFile = aUci.FilesBlock.Value(lIndex)
            If lFile.Typ.Length > 2 Then
                If lFile.Typ.Substring(0, 3) = "WDM" Then
                    aValidValues.Add(lFile.Typ)
                End If
            End If
        Next
    End Sub

    Private Sub CheckValidDsn(ByVal aValidValues As Collection, ByVal aWDMid As String, ByVal aUci As HspfUci)
        Dim lWdmId As Integer = WDMInd(aWDMid)
        For lDsn As Integer = 1 To 9999
            Dim lDsnObj As atcData.atcTimeseries = aUci.GetDataSetFromDsn(lWdmId, lDsn)
            If Not lDsnObj Is Nothing Then
                aValidValues.Add(lDsn)
            End If
        Next
    End Sub

    Private Sub CheckValidMemberName(ByVal aValidValues As Collection, ByVal aDsn As Integer, ByVal aWDMid As String, ByVal aUci As HspfUci)
        Dim lDsnObj As atcData.atcTimeseries = aUci.GetDataSetFromDsn(WDMInd(aWDMid), aDsn)
        If Not lDsnObj Is Nothing Then
            aValidValues.Add(lDsnObj.Attributes.GetDefinedValue("TSTYPE").Value)
        End If
    End Sub

    Private Sub SetValidTrans(ByVal aValidValues As Collection)
        aValidValues.Add(" ") 'allow default blank
        aValidValues.Add("SAME")
        aValidValues.Add("AVER")
        aValidValues.Add("DIV ")
        aValidValues.Add("INTP")
        aValidValues.Add("LAST")
        aValidValues.Add("MAX ")
        aValidValues.Add("MIN ")
        aValidValues.Add("SUM ")
    End Sub

    Private Sub SetValidOperations(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        For Each lOpnBlk As HspfOpnBlk In aUci.OpnBlks
            If lOpnBlk.Count > 0 Then
                aValidValues.Add(lOpnBlk.Name)
            End If
        Next
    End Sub

    Private Sub SetOperationMinMax(ByVal aValidValues As Collection, ByVal aUci As HspfUci, ByVal aOperType As String)
        If aOperType.Length > 0 Then
            Dim lOpnBlk As HspfOpnBlk = aUci.OpnBlks(Trim(aOperType))
            If lOpnBlk.Count > 0 Then
                For lIndex As Integer = 0 To lOpnBlk.Count - 1
                    aValidValues.Add(lOpnBlk.Ids(lIndex).Id)
                Next
            End If
        End If
    End Sub

    Private Sub SetGroupNames(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String)
        If aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper Then
                        aValidValues.Add(lGroup.Name)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub SetMemberNames(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String, ByVal aGroupName As String)
        If aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper And lGroup.Name = aGroupName Then
                        'this is the one we want
                        Dim lSkey As String = CStr(lGroup.Id)
                        For Each lMember As HspfTSMemberDef In aMsg.TSGroupDefs(lSkey).MemberDefs
                            aValidValues.Add(lMember.Name)
                        Next
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub SetMemberSubscript(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String, ByVal aGroupName As String, ByVal aMemberName As String, ByVal aFirst As Boolean)
        If aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper And lGroup.Name = aGroupName Then
                        Dim lSkey As String = CStr(lGroup.Id)
                        For Each lMember As HspfTSMemberDef In aMsg.TSGroupDefs(lSkey).MemberDefs
                            If lMember.Name = aMemberName Then
                                If aFirst Then
                                    If lMember.Maxsb1 = 1 Then aValidValues.Add(0)
                                    For lSub As Integer = 1 To lMember.Maxsb1
                                        aValidValues.Add(lSub)
                                    Next
                                Else
                                    If lMember.Maxsb2 = 1 Then aValidValues.Add(0)
                                    For lSub As Integer = 1 To lMember.Maxsb2
                                        aValidValues.Add(lSub)
                                    Next
                                End If
                                Exit Sub
                            End If
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub SetValidMassLinks(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        For Each lMassLink As HspfMassLink In aUci.MassLinks
            If Not aValidValues.Contains(lMassLink.MassLinkId) Then
                aValidValues.Add(lMassLink.MassLinkId)
            End If
        Next
    End Sub

    Private Function WDMInd(ByRef wdmid As String) As Integer
        Dim w As String

        If Len(wdmid) > 3 Then
            w = Mid(wdmid, 4, 1)
            If w = " " Then w = "1"
        Else
            w = "1"
        End If
        WDMInd = CInt(w)
    End Function

    Private Function HspfOperNum(ByRef aName As String) As HspfData.HspfOperType
        Dim lHspfOperNum As HspfData.HspfOperType

        Select Case aName
            Case "PERLND" : lHspfOperNum = HspfData.HspfOperType.hPerlnd
            Case "IMPLND" : lHspfOperNum = HspfData.HspfOperType.hImplnd
            Case "RCHRES" : lHspfOperNum = HspfData.HspfOperType.hRchres
            Case "COPY" : lHspfOperNum = HspfData.HspfOperType.hCopy
            Case "PLTGEN" : lHspfOperNum = HspfData.HspfOperType.hPltgen
            Case "DISPLY" : lHspfOperNum = HspfData.HspfOperType.hDisply
            Case "DURANL" : lHspfOperNum = HspfData.HspfOperType.hDuranl
            Case "GENER" : lHspfOperNum = HspfData.HspfOperType.hGener
            Case "MUTSIN" : lHspfOperNum = HspfData.HspfOperType.hMutsin
            Case "BMPRAC" : lHspfOperNum = HspfData.HspfOperType.hBmprac
            Case "REPORT" : lHspfOperNum = HspfData.HspfOperType.hReport
            Case Else : lHspfOperNum = 0
        End Select

        Return lHspfOperNum
    End Function

    Private Sub RemoveSourcesFromOperations()
        For lIndex As Integer = 0 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
            'remove sources from opns
            Dim lOper As HspfOperation = pConnection.Uci.OpnSeqBlock.Opns(lIndex)
            Do While lOper.Sources.Count > 0
                lOper.Sources.Remove(lOper.Sources(0))
            Loop
        Next
    End Sub

    Private Sub RemoveTargetsFromOperations()
        For lIndex As Integer = 0 To pConnection.Uci.OpnSeqBlock.Opns.Count - 1
            'remove targets from opns
            Dim lOper As HspfOperation = pConnection.Uci.OpnSeqBlock.Opns(lIndex)
            Do While lOper.Targets.Count > 0
                lOper.Targets.Remove(lOper.Targets(0))
            Loop
        Next
    End Sub

    Public Function CheckDataSetExistance(ByVal aUci As HspfUci) As Boolean

        Dim lNoDsns As New Collection
        Dim lWDMIds As New Collection
        Dim lScens As New Collection
        Dim lLocs As New Collection
        Dim lCons As New Collection

        With grdEdit.Source
            For lRow As Integer = 1 To .Rows
                'does this wdm data set exist
                Dim lDsn As String = .CellValue(lRow, 9)
                Dim lWdmid As String = .CellValue(lRow, 8)
                If lDsn.Length > 0 AndAlso lWdmid.Length > 0 AndAlso IsNumeric(lDsn) Then
                    Dim lDataSet As atcData.atcTimeseries = aUci.GetDataSetFromDsn(WDMInd(lWdmid), CInt(lDsn))
                    If lDataSet Is Nothing Then
                        'does not exist
                        lNoDsns.Add(lDsn)
                        lWDMIds.Add(lWdmid)
                        lScens.Add(UCase(FilenameNoExt(aUci.Name)))
                        lLocs.Add(.CellValue(lRow, 0) & .CellValue(lRow, 1))
                        lCons.Add(.CellValue(lRow, 10))
                    End If
                End If
            Next
        End With

        If lNoDsns.Count > 0 Then
            '    frmAddDataSet.icon = myUci.Icon
            '    Call frmAddDataSet.SetUCI(myUci)
            '    frmAddDataSet.Show(1)
            'if vbcancel return false
        End If
        Return True
    End Function

    Public Sub New(ByVal aHspfConnection As Object, ByVal aParent As Windows.Forms.Form, ByVal aTag As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        pConnectionType = aTag
        Data = aHspfConnection
    End Sub
End Class
