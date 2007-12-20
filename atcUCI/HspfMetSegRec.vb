'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfMetSegRecords
    Inherits KeyedCollection(Of String, HspfMetSegRecord)
    Protected Overrides Function GetKeyForItem(ByVal aMetSegRecord As HspfMetSegRecord) As String
        Return aMetSegRecord.Name
    End Function
End Class

Public Class HspfMetSegRecord
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

    Public MFactP As Double
    Public MFactR As Double
    Public Typ As Integer
    Public Name As String
    Public Tran As String
    Public Sgapstrg As String
    Public Ssystem As String
    Public Source As HspfSrcTar

    Public Function Compare(ByVal aTargetMetSegRecord As HspfMetSegRecord, _
                            ByRef aOperationName As String) As Boolean
        Compare = True
        If aOperationName = "PERLND" Or aOperationName = "IMPLND" Then
            If aTargetMetSegRecord.MFactP <> Me.MFactP Then
                Compare = False
            End If
        ElseIf aOperationName = "RCHRES" Then
            If aTargetMetSegRecord.MFactR <> Me.MFactR And Me.MFactR <> -999.0# Then
                Compare = False
            End If
        End If

        If aOperationName = "RCHRES" And Me.MFactP = -999.0# And Me.MFactR = -999 Then
            'dont bother to compare.  this is a rchres, and mfactp has not
            'been set, so whatever this record contains it will be fine.
            '(for situation like basins evap, that only gets written for rchres)
        ElseIf aOperationName = "RCHRES" And aTargetMetSegRecord.MFactR = -999.0# Then
            'dont bother to compare.  this is a rchres, mfactp has been set
            'but mfactr is not set, whatever this record contains will be fine.
            '(for situation like basins pevt, that only gets written for per/implnd
        Else
            If aTargetMetSegRecord.Tran <> Me.Tran Then
                Compare = False
            ElseIf aTargetMetSegRecord.Sgapstrg <> Me.Sgapstrg Then
                Compare = False
            ElseIf aTargetMetSegRecord.Ssystem <> Me.Ssystem Then
                Compare = False
            ElseIf aTargetMetSegRecord.Source.VolName <> Me.Source.VolName Then
                Compare = False
            ElseIf aTargetMetSegRecord.Source.VolId <> Me.Source.VolId Then
                Compare = False
            ElseIf aTargetMetSegRecord.Source.Member <> Me.Source.Member Then
                Compare = False
            ElseIf aTargetMetSegRecord.Source.MemSub1 <> Me.Source.MemSub1 Then
                Compare = False
            ElseIf aTargetMetSegRecord.Source.MemSub2 <> Me.Source.MemSub2 Then
                Compare = False
            End If
        End If

    End Function

    Public Sub New()
        MyBase.New()
        Source = New HspfSrcTar
        Typ = 0
        MFactP = -999.0#
        MFactR = -999.0#
        Sgapstrg = ""
        Ssystem = ""
    End Sub
End Class