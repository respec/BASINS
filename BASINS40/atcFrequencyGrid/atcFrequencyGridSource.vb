Imports atcData

Friend Class atcFrequencyGridSource
  Inherits atcControls.atcGridSource

  Private pDataGroup As atcDataGroup
  Private pNdays As SortedList
  Private pRecurrence As SortedList

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataGroup = aDataGroup
    pRecurrence = New SortedList
    pNdays = New SortedList
    Dim lKey As String
    For Each lData As atcDataSet In pDataGroup
      For Each lAttribute As atcDefinedValue In lData.Attributes
        If Not lAttribute.Arguments Is Nothing Then
          If lAttribute.Arguments.ContainsAttribute("Nday") Then
            Dim lNdays As String = lAttribute.Arguments.GetFormattedValue("Nday")
            lKey = Format(lAttribute.Arguments.GetValue("Nday"), "00000.0000")
            If Not pNdays.ContainsKey(lKey) Then
              pNdays.Add(lKey, lNdays)
            End If
          End If
          If lAttribute.Arguments.ContainsAttribute("Return Period") Then
            Dim lNyears As String = lAttribute.Arguments.GetFormattedValue("Return Period")
            lKey = Format(lAttribute.Arguments.GetValue("Return Period"), "00000.0000")
            If Not pRecurrence.ContainsKey(lKey) Then
              pRecurrence.Add(lKey, lNyears)
            End If
          End If
        End If
      Next
    Next
  End Sub

  Protected Overrides Property ProtectedColumns() As Integer
    Get
      If pNdays Is Nothing Then
        Return 3
      Else
        Return pNdays.Count + 2
      End If
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedRows() As Integer
    Get
      Try
        Return pDataGroup.Count * pRecurrence.Count + 1
      Catch
        Return 1
      End Try
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedCellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aRow = 0 Then
        Select Case aColumn
          Case 0 : Return "Data Set"
          Case 1 : Return "Return Period"
          Case Else : Return pNdays.GetByIndex(aColumn - 2)
        End Select
      Else
        Dim lAttributeIndex As Integer = (aRow - 1) Mod pRecurrence.Count
        Select Case aColumn
          Case 0 : Return pDataGroup((aRow - 1) \ pRecurrence.Count).ToString
          Case 1 : Return pRecurrence.GetByIndex(lAttributeIndex)
          Case Else
            Dim lAttrName As String = pNdays.GetByIndex(aColumn - 2) & "Hi" & pRecurrence.GetByIndex(lAttributeIndex)
            Return pDataGroup((aRow - 1) \ pRecurrence.Count).Attributes.GetFormattedValue(lAttrName)
        End Select
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Protected Overrides Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      If aColumn > 0 Then
        Return atcControls.atcAlignment.HAlignDecimal
      Else
        Return atcControls.atcAlignment.HAlignLeft
      End If
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property
End Class
