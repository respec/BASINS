'atcDataGroup is a group of atcDataSet objects and associated selection information
'
'Sharable between different views of the same data
'Events are defined to allow different views to remain synchronized

Imports atcUtility

Public Class atcDataGroup
  Inherits atcCollection

  Private pSelectedData As atcDataGroup 'tracks currently selected group within this group

  'One or more atcDataSet were just added to the group
  Public Event Added(ByVal aAdded As atcCollection)

  'One or more atcDataSet were just removed from the group
  Public Event Removed(ByVal aRemoved As atcCollection)

  Private Sub RaiseAddedOne(ByVal aDataSet As atcDataSet)
    Dim lDataSets As New atcCollection
    lDataSets.Add(aDataSet)
    RaiseEvent Added(lDataSets)
  End Sub

  Private Sub RaiseRemovedOne(ByVal aDataSet As atcDataSet)
    Dim lDataSets As New atcCollection
    lDataSets.Add(aDataSet)
    RaiseEvent Removed(lDataSets)
  End Sub

  'Get atcDataSet by index
  Default Public Shadows Property Item(ByVal index As Integer) As atcDataSet
    Get
      Return MyBase.Item(index)
    End Get
    Set(ByVal newValue As atcDataSet)
      MyBase.Item(index) = newValue
    End Set
  End Property

  Public Shadows Property ItemByIndex(ByVal index As Integer) As atcDataSet
    Get
      Return MyBase.Item(index)
    End Get
    Set(ByVal newValue As atcDataSet)
      MyBase.Item(index) = newValue
    End Set
  End Property

  Public Shadows Property ItemByKey(ByVal key As Object) As atcDataSet
    Get
      Return MyBase.ItemByKey(key)
    End Get
    Set(ByVal newValue As atcDataSet)
      MyBase.ItemByKey(key) = newValue
    End Set
  End Property

  'Add one atcDataSet to the group with the default key of its serial number
  Public Overloads Overrides Function Add(ByVal aDataSet As Object) As Integer
    Add(aDataSet.Serial, aDataSet)
  End Function

  'Add one atcDataSet to the group with a custom key
  Public Overloads Overrides Function Add(ByVal key As Object, ByVal aDataSet As Object) As Integer
    MyBase.Add(key, aDataSet)
    RaiseAddedOne(aDataSet)
  End Function

  'Add an atcCollection or atcDataGroup of atcDataSet to the group
  Public Overloads Sub Add(ByVal aAddThese As atcCollection)
    MyBase.AddRange(aAddThese)
    RaiseEvent Added(aAddThese)
  End Sub

  'Remove all atcDataSets and selection
  Public Overrides Sub Clear()
    If Not pSelectedData Is Nothing Then pSelectedData.Clear()
    If Count > 0 Then
      Dim lRemoved As atcDataGroup = Me.Clone
      MyBase.Clear()
      RaiseEvent Removed(lRemoved)
    End If
  End Sub

  Public Overrides Function Clone() As Object
    Dim newClone As New atcDataGroup
    For index As Integer = 0 To MyBase.Count - 1
      newClone.Add(MyBase.Keys(index), MyBase.Item(index))
    Next
    Return newClone
  End Function

  'Change this group to match the new group and raise the appropriate events
  Public Shadows Sub ChangeTo(ByVal aNewGroup As atcDataGroup)
    If aNewGroup Is Nothing Then
      Clear()
    Else

      Dim RemoveList As New atcCollection
      For Each oldTS As atcDataSet In Me
        If Not aNewGroup.Contains(oldTS) Then
          RemoveList.Add(oldTS)
        End If
      Next

      Dim AddList As New atcCollection
      For Each savedTS As atcDataSet In aNewGroup
        If Not Contains(savedTS) Then
          AddList.Add(savedTS)
        End If
      Next

      MyBase.ChangeTo(aNewGroup)
      If RemoveList.Count > 0 Then RaiseEvent Removed(RemoveList)
      If AddList.Count > 0 Then RaiseEvent Added(AddList)
    End If
  End Sub

  Public Function IndexOfSerial(ByVal aSerial As Integer) As Integer
    For iTS As Integer = 0 To Count - 1
      If Item(iTS).Serial = aSerial Then Return iTS
    Next
    Return -1
  End Function

  'Insert a new DataSet at the specified index
  Public Overloads Overrides Sub Insert(ByVal aIndex As Integer, ByVal aDataSet As Object)
    MyBase.Insert(aIndex, aDataSet)
    RaiseAddedOne(aDataSet)
  End Sub

  Public Overrides Sub RemoveAt(ByVal index As Integer)
    'Cannot just do: Remove(ItemByIndex(index))
    'because this overriding RemoveAt is called by MyBase.Remove--infinite loop
    Dim lDataSet As atcDataSet = ItemByIndex(index)
    MyBase.RemoveAt(index)
    RaiseRemovedOne(lDataSet)
  End Sub

  'Remove aDataSet from the group
  Public Overloads Overrides Sub Remove(ByVal aDataSet As Object)
    MyBase.Remove(aDataSet)
    RaiseRemovedOne(aDataSet)
  End Sub

  'Remove a list of atcDataSet from the group, can be atcCollection or atcDataGroup
  Public Overloads Sub Remove(ByVal aRemoveThese As atcCollection)
    If Not aRemoveThese Is Nothing AndAlso aRemoveThese.Count > 0 Then
      For Each ts As atcDataSet In aRemoveThese
        MyBase.Remove(ts)
      Next
      RaiseEvent Removed(aRemoveThese)
    End If
  End Sub

  'Remove a span of one or more DataSets from the group by index
  Public Overrides Sub RemoveRange(ByVal aIndex As Integer, ByVal aNumber As Integer)
    Dim lRemoveThese As New atcCollection
    For index As Integer = aIndex To aIndex + aNumber - 1
      lRemoveThese.Add(Item(index))
    Next
    Remove(lRemoveThese)
  End Sub

  Public Property SelectedData() As atcDataGroup
    Get
      If pSelectedData Is Nothing Then 'Initialize now if not already done
        pSelectedData = New atcDataGroup
      End If
      Return pSelectedData
    End Get
    Set(ByVal newValue As atcDataGroup)
      pSelectedData = newValue
    End Set
  End Property

  Public Overrides Function ToString() As String
    ToString = Count & " Data:"
    For Each lts As atcDataSet In Me
      ToString &= vbCrLf & "  " & lts.ToString
    Next
  End Function
End Class
