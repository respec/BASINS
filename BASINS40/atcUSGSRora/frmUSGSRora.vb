Imports atcData
Imports atcUtility
Imports atcUSGSUtility
Imports atcGraph
Imports ZedGraph
Imports MapWinUtility
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class frmUSGSRora
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

    Private pFileKname As String

    Private pOutputDir As String = ""
    Private pDALastUsed As Double = 0.0
    Private pRecessIndexLastUsed As Double = 0.0
    Private pAnteRecess As Integer = 0

    Private pLastRunConfigs As atcDataAttributes

    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private WithEvents pFrmIndex As frmIndex
    Private pRoraAnalysis As clsRora
    'Private WithEvents pfrmStations As frmStations

    Public Opened As Boolean = False
    Private pDidRora As Boolean = False
    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                          Optional ByVal aBasicAttributes As Generic.List(Of String) = Nothing, _
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

        If pLastRunConfigs Is Nothing Then
            pLastRunConfigs = New atcDataAttributes()
        End If

        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.DropDownItems.Add(lMenuText, Nothing, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow Data for RORA", _
                                                       pDataGroup, Nothing, True, True, Me.Icon)
        End If

        If pDataGroup.Count > 0 Then
            pRoraAnalysis = New clsRora()
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

        pOutputDir = GetSetting("atcUSGSRORA", "Defaults", "OutputDir", "")
        pFileKname = GetSetting("atcUSGSRORA", "Defaults", "FileKname", "")
        pRoraAnalysis.OutputFilenameRoot = GetSetting("atcUSGSRORA", "Defaults", "BaseOutputFilename", "")

        'atcUSGSStations.StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        Dim lDA As Double = 0
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
                lDA = lDataset.Attributes.GetValue("Drainage Area")
                txtDrainageArea.Text = DoubleToString(lDA)
            End If
            If lDataset.Attributes.ContainsAttribute("RORAKMed") Then
                txtRecessionIndex.Text = DoubleToString(lDataset.Attributes.GetValue("RORAKMed"), , "0.000")
            End If
            Dim lDate(5) As Integer
            If lDataset.Attributes.ContainsAttribute("RORASJD") Then
                J2Date(lDataset.Attributes.GetValue("RORASJD"), lDate)
                txtStartDateUser.Text = lDate(0) & "/" & lDate(1) & "/" & lDate(2)
            End If
            If lDataset.Attributes.ContainsAttribute("RORAEJD") Then
                J2Date(lDataset.Attributes.GetValue("RORAEJD"), lDate)
                If lDate(1) = 1 And lDate(2) = 1 Then
                    lDate(0) -= 1 : lDate(1) = 12 : lDate(2) = 31
                End If
                txtEndDateUser.Text = lDate(0) & "/" & lDate(1) & "/" & lDate(2)
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
        End If

        txtOutputDir.Text = pOutputDir
        txtOutputRootName.Text = pRoraAnalysis.OutputFilenameRoot
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

        Dim lSDate As Double = 0
        Dim lEDate As Double = 0
        If pDataGroup.Count = 0 Then
            lErrMsg &= "- No streamflow data selected" & vbCrLf
        Else
            lSDate = StartDateFromForm()
            lEDate = EndDateFromForm()
            If lSDate < 0 OrElse lEDate < 0 OrElse lSDate >= lEDate Then
                lErrMsg &= "- Problematic start and/or end date." & vbCrLf
            Else
                Dim lTs As atcTimeseries = Nothing
                For Each lTs In pDataGroup
                    Try
                        lTs = SubsetByDate(lTs, lSDate, lEDate, Nothing)
                        If lTs.Attributes.GetValue("Count missing") > 0 Then
                            lErrMsg &= "- Selected Dataset has gaps." & vbCrLf
                            lTs.Clear()
                            Exit For
                        Else
                            lTs.Clear()
                        End If
                    Catch ex As Exception
                        lErrMsg &= "- Problematic starting and ending dates." & vbCrLf
                    End Try
                Next
            End If
        End If

        Dim lDA As Double = 0.0
        If Not Double.TryParse(txtDrainageArea.Text.Trim, lDA) Then
            lErrMsg &= "- Drainage Area not set" & vbCrLf
            GoTo Notify
        ElseIf lDA <= 0 Then
            lErrMsg &= "- Drainage Area must be greater than zero" & vbCrLf
            GoTo Notify
        End If

        Dim lIndex As Double = 0.0
        If Not Double.TryParse(txtRecessionIndex.Text.Trim, lIndex) Then lErrMsg &= "- Recession Index not set" & vbCrLf

        Dim lAnteRecession As Integer = 0
        If cboAnteRecess.SelectedItem Is Nothing Then
            lErrMsg &= "- Antecedent Recession Days not set" & vbCrLf
        ElseIf Not Integer.TryParse(cboAnteRecess.SelectedItem.ToString.Trim, lAnteRecession) Then
            lErrMsg &= "- Antecedent Recession Days invalid" & vbCrLf
        Else
            If lAnteRecession <= 0 Then
                lErrMsg &= "- Antecedent Recession Days invalid" & vbCrLf
            End If
        End If

        If lErrMsg.Length = 0 Then
            'Set drainage area
            Args.SetValue("Drainage Area", lDA)
            'set index
            Args.SetValue("Recession Index", lIndex)
            Args.SetValue("Antecedent Recession", lAnteRecession)
            'set duration
            Args.SetValue("Start Date", StartDateFromForm)
            Args.SetValue("End Date", EndDateFromForm)
            'Set streamflow
            Args.SetValue("Streamflow", pDataGroup)
            Args.SetValue("Output Path", txtOutputDir.Text.Trim())
            Args.SetValue("Original Start Date", pDataGroup(0).Dates.Value(0))
            Args.SetValue("Original End Date", pDataGroup(0).Dates.Value(pDataGroup(0).numValues))
            Args.SetValue("Constituent", pDataGroup(0).Attributes.GetValue("Constituent"))
            Args.SetValue("History 1", pDataGroup(0).Attributes.GetValue("History 1"))
            'Set Unit
            Args.SetValue("EnglishUnit", True)
            'Set station.txt
            'Args.SetValue("Station File", atcUSGSStations.StationInfoFile)
        End If

