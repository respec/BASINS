'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

<System.Runtime.InteropServices.ProgId("HspfStatusType_NET.HspfStatusType")> _
Public Class HspfStatusType
    Public Defn As HspfTSMemberDef
    Public Max As Integer
    Public Name As String
    Public Occur As Integer
    Public Present As HspfStatus.HspfStatusPresentMissingEnum
    Public ReqOptUnn As HspfStatus.HspfStatusReqOptUnnEnum
    Public Tag As String

    Public Sub New()
        MyBase.New()
        Name = ""
        Occur = 0
        ReqOptUnn = 0
        Present = HspfStatus.HspfStatusPresentMissingEnum.HspfStatusMissing
        ReqOptUnn = HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusUnneeded
    End Sub
End Class