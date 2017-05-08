Attribute VB_Name = "modHUCFIPS"
Option Explicit

Private pDatabaseFile As Integer

'Binary file format of FIPS_HUC.bin:
'
'States, then counties are stored in ascending FIPS order.
'County FIPS codes include the state FIPS codes to make them unique.
'For each state or county FIPS code, the following is stored:
'
'FIPS + (nHUCS * &H100000) as Long
'
'HUC * nHUCS as 32-bit Longs
'
'See also commented-out routines CreateBinFromCSV and CreateCSVfromBin below

'Return all the HUCs in FIPS_HUC.bin
Public Function GetAllHUCS(asString As Boolean) As FastCollection
  Dim i As Integer
  
  Dim FIPS As Long
  Dim HUC As Long
  Dim HUCstr As String
  
  Dim allHUCs As FastCollection
  Dim nHUCS As Byte
  Dim ShouldClose As Boolean
  
  Set allHUCs = New FastCollection
  Set GetAllHUCS = allHUCs
  
  ShouldClose = OpenDatabase
  
  While Not EOF(pDatabaseFile)
    Get #pDatabaseFile, , FIPS
    nHUCS = FIPS \ &H100000
    FIPS = FIPS And &HFFFFF
    If FIPS > 0 Then
      For i = 1 To nHUCS
        Get #pDatabaseFile, , HUC
        HUCstr = Format(HUC, "00000000")
        If Not allHUCs.KeyExists(HUCstr) Then
          If asString Then
            allHUCs.Add HUCstr, HUCstr
          Else
            allHUCs.Add HUC, HUCstr
          End If
        End If
      Next
    End If
  Wend
  If ShouldClose Then CloseDatabase
End Function

Public Function GetAllFIPS(asString As Boolean, _
                        includeCounties As Boolean, _
                        includeStates As Boolean) As FastCollection
  Dim i As Integer
  
  Dim FIPS As Long
  Dim HUC As Long
  
  Dim intFIPS As Integer
  Dim allFIPS As FastCollection
  Dim nHUCS As Byte
  Dim ShouldClose As Boolean
  
  Set allFIPS = New FastCollection
  Set GetAllFIPS = allFIPS
  
  ShouldClose = OpenDatabase
  
  While Not EOF(pDatabaseFile)
    Get #pDatabaseFile, , FIPS
    nHUCS = FIPS \ &H100000
    FIPS = FIPS And &HFFFFF
    If FIPS < 1 Then
      'skip it
    ElseIf includeCounties And FIPS > 99 Then
      If asString Then
        allFIPS.Add Format(FIPS, "0000000")
      Else
        allFIPS.Add FIPS
      End If
    ElseIf includeStates And FIPS < 100 Then
      If asString Then
        allFIPS.Add Format(FIPS, "00")
      Else
        allFIPS.Add FIPS
      End If
    End If
    If nHUCS > 0 Then Seek #pDatabaseFile, Seek(pDatabaseFile) + 4 * nHUCS
  Wend
  If ShouldClose Then CloseDatabase
End Function

'Return the collection of 8-digit HUCs that overlap the state or county FIPS code
'if asString is True, returned collection will contain String version of HUCs
'if asString is False, returned collection will contain Long version of HUCs
Public Function GetHUCS(findFIPS As Long, asString As Boolean) As FastCollection
  Dim i As Integer
  
  Dim FIPS As Long
  Dim HUC As Long
  
  Dim HUCSinFIPS As FastCollection
  Dim nHUCS As Byte
  Dim ShouldClose As Boolean
  
  Set HUCSinFIPS = New FastCollection
  Set GetHUCS = HUCSinFIPS
  
  ShouldClose = OpenDatabase
  
  While Not EOF(pDatabaseFile) And HUCSinFIPS.count = 0
    Get #pDatabaseFile, , FIPS
    nHUCS = FIPS \ &H100000
    FIPS = FIPS And &HFFFFF
    If FIPS = findFIPS Then
      For i = 1 To nHUCS
        Get #pDatabaseFile, , HUC
        If asString Then
          HUCSinFIPS.Add Format(HUC, "00000000")
        Else
          HUCSinFIPS.Add HUC
        End If
      Next
    ElseIf FIPS > findFIPS Or FIPS = 0 Then
      Exit Function
    Else
      Seek #pDatabaseFile, Seek(pDatabaseFile) + 4 * nHUCS
    End If
  Wend
  If ShouldClose Then CloseDatabase
End Function

