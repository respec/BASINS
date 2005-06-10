'atcTimeseriesGroup is a group of atcTimeseries objects and associated selection information
'
'Sharable between different views of the same data
'Events are defined to allow different views to remain synchronized

Imports atcUtility

Public Class atcTimeseriesGroup
  Implements IEnumerable

  Private pTS As ArrayList 'of atcTimeseries

  Private pSelectedTS As atcTimeseriesGroup 'tracks currently selected group within this group

  'One or more atcTimeseries were just added to the group
  Public Event Added(ByVal aAdded As ArrayList)

  'One or more atcTimeseries were just removed from the group
  Public Event Removed(ByVal aRemoved As ArrayList)

  Public Sub New()
    pTS = New ArrayList
  End Sub

  Private Property PrivateList() As ArrayList
    Get
      Return pTS
    End Get
    Set(ByVal newValue As ArrayList)
      pTS = newValue
    End Set
  End Property

  'Get a Timeseries by index
  Default Public Property Item(ByVal index As Integer) As atcTimeseries
    Get
      Return pTS.Item(index)
    End Get
    Set(ByVal newValue As atcTimeseries)
      pTS.Item(index) = newValue
    End Set
  End Property

  'Add a Timeseries to the group
  Public Sub Add(ByVal aTS As atcTimeseries)
    Dim addList As New ArrayList(1)
    addList.Add(aTS)
    Add(addList)
  End Sub

  'Add a list of atcTimeseries to the group
  Public Sub Add(ByVal aList As ArrayList)
    pTS.AddRange(aList)
    RaiseEvent Added(aList) 'Insert is the other place where items are added
  End Sub

  'Remove all Timeseries and selection
  Public Sub Clear()
    Remove(pTS)
    If Not pSelectedTS Is Nothing Then pSelectedTS.Clear()
  End Sub

  Public Function Clone() As atcTimeseriesGroup
    Dim newGroup As New atcTimeseriesGroup
    newGroup.Add(pTS)
    Return newGroup
  End Function

  'Change this group to match the new group and raise the appropriate events
  Public Sub ChangeTo(ByVal aNewGroup As atcTimeseriesGroup)
    If aNewGroup Is Nothing Then
      Clear()
    Else

      Dim RemoveList As New ArrayList
      For Each oldTS As atcTimeseries In pTS
        If Not aNewGroup.Contains(oldTS) Then
          RemoveList.Add(oldTS)
        End If
      Next

      Dim AddList As New ArrayList
      For Each savedTS As atcTimeseries In aNewGroup
        If Not pTS.Contains(savedTS) Then
          AddList.Add(savedTS)
        End If
      Next

      pTS = aNewGroup.PrivateList.Clone
      RaiseEvent Added(AddList)
      RaiseEvent Removed(RemoveList)

    End If
  End Sub

  Public Function Contains(ByVal aTS As atcTimeseries) As Boolean
    Return pTS.Contains(aTS)
  End Function

  Public ReadOnly Property Count() As Integer
    Get
      Return pTS.Count
    End Get
  End Property

  'Allow use of "For Each" on the group
  Public Function GetEnumerator() As Collections.IEnumerator Implements Collections.IEnumerable.GetEnumerator
    Return pTS.GetEnumerator
  End Function

  'Find the index of the specified Timeseries
  Public Function IndexOf(ByVal aTS As atcTimeseries) As Integer
    Return pTS.IndexOf(aTS)
  End Function

  'Find the index of the specified Timeseries given that it is at or after aStartIndex
  Public Function IndexOf(ByVal aTS As atcTimeseries, ByVal aStartIndex As Integer) As Integer
    Return pTS.IndexOf(aTS, aStartIndex)
  End Function

  Public Function IndexOfSerial(ByVal aSerial As Integer) As Integer
    For iTS As Integer = 0 To pTS.Count - 1
      If pTS(iTS).Serial = aSerial Then Return iTS
    Next
    Return -1
  End Function

  'Insert a new Timeseries at the specified index
  Public Sub Insert(ByVal aIndex As Integer, ByVal aTS As atcTimeseries)
    pTS.Insert(aIndex, aTS)

    Dim addList As New ArrayList(1)
    addList.Add(aTS)
    RaiseEvent Added(addList)
  End Sub

  'Remove a Timeseries from the group
  Public Sub Remove(ByVal aTS As atcTimeseries)
    Dim removeList As New ArrayList(1)
    removeList.Remove(aTS)
    Remove(removeList)
  End Sub

  'Remove a list of atcTimeseries from the group
  Public Sub Remove(ByVal aList As ArrayList)
    If aList.Count > 0 Then
      For Each ts As atcTimeseries In aList
        pTS.Remove(ts)
      Next
      RaiseEvent Removed(aList)
    End If
  End Sub

  'Remove a span of one or more Timeseries from the group by index
  Public Sub Remove(ByVal aIndex As Integer, Optional ByVal aNumber As Integer = 1)
    Dim removeList As New ArrayList(aNumber)
    For index As Integer = aIndex To aIndex + aNumber - 1
      removeList.Add(pTS.Item(index))
    Next
    Remove(removeList)
  End Sub

  Public Property SelectedTimeseries() As atcTimeseriesGroup
    Get
      If pSelectedTS Is Nothing Then 'Initialize now if not already done
        pSelectedTS = New atcTimeseriesGroup
      End If
      Return pSelectedTS
    End Get
    Set(ByVal newValue As atcTimeseriesGroup)
      pSelectedTS = newValue
    End Set
  End Property

  Public Overrides Function ToString() As String
    ToString = pTS.Count & " Timeseries:"
    For Each lts As atcTimeseries In pTS
      ToString &= vbCrLf & "  " & lts.ToString
    Next
  End Function
End Class
