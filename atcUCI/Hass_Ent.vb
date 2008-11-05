Option Strict Off
Option Explicit On
''' <summary>
''' Entry points into hass_ent.dll
''' </summary>
''' <remarks>
''' Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
''' </remarks>
Module HassLibs
    'general routines
    Declare Sub F90_W99OPN Lib "hass_ent.dll" ()
    Declare Sub F90_W99CLO Lib "hass_ent.dll" ()
    Declare Sub F90_MSG Lib "hass_ent.dll" (ByVal aMsg As String, ByVal aMsgLen As Short)

    Declare Function F90_WDMOPN Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short) As Integer
    Declare Function F90_WDMCLO Lib "hass_ent.dll" (ByRef l As Integer) As Integer
    Declare Function F90_INQNAM Lib "hass_ent.dll" (ByVal s As String, ByVal i As Short) As Integer
    'util:utchar
    Declare Sub F90_DATLST_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_DECCHX_XX Lib "hass_ent.dll" (ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'util:utsort
    Declare Sub F90_ASRTRP Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single)
    'util:utdate
    Declare Sub F90_CMPTIM Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_DATNXT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Function F90_DAYMON Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer) As Integer
    Declare Sub F90_TIMADD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TIMDIF Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TIMCNV Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_TIMCVT Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_TIMBAK Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Function F90_TIMCHK Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer) As Integer
    Declare Sub F90_JDMODY Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_DTMCMN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_DECPRC Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef r As Single)
    'adwdm:utwdmd
    Declare Sub F90_WDBFIN Lib "hass_ent.dll" ()
    Declare Function F90_WDFLCL Lib "hass_ent.dll" (ByRef l As Integer) As Integer
    Declare Sub F90_WDDSNX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Function F90_WDCKDT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer) As Integer
    'adwdm:wdopxx
    Declare Function F90_WDBOPN Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short) As Integer
    Declare Sub F90_WDBOPNR Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByVal i As Short)
    'adwdm:ztwdmf
    Declare Sub F90_WMSGTX_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WMSGTW_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WMSGTT_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WMSGTH Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    'adwdm:wdmess
    Declare Sub F90_WDLBAX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDDSDL Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_GETATT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'wdm:wdtms1
    Declare Sub F90_WDTGET Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer)
    Declare Sub F90_WDTPUT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer)
    'wdm:wdtms2
    Declare Sub F90_WTFNDT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'wdm:wdbtch
    Declare Sub F90_WDBSGC_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDBSGI Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDBSGR Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer)
    Declare Sub F90_WDLBAD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDDSCL Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDDSRN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'wdm:wdatrb
    Declare Sub F90_WDBSAC Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_WDBSAI Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WDBSAR Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer)
    Declare Sub F90_WDSAGY_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'wdm:wddlg
    Declare Sub F90_WDLGET Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer)
    Declare Sub F90_WDLLSU Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'wdm:wdmid
    Declare Sub F90_WDIINI Lib "hass_ent.dll" ()
    Declare Sub F90_WIDADD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_WUA2ID Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_WID2UD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'graph:grutil
    Declare Sub F90_SCALIT Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    'newaqt:utdir
    Declare Sub F90_TSDRRE Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TSDSPC_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TSDSIN Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_TSDSCT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TSDSUN_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TSESPC Lib "hass_ent.dll" (ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_TSDSM Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_TSAPUT Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_TSDSGN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    'newaqt:scnutl
    Declare Sub F90_DELSCN Lib "hass_ent.dll" (ByVal s As String, ByVal i As Short)
    Declare Sub F90_COPSCN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_NEWFIL Lib "hass_ent.dll" (ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_NEWDSN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_FILSET Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_UCISAV Lib "hass_ent.dll" ()
    'newaqt:umaker
    Declare Sub F90_UMAKPR Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_UMAKDO Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_UCGNRC Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_UCGNCO Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_UCGNLA Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_UCGNEX Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_UCGNME Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_UCGOUT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_UCGIRC_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    'newaqt:watinp
    Declare Sub F90_WATINI Lib "hass_ent.dll" (ByVal s As String, ByRef l As Integer, ByVal i As Short)
    Declare Sub F90_WATHED_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_WATINP Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_WATCLO Lib "hass_ent.dll" (ByRef l As Integer)
    'newaqt:stspec
    Declare Sub F90_STSPGG_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_STSPGU_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer)
    Declare Sub F90_STSPFN_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_STSPPN Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    'newaqt:simnet
    Declare Sub F90_SIMNET Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    'newaqt:durbat
    Declare Sub F90_DAANST Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single)
    Declare Sub F90_DAANWV Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short)
    'newaqt:ucirept
    Declare Function F90_REPEXT Lib "hass_ent.dll" (ByRef l As Integer) As Integer
    Declare Sub F90_ADDREPT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_DELREPT Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_ADDBMP Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    'vb_scen
    Declare Sub F90_SPIPH Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_SCNDBG Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_ACTSCN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_GLOPRMI Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_GLOBLK_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_PUTGLO Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_SIMSCN Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_GTNXKW_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XTABLE_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XTABLEEX_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XBLOCK_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XBLOCKEX_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_XTINFO_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_REPUCI Lib "hass_ent.dll" (ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_DELUCI Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_PUTUCI Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal i As Short)
    Declare Sub F90_HGETI Lib "hass_ent.dll" (ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByVal i As Short)
    Declare Sub F90_HGETC_XX Lib "hass_ent.dll" (ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByVal i As Short)
    Declare Sub F90_HPUTC Lib "hass_ent.dll" (ByVal s As String, ByRef l As Integer, ByVal s As String, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_GTINS_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single)
    Declare Sub F90_PBMPAR Lib "hass_ent.dll" (ByVal s As String, ByRef l As Integer, ByRef r As Single, ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_DELBMP Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_GETOCR Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_FILSTA Lib "hass_ent.dll" (ByVal s As String, ByVal i As Short)
    'newaqt:sglabl
    Declare Sub F90_SGLABL_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_FITLIN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_CMSTRM Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    'iowdm:tsflat
    Declare Sub F90_TSFLAT Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByRef l As Integer, ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal i As Short, ByVal i As Short)
    'iowdm:inwbat
    Declare Sub F90_INFREE Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'tree (no longer used, taken out of hass_ent.dll)
    Declare Sub F90_TREE_BLD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TREE_SUM Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TREE_SET_NAME Lib "hass_ent.dll" (ByVal s As String, ByVal i As Short)
    Declare Sub F90_DISP_LIS Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TREE_END Lib "hass_ent.dll" ()
    Declare Sub F90_FILT_MOD Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_FILT_LIS Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    Declare Sub F90_TREE_ROOT Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_BRAN_GET_PARM Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)
    'debug
    Declare Sub F90_PUTOLV Lib "hass_ent.dll" (ByRef l As Integer)
    Declare Sub F90_MSGUNIT Lib "hass_ent.dll" (ByRef l As Integer)
    'awstat:tscbat
    Declare Sub F90_TSCBAT Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    'ann:pgebat
    Declare Sub F90_GNMEXE Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_GMANEX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    Declare Sub F90_GNTTRN Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal s As String, ByVal s As String, ByVal s As String, ByVal i As Short, ByVal i As Short, ByVal i As Short)
    'hspf:hiouci
    Declare Sub F90_GETUCI_XX Lib "hass_ent.dll" (ByRef l As Integer, ByRef l As Integer, ByRef l As Integer)

    Declare Sub F90_SET_DRIVER Lib "hass_ent.dll" (ByRef l As Integer, ByRef r As Single, ByVal s As String, ByVal i As Short)
    Declare Sub F90_SENDSTRING Lib "hass_ent.dll" (ByVal s As String, ByVal i As Short)

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


    Private Sub ChrNum(ByRef aLength As Integer, ByRef aString As String, ByRef aStrAsInt() As Integer)
        For lPos As Integer = 1 To aLength
            If lPos <= aString.Length Then
                aStrAsInt(lPos - 1) = Asc(Mid(aString, lPos, 1))
            Else
                aStrAsInt(lPos - 1) = 32
            End If
        Next lPos
    End Sub

    Public Sub F90_GTNXKW(ByRef Init As Integer, ByRef id As Integer, ByRef ckwd As String, ByRef kwdfg As Integer, ByRef contfg As Integer, ByRef retid As Integer)
        Dim ikwd(12) As Integer

        Call F90_GTNXKW_XX(Init, id, ikwd(0), kwdfg, contfg, retid)
        Call NumChr(12, ikwd, ckwd)

    End Sub

    Public Sub F90_XTABLE(ByRef omcode As Integer, ByRef tabno As Integer, ByRef uunits As Integer, ByRef Init As Integer, ByRef addfg As Integer, ByRef Occur As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef retcod As Integer)
        Dim ibuff(80) As Integer

        Call F90_XTABLE_XX(omcode, tabno, uunits, Init, addfg, Occur, retkey, ibuff(0), retcod)
        Call NumChr(80, ibuff, cbuff)

    End Sub

    Public Sub F90_XTABLEEX(ByRef omcode As Integer, ByRef tabno As Integer, ByRef uunits As Integer, ByRef Init As Integer, ByRef addfg As Integer, ByRef Occur As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim ibuff(80) As Integer

        Call F90_XTABLEEX_XX(omcode, tabno, uunits, Init, addfg, Occur, retkey, ibuff(0), rectyp, retcod)
        Call NumChr(80, ibuff, cbuff)

    End Sub

    Public Sub F90_XBLOCK(ByRef blkno As Integer, ByRef Init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef retcod As Integer)
        Dim ibuff(80) As Integer

        Call F90_XBLOCK_XX(blkno, Init, retkey, ibuff(0), retcod)
        Call NumChr(80, ibuff, cbuff)

    End Sub

    Public Sub F90_XBLOCKEX(ByRef blkno As Integer, ByRef Init As Integer, ByRef retkey As Integer, ByRef cbuff As String, ByRef rectyp As Integer, ByRef retcod As Integer)
        Dim ibuff(80) As Integer

        Call F90_XBLOCKEX_XX(blkno, Init, retkey, ibuff(0), rectyp, retcod)
        Call NumChr(80, ibuff, cbuff)

    End Sub

    Public Sub F90_WATHED(ByRef messunit As Integer, ByRef inwat As Integer, ByRef iVal() As Integer, ByRef rval() As Single, ByRef csit As String, ByRef ctyp As String, ByRef cid As String, ByRef cname As String)
        Dim id(8) As Integer
        Dim Name(48) As Integer
        Dim isit(2) As Integer
        Dim ityp(4) As Integer

        Call F90_WATHED_XX(messunit, inwat, iVal(0), rval(0), isit(0), ityp(0), id(0), Name(0))
        Call NumChr(2, isit, csit)
        Call NumChr(4, ityp, ctyp)
        Call NumChr(8, id, cid)
        Call NumChr(48, Name, cname)
    End Sub

    Public Sub F90_UCGIRC(ByRef ir As Integer, ByRef cname As String)
        Dim Name(12) As Integer

        Call F90_UCGIRC_XX(ir, Name(0))
        Call NumChr(12, Name, cname)
    End Sub

    Public Sub F90_STSPGU(ByRef initfg As Integer, ByRef retcod As Integer, ByRef uvquan As String, ByRef cgroup As String, ByRef uvdesc As String, ByRef uvname As String, ByRef umin As Single, ByRef umax As Single, ByRef udef As Single, ByRef intfg As Integer)
        Dim iname(6) As Integer
        Dim idesc(64) As Integer
        Dim ivname(6) As Integer
        Dim igroup(6) As Integer

        Call F90_STSPGU_XX(initfg, retcod, iname(0), igroup(0), idesc(0), ivname(0), umin, umax, udef, intfg)
        Call NumChr(6, iname, uvquan)
        Call NumChr(64, idesc, uvdesc)
        Call NumChr(6, ivname, uvname)
        Call NumChr(6, igroup, cgroup)
    End Sub

    Public Sub F90_STSPGG(ByRef initfg As Integer, ByRef retcod As Integer, ByRef gname As String, ByRef gdesc As String)
        Dim iname(6) As Integer
        Dim idesc(64) As Integer

        Call F90_STSPGG_XX(initfg, retcod, iname(0), idesc(0))
        Call NumChr(6, iname, gname)
        Call NumChr(64, idesc, gdesc)
    End Sub

    Public Sub F90_STSPFN(ByRef uvname As String, ByRef intfg As Integer, ByRef ucikey As Integer, ByRef idat() As Integer, ByRef aval As Single, ByRef ac As String, ByRef cond As String)

        Dim iac(3) As Integer
        Dim icond(64) As Integer

        Call F90_STSPFN_XX(intfg, ucikey, idat(0), aval, iac(0), icond(0), uvname, Len(uvname))
        Call NumChr(3, iac, ac)
        Call NumChr(64, icond, cond)
    End Sub


    Public Sub F90_GLOBLK(ByRef sdatim() As Integer, ByRef edatim() As Integer, ByRef outlev As Integer, ByRef spout As Integer, ByRef runfg As Integer, ByRef emfg As Integer, ByRef rninfo As String)
        Dim info(80) As Integer

        Call F90_GLOBLK_XX(sdatim(0), edatim(0), outlev, spout, runfg, emfg, info(0))
        Call NumChr(80, info, rninfo)
    End Sub

    Private Sub NumChr(ByRef aLength As Integer, ByRef aStrAsInt() As Integer, ByRef aString As String)
        aString = ""
        For lPos As Integer = 0 To aLength - 1
            If aStrAsInt(lPos) > 0 Then
                aString &= Chr(aStrAsInt(lPos))
            End If
        Next lPos
        aString = aString.TrimEnd
    End Sub
    Private Sub NumChrA(ByRef icnt As Integer, ByRef ilen As Integer, ByRef inam() As Integer, ByRef outstr() As String)

        Dim j, i, p As Integer
        Dim s As String = Nothing
        Dim jnam() As Integer

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


    Public Sub F90_DATLST(ByRef CurDat() As Integer, ByRef DatStr As String)
        Dim l, e As Integer
        Dim i(21) As Integer

        Call F90_DATLST_XX(CurDat(0), i(0), l, e)
        Call NumChr(l, i, DatStr)

    End Sub


    Public Sub F90_DECCHX(ByRef reain As Single, ByRef ilen As Integer, ByRef sigdig As Integer, ByRef decpla As Integer, ByRef RStr As String)

        Dim i() As Integer
        ReDim i(ilen)

        Call F90_DECCHX_XX(reain, ilen, sigdig, decpla, i(0))
        Call NumChr(ilen, i, RStr)

    End Sub
    Public Sub F90_WDBSGC(ByRef w As Integer, ByRef d As Integer, ByRef i As Integer, ByRef l As Integer, ByRef s As String)
        Dim iVal(48) As Integer

        Call F90_WDBSGC_XX(w, d, i, l, iVal(0))
        Call NumChr(l, iVal, s)

    End Sub

    Public Sub F90_TSDSPC(ByRef dsn As Integer, ByRef LSCENM As String, ByRef LRCHNM As String, ByRef LCONNM As String, ByRef Tu As Integer, ByRef ts As Integer, ByRef SDate() As Integer, ByRef EDate() As Integer, ByRef GRPSIZ As Integer)

        Dim Iscenm(8) As Integer
        Dim IRCHNM(8) As Integer
        Dim Iconnm(8) As Integer

        Call F90_TSDSPC_XX(dsn, Iscenm(0), IRCHNM(0), Iconnm(0), Tu, ts, SDate(0), EDate(0), GRPSIZ)
        Call NumChr(8, Iscenm, LSCENM)
        Call NumChr(8, IRCHNM, LRCHNM)
        Call NumChr(8, Iconnm, LCONNM)

    End Sub

    Sub F90_TSDSUN(ByRef cntloc As Integer, ByRef cntsen As Integer, ByRef cntcon As Integer, ByRef LunLoc() As String, ByRef LunSen() As String, ByRef LunCon() As String)

        Dim IunLoc() As Integer
        Dim IunSen() As Integer
        Dim IunCon() As Integer
        Dim slen As Integer

        ReDim IunLoc(8 * cntloc)
        ReDim IunSen(8 * cntsen)
        ReDim IunCon(8 * cntcon)
        Call F90_TSDSUN_XX(cntloc, cntsen, cntcon, IunLoc(0), IunSen(0), IunCon(0))
        slen = 8
        Call NumChrA(cntloc, slen, IunLoc, LunLoc)
        Call NumChrA(cntsen, slen, IunSen, LunSen)
        Call NumChrA(cntcon, slen, IunCon, LunCon)

    End Sub

    Sub F90_SGLABL(ByRef ndsn As Integer, ByRef Csennm() As String, ByRef clocnm() As String, ByRef cconnm() As String, ByRef Tu As Integer, ByRef Dtran As Integer, ByRef which() As Integer, ByRef typind() As Integer, ByRef cntcon As Integer, ByRef cntsen As Integer, ByRef cntloc As Integer, ByRef Calab As String, ByRef Cyrlab As String, ByRef Cyllab As String, ByRef Ctitl As String, ByRef clab() As String, ByRef ctran As String, ByRef ctunit As String)
        Dim i, j As Integer
        Dim Isennm() As Integer
        Dim Ilocnm() As Integer
        Dim Iconnm() As Integer
        Dim Ialab(80) As Integer
        Dim Iyrlab(80) As Integer
        Dim Iyllab(80) As Integer
        Dim Ititl(240) As Integer
        Dim Ilab() As Integer
        Dim Itran(8) As Integer
        Dim Itunit(8) As Integer
        Dim tsen(8) As Integer
        Dim tloc(8) As Integer
        Dim tcon(8) As Integer

        ReDim Ilab(20 * ndsn)
        ReDim Isennm(8 * ndsn)
        ReDim Ilocnm(8 * ndsn)
        ReDim Iconnm(8 * ndsn)

        For i = 0 To ndsn - 1
            Call ChrNum(8, Csennm(i), tsen)
            Call ChrNum(8, clocnm(i), tloc)
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
    Sub F90_WMSGTX(ByRef messfl As Integer, ByRef SCLU As Integer, ByRef SGRP As Integer, ByRef lnflds As Integer, ByRef lscol() As Integer, ByRef lflen() As Integer, ByRef lftyp() As String, ByRef lapos() As Integer, ByRef limin() As Integer, ByRef limax() As Integer, ByRef lidef() As Integer, ByRef lrmin() As Single, ByRef lrmax() As Single, ByRef lrdef() As Single, ByRef lnmhdr As Integer, ByRef hdrbuf() As String, ByRef retcod As Integer)
        Dim ihdrbuf(780) As Integer
        Dim ilftyp(30) As Integer

        Call F90_WMSGTX_XX(messfl, SCLU, SGRP, lnflds, lscol(0), lflen(0), ilftyp(0), lapos(0), limin(0), limax(0), lidef(0), lrmin(0), lrmax(0), lrdef(0), lnmhdr, ihdrbuf(0), retcod)

        Call NumChr(30, ilftyp, lftyp(0))
        Call NumChrA(10, 78, ihdrbuf, hdrbuf)

    End Sub

    Sub F90_WMSGTW(ByRef id As Integer, ByRef s As String)
        Dim i(48) As Integer

        Call F90_WMSGTW_XX(id, i(0))
        Call NumChr(48, i, s)

    End Sub
    Sub F90_WMSGTT(ByRef wdmsfl As Integer, ByRef dsn As Integer, ByRef gnum As Integer, ByRef initfg As Integer, ByRef olen As Integer, ByRef cont As Integer, ByRef obuff As String)
        Dim lobuff(256) As Integer

        Call F90_WMSGTT_XX(wdmsfl, dsn, gnum, initfg, olen, cont, lobuff(0))

        Call NumChr(olen, lobuff, obuff)
    End Sub

    Sub F90_XTINFO(ByRef omcode As Integer, ByRef tnum As Integer, ByRef uunits As Integer, ByRef estflg As Integer, ByRef lnflds As Integer, ByRef lscol() As Integer, ByRef lflen() As Integer, ByRef lftyp As String, ByRef lapos() As Integer, ByRef limin() As Integer, ByRef limax() As Integer, ByRef lidef() As Integer, ByRef lrmin() As Single, ByRef lrmax() As Single, ByRef lrdef() As Single, ByRef lnmhdr As Integer, ByRef hdrbuf() As String, ByRef lfdnam() As String, ByRef isect As Integer, ByRef irept As Integer, ByRef retcod As Integer)
        Dim ihdrbuf(780) As Integer
        Dim ilftyp(30) As Integer
        Dim ifdnam(360) As Integer

        Call F90_XTINFO_XX(omcode, tnum, uunits, estflg, lnflds, lscol(0), lflen(0), ilftyp(0), lapos(0), limin(0), limax(0), lidef(0), lrmin(0), lrmax(0), lrdef(0), lnmhdr, ihdrbuf(0), ifdnam(0), isect, irept, retcod)

        Call NumChr(30, ilftyp, lftyp)
        Call NumChrA(10, 78, ihdrbuf, hdrbuf)
        Call NumChrA(30, 12, ifdnam, lfdnam)

    End Sub

    Sub F90_HGETC(ByRef itmnam As String, ByRef idno As Integer, ByRef ctxt As String, ByRef l As Short)
        Dim itxt(80) As Integer

        Call F90_HGETC_XX(itmnam, idno, itxt(0), l)
        Call NumChr(80, itxt, ctxt)

    End Sub

    Sub F90_GTINS(ByRef Init As Integer, ByRef idno As Integer, ByRef rorb As Integer, ByRef ctxt As String, ByRef rarea As Single)
        Dim itxt(10) As Integer

        Call F90_GTINS_XX(Init, idno, rorb, itxt(0), rarea)
        Call NumChr(10, itxt, ctxt)

    End Sub

    Sub F90_WDSAGY(ByRef wdmsfl As Integer, ByRef id As Integer, ByRef ilen As Integer, ByRef itype As Integer, ByRef rmin As Single, ByRef rmax As Single, ByRef rdef As Single, ByRef hlen As Integer, ByRef hrec As Integer, ByRef hpos As Integer, ByRef vlen As Integer, ByRef Name As String, ByRef desc As String, ByRef valid As String)
        Dim itxt(6) As Integer
        Dim idesc(47) As Integer
        Dim ivalid(240) As Integer

        Call F90_WDSAGY_XX(wdmsfl, id, ilen, itype, rmin, rmax, rdef, hlen, hrec, hpos, vlen, itxt(0), idesc(0), ivalid(0))
        Call NumChr(6, itxt, Name)
        Call NumChr(47, idesc, desc)
        Call NumChr(vlen, ivalid, valid)

    End Sub

    Sub F90_GETUCI(ByRef rectyp As Integer, ByRef Key As Integer, ByRef ucibuf As String)
        Dim iucibf(80) As Integer

        Call F90_GETUCI_XX(rectyp, Key, iucibf(0))

        Call NumChr(80, iucibf, ucibuf)
    End Sub
End Module