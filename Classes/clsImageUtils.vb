'********************************************************************************************************
'File Name: clsImageUtils.vb
'Description: Provides some conversion routines to go between image formats
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'
'June 16, 2005: Christopher Michaelis cmichaelis@happysquirrel.com
'               Added this class with the needed functions to get away from using VisualBasic.Compatibility,
'               which no longer ships with Visual Studio (as of 2003)
'           Provides:
'               ImageToIPictureDisp and vice versa, IPictureDispToImage
'               TwipsPerPixelX, TwipsPerPixelY
Imports System.Drawing

Public Class ImageUtils
    Inherits System.Windows.Forms.AxHost

    Public Sub New()
        MyBase.New("59EE46BA-677D-4d20-BF10-8D8067CB8B33")
        'The GUID here has no meaning.
    End Sub


    Public Shared Function ObjectToImage(ByVal Picture As Object, Optional ByVal newWidth As Integer = -1, Optional ByVal newHeight As Integer = -1) As Image
        Dim img As Image = Nothing
        If TypeOf Picture Is Icon Then
            img = CType(Picture, Icon).ToBitmap
        ElseIf TypeOf Picture Is Image Then
            img = CType(Picture, Image)
        ElseIf TypeOf Picture Is stdole.IPictureDisp Then
            Dim ipdisp As stdole.IPictureDisp = CType(Picture, stdole.IPictureDisp)

            Const PIC_BITMAP As Integer = 1
            Const PIC_ICON As Integer = 3

            If ipdisp.Type = PIC_BITMAP Then
                'This is a shared function; create an instance
                'of myself so I can convert this.
                Dim cvter As New ImageUtils
                img = cvter.IPictureDispToImage(Picture)
            ElseIf ipdisp.Type = PIC_ICON Then
                Throw New System.Exception("VB6 Icons not currently supported")
            Else
                Throw New System.Exception("Unsupported image format")
            End If
        End If

        Dim retval As Image

        If newHeight > 0 And newWidth > 0 Then
            retval = New Bitmap(newWidth, newHeight)
            Dim drawtool As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(retval)
            If img IsNot Nothing Then
                drawtool.DrawImage(img, New Rectangle(0, 0, newWidth, newHeight))
            End If
        Else
            retval = img
        End If

        Return retval
    End Function

    'This cannot be "Shared"!

    '??? There was a warning about using something like MyBase here so I 
    'changed it on June 28, 2007.  Does 'This cannot be "Shared" mean
    'that it HAS to be called through MyBase to work?  And now that we are
    'using System.Windows.Forms.AxHost.GetIPictureDispFromPicture(image)
    'instead of MyBase wouldn't it be possible to share it?  Hope I didn't
    'break anything with this change...
    Public Function ImageToIPictureDisp(ByVal image As Drawing.Image) As Object
        Return System.Windows.Forms.AxHost.GetIPictureDispFromPicture(image)
    End Function

    'This cannot be "Shared"!
    Public Function IPictureDispToImage(ByVal image As Object) As Drawing.Image
        Return System.Windows.Forms.AxHost.GetPictureFromIPicture(image)
    End Function


    Private Declare Function GetDC Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function ReleaseDC Lib "user32" (ByVal hwnd As Long, _
      ByVal hdc As Long) As Long
    Private Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Long, _
      ByVal nIndex As Long) As Long

    Const HWND_DESKTOP As Long = 0
    Const LOGPIXELSX As Long = 88
    Const LOGPIXELSY As Long = 90

    Public Shared Function TwipsPerPixelX() As Single
        '--------------------------------------------------
        'Returns the width of a pixel, in twips.
        '--------------------------------------------------
        Dim lngDC As Long
        lngDC = GetDC(HWND_DESKTOP)
        Dim pixPerInch As Double
        pixPerInch = GetDeviceCaps(lngDC, LOGPIXELSX)
        TwipsPerPixelX = 1440& / pixPerInch
        MsgBox("a: " & TwipsPerPixelX)
        ReleaseDC(HWND_DESKTOP, lngDC)
    End Function

    Public Shared Function TwipsPerPixelY() As Single
        '--------------------------------------------------
        'Returns the height of a pixel, in twips.
        '--------------------------------------------------
        Dim lngDC As Long
        lngDC = GetDC(HWND_DESKTOP)
        TwipsPerPixelY = 1440& / GetDeviceCaps(lngDC, LOGPIXELSY)
        MsgBox("b: " & TwipsPerPixelY)
        ReleaseDC(HWND_DESKTOP, lngDC)
    End Function
End Class
