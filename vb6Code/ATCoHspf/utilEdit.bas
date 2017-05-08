Attribute VB_Name = "utilEdit"
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Public myMsgBox As ATCoMessage

Public Sub editInit(b As Object, icon As StdPicture, _
  Optional addRemFlg As Boolean = False, _
  Optional editFlg As Boolean = False, _
  Optional applyFlg As Boolean = True)
  
  Dim f As frmEdit
  
  Set f = New frmEdit
  f.init b, f, addRemFlg, editFlg, applyFlg
  Set f.icon = icon
  f.Show vbModal
End Sub

Public Sub editActivityAllInit(b As Object, icon As StdPicture)
  Dim f As frmActivityAll
  
  Set f = New frmActivityAll
  f.init b, f
  Set f.icon = icon
  f.Show vbModal
End Sub


