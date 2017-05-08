VERSION 5.00
Begin VB.Form frmInterpolate 
   Caption         =   "Interpolate"
   ClientHeight    =   1710
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4830
   LinkTopic       =   "Form1"
   ScaleHeight     =   1710
   ScaleWidth      =   4830
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkTS 
      Caption         =   "chkTS"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Index           =   0
      Left            =   480
      TabIndex        =   2
      Top             =   600
      Value           =   1  'Checked
      Width           =   4095
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2520
      TabIndex        =   1
      Top             =   1200
      Width           =   1335
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "&Interpolate"
      Default         =   -1  'True
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   960
      TabIndex        =   0
      Top             =   1200
      Width           =   1335
   End
   Begin VB.Label Label1 
      Caption         =   "Use times from:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   3
      Top             =   240
      Width           =   2175
   End
End
Attribute VB_Name = "frmInterpolate"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

'Slop factor for use in testing equality of floating point dates
Const JulianSecond = 1# / 86400# 'One day divided by (24 hours * 3600 seconds per hour)

Private nNewFeq&
Private oldFilIndex&()
Private oldNval&()

'de-allocate any added data structures we created to hold interpolated dates
Public Sub DeallocateModifiedTSer()
  Dim curFEQ&, curTS&, c&
  
  If nNewFeq > 0 Then
    For curTS = 0 To nts - 1
      TSer(curTS).FilIndex = oldFilIndex(curTS)
      TSer(curTS).NVal = oldNval(curTS)
    Next curTS
    
    For curFEQ = 1 To nNewFeq
      With p.FeoData(p.FeoCount - 1)
        'For c = 0 To .LocCount - 1
        '  ReDim .LocInfo(c).Constit(0)
        'Next c
        ReDim .LocInfo(0)
        ReDim .JDay(0)
      End With
    Next curFEQ
    
    p.FeoCount = p.FeoCount - nNewFeq
    ReDim Preserve p.FeoData(p.FeoCount - 1)
    nNewFeq = 0
  End If
  ModifiedTSer = False
End Sub

