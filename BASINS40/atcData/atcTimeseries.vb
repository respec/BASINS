Imports System.Math

Public Class atcTimeseries
  Inherits atcDataSet

  Private pDates As atcTimeseries
  'Private pDateLengths As atcTimeseries

  Private pDataSource As atcDataSource
  Private pValuesNeedToBeRead As Boolean

  Private pNumValues As Integer
  Private pValues() As Double
  Private pValueAttributes() As atcDataAttributes

  'Set or get an individual value
  Public Property Value(ByVal index As Integer) As Double
    Get
      EnsureValuesRead()
      If index >= 0 And index <= pNumValues Then
        Return pValues(index)
      Else
        'TODO: handle request for value outside range as error?
        Return Double.NaN
      End If
    End Get
    Set(ByVal newValue As Double)
      If index >= 0 And index <= pNumValues Then
        pValues(index) = newValue
      Else
        'TODO: handle setting value outside range as error? Expand as needed?
      End If
    End Set
  End Property

  'Set or get the entire array of values
  Public Property Values() As Double()
    Get
      EnsureValuesRead()
      Return pValues
    End Get
    Set(ByVal newValues() As Double)
      pValues = newValues
      pNumValues = newValues.GetUpperBound(0)
    End Set
  End Property

  'Get an attribute of a value without creating pValueAttributes or pValueAttributes(index)
  Public Function ValueAttributesGetValue(ByVal index As Integer, ByVal aAttributeName As String, ByVal aDefault As Object) As Object
    If pValueAttributes Is Nothing OrElse pValueAttributes(index) Is Nothing Then
      Return aDefault
    Else
      Return pValueAttributes(index).GetValue(aAttributeName, aDefault)
    End If
  End Function

  'Get whether a ValueAttribute exists for a Value without creating pValueAttributes or pValueAttributes(index)
  Public Function ValueAttributesExist(ByVal index As Integer) As Boolean
    If pValueAttributes Is Nothing OrElse pValueAttributes(index) Is Nothing Then
      Return False
    Else
      Return True
    End If
  End Function

  'Attributes associated with individual values (quality flags)
  Public Property ValueAttributes(ByVal index As Integer) As atcDataAttributes
    Get
      EnsureValuesRead()
      If index >= 0 And index <= pNumValues Then
        If pValueAttributes Is Nothing Then 'Need to allocate pValueAttributes
          ReDim pValueAttributes(pNumValues)
        End If
        If pValueAttributes(index) Is Nothing Then 'Create new atcDataAttributes for this value
          pValueAttributes(index) = New atcDataAttributes
          pValueAttributes(index).Owner = Me
        End If
        Return pValueAttributes(index)
      Else
        'TODO: handle request for value outside range as error?
        Return Nothing
      End If
    End Get
    Set(ByVal newValue As atcDataAttributes)
      EnsureValuesRead()
      If index >= 0 And index <= pNumValues Then
        If pValueAttributes Is Nothing Then 'Need to allocate pValueAttributes
          ReDim pValueAttributes(pNumValues)
        End If
        pValueAttributes(index) = newValue
      Else
        'TODO: handle setting value outside range as error?
      End If
    End Set
  End Property

  'Each value in Dates is the instant of measurement or the start of the interval
  '(Julian days since the start of 1900)
  Public Property Dates() As atcTimeseries
    Get
      Return pDates
    End Get
    Set(ByVal newValue As atcTimeseries)
      pDates = newValue
    End Set
  End Property

  ''True if we are representing intervals rather than instants
  'Public ReadOnly Property HasDateLengths() As Boolean
  '    Get
  '        Return Not (pDateLengths Is Nothing)
  '    End Get
  'End Property

  ''The length of each interval. Values should be greater than zero.
  'Public Property DateLengths() As atcTimeseries
  '    Get
  '        Return pDateLengths
  '    End Get
  '    Set(ByVal newValue As atcTimeseries)
  '        pDateLengths = newValue
  '    End Set
  'End Property

  'Clear all values and attributes, but not Dates
  Public Overrides Sub Clear()
    MyBase.Clear()
    ReDim pValues(0)
    numValues = 0
    If Not pValueAttributes Is Nothing Then
      ReDim pValueAttributes(pNumValues)
    End If
    'pDates = New atcTimeseries(aFile)
  End Sub

  Public Overrides Function Clone() As atcDataSet
    EnsureValuesRead()
    Dim lClone As New atcTimeseries(pDataSource)
    With lClone
      .Attributes.ChangeTo(Attributes)
      If Not pDates Is Nothing Then .Dates = pDates.Clone
      If Not pValues Is Nothing Then .Values = pValues.Clone
      If Not pValueAttributes Is Nothing Then
        For lValueAttIndex As Integer = 0 To pNumValues
          .ValueAttributes(lValueAttIndex) = pValueAttributes(lValueAttIndex).Clone
        Next
      End If
    End With
    Return lClone
  End Function

  'Create a new Timeseries and reference the file it came from
  Public Sub New(ByVal aDataSource As atcDataSource)
    MyBase.New()
    Clear()
    pDataSource = aDataSource
    Try
      Me.Attributes.SetValue("Data Source", aDataSource.Specification)
    Catch ex As Exception
      'atcDataSource is Nothing or is not really an atcDataSource
    End Try
  End Sub

  'Get or set the number of values
  Public Property numValues() As Long
    Get
      If pValuesNeedToBeRead Then 'might have only read header
        EnsureValuesRead()
      End If
      Return pNumValues
    End Get
    Set(ByVal newValue As Long)
      pNumValues = newValue
      ReDim Preserve pValues(pNumValues)
    End Set
  End Property

  'Make sure values have been read from the file
  Public Sub EnsureValuesRead()
    If pValuesNeedToBeRead Then
      'just header information was read at first, delaying using the time/space to read all the data
      pDataSource.ReadData(Me) 'ValuesNeedToBeRead = False should happen in pFile.ReadData            
    End If
  End Sub

  'True if we have read the header and not all the values to save time and memory
  'Should only be changed by the atcDataSource that reads this Timeseries (aFile from New)
  Public Property ValuesNeedToBeRead() As Boolean
    Get
      Return pValuesNeedToBeRead
    End Get
    Set(ByVal newValue As Boolean)
      pValuesNeedToBeRead = newValue
    End Set
  End Property

  'Return index of aValue or -1 if not found
  Public Function IndexOfValue(ByVal aValue As Double, ByVal aAssumeSorted As Boolean) As Integer
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
      If Math.Abs(pValues(lHigher) - aValue) < Double.Epsilon Then Return lHigher
      If lLower > 0 AndAlso Math.Abs(pValues(lLower) - aValue) < Double.Epsilon Then Return lLower
    Else 'do a linear search, find wanted value in up to pNumValues steps
      For lProbe As Integer = 1 To pNumValues
        If Math.Abs(pValues(lProbe) - aValue) < Double.Epsilon Then Return lprobe
      Next
    End If
    Return -1
  End Function
End Class
