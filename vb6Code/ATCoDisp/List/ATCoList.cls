VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoList"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

Public Event DataChanged(inum&)

Public Sub RaiseDataChanged(inum&)
  RaiseEvent DataChanged(inum)
End Sub

Public Function Count&()
'Attribute Count.VB_Description = "returns count of HLists opened by calling application. "

    Dim i&, j&

    i = Forms.Count
    Count = 0
    For j = 0 To NumList - 1
      If l(j).Tag >= 0 Then
        Count = Count + 1
      End If
    Next j

End Function


Public Sub GetDateFmt(ifmt&)

    Call frmL.GetDateFmt(ifmt)

End Sub

Public Sub SetDateFmt(ifmt&)

    Call frmL.SetDateFmt(ifmt)

End Sub


Public Sub GetFldPrms(cfld&, wid&, sdg&, dpl&)

    Call frmL.GetFldPrms(cfld, wid, sdg, dpl)

End Sub

Public Sub init()
'Attribute Init.VB_Description = "initializes a new HList"
    
    Call frmL.init
    Set frmL.parentlst = Me

End Sub

Public Sub SetEditable(inum&, Editable As Boolean)
  Call frmL.SetEditable(inum, Editable)
End Sub

Public Sub SetData(inum&, ipos&, nv&, arra#(), retcod&)
'Attribute SetData.VB_Description = "puts data values to list into a HList"

    Call frmL.SetData(inum, ipos, nv, arra, retcod)

End Sub

Public Sub GetData(inum&, arra#())

    Call frmL.GetData(inum, arra)

End Sub

Public Function GetVarLabel(vind&)

    GetVarLabel = frmL.GetVarLabel(vind)

End Function

Public Sub SetCurDrvDir(V$, d$)

  Call frmL.SetCurDrvDir(V$, d$)

End Sub

Public Sub SetFldRange(cfld&, lmin!, lmax!, linran As Boolean)

    Call frmL.SetFldRange(cfld, lmin, lmax, linran)

End Sub

Public Sub SetIcon(ic As Object)
  DbgMsg "SetIcon: icon=" & ic
  Call frmL.SetIcon(ic)
End Sub


Public Sub SetListType(ltyp&)

    Call frmL.SetListType(ltyp)

End Sub

Public Sub SetVarLabel(vind&, vlab$)

    Call frmL.SetVarLabel(vind, vlab)

End Sub

Public Sub GetTitles(titl$, capt$)

    Call frmL.GetTitles(titl, capt$)

End Sub
Public Sub SetTitles(titl$, capt$)

    Call frmL.SetTitles(titl, capt$)

End Sub

Public Sub GetNumVars(inlis&, invar&)
'Attribute GetNumVars.VB_Description = "sets number of listings for a HList"

    Call frmL.GetNumVars(inlis, invar)

End Sub
Public Sub SetNumVars(inlis&, invar&)

    Call frmL.SetNumVars(inlis, invar)

End Sub

Public Sub SetFldPrms(cfld&, wid&, sdg&, dpl&)

    Call frmL.SetFldPrms(cfld, wid, sdg, dpl)

End Sub
Public Sub GetSums(nsums&, sumtype&(), sumint&(), yrend&, mxsumcnt&)

    Call frmL.GetSums(nsums, sumtype(), sumint(), yrend, mxsumcnt)

End Sub
Public Sub SetSums(nsums&, sumtype&(), sumint&(), yrend&, mxsumcnt&)

    Call frmL.SetSums(nsums, sumtype(), sumint(), yrend, mxsumcnt)

End Sub

Public Sub GetTime(ts&(), tc&(), sdt&(), edt&(), dtype&())

    Call frmL.GetTime(ts, tc, sdt, edt, dtype)

End Sub
Public Sub SetTime(ts&(), tc&(), sdt&(), edt&(), dtype&())

    Call frmL.SetTime(ts, tc, sdt, edt, dtype)

End Sub

Public Sub SetVars(icrv&, y&, x&)
'Attribute SetVars.VB_Description = "sets which x and y data values are associated with a listing on a HList"

    Call frmL.SetVars(icrv, y, x)

End Sub

Public Sub ShowIt(Optional modal As Boolean = False)
'Attribute ShowIt.VB_Description = "draws a new HList - called after all data has been set"
  DbgMsg "ShowIt " & modal
  frmL.ShowIt modal
End Sub

Private Sub Class_Initialize()
   Debug.Print "List Class_Initialize"
    'Open "junk.out" For Output As #2
End Sub

Private Sub DbgMsg(Msg$)
  If Not (pIPC Is Nothing) Then pIPC.dbg "ATCoList:" & Msg
End Sub

Public Property Let HelpFileName(FileName$)
  GraphHelpFileName = FileName
  App.HelpFile = FileName
End Property


