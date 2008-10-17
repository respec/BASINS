'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfPollutant
    Private pTables As Generic.Dictionary(Of String, HspfTable)
    Private pMassLinks As Generic.List(Of HspfMassLink)
    Private pOperations As Generic.Dictionary(Of String, HspfOperation)

    Public Name As String
    Public Id As Integer
    Public Index As Integer
    Public ModelType As String

    Public ReadOnly Property Tables() As Generic.Dictionary(Of String, HspfTable)
        Get
            Return pTables
        End Get
    End Property

    Public Property MassLinks() As Generic.List(Of HspfMassLink)
        Get
            Return pMassLinks
        End Get
        Set(ByVal aValues As Generic.List(Of HspfMassLink))
            For Each lMassLink As HspfMassLink In aValues
                pMassLinks.Add(lMassLink)
            Next lMassLink
        End Set
    End Property

    Public Property Operations() As Generic.Dictionary(Of String, HspfOperation)
        Get
            Return pOperations
        End Get
        Set(ByVal aValues As Generic.Dictionary(Of String, HspfOperation))
            For Each lValue As Generic.KeyValuePair(Of String, HspfOperation) In aValues
                pOperations.Add(lValue.Key, lValue.Value)
            Next
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        pTables = New Generic.Dictionary(Of String, HspfTable)
        pOperations = New Generic.Dictionary(Of String, HspfOperation)
        pMassLinks = New Generic.List(Of HspfMassLink)
        Id = 0
        Name = ""
        ModelType = ""
        Index = 0
    End Sub

    Public Function TableExists(ByRef aTableName As String) As Boolean
        Return pTables.ContainsKey(aTableName)
    End Function
End Class