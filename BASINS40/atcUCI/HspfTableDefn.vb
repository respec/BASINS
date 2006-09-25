Option Strict Off
Option Explicit On

Imports atcUtility

<System.Runtime.InteropServices.ProgId("HspfTableDef_NET.HspfTableDef")> _
Public Class HspfTableDef
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pDefine As String
    Private pHeaderE As String
    Private pHeaderM As String
    Private pId As Integer
    Private pName As String
    Private pNumOccur As Integer
    Private pOccurGroup As Integer
    Private pParent As HspfSectionDef
    Private pParmDefs As Collection 'of HSPFParm
    Private pSGRP As Integer

    Public Property Define() As String
        Get
            Define = pDefine
        End Get
        Set(ByVal Value As String)
            pDefine = ReplaceString(Value, vbCrLf, " ")
        End Set
    End Property

    Public Property HeaderE() As String
        Get
            HeaderE = pHeaderE
        End Get
        Set(ByVal Value As String)
            pHeaderE = Value
        End Set
    End Property

    Public Property HeaderM() As String
        Get
            HeaderM = pHeaderM
        End Get
        Set(ByVal Value As String)
            pHeaderM = Value
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

    Public Property Name() As String
        Get
            Name = pName
        End Get
        Set(ByVal Value As String)
            pName = Value
        End Set
    End Property

    Public Property NumOccur() As Integer
        Get
            NumOccur = pNumOccur
        End Get
        Set(ByVal Value As Integer)
            pNumOccur = Value
        End Set
    End Property

    Public Property OccurGroup() As Integer
        Get
            OccurGroup = pOccurGroup
        End Get
        Set(ByVal Value As Integer)
            pOccurGroup = Value
        End Set
    End Property

    Public Property Parent() As HspfSectionDef
        Get
            Parent = pParent
        End Get
        Set(ByVal Value As HspfSectionDef)
            pParent = Value
        End Set
    End Property

    Public Property ParmDefs() As Collection
        Get 'of HSPFParm
            ParmDefs = pParmDefs
        End Get
        Set(ByVal Value As Collection) 'of HSPFParm
            pParmDefs = Value
        End Set
    End Property

    Public Property SGRP() As Integer
        Get
            SGRP = pSGRP
        End Get
        Set(ByVal Value As Integer)
            pSGRP = Value
        End Set
    End Property
End Class