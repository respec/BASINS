'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfTSGroupDefs
    Inherits KeyedCollection(Of String, HspfTSGroupDef)
    Protected Overrides Function GetKeyForItem(ByVal aHspfTSGroupDef As HspfTSGroupDef) As String
        Return aHspfTSGroupDef.Id
    End Function
End Class

Public Class HspfTSGroupDef
    Public BlockID As Integer
    Public Id As Integer
    Public MemberDefs As Collection(Of HspfTSMemberDef)
    Public Name As String
End Class