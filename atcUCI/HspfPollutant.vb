Option Strict Off
Option Explicit On

<System.Runtime.InteropServices.ProgId("HspfPollutant_NET.HspfPollutant")> _
Public Class HspfPollutant
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pName As String
    Private pId As Integer
    Private pIndex As Integer
    Private pModelType As String
    Private pTables As Collection
    Private pMassLinks As Collection
    Private pOperations As Collection

    Public Property Name() As String
        Get
            Name = pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Id = pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property Index() As Integer
        Get
            Index = pIndex
        End Get
        Set(ByVal Value As Integer)
            pIndex = Value
        End Set
    End Property

    Public Property ModelType() As String
        Get
            ModelType = pModelType
        End Get
        Set(ByVal Value As String)
            pModelType = Value
        End Set
    End Property

    Public ReadOnly Property Tables() As Collection 'of HspfTable
        Get
            Tables = pTables
        End Get
    End Property

    Public Property MassLinks() As Collection 'of HspfMasslink
        Get
            MassLinks = pMassLinks
        End Get
        Set(ByVal Value As Collection) 'of HspfMassLinks
            Dim lMassLink As HspfMassLink
            Dim vMassLink As Object
            For Each vMassLink In Value
                lMassLink = vMassLink
                pMassLinks.Add(lMassLink)
            Next vMassLink
        End Set
    End Property

    Public Property Operations() As Collection 'of HspfOperation
        Get
            Operations = pOperations
        End Get
        Set(ByVal Value As Collection) 'of HspfMassLinks
            Dim lOperation As HspfOperation
            Dim vOperation As Object
            For Each vOperation In Value
                lOperation = vOperation
                pOperations.Add(lOperation)
            Next vOperation
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        pTables = New Collection
        pOperations = New Collection
        pMassLinks = New Collection
        pId = 0
        pName = ""
        pModelType = ""
        pIndex = 0
    End Sub

    Public Function TableExists(ByRef Name As String) As Boolean
        Return pTables.Contains(Name)
    End Function
End Class