Notify:
        Return lErrMsg
    End Function

    Private Sub ClearAttributes()
        Dim lRemoveThese As New atcCollection
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Rora") Then
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
            Dim lAskUser As String = _
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

        If lMonth > 12 OrElse lMonth < 1 Then
            isGoodDate = False
        ElseIf lDay > DayMon(lYear, lMonth) Then
            isGoodDate = False
        End If
        Return isGoodDate
    End Function

    Private Function ConfigurationChanged() As Boolean
        Dim lIsConfigChanged As Boolean = False

        If pLastRunConfigs.Count = 0 Then
            AttributesFromForm(pLastRunConfigs)
            Return True
        Else
            With pLastRunConfigs
                Dim lLastStartDate As Double = .GetValue("Original Start Date", 0)
                Dim lLastEndDate As Double = .GetValue("Original End Date", 0)
                Dim lLastConstituent As String = .GetValue("Constituent", "")
                Dim lLastHistory1 As String = .GetValue("History 1", "")

                Dim lCurrentStartDate As Double = pDataGroup(0).Dates.Value(0)
                Dim lCurrentEndDate As Double = pDataGroup(0).Dates.Value(pDataGroup(0).numValues)
                Dim lCurrentConstituent As String = pDataGroup(0).Attributes.GetValue("Constituent")
                Dim lCurrentHistory1 As String = pDataGroup(0).Attributes.GetValue("History 1")

                If .GetValue("Drainage Area", "") <> txtDrainageArea.Text Then
                    lIsConfigChanged = True
                ElseIf .GetValue("Start Date", 0) <> StartDateFromForm() Then
                    lIsConfigChanged = True
                ElseIf .GetValue("End Date", 0) <> EndDateFromForm() Then
                    lIsConfigChanged = True
                ElseIf .GetValue("Recession Index") <> txtRecessionIndex.Text Then
                    lIsConfigChanged = True
                ElseIf .GetValue("Antecedent Recession") <> pAnteRecess Then
                    lIsConfigChanged = True
                ElseIf .GetValue("Output Path", "") <> txtOutputDir.Text Then
                    lIsConfigChanged = True
                ElseIf lLastConstituent <> lCurrentConstituent OrElse lLastHistory1 <> lCurrentHistory1 OrElse lLastStartDate <> lCurrentStartDate OrElse lLastEndDate <> lCurrentEndDate Then
                    lIsConfigChanged = True
                Else
                End If
                'Set Unit
                '.GetValue("EnglishUnit", True)
            End With
        End If
        If lIsConfigChanged Then
            pLastRunConfigs.Clear()
            lIsConfigChanged = ConfigurationChanged()
        End If
        Return lIsConfigChanged
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

    'Private Sub StationSelectionChanged(ByVal aSelectedIndex As Integer, ByVal aStationList As atcCollection, ByVal aIsDataDirty As Boolean) Handles pfrmStations.StationInfoChanged
    '    Dim lStationFilename As String
    '    Dim lDrainageArea As Double
    '    If aSelectedIndex >= 0 AndAlso aStationList.Item(aSelectedIndex) IsNot Nothing Then
    '        lStationFilename = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).Filename
    '        lDrainageArea = CType(aStationList.Item(aSelectedIndex), atcUSGSStations.USGSGWStation).DrainageArea
    '        If lDrainageArea < 0 Then lDrainageArea = 0.0
    '    End If
    '    txtDrainageArea.Text = lDrainageArea.ToString

    '    If aIsDataDirty Then
    '        'atcUSGSStations.SaveStations(aStationList, atcUSGSStations.StationInfoFile)
    '    End If
    'End Sub

    Private Sub btnExamineData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New frmDataSummary(atcUSGSScreen.PrintDataSummary(lTs))
            lfrmDataSummary.ClearSelection()
            lfrmDataSummary.Show()
        Next
    End Sub

    Private Sub ComputeRora()
        Dim lArgs As New atcDataAttributes
        Dim lFormCheckMsg As String = AttributesFromForm(lArgs)
        If lFormCheckMsg.Length > 0 Then
            Logger.Msg("Please address the following issues before proceeding:" & vbCrLf & vbCrLf & lFormCheckMsg, MsgBoxStyle.Information, "Input Needs Correction")
            Exit Sub
        End If
        ClearAttributes()
        Try
            pRoraAnalysis.Rora(lArgs)
            pDALastUsed = lArgs.GetValue("Drainage Area")
            pRecessIndexLastUsed = lArgs.GetValue("Recession Index")
            pRoraAnalysis.PARAMAnteRecessDays = lArgs.GetValue("Antecedent Recession")
            pDidRora = True
        Catch ex As Exception
            Logger.Msg("RORA failed: " & vbCrLf & ex.Message, MsgBoxStyle.Critical, "RORA")
        End Try
        'If pDidBFSeparation Then
        '    Logger.Msg("Baseflow separation is successful.", MsgBoxStyle.OkOnly, "Baseflow Separation")
        'End If
    End Sub

    Private Sub txtOutputRootName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputRootName.TextChanged
        pRoraAnalysis.OutputFilenameRoot = txtOutputRootName.Text.Trim()
        pRoraAnalysis.OutputFilenameRoot = pRoraAnalysis.OutputFilenameRoot.Replace(" ", "_")
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
            .Description = "Specify RORA ASCII output directory"
            If .ShowDialog = DialogResult.OK Then
                ' Display the selected folder if the user clicked on the OK button.
                lDir = .SelectedPath
            End If
        End With
        If IO.Directory.Exists(lDir) Then
            txtOutputDir.Text = lDir
            pOutputDir = lDir
            SaveSetting("atcUSGSRORA", "Defaults", "OutputDir", pOutputDir)
        End If
    End Sub

    Private Sub frmMain_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        If pDataGroup IsNot Nothing Then
            pDataGroup.Clear()
            pDataGroup = Nothing
        End If
        Opened = False
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Opened = True
    End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow for RORA Analysis", pDataGroup)
        If pDataGroup.Count > 0 Then
            If ConfigurationChanged() Then
                Me.Initialize(pDataGroup, pBasicAttributes, True)
            End If
        Else
            Logger.Msg("Need to select at least one daily streamflow dataset", "USGS RORA")
        End If
    End Sub

    'Private Sub mnuGraphCDistPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraphCDistPlot.Click
    '    If Not pDidRora Then
    '        ComputeRora()
    '        If Not pDidRora Then Exit Sub
    '    End If

    '    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
    '    DoBFGraphTimeseries("CDist")
    '    Me.Cursor = System.Windows.Forms.Cursors.Default
    'End Sub

    Private Sub btnWriteASCIIOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteASCIIOutput.Click
        If Not ConfigurationChanged() Then
            'Logger.Msg("Nothing is changed since previous RORA run." & vbCrLf & vbCrLf & pRoraAnalysis.Bulletin, "No Need To Rerun RORA")
            'Exit Sub
            GoTo ViewOutput
        Else
            pDidRora = False
        End If

        If Not pDidRora Then
            ComputeRora()
            Dim lResult As String = WriteRoraASCIIOutput()
            If lResult.StartsWith("Failed") Then
                Logger.Msg(lResult, "Writing RORA ASCII output failed")
                pDidRora = False
            End If
            If Not pDidRora Then Exit Sub
        End If

