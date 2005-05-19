'A value and its definition 
Public Class atcDefinedValue
  Public Definition As atcAttributeDefinition
  Public Value As Object
End Class

'atcAttributeDefinition contains metadata about a value.
'These metadata can be thought of as a more detailed "type" of a value.
'Does not include the value itself, so many values can share the same definition
Public Class atcAttributeDefinition
  Dim pName As String         'Short name (used for labeling in UI)
  Dim pDescription As String  'Something longer than Name but still short
  Dim pHelp As String         'Longer, more detailed than Description
  Dim pEditable As Boolean    'True if the attribute value can be edited by the user
  Dim pDefaultValue As Object 'Of type named by TypeString, or Nothing if not set
  Dim pID As Integer          'Identifier (used for WDM message file index)
  Dim pTypeString As String   'Usually "String", "Integer" or "Double". Default is "String"
  Dim pMin As Double          'Minimum acceptable value (NaN if not set)
  Dim pMax As Double          'Maximum acceptable value (NaN if not set)
  Dim pValidList As ArrayList 'List of acceptable values

  Public Property Name() As String
    Get
      Return pName
    End Get
    Set(ByVal newValue As String)
      pName = newValue
    End Set
  End Property

  Public Property Description() As String
    Get
      Return pDescription
    End Get
    Set(ByVal newValue As String)
      pDescription = newValue
    End Set
  End Property

  Public Property Help() As String
    Get
      Return pHelp
    End Get
    Set(ByVal newValue As String)
      pHelp = newValue
    End Set
  End Property

  Public Property Editable() As Boolean
    Get
      Return pEditable
    End Get
    Set(ByVal newValue As Boolean)
      pEditable = newValue
    End Set
  End Property

  Public Property ID() As Integer
    Get
      Return pID
    End Get
    Set(ByVal newValue As Integer)
      pID = newValue
    End Set
  End Property

  Public Property TypeString() As String
    Get
      Return pTypeString
    End Get
    Set(ByVal newValue As String)
      pTypeString = newValue
    End Set
  End Property

  Public Property ValidList() As ArrayList
    Get
      Return pValidList
    End Get
    Set(ByVal newValue As ArrayList)
      pValidList = newValue
    End Set
  End Property

  Public Property DefaultValue() As Object
    Get
      Return pDefaultValue
    End Get
    Set(ByVal newValue As Object)
      pDefaultValue = newValue
    End Set
  End Property

  Public Property Min() As Double
    Get
      Return pMin
    End Get
    Set(ByVal newValue As Double)
      pMin = newValue
    End Set
  End Property

  Public Property Max() As Double
    Get
      Return pMax
    End Get
    Set(ByVal newValue As Double)
      pMax = newValue
    End Set
  End Property

  Public Sub Clear()
    Name = ""
    Description = ""
    Help = ""
    TypeString = "String"
    DefaultValue = ""
    ID = 0
    Editable = True
    Min = Double.NaN
    Max = Double.NaN
    ValidList = New ArrayList
  End Sub

  Public Sub New()
    Clear()
  End Sub
End Class
