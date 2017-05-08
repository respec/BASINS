VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestPAR2PAR"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFileName As String          'name of PAR2PAR file
Private pParmData As FastCollection  'use parm name as key for collection of expressions
Private pTemplates As FastCollection 'use model input file as key for collection of templates
Private pPrecis As String            '"single"(default) or "double"
Private pDPoint As String            '"point"(default) or "nopoint"

Public Property Get Filename() As String
  Filename = pFileName
End Property
Public Property Let Filename(ByVal newvalue As String)
  pFileName = newvalue
  PopulatePAR2PAR
End Property

Public Property Get ParmData() As FastCollection
  Set ParmData = pParmData
End Property
Public Property Let ParmData(ByVal newvalue As FastCollection)
  Set pParmData = newvalue
End Property

Public Property Get Templates() As FastCollection
  Set Templates = pTemplates
End Property
Public Property Let Templates(ByVal newvalue As FastCollection)
  Set pTemplates = newvalue
End Property

Public Property Get Precis() As String
  Precis = pPrecis
End Property
Public Property Let Precis(ByVal newvalue As String)
  pPrecis = newvalue
End Property

Public Property Get DPoint() As String
  DPoint = pDPoint
End Property
Public Property Let DPoint(ByVal newvalue As String)
  pDPoint = newvalue
End Property

Private Sub PopulatePAR2PAR()
  Dim FUnit As Integer, section As Integer, ipos As Integer
  Dim str As String, var1 As String

  FUnit = FreeFile(0)
  Open pFileName For Input As #FUnit
  While Not EOF(FUnit)
    Line Input #FUnit, str
    str = Trim(str)
    If Len(str) > 0 Then
      If Left(str, 1) = "*" Then 'changing section
        If InStr(LCase(str), "parameter") > 0 Then 'parameter data section
          section = 1
        ElseIf InStr(LCase(str), "template") > 0 Then 'template and model input section
          section = 2
        ElseIf InStr(LCase(str), "control") > 0 Then
          section = 3
        End If
      ElseIf section = 1 Then 'look for "="
        ipos = InStr(str, "=")
        If ipos > 0 Then
          pParmData.Add Mid(str, ipos), Left(str, ipos - 1)
        End If
      Else 'continue processing current section
        var1 = StrRetRem(str)
        If section = 2 Then
          pTemplates.Add var1, str 'use model input file as key
        Else
          pPrecis = var1
          pDPoint = str
        End If
      End If
    End If
  Wend

End Sub

Public Function WriteFileAsString(Template As Boolean) As String
'Template indicates whether to write the file as a
'data file (Template=False) or as a template file (Template=True)
  Dim i As Long
  Dim str As String
  Dim lv As Variant

  If Template Then 'indicate marker ($) to define what gets filled in with data values
    str = "ptf $" & vbCrLf
  Else
    str = ""
  End If
  str = str & "* parameter data"
  For i = 1 To pParmData.Count
    str = str & vbCrLf & pParmData.Key(i) & " " & pParmData.ItemByIndex(i)
  Next i
  str = str & vbCrLf & vbCrLf & "* template and model input files"
  For i = 1 To pTemplates.Count
    str = str & vbCrLf & pTemplates.ItemByIndex(i) & " " & pTemplates.Key(i)
  Next
  str = str & vbCrLf & vbCrLf & "* control data"
  str = str & vbCrLf & pPrecis & " " & pDPoint
  WriteFileAsString = str

End Function

Private Sub Class_Initialize()
  pFileName = ""
  Set pParmData = New FastCollection
  Set pTemplates = New FastCollection
  pPrecis = "single"
  pDPoint = "point"
End Sub

Public Function Valid(Prop As String) As FastCollection
  Dim lProp As String
  Dim lColl As FastCollection

  Set lColl = Nothing
  lProp = LCase(Prop)
  If lProp = "precis" Or lProp = "dpoint" Then
    Set lColl = New FastCollection
    Select Case lProp
      Case "precis"
        lColl.Add "single"
        lColl.Add "double"
      Case "dpoint"
        lColl.Add "point"
        lColl.Add "nopoint"
    End Select
  End If
  Set Valid = lColl
End Function
