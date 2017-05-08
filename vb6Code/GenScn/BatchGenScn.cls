VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "BatchGenScn"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private cdl As CommonDialog
Private Recording As Boolean
Private Recorded As String

Private pGraphCaption As String
Private pGraphTitle As String

Public Property Set ComDlg(newComDlg As CommonDialog)
  Set cdl = newComDlg
End Property

Public Sub LogCommand(ByVal command As String)
  Dim lc As String
  If Recording Then
    lc = LCase(command)
    If InStr(lc, "record") = 0 Then Recorded = Recorded & command & vbCr
  End If
End Sub

Public Function Exec(ByVal command As String) As String
  Dim lc As String
  Dim firstWord As String
  Dim nextWord As String
  Dim lastWord As String
  Dim retval As String
  Dim parsePos As Long
  Dim intVal As Integer
  
  On Error GoTo ErrExitExec
  retval = ""
  parsePos = InStr(command, vbCr)
  If parsePos > 0 Then
    lc = StrSplit(command, vbCr, "")
    retval = retval & Exec(lc)
    parsePos = InStr(command, vbCr)
  Else
    lc = LCase(command)
    parsePos = InStr(lc, "#") 'Remove comments
    If parsePos > 0 Then
      lc = Left(lc, parsePos - 1)
      command = Left(command, parsePos - 1)
    End If
    lc = Trim(lc)
    command = Trim(command)
    firstWord = StrSplit(lc, " ", "")
    
    Select Case firstWord
      Case "", "c": LogCommand command  'ignore blank lines and FORTRAN-style comment lines
      
      Case "aggrmethod"
        If IsNumeric(lc) Then
          Me.AggrMethod = lc
        Else
          Select Case lc
            Case "sum", "div", "sum/div":     Me.AggrMethod = 0
            Case "aver", "same", "aver/same": Me.AggrMethod = 1
            Case "max":                       Me.AggrMethod = 2
            Case "min":                       Me.AggrMethod = 3
            Case "native", "none":            Me.AggrMethod = 4
            Case Else: GoTo NonNumeric
          End Select
        End If
        
      Case "aggrstep"
        If IsNumeric(lc) Then Me.AggrStep = lc Else GoTo NonNumeric
        
      Case "aggrunit"
        If IsNumeric(lc) Then
          Me.AggrUnit = lc
        Else
          Select Case lc
            Case "sec", "second", "seconds": Me.AggrUnit = 0
            Case "min", "minute", "minutes": Me.AggrUnit = 1
            Case "hr", "hour", "hours":      Me.AggrUnit = 2
            Case "dy", "day", "days":        Me.AggrUnit = 3
            Case "mon", "month", "months":   Me.AggrUnit = 4
            Case "yr", "year", "years":      Me.AggrUnit = 5
            Case Else: GoTo NonNumeric
          End Select
        End If
      Case "graphaxistype"
        nextWord = StrSplit(lc, " ", "")
        If IsNumeric(lc) Then
          
        Else
          Select Case lc
          
          End Select
        End If
        
        Select Case nextWord
          Case "x"
          Case "y"
          Case "yr", "right"
          Case "aux"
          Case Else
        End Select
        
      Case "graphcaption":   Me.GraphCaption = Trim(Mid(command, Len(firstWord)))
      Case "graphtitle":     Me.GraphTitle = Trim(Mid(command, Len(firstWord)))
      Case "openfile":       nextWord = StrSplit(lc, " ", "")
                             Me.OpenFile nextWord, lc
      Case "msg", "hspfmsg": Me.OpenHspfMsg lc
      Case "msgbox":         MsgBox Exec(lc)
      Case "recordstart":    Recording = True: Recorded = ""
      Case "recordstop":     Recording = False
      Case "recorded":       retval = Recorded
      Case "location"
        nextWord = StrSplit(lc, " ", "")
        Select Case nextWord
          Case "all"
          Case "none", "clear"
          Case Else
        End Select
      Case "selectdata"
        nextWord = StrSplit(lc, " ", "")
        Select Case nextWord
          Case "all"
          Case "dsn"
          Case "none", "clear"
          Case Else
        End Select
      Case Else: retval = command '"Unknown command '" & firstWord & "'."
    End Select
  End If
  Exec = retval
  Exit Function
ErrExitExec:
  retval = "Error Executing '" & command & "'" & vbCr & retval & vbCr & err.Description
  MsgBox retval, vbCritical, "Batch GenScn"
  Exit Function

NonNumeric:
  retval = "Error Executing '" & command & "'" & vbCr & "Numeric values is required"
  MsgBox retval, vbCritical, "Batch GenScn"
  Exit Function

End Function

Public Property Get AggrMethod() As Long
  AggrMethod = frmGenScn.ctlGenDate.TAggr
