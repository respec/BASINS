VERSION 5.00
Object = "*\A..\ATCoCtl\ATCoCtl.vbp"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmTable 
   Caption         =   "TMDL USLE"
   ClientHeight    =   6012
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   11592
   Icon            =   "frmTable.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   6012
   ScaleWidth      =   11592
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox pctEPALogo 
      BorderStyle     =   0  'None
      Height          =   612
      Left            =   10920
      Picture         =   "frmTable.frx":0CFA
      ScaleHeight     =   612
      ScaleWidth      =   612
      TabIndex        =   13
      Top             =   60
      Width           =   612
   End
   Begin MSComDlg.CommonDialog cdlReport 
      Left            =   7080
      Top             =   240
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
      CancelError     =   -1  'True
      DialogTitle     =   "Save Report As..."
      Filter          =   "Text (*.txt)|*.txt|All (*.*)|*.*"
   End
   Begin VB.CommandButton cmdReport 
      Height          =   372
      Left            =   1080
      Picture         =   "frmTable.frx":11E2
      Style           =   1  'Graphical
      TabIndex        =   2
      Tag             =   "Save"
      ToolTipText     =   "Write Report"
      Top             =   20
      Width           =   372
   End
   Begin MSComDlg.CommonDialog cdlg 
      Left            =   9960
      Top             =   0
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
   End
   Begin VB.CommandButton cmdSave 
      Height          =   372
      Left            =   600
      Picture         =   "frmTable.frx":127A
      Style           =   1  'Graphical
      TabIndex        =   1
      Tag             =   "Save"
      ToolTipText     =   "Save Table"
      Top             =   20
      Width           =   372
   End
   Begin VB.CommandButton cmdLoad 
      Height          =   372
      Left            =   120
      Picture         =   "frmTable.frx":13C4
      Style           =   1  'Graphical
      TabIndex        =   0
      Tag             =   "Load"
      ToolTipText     =   "Load Saved Table"
      Top             =   20
      Width           =   372
   End
   Begin VB.CommandButton cmdErase 
      Height          =   372
      Left            =   3000
      Picture         =   "frmTable.frx":1806
      Style           =   1  'Graphical
      TabIndex        =   3
      ToolTipText     =   "Erase Current Row"
      Top             =   20
      Width           =   372
   End
   Begin VB.Frame fraTotal 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   372
      Left            =   6000
      TabIndex        =   10
      Top             =   5640
      Width           =   5532
      Begin VB.TextBox txtTotal 
         Height          =   288
         HelpContextID   =   25
         Left            =   3960
         Locked          =   -1  'True
         TabIndex        =   9
         Top             =   0
         Width           =   1572
      End
      Begin VB.Label lbl 
         Caption         =   "Total Edge of Stream Sediment Load (tons/yr):"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   252
         Left            =   0
         TabIndex        =   11
         Top             =   0
         Width           =   4212
      End
   End
   Begin VB.CommandButton cmdHelp 
      Height          =   372
      Left            =   3480
      Picture         =   "frmTable.frx":1C48
      Style           =   1  'Graphical
      TabIndex        =   7
      Tag             =   "Help"
      ToolTipText     =   "Assistance"
      Top             =   20
      Width           =   372
   End
   Begin VB.CommandButton cmdReset 
      Height          =   372
      Left            =   1560
      Picture         =   "frmTable.frx":202B
      Style           =   1  'Graphical
      TabIndex        =   6
      ToolTipText     =   "Clear Table"
      Top             =   20
      Width           =   372
   End
   Begin VB.CommandButton cmdAddLand 
      Height          =   372
      Left            =   2520
      Picture         =   "frmTable.frx":246D
      Style           =   1  'Graphical
      TabIndex        =   5
      Tag             =   "Land"
      ToolTipText     =   "Add New Land Segment"
      Top             =   20
      Width           =   372
   End
   Begin VB.CommandButton cmdMap 
      Height          =   372
      Left            =   2040
      Picture         =   "frmTable.frx":2D37
      Style           =   1  'Graphical
      TabIndex        =   4
      Tag             =   "Streams"
      ToolTipText     =   "Select HUC-8s From Map"
      Top             =   20
      Width           =   372
   End
   Begin ATCoCtl.ATCoGrid agdTable 
      Height          =   5052
      Left            =   120
      TabIndex        =   8
      Top             =   480
      Width           =   11412
      _ExtentX        =   20130
      _ExtentY        =   8911
      SelectionToggle =   0   'False
      AllowBigSelection=   0   'False
      AllowEditHeader =   0   'False
      AllowLoad       =   0   'False
      AllowSorting    =   0   'False
      Rows            =   2
      Cols            =   17
      ColWidthMinimum =   300
      gridFontBold    =   0   'False
      gridFontItalic  =   0   'False
      gridFontName    =   "MS Sans Serif"
      gridFontSize    =   8
      gridFontUnderline=   0   'False
      gridFontWeight  =   400
      gridFontWidth   =   0
      Header          =   "Table for Calculating Aggregate Sediment Load from Specified Land Segments"
      FixedRows       =   3
      FixedCols       =   0
      ScrollBars      =   3
      SelectionMode   =   0
      BackColor       =   -2147483643
      ForeColor       =   -2147483640
      BackColorBkg    =   -2147483632
      BackColorSel    =   -2147483635
      ForeColorSel    =   -2147483634
      BackColorFixed  =   -2147483633
      ForeColorFixed  =   -2147483630
      InsideLimitsBackground=   -2147483643
      OutsideHardLimitBackground=   8421631
      OutsideSoftLimitBackground=   8454143
      ComboCheckValidValues=   0   'False
   End
   Begin VB.Label lblHelp 
      Caption         =   "<-Help with current column"
      Height          =   492
      Left            =   3960
      TabIndex        =   12
      Top             =   0
      Width           =   6492
   End
