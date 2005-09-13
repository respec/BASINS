Imports atcData

Friend Class atcSeasonalAttributesGridSource
  Inherits atcControls.atcGridSource

  Private pDataGroup As atcDataGroup
  Private pSeasons As atcDataAttributes
  Private pAttributes As atcDataAttributes

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataGroup = aDataGroup
  End Sub

  Public Overrides Property Columns() As Integer
    Get
      Return pDataGroup.Count + 1
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property Rows() As Integer
    Get
      Return pDataGroup.Count * pAttributes.count
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aColumn = 0 Then
        Return pDataGroup(aRow).ToString
      Else
        'Return pDataGroup(aColumn - 1).Attributes.GetValue(pDataManager.DisplayAttributes(aRow))
      End If
    End Get
    Set(ByVal Value As String)
    End Set
  End Property

  Public Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
    Get
      Return atcControls.atcAlignment.HAlignLeft
    End Get
    Set(ByVal Value As atcControls.atcAlignment)

    End Set
  End Property
End Class
