Imports ZedGraph

Public Module modGraph
    <CLSCompliant(False)> _
    Public Sub AddLine(ByRef aPane As ZedGraph.GraphPane, _
                       ByVal aACoef As Double, ByVal aBCoef As Double, _
                       Optional ByVal aLineStyle As Drawing.Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid)
        With aPane
            Dim lXValues(1) As Double
            Dim lYValues(1) As Double
            If aBCoef > 0 Then
                lXValues(0) = .XAxis.Scale.Min
                lYValues(0) = (aACoef * lXValues(0)) + aBCoef
            Else
                lYValues(0) = .YAxis.Scale.Min
                lXValues(0) = (lYValues(0) - aBCoef) / aACoef
            End If
            lXValues(1) = .XAxis.Scale.Max
            lYValues(1) = (aACoef * lXValues(1)) + aBCoef
            Dim lCurve As LineItem = .AddCurve("", lXValues, lYValues, Drawing.Color.Blue, SymbolType.None)
            lCurve.Line.Style = aLineStyle
            .CurveList.Add(lCurve)
        End With
    End Sub

    Public Sub SetGraphSpecs(ByRef aGraphForm As atcGraph.atcGraphForm, _
                             Optional ByRef aLabel1 As String = "Simulated", _
                             Optional ByRef aLabel2 As String = "Observed")
        aGraphForm.WindowState = Windows.Forms.FormWindowState.Maximized
        With aGraphForm.Pane
            .YAxis.MajorGrid.IsVisible = True
            .YAxis.MinorGrid.IsVisible = False
            .XAxis.MajorGrid.IsVisible = True
            With .CurveList(0)
                .Label.Text = aLabel1
                .Color = System.Drawing.Color.Red
            End With
            With .CurveList(1)
                .Label.Text = aLabel2
                .Color = System.Drawing.Color.Blue
            End With
        End With
        Windows.Forms.Application.DoEvents()
    End Sub
End Module
