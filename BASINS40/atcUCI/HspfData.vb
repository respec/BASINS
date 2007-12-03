Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("HspfData_NET.HspfData")> Public Class HspfData
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Structure HspfFile
        Dim Typ As String 'valid are MESSU, WDM(x), DSSx or blank
        Dim Unit As Integer 'use 21-99
        Dim Name As String 'complete path
        Dim Comment As String 'preceeding comment
    End Structure

    Public Structure HspfCategory
        Dim Id As Integer
        Dim Tag As String
        Dim Name As String
        Dim Comment As String 'preceeding comment
    End Structure

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