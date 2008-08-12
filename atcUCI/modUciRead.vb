Option Explicit On

Module modUciRead
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Function readParmDef(ByRef aNameParm As String) As HSPFParmDef
        Dim lParmDef As New HSPFParmDef

        If aNameParm = "OutLev" Then
            lParmDef.Define = "Run Interpreter Output Level"
        ElseIf aNameParm = "RunInf" Then
            lParmDef.Define = "Run Information"
        End If

        Return lParmDef
    End Function

    Public Function HspfOmCode(ByRef aOmName As String) As Integer
        Dim lHspfOmCode As Integer

        Select Case aOmName
            Case "GLOBAL" : lHspfOmCode = 2
            Case "OPN SEQUENCE" : lHspfOmCode = 3
            Case "FTABLES" : lHspfOmCode = 4
            Case "EXT SOURCES" : lHspfOmCode = 5
            Case "FORMATS" : lHspfOmCode = 6
            Case "NETWORK" : lHspfOmCode = 7
            Case "EXT TARGETS" : lHspfOmCode = 8
            Case "SPEC-ACTIONS" : lHspfOmCode = 9
            Case "SCHEMATIC" : lHspfOmCode = 10
            Case "MASS-LINK" : lHspfOmCode = 11
            Case "PERLND" : lHspfOmCode = 100
            Case "IMPLND" : lHspfOmCode = 100
            Case "RCHRES" : lHspfOmCode = 100
            Case "COPY" : lHspfOmCode = 100
            Case "PLTGEN" : lHspfOmCode = 100
            Case "DISPLY" : lHspfOmCode = 100
            Case "DURANL" : lHspfOmCode = 100
            Case "GENER" : lHspfOmCode = 100
            Case "MUTSIN" : lHspfOmCode = 100
            Case "BMPRAC" : lHspfOmCode = 100
            Case "REPORT" : lHspfOmCode = 100
            Case "FILES" : lHspfOmCode = 12
            Case "CATEGORY" : lHspfOmCode = 13
            Case "MONTH-DATA" : lHspfOmCode = 14
            Case "PATHNAMES" : lHspfOmCode = 15
            Case Else : lHspfOmCode = 0
        End Select

        Return lHspfOmCode
    End Function

    Public Function HspfOperName(ByRef aIndex As HspfData.HspfOperType) As String
        Dim lHspfOperName As String

        Select Case aIndex
            Case HspfData.HspfOperType.hPerlnd : lHspfOperName = "PERLND"
            Case HspfData.HspfOperType.hImplnd : lHspfOperName = "IMPLND"
            Case HspfData.HspfOperType.hRchres : lHspfOperName = "RCHRES"
            Case HspfData.HspfOperType.hCopy : lHspfOperName = "COPY"
            Case HspfData.HspfOperType.hPltgen : lHspfOperName = "PLTGEN"
            Case HspfData.HspfOperType.hDisply : lHspfOperName = "DISPLY"
            Case HspfData.HspfOperType.hDuranl : lHspfOperName = "DURANL"
            Case HspfData.HspfOperType.hGener : lHspfOperName = "GENER"
            Case HspfData.HspfOperType.hMutsin : lHspfOperName = "MUTSIN"
            Case HspfData.HspfOperType.hBmprac : lHspfOperName = "BMPRAC"
            Case HspfData.HspfOperType.hReport : lHspfOperName = "REPORT"
            Case Else : lHspfOperName = "UNKNOWN"
        End Select

        Return lHspfOperName
    End Function

    Public Function HspfOperNum(ByRef aName As String) As HspfData.HspfOperType
        Dim lHspfOperNum As HspfData.HspfOperType

        Select Case aName
            Case "PERLND" : lHspfOperNum = HspfData.HspfOperType.hPerlnd
            Case "IMPLND" : lHspfOperNum = HspfData.HspfOperType.hImplnd
            Case "RCHRES" : lHspfOperNum = HspfData.HspfOperType.hRchres
            Case "COPY" : lHspfOperNum = HspfData.HspfOperType.hCopy
            Case "PLTGEN" : lHspfOperNum = HspfData.HspfOperType.hPltgen
            Case "DISPLY" : lHspfOperNum = HspfData.HspfOperType.hDisply
            Case "DURANL" : lHspfOperNum = HspfData.HspfOperType.hDuranl
            Case "GENER" : lHspfOperNum = HspfData.HspfOperType.hGener
            Case "MUTSIN" : lHspfOperNum = HspfData.HspfOperType.hMutsin
            Case "BMPRAC" : lHspfOperNum = HspfData.HspfOperType.hBmprac
            Case "REPORT" : lHspfOperNum = HspfData.HspfOperType.hReport
            Case Else : lHspfOperNum = 0
        End Select

        Return lHspfOperNum
    End Function

    Public Function myFormatI(ByRef aValue As Integer, ByRef aWidth As Integer) As String
        Dim lString As String

        lString = Space(aWidth)
        lString = RSet(CStr(aValue), lString.Length)

        Return lString
    End Function

    Public Function compareTableString(ByRef aSkipF As Integer, ByRef aSkipL As Integer, _
                                       ByRef aStr1 As String, ByRef aStr2 As String) As Boolean
        Dim lStr1, lStr2 As String
        Dim lLen1, lLen2 As Integer
        Try
            lLen1 = aStr1.Length
            lLen2 = aStr2.Length
            If lLen1 <> lLen2 Then Return False
            If aSkipF = 1 Then 'skip at left side
                lStr1 = Right(aStr1, lLen1 - aSkipL)
                lStr2 = Right(aStr2, lLen2 - aSkipL)
            ElseIf aSkipL = lLen1 Then  'skip at right side
                lStr1 = Left(aStr1, aSkipF - 1)
                lStr2 = Left(aStr2, aSkipF - 1)
            Else 'skip in middle
                lStr1 = Left(aStr1, aSkipF - 1) & Right(aStr1, lLen1 - aSkipL)
                lStr2 = Left(aStr2, aSkipF - 1) & Right(aStr2, lLen1 - aSkipL)
            End If
            If lStr1 <> lStr2 Then Return False
            Return True 'match
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module