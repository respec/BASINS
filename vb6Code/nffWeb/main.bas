Attribute VB_Name = "modMain"
Option Explicit

Private Const NconvertPath = "c:\progra~1\XnView\nconvert.exe"

Sub Cgi_Main()
  Dim X As Long
  Dim SubmitPage As String
  Dim ReplyString As String
  Dim Filename As String
  
  Filename = GetCgiValue("filename")
  
  If Len(Filename) > 0 Then
    'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "Cgi_Main CurDir = " & CurDir & " Filename: " & Filename & vbCrLf
    SendImage Filename
  Else
    SendHeader "NFF Web"
    Send "<H1>NFF Web</H1>"
    Send "<H3>This version is for demonstration purposes only. Numbers generated may not be correct!</H3><hr>"
    'Send "<h2>Program temporarily unavailable while new version with units is prepared</h2>"
    Send "<FORM Action = ""/cgi-bin/nffweb.exe"" METHOD=""post"">"
  
    SubmitPage = GetCgiValue("submitpage")
    Select Case LCase(SubmitPage)
      Case "":       ShowSelectState
      Case "state":  ShowRegions
      Case "region": ShowSummary
      Case Else: Send "Don't know how to display next form after " & SubmitPage
    End Select
    Send "</form>"
    
    ' Send pairs
'    Send "<H4>debugging info</H4>"
'    For X = 0 To UBound(tPair)
'        Send Str$(X) & ": " & tPair(X).Name & "=" & tPair(X).Value & "<br>"
'    Next X
'    Send "<hr>"

    Send "<br>"
    SendFooter
  End If
End Sub

Private Sub SendImage(Filename As String)
  Dim SendStr As String
  Dim sImage As String
  Dim inFile As Integer
  Dim StartTime As Single

  On Error GoTo ReportError
  StartTime = Timer
  SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "StartTime " & StartTime & "  " & Time & vbCrLf
  While Len(Dir(Filename & ".bmp")) > 0
    DoEvents
    If Timer - StartTime > 5 Then Exit Sub
  Wend

  inFile = FreeFile
  Open Filename For Binary Access Read As #inFile
  sImage = String(LOF(inFile), Chr$(0))
  Get #inFile, , sImage
  Close #inFile

  Send "Status: 200 OK"
  SendStr = "Content-type: image/" & LCase(Right(Filename, 3)) & vbCrLf
  'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "Sending " & SendStr & vbCrLf
  Send SendStr
  'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "Sending " & Left(sImage, 5) & vbCrLf
  SendB sImage
  Kill Filename

  Exit Sub

ReportError:
  SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "SendImage Error " & Err.Description & vbCrLf
  Err.Clear
  Resume Next
End Sub

