Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMetSegRecord_NET.HspfMetSegRecord")> Public Class HspfMetSegRecord
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Enum MetSegRecordType
        msrUNK = 0
        msrPREC
        msrGATMP
        msrDTMPG
        msrWINMOV
        msrSOLRAD
        msrCLOUD
        msrPETINP
        msrPOTEV
    End Enum

    Private pMFactP As Double
    Private pMFactR As Double
    Private pTyp As Integer
    Private pTran As String
    Private pSgapstrg As String
    Private pSsystem As String
    Private pSource As HspfSrcTar

    Public Property MFactP() As Double
        Get
            MFactP = pMFactP
        End Get
        Set(ByVal Value As Double)
            pMFactP = Value
        End Set
    End Property

    Public Property MFactR() As Double
        Get
            MFactR = pMFactR
        End Get
        Set(ByVal Value As Double)
            pMFactR = Value
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

    Public Property Tran() As String
        Get
            Tran = pTran
        End Get
        Set(ByVal Value As String)
            pTran = Value
        End Set
    End Property

    Public Property typ() As Integer
        Get
            typ = pTyp
        End Get
        Set(ByVal Value As Integer)
            pTyp = Value
        End Set
    End Property

    Public Function Compare(ByRef tMetSegRecord As HspfMetSegRecord, ByRef opname As String) As Boolean
        Compare = True
        If opname = "PERLND" Or opname = "IMPLND" Then
            If tMetSegRecord.MFactP <> Me.MFactP Then
                Compare = False
            End If
        ElseIf opname = "RCHRES" Then
            If tMetSegRecord.MFactR <> Me.MFactR And Me.MFactR <> -999.0# Then
                Compare = False
            End If
        End If

        If opname = "RCHRES" And Me.MFactP = -999.0# And Me.MFactR = -999 Then
            'dont bother to compare.  this is a rchres, and mfactp has not
            'been set, so whatever this record contains it will be fine.
            '(for situation like basins evap, that only gets written for rchres)
        ElseIf opname = "RCHRES" And tMetSegRecord.MFactR = -999.0# Then
            'dont bother to compare.  this is a rchres, mfactp has been set
            'but mfactr is not set, whatever this record contains will be fine.
            '(for situation like basins pevt, that only gets written for per/implnd
        Else
            If tMetSegRecord.Tran <> Me.Tran Then
                Compare = False
            ElseIf tMetSegRecord.Sgapstrg <> Me.Sgapstrg Then
                Compare = False
            ElseIf tMetSegRecord.Ssystem <> Me.Ssystem Then
                Compare = False
            ElseIf tMetSegRecord.Source.VolName <> Me.Source.VolName Then
                Compare = False
            ElseIf tMetSegRecord.Source.VolId <> Me.Source.VolId Then
                Compare = False
            ElseIf tMetSegRecord.Source.Member <> Me.Source.Member Then
                Compare = False
            ElseIf tMetSegRecord.Source.MemSub1 <> Me.Source.MemSub1 Then
                Compare = False
            ElseIf tMetSegRecord.Source.MemSub2 <> Me.Source.MemSub2 Then
                Compare = False
            End If
        End If

    End Function

    Public Sub New()
        MyBase.New()
        pSource = New HspfSrcTar
        pTyp = 0
        pMFactP = -999.0#
        pMFactR = -999.0#
        pSgapstrg = ""
        pSsystem = ""
    End Sub
End Class