Imports atcData
Imports atcUtility

Public Class atcTimeseriesGridSource
    Inherits atcControls.atcGridSource

    Private WithEvents pDataGroup As atcDataGroup
    Private pAllDates As atcTimeseries
    Private pDisplayAttributes As ArrayList
    Private pDisplayValues As Boolean
    Private pDisplayValueAttributes As Boolean
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

    Public Property DisplayValueAttributes() As Boolean
        Get
            Return pDisplayValueAttributes
        End Get
        Set(ByVal newValue As Boolean)
            pDisplayValueAttributes = newValue
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
            Dim lCols As Integer = 1 + pDataGroup.Count
            If pDisplayValueAttributes Then
                For Each lTs As atcTimeseries In pDataGroup
                    If lTs.ValueAttributesExist Then lCols += 1
                Next
            End If
            Return lCols
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

    Private Sub CellDataset(ByVal aColumn As Integer, ByRef aTimeseries As atcTimeseries, ByRef aIsValue As Boolean)
        Dim lCol As Integer = 0
        Dim lTs As atcTimeseries
        'If aColumn = 3 Then Stop
        If pDisplayValueAttributes Then
            For lTsIndex As Integer = 0 To pDataGroup.Count - 1
                lTs = pDataGroup.ItemByIndex(lTsIndex)
                lCol += 1
                If lCol = aColumn Then
                    aTimeseries = lTs
                    aIsValue = True
                    Exit Sub
                End If
                If lTs.ValueAttributesExist Then
                    lCol += 1
                    If lCol = aColumn Then
                        aTimeseries = lTs
                        aIsValue = False
                        Exit Sub
                    End If
                End If
            Next
        Else
            aTimeseries = pDataGroup.ItemByIndex(aColumn - 1)
            aIsValue = True
        End If
    End Sub

    Public Overrides Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            Dim lAttributeRows As Integer = pDisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                    Return False
                Case Is < Columns
                    Dim lTs As atcTimeseries = Nothing
                    Dim lIsValue As Boolean = True
                    CellDataset(aColumn, lTs, lIsValue)
                    If lTs Is Nothing Then
                        Return False
                    End If
                    If Not lIsValue Then
                        Return False 'Can't edit anything in a ValueAttribute column yet
                    End If
                    If aRow < lAttributeRows Then
                        Dim lDefinedValue As atcDefinedValue = lTs.Attributes.GetDefinedValue(pDisplayAttributes(aRow))
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
                Case Is <= Columns
                    Dim lTs As atcTimeseries = Nothing
                    Dim lIsValue As Boolean = True
                    CellDataset(aColumn, lTs, lIsValue)
                    If lTs IsNot Nothing Then
                        If aRow < lAttributeRows Then
                            If lIsValue Then
                                Return lTs.Attributes.GetFormattedValue(pDisplayAttributes(aRow))
                            Else
                                Return ""
                            End If
                        ElseIf Not pAllDates Is Nothing Then
                            Try
                                If lTs.Dates IsNot Nothing Then
                                    Dim lDateDisplayed As Double = pAllDates.Value(aRow - lAttributeRows + 1)
                                    Dim lIndex As Integer = Array.BinarySearch(lTs.Dates.Values, lDateDisplayed)
                                    If lIsValue Then
                                        Dim lMaxWidth As Integer = lTs.Attributes.GetValue("FormatMaxWidth", pMaxWidth)
                                        Dim lFormat As String = lTs.Attributes.GetValue("FormatNumeric", pFormat)
                                        Dim lExpFormat As String = lTs.Attributes.GetValue("FormatExp", pExpFormat)
                                        Dim lCantFit As String = lTs.Attributes.GetValue("FormatCantFit", pCantFit)
                                        Dim lSignificantDigits As Integer = lTs.Attributes.GetValue("FormatSignificantDigits", pSignificantDigits)

                                        If lIndex < 0 Then 'Did not find this exact date in this TS
                                            lIndex = Not (lIndex) 'BinarySearch returned not(index of next greater value)
                                            'Test two values closest to lDateDisplayed to see if either is within a millisecond
                                            If lIndex <= lTs.numValues AndAlso Math.Abs(lTs.Dates.Value(lIndex) - lDateDisplayed) < JulianMillisecond Then
                                                Return DoubleToString(lTs.Value(lIndex), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                                            ElseIf lIndex > 0 AndAlso Math.Abs(lTs.Dates.Value(lIndex - 1) - lDateDisplayed) < JulianMillisecond Then
                                                Return DoubleToString(lTs.Value(lIndex - 1), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                                            Else 'No value in this TS is close enough to this date
                                                Return ""
                                            End If
                                        ElseIf Double.IsNaN(lTs.Value(lIndex)) Then
                                            Return ""
                                        Else
                                            Return DoubleToString(lTs.Value(lIndex), lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)
                                        End If
                                    Else
                                        Dim lValueAtts As String = ""
                                        If lTs.ValueAttributesExist(lIndex) Then
                                            For Each lValueAttribute As atcDefinedValue In lTs.ValueAttributes(lIndex)
                                                lValueAtts &= lValueAttribute.Definition.Name & "=" & lTs.ValueAttributes(lIndex).GetFormattedValue(lValueAttribute.Definition.Name) & " "
                                            Next
                                        End If
                                        'Debug.WriteLine(aRow & ", " & aColumn & " = " & lValueAtts)
                                        Return lValueAtts.TrimEnd(" ")
                                    End If
                                Else
                                    'Stop
                                End If
                            Catch 'was not a Timeseries or could not get a value
                            End Try
                        End If
                    End If
                    'Case Else 'Column out of range
            End Select
            Return ""
        End Get
        Set(ByVal newValue As String)
            Dim lAttributeRows As Integer = pDisplayAttributes.Count
            Select Case aColumn
                Case 0 'First column contains attribute name or date
                Case Is < Columns
                    Dim lTs As atcTimeseries = Nothing
                    Dim lIsValue As Boolean = True
                    CellDataset(aColumn, lTs, lIsValue)
                    If lIsValue Then
                        If aRow < lAttributeRows Then
                            Dim lDefinedValue As atcDefinedValue = lTs.Attributes.GetDefinedValue(pDisplayAttributes(aRow))
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