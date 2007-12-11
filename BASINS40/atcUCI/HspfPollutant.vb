'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfPollutant
    Private pTables As Collection
    Private pMassLinks As Collection
    Private pOperations As Collection

    Public Name As String
    Public Id As Integer
    Public Index As Integer
    Public ModelType As String
    Public ReadOnly Property Tables() As Collection 'of HspfTable
        Get
            Return pTables
        End Get
    End Property

    Public Property MassLinks() As Collection 'of HspfMasslink
        Get
            Return pMassLinks
        End Get
        Set(ByVal Value As Collection) 'of HspfMassLinks
            For Each lMassLink As HspfMassLink In Value
                pMassLinks.Add(lMassLink)
            Next lMassLink
        End Set
    End Property

    Public Property Operations() As Collection 'of HspfOperation
        Get
            Return pOperations
        End Get
        Set(ByVal Value As Collection) 'of HspfOperation
            For Each lOperation As HspfOperation In Value
                pOperations.Add(lOperation)
            Next lOperation
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        pTables = New Collection
        pOperations = New Collection
        pMassLinks = New Collection
        Id = 0
        Name = ""
        ModelType = ""
        Index = 0
    End Sub

    Public Function TableExists(ByRef aTableName As String) As Boolean
        Return pTables.Contains(aTableName)
    End Function
End Class