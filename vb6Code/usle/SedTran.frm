VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   2496
   ClientLeft      =   48
   ClientTop       =   336
   ClientWidth     =   3744
   LinkTopic       =   "Form1"
   ScaleHeight     =   2496
   ScaleWidth      =   3744
   StartUpPosition =   3  'Windows Default
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Function TopoFactor() As Double
  Dim Angle As Double, SFactor As Double, LFactor As Double, _
      Slope As Double, Coeff As Double, M As Double, B As Double
  Dim Length&
  Dim LandUse$
  Dim Freeze As Boolean
  
  LandUse = adgtable.textmatrix(.row, LandUseCOL)
  Slope = CDbl(adgtable.textmatrix(.row, InclineCOL))
  Length = CDbl(adgtable.textmatrix(.row, LengthCOL))
  If Length < 15 Then Length = 15
  Angle = (Atn(adgtable.textmatrix(.row, InclineCOL) / 100))  ' define angle in radians
  
  ' Calculate the L Factor
  Select Case LandUse
    LandUse = "Permanent Grassland": Coeff = 0.5
    LandUse = "Cropland": Coeff = 1
    LandUse = "Construction Sites": Coeff = 2
  End Select
  B = (Sin(Angle) / 0.0896) / (3 * (Sin(Angle)) ^ 0.8 + 0.56)
  M = Coeff * B / (1 + Coeff * B)
  LFactor = (Length / 72.6) ^ M

  ' Calculate the S Factor
  If LandUse = "Permanent Grass" _
      Or LandUse = "Agriculture" _
      Or LandUse = "Construction Sites" Then
    If Length >= 15 Then
      If Slope < 9 Then
        SFactor = 10.8 * Sin(Angle) + 0.03
      ElseIf Freeze = False Then
        SFactor = 16.8 * Sin(Angle) - 0.5
      Else  ' Following equation for areas with significant freeze/thaw cycles
        SFactor = (Sin(Angle) / 0.0896) ^ 0.6
      End If
    Else
      If Slope < 9 Then
        SFactor = 10.8 * Sin(Angle) + 0.03
      Else
        SFactor = 3 * (Sin(Angle)) ^ 0.8 + 0.56
        TopoFactor = 10 ^ (Log(LFactor * SFactor) + (0.078 * Log(LFactor) - 0.037))
      End If
    End If
  End If
  If TopoFactor = 0 Then
    TopoFactor = LFactor * SFactor
  End If
End Function

