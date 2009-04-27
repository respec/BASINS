Module modGIStoWasp
    '******************************************************************************
    '   Application: Mercury Model - Hydrology
    '   Company:     Tetra Tech, Inc
    '   File Name:   ModuleGISToWasp
    '   Purpose:     Creates a new table for selected reaches
    '   Author:      Mira Chokshi
    '   Modification History:   Created - 03/03/2004
    '
    '
    '******************************************************************************
    Public pSegmentTravelTime As Double
    Public pSegmentLengthMeters As Double

    Public Function StepRunDrainPreprocessorForWASP() As Boolean
        Try
            Dim frm As New frmProcess
            frm.Show()

            If GetInputLayer("Drain") Is Nothing Then
                WarningMsg("Drain Layer missing, Define Drain in Data Management.")
                frm.Close()
                Return False
            End If

            If Not Step0ExportSelectedStreamsToBranchesLayer() Then
                frm.Close()
                Return False
            End If

            If StepRunDrainPreprocessorForWASP Then
                Step1AddStreamDetailsToBranches()
                '* Call step 2 and step 3 to compute velocity, travel time and label the branches
                'Step2LabelBranchesAndDownstreamNetwork()
                'Step3ComputeVelocityAndTravelTimeForBranches()

                'Call module to compute time and length range
                Dim pMinSegmentLen, pMaxSegmentLen, pMinSegmentTime, pMaxSegmentTime, pMinVelocity As Double
                'modGIStoWasp.FindSegmentationLengthAndTimeRange(pMinSegmentLen, pMaxSegmentLen, pMinSegmentTime, pMaxSegmentTime, pMinVelocity)
                If 2 * gCellSize > pMaxSegmentLen Then
                    MsgBox("Select streams longer than 2 times DEM GRID size")
                    Return False
                Else
                    frm.Close()
                    With New frmWASPInterface
                        .ShowDialog()
                        .Dispose()
                    End With
                    Return True
                End If
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function

    ''' <summary>
    ''' Create new layer containing all selected streams on the Drain layer and place on Branches layer
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Step0ExportSelectedStreamsToBranchesLayer() As Boolean
        Try
            If Not GisUtil.IsLayer("Drain") Then Return False
            If GisUtil.NumSelectedFeatures(GisUtil.LayerIndex("Drain")) = 0 Then
                ErrorMsg("Use WASP Stream Selection Tool to select streams from Drain theme for the study area. Selected streams must be continuous!")
                Return False
            End If
            DeleteFeatureDataset("Branches")
            GisUtil.SaveSelectedFeatures("Drain", "Branches", True)
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function


    Public Sub Step1AddStreamDetailsToBranches()
        Try
            Dim pNHDFLayer As MapWindow.Interfaces.Layer = GetInputLayer("NHD")
            Dim pNHDFIndex As Integer = GisUtil.LayerIndex("NHD")

            If pNHDFLayer Is Nothing Then ErrorMsg("NHD Layer is missing!") : Exit Sub

            If Not (GisUtil.IsField(pNHDFIndex, "SLOPE") And GisUtil.IsField(pNHDFIndex, "Roughness")) Then ErrorMsg("Need to run 'Process Input Grids' first") : Exit Sub

            Exit Sub

            'following not needed!!!???!!!

            Dim pBranchesLayer As MapWindow.Interfaces.Layer = GetInputLayer("Branches")
            Dim pBranchesIndex As Integer = GisUtil.LayerIndex("Branches")

            If pBranchesLayer Is Nothing Then ErrorMsg("Branches Layer is missing!") : Exit Sub

            If Not GisUtil.IsField(pBranchesIndex, "NAME") Then GisUtil.AddField(pBranchesIndex, "NAME", MapWinGIS.FieldType.STRING_FIELD, 99)
            If Not GisUtil.IsField(pBranchesIndex, "SLOPE") Then GisUtil.AddField(pBranchesIndex, "SLOPE", MapWinGIS.FieldType.DOUBLE_FIELD, 11) 'may need to define precision 10 and scale 5?
            If Not GisUtil.IsField(pBranchesIndex, "ROUGHNESS") Then GisUtil.AddField(pBranchesIndex, "ROUGHNESS", MapWinGIS.FieldType.DOUBLE_FIELD, 11) 'may need to define precision 10 and scale 5?
            If Not GisUtil.IsField(pBranchesIndex, "RADIUS") Then GisUtil.AddField(pBranchesIndex, "RADIUS", MapWinGIS.FieldType.DOUBLE_FIELD, 11) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "WIDTH") Then GisUtil.AddField(pBranchesIndex, "WIDTH", MapWinGIS.FieldType.DOUBLE_FIELD, 11) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "DEPTH") Then GisUtil.AddField(pBranchesIndex, "DEPTH", MapWinGIS.FieldType.DOUBLE_FIELD, 11) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "FID") Then GisUtil.AddField(pBranchesIndex, "FID", MapWinGIS.FieldType.INTEGER_FIELD, 10) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "BRANCH") Then GisUtil.AddField(pBranchesIndex, "BRANCH", MapWinGIS.FieldType.INTEGER_FIELD, 10) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "SEGMENT") Then GisUtil.AddField(pBranchesIndex, "SEGMENT", MapWinGIS.FieldType.INTEGER_FIELD, 10) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "DSBRANCH") Then GisUtil.AddField(pBranchesIndex, "DSBRANCH", MapWinGIS.FieldType.INTEGER_FIELD, 10) 'may need to define precision 10 and scale 3?
            If Not GisUtil.IsField(pBranchesIndex, "RCH_COM_ID") Then GisUtil.AddField(pBranchesIndex, "RCH_COM_ID", MapWinGIS.FieldType.INTEGER_FIELD, 10) 'may need to define precision 10 and scale 3?

            'for each feature in Branch shapefile, copy field value from NHD

            'none of this should be necessary...when exported shape file all fields should have been sent!
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub


    '** Subroutine to create branches from streams, downstream most is node 1, branch 1
    'Public Sub Step2LabelBranchesAndDownstreamNetwork()

    '    InitializeInputDataDictionary()
    '    modInit.Initialize()

    '    Dim pSingleBranchDict As Generic.Dictionary(Of String, String)

    '    Dim pSegmentDict As Generic.Dictionary(Of String, String)

    '    Dim pBranchesFLayer As MapWindow.Interfaces.Layer = GetInputLayer("Branches")
    '    Dim pBranchesIndex As Integer = GisUtil.LayerIndex("Branches")

    '    'Get the user entry of reach id being the outlet
    '    Dim reachid As String = CStr(StepFindMostDownstream())

    '    pSingleBranchDict.Add(reachid, 0)
    '    pSegmentDict.Add(reachid, 1)
    '    Dim upstreamreach As String
    '    Dim oneupstreamreach As String

    '    Dim lstStream As New Generic.List(Of Integer)

    '    Dim pSelectionCount As Integer
    '    Dim iBranchCounter As Integer
    '    iBranchCounter = 0
    '    Dim iBranchID As Integer
    '    Dim iSegmentCounter As Integer
    '    Dim pFldFID As Integer = GisUtil.FieldIndex(pBranchesIndex, "FID")
    '    Dim pFldBranch As Integer = GisUtil.FieldIndex(pBranchesIndex, "BRANCH")
    '    Dim pFldSegment As Integer = GisUtil.FieldIndex(pBranchesIndex, "SEGMENT")
    '    Dim pFldDSBranch As Integer = GisUtil.FieldIndex(pBranchesIndex, "DSBRANCH")

    '    Do
    '        pSelectionCount = 0
    '        'find feature having specified reach ID (could user query, but for now, just loop)
    '        Dim pFeatureIndex As Integer = -1
    '        For i As Integer = 0 To GisUtil.NumFeatures(pBranchesIndex) - 1
    '            If GisUtil.FieldValue(pBranchesIndex, i, pFldFID) = reachid Then
    '                pFeatureIndex = i
    '                Exit For
    '            End If
    '        Next
    '        If pFeatureIndex <> -1 And pFeatureIndex <> GisUtil.NumFeatures(pBranchesIndex) - 1 Then
    '            If GisUtil.FieldValue(pBranchesIndex, pFeatureIndex, pFldFID) = 0 Then
    '                iBranchID = pSingleBranchDict(reachid) 'if it is continuing branch id, use it, else create a new one
    '                If iBranchID = 0 Then
    '                    iBranchCounter = iBranchCounter + 1
    '                    iBranchID = iBranchCounter
    '                    pSegmentDict.Add(reachid, 1)
    '                End If
    '                iSegmentCounter = pSegmentDict.Item(reachid)
    '                GisUtil.SetFeatureValue(pBranchesIndex, pFldBranch, pFeatureIndex, iBranchID)
    '                GisUtil.SetFeatureValue(pBranchesIndex, pFldSegment, pFeatureIndex, iSegmentCounter)
    '            End If

    '            'Query the same table to find all upstream reaches, add them to stream collection
    '            'NOTE: arcgis uses spatial filter to get connecting shapes (?); not sure how to reproduce this; maybe can use field values

    '            'With pSpatialFilter
    '            '    .Geometry = pFeature.Shape
    '            'End With
    '            'pFeature = Nothing
    '            'pFeatureCursor = Nothing

    '            'pFeatureCursor = pBranchesFClass.Update(pSpatialFilter, True)
    '            'pFeatureSelection = pBranchesFLayer
    '            'pSelectionSet = pFeatureSelection.SelectionSet
    '            'pFeature = pFeatureCursor.NextFeature
    '            'Do While Not pFeature Is Nothing
    '            '    upstreamreach = pFeature.Value(lFldFID)
    '            '    If Not (pSingleBranchDict.Exists(upstreamreach)) Then
    '            '        pSingleBranchDict.let_Item(upstreamreach, 0)
    '            '        streamCollection.Add(upstreamreach)
    '            '        oneupstreamreach = upstreamreach
    '            '        pSelectionCount = pSelectionCount + 1
    '            '        pFeature.Value(lFldDSBranch) = iBranchID
    '            '        pFeatureCursor.UpdateFeature(pFeature)
    '            '    End If
    '            '    pFeature = pFeatureCursor.NextFeature
    '            'Loop
    '            'If (pSelectionCount = 1) Then
    '            '    iSegmentCounter = pSegmentDict.Item(reachid)
    '            '    pSingleBranchDict.let_Item(oneupstreamreach, iBranchID)
    '            '    pSegmentDict.let_Item(oneupstreamreach, iSegmentCounter + 1)
    '            'End If
    '            'pFeature = Nothing
    '        End If
    '        'Get the next branch from the streamcollection
    '        If lstStream.Count() = 0 Then Exit Do
    '        reachid = lstStream.Item(0)
    '        lstStream.Remove(0)
    '    Loop

    '    If pSingleBranchDict.Count <> GisUtil.NumFeatures(pBranchesIndex) Then
    '        ErrorMsg("Some of the branches are missing, the selection is not continous.")
    '        Exit Sub
    '    End If

    '    pSingleBranchDict.Clear()
    '    pSingleBranchDict = Nothing
    '    lstStream.Clear()
    '    lstStream = Nothing
    'End Sub

    '** Subroutine to compute velocity and travel time for each branch
    Public Sub Step3ComputeVelocityAndTravelTimeForBranches()
        'Try
        '    Dim pBranchesFLayer As MapWindow.Interfaces.Layer
        '    pBranchesFLayer = GetInputLayer("Branches")

        '    Dim pBranchesFClass As IFeatureClass
        '    pBranchesFClass = pBranchesFLayer.FeatureClass

        '    Dim lFldBranch As Integer
        '    lFldBranch = pBranchesFClass.FindField("Branch")

        '    Dim lFldSlope As Integer
        '    lFldSlope = pBranchesFClass.FindField("Slope")

        '    Dim lFldRoughness As Integer
        '    lFldRoughness = pBranchesFClass.FindField("Roughness")

        '    Dim lFldRadius As Integer
        '    lFldRadius = pBranchesFClass.FindField("Radius")

        '    Dim lFldLength As Integer
        '    lFldLength = pBranchesFClass.FindField("Meters")

        '    'Check if velocity field is present, if not add velocity field
        '    Dim lFldVelocity As Integer
        '    lFldVelocity = pBranchesFClass.FindField("Velocity")
        '    Dim pFieldVelocity As IField
        '    Dim pFieldEditVelocity As IFieldEdit
        '    If lFldVelocity < 0 Then
        '        pFieldVelocity = New Field
        '        pFieldEditVelocity = pFieldVelocity
        '        With pFieldEditVelocity
        '            .name = "Velocity"
        '            .AliasName = "Velocity"
        '            .Precision = 10
        '            .Length = 10
        '            .Scale = 5
        '        End With
        '        pBranchesFClass.AddField(pFieldVelocity)
        '        lFldVelocity = pBranchesFClass.FindField("Velocity")
        '    End If

        '    Dim TotalBranches As Short
        '    TotalBranches = 0
        '    Dim pVelocity As Double
        '    Dim pSlope As Double
        '    Dim pRoughness As Double
        '    Dim pRadius As Double
        '    Dim pBranchID As Double

        '    Dim pFeatureCursor As IFeatureCursor
        '    pFeatureCursor = pBranchesFClass.Update(Nothing, True)

        '    Dim pFeature As IFeature
        '    pFeature = pFeatureCursor.NextFeature

        '    Do While Not pFeature Is Nothing
        '        pRadius = pFeature.Value(lFldRadius)
        '        pRoughness = pFeature.Value(lFldRoughness)
        '        pSlope = pFeature.Value(lFldSlope)
        '        pVelocity = 1 / pRoughness * (pRoughness ^ (2 / 3)) * System.Math.Sqrt(pSlope)
        '        pFeature.Value(lFldVelocity) = pVelocity
        '        pBranchID = pFeature.Value(lFldBranch)
        '        If (pBranchID > TotalBranches) Then
        '            TotalBranches = pBranchID
        '        End If
        '        pFeatureCursor.UpdateFeature(pFeature)
        '        pFeature = pFeatureCursor.NextFeature
        '    Loop
        '    pFeature = Nothing
        '    pFeatureCursor = Nothing

        '    ' Find the travel time field, add if not found
        '    Dim lFldTravelTime As Integer
        '    lFldTravelTime = pBranchesFClass.FindField("TravelTime")
        '    Dim pFieldTravelTime As IField
        '    Dim pFieldEditTravelTime As IFieldEdit
        '    If lFldTravelTime < 0 Then
        '        pFieldTravelTime = New Field
        '        pFieldEditTravelTime = pFieldTravelTime
        '        With pFieldEditTravelTime
        '            .name = "TravelTime"
        '            .AliasName = "TravelTime"
        '            .Precision = 10
        '            .Length = 10
        '            .Scale = 5
        '        End With
        '        pBranchesFClass.AddField(pFieldTravelTime)
        '        lFldTravelTime = pBranchesFClass.FindField("TravelTime")
        '    End If

        '    Dim pTotalLength As Double
        '    Dim pWeightedVelocity As Double
        '    Dim pTravelTime As Double

        '    Dim pQueryFilter As IQueryFilter
        '    pQueryFilter = New QueryFilter

        '    Dim iBranch As Short
        '    Dim pStreamtimeRaster As IRaster
        '    pStreamtimeRaster = OpenRasterDatasetFromDisk("Streamtime")

        '    If pStreamtimeRaster Is Nothing Then Err.Raise(vbObjectError + 5002, , "Streamtime grid is missing. Need to run 'Process Input Grids' first")

        '    DeleteRasterDataset("BranchG")

        '    Dim pBranchRasterDS As IRasterDataset

        '    Dim pBranchRaster As IRaster
        '    pBranchRaster = pBranchRasterDS.CreateDefaultRaster

        '    Dim pWSF As IWorkspaceFactory
        '    pWSF = New RasterWorkspaceFactory
        '    Dim pRWS As IRasterWorkspace2
        '    pRWS = pWSF.OpenFromFile(gMapTempFolder, 0)

        '    Dim pStreamtimeRasterProps As IRasterProps
        '    pStreamtimeRasterProps = pStreamtimeRaster

        '    Dim pZonalOp As IZonalOp
        '    pZonalOp = New RasterZonalOp
        '    Dim pRAEnv As IRasterAnalysisEnvironment
        '    pRAEnv = pZonalOp
        '    pRAEnv.OutSpatialReference = pStreamtimeRasterProps.SpatialReference
        '    pRAEnv.OutWorkspace = pRWS
        '    pRAEnv.Mask = pStreamtimeRaster

        '    'Create zonal statistics table
        '    Dim pTable As iTable
        '    pTable = pZonalOp.ZonalStatisticsAsTable(pBranchRaster, pStreamtimeRaster, True)

        '    Dim branchMinDict As Scripting.Dictionary
        '    branchMinDict = New Scripting.Dictionary
        '    Dim branchMaxDict As Scripting.Dictionary
        '    branchMaxDict = New Scripting.Dictionary

        '    Dim pCursor As ICursor
        '    Dim pRow As iRow

        '    Dim minStreamtime As Double
        '    Dim maxStreamtime As Double
        '    Dim meanStreamtime As Double

        '    pCursor = pTable.Search(Nothing, False)
        '    pRow = pCursor.NextRow
        '    Do While Not pRow Is Nothing
        '        maxStreamtime = pRow.Value(pTable.FindField("Max"))
        '        minStreamtime = pRow.Value(pTable.FindField("Min"))
        '        'if the branch happens to extend outside the boundaries
        '        If minStreamtime = 0 Then
        '            meanStreamtime = pRow.Value(pTable.FindField("Mean"))
        '            minStreamtime = meanStreamtime - (maxStreamtime - meanStreamtime)
        '            If minStreamtime < 0 Then minStreamtime = System.Math.Abs(minStreamtime)
        '        End If
        '        branchMinDict.let_Item(pRow.Value(pTable.FindField("Value")), minStreamtime)
        '        branchMaxDict.let_Item(pRow.Value(pTable.FindField("Value")), maxStreamtime)
        '        pRow = pCursor.NextRow
        '    Loop

        '    For iBranch = 1 To TotalBranches
        '        minStreamtime = branchMinDict.Item(iBranch)
        '        maxStreamtime = branchMaxDict.Item(iBranch)

        '        pTotalLength = 0
        '        pQueryFilter.WhereClause = "Branch = " & iBranch
        '        pFeatureCursor = pBranchesFClass.Update(pQueryFilter, True)
        '        pFeature = pFeatureCursor.NextFeature
        '        Do While Not pFeature Is Nothing
        '            pTotalLength = pTotalLength + pFeature.Value(lFldLength)
        '            pFeature = pFeatureCursor.NextFeature
        '        Loop
        '        pFeature = Nothing
        '        pFeatureCursor = Nothing

        '        'Rerun the query and update all the values again
        '        pFeatureCursor = pBranchesFClass.Update(pQueryFilter, True)
        '        pFeature = pFeatureCursor.NextFeature
        '        pTravelTime = System.Math.Abs(maxStreamtime - minStreamtime)

        '        If pTravelTime > 0 Then
        '            pWeightedVelocity = pTotalLength / (pTravelTime * 3600)
        '        Else
        '            pWeightedVelocity = 0
        '        End If
        '        Do While Not pFeature Is Nothing
        '            pFeature.Value(lFldTravelTime) = pTravelTime
        '            pFeature.Value(lFldVelocity) = pWeightedVelocity
        '            pFeatureCursor.UpdateFeature(pFeature)
        '            pFeature = pFeatureCursor.NextFeature
        '        Loop
        '        pFeature = Nothing
        '        pFeatureCursor = Nothing
        '    Next
        'Catch ex As Exception
        '    ErrorMsg(, ex)
        'End Try
    End Sub

    Public Sub Step4CreateLinkageToWASPmodel()
        '        Try
        '            Dim StepPointWithinDEM As Object
        '            Dim pPoint As Object
        '            On Error GoTo ShowError

        '            InitializeInputDataDictionary()
        '            ModuleInit.Initialize()

        '            Dim pPourPointFLayer As MapWindow.Interfaces.Layer
        '            pPourPointFLayer = GetInputLayer("AssessPoints")

        '            If Not pPourPointFLayer Is Nothing Then DeleteLayerFromMap("AssessPoints")

        '            Dim pWaspTableInfoDict As Scripting.Dictionary
        '            pWaspTableInfoDict = CreateObject("Scripting.Dictionary")
        '            Dim pRiverMileInfoDict As Scripting.Dictionary
        '            pRiverMileInfoDict = CreateObject("Scripting.Dictionary")

        '            Dim pBranchesFLayer As MapWindow.Interfaces.Layer
        '            pBranchesFLayer = GetInputLayer("Branches")

        '            Dim pBranchesFClass As IFeatureClass
        '            pBranchesFClass = pBranchesFLayer.FeatureClass
        '            Dim lFldBranch As Integer
        '            lFldBranch = pBranchesFClass.FindField("Branch")
        '            Dim lFldDSBranch As Integer
        '            lFldDSBranch = pBranchesFClass.FindField("DSBranch")
        '            Dim lFldSegment As Integer
        '            lFldSegment = pBranchesFClass.FindField("Segment")
        '            Dim lFldSegmentName As Integer
        '            lFldSegmentName = pBranchesFClass.FindField("Name")
        '            Dim lFldAvgWidth As Integer
        '            lFldAvgWidth = pBranchesFClass.FindField("Width")
        '            Dim lFldAvgDepth As Integer
        '            lFldAvgDepth = pBranchesFClass.FindField("Depth")
        '            Dim lFldAvgVelocity As Integer
        '            lFldAvgVelocity = pBranchesFClass.FindField("Velocity")
        '            Dim lFldSlope As Integer
        '            lFldSlope = pBranchesFClass.FindField("Slope")
        '            Dim lFldRoughness As Integer
        '            lFldRoughness = pBranchesFClass.FindField("Roughness")

        '            Dim pWASPTable As iTable
        '            pWASPTable = CreateDBFForWasp("Wasp")

        '            Dim pQueryFilter As IQueryFilter
        '            pQueryFilter = New QueryFilter
        '            Dim pFeatureCursor As IFeatureCursor
        '            Dim pFeature As IFeature

        '            Dim ContinueLoop As Boolean
        '            ContinueLoop = True

        '            Dim pPolyline As IPolyline
        '            Dim pGeoBag As IGeometryCollection
        '            Dim pTopoOp As ITopologicalOperator
        '            Dim iBranch As Short
        '            Dim pPourPointsFClass As IFeatureClass
        '            pPourPointsFClass = CreateFeatureClassForPointShapeFile(gMapTempFolder, "assesspoints")
        '            Dim lFldDSID As Integer
        '            lFldDSID = pPourPointsFClass.FindField("DSID")
        '            Dim pFieldDSID As IField
        '            Dim pFieldEditDSID As IFieldEdit
        '            If lFldDSID < 0 Then
        '                pFieldDSID = New Field
        '                pFieldEditDSID = pFieldDSID
        '                With pFieldEditDSID
        '                    .name = "DSID"
        '                    .Length = 10
        '                End With
        '                pPourPointsFClass.AddField(pFieldDSID)
        '                lFldDSID = pPourPointsFClass.FindField("DSID")
        '            End If

        '            Dim lFldHeadWater As Integer
        '            lFldHeadWater = pPourPointsFClass.FindField("HeadWater")
        '            Dim pFieldHeadWater As IField
        '            Dim pFieldEditHeadWater As IFieldEdit
        '            If lFldHeadWater < 0 Then
        '                pFieldHeadWater = New Field
        '                pFieldEditHeadWater = pFieldHeadWater
        '                With pFieldEditHeadWater
        '                    .name = "HeadWater"
        '                    '            .Type = esriFieldTypeString
        '                    .Length = 10
        '                End With
        '                pPourPointsFClass.AddField(pFieldHeadWater)
        '                lFldHeadWater = pPourPointsFClass.FindField("HeadWater")
        '            End If

        '            Dim pPointFeat As IFeature
        '            Dim isegment As Short
        '            Dim ilength As Double
        '            Dim totalpts As Short
        '            Dim iDistance As Double

        '            'Define variables for inserting into the dictionary
        '            Dim pSpatialFilter As ISpatialFilter
        '            pSpatialFilter = New SpatialFilter
        '            Dim pSegmentID As Short
        '            Dim pBranchID As Short
        '            Dim pRiverMile As Double
        '            Dim pSegmentName As String
        '            Dim pLengthMeters As Double
        '            Dim pBranchTravelTime As Double
        '            Dim pAverageWidth As Double
        '            Dim pAverageDepth As Double
        '            Dim pAverageVelocity As Double
        '            Dim pSlope As Double
        '            Dim pRoughness As Double
        '            Dim pBranchCursor As IFeatureCursor
        '            Dim pBranchFeature As IFeature
        '            pSegmentID = 0
        '            Dim i As Short
        '            Dim boolHeadWater As String
        '            Dim pDownstreamID As Short
        '            Dim pDSbranch As Short
        '            Dim iBranchDistance As Double
        '            Dim pPointCursor As IFeatureCursor

        '            Dim pBranchRelationDict As Scripting.Dictionary
        '            pBranchRelationDict = CreateObject("Scripting.Dictionary")
        '            Dim pUpBranch As Short
        '            Dim pDnBranch As Short
        '            'Prepare the branch relation dictionary
        '            pQueryFilter.WhereClause = "Segment = 1"
        '            pFeatureCursor = pBranchesFClass.Search(pQueryFilter, True)
        '            pFeature = pFeatureCursor.NextFeature
        '            Do While Not pFeature Is Nothing
        '                pUpBranch = pFeature.Value(lFldBranch)
        '                pDnBranch = pFeature.Value(lFldDSBranch)
        '                pBranchRelationDict.Add(pUpBranch, pDnBranch)
        '                pFeature = pFeatureCursor.NextFeature
        '            Loop
        '            pFeatureCursor = Nothing
        '            pFeature = Nothing


        '            iBranch = 1
        '            Dim pCurrentPt As Short
        '            pCurrentPt = 0
        '            Dim pDnPt As Short
        '            Dim isAlongFlowDir As Boolean
        '            Do While ContinueLoop
        '                pQueryFilter.WhereClause = "Branch = " & iBranch
        '                pFeatureCursor = pBranchesFClass.Search(pQueryFilter, True)
        '                pFeature = pFeatureCursor.NextFeature
        '                If (pFeature Is Nothing) Then
        '                    Exit Do
        '                End If

        '                'Generate one line from different segments
        '                pBranchTravelTime = pFeature.Value(pFeatureCursor.FindField("TravelTime"))
        '                pGeoBag = New GeometryBag
        '                isegment = 0
        '                Do While Not pFeature Is Nothing
        '                    pGeoBag.AddGeometry(pFeature.ShapeCopy, isegment)
        '                    isegment = isegment + 1
        '                    pFeature = pFeatureCursor.NextFeature
        '                Loop
        '                pFeatureCursor = Nothing
        '                pFeature = Nothing
        '                pTopoOp = New Polyline
        '                pTopoOp.ConstructUnion(pGeoBag)
        '                pPolyline = pTopoOp

        '                isAlongFlowDir = True
        '                If (StepReversePolyLineOrientation(pPolyline)) Then
        '                    isAlongFlowDir = False
        '                End If

        '                ilength = pPolyline.Length

        '                '** Decide the segment length
        '                If (pSegmentTravelTime > 0) Then
        '                    totalpts = CShort((pBranchTravelTime / pSegmentTravelTime) + 0.5)
        '                Else
        '                    totalpts = CShort(ilength / pSegmentLengthMeters)
        '                End If

        '                'Make sure that there will be at least one point on most downstream branch
        '                If iBranch = 1 And totalpts = 0 Then totalpts = 1


        '                If (totalpts > 0) Then
        '                    pLengthMeters = ilength / totalpts
        '                Else
        '                    pLengthMeters = 0
        '                End If

        '                pDnBranch = pBranchRelationDict.Item(iBranch)
        '                pDnPt = pWaspTableInfoDict.Item(pDnBranch)
        '                If (pDnPt = 0) Then
        '                    pDnBranch = pBranchRelationDict.Item(pDnBranch)
        '                    pDnPt = pWaspTableInfoDict.Item(pDnBranch)
        '                End If
        '                For i = 0 To totalpts - 1

        '                    If i = 0 Then
        '                        If isAlongFlowDir Then
        '                            iDistance = ilength - (2 * gCellSize)
        '                        Else
        '                            iDistance = 2 * gCellSize
        '                        End If
        '                    Else
        '                        If isAlongFlowDir Then
        '                            iDistance = ilength - (i * pLengthMeters)
        '                        Else
        '                            iDistance = i * pLengthMeters
        '                        End If
        '                    End If

        '                    iBranchDistance = iDistance '+ iBranchDistance

        '                    'Create points a certain distance on this path

        '                    If (StepPointWithinDEM(pPoint)) Then
        '                        'Construct a spatial query to find the reach on which the point lies
        '                        With pSpatialFilter
        '                            .Geometry = pPoint
        '                        End With
        '                        pPointCursor = Nothing
        '                        pPointCursor = pPourPointsFClass.Search(pSpatialFilter, True)
        '                        pPointFeat = Nothing
        '                        pPointFeat = pPointCursor.NextFeature
        '                        If (pPointFeat Is Nothing) Then
        '                            pCurrentPt = pCurrentPt + 1
        '                            pBranchCursor = pBranchesFClass.Search(pSpatialFilter, True)
        '                            pBranchFeature = pBranchCursor.NextFeature
        '                            If Not (pBranchFeature Is Nothing) Then
        '                                pSegmentID = pCurrentPt
        '                                pBranchID = iBranch
        '                                pRiverMile = iBranchDistance / 1000 * 0.6213712
        '                                pSegmentName = pBranchFeature.Value(lFldSegmentName)
        '                                pAverageWidth = pBranchFeature.Value(lFldAvgWidth)
        '                                pAverageDepth = pBranchFeature.Value(lFldAvgDepth)
        '                                pAverageVelocity = pBranchFeature.Value(lFldAvgVelocity)
        '                                pSlope = pBranchFeature.Value(lFldSlope)
        '                                pRoughness = pBranchFeature.Value(lFldRoughness)
        '                                AddRowToWASPtable(pWASPTable, pBranchID, pSegmentID, pRiverMile, pSegmentName, pLengthMeters, pAverageDepth, pAverageWidth, pAverageVelocity, pSlope, pRoughness, pSegmentID)
        '                                'Debug.Print pBranchID, pSegmentID, pRiverMile, pSegmentName, pAverageDepth, pAverageWidth, pAverageVelocity, pSlope, pRoughness
        '                            End If
        '                            boolHeadWater = "N"
        '                            pBranchCursor = Nothing
        '                            pBranchFeature = Nothing
        '                            'Save this point feature in a feature class
        '                            pPointFeat = pPourPointsFClass.CreateFeature
        '                            pPointFeat.Shape = pPoint
        '                            pPointFeat.Value(pPourPointsFClass.FindField("ID")) = pCurrentPt
        '                            pPointFeat.Value(pPourPointsFClass.FindField("TYPE")) = "U"
        '                            pPointFeat.Value(lFldDSID) = pDnPt
        '                            pPointFeat.Value(lFldHeadWater) = boolHeadWater
        '                            pPointFeat.Store()
        '                            pWaspTableInfoDict.let_Item(iBranch, pCurrentPt)
        '                            pDnPt = pCurrentPt
        '                        End If 'duplicate point check
        '                    End If 'StepPointWithinDEM check
        'Nextfor:
        '                Next
        '                pFeatureCursor = Nothing
        '                pFeature = Nothing
        '                pPolyline = Nothing
        '                pPointFeat = Nothing
        '                pPoint = Nothing
        '                pTopoOp = Nothing
        '                pGeoBag = Nothing
        '                'Next branch
        '                iBranch = iBranch + 1
        '            Loop

        '            'Update the downstream id for each point
        '            pBranchRelationDict.RemoveAll()
        '            pPointCursor = Nothing
        '            pPointCursor = pPourPointsFClass.Search(Nothing, True)
        '            pPointFeat = pPointCursor.NextFeature
        '            Do While Not pPointFeat Is Nothing
        '                pBranchRelationDict.let_Item(pPointFeat.Value(lFldDSID), "Y")
        '                pPointFeat = pPointCursor.NextFeature
        '            Loop
        '            pPointFeat = Nothing
        '            pPointFeat = pPointCursor.NextFeature
        '            pPointCursor = Nothing

        '            pPointCursor = pPourPointsFClass.Update(Nothing, True)
        '            pPointFeat = pPointCursor.NextFeature
        '            Do While Not pPointFeat Is Nothing
        '                pCurrentPt = pPointFeat.Value(pPourPointsFClass.FindField("ID"))
        '                If (pBranchRelationDict.Item(pCurrentPt) <> "Y") Then
        '                    pPointFeat.Value(lFldHeadWater) = "Y"
        '                Else
        '                    pPointFeat.Value(lFldHeadWater) = "N"
        '                End If
        '                pPointCursor.UpdateFeature(pPointFeat)
        '                pPointFeat = pPointCursor.NextFeature
        '            Loop
        '            pPointFeat = Nothing
        '            pPointCursor = Nothing


        '            Dim pPourPointLayer As MapWindow.Interfaces.Layer
        '            pPourPointLayer = GetInputLayer("AssessPoints")
        '            If Not (pPourPointLayer Is Nothing) Then
        '                DeleteLayerFromMap(("AssessPoints"))
        '            End If

        '            pPourPointLayer = Nothing
        '            pPourPointLayer = New FeatureLayer
        '            pPourPointLayer.FeatureClass = pPourPointsFClass
        '            '* Call step 5 to compute network route for each headwater
        '            'Step5CreateNetworkRoutesForUpstreamPourPoints

        '            FileCopy(gMapInputFolder & "\wasp.dbf", gMapInputFolder & "\wasp2.dbf")
        '        Catch ex As Exception
        '            ErrorMsg(, ex)
        '        End Try
    End Sub

    ' createDBF: simple function to create a DBASE file.
    ' note: the name of the DBASE file should not contain the .dbf extension
    'Public Function CreateDBFForWasp(ByRef strname As String) As iTable
    '    Try
    '        ' Delete the table from the map and disk
    '        DeleteDataTable(gModelWASPOutputFolder, strname)

    '        ' Open the Workspace
    '        Dim pWorkspaceFactory As IWorkspaceFactory
    '        pWorkspaceFactory = New ShapefileWorkspaceFactory
    '        Dim pFWS As IFeatureWorkspace
    '        pFWS = pWorkspaceFactory.OpenFromFile(gModelWASPOutputFolder, 0)

    '        Dim pFieldsEdit As IFieldsEdit
    '        Dim pFieldEdit As IFieldEdit
    '        Dim pField As IField
    '        Dim pFields As IFields

    '        ' if a fields collection is not passed in then create one
    '        ' create the fields used by our object
    '        pFields = New Fields
    '        pFieldsEdit = pFields
    '        pFieldsEdit.FieldCount = 11

    '        'Create BRANCH Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Branch"
    '            '        .Type = esriFieldTypeInteger
    '            .Length = 10
    '        End With
    '        pFieldsEdit.Field(0) = pField

    '        'Create SEGMENT Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Segment"
    '            '        .Type = esriFieldTypeInteger
    '            .Length = 10
    '        End With
    '        pFieldsEdit.Field(1) = pField

    '        'Create RIVERMILE Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "River_mile"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 2
    '        End With
    '        pFieldsEdit.Field(2) = pField

    '        'Create SEGMENTNAME Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "SegName"
    '            '        .Type = esriFieldTypeString
    '            .Length = 30
    '        End With
    '        pFieldsEdit.Field(3) = pField

    '        'Create LENGTH Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Length_m"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(4) = pField

    '        'Create WIDTH Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "AvgWidth_m"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(5) = pField

    '        'Create DEPTH Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "AvgDepth_m"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(6) = pField

    '        'Create VELOCITY Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Velocity_m_s"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(7) = pField

    '        'Create SLOPE Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Slope"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(8) = pField

    '        'Create ROUGHNESS Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Roughness"
    '            '        .Type = esriFieldTypeDouble
    '            .Length = 10
    '            .Precision = 10
    '            .Scale = 5
    '        End With
    '        pFieldsEdit.Field(9) = pField

    '        'Create FILE Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "OutputFile"
    '            '        .Type = esriFieldTypeString
    '            .Length = 30
    '        End With
    '        pFieldsEdit.Field(10) = pField

    '        CreateDBFForWasp = pFWS.CreateTable(strname, pFields, Nothing, Nothing, "")
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function

    'Private Sub AddRowToWASPtable(ByRef pWASPTable As iTable, ByRef iBranch As Short, ByRef isegment As Short, ByRef irivermile As Double, ByRef isegmentname As String, ByRef ilengthmeters As Double, ByRef iavgdepth As Double, ByRef iavgwidth As Double, ByRef iavgvelocity As Double, ByRef islope As Double, ByRef iroughness As Double, ByRef ipourpoint As Short)
    '    Try
    '        Dim wspFldBranch As Integer
    '        wspFldBranch = pWASPTable.FindField("Branch")
    '        Dim wspFldSegment As Integer
    '        wspFldSegment = pWASPTable.FindField("Segment")
    '        Dim wspFldrivermile As Integer
    '        wspFldrivermile = pWASPTable.FindField("River_mile")
    '        Dim wspFldSegmentName As Integer
    '        wspFldSegmentName = pWASPTable.FindField("SegName")
    '        Dim wspFldLength As Integer
    '        wspFldLength = pWASPTable.FindField("Length_m")
    '        Dim wspFldAvgWidth As Integer
    '        wspFldAvgWidth = pWASPTable.FindField("AvgWidth_m")
    '        Dim wspFldAvgDepth As Integer
    '        wspFldAvgDepth = pWASPTable.FindField("AvgDepth_m")
    '        Dim wspFldAvgVelocity As Integer
    '        wspFldAvgVelocity = pWASPTable.FindField("Velocity_m")
    '        Dim wspFldSlope As Integer
    '        wspFldSlope = pWASPTable.FindField("Slope")
    '        Dim wspFldRoughness As Integer
    '        wspFldRoughness = pWASPTable.FindField("Roughness")
    '        Dim wspFldFile As Integer
    '        wspFldFile = pWASPTable.FindField("OutputFile")

    '        Dim pRow As iRow
    '        pRow = pWASPTable.CreateRow
    '        pRow.Value(wspFldBranch) = iBranch
    '        pRow.Value(wspFldSegment) = isegment
    '        pRow.Value(wspFldrivermile) = irivermile
    '        pRow.Value(wspFldSegmentName) = isegmentname
    '        pRow.Value(wspFldLength) = ilengthmeters
    '        pRow.Value(wspFldAvgWidth) = iavgwidth
    '        pRow.Value(wspFldAvgDepth) = iavgdepth
    '        pRow.Value(wspFldAvgVelocity) = iavgvelocity
    '        pRow.Value(wspFldSlope) = islope
    '        pRow.Value(wspFldRoughness) = iroughness
    '        pRow.Value(wspFldFile) = "SubwatershedOutput_" & ipourpoint & ".out"
    '        pRow.Store()
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub

    '*** Subroutine to create network routes for all upstream pour points
    'Public Sub Step5CreateNetworkRoutesForUpstreamPourPoints()
    '    Try
    '        Dim pRouteDict As Scripting.Dictionary
    '        pRouteDict = CreateObject("Scripting.Dictionary")

    '        'Get the pour points feature layer
    '        Dim pPourPointFLayer As MapWindow.Interfaces.Layer
    '        pPourPointFLayer = GetInputLayer("SnapAssessPoints")
    '        Dim pPourPointFClass As IFeatureClass
    '        pPourPointFClass = pPourPointFLayer.FeatureClass

    '        Dim pIDindex As Integer
    '        pIDindex = pPourPointFClass.FindField("ID")
    '        Dim pDSIDindex As Integer
    '        pDSIDindex = pPourPointFClass.FindField("DSID")

    '        Dim pFeatureCursor As IFeatureCursor
    '        Dim pFeature As IFeature

    '        pFeatureCursor = pPourPointFClass.Search(Nothing, True)
    '        pFeature = pFeatureCursor.NextFeature
    '        Do While Not pFeature Is Nothing
    '            pRouteDict.Add(pFeature.Value(pIDindex), pFeature.Value(pDSIDindex))
    '            pFeature = pFeatureCursor.NextFeature
    '        Loop
    '        pFeature = Nothing
    '        pFeatureCursor = Nothing

    '        Dim pQueryFilter As IQueryFilter
    '        pQueryFilter = New QueryFilter
    '        pQueryFilter.WhereClause = "HeadWater = 'Y'"
    '        pFeatureCursor = pPourPointFClass.Search(pQueryFilter, True)
    '        pFeature = pFeatureCursor.NextFeature

    '        Dim pRouteTable As iTable
    '        Dim pRow As iRow

    '        Dim pFrom As Short
    '        Dim pTo As Short
    '        Dim pFlowPath As Short

    '        Dim pFromindex As Integer
    '        Dim pToindex As Integer
    '        Dim pFlowindex As Integer

    '        Dim pContinueLoop As Boolean
    '        pFlowPath = 0
    '        Do While Not pFeature Is Nothing
    '            pFlowPath = pFlowPath + 1
    '            'Create a new table for new head water point
    '            pRouteTable = CreateDBFForNetworkRoute("Route" & pFlowPath)
    '            pFlowindex = pRouteTable.FindField("Flow")
    '            pFromindex = pRouteTable.FindField("From")
    '            pToindex = pRouteTable.FindField("To")
    '            pFrom = 0
    '            pTo = pFeature.Value(pIDindex)
    '            pContinueLoop = True
    '            Do While pContinueLoop
    '                pRow = pRouteTable.CreateRow
    '                pRow.Value(pFlowindex) = pFlowPath
    '                pRow.Value(pFromindex) = pFrom
    '                pRow.Value(pToindex) = pTo
    '                pRow.Store()
    '                If (pTo = 0) Then
    '                    pContinueLoop = False
    '                Else
    '                    'Get the next route level
    '                    pFrom = pTo
    '                    pTo = pRouteDict.Item(pFrom)
    '                End If
    '            Loop
    '            pRouteTable = Nothing
    '            pFeature = pFeatureCursor.NextFeature
    '        Loop
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub


    '' createDBF: simple function to create a DBASE file.
    '' note: the name of the DBASE file should not contain the .dbf extension
    'Public Function CreateDBFForNetworkRoute(ByRef strname As String) As iTable
    '    Try
    '        ' Delete the table from the map and disk
    '        DeleteDataTable(gModelWASPOutputFolder, strname)

    '        ' Open the Workspace
    '        Dim pWorkspaceFactory As IWorkspaceFactory
    '        pWorkspaceFactory = New ShapefileWorkspaceFactory
    '        Dim pFWS As IFeatureWorkspace
    '        pFWS = pWorkspaceFactory.OpenFromFile(gModelWASPOutputFolder, 0)

    '        Dim pFieldsEdit As IFieldsEdit
    '        Dim pFieldEdit As IFieldEdit
    '        Dim pField As IField
    '        Dim pFields As IFields

    '        ' if a fields collection is not passed in then create one
    '        ' create the fields used by our object
    '        pFields = New Fields
    '        pFieldsEdit = pFields
    '        pFieldsEdit.FieldCount = 3

    '        'Create Flow Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "Flow"
    '            '        .Type = esriFieldTypeInteger
    '            .Length = 10
    '        End With
    '        pFieldsEdit.Field(0) = pField

    '        'Create From Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "From"
    '            '        .Type = esriFieldTypeInteger
    '            .Length = 10
    '        End With
    '        pFieldsEdit.Field(1) = pField

    '        'Create To Field
    '        pField = New Field
    '        pFieldEdit = pField
    '        With pFieldEdit
    '            .name = "To"
    '            '        .Type = esriFieldTypeInteger
    '            .Length = 10
    '        End With
    '        pFieldsEdit.Field(2) = pField

    '        CreateDBFForNetworkRoute = pFWS.CreateTable(strname, pFields, Nothing, Nothing, "")
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function


    'Private Function StepFindMostDownstream() As Integer
    '    Try
    '        Dim pPoint As Object
    '        Dim pOrg As Object

    '        Dim pBranchesFLayer As MapWindow.Interfaces.Layer
    '        pBranchesFLayer = GetInputLayer("Branches")
    '        If pBranchesFLayer Is Nothing Then
    '            Exit Function
    '        End If

    '        Dim pRasterFlowAcc As IRaster
    '        pRasterFlowAcc = OpenRasterDatasetFromDisk("FlowAccu")
    '        Dim pPixelBlockFlowAcc As IPixelBlock3
    '        Dim pRasterPropFlowAcc As IRasterProps
    '        Dim vPixelDataFlowAcc As Object

    '        '    Dim pOrg As esriGeometry.IPoint
    '        Dim pCellSize As Double
    '        Dim pOrigin As IPnt
    '        Dim pSize As IPnt
    '        Dim pLocation As IPnt

    '        Dim iCol As Integer
    '        Dim iRow As Integer
    '        Dim pValueFlowAcc As Double
    '        Dim pHighFlowAcc As Double
    '        '    Dim pFlowAccNoDataValue As Double
    '        Dim cCol As Integer
    '        Dim cRow As Integer

    '        ' get raster properties
    '        pRasterPropFlowAcc = pRasterFlowAcc

    '        '    Set pOrg = New esriGeometry.Point
    '        pOrg.X = pRasterPropFlowAcc.Extent.XMin
    '        pOrg.Y = pRasterPropFlowAcc.Extent.YMax
    '        pCellSize = (pRasterPropFlowAcc.MeanCellSize.X + pRasterPropFlowAcc.MeanCellSize.Y) / 2
    '        '    pFlowAccNoDataValue = pRasterPropFlowAcc.noDataValue(0)
    '        ' create a DblPnt to hold the PixelBlock size
    '        pSize = New DblPnt
    '        pSize.SetCoords(pRasterPropFlowAcc.Width, pRasterPropFlowAcc.Height)
    '        ' create pixelblock the size of the input raster
    '        pPixelBlockFlowAcc = pRasterFlowAcc.CreatePixelBlock(pSize)
    '        ' get vb supported pixel type
    '        pRasterPropFlowAcc.PixelType = GetVBSupportedPixelType(pRasterPropFlowAcc.PixelType)
    '        ' get pixeldata
    '        pOrigin = New DblPnt
    '        pOrigin.SetCoords(0, 0)

    '        pRasterFlowAcc.Read(pOrigin, pPixelBlockFlowAcc)
    '        vPixelDataFlowAcc = pPixelBlockFlowAcc.PixelData(0)
    '        Dim pFeatureClass As IFeatureClass
    '        pFeatureClass = pBranchesFLayer.FeatureClass
    '        Dim pFeatureCursor As IFeatureCursor
    '        pFeatureCursor = pFeatureClass.Search(Nothing, True)
    '        Dim pDSID As Short
    '        Dim pFldFID As Integer
    '        pFldFID = pFeatureCursor.FindField("FID")
    '        Dim pFeature As IFeature
    '        pFeature = pFeatureCursor.NextFeature
    '        Dim pLine As IPolyline
    '        '    Dim pPoint As esriGeometry.IPoint
    '        '    Set pPoint = New esriGeometry.Point
    '        pHighFlowAcc = 0
    '        Dim pBreakDist As Double
    '        Dim pLineBreaks As Short
    '        Do While Not pFeature Is Nothing
    '            pLine = pFeature.Shape
    '            pLine.SpatialReference = pRasterPropFlowAcc.SpatialReference
    '            pBreakDist = pLine.Length / 2
    '            For pLineBreaks = 0 To 2
    '                '            pLine.QueryPoint esriNoExtension, pBreakDist * pLineBreaks, False, pPoint
    '                iRow = CInt(((pOrg.Y - pPoint.Y) / pCellSize) - 0.5)
    '                iCol = CInt(((pPoint.X - pOrg.X) / pCellSize) - 0.5)

    '                For cCol = iCol - 2 To iCol + 2
    '                    For cRow = iRow - 2 To iRow + 2
    '                        If (cRow < pPixelBlockFlowAcc.Height And cRow > 0 And cCol < pPixelBlockFlowAcc.Width And cCol > 0) Then
    '                            ' If this cell has no value for flow accumulation, continue to process next cell
    '                            pValueFlowAcc = vPixelDataFlowAcc(cCol, cRow)
    '                            If ((pValueFlowAcc > pHighFlowAcc)) Then
    '                                pHighFlowAcc = pValueFlowAcc
    '                                pDSID = pFeature.Value(pFldFID)
    '                            End If
    '                        End If
    '                    Next cRow
    '                Next cCol
    '            Next pLineBreaks
    '            pFeature = pFeatureCursor.NextFeature
    '        Loop
    '        'Return the downstream id of the branches
    '        StepFindMostDownstream = pDSID
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function



    'Private Function StepPointWithinDEM(ByVal pPoint As esriGeometry.IPoint) As Boolean
    '    Try
    '        Dim pRasterFlowAcc As IRaster
    '        pRasterFlowAcc = OpenRasterDatasetFromDisk("FlowAccu")
    '        Dim pPixelBlockFlowAcc As IPixelBlock3
    '        Dim pRasterPropFlowAcc As IRasterProps
    '        Dim vPixelDataFlowAcc As Object

    '        Dim pOrg As esriGeometry.IPoint
    '        Dim pCellSize As Double
    '        Dim pOrigin As IPnt
    '        Dim pSize As IPnt
    '        Dim pLocation As IPnt

    '        Dim iCol As Long
    '        Dim iRow As Long
    '        Dim pValueFlowAcc As Double
    '        '    Dim pFlowAccNoDataValue As Double
    '        Dim cCol As Long
    '        Dim cRow As Long

    '        ' get raster properties
    '        pRasterPropFlowAcc = pRasterFlowAcc
    '        pOrg = New esriGeometry.Point
    '        pOrg.X = pRasterPropFlowAcc.Extent.XMin
    '        pOrg.Y = pRasterPropFlowAcc.Extent.YMax
    '        pCellSize = (pRasterPropFlowAcc.MeanCellSize.X + pRasterPropFlowAcc.MeanCellSize.Y) / 2
    '        '    pFlowAccNoDataValue = pRasterPropFlowAcc.noDataValue(0)
    '        ' create a DblPnt to hold the PixelBlock size
    '        pSize = New DblPnt
    '        pSize.SetCoords(pRasterPropFlowAcc.Width, pRasterPropFlowAcc.Height)
    '        ' create pixelblock the size of the input raster
    '        pPixelBlockFlowAcc = pRasterFlowAcc.CreatePixelBlock(pSize)
    '        ' get vb supported pixel type
    '        pRasterPropFlowAcc.PixelType = GetVBSupportedPixelType(pRasterPropFlowAcc.PixelType)
    '        ' get pixeldata
    '        pOrigin = New DblPnt
    '        pOrigin.SetCoords(0, 0)
    '        pRasterFlowAcc.Read(pOrigin, pPixelBlockFlowAcc)
    '        vPixelDataFlowAcc = pPixelBlockFlowAcc.PixelData(0)
    '        iRow = CLng(((pOrg.Y - pPoint.Y) / pCellSize) - 0.5)
    '        iCol = CLng(((pPoint.X - pOrg.X) / pCellSize) - 0.5)

    '        If (iRow < pPixelBlockFlowAcc.Height And iRow > 0 And iCol < pPixelBlockFlowAcc.Width And iCol > 0) Then
    '            pValueFlowAcc = vPixelDataFlowAcc(iCol, iRow)
    '            If pPixelBlockFlowAcc.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
    '                StepPointWithinDEM = True
    '            Else
    '                StepPointWithinDEM = False
    '            End If
    '        Else
    '            StepPointWithinDEM = False
    '        End If
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function

    'Private Function StepReversePolyLineOrientation(ByRef pPolyline As IPolyline) As Boolean
    '    Try
    '        Dim pToPoint As Object
    '        Dim pFromPoint As Object
    '        Dim pOrg As Object

    '        Dim pRasterFlowAcc As IRaster
    '        pRasterFlowAcc = OpenRasterDatasetFromDisk("FlowAccu")
    '        Dim pRasterPropFlowAcc As IRasterProps

    '        Dim pCellSize As Double
    '        Dim pSize As IPnt
    '        Dim pLocation As IPnt

    '        Dim iCol As Integer
    '        Dim iRow As Integer
    '        Dim pValueFlowAcc As Double
    '        Dim pFlowAccFromPt As Double
    '        Dim pFlowAccToPt As Double
    '        Dim cCol As Integer
    '        Dim cRow As Integer

    '        ' get raster properties
    '        pRasterPropFlowAcc = pRasterFlowAcc

    '        pOrg.X = pRasterPropFlowAcc.Extent.XMin
    '        pOrg.Y = pRasterPropFlowAcc.Extent.YMax
    '        pCellSize = (pRasterPropFlowAcc.MeanCellSize.X + pRasterPropFlowAcc.MeanCellSize.Y) / 2
    '        ' create a DblPnt to hold the PixelBlock size
    '        pSize = New DblPnt
    '        pSize.SetCoords(pRasterPropFlowAcc.Width, pRasterPropFlowAcc.Height)

    '        ' get vb supported pixel type
    '        pRasterPropFlowAcc.PixelType = GetVBSupportedPixelType(pRasterPropFlowAcc.PixelType)
    '        pFromPoint = pPolyline.frompoint
    '        pToPoint = pPolyline.topoint

    '        pFlowAccFromPt = 0
    '        pFlowAccToPt = 0

    '        iRow = CInt(((pOrg.Y - pFromPoint.Y) / pCellSize) - 0.5)
    '        iCol = CInt(((pFromPoint.X - pOrg.X) / pCellSize) - 0.5)
    '        For cCol = iCol - 2 To iCol + 2
    '            For cRow = iRow - 2 To iRow + 2
    '                If (cRow < pRasterPropFlowAcc.Height And cRow > 0 And cCol < pRasterPropFlowAcc.Width And cCol > 0) Then
    '                    ''            If (cRow < pPixelBlockFlowAcc.Height And cRow > 0 And cCol < pPixelBlockFlowAcc.Width And cCol > 0) Then
    '                    ''              ' If this cell has no value for flow accumulation, continue to process next cell
    '                    ''              pValueFlowAcc = vPixelDataFlowAcc(cCol, cRow)
    '                    ''              If pPixelBlockFlowAcc.GetNoDataMaskVal(0, cCol, cRow) = 1 And (pValueFlowAcc > pFlowAccFromPt) Then
    '                    ''                  pFlowAccFromPt = pValueFlowAcc
    '                    ''              End If
    '                End If
    '            Next cRow
    '        Next cCol


    '        iRow = ((pOrg.Y - pToPoint.Y) / pCellSize) - 0.5
    '        iCol = ((pToPoint.X - pOrg.X) / pCellSize) - 0.5
    '        If (iRow < pRasterPropFlowAcc.Height And iCol < pRasterPropFlowAcc.Width) Then
    '            For cCol = iCol - 2 To iCol + 2
    '                For cRow = iRow - 2 To iRow + 2
    '                    ' If this cell has no value for flow accumulation, continue to process next cell
    '                    ''                  pValueFlowAcc = vPixelDataFlowAcc(cCol, cRow)
    '                    ''                  If pPixelBlockFlowAcc.GetNoDataMaskVal(0, cCol, cRow) = 1 And pValueFlowAcc > pFlowAccToPt Then
    '                    ''                      pFlowAccToPt = pValueFlowAcc
    '                    ''                  End If
    '                Next cRow
    '            Next cCol
    '        End If

    '        If (pFlowAccFromPt > pFlowAccToPt) Then
    '            StepReversePolyLineOrientation = True
    '        Else
    '            StepReversePolyLineOrientation = False
    '        End If
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function

    'Public Sub FindSegmentationLengthAndTimeRange(ByRef pMinSegmentLen As Double, ByRef pMaxSegmentLen As Double, ByRef pMinSegmentTime As Double, ByRef pMaxSegmentTime As Double, ByRef pMinVelocity As Double)
    '    Try
    '        'Open the Branches layer
    '        Dim pBranchesFLayer As MapWindow.Interfaces.Layer
    '        pBranchesFLayer = GetInputLayer("Branches")
    '        If (pBranchesFLayer Is Nothing) Then
    '            Exit Sub
    '        End If

    '        Dim pBranchesFClass As IFeatureClass
    '        pBranchesFClass = pBranchesFLayer.FeatureClass
    '        Dim pBranchIndex As Integer
    '        pBranchIndex = pBranchesFClass.FindField("Branch")
    '        Dim pBranchTimeIndex As Integer
    '        pBranchTimeIndex = pBranchesFClass.FindField("TravelTime")
    '        Dim pBranchMetersIndex As Integer
    '        pBranchMetersIndex = pBranchesFClass.FindField("METERS")

    '        Dim pQueryFilter As IQueryFilter
    '        pQueryFilter = New QueryFilter
    '        Dim pBranchCursor As IFeatureCursor
    '        Dim pBranchFeature As IFeature
    '        Dim iBranch As Short
    '        iBranch = 1
    '        Dim pLength As Double
    '        Dim pTime As Double
    '        Dim DoContinue As Boolean
    '        DoContinue = True

    '        pMinSegmentLen = 1.0E+15
    '        pMinSegmentTime = 1.0E+15
    '        pMinVelocity = 1.0E+15

    '        Dim pVelocity As Double
    '        Do While DoContinue
    '            pLength = 0
    '            pTime = 0
    '            pVelocity = 0
    '            pQueryFilter.WhereClause = "Branch = " & iBranch
    '            pBranchCursor = pBranchesFClass.Search(pQueryFilter, True)
    '            pBranchFeature = pBranchCursor.NextFeature
    '            If (pBranchFeature Is Nothing) Then
    '                DoContinue = False
    '            End If
    '            Do While Not pBranchFeature Is Nothing
    '                pTime = pBranchFeature.Value(pBranchTimeIndex)
    '                pLength = pLength + pBranchFeature.Value(pBranchMetersIndex)
    '                pVelocity = pBranchFeature.Value(pBranchesFClass.FindField("Velocity"))
    '                pBranchFeature = pBranchCursor.NextFeature
    '            Loop
    '            If (pLength > pMaxSegmentLen) Then
    '                pMaxSegmentLen = pLength
    '            End If
    '            If (pTime > pMaxSegmentTime) Then
    '                pMaxSegmentTime = pTime
    '            End If
    '            If (pLength < pMinSegmentLen And pLength > 0) Then
    '                pMinSegmentLen = pLength
    '            End If
    '            If (pTime < pMinSegmentTime And pTime > 0) Then
    '                pMinSegmentTime = pTime
    '            End If
    '            If (pMinVelocity > pVelocity And pVelocity > 0) Then
    '                pMinVelocity = pVelocity
    '            End If
    '            iBranch = iBranch + 1
    '        Loop
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub


    ''Find if the streams follow the flow direction or against the flow direction
    'Public Function GetStreamDirectionFlag(ByRef streamLayerName As String, ByRef streamIDFldName As String, ByRef streamDsIdFldName As String) As Boolean
    '    Try
    '        Dim pFeatureLayer As MapWindow.Interfaces.Layer
    '        Dim pFeatureClass As IFeatureClass
    '        Dim pFeature As IFeature
    '        Dim pFeatureCursor As IFeatureCursor
    '        Dim subIdF As Integer
    '        Dim dsIdF As Integer

    '        pFeatureLayer = GetInputLayer(streamLayerName)

    '        If (pFeatureLayer Is Nothing) Then
    '            MsgBox("No stream available")
    '            Exit Function
    '        End If

    '        pFeatureClass = pFeatureLayer.FeatureClass

    '        subIdF = pFeatureClass.FindField(streamIDFldName)
    '        dsIdF = pFeatureClass.FindField(streamDsIdFldName)

    '        pFeatureCursor = pFeatureClass.Search(Nothing, False)
    '        pFeature = pFeatureCursor.NextFeature

    '        Dim subId As String
    '        Dim dsID As String
    '        Dim pTempFCursor As IFeatureCursor
    '        Dim ptempFeature As IFeature
    '        Dim pQueryFilter As IQueryFilter

    '        pQueryFilter = New QueryFilter

    '        Dim pPolylineWs As IPolyline
    '        Dim pPolylineDs As IPolyline

    '        Do While Not pFeature Is Nothing
    '            subId = pFeature.Value(subIdF)
    '            dsID = pFeature.Value(dsIdF)

    '            pQueryFilter.WhereClause = streamIDFldName & " = " & dsID & ""

    '            pTempFCursor = pFeatureClass.Search(pQueryFilter, False)
    '            ptempFeature = pTempFCursor.NextFeature

    '            If (Not ptempFeature Is Nothing) Then
    '                pPolylineWs = pFeature.Shape
    '                pPolylineDs = ptempFeature.Shape

    '                If (pPolylineWs.topoint.Compare(pPolylineDs.frompoint) = 0) Then
    '                    GetStreamDirectionFlag = True
    '                ElseIf (pPolylineWs.frompoint.Compare(pPolylineDs.topoint) = 0) Then
    '                    GetStreamDirectionFlag = False
    '                End If
    '                Exit Do
    '            End If
    '            pFeature = pFeatureCursor.NextFeature
    '        Loop
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try


    'End Function


    'Public Sub ConvertPourPointToPointFeature()
    '    Try
    '        DeleteLayerFromMap("SnapAssessPoints")

    '        Dim pPourPointRaster As IRaster
    '        pPourPointRaster = OpenRasterDatasetFromDisk("Pourpoint")
    '        If pPourPointRaster Is Nothing Then
    '            Exit Sub
    '        End If

    '        DeleteFeatureDataset("SnapAssessPoints")

    '        Dim pOp As IConversionOp
    '        Dim outGDS As IGeoDataset
    '        Dim outWork As IWorkspace
    '        Dim outWSF As IWorkspaceFactory
    '        Dim outFC As IFeatureClass
    '        Dim outFLayer As MapWindow.Interfaces.Layer
    '        Dim outLayer As ILayer
    '        Dim pRasterDataSet As IGeoDataset

    '        pRasterDataSet = pPourPointRaster
    '        pOp = New RasterConversionOp
    '        outWSF = New ShapefileWorkspaceFactory
    '        outWork = outWSF.OpenFromFile(gMapTempFolder, 0)
    '        outGDS = pOp.RasterDataToPointFeatureData(pRasterDataSet, outWork, "SnapAssessPoints")
    '        outFC = outGDS
    '        outFLayer = New FeatureLayer
    '        outFLayer.FeatureClass = outFC
    '        outLayer = outFLayer
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub


    ''******************************************************************************
    ''Subroutine: UpdateWASPtableToSAP
    ''Author:     Sabu Paul
    ''Purpose:    Updates WASP.dbf table with new segment numbers to match SnapAssessPoint layer
    ''******************************************************************************
    'Public Sub UpdateWASPtableToSAP()
    '    Try
    '        FileCopy(gMapInputFolder & "\wasp2.dbf", gMapInputFolder & "\wasp.dbf")

    '        Dim pFact As IWorkspaceFactory
    '        pFact = New ShapefileWorkspaceFactory

    '        Dim pWorkspace As IWorkspace
    '        pWorkspace = pFact.OpenFromFile(gMapInputFolder, 0)

    '        Dim pFeatWS As IFeatureWorkspace
    '        pFeatWS = pWorkspace

    '        Dim pWASPTable As iTable
    '        pWASPTable = pFeatWS.OpenTable("Wasp.dbf")

    '        Dim wspFldSegment As Integer
    '        wspFldSegment = pWASPTable.FindField("Segment")
    '        Dim wspFldFile As Integer
    '        wspFldFile = pWASPTable.FindField("OutputFile")

    '        Dim pFieldEdit As IFieldEdit
    '        'Add Segment2 field
    '        If pWASPTable.FindField("Segment2") < 0 Then
    '            pFieldEdit = New Field
    '            With pFieldEdit
    '                .name = "Segment2"
    '                .IsNullable = True
    '                '            .Type = esriFieldTypeInteger
    '            End With
    '            pWASPTable.AddField(pFieldEdit)
    '        End If

    '        'Add Segment3 field
    '        If pWASPTable.FindField("Segment3") < 0 Then
    '            pFieldEdit = New Field
    '            With pFieldEdit
    '                .name = "Segment3"
    '                .IsNullable = True
    '                '            .Type = esriFieldTypeInteger
    '            End With
    '            pWASPTable.AddField(pFieldEdit)
    '        End If

    '        Dim wspFldSegment2 As Integer
    '        wspFldSegment2 = pWASPTable.FindField("Segment2")

    '        Dim wspFldSegment3 As Integer
    '        wspFldSegment3 = pWASPTable.FindField("Segment3")

    '        Dim pSapFeatureLayer As MapWindow.Interfaces.Layer
    '        pSapFeatureLayer = GetInputLayer("SnapAssessPoints")

    '        If pSapFeatureLayer Is Nothing Then Err.Raise(vbObjectError + 5003, , "SnapAssessPoints is missing")

    '        Dim pSapFeatureClass As IFeatureClass
    '        pSapFeatureClass = pSapFeatureLayer.FeatureClass

    '        Dim pTabQueryFilter As IQueryFilter
    '        pTabQueryFilter = New QueryFilter

    '        Dim pSAPQueryFilter As IQueryFilter
    '        pSAPQueryFilter = New QueryFilter

    '        Dim segmentCount As Integer
    '        segmentCount = pWASPTable.RowCount(Nothing)

    '        Dim pRow As iRow

    '        Dim segIndex As Integer

    '        Dim pCursor As ICursor

    '        Dim segIndexNew As Integer
    '        segIndexNew = 1

    '        Dim pSelectionSet As ISelectionSet

    '        'Set the new segment index to segment2 field in wasp table
    '        For segIndex = 1 To segmentCount
    '            pTabQueryFilter.WhereClause = "Segment = " & segIndex
    '            pSAPQueryFilter.WhereClause = "ID = " & segIndex

    '            pCursor = pWASPTable.Search(pTabQueryFilter, False)
    '            ''        Set pSelectionSet = pSapFeatureClass.Select(pSAPQueryFilter, esriSelectionTypeIDSet, esriSelectionOptionNormal, Nothing)
    '            If pSelectionSet.Count > 0 Then
    '                pRow = pCursor.NextRow
    '                If Not pRow Is Nothing Then
    '                    pRow.Value(wspFldSegment2) = segIndexNew
    '                    pRow.Value(wspFldSegment3) = segIndex
    '                    'pRow.Value(wspFldFile) = "SubwatershedOutput_" & segIndex & ".out"
    '                    pRow.Store()
    '                    segIndexNew = segIndexNew + 1
    '                End If
    '            End If
    '        Next

    '        'Delete all rows with no segment2 values in WASP table
    '        pTabQueryFilter.WhereClause = "Segment2 < 1 "
    '        pWASPTable.DeleteSearchedRows(pTabQueryFilter)


    '        'Copy all segment2 values to segment field
    '        pCursor = pWASPTable.Search(Nothing, False)
    '        pRow = pCursor.NextRow
    '        Do While Not pRow Is Nothing
    '            pRow.Value(wspFldSegment) = pRow.Value(wspFldSegment2)
    '            pRow.Value(wspFldSegment2) = pRow.Value(wspFldSegment3)
    '            pRow.Store()
    '            pRow = pCursor.NextRow
    '        Loop


    '        Dim pFields As IFields
    '        Dim pField As IField
    '        pFields = pWASPTable.Fields
    '        pField = pFields.Field(pFields.FindField("Segment3"))
    '        pWASPTable.DeleteField(pField)

    '        'Update the mile point and distances
    '        ModuleGIStoWasp.UpdateWaspDistances()

    '        pFields = pWASPTable.Fields
    '        pField = pFields.Field(pFields.FindField("Segment2"))
    '        pWASPTable.DeleteField(pField)
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub

    'Public Sub UpdateWaspDistances()
    '    Try
    '        Dim FindRiverDistances As Object
    '        Dim startPoint As Object
    '        Dim mostDsPoint As Object

    '        Dim pBranchFLayer As MapWindow.Interfaces.Layer
    '        pBranchFLayer = GetInputLayer("Branches")

    '        If pBranchFLayer Is Nothing Then Err.Raise(vbObjectError + 5002, , "Branches feature layer is missing")

    '        Dim pBranchFClass As IFeatureClass
    '        pBranchFClass = pBranchFLayer.FeatureClass

    '        Dim pBranchFCursor As IFeatureCursor
    '        pBranchFCursor = pBranchFClass.Search(Nothing, False)

    '        Dim branchRchIdFld As Short
    '        branchRchIdFld = pBranchFClass.FindField("RCH_COM_ID")

    '        Dim pRFlowTable As iTable
    '        If Not InputDataDictionary.Exists("NHDRFlowTable") Then Err.Raise(vbObjectError, , "Rflow table is missing")

    '        pRFlowTable = GetInputTable(InputDataDictionary.Item("NHDRFlowTable"))
    '        Dim strIDName, strDSIDName As String
    '        strIDName = "COM_ID_1"
    '        strDSIDName = "COM_ID_2"

    '        Dim strIDFld, strDSIDFld As Short
    '        strIDFld = pRFlowTable.FindField(strIDName)
    '        strDSIDFld = pRFlowTable.FindField(strDSIDName)

    '        If strIDFld < 0 Or strDSIDFld < 0 Then Err.Raise(vbObjectError, , "Required fields ('COM_ID_1' and 'COM_ID_2') missing in Rflow table")

    '        Dim pQueryStr As String
    '        Dim isFirst As Boolean
    '        isFirst = True

    '        Dim pFeature As IFeature
    '        pFeature = pBranchFCursor.NextFeature

    '        Do While Not pFeature Is Nothing
    '            If isFirst Then
    '                pQueryStr = strIDName & " = " & pFeature.Value(branchRchIdFld)
    '                isFirst = False
    '            Else
    '                pQueryStr = pQueryStr & " OR " & strIDName & " = " & pFeature.Value(branchRchIdFld)
    '            End If
    '            pFeature = pBranchFCursor.NextFeature
    '        Loop

    '        If pQueryStr = "" Then Exit Sub

    '        Dim pQueryFilter As IQueryFilter
    '        pQueryFilter = New QueryFilter

    '        pQueryFilter.WhereClause = pQueryStr

    '        Dim pCursor As ICursor
    '        pCursor = pRFlowTable.Search(pQueryFilter, False)

    '        Dim pDSIDDictionary As Scripting.Dictionary
    '        pDSIDDictionary = New Scripting.Dictionary

    '        Dim pRow As iRow
    '        pRow = pCursor.NextRow
    '        Do While Not pRow Is Nothing
    '            If Not pDSIDDictionary.Exists(pRow.Value(strIDFld)) Then
    '                pDSIDDictionary.let_Item(pRow.Value(strIDFld), pRow.Value(strDSIDFld))
    '            End If
    '            pRow = pCursor.NextRow
    '        Loop

    '        Dim pAssessFLayer As MapWindow.Interfaces.Layer
    '        pAssessFLayer = GetInputLayer("AssessPoints")

    '        If pAssessFLayer Is Nothing Then Err.Raise(vbObjectError + 5002, , "AssessPoints feature layer is missing")

    '        Dim pAssessFClass As IFeatureClass
    '        pAssessFClass = pAssessFLayer.FeatureClass

    '        Dim pSapFLayer As MapWindow.Interfaces.Layer
    '        pSapFLayer = GetInputLayer("SnapAssessPoints")

    '        If pSapFLayer Is Nothing Then Err.Raise(vbObjectError + 5002, , "SnapAssessPoints feature layer is missing")

    '        Dim pSapFClass As IFeatureClass
    '        pSapFClass = pSapFLayer.FeatureClass

    '        Dim pSAPQueryFilter As IQueryFilter
    '        pSAPQueryFilter = New QueryFilter

    '        pQueryFilter.WhereClause = "DSID = 0"

    '        If pSapFClass.FeatureCount(pQueryFilter) > 1 Then Err.Raise(vbObjectError + 5002, , "There are multiple watershed outlets in the study area")

    '        Dim pSapFCursor As IFeatureCursor
    '        pSapFCursor = pSapFClass.Search(pQueryFilter, False)

    '        Dim pSapFeature As IFeature
    '        pSapFeature = pSapFCursor.NextFeature
    '        If pSapFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "Outlet is missing in SnapAssessPoints layer")

    '        Dim pAssessQueryFilter As IQueryFilter
    '        pAssessQueryFilter = New QueryFilter

    '        pAssessQueryFilter.WhereClause = "ID = " & pSapFeature.Value(pSapFClass.FindField("ID"))

    '        If pAssessFClass.FeatureCount(pAssessQueryFilter) = 0 Then Err.Raise(vbObjectError + 5002, , "Matching point corresponding to the outlet is missing in AssessPoints layer")

    '        Dim pAssessFCursor As IFeatureCursor
    '        pAssessFCursor = pAssessFClass.Search(pAssessQueryFilter, False)

    '        Dim pAssessFeature As IFeature
    '        pAssessFeature = pAssessFCursor.NextFeature


    '        'Get the most downstream stream id and location
    '        '    Dim mostDsPoint As esriGeometry.IPoint
    '        mostDsPoint = pAssessFeature.Value(pAssessFClass.FindField("Shape"))

    '        Dim pSpatialFilter As ISpatialFilter
    '        pSpatialFilter = New SpatialFilter

    '        With pSpatialFilter
    '            .Geometry = mostDsPoint
    '            '        .SpatialRel = esriSpatialRelIntersects
    '        End With

    '        If pBranchFClass.FeatureCount(pSpatialFilter) > 1 Then Err.Raise(vbObjectError + 5002, , "Multiple braches with 2 cell size of outlet")

    '        pBranchFCursor = pBranchFClass.Search(pSpatialFilter, False)
    '        pFeature = pBranchFCursor.NextFeature

    '        Dim mostDsComId As Integer
    '        If pFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "Matching branch corresponding to the outlet is missing in Branches layer")
    '        mostDsComId = pFeature.Value(branchRchIdFld)

    '        'The river outlet is the end point of the branch
    '        Dim pPolyline As IPolyline
    '        pPolyline = pFeature.Value(pBranchFClass.FindField("Shape"))

    '        If (StepReversePolyLineOrientation(pPolyline)) Then
    '            pPolyline.QueryFromPoint(mostDsPoint)
    '        Else
    '            pPolyline.QueryToPoint(mostDsPoint)
    '        End If

    '        'Get each points from SAP feature layer
    '        pSapFCursor = pSapFClass.Search(Nothing, False)
    '        pSapFeature = pSapFCursor.NextFeature

    '        '    Dim startPoint As esriGeometry.IPoint
    '        Dim startComId As Integer

    '        Dim distanceBetween As Double

    '        Dim WaspRiverMDict As Scripting.Dictionary
    '        WaspRiverMDict = New Scripting.Dictionary

    '        Dim downStreamDict As Scripting.Dictionary
    '        downStreamDict = New Scripting.Dictionary

    '        Dim milePoint As Double
    '        Dim sapId As Integer
    '        Dim dowsSapId As Integer


    '        Do While Not pSapFeature Is Nothing
    '            'Get the corresponding feature from Assesspoints
    '            sapId = pSapFeature.Value(pSapFClass.FindField("ID"))
    '            dowsSapId = pSapFeature.Value(pSapFClass.FindField("DSID"))
    '            downStreamDict.let_Item(sapId, dowsSapId)
    '            pAssessQueryFilter.WhereClause = "ID = " & sapId
    '            If pAssessFClass.FeatureCount(pAssessQueryFilter) = 0 Then Err.Raise(vbObjectError + 5002, , "Matching point corresponding to the outlet is missing in AssessPoints layer")

    '            pAssessFCursor = pAssessFClass.Search(pAssessQueryFilter, False)
    '            pAssessFeature = pAssessFCursor.NextFeature
    '            startPoint = pAssessFeature.Value(pAssessFClass.FindField("Shape"))

    '            With pSpatialFilter
    '                .Geometry = startPoint
    '                '            .SpatialRel = esriSpatialRelIntersects
    '            End With

    '            'If pBranchFClass.FeatureCount(pSpatialFilter) > 1 Then Err.Raise vbObjectError + 5002, , "Multiple braches with 2 cell size of outlet"
    '            pBranchFCursor = pBranchFClass.Search(pSpatialFilter, False)
    '            pFeature = pBranchFCursor.NextFeature

    '            If pFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "Matching branch corresponding to the outlet is missing in Branches layer")
    '            startComId = pFeature.Value(branchRchIdFld)

    '            distanceBetween = FindRiverDistances(startComId, startPoint, mostDsComId, mostDsPoint, pDSIDDictionary)
    '            milePoint = distanceBetween / 1000 * 0.6213712
    '            WaspRiverMDict.let_Item(sapId, milePoint)
    '            pSapFeature = pSapFCursor.NextFeature
    '        Loop

    '        Dim pFact As IWorkspaceFactory
    '        pFact = New ShapefileWorkspaceFactory

    '        Dim pWorkspace As IWorkspace
    '        pWorkspace = pFact.OpenFromFile(gMapInputFolder, 0)

    '        Dim pFeatWS As IFeatureWorkspace
    '        pFeatWS = pWorkspace

    '        Dim pWASPTable As iTable
    '        pWASPTable = pFeatWS.OpenTable("Wasp.dbf")

    '        Dim wspFldBranch As Integer
    '        wspFldBranch = pWASPTable.FindField("Branch")
    '        Dim wspFldrivermile As Integer
    '        wspFldrivermile = pWASPTable.FindField("River_mile")
    '        Dim wspFldLength As Integer
    '        wspFldLength = pWASPTable.FindField("Length_m")

    '        Dim i As Short
    '        Dim prevBranch As Integer
    '        prevBranch = 0

    '        Dim downStreamIndex As Short
    '        downStreamIndex = 1
    '        'Dictionary keys are 1,2....
    '        For i = 2 To WaspRiverMDict.Count
    '            Debug.Print(i)
    '            pQueryFilter.WhereClause = "Segment2 = " & i
    '            pCursor = pWASPTable.Search(pQueryFilter, False)
    '            pRow = pCursor.NextRow
    '            If Not pRow Is Nothing Then
    '                pRow.Value(wspFldrivermile) = WaspRiverMDict.Item(i)
    '                prevBranch = pRow.Value(wspFldBranch)
    '                pQueryFilter.WhereClause = "Segment2 = " & downStreamDict.Item(i)
    '                pCursor = pWASPTable.Search(pQueryFilter, False)
    '                pRow = pCursor.NextRow
    '                If Not pRow Is Nothing Then
    '                    If (prevBranch = pRow.Value(wspFldBranch)) Then
    '                        pRow.Value(wspFldLength) = (WaspRiverMDict.Item(i) - WaspRiverMDict.Item(downStreamDict.Item(i))) * 1000 / 0.6213712
    '                    End If
    '                    pRow.Store()
    '                End If
    '            End If
    '        Next
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try
    'End Sub

    'Public Function FindRiverDistances(ByVal pStartStreamId As Long, ByVal startPoint As esriGeometry.IPoint, ByVal pEndStreamId As Long, ByVal endPoint As esriGeometry.IPoint, ByVal DsIdDictionary As Dictionary) As Double
    '    Try
    '        Dim pStreamFLayer As MapWindow.Interfaces.Layer
    '        pStreamFLayer = GetInputLayer("NHD")

    '        If pStreamFLayer Is Nothing Then Err.Raise(vbObjectError + 5002, , "NHD feature layer is missing")

    '        Dim pFCStream As IFeatureClass
    '        pFCStream = pStreamFLayer.FeatureClass

    '        Dim totalDis As Double
    '        totalDis = 0

    '        Dim pQueryFilter As IQueryFilter
    '        pQueryFilter = New QueryFilter

    '        pQueryFilter.WhereClause = "COM_ID = " & pStartStreamId

    '        Dim pStreamFCursor As IFeatureCursor
    '        pStreamFCursor = pFCStream.Search(pQueryFilter, False)

    '        Dim isAlongFlowDir As Boolean
    '        Dim pStreamFeature As IFeature
    '        pStreamFeature = pStreamFCursor.NextFeature

    '        If pStreamFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "No stream in NHD with ID " & pStartStreamId)

    '        Dim pPolyline As IPolyline
    '        pPolyline = pStreamFeature.Value(pFCStream.FindField("Shape"))

    '        Dim lineLength As Double
    '        lineLength = pPolyline.Length

    '        Dim posFrom As Double
    '        Dim posTo As Double
    '        Dim pPt1 As esriGeometry.IPoint
    '        Dim curDis As Double
    '        Dim bRight As Boolean

    '        Dim curStreamId As Long
    '        'pPolyline.GetSubcurve posFrom, posTo, True, pSubPolyline
    '        pPolyline.QueryPointAndDistance(esriNoExtension, startPoint, False, pPt1, posFrom, curDis, bRight)
    '        If pStartStreamId = pEndStreamId Then
    '            pPolyline.QueryPointAndDistance(esriNoExtension, endPoint, False, pPt1, posTo, curDis, bRight)
    '            totalDis = Abs(posTo - posFrom)
    '        Else
    '            If (StepReversePolyLineOrientation(pPolyline)) Then 'Against flow direction
    '                totalDis = posFrom
    '            Else
    '                totalDis = lineLength - posFrom
    '            End If
    '            If Not DsIdDictionary.Exists(pStartStreamId) Then Err.Raise(vbObjectError + 5002, , "No downstream information for " & pStartStreamId)
    '            curStreamId = DsIdDictionary.Item(pStartStreamId)

    '            Do While curStreamId <> pEndStreamId
    '                pQueryFilter.WhereClause = "COM_ID = " & curStreamId
    '                pStreamFCursor = pFCStream.Search(pQueryFilter, False)
    '                pStreamFeature = pStreamFCursor.NextFeature
    '                If pStreamFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "No stream in NHD with ID " & pStartStreamId)
    '                pPolyline = pStreamFeature.Value(pFCStream.FindField("Shape"))
    '                lineLength = pPolyline.Length
    '                totalDis = totalDis + lineLength
    '                If Not DsIdDictionary.Exists(curStreamId) Then Err.Raise(vbObjectError + 5002, , "No downstream information for " & pStartStreamId)
    '                curStreamId = DsIdDictionary.Item(curStreamId)
    '            Loop

    '            'Last stream and end point
    '            pQueryFilter.WhereClause = "COM_ID = " & curStreamId
    '            pStreamFCursor = pFCStream.Search(pQueryFilter, False)
    '            pStreamFeature = pStreamFCursor.NextFeature
    '            If pStreamFeature Is Nothing Then Err.Raise(vbObjectError + 5002, , "No stream in NHD with ID " & pStartStreamId)
    '            pPolyline = pStreamFeature.Value(pFCStream.FindField("Shape"))
    '            lineLength = pPolyline.Length
    '            totalDis = totalDis + lineLength
    '        End If
    '        FindRiverDistances = totalDis
    '    Catch ex As Exception
    '        ErrorMsg(, ex)
    '    End Try

    'End Function
End Module