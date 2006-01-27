''' <summary>Base class for data.</summary>
Public Class atcDataSet

  Private Shared pNextSerial As Integer = 0 'Next serial number to be assigned
  Private Shared pStringFormat As String = "{0} {1} {2} # {3}"
  Private Shared pStringAttributeNames() As String = {"Scenario", "Location", "Constituent", "id"}

  Private pSerial As Integer 'Serial number of this object

  Private pAttributes As atcDataAttributes

  ''' <summary>Attributes associated with the whole data set(location, constituent, 
  ''' etc.)</summary>
  Public ReadOnly Property Attributes() As atcDataAttributes
    Get
      Return pAttributes
    End Get
  End Property

  ''' <summary>Reset data attributes to an empty collection</summary>
  Public Overridable Sub Clear()
    pAttributes = New atcDataAttributes
    pAttributes.Owner = Me
  End Sub

  ''' <summary>Create a new data set with identical attributes to current one</summary>
  Public Overridable Function Clone() As atcDataSet
    Dim lClone As New atcDataSet
    lClone.Attributes.ChangeTo(pAttributes)
    Return lClone
  End Function

  ''' <summary>Create a new data set</summary>
  Public Sub New()
    pSerial = System.Threading.Interlocked.Increment(pNextSerial)
    Clear()
  End Sub

  ''' <summary>Unique serial number assigned when data object is created</summary>
  Public ReadOnly Property Serial() As Integer
    Get
      Return pSerial
    End Get
  End Property

  ''' <summary>Formated string containing the attributes of data set in pStringAttributeNames.</summary>
  Public Overrides Function ToString() As String
    Dim lLastAttribute As Integer = pStringAttributeNames.GetUpperBound(0)
    Dim lAttrValues(lLastAttribute) As String
    For iArg As Integer = 0 To lLastAttribute
      lAttrValues(iArg) = pAttributes.GetFormattedValue(pStringAttributeNames(iArg), "-")
    Next
    Return String.Format(pStringFormat, lAttrValues)
  End Function

  ''' <summary>Build a default format string with all arguments separated by 
  ''' spaces</summary>
  Public Shared Sub SetStringFormat(ByVal aAttributeNames() As String, _
                           Optional ByVal aFormat As String = "")

    If aFormat.Length = 0 Then 'Create a format containing all the named attributes
      For iArg As Integer = 0 To aAttributeNames.GetUpperBound(0)
        aFormat &= "{" & iArg & "} "
      Next
      aFormat = RTrim(aFormat) 'remove trailing space
    End If
    pStringFormat = aFormat
    pStringAttributeNames = aAttributeNames
  End Sub

End Class
