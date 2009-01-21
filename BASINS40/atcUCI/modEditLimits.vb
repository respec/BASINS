'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Module modEditLimits

    Private NoDsn() As Integer
    Private NoDsnCount, resp As Integer
    Private scen() As String
    Private locn() As String
    Private cons() As String
    Private cwdmid() As String
	
    'Public Sub CheckLimitsExtTargets(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.ClearValues. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	g.ClearValues()
    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	If g.col = 0 Then 'svol
    '		Call SetValidOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 1 Then  'svolno
    '		Call SetOperationMinMax(g, myUci, 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 2 Then  'sgrpn
    '		Call SetGroupNames(g, (myUci.Msg), 2)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 3 Then  'smemn
    '		Call SetMemberNames(g, (myUci.Msg), 3)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 4 Then  'smems1
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 5 Then  'smems2
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 6 Then  'mfactr
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 7 Then  'tran
    '		Call SetValidTrans(g)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 8 Then  'tvol:wdm and what else
    '		Call SetLimitsWDM(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 9 Then  'tvolno
    '		Call CheckValidDsn(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 10 Then  'tmemn
    '		Call CheckValidMemberName(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 11 Then  'qflg
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.ColMin(g.col) = 0
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.ColMax(g.col) = 31
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 12 Then  'tsyst
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue("ENGL")
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue("METR")
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 13 Then  'aggst
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue(" ") 'allow default blank
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue("AGGR")
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 14 Then  'amdst
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue("ADD ")
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue("REPL")
    '	End If
    'End Sub
	
    'Public Sub CheckLimitsNetwork(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.ClearValues. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	g.ClearValues()
    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	If g.col = 0 Then 'svol
    '		Call SetValidOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 1 Then  'svolno
    '		Call SetOperationMinMax(g, myUci, 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 2 Then  'sgrpn
    '		Call SetGroupNames(g, (myUci.Msg), 2)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 3 Then  'smemn
    '		Call SetMemberNames(g, (myUci.Msg), 3)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 4 Then  'smems1
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 5 Then  'smems2
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 6 Then  'mfactr
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 7 Then  'tran
    '		Call SetValidTrans(g)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 8 Then  'tvol
    '		Call SetValidOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 9 Then  'topfst
    '		Call SetOperationMinMax(g, myUci, 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 10 Then  'tgrpn
    '		Call SetGroupNames(g, (myUci.Msg), 2)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 11 Then  'tmem
    '		Call SetMemberNames(g, (myUci.Msg), 3)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 12 Then  'tmems1
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 13 Then  'tmems2
    '	End If
    'End Sub

    'Public Sub CheckLimitsMassLink(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.ClearValues. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	g.ClearValues()
    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	If g.col = 0 Then 'svol
    '		Call SetAllOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 1 Then  'sgrpn
    '		Call SetGroupNames(g, (myUci.Msg), 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 2 Then  'smemn
    '		Call SetMemberNames(g, (myUci.Msg), 2)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 3 Then  'smems1
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 4 Then  'smems2
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 5 Then  'mfactr
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 6 Then  'tvol
    '		Call SetAllOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 7 Then  'tgrpn
    '		Call SetGroupNames(g, (myUci.Msg), 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 8 Then  'tmem
    '		Call SetMemberNames(g, (myUci.Msg), 2)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 9 Then  'tmems1
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 10 Then  'tmems2
    '	End If
    'End Sub

    'Public Sub CheckLimitsSchematic(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.ClearValues. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	g.ClearValues()
    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	If g.col = 0 Then 'svol
    '		Call SetValidOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 1 Then  'svolno
    '		Call SetOperationMinMax(g, myUci, 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 2 Then  'afacter
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 3 Then  'tvol
    '		Call SetValidOperations(g, myUci)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 4 Then  'tvolno
    '		Call SetOperationMinMax(g, myUci, 1)
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf g.col = 5 Then  'mslkno
    '		Call SetValidMassLinks(g, myUci)
    '	End If
    'End Sub

    'Public Sub CheckLimitsSpecialActions(ByRef Index As Object, ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)
    '	Dim vOpnBlk As Object
    '	Dim lopnblk As HspfOpnBlk

    '	'UPGRADE_WARNING: Couldn't resolve default property of object g.ClearValues. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	g.ClearValues()
    '	'UPGRADE_WARNING: Couldn't resolve default property of object Index. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	If Index = 1 Then
    '		'action type record
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		If g.col = 0 Then
    '			'valid operation types
    '			For	Each vOpnBlk In myUci.OpnBlks
    '				lopnblk = vOpnBlk
    '				If lopnblk.Count > 0 Then
    '					If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
    '						'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '						g.addValue(lopnblk.Name)
    '					End If
    '				End If
    '			Next vOpnBlk
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 1 Then 
    '			Call SetOperationMinMax(g, myUci, 1)
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 2 Then 
    '			Call SetOperationMinMax(g, myUci, 2)
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 3 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MI")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("HR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("DY")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MO")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("YR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 4 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 5 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 6 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 7 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 8 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 9 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 10 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 11 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = 4
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 2
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 18 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MI")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("HR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("DY")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MO")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("YR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 19 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 20 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '		End If
    '		'UPGRADE_WARNING: Couldn't resolve default property of object Index. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf Index = 2 Then 
    '		'distributes
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		If g.col = 0 Then
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 1 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = 10
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 1
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 2 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MI")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("HR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("DY")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MO")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("YR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 3 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 4 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("SKIP")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("SHIFT")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("ACCUM")
    '		Else
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '		End If
    '		'UPGRADE_WARNING: Couldn't resolve default property of object Index. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf Index = 3 Then 
    '		'uvname
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		If g.col = 1 Then
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 1
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 7 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("QUAN")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOVT")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOV1")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOV2")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 13 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("QUAN")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOVT")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOV1")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MOV2")
    '		End If
    '		'UPGRADE_WARNING: Couldn't resolve default property of object Index. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf Index = 4 Then 
    '		'User Defn Quan
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		If g.col = 1 Then
    '			'valid operation types
    '			For	Each vOpnBlk In myUci.OpnBlks
    '				lopnblk = vOpnBlk
    '				If lopnblk.Count > 0 Then
    '					If lopnblk.Name = "PERLND" Or lopnblk.Name = "IMPLND" Or lopnblk.Name = "RCHRES" Or lopnblk.Name = "PLTGEN" Or lopnblk.Name = "COPY" Or lopnblk.Name = "GENER" Then
    '						'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '						g.addValue(lopnblk.Name)
    '					End If
    '				End If
    '			Next vOpnBlk
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 2 Then 
    '			Call SetOperationMinMax(g, myUci, 1)
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 7 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = 4
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 2
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 9 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MI")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("HR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("DY")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MO")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("YR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 10 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 11 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MI")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("HR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("DY")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MO")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("YR")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 12 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMax. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMax(g.col) = -999
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.ColMin. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.ColMin(g.col) = 0
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.col. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		ElseIf g.col = 13 Then 
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("SUM")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("AVER")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MAX")
    '			'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			g.addValue("MIN")
    '		End If
    '		'UPGRADE_WARNING: Couldn't resolve default property of object Index. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '	ElseIf Index = 5 Then 
    '		'conditional
    '	End If
    'End Sub

    'Private Sub SetAllOperations(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	Dim vOpnBlk As Object
    '	Dim lopnblk As HspfOpnBlk

    '	For	Each vOpnBlk In myUci.OpnBlks
    '		lopnblk = vOpnBlk
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue(lopnblk.Name)
    '	Next vOpnBlk
    'End Sub

    'Private Sub SetValidMassLinks(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci)

    '	Dim vMassLink As Object
    '	Dim lMassLink As HspfMassLink
    '	Dim tMassLinks() As String
    '	Dim tcnt, i As Integer
    '	Dim ifound As Boolean

    '	tcnt = 0

    '	For	Each vMassLink In myUci.MassLinks
    '		lMassLink = vMassLink
    '		ifound = False
    '		For i = 1 To tcnt
    '			If CDbl(tMassLinks(i)) = lMassLink.MassLinkID Then
    '				ifound = True
    '			End If
    '		Next i
    '		If ifound = False Then
    '			tcnt = tcnt + 1
    '			ReDim Preserve tMassLinks(tcnt)
    '			tMassLinks(tcnt) = CStr(lMassLink.MassLinkID)
    '		End If
    '	Next vMassLink

    '	For i = 1 To tcnt
    '		'UPGRADE_WARNING: Couldn't resolve default property of object g.addValue. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '		g.addValue(tMassLinks(i))
    '	Next i
    'End Sub

    'Public Sub CheckDataSetExistance(ByRef g As System.Windows.Forms.Control, ByRef myUci As HspfUci, ByRef retcod As Integer)
    '       Dim dsnObj As ATCData.ATCclsTserData
    '	Dim dsn, wdmid As String
    '	Dim i As Integer

    '	NoDsnCount = 0
    '       ReDim NoDsn(g.rows)
    '       ReDim cwdmid(g.rows)
    '	ReDim scen(g.rows)
    '	ReDim locn(g.rows)
    '	ReDim cons(g.rows)

    '	For i = 1 To g.rows
    '           'does this wdm data set exist
    '           dsn = g.TextMatrix(i, 9)
    '           wdmid = g.TextMatrix(i, 8)
    '           If Len(dsn) > 0 And Len(wdmid) > 0 Then
    '               dsnObj = myUci.GetDataSetFromDsn(WDMInd(wdmid), CShort(dsn))
    '               If dsnObj Is Nothing Then
    '                   'does not exist
    '                   NoDsnCount = NoDsnCount + 1
    '                   NoDsn(NoDsnCount) = CInt(dsn)
    '                   cwdmid(NoDsnCount) = wdmid
    '                   scen(NoDsnCount) = UCase(IO.Path.GetFileNameWithoutExtension((myUci.Name)))
    '                   'UPGRADE_WARNING: Couldn't resolve default property of object g.TextMatrix. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '                   locn(NoDsnCount) = g.TextMatrix(i, 0) & g.TextMatrix(i, 1)
    '                   'UPGRADE_WARNING: Couldn't resolve default property of object g.TextMatrix. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '                   cons(NoDsnCount) = g.TextMatrix(i, 10)
    '               End If
    '           End If
    '       Next i

    '	retcod = -1 'add dataset window not needed
    '	If NoDsnCount > 0 Then
    '		'    Set frmAddDataSet.icon = myUci.icon
    '		'    Call frmAddDataSet.SetUCI(myUci)
    '		'    frmAddDataSet.Show 1
    '		'    retcod = resp 'return code from add data set, 0-ok, 1-cancel
    '	End If
    'End Sub
	
    Public Sub GetNonExistentDataSetInfo(ByRef i As Integer, _
                                         ByRef adsn As Integer, _
                                         ByRef wid As String, _
                                         ByRef s As String, _
                                         ByRef l As String, _
                                         ByRef c As String)

        adsn = NoDsn(i)
        wid = cwdmid(i)
        s = scen(i)
        l = locn(i)
        c = cons(i)
    End Sub
	
    Public Sub GetNonExistentDataSetCount(ByRef n As Integer)
        n = NoDsnCount
    End Sub
	
    Public Sub UpdateRespFromAddDataSet(ByRef i As Integer)
        '1 - cancel from 'add data set'
        '0 - ok from 'add data set'
        '-1 - 'add data set' not needed
        resp = i
    End Sub
End Module