ViewOutput:
        Dim lResponse As String = Logger.MsgCustomOwned("RORA output completed." & vbCrLf & vbCrLf & pRoraAnalysis.Bulletin, "USGS RORA", Me, New String() {"OK", "View Output Files"})
        If lResponse = "View Output Files" Then
            'pRoraAnalysis.GetRechargeTimeseries(atcTimeUnit.TUMonth)
            Dim lFrmOutput As New frmOutput
            lFrmOutput.Initialize(txtOutputDir.Text.Trim(), txtOutputRootName.Text.Trim())
            lFrmOutput.Show()
        End If
    End Sub

    Private Function WriteRoraASCIIOutput() As String
        Dim lSpecification As String = ""
        If pDataGroup Is Nothing OrElse pDataGroup.Count = 0 Then
            Return "Failed,no data"
        End If
        If Not IO.Directory.Exists(txtOutputDir.Text.Trim()) Then
            Logger.Msg("Please specify an output directory", "RORA ASCII Output")
            txtOutputDir.Focus()
            Return "Failed,output directory not specified"
        End If
        If pRoraAnalysis.OutputFilenameRoot = "" Then
            Logger.Msg("Please specify a base output filename", "RORA ASCII Output")
            txtOutputRootName.Focus()
            Return "Failed,base output filename not specified"
        Else
            SaveSetting("atcUSGSRORA", "Defaults", "BaseOutputFilename", pRoraAnalysis.OutputFilenameRoot)
        End If
        pRoraAnalysis.OutputDir = txtOutputDir.Text.Trim()
        pRoraAnalysis.ASCIICommon()
        Return "Succeed"
    End Function

    'Private Sub btnGraphTimeseries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphTimeseries.Click
    '    mnuGraphTimeseries_Click(Nothing, Nothing)
    'End Sub

    Private Sub mnuHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click
        MsgBox("Documentation not yet available")
    End Sub

    Private Sub txtDrainageArea_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDrainageArea.TextChanged
        pDidRora = False
    End Sub

    Private Sub txtDrainageArea_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDrainageArea.Validated
        Dim lMsg As String = CheckDrainageArea()
        If lMsg.Trim.Length > 0 Then
            Logger.Msg(lMsg)
        Else
            Dim lMustExitDueToFatalError As Boolean = SetAnteRecess()
            If lMustExitDueToFatalError Then
                Logger.MsgCustomOwned("The drainage area entered results in antecednet recession period being too long." & vbCrLf & _
                                      "Unable to conduct RORA analysis." & vbCrLf & vbCrLf & _
                                      "Please reexamine the drainage area of this streamflow station.", "End RORA", Me, _
                                      New String() {"OK"})
            Else
                PopulateAnteRecessList()
            End If
        End If
    End Sub

    Private Sub txtDrainageArea_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDrainageArea.Validating

    End Sub

    Private Function CheckDrainageArea() As String
        Dim lMsg As String = ""
        Dim lDA As Double
        If Not Double.TryParse(txtDrainageArea.Text.Trim(), lDA) Then
            lMsg = "Please enter a numeric value for drainage area."
        Else
            If lDA < 1.0 Then
                lMsg = "Drainage area is too small. Use Results with caution."
            ElseIf lDA > 500 Then
                lMsg = "Drainage area is too large. Use Results with caution."
            End If
        End If
        Return lMsg
    End Function

    Private Sub frmUSGSRora_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Dim lMsg As String = CheckDrainageArea()
        If lMsg.Length > 0 Then Logger.Msg(lMsg)

        Dim lMustExitDueToFatalError As Boolean = SetAnteRecess()
        If lMustExitDueToFatalError Then
            Logger.MsgCustomOwned("The calulated mininum antecedent recession period is too long." & vbCrLf & _
                      "Can not continue with RORA analysis." & vbCrLf & vbCrLf & _
                      "Please examine the drainage area of this streamflow station.", "End RORA", Me, _
                      New String() {"OK"})
            'Me.Close()
        Else
            PopulateAnteRecessList()
        End If
    End Sub

    Private Function SetAnteRecess() As Boolean
        Dim lMustExitDueToFatalError As Boolean = False
        Dim lDA As Double = 0
        If Double.TryParse(txtDrainageArea.Text.Trim, lDA) Then
            If lDA > 0 Then
                pRoraAnalysis.ITBase(lDA)
                lMustExitDueToFatalError = pRoraAnalysis.FatalError
            End If
        End If
        Return lMustExitDueToFatalError
    End Function

    Private Sub PopulateAnteRecessList()
        If pRoraAnalysis.RangeOfAnteRecessionDays IsNot Nothing Then
            With cboAnteRecess.Items
                .Clear()
                For Each lDay As Integer In pRoraAnalysis.RangeOfAnteRecessionDays
                    .Add(lDay.ToString)
                Next
            End With
            cboAnteRecess.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnExamineData_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExamineData.Click
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New frmDataSummary(atcUSGSScreen.PrintDataSummary(lTs))
            lfrmDataSummary.ClearSelection()
            lfrmDataSummary.Show()
        Next
    End Sub

    Private Sub btnRecessionIndex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecessionIndex.Click

        Dim lNewFileKFullName As String = ""
        Dim lInitialPath As String = ""
        Try
            If pFileKname <> "" Then
                lInitialPath = IO.Path.GetDirectoryName(pFileKname)
            Else
                lInitialPath = "C:\"
            End If
        Catch ex As Exception

        End Try

        Dim lOpenFileDialog As New System.Windows.Forms.OpenFileDialog()
        With lOpenFileDialog
            .Title = "Browse For Recession Index File"
            .InitialDirectory = lInitialPath '"c:\"
            .Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 2
            .RestoreDirectory = True
            If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                lNewFileKFullName = .FileName
                SaveSetting("atcUSGSRORA", "Defaults", "FileKname", lNewFileKFullName)
            End If
            .Dispose()
        End With

        If IO.File.Exists(lNewFileKFullName) Then
            pFileKname = lNewFileKFullName
            If pFrmIndex Is Nothing OrElse pFrmIndex.IsDisposed Then
                pFrmIndex = New frmIndex(pFileKname)
                pFrmIndex.Show()
            End If
        End If
    End Sub

    Private Sub ChangeK(ByVal aNewK As Double) Handles pFrmIndex.NewK
        txtRecessionIndex.Text = DoubleToString(aNewK)
    End Sub

    Private Sub cboAnteRecess_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAnteRecess.SelectedIndexChanged
        Integer.TryParse(cboAnteRecess.SelectedItem.ToString, pAnteRecess)
    End Sub

    Private Sub btnGraphRecharge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraphRecharge.Click
        If Not ConfigurationChanged() Then
            GoTo PlotOutput
        Else
            pDidRora = False
        End If

        If Not pDidRora Then
            ComputeRora()
            Dim lResult As String = WriteRoraASCIIOutput()
            If lResult.StartsWith("Failed") Then
                Logger.Msg(lResult, "Writing RORA ASCII output failed")
                pDidRora = False
            End If
            If Not pDidRora Then Exit Sub
        End If

        Dim lSpecification As String = ""
        If pDataGroup Is Nothing OrElse pDataGroup.Count = 0 Then
            Exit Sub
        End If

