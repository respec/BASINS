Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Collections.ObjectModel

Public Class frmNewFTable

    Dim lFtab As HspfFtable
    Dim pFTableCtl As Object 'the control containing the ftable grid
    Public Sub SetCurrentFTable(ByVal ftab As HspfFtable, ByVal O As Object)
        lFtab = ftab
        pFTableCtl = O
    End Sub

    Friend Sub Init()
        '    Friend Sub Init(ByVal aCtl As ctlEditFTables)
        Me.Icon = pFTableCtl.ParentForm.Icon
        Me.Text = "New FTable"
        atxChannelLength.ValueDouble = lFtab.Operation.Tables("HYDR-PARM2").Parms("LEN").Value * 5280.0#
        atxChannelSlope.ValueDouble = SignificantDigits(lFtab.Operation.Tables("HYDR-PARM2").Parms("DELTH").Value / (lFtab.Operation.Tables("HYDR-PARM2").Parms("LEN").Value * 5280.0#), 3)
        atxDrainageArea.ValueDouble = Format(lFtab.Operation.Uci.UpstreamArea(lFtab.Id) / 640.0#, "0.##")

        cboProv.Items.Clear()
        cboProv.Items.Add("Select Physiographic Province")
        cboProv.Items.Add("Appalachian Plateau")
        cboProv.Items.Add("Blue Ridge And Ridge And Valley")
        cboProv.Items.Add("Piedmont")
        cboProv.SelectedIndex = 0

        atxChannelWidth.ValueDouble = 0
        atxChannelDepth.ValueDouble = 0
        atxChannelManningsN.ValueDouble = 0
        atxFloodplainManningsN.ValueDouble = 0
        atxBankfullDepth.ValueDouble = 0
        atxMaximumFloodplainDepth.ValueDouble = 0
        atxLeftSideFloodPlainWidth.ValueDouble = 0
        atxRightSideFloodPlainWidth.ValueDouble = 0
        atxChannelSideSlope.ValueDouble = 0
        atxFloodplainSideSlope.ValueDouble = 0

        ToggleEstimateFields(False)
        TextLabel5.Enabled = False
        TextLabel5.Visible = True
        cboProv.Cursor = Windows.Forms.Cursors.Hand
        cboProv.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList

    End Sub

    Private Sub ToggleEstimateFields(ByVal lToggleVal As Boolean)

        TextLabel5.Visible = Not lToggleVal

        atxChannelWidth.Visible = lToggleVal
        atxChannelDepth.Visible = lToggleVal
        atxChannelManningsN.Visible = lToggleVal
        atxFloodplainManningsN.Visible = lToggleVal
        atxBankfullDepth.Visible = lToggleVal
        atxMaximumFloodplainDepth.Visible = lToggleVal
        atxLeftSideFloodPlainWidth.Visible = lToggleVal
        atxRightSideFloodPlainWidth.Visible = lToggleVal
        atxChannelSideSlope.Visible = lToggleVal
        atxFloodplainSideSlope.Visible = lToggleVal

        TextLabel6.Visible = lToggleVal
        TextLabel7.Visible = lToggleVal
        TextLabel8.Visible = lToggleVal
        TextLabel9.Visible = lToggleVal
        TextLabel10.Visible = lToggleVal
        TextLabel11.Visible = lToggleVal
        TextLabel12.Visible = lToggleVal
        TextLabel13.Visible = lToggleVal
        TextLabel14.Visible = lToggleVal
        Textlabel15.Visible = lToggleVal

        cmdComputeFTable.Enabled = lToggleVal

    End Sub

    Private Sub DoEstimate()
        Dim Rtn, lChannelN As Double

        Rtn = getMeanAnnualFlow_FPS(cboProv.SelectedIndex, atxDrainageArea.ValueDouble)
        atxChannelWidth.ValueDouble = SignificantDigits(getMeanChannelWidth_Ft(cboProv.SelectedIndex, Rtn), 5)
        atxChannelDepth.ValueDouble = SignificantDigits(getMeanChannelDepth_Ft(cboProv.SelectedIndex, Rtn), 4) 'THIS ONE MODIFIED
        lChannelN = SignificantDigits(getChannelManningsN(cboProv.SelectedIndex, atxDrainageArea.ValueDouble, atxChannelSlope.ValueDouble), 5)
        atxChannelManningsN.ValueDouble = CStr(lChannelN)
        lChannelN = SignificantDigits(getFloodplainManningsN(cboProv.SelectedIndex, atxDrainageArea.ValueDouble, atxChannelSlope.ValueDouble), 5)
        atxFloodplainManningsN.ValueDouble = CStr(lChannelN)
        atxBankfullDepth.ValueDouble = SignificantDigits(getBankfullDepth_ft(Val(atxChannelDepth.ValueDouble)), 5)
        atxMaximumFloodplainDepth.ValueDouble = SignificantDigits(getMaximumFloodplainDepth_ft(Val(atxChannelDepth.ValueDouble)), 5)
        atxLeftSideFloodPlainWidth.ValueDouble = SignificantDigits(getMeanChannelWidth_Ft(cboProv.SelectedIndex, Rtn), 5)
        atxRightSideFloodPlainWidth.ValueDouble = SignificantDigits(getMeanChannelWidth_Ft(cboProv.SelectedIndex, Rtn), 5)
        atxChannelSideSlope.ValueDouble = SignificantDigits(getChannelSideSlope(), 5)
        atxFloodplainSideSlope.ValueDouble = SignificantDigits(getFloodplainSideSlope(), 5)
    End Sub
    Private Sub cmdComputeFTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdComputeFTable.Click

        Dim lft(,) As Double = Nothing
        Dim Msg As String = Nothing
        Dim lRowCount As Integer
        Dim lRow As Integer
        Dim lCol As Long
        Dim lFormat As String

        getFTableForNaturalTrapezoidalChannel(CDbl(atxChannelLength.ValueDouble), _
                                                    Val(atxChannelSlope.ValueDouble), _
                                                    Val(atxChannelWidth.ValueDouble), _
                                                    Val(atxChannelDepth.ValueDouble), _
                                                    Val(atxChannelManningsN.ValueDouble), _
                                                    Val(atxChannelSideSlope.ValueDouble), _
                                                    Val(atxBankfullDepth.ValueDouble), _
                                                    Val(atxFloodplainManningsN.ValueDouble), _
                                                    Val(atxFloodplainSideSlope.ValueDouble), _
                                                    Val(atxLeftSideFloodPlainWidth.ValueDouble), _
                                                    Val(atxRightSideFloodPlainWidth.ValueDouble), _
                                                    Val(atxMaximumFloodplainDepth.ValueDouble), _
                                                    Msg, lft)
        If (Msg <> "") Then
            MsgBox(Msg, vbOKOnly, "Compute New FTABLE Problem")
            Exit Sub
        End If

        lRowCount = UBound(lft)

        'save to new hspf ftable
       
        lFormat = "0.##"
        lFtab.Nrows = lRowCount + 1
        lFtab.Ncols = 4
        lFtab.Depth(0) = 0
        lFtab.Area(0) = 0
        lFtab.Volume(0) = 0
        lFtab.Outflow1(0) = 0
        For lRow = 0 To lRowCount
            lCol = lRow + 1
            lFtab.Depth(lCol) = Format(lft(lRow, 0), lFormat)
            lFtab.Area(lCol) = Format(lft(lRow, 1), lFormat)
            lFtab.Volume(lCol) = Format(lft(lRow, 2), lFormat)
            lFtab.Outflow1(lCol) = Format(lft(lRow, 3), lFormat)
        Next
        pFTableCtl.UpdateFTABLE(lFtab)
    End Sub

    Private Sub cboProv_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProv.SelectedIndexChanged
        If cboProv.SelectedIndex > 0 Then
            ToggleEstimateFields(True)
            DoEstimate()
        Else
            ToggleEstimateFields(False)
        End If
    End Sub

    Public Function validateDrainageArea(ByVal drainageArea_SqMiles As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateDrainageArea = True
        If (drainageArea_SqMiles < 0.00001) Then
            Msg = "Drainage Area must be greater greater than or equal to 0.00001"
            validateDrainageArea = False
        End If
    End Function

    Public Function validateChannelLength(ByVal channelLength_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelLength = True
        If (channelLength_ft < 0.00001) Then
            Msg = "Channel Length must be greater than or equal to 0.00001"
            validateChannelLength = False
        End If
    End Function

    Public Function validateChannelSlope(ByVal channelSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelSlope = True
        If (channelSlope_ftPerFt < 0.00001) Then
            Msg = "Channel Slope must be greater than or equal to 0.00001"
            validateChannelSlope = False
        End If
    End Function

    Public Function validateChannelWidth(ByVal channelWidth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelWidth = True
        If (channelWidth_ft < 0.00001) Then
            Msg = "Channel Width must be greater than or equal to 0.00001"
            validateChannelWidth = False
        End If
    End Function

    Public Function validateChannelDepth(ByVal channelDepth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelDepth = True
        If (channelDepth_ft < 0.00001) Then
            Msg = "Channel Depth must be greater than or equal to 0.00001"
            validateChannelDepth = False
        End If
    End Function

    Public Function validateChannelManningsN(ByVal channelManningsN As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelManningsN = True
        If (channelManningsN < 0.00001) Then
            Msg = "Channel Manning's N must be greater than or equal to 0.00001"
            validateChannelManningsN = False
        End If
    End Function

    Public Function validateFloodplainManningsN(ByVal floodplainManningsN As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateFloodplainManningsN = True
        If (floodplainManningsN < 0.00001) Then
            Msg = "Floodplain Manning's N must be greater than or equal to 0.00001"
            validateFloodplainManningsN = False
        End If
    End Function

    Public Function validateChannelSideSlope(ByVal channelSideSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateChannelSideSlope = True
        If (channelSideSlope_ftPerFt < 0.00001) Then
            Msg = "Channel Side Slope must be greater than or equal to 0.00001"
            validateChannelSideSlope = False
        End If
    End Function

    Public Function validateFloodplainSideSlope(ByVal floodplainSideSlope_ftPerFt As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateFloodplainSideSlope = True
        If (floodplainSideSlope_ftPerFt < 0.00001) Then
            Msg = "Floodplain Side Slope must be greater than or equal to 0.00001"
            validateFloodplainSideSlope = False
        End If
    End Function

    Public Function validateBankfullDepth(ByVal bankfullDepth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateBankfullDepth = True
        If (bankfullDepth_ft < 0.00001) Then
            Msg = "Bankfull Depth must be greater than or equal to 0.00001"
            validateBankfullDepth = False
        End If
    End Function

    Public Function validateMaximumFloodplainDepth(ByVal maximumFloodPlainDepth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateMaximumFloodplainDepth = True
        If (maximumFloodPlainDepth_ft < 0.00001) Then
            Msg = "Maximum Floodplain Depth must be greater than or equal to 0.00001"
            validateMaximumFloodplainDepth = False
        End If
    End Function

    Public Function validateLeftSideFloodplainWidth(ByVal leftSideFloodplainWidth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateLeftSideFloodplainWidth = True
        If (leftSideFloodplainWidth_ft < 0.00001) Then
            Msg = "Left Side Floodplian Width must be greater than or equal to 0.00001"
            validateLeftSideFloodplainWidth = False
        End If
    End Function

    Public Function validateRightSideFloodplainWidth(ByVal rightSideFloodplainWidth_ft As Double, ByRef Msg As String) As Boolean
        Msg = ""
        validateRightSideFloodplainWidth = True
        If (rightSideFloodplainWidth_ft < 0.00001) Then
            Msg = "Left Side Floodplian Width must be greater than or equal to 0.00001"
            validateRightSideFloodplainWidth = False
        End If
    End Function

    Public Function getMeanAnnualFlow_FPS(ByVal physiographicProvince As Integer, ByVal drainageArea_SqMiles As Double) As Double

        Try
            If (physiographicProvince = 1) Then
                getMeanAnnualFlow_FPS = 3.41 * drainageArea_SqMiles ^ 0.85
            ElseIf (physiographicProvince = 2) Then
                getMeanAnnualFlow_FPS = 2.98 * drainageArea_SqMiles ^ 0.82
            ElseIf (physiographicProvince = 3) Then
                getMeanAnnualFlow_FPS = 1.35 * drainageArea_SqMiles ^ 0.99
            End If
            Exit Function
        Catch ex As Exception
            getMeanAnnualFlow_FPS = -9999
        End Try
    End Function

    Public Function getMeanChannelWidth_Ft(ByVal physiographicProvince As Integer, ByVal flow_ft3PerSec As Double) As Double
        If (physiographicProvince = 1) Then
            getMeanChannelWidth_Ft = 6.09 * flow_ft3PerSec ^ 0.47
        ElseIf (physiographicProvince = 2) Then
            getMeanChannelWidth_Ft = 3.82 * flow_ft3PerSec ^ 0.58
        ElseIf (physiographicProvince = 3) Then
            getMeanChannelWidth_Ft = 7.39 * flow_ft3PerSec ^ 0.47
        End If
    End Function

    Public Function getMeanChannelDepth_Ft(ByVal physiographicProvince As Integer, ByVal flow_ft3PerSec As Double) As Double
        If (physiographicProvince = 1) Then
            getMeanChannelDepth_Ft = 0.33 * flow_ft3PerSec ^ 0.27
        ElseIf (physiographicProvince = 2) Then
            getMeanChannelDepth_Ft = 0.31 * flow_ft3PerSec ^ 0.3
        ElseIf (physiographicProvince = 3) Then
            getMeanChannelDepth_Ft = 0.4 * flow_ft3PerSec ^ 0.23
        End If
    End Function

    Public Function getMeanCrossSectionalArea_SqFt(ByVal physiographicProvince As Integer, ByVal flow_ft3PerSec As Double) As Double
        If (physiographicProvince = 1) Then
            getMeanCrossSectionalArea_SqFt = 3.16 * flow_ft3PerSec ^ 0.67
        ElseIf (physiographicProvince = 2) Then
            getMeanCrossSectionalArea_SqFt = 1.12 * flow_ft3PerSec ^ 0.89
        ElseIf (physiographicProvince = 3) Then
            getMeanCrossSectionalArea_SqFt = 3.67 * flow_ft3PerSec ^ 0.65
        End If
    End Function

    Public Function getChannelManningsN(ByVal physiographicProvince As Integer, _
    ByVal lDrainageArea_SqMiles As Double, _
    ByVal streamSlope As Double) As Double

        Dim lMeanAnnualFlow_cfs As Double
        Dim lDepth_ft As Double
        Dim lWidth_ft As Double
        Dim lXsectionalArea_SqFt As Double
        Dim la As Double
        Dim lr As Double
        Dim ls As Double
        Dim lQ As Double

        lMeanAnnualFlow_cfs = getMeanAnnualFlow_FPS(physiographicProvince, lDrainageArea_SqMiles)
        lDepth_ft = getMeanChannelDepth_Ft(physiographicProvince, lMeanAnnualFlow_cfs)
        lWidth_ft = getMeanChannelWidth_Ft(physiographicProvince, lMeanAnnualFlow_cfs)
        lXsectionalArea_SqFt = getMeanCrossSectionalArea_SqFt(physiographicProvince, lMeanAnnualFlow_cfs)
        la = lXsectionalArea_SqFt
        lr = 0.67 * lDepth_ft
        ls = streamSlope
        lQ = lMeanAnnualFlow_cfs
        getChannelManningsN = (1.49 * la * lr ^ (2.0# / 3.0#) * ls ^ (1.0# / 2.0#)) / lQ

    End Function

    Public Function getFloodplainManningsN(ByVal lPhysiographicProvince As Integer, _
    ByVal lDrainageArea_SqMiles As Double, _
    ByVal lStreamSlope As Double) As Double
        getFloodplainManningsN = getChannelManningsN(lPhysiographicProvince, lDrainageArea_SqMiles, lStreamSlope)
    End Function

    Public Function getBankfullDepth_ft(ByVal lMeanChannelDepth As Double) As Double
        getBankfullDepth_ft = 5.0# * lMeanChannelDepth
    End Function

    Public Function getMaximumFloodplainDepth_ft(ByVal lMeanChannelDepth As Double) As Double
        getMaximumFloodplainDepth_ft = 50.0# * lMeanChannelDepth
    End Function

    Public Function getChannelSideSlope() As Double
        getChannelSideSlope = 1.5
    End Function

    Public Function getFloodplainSideSlope() As Double
        getFloodplainSideSlope = 1.5
    End Function

    Private Sub validateInputsForFTableCalculations(ByVal channelLength_ft As Double, _
    ByVal lAverageChannelSlope_ftPerFt As Double, _
    ByVal lMeanChannelWidth_ft As Double, _
    ByVal lMeanChannelDepth_ft As Double, _
    ByVal lChannelManningsValue As Double, _
    ByVal lChannelSideSlope_ftPerFt As Double, _
    ByVal lBankfullDepth_ft As Double, _
    ByVal lFloodPlainManningsValue As Double, _
    ByVal lFloodplainSideSlope_ftPerFt As Double, _
    ByVal lLeftSideFloodplainWidth_ft As Double, _
    ByVal lRightSideFloodplainWidth_ft As Double, _
    ByVal lMaximumFloodPlainDepth_ft As Double, _
                                                          ByRef lMsg As String)
        Dim Rtn As Boolean
        Dim Message As String
        Message = ""
        Rtn = False
        Rtn = validateChannelLength(channelLength_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateChannelSlope(lAverageChannelSlope_ftPerFt, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateChannelWidth(lMeanChannelWidth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateChannelDepth(lMeanChannelDepth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateChannelManningsN(lChannelManningsValue, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateFloodplainManningsN(lFloodPlainManningsValue, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateBankfullDepth(lBankfullDepth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateFloodplainSideSlope(lFloodplainSideSlope_ftPerFt, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateLeftSideFloodplainWidth(lLeftSideFloodplainWidth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateRightSideFloodplainWidth(lRightSideFloodplainWidth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        Rtn = validateMaximumFloodplainDepth(lMaximumFloodPlainDepth_ft, Message)
        If (Message <> "") Then
            lMsg = lMsg & vbCrLf & Message
        End If
        If (lBankfullDepth_ft <= lMeanChannelDepth_ft) Then
            Message = "Bankfull Depth must be greater than Mean Channel Depth"
            lMsg = lMsg & vbCrLf & Message
        End If
        If (lMaximumFloodPlainDepth_ft <= lBankfullDepth_ft) Then
            Message = "Maximum floodplain Depth must be greater than bankfull Depth"
            lMsg = lMsg & vbCrLf & Message
        End If

    End Sub

    Private Function ConvertNumberToSimpleFormat(ByVal dblVvNumber As Double) As String
        Dim lStrLvNumber As String : lStrLvNumber = CStr(dblVvNumber)
        Dim lIntLvMultiplier As Integer
        Dim intLvPositionOfComplexBit As Integer

        ConvertNumberToSimpleFormat = 0
        Const strLcCOMPLEX_BIT As String = "E-"
        Const strLcPADDING_CHARATER As String = "#"
        intLvPositionOfComplexBit = InStr(lStrLvNumber, strLcCOMPLEX_BIT)
        If intLvPositionOfComplexBit > 0 Then
            lIntLvMultiplier = Val(Mid(lStrLvNumber, intLvPositionOfComplexBit + Len(strLcCOMPLEX_BIT)))
            ConvertNumberToSimpleFormat = Format(dblVvNumber, "0." & StrDup(lIntLvMultiplier + _
                Len(lStrLvNumber) - (Len(strLcCOMPLEX_BIT) + Len(Trim(Str(lIntLvMultiplier)))), strLcPADDING_CHARATER))
        Else
            ConvertNumberToSimpleFormat = lStrLvNumber
        End If
    End Function

    Public Sub getFTableForNaturalTrapezoidalChannel(ByVal lChannelLength_ft As Double, _
    ByVal lAverageChannelSlope_ftPerFt As Double, _
    ByVal lMeanChannelWidth_ft As Double, _
    ByVal lMeanChannelDepth_ft As Double, _
    ByVal lChannelManningsValue As Double, _
    ByVal lChannelSideSlope_ftPerFt As Double, _
    ByVal lBankfullDepth_ft As Double, _
    ByVal lFloodPlainManningsValue As Double, _
    ByVal lFloodplainSideSlope_ftPerFt As Double, _
    ByVal lLeftSideFloodplainWidth_ft As Double, _
    ByVal lRightSideFloodplainWidth_ft As Double, _
    ByVal lMaximumFloodPlainDepth_ft As Double, ByRef lMsg As String, ByRef lFt As Double(,))
        validateInputsForFTableCalculations(lChannelLength_ft, lAverageChannelSlope_ftPerFt, _
                                            lMeanChannelWidth_ft, lMeanChannelDepth_ft, _
                                            lChannelManningsValue, lChannelSideSlope_ftPerFt, _
                                            lBankfullDepth_ft, lFloodPlainManningsValue, _
                                            lFloodplainSideSlope_ftPerFt, lLeftSideFloodplainWidth_ft, _
                                            lRightSideFloodplainWidth_ft, lMaximumFloodPlainDepth_ft, _
                                            lMsg)

        Dim lRow As Integer
        Dim lColDepth As Integer
        Dim lColArea As Integer
        Dim lColVolume As Integer
        Dim lColFlow As Integer
        Dim lNumRows As Integer
        Dim lDepthsChannel() As Double = Nothing
        Dim lDepthsFloodplain() As Double = Nothing
        Dim FTable(,) As Double
        Dim lMaxWaterDepth As Double
        Dim lDblCrossSecionalArea As Double 'A
        Dim lDblWettedPerimeter As Double 'wd
        Dim lDblWaterSurfaceWidth As Double 'W
        Dim lDblC As Double
        Dim lDblOutflow As Double 'QC
        Dim lDblArea As Double 'acr
        Dim lDblVolume As Double 'stot
        Dim lDblBottomWidth As Double 'BW
        Dim lDblWaterSurfaceWidth_Previous As Double
        Dim lDblArea_Previous As Double
        Dim lDblHyraulicRadius As Double 'HR
        Dim lDblWaterDepth As Double 'G
        Dim lDblDepthIncrement As Double
        Dim lRowOper As Integer
        Dim lDblFloodPlainSide1Width As Double
        Dim lDblFloodPlainSide2Width As Double
        Dim lDblFloodPlainBottomWidth As Double
        Dim lDblCrossSecionalAreaFloodPlain As Double
        Dim lDblWettedPerimeterFloodPlain As Double
        Dim lDblHydraulicRadiusFloodPlain As Double
        Dim lDblAreaFloodPlain As Double
        Dim lDblW As Double
        Dim lDblVolumeFP As Double
        Dim lDblCurrentDepth As Double
        Dim lBW2 As Double


        Trim(lMsg)
        If (lMsg <> "") Then
            Exit Sub
        End If

        getDepthIncrements(lMeanChannelDepth_ft, lBankfullDepth_ft, lMaximumFloodPlainDepth_ft, lDepthsChannel, lDepthsFloodplain)
        lMaxWaterDepth = lMaximumFloodPlainDepth_ft + lBankfullDepth_ft
        lDblCrossSecionalArea = 0
        lDblWettedPerimeter = 0
        lDblHyraulicRadius = 0
        lDblWaterSurfaceWidth = 0
        lDblC = 0
        lDblOutflow = 0
        lDblArea = 0
        lDblVolume = 0
        lDblBottomWidth = lMeanChannelWidth_ft - 2.0# * lMeanChannelDepth_ft * lChannelSideSlope_ftPerFt
        lDblWaterSurfaceWidth_Previous = 0
        lDblArea_Previous = 0
        lRow = 0
        lColDepth = 0
        lColArea = 1
        lColVolume = 2
        lColFlow = 3
        lNumRows = UBound(lDepthsChannel) + UBound(lDepthsFloodplain)

        ReDim FTable(lNumRows + 2, 3)

        FTable(lRow, lColDepth) = 0 'Depth
        FTable(lRow, lColArea) = 0 'Water surface area
        FTable(lRow, lColVolume) = 0 'Volume
        FTable(lRow, lColFlow) = 0 'Flow


        lDblWaterDepth = 0
        lNumRows = UBound(lDepthsChannel)

        For lRowOper = 0 To lNumRows
            lDblWaterDepth = lDepthsChannel(lRowOper)
            If (lRowOper = 0) Then
                lDblDepthIncrement = lDepthsChannel(lRowOper)
            Else
                lDblDepthIncrement = lDepthsChannel(lRowOper) - lDepthsChannel(lRowOper - 1)
            End If
            lDblCrossSecionalArea = lDblBottomWidth * lDblWaterDepth + lChannelSideSlope_ftPerFt * lDblWaterDepth * lDblWaterDepth
            lDblWettedPerimeter = lDblBottomWidth + 2.0# * lDblWaterDepth * (1.0# + lChannelSideSlope_ftPerFt * lChannelSideSlope_ftPerFt) ^ 0.5
            lDblHyraulicRadius = lDblCrossSecionalArea / lDblWettedPerimeter
            lDblWaterSurfaceWidth = lDblBottomWidth + 2.0# * lChannelSideSlope_ftPerFt * lDblWaterDepth
            lDblWaterSurfaceWidth_Previous = lDblBottomWidth + 2.0# * lChannelSideSlope_ftPerFt * (lDblWaterDepth - lDblDepthIncrement)
            lDblC = (1.49 * (lAverageChannelSlope_ftPerFt) ^ 0.5) / (lChannelManningsValue)
            lDblOutflow = lDblCrossSecionalArea * (lDblHyraulicRadius ^ (2.0# / 3.0#)) * lDblC
            lDblArea = (lChannelLength_ft * lDblWaterSurfaceWidth) / 43560.0#
            lDblArea_Previous = (lChannelLength_ft * lDblWaterSurfaceWidth_Previous) / 43560.0#
            lDblVolume = FTable(lRow, lColVolume) + (lDblArea + lDblArea_Previous) / 2.0# * lDblDepthIncrement
            lRow = lRow + 1
            FTable(lRow, lColDepth) = lDblWaterDepth
            FTable(lRow, lColArea) = lDblArea
            FTable(lRow, lColVolume) = lDblVolume
            FTable(lRow, lColFlow) = lDblOutflow
        Next

        'FloodPlain Calculations
        lDblFloodPlainSide1Width = lLeftSideFloodplainWidth_ft
        lDblFloodPlainSide2Width = lLeftSideFloodplainWidth_ft
        lBW2 = lDblFloodPlainSide1Width + lDblFloodPlainSide2Width
        lDblFloodPlainBottomWidth = lBW2 + lDblWaterSurfaceWidth 'Bottom width of flood plain
        lDblCrossSecionalAreaFloodPlain = 0
        lDblWettedPerimeterFloodPlain = 0
        lDblHydraulicRadiusFloodPlain = 0
        lDblAreaFloodPlain = 0
        lDblW = 0
        lDblVolumeFP = 0
        lDblCurrentDepth = 0

        'Dim intNumIncrements As Integer
        'intNumIncrements = maximumFloodPlainDepth_ft - bankfullDepth_ft - 1

        'Dim dblFloodPlainDepth As Double
        lNumRows = UBound(lDepthsFloodplain)
        For lRowOper = 0 To lNumRows
            lDblCurrentDepth = lDepthsFloodplain(lRowOper)
            'If (i = 1) Then
            lDblDepthIncrement = lDepthsFloodplain(lRowOper) - lBankfullDepth_ft
            'Else
            'dblDepthIncrement = depthsFloodplain(i) - depthsFloodplain(i - 1)
            'End If
            lDblCrossSecionalAreaFloodPlain = lDblFloodPlainBottomWidth * lDblDepthIncrement + lFloodplainSideSlope_ftPerFt * lDblDepthIncrement * lDblDepthIncrement + lDblCrossSecionalArea
            lDblWettedPerimeterFloodPlain = lBW2 + 2.0# * lDblDepthIncrement * (1.0# + lFloodplainSideSlope_ftPerFt * lFloodplainSideSlope_ftPerFt) ^ 0.5 + lDblWettedPerimeter
            lDblHydraulicRadiusFloodPlain = lDblCrossSecionalAreaFloodPlain / lDblWettedPerimeterFloodPlain
            lDblC = (1.49 * lAverageChannelSlope_ftPerFt ^ 0.5) / (lFloodPlainManningsValue)
            lDblOutflow = lDblCrossSecionalAreaFloodPlain * (lDblHydraulicRadiusFloodPlain ^ (2.0# / 3.0#)) * lDblC
            'dblArea = (channelLength_ft * dblWaterSurfaceWidth) / 43560#
            lDblW = lDblFloodPlainSide1Width + lDblFloodPlainSide2Width + 2.0# * lFloodplainSideSlope_ftPerFt * lDblDepthIncrement
            lDblAreaFloodPlain = (lChannelLength_ft * lDblW) / 43560.0# + lDblArea
            If (lRowOper = 0) Then
                lDblVolumeFP = (lDblAreaFloodPlain + lDblArea) * lDblDepthIncrement / 2.0# + lDblVolume
            Else
                lDblVolumeFP = (lDblAreaFloodPlain + FTable(lRow, lColArea)) * (lDblCurrentDepth - FTable(lRow, lColDepth)) / 2.0# + FTable(lRow, lColVolume)
                'dblVolumeFP = (dblAreaFloodPlain + FTable(lRow, lColArea)) / 2# + FTable(lRow, lColVolume)
            End If
            lRow = lRow + 1
            FTable(lRow, lColDepth) = lDblCurrentDepth
            FTable(lRow, lColArea) = lDblAreaFloodPlain
            FTable(lRow, lColVolume) = lDblVolumeFP
            FTable(lRow, lColFlow) = lDblOutflow
        Next
        lFt = FTable
    End Sub

    Private Sub getDepthIncrements(ByVal lMeanChannelDepth_ft As Double, _
    ByVal lBankfullDepth_ft As Double, _
    ByVal lMaximumFloodPlainDepth_ft As Double, _
                                   ByRef lDepthsChannel() As Double, _
                                   ByRef lDepthsFloodplain() As Double)
        Dim lDblWaterDepth As Double 'G

        If (lBankfullDepth_ft <= 0.02) Then
            ReDim lDepthsChannel(0)
            lDepthsChannel(0) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 0.06) Then
            ReDim lDepthsChannel(1)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 0.1) Then
            ReDim lDepthsChannel(2)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 0.2) Then
            ReDim lDepthsChannel(3)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 0.6) Then
            ReDim lDepthsChannel(4)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.6
            lDepthsChannel(4) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 1.0#) Then
            ReDim lDepthsChannel(5)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.2
            lDepthsChannel(4) = 0.6
            lDepthsChannel(5) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 1.2) Then
            ReDim lDepthsChannel(6)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.2
            lDepthsChannel(4) = 0.6
            lDepthsChannel(5) = 1.0#
            lDepthsChannel(6) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 1.6) Then
            ReDim lDepthsChannel(7)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.2
            lDepthsChannel(4) = 0.6
            lDepthsChannel(5) = 1.0#
            lDepthsChannel(6) = 1.2
            lDepthsChannel(7) = lBankfullDepth_ft
        ElseIf (lBankfullDepth_ft <= 2.0#) Then
            ReDim lDepthsChannel(8)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.2
            lDepthsChannel(4) = 0.6
            lDepthsChannel(5) = 1.0#
            lDepthsChannel(6) = 1.2
            lDepthsChannel(7) = 1.6
            lDepthsChannel(8) = lBankfullDepth_ft
        Else
            ReDim lDepthsChannel(8)
            lDepthsChannel(0) = 0.02
            lDepthsChannel(1) = 0.06
            lDepthsChannel(2) = 0.1
            lDepthsChannel(3) = 0.2
            lDepthsChannel(4) = 0.6
            lDepthsChannel(5) = 1.0#
            lDepthsChannel(6) = 1.2
            lDepthsChannel(7) = 1.6
            lDepthsChannel(8) = 2.0#
            lDblWaterDepth = 2.0#
            While (lDblWaterDepth < lBankfullDepth_ft)
                lDblWaterDepth = lDblWaterDepth + 1.0#
                ReDim Preserve lDepthsChannel(lDepthsChannel.Length)
                If (lDblWaterDepth < lBankfullDepth_ft) Then
                    lDepthsChannel(lDepthsChannel.Length - 1) = lDblWaterDepth
                Else
                    lDepthsChannel(lDepthsChannel.Length - 1) = lBankfullDepth_ft
                End If
            End While
        End If

        lDblWaterDepth = lBankfullDepth_ft * 1.5
        If (lDblWaterDepth >= lMaximumFloodPlainDepth_ft) Then
            'oh
        Else
            ReDim lDepthsFloodplain(0)
            lDepthsFloodplain(0) = lDblWaterDepth
        End If
        lDblWaterDepth = lBankfullDepth_ft * 2.0#
        If (lDblWaterDepth >= lMaximumFloodPlainDepth_ft) Then
            GoTo LastDepth
        Else
            ReDim Preserve lDepthsFloodplain(lDepthsFloodplain.Length)
            lDepthsFloodplain(lDepthsFloodplain.Length - 1) = lDblWaterDepth
        End If
        lDblWaterDepth = lBankfullDepth_ft * 2.5
        If (lDblWaterDepth >= lMaximumFloodPlainDepth_ft) Then
            GoTo LastDepth
        Else
            ReDim Preserve lDepthsFloodplain(lDepthsFloodplain.Length)
            lDepthsFloodplain(lDepthsFloodplain.Length - 1) = lDblWaterDepth
        End If
        lDblWaterDepth = lBankfullDepth_ft * 3.0#
        If (lDblWaterDepth >= lMaximumFloodPlainDepth_ft) Then
            GoTo LastDepth
        Else
            ReDim Preserve lDepthsFloodplain(lDepthsFloodplain.Length)
            lDepthsFloodplain(lDepthsFloodplain.Length - 1) = lDblWaterDepth
        End If

LastDepth:
        If (lMaximumFloodPlainDepth_ft <= lBankfullDepth_ft * 1.5) Then
            ReDim Preserve lDepthsFloodplain(0)
        Else
            ReDim Preserve lDepthsFloodplain(lDepthsFloodplain.Length)
        End If
        lDblWaterDepth = lMaximumFloodPlainDepth_ft
        lDepthsFloodplain(lDepthsFloodplain.Length - 1) = lDblWaterDepth

        If (lMaximumFloodPlainDepth_ft < lMeanChannelDepth_ft * 50.0#) Then
            ReDim Preserve lDepthsFloodplain(lDepthsFloodplain.Length)
            lDepthsFloodplain(lDepthsFloodplain.Length - 1) = lMeanChannelDepth_ft * 50.0#
        End If
    End Sub

    'Private Sub Form_Unload(ByVal Cancel As Integer) Handles Me.FormClosed
    '    lFtab = Nothing
    '    lo = Nothing
    'End Sub

End Class