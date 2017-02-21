Imports atcData

Friend Module mod_demo
    Public Sub CalculateSumPercentileWithMissingValues(ByVal aTSer As atcTimeseries)
        Dim lDatas As New ArrayList()
        For I As Integer = 0 To aTSer.numValues
            If Double.IsNaN(aTSer.Value(I)) OrElse aTSer.Value(I) = -999 Then
            Else
                lDatas.Add(aTSer.Value(I))
            End If
        Next
        Dim lTser As atcTimeseries = aTSer.Clone()
        lTser.Values = CType(lDatas.ToArray(GetType(Double)), Double())
        ReDim Preserve lTser.Dates.Values(lDatas.Count - 1)

        Dim lsum As Double = lTser.Attributes.GetValue("Sum") 'this is the same as the original
        Dim lsum05pct As Double = lTser.Attributes.GetValue("%Sum5")
        Dim lsum10pct As Double = lTser.Attributes.GetValue("%Sum10")
        Dim lsum15pct As Double = lTser.Attributes.GetValue("%Sum15")
        Dim lsum70pct As Double = lTser.Attributes.GetValue("%Sum70")
        Dim lsum90pct As Double = lTser.Attributes.GetValue("%Sum90")
    End Sub

End Module