PlotOutput:
        DoGraphRecharge("Timeseries")
    End Sub

    Public Sub DoGraphRecharge(ByVal aGraphType As String)
        If pDataGroup Is Nothing OrElse pDataGroup.Count = 0 Then
            Exit Sub
        End If
        pRoraAnalysis.GetRechargeTimeseries(atcTimeUnit.TUMonth)
        Dim lDataGroup As New atcTimeseriesGroup
        If pRoraAnalysis.TsRecharge IsNot Nothing Then
            lDataGroup.Add(pRoraAnalysis.TsRecharge)
        Else
            Logger.Msg("Recharge timeseries is empty. Unable to generate graph.", "RORA: problem plotting recharge")
            Exit Sub
        End If

        Dim lYAxisTitleText As String = "Recharge (inches)"
        With lDataGroup(0).Attributes
            .SetValue("Constituent", "Recharge")
            '.SetValue("Scenario", "Estimated by " & lMethod)
            .SetValue("Units", lYAxisTitleText)
            .SetValue("YAxis", "LEFT")
        End With

        If aGraphType = "Timeseries" Then
            DisplayTsGraph(lDataGroup)
        End If

    End Sub

    Private Sub DisplayTsGraph(ByVal aDataGroup As atcTimeseriesGroup)
        Dim lGraphForm As New atcGraph.atcGraphForm()
        lGraphForm.Icon = Me.Icon
        Dim lZgc As ZedGraphControl = lGraphForm.ZedGraphCtrl
        Dim lGraphTS As New clsGraphTime(aDataGroup, lZgc)
        lGraphForm.Grapher = lGraphTS

        With lGraphForm.Grapher.ZedGraphCtrl.GraphPane
            .CurveList.Item(0).Color = Drawing.Color.Blue
            'With CType(.CurveList.Item(0), LineItem).Symbol
            '    .Type = SymbolType.TriangleDown
            '    .IsVisible = True
            'End With
        End With

        lGraphForm.Show()
    End Sub
End Class