End
Attribute VB_Name = "frmTable"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim OpeningHelp As Boolean

Private Sub AddRow(Optional HUC& = 0)
  '  This subroutine adds a new HUC-8, land use, or soil type as specified by user.
  '  New HUCs are addended to the end of the table.
  '  New land uses and soil types are inserted where specified.
  '  Sub DropDown then called to fill in the appropriate drop-down lists.
  'Dim ChartRange As Range
  Dim NewRow&, col&
  
  With agdTable
    '  Find location of new row
    If HUC > 0 Then
      If Len(.TextMatrix(1, HUCCOL)) < 7 Then
        NewRow = 1
      Else
        NewRow = .rows + 1
      End If
      .TextMatrix(NewRow, HUCCOL) = HUC
    Else  ' Adding a new land use or soil type
      NewRow = .row + 1
      If NewRow <= .rows Then
        .InsertRow .row
      End If
      For col = 0 To .cols - 1
        .TextMatrix(NewRow, col) = ""
      Next
    End If
  End With
End Sub

Private Function HUCfromRow(row As Long) As Long
  Dim r&, txt As String
  r = row
  While r > 0
    txt = agdTable.TextMatrix(r, HUCCOL)
    If Len(txt) > 6 Then
      HUCfromRow = CLng(txt)
      Exit Function
    End If
    r = r - 1
  Wend
End Function

Public Function LandUsefromRow(Optional row As Long) As String
  Dim r&, txt As String
  If IsMissing(row) Then row = agdTable.row
  r = row
  While r > 0
    txt = agdTable.TextMatrix(r, LandUseCOL)
    If Len(txt) > 0 Then
      LandUsefromRow = txt
      Exit Function
    End If
    r = r - 1
  Wend
End Function

Private Function SoilNamefromRow(row As Long) As String
  Dim r&, txt As String
  r = row
  While r > 0
    txt = agdTable.TextMatrix(r, SoilSeriesCOL)
    If Len(txt) > 0 Then
      SoilNamefromRow = txt
      Exit Function
    End If
    r = r - 1
  Wend
End Function

'Private Sub agdTable_Click()
'  If Not agdTable.ColEditable(agdTable.col) Then cmdHelp_Click
'End Sub

Private Sub agdTable_CommitChange(ChangeFromRow As Long, ChangeToRow As Long, ChangeFromCol As Long, ChangeToCol As Long)
  Dim mydb As Database
  Set mydb = OpenDatabase(SoilDBfile, , True)
  Dim row As Long, col As Long, total As Double
  
  With agdTable
    For col = ChangeFromCol To ChangeToCol
      For row = ChangeFromRow To ChangeToRow
        Select Case col
          Case SoilSeriesCOL, SoilTextureCOL
               Kvalues row, mydb
          
'          Case LandUseCOL, InclineCOL, LengthCOL 'Calculate LS factor from Agriculture Handbook 703 p109
               
          Case KCOL, RCOL, LSCOL, CCOL, PCOL, AreaCOL, FieldCOL, DRCOL
            txtTotal.Text = CStr(DoSedCalcs(agdTable, row))
