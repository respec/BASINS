Imports atcData
Imports atcUtility

Public Class atcTimeseriesGridSource
    Inherits atcControls.atcGridSource

    Private WithEvents pDataGroup As atcDataGroup
    Private pAllDates As atcTimeseries
    Private pDisplayAttributes As ArrayList
    Private pDisplayValues As Boolean
    Private pFilterNoData As Boolean

    Private pDateFormat As New atcDateFormat

    'Value formatting options, can be overridden by timeseries attributes
    Private pMaxWidth As Integer = 10
    Private pFormat As String = "#,##0.########"
    Private pExpFormat As String = "#.#e#"
    Private pCantFit As String = "#"
    Private pSignificantDigits As Integer = 5

    Sub New(ByVal aDataGroup As atcData.atcDataGroup, _
            ByVal aDisplayAttributes As ArrayList, _
            ByVal aDisplayValues As Boolean)
        Me.New(aDataGroup, aDisplayAttributes, aDisplayValues, False)
    End Sub

    Sub New(ByVal aDataGroup As atcData.atcDataGroup, _
            ByVal aDisplayAttributes As ArrayList, _
            ByVal aDisplayValues As Boolean, _
            ByVal aFilterNoData As Boolean)
        pDataGroup = aDataGroup
        pDisplayAttributes = aDisplayAttributes
        pDisplayValues = aDisplayValues
        pFilterNoData = aFilterNoData
        RefreshAllDates()
    End Sub

    Public Property DateFormat() As atcDateFormat
        Get
            Return pDateFormat
        End Get
        Set(ByVal newValue As atcDateFormat)
            pDateFormat = newValue
        End Set
    End Property

    Public Sub ValueFormat(Optional ByVal aMaxWidth As Integer = 10, _
                           Optional ByVal aFormat As String = "#,##0.########", _
                           Optional ByVal aExpFormat As String = "#.#e#", _
                           Optional ByVal aCantFit As String = "#", _
                           Optional ByVal aSignificantDigits As Integer = 5)
        pMaxWidth = aMaxWidth
        pFormat = aFormat
        pExpFormat = aExpFormat
        pCantFit = aCantFit
        pSignificantDigits = aSignificantDigits
    End Sub

    Private Sub RefreshAllDates()
        Try
            If pDisplayValues Then
                If pFilterNoData OrElse pDataGroup.Count > 1 Then
                    pAllDates = MergeTimeseries(pDataGroup, pFilterNoData).Dates
                ElseIf pDataGroup.Count = 1 Then
                    Dim lTS As atcTimeseries = pDataGroup.ItemByIndex(0)
                    lTS.EnsureValuesRead()
                    pAllDates = lTS.Dates
                Else
                    pAllDates = New atcTimeseries(Nothing)
                End If
            Else
                pAllDates = Nothing
            End If
        Catch
            pAllDates = Nothing
        End Try
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
                Return pDisplayAttributes.Count
            Else
                Return pDisplayAttributes.Count + pAllDates.numValues
            End If
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Public Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
        Get
            If CellEditable(aRow, aColumn) Then
                Return System.Drawing.SystemColors.Window
            Else
                Return System.Drawing.Color.WhiteSmoke
            End If
            Return MyBase.CellColor(aRow, aColumn)
        End Get
        Set(ByVal value As System.Drawing.Color)
            'MyBase.CellColor(aRow, aColumn) = value
        End Set
    End Property

    Public Overrides Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Dim lAttributeRows As Integer = pDisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                    Return False
                Case Is <= pDataGroup.Count
                    If aRow < lAttributeRows Then
                        Dim lDefinedValue As atcDefinedValue = pDataGroup(aColumn - 1).Attributes.GetDefinedValue(pDisplayAttributes(aRow))
                        If Not lDefinedValue Is Nothing AndAlso lDefinedValue.Definition.Editable Then
                            Select Case lDefinedValue.Definition.TypeString
                                Case "String", "Double", "Single", "Integer" : Return True
                            End Select
                        End If
                        Return False
                    ElseIf Not pAllDates Is Nothing Then
                        Return (CellValue(aRow, aColumn).Length > 0)
                    Else
                        Return False
                    End If
                Case Else 'Column out of range
                    Return False
            End Select
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            Dim lAttributeRows As Integer = pDisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                    If aRow < lAttributeRows Then
                        Return pDisplayAttributes(aRow)
                    ElseIf Not pAllDates Is Nothing Then
                        Return pDateFormat.JDateToString(pAllDates.Value(aRow - lAttributeRows + 1))
                    Else
                        Return ""
                    End If
                Case Is <= pDataGroup.Count
                    If aRow < lAttributeRows Then
                        Return pDataGroup(aColumn - 1).Attributes.GetFormattedValue(pDisplayAttributes(aRow))
                    ElseIf Not pAllDates Is Nothing Then
                        Try
                            Dim lTS As atcTimeseries = pDataGroup.ItemByIndex(aColumn - 1)
                            Dim lDateDisplayed As Double = pAllDates.Value(aRow - lAttributeRows + 1)
                            Dim lIndex As Integer = Array.BinarySearch(lTS.Dates.Values, lDateDisplayed)

                            Dim lMaxWidth As Integer = lTS.Attributes.GetValue("FormatMaxWidth", pMaxWidth)
                            Dim lFormat As String = lTS.Attributes.GetValue("FormatNumeric", pFormat)
                            Dim lExpFormat As String = lTS.Attributes.GetValue("FormatExp", pExpFormat)
                            Dim lCantFit As String = lTS.Attributes.GetValue("FormatCantFit", pCantFit)
                            Dim lSignificantDigits As Integer = lTS.Attributes.GetValue("FormatSignificantDigits", pSignificantDigits)

                            If lIndex < 0 Then 'Did not find this exact date in this TS
                                lIndex = Not (lIndex) 'BinarySearch returned not(index of next greater value)
                                'Test two values closest to lDateDisplayed to see if either is within a millisecond
                                If lIndex <= lTS.numValues AndAlso Math.Abs(lTS.Dates.Value(lIndex) - lDateDisplayed) < JulianMillisecond Then
                                    Return DoubleToString(lTS.Value(lIndex), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                                ElseIf lIndex > 0 AndAlso Math.Abs(lTS.Dates.Value(lIndex - 1) - lDateDisplayed) < JulianMillisecond Then
                                    Return DoubleToString(lTS.Value(lIndex - 1), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                                Else 'No value in this TS is close enough to this date
                                    Return ""
                                End If
                            ElseIf Double.IsNaN(lTS.Value(lIndex)) Then
                                Return ""
                            Else
                                Return DoubleToString(lTS.Value(lIndex), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                            End If
                        Catch 'was not a Timeseries or could not get a value
                            Return ""
                        End Try
                    Else
                        Return ""
                    End If
                Case Else 'Column out of range
                    Return ""
            End Select
        End Get
        Set(ByVal newValue As String)
            Dim lAttributeRows As Integer = pDisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                Case Is <= pDataGroup.Count
                    If aRow < lAttributeRows Then
                        Dim lDefinedValue As atcDefinedValue = pDataGroup(aColumn - 1).Attributes.GetDefinedValue(pDisplayAttributes(aRow))
                        Select Case lDefinedValue.Definition.TypeString
                            Case "String" : lDefinedValue.Value = newValue
                            Case "Double" : lDefinedValue.Value = Double.Parse(newValue)
                            Case "Single" : lDefinedValue.Value = Single.Parse(newValue)
                            Case "Integer" : lDefinedValue.Value = Integer.Parse(newValue)
                            Case Else
                                MapWinUtility.Logger.Msg("Cannot yet edit values of type '" & lDefinedValue.Definition.TypeString & "'")
                        End Select
                    ElseIf Not pAllDates Is Nothing Then
                        'Try
                        Dim lTS As atcTimeseries = pDataGroup.ItemByIndex(aColumn - 1)
                        Dim lDateDisplayed As Double = pAllDates.Value(aRow - lAttributeRows + 1)
                        Dim lIndex As Integer = Array.BinarySearch(lTS.Dates.Values, lDateDisplayed)

                        If lIndex < 0 Then 'Did not find this exact date in this TS
                            lIndex = Not (lIndex) 'BinarySearch returned not(index of next greater value)
                            'Test two values closest to lDateDisplayed to see if either is within a millisecond
                            If lIndex <= lTS.numValues AndAlso Math.Abs(lTS.Dates.Value(lIndex) - lDateDisplayed) < JulianMillisecond Then
                                lTS.Value(lIndex) = CDbl(newValue)
                            ElseIf lIndex > 0 AndAlso Math.Abs(lTS.Dates.Value(lIndex - 1) - lDateDisplayed) < JulianMillisecond Then
                                lTS.Value(lIndex - 1) = CDbl(newValue)
                            Else
                                'TODO: exception?
                            End If
                        Else
                            lTS.Value(lIndex) = CDbl(newValue)
                        End If
                        'Catch 'was not a Timeseries or could not get a value
                        'End Try
                    Else
                        'TODO: exception?
                    End If
                Case Else 'Column out of range
                    'TODO: exception?
            End Select
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