Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class frmMain
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

    Private pMethod As String = ""
    Private pScenario As String = ""
    Private pMethodLastDone As String = ""
    Private WithEvents pDataGroup As atcTimeseriesGroup
    Private WithEvents pfrmStations As frmStations

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

        If aShowForm Then
            Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
            For Each lDisp As atcDataDisplay In DisplayPlugins
                Dim lMenuText As String = lDisp.Name
                If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
                mnuAnalysis.DropDownItems.Add(lMenuText, Nothing, New EventHandler(AddressOf mnuAnalysis_Click))
            Next
        End If

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            pDataGroup = atcDataManager.UserSelectData("Select Daily Streamflow Data for Baseflow Separation", _
                                                       pDataGroup, Nothing, True, True, Me.Icon)
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

        pMethod = GetSetting("atcUSGSBaseflow", "Defaults", "Model", "HySep-Fixed")
        StationInfoFile = GetSetting("atcUSGSBaseflow", "Defaults", "Stations", "Station.txt")

        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        Dim lFirstDate As Double = GetMaxValue()
        Dim lLastDate As Double = GetMinValue()

        pCommonStart = GetMinValue()
        pCommonEnd = GetMaxValue()

        Dim lAllText As String = "All"
        Dim lCommonText As String = "Common"

        For Each lDataset As atcData.atcTimeseries In pDataGroup
            If lDataset.Dates.numValues > 0 Then
                Dim lThisDate As Double = lDataset.Dates.Value(0)
                If lThisDate < lFirstDate Then lFirstDate = lThisDate
                If lThisDate > pCommonStart Then pCommonStart = lThisDate
                lThisDate = lDataset.Dates.Value(lDataset.Dates.numValues)
                If lThisDate > lLastDate Then lLastDate = lThisDate
                If lThisDate < pCommonEnd Then pCommonEnd = lThisDate
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
            lAllText &= ": " & pDateFormat.JDateToString(lFirstDate + 1) & " to " & pDateFormat.JDateToString(lLastDate)
        End If

        If pCommonStart > GetMinValue() AndAlso pCommonEnd < GetMaxValue() AndAlso pCommonStart < pCommonEnd Then
            lCommonText &= ": " & pDateFormat.JDateToString(pCommonStart + 1) & " to " & pDateFormat.JDateToString(pCommonEnd)
        Else
            lCommonText &= pNoDatesInCommon
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

        If pDataGroup.Count = 0 Then
            lErrMsg &= "- No streamflow data selected" & vbCrLf
        Else
            Dim lSDate As Double = StartDateFromForm()
            Dim lEDate As Double = EndDateFromForm()
            If lSDate < 0 OrElse lEDate < 0 Then
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

        If pMethod = "" Then lErrMsg = "- Method not set" & vbCrLf
        Dim lDA As Double = 0.0
        If Not Double.TryParse(txtDrainageArea.Text.Trim, lDA) Then lErrMsg &= "- Drainage Area not set" & vbCrLf
        If txtOutputRootName.Text.Trim = "" Then
            lErrMsg &= "- Need to specify an output name" & vbCrLf
        ElseIf txtOutputRootName.Text.Trim.Length > 25 Then
            lErrMsg &= "- Output name should be less than 25 characters" & vbCrLf
        End If

        If lErrMsg.Length = 0 Then
            'set method
            Args.SetValue("Method", pMethod)
            'Set drainage area
            Args.SetValue("Drainage Area", lDA)
            'set duration
            Args.SetValue("Start Date", StartDateFromForm)
            Args.SetValue("End Date", EndDateFromForm)
            'Set streamflow
            Args.SetValue("Streamflow", pDataGroup)
            'Set Unit
            Args.SetValue("EnglishUnit", True)
            'Set station.txt
            Args.SetValue("Station File", StationInfoFile)
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
            Dim lAskUser As String = _
            Logger.MsgCustom("Invalid starting date. Use dataset start date?", "Start Date Correction", New String() {"Yes", "No"})
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
            Logger.MsgCustom("Invalid ending date. Use dataset end date?", "End Date Correction", New String() {"Yes", "No"})
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


    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub btnFindStations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindStations.Click
        StationInfoFile = FindFile("Locate Station File", StationInfoFile, "txt")
        SaveSetting("atcUSGSBaseflow", "Defaults", "Stations", StationInfoFile)
        GetStations()
        pfrmStations = New frmStations()
        pfrmStations.AskUser(Stations)
    End Sub

    Private Sub StationSelectionChanged(ByVal aSelectedIndex As Integer, ByVal aStationList As atcCollection, ByVal aIsDataDirty As Boolean) Handles pfrmStations.StationInfoChanged
        Dim lStationFilename As String
        Dim lDrainageArea As Double
        If aSelectedIndex >= 0 AndAlso aStationList.Item(aSelectedIndex) IsNot Nothing Then
            lStationFilename = CType(aStationList.Item(aSelectedIndex), USGSGWStation).Filename
            lDrainageArea = CType(aStationList.Item(aSelectedIndex), USGSGWStation).DrainageArea
            If lDrainageArea < 0 Then lDrainageArea = 0.0
        End If
        txtDrainageArea.Text = lDrainageArea.ToString

        If aIsDataDirty Then
            SaveStations(aStationList, StationInfoFile)
        End If
    End Sub

    Private Sub btnExamineData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExamineData.Click
        For Each lTs As atcTimeseries In pDataGroup
            Dim lfrmDataSummary As New frmDataSummary(PrintDataSummary(lTs))
            lfrmDataSummary.txtDataSummary.SelectionStart = 0
            lfrmDataSummary.Show()
        Next
    End Sub

    Private Sub cboBFMothod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBFMothod.SelectedIndexChanged
        pMethod = sender.SelectedItem()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim lErrMsg As String = ""

        Dim lArgs As New atcDataAttributes
        Dim lFormCheckMsg As String = AttributesFromForm(lArgs)
        If lFormCheckMsg.Length > 0 Then
            Logger.Msg("Please address the following issues before proceed:" & vbCrLf & vbCrLf & lFormCheckMsg, MsgBoxStyle.Information, "Input Needs Correction")
            Exit Sub
        End If

        ClearAttributes()
        Dim lClsBaseFlowCalculator As atcTimeseriesBaseflow.atcTimeseriesBaseflow
        lClsBaseFlowCalculator = New atcTimeseriesBaseflow.atcTimeseriesBaseflow
        Try
            lClsBaseFlowCalculator.Open("Baseflow", lArgs)
            lClsBaseFlowCalculator.DataSets.Clear()
            pMethodLastDone = lArgs.GetValue("Method")
        Catch ex As Exception
            Logger.Msg("Baseflow separation failed: " & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Baseflow separation")
            lErrMsg = ex.Message
        End Try
        If lErrMsg = "" Then
            Logger.Msg("Baseflow separation is successful.", MsgBoxStyle.OkOnly, "Baseflow Separation")
        End If
    End Sub

    Private Sub mnuOutputASCII_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOutputASCII.Click
        Dim lSpecification As String = ""
        If pScenario = "" Then
            Logger.Msg("Please specify an output scenario name", "Baseflow ASCII Output")
            txtOutputRootName.Focus()
            Exit Sub
        End If
        Logger.Dbg("mnuOutputASCII_Click " & pMethod)
        'Dim cdlg As New Windows.Forms.OpenFileDialog
        'With cdlg
        '    .Title = "Specify Baseflow ASCII output filename"
        'End With

        Dim lDir As String = IO.Path.GetDirectoryName(pDataGroup(0).Attributes.GetValue("History 1").ToString.ToLower.Substring("read from ".Length))
        Dim FolderBrowserDialog1 As New FolderBrowserDialog
        With FolderBrowserDialog1
            ' Desktop is the root folder in the dialog.
            .RootFolder = Environment.SpecialFolder.Desktop
            ' Select directory on entry.
            .SelectedPath = lDir
            ' Prompt the user with a custom message.
            .Description = "Specify Baseflow ASCII output directory"
            If .ShowDialog = DialogResult.OK Then
                ' Display the selected folder if the user clicked on the OK button.
                lDir = .SelectedPath
            End If
        End With
        'If lBaseFileName.Length > 0 AndAlso FileExists(lFileName) Then
        '    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\FindFile\FoundFiles", lBaseFileName, lFileName)
        'End If

        If pMethodLastDone.ToUpper.StartsWith("HYSEP") Then
            Dim lFilename As String = IO.Path.GetDirectoryName(pDataGroup(0).Attributes.GetValue("History 1").ToString.ToLower.Substring("read from ".Length))
            lFilename = IO.Path.Combine(lDir, pScenario & ".SBF")
            ASCIIHySepBSF(pDataGroup(0), lFilename)
            lSpecification = lFilename

            'With cdlg
            '    lFilename = AbsolutePath(lFilename, CurDir)
            '    .FileName = lFilename
            '    .Filter = ""
            '    '.FilterIndex = 0
            '    .DefaultExt = "SBF"
            'End With
        ElseIf pMethodLastDone.ToUpper.StartsWith("PART") Then
            'With cdlg
            '    lFilename = AbsolutePath(lFilename, CurDir)
            '    .FileName = lFilename
            '    .Filter = ""
            '    '.FilterIndex = 0
            '    .DefaultExt = "SBF"
            'End With
            Dim lFilename As String = IO.Path.Combine(lDir, pScenario & "_partday.txt")
            ASCIIPartDaily(pDataGroup(0), lFilename)
            lFilename = IO.Path.Combine(lDir, pScenario & "_partmon.txt")
            lSpecification = lFilename
            ASCIIPartMonthly(pDataGroup(0), lFilename)
            lFilename = IO.Path.Combine(lDir, pScenario & "_partqrt.txt")
            ASCIIPartQuarterly(pDataGroup(0), lFilename)
            lFilename = IO.Path.Combine(lDir, pScenario & "_partWY.txt")
            ASCIIPartWaterYear(pDataGroup(0), lFilename)
            lFilename = IO.Path.Combine(lDir, pScenario & "_partsum.txt")
            ASCIIPartBFSum(pDataGroup(0), lFilename)
        End If

        'With cdlg
        '    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        lFilename = AbsolutePath(.FileName, CurDir)
        '        aFilterIndex = .FilterIndex
        '        Logger.Dbg("User specified file '" & lFilename & "'")
        '        Logger.LastDbgText = ""
        '    Else 'Return empty string if user clicked Cancel
        '        lFilename = ""
        '        Logger.Dbg("User Cancelled File Selection Dialog for " & aFileDialogTitle)
        '        Logger.LastDbgText = "" 'forget about this - user was in control - no additional message box needed
        '    End If
        'End With

        Dim lProcess As New Process
        With lProcess
            .StartInfo.FileName = "Notepad.exe"
            .StartInfo.Arguments = lSpecification
            Try
                .Start()
            Catch lException As System.SystemException
                'Dim lExtension As String = FileExt(lSpecification)
                'lProcess.StartInfo.FileName = "Notepad.exe"
                'lProcess.StartInfo.Arguments = lSpecification
                'lProcess.Start()
                .Dispose()
            End Try
        End With
    End Sub

    Private Sub txtOutputRootName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputRootName.TextChanged
        pScenario = txtOutputRootName.Text.Trim()
        pScenario = pScenario.Replace(" ", "_")
    End Sub

    Private Sub mnuGraphBF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuGraphBF.Click
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        DoBFGraph()
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Public Sub DoBFGraph(Optional ByVal aDataGroup As atcData.atcTimeseriesGroup = Nothing)

        Dim lGraphPlugin As New atcGraph.atcGraphPlugin
        Dim lGraphForm As atcGraph.atcGraphForm
        'Dim lSeparateGraphs As Boolean = False
        'Select Case pDataGroup.Count
        '    Case 0 : Return
        '    Case 1 : lSeparateGraphs = False
        '    Case Else
        '        lSeparateGraphs = (Logger.MsgCustomCheckbox("Create separate graphs or all on one graph?", _
        '                                                    pDataGroup.Count & " datasets selected", _
        '                                                    "Do not ask again", "BASINS", "SWSTAT", "SeparateFreqGraphs", _
        '                                                    "Separate", "One Graph") = "Separate")
        'End Select

        Dim lTsDailyStreamflow As atcTimeseries = pDataGroup(0)

        Dim lTsBaseflow1 As atcTimeseries = Nothing
        Dim lTsBaseflow2 As atcTimeseries = Nothing
        Dim lTsBaseflow3 As atcTimeseries = Nothing

        Dim lBFDatagroup As atcTimeseriesGroup = lTsDailyStreamflow.Attributes.GetDefinedValue("Baseflow").Value
        If lBFDatagroup IsNot Nothing Then
            For Each lTsBF As atcTimeseries In lBFDatagroup
                If pMethodLastDone.ToLower.StartsWith("hysep") Then
                    If lTsBF.Attributes.GetValue("Scenario").ToString.ToLower.StartsWith("hysep") Then
                        lTsBaseflow1 = lTsBF
                        Exit For
                    End If
                ElseIf pMethodLastDone.ToLower.StartsWith("part") Then
                    Select Case lTsBF.Attributes.GetValue("Scenario")
                        Case "PartDaily1"
                            lTsBaseflow1 = lTsBF
                        Case "PartDaily2"
                            lTsBaseflow2 = lTsBF
                        Case "PartDaily3"
                            lTsBaseflow3 = lTsBF
                    End Select
                End If
            Next
        Else
            Logger.Dbg("DoBFGraph: no baseflow data found.")
            Exit Sub
        End If

        If pMethodLastDone.ToLower.StartsWith("hysep") Then
            If lTsBaseflow1 Is Nothing Then
                Logger.Dbg("DoBFGraph: no baseflow data found.")
                Exit Sub
            End If
        ElseIf pMethodLastDone.ToLower.StartsWith("part") Then
            If lTsBaseflow1 Is Nothing OrElse lTsBaseflow2 Is Nothing OrElse lTsBaseflow3 Is Nothing Then
                Logger.Dbg("DoBFGraph: no baseflow data found.")
                Exit Sub
            End If
        End If

        Dim lstart As Double = lTsBaseflow1.Attributes.GetValue("SJDay")
        Dim lend As Double = lTsBaseflow1.Attributes.GetValue("EJDay")
        Dim lTsFlow As atcTimeseries = SubsetByDate(lTsDailyStreamflow, lstart, lend, Nothing)
        lTsFlow.Attributes.SetValue("Units", "Inch")
        lTsFlow.Attributes.SetValue("YAxis", "LEFT")

        lTsBaseflow1.Attributes.SetValue("Constituent", "Baseflow")
        lTsBaseflow1.Attributes.SetValue("Units", "Inch")
        lTsBaseflow1.Attributes.SetValue("YAxis", "LEFT")

        Dim lDataGroup As New atcData.atcTimeseriesGroup
        lDataGroup.Add(lTsFlow)
        lDataGroup.Add(lTsBaseflow1)
        lGraphForm = lGraphPlugin.Show(lDataGroup, "Timeseries")
        lGraphForm.Icon = Me.Icon
        'lDataGroup.Clear()
        'lDataGroup = Nothing

    End Sub
End Class