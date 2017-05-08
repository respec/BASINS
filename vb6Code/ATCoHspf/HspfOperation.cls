VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "HspfOperation"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private Enum LegendType
  LegLand = 0
  LegMet = 1
  LegPoint = 2
End Enum

Dim pOpTyp As HspfOperType
Dim pId As Long
Dim pDescription As String
Dim pTables As Collection 'of HspfTable
Dim pTableStatus As HspfStatus
Dim pInputTimeseriesStatus As HspfStatus
Dim pOutputTimeseriesStatus As HspfStatus
Dim pSources As Collection 'of HspfConnection
Dim pTargets As Collection 'of HspfConnection
Dim pOpnBlk As HspfOpnBlk
Dim pEdited As Boolean
Dim pFTable As HspfFtable
Dim pMetSeg As HspfMetSeg
Dim pPointSources As Collection 'of HspfPoint
Dim pUci As HspfUci
Dim pComment As String
Dim pSerial As Long
Dim pDefOpnId As Long

Public Property Get EditControlName() As String
  EditControlName = "ATCoHspf.ctlOperationEdit"
End Property

Public Sub Edit()
  'status or hourglass needed here
  editInit Me, pUci.icon, True, True, False
End Sub

Public Property Get Caption() As String
  Caption = "Operation:  " & HspfOperName(pOpTyp) & " " & pId & " - " & pDescription
End Property

Public Property Get Comment() As String
  Comment = pComment
End Property

Public Property Let Comment(newComment As String)
  pComment = newComment
End Property

Public Property Get Edited() As Boolean
  Edited = pEdited
End Property
Public Property Let Edited(newEdited As Boolean)
  pEdited = newEdited
  If newEdited Then pOpnBlk.Edited = True
End Property

Public Property Get optyp() As HspfOperType
  optyp = pOpTyp
End Property
Public Property Let optyp(newOpTyp As HspfOperType)
  pOpTyp = newOpTyp
End Property

Public Property Get Name() As String
Attribute Name.VB_MemberFlags = "200"
  Name = HspfOperName(pOpTyp)
End Property
Public Property Let Name(newName As String)
  pOpTyp = HspfOperNum(newName)
End Property

Public Property Get Id() As Long
  Id = pId
End Property
Public Property Let Id(newId As Long)
  pId = newId
End Property

Public Property Get DefOpnId() As Long
  DefOpnId = pDefOpnId
End Property
Public Property Let DefOpnId(newId As Long)
  pDefOpnId = newId
End Property

Public Property Get Serial() As Long
  Serial = pSerial
End Property

Public Property Get Description() As String
  Description = pDescription
End Property
Public Property Let Description(newDescription As String)
  Dim colonPos&
  pDescription = newDescription
  colonPos = InStr(pDescription, ":")
  If colonPos > 0 Then pDescription = Mid(pDescription, colonPos + 1)
End Property

Public Property Get OpnBlk() As HspfOpnBlk
  Set OpnBlk = pOpnBlk
End Property
Public Property Set OpnBlk(newOpnBlk As HspfOpnBlk)
  Set pOpnBlk = newOpnBlk
End Property

Public Property Get FTable() As HspfFtable
  Set FTable = pFTable
End Property
Public Property Set FTable(newFTable As HspfFtable)
  Set pFTable = newFTable
End Property

Public Property Get MetSeg() As HspfMetSeg
  Set MetSeg = pMetSeg
End Property
Public Property Set MetSeg(newMetSeg As HspfMetSeg)
  Set pMetSeg = newMetSeg
End Property

Public Property Get TableStatus() As HspfStatus
  Set TableStatus = pTableStatus
End Property

Public Property Get InputTimeseriesStatus() As HspfStatus
  Set InputTimeseriesStatus = pInputTimeseriesStatus
End Property

Public Property Get OutputTimeseriesStatus() As HspfStatus
  Set OutputTimeseriesStatus = pOutputTimeseriesStatus
