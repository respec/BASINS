'atcDataGroup is a group of atcDataSet objects and associated selection information
'
'Sharable between different views of the same data
'Events are defined to allow different views to remain synchronized

Imports atcUtility

Public Class atcDataGroup
  Implements IEnumerable

  Private pTS As ArrayList 'of atcDataSet

  Private pSelectedTS As atcDataGroup 'tracks currently selected group within this group

  'One or more atcDataSet were just added to the group
  Public Event Added(ByVal aAdded As ArrayList)

  'One or more atcDataSet were just removed from the group
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

  'Get a atcDataSet by index
  Default Public Property Item(ByVal index As Integer) As atcDataSet
    Get
      Return pTS.Item(index)
    End Get
    Set(ByVal newValue As atcDataSet)
      pTS.Item(index) = newValue
    End Set
  End Property

  'Add a atcDataSet to the group
  Public Sub Add(ByVal aTS As atcDataSet)
    Dim addList As New ArrayList(1)
    addList.Add(aTS)
    Add(addList)
  End Sub

  'Add a list of atcDataSet to the group
  Public Sub Add(ByVal aList As ArrayList)
    pTS.AddRange(aList)
    RaiseEvent Added(aList) 'Insert is the other place where items are added
  End Sub

  'Remove all atcDataSets and selection
  Public Sub Clear()
    If pTS.Count > 0 Then
      Dim lRemoved As ArrayList = pTS
      pTS = New ArrayList
      RaiseEvent Removed(lRemoved)
    End If
    If Not pSelectedTS Is Nothing Then pSelectedTS.Clear()
  End Sub

  Public Function Clone() As atcDataGroup
    Dim newGroup As New atcDataGroup
    newGroup.Add(pTS)
    Return newGroup
  End Function

  'Change this group to match the new group and raise the appropriate events
  Public Sub ChangeTo(ByVal aNewGroup As atcDataGroup)
    If aNewGroup Is Nothing Then
      Clear()
    Else

      Dim RemoveList As New ArrayList
      For Each oldTS As atcDataSet In pTS
        If Not aNewGroup.Contains(oldTS) Then
          RemoveList.Add(oldTS)
        End If
      Next

      Dim AddList As New ArrayList
      For Each savedTS As atcDataSet In aNewGroup
        If Not pTS.Contains(savedTS) Then
          AddList.Add(savedTS)
        End If
      Next

      pTS = aNewGroup.PrivateList.Clone
      RaiseEvent Added(AddList)
      RaiseEvent Removed(RemoveList)

    End If
  End Sub

  Public Function Contains(ByVal aTS As atcDataSet) As Boolean
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

  'Find the index of the specified atcDataSet
  Public Function IndexOf(ByVal aDataSet As atcDataSet) As Integer
    Return pTS.IndexOf(aDataset)
  End Function

  'Find the index of the specified DataSet given that it is at or after aStartIndex
  Public Function IndexOf(ByVal aDataSet As atcDataSet, ByVal aStartIndex As Integer) As Integer
    Return pTS.IndexOf(aDataSet, aStartIndex)
  End Function

  Public Function IndexOfSerial(ByVal aSerial As Integer) As Integer
    For iTS As Integer = 0 To pTS.Count - 1
      If pTS(iTS).Serial = aSerial Then Return iTS
    Next
    Return -1
  End Function

  'Insert a new Timeseries at the specified index
  Public Sub Insert(ByVal aIndex As Integer, ByVal aDataSet As atcDataSet)
    pTS.Insert(aIndex, aDataSet)

    Dim addList As New ArrayList(1)
    addList.Add(aDataSet)
    RaiseEvent Added(addList)
  End Sub

  'Remove a Timeseries from the group
  Public Sub Remove(ByVal aDataSet As atcDataSet)
    Dim removeList As New ArrayList(1)
    removeList.Add(aDataSet)
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

  Public Property SelectedTimeseries() As atcDataGroup
    Get
      If pSelectedTS Is Nothing Then 'Initialize now if not already done
        pSelectedTS = New atcDataGroup
      End If
      Return pSelectedTS
    End Get
    Set(ByVal newValue As atcDataGroup)
      pSelectedTS = newValue
    End Set
  End Property

  Public Overrides Function ToString() As String
    ToString = pTS.Count & " Data:"
    For Each lts As atcDataSet In pTS
      ToString &= vbCrLf & "  " & lts.ToString
    Next
  End Function
End Class