Private Sub ShowSelectState()
  Dim retval As String
  Dim member As Variant
  Dim Project As nffProject
  Set Project = New nffProject
  Project.LoadNFFdatabase "C:\httpd\Cgi-Bin\NFF.mdb"

  retval = "<input type=""hidden"" name=""submitpage"" value=""state"">"
  retval = retval & vbCrLf & "State: <select name=""state"" size=""1"">"

  For Each member In Project.DB.States
    retval = retval & vbCrLf & "<option value=""" & member.Name & """>" & member.Name
  Next
  retval = retval & vbCrLf & "</select>"
  retval = retval & vbCrLf & "<input type=""submit"" value=""Create Rural Scenario"">"
  Send retval
End Sub

Private Sub ShowRegions()
  Dim Min As Double
  Dim Max As Double
  Dim retval As String
  Dim State As nffState
  Dim StateString As String
  Dim member As Variant
  Dim param As Variant
  Dim Parameter As nffParameter
  Dim Region As nffRegion
  Dim Project As nffProject
  Set Project = New nffProject
  Project.LoadNFFdatabase "C:\vbExperimental\libNFF\NFF.mdb"

  StateString = GetCgiValue("state")
  On Error GoTo BadState
  Set State = Project.DB.States(StateString)

  retval = "<input type=""hidden"" name=""submitpage"" value=""region"">"
  retval = retval & vbCrLf & "<input type=""hidden"" name=""state"" value=""" & StateString & """>"
  retval = retval & vbCrLf & StateString & " - Enter parameter values for the region(s) in this scenario"
  retval = retval & vbCrLf & "<p><table border=0 cellspacing=10><th>Region/Parameter<th>Value<th>Minimum<th>Maximum"
  For Each member In State.Regions
    Set Region = member
    If Not Region.Urban Then
      retval = retval & vbCrLf & "<tr><td colspan=4><b>" & Region.Name & "</b></tr>" & vbCrLf
      For Each param In Region.Parameters
        Set Parameter = param
        retval = retval & vbCrLf & "<tr><td align=right>" & Parameter.Name & " (" & Parameter.Units.EnglishLabel & ")" _
                                 & "<td><input type=""text"" name=""" & Region.Name & "." & Parameter.Name & """>"
        Min = Parameter.GetMin(False)
        Max = Parameter.GetMax(False)
        If Max > Min Then
          retval = retval & "<td align=right>" & Min & "<td align=right>" & Max
        End If
        retval = retval & "</tr>"
      Next
    End If
  Next
  retval = retval & vbCrLf & "</table>"

  retval = retval & vbCrLf & "<p><input type=""submit"" value=""Next"">"
  Send retval
  Exit Sub

BadState:
  retval = "Error: could not display regions for state " ' & statestring & "'"
  Send retval

End Sub

Private Sub ShowSummary()
  Dim Min As Double
  Dim Max As Double
  Dim sParamVal As String
  Dim ParamVal As Double
  Dim retval As String
  Dim State As nffState
  Dim StateString As String
  Dim member As Variant
  Dim param As Variant
  Dim Parameter As nffParameter
  Dim Region As nffRegion
  Dim RegionNameStarted As String
  Dim newRegion As userRegion
  Dim Scenario As nffScenario
  Dim Project As nffProject
  Set Project = New nffProject
  Set Scenario = New nffScenario
  Project.LoadNFFdatabase "C:\vbExperimental\libNFF\NFF.mdb"
  Set Scenario.Project = Project

  StateString = GetCgiValue("state")
  On Error GoTo BadState
  Set State = Project.DB.States(StateString)

  retval = "<input type=""hidden"" name=""submitpage"" value=""summary"">"
  retval = retval & vbCrLf & "<input type=""hidden"" name=""state"" value=""" & StateString & """>"
  retval = retval & vbCrLf & StateString & " - Summary of this scenario"
  retval = retval & vbCrLf & "<p><table border=0 cellspacing=10><th>Region/Parameter<th>Value<th>Warnings"

  For Each member In State.Regions
    Set Region = member
    For Each param In Region.Parameters
      Set Parameter = param
      sParamVal = Trim(GetCgiValue(Region.Name & "." & Parameter.Name))
      If Len(sParamVal) > 0 Then
        If IsNumeric(sParamVal) Then
          ParamVal = CDbl(sParamVal)
          If newRegion Is Nothing Then
            Set newRegion = New userRegion
            Set newRegion.Region = Region
            Scenario.UserRegions.Add newRegion, Region.Name
            If RegionNameStarted <> Region.Name Then
              RegionNameStarted = Region.Name
              retval = retval & vbCrLf & "<tr><td colspan=3><b>" & RegionNameStarted & "</b></tr>" & vbCrLf
              Scenario.Area = Scenario.Area + ParamVal
            End If
          End If
          retval = retval & vbCrLf & "<tr><td align=right>" & Parameter.Name & " (" & Parameter.Units.EnglishLabel & ")" & "<td align=right>" & sParamVal
          newRegion.UserParms(LCase(Parameter.Name)).SetValue ParamVal, False
          Min = Parameter.GetMin(False)
          Max = Parameter.GetMax(False)
          If Max > Min Then
            If ParamVal > Max Then retval = retval & vbCrLf & "<td><b>Warning: suggested maximum = " & Max & "</b>"
            If ParamVal < Min Then retval = retval & vbCrLf & "<td><b>Warning: suggested minimum = " & Min & "</b>"
          End If
          retval = retval & "</tr>" & vbCrLf
        End If
      End If
    Next
    Set newRegion = Nothing
  Next
  retval = retval & vbCrLf & "</table>"

  retval = retval & vbCrLf & "<p>Total area = " & Scenario.Area & "<p>"


  Dim Flows() As Double
  Dim Years() As Double
  Dim StdError() As Double
  Dim Index As Long
  Dim IntervalText As String
  Dim YearsText As String
  Dim Filename As String

  'retval = retval & vbCrLf & "<p>Computing Discharges for " & Scenario.Name
  Flows = Scenario.Discharges
  'retval = retval & vbCrLf & "<p>Computed Discharges"
  Years = Scenario.EquivalentYears
  'retval = retval & vbCrLf & "<p>Computed Equivalent Years"
  StdError = Scenario.StdError
  'retval = retval & vbCrLf & "<p>Computed Standard Error"

  retval = retval & vbCrLf & "<pre>" & vbCrLf
  retval = retval & "Recurrence     Peak,  Standard  Equivalent" & vbCrLf _
                  & "Interval, yrs   cfs   Error, %  Years"
  For Index = LBound(Flows) To UBound(Flows)
    IntervalText = NumFmted(Scenario.UserRegions(1).Region.Returns(Index).Interval, 7, 1)
    If Right(IntervalText, 2) = ".0" Then IntervalText = Left(IntervalText, Len(IntervalText) - 2) & "  "
    YearsText = NumFmted(Years(Index), 10, 0)
    If Right(YearsText, 1) = "." Then YearsText = Left(YearsText, Len(YearsText) - 1) & "  "
    retval = retval & vbCrLf & IntervalText & "  " _
                             & NumFmted(Flows(Index), 11, 0) _
                             & NumFmted(StdError(Index), 8, 0) _
                             & YearsText
  Next
  If Scenario.RegCrippenBue > 0 Then
    retval = retval & vbCrLf & "maximum: " & NumFmted(Scenario.MaxFloodEnvelope, 11, 0)
  End If

  retval = retval & vbCrLf & "</pre>"
  Send retval: retval = ""
  Filename = ReplaceString(Timer, ".", "") & ".png"
  PlotFreq Scenario, Filename
  Send "<p><img src=""/cgi-bin/nffWeb.exe?filename=" & Filename & """><p>"

  'retval = retval & vbCrLf & "<p><input type=""submit"" value =""Next"">"
  'Send retval
  Exit Sub

