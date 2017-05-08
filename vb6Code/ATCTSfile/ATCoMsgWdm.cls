VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoMsgWDM"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants
Dim pMsgUnit&
Dim pAttributes As Collection 'of clsAttributeWDM

Public Property Let MsgUnit(newMsgUnit As Long)
  Dim i&
  Dim ilen&, itype&, rmin!, rmax!, rdef!
  Dim hlen&, hrec&, hpos&, vlen&
  Dim aName$, desc$, valid$
  Dim myAttr As clsAttributeWDM
  
  Dim lMsg As String
  lMsg = "ATCoMsgWDM:MsgUnit:Let:" & newMsgUnit
  Call F90_MSG(lMsg, Len(lMsg))
  
  pMsgUnit = newMsgUnit
  For i = 1 To 500
    Call F90_WDSAGY(pMsgUnit, i, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, aName, desc, valid)
    Set myAttr = New clsAttributeWDM
    If ilen = 0 Then 'dummy
      myAttr.Name = "Dummy" & i
      pAttributes.Add myAttr, "K" & i
    Else
      myAttr.Name = aName
      myAttr.Ind = i
      myAttr.Description = desc
      myAttr.ValidValues = valid
      If InStr("-TGROUP-TSFORM-VBTIME-COMPFG-TSFILL-TSBYR-TSBMO-TSBDY-TSBHR-TSPREC-TSSTEP-TCODE-Time Units-Time Step-", _
               "-" & aName & "-") > 0 Then
        myAttr.Editable = False
      Else
        myAttr.Editable = True
      End If
      myAttr.ilen = ilen
      Select Case itype
        Case 1: myAttr.DataType = ATCoInt
        Case 2: myAttr.DataType = ATCoSng
        Case 3: myAttr.DataType = ATCoTxt
      End Select
      'myAttr.itype = itype
      myAttr.Min = rmin
      myAttr.Max = rmax
      myAttr.Default = rdef
      myAttr.hlen = hlen
      myAttr.hrec = hrec
      myAttr.hpos = hpos
      myAttr.vlen = vlen
      pAttributes.Add myAttr, aName
    End If
  Next i
  
End Property

Public Property Get Attrib(AttrName As String) As clsAttributeWDM
  On Error GoTo NoSuchAttribute
  Set Attrib = pAttributes(AttrName)
  Exit Property
NoSuchAttribute:
  Set Attrib = Nothing
End Property

Public Property Get Attributes() As Collection
  Set Attributes = pAttributes
End Property

Private Sub Class_Initialize()
  Set pAttributes = New Collection
End Sub
