Imports atcUCI
Imports atcControls
Imports System

Public Class frmPwatEdit

    Dim lMyTable As HspfTable
    Dim lOptionVals As New ArrayList
    Dim lIdList As New ArrayList
    Dim lPrevCboLandIndex As Integer

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLand.SelectedIndexChanged
        Dim lOper As Integer

        StoreChanges(lPrevCboLandIndex)
        'after user changed combo list, use to refresh values
        If chkAssign.Checked = False Then
            'assign to all is not checked
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
            ElseIf lOptionVals(lOper + 1) = 0 Then
                radio1n2.Checked = True
            ElseIf lOptionVals(lOper + 1) = 2 Then
                radio1n3.Checked = True
            ElseIf lOptionVals(lOper + 1) = 3 Then
                radio1n4.Checked = True
            End If

            'UZFG
            If lOptionVals(lOper + 2) = 1 Then
                radio2n1.Checked = True
            Else
                radio2n2.Checked = True
            End If

            'VCSFG
            If lOptionVals(lOper + 3) = 1 Then
                radio3n2.Checked = True
            Else
                radio3n1.Checked = True
            End If

            'VUZFG
            If lOptionVals(lOper + 4) = 1 Then
                radio4n2.Checked = True
            Else
                radio4n1.Checked = True
            End If

            'VMNFG
            If lOptionVals(lOper + 5) = 1 Then
                radio5n2.Checked = True
            Else
                radio5n1.Checked = True
            End If

            'VIFWFG
            If lOptionVals(lOper + 6) = 1 Then
                radio6n2.Checked = True
            Else
                radio6n1.Checked = True
            End If

            'VIRCFG
            If lOptionVals(lOper + 7) = 1 Then
                radio7n2.Checked = True
            Else
                radio7n1.Checked = True
            End If

            'VLEFG
            If lOptionVals(lOper + 8) = 1 Then
                radio8n2.Checked = True
            Else
                radio8n1.Checked = True
            End If

            'IFFCFG
            If lOptionVals(lOper + 9) = 1 Then
                radio9n1.Checked = True
            Else
                radio9n2.Checked = True
            End If

            'HWTFG
            If lOptionVals(lOper + 10) = 1 Then
                chkHigh.Checked = True
            Else
                chkHigh.Checked = False
            End If

            'IRRGFG
            If lOptionVals(lOper + 11) = 0 Then
                radio10n1.Checked = True
            ElseIf lOptionVals(lOper + 11) = 1 Then
                radio10n2.Checked = True
            ElseIf lOptionVals(lOper + 11) = 2 Then
                radio10n3.Checked = True
            ElseIf lOptionVals(lOper + 11) = 3 Then
                radio10n4.Checked = True
            End If

        End If
        lPrevCboLandIndex = cboLand.SelectedIndex
    End Sub

    Public Sub Init(ByVal aParent As Object)
        Me.Text = "PWATER Simulation and Input Options"
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        lMyTable = aParent

        Dim lVOpn As Object
        Dim lOpn As HspfOperation
        Dim lOper As Integer
        Dim tempTable As HspfTable

        cboLand.Items.Clear()
        cboLand.Cursor = Windows.Forms.Cursors.Hand
        cboLand.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList

        For Each lVOpn In lMyTable.Opn.Uci.OpnBlks("PERLND").Ids
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

    Private Sub SaveChanges(ByVal lSaveIndex As Integer)
        Dim lOper1 As Integer
        Dim lOper2 As Integer
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable

        lOper2 = lSaveIndex * lMyTable.Parms.Count

        lOpn = lMyTable.Opn.Uci.OpnBlks("PERLND").OperFromID(lIdList(lSaveIndex))
        lTable = lOpn.Tables(lMyTable.Name)

        For lOper1 = 0 To lMyTable.Parms.Count - 1
            lTable.Parms(lOper1).Value = lOptionVals(lOper2 + lOper1)
        Next

    End Sub

    Private Sub StoreChanges(ByVal lStoreIndex As Long)
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
            ElseIf radio1n2.Checked = True Then
                lOptionVals(lOper + 1) = 0
            ElseIf radio1n3.Checked = True Then
                lOptionVals(lOper + 1) = 2
            ElseIf radio1n4.Checked = True Then
                lOptionVals(lOper + 1) = 3
            End If

            'UZFG
            If radio2n1.Checked = True Then
                lOptionVals(lOper + 2) = 1
            Else
                lOptionVals(lOper + 2) = 0
            End If

            'VCSFG
            If radio3n1.Checked = True Then
                lOptionVals(lOper + 3) = 0
            Else
                lOptionVals(lOper + 3) = 1
            End If

            'VUZFG
            If radio4n1.Checked = True Then
                lOptionVals(lOper + 4) = 0
            Else
                lOptionVals(lOper + 4) = 1
            End If

            'VMNFG
            If radio5n1.Checked = True Then
                lOptionVals(lOper + 5) = 0
            Else
                lOptionVals(lOper + 5) = 1
            End If

            'VIFWFG
            If radio6n1.Checked = True Then
                lOptionVals(lOper + 6) = 0
            Else
                lOptionVals(lOper + 6) = 1
            End If

            'VIRCFG
            If radio7n1.Checked = True Then
                lOptionVals(lOper + 7) = 0
            Else
                lOptionVals(lOper + 7) = 1
            End If

            'VLEFG
            If radio8n1.Checked = True Then
                lOptionVals(lOper + 8) = 0
            Else
                lOptionVals(lOper + 8) = 1
            End If

            'IFFCFG
            If radio9n2.Checked = True Then
                lOptionVals(lOper + 9) = 0
            Else
                lOptionVals(lOper + 9) = 1
            End If

            'HWTFG
            If chkHigh.Checked = True Then
                lOptionVals(lOper + 10) = 1
            Else
                lOptionVals(lOper + 10) = 0
            End If

            'IRRGFG
            If radio10n1.Checked = True Then
                lOptionVals(lOper + 11) = 0
            ElseIf radio10n2.Checked = True Then
                lOptionVals(lOper + 11) = 1
            ElseIf radio10n3.Checked = True Then
                lOptionVals(lOper + 11) = 2
            ElseIf radio10n4.Checked = True Then
                lOptionVals(lOper + 11) = 3
            End If

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
