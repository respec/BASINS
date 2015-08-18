Imports atcUtility
Imports atcData
Imports atcSWSTAT
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

    Private WithEvents pfrmParamsBF As frmUSGSBaseflowBatch
    Private WithEvents pfrmParamsSWSTAT As atcSWSTAT.frmSWSTAT
    Private WithEvents pfrmParamsDFLOW As DFLOWAnalysis.frmDFLOWArgs

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
        For Each lStationID As String In pListStations
            lstStations.LeftItem(lindex) = lStationID
            lindex += 1
        Next
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
        Dim lAnalysis As String = ""
        Dim lCount As Integer = 0
        If rdoSWSTAT.Checked Then
            lAnalysis = clsBatch.ANALYSIS.ITA.ToString()
            pBatchGroupCountSWSTAT += 1
            lCount = pBatchGroupCountSWSTAT
        ElseIf rdoDFLOW.Checked Then
            lAnalysis = clsBatch.ANALYSIS.DFLOW.ToString()
            pBatchGroupCountDFLOW += 1
            lCount = pBatchGroupCountDFLOW
        End If
        If String.IsNullOrEmpty(lAnalysis) Then
            Exit Sub
        End If
        Dim lnewTreeNode As New Windows.Forms.TreeNode("BatchGroup_" & lAnalysis & "_" & lCount)
        treeBFGroups.Nodes.Add(lnewTreeNode)
        For I As Integer = 0 To lstStations.RightCount - 1
            With lnewTreeNode
                .Nodes.Add(lstStations.RightItem(I))
            End With
        Next
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
                Dim lBatchInputs As atcDataAttributes = Nothing
                Dim lIndex As Integer = lGroupName.LastIndexOf("_")
                Dim lGroupNum As Integer = Integer.Parse(lGroupName.Substring(lIndex + 1))
                If lGroupName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
                    lBatchInputs = pGroupsInputsSWSTAT.ItemByKey(lGroupName)
                ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                    lBatchInputs = pGroupsInputsDFLOW.ItemByKey(lGroupName)
                Else
                    lBatchInputs = pGroupsInputsBF.ItemByKey(lGroupName)
                End If

                If lBatchInputs Is Nothing Then
                    lBatchInputs = New atcDataAttributes()
                    lBatchInputs.SetValue("Operation", "GroupSetParm")
                    lBatchInputs.SetValue("Group", lGroupName)

                    If lGroupName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
                        pGroupsInputsSWSTAT.Add(lGroupName, lBatchInputs)
                    ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                        pGroupsInputsDFLOW.Add(lGroupName, lBatchInputs)
                    Else
                        pGroupsInputsBF.Add(lGroupName, lBatchInputs)
                    End If
                End If

                'Try to use global setting as much as possible
                If lGroupName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
                    atcSWSTAT.modUtil.InputNames.BuildInputSet(lBatchInputs, pGlobalInputsSWSTAT)
                ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                Else

                End If

                Dim lTsGroup As atcTimeseriesGroup = BuildTserGroup(lGroupNode)
                If lTsGroup.Count > 0 Then
                    If lGroupName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
                        pfrmParamsSWSTAT = New atcSWSTAT.frmSWSTAT()
                        pfrmParamsSWSTAT.Initialize(lTsGroup, lBatchInputs)
                    ElseIf lGroupName.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then

                    Else
                        pfrmParamsBF = New frmUSGSBaseflowBatch()
                        pfrmParamsBF.Initialize(lTsGroup, lBatchInputs)
                    End If
                End If
            Case "cmsGlobalSetParm"
                Dim lNodeText As String = node.Text
                If lNodeText.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
                    pGlobalInputsSWSTAT.SetValue("Operation", "GlobalSetParm")
                    pfrmParamsSWSTAT = New atcSWSTAT.frmSWSTAT()
                    atcSWSTAT.modUtil.InputNames.BuildInputSet(pGlobalInputsSWSTAT, Nothing)
                    Dim lNDay() As Double = pGlobalInputsSWSTAT.GetValue(atcSWSTAT.modUtil.InputNames.NDay, Nothing)
                    Dim lNDays As atcCollection = pGlobalInputsSWSTAT.GetValue(atcSWSTAT.modUtil.InputNames.NDays, Nothing)
                    Dim lRP() As Double = pGlobalInputsSWSTAT.GetValue(atcSWSTAT.modUtil.InputNames.ReturnPeriod, Nothing)
                    Dim lRPs As atcCollection = pGlobalInputsSWSTAT.GetValue(atcSWSTAT.modUtil.InputNames.ReturnPeriods, Nothing)
                    pfrmParamsSWSTAT.Initialize(Nothing, pGlobalInputsSWSTAT)
                ElseIf lNodeText.Contains(clsBatch.ANALYSIS.DFLOW.ToString()) Then
                    pGlobalInputsDFLOW.SetValue("Operation", "GlobalSetParm")
                    pfrmParamsDFLOW = New DFLOWAnalysis.frmDFLOWArgs()
                    pfrmParamsDFLOW.Initialize(Nothing, pGlobalInputsDFLOW)
                Else
                    pGlobalInputsBF.SetValue("Operation", "GlobalSetParm")
                    pfrmParamsBF = New frmUSGSBaseflowBatch()
                    pfrmParamsBF.Initialize(Nothing, pGlobalInputsBF)
                End If
        End Select
    End Sub

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
                        If lTs.Attributes.GetValue("Location") = lstationId AndAlso _
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

        pGroupsInputsBF.RemoveByKey(aBFGroupNode.Text)
        treeBFGroups.Nodes.Remove(aBFGroupNode)
        pBatchGroupCount -= 1
        If lGroupingName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
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

    Private Sub ParmetersSetSWSTAT(ByVal aArgs As atcDataAttributes) Handles pfrmParamsSWSTAT.ParametersSet
        Dim lSpecUtil As New clsBatchSpec()

        Dim lText As String = lSpecUtil.ParametersToText(clsBatch.ANALYSIS.ITA, aArgs)

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

    Private Sub btnBrowseDataDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDataDir.Click
        Dim lFolder As New System.Windows.Forms.FolderBrowserDialog()
        If lFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDataDir.Text = lFolder.SelectedPath
            If IO.Directory.Exists(txtDataDir.Text) Then
                pDataPath = txtDataDir.Text
            End If
        End If
    End Sub

    Private Sub txtDataDir_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataDir.TextChanged
        If IO.Directory.Exists(txtDataDir.Text.Trim()) Then
            pDataPath = txtDataDir.Text.Trim()
        End If
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        If Not String.IsNullOrEmpty(pDataPath) AndAlso IO.Directory.Exists(pDataPath) Then
            Dim lStationsNeedDownload As New atcCollection()
            For Each lStationID As String In pListStations
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
                Logger.Msg("Some issues occurred during download, please examine the RDB files in data directory:" & vbCrLf & _
                           pDataPath, "Batch Map")
                Return
            End Try
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
        If pGroupsInputsSWSTAT.Count = 0 AndAlso pGroupsInputsDFLOW.Count = 0 Then
            'Logger.Msg("Need to specify global default parameters.", "Batch Run Parameter Setup")
            Logger.Msg("Need to set up batch run groups.", "Batch Run Parameter Setup")
            Exit Sub
        End If
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
            SaveBatchFile(pBatchSpecFilefullname, clsBatch.ANALYSIS.ITA, pGlobalInputsSWSTAT, pGroupsInputsSWSTAT)
        End If
        If pGlobalInputsDFLOW.Count > 0 AndAlso pGroupsInputsDFLOW.Count > 0 Then
            Dim lSpecDirDFLOW As String = pGlobalInputsDFLOW.GetValue(InputNames.OutputDir, "")
            If String.IsNullOrEmpty(lSpecDirDFLOW) Then
                lSpecDirDFLOW = pDataPath
            End If
            pBatchSpecFilefullname = IO.Path.Combine(lSpecDirDFLOW, "BatchRun_DFLOW_" & SafeFilename(DateTime.Now) & ".txt")
            SaveBatchFile(pBatchSpecFilefullname, clsBatch.ANALYSIS.DFLOW, pGlobalInputsDFLOW, pGroupsInputsDFLOW)
        End If
        pBatchSpecFilefullname = ""
    End Sub

    Private Sub SaveBatchFile(ByVal aFilename As String, ByVal aAnalysis As clsBatch.ANALYSIS, _
                              ByVal aCommonSetting As atcDataAttributes, ByVal aGroupSettings As atcCollection)
        Dim lSW As IO.StreamWriter = Nothing
        Dim lTitle As String = ""
        If aAnalysis = clsBatch.ANALYSIS.ITA Then
            lTitle = "Save SWSTAT Batch File"
        ElseIf aAnalysis = clsBatch.ANALYSIS.DFLOW Then
            lTitle = "Save DFLOW Batch File"
        End If
        Try
            Dim lBatchSpec As New clsBatchSpec()
            lSW = New IO.StreamWriter(aFilename, False)
            'write global block first
            lSW.WriteLine(lBatchSpec.ParametersToText(aAnalysis, aCommonSetting))
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
                            lStationsInfo = BuildStationsInfo(lTsGroup)
                        End If
                    End If
                    lGroupAttrib.SetValue(GlobalInputNames.StationsInfo, lStationsInfo)
                End If
                lSW.WriteLine(lBatchSpec.ParametersToText(aAnalysis, lGroupAttrib))
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
        If lGroupName.Contains(clsBatch.ANALYSIS.ITA.ToString()) Then
            lAnalysis = clsBatch.ANALYSIS.ITA
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
                            If loc = lstationId AndAlso _
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
                    Case clsBatch.ANALYSIS.ITA
                    Case Else
                        pfrmParamsBF = New frmUSGSBaseflowBatch()
                        pfrmParamsBF.Initialize(lTsGroup, lArgs)
                End Select
            End If
        End If
    End Sub
End Class