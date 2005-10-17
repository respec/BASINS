Imports atcData
Imports atcUtility

Public Class atcEventBase

  Private pAvailableOperations As atcDataAttributes ' atcDataGroup

  Public ReadOnly Property Name() As String
    Get
      Return "Timeseries::Events"
    End Get
  End Property

  Public ReadOnly Property Category() As String
    Get
      Return "Events"
    End Get
  End Property

  Public ReadOnly Property Description() As String
    Get
      Return Name
    End Get
  End Property

  'Divide the data in aTS into a group of TS, one per season
  Public Function Split(ByVal aTS As atcTimeseries, ByVal aSource As atcDataSource, ByVal aThresh As Double, ByVal aHigh As Boolean) As atcDataGroup
    Dim lNewGroup As New atcDataGroup
    Dim lEventIndex As Integer = 0
    Dim lNewTS As atcTimeseries
    Dim lNewTSvalueIndex As Integer
    Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)
    Dim lInEvent As Boolean = False
    Dim lSPos As Integer
    Dim lEPos As Integer
    Dim iValue As Integer = 1

    While iValue <= aTS.numValues

      If lInEvent Then
        If (aHigh And aTS.Value(iValue) < aThresh) Or _
           (Not aHigh And aTS.Values(iValue) > aThresh) Then
          'clean up latest event tser
          lEPos = iValue - 1
          lNewTS = New atcTimeseries(aSource)
          CopyBaseAttributes(aTS, lNewTS)
          lNewTS.Dates = New atcTimeseries(aSource)
          lNewTS.numValues = lEPos - lSPos + 1
          lNewTS.Dates.numValues = lNewTS.numValues
          Array.Copy(aTS.Values, lSPos, lNewTS.Values, 1, lNewTS.numValues)
          If lPoint Then
            Array.Copy(aTS.Dates.Values, lSPos, lNewTS.Dates.Values, 1, lNewTS.numValues)
          Else
            Array.Copy(aTS.Dates.Values, lSPos - 1, lNewTS.Dates.Values, 0, lNewTS.numValues + 1)
          End If
          If aHigh Then
            lNewTS.Attributes.AddHistory("Event above " & aThresh)
          Else
            lNewTS.Attributes.AddHistory("Event below " & aThresh)
          End If
          lNewTS.Attributes.Add("EventIndex", lEventIndex)
          lNewTS.Attributes.Add("EventThreshold", aThresh)
          lNewGroup.Add(lEventIndex, lNewTS)
          lInEvent = False
        End If
        ElseIf (aHigh And aTS.Value(iValue) > aThresh) Or _
               (Not aHigh And aTS.Value(iValue) < aThresh) Then
        'new event
        lEventIndex += 1
        lSPos = iValue
        lInEvent = True
      End If

      iValue += 1
    End While

    Return lNewGroup
  End Function

  Public Overrides Function ToString() As String
    Return Name.Substring(12) 'Skip first part of Name which is "Timeseries::"
  End Function

End Class