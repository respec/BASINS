Imports System.Drawing
Imports MapWinUtility
Imports atcMwGisUtility
Imports atcControls
Imports atcUtility

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class PollutantLoadingBMPs
    Public ReadOnly LayerName As String
    Public ReadOnly AreaField As String
    Public ReadOnly TypeField As String
    Public ReadOnly GridSource As atcGridSource

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aLayerName"></param>
    ''' <param name="aAreaField"></param>
    ''' <param name="aTypefield"></param>
    ''' <param name="aGridSource"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aLayerName As String, _
                   ByVal aAreaField As String, _
                   ByVal aTypefield As String, _
                   ByVal aGridSource As atcGridSource)
        LayerName = aLayerName
        AreaField = aAreaField
        TypeField = aTypefield
        GridSource = aGridSource
    End Sub
End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class PollutantLoadingPointLoads
    Public ReadOnly PointLayerName As String
    Public ReadOnly PointIDField As String
    Public ReadOnly PointGridSource As atcGridSource

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aPointLayerName"></param>
    ''' <param name="aPointIDField"></param>
    ''' <param name="aPointGridSource"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aPointLayerName As String, _
                   ByVal aPointIDField As String, _
                   ByVal aPointGridSource As atcGridSource)
        PointLayerName = aPointLayerName
        PointIDField = aPointIDField
        PointGridSource = aPointGridSource
    End Sub
End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class PollutantLoadingStreamBankLoads
    Public ReadOnly StreamIDField As String
    Public ReadOnly BankGridSource As atcGridSource

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aStreamIDField"></param>
    ''' <param name="aBankGridSource"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal aStreamIDField As String, _
                   ByVal aBankGridSource As atcGridSource)
        StreamIDField = aStreamIDField
        BankGridSource = aBankGridSource
    End Sub
