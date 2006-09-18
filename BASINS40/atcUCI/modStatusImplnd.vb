Option Strict Off
Option Explicit On
Module modStatusImplnd
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public Sub UpdateImplnd(ByRef O As HspfOperation, ByRef TableStatus As HspfStatus)
		Dim ltable As HspfTable
		Dim i As Integer
        'added flag for snow THJ
		Dim Vkmfg As Integer
		Dim Vrsfg, Vnnfg As Integer
		Dim Vasdfg, Vrsdfg As Integer
		Dim Wtfvfg As Integer
		Dim Vpfwfg, Nqual, Vqofg As Integer
		Dim tabname As String
		
		'always can be present
		TableStatus.Change("ACTIVITY", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
		TableStatus.Change("PRINT-INFO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
		TableStatus.Change("GEN-INFO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
		
		If O.TableExists("ACTIVITY") Then
			ltable = O.Tables.Item("ACTIVITY")
            If ltable.Parms.Item("ATMPFG") = 1 Then
                TableStatus.Change("ATEMP-DAT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("SNOWFG") = 1 Then
                TableStatus.Change("ICE-FLAG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("SNOW-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("SNOW-FLAGS") Then
                    Vkmfg = O.Tables.Item("SNOW-FLAGS").ParmValue("VKMFG")
                Else
                    Vkmfg = 0
                End If
                TableStatus.Change("SNOW-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("SNOW-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Vkmfg = 1 Then
                    TableStatus.Change("MON-MELT-FAC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("SNOW-INIT1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("SNOW-INIT2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("IWATFG") = 1 Then
                TableStatus.Change("IWAT-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("IWAT-PARM1") Then
                    Vrsfg = O.Tables.Item("IWAT-PARM1").ParmValue("VRSFG")
                    Vnnfg = O.Tables.Item("IWAT-PARM1").ParmValue("VNNFG")
                    If ltable.Parms.Item("SNOWFG") = 1 Then
                        O.Tables.Item("IWAT-PARM1").ParmValue("CSNOFG") = 1
                    End If
                Else
                    Vrsfg = 0
                    Vnnfg = 0
                End If
                TableStatus.Change("IWAT-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TableStatus.Change("IWAT-PARM3", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Vrsfg = 1 Then
                    TableStatus.Change("MON-RETN", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If Vnnfg = 1 Then
                    TableStatus.Change("MON-MANNING", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("IWAT-STATE1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("SLDFG") = 1 Then
                TableStatus.Change("SLD-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("SLD-PARM1") Then
                    Vasdfg = O.Tables.Item("SLD-PARM1").ParmValue("VASDFG")
                    Vrsdfg = O.Tables.Item("SLD-PARM1").ParmValue("VRSDFG")
                Else
                    Vasdfg = 0
                    Vrsdfg = 0
                End If
                TableStatus.Change("SLD-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If Vasdfg = 1 Then
                    TableStatus.Change("MON-SACCUM", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If Vrsdfg = 1 Then
                    TableStatus.Change("MON-REMOV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("SLD-STOR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("IWGFG") = 1 Then
                TableStatus.Change("IWT-PARM1", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("IWT-PARM1") Then
                    Wtfvfg = O.Tables.Item("IWT-PARM1").ParmValue("WTFVFG")
                Else
                    Wtfvfg = 0
                End If
                TableStatus.Change("IWT-PARM2", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("LAT-FACTOR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If Wtfvfg = 1 Then
                    TableStatus.Change("MON-AWTF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    TableStatus.Change("MON-BWTF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                TableStatus.Change("IWT-INIT", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
            End If
            If ltable.Parms.Item("IQALFG") = 1 Then
                TableStatus.Change("NQUALS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If O.TableExists("NQUALS") Then
                    Nqual = O.Tables.Item("NQUALS").ParmValue("NQUAL")
                Else
                    Nqual = 1
                End If
                TableStatus.Change("IQL-AD-FLAGS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                TableStatus.Change("LAT-FACTOR", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                'i'm assuming that it is ok to set this twice - either section can ask
                'for it.  same for PERLND LAT-FACTOR (PWTGAS, PQUAL); PERLND SOIL-DATA
                '(PWATER, PEST, NITR, PHOS); PERLND SOIL-DATA2 (PWATER,NITR).  THJ
                'need more here for monthly values - what do you refer to here? THJ
                For i = 1 To Nqual
                    TableStatus.Change("QUAL-PROPS", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    If i > 1 Then
                        tabname = "QUAL-PROPS:" & i
                    Else
                        tabname = "QUAL-PROPS"
                    End If
                    If O.TableExists(tabname) Then
                        Vpfwfg = O.Tables.Item(tabname).ParmValue("VPFWFG")
                        Vqofg = O.Tables.Item(tabname).ParmValue("VQOFG")
                    Else
                        Vpfwfg = 0
                        Vqofg = 0
                    End If
                    TableStatus.Change("QUAL-INPUT", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                    If Vpfwfg = 1 Then
                        TableStatus.Change("MON-POTFW", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If Vqofg = 1 Then
                        TableStatus.Change("MON-ACCUM", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TableStatus.Change("MON-SQOLIM", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                Next i
                'the use of "i" here is inaccurate.  E.G. if you have four quals, but
                'only the first and fourth are solids associated, then the fourth qual
                'will look for the second occurrence of MON-POTFW (assuming VPFWFG is 1
                'for both), not the fourth.
                'On the other hand, if all four are solids associated, but only the first
                'and fourth have VPFWFG=1, then the fourth *will* look for the fourth
                'occurrence, meaning that there have to be dummy tables for the second
                'and third! I think your code finds this correctly, but are you also
                'planning to be able to build a uci from scratch with this code?  If
                'so, then you'll have to add those dummy tables yourself if needed. THJ
            End If
		End If
	End Sub
End Module