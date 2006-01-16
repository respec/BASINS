Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcSeasonBase

  Private pAvailableOperations As atcDataAttributes ' atcDataGroup

  Public ReadOnly Property Name() As String
    Get
      Dim lName As String = Me.GetType.Name
      If lName.Equals("atcSeasonsBase") Then
        Return ""
      Else
        Return "Timeseries::Seasonal - " & lName.Substring(10) 'Skip prefix "atcSeasons"
      End If
    End Get
  End Property

  Public ReadOnly Property Category() As String
    Get
      Return "Seasons"
    End Get
  End Property

  Public ReadOnly Property Description() As String
    Get
      Return Name
    End Get
  End Property

  'Divide the data in aTS into a group of TS, one per season
  Public Overridable Function Split(ByVal aTS As atcTimeseries, ByVal aSource As atcDataSource) As atcDataGroup
    Dim lNewGroup As New atcDataGroup
    Dim lSeasonIndex As Integer = -1
    Dim lPrevSeasonIndex As Integer
    Dim lNewTS As atcTimeseries
    Dim lNewTSvalueIndex As Integer
    Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)

    For iValue As Integer = 1 To aTS.numValues
      lPrevSeasonIndex = lSeasonIndex
      If lPoint Then
        lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
      Else '
        lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue - 1))
      End If
      lNewTS = lNewGroup.ItemByKey(lSeasonIndex)
      If lNewTS Is Nothing Then
        lNewTS = New atcTimeseries(aSource)
        CopyBaseAttributes(aTS, lNewTS)
        lNewTS.Dates = New atcTimeseries(aSource)
        lNewTS.numValues = aTS.numValues
        lNewTS.Dates.numValues = aTS.numValues
        lNewTS.Attributes.AddHistory("Split by " & ToString() & " " & SeasonName(lSeasonIndex))
        lNewTS.Attributes.Add("SeasonDefinition", Me)
        lNewTS.Attributes.Add("SeasonIndex", lSeasonIndex)
        lNewTS.Attributes.Add("SeasonName", SeasonName(lSeasonIndex))
        lNewGroup.Add(lSeasonIndex, lNewTS)
      End If

      If lPoint Then
        lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
      Else
        lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 0)
        If aTS.ValueAttributesExist(lNewTSvalueIndex) Then 'TODO:finish this!
          'lnewts.ValueAttributes(lnewtsvalueindex).SetRange(
        End If
        If lPrevSeasonIndex <> lSeasonIndex Then
          'Insert dummy value for start of interval after skipping dates outside season
          lNewTS.Values(lNewTSvalueIndex) = Double.NaN
          lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
          lNewTS.ValueAttributes(lNewTSvalueIndex).SetValue("Inserted", True)
          lNewTSvalueIndex += 1
        End If
      End If
      lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
      lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue)
      lNewTS.Attributes.SetValue("NextIndex", lNewTSvalueIndex + 1)
    Next

    For Each lNewTS In lNewGroup
      lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1) - 1
      lNewTS.numValues = lNewTSvalueIndex
      lNewTS.Dates.numValues = lNewTSvalueIndex
      lNewTS.Attributes.RemoveByKey("nextindex")
    Next
    Return lNewGroup
  End Function

  Public Overridable Sub SetSeasonalAttributes(ByVal aTS As atcTimeseries, _
                                   ByVal aAttributes As atcDataAttributes, _
                          Optional ByVal aCalculatedAttributes As atcDataAttributes = Nothing)

    Dim lSplit As atcDataGroup = Me.Split(aTS, Nothing)

    If aCalculatedAttributes Is Nothing Then
      aCalculatedAttributes = aTS.Attributes
    End If

    Dim lFormat As String = ""
    lFormat = lFormat.PadRight(Int(Log10(lSplit.Count)) + 1, "0")

    For Each lSeasonalTS As atcTimeseries In lSplit
      Dim lSeasonIndex As Integer = lSeasonalTS.Attributes.GetValue("SeasonIndex", 0)
      Dim lSeasonName As String = SeasonName(lSeasonIndex)
      For Each lAttribute As atcDefinedValue In aAttributes
        Dim lNewAttrDefinition As atcAttributeDefinition = lAttribute.Definition.Clone _
           (lAttribute.Definition.Name & " " & Me.ToString & " " & Format(lSeasonIndex, lFormat) & " " & lSeasonName, _
            Me.ToString & " " & lAttribute.Definition.Description)
        Dim lNewArguments As New atcDataAttributes
        lNewArguments.SetValue("SeasonDefinition", Me)
        lNewArguments.SetValue("SeasonIndex", lSeasonIndex)
        lNewArguments.SetValue("SeasonName", lSeasonName)
        lNewAttrDefinition.Calculator = lAttribute.Definition.Calculator
        aCalculatedAttributes.SetValue(lNewAttrDefinition, lSeasonalTS.Attributes.GetValue(lAttribute.Definition.Name), lNewArguments)
      Next
    Next
  End Sub

  Public Overridable Function SeasonIndex(ByVal aDate As Double) As Integer
    Return -1
  End Function

  Public Overridable Function SeasonName(ByVal aDate As Double) As String
    Return SeasonName(SeasonIndex(aDate))
  End Function

  Public Overridable Function SeasonName(ByVal aIndex As Integer) As String
    Return CStr(aIndex)
  End Function

  Public Overrides Function ToString() As String
    Return Name.Substring(23) 'Skip first part of Name which is "Timeseries::Seasonal - "
  End Function

End Class