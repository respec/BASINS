Attribute VB_Name = "modGenScnScript"
Option Explicit

Sub RunScript(Optional scriptString As String = "", Optional args As String = "")
  Dim Progress As String
  Dim script As clsATCScript
  Dim pScriptManager As ATCScriptManager
  Dim testval As String
  Dim TserArray() As ATCclsTserData
  Dim TserIndex As Long
  Dim SaveActiveCount As Long
  Dim SelectedColl As Collection
  Dim SaveDir As String
     
  On Error GoTo errhandler

  SaveDir = CurDir
  ChDriveDir App.path
  
  If pScriptManager Is Nothing Then
    Progress = Progress & vbCr & "  Creating Manager"
    Set pScriptManager = New ATCScriptManager
    Set pScriptManager.IPC = IPC
  End If
  
  If Len(scriptString) = 0 Then
    With frmGenScn.cdl
      .CancelError = True
      .DialogTitle = "Select a script to run"
      .Filename = GetSetting("GenScn", "Script", "LastRun", "")
      If Len(.Filename) = 0 Then
        .Filename = Registry.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\AQUA TERRA Consultants\GenScn\Script", "LastRun")
      End If
      .filter = "Script Files (*.spt)|*.spt|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
      .FilterIndex = 1
      .ShowOpen
      If FileExists(.Filename) Then
        Progress = Progress & vbCr & "  Opening file: " & .Filename
        SaveSetting "GenScn", "Script", "LastRun", .Filename
        Progress = Progress & vbCr & "  ChDriveDir " & PathNameOnly(.Filename)
        ChDriveDir PathNameOnly(.Filename)
        Progress = Progress & vbCr & "  WholeFileString " & .Filename
        scriptString = WholeFileString(.Filename)
      End If
    End With
  End If
  frmGenScn.MousePointer = vbHourglass
  If Len(scriptString) > 0 Then
    Progress = Progress & vbCr & "  SaveActiveCount "
    SaveActiveCount = TserFiles.Active.Count
    Progress = Progress & vbCr & "  With pScriptManager "
    With pScriptManager
      Progress = Progress & vbCr & "  Set script = .ScriptFromString "
      Set script = .ScriptFromString(scriptString)
      If Not .Aborting Then
        If script Is Nothing Then
          MsgBox "Could not load script " & vbCr & err.Description, vbExclamation, "Run Script"
        Else
          Set SelectedColl = frmGenScn.TSerSelected
          ReDim TserArray(SelectedColl.Count)
          For TserIndex = 1 To SelectedColl.Count
            Set TserArray(TserIndex) = SelectedColl(TserIndex)
          Next
          .SetVariable "DataSelected", TserArray
          .SetVariable "PlugInManager", TserFiles
          .SetVariable "InputDir", p.StatusFilePath & "\"
          Debug.Print "GenScn Starting Script: " & vbCr & script.AsText(, "  ")
          .SetVariable "AskingForArgs", 1
          Progress = Progress & vbCr & "  running... "
          .run script, args
          Progress = Progress & vbCr & "  finished running "
          If TserFiles.Active.Count <> SaveActiveCount Then RefreshSLC
        End If
      End If
    End With
  End If
cleanExit:
  ChDriveDir SaveDir
  frmGenScn.MousePointer = vbDefault

  Exit Sub
errhandler:
  If err.Number <> 32755 Then 'If it was not the user cancelling the Select a Script dialog
    MsgBox "Error " & err.Description & " (" & err.Number & ")" & vbCr & vbCr & Progress
  End If
  GoTo cleanExit
End Sub

Sub ShowScriptDocumentation()
  Dim Filename As String
  Dim ff As New ATCoFindFile
  On Error GoTo ErrHand
  
  ff.SetRegistryInfo "Script", "files", "Scripting.chm"
  ff.SetDialogProperties "Please locate script documentation 'Scripting.chm'", _
                          PathNameOnly(App.HelpFile) & "\Scripting.chm"
  Filename = ff.GetName
  
  If Not FileExists(Filename) Then Filename = GetTmpPath & "ScriptDocumentation.txt"
  If Not FileExists(Filename) Then
    'Fall back on automatically generated documentation
    Dim pManager As New ATCScriptManager
    Dim allDocumentation As String
    Dim curDocumentation As String
    Dim curName As Variant
    Dim LibraryNames As New Collection
  
    On Error GoTo ErrHand
  
    LibraryNames.Add "Base"
    LibraryNames.Add "Data"
    LibraryNames.Add "File"
    LibraryNames.Add "Grid"
    LibraryNames.Add "Hspf"
    LibraryNames.Add "Misc"
  
    allDocumentation = "Could not find formatted documentation 'Scripting.chm'" & vbCrLf _
                     & "Generating documentation from script libraries:" & vbCrLf
    For Each curName In LibraryNames
      allDocumentation = allDocumentation & CStr(curName) & ", "
    Next
    allDocumentation = Left(allDocumentation, Len(allDocumentation) - 2) & vbCrLf & vbCrLf
    For Each curName In LibraryNames
      curDocumentation = pManager.LibraryDocumentation("ATCScript" & CStr(curName))
      If InStr(curDocumentation, "Documentation of") > 0 Then
        allDocumentation = allDocumentation & ReplaceString(curDocumentation, vbCr, vbCrLf)
      End If
    Next
  
    SaveFileString Filename, allDocumentation
  End If
  OpenFile Filename, frmGenScn.cdl
  Exit Sub
ErrHand:
  MsgBox "Error generating script documentation:" & vbCr & err.Description, vbOKOnly, "Script Documentation"
End Sub
