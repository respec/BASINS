Imports atcUtility
Imports atcData
Imports atcBatchProcessing
Imports MapWinUtility
Imports System.Windows.Forms

Public Class frmBatchMap
    Private pListStations As atcCollection
    Private pDataPath As String
    Private pBatchGroupCount As Integer = 0
    Private pBatchGroupCountSWSTAT As Integer = 0
    Private pBatchGroupCountDFLOW As Integer = 0
    Private pGlobalInputsBF As atcDataAttributes
    Private pGlobalInputsDFLOW As atcDataAttributes
    Private pGlobalInputsSWSTAT As atcDataAttributes
    Private pGroupsInputsBF As atcCollection 'Of atcDataAttributes per group
    Private pGroupsInputsDFLOW As atcCollection 'Of atcDataAttributes per group
    Private pGroupsInputsSWSTAT As atcCollection 'Of atcDataAttributes per group
    Private pBatchSpecFilefullname As String = ""

    Private WithEvents pfrmParams As frmSWSTAT
    Public BatchAnalysis As clsBatch.ANALYSIS

    Private ReadOnly Property GetDataFileFullPath(ByVal aStationId As String) As String
        Get
            If IO.Directory.Exists(pDataPath) Then
                Return IO.Path.Combine(pDataPath, "NWIS\NWIS_discharge_" & aStationId & ".rdb")
            Else
                Return ""
            End If
        End Get
    End Property
    Public Sub Initiate(ByVal aList As atcCollection)
        pListStations = aList
        If pListStations Is Nothing Then pListStations = New atcCollection()
    End Sub

    Private Sub frmBatchMap_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IO.Directory.Exists(txtDataDir.Text) Then
            SaveSetting("atcUSGSBaseflow", "Default", "DataDir", txtDataDir.Text)
        End If
    End Sub

    Private Sub frmBatchMap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lstStations.LeftLabel = "Stations from map"
        lstStations.RightLabel = "Selected for a group"
        Dim lindex As Integer = 0
        If pListStations IsNot Nothing AndAlso pListStations.Count > 0 Then
            Dim lStnIDinKey As Boolean = True
            If IsNumeric(pListStations.ItemByIndex(0)) AndAlso Not IsNumeric(pListStations.Keys.Item(0)) Then
                lStnIDinKey = False
            End If
            For Each lStationID As String In pListStations.Keys
                If lStnIDinKey Then
                    lstStations.LeftItem(lindex) = lStationID
                Else
                    lstStations.LeftItem(lindex) = pListStations.ItemByKey(lStationID)
                End If
                lindex += 1
            Next
        End If

        pGlobalInputsBF = New atcDataAttributes()
        pGroupsInputsBF = New atcCollection()

        pGlobalInputsDFLOW = New atcDataAttributes()
        pGlobalInputsSWSTAT = New atcDataAttributes()
        pGroupsInputsDFLOW = New atcCollection()
        pGroupsInputsSWSTAT = New atcCollection()

        Dim lDataDir As String = GetSetting("atcUSGSBaseflow", "Default", "DataDir", "")
        If IO.Directory.Exists(lDataDir) Then
            txtDataDir.Text = lDataDir
        End If
    End Sub

    Private Sub btnCreateGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateGroup.Click
        Dim lAnalysis As String = BatchAnalysis.ToString()
        Dim lCount As Integer = 0
        Select Case BatchAnalysis
            Case clsBatch.ANALYSIS.SWSTAT
                pBatchGroupCountSWSTAT += 1
                lCount = pBatchGroupCountSWSTAT
            Case clsBatch.ANALYSIS.DFLOW
                pBatchGroupCountDFLOW += 1
                lCount = pBatchGroupCountDFLOW
        End Select
        If lstStations.RightCount = 0 OrElse String.IsNullOrEmpty(lAnalysis) Then
            Exit Sub
        End If
        Dim listStations As New List(Of String)
        Dim lNewNodeText As String = "BatchGroup_" & lAnalysis & "_" & lCount
        Dim lnewTreeNode As New Windows.Forms.TreeNode(lNewNodeText)
        treeBFGroups.Nodes.Add(lnewTreeNode)
        For I As Integer = 0 To lstStations.RightCount - 1
            With lnewTreeNode
                .Nodes.Add(lstStations.RightItem(I))
            End With
            listStations.Add(lstStations.RightItem(I))
        Next
        Dim lBatchInputs As atcDataAttributes = GetGroupParams(lNewNodeText, False)
        Dim lDataGroup As atcTimeseriesGroup = BuildTserGroup(listStations)
        Dim lStationsInfo As atcCollection = clsBatchUtil.BuildStationsInfo(lDataGroup)
        lBatchInputs.SetValue(InputNames.StationsInfo, lStationsInfo)
        pBatchGroupCount += 1
    End Sub

    Private Sub treeBFGroups_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles treeBFGroups.MouseUp
        ' Point where mouse is clicked
        Dim p As System.Drawing.Point = New System.Drawing.Point(e.X, e.Y)
        ' Go to the node that the user clicked
        Dim node As TreeNode = treeBFGroups.GetNodeAt(p)
        If node IsNot Nothing Then
            treeBFGroups.SelectedNode = node
            ' Show menu only if Right Mouse button is clicked
            If e.Button = Windows.Forms.MouseButtons.Right Then
                cmsNode.Show(treeBFGroups, p)
            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                Dim lGroupName As String = node.Text
                If Not lGroupName.StartsWith("BatchGroup") Then
                    lGroupName = node.Parent.Text
                End If
                Dim lArgs As atcDataAttributes = pGroupsInputsBF.ItemByKey(lGroupName)
                If lArgs IsNot Nothing Then
                    'txtParameters.Text = ParametersToText(lArgs)
                End If
            End If
        End If
    End Sub

    Private Sub cmsNode_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmsNode.ItemClicked
        Dim lcmdName As String = e.ClickedItem.Name

        'ToDo: Put in Buttons
        'Dim p As System.Drawing.Point = New System.Drawing.Point(e.ClickedItem.Bounds.Location.X, e.ClickedItem.Bounds.Location.Y)
        '' Go to the node that the user clicked
        'Dim node As TreeNode = treeBFGroups.GetNodeAt(p)
        Dim node As TreeNode = treeBFGroups.SelectedNode
        Dim lFullpath As String = node.FullPath
        Dim lGroupingName As String = "BatchGroup"
        Select Case lcmdName
            Case "cmsRemove"
                If node.Text.StartsWith(lGroupingName) Then
                    RemoveBFGroup(node)
                Else
                    If node.Parent IsNot Nothing Then
                        If node.Parent.Nodes.Count = 1 Then
                            RemoveBFGroup(node)
                        Else
                            Dim lGroupName As String = node.Parent.Text
                            Dim lStationID As String = node.Text
                            treeBFGroups.Nodes.Remove(node)

                            Dim lBFGroupAttribs As atcDataAttributes = pGroupsInputsBF.ItemByKey(lGroupName)
                            If lBFGroupAttribs IsNot Nothing Then
                                Dim lStationInfo As ArrayList = lBFGroupAttribs.GetValue(GlobalInputNames.StationsInfo)
                                If lStationInfo IsNot Nothing Then
                                    Dim lIndexToRemove As Integer = -99
                                    For Each lStation As String In lStationInfo
                                        If lStation.Contains(lStationID) Then
                                            lIndexToRemove = lStationInfo.IndexOf(lStation)
                                            Exit For
                                        End If
                                    Next
                                    If lIndexToRemove >= 0 Then
                                        lStationInfo.RemoveAt(lIndexToRemove)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Case "cmsPlotDur"
                Dim lTsGroup As New atcTimeseriesGroup()
                Dim lArgs As New atcDataAttributes()
                lArgs.Add("Constituent", "streamflow,flow")
                If Not node.Text.StartsWith(lGroupingName) Then
                    node = node.Parent()
                End If
                For Each lStationNode As TreeNode In node.Nodes
                    Dim lstationId As String = lStationNode.Text
                    Dim lDataPath As String = GetDataFileFullPath(lstationId)
                    Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                    If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                        lTsGroup.Add(lTsGroupTemp(0))
                    End If
                Next
                If lTsGroup.Count > 0 Then
                    atcUSGSUtility.atcUSGSScreen.GraphDataDuration(lTsGroup)
                Else
                    Logger.Msg("Need to download data first.", "Batch Map:Plot")
                End If
            Case "cmsGroupSetParm"
                Dim lGroupName As String = ""
                Dim lGroupNode As TreeNode
                If node.Text.StartsWith(lGroupingName) Then
                    lGroupNode = node
                Else
                    lGroupNode = node.Parent
                End If
                lGroupName = lGroupNode.Text

                Dim lIndex As Integer = lGroupName.LastIndexOf("_")
                Dim lGroupNum As Integer = Integer.Parse(lGroupName.Substring(lIndex + 1))
                Dim lBatchInputs As atcDataAttributes = GetGroupParams(lGroupName, True)
                'If lGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
                '    lBatchInputs = pGroupsInputsSWSTAT.ItemByKey(lGroupName)
                'ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                '    lBatchInputs = pGroupsInputsDFLOW.ItemByKey(lGroupName)
                'Else
                '    lBatchInputs = pGroupsInputsBF.ItemByKey(lGroupName)
                'End If

                Dim lTsGroup As atcTimeseriesGroup = BuildTserGroup(lGroupNode)
                If lTsGroup.Count > 0 Then
                    pfrmParams = New atcIDF.frmSWSTAT()
                    pfrmParams.BatchAnalysis = BatchAnalysis
                    pfrmParams.Initialize(lTsGroup, lBatchInputs)
                    'pfrmParameters = pfrmParameters.GetType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                    'pfrmParameters.GetType.InvokeMember("Initialize", Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {lTsGroup, lBatchInputs})
                    If lGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
                    End If
                End If
            Case "cmsGlobalSetParm"
                Dim lNodeText As String = node.Text
                pfrmParams = New atcIDF.frmSWSTAT()
                pfrmParams.BatchAnalysis = BatchAnalysis
                If lNodeText.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
                    pGlobalInputsSWSTAT.SetValue("Operation", "GlobalSetParm")
                    atcIDF.modUtil.InputNames.BuildInputSet(pGlobalInputsSWSTAT, Nothing)
                    Dim lNDay() As Double = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.NDay, Nothing)
                    Dim lNDays As atcCollection = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.NDays, Nothing)
                    Dim lRP() As Double = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.ReturnPeriod, Nothing)
                    Dim lRPs As atcCollection = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.ReturnPeriods, Nothing)
                    pfrmParams.Initialize(Nothing, pGlobalInputsSWSTAT)
                Else
                    pGlobalInputsDFLOW.SetValue("Operation", "GlobalSetParm")
                    pfrmParams.Initialize(Nothing, pGlobalInputsDFLOW)
                End If
        End Select
    End Sub

    Private Function GetGroupParams(ByVal aGroupName As String, Optional ByVal aUseGlobalDefault As Boolean = False) As atcDataAttributes
        Dim lBatchInputs As atcDataAttributes = pGroupsInputsSWSTAT.ItemByKey(aGroupName)
        Select Case BatchAnalysis
            Case clsBatch.ANALYSIS.DFLOW
                lBatchInputs = pGroupsInputsDFLOW.ItemByKey(aGroupName)
            Case clsBatch.ANALYSIS.SWSTAT
                lBatchInputs = pGroupsInputsSWSTAT.ItemByKey(aGroupName)
            Case Else
                '    lBatchInputs = pGroupsInputsBF.ItemByKey(lGroupName)
        End Select
        If lBatchInputs Is Nothing Then
            lBatchInputs = New atcDataAttributes()
            lBatchInputs.SetValue("Operation", "GroupSetParm")
            lBatchInputs.SetValue("Group", aGroupName)

            If aGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
                pGroupsInputsSWSTAT.Add(aGroupName, lBatchInputs)
            ElseIf aGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                pGroupsInputsDFLOW.Add(aGroupName, lBatchInputs)
            Else
                pGroupsInputsBF.Add(aGroupName, lBatchInputs)
            End If
        End If

        If aUseGlobalDefault Then
            'Try to use global setting as much as possible
            If aGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
                atcIDF.modUtil.InputNames.BuildInputSet(lBatchInputs, pGlobalInputsSWSTAT)
            ElseIf aGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                atcIDF.modUtil.InputNamesDFLOW.BuildInputSet(lBatchInputs, pGlobalInputsDFLOW)
            Else

            End If
        End If
        Return lBatchInputs
    End Function

    Private Function BuildTserGroup(ByVal aGroupNode As System.Windows.Forms.TreeNode) As atcTimeseriesGroup
        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow,flow")
        Dim lTsGroup As New atcTimeseriesGroup()
        For Each lStationNode As System.Windows.Forms.TreeNode In aGroupNode.Nodes
            Dim lstationId As String = lStationNode.Text

            Dim lDataLoaded As Boolean = False
            For Each lDS As atcDataSource In atcDataManager.DataSources
                If lDS.Name.ToString.Contains("USGS RDB") Then
                    Dim lTsCons As String = ""
                    For Each lTs As atcTimeseries In lDS.DataSets
                        lTsCons = lTs.Attributes.GetValue("Constituent").ToString()
                        If lTs.Attributes.GetValue("Location") = lstationId AndAlso
                           (lTsCons.ToLower = "streamflow" OrElse lTsCons.ToLower() = "flow") Then
                            lTsGroup.Add(lTs)
                            lDataLoaded = True
                            Exit For
                        End If
                    Next
                    If lDataLoaded Then
                        Exit For
                    End If
                End If
            Next
            If Not lDataLoaded Then
                Dim lDataPath As String = GetDataFileFullPath(lstationId)
                Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                    lTsGroup.Add(lTsGroupTemp(0).Clone)
                End If
            End If
        Next
        Return lTsGroup
    End Function

    Private Function BuildTserGroup(ByVal aListStations As List(Of String)) As atcTimeseriesGroup
        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow,flow")
        Dim lTsGroup As New atcTimeseriesGroup()
        For Each lStationID As String In aListStations
            Dim lDataLoaded As Boolean = False
            For Each lDS As atcDataSource In atcDataManager.DataSources
                If lDS.Name.ToString.Contains("USGS RDB") Then
                    Dim lTsCons As String = ""
                    For Each lTs As atcTimeseries In lDS.DataSets
                        lTsCons = lTs.Attributes.GetValue("Constituent").ToString()
                        If lTs.Attributes.GetValue("Location") = lStationID AndAlso
                           (lTsCons.ToLower = "streamflow" OrElse lTsCons.ToLower() = "flow") Then
                            lTsGroup.Add(lTs)
                            lDataLoaded = True
                            Exit For
                        End If
                    Next
                    If lDataLoaded Then
                        Exit For
                    End If
                End If
            Next
            If Not lDataLoaded Then
                Dim lDataPath As String = GetDataFileFullPath(lStationID)
                Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                    lTsGroup.Add(lTsGroupTemp(0).Clone)
                End If
            End If
        Next
        Return lTsGroup
    End Function

    ''' <summary>
    ''' Only call when removing a whole group
    ''' </summary>
    ''' <param name="aBFGroupNode"></param>
    ''' <remarks></remarks>
    Private Sub RemoveBFGroup(ByVal aBFGroupNode As TreeNode)
        Dim lIndex As Integer = aBFGroupNode.Text.LastIndexOf("_")
        Dim lGroupingName As String = aBFGroupNode.Text.Substring(0, lIndex + 1) '"BatchGroup"
        Dim lGroupNum As Integer = 0
        If Not Integer.TryParse(aBFGroupNode.Text.Substring(lGroupingName.Length), lGroupNum) Then
            Exit Sub
        End If

        pGroupsInputsSWSTAT.RemoveByKey(aBFGroupNode.Text)
        treeBFGroups.Nodes.Remove(aBFGroupNode)
        pBatchGroupCount -= 1
        If lGroupingName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
            pBatchGroupCountSWSTAT -= 1
        ElseIf lGroupingName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
            pBatchGroupCountDFLOW -= 1
        End If
        Dim loldNodeText As String = ""
        Dim loldNodeIndex As Integer
        For Each lNode As TreeNode In treeBFGroups.Nodes
            If lNode.Text.StartsWith(lGroupingName) Then
                loldNodeText = lNode.Text
                loldNodeIndex = pGroupsInputsBF.Keys.IndexOf(loldNodeText)
                Dim lGroupNumRemains As Integer = Integer.Parse(lNode.Text.Substring(lGroupingName.Length))
                If lGroupNumRemains > lGroupNum Then
                    Dim lNewNodeText As String = lGroupingName & (lGroupNumRemains - 1).ToString
                    lNode.Text = lNewNodeText
                    If loldNodeIndex >= 0 Then pGroupsInputsBF.Keys.Item(loldNodeIndex) = lNewNodeText
                End If
            End If
        Next
    End Sub

    Private Sub ParmetersSetSWSTAT(ByVal aArgs As atcDataAttributes) Handles pfrmParams.ParametersSetSWSTAT
        Dim lText As String = InputNames.ParametersToText(aArgs)

        If String.IsNullOrEmpty(lText) Then
            txtParameters.Text = ""
        Else
            'Dim loperation As String = aArgs.GetValue("Operation", "")
            'Dim lgroupname As String = aArgs.GetValue("Group", "")
            'Dim lArg As atcDataAttributes = Nothing
            'If loperation.ToLower = "groupsetparm" Then
            '    lArg = pGroupsInputsBF.ItemByKey(lgroupname)
            '    If lArg Is Nothing Then
            '        lArg = New atcDataAttributes()
            '        pGroupsInputsBF.Add(lgroupname, lArg)
            '    End If
            'Else
            '    lArg = pGlobalInputsBF
            'End If
            'For Each lDataDef As atcDefinedValue In aArgs
            '    lArg.SetValue(lDataDef.Definition.Name, lDataDef.Value)
            'Next
            txtParameters.Text = lText.ToString()
        End If
    End Sub

    Private Sub ParmetersSetDFLOW(ByVal aArgs As atcDataAttributes) Handles pfrmParams.ParametersSetDFLOW
        Dim lText As String = InputNamesDFLOW.ParametersToText(aArgs)

        If String.IsNullOrEmpty(lText) Then
            txtParameters.Text = ""
        Else
            'Dim loperation As String = aArgs.GetValue("Operation", "")
            'Dim lgroupname As String = aArgs.GetValue("Group", "")
            'Dim lArg As atcDataAttributes = Nothing
            'If loperation.ToLower = "groupsetparm" Then
            '    lArg = pGroupsInputsBF.ItemByKey(lgroupname)
            '    If lArg Is Nothing Then
            '        lArg = New atcDataAttributes()
            '        pGroupsInputsBF.Add(lgroupname, lArg)
            '    End If
            'Else
            '    lArg = pGlobalInputsBF
            'End If
            'For Each lDataDef As atcDefinedValue In aArgs
            '    lArg.SetValue(lDataDef.Definition.Name, lDataDef.Value)
            'Next
            txtParameters.Text = lText
        End If
    End Sub

    Private Sub btnBrowseDataDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDataDir.Click
        Dim lFolder As New System.Windows.Forms.FolderBrowserDialog()
        If IO.Directory.Exists(pDataPath) Then
            lFolder.SelectedPath = pDataPath
        End If
        If lFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDataDir.Text = lFolder.SelectedPath
            If IO.Directory.Exists(txtDataDir.Text) Then
                pDataPath = txtDataDir.Text
            End If
        End If
        If pListStations.Count = 0 Then
            TryPopulateStationListFromDataPath()
        End If
    End Sub

    Private Sub TryPopulateStationListFromDataPath()
        Dim lStationFile As String = IO.Path.Combine(pDataPath, "Stations.txt")
        Dim listStations As New atcCollection()
        If IO.File.Exists(lStationFile) Then
            Dim lSR As IO.StreamReader = Nothing
            Try
                lSR = New IO.StreamReader(lStationFile)
                Dim line As String = ""
                While Not lSR.EndOfStream
                    line = lSR.ReadLine()
                    If Not String.IsNullOrEmpty(line) AndAlso IsNumeric(line) Then
                        If Not String.IsNullOrEmpty(line.Trim()) Then
                            listStations.Add(line)
                        End If
                    End If
                End While

            Catch ex As Exception

            Finally
                If lSR IsNot Nothing Then
                    lSR.Close() : lSR = Nothing
                End If
            End Try
        End If

        'Now get all rdbs from the same folder
        Dim lFiles As New System.Collections.Specialized.NameValueCollection()
        AddFilesInDir(lFiles, IO.Path.Combine(pDataPath, "NWIS"), False, "*.rdb")
        For Each lFile As String In lFiles
            Dim lName As String = FilenameNoExt(FilenameNoPath(lFile)).ToLower()
            Dim lStationID As String = ""
            If lName.StartsWith("nwis_stations_") Then
                lStationID = lName.Substring("nwis_stations_".Length)
            ElseIf lName.StartsWith("nwis_discharge_") Then
                lStationID = lName.Substring("nwis_discharge_".Length)
            End If
            If Not String.IsNullOrEmpty(lStationID) AndAlso
               lStationID.Length = 8 AndAlso
               Not listStations.Contains(lStationID) Then
                listStations.Add(lStationID)
            End If
        Next

        For Each lStationID As String In listStations
            pListStations.Add(lStationID)
        Next
        Dim lindex As Integer = 0
        For Each lStationID As String In pListStations
            lstStations.LeftItem(lindex) = lStationID
            lindex += 1
        Next
    End Sub
    Private Sub txtDataDir_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataDir.TextChanged
        If IO.Directory.Exists(txtDataDir.Text.Trim()) Then
            pDataPath = txtDataDir.Text.Trim()
        End If
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If Not String.IsNullOrEmpty(pDataPath) AndAlso IO.Directory.Exists(pDataPath) Then
            Dim lStationsNeedDownload As New atcCollection()
            For Each lStationID As String In pListStations.Keys
                If Not IO.File.Exists(GetDataFileFullPath(lStationID)) Then
                    lStationsNeedDownload.Add(lStationID, lStationID)
                End If
            Next
            clsBatchUtil.SiteInfoDir = pDataPath
            Try
                If lStationsNeedDownload.Count > 0 Then
                    clsBatchUtil.DownloadData(lStationsNeedDownload)
                Else
                    Logger.Msg("Data files already exists in " & pDataPath, "Batch Map:Download")
                End If
            Catch ex As Exception
                Logger.Msg("Some issues occurred during download, please examine the RDB files in data directory:" & vbCrLf &
                           pDataPath, "Batch Map")
                Return
            End Try
        Else
            Logger.Msg("Please specify a directory for data download.")
        End If
    End Sub

    Private Sub btnPlotDuration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlotDuration.Click
        Dim lTsGroup As New atcTimeseriesGroup()
        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow,flow")
        For I As Integer = 0 To lstStations.RightCount - 1
            Dim lstationId As String = lstStations.RightItem(I)
            Dim lDataPath As String = GetDataFileFullPath(lstationId)
            Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
            If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                If String.Compare(lTsGroupTemp(0).Attributes.GetValue("Constituent").ToString(), "flow", True) = 0 Then
                    lTsGroupTemp(0).Attributes.SetValue("Constituent", "Streamflow")
                End If
                lTsGroup.Add(lTsGroupTemp(0))
            End If
        Next
        If lTsGroup.Count > 0 Then
            atcUSGSUtility.atcUSGSScreen.GraphDataDuration(lTsGroup)
        End If
    End Sub

    Private Sub btnDoBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDoBatch.Click
        'If pGroupsInputsSWSTAT.Count = 0 AndAlso pGroupsInputsDFLOW.Count = 0 Then
        '    'Logger.Msg("Need to specify global default parameters.", "Batch Run Parameter Setup")
        '    Logger.Msg("Need to set up batch run groups.", "Batch Run Parameter Setup")
        '    Exit Sub
        'End If
        'If pGlobalInputsSWSTAT.Count = 0 Then
        '    Logger.Msg("Need to specify global default parameters.", "Batch Map Base-flow Separation")
        '    Return
        'End If
        'If Not String.IsNullOrEmpty(pBatchSpecFilefullname) AndAlso IO.File.Exists(pBatchSpecFilefullname) Then
        'Else
        '    Logger.Msg("Need to construct a batch specification file first.", "Batch Base-flow Separation")
        'End If
        Dim lfrmBatch As New frmBatch()
        lfrmBatch.BatchSpecFile = pBatchSpecFilefullname
        lfrmBatch.BatchAnalysis = BatchAnalysis
        lfrmBatch.Text = "Batch Run: " & BatchAnalysis.ToString() & " Analysis"
        lfrmBatch.ShowDialog()
    End Sub

    Private Sub btnSaveSpecs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSpecs.Click
        If pGlobalInputsSWSTAT.Count = 0 AndAlso pGlobalInputsDFLOW.Count = 0 Then
            Logger.Msg("Need to specify global default parameters.", "Batch Run Parameter Setup")
            Exit Sub
        End If
        If pGroupsInputsSWSTAT.Count = 0 AndAlso pGroupsInputsDFLOW.Count = 0 Then
            Logger.Msg("Need to set up batch run groups.", "Batch Run Parameter Setup")
            Exit Sub
        End If
        If pGlobalInputsSWSTAT.Count > 0 AndAlso pGroupsInputsSWSTAT.Count > 0 Then
            Dim lSpecDirSWSTAT As String = pGlobalInputsSWSTAT.GetValue(InputNames.OutputDir, "")
            If String.IsNullOrEmpty(lSpecDirSWSTAT) Then
                lSpecDirSWSTAT = pDataPath
            End If
            pBatchSpecFilefullname = IO.Path.Combine(lSpecDirSWSTAT, "BatchRun_SWSTAT_" & SafeFilename(DateTime.Now) & ".txt")
            SaveBatchFile(pBatchSpecFilefullname, clsBatch.ANALYSIS.SWSTAT, pGlobalInputsSWSTAT, pGroupsInputsSWSTAT)
        End If
        If pGlobalInputsDFLOW.Count > 0 AndAlso pGroupsInputsDFLOW.Count > 0 Then
            Dim lSpecDirDFLOW As String = pGlobalInputsDFLOW.GetValue(InputNamesDFLOW.OutputDir, "")
            If String.IsNullOrEmpty(lSpecDirDFLOW) Then
                lSpecDirDFLOW = pDataPath
            End If
            pBatchSpecFilefullname = IO.Path.Combine(lSpecDirDFLOW, "BatchRun_DFLOW_" & SafeFilename(DateTime.Now) & ".txt")
            SaveBatchFile(pBatchSpecFilefullname, clsBatch.ANALYSIS.DFLOW, pGlobalInputsDFLOW, pGroupsInputsDFLOW)
        End If
        pBatchSpecFilefullname = ""
    End Sub

    Private Sub SaveBatchFile(ByVal aFilename As String, ByVal aAnalysis As clsBatch.ANALYSIS,
                              ByVal aCommonSetting As atcDataAttributes, ByVal aGroupSettings As atcCollection)
        Dim lSW As IO.StreamWriter = Nothing
        Dim lTitle As String = ""
        If aAnalysis = clsBatch.ANALYSIS.SWSTAT Then
            lTitle = "Save SWSTAT Batch File"
        ElseIf aAnalysis = clsBatch.ANALYSIS.DFLOW Then
            lTitle = "Save DFLOW Batch File"
        End If
        Try
            lSW = New IO.StreamWriter(aFilename, False)
            'write global block first
            Select Case aAnalysis
                Case clsBatch.ANALYSIS.SWSTAT
                    lSW.WriteLine(InputNames.ParametersToText(aCommonSetting))
                Case clsBatch.ANALYSIS.DFLOW
                    lSW.WriteLine(InputNamesDFLOW.ParametersToText(aCommonSetting))
            End Select
            lSW.WriteLine("")
            'write each group settings
            For Each lGroupAttrib As atcDataAttributes In aGroupSettings
                Dim lStationsInfo As atcCollection = lGroupAttrib.GetValue(GlobalInputNames.StationsInfo)
                If lStationsInfo Is Nothing Then
                    Dim lGroupName As String = lGroupAttrib.GetValue("Group", "")
                    If Not String.IsNullOrEmpty(lGroupName) Then
                        Dim lGroupNode As TreeNode = Nothing
                        For Each lBatchGroupNode As TreeNode In treeBFGroups.Nodes
                            If String.Compare(lGroupName, lBatchGroupNode.Text, True) = 0 Then
                                lGroupNode = lBatchGroupNode
                                Exit For
                            End If
                        Next
                        If lGroupNode IsNot Nothing Then
                            Dim lTsGroup As atcTimeseriesGroup = BuildTserGroup(lGroupNode)
                            lStationsInfo = clsBatchUtil.BuildStationsInfo(lTsGroup)
                        End If
                    End If
                    lGroupAttrib.SetValue(GlobalInputNames.StationsInfo, lStationsInfo)
                End If
                Select Case aAnalysis
                    Case clsBatch.ANALYSIS.SWSTAT
                        lSW.WriteLine(InputNames.ParametersToText(lGroupAttrib))
                    Case clsBatch.ANALYSIS.DFLOW
                        lSW.WriteLine(InputNamesDFLOW.ParametersToText(lGroupAttrib))
                End Select
                lSW.WriteLine("")
            Next
            Logger.Msg("Batch file is saved:" & vbCrLf & aFilename, lTitle)
        Catch ex As Exception
            Logger.Msg("Error Writing Spec File:" & vbCrLf & aFilename & vbCrLf & vbCrLf & ex.Message, lTitle)
        Finally
            If lSW IsNot Nothing Then
                lSW.Close()
                lSW = Nothing
            End If
        End Try
    End Sub
    Private Sub btnParmForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParmForm.Click
        Dim lGroupNode As TreeNode = treeBFGroups.SelectedNode
        If lGroupNode Is Nothing Then Return

        Dim lGroupName As String = lGroupNode.Text
        If Not lGroupName.StartsWith("BatchGroup") Then
            lGroupName = lGroupNode.Parent.Text
            lGroupNode = lGroupNode.Parent
        End If
        Dim lAnalysis As clsBatch.ANALYSIS = clsBatch.ANALYSIS.DFLOW
        If lGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
            lAnalysis = clsBatch.ANALYSIS.SWSTAT
        End If
        Dim lArgs As atcDataAttributes = pGroupsInputsBF.ItemByKey(lGroupName)
        If lArgs IsNot Nothing Then
            lArgs.SetValue("Constituent", "streamflow,flow")
            Dim lCon As String = ""
            Dim loc As String = ""
            Dim lTsGroup As New atcTimeseriesGroup()
            For Each lStationNode As TreeNode In lGroupNode.Nodes
                Dim lstationId As String = lStationNode.Text

                Dim lDataLoaded As Boolean = False
                For Each lDS As atcDataSource In atcDataManager.DataSources
                    If lDS.Name.ToString.Contains("USGS RDB") Then
                        For Each lTs As atcTimeseries In lDS.DataSets
                            lCon = lTs.Attributes.GetValue("Constituent")
                            loc = lTs.Attributes.GetValue("Location")
                            If String.IsNullOrEmpty(lCon) Then
                                Continue For
                            End If
                            If loc = lstationId AndAlso
                               (lCon.ToString.ToLower = "streamflow" OrElse lCon.ToString.ToLower = "flow") Then
                                lTsGroup.Add(lTs)
                                lDataLoaded = True
                            End If
                        Next
                    End If
                Next
                If Not lDataLoaded Then
                    Dim lDataPath As String = GetDataFileFullPath(lstationId)
                    Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                    If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                        lTsGroup.Add(lTsGroupTemp(0).Clone)
                    End If
                End If
            Next
            If lTsGroup.Count > 0 Then
                Select Case lAnalysis
                    Case clsBatch.ANALYSIS.DFLOW
                    Case clsBatch.ANALYSIS.SWSTAT
                End Select
            End If
        End If
    End Sub

    Private Sub btnGroupGlobal_Click(sender As Object, e As EventArgs) Handles btnGroupGlobal.Click
        If treeBFGroups.SelectedNode Is Nothing Then Exit Sub
        Dim lnode As TreeNode = treeBFGroups.SelectedNode
        Dim lNodeText As String = lnode.Text
        pfrmParams = New atcIDF.frmSWSTAT()
        pfrmParams.BatchAnalysis = BatchAnalysis
        If lNodeText.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
            pGlobalInputsSWSTAT.SetValue("Operation", "GlobalSetParm")
            atcIDF.modUtil.InputNames.BuildInputSet(pGlobalInputsSWSTAT, Nothing)
            Dim lNDay() As Double = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.NDay, Nothing)
            Dim lNDays As atcCollection = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.NDays, Nothing)
            Dim lRP() As Double = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.ReturnPeriod, Nothing)
            Dim lRPs As atcCollection = pGlobalInputsSWSTAT.GetValue(atcIDF.modUtil.InputNames.ReturnPeriods, Nothing)
            pfrmParams.Initialize(Nothing, pGlobalInputsSWSTAT)
        Else
            pGlobalInputsDFLOW.SetValue("Operation", "GlobalSetParm")
            atcIDF.modUtil.InputNamesDFLOW.BuildInputSet(pGlobalInputsDFLOW, Nothing)
            'Dim lNDay() As Double = pGlobalInputsDFLOW.GetValue(atcIDF.modUtil.InputNamesDFLOW.NDay, Nothing)
            'Dim lNDays As atcCollection = pGlobalInputsDFLOW.GetValue(atcIDF.modUtil.InputNamesDFLOW.NDays, Nothing)
            'Dim lRP() As Double = pGlobalInputsDFLOW.GetValue(atcIDF.modUtil.InputNamesDFLOW.ReturnPeriod, Nothing)
            'Dim lRPs As atcCollection = pGlobalInputsDFLOW.GetValue(atcIDF.modUtil.InputNamesDFLOW.ReturnPeriods, Nothing)
            pfrmParams.Initialize(Nothing, pGlobalInputsDFLOW)
        End If
    End Sub

    Private Sub btnGroupGroup_Click(sender As Object, e As EventArgs) Handles btnGroupGroup.Click
        If treeBFGroups.SelectedNode Is Nothing Then Exit Sub
        Dim lnode As TreeNode = treeBFGroups.SelectedNode
        Dim lGroupingName As String = "BatchGroup"
        Dim lGroupName As String = ""
        Dim lGroupNode As TreeNode
        If lnode.Text.StartsWith(lGroupingName) Then
            lGroupNode = lnode
        Else
            lGroupNode = lnode.Parent
        End If
        lGroupName = lGroupNode.Text

        Dim lIndex As Integer = lGroupName.LastIndexOf("_")
        Dim lGroupNum As Integer = Integer.Parse(lGroupName.Substring(lIndex + 1))
        Dim lBatchInputs As atcDataAttributes = GetGroupParams(lGroupName, True)
        'If lGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
        '    lBatchInputs = pGroupsInputsSWSTAT.ItemByKey(lGroupName)
        'ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
        '    lBatchInputs = pGroupsInputsDFLOW.ItemByKey(lGroupName)
        'Else
        '    lBatchInputs = pGroupsInputsBF.ItemByKey(lGroupName)
        'End If

        Dim lTsGroup As atcTimeseriesGroup = BuildTserGroup(lGroupNode)
        If lTsGroup.Count > 0 Then
            pfrmParams = New atcIDF.frmSWSTAT()
            pfrmParams.BatchAnalysis = BatchAnalysis
            pfrmParams.Initialize(lTsGroup, lBatchInputs)
            'pfrmParameters = pfrmParameters.GetType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
            'pfrmParameters.GetType.InvokeMember("Initialize", Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {lTsGroup, lBatchInputs})
            'If lGroupName.Contains(clsBatch.ANALYSIS.SWSTAT.ToString()) Then
            'End If
        End If
    End Sub

    Private Sub btnGroupRemove_Click(sender As Object, e As EventArgs) Handles btnGroupRemove.Click
        Dim node As TreeNode = treeBFGroups.SelectedNode
        If node Is Nothing Then Exit Sub
        Dim lGroupingName As String = "BatchGroup"
        If node.Text.StartsWith(lGroupingName) Then
            RemoveBFGroup(node)
        Else
            If node.Parent IsNot Nothing Then
                If node.Parent.Nodes.Count = 1 Then
                    RemoveBFGroup(node)
                Else
                    Dim lGroupName As String = node.Parent.Text
                    Dim lStationID As String = node.Text
                    treeBFGroups.Nodes.Remove(node)

                    Dim lBFGroupAttribs As atcDataAttributes = pGroupsInputsBF.ItemByKey(lGroupName)
                    If lBFGroupAttribs IsNot Nothing Then
                        Dim lStationInfo As ArrayList = lBFGroupAttribs.GetValue(GlobalInputNames.StationsInfo)
                        If lStationInfo IsNot Nothing Then
                            Dim lIndexToRemove As Integer = -99
                            For Each lStation As String In lStationInfo
                                If lStation.Contains(lStationID) Then
                                    lIndexToRemove = lStationInfo.IndexOf(lStation)
                                    Exit For
                                End If
                            Next
                            If lIndexToRemove >= 0 Then
                                lStationInfo.RemoveAt(lIndexToRemove)
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub btnGroupPlot_Click(sender As Object, e As EventArgs) Handles btnGroupPlot.Click
        Dim node As TreeNode = treeBFGroups.SelectedNode
        If node Is Nothing Then Exit Sub
        Dim lGroupingName As String = "BatchGroup"
        Dim lTsGroup As New atcTimeseriesGroup()
        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow,flow")
        If Not node.Text.StartsWith(lGroupingName) Then
            node = node.Parent()
        End If
        For Each lStationNode As TreeNode In node.Nodes
            Dim lstationId As String = lStationNode.Text
            Dim lDataPath As String = GetDataFileFullPath(lstationId)
            Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
            If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                lTsGroup.Add(lTsGroupTemp(0))
            End If
        Next
        If lTsGroup.Count > 0 Then
            atcUSGSUtility.atcUSGSScreen.GraphDataDuration(lTsGroup)
        Else
            Logger.Msg("Need to download data first.", "Batch Map:Plot")
        End If
    End Sub
End Class