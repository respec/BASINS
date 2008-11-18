Imports atcUCI
Imports atcUtility

Public Class ctlSchematic

    Private Enum LegendType
        LegLand = 0
        LegMet = 1
        LegPoint = 2
    End Enum

    Dim myUci As atcUCI.HspfUci
    Dim OpTyps() As String = {"RCHRES", "BMPRAC"}
    Dim pOperations As Collection 'Set of operations pictured in the main tree diagram
    Dim picReach As Generic.List(Of PictureBox)
    Private ColorMap As atcCollection '(Of Color)
    Private CurrentLegend As LegendType
    Private LegendOrder As Generic.List(Of String)
    Private TreeProblemMessageDisplayed As Boolean = False
    Private ReachSelected() As Boolean
    Private LandSelected() As Boolean
    Private HighlightColor As Color = Color.Aquamarine
    Private picLegend As atcCollection
    Private LegendScrollPos, LegendFullHeight As Integer
    Private MetSelected As Integer

    Public Sub BuildTree(Optional ByRef Printing As Boolean = False)

        Dim iCur, j, i, k, ypos As Integer
        Dim iTrib As New Generic.List(Of Integer)
        Dim iNow As New Generic.List(Of Integer)
        Dim iTotalTrib As New Generic.List(Of Integer)
        Dim iTotalTribOp As New Generic.List(Of String)
        Dim lSrcOpn, lOpn, lTarOpn As atcUCI.HspfOperation
        Dim lSrc As atcUCI.HspfConnection
        Dim ifinish, istart, workingwidth As Integer
        Dim ifound As Boolean
        Dim maxlayer, iLayer As Integer
        Dim itemp As Integer
        Dim Xbase, newstart, curline, newfinish, Ybase As Integer
        Dim loptyp As Object
        Dim halfBoxHeight, dy, halfBoxWidth As Integer
        Dim PicTop() As Integer
        Dim PicLeft() As Integer
        Dim drawsurface As Object
        Dim pic As System.Windows.Forms.PictureBox
        Dim lOpnBlk As atcUCI.HspfOpnBlk
        Dim icount As Integer
        Dim g As Graphics

        icount = 0
        If myUci.OpnBlks.Count > 0 Then
            For Each loptyp In OpTyps
                lOpnBlk = myUci.OpnBlks.Item(loptyp)
                icount = icount + lOpnBlk.Count
            Next loptyp
        End If

        If icount > 0 Then
            'okay to do tree
            pOperations = Nothing
            pOperations = New Collection

            If myUci.Name = "" Then Exit Sub

            'UPGRADE_WARNING: Couldn't resolve default property of object myUci.OpnSeqBlock.Opns.Count. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For j = 1 To myUci.OpnSeqBlock.Opns.Count 'or other bottom oper
                lOpn = myUci.OpnSeqBlock.Opns(j)
                For Each loptyp In OpTyps
                    'UPGRADE_WARNING: Couldn't resolve default property of object loptyp. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If lOpn.Name = loptyp Then
                        pOperations.Add(lOpn)
                        Exit For
                    End If
                Next loptyp
            Next j
            lOpn = pOperations.Item(pOperations.Count())

            'find max number of layers in tree
            maxlayer = 0
            For i = 1 To pOperations.Count() - 1
                iLayer = DownLayers(1, i, pOperations)
                If iLayer > maxlayer Then
                    maxlayer = iLayer
                End If
            Next i
            If maxlayer = 0 Then maxlayer = 1

            If Printing Then
                ''UPGRADE_ISSUE: Constant vbPixels was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
                ''UPGRADE_ISSUE: Printer property Printer.ScaleMode was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                'Printer.ScaleMode = vbPixels
                ''UPGRADE_ISSUE: Printer property Printer.ScaleWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                'picBuffer.Width = Printer.ScaleWidth / 6
                ''UPGRADE_ISSUE: Printer property Printer.ScaleHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                'picBuffer.Height = Printer.ScaleHeight / 20
                ''UPGRADE_ISSUE: Printer property Printer.ScaleHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                'dy = Printer.ScaleHeight / (maxlayer + 1)
                ''UPGRADE_ISSUE: Printer object was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
                'drawsurface = Printer
                'pic = picBuffer
            Else
                'TODO: picTree.Clear()
                picTree.Visible = True 'False
                dy = (picTree.Height - 20) / maxlayer
                drawsurface = picTree
                pic = picReach(0)
                g = Graphics.FromHwnd(pic.Handle)
            End If
            ReDim PicTop(picReach.Count)
            ReDim PicLeft(picReach.Count)

            halfBoxHeight = pic.Height / 2
            halfBoxWidth = pic.Width / 2
            'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.ScaleHeight. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Ybase = drawsurface.ScaleHeight - dy / 2
            PicTop(0) = Ybase - halfBoxHeight
            'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.ScaleWidth. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PicLeft(0) = drawsurface.ScaleWidth / 2 - halfBoxWidth

            setPicture(lOpn, g)
            drawBorder(g, Not Printing)
            If Printing Then
                'UPGRADE_ISSUE: PictureBox property pic.Image was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.PaintPicture. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                drawsurface.PaintPicture(pic.Image, PicLeft(0), PicTop(0))
            Else
                pic.Top = PicTop(0)
                pic.Left = PicLeft(0) '+ pic.width
                pic.Tag = pOperations.Count()
            End If
            'initialize before big tree loop
            istart = 0
            ifinish = 0
            curline = 0
            newstart = 0
            newfinish = 0

