Attribute VB_Name = "est_ent"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Declare Sub F90_GPCODE Lib "estimato.dll" (l&, l&, ByVal s$, ByVal i%)

Declare Sub F90_REALDATE Lib "estimato.dll" (l&, l&, l&, r!)

Declare Sub F90_I3DATE Lib "estimato.dll" (d#, l&)

Declare Sub F90_ESTIM Lib "estimato.dll" (l&, l&, l&, l&, l&, _
                      r!, r!, l&, l&, l&, l&, _
                      ByVal s$, ByVal s$, ByVal s$, ByVal s$, ByVal s$, ByVal s$, _
                      ByVal i%, ByVal i%, ByVal i%, ByVal i%, _
                      ByVal i%, ByVal i%)

Declare Sub F90_DPLOTI Lib "estimato.dll" ()

Declare Sub F90_GBOXPN Lib "estimato.dll" (l&)

Declare Sub F90_GPLOTN Lib "estimato.dll" (l&)

Declare Sub F90_GBOXP_XX Lib "estimato.dll" (l&, l&, l&, d#, l&)

Declare Sub F90_GPLOTP_XX Lib "estimato.dll" (l&, l&, l&, d#, d#, r!, l&, l&, l&, l&)

Public Sub F90_GPLOTP(iplot&, ndata&, nfun&, x#(), a#(), range!(), symbol$, xtitle$, ytitle$, Title$)
   Dim istr&(10), ix&(80), iy&(80), it&(80)
   
   Call F90_GPLOTP_XX(iplot, ndata, nfun, x(0), a(0, 0), range(0), istr(0), ix(0), iy(0), it(0))
   Call NumChr(9, istr, symbol)
   Call NumChr(79, ix, xtitle)
   Call NumChr(79, iy, ytitle)
   Call NumChr(79, it, Title)
   Title = RTrim(Title)
   xtitle = RTrim(xtitle)
   ytitle = RTrim(ytitle)

End Sub

Public Sub F90_GBOXP(iplot&, ngroup&, ni&(), x#(), Title$)
   Dim it&(80)
   
   Call F90_GBOXP_XX(iplot, ngroup, ni(0), x(0), it(0))
   Call NumChr(79, it, Title)
   Title = RTrim(Title)

End Sub
