Option Strict Off
Option Explicit On
Module utilEdit
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	Public myMsgBox As ATCoCtl.ATCoMessage
	
	Public Sub editInit(ByRef b As Object, ByRef icon As System.Drawing.Image, Optional ByRef addRemFlg As Boolean = False, Optional ByRef editFlg As Boolean = False, Optional ByRef applyFlg As Boolean = True)
		
		'  Dim f As frmEdit
		'
		'  Set f = New frmEdit
		'  f.init b, f, addRemFlg, editFlg, applyFlg
		'  Set f.icon = icon
		'  f.Show vbModal
	End Sub
	
	Public Sub editActivityAllInit(ByRef b As Object, ByRef icon As System.Drawing.Image)
		'  Dim f As frmActivityAll
		'
		'  Set f = New frmActivityAll
		'  f.init b, f
		'  Set f.icon = icon
		'  f.Show vbModal
	End Sub
End Module