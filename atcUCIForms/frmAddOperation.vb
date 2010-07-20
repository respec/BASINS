Imports atcUCI
Imports MapWinUtility

Public Class frmAddOperation

    Dim pUci As HspfUci
    Dim pSelectedRow As Integer
    Dim pParentForm As Object

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

    End Sub

    Friend Sub Init(ByVal aHspfOpnSeqBlk As HspfOpnSeqBlk, ByVal aParentForm As Windows.Forms.Form, ByVal aSelectedRow As Integer)
        pUci = aHspfOpnSeqBlk.Uci
        pSelectedRow = aSelectedRow
        pParentForm = aParentForm
        Me.Icon = aParentForm.Icon

        With cboOperationType
            .Items.Clear()
            For lIndex As Integer = 0 To pUci.OpnBlks.Count - 1
                .Items.Add(pUci.OpnBlks(lIndex).Name)
            Next
        End With
        cboOperationType.SelectedIndex = 0

        lstUp.Items.Clear()
        lstDown.Items.Clear()
        For lIndex As Integer = 0 To pUci.OpnSeqBlock.Opns.Count - 1
            If lIndex < aSelectedRow Or (lIndex = aSelectedRow And lIndex = pUci.OpnSeqBlock.Opns.Count) Then
                lstUp.Items.Add(pUci.OpnSeqBlock.Opns(lIndex).Name & " " & pUci.OpnSeqBlock.Opns(lIndex).Id)
            Else
                lstDown.Items.Add(pUci.OpnSeqBlock.Opns(lIndex).Name & " " & pUci.OpnSeqBlock.Opns(lIndex).Id)
            End If
        Next

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        'first check to make sure this name/number are okay
        Dim lOperationName As String = cboOperationType.Items(cboOperationType.SelectedIndex)
        Dim lOperationId As Integer = 0
        If IsNumeric(txtOperationNumber.Text) Then
            lOperationId = CInt(txtOperationNumber.Text)
        End If
        If Len(lOperationName) > 0 And lOperationId > 0 Then
            If pUci.OpnBlks(lOperationName).OperFromID(lOperationId) Is Nothing Then
                'okay oper to add
                pUci.AddOperation(lOperationName, lOperationId)
                pUci.AddOperationToOpnSeqBlock(lOperationName, lOperationId, pSelectedRow)
                pUci.OpnBlks(lOperationName).OperFromID(lOperationId).Description = txtOperationDescription.Text
                Dim lTxt As String = ""
                Dim lSpacePos As Integer = 0
                Dim lName As String = ""
                Dim lNum As Integer = 0
                If lstUp.SelectedItems.Count > 0 Then
                    'want some upstream operations
                    For lIndex As Integer = 0 To lstUp.SelectedItems.Count - 1
                        lTxt = lstUp.SelectedItems(lIndex).ToString
                        lSpacePos = InStr(1, lTxt, " ")
                        lName = Mid(lTxt, 1, lSpacePos - 1)
                        lNum = CInt(Mid(lTxt, lSpacePos + 1))
                        Call AddConnection(lName, lNum, lOperationName, lOperationId)
                    Next
                End If
                If lstDown.SelectedItems.Count > 0 Then
                    'want some downstream operations
                    For lIndex As Integer = 0 To lstDown.SelectedItems.Count - 1
                        lTxt = lstDown.SelectedItems(lIndex).ToString
                        lSpacePos = InStr(1, lTxt, " ")
                        lName = Mid(lTxt, 1, lSpacePos - 1)
                        lNum = CInt(Mid(lTxt, lSpacePos + 1))
                        Call AddConnection(lOperationName, lOperationId, lName, lNum)
                    Next
                End If
                pUci.Edited = True
            Else
                'this oper already exists!
                Logger.Msg("This operation already exists." & vbCrLf & vbCrLf & _
                           "Try a different operation type or number.", MsgBoxStyle.OkOnly, _
                           "Add Operation Problem")
            End If
        Else
            'some info not entered!
            Logger.Msg("A valid value has not been entered for the" & vbCrLf & _
                       "operation type or number.", MsgBoxStyle.OkOnly, _
                       "Add Operation Problem")
        End If
        Me.Dispose()

    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub AddConnection(ByVal aSourceName As String, ByVal aSourceId As Integer, ByVal aTargetName As String, ByVal aTargetId As Integer)

        Dim lMLId As Integer
        If Not pUci.MassLinks Is Nothing Then
            lMLId = pUci.MassLinks(1).FindMassLinkID(aSourceName, aTargetName)
        Else
            lMLId = 0
        End If
        If lMLId < 1 Then
            'no masslink exists for this combination, display message
            Logger.Msg("No connections exist between operation types " & aSourceName & " and " & aTargetName & "." & vbCrLf & vbCrLf & _
                       "This connection must be added manually by editing the Schematic and MassLink Blocks.", MsgBoxStyle.OkOnly, "Add Operation Problem")
        Else
            'masslink exists, go ahead and add connection
            Dim lConnection = New HspfConnection
            lConnection.Uci = pUci
            lConnection.Typ = 3
            lConnection.Source.VolName = aSourceName
            lConnection.Source.VolId = aSourceId
            lConnection.Source.Opn = pUci.OpnBlks(aSourceName).OperFromID(aSourceId)
            lConnection.MFact = 1
            lConnection.Target.VolName = aTargetName
            lConnection.Target.VolId = aTargetId
            lConnection.Target.Opn = pUci.OpnBlks(aTargetName).OperFromID(aTargetId)
            lConnection.MassLink = lMLId
            pUci.Connections.Add(lConnection)
            pUci.OpnBlks(aSourceName).OperFromID(aSourceId).Targets.Add(lConnection)
            pUci.OpnBlks(aTargetName).OperFromID(aTargetId).Sources.Add(lConnection)
        End If

    End Sub
End Class