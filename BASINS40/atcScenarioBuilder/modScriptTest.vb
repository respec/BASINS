Imports atcCligen
Imports atcData
Imports atcUtility
Imports MapWinUtility

Module modScriptTest

  Public Function BuiltInVariationScript(ByVal aVariationTemplate As Variation) As Variation
    Dim lNewVariation As New CliGenVariation
    lNewVariation.XML = aVariationTemplate.XML
    Return lNewVariation
  End Function

  Private Class CliGenVariation
    Inherits Variation

    Dim pBaseParmFileName As String = "C:\test\Climate\CliGen\WashNat.par"
    Dim pTempParmFileName As String = "C:\test\Climate\CliGen\WashNat.temp.par"
    Dim pTempOutputFileName As String = "C:\test\Climate\CliGen\WashNat.dat"
    Dim pParmToVary As String = "P(W/D)"
    Dim pStartYear As Integer = 1985
    Dim pNumYears As Integer = 4

    Protected Overrides Function VaryData() As atcData.atcDataGroup
      Dim lHeader As String
      Dim lFooter As String
      Dim lTable As atcTableFixed
      Dim lArgs As New atcDataAttributes
      Dim lCliGen As New atcCligen.atcCligen
      If ReadParmFile(pBaseParmFileName, lHeader, lTable, lFooter) Then
        Dim lNewVal As Double
        lTable.FindFirst(1, pParmToVary)
        For iMon As Integer = 2 To 13
          'Vary each monthly value unless it is in a non-selected season
          If Me.Seasons Is Nothing OrElse Me.Seasons.SeasonSelected(Me.Seasons.SeasonIndex(Jday(pStartYear, iMon - 1, 1, 0, 1, 0))) Then
            UpdateParmTable(lTable, pParmToVary, iMon, CStr(CDbl(lTable.Value(iMon)) * CurrentValue))
          End If
        Next
        WriteParmFile(pTempParmFileName, lHeader, lTable, lFooter)

        lArgs.SetValue("CliGen Parm", pTempParmFileName)
        lArgs.SetValue("CliGen Out", pTempOutputFileName)
        lArgs.SetValue("Start Year", pStartYear)
        lArgs.SetValue("Num Years", pNumYears)
        lArgs.SetValue("Include Daily", False)
        lArgs.SetValue("Include Hourly", True)
        lCliGen.Open("Run CliGen", lArgs)
        For lDataSetIndex As Integer = 0 To Me.DataSets.Count - 1
          Dim lDataSet As atcTimeseries = Me.DataSets(lDataSetIndex)
          Dim lCligenConstituent As String = ""
          Select Case lDataSet.Attributes.GetValue("Constituent")
            Case "AIRTMP" : lCligenConstituent = "ATMP"
            Case "HPRECIP" : lCligenConstituent = "HPCP"
              'Case "PET" : lCligenConstituent = "EVAP"
              'Case "" : lCligenConstituent = "WIND"
              'Case "" : lCligenConstituent = "DEWP"
              'Case "" : lCligenConstituent = "SOLR"
          End Select
          If lCligenConstituent.Length > 0 Then
            For Each lCligenDataSet As atcTimeseries In lCliGen.DataSets
              If lCligenDataSet.Attributes.GetValue("Constituent") = lCligenConstituent Then
                lCligenDataSet.Attributes.SetValue("ID", lDataSet.Attributes.GetValue("ID"))
                lCligenDataSet.Attributes.SetValue("Constituent", lDataSet.Attributes.GetValue("Constituent"))
                Me.DataSets.SetRange(lDataSetIndex, New atcCollection(lCligenDataSet))
              End If
            Next
          End If
        Next
        Return Me.DataSets
      End If
    End Function

  End Class
End Module
