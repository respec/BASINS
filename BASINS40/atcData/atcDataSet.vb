Public Class atcDataSet

  Private Shared pNextSerial As Integer = 0 'Next serial number to be assigned
  Private Shared pStringFormat As String = "{0} {1} {2} # {3}"
  Private Shared pStringAttributeNames() As String = {"Scenario", "Location", "Constituent", "id"}

  Private pSerial As Integer 'Serial number of this object

  Private pAttributes As atcDataAttributes

  'Attributes associated with the whole Data (location, constituent, etc.)
  Public ReadOnly Property Attributes() As atcDataAttributes
    Get
      Return pAttributes
    End Get
  End Property

  Public Overridable Sub Clear()
    pAttributes = New atcDataAttributes
    pAttributes.Owner = Me
  End Sub

  Public Sub New()
    pSerial = System.Threading.Interlocked.Increment(pNextSerial) 'Safely increment pNextSerial
    Clear()
  End Sub

  'Unique serial number assigned when object is created
  Public ReadOnly Property Serial() As Integer
    Get
      Return pSerial
    End Get
  End Property

  Public Overrides Function ToString() As String
    Dim lLastAttribute As Integer = pStringAttributeNames.GetUpperBound(0)
    Dim lAttrValues(lLastAttribute) As String
    For iArg As Integer = 0 To lLastAttribute
      lAttrValues(iArg) = pAttributes.GetFormattedValue(pStringAttributeNames(iArg), "-")
    Next
    Return String.Format(pStringFormat, lAttrValues)
  End Function

  Public Shared Sub SetStringFormat(ByVal aAttributeNames() As String, Optional ByVal aFormat As String = "")
    If aFormat.Length = 0 Then 'Build a default format string with all arguments separated by spaces
      For iArg As Integer = 0 To aAttributeNames.GetUpperBound(0)
        aFormat &= "{" & iArg & "} "
      Next
      aFormat = RTrim(aFormat) 'remove trailing space
    End If
    pStringFormat = aFormat
    pStringAttributeNames = aAttributeNames
  End Sub

End Class
