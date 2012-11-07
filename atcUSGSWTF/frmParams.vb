Imports atcData

Public Class frmParams

    Public Event ParameterChanged(ByVal aArgs As atcDataAttributes)
    Private pMethod As AntecedentGWLMethod
    Private pArgs As atcDataAttributes
    Public Sub Initialize(ByVal aMethod As AntecedentGWLMethod, ByVal aArgs As atcDataAttributes)
        pMethod = aMethod
        pArgs = aArgs
        Select Case aMethod
            Case AntecedentGWLMethod.FALL
                gbFall.Dock = Windows.Forms.DockStyle.Fill
                gbFall.Visible = True
                gbLinear.Visible = False
                gbPower.Visible = False

                If aArgs IsNot Nothing Then
                    txtFallD.Text = aArgs.GetValue("FallD", "")
                    txtFallKgw.Text = aArgs.GetValue("FallKgw", "")
                End If
            Case AntecedentGWLMethod.Linear
                gbLinear.Dock = Windows.Forms.DockStyle.Fill
                gbFall.Visible = False
                gbLinear.Visible = True
                gbPower.Visible = False
                If aArgs IsNot Nothing Then
                    txtLinearA.Text = aArgs.GetValue("LinearA", "")
                    txtLinearB.Text = aArgs.GetValue("LinearB", "")
                End If
            Case AntecedentGWLMethod.Power
                gbPower.Dock = Windows.Forms.DockStyle.Fill
                gbFall.Visible = False
                gbLinear.Visible = False
                gbPower.Visible = True
                If aArgs IsNot Nothing Then
                    txtPowerDatum.Text = aArgs.GetValue("PowerEDatum", "")
                    txtPowerExp.Text = aArgs.GetValue("PowerFExp", "")
                    txtPowerIntercept.Text = aArgs.GetValue("PowerCIntercept", "")
                    txtPowerMultiplier.Text = aArgs.GetValue("PowerDMultiplier", "")
                End If
        End Select

        Me.Show()
    End Sub

    Private Sub Parameter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtFallD.TextChanged, _
        txtFallKgw.TextChanged, _
        txtLinearA.TextChanged, _
        txtLinearB.TextChanged, _
        txtPowerDatum.TextChanged, _
        txtPowerExp.TextChanged, _
        txtPowerIntercept.TextChanged, _
        txtPowerMultiplier.TextChanged


    End Sub

    Private Sub frmParms_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If pArgs Is Nothing Then pArgs = New atcDataAttributes()
        Select Case pMethod
            Case AntecedentGWLMethod.FALL
                Dim lD As Double
                Dim lKgw As Double
                If Double.TryParse(txtFallD.Text, lD) Then pArgs.SetValue("FallD", lD)
                If Double.TryParse(txtFallKgw.Text, lKgw) Then pArgs.SetValue("FallKgw", lKgw)
            Case AntecedentGWLMethod.Linear
                Dim linearA As Double
                Dim linearB As Double

                If Double.TryParse(txtLinearA.Text, linearA) Then pArgs.SetValue("LinearA", linearA)
                If Double.TryParse(txtLinearB.Text, linearB) Then pArgs.SetValue("LinearB", linearB)

            Case AntecedentGWLMethod.Power
                Dim lPowerDatum As Double
                Dim lPowerExp As Double
                Dim lPowerIntercept As Double
                Dim lPowerMultiplier As Double

                If Double.TryParse(txtPowerDatum.Text, lPowerDatum) Then pArgs.SetValue("PowerEDatum", lPowerDatum)
                If Double.TryParse(txtPowerExp.Text, lPowerExp) Then pArgs.SetValue("PowerFExp", lPowerExp)
                If Double.TryParse(txtPowerIntercept.Text, lPowerIntercept) Then pArgs.SetValue("PowerCIntercept", lPowerIntercept)
                If Double.TryParse(txtPowerMultiplier.Text, lPowerMultiplier) Then pArgs.GetValue("PowerDMultiplier", lPowerMultiplier)
        End Select

        RaiseEvent ParameterChanged(pArgs)
    End Sub
End Class