End Property

Public Property Get PointSources() As Collection
  Set PointSources = pPointSources
End Property

Public Property Set PointSources(newPointSource As HspfPoint)
  Set pPointSources = newPointSource
End Property

Public Property Get Tables() As Collection 'of HspfTable
  Set Tables = pTables
End Property

Public Function TableExists(Name$) As Boolean
  Dim vTable As Variant
  
  On Error GoTo NoTable:
  Set vTable = pTables(Name)
  TableExists = True
  Exit Function
NoTable:
  TableExists = False
End Function

Public Property Get Sources() As Collection 'of HspfConnection
  Set Sources = pSources
End Property

Public Property Get Targets() As Collection 'of HspfConnection
  Set Targets = pTargets
End Property

Public Property Set Uci(newUci As HspfUci)
  Set pUci = newUci
End Property
Public Property Get Uci()
  Set Uci = pUci
End Property

Public Sub setTimSerConnections()
  Dim vConn As Variant, lConn As HspfConnection
  Dim idf&, idl&, lName$
  
  lName = HspfOperName(pOpTyp)
  For Each vConn In pUci.Connections
    Set lConn = vConn
    With lConn.Target
      If .VolName = lName Then
        If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
          Set lConn.Target.Opn = Me
          pSources.Add lConn
        End If
      End If
    End With
    With lConn.Source
      If .VolName = lName Then
        If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
          Set lConn.Source.Opn = Me
          pTargets.Add lConn
        End If
      End If
    End With
  Next vConn
End Sub

Public Sub setTimSerConnectionsSources()
  Dim vConn As Variant, lConn As HspfConnection
  Dim idf&, idl&, lName$
  
  lName = HspfOperName(pOpTyp)
  For Each vConn In pUci.Connections
    Set lConn = vConn
    With lConn.Target
      If .VolName = lName Then
        If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
          Set lConn.Target.Opn = Me
          pSources.Add lConn
        End If
      End If
    End With
  Next vConn
End Sub

Public Sub setTimSerConnectionsTargets()
  Dim vConn As Variant, lConn As HspfConnection
  Dim idf&, idl&, lName$
  
  lName = HspfOperName(pOpTyp)
  For Each vConn In pUci.Connections
    Set lConn = vConn
    With lConn.Source
      If .VolName = lName Then
        If .VolId = pId Or (.VolId < pId And .VolIdL >= pId) Then
          Set lConn.Source.Opn = Me
          pTargets.Add lConn
        End If
      End If
    End With
  Next vConn
End Sub

Public Function DownOper(OpType$) As Long
  Dim vConn As HspfConnection
  For Each vConn In pTargets
    If Len(OpType) = 0 Then 'take first one of any type
      DownOper = vConn.Target.VolId
      Exit For
    ElseIf vConn.Target.VolName = OpType Then 'first of selected type
      DownOper = vConn.Target.VolId
      Exit For
    End If
  Next vConn
End Function

Public Sub setPicture(O As Object, ColorMap As Collection, CurrentLegend As Long, Optional LegendOrder As Collection)
  Dim sid&, barPos&, barHeight&, barWidth&, barbase&, maxNBars&, lTemp$
  Dim pic As PictureBox
  Dim str As String, Desc As String, barDesc As Variant
  Dim lSource As HspfConnection
  Dim lDesc As String
  Dim colr As Long
  Dim barMaxVal As Double
  Dim started As Boolean
  Dim included() As Boolean
  
  barWidth = 3
  Set pic = O
