Option Strict Off
Option Explicit On
''' <summary>
''' A model parameter value.
''' </summary>
''' <remarks>Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license</remarks>
Public Class HspfParm
    Private pValue As String
    Private pValueAsRead As String
    Private pDef As HSPFParmDef
    Private pParent As Object = Nothing

    ''' <summary>
    ''' Value of parameter.
    ''' </summary>
    Public Property Value() As String
        Get
            Value = pValue
        End Get
        Set(ByVal Value As String)
            pValue = Value
            If Not pParent Is Nothing Then
                pParent.Edited = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' Value of parameter as read from UCI
    ''' </summary>
    Public Property ValueAsRead() As String
        Get
            ValueAsRead = pValueAsRead
        End Get
        Set(ByVal Value As String)
            pValueAsRead = Value
        End Set
    End Property

    ''' <summary>
    ''' Link to object containing definition of parameter.
    ''' </summary>
    Public Property Def() As HSPFParmDef
        Get
            Def = pDef
        End Get
        Set(ByVal Value As HSPFParmDef)
            pDef = Value
        End Set
    End Property

    ''' <summary>
    ''' Link to object that is the parent of this parameter.
    ''' </summary>
    Public Property Parent() As Object
        Get
            Parent = pParent
        End Get
        Set(ByVal Value As Object)
            pParent = Value
        End Set
    End Property

    ''' <summary>
    ''' Name of parameter.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Name = pDef.Name
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub
End Class