End Property
Public Property Let AggrMethod(ByVal newValue As Long)
  frmGenScn.ctlGenDate.TAggr = newValue
  LogCommand "AggrMethod " & newValue
End Property

Public Property Get AggrStep() As Long
  AggrStep = frmGenScn.ctlGenDate.TSTEP
End Property
Public Property Let AggrStep(ByVal newValue As Long)
  frmGenScn.ctlGenDate.TSTEP = newValue
  LogCommand "AggrStep " & newValue
End Property

Public Property Get AggrUnit() As Long
  AggrUnit = frmGenScn.ctlGenDate.TUnit
End Property
Public Property Let AggrUnit(ByVal newValue As Long)
  frmGenScn.ctlGenDate.TUnit = newValue
  LogCommand "AggrUnit " & newValue
End Property

Public Property Get GraphCaption() As String
  GraphCaption = pGraphCaption
End Property
Public Property Let GraphCaption(ByVal newValue As String)
  pGraphCaption = newValue
  LogCommand "GraphCaption " & newValue
End Property

Public Property Get GraphTitle() As String
  GraphTitle = pGraphTitle
End Property
Public Property Let GraphTitle(ByVal newValue As String)
  pGraphTitle = newValue
  LogCommand "GraphTitle " & newValue
End Property

Public Function OpenHspfMsg(ByVal Filename As String) As Boolean
  Dim n$, i&, fno As String
  LogCommand "OpenHspfMsg " & Filename
  OpenHspfMsg = True
  If p.HSPFMsg.Unit = 0 Then
    fno = FilenameOnly(Filename) & ".wdm"
    n = Filename
    If Not FileExists(n) Then n = fno
    If Not FileExists(n) Then n = ExePath & "bin\hspfmsg.wdm"
    If Not FileExists(n) Then n = ExePath & fno
    If Not FileExists(n) Then n = ExePath & "bin_data\" & fno
    If Not FileExists(n) Then n = "C:\Program Files\GenScn\hspfmsg.wdm"
    If Not FileExists(n) Then n = "C:\BASINS\models\HSPF\bin\hspfmsg.wdm"
    If Not FileExists(n) Then 'Check directories above the one containing status file
      i = InStrRev(p.StatusFilePath, "\")
      While i > 0 And Not FileExists(n)
        n = Left(p.StatusFilePath, i) & fno
        i = InStrRev(p.StatusFilePath, "\", i - 1)
      Wend
    End If
    If Not FileExists(n) Then
      Dim ff As New ATCoFindFile
      ff.SetDialogProperties "Please locate HSPF message file (" & fno & ")", fno
      ff.SetRegistryInfo AppName, "files", fno
      n = ff.GetName
    End If
    
    If Not FileExists(n) Then
      OpenHspfMsg = False
    Else
      i = 1
      p.HSPFMsg.Unit = F90_WDBOPN(i, n, Len(n))
      If p.HSPFMsg.Unit > 0 Then
        If p.HSPFMsg.Name <> n Then
          p.HSPFMsg.Name = n
        End If
      Else
        OpenHspfMsg = False
      End If
    End If
  End If
End Function

'May be case sensitive on FileType
Public Sub OpenFile(ByVal FileType As String, ByVal Filename As String)
  Dim TserIndex&
  Dim newClsTserFile As ATCclsTserFile
  Dim ErrDesc As String
  LogCommand "OpenFile " & FileType & " " & Filename
  'DbgMsg "OpenStatusFile:ATType" & StrTyp, 3, "frmGenScn", "t"
  TserIndex = TserFiles.AvailIndexByName("clsTSer" & FileType)
  If TserIndex = 0 Then 'not a valid type
    MsgBox FileType & " Read Error: " & TserFiles.ErrorDescription & vbCrLf & "Invalid Type " & FileType, vbExclamation
  Else 'a valid type, create an instance
    TserFiles.Create TserIndex
    'make monitor available
    Set newClsTserFile = TserFiles.CurrentActive.obj
    Set newClsTserFile.Monitor = IPC
    
    'Can't use newClsTserFile here because it doesn't know about msgUnit
    If FileType = "WDM" Then TserFiles.CurrentActive.obj.msgUnit = p.HSPFMsg.Unit

    'read the file
    newClsTserFile.Filename = Filename
    'check read errors
    ErrDesc = newClsTserFile.ErrorDescription
    If Len(ErrDesc) > 0 Then 'had a problem
      MsgBox ErrDesc, vbExclamation, FileType & " Read Error"
      TserFiles.Delete TserFiles.CurrentActiveIndex
    Else
      If FileType = "WDM" Then p.WDMFiles.Add newClsTserFile, "FU" & newClsTserFile.FileUnit
    End If
  End If

End Sub