'            If IsNumeric(.TextMatrix(row, KCOL)) And _
'               IsNumeric(.TextMatrix(row, RCOL)) And _
'               IsNumeric(.TextMatrix(row, LSCOL)) And _
'               IsNumeric(.TextMatrix(row, CCOL)) And _
'               IsNumeric(.TextMatrix(row, PCOL)) And _
'               IsNumeric(.TextMatrix(row, AreaCOL)) Then
'               .TextMatrix(row, FieldCOL) = CSng(.TextMatrix(row, KCOL)) * _
'                                            CSng(.TextMatrix(row, RCOL)) * _
'                                            CSng(.TextMatrix(row, LSCOL)) * _
'                                            CSng(.TextMatrix(row, CCOL)) * _
'                                            CSng(.TextMatrix(row, PCOL)) * _
'                                            CSng(.TextMatrix(row, AreaCOL))
'            End If
          
'          Case FieldCOL, DRCOL
'            If IsNumeric(.TextMatrix(row, FieldCOL)) And _
'               IsNumeric(.TextMatrix(row, DRCOL)) Then
'               .TextMatrix(row, StreamCol) = CSng(.TextMatrix(row, FieldCOL)) * _
'                                             CSng(.TextMatrix(row, DRCOL))
'            End If
            
        End Select
      Next row
    Next col
'    total = 0
'    For row = 1 To .rows
'      If IsNumeric(.TextMatrix(row, StreamCol)) Then
'        total = total + CDbl(.TextMatrix(row, StreamCol))
'      End If
'    Next
'    txtTotal.Text = total
  End With
  mydb.Close
End Sub

Private Sub agdTable_RowColChange()
  Static OldCol As Long, OldRow As Long
  Dim i&
  Dim ThisHUC As Long
  Dim ThisSoilName As String
  Dim mydb As Database, SoilRec As Recordset
  Dim SQL As String
  
  On Error GoTo ErrMessage
  
  With agdTable
'    If OldCol = LandUseCOL Then 'need to do LS calc
'      If IsNumeric(.TextMatrix(OldRow, InclineCOL)) And _
'         IsNumeric(.TextMatrix(OldRow, LengthCOL)) And _
'         Len(.TextMatrix(OldRow, OldCol)) > 0 Then
'        .TextMatrix(OldRow, LSCOL) = TopoFactor(.TextMatrix(OldRow, OldCol), .TextMatrix(OldRow, InclineCOL), .TextMatrix(OldRow, LengthCOL))
'      End If
'    End If
    .ClearValues
    Select Case .col
      Case LandUseCOL: HelpContextID = 29
        lblHelp.Caption = "Double click for available list or enter user-defined type" & vbCrLf & "<- Help with defining Land Use"
        For i = 1 To NumLandUses
          .addValue LandUseNames(i)
        Next
      Case SoilSeriesCOL: HelpContextID = 19
        lblHelp.Caption = "Double click for available list or enter user-defined type" & vbCrLf & "<- Help with defining Soil Series"
        If GotSoil Then
          ThisHUC = HUCfromRow(.row)
          Set mydb = OpenDatabase(SoilDBfile, , True)
          SQL = "SELECT DISTINCT HUCData.NAME FROM HUCData WHERE HUC=" & ThisHUC
          Set SoilRec = mydb.OpenRecordset(SQL, dbOpenDynaset)
          If SoilRec.AbsolutePosition = -1 Then  ' There is no such HUC-8
              MsgBox "The HUC-8 '" & ThisHUC & "' listed in row " & .row & vbCr & _
                      "Was not found in the soils database:" & vbCr & SoilDBfile, _
                      vbOKOnly, "No such HUC-8"
          Else
            While Not SoilRec.EOF
              .addValue SoilRec(0)
              SoilRec.MoveNext
            Wend
            SoilRec.Close
            mydb.Close
          End If
        End If
      Case SoilTextureCOL: HelpContextID = 19
        lblHelp.Caption = "Double click for available list" & vbCrLf & "<- Help with defining Soil Texture"
        If GotSoil Then
          ThisSoilName = SoilNamefromRow(.row)
          Set mydb = OpenDatabase(SoilDBfile, , True)
          SQL = "SELECT DISTINCT TEXTURE FROM HUCData " & _
                "WHERE NAME='" & ThisSoilName & "';"
          Set SoilRec = mydb.OpenRecordset(SQL, dbOpenDynaset)
          While Not SoilRec.EOF
            .addValue SoilRec(0)
            SoilRec.MoveNext
          Wend
          SoilRec.Close
          mydb.Close
        End If
      Case HUCCOL:       HelpContextID = 10: lblHelp.Caption = vbCrLf & "<- Access map for defining Hydrologic Units"
      Case KrangeCOL:    HelpContextID = 19: lblHelp.Caption = "Range of Recorded K Factors is not editable" & vbCrLf & "<- Help with Range of Recorded K Factors"
      Case KCOL:         HelpContextID = 19: lblHelp.Caption = "Use Help for other K Factor estimation tool" & vbCrLf & "<- Help with K Factor"
      Case RCOL:         HelpContextID = 20: lblHelp.Caption = "Use Help for map of R Factors" & vbCrLf & "<- Help with R Factor"
      Case InclineCOL:   HelpContextID = 21: lblHelp.Caption = "User-entered value" & vbCrLf & "<- Help with Slope Incline"
      Case LengthCOL:    HelpContextID = 21: lblHelp.Caption = "User-entered value" & vbCrLf & "<- Help with Slope Length"
      Case LSCOL:        HelpContextID = 21: lblHelp.Caption = "LS value generated from Incline and Length values, no further assistance tools available" & vbCrLf & "<- Help with LS Factor"
      Case CCOL:         HelpContextID = 22: lblHelp.Caption = "No estimation tools available for C Factor" & vbCrLf & "<- Help with C Factor"
      Case PCOL:         HelpContextID = 23: lblHelp.Caption = "Use Help for P Factor estimation tool" & vbCrLf & "<- Help with P Factor"
      Case FieldRateCOL: HelpContextID = 18: lblHelp.Caption = "Edge of Field Rate is not editable" & vbCrLf & "<- Help with Edge of Field Rate"
      Case AreaCOL:      HelpContextID = 29: lblHelp.Caption = "User-entered value" & vbCrLf & "<- Help with Area"
      Case FieldCOL:     HelpContextID = 18: lblHelp.Caption = "Edge of Field Load is not editable" & vbCrLf & "<- Help with Edge of Field Load"
      Case DRCOL:        HelpContextID = 26: lblHelp.Caption = "Use Help for Delivery Ratio estimation tools" & vbCrLf & "<- Help with Delivery Ratio"
      Case StreamCol:    HelpContextID = 25: lblHelp.Caption = "Edge of Stream Load is not editable" & vbCrLf & "<- Help with Edge of Stream Load"
      Case Else:         HelpContextID = 1
    End Select
    OldCol = .col
    OldRow = .row
  End With
  If OpeningHelp Then HelpContextID = 1: OpeningHelp = False
  Exit Sub
