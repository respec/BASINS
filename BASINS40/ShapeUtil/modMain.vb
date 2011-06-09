Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic

Imports atcUtility
Imports MapWinUtility.Strings
Imports MapWinUtility

Module modMain
	
	Private Declare Function GetTempPath Lib "kernel32"  Alias "GetTempPathA"(ByVal nBufferLength As Integer, ByVal lpBuffer As String) As Integer
	
	'Usage: ShapeUtil <newBaseFilename> <shpFilename>* [projectionfilename.proj | dump]
	'newBaseFilename: the path and filename without extension of the
	'   destination merged dbf, shp, and shx. If the file already exists, its
	'   current contents will be merged with the other file(s) in <shpFileName>*
	'   If a projection file is specified, <shpFileName>* will be re-projected before merging
	'   If "dump" is specified, all the named files are dumped as text to the log file
	'
	'Example: ShapeUtil c:\BASINS\data\project\st c:\temp\georgia.shp c:\temp\florida.shp c:\BASINS\data\project\prj.proj
	'   This will project georgia.shp and florida.shp from lat/long to the projection described in prj.proj,
	'   then merge both into st.shp. If st.shp already contains shapes, they are assumed to already be projected.
	'
	'Example: ShapeUtil c:\BASINS\data\project\st.shp dump
	'   This will dump the contents of st.shp to the log file c:\BASINS\data\project\ShapeUtilLog.txt
	'
	'Example: ShapeUtil c:\BASINS\data\project\st c:\BASINS\data\project\prj.proj
	'   st.shp will be re-projected from lat/long to the projection described in prj.proj
	'
    Public Sub Main()
        Dim exeFilename As String
        Dim cmd As String
        Dim curFilename As String
        Dim newBaseFilename As String
        Dim projectionSource As String = "" 'filename of source projection, default is lat/long degrees
        Dim projectionDest As String = ""   'filename of destination projection
        Dim shpFileNames As New ArrayList
        Dim dumpFlag As Boolean
        Dim starttime As Date

        starttime = Now
        cmd = Environment.CommandLine
        If Len(cmd) > 0 Then
            Try
                If Logger.FileName Is Nothing OrElse Logger.FileName.Length = 0 Then
                    Logger.StartToFile(IO.Path.Combine(IO.Path.GetTempPath, "ShapeUtil.log"))
                    Logger.TimeStamping = True
                End If
                Logger.Dbg("ShapeUtil CommandLine '" & cmd & "'")

                exeFilename = StrSplit(cmd, " ", """")
                newBaseFilename = StrSplit(cmd, " ", """")
                curFilename = StrSplit(cmd, " ", """")
                While Len(curFilename) > 0
                    If curFilename = "dump" Then
                        dumpFlag = True
                    Else
                        If Not IO.File.Exists(curFilename) Then
                            TryShapePointsFromDBF(FilenameNoExt(curFilename) & ".dbf")
                        End If
                        If IO.File.Exists(curFilename) Then
                            If curFilename.ToLower.EndsWith(".proj") Then
                                If Len(projectionDest) = 0 Then
                                    projectionDest = curFilename
                                Else
                                    projectionSource = curFilename
                                End If
                            Else
                                shpFileNames.Add(curFilename)
                            End If
                        ElseIf Len(cmd) > 0 Then
                            Logger.Dbg("File not found: " & curFilename)
                        End If
                    End If
                    curFilename = StrSplit(cmd, " ", """")
                End While
                If dumpFlag Then
                    Logger.TimeStamping = False 'turn off logging date and time to ease comparing dumped shape files
                    shpFileNames.Add(FilenameNoExt(newBaseFilename) & ".shp")
                    ShapeDump(shpFileNames)
                Else
                    If shpFileNames.Count > 0 Then
                        If Len(projectionDest) > 0 Then 'project all but new base
                            'frmShapeUtil.lblStatus.Caption = "Projecting..."
                            ShapeProject(projectionDest, projectionSource, shpFileNames)
                        End If
                        'frmShapeUtil.lblStatus.Caption = "Merging into " & newBaseFilename
                        ShapeMerge(GetKeyField(IO.Path.GetFileNameWithoutExtension(newBaseFilename)), newBaseFilename, shpFileNames)
                    ElseIf Len(projectionDest) > 0 Then
                        shpFileNames(0) = newBaseFilename
                        'frmShapeUtil.lblStatus.Caption = "Projecting..."
                        ShapeProject(projectionDest, projectionSource, shpFileNames)
                    Else
                        Logger.Msg("Could not find any files", "ShapeUtil")
                    End If
                End If
                Logger.Dbg("Elapsed time: " & Format((Now - starttime).TotalSeconds, "0.### sec"))
                Logger.Flush()
                'IO.File.AppendAllText(GetLogFilename(IO.Path.GetDirectoryName(newBaseFilename), "shape"), IO.File.ReadAllText(Logger.FileName))
            Catch ex As Exception
                If Logger.Msg(ex.Message & vbCrLf & "View Trace?", MsgBoxStyle.YesNo, "ShapeUtil Main") = MsgBoxResult.Yes Then
                    Logger.Flush()
                    MsgBox(IO.File.ReadAllText(Logger.FileName))
                End If
            End Try
        End If
    End Sub
	
	Private Function GetKeyField(ByRef shpBaseName As String) As String
        'Dim ff As New ATCoCtl.ATCoFindFile
        Dim lyrDBF As New atcTableDBF
        Dim lFilename As String = GetSetting("ShapeMerge", "files", "layers.dbf", "layers.dbf")
        'ff.SetRegistryInfo("ShapeMerge", "files", "layers.dbf")
        lyrDBF.OpenFile(FindFile("Please locate layers.dbf", lFilename))
        SaveSetting("ShapeMerge", "files", "layers.dbf", lFilename)
        If lyrDBF.FindFirst(1, shpBaseName) Then
            GetKeyField = lyrDBF.Value(3) '3 = key name, 4 = key number
        Else
            GetKeyField = CStr(0)
        End If
	End Function
	
	Private Function GetLogFilename(Optional ByVal aDefaultPath As String = "", Optional ByRef aDefaultSuffix As String = "") As String
		Dim BasinsPos As Integer
		If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir()
		If Right(aDefaultPath, 1) <> "\" Then aDefaultPath = aDefaultPath & "\"
		
		BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
		If BasinsPos > 0 Then aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "cache\"
		
        aDefaultPath = aDefaultPath & Format(Today, "log\\yyyy-mm-dd") & Format(TimeOfDay, "atHH-MM")
		If Len(aDefaultSuffix) > 0 Then aDefaultPath = aDefaultPath & "_" & aDefaultSuffix
		GetLogFilename = aDefaultPath & ".txt"
	End Function
	
	Private Function TryShapePointsFromDBF(ByRef dbfFilename As String) As Boolean
		Dim iField As Integer
		Dim LatitudeField As Integer
		Dim LongitudeField As Integer
        Dim dbf As atcTableDBF
        If IO.File.Exists(dbfFilename) Then
            dbf = New atcTableDBF
            dbf.OpenFile(dbfFilename)
            For iField = 1 To dbf.NumFields
                If Left(UCase(dbf.FieldName(iField)), 3) = "LAT" Then LatitudeField = iField
                If Left(UCase(dbf.FieldName(iField)), 3) = "LON" Then LongitudeField = iField
            Next
            If LatitudeField > 0 And LongitudeField > 0 Then
                WriteShapePointsFromDBF(dbf, LatitudeField, LongitudeField)
                TryShapePointsFromDBF = True
            End If
        End If
	End Function
End Module