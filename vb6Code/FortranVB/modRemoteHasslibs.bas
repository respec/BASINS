Attribute VB_Name = "modRemoteHasslibs"
Option Explicit

Public Sub REM_GLOBLK(myUci As HspfUci, sdatim&(), edatim&(), outlev&, spout&, runfg&, emfg&, rninfo$)
  Dim M$, i&
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "GLOBLK"
  M = myUci.WaitForChildMessage
  For i = 0 To 5
    sdatim(i) = StrRetRem(M)
  Next
  For i = 0 To 5
    edatim(i) = StrRetRem(M)
  Next
  outlev = StrRetRem(M)
  spout = StrRetRem(M)
  runfg = StrRetRem(M)
  emfg = StrRetRem(M)
  rninfo = M
End Sub

Public Sub REM_GLOPRMI(myUci As HspfUci, ival&, parmname$)
  Dim M$, i&
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "GLOPRMI " & parmname
  M = myUci.WaitForChildMessage
  ival = -999
  If Len(Trim(M)) > 0 Then
    If IsNumeric(M) Then
      ival = CInt(M)
    End If
  End If
End Sub

Public Sub REM_XBLOCK(myUci As HspfUci, blkno&, init&, retkey&, cbuff$, retcod&)
  Dim M$, i&
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "XBLOCK " & blkno & " " & init
  M = myUci.WaitForChildMessage
  'Debug.Print blkno & "=" & M
  blkno = StrRetRem(M)
  init = StrRetRem(M)
  retkey = StrRetRem(M)
  i = InStr(M, " ")
  retcod = Left(M, i - 1)
  cbuff = Right(M, Len(M) - i)
End Sub

Public Sub REM_XBLOCKEX(myUci As HspfUci, blkno&, init&, retkey&, cbuff$, rectyp&, retcod&)
  Dim M$, i&
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "XBLOCKEX " & blkno & " " & init & " " & retkey
  M = myUci.WaitForChildMessage
  Debug.Print blkno & "=" & M
  blkno = StrRetRem(M)
  init = StrRetRem(M)
  retkey = StrRetRem(M)
  rectyp = StrRetRem(M)
  i = InStr(M, " ")
  retcod = Left(M, i - 1)
  cbuff = Right(M, Len(M) - i)
End Sub

Public Sub REM_GTNXKW(myUci As HspfUci, init&, Id&, ckwd$, kwdfg&, contfg&, retid&)
  Dim M$
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "GTNXKW " & init & " " & Id
  M = myUci.WaitForChildMessage
  init = StrRetRem(M)
  Id = StrRetRem(M)
  kwdfg = StrRetRem(M)
  contfg = StrRetRem(M)
  retid = StrRetRem(M)
  ckwd = M
End Sub

Public Sub REM_GETOCR(myUci As HspfUci, itype&, noccur&)
  Dim M$
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "GETOCR " & itype
  M = myUci.WaitForChildMessage
  itype = StrRetRem(M)
  noccur = M
End Sub

Public Sub REM_XTABLEEX(myUci As HspfUci, OmCode&, tabno&, uunits&, init&, addfg&, Occur&, retkey&, cbuff$, rectyp&, retcod&)
  Dim M$, i&
  
  myUci.Monitor.SendProcessMessage "HSPFUCI", "XTABLEEX " & OmCode & " " & tabno & " " & uunits & " " & init & " " & addfg & " " & Occur & " " & retkey
  M = myUci.WaitForChildMessage
  retkey = StrRetRem(M)
  rectyp = StrRetRem(M)
  i = InStr(M, " ")
  retcod = Left(M, i - 1)
  cbuff = Right(M, Len(M) - i)

End Sub