'Takes n timeseries in array ts
'When finished, timeseries values have been interpolated
' so each timser has the same number of values at the same times
' Only values in the overlapping time span are retained.
Private Sub InterpolateTimeseries(nts&, ts() As Timser)
  Dim curTS&, chkTS&, nextIndex&, jd&
  Dim found As Boolean
  Dim JSdt#, JEdt#, OverlapStart#, OverlapEnd#
  Dim Timbuf() As Double
  Dim nDates&
  Dim ConstInt As Boolean
  Dim r&, c&
  Dim nOldFeq&, oldTimCount&
  Dim curTime#, oldTime1#, oldTime2#
  Dim oldVal1#, oldVal2#
  Dim newVals!()

  For curTS = 0 To nts - 1
    If ts(curTS).Type <> "FEQ" Then
      MsgBox "Interpolate does not yet support non-FEQ data"
      Exit Sub
    End If
  Next curTS
  
  If nNewFeq > 0 Then DeallocateModifiedTSer
  
  ReDim oldFilIndex(nts)
  ReDim oldNval(nts)
  For curTS = 0 To nts - 1
    oldFilIndex(curTS) = ts(curTS).FilIndex
    oldNval(curTS) = ts(curTS).NVal
  Next curTS
  
  nOldFeq = p.FeoCount
  'get the data values for selected data sets
  ConstInt = False
  ModifiedTSer = False
  Call FillTimSerExt(nts, ts(), Timbuf(), ConstInt)
  ModifiedTSer = True
  'For c = 0 To nts - 1
  '  'agd.ColType(c) = ATCoSng
  '  agd.ColTitle(c) = TSer(c).id
  '  For r = 1 To TSer(c).NVal
  '    agd.TextMatrix(r, c) = Timbuf(r - 1, c)
  '  Next r
  'Next c
  
  ReDim Preserve p.FeoData(p.FeoCount)
  p.FeoCount = p.FeoCount + 1
  CopyFeqdata p.FeoData(ts(0).FilIndex), p.FeoData(p.FeoCount - 1), True, False
  ts(0).FilIndex = p.FeoCount - 1
  BuildNewDateArray nts, ts, Timbuf, nDates, p.FeoData(p.FeoCount - 1).JDay
  p.FeoData(p.FeoCount - 1).TimCount = nDates
  'For r = 1 To nDates
  '  agd.TextMatrix(r, c) = p.FeoData(p.FeoCount - 1).JDay(r - 1)
  'Next r
  
  For curTS = 1 To nts - 1
    found = False
    For chkTS = 0 To curTS - 1
      If oldFilIndex(chkTS) = ts(curTS).FilIndex Then
        found = True
        ts(curTS).FilIndex = ts(chkTS).FilIndex
        chkTS = curTS
      End If
    Next chkTS
    If Not found Then 'add new feodata to project
      ReDim Preserve p.FeoData(p.FeoCount)
      p.FeoCount = p.FeoCount + 1
      CopyFeqdata p.FeoData(ts(curTS).FilIndex), p.FeoData(p.FeoCount - 1), True, False
      ReDim p.FeoData(p.FeoCount - 1).JDay(nDates - 1)
      For jd = 0 To nDates - 1
        p.FeoData(p.FeoCount - 1).JDay(jd) = p.FeoData(ts(0).FilIndex).JDay(jd)
      Next jd
      p.FeoData(p.FeoCount - 1).TimCount = nDates
      ts(curTS).FilIndex = p.FeoCount - 1
    End If
  Next curTS
  
  ReDim newVals(nDates)
  For curTS = 0 To nts - 1
    With p.FeoData(oldFilIndex(curTS))
      curTime = p.FeoData(p.FeoCount - 1).JDay(0)
      oldTimCount = .TimCount
      oldTime2 = .JDay(1)
      nextIndex = 2
      For jd = 0 To nDates - 1
        curTime = p.FeoData(p.FeoCount - 1).JDay(jd)
        
        'skip to overlapping date
        While oldTime2 < curTime And nextIndex < oldTimCount
          oldTime2 = .JDay(nextIndex)
          nextIndex = nextIndex + 1
        Wend
        
        oldTime1 = .JDay(nextIndex - 2)
        oldVal1 = ts(curTS).Vals(nextIndex - 2)
        oldVal2 = ts(curTS).Vals(nextIndex - 1)
        
        If Abs(curTime - oldTime1) < JulianSecond Then     'close enough to this time
          newVals(jd) = oldVal1
        ElseIf Abs(curTime - oldTime2) < JulianSecond Then 'close enough to that time
          newVals(jd) = oldVal2
        ElseIf (oldTime2 - oldTime1) > 0.0000001 Then      'interpolate value
          newVals(jd) = oldVal1 + (curTime - oldTime1) * (oldVal2 - oldVal1) / (oldTime2 - oldTime1)
        Else 'if they happen at the same time, take average
          newVals(jd) = (oldVal1 + oldVal2) / 2
        End If
      Next jd
      ts(curTS).NVal = nDates
      ReDim ts(curTS).Vals(nDates - 1)
      For jd = 0 To nDates - 1
        ts(curTS).Vals(jd) = newVals(jd)
      Next jd
    End With
  Next curTS
  nNewFeq = p.FeoCount - nOldFeq
End Sub

