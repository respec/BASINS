Imports MapWinUtility

Public Class NetworkOperations
    ''' <summary>
    ''' Eliminate flowlines which do not have corresponding catchments
    ''' </summary>
    ''' <param name="aLog">destination for log messages, Nothing to use MapWinUtility.Logger</param>
    ''' <param name="aFlowlinesFileName">source flowlines shape file name</param>
    ''' <param name="aCatchmentFileName">source catchment shape file name</param>
    ''' <param name="aNewFlowlinesFilename">destination flowlines shape file name</param>
    ''' <param name="aNewCatchmentFilename">destination catchment shape file name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RemoveFlowlinesWithoutCatchment(ByVal aLog As IO.StreamWriter, _
                                                           ByVal aFlowlinesFileName As String, _
                                                           ByVal aCatchmentFileName As String, _
                                                           ByVal aNewFlowlinesFilename As String, _
                                                           ByVal aNewCatchmentFilename As String) As Boolean
        Dim lOutletComIds As New Generic.List(Of Long)

        'Dim lSimplifiedFlowlinesFileName As String = aFlowlinesFileName.Replace("flowline", "flowlineNoShort")
        'Dim lSimplifiedCatchmentFileName As String = aCatchmentFileName.Replace("catchment", "catchmentNoShort")

        Dim lSimplifiedFlowlines As MapWinGIS.Shapefile = CopyAndOpenNewShapefile(aLog, aFlowlinesFileName, aNewFlowlinesFilename)
        If lSimplifiedFlowlines Is Nothing Then Return False
        Dim lSimplifiedCatchment As MapWinGIS.Shapefile = CopyAndOpenNewShapefile(aLog, aCatchmentFileName, aNewCatchmentFilename)
        If lSimplifiedCatchment Is Nothing Then Return False

        LogMessage(aLog, "Process " & lSimplifiedFlowlines.NumShapes & " Flowlines " _
                                    & lSimplifiedCatchment.NumShapes & " Catchments ")

        Dim lRemoveFlowlineWithoutCatchmentCount As Integer = 0
        Dim lFlowlinesRecord As Integer = 0

        Dim lFields As New FieldIndexes(lSimplifiedFlowlines, lSimplifiedCatchment)

        'Remove flowlines with no catchment
        While lFlowlinesRecord < lSimplifiedFlowlines.NumShapes
            Dim lComId As Long = -1
            UpdateValueIfNotNull(lComId, lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lFlowlinesRecord))
            Dim lCatchmentRecord As Integer = FindRecord(lSimplifiedCatchment, lFields.CatchmentComId, lComId)
            If lCatchmentRecord = -1 Then 'missing catchment
                LogMessage(aLog, "RemoveFlowline " & DumpComid(lSimplifiedFlowlines, lComId, lFields) & " without catchment")
                If lOutletComIds.Contains(lComId) Then
                    lOutletComIds.Remove(lComId)
                End If
                Dim lToNode As Int64 = -1
                UpdateValueIfNotNull(lToNode, lSimplifiedFlowlines.CellValue(lFields.FlowlinesToNode, lFlowlinesRecord))
                Dim lRecordCombineIndex As Integer = FindRecord(lSimplifiedFlowlines, lFields.FlowlinesFromNode, lToNode)
                If lToNode > 0 AndAlso lRecordCombineIndex > -1 Then
                    Dim lToComId As Long = -1
                    UpdateValueIfNotNull(lToComId, lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lRecordCombineIndex))
                    LogMessage(aLog, "MergeDownWith " & DumpComid(lSimplifiedFlowlines, lToComId, lFields))
                    CombineFlowlines(aLog, lSimplifiedFlowlines, lRecordCombineIndex, lFlowlinesRecord, True, False, lFields, lOutletComIds)
                Else
                    Dim lFromNode As Int64 = -1
                    UpdateValueIfNotNull(lFromNode, lSimplifiedFlowlines.CellValue(lFields.FlowlinesFromNode, lFlowlinesRecord))
                    Dim lFromNodeCount As Integer = Count(lSimplifiedFlowlines, lFields.FlowlinesFromNode, lFromNode)
                    lRecordCombineIndex = FindRecord(lSimplifiedFlowlines, lFields.FlowlinesToNode, lFromNode)
                    If lFromNode > 0 AndAlso lRecordCombineIndex > -1 Then
                        LogMessage(aLog, "MergeUpWith " & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lRecordCombineIndex), lFields))
                        CombineFlowlines(aLog, lSimplifiedFlowlines, lRecordCombineIndex, lFlowlinesRecord, True, False, lFields, lOutletComIds)
                    Else
                        LogMessage(aLog, "OrphanFlowLine - no connections")
                        If lSimplifiedFlowlines.EditDeleteShape(lFlowlinesRecord) Then
                            LogMessage(aLog, "Removed " & lComId & " at " & lFlowlinesRecord & " of " & lSimplifiedFlowlines.NumShapes)
                        Else
                            LogMessage(aLog, "RemoveFailed")
                        End If
                    End If
                End If
            Else
                lFlowlinesRecord += 1
            End If
        End While
        LogMessage(aLog, "========RemovedFlowlinesWithoutCatchments " & lSimplifiedFlowlines.NumShapes & " " & lSimplifiedCatchment.NumShapes)
    End Function

    Public Shared Function ClipFlowLinesToCatchments(ByVal aLog As IO.StreamWriter, _
                                                     ByVal aCatchmentsFilename As String, _
                                                     ByVal aFlowLinesShapeFilename As String, _
                                                     ByVal aNewFlowlinesFilename As String) As Boolean
        Try
            Dim lFlowLinesToUse As New MapWinGIS.Shapefile
            Dim lFlowLinesDBF As New atcUtility.atcTableDBF
            lFlowLinesDBF.OpenFile(IO.Path.ChangeExtension(aFlowLinesShapeFilename, "dbf"))
            Dim lFlowLinesComIdField As Integer = lFlowLinesDBF.FieldNumber("COMID")
            Dim lFlowLinesDownstreamField As Integer = lFlowLinesDBF.FieldNumber("TOCOMID")

            Dim lCatchmentsDBF As New atcUtility.atcTableDBF
            lCatchmentsDBF.OpenFile(IO.Path.ChangeExtension(aCatchmentsFilename, "dbf"))
            Dim lCatchmentsComIdField As Integer = lCatchmentsDBF.FieldNumber("COMID")

            Dim lFlowLinesToUseDBF As atcUtility.atcTableDBF = lFlowLinesDBF.Cousin
            lFlowLinesToUseDBF.InitData()

            Dim lFlowLines As New MapWinGIS.Shapefile
            lFlowLines.Open(aFlowLinesShapeFilename)
            If Not (lFlowLinesToUse.CreateNew(aNewFlowlinesFilename, lFlowLines.ShapefileType)) Then
                Logger.Dbg("ProblemCreating " & aNewFlowlinesFilename & " Message " & lFlowLinesToUse.ErrorMsg(lFlowLinesToUse.LastErrorCode))
            End If
            If Not lFlowLinesToUse.StartEditingShapes() Then
                Logger.Dbg("ProblemBeginingToEdit " & lFlowLinesToUse.Filename)
            End If
            LogMessage(aLog, "FlowlineCountInitial " & lFlowLines.NumShapes)

            Dim lReconnectIndex As Integer
            Dim lLastIndex As Integer = lFlowLines.NumShapes - 1
            For lFlowLinesShapeIndex As Integer = 0 To lLastIndex
                lFlowLinesDBF.CurrentRecord = lFlowLinesShapeIndex + 1
                Dim lComId As String = lFlowLinesDBF.Value(lFlowLinesComIdField)
                If IsNumeric(lComId) Then
                    If lCatchmentsDBF.FindFirst(lCatchmentsComIdField, lComId) Then
                        Logger.Dbg("Found " & lComId & " addShape " & lFlowLinesToUse.NumShapes)
                        Dim lFlowLineShape As MapWinGIS.Shape = lFlowLines.Shape(lFlowLinesShapeIndex)
                        lFlowLinesToUse.EditInsertShape(lFlowLineShape, lFlowLinesToUse.NumShapes)
                        lFlowLinesToUseDBF.CurrentRecord = lFlowLinesToUseDBF.NumRecords + 1
                        lFlowLinesToUseDBF.RawRecord = lFlowLinesDBF.RawRecord
                    Else
                        LogMessage(aLog, "FlowLine " & lComId & " not added, does not have associated catchment")
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
                    LogMessage(aLog, "Error: Non-numeric COMID '" & lComId & "' at index " & lFlowLinesShapeIndex)
                End If
            Next
            LogMessage(aLog, "Writing '" & lFlowLinesToUse.NumShapes & "' flowlines, leaving out " & lFlowLines.NumShapes - lFlowLinesToUse.NumShapes)
            lFlowLinesToUse.StopEditingShapes()
            lFlowLinesToUse.Projection = lFlowLines.Projection
            lFlowLinesToUse.StopEditingTable()
            lFlowLinesToUse.Close()
            lFlowLinesToUseDBF.WriteFile(IO.Path.ChangeExtension(aNewFlowlinesFilename, "dbf"))
            TryCopy(IO.Path.ChangeExtension(aFlowLinesShapeFilename, "mwsr"), _
                    IO.Path.ChangeExtension(aNewFlowlinesFilename, "mwsr"))

            Return True
        Catch lEx As Exception
            LogMessage(aLog, "***** Problem: " & lEx.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Eliminate secondary flowlines which are in the same catchment as another flowline
    ''' </summary>
    ''' <param name="aLog">destination for log messages, Nothing to use MapWinUtility.Logger</param>
    ''' <param name="aFlowlinesFileName">source flowlines shape file name</param>
    ''' <param name="aCatchmentFileName">source catchment shape file name</param>
    ''' <param name="aNewFlowlinesFilename">destination flowlines shape file name</param>
    ''' <param name="aNewCatchmentFilename">destination catchment shape file name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RemoveBraidedFlowlines(ByVal aLog As IO.StreamWriter, _
                                                  ByVal aFlowlinesFileName As String, _
                                                  ByVal aCatchmentFileName As String, _
                                                  ByVal aNewFlowlinesFilename As String, _
                                                  ByVal aNewCatchmentFilename As String) As Boolean

        Dim lOutletComIds As New Generic.List(Of Long)
        Dim lSaveIntermediate As Integer = 500

        'Dim lSimplifiedFlowlinesFileName As String = aFlowlinesFileName.Replace("flowline", "flowlineNoShort")
        'Dim lSimplifiedCatchmentFileName As String = aCatchmentFileName.Replace("catchment", "catchmentNoShort")

        Dim lSimplifiedFlowlines As MapWinGIS.Shapefile = CopyAndOpenNewShapefile(aLog, aFlowlinesFileName, aNewFlowlinesFilename)
        If lSimplifiedFlowlines Is Nothing Then Return False
        Dim lSimplifiedCatchment As MapWinGIS.Shapefile = CopyAndOpenNewShapefile(aLog, aCatchmentFileName, aNewCatchmentFilename)
        If lSimplifiedCatchment Is Nothing Then Return False

        LogMessage(aLog, "Process " & lSimplifiedFlowlines.NumShapes & " Flowlines " _
                                    & lSimplifiedCatchment.NumShapes & " Catchments ")

        Dim lRemoveFlowlineWithoutCatchmentCount As Integer = 0
        Dim lFlowlinesRecord As Integer = 0

        Dim lFields As New FieldIndexes(lSimplifiedFlowlines, lSimplifiedCatchment)

        CheckConnectivity(aLog, lSimplifiedFlowlines, lFields, lOutletComIds)

        Dim lOldFlowlineCatchmentDelta As Integer
        Dim lBraidedFlowlinesCombinedCount As Integer = 0
        Dim lFlowlineCatchmentDelta As Integer = lSimplifiedFlowlines.NumShapes - lSimplifiedCatchment.NumShapes
        lFlowlinesRecord = 0
        'Remove braided flowlines
        While lFlowlinesRecord < lSimplifiedFlowlines.NumShapes
            Dim lComId As Long = lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lFlowlinesRecord)
            Dim lFromNode As Long = lSimplifiedFlowlines.CellValue(lFields.FlowlinesFromNode, lFlowlinesRecord)
            Dim lFromNodeCount As Integer = Count(lSimplifiedFlowlines, lFields.FlowlinesFromNode, lFromNode)
            If lFromNodeCount > 1 AndAlso lFromNode > 0 Then 'braided channel
                'Dim lDownstreamCount As Integer = Count(lSimplifiedFlowlines, lFlowlinesDownstreamComIdFieldIndex, lComId)
                Dim lDownstreamRecords As Generic.List(Of Integer) = FindRecords(lSimplifiedFlowlines, lFields.FlowlinesDownstreamComId, lComId)
                Dim lDownstreamCount As Integer = lDownstreamRecords.Count
                LogMessage(aLog, "BraidedChannel at " & lFlowlinesRecord & " of " & lSimplifiedFlowlines.NumShapes & " DownstreamCount " & lDownstreamCount)
                For Each lDownstreamRecord As Integer In lDownstreamRecords
                    LogMessage(aLog, "Downstream" & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lDownstreamRecord), lFields))
                Next
                If lDownstreamCount > 0 Then '
                    LogMessage(aLog, "PreferredChannel Keep " & lComId)
                Else ' not preferred(branch - remove)
                    Dim lCumAreaLarge As Double = 0
                    Dim lFindRecordKeep As Integer = -1
                    Dim lFindRecordRemove As Integer = -1
                    Dim lFindRecords As Generic.List(Of Integer) = FindRecords(lSimplifiedFlowlines, lFields.FlowlinesFromNode, lFromNode)
                    For Each lFindRecord As Integer In lFindRecords
                        Dim lCumArea As Double = -1
                        UpdateValueIfNotNull(lCumArea, lSimplifiedFlowlines.CellValue(lFields.FlowlinesCumDrainArea, lFindRecord))
                        LogMessage(aLog, " Found " & DumpComid(lSimplifiedFlowlines, lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lFindRecord), lFields))
                        If lCumAreaLarge < lCumArea Then
                            lCumAreaLarge = lCumArea
                            lFindRecordRemove = lFindRecordKeep
                            lFindRecordKeep = lFindRecord
                        Else
                            lFindRecordRemove = lFindRecord
                        End If
                    Next
                    Dim lCombineWithComId As Long = lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lFindRecordKeep)
                    Dim lRemoveComId As Long = lSimplifiedFlowlines.CellValue(lFields.FlowlinesComId, lFindRecordRemove)
                    If CombineCatchments(aLog, lSimplifiedCatchment, lCombineWithComId, lRemoveComId, lFields.CatchmentComId) Then
                        CombineFlowlines(aLog, lSimplifiedFlowlines, lFindRecordKeep, lFindRecordRemove, False, False, lFields, lOutletComIds)
                    Else
                        Logger.Dbg("Failed to merge braided catchment " & lRemoveComId & " into " & lCombineWithComId)
                    End If
                    lFlowlinesRecord -= 1
                    lBraidedFlowlinesCombinedCount += 1
                    lOldFlowlineCatchmentDelta = lFlowlineCatchmentDelta
                    lFlowlineCatchmentDelta = lSimplifiedFlowlines.NumShapes - lSimplifiedCatchment.NumShapes
                    LogMessage(aLog, "CountsFlowlines " & lSimplifiedFlowlines.NumShapes _
                            & " Catchments " & lSimplifiedCatchment.NumShapes _
                            & " Delta " & lFlowlineCatchmentDelta _
                            & " Removed " & lBraidedFlowlinesCombinedCount)
                End If
                If lFlowlineCatchmentDelta <> lOldFlowlineCatchmentDelta Then
                    LogMessage(aLog, "Error: ************* Delta Change - WHY ***********************")
                End If
                SaveIntermediate(aLog, lSimplifiedCatchment, lSimplifiedFlowlines)
            End If
            lFlowlinesRecord += 1
        End While
        LogMessage(aLog, "======== " & lBraidedFlowlinesCombinedCount & " Braided Flowlines Combined ========")
    End Function

    ''' <summary>
    ''' Eliminate flowlines which do not have corresponding catchments
    ''' </summary>
    ''' <param name="aLog">destination for log messages, Nothing to use MapWinUtility.Logger</param>
    ''' <param name="aFlowlinesFileName">source flowlines shape file name</param>
    ''' <param name="aCatchmentFileName">source catchment shape file name</param>
    ''' <param name="aNewFlowlinesFilename">destination flowlines shape file name</param>
    ''' <param name="aNewCatchmentFilename">destination catchment shape file name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MergeShortFlowlinesOrSmallCatchments(ByVal aLog As IO.StreamWriter, _
                                                                ByVal aFlowlinesFileName As String, _
                                                                ByVal aCatchmentFileName As String, _
                                                                ByVal aMinCatchmentKM2 As Double, _
                                                                ByVal aMinLengthKM As Double, _
                                                                ByVal aNewFlowlinesFilename As String, _
                                                                ByVal aNewCatchmentFilename As String) As Boolean

    End Function


    ''' <summary>
    ''' Copy existing shape file to a new name and open the new one for editing
    ''' If new file already exists, it is removed first
    ''' </summary>
    ''' <param name="aLog"></param>
    ''' <param name="aOldFilename"></param>
    ''' <param name="aNewFilename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CopyAndOpenNewShapefile(ByVal aLog As IO.StreamWriter, ByVal aOldFilename As String, ByVal aNewFilename As String) As MapWinGIS.Shapefile
        If aOldFilename.ToLower.Equals(aNewFilename.ToLower) Then
            LogMessage(aLog, "New shape file name was not different from old one, cannot copy '" & aNewFilename & "'")
            Return Nothing
        End If
        atcUtility.TryDeleteShapefile(aNewFilename)
        atcUtility.TryCopyShapefile(aOldFilename, aNewFilename)

        Dim lNewShapefile As New MapWinGIS.Shapefile
        If Not lNewShapefile.Open(aNewFilename) Then
            LogMessage(aLog, "Could not open '" & aNewFilename & "'")
            Return Nothing
        Else
            lNewShapefile.StartEditingShapes()
            lNewShapefile.StartEditingTable()
            Return lNewShapefile
        End If
    End Function

    Private Shared Function FieldIndex(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldName As String) As Integer
        For lFieldIndex As Integer = 0 To aShapeFile.NumFields - 1
            If aShapeFile.Field(lFieldIndex).Name.ToUpper = aFieldName.ToUpper Then 'this is the field we want
                Return lFieldIndex
            End If
        Next
        'Logger.Dbg("FieldIndex:Error:FieldName:" & aFieldName & ":IsNotRecognized")
        Return -1
    End Function

    ''' <summary>
    ''' Return record indexes of shapes with value in aFieldIndex matching aValue
    ''' </summary>
    ''' <param name="aShapeFile">shapes to search</param>
    ''' <param name="aFieldIndex">field index to check</param>
    ''' <param name="aValue">value of field to match</param>
    ''' <returns>list of Integer record indexes</returns>
    Private Shared Function FindRecords(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As Long) As Generic.List(Of Integer)
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

    'Find first matching record by field value as Long
    Private Shared Function FindRecord(ByVal aShapeFile As MapWinGIS.Shapefile, ByVal aFieldIndex As Integer, ByVal aValue As Long) As Integer
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
    Private Shared Function CombineFlowlines(ByVal aLog As IO.StreamWriter, _
                                             ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile, _
                                             ByVal aSourceBaseIndex As Integer, _
                                             ByVal aSourceDeletingIndex As Integer, _
                                             ByVal aMergeShapes As Boolean, _
                                             ByVal aKeepCosmeticRemovedLine As Boolean, _
                                             ByVal aFields As FieldIndexes, _
                                             ByVal aOutletComIDs As Generic.List(Of Long)) As Boolean
        Try

            Dim lSourceBaseIndex As Integer = aSourceBaseIndex
            With aFlowlinesShapeFile
                'Dim lNewDownstreamComId As Long = .CellValue(lFlowlinesComIdFieldIndex, aSourceBaseIndex)
                Dim lDeletedComId As Long = .CellValue(aFields.FlowlinesComId, aSourceDeletingIndex)
                Dim lBaseComId As Long = .CellValue(aFields.FlowlinesComId, lSourceBaseIndex)
                Dim lBaseDownStreamComId As Long = .CellValue(aFields.FlowlinesDownstreamComId, lSourceBaseIndex)
                Dim lDeletingDownstream As Boolean = False
                If lBaseDownStreamComId = lDeletedComId Then
                    Dim lNewDownstreamComId As Long = .CellValue(aFields.FlowlinesDownstreamComId, aSourceDeletingIndex)
                    LogMessage(aLog, "ChangeDownFor " & lBaseComId & " From " & lDeletedComId & " to " & lNewDownstreamComId)
                    .EditCellValue(aFields.FlowlinesDownstreamComId, lSourceBaseIndex, lNewDownstreamComId)
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
                                    'If lFieldName = "CUMLENKM" Then Logger.Dbg("Merged CUMLENKM = " & lNewFieldValues(lNewFieldValues.Count - 1))
                                Case "INCRFLOWU", "LOCDRAINA"
                                    If IsDBNull(lValueFromBase) OrElse Not IsNumeric(lValueFromBase) Then
                                        lNewFieldValues.Add(lValueFromDeleting)
                                    ElseIf IsDBNull(lValueFromDeleting) OrElse Not IsNumeric(lValueFromDeleting) Then
                                        lNewFieldValues.Add(lValueFromBase)
                                    Else
                                        lNewFieldValues.Add(lValueFromDeleting + lValueFromBase)
                                    End If
                                    'If lFieldName = "LOCDRAINA" Then Logger.Dbg("Merged LOCDRAINA = " & lNewFieldValues(lNewFieldValues.Count - 1) & " = " & lValueFromDeleting & " + " & lValueFromBase)
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
                                        LogMessage(aLog, "SlopeEstimatedAs " & lSlope & " MaxElev " & lMaxElev & " MinElev " & lMinElev & " Length " & lLength)
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
                        LogMessage(aLog, "FailedToSetFieldValueFor " & lFieldIndex & " " & lFieldName.ToUpper)
                        lNewFieldValues.Add("")
                    End Try
                    'Logger.Dbg(lFieldName & ": " & aFlowlinesShapeFile.CellValue(lFieldIndex, aSourceDeletingIndex) & ", " & aFlowlinesShapeFile.CellValue(lFieldIndex, lSourceBaseIndex) & " -> " & lNewFieldValues(lNewFieldValues.Count - 1))
                Next
                If aMergeShapes Then
                    Dim lMergedShape As New MapWinGIS.Shape
                    LogMessage(aLog, "FlowlineMerge " & lSourceBaseIndex & " and " & aSourceDeletingIndex)
                    MapWinGeoProc.SpatialOperations.MergeShapes(aFlowlinesShapeFile, lSourceBaseIndex, aSourceDeletingIndex, lMergedShape)
                    .EditDeleteShape(lSourceBaseIndex)
                    .EditInsertShape(lMergedShape, lSourceBaseIndex)
                Else
                    LogMessage(aLog, "FlowlineKeep " & lSourceBaseIndex & " Discard " & aSourceDeletingIndex)
                End If
                For lFieldIndex As Integer = 0 To .NumFields - 1
                    Try
                        .EditCellValue(lFieldIndex, lSourceBaseIndex, lNewFieldValues(lFieldIndex))
                    Catch
                        LogMessage(aLog, "FailedToEditFieldValueFor " & lFieldIndex)
                    End Try
                Next
                If Not aMergeShapes AndAlso aKeepCosmeticRemovedLine Then
                    LogMessage(aLog, "Moving cosmetic line to end of file")
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

                ReconnectUpstreamToDownstream(aLog, aFlowlinesShapeFile, lDeletedComId, lBaseComId, aFields)

                LogMessage(aLog, "Kept " & DumpComid(aFlowlinesShapeFile, lBaseComId, aFields))
            End With
            Return True
        Catch e As Exception
            LogMessage(aLog, "Error: CombineFlowlines:  " & e.Message)
            Return False
        End Try
    End Function

    Private Shared Sub ReconnectUpstreamToDownstream(ByVal aLog As IO.StreamWriter, _
                                                     ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile, _
                                                     ByVal aDeletedComId As Long, _
                                                     ByVal aDownstreamComId As Long, _
                                                     ByVal aFields As FieldIndexes)
        With aFlowlinesShapeFile
            Dim lFlowlinesRecord As Integer = 0
            While lFlowlinesRecord < .NumShapes
                Try
                    Dim lDownstreamComId As Long = .CellValue(aFields.FlowlinesDownstreamComId, lFlowlinesRecord)
                    If aDeletedComId = lDownstreamComId Then
                        Dim lComId As Long = .CellValue(aFields.FlowlinesComId, lFlowlinesRecord)
                        'LogMessage(aLog,"ChangeDownFor " & lComId & " From " & lDeletedComId & " to " & lNewDownstreamComId)
                        '.EditCellValue(lFlowlinesDownstreamFieldIndex, lFlowlinesRecord, lNewDownstreamComId)
                        LogMessage(aLog, "ChangeDownFor " & lComId & " From " & aDeletedComId & " to " & aDownstreamComId & " at " & lFlowlinesRecord)
                        .EditCellValue(aFields.FlowlinesDownstreamComId, lFlowlinesRecord, aDownstreamComId)
                    End If
                Catch lEx As Exception
                    LogMessage(aLog, "ChangeFailedAt " & lFlowlinesRecord)
                End Try
                lFlowlinesRecord += 1
            End While
        End With
    End Sub

    Private Shared Function DumpComid(ByVal aFlowLinesShapeFile As MapWinGIS.Shapefile, ByVal aComId As Long, ByVal aFields As FieldIndexes) As String
        Dim lRecordIndex As Integer = FindRecord(aFlowLinesShapeFile, aFields.FlowlinesComId, aComId)
        With aFlowLinesShapeFile
            Dim lToNode As Int64 = -1
            Dim lFromNode As Int64 = -1
            Dim lLength As Double = -1
            Dim lCumLength As Double = -1
            Dim lCumArea As Double = -1
            Dim lLocArea As Double = -1
            Dim lToComId As Long = 0
            UpdateValueIfNotNull(lToNode, .CellValue(aFields.FlowlinesToNode, lRecordIndex))
            UpdateValueIfNotNull(lFromNode, .CellValue(aFields.FlowlinesFromNode, lRecordIndex))
            UpdateValueIfNotNull(lLength, .CellValue(aFields.FlowlinesLength, lRecordIndex))
            UpdateValueIfNotNull(lCumLength, .CellValue(aFields.FlowlinesCumLen, lRecordIndex))
            UpdateValueIfNotNull(lLocArea, .CellValue(aFields.FlowlinesLocDrainArea, lRecordIndex))
            UpdateValueIfNotNull(lCumArea, .CellValue(aFields.FlowlinesCumDrainArea, lRecordIndex))
            UpdateValueIfNotNull(lToComId, .CellValue(aFields.FlowlinesDownstreamComId, lRecordIndex))
            Return aComId & "(" & lRecordIndex & "-u" & lFromNode & "-d" & lToNode & "-Len" & lLength & "-LenCum" & lCumLength & "-Area" & lLocArea & "-AreaCum" & lCumArea & "-t" & lToComId & ")"
        End With
    End Function

    Private Shared Function UpdateValueIfNotNull(ByRef aCurrentValue As Long, ByRef aNewObject As Object) As Boolean
        Try
            If aNewObject IsNot System.DBNull.Value Then
                aCurrentValue = aNewObject
                Return True
            End If
        Catch
        End Try
        Return False
    End Function

    Private Shared Function UpdateValueIfNotNull(ByRef aCurrentValue As Double, ByRef aNewObject As Object) As Boolean
        Try
            If aNewObject IsNot System.DBNull.Value Then
                aCurrentValue = aNewObject
                Return True
            End If
        Catch
        End Try
        Return False
    End Function

    Private Shared Function CheckConnectivity(ByVal aLog As IO.StreamWriter, _
                                              ByVal aFlowlinesShapeFile As MapWinGIS.Shapefile, _
                                              ByVal aFields As FieldIndexes, _
                                              ByVal aOutletComIDs As Generic.List(Of Long)) As Boolean
        LogMessage(aLog, "*** CheckConnectivity Begin")
        Dim lRemoveCount As Integer = 0
        Dim lMissingCount As Integer = 0
        Dim lLargestContribAreaComId As Long = 0
        Dim lLargestContribArea As Double = 0.0
        With aFlowlinesShapeFile
            Dim lFlowlinesRecord As Integer = .NumShapes - 1
            Dim lSkipThisRecord As Boolean = False
            While lFlowlinesRecord >= 0
                Dim lComId As Long = -1
                UpdateValueIfNotNull(lComId, .CellValue(aFields.FlowlinesComId, lFlowlinesRecord))
                Dim lContribArea As Double = -1
                If UpdateValueIfNotNull(lContribArea, .CellValue(aFields.FlowlinesCumDrainArea, lFlowlinesRecord)) _
                   AndAlso lLargestContribArea < lContribArea Then
                    lLargestContribArea = lContribArea
                    lLargestContribAreaComId = lComId
                    LogMessage(aLog, "NewLargestContribArea " & DumpComid(aFlowlinesShapeFile, lComId, aFields))
                End If
                Dim lDownstreamComId As Long = -1

                If UpdateValueIfNotNull(lDownstreamComId, .CellValue(aFields.FlowlinesDownstreamComId, lFlowlinesRecord)) AndAlso lDownstreamComId > -1 Then
                    If lDownstreamComId = 0 Then
                        LogMessage(aLog, "ZeroDownstreamComID, add outlet " & DumpComid(aFlowlinesShapeFile, lComId, aFields))
                        lMissingCount += 1
                        If Not aOutletComIDs.Contains(lComId) Then
                            aOutletComIDs.Add(lComId)
                        End If
                    ElseIf FindRecord(aFlowlinesShapeFile, aFields.FlowlinesComId, lDownstreamComId) < 0 Then
                        LogMessage(aLog, "MissingDownstreamComID " & lDownstreamComId & ", add outlet " & DumpComid(aFlowlinesShapeFile, lComId, aFields))
                        lMissingCount += 1
                        If Not aOutletComIDs.Contains(lComId) Then
                            aOutletComIDs.Add(lComId)
                        End If
                    End If
                ElseIf lSkipThisRecord = False Then
                    LogMessage(aLog, "FlowLineRecordNotNumericDown, add outlet " & DumpComid(aFlowlinesShapeFile, lComId, aFields))
                    lMissingCount += 1
                    If Not aOutletComIDs.Contains(lComId) Then
                        aOutletComIDs.Add(lComId)
                    End If
                End If
                lFlowlinesRecord -= 1
            End While
        End With
        LogMessage(aLog, " ** CheckConnectivityEnd, MissingOutletsAdded " & lMissingCount)
        If Not aOutletComIDs.Contains(lLargestContribAreaComId) Then
            LogMessage(aLog, " ** AddMainChannelToOutletComIds")
            aOutletComIDs.Add(lLargestContribAreaComId)
        End If

        Return True
    End Function

    Private Shared Function CombineCatchments(ByVal aLog As IO.StreamWriter, _
                                              ByVal aCatchmentShapeFile As MapWinGIS.Shapefile, _
                                              ByVal aKeptComId As Long, _
                                              ByVal aAddingComId As Long, _
                                              ByVal aCatchmentComIdFieldIndex As Integer) As Boolean
        Try
            Dim lRecordKeptIndex As Integer = FindRecord(aCatchmentShapeFile, aCatchmentComIdFieldIndex, aKeptComId)
            Dim lRecordAddingIndex As Integer = FindRecord(aCatchmentShapeFile, aCatchmentComIdFieldIndex, aAddingComId)
            LogMessage(aLog, "CatchmentKeep " & aKeptComId & "(" & lRecordKeptIndex & ") Merge " & aAddingComId & "(" & lRecordAddingIndex & ")")
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
                            Logger.Dbg("Exception setting " & .Field(lFieldIndex).Name & ": " & exField.Message)
                            lNewFieldValues.Add("")
                        End Try
                    Next

                    Dim lTargetShape As MapWinGIS.Shape = Nothing
                    Try
                        lTargetShape = aCatchmentShapeFile.Shape(lRecordKeptIndex).Clip(aCatchmentShapeFile.Shape(lRecordAddingIndex), MapWinGIS.tkClipOperation.clUnion)
                    Catch e As Exception
                        LogMessage(aLog, "Error: CombineCatchments:ClipTargetShape  " & e.Message)
                        Try
                            MapWinGeoProc.SpatialOperations.MergeShapes(aCatchmentShapeFile, lRecordKeptIndex, lRecordAddingIndex, lTargetShape)
                        Catch ex As Exception
                            LogMessage(aLog, "Error: CombineCatchments:MergeShapes  " & ex.Message)
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
                LogMessage(aLog, "SkipMerge " & lRecordKeptIndex & " " & lRecordAddingIndex)
            End If
            Return True
        Catch e As Exception
            LogMessage(aLog, "Error: CombineCatchments:  " & e.Message)
            Return False
        End Try
    End Function

    Private Shared Sub LogMessage(ByVal aLog As IO.StreamWriter, ByVal aMessage As String)
        If aLog Is Nothing Then
            Logger.Dbg(aMessage)
        Else
            aLog.WriteLine(aMessage)
            Debug.WriteLine(aMessage)
        End If
    End Sub

    Private Shared Function Count(ByVal aShapeFile As MapWinGIS.Shapefile, _
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

    Private Shared Sub SaveIntermediate(ByVal aLog As IO.StreamWriter, _
                                        ByVal aCatchments As MapWinGIS.Shapefile, _
                                        ByVal aFlowlines As MapWinGIS.Shapefile)
        Static lCount As Integer
        Dim lResult As Integer
        lCount += 1
        Math.DivRem(lCount, 500, lResult) 'every 500 write intermediate shapes
        If lResult = 0 Then
            LogMessage(aLog, "SaveIntermediate " & lCount & " " & aCatchments.NumShapes)
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

    Private Class FieldIndexes
        Public FlowlinesLength As Integer
        Public FlowlinesCumLen As Integer
        Public FlowlinesCumDrainArea As Integer
        Public FlowlinesLocDrainArea As Integer
        Public FlowlinesDownstreamComId As Integer
        Public FlowlinesComId As Integer
        Public FlowlinesToNode As Integer
        Public FlowlinesFromNode As Integer
        Public CatchmentComId As Integer

        Public Sub New(ByVal lFlowlineShapefile As MapWinGIS.Shapefile, ByVal lCatchmentShapefile As MapWinGIS.Shapefile)
            FlowlinesLength = FieldIndex(lFlowlineShapefile, "LENGTHKM")
            FlowlinesCumLen = FieldIndex(lFlowlineShapefile, "CUMLENKM")
            If FlowlinesCumLen < 0 Then 'Need to add cumulative length field
                FlowlinesCumLen = lFlowlineShapefile.NumFields
                Dim lLenField As MapWinGIS.Field = lFlowlineShapefile.Field(FlowlinesLength)
                Dim lNewField As New MapWinGIS.Field
                With lNewField
                    .Name = "CUMLENKM"
                    .Type = lLenField.Type
                    .Width = lLenField.Width + 1
                    .Precision = lLenField.Precision
                End With
                lFlowlineShapefile.EditInsertField(lNewField, FlowlinesCumLen)
            End If

            FlowlinesCumDrainArea = FieldIndex(lFlowlineShapefile, "CUMDRAINAG")
            FlowlinesLocDrainArea = FieldIndex(lFlowlineShapefile, "LOCDRAINA")
            If FlowlinesLocDrainArea < 0 Then 'Need to add local drainage area field
                FlowlinesLocDrainArea = lFlowlineShapefile.NumFields
                Dim lCumField As MapWinGIS.Field = lFlowlineShapefile.Field(FlowlinesCumDrainArea)
                Dim lNewField As New MapWinGIS.Field
                With lNewField
                    .Name = "LOCDRAINA"
                    .Type = lCumField.Type
                    .Width = lCumField.Width
                    .Precision = lCumField.Precision
                End With
                lFlowlineShapefile.EditInsertField(lNewField, FlowlinesLocDrainArea)
            End If

            FlowlinesDownstreamComId = FieldIndex(lFlowlineShapefile, "TOCOMID")
            FlowlinesComId = FieldIndex(lFlowlineShapefile, "COMID")
            FlowlinesToNode = FieldIndex(lFlowlineShapefile, "TONODE")
            FlowlinesFromNode = FieldIndex(lFlowlineShapefile, "FROMNODE")
            CatchmentComId = FieldIndex(lCatchmentShapefile, "COMID")
        End Sub
    End Class
End Class
