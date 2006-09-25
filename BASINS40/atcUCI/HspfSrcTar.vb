Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfSrcTar_NET.HspfSrcTar")> _
Public Class HspfSrcTar
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pGroup As String
    Private pOpn As HspfOperation
    Private pMember As String
    Private pMemSub1 As Integer
    Private pMemSub2 As Integer
    Private pVolId As Integer
    Private pVolIdL As Integer
    Private pVolName As String

    Public Property Group() As String
        Get
            Group = pGroup
        End Get
        Set(ByVal Value As String)
            pGroup = Value
        End Set
    End Property

    Public Property Opn() As HspfOperation
        Get
            Opn = pOpn
        End Get
        Set(ByVal Value As HspfOperation)
            pOpn = Value
            pVolName = pOpn.Name
            If pOpn.Id < pVolId Then
                pVolId = pOpn.Id
            End If
        End Set
    End Property

    Public Property Member() As String
        Get
            Member = pMember
        End Get
        Set(ByVal Value As String)
            pMember = Value
        End Set
    End Property

    Public Property MemSub1() As Integer
        Get
            MemSub1 = pMemSub1
        End Get
        Set(ByVal Value As Integer)
            pMemSub1 = Value
        End Set
    End Property

    Public Property MemSub2() As Integer
        Get
            MemSub2 = pMemSub2
        End Get
        Set(ByVal Value As Integer)
            pMemSub2 = Value
        End Set
    End Property

    Public Property VolId() As Integer
        Get
            VolId = pVolId
        End Get
        Set(ByVal Value As Integer)
            pVolId = Value
        End Set
    End Property

    Public Property VolIdL() As Integer
        Get
            VolIdL = pVolIdL
        End Get
        Set(ByVal Value As Integer)
            pVolIdL = Value
        End Set
    End Property

    Public Property VolName() As String
        Get
            VolName = pVolName
        End Get
        Set(ByVal Value As String)
            pVolName = Value
        End Set
    End Property
End Class