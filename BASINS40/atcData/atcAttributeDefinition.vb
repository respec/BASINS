''' <summary>A value and its definition</summary>
Public Class atcDefinedValue
    Public Definition As atcAttributeDefinition
    Public Value As Object
    Public Arguments As atcDataAttributes 'Arguments used for calculating Value, if any, or Nothing

    Public Function Clone() As atcDefinedValue
        Dim newDefinedValue As New atcDefinedValue
        With newDefinedValue
            .Definition = Me.Definition
            .Value = Me.Value
            .Arguments = Me.Arguments
        End With
        Return newDefinedValue
    End Function
End Class

''' <summary><para>Contains metadata about a value or data set.</para></summary>
''' <remarks>
'''     <para>These metadata can be thought of as a more detailed "type" of a 
''' value.</para>
'''     <para>Does not include the value itself, so many values can share the same
'''     definition</para>
''' </remarks>
Public Class atcAttributeDefinition
    Dim pName As String         'Short name (used for labeling in UI)
    Dim pDescription As String  'Something longer than Name but still short
    Dim pCategory As String     'Optional, used for grouping similar attributes in UI
    Dim pCopiesInherit As Boolean 'True if attribute should be copied when parent object is copied 
    Dim pHelp As String         'Longer, more detailed than Description
    Dim pEditable As Boolean    'True if the attribute value can be edited by the user
    Dim pDefaultValue As Object 'Of type named by TypeString, or Nothing if not set
    Dim pID As Integer          'Identifier (used for WDM message file index)
    Dim pTypeString As String   'Usually "String", "Integer" or "Double". Default is "String"
    Dim pMin As Double          'Minimum acceptable value (NaN if not set)
    Dim pMax As Double          'Maximum acceptable value (NaN if not set)
    Dim pValidList As ArrayList 'List of acceptable values

    Dim pCalculator As atcDataSource 'The source responsible for calculating this attribute or Nothing

    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal newValue As String)
            pName = newValue
        End Set
    End Property

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

    Public ReadOnly Property Calculated() As Boolean
        Get
            If pCalculator Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public Property Calculator() As atcDataSource
        Get
            Return pCalculator
        End Get
        Set(ByVal newValue As atcDataSource)
            pCalculator = newValue
        End Set
    End Property

    Public Property Category() As String
        Get
            Return pCategory
        End Get
        Set(ByVal newValue As String)
            pCategory = newValue
        End Set
    End Property

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
            If Not Me.ValidList Is Nothing Then
                .ValidList = Me.ValidList.Clone
            End If
        End With
        Return myClone
    End Function

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
        Category = ""
        pCopiesInherit = True
        Help = ""
        TypeString = "String"
        DefaultValue = Nothing
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
