Imports atcData
Imports atcSeasons

Friend Class atcSeasonalAttributesGridSource
  Inherits atcControls.atcGridSource

  Private pDataGroup As atcDataGroup
  Private pSeasons As SortedList
  Private pAttributes As SortedList

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataGroup = aDataGroup
    pAttributes = New SortedList
    pSeasons = New SortedList
    For Each lData As atcDataSet In pDataGroup
      For Each lAttribute As atcDefinedValue In lData.Attributes
        If Not lAttribute.Arguments Is Nothing AndAlso lAttribute.Arguments.ContainsAttribute("SeasonIndex") Then
          Dim lSeasonDefinition As atcSeasonBase = lAttribute.Arguments.GetValue("SeasonDefinition")
          Dim lSeasonName As String = lAttribute.Arguments.GetValue("SeasonName", "")
          'TODO: Format will have to change 00 to 0 for some seasons
          Dim lSeasonKey As String = lSeasonDefinition.ToString & " " _
                                   & Format(lAttribute.Arguments.GetValue("SeasonIndex", ""), "00 ") _
                                   & lSeasonName
          If lSeasonKey.Length > 0 AndAlso Not pSeasons.Contains(lSeasonKey) Then
            pSeasons.Add(lSeasonKey, lSeasonName)
          End If

          Dim lAttributeName As String = Left(lAttribute.Definition.Name, Len(lAttribute.Definition.Name) - Len(lSeasonKey))
          If Not pAttributes.Contains(lAttributeName) Then
            pAttributes.Add(lAttributeName, lAttributeName)
          End If
        End If
      Next
    Next
  End Sub

  Protected Overrides Property ProtectedColumns() As Integer
    Get
      If pSeasons Is Nothing Then
        Return 3
      Else
        Return pSeasons.Count + 2
      End If
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Protected Overrides Property ProtectedRows() As Integer
    Get
      Try
        Return pDataGroup.Count * pAttributes.Count + 1
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
          Case 1 : Return "Attribute"
          Case Else : Return pSeasons.GetByIndex(aColumn - 2)
        End Select
      Else
        Dim lAttributeIndex As Integer = (aRow - 1) Mod pAttributes.Count
        Select Case aColumn
          Case 0 : Return pDataGroup((aRow - 1) \ pAttributes.Count).ToString
          Case 1 : Return pAttributes.GetByIndex(lAttributeIndex)
          Case Else
            Dim lSeasonalAttrName As String = pAttributes.GetByIndex(lAttributeIndex) & pSeasons.GetKey(aColumn - 2)
            Return pDataGroup((aRow - 1) \ pAttributes.Count).Attributes.GetFormattedValue(lSeasonalAttrName)
        End Select
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Protected Overrides Property ProtectedAlignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      If aColumn > 1 Then
        Return atcControls.atcAlignment.HAlignDecimal
      Else
        Return atcControls.atcAlignment.HAlignLeft
      End If
    End Get
    Set(ByVal Value As atcControls.atcAlignment)
    End Set
  End Property
End Class