ErrMessage:
  MsgBox Err.Description, vbOKOnly, "TMDL USLE Error"
End Sub

Public Sub OpenHelp(Optional HelpContext As Long)
  If Not IsMissing(HelpContext) Then Me.HelpContextID = HelpContext
  Me.SetFocus
  OpeningHelp = True
  SendKeys "{F1}"
  Debug.Print "ShowHelp: " & HelpContextID
End Sub


Private Sub cmdHelp_Click()
  Dim RowDesc As String
  Dim Parm1 As Single, Parm2 As Single
  Parm1 = -999
  Parm2 = -999
  With agdTable
    RowDesc = "Row " & .row & _
              ": HUC " & HUCfromRow(.row) & _
              ", " & LandUsefromRow(.row) & _
              ", " & SoilNamefromRow(.row)

    Select Case .col
      Case HUCCOL: cmdMap_Click
      'Case LandUseCOL:     SendKeys "{F1}"
      'Case SoilSeriesCOL:  SendKeys "{F1}"
      'Case SoilTextureCOL: SendKeys "{F1}"
      'Case KrangeCOL:      SendKeys "{F1}"
      Case KCOL:
        frmKFact.Caption = "K Factor for " & RowDesc
        frmKFact.TableRow = .row
        frmKFact.Show
      Case RCOL:
        frmRMaps.Caption = "R Factor for " & RowDesc
        frmRMaps.TableRow = .row
        frmRMaps.Show
      'Case InclineCOL:     SendKeys "{F1}"
      'Case LengthCOL:      SendKeys "{F1}"
      Case LSCOL:
        frmLS.Caption = "LS Factor for " & RowDesc
        frmLS.Show
      
