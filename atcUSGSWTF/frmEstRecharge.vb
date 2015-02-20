Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmEstRecharge
    Private pWTF As clsWTF

    Public Sub Initiate(ByVal aWTF As clsWTF)
        pWTF = aWTF
    End Sub
    Private Sub ParameterChanged(ByVal aArgs As atcDataAttributes) Handles FfrmParam.ParameterChanged
        If rdoAntMethodFall.Checked Then
            With CType(pWTF, clsWTFFall)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("FallD")) AndAlso Double.TryParse(aArgs.GetValue("FallD"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("FallKgw")) AndAlso Double.TryParse(aArgs.GetValue("FallKgw"), lV1) Then
                    .GWLAsymptote = aArgs.GetValue("FallD")
                    .KGWL = aArgs.GetValue("FallKgw")
                    .ParametersSet = True
                Else
                    .ParametersSet = False
                End If
            End With
        ElseIf rdoAntMethodLinear.Checked Then
            With CType(pWTF, clsWTFLinear)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("LinearA")) AndAlso Double.TryParse(aArgs.GetValue("LinearA"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("LinearB")) AndAlso Double.TryParse(aArgs.GetValue("LinearB"), lV1) Then
                    .LinearSlope = aArgs.GetValue("LinearA")
                    .LinearIntercept = aArgs.GetValue("LinearB")
                    CType(pWTF, clsWTFLinear).ParametersSet = True
                Else
                    CType(pWTF, clsWTFLinear).ParametersSet = False
                End If
            End With
        ElseIf rdoAntMethodPower.Checked Then
            With CType(pWTF, clsWTFPower)
                Dim lV1 As Double
                If Not Double.IsNaN(aArgs.GetValue("PowerCIntercept")) AndAlso Double.TryParse(aArgs.GetValue("PowerCIntercept"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerDMultiplier")) AndAlso Double.TryParse(aArgs.GetValue("PowerDMultiplier"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerEDatum")) AndAlso Double.TryParse(aArgs.GetValue("PowerEDatum"), lV1) AndAlso _
                   Not Double.IsNaN(aArgs.GetValue("PowerFExp")) AndAlso Double.TryParse(aArgs.GetValue("PowerFExp"), lV1) Then
                    .ParamCIntercept = aArgs.GetValue("PowerCIntercept")
                    .ParamDMultiplier = aArgs.GetValue("PowerDMultiplier")
                    .ParamEDatum = aArgs.GetValue("PowerEDatum")
                    .ParamFExp = aArgs.GetValue("PowerFExp")
                    CType(pWTF, clsWTFPower).ParametersSet = True
                Else
                    CType(pWTF, clsWTFPower).ParametersSet = False
                End If
            End With
        End If
    End Sub

    Private Sub txtSy_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lV As Double
        Dim lSyText As String = txtSy.Text.Trim()
        If Not Double.IsNaN(lSyText) AndAlso Double.TryParse(lSyText, lV) Then
            pWTF.SpecificYield = lV
        End If
    End Sub
End Class