Imports atcdata
Imports atcUtility

Imports System.Reflection

Public Class atcMetCmpPlugin
  Inherits atcDataSource

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
    Dim lTMinTSer As atcTimeseries
    Dim lTMaxTSer As atcTimeseries
    Dim lLatitude As Double
    Dim lCTS(12) As Double
    Dim lDegF As Boolean
    Dim lAttDef As atcAttributeDefinition
    Dim lOk As Boolean
    Dim lExtreme As Double

    Select Case aOperationName
      Case "Solar Radiation"
        Dim lCldTSer As atcTimeseries
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
        Dim lSRadTSer As atcTimeseries
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
          lCTS = aArgs.GetValue("Monthly Coefficients")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Monthly Coefficients")
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
          lCTS = aArgs.GetValue("Monthly Coefficients")
          lOk = True
        End If
        lAttDef = atcDataAttributes.GetDefinition("Monthly Coefficients")
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
    End Select
    '    Next
    If MyBase.DataSets.Count > 0 Then Return True
    '   End If

  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal ParentHandle As Integer)
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
          .Name = "Monthly Coefficients"
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
          .Name = "Monthly Coefficients"
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
        lArguments.SetValue(defTimeSeriesOne.Clone("SRAD", "Daily Solar Radiation (Langleys)"), Nothing)
        lArguments.SetValue(defDegF, Nothing)
        lArguments.SetValue(defJMonCoeff, Nothing)
        lArguments.SetValue(defConstCoeff, Nothing)

        lOperations.SetValue(lJensen, Nothing, lArguments)
      End If

      Return lOperations
    End Get
  End Property

End Class