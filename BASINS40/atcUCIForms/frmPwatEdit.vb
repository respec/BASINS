Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcSegmentation
Imports atcControls
Imports System.Collections.ObjectModel
Imports System.IO
Imports System

Public Class frmPwatEdit

    'Private pParent As Windows.Forms.Form
    Dim myTable As HspfTable
    Dim cVals As New ArrayList
    Dim initing As Boolean


    Private Sub chkHigh_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHigh.Click
        'StoreChanges(cmbLand.ListIndex)
    End Sub


    Private Sub chkSnow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSnow.Click
        'StoreChanges(cmbLand.ListIndex)
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLand.SelectedIndexChanged
        Dim j As Integer
        initing = True
        'after user changed combo list, use to refresh values
        If chkAssign.Checked <> True Then
            'assign to all is not checked
            j = cboLand.SelectedIndex * myTable.Parms.Count

            'CSNOFG
            If cVals(j) = 1 Then
                chkSnow.Checked = True
            Else
                chkSnow.Checked = False
            End If


            'RTOPFG
            If cVals(j + 1) = 1 Then
                radio1n1.Checked = True
            ElseIf cVals(j + 1) = 0 Then
                radio1n2.Checked = True
            ElseIf cVals(j + 1) = 2 Then
                radio1n3.Checked = True
            ElseIf cVals(j + 1) = 3 Then
                radio1n4.Checked = True
            End If

            'UZFG
            If cVals(j + 2) = 1 Then
                radio2n1.Checked = True
            Else
                radio2n2.Checked = True
            End If

            'VCSFG
            If cVals(j + 3) = 1 Then
                radio3n2.Checked = True
            Else
                radio3n1.Checked = True
            End If

            'VUZFG
            If cVals(j + 4) = 1 Then
                radio4n2.Checked = True
            Else
                radio4n1.Checked = True
            End If

            'VMNFG
            If cVals(j + 5) = 1 Then
                radio5n2.Checked = True
            Else
                radio5n1.Checked = True
            End If

            'VIFWFG
            If cVals(j + 6) = 1 Then
                radio6n2.Checked = True
            Else
                radio6n1.Checked = True
            End If

            'VIRCFG
            If cVals(j + 7) = 1 Then
                radio7n2.Checked = True
            Else
                radio7n1.Checked = True
            End If

            'VLEFG
            If cVals(j + 8) = 1 Then
                radio8n2.Checked = True
            Else
                radio8n1.Checked = True
            End If

            'IFFCFG
            If cVals(j + 9) = 1 Then
                radio9n1.Checked = True
            Else
                radio9n2.Checked = True
            End If

            'HWTFG
            If cVals(j + 10) = 1 Then
                chkHigh.Checked = True
            Else
                chkHigh.Checked = False
            End If


            'IRRGFG
            If cVals(j + 11) = 0 Then
                radio10n1.Checked = True
            ElseIf cVals(j + 11) = 1 Then
                radio10n2.Checked = True
            ElseIf cVals(j + 11) = 2 Then
                radio10n3.Checked = True
            ElseIf cVals(j + 11) = 3 Then
                radio10n4.Checked = True
            End If

        End If
            initing = False
    End Sub

    'Private Sub cmdCancel_Click()
    '    Unload(Me)
    'End Sub

    'Private Sub cmdOK_Click()
    '    Dim i&, istart&, iend&, j&

    '    If chkAssign.Value = 1 Then
    '        For j = 0 To cmbLand.ListCount - 1
    '            StoreChanges(j)
    '        Next j
    '    Else
    '        StoreChanges(cmbLand.ListIndex)
    '    End If

    '    For j = 0 To cmbLand.ListCount - 1
    '        SaveChanges(j)
    '    Next j
    '    Unload(Me)
    'End Sub

    Public Sub Init(ByVal aParent As Object)
        'Me.Icon = aParent.ParentForm.Icon
        Me.Text = "PWATER Simulation and Input Options"
        myTable = aParent


        Dim vOpn As Object
        Dim lOpn As HspfOperation
        Dim j As Integer
        Dim tempTable As HspfTable

        cboLand.Items.Clear()
        initing = True
        For Each vOpn In myTable.Opn.Uci.OpnBlks("PERLND").Ids
            lOpn = vOpn
            cboLand.Items.Add(lOpn.Description & " (" & lOpn.Id & ")")
            cboLand.ValueMember = lOpn.Id
            'save a local copy of the values
            tempTable = lOpn.Tables(myTable.Name)
            For j = 0 To tempTable.Parms.Count - 1
                cVals.Add(tempTable.Parms(j).Value)
            Next
        Next

        cboLand.SelectedIndex = 0
        initing = False
    End Sub

    'Private Sub SaveChanges(ByVal ioper&)
    '    Dim i As Long
    '    Dim j As Long
    '    Dim lOpn As HspfOperation
    '    Dim ltable As HspfTable

    '    j = ioper * myTable.Parms.Count
    '    lOpn = myTable.Opn.Uci.OpnBlks("PERLND").OperFromID(cmbLand.ItemData(ioper))
    '    ltable = lOpn.Tables(myTable.Name)

    '    For i = 1 To myTable.Parms.Count
    '        ltable.Parms(i).Value = cVals(j + i)
    '    Next i

    'End Sub

    Private Sub StoreChanges(ByVal ioper As Long)
        Dim i As Long
        Dim j As Long
        i += 1
        If Not initing Then
            j = ioper * myTable.Parms.Count

            cVals(j + 1) = chkSnow.Checked
            If radio1n1.Checked = True Then
                cVals(j + 1) = 1
                'ElseIf opt2(0).Value = True Then
                '    cVals(j + 2) = 0
                'ElseIf opt3(0).Value = True Then
                '    cVals(j + 2) = 2
                'ElseIf opt4(0).Value = True Then
                '    cVals(j + 2) = 3
            End If
            '    If opt1(1).Value = True Then
            '        cVals(j + 3) = 1
            '    Else
            '        cVals(j + 3) = 0
            '    End If
            '    For i = 4 To 9
            '        If opt2(i - 2).Value = True Then
            '            cVals(j + i) = 1
            '        Else
            '            cVals(j + i) = 0
            '        End If
            '    Next i
            '    If opt1(8).Value = True Then
            '        cVals(j + 10) = 1
            '    Else
            '        cVals(j + 10) = 2
            '    End If
            '    cVals(j + 11) = chkHigh.Value
            '    If opt1(10).Value = True Then
            '        cVals(j + 12) = 0
            '    ElseIf opt2(10).Value = True Then
            '        cVals(j + 12) = 1
            '    ElseIf opt3(1).Value = True Then
            '        cVals(j + 12) = 2
            '    ElseIf opt4(1).Value = True Then
            '        cVals(j + 12) = 3
            '    End If
        End If
    End Sub

    'Private Sub opt1_Click(ByVal Index As Integer)
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub

    'Private Sub opt2_Click(ByVal Index As Integer)
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub

    'Private Sub opt3_Click(ByVal Index As Integer)
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub

    'Private Sub opt4_Click(ByVal Index As Integer)
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub



    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

    End Sub

End Class