'  maxNBars = pUci.OpnBlks("PERLND").Count + pUci.OpnBlks("IMPLND").Count
'  If maxNBars > 10 Then maxNBars = 10
  'pic.Caption = pOpnBlk.Name & " " & pId
  str = pOpnBlk.Name & " " & pId
  pic.ToolTipText = pOpnBlk.Name & " " & pId & " " & pDescription
  'frmPictures.Show
  pic.Picture = LoadPicture    '.picTemp.Picture = .picBlank.Picture
  '.picTemp.Height = pic.Height
  '.picTemp.Width = pic.Width + barWidth * 16 '(pSources.Count + 2)
      
  pic.CurrentX = (pic.Width - pic.TextWidth(str)) / 2
  pic.CurrentY = pic.Height - pic.TextHeight(str) * 1.25
  barbase = pic.CurrentY
  pic.Print str
  'Debug.Print str & " ";
  Select Case CurrentLegend
    Case LegLand
      barMaxVal = pUci.MaxAreaByLand2Stream
      barPos = barWidth
      If LegendOrder Is Nothing Then 'Draw all in the order they fall
        For Each lSource In pSources
          If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
            barHeight = lSource.MFact / barMaxVal * barbase
            On Error GoTo ColorNotFound
            lDesc = lSource.Source.Opn.Description
            colr = ColorMap(lDesc)
            lDesc = ""
            On Error GoTo 0
            pic.Line (barPos, barbase)-Step(barWidth, -barHeight), colr, BF
            barPos = barPos + barWidth + 1
          End If
        Next lSource
      Else 'Draw only land uses in LegendOrder, in order and leaving spaces for ones that do not appear in this segment
        For Each barDesc In LegendOrder
          barHeight = 0
          For Each lSource In pSources
            If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
              If Not lSource.Source.Opn Is Nothing Then
                If lSource.Source.Opn.Description = barDesc Then
                  barHeight = barHeight + lSource.MFact / barMaxVal * barbase
                End If
              End If
            End If
          Next lSource
          If barHeight > 0 Then
            On Error GoTo ColorNotFound
            colr = ColorMap(barDesc)
            On Error GoTo 0
            pic.Line (barPos, barbase)-Step(barWidth, -barHeight), colr, BF
          End If
          barPos = barPos + barWidth + 1
        Next barDesc
      End If
    Case LegMet
      Dim myid As Long
      ReDim included(pUci.MetSegs.Count)
      If Not pMetSeg Is Nothing Then included(pMetSeg.Id) = True: myid = pMetSeg.Id Else myid = 0
      'myid = 0
      For Each lSource In pSources
        If Not lSource.Source.Opn Is Nothing Then
          If Not lSource.Source.Opn.MetSeg Is Nothing Then
            If lSource.Source.Opn.Name <> "RCHRES" Then
              'myid = lSource.Source.Opn.MetSeg.Id
              included(lSource.Source.Opn.MetSeg.Id) = True
            End If
          End If
        End If
      Next
      pic.CurrentX = pic.TextWidth("X")
      pic.CurrentY = (barbase - pic.TextHeight("X")) / 2
      started = False
      For sid = 1 To pUci.MetSegs.Count
        If included(sid) Then
          If started Then pic.Print ", "; Else started = True
          'underline if this met seg contribs to reach directly,
          'dont underline if this met seg contribs to reach only
          'indirectly through land segment
          If sid = myid Then pic.FontUnderline = True Else pic.FontUnderline = False  ' .ForeColor = vbHighlight Else pic.ForeColor = vbButtonText
          pic.Print sid;
        End If
      Next
      pic.FontUnderline = False
    Case LegPoint
      Dim pPoint As HspfPoint
      ReDim included(pUci.PointSources.Count)
      'Debug.Print pPointSources.Count
      For Each pPoint In pPointSources
        included(pPoint.Id) = True
      Next
      pic.CurrentX = pic.TextWidth("X")
      pic.CurrentY = (barbase - pic.TextHeight("X")) / 2
      For sid = 1 To pUci.PointSources.Count
        If included(sid) Then
          If started Then pic.Print ", "; Else started = True
          pic.Print sid;
        End If
      Next
  End Select
  With frmPictures
    If pOpnBlk.Name = "RCHRES" Then
      If pTables("GEN-INFO").Parms("LKFG") = 1 Then 'get the lake picture
        pic.PaintPicture .picLake.Picture, pic.Width - .picLake.Width, 0, , , , , , barbase
      Else
        pic.PaintPicture .picStream.Picture, pic.Width - .picStream.Width, 0, , , , , , barbase
      End If
    ElseIf pOpnBlk.Name = "BMPRAC" Then
      pic.PaintPicture .picBMP.Picture, pic.Width - .picBMP.Width, 0, , , , , , barbase
    Else
      'don't know what picture to use
    End If
  End With
  
  Exit Sub
