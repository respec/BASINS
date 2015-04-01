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

    Public AllIcons As New IconCollection
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
    Public IconWidth As Integer = 150
    Public IconHeight As Integer = 150
    Private pBorderWidth As Integer = 3
    Private pTreeBackground As Bitmap

    Private LegendOrder As Generic.List(Of String)

    Private Sub RemoveIconHandlers(aIcon As clsIcon)
        RemoveHandler aIcon.MouseDown, AddressOf Icon_MouseDown
        RemoveHandler aIcon.MouseMove, AddressOf Icon_MouseMove
        RemoveHandler aIcon.MouseUp, AddressOf Icon_MouseUp
        RemoveHandler aIcon.DragDrop, AddressOf Icon_DragDrop
        RemoveHandler aIcon.DragEnter, AddressOf Icon_DragEnter
    End Sub

    Public Function AllScenarioNames() As SortedSet(Of String)
        Dim lAllNames As New SortedSet(Of String)
        For Each lIcon As clsIcon In AllIcons
            For Each lScenario As clsUciScenario In lIcon.Scenarios
                If Not lAllNames.Contains(lScenario.ScenarioName) Then
                    lAllNames.Add(lScenario.ScenarioName)
                End If
            Next
        Next
        Return lAllNames
    End Function

    Public Sub BuildTree(ByVal aIcons As IconCollection, Optional ByVal aPrinting As Boolean = False)
        'Clear Tree
        Dim lSameIcons As Boolean = ReferenceEquals(aIcons, AllIcons)

        picTree.SuspendLayout()
        If picTree.BackgroundImage IsNot Nothing Then
            picTree.BackgroundImage.Dispose()
            picTree.BackgroundImage = Nothing 'New Bitmap(0, 0, Drawing.Imaging.PixelFormat.Format32bppArgb)
        End If
        picTree.Controls.Clear()

        For Each lOldIcon As clsIcon In AllIcons
            RemoveIconHandlers(lOldIcon)
        Next
        If Not lSameIcons Then
            For Each lControl As Control In AllIcons
                lControl.Dispose()
            Next
            AllIcons.Clear()
        End If
        picTree.ResumeLayout()
        pIconsDistantFromOutlet.Clear()
        pIconsDistantFromOutletPlaced.Clear()
        pOutlets.Clear()

        Dim lNodeSize As New Drawing.Size(IconWidth, IconHeight)

        'Will be set True if any icons need a new location (because they are at zero)
        Dim lRefreshingLayout As Boolean = False

        For Each lNewIcon As clsIcon In aIcons
            With lNewIcon
                .Size = lNodeSize
            End With
            AddHandler lNewIcon.MouseDown, AddressOf Icon_MouseDown
            AddHandler lNewIcon.MouseMove, AddressOf Icon_MouseMove
            AddHandler lNewIcon.MouseUp, AddressOf Icon_MouseUp
            AddHandler lNewIcon.DragDrop, AddressOf Icon_DragDrop
            AddHandler lNewIcon.DragEnter, AddressOf Icon_DragEnter
            If Not AllIcons.Contains(lNewIcon) Then AllIcons.Add(lNewIcon)
            picTree.Controls.Add(lNewIcon)
            If lNewIcon.Location.X = 0 AndAlso lNewIcon.Location.Y = 0 Then lRefreshingLayout = True
            DrawIcon(False, lNewIcon)
        Next

        If AllIcons.Count > 0 Then 'okay to do tree

            pMaximumTreeDepth = 1

            For Each lIcon As clsIcon In AllIcons
                With lIcon
                    .DistanceFromOutlet = DownLayers(lIcon, lIcon.Key & vbCrLf)
                    pIconsDistantFromOutlet.Increment(.DistanceFromOutlet, 1)
                    'Debug.WriteLine(.DistanceFromOutlet & ":" & pIconsDistantFromOutlet.ItemByKey(.DistanceFromOutlet))
                    If .DistanceFromOutlet > pMaximumTreeDepth Then
                        pMaximumTreeDepth = .DistanceFromOutlet
                    End If
                    If .DistanceFromOutlet = 1 AndAlso Not pOutlets.Contains(lIcon) Then
                        pOutlets.Add(lIcon)
                    End If
                End With
            Next
            'For Each lIcon In AllIcons
            '    For Each lUpIcon As clsIcon In lIcon.UpstreamIcons
            '        lUpIcon.DownstreamIcons.Add(lIcon)
            '    Next
            'Next
            If lRefreshingLayout Then
                LayoutTree(aPrinting)
            Else
                DrawTreeBackground()
            End If
            'SetScrollVisible()
        End If
    End Sub

    'Public Sub SetScrollVisible()
    '    Dim lMinX As Integer = Me.Width
    '    Dim lMaxX As Integer = 0
    '    Dim lMinY As Integer = Me.Height
    '    Dim lMaxY As Integer = 0
    '    For Each lNewIcon As clsIcon In AllIcons
    '        If lNewIcon.Left < lMinX Then lMinX = lNewIcon.Left
    '        If lNewIcon.Right > lMaxX Then lMaxX = lNewIcon.Right
    '        If lNewIcon.Top < lMinY Then lMinY = lNewIcon.Top
    '        If lNewIcon.Bottom > lMaxY Then lMaxY = lNewIcon.Bottom
    '    Next
    '    If lMinX < 0 OrElse lMaxX > Me.Width Then
    '        Me.HScroll = True
    '    Else
    '        Me.HScroll = False
    '    End If
    '    If lMinY < 0 OrElse lMaxY > Me.Height Then
    '        Me.VScroll = True
    '    Else
    '        Me.VScroll = False
    '    End If
    'End Sub

    Private Sub LayoutTree(Optional ByVal aPrinting As Boolean = False)
        Static lInLayout As Boolean = False
        If AllIcons.Count > 0 Then 'okay to do tree
            If lInLayout Then Exit Sub
            lInLayout = True
            Try
                pIconsDistantFromOutletPlaced = New atcCollection
                Dim drawsurface As Control
                Dim dy As Integer

                'If Printing Then
                'TODO: use new System.Drawing.Printing:
                'Public Sub prt(ByVal text As String)
                '    TextToBePrinted = text
                '    Dim prn As New Printing.PrintDocument
                '    Using (prn)
                '        prn.PrinterSettings.PrinterName _
                '           = "PrinterName" 'Printing.PrinterSettings.InstalledPrinters.Item(0)
                '        AddHandler prn.PrintPage, _
                '           AddressOf Me.PrintPageHandler
                '        prn.Print()
                '        RemoveHandler prn.PrintPage, _
                '           AddressOf Me.PrintPageHandler
                '    End Using
                'End Sub
                'Private Sub PrintPageHandler(ByVal sender As Object, _
                '   ByVal args As Printing.PrintPageEventArgs)
                '    Dim myFont As New Font("Microsoft San Serif", 10)
                '    args.Graphics.DrawString(TextToBePrinted, _
                '       New Font(myFont, FontStyle.Regular), _
                '       Brushes.Black, 50, 50)
                'End Sub
                '
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
                If pMaximumTreeDepth > 1 Then dy = (picTree.Height - IconHeight * 2) / (pMaximumTreeDepth - 1)
                If dy < IconHeight * 1.5 Then
                    dy = IconHeight * 1.5
                    'TODO: enable vertical scrolling
                End If
                drawsurface = picTree
                picTree.SuspendLayout()
                'End If 'Printing

                picTree.Size = Me.ClientSize
                Dim Ybase As Integer = drawsurface.Height - IconHeight
                pHeightNeeded = 0
                pWidthNeeded = 0

                For Each lOutlet As clsIcon In pOutlets
                    LayoutFromIcon(lOutlet, Ybase, dy, drawsurface.Width, aPrinting)
                Next

                If pHeightNeeded > picTree.Height Then
                    Dim lMove As Integer = pHeightNeeded - picTree.Height
                    picTree.Height = pHeightNeeded
                    For Each lControl As clsIcon In AllIcons
                        lControl.Top += lMove
                    Next
                End If
                If pWidthNeeded > picTree.Width Then
                    picTree.Width = pWidthNeeded
                End If

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

    Private Sub CheckScrollbars()
        Dim lMinX As Integer = picTree.Width
        Dim lMaxX As Integer = 0
        Dim lMinY As Integer = picTree.Height
        Dim lMaxY As Integer = 0

        Dim lDx As Integer = 0
        Dim lDy As Integer = 0

        For Each lIcon As clsIcon In AllIcons
            With lIcon
                If .Top < lMinY Then lMinY = .Top
                If .Bottom > lMaxY Then lMaxY = .Bottom
                If .Left < lMinX Then lMinX = .Left
                If .Right > lMaxX Then lMaxX = .Right
            End With
        Next

        Dim lAvailableWidth As Integer = Me.ClientRectangle.Width '- VScroller.Width
        Dim lAvailableHeight As Integer = Me.ClientRectangle.Height '- HScroller.Height

        If lMaxX - lMinX < lAvailableWidth Then
            'HScroller.Visible = False
            'If lMaxX > lAvailableWidth OrElse lMinX < 0 Then
            lDx = lMinX - (lAvailableWidth - (lMaxX - lMinX)) / 2
            'End If
        Else
            'HScroller.Visible = True
            'HScroller.Maximum = lAvailableWidth - (lMaxX - lMinX)
        End If

        If lMaxY - lMinY < lAvailableHeight Then
            'VScroller.Visible = False
            'If lMaxY > lAvailableHeight OrElse lMinY < 0 Then
            lDy = lMinY - (lAvailableHeight - (lMaxY - lMinY)) / 2
            'End If
        Else
            'VScroller.Visible = True
            'VScroller.Maximum = lAvailableHeight - (lMaxY - lMinY)
        End If

        If Not pDragging AndAlso (lDx <> 0 OrElse lDy <> 0) Then
            For Each lIcon As clsIcon In AllIcons
                With lIcon
                    .Top -= lDy
                    .Left -= lDx
                End With
            Next
        End If
    End Sub

    ''' <summary>
    ''' Draw the connecting lines and selection halos behind the tree nodes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DrawTreeBackground()
        If picTree IsNot Nothing Then
            Try
                CheckScrollbars()
                Dim lDx As Integer = 0, lMaxDx As Integer = 0
                Dim lDy As Integer = 0, lMaxDy As Integer = 0
                'If HScroller.Visible Then
                '    lDx = HScroller.Value
                '    lMaxDx = HScroller.Maximum
                'End If
                'If VScroller.Visible Then
                '    lDy = VScroller.Value
                '    lMaxDy = VScroller.Maximum
                'End If

                Dim lGoodLinePen As Pen = SystemPens.ControlDarkDark
                Dim lBadLinePen As Pen = New Drawing.Pen(Brushes.Salmon)
                Dim lLinePen As Pen = lGoodLinePen

                If pTreeBackground IsNot Nothing Then pTreeBackground.Dispose()
                pTreeBackground = New Bitmap(picTree.Width + lMaxDx, picTree.Height + lMaxDy, Drawing.Imaging.PixelFormat.Format32bppArgb)
                Dim lGraphics As Graphics = Graphics.FromImage(pTreeBackground)
                For Each lIcon As clsIcon In AllIcons
                    With lIcon
                        If lIcon.DownstreamIcon IsNot Nothing Then
                            'TODO: assign lLinePen to lGoodLinePen or lBadLinePen
                            lGraphics.DrawLine(lLinePen, lIcon.Center, lIcon.DownstreamIcon.Center)
                        End If

                        If .Selected Then
                            lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
                        End If
                    End With
                Next
                lGraphics.Dispose()
                picTree.BackgroundImage = pTreeBackground
            Catch e As Exception
                Logger.Dbg(e.ToString)
            End Try
        End If
    End Sub

    'Private Sub DrawTreeBackground(ByVal aIcon As clsIcon)
    '    Dim lGraphics As Graphics = Graphics.FromImage(picTree.BackgroundImage)
    '    Dim lLinesPen As Pen = SystemPens.ControlDarkDark
    '    'Dim lClipLeft As Integer = aIcon.Left
    '    'Dim lClipRight As Integer = aIcon.Right
    '    'Dim lClipTop As Integer = aIcon.Top
    '    'Dim lClipBottom As Integer = aIcon.Bottom

    '    'ExtendClip(lClipLeft, lClipRight, lClipTop, lClipBottom, aIcon.UpstreamIcons)
    '    'ExtendClip(lClipLeft, lClipRight, lClipTop, lClipBottom, aIcon.DownstreamIcons)

    '    'lGraphics.SetClip(New Rectangle(lClipLeft - pIconWidth, lClipTop - pIconWidth, lClipRight - lClipLeft + pIconWidth * 2, lClipBottom - lClipTop + pIconWidth * 2))
    '    lGraphics.Clear(SystemColors.Window)

    '    With aIcon
    '        Dim lIconCenter As Point = .Center
    '        For Each lUpstreamIcon As clsIcon In aIcon.UpstreamIcons
    '            With lUpstreamIcon
    '                lGraphics.DrawLine(lLinesPen, lIconCenter, .Center)
    '                If .Selected Then lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
    '                .Invalidate()
    '            End With
    '        Next
    '        With aIcon.DownstreamIcon
    '            lGraphics.DrawLine(lLinesPen, lIconCenter, .Center)
    '            If .Selected Then lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
    '            .Invalidate()
    '        End With
    '        If .Selected Then
    '            lGraphics.FillRectangle(HighlightBrush, .Left - pBorderWidth, .Top - pBorderWidth, .Width + pBorderWidth * 2, .Height + pBorderWidth * 2)
    '        End If
    '    End With

    '    aIcon.Invalidate()
    '    lGraphics.Dispose()
    '    'picTree.BackgroundImage = pTreeBackground
    '    picTree.Invalidate()
    'End Sub

    Private Sub ExtendClip(ByRef lClipLeft As Integer, ByRef lClipRight As Integer, ByRef lClipTop As Integer, ByRef lClipBottom As Integer, ByVal aIcons As Generic.List(Of clsIcon))
        For Each lIcon As clsIcon In aIcons
            If lIcon.Left < lClipLeft Then lClipLeft = lIcon.Left
            If lIcon.Right > lClipRight Then lClipRight = lIcon.Right
            If lIcon.Top < lClipTop Then lClipTop = lIcon.Top
            If lIcon.Bottom > lClipBottom Then lClipBottom = lIcon.Bottom
        Next
    End Sub

    Private Sub DrawIcon(ByVal aPrinting As Boolean, ByVal aIcon As clsIcon)
        Dim lBitmap As New Bitmap(IconWidth, IconHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(lBitmap)

        g.Clear(aIcon.BackColor)

        If aIcon.Font.Size < 8.25 Then
            aIcon.Font.Dispose()
            aIcon.Font = New Font("Microsoft Sans Serif", 8.25)
        End If
        Dim lLabel As String = aIcon.WatershedName

        Dim lStringMeasurement As Drawing.SizeF = g.MeasureString(lLabel, aIcon.Font)
        While lStringMeasurement.Width > IconWidth
            Dim lOldSize As Single = aIcon.Font.Size
            aIcon.Font = New Font("Microsoft Sans Serif", aIcon.Font.Size - 1)
            If aIcon.Font.Size = lOldSize Then Exit While
            lStringMeasurement = g.MeasureString(lLabel, aIcon.Font)
        End While
        Dim lStringX As Single = (IconWidth - lStringMeasurement.Width) / 2
        Dim lStringY As Single = 10
        Dim lImageTop As Single = 10 + lStringMeasurement.Height * 1.25
        g.DrawString(lLabel, aIcon.Font, SystemBrushes.ControlDarkDark, lStringX, lStringY)

        If aIcon.Font.Size < 8.25 Then
            aIcon.Font.Dispose()
            aIcon.Font = New Font("Microsoft Sans Serif", 8.25)
        End If
        lLabel = aIcon.Scenario.ScenarioName

        lStringMeasurement = g.MeasureString(lLabel, aIcon.Font)
        While lStringMeasurement.Width > IconWidth
            Dim lOldSize As Single = aIcon.Font.Size
            aIcon.Font = New Font("Microsoft Sans Serif", aIcon.Font.Size - 1)
            If aIcon.Font.Size = lOldSize Then Exit While
            lStringMeasurement = g.MeasureString(lLabel, aIcon.Font)
        End While
        lStringX = (IconWidth - lStringMeasurement.Width) / 2
        lStringY = IconHeight - lStringMeasurement.Height * 1.25
        g.DrawString(lLabel, aIcon.Font, SystemBrushes.ControlDarkDark, lStringX, lStringY)

        Dim lIconHeightAvailable As Single = lStringY - lImageTop - 2
        If aIcon.WatershedImage IsNot Nothing Then
            Dim lScaleWidth As Single = (IconWidth - 2) / aIcon.WatershedImage.Width
            Dim lScaleHeight As Single = (lIconHeightAvailable) / aIcon.WatershedImage.Height
            Dim lScale As Single = Math.Min(lScaleHeight, lScaleWidth)
            g.DrawImage(aIcon.WatershedImage, _
                        (IconWidth - lScale * aIcon.WatershedImage.Width) / 2, _
                        lImageTop + (lIconHeightAvailable - lScale * aIcon.WatershedImage.Height) / 2, _
                        lScale * aIcon.WatershedImage.Width, _
                        lScale * aIcon.WatershedImage.Height)
        End If

        DrawBorder(g, IconWidth, IconHeight, Not aPrinting)
        g.Dispose()
        aIcon.BackgroundImage = lBitmap
    End Sub

    Private Sub DrawBorder(ByVal pic As Graphics, ByVal aWidth As Integer, ByVal aHeight As Integer, ByVal threeD As Boolean)
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
    Private Function DownLayers(ByVal aIcon As clsIcon, ByVal aAlreadyInPath As String) As Integer
        Dim lDeepestTarget As Integer = 0
        If aIcon.DownstreamIcon IsNot Nothing Then
            Dim lDownstreamKey As String = aIcon.DownstreamIcon.Key
            If aAlreadyInPath.Contains(lDownstreamKey & vbCrLf) AndAlso Not TreeLoopMessageDisplayed Then
                Dim lMessage As String = aAlreadyInPath
                Logger.Msg(aAlreadyInPath, "Detected possible loop in operations")
                TreeLoopMessageDisplayed = True
            Else
                If AllIcons.Contains(lDownstreamKey) Then
                    aAlreadyInPath &= lDownstreamKey & vbCrLf
                    Dim lIcon As clsIcon = AllIcons(lDownstreamKey)
                    If Not lIcon.UpstreamIcons.Contains(aIcon) Then lIcon.UpstreamIcons.Add(aIcon)
                    Dim lTargetDepth As Integer = DownLayers(lIcon, aAlreadyInPath)
                    If lTargetDepth > lDeepestTarget Then
                        lDeepestTarget = lTargetDepth
                    End If
                End If
            End If
        End If
        Return lDeepestTarget + 1
    End Function

    Private Sub LayoutFromIcon(ByVal aIcon As clsIcon, ByVal aY As Integer, ByVal aDy As Integer, ByVal aWidth As Integer, ByVal aPrinting As Boolean)
        With aIcon
            aIcon.Top = aY - IconHeight / 2
            Dim lWidthPerItemThisRow As Integer = aWidth / pIconsDistantFromOutlet.ItemByKey(.DistanceFromOutlet)
            If lWidthPerItemThisRow < aIcon.Width * 1.25 Then lWidthPerItemThisRow = aIcon.Width * 1.25
            aIcon.Left = pIconsDistantFromOutletPlaced.ItemByKey(.DistanceFromOutlet) * lWidthPerItemThisRow + (lWidthPerItemThisRow - IconWidth) / 2
            pIconsDistantFromOutletPlaced.Increment(.DistanceFromOutlet, 1)
            Dim lNumUpstream As Integer = aIcon.UpstreamIcons.Count
            If lNumUpstream > 0 Then
                aY -= aDy
                For Each lUpstreamIcon As clsIcon In aIcon.UpstreamIcons
                    LayoutFromIcon(lUpstreamIcon, aY, aDy, aWidth, aPrinting)
                Next
            End If
            Dim lHeightNeeded = picTree.Height - aY + IconHeight
            If lHeightNeeded > pHeightNeeded Then pHeightNeeded = lHeightNeeded
            Dim lWidthNeeded = aIcon.Left + IconWidth * 1.5
            If lWidthNeeded > pWidthNeeded Then pWidthNeeded = lWidthNeeded
        End With
    End Sub

    'Private Sub PopulateLandGrid()
    '    Dim lConn As HspfConnection
    '    Dim lDesc As String
    '    Dim AddedThisReach As Boolean
    '    Dim ReachNames, LastName As String
    '    Dim PgrandTotal, Ptotal, Itotal, IgrandTotal As Single
    '    Dim t As Double = 0
    '    Dim AreaUnits As String
    '    Dim lRow As Integer

    '    If pUci Is Nothing OrElse pUci.Name.Length = 0 Then Exit Sub

    '    If pUci.GlobalBlock.EmFg = 1 Then
    '        AreaUnits = " (Acres)"
    '    Else
    '        AreaUnits = " (Hectares)"
    '    End If

    '    Dim lDetailsSource As New atcControls.atcGridSource
    '    With lDetailsSource
    '        .Rows = 1
    '        .FixedRows = 1
    '        .Columns = 5
    '        .CellValue(0, 0) = "Land Use"
    '        .CellValue(0, 1) = "Reaches"
    '        .CellValue(0, 2) = "Implnd" & AreaUnits
    '        .CellValue(0, 3) = "Perlnd" & AreaUnits
    '        .CellValue(0, 4) = "Total" & AreaUnits

    '        '.set_ColType(2, ATCoCtl.ATCoDataType.ATCoTxt)
    '        '.set_ColType(3, ATCoCtl.ATCoDataType.ATCoTxt)
    '        '.set_ColType(4, ATCoCtl.ATCoDataType.ATCoTxt)
    '        For Each lLegendIcon As clsIcon In pCurrentLegend.Icons
    '            If lLegendIcon.Selected Then
    '                Ptotal = 0
    '                Itotal = 0
    '                ReachNames = ""
    '                LastName = ""
    '                lDesc = lLegendIcon.Key
    '                lRow = .Rows
    '                .CellValue(lRow, 0) = lDesc
    '                For Each lReach As clsIcon In pIcons
    '                    AddedThisReach = False
    '                    If lReach.Selected Then
    '                        Dim lOperation As HspfOperation = lReach.ReachOrBMP
    '                        For Each lConn In lOperation.Sources
    '                            Select Case lConn.Source.VolName
    '                                Case "PERLND"
    '                                    If lConn.Source.Opn IsNot Nothing Then
    '                                        If lDesc = DescToLabel(lConn.Source.Opn.Description) Then
    '                                            Ptotal = Ptotal + lConn.MFact

    '                                            If Not AddedThisReach Then
    '                                                If lOperation.Name <> LastName Then
    '                                                    LastName = lOperation.Name
    '                                                    ReachNames = ReachNames & LastName & " "
    '                                                End If
    '                                                ReachNames = ReachNames & lOperation.Id & ", "
    '                                                AddedThisReach = True
    '                                            End If

    '                                        End If
    '                                    End If
    '                                Case "IMPLND"
    '                                    If lConn.Source.Opn IsNot Nothing Then
    '                                        If lDesc = DescToLabel(lConn.Source.Opn.Description) Then 'lConn.Source.Opn.Name & " " & lConn.Source.Opn.Id Then
    '                                            Itotal = Itotal + lConn.MFact

    '                                            If Not AddedThisReach Then
    '                                                If lOperation.Name <> LastName Then
    '                                                    LastName = lOperation.Name
    '                                                    ReachNames = ReachNames & LastName & " "
    '                                                End If
    '                                                ReachNames = ReachNames & lOperation.Id & ", "
    '                                                AddedThisReach = True
    '                                            End If

    '                                        End If
    '                                    End If
    '                            End Select
    '                        Next
    '                    End If
    '                Next
    '                If Len(ReachNames) < 2 Then
    '                    .CellValue(lRow, 1) = ""
    '                Else 'remove final ", "
    '                    .CellValue(lRow, 1) = ReachNames.Substring(0, ReachNames.Length - 2)
    '                End If
    '                .CellValue(lRow, 2) = DoubleToString(Itotal, 8)
    '                .CellValue(lRow, 3) = DoubleToString(Ptotal, 8)
    '                .CellValue(lRow, 4) = DoubleToString(Ptotal + Itotal, 8)
    '                PgrandTotal = PgrandTotal + Ptotal
    '                IgrandTotal = IgrandTotal + Itotal
    '            End If
    '        Next
    '        lRow = .Rows
    '        .CellValue(lRow, 0) = "Total"
    '        .CellValue(lRow, 1) = ""
    '        .CellValue(lRow, 2) = DoubleToString(IgrandTotal, 8)
    '        .CellValue(lRow, 3) = DoubleToString(PgrandTotal, 8)
    '        .CellValue(lRow, 4) = DoubleToString(PgrandTotal + IgrandTotal, 8)
    '        '    For i = 0 To picReach.Count - 1
    '        '      If ReachSelected(i) Then
    '        '        Set lOper = lOpns(CLng(picReach(i).tag))
    '        '        For Each vConn In lOper.Sources
    '        '          Set lConn = vConn
    '        '          'Debug.Print lConn.source.volname
    '        '          If lConn.source.volname = "PERLND" Or lConn.source.volname = "IMPLND" Then
    '        '            lDesc = lConn.source.Opn.Description
    '        '            If LegendSelected(lDesc) Then
    '        '              .rows = .rows + 1
    '        '              .TextMatrix(.rows, 0) = lConn.source.volname & " " & lConn.source.volid
    '        '              .TextMatrix(.rows, 1) = lDesc
    '        '              .TextMatrix(.rows, 2) = lOper.Name & " " & lOper.id
    '        '              .TextMatrix(.rows, 3) = lConn.MFact
    '        '              t = t + lConn.MFact
    '        '            End If
    '        '          End If
    '        '        Next vConn
    '        '      End If
    '        '    Next i
    '        '    lblTotal(0) = "Total: " & t
    '        '    lblTotal(1).Visible = False
    '        '    lblTotal(2).Visible = False
    '        '    OrigTotal = t
    '    End With
    '    agdDetails.Initialize(lDetailsSource)
    'End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.DoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        UpdateStyles()

        picTree = New PanelDoubleBuffer
        Me.Controls.Add(Me.picTree)
        picTree.BackColor = SystemColors.Window
        'picTree.Dock = DockStyle.Fill
        picTree.Top = 0
        picTree.Left = 0
        'picTree.Size = Me.ClientSize
        picTree.Dock = DockStyle.Fill
        'picTree.Height = Me.SplitLegendTree.Panel2.Width
        'picTree.AutoScroll = True

        'LegendLandSurface.LegendType = EnumLegendType.LegLand
        'LegendMetSegs.LegendType = EnumLegendType.LegMet
        'LegendPointSources.LegendType = EnumLegendType.LegPoint
        'pCurrentLegend = LegendLandSurface

        'InitColorMap()
    End Sub

    ''' <summary>
    ''' User is clicking on an icon in the schematic
    ''' </summary>
    ''' <param name="sender">Schematic icon being clicked</param>
    ''' <param name="e"></param>
    Private Sub Icon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim lIcon As clsIcon = sender
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                pBeforeDragLocation = lIcon.Location
                Dim MPosition As Point = Me.PointToClient(MousePosition)
                pDragging = True
                pDragOffset = lIcon.Location - MPosition
                Windows.Forms.Cursor.Clip = Me.RectangleToScreen(New Rectangle(picTree.Left + e.X, _
                                                                 picTree.Top + e.Y, picTree.Width - IconWidth, picTree.Height - IconHeight))
            Case Windows.Forms.MouseButtons.Right
                pClickedIcon = lIcon
                RightClickMenu.MenuItems.Clear()
                RightClickMenu.MenuItems.Add("Edit", AddressOf EditIcon)
                RightClickMenu.MenuItems.Add("Remove", AddressOf RemoveClickedIcon)
                RightClickMenu.MenuItems.Add("Open Folder", AddressOf OpenFolder)
                'RightClickMenu.MenuItems.Add("Open in WinHSPF: """ & lSender.Key & """")
                'If lSender.Label <> lSender.Key Then RightClickMenu.MenuItems.Add("""" & lSender.Label & """")
                If lIcon.DistanceFromOutlet > 1 Then
                    RightClickMenu.MenuItems.Add("Select Downstream", AddressOf Event_SelectDownstream)
                End If
                If lIcon.UpstreamIcons.Count > 0 Then
                    RightClickMenu.MenuItems.Add("Select Upstream", AddressOf Event_SelectUpstream)
                End If
                RightClickMenu.Show(lIcon, e.Location)
        End Select
    End Sub

    Public Sub EditIcon(ByVal aSender As Object, ByVal e As System.EventArgs)
        Dim lWatershedForm As New frmEditWatershed
        lWatershedForm.Schematic = Me
        lWatershedForm.ModelIcon = pClickedIcon
        lWatershedForm.Show()
    End Sub

    Public Sub RemoveClickedIcon(ByVal aSender As Object, ByVal e As System.EventArgs)
        RemoveIconAfterAsking(pClickedIcon)
    End Sub

    ''' <summary>
    ''' Ask user whether to remove the given watershed icon from the diagram
    ''' </summary>
    ''' <param name="aIcon">Watershed icon to remove</param>
    ''' <returns>True if icon was removed, False if icon was not removed</returns>
    ''' <remarks>pClickedIcon is cleared if that was the one removed</remarks>
    Public Function RemoveIconAfterAsking(aIcon As clsIcon) As Boolean
        If Logger.Msg("Remove watershed " & aIcon.WatershedName & "?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            AllIcons.Remove(aIcon)
            RemoveIconHandlers(aIcon)

            If aIcon.DownstreamIcon IsNot Nothing AndAlso _
                aIcon.DownstreamIcon.UpstreamIcons.Contains(aIcon) Then
                aIcon.DownstreamIcon.UpstreamIcons.Remove(aIcon)
            End If

            If Object.ReferenceEquals(aIcon, pClickedIcon) Then
                pClickedIcon = Nothing
            End If
            BuildTree(AllIcons)
            Return True
        End If
        Return False
    End Function

    Public Sub OpenFolder(ByVal aSender As Object, ByVal e As System.EventArgs)
        OpenFile(IO.Path.GetDirectoryName(pClickedIcon.UciFileName))
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
        Dim lSender As clsIcon = sender
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                If pDragging Then 'end the dragging
                    pDragging = False
                    Windows.Forms.Cursor.Clip = Nothing
                    'If same location, count as click rather than a drag
                    If lSender.Location = pBeforeDragLocation Then

                        lSender.Selected = Not lSender.Selected
                        'UpdateDetails()
                    End If
                    DrawTreeBackground()
                    'sender.Invalidate()
                End If
        End Select
    End Sub

    Private Sub Icon_DragDrop(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lIcon As clsIcon = sender
            For Each lFileName As String In e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
                Select Case IO.Path.GetExtension(lFileName).ToLower
                    Case ".uci"
                        If Logger.Msg("Set " & lIcon.WatershedName & " UCI file to:" & vbCrLf & lFileName & " ?", MsgBoxStyle.YesNo, frmHspfSimulationManager.g_AppNameLong) = MsgBoxResult.Yes Then
                            lIcon.UciFileName = lFileName
                            DrawIcon(False, lIcon)
                        End If

                    Case ".png", ".jpg", ".gif"
                        lIcon.WatershedImageFilename = lFileName
                        lIcon.WatershedImage = Drawing.Image.FromFile(lFileName)
                        DrawIcon(False, lIcon)

                End Select
            Next
        End If
    End Sub

    Private Sub Icon_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            If lFileNames.Length > 0 AndAlso lFileNames(0).ToLower.EndsWith(".uci") Then
                e.Effect = Windows.Forms.DragDropEffects.All
            End If
        End If
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

    Private Sub SelectDownstreamIcons(ByVal aSelect As Boolean, ByVal aIcon As clsIcon)
        aIcon.Selected = True
        If aIcon.DownstreamIcon IsNot Nothing Then
            SelectDownstreamIcons(aSelect, aIcon.DownstreamIcon)
        End If
    End Sub

    Private Sub SelectUpstreamIcons(ByVal aSelect As Boolean, ByVal aIcon As clsIcon)
        aIcon.Selected = True
        For Each lIcon As clsIcon In aIcon.UpstreamIcons
            SelectUpstreamIcons(aSelect, lIcon)
        Next
    End Sub

    Private Sub SelectAllIcons(ByVal aSelect As Boolean)
        For Each lIcon As clsIcon In AllIcons
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

    Private Sub ctlSchematic_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            For Each lFileName As String In e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
                Select Case MsgBox("Add new watershed for UCI file " & lFileName & " ?", MsgBoxStyle.YesNoCancel, frmHspfSimulationManager.g_AppNameLong)
                    Case MsgBoxResult.Yes
                        Dim lIcon = AllIcons.FindOrAddIcon(IO.Path.GetFileNameWithoutExtension(lFileName))
                        lIcon.UciFileName = lFileName
                        If lIcon.Left = 0 AndAlso lIcon.Top = 0 Then
                            Dim lOffset As Drawing.Point = Me.PointToScreen(New Drawing.Point(0, 0))
                            lIcon.Left = e.X - lOffset.X - IconWidth / 2
                            If lIcon.Left <= 0 Then
                                lIcon.Left = 10
                            ElseIf lIcon.Left + IconWidth > Width Then
                                lIcon.Left = Width - IconWidth - 10
                            End If

                            lIcon.Top = e.Y - lOffset.Y - IconHeight / 2
                            If lIcon.Top <= 0 Then
                                lIcon.Top = 10
                            ElseIf lIcon.Top + IconHeight > Height Then
                                lIcon.Top = Height - IconHeight - 10
                            End If
                        End If

                        Dim lWatershedForm As New frmEditWatershed
                        lWatershedForm.Schematic = Me
                        lWatershedForm.ModelIcon = lIcon
                        lWatershedForm.Show()
                    Case MsgBoxResult.Cancel
                        Exit For
                End Select
            Next
        End If
    End Sub

    Private Sub ctlSchematic_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)
            If lFileNames.Length > 0 AndAlso lFileNames(0).ToLower.EndsWith(".uci") Then
                e.Effect = Windows.Forms.DragDropEffects.All
            End If
        End If
    End Sub

    Private Sub Scroll_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel, picTree.MouseWheel ', HScroller.MouseWheel, VScroller.MouseWheel
        Dim lScale As Single
        If e.Delta > 0 Then
            lScale = 1.111111
        Else
            lScale = 0.9
        End If

        If IconHeight * lScale < 50 Then
            lScale = 50 / IconHeight
        ElseIf IconHeight * lScale > 500 Then
            lScale = 500 / IconHeight
        End If

        IconHeight *= lScale
        IconWidth *= lScale
        'Dim lScale As Single = (pIconHeight + lDelta) / pIconHeight


        Dim lNodeSize As New Drawing.Size(IconWidth, IconHeight)

        For Each lIcon As clsIcon In AllIcons
            With lIcon
                'If .Left * lScale <= 0 OrElse .Top * lScale <= 0 Then
                '    Stop
                'End If
                .Size = lNodeSize
                .Location = New Drawing.Point(.Left * lScale, .Top * lScale)
                DrawIcon(False, lIcon)
            End With
        Next
        DrawTreeBackground()
    End Sub

    'Private Sub pCurrentLegend_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pCurrentLegend.Resize
    '    UpdateLegend()
    'End Sub

    Private Sub ctlSchematic_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        DrawTreeBackground() ' CheckScrollbars()
    End Sub

    Private Sub SplitLegendTree_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        LayoutTree()
    End Sub

    Private Sub SplitLegendTree_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs)
        LayoutTree()
    End Sub

End Class