'Return the collection of state or county FIPS codes overlapping the given 8-digit HUC
'if asString is True, returned collection will contain String version of HUCs
'if asString is False, returned collection will contain Long version of HUCs
Public Function GetFIPS(findHUC As Long, asString As Boolean, _
                        includeCounties As Boolean, _
                        includeStates As Boolean) As FastCollection
  Dim i As Integer
  
  Dim FIPS As Long
  Dim HUC As Long
  
  Dim intFIPS As Integer
  Dim FIPSinHUC As FastCollection
  Dim nHUCS As Byte
  Dim ShouldClose As Boolean
  
  Set FIPSinHUC = New FastCollection
  Set GetFIPS = FIPSinHUC
  
  ShouldClose = OpenDatabase
  
  While Not EOF(pDatabaseFile)
    Get #pDatabaseFile, , FIPS
    nHUCS = FIPS \ &H100000
    FIPS = FIPS And &HFFFFF
    If includeCounties And FIPS > 99 Or includeStates And FIPS < 100 Then
      i = 1
      While (i <= nHUCS)
        Get #pDatabaseFile, , HUC
        If HUC = findHUC Then
          If asString Then
            If FIPS < 100 Then
              FIPSinHUC.Add Format(FIPS, "00")
            Else
              FIPSinHUC.Add Format(FIPS, "0000000")
            End If
          Else
            FIPSinHUC.Add FIPS
          End If
          Seek #pDatabaseFile, Seek(pDatabaseFile) + 4 * (nHUCS - i) 'skip checking other overlapping HUCs
          i = nHUCS + 1
        Else
          i = i + 1
        End If
      Wend
    Else
      Seek #pDatabaseFile, Seek(pDatabaseFile) + 4 * nHUCS
    End If
  Wend
  If ShouldClose Then CloseDatabase
End Function

Private Function OpenDatabase() As Boolean
  Dim Filename As String
  Dim ff As New ATCoFindFile
  Dim inFile As Integer
  If pDatabaseFile = 0 Then
    ff.SetDialogProperties "Need to open FIPS_HUC.bin", "FIPS_HUC.bin"
    ff.SetRegistryInfo "BASINS", "files", "FIPS_HUC.bin"
    Filename = ff.GetName
    inFile = FreeFile
    Open Filename For Binary Access Read As inFile
    pDatabaseFile = inFile
    OpenDatabase = True
  Else
    Seek #pDatabaseFile, 1
  End If
End Function

Private Sub CloseDatabase()
  If pDatabaseFile > 0 Then Close pDatabaseFile
  pDatabaseFile = 0
End Sub

'Public Sub PrintAllFIPS()
'  Dim v As Variant
'  Dim all As FastCollection
'  Set all = GetAllFIPS(True, True, True)
'  For Each v In all
'    Debug.Print v
'  Next
'  Debug.Print all.Count & " total"
'End Sub

'Public Sub PrintStateFIPScounts()
'  Dim v As Variant
'  Dim nHUCS As Long
'  Dim maxHUCS As Long
'  Dim all As FastCollection
'  Set all = GetAllFIPS(False, False, True)
'  For Each v In all
'    nHUCS = GetHUCS(CLng(v), True).count
'    Debug.Print v & ": " & nHUCS
'    If nHUCS > maxHUCS Then maxHUCS = nHUCS
'  Next
'  Debug.Print "Max = " & maxHUCS
'End Sub

'Public Sub PrintAllHUCS()
'  Dim v As Variant
'  Dim all As FastCollection
'  Set all = GetAllHUCS(True)
'  For Each v In all
'    Debug.Print v
'  Next
'  Debug.Print all.Count & " total"
'End Sub

'Public Sub main()
'  Dim c As FastCollection, v As Variant, FIPS As Long
'
'  ChDir "C:\vbExperimental\WebDataManager\GeoSelect"
''  CreateBinFromCSV "FIPS_HUC.csv", "FIPS_HUC.bin"
''  CreateCSVfromBin "FIPS_HUC.csv.out", "FIPS_HUC.bin"
''  CreateCSV "FIPS_HUC.regen.csv"
'
'  pDatabaseFile = FreeFile
'  Open "FIPS_HUC.bin" For Binary Access Read As pDatabaseFile
'
'  For FIPS = 15 To 15
'    Debug.Print FIPS & ": ";
'    Set c = GetHUCS(FIPS, True)
'
'    For Each v In c
'      Debug.Print v & ", ";
'    Next
'    Debug.Print
'  Next
'End Sub

