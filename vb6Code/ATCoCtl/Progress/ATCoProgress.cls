VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCoProgress"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

'The ATCoProgress control opens a small window to show the user progress being
'made on a lengthy task. If the calling program knows what percent of the way
'through a task it is, it may use the Progress property to control the progress bar.
'Otherwise, text messages can be displayed in LabelText(1) and LabelText(2).
'
'In a very processor-intensive operation, updating the progress bar too frequently
'can slow down the computation. In this case it might be desirable to refresh
'the progress property in an outer loop rather than an inner loop or only every Nth
'iteration of the loop.
'
'Pause and resume are handled by the control. When any property or method is
'accessed, the control will wait until the user presses "Resume" or "Cancel"
'if the user has pressed "Pause"
'
'When the user presses "Cancel" Status is changed from "R" (running) to "C"
'(cancelled) It is the responsibility of the program using this control to
'check Status occasionally (perhaps every time progress is updated) and respond
'correctly when the status is "C". When a new task is started, Status must be reset
'to "R" or the new task may exit immediately. WindowOpen resets Status to "R".
'
'Example Use of ATCoProgress:
'
' (acp is the name of an ATCoProgress in this example)
'
' acp.Caption = "Calculating Pi"
' acp.LabelText(1) = "First Stage"
' acp.LabelText(2) = "1"
' acp.LabelText(4) = "515"
' acp.WindowOpen
' for i = 1 to 515
'   acp.LabelText(3) = "Step " & i & " (" & csng(i)/515 & "%)"
'   (misc computation)
'   acp.Progress = csng(i)/515
'   if acp.Status = "C" then goto CalculationCancelled
' next i
' (code to be run if loop completed successfully
' ...
'
'CalculationCancelled:
' (code to be run when calculation was cancelled)
'

Private frm As frmStatusInprocess

'Implement Pause/Resume functionality and make sure display refreshes
Private Sub PossiblyPause()
  Do
    DoEvents
  Loop While frm.Status = "P" 'And frm.WindowState = vbNormal
End Sub

'Caption text appears in window title of form
Public Property Get Caption() As String
  PossiblyPause
  Caption = frm.Caption
End Property

Public Property Let Caption(ByVal NewValue As String)
  PossiblyPause
  frm.Caption = NewValue
End Property

'Status is "R" for running, "P" for paused, or "C" for cancelled
Public Property Get Status() As String
  PossiblyPause
  Status = frm.Status
End Property

Public Property Let Status(ByVal NewValue As String)
  PossiblyPause
  frm.Status = NewValue
End Property

'By default, all three buttons are visible
Public Property Get ButtonVisible(ButtonName$) As Boolean
  PossiblyPause
  Select Case UCase(ButtonName)
    Case "CANCEL":  ButtonVisible = frm.cmdCancel.Visible
    Case "PAUSE":   ButtonVisible = frm.cmdPause.Visible
    Case "DETAILS": ButtonVisible = frm.cmdDetails.Visible
  End Select
End Property

Public Property Let ButtonVisible(ButtonName$, makeVisible As Boolean)
  PossiblyPause
  Select Case UCase(ButtonName)
    Case "CANCEL":  frm.cmdCancel.Visible = makeVisible
    Case "PAUSE":   frm.cmdPause.Visible = makeVisible
    Case "DETAILS": frm.cmdDetails.Visible = makeVisible
  End Select
End Property

'The textbox contains a history of recent status messages
'The Details button expands and contracts the form to make the box visible or not
Public Property Get TextboxVisible() As Boolean
  PossiblyPause
  If frm.txtDetails.Top < frm.Height Then
    TextboxVisible = True
  Else
    TextboxVisible = False
  End If
End Property

Public Property Let TextboxVisible(NewValue As Boolean)
  PossiblyPause
  If frm.WindowState = vbNormal Then
    If NewValue Then
      frm.Height = 6500
      frm.Width = 10000
    Else
      frm.Height = frm.cmdDetails.Top + 1200
      frm.Width = 6000
    End If
  End If
End Property

