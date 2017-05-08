Attribute VB_Name = "HassLibs"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'general routines
Declare Sub F90_W99OPN Lib "hass_ent.dll" ()
Declare Sub F90_W99CLO Lib "hass_ent.dll" ()
Declare Sub F90_MSG Lib "hass_ent.dll" (ByVal aMsg As String, ByVal aMsgLen As Integer)

Declare Function F90_WDMOPN Lib "hass_ent.dll" (l&, ByVal s$, ByVal i%) As Long
Declare Function F90_WDMCLO Lib "hass_ent.dll" (l&) As Long
Declare Function F90_INQNAM Lib "hass_ent.dll" (ByVal s$, ByVal i%) As Long
'util:utchar
Declare Sub F90_DATLST_XX Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_DECCHX_XX Lib "hass_ent.dll" (r!, l&, l&, l&, l&)
'util:utsort
Declare Sub F90_ASRTRP Lib "hass_ent.dll" (l&, r!)
'util:utdate
Declare Sub F90_CMPTIM Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_DATNXT Lib "hass_ent.dll" (l&, l&, l&)
Declare Function F90_DAYMON Lib "hass_ent.dll" (l&, l&) As Long
Declare Sub F90_TIMADD Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_TIMDIF Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_TIMCNV Lib "hass_ent.dll" (l&)
Declare Sub F90_TIMCVT Lib "hass_ent.dll" (l&)
Declare Sub F90_TIMBAK Lib "hass_ent.dll" (l&, l&)
Declare Function F90_TIMCHK Lib "hass_ent.dll" (l&, l&) As Long
Declare Sub F90_JDMODY Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_DTMCMN Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_DECPRC Lib "hass_ent.dll" (l&, l&, r!)
'adwdm:utwdmd
Declare Sub F90_WDBFIN Lib "hass_ent.dll" ()
Declare Function F90_WDFLCL Lib "hass_ent.dll" (l&) As Long
Declare Sub F90_WDDSNX Lib "hass_ent.dll" (l&, l&)
Declare Function F90_WDCKDT Lib "hass_ent.dll" (l&, l&) As Long
'adwdm:wdopxx
Declare Function F90_WDBOPN Lib "hass_ent.dll" (l&, ByVal s$, ByVal i%) As Long
Declare Sub F90_WDBOPNR Lib "hass_ent.dll" (l&, ByVal s$, l&, l&, ByVal i%)
'adwdm:ztwdmf
Declare Sub F90_WMSGTX_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, r!, r!, r!, l&, l&, l&)
Declare Sub F90_WMSGTW_XX Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_WMSGTT_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_WMSGTH Lib "hass_ent.dll" (l&, l&)
'adwdm:wdmess
Declare Sub F90_WDLBAX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_WDDSDL Lib "hass_ent.dll" (l&, l&, l&)
Declare Sub F90_GETATT Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
'wdm:wdtms1
Declare Sub F90_WDTGET Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, r!, l&)
Declare Sub F90_WDTPUT Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, r!, l&)
'wdm:wdtms2
Declare Sub F90_WTFNDT Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&)
'wdm:wdbtch
Declare Sub F90_WDBSGC_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_WDBSGI Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_WDBSGR Lib "hass_ent.dll" (l&, l&, l&, l&, r!, l&)
Declare Sub F90_WDLBAD Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_WDDSCL Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_WDDSRN Lib "hass_ent.dll" (l&, l&, l&, l&)
'wdm:wdatrb
Declare Sub F90_WDBSAC Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_WDBSAI Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_WDBSAR Lib "hass_ent.dll" (l&, l&, l&, l&, l&, r!, l&)
Declare Sub F90_WDSAGY_XX Lib "hass_ent.dll" (l&, l&, l&, l&, r!, r!, r!, l&, l&, l&, l&, l&, l&, l&)
'wdm:wddlg
Declare Sub F90_WDLGET Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, r!, l&)
Declare Sub F90_WDLLSU Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&)
'wdm:wdmid
Declare Sub F90_WDIINI Lib "hass_ent.dll" ()
Declare Sub F90_WIDADD Lib "hass_ent.dll" (l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_WUA2ID Lib "hass_ent.dll" (l&, l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_WID2UD Lib "hass_ent.dll" (l&, l&, l&, l&)
'graph:grutil
Declare Sub F90_SCALIT Lib "hass_ent.dll" (l&, r!, r!, r!, r!)
'newaqt:utdir
Declare Sub F90_TSDRRE Lib "hass_ent.dll" (l&, l&, l&)
Declare Sub F90_TSDSPC_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_TSDSIN Lib "hass_ent.dll" (l&, ByVal s$, ByVal i%)
Declare Sub F90_TSDSCT Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_TSDSUN_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_TSESPC Lib "hass_ent.dll" (ByVal s$, ByVal s$, ByVal s$, ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_TSDSM Lib "hass_ent.dll" (l&)
Declare Sub F90_TSAPUT Lib "hass_ent.dll" (l&, ByVal s$, ByVal s$, ByVal s$, ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_TSDSGN Lib "hass_ent.dll" (l&, l&)
'newaqt:scnutl
Declare Sub F90_DELSCN Lib "hass_ent.dll" (ByVal s$, ByVal i%)
Declare Sub F90_COPSCN Lib "hass_ent.dll" (l&, l&, ByVal s$, ByVal s$, ByVal i%, ByVal i%)
Declare Sub F90_NEWFIL Lib "hass_ent.dll" (ByVal s$, ByVal s$, ByVal i%, ByVal i%)
Declare Sub F90_NEWDSN Lib "hass_ent.dll" (l&, l&, l&)
Declare Sub F90_FILSET Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_UCISAV Lib "hass_ent.dll" ()
'newaqt:umaker
Declare Sub F90_UMAKPR Lib "hass_ent.dll" (l&, l&, l&, l&, _
                                           l&, l&, l&, l&, l&, l&, _
                                           ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%, ByVal i%, _
                                           ByVal i%, ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_UMAKDO Lib "hass_ent.dll" (l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_UCGNRC Lib "hass_ent.dll" (l&)
Declare Sub F90_UCGNCO Lib "hass_ent.dll" (l&)
Declare Sub F90_UCGNLA Lib "hass_ent.dll" (l&)
Declare Sub F90_UCGNEX Lib "hass_ent.dll" (l&)
Declare Sub F90_UCGNME Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_UCGOUT Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_UCGIRC_XX Lib "hass_ent.dll" (l&, l&)
'newaqt:watinp
Declare Sub F90_WATINI Lib "hass_ent.dll" (ByVal s$, l&, ByVal i%)
Declare Sub F90_WATHED_XX Lib "hass_ent.dll" (l&, l&, l&, r!, l&, l&, l&, l&)
Declare Sub F90_WATINP Lib "hass_ent.dll" (l&, l&, l&, l&, l&, r!, ByVal s$, ByVal s$, ByVal s$, ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%, ByVal i%, ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_WATCLO Lib "hass_ent.dll" (l&)
'newaqt:stspec
Declare Sub F90_STSPGG_XX Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_STSPGU_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, r!, r!, r!, l&)
Declare Sub F90_STSPFN_XX Lib "hass_ent.dll" (l&, l&, l&, r!, l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_STSPPN Lib "hass_ent.dll" (l&, r!, l&, l&, ByVal s$, ByVal i%)
'newaqt:simnet
Declare Sub F90_SIMNET Lib "hass_ent.dll" (l&, r!, r!, l&, _
                                           ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%, ByVal i%, _
                                           ByVal i%, ByVal i%, ByVal i%)
'newaqt:durbat
Declare Sub F90_DAANST Lib "hass_ent.dll" (l&, r!)
Declare Sub F90_DAANWV Lib "hass_ent.dll" (l&, l&, r!, l&, l&, l&, _
                                           l&, l&, r!, l&, l&, l&, _
                                           l&, l&, l&, l&, l&, _
                                           l&, l&, l&, r!, r!, r!, _
                                           r!, r!, r!, r!, r!, r!, _
                                           r!, r!, r!, r!, r!, r!, _
                                           r!, r!, r!, r!, r!, r!, _
                                           r!, r!, r!, r!, r!, l&, _
                                           ByVal s$, ByVal s$, ByVal i%, ByVal i%)
'newaqt:ucirept
Declare Function F90_REPEXT Lib "hass_ent.dll" (l&) As Long
Declare Sub F90_ADDREPT Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_DELREPT Lib "hass_ent.dll" (l&)
Declare Sub F90_ADDBMP Lib "hass_ent.dll" (l&, l&)
'vb_scen
Declare Sub F90_SPIPH Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_SCNDBG Lib "hass_ent.dll" (l&)
Declare Sub F90_ACTSCN Lib "hass_ent.dll" (l&, l&, l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_GLOPRMI Lib "hass_ent.dll" (l&, ByVal s$, ByVal i%)
Declare Sub F90_GLOBLK_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_PUTGLO Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_SIMSCN Lib "hass_ent.dll" (l&)
Declare Sub F90_GTNXKW_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_XTABLE_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_XTABLEEX_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_XBLOCK_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_XBLOCKEX_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_XTINFO_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, r!, r!, r!, l&, l&, l&, l&, l&, l&)
Declare Sub F90_REPUCI Lib "hass_ent.dll" (l&, ByVal s$, ByVal i%)
Declare Sub F90_DELUCI Lib "hass_ent.dll" (l&)
Declare Sub F90_PUTUCI Lib "hass_ent.dll" (l&, l&, ByVal s$, ByVal i%)
Declare Sub F90_HGETI Lib "hass_ent.dll" (ByVal s$, l&, l&, ByVal i%)
Declare Sub F90_HGETC_XX Lib "hass_ent.dll" (ByVal s$, l&, l&, ByVal i%)
Declare Sub F90_HPUTC Lib "hass_ent.dll" (ByVal s$, l&, ByVal s$, ByVal i%, ByVal i%)
Declare Sub F90_GTINS_XX Lib "hass_ent.dll" (l&, l&, l&, l&, r!)
Declare Sub F90_PBMPAR Lib "hass_ent.dll" (ByVal s$, l&, r!, ByVal s$, l&, l&, ByVal i%, ByVal i%)
Declare Sub F90_DELBMP Lib "hass_ent.dll" (l&)
Declare Sub F90_GETOCR Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_FILSTA Lib "hass_ent.dll" (ByVal s$, ByVal i%)
'newaqt:sglabl
Declare Sub F90_SGLABL_XX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_FITLIN Lib "hass_ent.dll" (l&, l&, r!, r!, r!, r!)
Declare Sub F90_CMSTRM Lib "hass_ent.dll" (l&, l&, l&, l&, r!, l&, r!, l&, l&, l&, l&, l&, l&, l&, r!, r!, r!)
'iowdm:tsflat
Declare Sub F90_TSFLAT Lib "hass_ent.dll" (l&, l&, ByVal s$, l&, ByVal s$, l&, l&, l&, l&, l&, r!, l&, l&, l&, ByVal i%, ByVal i%)
'iowdm:inwbat
Declare Sub F90_INFREE Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
'tree (no longer used, taken out of hass_ent.dll)
Declare Sub F90_TREE_BLD Lib "hass_ent.dll" (l&, l&, l&, l&)
Declare Sub F90_TREE_SUM Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
Declare Sub F90_TREE_SET_NAME Lib "hass_ent.dll" (ByVal s$, ByVal i%)
Declare Sub F90_DISP_LIS Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&, l&, l&)
Declare Sub F90_TREE_END Lib "hass_ent.dll" ()
Declare Sub F90_FILT_MOD Lib "hass_ent.dll" (l&, l&)
Declare Sub F90_FILT_LIS Lib "hass_ent.dll" (l&, l&, l&, l&, l&, l&)
Declare Sub F90_TREE_ROOT Lib "hass_ent.dll" (l&)
Declare Sub F90_BRAN_GET_PARM Lib "hass_ent.dll" (l&, l&, l&, l&, l&)
'debug
Declare Sub F90_PUTOLV Lib "hass_ent.dll" (l&)
Declare Sub F90_MSGUNIT Lib "hass_ent.dll" (l&)
'awstat:tscbat
Declare Sub F90_TSCBAT Lib "hass_ent.dll" (l&, r!, l&, r!, r!, _
                                           l&, l&, l&, l&, l&, _
                                           l&, l&, l&, l&, l&, l&, r!, r!, _
                                           r!, r!, r!, r!, r!, l&, _
                                           r!, r!, _
                                           ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%)
'ann:pgebat
Declare Sub F90_GNMEXE Lib "hass_ent.dll" (l&, l&, l&, l&, l&, _
                                           l&, l&, l&, r!, l&, _
                                           l&, l&, ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_GMANEX Lib "hass_ent.dll" (l&, l&, l&, l&, l&, _
                                           l&, l&, l&, r!, l&, _
                                           l&, l&, ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%)
Declare Sub F90_GNTTRN Lib "hass_ent.dll" (l&, l&, l&, l&, _
                                           l&, l&, l&, l&, _
                                           l&, l&, l&, l&, _
                                           ByVal s$, ByVal s$, ByVal s$, _
                                           ByVal i%, ByVal i%, ByVal i%)
'hspf:hiouci
Declare Sub F90_GETUCI_XX Lib "hass_ent.dll" (l&, l&, l&)

Declare Sub F90_SET_DRIVER Lib "hass_ent.dll" (l&, r!, ByVal s$, ByVal i%)
Declare Sub F90_SENDSTRING Lib "hass_ent.dll" (ByVal s$, ByVal i%)

'feq
'Declare Sub F90_INVMJD Lib "feqlib.dll" (L&, L&, L&, L&)
'Declare Sub F90_TABTYP Lib "feqlib.dll" (L&, L&)
'Declare Sub F90_FILTAB Lib "feqlib.dll" (ByVal s$, L&, L&, L&, ByVal i%)
'Declare Sub F90_XLKT20 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!)
'Declare Sub F90_XLKT21 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!, r!)
'Declare Sub F90_XLKT22 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
'Declare Sub F90_XLKT23 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
'Declare Sub F90_XLKT24 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
'Declare Sub F90_XLKT25 Lib "feqlib.dll" (L&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)


Private Sub ChrNum(ilen&, istr$, onam&())
    Dim i%
    For i = 1 To ilen
      If i <= Len(istr) Then
        onam(i - 1) = asc(Mid(istr, i, 1))
      Else
        onam(i - 1) = 32
      End If
    Next i
End Sub

Public Sub F90_GTNXKW(Init&, id&, ckwd$, kwdfg&, contfg&, retid&)
    Dim ikwd&(12)
    
    Call F90_GTNXKW_XX(Init&, id&, ikwd(0), kwdfg&, contfg&, retid&)
    Call NumChr(12, ikwd, ckwd)
    
End Sub

Public Sub F90_XTABLE(omcode&, tabno&, uunits&, Init&, addfg&, Occur&, _
                      retkey&, cbuff$, retcod&)
    Dim ibuff&(80)
    
    Call F90_XTABLE_XX(omcode&, tabno&, uunits&, Init&, addfg&, Occur&, _
                       retkey&, ibuff(0), retcod&)
    Call NumChr(80, ibuff, cbuff)
    
End Sub

Public Sub F90_XTABLEEX(omcode&, tabno&, uunits&, Init&, addfg&, Occur&, _
                        retkey&, cbuff$, rectyp&, retcod&)
    Dim ibuff&(80)
    
    Call F90_XTABLEEX_XX(omcode&, tabno&, uunits&, Init&, addfg&, Occur&, _
                         retkey&, ibuff(0), rectyp&, retcod&)
    Call NumChr(80, ibuff, cbuff)
    
End Sub

Public Sub F90_XBLOCK(blkno&, Init&, retkey&, cbuff$, retcod&)
    Dim ibuff&(80)
    
    Call F90_XBLOCK_XX(blkno&, Init&, retkey&, ibuff(0), retcod&)
    Call NumChr(80, ibuff, cbuff)
    
End Sub

Public Sub F90_XBLOCKEX(blkno&, Init&, retkey&, cbuff$, rectyp&, retcod&)
    Dim ibuff&(80)
    
    Call F90_XBLOCKEX_XX(blkno&, Init&, retkey&, ibuff(0), rectyp&, retcod&)
    Call NumChr(80, ibuff, cbuff)
    
End Sub

Public Sub F90_WATHED(messunit&, inwat&, iVal&(), rval!(), csit$, ctyp$, cid$, cname$)
    Dim id&(8), Name&(48), isit&(2), ityp&(4)
    
    Call F90_WATHED_XX(messunit, inwat, iVal(0), rval(0), isit&(0), ityp&(0), id&(0), Name&(0))
    Call NumChr(2, isit, csit)
    Call NumChr(4, ityp, ctyp)
    Call NumChr(8, id, cid)
    Call NumChr(48, Name, cname)
End Sub

Public Sub F90_UCGIRC(ir&, cname$)
    Dim Name&(12)
    
    Call F90_UCGIRC_XX(ir, Name&(0))
    Call NumChr(12, Name, cname)
End Sub

Public Sub F90_STSPGU(initfg&, retcod&, uvquan$, cgroup$, uvdesc$, uvname$, _
                      umin!, umax!, udef!, intfg&)
    Dim iname&(6), idesc&(64), ivname&(6), igroup&(6)
    
    Call F90_STSPGU_XX(initfg, retcod, iname(0), igroup(0), idesc(0), ivname(0), _
                       umin, umax, udef, intfg)
    Call NumChr(6, iname, uvquan)
    Call NumChr(64, idesc, uvdesc)
    Call NumChr(6, ivname, uvname)
    Call NumChr(6, igroup, cgroup)
End Sub

Public Sub F90_STSPGG(initfg&, retcod&, gname$, gdesc$)
    Dim iname&(6), idesc&(64)
    
    Call F90_STSPGG_XX(initfg, retcod, iname(0), idesc(0))
    Call NumChr(6, iname, gname)
    Call NumChr(64, idesc, gdesc)
End Sub

Public Sub F90_STSPFN(uvname$, intfg&, ucikey&, idat&(), _
                      aval!, ac$, cond$)
     
    Dim iac&(3), icond&(64)
    
    Call F90_STSPFN_XX(intfg, ucikey, idat(0), _
                       aval!, iac(0), icond(0), uvname, Len(uvname))
    Call NumChr(3, iac, ac)
    Call NumChr(64, icond, cond)
End Sub


Public Sub F90_GLOBLK(sdatim&(), edatim&(), outlev&, spout&, runfg&, emfg&, rninfo$)
    Dim info&(80)
    
    Call F90_GLOBLK_XX(sdatim&(0), edatim&(0), outlev&, spout&, runfg&, emfg&, info&(0))
    Call NumChr(80, info, rninfo)
End Sub

Private Sub NumChr(ilen&, inam&(), outstr$)

    Dim i&
    
    outstr = ""
    For i = 0 To ilen - 1 'added "- 1" 8/16/2002 Mark Gray
      If inam(i) > 0 Then
        outstr = outstr & Chr(inam(i))
      End If
    Next i
    outstr = RTrim(outstr)

End Sub
Private Sub NumChrA(icnt&, ilen&, inam() As Long, outstr() As String)

    Dim i&, j&, s$, p&, jnam&()
 
    ReDim jnam(ilen)
    p = 0
    For i = 0 To icnt - 1
      For j = 0 To ilen - 1
        jnam(j) = inam(p + j)
      Next j
      Call NumChr(ilen, jnam, s)
      outstr(i) = s
      p = p + ilen
   Next i

End Sub


Public Sub F90_DATLST(CurDat&(), DatStr$)
   Dim l&, e&, i&(21)
   
   Call F90_DATLST_XX(CurDat(0), i(0), l, e)
   Call NumChr(l, i, DatStr)

End Sub


Public Sub F90_DECCHX(reain!, ilen&, sigdig&, decpla&, RStr$)

   Dim i&()
   ReDim i(ilen)

   Call F90_DECCHX_XX(reain, ilen, sigdig, decpla, i(0))
   Call NumChr(ilen, i, RStr)

End Sub
Public Sub F90_WDBSGC(w&, d&, i&, l&, s$)
    Dim iVal(48) As Long

    Call F90_WDBSGC_XX(w&, d&, i&, l&, iVal(0))
    Call NumChr(l, iVal, s)

End Sub

Public Sub F90_TSDSPC(dsn&, LSCENM$, LRCHNM$, LCONNM$, Tu&, ts&, SDate&(), EDate&(), GRPSIZ&)

    Dim Iscenm&(8), IRCHNM&(8), Iconnm&(8)

    Call F90_TSDSPC_XX(dsn&, Iscenm(0), IRCHNM(0), Iconnm(0), Tu&, ts&, SDate&(0), EDate&(0), GRPSIZ&)
    Call NumChr(8, Iscenm, LSCENM)
    Call NumChr(8, IRCHNM, LRCHNM)
    Call NumChr(8, Iconnm, LCONNM)

End Sub

Sub F90_TSDSUN(cntloc&, cntsen&, cntcon&, LunLoc$(), LunSen$(), LunCon$())

    Dim IunLoc&(), IunSen&(), IunCon&(), slen As Long

    ReDim IunLoc(8 * cntloc)
    ReDim IunSen(8 * cntsen)
    ReDim IunCon(8 * cntcon)
    Call F90_TSDSUN_XX(cntloc, cntsen, cntcon, IunLoc(0), IunSen(0), IunCon(0))
    slen = 8
    Call NumChrA(cntloc, slen, IunLoc(), LunLoc())
    Call NumChrA(cntsen, slen, IunSen(), LunSen())
    Call NumChrA(cntcon, slen, IunCon(), LunCon())

End Sub

Sub F90_SGLABL(ndsn&, Csennm$(), clocnm$(), cconnm$(), Tu&, Dtran&, which&(), typind&(), cntcon&, cntsen&, cntloc&, Calab$, Cyrlab$, Cyllab$, Ctitl$, clab$(), ctran$, ctunit$)
    Dim i, j, Isennm&(), Ilocnm&(), Iconnm&(), Ialab&(80), Iyrlab&(80), Iyllab&(80), Ititl&(240), Ilab&(), Itran&(8), Itunit&(8)
    Dim tsen&(8), tloc&(8), tcon&(8)
    
    ReDim Ilab(20 * ndsn)
    ReDim Isennm(8 * ndsn)
    ReDim Ilocnm(8 * ndsn)
    ReDim Iconnm(8 * ndsn)
      
    For i = 0 To ndsn - 1
        Call ChrNum(8, Csennm(i), tsen())
        Call ChrNum(8, clocnm(i), tloc())
        Call ChrNum(8, cconnm(i), tcon())
        For j = 0 To 7
          Isennm((i * 8) + j) = tsen(j)
          Ilocnm((i * 8) + j) = tloc(j)
          Iconnm((i * 8) + j) = tcon(j)
        Next j
    Next i
    
    Call F90_SGLABL_XX(ndsn, Isennm(0), Ilocnm(0), Iconnm(0), Tu, Dtran, which(0), typind(0), cntcon, cntsen, cntloc, Ialab(0), Iyrlab(0), Iyllab(0), Ititl(0), Ilab(0), Itran(0), Itunit(0))
    
    Call NumChr(80, Ialab, Calab)
    Call NumChr(80, Iyrlab, Cyrlab)
    Call NumChr(80, Iyllab, Cyllab)
    Call NumChr(240, Ititl, Ctitl)
    Call NumChr(8, Itran, ctran)
    Call NumChr(8, Itunit, ctunit)
    Call NumChrA(ndsn, 20, Ilab, clab)
End Sub
Sub F90_WMSGTX(messfl&, SCLU&, SGRP&, lnflds&, lscol&(), lflen&(), lftyp$(), lapos&(), _
               limin&(), limax&(), lidef&(), lrmin!(), lrmax!(), lrdef!(), _
               lnmhdr&, hdrbuf$(), retcod&)
    Dim ihdrbuf&(780), ilftyp&(30)
    
    Call F90_WMSGTX_XX(messfl, SCLU, SGRP, lnflds, lscol(0), lflen(0), ilftyp(0), lapos(0), _
                       limin(0), limax(0), lidef(0), lrmin(0), lrmax(0), lrdef(0), _
                       lnmhdr, ihdrbuf(0), retcod)
                    
    Call NumChr(30, ilftyp, lftyp(0))
    Call NumChrA(10, 78, ihdrbuf, hdrbuf)
    
End Sub

Sub F90_WMSGTW(id&, s$)
   Dim i&(48)
   
   Call F90_WMSGTW_XX(id, i(0))
   Call NumChr(48, i, s)
   
End Sub
Sub F90_WMSGTT(wdmsfl&, dsn&, gnum&, initfg&, olen&, cont&, obuff$)
    Dim lobuff&(256)
    
    Call F90_WMSGTT_XX(wdmsfl, dsn, gnum, initfg, olen, cont, lobuff(0))
    
    Call NumChr(olen, lobuff, obuff)
End Sub

Sub F90_XTINFO(omcode&, tnum&, uunits&, estflg&, lnflds&, lscol&(), lflen&(), lftyp$, lapos&(), _
               limin&(), limax&(), lidef&(), lrmin!(), lrmax!(), lrdef!(), _
               lnmhdr&, hdrbuf$(), lfdnam$(), isect&, irept&, retcod&)
    Dim ihdrbuf&(780), ilftyp&(30), ifdnam&(360)
    
    Call F90_XTINFO_XX(omcode, tnum, uunits, estflg, lnflds, lscol(0), lflen(0), ilftyp(0), lapos(0), _
                       limin(0), limax(0), lidef(0), lrmin(0), lrmax(0), lrdef(0), _
                       lnmhdr, ihdrbuf(0), ifdnam(0), isect, irept, retcod)
                    
    Call NumChr(30, ilftyp, lftyp)
    Call NumChrA(10, 78, ihdrbuf, hdrbuf)
    Call NumChrA(30, 12, ifdnam, lfdnam)
    
End Sub
    
Sub F90_HGETC(itmnam$, idno&, ctxt$, l%)
   Dim itxt&(80)
   
   Call F90_HGETC_XX(itmnam, idno, itxt(0), l)
   Call NumChr(80, itxt, ctxt)
   
End Sub

Sub F90_GTINS(Init&, idno&, rorb&, ctxt$, rarea!)
   Dim itxt&(10)
   
   Call F90_GTINS_XX(Init, idno, rorb, itxt(0), rarea)
   Call NumChr(10, itxt, ctxt)
   
End Sub

Sub F90_WDSAGY(wdmsfl&, id&, ilen&, itype&, rmin!, rmax!, rdef!, hlen&, hrec&, hpos&, vlen&, Name$, desc$, valid$)
   Dim itxt&(6)
   Dim idesc&(47), ivalid&(240)
   
   Call F90_WDSAGY_XX(wdmsfl, id, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, itxt(0), idesc(0), ivalid(0))
   Call NumChr(6, itxt, Name)
   Call NumChr(47, idesc, desc)
   Call NumChr(vlen, ivalid, valid)
   
End Sub

Sub F90_GETUCI(rectyp&, Key&, _
               ucibuf$)
    Dim iucibf&(80)
    
    Call F90_GETUCI_XX(rectyp, Key, _
                       iucibf(0))
                    
    Call NumChr(80, iucibf, ucibuf)
End Sub

