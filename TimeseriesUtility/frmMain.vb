Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmMain

    WithEvents SelectedData As New atcTimeseriesGroup
    Friend g_ProgramDir As String = ""
    Friend Const g_AppNameShort As String = "TimeseriesUtility"
    Friend Const g_AppNameLong As String = "Timeseries Utility"

    Private pStatusMonitor As MonitorProgressStatus

    Public Sub New()
        'For Each lDir As String In IO.Directory.EnumerateDirectories("S:\dev\MW4.8.6") '"S:\dev\MW4.8.6\MapWindow4Dev\TableEditor") '"S:\dev\MW4.8.6\MapWindow4Dev") '"S:\dev\MapWindow4Plugins") '"S:\dev\BASINS40")
        '    For Each lProjectFileName As String In IO.Directory.GetFiles(lDir)
        '        If lProjectFileName.EndsWith("10.vbproj") Then
        '            Dim lProjectFileContents As String = IO.File.ReadAllText(lProjectFileName)
        '            lProjectFileContents = lProjectFileContents.Replace("10.vbproj", "13.vbproj")
        '            lProjectFileContents = lProjectFileContents.Replace("10""", "13""")
        '            lProjectFileContents = lProjectFileContents.Replace("10</Name>", "13</Name>")
        '            lProjectFileName = lProjectFileName.Substring(0, lProjectFileName.Length - 9) & "13.vbproj"
        '            IO.File.WriteAllText(lProjectFileName, lProjectFileContents)
        '            Logger.Dbg("Wrote " & lProjectFileName)
        '        End If
        '    Next
        'Next

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        g_ProgramDir = PathNameOnly(Reflection.Assembly.GetEntryAssembly.Location)
        Try
            Environment.SetEnvironmentVariable("PATH", g_ProgramDir & ";" & Environment.GetEnvironmentVariable("PATH"))
        Catch eEnv As Exception
        End Try
        If g_ProgramDir.EndsWith("bin") Then g_ProgramDir = PathNameOnly(g_ProgramDir)
        g_ProgramDir &= g_PathChar

        ShowHelp(g_ProgramDir & "TimeseriesUtility.chm")

        Dim lLogFolder As String = g_ProgramDir & "cache"
        If IO.Directory.Exists(lLogFolder) Then
            lLogFolder = lLogFolder & g_PathChar & "log" & g_PathChar
        Else
            lLogFolder = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "log") & g_PathChar
        End If

        Logger.StartToFile(lLogFolder & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-" & g_AppNameShort & ".log")
        Logger.Icon = Me.Icon
        If Logger.ProgressStatus Is Nothing OrElse Not (TypeOf (Logger.ProgressStatus) Is MonitorProgressStatus) Then
            'Start running status monitor to give better progress and status indication during long-running processes
            pStatusMonitor = New MonitorProgressStatus
            If pStatusMonitor.StartMonitor(FindFile("Find Status Monitor", "StatusMonitor.exe"), _
                                            g_ProgramDir, _
                                            System.Diagnostics.Process.GetCurrentProcess.Id) Then
                pStatusMonitor.InnerProgressStatus = Logger.ProgressStatus
                Logger.ProgressStatus = pStatusMonitor
                Logger.Status("LABEL TITLE " & g_AppNameLong & " Status")
                Logger.Status("PROGRESS TIME ON") 'Enable time-to-completion estimation
                Logger.Status("")
            Else
                pStatusMonitor.StopMonitor()
                pStatusMonitor = Nothing
            End If
        End If

        atcDataManager.Clear()
        With atcDataManager.DataPlugins
            .Add(New atcBasinsObsWQ.atcDataSourceBasinsObsWQ)
            .Add(New atcHspfBinOut.atcTimeseriesFileHspfBinOut)
            .Add(New atcWdmVb.atcWDMfile)
            .Add(New atcWDM.atcDataSourceWDM)
            .Add(New atcTimeseriesNCDC.atcTimeseriesNCDC)
            .Add(New atcTimeseriesRDB.atcTimeseriesRDB)
            .Add(New atcTimeseriesScript.atcTimeseriesScriptPlugin)
            .Add(New atcTimeseriesSUSTAIN.atcTimeseriesSUSTAIN)
            .Add(New atcTimeseriesGSSHA.atcTimeseriesGSSHA)
            '.Add(New atcTimeseriesCSV.atcTimeseriesCSV)

            .Add(New atcList.atcListPlugin)
            .Add(New atcGraph.atcGraphPlugin)
            .Add(New atcDataTree.atcDataTreePlugin)
        End With

        atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared()
        AddHandler (atcDataManager.OpenedData), (AddressOf FileOpenedOrClosed)
        AddHandler (atcDataManager.ClosedData), (AddressOf FileOpenedOrClosed)

        Try
            For Each lArg As String In My.Application.CommandLineArgs
                If IO.File.Exists(lArg) Then
                    atcDataManager.OpenDataSource(lArg)
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub frmMain_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Static Exiting As Boolean = False
        If Not Exiting Then
            Exiting = True
            Application.Exit()
        End If
    End Sub

    Private Sub frm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("Cover.html")
        End If
    End Sub

    Private Sub FileOpenedOrClosed(ByVal aDataSource As atcTimeseriesSource)
        Select Case atcDataManager.DataSources.Count
            Case Is < 1 : lblFile.Text = "No files are open" : SelectedData.Clear()
            Case 1 : lblFile.Text = atcDataManager.DataSources(0).Specification
            Case Else : lblFile.Text = atcDataManager.DataSources.Count & " files are open"
        End Select

        Dim lSelectedStillOpen As New atcTimeseriesGroup
        For Each lSelectedTS As atcTimeseries In SelectedData
            For Each lDataSource As atcTimeseriesSource In atcDataManager.DataSources
                If lDataSource.DataSets.Contains(lSelectedTS) Then
                    lSelectedStillOpen.Add(lSelectedTS)
                End If
            Next
        Next
        If lSelectedStillOpen.Count <> SelectedData.Count Then
            SelectedData = lSelectedStillOpen
        End If
        UpdateSelectedDataLabel()
    End Sub

    Private Sub SelectedData_Changed(ByVal aAdded As atcUtility.atcCollection) Handles SelectedData.Added, SelectedData.Removed
        UpdateSelectedDataLabel()
    End Sub

    Private Sub UpdateSelectedDataLabel()
        Dim lCountAllTimeseries As Integer = atcDataManager.DataSets.Count
        If lCountAllTimeseries < 1 Then
            lblDatasets.Text = "No timeseries are open"
            SelectedData.Clear()
        Else
            Select Case SelectedData.Count
                Case Is < 1 : lblDatasets.Text = "No timeseries are selected out of " & DoubleToString(atcDataManager.DataSets.Count)
                Case 1 : lblDatasets.Text = SelectedData(0).ToString
                Case Else
                    If SelectedData.Count = lCountAllTimeseries Then
                        lblDatasets.Text = "All " & SelectedData.Count & " timeseries are selected"
                    Else
                        lblDatasets.Text = SelectedData.Count & " of " & DoubleToString(atcDataManager.DataSets.Count) & " timeseries are selected"
                    End If
            End Select
        End If
        HideOrShowLogo()
        Logger.Dbg(lblDatasets.Text)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Dim lCollection As New ArrayList
        lCollection.Add("File")
        Dim lNewSource As atcTimeseriesSource = atcDataManager.UserSelectDataSource(lCollection, "Select type of file to open", True, False, Me.Icon)
        If Not lNewSource Is Nothing Then
            If Not (atcDataManager.OpenDataSource(lNewSource, lNewSource.Specification, Nothing)) Then
                If Logger.LastDbgText.Length > 0 Then
                    Logger.Msg(Logger.LastDbgText, "Data Open Problem")
                End If
            End If
        End If
    End Sub

    Private Sub btnManageFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManageFiles.Click
        atcDataManager.UserManage("Manage Files", -1, Me.Icon)
    End Sub

    Private Sub btnSelectData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectData.Click
        SelectData()
    End Sub

    Private Sub SelectData()
        SelectedData = atcDataManager.UserSelectData("Select Timeseries", SelectedData, Nothing, True, True, Me.Icon)
        UpdateSelectedDataLabel()
    End Sub

    Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnList.Click
        Dim lList As New atcList.atcListPlugin
        Dim lForm As atcList.atcListForm = lList.Show(SelectedData, Me.Icon)
     End Sub

    Private Sub btnGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGraph.Click
        Dim lGraph As New atcGraph.atcGraphPlugin
        Dim lForm As atcGraph.atcGraphForm = lGraph.Show(SelectedData, Me.Icon)
    End Sub

    Private Sub btnTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTree.Click
        Dim lTree As New atcDataTree.atcDataTreePlugin        
        Dim lForm As Windows.Forms.Form = lTree.Show(SelectedData, Me.Icon)
     End Sub

    Private Sub Form_DragEnter( _
        ByVal sender As Object, ByVal e As Windows.Forms.DragEventArgs) _
        Handles Me.DragEnter

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            e.Effect = Windows.Forms.DragDropEffects.All
        End If
    End Sub

    Private Sub Form_DragDrop( _
        ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles Me.DragDrop

        If e.Data.GetDataPresent(Windows.Forms.DataFormats.FileDrop) Then
            Dim lFileNames() As String = e.Data.GetData(Windows.Forms.DataFormats.FileDrop)

            If lFileNames.Count = 1 Then
                atcDataManager.OpenDataSource(lFileNames(0))
            Else
                Dim lFileIndex As Integer = 1
                For Each lFileName As String In lFileNames
                    Logger.Status("Opening file " & lFileIndex & " of " & lFileNames.Length)
                    Using lProgressLevel As New ProgressLevel
                        atcDataManager.OpenDataSource(lFileName)
                    End Using
                    lFileIndex += 1
                Next
            End If

            Logger.Status("")
        End If
    End Sub

    Private Sub btnSaveWDM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveWDM.Click
        If SelectedData.Count < 1 Then
            SelectData()
        End If

        If SelectedData.Count > 0 Then
            Dim lFormSave As New frmSaveData
            Dim lSaveSource As atcDataSource = lFormSave.AskUser(SelectedData)
            If lSaveSource IsNot Nothing AndAlso Not String.IsNullOrEmpty(lSaveSource.Specification) Then
                Logger.Status("Saving " & lSaveSource.Specification)
                lSaveSource.AddDataSets(SelectedData)
                Logger.Status("")
            End If
        End If
    End Sub

    Private Sub btnImportWDM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportWDM.Click
        Dim lFormImport As New atcWDM.frmImport
        lFormImport.Show()
    End Sub

    Private Sub btnDump_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDump.Click
        'Anurag Mishra: I had to comment out these lines for the script to work
        'Dim lHspfBinDataSource As atcDataSource = atcData.atcDataManager.DataSources(0)
        'Dim lNumDataSets As Integer = lHspfBinDataSource.DataSets.Count
        'Dim lDataSetIndex As Integer = 1
        'For Each lTscheck As atcTimeseries In lHspfBinDataSource.DataSets
        '    If lTscheck.Dates.Value(1) < 1 Then Stop
        '    Logger.Progress(lDataSetIndex, lNumDataSets) : lDataSetIndex += 1
        '    lTscheck.ValuesNeedToBeRead = True
        'Next

        'Anurag:  I am working on a script right now
        ' Sent at 3:41 PM on Wednesday
        'Anurag:  and it is taking long time at this step
        'If pAllAliases.TryGetValue(lNameLower, lAlias) Then 'We have a preferred alias for this name
        'aAttributeName = lAlias
        'in atcDataSttributes.vb

        Dim lSaveDialog As New Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save as..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                DumpToText(.FileName)
            End If
        End With
    End Sub

    Private Sub DumpToText(ByVal aFilename As String)
        Dim lMaxTS As Integer = 1000000
        Dim lNumTS As Integer = 0
        Dim lWriter As New IO.StreamWriter(aFilename, True, System.Text.Encoding.ASCII)

        For Each lFile As atcDataSource In atcDataManager.DataSources
            Dim lLastIndex As Integer = lFile.DataSets.Count
            Logger.Status("Writing " & Format(lLastIndex, "#,##0") & " datasets From " & IO.Path.GetFileName(lFile.Specification) & " to " & IO.Path.GetFileName(aFilename), True)
            Dim lIndex As Integer = 0
            For Each lDataSet As atcData.atcDataSet In lFile.DataSets
                WriteDataset(lDataSet, lWriter)
                lIndex += 1
                Logger.Progress(lIndex, lLastIndex)
                lNumTS += 1
                If lNumTS > lMaxTS Then
                    Logger.Progress(0, 0)
                    Exit For
                End If
            Next
            Logger.Dbg("Wrote " & lIndex & " of " & lFile.DataSets.Count & " Datasets From " & lFile.Specification)
        Next
        Logger.Status("")
        lWriter.Close()
    End Sub

    Private Sub WriteDataset(ByVal aDataSet As atcData.atcDataSet, ByVal aWriter As IO.StreamWriter)
        Dim lTimeseries As atcTimeseries = aDataSet
        Dim lNeededToBeRead As Boolean = lTimeseries.ValuesNeedToBeRead
        If lNeededToBeRead Then lTimeseries.EnsureValuesRead()

        aWriter.Write(lTimeseries.Attributes.ToString)
        'For Each lAttribute As atcDefinedValue In lTimeseries.Attributes.ValuesSortedByName
        '    If Not lAttribute.Definition.Calculated Then
        '        Dim lName As String = lAttribute.Definition.Name
        '        Select Case lName
        '            Case "Key", "Data Source"
        '            Case Else
        '                aWriter.Write(lName & " (" & lAttribute.Definition.TypeString & ") " & lAttribute.Value.ToString.TrimEnd & vbCrLf)
        '        End Select
        '    End If
        'Next
        'aWriter.Write("<end of attributes>")

        Dim lTimeUnits As atcTimeUnit = lTimeseries.Attributes.GetValue("tu", atcTimeUnit.TUUnknown)
        Dim lTimeStep As Integer = lTimeseries.Attributes.GetValue("ts", 1)
        Dim lDateEndComputed As Double = TimAddJ(lTimeseries.Dates.Value(0), lTimeUnits, lTimeStep, lTimeseries.numValues)
        If Math.Abs(lDateEndComputed - lTimeseries.Dates.Value(lTimeseries.numValues)) < 0.00001 Then
            aWriter.Write("Constant Interval " & lTimeseries.numValues & " values starting at " & DumpDate(lTimeseries.Dates.Values(0)) & vbCrLf)
        Else
            aWriter.Write("Non-constant interval " & lTimeseries.numValues & " values" & vbCrLf)
        End If
        Dim lValues() As Double = lTimeseries.Values
        Dim lDates() As Double = lTimeseries.Dates.Values
        Dim lValueAttributesExist As Boolean = lTimeseries.ValueAttributesExist
        Dim lValueAttributesString As String = ""
        For lIndex As Integer = 0 To lTimeseries.numValues
            If lValueAttributesExist Then
                lValueAttributesString = " " & lTimeseries.ValueAttributes(lIndex).ToString.Replace(vbTab, " = ").Replace(vbCrLf, ", ")
            End If
            aWriter.Write(DumpDate(lDates(lIndex)) & " " & DoubleToString(lValues(lIndex)) & lValueAttributesString & vbCrLf)
        Next
        'todo: write value attributes (if any)

        'If lNeededToBeRead Then lTimeseries.ValuesNeedToBeRead = True
    End Sub

    Private Sub btnCompare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompare.Click
        Dim lOpenDialog As New Windows.Forms.OpenFileDialog
        With lOpenDialog
            .Title = "Select Both Files..."
            .DefaultExt = "txt"
            .Filter = "Text Files|*.txt|All Files|*.*"
            .FilterIndex = 0
            .Multiselect = True
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                If .FileNames.Count = 2 Then
                    Dim lLinesInFile1 As IEnumerator = atcUtility.LinesInFile(.FileNames(0)).GetEnumerator
                    Dim lLinesInFile2 As IEnumerator = atcUtility.LinesInFile(.FileNames(1)).GetEnumerator
                    Dim lLineNumber As Long = 0
                    Try
                        Do
                            lLineNumber += 1
                            If Not lLinesInFile1.MoveNext() Then
                                If lLinesInFile2.MoveNext() Then
                                    Logger.Msg(.FileNames(0) & vbCrLf & .FileNames(1) & vbCrLf & "Reached end of file 1 after " & DoubleToString(CDbl(lLineNumber)) & " lines. " & vbCrLf & "File 2 next line was: " & lLinesInFile2.Current, "Files do not match")
                                Else
                                    Logger.Msg(.FileNames(0) & vbCrLf & .FileNames(1) & vbCrLf & "Reached end of both files. No mismatches in " & DoubleToString(CDbl(lLineNumber)) & " lines", "Files Match")
                                End If
                                Exit Do
                            End If
                            If Not lLinesInFile2.MoveNext() Then
                                Logger.Msg(.FileNames(0) & vbCrLf & .FileNames(1) & vbCrLf & "Reached end of file 2 after " & DoubleToString(CDbl(lLineNumber)) & " lines. " & vbCrLf & "File 1 next line was: " & lLinesInFile1.Current, "Files do not match")
                                Exit Do
                            End If
                            If lLinesInFile1.Current.Length = 0 Then
                                Stop
                            End If
Compare12:
                            If Not lLinesInFile1.Current.Equals(lLinesInFile2.Current) Then
                                Select Case lLinesInFile1.Current
                                    Case "Intvl (String) 0", "Intvl (String) 1", "&Scenario (String) SaladoCreek_HSPF10"
                                        lLinesInFile1.MoveNext()
                                        GoTo Compare12
                                End Select
                                Select Case lLinesInFile2.Current
                                    Case "Intvl (String) 0", "Intvl (String) 1", "&Scenario (String) SaladoCreek_HSPF10"
                                        lLinesInFile2.MoveNext()
                                        GoTo Compare12
                                End Select
                                Logger.Msg(lLinesInFile1.Current & vbCrLf & lLinesInFile2.Current, vbExclamation, "Files do not match at line " & lLineNumber)
                                Exit Do
                            End If
                        Loop
                    Catch ex As Exception
                        Logger.Dbg(ex.ToString)
                    End Try
                End If
            End If
        End With
    End Sub

    Private Sub btnSaveList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveList.Click
        If SelectedData.Count < 1 Then
            SelectData()
        End If

        If SelectedData.Count > 0 Then
            Dim lSaveDialog As New Windows.Forms.SaveFileDialog
            With lSaveDialog
                .Title = "Save as..."
                .DefaultExt = "txt"
                .Filter = "Text Files|*.txt|All Files|*.*"
                .FilterIndex = 0
                If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Dim lList As New atcList.atcListPlugin
                    lList.Save(SelectedData, .FileName)
                End If
            End With
        End If
    End Sub

    Private Sub btnGenerateMet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateMet.Click
        Dim lButtonForm As New frmSelectMet
        lButtonForm.ShowDialog(Me)
        Dim lComputationName As String = lButtonForm.Choice ' lButtonForm.AskUser("Select Computation", "VERTICAL", lButtonNames)
        If Not String.IsNullOrEmpty(lComputationName) Then ' <> lButtonForm.LabelCancel Then
            Dim lDataSource As New atcMetCmp.atcMetCmpPlugin
            lDataSource.Specification = lComputationName
            'TODO: use selected data as argument
            If atcDataManager.OpenDataSource(lDataSource, lDataSource.Specification, Nothing) Then
                If lDataSource.DataSets.Count > 0 Then
                    SelectedData.ChangeTo(lDataSource.DataSets)
                    Dim lTitle As String = lDataSource.ToString
                    atcDataManager.UserSelectDisplay(lTitle, lDataSource.DataSets)
                End If
            End If
        End If

    End Sub

    Private Sub btnGenerateMath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateMath.Click
        ChooseGenerate(New atcTimeseriesMath.atcTimeseriesMath)
    End Sub

    Sub ChooseGenerate(ByVal aDataSource As atcDataSource)
        Dim lButtonNames As New Generic.List(Of String)
        If aDataSource.Category <> "File" Then
            Dim lCategoryMenuName As String = atcDataManager.ComputeMenuName & "_" & aDataSource.Category
            Dim lOperations As atcDataAttributes = aDataSource.AvailableOperations
            If Not lOperations Is Nothing AndAlso lOperations.Count > 0 Then
                For Each lOperation As atcDefinedValue In lOperations
                    Select Case lOperation.Definition.TypeString
                        Case "atcTimeseries", "atcDataGroup", "atcTimeseriesGroup"
                            lButtonNames.Add(lOperation.Definition.Name)
                            Logger.Dbg(lOperation.Definition.Name & " - " & lOperation.Definition.Category)
                    End Select
                Next
            End If
        End If

        If lButtonNames.Count > 0 Then
            Dim lButtonForm As New atcControls.frmButtons()
            lButtonForm.Icon = Me.Icon
            Dim lComputationName As String = lButtonForm.AskUser("Select Computation", "VERTICAL", lButtonNames)
            If lComputationName <> lButtonForm.LabelCancel Then
                aDataSource.Specification = lComputationName
                If atcDataManager.OpenDataSource(aDataSource, aDataSource.Specification, Nothing) Then
                    If aDataSource.DataSets.Count > 0 Then
                        SelectedData.ChangeTo(aDataSource.DataSets)
                        Dim lTitle As String = aDataSource.ToString
                        atcDataManager.UserSelectDisplay(lTitle, aDataSource.DataSets)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        Try
            If My.Computer.Keyboard.ShiftKeyDown Then
                btnDump.Visible = Not btnDump.Visible
                btnCompare.Visible = Not btnCompare.Visible
                Exit Sub
            End If
        Catch
        End Try
        ShowHelp("Tutorial.html")
    End Sub

    Private Sub HideOrShowLogo()
        pictureLogo.Visible = (lblFile.Left + lblFile.Width < pictureLogo.Left AndAlso lblDatasets.Left + lblDatasets.Width < pictureLogo.Left)
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        HideOrShowLogo()
    End Sub

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        Dim lAbout As New frmAbout
        lAbout.Show()
    End Sub

End Class
