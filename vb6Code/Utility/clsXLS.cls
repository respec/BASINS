VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsXLS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private Const AscA = 65

Private outf As Integer

Private Enum XLrecEnum
  XLrecDIMENSIONS = 0
  XLrecBLANK = 1
  XLrecINTEGER = 2
  XLrecNumber = 3
  XLrecLabel = 4
  XLrecBOOLERR = 5
  XLrecFORMULA = 6
  XLrecSTRING = 7
  XLrecrow = 8
  XLrecBOF = 9
  XLrecEOF = 10                  ' 0A
  XLrecINDEX = 11                ' 0B
  XLrecCALCCOUNT = 12            ' 0C
  XLrecCALCMODE = 13             ' 0D
  XLrecPRECISION = 14            ' 0E
  XLrecREFMODE = 15              ' 0F
  XLrecDELTA = 16                ' 10
  XLrecITERATION = 17            ' 11
  XLrecPROTECT = 18              ' 12
  XLrecPASSWORD = 19             ' 13
  XLrecHEADER = 20               ' 14
  XLrecFOOTER = 21               ' 15
  XLrecEXTERNCOUNT = 22          ' 16
  XLrecEXTERNSHEET = 23          ' 17
  XLrecNAME = 24                 ' 18
  XLrecWINDOWPROTECT = 25        ' 19
  XLrecVERTICALPAGEBREAKS = 26   ' 1A
  XLrecHORIZONTALPAGEBREAKS = 27 ' 1B
  XLrecNOTE = 28                 ' 1C
  XLrecSELECTION = 29            ' 1D
  XLrecFORMAT = 30               ' 1E
  XLrecFORMATCOUNT = 31          ' 1F
  XLrecCOLUMNDEFAULT = 32        ' 20
  XLrecARRAY = 33                ' 21
  XLrec1904 = 34                 ' 22
  XLrecEXTERNNAME = 35           ' 23
  XLrecCOLWIDTH = 36             ' 24
  XLrecDEFAULTROWHEIGHT = 37     ' 25
  XLrecLEFTMARGIN = 38           ' 26
  XLrecRIGHTMARGIN = 39          ' 27
  XLrecTOPMARGIN = 40            ' 28
  XLrecBOTTOMMARGIN = 41         ' 29
  XLrecPRINTROWHEADERS = 42      ' 2A
  XLrecPRINTGRIDLINES = 43       ' 2B
  XLrecFILEPASS = 47             ' 2F
  XLrecFont = 49                 ' 31
  XLrecFONT2 = 50                ' 32
  XLrecTable = 54                ' 36
  XLrecTABLE2 = 55               ' 37
  XLrecCONTINUE = 60             ' 3C
  XLrecWINDOW1 = 61              ' 3D
  XLrecWINDOW2 = 62              ' 3E
  XLrecBACKUP = 64               ' 40
  XLrecPANE = 65                 ' 41
End Enum

Private Sub UserToInternal(Coords As String, ByRef row As Long, ByRef col As Long)
  Dim RowStartPos As Long
  RowStartPos = 2
  col = Asc(Left(Coords, 1)) - AscA
  While Not IsNumeric(Mid(Coords, RowStartPos, 1))
    RowStartPos = RowStartPos + 1
    col = (col + 1) * 26 + Asc(Left(Coords, 1)) - AscA
  Wend
  row = CLng(Mid(Coords, RowStartPos))
End Sub

Private Sub WriteRecordHeader(opcode As XLrecEnum, oplength As Integer)
  Dim opcodeOut As Integer
  opcodeOut = opcode
  Put #outf, , opcodeOut
  Put #outf, , oplength
End Sub

Public Sub SaveAs(Filename As String)
  Dim IntZero As Integer
  Dim version As Integer, filetype As Integer
  Dim IterCount As Integer
  Dim RefMode As Integer
  
  IntZero = 0
  
  outf = FreeFile(0)
  Open Filename For Binary As outf
  
  WriteRecordHeader XLrecBOF, 4
  version = 2
  filetype = 16
  Put #outf, , version
  Put #outf, , filetype
  
  'INDEX record is optional
  
  WriteRecordHeader XLrecCALCCOUNT, 2
  IterCount = 1
  Put #outf, , IterCount
  
  WriteRecordHeader XLrecCALCMODE, 2
  Put #outf, , IntZero
  
  WriteRecordHeader XLrecPRECISION, 2
  Put #outf, , IntZero
  
  WriteRecordHeader XLrecREFMODE, 2
  RefMode = 1
  Put #outf, , RefMode
  
  Close outf
End Sub
