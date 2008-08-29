Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Collections.ObjectModel


Public Class frmNewFTable

    Friend Sub Init(ByVal aCtl As ctlEditFTables)
        Me.Icon = aCtl.ParentForm.Icon
    End Sub

    '    Dim lFtab As HspfFtable
    '    Dim lo As Object 'the control containing the ftable grid

    '    Public Sub SetCurrentFTable(ByVal ftab As HspfFtable, ByVal O As Object)
    '        lFtab = ftab
    '        lo = O
    '    End Sub

    '    Private Sub btnEstimateAll_Click()
    '        Dim Rtn As Double
    '        Dim channelN As Double

    '        Rtn = getMeanAnnualFlow_FPS(Combo1.ListIndex, atxDrainageArea.Value)
    '        atxChannelWidth.Value = SignificantDigits(getMeanChannelWidth_Ft(Combo1.ListIndex, Rtn), 5)
    '        atxChannelDepth.Value = SignificantDigits(getMeanChannelDepth_Ft(Combo1.ListIndex, Rtn), 5)
    '        channelN = SignificantDigits(getChannelManningsN(Combo1.ListIndex, atxDrainageArea.Value, atxChannelSlope.Value), 5)
    '        atxChannelManningsN.Value = CStr(channelN)
    '        channelN = SignificantDigits(getFloodplainManningsN(Combo1.ListIndex, atxDrainageArea.Value, atxChannelSlope.Value), 5)
    '        atxFloodplainManningsN.Value = CStr(channelN)
    '        atxBankfullDepth.Value = SignificantDigits(getBankfullDepth_ft(val(atxChannelDepth.Value)), 5)
    '        atxMaximumFloodplainDepth.Value = SignificantDigits(getMaximumFloodplainDepth_ft(val(atxChannelDepth.Value)), 5)
    '        atxLeftSideFloodPlainWidth.Value = SignificantDigits(getMeanChannelWidth_Ft(Combo1.ListIndex, Rtn), 5)
    '        atxRightSideFloodPlainWidth.Value = SignificantDigits(getMeanChannelWidth_Ft(Combo1.ListIndex, Rtn), 5)
    '        atxChannelSideSlope.Value = SignificantDigits(getChannelSideSlope(), 5)
    '        atxFloodplainSideSlope.Value = SignificantDigits(getFloodplainSideSlope(), 5)
    '    End Sub

    '    Private Sub btnFTable_Click()
    '        Dim ft() As Double
    '        Dim Msg As String

    '        getFTableForNaturalTrapezoidalChannel(CDbl(atxChannelLength.Value), _
    '                                                    val(atxChannelSlope.Value), _
    '                                                    val(atxChannelWidth.Value), _
    '                                                    val(atxChannelDepth.Value), _
    '                                                    val(atxChannelManningsN.Value), _
    '                                                    val(atxChannelSideSlope.Value), _
    '                                                    val(atxBankfullDepth.Value), _
    '                                                    val(atxFloodplainManningsN.Value), _
    '                                                    val(atxFloodplainSideSlope.Value), _
    '                                                    val(atxLeftSideFloodPlainWidth.Value), _
    '                                                    val(atxRightSideFloodPlainWidth.Value), _
    '                                                    val(atxMaximumFloodplainDepth.Value), _
    '                                                    Msg, ft())
    '        If (Msg <> "") Then
    '            MsgBox(Msg, vbOKOnly, "Compute New FTABLE Problem")
    '            Exit Sub
    '        End If

    '        Dim rows As Integer
    '        rows = UBound(ft)

    '        'save to new hspf ftable
    '        Dim i As Integer
    '        Dim j As Long
    '        Dim fmt As String
    '        fmt = "0.##"
    '        lFtab.Nrows = rows + 1
    '        lFtab.Ncols = 4
    '        lFtab.Depth(0) = 0
    '        lFtab.Area(0) = 0
    '        lFtab.Volume(0) = 0
    '        lFtab.Outflow1(0) = 0
    '        For i = 0 To rows
    '            j = i + 1
    '            lFtab.Depth(j) = Format(ft(i, 0), fmt)
    '            lFtab.Area(j) = Format(ft(i, 1), fmt)
    '            lFtab.Volume(j) = Format(ft(i, 2), fmt)
    '            lFtab.Outflow1(j) = Format(ft(i, 3), fmt)
    '        Next i
    '        lo.UpdateFTABLE(lFtab)

    '    End Sub

    '    Private Sub Combo1_Click()
    '        If Combo1.ListIndex > 0 Then
    '            btnEstimateAll.Enabled = True
    '        Else
    '            btnEstimateAll.Enabled = False
    '        End If
    '    End Sub

    '    Private Sub Form_Load()
    '        Me.Caption = Me.Caption & lFtab.Id

    '        atxChannelLength.Value = lFtab.Operation.Tables("HYDR-PARM2").Parms("LEN") * 5280.0#
    '        atxChannelSlope.Value = SignificantDigits(lFtab.Operation.Tables("HYDR-PARM2").Parms("DELTH") / (lFtab.Operation.Tables("HYDR-PARM2").Parms("LEN") * 5280.0#), 3)
    '        atxDrainageArea.Value = Format(lFtab.Operation.Uci.UpstreamArea(lFtab.Id) / 640.0#, "0.##")

    '        Combo1.Clear()
    '        Combo1.AddItem("Select Physiographic Province")
    '        Combo1.AddItem("Appalachian Plateau")
    '        Combo1.AddItem("Blue Ridge And Ridge And Valley")
    '        Combo1.AddItem("Piedmont")
    '        Combo1.ListIndex = 0
    '        btnEstimateAll.Enabled = False
    '    End Sub

    '    Public Function validateDrainageArea(ByVal drainageArea_SqMiles As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateDrainageArea = True
    '        If (drainageArea_SqMiles < 0.00001) Then
    '            Msg = "Drainage Area must be greater greater than or equal to 0.00001"
    '            validateDrainageArea = False
    '        End If
    '    End Function

    '    Public Function validateChannelLength(ByVal channelLength_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelLength = True
    '        If (channelLength_ft < 0.00001) Then
    '            Msg = "Channel Length must be greater than or equal to 0.00001"
    '            validateChannelLength = False
    '        End If
    '    End Function

    '    Public Function validateChannelSlope(ByVal channelSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelSlope = True
    '        If (channelSlope_ftPerFt < 0.00001) Then
    '            Msg = "Channel Slope must be greater than or equal to 0.00001"
    '            validateChannelSlope = False
    '        End If
    '    End Function

    '    Public Function validateChannelWidth(ByVal channelWidth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelWidth = True
    '        If (channelWidth_ft < 0.00001) Then
    '            Msg = "Channel Width must be greater than or equal to 0.00001"
    '            validateChannelWidth = False
    '        End If
    '    End Function

    '    Public Function validateChannelDepth(ByVal channelDepth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelDepth = True
    '        If (channelDepth_ft < 0.00001) Then
    '            Msg = "Channel Depth must be greater than or equal to 0.00001"
    '            validateChannelDepth = False
    '        End If
    '    End Function

    '    Public Function validateChannelManningsN(ByVal channelManningsN As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelManningsN = True
    '        If (channelManningsN < 0.00001) Then
    '            Msg = "Channel Manning's N must be greater than or equal to 0.00001"
    '            validateChannelManningsN = False
    '        End If
    '    End Function

    '    Public Function validateFloodplainManningsN(ByVal floodplainManningsN As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateFloodplainManningsN = True
    '        If (floodplainManningsN < 0.00001) Then
    '            Msg = "Floodplain Manning's N must be greater than or equal to 0.00001"
    '            validateFloodplainManningsN = False
    '        End If
    '    End Function

    '    Public Function validateChannelSideSlope(ByVal channelSideSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateChannelSideSlope = True
    '        If (channelSideSlope_ftPerFt < 0.00001) Then
    '            Msg = "Channel Side Slope must be greater than or equal to 0.00001"
    '            validateChannelSideSlope = False
    '        End If
    '    End Function

    '    Public Function validateFloodplainSideSlope(ByVal floodplainSideSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateFloodplainSideSlope = True
    '        If (floodplainSideSlope_ftPerFt < 0.00001) Then
    '            Msg = "Floodplain Side Slope must be greater than or equal to 0.00001"
    '            validateFloodplainSideSlope = False
    '        End If
    '    End Function

    '    Public Function validateBankfullDepth(ByVal bankfullDepth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateBankfullDepth = True
    '        If (bankfullDepth_ft < 0.00001) Then
    '            Msg = "Bankfull Depth must be greater than or equal to 0.00001"
    '            validateBankfullDepth = False
    '        End If
    '    End Function

    '    Public Function validateMaximumFloodplainDepth(ByVal maximumFloodPlainDepth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateMaximumFloodplainDepth = True
    '        If (maximumFloodPlainDepth_ft < 0.00001) Then
    '            Msg = "Maximum Floodplain Depth must be greater than or equal to 0.00001"
    '            validateMaximumFloodplainDepth = False
    '        End If
    '    End Function

    '    Public Function validateLeftSideFloodplainWidth(ByVal leftSideFloodplainWidth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateLeftSideFloodplainWidth = True
    '        If (leftSideFloodplainWidth_ft < 0.00001) Then
    '            Msg = "Left Side Floodplian Width must be greater than or equal to 0.00001"
    '            validateLeftSideFloodplainWidth = False
    '        End If
    '    End Function

    '    Public Function validateRightSideFloodplainWidth(ByVal rightSideFloodplainWidth_ft As Double, ByRef Msg As String) As Boolean
    '        Msg = ""
    '        validateRightSideFloodplainWidth = True
    '        If (rightSideFloodplainWidth_ft < 0.00001) Then
    '            Msg = "Left Side Floodplian Width must be greater than or equal to 0.00001"
    '            validateRightSideFloodplainWidth = False
    '        End If
    '    End Function

    '    Public Function getMeanAnnualFlow_FPS(ByVal physiographicProvince As Integer, _
    '    ByVal drainageArea_SqMiles As Double) As Double

    '        If (physiographicProvince = 1) Then
    '            getMeanAnnualFlow_FPS = 3.41 * drainageArea_SqMiles ^ 0.85
    '        ElseIf (physiographicProvince = 2) Then
    '            getMeanAnnualFlow_FPS = 2.98 * drainageArea_SqMiles ^ 0.82
    '        ElseIf (physiographicProvince = 3) Then
    '            getMeanAnnualFlow_FPS = 1.35 * drainageArea_SqMiles ^ 0.99
    '        End If
    '        Exit Function
    'Error:
    '        getMeanAnnualFlow_FPS = -9999
    '    End Function

    '    Public Function getMeanChannelWidth_Ft(ByVal physiographicProvince As Integer, _
    '    ByVal flow_ft3PerSec As Double) As Double
    '        If (physiographicProvince = 1) Then
    '            getMeanChannelWidth_Ft = 6.09 * flow_ft3PerSec ^ 0.47
    '        ElseIf (physiographicProvince = 2) Then
    '            getMeanChannelWidth_Ft = 3.82 * flow_ft3PerSec ^ 0.58
    '        ElseIf (physiographicProvince = 3) Then
    '            getMeanChannelWidth_Ft = 7.39 * flow_ft3PerSec ^ 0.47
    '        End If
    '    End Function

    '    Public Function getMeanChannelDepth_Ft(ByVal physiographicProvince As Integer, _
    '    ByVal flow_ft3PerSec As Double) As Double
    '        If (physiographicProvince = 1) Then
    '            getMeanChannelDepth_Ft = 0.33 * flow_ft3PerSec ^ 0.27
    '        ElseIf (physiographicProvince = 2) Then
    '            getMeanChannelDepth_Ft = 0.31 * flow_ft3PerSec ^ 0.3
    '        ElseIf (physiographicProvince = 3) Then
    '            getMeanChannelDepth_Ft = 0.4 * flow_ft3PerSec ^ 0.23
    '        End If
    '    End Function

    '    Public Function getMeanCrossSectionalArea_SqFt(ByVal physiographicProvince As Integer, _
    '    ByVal flow_ft3PerSec As Double) As Double
    '        If (physiographicProvince = 1) Then
    '            getMeanCrossSectionalArea_SqFt = 3.16 * flow_ft3PerSec ^ 0.67
    '        ElseIf (physiographicProvince = 2) Then
    '            getMeanCrossSectionalArea_SqFt = 1.12 * flow_ft3PerSec ^ 0.89
    '        ElseIf (physiographicProvince = 3) Then
    '            getMeanCrossSectionalArea_SqFt = 3.67 * flow_ft3PerSec ^ 0.65
    '        End If
    '    End Function

    '    Public Function getChannelManningsN(ByVal physiographicProvince As Integer, _
    '    ByVal drainageArea_SqMiles As Double, _
    '    ByVal streamSlope As Double) As Double
    '        Dim meanAnnualFlow_cfs As Double
    '        meanAnnualFlow_cfs = getMeanAnnualFlow_FPS(physiographicProvince, drainageArea_SqMiles)
    '        Dim depth_ft As Double
    '        depth_ft = getMeanChannelDepth_Ft(physiographicProvince, meanAnnualFlow_cfs)
    '        Dim width_ft As Double
    '        width_ft = getMeanChannelWidth_Ft(physiographicProvince, meanAnnualFlow_cfs)
    '        Dim xsectionalArea_SqFt As Double
    '        xsectionalArea_SqFt = getMeanCrossSectionalArea_SqFt(physiographicProvince, meanAnnualFlow_cfs)
    '        Dim a As Double
    '        a = xsectionalArea_SqFt
    '        Dim r As Double
    '        r = 0.67 * depth_ft
    '        Dim s As Double
    '        s = streamSlope
    '        Dim Q As Double
    '        Q = meanAnnualFlow_cfs
    '        getChannelManningsN = (1.49 * a * r ^ (2.0# / 3.0#) * s ^ (1.0# / 2.0#)) / Q
    '    End Function

    '    Public Function getFloodplainManningsN(ByVal physiographicProvince As Integer, _
    '    ByVal drainageArea_SqMiles As Double, _
    '    ByVal streamSlope As Double) As Double
    '        getFloodplainManningsN = getChannelManningsN(physiographicProvince, drainageArea_SqMiles, streamSlope)
    '    End Function

    '    Public Function getBankfullDepth_ft(ByVal meanChannelDepth As Double) As Double
    '        getBankfullDepth_ft = 5.0# * meanChannelDepth
    '    End Function

    '    Public Function getMaximumFloodplainDepth_ft(ByVal meanChannelDepth As Double) As Double
    '        getMaximumFloodplainDepth_ft = 50.0# * meanChannelDepth
    '    End Function

    '    Public Function getChannelSideSlope() As Double
    '        getChannelSideSlope = 1.5
    '    End Function

    '    Public Function getFloodplainSideSlope() As Double
    '        getFloodplainSideSlope = 1.5
    '    End Function

    '    Private Sub validateInputsForFTableCalculations(ByVal channelLength_ft As Double, _
    '    ByVal averageChannelSlope_ftPerFt As Double, _
    '    ByVal meanChannelWidth_ft As Double, _
    '    ByVal meanChannelDepth_ft As Double, _
    '    ByVal channelManningsValue As Double, _
    '    ByVal channelSideSlope_ftPerFt As Double, _
    '    ByVal bankfullDepth_ft As Double, _
    '    ByVal floodPlainManningsValue As Double, _
    '    ByVal floodplainSideSlope_ftPerFt As Double, _
    '    ByVal leftSideFloodplainWidth_ft As Double, _
    '    ByVal rightSideFloodplainWidth_ft As Double, _
    '    ByVal maximumFloodPlainDepth_ft As Double, _
    '                                                          ByRef Msg As String)
    '        Dim Rtn As Boolean
    '        Dim Message As String
    '        Message = ""
    '        Rtn = False
    '        Rtn = validateChannelLength(channelLength_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateChannelSlope(averageChannelSlope_ftPerFt, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateChannelWidth(meanChannelWidth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateChannelDepth(meanChannelDepth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateChannelManningsN(channelManningsValue, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateFloodplainManningsN(floodPlainManningsValue, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateBankfullDepth(bankfullDepth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateFloodplainSideSlope(floodplainSideSlope_ftPerFt, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateLeftSideFloodplainWidth(leftSideFloodplainWidth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateRightSideFloodplainWidth(rightSideFloodplainWidth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        Rtn = validateMaximumFloodplainDepth(maximumFloodPlainDepth_ft, Message)
    '        If (Message <> "") Then
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        If (bankfullDepth_ft <= meanChannelDepth_ft) Then
    '            Message = "Bankfull Depth must be greater than Mean Channel Depth"
    '            Msg = Msg & vbCrLf & Message
    '        End If
    '        If (maximumFloodPlainDepth_ft <= bankfullDepth_ft) Then
    '            Message = "Maximum floodplain Depth must be greater than bankfull Depth"
    '            Msg = Msg & vbCrLf & Message
    '        End If

    '    End Sub

    '    Private Function ConvertNumberToSimpleFormat(ByVal dblVvNumber As Double) As String
    '        ConvertNumberToSimpleFormat = 0
    '        Const strLcCOMPLEX_BIT As String = "E-"
    '        Const strLcPADDING_CHARATER As String = "#"
    '        Dim strLvNumber As String : strLvNumber = CStr(dblVvNumber)
    '        Dim intLvMultiplier As Integer
    '        Dim intLvPositionOfComplexBit As Integer
    '        intLvPositionOfComplexBit = InStr(strLvNumber, strLcCOMPLEX_BIT)
    '        If intLvPositionOfComplexBit > 0 Then
    '            intLvMultiplier = val(Mid(strLvNumber, intLvPositionOfComplexBit + Len(strLcCOMPLEX_BIT)))
    '        ConvertNumberToSimpleFormat = Format(dblVvNumber, "0." & String(intLvMultiplier + _
    '            Len(strLvNumber) - (Len(strLcCOMPLEX_BIT) + Len(Trim(str(intLvMultiplier)))), strLcPADDING_CHARATER))
    '        Else
    '            ConvertNumberToSimpleFormat = strLvNumber
    '        End If
    '    End Function

    '    Public Sub getFTableForNaturalTrapezoidalChannel(ByVal channelLength_ft As Double, _
    '    ByVal averageChannelSlope_ftPerFt As Double, _
    '    ByVal meanChannelWidth_ft As Double, _
    '    ByVal meanChannelDepth_ft As Double, _
    '    ByVal channelManningsValue As Double, _
    '    ByVal channelSideSlope_ftPerFt As Double, _
    '    ByVal bankfullDepth_ft As Double, _
    '    ByVal floodPlainManningsValue As Double, _
    '    ByVal floodplainSideSlope_ftPerFt As Double, _
    '    ByVal leftSideFloodplainWidth_ft As Double, _
    '    ByVal rightSideFloodplainWidth_ft As Double, _
    '    ByVal maximumFloodPlainDepth_ft As Double, _
    '                                                     ByRef Msg As String, _
    '                                                     ByRef ft() As Double)
    '        validateInputsForFTableCalculations(channelLength_ft, averageChannelSlope_ftPerFt, _
    '                                            meanChannelWidth_ft, meanChannelDepth_ft, _
    '                                            channelManningsValue, channelSideSlope_ftPerFt, _
    '                                            bankfullDepth_ft, floodPlainManningsValue, _
    '                                            floodplainSideSlope_ftPerFt, leftSideFloodplainWidth_ft, _
    '                                            rightSideFloodplainWidth_ft, maximumFloodPlainDepth_ft, _
    '                                            Msg)
    '        Trim(Msg)
    '        If (Msg <> "") Then
    '            Exit Sub
    '        End If

    '        Dim depthsChannel() As Double
    '        Dim depthsFloodplain() As Double
    '        getDepthIncrements(meanChannelDepth_ft, bankfullDepth_ft, maximumFloodPlainDepth_ft, depthsChannel, depthsFloodplain)

    '        Dim maxWaterDepth As Double
    '        maxWaterDepth = maximumFloodPlainDepth_ft + bankfullDepth_ft
    '        Dim dblCrossSecionalArea As Double 'A
    '        dblCrossSecionalArea = 0
    '        Dim dblWettedPerimeter As Double 'wd
    '        dblWettedPerimeter = 0
    '        Dim dblHyraulicRadius As Double 'HR
    '        dblHyraulicRadius = 0
    '        Dim dblWaterSurfaceWidth As Double 'W
    '        dblWaterSurfaceWidth = 0
    '        Dim dblC As Double
    '        dblC = 0
    '        Dim dblOutflow As Double 'QC
    '        dblOutflow = 0
    '        Dim dblArea As Double 'acr
    '        dblArea = 0
    '        Dim dblVolume As Double 'stot
    '        dblVolume = 0
    '        Dim dblBottomWidth As Double 'BW
    '        dblBottomWidth = meanChannelWidth_ft - 2.0# * meanChannelDepth_ft * channelSideSlope_ftPerFt
    '        Dim dblWaterSurfaceWidth_Previous As Double
    '        dblWaterSurfaceWidth_Previous = 0
    '        Dim dblArea_Previous As Double
    '        dblArea_Previous = 0
    '        Dim row As Integer
    '        Dim colDepth As Integer
    '        Dim colArea As Integer
    '        Dim colVolume As Integer
    '        Dim colFlow As Integer
    '        row = 0
    '        colDepth = 0
    '        colArea = 1
    '        colVolume = 2
    '        colFlow = 3
    '        Dim numRows As Integer
    '        numRows = UBound(depthsChannel) + UBound(depthsFloodplain)
    '    ReDim FTable(0 To numRows, 0 To 3) As Double

    '        FTable(row, colDepth) = 0 'Depth
    '        FTable(row, colArea) = 0 'Water surface area
    '        FTable(row, colVolume) = 0 'Volume
    '        FTable(row, colFlow) = 0 'Flow

    '        Dim dblWaterDepth As Double 'G
    '        Dim dblDepthIncrement As Double
    '        dblWaterDepth = 0
    '        numRows = UBound(depthsChannel)
    '        Dim i As Integer
    '        For i = 1 To numRows
    '            dblWaterDepth = depthsChannel(i)
    '            If (i = 1) Then
    '                dblDepthIncrement = depthsChannel(i)
    '            Else
    '                dblDepthIncrement = depthsChannel(i) - depthsChannel(i - 1)
    '            End If
    '            dblCrossSecionalArea = dblBottomWidth * dblWaterDepth + channelSideSlope_ftPerFt * dblWaterDepth * dblWaterDepth
    '            dblWettedPerimeter = dblBottomWidth + 2.0# * dblWaterDepth * (1.0# + channelSideSlope_ftPerFt * channelSideSlope_ftPerFt) ^ 0.5
    '            dblHyraulicRadius = dblCrossSecionalArea / dblWettedPerimeter
    '            dblWaterSurfaceWidth = dblBottomWidth + 2.0# * channelSideSlope_ftPerFt * dblWaterDepth
    '            dblWaterSurfaceWidth_Previous = dblBottomWidth + 2.0# * channelSideSlope_ftPerFt * (dblWaterDepth - dblDepthIncrement)
    '            dblC = (1.49 * (averageChannelSlope_ftPerFt) ^ 0.5) / (channelManningsValue)
    '            dblOutflow = dblCrossSecionalArea * (dblHyraulicRadius ^ (2.0# / 3.0#)) * dblC
    '            dblArea = (channelLength_ft * dblWaterSurfaceWidth) / 43560.0#
    '            dblArea_Previous = (channelLength_ft * dblWaterSurfaceWidth_Previous) / 43560.0#
    '            dblVolume = FTable(row, colVolume) + (dblArea + dblArea_Previous) / 2.0# * dblDepthIncrement
    '            row = row + 1
    '            FTable(row, colDepth) = dblWaterDepth
    '            FTable(row, colArea) = dblArea
    '            FTable(row, colVolume) = dblVolume
    '            FTable(row, colFlow) = dblOutflow
    '        Next i

    '        'FloodPlain Calculations
    '        Dim dblFloodPlainSide1Width As Double
    '        dblFloodPlainSide1Width = leftSideFloodplainWidth_ft
    '        Dim dblFloodPlainSide2Width As Double
    '        dblFloodPlainSide2Width = leftSideFloodplainWidth_ft
    '        Dim BW2 As Double
    '        BW2 = dblFloodPlainSide1Width + dblFloodPlainSide2Width
    '        Dim dblFloodPlainBottomWidth As Double
    '        dblFloodPlainBottomWidth = BW2 + dblWaterSurfaceWidth 'Bottom width of flood plain
    '        Dim dblCrossSecionalAreaFloodPlain As Double
    '        dblCrossSecionalAreaFloodPlain = 0
    '        Dim dblWettedPerimeterFloodPlain As Double
    '        dblWettedPerimeterFloodPlain = 0
    '        Dim dblHydraulicRadiusFloodPlain As Double
    '        dblHydraulicRadiusFloodPlain = 0
    '        Dim dblAreaFloodPlain As Double
    '        dblAreaFloodPlain = 0
    '        Dim dblW As Double
    '        dblW = 0
    '        Dim dblVolumeFP As Double
    '        dblVolumeFP = 0
    '        Dim dblCurrentDepth As Double
    '        dblCurrentDepth = 0
    '        Dim intNumIncrements As Integer
    '        'intNumIncrements = maximumFloodPlainDepth_ft - bankfullDepth_ft - 1

    '        'Dim dblFloodPlainDepth As Double
    '        numRows = UBound(depthsFloodplain)
    '        For i = 1 To numRows
    '            dblCurrentDepth = depthsFloodplain(i)
    '            'If (i = 1) Then
    '            dblDepthIncrement = depthsFloodplain(i) - bankfullDepth_ft
    '            'Else
    '            'dblDepthIncrement = depthsFloodplain(i) - depthsFloodplain(i - 1)
    '            'End If
    '            dblCrossSecionalAreaFloodPlain = dblFloodPlainBottomWidth * dblDepthIncrement + floodplainSideSlope_ftPerFt * dblDepthIncrement * dblDepthIncrement + dblCrossSecionalArea
    '            dblWettedPerimeterFloodPlain = BW2 + 2.0# * dblDepthIncrement * (1.0# + floodplainSideSlope_ftPerFt * floodplainSideSlope_ftPerFt) ^ 0.5 + dblWettedPerimeter
    '            dblHydraulicRadiusFloodPlain = dblCrossSecionalAreaFloodPlain / dblWettedPerimeterFloodPlain
    '            dblC = (1.49 * averageChannelSlope_ftPerFt ^ 0.5) / (floodPlainManningsValue)
    '            dblOutflow = dblCrossSecionalAreaFloodPlain * (dblHydraulicRadiusFloodPlain ^ (2.0# / 3.0#)) * dblC
    '            'dblArea = (channelLength_ft * dblWaterSurfaceWidth) / 43560#
    '            dblW = dblFloodPlainSide1Width + dblFloodPlainSide2Width + 2.0# * floodplainSideSlope_ftPerFt * dblDepthIncrement
    '            dblAreaFloodPlain = (channelLength_ft * dblW) / 43560.0# + dblArea
    '            If (i = 1) Then
    '                dblVolumeFP = (dblAreaFloodPlain + dblArea) * dblDepthIncrement / 2.0# + dblVolume
    '            Else
    '                dblVolumeFP = (dblAreaFloodPlain + FTable(row, colArea)) * (dblCurrentDepth - FTable(row, colDepth)) / 2.0# + FTable(row, colVolume)
    '                'dblVolumeFP = (dblAreaFloodPlain + FTable(row, colArea)) / 2# + FTable(row, colVolume)
    '            End If
    '            row = row + 1
    '            FTable(row, colDepth) = dblCurrentDepth
    '            FTable(row, colArea) = dblAreaFloodPlain
    '            FTable(row, colVolume) = dblVolumeFP
    '            FTable(row, colFlow) = dblOutflow
    '        Next i

    '        ft = FTable
    '    End Sub

    '    Private Sub getDepthIncrements(ByVal meanChannelDepth_ft As Double, _
    '    ByVal bankfullDepth_ft As Double, _
    '    ByVal maximumFloodPlainDepth_ft As Double, _
    '                                   ByRef depthsChannel() As Double, _
    '                                   ByRef depthsFloodplain() As Double)
    '        Dim dblWaterDepth As Double 'G
    '        Dim upLimit As Integer
    '        If (bankfullDepth_ft <= 0.02) Then
    '        ReDim depthsChannel(1 To 1)
    '            depthsChannel(1) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 0.06) Then
    '        ReDim depthsChannel(1 To 2)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 0.1) Then
    '        ReDim depthsChannel(1 To 3)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 0.2) Then
    '        ReDim depthsChannel(1 To 4)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 0.6) Then
    '        ReDim depthsChannel(1 To 5)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.6
    '            depthsChannel(5) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 1.0#) Then
    '        ReDim depthsChannel(1 To 6)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.2
    '            depthsChannel(5) = 0.6
    '            depthsChannel(6) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 1.2) Then
    '        ReDim depthsChannel(1 To 7)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.2
    '            depthsChannel(5) = 0.6
    '            depthsChannel(6) = 1.0#
    '            depthsChannel(7) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 1.6) Then
    '        ReDim depthsChannel(1 To 8)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.2
    '            depthsChannel(5) = 0.6
    '            depthsChannel(6) = 1.0#
    '            depthsChannel(7) = 1.2
    '            depthsChannel(8) = bankfullDepth_ft
    '        ElseIf (bankfullDepth_ft <= 2.0#) Then
    '        ReDim depthsChannel(1 To 9)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.2
    '            depthsChannel(5) = 0.6
    '            depthsChannel(6) = 1.0#
    '            depthsChannel(7) = 1.2
    '            depthsChannel(8) = 1.6
    '            depthsChannel(9) = bankfullDepth_ft
    '        Else
    '        ReDim depthsChannel(1 To 9)
    '            depthsChannel(1) = 0.02
    '            depthsChannel(2) = 0.06
    '            depthsChannel(3) = 0.1
    '            depthsChannel(4) = 0.2
    '            depthsChannel(5) = 0.6
    '            depthsChannel(6) = 1.0#
    '            depthsChannel(7) = 1.2
    '            depthsChannel(8) = 1.6
    '            depthsChannel(9) = 2.0#
    '            dblWaterDepth = 2.0#
    '            While (dblWaterDepth < bankfullDepth_ft)
    '                dblWaterDepth = dblWaterDepth + 1.0#
    '                upLimit = UBound(depthsChannel) + 1
    '                ReDim Preserve depthsChannel(1 To upLimit)
    '                If (dblWaterDepth < bankfullDepth_ft) Then
    '                    depthsChannel(upLimit) = dblWaterDepth
    '                Else
    '                    depthsChannel(upLimit) = bankfullDepth_ft
    '                End If
    '            End While
    '        End If

    '        dblWaterDepth = bankfullDepth_ft * 1.5
    '        If (dblWaterDepth >= maximumFloodPlainDepth_ft) Then
    '            GoTo LastDepth
    '        Else
    '        ReDim depthsFloodplain(1 To 1)
    '            depthsFloodplain(1) = dblWaterDepth
    '        End If
    '        dblWaterDepth = bankfullDepth_ft * 2.0#
    '        If (dblWaterDepth >= maximumFloodPlainDepth_ft) Then
    '            GoTo LastDepth
    '        Else
    '            upLimit = UBound(depthsFloodplain) + 1
    '        ReDim Preserve depthsFloodplain(1 To upLimit)
    '            depthsFloodplain(upLimit) = dblWaterDepth
    '        End If
    '        dblWaterDepth = bankfullDepth_ft * 2.5
    '        If (dblWaterDepth >= maximumFloodPlainDepth_ft) Then
    '            GoTo LastDepth
    '        Else
    '            upLimit = UBound(depthsFloodplain) + 1
    '        ReDim Preserve depthsFloodplain(1 To upLimit)
    '            depthsFloodplain(upLimit) = dblWaterDepth
    '        End If
    '        dblWaterDepth = bankfullDepth_ft * 3.0#
    '        If (dblWaterDepth >= maximumFloodPlainDepth_ft) Then
    '            GoTo LastDepth
    '        Else
    '            upLimit = UBound(depthsFloodplain) + 1
    '        ReDim Preserve depthsFloodplain(1 To upLimit)
    '            depthsFloodplain(upLimit) = dblWaterDepth
    '        End If
    'LastDepth:
    '        If (maximumFloodPlainDepth_ft <= bankfullDepth_ft * 1.5) Then
    '            upLimit = 1
    '        Else
    '            upLimit = UBound(depthsFloodplain) + 1
    '        End If
    '    ReDim Preserve depthsFloodplain(1 To upLimit)
    '        dblWaterDepth = maximumFloodPlainDepth_ft
    '        depthsFloodplain(upLimit) = dblWaterDepth

    '        If (maximumFloodPlainDepth_ft < meanChannelDepth_ft * 50.0#) Then
    '            upLimit = UBound(depthsFloodplain) + 1
    '        ReDim Preserve depthsFloodplain(1 To upLimit)
    '            depthsFloodplain(upLimit) = meanChannelDepth_ft * 50.0#
    '        End If
    '    End Sub

    '    Private Sub Form_Resize()
    '        If Me.Width > 480 Then
    '            Frame1.Width = Me.Width - 480
    '        End If
    '        If Frame1.Width > 4089 Then
    '            Combo1.Width = Frame1.Width - 2757
    '        End If
    '        btnFTable.Left = (Me.Width / 2) - (btnFTable.Width / 2)
    '    End Sub

    '    Private Sub Form_Unload(ByVal Cancel As Integer)
    '        lFtab = Nothing
    '        lo = Nothing
    '    End Sub
End Class