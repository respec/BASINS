Attribute VB_Name = "FtnIO"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Private LastLenRd As Long
Private LastLenWr As Long
Private LastHdrLen As Long

Function FtnUnfSeqInitRd(n$) As Long
    Dim f&, b As Byte
    
    f = FreeFile(0)
    Open n For Binary Access Read As #f
    Get #f, , b
    If b <> &HFD Then 'lahey code for FtnUnfSeq (UserManual-App A)
      Close f
      f = -1
    End If
    LastLenRd = 0
    
    FtnUnfSeqInitRd = f
    
End Function

Function FtnUnfSeqInitWr(n$) As Long
    Dim f&, b As Byte
    
    f = FreeFile(0)
    Open n For Binary Access Write As #f
    'put hex header at top of file
    b = &HFD
    Put #f, , b
    LastLenWr = 0
    LastHdrLen = 0
    
    FtnUnfSeqInitWr = f
    
End Function

Sub FtnUnfSeqReset()
  LastLenRd = 0
End Sub

Function FtnUnfSeqRecLen(f%) As Long
    Dim b As Byte, RecLen As Long, bytes As Integer, c As Long

    If LastLenRd > 0 Then 'flush the trailing record length
      c = 64
      Get #f, , b
      While LastLenRd > c
        c = c * 256
        Get #f, , b
      Wend
    End If
    Get #f, , b 'now the record length for this record
    bytes = b And 3
    RecLen = (b - bytes) \ 4 'truncate
    c = 64
    Do While bytes > 0
      Get #f, , b
      bytes = bytes - 1
      RecLen = RecLen + b * c
      c = c * 256
    Loop
    LastLenRd = RecLen
    FtnUnfSeqRecLen = RecLen

End Function

Sub FtnUnfSeqWrHead(f%, l&)
    Dim b As Byte, x As Long
    Dim c As Long, r As Integer, i As Long
 
    r = 0
    If LastLenWr > 0 Then  'finish trailing record length
      If LastLenWr >= &H400000 Then 'will need 4 bytes
        r = r + 1
        c = LastLenWr \ &H100 'truncate
        b = c
        Put #f, , b
        LastLenWr = LastLenWr - (c * &H100)
      End If
      If LastLenWr >= &H4000 Or r > 0 Then 'at least 3 bytes
        r = r + 1
        c = LastLenWr \ &H100 'truncate
        b = c
        Put #f, , b
        LastLenWr = LastLenWr - (c * &H100)
      End If
      If LastLenWr >= &H40 Or r > 0 Then 'at least 2 bytes
        r = r + 1
        c = LastLenWr \ &H40 'truncate
        b = c
        Put #f, , b
        LastLenWr = LastLenWr - (c * &H40)
      End If
      If LastLenWr > 0 Or r > 0 Then 'at least 1 byte
        b = LastLenWr * 4 + r
        LastLenWr = 0
        Put #f, , b
      End If
    End If

    If l > 0 Then
       If l >= &H400000 Then
         r = 3
       ElseIf l >= &H4000 Then
         r = 2
       ElseIf l >= &H40 Then
         r = 1
       Else
         r = 0
       End If
       LastLenWr = l + r + 1
       c = l Mod &H40
       x = (l - c) \ &H40 'truncate
       b = c * 4 + r
       Put #f, , b
       For i = 1 To r Step 1
          c = x Mod &H100
          x = (x - c) \ &H100 'truncate
          b = c
          Put #f, , b
       Next i
    End If
End Sub

Sub FtnUnfSeqWrStr(f, l, s)
    Dim b As Byte, lc&, ls&
    
    ls = Len(s)
    
    For lc = 1 To l
      If lc > ls Then
        b = &H20 'pad with blank
      Else
        b = asc(Mid(s, lc, 1))
      End If
      Put #f, , b
    Next lc
End Sub

Public Function FtnUnFmtClean(b() As Byte) As Byte()
  Dim c() As Byte, l&, iIn&, iOu&, r, i&, first As Boolean

  If b(0) = &HFD Then 'this is a lahey unformatted byte stream
    l = UBound(b)
    ReDim c(l)
    iIn = 1
    iOu = 0
    first = True
    While iIn < l
      r = FtnUnfSeqRecLenMem(b, iIn, first)
      For i = 1 To r
        c(iOu) = b(iIn)
        iOu = iOu + 1
        iIn = iIn + 1
      Next i
    Wend
    ReDim Preserve c(iOu - 1)
    FtnUnFmtClean = c
  Else
    FtnUnFmtClean = b
  End If
End Function

Private Function FtnUnfSeqRecLenMem(b() As Byte, ind As Long, first As Boolean) As Long
  Dim RecLen As Long, bytes As Integer, c As Long
  Static LastLen As Long
  
  If first Then
    LastLen = 0
    first = False
  Else
    c = 64
    ind = ind + 1
    While LastLen > c
      c = c * 256
      ind = ind + 1
    Wend
  End If
  If ind > UBound(b) Then 'all done
    RecLen = 1
  Else
    bytes = b(ind) And 3
    RecLen = b(ind) / 4
    ind = ind + 1
    c = 64
    Do While bytes > 0
      bytes = bytes - 1
      RecLen = RecLen + b(ind) * c
      ind = ind + 1
      c = c * 256
    Loop
  End If
  LastLen = RecLen - 1
  FtnUnfSeqRecLenMem = RecLen - 1
End Function
