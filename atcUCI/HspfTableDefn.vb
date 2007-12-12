'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcUtility
Imports System.Collections.ObjectModel

Public Class HspfTableDefs
    Inherits KeyedCollection(Of String, HspfTableDef)
    Protected Overrides Function GetKeyForItem(ByVal aHspfTableDef As HspfTableDef) As String
        Return aHspfTableDef.Name
    End Function
End Class

Public Class HspfTableDef
    Public HeaderE As String
    Public HeaderM As String
    Public Id As Integer
    Public Name As String
    Public NumOccur As Integer
    Public OccurGroup As Integer
    Public Parent As HspfSectionDef
    Public ParmDefs As HspfParmDefs
    Public SGRP As Integer
    Private pDefine As String
    Public Property Define() As String
        Get
            Define = pDefine
        End Get
        Set(ByVal Value As String)
            pDefine = ReplaceString(Value, vbCrLf, " ")
        End Set
    End Property
End Class