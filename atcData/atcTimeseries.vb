Public Class atcTimeseries
  Private Shared pNextSerial As Integer = 0

  Private pDates As atcTimeseries
  'Private pDateLengths As atcTimeseries

  Private pFile As atcTimeseriesFile
  Private pValuesNeedToBeRead As Boolean

  Private pSerial As Integer
  Private pNumValues As Integer
  Private pValues() As Double
  Private pAttributes As DataAttributes
  Private pValueAttributes() As DataAttributes

  Public Overrides Function ToString() As String
    Dim id As String = pAttributes.GetValue("id")
    'If id.Length = 0 Then id = "# " & CStr(pSerial)

    Return pAttributes.GetValue("Scenario") & " " _
         & pAttributes.GetValue("Location") & " " _
         & pAttributes.GetValue("Constituent") & " " & id & " # " & CStr(pSerial)
  End Function

  'Set or get an individual value
  Public Property Value(ByVal index As Long) As Double
    Get
      EnsureValuesRead()
      If index >= 0 And index < pNumValues Then
        Return pValues(index)
      Else
        'TODO: handle request for value outside range as error?
        Return Double.NaN
      End If
    End Get
    Set(ByVal newValue As Double)
      If index >= 0 And index < pNumValues Then
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
      pNumValues = newValues.GetUpperBound(0) + 1
    End Set
  End Property

  'Attributes associated with the whole Timeseries (location, constituent, etc.)
  Public ReadOnly Property Attributes() As DataAttributes
    Get
      Return pAttributes
    End Get
  End Property

  'Attributes associated with individual values (quality flags)
  Public Property ValueAttributes(ByVal index As Long) As DataAttributes
    Get
      EnsureValuesRead()
      If index >= 0 And index < pNumValues Then
        If pValueAttributes Is Nothing Then 'Need to allocate pValueAttributes
          ReDim pValueAttributes(pNumValues)
        End If
        If pValueAttributes(index) Is Nothing Then 'Create new DataAttributes for this value
          pValueAttributes(index) = New DataAttributes(Nothing)
        End If
        Return pValueAttributes(index)
      Else
        'TODO: handle request for value outside range as error?
        Return Nothing
      End If
    End Get
    Set(ByVal newValue As DataAttributes)
      EnsureValuesRead()
      If index >= 0 And index < pNumValues Then
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
  Public Sub Clear()
    ReDim pValues(0)
    numValues = 0
    pAttributes = New DataAttributes(Me)
    If Not pValueAttributes Is Nothing Then
      ReDim pValueAttributes(pNumValues)
    End If
    'pDates = New atcTimeseries(aFile)
  End Sub

  'Create a new Timeseries and reference the file it came from
  Public Sub New(ByVal aFile As atcTimeseriesFile)
    Clear()
    pSerial = System.Threading.Interlocked.Increment(pNextSerial) 'Safely increment pNextSerial
    pFile = aFile
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

  'Unique serial number assigned when object is created
  Public ReadOnly Property Serial() As Integer
    Get
      Return pSerial
    End Get
  End Property

  'Make sure values have been read from the file
  Public Sub EnsureValuesRead()
    If pValuesNeedToBeRead Then
      'just header information was read at first, delaying using the time/space to read all the data
      pFile.ReadData(Me) 'ValuesNeedToBeRead = False should happen in pFile.ReadData            
    End If
  End Sub

  'True if we have read the header and not all the values to save time and memory
  'Should only be changed by the TimeseriesFile that reads this Timeseries (aFile from New)
  Public Property ValuesNeedToBeRead() As Boolean
    Get
      Return pValuesNeedToBeRead
    End Get
    Set(ByVal newValue As Boolean)
      pValuesNeedToBeRead = newValue
    End Set
  End Property

End Class
