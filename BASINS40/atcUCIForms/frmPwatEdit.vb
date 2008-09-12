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
    'Dim myTable As HspfTable
    'Dim cVals&()
    'Dim initing As Boolean

    'Private Sub chkHigh_Click()
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub

    'Private Sub chkSnow_Click()
    '    StoreChanges(cmbLand.ListIndex)
    'End Sub

    'Private Sub cmbLand_Click()
    '    Dim i&, j&

    '    initing = True
    '    'after user changed combo list, use to refresh values
    '    If chkAssign.Value <> 1 Then
    '        'assign to all is not checked
    '        j = cmbLand.ListIndex * myTable.Parms.Count
    '        chkSnow.Value = cVals(j + 1)
    '        If cVals(j + 2) = 1 Then
    '            opt1(0).Value = True
    '        ElseIf cVals(j + 2) = 0 Then
    '            opt2(0).Value = True
    '        ElseIf cVals(j + 2) = 2 Then
    '            opt3(0).Value = True
    '        ElseIf cVals(j + 2) = 3 Then
    '            opt4(0).Value = True
    '        End If
    '        If cVals(j + 3) = 1 Then
    '            opt1(1).Value = True
    '        Else
    '            opt2(1).Value = True
    '        End If
    '        For i = 4 To 9
    '            If cVals(j + i) = 1 Then
    '                opt2(i - 2).Value = True
    '            Else
    '                opt1(i - 2).Value = True
    '            End If
    '        Next i
    '        If cVals(j + 10) = 1 Then
    '            opt1(8).Value = True
    '        Else
    '            opt2(8).Value = True
    '        End If
    '        chkHigh.Value = cVals(j + 11)
    '        If cVals(j + 12) = 0 Then
    '            opt1(10).Value = True
    '        ElseIf cVals(j + 12) = 1 Then
    '            opt2(10).Value = True
    '        ElseIf cVals(j + 12) = 2 Then
    '            opt3(1).Value = True
    '        ElseIf cVals(j + 12) = 3 Then
    '            opt4(1).Value = True
    '        End If
    '    End If
    '    initing = False
    'End Sub

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

    Public Sub Init(ByVal aParent As Windows.Forms.Form)
        Me.Icon = aParent.ParentForm.Icon
        Me.Text = "PWATER Simulation and Input Options"

        'Dim vOpn As Object
        'Dim lOpn As HspfOperation
        'Dim i As Long
        'Dim j As Long
        'Dim tempTable As HspfTable

        'cmbLand.Clear()
        'i = 0
        'initing = True
        'For Each vOpn In myTable.Opn.Uci.OpnBlks("PERLND").Ids
        '    lOpn = vOpn
        '    cmbLand.AddItem(lOpn.Description & " (" & lOpn.Id & ")")
        '    cmbLand.ItemData(i) = lOpn.Id
        '    i = i + 1
        '    'save a local copy of the values
        '    tempTable = lOpn.Tables(myTable.Name)
        '    ReDim Preserve cVals(i * tempTable.Parms.Count)
        '    For j = 1 To tempTable.Parms.Count
        '        cVals(((i - 1) * tempTable.Parms.Count) + j) = tempTable.Parms(j)
        '    Next j
        'Next vOpn
        'cmbLand.ListIndex = 0
        'initing = False
    End Sub
    'Public Sub init(ByVal O As Object, ByVal icon As StdPicture)
    '    myIcon = icon
    '    myTable = O
    'End Sub

    'Private Sub Form_Resize()
    '    Dim i As Long
    '    If Width > 1200 Then
    '        For i = 0 To 9
    '            fraOption(i).Width = Width / 2 - 228
    '        Next i
    '        chkSnow.Width = Width / 2 - 228
    '        chkHigh.Width = Width / 2 - 228
    '        For i = 0 To 10
    '            If i <> 9 Then
    '                opt1(i).Width = fraOption(0).Width - 500
    '                opt2(i).Width = fraOption(0).Width - 500
    '            End If
    '        Next i
    '        For i = 0 To 1
    '            opt3(i).Width = fraOption(0).Width - 500
    '            opt4(i).Width = fraOption(0).Width - 500
    '        Next i
    '        For i = 5 To 9
    '            fraOption(i).Left = Width / 2
    '        Next i
    '        chkHigh.Left = Width / 2
    '        cmdOk.Left = Width / 2 - cmdOk.Width - 200
    '        cmdCancel.Left = Width / 2 + 100
    '    End If
    'End Sub

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

    'Private Sub StoreChanges(ByVal ioper&)
    '    Dim i As Long
    '    Dim j As Long

    '    If Not initing Then
    '        j = ioper * myTable.Parms.Count

    '        cVals(j + 1) = chkSnow.Value
    '        If opt1(0).Value = True Then
    '            cVals(j + 2) = 1
    '        ElseIf opt2(0).Value = True Then
    '            cVals(j + 2) = 0
    '        ElseIf opt3(0).Value = True Then
    '            cVals(j + 2) = 2
    '        ElseIf opt4(0).Value = True Then
    '            cVals(j + 2) = 3
    '        End If
    '        If opt1(1).Value = True Then
    '            cVals(j + 3) = 1
    '        Else
    '            cVals(j + 3) = 0
    '        End If
    '        For i = 4 To 9
    '            If opt2(i - 2).Value = True Then
    '                cVals(j + i) = 1
    '            Else
    '                cVals(j + i) = 0
    '            End If
    '        Next i
    '        If opt1(8).Value = True Then
    '            cVals(j + 10) = 1
    '        Else
    '            cVals(j + 10) = 2
    '        End If
    '        cVals(j + 11) = chkHigh.Value
    '        If opt1(10).Value = True Then
    '            cVals(j + 12) = 0
    '        ElseIf opt2(10).Value = True Then
    '            cVals(j + 12) = 1
    '        ElseIf opt3(1).Value = True Then
    '            cVals(j + 12) = 2
    '        ElseIf opt4(1).Value = True Then
    '            cVals(j + 12) = 3
    '        End If
    '    End If
    'End Sub

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

End Class
