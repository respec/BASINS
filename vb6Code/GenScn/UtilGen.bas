Attribute VB_Name = "UtilGen"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Sub HspfTableTest()
    If p.HSPFMsg.Unit > 0 Then
      Dim lnflds&, lscol&(30), lflen&(30), lftyp$, lapos&(30)
      Dim limin&(30), limax&(30), lidef&(30)
      Dim lrmin!(30), lrmax!(30), lrdef!(30), isect&, irept&
      Dim lnmhdr&, hdrbuf$(5), lfdnam$(30), retcod&
      Call F90_XTINFO(1, 1, 1, 1, _
        lnflds, lscol, lflen, lftyp, lapos, _
        limin, limax, lidef, lrmin, lrmax, lrdef, _
        lnmhdr, hdrbuf, lfdnam, isect, irept, retcod)
    Else
      MsgBox "open a project with a hspfmsg file"
    End If
End Sub
