Attribute VB_Name = "UciRead"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public Function readParmDef(nameParm$) As HSPFParmDef
  Dim d As HSPFParmDef
  
  Set d = New HSPFParmDef

  If nameParm = "OutLev" Then
    d.Define = "Run Interpreter Output Level"
  ElseIf nameParm = "RunInf" Then
    d.Define = "Run Information"
  End If
  
  Set readParmDef = d
End Function

Public Function HspfOmCode(OmName$) As Long
  Select Case OmName
    Case "GLOBAL": HspfOmCode = 2
    Case "OPN SEQUENCE": HspfOmCode = 3
    Case "FTABLES": HspfOmCode = 4
    Case "EXT SOURCES": HspfOmCode = 5
    Case "FORMATS": HspfOmCode = 6
    Case "NETWORK": HspfOmCode = 7
    Case "EXT TARGETS": HspfOmCode = 8
    Case "SPEC-ACTIONS": HspfOmCode = 9
    Case "SCHEMATIC": HspfOmCode = 10
    Case "MASS-LINK": HspfOmCode = 11
    Case "PERLND": HspfOmCode = 100
    Case "IMPLND": HspfOmCode = 100
    Case "RCHRES": HspfOmCode = 100
    Case "COPY": HspfOmCode = 100
    Case "PLTGEN": HspfOmCode = 100
    Case "DISPLY": HspfOmCode = 100
    Case "DURANL": HspfOmCode = 100
    Case "GENER": HspfOmCode = 100
    Case "MUTSIN": HspfOmCode = 100
    Case "BMPRAC": HspfOmCode = 100
    Case "REPORT": HspfOmCode = 100
    Case "FILES": HspfOmCode = 12
    Case "CATEGORY": HspfOmCode = 13
    Case "MONTH-DATA": HspfOmCode = 14
    Case "PATHNAMES": HspfOmCode = 15
  End Select
End Function

Public Function HspfOperName(Index As HspfOperType) As String
  Select Case Index
    Case hPerlnd: HspfOperName = "PERLND"
    Case hImplnd: HspfOperName = "IMPLND"
    Case hRchres: HspfOperName = "RCHRES"
    Case hCopy: HspfOperName = "COPY"
    Case hPltgen: HspfOperName = "PLTGEN"
    Case hDisply: HspfOperName = "DISPLY"
    Case hDuranl: HspfOperName = "DURANL"
    Case hGener: HspfOperName = "GENER"
    Case hMutsin: HspfOperName = "MUTSIN"
    Case hBmprac: HspfOperName = "BMPRAC"
    Case hReport: HspfOperName = "REPORT"
    Case Else: HspfOperName = "UNKNOWN"
  End Select
End Function

Public Function HspfOperNum(Name As String) As HspfOperType
  Select Case Name
    Case "PERLND": HspfOperNum = hPerlnd
    Case "IMPLND": HspfOperNum = hImplnd
    Case "RCHRES": HspfOperNum = hRchres
    Case "COPY": HspfOperNum = hCopy
    Case "PLTGEN": HspfOperNum = hPltgen
    Case "DISPLY": HspfOperNum = hDisply
    Case "DURANL": HspfOperNum = hDuranl
    Case "GENER": HspfOperNum = hGener
    Case "MUTSIN": HspfOperNum = hMutsin
    Case "BMPRAC": HspfOperNum = hBmprac
    Case "REPORT": HspfOperNum = hReport
    Case Else: HspfOperNum = 0
  End Select
End Function

Public Function HspfSpecialRecordName(myType As HspfSpecialRecordType) As String
  Dim s$
  
  Select Case myType
    Case hComment: s = "Comment"
    Case hAction: s = "Action"
    Case hDistribute: s = "Distribute"
    Case hUserDefineName: s = "User Defn Name"
    Case hUserDefineQuan: s = "User Defn Quan"
    Case hCondition: s = "Condition"
    Case Else: s = "Unknown"
  End Select
  HspfSpecialRecordName = s
End Function

Public Function myFormatI(i&, fieldWidth&)
  Dim s$
  
  s = Space(fieldWidth)
  RSet s = CStr(i)
  myFormatI = s
End Function

Public Function compareTableString(skipF&, skipL&, str1$, str2$) As Boolean
  Dim lstr1$, lstr2$, len1&, len2&
  
  On Error GoTo NoMatch:
  
  len1 = Len(str1)
  len2 = Len(str2)
  If len1 <> len2 Then GoTo NoMatch:
  If skipF = 1 Then 'skip at left side
    lstr1 = Right(str1, len1 - skipL)
    lstr2 = Right(str2, len2 - skipL)
  ElseIf skipL = len1 Then 'skip at right side
    lstr1 = Left(str1, skipF - 1)
    lstr2 = Left(str2, skipF - 1)
  Else 'skip in middle
    lstr1 = Left(str1, skipF - 1) & Right(str1, len1 - skipL)
    lstr2 = Left(str2, skipF - 1) & Right(str2, len1 - skipL)
  End If
  If lstr1 <> lstr2 Then GoTo NoMatch:
  'match
  compareTableString = True
  Exit Function
NoMatch:
  compareTableString = False
End Function
