Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfMonthDataTable_NET.HspfMonthDataTable")> Public Class HspfMonthDataTable
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pId As Integer
    Private pMonthValues(12) As Double
    Private pBlock As HspfMonthData
    Private pReferencedBy As Collection 'of hspfoperation

    Public Comment As String = ""
    Public Id As Integer
    Public Block As HspfMonthData

    Public Property MonthValue(ByVal aMonth As Integer) As Single
        Get
            MonthValue = pMonthValues(aMonth)
        End Get
        Set(ByVal newValue As Single)
            pMonthValues(aMonth) = newValue
        End Set
    End Property

    Public ReadOnly Property ReferencedBy() As Collection
        Get
            ReferencedBy = pReferencedBy
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        pReferencedBy = New Collection
    End Sub
End Class