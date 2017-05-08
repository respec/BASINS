VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsHspfBinary"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2002 by AQUA TERRA Consultants

Private pFileName As String
Private pErrorDescription As String
Private pFile As clsFtnUnfFile
Private pFileRecordIndex As Long

Public Type HspfBinaryID
  OperationName As String
  OperationNumber As Long
  SectionName As String
End Type
Public Type HSPFBinaryData
  UnitFlag As Long
  OutLev As Long
  Date(5) As Long
  value() As Single
End Type
Public Type HSPFBinaryHeader
  id As HspfBinaryID
  VarNames As FastCollection 'of String
  Data As FastCollection 'of HSPFBinaryData
End Type
Dim pHeaders As FastCollection 'of HSPFBinaryHeader
Private pMonitor As Object
Private pMonitorSet As Boolean

Public Property Get DateAsJulian(hindex As Long, dindex As Long) As Double
  Dim lData As HSPFBinaryData
  lData = Header(hindex).Data.ItemByIndex(dindex)
  DateAsJulian = Date2J(lData.Date)
End Property

Public Property Get DateAsText(hindex As Long, dindex As Long) As String
  Dim lData As HSPFBinaryData
  lData = Header(hindex).Data.ItemByIndex(dindex)
  With lData
    DateAsText = .Date(0) & "/" & .Date(1) & "/" & .Date(2) & " " & _
                 .Date(3) & ":" & .Date(4) & ":" & .Date(5)
  End With
End Property

Public Property Get Header(index As Long) As HSPFBinaryHeader
  Header = pHeaders.ItemByIndex(index)
End Property

Public Property Get Headers() As FastCollection
  Set Headers = pHeaders
End Property

Public Property Set Monitor(ByVal o As Object)
  Set pMonitor = o
  pMonitorSet = True
End Property

Public Property Get HeaderIdAsText(index As Long) As String
  HeaderIdAsText = HspfBinaryIdAsText(Header(index).id)
End Property

Public Property Get Filename() As String
  Filename = pFile.Filename
End Property
Public Property Let Filename(newFileName As String)
  Set pFile = New clsFtnUnfFile
  pFile.Filename = newFileName
  pErrorDescription = pFile.ErrorDescription
  If Len(pErrorDescription) = 0 Then
    Set pHeaders = Nothing
    Set pHeaders = New FastCollection
    pFileRecordIndex = 1
    ReadNewRecords
  End If
  'Set pFile = Nothing
End Property

Public Sub ReadNewRecords()
  Dim myId As HspfBinaryID
  Dim myHeader As HSPFBinaryHeader
  Dim myData As HSPFBinaryData
  Dim myVar As String
  Dim myKey As String
  Dim j As Long, varCnt As Long, index As Long
  Dim k As String, s As String
  Dim currec() As Byte
  Dim recbyte As Long 'Byte position within current header record
  Dim thisVarName As String
  Dim varLen As Long
  Dim i As Long, percent As Double, lastprog As Double
  
  pFile.ReadRestOfRecordsInFile
  While pFileRecordIndex <= pFile.RecordCount
    currec = pFile.rec(pFileRecordIndex)
    Select Case Byte2Long(currec, 0)
    Case 0: 'header record
      With myHeader
        myKey = MakeIdAndKey(currec, .id)
        Set .VarNames = Nothing
        Set .VarNames = New FastCollection
        
        recbyte = 24
        While recbyte < UBound(currec)
          varLen = Byte2Long(currec, recbyte)
          thisVarName = Byte2String(currec, recbyte + 4, varLen)
          On Error Resume Next
          If InStr(thisVarName, "(") Then
            Debug.Print "bad varname:" & thisVarName
          End If
          .VarNames.Add thisVarName, thisVarName
          On Error GoTo 0
          recbyte = recbyte + varLen + 4
        Wend
        
