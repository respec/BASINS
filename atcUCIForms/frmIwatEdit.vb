Imports atcUCI
Imports atcControls
Imports System

Public Class frmIwatEdit

    Dim lMyTable As HspfTable
    Dim lOptionVals As New ArrayList
    Dim lIdList As New ArrayList
    Dim lPrevCboLandIndex As Integer

    Public Sub Init(ByVal aParent As Object)
        Me.Text = "IWATER Simulation and Input Options"
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        lMyTable = aParent

        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lOper As Integer
        Dim tempTable As HspfTable

        cboLand.Items.Clear()
        cboLand.Cursor = Windows.Forms.Cursors.Hand
        cboLand.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList

        For Each lVOpn In lMyTable.Opn.Uci.OpnBlks("IMPLND").Ids
            lOpn = lVOpn
            cboLand.Items.Add(lOpn.Description & " (" & lOpn.Id & ")")
            lIdList.Add(lOpn.Id)
            'save a local copy of the values
            tempTable = lOpn.Tables(lMyTable.Name)
            For lOper = 0 To tempTable.Parms.Count - 1
                lOptionVals.Add(tempTable.Parms(lOper).Value)
            Next
        Next

        cboLand.SelectedIndex = 0
    End Sub

    Private Sub cboLand_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLand.SelectedIndexChanged
        Dim lOper As Integer

        'after user changed combo list, use to refresh values
        If chkAssign.Checked = False Then

            lOper = cboLand.SelectedIndex * lMyTable.Parms.Count

            'CSNOFG
            If lOptionVals(lOper) = 1 Then
                chkSnow.Checked = True
            Else
                chkSnow.Checked = False
            End If

            'RTOPFG
            If lOptionVals(lOper + 1) = 1 Then
                radio1n1.Checked = True
            Else
                radio1n2.Checked = True
            End If

            'VRSFG (note: '1' is bottom radio)
            If lOptionVals(lOper + 2) = 1 Then
                radio2n2.Checked = True
            Else
                radio2n1.Checked = True
            End If

            'VMNFG (note: '1' is bottom radio)
            If lOptionVals(lOper + 3) = 1 Then
                radio3n2.Checked = True
            Else
                radio3n1.Checked = True
            End If

            'RTLIFG
            If lOptionVals(lOper + 4) = 1 Then
                chkLateral.Checked = True
            Else
                chkLateral.Checked = False
            End If

        End If

    End Sub


    Private Sub StoreChanges(ByVal lStoreIndex As Integer)
        Dim lOper As Integer

        lOper = lStoreIndex * lMyTable.Parms.Count

        'CSNOFG
        If chkSnow.Checked = True Then
            lOptionVals(lOper) = 1
        Else
            lOptionVals(lOper) = 0
        End If

        'RTOPFG
        If radio1n1.Checked = True Then
            lOptionVals(lOper + 1) = 1
        Else
            lOptionVals(lOper + 1) = 0
        End If

        'VRSFG (note: '1' is bottom radio)
        If radio2n1.Checked = True Then
            lOptionVals(lOper + 2) = 0
        Else
            lOptionVals(lOper + 2) = 1
        End If

        'VMNFG (note: '1' is bottom radio)
        If radio3n1.Checked = True Then
            lOptionVals(lOper + 3) = 0
        Else
            lOptionVals(lOper + 3) = 1
        End If

        'RTLIFG
        If chkLateral.Checked = True Then
            lOptionVals(lOper + 4) = 1
        Else
            lOptionVals(lOper + 4) = 0
        End If

    End Sub

    Private Sub SaveChanges(ByVal lSaveIndex As Integer)
        Dim lOper1 As Integer
        Dim lOper2 As Integer
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable

        lOper2 = lSaveIndex * lMyTable.Parms.Count
        lOpn = lMyTable.Opn.Uci.OpnBlks("IMPLND").OperFromID(lIdList(lSaveIndex))
        lTable = lOpn.Tables(lMyTable.Name)

        For lOper1 = 0 To lMyTable.Parms.Count - 1
            lTable.Parms(lOper1).Value = lOptionVals(lOper2 + lOper1)
        Next

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lOper As Integer

        If chkAssign.Checked = True Then
            For lOper = 0 To cboLand.Items.Count - 1
                StoreChanges(lOper)
            Next
        Else
            StoreChanges(cboLand.SelectedIndex)
        End If

        For lOper = 0 To cboLand.Items.Count - 1
            SaveChanges(lOper)
        Next

        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub
End Class