'      Case CCOL:
'        frmMisc.HelpContextID = Me.HelpContextID
'        frmMisc.Caption = "C Factor for " & RowDesc
'        frmMisc.cmdMore.Caption = "More on C"
'        frmMisc.lbl.Caption = "No interactive estimation tools exist for the C Factor." & vbCrLf & "Click the More button for C Factor Help."
'        frmMisc.Show
      Case PCOL:
        If IsNumeric(.TextMatrix(.row, InclineCOL)) Then Parm1 = CSng(.TextMatrix(.row, InclineCOL))
        If IsNumeric(.TextMatrix(.row, LengthCOL)) Then Parm2 = CSng(.TextMatrix(.row, LengthCOL))
        frmPFact.TableRow = .row
        frmPFact.LSlope = Parm1
        frmPFact.LTerInt = Parm2
        frmPFact.Caption = "P Factor for " & RowDesc
        frmPFact.Show
      'Case AreaCOL:        SendKeys "{F1}"
      'Case FieldCOL:       SendKeys "{F1}"
      Case DRCOL:
        frmDR.Caption = "Delivery Ratio for " & RowDesc
        frmDR.TableRow = .row
        frmDR.Show
      'Case StreamCol:      SendKeys "{F1}"
      Case Else:           SendKeys "{F1}"
    End Select
    
  End With

End Sub

Private Sub cmdLoad_Click()
  Static thisSaveFilename As String
  If thisSaveFilename = "" Then thisSaveFilename = DefaultSaveFile
  LoadFile thisSaveFilename, True
End Sub

Public Sub LoadFile(Filename As String, ShowFileDialog As Boolean)
  Dim i&
  If Len(agdTable.TextMatrix(1, HUCCOL)) = 0 Then
    i = vbYes
  Else
    i = MsgBox("Loading a saved file will clear the current table values." & vbCrLf & "Are you sure you want to clear the whole table?", vbYesNo, "TMDL USLE")
    If i = vbYes Then 'clear 1st HUC so that cmdReset won't ask for confirmation
      agdTable.TextMatrix(1, HUCCOL) = ""
    End If
  End If
  If i = vbYes Then
    cmdReset_Click
    If Len(agdTable.TextMatrix(1, HUCCOL)) = 0 Then 'decided to clear table
      If ShowFileDialog Then
        cdlg.Filename = Filename
        cdlg.DialogTitle = "Load previously saved values"
        cdlg.ShowOpen
        Filename = cdlg.Filename
      End If
      If Len(Filename) > 0 Then
        agdTable.LoadFile "#", False, False, Filename, vbTab, "", "'"
        DoEvents
        HUCsFromTable agdTable
      End If
    End If
  End If
End Sub

Private Sub cmdMap_Click()
  Me.MousePointer = vbHourglass
  frmMap.Show
  frmMap.MousePointer = vbHourglass
  HUCsToMap frmMap.Map1
  Me.MousePointer = vbDefault
  frmMap.MousePointer = vbDefault
  frmMap.ZOrder
End Sub

