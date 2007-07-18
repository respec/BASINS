Imports atcData
Imports atcSeasons
Imports atcUtility

Friend Class atcSeasonalAttributesGridSource
    Inherits atcControls.atcGridSource

    Private pDataGroup As atcDataGroup
    Private pSeasons As SortedList
    Private pAttributes As SortedList

    Sub New(ByVal aDataGroup As atcData.atcDataGroup)
        pDataGroup = aDataGroup
        pAttributes = New SortedList
        pSeasons = New SortedList
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing AndAlso lAttribute.Arguments.ContainsAttribute("SeasonIndex") Then
                    Dim lSeasonDefinition As atcSeasonBase = lAttribute.Arguments.GetValue("SeasonDefinition")
                    Dim lSeasonName As String = lAttribute.Arguments.GetValue("SeasonName", "")
                    Dim lSeasonKey As String = lSeasonDefinition.ToString & " " _
                                             & Format(lAttribute.Arguments.GetValue("SeasonIndex", ""), "000 ") _
                                             & lSeasonName
                    If lSeasonKey.Length > 0 AndAlso Not pSeasons.Contains(lSeasonKey) Then
                        pSeasons.Add(lSeasonKey, lSeasonName)
                    End If

                    Dim lAttributeName As String = Left(lAttribute.Definition.Name, lAttribute.Definition.Name.IndexOf(lSeasonDefinition.ToString) - 1)
                    If Not pAttributes.Contains(lAttributeName) Then
                        pAttributes.Add(lAttributeName, lAttributeName)
                    End If
                End If
            Next
        Next
    End Sub

    Overrides Property Columns() As Integer
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

    Overrides Property Rows() As Integer
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

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
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
                        With pDataGroup((aRow - 1) \ pAttributes.Count).Attributes
                            Dim lSeasonalAttrName As String = pAttributes.GetByIndex(lAttributeIndex) & " " & pSeasons.GetKey(aColumn - 2)
                            CellValue = .GetFormattedValue(lSeasonalAttrName, "<nothing>") 'works first try only for 3-digit seasons
                            If CellValue.Equals("<nothing>") Then 'work around formatting issue for 2 digit season index
                                CellValue = .GetFormattedValue(ReplaceString(lSeasonalAttrName, " 0", " "), "<nothing>")
                                If CellValue.Equals("<nothing>") Then 'work around formatting issue for 1 digit season index
                                    CellValue = .GetFormattedValue(ReplaceString(lSeasonalAttrName, " 00", " "))
                                End If
                            End If
                        End With
                End Select
            End If
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
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