'Sub CreateCSV(CSVfilename As String)
'  Dim HUCS As FastCollection
'  Dim FIPS As Long
'  Dim vHUC As Variant
'  Dim outFile As Integer
'  OpenDatabase
'  outFile = FreeFile
'  Open CSVfilename For Output Access Write As outFile
'  Debug.Print "FIPS" & vbTab & "nHUCS"
'  For FIPS = 1 To 79000
'    Set HUCS = GetHUCS(FIPS, False)
'    If HUCS.count > 0 Then
'      Debug.Print FIPS & vbTab & HUCS.count
'      For Each vHUC In HUCS
'        Print #outFile, FIPS & "," & vHUC
'      Next
'    End If
'  Next
'  CloseDatabase
'End Sub

'Sub CreateCSVfromBin(CSVfilename As String, BINfilename As String)
'  Dim inFile As Integer
'  Dim outFile As Integer
'  Dim outHUC As Long
'  Dim i As Integer
'  Dim totalHUCS As Long
'  Dim totalFIPS As Long
'
'  Dim FIPS As Long
'  Dim HUC As Long
'
'  Dim lastFIPS As Long
'  Dim intFIPS As Integer
'  Dim HUCSinFIPS As Collection
'  Dim nHUCS As Byte
'
'  inFile = FreeFile
'  Open BINfilename For Binary Access Read As inFile
'  outFile = FreeFile
'  Open CSVfilename For Output Access Write As outFile
'  While Not EOF(inFile)
''    Get #inFile, , intFIPS
''    If intFIPS <> 0 Then
''      If intFIPS < 0 Then
''        FIPS = 32767& - intFIPS
''      Else
''        FIPS = intFIPS
''      End If
''      Get #inFile, , nHUCS
'    Get #inFile, , FIPS
'    nHUCS = FIPS \ &H100000
'    FIPS = FIPS And &HFFFFF
'    If FIPS > 0 Then
'      totalFIPS = totalFIPS + 1
'      For i = 1 To nHUCS
'        Get #inFile, , HUC
'        Print #outFile, FIPS & "," & HUC
'        totalHUCS = totalHUCS + 1
'      Next
'    End If
'  Wend
'  Close #inFile
'  Close #outFile
'  Debug.Print "Wrote " & totalFIPS & " FIPS containing " & totalHUCS & " HUC Segments"
'
'End Sub

'Sub CreateBinFromCSV(CSVfilename As String, BINfilename As String)
'  Dim inFile As Integer
'  Dim outFile As Integer
'  Dim outHUC As Long
'  Dim i As Integer
'  Dim totalHUCS As Long
'  Dim totalFIPS As Long
'
'  Dim FIPS As Long
'  Dim HUC As Long
'
'  Dim lastFIPS As Long
'  Dim intFIPS As Integer
'  Dim HUCSinFIPS As Collection
'  Dim nHUCS As Byte
'
'  'Create FIPS_HUC.bin from FIPS_HUC.csv
'  inFile = FreeFile
'  Open CSVfilename For Input Access Read As inFile
'  outFile = FreeFile
'  If Len(Dir(BINfilename)) > 0 Then Kill BINfilename
'  Open BINfilename For Binary Access Write As outFile
'  While Not EOF(inFile)
'    Input #inFile, FIPS, HUC
'    'If FIPS = 56045 Then Stop
'    If FIPS <> lastFIPS Then
'      GoSub CheckForTimeToWrite
'      Set HUCSinFIPS = Nothing
'      Set HUCSinFIPS = New Collection
'      lastFIPS = FIPS
'    End If
'    HUCSinFIPS.Add HUC
'    totalHUCS = totalHUCS + 1
'  Wend
'  GoSub CheckForTimeToWrite
'  Close inFile
'  Close outFile
'  Debug.Print "Wrote " & totalFIPS & " FIPS containing " & totalHUCS & " HUC Segments"
'
'  Exit Sub
'
'CheckForTimeToWrite:
'  If lastFIPS > 0 Then
'    totalFIPS = totalFIPS + 1
'
'    If HUCSinFIPS.Count > 255 Or lastFIPS >= &H100000 Then Stop '255 should really be larger now
'
'    Put #outFile, , lastFIPS + HUCSinFIPS.Count * &H100000
'
'    For i = 1 To HUCSinFIPS.Count
'      outHUC = HUCSinFIPS.Item(i)
'      Put #outFile, , outHUC
'    Next
'  End If
'  Return
'End Sub