'Labels on the progress form
'All of these are populated manually at runtime
'1 = top line of progress form
'2 = minimum value displayed at left edge above progress bar
'    (may be used as second line of text if progress bar is not used)
'3 = current value text above middle of progress bar
'4 = maximum value displayed at right edge above progress bar
'Text for min,current,max could be displayed in % or in absolute number
'of steps to completion, or 3 might be best as both
Public Property Get LabelText(labelNum&) As String
  PossiblyPause
  If labelNum >= 1 And labelNum <= 4 Then
    LabelText = frm.lblMsg(labelNum).Caption
  Else
    LabelText = ""
  End If
End Property

Public Property Let LabelText(labelNum&, NewValue As String)
  PossiblyPause
  If labelNum >= 1 And labelNum <= 4 Then
    frm.lblMsg(labelNum).Caption = NewValue
  End If
  If frm.txtDetails.Visible Then
    Dim str
    str = frm.txtDetails.Text & vbCrLf & NewValue
    If Len(str) > 10000 Then
      str = Right(str, 10000)
    End If
    frm.txtDetails.Text = str
    frm.txtDetails.SelStart = Len(str)
  End If
End Property

Public Property Get Progress() As Single
  PossiblyPause
  Progress = frm.ProgressBar1.Value
End Property

'Accepts percent complete as numbers between zero and one
'A newValue of less than zero clears the progress bar and estimated time.
'Use LabelText to set min, max, current labels
Public Property Let Progress(NewValue As Single)
  Static TimeStart As Date, PercentLast As Single
  Dim TimeCurr As Date, TimeDone As Date
  Dim SecondsElapsed As Variant, TotalSeconds&, RemainingSeconds&
  Dim percent As Single
  percent = NewValue
  
  frm.lblMsg(1) = percent
  
  If percent > 1 Then percent = percent / 100
  
  PossiblyPause
  
  If PercentLast > percent Or PercentLast <= 0 Then
    TimeStart = Time
    frm.lblMsg(0) = "Computing Estimate for Completion Time..."
  ElseIf percent > PercentLast Then
    TimeCurr = Time
    SecondsElapsed = DateDiff("s", TimeStart, TimeCurr)
    TotalSeconds = SecondsElapsed / percent
    RemainingSeconds = TotalSeconds - SecondsElapsed
    TimeDone = DateAdd("s", TotalSeconds, TimeStart)
    frm.lblMsg(0) = "Estimate Done at: " & TimeDone
    If RemainingSeconds < 60 Then
      frm.lblMsg(0) = frm.lblMsg(0) & " (" & RemainingSeconds & " seconds)"
    Else
      Dim RemainingMinutes&
      RemainingMinutes = RemainingSeconds / 60
      frm.lblMsg(0) = frm.lblMsg(0) & " (" & RemainingMinutes & " minutes)"
    End If
  End If
        
  If percent >= 0 And percent <= 100 Then
    frm.ProgressBar1.Value = percent * frm.ProgressBar1.Max
    If frm.Visible Then frm.ProgressBar1.Visible = True
  Else
    frm.ProgressBar1.Visible = False
    frm.lblMsg(0) = ""
  End If
  PercentLast = percent

End Property

Public Sub Clear()
  PossiblyPause
  frm.Clear
End Sub

Public Sub WindowClose()
  PossiblyPause
  On Error Resume Next 'Maybe we can't hide because a modal form is already open
  frm.Hide
End Sub

Public Sub WindowOpen()
  PossiblyPause
  On Error GoTo TryModal
  frm.Status = "R"
  frm.Show
  frm.WindowState = vbNormal
  frm.ZOrder 0 'bring to front

  Exit Sub

TryModal:
  If Err.Number = 401 Or Err.Number = 402 Then
    'Can't show form because a modal form is already open
  Else
    MsgBox "Error opening progress window" & vbCr & Err.Description
  End If
End Sub

Private Sub Class_Initialize()
  Set frm = New frmStatusInprocess
End Sub

Private Sub Class_Terminate()
  Unload frm
End Sub
