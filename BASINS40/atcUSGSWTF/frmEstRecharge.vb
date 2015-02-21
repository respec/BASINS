Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmEstRecharge

    Private pFall As clsWTFFall
    Private pLinear As clsWTFLinear
    Private pPower As clsWTFPower

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pFall = New clsWTFFall()
        pLinear = New clsWTFLinear()
        pPower = New clsWTFPower()
        gbFall.Visible = False
        gbLinear.Visible = False
        gbPower.Visible = False
    End Sub
    'Private Sub ParameterChanged(ByVal aArgs As atcDataAttributes) Handles FfrmParam.ParameterChanged
    '    If rdoAntMethodFall.Checked Then
    '        With CType(pWTF, clsWTFFall)
    '            Dim lV1 As Double
    '            If Not Double.IsNaN(aArgs.GetValue("FallD")) AndAlso Double.TryParse(aArgs.GetValue("FallD"), lV1) AndAlso _
    '               Not Double.IsNaN(aArgs.GetValue("FallKgw")) AndAlso Double.TryParse(aArgs.GetValue("FallKgw"), lV1) Then
    '                .GWLAsymptote = aArgs.GetValue("FallD")
    '                .KGWL = aArgs.GetValue("FallKgw")
    '                .ParametersSet = True
    '            Else
    '                .ParametersSet = False
    '            End If
    '        End With
    '    ElseIf rdoAntMethodLinear.Checked Then
    '        With CType(pWTF, clsWTFLinear)
    '            Dim lV1 As Double
    '            If Not Double.IsNaN(aArgs.GetValue("LinearA")) AndAlso Double.TryParse(aArgs.GetValue("LinearA"), lV1) AndAlso _
    '               Not Double.IsNaN(aArgs.GetValue("LinearB")) AndAlso Double.TryParse(aArgs.GetValue("LinearB"), lV1) Then
    '                .LinearSlope = aArgs.GetValue("LinearA")
    '                .LinearIntercept = aArgs.GetValue("LinearB")
    '                CType(pWTF, clsWTFLinear).ParametersSet = True
    '            Else
    '                CType(pWTF, clsWTFLinear).ParametersSet = False
    '            End If
    '        End With
    '    ElseIf rdoAntMethodPower.Checked Then
    '        With CType(pWTF, clsWTFPower)
    '            Dim lV1 As Double
    '            If Not Double.IsNaN(aArgs.GetValue("PowerCIntercept")) AndAlso Double.TryParse(aArgs.GetValue("PowerCIntercept"), lV1) AndAlso _
    '               Not Double.IsNaN(aArgs.GetValue("PowerDMultiplier")) AndAlso Double.TryParse(aArgs.GetValue("PowerDMultiplier"), lV1) AndAlso _
    '               Not Double.IsNaN(aArgs.GetValue("PowerEDatum")) AndAlso Double.TryParse(aArgs.GetValue("PowerEDatum"), lV1) AndAlso _
    '               Not Double.IsNaN(aArgs.GetValue("PowerFExp")) AndAlso Double.TryParse(aArgs.GetValue("PowerFExp"), lV1) Then
    '                .ParamCIntercept = aArgs.GetValue("PowerCIntercept")
    '                .ParamDMultiplier = aArgs.GetValue("PowerDMultiplier")
    '                .ParamEDatum = aArgs.GetValue("PowerEDatum")
    '                .ParamFExp = aArgs.GetValue("PowerFExp")
    '                CType(pWTF, clsWTFPower).ParametersSet = True
    '            Else
    '                CType(pWTF, clsWTFPower).ParametersSet = False
    '            End If
    '        End With
    '    End If
    'End Sub

    Private Sub txtSy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lV As Double
        Dim lSyText As String = txtSy.Text.Trim()
        If Not Double.IsNaN(lSyText) AndAlso Double.TryParse(lSyText, lV) Then
            pFall.SpecificYield = lV
        End If
    End Sub

    Private Sub chkAntMethod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAntMethodFall.Click, _
    chkAntMethodLinear.Click, _
    chkAntMethodPower.Click
        gbFall.Visible = False
        gbLinear.Visible = False
        gbPower.Visible = False
        Dim lchkBoxName As String = CType(sender, Windows.Forms.CheckBox).Name
        Dim lMethod As AntecedentGWLMethod = -9
        If String.Compare(lchkBoxName, "chkAntMethodFall", True) = 0 Then
            gbFall.Dock = Windows.Forms.DockStyle.Fill
            gbFall.Visible = True
            lMethod = AntecedentGWLMethod.FALL
        ElseIf String.Compare(lchkBoxName, "chkAntMethodLinear", True) = 0 Then
            gbLinear.Dock = Windows.Forms.DockStyle.Fill
            gbLinear.Visible = True
            lMethod = AntecedentGWLMethod.Linear
        ElseIf String.Compare(lchkBoxName, "chkAntMethodPower", True) = 0 Then
            gbPower.Dock = Windows.Forms.DockStyle.Fill
            gbPower.Visible = True
            lMethod = AntecedentGWLMethod.Power
        End If
        Select Case lMethod
            Case AntecedentGWLMethod.FALL
                If pFall Is Nothing Then pFall = New clsWTFFall()
                txtFallD.Text = pFall.GWLAsymptote.ToString()
                txtFallKgw.Text = pFall.KGWL.ToString()
            Case AntecedentGWLMethod.Linear
                If pLinear Is Nothing Then pLinear = New clsWTFLinear()
                txtLinearA.Text = pLinear.LinearIntercept.ToString
                txtLinearB.Text = pLinear.LinearSlope.ToString
            Case AntecedentGWLMethod.Power
                If pPower Is Nothing Then pPower = New clsWTFPower()
                txtPowerDatum.Text = pPower.ParamEDatum.ToString
                txtPowerExp.Text = pPower.ParamFExp.ToString
                txtPowerIntercept.Text = pPower.ParamCIntercept.ToString
                txtPowerMultiplier.Text = pPower.ParamDMultiplier.ToString
        End Select
    End Sub
End Class