BadState:
  retval = retval & vbCrLf & "<H3>Error displaying summary: " & Err.Description & "</h3>"
  Send retval
End Sub

Function NumFmted(ByVal rtmp!, ByVal wid&, ByVal dpla&) As String

  On Error GoTo prob
  'format a number
  Dim fmt$, i&, nspc&, stmp$

  If wid - dpla - 2 > 0 Then
    fmt = String$(wid - dpla - 2, "#") & "0." & String$(dpla, "0") 'force 0.
  Else
    fmt = String$(wid - dpla - 1, "#") & "." & String$(dpla, "0") 'orig way
  End If
  stmp = Format$(rtmp, fmt)
  'add leading blanks
  nspc = wid - dpla - InStr(1, stmp, ".")
  If nspc < 0 Then nspc = 0
  stmp = Space$(nspc) & stmp
  'add trailing blanks
  nspc = wid - Len(stmp)
  If nspc < 0 Then nspc = 0
  NumFmted = stmp & Space$(nspc)
  Exit Function
prob:
  Debug.Print "NumFmted Problem", rtmp, wid, dpla
  NumFmted = String(wid, "#")

End Function

'Private Sub PlotFreq(Scenario As nffScenario)
'  frmDebug.ForeColor = vbBlack
'  frmDebug.DrawMode = vbCopyPen
'  frmDebug.DrawStyle = vbSolid
'  frmDebug.DrawWidth = 10
'  frmDebug.Line (0, 0)-(frmDebug.ScaleWidth, frmDebug.ScaleHeight)
'  SavePicture frmDebug.Image, "graph.bmp"
'End Sub

