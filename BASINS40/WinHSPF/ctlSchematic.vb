Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

<System.Runtime.InteropServices.ComVisible(False)> Public Class ctlSchematic

    Private Enum LegendType
        LegLand = 0
        LegMet = 1
        LegPoint = 2
    End Enum

    Private pDragging As Boolean = False
    Private pDragOffset As Point
    'Private pBeforeDragLocation As Point

    Private Class clsIcon
        Inherits Windows.Forms.Control

        Public Selected As Boolean
        Public pOperation As HspfOperation
        Public UpstreamIcons As New Generic.List(Of clsIcon)
        Public Key As String = ""

        Public Function Center() As Point
            Return New Point(Me.Left + Me.Width / 2, Me.Top + Me.Height / 2)
        End Function

        Public Property Operation() As HspfOperation
            Get
                Return pOperation
            End Get
            Set(ByVal newValue As HspfOperation)
                pOperation = newValue
                If pOperation Is Nothing Then
                    Key = ""
                Else
                    Key = OperationKey(pOperation)
                End If
            End Set
        End Property
    End Class

    Private Class IconCollection
        Inherits KeyedCollection(Of String, clsIcon)
        Protected Overrides Function GetKeyForItem(ByVal item As clsIcon) As String
            Return item.Key
        End Function
    End Class

    Private pUci As atcUCI.HspfUci
    Private pOperationTypesToInclude As New Generic.List(Of String)
    Private pIcons As New IconCollection
    Private ColorMap As atcCollection '(Of Color)
    Private CurrentLegend As LegendType = LegendType.LegLand
    Private LegendOrder As Generic.List(Of String)
    Private TreeProblemMessageDisplayed As Boolean = False
    Private TreeLoopMessageDisplayed As Boolean = False
    Private LandSelected() As Boolean
    Private HighlightBrush As Brush = Brushes.Aquamarine
    Private picLegend As atcCollection
    Private LegendScrollPos As Integer
    Private LegendFullHeight As Integer
    Private MetSelected As Integer
    Private pIconWidth As Integer = 73
    Private pIconHeight As Integer = 41
    Private pBorderWidth As Integer = 3

    Public Property UCI() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal newValue As HspfUci)
            pUci = newValue
            BuildTree()
        End Set
    End Property

    Public Sub BuildTree(Optional ByRef aPrinting As Boolean = False)
        ClearTree()
        If pUci IsNot Nothing Then

            Dim lNodeSize As New Drawing.Size(pIconWidth, pIconHeight)

            Dim lOperation As atcUCI.HspfOperation
            For Each lOperation In pUci.OpnSeqBlock.Opns
                If pOperationTypesToInclude.Contains(lOperation.Name) Then
                    Dim lNewIcon As New clsIcon
                    With lNewIcon
                        .Size = lNodeSize
                        .BackColor = Color.SkyBlue
                        .Operation = lOperation
                    End With
                    AddHandler lNewIcon.MouseDown, AddressOf Icon_MouseDown
                    AddHandler lNewIcon.MouseMove, AddressOf Icon_MouseMove
                    AddHandler lNewIcon.MouseUp, AddressOf Icon_MouseUp
                    pIcons.Add(lNewIcon)
                    picTree.Controls.Add(lNewIcon)
                End If
            Next

            If pIcons.Count > 0 Then 'okay to do tree

                Dim lOutlets As New IconCollection
                Dim lMaximumTreeDepth As Integer = 1
                Dim lDepth As Integer
                Dim lIcon As clsIcon
                Dim lKey As String

                For Each lIcon In pIcons
                    lDepth = DownLayers(lIcon, lIcon.Key & vbCrLf)
                    If lDepth > lMaximumTreeDepth Then
                        lMaximumTreeDepth = lDepth
                    End If
                    If lDepth = 1 Then
                        lOutlets.Add(lIcon)
                    End If
                Next

                Dim halfBoxHeight As Integer = pIconHeight / 2
                Dim dy As Integer
                Dim halfBoxWidth As Integer = pIconWidth / 2
                Dim drawsurface As Control
                'Dim pic As System.Windows.Forms.Control

                'If Printing Then
                '    ''UPGRADE_ISSUE: Constant vbPixels was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
                '    ''UPGRADE_ISSUE: Printer property Printer.ScaleMode was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                '    'Printer.ScaleMode = vbPixels
                '    ''UPGRADE_ISSUE: Printer property Printer.ScaleWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                '    'picBuffer.Width = Printer.ScaleWidth / 6
                '    ''UPGRADE_ISSUE: Printer property Printer.ScaleHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                '    'picBuffer.Height = Printer.ScaleHeight / 20
                '    ''UPGRADE_ISSUE: Printer property Printer.ScaleHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                '    'dy = Printer.ScaleHeight / (maxlayer + 1)
                '    ''UPGRADE_ISSUE: Printer object was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
                '    'drawsurface = Printer
                '    'pic = picBuffer
                'Else
                dy = (picTree.Height - 20) / lMaximumTreeDepth
                If dy < pIconHeight * 1.5 Then
                    dy = pIconHeight * 1.5
                    'TODO: enable vertical scrolling
                End If
                drawsurface = picTree
                'pic = pIcons(0)
                'End If 'Printing

                Dim Ybase As Integer = drawsurface.Height - dy / 2
                Dim lWidthPerOutlet As Integer = (drawsurface.Width - 20) / lOutlets.Count
                Dim Xbase As Integer = lWidthPerOutlet

                For Each lOutlet As clsIcon In lOutlets
                    LayoutFromIcon(lOutlet, Xbase, Ybase, dy, lWidthPerOutlet, aPrinting)
                    Xbase += lWidthPerOutlet
                Next

                'Dim iCur, i, j As Integer
                'Dim iTrib As New Generic.List(Of Integer)
                'Dim iNow As New Generic.List(Of Integer)
                'Dim iTotalTrib As New Generic.List(Of String)
                'Dim lSrcOpn, lTarOpn As atcUCI.HspfOperation
                'Dim lSrc As atcUCI.HspfConnection
                'Dim ifinish, istart, workingwidth As Integer
                'Dim ifound As Boolean
                'Dim Xbase, newstart, curline, newfinish
                'lOperation = pOperations.Item(pOperations.Count - 1)
                'Dim PicTop(pIcons.Count) As Integer
                'Dim PicLeft(pIcons.Count) As Integer
                ''PicTop(0) = Ybase - halfBoxHeight
                'PicLeft(0) = drawsurface.Width / 2 - halfBoxWidth

                'DrawPictureOnReachControl(lOperation, Printing, pIcons(0))

                'If Printing Then
                '    'UPGRADE_ISSUE: PictureBox property pic.Image was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                '    'UPGRADE_WARNING: Couldn't resolve default property of object drawsurface.PaintPicture. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                '    'drawsurface.PaintPicture(pic.Image, PicLeft(0), PicTop(0))
                'Else
                '    pic.Top = PicTop(0)
                '    pic.Left = PicLeft(0) '+ pic.width
                '    pic.Tag = pOperations.Count()
                'End If
                ''                'initialize before big tree loop
                '                istart = 0
                '                ifinish = 0
                '                curline = 0
                '                newstart = 0
                '                newfinish = 0

                '10:             'start of tree loop
                '                newstart = 0
                '                newfinish = 0

                '                'ypos = Ybase - istart * dy 'lineT(istart).Y1 - dy
                '                For j = istart To ifinish 'loop the top layer of branches already placed - count next layer
                '                    iTrib.Clear()
                '                    iCur = CInt(pIcons(j).Tag) - 1
                '                    lTarOpn = pOperations.Item(iCur)
                '                    For i = iCur - 1 To 0 Step -1
                '                        lSrcOpn = pOperations.Item(i)
                '                        For Each lSrc In lTarOpn.Sources
                '                            If lSrcOpn.Name = lSrc.Source.VolName AndAlso lSrcOpn.Id = lSrc.Source.VolId Then
                '                                'found a trib to this branch
                '                                ifound = False
                '                                '            For k = 1 To itribcnt 'check to see if we already have it
                '                                '              If iTrib(k) = i Then
                '                                '                ifound = True       fix 092602 to catch all mult exit situations
                '                                '              End If
                '                                '            Next k
                '                                Dim lTribKey As String = lSrcOpn.Name & lSrcOpn.Id
                '                                If iTotalTrib.Contains(lTribKey) Then
                '                                    'TODO: show multiple exits from a reach
                '                                    'we can't do tree diagram with multiple exits from a reach,
                '                                    'just skip this connection on tree diagram for now
                '                                    If TreeProblemMessageDisplayed = False Then
                '                                        MsgBox("A reach with multiple exits has been detected." & vbCrLf & "Only the first exit will be drawn in the tree diagram.", MsgBoxStyle.OkOnly, "WinHSPF Problem")
                '                                        TreeProblemMessageDisplayed = True
                '                                    End If
                '                                Else
                '                                    iTrib.Add(i)
                '                                    iNow.Add(j)
                '                                    iTotalTrib.Add(lTribKey)
                '                                End If
                '                            End If
                '                        Next lSrc
                '                    Next i
                '                    If iTrib.Count = 0 Then 'space holder
                '                        iTrib.Add(0)
                '                        iNow.Add(0)
                '                    End If
                '                Next j

                '                workingwidth = drawsurface.Width / iTrib.Count
                '                Xbase = workingwidth / 2
                '                For i = 0 To iTrib.Count - 1 'loop to place each trib to this branch
                '                    If iTrib(i) > 0 Then 'not a space holder
                '                        curline = curline + 1
                '                        If newstart = 0 Then newstart = curline 'the first branch on this row
                '                        newfinish = curline
                '                        j = iNow(i)
                '                        If Not Printing Then pic = pIcons(curline)
                '                        PicTop(curline) = PicTop(j) - dy
                '                        PicLeft(curline) = Xbase - halfBoxWidth

                '                        DrawPictureOnReachControl(pOperations.Item(iTrib(i)), Printing, pic)
                '                        With pic
                '                            If Printing Then
                '                                'TODO: drawsurface.PaintPicture(.Image, PicLeft(curline), PicTop(curline))
                '                            Else
                '                                .Top = PicTop(curline)
                '                                .Left = PicLeft(curline) '+ .width
                '                                .Tag = iTrib(i)
                '                                Logger.Dbg("Placing " & .Tag & " at (" & .Left & ", " & .Top & ")")
                '                                .Visible = True
                '                            End If
                '                        End With
                '                    End If
                '                    Xbase = Xbase + workingwidth
                '                Next i

                '                If newstart > 0 Then 'another row of branches to do
                '                    istart = newstart
                '                    ifinish = newfinish
                '                End If
                '                If newstart > 0 Then GoTo 10

                If aPrinting Then
                    'UPGRADE_ISSUE: Printer method Printer.EndDoc was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                    'Printer.EndDoc()
                Else
                    DrawTreeBackground()
                    picTree.Visible = True
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Draw the connecting lines and selection halos behind the tree nodes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DrawTreeBackground()
        Dim lBitmap As Bitmap = New Bitmap(picTree.Width, picTree.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim lGraphics As Graphics = Graphics.FromImage(lBitmap)
        Dim lLinesPen As Pen = Pens.Black

        For Each lIcon As clsIcon In pIcons
            With lIcon
                Dim lIconCenter As Point = .Center
                For Each lUpstreamIcon As clsIcon In lIcon.UpstreamIcons
                    lGraphics.DrawLine(lLinesPen, lIconCenter, lUpstreamIcon.Center)
                Next
                If .Selected Then
                    lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth - 1, .Top - pBorderWidth - 1, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
                End If
            End With
        Next
        lGraphics.Dispose()
        picTree.BackgroundImage = lBitmap
    End Sub

    Private Sub DrawPictureOnReachControl(ByVal aOperation As HspfOperation, ByVal aPrinting As Boolean, ByVal aControl As Control)
        Dim lBitmap As New Bitmap(pIconWidth, pIconHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(lBitmap)
        setPicture(aOperation, g)
        drawBorder(g, Not aPrinting)
        g.Dispose()
        aControl.BackgroundImage = lBitmap
    End Sub

    'Draw icon representing aOperation into given graphics object
    Public Sub setPicture(ByVal aOperation As HspfOperation, ByVal g As Graphics)
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
        lStr = OperationKey(aOperation)

        'TODO: pic.ToolTipText = pOpnBlk.Name & " " & pId & " " & pDescription
        g.Clear(Drawing.SystemColors.Control)

        Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(lStr, Me.Font)
        Dim lX As Single = (pIconWidth - lStringMeasurement.Width) / 2
        Dim lY As Single = pIconHeight - lStringMeasurement.Height * 1.25
        barbase = lY
        g.DrawString(lStr, Me.Font, Brushes.Black, lX, lY)
        Dim myid As Integer
        Dim pPoint As HspfPointSource
        Select Case CurrentLegend
            Case LegendType.LegLand
                barMaxVal = pUci.MaxAreaByLand2Stream
                barPos = barWidth
                If LegendOrder Is Nothing Then 'Draw all in the order they fall
                    For Each lSource In aOperation.Sources
                        If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
                            barHeight = lSource.MFact / barMaxVal * barbase
                            On Error GoTo ColorNotFound
                            lDesc = lSource.Source.Opn.Description
                            Dim lPen As New Pen(CType(ColorMap.ItemByKey(lDesc), Color))
                            lDesc = ""
                            On Error GoTo 0
                            g.DrawLine(lPen, barPos, barbase, barPos + barWidth, barbase - barHeight)
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
                            g.DrawLine(lPen, barPos, barbase, barPos + barWidth, barbase - barHeight)
                        End If
                        barPos = barPos + barWidth + 1
                    Next barDesc
                End If
            Case LegendType.LegMet
                ReDim included(pUci.MetSegs.Count)
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

                lStringMeasurement = g.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (barbase - lStringMeasurement.Height) / 2
                started = False
                For sid = 1 To pUci.MetSegs.Count
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
                        g.DrawString(sid, Me.Font, Brushes.Black, lX, lY)
                        lX += g.MeasureString(sid, Me.Font).Width
                    End If
                Next
                'pic.Font.Underline = False
            Case LegendType.LegPoint
                ReDim included(pUci.PointSources.Count)
                'Debug.Print pPointSources.Count
                For Each pPoint In aOperation.PointSources
                    included(pPoint.Id) = True
                Next pPoint
                lStringMeasurement = g.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (barbase - lStringMeasurement.Height) / 2
                For sid = 1 To pUci.PointSources.Count
                    If included(sid) Then
                        Dim lStrPrint As String
                        If started Then
                            lStrPrint = ", " & sid
                        Else
                            lStrPrint = sid
                            started = True
                        End If
                        g.DrawString(sid, Me.Font, Brushes.Black, lX, lY)
                        lX += g.MeasureString(sid, Me.Font).Width
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
            ColorMap.Add(ColorMap.ItemByKey("FOREST"), lTemp)
        ElseIf InStr(lTemp, "AGRI") > 0 Or InStr(lTemp, "FARM") > 0 Then
            ColorMap.Add(ColorMap.ItemByKey("AGRICULTURAL"), lTemp)
        ElseIf InStr(lTemp, "CROP") > 0 Then
            ColorMap.Add(ColorMap.ItemByKey("AGRICULTURAL"), lTemp)
        ElseIf InStr(lTemp, "URBAN") > 0 Or InStr(lTemp, "INDU") > 0 Then
            ColorMap.Add(ColorMap.ItemByKey("URBAN"), lTemp)
        ElseIf InStr(lTemp, "WATER") > 0 Then
            ColorMap.Add(ColorMap.ItemByKey("WATERWETLANDS"), lTemp)
        ElseIf InStr(lTemp, "RESIDENTIAL") > 0 Then
            ColorMap.Add(ColorMap.ItemByKey("RESIDENTIAL"), lTemp)
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
        pic.DrawLine(lPen, 0, 0, 0, pIconHeight)
        pic.DrawLine(lPen, 0, 0, pIconWidth, 0)
        If threeD Then
            lPen = New Pen(System.Drawing.SystemColors.ControlDark)
            'Else
            '    lPen = New Pen(System.Drawing.Color.Black)
        End If
        pic.DrawLine(lPen, 0, pIconHeight - 1, pIconWidth - 1, pIconHeight - 1)
        pic.DrawLine(lPen, pIconWidth - 1, 0, pIconWidth - 1, pIconHeight - 1)
    End Sub

    'height of tree of this plus all downstream operations. 1=none downstream
    Private Function DownLayers(ByVal aIcon As clsIcon, ByVal aAlreadyInPath As String) As Integer
        Dim lDeepestTarget As Integer = 0
        For Each lTarConn As atcUCI.HspfConnection In aIcon.Operation.Targets
            Dim lTarOperation As HspfOperation = lTarConn.Target.Opn
            Dim lKey As String = OperationKey(lTarOperation)
            If aAlreadyInPath.Contains(lKey & vbCrLf) AndAlso Not TreeLoopMessageDisplayed Then
                Dim lMessage As String = aAlreadyInPath
                Logger.Msg(aAlreadyInPath, "Detected loop in operations")
                TreeLoopMessageDisplayed = True
            Else
                If pIcons.Contains(lKey) Then
                    aAlreadyInPath &= lKey & vbCrLf
                    Dim lIcon As clsIcon = pIcons(lKey)
                    lIcon.UpstreamIcons.Add(aIcon)
                    Dim lTargetDepth As Integer = DownLayers(lIcon, aAlreadyInPath)
                    If lTargetDepth > lDeepestTarget Then
                        lDeepestTarget = lTargetDepth
                    End If
                End If
            End If
        Next lTarConn
        Return lDeepestTarget + 1
    End Function

    Private Sub LayoutFromIcon(ByVal aIcon As clsIcon, ByVal aX As Integer, ByVal aY As Integer, ByVal aDy As Integer, ByVal aWidth As Integer, ByVal aPrinting As Boolean)
        DrawPictureOnReachControl(aIcon.Operation, aPrinting, aIcon)
        aIcon.Top = aY
        aIcon.Left = aX
        Dim lNumUpstream As Integer = aIcon.UpstreamIcons.Count
        If lNumUpstream > 0 Then
            aY -= aDy
            Dim lWidthPerUpstream As Integer = aWidth / lNumUpstream
            For Each lUpstreamIcon As clsIcon In aIcon.UpstreamIcons
                LayoutFromIcon(lUpstreamIcon, aX, aY, aDy, lWidthPerUpstream, aPrinting)
                aX -= lWidthPerUpstream
            Next
        End If
    End Sub

    Public Sub ClearTree()
        picTree.Controls.Clear()
        For Each lControl As Control In pIcons
            lControl.Dispose()
        Next
        pIcons.Clear()
        If picTree.BackgroundImage IsNot Nothing Then
            picTree.BackgroundImage.Dispose()
            picTree.BackgroundImage = Nothing 'New Bitmap(0, 0, Drawing.Imaging.PixelFormat.Format32bppArgb)
        End If
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
                If pUci.Name <> "" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object myUci.OpnSeqBlock.Opns.Count. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    For oprindex = 1 To pUci.OpnSeqBlock.Opns.Count
                        lOper = pUci.OpnSeqBlock.Opn(oprindex)
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
                For Each lmetseg In pUci.MetSegs
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
                For Each lPoint In pUci.PointSources
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
                    'If picLegend(i).Point(0, 0) = HighlightColor Then LegendSelected = True
                    Exit Function
                End If
            Next
        End If
    End Function

    Private Sub SetLegendScrollButtons()

        'If LegendScrollPos > 0 Then
        '    btnScrollLegendUp.Visible = True
        'Else
        '    btnScrollLegendUp.Visible = False
        'End If

        'If LegendFullHeight - LegendScrollPos > Height Then
        '    btnScrollLegendDown.Visible = True
        'Else
        '    btnScrollLegendDown.Visible = False
        'End If

    End Sub

    Private Sub ScrollLegend()
        Dim ypos, boxHeight, txtHeight, Index As Integer
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

    Private Sub btnScrollLegendUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LegendScrollPos -= Height / 4
        If LegendScrollPos < 0 Then LegendScrollPos = 0
        ScrollLegend()
    End Sub

    Private Sub btnScrollLegendDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LegendScrollPos += Height / 4
        If LegendScrollPos > LegendFullHeight Then LegendScrollPos = LegendFullHeight
        ScrollLegend()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        pOperationTypesToInclude.Add("RCHRES")
        pOperationTypesToInclude.Add("BMPRAC")
        InitColorMap()
    End Sub

    Private Sub InitColorMap()
        ColorMap = New atcCollection
        ColorMap.Add("WaterWetlands", TextOrNumericColor("blue"))
        ColorMap.Add("Water/Wetland", TextOrNumericColor("blue"))
        ColorMap.Add("Construction", TextOrNumericColor("red"))
        ColorMap.Add("Institutional", TextOrNumericColor("azure"))
        ColorMap.Add("Agricultural", TextOrNumericColor("green"))
        ColorMap.Add("Forest/Open", TextOrNumericColor("forestgreen1"))
        ColorMap.Add("Forest", TextOrNumericColor("forestgreen1"))
        ColorMap.Add("Commercial", TextOrNumericColor("brickred"))
        ColorMap.Add("Urban", TextOrNumericColor("gray"))
        ColorMap.Add("LowResidential", TextOrNumericColor("goldenrod"))
        ColorMap.Add("MedResidential", TextOrNumericColor("orange"))
        ColorMap.Add("Residential", TextOrNumericColor("orange"))
        ColorMap.Add("MultResidential", TextOrNumericColor("orangered"))
        ColorMap.Add("HIGH TILL CROPLAND", TextOrNumericColor("darkbrown"))
        ColorMap.Add("LOW TILL CROPLAND", TextOrNumericColor("brown"))
        ColorMap.Add("Pasture", TextOrNumericColor("lightgreen"))
        ColorMap.Add("Hay", TextOrNumericColor("lemonchiffon"))
        ColorMap.Add("Animal/Feedlot", TextOrNumericColor("pink"))
    End Sub

    Private Sub tabLeft_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tabLeft.Selected
        Select Case e.TabPageIndex
            Case 0 : CurrentLegend = LegendType.LegLand
            Case 1 : CurrentLegend = LegendType.LegMet
            Case 2 : CurrentLegend = LegendType.LegPoint
        End Select
        Me.BuildTree()
    End Sub


    Private Sub Icon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            Dim MPosition As Point = Me.PointToClient(MousePosition)
            pDragging = True
            pDragOffset = sender.Location - MPosition
            Cursor.Clip = Me.RectangleToScreen(New Rectangle(picTree.Left + SplitLegendTree.SplitterDistance + SplitLegendTree.SplitterWidth + e.X, _
                                                             picTree.Top + e.Y, picTree.Width - pIconWidth, picTree.Height - pIconHeight))
        End If
    End Sub

    Private Sub Icon_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If pDragging Then
            'move control to new position
            Dim MPosition As Point = Me.PointToClient(MousePosition)
            MPosition.Offset(pDragOffset)
            sender.Location = MPosition
            DrawTreeBackground()
        End If
    End Sub

    Private Sub Icon_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                If pDragging Then 'end the dragging
                    pDragging = False
                    Cursor.Clip = Nothing
                    sender.Invalidate()
                End If
        End Select
    End Sub

    Private Sub ctlSchematic_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        BuildTree()
    End Sub
End Class
