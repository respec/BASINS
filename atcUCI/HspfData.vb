'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Public Class HspfData
    Public Enum HspfOperType
        hPerlnd = 1
        hImplnd = 2
        hRchres = 3
        hCopy = 4
        hPltgen = 5
        hDisply = 6
        hDuranl = 7
        hGener = 8
        hMutsin = 9
        hBmprac = 10
        hReport = 11
    End Enum

    Public Enum HspfSpecialRecordType
        hAction = 1
        hDistribute = 2
        hUserDefineName = 3
        hUserDefineQuan = 4
        hCondition = 5
        hComment = 6
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub
End Class