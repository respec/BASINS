Imports atcdata
Imports atcUtility

Imports System.Reflection

Public Class atcMetCmpPlugin
  Inherits atcData.atcDataSource

  Private pAvailableOperations As atcDataAttributes ' atcDataGroup
  Private pName As String = "Timeseries::Meteorologic Generation"

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Timeseries::Meteorologic Generation"
    End Get
  End Property

  Public Overrides ReadOnly Property Category() As String
    Get
      Return "Meteorologic Generation"
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get
      Return Name
    End Get
  End Property

  Public Overrides ReadOnly Property CanOpen() As Boolean
    Get
      Return True
    End Get
  End Property

  'The only element of aArgs is an atcDataGroup or atcTimeseries
  'The attribute(s) will be set to the result(s) of calculation(s)
  Public Overrides Function Open(ByVal aOperationName As String, _
                        Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim lTMinTSer As atcTimeseries = Nothing
    Dim lTMaxTSer As atcTimeseries = Nothing
    Dim lSRadTSer As atcTimeseries = Nothing
    Dim lWindTSer As atcTimeseries = Nothing
    Dim lDlyTSer As atcTimeseries = Nothing
    Dim lLatitude As Double
    Dim lCTS(12) As Double
    Dim lDegF As Boolean
    Dim lObsTime As Integer
    Dim lAttDef As atcAttributeDefinition
    Dim lOk As Boolean

    Select Case aOperationName
      Case "Solar Radiation"
        Dim lCldTSer As atcTimeseries = Nothing
        If aArgs Is Nothing Then
          Dim lForm As New frmCmpSol
          lOk = lForm.AskUser(DataManager, lCldTSer, lLatitude)
        Else
          lCldTSer = aArgs.GetValue("DCLD")
          lLatitude = aArgs.GetValue("Latitude")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Latitude")
        If lOk And Not lCldTSer Is Nothing And _
          lLatitude >= lAttDef.Min And lLatitude <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = CmpSol(lCldTSer, Me, lLatitude)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Jensen PET"
        Dim lCTX As Double
        If aArgs Is Nothing Then
          Dim lForm As New frmCmpJPET
          lOk = lForm.AskUser(DataManager, lTMinTSer, lTMaxTSer, lSRadTSer, lDegF, lCTX, lCTS)
        Else
          lTMinTSer = aArgs.GetValue("TMIN")
          lTMaxTSer = aArgs.GetValue("TMAX")
          lSRadTSer = aArgs.GetValue("SRAD")
          lDegF = aArgs.GetValue("Degrees F")
          lCTX = aArgs.GetValue("Constant Coefficient")
          lCTS = aArgs.GetValue("Jensen Monthly Coefficients")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Jensen Monthly Coefficients")
        For i As Integer = 1 To 12
          If lCTS(i) > lAttDef.Max Or lCTS(i) < lAttDef.Min Then
            lOk = False
            Exit For
          End If
        Next
        lAttDef = atcDataAttributes.GetDefinition("Constant Coefficient")
        If lOk And Not lTMinTSer Is Nothing And Not lTMaxTSer Is Nothing And _
          Not lSRadTSer Is Nothing And lCTX >= lAttDef.Min And lCTX <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = CmpJen(lTMinTSer, lTMaxTSer, lSRadTSer, Me, lDegF, lCTX, lCTS)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Hamon PET"
        If aArgs Is Nothing Then
          Dim lForm As New frmCmpHPET
          lOk = lForm.AskUser(DataManager, lTMinTSer, lTMaxTSer, lDegF, lLatitude, lCTS)
        Else
          lTMinTSer = aArgs.GetValue("TMIN")
          lTMaxTSer = aArgs.GetValue("TMAX")
          lDegF = aArgs.GetValue("Degrees F")
          lLatitude = aArgs.GetValue("Latitude")
          lCTS = aArgs.GetValue("Hamon Monthly Coefficients")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Hamon Monthly Coefficients")
        For i As Integer = 1 To 12
          If lCTS(i) > lAttDef.Max Or lCTS(i) < lAttDef.Min Then
            lOk = False
            Exit For
          End If
        Next
        lAttDef = atcDataAttributes.GetDefinition("Latitude")
        If lOk And Not lTMinTSer Is Nothing And Not lTMinTSer Is Nothing And _
          lLatitude >= lAttDef.Min And lLatitude <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = CmpHam(lTMinTSer, lTMaxTSer, Me, lDegF, lLatitude, lCTS)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Penman Pan Evaporation"
        Dim lDewPTSer As atcTimeseries
        If aArgs Is Nothing Then
          Dim lForm As New frmCmpPenman
          lOk = lForm.AskUser(DataManager, lTMinTSer, lTMaxTSer, lSRadTSer, lDewPTSer, lWindTSer)
        Else
          lTMinTSer = aArgs.GetValue("TMIN")
          lTMaxTSer = aArgs.GetValue("TMAX")
          lSRadTSer = aArgs.GetValue("SRAD")
          lDewPTSer = aArgs.GetValue("DEWP")
          lWindTSer = aArgs.GetValue("TWND")
          lOk = True
        End If
        If lOk And Not lTMinTSer Is Nothing And Not lTMaxTSer Is Nothing And _
          Not lSRadTSer Is Nothing And Not lDewPTSer Is Nothing And Not lWindTSer Is Nothing Then
          Dim lMetCmpTS As atcTimeseries = CmpPen(lTMinTSer, lTMaxTSer, lSRadTSer, lDewPTSer, lWindTSer, Me)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Wind Travel"
        If aArgs Is Nothing Then
          Dim ltsgroup As atcDataGroup = DataManager.UserSelectData("Select Wind Speed data for computing " & aOperationName)
          If Not ltsgroup Is Nothing Then lWindTSer = ltsgroup(0)
        Else
          lWindTSer = aArgs.GetValue("WIND")
        End If
        If Not lWindTSer Is Nothing Then
          Dim lMetCmpTS As atcTimeseries = CmpWnd(lWindTSer, Me)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Cloud Cover"
        Dim lPctSunTSer As atcTimeseries
        If aArgs Is Nothing Then
          Dim ltsgroup As atcDataGroup = DataManager.UserSelectData("Select Percent Sun data for computing " & aOperationName)
          If Not ltsgroup Is Nothing Then lPctSunTSer = ltsgroup(0)
        Else
          lPctSunTSer = aArgs.GetValue("PSUN")
        End If
        If Not lPctSunTSer Is Nothing Then
          Dim lMetCmpTS As atcTimeseries = CmpCld(lPctSunTSer, Me)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Solar Radiation (Disaggregate)"
        If aArgs Is Nothing Then
          Dim lForm As New frmDisSol
          lOk = lForm.AskUser(DataManager, lDlyTSer, lLatitude)
        Else
          lDlyTSer = aArgs.GetValue("SRAD")
          lLatitude = aArgs.GetValue("Latitude")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Latitude")
        If lOk And Not lDlyTSer Is Nothing And _
          lLatitude >= lAttDef.Min And lLatitude <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = DisSolPet(lDlyTSer, Me, 1, lLatitude)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Evapotranspiration"
        If aArgs Is Nothing Then
          Dim lForm As New frmDisSol
          lform.text = "Disaggregate Evapotranspiration"
          lform.lblTSer.Text = "Specify Daily Evapotranspiration Timeseries"
          lOk = lForm.AskUser(DataManager, lDlyTSer, lLatitude)
        Else
          lDlyTSer = aArgs.GetValue("DEVT")
          lLatitude = aArgs.GetValue("Latitude")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Latitude")
        If lOk And Not lDlyTSer Is Nothing And _
          lLatitude >= lAttDef.Min And lLatitude <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = DisSolPet(lDlyTSer, Me, 2, lLatitude)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Temperature"
        If aArgs Is Nothing Then
          Dim lForm As New frmDisTemp
          lOk = lForm.AskUser(DataManager, lTMinTSer, lTMaxTSer, lObsTime)
        Else
          lTMinTSer = aArgs.GetValue("TMIN")
          lTMaxTSer = aArgs.GetValue("TMAX")
          lObsTime = aArgs.GetValue("Observation Time")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Observation Time")
        If lOk And Not lTMinTSer Is Nothing And Not lTMinTSer Is Nothing And _
          lObsTime >= lAttDef.Min And lObsTime <= lAttDef.Max Then
          Dim lMetCmpTS As atcTimeseries = DisTemp(lTMinTSer, lTMaxTSer, Me, lObsTime)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Wind (Disaggregate)"
        Dim lHrDist(24) As Double
        Dim lHrSum As Double = 0
        If aArgs Is Nothing Then
          Dim lForm As New frmDisWind
          lOk = lForm.AskUser(DataManager, lWindTSer, lHrDist)
        Else
          lWindTSer = aArgs.GetValue("TWND")
          lHrDist = aArgs.GetValue("Hourly Distribution")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Hourly Distribution")
        For i As Integer = 1 To 24
          If lHrDist(i) > lAttDef.Max Or lHrDist(i) < lAttDef.Min Then
            lOk = False
            Exit For
          Else
            lHrSum += lHrDist(i)
          End If
        Next
        If lOk And Not lWindTSer Is Nothing And Math.Abs(lHrSum - 1) < 0.001 Then
          Dim lMetCmpTS As atcTimeseries = DisWnd(lWindTSer, Me, lHrDist)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
      Case "Precipitation"
        Dim lHrTSers As atcDataGroup
        Dim lTol As Double
        Dim lAttDef2 As atcAttributeDefinition
        If aArgs Is Nothing Then
          Dim lForm As New frmDisPrec
          lOk = lForm.AskUser(DataManager, lDlyTSer, lHrTSers, lObsTime, lTol)
        Else
          lDlyTSer = aArgs.GetValue("DPRC")
          lHrTSers = aArgs.GetValue("HPCP")
          lObsTime = aArgs.GetValue("Observation Hour")
          lTol = aArgs.GetValue("Data Tolerance")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Observation Hour")
        lAttDef2 = atcDataAttributes.GetDefinition("Data Tolerance")
        If lOk And Not lHrTSers Is Nothing And _
          lObsTime >= lAttDef.Min And lObsTime <= lAttDef.Max And _
          lTol >= lAttDef2.Min And lTol <= lAttDef2.Max Then
          Dim lMetCmpTS As atcTimeseries = DisPrecip(lDlyTSer, Me, lHrTSers, lObsTime, lTol)
          MyBase.DataSets.Add(lMetCmpTS)
        End If
    End Select

    If MyBase.DataSets.Count > 0 Then Return True

  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal ParentHandle As Integer)
    Dim lAvlOps As atcDataAttributes = AvailableOperations()
  End Sub

  Public Overrides Function ToString() As String
    Return Name.Substring(23) 'Skip first part of Name which is "Timeseries::Seasonal - "
  End Function

  Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
    Get
      Dim lOperations As atcDataAttributes
      If Not pAvailableOperations Is Nothing Then
        lOperations = pAvailableOperations
      Else
        lOperations = New atcDataAttributes
        Dim lArguments As atcDataAttributes
        Dim defTimeSeriesOne As New atcAttributeDefinition
        With defTimeSeriesOne
          .Name = "Timeseries"
          .Description = "One time series"
          .Editable = True
          .TypeString = "atcTimeseries"
        End With

        Dim defTMinTS As New atcAttributeDefinition
        defTMinTS = defTimeSeriesOne.Clone("TMIN", "Daily Minimum Temperature")
        Dim defTMaxTS As New atcAttributeDefinition
        defTMaxTS = defTimeSeriesOne.Clone("TMAX", "Daily Maximum Temperature")

        Dim defLat As New atcAttributeDefinition
        With defLat
          .Name = "Latitude"
          .Description = "Latitude in decimal degrees"
          .DefaultValue = 0
          .Max = 51
          .Min = 25
          .Editable = True
          .TypeString = "Double"
        End With

        Dim lSolar As New atcAttributeDefinition
        With lSolar
          .Name = "Solar Radiation"
          .Category = "Meteorologic Computations"
          .Description = "Generate Solar Radiation from Cloud Cover"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTimeSeriesOne.Clone("DCLD", "Daily Cloud Cover (values = 0 - 10)"), Nothing)
        lArguments.SetValue(defLat, Nothing)

        lOperations.SetValue(lSolar, Nothing, lArguments)

        Dim defDegF As New atcAttributeDefinition
        With defDegF
          .Name = "Degrees F"
          .Description = "Temperature in Degrees F, True or False"
          .DefaultValue = True
          .Editable = True
          .TypeString = "Boolean"
        End With

        Dim defHMonCoeff As New atcAttributeDefinition
        With defHMonCoeff
          .Name = "Hamon Monthly Coefficients"
          .Description = "Coefficients for Hamon PET computation"
          .DefaultValue = New Double() {0, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055}
          .Max = 1
          .Min = 0
          .Editable = True
          .TypeString = "Double(12)"
        End With

        Dim lHamon As New atcAttributeDefinition
        With lHamon
          .Name = "Hamon PET"
          .Category = "Meteorologic Computations"
          .Description = "Generate PET using Hamon's algorithm"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTMinTS, Nothing)
        lArguments.SetValue(defTMaxTS, Nothing)
        lArguments.SetValue(defDegF, Nothing)
        lArguments.SetValue(defLat, Nothing)
        lArguments.SetValue(defHMonCoeff, Nothing)

        lOperations.SetValue(lHamon, Nothing, lArguments)

        Dim defJMonCoeff As New atcAttributeDefinition
        With defJMonCoeff
          .Name = "Jensen Monthly Coefficients"
          .Description = "Coefficients for Jensen PET computation"
          .DefaultValue = New Double() {0, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012, 0.012}
          .Max = 1
          .Min = 0
          .Editable = True
          .TypeString = "Double(12)"
        End With

        Dim lJensen As New atcAttributeDefinition
        With lJensen
          .Name = "Jensen PET"
          .Category = "Meteorologic Computations"
          .Description = "Generate PET using Jensen's algorithm"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defSRadTS As New atcAttributeDefinition
        defSRadTS = defTimeSeriesOne.Clone("SRAD", "Daily Solar Radiation (Langleys)")

        Dim defConstCoeff As New atcAttributeDefinition
        With defConstCoeff
          .Name = "Constant Coefficient"
          .Description = "Constant Coefficient CTX used in Jensen PET computation"
          .DefaultValue = 23
          .Max = 27
          .Min = 10
          .Editable = True
          .TypeString = "Double"
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTMinTS, Nothing)
        lArguments.SetValue(defTMaxTS, Nothing)
        lArguments.SetValue(defSRadTS, Nothing)
        lArguments.SetValue(defDegF, Nothing)
        lArguments.SetValue(defJMonCoeff, Nothing)
        lArguments.SetValue(defConstCoeff, Nothing)

        lOperations.SetValue(lJensen, Nothing, lArguments)

        Dim lPenman As New atcAttributeDefinition
        With lPenman
          .Name = "Penman Pan Evaporation"
          .Category = "Meteorologic Computations"
          .Description = "Generate Pan Evaporation using Penman's algorithm"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defDewPTS As New atcAttributeDefinition
        defDewPTS = defTimeSeriesOne.Clone("DEWP", "Daily Dewpoint Temperature")
        Dim defTWindTS As New atcAttributeDefinition
        defTWindTS = defTimeSeriesOne.Clone("TWND", "Daily Wind Movement (miles)")

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTMinTS, Nothing)
        lArguments.SetValue(defTMaxTS, Nothing)
        lArguments.SetValue(defSRadTS, Nothing)
        lArguments.SetValue(defDewPTS, Nothing)
        lArguments.SetValue(defTWindTS, Nothing)

        lOperations.SetValue(lPenman, Nothing, lArguments)

        Dim lWind As New atcAttributeDefinition
        With lWind
          .Name = "Wind Travel"
          .Category = "Meteorologic Computations"
          .Description = "Generate Daily Wind Travel using Average Daily Wind Speed"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defWindTS As New atcAttributeDefinition
        defWindTS = defTimeSeriesOne.Clone("WIND", "Daily Average Wind Speed (mph)")

        lArguments = New atcDataAttributes
        lArguments.SetValue(defWindTS, Nothing)
        lOperations.SetValue(lWind, Nothing, lArguments)

        Dim lCldCov As New atcAttributeDefinition
        With lCldCov
          .Name = "Cloud Cover"
          .Category = "Meteorologic Computations"
          .Description = "Generate Daily Cloud Cover using Daily Percent Sunshine"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defPSunTS As New atcAttributeDefinition
        defPSunTS = defTimeSeriesOne.Clone("PSUN", "Daily Percent Sunshine")

        lArguments = New atcDataAttributes
        lArguments.SetValue(defPSunTS, Nothing)
        lOperations.SetValue(lCldCov, Nothing, lArguments)

        Dim lDisSolar As New atcAttributeDefinition
        With lDisSolar
          .Name = "Solar Radiation (Disaggregate)"
          .Category = "Meteorologic Disaggregations"
          .Description = "Disaggregate Daily Solar Radiation to Hourly"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defSRadTS, Nothing)
        lArguments.SetValue(defLat, Nothing)
        lOperations.SetValue(lDisSolar, Nothing, lArguments)

        Dim lDisEvap As New atcAttributeDefinition
        With lDisEvap
          .Name = "Evapotranspiration"
          .Category = "Meteorologic Disaggregations"
          .Description = "Disaggregate Daily Evapotranspiration to Hourly"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defEvapTS As New atcAttributeDefinition
        defEvapTS = defTimeSeriesOne.Clone("DEVT", "Daily Evapotranspiration (in)")

        lArguments = New atcDataAttributes
        lArguments.SetValue(defEvapTS, Nothing)
        lArguments.SetValue(defLat, Nothing)
        lOperations.SetValue(lDisEvap, Nothing, lArguments)

        Dim lDisTemp As New atcAttributeDefinition
        With lDisTemp
          .Name = "Temperature"
          .Category = "Meteorologic Disaggregations"
          .Description = "Disaggregate Daily TMIN/TMAX to Hourly Temperature"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defObsTime As New atcAttributeDefinition
        With defObsTime
          .Name = "Observation Time"
          .Description = "Hour (1 - 24) that Daily TMin/TMax observations were made"
          .DefaultValue = 24
          .Max = 24
          .Min = 1
          .Editable = True
          .TypeString = "Integer"
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTMinTS, Nothing)
        lArguments.SetValue(defTMaxTS, Nothing)
        lArguments.SetValue(defObsTime, Nothing)
        lOperations.SetValue(lDisTemp, Nothing, lArguments)

        Dim lDisWind As New atcAttributeDefinition
        With lDisWind
          .Name = "Wind (Disaggregate)"
          .Category = "Meteorologic Disaggregations"
          .Description = "Disaggregate Daily Wind to Hourly"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defHrDist As New atcAttributeDefinition
        With defHrDist
          .Name = "Hourly Distribution"
          .Description = "Hourly Distribution for Wind Travel Disaggregation"
          .DefaultValue = New Double() {0, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.035, 0.037, 0.041, 0.046, 0.05, 0.053, 0.054, 0.058, 0.057, 0.056, 0.05, 0.043, 0.04, 0.038, 0.035, 0.035, 0.034}
          .Max = 1
          .Min = 0
          .Editable = True
          .TypeString = "Double(24)"
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defTWindTS, Nothing)
        lArguments.SetValue(defHrDist, Nothing)
        lOperations.SetValue(lDisWind, Nothing, lArguments)

        Dim lDisPrec As New atcAttributeDefinition
        With lDisPrec
          .Name = "Precipitation"
          .Category = "Meteorologic Disaggregations"
          .Description = "Disaggregate Daily Precip to Hourly based on nearby Hourly data"
          .Editable = False
          .TypeString = "atcTimeseries"
          .Calculator = Me
        End With

        Dim defDPrecTS As New atcAttributeDefinition
        defDPrecTS = defTimeSeriesOne.Clone("DPRC", "Daily Precipitation")

        Dim defHrPrec As New atcAttributeDefinition
        With defHrPrec
          .Name = "HPCP"
          .Description = "Hourly Precipitation stations to use for Disaggregating Daily Precip"
          .Editable = True
          .TypeString = "atcDataGroup"
        End With

        Dim defTolerance As New atcAttributeDefinition
        With defTolerance
          .Name = "Data Tolerance"
          .Description = "Allowable ratio (as %) between Daily and Hourly values to use Hourly values for disaggregation"
          .DefaultValue = 90
          .Max = 100
          .Min = 0
          .Editable = True
          .TypeString = "Double"
        End With

        lArguments = New atcDataAttributes
        lArguments.SetValue(defDPrecTS, Nothing)
        lArguments.SetValue(defHrPrec, Nothing)
        lArguments.SetValue(defObsTime.Clone("Observation Hour", "Hour (1 - 24) that Daily Precipitation was recorded"), Nothing)
        lArguments.SetValue(defTolerance, Nothing)
        lOperations.SetValue(lDisPrec, Nothing, lArguments)
      End If

      Return lOperations
    End Get
  End Property

End Class