End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Module PollutantLoading
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aSubbasinLayerName"></param>
    ''' <param name="aGridSource"></param>
    ''' <param name="aUseExportCoefficent"></param>
    ''' <param name="aLandUse"></param>
    ''' <param name="aLandUseLayer"></param>
    ''' <param name="aLandUseId"></param>
    ''' <param name="aPrec"></param>
    ''' <param name="aRatio"></param>
    ''' <param name="aConstituents"></param>
    ''' <param name="aBmps"></param>
    ''' <param name="aPointLoads"></param>
    ''' <remarks></remarks>
    Public Sub GenerateLoads(ByVal aSubbasinLayerName As String, _
                             ByVal aGridSource As atcGridSource, _
                             ByVal aUseExportCoefficent As Boolean, _
                             ByVal aLandUse As String, _
                             ByVal aLandUseLayer As String, _
                             ByVal aLandUseId As String, _
                             ByVal aPrec() As Double, _
                             ByVal aRatio As Double, _
                             ByVal aConstituents As atcCollection, _
                             ByVal aBmps As PollutantLoadingBMPs, _
                             ByVal aPointLoads As PollutantLoadingPointLoads, _
                             ByVal aStreamBankLoads As PollutantLoadingStreamBankLoads)

        Dim i As Integer, j As Integer, k As Integer
        Dim lSubbasinLayerIndex As Integer
        Dim lLanduseLayerIndex As Integer
        Dim lBMPLayerIndex As Integer
        Dim lBMPLayerType As Integer
        Dim lBMPArea As Single
        Dim lBMPAreaFieldIndex As Integer
        Dim lBMPTypeFieldIndex As Integer
        Dim lBMPType As String
        Dim lEffic As Single
        Dim lPointLayerIndex As Integer
        Dim lPointIDFieldIndex As Integer
        Dim lFacility As String
        Dim lLoad As Single
        Dim lLucode As Integer
        Dim lProblem As String

        Logger.Dbg("Start")
        lSubbasinLayerIndex = GisUtil.LayerIndex(aSubbasinLayerName)

        'are any areas selected?
        Dim lSelectedAreaIndexes As New Collection
        For i = 1 To GisUtil.NumSelectedFeatures(lSubbasinLayerIndex)
            'add selected areas to the collection
            lSelectedAreaIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lSubbasinLayerIndex))
        Next
        If lSelectedAreaIndexes.Count = 0 Then
            'no areas selected, act as if all are selected
            For i = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                lSelectedAreaIndexes.Add(i - 1)
            Next
        End If
        Logger.Dbg("AreaCount " & lSelectedAreaIndexes.Count)

        'build array for output area values (area of each landuse in each subbasin)
        Dim lMaxlu As Integer = aGridSource.CellValue(aGridSource.Rows - 1, 1)
        Dim lAreasLS(lMaxlu, lSelectedAreaIndexes.Count) As Double

        'build array of constituent names
        Dim lOffset As Integer
        If aUseExportCoefficent Then
            lOffset = 3
        Else
            'emc method (has extra column for impervious)
            lOffset = 4
        End If
        Dim lCountCons As Integer = aGridSource.Columns - lOffset - 1
        Dim lConsNames(lCountCons) As String
        For i = 0 To lCountCons
            lConsNames(i) = aGridSource.CellValue(0, i + lOffset)
        Next i
        Logger.Dbg("ConstituentCount " & lCountCons + 1)

        'build array for each export coeff or emc for each land use type
        Dim lCoeffsLC(lMaxlu, lCountCons) As Double
        For j = 1 To aGridSource.Rows - 1
            lLucode = aGridSource.CellValue(j, 1)
            For i = 0 To lCountCons
                lCoeffsLC(lLucode, i) = aGridSource.CellValue(j, i + lOffset)
            Next i
        Next j

        'build array for precip to each subbasin 
        Dim lPrecS(lSelectedAreaIndexes.Count) As Single
        For i = 1 To lSelectedAreaIndexes.Count  'for each subbasin
            lPrecS(i - 1) = aPrec(lSelectedAreaIndexes(i))
        Next i

        'build array for output load values (load for each subbasin and constituent)
        Dim lLoadsSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double
        'build array for area of each subbasin 
        Dim lAreasS(lSelectedAreaIndexes.Count) As Double
        'build array for per acre load values (load for each subbasin and constituent)
        Dim lLoadsPerAcreSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double
        'build array for output event mean concentrations (emc for each subbasin and constituent)
        'only for use in emc (simple) method
        Dim lEMCsSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double
        'build array for bmp removal (amount removed for each subbasin and constituent)
        Dim lBMPRemovalSC(lSelectedAreaIndexes.Count, lConsNames.GetUpperBound(0)) As Double
        Logger.Dbg("OutputArraysBuilt")


        'calculate areas of each land use in each subbasin
        If aLandUse = "USGS GIRAS Shapefile" Then
            CalculateGIRASAreas(lSubbasinLayerIndex, lSelectedAreaIndexes, _
                                lAreasLS)
            'areas will be returned in square meters
        ElseIf aLandUse = "Other Shapefile" Then
            lLanduseLayerIndex = GisUtil.LayerIndex(aLandUseLayer)
            GisUtil.TabulatePolygonAreas(lLanduseLayerIndex, _
                                         GisUtil.FieldIndex(lLanduseLayerIndex, aLandUseId), _
                                         lSubbasinLayerIndex, lSelectedAreaIndexes, _
                                         lAreasLS)
        Else 'grid
            lLanduseLayerIndex = GisUtil.LayerIndex(aLandUseLayer)
            Dim lGridmax As Integer = System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
            Dim laAreaLS(lGridmax, GisUtil.NumFeatures(lSubbasinLayerIndex)) As Double
            GisUtil.TabulateAreas(lLanduseLayerIndex, lSubbasinLayerIndex, _
                                  laAreaLS)
            'transfer values from selected polygons to lAreasLS
            For i = 1 To lSelectedAreaIndexes.Count
                For k = 1 To lMaxlu
                    lAreasLS(k, i - 1) = laAreaLS(k, lSelectedAreaIndexes(i))
                Next k
            Next i
        End If
        Logger.Dbg("AreaOfLandUsesInSubBasinsCalculated")

        'calculate areas of each subbasin
        For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
            For k = 1 To lMaxlu
                lAreasS(i) = lAreasS(i) + (lAreasLS(k, i) / 4046.8564)
                ' / 4046.8564 to convert from m2 to acres
            Next k
        Next i
        Logger.Dbg("SubBasinAreasCalculated")

        Dim lRatio As Single = aRatio
        Dim lRunoffL(lMaxlu) As Single
        If aUseExportCoefficent Then 'Export Coefficients Method
            'calculate loads
            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    For k = 1 To lMaxlu
                        lLoadsSC(i, j) = lLoadsSC(i, j) + (lCoeffsLC(k, j) * lAreasLS(k, i) / 4046.8564)
                        ' / 4046.8564 to convert from m2 to acres
                    Next k
                Next j
            Next i
        Else
            'calculate loads by emc (simple) method
            For j = 1 To aGridSource.Rows - 1
                'build array for each runoff coeff for each land use type
                lLucode = aGridSource.CellValue(j, 1)
                lRunoffL(lLucode) = 0.05 + (0.009 * aGridSource.CellValue(j, 3))
                'will result in values from .05 to .95 
            Next j
            'calc loads
            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    For k = 1 To lMaxlu
                        lLoadsSC(i, j) = lLoadsSC(i, j) + (lPrecS(i) * lRatio * lRunoffL(k) * lCoeffsLC(k, j) * lAreasLS(k, i) / 4046.8564 * 2.72 / 12)
                        ' / 4046.8564 to convert from m2 to acres
                    Next k
                Next j
            Next i
            
        End If
        Logger.Dbg("LoadsCalculated")

        Dim lSubID As String
        If Not (aBmps Is Nothing) Then
            'reduce loads due to bmps
            If aBmps.LayerName <> "<none>" Then
                'have a bmp map layer
                lBMPLayerIndex = GisUtil.LayerIndex(aBmps.LayerName)
                lBMPLayerType = GisUtil.LayerType(lBMPLayerIndex)
                lBMPAreaFieldIndex = GisUtil.FieldIndex(lBMPLayerIndex, aBmps.AreaField)
                lBMPTypeFieldIndex = GisUtil.FieldIndex(lBMPLayerIndex, aBmps.TypeField)
                'for each subbasin
                For i = 0 To lSelectedAreaIndexes.Count - 1
                    'for each bmp feature
                    For k = 1 To GisUtil.NumFeatures(lBMPLayerIndex)
                        'is there an intersect?
                        lBMPArea = 0.0
                        If lBMPLayerType = 1 Then
                            'point layer
                            If GisUtil.PointInPolygon(lBMPLayerIndex, k - 1, lSubbasinLayerIndex) = lSelectedAreaIndexes(i + 1) Then
                                lBMPArea = GisUtil.FieldValue(lBMPLayerIndex, k - 1, lBMPAreaFieldIndex)
                            End If
                        Else
                            'polygon layer
                            lBMPArea = GisUtil.AreaOverlappingPolygons(lBMPLayerIndex, k - 1, lSubbasinLayerIndex, lSelectedAreaIndexes(i + 1)) / 4046.8564
                        End If
                        If lBMPArea > 0.0 Then
                            'have some bmp area
                            lBMPType = GisUtil.FieldValue(lBMPLayerIndex, k - 1, lBMPTypeFieldIndex)
                            'calculate area of each landuse within this bmp zone and subbasin
                            Dim lBMPAreaL(lMaxlu) As Double
                            TabulateBMPAreas(lBMPLayerIndex, k - 1, lSubbasinLayerIndex, lSelectedAreaIndexes(i + 1), _
                                             aLandUse, aLandUseLayer, aLandUseId, lMaxlu, _
                                             lBMPAreaL)
                            For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                                'find the efficiency of this bmp type for this constituent
                                lEffic = GetEfficiency(aBmps.GridSource, lBMPType, lConsNames(j))

                                'calculate removals for each landuse;
                                'the load reduction is the load * fractional area * fractional removal efficiency,
                                'ie 1000 lbs load with 20% bmp area with a 30% removal = 60 lbs removed 
                                For lLuIndex As Integer = 1 To lMaxlu
                                    'calculate the full load first, for this area, as if no bmp
                                    If aUseExportCoefficent Then 'Export Coefficients Method
                                        lLoad = (lCoeffsLC(lLuIndex, j) * lBMPAreaL(lLuIndex) / 4046.8564)
                                    Else
                                        lLoad = (lPrecS(i) * lRatio * lRunoffL(lLuIndex) * lCoeffsLC(lLuIndex, j) * lBMPAreaL(lLuIndex) / 4046.8564 * 2.72 / 12)
                                    End If
                                    'the removal due to the bmp is the load times the efficiency
                                    If lBMPAreaL(lLuIndex) > 0 Then
                                        lBMPRemovalSC(i, j) = lBMPRemovalSC(i, j) + (lLoad * (lEffic) / 100)
                                    End If
                                Next lLuIndex
                            Next j
                        End If
                    Next k
                    'now subtract the bmp reductions from the total load
                    For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                        lLoadsSC(i, j) = lLoadsSC(i, j) - lBMPRemovalSC(i, j)
                    Next j
                Next i
            Else
                'add bmp reductions by table
                Dim lnthConstit As Integer = 0
                lBMPTypeFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, aBmps.TypeField)
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    If aConstituents.IndexOf(lConsNames(j)) > -1 Then
                        'this is a selected constituent
                        lnthConstit = lnthConstit + 1
                        With aBmps.GridSource
                            For k = 1 To .Rows - 1  'for each row of the grid source
                                lSubID = .CellValue(k, 1)
                                For i = 0 To lSelectedAreaIndexes.Count - 1   'find the corresponding subbasin
                                    If lSubID = GisUtil.FieldValue(lSubbasinLayerIndex, lSelectedAreaIndexes(i + 1), lBMPTypeFieldIndex) Then
                                        'this is the one
                                        lBMPRemovalSC(i, j) = lBMPRemovalSC(i, j) + (lLoadsSC(i, j) * .CellValue(k, lnthConstit + 1) / 100)
                                        lLoadsSC(i, j) = lLoadsSC(i, j) - lBMPRemovalSC(i, j)
                                        Exit For
                                    End If
                                Next i
                            Next k
                        End With
                    End If
                Next j
            End If
            Logger.Dbg("BmpsApplied")
        Else
            Logger.Dbg("NoBmpsApplied")
        End If

        If Not (aPointLoads Is Nothing) Then
            'add point loads 
            If aPointLoads.PointLayerName <> "<none>" Then
                'have a point loads layer
                lPointLayerIndex = GisUtil.LayerIndex(aPointLoads.PointLayerName)
                lPointIDFieldIndex = GisUtil.FieldIndex(lPointLayerIndex, aPointLoads.PointIDField)
                'for each point source feature
                For k = 1 To GisUtil.NumFeatures(lPointLayerIndex)
                    'is there an intersect?
                    j = GisUtil.PointInPolygon(lPointLayerIndex, k - 1, lSubbasinLayerIndex)
                    For i = 0 To lSelectedAreaIndexes.Count - 1
                        If j = lSelectedAreaIndexes(i + 1) Then
                            'found it in a selected subbasin
                            lFacility = GisUtil.FieldValue(lPointLayerIndex, k - 1, lPointIDFieldIndex)
                            For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                                'find the load for this facility for this constituent
                                lLoad = GetPointLoad(aPointLoads.PointGridSource, lFacility, lConsNames(j))
                                'add the point load to the total load
                                lLoadsSC(i, j) = lLoadsSC(i, j) + lLoad
                            Next j
                            Exit For
                        End If
                    Next i
                Next k
            Else
                'add point loads by table
                Dim lnthConstit As Integer = 0
                lPointIDFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, aPointLoads.PointIDField)
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    If aConstituents.IndexOf(lConsNames(j)) > -1 Then
                        'this is a selected constituent
                        lnthConstit = lnthConstit + 1
                        With aPointLoads.PointGridSource
                            For k = 1 To .Rows - 1  'for each row of the grid source
                                lSubID = .CellValue(k, 1)
                                For i = 0 To lSelectedAreaIndexes.Count - 1   'find the corresponding subbasin
                                    If lSubID = GisUtil.FieldValue(lSubbasinLayerIndex, lSelectedAreaIndexes(i + 1), lPointIDFieldIndex) Then
                                        'this is the one
                                        lLoadsSC(i, j) = lLoadsSC(i, j) + .CellValue(k, lnthConstit + 1)
                                        Exit For
                                    End If
                                Next i
                            Next k
                        End With
                    End If
                Next j
            End If
            Logger.Dbg("PointSourcesApplied")
        Else
            Logger.Dbg("NoPointSourcesApplied")
        End If

        Dim lStreamIDFieldIndex As Integer
        If Not (aStreamBankLoads Is Nothing) Then
            'add stream bank loads for TSS
            lStreamIDFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, aStreamBankLoads.StreamIDField)
            For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                If lConsNames(j) = "TSS" Then
                    With aStreamBankLoads.BankGridSource
                        For k = 1 To .Rows - 1  'for each row of the grid source
                            lSubID = .CellValue(k, 1)
                            For i = 0 To lSelectedAreaIndexes.Count - 1   'find the corresponding subbasin
                                If lSubID = GisUtil.FieldValue(lSubbasinLayerIndex, lSelectedAreaIndexes(i + 1), lStreamIDFieldIndex) Then
                                    'this is the one
                                    lLoadsSC(i, j) = lLoadsSC(i, j) + .CellValue(k, 2)
                                    Exit For
                                End If
                            Next i
                        Next k
                    End With
                End If
            Next j
            Logger.Dbg("StreamBankLoadsApplied")
        Else
            Logger.Dbg("NoStreamBankLoadsApplied")
        End If

        Dim lVolume(lSelectedAreaIndexes.Count) As Double
        If Not aUseExportCoefficent Then
            'calc emcs (runoff volume weighted)
            Dim lDenomS(lSelectedAreaIndexes.Count) As Double
            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                For k = 1 To lMaxlu
                    lDenomS(i) = lDenomS(i) + (lPrecS(i) * lRatio * lRunoffL(k) * lAreasLS(k, i) * 2.72)
                Next k
            Next i
            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    lEMCsSC(i, j) = lEMCsSC(i, j) + (lLoadsSC(i, j) * 4046.8564 * 12 / lDenomS(i))
                Next j
            Next i
            'calc runoff volume in cfs
            For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                For k = 1 To lMaxlu
                    lVolume(i) = lVolume(i) + (lPrecS(i) / 12 * lRatio * lRunoffL(k) * lAreasLS(k, i) * 3.281 * 3.281 / 31536000)  '31536000 is what pload uses as the number of seconds in a year
                Next k
            Next i
        End If

        'calculate loads per acre
        For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
            If lAreasS(i) > 0 Then
                For j = 0 To lConsNames.GetUpperBound(0)  'for each constituent
                    lLoadsPerAcreSC(i, j) = lLoadsSC(i, j) / lAreasS(i)
                Next j
            End If
        Next i
        Logger.Dbg("LoadsPerAcreCalculated")

        'add group to map for output 
        Dim lGroupName As String = "Estimated Annual Pollutant Loads"
        GisUtil.AddGroup(lGroupName)

        'now build the output shapefiles 
        Dim lOutputShapefileName As String
        Dim lLayerDesc As String = ""
        For j = 0 To lConsNames.GetUpperBound(0)
            'for each constituent
            If aConstituents.IndexOf(lConsNames(j)) > -1 Then
                For k = 1 To 3
                    If k = 3 And aUseExportCoefficent Then
                        'cant build an emc shapefile for ec method
                    Else
                        lOutputShapefileName = ""
                        GisUtil.SaveSelectedFeatures(lSubbasinLayerIndex, lSelectedAreaIndexes, _
                                                     lOutputShapefileName)
                        'need a prj file
                        Dim lSubbasinFileName As String = GisUtil.LayerFileName(lSubbasinLayerIndex)
                        Dim lInputProjectionFileName As String = FilenameSetExt(lSubbasinFileName, "prj")
                        If FileExists(lInputProjectionFileName) Then
                            FileCopy(lInputProjectionFileName, FilenameSetExt(lOutputShapefileName, "prj"))
                        End If

                        'add the output shapefile to the map
                        If Mid(lConsNames(j), 1, 5) = "FECAL" Or Mid(lConsNames(j), 1, 9) = "PATHOGENS" Or Mid(lConsNames(j), 1, 8) = "BACTERIA" Then
                            If k = 1 Then
                                lLayerDesc = lConsNames(j) & " Load Per Acre (counts)"
                            ElseIf k = 2 Then
                                lLayerDesc = lConsNames(j) & " Load (counts)"
                            ElseIf k = 3 Then
                                lLayerDesc = lConsNames(j) & " EMC (counts/100mL)"
                            End If
                        Else
                            If k = 1 Then
                                lLayerDesc = lConsNames(j) & " Load Per Acre (lbs)"
                            ElseIf k = 2 Then
                                lLayerDesc = lConsNames(j) & " Load (lbs)"
                            ElseIf k = 3 Then
                                lLayerDesc = lConsNames(j) & " EMC (mg/L)"
                            End If
                        End If

                        Dim lBasename As String = lLayerDesc
                        i = 0
                        Do While GisUtil.IsLayer(lLayerDesc)
                            i = i + 1
                            lLayerDesc = lBasename & " " & i
                        Loop
                        If Not GisUtil.AddLayerToGroup(lOutputShapefileName, lLayerDesc, lGroupName) Then
                            lProblem = "Cant load layer " & lOutputShapefileName
                            Logger.Dbg(lProblem)
                        End If

                        'add fields to the output shapefile
                        Dim lOutputLayerIndex As String = GisUtil.LayerIndex(lLayerDesc)
                        Dim lFieldIndex As Integer
                        Dim lFlowFieldIndex As Integer
                        GisUtil.StartSetFeatureValue(lOutputLayerIndex)

                        Dim lFieldNameSuffix As String = ""
                        If k = 1 Then
                            lFieldNameSuffix = "_acre"
                        ElseIf k = 2 Then
                            lFieldNameSuffix = "_load"
                        ElseIf k = 3 Then
                            lFieldNameSuffix = "_emc"
                        End If

                        If k = 3 Then
                            'also add field for flow volumes in cfs
                            Try
                                lFlowFieldIndex = GisUtil.FieldIndex(lOutputLayerIndex, "Flow_Cfs")
                            Catch
                                lFlowFieldIndex = GisUtil.AddField(lOutputLayerIndex, "Flow_Cfs", 2, 10)
                            End Try
                        End If

                        'add loads
                        Try
                            lFieldIndex = GisUtil.FieldIndex(lOutputLayerIndex, lConsNames(j) & lFieldNameSuffix)
                        Catch
                            lFieldIndex = GisUtil.AddField(lOutputLayerIndex, lConsNames(j) & lFieldNameSuffix, 2, 10)
                        End Try

                        For i = 0 To lSelectedAreaIndexes.Count - 1 'for each subbasin
                            If k = 1 Then
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, lLoadsPerAcreSC(i, j))
                            ElseIf k = 2 Then
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, lLoadsSC(i, j))
                            ElseIf k = 3 Then
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFieldIndex, i, lEMCsSC(i, j))
                                GisUtil.SetFeatureValue(lOutputLayerIndex, lFlowFieldIndex, i, lVolume(i))
                            End If
                        Next i
                        GisUtil.StopSetFeatureValue(lOutputLayerIndex)

                        'TODO: should this only include some fields, but which ones?
                        Dim lStr As String = vbCrLf & "Row"
                        For f As Integer = 0 To GisUtil.NumFields - 1
                            lStr &= vbTab & GisUtil.FieldName(f, lOutputLayerIndex)
                        Next
                        For i = 0 To GisUtil.NumFeatures(lOutputLayerIndex) - 1
                            lStr &= vbCrLf & i + 1
                            For f As Integer = 0 To GisUtil.NumFields - 1
                                lStr &= vbTab & GisUtil.FieldValue(lOutputLayerIndex, i, f)
                            Next
                        Next
                        Logger.Dbg("Loads" & lStr)

                        'set renderer for this layer
                        'TODO: should this be a color ramp?

                        GisUtil.UniqueValuesRenderer(lOutputLayerIndex, lFieldIndex)
                    End If
                Next k
            End If
        Next j
        Logger.Dbg("Complete")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aBMPGridSource"></param>
    ''' <param name="aBMPType"></param>
    ''' <param name="aConsName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetEfficiency(ByVal aBMPGridSource As atcGridSource, _
                                   ByVal aBMPType As String, _
                                   ByVal aConsName As String) As Single
        'look thru table of BMP efficiency values looking for this bmp type and consitutuent
        Dim i As Integer
        Dim j As Integer

        GetEfficiency = 0.0
        With aBMPGridSource
            For i = 1 To .Rows - 1
                If .CellValue(i, 1) = aBMPType Then
                    For j = 1 To .Columns
                        If .CellValue(0, j) = aConsName Then
                            GetEfficiency = .CellValue(i, j)
                            Exit For
                        End If
                    Next j
                    Exit For
                End If
            Next i
        End With

    End Function

    Private Function GetPointLoad(ByVal aPointGridSource As atcGridSource, _
                                  ByVal aFacility As String, _
                                  ByVal aConsName As String) As Single
        'look thru table of point loads values looking for this facility and consitutuent
        Dim i As Integer
        Dim j As Integer

        GetPointLoad = 0.0
        With aPointGridSource
            For i = 1 To .Rows - 1
                If .CellValue(i, 1) = aFacility Then
                    For j = 0 To .Columns
                        If .CellValue(0, j) = aConsName Then
                            GetPointLoad = .CellValue(i, j)
                            Exit For
                        End If
                    Next j
                    Exit For
                End If
            Next i
        End With

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aAreaLayerIndex"></param>
    ''' <param name="aSelectedAreaIndexes"></param>
    ''' <param name="aAreaLandSub"></param>
    ''' <remarks></remarks>
    Private Sub CalculateGIRASAreas(ByVal aAreaLayerIndex As Integer, _
                                ByVal aSelectedAreaIndexes As Collection, _
                                ByRef aAreaLandSub(,) As Double)

        Dim lProblem As String = ""
        Dim lLanduseLayerIndex As Integer
        Dim lLandUseFieldIndex As Integer
        Dim lGridSource As New atcGridSource

        'set land use index layer
        Try
            lLanduseLayerIndex = GisUtil.LayerIndex("Land Use Index")
        Catch
            lProblem = "Missing Land Use Index Layer"
            Logger.Dbg(lProblem)
        End Try

        If Len(lProblem) = 0 Then

            Try
                lLandUseFieldIndex = GisUtil.FieldIndex(lLanduseLayerIndex, "COVNAME")
            Catch
                lProblem = "Expected field missing from Land Use Index Layer"
                Logger.Dbg(lProblem)
            End Try

            If Len(lProblem) = 0 Then
                'figure out which land use tiles to list
                Dim lcluTiles As New Collection
                Dim lNewFileName As String
                Dim j As Integer
                Dim i As Integer
                For i = 1 To GisUtil.NumFeatures(lLanduseLayerIndex)
                    'loop thru each shape of land use index shapefile
                    lNewFileName = GisUtil.FieldValue(lLanduseLayerIndex, i - 1, lLandUseFieldIndex)
                    For j = 1 To aSelectedAreaIndexes.Count
                        If GisUtil.OverlappingPolygons(lLanduseLayerIndex, i - 1, aAreaLayerIndex, aSelectedAreaIndexes(j)) Then
                            'yes, add it
                            lcluTiles.Add(lNewFileName)
                            Exit For
                        End If
                    Next j
                Next i

                Dim lPathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex)) & "\landuse"
                Dim lLandUseFieldName As String
                For j = 1 To lcluTiles.Count
                    'loop thru each land use tile
                    lNewFileName = lPathName & "\" & lcluTiles(j) & ".shp"
                    If Not FileExists(lNewFileName) Then
                        lProblem = "Missing GIRAS Land Use Layer " & lcluTiles(j) & vbCrLf & _
                                   "GIRAS Land Use data has not been added to the project."
                        Logger.Msg(lProblem, "Calculate GIRAS Areas Problem")
                    End If
                    If Len(lProblem) = 0 Then
                        If Not GisUtil.AddLayer(lNewFileName, lcluTiles(j)) Then
                            lProblem = "Failed to add GIRAS Land Use Layer " & lcluTiles(j)
                            Logger.Dbg(lProblem)
                        End If
                    End If

                    If Len(lProblem) = 0 Then
                        lLandUseFieldName = "LUCODE"
                        If GisUtil.LayerName(aAreaLayerIndex) <> "<none>" Then
                            'do overlay
                            Dim lLayer1Index As Integer = GisUtil.LayerIndex(lcluTiles(j))
                            GisUtil.TabulatePolygonAreas(lLayer1Index, _
                                                         GisUtil.FieldIndex(lLayer1Index, lLandUseFieldName), _
                                                         aAreaLayerIndex, aSelectedAreaIndexes, _
                                                         aAreaLandSub)
                        End If
                    End If
                Next j
            End If
        End If
    End Sub

    Private Sub TabulateBMPAreas(ByVal aBMPLayerIndex As Integer, ByVal aBMPFeatureIndex As Integer, _
                                 ByVal aSubbasinLayerIndex As Integer, ByVal aSubbasinFeatureIndex As Integer, _
                                 ByVal aLandUseType As String, ByVal aLandUseLayer As String, ByVal aLandUseId As String, ByVal aMaxlu As Integer, _
                                 ByRef aBMPAreaL() As Double)

        If GisUtil.OverlappingPolygons(aBMPLayerIndex, aBMPFeatureIndex, aSubbasinLayerIndex, aSubbasinFeatureIndex) Then

            'this bmp polygon and subbasin overlap, process it
            Dim lOverlayOutputFile As String = PathNameOnly(GisUtil.LayerFileName(aSubbasinLayerIndex)) & "\tempbmpoverlay.shp"
            GisUtil.OverlaySelected(aBMPLayerIndex, aBMPFeatureIndex, aSubbasinLayerIndex, aSubbasinFeatureIndex, lOverlayOutputFile, True)
            If Not FileExists(FilenameNoExt(lOverlayOutputFile) & ".prj") Then
                'create .prj file
                IO.File.Copy(FilenameNoExt(GisUtil.LayerFileName(aSubbasinLayerIndex)) & ".prj", FilenameNoExt(lOverlayOutputFile) & ".prj")
            End If
            GisUtil.AddLayer(lOverlayOutputFile, "TempBmpOverlay")

            Dim lOverlayIndex As Integer
            lOverlayIndex = GisUtil.LayerIndex(lOverlayOutputFile)

            Dim lLanduseLayerIndex As Integer
            Dim lOverlayIndexes As New Collection
            lOverlayIndexes.Add(0)  'there will be only one feature in overlay 
            Dim lAreasLS(aMaxlu, 1) As Double

            If aLandUseType = "USGS GIRAS Shapefile" Then
                CalculateGIRASAreas(lOverlayIndex, lOverlayIndexes, _
                                    lAreasLS)
            ElseIf aLandUseType = "Other Shapefile" Then
                lLanduseLayerIndex = GisUtil.LayerIndex(aLandUseLayer)
                GisUtil.TabulatePolygonAreas(lLanduseLayerIndex, _
                                             GisUtil.FieldIndex(lLanduseLayerIndex, aLandUseId), _
                                             lOverlayIndex, lOverlayIndexes, _
                                             lAreasLS)
            Else 'grid
                lLanduseLayerIndex = GisUtil.LayerIndex(aLandUseLayer)
                Dim lGridmax As Integer = System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
                Dim laAreaLS(lGridmax, GisUtil.NumFeatures(lOverlayIndex)) As Double
                GisUtil.TabulateAreas(lLanduseLayerIndex, lOverlayIndex, _
                                      laAreaLS)
                'transfer values from selected polygons to lAreasLS
                For lLandUseIndex As Integer = 1 To aMaxlu
                    lAreasLS(lLandUseIndex, 0) = laAreaLS(lLandUseIndex, 1)
                Next
            End If

            'transfer values from 2d array to 1d array for returning
            For lLandUseIndex As Integer = 1 To aMaxlu
                aBMPAreaL(lLandUseIndex) = lAreasLS(lLandUseIndex, 0)
            Next

            GisUtil.RemoveLayer(lOverlayIndex)
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aGridValuesFileName"></param>
    ''' <param name="aGridValuesSource"></param>
    ''' <remarks></remarks>
    Public Sub SetGridValuesSource(ByVal aGridValuesFileName As String, ByVal aGridValuesSource As atcGridSource)
        Dim lDbf As IatcTable
        lDbf = atcUtility.atcTableOpener.OpenAnyTable(aGridValuesFileName)

        With aGridValuesSource
            .Rows = 1
            .Columns = lDbf.NumFields
            .ColorCells = True
            .FixedRows = 1
            .FixedColumns = 1
            For i As Integer = 1 To lDbf.NumFields
                .CellValue(0, i) = lDbf.FieldName(i)
                .CellColor(0, i) = SystemColors.ControlDark
            Next i

            For k As Integer = 1 To lDbf.NumRecords
                lDbf.CurrentRecord = k
                For i As Integer = 1 To lDbf.NumFields
                    .CellValue(k, i) = lDbf.Value(i)
                    If i > 2 Then
                        .CellEditable(k, i) = True
                    Else
                        .CellColor(k, i) = SystemColors.ControlDark
                    End If
                Next i
            Next k
        End With
    End Sub
End Module
