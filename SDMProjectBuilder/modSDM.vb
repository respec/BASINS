Imports MapWinUtility

Public Module modSDM
    Friend g_MapWin As MapWindow.Interfaces.IMapWin
    Friend g_AppNameRegistry As String = "FramesSDM" 'For preferences in registry
    Friend g_AppNameShort As String = "FramesSDM"
    Friend g_AppNameLong As String = "Frames SDM"
    Friend g_MapWinWindowHandle As Integer
    Friend g_ProgramDir As String = ""
    Friend g_ClipCatchments As Boolean = True
    Private g_MinCatchmentKM2 As Double = 1.0 'Minimum catchment size
    Private g_MinFlowlineKM As Double = 15.0 'Minimum flowline length
    Public g_KeepConnectingRemovedFlowLines As Boolean = True
    Friend pBuildFrm As frmBuildNew

    Friend Sub UpdateSelectedFeatures()
        If Not pBuildFrm Is Nothing AndAlso g_MapWin.Layers.NumLayers > 0 AndAlso g_MapWin.Layers.CurrentLayer > -1 Then
            Dim lFieldName As String = ""
            Dim lFieldDesc As String = ""
            Dim lField As Integer
            Dim lNameIndex As Integer = -1
            Dim lDescIndex As Integer = -1
            Dim lCurLayer As MapWinGIS.Shapefile
            Dim ctext As String

            RefreshView()
            ctext = "Selected Features:" & vbCrLf & "  <none>"
            If g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).LayerType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                lCurLayer = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                If g_MapWin.View.SelectedShapes.NumSelected > 0 Then
                    ctext = "Selected Features:"
                    Select Case IO.Path.GetFileNameWithoutExtension(lCurLayer.Filename).ToLower
                        Case "cat", "huc", "huc250d3"
                            lFieldName = "CU"
                            lFieldDesc = "catname"
                        Case "cnty"
                            lFieldName = "FIPS"
                            lFieldDesc = "cntyname"
                        Case "st"
                            lFieldName = "ST"
                            lFieldDesc = "name"
                    End Select

                    lFieldName = lFieldName.ToLower
                    lFieldDesc = lFieldDesc.ToLower
                    For lField = 0 To lCurLayer.NumFields - 1
                        If lCurLayer.Field(lField).Name.ToLower = lFieldName Then
                            lNameIndex = lField
                        End If
                        If lCurLayer.Field(lField).Name.ToLower = lFieldDesc Then
                            lDescIndex = lField
                        End If
                    Next

                    Dim lSelected As Integer
                    Dim lShape As Integer
                    Dim lname As String
                    Dim ldesc As String
                    Dim lSf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                    For lSelected = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
                        lShape = g_MapWin.View.SelectedShapes.Item(lSelected).ShapeIndex()
                        lname = ""
                        ldesc = ""
                        If lNameIndex > -1 Then
                            lname = lSf.CellValue(lNameIndex, lShape)
                        End If
                        If lDescIndex > -1 Then
                            ldesc = lSf.CellValue(lDescIndex, lShape)
                        End If
                        ctext = ctext & vbCrLf & "  " & lname & " : " & ldesc
                    Next
                End If
                pBuildFrm.txtSelected.Text = ctext
            End If
        End If
    End Sub

    Friend Sub ClearLayers()
        g_MapWin.Layers.Clear()
    End Sub

    Friend Sub RefreshView()
        g_MapWin.Refresh()
    End Sub

    Friend Sub ProcessNetwork(ByVal aParms() As Object, _
                              ByRef aSimplifiedFlowlinesFileName As String, _
                              ByRef aSimplifiedCatchmentsFileName As String)
        Dim lHuc As String = aParms(0)
        Dim lShapeOfInterest As MapWinGIS.Shape = aParms(1)
        Dim lProjectFolder As String = aParms(2)
        Dim lProblem As String = ""

        aSimplifiedFlowlinesFileName = ""
        aSimplifiedCatchmentsFileName = ""

        Try
            Dim lCatchmentsToUseFilename As String
            Dim lFlowLinesToUseFilename As String

            lCatchmentsToUseFilename = IO.Path.Combine(lProjectFolder, "nhdplus" & lHuc & "\drainage\catchment.shp")
            lFlowLinesToUseFilename = IO.Path.Combine(lProjectFolder, "nhdplus" & lHuc & "\hydrography\nhdflowline.shp")

            Dim lFlowLinesShapeFilename As String = lFlowLinesToUseFilename
            Dim lCatchmentsShapeFilename As String = lCatchmentsToUseFilename
            lCatchmentsToUseFilename = IO.Path.Combine(lProjectFolder, "nhdplus" & lHuc & "\drainage\usecatchment.shp")
            lFlowLinesToUseFilename = IO.Path.Combine(lProjectFolder, "nhdplus" & lHuc & "\hydrography\usenhdflowline.shp")

            Dim lCatchments As New MapWinGIS.Shapefile
            If lCatchments.Open(lCatchmentsShapeFilename) Then
                Logger.Status("CatchmentCount before Clipping " & lCatchments.NumShapes)
                If g_ClipCatchments Then
                    ClipCatchments(lCatchmentsToUseFilename, lCatchments, lShapeOfInterest)
                Else
                    atcUtility.TryCopyShapefile(lCatchmentsShapeFilename, lCatchmentsToUseFilename)
                End If
            Else
                Logger.Status("Unable to open NHDPlus catchments in '" & lCatchmentsShapeFilename & "'")
            End If

            'check for channel with no contrib area - no associated catchment COMID, remove and report
            If ClipFlowLinesToCatchments(lCatchmentsToUseFilename, lFlowLinesShapeFilename, lFlowLinesToUseFilename) Then
                Logger.Status("Clipped Flowlines")
            Else
                Logger.Status("Unable to clip NHDPlus flowlines in '" & lFlowLinesShapeFilename & "'")
            End If

            Logger.Status("CombineShortOrBraidedFlowlines for " & lHuc & " with MinCatchment " & g_MinCatchmentKM2)
            CombineShortOrBraidedFlowlines(lFlowLinesToUseFilename, lCatchmentsToUseFilename, _
                                           g_MinCatchmentKM2, g_MinFlowlineKM)
            aSimplifiedFlowlinesFileName = lFlowLinesToUseFilename.Replace("flowline", "flowlineNoShort")
            aSimplifiedCatchmentsFileName = lCatchmentsToUseFilename.Replace("catchment", "catchmentNoShort")

            Logger.Status("DoneSimplifyHydrography")
            My.Computer.FileSystem.CurrentDirectory = lProjectFolder

        Catch lEx As Exception
            lProblem = "Exception " & lEx.Message & vbCrLf & lEx.StackTrace
            Logger.Status(lProblem)
        End Try

    End Sub

    Friend Sub LogMessage(ByVal aLog As IO.StreamWriter, ByVal aMessage As String)
        If aLog Is Nothing Then
            Logger.Dbg(aMessage)
        Else
            aLog.WriteLine(aMessage)
            Debug.WriteLine(aMessage)
        End If
    End Sub
End Module
