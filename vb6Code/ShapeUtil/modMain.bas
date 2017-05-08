Attribute VB_Name = "modMain"
Option Explicit

Public gLogger As ATCoFeedback.clsATCoLogger

Private Declare Function GetTempPath Lib "kernel32" Alias "GetTempPathA" _
            (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long

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
  Dim cmd As String
  Dim curFilename As String
  Dim newBaseFilename As String
  Dim projectionSource As String 'filename of source projection, default is lat/long degrees
  Dim projectionDest As String   'filename of destination projection
  Dim shpFileNames() As String
  Dim nFileNames As Integer
  Dim dumpFlag As Boolean
  Dim starttime As Single
  Dim tmpLogFilename As String
  
  On Error GoTo ErrHand

  starttime = Timer
  ReDim shpFileNames(0)
  cmd = Command
  If Len(cmd) > 0 Then
    tmpLogFilename = GetTmpPath
    If Right(tmpLogFilename, 1) <> "\" Then tmpLogFilename = tmpLogFilename & "\"
    tmpLogFilename = tmpLogFilename & "ShapeUtil.log"
    
    Set gLogger = New clsATCoLogger
    gLogger.SetFileName tmpLogFilename
    gLogger.Log2Debug = False
    gLogger.DateTime = True
  
    gLogger.Log "ShapeUtil CommandLine '" & Command & "'"
    
    newBaseFilename = StrSplit(cmd, " ", """")
    curFilename = StrSplit(cmd, " ", """")
    While Len(curFilename) > 0
      If curFilename = "dump" Then
          dumpFlag = True
      Else
        If Not FileExists(curFilename) Then
          TryShapePointsFromDBF FilenameNoExt(curFilename) & ".dbf"
        End If
        If FileExists(curFilename) Then
          If LCase(FileExt(curFilename)) = "proj" Then
            If Len(projectionDest) = 0 Then
              projectionDest = curFilename
            Else
              projectionSource = curFilename
            End If
          Else
            nFileNames = nFileNames + 1
            ReDim Preserve shpFileNames(nFileNames)
            shpFileNames(nFileNames) = curFilename
          End If
        ElseIf Len(cmd) > 0 Then
          gLogger.Log "File not found: " & curFilename
        End If
      End If
      curFilename = StrSplit(cmd, " ", """")
    Wend
    If dumpFlag Then
      gLogger.DateTime = False 'turn off logging date and time to ease comparing dumped shape files
      nFileNames = nFileNames + 1
      ReDim Preserve shpFileNames(nFileNames)
      shpFileNames(nFileNames) = FilenameNoExt(newBaseFilename) & ".shp"
      ShapeDump shpFileNames
    Else
      If nFileNames > 0 Then
        If Len(projectionDest) > 0 Then 'project all but new base
          'frmShapeUtil.lblStatus.Caption = "Projecting..."
          ShapeProject projectionDest, projectionSource, shpFileNames
        End If
        'frmShapeUtil.lblStatus.Caption = "Merging into " & newBaseFilename
        ShapeMerge GetKeyField(FilenameOnly(newBaseFilename)), _
                   newBaseFilename, shpFileNames
      ElseIf Len(projectionDest) > 0 Then
        shpFileNames(0) = newBaseFilename
        'frmShapeUtil.lblStatus.Caption = "Projecting..."
        ShapeProject projectionDest, projectionSource, shpFileNames
      Else
        gLogger.LogMsg "Could not find any files", "ShapeUtil"
      End If
    End If
CloseLog:
    gLogger.Log "Elapsed time: " & Format(Timer - starttime, "#.### sec")
    AppendFileString GetLogFilename(PathNameOnly(newBaseFilename), "shape"), gLogger.CurrentLog
    Set gLogger = Nothing
    On Error Resume Next
    Kill tmpLogFilename
  End If
  
  Exit Sub
  
ErrHand:
  If gLogger.LogMsg(Err.Description & " (" & Err.Source & ")", "ShapeUtil Main", "Ok", "View Trace") = 2 Then
    MsgBox gLogger.CurrentLog
  End If
  Resume CloseLog
End Sub

Private Function GetKeyField(shpBaseName As String) As String
  Dim ff As New ATCoFindFile
  Dim lyrDBF As New clsDBF
  ff.SetDialogProperties "Please locate layers.dbf", "layers.dbf"
  ff.SetRegistryInfo "ShapeMerge", "files", "layers.dbf"
  lyrDBF.OpenDBF ff.GetName
  If lyrDBF.FindFirst(1, shpBaseName) Then
    GetKeyField = lyrDBF.Value(3) '3 = key name, 4 = key number
  Else
    GetKeyField = 0
  End If
End Function

Private Function GetLogFilename(Optional ByVal aDefaultPath As String = "", _
                                Optional ByRef aDefaultSuffix As String = "") As String
  Dim BasinsPos As Long
  If Len(aDefaultPath) = 0 Then aDefaultPath = CurDir
  If Right(aDefaultPath, 1) <> "\" Then aDefaultPath = aDefaultPath & "\"
  
  BasinsPos = InStr(UCase(aDefaultPath), "BASINS\")
  If BasinsPos > 0 Then aDefaultPath = Left(aDefaultPath, BasinsPos + 6) & "cache\"
  
  aDefaultPath = aDefaultPath & Format(Date, "log\\yyyy-mm-dd") & Format(Time, "atHH-MM")
  If Len(aDefaultSuffix) > 0 Then aDefaultPath = aDefaultPath & "_" & aDefaultSuffix
  GetLogFilename = aDefaultPath & ".txt"
End Function

Private Function GetTmpPath() As String
  Dim strFolder As String
  Dim lngResult As Long
  Dim MAX_PATH As Long
  MAX_PATH = 260
  strFolder = String(MAX_PATH, 0)
  lngResult = GetTempPath(MAX_PATH, strFolder)
  If lngResult <> 0 Then
      GetTmpPath = Left(strFolder, InStr(strFolder, Chr(0)) - 1)
  Else
      GetTmpPath = ""
  End If
End Function

Private Function TryShapePointsFromDBF(dbfFilename As String) As Boolean
  If FileExists(dbfFilename) Then
    Dim iField As Long
    Dim LatitudeField As Long
    Dim LongitudeField As Long
    Dim dbf As clsDBF
    Set dbf = New clsDBF
    dbf.OpenDBF dbfFilename
    For iField = 1 To dbf.NumFields
      If Left(UCase(dbf.FieldName(iField)), 3) = "LAT" Then LatitudeField = iField
      If Left(UCase(dbf.FieldName(iField)), 3) = "LON" Then LongitudeField = iField
    Next
    If LatitudeField > 0 And LongitudeField > 0 Then
      WriteShapePointsFromDBF dbf, LatitudeField, LongitudeField
      TryShapePointsFromDBF = True
    End If
  End If
End Function


