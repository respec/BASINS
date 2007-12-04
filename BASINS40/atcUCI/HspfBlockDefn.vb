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
            Return pName
        End Get
        Set(ByVal aName As String)
            pName = aName
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Return pId
        End Get
        Set(ByVal aId As Integer)
            pId = aId
        End Set
    End Property

    Public Property SectionDefs() As Collection
        Get 'of HspfSectionDef
            Return pSectionDefs
        End Get
        Set(ByVal aSectionDefs As Collection) 'of HspfSectionDef
            pSectionDefs = aSectionDefs
        End Set
    End Property

    Public Property TableDefs() As Collection
        Get 'of HspfTableDef
            Return pTableDefs
        End Get
        Set(ByVal aTableDefs As Collection) 'of HspfTableDef
            pTableDefs = aTableDefs
        End Set
    End Property

    Public Function SectionID(ByRef aSectionName As String) As Integer
        Dim lIndex As Integer
        Dim lSectionID As Integer = 0

        For lIndex = 1 To pSectionDefs.Count
            If pSectionDefs.Item(lIndex).Name = aSectionName Then
                If pName <> "RCHRES" Or (pName = "RCHRES" And lIndex < 8) Then
                    lSectionID = lIndex - 1
                Else
                    lSectionID = lIndex - 2
                End If
            End If
        Next lIndex

        Return lSectionID
    End Function
End Class