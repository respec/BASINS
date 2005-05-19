Option Strict Off
Option Explicit On 

Imports System.Collections.Specialized

Public Class ProjectionCollection
    Inherits NameObjectCollectionBase

    ' Gets or sets the value associated with the specified key.
    Default Public Property Item(ByVal key As [String]) As Projection
        Get
            Return Me.BaseGet(key)
        End Get
        Set(ByVal Value As Projection)
            Me.BaseSet(key, Value)
        End Set
    End Property

    ' Adds an entry to the collection.
    Public Sub Add(ByVal key As [String], ByVal value As Projection)
        Me.BaseAdd(key, value)
    End Sub 'Add

    Public Shadows Function GetEnumerator() As IEnumerator
        Return Me.BaseGetAllValues.GetEnumerator
    End Function

End Class 'ProjectionCollection

Public Class Projection

    Dim pClass As String
    Dim pCategory As String
    Dim pName As String
    Dim pZone As String
    Dim pEllipsoid As String
    Dim pD(6) As String 'pD(0) not used

    Dim pDefaults As Projection
    Dim pFieldnames As Projection

    Public Property Defaults() As Projection
        Get
            Defaults = pDefaults
        End Get
        Set(ByVal Value As Projection)
            pDefaults = Value
        End Set
    End Property

    Public Property Fieldnames() As Projection
        Get
            Fieldnames = pFieldnames
        End Get
        Set(ByVal Value As Projection)
            pFieldnames = Value
        End Set
    End Property

    Public Property ProjectionClass() As String
        Get
            ProjectionClass = pClass
        End Get
        Set(ByVal Value As String)
            pClass = Value
        End Set
    End Property

    Public Property Category() As String
        Get
            Category = pCategory
        End Get
        Set(ByVal Value As String)
            pCategory = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Name = pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public Property Zone() As String
        Get
            Zone = pZone
        End Get
        Set(ByVal Value As String)
            pZone = Value
        End Set
    End Property

    Public Property Ellipsoid() As String
        Get
            Ellipsoid = pEllipsoid
        End Get
        Set(ByVal Value As String)
            pEllipsoid = Value
        End Set
    End Property

    Public Property d(ByVal Index As Integer) As String
        Get
            If Index > 0 And Index <= 6 Then d = pD(Index)
        End Get
        Set(ByVal Value As String)
            If Index > 0 And Index <= 6 Then pD(Index) = Value
        End Set
    End Property

    Public Overrides Function toString() As String
        'TODO: make this return the string formatted for proj
        Return pName & ":" & pClass & ":" & pEllipsoid & ":" & pCategory & ":" & pZone _
                     & ":" & pD(1) & ":" & pD(2) & ":" & pD(3) & ":" & pD(4) & ":" & pD(5) & ":" & pD(6)
    End Function

    'Public Property xml() As CHILKATXMLLib.ChilkatXml
    '    Get
    '        xml = New CHILKATXMLLib.ChilkatXml
    '        With xml
    '            .Tag = "Projection"
    '            .Content = pName
    '            If Len(pClass) > 0 Then .AddAttribute("Class", pClass)
    '            If Len(pEllipsoid) > 0 Then .AddAttribute("Ellipsoid", pEllipsoid)
    '            If Len(pCategory) > 0 Then .AddAttribute("Category", pCategory)
    '            If Len(pZone) > 0 Then .AddAttribute("Zone", pZone)
    '            If Len(pD(1)) > 0 Then .AddAttribute("Parm1", pD(1))
    '            If Len(pD(2)) > 0 Then .AddAttribute("Parm2", pD(2))
    '            If Len(pD(3)) > 0 Then .AddAttribute("Parm3", pD(3))
    '            If Len(pD(4)) > 0 Then .AddAttribute("Parm4", pD(4))
    '            If Len(pD(5)) > 0 Then .AddAttribute("Parm5", pD(5))
    '            If Len(pD(6)) > 0 Then .AddAttribute("Parm6", pD(6))
    '        End With
    '    End Get
    '    Set(ByVal Value As CHILKATXMLLib.ChilkatXml)
    '        With Value
    '            pClass = .GetAttrValue("Class")
    '            pCategory = .GetAttrValue("Category")
    '            pZone = .GetAttrValue("Zone")
    '            pEllipsoid = .GetAttrValue("Ellipsoid")
    '            pName = .Content
    '            pD(1) = .GetAttrValue("Parm1")
    '            pD(2) = .GetAttrValue("Parm2")
    '            pD(3) = .GetAttrValue("Parm3")
    '            pD(4) = .GetAttrValue("Parm4")
    '            pD(5) = .GetAttrValue("Parm5")
    '            pD(6) = .GetAttrValue("Parm6")
    '        End With
    '    End Set
    'End Property
End Class