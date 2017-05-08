VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfTSMemberDef"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Dim pName As String
Dim pId As Long
Dim pTSGroupID As Long
Dim pParent As HspfTSGroupDef
Dim pSCLU As Long
Dim pSGRP As Long
Dim pmdim1 As Long
Dim pmdim2 As Long
Dim pmaxsb1 As Long
Dim pmaxsb2 As Long
Dim pmkind As Long
Dim psptrn As Long
Dim pmsect As Long
Dim pmio As Long
Dim posvbas As Long
Dim posvoff As Long
Dim peunits As String
Dim pltval1 As Single
Dim pltval2 As Single
Dim pltval3 As Single
Dim pltval4 As Single
Dim pdefn As String
Dim pmunits As String
Dim pltval5 As Single
Dim pltval6 As Single
Dim pltval7 As Single
Dim pltval8 As Single

Public Property Get Name() As String
  Name = pName
End Property
Public Property Let Name(newName As String)
  pName = newName
End Property

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get TSGroupId() As Long
  TSGroupId = pTSGroupID
End Property
Public Property Let TSGroupId(newTSGroupId As Long)
  pTSGroupID = newTSGroupId
End Property

Public Property Get Parent() As HspfTSGroupDef
  Set Parent = pParent
End Property
Public Property Set Parent(newParent As HspfTSGroupDef)
  Set pParent = newParent
End Property

Public Property Get SCLU() As Long
  SCLU = pSCLU
End Property
Public Property Let SCLU(newSCLU As Long)
  pSCLU = newSCLU
End Property

Public Property Get SGRP() As Long
  SGRP = pSGRP
End Property
Public Property Let SGRP(newSGRP As Long)
  pSGRP = newSGRP
End Property

Public Property Get mdim1() As Long
  mdim1 = pmdim1
End Property
Public Property Let mdim1(newmdim1 As Long)
  pmdim1 = newmdim1
End Property

Public Property Get mdim2() As Long
  mdim2 = pmdim2
End Property
Public Property Let mdim2(newmdim2 As Long)
  pmdim2 = newmdim2
End Property

Public Property Get maxsb1() As Long
  maxsb1 = pmaxsb1
End Property
Public Property Let maxsb1(newmaxsb1 As Long)
  pmaxsb1 = newmaxsb1
End Property

Public Property Get maxsb2() As Long
  maxsb2 = pmaxsb2
End Property
Public Property Let maxsb2(newmaxsb2 As Long)
  pmaxsb2 = newmaxsb2
End Property

Public Property Get mkind() As Long
  mkind = pmkind
End Property
Public Property Let mkind(newmkind As Long)
  pmkind = newmkind
End Property

Public Property Get sptrn() As Long
  sptrn = psptrn
End Property
Public Property Let sptrn(newsptrn As Long)
  psptrn = newsptrn
End Property

Public Property Get msect() As Long
  msect = pmsect
End Property
Public Property Let msect(newmsect As Long)
  pmsect = newmsect
End Property

Public Property Get mio() As Long
  mio = pmio
End Property
Public Property Let mio(newmio As Long)
  pmio = newmio
End Property

Public Property Get osvbas() As Long
  osvbas = posvbas
End Property
Public Property Let osvbas(newosvbas As Long)
  posvbas = newosvbas
End Property

Public Property Get osvoff() As Long
  osvoff = posvoff
End Property
Public Property Let osvoff(newosvoff As Long)
  posvoff = newosvoff
End Property

Public Property Get eunits() As String
  eunits = peunits
End Property
Public Property Let eunits(neweunits As String)
  peunits = neweunits
End Property

Public Property Get ltval1() As Single
  ltval1 = pltval1
End Property
Public Property Let ltval1(newltval1 As Single)
  pltval1 = newltval1
End Property

Public Property Get ltval2() As Single
  ltval2 = pltval2
End Property
Public Property Let ltval2(newltval2 As Single)
  pltval2 = newltval2
End Property

Public Property Get ltval3() As Single
  ltval3 = pltval3
End Property
Public Property Let ltval3(newltval3 As Single)
  pltval3 = newltval3
End Property

Public Property Get ltval4() As Single
  ltval4 = pltval4
End Property
Public Property Let ltval4(newltval4 As Single)
  pltval4 = newltval4
End Property

Public Property Get defn() As String
  defn = pdefn
End Property
Public Property Let defn(newdefn As String)
  pdefn = newdefn
End Property

Public Property Get munits() As String
  munits = pmunits
End Property
Public Property Let munits(newmunits As String)
  pmunits = newmunits
End Property

Public Property Get ltval5() As Single
  ltval5 = pltval5
End Property
Public Property Let ltval5(newltval5 As Single)
  pltval5 = newltval5
End Property

Public Property Get ltval6() As Single
  ltval6 = pltval6
End Property
Public Property Let ltval6(newltval6 As Single)
  pltval6 = newltval6
End Property

Public Property Get ltval7() As Single
  ltval7 = pltval7
End Property
Public Property Let ltval7(newltval7 As Single)
  pltval7 = newltval7
End Property

Public Property Get ltval8() As Single
  ltval8 = pltval8
End Property
Public Property Let ltval8(newltval8 As Single)
  pltval8 = newltval8
End Property
