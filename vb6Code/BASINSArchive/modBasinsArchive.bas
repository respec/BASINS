Attribute VB_Name = "modBasinsArchive"
Option Explicit

Public Const TEMP_PREFIX = "BASINS"
Public BasinsDrive As String
Public TempFiles As FastCollection

Public pLogger As clsATCoLogger

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

Sub Main()
  Dim ProjectName As String
  Dim s As String
  Dim ctag As String
  Dim FolderName As String
  Dim delim As String
  Dim quote As String
  Dim i As Long
  
  On Error GoTo ErrHand

  If App.PrevInstance Then
    MsgBox "Another copy of BASINS Archive is already running." & vbCr _
         & "Only one copy can be running at a time." & vbCr _
         & "You may need to use Task Manager if you cannot see the other copy.", vbCritical, "BASINS Archive"
  Else
    s = Command
    's = "/build, " & """" & "c:\basins\data\stlouis" & """, """ & "stl" & """"
    
    Set pLogger = New clsATCoLogger
    With pLogger
      .SetFileName GetLogFilename(, "archive"), False
      .DateTime = True
      .Log "BasinsArchive Main Entry " & App.Major & "." & App.Minor
      Set .Icon = frmAbout.Icon
    End With
        
    If InStr(LCase(s), "/feedback") > 0 Then 'Launch a feedback window instead of doing anything normal
      
      pLogger.Log "1: Dim feedback As clsATCoFeedback"
      Dim feedback As clsATCoFeedback
      pLogger.Log "2: Set feedback = New clsATCoFeedback"
      Set feedback = New clsATCoFeedback
      pLogger.Log "3: feedback.AddFile"
      feedback.AddFile Left(App.path, InStr(4, App.path, "\")) & "unins000.dat"
      pLogger.Log "4: feedback.AddText"
      feedback.AddText AboutString(False)
      pLogger.Log "5: feedback.AddLogFile"
      Set feedback.Logger = pLogger
      pLogger.Log "6: feedback.Show"
      feedback.Wait = True
      feedback.Show App, frmAbout.Icon
      Unload frmAbout
    Else
      BasinsDrive = "C:"
      Set TempFiles = New FastCollection
      
      App.HelpFile = GetSetting("BasinsArchive", "files", "BasinsArchive.chm")

      If Len(s) > 0 Then
        pLogger.Log "  Command Line:" & s
      Else
        pLogger.Log "  No Command Line"
      End If
          
      If InStr(LCase(s), "/build") > 0 Then
        delim = ","
        quote = """"
        ctag = StrSplit(s, delim, quote)
        FolderName = StrSplit(s, delim, quote)
        ProjectName = StrSplit(s, delim, quote)
        pLogger.Log "  Folder, Project from CL:" & FolderName & ", " & ProjectName
        If Len(FolderName) > 0 And Len(ProjectName) > 0 Then
          pLogger.Log "  Build apr without user interaction"
          If Not frmArchive.BuildNewBasinsApr(FolderName, ProjectName) Then
            pLogger.LogMsg "Batch Build of Basins Apr Failed for " & ProjectName, "BasinsArchive Main"
          End If
          Unload frmArchive
          End 'force program exit
        End If
      End If
      
      pLogger.Log "  Show frmArchive"
      frmArchive.Show
      
      If InStr(LCase(Command), "/project") > 0 Then
        ProjectName = Trim(Mid(Command, InStr(LCase(Command), "/project") + 9))
        pLogger.Log "   Set ProjectName to:" & ProjectName
        If Mid(ProjectName, 2, 1) = ":" Then
          BasinsDrive = Left(ProjectName, 2)
          pLogger.Log "   Set BasinsDrive to:" & BasinsDrive
          frmArchive.cboBasinsProject.Text = BasinsDrive
        End If
        frmArchive.lstBasinsProject.Text = FilenameOnly(ProjectName)
        frmArchive.cboBasinsProject.Text = FilenameOnly(ProjectName)
      End If
      
      If InStr(LCase(Command), "/restore") > 0 Then
        frmArchive.tabMain.Tab = 1
        pLogger.Log "   Set Tab to Restore"
      End If
      If InStr(LCase(Command), "/compare") > 0 Then
        frmArchive.tabMain.Tab = 2
        pLogger.Log "   Set Tab to Compare"
      End If
      If InStr(LCase(Command), "/showview") > 0 Then
        frmArchive.cmdView.Visible = True
        pLogger.Log "   Set View to visible"
      End If
      If InStr(LCase(Command), "/build") > 0 Then
        frmArchive.tabMain.Tab = 3
        pLogger.Log "   Set Tab to Build"
        If Len(FolderName) > 0 Then
          For i = 1 To frmArchive.lstFolders.ListCount
            If frmArchive.lstFolders.List(i - 1) = FilenameOnly(FolderName) Then
              frmArchive.lstFolders.ListIndex = i - 1
              Exit For
            End If
          Next i
        End If
      End If
      With frmArchive.tabMain
        pLogger.Log "   Tab is " & .TabCaption(.TabIndex - 1)
      End With
    End If
  End If
  
  Exit Sub
ErrHand:
  MsgBox Err.Description, vbCritical, "BASINS Archive"
End Sub

Public Function AboutString(Optional AboutFlag As Boolean = True) As String
  Dim s As String
  
  If AboutFlag Then
    s = App.title & " " & App.Major & "." & App.Minor & "." & App.Revision & vbCrLf _
    's = s & "FOR TESTING AND EVALUATION USE ONLY!" & vbCrLf
    s = s & "-----------" & vbCrLf
    s = s & "Inquiries about this software should be directed to" & vbCrLf
    s = s & "the organization which supplied you this software." & vbCrLf
    s = s & "-----------" & vbCrLf
  End If
  s = s & "A project may be restored with a new name and/or on a different drive. " & vbCrLf _
        & "Restored files will be automatically updated to reflect the new name." & vbCrLf & vbCrLf _
        & "The freely available tool 'gzip' is used for compression." & vbCrLf _
        & "See http://www.gzip.org/ for more information about gzip." & vbCrLf _
        & ""
  AboutString = s
End Function


