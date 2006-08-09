Imports atcData
Imports atcUtility

Friend Class atcListGridSource
    Inherits atcControls.atcGridSource

    Private pDataManager As atcDataManager
    Private WithEvents pDataGroup As atcDataGroup
    Private pAllDates As atcTimeseries
    Private pDateFormat As New atcDateFormat

    Private Sub RefreshAllDates()
        Try
            Select Case pDataGroup.Count
                Case 1
                    Dim lTS As atcTimeseries = pDataGroup.ItemByIndex(0)
                    lTS.EnsureValuesRead()
                    pAllDates = lTS.Dates
                Case Is > 1 : pAllDates = MergeTimeseries(pDataGroup).Dates
                Case Else : pAllDates = New atcTimeseries(Nothing)
            End Select
        Catch
            pAllDates = Nothing
        End Try
    End Sub

    Sub New(ByVal aDataManager As atcData.atcDataManager, _
            ByVal aDataGroup As atcData.atcDataGroup)
        pDataManager = aDataManager
        pDataGroup = aDataGroup
        RefreshAllDates()
    End Sub

    Overrides Property Columns() As Integer
        Get
            Return pDataGroup.Count + 1
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            If pAllDates Is Nothing Then
                Return pDataManager.DisplayAttributes.Count
            Else
                Return pDataManager.DisplayAttributes.Count + pAllDates.numValues
            End If
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            Dim lAttributeRows As Integer = pDataManager.DisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                    If aRow < lAttributeRows Then
                        Return pDataManager.DisplayAttributes(aRow)
                    Else
                        Return pDateFormat.JDateToString(pAllDates.Value(aRow - lAttributeRows + 1))
                    End If
                Case Is <= pDataGroup.Count
                    If aRow < lAttributeRows Then
                        Return pDataGroup(aColumn - 1).Attributes.GetFormattedValue(pDataManager.DisplayAttributes(aRow))
                    Else
                        Try
                            Dim lTS As atcTimeseries = pDataGroup.ItemByIndex(aColumn - 1)
                            Dim lDateDisplayed As Double = pAllDates.Value(aRow - lAttributeRows + 1)
                            Dim lIndex As Integer = Array.BinarySearch(lTS.Dates.Values, lDateDisplayed)

                            If lIndex < 0 Then 'Did not find this exact date in this TS
                                lIndex = Not (lIndex) 'BinarySearch returned not(index of next greater value)
                                'Test two values closest to lDateDisplayed to see if either is within a millisecond
                                If lIndex <= pAllDates.numValues AndAlso Math.Abs(lTS.Dates.Value(lIndex) - lDateDisplayed) < JulianMillisecond Then
                                    Return DoubleToString(lTS.Value(lIndex))
                                ElseIf lIndex > 0 AndAlso Math.Abs(lTS.Dates.Value(lIndex - 1) - lDateDisplayed) < JulianMillisecond Then
                                    Return DoubleToString(lTS.Value(lIndex - 1))
                                Else 'No value in this TS is close enough to this date
                                    Return ""
                                End If
                            Else
                                Return DoubleToString(lTS.Value(lIndex))
                            End If
                        Catch 'was not a Timeseries or could not get a value
                            Return ""
                        End Try
                    End If
                Case Else 'Column out of range
                    Return ""
            End Select
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            If aColumn = 0 Then
                Return atcControls.atcAlignment.HAlignLeft
            Else
                Return atcControls.atcAlignment.HAlignDecimal
            End If
        End Get
        Set(ByVal Value As atcControls.atcAlignment)

        End Set
    End Property

    Private Sub pDataGroup_Added(ByVal aAdded As atcUtility.atcCollection) Handles pDataGroup.Added
        RefreshAllDates()
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcUtility.atcCollection) Handles pDataGroup.Removed
        RefreshAllDates()
    End Sub
End Class