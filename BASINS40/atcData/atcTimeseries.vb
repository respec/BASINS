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

  Public Shared Function Merge(ByVal aGroup As atcDataGroup) As atcTimeseries
    Dim lNewTS As New atcTimeseries(Nothing)
    Dim lNewIndex As Integer
    Dim lTotalNumValues As Integer = 0
    Dim lOldTS As atcTimeseries
    Dim lMinDate As Double = Double.MaxValue
    Dim lMaxGroupIndex As Integer = aGroup.Count - 1
    Dim lIndex As Integer
    Dim lMinIndex As Integer
    Dim lNextIndex() As Integer
    Dim lNextDate() As Double

    ReDim lNextIndex(aGroup.Count - 1)
    ReDim lNextDate(aGroup.Count - 1)

    lNewTS.Dates = New atcTimeseries(Nothing)

    'Count total number of values and set up 
    For lIndex = 0 To lMaxGroupIndex
      lOldTS = aGroup.ItemByIndex(lIndex)
      Try
        lTotalNumValues += lOldTS.numValues
        lNextIndex(lIndex) = 1
        lNextDate(lIndex) = lOldTS.Dates.Value(1)
        'Find minimum starting date and take date 0 from that TS
        If lNextDate(lIndex) < lMinDate Then
          lMinDate = lOldTS.Dates.Value(0)
        End If
      Catch 'Can't get dates values from this TS
        lNextIndex(lIndex) = -1
      End Try
    Next

    If lTotalNumValues > 0 Then
      lNewTS.numValues = lTotalNumValues
      lNewTS.Dates.numValues = lTotalNumValues
      If lMinDate < Double.MaxValue Then
        lNewTS.Dates.Value(0) = lMinDate
      Else
        lNewTS.Dates.Value(0) = Double.NaN
      End If
      lNewTS.Value(0) = Double.NaN

      For lNewIndex = 1 To lTotalNumValues
        'Find earliest date not yet used
        lMinIndex = -1
        lMinDate = Double.MaxValue
        For lIndex = 0 To lMaxGroupIndex
          If lNextIndex(lIndex) > 0 Then
            If lNextDate(lIndex) < lMinDate Then
              lMinIndex = lIndex
              lMinDate = lNextDate(lIndex)
            End If
          End If
        Next

        'Add earliest date/value to new TS
        If lMinIndex >= 0 Then
          lOldTS = aGroup.ItemByIndex(lMinIndex)
          lNewTS.Dates.Value(lNewIndex) = lMinDate
          lNewTS.Value(lNewIndex) = lOldTS.Value(lNextIndex(lMinIndex))
          lNextIndex(lMinIndex) += 1
          If lNextIndex(lMinIndex) <= lOldTS.numValues Then
            lNextDate(lIndex) = lOldTS.Dates.Value(lNextIndex(lMinIndex))
          Else
            lNextDate(lIndex) = -1
          End If
        Else
          'Should never get here 
          If lNewTS.numValues >= lNewIndex Then
            atcUtility.LogMsg("Ran out of values after finding " & lNewIndex - 1 & " but expected " & lTotalNumValues, "Timeseries Merge")
            lNewTS.numValues = lNewIndex - 1
            Exit For
          End If
        End If
      Next
    End If
      Return lNewTS
  End Function

  Public Overrides Function ToString() As String
    Dim id As String = Attributes.GetValue("id")
    'If id.Length = 0 Then id = "# " & CStr(pSerial)

    Return Attributes.GetValue("Scenario") & " " _
         & Attributes.GetValue("Location") & " " _
         & Attributes.GetValue("Constituent") & " " & id & " # " & CStr(Serial)
  End Function

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

End Class
