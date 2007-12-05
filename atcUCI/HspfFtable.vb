'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports System.Text
Imports MapWinUtility

Public Class HspfFtable
    Private pId As Integer
    Private pNrows As Integer
    Private pNcols As Integer
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
    Private pComment As String

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

    'Public Sub ReadUciFile()
    '  Dim init&, OmCode&, retkey&, cbuff$, retcod&
    '  Dim done As Boolean
    '  Dim i&, j&
    '
    '  OmCode = HspfOmCode("FTABLES")
    '  init = 1
    '  done = False
    '  Do Until done
    '    Call REM_XBLOCK(Me.Operation.Uci, OmCode, init, retkey, cbuff, retcod)
    '    init = 0
    '    If InStr(cbuff, "FTABLE") > 0 Then 'is this the one
    '      If Right(cbuff, 3) = pId Then 'it is
    '        Call REM_XBLOCK(Me.Operation.Uci, OmCode, init, retkey, cbuff, retcod)
    '        Nrows = Left(cbuff, 5)
    '        Ncols = Mid(cbuff, 6, 5)
    '        For i = 1 To pNrows
    '          Call REM_XBLOCK(Me.Operation.Uci, OmCode, init, retkey, cbuff, retcod)
    '          pDepth(i) = Left(cbuff, 10)
    '          pArea(i) = Mid(cbuff, 11, 10)
    '          pVolume(i) = Mid(cbuff, 21, 10)
    '          j = Ncols - 3
    '          If j > 0 Then
    '            pOutflow1(i) = Mid(cbuff, 31, 10)
    '          End If
    '          If j > 1 Then
    '            pOutflow2(i) = Mid(cbuff, 41, 10)
    '          End If
    '          If j > 2 Then
    '            pOutflow3(i) = Mid(cbuff, 51, 10)
    '          End If
    '          If j > 3 Then
    '            pOutflow4(i) = Mid(cbuff, 61, 10)
    '          End If
    '          If j > 4 Then
    '            pOutflow5(i) = Mid(cbuff, 71, 10)
    '          End If
    '        Next i
    '        done = True
    '      End If
    '    End If
    '  Loop
    '
    'End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder
        Dim i, j As Integer
        Dim t, s As String
        Dim lFmt As String = "#0.##;;0\."

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
                    For j = 1 To .Ncols - 3
                        s &= "  outflow" & j
                    Next j
                    lSB.AppendLine(s & " ***")
                End If
                For i = 1 To .Nrows
                    If NumericallyTheSame(.DepthAsRead(i), .Depth(i)) Then
                        s = .DepthAsRead(i)
                    Else
                        s = Format(.Depth(i), lFmt).PadLeft(10)
                    End If
                    If NumericallyTheSame(.AreaAsRead(i), .Area(i)) Then
                        t = .AreaAsRead(i)
                    Else
                        t = Format(.Area(i), lFmt).PadLeft(10)
                    End If
                    s &= t
                    If NumericallyTheSame(.VolumeAsRead(i), .Volume(i)) Then
                        t = .VolumeAsRead(i)
                    Else
                        t = Format(.Volume(i), lFmt).PadLeft(10)
                    End If
                    s &= t
                    For j = 1 To .Ncols - 3
                        If j = 1 Then
                            If NumericallyTheSame(.Outflow1AsRead(i), .Outflow1(i)) Then
                                t = .Outflow1AsRead(i)
                            Else
                                t = Format(.Outflow1(i), lFmt).PadLeft(10)
                                If t.Length > 10 Then
                                    'too many digits in the number
                                    t = RSet(atcUCI.HspfTable.NumFmtRE(CSng(.Outflow1(i)), 10), 10)
                                End If
                            End If
                        End If
                        If j = 2 Then
                            If NumericallyTheSame(.Outflow2AsRead(i), .Outflow2(i)) Then
                                t = .Outflow2AsRead(i)
                            Else
                                t = Format(.Outflow2(i), lFmt).PadLeft(10)
                            End If
                        End If
                        If j = 3 Then
                            If NumericallyTheSame(.Outflow3AsRead(i), .Outflow3(i)) Then
                                t = .Outflow3AsRead(i)
                            Else
                                t = Format(.Outflow3(i), lFmt).PadLeft(10)
                            End If
                        End If
                        If j = 4 Then
                            If NumericallyTheSame(.Outflow4AsRead(i), .Outflow4(i)) Then
                                t = .Outflow4AsRead(i)
                            Else
                                t = Format(.Outflow4(i), lFmt).PadLeft(10)
                            End If
                        End If
                        If j = 5 Then
                            If NumericallyTheSame(.Outflow5AsRead(i), .Outflow5(i)) Then
                                t = .Outflow5AsRead(i)
                            Else
                                t = Format(.Outflow5(i), lFmt).PadLeft(10)
                            End If
                        End If
                        s &= t
                    Next j
                    lSB.AppendLine(s)
                Next i
            End With
            lSB.AppendLine("  END FTABLE" & myFormatI(lOpn.FTable.Id, 3))
        Next lOpn
        lSB.AppendLine("END FTABLES")
        Return lSB.ToString
    End Function

    Public Sub Edit()
        editInit(Me, Me.Operation.OpnBlk.Uci.icon)
    End Sub

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

    Public Sub FTableFromCrossSect(ByRef dL As Single, ByRef dYm As Single, ByRef dWm As Single, ByRef dN As Single, ByRef dS As Single, ByRef dM11 As Single, ByRef dM12 As Single, ByRef dYc As Single, ByRef dM21 As Single, ByRef dM22 As Single, ByRef dYt1 As Single, ByRef dYt2 As Single, ByRef dM31 As Single, ByRef dM32 As Single, ByRef dW11 As Single, ByRef dW12 As Single)

        'algorithm from tt
        Dim Depth(8) As Single
        Dim sfarea(8) As Single
        Dim Volume(8) As Single
        Dim disch(8) As Single
        Dim i As Integer
        Dim CrossSectionArea, NearestBase, HydraulicRadius As Single
        Dim lp, dDepth, dArea, dDenominator, rp As Single
        Dim dWt1, dWb, dWc, dWt2 As Single

        On Error GoTo errorhandler
        'initialize parameters
        If (dYm < dYc) Then
            dWb = dWm - (dYm / dM11) - (dYm / dM12)
        End If
        If (dYm > dYc And dYm < dYt1) Then
            dWb = dWm - dW11 - dW12 - ((dYm - dYc) / dM21) - ((dYm - dYc) / dM22) - (dYc / dM11) - (dYc / dM12)
        End If
        If (dYm > dYt1 And dYm < dYt2) Then
            dWb = dWm - ((dYm - dYt1) / dM31) - ((dYm - dYt1) / dM32) - dW11 - dW12 - ((dYt1 - dYc) / dM21) - ((dYt1 - dYc) / dM22) - (dYc / dM11) - (dYc / dM12)
        End If
        If dWb < 0 Then
            dWb = 0.0001
        End If
        If (dYm > dYt2) Then
            'should not happen
            dYm = -999
        End If
        dWc = dWb + (dYc / dM11) + (dYc / dM12)
        dWt1 = dWc + dW11 + dW12 + ((dYt1 - dYc) / dM21) + ((dYt1 - dYc) / dM22)
        dWt2 = dWt1 + ((dYt2 - dYt1) / dM31) + ((dYt2 - dYt1) / dM32)

        If dYm < 0.0# Or dWm < 0.0# Or dN < 0.0# Or dS < 0.0# Or dM11 < 0.0# Or dM12 < 0.0# Or dM21 < 0.0# Or dM22 < 0.0# Or dM31 < 0.0# Or dM32 < 0.0# Then
            Nrows = 1
            Ncols = 4
            pDepth(1) = 0
            pArea(1) = 0
            pVolume(1) = 0
            pOutflow1(1) = 0
        Else
            'calculate for eight depths
            Depth(1) = 0.0#
            Depth(2) = dYm / 10.0#
            Depth(3) = dYm
            Depth(4) = dYc
            Depth(5) = (dYc + dYt1) / 2.0#
            Depth(6) = dYt1
            Depth(7) = (dYt1 + dYt2) / 2.0#
            Depth(8) = dYt2
            Nrows = 8
            For i = 1 To Nrows
                'get nearest base
                If (Depth(i) > dYt1) Then
                    NearestBase = dWt1
                ElseIf (Depth(i) > dYc) Then
                    NearestBase = dWc + dW11 + dW12
                    'ElseIf (Depth(i) = dYc) Then
                    '  NearestBase = dWc    pbd - should still be bottom channel width
                Else
                    NearestBase = dWb
                End If

                'get cross section area
                If dYc > Depth(i) Then
                    dDepth = Depth(i)
                Else
                    dDepth = dYc
                End If
                lp = LeftPiece(dDepth, dYt1, dM32, dYc, dM22, dM12)
                rp = RightPiece(dDepth, dYt1, dM31, dYc, dM21, dM11)
                CrossSectionArea = dDepth * (dWb + (lp * 0.5) + (rp * 0.5))
                If (Depth(i) > dYc) Then
                    If dYt1 > Depth(i) Then
                        dDepth = Depth(i)
                    Else
                        dDepth = dYt1
                    End If
                    lp = LeftPiece(dDepth, dYt1, dM32, dYc, dM22, dM12)
                    rp = RightPiece(dDepth, dYt1, dM31, dYc, dM21, dM11)
                    CrossSectionArea = CrossSectionArea + (dDepth - dYc) * (dWc + dW11 + dW12 + (lp * 0.5) + (rp * 0.5))
                End If
                If (Depth(i) > dYt1) Then
                    If dYt2 > Depth(i) Then
                        dDepth = Depth(i)
                    Else
                        dDepth = dYt2
                    End If
                    lp = LeftPiece(dDepth, dYt1, dM32, dYc, dM22, dM12)
                    rp = RightPiece(dDepth, dYt1, dM31, dYc, dM21, dM11)
                    CrossSectionArea = CrossSectionArea + (dDepth - dYt1) * (dWt1 + (lp * 0.5) + (rp * 0.5))
                End If

                'get hydraulic radius
                dDenominator = dWb
                If dYc > Depth(i) Then
                    dDepth = Depth(i)
                Else
                    dDepth = dYc
                End If
                dDenominator = dDenominator + dDepth * (System.Math.Sqrt(1.0# + 1.0# / (dM11 * dM11)) + System.Math.Sqrt(1.0# + 1.0# / (dM12 * dM12)))
                If (Depth(i) > dYc) Then
                    If dYt1 > Depth(i) Then
                        dDepth = Depth(i)
                    Else
                        dDepth = dYt1
                    End If
                    dDenominator = dDenominator + dW11 + dW12 + (dDepth - dYc) * (System.Math.Sqrt(1.0# + 1.0# / (dM21 * dM21)) + System.Math.Sqrt(1.0# + 1.0# / (dM22 * dM22)))
                End If
                If (Depth(i) > dYt1) Then
                    If dYt2 > Depth(i) Then
                        dDepth = Depth(i)
                    Else
                        dDepth = dYt2
                    End If
                    dDenominator = dDenominator + (dDepth - dYt1) * (System.Math.Sqrt(1.0# + 1.0# / (dM31 * dM31)) + System.Math.Sqrt(1.0# + 1.0# / (dM32 * dM32)))
                End If
                HydraulicRadius = CrossSectionArea / dDenominator

                lp = LeftPiece(Depth(i), dYt1, dM32, dYc, dM22, dM12)
                rp = RightPiece(Depth(i), dYt1, dM31, dYc, dM21, dM11)

                sfarea(i) = dL * (NearestBase + lp + rp) / 43560.0#
                Volume(i) = dL * CrossSectionArea / 43560.0#
                disch(i) = 1.49 / dN * (HydraulicRadius ^ (2 / 3)) * System.Math.Sqrt(dS) * CrossSectionArea
            Next i

            'build ftable
            Nrows = 8
            Ncols = 4
            For i = 1 To Nrows
                pDepth(i) = Depth(i)
                pArea(i) = sfarea(i)
                pVolume(i) = Volume(i)
                pOutflow1(i) = disch(i)
            Next i
        End If
        On Error Resume Next
        Exit Sub
errorhandler:
        'TODO: remove MsgBox
        logger.Msg("An error occurred while building FTable" & Me.Id, "FTable Create From XSect Problem")
        On Error Resume Next
    End Sub

    Private Function LeftPiece(ByRef Depth As Single, ByRef dYt1 As Single, ByRef dM32 As Single, ByRef dYc As Single, ByRef dM22 As Single, ByRef dM12 As Single) As Single
        'get left piece
        If (Depth > dYt1) Then
            LeftPiece = (Depth - dYt1) / dM32
        ElseIf (Depth > dYc) Then
            LeftPiece = (Depth - dYc) / dM22
        Else
            LeftPiece = Depth / dM12
        End If
    End Function

    Private Function RightPiece(ByRef Depth As Single, ByRef dYt1 As Single, ByRef dM31 As Single, ByRef dYc As Single, ByRef dM21 As Single, ByRef dM11 As Single) As Single
        'get right piece
        If (Depth > dYt1) Then
            RightPiece = (Depth - dYt1) / dM31
        ElseIf (Depth > dYc) Then
            RightPiece = (Depth - dYc) / dM21
        Else
            RightPiece = Depth / dM11
        End If
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

    Private Function NumericallyTheSame(ByRef ValueAsRead As String, ByRef ValueStored As Single) As Boolean
        'see if the current ftable value is the same as the value as read from the uci
        '4. is the same as 4.0
        Dim rtemp1 As Single

        NumericallyTheSame = False
        If IsNumeric(ValueStored) Then
            If IsNumeric(ValueAsRead) Then
                'simple case
                rtemp1 = CSng(ValueAsRead)
                If rtemp1 = ValueStored Then
                    NumericallyTheSame = True
                End If
            End If
        End If
    End Function
End Class