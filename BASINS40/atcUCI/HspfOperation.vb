'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfOperations
    Inherits KeyedCollection(Of String, HspfOperation)
    Protected Overrides Function GetKeyForItem(ByVal aHspfOperation As HspfOperation) As String
        Return "K" & aHspfOperation.Id
    End Function
End Class

Public Class HspfOperation
    Private Enum LegendType
        LegLand = 0
        LegMet = 1
        LegPoint = 2
    End Enum

    Private pDescription As String
    Private pEdited As Boolean
    Private pSerial As Integer

    Public Uci As HspfUci
    Public OpTyp As HspfData.HspfOperType
    Public OpnBlk As HspfOpnBlk
    Public FTable As HspfFtable
    Public MetSeg As HspfMetSeg
    Public PointSources As Collection(Of HspfPointSource)
    Public Comment As String = ""
    Public Id As Integer
    Public Tag As String = ""
    Public DefOpnId As Integer
    Public Const EditControlName As String = "ATCoHspf.ctlOperationEdit"
    Public ReadOnly TableStatus As HspfStatus
    Public ReadOnly InputTimeseriesStatus As HspfStatus
    Public ReadOnly OutputTimeseriesStatus As HspfStatus
    Public ReadOnly Tables As HspfTables
    Public ReadOnly Sources As Collection(Of HspfConnection)
    Public ReadOnly Targets As Collection(Of HspfConnection)

    Public ReadOnly Property Caption() As String
        Get
            Return "Operation:  " & HspfOperName(OpTyp) & " " & Id & " - " & pDescription
        End Get
    End Property

    Public Property Edited() As Boolean
        Get
            Return pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value AndAlso OpnBlk IsNot Nothing Then
                OpnBlk.Edited = True
            End If
        End Set
    End Property

    Public Property Name() As String
        Get
            Return HspfOperName(OpTyp)
        End Get
        Set(ByVal Value As String)
            OpTyp = HspfOperNum(Value)
        End Set
    End Property

    Public ReadOnly Property Serial() As Integer
        Get
            Return pSerial
        End Get
    End Property

    Public Property Description() As String
        Get
            Return pDescription
        End Get
        Set(ByVal aValue As String)
            pDescription = aValue
            Dim lColonPos As Integer = pDescription.IndexOf(":")
            If lColonPos > -1 Then
                pDescription = Mid(pDescription, lColonPos + 1)
            End If
        End Set
    End Property

    Public Sub Edit()
        'status or hourglass needed here
        editInit(Me, (Uci.Icon), True, True, False)
    End Sub

    Public Function TableExists(ByRef aName As String) As Boolean
        Return Tables.Contains(aName)
    End Function

    Public Sub SetTimSerConnections()
        Dim lOperName As String = HspfOperName(OpTyp)
        For Each lConnection As HspfConnection In Uci.Connections
            With lConnection.Target
                If .VolName = lOperName Then
                    If .VolId = Id Or (.VolId < Id And .VolIdL >= Id) Then
                        lConnection.Target.Opn = Me
                        Sources.Add(lConnection)
                    End If
                End If
            End With
            With lConnection.Source
                If .VolName = lOperName Then
                    If .VolId = Id Or (.VolId < Id And .VolIdL >= Id) Then
                        lConnection.Source.Opn = Me
                        Targets.Add(lConnection)
                    End If
                End If
            End With
        Next lConnection
    End Sub

    Public Sub SetTimSerConnectionsSources()
        Dim lOperName As String = HspfOperName(OpTyp)
        For Each lConnection As HspfConnection In Uci.Connections
            With lConnection.Target
                If .VolName = lOperName Then
                    If .VolId = Id Or (.VolId < Id And .VolIdL >= Id) Then
                        lConnection.Target.Opn = Me
                        Sources.Add(lConnection)
                    End If
                End If
            End With
        Next lConnection
    End Sub

    Public Sub SetTimSerConnectionsTargets()
        Dim lOperName As String = HspfOperName(OpTyp)
        For Each lConnection As HspfConnection In Uci.Connections
            With lConnection.Source
                If .VolName = lOperName Then
                    If .VolId = Id Or (.VolId < Id And .VolIdL >= Id) Then
                        lConnection.Source.Opn = Me
                        Targets.Add(lConnection)
                    End If
                End If
            End With
        Next lConnection
    End Sub

    Public Function DownOper(ByRef aOpType As String) As Integer
        For Each lConnection As HspfConnection In Targets
            If aOpType.Length = 0 Then 'take first one of any type
                Return lConnection.Target.VolId
            ElseIf lConnection.Target.VolName = aOpType Then  'first of selected type
                Return lConnection.Target.VolId
            End If
        Next lConnection
    End Function

    'Returns percent (0..1) given a source.VolId and value
    'Private Function IdPercentRange(id As Long, Value As Single) As Single
    '  If Value < 0 Then Value = -Value
    '  If Value < 1 Then
    '    IdPercentRange = Value
    '  ElseIf Value < 10 Then
    '    IdPercentRange = Value / 10
    '  ElseIf Value < 100 Then
    '    IdPercentRange = Value / 100
    '  ElseIf Value < 1000 Then
    '    IdPercentRange = Value / 1000
    '  ElseIf Value < 10000 Then
    '    IdPercentRange = Value / 10000
    '  ElseIf Value < 100000 Then
    '    IdPercentRange = Value / 100000
    '  ElseIf Value < 1000000 Then
    '    IdPercentRange = Value / 1000000
    '  ElseIf Value < 10000000 Then
    '    IdPercentRange = Value / 10000000
    '  End If
    'End Function

    Public Sub New()
        MyBase.New()
        'Debug.Print "init HspfOperation"
        Tables = New HspfTables
        Sources = New Collection(Of HspfConnection)
        Targets = New Collection(Of HspfConnection)
        PointSources = New Collection(Of HspfPointSource)
        TableStatus = New HspfStatus
        TableStatus.Init(Me)
        InputTimeseriesStatus = New HspfStatus
        InputTimeseriesStatus.StatusType = HspfStatus.HspfStatusTypes.HspfInputTimeseries
        InputTimeseriesStatus.Init(Me)
        OutputTimeseriesStatus = New HspfStatus
        OutputTimeseriesStatus.StatusType = HspfStatus.HspfStatusTypes.HspfOutputTimeseries
        OutputTimeseriesStatus.Init(Me)
        Id = 0
        OpTyp = 0
        pDescription = ""
        pLastOperationSerial += 1
        pSerial = pLastOperationSerial
        DefOpnId = 0
    End Sub
End Class