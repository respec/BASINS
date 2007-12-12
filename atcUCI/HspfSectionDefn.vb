'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfSectionDefs
    Inherits KeyedCollection(Of String, HspfSectionDef)
    Protected Overrides Function GetKeyForItem(ByVal aSectionDef As HspfSectionDef) As String
        Return aSectionDef.Name
    End Function
End Class

Public Class HspfSectionDef
    Public Name As String
    Public Id As Integer
    Public TableDefs As HspfTableDefs
End Class