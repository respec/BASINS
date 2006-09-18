Option Strict Off
Option Explicit On
Module modStatusInputTimeseriesImplnd
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
	
	Public Sub UpdateInputTimeseriesImplnd(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
		Dim ltable As HspfTable
		Dim nquals As Integer
        Dim csnofg, i As Integer
		Dim iqadfg(20) As Integer
		Dim ctemp As String
		Dim qualsd, qualif, dryadfg, wetadfg, qualof, qualgw, snopfg As Integer
		
		If O.TableExists("ACTIVITY") Then
			ltable = O.Tables.Item("ACTIVITY")
			
			'section atemp
            If ltable.Parms.Item("ATMPFG") = 1 Then
                TimserStatus.Change("EXTNL:GATMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
            End If
			
			'section snow
			If O.TableExists("SNOW-FLAGS") Then
                snopfg = O.Tables.Item("SNOW-FLAGS").ParmValue("SNOPFG")
			Else
				snopfg = 0
			End If
            If ltable.Parms.Item("SNOWFG") = 1 Then
                TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                If snopfg = 0 Then
                    TimserStatus.Change("EXTNL:DTMPG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:WINMOV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:SOLRAD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("EXTNL:CLOUD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                Else
                    TimserStatus.Change("EXTNL:DTMPG", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                End If
                If ltable.Parms.Item("ATMPFG") = 0 Then
                    TimserStatus.Change("ATEMP:AIRTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			If O.TableExists("IWAT-PARM1") Then
                csnofg = O.Tables.Item("IWAT-PARM1").ParmValue("CSNOFG")
			Else
				csnofg = 0
			End If
			
			'section iwater
            If ltable.Parms.Item("IWATFG") = 1 Then
                TimserStatus.Change("EXTNL:PETINP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:SURLI", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If csnofg = 0 Then
                    TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If csnofg = 1 Then
                    If ltable.Parms.Item("ATMPFG") = 0 Then
                        TimserStatus.Change("ATEMP:AIRTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If ltable.Parms.Item("SNOWFG") = 0 Then
                        TimserStatus.Change("SNOW:RAINF", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TimserStatus.Change("SNOW:SNOCOV", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                        TimserStatus.Change("SNOW:WYIELD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                End If
            End If
			
			'section solids
            If ltable.Parms.Item("SLDFG") = 1 Then
                TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                TimserStatus.Change("EXTNL:SLSLD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusOptional)
                If ltable.Parms.Item("IWATFG") = 0 Then
                    TimserStatus.Change("IWATER:SURO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    TimserStatus.Change("IWATER:SURS", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			'section iwtgas
            If ltable.Parms.Item("IWGFG") = 1 Then
                If ltable.Parms.Item("ATMPFG") = 0 Then
                    TimserStatus.Change("ATEMP:AIRTMP", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("SNOWFG") = 0 And csnofg = 1 Then
                    TimserStatus.Change("SNOW:WYIELD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
                If ltable.Parms.Item("IWATFG") = 0 Then
                    TimserStatus.Change("IWATER:SURO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
			If O.TableExists("IQL-AD-FLAGS") Then
				With O.Tables.Item("IQL-AD-FLAGS")
					For i = 1 To 20
						ctemp = "IQADFG(" & CStr(i) & ")"
                        iqadfg(i) = .Parms(ctemp)
					Next i
				End With
			Else
				For i = 1 To 20
					iqadfg(i) = 0
				Next i
			End If
			
			'section iqual
            If ltable.Parms.Item("IQALFG") = 1 Then
                If O.TableExists("NQUALS") Then
                    nquals = O.Tables.Item("NQUALS").ParmValue("NQUAL")
                Else
                    nquals = 1
                End If
                wetadfg = 0
                dryadfg = 0
                qualof = 0
                qualif = 0
                qualgw = 0
                qualsd = 0
                For i = 1 To nquals
                    wetadfg = 0
                    dryadfg = 0
                    If iqadfg(((i - 1) * 2) + 1) < 0 Then 'need input timeseries
                        'dry ad simulated
                        dryadfg = 1
                    End If
                    If iqadfg(((i - 1) * 2) + 2) < 0 Then 'need input timeseries
                        'wet ad simulated
                        wetadfg = 1
                    End If
                    ctemp = "QUAL-PROPS" & CStr(i)
                    If O.TableExists(ctemp) Then
                        If O.Tables.Item(ctemp).ParmValue("QSOFG") > 0 Then
                            qualof = 1
                        End If
                        If O.Tables.Item(ctemp).ParmValue("QSDFG") > 0 Then
                            qualsd = 1
                        End If
                    End If
                    If dryadfg = 1 Then
                        TimserStatus.Change("EXTNL:IQADFX", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If wetadfg = 1 Then
                        TimserStatus.Change("EXTNL:IQADCN", i, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                    If wetadfg = 1 Then
                        TimserStatus.Change("EXTNL:PREC", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                    End If
                Next i

                If ltable.Parms.Item("IWATFG") = 0 Then
                    If qualof > 0 Then
                        TimserStatus.Change("PWATER:SURO", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired) 'or req if SOQC is required for one or more QUALs
                    End If
                End If
                If ltable.Parms.Item("SLDFG") = 0 And qualsd > 0 Then
                    TimserStatus.Change("SOLIDS:SOSLD", 1, HspfStatus.HspfStatusReqOptUnnEnum.HspfStatusRequired)
                End If
            End If
			
		End If
	End Sub
End Module