Imports atcUCI
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel

<System.Runtime.InteropServices.ComVisible(False)> _
Friend Class ctlSchematic

    Friend WithEvents picTree As PanelDoubleBuffer
    Dim pHeightNeeded As Integer = 0
    Dim pWidthNeeded As Integer = 0

    Private pDragging As Boolean = False
    Private pDragOffset As Point
    Private pBeforeDragLocation As Point
    Private pClickedIcon As clsIcon

    Private pUci As HspfUci
    Private pOperationTypesInDiagram As New Generic.List(Of String)
    Private pIcons As New IconCollection
    Private pOutlets As New IconCollection
    Private pIconsDistantFromOutlet As New atcCollection
    Private pIconsDistantFromOutletPlaced As New atcCollection
    Private pMaximumTreeDepth As Integer
    Private ColorMap As atcCollection '(Of Color)

    Private TreeProblemMessageDisplayed As Boolean = False
    Private TreeLoopMessageDisplayed As Boolean = False
    Private HighlightBrush As Brush = SystemBrushes.Highlight
    Private LegendScrollPos As Integer
    Private LegendFullHeight As Integer
    Private pIconWidth As Integer = 73
    Private pIconHeight As Integer = 41
    Private pBorderWidth As Integer = 3
    Private pTreeBackground As Bitmap

    Private WithEvents pCurrentLegend As ctlLegend = Nothing
    Private LegendOrder As Generic.List(Of String)

    Public Property UCI() As HspfUci
        Get
            Return pUci
        End Get
        Set(ByVal newValue As HspfUci)
            pUci = newValue
            UpdateLegend()
            BuildTree()
            UpdateDetails()
        End Set
    End Property

    Public Sub BuildTree(Optional ByVal aPrinting As Boolean = False)
        ClearTree()
        If pUci IsNot Nothing Then

            Dim lNodeSize As New Drawing.Size(pIconWidth, pIconHeight)

            Dim lOperation As HspfOperation
            For Each lOperation In pUci.OpnSeqBlock.Opns
                If pOperationTypesInDiagram.Contains(lOperation.Name) Then
                    Dim lNewIcon As New clsSchematicIcon
                    With lNewIcon
                        .Size = lNodeSize
                        .BackColor = Color.SkyBlue
                        .ReachOrBMP = lOperation
                        .Key = OperationKey(lOperation)
                    End With
                    AddHandler lNewIcon.MouseDown, AddressOf Icon_MouseDown
                    AddHandler lNewIcon.MouseMove, AddressOf Icon_MouseMove
                    AddHandler lNewIcon.MouseUp, AddressOf Icon_MouseUp
                    pIcons.Add(lNewIcon)
                    picTree.Controls.Add(lNewIcon)
                End If
            Next

            If pIcons.Count > 0 Then 'okay to do tree

                Dim lIcon As clsSchematicIcon

                pMaximumTreeDepth = 1

                For Each lIcon In pIcons
                    With lIcon
                        .DistanceFromOutlet = DownLayers(lIcon, lIcon.Key & vbCrLf)
                        pIconsDistantFromOutlet.Increment(.DistanceFromOutlet, 1)
                        'Debug.WriteLine(.DistanceFromOutlet & ":" & pIconsDistantFromOutlet.ItemByKey(.DistanceFromOutlet))
                        If .DistanceFromOutlet > pMaximumTreeDepth Then
                            pMaximumTreeDepth = .DistanceFromOutlet
                        End If
                        If .DistanceFromOutlet = 1 Then
                            pOutlets.Add(lIcon)
                        End If
                    End With
                Next
                For Each lIcon In pIcons
                    For Each lUpIcon As clsSchematicIcon In lIcon.UpstreamIcons
                        lUpIcon.DownstreamIcons.Add(lIcon)
                    Next
                Next

                LayoutTree(aPrinting)
            End If
        End If
    End Sub

    Private Sub LayoutTree(Optional ByVal aPrinting As Boolean = False)
        Static lInLayout As Boolean = False
        If pIcons.Count > 0 Then 'okay to do tree
            If lInLayout Then Exit Sub
            lInLayout = True
            Try
                pIconsDistantFromOutletPlaced = New atcCollection
                Dim drawsurface As Control
                Dim dy As Integer

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
                If pMaximumTreeDepth > 1 Then dy = (picTree.Height - pIconHeight * 2) / (pMaximumTreeDepth - 1)
                If dy < pIconHeight * 1.5 Then
                    dy = pIconHeight * 1.5
                    'TODO: enable vertical scrolling
                End If
                drawsurface = picTree
                picTree.SuspendLayout()
                'End If 'Printing

                picTree.Size = Me.SplitLegendTree.Panel2.ClientSize
                Dim Ybase As Integer = drawsurface.Height - pIconHeight
                pHeightNeeded = 0
                pWidthNeeded = 0

                For Each lOutlet As clsIcon In pOutlets
                    LayoutFromIcon(lOutlet, Ybase, dy, drawsurface.Width, aPrinting)
                Next

                If pHeightNeeded > picTree.Height Then
                    Dim lMove As Integer = pHeightNeeded - picTree.Height
                    picTree.Height = pHeightNeeded
                    For Each lControl As Control In pIcons
                        lControl.Top += lMove
                    Next
                End If
                If pWidthNeeded > picTree.Width Then picTree.Width = pWidthNeeded

                If aPrinting Then
                    'UPGRADE_ISSUE: Printer method Printer.EndDoc was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
                    'Printer.EndDoc()
                Else
                    DrawTreeBackground()
                    picTree.ResumeLayout()
                End If
            Finally
                lInLayout = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Draw the connecting lines and selection halos behind the tree nodes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DrawTreeBackground()
        If pTreeBackground IsNot Nothing Then pTreeBackground.Dispose()
        pTreeBackground = New Bitmap(picTree.Width, picTree.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim lGraphics As Graphics = Graphics.FromImage(pTreeBackground)
        Dim lLinesPen As Pen = SystemPens.ControlDarkDark

        For Each lIcon As clsSchematicIcon In pIcons
            With lIcon
                Dim lIconCenter As Point = .Center
                For Each lUpstreamIcon As clsIcon In lIcon.UpstreamIcons
                    lGraphics.DrawLine(lLinesPen, lIconCenter, lUpstreamIcon.Center)
                Next
                If .Selected Then
                    lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
                End If
            End With
        Next
        lGraphics.Dispose()
        picTree.BackgroundImage = pTreeBackground
    End Sub

    Private Sub DrawTreeBackground(ByVal aIcon As clsSchematicIcon)
        Dim lGraphics As Graphics = Graphics.FromImage(picTree.BackgroundImage)
        Dim lLinesPen As Pen = SystemPens.ControlDarkDark
        'Dim lClipLeft As Integer = aIcon.Left
        'Dim lClipRight As Integer = aIcon.Right
        'Dim lClipTop As Integer = aIcon.Top
        'Dim lClipBottom As Integer = aIcon.Bottom

        'ExtendClip(lClipLeft, lClipRight, lClipTop, lClipBottom, aIcon.UpstreamIcons)
        'ExtendClip(lClipLeft, lClipRight, lClipTop, lClipBottom, aIcon.DownstreamIcons)

        'lGraphics.SetClip(New Rectangle(lClipLeft - pIconWidth, lClipTop - pIconWidth, lClipRight - lClipLeft + pIconWidth * 2, lClipBottom - lClipTop + pIconWidth * 2))
        lGraphics.Clear(SystemColors.Window)

        With aIcon
            Dim lIconCenter As Point = .Center
            For Each lUpstreamIcon As clsIcon In aIcon.UpstreamIcons
                With lUpstreamIcon
                    lGraphics.DrawLine(lLinesPen, lIconCenter, .Center)
                    If .Selected Then lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
                    .Invalidate()
                End With
            Next
            For Each lDownstreamIcon As clsIcon In aIcon.DownstreamIcons
                With lDownstreamIcon
                    lGraphics.DrawLine(lLinesPen, lIconCenter, .Center)
                    If .Selected Then lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
                    .Invalidate()
                End With
            Next
            If .Selected Then
                lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
            End If
        End With

        aIcon.Invalidate()
        lGraphics.Dispose()
        'picTree.BackgroundImage = pTreeBackground
        picTree.Invalidate()
    End Sub

    Private Sub ExtendClip(ByRef lClipLeft As Integer, ByRef lClipRight As Integer, ByRef lClipTop As Integer, ByRef lClipBottom As Integer, ByVal aIcons As Generic.List(Of clsIcon))
        For Each lIcon As clsIcon In aIcons
            If lIcon.Left < lClipLeft Then lClipLeft = lIcon.Left
            If lIcon.Right > lClipRight Then lClipRight = lIcon.Right
            If lIcon.Top < lClipTop Then lClipTop = lIcon.Top
            If lIcon.Bottom > lClipBottom Then lClipBottom = lIcon.Bottom
        Next
    End Sub

    Private Sub DrawPictureOnReachControl(ByVal aOperation As HspfOperation, ByVal aPrinting As Boolean, ByVal aControl As Control)
        Dim lBitmap As New Bitmap(pIconWidth, pIconHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(lBitmap)
        SetSchematicIcon(aOperation, g)
        drawBorder(g, pIconWidth, pIconHeight, Not aPrinting)
        g.Dispose()
        aControl.BackgroundImage = lBitmap
    End Sub

    Private Sub SetLandLegendIcon(ByVal aOperation As HspfOperation, ByVal aPrinting As Boolean, ByRef aExistingIcon As clsIcon, Optional ByVal aClear As Boolean = False)
        Dim lNewIcon As clsSchematicIcon
        Dim lBitmap As Bitmap
        If aExistingIcon Is Nothing Then
            lNewIcon = New clsSchematicIcon
            AddHandler lNewIcon.MouseDown, AddressOf LegendIcon_MouseDown
            aClear = True
            lNewIcon.Width = pCurrentLegend.IconWidth
            lNewIcon.Height = pIconHeight
            lNewIcon.Label = DescToLabel(aOperation.Description)
            lBitmap = New Bitmap(lNewIcon.Width, lNewIcon.Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Else
            lNewIcon = aExistingIcon
            lBitmap = lNewIcon.BackgroundImage
        End If
        With lNewIcon

            Dim g As Graphics = Graphics.FromImage(lBitmap)

            Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(.Label, Me.Font)
            Dim lX As Single = (.Width - lStringMeasurement.Width) / 2
            Dim lY As Single = .Height - lStringMeasurement.Height * 1.25

            If aClear Then
                .Key = .Label
                g.Clear(SystemColors.Control)
                g.DrawString(.Label, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
            End If

            Dim lBoxHeight As Integer = lY * 0.6
            lY /= 4
            lX = lY
            Dim lBoxWidth As Integer = .Width * 0.4

            If aOperation.Name = "PERLND" Then
                .Perlnd = aOperation
                lX = lY
            Else
                .Implnd = aOperation
                lX = .Width - lY - lBoxWidth
            End If

            Dim lBrush As Brush = New SolidBrush(ColorFromDesc(.Label))
            g.FillRectangle(lBrush, lX, lY, lBoxWidth, lBoxHeight)

            drawBorder(g, .Width, .Height, Not aPrinting)
            g.Dispose()
            .BackgroundImage = lBitmap
        End With
        aExistingIcon = lNewIcon
    End Sub

    Private Function DescToLabel(ByVal aDesc As String) As String
        If aDesc Is Nothing OrElse aDesc.Length = 0 Then Return "Unnamed"
        If aDesc.Contains(":") Then Return aDesc.Substring(aDesc.IndexOf(":") + 1)
        Return aDesc
    End Function

    Private Function MetSegLabel(ByVal aMetSeg As HspfMetSeg) As String
        Return aMetSeg.Id & ":" & aMetSeg.Name.Replace(",", "") 'Precip location name might be nicer
    End Function

    Private Sub SetMetLegendIcon(ByVal aMetSeg As HspfMetSeg, ByVal aPrinting As Boolean, ByRef aExistingIcon As clsIcon)
        Dim lNewIcon As clsSchematicIcon
        If aExistingIcon Is Nothing Then
            lNewIcon = New clsSchematicIcon
            lNewIcon.Width = pCurrentLegend.IconWidth
            lNewIcon.Height = pIconHeight
            AddHandler lNewIcon.MouseDown, AddressOf LegendIcon_MouseDown
        Else
            lNewIcon = aExistingIcon
        End If
        With lNewIcon
            .Key = aMetSeg.Id
            Dim lBitmap As New Bitmap(.Width, .Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(lBitmap)

            .MetSeg = aMetSeg
            .Label = MetSegLabel(aMetSeg)

            Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(.Label, Me.Font)
            Dim lX As Single = (.Width - lStringMeasurement.Width) / 2
            Dim lY As Single = (.Height - lStringMeasurement.Height) / 2

            g.Clear(SystemColors.Control)
            g.DrawString(.Label, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)

            lY /= 4

            drawBorder(g, .Width, .Height, Not aPrinting)
            g.Dispose()
            .BackgroundImage = lBitmap
        End With
        aExistingIcon = lNewIcon
    End Sub

    Private Sub SetPointLegendIcon(ByVal aPointSource As HspfPointSource, ByVal aPrinting As Boolean, ByRef aExistingIcon As clsIcon)
        Dim lNewIcon As clsSchematicIcon
        If aExistingIcon Is Nothing Then
            lNewIcon = New clsSchematicIcon
            lNewIcon.Width = pCurrentLegend.IconWidth
            lNewIcon.Height = pIconHeight
            AddHandler lNewIcon.MouseDown, AddressOf LegendIcon_MouseDown
        Else
            lNewIcon = aExistingIcon
        End If
        With lNewIcon
            .Key = aPointSource.Id
            Dim lBitmap As New Bitmap(.Width, .Height, Drawing.Imaging.PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(lBitmap)

            .PointSource = aPointSource
            .Label = aPointSource.Id & ":" & aPointSource.Name

            Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(.Label, Me.Font)
            Dim lX As Single = (.Width - lStringMeasurement.Width) / 2
            Dim lY As Single = (.Height - lStringMeasurement.Height) / 2

            g.Clear(SystemColors.Control)
            g.DrawString(.Label, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)

            lY /= 4

            drawBorder(g, .Width, .Height, Not aPrinting)
            g.Dispose()
            .BackgroundImage = lBitmap
        End With
        aExistingIcon = lNewIcon
    End Sub

    'Draw icon representing aOperation into given graphics object
    Public Sub SetSchematicIcon(ByVal aOperation As HspfOperation, ByVal g As Graphics)
        Dim sid, barPos As Integer
        Dim lStr As String
        Dim barDesc As String
        Dim lSource As HspfConnection
        Dim started As Boolean
        Dim included() As Boolean

        lStr = OperationKey(aOperation)

        'TODO: pic.ToolTipText = pOpnBlk.Name & " " & pId & " " & pDescription
        g.Clear(SystemColors.Control)

        Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(lStr, Me.Font)
        Dim lX As Single = (pIconWidth - lStringMeasurement.Width) / 2
        Dim lY As Single = pIconHeight - lStringMeasurement.Height * 1.25
        Dim lBarBottom As Integer = lY
        Dim lBarMaxVal As Double
        Dim lBarMaxTop As Integer = 1
        g.DrawString(lStr, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
        Dim myid As Integer
        Dim pPoint As HspfPointSource
        Select Case pCurrentLegend.LegendType
            Case EnumLegendType.LegLand
                Dim lBarFraction As Double
                lBarMaxVal = pUci.MaxAreaByLand2Stream
                barPos = 3
                Dim lBarWidth As Integer
                Dim lBarSpace As Integer = 1
                If LegendOrder Is Nothing OrElse LegendOrder.Count = 0 Then 'Draw all in the order they fall
                    lBarWidth = (pIconWidth - 6) / aOperation.Sources.Count
                    If lBarWidth < 1 Then
                        lBarWidth = 1
                        lBarSpace = 0
                    ElseIf lBarWidth > 10 Then
                        lBarWidth = 10
                    End If
                    For Each lSource In aOperation.Sources
                        If lSource.Source IsNot Nothing AndAlso lSource.Source.Opn IsNot Nothing _
                          AndAlso (lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND") Then
                            'barHeight = barbase * lSource.MFact / barMaxVal
                            lBarFraction = lSource.MFact / lBarMaxVal
                            If lBarFraction > 0 Then
                                Dim lBrush As Brush = New SolidBrush(ColorFromDesc(DescToLabel(lSource.Source.Opn.Description)))
                                'g.FillRectangle(lBrush, barPos, barbase - barHeight, lBarWidth, barHeight)
                                g.FillRectangle(lBrush, barPos, lBarBottom, lBarWidth, CInt((lBarMaxTop - lBarBottom) * lBarFraction))
                            End If
                            barPos += lBarWidth + lBarSpace
                        End If
                    Next lSource
                Else 'Draw only land uses in LegendOrder, in order and leaving spaces for ones that do not appear in this segment
                    lBarWidth = (pIconWidth - 6) / LegendOrder.Count
                    If lBarWidth < 1 Then
                        lBarWidth = 1
                        lBarSpace = 0
                    ElseIf lBarWidth > 10 Then
                        lBarWidth = 10
                    End If
                    For Each barDesc In LegendOrder
                        lBarFraction = 0
                        For Each lSource In aOperation.Sources
                            If lSource.Source IsNot Nothing AndAlso lSource.Source.Opn IsNot Nothing _
                              AndAlso (lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND") _
                              AndAlso lSource.Source.Opn.Description = barDesc Then
                                lBarFraction += lSource.MFact / lBarMaxVal
                            End If
                        Next lSource
                        If lBarFraction > 0 Then
                            Dim lBrush As Brush = New SolidBrush(ColorFromDesc(barDesc))
                            'g.FillRectangle(lBrush, barPos, barbase - barHeight, lBarWidth, barHeight)
                            Dim lBarTop As Integer = lBarMaxTop + CInt((1 - lBarFraction) * (lBarBottom - lBarMaxTop))
                            g.FillRectangle(lBrush, barPos, lBarTop, lBarWidth, (lBarBottom - lBarTop))
                        End If
                        barPos += lBarWidth + lBarSpace
                    Next barDesc
                End If

            Case EnumLegendType.LegMet
                ReDim included(pUci.MetSegs.Count)
                If Not aOperation.MetSeg Is Nothing Then
                    included(aOperation.MetSeg.Id) = True
                    myid = aOperation.MetSeg.Id
                Else
                    myid = 0
                End If
                'myid = 0
                For Each lSource In aOperation.Sources
                    If lSource.Source.Opn IsNot Nothing Then
                        If lSource.Source.Opn.MetSeg IsNot Nothing Then
                            If lSource.Source.Opn.Name <> "RCHRES" Then
                                'myid = lSource.Source.Opn.MetSeg.Id
                                included(lSource.Source.Opn.MetSeg.Id) = True
                            End If
                        End If
                    End If
                Next lSource

                lStringMeasurement = g.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (lBarBottom - lStringMeasurement.Height) / 2
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
                        'TODO: underline if this met seg contribs to reach directly,
                        'dont underline if this met seg contribs to reach only indirectly through land segment
                        'If sid = myid Then pic.Font = VB6.FontChangeUnderline(pic.Font, True) Else pic.Font = VB6.FontChangeUnderline(pic.Font, False) ' .ForeColor = vbHighlight Else pic.ForeColor = vbButtonText
                        g.DrawString(sid, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
                        lX += g.MeasureString(sid, Me.Font).Width
                    End If
                Next
                'pic.Font.Underline = False
            Case EnumLegendType.LegPoint
                ReDim included(pUci.PointSources.Count)
                'Debug.Print pPointSources.Count
                For Each pPoint In aOperation.PointSources
                    included(pPoint.Id) = True
                Next pPoint
                lStringMeasurement = g.MeasureString("X", Me.Font)
                lX = lStringMeasurement.Width
                lY = (lBarBottom - lStringMeasurement.Height) / 2
                For sid = 1 To pUci.PointSources.Count
                    If included(sid) Then
                        Dim lStrPrint As String
                        If started Then
                            lStrPrint = ", " & sid
                        Else
                            lStrPrint = sid
                            started = True
                        End If
                        g.DrawString(sid, Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
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
    End Sub

    'Returns color for source.VolId
    Private Function IdColor(ByRef aColorId As Integer) As Integer
        Dim lIdColor As Integer = RGB(Rnd(-aColorId - 53) * 255, _
                                      Rnd(-aColorId - 27) * 255, _
                                      Rnd(-aColorId - 33) * 255)
        Return lIdColor
    End Function

    Private Function ColorFromDesc(ByVal aDescription As String) As Color
        Dim lColor As Color
        Dim lUcaseDesc As String = aDescription.ToUpper
        Dim lColorIndex As Integer = ColorMap.IndexFromKey(lUcaseDesc)
        If lColorIndex >= 0 Then
            lColor = ColorMap.ItemByIndex(lColorIndex)
        Else
            If lUcaseDesc.Contains("FOREST") OrElse lUcaseDesc.Contains("WOOD") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("FOREST"))
            ElseIf lUcaseDesc.Contains("AGRI") OrElse lUcaseDesc.Contains("FARM") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("AGRICULTURAL"))
            ElseIf lUcaseDesc.Contains("CROP") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("AGRICULTURAL"))
            ElseIf lUcaseDesc.Contains("URBAN") OrElse lUcaseDesc.Contains("INDU") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("URBAN"))
            ElseIf lUcaseDesc.Contains("WATER") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("WATERWETLANDS"))
            ElseIf lUcaseDesc.Contains("RESIDENTIAL") Then
                ColorMap.Add(lUcaseDesc, ColorMap.ItemByKey("RESIDENTIAL"))
            Else
                ColorMap.Add(lUcaseDesc, Color.Black)
            End If
            Return ColorMap.ItemByKey(lUcaseDesc)
        End If
        Return lColor

    End Function

    Private Sub drawBorder(ByVal pic As Graphics, ByVal aWidth As Integer, ByVal aHeight As Integer, ByVal threeD As Boolean)
        Dim lPen As Pen
        If threeD Then
            lPen = New Pen(SystemColors.ControlLightLight)
        Else
            lPen = New Pen(Color.Black)
        End If
        pic.DrawLine(lPen, 0, 0, 0, aHeight)
        pic.DrawLine(lPen, 0, 0, aWidth, 0)
        If threeD Then
            lPen = New Pen(System.Drawing.SystemColors.ControlDark)
        End If
        pic.DrawLine(lPen, 0, aHeight - 1, aWidth - 1, aHeight - 1)
        pic.DrawLine(lPen, aWidth - 1, 0, aWidth - 1, aHeight - 1)
    End Sub

    'height of tree of this plus all downstream operations. 1=none downstream
    Private Function DownLayers(ByVal aIcon As clsSchematicIcon, ByVal aAlreadyInPath As String) As Integer
        Dim lDeepestTarget As Integer = 0
        For Each lTarConn As HspfConnection In aIcon.ReachOrBMP.Targets
            Dim lTarOperation As HspfOperation = lTarConn.Target.Opn
            If lTarOperation IsNot Nothing Then
                Dim lKey As String = OperationKey(lTarOperation)
                If aAlreadyInPath.Contains(lKey & vbCrLf) AndAlso Not TreeLoopMessageDisplayed Then
                    Dim lMessage As String = aAlreadyInPath
                    Logger.Msg(aAlreadyInPath, "Detected possible loop in operations")
                    TreeLoopMessageDisplayed = True
                Else
                    If pIcons.Contains(lKey) Then
                        aAlreadyInPath &= lKey & vbCrLf
                        Dim lIcon As clsSchematicIcon = pIcons(lKey)
                        If Not lIcon.UpstreamIcons.Contains(aIcon) Then lIcon.UpstreamIcons.Add(aIcon)
                        Dim lTargetDepth As Integer = DownLayers(lIcon, aAlreadyInPath)
                        If lTargetDepth > lDeepestTarget Then
                            lDeepestTarget = lTargetDepth
                        End If
                    End If
                End If
            End If
        Next lTarConn
        Return lDeepestTarget + 1
    End Function

    Private Sub LayoutFromIcon(ByVal aIcon As clsSchematicIcon, ByVal aY As Integer, ByVal aDy As Integer, ByVal aWidth As Integer, ByVal aPrinting As Boolean)
        DrawPictureOnReachControl(aIcon.ReachOrBMP, aPrinting, aIcon)
        With aIcon
            aIcon.Top = aY - pIconHeight / 2
            Dim lWidthPerItemThisRow As Integer = aWidth / pIconsDistantFromOutlet.ItemByKey(.DistanceFromOutlet)
            If lWidthPerItemThisRow < aIcon.Width * 1.25 Then lWidthPerItemThisRow = aIcon.Width * 1.25
            aIcon.Left = pIconsDistantFromOutletPlaced.ItemByKey(.DistanceFromOutlet) * lWidthPerItemThisRow + (lWidthPerItemThisRow - pIconWidth) / 2
            pIconsDistantFromOutletPlaced.Increment(.DistanceFromOutlet, 1)
            Dim lNumUpstream As Integer = aIcon.UpstreamIcons.Count
            If lNumUpstream > 0 Then
                aY -= aDy
                For Each lUpstreamIcon As clsIcon In aIcon.UpstreamIcons
                    LayoutFromIcon(lUpstreamIcon, aY, aDy, aWidth, aPrinting)
                Next
            End If            
            Dim lHeightNeeded = picTree.Height - aY + pIconHeight
            If lHeightNeeded > pHeightNeeded Then pHeightNeeded = lHeightNeeded
            Dim lWidthNeeded = aIcon.Left + pIconWidth * 1.5
            If lWidthNeeded > pWidthNeeded Then pWidthNeeded = lWidthNeeded
        End With
    End Sub

    Public Sub ClearTree()
        picTree.SuspendLayout()
        If picTree.BackgroundImage IsNot Nothing Then
            picTree.BackgroundImage.Dispose()
            picTree.BackgroundImage = Nothing 'New Bitmap(0, 0, Drawing.Imaging.PixelFormat.Format32bppArgb)
        End If
        picTree.Controls.Clear()
        For Each lControl As Control In pIcons
            lControl.Dispose()
        Next
        pIcons.Clear()
        picTree.ResumeLayout()
        pIconsDistantFromOutlet.Clear()
        pIconsDistantFromOutletPlaced.Clear()
        pOutlets.Clear()
    End Sub

    Public Sub UpdateDetails()
        Select Case pCurrentLegend.LegendType
            Case EnumLegendType.LegLand : PopulateLandGrid()
            Case EnumLegendType.LegMet : PopulateMetGrid()
            Case EnumLegendType.LegPoint : PopulatePointGrid()
            Case Else : agdDetails.Visible = False
        End Select
        RefreshDetails()
    End Sub

    Private Sub PopulateLandGrid()
        Dim lConn As HspfConnection
        Dim lDesc As String
        Dim AddedThisReach As Boolean
        Dim ReachNames, LastName As String
        Dim PgrandTotal, Ptotal, Itotal, IgrandTotal As Single
        Dim t As Double = 0
        Dim AreaUnits As String
        Dim lRow As Integer

        If pUci Is Nothing OrElse pUci.Name.Length = 0 Then Exit Sub

        If pUci.GlobalBlock.EmFg = 1 Then
            AreaUnits = " (Acres)"
        Else
            AreaUnits = " (Hectares)"
        End If

        Dim lDetailsSource As New atcControls.atcGridSource
        With lDetailsSource
            .Rows = 1
            .FixedRows = 1
            .Columns = 5
            .CellValue(0, 0) = "Land Use"
            .CellValue(0, 1) = "Reaches"
            .CellValue(0, 2) = "Implnd" & AreaUnits
            .CellValue(0, 3) = "Perlnd" & AreaUnits
            .CellValue(0, 4) = "Total" & AreaUnits

            '.set_ColType(2, ATCoCtl.ATCoDataType.ATCoTxt)
            '.set_ColType(3, ATCoCtl.ATCoDataType.ATCoTxt)
            '.set_ColType(4, ATCoCtl.ATCoDataType.ATCoTxt)
            For Each lLegendIcon As clsSchematicIcon In pCurrentLegend.Icons
                If lLegendIcon.Selected Then
                    Ptotal = 0
                    Itotal = 0
                    ReachNames = ""
                    LastName = ""
                    lDesc = lLegendIcon.Key
                    lRow = .Rows
                    .CellValue(lRow, 0) = lDesc
                    For Each lReach As clsSchematicIcon In pIcons
                        AddedThisReach = False
                        If lReach.Selected Then
                            Dim lOperation As HspfOperation = lReach.ReachOrBMP
                            For Each lConn In lOperation.Sources
                                Select Case lConn.Source.VolName
                                    Case "PERLND"
                                        If lConn.Source.Opn IsNot Nothing Then
                                            If lDesc = DescToLabel(lConn.Source.Opn.Description) Then
                                                Ptotal = Ptotal + lConn.MFact

                                                If Not AddedThisReach Then
                                                    If lOperation.Name <> LastName Then
                                                        LastName = lOperation.Name
                                                        ReachNames = ReachNames & LastName & " "
                                                    End If
                                                    ReachNames = ReachNames & lOperation.Id & ", "
                                                    AddedThisReach = True
                                                End If

                                            End If
                                        End If
                                    Case "IMPLND"
                                        If lConn.Source.Opn IsNot Nothing Then
                                            If lDesc = DescToLabel(lConn.Source.Opn.Description) Then 'lConn.Source.Opn.Name & " " & lConn.Source.Opn.Id Then
                                                Itotal = Itotal + lConn.MFact

                                                If Not AddedThisReach Then
                                                    If lOperation.Name <> LastName Then
                                                        LastName = lOperation.Name
                                                        ReachNames = ReachNames & LastName & " "
                                                    End If
                                                    ReachNames = ReachNames & lOperation.Id & ", "
                                                    AddedThisReach = True
                                                End If

                                            End If
                                        End If
                                End Select
                            Next
                        End If
                    Next
                    If Len(ReachNames) < 2 Then
                        .CellValue(lRow, 1) = ""
                    Else 'remove final ", "
                        .CellValue(lRow, 1) = ReachNames.Substring(0, ReachNames.Length - 2)
                    End If
                    .CellValue(lRow, 2) = DoubleToString(Itotal, 8)
                    .CellValue(lRow, 3) = DoubleToString(Ptotal, 8)
                    .CellValue(lRow, 4) = DoubleToString(Ptotal + Itotal, 8)
                    PgrandTotal = PgrandTotal + Ptotal
                    IgrandTotal = IgrandTotal + Itotal
                End If
            Next
            lRow = .Rows
            .CellValue(lRow, 0) = "Total"
            .CellValue(lRow, 1) = ""
            .CellValue(lRow, 2) = DoubleToString(IgrandTotal, 8)
            .CellValue(lRow, 3) = DoubleToString(PgrandTotal, 8)
            .CellValue(lRow, 4) = DoubleToString(PgrandTotal + IgrandTotal, 8)
            '    For i = 0 To picReach.Count - 1
            '      If ReachSelected(i) Then
            '        Set lOper = lOpns(CLng(picReach(i).tag))
            '        For Each vConn In lOper.Sources
            '          Set lConn = vConn
            '          'Debug.Print lConn.source.volname
            '          If lConn.source.volname = "PERLND" Or lConn.source.volname = "IMPLND" Then
            '            lDesc = lConn.source.Opn.Description
            '            If LegendSelected(lDesc) Then
            '              .rows = .rows + 1
            '              .TextMatrix(.rows, 0) = lConn.source.volname & " " & lConn.source.volid
            '              .TextMatrix(.rows, 1) = lDesc
            '              .TextMatrix(.rows, 2) = lOper.Name & " " & lOper.id
            '              .TextMatrix(.rows, 3) = lConn.MFact
            '              t = t + lConn.MFact
            '            End If
            '          End If
            '        Next vConn
            '      End If
            '    Next i
            '    lblTotal(0) = "Total: " & t
            '    lblTotal(1).Visible = False
            '    lblTotal(2).Visible = False
            '    OrigTotal = t
        End With
        agdDetails.Initialize(lDetailsSource)
    End Sub

    Private Sub PopulateMetGrid()
        Dim lDetailsSource As New atcControls.atcGridSource
        With lDetailsSource
            Dim lDataTypeColumn As Integer = 0
            Dim lSourceColumn As Integer = 1
            Dim lMfactColumnPI As Integer = 2
            Dim lMfactColumnR As Integer = 3
            Dim lTranColumn As Integer = 4
            .FixedRows = 1
            .Columns = 5
            .Rows = 7
            Dim lRow As Integer = 0
            .CellValue(0, lDataTypeColumn) = "Data Type" ') : .set_ColType(0, ATCoCtl.ATCoDataType.ATCoTxt)
            .CellValue(0, lSourceColumn) = "Source" ') : .set_ColType(1, ATCoCtl.ATCoDataType.ATCoTxt)
            .CellValue(0, lMfactColumnPI) = "P/I MFact" ') : .set_ColType(2, ATCoCtl.ATCoDataType.ATCoSng)
            .CellValue(0, lMfactColumnR) = "R MFact" ') : .set_ColType(3, ATCoCtl.ATCoDataType.ATCoSng)
            .CellValue(0, lTranColumn) = "Tran" ') : .set_ColType(4, ATCoCtl.ATCoDataType.ATCoTxt)

            .CellValue(1, 0) = "Precip"
            .CellValue(2, 0) = "Air Temp"
            .CellValue(3, 0) = "Dew Point"
            .CellValue(4, 0) = "Wind"
            .CellValue(5, 0) = "Solar Rad"
            .CellValue(6, 0) = "Cloud"
            .CellValue(7, 0) = "Pot Evap"

            If pUci IsNot Nothing AndAlso pUci.Name.Length > 0 AndAlso pUci.MetSegs.Count > 0 Then
                For Each lMetSeg As HspfMetSeg In pUci.MetSegs
                    If Me.LegendMetSegs.Icon(lMetSeg.Id).Selected Then
                        For Each lRec As HspfMetSegRecord In lMetSeg.MetSegRecs
                            Select Case lRec.Name
                                Case "PREC" : lRow = 1
                                Case "ATEM" : lRow = 2
                                Case "DEWP" : lRow = 3
                                Case "WIND" : lRow = 4
                                Case "SOLR" : lRow = 5
                                Case "CLOU" : lRow = 6
                                Case "PEVT" : lRow = 7
                            End Select
                            Try
                                .CellValue(lRow, lSourceColumn) = lRec.Source.VolName & " " & lRec.Source.VolId
                                .CellValue(lRow, lMfactColumnPI) = lRec.MFactP
                                .CellValue(lRow, lMfactColumnR) = lRec.MFactR
                                .CellValue(lRow, lTranColumn) = lRec.Tran
                            Catch e As Exception
                                Logger.Dbg("Exception filling met grid: " & e.Message)
                            End Try
                        Next
                    End If
                Next
            End If
        End With
        agdDetails.Initialize(lDetailsSource)
    End Sub

    Private Sub PopulatePointGrid()

        Dim lDetailsSource As New atcControls.atcGridSource
        With lDetailsSource
            .Rows = 1
            .FixedRows = 1
            .Columns = 2
            .CellValue(0, 0) = "Point Source"
            .CellValue(0, 1) = "Constituent"

            If pUci IsNot Nothing AndAlso pUci.Name.Length > 0 AndAlso pUci.PointSources.Count > 0 Then
                For Each lPoint As HspfPointSource In pUci.PointSources
                    If Me.LegendPointSources.Icon(lPoint.Id) IsNot Nothing AndAlso Me.LegendPointSources.Icon(lPoint.Id).Selected Then
                        Dim lRow As Integer = .Rows
                        .CellValue(lRow, 0) = lPoint.Name
                        Dim lCon As String = lPoint.Con
                        If lCon.Length > 0 Then
                            'look for this con in pollutant list
                            For Each lPol As String In pPollutantList
                                If Mid(lCon, 1, 5) = Mid(lPol, 1, 5) Then
                                    lCon = lPol
                                    Exit For
                                End If
                            Next
                            .CellValue(lRow, 1) = lCon
                        End If
                    End If
                Next
            End If
        End With

        agdDetails.Initialize(lDetailsSource)
    End Sub

    Public Sub UpdateLegend()
        pCurrentLegend.SuspendLayout()
        pCurrentLegend.Clear()
        If pUci IsNot Nothing Then
            LegendOrder = New Generic.List(Of String)
            Select Case pCurrentLegend.LegendType
                Case EnumLegendType.LegLand
                    For Each lOperation As HspfOperation In pUci.OpnSeqBlock.Opns
                        If lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND" Then
                            Dim lIcon As clsIcon = pCurrentLegend.Icon(DescToLabel(lOperation.Description))
                            If lIcon Is Nothing Then
                                SetLandLegendIcon(lOperation, False, lIcon)
                                pCurrentLegend.Add(lIcon)
                                LegendOrder.Add(lIcon.Label)
                            Else 'Just add to existing icon, don't need to create a new one
                                SetLandLegendIcon(lOperation, False, lIcon)
                            End If
                        End If
                    Next
                Case EnumLegendType.LegMet
                    For Each lMetSeg As HspfMetSeg In pUci.MetSegs
                        Dim lIcon As clsIcon = Nothing
                        SetMetLegendIcon(lMetSeg, False, lIcon)
                        pCurrentLegend.Add(lIcon)
                        LegendOrder.Add(lIcon.Key)
                    Next lMetSeg
                Case EnumLegendType.LegPoint
                    For Each lPoint As HspfPointSource In pUci.PointSources
                        Dim lIcon As clsIcon = Nothing
                        SetPointLegendIcon(lPoint, False, lIcon)
                        pCurrentLegend.Add(lIcon)
                        LegendOrder.Add(lIcon.Key)
                    Next lPoint
            End Select
        End If
        pCurrentLegend.ResumeLayout()
    End Sub

    Private Sub ResizeLegend()
        pCurrentLegend.SuspendLayout()
        If pUci IsNot Nothing Then
            Select Case pCurrentLegend.LegendType
                Case EnumLegendType.LegLand
                    For Each lIcon As clsSchematicIcon In pCurrentLegend.Icons
                        Dim lClear As Boolean = True
                        If lIcon.Perlnd IsNot Nothing Then SetLandLegendIcon(lIcon.Perlnd, False, lIcon, lClear) : lClear = False
                        If lIcon.Implnd IsNot Nothing Then SetLandLegendIcon(lIcon.Implnd, False, lIcon, lClear)
                    Next
                Case EnumLegendType.LegMet
                    For Each lIcon As clsSchematicIcon In pCurrentLegend.Icons
                        SetMetLegendIcon(lIcon.MetSeg, False, lIcon)
                    Next
            End Select
        End If
        pCurrentLegend.ResumeLayout()
        pCurrentLegend.PlaceIcons()
    End Sub

    '    Public Sub UpdateLegend()
    '        Dim item As Object
    '        Dim Key As String
    '        Dim srch, Index, oprindex As Integer
    '        Dim colr As Color
    '        Dim i As Integer
    '        Dim S As String
    '        Dim ypos, xpos, colonpos As Integer
    '        Dim boxWidth, boxHeight, txtHeight As Integer
    '        Dim lOper As HspfOperation
    '        Dim spos, maxpic, cpos As Integer
    '        Dim tname As String

    '        LegendOrder = Nothing

    '        Dim picTab As Graphics
    '        picTab.Clear(Color.White)
    '        For Index = 0 To picLegend.Count - 1
    '            With picLegend(Index)
    '                .Tag = ""
    '                .Cls()
    '                .Visible = False
    '                .Width = picTab.ClipBounds.Width
    '            End With
    '        Next

    '        boxWidth = picTab.ClipBounds.Width * 0.4
    '        txtHeight = picTab.MeasureString("X", Me.Font).Height
    '        boxHeight = txtHeight * 2
    '        Index = 0
    '        ypos = 0 'boxHeight + txtHeight
    '        maxpic = 0
    '        Dim lPoint As HspfPointSource
    '        Dim lmetseg As HspfMetSeg
    '        Dim lX As Single = 0
    '        Dim lY As Single = 0
    '        Select Case pCurrentLegend.LegendType
    '            Case EnumLegendType.LegLand
    '                LegendOrder = New Generic.List(Of String)
    '                'TODO: picTab.Font = VB6.FontChangeSize(picTab.Font, 8)
    '                If picTab.MeasureString("Perlnd   Implnd", Me.Font).Width > picTab.ClipBounds.Width Then
    '                    lX = (boxWidth - picTab.MeasureString("Per", Me.Font).Width) / 2
    '                    picTab.DrawString(" Per", Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
    '                    lX = picTab.ClipBounds.Width - ((boxWidth + picTab.MeasureString("Imp", Me.Font).Width) / 2)
    '                    picTab.DrawString("Imp ", Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
    '                Else
    '                    lX = (boxWidth - picTab.MeasureString("Perlnd", Me.Font).Width) / 2
    '                    picTab.DrawString(" Perlnd", Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
    '                    lX = picTab.ClipBounds.Width - ((boxWidth + picTab.MeasureString("Implnd", Me.Font).Width) / 2)
    '                    picTab.DrawString("Implnd ", Me.Font, SystemBrushes.ControlDarkDark, lX, lY)
    '                End If
    '                ypos = txtHeight
    '                picLegend(0).Top = ypos
    '                If pUci.Name <> "" Then
    '                    'UPGRADE_WARNING: Couldn't resolve default property of object pUci.OpnSeqBlock.Opns.Count. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '                    For oprindex = 1 To pUci.OpnSeqBlock.Opns.Count
    '                        lOper = pUci.OpnSeqBlock.Opn(oprindex)
    '                        If lOper.Name = "PERLND" Or lOper.Name = "IMPLND" Then
    '                            Key = lOper.Description
    '                            colonpos = InStr(Key, ":")
    '                            If colonpos > 0 Then Key = Mid(Key, colonpos + 1)
    '                            For Index = 0 To picLegend.Count - 1
    '                                If Key = picLegend(Index).Tag Or picLegend(Index).Tag = "" Then Exit For
    '                            Next
    '                            If Index >= picLegend.Count Then
    '                                'TODO: picLegend.Load(Index)
    '                                picLegend(Index).Tag = ""
    '                                picLegend(Index).Width = picTab.ClipBounds.Width
    '                            End If
    '                            With picLegend(Index)
    '                                If Index > maxpic Then maxpic = Index
    '                                .Height = boxHeight * 1.5 + txtHeight
    '                                If .Tag = "" Then
    '                                    .Tag = Key
    '                                    .Top = ypos - LegendScrollPos
    '                                    ypos = ypos + .Height
    '                                    .Visible = True
    '                                End If
    '                                'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                .CurrentX = (.Width - .TextWidth(Key)) / 2
    '                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                If .CurrentX < 2 Then .CurrentX = 2
    '                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                .CurrentY = boxHeight * 1.5
    '                                'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                picLegend(Index).Print(Key)

    '                                On Error GoTo ColorError
    '                                'UPGRADE_WARNING: Couldn't resolve default property of object ColorMap(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
    '                                colr = ColorFromDesc(Key)
    '                                On Error GoTo 0
    '                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                .CurrentY = boxHeight / 4
    '                                'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                If lOper.Name = "PERLND" Then
    '                                    .CurrentX = 0
    '                                Else
    '                                    'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                    .CurrentX = .Width - boxWidth
    '                                End If
    '                                'UPGRADE_ISSUE: PictureBox method picLegend.Line was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                                'TODO: picLegend(Index).Line (boxWidth, boxHeight), colr, BF
    '                            End With
    '                        End If
    '                    Next
    '                End If
    '                LegendFullHeight = ypos
    '                ReDim Preserve LandSelected(maxpic)
    '                For Index = 0 To picLegend.Count - 1
    '                    If picLegend(Index).Tag <> "" Then LegendOrder.Add(picLegend(Index).Tag)
    '                Next Index
    '            Case EnumLegendType.LegMet
    '                For Each lmetseg In pUci.MetSegs
    '                    'TODO: If Index >= picLegend.Count Then picLegend.Load(Index)
    '                    With picLegend(Index)
    '                        .Tag = Index + 1
    '                        i = InStr(1, lmetseg.Name, ",")
    '                        If i > 0 Then
    '                            S = Mid(lmetseg.Name, 1, i - 1) & vbCr & Mid(lmetseg.Name, i + 1)
    '                        Else
    '                            S = lmetseg.Name
    '                        End If
    '                        Key = lmetseg.Id & ":" & S
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        .CurrentX = (.Width - .TextWidth(Key)) / 2
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        .CurrentY = (.Height - .TextHeight(Key)) / 2
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        picLegend(Index).Print(Key) 'Precip location name might be nicer
    '                        If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
    '                        .Visible = True
    '                    End With
    '                    Index = Index + 1
    '                Next lmetseg
    '                LegendFullHeight = picLegend(0).Height * (Index + 1)
    '            Case EnumLegendType.LegPoint
    '                For Each lPoint In pUci.PointSources
    '                    Index = lPoint.Id - 1
    '                    'TODO: If Index >= picLegend.Count Then picLegend.Load(Index)
    '                    With picLegend(Index)
    '                        .Tag = Index + 1
    '                        'find a way to shorten name
    '                        spos = InStr(1, lPoint.Name, " ")
    '                        cpos = InStr(1, lPoint.Name, ",")
    '                        If spos < 10 And spos > 2 Then
    '                            tname = lPoint.Name.Substring(0, spos - 1)
    '                        ElseIf cpos < 10 And cpos > 2 Then
    '                            tname = lPoint.Name.Substring(0, cpos - 1)
    '                        Else
    '                            tname = lPoint.Name.Substring(0, 10)
    '                        End If
    '                        Key = lPoint.Id & ":" & tname
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.TextWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentX was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        .CurrentX = (.Width - .TextWidth(Key)) / 2
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.TextHeight was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        'UPGRADE_ISSUE: PictureBox property picLegend.CurrentY was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        .CurrentY = (.Height - .TextHeight(Key)) / 2
    '                        'UPGRADE_ISSUE: PictureBox method picLegend.Print was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '                        picLegend(Index).Print(Key)
    '                        If Index > 0 Then .Top = picLegend(Index - 1).Top + picLegend(Index - 1).Height
    '                        .Visible = True
    '                    End With
    '                    Index = Index + 1
    '                Next lPoint
    '                LegendFullHeight = picLegend(0).Height * (Index + 1)
    '            Case Else
    '        End Select
    '        SetLegendScrollButtons()
    '        RefreshLegendSelections()
    '        UpdateDetails()
    '        Exit Sub
    'ColorError:
    '        colr = Color.Black
    '        Err.Clear()
    '        Resume Next
    '    End Sub

    'Private Sub RefreshLegendSelections()
    '    Dim Index As Short
    '    For Index = 0 To UBound(LandSelected)
    '        DrawLegendSelectBox(Index, LegendSelected(CStr(Index)))
    '    Next Index
    'End Sub

    'Private Sub picLegend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picLegend.Click
    '    Dim Index As Short = picLegend.GetIndex(eventSender)
    '    Dim tmp As Short
    '    Dim rch As Integer
    '    Dim vConn As Object
    '    Dim i As Integer
    '    Dim lConn As HspfConnection
    '    Dim lOper As HspfOperation
    '    Dim lPoint As HspfPoint
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

    'Private Sub DrawLegendSelectBox(ByRef Index As Object, ByRef Selected As Boolean)
    '    Dim colr As Integer
    '    With picLegend(Index)
    '        If Selected Then colr = HighlightColor Else colr = System.Drawing.ColorTranslator.ToOle(picLegend(Index).BackColor)
    '        'UPGRADE_ISSUE: Constant vbFSTransparent was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
    '        'UPGRADE_ISSUE: PictureBox property picLegend.FillStyle was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '        .FillStyle = vbFSTransparent
    '        'UPGRADE_ISSUE: Constant vbSolid was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
    '        'UPGRADE_ISSUE: PictureBox property picLegend.DrawStyle was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '        .DrawStyle = vbSolid
    '        'UPGRADE_ISSUE: PictureBox property picLegend.DrawWidth was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '        .DrawWidth = 2
    '        'UPGRADE_ISSUE: PictureBox method picLegend.Line was not upgraded. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
    '    picLegend(Index).Line (1, 1) - (.Width - 1, .Height - 1), colr, B
    '        UpdateDetails()
    '    End With
    'End Sub

    'Private Function LegendSelected(ByVal aLegendTag As String) As Boolean
    '    LegendSelected = False
    '    If IsNumeric(aLegendTag) Then
    '        Dim lLegendIndex As Integer = aLegendTag
    '        If lLegendIndex >= 0 AndAlso lLegendIndex < pCurrentLegend.Icons.Count Then
    '            Return pCurrentLegend.Icons.Item(lLegendIndex).Selected
    '        End If
    '    Else
    '        For Each lLegendIcon As clsSchematicIcon In pCurrentLegend.Icons
    '            If lLegendIcon.Tag = aLegendTag Then
    '                Return lLegendIcon.Selected
    '            End If
    '        Next
    '    End If
    'End Function

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

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        picTree = New PanelDoubleBuffer
        Me.SplitLegendTree.Panel2.Controls.Add(Me.picTree)
        picTree.BackColor = SystemColors.Window
        'picTree.Dock = DockStyle.Fill
        picTree.Top = 0
        picTree.Left = 0
        picTree.Size = Me.SplitLegendTree.Panel2.ClientSize
        'picTree.Height = Me.SplitLegendTree.Panel2.Width
        Me.SplitLegendTree.Panel2.AutoScroll = True

        pOperationTypesInDiagram.Add("RCHRES")
        pOperationTypesInDiagram.Add("BMPRAC")

        LegendLandSurface.LegendType = EnumLegendType.LegLand
        LegendMetSegs.LegendType = EnumLegendType.LegMet
        LegendPointSources.LegendType = EnumLegendType.LegPoint
        pCurrentLegend = LegendLandSurface

        InitColorMap()
    End Sub

    Private Sub InitColorMap()
        ColorMap = New atcCollection
        ColorMap.Add("WaterWetlands".ToUpper, TextOrNumericColor("blue"))
        ColorMap.Add("Water/Wetland".ToUpper, TextOrNumericColor("blue"))
        ColorMap.Add("Construction".ToUpper, TextOrNumericColor("red"))
        ColorMap.Add("Institutional".ToUpper, TextOrNumericColor("azure"))
        ColorMap.Add("Agricultural".ToUpper, TextOrNumericColor("green"))
        ColorMap.Add("Forest/Open".ToUpper, TextOrNumericColor("forestgreen1"))
        ColorMap.Add("Forest".ToUpper, TextOrNumericColor("forestgreen1"))
        ColorMap.Add("Commercial".ToUpper, TextOrNumericColor("brickred"))
        ColorMap.Add("Urban".ToUpper, TextOrNumericColor("gray"))
        ColorMap.Add("LowResidential".ToUpper, TextOrNumericColor("goldenrod"))
        ColorMap.Add("MedResidential".ToUpper, TextOrNumericColor("orange"))
        ColorMap.Add("Residential".ToUpper, TextOrNumericColor("orange"))
        ColorMap.Add("MultResidential".ToUpper, TextOrNumericColor("orangered"))
        ColorMap.Add("HIGH TILL CROPLAND".ToUpper, TextOrNumericColor("darkbrown"))
        ColorMap.Add("LOW TILL CROPLAND".ToUpper, TextOrNumericColor("brown"))
        ColorMap.Add("Pasture".ToUpper, TextOrNumericColor("lightgreen"))
        ColorMap.Add("Hay".ToUpper, TextOrNumericColor("lemonchiffon"))
        ColorMap.Add("Animal/Feedlot".ToUpper, TextOrNumericColor("pink"))
    End Sub

    Private Sub tabLeft_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tabLeft.Selected
        Select Case e.TabPageIndex
            Case 0 : pCurrentLegend = LegendLandSurface
            Case 1 : pCurrentLegend = LegendMetSegs
            Case 2 : pCurrentLegend = LegendPointSources
        End Select
        UpdateLegend()
        BuildTree()
        UpdateDetails()
    End Sub

    ''' <summary>
    ''' User is clicking on an icon in the legend
    ''' </summary>
    ''' <param name="sender">Legend icon being clicked</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LegendIcon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim lSender As clsSchematicIcon = sender
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                ClickLegendIcon(lSender)
            Case Windows.Forms.MouseButtons.Right
                RightClickMenu.MenuItems.Clear()
                RightClickMenu.MenuItems.Add(lSender.Key)
                RightClickMenu.Show(lSender, e.Location)
        End Select
    End Sub

    Private Sub ClickLegendIcon(ByVal aLegendIcon As clsSchematicIcon)
        Debug.Print("ClickLegendIcon")
        Dim lChangedSchematicSelection As Boolean = False
        Select Case pCurrentLegend.LegendType
            Case EnumLegendType.LegLand
                aLegendIcon.Selected = Not aLegendIcon.Selected
                Dim lSource As HspfConnection
                For Each lIcon As clsSchematicIcon In pIcons
                    For Each lSource In lIcon.ReachOrBMP.Sources
                        If lSource.Source IsNot Nothing AndAlso lSource.Source.Opn IsNot Nothing _
                          AndAlso (lSource.Source.VolName = "PERLND" OrElse lSource.Source.VolName = "IMPLND") Then
                            Dim lKey As String = lSource.Source.Opn.Description
                            If lKey = aLegendIcon.Label Then
                                If lIcon.Selected <> aLegendIcon.Selected Then
                                    lChangedSchematicSelection = True
                                    lIcon.Selected = aLegendIcon.Selected
                                End If
                                GoTo NextLandIcon
                            End If
                        End If
                    Next
NextLandIcon:
                Next
            Case EnumLegendType.LegMet
                ''Set only the clicked icon to selected, unselect others
                'For Each lLegendIcon As clsSchematicIcon In pCurrentLegend.Icons
                '    lLegendIcon.Selected = ReferenceEquals(lLegendIcon, aLegendIcon)
                'Next
                'For Each lIcon As clsSchematicIcon In pIcons
                '    Dim lSelect As Boolean = (lIcon.Operation.MetSeg.Id = aLegendIcon.Key)
                '    If lIcon.Selected <> lSelect Then
                '        lChangedSchematicSelection = True
                '        lIcon.Selected = lSelect
                '    End If
                'Next

                'Toggle selection of the clicked icon and set selection of schematic icons with this met seg
                aLegendIcon.Selected = Not aLegendIcon.Selected
                For Each lIcon As clsSchematicIcon In pIcons
                    If lIcon.ReachOrBMP.MetSeg.Id = aLegendIcon.Key AndAlso lIcon.Selected <> aLegendIcon.Selected Then
                        lChangedSchematicSelection = True
                        lIcon.Selected = aLegendIcon.Selected
                    End If
                Next

            Case EnumLegendType.LegPoint
                aLegendIcon.Selected = Not aLegendIcon.Selected
                For Each lIcon As clsSchematicIcon In pIcons
                    For Each lPoint As HspfPointSource In lIcon.ReachOrBMP.PointSources
                        If lPoint.Id = aLegendIcon.Key AndAlso lIcon.Selected <> aLegendIcon.Selected Then
                            lChangedSchematicSelection = True
                            lIcon.Selected = aLegendIcon.Selected
                        End If
                    Next
                Next
        End Select
        pCurrentLegend.PlaceIcons()
        If lChangedSchematicSelection Then
            DrawTreeBackground()
        End If
        UpdateDetails()
    End Sub

    ''' <summary>
    ''' User is clicking on an icon in the schematic
    ''' </summary>
    ''' <param name="sender">Schematic icon being clicked</param>
    ''' <param name="e"></param>
    Private Sub Icon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim lSender As clsSchematicIcon = sender
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                pBeforeDragLocation = lSender.Location
                Dim MPosition As Point = Me.PointToClient(MousePosition)
                pDragging = True
                pDragOffset = lSender.Location - MPosition
                Windows.Forms.Cursor.Clip = Me.RectangleToScreen(New Rectangle(picTree.Left + SplitLegendTree.SplitterDistance + SplitLegendTree.SplitterWidth + e.X, _
                                                                 picTree.Top + e.Y, picTree.Width - pIconWidth, picTree.Height - pIconHeight))
            Case Windows.Forms.MouseButtons.Right
                pClickedIcon = lSender
                RightClickMenu.MenuItems.Clear()
                RightClickMenu.MenuItems.Add("""" & lSender.Key & """")
                If lSender.Label <> lSender.Key Then RightClickMenu.MenuItems.Add("""" & lSender.Label & """")
                If lSender.DistanceFromOutlet > 1 Then
                    RightClickMenu.MenuItems.Add("Select Downstream", AddressOf Event_SelectDownstream)
                End If
                If lSender.UpstreamIcons.Count > 0 Then
                    RightClickMenu.MenuItems.Add("Select Upstream", AddressOf Event_SelectUpstream)
                End If
                RightClickMenu.Show(lSender, e.Location)
        End Select
    End Sub

    ''' <summary>
    ''' Mouse is moving on an icon in the schematic, may be dragging
    ''' </summary>
    ''' <param name="sender">Schematic icon under mouse</param>
    ''' <param name="e"></param>
    Private Sub Icon_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim lSender As clsIcon = sender
        If pDragging Then
            'move control to new position
            Dim MPosition As Point = Me.PointToClient(MousePosition)
            MPosition.Offset(pDragOffset)
            lSender.Location = MPosition
            DrawTreeBackground() '(lSender)
        End If
    End Sub

    ''' <summary>
    ''' User is releasing mouse button on an icon in the schematic
    ''' </summary>
    ''' <param name="sender">Schematic icon being clicked</param>
    ''' <param name="e"></param>
    Private Sub Icon_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                If pDragging Then 'end the dragging
                    pDragging = False
                    Windows.Forms.Cursor.Clip = Nothing
                    If sender.Location = pBeforeDragLocation Then
                        sender.Selected = Not sender.Selected
                        UpdateDetails()
                    End If
                    DrawTreeBackground()
                    'sender.Invalidate()
                End If
        End Select
    End Sub

    Private Sub Event_SelectDownstream(ByVal sender As Object, ByVal e As System.EventArgs)
        SelectDownstreamIcons(True, pClickedIcon)
        DrawTreeBackground()
    End Sub

    Private Sub Event_SelectUpstream(ByVal sender As Object, ByVal e As System.EventArgs)
        SelectUpstreamIcons(True, pClickedIcon)
        DrawTreeBackground()
    End Sub

    Private Sub Event_SelectAll(ByVal sender As Object, ByVal e As System.EventArgs)
        SelectAllIcons(True)
        DrawTreeBackground()
    End Sub

    Private Sub Event_UnselectAll(ByVal sender As Object, ByVal e As System.EventArgs)
        SelectAllIcons(False)
        DrawTreeBackground()
    End Sub

    Private Sub SelectDownstreamIcons(ByVal aSelect As Boolean, ByVal aIcon As clsSchematicIcon)
        aIcon.Selected = True
        For Each lIcon As clsSchematicIcon In aIcon.DownstreamIcons
            SelectDownstreamIcons(aSelect, lIcon)
        Next
    End Sub

    Private Sub SelectUpstreamIcons(ByVal aSelect As Boolean, ByVal aIcon As clsSchematicIcon)
        aIcon.Selected = True
        For Each lIcon As clsSchematicIcon In aIcon.UpstreamIcons
            SelectUpstreamIcons(aSelect, lIcon)
        Next
    End Sub

    Private Sub SelectAllIcons(ByVal aSelect As Boolean)
        For Each lIcon As clsIcon In pIcons
            lIcon.Selected = aSelect
        Next
    End Sub

    Private Sub picTree_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picTree.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                'TODO: start select rectangle
            Case Windows.Forms.MouseButtons.Right
                RightClickMenu.MenuItems.Clear()
                RightClickMenu.MenuItems.Add("Select All", AddressOf Event_SelectAll)
                RightClickMenu.MenuItems.Add("Unselect All", AddressOf Event_UnselectAll)
                RightClickMenu.Show(sender, e.Location)
        End Select

    End Sub

    Private Sub pCurrentLegend_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pCurrentLegend.Resize
        UpdateLegend()
    End Sub

    Private Sub ctlSchematic_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        RefreshDetails
    End Sub

    Public Sub RefreshDetails()
        agdDetails.SizeAllColumnsToContents(-1)
        agdDetails.Refresh()
    End Sub

    Private Sub SplitLegendTree_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitLegendTree.SizeChanged
        LayoutTree()
    End Sub

    Private Sub SplitLegendTree_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitLegendTree.SplitterMoved
        LayoutTree()
    End Sub
End Class
