Friend Class frmSimulationNew
    Inherits System.Windows.Forms.Form

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub


    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        ComputeSedimentFlag = Me.cbxSediment.CheckState
        ComputeMercuryFlag = Me.cbxMercury.Checked
        ComputeWhAEMFlag = Me.cbxWhAEM.Checked
        ComputeWASPFlag = Me.cbxWASP.Checked
        'ComputeBalanceFlag = Me.cbxBalance.CheckState
        If modFormInteract.BlankCheck(Me) Then
            modFormInteract.WritetoDict(Me)
            modFormInteract.SaveDict()
            Me.Close()
        End If
    End Sub

    Private Sub frmSimulationNew_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'Read values from inputdata.txt
        modFormInteract.CreateFile()
        modFormInteract.Filer()
        modFormInteract.LoadForm(Me)

        'If cbxSediment.CheckState = 0 Then
        '    cbxMercury.CheckState = System.Windows.Forms.CheckState.Unchecked
        '    cbxSediment.CheckState = System.Windows.Forms.CheckState.Unchecked
        'Else
        '    cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
        '    cbxSediment.CheckState = System.Windows.Forms.CheckState.Checked
        'End If
        'If cbxMercury.CheckState = 1 Then
        '    cbxSediment.CheckState = System.Windows.Forms.CheckState.Checked
        '    cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
        'End If
    End Sub

    Private Sub cbxHydro_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxHydro.CheckedChanged, cbxSediment.CheckedChanged, cbxMercury.CheckedChanged
        'must be checked from left to right
        If sender Is cbxHydro Then
            If cbxHydro.Checked Then
            Else
                cbxSediment.Checked = False
                cbxMercury.Checked = False
            End If
        ElseIf sender Is cbxSediment Then
            If cbxSediment.Checked Then
                cbxHydro.Checked = True
            Else
                cbxMercury.Checked = False
            End If
        ElseIf sender Is cbxMercury Then
            If cbxMercury.Checked Then
                cbxHydro.Checked = True
                cbxSediment.Checked = True
            End If
        End If
        If Not cbxHydro.Checked Then
            cbxSediment.Checked = False
            cbxMercury.Checked = False
        ElseIf Not cbxSediment.Checked Then
            cbxMercury.Checked = False
        End If
    End Sub



    'Private Sub cbxHydro_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbxHydro.CheckStateChanged
    '    cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
    '    cbxSediment.CheckState = System.Windows.Forms.CheckState.Unchecked
    '    cbxMercury.CheckState = System.Windows.Forms.CheckState.Unchecked
    'End Sub

    'Private Sub cbxMercury_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbxMercury.CheckStateChanged
    '    If (cbxMercury.CheckState = 1) Then
    '        cbxSediment.CheckState = System.Windows.Forms.CheckState.Checked
    '        cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
    '    Else
    '        cbxMercury.CheckState = System.Windows.Forms.CheckState.Unchecked
    '    End If
    'End Sub

    'Private Sub cbxSediment_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbxSediment.CheckStateChanged
    '    If (cbxSediment.CheckState = 1) Then
    '        cbxHydro.CheckState = System.Windows.Forms.CheckState.Checked
    '    Else
    '        cbxMercury.CheckState = System.Windows.Forms.CheckState.Unchecked
    '    End If
    'End Sub

    'Private Sub cbxStandard_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbxStandard.CheckStateChanged
    '    If (cbxStandard.CheckState = 1) Then
    '        cbxWASP.CheckState = System.Windows.Forms.CheckState.Unchecked
    '    End If
    '    If (cbxStandard.CheckState = 0 And cbxWASP.CheckState = 0) Then
    '        cbxStandard.CheckState = System.Windows.Forms.CheckState.Checked
    '    End If
    'End Sub

    'Private Sub cbxWASP_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cbxWASP.CheckStateChanged
    '    If cbxWASP.Checked Then cbxStandard.Checked = False
    '    If Not (cbxStandard.Checked Or cbxWASP.Checked) Then cbxStandard.Checked = True
    'End Sub

    Private Sub cbxWASP_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxWASP.CheckedChanged, cbxWhAEM.CheckedChanged
        'not sure what cbxStandard does...
        'If sender Is cbxWASP Then
        '    cbxWhAEM.Checked = Not cbxWASP.Checked
        'Else
        '    cbxWASP.Checked = Not cbxWhAEM.Checked
        'End If
    End Sub
End Class