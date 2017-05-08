VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestGroupFile"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pFileName As String          'name of group data file
Private pGroupData As FastCollection 'of clsPestGroupData

Public Property Get Filename() As String
  Filename = pFileName
End Property
Public Property Let Filename(ByVal newvalue As String)
  pFileName = newvalue
  PopulateGroupData
End Property

Public Property Get GroupData() As FastCollection
  Set GroupData = pGroupData
End Property
Public Property Let GroupData(ByVal newvalue As FastCollection)
  Set pGroupData = newvalue
End Property

Private Sub PopulateGroupData()
  Dim FUnit As Long, str As String
  Dim lpd As clsPestGroupData

  FUnit = FreeFile(0)
  Open pFileName For Input As #FUnit
  While Not EOF(FUnit)
    Line Input #FUnit, str
    If InStr(str, "#") = 0 Then 'not a comment
      Set lpd = New clsPestGroupData
      lpd.Name = StrRetRem(str)
      lpd.IncType = StrRetRem(str)
      lpd.DerInc = CSng(StrRetRem(str))
      lpd.DILBnd = CSng(StrRetRem(str))
      lpd.ForCen = StrRetRem(str)
      lpd.DIMult = CSng(StrRetRem(str))
      lpd.DerMeth = StrRetRem(str)
      pGroupData.Add lpd
    End If
  Wend
  Close #FUnit
End Sub

Public Function WriteFileAsString()
  Dim str As String
  Dim lgd As Variant

  str = ""
  For Each lgd In pGroupData
    str = str & lgd.Name & Space(22 - Len(lgd.Name)) & _
          lgd.IncType & Space(12 - Len(lgd.IncType)) & _
          NumFmted(lgd.DerInc, 12, 5) & NumFmted(lgd.DILBnd, 12, 5) & "  " & _
          lgd.ForCen & Space(10 - Len(lgd.ForCen)) & _
          NumFmted(lgd.DIMult, 12, 5) & "  " & lgd.DerMeth & vbCrLf
  Next
  WriteFileAsString = Left(str, Len(str) - 2) 'trim last CR/LF
End Function


Private Sub Class_Initialize()
  pFileName = ""
  Set pGroupData = New FastCollection
End Sub
