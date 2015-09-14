Public Class WaterQualityUtility
    '''' <summary>
    '''' Given a USGS Hyrologic Unit (HUC2 or HUC8) ID, 
    '''' a USGS Station Type, and 
    '''' a USGS Water Quality Parameter Code
    '''' This method returns a string containing Parameter values
    '''' </summary>
    '''' <param name="HUC"></param>
    '''' <param name="ParameterCode"></param>
    '''' <param name="stationType"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    '<CLSCompliant(False)> _
    'Public Function GetNWISStats( _
    '                ByVal HUC As String, _
    '                ByVal ParameterCode As String, _
    '                ByVal StationType As EDDTLib.NWIS.StationType) As String

    '    Dim lEDDTNWIS As New EDDTLib.NWIS
    '    Dim lStations As String = lEDDTNWIS.getStations(HUC, StationType, ParameterCode)
    '    Dim myXML As String = ""

    '    If (lStations = "No stations found that match request.") Then
    '        Return getEmptyXML(HUC)
    '    End If
    '    Dim arStations() As String = lStations.Split(vbCrLf)
    '    Dim station As String = lStations
    '    Dim idx As Integer = 0
    '    Dim results As String = ""
    '    Dim cntStations As Integer = 0
    '    Dim tempResult As String = ""
    '    While (lStations.IndexOf(vbCr) >= 0)
    '        idx = lStations.IndexOf(vbCr)
    '        station = lStations.Substring(0, idx + 2)
    '        lStations = lStations.Substring((idx + 2), (lStations.Length - (idx + 2)))
    '        idx = station.IndexOf(vbCr)
    '        station = station.Remove(idx, 2)
    '        tempResult = GetWQValues(station, ParameterCode, "01-01-1900", DateTime.Now().Date().ToString())
    '        If Not tempResult.StartsWith("No") Then
    '            results &= tempResult
    '            cntStations += 1
    '        End If
    '    End While

    '    If cntStations = 0 Then
    '        Return getEmptyXML(HUC)
    '    End If

    '    'char[] delimiterChars = {','};
    '    Dim words() As String
    '    Dim value As String
    '    Dim alValues As System.Collections.ArrayList = New System.Collections.ArrayList()
    '    While (results.IndexOf(vbCr) >= 0)
    '        idx = results.IndexOf(vbCr)
    '        value = results.Substring(0, idx + 2)
    '        results = results.Substring((idx + 2), (results.Length - (idx + 2)))
    '        idx = value.IndexOf(vbCr)
    '        value = value.Remove(idx, 2)
    '        words = value.Split(",")
    '        alValues.Add(words(1))
    '    End While
    '    Dim count As Integer = alValues.Count

    '    If count = 0 Then
    '        Return getEmptyXML(HUC)
    '    End If

    '    Dim arValues As System.Array = System.Array.CreateInstance(GetType(Double), count)
    '    Dim dblValue As Double = 0
    '    Dim sum As Double = 0
    '    For i As Integer = 0 To count - 1
    '        dblValue = Convert.ToDouble(alValues(i))
    '        arValues.SetValue(dblValue, i)
    '        sum += dblValue
    '    Next
    '    Array.Sort(arValues)
    '    Dim avgValue As Double = sum / count
    '    Dim Percentile95 As Double = CDbl(arValues.GetValue(CInt(count * 0.95)))
    '    Dim Percentile5 As Double = CDbl(arValues.GetValue(CInt(count * 0.05)))
    '    Dim median As Double = CDbl(arValues.GetValue(CInt(count * 0.5)))

    '    Dim units As String = GetWQParamUnits(ParameterCode)
    '    Return "<HUC>" & HUC & "</HUC>" _
    '         & "<NumStations>" & cntStations & "</NumStations>" _
    '         & "<Units>" & units & "</Units>" _
    '         & "<Statistics>" _
    '         & "<Count>" & count & "</Count>" _
    '         & "<Maximum>" & arValues.GetValue(count - 1).ToString() & "</Maximum>" _
    '         & "<Minimum>" & arValues.GetValue(0).ToString() & "</Minimum>" _
    '         & "<Mean>" & avgValue & "</Mean>" _
    '         & "<Median>" & median & "</Median>" _
    '         & "<Percentile5>" & Percentile5 & "</Percentile5>" _
    '         & "<Percentile95>" & Percentile95 & "</Percentile95>" _
    '         & "</Statistics>"
    'End Function

    'Private Function getEmptyXML(ByVal HUC As String) As String
    '    Return "<HUC>" & HUC & "</HUC>" _
    '         & "<NumStations>" & "0" & "</NumStations>" _
    '         & "<Units>" & "unknown" & "</Units>" _
    '         & "<Statistics>" _
    '         & "<Count>" & "0" & "</Count>" _
    '         & "<Maximum>" & "0" & "</Maximum>" _
    '         & "<Minimum>" & "0" & "</Minimum>" _
    '         & "<Mean>" & "0" & "</Mean>" _
    '         & "<Median>" & "0" & "</Median>" _
    '         & "<Percentile5>" & "0" & "</Percentile5>" _
    '         & "<Percentile95>" & "0" & "</Percentile95>" _
    '         & "</Statistics>"
    'End Function

End Class
