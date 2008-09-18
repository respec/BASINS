Imports atcUCI
Imports atcControls
Imports System

Public Class frmHydrEdit

    Dim lMyTable As HspfTable
    Dim lOptionVals As New ArrayList
    Dim lIdList As New ArrayList
    Dim lPrevCboLandIndex As Integer
    Dim parmcount As Integer

    Public Sub Init(ByVal aParent As Object)
        Dim lOper As Integer
        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim tempTable As HspfTable
        lMyTable = aParent

        Me.Text = "HYDR Simulation and Input Options"
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        cboReach.Items.Clear()
        cboReach.Cursor = Windows.Forms.Cursors.Hand
        cboReach.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList
        cboReach.Items.Clear()

        For Each vOpn In lMyTable.Opn.Uci.OpnBlks("RCHRES").Ids
            lOpn = vOpn
            cboReach.Items.Add(lOpn.Description & " (" & lOpn.Id & ")")
            lIdList.Add(lOpn.Id)
            'save a local copy of the values
            tempTable = lOpn.Tables(lMyTable.Name)
            parmcount = tempTable.Parms.Count + 1
            For lOper = 0 To tempTable.Parms.Count - 1
                lOptionVals.Add(tempTable.Parms(lOper).Value)
            Next
            lOptionVals.Add(lOpn.Tables("GEN-INFO").Parms("NEXITS").Value)
        Next

        cboReach.SelectedIndex = 0

    End Sub

    Private Sub cboReach_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboReach.SelectedIndexChanged
        Dim i, nexit, j As Integer

        'after user changed combo list, use to refresh values
        If chkAssign.Checked = False Then
            'assign to all is not checked
            j = cboReach.SelectedIndex * parmcount

            If lOptionVals(j) = 0 Then
                radio1n1.Checked = True
            Else
                radio1n2.Checked = True
            End If

            If lOptionVals(j + 1) = 1 Then
                chkAux1.Checked = True
            Else
                chkAux1.Checked = False
            End If

            If lOptionVals(j + 2) = 1 Then
                chkAux2.Checked = True
            Else
                chkAux2.Checked = False
            End If

            If lOptionVals(j + 3) = 1 Then
                chkAux3.Checked = True
            Else
                chkAux3.Checked = False
            End If

            nexit = lOptionVals(j + parmcount)
            cboExit.Items.Clear()
            For i = 1 To nexit
                cboExit.Items.Add("Exit " & i)
            Next
            'cboExit.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboExit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboExit.SelectedIndexChanged
        'Dim i, nexit, j As Integer

        ''after user changed combo list, use to refresh values
        'j = cboReach.SelectedIndex * parmcount
        'nexit = cboExit.SelectedIndex + 1

        'atxFvol.Value = lOptionVals(j + 4 + nexit)
        'atxGt.Value = lOptionVals(j + 9 + nexit)

        'If lOptionVals(j + 14 + nexit) = 1 Then
        '    radio2n1.Checked = True
        'ElseIf lOptionVals(j + 14 + nexit) = 2 Then
        '    radio2n2.Checked = True
        'ElseIf lOptionVals(j + 14 + nexit) = 3 Then
        '    radio2n3.Checked = True
        'End If

    End Sub
End Class