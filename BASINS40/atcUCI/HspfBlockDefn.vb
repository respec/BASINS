' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

''' <summary>
''' Keyed Collection of HspfBlockDef
''' </summary>
''' <remarks>Key is Name</remarks>
Public Class HspfBlockDefs
    Inherits KeyedCollection(Of String, HspfBlockDef)
    Protected Overrides Function GetKeyForItem(ByVal aHspfBlockDef As HspfBlockDef) As String
        Return aHspfBlockDef.Name
    End Function
End Class

''' <summary>
''' HSPF Block Definition
''' </summary>
Public Class HspfBlockDef
    Public Name As String
    Public Id As Integer
    Public SectionDefs As HspfSectionDefs
    Public TableDefs As HspfTableDefs

    Public Function SectionID(ByRef aSectionName As String) As Integer
        Dim lSectionID As Integer = 0
        For lIndex As Integer = 0 To SectionDefs.Count - 1
            If SectionDefs.Item(lIndex).Name = aSectionName Then
                If Name <> "RCHRES" Or (Name = "RCHRES" And lIndex < 8) Then
                    lSectionID = lIndex
                Else
                    lSectionID = lIndex - 1
                End If
            End If
        Next lIndex
        Return lSectionID
    End Function
End Class