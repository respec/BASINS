Attribute VB_Name = "UtilStat"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants


Function gausex(exprob!) As Single

  'GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
     'GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
     'GAUSAB=VALUE (NOT EXCEEDED) WITH PROBCUMPROB
     'GAUSCF=CUMULATIVE PROBABILITY FUNCTION
     'GAUSDY=DENSITY FUNCTION
  'SUBPGMS USED -- NONE

  'GAUSCF MODIFIED 740906 WK -- REPLACED ERF FCN REF BY RATIONAL APPRX N
  'ALSO REMOVED DOUBLE PRECISION FROM GAUSEX AND GAUSAB.
  '76-05-04 WK -- TRAP UNDERFLOWS IN EXP IN GUASCF AND DY.

  'rev 8/96 by PRH for VB

  Const c0! = 2.515517
  Const c1! = 0.802853
  Const c2! = 0.010328
  Const d1! = 1.432788
  Const d2! = 0.189269
  Const d3! = 0.001308
  Dim pr!, rtmp!, p!, t!, numerat!, Denom!

  p = exprob
  If p >= 1# Then
    'set to minimum
    rtmp = -10#
  ElseIf p <= 0# Then
    'set at maximum
    rtmp = 10#
  Else
    'compute value
    pr = p
    If p > 0.5 Then pr = 1# - pr
    t = (-2# * Log(pr)) ^ 0.5
    numerat = (c0 + t * (c1 + t * c2))
    Denom = (1# + t * (d1 + t * (d2 + t * d3)))
    rtmp = t - numerat / Denom
    If p > 0.5 Then rtmp = -rtmp
  End If
  gausex = rtmp

End Function

