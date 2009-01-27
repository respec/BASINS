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