ColorNotFound:
  lTemp = UCase(lDesc)
  If Len(lTemp) = 0 Then  'changed to use bardesc, pbd
    lTemp = UCase(barDesc)
  End If
  If InStr(lTemp, "FOREST") > 0 Or InStr(lTemp, "WOOD") > 0 Then
    ColorMap.Add ColorMap("FOREST"), lTemp
  ElseIf InStr(lTemp, "AGRI") > 0 Or InStr(lTemp, "FARM") > 0 Then
    ColorMap.Add ColorMap("AGRICULTURAL"), lTemp
  ElseIf InStr(lTemp, "CROP") > 0 Then
    ColorMap.Add ColorMap("AGRICULTURAL"), lTemp
  ElseIf InStr(lTemp, "URBAN") > 0 Or InStr(lTemp, "INDU") > 0 Then
    ColorMap.Add ColorMap("URBAN"), lTemp
  ElseIf InStr(lTemp, "WATER") > 0 Then
    ColorMap.Add ColorMap("WATERWETLANDS"), lTemp
  ElseIf InStr(lTemp, "RESIDENTIAL") > 0 Then
    ColorMap.Add ColorMap("RESIDENTIAL"), lTemp
  Else
    ColorMap.Add vbBlack, lTemp
  End If
  err.Clear
  Resume
End Sub

'Returns color for source.VolId
Private Function IdColor(Id As Long) As Long
  IdColor = RGB(Rnd(-Id - 53) * 255, Rnd(-Id - 27) * 255, Rnd(-Id - 33) * 255)
End Function

'Returns percent (0..1) given a source.VolId and value
'Private Function IdPercentRange(id As Long, Value As Single) As Single
'  If Value < 0 Then Value = -Value
'  If Value < 1 Then
'    IdPercentRange = Value
'  ElseIf Value < 10 Then
'    IdPercentRange = Value / 10
'  ElseIf Value < 100 Then
'    IdPercentRange = Value / 100
'  ElseIf Value < 1000 Then
'    IdPercentRange = Value / 1000
'  ElseIf Value < 10000 Then
'    IdPercentRange = Value / 10000
'  ElseIf Value < 100000 Then
'    IdPercentRange = Value / 100000
'  ElseIf Value < 1000000 Then
'    IdPercentRange = Value / 1000000
'  ElseIf Value < 10000000 Then
'    IdPercentRange = Value / 10000000
'  End If
'End Function

Private Sub Class_Initialize()
  'Debug.Print "init HspfOperation"
  Set pTables = New Collection
  Set pSources = New Collection
  Set pTargets = New Collection
  Set pPointSources = New Collection
  Set pTableStatus = New HspfStatus
  pTableStatus.init Me
  Set pInputTimeseriesStatus = New HspfStatus
  pInputTimeseriesStatus.StatusType = HspfInputTimeseries
  pInputTimeseriesStatus.init Me
  Set pOutputTimeseriesStatus = New HspfStatus
  pOutputTimeseriesStatus.StatusType = HspfOutputTimeseries
  pOutputTimeseriesStatus.init Me
  pId = 0
  pOpTyp = 0
  pDescription = ""
  lastOperationSerial = lastOperationSerial + 1
  pSerial = lastOperationSerial
  pDefOpnId = 0
End Sub
