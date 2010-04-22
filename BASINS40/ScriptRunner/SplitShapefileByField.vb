Imports atcData
Imports atcData.atcTimeseriesGroup
Imports atcUtility
Imports atcUtility.modFile
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGIS

Imports Microsoft.VisualBasic
Imports System


Public Module SplitShapefileByField
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalShapefileName As String = ""
        Dim lSplitFieldName As String = "HUC_8"
        Dim lSplitFilenamePattern As String = "split\$SPLIT$\huc12.shp"
        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("Shape File To Split", lOriginalShapefileName)
            .Add("Split Field Name", lSplitFieldName)
            .Add("Split Filename Pattern", lSplitFilenamePattern)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Specify full path of shape file to split and pattern of split file names to write into", lUserParms) Then
            With lUserParms
                lOriginalShapefileName = .ItemByKey("Shape File To Split")
                lSplitFieldName = .ItemByKey("Split Field Name")
                lSplitFilenamePattern = .ItemByKey("Split Filename Pattern")
            End With
            Dim lOriginalShapefile As New MapWinGIS.Shapefile
            Dim lOriginalDBF As New atcTableDBF
            If lOriginalShapefile.Open(lOriginalShapefileName) AndAlso lOriginalDBF.OpenFile(IO.Path.ChangeExtension(lOriginalShapefileName, "dbf")) Then
                Dim lSplitField As Integer = lOriginalDBF.FieldNumber(lSplitFieldName)
                If lSplitField > 0 Then
                    Dim lProjectionFilename As String = IO.Path.ChangeExtension(lOriginalShapefileName, "prj")
                    Dim lProjectionFile As String = ""
                    If IO.File.Exists(lProjectionFilename) Then
                        lProjectionFile = atcUtility.WholeFileString(lProjectionFilename)
                    End If
                    Dim lCurrentRecord As Integer
                    Dim lNumRecords As Integer = lOriginalDBF.NumRecords
                    Dim lSplitValue As String
                    Dim lSplitValues As New atcCollection

                    Logger.Status("Finding unique values for " & lSplitFieldName)

                    For lCurrentRecord = 1 To lNumRecords
                        lOriginalDBF.CurrentRecord = lCurrentRecord
                        lSplitValue = lOriginalDBF.Value(lSplitField)
                        lSplitValues.Increment(lSplitValue)
                        Logger.Progress(lCurrentRecord, lNumRecords)
                    Next
                    Dim lNumSplitValues As Integer = lSplitValues.Count
                    Logger.Status("Splitting into " & lNumSplitValues & " files")
                    lSplitValues.Sort()

                    For lSplitValueIndex As Integer = 0 To lNumSplitValues - 1
                        lSplitValue = lSplitValues.Keys(lSplitValueIndex)
                        Logger.Status("MSG2 " & lSplitValue)
                        Dim lNewShapefileName As String = lSplitFilenamePattern.Replace("$SPLIT$", lSplitValue)
                        If Not IO.Path.IsPathRooted(lNewShapefileName) Then
                            lNewShapefileName = IO.Path.Combine(IO.Path.GetDirectoryName(lOriginalShapefileName), lNewShapefileName)
                        End If
                        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lNewShapefileName))
                        Dim lNewShapefile As New MapWinGIS.Shapefile
                        lNewShapefile.CreateNewWithShapeID(lNewShapefileName, lOriginalShapefile.ShapefileType)
                        lNewShapefile.StartEditingShapes()

                        Dim lNewShapeIndex As Integer = 0

                        Dim lNewDBF As atcTableDBF = lOriginalDBF.Cousin
                        lNewDBF.NumRecords = lSplitValues.ItemByIndex(lSplitValueIndex)
                        lNewDBF.InitData()

                        If lOriginalDBF.FindFirst(lSplitField, lSplitValue) Then
                            Do
                                lNewShapefile.EditInsertShape(lOriginalShapefile.Shape(lOriginalDBF.CurrentRecord - 1), lNewShapeIndex)
                                lNewShapeIndex += 1
                                lNewDBF.CurrentRecord = lNewShapeIndex
                                lNewDBF.RawRecord = lOriginalDBF.RawRecord
                            Loop While lOriginalDBF.FindNext(lSplitField, lSplitValue)
                        End If
                        lNewShapefile.StopEditingShapes()
                        lNewShapefile.Save()
                        lNewShapefile.Close()

                        If lProjectionFile.Length > 0 Then
                            lProjectionFilename = IO.Path.ChangeExtension(lNewShapefileName, "prj")
                            atcUtility.SaveFileString(lProjectionFilename, lProjectionFile)
                        End If

                        lNewDBF.WriteFile(IO.Path.ChangeExtension(lNewShapefileName, "dbf"))
                        lNewDBF.Clear()

                        Logger.Progress(lSplitValueIndex + 1, lNumSplitValues)
                    Next
                End If
                Logger.Status("")
            End If
        End If
    End Sub

End Module
