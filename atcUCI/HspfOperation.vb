'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Collections.ObjectModel

Public Class HspfOperation

    Private Enum LegendType
        LegLand = 0
        LegMet = 1
        LegPoint = 2
    End Enum

    Private pOpTyp As HspfData.HspfOperType
    Private pId As Integer
    Private pDescription As String
    Private pTables As Collection 'of HspfTable
    Private pTableStatus As HspfStatus
    Private pInputTimeseriesStatus As HspfStatus
    Private pOutputTimeseriesStatus As HspfStatus
    Private pSources As Collection 'of HspfConnection
    Private pTargets As Collection 'of HspfConnection
    Private pOpnBlk As HspfOpnBlk
    Private pEdited As Boolean
    Private pFTable As HspfFtable
    Private pMetSeg As HspfMetSeg
    Private pPointSources As Collection(Of HspfPoint)
    Private pUci As HspfUci
    Private pComment As String
    Private pSerial As Integer
    Private pDefOpnId As Integer

    Public ReadOnly Property EditControlName() As String
        Get
            EditControlName = "ATCoHspf.ctlOperationEdit"
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Caption = "Operation:  " & HspfOperName(pOpTyp) & " " & pId & " - " & pDescription
        End Get
    End Property


    Public Property Comment() As String
        Get
            Comment = pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public Property Edited() As Boolean
        Get
            Edited = pEdited
        End Get
        Set(ByVal Value As Boolean)
            pEdited = Value
            If Value Then pOpnBlk.Edited = True
        End Set
    End Property

    Public Property optyp() As HspfData.HspfOperType
        Get
            optyp = pOpTyp
        End Get
        Set(ByVal Value As HspfData.HspfOperType)
            pOpTyp = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Name = HspfOperName(pOpTyp)
        End Get
        Set(ByVal Value As String)
            pOpTyp = HspfOperNum(Value)
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Id = pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property DefOpnId() As Integer
        Get
            DefOpnId = pDefOpnId
        End Get
        Set(ByVal Value As Integer)
            pDefOpnId = Value
        End Set
    End Property

    Public ReadOnly Property Serial() As Integer
        Get
            Serial = pSerial
        End Get
    End Property

    Public Property Description() As String
        Get
            Description = pDescription
        End Get
        Set(ByVal Value As String)
            Dim colonPos As Integer
            pDescription = Value
            colonPos = InStr(pDescription, ":")
            If colonPos > 0 Then pDescription = Mid(pDescription, colonPos + 1)
        End Set
    End Property

    Public Property OpnBlk() As HspfOpnBlk
        Get
            OpnBlk = pOpnBlk
        End Get
        Set(ByVal Value As HspfOpnBlk)
            pOpnBlk = Value
        End Set
    End Property

    Public Property FTable() As HspfFtable
        Get
            FTable = pFTable
        End Get
        Set(ByVal Value As HspfFtable)
            pFTable = Value
        End Set
    End Property

    Public Property MetSeg() As HspfMetSeg
        Get
            MetSeg = pMetSeg
        End Get
        Set(ByVal Value As HspfMetSeg)
            pMetSeg = Value
        End Set
    End Property

    Public ReadOnly Property TableStatus() As HspfStatus
        Get
            TableStatus = pTableStatus
        End Get
    End Property

    Public ReadOnly Property InputTimeseriesStatus() As HspfStatus
        Get
            InputTimeseriesStatus = pInputTimeseriesStatus
        End Get
    End Property

    Public ReadOnly Property OutputTimeseriesStatus() As HspfStatus
        Get
            OutputTimeseriesStatus = pOutputTimeseriesStatus
        End Get
    End Property


    Public Property PointSources() As Collection(Of HspfPoint)
        Get
            PointSources = pPointSources
        End Get
        Set(ByVal Value As Collection(Of HspfPoint))
            pPointSources = Value
        End Set
    End Property

    Public ReadOnly Property Tables() As Collection
        Get 'of HspfTable
            Tables = pTables
        End Get
    End Property

    Public ReadOnly Property Sources() As Collection
        Get 'of HspfConnection
            Sources = pSources
        End Get
    End Property

    Public ReadOnly Property Targets() As Collection
        Get 'of HspfConnection
            Targets = pTargets
        End Get
    End Property

    Public Property Uci() As HspfUci
        Get
            Uci = pUci
        End Get
        Set(ByVal Value As HspfUci)
            pUci = Value
        End Set
    End Property

    Public Sub Edit()
        'status or hourglass needed here
        editInit(Me, (pUci.icon), True, True, False)
    End Sub

    Public Function TableExists(ByRef aName As String) As Boolean
        Return pTables.Contains(aName)
    End Function

    Public Sub setTimSerConnections()
        Dim vConn As Object
        Dim lConn As HspfConnection
        Dim lName As String

        lName = HspfOperName(pOpTyp)
        For Each vConn In pUci.Connections
            lConn = vConn
            With lConn.Target
                If .VolName = lName Then
                    If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
                        lConn.Target.Opn = Me
                        pSources.Add(lConn)
                    End If
                End If
            End With
            With lConn.Source
                If .VolName = lName Then
                    If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
                        lConn.Source.Opn = Me
                        pTargets.Add(lConn)
                    End If
                End If
            End With
        Next vConn
    End Sub

    Public Sub setTimSerConnectionsSources()
        Dim vConn As Object
        Dim lConn As HspfConnection
        Dim lName As String

        lName = HspfOperName(pOpTyp)
        For Each vConn In pUci.Connections
            lConn = vConn
            With lConn.Target
                If .VolName = lName Then
                    If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
                        lConn.Target.Opn = Me
                        pSources.Add(lConn)
                    End If
                End If
            End With
        Next vConn
    End Sub

    Public Sub setTimSerConnectionsTargets()
        Dim vConn As Object
        Dim lConn As HspfConnection
        Dim lName As String

        lName = HspfOperName(pOpTyp)
        For Each vConn In pUci.Connections
            lConn = vConn
            With lConn.Source
                If .VolName = lName Then
                    If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
                        lConn.Source.Opn = Me
                        pTargets.Add(lConn)
                    End If
                End If
            End With
        Next vConn
    End Sub

    Public Function DownOper(ByRef OpType As String) As Integer
        Dim vConn As HspfConnection
        For Each vConn In pTargets
            If Len(OpType) = 0 Then 'take first one of any type
                DownOper = vConn.Target.VolId
                Exit For
            ElseIf vConn.Target.VolName = OpType Then  'first of selected type
                DownOper = vConn.Target.VolId
                Exit For
            End If
        Next vConn
    End Function

    'Not needed until WinHSPF wants it
    '	Public Sub setPicture(ByRef O As Object, ByRef ColorMap As Collection, ByRef CurrentLegend As Integer, Optional ByRef LegendOrder As Collection = Nothing)
    '		Dim barbase, barHeight, sid, barPos, barWidth, maxNBars As Integer
    '		Dim lTemp As String
    '		Dim pic As System.Windows.Forms.PictureBox
    '		Dim lStr, desc As String
    '		Dim barDesc As Object
    '		Dim lSource As HspfConnection
    '		Dim lDesc As String
    '		Dim colr As Integer
    '		Dim barMaxVal As Double
    '		Dim started As Boolean
    '		Dim included() As Boolean

    '		barWidth = 3
    '		pic = O
    '		'  maxNBars = pUci.OpnBlks("PERLND").Count + pUci.OpnBlks("IMPLND").Count
    '		'  If maxNBars > 10 Then maxNBars = 10
    '		'pic.Caption = pOpnBlk.Name & " " & pId
    '		lStr = pOpnBlk.Name & " " & pId
    '		'UPGRADE_ISSUE: PictureBox property pic.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		pic.ToolTipText = pOpnBlk.Name & " " & pId & " " & pDescription
    '		'frmPictures.Show
    '		pic.Image = Nothing '.picTemp.Picture = .picBlank.Picture
    '		'.picTemp.Height = pic.Height
    '		'.picTemp.Width = pic.Width + barWidth * 16 '(pSources.Count + 2)

    '		'UPGRADE_ISSUE: PictureBox method pic.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		'UPGRADE_ISSUE: PictureBox property pic.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		pic.CurrentX = (VB6.PixelsToTwipsX(pic.Width) - pic.TextWidth(lStr)) / 2
    '		'UPGRADE_ISSUE: PictureBox method pic.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		'UPGRADE_ISSUE: PictureBox property pic.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		pic.CurrentY = VB6.PixelsToTwipsY(pic.Height) - pic.TextHeight(lStr) * 1.25
    '		'UPGRADE_ISSUE: PictureBox property pic.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		barbase = pic.CurrentY
    '		'UPGRADE_ISSUE: PictureBox method pic.Print was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '		pic.Print(lStr)
    '		'Debug.Print str & " ";
    '		Dim myid As Integer
    '		Dim pPoint As HspfPoint
    '		Select Case CurrentLegend
    '			Case LegendType.LegLand
    '				barMaxVal = pUci.MaxAreaByLand2Stream
    '				barPos = barWidth
    '				If LegendOrder Is Nothing Then 'Draw all in the order they fall
    '					For	Each lSource In pSources
    '						If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
    '							barHeight = lSource.MFact / barMaxVal * barbase
    '							On Error GoTo ColorNotFound
    '							lDesc = lSource.Source.Opn.Description
    '							'UPGRADE_WARNING: Couldn't resolve default property of object ColorMap(). Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '							colr = ColorMap.Item(lDesc)
    '							lDesc = ""
    '							On Error GoTo 0
    '							'UPGRADE_ISSUE: PictureBox method pic.Line was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '							pic.Line (barPos, barbase) - (barWidth, -barHeight), colr, BF
    '							barPos = barPos + barWidth + 1
    '						End If
    '					Next lSource
    '				Else 'Draw only land uses in LegendOrder, in order and leaving spaces for ones that do not appear in this segment
    '					For	Each barDesc In LegendOrder
    '						barHeight = 0
    '						For	Each lSource In pSources
    '							If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
    '								If Not lSource.Source.Opn Is Nothing Then
    '                                    If lSource.Source.Opn.Description = barDesc Then
    '                                        barHeight = barHeight + lSource.MFact / barMaxVal * barbase
    '                                    End If
    '								End If
    '							End If
    '						Next lSource
    '						If barHeight > 0 Then
    '							On Error GoTo ColorNotFound
    '							'UPGRADE_WARNING: Couldn't resolve default property of object ColorMap(). Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '							colr = ColorMap.Item(barDesc)
    '							On Error GoTo 0
    '							'UPGRADE_ISSUE: PictureBox method pic.Line was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '							pic.Line (barPos, barbase) - (barWidth, -barHeight), colr, BF
    '						End If
    '						barPos = barPos + barWidth + 1
    '					Next barDesc
    '				End If
    '			Case LegendType.LegMet
    '				ReDim included(pUci.MetSegs.Count())
    '				If Not pMetSeg Is Nothing Then included(pMetSeg.Id) = True : myid = pMetSeg.Id Else myid = 0
    '				'myid = 0
    '				For	Each lSource In pSources
    '					If Not lSource.Source.Opn Is Nothing Then
    '						If Not lSource.Source.Opn.MetSeg Is Nothing Then
    '							If lSource.Source.Opn.Name <> "RCHRES" Then
    '								'myid = lSource.Source.Opn.MetSeg.Id
    '								included(lSource.Source.Opn.MetSeg.Id) = True
    '							End If
    '						End If
    '					End If
    '				Next lSource
    '				'UPGRADE_ISSUE: PictureBox method pic.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				'UPGRADE_ISSUE: PictureBox property pic.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				pic.CurrentX = pic.TextWidth("X")
    '				'UPGRADE_ISSUE: PictureBox method pic.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				'UPGRADE_ISSUE: PictureBox property pic.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				pic.CurrentY = (barbase - pic.TextHeight("X")) / 2
    '				started = False
    '				For sid = 1 To pUci.MetSegs.Count()
    '					If included(sid) Then
    '						'UPGRADE_ISSUE: PictureBox method pic.Print was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '						If started Then pic.Print(", ") Else started = True
    '						'underline if this met seg contribs to reach directly,
    '						'dont underline if this met seg contribs to reach only
    '						'indirectly through land segment
    '						If sid = myid Then pic.Font = VB6.FontChangeUnderline(pic.Font, True) Else pic.Font = VB6.FontChangeUnderline(pic.Font, False) ' .ForeColor = vbHighlight Else pic.ForeColor = vbButtonText
    '						'UPGRADE_ISSUE: PictureBox method pic.Print was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '						pic.Print(sid)
    '					End If
    '				Next 
    '				pic.Font = VB6.FontChangeUnderline(pic.Font, False)
    '			Case LegendType.LegPoint
    '				ReDim included(pUci.PointSources.Count())
    '				'Debug.Print pPointSources.Count
    '				For	Each pPoint In pPointSources
    '					included(pPoint.Id) = True
    '				Next pPoint
    '				'UPGRADE_ISSUE: PictureBox method pic.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				'UPGRADE_ISSUE: PictureBox property pic.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				pic.CurrentX = pic.TextWidth("X")
    '				'UPGRADE_ISSUE: PictureBox method pic.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				'UPGRADE_ISSUE: PictureBox property pic.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '				pic.CurrentY = (barbase - pic.TextHeight("X")) / 2
    '				For sid = 1 To pUci.PointSources.Count()
    '					If included(sid) Then
    '						'UPGRADE_ISSUE: PictureBox method pic.Print was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '						If started Then pic.Print(", ") Else started = True
    '						'UPGRADE_ISSUE: PictureBox method pic.Print was not upgraded. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '						pic.Print(sid)
    '					End If
    '				Next 
    '		End Select
    '		'  With frmPictures
    '		'    If pOpnBlk.Name = "RCHRES" Then
    '		'      If pTables("GEN-INFO").ParmValue("LKFG") = 1 Then 'get the lake picture
    '		'        pic.PaintPicture .picLake.Picture, pic.Width - .picLake.Width, 0, , , , , , barbase
    '		'      Else
    '		'        pic.PaintPicture .picStream.Picture, pic.Width - .picStream.Width, 0, , , , , , barbase
    '		'      End If
    '		'    ElseIf pOpnBlk.Name = "BMPRAC" Then
    '		'      pic.PaintPicture .picBMP.Picture, pic.Width - .picBMP.Width, 0, , , , , , barbase
    '		'    Else
    '		'      'don't know what picture to use
    '		'    End If
    '		'  End With

    '		Exit Sub
    'ColorNotFound: 
    '		lTemp = UCase(lDesc)
    '		If Len(lTemp) = 0 Then 'changed to use bardesc, pbd
    '			'UPGRADE_WARNING: Couldn't resolve default property of object barDesc. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '			lTemp = UCase(barDesc)
    '		End If
    '		If InStr(lTemp, "FOREST") > 0 Or InStr(lTemp, "WOOD") > 0 Then
    '			ColorMap.Add(ColorMap.Item("FOREST"), lTemp)
    '		ElseIf InStr(lTemp, "AGRI") > 0 Or InStr(lTemp, "FARM") > 0 Then 
    '			ColorMap.Add(ColorMap.Item("AGRICULTURAL"), lTemp)
    '		ElseIf InStr(lTemp, "CROP") > 0 Then 
    '			ColorMap.Add(ColorMap.Item("AGRICULTURAL"), lTemp)
    '		ElseIf InStr(lTemp, "URBAN") > 0 Or InStr(lTemp, "INDU") > 0 Then 
    '			ColorMap.Add(ColorMap.Item("URBAN"), lTemp)
    '		ElseIf InStr(lTemp, "WATER") > 0 Then 
    '			ColorMap.Add(ColorMap.Item("WATERWETLANDS"), lTemp)
    '		ElseIf InStr(lTemp, "RESIDENTIAL") > 0 Then 
    '			ColorMap.Add(ColorMap.Item("RESIDENTIAL"), lTemp)
    '		Else
    '			ColorMap.Add(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), lTemp)
    '		End If
    '		err.Clear()
    '		Resume 
    '	End Sub

    'Returns color for source.VolId
    Private Function IdColor(ByRef Id As Integer) As Integer
        IdColor = RGB(Rnd(-Id - 53) * 255, Rnd(-Id - 27) * 255, Rnd(-Id - 33) * 255)
    End Function

    'Returns percent (0..1) given a source.VolId and value
    'Private Function IdPercentRange(id As Long, Value As Single) As Single
    '  If Value < 0 Then Value = -Value
    '  If Value < 1 Then
    '    IdPercentRange = Value
    '  ElseIf Value < 10 Then
    '    IdPercentRange = Value / 10
    '  ElseIf Value < 100 Then
    '    IdPercentRange = Value / 100
    '  ElseIf Value < 1000 Then
    '    IdPercentRange = Value / 1000
    '  ElseIf Value < 10000 Then
    '    IdPercentRange = Value / 10000
    '  ElseIf Value < 100000 Then
    '    IdPercentRange = Value / 100000
    '  ElseIf Value < 1000000 Then
    '    IdPercentRange = Value / 1000000
    '  ElseIf Value < 10000000 Then
    '    IdPercentRange = Value / 10000000
    '  End If
    'End Function

    Public Sub New()
        MyBase.New()
        'Debug.Print "init HspfOperation"
        pTables = New Collection
        pSources = New Collection
        pTargets = New Collection
        pPointSources = New Collection(Of HspfPoint)
        pTableStatus = New HspfStatus
        pTableStatus.init(Me)
        pInputTimeseriesStatus = New HspfStatus
        pInputTimeseriesStatus.StatusType = HspfStatus.HspfStatusTypes.HspfInputTimeseries
        pInputTimeseriesStatus.init(Me)
        pOutputTimeseriesStatus = New HspfStatus
        pOutputTimeseriesStatus.StatusType = HspfStatus.HspfStatusTypes.HspfOutputTimeseries
        pOutputTimeseriesStatus.init(Me)
        pId = 0
        pOpTyp = 0
        pDescription = ""
        lastOperationSerial = lastOperationSerial + 1
        pSerial = lastOperationSerial
        pDefOpnId = 0
    End Sub
End Class