'Builds an array of julian dates starting with the latest start time in timbuf and ending with the earliest end time in timbuf
'If a time is less than a second from the previous time, it is not used
Private Sub BuildNewDateArray(nts&, ts() As Timser, Timbuf#(), nDates&, newDates() As Double)
  Dim curTS&            'counts through the set of timeseries in ts
  Dim nextIndex&()      'keeps track of next unused time value in each ts
  Dim nextNewDateIndex& 'location in newDates array where next date will be appended
  Dim nextDate#         'Julian (day.fractionofday) date of potential next value for newDates
  Dim thisDate#         'date we are checking as potential nextDate
  Dim maxDates&         'size of newDates array
  Dim prevDate#         'Julian (day.fractionofday) date of previous value for newDates
  
  'Initialize nextIndex(), find latest start date and largest date array
  nextDate = 0
  ReDim nextIndex(nts)
  For curTS = 0 To nts - 1
    nextIndex(curTS) = 0
    
    thisDate = Timbuf(0, curTS)
    If thisDate > nextDate Then nextDate = thisDate
    
    If ts(curTS).NVal > maxDates Then maxDates = ts(curTS).NVal
  Next curTS
  prevDate = nextDate
  
  'Skip dates before last start date
  For curTS = 0 To nts - 1
    While Timbuf(nextIndex(curTS), curTS) < nextDate
      nextIndex(curTS) = nextIndex(curTS) + 1
      If nextIndex(curTS) >= ts(curTS).NVal Then GoTo NoOverlap
    Wend
  Next curTS
  
  maxDates = maxDates * 2 'Start with twice as much room as largest ts
  ReDim newDates(maxDates)

  'To keep going after finding earliest end time:
  'Uncomment the 2 lines "If nextIndex(curTS) >= 0 Then" and their "End If"s
  'Change "Then nextDate = 0" to "Then nextIndex(curTS) = -1"
  Do
    DoEvents
    nextDate = 0
    For curTS = 0 To nts - 1
      thisDate = Timbuf(nextIndex(curTS), curTS)
      If nextDate = 0 Or thisDate < nextDate Then
        nextDate = thisDate
      End If
    Next curTS
    If nextDate > 0 Then
      
      If nDates = 0 Or nextDate - prevDate >= JulianSecond Then
        'Put new date in array
        If nDates >= maxDates Then
          maxDates = maxDates * 2
          ReDim Preserve newDates(maxDates)
        End If
        newDates(nDates) = nextDate
        nDates = nDates + 1
      End If
      
      'Increment pointer(s) to this date
      For curTS = 0 To nts - 1
        While nextDate > 0 And Abs(nextDate - Timbuf(nextIndex(curTS), curTS)) < JulianSecond
          nextIndex(curTS) = nextIndex(curTS) + 1
          If nextIndex(curTS) >= ts(curTS).NVal Then nextDate = 0
        Wend
      Next curTS
      prevDate = nextDate
    End If
    
  Loop While nextDate > 0
  ReDim Preserve newDates(nDates - 1) 'Shrink array down to exact size needed
  Exit Sub
NoOverlap:
  MsgBox "The selected timeseries do not overlap in time, so they cannot be interpolated."
  nDates = 0
  ReDim newDates(0)
End Sub

'Copy simple variables, not arrays LocInfo and JDay
Private Sub CopyFeqdata(src As FeqData, dst As FeqData, copyLocInfo As Boolean, copyJDay As Boolean)
  dst.EJday = src.EJday
  dst.ItemPerRec = src.ItemPerRec
  dst.LeftItemCnt = src.LeftItemCnt
  dst.LocCount = src.LocCount
  dst.NameFeo = src.NameFeo
  dst.NameFtf = src.NameFtf
  dst.NameTsd = src.NameTsd
  dst.NumbFullRec = src.NumbFullRec
  dst.RecLen = src.RecLen
  dst.Scenario = src.Scenario
  dst.SJday = src.SJday
  dst.Term = src.Term
  dst.TimCount = src.TimCount
  dst.version = src.version
  If copyLocInfo Then
    Dim Loc&
    ReDim dst.LocInfo(src.LocCount - 1)
    For Loc = 0 To src.LocCount - 1
      dst.LocInfo(Loc) = src.LocInfo(Loc)
    Next Loc
  End If
  If copyJDay Then
    Dim jd&
    ReDim dst.JDay(src.TimCount - 1)
    For jd = 0 To src.TimCount - 1
      dst.JDay(jd) = src.JDay(jd)
    Next jd
  End If
End Sub

Private Sub cmdOk_Click()
  Me.MousePointer = vbHourglass
  InterpolateTimeseries nts, TSer
  Me.MousePointer = vbDefault
  Unload Me
End Sub

Private Sub Command1_Click()
  Unload Me
End Sub

Private Sub Form_Load()
  Dim ChkNum&
  
  If nts > 1 Then
    
    For tsnum = 0 To nts - 1
      If tsnum > 0 Then
        Load chkTS(tsnum)
        chkTS(tsnum).Top = chkTS(0).Top + 240 * nextChkNum
      End If
      With TSer(tsnum)
        chkTS(tsnum).Caption = .Stanam & "," & .Con & " (" & .NVal & ")"
      End With
    Next tsnum
    
    cmdOk.Top = chkTS(tsnum - 1).Top + 350
    cmdCancel.Top = cmdOk.Top
    Height = cmdOk.Top + 915
    'Dim c&
    'agd.Cols = nts + 1
    'agd.ColTitle(nts) = "All"
    'For c = 0 To nts - 1
    '  agd.ColTitle(c) = TSer(c).NVal
    'Next c
  Else
    MsgBox "There must be at least two timeseries to interpolate"
  End If
End Sub

Private Sub Form_Resize()
  'If Height > 1035 Then agd.Height = Height - 1035
  'If Width > 135 Then agd.Width = Width - 135
End Sub
