Imports System.ComponentModel
Imports RDotNet

Public Class frmRunR


    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        Try
            Dim lMsg As String = ""
            Dim lEngine As REngine = GetEngine(lMsg)
            If lEngine IsNot Nothing Then
                lEngine.Evaluate(txtR.Text)
                SetIntegerVectorFromString(lEngine, "InputYears", txtYears.Text)
                SetDoubleVectorFromString(lEngine, "InputValues", txtValues.Text)
                Dim lConfidencePercent As Double = 95
                Double.TryParse(txtConfidence.Text, lConfidencePercent)
                lEngine.SetSymbol("InputPercentConfidence", lEngine.CreateNumeric(lConfidencePercent))
                lEngine.SetSymbol("InputWhat", lEngine.CreateInteger(ComboResult.SelectedIndex))
                Dim lResult As String() = lEngine.Evaluate("fnGetTrend(InputYears, InputValues, InputPercentConfidence, InputWhat)").AsCharacter().ToArray()
                'Dim lResult As String() = lEngine.Evaluate("InputYears").AsCharacter().ToArray()
                MsgBox(String.Join(Environment.NewLine, lResult), MsgBoxStyle.OkOnly, "Result")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Error running R")
        End Try
    End Sub

    Private Sub frmRunR_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        DisposeEngine()
    End Sub

    Private Sub frmRunR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboResult.SelectedIndex = 0
        btnRun.Focus()
    End Sub
End Class