10:         'start of tree loop
            newstart = 0
            newfinish = 0

            'ypos = Ybase - istart * dy 'lineT(istart).Y1 - dy
            For j = istart To ifinish 'loop the top layer of branches already placed - count next layer
                iTrib.Clear()
                iCur = CInt(picReach(j).Tag)
                lTarOpn = pOperations.Item(iCur)
                For i = iCur - 1 To 1 Step -1
                    lSrcOpn = pOperations.Item(i)
                    For Each lSrc In lTarOpn.Sources
                        If lSrcOpn.Name = lSrc.Source.VolName And lSrcOpn.Id = lSrc.Source.VolId Then
                            'found a trib to this branch
                            ifound = False
                            '            For k = 1 To itribcnt 'check to see if we already have it
                            '              If iTrib(k) = i Then
                            '                ifound = True       fix 092602 to catch all mult exit situations
                            '              End If
                            '            Next k
                            For k = 0 To iTotalTrib.Count - 1 'check to see if we already have it
                                If iTotalTrib(k) = lSrcOpn.Id AndAlso iTotalTribOp(k) = lSrcOpn.Name Then
                                    ifound = True
                                End If
                            Next
                            If ifound Then
                                'TODO: show multiple exits from a reach
                                'we can't do tree diagram with multiple exits from a reach,
                                'just skip this connection on tree diagram for now
                                If TreeProblemMessageDisplayed = False Then
                                    MsgBox("A reach with multiple exits has been detected." & vbCrLf & "Only the first exit will be drawn in the tree diagram.", MsgBoxStyle.OkOnly, "WinHSPF Problem")
                                    TreeProblemMessageDisplayed = True
                                End If
                            Else
                                iTrib.Add(i)
                                iNow.Add(j)
                                iTotalTrib.Add(lSrcOpn.Id)
                                iTotalTribOp.Add(lSrcOpn.Name)
                            End If
                        End If
                    Next lSrc
                Next i
                If iTrib.Count = 0 Then 'space holder
                    iTrib.Add(0)
                    iNow.Add(0)
                End If
            Next j

            'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.ScaleWidth. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            workingwidth = drawsurface.ScaleWidth / iTrib.Count '* Screen.TwipsPerPixelX
            Xbase = workingwidth / 2
            For i = 0 To iTrib.Count - 1 'loop to place each trib to this branch
                If iTrib(i) > 0 Then 'not a space holder
                    curline = curline + 1
                    If newstart = 0 Then newstart = curline 'the first branch on this row
                    newfinish = curline
                    j = iNow(i)
                    If Not Printing Then pic = picReach(curline)
                    PicTop(curline) = PicTop(j) - dy
                    PicLeft(curline) = Xbase - halfBoxWidth
                    setPicture(pOperations.Item(iTrib(i)), g)
                    drawBorder(g, Not Printing)
                    'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.Line. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    drawsurface.Line(Xbase, PicTop(curline) + halfBoxHeight, PicLeft(j) + halfBoxWidth, PicTop(j) + halfBoxHeight)
                    With pic
                        If Printing Then
                            'UPGRADE_ISSUE: PictureBox property pic.Image was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                            'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.PaintPicture. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            drawsurface.PaintPicture(.Image, PicLeft(curline), PicTop(curline))
                        Else
                            .Top = PicTop(curline)
                            .Left = PicLeft(curline) '+ .width
                            .Tag = iTrib(i)
                            .Visible = True
                        End If
                    End With
                End If
                Xbase = Xbase + workingwidth
            Next i

            If newstart > 0 Then 'another row of branches to do
                istart = newstart
                ifinish = newfinish
            End If
            If newstart > 0 Then GoTo 10

            If Printing Then
                'UPGRADE_ISSUE: Printer method Printer.EndDoc was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                'Printer.EndDoc()
            Else
                ReDim Preserve ReachSelected(picReach.Count - 1)
                For i = 0 To picReach.Count - 1
                    SetReachSelected(i, ReachSelected(i))
                Next
                picTree.Visible = True
            End If
        End If
    End Sub

    Private Sub SetReachSelected(ByVal reachIndex&, ByVal sel As Boolean)
        Dim colr As Color, BorderWidth As Integer
        BorderWidth = 3
        With picReach(reachIndex)
            If sel Then colr = HighlightColor Else colr = picTree.BackColor
            '.FillStyle = vbFSSolid
            '.DrawStyle = vbSolid
            '.DrawWidth = 1
            'TODO: picTree.Line(colr, .Left - BorderWidth - 1, .Top - BorderWidth - 1, .Width + BorderWidth * 2, .Height + BorderWidth * 2)
            ReachSelected(reachIndex) = sel
        End With
    End Sub

    'Not needed until WinHSPF wants it
    Public Sub setPicture(ByVal aOperation As HspfOperation, ByVal pic As Graphics)
        Dim barbase, barHeight, sid, barPos, barWidth, maxNBars As Integer
        Dim lTemp As String
        Dim lStr, desc As String
        Dim barDesc As Object
        Dim lSource As HspfConnection
        Dim lDesc As String
        Dim colr As Color
        Dim barMaxVal As Double
        Dim started As Boolean
        Dim included() As Boolean

        barWidth = 3
        'pic.Caption = pOpnBlk.Name & " " & pId
        lStr = aOperation.OpnBlk.Name & " " & aOperation.Id

        'TODO: pic.ToolTipText = pOpnBlk.Name & " " & pId & " " & pDescription
        pic.Clear(Color.White)

        Dim lStringMeasurement As Drawing.SizeF = pic.MeasureString(lStr, Me.Font)
        Dim lX As Single = (pic.ClipBounds.Width - lStringMeasurement.Width) / 2
        Dim lY As Single = pic.ClipBounds.Height - lStringMeasurement.Height * 1.25
        barbase = lY
        pic.DrawString(lStr, Me.Font, Brushes.Black, lX, lY)
        Dim myid As Integer
        Dim pPoint As HspfPointSource
        Select Case CurrentLegend
            Case LegendType.LegLand
                barMaxVal = pUCI.MaxAreaByLand2Stream
                barPos = barWidth
                If LegendOrder Is Nothing Then 'Draw all in the order they fall
                    For Each lSource In aOperation.Sources
                        If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
                            barHeight = lSource.MFact / barMaxVal * barbase
                            On Error GoTo ColorNotFound
                            lDesc = lSource.Source.Opn.Description
                            Dim lPen As New Pen(CType(ColorMap(lDesc), Color))
                            lDesc = ""
                            On Error GoTo 0
                            pic.DrawLine(lPen, barPos, barbase, barPos + barWidth, barbase - barHeight)
                            barPos = barPos + barWidth + 1
                        End If
                    Next lSource
                Else 'Draw only land uses in LegendOrder, in order and leaving spaces for ones that do not appear in this segment
                    For Each barDesc In LegendOrder
                        barHeight = 0
                        For Each lSource In aOperation.Sources
                            If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
                                If Not lSource.Source.Opn Is Nothing Then
                                    If lSource.Source.Opn.Description = barDesc Then
                                        barHeight = barHeight + lSource.MFact / barMaxVal * barbase
                                    End If
                                End If
                            End If
                        Next lSource
                        If barHeight > 0 Then
                            On Error GoTo ColorNotFound
                            Dim lPen As New Pen(CType(ColorMap(barDesc), Color))
                            On Error GoTo 0
                            pic.DrawLine(lPen, barPos, barbase, barPos + barWidth, barbase - barHeight)
                        End If
                        barPos = barPos + barWidth + 1
                    Next barDesc
                End If
            Case LegendType.LegMet
                ReDim included(pUCI.MetSegs.Count)
                If Not aOperation.MetSeg Is Nothing Then
                    included(aOperation.MetSeg.Id) = True
                    myid = aOperation.MetSeg.Id
                Else
                    myid = 0
                End If
                'myid = 0
                For Each lSource In aOperation.Sources
                    If Not lSource.Source.Opn Is Nothing Then
                        If Not lSource.Source.Opn.MetSeg Is Nothing Then
                            If lSource.Source.Opn.Name <> "RCHRES" Then
                                'myid = lSource.Source.Opn.MetSeg.Id
                                included(lSource.Source.Opn.MetSeg.Id) = True
                            End If
                        End If
                    End If
                Next lSource

                lStringMeasurement = pic.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (barbase - lStringMeasurement.Height) / 2
                started = False
                For sid = 1 To pUCI.MetSegs.Count
                    If included(sid) Then
                        Dim lStrPrint As String
                        If started Then
                            lStrPrint = ", " & sid
                        Else
                            lStrPrint = sid
                            started = True
                        End If
                        'underline if this met seg contribs to reach directly,
                        'dont underline if this met seg contribs to reach only
                        'indirectly through land segment
                        'If sid = myid Then pic.Font = VB6.FontChangeUnderline(pic.Font, True) Else pic.Font = VB6.FontChangeUnderline(pic.Font, False) ' .ForeColor = vbHighlight Else pic.ForeColor = vbButtonText
                        pic.DrawString(sid, Me.Font, Brushes.Black, lX, lY)
                        lX += pic.MeasureString(sid, Me.Font).Width
                    End If
                Next
                'pic.Font.Underline = False
            Case LegendType.LegPoint
                ReDim included(pUCI.PointSources.Count)
                'Debug.Print pPointSources.Count
                For Each pPoint In aOperation.PointSources
                    included(pPoint.Id) = True
                Next pPoint
                lStringMeasurement = pic.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (barbase - lStringMeasurement.Height) / 2
                For sid = 1 To pUCI.PointSources.Count
                    If included(sid) Then
                        Dim lStrPrint As String
                        If started Then
                            lStrPrint = ", " & sid
                        Else
                            lStrPrint = sid
                            started = True
                        End If
                        pic.DrawString(sid, Me.Font, Brushes.Black, lX, lY)
                        lX += pic.MeasureString(sid, Me.Font).Width
                    End If
                Next
        End Select
        '  With frmPictures
        '    If pOpnBlk.Name = "RCHRES" Then
        '      If pTables("GEN-INFO").ParmValue("LKFG") = 1 Then 'get the lake picture
        '        pic.PaintPicture .picLake.Picture, pic.Width - .picLake.Width, 0, , , , , , barbase
        '      Else
        '        pic.PaintPicture .picStream.Picture, pic.Width - .picStream.Width, 0, , , , , , barbase
        '      End If
        '    ElseIf pOpnBlk.Name = "BMPRAC" Then
        '      pic.PaintPicture .picBMP.Picture, pic.Width - .picBMP.Width, 0, , , , , , barbase
        '    Else
        '      'don't know what picture to use
        '    End If
        '  End With

        Exit Sub
