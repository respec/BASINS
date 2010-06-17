Imports MapWinUtility

Friend Module modNetwork
    Private pCheckCount As Integer
    Private pOutletComIds As Generic.List(Of Long)
    Private pFlowlinesLengthFieldIndex As Integer
    Private pFlowlinesCumLenFieldIndex As Integer  'Adding cumulative length field to NHDPlus (computing from length)
    Private pFlowlinesDownstreamComIdFieldIndex As Integer
    Private pFlowlinesComIdFieldIndex As Integer
    Private pFlowlinesToNodeIndex As Integer
    Private pFlowlinesFromNodeIndex As Integer
    Private pFlowlinesLocDrainAreaIndex As Integer 'Adding local drainage area field to NHDPlus (computing from cumulative)
    Private pFlowlinesCumDrainAreaIndex As Integer
    Private pCatchmentComIdFieldIndex As Integer
    Private pCatchmentAreaIndex As Integer

    Private Function FieldIndex(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldName As String) As Integer
        For lFieldIndex As Integer = 0 To aShapeFile.NumFields - 1
            If aShapeFile.Field(lFieldIndex).Name.ToUpper = aFieldName.ToUpper Then 'this is the field we want
                Return lFieldIndex
            End If
        Next
        Logger.Dbg("FieldIndex:Error:FieldName:" & aFieldName & ":IsNotRecognized")
        Return -1
    End Function

    ''Find ArrayList of record numbers by field value as string
    'Private Function FindRecords(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As String) As ArrayList
    '    Dim lRecordsMatching As New ArrayList
    '    For lRecordIndex As Integer = 0 To aShapeFile.NumShapes - 1
    '        If aShapeFile.CellValue(aFieldIndex, lRecordIndex) = aValue Then 'this is the record we want
    '            lRecordsMatching.Add(lRecordIndex)
    '        End If
    '    Next
    '    Return lRecordsMatching
    'End Function

    ''' <summary>
    ''' Return record indexes of shapes with value in aFieldIndex matching aValue
    ''' </summary>
    ''' <param name="aShapeFile">shapes to search</param>
    ''' <param name="aFieldIndex">field index to check</param>
    ''' <param name="aValue">value of field to match</param>
    ''' <returns>list of Integer record indexes</returns>
    Private Function FindRecords(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As Long) As Generic.List(Of Integer)
        Dim lRecordsMatching As New Generic.List(Of Integer)
        For lRecordIndex As Integer = 0 To aShapeFile.NumShapes - 1
            Try
                If CLng(aShapeFile.CellValue(aFieldIndex, lRecordIndex)) = aValue Then 'this is the record we want
                    lRecordsMatching.Add(lRecordIndex)
                End If
            Catch 'Ignore non-numeric values
            End Try
        Next
        Return lRecordsMatching
    End Function

    ''Find record by field value as string
    'Private Function FindRecord(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As String) As Integer
    '    For lRecordIndex As Integer = 0 To aShapeFile.NumShapes - 1
    '        If aShapeFile.CellValue(aFieldIndex, lRecordIndex) = aValue Then 'this is the record we want
    '            Return lRecordIndex
    '        End If
    '    Next
    '    Return -1
    'End Function

    'Find first matching record by field value as Long
    Private Function FindRecord(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As Long) As Integer
        Dim lFieldValue As Long = 0 'Initial value does not matter, gets set in UpdateValueIfNotNull before being checked
        For lRecordIndex As Integer = 0 To aShapeFile.NumShapes - 1
            Try
                If UpdateValueIfNotNull(lFieldValue, aShapeFile.CellValue(aFieldIndex, lRecordIndex)) AndAlso lFieldValue = aValue Then 'this is the record we want
                    Return lRecordIndex
                End If
            Catch 'Ignore non-numeric values
            End Try
        Next
        Return -1
    End Function

    ''' <summary>
    ''' Combines NHDPlus flowline shapes and attributes
    ''' </summary>
    ''' <param name="aFlowlinesShapeFile">Flowline shape file</param>
    ''' <param name="aSourceBaseIndex">Record number of flowline to be kept</param>
    ''' <param name="aSourceDeletingIndex">Record number of flowline to be merged and deleted</param>
    ''' <param name="aMergeShapes">True to keep all flowline segments, False to keep only base segments</param>
    ''' <param name="aKeepCosmeticRemovedLine">True to keep a flowline in the layer even when it is removed from connectivity</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CombineFlowlines(ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile, _
                                      ByVal aSourceBaseIndex As Integer, _
                                      ByVal aSourceDeletingIndex As Integer, _
                                      ByVal aMergeShapes As Boolean, _
                                      ByVal aKeepCosmeticRemovedLine As Boolean) As Boolean
        Try
            Dim lSourceBaseIndex As Integer = aSourceBaseIndex
            With aFlowlinesShapeFile
                'Dim lNewDownstreamComId As Long = .CellValue(lFlowlinesComIdFieldIndex, aSourceBaseIndex)
                Dim lDeletedComId As Long = .CellValue(pFlowlinesComIdFieldIndex, aSourceDeletingIndex)
                Dim lBaseComId As Long = .CellValue(pFlowlinesComIdFieldIndex, lSourceBaseIndex)
                Dim lBaseDownStreamComId As Long = .CellValue(pFlowlinesDownstreamComIdFieldIndex, lSourceBaseIndex)
                Dim lDeletingDownstream As Boolean = False
                If lBaseDownStreamComId = lDeletedComId Then
                    Dim lNewDownstreamComId = .CellValue(pFlowlinesDownstreamComIdFieldIndex, aSourceDeletingIndex)
                    Logger.Dbg("ChangeDownFor " & lBaseComId & " From " & lDeletedComId & " to " & lNewDownstreamComId)
                    .EditCellValue(pFlowlinesDownstreamComIdFieldIndex, lSourceBaseIndex, lNewDownstreamComId)
                    lDeletingDownstream = True
                End If

                Dim lMinElev As Double = 1.0E+30
                Dim lMaxElev As Double = -1.0E+30
                Dim lLength As Double = 0
                Dim lNewFieldValues As New ArrayList
                For lFieldIndex As Integer = 0 To .NumFields - 1
                    Dim lFieldName As String = .Field(lFieldIndex).Name.ToUpper
                    Try
                        Dim lValueFromBase As Object = .CellValue(lFieldIndex, lSourceBaseIndex)
                        Dim lValueFromDeleting As Object = .CellValue(lFieldIndex, aSourceDeletingIndex)

                        If IsDBNull(lValueFromDeleting) AndAlso IsDBNull(lValueFromBase) Then 'Both were DBNull, result is too
                            lNewFieldValues.Add(lValueFromBase)
                        Else

                            Select Case lFieldName
                                Case "COMID"
                                    If IsDBNull(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    Else
                                        lNewFieldValues.Add(lValueFromBase)
                                    End If
                                    'Case "FDATE"
                                    'Case "RESOLUTION"
                                Case "GNIS_ID", "GNIS_NAME"
                                    If IsDBNull(lValueFromBase) OrElse CStr(lValueFromBase).Length = 0 Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    Else
                                        lNewFieldValues.Add(lValueFromBase)
                                    End If
                                Case "LENGTHKM", "SHAPE_LENG", "PATHLENGTH"
                                    Dim lValue As Double = 0
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lValue = lValueFromDeleting
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lValue = lValueFromBase
                                    ElseIf aMergeShapes Then
                                        lValue = lValueFromBase + lValueFromDeleting
                                    Else
                                        lValue = lValueFromBase
                                    End If

                                    If lFieldName = "LENGTHKM" Then
                                        lLength = lValue
                                    End If

                                    lNewFieldValues.Add(lValue)

                                    'Case "FLOWDIR"
                                    'Case "FTYPE"
                                    'Case "FCODE"
                                    'Case "ENABLED"
                                Case "TOCOMID"
                                    If lDeletingDownstream Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    Else
                                        lNewFieldValues.Add(lValueFromBase)
                                    End If
                                    'Case "DIRECTION"
                                Case "TOCOMIDMEA"
                                    lNewFieldValues.Add(lValueFromBase)
                                Case "FROMNODE", "UPHYDROSEQ", "UPLEVELPAT"
                                    If lDeletingDownstream Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    End If
                                    'Case "TERMINALPA"
                                    'Case "ARBOLATESU"
                                    'Case "TERMINALFL"
                                    'Case "THINNERCOD"
                                    'Case "UPMINHYDRO"
                                    'Case "DNMINHYDRO"
                                Case "CUMDRAINAG", "CUMLENKM", _
                                     "CUMNLCD_11", "CUMNLCD_12", _
                                     "CUMNLCD_21", "CUMNLCD_22", "CUMNLCD_23", _
                                     "CUMNLCD_31", "CUMNLCD_32", "CUMNLCD_33", _
                                     "CUMNLCD_41", "CUMNLCD_42", "CUMNLCD_43", _
                                     "CUMNLCD_51", _
                                     "CUMNLCD_61", _
                                     "CUMNLCD_71", _
                                     "CUMNLCD_81", "CUMNLCD_82", "CUMNLCD_83", "CUMNLCD_84", "CUMNLCD_85", _
                                     "CUMNLCD_91", "CUMNLCD_92", _
                                     "CUMPCT_CN", "CUMPCT_MX", "CUMSUM_PCT", _
                                     "MAFLOWU", "MAFLOWV", "MAVELU", "MAVELV", _
                                     "AREAWTMAP", "AREAWTMAT", _
                                     "REACHCODE", "TONODE", "HYDROSEQ", "LEVELPATHI", "GRID_CODE", "DNLEVELPAT", "DNMINHYDRO", "DNDRAINCOU"
                                    If lDeletingDownstream Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    Else
                                        lNewFieldValues.Add(lValueFromBase)
                                    End If
                                    'If lFieldName = "CUMLENKM" Then LogMessage(aLog,"Merged CUMLENKM = " & lNewFieldValues(lNewFieldValues.Count - 1))
                                Case "INCRFLOWU", "LOCDRAINA"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(lValueFromDeleting + lValueFromBase)
                                    End If
                                    'If lFieldName = "LOCDRAINA" Then LogMessage(aLog,"Merged LOCDRAINA = " & lNewFieldValues(lNewFieldValues.Count - 1) & " = " & lValueFromDeleting & " + " & lValueFromBase)
                                Case "MAXELEVRAW", "MAXELEVSMO"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(Math.Max(lValueFromDeleting, lValueFromBase))
                                    End If
                                    lMaxElev = lNewFieldValues.Item(lNewFieldValues.Count - 1)
                                Case "STARTFLAG", "WBAREACOMI"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(Math.Max(lValueFromDeleting, lValueFromBase))
                                    End If
                                Case "MINELEVRAW", "MINELEVSMO"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(Math.Min(lValueFromDeleting, lValueFromBase))
                                    End If
                                    lMinElev = lNewFieldValues.Item(lNewFieldValues.Count - 1)
                                Case "STREAMLEVE", "STREAMORDE", "DIVERGENCE", "DELTALEVEL", "DNLEVEL"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(Math.Min(lValueFromDeleting, lValueFromBase))
                                    End If
                                Case "SLOPE"
                                    Dim lSlope As Double
                                    If lLength > 0 AndAlso lMaxElev > 0 AndAlso lMinElev < 1.0E+30 Then
                                        lSlope = (lMaxElev - lMinElev) / (lLength * 1000)
                                    Else
                                        lSlope = 0.001 'TODO: estimate this!
                                        Logger.Dbg("SlopeEstimatedAs " & lSlope & " MaxElev " & lMaxElev & " MinElev " & lMinElev & " Length " & lLength)
                                    End If
                                    lNewFieldValues.Add(lSlope)
                                Case Else
                                    'TODO: move case starting with CUMDRAINAG here? makes sense to keep the field of the one we are keeping?
                                    lNewFieldValues.Add(lValueFromBase)
                                    If Not IsDBNull(lValueFromBase) AndAlso Not IsDBNull(lValueFromDeleting) AndAlso lValueFromBase <> lValueFromDeleting Then
                                        Debug.Print("Case Else " & lFieldName & " = " & lValueFromBase & " not " & lValueFromDeleting)
                                    End If
                                    'Try
                                    '    Dim lValue1 As Object = .CellValue(lFieldIndex, aSourceBaseIndex)
                                    '    Dim lValue2 As Object = .CellValue(lFieldIndex, aSourceDeletingIndex)
                                    '    If (lValue1.ToString = lValue2.ToString) Then
                                    '        lNewFieldValues.Add(lValue1)
                                    '    ElseIf lValue1.ToString.Contains(lValue2.ToString) Then
                                    '        lNewFieldValues.Add(lValue1)
                                    '    Else
                                    '        lNewFieldValues.Add(lValue1 & ", " & lValue2)
                                    '    End If
                                    'Catch
                                    '    lNewFieldValues.Add("")
                                    'End Try
                            End Select
                        End If
                    Catch
                        Logger.Dbg("FailedToSetFieldValueFor " & lFieldIndex & " " & lFieldName.ToUpper)
                        lNewFieldValues.Add("")
                    End Try
                    'LogMessage(aLog,lFieldName & ": " & aFlowlinesShapeFile.CellValue(lFieldIndex, aSourceDeletingIndex) & ", " & aFlowlinesShapeFile.CellValue(lFieldIndex, lSourceBaseIndex) & " -> " & lNewFieldValues(lNewFieldValues.Count - 1))
                Next
                If aMergeShapes Then
                    Dim lMergedShape As New MapWinGIS.Shape
                    Logger.Dbg("FlowlineMerge " & lSourceBaseIndex & " and " & aSourceDeletingIndex)
                    MapWinGeoProc.SpatialOperations.MergeShapes(aFlowlinesShapeFile, lSourceBaseIndex, aSourceDeletingIndex, lMergedShape)
                    .EditDeleteShape(lSourceBaseIndex)
                    .EditInsertShape(lMergedShape, lSourceBaseIndex)
                Else
                    Logger.Dbg("FlowlineKeep " & lSourceBaseIndex & " Discard " & aSourceDeletingIndex)
                End If
                For lFieldIndex As Integer = 0 To .NumFields - 1
                    Try
                        .EditCellValue(lFieldIndex, lSourceBaseIndex, lNewFieldValues(lFieldIndex))
                    Catch
                        Logger.Dbg("FailedToEditFieldValueFor " & lFieldIndex)
                    End Try
                Next
                If Not aMergeShapes AndAlso aKeepCosmeticRemovedLine Then
                    Logger.Dbg("Moving cosmetic line to end of file")
                    Dim lMoveTo As Integer = .NumShapes
                    .EditInsertShape(.Shape(aSourceDeletingIndex), lMoveTo)
                    For lFieldIndex As Integer = 0 To .NumFields - 1
                        Try
                            .EditCellValue(lFieldIndex, lMoveTo, "0")
                        Catch
                        End Try
                    Next
                End If
                .EditDeleteShape(aSourceDeletingIndex)
                If lSourceBaseIndex > aSourceDeletingIndex Then lSourceBaseIndex -= 1
                'lSourceBaseIndex = FindRecord(aFlowlinesShapeFile, pFlowlinesComIdFieldIndex, lBaseComId)
                'If lSourceBaseIndex <> aSourceBaseIndex Then
                '    LogMessage(aLog,"ChangeSourceBaseIndex " & aSourceBaseIndex & " to " & lSourceBaseIndex)
                'End If

                ReconnectUpstreamToDownstream(aFlowlinesShapeFile, lDeletedComId, lBaseComId)

                Logger.Dbg("Kept " & DumpComid(aFlowlinesShapeFile, lBaseComId))
            End With
            Return True
        Catch e As Exception
            Logger.Status("Error: CombineFlowlines:  " & e.Message)
            Return False
        End Try
    End Function

    Private Sub ReconnectUpstreamToDownstream(ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile, _
                                              ByVal aDeletedComId As Long, _
                                              ByVal aDownstreamComId As Long)
        With aFlowlinesShapeFile
            Dim lFlowlinesRecord As Integer = 0
            While lFlowlinesRecord < .NumShapes
                Try
                    Dim lDownstreamComId As Long = .CellValue(pFlowlinesDownstreamComIdFieldIndex, lFlowlinesRecord)
                    If aDeletedComId = lDownstreamComId Then
                        Dim lComId As Long = .CellValue(pFlowlinesComIdFieldIndex, lFlowlinesRecord)
                        'LogMessage(aLog,"ChangeDownFor " & lComId & " From " & lDeletedComId & " to " & lNewDownstreamComId)
                        '.EditCellValue(lFlowlinesDownstreamFieldIndex, lFlowlinesRecord, lNewDownstreamComId)
                        Logger.Dbg("ChangeDownFor " & lComId & " From " & aDeletedComId & " to " & aDownstreamComId & " at " & lFlowlinesRecord)
                        .EditCellValue(pFlowlinesDownstreamComIdFieldIndex, lFlowlinesRecord, aDownstreamComId)
                    End If
                Catch lEx As Exception
                    Logger.Status("ChangeFailedAt " & lFlowlinesRecord, True)
                End Try
                lFlowlinesRecord += 1
            End While
        End With
    End Sub

    Private Function DumpComid(ByVal aFlowLinesShapeFile As MapWinGIS.Shapefile, ByVal aComId As Long) As String
        Dim lRecordIndex As Integer = FindRecord(aFlowLinesShapeFile, pFlowlinesComIdFieldIndex, aComId)
        With aFlowLinesShapeFile
            Dim lToNode As Int64 = -1
            Dim lFromNode As Int64 = -1
            Dim lLength As Double = -1
            Dim lCumLength As Double = -1
            Dim lCumArea As Double = -1
            Dim lLocArea As Double = -1
            Dim lToComId As Long = 0
            UpdateValueIfNotNull(lToNode, .CellValue(pFlowlinesToNodeIndex, lRecordIndex))
            UpdateValueIfNotNull(lFromNode, .CellValue(pFlowlinesFromNodeIndex, lRecordIndex))
            UpdateValueIfNotNull(lLength, .CellValue(pFlowlinesLengthFieldIndex, lRecordIndex))
            UpdateValueIfNotNull(lCumLength, .CellValue(pFlowlinesCumLenFieldIndex, lRecordIndex))
            UpdateValueIfNotNull(lLocArea, .CellValue(pFlowlinesLocDrainAreaIndex, lRecordIndex))
            UpdateValueIfNotNull(lCumArea, .CellValue(pFlowlinesCumDrainAreaIndex, lRecordIndex))
            UpdateValueIfNotNull(lToComId, .CellValue(pFlowlinesDownstreamComIdFieldIndex, lRecordIndex))
            Return aComId & "(" & lRecordIndex & "-u" & lFromNode & "-d" & lToNode & "-Len" & lLength & "-LenCum" & lCumLength & "-Area" & lLocArea & "-AreaCum" & lCumArea & "-t" & lToComId & ")"
        End With
    End Function

    Private Function UpdateValueIfNotNull(ByRef aCurrentObject As Object, ByRef aNewObject As Object) As Boolean
        If aNewObject IsNot System.DBNull.Value Then
            aCurrentObject = aNewObject
            Return True
        End If
        Return False
    End Function

    Private Function CheckConnectivity(ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile) As Boolean
        Dim lResult As Integer
        Math.DivRem(pCheckCount, 100, lResult)
        If lResult = 0 Then
            Logger.Dbg("*** CheckConnectivity Begin")
            Dim lRemoveCount As Integer = 0
            Dim lMissingCount As Integer = 0
            Dim lLargestContribAreaComId As Long = 0
            Dim lLargestContribArea As Double = 0.0
            With aFlowlinesShapeFile
                Dim lFlowlinesRecord As Integer = .NumShapes - 1
                Dim lSkipThisRecord As Boolean = False
                While lFlowlinesRecord >= 0
                    Dim lComId As Long = -1
                    UpdateValueIfNotNull(lComId, .CellValue(pFlowlinesComIdFieldIndex, lFlowlinesRecord))
                    Dim lContribArea As Double = -1
                    If UpdateValueIfNotNull(lContribArea, .CellValue(pFlowlinesCumDrainAreaIndex, lFlowlinesRecord)) _
                       AndAlso lLargestContribArea < lContribArea Then
                        lLargestContribArea = lContribArea
                        lLargestContribAreaComId = lComId
                        Logger.Dbg("NewLargestContribArea " & DumpComid(aFlowlinesShapeFile, lComId))
                    End If
                    Dim lDownstreamComId As Long = -1

                    If UpdateValueIfNotNull(lDownstreamComId, .CellValue(pFlowlinesDownstreamComIdFieldIndex, lFlowlinesRecord)) AndAlso lDownstreamComId > -1 Then
                        If lDownstreamComId = 0 Then
                            Logger.Dbg("ZeroDownstreamComID, add outlet " & DumpComid(aFlowlinesShapeFile, lComId))
                            lMissingCount += 1
                            If Not pOutletComIds.Contains(lComId) Then
                                pOutletComIds.Add(lComId)
                            End If
                        ElseIf FindRecord(aFlowlinesShapeFile, pFlowlinesComIdFieldIndex, lDownstreamComId) < 0 Then
                            Logger.Dbg("MissingDownstreamComID " & lDownstreamComId & ", add outlet " & DumpComid(aFlowlinesShapeFile, lComId))
                            lMissingCount += 1
                            If Not pOutletComIds.Contains(lComId) Then
                                pOutletComIds.Add(lComId)
                            End If
                        End If
                    ElseIf lSkipThisRecord = False Then
                        Logger.Dbg("FlowLineRecordNotNumericDown, add outlet " & DumpComid(aFlowlinesShapeFile, lComId))
                        lMissingCount += 1
                        If Not pOutletComIds.Contains(lComId) Then
                            pOutletComIds.Add(lComId)
                        End If
                    End If
                    lFlowlinesRecord -= 1
                End While
            End With
            Logger.Dbg(" ** CheckConnectivityEnd, MissingOutletsAdded " & lMissingCount)
            If Not pOutletComIds.Contains(lLargestContribAreaComId) Then
                Logger.Dbg(" ** AddMainChannelToOutletComIds")
                pOutletComIds.Add(lLargestContribAreaComId)
            End If
        End If
        pCheckCount += 1
        Return True
    End Function

    Private Function CombineCatchments(ByVal aCatchmentShapeFile As MapWinGIS.Shapefile, _
                                       ByVal aKeptComId As Long, _
                                       ByVal aAddingComId As Long) As Boolean
        Try
            Dim lRecordKeptIndex As Integer = FindRecord(aCatchmentShapeFile, pCatchmentComIdFieldIndex, aKeptComId)
            Dim lRecordAddingIndex As Integer = FindRecord(aCatchmentShapeFile, pCatchmentComIdFieldIndex, aAddingComId)
            Logger.Dbg("CatchmentKeep " & aKeptComId & "(" & lRecordKeptIndex & ") Merge " & aAddingComId & "(" & lRecordAddingIndex & ")")
            If lRecordAddingIndex > -1 AndAlso lRecordKeptIndex > -1 Then
                With aCatchmentShapeFile
                    Dim lNewFieldValues As New ArrayList
                    Dim lAreaBase As Double = 0
                    Dim lAreaAdding As Double = 0
                    Dim lAreaTotal As Double = 0
                    Dim lAreaPercentTotal As Double = 0.0
                    For lFieldIndex As Integer = 0 To .NumFields - 1
                        'Debug.Print("Case """ & .Field(lFieldIndex).Name.ToUpper() & """")
                        Try
                            Dim lValueFromKept As Object = .CellValue(lFieldIndex, lRecordKeptIndex)
                            Dim lValueFromAdding As Object = .CellValue(lFieldIndex, lRecordAddingIndex)

                            If IsDBNull(lValueFromAdding) AndAlso IsDBNull(lValueFromKept) Then 'Both were DBNull, result is too
                                lNewFieldValues.Add(lValueFromKept)
                            Else
                                Select Case .Field(lFieldIndex).Name.ToUpper
                                    Case "COMID"
                                        If IsDBNull(lValueFromKept) Then
                                            lNewFieldValues.Add(lValueFromAdding)
                                        Else
                                            lNewFieldValues.Add(lValueFromKept)
                                        End If
                                    Case "GRID_CODE" 'TODO: check this
                                        If IsDBNull(lValueFromKept) Then
                                            lNewFieldValues.Add(lValueFromAdding)
                                        Else
                                            lNewFieldValues.Add(lValueFromKept)
                                        End If
                                    Case "GRID_COUNT"
                                        If IsDBNull(lValueFromAdding) Then
                                            lNewFieldValues.Add(lValueFromKept)
                                        ElseIf IsDBNull(lValueFromKept) Then
                                            lNewFieldValues.Add(lValueFromAdding)
                                        Else
                                            lNewFieldValues.Add(lValueFromKept + lValueFromAdding)
                                        End If
                                    Case "PROD_UNIT"
                                        If IsDBNull(lValueFromKept) Then
                                            lNewFieldValues.Add(lValueFromAdding)
                                        Else
                                            lNewFieldValues.Add(lValueFromKept)
                                        End If
                                    Case "AREASQKM"
                                        If IsDBNull(lValueFromAdding) Then
                                            lAreaAdding = 0
                                        Else
                                            Try
                                                lAreaAdding = lValueFromAdding
                                            Catch
                                                lAreaAdding = 0
                                            End Try
                                        End If

                                        If IsDBNull(lValueFromKept) Then
                                            lAreaBase = 0
                                        Else
                                            Try
                                                lAreaBase = lValueFromKept
                                            Catch
                                                lAreaBase = 0
                                            End Try
                                        End If

                                        lAreaTotal = lAreaBase + lAreaAdding
                                        lNewFieldValues.Add(lAreaTotal)

                                    Case "NLCD_11", "NLCD_12", _
                                         "NLCD_21", "NLCD_22", "NLCD_23", _
                                         "NLCD_31", "NLCD_32", "NLCD_33", _
                                         "NLCD_41", "NLCD_42", "NLCD_43", _
                                         "NLCD_51", "NLCD_61", "NLCD_71", _
                                         "NLCD_81", "NLCD_82", "NLCD_83", "NLCD_84", "NLCD_85", _
                                         "NLCD_91", "NLCD_92", "PCT_CN", "PCT_MX"
                                        Dim lNewValue As Double = 0
                                        If IsDBNull(lValueFromAdding) Then
                                            lNewValue = lValueFromKept
                                        ElseIf IsDBNull(lValueFromKept) Then
                                            lNewValue = lValueFromAdding
                                        ElseIf lAreaTotal > 0 Then
                                            lNewValue = ((lValueFromKept * lAreaBase) + _
                                                         (lValueFromAdding * lAreaAdding)) / lAreaTotal
                                        Else
                                            lNewFieldValues.Add(0)
                                        End If
                                        lAreaPercentTotal += lNewValue
                                        lNewFieldValues.Add(lNewValue)
                                    Case "SUM_PCT"
                                        lNewFieldValues.Add(lAreaPercentTotal)
                                    Case "PRECIP", "TEMP"
                                        If IsDBNull(lValueFromAdding) Then
                                            lNewFieldValues.Add(lValueFromKept)
                                        ElseIf IsDBNull(lValueFromKept) Then
                                            lNewFieldValues.Add(lValueFromAdding)
                                        ElseIf lAreaTotal > 0 Then
                                            Dim lNewValue As Double = (lValueFromKept * lAreaBase) + _
                                                                      (lValueFromAdding * lAreaAdding) / lAreaTotal
                                            lNewFieldValues.Add(lNewValue)
                                        Else
                                            lNewFieldValues.Add(0)
                                        End If
                                    Case Else
                                        lNewFieldValues.Add("")
                                End Select
                            End If
                        Catch exField As Exception
                            Logger.Status("Exception setting " & .Field(lFieldIndex).Name & ": " & exField.Message, True)
                            lNewFieldValues.Add("")
                        End Try
                    Next

                    Dim lTargetShape As MapWinGIS.Shape = Nothing
                    Try
                        lTargetShape = aCatchmentShapeFile.Shape(lRecordKeptIndex).Clip(aCatchmentShapeFile.Shape(lRecordAddingIndex), MapWinGIS.tkClipOperation.clUnion)
                    Catch e As Exception
                        Logger.Status("Error: CombineCatchments:ClipTargetShape  " & e.Message, True)
                        Try
                            MapWinGeoProc.SpatialOperations.MergeShapes(aCatchmentShapeFile, lRecordKeptIndex, lRecordAddingIndex, lTargetShape)
                        Catch ex As Exception
                            Logger.Status("Error: CombineCatchments:MergeShapes  " & ex.Message, True)
                        End Try
                    End Try
                    If lTargetShape IsNot Nothing Then
                        .EditDeleteShape(lRecordKeptIndex)
                        .EditInsertShape(lTargetShape, lRecordKeptIndex)
                    Else
                        Logger.Dbg("Shape Union Failed, merging by adding parts " & aCatchmentShapeFile.Shape(lRecordKeptIndex).LastErrorCode & " " & aCatchmentShapeFile.LastErrorCode)
                        'Throw New ApplicationException("Shape Union Failed " & aCatchmentShapeFile.Shape(lRecordKeptIndex).LastErrorCode & " " & aCatchmentShapeFile.LastErrorCode)
                        Dim lToPointIndex As Integer = aCatchmentShapeFile.Shape(lRecordKeptIndex).numPoints
                        For lFromPartIndex As Integer = 0 To aCatchmentShapeFile.Shape(lRecordAddingIndex).NumParts - 1
                            Dim lLastFromPointIndex As Integer = aCatchmentShapeFile.Shape(lRecordAddingIndex).numPoints - 1
                            If lFromPartIndex + 1 < aCatchmentShapeFile.Shape(lRecordAddingIndex).NumParts Then
                                lLastFromPointIndex = aCatchmentShapeFile.Shape(lRecordAddingIndex).Part(lFromPartIndex + 1) - 1
                            End If
                            If lLastFromPointIndex >= lFromPartIndex Then
                                aCatchmentShapeFile.Shape(lRecordKeptIndex).InsertPart(lToPointIndex, aCatchmentShapeFile.Shape(lRecordKeptIndex).NumParts)
                                For lFromPointIndex As Integer = aCatchmentShapeFile.Shape(lRecordAddingIndex).Part(lFromPartIndex) To lLastFromPointIndex
                                    aCatchmentShapeFile.Shape(lRecordKeptIndex).InsertPoint(aCatchmentShapeFile.Shape(lRecordAddingIndex).Point(lFromPointIndex), lToPointIndex)
                                    lToPointIndex += 1
                                Next
                            End If
                        Next
                    End If
                    For lFieldIndex As Integer = 0 To .NumFields - 1
                        .EditCellValue(lFieldIndex, lRecordKeptIndex, lNewFieldValues(lFieldIndex))
                    Next
                    .EditDeleteShape(lRecordAddingIndex)
                End With
            Else
                Logger.Dbg("SkipMerge " & lRecordKeptIndex & " " & lRecordAddingIndex)
            End If
            Return True
        Catch e As Exception
            Logger.Status("Error: CombineCatchments:  " & e.Message, True)
            Return False
        End Try
    End Function

    Private Function SortCatchmentsToMatchFlowlines( _
                ByVal aFlowlines As MapWinGIS.Shapefile, _
                ByVal aCatchments As MapWinGIS.Shapefile) As String
        Dim lSortedCatchmentsFilename As String = atcUtility.GetTemporaryFileName(IO.Path.GetFileNameWithoutExtension(aCatchments.Filename), "shp")
        atcUtility.TryCopyShapefile(aCatchments.Filename, lSortedCatchmentsFilename)
        Dim lNewCatchments As New MapWinGIS.Shapefile
        lNewCatchments.Open(lSortedCatchmentsFilename)
        lNewCatchments.StartEditingShapes()
        lNewCatchments.StartEditingTable()

        For lCatchmentShape As Integer = lNewCatchments.NumShapes - 1 To 0 Step -1
            lNewCatchments.EditDeleteShape(lCatchmentShape)
        Next

        Dim lLastFlowlineIndex As Integer = aFlowlines.NumShapes - 1
        Dim llLastCatchmentIndex As Integer = aCatchments.NumShapes - 1
        Dim llLastCatchmentFieldIndex As Integer = aCatchments.NumFields - 1
        Dim lFieldIndex As Integer

        For lFlowlineIndex As Integer = 0 To lLastFlowlineIndex
            Dim lCOMID As String = aFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFlowlineIndex).ToString.Trim
            If lCOMID.Length > 1 Then
                Dim lCatchmentsIndex As Integer
                For lCatchmentsIndex = 0 To llLastCatchmentIndex
                    If lCOMID = aCatchments.CellValue(pCatchmentComIdFieldIndex, lCatchmentsIndex).ToString.Trim Then
                        Exit For
                    End If
                Next
                If lCatchmentsIndex <= llLastCatchmentIndex Then
                    lNewCatchments.EditInsertShape(aCatchments.Shape(lCatchmentsIndex), lFlowlineIndex)
                    For lFieldIndex = 0 To llLastCatchmentFieldIndex
                        lNewCatchments.EditCellValue(lFieldIndex, lFlowlineIndex, aCatchments.CellValue(lFieldIndex, lCatchmentsIndex))
                    Next
                Else
                    Logger.Dbg("Warning: Inserting dummy shape in catchments to align with flowlines because could not find COMID " & lCOMID)
                    lNewCatchments.EditInsertShape(aCatchments.Shape(0), lFlowlineIndex)
                    lNewCatchments.EditCellValue(0, lFlowlineIndex, "0")
                End If
            End If
        Next

        lNewCatchments.StopEditingShapes()
        lNewCatchments.StopEditingTable()
        lNewCatchments.Close()

        Return lSortedCatchmentsFilename
    End Function

    ''' <summary>
    ''' Eliminate flowlines and catchments by combining them up or down stream to reach large enough area and/or length
    ''' Eliminate braided streams by keeping only the preferred channel
    ''' </summary>
    ''' <param name="aFlowlinesFileName"></param>
    ''' <param name="aCatchmentFileName"></param>
    ''' <param name="aMinCatchmentKM2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function CombineShortOrBraidedFlowlines(ByVal aFlowlinesFileName As String, _
                                                   ByVal aCatchmentFileName As String, _
                                                   ByVal aMinCatchmentKM2 As Double, _
                                                   ByVal aMinLengthKM As Double) As Boolean
        pOutletComIds = New Generic.List(Of Long)
        Dim lSaveIntermediate As Integer = 500

        Dim lSimplifiedFlowlinesFileName As String = aFlowlinesFileName.Replace("flowline", "flowlineNoShort")
        atcUtility.TryDeleteShapefile(lSimplifiedFlowlinesFileName)
        atcUtility.TryCopyShapefile(aFlowlinesFileName, lSimplifiedFlowlinesFileName)

        Dim lSimplifiedFlowlines As New MapWinGIS.Shapefile
        If Not lSimplifiedFlowlines.Open(lSimplifiedFlowlinesFileName) Then
            Logger.Status("Could not open '" & lSimplifiedFlowlinesFileName & "'", True)
            Exit Function
        End If
        lSimplifiedFlowlines.StartEditingShapes()
        lSimplifiedFlowlines.StartEditingTable()

        Dim lSimplifiedCatchmentFileName As String = aCatchmentFileName.Replace("catchment", "catchmentNoShort")
        atcUtility.TryDeleteShapefile(lSimplifiedCatchmentFileName)
        atcUtility.TryCopyShapefile(aCatchmentFileName, lSimplifiedCatchmentFileName)

        Dim lSimplifiedCatchment As New MapWinGIS.Shapefile
        If Not lSimplifiedCatchment.Open(lSimplifiedCatchmentFileName) Then
            Logger.Status("Could not open '" & lSimplifiedCatchmentFileName & "'", True)
            Exit Function
        End If
        lSimplifiedCatchment.StartEditingShapes()
        lSimplifiedCatchment.StartEditingTable()

        pFlowlinesLengthFieldIndex = FieldIndex(lSimplifiedFlowlines, "LENGTHKM")
        pFlowlinesCumLenFieldIndex = FieldIndex(lSimplifiedFlowlines, "CUMLENKM")
        If pFlowlinesCumLenFieldIndex < 0 Then 'Need to add cumulative length field
            pFlowlinesCumLenFieldIndex = lSimplifiedFlowlines.NumFields
            Dim lLenField As MapWinGIS.Field = lSimplifiedFlowlines.Field(pFlowlinesLengthFieldIndex)
            Dim lNewField As New MapWinGIS.Field
            With lNewField
                .Name = "CUMLENKM"
                .Type = lLenField.Type
                .Width = lLenField.Width + 1
                .Precision = lLenField.Precision
            End With
            lSimplifiedFlowlines.EditInsertField(lNewField, pFlowlinesCumLenFieldIndex)
        End If

        pFlowlinesCumDrainAreaIndex = FieldIndex(lSimplifiedFlowlines, "CUMDRAINAG")
        pFlowlinesLocDrainAreaIndex = FieldIndex(lSimplifiedFlowlines, "LOCDRAINA")
        If pFlowlinesLocDrainAreaIndex < 0 Then 'Need to add local drainage area field
            pFlowlinesLocDrainAreaIndex = lSimplifiedFlowlines.NumFields
            Dim lCumField As MapWinGIS.Field = lSimplifiedFlowlines.Field(pFlowlinesCumDrainAreaIndex)
            Dim lNewField As New MapWinGIS.Field
            With lNewField
                .Name = "LOCDRAINA"
                .Type = lCumField.Type
                .Width = lCumField.Width
                .Precision = lCumField.Precision
            End With
            lSimplifiedFlowlines.EditInsertField(lNewField, pFlowlinesLocDrainAreaIndex)
        End If

        pFlowlinesDownstreamComIdFieldIndex = FieldIndex(lSimplifiedFlowlines, "TOCOMID")
        pFlowlinesComIdFieldIndex = FieldIndex(lSimplifiedFlowlines, "COMID")
        pFlowlinesToNodeIndex = FieldIndex(lSimplifiedFlowlines, "TONODE")
        pFlowlinesFromNodeIndex = FieldIndex(lSimplifiedFlowlines, "FROMNODE")

        pCatchmentComIdFieldIndex = FieldIndex(lSimplifiedCatchment, "COMID")
        pCatchmentAreaIndex = FieldIndex(lSimplifiedCatchment, "AREASQKM")

        Logger.Status("Process " & lSimplifiedFlowlines.NumShapes & " Flowlines " _
                                 & lSimplifiedCatchment.NumShapes & " Catchments ", True)

        Dim lOldFlowlineCatchmentDelta As Integer
        Dim lBraidedFlowlinesCombinedCount As Integer = 0
        Dim lRemoveFlowlineWithoutCatchmentCount As Integer = 0
        Dim lFlowlinesRecord As Integer = 0
        'Remove flowlines with no catchment
        While lFlowlinesRecord < lSimplifiedFlowlines.NumShapes
            Dim lComId As Long = -1
            UpdateValueIfNotNull(lComId, lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFlowlinesRecord))
            Dim lCatchmentRecord As Integer = FindRecord(lSimplifiedCatchment, pCatchmentComIdFieldIndex, lComId)
            If lCatchmentRecord = -1 Then 'missing catchment
                Logger.Dbg("RemoveFlowline " & DumpComid(lSimplifiedFlowlines, lComId) & " without catchment")
                If pOutletComIds.Contains(lComId) Then
                    pOutletComIds.Remove(lComId)
                End If
                Dim lToNode As Int64 = -1
                UpdateValueIfNotNull(lToNode, lSimplifiedFlowlines.CellValue(pFlowlinesToNodeIndex, lFlowlinesRecord))
                Dim lRecordCombineIndex As Integer = FindRecord(lSimplifiedFlowlines, pFlowlinesFromNodeIndex, lToNode)
                If lToNode > 0 AndAlso lRecordCombineIndex > -1 Then
                    Dim lToComId As Long = -1
                    UpdateValueIfNotNull(lToComId, lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lRecordCombineIndex))
                    Logger.Dbg("MergeDownWith " & DumpComid(lSimplifiedFlowlines, lToComId))
                    CombineFlowlines(lSimplifiedFlowlines, lRecordCombineIndex, lFlowlinesRecord, True, False)
                Else
                    Dim lFromNode As Int64 = -1
                    UpdateValueIfNotNull(lFromNode, lSimplifiedFlowlines.CellValue(pFlowlinesFromNodeIndex, lFlowlinesRecord))
                    Dim lFromNodeCount As Integer = Count(lSimplifiedFlowlines, pFlowlinesFromNodeIndex, lFromNode)
                    lRecordCombineIndex = FindRecord(lSimplifiedFlowlines, pFlowlinesToNodeIndex, lFromNode)
                    If lFromNode > 0 AndAlso lRecordCombineIndex > -1 Then
                        Logger.Dbg("MergeUpWith " & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lRecordCombineIndex)))
                        CombineFlowlines(lSimplifiedFlowlines, lRecordCombineIndex, lFlowlinesRecord, True, False)
                    Else
                        Logger.Status("OrphanFlowLine - no connections", True)
                        If lSimplifiedFlowlines.EditDeleteShape(lFlowlinesRecord) Then
                            Logger.Dbg("Removed " & lComId & " at " & lFlowlinesRecord & " of " & lSimplifiedFlowlines.NumShapes)
                        Else
                            Logger.Dbg("RemoveFailed")
                        End If
                    End If
                End If
            Else
                lFlowlinesRecord += 1
            End If
        End While
        Logger.Status("Removed Flowlines Without Catchments, now " & lSimplifiedFlowlines.NumShapes & " flowlines, " & lSimplifiedCatchment.NumShapes & " catchments", True)

        pCheckCount = 0
        CheckConnectivity(lSimplifiedFlowlines)

        Dim lFlowlineCatchmentDelta As Integer = lSimplifiedFlowlines.NumShapes - lSimplifiedCatchment.NumShapes
        lFlowlinesRecord = 0
        'Remove braided flowlines
        While lFlowlinesRecord < lSimplifiedFlowlines.NumShapes
            Dim lComId As Long = lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFlowlinesRecord)
            Dim lFromNode As Long = lSimplifiedFlowlines.CellValue(pFlowlinesFromNodeIndex, lFlowlinesRecord)
            Dim lFromNodeCount As Integer = Count(lSimplifiedFlowlines, pFlowlinesFromNodeIndex, lFromNode)
            If lFromNodeCount > 1 AndAlso lFromNode > 0 Then 'braided channel
                'Dim lDownstreamCount As Integer = Count(lSimplifiedFlowlines, lFlowlinesDownstreamComIdFieldIndex, lComId)
                Dim lDownstreamRecords As Generic.List(Of Integer) = FindRecords(lSimplifiedFlowlines, pFlowlinesDownstreamComIdFieldIndex, lComId)
                Dim lDownstreamCount As Integer = lDownstreamRecords.Count
                Logger.Dbg("BraidedChannel at " & lFlowlinesRecord & " of " & lSimplifiedFlowlines.NumShapes & " DownstreamCount " & lDownstreamCount)
                For Each lDownstreamRecord As Integer In lDownstreamRecords
                    Logger.Dbg("Downstream" & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lDownstreamRecord)))
                Next
                If lDownstreamCount > 0 Then '
                    Logger.Dbg("PreferredChannel Keep " & lComId)
                Else ' not preferred(branch - remove)
                    Dim lCumAreaLarge As Double = 0
                    Dim lFindRecordKeep As Integer = -1
                    Dim lFindRecordRemove As Integer = -1
                    Dim lFindRecords As Generic.List(Of Integer) = FindRecords(lSimplifiedFlowlines, pFlowlinesFromNodeIndex, lFromNode)
                    For Each lFindRecord As Integer In lFindRecords
                        Dim lCumArea As Double = -1
                        UpdateValueIfNotNull(lCumArea, lSimplifiedFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lFindRecord))
                        Logger.Dbg(" Found " & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFindRecord)))
                        If lCumAreaLarge < lCumArea Then
                            lCumAreaLarge = lCumArea
                            lFindRecordRemove = lFindRecordKeep
                            lFindRecordKeep = lFindRecord
                        Else
                            lFindRecordRemove = lFindRecord
                        End If
                    Next
                    Dim lCombineWithComId As Long = lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFindRecordKeep)
                    Dim lRemoveComId As Long = lSimplifiedFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFindRecordRemove)
                    If CombineCatchments(lSimplifiedCatchment, lCombineWithComId, lRemoveComId) Then
                        CombineFlowlines(lSimplifiedFlowlines, lFindRecordKeep, lFindRecordRemove, False, False)
                    Else
                        Logger.Status("Failed to merge braided catchment " & lRemoveComId & " into " & lCombineWithComId, True)
                    End If
                    lFlowlinesRecord -= 1
                    lBraidedFlowlinesCombinedCount += 1
                    lOldFlowlineCatchmentDelta = lFlowlineCatchmentDelta
                    lFlowlineCatchmentDelta = lSimplifiedFlowlines.NumShapes - lSimplifiedCatchment.NumShapes
                    Logger.Dbg("CountsFlowlines " & lSimplifiedFlowlines.NumShapes _
                            & " Catchments " & lSimplifiedCatchment.NumShapes _
                            & " Delta " & lFlowlineCatchmentDelta _
                            & " Removed " & lBraidedFlowlinesCombinedCount)
                End If
                If lFlowlineCatchmentDelta <> lOldFlowlineCatchmentDelta Then
                    Logger.Status("Error: ************* Delta Change ***********************", True)
                End If
                SaveIntermediate(lSimplifiedCatchment, lSimplifiedFlowlines)
            End If
            lFlowlinesRecord += 1
        End While
        Logger.Status("======== " & lBraidedFlowlinesCombinedCount & " Braided Flowlines Combined ========", True)

        Logger.Status("Filling Missing Flowline CUMDRAINAG", True)
        FillMissingFlowlineCUMDRAINAG(lSimplifiedFlowlines, lSimplifiedCatchment)

        Logger.Status("Enforcing Minimum Catchment Size", True)
        'Merge short lines and/or small catchments
        For Each lOutletComID As Long In pOutletComIds
            EnforceMinimumSize(lSimplifiedFlowlines, lSimplifiedCatchment, aMinCatchmentKM2, aMinLengthKM, lOutletComID)
            Logger.Dbg("====AfterOutlet " & lOutletComID & " Flowlines " & lSimplifiedFlowlines.NumShapes & " Catchments " & lSimplifiedCatchment.NumShapes)
        Next
        Logger.Dbg("======== EnforceMinimumSizeDone OutletCount " & pOutletComIds.Count & " ShapeCount " & lSimplifiedFlowlines.NumShapes)

        CombineMissingOutletCatchments(lSimplifiedFlowlines, lSimplifiedCatchment, aMinCatchmentKM2, aMinLengthKM)
        Logger.Dbg("Combined Missing Outlet Catchments OutletCount " & pOutletComIds.Count & " ShapeCount " & lSimplifiedFlowlines.NumShapes & " " & lSimplifiedCatchment.NumShapes)

        lSimplifiedCatchment.StopEditingShapes()
        lSimplifiedCatchment.StopEditingTable()
        Dim lSortedCatchmentsFilename As String = SortCatchmentsToMatchFlowlines(lSimplifiedFlowlines, lSimplifiedCatchment)
        lSimplifiedCatchment.Close()

        lSimplifiedFlowlines.StopEditingShapes()
        lSimplifiedFlowlines.StopEditingTable()
        lSimplifiedFlowlines.Close()

        pOutletComIds.Clear()

        If IO.File.Exists(lSortedCatchmentsFilename) Then
            atcUtility.TryMoveShapefile(lSortedCatchmentsFilename, lSimplifiedCatchmentFileName)
        End If
        Return True
    End Function

    Private Sub CombineMissingOutletCatchments(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                               ByVal aCatchments As MapWinGIS.Shapefile, _
                                               ByVal aMinCatchmentKM2 As Double, _
                                               ByVal aMinLengthKM As Double)
        Logger.Dbg("CombineMissingOutletCatchments Count " & pOutletComIds.Count)
        Dim lCheckArea As Boolean = (aMinCatchmentKM2 > 0)
        Dim lCheckLength As Boolean = (aMinLengthKM > 0)
        Dim lMergedThese As New Generic.List(Of Long)
        Dim lOutletComID As Long
        For Each lOutletComID In pOutletComIds
            Logger.Dbg("Process " & lOutletComID)
            Dim lFlowlineIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lOutletComID)
            'Dim lOutletComIdRecordIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lOutletComID)
            If lFlowlineIndex > -1 Then
                If IsTooSmall(aFlowlines, lFlowlineIndex, aMinCatchmentKM2, aMinLengthKM, lCheckArea, lCheckLength, False) Then
                    'Dim lOutletCumArea As Double = aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lOutletComIdRecordIndex)
                    'If lOutletCumArea < aMinCatchmentKM2 Then 'TODO: is this an appropriate check?
                    'whats most likely downstream - assume flowline is upstream to downstream
                    Dim lFlowLineNumPoints As Integer = aFlowlines.Shape(lFlowlineIndex).numPoints
                    With aFlowlines.Shape(lFlowlineIndex).Point(lFlowLineNumPoints - 1)
                        Dim lNearestIndex As Integer = NearestNeighbor(.x, .y, lOutletComID, aCatchments)
                        Dim lMergeWithComid As Long = aCatchments.CellValue(pCatchmentComIdFieldIndex, lNearestIndex)
                        Logger.Dbg("MergeWith " & DumpComid(aFlowlines, lMergeWithComid))
                        Dim lMainFlowLineIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lMergeWithComid)
                        If CombineCatchments(aCatchments, lMergeWithComid, lOutletComID) Then
                            CombineFlowlines(aFlowlines, lMainFlowLineIndex, lFlowlineIndex, False, False)
                            lMergedThese.Add(lOutletComID)
                        Else
                            Logger.Status("Failed to merge missing outlet catchment " & lOutletComID & " into " & lMergeWithComid, True)
                        End If
                    End With
                Else
                    'LogMessage(aLog, "Big Enough " & lOutletComID & " Area " & lOutletCumArea)
                End If
            Else
                Logger.Status("Missing " & lOutletComID, True)
            End If
        Next
        For Each lOutletComID In lMergedThese
            pOutletComIds.Remove(lOutletComID)
        Next
        Logger.Status("CombineMissingOutletCatchments Complete: " & pOutletComIds.Count & " outlets remain", True)
    End Sub

    Private Function NearestNeighbor(ByVal aX As Double, ByVal aY As Double, _
                                     ByVal aComId As Long, ByVal aPolygonShapeFile As MapWinGIS.Shapefile) As Integer
        'given a point and a polygon shapefile, find index of closest neighboring polygon to this point

        'this is accomplished by finding the point on the containing polygon's boarder closest to the input point,
        'and then using the selection functionality to see which other polygon coincides with that point.
        Dim lNearestNeighbor As Integer = -1
        Select Case aPolygonShapeFile.NumShapes
            Case 0 : lNearestNeighbor = -1
            Case 1 : lNearestNeighbor = 0
            Case Else
                Dim lPt As New MapWinGIS.Point
                lPt.x = aX
                lPt.y = aY
                Dim lPolygonFeatureIndex As Integer = FindRecord(aPolygonShapeFile, pCatchmentComIdFieldIndex, aComId)

                Dim lExtents As New MapWinGIS.Extents
                Dim lDelta As Double = 200
                Dim lCount As Integer = -1

                Dim lNearestDistance As Double = 1.0E+30
                While lNearestNeighbor = -1
                    lExtents.SetBounds(lPt.x - lDelta, lPt.y - lDelta, 0, lPt.x + lDelta, lPt.y + lDelta, 0)
                    Dim lIntersectingPolygonIndices As Object = Nothing
                    If aPolygonShapeFile.SelectShapes(lExtents, 0.0, MapWinGIS.SelectMode.INTERSECTION, lIntersectingPolygonIndices) Then
                        'could use MapWinGeoProc.Selection.SelectPolygonsWithPolygon instead?
                        lCount = lIntersectingPolygonIndices.GetUpperBound(0) + 1
                        If lCount = 1 Then
                            lDelta *= 2.0
                        Else 'found more than one intersecting polygons
                            For lIndex As Integer = 0 To lCount - 1
                                If lIntersectingPolygonIndices(lIndex) <> lPolygonFeatureIndex Then
                                    'this polygon is not the one the input point is in, so it must be neighbor
                                    If lCount = 2 Then 'nearest for sure
                                        lNearestNeighbor = lIntersectingPolygonIndices(lIndex)
                                    Else 'check for nearest
                                        Dim lContainingShape As MapWinGIS.Shape = aPolygonShapeFile.Shape(lIntersectingPolygonIndices(lIndex))
                                        Dim lNearestPoint As New MapWinGIS.Point
                                        Dim lLocation As Integer
                                        Dim lDistance As Double
                                        If MapWinGeoProc.Utils.FindNearestPointAndLocOld(lPt, lContainingShape, lNearestPoint, lLocation, lDistance) Then
                                            If lDistance < lNearestDistance Then
                                                lNearestDistance = lDistance
                                                lNearestNeighbor = lIntersectingPolygonIndices(lIndex)
                                            End If
                                        Else
                                            Logger.Status("NoNearestPointAt " & lIndex, True)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Else 'nothing to select
                        lDelta *= 2
                    End If
                End While
        End Select
        Return lNearestNeighbor
    End Function

    Private Function FindUpstreamComIDs(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                        ByVal aOutletComId As Long) As Generic.List(Of Long)
        Dim lFlowLinesUpstreamComIds As New Generic.List(Of Long)
        Dim lFlowLinesUpstream As Generic.List(Of Integer) = FindRecords(aFlowlines, pFlowlinesDownstreamComIdFieldIndex, aOutletComId)
        Dim lComId As Long
        For Each lFlowlineIndex As Integer In lFlowLinesUpstream
            Try
                lComId = -1
                If UpdateValueIfNotNull(lComId, aFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFlowlineIndex)) Then
                    If lComId = aOutletComId Then
                        Logger.Status("Error: ComIDProblem: upstream of itself: " & lComId, True)
                        Return lFlowLinesUpstreamComIds
                    Else
                        Logger.Dbg("Upstream " & DumpComid(aFlowlines, lComId))
                        lFlowLinesUpstreamComIds.Add(lComId)
                    End If
                Else
                    Logger.Status("Error: NoComIdFor " & lFlowlineIndex, True)
                End If
            Catch lEx As Exception
                Logger.Status("Error: ProblemUpstream index " & lFlowlineIndex & ": " & lEx.Message, True)
            End Try
        Next
        Return lFlowLinesUpstreamComIds
    End Function

    Public Sub FillMissingFlowlineCUMDRAINAG(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                             ByVal aCatchments As MapWinGIS.Shapefile, _
                                             Optional ByVal aIndex As Integer = -1)
        Dim lFirstIndex As Integer
        Dim lLastIndex As Integer
        If aIndex = -1 Then
            lFirstIndex = 0
            lLastIndex = aFlowlines.NumShapes - 1
        Else
            lFirstIndex = aIndex
            lLastIndex = lFirstIndex
        End If
        For lFlowlineIndex As Integer = lFirstIndex To lLastIndex
            Dim lContribArea As Double = -1
            UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lFlowlineIndex))
            If lContribArea <= 0 Then
                lContribArea = 0
                Dim lComId As Long = 0
                If Not UpdateValueIfNotNull(lComId, aFlowlines.CellValue(pFlowlinesComIdFieldIndex, lFlowlineIndex)) Then
                    Logger.Status("Error: COMID and CUMDRAINAG missing at record " & lFlowlineIndex & " of " & aFlowlines.Filename, True)
                Else
                    Logger.Dbg("CUMDRAINAG missing for " & lComId & ", attempting to compute value")
                    'Find local drainage area from area of associated catchment
                    Dim lCatchmentIndex As Integer = FindRecord(aCatchments, pCatchmentComIdFieldIndex, lComId)
                    If lCatchmentIndex < 0 Then
                        Logger.Status("Could not find catchment associated with this outlet: " & lComId, True)
                    Else
                        If UpdateValueIfNotNull(lContribArea, aCatchments.CellValue(pCatchmentAreaIndex, lCatchmentIndex)) Then
                            aFlowlines.EditCellValue(pFlowlinesLocDrainAreaIndex, lFlowlineIndex, lContribArea)
                            Logger.Dbg("Catchment Area = " & lContribArea)
                        Else
                            Logger.Status("Catchment Area not available for " & lComId, True)
                        End If
                    End If
                End If

                Dim lUpstreamComIds As Generic.List(Of Long) = FindUpstreamComIDs(aFlowlines, lComId)
                If lUpstreamComIds.Count = 0 Then
                    Logger.Dbg("FillMissingFlowlineCUMDRAINAG: No Upstream flowlines to search from " & lComId)
                    'lThisChannelCumLen = Math.Max(0, lThisChannelCumLen)
                Else
                    Logger.Dbg("FillMissingFlowlineCUMDRAINAG: Count Upstream = " & lUpstreamComIds.Count)
                    For Each lUpComId As Long In lUpstreamComIds
                        Dim lUpIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lUpComId)
                        FillMissingFlowlineCUMDRAINAG(aFlowlines, aCatchments, lUpIndex)
                        Dim lUpContribArea As Double = 0
                        'Dim lUpCumLen = 0
                        UpdateValueIfNotNull(lUpContribArea, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lUpIndex))
                        lContribArea += lUpContribArea
                    Next
                    'lThisChannelCumLen = Math.Max(lUpCumLen, lThisChannelCumLen)
                End If

                Logger.Dbg("FillMissingFlowlineCUMDRAINAG: Computed CumDrainArea = " & lContribArea & " at " & lComId) '& " CumLen=" & lThisChannelCumLen)
                aFlowlines.EditCellValue(pFlowlinesCumDrainAreaIndex, lFlowlineIndex, lContribArea)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Find the flowline from the list aFlowLineComIds with the largest contributing area
    ''' </summary>
    ''' <param name="aFlowlines">flowlines shapefile</param>
    ''' <param name="aFlowLineComIds">list of ComIDs to search for the main one</param>
    ''' <param name="aTotalContribArea">on return this is set to the total contributing area from all upstream channels</param>
    ''' <param name="aMainChannelCumLen">on return this is set to the cumulative length of the main channel</param>
    ''' <param name="aMainChannelIndex">on return this is set to the shape index of the main channel in aFlowlines</param>
    ''' <returns>ComID of flowline from aFlowLinesUpstreamComIds with the largest contributing drainage area</returns>
    ''' <remarks></remarks>
    Private Function FindMainChannel(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                     ByVal aFlowLineComIds As Generic.List(Of Long), _
                                     ByRef aTotalContribArea As Double, _
                                     ByRef aMainChannelCumLen As Double, _
                                     ByRef aMainChannelIndex As Integer) As Long
        Dim lMainLocalChannelComId As Long = 0
        Dim lFlowlineIndex As Integer
        Dim lMainChannelContribArea As Double = 0
        For Each lComId As Long In aFlowLineComIds
            Try
                lFlowlineIndex = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lComId)
                Dim lContribArea As Double = -2
                UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lFlowlineIndex))

                Dim lThisChannelCumLen As Double = -2
                UpdateValueIfNotNull(lThisChannelCumLen, aFlowlines.CellValue(pFlowlinesCumLenFieldIndex, lFlowlineIndex))
                If lContribArea <= 0 OrElse lThisChannelCumLen <= 0 Then 'Look upstream for valid values
                    Logger.Status("Warning: FindMainChannel " & lComId & " CumDrainArea=" & lContribArea & " CumLen=" & lThisChannelCumLen & " (both should be > 0)", True)
                End If

                'Moved most of this block into FillMissingFlowlineCUMDRAINAG
                'If lContribArea <= 0 OrElse lThisChannelCumLen <= 0 Then 'Look upstream for valid values
                '    LogMessage(aLog, "FindMainChannel " & lComId & " CumDrainArea=" & lContribArea & " CumLen=" & lThisChannelCumLen & " (both should be > 0)")
                '    Dim lUpstreamComIds As Generic.List(Of Long) = FindUpstreamComIDs(aLog, aFlowlines, lComId)
                '    If lUpstreamComIds.Count = 0 Then
                '        LogMessage(aLog, "FindMainChannel: No Upstream flowlines to search")
                '        lContribArea = Math.Max(0, lContribArea)
                '        lThisChannelCumLen = Math.Max(0, lThisChannelCumLen)
                '    Else
                '        LogMessage(aLog, "FindMainChannel: Count Upstream = " & lUpstreamComIds.Count)
                '        Dim lUpContribArea As Double = 0
                '        Dim lUpCumLen = 0
                '        Dim lUpstreamChannelIndex As Integer 'we don't need to know this number now, but need a variable to send below
                '        FindMainChannel(aLog, aFlowlines, lUpstreamComIds, lUpContribArea, lUpCumLen, lUpstreamChannelIndex)
                '        lContribArea = Math.Max(lUpContribArea, lContribArea)
                '        lThisChannelCumLen = Math.Max(lUpCumLen, lThisChannelCumLen)
                '        LogMessage(aLog, "FindMainChannel: looked upstream and found CumDrainArea=" & lContribArea & " CumLen=" & lThisChannelCumLen)
                '    End If
                'End If

                If lContribArea > 0 AndAlso lThisChannelCumLen > 0 Then
                    aTotalContribArea += lContribArea
                    'Choose upstream branch with largest contributing area as main channel
                    If lContribArea > lMainChannelContribArea Then
                        lMainChannelContribArea = lContribArea
                        UpdateValueIfNotNull(aMainChannelCumLen, lThisChannelCumLen)
                        lMainLocalChannelComId = lComId
                        aMainChannelIndex = lFlowlineIndex
                    End If
                End If
            Catch lEx As Exception
                Logger.Status("Error: ProblemUpstream " & lEx.Message, True)
            End Try
        Next
        Return lMainLocalChannelComId
    End Function

    ''' <summary>
    ''' Combine flowlines and catchments upstream of given outlet if catchment smaller than aMinCatchmentKM2 or flowline shorter than aMinLengthKM
    ''' </summary>
    ''' <param name="aFlowlines">flowlines shapefile</param>
    ''' <param name="aCatchments">catchments shapefile</param>
    ''' <param name="aMinCatchmentKM2">Minimum area for a catchment (square kilometers)</param>
    ''' <param name="aMinLengthKM">Minimum length for a flowline (kilometers)</param>
    ''' <param name="aOutletComId"></param>
    ''' <remarks></remarks>
    Private Sub EnforceMinimumSize(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                   ByVal aCatchments As MapWinGIS.Shapefile, _
                                   ByVal aMinCatchmentKM2 As Double, _
                                   ByVal aMinLengthKM As Double, _
                                   ByVal aOutletComId As Long)
        Dim lCheckArea As Boolean = (aMinCatchmentKM2 > 0)
        Dim lCheckLength As Boolean = (aMinLengthKM > 0)
        Logger.Dbg("EnforceMinimumSizeEntry  " & DumpComid(aFlowlines, aOutletComId) _
                       & " Flowlines: " & aFlowlines.NumShapes _
                       & " Catchments: " & aCatchments.NumShapes)

        Dim lComIdUp As Long
        Dim lContribArea As Double
        Dim lLength As Double
        Dim lFlowlineIndex As Integer
        Dim lUpstreamComIds As Generic.List(Of Long) = FindUpstreamComIDs(aFlowlines, aOutletComId)

        Logger.Dbg("Count Upstream = " & lUpstreamComIds.Count)
        For Each lComIdUp In lUpstreamComIds
            Logger.Dbg("Upstream EnforceMinimumSize " & DumpComid(aFlowlines, lComIdUp))
            EnforceMinimumSize(aFlowlines, aCatchments, aMinCatchmentKM2, aMinLengthKM, lComIdUp)
        Next

        Dim lMainUpstreamChannelCumLen As Double = 0.0
        Dim lTotalContribArea As Double = 0.0
        Dim lMainUpstreamChannelComId As Long = FindMainChannel(aFlowlines, lUpstreamComIds, lTotalContribArea, lMainUpstreamChannelCumLen, 0)

        Dim lOutletFlowlineIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, aOutletComId)
        If lOutletFlowlineIndex = -1 Then
            Logger.Status("OutletNotInLayer " & DumpComid(aFlowlines, aOutletComId), True)
            Return
        End If

        lLength = 0
        UpdateValueIfNotNull(lLength, aFlowlines.CellValue(pFlowlinesCumLenFieldIndex, lOutletFlowlineIndex))
        If lLength = 0 Then 'Cumulative length not yet set, so set it to sum of main channel cumulative + outlet length
            UpdateValueIfNotNull(lLength, aFlowlines.CellValue(pFlowlinesLengthFieldIndex, lOutletFlowlineIndex))
            aFlowlines.EditCellValue(pFlowlinesCumLenFieldIndex, lOutletFlowlineIndex, lLength + lMainUpstreamChannelCumLen)
            '    LogMessage(aLog,"Set CumLen of " & aOutletComId & " " & lLength & " + " & lMainUpstreamChannelCumLen & " = " & aFlowlines.CellValue(pFlowlinesCumLenFieldIndex, lOutletFlowlineIndex))
            'Else
            '    LogMessage(aLog,"Did not Set CumLen of aOutletComId = " & lLength)
        End If

        lContribArea = 0
        UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(pFlowlinesLocDrainAreaIndex, lOutletFlowlineIndex))
        If lContribArea = 0 Then 'Local drainage area not yet set, so set it to difference of outlet cum - main channel cum
            If lContribArea <= 0 Then 'Find local drainage area from FlowlinesCumDrainArea
                If Not UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lOutletFlowlineIndex)) Then
                    Logger.Status("Could not set local drainage area, no value for FlowlinesCumDrainArea", True)
                End If
            End If
            If lContribArea >= lTotalContribArea Then
                aFlowlines.EditCellValue(pFlowlinesLocDrainAreaIndex, lOutletFlowlineIndex, lContribArea - lTotalContribArea)
            Else
                Logger.Status("Could not set local drainage area, local+upstream < upstream, (" & lContribArea & " < " & lTotalContribArea & ")", True)
            End If
        End If

        Dim lCumDrainAreaDown As Double = 0.0
        UpdateValueIfNotNull(lCumDrainAreaDown, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lOutletFlowlineIndex))

        If lMainUpstreamChannelComId > 0 Then
            Logger.Dbg("MainUpstream from " & aOutletComId & " is " & DumpComid(aFlowlines, lMainUpstreamChannelComId))
        ElseIf lUpstreamComIds.Count > 0 Then
            Logger.Status("Error: NoMainUpstream from " & aOutletComId, True)
        Else
            Logger.Dbg("      TopOfStream at " & aOutletComId)
        End If

        For Each lComIdUp In lUpstreamComIds
            'Need to update each time through, index may change while merging
            lOutletFlowlineIndex = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, aOutletComId)
            If lOutletFlowlineIndex = -1 Then
                Logger.Status("Error: OutletLostFromLayer " & DumpComid(aFlowlines, aOutletComId), True)
                Return
            Else
                'Find contributing area and/or length of upstream segment
                lFlowlineIndex = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lComIdUp)
                If lFlowlineIndex < 0 Then
                    Logger.Dbg("UpstreamFlowlineNotFound: " & lComIdUp)
                Else
                    If IsTooSmall(aFlowlines, lFlowlineIndex, aMinCatchmentKM2, aMinLengthKM, lCheckArea, lCheckLength, False) Then
                        'this upstream segment is too small/short, need to merge it with something
                        Dim lMerge As Boolean = True
                        Dim lKeepBothFlowlines As Boolean = False
                        Dim lCumulativeBigEnough As Boolean = Not IsTooSmall(aFlowlines, lFlowlineIndex, aMinCatchmentKM2, aMinLengthKM, lCheckArea, lCheckLength, True)

                        If lCumulativeBigEnough Then
                            'Even though this segment is small, it has significant upstream, so merge it upstream instead of discarding
                            Dim lUpUpstream As Generic.List(Of Long) = FindUpstreamComIDs(aFlowlines, lComIdUp)
                            Dim lCountBigUpstream As Integer = 0
                            For Each lComIdUpUp As Long In lUpUpstream
                                Dim lRecordUpUp As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, lComIdUpUp)
                                If Not IsTooSmall(aFlowlines, lRecordUpUp, aMinCatchmentKM2, aMinLengthKM, lCheckArea, lCheckLength, False) Then
                                    lCountBigUpstream += 1
                                End If
                            Next
                            If lCountBigUpstream = 1 Then 'can merge upstream if there is only one large contributor
                                If MergeUpstream(aFlowlines, aCatchments, lUpUpstream, lComIdUp) Then
                                    lMerge = False 'merge upstream succeeded, so do not merge downstream below
                                End If
                            End If
                        End If

                        If lMerge Then
                            lOutletFlowlineIndex = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, aOutletComId) 'might have gotten off during MergeUpstream

                            If lComIdUp = lMainUpstreamChannelComId Then 'This upstream is on the main channel, probably want to keep the stream
                                If lCumDrainAreaDown > (aMinCatchmentKM2 / 2) Then 'only keep large enough segments even if on main channel 
                                    lKeepBothFlowlines = True
                                Else
                                    Logger.Dbg("Discarding main channel small upstream segment " & lCumDrainAreaDown & " < " & (aMinCatchmentKM2 / 2) & " " & DumpComid(aFlowlines, lMainUpstreamChannelComId))
                                End If
                            End If

                            Logger.Dbg("FlowlineMergeDownstream Keep=" & lKeepBothFlowlines & " " & DumpComid(aFlowlines, aOutletComId) & _
                                                           " with upstream " & DumpComid(aFlowlines, lComIdUp))
                            Try
                                If CombineCatchments(aCatchments, aOutletComId, lComIdUp) Then
                                    CombineFlowlines(aFlowlines, lOutletFlowlineIndex, lFlowlineIndex, lKeepBothFlowlines, lCumulativeBigEnough AndAlso g_KeepConnectingRemovedFlowLines)
                                Else
                                    Logger.Status("Failed to merge missing outlet catchment " & lComIdUp & " into " & aOutletComId, True)
                                End If
                                If lKeepBothFlowlines Then
                                    Logger.Dbg("AfterCombineWithMerge " & DumpComid(aFlowlines, aOutletComId))
                                Else
                                    Logger.Dbg("AfterCombineNoMerge " & DumpComid(aFlowlines, aOutletComId))
                                End If

                            Catch e As Exception
                                Logger.Status("Error, did not combine: " & e.Message, True)
                            End Try
                        End If
                        SaveIntermediate(aCatchments, aFlowlines)
                    End If
                End If
            End If
        Next

        'If this is an outlet, merge upstream if not large enough
        If pOutletComIds.Contains(aOutletComId) Then
            lFlowlineIndex = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, aOutletComId)
            lContribArea = -1
            If lCheckArea Then UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lFlowlineIndex))
            lLength = -1
            If lCheckLength Then UpdateValueIfNotNull(lLength, aFlowlines.CellValue(pFlowlinesLengthFieldIndex, lFlowlineIndex))
            If (lCheckArea AndAlso lContribArea < aMinCatchmentKM2) OrElse (lCheckLength AndAlso lLength < aMinLengthKM) Then
                If MergeUpstream(aFlowlines, aCatchments, FindUpstreamComIDs(aFlowlines, aOutletComId), aOutletComId) Then
                    Logger.Dbg("Merged small outlet upstream " & aOutletComId)
                Else
                    Logger.Status("Failed to merge small outlet upstream " & aOutletComId, True)
                End If
            End If
        End If

        Logger.Dbg("AllDone " & DumpComid(aFlowlines, aOutletComId) & " " & aFlowlines.NumShapes & " " & aCatchments.NumShapes)

    End Sub

    Private Function IsTooSmall(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                ByVal aFlowlineIndex As Integer, _
                                ByVal aMinCatchmentKM2 As Double, _
                                ByVal aMinLengthKM As Double, _
                                ByVal aCheckArea As Boolean, _
                                ByVal aCheckLength As Boolean, _
                                ByVal aCumulative As Boolean) As Boolean
        Dim lField As Integer
        Dim lLocCum As String

        If aCumulative Then
            lLocCum = "Cumulative"
        Else
            lLocCum = "Local"
        End If

        If aCheckArea Then
            Dim lContribArea As Double = -1
            If aCumulative Then
                lField = pFlowlinesCumDrainAreaIndex
            Else
                lField = pFlowlinesLocDrainAreaIndex
            End If
            UpdateValueIfNotNull(lContribArea, aFlowlines.CellValue(lField, aFlowlineIndex))
            If lContribArea < aMinCatchmentKM2 Then
                Logger.Dbg(lLocCum & " area too small (" & lContribArea & " < " & aMinCatchmentKM2 & ")")
                Return True
            End If
        End If

        If aCheckLength Then
            Dim lLength As Double = -1
            If aCumulative Then
                lField = pFlowlinesCumLenFieldIndex
            Else
                lField = pFlowlinesLengthFieldIndex
            End If
            UpdateValueIfNotNull(lLength, aFlowlines.CellValue(lField, aFlowlineIndex))
            If lLength < aMinLengthKM Then
                Logger.Dbg(lLocCum & " length too short (" & lLength & " < " & aMinLengthKM & ")")
                Return True
            End If
        End If
        Return False
    End Function

    Private Function MergeUpstream(ByVal aFlowlines As MapWinGIS.Shapefile, _
                                   ByVal aCatchments As MapWinGIS.Shapefile, _
                                   ByVal aUpstreamComIds As Generic.List(Of Long), _
                                   ByVal aOutletComId As Long) As Boolean
        Dim lSuccess As Boolean = False
        Try
            Logger.Dbg("Merging upstream of " & DumpComid(aFlowlines, aOutletComId))
            Dim lMainChannelIndex As Integer
            If aUpstreamComIds.Count < 1 Then
                Logger.Dbg("Nothing upstream to merge with")
            Else
                Dim lMainUpstreamChannelComId As Long = FindMainChannel(aFlowlines, aUpstreamComIds, 0, 0, lMainChannelIndex)
                If lMainUpstreamChannelComId > 0 Then
                    Logger.Dbg("Merging upstream with " & DumpComid(aFlowlines, lMainUpstreamChannelComId))
                    Dim lOutletFlowlineIndex As Integer = FindRecord(aFlowlines, pFlowlinesComIdFieldIndex, aOutletComId)
                    lSuccess = CombineCatchments(aCatchments, aOutletComId, lMainUpstreamChannelComId)
                    If lSuccess Then
                        lSuccess = CombineFlowlines(aFlowlines, lOutletFlowlineIndex, lMainChannelIndex, True, False)
                        If Not lSuccess Then
                            Logger.Msg("Error: merged catchments but not flowlines up " & lMainUpstreamChannelComId & "-" & aOutletComId)
                        End If
                    Else
                        Logger.Msg("Error: Could not merge flowlines up " & lMainUpstreamChannelComId & "-" & aOutletComId)
                    End If
                Else
                    Logger.Dbg("No Main Channel upstream to merge with")
                End If
            End If
        Catch e As Exception
            Logger.Status("Error, did not combine: " & e.Message, True)
        End Try
        Return lSuccess
    End Function

    Private Sub SaveIntermediate(ByVal aCatchments As MapWinGIS.Shapefile, _
                                 ByVal aFlowlines As MapWinGIS.Shapefile)
        Static lCount As Integer
        Dim lResult As Integer
        lCount += 1
        Math.DivRem(lCount, 500, lResult) 'every 500 write intermediate shapes
        If lResult = 0 Then
            Logger.Dbg("SaveIntermediate " & lCount & " " & aCatchments.NumShapes)
            aFlowlines.StopEditingShapes()
            aFlowlines.StopEditingTable()
            aCatchments.StopEditingShapes()
            aCatchments.StopEditingTable()
            aFlowlines.StartEditingShapes()
            aFlowlines.StartEditingTable()
            aCatchments.StartEditingShapes()
            aCatchments.StartEditingTable()
        End If
    End Sub

    ''' <summary>
    ''' Generate a .fig file from NHDPlus flowlines for use in SWAT.
    ''' </summary>
    ''' <param name="aFlowlinesFileName"></param>
    ''' <param name="aFigFilename"></param>
    ''' <returns>collection of ComId(key) to SubBasinId(value) mapping</returns>
    ''' <remarks></remarks>
    Friend Function CreateSWATFig(ByVal aFlowlinesFileName As String, ByVal aFigFilename As String) As atcUtility.atcCollection
        Dim lSwatSubbasinFieldname As String = "SWATSUB"
        Logger.Status("Create SWAT .fig", True)
        CreateSWATFig = New atcUtility.atcCollection

        Dim lFlowlinesShapefile As New MapWinGIS.Shapefile()
        If Not lFlowlinesShapefile.Open(aFlowlinesFileName, Nothing) Then
            Logger.Dbg("Unable to open '" & aFlowlinesFileName & "'")
            Exit Function
        End If

        If pFlowlinesComIdFieldIndex = -1 Then
            Logger.Dbg("COMID not found in '" & aFlowlinesFileName & "'")
            Logger.Status("")
            Exit Function
        ElseIf pFlowlinesDownstreamComIdFieldIndex = -1 Then
            Logger.Dbg("TOCOMID not found in '" & aFlowlinesFileName & "'")
            Logger.Status("")
            Exit Function
        End If

        lFlowlinesShapefile.StartEditingTable()
        Dim lFlowlinesSubBasinFieldIndex As Integer = FieldIndex(lFlowlinesShapefile, lSwatSubbasinFieldname)
        If lFlowlinesSubBasinFieldIndex = -1 Then
            lFlowlinesSubBasinFieldIndex = lFlowlinesShapefile.NumFields
            Dim lSubBasinField As New MapWinGIS.Field
            With lSubBasinField
                .Name = lSwatSubbasinFieldname
                .Type = MapWinGIS.FieldType.INTEGER_FIELD
            End With
            lFlowlinesShapefile.EditInsertField(lSubBasinField, lFlowlinesSubBasinFieldIndex)
        End If

        Dim fig As New IO.StreamWriter(aFigFilename)

        Dim substack As New Generic.Stack(Of Integer)
        Dim subIDs As New Generic.List(Of Integer)
        Dim Hyd_Stor_Num As Integer = 0
        Dim Res_Num As Integer = 0
        Dim InFlow_Num1 As Integer = 0
        Dim InFlow_Num2 As Integer = 0
        Dim InFlow_ID As Integer = 0
        Dim UpstreamCount, UpstreamFinishedCount As Integer

        'Write subbasins
        Dim lFlowlineIndex As Integer
        For lFlowlineIndex = 0 To lFlowlinesShapefile.NumShapes - 1
            Dim lComId As Long = lFlowlinesShapefile.CellValue(pFlowlinesComIdFieldIndex, lFlowlineIndex)
            If lComId = 0 Then
                'TODO: adding a subID=0 here is not enough to make the code below work, 
                'we currently depend on cosmetic flowlines being at end of shapefile so there are no skipped indexes
                subIDs.Add(0)
                Logger.Dbg("CreateSWATFig: Skipping cosmetic flowline at index " & lFlowlineIndex)
            Else
                Hyd_Stor_Num += 1
                fig.Write("subbasin       1{0,6:G6}{0,6:G6}                              Subbasin: {0:G} ComId: {1:G}" & ControlChars.Lf & _
                          "          {0,5:D5}0000.sub" & ControlChars.Lf, _
                          Hyd_Stor_Num, lComId)
                InFlow_ID = lFlowlinesShapefile.CellValue(pFlowlinesDownstreamComIdFieldIndex, lFlowlineIndex).ToString()
                lFlowlinesShapefile.EditCellValue(lFlowlinesSubBasinFieldIndex, lFlowlineIndex, Hyd_Stor_Num)
                If FindRecord(lFlowlinesShapefile, pFlowlinesComIdFieldIndex, InFlow_ID) < 0 Then
                    substack.Push(lFlowlineIndex)
                End If
                subIDs.Add(-1)
                CreateSWATFig.Add(lComId.ToString, Hyd_Stor_Num)
            End If
        Next

        'Write the rest
        Dim curridx As Integer
        'Dim currUS1, currUS2 As String
        'Dim currUS1idx, currUS2idx, currUS1ID, currUS2ID As Integer
        While substack.Count > 0
            curridx = substack.Pop()
            Dim currComID As Long = lFlowlinesShapefile.CellValue(pFlowlinesComIdFieldIndex, curridx)
            Dim lUpstreamIndexes As Generic.List(Of Integer) = FindRecords(lFlowlinesShapefile, pFlowlinesDownstreamComIdFieldIndex, currComID)

            UpstreamCount = lUpstreamIndexes.Count
            If UpstreamCount = 0 Then 'then we're on an outer reach.
                If subIDs(curridx) = -1 Then 'then it hasn't been added yet. add a route
                    Hyd_Stor_Num += 1
                    InFlow_Num1 = curridx + 1
                    fig.Write("route          2{0,6:G6}{1,6:G6}{2,6:G6}" + ControlChars.Lf + "          {1,5:D5}0000.rte{1,5:D5}0000.swq" + ControlChars.Lf, Hyd_Stor_Num, curridx + 1, InFlow_Num1)
                    subIDs(curridx) = Hyd_Stor_Num

                    'TODO: handle reservoirs
                    'If lFlowlinesShapefile.CellValue(ReservoirFieldNum, curridx).ToString() = "1" Then 'it's a reservoir
                    '    Hyd_Stor_Num += 1
                    '    Res_Num += 1
                    '    InFlow_Num1 = Hyd_Stor_Num - 1
                    '    InFlow_ID = curridx + 1
                    '    fig.Write("routres        3{0,6:G6}{1,6:G6}{2,6:G6}{3,6:G6}" + ControlChars.Lf + "          {3,5:D5}0000.res{3,5:D5}0000.lwq" + ControlChars.Lf, Hyd_Stor_Num, Res_Num, InFlow_Num1, InFlow_ID)
                    '    subIDs(curridx) = Hyd_Stor_Num
                    'End If
                End If

            Else 'we're on a middle or final reach
                'UpstreamCount = 0
                UpstreamFinishedCount = 0

                For Each lUpstreamIndex As Integer In lUpstreamIndexes
                    Dim lUpstreamID As Integer = subIDs(lUpstreamIndex)
                    If lUpstreamID <> -1 Then
                        UpstreamFinishedCount += 1
                    End If
                Next

                If UpstreamCount = UpstreamFinishedCount Then 'all upstreams finished
                    InFlow_Num2 = curridx + 1
                    For Each lUpstreamIndex As Integer In lUpstreamIndexes
                        Dim lUpstreamID As Integer = subIDs(lUpstreamIndex)
                        Hyd_Stor_Num += 1
                        InFlow_Num1 = lUpstreamID
                        fig.Write("add            5{0,6:G6}{1,6:G6}{2,6:G6}" + ControlChars.Lf, Hyd_Stor_Num, lUpstreamID, InFlow_Num2)
                        InFlow_Num2 = Hyd_Stor_Num
                    Next

                    'After summing, create the route and possibly reservoir
                    Hyd_Stor_Num += 1
                    InFlow_Num1 = Hyd_Stor_Num - 1
                    fig.Write("route          2{0,6:G6}{1,6:G6}{2,6:G6}" + ControlChars.Lf + "          {1,5:D5}0000.rte{1,5:D5}0000.swq" + ControlChars.Lf, Hyd_Stor_Num, curridx + 1, InFlow_Num1)
                    subIDs(curridx) = Hyd_Stor_Num

                    'TODO: handle reservoirs
                    'If sf.CellValue(ReservoirFieldNum, curridx).ToString() = "1" Then
                    '    Hyd_Stor_Num += 1
                    '    Res_Num += 1
                    '    InFlow_Num1 = Hyd_Stor_Num - 1
                    '    InFlow_ID = curridx + 1
                    '    fig.Write("routres        3{0,6:G6}{1,6:G6}{2,6:G6}{3,6:G6}" + ControlChars.Lf + "          {3,5:D5}0000.res{3,5:D5}0000.lwq" + ControlChars.Lf, Hyd_Stor_Num, Res_Num, InFlow_Num1, InFlow_ID)
                    '    subIDs(curridx) = Hyd_Stor_Num
                    'End If

                Else 'There are upstream items that need to still be processed before this one
                    substack.Push(curridx)
                    For Each lUpstreamIndex As Integer In lUpstreamIndexes
                        substack.Push(lUpstreamIndex)
                    Next
                End If
            End If
        End While

        'Write out the saveconc and finish commands
        Dim SaveFile_Num As Integer = 1
        Dim Print_Freq As Integer = 0 '0 for daily, 1 for hourly
        fig.Write("saveconc      14{0,6:G6}{1,6:G6}{2,6:G6}" + ControlChars.Lf + "          watout.dat" + ControlChars.Lf, Hyd_Stor_Num, SaveFile_Num, Print_Freq)
        fig.Write("finish         0" + ControlChars.Lf)

        fig.Close()
        lFlowlinesShapefile.StopEditingTable()
        lFlowlinesShapefile.Close()
        Logger.Status("")
    End Function

    Friend Sub CalculateCatchmentProperty(ByVal aCatchmentFileName As String, ByVal aDemGridFilename As String)
        Dim lDemGrid As New MapWinGIS.Grid
        Dim lCatchments As New MapWinGIS.Shapefile
        If lDemGrid.Open(aDemGridFilename) AndAlso _
           lCatchments.Open(aCatchmentFileName) Then
            Dim lSB As New System.Text.StringBuilder
            Dim lArcSwatShapeFile As Boolean = False
            Dim lElevFieldIndex As Integer = -1
            Dim lLatFieldIndex As Integer = -1
            Dim lLngFieldIndex As Integer = -1
            If pCatchmentComIdFieldIndex = -1 Then
                lSB.AppendLine("Index" & vbTab & "SubBasin" & vbTab & "Lat" & vbTab & "Long" & vbTab & "Elev")
                pCatchmentComIdFieldIndex = FieldIndex(lCatchments, "Subbasin")
                lElevFieldIndex = FieldIndex(lCatchments, "Elev")
                lLatFieldIndex = FieldIndex(lCatchments, "Lat")
                lLngFieldIndex = FieldIndex(lCatchments, "Long_")
                lArcSwatShapeFile = True
            Else
                lSB.AppendLine("Index" & vbTab & "ComID" & vbTab & "Lat" & vbTab & "Long" & vbTab & "Elev")
            End If
            Dim lCatchmentProjection As String = lCatchments.Projection
            Dim lElevationProjection As String = lDemGrid.Header.Projection
            Dim lSameProjection As Boolean = D4EMDataManager.SpatialOperations.SameProjection(lCatchmentProjection, lElevationProjection)
            For lShapeIndex As Integer = 0 To lCatchments.NumShapes - 1
                If lArcSwatShapeFile Then
                    lSB.AppendLine(lShapeIndex & vbTab & _
                                   lCatchments.CellValue(pCatchmentComIdFieldIndex, lShapeIndex) & vbTab & _
                                   lCatchments.CellValue(lLatFieldIndex, lShapeIndex) & vbTab & _
                                   lCatchments.CellValue(lLngFieldIndex, lShapeIndex) & vbTab & _
                                   lCatchments.CellValue(lElevFieldIndex, lShapeIndex))
                Else
                    Dim lCatchmentShape As MapWinGIS.Shape = lCatchments.Shape(lShapeIndex)
                    Dim lCentroid As MapWinGIS.Point = MapWinGeoProc.Statistics.Centroid(lCatchmentShape)
                    Dim lX As Double = lCentroid.x
                    Dim lY As Double = lCentroid.y
                    With lCatchmentShape.Extents
                        If lX < .xMin OrElse lX > .xMax OrElse _
                           lY < .yMin OrElse lY > .yMax Then
                            'MapWinGeoProc.Statistics.Centroid failed, probably a multi-polygon
                            'Use elevation at middle of bounding box
                            lX = (.xMax + .xMin) / 2
                            lY = (.yMax + .yMin) / 2
                        End If
                    End With
                    If Not lSameProjection Then
                        MapWinGeoProc.SpatialReference.ProjectPoint(lX, lY, lCatchmentProjection, lElevationProjection)
                    End If
                    'TODO - update to use average elevation for whole shape
                    Dim lRow As Integer
                    Dim lCol As Integer
                    lDemGrid.ProjToCell(lX, lY, lCol, lRow)
                    If lRow < 0 Then lRow = 0
                    If lCol < 0 Then lCol = 0
                    lRow = Math.Min(lRow, lDemGrid.Header.NumberRows - 1)
                    lCol = Math.Min(lCol, lDemGrid.Header.NumberCols - 1)
                    Dim lElevation As Double = lDemGrid.Value(lCol, lRow) 'centimeters
                    If Double.IsNaN(lElevation) OrElse lElevation = lDemGrid.Header.NodataValue OrElse lElevation < -100000 Then
                        Dim lDirection As Integer = 1
                        Dim lRemainingDistance As Integer = 2
                        Dim lDistance As Integer = 1
                        Dim dx As Integer = 0
                        Dim dy As Integer = 0
                        Dim lSpiralRow As Integer = lRow
                        Dim lSpiralCol As Integer = lCol
                        Dim lMaxRow As Integer = lDemGrid.Header.NumberRows - 1
                        Dim lMaxCol As Integer = lDemGrid.Header.NumberCols - 1
                        Do
                            lRemainingDistance -= 1
                            If lRemainingDistance = 0 Then
                                Select Case lDirection
                                    Case 0 'Up
                                        lDistance += 1
                                        lDirection = 3 'Left
                                    Case 1 'Right
                                        lDirection = 0 'Up
                                    Case 2 ' Down
                                        lDistance += 1
                                        lDirection = 1 'Right
                                    Case 3 'Left
                                        lDirection = 2 'Down
                                End Select
                                lRemainingDistance = lDistance
                            End If
                            Select Case lDirection
                                Case 0 'Up
                                    If lSpiralRow > 0 Then
                                        dy -= 1
                                    Else
                                        lRemainingDistance = 1
                                    End If
                                Case 1 'Right
                                    If lSpiralCol < lMaxCol Then
                                        dx += 1
                                    Else
                                        lRemainingDistance = 1
                                    End If
                                Case 2 ' Down
                                    If lSpiralRow < lMaxRow Then
                                        dy += 1
                                    Else
                                        lRemainingDistance = 1
                                    End If
                                Case 3 'Left
                                    If lSpiralCol > 0 Then
                                        dx -= 1
                                    Else
                                        lRemainingDistance = 1
                                    End If
                            End Select
                            lSpiralRow = lRow + dy
                            If lSpiralRow >= 0 AndAlso lSpiralRow <= lMaxRow Then
                                lSpiralCol = lCol + dx
                                If lSpiralCol >= 0 AndAlso lSpiralCol < lMaxCol Then
                                    lElevation = lDemGrid.Value(lSpiralCol, lSpiralRow)
                                End If
                            End If
                        Loop While Double.IsNaN(lElevation) OrElse lElevation = lDemGrid.Header.NodataValue OrElse lElevation < -100000
                        Logger.Dbg("Found elevation (" & dx & ", " & dy & ") from (" & lCol & ", " & lRow & ")")
                    End If
                    lElevation /= 100.0 'cm to m
                    lSB.AppendLine(lShapeIndex & vbTab & lCatchments.CellValue(pCatchmentComIdFieldIndex, lShapeIndex) & vbTab _
                                 & lY & vbTab & lX & vbTab & lElevation)
                End If
            Next
            IO.File.WriteAllText(IO.Directory.GetParent(IO.Path.GetDirectoryName(aDemGridFilename)).ToString & "\Catchments.txt", lSB.ToString)
        End If
    End Sub

    Friend Sub CalculateFlowlineProperty(ByVal aFlowLineFileName As String, ByVal aDemGridFilename As String)
        Dim lOutputFilename As String = IO.Directory.GetParent(IO.Path.GetDirectoryName(aDemGridFilename)).ToString & "\Flowlines.txt"
        TryDelete(lOutputFilename)
        Dim lDemGrid As New MapWinGIS.Grid
        Dim lFlowlines As New MapWinGIS.Shapefile
        If lDemGrid.Open(aDemGridFilename) AndAlso _
           lFlowlines.Open(aFlowLineFileName) Then
            Dim lSB As New System.Text.StringBuilder
            Dim lArcSwatShapeFile As Boolean = False
            Dim lQMeanFieldIndex As Integer = FieldIndex(lFlowlines, "MAFlowU")
            Dim lSlopeFieldIndex As Integer = FieldIndex(lFlowlines, "Slope")
            Dim lLengthFieldIndex As Integer = FieldIndex(lFlowlines, "LengthKM")
            Dim lWidthFieldIndex As Integer
            Dim lDepthFieldIndex As Integer
            If pFlowlinesComIdFieldIndex = -1 Then
                pFlowlinesComIdFieldIndex = FieldIndex(lFlowlines, "Subbasin")
                pFlowlinesCumDrainAreaIndex = FieldIndex(lFlowlines, "AreaC")
                lQMeanFieldIndex = FieldIndex(lFlowlines, "MAFlowU")
                lSlopeFieldIndex = FieldIndex(lFlowlines, "Slo2")
                lLengthFieldIndex = FieldIndex(lFlowlines, "Len2")
                lWidthFieldIndex = FieldIndex(lFlowlines, "Wid2")
                lDepthFieldIndex = FieldIndex(lFlowlines, "Dep2")

                lArcSwatShapeFile = True
                lSB.AppendLine("Index" & vbTab & "SubBasin" & vbTab _
                             & "Width" & vbTab & "Depth" & vbTab & "Slope" & vbTab & "Length")
            Else
                lSB.AppendLine("Index" & vbTab & "ComID" & vbTab _
                             & "Width" & vbTab & "Depth" & vbTab & "Slope" & vbTab & "Length")
                pFlowlinesCumDrainAreaIndex = FieldIndex(lFlowlines, "CumDrainag")
                lQMeanFieldIndex = FieldIndex(lFlowlines, "MAFlowU")
                lSlopeFieldIndex = FieldIndex(lFlowlines, "Slope")
                lLengthFieldIndex = FieldIndex(lFlowlines, "LengthKM")
            End If

            For lShapeIndex As Integer = 0 To lFlowlines.NumShapes - 1
                Dim lFlowline As MapWinGIS.Shape = lFlowlines.Shape(lShapeIndex)
                Dim lComID As String
                Dim lWidth As Double = 0
                Dim lDepth As Double = 0
                Dim lSlope As Double = 0
                Dim lLength As Double = 0
                If lFlowline IsNot Nothing Then
                    lComID = lFlowlines.CellValue(pFlowlinesComIdFieldIndex, lShapeIndex)
                    If lComID <> "0" Then
                        If lArcSwatShapeFile Then
                            Try
                                lWidth = lFlowlines.CellValue(lWidthFieldIndex, lShapeIndex)
                                Logger.Dbg("Missing Width in flowline record " & lShapeIndex & " field " & lWidthFieldIndex & " of " & aFlowLineFileName)
                            Catch
                            End Try
                            Try
                                lDepth = lFlowlines.CellValue(lDepthFieldIndex, lShapeIndex)
                            Catch
                                Logger.Dbg("Missing Depth in flowline record " & lShapeIndex & " field " & lDepthFieldIndex & " of " & aFlowLineFileName)
                            End Try
                            Try
                                lSlope = lFlowlines.CellValue(lSlopeFieldIndex, lShapeIndex)
                                Logger.Dbg("Missing Slope in flowline record " & lShapeIndex & " field " & lSlopeFieldIndex & " of " & aFlowLineFileName)
                            Catch
                            End Try
                            Try
                                lLength = lFlowlines.CellValue(lLengthFieldIndex, lShapeIndex)
                                Logger.Dbg("Missing Length in flowline record " & lShapeIndex & " field " & lLengthFieldIndex & " of " & aFlowLineFileName)
                            Catch
                            End Try
                        Else
                            Dim lCentroid As MapWinGIS.Point = Nothing
                            Try
                                lCentroid = MapWinGeoProc.Statistics.Centroid(lFlowline)
                            Catch
                            End Try
                            If lCentroid Is Nothing OrElse lCentroid.x > 1.0E+30 OrElse lCentroid.y > 1.0E+30 Then
                                Logger.Dbg("Computing centroid as center of extents for flowline record " & lShapeIndex & " of " & aFlowLineFileName)
                                With lFlowline.Extents
                                    lCentroid.y = (.yMax + .yMin) / 2
                                    lCentroid.x = (.xMax + .xMin) / 2
                                End With
                            End If
                            Dim lY As Double = lCentroid.y
                            Dim lX As Double = lCentroid.x
                            MapWinGeoProc.SpatialReference.ProjectPoint(lX, lY, lFlowlines.Projection, D4EMDataManager.SpatialOperations.GeographicProjection)
                            Dim lRow As Integer
                            Dim lCol As Integer
                            lDemGrid.ProjToCell(lCentroid.x, lCentroid.y, lCol, lRow)
                            Dim lElevation As Double = lDemGrid.Value(lCol, lRow) / 100.0 'cm to m
                            Dim lAreaContrib As Double
                            If Not ObjectToDouble(lFlowlines.CellValue(pFlowlinesCumDrainAreaIndex, lShapeIndex), lAreaContrib) Then 'km
                                Logger.Dbg("Missing contributing area in flowline record " & lShapeIndex & " field " & pFlowlinesCumDrainAreaIndex & " of " & aFlowLineFileName)
                            End If
                            'Dim lQMean As Double = lFlowlines.CellValue(lQMeanFieldIndex, lShapeIndex) 'cfs
                            'Dim lQPk2 As Double = lQMean * 5 'TODO: WAG UGH - get BASINS neural network code !!!!!!!!!
                            'lWidth = 1.22 * (lQPk2 ^ 0.557) 'from BASINS FAQ
                            'lDepth = 0.34 * (lQPk2 ^ 0.341)
                            lWidth = 1.29 * (lAreaContrib ^ 0.6) 'from BASINS avenue script
                            lDepth = 0.13 * (lAreaContrib ^ 0.4)
                            If Not ObjectToDouble(lFlowlines.CellValue(lSlopeFieldIndex, lShapeIndex), lSlope) Then
                                Logger.Dbg("Missing slope in flowline record " & lShapeIndex & " field " & lSlopeFieldIndex & " of " & aFlowLineFileName)
                            End If
                            If Not ObjectToDouble(lFlowlines.CellValue(lLengthFieldIndex, lShapeIndex), lLength) Then
                                Logger.Dbg("Missing length in flowline record " & lShapeIndex & " field " & lLengthFieldIndex & " of " & aFlowLineFileName)
                            End If
                        End If
                    End If
                    If lSB.Length > 1000000 Then
                        IO.File.AppendAllText(lOutputFilename, lSB.ToString)
                        lSB = New System.Text.StringBuilder
                    End If
                    lSB.AppendLine(lShapeIndex & vbTab & lComID & _
                                                 vbTab & lWidth & _
                                                 vbTab & lDepth & _
                                                 vbTab & lSlope & _
                                                 vbTab & lLength)
                End If
            Next
            IO.File.AppendAllText(lOutputFilename, lSB.ToString)
        End If
    End Sub

    Private Function ObjectToDouble(ByVal aObject As Object, ByRef aDouble As Double) As Boolean
        If DBNull.Value.Equals(aObject) Then
            aDouble = 0
            Return False
        Else
            aDouble = CDbl(aObject)
            Return True
        End If
    End Function

    Friend Function ClipFlowLinesToCatchments(ByVal aCatchmentsToUseFilename As String, _
                                              ByVal aFlowLinesShapeFilename As String, _
                                              ByVal aFlowLinesToUseFilename As String) As Boolean
        Try
            Dim lFlowLinesToUse As New MapWinGIS.Shapefile
            Dim lFlowLinesDBF As New atcUtility.atcTableDBF
            lFlowLinesDBF.OpenFile(IO.Path.ChangeExtension(aFlowLinesShapeFilename, "dbf"))
            Dim lFlowLinesComIdField As Integer = lFlowLinesDBF.FieldNumber("COMID")
            Dim lFlowLinesDownstreamField As Integer = lFlowLinesDBF.FieldNumber("TOCOMID")

            Dim lCatchmentsDBF As New atcUtility.atcTableDBF
            lCatchmentsDBF.OpenFile(IO.Path.ChangeExtension(aCatchmentsToUseFilename, "dbf"))
            Dim lCatchmentsComIdField As Integer = lCatchmentsDBF.FieldNumber("COMID")

            Dim lFlowLinesToUseDBF As atcUtility.atcTableDBF = lFlowLinesDBF.Cousin
            lFlowLinesToUseDBF.InitData()

            Dim lFlowLines As New MapWinGIS.Shapefile
            lFlowLines.Open(aFlowLinesShapeFilename)
            If Not (lFlowLinesToUse.CreateNew(aFlowLinesToUseFilename, lFlowLines.ShapefileType)) Then
                Logger.Status("ProblemCreating " & aFlowLinesToUseFilename & " Message " & lFlowLinesToUse.ErrorMsg(lFlowLinesToUse.LastErrorCode), True)
            End If
            If Not lFlowLinesToUse.StartEditingShapes() Then
                Logger.Status("ProblemBeginingToEdit " & lFlowLinesToUse.Filename, True)
            End If
            Logger.Dbg("FlowlineCountInitial " & lFlowLines.NumShapes)

            Dim lReconnectIndex As Integer
            Dim lLastIndex As Integer = lFlowLines.NumShapes - 1
            For lFlowLinesShapeIndex As Integer = 0 To lLastIndex
                lFlowLinesDBF.CurrentRecord = lFlowLinesShapeIndex + 1
                Dim lComId As String = lFlowLinesDBF.Value(lFlowLinesComIdField)
                If IsNumeric(lComId) Then
                    If lCatchmentsDBF.FindFirst(lCatchmentsComIdField, lComId) Then
                        'LogMessage(aLog, "Found " & lComId & " addShape " & lFlowLinesToUse.NumShapes)
                        Dim lFlowLineShape As MapWinGIS.Shape = lFlowLines.Shape(lFlowLinesShapeIndex)
                        lFlowLinesToUse.EditInsertShape(lFlowLineShape, lFlowLinesToUse.NumShapes)
                        lFlowLinesToUseDBF.CurrentRecord = lFlowLinesToUseDBF.NumRecords + 1
                        lFlowLinesToUseDBF.RawRecord = lFlowLinesDBF.RawRecord
                    Else
                        Logger.Dbg("FlowLine " & lComId & " not added, does not have associated catchment")
                        Dim lDownstreamComID As String = lFlowLinesDBF.Value(lFlowLinesDownstreamField)
                        'Reconnect flowlines not yet reached in lFlowLinesDBF
                        For lReconnectIndex = lFlowLinesDBF.CurrentRecord + 1 To lLastIndex + 1
                            lFlowLinesDBF.CurrentRecord = lReconnectIndex
                            If lFlowLinesDBF.Value(lFlowLinesDownstreamField) = lComId Then
                                lFlowLinesDBF.Value(lFlowLinesDownstreamField) = lDownstreamComID
                            End If
                        Next
                        'Reconnect flowlines already added to lFlowLinesToUseDBF
                        For lReconnectIndex = 1 To lFlowLinesToUseDBF.NumRecords
                            lFlowLinesToUseDBF.CurrentRecord = lReconnectIndex
                            If lFlowLinesToUseDBF.Value(lFlowLinesDownstreamField) = lComId Then
                                lFlowLinesToUseDBF.Value(lFlowLinesDownstreamField) = lDownstreamComID
                            End If
                        Next

                    End If
                Else
                    Logger.Status("Error: Non-numeric COMID '" & lComId & "' at index " & lFlowLinesShapeIndex, True)
                End If
            Next
            Logger.Status("Writing " & lFlowLinesToUse.NumShapes & " flowlines, leaving out " & lFlowLines.NumShapes - lFlowLinesToUse.NumShapes)
            lFlowLinesToUse.StopEditingShapes()
            lFlowLinesToUse.Projection = lFlowLines.Projection
            lFlowLinesToUse.StopEditingTable()
            lFlowLinesToUse.Close()
            lFlowLinesToUseDBF.WriteFile(IO.Path.ChangeExtension(aFlowLinesToUseFilename, "dbf"))
            TryCopy(IO.Path.ChangeExtension(aFlowLinesShapeFilename, "mwsr"), _
                    IO.Path.ChangeExtension(aFlowLinesToUseFilename, "mwsr"))

            Return True
        Catch lEx As Exception
            Logger.Status("ClipFlowLinesToCatchments Problem: " & lEx.ToString, True)
            Return False
        End Try
    End Function

    Friend Sub ClipCatchments(ByVal aCatchmentsToUseFileName As String, _
                              ByVal aCatchments As MapWinGIS.Shapefile, _
                              ByVal aShapeOfInterest As MapWinGIS.Shape)
        Dim lCatchmentsToUse As New MapWinGIS.Shapefile
        Dim lLargestFractionInsideLeftOut As Double = 0
        Dim lSmallestFractionInsideKept As Double = 1
        Dim lLargestToUse As Double = atcUtility.GetNaN
        Dim lSmallestToUse As Double = atcUtility.GetNaN
        Dim lCatchmentsDBF As New atcUtility.atcTableDBF
        lCatchmentsDBF.OpenFile(IO.Path.ChangeExtension(aCatchments.Filename, "dbf"))

        Dim lCatchmentsToUseDBF As atcUtility.atcTableDBF = lCatchmentsDBF.Cousin
        lCatchmentsToUseDBF.InitData()

        lCatchmentsToUse.CreateNew(aCatchmentsToUseFileName, aCatchments.ShapefileType)
        lCatchmentsToUse.StartEditingShapes()

        For lCatchmentShapeIndex As Integer = 0 To aCatchments.NumShapes - 1
            Dim lCatchmentShape As MapWinGIS.Shape = aCatchments.Shape(lCatchmentShapeIndex)
            Dim lIntersection As MapWinGIS.Shape = MapWinGeoProc.SpatialOperations.Intersection(lCatchmentShape, aShapeOfInterest)
            If lIntersection IsNot Nothing AndAlso lIntersection.numPoints > 0 Then
                Dim lCatchmentArea As Double = MapWinGeoProc.Utils.Area(lCatchmentShape)
                If lCatchmentArea > 0 Then
                    Dim lIntersectionArea As Double = MapWinGeoProc.Utils.Area(lIntersection)
                    Dim lFractionInside As Double = lIntersectionArea / lCatchmentArea
                    If lFractionInside > 0.5 Then '0.5 = 50%
                        lCatchmentsToUse.EditInsertShape(lCatchmentShape, lCatchmentsToUse.NumShapes)

                        lCatchmentsDBF.CurrentRecord = lCatchmentShapeIndex + 1
                        lCatchmentsToUseDBF.CurrentRecord = lCatchmentsToUseDBF.NumRecords + 1
                        lCatchmentsToUseDBF.RawRecord = lCatchmentsDBF.RawRecord

                        If lFractionInside < lSmallestFractionInsideKept Then lSmallestFractionInsideKept = lFractionInside
                        If lCatchmentArea > lLargestToUse OrElse Double.IsNaN(lLargestToUse) Then lLargestToUse = lCatchmentArea
                        If lCatchmentArea < lSmallestToUse OrElse Double.IsNaN(lSmallestToUse) Then lSmallestToUse = lCatchmentArea
                        'LogMessage(aLog,"Catchment " & lCatchmentShapeIndex & " added: " & lFractionInside.ToString("0.0%"))
                    Else
                        If lFractionInside > lLargestFractionInsideLeftOut Then lLargestFractionInsideLeftOut = lFractionInside
                        'LogMessage(aLog,"Catchment " & lCatchmentShapeIndex & " skipped: " & lFractionInside.ToString("0.0%"))
                    End If
                Else
                    'LogMessage(aLog,"Catchment " & lCatchmentShapeIndex & " skipped, area<=0: " & lCatchmentArea.ToString)
                End If
            Else
                'LogMessage(aLog,"Catchment " & lCatchmentShapeIndex & " skipped, does not intersect shape of interest")
            End If
        Next
        Logger.Status("Writing '" & lCatchmentsToUse.NumShapes & "' catchments inside shape of interest, leaving out " & aCatchments.NumShapes - lCatchmentsToUse.NumShapes, True)
        Logger.Dbg("Smallest catchment (" & lSmallestToUse.ToString("#,###") & ") is " & (lSmallestToUse / lLargestToUse).ToString("0.0000%") & " of area of largest (" & lLargestToUse.ToString("#,###") & ")")
        If lLargestFractionInsideLeftOut > 0.001 Then Logger.Dbg("Largest percent inside left out = " & lLargestFractionInsideLeftOut.ToString("0.0000%"))
        If lSmallestFractionInsideKept < 0.999 Then Logger.Dbg("Smallest percent inside kept = " & lSmallestFractionInsideKept.ToString("0.0000%"))
        lCatchmentsToUse.StopEditingShapes()
        lCatchmentsToUse.Projection = aCatchments.Projection
        lCatchmentsToUse.StopEditingTable()
        lCatchmentsToUse.Close()
        lCatchmentsToUseDBF.WriteFile(IO.Path.ChangeExtension(aCatchmentsToUseFileName, "dbf"))
        TryCopy(IO.Path.ChangeExtension(aCatchments.Filename, "mwsr"), _
                IO.Path.ChangeExtension(aCatchmentsToUseFileName, "mwsr"))
    End Sub


    ''' <summary>
    ''' Overlay two shape files to create a lookup table.
    ''' Each shape in the second file is mapped to a shape in the first file.
    ''' The center of each shape's extents in the second shape file is tested for which shape contains it in the first shape file.
    ''' Resulting table will allow looking up an index in the first shape file from a key value from the second shape file.
    ''' </summary>
    ''' <param name="aShapeFileNameBigAreas"></param>
    ''' <param name="aShapeFileNameSmallAreas"></param>
    ''' <param name="aSmallAreasKeyFieldName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OverlayShapefiles(ByVal aShapeFileNameBigAreas As String, ByVal aShapeFileNameSmallAreas As String, ByVal aSmallAreasKeyFieldName As String) As atcUtility.atcCollection
        Dim lLookup As New atcUtility.atcCollection
        Dim lBig As New MapWinGIS.Shapefile
        If IO.File.Exists(aShapeFileNameBigAreas) AndAlso lBig.Open(aShapeFileNameBigAreas) Then
            lBig.BeginPointInShapefile()
            Dim lSmall As New MapWinGIS.Shapefile
            If IO.File.Exists(aShapeFileNameSmallAreas) AndAlso lSmall.Open(aShapeFileNameSmallAreas) Then
                Dim lSmallKeyFieldIndex As Integer = FieldIndex(lSmall, aSmallAreasKeyFieldName)
                Dim lX As Double, lY As Double
                Dim lBigShapeIndex As Integer
                For lSmallShapeIndex As Integer = lSmall.NumShapes - 1 To 0 Step -1
                    With lSmall.Shape(lSmallShapeIndex).Extents
                        lX = .xMin + (.xMax - .xMin) / 2
                        lY = .yMin + (.yMax - .yMin) / 2
                        lBigShapeIndex = lBig.PointInShapefile(lX, lY)
                        If lBigShapeIndex < 0 Then
                            Logger.Dbg("Small shape not found in big shapes " & lSmall.CellValue(lSmallKeyFieldIndex, lSmallShapeIndex))
                        End If
                        lLookup.Add(lSmall.CellValue(lSmallKeyFieldIndex, lSmallShapeIndex), lBigShapeIndex)
                    End With
                Next
                lSmall.Close()
            End If
            lBig.EndPointInShapefile()
            lBig.Close()
        End If
        Return lLookup
    End Function

    Private Function Count(ByVal aShapeFile As MapWinGIS.Shapefile, _
                           ByVal aFieldIndex As Integer, _
                           ByVal aFieldValue As Object) As Integer
        Dim lCount As Integer = 0
        For lRecordIndex As Integer = 0 To aShapeFile.NumShapes - 1
            Dim lCellValue As Object = aShapeFile.CellValue(aFieldIndex, lRecordIndex)
            If lCellValue.ToString.Length > 0 AndAlso lCellValue = aFieldValue Then
                lCount += 1
            End If
        Next
        Return lCount
    End Function
End Module
