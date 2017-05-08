Attribute VB_Name = "FeqLibs"
Attribute VB_Ext_KEY = "RVB_UniqueId" ,"3606AF0800B9"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'invmjd routine
'Declare Function F90_MJD Lib "feqlib.dll" (l&, l&, l&) As Long
'mjd routine
'Declare Sub F90_INVMJD Lib "feqlib.dll" (l&, l&, l&, l&)
'lktab routine
Declare Sub F90_LKTAB Lib "feqlib.dll" (l&, r!, l&, r!, l&, r!)
'xlkt routines
Declare Sub F90_XLKT20 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_XLKT21 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_XLKT22 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_XLKT23 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_XLKT24 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_XLKT25 Lib "feqlib.dll" (l&, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!, r!)
Declare Sub F90_FILTAB Lib "feqlib.dll" (ByVal s$, l&, l&, l&, ByVal i%)
Declare Sub F90_TABTYP Lib "feqlib.dll" (l&, l&)

