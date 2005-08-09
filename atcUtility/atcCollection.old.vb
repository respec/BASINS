Public Class atcCollection
  Implements IEnumerable

  Private pValues As ArrayList
  Private pKeys As ArrayList

  Public Sub New()
    Me.Clear()
  End Sub

  Public Sub Add(ByVal aKey As Object, ByVal aValue As Object)
    pValues.Add(aValue)
    pKeys.Add(aKey)
  End Sub

  Public Sub Clear()
    pValues = New ArrayList
    pKeys = New ArrayList
  End Sub

  Public ReadOnly Property Count() As Integer
    Get
      Return pValues.Count
    End Get
  End Property

  'Supports "For Each"
  Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    Return pValues.GetEnumerator
  End Function

  Public ReadOnly Property ItemByIndex(ByVal aIndex As Integer) As Object
    Get
      Return pValues(aIndex)
    End Get
  End Property

  Public ReadOnly Property ItemByKey(ByVal aKey As String) As Object
    Get
      Return ItemByIndex(IndexFromKey(aKey))
    End Get
  End Property

  'Returns first index where the specified key can be found or -1 if key does not exist
  'if StartAt is greater than zero, skips that many keys before starting search
  'if StartAt is -1, searches from end
  'if StartAt is less than -1, searches from end and skips (1 - StartAt) keys before starting search
  Public Function IndexFromKey(ByVal aKey As Object, Optional ByVal StartAt As Integer = 0) As Integer
    If StartAt >= 0 Then
      Return pKeys.IndexOf(aKey, StartAt)
    Else
      Return pKeys.LastIndexOf(aKey, Count + StartAt + 1)
    End If
  End Function

  'Returns first index where the specified value can be found or -1 if value does not exist
  'if StartAt is greater than zero, skips that many values before starting search
  'if StartAt is -1, searches backwards from the end
  'if StartAt is less than -1, searches from end and skips values before starting search
  Public Function IndexFromValue(ByVal aValue As Object, Optional ByVal StartAt As Integer = 0) As Integer
    If StartAt >= 0 Then
      Return pValues.IndexOf(aValue, StartAt)
    Else
      Return pValues.LastIndexOf(aValue, Count + StartAt + 1)
    End If
  End Function

  Public Function KeyByIndex(ByVal aIndex As Integer) As Object
    Return pKeys(aIndex)
  End Function

  Public Sub RemoveByIndex(ByVal aIndex As Integer)
    If aIndex >= 0 AndAlso aIndex < Count Then
      pValues.RemoveAt(aIndex)
      pKeys.RemoveAt(aIndex)
    End If
  End Sub

  Public Sub RemoveByKey(ByVal aKey As Object)
    RemoveByIndex(IndexFromKey(aKey))
  End Sub

  Public Sub RemoveByValue(ByVal aValue As Object)
    RemoveByIndex(IndexFromValue(aValue))
  End Sub
End Class
