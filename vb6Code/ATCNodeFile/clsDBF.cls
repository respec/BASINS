VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsDBF"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

'===========================================================================
' Subject: READ DBASE III                    Date: 1/25/88 (00:00)
' Author:  David Perry                       Code: QB, PDS
' Keys:    READ,DBASE,III                  Packet: MISC.ABC
'===========================================================================

'This QB source was adjusted for use with VB by Robert Smith
'on June 14, 1999, source was provided to Smith by Marc Hoogerwerf
'contact Smith via: www.smithvoice.com/vbfun.htm

'This code was turned into a class by Mark.Gray@pobox.com March 14, 2001

'dBaseIII file header, 32 bytes
Private Type dBHeader
   version As Byte
   Year As Byte
   Month As Byte
   Day As Byte
   NumRecs As Long
   NumBytesHeader As Integer
   NumBytesRec As Integer
   Trash(1 To 20) As Byte
End Type

'Field Descriptions, 32 bytes * Number of Fields
'Up to 128 Fields
Private Type FieldDescriptor
   fname As String * 11
   FType As String * 1 ' C = Character, D = Date, N = Numeric, L = Logical, M = Memo
   DataAddress As Long 'set in memory; not useful on disk
   Length As Byte
   DecimalCount As Byte
   Trash(1 To 14) As Byte
End Type

Private pHeader As dBHeader
Private pFields() As FieldDescriptor
Private pNumFields As Long
Private pData() As Byte
Private pDataBytes As Long
Private pCurrentRecord As Long
Private pCurrentRecordStart As Long

Public Function Summary() As String
  Dim retval As String
  Dim iTrash As Integer
  Dim iField As Integer
  With pHeader
    retval = "DBF Header: " & .version
    retval = retval & vbCrLf & "    Version: " & .version
    retval = retval & vbCrLf & "    Date: " & .Year & "/" & .Month & .Day
    retval = retval & vbCrLf & "    NumRecs: " & .NumRecs
    retval = retval & vbCrLf & "    NumBytesHeader: " & .NumBytesHeader
    retval = retval & vbCrLf & "    NumBytesRec: " & .NumBytesRec
    retval = retval & vbCrLf & "    Trash: " & .NumBytesRec
    For iTrash = 1 To 20
      retval = retval & .Trash(iTrash) & " "
    Next
  End With
  For iField = 1 To pNumFields
    retval = retval & vbCrLf
    With pFields(iField)
      retval = retval & vbCrLf & "Field " & iField & ": '" & .fname & "'"
      retval = retval & vbCrLf & "    Type: " & .FType & " "
      Select Case .FType
        Case "C": retval = retval & "(Character)"
        Case "D": retval = retval & "(Date)"
        Case "N": retval = retval & "(Numeric)"
        Case "L": retval = retval & "(Logical)"
        Case "M": retval = retval & "(Memo)"
      End Select
      retval = retval & vbCrLf & "    Length: " & .Length & " "
      retval = retval & vbCrLf & "    DecimalCount: " & .DecimalCount & " "
      retval = retval & vbCrLf & "    Trash: "
      For iTrash = 1 To 14
        retval = retval & .Trash(iTrash) & " "
      Next
    End With
  Next
End Function

Public Property Get Year() As Byte
  Year = pHeader.Year
End Property
Public Property Let Year(ByVal NewValue As Byte)
  pHeader.Year = NewValue
End Property

Public Property Get Month() As Byte
  Month = pHeader.Month
End Property
Public Property Let Month(ByVal NewValue As Byte)
  pHeader.Month = NewValue
End Property

Public Property Get Day() As Byte
  Day = pHeader.Day
End Property
Public Property Let Day(ByVal NewValue As Byte)
  pHeader.Day = NewValue
End Property

Public Property Get NumFields() As Long
  NumFields = pNumFields
End Property
Public Property Let NumFields(ByVal NewValue As Long)
  pNumFields = NewValue
  ReDim pFields(1 To pNumFields)
  pHeader.NumBytesHeader = (pNumFields + 1) * 32 + 1
End Property

Public Property Get FieldName(ByVal FieldNumber As Long) As String
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldName = TrimNull(pFields(FieldNumber).fname)
  Else
    FieldName = "Undefined"
  End If
End Property
Public Property Let FieldName(ByVal FieldNumber As Long, ByVal NewValue As String)
  Dim lenNew As Long
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    lenNew = Len(NewValue)
    If lenNew < 11 Then NewValue = NewValue & String(11 - lenNew, Chr$(0))
    pFields(FieldNumber).fname = NewValue
  End If
End Property

