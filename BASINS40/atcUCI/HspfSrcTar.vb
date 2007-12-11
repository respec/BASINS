'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfSrcTar
    Private pOpn As HspfOperation

    Public Group As String
    Public Member As String
    Public MemSub1 As Integer
    Public MemSub2 As Integer
    Public VolId As Integer
    Public VolIdL As Integer
    Public VolName As String
    Public Property Opn() As HspfOperation
        Get
            Return pOpn
        End Get
        Set(ByVal Value As HspfOperation)
            pOpn = Value
            VolName = pOpn.Name
            If pOpn.Id < VolId Then
                VolId = pOpn.Id
            End If
        End Set
    End Property
End Class