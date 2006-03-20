Imports atcCligen
Imports atcData
Imports atcUtility

Module modScriptTest

  Public Function BuiltInVariationScript(ByVal aVariationTemplate As Variation) As Variation
    Dim lNewVariation As New CliGenVariation
    lNewVariation.XML = aVariationTemplate.XML
    Return lNewVariation
  End Function

  Private Class CliGenVariation
    Inherits Variation

    Dim pBaseParmFileName As String = "c:\test\atcCliGen\input\Ga090451.par"
    Dim pTempParmFileName As String = "c:\test\atcCliGen\input\Ga090451.temp.par"
    Dim pTempOutputFileName As String = "c:\test\atcCliGen\current\TestRun.dat"
    Dim pParmToVary As String = "SOL.RAD"
    Dim pStartYear As Integer = 1990
    Dim pNumYears As Integer = 10

    Protected Overrides Function VaryData() As atcData.atcDataGroup
      Dim lHeader As String
      Dim lFooter As String
      Dim lTable As atcTableFixed
      If ReadParmFile(pBaseParmFileName, lHeader, lTable, lFooter) Then
        Dim lNewVal As Double
        Dim lCliGen As New atcCligen.atcCligen
        Dim lCliGenArgs As New atcDataAttributes
        lTable.FindFirst(1, pParmToVary)
        For iMon As Integer = 2 To 13
          'Vary each monthly value unless it is in a non-selected season
          If Me.Seasons Is Nothing OrElse Me.Seasons.SeasonSelected(Me.Seasons.SeasonIndex(Jday(pStartYear, iMon - 1, 1, 0, 1, 0))) Then
            UpdateParmTable(lTable, pParmToVary, iMon, CStr(CDbl(lTable.Value(iMon)) * CurrentValue))
          End If
        Next
        WriteParmFile(pTempParmFileName, lHeader, lTable, lFooter)

        lCliGenArgs.SetValue("CliGen Parm", pTempParmFileName)
        lCliGenArgs.SetValue("CliGen Out", pTempOutputFileName)
        lCliGenArgs.SetValue("Start Year", pStartYear)
        lCliGenArgs.SetValue("Num Years", pNumYears)
        lCliGen.Open("", lCliGenArgs)

        For lDataSetIndex As Integer = 0 To Me.DataSets.Count
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
                Me.DataSets.SetRange(lDataSetIndex, New atcCollection(lCligenDataSet))
              End If
            Next
          End If
        Next
      End If
    End Function
  End Class
End Module
