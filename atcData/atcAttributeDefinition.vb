Imports atcUtility

''' <summary>A value and its definition</summary>
Public Class atcDefinedValue
    Public Definition As atcAttributeDefinition
    Public Value As Object
    Public Arguments As atcDataAttributes 'Arguments used for calculating Value, if any, or Nothing

    Public Sub New()
    End Sub

    Public Sub New(ByVal aDefinition As atcAttributeDefinition, ByVal aValue As Object, Optional ByVal aArguments As atcDataAttributes = Nothing)
        Definition = aDefinition
        Value = aValue
        Arguments = aArguments
    End Sub

    Public Function Clone() As atcDefinedValue
        Dim newDefinedValue As New atcDefinedValue
        With newDefinedValue
            .Definition = Me.Definition
            .Value = Me.Value
            .Arguments = Me.Arguments
        End With
        Return newDefinedValue
    End Function

    Public Overrides Function ToString() As String
        On Error Resume Next
        Dim lString As String = ""
        If Definition Is Nothing Then
            lString &= "No Definition"
        Else
            lString &= Definition.ToString
        End If
        lString &= " Value = "
        If Value Is Nothing Then
            lString &= "Nothing"
        Else
            lString &= Value.ToString
        End If
        If Arguments IsNot Nothing AndAlso Arguments.Count > 0 Then
            lString &= " " & Arguments.Count & " Arguments: "
            For Each lValidItem As atcDefinedValue In Arguments
                lString &= lValidItem.ToString & " "
            Next
        End If
        Return lString
    End Function
End Class

''' <summary><para>Detailed type information about an attribute value</para></summary>
''' <remarks>Does not include the value itself, so different objects 
'''          can have values which share the same definition
''' </remarks>
Public Class atcAttributeDefinition

    Private Shared pNaN As Double = GetNaN()

    Public Units As String
    Dim pName As String         'Short name (used for labeling in UI)
    Dim pDescription As String  'Something longer than Name but still short
    Dim pCategory As String     'Optional, used for grouping similar attributes in UI
    Dim pCopiesInherit As Boolean 'True if attribute should be copied when parent object is copied 
    Dim pHelp As String         'Longer, more detailed than Description
    Dim pEditable As Boolean    'True if the attribute value can be edited by the user
    Dim pDefaultValue As Object 'Of type named by TypeString, or Nothing if not set
    Dim pID As Integer          'Identifier - in WDM, this is set to message file index
    Dim pTypeString As String   'Usually "String", "Integer" or "Double". Default is "String"
    Dim pMin As Double          'Minimum acceptable value (NaN if not set)
    Dim pMax As Double          'Maximum acceptable value (NaN if not set)
    Dim pValidList As ArrayList 'List of acceptable values

    Dim pCalculator As atcTimeseriesSource 'The source responsible for calculating this attribute or Nothing

    ''' <summary>
    ''' Short name, used for labeling in UI
    ''' </summary>
    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal newValue As String)
            pName = newValue
        End Set
    End Property

    ''' <summary>
    ''' True if attribute should be copied when parent object is copied, false to skip when copying parent object
    ''' </summary>
    Public Property CopiesInherit() As Boolean
        Get
            If Calculated Then
                Return False
            Else
                Return pCopiesInherit
            End If
        End Get
        Set(ByVal newValue As Boolean)
            pCopiesInherit = newValue
        End Set
    End Property

    ''' <summary>
    ''' True if this attribute has a Calculator, False if this attribute does not have a Calculator
    ''' </summary>
    Public ReadOnly Property Calculated() As Boolean
        Get
            If pCalculator Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    ''' <summary>
    ''' The source responsible for calculating this attribute, Nothing if this attribute is not calculated
    ''' </summary>
    Public Property Calculator() As atcTimeseriesSource
        Get
            Return pCalculator
        End Get
        Set(ByVal newValue As atcTimeseriesSource)
            pCalculator = newValue
        End Set
    End Property

    ''' <summary>
    ''' Optional, used for grouping similar attributes in UI
    ''' </summary>
    Public Property Category() As String
        Get
            Return pCategory
        End Get
        Set(ByVal newValue As String)
            pCategory = newValue
        End Set
    End Property

    ''' <summary>
    ''' Create a new copy of this definition
    ''' </summary>
    ''' <param name="aNewName">Optional name for the copy</param>
    ''' <param name="aNewDescription">Optional description of the copy</param>
    Public Function Clone(Optional ByVal aNewName As String = Nothing, _
                          Optional ByVal aNewDescription As String = Nothing) As atcAttributeDefinition
        Dim myClone As New atcAttributeDefinition
        With myClone
            If aNewName Is Nothing Then
                .Name = Me.Name
            Else
                .Name = aNewName
            End If
            If aNewDescription Is Nothing Then
                .Description = Me.Description
            Else
                .Description = aNewDescription
            End If
            .Category = Me.Category
            .DefaultValue = Me.DefaultValue
            .Editable = Me.Editable
            .Help = Me.Help
            .ID = Me.ID
            .Max = Me.Max
            .Min = Me.Min
            .TypeString = Me.TypeString
            .Calculator = Me.Calculator
            .Units = Me.Units
            If Not Me.ValidList Is Nothing Then
                .ValidList = Me.ValidList.Clone
            End If
        End With
        Return myClone
    End Function

    ''' <summary>
    ''' Longer than Name but still short
    ''' </summary>
    Public Property Description() As String
        Get
            Return pDescription
        End Get
        Set(ByVal newValue As String)
            pDescription = newValue
        End Set
    End Property

    ''' <summary>
    ''' More detailed version of Description
    ''' </summary>
    Public Property Help() As String
        Get
            Return pHelp
        End Get
        Set(ByVal newValue As String)
            pHelp = newValue
        End Set
    End Property

    ''' <summary>
    ''' True if the attribute value can be edited by the user, False if user should not be allowed to edit
    ''' </summary>
    Public Property Editable() As Boolean
        Get
            Return pEditable
        End Get
        Set(ByVal newValue As Boolean)
            pEditable = newValue
        End Set
    End Property

    ''' <summary>
    ''' Integer Identifier - in WDM, this is set to message file index
    ''' </summary>
    Public Property ID() As Integer
        Get
            Return pID
        End Get
        Set(ByVal newValue As Integer)
            pID = newValue
        End Set
    End Property

    ''' <summary>
    ''' True if TypeString is Integer, Single or Double, False otherwise
    ''' </summary>
    Public Function IsNumeric() As Boolean
        If Not TypeString Is Nothing Then
            Select Case TypeString.ToLower
                Case "integer", "single", "double" : Return True
            End Select
        End If
        Return False
    End Function

    ''' <summary>
    ''' Usually "String", "Integer" or "Double". Default is "String"
    ''' </summary>
    Public Property TypeString() As String
        Get
            Return pTypeString
        End Get
        Set(ByVal newValue As String)
            pTypeString = newValue
        End Set
    End Property

    ''' <summary>
    ''' List of acceptable values for this attribute
    ''' </summary>
    Public Property ValidList() As ArrayList
        Get
            Return pValidList
        End Get
        Set(ByVal newValue As ArrayList)
            pValidList = newValue
        End Set
    End Property

    ''' <summary>
    ''' Value to use if this attribute does not have a value set, Nothing is the default
    ''' </summary>
    Public Property DefaultValue() As Object
        Get
            Return pDefaultValue
        End Get
        Set(ByVal newValue As Object)
            pDefaultValue = newValue
        End Set
    End Property

    ''' <summary>
    ''' Minimum acceptable numeric value for this attribute (NaN if not set)
    ''' </summary>
    Public Property Min() As Double
        Get
            Return pMin
        End Get
        Set(ByVal newValue As Double)
            pMin = newValue
        End Set
    End Property

    ''' <summary>
    ''' Maximum acceptable numeric value for this attribute (NaN if not set)
    ''' </summary>
    Public Property Max() As Double
        Get
            Return pMax
        End Get
        Set(ByVal newValue As Double)
            pMax = newValue
        End Set
    End Property

    Public Overrides Function ToString() As String
        On Error Resume Next
        Dim lString As String = ""
        lString &= Name
        lString &= " (" & TypeString & ") '"
        lString &= Description & "'"
        lString &= " Category: " & Category
        lString &= " CopiesInherit: " & pCopiesInherit
        lString &= " Help: " & Help
        If DefaultValue IsNot Nothing Then lString &= " DefaultValue: " & DefaultValue.ToString
        lString &= " ID: " & ID
        lString &= " Editable: " & Editable
        lString &= " Min: " & Min
        lString &= " Max: " & Max
        If ValidList IsNot Nothing AndAlso ValidList.Count > 0 Then
            lString &= " ValidList: "
            For Each lValidItem As Object In ValidList
                lString &= lValidItem.ToString & " "
            Next
        End If
        Return lString
    End Function

    Public Sub Clear()
        Name = ""
        Description = ""
        Category = ""
        pCopiesInherit = True
        Help = ""
        TypeString = "String"
        DefaultValue = Nothing
        ID = 0
        Editable = True
        Min = pNaN
        Max = pNaN
        ValidList = New ArrayList
    End Sub

    Public Sub New()
        Clear()
    End Sub
End Class
