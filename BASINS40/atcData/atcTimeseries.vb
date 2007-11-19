''' <summary>Base class for timeseries data</summary>
Public Class atcTimeseries
    Inherits atcDataSet

    Private pDates As atcTimeseries
    Private pDataSource As atcDataSource
    Private pValuesNeedToBeRead As Boolean

    Private pNumValues As Integer
    Private pValues() As Double
    Private pValueAttributes() As atcDataAttributes

    Private Const pEpsilon As Double = 0.000000001
    Private Shared pNaN As Double = atcUtility.GetNaN

    ''' <summary>Set or get an individual value</summary>
    Public Property Value(ByVal aIndex As Integer) As Double
        Get
            EnsureValuesRead()
            If aIndex >= 0 And aIndex <= pNumValues Then
                Return pValues(aIndex)
            Else
                Throw New ApplicationException("Timeseries Value Index " & aIndex & " out of range 0-" & pNumValues)
            End If
        End Get
        Set(ByVal newValue As Double)
            If aIndex >= 0 And aIndex <= pNumValues Then
                pValues(aIndex) = newValue
            Else
                'TODO: handle setting value outside range as error? Expand as needed?
            End If
        End Set
    End Property

    ''' <summary>First number in Values that is not NaN or Infinity, NaN if no numeric values</summary>
    Public Function FirstNumeric() As Double
        Dim lValue As Double = pNaN
        For lIndex As Integer = 0 To pNumValues
            lValue = Value(lIndex)
            If Not Double.IsNaN(lValue) AndAlso Not Double.IsInfinity(lValue) Then Exit For
        Next
        Return lValue
    End Function

    ''' <summary>Set or get the entire array of values</summary>
    Public Property Values() As Double()
        Get
            EnsureValuesRead()
            Return pValues
        End Get
        Set(ByVal newValues() As Double)
            pValues = newValues
            pNumValues = newValues.GetUpperBound(0)
            If Not pDates Is Nothing AndAlso pDates.numValues <> pNumValues Then
                pDates.numValues = pNumValues
            End If
            If Not pValueAttributes Is Nothing AndAlso pValueAttributes.GetUpperBound(0) <> pNumValues Then
                ReDim Preserve pValueAttributes(pNumValues)
            End If
            Attributes.DiscardCalculated()
        End Set
    End Property

    ''' <summary>Get the value of an attribute 
    ''' without creating pValueAttributes or pValueAttributes(index)</summary>
    Public Function ValueAttributesGetValue(ByVal aIndex As Integer, ByVal aAttributeName As String, ByVal aDefault As Object) As Object
        If pValueAttributes Is Nothing OrElse pValueAttributes(aIndex) Is Nothing Then
            Return aDefault
        Else
            Return pValueAttributes(aIndex).GetValue(aAttributeName, aDefault)
        End If
    End Function

    ''' <summary>Get whether a ValueAttribute exists by index</summary>
    Public Function ValueAttributesExist(Optional ByVal aIndex As Integer = -1) As Boolean
        If pValueAttributes Is Nothing OrElse aIndex > 0 AndAlso pValueAttributes(aIndex) Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>Attributes associated with individual data values</summary>
    ''' <remarks>May be used to store data quality flags or other metadata.</remarks>
    Public Property ValueAttributes(ByVal aIndex As Integer) As atcDataAttributes
        Get
            EnsureValuesRead()
            If aIndex >= 0 And aIndex <= pNumValues Then
                If pValueAttributes Is Nothing Then 'Need to allocate pValueAttributes
                    ReDim pValueAttributes(pNumValues)
                End If
                If pValueAttributes(aIndex) Is Nothing Then 'Create new atcDataAttributes for this value
                    pValueAttributes(aIndex) = New atcDataAttributes
                    pValueAttributes(aIndex).Owner = Me
                End If
                Return pValueAttributes(aIndex)
            Else
                'TODO: handle request for value outside range as error?
                Return Nothing
            End If
        End Get
        Set(ByVal newValue As atcDataAttributes)
            EnsureValuesRead()
            If aIndex >= 0 And aIndex <= pNumValues Then
                If pValueAttributes Is Nothing Then 'Need to allocate pValueAttributes
                    ReDim pValueAttributes(pNumValues)
                End If
                pValueAttributes(aIndex) = newValue
            Else
                'TODO: handle setting value outside range as error?
            End If
        End Set
    End Property

    ''' <summary>Each value in Dates is the instant of measurement or the start of the interval</summary>
    ''' <remarks>
    ''' Dates are julian days since the start of 1900. 
    ''' Fractional part of a date is time of day.
    ''' </remarks>
    Public Property Dates() As atcTimeseries
        Get
            EnsureValuesRead()
            Return pDates
        End Get
        Set(ByVal newValue As atcTimeseries)
            pDates = newValue
        End Set
    End Property

    ''' <summary>Clear all values and attributes, but not dates.</summary>
    Public Overrides Sub Clear()
        MyBase.Clear()
        ReDim pValues(0)
        numValues = 0
        If Not pValueAttributes Is Nothing Then
            ReDim pValueAttributes(pNumValues)
        End If
    End Sub

    ''' <summary>Make a copy of the current dataset and return it</summary>
    Public Overrides Function Clone() As atcDataSet
        EnsureValuesRead()
        Dim lClone As New atcTimeseries(pDataSource)
        With lClone
            .Attributes.ChangeTo(Attributes)
            If Not pDates Is Nothing Then .Dates = pDates.Clone
            If Not pValues Is Nothing Then .Values = pValues.Clone
            If Not pValueAttributes Is Nothing Then
                For lValueAttIndex As Integer = 0 To pNumValues
                    If Not pValueAttributes(lValueAttIndex) Is Nothing Then
                        .ValueAttributes(lValueAttIndex) = pValueAttributes(lValueAttIndex).Clone
                    End If
                Next
            End If
        End With
        Return lClone
    End Function

    ''' <summary>Create a new timeseries and reference the source that it came from</summary>
    Public Sub New(ByVal aDataSource As atcDataSource)
        MyBase.New()

        Clear()
        pDataSource = aDataSource
        If Not aDataSource Is Nothing Then
            Try
                Attributes.SetValue("Data Source", aDataSource.Specification)
            Catch ex As Exception
                'atcDataSource is not really an atcDataSource
            End Try
        End If
    End Sub

    ''' <summary>Number of data values in data set</summary>
    Public Property numValues() As Long
        Get
            If pValuesNeedToBeRead Then 'might have only read header
                EnsureValuesRead()
            End If
            Return pNumValues
        End Get
        Set(ByVal newValue As Long)
            If pNumValues <> newValue Then
                pNumValues = newValue
                ReDim Preserve pValues(pNumValues)
            End If
            If Not pDates Is Nothing AndAlso pDates.numValues <> newValue Then
                pDates.numValues = newValue
            End If
            If Not pValueAttributes Is Nothing AndAlso pValueAttributes.GetUpperBound(0) <> newValue Then
                ReDim Preserve pValueAttributes(pNumValues)
            End If
        End Set
    End Property

    ''' <summary>Make sure values have been read from the source.</summary>
    Public Sub EnsureValuesRead()
        If pValuesNeedToBeRead AndAlso Not pDataSource Is Nothing Then
            'just header information was read at first, delaying using the time/space to read all the data
            ValuesNeedToBeRead = False
            pDataSource.ReadData(Me)
        End If
    End Sub

    ''' <summary>True if we have read the header and not all the values (to save time and memory)</summary>
    ''' <remarks>
    '''     Should only be changed by the
    '''     <see cref="atcData.atcDataSource">atcDataSource</see> that reads this
    '''     timeseries (aFile from New)
    ''' </remarks>
    Public Property ValuesNeedToBeRead() As Boolean
        Get
            Return pValuesNeedToBeRead
        End Get
        Set(ByVal newValue As Boolean)
            pValuesNeedToBeRead = newValue
            If Not pDates Is Nothing Then
                pDates.ValuesNeedToBeRead = newValue
            End If
        End Set
    End Property

    ''' <summary>Return index of aValue or -1 if not found</summary>
    Public Function IndexOfValue(ByVal aValue As Double, _
                                 ByVal aAssumeSorted As Boolean) As Integer
        If aAssumeSorted Then 'do a binary search, find wanted value in log2(pNumValues) steps
            Dim lHigher As Integer = pNumValues
            Dim lLower As Integer = 0 'Note: this starts one *lower than* start of where to search in array
            Dim lProbe As Integer
            While (lHigher - lLower > 1)
                lProbe = (lHigher + lLower) / 2
                If pValues(lProbe) < aValue Then
                    lLower = lProbe
                Else
                    lHigher = lProbe
                End If
            End While
            If Math.Abs(pValues(lHigher) - aValue) < pEpsilon Then Return lHigher
            If lLower > 0 AndAlso Math.Abs(pValues(lLower) - aValue) < pEpsilon Then Return lLower
        Else 'do a linear search, find wanted value in up to pNumValues steps
            For lProbe As Integer = 1 To pNumValues
                If Math.Abs(pValues(lProbe) - aValue) < pEpsilon Then Return lProbe
            Next
        End If
        Return -1
    End Function
End Class
