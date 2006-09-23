Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HSPFParm_NET.HSPFParm")> Public Class HSPFParm
    '##MODULE_SUMMARY Class containing a model parameter value.
    '##MODULE_REMARKS Copyright 2001-3AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pValue As String
    Private pValueAsRead As String
    Private pDef As HSPFParmDef
    Private pParent As Object = Nothing

    '##SUMMARY <P>Value of parameter.
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

    '##SUMMARY <P>Value of parameter as read from UCI
    Public Property ValueAsRead() As String
        Get
            ValueAsRead = pValueAsRead
        End Get
        Set(ByVal Value As String)
            pValueAsRead = Value
        End Set
    End Property

    '##SUMMARY Link to object containing definition of parameter.
    Public Property Def() As HSPFParmDef
        Get
            Def = pDef
        End Get
        Set(ByVal Value As HSPFParmDef)
            pDef = Value
        End Set
    End Property

    '##SUMMARY Link to object that is the parent of this parameter.
    Public Property Parent() As Object
        Get
            Parent = pParent
        End Get
        Set(ByVal Value As Object)
            pParent = Value
        End Set
    End Property

    '##SUMMARY Name of parameter.
    Public ReadOnly Property Name() As String
        Get
            Name = pDef.Name
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub
End Class