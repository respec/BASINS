Public Class atcCollection
  Inherits ArrayList

  Private pKeys As ArrayList = New ArrayList

  Public Property Keys() As ArrayList
    Get
      Return pKeys
    End Get
    Set(ByVal newValue As ArrayList)
      pKeys = newValue
    End Set
  End Property

  Public Overloads Overrides Function Add(ByVal Value As Object) As Integer
    Return Add(Value, Value)
  End Function
  Public Overloads Function Add(ByVal key As Object, ByVal value As Object) As Integer
    pKeys.Add(key)
    Return MyBase.Add(value)
  End Function

  Public Overrides Sub AddRange(ByVal c As System.Collections.ICollection)
    pKeys.AddRange(c)
    MyBase.AddRange(c)
  End Sub

  Public Overrides Property Capacity() As Integer
    Get
      Return MyBase.Capacity
    End Get
    Set(ByVal newValue As Integer)
      pKeys.Capacity = newValue
      MyBase.Capacity = newValue
    End Set
  End Property

  Public Overrides Sub Clear()
    pKeys.Clear()
    MyBase.Clear()
  End Sub

  Public Overrides Function Clone() As Object
    Dim newClone As New atcCollection
    For index As Integer = 1 To MyBase.Count
      newClone.Add(MyBase.Item(index), pKeys.Item(index))
    Next
    Return newClone
  End Function

  Public Overloads Overrides Sub Insert(ByVal index As Integer, ByVal value As Object)
    Insert(index, value, value)
  End Sub
  Public Overloads Sub Insert(ByVal index As Integer, ByVal key As Object, ByVal value As Object)
    pKeys.Insert(index, key)
    MyBase.Insert(index, value)
  End Sub

  Public Overloads Overrides Sub InsertRange(ByVal index As Integer, ByVal collValues As ICollection)
    InsertRange(index, collValues, collValues)
  End Sub
  Public Overloads Sub InsertRange(ByVal index As Integer, ByVal collKeys As ICollection, ByVal collValues As ICollection)
    pKeys.InsertRange(index, collKeys)
    MyBase.InsertRange(index, collValues)
  End Sub

  Public Overrides Sub Remove(ByVal value As Object)
    Try
      Dim index As Integer = MyBase.IndexOf(value)
      If index >= 0 Then RemoveAt(index)
    Catch e As Exception
    End Try
  End Sub
  Public Sub RemoveByKey(ByVal key As Object)
    Try
      Dim index As Integer = pKeys.IndexOf(key)
      If index >= 0 Then RemoveAt(index)
    Catch e As Exception
    End Try
  End Sub

  Public Overrides Sub RemoveAt(ByVal index As Integer)
    pKeys.RemoveAt(index)
    MyBase.RemoveAt(index)
  End Sub

  Public Overrides Sub RemoveRange(ByVal index As Integer, ByVal count As Integer)
    pKeys.RemoveRange(index, count)
    MyBase.RemoveRange(index, count)
  End Sub

  Public Overloads Overrides Sub Reverse()
    pKeys.Reverse()
    MyBase.Reverse()
  End Sub

  Public Overloads Overrides Sub Reverse(ByVal index As Integer, ByVal count As Integer)
    pKeys.Reverse(index, count)
    MyBase.Reverse(index, count)
  End Sub

  Public Overloads Overrides Sub SetRange(ByVal index As Integer, ByVal values As ICollection)
    SetRange(index, values, values)
  End Sub
  Public Overloads Sub SetRange(ByVal index As Integer, ByVal keys As ICollection, ByVal values As ICollection)
    pKeys.SetRange(index, keys)
    MyBase.SetRange(index, keys)
  End Sub

  Public Overloads Overrides Sub Sort()
    Sort(New Comparer(New System.Globalization.CultureInfo("")))
  End Sub

  Public Overloads Overrides Sub Sort(ByVal comparer As System.Collections.IComparer)
    Sort(1, MyBase.Count, comparer)
  End Sub

  Public Overloads Overrides Sub Sort(ByVal index As Integer, ByVal count As Integer, ByVal comparer As System.Collections.IComparer)
    Err.Raise(1000, Me, "Sort not implemented")
  End Sub

  Public Overrides Sub TrimToSize()
    pKeys.TrimToSize()
    MyBase.TrimToSize()
  End Sub

  Public Property ItemByKey(ByVal key As String) As Object
    Get
      Try
        Dim index As Integer = pKeys.IndexOf(key)
        If index >= 0 Then Return MyBase.Item(index)
      Catch e As Exception
        Return Nothing
      End Try
    End Get
    Set(ByVal newValue As Object)
      Try
        Dim index As Integer = pKeys.IndexOf(key)
        If index >= 0 Then
          MyBase.Item(index) = newValue
        Else
          Add(key, newValue)
        End If
      Catch e As Exception
        Add(key, newValue)
      End Try
    End Set
  End Property
End Class
