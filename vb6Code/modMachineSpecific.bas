Attribute VB_Name = "modMachineSpecific"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

'Full path of GenScn executable - WinKeyDriver.exe is expected in same place
Global Const MACHINE_EXENAME = "E:\GenScnWinHSPF2.2b1\Support\bin\GenScn.exe"

'Data file to load when GenScn starts - can be "" to start w/o loading anything
Global Const MACHINE_EXECMD = "" '"e:\data\shena\shena.sta"

'Full path of Status.exe (Status monitor)
Global Const MACHINE_EXESTATUS = "E:\GenScnWinHSPF2.2b1\Support\bin\Status.exe"

'WinHSPF uses the following constants, GenScn does not use them
Global Const MACHINE_HSPFMSGMDB = "E:\GenScnWinHSPF2.2b1\Support\bin\hspfmsg.mdb"
Global Const MACHINE_POLLUTANTLIST = "E:\GenScnWinHSPF2.2b1\Support\binpoltnt_2.prn"
Global Const MACHINE_HSPFMSGWDM = "e:\GenScnWinHSPF2.2b1\Support\bin\hspfmsg.wdm"
Global Const MACHINE_STARTERPATH = "e:\GenScnWinHSPF2.2b1\Support\bin\Starter"
Global Const MACHINE_EXEWINHSPF = "e:\GenScnWinHSPF2.2b1\Support\bin\winhspf.exe"

Global Const MACHINE_EXEWDMUTIL = "E:\GenScnWinHSPF2.2b1\Support\bin\WDMUtil.exe"
Global Const MACHINE_CMDWDMUTIL = ""

