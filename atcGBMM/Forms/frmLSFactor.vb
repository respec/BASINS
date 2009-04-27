Friend Class FrmLSFactor
    Inherits System.Windows.Forms.Form

    'UPGRADE_WARNING: Event chkMaxSlope.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkMaxSlope_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkMaxSlope.CheckStateChanged
        If chkMaxSlope.CheckState = 1 Then
            If optionDefaultLSFactor.Checked = False Then optionDefaultLSFactor.Checked = True
            txtMaxSlope.Enabled = True
        Else
            txtMaxSlope.Enabled = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        'Unload the dialog box
        boolContinueLSFactor = False
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click

        Dim userChoice As Object
        If Not optionExisting.Checked Then
            'UPGRADE_WARNING: Couldn't resolve default property of object userChoice. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            userChoice = MsgBox("LS Factor calculation may take long time. Do you want to continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Warning ....")
            If userChoice = MsgBoxResult.No Then
                boolContinueLSFactor = False
                Me.Close()
                Exit Sub
            End If
        End If
        IsMaxSlopeLength = False
        MaxSlopeLength = 0
        If optionDefaultLSFactor.Checked = True And chkMaxSlope.CheckState = 1 Then
            If Not IsNumeric(txtMaxSlope.Text) Then
                Exit Sub
            Else
                IsMaxSlopeLength = True
                MaxSlopeLength = CShort(txtMaxSlope.Text)
            End If
        End If

        If optionExisting.Checked = True Then
            'Unload the dialog box
            skipLSfactor = True
            boolContinueLSFactor = True
            Me.Close()
        Else
            skipLSfactor = False
            If (optionDefineSlopeLength.Checked = True And Trim(txtCSL.Text) = "") Then
                MsgBox("Define Slope Length value.", MsgBoxStyle.Exclamation)
                Exit Sub
            Else
                'Call the subroutine to get the values
                GetLSFactorParameters()
                boolContinueLSFactor = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub FrmLSFactor_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        If LSexists() Then
            optionExisting.Enabled = True
        Else
            optionExisting.Enabled = False
        End If
    End Sub

    'UPGRADE_WARNING: Event optionDefaultLSFactor.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optionDefaultLSFactor_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionDefaultLSFactor.CheckedChanged
        If eventSender.Checked Then
            txtCSL.Enabled = False
            chkMaxSlope.Enabled = True
        End If
    End Sub

    'UPGRADE_WARNING: Event optionDefineSlopeLength.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optionDefineSlopeLength_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionDefineSlopeLength.CheckedChanged
        If eventSender.Checked Then
            txtCSL.Enabled = True
        End If
    End Sub

    'UPGRADE_WARNING: Event optionExisting.CheckedChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub optionExisting_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionExisting.CheckedChanged
        If eventSender.Checked Then
            txtCSL.Enabled = False
        End If
    End Sub

    'UPGRADE_WARNING: Event txtCSL.TextChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub txtCSL_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtCSL.TextChanged
        If (optionDefineSlopeLength.Checked = False) Then
            optionDefineSlopeLength.Checked = True
        End If
    End Sub

    Public Function LSexists() As Boolean
        ' check if raster dataset exist
        Dim fsObj As Scripting.FileSystemObject
        fsObj = CreateObject("Scripting.FileSystemObject")

        If (fsObj.FolderExists(gMapTempFolder & "\lsfactor") And fsObj.FolderExists(gMapTempFolder & "\slope")) Then
            LSexists = True
        Else
            LSexists = False
        End If

        'UPGRADE_NOTE: Object fsObj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        fsObj = Nothing
    End Function
End Class