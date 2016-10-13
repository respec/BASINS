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

    Private pDataGroupsChanged As Boolean = False
    Private pDataStart As Double
    Private pDataEnd As Double
    Private pDataStartCommon As Double
    Private pDataEndCommon As Double
    Private pRefreshTimeSpan As Boolean = False

    Private pStationInfoGroup As atcCollection

    Private WithEvents pfrmBFParms As frmUSGSBaseflowBatch

    Private pLoaded As Boolean = False

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
        lstStations.LeftLabel = "Stations"
        lstStations.RightLabel = "Selected Stations"
        Dim lindex As Integer = 0
        For Each lStationID As String In pListStations.Keys
            lstStations.LeftItem(lindex) = lStationID
            lindex += 1
        Next
        pBFInputsGlobal = New atcDataAttributes()
        pBFInputsGroups = New atcCollection()
        pStationInfoGroup = New atcCollection()
        Dim lDataDir As String = GetSetting("atcUSGSBaseflow", "Default", "DataDir", "")
        If IO.Directory.Exists(lDataDir) Then
            txtDataDir.Text = lDataDir
        End If
        pLoaded = True
    End Sub

    Private Sub LogStationInfo(ByVal aTser As atcTimeseries, ByVal lStationId As String, ByVal lDataPath As String)
        If Not pStationInfoGroup.Keys.Contains(lStationId) Then
            Dim ldrainarea As Double = aTser.Attributes.GetValue("Drainage Area", 0.0)
            pStationInfoGroup.Add(lStationId, "Station" & vbTab & lStationId & "," & ldrainarea & "," & lDataPath)
        End If
    End Sub

    Private Sub btnCreateGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateGroup.Click
        Dim lGroupName As String = "BatchGroup_" & pBatchGroupCount
        Dim lnewTreeNode As New Windows.Forms.TreeNode(lGroupName)
        treeBFGroups.Nodes.Add(lnewTreeNode)
        For I As Integer = 0 To lstStations.RightCount - 1
            With lnewTreeNode
                .Nodes.Add(lstStations.RightItem(I))
            End With
        Next

        Dim lBFInputs As atcDataAttributes = pBFInputsGroups.ItemByKey(lGroupName)
        If lBFInputs Is Nothing Then
            lBFInputs = New atcDataAttributes()
            lBFInputs.SetValue("Operation", "GroupSetParm")
            lBFInputs.SetValue("Group", lGroupName)
            pBFInputsGroups.Add(lGroupName, lBFInputs)
            Dim lStationsInfo As New ArrayList()
            For I As Integer = 0 To lstStations.RightCount - 1
                lStationsInfo.Add("Station" & vbTab & lstStations.RightItem(I) & ",0.0,unknown")
            Next
            lBFInputs.SetValue("StationInfo", lStationsInfo)
        End If

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
                                        pDataGroupsChanged = True
                                        If pRefreshTimeSpan Then
                                            SetDataTimeSpan()
                                        End If
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
                        LogStationInfo(lTsGroupTemp(0), lstationId, lDataPath)
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
                            LogStationInfo(lTsGroupTemp(0), lstationId, lDataPath)
                        End If
                    End If
                Next
                If lTsGroup.Count > 0 Then
                    pfrmBFParms = New frmUSGSBaseflowBatch()
                    pfrmBFParms.Initialize(lTsGroup, lBFInputs)
                End If
            Case "cmsGlobalSetParm"
                If pDataGroupsChanged OrElse pBFInputsGlobal.Count = 0 Then
                    SetDataTimeSpan(True)
                End If
                pBFInputsGlobal.SetValue("Operation", "GlobalSetParm")
                pBFInputsGlobal.SetValue("SJDATE", pDataStart)
                pBFInputsGlobal.SetValue("EJDATE", pDataEnd)
                pBFInputsGlobal.SetValue("SJDATECommon", pDataStartCommon)
                pBFInputsGlobal.SetValue("EJDATECommon", pDataEndCommon)
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

    Private Function ParametersToText(ByVal aArgs As atcDataAttributes, Optional aDefaultArgs As atcDataAttributes = Nothing) As String
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

        Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, -99) 'Date2J(2014, 8, 20, 0, 0, 0))
        Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, -99) 'Date2J(2014, 8, 20, 24, 0, 0))
        If lStartDate < 0 AndAlso aDefaultArgs IsNot Nothing Then
            lStartDate = aDefaultArgs.GetValue(BFInputNames.StartDate, 0)
        End If
        If lEndDate < 0 AndAlso aDefaultArgs IsNot Nothing Then
            lEndDate = aDefaultArgs.GetValue(BFInputNames.EndDate, 0)
        End If

        Dim lDates(5) As Integer
        J2Date(lStartDate, lDates)
        lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
        J2Date(lEndDate, lDates)
        timcnv(lDates)
        lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

        Dim lMethods As ArrayList = Nothing
        If aArgs.ContainsAttribute(BFInputNames.BFMethods) Then
            lMethods = aArgs.GetValue(BFInputNames.BFMethods)
        ElseIf aDefaultArgs IsNot Nothing AndAlso aDefaultArgs.ContainsAttribute(BFInputNames.BFMethods) Then
            lMethods = aDefaultArgs.GetValue(BFInputNames.BFMethods)
        End If
        If lMethods IsNot Nothing Then
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
                    Case BFMethods.BFLOW
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFLOW)
                    Case BFMethods.TwoPRDF
                        lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_TwoPRDF)
                End Select
            Next
        Else
            'get 3 representatives
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_PART)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYFX)
            'lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYLM)
            'lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_HYSL)
            lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIS)
            'lText.AppendLine("BFMethod" & vbTab & BFBatchInputNames.BFM_BFIM)
        End If

        Dim lBFITurnPtFrac As Double = 0.9
        If aArgs.ContainsAttribute(BFInputNames.BFITurnPtFrac) Then
            lBFITurnPtFrac = aArgs.GetValue(BFInputNames.BFITurnPtFrac)
        ElseIf aDefaultArgs IsNot Nothing AndAlso aDefaultArgs.ContainsAttribute(BFInputNames.BFITurnPtFrac) Then
            lBFITurnPtFrac = aDefaultArgs.GetValue(BFInputNames.BFITurnPtFrac)
        End If
        lText.AppendLine(BFInputNames.BFITurnPtFrac & vbTab & lBFITurnPtFrac)


        Dim lBFINDayScreen As Double = 5
        If aArgs.ContainsAttribute(BFInputNames.BFINDayScreen) Then
            lBFINDayScreen = aArgs.GetValue(BFInputNames.BFINDayScreen)
        ElseIf aDefaultArgs IsNot Nothing AndAlso aDefaultArgs.ContainsAttribute(BFInputNames.BFINDayScreen) Then
            lBFINDayScreen = aDefaultArgs.GetValue(BFInputNames.BFINDayScreen)
        End If
        lText.AppendLine(BFInputNames.BFINDayScreen & vbTab & lBFINDayScreen)

        Dim lBFIRecessConst As Double = 0.97915
        If aArgs.ContainsAttribute(BFInputNames.BFIRecessConst) Then
            lBFIRecessConst = aArgs.GetValue(BFInputNames.BFIRecessConst)
        ElseIf aDefaultArgs IsNot Nothing AndAlso aDefaultArgs.ContainsAttribute(BFInputNames.BFIRecessConst) Then
            lBFIRecessConst = aDefaultArgs.GetValue(BFInputNames.BFIRecessConst)
        End If
        lText.AppendLine(BFInputNames.BFIRecessConst & vbTab & lBFIRecessConst)

        Dim lBFIReportBy As String = ""
        If aArgs.ContainsAttribute(BFInputNames.BFIReportby) Then
            lBFIReportBy = aArgs.GetValue(BFInputNames.BFIReportby, "")
            Select Case lBFIReportBy
                Case BFInputNames.BFIReportbyCY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
                Case BFInputNames.BFIReportbyWY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByWY)
            End Select
        ElseIf aDefaultArgs IsNot Nothing AndAlso aDefaultArgs.ContainsAttribute(BFInputNames.BFIReportby) Then
            lBFIReportBy = aDefaultArgs.GetValue(BFInputNames.BFIReportby, "")
            Select Case lBFIReportBy
                Case BFInputNames.BFIReportbyCY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByCY)
                Case BFInputNames.BFIReportbyWY
                    lText.AppendLine(BFInputNames.BFIReportby & vbTab & BFBatchInputNames.ReportByWY)
            End Select
        Else
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
        If String.IsNullOrEmpty(lOutputDir) AndAlso aDefaultArgs IsNot Nothing Then
            lOutputDir = aDefaultArgs.GetValue(BFBatchInputNames.OUTPUTDIR, "")
        End If
        lText.AppendLine(BFBatchInputNames.OUTPUTDIR & vbTab & lOutputDir)

        Dim lOutputPrefix As String = aArgs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
        If String.IsNullOrEmpty(lOutputPrefix) AndAlso aDefaultArgs IsNot Nothing Then
            lOutputPrefix = aDefaultArgs.GetValue(BFBatchInputNames.OUTPUTPrefix, "")
        End If
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
        If IO.Directory.Exists(txtDataDir.Text) Then
            lFolder.SelectedPath = txtDataDir.Text
            Try
                SendKeys.Send("{TAB}{TAB}{RIGHT}")
            Catch ex As Exception
                'it's ok to fail
            End Try
        End If
        If lFolder.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDataDir.Text = lFolder.SelectedPath
            If IO.Directory.Exists(txtDataDir.Text) Then
                pDataPath = txtDataDir.Text
            End If
            If pListStations.Count = 0 Then
                TryPopulateStationListFromDataPath()
            End If
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
               IsNumeric(lStationID) AndAlso
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
            Dim lDataFileFullPath As String = ""
            For Each lStationID As String In pListStations.Keys
                lDataFileFullPath = GetDataFileFullPath(lStationID)
                pListStations.ItemByKey(lStationID) = lDataFileFullPath
                If Not IO.File.Exists(lDataFileFullPath) Then
                    lStationsNeedDownload.Add(lStationID, lStationID)
                End If
            Next
            clsBatchUtil.SiteInfoDir = pDataPath
            Dim lArgs As New atcDataAttributes()
            With lArgs
                .SetValue("GetNewest", chkGetNewest.Checked)
                .SetValue("CacheFolder", BASINS.g_CacheDir)
            End With
            Try
                If lStationsNeedDownload.Count > 0 OrElse chkGetNewest.Checked Then
                    If lStationsNeedDownload.Count = 0 Then
                        For Each lStationID As String In pListStations.Keys
                            lStationsNeedDownload.Add(lStationID, lStationID)
                        Next
                    End If
                    clsBatchUtil.DownloadData(lStationsNeedDownload, lArgs)
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
            lSW.Flush()

            'write each group settings
            For Each lAttrib As atcDataAttributes In pBFInputsGroups
                'check for stationInfo to be set
                Dim lStationInfo As ArrayList = lAttrib.GetValue("StationInfo")
                Dim lNeedToRefreshStationInfo As Boolean = False
                For Each lSInfo As String In lStationInfo
                    If lSInfo.ToLower().Contains("unknown") Then
                        lNeedToRefreshStationInfo = True
                        Exit For
                    End If
                Next
                If lNeedToRefreshStationInfo Then
                    Dim lStartDate As Double = lAttrib.GetValue("StartDate", -99)
                    Dim lEndDate As Double = lAttrib.GetValue("EndDate", -99)
                    Dim lNeedToSetDates As Boolean = False
                    Dim lTsGroup4Dates As atcTimeseriesGroup = Nothing
                    If lStartDate < -90 OrElse lEndDate < -90 Then
                        Dim lgStartDate As Double = pBFInputsGlobal.GetValue("StartDate", -99)
                        Dim lgEndDate As Double = pBFInputsGlobal.GetValue("EndDate", -99)
                        If lgStartDate > 0 AndAlso lgEndDate > 0 Then
                            'No need to set group's dates if they are not already set, use default
                        Else
                            lNeedToSetDates = True
                            lTsGroup4Dates = New atcTimeseriesGroup()
                        End If
                    End If

                    Dim lNewStationInfo As New ArrayList()
                    For Each lSInfo As String In lStationInfo
                        Dim lArr() As String = lSInfo.Substring(lSInfo.IndexOf(vbTab) + 1).Split(",")
                        If pStationInfoGroup.Keys.Contains(lArr(0)) AndAlso Not lNeedToSetDates Then
                            lNewStationInfo.Add(pStationInfoGroup.ItemByKey(lArr(0)))
                        Else
                            Dim lArgs As New atcDataAttributes()
                            lArgs.Add("Constituent", "streamflow")
                            Dim lDataPath As String = GetDataFileFullPath(lArr(0))
                            Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                            If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                                LogStationInfo(lTsGroupTemp(0), lArr(0), lDataPath)
                                If lNeedToSetDates Then lTsGroup4Dates.Add(lTsGroupTemp(0))
                            End If
                            lNewStationInfo.Add(pStationInfoGroup.ItemByKey(lArr(0)))
                        End If
                    Next
                    If lNeedToSetDates AndAlso lTsGroup4Dates.Count > 0 Then
                        Dim lcstart As Double
                        Dim lcend As Double
                        Dim lstart As Double
                        Dim lend As Double
                        CommonDates(lTsGroup4Dates, lstart, lend, lcstart, lcend)
                        lAttrib.SetValue("StartDate", lstart)
                        lAttrib.SetValue("EndDate", lend)
                        lTsGroup4Dates.Clear()
                    End If
                    lAttrib.RemoveByKey("StationInfo")
                    lAttrib.SetValue("StationInfo", lNewStationInfo)
                End If
                lSW.WriteLine(ParametersToText(lAttrib, pBFInputsGlobal))
                lSW.Flush()
            Next

            Logger.Msg("Batch file is saved:" & vbCrLf & pBatchSpecFilefullname, "Batch Base-flow Separation")
        Catch ex As Exception
            Logger.Msg("Error Writing Spec File:" & vbCrLf & pBatchSpecFilefullname & vbCrLf & vbCrLf & ex.Message, "Batch Base-flow Separation")
            pBatchSpecFilefullname = ""
        Finally
            If lSW IsNot Nothing Then
                lSW.Flush()
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
                    LogStationInfo(lTsGroupTemp(0), lstationId, lDataPath)
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

    Private Sub SetDataTimeSpan(Optional ByVal aMandatory As Boolean = False)
        If Not aMandatory AndAlso Not pDataGroupsChanged Then Exit Sub
        If pBFInputsGroups Is Nothing OrElse pBFInputsGroups.Count = 0 Then Exit Sub

        Dim lTsGroup As New atcTimeseriesGroup()
        Dim lArgs As New atcDataAttributes()
        lArgs.Add("Constituent", "streamflow")

        Dim lTotalGroups As Integer = pBFInputsGroups.Count
        Dim lTotalStations As Integer
        Dim lGroupCtr As Integer = 1
        Dim lStnCtr As Integer
        For Each lBFGroupAttribs As atcDataAttributes In pBFInputsGroups
            Logger.Progress("Reading group: " & lBFGroupAttribs.GetValue("Group"), lGroupCtr, lTotalGroups)
            If lBFGroupAttribs IsNot Nothing Then
                Dim lStationInfo As ArrayList = lBFGroupAttribs.GetValue("StationInfo")
                If lStationInfo IsNot Nothing Then
                    lTotalStations = lStationInfo.Count
                    Using lProgressLevel As New ProgressLevel(False, lStationInfo.Count = 0)
                        lStnCtr = 1
                        For Each lStation As String In lStationInfo
                            Dim lStationID As String = ""
                            Try
                                lStationID = lStation.Split(vbTab)(1).Split(",")(0)
                            Catch ex As Exception
                                lStnCtr += 1
                                Continue For
                            End Try
                            Logger.Progress("reading station: " & lStationID, lStnCtr, lTotalStations)
                            If Not String.IsNullOrEmpty(lStationID) AndAlso lStationID.Length = 8 Then
                                Dim lDataPath As String = GetDataFileFullPath(lStationID)
                                Dim lTsGroupTemp As atcTimeseriesGroup = clsBatchUtil.ReadTSFromRDB(lDataPath, lArgs)
                                If lTsGroupTemp IsNot Nothing AndAlso lTsGroupTemp.Count > 0 Then
                                    lTsGroup.Add(lTsGroupTemp(0))
                                    LogStationInfo(lTsGroupTemp(0), lStationID, lDataPath)
                                End If
                            End If
                            lStnCtr += 1
                        Next
                    End Using
                End If
            End If
            lGroupCtr += 1
        Next
        If lTsGroup.Count > 0 Then
            CommonDates(lTsGroup, pDataStart, pDataEnd, pDataStartCommon, pDataEndCommon)
        Else
            pDataStart = 0
            pDataEnd = 0
            pDataStartCommon = 0
            pDataEndCommon = 0
        End If
        pDataGroupsChanged = False
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        If Not pLoaded Then Exit Sub
        pDataGroupsChanged = True
        Try
            SetDataTimeSpan()
        Catch ex As Exception
            pDataGroupsChanged = False
        End Try
    End Sub
End Class