ColorNotFound:
        lTemp = UCase(lDesc)
        If Len(lTemp) = 0 Then 'changed to use bardesc, pbd
            'UPGRADE_WARNING: Couldn't resolve default property of object barDesc. Click for more: 'ms-help://MS.VSExpressCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lTemp = UCase(barDesc)
        End If
        If InStr(lTemp, "FOREST") > 0 Or InStr(lTemp, "WOOD") > 0 Then
            ColorMap.Add(ColorMap.Item("FOREST"), lTemp)
        ElseIf InStr(lTemp, "AGRI") > 0 Or InStr(lTemp, "FARM") > 0 Then
            ColorMap.Add(ColorMap.Item("AGRICULTURAL"), lTemp)
        ElseIf InStr(lTemp, "CROP") > 0 Then
            ColorMap.Add(ColorMap.Item("AGRICULTURAL"), lTemp)
        ElseIf InStr(lTemp, "URBAN") > 0 Or InStr(lTemp, "INDU") > 0 Then
            ColorMap.Add(ColorMap.Item("URBAN"), lTemp)
        ElseIf InStr(lTemp, "WATER") > 0 Then
            ColorMap.Add(ColorMap.Item("WATERWETLANDS"), lTemp)
        ElseIf InStr(lTemp, "RESIDENTIAL") > 0 Then
            ColorMap.Add(ColorMap.Item("RESIDENTIAL"), lTemp)
        Else
            ColorMap.Add(System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), lTemp)
        End If
        Err.Clear()
        Resume
    End Sub

    'Returns color for source.VolId
    Private Function IdColor(ByRef aColorId As Integer) As Integer
        Dim lIdColor As Integer = RGB(Rnd(-aColorId - 53) * 255, _
                                      Rnd(-aColorId - 27) * 255, _
                                      Rnd(-aColorId - 33) * 255)
        Return lIdColor
    End Function

    Private Sub drawBorder(ByRef pic As Graphics, ByRef threeD As Boolean)
        Dim lPen As Pen
        If threeD Then
            lPen = New Pen(SystemColors.ControlLightLight)
        Else
            lPen = New Pen(Color.Black)
        End If
        pic.DrawLine(lPen, 0, 0, 0, pic.ClipBounds.Height)
        pic.DrawLine(lPen, 0, 0, pic.ClipBounds.Width, 0)
        If threeD Then
            lPen = New Pen(System.Drawing.SystemColors.ControlDark)
            'Else
            '    lPen = New Pen(System.Drawing.Color.Black)
        End If
        pic.DrawLine(lPen, 0, pic.ClipBounds.Height - 1, pic.ClipBounds.Width - 1, pic.ClipBounds.Height - 1)
        pic.DrawLine(lPen, pic.ClipBounds.Width - 1, 0, pic.ClipBounds.Width - 1, pic.ClipBounds.Height - 1)
    End Sub


    Private Function DownLayers(ByRef iLayer As Integer, ByRef iOpnInd As Integer, ByRef lOpns As Collection) As Integer
        Dim lLayer, j, mLayer As Integer
        Dim lSrcOpn As atcUCI.HspfOperation
        Dim lTarOpn As atcUCI.HspfOperation
        Dim lTar As atcUCI.HspfConnection

        mLayer = iLayer
        lSrcOpn = lOpns.Item(iOpnInd)
        With lSrcOpn
            For Each lTar In .Targets
                For j = iOpnInd + 1 To lOpns.Count()
                    lTarOpn = lOpns.Item(j)
                    If lTarOpn.Name = lTar.Target.VolName And lTarOpn.Id = lTar.Target.VolId Then
                        lLayer = DownLayers(iLayer + 1, j, lOpns)
                        If lLayer > mLayer Then
                            mLayer = lLayer
                        End If
                    End If
                Next j
            Next lTar
        End With
        DownLayers = mLayer

    End Function

    Public Sub ClearTree()
        'Dim lOpnBlk As atcUCI.HspfOpnBlk
        'Dim loptyp As Object
        'Dim i, icount As Integer

        ''UPGRADE_ISSUE: PictureBox method picTree.Cls was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'picTree.Cls()
        'For i = 1 To picReach.UBound 'unload any previously loaded branches
        '    'Unload lineT(i)
        '    picReach.Unload(i)
        'Next i

        'icount = 0
        'For Each loptyp In OpTyps
        '    lOpnBlk = myUci.OpnBlks.Item(loptyp)
        '    icount = icount + lOpnBlk.Count
        'Next loptyp

        'For i = 1 To icount - 1
        '    'Load lineT(i)
        '    picReach.Load(i)
        'Next i
    End Sub

    Public Sub UpdateLegend(ByVal picTab As Graphics)
        Dim item As Object
        Dim Key As String
        Dim srch, Index, oprindex As Integer
        Dim colr, i As Integer
        Dim S As String
        Dim ypos, xpos, colonpos As Integer
        Dim boxWidth, boxHeight, txtHeight As Integer
        Dim lOper As atcUCI.HspfOperation
        Dim spos, maxpic, cpos As Integer
        Dim tname As String

        LegendOrder = Nothing

        picTab.Clear(Color.White)
        For Index = 0 To picLegend.Count - 1
            With picLegend(Index)
                .Tag = ""
                .Cls()
                .Visible = False
                .Width = picTab.ClipBounds.Width
            End With
        Next

        boxWidth = picTab.ClipBounds.Width * 0.4
        txtHeight = picTab.MeasureString("X", Me.Font).Height
        boxHeight = txtHeight * 2
        Index = 0
        ypos = 0 'boxHeight + txtHeight
        maxpic = 0
        Dim lPoint As atcUCI.HspfPointSource
        Dim lmetseg As atcUCI.HspfMetSeg
        Dim lX As Single = 0
        Dim lY As Single = 0
        Select Case CurrentLegend
            Case LegendType.LegLand
                LegendOrder = New Generic.List(Of String)
                'TODO: picTab.Font = VB6.FontChangeSize(picTab.Font, 8)
                If picTab.MeasureString("Perlnd   Implnd", Me.Font).Width > picTab.ClipBounds.Width Then
                    lX = (boxWidth - picTab.MeasureString("Per", Me.Font).Width) / 2
                    picTab.DrawString(" Per", Me.Font, Brushes.Black, lX, lY)
                    lX = picTab.ClipBounds.Width - ((boxWidth + picTab.MeasureString("Imp", Me.Font).Width) / 2)
                    picTab.DrawString("Imp ", Me.Font, Brushes.Black, lX, lY)
                Else
                    lX = (boxWidth - picTab.MeasureString("Perlnd", Me.Font).Width) / 2
                    picTab.DrawString(" Perlnd", Me.Font, Brushes.Black, lX, lY)
                    lX = picTab.ClipBounds.Width - ((boxWidth + picTab.MeasureString("Implnd", Me.Font).Width) / 2)
                    picTab.DrawString("Implnd ", Me.Font, Brushes.Black, lX, lY)
                End If
                ypos = txtHeight
                picLegend(0).Top = ypos
                If myUci.Name <> "" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object myUci.OpnSeqBlock.Opns.Count. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    For oprindex = 1 To myUci.OpnSeqBlock.Opns.Count
                        lOper = myUci.OpnSeqBlock.Opn(oprindex)
                        If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
                            Key = lOper.Description
                            colonpos = InStr(Key, ":")
                            If colonpos > 0 Then Key = Mid(Key, colonpos + 1)
                            For Index = 0 To picLegend.Count - 1
                                If Key = picLegend(Index).Tag Or picLegend(Index).Tag = "" Then Exit For
                            Next
                            If Index >= picLegend.Count Then
                                'TODO: picLegend.Load(Index)
                                picLegend(Index).Tag = ""
                                picLegend(Index).Width = picTab.ClipBounds.Width
                            End If
                            With picLegend(Index)
                                If Index > maxpic Then maxpic = Index
                                .Height = boxHeight * 1.5 + txtHeight
                                If .Tag = "" Then
                                    .Tag = Key
                                    .Top = ypos - LegendScrollPos
                                    ypos = ypos + .Height
                                    .Visible = True
                                End If
                                'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                .CurrentX = (.Width - .TextWidth(Key)) / 2
                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                If .CurrentX < 2 Then .CurrentX = 2
                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                .CurrentY = boxHeight * 1.5
                                'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                picLegend(Index).Print(Key)

                                On Error GoTo ColorError
                                'UPGRADE_WARNING: Couldn't resolve default property of object ColorMap(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                colr = ColorMap.Item(Key)
                                On Error GoTo 0
                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                .CurrentY = boxHeight / 4
                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                If lOper.Name = "PERLND" Then
                                    .CurrentX = 0
                                Else
                                    'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                    .CurrentX = .Width - boxWidth
                                End If
                                'UPGRADE_ISSUE: PictureBox method picLegend.Line was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                                'TODO: picLegend(Index).Line (boxWidth, boxHeight), colr, BF
                            End With
                        End If
                    Next
                End If
                LegendFullHeight = ypos
                ReDim Preserve LandSelected(maxpic)
                For Index = 0 To picLegend.Count - 1
                    If picLegend(Index).Tag <> "" Then LegendOrder.Add(picLegend(Index).Tag)
                Next Index
            Case LegendType.LegMet
                For Each lmetseg In myUci.MetSegs
                    'TODO: If Index >= picLegend.Count Then picLegend.Load(Index)
                    With picLegend(Index)
                        .Tag = Index + 1
                        i = InStr(1, lmetseg.Name, ",")
                        If i > 0 Then
                            S = Mid(lmetseg.Name, 1, i - 1) & vbCr & Mid(lmetseg.Name, i + 1)
                        Else
                            S = lmetseg.Name
                        End If
                        Key = lmetseg.Id & ":" & S
                        'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        .CurrentX = (.Width - .TextWidth(Key)) / 2
                        'UPGRADE_ISSUE: PictureBox method picLegend.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        .CurrentY = (.Height - .TextHeight(Key)) / 2
                        'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        picLegend(Index).Print(Key) 'Precip location name might be nicer
                        If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
                        .Visible = True
                    End With
                    Index = Index + 1
                Next lmetseg
                LegendFullHeight = picLegend(0).Height * (Index + 1)
            Case LegendType.LegPoint
                For Each lPoint In myUci.PointSources
                    Index = lPoint.Id - 1
                    'TODO: If Index >= picLegend.Count Then picLegend.Load(Index)
                    With picLegend(Index)
                        .Tag = Index + 1
                        'find a way to shorten name
                        spos = InStr(1, lPoint.Name, " ")
                        cpos = InStr(1, lPoint.Name, ",")
                        If spos < 10 And spos > 2 Then
                            tname = lPoint.Name.Substring(0, spos - 1)
                        ElseIf cpos < 10 And cpos > 2 Then
                            tname = lPoint.Name.Substring(0, cpos - 1)
                        Else
                            tname = lPoint.Name.Substring(0, 10)
                        End If
                        Key = lPoint.Id & ":" & tname
                        'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        .CurrentX = (.Width - .TextWidth(Key)) / 2
                        'UPGRADE_ISSUE: PictureBox method picLegend.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        .CurrentY = (.Height - .TextHeight(Key)) / 2
                        'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                        picLegend(Index).Print(Key)
                        If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
                        .Visible = True
                    End With
                    Index = Index + 1
                Next lPoint
                LegendFullHeight = picLegend(0).Height * (Index + 1)
            Case Else
        End Select
        SetLegendScrollButtons()
        RefreshLegendSelections()
        'UpdateDetails()
        Exit Sub
ColorError:
        colr = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
        Err.Clear()
        Resume Next
    End Sub

    Private Sub RefreshLegendSelections()
        Dim Index As Short
        For Index = 0 To UBound(LandSelected)
            DrawLegendSelectBox(Index, LegendSelected(CStr(Index)))
        Next Index
    End Sub

    'Private Sub picLegend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picLegend.Click
    '    Dim Index As Short = picLegend.GetIndex(eventSender)
    '    Dim tmp As Short
    '    Dim rch As Integer
    '    Dim vConn As Object
    '    Dim i As Integer
    '    Dim lConn As ATCoHspf.HspfConnection
    '    Dim lOper As ATCoHspf.HspfOperation
    '    Dim lPoint As ATCoHspf.HspfPoint
    '    Select Case CurrentLegend
    '        Case LegendType.LegLand
    '            LandSelected(Index) = Not LandSelected(Index)
    '            DrawLegendSelectBox(Index, LandSelected(Index))
    '        Case LegendType.LegMet
    '            If MetSelected <> Index Then
    '                DrawLegendSelectBox(MetSelected, False)
    '                MetSelected = Index
    '            End If
    '            DrawLegendSelectBox(MetSelected, True)
    '            For rch = 0 To picReach.Count - 1
    '                If IsNumeric(picReach(rch).Tag) Then
    '                    lOper = lOpns.Item(CInt(picReach(rch).Tag))
    '                    If lOper.MetSeg Is Nothing Then
    '                        '          For Each vConn In lOper.Sources
    '                        '              Set lConn = vConn
    '                        '              If lConn.source.volname = "PERLND" Or lConn.source.volname = "IMPLND" Then
    '                        '                if lconn.Parent.m
    '                        SetReachSelected(rch, False)
    '                    Else
    '                        If lOper.MetSeg.Id = MetSelected + 1 Then
    '                            SetReachSelected(rch, True)
    '                        Else
    '                            SetReachSelected(rch, False)
    '                        End If
    '                    End If
    '                End If
    '            Next rch
    '        Case LegendType.LegPoint
    '            If PointSelected <> Index Then
    '                DrawLegendSelectBox(PointSelected, False)
    '                PointSelected = Index
    '            End If
    '            DrawLegendSelectBox(PointSelected, True)
    '            For rch = 0 To picReach.Count - 1
    '                If IsNumeric(picReach(rch).Tag) Then
    '                    lOper = lOpns.Item(CInt(picReach(rch).Tag))
    '                    If lOper.PointSources Is Nothing Or lOper.PointSources.Count = 0 Then
    '                        SetReachSelected(rch, False)
    '                    Else
    '                        i = 0
    '                        SetReachSelected(rch, False)
    '                        For Each lPoint In lOper.PointSources
    '                            i = i + 1
    '                            'UPGRADE_WARNING: Couldn't resolve default property of object lOper.PointSources(i).Id. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '                            If lOper.PointSources.Item(i).Id = PointSelected + 1 Then
    '                                SetReachSelected(rch, True)
    '                            End If
    '                        Next lPoint
    '                    End If
    '                End If
    '            Next rch
    '    End Select
    'End Sub

    Private Sub DrawLegendSelectBox(ByRef Index As Object, ByRef Selected As Boolean)
        '     Dim colr As Integer
        '     With picLegend(Index)
        '         If Selected Then colr = HighlightColor Else colr = System.Drawing.ColorTranslator.ToOle(picLegend(Index).BackColor)
        '         'UPGRADE_ISSUE: Constant vbFSTransparent was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
        '         'UPGRADE_ISSUE: PictureBox property picLegend.FillStyle was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        '         .FillStyle = vbFSTransparent
        '         'UPGRADE_ISSUE: Constant vbSolid was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
        '         'UPGRADE_ISSUE: PictureBox property picLegend.DrawStyle was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        '         .DrawStyle = vbSolid
        '         'UPGRADE_ISSUE: PictureBox property picLegend.DrawWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        '         .DrawWidth = 2
        '         'UPGRADE_ISSUE: PictureBox method picLegend.Line was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        'picLegend(Index).Line (1, 1) - (.Width - 1, .Height - 1), colr, B
        '         UpdateDetails()
        '     End With
    End Sub

    'UPGRADE_NOTE: tag was upgraded to tag_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function LegendSelected(ByVal tag_Renamed As String) As Boolean
        Dim i As Integer
        LegendSelected = False
        If IsNumeric(tag_Renamed) Then
            'If picLegend(CInt(tag)).Point(0, 0) = HighlightColor Then LegendSelected = True
            Select Case CurrentLegend
                Case LegendType.LegLand
                    If CShort(tag_Renamed) <= UBound(LandSelected) Then
                        LegendSelected = LandSelected(CShort(tag_Renamed))
                    End If
                Case LegendType.LegMet : If CShort(tag_Renamed) = MetSelected Then LegendSelected = True
            End Select
        Else
            For i = 0 To picLegend.Count - 1
                If tag_Renamed = picLegend(i).Tag Then
                    'UPGRADE_ISSUE: PictureBox method picLegend.Point was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    If picLegend(i).Point(0, 0) = HighlightColor Then LegendSelected = True
                    Exit Function
                End If
            Next
        End If
    End Function

    Private Sub SetLegendScrollButtons()

        If LegendScrollPos > 0 Then
            btnScrollLegendUp.Visible = True
        Else
            btnScrollLegendUp.Visible = False
        End If

        If LegendFullHeight - LegendScrollPos > Height Then
            btnScrollLegendDown.Visible = True
        Else
            btnScrollLegendDown.Visible = False
        End If

    End Sub

    Private Sub ScrollLegend()
        Dim ypos, boxHeight, txtHeight, Index As Integer
        'UPGRADE_ISSUE: PictureBox method picTab.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        txtHeight = 10
        boxHeight = txtHeight * 2
        ypos = txtHeight
        If LegendFullHeight - Height < LegendScrollPos Then
            LegendScrollPos = LegendFullHeight - Height
        End If
        If LegendScrollPos < 0 Then LegendScrollPos = 0
        For Index = 0 To picLegend.Count - 1
            picLegend(Index).Top = ypos - LegendScrollPos
            ypos = ypos + picLegend(Index).Height
        Next
        SetLegendScrollButtons()
    End Sub

    Private Sub btnScrollLegendUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendUp.Click
        LegendScrollPos -= Height / 4
        If LegendScrollPos < 0 Then LegendScrollPos = 0
        ScrollLegend()
    End Sub

    Private Sub btnScrollLegendDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollLegendDown.Click
        LegendScrollPos += Height / 4
        If LegendScrollPos > LegendFullHeight Then LegendScrollPos = LegendFullHeight
        ScrollLegend()
    End Sub
End Class
