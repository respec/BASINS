Option Strict Off
Option Explicit On
Module UciRead
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Function readParmDef(ByRef nameParm As String) As HSPFParmDef
        Dim d As HSPFParmDef

        d = New HSPFParmDef

        If nameParm = "OutLev" Then
            d.Define = "Run Interpreter Output Level"
        ElseIf nameParm = "RunInf" Then
            d.Define = "Run Information"
        End If

        readParmDef = d
    End Function

    Public Function HspfOmCode(ByRef OmName As String) As Integer
        Select Case OmName
            Case "GLOBAL" : HspfOmCode = 2
            Case "OPN SEQUENCE" : HspfOmCode = 3
            Case "FTABLES" : HspfOmCode = 4
            Case "EXT SOURCES" : HspfOmCode = 5
            Case "FORMATS" : HspfOmCode = 6
            Case "NETWORK" : HspfOmCode = 7
            Case "EXT TARGETS" : HspfOmCode = 8
            Case "SPEC-ACTIONS" : HspfOmCode = 9
            Case "SCHEMATIC" : HspfOmCode = 10
            Case "MASS-LINK" : HspfOmCode = 11
            Case "PERLND" : HspfOmCode = 100
            Case "IMPLND" : HspfOmCode = 100
            Case "RCHRES" : HspfOmCode = 100
            Case "COPY" : HspfOmCode = 100
            Case "PLTGEN" : HspfOmCode = 100
            Case "DISPLY" : HspfOmCode = 100
            Case "DURANL" : HspfOmCode = 100
            Case "GENER" : HspfOmCode = 100
            Case "MUTSIN" : HspfOmCode = 100
            Case "BMPRAC" : HspfOmCode = 100
            Case "REPORT" : HspfOmCode = 100
            Case "FILES" : HspfOmCode = 12
            Case "CATEGORY" : HspfOmCode = 13
            Case "MONTH-DATA" : HspfOmCode = 14
            Case "PATHNAMES" : HspfOmCode = 15
        End Select
    End Function

    Public Function HspfOperName(ByRef Index As HspfData.HspfOperType) As String
        Select Case Index
            Case HspfData.HspfOperType.hPerlnd : HspfOperName = "PERLND"
            Case HspfData.HspfOperType.hImplnd : HspfOperName = "IMPLND"
            Case HspfData.HspfOperType.hRchres : HspfOperName = "RCHRES"
            Case HspfData.HspfOperType.hCopy : HspfOperName = "COPY"
            Case HspfData.HspfOperType.hPltgen : HspfOperName = "PLTGEN"
            Case HspfData.HspfOperType.hDisply : HspfOperName = "DISPLY"
            Case HspfData.HspfOperType.hDuranl : HspfOperName = "DURANL"
            Case HspfData.HspfOperType.hGener : HspfOperName = "GENER"
            Case HspfData.HspfOperType.hMutsin : HspfOperName = "MUTSIN"
            Case HspfData.HspfOperType.hBmprac : HspfOperName = "BMPRAC"
            Case HspfData.HspfOperType.hReport : HspfOperName = "REPORT"
            Case Else : HspfOperName = "UNKNOWN"
        End Select
    End Function

    Public Function HspfOperNum(ByRef Name As String) As HspfData.HspfOperType
        Select Case Name
            Case "PERLND" : HspfOperNum = HspfData.HspfOperType.hPerlnd
            Case "IMPLND" : HspfOperNum = HspfData.HspfOperType.hImplnd
            Case "RCHRES" : HspfOperNum = HspfData.HspfOperType.hRchres
            Case "COPY" : HspfOperNum = HspfData.HspfOperType.hCopy
            Case "PLTGEN" : HspfOperNum = HspfData.HspfOperType.hPltgen
            Case "DISPLY" : HspfOperNum = HspfData.HspfOperType.hDisply
            Case "DURANL" : HspfOperNum = HspfData.HspfOperType.hDuranl
            Case "GENER" : HspfOperNum = HspfData.HspfOperType.hGener
            Case "MUTSIN" : HspfOperNum = HspfData.HspfOperType.hMutsin
            Case "BMPRAC" : HspfOperNum = HspfData.HspfOperType.hBmprac
            Case "REPORT" : HspfOperNum = HspfData.HspfOperType.hReport
            Case Else : HspfOperNum = 0
        End Select
    End Function

    Public Function HspfSpecialRecordName(ByRef myType As HspfData.HspfSpecialRecordType) As String
        Dim s As String

        Select Case myType
            Case HspfData.HspfSpecialRecordType.hComment : s = "Comment"
            Case HspfData.HspfSpecialRecordType.hAction : s = "Action"
            Case HspfData.HspfSpecialRecordType.hDistribute : s = "Distribute"
            Case HspfData.HspfSpecialRecordType.hUserDefineName : s = "User Defn Name"
            Case HspfData.HspfSpecialRecordType.hUserDefineQuan : s = "User Defn Quan"
            Case HspfData.HspfSpecialRecordType.hCondition : s = "Condition"
            Case Else : s = "Unknown"
        End Select
        HspfSpecialRecordName = s
    End Function

    Public Function myFormatI(ByRef i As Integer, ByRef fieldWidth As Integer) As String
        Dim s As String
        s = Space(fieldWidth)
        s = RSet(CStr(i), Len(s))
        Return s
    End Function

    Public Function compareTableString(ByRef skipF As Integer, ByRef skipL As Integer, ByRef str1 As String, ByRef str2 As String) As Boolean
        Dim lstr1, lstr2 As String
        Dim len1, len2 As Integer
        Try
            len1 = Len(str1)
            len2 = Len(str2)
            If len1 <> len2 Then Return False
            If skipF = 1 Then 'skip at left side
                lstr1 = Right(str1, len1 - skipL)
                lstr2 = Right(str2, len2 - skipL)
            ElseIf skipL = len1 Then  'skip at right side
                lstr1 = Left(str1, skipF - 1)
                lstr2 = Left(str2, skipF - 1)
            Else 'skip in middle
                lstr1 = Left(str1, skipF - 1) & Right(str1, len1 - skipL)
                lstr2 = Left(str2, skipF - 1) & Right(str2, len1 - skipL)
            End If
            If lstr1 <> lstr2 Then Return False
            Return True 'match
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module