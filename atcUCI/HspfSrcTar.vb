'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfSrcTar
    Implements ICloneable

    Private pOpn As HspfOperation

    Public Group As String
    Public Member As String
    Public MemSub1 As Integer
    Public MemSub2 As Integer
    Public VolId As Integer
    Public VolIdL As Integer
    Public VolName As String

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim lNewHspfSrcTar As New HspfSrcTar
        With lNewHspfSrcTar
            .Group = Me.Group
            .Member = Me.Member
            .MemSub1 = Me.MemSub1
            .MemSub2 = Me.MemSub2
            .VolId = Me.VolId
            .VolIdL = Me.VolIdL
            .VolName = Me.VolName
            .Opn = Me.pOpn
        End With
        Return lNewHspfSrcTar
    End Function

    Public Property Opn() As HspfOperation
        Get
            Return pOpn
        End Get
        Set(ByVal Value As HspfOperation)
            pOpn = Value
            If pOpn IsNot Nothing Then
                VolName = pOpn.Name
                If pOpn.Id < VolId Then
                    VolId = pOpn.Id
                End If
            End If
        End Set
    End Property
End Class