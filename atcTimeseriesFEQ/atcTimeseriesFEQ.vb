Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcTimeseriesFEQ
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "FEQ Data Files (*.feo)|*.feo"
    Private Shared pNaN As Double = GetNaN()
    Private pFileBytes() As Byte
    Private pFileInt() As Short
    Private pFileSng() As Single

    Private pFilename As String
    Private pFileExt As String
    Private pErrorDescription As String
    'Private pMonitor As Object
    'Private pMonitorSet As Boolean
    Private pDates As atcTimeseries
    Private pDatesPopulated As Boolean
    Private XTIOFF As Integer
    Private Const JulianModification1858 As Integer = 679006 '17 Nov 1858
    Private Const JulianModification1899 As Integer = 694024 '30 Dec 1899
    Private Const FEQ_JULIAN_OFFSET As Double = JulianModification1858 - JulianModification1899

    'lktab routine
    Declare Sub F90_LKTAB Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef r As Single, ByRef l As Integer, ByRef r As Single)
    'xlkt routines
    Declare Sub F90_XLKT20 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_XLKT21 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_XLKT22 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_XLKT23 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_XLKT24 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_XLKT25 Lib "feqlib.dll" (ByRef l As Integer, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single, ByRef r As Single)
    Declare Sub F90_FILTAB Lib "feqlib.dll" (ByVal s As String, ByRef l As Integer, ByRef l As Integer, ByRef l As Integer, ByVal i As Short)
    Declare Sub F90_TABTYP Lib "feqlib.dll" (ByRef l As Integer, ByRef l As Integer)

    Private Structure FeqFtf
        Dim Bindex As Integer
        Dim Lindex As Integer
        Dim HA As Integer
        Dim NTab As Integer
        Dim Type As Integer
        Dim LSta As Integer
        Dim Shift As Integer
        Dim Off As Integer
    End Structure

    Private Structure FeqLocDir
        Dim Location As String
        Dim Constit As Collection
        Dim Label As String
        Dim LocClass As String
        Dim Branch As Integer
        Dim NodeID As String
        Dim Station As Single
        Dim Invert As Single
        Dim UserID As String
        Dim FtabIndex As Integer
        Dim FtabTyp As Integer
        Dim SeriesOffset As Integer
        Dim Easting As Double
        Dim Northing As Double
    End Structure

    Private Class FeqDataFile
        Public version As String ' FEQ version
        Public Scenario As String
        Public NameFeo As String ' output file name
        Public NameTsd As String ' timeseries file name
        Public RecLen As Integer ' timeseries file record length
        Public NameFtf As String ' function table file name
        Public ItemPerRec As Integer ' items per record
        Public LeftItemCnt As Integer ' left over item count
        Public NumbFullRec As Integer ' number of full records
        Public LocDir() As FeqLocDir
        Public LocCount As Integer ' count of locations
        Public Term As String ' termination status
        Public ftf() As FeqFtf
    End Class
    Private f As New FeqDataFile

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "FEQ Data Files"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::" & Description
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            Select Case IO.Path.GetExtension(Specification).ToLower
                Case ".feo"
                    f.NameFeo = Specification
                    FeoRead()
                    Return True
                Case Else
                    Throw New ApplicationException("Unknown SWAT Data File Type: " & aFileName)
            End Select
        End If
        Return False
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        pData = Nothing
        ReDim f.LocDir(0)
    End Sub

    Private Sub AddToColl(ByRef coll As Collection, ByVal ParamArray newValues() As Object)
        For Each lValue As Object In newValues
            coll.Add(lValue)
        Next lValue
    End Sub

    Private Sub SetConstitFromTabType(ByRef LocDir As FeqLocDir)
        With LocDir
            Select Case .FtabTyp
                Case 20 : AddToColl(.Constit, "A", "T", "DT", "K", "DK", "B", "DB")
                Case 23 : AddToColl(.Constit, "A", "T", "DT", "K", "DK", "B", "DB", "MA", "DMA", "MQ", "DMQ")
                Case 21 : AddToColl(.Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB")
                Case 22 : AddToColl(.Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "ALP", "DALP", "QC")
                Case 24 : AddToColl(.Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "MA", "DMA", "MQ", "DMQ")
                Case 25 : AddToColl(.Constit, "A", "T", "DT", "J", "K", "DK", "B", "DB", "ALP", "DALP", "QC", "MA", "DMA", "MQ", "DMQ")
            End Select
        End With
    End Sub

    Private Function FtabTyp(ByRef Ind As Integer) As Integer
        'For lIndex As Integer = 0 To pFileBytes.GetUpperBound(0) - 4
        '    Select Case BitConverter.ToInt32(pFileBytes, lIndex)
        '        Case 20, 23, 21, 22, 24, 25 : Debug.Print(lIndex & " = " & BitConverter.ToInt32(pFileBytes, lIndex))
        '    End Select
        'Next
        If UBound(pFileBytes) > 0 Then Return BitConverter.ToInt32(pFileBytes, (Ind + 2) * 4)
        Return 0
    End Function

    Public Overrides Sub ReadData(aDataSet As atcData.atcDataSet)
        Dim lFillTS As atcTimeseries = aDataSet
        'Dim index#
        Dim i As Short
        Dim lp, lind, pos, tcol, Apos As Integer
        Dim X, r, e As Single
        Dim addr, c As Integer
        Dim ya As Single
        'Dim a!, t!, dt!, j!, k!, dk!, b!, db!, alp!, dalp!, qc!, ma!, dma!, mq!, dmq!
        Dim a() As Double
        Dim t(0) As Double
        Dim dt(0) As Double
        Dim j(0) As Double
        Dim k(0) As Double
        Dim dk(0) As Double
        Dim b(0) As Double
        Dim db(0) As Double
        Dim alp(0) As Double
        Dim dalp(0) As Double
        Dim qc(0) As Double
        Dim ma(0) As Double
        Dim dma(0) As Double
        Dim mq(0) As Double
        Dim dmq(0) As Double
        Dim spos As Integer
        'Dim Ftab() As Single
        Dim LA, HA, Xoff As Integer
        'Dim flg&()
        Dim LocName, ConName As String

        'Logger.Dbg "(OPEN Filling FEO Data)(BUTTOFF CANCEL)(BUTTOFF PAUSE)"

        If UBound(pFileBytes) = 0 Then
            pErrorDescription = "FTF file not found: " & f.NameFtf
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage.
            Logger.Dbg("(MSG1 " & pErrorDescription & ")(CLOSE)")
            Exit Sub
        End If

        LocName = lFillTS.Attributes.GetValue("Location")
        ConName = lFillTS.Attributes.GetValue("Constituent")
        'ReDim v(f.TimCount)
        'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage.
        Logger.Dbg("(MSG1 Filling FEO Data for Node " & LocName & ", Constituent " & ConName & ")")

        'index = dataObject.Header.id                'FIXME -- Is this where the index should come from?
        lind = 0
        While f.LocDir(lind).Location <> LocName
            lind = lind + 1
            If lind > UBound(f.LocDir) Then
                MsgBox("Location '" & LocName & "' not found in FEO.", MsgBoxStyle.Critical, "FEQ Fill Timser")
                'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage.
                Logger.Dbg("(CLOSE)")
                Exit Sub
            End If
        End While
        c = 0
        'UPGRADE_WARNING: Couldn't resolve default property of object f.LocDir(lind).Constit(c + 1).
        While f.LocDir(lind).Constit.Item(c + 1) <> ConName
            c = c + 1
            If c >= f.LocDir(lind).Constit.Count() Then
                MsgBox("Constituent '" & ConName & "' not found in FEO.", MsgBoxStyle.Critical, "FEQ Fill Timser")
                Exit Sub
            End If
        End While
        If c < 2 Then
            tcol = f.LocDir(lind).SeriesOffset + c
            X = 0
        Else
            tcol = f.LocDir(lind).SeriesOffset + 1
            X = c - 1
        End If
        e = f.LocDir(lind).Invert
        addr = f.LocDir(lind).FtabIndex

        Apos = pDates.numValues
        If addr = 0 Then
            pErrorDescription = "No FtabIndex for node " & f.LocDir(lind).NodeID
            ReDim a(Apos)
            ReDim lFillTS.Values(Apos)
            lFillTS.Attributes.DiscardCalculated()
            Exit Sub
        End If
        'ReDim flg(Apos)
        ReDim a(Apos)
        If X > 0 Then
            ReDim t(Apos)
            ReDim dt(Apos)
            ReDim k(Apos)
            ReDim dk(Apos)
            ReDim b(Apos)
            ReDim db(Apos)
            'Some of these may get larger in the select below, but by default we want all of them dimensioned
            ReDim ma(0) : ReDim dma(0) : ReDim mq(0) : ReDim dmq(0) : ReDim j(0) : ReDim alp(0) : ReDim dalp(0) : ReDim qc(0)
            Select Case f.LocDir(lind).FtabTyp
                Case 20 : Xoff = 5 'all redims are included in defaults
                Case 21 : Xoff = 6 : ReDim j(Apos)
                Case 23 : Xoff = 7 : ReDim ma(Apos) : ReDim dma(Apos) : ReDim mq(Apos) : ReDim dmq(Apos)
                Case 24 : Xoff = 8 : ReDim j(Apos) : ReDim ma(Apos) : ReDim dma(Apos) : ReDim mq(Apos) : ReDim dmq(Apos)
                Case 22 : Xoff = 8 : ReDim j(Apos) : ReDim alp(Apos) : ReDim dalp(Apos) : ReDim qc(Apos)
                Case 25 : Xoff = 10 : ReDim j(Apos) : ReDim alp(Apos) : ReDim dalp(Apos) : ReDim qc(Apos) : ReDim ma(Apos) : ReDim dma(Apos) : ReDim mq(Apos) : ReDim dmq(Apos)
            End Select
        End If

        LA = addr + XTIOFF
        HA = BitConverter.ToInt32(pFileBytes, addr * 4)

        'Populate Ftab from a section of pFileBytes read from *.ftf
        'ReDim Ftab(HA - LA)
        'For FtabPos As Integer = LA To HA
        '    Ftab(FtabPos - LA) = BitConverter.ToSingle(pFileBytes, FtabPos * 4)
        'Next

        i = FreeFile()
        FileOpen(i, f.NameTsd, OpenMode.Binary, OpenAccess.Read) 'Len = f.RecLen
        spos = 0 'fix this!!!!
        Apos = 0
        For pos = spos To spos + pDates.numValues - 1
            Apos = Apos + 1
            lp = (pos * f.RecLen) + (tcol * 4) + 5
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior.
            FileGet(i, r, lp)
            If X > 0 Then ' need to use ftable
                ya = r - e
                Select Case f.LocDir(lind).FtabTyp
                    Case 20 : F90_XLKT20(addr, ya, a(Apos), t(Apos), dt(Apos), k(Apos), dk(Apos), b(Apos), db(Apos))
                    Case 23 : F90_XLKT23(addr, ya, a(Apos), t(Apos), dt(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos))
                    Case 21 : F90_XLKT21(addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos))
                    Case 22 : F90_XLKT22(addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), alp(Apos), dalp(Apos), qc(Apos))
                    Case 24 : F90_XLKT24(addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos))
                    Case 25 : F90_XLKT25(addr, ya, a(Apos), t(Apos), dt(Apos), j(Apos), k(Apos), dk(Apos), b(Apos), db(Apos), alp(Apos), dalp(Apos), qc(Apos), ma(Apos), dma(Apos), mq(Apos), dmq(Apos))
                End Select
            Else
                a(Apos) = r
            End If
            'flg(Apos) = 0 'Start at index 0
        Next pos
        FileClose(i)

        If X > 0 Then
            FindSetTS(LocName, "A", a)
            FindSetTS(LocName, "T", t)
            FindSetTS(LocName, "DT", dt)
            FindSetTS(LocName, "J", j)
            FindSetTS(LocName, "K", k)
            FindSetTS(LocName, "DK", dk)
            FindSetTS(LocName, "B", b)
            FindSetTS(LocName, "DB", db)
            FindSetTS(LocName, "ALP", alp)
            FindSetTS(LocName, "DALP", dalp)
            FindSetTS(LocName, "QC", qc)
            FindSetTS(LocName, "MA", ma)
            FindSetTS(LocName, "DMA", dma)
            FindSetTS(LocName, "MQ", mq)
            FindSetTS(LocName, "DMQ", dmq)
        Else
            lFillTS.Values = a
            'dataObject.flags = flg
            lFillTS.Attributes.DiscardCalculated()
        End If
        'Logger.Dbg "(CLOSE)"
    End Sub

    Private Sub FindSetTS(ByRef aLocation As String, ByRef aConstituent As String, ByRef setValues() As Double)
        Dim SetTS As atcTimeseries
        Dim i As Integer
        Dim lValues() As Double ', Msg$
        If UBound(setValues) > 0 Then
            SetTS = FindTS(aLocation, aConstituent)
            If SetTS Is Nothing Then
                Logger.Dbg("Did not find " & aLocation & " " & aConstituent)
            Else
                ReDim lValues(UBound(setValues))
                'MsgBox "About to set values(" & LBound(setValues) & " - " & UBound(setValues) & ") for " & loc & " " & con, vbOKOnly, "FindSetTS"
                For i = LBound(setValues) To UBound(setValues)
                    lValues(i) = setValues(i)
                Next
                SetTS.Values = lValues
                'MsgBox "Successfully set values(" & UBound(setValues) & " for " & loc & " " & con, vbOKOnly, "FindSetTS"
                SetTS.Attributes.DiscardCalculated()
                'MsgBox "Successfully calculated summary for " & loc & " " & con, vbOKOnly, "FindSetTS"
            End If
        End If
    End Sub

    'UPGRADE_NOTE: loc was upgraded to loc_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Function FindTS(ByRef loc_Renamed As String, ByRef con As String) As atcTimeseries
        Dim vTS As Object
        'UPGRADE_NOTE: Object FindTS may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        FindTS = Nothing
        For Each vTS In pData
            'UPGRADE_WARNING: Couldn't resolve default property of object vTS.Header. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If vTS.Header.loc = loc_Renamed Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vTS.Header. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If vTS.Header.con = con Then
                    FindTS = vTS
                    Exit For
                End If
            End If
        Next vTS
    End Function

    Private Sub FtfRead()
        Dim u, l, i, j As Integer
        'Dim kb, kl, p As Integer
        'Dim s As String
        'Dim ftfDumpFilename As String
        ReDim f.ftf(0)

        'ftfDumpFilename = GetTmpFileName
        'Open ftfDumpFilename For Output As 99

        l = UBound(pFileBytes)
        For i = 0 To l - 3 Step 4
            j = BitConverter.ToInt32(pFileBytes, i)
            If j >= 20 And j <= 26 Then
                u = UBound(f.ftf) + 1
                ReDim Preserve f.ftf(u)
                With f.ftf(u)
                    .Bindex = i - 8
                    .Lindex = (.Bindex - 1) / 4
                    .Type = j
                    If .Type = 20 Then
                        .Off = 5
                    ElseIf .Type = 21 Then
                        .Off = 6
                    ElseIf .Type = 22 Then
                        .Off = 8
                    Else
                        .Off = 3 'add others
                    End If
                    .HA = BitConverter.ToInt32(pFileBytes, .Bindex)
                    .NTab = BitConverter.ToInt32(pFileBytes, .Bindex + 4)
                    .LSta = BitConverter.ToInt32(pFileBytes, .Bindex + 12)
                    's = NumFmtI(.Type, 2) & " " & NumFmtI(.Bindex, 12) & " " & NumFmtI(.Lindex, 12) & " " & _
                    ''    NumFmtI(.HA, 12) & " " & NumFmtI((.LSta), 12) & " " & NumFmtI(.NTab, 12)
                    'Print #99, s
                    'If .HA < 100000# Then
                    '  For kl = .Lindex + 9 To .HA Step .Off
                    '    kb = kl * 4
                    '    s = "   " & NumFmtI(kb, 12) & " " & NumFmtI(kl, 12) & " "
                    '    For p = 1 To .Off
                    '      s = s & NumFmted(Byte2Single(pFileBytes, kb), 12, 3) & " "
                    '      kb = kb + 4
                    '    Next p
                    '    Print #99, s
                    '  Next kl
                    '  Print #99, " "
                    'End If
                End With
            End If
        Next i
        'Close #99
        'Kill ftfDumpFilename
    End Sub

    Private Sub FeoRead()
        Dim lData As atcTimeseries
        'Dim lDataHeader As ATCData.ATTimSerDataHeader
        Dim j, i, b As Short
        Dim s As String
        Dim d, jdif As Double
        Dim l As Integer
        Dim UserIDWidth, LabelWidth, NodeIdWidth, FieldStart As Integer
        'Dim datsum As ATCData.ATTimSerDateSummary
        Dim SJDay As Double
        Dim EJDay As Double
        Dim NVALS As Integer

        Dim path As String
        Dim progress As String
        Dim fileformat As Integer

        LabelWidth = 8
        NodeIdWidth = 6
        UserIDWidth = 12 '8
        XTIOFF = 7
        fileformat = 0

        On Error GoTo error_Renamed

        Logger.Dbg("(OPEN FEO File)")
        Logger.Dbg("(BUTTOFF CANCEL)")
        Logger.Dbg("(BUTTOFF PAUSE)")
        Logger.Dbg("(MSG1 " & f.NameFeo & ")")
        
        progress = "i = FreeFile(0)"
        i = FreeFile()
        progress = "Open f.NameFeo For Input As #i"
        FileOpen(i, f.NameFeo, OpenMode.Input)
        progress = "path = PathNameOnly(f.NameFeo)"
        path = PathNameOnly(f.NameFeo)
        progress = "ChDriveDir " & path
        ChDriveDir(path)
        progress = "ReDim f.LocDir(0) "
        ReDim f.LocDir(0)

        Do While Not EOF(i) ' Loop until end of file.
            progress = "Line Input #i, s"
            s = LineInput(i)
            Select Case Trim(s)

                Case "-VERSION" : s = LineInput(i)
                    f.version = Trim(s)

                Case "-FILE_FORMAT", "-FORMAT" : s = LineInput(i)
                    If IsNumeric(s) Then
                        fileformat = CInt(s)
                        If fileformat = 1 Or fileformat = 2 Then
                            LabelWidth = 16
                            NodeIdWidth = 7
                            UserIDWidth = 16
                            If fileformat = 1 Then
                                XTIOFF = 21
                            ElseIf fileformat = 2 Then
                                XTIOFF = 0
                            End If
                        ElseIf fileformat > 2 Then
                            MsgBox("This format (version " & s & ") is not recognized." & vbCr & "This program only knows about version 0 and 1.", MsgBoxStyle.Critical, "FEO Read Error")
                            Exit Sub
                        End If
                    End If

                Case "-FILES" : s = LineInput(i) ': f.NameFeo = Trim(s)
                    f.Scenario = IO.Path.GetFileNameWithoutExtension(f.NameFeo)
                    s = LineInput(i) : f.NameTsd = Trim(s)
                    s = LineInput(i) : f.NameFtf = Trim(s)
                    'Hack here to get rid of the extra relative directories from f.NameTsd
                    'so as to just leave the filename itself
                    f.NameTsd = IO.Path.GetFileName(f.NameTsd)
                    f.NameFtf = IO.Path.GetFileName(f.NameFtf)
                Case "-UNITS" 'nothing yet

                Case "-GATE_PUMP_STATUS" 'nothing yet

                Case "-FTF" : s = LineInput(i) 'NUMBER_OF_FULL_RECORDS=
                    f.NumbFullRec = CInt(Right(s, 8))
                    s = LineInput(i) 'ITEMS_PER_RECORD=
                    f.ItemPerRec = CInt(Right(s, 8))
                    s = LineInput(i) 'LETOVER_ITEM_KNT=
                    f.LeftItemCnt = CInt(Right(s, 8))
                    'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                    If Len(Dir(f.NameFtf)) > 0 Then
                        Logger.Dbg("(MSG2 Loading FTF File)")
                        progress = "Loading FTF: " & f.NameFtf
                        pFileBytes = IO.File.ReadAllBytes(f.NameFtf)
                        Logger.Dbg("(MSG2 Cleaning FTF File - " & UBound(pFileBytes) & " bytes)")
                        progress = "FtnUnFmtClean"
                        pFileBytes = FtnUnFmtClean(pFileBytes)
                        Logger.Dbg("(MSG2 Scanning FTF File)")
                        progress = "FtfRead"
                        FtfRead()
                    Else
                        ReDim pFileBytes(0)
                        pErrorDescription = "FTF file not found: " & f.NameFtf
                    End If
                    'Call F90_FILTAB(f.NameFtf, f.NumbFullRec, f.ItemPerRec, f.LeftItemCnt, Len(f.NameFtf))
                    '****^^^^^^^^^^***********
                Case "-TSDDIR"
                    Logger.Status("Reading Node Information in FEO TSDDIR")
                    s = LineInput(i) 'First line of column labels
                    s = LineInput(i) 'Second line of labels
                    s = LineInput(i) 'Dashes separating labels from table
                    b = 0
                    j = 0
                    While b >= 0
                        s = LineInput(i)
                        If IsNumeric(Mid(s, LabelWidth + 9, 5)) Then
                            b = CShort(Trim(Mid(s, LabelWidth + 9, 5)))
                        Else
                            b = -1
                        End If
                        If b >= 0 Then
                            With f.LocDir(j)
                                .Branch = b
                                FieldStart = 2

                                .Label = Trim(Mid(s, FieldStart, LabelWidth))
                                FieldStart = FieldStart + LabelWidth + 1

                                .LocClass = Trim(Mid(s, FieldStart, 5))
                                FieldStart = FieldStart + 12

                                .NodeID = Trim(Mid(s, FieldStart, NodeIdWidth))
                                FieldStart = FieldStart + NodeIdWidth + 1

                                .Station = CSng(Trim(Mid(s, FieldStart, 15)))
                                FieldStart = FieldStart + 16

                                .Invert = CSng(Trim(Mid(s, FieldStart, 10)))
                                FieldStart = FieldStart + 11

                                .UserID = Trim(Mid(s, FieldStart, UserIDWidth))
                                FieldStart = FieldStart + UserIDWidth + 1

                                .FtabIndex = CInt(Trim(Mid(s, FieldStart, 8)))
                                FieldStart = FieldStart + 9

                                'Call F90_TABTYP(.FtabIndex, .FtabTyp)
                                .FtabTyp = FtabTyp(.FtabIndex)
                                .SeriesOffset = CInt(Mid(s, FieldStart, 8))
                                FieldStart = FieldStart + 9
                                If Len(Trim(Mid(s, FieldStart, 12))) > 0 Then
                                    If IsNumeric(Trim(Mid(s, FieldStart, 12))) Then
                                        .Easting = CDbl(Trim(Mid(s, FieldStart, 12)))
                                    End If
                                End If
                                FieldStart = FieldStart + 13
                                If Len(Trim(Mid(s, FieldStart, 12))) > 0 Then
                                    If IsNumeric(Trim(Mid(s, FieldStart, 12))) Then
                                        .Northing = CDbl(Trim(Mid(s, FieldStart, 12)))
                                    End If
                                End If
                                ' update later
                                .Location = Trim(.NodeID)
                                .Constit = New Collection
                                .Constit.Add("FLOW")
                                .Constit.Add("WSELEV")
                                If .LocClass = "LPR" And fileformat <> 2 Then 'dont know how to handle for format2
                                    .Constit.Add("SUR_AREA")
                                    .Constit.Add("VOLUME")
                                Else
                                    SetConstitFromTabType(f.LocDir(j))
                                End If
                            End With
                            j = j + 1
                            ReDim Preserve f.LocDir(j)
                        End If
                    End While
                    f.LocCount = UBound(f.LocDir)

                Case "-TSD"
                    Logger.Dbg("(MSG2 Reading Date Information in FEO TSD)")
                    s = LineInput(i) 'LOCATION_KNT=
                    s = Trim(Mid(s, 14, 8))
                    If IsNumeric(s) Then
                        If CInt(s) <> f.LocCount Then
                            MsgBox("Warning: Location count in '" & f.NameFeo & "' = " & s & vbCr & "But number of locations read = " & f.LocCount, MsgBoxStyle.Critical, "FEO Read")
                        End If
                    End If
                    s = LineInput(i) 'Start time labels
                    s = LineInput(i) 'Start time values
                    SJDay = CDbl(Mid(s, 23, 15))
                    s = LineInput(i) 'End time labels
                    s = LineInput(i) 'End time values
                    EJDay = CDbl(Mid(s, 23, 15))
                    NVALS = CInt(Mid(s, 40, 10))
                    Logger.Dbg("(MSG2 Start Day = " & DumpDate((SJDay)) & " End = " & DumpDate((EJDay)) & " NVALS = " & NVALS & ")")

                Case "-TERMINATION" : f.Term = LineInput(i)
                Case Else
                    Logger.Dbg("(MSG2 Skipping line " & s & ")")
            End Select
        Loop
        FileClose(i)
        'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Logger.Dbg("(MSG2 Creating Dates)")
        f.RecLen = (f.LocCount + 1) * 8

        'Need to re-jigger if he has fixed FEQ to not save bogus zero element of array


        'UPGRADE_NOTE: Object pDates may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        pDates = Nothing
        pDates = New atcTimeseries(Me)

        'Read in all the dates - maybe we don't have to do this right away
        progress = "If Len(Dir(f.NameTsd)) = 0 Then -- " & f.NameTsd

        If Len(Dir(f.NameTsd)) = 0 Then
            pErrorDescription = "Could not find TSD file " & f.NameTsd
            'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Logger.Dbg("(MSG2 " & pErrorDescription & ")")
            Exit Sub
        End If

        'datsum.NVALS = datsum.NVALS - 1
        Dim jday(NVALS) As Double 'julian day of values
        Dim jdflg(NVALS) As Integer

        'jday(0) = SJDay

        progress = "Open f.NameTsd For Random "
        FileOpen(i, f.NameTsd, OpenMode.Random, , , f.RecLen)
        For j = 1 To NVALS
            'UPGRADE_WARNING: Get was upgraded to FileGet and has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FileGet(i, d, j)
            jday(j) = d + FEQ_JULIAN_OFFSET
            jdflg(j) = JDateIntrvl(d + FEQ_JULIAN_OFFSET)
        Next j
        FileClose(i)

        'TODO: save interval lengths as value attributes? pDates.flags = VB6.CopyArray(jdflg)
        pDates.Values = jday

        'Dim lDateDiff As Double = jday(2) - jday(1)
        'Dim lTimeUnit As atcTimeUnit
        'Dim lTimeStep As Integer
        'If lDateDiff / JulianSecond > 1.0 Then
        '    lTimeUnit = atcTimeUnit.TUSecond
        '    lTimeStep = lDateDiff / JulianSecond + 0.1
        'End If
        'If lDateDiff / JulianMinute > 1.0 Then
        '    lTimeUnit = atcTimeUnit.TUMinute
        '    lTimeStep = lDateDiff / JulianMinute + 0.1
        'End If
        'If lDateDiff / JulianHour > 1.0 Then
        '    lTimeUnit = atcTimeUnit.TUHour
        '    lTimeStep = lDateDiff / JulianHour + 0.1
        'End If
        'If lDateDiff / JulianHour / 24.0 > 1.0 Then
        '    lTimeUnit = atcTimeUnit.TUDay
        '    lTimeStep = lDateDiff / JulianHour / 24.0 + 0.1
        'End If

        '  pDates.calcSummary 'This could cause shifting of date when the data is constant interval - better to skip it
        pDatesPopulated = True

        'UPGRADE_WARNING: Couldn't resolve default property of object pMonitor.SendMonitorMessage. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Logger.Dbg("(MSG2 Creating Datsets)")
        For j = 0 To f.LocCount - 1
            For i = 1 To f.LocDir(j).Constit.Count()
                'UPGRADE_NOTE: Object lData may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                lData = Nothing
                lData = New atcTimeseries(Me)
                lData.ValuesNeedToBeRead = True
                With lData.Attributes
                    .SetValue("ID", j * f.LocDir(j).Constit.Count() + i)
                    .SetValue("Location", f.LocDir(j).Location)

                    .SetValue("Constituent", f.LocDir(j).Constit.Item(i))
                    .SetValue("Scenario", f.Scenario)
                    .SetValue("Description", "FEO:" & .GetValue("Scenario") & ":" & .GetValue("Location") & ":" & .GetValue("Constituent"))

                    .SetValue("BRANCH", CStr(f.LocDir(j).Branch))
                    .SetValue("NODE", f.LocDir(j).NodeID)
                    If j < f.LocCount - 1 Then 'this is a guess!
                        .SetValue("DSNODE", f.LocDir(j + 1).NodeID)
                    End If
                    .SetValue("DISTANCE", CStr(f.LocDir(j).Station))
                    .SetValue("INVERT", CStr(f.LocDir(j).Invert))
                    .SetValue("FtabType", CStr(f.LocDir(j).FtabTyp))
                    .SetValue("FtabIndex", CStr(f.LocDir(j).FtabIndex))
                    .SetValue("LocClass", f.LocDir(j).LocClass)
                    .SetValue("SeriesOffset", CStr(f.LocDir(j).SeriesOffset))
                    .SetValue("UserID", f.LocDir(j).UserID)

                    'lData.SetInterval(lTimeUnit, lTimeStep)
                End With
                lData.Dates = pDates
                pData.Add(lData)
            Next i
        Next j

        GoTo e

error_Renamed:
        Logger.Msg("Error #" & Err.Number & ": " & Err.Description & vbCrLf & "NameFeo = " & f.NameFeo & vbCrLf & "CurDir = " & CurDir() & vbCrLf & "progress = " & progress & vbCrLf & "s = " & s & vbCrLf & "pErrorDescription = " & pErrorDescription)

e:
        Logger.Status("HIDE")

        On Error GoTo 0
    End Sub
End Class
