'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports MapWinUtility

Module modUtilEdit
    Public Sub editInit(ByRef b As Object, _
                        ByRef aIcon As System.Drawing.Image, _
               Optional ByRef addRemFlg As Boolean = False, _
               Optional ByRef editFlg As Boolean = False, _
               Optional ByRef applyFlg As Boolean = True)

        Logger.Dbg("EditInit:" & b.ToString)
        '  Dim f As frmEdit
        '
        '  Set f = New frmEdit
        '  f.init b, f, addRemFlg, editFlg, applyFlg
        '  Set f.icon = aicon
        '  f.Show vbModal
    End Sub

    Public Sub editActivityAllInit(ByRef b As Object, _
                                   ByRef icon As System.Drawing.Image)

        '  Dim f As frmActivityAll
        '
        '  Set f = New frmActivityAll
        '  f.init b, f
        '  Set f.icon = icon
        '  f.Show vbModal
    End Sub
End Module