Private Sub PlotFreq(Scenario As nffScenario, Filename As String)
  Dim agr As ATCoGraph
  'calculate values for flood frequency plot
  Dim i&, pflg&, icrv&, ivar&, ipos&, iret&, cnt&, lNumIntervals As Long
  Dim qmin!, qmax!, tmin!, tmax!
  Dim plmn!(10), plmx!(10), vmin!(40), vmax!(40)
  Dim Ntics&(3), xlab$, ylab$, titl$, capt$
  Dim t() As Double
  Dim q() As Double
  Dim which&(40) 'which axis for each variable
  Dim tran&(40)  'transformation (1=none - arithmetic, 2=log)
  Dim vlab$(40)  'variable label
  Dim clab$(20)  'curve label
  Dim ctype&(20), ltype&(20), stype&(20), lthick&(20), lcolor&(20)
  Dim eqnMetric As Boolean
  Dim BitmapFilename As String

  SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "Set agr = New ATCoGraph " & vbCrLf
  Set agr = New ATCoGraph
  SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "agr.init " & vbCrLf
  agr.init

  qmin = 100000
  qmax = 0
  tmin = 500
  tmax = -500
  ivar = -1
  icrv = -1

  lNumIntervals = Scenario.UserRegions(1).Region.Returns.Count
  q = Scenario.Discharges
  ReDim t(LBound(q) To UBound(q))
  icrv = icrv + 1
  ivar = ivar + 1
  'set x-axis values (probabilities)
  For i = 1 To lNumIntervals
    t(i) = gausex(1 / CDbl(Scenario.UserRegions(1).Region.Returns(i).Interval))
  Next i
  If t(1) < tmin Then tmin = t(1)
  If t(lNumIntervals) > tmax Then tmax = t(lNumIntervals)

  'put x values in plot buffer
  agr.SetData ivar, ipos, lNumIntervals, t(), iret
  vmin(ivar) = t(lNumIntervals)
  vmax(ivar) = t(1)
  which(ivar) = 4
  tran(ivar) = 1
  vlab(ivar) = "Years"
  'update buffer position
  ipos = ipos + lNumIntervals
  'put flow values in plot buffer
  If q(1) < qmin Then qmin = q(1)
  If q(lNumIntervals) > qmax Then qmax = q(lNumIntervals)

  ivar = ivar + 1
  agr.SetData ivar, ipos, lNumIntervals, q(), iret
  vmin(ivar) = q(1)
  vmax(ivar) = q(lNumIntervals)
  which(ivar) = 1
  tran(ivar) = 2
  vlab(ivar) = "Discharge"
  'update buffer position
  ipos = ipos + lNumIntervals
  clab(icrv) = Scenario.Name
  ctype(icrv) = 7
  ltype(icrv) = 1
  stype(icrv) = 0
  lthick(icrv) = 1
  lcolor(icrv) = (icrv + 9) Mod 15
  If lcolor(icrv) = 7 Or lcolor(icrv) = 15 Then lcolor(icrv) = 8 'White -> Gray

  agr.SetVars icrv, ivar, ivar - 1
  'set min/max Y-axis range
  Call Scalit(2, qmin, qmax, plmn(0), plmx(0))
  Call Scalit(4, tmin, tmax, plmn(3), plmx(3))
  agr.SetNumVars icrv + 1, ivar + 1
  agr.SetScale plmn(), plmx(), Ntics()
  agr.SetCurveInfo ctype, ltype, lthick, stype, lcolor, clab
  agr.SetVarInfo vmin, vmax, which, tran, vlab
  'set axes types and labels
  xlab = "Recurrence Interval, in years"
  'ylab = "Peak Discharge"
  'If Project.metric Then
  '  ylab = "Peak Discharge, in cubic meters per second"
  'Else
    ylab = "Peak Discharge, in cubic feet per second"
  'End If
  titl = "Flood Frequency Plot"
  capt = "Frequency Plot"
  'set x-axis to probability, y-axis to log
  agr.SetAxesInfo 4, 2, 0, 0, xlab, ylab, "", ""
  agr.SetTitles titl, capt
  'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "agr.ShowIt " & vbCrLf
  agr.ShowIt
  'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "SavePicture " & vbCrLf
  If Len(Dir(Filename)) > 0 Then Kill Filename
  BitmapFilename = Filename & ".bmp"
  SavePicture agr.GraphForm.scrGraph.Image, BitmapFilename
  Shell NconvertPath & " -D -o " & Filename & " -out png " & BitmapFilename
  'SaveFileString "c:\nfflog.txt", WholeFileString("c:\nfflog.txt") & "SavedPicture " & vbCrLf
End Sub

Public Function gausex(exprob!) As Single

  'GAUSSIAN PROBABILITY FUNCTIONS   W.KIRBY  JUNE 71
     'GAUSEX=VALUE EXCEEDED WITH PROB EXPROB
     'GAUSAB=VALUE (NOT EXCEEDED) WITH PROBCUMPROB
     'GAUSCF=CUMULATIVE PROBABILITY FUNCTION
     'GAUSDY=DENSITY FUNCTION
  'SUBPGMS USED -- NONE

  'GAUSCF MODIFIED 740906 WK -- REPLACED ERF FCN REF BY RATIONAL APPRX N
  'ALSO REMOVED DOUBLE PRECISION FROM GAUSEX AND GAUSAB.
  '76-05-04 WK -- TRAP UNDERFLOWS IN EXP IN GUASCF AND DY.

  'rev 8/96 by PRH for VB

  Const c0! = 2.515517
  Const c1! = 0.802853
  Const c2! = 0.010328
  Const d1! = 1.432788
  Const d2! = 0.189269
  Const d3! = 0.001308
  Dim pr!, rtmp!, p!, t!, numerat!, denom!

  p = exprob
  If p >= 1# Then
    'set to minimum
    rtmp = -10#
  ElseIf p <= 0# Then
    'set at maximum
    rtmp = 10#
  Else
    'compute value
    pr = p
    If p > 0.5 Then pr = 1# - pr
    t = (-2# * Log(pr)) ^ 0.5
    numerat = (c0 + t * (c1 + t * c2))
    denom = (1# + t * (d1 + t * (d2 + t * d3)))
    rtmp = t - numerat / denom
    If p > 0.5 Then rtmp = -rtmp
  End If
  gausex = rtmp

End Function

