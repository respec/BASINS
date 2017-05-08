Attribute VB_Name = "HassLibs"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants
'util:utchar
Declare Sub F90_DATLST_XX Lib "hass_lib.dll" (l&, l&, l&, l&)
Declare Sub F90_DECCHX_XX Lib "hass_lib.dll" (r!, l&, l&, l&, l&)
'util:utsort
Declare Sub F90_ASRTRP Lib "hass_lib.dll" (l&, r!)
'util:utdate
Declare Sub F90_CMPTIM Lib "hass_lib.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_DATNXT Lib "hass_lib.dll" (l&, l&, l&)
Declare Function F90_DAYMON Lib "hass_lib.dll" (l&, l&) As Long
Declare Sub F90_TIMADD Lib "hass_lib.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_TIMDIF Lib "hass_lib.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_TIMCNV Lib "hass_lib.dll" (l&)
Declare Sub F90_TIMCVT Lib "hass_lib.dll" (l&)
Declare Sub F90_TIMBAK Lib "hass_lib.dll" (l&, l&)
Declare Function F90_TIMCHK Lib "hass_lib.dll" (l&, l&) As Long
Declare Sub F90_JDMODY Lib "hass_lib.dll" (l&, l&, l&, l&)
Declare Sub F90_DTMCMN Lib "hass_lib.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_DECPRC Lib "hass_lib.dll" (l&, l&, r!)

Private Sub ChrNum(ilen&, istr$, onam&())
    Dim i%
    For i = 1 To ilen
      If i <= Len(istr) Then
        onam(i - 1) = Asc(Mid(istr, i, 1))
      Else
        onam(i - 1) = 32
      End If
    Next i
End Sub

Private Sub NumChr(ilen&, inam&(), outstr$)

    Dim i&
    
    outstr = ""
    For i = 0 To ilen
      If inam(i) > 0 Then
        outstr = outstr & Chr(inam(i))
      End If
    Next i
    outstr = RTrim(outstr)

End Sub
Private Sub NumChrA(icnt&, ilen&, inam() As Long, outstr() As String)

    Dim i&, j&, s$, p&, jnam&()
 
    ReDim jnam(ilen)
    p = 0
    For i = 0 To icnt - 1
      For j = 0 To ilen - 1
        jnam(j) = inam(p + j)
      Next j
      Call NumChr(ilen, jnam, s)
      outstr(i) = s
      p = p + ilen
   Next i

End Sub


Public Sub F90_DATLST(CurDat&(), DatStr$)
   Dim l&, e&, i&(21)
   
   Call F90_DATLST_XX(CurDat(0), i(0), l, e)
   Call NumChr(l, i, DatStr)

End Sub


Public Sub F90_DECCHX(reain!, ilen&, sigdig&, decpla&, RStr$)

   Dim i&()
   ReDim i(ilen)

   Call F90_DECCHX_XX(reain, ilen, sigdig, decpla, i(0))
   Call NumChr(ilen, i, RStr)

End Sub
