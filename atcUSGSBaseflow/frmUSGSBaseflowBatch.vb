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

Public Class frmUSGSBaseflowBatch
    Private pName As String = "Unnamed"
    Private pBasicAttributes As atcDataAttributes
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

    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private WithEvents pfrmStations As frmStations

    Public Opened As Boolean = False
    Private pDidBFSeparation As Boolean = False

    Private pSetGlobal As Boolean = False
    Private pTwoParamEstimationMethod As clsBaseflow2PRDF.ETWOPARAMESTIMATION = clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE

    Public Event ParametersSet(ByVal aArgs As atcDataAttributes)

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                      Optional ByVal aBasicAttributes As atcDataAttributes = Nothing, _
                      Optional ByVal aShowForm As Boolean = True)
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        pBasicAttributes = aBasicAttributes
        If pBasicAttributes Is Nothing Then
            pBasicAttributes = New atcDataAttributes()
        End If
        pMethods = New ArrayList()
        MethodsLastDone = New ArrayList()
        If aShowForm Then
            'No need to load any menu items
        End If

        Dim loperation As String = pBasicAttributes.GetValue("Operation", "")
        If loperation.ToLower = "globalsetparm" Then
            pSetGlobal = True
        End If
        If pDataGroup.Count = 0 AndAlso Not pSetGlobal Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", _
                                                       pDataGroup, Nothing, True, True, Me.Icon)
        End If

        If pDataGroup.Count > 0 OrElse pSetGlobal Then
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

        'pOutputDir = GetSetting("atcUSGSBaseflowBatch", "Defaults", "OutputDir", "")
        'OutputFilenameRoot = GetSetting("atcUSGSBaseflowBatch", "Defaults", "BaseOutputFilename", "")

        'atcUSGSStations.StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()
        Dim lRow As Integer = 1
        Dim lDates(5) As Integer
        Dim lDatesUser(5) As Integer

        If Not pSetGlobal Then
            btnExamineData.Visible = True
            btnPlotDur.Visible = True
            Dim lStations As ArrayList = pBasicAttributes.GetValue("StationInfo", Nothing)
            For Each lDataset As atcData.atcTimeseries In pDataGroup
                If lDataset.Dates.numValues > 0 Then
                    Dim lThisDate As Double = lDataset.Dates.Value(0)
                    If lThisDate < lFirstDate Then lFirstDate = lThisDate
                    If lThisDate > pCommonStart Then pCommonStart = lThisDate
                    lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                    If lThisDate > lLastDate Then lLastDate = lThisDate
                    If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
                End If
                Dim loc As String = lDataset.Attributes.GetValue("Location", "")
                Dim lDA As String = lDataset.Attributes.GetValue("Drainage Area", "")
                If lDA = "" AndAlso lStations IsNot Nothing Then
                    For Each lStation As String In lStations
                        If lStation.Contains(loc) Then
                            Dim larr() As String = lStation.Split(",")
                            If larr.Length >= 2 Then
                                Dim lDAdouble As Double = 0
                                If Double.TryParse(larr(1), lDAdouble) Then
                                    lDA = lDAdouble.ToString()
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                Dim lPath As String = lDataset.Attributes.GetValue("History 1", "").ToString.Substring("History 1 ".Length)
                With grdStations
                    .Rows.Add(New String() {loc, lDA, lPath})
                    '.Item(0, lRow).Value = loc
                    '.Item(1, lRow).Value = lDA
                    '.Item(2, lRow).Value = lPath
                End With
                lRow += 1
            Next
            grdStations.Visible = True
            btnWriteASCIIOutput.Text = "Set Parameters: " & pBasicAttributes.GetValue("Group", "")
        Else
            btnExamineData.Visible = False
            btnPlotDur.Visible = False
            grdStations.Visible = False
            btnWriteASCIIOutput.Text = "Set Parameters: Global Defaults"
            pCommonStart = pBasicAttributes.GetValue("SJDATECommon", 0)
            pCommonEnd = pBasicAttributes.GetValue("EJDATECommon", 0)
            Dim lStartDate As Double = pBasicAttributes.GetValue("SJDATE", 0)
            Dim lEndDate As Double = pBasicAttributes.GetValue("EJDATE", 0)
            Dim lStartDateUser As Double = pBasicAttributes.GetValue("StartDate", 0)
            Dim lEndDateUser As Double = pBasicAttributes.GetValue("EndDate", 0)
            J2Date(lStartDate, lDates)
            If lStartDate > 0 Then txtDataStart.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
            J2Date(lStartDateUser, lDatesUser)
            If lStartDateUser > 0 Then
                txtStartDateUser.Text = lDatesUser(0) & "/" & lDatesUser(1) & "/" & lDatesUser(2)
            ElseIf lStartDate > 0 Then
                txtStartDateUser.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
            End If
            J2Date(lEndDate, lDates)
            If lEndDate > 0 Then txtDataEnd.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
            J2Date(lEndDateUser, lDatesUser)
            If lEndDateUser > 0 Then
                txtEndDateUser.Text = lDatesUser(0) & "/" & lDatesUser(1) & "/" & lDatesUser(2)
            ElseIf lEndDate > 0 Then
                txtEndDateUser.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
            End If
        End If

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

        Dim lStartFromConfig As Double = pBasicAttributes.GetValue(BFInputNames.StartDate, -99)
        If lStartFromConfig > 0 Then
            J2Date(lStartFromConfig, lDates)
            txtStartDateUser.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
        End If
        Dim lEndFromConfig As Double = pBasicAttributes.GetValue(BFInputNames.EndDate, -99)
        If lEndFromConfig > 0 AndAlso lEndFromConfig > lStartFromConfig Then
            J2Date(lEndFromConfig, lDates)
            timcnv(lDates)
            txtEndDateUser.Text = lDates(0) & "/" & lDates(1) & "/" & lDates(2)
        End If

        chkMethodPART.Checked = False
        chkMethodHySEPFixed.Checked = False
        chkMethodHySEPLocMin.Checked = False
        chkMethodHySEPSlide.Checked = False
        chkMethodBFIStandard.Checked = False
        chkMethodBFIModified.Checked = False
        chkMethodBFLOW.Checked = False
        chkMethodTwoPRDF.Checked = False
        Dim lMethods As ArrayList = pBasicAttributes.GetValue(BFInputNames.BFMethods, Nothing)
        If lMethods IsNot Nothing Then
            For Each lMethod As BFMethods In lMethods
                Select Case lMethod
                    Case BFMethods.PART
                        chkMethodPART.Checked = True
                        pMethods.Add(BFMethods.PART)
                    Case BFMethods.HySEPFixed
                        chkMethodHySEPFixed.Checked = True
                        pMethods.Add(BFMethods.HySEPFixed)
                    Case BFMethods.HySEPLocMin
                        chkMethodHySEPLocMin.Checked = True
                        pMethods.Add(BFMethods.HySEPLocMin)
                    Case BFMethods.HySEPSlide
                        chkMethodHySEPSlide.Checked = True
                        pMethods.Add(BFMethods.HySEPSlide)
                    Case BFMethods.BFIStandard
                        chkMethodBFIStandard.Checked = True
                        pMethods.Add(BFMethods.BFIStandard)
                    Case BFMethods.BFIModified
                        chkMethodBFIModified.Checked = True
                        pMethods.Add(BFMethods.BFIModified)
                    Case BFMethods.BFLOW
                        chkMethodBFLOW.Checked = True
                        pMethods.Add(BFMethods.BFLOW)
                    Case BFMethods.TwoPRDF
                        chkMethodTwoPRDF.Checked = True
                        pMethods.Add(BFMethods.TwoPRDF)
                End Select
            Next
        End If

        Dim lBFINDayScreen As Double = pBasicAttributes.GetValue(BFInputNames.BFINDayScreen, -99)
        Dim lBFITurnPtFrac As Double = pBasicAttributes.GetValue(BFInputNames.BFITurnPtFrac, -99)
        Dim lBFIRecessCont As Double = pBasicAttributes.GetValue(BFInputNames.BFIRecessConst, -99)
        If lBFINDayScreen > 0 Then
            txtN.Text = lBFINDayScreen.ToString
        End If
        If lBFITurnPtFrac > 0 Then
            txtF.Text = lBFITurnPtFrac.ToString
        End If
        If lBFIRecessCont > 0 Then
            txtK.Text = lBFIRecessCont.ToString
        End If

        Dim lBFIReportBy As String = pBasicAttributes.GetValue(BFInputNames.BFIReportby, "")
        If lBFIReportBy <> "" Then
            If lBFIReportBy = BFBatchInputNames.ReportByCY OrElse lBFIReportBy = BFInputNames.BFIReportbyCY Then
                rdoBFIReportbyCalendarYear.Checked = True
            ElseIf lBFIReportBy = BFBatchInputNames.ReportByWY OrElse lBFIReportBy = BFInputNames.BFIReportbyWY Then
                rdoBFIReportbyWaterYear.Checked = True
            End If
        End If

        Dim lReportBy As String = pBasicAttributes.GetValue(BFInputNames.Reportby, "")
        If lReportBy <> "" Then
            If lReportBy = BFBatchInputNames.ReportByCY OrElse lReportBy = BFInputNames.BFIReportbyCY Then
                rdoBFIReportbyCalendarYear.Checked = True
            ElseIf lReportBy = BFBatchInputNames.ReportByWY OrElse lReportBy = BFInputNames.BFIReportbyWY Then
                rdoBFIReportbyWaterYear.Checked = True
            End If
        End If

        If pMethods.Contains(BFMethods.BFLOW) Then
            Dim lFP1 As Double = pBasicAttributes.GetValue(BFInputNames.BFLOWFilter, 0.925)
            If Double.IsNaN(lFP1) OrElse lFP1 < 0 OrElse lFP1 > 1 Then
                txtDFParamBeta.Text = "0.925"
            Else
                txtDFParamBeta.Text = lFP1.ToString()
            End If
        End If
        pTwoParamEstimationMethod = pBasicAttributes.GetValue(BFInputNames.TwoParamEstMethod, clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT)
        If pMethods.Contains(BFMethods.TwoPRDF) Then
            Dim lRC As Double = pBasicAttributes.GetValue(BFInputNames.TwoPRDFRC, 0.978)
            Dim lBFImax As Double = pBasicAttributes.GetValue(BFInputNames.TwoPRDFBFImax, 0.8)
            Select Case pTwoParamEstimationMethod
                Case clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM
                    If Not Double.IsNaN(lRC) AndAlso Not Double.IsNaN(lBFImax) Then
                        txtDFParamRC.Text = lRC.ToString()
                        txtDFParamBFImax.Text = lBFImax.ToString()
                    End If
                    txt2PDefaultNotice.Visible = False
                    rdo2PSpecify.Checked = True
                Case clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT
                    txtDFParamRC.Text = ""
                    txtDFParamBFImax.Text = ""
                    txt2PDefaultNotice.Visible = True
                    rdo2PDefault.Checked = True
            End Select
        End If

        pOutputDir = pBasicAttributes.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        txtOutputDir.Text = pOutputDir
        OutputFilenameRoot = pBasicAttributes.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
        txtOutputRootName.Text = OutputFilenameRoot
        If pBasicAttributes.GetValue(BFInputNames.FullSpanDuration, False) Then
            rdoDurationYes.Checked = True
        Else
            rdoDurationNo.Checked = True
        End If
    End Sub

    Public Function AskUser(ByVal aName As String, _
                        ByVal aDataGroup As atcData.atcTimeseriesGroup, _
                        ByRef aStartMonth As Integer, _
                        ByRef aStartDay As Integer, _
                        ByRef aEndMonth As Integer, _
                        ByRef aEndDay As Integer, _
                        ByRef aFirstYear As Integer, _
                        ByRef aLastYear As Integer) As Boolean

    End Function

    Private Function AttributesFromForm(ByRef Args As atcDataAttributes) As String
        'check validity of inputs
        Dim lErrMsg As String = ""
        Dim lSDate As Double = -99
        Dim lEDate As Double = -99

        If pDataGroup.Count = 0 AndAlso Not pSetGlobal Then
            lErrMsg &= "- No streamflow data selected" & vbCrLf
        Else
            lSDate = StartDateFromForm()
            lEDate = EndDateFromForm()
            If Not pSetGlobal Then
                If lSDate < 0 OrElse lEDate < 0 Then
                    lErrMsg &= "- Problematic start and/or end date." & vbCrLf
                Else
                    Dim lTs As atcTimeseries = Nothing
                    For Each lTs In pDataGroup
                        Try
                            lTs = SubsetByDate(lTs, lSDate, lEDate, Nothing)
                            If lTs.Attributes.GetValue("Count missing") > 0 Then
                                If chkMethodHySEPFixed.Checked OrElse _
                                   chkMethodHySEPLocMin.Checked OrElse _
                                   chkMethodHySEPSlide.Checked OrElse _
                                   chkMethodPART.Checked Then
                                    'lErrMsg &= "- Selected Dataset has gaps." & vbCrLf
                                    lTs.Clear()
                                    Exit For
                                End If
                            Else
                                lTs.Clear()
                            End If
                        Catch ex As Exception
                            lErrMsg &= "- Problematic starting and ending dates." & vbCrLf
                        End Try
                    Next
                End If
            End If
        End If

        If Not pSetGlobal Then
            Dim lStationsInfo As New ArrayList()
            With grdStations
                For I As Integer = 0 To .Rows.Count - 1
                    If .Item(0, I).Value IsNot Nothing Then
                        lStationsInfo.Add("Station" & vbTab & .Item(0, I).Value & "," & .Item(1, I).Value & "," & .Item(2, I).Value)
                    End If
                Next
            End With
            Args.SetValue("StationInfo", lStationsInfo)
        End If

        If pMethods.Count = 0 Then lErrMsg = "- Method not set" & vbCrLf

        If Not IO.Directory.Exists(txtOutputDir.Text.Trim()) Then
            lErrMsg &= "- Need to specify output directory" & vbCrLf
        Else
            Args.SetValue(BFBatchInputNames.OUTPUTDIR, txtOutputDir.Text.Trim())
        End If

        Args.SetValue(BFBatchInputNames.OUTPUTPrefix, txtOutputRootName.Text.Trim())

        Dim lDA As Double = 0.0
        'If Not Double.TryParse(txtDrainageArea.Text.Trim, lDA) Then
        '    If chkMethodHySEPFixed.Checked OrElse _
        '       chkMethodHySEPLocMin.Checked OrElse _
        '       chkMethodHySEPSlide.Checked OrElse _
        '       chkMethodPART.Checked Then
        '        lErrMsg &= "- Drainage Area not set" & vbCrLf
        '    End If
        'ElseIf lDA <= 0 Then
        '    lErrMsg &= "- Drainage Area must be greater than zero" & vbCrLf
        'End If

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
        Dim lFP1 As Double
        If chkMethodBFLOW.Checked Then
            If Not Double.TryParse(txtDFParamBeta.Text.Trim(), lFP1) Then
                lErrMsg &= "- BFLOW method needs a valid beta value" & vbCrLf
            End If
        End If
        Dim lRC As Double = Double.NaN
        Dim lBFImax As Double = Double.NaN
        Dim lDF2P_NeedCalculation As Boolean = False
        If chkMethodTwoPRDF.Checked Then
            If pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.ECKHARDT Then
                'User choose Default parameter
                lDF2P_NeedCalculation = True
            ElseIf pTwoParamEstimationMethod = clsBaseflow2PRDF.ETWOPARAMESTIMATION.CUSTOM Then
                'User choose Specify parameter
                If Not Double.TryParse(txtDFParamRC.Text.Trim(), lRC) Then
                    lErrMsg &= "- Custom TwoPRDF method needs a valid recession constant." & vbCrLf
                End If
                If Not Double.TryParse(txtDFParamBFImax.Text.Trim(), lBFImax) OrElse
                    Not (lBFImax > 0 AndAlso lBFImax < 1) Then
                    lErrMsg &= "- Custom TwoPRDF method needs a valid BFImax." & vbCrLf
                End If
            End If
        End If

        If lErrMsg.Length = 0 Then
            'set methods
            Args.SetValue(BFInputNames.BFMethods, pMethods) '"Methods"
            'Set drainage area
            Args.SetValue(BFInputNames.DrainageArea, lDA) '"Drainage Area"
            'set duration
            If lSDate > 0 Then
                Args.SetValue(BFInputNames.StartDate, StartDateFromForm) '"Start Date"
            End If
            If lEDate > 0 Then
                Args.SetValue(BFInputNames.EndDate, EndDateFromForm) '"End Date"
            End If
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
            Dim lYearBasis As String = BFInputNames.ReportbyCY '"Calendar"
            If rdoBFIReportbyWaterYear.Checked Then
                lYearBasis = BFInputNames.ReportbyWY '"Water"
            End If
            Args.SetValue(BFInputNames.Reportby, lYearBasis) '"Reportby"
            If pMethods.Contains(BFMethods.BFIStandard) OrElse pMethods.Contains(BFMethods.BFIModified) Then
                Args.SetValue(BFInputNames.BFINDayScreen, lNDay) '"BFINDay"
                Dim lBFIYearBasis As String = BFInputNames.BFIReportbyCY '"Calendar"
                If rdoBFIReportbyWaterYear.Checked Then
                    lBFIYearBasis = BFInputNames.BFIReportbyWY '"Water"
                End If
                Args.SetValue(BFInputNames.BFIReportby, lBFIYearBasis) '"BFIReportby"
            End If
            If pMethods.Contains(BFMethods.BFLOW) Then
                Args.SetValue(BFInputNames.BFLOWFilter, lFP1)
            End If
            If pMethods.Contains(BFMethods.TwoPRDF) Then
                Args.SetValue(BFInputNames.TwoPRDFRC, lRC)
                Args.SetValue(BFInputNames.TwoPRDFBFImax, lBFImax)
                Args.SetValue(BFInputNames.TwoParamEstMethod, pTwoParamEstimationMethod)
            End If
            Args.SetValue(BFInputNames.BFMethods, pMethods)
            Args.SetValue(BFInputNames.FullSpanDuration, rdoDurationYes.Checked)
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
            If pSetGlobal Then
                Return -99
            End If
            Dim lAskUser As String =
            Logger.MsgCustomOwned("Invalid starting date. Use dataset start date?", "Start Date Correction", Me, New String() {"Yes", "No"})
            If lAskUser = "Yes" Then
                lArr = txtDataStart.Text.Trim.Split("/")
                txtStartDateUser.Text = ""
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
                If lMonth = 10 And lDay = 1 Then
                    If Not rdoBFIReportbyWaterYear.Checked Then
                        Dim lAskUser As String =
            Logger.MsgCustomOwned("Report by water year?", "Start Date", Me, New String() {"Yes", "No"})
                        If lAskUser = "Yes" Then
                            rdoBFIReportbyWaterYear.Checked = True
                        End If
                    End If
                End If
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
            If pSetGlobal Then
                Return -99
            End If
            Dim lAskUser As String = _
            Logger.MsgCustomOwned("Invalid ending date. Use dataset end date?", "End Date Correction", Me, New String() {"Yes", "No"})
            If lAskUser = "Yes" Then
                lArr = txtDataEnd.Text.Trim.Split("/")
                txtEndDateUser.Text = ""
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

        If lMonth > 12 OrElse lMonth <1 Then
            isGoodDate= False
        ElseIf lDay > DayMon(lYear, lMonth) Then
            isGoodDate = False
        End If
        Return isGoodDate
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub StationSelectionChanged(ByVal aSelectedIndex As Integer, ByVal aStationList As atcCollection, ByVal aIsDataDirty As Boolean) Handles pfrmStations.StationInfoChanged
        Dim lStationFilename As String
        Dim lDrainageArea As Double
        If aSelectedIndex >= 0 AndAlso aStationList.Item(aSelectedIndex) IsNot Nothing Then
            lStationFilename = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).Filename
            lDrainageArea = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).DrainageArea
            If lDrainageArea < 0 Then lDrainageArea = 0.0
        End If
        'txtDrainageArea.Text = lDrainageArea.ToString

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

    Private Sub SetBaseflowSepParm()
        Dim lFormCheckMsg As String = AttributesFromForm(pBasicAttributes)
        If lFormCheckMsg.Length > 0 Then
            Logger.Msg("Please address the following issues before proceeding:" & vbCrLf & vbCrLf & lFormCheckMsg, MsgBoxStyle.Information, "Input Needs Correction")
            Exit Sub
        Else
            If pSetGlobal Then
                Logger.MsgCustomOwned("Parameters are set for Global Defaults", "USGS Base-Flow Separation", Me, New String() {"OK"})
            Else
                Dim lGroup As String = pBasicAttributes.GetValue("Group", "")
                Logger.MsgCustomOwned("Group Parameters are set for " & lGroup, "USGS Base-Flow Separation", Me, New String() {"OK"})
            End If
        End If
        RaiseEvent ParametersSet(pBasicAttributes)
    End Sub

    Private Sub mnuOutputASCII_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        SetBaseflowSepParm()
        If pSetGlobal Then
            Me.Close()
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
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodPART", chkMethodPART.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodHySEPFixed", chkMethodHySEPFixed.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodHySEPLocMin", chkMethodHySEPLocMin.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodHySEPSlide", chkMethodHySEPSlide.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodBFIStandard", chkMethodBFIStandard.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "MethodBFIModified", chkMethodBFIModified.Checked)
        'SaveSetting("atcUSGSBaseflowBatch", "Defaults", "BFISymbols", chkBFISymbols.Checked)

        OutputDir = txtOutputDir.Text.Trim()
        Me.Close()
        'Dim lRDBWriter As New atcTimeseriesRDB()
    End Sub

    Private Sub txtOutputRootName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputRootName.TextChanged
        OutputFilenameRoot = txtOutputRootName.Text.Trim()
        OutputFilenameRoot = OutputFilenameRoot.Replace(" ", "_")
    End Sub

    Private Sub mnuGraphTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not pDidBFSeparation Then
            SetBaseflowSepParm()
            If Not pDidBFSeparation Then Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Function SetupGraphTsGroup(ByVal aTsCollection As atcCollection, ByVal aParams As atcDataAttributes) As atcTimeseriesGroup
        Dim lDA As Double = aParams.GetValue("Drainage Area")
        Dim lPerUnitArea As Boolean = aParams.GetValue("PerUnitArea")
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
        Dim lTsBF4Graph As atcTimeseries = aTsCollection.ItemByKey("RateDaily").Clone()
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
            lTsRunoff = lTsFlow - lTsBF4Graph
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
            lTsRunoff = lTsFlow - lTsBF4Graph

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
        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
            pDataGroup = Nothing
        End If
        Opened = False
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodPART", "False") = "True" Then
        '    chkMethodPART.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPFixed", "False") = "True" Then
        '    chkMethodHySEPFixed.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPLocMin", "False") = "True" Then
        '    chkMethodHySEPLocMin.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodHySEPSlide", "False") = "True" Then
        '    chkMethodHySEPSlide.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodBFIStandard", "False") = "True" Then
        '    chkMethodBFIStandard.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "MethodBFIModified", "False") = "True" Then
        '    chkMethodBFIModified.Checked = True
        'End If
        'If GetSetting("atcUSGSBaseflow", "Defaults", "BFISymbols", "False") = "True" Then
        '    chkBFISymbols.Checked = True
        'End If
        If chkMethodBFIStandard.Checked OrElse chkMethodBFIModified.Checked Then
            gbBFI.Enabled = True
        Else
            gbBFI.Enabled = False
        End If
        Opened = True
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for Analysis", pDataGroup)
        If pDataGroup.Count > 0 Then
            Me.Initialize(pDataGroup, pBasicAttributes, True)
        Else
            Logger.Msg("Need to select at least one daily streamflow dataset", "USGS Base-Flow Separation")
        End If
    End Sub

    Private Sub btnWriteASCIIOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteASCIIOutput.Click
        mnuOutputASCII_Click(Nothing, Nothing)
    End Sub

    Private Sub btnGraphTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        mnuGraphTimeseries_Click(Nothing, Nothing)
    End Sub

    Private Sub BFMethods_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMethodHySEPFixed.CheckedChanged,
                                                                                                             chkMethodHySEPLocMin.CheckedChanged,
                                                                                                             chkMethodHySEPSlide.CheckedChanged,
                                                                                                             chkMethodPART.CheckedChanged,
                                                                                                             chkMethodBFIStandard.CheckedChanged,
                                                                                                             chkMethodBFIModified.CheckedChanged,
                                                                                                             chkMethodBFLOW.CheckedChanged,
                                                                                                             chkMethodTwoPRDF.CheckedChanged

        If Opened Then
            pDidBFSeparation = False
            pMethods.Clear()

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

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MsgBox("Documentation not yet available")
    End Sub

    Private Sub txtAny_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles txtStartDateUser.TextChanged, txtEndDateUser.TextChanged, _
            txtN.TextChanged, txtF.TextChanged, txtK.TextChanged, txtOutputDir.TextChanged, txtOutputRootName.TextChanged
        pDidBFSeparation = False
    End Sub

    Private Sub btnPlotDur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlotDur.Click
        If pDataGroup.Count > 0 Then
            atcUSGSUtility.atcUSGSScreen.GraphDataDuration(pDataGroup)
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