Public Property Get FieldDecimalCount(ByVal FieldNumber As Long) As Byte
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldDecimalCount = pFields(FieldNumber).DecimalCount
  Else
    FieldDecimalCount = 0
  End If
End Property
Public Property Let FieldDecimalCount(ByVal FieldNumber As Long, ByVal NewValue As Byte)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).DecimalCount = NewValue
  End If
End Property

Public Property Get FieldLength(ByVal FieldNumber As Long) As Byte
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldLength = pFields(FieldNumber).Length
  Else
    FieldLength = 0
  End If
End Property
Public Property Let FieldLength(ByVal FieldNumber As Long, ByVal NewValue As Byte)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).Length = NewValue
  End If
End Property

Public Property Get FieldType(ByVal FieldNumber As Long) As String
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    FieldType = pFields(FieldNumber).FType
  Else
    FieldType = "Undefined"
  End If
End Property
Public Property Let FieldType(ByVal FieldNumber As Long, ByVal NewValue As String)
  If FieldNumber > 0 And FieldNumber <= pNumFields Then
    pFields(FieldNumber).FType = NewValue
  End If
End Property

Public Property Get NumRecords() As Long
  NumRecords = pHeader.NumRecs
End Property
Public Property Let NumRecords(ByVal NewValue As Long)
  pHeader.NumRecs = NewValue
End Property

Public Property Get CurrentRecord() As Long
  CurrentRecord = pCurrentRecord
End Property
Public Property Let CurrentRecord(ByVal NewValue As Long)
  If NewValue < 1 Or NewValue > pHeader.NumRecs Then
    pCurrentRecord = 1
  Else
    pCurrentRecord = NewValue
  End If
  pCurrentRecordStart = pHeader.NumBytesRec * (pCurrentRecord - 1) + 1
End Property

Public Property Get Value(ByVal FieldNum As Long) As String
  If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
    Value = "Invalid Current Record Number"
  ElseIf FieldNum < 1 Or FieldNum > NumFields Then
    Value = "Invalid Field Number"
  Else
    Dim FieldStart As Long
    Dim i As Long
    Dim strRet As String
    
    FieldStart = pCurrentRecordStart + 1
    For i = 1 To FieldNum - 1
      FieldStart = FieldStart + pFields(i).Length
    Next
    
    strRet = ""
    For i = 0 To pFields(FieldNum).Length - 1
      If pData(FieldStart + i) > 0 Then
        strRet = strRet & Chr(pData(FieldStart + i))
      Else
        i = 256
      End If
    Next
    Value = Trim(strRet)
'    If pFields(FieldNum).FType = "N" Then
'      Dim dblval As Double
'      dblval = CDbl(strRet)
'      If pFields(FieldNum).DecimalCount <> 0 Then
'        dblval = dblval * 10 ^ pFields(FieldNum).DecimalCount
'      End If
'      Value = dblval
'    End If
  End If
End Property
Public Property Let Value(ByVal FieldNum As Long, ByVal NewValue As String)
  If pCurrentRecord < 1 Or pCurrentRecord > pHeader.NumRecs Then
    'Value = "Invalid Current Record Number"
  ElseIf FieldNum < 1 Or FieldNum > NumFields Then
    'Value = "Invalid Field Number"
  Else
    Dim FieldStart As Long
    Dim i As Long
    Dim strRet As String
    Dim lenStr As Long
    
    FieldStart = pCurrentRecordStart + 1
    For i = 1 To FieldNum - 1
      FieldStart = FieldStart + pFields(i).Length
    Next
    strRet = NewValue
    lenStr = Len(strRet)
    If lenStr > pFields(FieldNum).Length Then
      strRet = Left(strRet, pFields(FieldNum).Length)
    ElseIf pFields(FieldNum).FType = "N" Then
      strRet = Space(pFields(FieldNum).Length - lenStr) & strRet
    Else
      strRet = strRet & Space(pFields(FieldNum).Length - lenStr)
    End If
    For i = 0 To pFields(FieldNum).Length - 1
      pData(FieldStart + i) = Asc(Mid(strRet, i + 1, 1))
    Next
  End If
End Property

'Dimension and initialize data buffer to all spaces (except for initial carriage return)
'Do not call on an existing DBF since all data will be removed from memory
'If creating a new DBF:
' Call after setting NumRecords, NumFields and all FieldLength
' Call before setting any Value
Public Sub InitData()
  Dim b As Long
  
  pHeader.NumBytesRec = 1
  For b = 1 To pNumFields
    pHeader.NumBytesRec = pHeader.NumBytesRec + pFields(b).Length
  Next
  
  pDataBytes = pHeader.NumRecs * pHeader.NumBytesRec
  ReDim pData(pDataBytes)
  pData(0) = 13
  For b = 1 To pDataBytes
    pData(b) = 32
  Next
