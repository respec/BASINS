Imports atcData
Imports atcUtility
Imports atcUSGSUtility
Imports atcTimeseriesBaseflow
Imports atcTimeseriesRDB
Imports atcGraph
Imports ZedGraph
Imports MapWinUtility
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class frmUSGSBaseflow
    Private pName As String = "Unnamed"
    Private pBasicAttributes As Generic.List(Of String)
    Private pDateFormat As atcDateFormat

    Private pYearStartMonth As Integer = 0
    Private pYearStartDay As Integer = 0
    Private pYearEndMonth As Integer = 0
    Private pYearEndDay As Integer = 0
    Private pFirstYear As Integer = 0
    Private pLastYear As Integer = 0

    Private pCommonStart As Double = GetMinValue()
    Private pCommonEnd As Double = GetMaxValue()
    Private Const pNoDatesInCommon As String = ": No dates in common"
    Private pAnalysisOverCommonDuration As Boolean = True

    Private pMethods As ArrayList = Nothing
    Private pOutputDir As String = ""
    'Private pBaseOutputFilename As String = ""
    Private pMethodLastDone As String = ""
    'Private pMethodsLastDone As ArrayList = Nothing
    Private pDALastUsed As Double = 0.0
    Private pBFINDay As Integer = 0
    Private pBFIFrac As Double = 0
    Private pBFIK1Day As Double = 0
    Private pBFIUseSymbol As Boolean = False
    Private pTwoParamEstimationMethod As clsBaseflow2PRDF.ETWOPARAMESTIMATION = clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE

    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private WithEvents pfrmStations As frmStations

    Public Opened As Boolean = False
    Private pDidBFSeparation As Boolean = False

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing,
                      Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing,
                      Optional ByVal aShowForm As Boolean = True)
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        If aBasicAttributes Is Nothing Then
            pBasicAttributes = atcDataManager.DisplayAttributes
        Else
            pBasicAttributes = aBasicAttributes
        End If
        pMethods = New ArrayList()
        MethodsLastDone = New ArrayList()
        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.DropDownItems.Add(lMenuText, Nothing, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis",
                                                       pDataGroup, Nothing, True, True, Me.Icon)
        Else
            Dim lconstituent As String = pDataGroup(0).Attributes.GetValue("Constituent", "")
            If String.IsNullOrEmpty(lconstituent) Then
                Dim lParams() As String = {"Yes", "No"}
                Dim lResponse As String = Logger.MsgCustomOwned("Is this a streamflow dataset?", "Base-flow Separation", Me, lParams)
                If lResponse = "Yes" Then
                    pDataGroup(0).Attributes.SetValue("Constituent", "Streamflow")
                Else
                    Logger.Msg("Please select a daily streamflow dataset for analysis.", MsgBoxStyle.Information, "Base-flow Separation")
                    Me.Close()
                    Exit Sub
                End If
            End If
        End If

        If pDataGroup.Count > 0 Then
            PopulateForm()
            If aShowForm Then Me.Show()
        Else 'user declined to specify timeseries
            Me.Close()
        End If

    End Sub

    Private Sub PopulateForm()
        pDateFormat = New atcDateFormat
        With pDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .IncludeSeconds = False
        End With

        pOutputDir = GetSetting("atcUSGSBaseflow", "Defaults", "OutputDir", "")
        OutputFilenameRoot = GetSetting("atcUSGSBaseflow", "Defaults", "BaseOutputFilename", "")

        'atcUSGSStations.StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(0)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
            End If
            If lDataset.Attributes.ContainsAttribute("Drainage Area") Then
                txtDrainageArea.Text = DoubleToString(lDataset.Attributes.GetValue("Drainage Area"))
            End If
        Next

        If lFirstDate < GetMaxValue() AndAlso lLastDate > GetMinValue() Then
            If txtDataStart.Tag IsNot Nothing AndAlso txtDataEnd.Tag IsNot Nothing Then
                txtDataStart.Text = txtDataStart.Tag & " " & pDateFormat.JDateToString(lFirstDate + 1)
                txtDataEnd.Text = txtDataEnd.Tag & " " & pDateFormat.JDateToString(lLastDate)
            Else
                txtDataStart.Text = pDateFormat.JDateToString(lFirstDate + 1)
                txtDataEnd.Text = pDateFormat.JDateToString(lLastDate)
            End If
            If txtStartDateUser.Text.Length = 0 Then
                txtStartDateUser.Text = txtDataStart.Text
            End If
            If txtEndDateUser.Text.Length = 0 Then
                txtEndDateUser.Text = txtDataEnd.Text
            End If
        End If

        txtOutputDir.Text = pOutputDir
        txtOutputRootName.Text = OutputFilenameRoot
    End Sub

    Public Function AskUser(ByVal aName As String,
                        ByVal aDataGroup As atcData.atcTimeseriesGroup,
                        ByRef aStartMonth As Integer,
                        ByRef aStartDay As Integer,
                        ByRef aEndMonth As Integer,
                        ByRef aEndDay As Integer,
                        ByRef aFirstYear As Integer,
                        ByRef aLastYear As Integer) As Boolean

    End Function

    Private Function AttributesFromForm(ByRef Args As atcDataAttributes) As String
        'check validity of inputs
        Dim lErrMsg As String = ""

        Dim lSDate As Double = Double.NaN
        Dim lEDate As Double = Double.NaN
        If pDataGroup.Count = 0 Then
            lErrMsg &= "- No streamflow data selected" & vbCrLf
        Else
            lSDate = StartDateFromForm()
            lEDate = EndDateFromForm()
            If lSDate < 0 OrElse lEDate < 0 Then
                lErrMsg &= "- Problematic start and/or end date." & vbCrLf
            ElseIf (lEDate - lSDate) / (JulianHour * 24) < 31 Then
                lErrMsg &= "- Analysis duration is too short (31 days minimum)." & vbCrLf
            Else
                'Dim lTs As atcTimeseries = Nothing
                'For Each lTs In pDataGroup
                '    Try
                '        lTs = SubsetByDate(lTs, lSDate, lEDate, Nothing)
                '        If lTs.Attributes.GetValue("Count missing") > 0 Then
                '            If chkMethodHySEPFixed.Checked OrElse
                '               chkMethodHySEPLocMin.Checked OrElse
                '               chkMethodHySEPSlide.Checked OrElse
                '               chkMethodPART.Checked Then
                '                lErrMsg &= "- Selected Dataset has gaps." & vbCrLf
                '                lTs.Clear()
                '                Exit For
                '            End If
                '        ElseIf lTs.numValues < 31 Then
                '            lErrMsg &= "- Selected Dataset is too short (31 days minimum)." & vbCrLf
                '            lTs.Clear()
                '            Exit For
                '        Else
                '            lTs.Clear()
                '        End If
                '    Catch ex As Exception
                '        lErrMsg &= "- Problematic starting and ending dates." & vbCrLf
                '    End Try
                'Next
            End If
        End If

        'If pMethod = "" Then lErrMsg = "- Method not set" & vbCrLf
        If pMethods.Count = 0 Then lErrMsg = "- Method not set" & vbCrLf
        Dim lDA As Double = 0.0
        If Not Double.TryParse(txtDrainageArea.Text.Trim, lDA) Then
            If chkMethodHySEPFixed.Checked OrElse
               chkMethodHySEPLocMin.Checked OrElse
               chkMethodHySEPSlide.Checked OrElse
               chkMethodPART.Checked Then
                lErrMsg &= "- Drainage Area not set" & vbCrLf
            End If
        ElseIf lDA <= 0 Then
            lErrMsg &= "- Drainage Area must be greater than zero" & vbCrLf
        End If
        Dim lNDay As Integer
        If chkMethodBFIStandard.Checked OrElse chkMethodBFIModified.Checked Then
            If Not Integer.TryParse(txtN.Text.Trim(), lNDay) Then
                lErrMsg &= "- BFI method needs a valid screening duration (N)" & vbCrLf
            End If
        End If
        Dim lFrac As Double
        If chkMethodBFIStandard.Checked Then
            If Not Double.TryParse(txtF.Text.Trim(), lFrac) Then
                lErrMsg &= "- BFI standard method needs a valid turning point fraction (F)" & vbCrLf
            End If
        End If
        Dim lK1Day As Double
        If chkMethodBFIModified.Checked Then
            If Not Double.TryParse(txtK.Text.Trim(), lK1Day) Then
                lErrMsg &= "- BFI modified method needs a valid recession constant (K)" & vbCrLf
            End If
        End If

        Dim lDFBeta As Double = Double.NaN
        If chkMethodBFLOW.Checked Then
            If Not Double.TryParse(txtDFParamBeta.Text.Trim(), lDFBeta) Then
                lErrMsg &= "- BFLOW method needs a valid filter parameter." & vbCrLf
            End If
        End If
        Dim lDFRC As Double = Double.NaN
        Dim lDFBFImax As Double = Double.NaN
        If chkMethodTwoPRDF.Checked Then
            If pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT Then
                'User choose Default parameter
                If lErrMsg.Length = 0 Then
                    'For default, use the Eckhardt method to estimate the RC and BFImax parameters
                    'using the original time series subset to the user specified analysis duration
                    Try
                        Dim lDF2P As New clsBaseflow2PRDF()
                        Dim lTsAnalysis As atcTimeseries = SubsetByDate(pDataGroup(0), lSDate, lEDate, Nothing)
                        If lTsAnalysis IsNot Nothing Then
                            lDF2P.CalculateBFImax_RC(lTsAnalysis)
                            lDFRC = lDF2P.RC
                            lDFBFImax = lDF2P.BFImax
                        Else
                            lDFRC = Double.NaN
                            lDFBFImax = Double.NaN
                            lErrMsg &= "- Default TwoPRDF method failed estimate RC and BFImax (check dataset)." & vbCrLf
                        End If
                    Catch ex As Exception
                        lDFRC = Double.NaN
                        lDFBFImax = Double.NaN
                        lErrMsg &= "- Default TwoPRDF method failed estimate RC and BFImax (check dataset)." & vbCrLf
                    End Try

                End If
            ElseIf pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM Then
                'User choose Specify parameter
                If Not Double.TryParse(txtDFParamRC.Text.Trim(), lDFRC) Then
                    lErrMsg &= "- Custom TwoPRDF method needs a valid recession constant." & vbCrLf
                End If
                If Not Double.TryParse(txtDFParamBFImax.Text.Trim(), lDFBFImax) OrElse
                    Not (lDFBFImax > 0 AndAlso lDFBFImax < 1) Then
                    lErrMsg &= "- Custom TwoPRDF method needs a valid BFImax." & vbCrLf
                End If
            End If
        End If

        If IO.Directory.Exists(txtOutputDir.Text) Then
            Args.SetValue("OutputDir", txtOutputDir.Text)
        End If

        If lErrMsg.Length = 0 Then
            'set methods
            Args.SetValue(BFInputNames.BFMethods, pMethods) '"Methods"
            'Set drainage area
            Args.SetValue(BFInputNames.DrainageArea, lDA) '"Drainage Area"
            'set duration
            Args.SetValue(BFInputNames.StartDate, lSDate) '"Start Date"
            Args.SetValue(BFInputNames.EndDate, lEDate) '"End Date"
            'Set streamflow
            Args.SetValue(BFInputNames.Streamflow, pDataGroup) '"Streamflow"
            'Set Unit
            Args.SetValue(BFInputNames.EnglishUnit, True) '"EnglishUnit"
            'Set station.txt
            'Args.SetValue("Station File", atcUSGSStations.StationInfoFile)
            If pMethods.Contains(BFMethods.BFIStandard) Then
                Args.SetValue(BFInputNames.BFITurnPtFrac, lFrac) '"BFIFrac"
            End If
            If pMethods.Contains(BFMethods.BFIModified) Then
                Args.SetValue(BFInputNames.BFIRecessConst, lK1Day) '"BFIK1Day"
            End If
            If pMethods.Contains(BFMethods.BFIStandard) OrElse pMethods.Contains(BFMethods.BFIModified) Then
                Args.SetValue(BFInputNames.BFINDayScreen, lNDay) '"BFINDay"
                'Args.SetValue(BFInputNames.BFIUseSymbol, (chkBFISymbols.Checked)) '"BFIUseSymbol"
                Dim lBFIYearBasis As String = BFInputNames.BFIReportbyCY '"Calendar"
                If rdoBFIReportbyWaterYear.Checked Then
                    lBFIYearBasis = BFInputNames.BFIReportbyWY '"Water"
                End If
                Args.SetValue(BFInputNames.BFIReportby, lBFIYearBasis) '"BFIReportby"
            End If
            If pMethods.Contains(BFMethods.BFLOW) Then
                Args.SetValue(BFInputNames.BFLOWFilter, lDFBeta)
            End If
            If pMethods.Contains(BFMethods.TwoPRDF) Then
                Args.SetValue(BFInputNames.TwoPRDFRC, lDFRC)
                Args.SetValue(BFInputNames.TwoPRDFBFImax, lDFBFImax)
                Args.SetValue(BFInputNames.TwoParamEstMethod, pTwoParamEstimationMethod)
            End If
        End If
        Return lErrMsg
    End Function

    Private Sub ClearAttributes()
        Dim lRemoveThese As New atcCollection
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Baseflow") Then
                        lRemoveThese.Add(lAttribute)
                    End If
                End If
            Next
            For Each lAttribute As atcDefinedValue In lRemoveThese
                lData.Attributes.Remove(lAttribute)
            Next
        Next
    End Sub

    Private Function StartDateFromForm() As Double
        Dim lMatches As MatchCollection = Nothing
        If txtStartDateUser.Text.Trim() = "" Then
            lMatches = Regex.Matches(txtDataStart.Text, "\d{4}\/\d{1,2}\/\d{1,2}")
        Else
            lMatches = Regex.Matches(txtStartDateUser.Text, "\d{4}\/\d{1,2}\/\d{1,2}")
        End If
        Dim lArr() As String = Nothing
        If lMatches.Count > 0 Then
            lArr = lMatches.Item(0).ToString.Split("/")
        Else
            Dim lAskUser As String =
            Logger.MsgCustomOwned("Invalid starting date. Use dataset start date?", "Start Date Correction", Me, New String() {"Yes", "No"})
            If lAskUser = "Yes" Then
                If String.IsNullOrEmpty(txtDataStart.Text.Trim()) Then
                    Dim lDates(5) As Integer
                    Dim lDateVal As Double
                    For I As Integer = 0 To pDataGroup(0).Dates.numValues
                        lDateVal = pDataGroup(0).Dates.Value(I)
                        If Not Double.IsNaN(lDateVal) Then
                            J2Date(lDateVal, lDates)
                            ReDim lArr(2)
                            lArr(0) = lDates(0).ToString()
                            lArr(1) = lDates(1).ToString()
                            lArr(2) = lDates(2).ToString()
                            Exit For
                        End If
                    Next
                Else
                    lArr = txtDataStart.Text.Trim.Split("/")
                    txtStartDateUser.Text = ""
                End If
            Else
                txtStartDateUser.Focus()
                Return -99.0
            End If
        End If

        Dim lYear As Integer = lArr(0)
        Dim lMonth As Integer = lArr(1)
        Dim lDay As Integer = lArr(2)
        If IsDateValid(lArr) Then
            If pAnalysisOverCommonDuration Then
                pCommonStart = Date2J(lYear, lMonth, lDay)
            End If
        Else
            Return -99.0
        End If
        Return pCommonStart
    End Function

    Private Function EndDateFromForm() As Double
        Dim lMatches As MatchCollection = Nothing
        If txtEndDateUser.Text.Trim() = "" Then
            lMatches = Regex.Matches(txtDataEnd.Text, "\d{4}/\d{1,2}/\d{1,2}")
        Else
            lMatches = Regex.Matches(txtEndDateUser.Text, "\d{4}/\d{1,2}/\d{1,2}")
        End If
        Dim lArr() As String = Nothing
        If lMatches.Count > 0 Then
            lArr = lMatches.Item(0).ToString.Split("/")
        Else
            Dim lAskUser As String =
            Logger.MsgCustomOwned("Invalid ending date. Use dataset end date?", "End Date Correction", Me, New String() {"Yes", "No"})
            If lAskUser = "Yes" Then
                If String.IsNullOrEmpty(txtDataEnd.Text.Trim()) Then
                    Dim lDates(5) As Integer
                    Dim lDateVal As Double
                    For I As Integer = pDataGroup(0).Dates.numValues To 0 Step -1
                        lDateVal = pDataGroup(0).Dates.Value(I)
                        If Not Double.IsNaN(lDateVal) Then
                            J2Date(lDateVal, lDates)
                            ReDim lArr(2)
                            lArr(0) = lDates(0).ToString()
                            lArr(1) = lDates(1).ToString()
                            lArr(2) = lDates(2).ToString()
                            Exit For
                        End If
                    Next
                Else
                    lArr = txtDataEnd.Text.Trim.Split("/")
                    txtEndDateUser.Text = ""
                End If
            Else
                txtEndDateUser.Focus()
                Return -99.0
            End If

        End If
        Dim lYear As Integer = lArr(0)
        Dim lMonth As Integer = lArr(1)
        Dim lDay As Integer = lArr(2)
        If IsDateValid(lArr) Then
            If pAnalysisOverCommonDuration Then
                pCommonEnd = Date2J(lYear, lMonth, lDay, 24, 0, 0)
            End If
        Else
            Return -99.0
        End If

        Return pCommonEnd
    End Function

    Private Function IsDateValid(ByVal aDate() As String) As Boolean
        Dim isGoodDate As Boolean = True
        Dim lYear As Integer = aDate(0)
        Dim lMonth As Integer = aDate(1)
        Dim lDay As Integer = aDate(2)

        If lMonth > 12 OrElse lMonth < 1 Then
            isGoodDate = False
        ElseIf lDay > DayMon(lYear, lMonth) Then
            isGoodDate = False
        End If
        Return isGoodDate
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    'Private Sub btnFindStations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindStations.Click
    '    atcUSGSStations.StationInfoFile = FindFile("Locate Station File", atcUSGSStations.StationInfoFile, "txt")
    '    SaveSetting("atcUSGSBaseflow", "Defaults", "Stations", atcUSGSStations.StationInfoFile)
    '    atcUSGSStations.GetStations()
    '    pfrmStations = New frmStations()
    '    pfrmStations.AskUser(atcUSGSStations.Stations)
    'End Sub

    Private Sub StationSelectionChanged(ByVal aSelectedIndex As Integer, ByVal aStationList As atcCollection, ByVal aIsDataDirty As Boolean) Handles pfrmStations.StationInfoChanged
        Dim lStationFilename As String
        Dim lDrainageArea As Double
        If aSelectedIndex >= 0 AndAlso aStationList.Item(aSelectedIndex) IsNot Nothing Then
            lStationFilename = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).Filename
            lDrainageArea = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).DrainageArea
            If lDrainageArea < 0 Then lDrainageArea = 0.0
        End If
        txtDrainageArea.Text = lDrainageArea.ToString

        If aIsDataDirty Then
            'atcUSGSStations.SaveStations(aStationList, atcUSGSStations.StationInfoFile)
        End If
    End Sub

    Private Sub btnExamineData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExamineData.Click
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New atcUSGSUtility.frmDataSummary(atcUSGSScreen.PrintDataSummary(lTs))
            lfrmDataSummary.ClearSelection()
            lfrmDataSummary.Show()
        Next
    End Sub

    'Private Sub cboBFMothod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    pMethod = sender.SelectedItem()
    'End Sub

    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    pDataGroup.Clear()
    '    pDataGroup = Nothing
    '    Me.Dispose()
    '    Me.Close()
    'End Sub

    'Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ComputeBaseflow()
    'End Sub

    Private Sub ComputeBaseflow()
        Dim lArgs As New atcDataAttributes
        Dim lFormCheckMsg As String = AttributesFromForm(lArgs)
        If lFormCheckMsg.Length > 0 Then
            Logger.Msg("Please address the following issues before proceeding:" & vbCrLf & vbCrLf & lFormCheckMsg, MsgBoxStyle.Information, "Input Needs Correction")
            Exit Sub
        End If
        ClearAttributes()
        modBaseflowUtil.ComputeBaseflowIntermittent(lArgs, True)
        'pMethodLastDone = lArgs.GetValue("Method")
        MethodsLastDone = lArgs.GetValue(BFInputNames.BFMethods)
        pDALastUsed = lArgs.GetValue(BFInputNames.DrainageArea) '"Drainage Area")
        Dim lBFIchosen As Boolean = False
        If MethodsLastDone.Contains(BFMethods.BFIStandard) Then
            lBFIchosen = True
            pBFIFrac = lArgs.GetValue(BFInputNames.BFITurnPtFrac) '"BFIFrac")
        End If
        If MethodsLastDone.Contains(BFMethods.BFIModified) Then
            lBFIchosen = True
            pBFIK1Day = lArgs.GetValue(BFInputNames.BFIRecessConst) '"BFIK1Day")
        End If
        If lBFIchosen Then
            pBFINDay = lArgs.GetValue(BFInputNames.BFINDayScreen) '"BFINDay")
            pBFIUseSymbol = lArgs.GetValue(BFInputNames.BFIUseSymbol) '"BFIUseSymbol")
        End If
        pDidBFSeparation = True
    End Sub

    Private Sub mnuOutputASCII_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOutputASCII.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Dim lSpecification As String = ""
        If pDataGroup Is Nothing OrElse pDataGroup.Count = 0 Then
            Exit Sub
        End If
        If Not IO.Directory.Exists(txtOutputDir.Text.Trim()) Then
            Logger.Msg("Please specify an output directory", "Baseflow ASCII Output")
            txtOutputDir.Focus()
            Exit Sub
        End If
        If OutputFilenameRoot = "" Then
            Logger.Msg("Please specify a base output filename", "Baseflow ASCII Output")
            txtOutputRootName.Focus()
            Exit Sub
        Else
            SaveSetting("atcUSGSBaseflow", "Defaults", "BaseOutputFilename", OutputFilenameRoot)
        End If
        Logger.Dbg("Output ASCII")
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodPART", chkMethodPART.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPFixed", chkMethodHySEPFixed.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPLocMin", chkMethodHySEPLocMin.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPSlide", chkMethodHySEPSlide.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFIStandard", chkMethodBFIStandard.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFIModified", chkMethodBFIModified.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFLOW", chkMethodBFLOW.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodTwoPRDF", chkMethodTwoPRDF.Checked)
        'SaveSetting("atcUSGSBaseflow", "Defaults", "BFISymbols", chkBFISymbols.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "DFTwoParamEstMethod", pTwoParamEstimationMethod.ToString())

        OutputDir = txtOutputDir.Text.Trim()

        'The following ascii output steps are already done during the base-flow analysis process
        '
        'Logger.Status("Writing common ascii output files...") 
        'ASCIICommon(pDataGroup(0))
        'Logger.Status("Writing legacy ascii output files...")
        'For Each lMethod As BFMethods In MethodsLastDone
        '    Logger.Status("Writing legacy ascii output files for method: " & lMethod.ToString())
        '    ASCIIOriginal(pDataGroup(0), lMethod)
        'Next
        Logger.Status("Hide")

        'ToDo: persist the baseflow analysis results?
        'Dim lRDBWriter As New atcTimeseriesRDB()

        Logger.MsgCustomOwned("Baseflow output completed.", "USGS Base-Flow Separation", Me, New String() {"OK"})
    End Sub

    Private Sub txtOutputRootName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputRootName.TextChanged
        OutputFilenameRoot = txtOutputRootName.Text.Trim()
        OutputFilenameRoot = OutputFilenameRoot.Replace(" ", "_")
    End Sub

    Private Sub mnuGraphTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraphTimeseries.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraphTimeseries("Timeseries")
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Public Sub DoBFGraphTimeseries(ByVal aGraphType As String, Optional ByVal aPerUnitArea As Boolean = False, Optional ByVal aDataGroup As atcData.atcTimeseriesGroup = Nothing)
        'Dim lGraphPlugin As New atcGraph.atcGraphPlugin
        If pDataGroup Is Nothing OrElse pDataGroup.Count = 0 Then
            Exit Sub
        End If

        Dim lSeparateGraphs As Boolean = False
        Select Case MethodsLastDone.Count
            Case 0 : Exit Sub
            Case 1 : lSeparateGraphs = False
            Case Else
                lSeparateGraphs = (Logger.MsgCustomCheckbox("Create separate graphs or all on one graph?",
                                                            MethodsLastDone.Count & " methods selected",
                                                            "Do not ask again", "USGS GW Toolbox", "Baseflow Separation", "Separate Timeseries Graphs",
                                                            "Separate", "One Graph") = "Separate")
        End Select

        Dim lStart As Double = -99.9
        Dim lEnd As Double = -99.9
        Dim lDA As Double = -99.9

        Dim lTsDailyStreamflow As atcTimeseries = pDataGroup(0)

        Dim lTsGroupPart As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.PART, lStart, lEnd, lDA)
        Dim lTsGroupFixed As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.HySEPFixed, lStart, lEnd, lDA)
        Dim lTsGroupLocMin As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.HySEPLocMin, lStart, lEnd, lDA)
        Dim lTsGroupSlide As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.HySEPSlide, lStart, lEnd, lDA)
        Dim lTsGroupBFIStandard As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.BFIStandard, lStart, lEnd, lDA)
        Dim lTsGroupBFIModified As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.BFIModified, lStart, lEnd, lDA)
        Dim lTsGroupBFLOW As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.BFLOW, lStart, lEnd, lDA)
        Dim lTsGroupTwoPRDF As atcCollection = ConstructGraphTsGroup(lTsDailyStreamflow, BFMethods.TwoPRDF, lStart, lEnd, lDA)

        Dim lYAxisTitleText As String = "FLOW, IN CUBIC FEET PER SECOND"
        If aPerUnitArea Then lYAxisTitleText &= " (per unit square mile)"
        If aGraphType = "CDist" Then lYAxisTitleText = "Flow (in)"
        Dim lTsFlow As atcTimeseries = SubsetByDate(lTsDailyStreamflow, lStart, lEnd, Nothing)
        lTsFlow.Attributes.SetValue("Units", lYAxisTitleText)
        lTsFlow.Attributes.SetValue("YAxis", "LEFT")

        Dim lArgs As New atcDataAttributes
        With lArgs
            .SetValue("Drainage Area", lDA)
            .SetValue("PerUnitArea", aPerUnitArea)
            .SetValue("GraphType", aGraphType)
            .SetValue("StreamFlow", lTsFlow)
            .SetValue("YAxisTitleText", lYAxisTitleText)
            .SetValue("Method", "")
        End With

        Dim lTsGraphAll As atcTimeseriesGroup
        If lSeparateGraphs Then
            Dim lTsBF4Graph As atcTimeseries = Nothing
            If lTsGroupPart.Count > 0 Then
                lArgs.SetValue("Method", "Part")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupPart, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
            If lTsGroupFixed.Count > 0 Then
                lArgs.SetValue("Method", "Fixed")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupFixed, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
            If lTsGroupLocMin.Count > 0 Then
                lArgs.SetValue("Method", "LocMin")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupLocMin, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If

            End If
            If lTsGroupSlide.Count > 0 Then
                lArgs.SetValue("Method", "Slide")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupSlide, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If

            If lTsGroupBFIStandard.Count > 0 Then
                lArgs.SetValue("Method", "BFIStandard")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupBFIStandard, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
            If lTsGroupBFIModified.Count > 0 Then
                lArgs.SetValue("Method", "BFIModified")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupBFIModified, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
            If lTsGroupBFLOW.Count > 0 Then
                lArgs.SetValue("Method", "BFLOW")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupBFLOW, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
            If lTsGroupTwoPRDF.Count > 0 Then
                lArgs.SetValue("Method", "TwoPRDF")
                Dim lDataGroup As atcTimeseriesGroup = SetupGraphTsGroup(lTsGroupTwoPRDF, lArgs)
                If aGraphType = "Timeseries" Then
                    DisplayTsGraph(lDataGroup)
                ElseIf aGraphType = "Duration" Then
                    DisplayDurGraph(lDataGroup, aPerUnitArea)
                ElseIf aGraphType = "CDist" Then
                    DisplayCDistGraph(lDataGroup)
                End If
            End If
        Else 'all in one graph
            lTsGraphAll = New atcTimeseriesGroup

            Dim lTsGroupPart1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupFixed1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupLocMin1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupSlide1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupBFIStandard1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupBFIModified1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupBFLOW1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupTwoPRDF1 As atcTimeseriesGroup = Nothing
            Dim lTsGroupStock As New atcTimeseriesGroup
            If lTsGroupPart.Count > 0 Then
                lArgs.SetValue("Method", "Part")
                lTsGroupPart1 = SetupGraphTsGroup(lTsGroupPart, lArgs)
                lTsGroupStock.AddRange(lTsGroupPart1)
                'lTsTemp = lTsGroupPart.ItemByKey("RateDaily").Clone()
                'With lTsTemp.Attributes
                '    .SetValue("Constituent", "Baseflow")
                '    .SetValue("Scenario", "Estimated by Part")
                '    .SetValue("Units", lYAxisTitleText)
                '    .SetValue("YAxis", "LEFT")
                'End With
                'lTsGraphAll.Add(lTsTemp)
                'If aGraphType = "Duration" Then
                '    lTsROPart = lTsFlow - lTsTemp
                '    With lTsROPart.Attributes
                '        .SetValue("Constituent", "Runoff")
                '        .SetValue("Scenario", "Estimated by Part")
                '        .SetValue("Units", lYAxisTitleText)
                '        .SetValue("YAxis", "LEFT")
                '    End With
                'End If
            End If
            If lTsGroupFixed.Count > 0 Then
                lArgs.SetValue("Method", "Fixed")
                lTsGroupFixed1 = SetupGraphTsGroup(lTsGroupFixed, lArgs)
                lTsGroupStock.AddRange(lTsGroupFixed1)
            End If
            If lTsGroupLocMin.Count > 0 Then
                lArgs.SetValue("Method", "LocMin")
                lTsGroupLocMin1 = SetupGraphTsGroup(lTsGroupLocMin, lArgs)
                lTsGroupStock.AddRange(lTsGroupLocMin1)
            End If
            If lTsGroupSlide.Count > 0 Then
                lArgs.SetValue("Method", "Slide")
                lTsGroupSlide1 = SetupGraphTsGroup(lTsGroupSlide, lArgs)
                lTsGroupStock.AddRange(lTsGroupSlide1)
            End If
            If lTsGroupBFIStandard.Count > 0 Then
                lArgs.SetValue("Method", "BFIStandard")
                lTsGroupBFIStandard1 = SetupGraphTsGroup(lTsGroupBFIStandard, lArgs)
                lTsGroupStock.AddRange(lTsGroupBFIStandard1)
            End If
            If lTsGroupBFIModified.Count > 0 Then
                lArgs.SetValue("Method", "BFIModified")
                lTsGroupBFIModified1 = SetupGraphTsGroup(lTsGroupBFIModified, lArgs)
                lTsGroupStock.AddRange(lTsGroupBFIModified1)
            End If
            If lTsGroupBFLOW.Count > 0 Then
                lArgs.SetValue("Method", "BFLOW")
                lTsGroupBFLOW1 = SetupGraphTsGroup(lTsGroupBFLOW, lArgs)
                lTsGroupStock.AddRange(lTsGroupBFLOW1)
            End If
            If lTsGroupTwoPRDF.Count > 0 Then
                lArgs.SetValue("Method", "TwoPRDF")
                lTsGroupTwoPRDF1 = SetupGraphTsGroup(lTsGroupTwoPRDF, lArgs)
                lTsGroupStock.AddRange(lTsGroupTwoPRDF1)
            End If

            'Originally thought would treat CDist graph differently from other type of graphs
            'If aGraphType = "CDist" Then
            '    lTsGraphAll.Add(lTsGroupStock.FindData("Constituent", "Streamflow")(0))
            'Else
            '    'lTsGraphAll.Add(lTsFlow)
            '    lTsGraphAll.Add(lTsGroupStock.FindData("Constituent", "Streamflow")(0))
            'End If

            lTsGraphAll.Add(lTsGroupStock.FindData("Constituent", "Streamflow")(0))
            If lTsGraphAll.Count = 0 Then 'didn't find Streamflow, then try finding 'FLOW'
                lTsGraphAll.Add(lTsGroupStock.FindData("Constituent", "FLOW")(0))
            End If

            Dim lTsGroupBf As atcTimeseriesGroup = lTsGroupStock.FindData("Constituent", "Baseflow")
            lTsGraphAll.AddRange(lTsGroupBf)
            If aGraphType = "Timeseries" Then
                DisplayTsGraph(lTsGraphAll)
            ElseIf aGraphType = "Duration" Then
                Dim lTsGroupRO As atcTimeseriesGroup = lTsGroupStock.FindData("Constituent", "Runoff")
                lTsGraphAll.AddRange(lTsGroupRO)
                DisplayDurGraph(lTsGraphAll, aPerUnitArea)
            ElseIf aGraphType = "CDist" Then
                Dim lTsGroupRO As atcTimeseriesGroup = lTsGroupStock.FindData("Constituent", "Runoff")
                lTsGraphAll.AddRange(lTsGroupRO)
                DisplayCDistGraph(lTsGraphAll)
            End If
        End If

        'lDataGroup.Clear()
        'lDataGroup = Nothing
    End Sub

    Private Function SetupGraphTsGroup(ByVal aTsCollection As atcCollection, ByVal aParams As atcDataAttributes) As atcTimeseriesGroup
        Dim lDA As Double = aParams.GetValue("Drainage Area")
        Dim lPerUnitArea As Boolean = aParams.GetValue("PerUnitArea")
        If lPerUnitArea AndAlso (Not (lDA > 0) OrElse Double.IsNaN(lDA)) Then
            lDA = 1.0
            Logger.Msg("Drainage area is not positive, it is set to 1.", MsgBoxStyle.Information, "Setup Graph Time Series Group")
        End If
        Dim lGraphType As String = aParams.GetValue("GraphType")
        Dim lTsFlow As atcTimeseries = aParams.GetValue("StreamFlow").Clone()
        Dim lYAxisTitleText As String = aParams.GetValue("YAxisTitleText")
        Dim lMethod As String = aParams.GetValue("Method")
        Dim lMethodAtt As BFMethods
        Select Case lMethod.ToLower
            Case "part" : lMethodAtt = BFMethods.PART
            Case "fixed" : lMethodAtt = BFMethods.HySEPFixed
            Case "locmin" : lMethodAtt = BFMethods.HySEPLocMin
            Case "slide" : lMethodAtt = BFMethods.HySEPSlide
            Case "bfistandard" : lMethodAtt = BFMethods.BFIStandard
            Case "bfimodified" : lMethodAtt = BFMethods.BFIModified
            Case "bflow" : lMethodAtt = BFMethods.BFLOW
            Case "twoprdf" : lMethodAtt = BFMethods.TwoPRDF
        End Select
        Dim lBFTser As atcTimeseries = aTsCollection.ItemByKey("RateDaily")
        Dim lTsBF4Graph As atcTimeseries = Nothing
        If lMethod.ToLower().Contains("bfi") Then
            Dim lStartOfGoodValue As Integer
            For I As Integer = 0 To lBFTser.numValues - 1
                If lBFTser.Value(I + 1) > 0 Then
                    lStartOfGoodValue = lBFTser.Dates.Value(I)
                    Exit For
                End If
            Next
            If lStartOfGoodValue > lBFTser.Dates.Value(0) Then
                lTsBF4Graph = SubsetByDate(lBFTser, lStartOfGoodValue, lBFTser.Dates.Value(lBFTser.numValues), Nothing)
            ElseIf lTsBF4Graph Is Nothing Then
                lTsBF4Graph = lBFTser.Clone()
            End If
        Else
            lTsBF4Graph = lBFTser.Clone()
        End If

        lDA = lTsBF4Graph.Attributes.GetValue("Drainage Area")
        With lTsBF4Graph.Attributes
            .SetValue("Constituent", "Baseflow")
            .SetValue("Scenario", "Estimated by " & lMethod)
            .SetValue("Units", lYAxisTitleText)
            .SetValue("YAxis", "LEFT")
        End With
        Dim lDataGroup As New atcData.atcTimeseriesGroup
        Dim lTsRunoff As atcTimeseries = Nothing
        If lGraphType = "Duration" Then
            lTsRunoff = Nothing 'lTsFlow - lTsBF4Graph
            If lMethod.ToLower().Contains("bfi") Then
                If lTsFlow.Dates.Value(0) < lTsBF4Graph.Dates.Value(0) Then
                    Dim lTsFlowSubs As atcTimeseries = SubsetByDate(lTsFlow, lTsBF4Graph.Dates.Value(0), lTsBF4Graph.Dates.Value(lTsBF4Graph.numValues), Nothing)
                    lTsRunoff = lTsFlowSubs - lTsBF4Graph
                    lTsFlowSubs.Clear()
                Else
                    lTsRunoff = lTsFlow - lTsBF4Graph
                End If
            Else
                lTsRunoff = lTsFlow - lTsBF4Graph
            End If

            If lPerUnitArea Then
                lTsFlow = lTsFlow / lDA
                lTsBF4Graph = lTsBF4Graph / lDA
                If lTsRunoff IsNot Nothing Then
                    lTsRunoff = lTsRunoff / lDA
                End If
            End If
            With lTsRunoff.Attributes
                .SetValue("Constituent", "Runoff")
                .SetValue("Scenario", "Estimated by " & lMethod)
                .SetValue("Units", lYAxisTitleText)
                .SetValue("YAxis", "LEFT")
                .SetValue("Method", lMethodAtt)
            End With
        End If
        If lGraphType = "CDist" Then
            'Dim lTimeUnitToAggregate As atcTimeUnit = atcTimeUnit.TUMonth
            lTsRunoff = Nothing
            If lMethod.ToLower().Contains("bfi") Then
                If lTsFlow.Dates.Value(0) < lTsBF4Graph.Dates.Value(0) Then
                    Dim lTsFlowSubs As atcTimeseries = SubsetByDate(lTsFlow, lTsBF4Graph.Dates.Value(0), lTsBF4Graph.Dates.Value(lTsBF4Graph.numValues), Nothing)
                    lTsRunoff = lTsFlowSubs - lTsBF4Graph
                    lTsFlowSubs.Clear()
                Else
                    lTsRunoff = lTsFlow - lTsBF4Graph
                End If
            Else
                lTsRunoff = lTsFlow - lTsBF4Graph
            End If

            'Convert to depth inch
            Dim lConversionFactor As Double = 0.03719 / lDA

            'Dim lGraphTsFlow As atcTimeseries = Aggregate(lTsFlow, lTimeUnitToAggregate, 1, atcTran.TranSumDiv)
            'Dim lGraphTsBF As atcTimeseries = Aggregate(lTsBF4Graph, lTimeUnitToAggregate, 1, atcTran.TranSumDiv)
            'Dim lGraphTsRunoff As atcTimeseries = Aggregate(lTsRunoff, lTimeUnitToAggregate, 1, atcTran.TranSumDiv)

            Dim lGraphTsFlow As atcTimeseries = lTsFlow.Clone()
            Dim lGraphTsBF As atcTimeseries = lTsBF4Graph.Clone()
            Dim lGraphTsRunoff As atcTimeseries = lTsRunoff.Clone()

            Dim lGraphTsFlowIn As atcTimeseries = lGraphTsFlow * lConversionFactor
            With lGraphTsFlowIn.Attributes
                '.SetValue("Constituent", "Streamflow")
                '.SetValue("Scenario", "Observed")
                .SetValue("Units", lYAxisTitleText)
                .SetValue("YAxis", "LEFT")
            End With

            Dim lGraphTsBFIn As atcTimeseries = lGraphTsBF * lConversionFactor
            With lGraphTsBFIn.Attributes
                .SetValue("Constituent", "Baseflow")
                .SetValue("Scenario", "Estimated by " & lMethod)
                .SetValue("Units", lYAxisTitleText)
                .SetValue("YAxis", "LEFT")
            End With

            Dim lGraphTsRunoffIn As atcTimeseries = lGraphTsRunoff * lConversionFactor
            With lGraphTsRunoffIn.Attributes
                .SetValue("Constituent", "Runoff")
                .SetValue("Scenario", "Estimated by " & lMethod)
                .SetValue("Units", lYAxisTitleText)
                .SetValue("YAxis", "LEFT")
                .SetValue("Method", lMethodAtt)
            End With

            lDataGroup.Add(lGraphTsFlowIn)
            lDataGroup.Add(lGraphTsBFIn)
            lDataGroup.Add(lGraphTsRunoffIn)
        Else
            lDataGroup.Add(lTsFlow)
            lDataGroup.Add(lTsBF4Graph)
            If lTsRunoff IsNot Nothing Then lDataGroup.Add(lTsRunoff)
        End If

        Return lDataGroup
    End Function

    Private Sub DisplayTsGraph(ByVal aDataGroup As atcTimeseriesGroup)
        'Make sure graph can't find provisional attribute
        For Each lTs As atcTimeseries In aDataGroup
            lTs.Attributes.SetValue("ProvisionalValueAttribute", "X" & lTs.Attributes.GetValue("ProvisionalValueAttribute", ""))
        Next
        Dim lGraphForm As New atcGraph.atcGraphForm()
        lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphTime(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            .YAxis.Type = AxisType.Log
            'Dim lScaleMin As Double = 1
            '.YAxis.Scale.Min = lScaleMin
            .AxisChange()
            .CurveList.Item(0).Color = Drawing.Color.Red
            If aDataGroup.Count > 2 Then
                CType(.CurveList.Item(0), LineItem).Line.Width = 3
                For I As Integer = 1 To aDataGroup.Count - 1
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                Next
            Else
                .CurveList.Item(1).Color = Drawing.Color.DarkBlue
                CType(.CurveList.Item(1), LineItem).Line.Width = 2
            End If
        End With
        lGraphForm.Show()
        'Restore provisional attribute after graphing
        For Each lTs As atcTimeseries In aDataGroup
            lTs.Attributes.SetValue("ProvisionalValueAttribute", lTs.Attributes.GetValue("ProvisionalValueAttribute", "").ToString.Substring(1))
        Next
    End Sub

    Private Sub mnuGraphDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraphDuration.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim lPerUnitArea As Boolean = False
        Dim lResponse As String = Logger.MsgCustomOwned("Calculate exceedance probability per unit drainage area?",
                                                        "Per Unit Area Plot", Me,
                                                        New String() {"Yes", "No"})
        If lResponse = "Yes" Then
            lPerUnitArea = True
        End If
        DoBFGraphTimeseries("Duration", lPerUnitArea)
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Function GetCurveColor(ByVal aTs As atcTimeseries) As System.Drawing.Color
        Dim lCons As String = aTs.Attributes.GetValue("Constituent").ToString.ToLower
        Dim lMethod As BFMethods = aTs.Attributes.GetValue("Method")

        Select Case lMethod
            Case BFMethods.PART
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkBlue
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.LightBlue
                End If
            Case BFMethods.HySEPFixed
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkGreen
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.LightGreen
                End If
            Case BFMethods.HySEPLocMin
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkOrange
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.Orange
                End If
            Case BFMethods.HySEPSlide
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.Maroon
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.Magenta
                End If
            Case BFMethods.BFIStandard
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkCyan
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.Cyan
                End If
            Case BFMethods.BFIModified
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.Yellow
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.YellowGreen
                End If
            Case BFMethods.BFLOW
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkMagenta
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.Magenta
                End If
            Case BFMethods.TwoPRDF
                If lCons.StartsWith("baseflow") Then
                    Return Drawing.Color.DarkGoldenrod
                ElseIf lCons.StartsWith("runoff") Then
                    Return Drawing.Color.Goldenrod
                End If
        End Select
    End Function

    Private Sub DisplayDurGraph(ByVal aDataGroup As atcTimeseriesGroup, ByVal aPerUnitArea As Boolean)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphProbability(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            .YAxis.Scale.MinAuto = False
            Dim lScaleMin As Double = 10
            If pDataGroup IsNot Nothing AndAlso pDataGroup.Count > 0 Then
                Dim lmeanVal As Double = pDataGroup(0).Attributes.GetValue("Mean")
                If lmeanVal < lScaleMin Then
                    lScaleMin = 1.0
                End If
            End If
            If aPerUnitArea Then lScaleMin = 0.005
            .YAxis.Scale.Min = lScaleMin
            .AxisChange()
            .CurveList.Item(0).Color = Drawing.Color.Red

            If aDataGroup.Count > 3 Then
                'this scheme assume the order in which ts are added is as follow:
                'streamflow, all bf, then all ro
                Dim lBFCurveCount As Integer = (aDataGroup.Count - 1) / 2
                Dim lBFInitColor As Integer = Drawing.Color.DarkBlue.ToArgb
                For I As Integer = 1 To lBFCurveCount
                    '.CurveList.Item(I).Color = System.Drawing.Color.FromArgb(lBFInitColor - (I - 1) * 8)
                    'CType(.CurveList.Item(I), LineItem).Line.Style = Drawing.Drawing2D.DashStyle.Dash
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                Next
                lBFInitColor = Drawing.Color.DarkCyan.ToArgb
                For I As Integer = lBFCurveCount + 1 To aDataGroup.Count - 1
                    '.CurveList.Item(I).Color = System.Drawing.Color.FromArgb(lBFInitColor - (I - 1) * 8)
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                Next
            Else
                .CurveList.Item(1).Color = Drawing.Color.DarkBlue
                'CType(.CurveList.Item(1), LineItem).Line.Width = 2
                .CurveList.Item(2).Color = Drawing.Color.Cyan
            End If
            With .Legend.FontSpec
                .IsBold = False
                .Border.IsVisible = False
                .Size = 12
            End With
            .XAxis.Title.Text = "PERCENTAGE OF TIME FLOW WAS EQUALED OR EXCEEDED"
        End With
        lGraphForm.Grapher.ZedGraphCtrl.Refresh()
        lGraphForm.Show()

    End Sub

    Private Sub txtOutputDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOutputDir.Click
        Dim lDir As String = ""
        If IO.Directory.Exists(txtOutputDir.Text.Trim()) Then
            lDir = txtOutputDir.Text.Trim()
        ElseIf IO.Directory.Exists(pOutputDir) Then
            lDir = pOutputDir
        End If
        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        With FolderBrowserDialog1
            ' Desktop is the root folder in the dialog.
            .RootFolder = Environment.SpecialFolder.Desktop
            ' Select directory on entry.
            .SelectedPath = lDir
            ' Prompt the user with a custom message.
            .Description = "Specify Base-Flow ASCII output directory"
            If .ShowDialog = DialogResult.OK Then
                ' Display the selected folder if the user clicked on the OK button.
                lDir = .SelectedPath
            End If
        End With
        If IO.Directory.Exists(lDir) Then
            txtOutputDir.Text = lDir
            pOutputDir = lDir
            SaveSetting("atcUSGSBaseflow", "Defaults", "OutputDir", pOutputDir)
        End If
    End Sub

    Private Sub frmMain_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodPART", chkMethodPART.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPFixed", chkMethodHySEPFixed.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPLocMin", chkMethodHySEPLocMin.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPSlide", chkMethodHySEPSlide.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFIStandard", chkMethodBFIStandard.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFIModified", chkMethodBFIModified.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodBFLOW", chkMethodBFLOW.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "MethodTwoPRDF", chkMethodTwoPRDF.Checked)
        'SaveSetting("atcUSGSBaseflow", "Defaults", "BFISymbols", chkBFISymbols.Checked)
        SaveSetting("atcUSGSBaseflow", "Defaults", "DFTwoParamEstMethod", pTwoParamEstimationMethod.ToString())

        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
            pDataGroup = Nothing
        End If
        Opened = False
    End Sub

    Private Sub frmUSGSBaseflow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details/Analysis/GW Toolbox Hydrograph Analysis/Base-Flow Separation.html")
        End If
    End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        ShowHelp("BASINS Details/Analysis/GW Toolbox Hydrograph Analysis/Base-Flow Separation.html")
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Opened = True
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodPART", "False") = "True" Then
            chkMethodPART.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPFixed", "False") = "True" Then
            chkMethodHySEPFixed.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPLocMin", "False") = "True" Then
            chkMethodHySEPLocMin.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPSlide", "False") = "True" Then
            chkMethodHySEPSlide.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodBFIStandard", "False") = "True" Then
            chkMethodBFIStandard.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodBFIModified", "False") = "True" Then
            chkMethodBFIModified.Checked = True
        End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "BFISymbols", "False") = "True" Then
        '    chkBFISymbols.Checked = True
        'End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodBFLOW", "False") = "True" Then
            chkMethodBFLOW.Checked = True
        End If
        If GetSetting("atcUSGSBaseflow", "Defaults", "MethodTwoPRDF", "False") = "True" Then
            chkMethodTwoPRDF.Checked = True
            Dim lparamEstMethod As String = GetSetting("atcUSGSBaseflow", "Defaults", "DFTwoParamEstMethod", clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM)
            Select Case lparamEstMethod
                Case clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT.ToString(), "NONE", "None", "none"
                    rdo2PDefault.Checked = True
                Case clsBaseflow2PRDF.ETWOPARAMESTIMATION.CF.ToString()
                    'mnuDFTwoParamCF.PerformClick()
                Case clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM.ToString()
                    rdo2PSpecify.Checked = True
            End Select
        End If
        If chkMethodBFIStandard.Checked OrElse chkMethodBFIModified.Checked Then
            gbBFI.Enabled = True
        Else
            gbBFI.Enabled = False
        End If
        BFMethods_CheckedChanged(Nothing, Nothing)
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", pDataGroup)
        If pDataGroup.Count > 0 Then
            Me.Initialize(pDataGroup, pBasicAttributes, True)
        Else
            Logger.Msg("Need to select at least one daily streamflow dataset", "USGS Base-Flow Separation")
        End If
    End Sub

    Private Sub mnuGraphCDistPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraphCDistPlot.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraphTimeseries("CDist")
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub DisplayCDistGraph(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphProbability(aDataGroup, lZgc)
        lGraphTS.Exceedance = False
        lGraphForm.Grapher = lGraphTS
        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            .YAxis.Type = AxisType.Linear
            'Dim lScaleMin As Double = 10
            '.YAxis.Scale.MinAuto = False
            '.YAxis.Scale.Min = lScaleMin
            '.YAxis.Scale.Max = 15
            .AxisChange()
            .CurveList.Item(0).Color = Drawing.Color.Red
            With CType(.CurveList.Item(0), LineItem).Symbol
                .Type = SymbolType.Triangle
                .IsVisible = True
            End With

            If aDataGroup.Count > 3 Then
                'this scheme assume the order in which ts are added is as follow:
                'streamflow, all bf, then all ro
                Dim lBFCurveCount As Integer = (aDataGroup.Count - 1) / 2
                Dim lBFInitColor As Integer = Drawing.Color.DarkBlue.ToArgb
                For I As Integer = 1 To lBFCurveCount
                    '.CurveList.Item(I).Color = System.Drawing.Color.FromArgb(lBFInitColor - (I - 1) * 8)
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                    'With CType(.CurveList.Item(I), LineItem).Symbol
                    '    .Type = SymbolType.Circle
                    '    .IsVisible = True
                    'End With
                Next
                lBFInitColor = Drawing.Color.DarkCyan.ToArgb
                For I As Integer = lBFCurveCount + 1 To aDataGroup.Count - 1
                    '.CurveList.Item(I).Color = System.Drawing.Color.FromArgb(lBFInitColor - (I - 1) * 8)
                    .CurveList.Item(I).Color = GetCurveColor(aDataGroup(I))
                    'With CType(.CurveList.Item(I), LineItem).Symbol
                    '    .Type = SymbolType.Square
                    '    .IsVisible = True
                    'End With
                Next

            Else
                .CurveList.Item(1).Color = Drawing.Color.DarkBlue
                With CType(.CurveList.Item(1), LineItem).Symbol
                    .Type = SymbolType.Circle
                    .IsVisible = True
                End With

                .CurveList.Item(2).Color = Drawing.Color.DarkCyan
                'With CType(.CurveList.Item(2), LineItem).Symbol
                '    .Type = SymbolType.Square
                '    .IsVisible = True
                'End With
            End If

            With .Legend.FontSpec
                .IsBold = False
                .Border.IsVisible = False
                .Size = 12
            End With
            .XAxis.Title.Text = "PERCENTAGE OF TIME FLOW WAS LESS THAN " & vbCrLf & "OR EQUAL TO INDICATED VALUE"
        End With
        lGraphForm.Grapher.ZedGraphCtrl.Refresh()
        lGraphForm.Show()
    End Sub
    Private Sub btnWriteASCIIOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteASCIIOutput.Click
        mnuOutputASCII_Click(Nothing, Nothing)
    End Sub

    Private Sub btnGraphTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphTimeseries.Click
        mnuGraphTimeseries_Click(Nothing, Nothing)
    End Sub

    Private Sub btnGraphDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphDuration.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraphTimeseries("Duration", False)
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnGraphDurationPUA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphDurationPUA.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraphTimeseries("Duration", True)
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub btnGraphCDist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphCDist.Click
        If Not pDidBFSeparation Then
            ComputeBaseflow()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraphTimeseries("CDist")
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub BFMethods_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMethodHySEPFixed.CheckedChanged,
                                                                                                             chkMethodHySEPLocMin.CheckedChanged,
                                                                                                             chkMethodHySEPSlide.CheckedChanged,
                                                                                                             chkMethodPART.CheckedChanged,
                                                                                                             chkMethodBFIStandard.CheckedChanged,
                                                                                                             chkMethodBFIModified.CheckedChanged,
                                                                                                             chkMethodBFLOW.CheckedChanged,
                                                                                                             chkMethodTwoPRDF.CheckedChanged
        pDidBFSeparation = False
        pMethods.Clear()
        If Opened Then
            If chkMethodPART.Checked Then pMethods.Add(BFMethods.PART)
            If chkMethodHySEPFixed.Checked Then pMethods.Add(BFMethods.HySEPFixed)
            If chkMethodHySEPLocMin.Checked Then pMethods.Add(BFMethods.HySEPLocMin)
            If chkMethodHySEPSlide.Checked Then pMethods.Add(BFMethods.HySEPSlide)
            If chkMethodBFIStandard.Checked Then pMethods.Add(BFMethods.BFIStandard)
            If chkMethodBFIModified.Checked Then pMethods.Add(BFMethods.BFIModified)
            If chkMethodBFLOW.Checked Then pMethods.Add(BFMethods.BFLOW)
            If chkMethodTwoPRDF.Checked Then pMethods.Add(BFMethods.TwoPRDF)

            Dim lBFIChosen As Boolean = False
            If chkMethodBFIStandard.Checked And chkMethodBFIModified.Checked Then
                lblF.Visible = True
                txtF.Visible = True
                lblK.Visible = True
                txtK.Visible = True
                lBFIChosen = True
            ElseIf chkMethodBFIStandard.Checked Then
                lblF.Visible = True
                txtF.Visible = True
                lblK.Visible = False
                txtK.Visible = False
                lBFIChosen = True
            ElseIf chkMethodBFIModified.Checked Then
                lblF.Visible = False
                txtF.Visible = False
                lblK.Visible = True
                txtK.Visible = True
                lBFIChosen = True
            Else 'none is checked
                lblF.Visible = False
                txtF.Visible = False
                lblK.Visible = False
                txtK.Visible = False
            End If
            txtDFParamBeta.Enabled = chkMethodBFLOW.Checked
            lblBeta.Enabled = chkMethodBFLOW.Checked

            If chkMethodTwoPRDF.Checked Then
                'mnuDFTwoParam.Enabled = True
                txtDFParamRC.Enabled = True
                txtDFParamBFImax.Enabled = True
                lblRC.Enabled = True
                lblBFImax.Enabled = True
            Else
                'mnuDFTwoParam.Enabled = False
                txtDFParamRC.Enabled = False
                txtDFParamBFImax.Enabled = False
                lblRC.Enabled = False
                lblBFImax.Enabled = False
            End If
            If lBFIChosen Then
                gbBFI.Enabled = True
            Else
                gbBFI.Enabled = False
            End If
        End If
    End Sub

    Private Sub txtAny_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles txtDrainageArea.TextChanged, txtStartDateUser.TextChanged, txtEndDateUser.TextChanged,
            txtN.TextChanged, txtF.TextChanged, txtK.TextChanged, txtOutputDir.TextChanged, txtOutputRootName.TextChanged
        pDidBFSeparation = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not pDidBFSeparation Then Return
        Dim lBFTSGroup As atcTimeseriesGroup = pDataGroup(0).Attributes.GetDefinedValue("Baseflow").Value

        Dim lsaveDataGroup As atcDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", lBFTSGroup, lBFTSGroup)
        If lsaveDataGroup.Count > 0 Then
            Dim lTsGroupRemove As New atcTimeseriesGroup()
            For Each lTS As atcTimeseries In lsaveDataGroup
                If lTS.Attributes.GetValue("time unit") <> atcTimeUnit.TUDay Then
                    lTsGroupRemove.Add(lTS)
                End If
            Next
            For Each lTs As atcTimeseries In lTsGroupRemove
                lsaveDataGroup.Remove(lTs)
            Next
            Dim lPath As String = txtOutputDir.Text.Trim()
            Dim lRootName As String = txtOutputRootName.Text.Trim()
            Dim lRDBFilenameTemplate As String = IO.Path.Combine(lPath, lRootName & ".rdb")
            If Not IO.Directory.Exists(lPath) OrElse String.IsNullOrEmpty(lRootName) Then
                lPath = "C:\"
                lRDBFilenameTemplate = "C:\z.RDB"
            End If
            Dim lsaveRDB As New atcTimeseriesRDB.atcTimeseriesRDB()
            lsaveRDB.DataSets.AddRange(lsaveDataGroup)
            lsaveRDB.Save(lRDBFilenameTemplate, atcDataSource.EnumExistAction.ExistReplace)
            Logger.Msg("Timeseries are saved in RDB format in folder: " & vbCrLf & lPath, "Save Base-Flow Timeseries Data")
        Else
            Logger.Msg("Need to select at least one daily streamflow dataset", "USGS Base-Flow Separation")
        End If
    End Sub

    Private Sub rdo2P_CheckedChanged(sender As Object, e As EventArgs) Handles rdo2PSpecify.CheckedChanged,
                                                                               rdo2PDefault.CheckedChanged
        If rdo2PSpecify.Checked Then
            txt2PDefaultNotice.Visible = False
            lblBFImax.Enabled = True
            lblRC.Enabled = True
            txtDFParamRC.Enabled = True
            txtDFParamBFImax.Enabled = True
            pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM
        Else
            txt2PDefaultNotice.Visible = True
            lblBFImax.Enabled = False
            lblRC.Enabled = False
            txtDFParamRC.Enabled = False
            txtDFParamBFImax.Enabled = False
            pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT
        End If
    End Sub
End Class