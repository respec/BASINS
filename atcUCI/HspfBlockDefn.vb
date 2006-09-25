Option Strict Off
Option Explicit On
''' <summary>
''' HSPF Block Definition
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
<System.Runtime.InteropServices.ProgId("HspfBlockDef_NET.HspfBlockDef")> Public Class HspfBlockDef
    Private pName As String
    Private pId As Integer
    Private pSectionDefs As Collection 'of HspfSectionDef
    Private pTableDefs As Collection 'of HspfTableDef

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

    Public Property SectionDefs() As Collection
        Get 'of HspfSectionDef
            SectionDefs = pSectionDefs
        End Get
        Set(ByVal Value As Collection) 'of HspfSectionDef
            pSectionDefs = Value
        End Set
    End Property

    Public Property TableDefs() As Collection
        Get 'of HspfTableDef
            TableDefs = pTableDefs
        End Get
        Set(ByVal Value As Collection) 'of HspfTableDef
            pTableDefs = Value
        End Set
    End Property

    Public Function SectionID(ByRef inname As String) As Integer
        Dim i As Integer
        SectionID = 0
        For i = 1 To pSectionDefs.Count()
            If pSectionDefs.Item(i).Name = inname Then
                If pName <> "RCHRES" Or (pName = "RCHRES" And i < 8) Then
                    SectionID = i - 1
                Else
                    SectionID = i - 2
                End If
            End If
        Next i
    End Function
End Class