'        varCnt = (UBound(currec) - 24) / 8 '8 will change soon
'        For j = 0 To varCnt - 1
'          .VarNames.Add Byte2String(currec, 24 + (j * 8), 8) '8 will change soon
'        Next j
        
        Set .Data = Nothing
        Set .Data = New FastCollection
      End With
      pHeaders.Add myHeader, myKey
      Debug.Print "header:" & myKey
    Case 1: 'data record
      myKey = MakeIdAndKey(currec, myId)
      If pHeaders.KeyExists(myKey) Then
        index = pHeaders.IndexFromKey(myKey)
        With myData
          .UnitFlag = Byte2Long(currec, 24)
          .OutLev = Byte2Long(currec, 28)
          For j = 0 To 4
            .Date(j) = Byte2Long(currec, 32 + (j * 4))
          Next j
          'note Tom Jobes look at this
          If .Date(4) = 60 And .Date(3) = 24 Then
            .Date(4) = 0 'otherwise jdate is first hour in next day
          End If
          'end note to Tom
          varCnt = pHeaders(index).VarNames.Count
          ReDim .value(varCnt)
          For j = 0 To varCnt
            .value(j) = Byte2Single(currec, 48 + (j * 4))
          Next j
          On Error Resume Next
          pHeaders(index).Data.Add myData, .OutLev & ":" & Date2J(.Date) 'should this be a date string YYYY/MM/DD HH?
        End With
      Else
        s = "Data Without Header for Key:" & myKey
        MsgBox s, vbOKOnly, "clsHspfBinary"
        pErrorDescription = s
      End If
    Case Else
      s = "Bad Record Type: " & Byte2Long(currec, 0)
      MsgBox s, vbOKOnly, "clsHspfBinary"
      pErrorDescription = s
    End Select
    If pMonitorSet Then
      percent = (100 * i) / (pFile.RecordCount * 2)
      If percent > lastprog + 1 Then  ' update progress message
        s = "(PROGRESS " & CStr(percent) & ")"
        pMonitor.SendMonitorMessage s
        lastprog = percent
      End If
    End If
    pFileRecordIndex = pFileRecordIndex + 1
  Wend
End Sub

Private Function MakeIdAndKey(lRec() As Byte, lid As HspfBinaryID) As String
  With lid
    .OperationName = Trim(Byte2String(lRec, 4, 8))
    .OperationNumber = Byte2Long(lRec, 12)
    .SectionName = Trim(Byte2String(lRec, 16, 8))
  End With
  MakeIdAndKey = HspfBinaryIdAsText(lid)

End Function

Private Function HspfBinaryIdAsText(lid As HspfBinaryID) As String
  With lid
    HspfBinaryIdAsText = .OperationName & ":" & .OperationNumber & ":" & .SectionName
  End With
End Function

Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

Private Sub Class_Initialize()
  pFileName = ""
  pErrorDescription = ""
End Sub

Private Sub Class_Terminate()
  Set pFile = Nothing
End Sub

Public Function BinaryValue(opKey As String, varKey As String, dateKey As String) As Single
  
  Dim HeaderIndex As Long, VarIndex As Long, DateIndex As Long
  Dim val As Single
  
  val = -999#
  
  If pHeaders.KeyExists(opKey) Then
    HeaderIndex = pHeaders.IndexFromKey(opKey)
    If pHeaders(HeaderIndex).VarNames.KeyExists(varKey) Then
      VarIndex = pHeaders(HeaderIndex).VarNames.IndexFromKey(varKey)
      If pHeaders(HeaderIndex).Data.KeyExists(dateKey) Then
        DateIndex = pHeaders(HeaderIndex).Data.IndexFromKey(dateKey)
        val = pHeaders(HeaderIndex).Data(DateIndex).value(VarIndex)
      Else
        pErrorDescription = "Missing Datekey " & dateKey & _
                            " in Date Key Collection for Header " & _
                           HspfBinaryIdAsText(Header(HeaderIndex).id)
      End If
    Else
      pErrorDescription = "Missing Varkey " & varKey & _
                         " in Variable Name Collection for Header " & _
                         HspfBinaryIdAsText(Header(HeaderIndex).id)
    End If
  Else
    pErrorDescription = "Missing Opkey " & opKey & " in Headers Collection"
  End If

  BinaryValue = val
End Function

