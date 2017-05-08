VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestParmFile"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFileName As String         'name of parameter data file
Private pParmData As FastCollection 'of clsPestParmData

Public Property Get Filename() As String
  Filename = pFileName
End Property
Public Property Let Filename(ByVal newvalue As String)
  pFileName = newvalue
  PopulateParmData
End Property

Public Property Get ParmData() As FastCollection
  Set ParmData = pParmData
End Property
Public Property Let ParmData(ByVal newvalue As FastCollection)
  Set pParmData = newvalue
End Property

Private Sub PopulateParmData()
  Dim FUnit As Long, str As String
  Dim lpd As clsPestParmData

  FUnit = FreeFile(0)
  Open pFileName For Input As #FUnit
  While Not EOF(FUnit)
    Line Input #FUnit, str
    If InStr(str, "#") = 0 Then 'not a comment
      Set lpd = New clsPestParmData
      lpd.Name = StrRetRem(str)
      lpd.Trans = StrRetRem(str)
      lpd.LimType = StrRetRem(str)
      lpd.InitVal = CSng(StrRetRem(str))
      lpd.LBnd = CSng(StrRetRem(str))
      lpd.UBnd = CSng(StrRetRem(str))
      lpd.Group = StrRetRem(str)
      lpd.Mult = CSng(StrRetRem(str))
      lpd.Offset = CSng(StrRetRem(str))
      lpd.ModelSpecs = str
      pParmData.Add lpd
    End If
  Wend
  Close #FUnit
End Sub

Public Function WriteFileAsString() As String
  Dim str As String
  Dim lpd As Variant

  str = ""
  For Each lpd In pParmData
    str = str & lpd.Name & Space(22 - Len(lpd.Name)) & _
          lpd.Trans & Space(20 - Len(lpd.Trans)) & _
          lpd.LimType & Space(10 - Len(lpd.LimType)) & _
          NumFmted(lpd.InitVal, 12, 5) & NumFmted(lpd.LBnd, 12, 5) & _
          NumFmted(lpd.UBnd, 12, 5) & "   " & lpd.Group & Space(22 - Len(lpd.Group)) & _
          NumFmted(lpd.Mult, 12, 5) & NumFmted(lpd.Offset, 12, 5) & _
          Space(5) & lpd.ModelSpecs & vbCrLf
  Next
  WriteFileAsString = Left(str, Len(str) - 2) 'trim last CR/LF
End Function


Private Sub Class_Initialize()
  pFileName = ""
  Set pParmData = New FastCollection
End Sub
