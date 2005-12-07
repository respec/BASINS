Imports atcData
Imports atcUtility

Module modSWSTATRpt

  Function GenFreqReport(ByVal aTS As atcTimeseries, _
                         ByVal aNDay As Object, _
                         ByVal aHigh As Boolean, _
                         ByVal aRecurOrProb As Object) As String
    Dim lNDaySource As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
    Dim lNDayTSGroup As New atcDataGroup
    Dim ls As String
    ls = "Pearson Type III Statistics" & vbCrLf & _
         "SWSTAT(4.1)" & vbCrLf & _
         "(based on USGS Program A193)" & vbCrLf & vbCrLf & _
         "Notice -- Use of Pearson Type III distribution is for" & vbCrLf & _
         "preliminary(computations.User Is responsible)" & vbCrLf & _
         "for assessment and interpretation." & vbCrLf
    ls &= "The following 7 statistics are based on non-zero values:"
    lNDaySource.ComputeFreq(aTS, aNDay, aHigh, aRecurOrProb, False, Nothing, lNDayTSGroup)
    Return ls
  End Function

  Function GenNDayReport() As String
    Dim ls As String
    ls = "Station Number " & vbCrLf

    Return ls
  End Function

  Function GenTrendReport(ByVal aTS As atcTimeseries) As String
    Dim ls As String
    Dim ldate(5) As Integer
    ls = "Data-set number   =" & RightJustify(aTS.Attributes.GetValue("ID"), 5) & vbCrLf
    ls &= " Station number   = " & aTS.Attributes.GetValue("STAID") & vbCrLf
    ls &= " Station name     = " & aTS.Attributes.GetValue("Description") & vbCrLf
    ls &= " Data type        = " & aTS.Attributes.GetValue("TSTYPE") & vbCrLf
    J2Date(CDbl(aTS.Attributes.GetValue("SJDAY")), ldate)
    ls &= " Starting year    = " & ldate(0) & vbCrLf
    J2Date(CDbl(aTS.Attributes.GetValue("EJDAY")), ldate)
    ls &= " Ending year      = " & ldate(0) & vbCrLf
    ls &= " Values used      =" & RightJustify(aTS.numValues, 5) & vbCrLf
    ls &= " Values skipped   = " & vbCrLf
    ls &= " Kendall Tau      = " & aTS.Attributes.GetValue("Kendall Tau") & vbCrLf
    ls &= " P-level          = " & vbCrLf
    ls &= " Median slope     = " & vbCrLf
    Return ls
  End Function

End Module
