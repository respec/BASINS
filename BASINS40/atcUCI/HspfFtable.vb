'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text
Imports MapWinUtility
Imports atcSegmentation

Public Class HspfFtable
    Private pId As Integer
    Private pNRows As Integer
    Private pNCols As Integer
    'TODO: make this a class!
    Private pDepth() As Double
    Private pArea() As Double
    Private pVolume() As Double
    Private pOutflow1() As Double 'redim preserve does not work
    Private pOutflow2() As Double 'with multiple subscripts
    Private pOutflow3() As Double
    Private pOutflow4() As Double
    Private pOutflow5() As Double
    Private pDepthAsRead() As String
    Private pAreaAsRead() As String
    Private pVolumeAsRead() As String
    Private pOutflow1AsRead() As String
    Private pOutflow2AsRead() As String
    Private pOutflow3AsRead() As String
    Private pOutflow4AsRead() As String
    Private pOutflow5AsRead() As String
    Private pOperation As HspfOperation
    Private pComment As String = ""

    Public Property Depth(ByVal aRow As Integer) As Double
        Get
            Return pDepth(aRow)
        End Get
        Set(ByVal Value As Double)
            pDepth(aRow) = Value
        End Set
    End Property

    Public Property Area(ByVal row As Integer) As Double
        Get
            Area = pArea(row)
        End Get
        Set(ByVal Value As Double)
            pArea(row) = Value
        End Set
    End Property

    Public Property Volume(ByVal aRow As Integer) As Double
        Get
            Return pVolume(aRow)
        End Get
        Set(ByVal Value As Double)
            pVolume(aRow) = Value
        End Set
    End Property

    Public Property Outflow1(ByVal aRow As Integer) As Double
        Get
            Return pOutflow1(aRow)
        End Get
        Set(ByVal Value As Double)
            pOutflow1(aRow) = Value
        End Set
    End Property

    Public Property Outflow2(ByVal aRow As Integer) As Double
        Get
            Return pOutflow2(aRow)
        End Get
        Set(ByVal Value As Double)
            pOutflow2(aRow) = Value
        End Set
    End Property

    Public Property Outflow3(ByVal aRow As Integer) As Double
        Get
            Return pOutflow3(aRow)
        End Get
        Set(ByVal Value As Double)
            pOutflow3(aRow) = Value
        End Set
    End Property

    Public Property Outflow4(ByVal aRow As Integer) As Double
        Get
            Return pOutflow4(aRow)
        End Get
        Set(ByVal Value As Double)
            pOutflow4(aRow) = Value
        End Set
    End Property

    Public Property Outflow5(ByVal aRow As Integer) As Double
        Get
            Return pOutflow5(aRow)
        End Get
        Set(ByVal Value As Double)
            pOutflow5(aRow) = Value
        End Set
    End Property

    Public Property DepthAsRead(ByVal aRow As Integer) As String
        Get
            Return pDepthAsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pDepthAsRead(aRow) = Value
        End Set
    End Property

    Public Property AreaAsRead(ByVal aRow As Integer) As String
        Get
            Return pAreaAsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pAreaAsRead(aRow) = Value
        End Set
    End Property

    Public Property VolumeAsRead(ByVal aRow As Integer) As String
        Get
            Return pVolumeAsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pVolumeAsRead(aRow) = Value
        End Set
    End Property

    Public Property Outflow1AsRead(ByVal aRow As Integer) As String
        Get
            Return pOutflow1AsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pOutflow1AsRead(aRow) = Value
        End Set
    End Property

    Public Property Outflow2AsRead(ByVal aRow As Integer) As String
        Get
            Return pOutflow2AsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pOutflow2AsRead(aRow) = Value
        End Set
    End Property

    Public Property Outflow3AsRead(ByVal aRow As Integer) As String
        Get
            Return pOutflow3AsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pOutflow3AsRead(aRow) = Value
        End Set
    End Property

    Public Property Outflow4AsRead(ByVal aRow As Integer) As String
        Get
            Return pOutflow4AsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pOutflow4AsRead(aRow) = Value
        End Set
    End Property

    Public Property Outflow5AsRead(ByVal aRow As Integer) As String
        Get
            Return pOutflow5AsRead(aRow)
        End Get
        Set(ByVal Value As String)
            pOutflow5AsRead(aRow) = Value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return pComment
        End Get
        Set(ByVal Value As String)
            pComment = Value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Return pId
        End Get
        Set(ByVal Value As Integer)
            pId = Value
        End Set
    End Property

    Public Property Nrows() As Integer
        Get
            Return pNrows
        End Get
        Set(ByVal Value As Integer)
            pNrows = Value
            Call initArrays()
        End Set
    End Property

    Public Property Ncols() As Integer
        Get
            Return pNcols
        End Get
        Set(ByVal Value As Integer)
            pNcols = Value
            Call initArrays()
        End Set
    End Property
    Public Property Operation() As HspfOperation
        Get
            Return pOperation
        End Get
        Set(ByVal Value As HspfOperation)
            pOperation = Value
        End Set
    End Property

    Public ReadOnly Property EditControlName() As String
        Get
            Return "ATCoHspf.ctlFTableEdit"
        End Get
    End Property

    Public ReadOnly Property Caption() As String
        Get
            Return "Ftable"
        End Get
    End Property

    Public Sub Edited()
        pOperation.Edited = True
    End Sub

    Private Sub initArrays()
        If pNrows > 0 And pNcols > 0 Then 'ok to do this
            ReDim Preserve pDepth(pNrows)
            ReDim Preserve pArea(pNrows)
            ReDim Preserve pVolume(pNrows)
            ReDim Preserve pOutflow1(pNrows)
            ReDim Preserve pOutflow2(pNrows)
            ReDim Preserve pOutflow3(pNrows)
            ReDim Preserve pOutflow4(pNrows)
            ReDim Preserve pOutflow5(pNrows)
            ReDim Preserve pDepthAsRead(pNrows)
            ReDim Preserve pAreaAsRead(pNrows)
            ReDim Preserve pVolumeAsRead(pNrows)
            ReDim Preserve pOutflow1AsRead(pNrows)
            ReDim Preserve pOutflow2AsRead(pNrows)
            ReDim Preserve pOutflow3AsRead(pNrows)
            ReDim Preserve pOutflow4AsRead(pNrows)
            ReDim Preserve pOutflow5AsRead(pNrows)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        Dim t, s As String
        Dim lFmt As String = "#0.0#"

        lSB.AppendLine("FTABLES")
        For Each lOpn As HspfOperation In pOperation.OpnBlk.Ids
            lSB.AppendLine(" ")
            lSB.AppendLine("  FTABLE    " & myFormatI(lOpn.FTable.Id, 3))
            With lOpn.FTable
                lSB.AppendLine(" rows cols" & Space((.Ncols - 1) * 10) & " ***")
                lSB.AppendLine(myFormatI(lOpn.FTable.Nrows, 5) & myFormatI(lOpn.FTable.Ncols, 5))
                If Not (.Comment Is Nothing) AndAlso .Comment.Length > 0 Then
                    lSB.AppendLine(.Comment)
                Else
                    s = "     depth      area    volume"
                    For lColumn As Integer = 1 To .Ncols - 3
                        s &= "  outflow" & lColumn
                    Next lColumn
                    lSB.AppendLine(s & " ***")
                End If
                For lRow As Integer = 1 To .Nrows
                    If NumericallyTheSame(.DepthAsRead(lRow), .Depth(lRow)) Then
                        s = .DepthAsRead(lRow)
                    Else
                        s = Format(.Depth(lRow), lFmt).PadLeft(10)
                    End If
                    If NumericallyTheSame(.AreaAsRead(lRow), .Area(lRow)) Then
                        t = .AreaAsRead(lRow)
                    Else
                        t = Format(.Area(lRow), lFmt).PadLeft(10)
                    End If
                    s &= t
                    If NumericallyTheSame(.VolumeAsRead(lRow), .Volume(lRow)) Then
                        t = .VolumeAsRead(lRow)
                    Else
                        t = Format(.Volume(lRow), lFmt).PadLeft(10)
                    End If
                    s &= t
                    For lOutflowIndex As Integer = 1 To .Ncols - 3
                        If lOutflowIndex = 1 Then
                            If NumericallyTheSame(.Outflow1AsRead(lRow), .Outflow1(lRow)) Then
                                t = .Outflow1AsRead(lRow)
                            Else
                                t = Format(.Outflow1(lRow), lFmt).PadLeft(10)
                                If t.Length > 10 Then 'too many digits in the number
                                    t = atcUCI.HspfTable.NumFmtRE(CSng(.Outflow1(lRow)), 10).PadLeft(10)
                                End If
                            End If
                        ElseIf lOutflowIndex = 2 Then
                            If NumericallyTheSame(.Outflow2AsRead(lRow), .Outflow2(lRow)) Then
                                t = .Outflow2AsRead(lRow)
                            Else
                                t = Format(.Outflow2(lRow), lFmt).PadLeft(10)
                            End If
                        ElseIf lOutflowIndex = 3 Then
                            If NumericallyTheSame(.Outflow3AsRead(lRow), .Outflow3(lRow)) Then
                                t = .Outflow3AsRead(lRow)
                            Else
                                t = Format(.Outflow3(lRow), lFmt).PadLeft(10)
                            End If
                        ElseIf lOutflowIndex = 4 Then
                            If NumericallyTheSame(.Outflow4AsRead(lRow), .Outflow4(lRow)) Then
                                t = .Outflow4AsRead(lRow)
                            Else
                                t = Format(.Outflow4(lRow), lFmt).PadLeft(10)
                            End If
                        ElseIf lOutflowIndex = 5 Then
                            If NumericallyTheSame(.Outflow5AsRead(lRow), .Outflow5(lRow)) Then
                                t = .Outflow5AsRead(lRow)
                            Else
                                t = Format(.Outflow5(lRow), lFmt).PadLeft(10)
                            End If
                        End If
                        s &= t
                    Next lOutflowIndex
                    lSB.AppendLine(s)
                Next lRow
            End With
            lSB.AppendLine("  END FTABLE" & myFormatI(lOpn.FTable.Id, 3))
        Next lOpn
        lSB.AppendLine("END FTABLES")
        Return lSB.ToString
    End Function

    Private Function NumericallyTheSame(ByVal aValueAsRead As String, ByVal aValueStored As Single) As Boolean
        'see if the current ftable value is the same as the value as read from the uci
        '4. is the same as 4.0
        Dim lTemp As Single
        Dim lNumericallyTheSame As Boolean = False
        If IsNumeric(aValueStored) AndAlso IsNumeric(aValueAsRead) Then
            'simple case
            lTemp = CSng(aValueAsRead)
            If lTemp = aValueStored Then
                lNumericallyTheSame = True
            End If
        End If
        Return lNumericallyTheSame
    End Function

    Public Sub New()
        MyBase.New()
        Nrows = 1
        Ncols = 4
        pDepth(1) = 0
        pArea(1) = 0
        pVolume(1) = 0
        pOutflow1(1) = 0
        pOutflow2(1) = 0
        pOutflow3(1) = 0
        pOutflow4(1) = 0
        pOutflow5(1) = 0
    End Sub

    'Public Sub FTableFromCrossSect(length!, elup!, eldown!, w1!, w2!, h!, sfp!, nch!, nfp!)
    '
    '  'from xsect, replaced by algorithm from tt
    '  'LENGTH - reach length (miles)
    '  'ELUP   - upstream elevation (ft)
    '  'ELDOWN - downstream elevation (ft)
    '  'W1     - channel bottom width (ft)
    '  'W2     - channel bankfull width (ft)
    '  'H      - channel height (ft)
    '  'SFP    - slope of flood plain (-)
    '  'NCH    - mannings n for the channel
    '  'NFP    - mannings n for the flood plain
    '
    '  Dim i1&, i2&, i3&, i&
    '  Dim slope!, theta1!, wp1!
    '  Dim theta2!, wp2!, inc1!, inc2!, inc3!, tw!, area!, wetp!, hydrad!
    '  Dim areain!, wetpin!
    '  Dim depth!(15), sfarea!(15), volume!(15), disch!(15), flotim!(15)
    '
    '  'INTRINSIC   ABS,ATAN,COS,SIN
    '
    '  slope = Abs(elup - eldown) / (length * 5280#)
    '  theta1 = Atn((w2 - w1) / (2# * h))
    '  wp1 = Cos(theta1)
    '  theta2 = Atn(sfp)
    '  wp2 = Sin(theta2)
    '
    '  inc1 = h / 12#
    '
    '  depth(1) = 0#
    '  sfarea(1) = 0#
    '  volume(1) = 0#
    '  disch(1) = 0#
    '  flotim(1) = 0#
    '
    '  'main channel computations
    '  For i1 = 2 To 7
    '    depth(i1) = (i1 - 1) * inc1
    '    tw = w1 + ((w2 - w1) / h) * depth(i1)
    '    sfarea(i1) = tw * length * 5280# / 43560#
    '    area = ((tw + w1) / 2) * depth(i1)
    '    volume(i1) = area * length * 5280# / 43560#
    '    wetp = w1 + 2 * (depth(i1) / wp1)
    '    hydrad = area / wetp
    '    disch(i1) = 1.49 * area * (hydrad ^ 0.667) * (slope ^ 0.5) / nch
    '    flotim(i1) = (volume(i1) * 43560#) / (disch(i1) * 60#)
    '  Next i1
    '
    '  inc2 = 2# * inc1
    '  For i2 = 8 To 10
    '    depth(i2) = 6 * inc1 + (i2 - 7) * inc2
    '    tw = w1 + ((w2 - w1) / h) * depth(i2)
    '    sfarea(i2) = tw * length * 5280# / 43560#
    '    area = ((tw + w1) / 2) * depth(i2)
    '    volume(i2) = area * length * 5280# / 43560#
    '    wetp = w1 + 2 * (depth(i2) / wp1)
    '    hydrad = area / wetp
    '    disch(i2) = 1.49 * area * (hydrad ^ 0.667) * (slope ^ 0.5) / nch
    '    flotim(i2) = (volume(i2) * 43560#) / (disch(i2) * 60#)
    '  Next i2
    '
    '  'overbank computations
    '  areain = ((w1 + w2) / 2) * h
    '  wetpin = w1 + 2 * (h / wp1)
    '  'inc3 = 6# * inc2  '(CHANGED 2 TO 6 1/2/90 FOR CHES BAY WORK)
    '  inc3 = 2# * inc2
    '  For i3 = 11 To 15
    '    depth(i3) = 6 * inc1 + 3 * inc2 + (i3 - 10) * inc3
    '    tw = w2 + 2 * (depth(i3) - h) / sfp
    '    sfarea(i3) = tw * length * 5280# / 43560#
    '    'incised channel
    '    area = areain + w2 * (depth(i3) - h)
    '    volume(i3) = area * length * 5280# / 43560#
    '    hydrad = area / wetpin
    '    disch(i3) = 1.49 * area * (hydrad ^ 0.667) * (slope ^ 0.5) / nch
    '    'overbank
    '    area = (depth(i3) - h) * (depth(i3) - h) / sfp
    '    volume(i3) = volume(i3) + area * length * 5280# / 43560#
    '    wetp = 2 * (depth(i3) - h) / wp2
    '    hydrad = area / wetp
    '    disch(i3) = disch(i3) + 1.49 * area * (hydrad ^ 0.667) * (slope ^ 0.5) / nfp
    '    flotim(i3) = (volume(i3) * 43560#) / (disch(i3) * 60#)
    '  Next i3
    '
    '  Nrows = 15
    '  Ncols = 4
    '  For i = 1 To Nrows
    '    pDepth(i) = depth(i)
    '    pArea(i) = sfarea(i)
    '    pVolume(i) = volume(i)
    '    pOutflow(i, 1) = disch(i)
    '  Next i
    '
    'End Sub

    Public Sub FTableFromCrossSect(ByVal aChannel As Channel)
        'algorithm from tt
        Dim lDepth(8) As Single
        Dim lSurfaceArea(8) As Single
        Dim lVolume(8) As Single
        Dim lDischarge(8) As Single

        Try
            With aChannel
                'initialize parameters
                Dim dWb As Single
                If (.DepthMean < .DepthChannel) Then
                    dWb = .WidthMean - (.DepthMean / .SlopeSideLeft) _
                                     - (.DepthMean / .SlopeSideRight)
                End If
                If (.DepthMean > .DepthChannel And .DepthMean < .DepthSlopeChange) Then
                    dWb = .WidthMean - .WidthZeroSlopeLeft - .WidthZeroSlopeRight - ((.DepthMean - .DepthChannel) / .SlopeSideLowerFPLeft) - ((.DepthMean - .DepthChannel) / .SlopeSideLowerFPRight) - (.DepthChannel / .SlopeSideLeft) - (.DepthChannel / .SlopeSideRight)
                End If
                If (.DepthMean > .DepthSlopeChange And .DepthMean < .DepthMax) Then
                    dWb = .WidthMean - ((.DepthMean - .DepthSlopeChange) / .SlopeSideUpperFPLeft) - ((.DepthMean - .DepthSlopeChange) / .SlopeSideUpperFPRight) - .WidthZeroSlopeLeft - .WidthZeroSlopeRight - ((.DepthSlopeChange - .DepthChannel) / .SlopeSideLowerFPLeft) - ((.DepthSlopeChange - .DepthChannel) / .SlopeSideLowerFPRight) - (.DepthChannel / .SlopeSideLeft) - (.DepthChannel / .SlopeSideRight)
                End If
                If dWb < 0 Then
                    dWb = 0.0001
                End If
                If (.DepthMean > .DepthMax) Then
                    'should not happen
                    .DepthMean = -999
                End If
                Dim dWt1, dWc, dWt2 As Single
                dWc = dWb + (.DepthChannel / .SlopeSideLeft) + (.DepthChannel / .SlopeSideRight)
                dWt1 = dWc + .WidthZeroSlopeLeft + .WidthZeroSlopeRight + ((.DepthSlopeChange - .DepthChannel) / .SlopeSideLowerFPLeft) + ((.DepthSlopeChange - .DepthChannel) / .SlopeSideLowerFPRight)
                dWt2 = dWt1 + ((.DepthMax - .DepthSlopeChange) / .SlopeSideUpperFPLeft) + ((.DepthMax - .DepthSlopeChange) / .SlopeSideUpperFPRight)

                If .DepthMean < 0.0# Or .WidthMean < 0.0# Or _
                   .ManningN < 0.0# Or .SlopeProfile < 0.0# Or _
                   .SlopeSideLeft < 0.0# Or .SlopeSideRight < 0.0# Or _
                   .SlopeSideLowerFPLeft < 0.0# Or .SlopeSideLowerFPRight < 0.0# Or _
                   .SlopeSideUpperFPLeft < 0.0# Or .SlopeSideUpperFPRight < 0.0# Then
                    Nrows = 1
                    Ncols = 4
                    pDepth(1) = 0
                    pArea(1) = 0
                    pVolume(1) = 0
                    pOutflow1(1) = 0
                Else
                    'calculate for eight depths
                    lDepth(1) = 0.0#
                    lDepth(2) = .DepthMean / 10.0#
                    lDepth(3) = .DepthMean
                    lDepth(4) = .DepthChannel
                    lDepth(5) = (.DepthChannel + .DepthSlopeChange) / 2.0#
                    lDepth(6) = .DepthSlopeChange
                    lDepth(7) = (.DepthSlopeChange + .DepthMax) / 2.0#
                    lDepth(8) = .DepthMax
                    Nrows = 8
                    For i As Integer = 1 To Nrows
                        'get nearest base
                        Dim lNearestBase As Single
                        If (lDepth(i) > .DepthSlopeChange) Then
                            lNearestBase = dWt1
                        ElseIf (lDepth(i) > .DepthChannel) Then
                            lNearestBase = dWc + .WidthZeroSlopeLeft + .WidthZeroSlopeRight
                            'ElseIf (lDepth(i) = .DepthChannel) Then
                            '  NearestBase = dWc    pbd - should still be bottom channel width
                        Else
                            lNearestBase = dWb
                        End If

                        'get cross section area
                        Dim lDepthD As Single
                        If .DepthChannel > lDepth(i) Then
                            lDepthD = lDepth(i)
                        Else
                            lDepthD = .DepthChannel
                        End If
                        Dim lLeftPiece As Single = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPRight, .DepthChannel, .SlopeSideLowerFPRight, .SlopeSideRight)
                        Dim lRightPiece As Single = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPLeft, .DepthChannel, .SlopeSideLowerFPLeft, .SlopeSideLeft)
                        Dim lCrossSectionArea As Single = lDepthD * (dWb + (lLeftPiece * 0.5) + (lRightPiece * 0.5))
                        If (lDepth(i) > .DepthChannel) Then
                            If .DepthSlopeChange > lDepth(i) Then
                                lDepthD = lDepth(i)
                            Else
                                lDepthD = .DepthSlopeChange
                            End If
                            lLeftPiece = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPRight, .DepthChannel, .SlopeSideLowerFPRight, .SlopeSideRight)
                            lRightPiece = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPLeft, .DepthChannel, .SlopeSideLowerFPLeft, .SlopeSideLeft)
                            lCrossSectionArea += (lDepthD - .DepthChannel) * (dWc + .WidthZeroSlopeLeft + .WidthZeroSlopeRight + (lLeftPiece * 0.5) + (lRightPiece * 0.5))
                        End If
                        If (lDepth(i) > .DepthSlopeChange) Then
                            If .DepthMax > lDepth(i) Then
                                lDepthD = lDepth(i)
                            Else
                                lDepthD = .DepthMax
                            End If
                            lLeftPiece = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPRight, .DepthChannel, .SlopeSideLowerFPRight, .SlopeSideRight)
                            lRightPiece = SidePiece(lDepthD, .DepthSlopeChange, .SlopeSideUpperFPLeft, .DepthChannel, .SlopeSideLowerFPLeft, .SlopeSideLeft)
                            lCrossSectionArea += (lDepthD - .DepthSlopeChange) * (dWt1 + (lLeftPiece * 0.5) + (lRightPiece * 0.5))
                        End If

                        'get hydraulic radius
                        Dim lDenominator As Single = dWb
                        If .DepthChannel > lDepth(i) Then
                            lDepthD = lDepth(i)
                        Else
                            lDepthD = .DepthChannel
                        End If
                        lDenominator += lDepthD * (System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideLeft * .SlopeSideLeft)) + System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideRight * .SlopeSideRight)))
                        If (lDepth(i) > .DepthChannel) Then
                            If .DepthSlopeChange > lDepth(i) Then
                                lDepthD = lDepth(i)
                            Else
                                lDepthD = .DepthSlopeChange
                            End If
                            lDenominator += .WidthZeroSlopeLeft + .WidthZeroSlopeRight + (lDepthD - .DepthChannel) * (System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideLowerFPLeft * .SlopeSideLowerFPLeft)) + System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideLowerFPRight * .SlopeSideLowerFPRight)))
                        End If
                        If (lDepth(i) > .DepthSlopeChange) Then
                            If .DepthMax > lDepth(i) Then
                                lDepthD = lDepth(i)
                            Else
                                lDepthD = .DepthMax
                            End If
                            lDenominator += (lDepthD - .DepthSlopeChange) * (System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideUpperFPLeft * .SlopeSideUpperFPLeft)) + System.Math.Sqrt(1.0# + 1.0# / (.SlopeSideUpperFPRight * .SlopeSideUpperFPRight)))
                        End If
                        Dim lHydraulicRadius As Single = lCrossSectionArea / lDenominator

                        lLeftPiece = SidePiece(lDepth(i), .DepthSlopeChange, .SlopeSideUpperFPRight, .DepthChannel, .SlopeSideLowerFPRight, .SlopeSideRight)
                        lRightPiece = SidePiece(lDepth(i), .DepthSlopeChange, .SlopeSideUpperFPLeft, .DepthChannel, .SlopeSideLowerFPLeft, .SlopeSideLeft)

                        lSurfaceArea(i) = .Length * (lNearestBase + lLeftPiece + lRightPiece) / 43560.0#
                        lVolume(i) = .Length * lCrossSectionArea / 43560.0#
                        lDischarge(i) = 1.49 / .ManningN * (lHydraulicRadius ^ (2 / 3)) * System.Math.Sqrt(.SlopeProfile) * lCrossSectionArea
                    Next i

                    'build ftable
                    Nrows = 8
                    Ncols = 4
                    For i As Integer = 1 To Nrows
                        pDepth(i) = lDepth(i)
                        pArea(i) = lSurfaceArea(i)
                        pVolume(i) = lVolume(i)
                        pOutflow1(i) = lDischarge(i)
                    Next i
                End If
            End With
        Catch e As ApplicationException
            Logger.Msg("Error occurred while building FTable" & Me.Id, "FTable Create From XSect Problem")
        End Try
    End Sub

    Private Function SidePiece(ByRef aDepth As Single, _
                               ByRef aDepthSlopeChange As Single, _
                               ByRef aSideSlopeUpperFP As Single, _
                               ByRef aDepthChannel As Single, _
                               ByRef aSideSlopeLowerFP As Single, _
                               ByRef aSlopeSide As Single) As Single
        Dim lSidePiece As Single
        If (aDepth > aDepthSlopeChange) Then
            lSidePiece = (aDepth - aDepthSlopeChange) / aSideSlopeUpperFP
        ElseIf (aDepth > aDepthChannel) Then
            lSidePiece = (aDepth - aDepthChannel) / aSideSlopeLowerFP
        Else
            lSidePiece = aDepth / aSlopeSide
        End If
        Return lSidePiece
    End Function


    'bool CPTFData:: DataValid (void)
    '{
    '    double dAcRc;       // CrossSectionArea (Yc)                * HydraulicRadius (Yc)
    '    double dAct1Rct1;   // CrossSectionArea ((Yc + Yt1) / 2)    * HydraulicRadius ((Yc + Yt1) / 2)
    '    double dAt1Rt1;     // CrossSectionArea (Yt1)               * HydraulicRadius (Yt1)
    '    double dAt1t2Rt1t2; // CrossSectionArea ((Yt1 + Yt2) / 2)   * HydraulicRadius ((Yt1 + Yt2) / 2)
    '
    '
    '
    '    dAcRc       = GetCrossSectionArea (m_dYc)                   * pow (GetHydraulicRadius (m_dYc),                      2.0 / 3.0);
    '    dAct1Rct1   = GetCrossSectionArea ((m_dYc + m_dYt1)  / 2.0) * pow (GetHydraulicRadius ((m_dYc + m_dYt1)  / 2.0),    2.0 / 3.0);
    '    dAt1Rt1     = GetCrossSectionArea (m_dYt1)                  * pow (GetHydraulicRadius (m_dYt1),                     2.0 / 3.0);
    '    dAt1t2Rt1t2 = GetCrossSectionArea ((m_dYt1 + m_dYt2) / 2.0) * pow (GetHydraulicRadius ((m_dYt1 + m_dYt2)  / 2.0),   2.0 / 3.0);
    '
    '    return ((((1.1 * dAcRc) - dAct1Rct1) < 0.0) && (((1.1 * dAt1Rt1) - dAt1t2Rt1t2) < 0.0));
    '}
End Class