Private Sub cmdReport_Click()
  Dim fun%, fname$, lstr$, lIncline!(), lLength!(), i&
  Dim hdrstr$
  On Error GoTo NeverMind
  
  If MsgBox("Save as Word table?", vbYesNo, "Save Report") = vbYes Then
    'try to save grid as a Word table
    'cdlReport.Filter = "Word Documents (*.doc)|*.doc|All (*.*)|*.*"
    'cdlReport.ShowSave
    'SaveGridAsDoc agdTable, "", vbCr & "Total Edge of Stream Sediment Load (tons/yr):  " & txtTotal.Text, cdlReport.Filename
    Dim rows As Long, cols As Long
    Dim NewDoc As Word.Document
    Set NewDoc = OpenGridAsDoc(agdTable)
    With NewDoc.Application.Selection
      cols = .Tables(1).Columns.Count
      .Tables(1).rows.Add
      rows = .Tables(1).rows.Count
      .Tables(1).Cell(rows, cols).Select
      .Text = txtTotal.Text
      With .Tables(1)
        cols = cols - 1
        While cols > 1
          .Cell(rows, cols).Merge .Cell(rows, cols - 1)
          cols = cols - 1
        Wend
      End With
      .Tables(1).Cell(rows, 1).Select
      .Text = "Total Edge of Stream Sediment Load (tons/yr):"
      .Tables(1).rows(1).Select
      .Orientation = wdTextOrientationUpward
      .Tables(1).AutoFitBehavior wdAutoFitContent
      .Tables(1).AutoFitBehavior wdAutoFitWindow
      .Start = NewDoc.Characters.Count 'move cursor to after table
      .InsertBreak
      .InsertAfter "Additional sub-form inputs" & vbCr & vbCr
      .ParagraphFormat.Alignment = wdAlignParagraphCenter
      .Font.size = 12
      .MoveRight
      lstr = "Hydrologic Unit (HUC)" & vbTab & "Land Use" & vbTab & _
             "Soil Series Name" & vbTab & "LS Slope Length (ft)" & vbTab & _
             "LS Slope Incline (%)" & vbTab & "Delivery Ratio Area (acres)" & vbTab & _
             "Delivery Ratio Relief (ft)" & vbTab & "Delivery Ratio Length (ft)" & vbTab & _
             "Delivery Ratio Bifurcation Ratio" & vbCr
      With agdTable
        For rows = 1 To .rows
          lstr = lstr & .TextMatrix(rows, HUCCOL) & vbTab & _
                        .TextMatrix(rows, LandUseCOL) & vbTab & _
                        .TextMatrix(rows, SoilSeriesCOL) & vbTab & _
                        .TextMatrix(rows, InclineCOL) & vbTab & _
                        .TextMatrix(rows, LengthCOL)
          If UBound(DRElems, 2) >= rows Then
            For i = 1 To 4
              lstr = lstr & vbTab & DRElems(i, rows)
            Next i
          End If
          lstr = lstr & vbCr
        Next rows
      End With
      .InsertAfter lstr
      .Font.size = 8
      .ConvertToTable _
        Separator:=wdSeparateByTabs, _
        NumColumns:=9, _
        NumRows:=agdTable.rows + 1, _
        Format:=wdTableFormatElegant, _
        ApplyBorders:=True, _
        ApplyShading:=True, _
        ApplyFont:=True, _
        ApplyColor:=True, _
        ApplyHeadingRows:=False, _
        ApplyLastRow:=False, _
        ApplyFirstColumn:=False, _
        ApplyLastColumn:=False, _
        AutoFit:=True, _
        AutoFitBehavior:=wdAutoFitContent
      .Tables(1).AutoFitBehavior wdAutoFitContent
      .Tables(1).rows(1).Select
      .Orientation = wdTextOrientationUpward
      .Tables(1).AutoFitBehavior wdAutoFitContent
      .Tables(1).AutoFitBehavior wdAutoFitWindow
    End With
    NewDoc.Application.Visible = True
  Else
    cdlReport.Filter = "Text (*.txt)|*.txt|All (*.*)|*.*"
    cdlReport.ShowSave
    fname = cdlReport.Filename
    fun = FreeFile(0)
    hdrstr = "Table for Calculating Aggregate Sediment Load from Specified Land Segments"
    With agdTable
      'temporarily remove hidden fields from grid
      .TextMatrix(-2, InclineCOL) = ""
      .TextMatrix(-1, InclineCOL) = ""
      .TextMatrix(0, InclineCOL) = ""
      .TextMatrix(-2, LengthCOL) = ""
      .TextMatrix(-1, LengthCOL) = ""
      .TextMatrix(0, LengthCOL) = ""
      'save grid values in hidden fields
      ReDim lIncline(.rows)
      ReDim lLength(.rows)
      For i = 1 To .rows
        lIncline(i) = .TextMatrix(i, InclineCOL)
        .TextMatrix(i, InclineCOL) = ""
        lLength(i) = .TextMatrix(i, LengthCOL)
        .TextMatrix(i, LengthCOL) = ""
      Next i
      .SavePrintGridBatch fun, , , hdrstr, "", fname
      .TextMatrix(-2, InclineCOL) = "HIDE"
      .TextMatrix(-1, InclineCOL) = "Incline"
      .TextMatrix(0, InclineCOL) = "(%)"
      .TextMatrix(-2, LengthCOL) = "HIDE"
      .TextMatrix(-1, LengthCOL) = "Length"
      .TextMatrix(0, LengthCOL) = "(ft)"
      For i = 1 To .rows 'restore grid values
        .TextMatrix(i, InclineCOL) = lIncline(i)
        .TextMatrix(i, LengthCOL) = lLength(i)
      Next i
    End With
    Open fname For Append As fun
    Print #fun,
    lstr = "Total Edge of Stream Sediment Load (tons/yr):  " & txtTotal.Text
    Print #fun, lstr
    'include additional inputs
    For i = 1 To 5
      Print #fun,
    Next i
    Print #fun, "Additional sub-form inputs"
    Print #fun,
    Print #fun, "        LS        LS        DR        DR        DR        DR"
    Print #fun, " Length-ft Incline-%   Area-ac Relief-ft Length-ft Bif Ratio"
    With agdTable
      For i = 1 To .rows
        lstr = NumFmted(CSng(.TextMatrix(i, LengthCOL)), 10, 1) & _
               NumFmted(CSng(.TextMatrix(i, InclineCOL)), 10, 1)
        If UBound(DRElems, 2) >= i Then
          lstr = lstr & NumFmted(DRElems(1, i), 10, 1) & NumFmted(DRElems(2, i), 10, 1) & _
                        NumFmted(DRElems(3, i), 10, 1) & NumFmted(DRElems(4, i), 10, 1)
        End If
        Print #fun, lstr
      Next i
    End With
    Close #fun
  End If