End Sub

Public Sub OpenDBF(ByVal filename As String)

  'Dim header As dBHeader, FieldDes As FieldDescriptor    'Creating variables for user-defined types
  'Dim memo As String * 512                               'Create a 512 byte fixed string variable
                                                          ' to read memo fields
  Dim inFile As Integer
  Dim i As Integer, dot As Integer
  
  If Len(filename) = 0 Then
    Exit Sub
  ElseIf Len(Dir(filename)) = 0 Then
    Exit Sub
  End If
  
  dot = InStr(filename, ".")
  If dot > 0 Then
    filename = Left$(filename, dot - 1) & ".dbf"
  Else
    filename = filename$ & ".dbf"
  End If

  inFile = FreeFile(0)
  Open filename For Binary As inFile
  Get #inFile, , pHeader
  Select Case pHeader.version 'Be sure we're using a dBASE III file
    Case 3 'Normal dBASEIII file
'   Case &H83 'Open a .DBT file
    Case Else
      MsgBox "This is not a dBASE III file: '" & filename & "'"
      Close inFile
      Exit Sub
  End Select

  NumFields = pHeader.NumBytesHeader \ 32 - 1 'Calculate the number of fields

  'frm.fa.TextMatrix(frm.fa.Rows - 1, 0) = "Field  Field Name     Type   Width  Dec"
  For i = 1 To (NumFields)
    Get #inFile, , pFields(i)  'Looping through NumFields by reading in 32 byte records
  Next i

  pDataBytes = LOF(inFile) - pHeader.NumBytesHeader
  'ReDim Preserve pData(pDataBytes)
  pData = InputB(pDataBytes, inFile)
  Close #inFile
  CurrentRecord = 1
End Sub

Public Sub WriteDBF(ByVal filename As String)
  Dim outfile As Integer
  Dim i As Integer, j As Integer, dot As Integer
  Dim s As String

  dot = InStr(filename, ".")
  If dot > 0 Then
    filename = Left$(filename, dot - 1) & ".dbf"
  Else
    filename = filename$ & ".dbf"
  End If

  outfile = FreeFile(0)
  Open filename For Binary As outfile
  Put #outfile, , pHeader

  For i = 1 To (NumFields)
    Put #outfile, (32 * i) + 1, pFields(i)
  Next i
  
  Put #outfile, , pData
  Close #outfile
End Sub

'     Select Case FieldDes(i).FType                       'Reading the dBASE Field Type
'        Case "C":           printtype$ = "Character"
'        Case "D":           printtype$ = "Date"
'        Case "N":           printtype$ = "Numeric"
'        Case "L":           printtype$ = "Logical"
'        Case "M":           printtype$ = "Memo"
'     End Select


'Static Sub Stripchar(a As String)
'
'  Dim sTemp As String
'  Dim sTemp2 As String
'  Dim iCount As Integer
'
'  iCount = InStr(a, Chr$(&HA))
'  Do While iCount
'     sTemp = Left$(a, iCount - 1)
'     sTemp2 = Right$(a, Len(a$) - iCount)
'     a$ = sTemp & sTemp2
'     iCount = InStr(a$, Chr$(&HA))
'  Loop
'  iCount = InStr(a, Chr$(&H8D))
'  Do While iCount
'     sTemp = Left$(a$, iCount - 1)
'     sTemp2 = Right$(a$, Len(a$) - iCount)
'     a$ = sTemp & Chr$(&HD) & sTemp2
'     iCount = InStr(a$, Chr$(&H8D))
'  Loop
'End Sub

Private Function TrimNull(str As String) As String
  Dim nullPos As Long
  nullPos = InStr(str, Chr$(0))
  If nullPos = 0 Then
    TrimNull = Trim(str)
  Else
    TrimNull = Trim(Left(str, nullPos - 1))
  End If
End Function

Private Sub Class_Initialize()
  Clear
End Sub

Public Sub Clear()
  pHeader.version = 3
  pHeader.Day = 1
  pHeader.Month = 1
  pHeader.Year = 70
  pHeader.NumBytesHeader = 32
  pHeader.NumBytesRec = 0
  pHeader.NumRecs = 0
  pNumFields = 0
  pDataBytes = 0
  pCurrentRecord = 1
  pCurrentRecordStart = 0
  ReDim pData(0)
  ReDim pFields(0)
End Sub
