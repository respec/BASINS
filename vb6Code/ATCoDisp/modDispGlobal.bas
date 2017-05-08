Attribute VB_Name = "modDispGlobal"
Option Explicit
'Copyright 2001-2005 by AQUA TERRA Consultants
'Global dbg As ATCoDebug
'Global pLaunch As AtCoLaunch

Global Const AppName As String = "ATCoOldDisp"

Global pIPC As ATCoIPC
Global GraphHelpFileName$

'list form array
Global NumList As Long
Global l() As New frmL
Global curL As frmL

Global NumGra As Long
'Global g() As New frmG
Global curG As frmG