NeverMind:
End Sub

Private Sub cmdReset_Click()
  Dim i&, lstr$
  If Len(agdTable.TextMatrix(1, HUCCOL)) = 0 Then
    i = vbYes
  Else
    i = MsgBox("Are you sure you want to clear the whole table?", vbYesNo, "TMDL USLE")
  End If
  If i = vbYes Then
    MousePointer = vbHourglass
    agdTable.rows = 1
    agdTable.ClearData
    agdTable.ClearValues
    txtTotal.Text = ""
    
    'If CurHUCs.Count > 0 Then frmMap.Map1.SelectAll 1, False
    Set CurHUCs = Nothing
    Set CurHUCs = New Collection
    MousePointer = vbDefault
  End If
End Sub

Private Sub cmdAddLand_Click()
  AddRow
End Sub

Private Sub cmdErase_Click()
  'agdTable.DeleteRow agdTable.row
  Dim c As Long, r As Long
  With agdTable
    r = .row
    For c = 1 To .cols - 1
      .TextMatrix(r, c) = ""
    Next c
  End With
End Sub

Private Sub cmdSave_Click()
  agdTable.SavePrintGridBatch FreeFile(0), False, False, "USLE " & agdTable.header, "#", "", vbTab, "", "'"
End Sub

Private Sub Form_Load()
  Dim col&
  With agdTable
    .TextMatrix(-2, HUCCOL) = "Hydro-"
    .TextMatrix(-1, HUCCOL) = "logic Unit"
    .TextMatrix(0, HUCCOL) = "(HUC-8)"
    
    .TextMatrix(-1, LandUseCOL) = "Land"
    .TextMatrix(0, LandUseCOL) = "Use"
    .ColEditable(LandUseCOL) = True
    
    .TextMatrix(-2, SoilSeriesCOL) = "Soil"
    .TextMatrix(-1, SoilSeriesCOL) = "Series"
    .TextMatrix(0, SoilSeriesCOL) = "Name"
    .ColEditable(SoilSeriesCOL) = True
    
    .TextMatrix(-1, SoilTextureCOL) = "Soil"
    .TextMatrix(0, SoilTextureCOL) = "Texture"
    .ColEditable(SoilTextureCOL) = True
    
    .TextMatrix(-2, KrangeCOL) = "Range"
    .TextMatrix(-1, KrangeCOL) = "of Re-"
    .TextMatrix(0, KrangeCOL) = "corded K"
    .ColEditable(KrangeCOL) = False
    
    .TextMatrix(-2, KCOL) = "Soil"
    .TextMatrix(-1, KCOL) = "Erodibil-"
    .TextMatrix(0, KCOL) = "ity (K)"
    .ColEditable(KCOL) = True

    .TextMatrix(-2, RCOL) = "Rainfall"
    .TextMatrix(-1, RCOL) = "& Runoff"
    .TextMatrix(0, RCOL) = "(R)"
    .ColEditable(RCOL) = True
    
    .TextMatrix(-2, InclineCOL) = "HIDE"
    .TextMatrix(-1, InclineCOL) = "Incline"
    .TextMatrix(0, InclineCOL) = "(%)"
    .ColEditable(InclineCOL) = True
    
    .TextMatrix(-2, LengthCOL) = "HIDE"
    .TextMatrix(-1, LengthCOL) = "Length"
    .TextMatrix(0, LengthCOL) = "(ft)"
    .ColEditable(LengthCOL) = True
    
    .TextMatrix(-2, LSCOL) = "Topo-"
    .TextMatrix(-1, LSCOL) = "graphy"
    .TextMatrix(0, LSCOL) = "(LS)"
    .ColEditable(LSCOL) = True

    .TextMatrix(-2, CCOL) = "Cover &"
    .TextMatrix(-1, CCOL) = "Mngmnt."
    .TextMatrix(0, CCOL) = "(C)"
    .ColEditable(CCOL) = True

    .TextMatrix(-2, PCOL) = "Support "
    .TextMatrix(-1, PCOL) = "Practice"
    .TextMatrix(0, PCOL) = "(P)"
    .ColEditable(PCOL) = True

    .TextMatrix(-2, FieldRateCOL) = "Edge of"
    .TextMatrix(-1, FieldRateCOL) = "Field Rt"
    .TextMatrix(0, FieldRateCOL) = "(tn/ac/yr)"
    .ColEditable(FieldRateCOL) = False

    .TextMatrix(-2, AreaCOL) = "Segment"
    .TextMatrix(-1, AreaCOL) = "Area"
    .TextMatrix(0, AreaCOL) = "(acres)"
    .ColEditable(AreaCOL) = True

    .TextMatrix(-2, FieldCOL) = "Edge of"
    .TextMatrix(-1, FieldCOL) = "Field Ld."
    .TextMatrix(0, FieldCOL) = "(tons/yr)"
    .ColEditable(FieldCOL) = False

    .TextMatrix(-2, DRCOL) = "Delivery"
    .TextMatrix(-1, DRCOL) = "Ratio"
    .TextMatrix(0, DRCOL) = "(DR)"
    .ColEditable(DRCOL) = True

    .TextMatrix(-2, StreamCol) = "Edge of"
    .TextMatrix(-1, StreamCol) = "Strm. Ld."
    .TextMatrix(0, StreamCol) = "(tons/yr)"
    .ColEditable(StreamCol) = False
    
    .rows = 1
    For col = 0 To .cols - 1
      .ColAlignment(col) = 7 'flexAlignRightCenter
    Next
    '.ColsSizeByContents
  End With
