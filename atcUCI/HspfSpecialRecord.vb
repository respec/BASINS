Option Strict Off
Option Explicit On

''' <summary>
''' HSPF Special Record
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
<System.Runtime.InteropServices.ProgId("HspfSpecialRecord_NET.HspfSpecialRecord")> _
Public Class HspfSpecialRecord
    Dim pText As String
    Dim pSpecType As HspfData.HspfSpecialRecordType

    Public Property Text() As String
        Get
            Text = pText
        End Get
        Set(ByVal Value As String)
            pText = Value
        End Set
    End Property

    Public Property SpecType() As HspfData.HspfSpecialRecordType
        Get
            SpecType = pSpecType
        End Get
        Set(ByVal Value As HspfData.HspfSpecialRecordType)
            pSpecType = Value
        End Set
    End Property
End Class