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

        Dim lDatagroup As atcDataGroup = CliGenUpdate(pParmToVary, pTempParmFileName, pTempOutputFileName, pStartYear, pNumYears)
        Return lDatagroup
      End If
    End Function

    Public Function CliGenUpdate(ByVal aParmToVary As String, _
                                 ByVal aParmFileName As String, _
                                 ByVal aOutputFileName As String, _
                                 ByVal aStartYear As String, _
                                 ByVal aNumYears As Integer) As atcDataGroup

      Logger.Dbg("CliGenUpdate:Start2Vary:" & aParmToVary)
      Dim lCliGen As New atcCligen.atcCligen
      Dim lDisTS As New atcMetCmp.atcMetCmpPlugin
      Dim lts As atcTimeseries

      Dim lArgsMet As New atcDataAttributes
      Dim lMatch As New atcDataGroup
      Dim lErr As String = ""
      Dim lStr As String = ""

      Logger.Dbg("CliGenUpdate:FindDataManager")
      Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
      Logger.Dbg("CliGenUpdate:TsMath: " & lTsMath.ToString)

      Dim lDataManager As New atcDataManager(g_MapWin)
      'Dim lWDMfile As atcDataSource = New atcWDM.atcDataSourceWDM
      'Dim lWDMFileName As String = "base.wdm"
      'lDataManager.OpenDataSource(lWDMfile, lWDMFileName, Nothing)
      'Logger.Dbg("CliGenUpdate:DataManagerAfterWdmOpen: " & lDataManager.ToString)

      lArgsMet.Clear()
      Logger.Dbg("CliGenUpdate:Cleared ArgsMet")
      lArgsMet.SetValue("CliGen Parm", aParmFileName)
      lArgsMet.SetValue("CliGen Out", aOutputFileName)
      lArgsMet.SetValue("Start Year", aStartYear)
      lArgsMet.SetValue("Num Years", aNumYears)
      Logger.Dbg("CliGenUpdate: Set Args, about to run CliGen")
      lCliGen.Open("Run CliGen", lArgsMet)
      Logger.Dbg("CliGenUpdate: Ran CliGen!")

      Dim lTMax As atcTimeseries = Nothing
      Dim lTMin As atcTimeseries = Nothing
      Dim lATmp As atcTimeseries = Nothing
      Dim lDewPt As atcTimeseries = Nothing
      Dim lPrecip As atcTimeseries = Nothing
      Dim lPrecDur As atcTimeseries = Nothing
      Dim lPrecPkTime As atcTimeseries = Nothing
      Dim lPrecPkInten As atcTimeseries = Nothing
      For Each lDS As atcTimeseries In lCliGen.DataSets
        lStr = UCase(lDS.Attributes.GetValue("Constituent"))
        Select Case lStr
          Case "TMAX"
            Logger.Dbg("CliGenUpdate: Found CliGen TMax data")
            lArgsMet.Clear()
            lArgsMet.SetValue("Timeseries", lDS)
            lTsMath.DataSets.Clear()
            lDataManager.Clear()
            lDataManager.OpenDataSource(lTsMath, "Celsius to F", lArgsMet)
            lTMax = lDataManager.DataSets(0)
            Logger.Dbg("CliGenUpdate: Converted CliGen TMax data to Deg F")
          Case "TMIN"
            Logger.Dbg("CliGenUpdate: Found CliGen TMin data")
            lArgsMet.Clear()
            lArgsMet.SetValue("Timeseries", lDS)
            lTsMath.DataSets.Clear()
            lDataManager.Clear()
            lDataManager.OpenDataSource(lTsMath, "Celsius to F", lArgsMet)
            lTMin = lDataManager.DataSets(0)
            Logger.Dbg("CliGenUpdate: Converted CliGen TMax data to Deg F")
          Case "RAD"
            Logger.Dbg("CliGenUpdate: Disaggregating Daily Solar Radiation to Hourly")
            lArgsMet.Clear()
            lArgsMet.SetValue("SRAD", lDS)
            lArgsMet.SetValue("Latitude", "38.85") 'latitude of Washington National
            lDisTS.DataSets.Clear()
            lDisTS.Open("Solar Radiation (Disaggregate)", lArgsMet)
            lts = lDisTS.DataSets(0)
            Logger.Dbg("CliGenUpdate: Solar Radiation Disaggregation complete!")
            'SaveOnWDM(lWDMfile, lts, "SOLR", 9161)
          Case "W-VL"
            Logger.Dbg("CliGenUpdate: Found Wlind Velocity (m/s) - convert to mi/day")
            lArgsMet.Clear()
            lArgsMet.SetValue("Timeseries", lDS)
            lArgsMet.SetValue("Number", 53.7) 'multiplier converts (m/s) to (mi/day)
            lTsMath.DataSets.Clear()
            lDataManager.Clear()
            lDataManager.OpenDataSource(lTsMath, "Multiply", lArgsMet)
            Dim lWind As atcTimeseries = lDataManager.DataSets(0)
            Logger.Dbg("CliGenUpdate: Disaggregating Daily Wind to Hourly")
            Dim lHrDist() As Double = {0, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.035, 0.037, 0.041, 0.046, 0.05, 0.053, 0.054, 0.058, 0.057, 0.056, 0.05, 0.043, 0.04, 0.038, 0.035, 0.035, 0.034}
            lArgsMet.Clear()
            lArgsMet.SetValue("TWND", lWind)
            lArgsMet.SetValue("Hourly Distribution", lHrDist)
            lDisTS.DataSets.Clear()
            lDisTS.Open("Wind (Disaggregate)", lArgsMet)
            lts = lDisTS.DataSets(0)
            Logger.Dbg("CliGenUpdate: Wind Disaggregation complete!")
            'SaveOnWDM(lWDMfile, lts, "WIND", 9141)
          Case "TDEW"
            Logger.Dbg("CliGenUpdate: Found Dewpoint Temp - Convert to Deg F")
            lArgsMet.Clear()
            lArgsMet.SetValue("Timeseries", lDS)
            lTsMath.DataSets.Clear()
            lDataManager.Clear()
            lDataManager.OpenDataSource(lTsMath, "Celsius to F", lArgsMet)
            lDewPt = lDataManager.DataSets(0)
            Logger.Dbg("CliGenUpdate: Converted Dewpoint Temp to Deg F")
          Case "PRCP"
            Logger.Dbg("CliGenUpdate: Found Precip")
            lPrecip = lDS
            Logger.Dbg("CliGenUpdate: Precip contains " & lPrecip.numValues & " values")
          Case "DUR"
            Logger.Dbg("CliGenUpdate: Found Precip Duration")
            lPrecDur = lDS
            Logger.Dbg("CliGenUpdate: Precip Duration contains " & lPrecDur.numValues & " values")
          Case "TP"
            Logger.Dbg("CliGenUpdate: Found Precip Time to Peak")
            lPrecPkTime = lDS
            Logger.Dbg("CliGenUpdate: Precip Time to Peak contains " & lPrecPkTime.numValues & " values")
          Case "IP"
            Logger.Dbg("CliGenUpdate: Found Precip Peak Intensity")
            lPrecPkInten = lDS
            Logger.Dbg("CliGenUpdate: Precip Peak Intensity contains " & lPrecPkInten.numValues & " values")
        End Select
      Next

      Dim lDataGroup As New atcDataGroup
      If Not lTMin Is Nothing AndAlso Not lTMax Is Nothing Then 'disaggregate tmin/tmax to hrly temp
        Logger.Dbg("CliGenUpdate: Disaggregating TMin/TMax to Hourly Temp")
        lArgsMet.Clear()
        lArgsMet.SetValue("TMIN", lTMin)
        lArgsMet.SetValue("TMAX", lTMax)
        lArgsMet.SetValue("Observation Time", "24")
        lDisTS.DataSets.Clear()
        lDisTS.Open("Temperature", lArgsMet)
        lATmp = lDisTS.DataSets(0)
        Logger.Dbg("CliGenUpdate: Temperature Disaggregation complete!")
        'SaveOnWDM(lWDMfile, lATmp, "ATMP", 122)
        'now disaggregate Dewpoint using hourly temp
        lArgsMet.Clear()
        lArgsMet.SetValue("Dewpoint", lDewPt)
        lArgsMet.SetValue("ATMP", lATmp)
        lDisTS.DataSets.Clear()
        lDisTS.Open("Dewpoint", lArgsMet)
        lts = lDisTS.DataSets(0)
        Logger.Dbg("CliGenUpdate: Dewpoint Temp Disaggregation complete!")
        'SaveOnWDM(lWDMfile, lts, "DEWP", 151)
        'now generate Hamon PET
        Dim lCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
        Logger.Dbg("CliGenUpdate: Computing Hamon PET from Hourly temperature")
        lts = atcMetCmp.CmpHamX(lATmp, Nothing, True, 39, lCTS)
        Logger.Dbg("CliGenUpdate: Hamon PET generation complete!")
        lts.Attributes.SetValue("Location", lTMin.Attributes.GetValue("Location"))
        'SaveOnWDM(lWDMfile, lts, "EVAP", 111)
        If aParmToVary = "ATMP" Then
          lDataGroup.Add(lATmp)
        End If
      End If
      If Not lPrecip Is Nothing AndAlso Not lPrecDur Is Nothing AndAlso _
         Not lPrecPkTime Is Nothing AndAlso Not lPrecPkInten Is Nothing Then 'disagg daily precip
        Logger.Dbg("CliGenUpdate: Disaggregating Precip")
        lts = atcMetCmp.DisCliGenPrecip(lPrecip, lPrecDur, lPrecPkTime, lPrecPkInten)
        Logger.Dbg("CliGenUpdate: Precip Disaggregation complete!")
        lArgsMet.Clear()
        lArgsMet.SetValue("Timeseries", lts)
        lArgsMet.SetValue("Number", 25.4) 'multiplier converts mm to inches
        lTsMath.DataSets.Clear()
        lDataManager.Clear()
        lDataManager.OpenDataSource(lTsMath, "Divide", lArgsMet)
        'SaveOnWDM(lWDMfile, lDataManager.DataSets(0), "HPCP", 105)
        If aParmToVary = "HPCP" Then
          lDataGroup.Add(lDataManager.DataSets(0))
        End If
      End If
      Return lDataGroup
    End Function

    'Private Sub SaveOnWDM(ByVal aWDMFile As atcDataSource, ByVal aTs As atcTimeseries, ByVal aCons As String, ByVal aID As Integer)
    '  aTs.Attributes.SetValue("Scenario", "CLIGEN")
    '  aTs.Attributes.SetValue("Constituent", aCons)
    '  aTs.Attributes.SetValue("ID", aID)
    '  If aWDMFile.AddDataSet(aTs, atcDataSource.EnumExistAction.ExistReplace) Then
    '    Logger.Dbg("CliGenUpdate: Saved " & aCons & " on WDM file '" & aWDMFile.Name & "'")
    '  Else
    '    Logger.Dbg("CliGenUpdate: PROBLEM Saving " & aCons & " on WDM file '" & aWDMFile.Name & "'")
    '  End If
    'End Sub
  End Class
End Module
