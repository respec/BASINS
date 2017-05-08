VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsUninstallFile"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
Dim pName As String
Dim pExceptions As String

Private Type unsHeader
  id As String * 64
  appId As String * 128
  appName As String * 128
  version As Integer
  numRecs As Integer
  endOffSet As Long
  flags As Long
End Type
Dim pHeader As unsHeader

Private Type unsString
  type As Integer
  'type definitions:
  'utUserDefined          = $01;
  'utStartInstall         = $10;
  'utEndInstall           = $11;
  'utRun                  = $80;
  'utDeleteDirOrFiles     = $81;
  'utDeleteFile           = $82;
  'utDeleteGroupOrItem    = $83;
  'utIniDeleteEntry       = $84;
  'utIniDeleteSection     = $85;
  'utRegDeleteEntireKey   = $86;
  'utRegClearValue        = $87;
  'utRegDeleteKeyIfEmpty  = $88;
  'utRegDeleteValue       = $89;
  'utDecrementSharedCount = $8A;
  'utRefreshFileAssoc     = $8B;
  'utMutexCheck           = $8C;
  value As String
End Type
Dim pString As unsString
Dim pStrings As FastCollection 'of unsString
Dim pCounts As FastCollection 'of long, key is rec id

Property Get Exceptions() As String
  Exceptions = pExceptions
End Property

Property Let Name(newName As String)
  Dim lByte() As Byte, oByte() As Byte
  Dim Ind As Long, bInd As Long, i As Long, j As Long, ilen As Long
  Dim eOut As Boolean
  Dim t As String
  Dim Count As Long
  Dim SkipStart As Long, Skip As Boolean, SkipCount
  Dim CrapCount As Long, CrapPrev As Long
  Dim BaseFile As String
  Const CheckStr As String = "Inno Setup Uninstall Log"
  
  Set pStrings = New FastCollection
  Set pCounts = New FastCollection
  pCounts.AllowUpdates = True
  
  pName = newName
  
  oByte = WholeFileBytes(newName)
  ReDim lByte(UBound(oByte))
  While Ind < UBound(oByte) - 7
    If oByte(Ind) = 0 And _
       oByte(Ind + 1) = &H10 And _
       oByte(Ind + 2) = 0 And _
       oByte(Ind + 3) = 0 And _
       oByte(Ind + 6) = &HFF And _
       oByte(Ind + 7) = &HFF Then
      pExceptions = pExceptions & "  CrapAt:" & Hex(Ind) & " Offset:" & Hex(Ind - CrapPrev) & vbCrLf
      CrapCount = CrapCount + 1
      CrapPrev = Ind
      Ind = Ind + 12
    Else
      lByte(i) = oByte(Ind)
      i = i + 1
      Ind = Ind + 1
    End If
  Wend
  ReDim Preserve lByte(i)
  
  If UBound(lByte) > 328 Then
    With pHeader
      .id = Byte2String(lByte, 0, 64)
      .appId = Byte2String(lByte, 64, 128)
      .appName = Byte2String(lByte, 192, 128)
      .version = Byte2Integer(lByte, 320)
      .numRecs = Byte2Integer(lByte, 322)
      .endOffSet = Byte2Long(lByte, 324)
    End With
  End If
  
  If Left(pHeader.id, Len(CheckStr)) <> CheckStr Then
    pExceptions = "'" & pName & "' is NOT an Inno Uninstall Log File"
  Else
    Ind = 447
    While Ind < UBound(lByte) - 1
      If lByte(Ind) = &HFF Then
        Ind = Ind + 1
        If lByte(Ind) = &H10 Or lByte(Ind) = &H11 Then
          'Debug.Print "StartStop:" & Hex(Ind)
          's = s & Hex(lByte(Ind)) & " " & Hex(Ind) & vbCrLf
          Count = Count + 1
        ElseIf lByte(Ind) >= &H80 And lByte(Ind) <= &H8C Then
          If Skip Then 'show what was skipped
            If Ind - SkipStart > 4 Then
              pExceptions = pExceptions & "  Skip:" & Hex(SkipStart) & "-" & Hex(Ind - 1) & " L:" & Ind - SkipStart & " V:" & Hex(lByte(SkipStart)) & vbCrLf
            End If
            Skip = False
            SkipCount = SkipCount + 1
          End If
          
          If pCounts.KeyExists(lByte(Ind)) Then
            i = pCounts.IndexFromKey(lByte(Ind))
            pCounts(i) = pCounts(i) + 1
          Else
            pCounts.Add 1, CStr(lByte(Ind))
          End If
          
          bInd = Ind
          Count = Count + 1
          With pString
            .type = Byte2Integer(lByte, Ind)
            .value = ""
            Ind = Ind + 9
            ilen = (lByte(Ind) * 256) + lByte(Ind + 1)
            Ind = Ind + 2
            eOut = False
            i = Ind
            If Ind + ilen > UBound(lByte) Then
              ilen = UBound(lByte) - Ind
            End If
            While i < Ind + ilen And i <= UBound(lByte)
              If lByte(i) < 32 Or lByte(i) > 240 Then 'exception, skip it??
                i = i + 12
                ilen = ilen + 12
                eOut = True
              Else
                .value = .value & Byte2String(lByte, i, 1)
                i = i + 1
              End If
            Wend
            If Ind + ilen > UBound(lByte) Then
              ilen = UBound(lByte) - Ind
            End If
            If eOut Then
              pExceptions = pExceptions & vbCrLf & Hex(.type) & " " & Hex(bInd) & " " & ilen & " '" & Byte2String(lByte, Ind, ilen) & "'" & vbCrLf & "  "
              For j = bInd To Ind + ilen - 1
                t = Hex(lByte(j))
                If Len(t) = 1 Then t = "0" & t
                pExceptions = pExceptions & t & " "
              Next j
              pExceptions = pExceptions & vbCrLf & Space(8) & Len(.value) & " '" & .value & "'" & vbCrLf & vbCrLf
            End If
            Ind = i
            'Debug.Print Hex(.type), ilen, .value
            's = s & Hex(.type) & " " & Hex(bInd) & " " & ilen & " '" & .value & "'" & vbCrLf
          End With
          pStrings.Add Hex(pString.type) & vbTab & pString.value
        End If
      Else
        If Not Skip Then
          SkipStart = Ind
          Skip = True
        End If
        Ind = Ind + 1
      End If
    Wend
  End If
   
End Property
Property Get Name() As String
  Name = pName
End Property

Property Get HeaderAsString() As String
  With pHeader
    HeaderAsString = "Id:'" & Trim(.id) & "'" & vbCrLf & _
        "AppId:'" & Trim(.appId) & "'" & vbCrLf & _
        "AppName:'" & Trim(.appName) & "'" & vbCrLf & _
        "Version:" & .version & vbCrLf & _
        "NumRecs:" & .numRecs & vbCrLf & _
        "EndOffSet:" & .endOffSet & vbCrLf
  End With
End Property

Property Get CountsAsString() As String
  Dim s As String, i As Long, totalCnt As Long
  
  For i = 1 To pCounts.Count
    s = s & Hex(pCounts.key(i)) & vbTab & pCounts.ItemByIndex(i) & vbCrLf
    totalCnt = totalCnt + pCounts.ItemByIndex(i)
  Next
  CountsAsString = s & "Total" & vbTab & totalCnt & " " & pStrings.Count & vbCrLf
End Property

Property Get Strings() As FastCollection
  Set Strings = pStrings
End Property
