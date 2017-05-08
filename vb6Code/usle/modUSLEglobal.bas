Attribute VB_Name = "modUSLEglobal"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Global GotSoil As Boolean, NewHUC As Boolean
Global FirstRow As Byte, LastRow As Byte
Global LandUseNames() As String
Global NumLandUses As Long
Global SoilDBfile As String
Global DefaultSaveFile As String
Global MapDir As String
Global CurHUCs As Collection
Global MapName As String
Global MapExtent As Rectangle, MapExtentSet As Boolean
Global Const AppRegName = "TMDL USLE"
Public DRElems() As Single

Public Enum ColNumEnum
  HUCCOL = 0
  LandUseCOL
  SoilSeriesCOL
  SoilTextureCOL
  KrangeCOL
  KCOL
  RCOL
  InclineCOL
  LengthCOL
  LSCOL
  CCOL
  PCOL
  FieldRateCOL
  AreaCOL
  FieldCOL
  DRCOL
  StreamCol
End Enum

Sub Main()
  Dim butt&, LastSheet&, i&, ShowHelp As Boolean
  Dim DefaultMap As String, DefaultSoils As String, Msg As String
  
  Set CurHUCs = New Collection
  DefaultMap = "C:\Program Files\TMDL USLE\MapData\SedTran.map"
  DefaultSoils = "C:\Program Files\TMDL USLE\soils.mdb"
  'MapName = "C:\USLE\mapdata\SedTran.Map"
  'SoilDBfile = "C:\USLE\soils.mdb"
    
  MapName = GetSetting(AppRegName, "Defaults", "MapName", DefaultMap)
  SoilDBfile = GetSetting(AppRegName, "Defaults", "SoilDB", DefaultSoils)
  
  'Working around a bug that should be fixed in Setup1
  'where on Win98 but not NT we get \\ intead of \ in a path we put in the registry
  MapName = ReplaceString(MapName, "\\", "\")
  SoilDBfile = ReplaceString(SoilDBfile, "\\", "\")
  
  If Len(Dir(MapName)) = 0 Then
    frmTable.cdlg.DialogTitle = "Please locate map file"
    frmTable.cdlg.Filename = MapName
    frmTable.cdlg.ShowOpen
    MapName = frmTable.cdlg.Filename
    SaveSetting AppRegName, "Defaults", "MapName", MapName
  End If
  
  If Len(Dir(SoilDBfile)) = 0 Then
    frmTable.cdlg.DialogTitle = "Please locate soil database"
    frmTable.cdlg.Filename = "soils.mdb"
    frmTable.cdlg.ShowOpen
    SoilDBfile = frmTable.cdlg.Filename
    SaveSetting AppRegName, "Defaults", "SoilDB", SoilDBfile
  End If
  
  MapDir = PathNameOnly(MapName) & "\"
  App.HelpFile = PathNameOnly(SoilDBfile) & "\USLE.chm"

  ReDim DRElems(4, 1)
  ReDim LandUseNames(50)
  i = 0
  i = i + 1: LandUseNames(i) = "Agriculture"
  i = i + 1: LandUseNames(i) = "Construction Sites"
  i = i + 1: LandUseNames(i) = "Disturbed Forest"
  i = i + 1: LandUseNames(i) = "Forest"
  i = i + 1: LandUseNames(i) = "Landfills"
  i = i + 1: LandUseNames(i) = "Military Training"
  i = i + 1: LandUseNames(i) = "Mining Sites"
  i = i + 1: LandUseNames(i) = "Parks"
  i = i + 1: LandUseNames(i) = "Permanent Grass"
  i = i + 1: LandUseNames(i) = "Urban Areas"
  ReDim Preserve LandUseNames(i)
  NumLandUses = i
  
  'get reach info from database
  frmTable.Show
  ShowHelp = False
  DefaultSaveFile = PathNameOnly(SoilDBfile) & "\USLEsave.txt"
  If Len(Dir(DefaultSaveFile)) > 0 Then
    frmTable.LoadFile DefaultSaveFile, False
    i = MsgBox("Would you like to view the Getting Started Help before working with the TMDL USLE Table?", vbYesNo + vbDefaultButton2 + vbQuestion, "TMDL USLE")
    If i = vbYes Then ShowHelp = True
  Else
    ShowHelp = True
  End If
  If ShowHelp Then
    'frmTable.OpenHelp 1
    Msg = OpenFile(App.HelpFile, frmTable.cdlg)
    If Msg = "No application found for this file" Then
      MsgBox "Unable to open TMDL USLE's help system." & vbCrLf & _
             "Internet Explorer 5.0 or newer is required for viewing TMDL USLE's help." & vbCrLf & _
             "The help system is an essential part of TMDL USLE, thus it may not be run." & vbCrLf & _
             "To download the latest version of Internet Explorer, go to " & vbCrLf & _
             "http://www.microsoft.com/windows/ie/download/ie55.htm.", vbExclamation, "TMDL USLE Problem"
      Dim frm As Form
      For Each frm In Forms
        Unload frm
      Next
    End If
  End If
End Sub

Private Sub GetSoils()
  Dim i As Integer
  Dim mydb As Database, qdfHUC As QueryDef
  Dim SQL$, Areas$
  
  If CurHUCs.Count > 0 Then
    frmSoilRead.Show
    frmTable.MousePointer = vbHourglass
    Areas = "huc_muid.HUC IN ("
    For i = 1 To CurHUCs.Count
      Areas = Areas & CurHUCs(i) & ", "
    Next i
    Areas = Areas & ")"
    Set mydb = OpenDatabase(SoilDBfile, , True)
    On Error Resume Next
    If Len(Dir(SoilDBfile)) > 0 Then mydb.Execute "DROP TABLE [HUCData];"
    SQL = "SELECT DISTINCT huc_muid.HUC, SoilNames.NAME, MUID_Text_K.TEXTURE, " & _
          "MUID_Text_K.KFFACT INTO HUCData IN '" & SoilDBfile & "' " _
          & "FROM (huc_muid INNER JOIN SoilNames ON huc_muid.MUID = SoilNames.MUID) " & _
          "INNER JOIN MUID_Text_K ON SoilNames.MUID = MUID_Text_K.MUID " & _
          "WHERE " & Areas
    Set qdfHUC = mydb.CreateQueryDef("", SQL)
    qdfHUC.Execute
    qdfHUC.Close
    mydb.Close
    GotSoil = True
    frmTable.MousePointer = vbDefault
    frmSoilRead.Hide
  End If
End Sub

' Calculate the L Factor
Public Function TopoFactor(ByVal Slope As Double, ByVal Length As Long, ByVal Coeff As Double) As Double
  Dim Angle As Double, SFactor As Double, LFactor As Double, _
      M As Double, b As Double
  Dim Freeze As Boolean
  
  Angle = (Atn(Slope / 100))  ' define angle in radians
  
'  Select Case LandUse 'Need to add more land uses here
'    Case "Permanent Grass", "Rangeland":                                       Coeff = 0.5
'    Case "Agriculture", "Disturbed Forest", "Parks", "Urban Areas":            Coeff = 1
'    Case "Construction Sites", "Mining Sites", "Landfills", "Landslide Areas": Coeff = 2
'    Case Else
'      MsgBox "Agriculture or similar land use assumed in LS computation." & vbCrLf & _
'      "See Help for LS Factor for more information.", vbInformation Or vbMsgBoxHelpButton, "TMDL USLE", App.HelpFile, 21
'      Coeff = 1
''      TopoFactor = 0
''      Exit Function
'  End Select
  b = (Sin(Angle) / 0.0896) / (3 * (Sin(Angle)) ^ 0.8 + 0.56)
  M = Coeff * b / (1 + Coeff * b)
  If Slope < 9 And Length < 15 Then Length = 15
  LFactor = (Length / 72.6) ^ M

  ' Calculate the S Factor
  If Length >= 15 Then
    If Slope < 9 Then
      SFactor = 10.8 * Sin(Angle) + 0.03
    ElseIf Freeze = False Then
      SFactor = 16.8 * Sin(Angle) - 0.5
    Else  ' Following equation for areas with significant freeze/thaw cycles
      SFactor = (Sin(Angle) / 0.0896) ^ 0.6
    End If
  Else
    If Slope < 9 Then
      SFactor = 10.8 * Sin(Angle) + 0.03
    Else
      LFactor = (15 / 72.6) ^ M
      SFactor = 3 * (Sin(Angle)) ^ 0.8 + 0.56
      'TopoFactor = 10 ^ (Log10(LFactor * SFactor) + (0.078 * Log10(Length) - 0.037))
      TopoFactor = LFactor * (((16.8 * Sin(Angle) - 0.5) - SFactor) * (Length - 3) / 12 + SFactor)
    End If
  End If
  If TopoFactor = 0 Then
    TopoFactor = LFactor * SFactor
  End If
End Function

'Calculate edge of field and edge of stream loads
Public Function DoSedCalcs(agd As ATCoGrid, row As Long) As String

  Dim lrow As Long, total As Single

  With agd
    If IsNumeric(.TextMatrix(row, KCOL)) And _
       IsNumeric(.TextMatrix(row, RCOL)) And _
       IsNumeric(.TextMatrix(row, LSCOL)) And _
       IsNumeric(.TextMatrix(row, CCOL)) And _
       IsNumeric(.TextMatrix(row, PCOL)) Then 'do edge of field ratecalc
       .TextMatrix(row, FieldRateCOL) = Format( _
         CSng(.TextMatrix(row, KCOL)) * _
         CSng(.TextMatrix(row, RCOL)) * _
         CSng(.TextMatrix(row, LSCOL)) * _
         CSng(.TextMatrix(row, CCOL)) * _
         CSng(.TextMatrix(row, PCOL)), "0.##")
    End If
    If IsNumeric(.TextMatrix(row, FieldRateCOL)) And _
       IsNumeric(.TextMatrix(row, AreaCOL)) Then
      .TextMatrix(row, FieldCOL) = CLng( _
        CSng(.TextMatrix(row, FieldRateCOL)) * _
        CSng(.TextMatrix(row, AreaCOL)))
    End If
    If IsNumeric(.TextMatrix(row, FieldCOL)) And _
       IsNumeric(.TextMatrix(row, DRCOL)) Then
       .TextMatrix(row, StreamCol) = CLng( _
         CSng(.TextMatrix(row, FieldCOL)) * _
         CSng(.TextMatrix(row, DRCOL)))
    End If
    total = 0
    For lrow = 1 To .rows
      If IsNumeric(.TextMatrix(lrow, StreamCol)) Then
        total = total + CDbl(.TextMatrix(lrow, StreamCol))
      End If
    Next
  End With
  DoSedCalcs = CLng(total)
End Function
'
'
'Static Function Log10(x)
'   Log10 = Log(x) / Log(10#)
'End Function

Public Sub HUCsFromMap(Map1 As ATCoMap)
  Dim i&, NewHUCs() As String
  Set CurHUCs = Nothing
  Set CurHUCs = New Collection
  If Map1.SelCount > 0 Then
    Map1.GetSelectedKeys NewHUCs
    For i = LBound(NewHUCs) To UBound(NewHUCs)
      If IsNumeric(NewHUCs(i)) Then CurHUCs.Add NewHUCs(i)
    Next i
  End If
  GetSoils
End Sub

Public Sub HUCsToMap(Map1 As ATCoMap)
  Dim ThisHUC As Variant
  Map1.CurLayer = 1
  Map1.SelectAll 1, False
  For Each ThisHUC In CurHUCs
    Map1.SetSelectedByKey 1, ThisHUC, True
  Next
End Sub

Public Sub HUCsFromTable(agd As ATCoGrid)
  Dim row&
  Set CurHUCs = Nothing
  Set CurHUCs = New Collection
  For row = 1 To agd.rows
    If IsNumeric(agd.TextMatrix(row, HUCCOL)) Then _
      CurHUCs.Add agd.TextMatrix(row, HUCCOL)
  Next
  GetSoils
End Sub

Public Sub HUCsToTable(agd As ATCoGrid)
  ' This sub searches through HUC-8s in table, removes deselected HUC-8s from HUCs()
  ' then adds newly selected HUC-8s to that array.
  ' If there are new HUC-8s, then the HUCData table is rebuilt.
  'Dim ChartRange As Range
  Dim row&, col&, delRow&
  Dim ThisHUCstr As String, TestHUCstr As Variant
  Dim ThisHUC As Long
  
  row = 1
  With agd
    ' Run remove HUC-8s in table that are not in CurHUCs
    While row <= .rows
      ThisHUCstr = .TextMatrix(row, HUCCOL)
      If IsNumeric(ThisHUCstr) Then  ' A HUC-8 has been found in the table
        ThisHUC = CLng(ThisHUCstr)
        ' Check if it is a deselected HUC-8
        For Each TestHUCstr In CurHUCs
          If ThisHUCstr = TestHUCstr Then GoTo x ' HUC-8 already in HUCs()
        Next
        delRow = row
        If .rows > 1 Then
          row = row - 1
          Do
            .DeleteRow delRow
            delRow = delRow + 1
          Loop While delRow <= .rows And Len(.TextMatrix(delRow, HUCCOL)) < 7
        Else
          For col = 0 To .cols - 1
            .TextMatrix(1, col) = ""
          Next
        End If
x:    End If
      row = row + 1
    Wend
    
    'Add HUC rows fo ones not already in table
    For Each TestHUCstr In CurHUCs
      If .RowContaining(TestHUCstr, HUCCOL) < 1 Then
        If .TextMatrix(1, HUCCOL) = "" Then
          .TextMatrix(1, HUCCOL) = TestHUCstr
        Else
          .TextMatrix(.rows + 1, HUCCOL) = TestHUCstr
        End If
      End If
    Next
'    On Error GoTo y
'    If NewHUC Then
'      GetSoils HUCs()  ' Creates new table with soils from HUC-8s in HUCs()
'    ElseIf Len(.TextMatrix(1, HUCCOL)) < 7 Then
'      ReDim HUCs(0)  ' Chart is empty so reset HUCs() to null
'      HUCs(0) = 0
'    End If
  End With
y:
End Sub

