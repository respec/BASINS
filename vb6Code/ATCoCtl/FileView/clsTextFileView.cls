VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "clsTextFileView"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit
Implements clsFileView

Private frmPar As Form       'parent form (frmFileView)

Public Function clsFileView_Desc() As String
  clsFileView_Desc = "Text"
End Function

Public Sub clsFileView_SaveChanges()
  MsgBox "TextFileView:SaveChanges"
End Sub

Public Sub clsFileView_OpenFile(frmParent As Form)
  Set frmPar = frmParent
  Open frmPar.Filename For Input As #frmPar.FileHandle
  frmPar.AddOption "Line Break", ""
  frmPar.AddOption "Page Break", ""
  frmPar.AddOption "Page Length", ""
  'CalcFilePos
End Sub

'Returns first byte position in file that should be displayed to scroll the specified amount
'If any parameter is zero, no scrolling means frmPar.FirstDispByte will be returned unchanged
'Direction: -1 = back toward beginning of file, 1 = moving toward end of file
'Units: 1=byte, 2=line, 3=page, 4=logical units of file (same as page if no logical units)
'Distance: number of units to scroll. If beyond end of file, scrolls to end of file
Public Function clsFileView_ScrollPos(ByVal Direction&, ByVal Units&, ByVal Distance&) As Long
  Dim retval& 'return value
  Dim line&, page&, unit&   'counter to count up/down to Distance
  Dim lineperpage&
  Dim cbuff$                'character buffer for reading lines of file
  Dim FirstByte&, NextByte& 'first and last byte just read from file
  Dim EOLstring$, LenEOL&, CheckEOL As Boolean
  Dim EOPstring$, LenEOP&, CheckEOP As Boolean, EOPfound As Boolean
  
  SetPageParams lineperpage, EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
  
  If Direction * Units * Distance = 0 Then ' no scrolling necessary, return frmPar.FirstDispByte
    retval = frmPar.FirstDispByte
  Else    'Figure out how far to move display pointer
    Seek frmPar.FileHandle, frmPar.FirstDispByte
    Select Case Units
      Case 1: 'byte
        retval = frmPar.FirstDispByte + Direction * Distance
      Case 2: 'line
        If Direction = 1 Then 'line forward
          line = 1
          While line <= Distance And Not EOF(frmPar.FileHandle)
            ReadLine cbuff, NextByte, EOPfound, _
                     EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
            line = line + 1
          Wend
          retval = NextByte
        Else 'line back toward file start
                  
        End If
      Case 3, 4: 'page
        If Direction = 1 And frmPar.NextDispByte < frmPar.BytesInFile Then 'page forward
          retval = frmPar.NextDispByte
          Seek frmPar.FileHandle, retval
          Distance = Distance - 1
          While Distance > 0 And Not EOF(frmPar.FileHandle)
            line = 1
            While line <= lineperpage And Not EOF(frmPar.FileHandle)
              ReadLine cbuff, NextByte, EOPfound, _
                       EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
              If EOPfound Then GoTo NextPage
              line = line + 1
            Wend
NextPage:
            Distance = Distance - 1
            retval = NextByte
          Wend
        ElseIf Direction = -1 And frmPar.FirstDispByte > 1 Then
          'page back toward file start
        
        End If
    End Select
  End If

  'Make sure return value is sane
  If retval > frmPar.BytesInFile Then retval = frmPar.BytesInFile
  If retval < 1 Then retval = 1

  clsFileView_ScrollPos = retval
End Function

Private Sub SetPageParams(ByRef lineperpage&, _
  ByRef EOLstring$, ByRef LenEOL&, ByRef CheckEOL As Boolean, _
  ByRef EOPstring$, ByRef LenEOP&, ByRef CheckEOP As Boolean)
  
  Dim opt As clsOption
  
  frmPar.ParseDelimiterOption "Line Break", EOLstring, LenEOL, CheckEOL
  frmPar.ParseDelimiterOption "Page Break", EOPstring, LenEOP, CheckEOP
  
  lineperpage = (frmPar.txtMain.Height - 700) / (1# * frmPar.TextHeight("W"))
  Set opt = frmPar.Options("Page Length")
  If opt.Valid Then
    If IsNumeric(opt.Value) Then lineperpage = opt.Value
  End If
  If lineperpage < 1 Then lineperpage = 1

End Sub

Private Sub ReadLine(ByRef cbuff$, ByRef NextByte&, ByRef EOPfound As Boolean, _
  EOLstring$, LenEOL&, CheckEOL As Boolean, _
  EOPstring$, LenEOP&, CheckEOP As Boolean)
  
  Dim srch&
  
  Line Input #frmPar.FileHandle, cbuff
  NextByte = Seek(frmPar.FileHandle)
  If CheckEOL Then
    srch = InStr(cbuff, EOLstring)
    If srch > 0 Then
      Seek frmPar.FileHandle, NextByte - Len(cbuff) + srch
      NextByte = NextByte - Len(cbuff) + srch
      cbuff = Left(cbuff, srch - 1) & vbCrLf
    End If
  End If
  
  EOPfound = False
  If CheckEOP Then
    srch = InStr(cbuff, EOPstring)
    If srch > 0 Then
      EOPfound = True
      Seek frmPar.FileHandle, NextByte - Len(cbuff) + srch
      NextByte = NextByte - Len(cbuff) + srch
      cbuff = Left(cbuff, srch - 1)
    End If
  End If
      
End Sub

Public Sub clsFileView_DispPage(ByVal linePrefix$)
  Dim cbuff$, tbuff$, lineperpage&, linecnt&, NextByte&
  Dim EOLstring$, LenEOL&, CheckEOL As Boolean
  Dim EOPstring$, LenEOP&, CheckEOP As Boolean, EOPfound As Boolean
  
  SetPageParams lineperpage, EOLstring, LenEOL, CheckEOL, EOPstring, LenEOP, CheckEOP
  
  If frmPar.FirstDispByte > frmPar.BytesInFile Then frmPar.SetFirstDispByte frmPar.BytesInFile - 100
  If frmPar.FirstDispByte < 1 Then frmPar.SetFirstDispByte 1
  
  Seek frmPar.FileHandle, frmPar.FirstDispByte
  tbuff = ""
  EOPfound = False
  linecnt = 0
  While Not EOF(frmPar.FileHandle) And linecnt <= lineperpage And Not EOPfound
  
    ReadLine cbuff, NextByte, EOPfound, _
      EOLstring, LenEOL, CheckEOL, _
      EOPstring, LenEOP, CheckEOP
      
    tbuff = tbuff & linePrefix & cbuff & vbCrLf
    linecnt = linecnt + 1
  Wend
  frmPar.txtMain.Text = Left(tbuff, Len(tbuff) - 2) 'strip final vbCrLf
  frmPar.SetNextDispByte NextByte
End Sub

Public Function clsFileView_CanOpen(Filename As String) As Boolean
  clsFileView_CanOpen = True
End Function

Public Sub clsFileView_Find(SearchFor As String, Direction As Long)

End Sub

