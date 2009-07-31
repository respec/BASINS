'********************************************************************************************************
'File Name: clsColors.vb
'Description: Color-related utilities that are useful.
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source Utility Library. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'********************************************************************************************************

Imports System.drawing

Public Class Colors
    Public Shared Sub GetRGB(ByVal color As Integer, ByRef r As Integer, ByRef g As Integer, ByRef b As Integer)
        r = color And &HFF
        g = CInt((color And &HFF00) / 256) 'shift right 8 bits
        b = CInt((color And &HFF0000) / 65536) ' shift right 16 bits
    End Sub

    Public Shared Function HSLToRGB(ByVal Hue As Integer, ByVal Saturation As Integer, ByVal Luminance As Integer) As Integer
        Dim R As Integer, G As Integer, B As Integer
        Dim lMax As Integer, lMid As Integer, lMin As Integer
        Dim q As Single

        lMax = CInt((Luminance * 255) / 100)
        lMin = CInt((100 - Saturation) * lMax / 100)
        q = CSng((lMax - lMin) / 60)

        Select Case Hue
            Case 0 To 60
                lMid = CInt((Hue - 0) * q + lMin)
                R = lMax : G = lMid : B = lMin
            Case 60 To 120
                lMid = CInt(-(Hue - 120) * q + lMin)
                R = lMid : G = lMax : B = lMin
            Case 120 To 180
                lMid = CInt((Hue - 120) * q + lMin)
                R = lMin : G = lMax : B = lMid
            Case 180 To 240
                lMid = CInt(-(Hue - 240) * q + lMin)
                R = lMin : G = lMid : B = lMax
            Case 240 To 300
                lMid = CInt((Hue - 240) * q + lMin)
                R = lMid : G = lMin : B = lMax
            Case 300 To 360
                lMid = CInt(-(Hue - 360) * q + lMin)
                R = lMax : G = lMin : B = lMid
        End Select

        Return CInt(B * &H10000 + G * &H100& + R)
    End Function

    Public Shared Function ColorToInteger(ByVal c As Color) As Integer
        Return RGB(c.R, c.G, c.B)
    End Function

    <CLSCompliant(False)> _
    Public Shared Function ColorToUInteger(ByVal c As Color) As UInt32
        Return System.Convert.ToUInt32(RGB(c.R, c.G, c.B))
    End Function

    <CLSCompliant(False)> _
    Public Shared Function IntegerToColor(ByVal IntColor As UInt32) As Color
        Dim r, g, b As Integer
        GetRGB(System.Convert.ToInt32(IntColor), r, g, b)
        Return Color.FromArgb(255, r, g, b)
    End Function

    Public Shared Function IntegerToColor(ByVal IntColor As Integer) As Color
        Dim r, g, b As Integer
        GetRGB(IntColor, r, g, b)
        Return Color.FromArgb(255, r, g, b)
    End Function
End Class
