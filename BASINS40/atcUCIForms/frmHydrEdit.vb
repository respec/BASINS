Imports atcUCI
Imports atcControls
Imports System

Public Class frmHydrEdit

    Dim lMyTable As HspfTable
    Dim lOptionVals As New ArrayList
    Dim lIdList As New ArrayList
    Dim lPrevCboReachIndex As Integer
    Dim lPrevCboExitIndex As Integer
    Dim lParmCount As Integer

    Public Sub Init(ByVal lParent As Object)
        Dim lOper As Integer
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim lTempTable As HspfTable
        lMyTable = lParent

        Me.Text = "HYDR Simulation and Input Options"
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        cboReach.Items.Clear()
        cboReach.Cursor = Windows.Forms.Cursors.Hand
        cboReach.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
        cboReach.Items.Clear()

        cboExit.Cursor = Windows.Forms.Cursors.Hand
        cboExit.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
        cboExit.Items.Clear()

        For Each vOpn In lMyTable.Opn.Uci.OpnBlks("RCHRES").Ids
            lOpn = vOpn
            cboReach.Items.Add(lOpn.Description & " (" & lOpn.Id & ")")
            lIdList.Add(lOpn.Id)
            'save a local copy of the values
            lTempTable = lOpn.Tables(lMyTable.Name)
            lParmCount = lTempTable.Parms.Count + 1
            For lOper = 0 To lTempTable.Parms.Count - 1
                lOptionVals.Add(lTempTable.Parms(lOper).Value)
            Next
            lOptionVals.Add(lOpn.Tables("GEN-INFO").Parms("NEXITS").Value)
        Next
        lPrevCboReachIndex = -1
        lPrevCboExitIndex = -1
        cboReach.SelectedIndex = 0
    End Sub

    Private Sub cboReach_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboReach.SelectedIndexChanged
        Dim lOptionIndex, lNumExits, lReachIdOffset As Integer

        If lPrevCboReachIndex <> -1 Then
            StoreChanges(lPrevCboReachIndex)
        End If

        'after user changed combo list, use to refresh values
        If chkAssign.Checked = False Then
            'assign to all is not checked
            lReachIdOffset = cboReach.SelectedIndex * lParmCount

            If lOptionVals(lReachIdOffset) = 0 Then
                radio1n1.Checked = True
            Else
                radio1n2.Checked = True
            End If

            If lOptionVals(lReachIdOffset + 1) = 1 Then
                chkAux1.Checked = True
            Else
                chkAux1.Checked = False
            End If

            If lOptionVals(lReachIdOffset + 2) = 1 Then
                chkAux2.Checked = True
            Else
                chkAux2.Checked = False
            End If

            If lOptionVals(lReachIdOffset + 3) = 1 Then
                chkAux3.Checked = True
            Else
                chkAux3.Checked = False
            End If

            lNumExits = lOptionVals(lReachIdOffset + lParmCount - 1)
            cboExit.Items.Clear()
            For lOptionIndex = 1 To lNumExits
                cboExit.Items.Add("Exit " & lOptionIndex)
            Next
            cboExit.SelectedIndex = 0
        End If
        lPrevCboReachIndex = cboReach.SelectedIndex

    End Sub

    Private Sub cboExit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboExit.SelectedIndexChanged
        Dim lExitIndex, lReachIdOffset As Integer

        If lPrevCboExitIndex <> -1 Then
            StoreChanges(cboReach.SelectedIndex)
        End If

        'after user changed combo list, use to refresh values
        lReachIdOffset = cboReach.SelectedIndex * lParmCount
        lExitIndex = cboExit.SelectedIndex

        atxFvol.ValueDouble = lOptionVals(lReachIdOffset + 4 + lExitIndex)
        atxGt.ValueDouble = lOptionVals(lReachIdOffset + 9 + lExitIndex)

        If lOptionVals(lReachIdOffset + 14 + lExitIndex) = 1 Then
            radio2n1.Checked = True
        ElseIf lOptionVals(lReachIdOffset + 14 + lExitIndex) = 2 Then
            radio2n2.Checked = True
        ElseIf lOptionVals(lReachIdOffset + 14 + lExitIndex) = 3 Then
            radio2n3.Checked = True
        End If
        lPrevCboExitIndex = cboExit.SelectedIndex

    End Sub

    Private Sub StoreChanges(ByVal lStoreIndex As Integer)
        Dim lReachIdOffset, lExitIndex, lExitStartNum, lExitStopNum As Integer

        lReachIdOffset = lStoreIndex * lParmCount

        If radio1n1.Checked = True Then
            lOptionVals(lReachIdOffset) = 0
        Else
            lOptionVals(lReachIdOffset) = 1
        End If

        If chkAux1.Checked = True Then
            lOptionVals(lReachIdOffset + 1) = 1
        Else
            lOptionVals(lReachIdOffset + 1) = 0
        End If

        If chkAux2.Checked = True Then
            lOptionVals(lReachIdOffset + 2) = 1
        Else
            lOptionVals(lReachIdOffset + 2) = 0
        End If

        If chkAux3.Checked = True Then
            lOptionVals(lReachIdOffset + 3) = 1
        Else
            lOptionVals(lReachIdOffset + 3) = 0
        End If

        If chkAssignExit.Checked = False Then
            lExitStartNum = lPrevCboExitIndex
            lExitStopNum = lPrevCboExitIndex
        Else
            lExitStartNum = 0
            lExitStopNum = cboExit.Items.Count - 1
        End If

        For lExitIndex = lExitStartNum To lExitStopNum
            lOptionVals(lReachIdOffset + 4 + lExitIndex) = atxFvol.ValueDouble
            lOptionVals(lReachIdOffset + 9 + lExitIndex) = atxGt.ValueDouble
            If radio2n1.Checked = True Then
                lOptionVals(lReachIdOffset + 14 + lExitIndex) = 1
            ElseIf radio2n2.Checked = True Then
                lOptionVals(lReachIdOffset + 14 + lExitIndex) = 2
            ElseIf radio2n3.Checked = True Then
                lOptionVals(lReachIdOffset + 14 + lExitIndex) = 3
            End If
        Next

    End Sub

    Private Sub SaveChanges(ByVal lSaveIndex As Integer)
        Dim lOptionIndex, lReachIdOffset As Integer
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable

        lReachIdOffset = lSaveIndex * lParmCount
        lOpn = lMyTable.Opn.Uci.OpnBlks("RCHRES").OperFromID(lIdList(lSaveIndex))
        ltable = lOpn.Tables(lMyTable.Name)

        For lOptionIndex = 0 To lMyTable.Parms.Count - 1
            lTable.Parms(lOptionIndex).Value = lOptionVals(lReachIdOffset + lOptionIndex)
        Next
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lReachId As Integer

        If chkAssign.Checked = True Then
            For lReachId = 0 To cboReach.Items.Count - 1
                StoreChanges(lReachId)
            Next
        Else
            StoreChanges(cboReach.SelectedIndex)
        End If

        For lReachId = 0 To cboReach.Items.Count - 1
            SaveChanges(lReachId)
        Next

        Me.Close()
    End Sub
End Class