Imports atcData

Friend Class atcListGridSource
  Inherits atcControls.atcGridSource

  Private pDataManager As atcDataManager
  Private pDataGroup As atcDataGroup

  Sub New(ByVal aDataManager As atcData.atcDataManager, _
          ByVal aDataGroup As atcData.atcDataGroup)
    pDataManager = aDataManager
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
      Return pDataManager.DisplayAttributes.Count()
    End Get
    Set(ByVal Value As Integer)
    End Set
  End Property

  Public Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
    Get
      If aColumn = 0 Then
        Return pDataManager.DisplayAttributes(aRow)
      Else
        Return pDataGroup(aColumn - 1).Attributes.GetValue(pDataManager.DisplayAttributes(aRow))
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