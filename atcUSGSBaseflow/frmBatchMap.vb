Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcTimeseriesBaseflow
Imports atcBatchProcessing
Imports System.Windows.Forms

Public Class frmBatchMap
    Private pListStations As atcCollection
    Private pDataPath As String
    Private pBatchGroupCount As Integer = 1
    Private pBFInputsGlobal As atcDataAttributes
    Private pBFInputsGroups As atcCollection
    Private pBatchSpecFilefullname As String = ""

    Private WithEvents pfrmBFParms As frmUSGSBaseflowBatch

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
        For Each lStationID As String In pListStations.Keys
            lstStations.LeftItem(lindex) = lStationID
            lindex += 1
        Next
        pBFInputsGlobal = New atcDataAttributes()
        pBFInputsGroups = New atcCollection()
        Dim lDataDir As String = GetSetting("atcUSGSBaseflow", "Default", "DataDir", "")
        If IO.Directory.Exists(lDataDir) Then
            txtDataDir.Text = lDataDir
        End If
    End Sub

    Private Sub btnCreateGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateGroup.Click
        Dim lnewTreeNode As New Windows.Forms.TreeNode("BatchGroup_" & pBatchGroupCount)
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
                Dim lArgs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)
                If lArgs IsNot Nothing Then
                    txtParameters.Text = ParametersToText(lArgs)
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

                            Dim lBFGroupAttribs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)
                            If lBFGroupAttribs IsNot Nothing Then
                                Dim lStationInfo As ArrayList = lBFGroupAttribs.GetValue("StationInfo")
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
                lArgs.Add("Constituent", "streamflow")
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

                Dim lGroupNum As Integer = Integer.Parse(lGroupName.Substring(lGroupingName.Length + 1))
                Dim lBFInputs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)

                If lBFInputs Is Nothing Then
                    lBFInputs = New atcDataAttributes()
                    lBFInputs.SetValue("Operation", "GroupSetParm")
                    lBFInputs.SetValue("Group", lGroupName)
                    pBFInputsGroups.Add(lGroupName, lBFInputs)
                End If
                'Try to use global setting as much as possible
                If pBFInputsGlobal IsNot Nothing Then
                    Dim lMethods As ArrayList = lBFInputs.GetValue(BFInputNames.BFMethods, Nothing)
                    If lMethods Is Nothing Then
                        Dim lMethodsGlobal As ArrayList = pBFInputsGlobal.GetValue(BFInputNames.BFMethods, Nothing)
                        If lMethodsGlobal IsNot Nothing Then
                            lBFInputs.SetValue(BFInputNames.BFMethods, lMethodsGlobal)
                        End If
                    End If
                    Dim lOutputDir As String = lBFInputs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
                    If String.IsNullOrEmpty(lOutputDir) Then
                        Dim lOutputDirGlobal As String = pBFInputsGlobal.GetValue(BFBatchInputNames.OUTPUTDIR, "")
                        If Not String.IsNullOrEmpty(lOutputDirGlobal) Then
                            lBFInputs.SetValue(BFBatchInputNames.OUTPUTDIR, lOutputDirGlobal)
                        End If
                    End If
                    Dim lOutputFileRoot As String = lBFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                    If String.IsNullOrEmpty(lOutputFileRoot) Then
                        Dim lOutputFileRootGlobal As String = pBFInputsGlobal.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                        If Not String.IsNullOrEmpty(lOutputFileRootGlobal) Then
                            lBFInputs.SetValue(BFBatchInputNames.OUTPUTPrefix, lOutputFileRootGlobal)
                        End If
                    End If
                    Dim lBFIReportBy As String = lBFInputs.GetValue(BFInputNames.BFIReportby, "")
                    If String.IsNullOrEmpty(lBFIReportBy) Then
                        Dim lBFIReportByGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFIReportby, "")
                        If Not String.IsNullOrEmpty(lBFIReportByGlobal) Then
                            lBFInputs.SetValue(BFInputNames.BFIReportby, lBFIReportByGlobal)
                        End If
                    End If
                    Dim lBFIRecessConst As String = lBFInputs.GetValue(BFInputNames.BFIRecessConst, "")
                    If String.IsNullOrEmpty(lBFIRecessConst) Then
                        Dim lBFIRecessConstGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFIRecessConst, "")
                        If Not String.IsNullOrEmpty(lBFIRecessConstGlobal) Then
                            lBFInputs.SetValue(BFInputNames.BFIRecessConst, lBFIRecessConstGlobal)
                        End If
                    End If
                    Dim lBFITurnPtFrac As String = lBFInputs.GetValue(BFInputNames.BFITurnPtFrac, "")
                    If String.IsNullOrEmpty(lBFITurnPtFrac) Then
                        Dim lBFITurnPtFracGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFITurnPtFrac, "")
                        If Not String.IsNullOrEmpty(lBFITurnPtFracGlobal) Then
                            lBFInputs.SetValue(BFInputNames.BFITurnPtFrac, lBFITurnPtFracGlobal)
                        End If
                    End If
                    Dim lBFINDay As String = lBFInputs.GetValue(BFInputNames.BFINDayScreen, "")
                    If String.IsNullOrEmpty(lBFINDay) Then
                        Dim lBFINDayGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFINDayScreen, "")
                        If Not String.IsNullOrEmpty(lBFINDayGlobal) Then
                            lBFInputs.SetValue(BFInputNames.BFINDayScreen, lBFINDayGlobal)
                        End If
                    End If

                    Dim lBFStartDate As String = lBFInputs.GetValue(BFInputNames.StartDate, "")
                    If String.IsNullOrEmpty(lBFStartDate) Then
                        Dim lBFStartDateGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.StartDate, "")
                        If Not String.IsNullOrEmpty(lBFStartDateGlobal) Then
                            lBFInputs.SetValue(BFInputNames.StartDate, lBFStartDateGlobal)
                        End If
                    End If
                    Dim lBFEndDate As String = lBFInputs.GetValue(BFInputNames.EndDate, "")
                    If String.IsNullOrEmpty(lBFEndDate) Then
                        Dim lBFEndDateGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.EndDate, "")
                        If Not String.IsNullOrEmpty(lBFEndDateGlobal) Then
                            lBFInputs.SetValue(BFInputNames.EndDate, lBFEndDateGlobal)
                        End If
                    End If
                End If

                Dim lArgs As New atcDataAttributes()
                lArgs.Add("Constituent", "streamflow,flow")
                Dim lTsGroup As New atcTimeseriesGroup()
                For Each lStationNode As TreeNode In lGroupNode.Nodes
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
                If lTsGroup.Count > 0 Then
                    pfrmBFParms = New frmUSGSBaseflowBatch()
                    pfrmBFParms.Initialize(lTsGroup, lBFInputs)
                End If
            Case "cmsGlobalSetParm"
                pBFInputsGlobal.SetValue("Operation", "GlobalSetParm")
                pfrmBFParms = New frmUSGSBaseflowBatch()
                pfrmBFParms.Initialize(Nothing, pBFInputsGlobal)
        End Select
    End Sub

    ''' <summary>
    ''' Only call when removing a whole group
    ''' </summary>
    ''' <param name="aBFGroupNode"></param>
    ''' <remarks></remarks>
    Private Sub RemoveBFGroup(ByVal aBFGroupNode As TreeNode)
        Dim lGroupingName As String = "BatchGroup"
        Dim lGroupNum As Integer = Integer.Parse(aBFGroupNode.Text.Substring(lGroupingName.Length + 1))
        pBFInputsGroups.RemoveByKey(aBFGroupNode.Text)
        treeBFGroups.Nodes.Remove(aBFGroupNode)
        pBatchGroupCount -= 1
        Dim loldNodeText As String = ""
        Dim loldNodeIndex As Integer
        For Each lNode As TreeNode In treeBFGroups.Nodes
            If lNode.Text.StartsWith(lGroupingName) Then
                loldNodeText = lNode.Text
                loldNodeIndex = pBFInputsGroups.Keys.IndexOf(loldNodeText)
                Dim lGroupNumRemains As Integer = Integer.Parse(lNode.Text.Substring(lGroupingName.Length + 1))
                If lGroupNumRemains > lGroupNum Then
                    Dim lNewNodeText As String = lGroupingName & "_" & (lGroupNumRemains - 1).ToString
                    lNode.Text = lNewNodeText
                    If loldNodeIndex >= 0 Then pBFInputsGroups.Keys.Item(loldNodeIndex) = lNewNodeText
                End If
            End If
        Next
    End Sub

    Private Sub ParmetersSet(ByVal aArgs As atcDataAttributes) Handles pfrmBFParms.ParametersSet
        Dim lText As String = ParametersToText(aArgs)
        
        If String.IsNullOrEmpty(lText) Then
            txtParameters.Text = ""
        Else
            Dim loperation As String = aArgs.GetValue("Operation", "")
            Dim lgroupname As String = aArgs.GetValue("Group", "")
            Dim lArg As atcDataAttributes = Nothing
            If loperation.ToLower = "groupsetparm" Then
                lArg = pBFInputsGroups.ItemByKey(lgroupname)
                If lArg Is Nothing Then
                    lArg = New atcDataAttributes()
                    pBFInputsGroups.Add(lgroupname, lArg)
                End If
            Else
                lArg = pBFInputsGlobal
            End If
            For Each lDataDef As atcDefinedValue In aArgs
                lArg.SetValue(lDataDef.Definition.Name, lDataDef.Value)
            Next
            txtParameters.Text = lText.ToString()
        End If
    End Sub

    Private Function ParametersToText(ByVal aArgs As atcDataAttributes) As String
        If aArgs Is Nothing Then Return ""
        Dim loperation As String = aArgs.GetValue("Operation", "")
        Dim lgroupname As String = aArgs.GetValue("Group", "")
        Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

        Dim lText As New Text.StringBuilder()
        If loperation.ToLower = "groupsetparm" Then
            lText.AppendLine("BASE-FLOW")
            Dim lStationInfo As ArrayList = aArgs.GetValue("StationInfo")
            If lStationInfo IsNot Nothing Then
                For Each lstation As String In lStationInfo
                    lText.AppendLine(lstation)
                Next
            End If
        ElseIf lSetGlobal Then
            lText.AppendLine("GLOBAL")
        End If

        Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
        Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
        Dim lDates(5) As Integer
        J2Date(lStartDate, lDates)
        lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
        J2Date(lEndDate, lDates)
        timcnv(lDates)
        lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

        If aArgs.ContainsAttribute(BFInputNames.BFMethods) Then
            Dim lMethods As ArrayList = aArgs.GetValue(BFInputNames.BFMethods)
            For Each lMethod As BFMethods In lMethods
                Select Case lMethod
                    Case BFMethods.PART
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_PART)
                    Case BFMethods.HySEPFixed
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYFX)
                    Case BFMethods.HySEPLocMin
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYLM)
                    Case BFMethods.HySEPSlide
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYSL)
                    Case BFMethods.BFIStandard
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIS)
                    Case BFMethods.BFIModified
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIM)
                End Select
            Next
        ElseIf lSetGlobal Then
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_PART)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYFX)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYLM)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYSL)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIS)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIM)
        End If

        If aArgs.ContainsAttribute(BFInputNames.BFITurnPtFrac) Then
            Dim lBFITurnPtFrac As Double = aArgs.GetValue(BFInputNames.BFITurnPtFrac)
            lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & lBFITurnPtFrac)
        ElseIf lSetGlobal Then
            lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & "0.9")
        End If
        If aArgs.ContainsAttribute(BFInputNames.BFINDayScreen) Then
            Dim lBFINDayScreen As Double = aArgs.GetValue(BFInputNames.BFINDayScreen)
            lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & lBFINDayScreen)
        ElseIf lSetGlobal Then
            lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & "5")
        End If
        If aArgs.ContainsAttribute(BFInputNames.BFIRecessConst) Then
            Dim lBFIRecessConst As Double = aArgs.GetValue(BFInputNames.BFIRecessConst)
            lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & lBFIRecessConst)
        ElseIf lSetGlobal Then
            lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & "0.97915")
        End If

        If aArgs.ContainsAttribute(BFInputNames.BFIReportby) Then
            Dim lBFIReportBy As String = aArgs.GetValue(BFInputNames.BFIReportby, "")
            Select Case lBFIReportBy
                Case BFInputNames.BFIReportbyCY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
                Case BFInputNames.BFIReportbyWY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByWY)
            End Select
        ElseIf lSetGlobal Then
            lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
        End If

        If lSetGlobal Then
            Dim lDatadir As String = aArgs.GetValue(BFBatchInputNames.DataDir, "")
            If lDatadir = "" Then
                lDatadir = txtDataDir.Text.Trim()
                If IO.Directory.Exists(lDatadir) Then
                    lText.AppendLine(BFBatchInputNames.DataDir & vbTab & lDatadir)
                End If
            End If
        End If

        Dim lOutputDir As String = aArgs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        Dim lOutputPrefix As String = aArgs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
        lText.AppendLine(BFBatchInputNames.OUTPUTDIR & vbTab & lOutputDir)
        lText.AppendLine(BFBatchInputNames.OUTPUTPrefix & vbTab & lOutputPrefix)

        If loperation.ToLower = "groupsetparm" Then
            lText.AppendLine("END BASE-FLOW")
        ElseIf lSetGlobal Then
            lText.AppendLine("END GLOBAL")
        End If
        Return lText.ToString()
    End Function

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
        If pBFInputsGlobal.Count = 0 Then
            Logger.Msg("Need to specify global default parameters.", "Batch Map Base-flow Separation")
            Return
        End If
        If pBFInputsGroups.Count = 0 Then
            Logger.Msg("Need to set up batch run groups.", "Batch Map Base-flow Separation")
            Return
        End If
        If Not String.IsNullOrEmpty(pBatchSpecFilefullname) AndAlso IO.File.Exists(pBatchSpecFilefullname) Then
            Dim lfrmBatch As New frmBatch()
            lfrmBatch.BatchSpecFile = pBatchSpecFilefullname
            lfrmBatch.ShowDialog()
        Else
            Logger.Msg("Need to construct a batch specification file first.", "Batch Base-flow Separation")
        End If
    End Sub

    Private Sub btnSaveSpecs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSpecs.Click
        If pBFInputsGlobal.Count = 0 Then
            Logger.Msg("Need to specify global default parameters.", "Batch Map Base-flow Separation")
            Exit Sub
        End If
        If pBFInputsGroups.Count = 0 Then
            Logger.Msg("Need to set up batch run groups.", "Batch Map Base-flow Separation")
            Exit Sub
        End If
        Dim lSpecDir As String = pBFInputsGlobal.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        If String.IsNullOrEmpty(lSpecDir) Then
            lSpecDir = pDataPath
        End If
        pBatchSpecFilefullname = IO.Path.Combine(lSpecDir, "BatchConfigBase-flowSep_" & SafeFilename(DateTime.Now) & ".txt")
        Dim lSW As IO.StreamWriter = Nothing
        Try
            lSW = New IO.StreamWriter(pBatchSpecFilefullname, False)
            'write global block first
            lSW.WriteLine(ParametersToText(pBFInputsGlobal))

            'write each group settings
            For Each lAttrib As atcDataAttributes In pBFInputsGroups
                lSW.WriteLine(ParametersToText(lAttrib))
            Next

            Logger.Msg("Batch file is saved:" & vbCrLf & pBatchSpecFilefullname, "Batch Base-flow Separation")
        Catch ex As Exception
            Logger.Msg("Error Writing Spec File:" & vbCrLf & pBatchSpecFilefullname & vbCrLf & vbCrLf & ex.Message, "Batch Base-flow Separation")
            pBatchSpecFilefullname = ""
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
        Dim lArgs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)
        If lArgs IsNot Nothing Then
            lArgs.SetValue("Constituent", "streamflow")
            Dim lTsGroup As New atcTimeseriesGroup()
            For Each lStationNode As TreeNode In lGroupNode.Nodes
                Dim lstationId As String = lStationNode.Text

                Dim lDataLoaded As Boolean = False
                For Each lDS As atcDataSource In atcDataManager.DataSources
                    If lDS.Name.ToString.Contains("USGS RDB") Then
                        For Each lTs As atcTimeseries In lDS.DataSets
                            If lTs.Attributes.GetValue("Location") = lstationId AndAlso _
                               lTs.Attributes.GetValue("Constituent").ToString.ToLower = "streamflow" Then
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
                pfrmBFParms = New frmUSGSBaseflowBatch()
                pfrmBFParms.Initialize(lTsGroup, lArgs)
            End If
        End If
    End Sub

    Private Sub btnGroupGlobal_Click(sender As Object, e As EventArgs) Handles btnGroupGlobal.Click
        pBFInputsGlobal.SetValue("Operation", "GlobalSetParm")
        pfrmBFParms = New frmUSGSBaseflowBatch()
        pfrmBFParms.Initialize(Nothing, pBFInputsGlobal)
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

        Dim lGroupNum As Integer = Integer.Parse(lGroupName.Substring(lGroupingName.Length + 1))
        Dim lBFInputs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)

        If lBFInputs Is Nothing Then
            lBFInputs = New atcDataAttributes()
            lBFInputs.SetValue("Operation", "GroupSetParm")
            lBFInputs.SetValue("Group", lGroupName)
            pBFInputsGroups.Add(lGroupName, lBFInputs)
        End If
        'Try to use global setting as much as possible
        If pBFInputsGlobal IsNot Nothing Then
            Dim lMethods As ArrayList = lBFInputs.GetValue(BFInputNames.BFMethods, Nothing)
            If lMethods Is Nothing Then
                Dim lMethodsGlobal As ArrayList = pBFInputsGlobal.GetValue(BFInputNames.BFMethods, Nothing)
                If lMethodsGlobal IsNot Nothing Then
                    lBFInputs.SetValue(BFInputNames.BFMethods, lMethodsGlobal)
                End If
            End If
            Dim lOutputDir As String = lBFInputs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
            If String.IsNullOrEmpty(lOutputDir) Then
                Dim lOutputDirGlobal As String = pBFInputsGlobal.GetValue(BFBatchInputNames.OUTPUTDIR, "")
                If Not String.IsNullOrEmpty(lOutputDirGlobal) Then
                    lBFInputs.SetValue(BFBatchInputNames.OUTPUTDIR, lOutputDirGlobal)
                End If
            End If
            Dim lOutputFileRoot As String = lBFInputs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
            If String.IsNullOrEmpty(lOutputFileRoot) Then
                Dim lOutputFileRootGlobal As String = pBFInputsGlobal.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
                If Not String.IsNullOrEmpty(lOutputFileRootGlobal) Then
                    lBFInputs.SetValue(BFBatchInputNames.OUTPUTPrefix, lOutputFileRootGlobal)
                End If
            End If
            Dim lBFIReportBy As String = lBFInputs.GetValue(BFInputNames.BFIReportby, "")
            If String.IsNullOrEmpty(lBFIReportBy) Then
                Dim lBFIReportByGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFIReportby, "")
                If Not String.IsNullOrEmpty(lBFIReportByGlobal) Then
                    lBFInputs.SetValue(BFInputNames.BFIReportby, lBFIReportByGlobal)
                End If
            End If
            Dim lBFIRecessConst As String = lBFInputs.GetValue(BFInputNames.BFIRecessConst, "")
            If String.IsNullOrEmpty(lBFIRecessConst) Then
                Dim lBFIRecessConstGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFIRecessConst, "")
                If Not String.IsNullOrEmpty(lBFIRecessConstGlobal) Then
                    lBFInputs.SetValue(BFInputNames.BFIRecessConst, lBFIRecessConstGlobal)
                End If
            End If
            Dim lBFITurnPtFrac As String = lBFInputs.GetValue(BFInputNames.BFITurnPtFrac, "")
            If String.IsNullOrEmpty(lBFITurnPtFrac) Then
                Dim lBFITurnPtFracGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFITurnPtFrac, "")
                If Not String.IsNullOrEmpty(lBFITurnPtFracGlobal) Then
                    lBFInputs.SetValue(BFInputNames.BFITurnPtFrac, lBFITurnPtFracGlobal)
                End If
            End If
            Dim lBFINDay As String = lBFInputs.GetValue(BFInputNames.BFINDayScreen, "")
            If String.IsNullOrEmpty(lBFINDay) Then
                Dim lBFINDayGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.BFINDayScreen, "")
                If Not String.IsNullOrEmpty(lBFINDayGlobal) Then
                    lBFInputs.SetValue(BFInputNames.BFINDayScreen, lBFINDayGlobal)
                End If
            End If

            Dim lBFStartDate As String = lBFInputs.GetValue(BFInputNames.StartDate, "")
            If String.IsNullOrEmpty(lBFStartDate) Then
                Dim lBFStartDateGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.StartDate, "")
                If Not String.IsNullOrEmpty(lBFStartDateGlobal) Then
                    lBFInputs.SetValue(BFInputNames.StartDate, lBFStartDateGlobal)
                End If
            End If
            Dim lBFEndDate As String = lBFInputs.GetValue(BFInputNames.EndDate, "")
            If String.IsNullOrEmpty(lBFEndDate) Then
                Dim lBFEndDateGlobal As String = pBFInputsGlobal.GetValue(BFInputNames.EndDate, "")
                If Not String.IsNullOrEmpty(lBFEndDateGlobal) Then
                    lBFInputs.SetValue(BFInputNames.EndDate, lBFEndDateGlobal)
                End If
            End If
        End If

        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow,flow")
        Dim lTsGroup As New atcTimeseriesGroup()
        For Each lStationNode As TreeNode In lGroupNode.Nodes
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
        If lTsGroup.Count > 0 Then
            pfrmBFParms = New frmUSGSBaseflowBatch()
            pfrmBFParms.Initialize(lTsGroup, lBFInputs)
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

                    Dim lBFGroupAttribs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)
                    If lBFGroupAttribs IsNot Nothing Then
                        Dim lStationInfo As ArrayList = lBFGroupAttribs.GetValue("StationInfo")
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
        lArgs.Add("Constituent", "streamflow")
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