End Sub

Private Sub Form_Resize()
  If Width > 300 Then
    Dim newAGDtop As Long
    agdTable.Width = Width - 276
    fraTotal.Left = Width - 5688
    pctEPALogo.Left = agdTable.Left + agdTable.Width - pctEPALogo.Width
    newAGDtop = pctEPALogo.Top + pctEPALogo.Height - Me.TextHeight("X")
    If agdTable.Top < newAGDtop Then
      agdTable.Top = newAGDtop
    End If
  End If
  If Height > 1300 Then
    agdTable.Height = Height - agdTable.Top - 800
    fraTotal.Top = agdTable.Top + agdTable.Height + 100
  End If
End Sub

Private Sub Kvalues(row As Long, mydb As Database)
  Dim Min As Single, Max As Single, AveK As Single, nVal As Long, val As Single
  Dim SQL$
  Dim myRS As Recordset
            
  If GotSoil = True Then
    With agdTable
      SQL = "SELECT NAME, TEXTURE, KFFACT FROM HUCData " & _
            "WHERE (HUCData.NAME='" & .TextMatrix(row, SoilSeriesCOL)
      If Len(.TextMatrix(row, SoilTextureCOL)) > 0 Then
        SQL = SQL & "' AND HUCData.TEXTURE='" & .TextMatrix(row, SoilTextureCOL)
      End If
      SQL = SQL & "');"
      Set myRS = mydb.OpenRecordset(SQL, dbOpenDynaset)
      ' Calculate max and min values for K-Factor
      Min = 1
      Max = 0
      nVal = 0
      AveK = 0
      While Not myRS.EOF
        val = myRS("KFFACT")
        nVal = nVal + 1
        AveK = AveK + val
        If val > Max Then Max = val
        If val < Min Then Min = val
        myRS.MoveNext
      Wend
      If nVal > 0 Then AveK = AveK / nVal
      ' Fill in K-Factor values on spreadsheet for current soil
      If Max > Min Then
        .TextMatrix(row, KrangeCOL) = NumFmted(Min, 6, 2) & "-" & NumFmted(Max, 6, 2)
        .TextMatrix(row, KCOL) = NumFmted(AveK, 6, 2)
      ElseIf Min < 0.999 Then
        .TextMatrix(row, KrangeCOL) = NumFmted(Min, 6, 2)
        .TextMatrix(row, KCOL) = NumFmted(Min, 6, 2)
      End If
    End With
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  Dim frm As Form
  agdTable.SavePrintGridBatch FreeFile(0), False, False, "USLE " & agdTable.header, "#", DefaultSaveFile, vbTab, "", "'"
  For Each frm In Forms
    Unload frm
  Next
End Sub

Private Sub txtTotal_GotFocus()
  agdTable.col = StreamCol
End Sub
