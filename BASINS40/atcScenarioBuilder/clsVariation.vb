Imports atcData
Imports MapWinUtility

Public Class Variation

  'Parameters for Hammond - TODO: don't hard code these
  Private pDegF As Boolean = True
  Private pLatDeg As Double = 39
  Private pCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}

  Private pName As String = "<untitled>"
  Private pDataSets As atcDataGroup
  Private pComputationSource As atcDataSource
  Private pOperation As String = ""
  Private pSelected As Boolean = False
  'TODO: make rest of public variables into peoperties
  Public Seasons As atcSeasons.atcSeasonBase
  Public Min As Double = Double.NaN
  Public Max As Double = Double.NaN
  Public Increment As Double = Double.NaN
  Private pIncrementsSinceStart As Integer = 0
  Public CurrentValue As Double = Double.NaN

  Public ColorAboveMax As System.Drawing.Color = System.Drawing.Color.OrangeRed
  Public ColorBelowMin As System.Drawing.Color = System.Drawing.Color.DeepSkyBlue
  Public ColorDefault As System.Drawing.Color = System.Drawing.Color.White

  Public Property Name() As String
    Get
      Return pName
    End Get
    Set(ByVal newValue As String)
      pName = newValue
    End Set
  End Property

  Public Property DataSets() As atcDataGroup
    Get
      Return pDataSets
    End Get
    Set(ByVal newValue As atcDataGroup)
      pDataSets = newValue
    End Set
  End Property

  Public Property ComputationSource() As atcDataSource
    Get
      Return pComputationSource
    End Get
    Set(ByVal newValue As atcDataSource)
      pComputationSource = newValue
    End Set
  End Property

  Public Property Operation() As String
    Get
      Return pOperation
    End Get
    Set(ByVal newValue As String)
      pOperation = newValue
    End Set
  End Property

  Public Property Selected() As Boolean
    Get
      Return pSelected
    End Get
    Set(ByVal newValue As Boolean)
      pSelected = newValue
    End Set
  End Property
  Public ReadOnly Property Iterations() As Integer
    Get
      Try
        Return (Max - Min) / Increment + 1
      Catch ex As Exception
        Return 1
      End Try
    End Get
  End Property

  Public Function StartIteration() As atcDataGroup
    Me.CurrentValue = Me.Min
    pIncrementsSinceStart = 0
    Return VaryData()
  End Function

  Public Function NextIteration() As atcDataGroup
    pIncrementsSinceStart += 1
    If pIncrementsSinceStart < Iterations Then
      Me.CurrentValue = Me.Min + Me.Increment * pIncrementsSinceStart
      Return VaryData()
    Else
      Return Nothing
    End If
  End Function

  Private Function VaryData() As atcDataGroup
    Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
    Dim lMetCmp As New atcMetCmp.atcMetCmpPlugin
    Dim lArgsMath As New atcDataAttributes
    Dim lModifiedTS As atcTimeseries
    Dim lModifiedGroup As New atcDataGroup
    For Each lOriginalData As atcDataSet In DataSets
      If Seasons Is Nothing Then
        lTsMath.DataSets.Clear()
        lArgsMath.Clear()
        lArgsMath.SetValue("timeseries", lOriginalData)
        lArgsMath.SetValue("Number", CurrentValue)
        lTsMath.Open(Operation, lArgsMath)
        lModifiedTS = lTsMath.DataSets(0)
      Else
        Dim lSplitData As atcDataGroup = Seasons.Split(lOriginalData, Nothing)
        Dim lModifiedSplit As New atcDataGroup
        For Each lSplitTS As atcTimeseries In lSplitData
          If Seasons.SeasonSelected(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
            'modify data from this season
            lTsMath.DataSets.Clear()
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lSplitTS)
            lArgsMath.SetValue("Number", CurrentValue)
            lTsMath.Open(Operation, lArgsMath)
            lModifiedSplit.Add(lTsMath.DataSets(0))
          Else 'Add unmodified data from this season that was not selected
            lModifiedSplit.Add(lSplitTS)
          End If
        Next
        lModifiedTS = MergeTimeseries(lModifiedSplit)
      End If

      lModifiedGroup.Add(lModifiedTS)

      Select Case DataSets.ItemByIndex(0).Attributes.GetValue("Constituent").ToString.ToUpper
        Case "ATMP", "AIRTMP", "AIRTEMP" 'recompute PET when ATMP is changed - TODO: don't hard code ATMP
          'Dim lAirTmpMean As String = Format(lModifiedTS.Attributes.GetValue("Mean"), "#.00")
          lModifiedTS = atcMetCmp.CmpHamX(lModifiedTS, Nothing, pDegF, pLatDeg, pCTS)
          lModifiedGroup.Add(lModifiedTS)
          'Dim lEvapMean As String = Format(lModifiedTS.Attributes.GetValue("Mean") * 365.25, "#.00")
          With lModifiedTS.Attributes
            .SetValue("Constituent", "PET")
            .SetValue("Id", 111)
          End With
      End Select
    Next
    Return lModifiedGroup
  End Function

  Public Function Clone() As Variation
    Dim newVariation As New Variation
    With newVariation
      .Name = Name
      If Not DataSets Is Nothing Then .DataSets = DataSets.Clone()
      .ComputationSource = ComputationSource
      .Operation = Operation.Clone()
      .Seasons = Seasons 'TODO: clone Seasons of not Nothing
      .Selected = Selected
      .Min = Min
      .Max = Max
      .Increment = Increment
      .CurrentValue = CurrentValue
      .ColorAboveMax = ColorAboveMax
      .ColorBelowMin = ColorBelowMin
      .ColorDefault = ColorDefault
    End With
    Return newVariation
  End Function

  Private Property SeasonsXML() As String
    Get
      If Seasons Is Nothing Then
        Return ""
      Else
        Return "  <Seasons Type='" & Seasons.GetType.Name & "'>" & vbCrLf _
             & "  " & Seasons.SeasonsSelectedXML & "  </Seasons>" & vbCrLf
      End If
    End Get
    Set(ByVal newValue As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(newValue) Then
        If lXML.Tag.ToLower.Equals("seasons") Then
          Dim lSeasonTypeName As String = lXML.GetAttrValue("Type")
          For Each lSeasonType As Type In atcSeasons.atcSeasonPlugin.AllSeasonTypes
            If lSeasonType.Name.Equals(lSeasonTypeName) Then
              Seasons = lSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
              If lXML.FirstChild2 Then
                Seasons.SeasonsSelectedXML = lXML.GetXml
              End If
            End If
          Next
        End If
      End If
    End Set
  End Property

  Private Property DataSetsXML() As String
    Get
      If DataSets Is Nothing Then
        Return ""
      Else
        Dim lXML As String = "  <DataSets count='" & DataSets.Count & "'>" & vbCrLf
        For Each lDataSet As atcDataSet In DataSets
          lXML &= "    <DataSet"
          lXML &= " ID='" & lDataSet.Attributes.GetValue("ID") & "'"
          lXML &= " Location='" & lDataSet.Attributes.GetValue("Location") & "'"
          lXML &= " Constituent='" & lDataSet.Attributes.GetValue("Constituent") & "'"
          lXML &= " />" & vbCrLf
        Next
        Return lXML & "  </DataSets>" & vbCrLf
      End If

    End Get
    Set(ByVal newValue As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(newValue) Then
        DataSets = New atcDataGroup
        If lXML.FirstChild2() Then
          Do
            Dim lID As String = lXML.GetAttrValue("ID")
            If lID.Length > 0 Then
              Dim lDataGroup As atcDataGroup = g_DataManager.DataSets.FindData("ID", lID, 1)
              If lDataGroup.Count > 0 Then
                DataSets.Add(lDataGroup.ItemByIndex(0))
              Else
                Logger.Msg("No data loaded with ID " & lID, "Variation from XML")
              End If
            Else
              Logger.Msg("No data set ID found in XML", "Variation from XML")
            End If
          Loop While lXML.NextSibling2
        End If
      End If
    End Set
  End Property

  Public Property XML() As String
    Get
      Dim lXML As String = "<Variation>" & vbCrLf _
           & "  <Name>" & Name & "</Name>" & vbCrLf
      If Not Double.IsNaN(Min) Then
        lXML &= "  <Min>" & Min & "</Min>" & vbCrLf
      End If
      If Not Double.IsNaN(Max) Then
        lXML &= "  <Max>" & Max & "</Max>" & vbCrLf
      End If
      If Not Double.IsNaN(Increment) Then
        lXML &= "  <Increment>" & Increment & "</Increment>" & vbCrLf
      End If
      lXML &= "  <Operation>" & Operation & "</Operation>" & vbCrLf
      If Not ComputationSource Is Nothing Then
        lXML &= "  <ComputationSource>" & ComputationSource.Name & "</ComputationSource>" & vbCrLf
      End If
      lXML &= "  <Selected>" & Selected & "</Selected>" & vbCrLf _
           & DataSetsXML _
           & SeasonsXML _
           & "</Variation>" & vbCrLf
      Return lXML
    End Get
    Set(ByVal Value As String)
      Dim lXML As New Chilkat.Xml
      If lXML.LoadXml(Value) Then
        If lXML.FirstChild2() Then
          Do
            With lXML
              Select Case .Tag.ToLower
                Case "name" : Name = .Content
                Case "min" : Min = CDbl(.Content)
                Case "max" : Max = CDbl(.Content)
                Case "increment" : Increment = CDbl(.Content)
                Case "operation" : Operation = .Content
                Case "computationsource"
                  ComputationSource = g_DataManager.DataSourceByName(.Content)
                Case "datasets" : DataSetsXML = .GetXml
                Case "selected" : Selected = .Content.ToLower.Equals("true")
                Case "seasons" : SeasonsXML = .GetXml
              End Select
            End With
          Loop While lXML.NextSibling2
        End If
      End If
    End Set
  End Property

  Public Overrides Function ToString() As String
    Dim retStr As String = Name & " " & Operation
    If Not Double.IsNaN(Min) Then retStr &= " from " & Format(Min, "0.0")
    If Not Double.IsNaN(Max) Then retStr &= " to " & Format(Max, "0.0")
    If Not Double.IsNaN(Increment) Then retStr &= " step " & Format(Increment, "0.0")
    If Not Seasons Is Nothing Then
      retStr &= " " & atcSeasons.atcSeasonPlugin.SeasonClassNameToLabel(Seasons.GetType.Name) _
             & ": " & Seasons.SeasonsSelectedString
    End If
    Return retStr
  End Function
End Class
