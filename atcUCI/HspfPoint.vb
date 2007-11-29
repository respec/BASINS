Option Strict Off
Option Explicit On

Imports System.Text

''' <summary>
''' HSPF Point Source
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Public Class HspfPoint
    Private pId As Integer
    Private pName As String
    Private pCon As String
    Private pMFact As Double
    Private pRFact As Double
    Private pTran As String
    Private pSgapstrg As String
    Private pSsystem As String
    Private pSource As HspfSrcTar
    Private pTarget As HspfSrcTar
    Private pAssocOper As Integer

    Public Property Id() As Integer
        Get
            Id = pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property AssocOper() As Integer
        Get
            AssocOper = pAssocOper
        End Get
        Set(ByVal Value As Integer)
            pAssocOper = Value
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

    Public Property Con() As String
        Get
            Con = pCon
        End Get
        Set(ByVal Value As String)
            pCon = Value
        End Set
    End Property

    Public Property MFact() As Double
        Get
            MFact = pMFact
        End Get
        Set(ByVal Value As Double)
            pMFact = Value
        End Set
    End Property

    Public Property RFact() As Double
        Get
            RFact = pRFact
        End Get
        Set(ByVal Value As Double)
            pRFact = Value
        End Set
    End Property

    Public Property Ssystem() As String
        Get
            Ssystem = pSsystem
        End Get
        Set(ByVal Value As String)
            pSsystem = Value
        End Set
    End Property

    Public Property Sgapstrg() As String
        Get
            Sgapstrg = pSgapstrg
        End Get
        Set(ByVal Value As String)
            pSgapstrg = Value
        End Set
    End Property

    Public Property Source() As HspfSrcTar
        Get
            Source = pSource
        End Get
        Set(ByVal Value As HspfSrcTar)
            pSource = Value
        End Set
    End Property

    Public Property Target() As HspfSrcTar
        Get
            Target = pTarget
        End Get
        Set(ByVal Value As HspfSrcTar)
            pTarget = Value
        End Set
    End Property

    Public Property Tran() As String
        Get
            Tran = pTran
        End Get
        Set(ByVal Value As String)
            pTran = Value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        pSource = New HspfSrcTar
        pTarget = New HspfSrcTar
        pMFact = 1.0#
        pRFact = 1.0#
        pSgapstrg = ""
        pSsystem = ""
    End Sub

    Public Function ToStringFromSpecs(ByRef icol() As Integer, ByRef ilen() As Integer) As String
        Dim lStr As New StringBuilder
        Dim t As String

        lStr.Append(pSource.VolName.Trim)
        lStr.Append(Space(icol(1) - lStr.Length - 1)) 'pad prev field
        lStr.Append(CStr(pSource.VolId).PadLeft(ilen(1)))
        lStr.Append(Space(icol(2) - lStr.Length - 1))
        lStr.Append(pSource.Member)
        lStr.Append(Space(icol(3) - lStr.Length - 1))
        If pSource.MemSub1 <> 0 Then
            lStr.Append(CStr(pSource.MemSub1).PadLeft(ilen(3)))
        End If
        lStr.Append(Space(icol(4) - lStr.Length - 1))
        lStr.Append(Me.Ssystem)
        lStr.Append(Space(icol(5) - lStr.Length - 1))
        lStr.Append(Me.Sgapstrg)
        lStr.Append(Space(icol(6) - lStr.Length - 1))
        If Me.MFact <> 1 Then
            lStr.Append(CStr(Me.MFact).PadLeft(ilen(6)))
        End If
        lStr.Append(Space(icol(7) - lStr.Length - 1))
        lStr.Append(Me.Tran)
        lStr.Append(Space(icol(8) - lStr.Length - 1))
        lStr.Append(Me.Target.VolName)
        lStr.Append(Space(icol(9) - lStr.Length - 1))
        If Me.Target.VolId > 0 And Me.Target.VolIdL > 0 Then
            'have a range of operations, just write the one for the assoc oper
            lStr.Append(CStr(Me.AssocOper).PadLeft(ilen(9)))
        Else
            lStr.Append(CStr(Me.Target.VolId).PadLeft(ilen(9)))
        End If
        lStr.Append(Space(icol(11) - lStr.Length - 1))
        lStr.Append(Me.Target.Group)
        lStr.Append(Space(icol(12) - lStr.Length - 1))
        lStr.Append(Me.Target.Member)
        lStr.Append(Space(icol(13) - lStr.Length - 1))
        If Me.Target.MemSub1 > 0 Then
            t = CStr(Me.Target.MemSub1).PadLeft(ilen(13))
            If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 1, t)
            lStr.Append(t)
            lStr.Append(Space(icol(14) - lStr.Length - 1))
        End If
        If Me.Target.MemSub2 > 0 Then
            t = CStr(Me.Target.MemSub2).PadLeft(ilen(14))
            If Me.Target.VolName = "RCHRES" Then t = Me.Target.Opn.Uci.IntAsCat(Me.Target.Member, 2, t)
            lStr.Append(t)
        End If
        Return lStr.ToString
    End Function
End Class