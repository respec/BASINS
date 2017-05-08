Attribute VB_Name = "UtilPest"
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Sub ProcessKwdColl(spec As String, KwdColl As FastCollection, Optional com As String = "", Optional KwdCollCom As FastCollection)
  'add contents of spec to keyword collection, KwdColl
  'if provided also adds comment to associated comment collection, KwdCollCom
  '*** assumes both collections exist already or have been initialized ***

  KwdColl.Add spec, spec
  If Len(com) > 0 Then
    If Not IsMissing(KwdCollCom) Then
      KwdCollCom.Add com, spec
    End If
  End If

End Sub
