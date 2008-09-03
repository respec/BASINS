Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcSegmentation
Imports atcControls
Imports System.Collections.ObjectModel
Imports System.IO

Public Class frmXSect

    Dim pCurrentReachNum As Integer
    Dim pCurrentFTab As HspfFtable
    Dim pHspfFTable As HspfFtable
    Private FileName As String
    Dim chanid$(), ChanL!(), ChanYm!(), ChanWm!(), ChanN!(), ChanS!()
    Dim ChanM11!(), ChanM12!(), ChanYc!(), ChanM21!(), ChanM22!()
    Dim ChanYt1!(), ChanYt2!(), ChanM31!(), ChanM32!(), ChanW11!()
    Dim ChanW12!(), ChanRecCnt&

    Public Sub ReadPTFFile(ByVal newName As String, ByVal ret As Integer)

        Dim delim, quote, lstr, lTname, tstr As String
        Dim lFreeFile, amax As Integer

        ret = 0
        delim = " "
        quote = """"

        'read ptf file for channel data
        lFreeFile = FreeFile()
        'On Error GoTo ErrHandler
        lTname = Microsoft.VisualBasic.Left(newName, Len(newName) - 3) & "ptf"
        FileOpen(lFreeFile, lTname, OpenMode.Input)
        lstr = LineInput(lFreeFile) 'header line
        ChanRecCnt = 0
        ReDim chanid(1)
        ReDim ChanL(1)
        ReDim ChanYm(1)
        ReDim ChanWm(1)
        ReDim ChanN(1)
        ReDim ChanS(1)
        ReDim ChanM11(1)
        ReDim ChanM12(1)
        ReDim ChanYc(1)
        ReDim ChanM21(1)
        ReDim ChanM22(1)
        ReDim ChanYt1(1)
        ReDim ChanYt2(1)
        ReDim ChanM31(1)
        ReDim ChanM32(1)
        ReDim ChanW11(1)
        ReDim ChanW12(1)
        Do Until EOF(lFreeFile)
            lstr = LineInput(lFreeFile)
            ChanRecCnt = ChanRecCnt + 1
            amax = UBound(chanid)
            If ChanRecCnt > amax Then
                ReDim Preserve chanid(amax * 2)
                ReDim Preserve ChanL(amax * 2)
                ReDim Preserve ChanYm(amax * 2)
                ReDim Preserve ChanWm(amax * 2)
                ReDim Preserve ChanN(amax * 2)
                ReDim Preserve ChanS(amax * 2)
                ReDim Preserve ChanM11(amax * 2)
                ReDim Preserve ChanM12(amax * 2)
                ReDim Preserve ChanYc(amax * 2)
                ReDim Preserve ChanM21(amax * 2)
                ReDim Preserve ChanM22(amax * 2)
                ReDim Preserve ChanYt1(amax * 2)
                ReDim Preserve ChanYt2(amax * 2)
                ReDim Preserve ChanM31(amax * 2)
                ReDim Preserve ChanM32(amax * 2)
                ReDim Preserve ChanW11(amax * 2)
                ReDim Preserve ChanW12(amax * 2)
            End If
            chanid(ChanRecCnt) = StrSplit(lstr, delim, quote) 'reach id
            ChanL(ChanRecCnt) = StrSplit(lstr, delim, quote)  'length
            ChanYm(ChanRecCnt) = StrSplit(lstr, delim, quote) 'mean depth
            ChanWm(ChanRecCnt) = StrSplit(lstr, delim, quote) 'mean width
            ChanN(ChanRecCnt) = StrSplit(lstr, delim, quote)  'mann n
            ChanS(ChanRecCnt) = StrSplit(lstr, delim, quote)  'long slope
            If ChanS(ChanRecCnt) < 0.0001 Then
                ChanS(ChanRecCnt) = 0.0001
            End If
            tstr = StrSplit(lstr, delim, quote)
            ChanM31(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope upper left
            ChanM21(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope lower left
            ChanW11(ChanRecCnt) = StrSplit(lstr, delim, quote)  'zero slope width left
            ChanM11(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope chan left
            ChanM12(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope chan right
            ChanW12(ChanRecCnt) = StrSplit(lstr, delim, quote)  'zero slope width right
            ChanM22(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope lower right
            ChanM32(ChanRecCnt) = StrSplit(lstr, delim, quote)  'side slope upper right
            ChanYc(ChanRecCnt) = StrSplit(lstr, delim, quote)  'channel depth
            ChanYt1(ChanRecCnt) = StrSplit(lstr, delim, quote)  'depth at slope change
            ChanYt2(ChanRecCnt) = StrSplit(lstr, delim, quote)  'channel max depth
        Loop
        FileClose()
        Exit Sub
        'ErrHandler:
        '        Call MsgBox("Problem reading file " & tname, , "Create Problem")
        '        ret = 3
    End Sub

    Public Sub GetPTFData(ByVal RCHId As String, ByVal ChanRecCnt As Integer, ByRef ArrayVals() As Single)
        Dim lOper As Integer
        Dim Id As String

        lOper = Len(RCHId)
        If lOper > 0 Then  'have a reach id
            Id = CInt(RCHId)
            For lOper = 1 To ChanRecCnt
                If Trim(chanid(lOper)) = Id Then  'found the oneq
                    ArrayVals(1) = ChanL(lOper)
                    ArrayVals(2) = ChanYm(lOper)
                    ArrayVals(3) = ChanWm(lOper)
                    ArrayVals(4) = ChanN(lOper)
                    ArrayVals(5) = ChanS(lOper)
                    ArrayVals(6) = ChanM32(lOper)
                    ArrayVals(7) = ChanM22(lOper)
                    ArrayVals(8) = ChanW12(lOper)
                    ArrayVals(9) = ChanM12(lOper)
                    ArrayVals(10) = ChanM11(lOper)
                    ArrayVals(11) = ChanW11(lOper)
                    ArrayVals(12) = ChanM21(lOper)
                    ArrayVals(13) = ChanM31(lOper)
                    ArrayVals(14) = ChanYc(lOper)
                    ArrayVals(15) = ChanYt1(lOper)
                    ArrayVals(16) = ChanYt2(lOper)
                End If
            Next
        End If
    End Sub
   
    Public Sub cboXFile_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboXFile.SelectedIndexChanged
        Dim ArrayVals(16) As Single
        GetPTFData(cboXFile.Text, ChanRecCnt, ArrayVals)
        With agdXSect.Source
            .CellValue(1, 2) = ArrayVals(1)
            .CellValue(2, 2) = ArrayVals(2)
            .CellValue(3, 2) = ArrayVals(3)
            .CellValue(4, 2) = ArrayVals(4)
            .CellValue(5, 2) = ArrayVals(5)
            .CellValue(6, 2) = ArrayVals(6)
            .CellValue(7, 2) = ArrayVals(7)
            .CellValue(8, 2) = ArrayVals(8)
            .CellValue(9, 2) = ArrayVals(9)
            .CellValue(10, 2) = ArrayVals(10)
            .CellValue(11, 2) = ArrayVals(11)
            .CellValue(12, 2) = ArrayVals(12)
            .CellValue(13, 2) = ArrayVals(13)
            .CellValue(14, 2) = ArrayVals(14)
            .CellValue(15, 2) = ArrayVals(15)
            .CellValue(16, 2) = ArrayVals(16)
        End With
        agdXSect.Refresh()
    End Sub
    Public Sub GetPTFFileIds(ByRef cnt As Integer, ByRef ArrayIds() As String)
        Dim lOper As Integer

        cnt = ChanRecCnt
        ReDim ArrayIds(cnt)
        For lOper = 1 To cnt
            ArrayIds(lOper - 1) = chanid(lOper)
        Next
    End Sub
    

    Public Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        Dim ret&, ArrayIds$(), cnt&, lOper&
        Dim lStream As Stream = Nothing
        ArrayIds = New String() {}

        cnt = 0
        ChDriveDir("\basins\modelout")
        OpenFileDialog1.Filter = "BASINS Trapezoidal Files | *.ptf"
        OpenFileDialog1.FileName = "*.ptf"
        OpenFileDialog1.Title = "Select BASINS Trapezoidal File"
        OpenFileDialog1.ShowDialog()
        'read file here
        'lStream = OpenFileDialog1.OpenFile()
        ReadPTFFile(OpenFileDialog1.FileName, ret)
        If ret = 0 Then
            Call GetPTFFileIds(cnt, ArrayIds)
            cboXFile.Enabled = True
            cboXFile.Items.Clear()
            For lOper = 0 To cnt - 1
                cboXFile.Items.Add(ArrayIds(lOper))
            Next
            cboXFile.SelectedIndex = 0
        End If

    End Sub

    'Private Sub cmdXSect_Click(ByVal Index As Integer)
    '    Dim l!, ym!, wm!, n!, s!, m32!, m22!, w12!
    '    Dim m12!, m11!, w11!, m21!, m31!, yc!, yt1!, yt2!

    '    If Index = 0 Then
    '        'okay
    '        With agdXSect
    '            l = .CellValue(1, 2)
    '            ym = .CellValue(2, 2)
    '            wm = .CellValue(3, 2)
    '            n = .CellValue(4, 2)
    '            s = .CellValue(5, 2)
    '            m32 = .CellValue(6, 2)
    '            m22 = .CellValue(7, 2)
    '            w12 = .CellValue(8, 2)
    '            m12 = .CellValue(9, 2)
    '            m11 = .CellValue(10, 2)
    '            w11 = .CellValue(11, 2)
    '            m21 = .CellValue(12, 2)
    '            m31 = .CellValue(13, 2)
    '            yc = .CellValue(14, 2)
    '            yt1 = .CellValue(15, 2)
    '            yt2 = .CellValue(16, 2)
    '            curftab.FTableFromCrossSect(l, ym, wm, n, s, m11, m12, yc, m21, _
    '                           m22, yt1, yt2, m31, m32, w11, w12)
    '        End With
    '        Unload(Me)
    '    ElseIf Index = 1 Then
    '        'cancel
    '        Unload(Me)
    '    Else
    '        Dim d As HH_AKLINK, h$
    '        d.pszKeywords = "Reach Editor"
    '        d.fReserved = vbFalse
    '        d.cbStruct = LenB(d)
    '        HtmlHelp(Me.hwnd, App.HelpFile, HH_KEYWORD_LOOKUP, d)
    '    End If
    'End Sub

    Private Sub Form_Load()
        Dim lRow, lCol As Integer
        '.net converstion: Disable and fill file combo box with text indicating that file should be loaded
        cboXFile.Text = "<Select A File> "
        cboXFile.Enabled = False

        '.net conversion: Initialize and populate the table
        With agdXSect
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .ColumnWidth(0) = 75
            .ColumnWidth(1) = 300
            .ColumnWidth(2) = 75
        End With

        With agdXSect.Source
            .Columns = 3
            .CellValue(0, 0) = "Variable"
            .CellValue(0, 1) = "Description"
            .CellValue(0, 2) = "Value"
            .CellValue(1, 0) = "L"
            .CellValue(2, 0) = "Ym"
            .CellValue(3, 0) = "Wm"
            .CellValue(4, 0) = "n"
            .CellValue(5, 0) = "S"
            .CellValue(6, 0) = "m32"
            .CellValue(7, 0) = "m22"
            .CellValue(8, 0) = "W12"
            .CellValue(9, 0) = "m12"
            .CellValue(10, 0) = "m11"
            .CellValue(11, 0) = "W11"
            .CellValue(12, 0) = "m21"
            .CellValue(13, 0) = "m31"
            .CellValue(14, 0) = "Yc"
            .CellValue(15, 0) = "Yt1"
            .CellValue(16, 0) = "Yt2"
            For lRow = 1 To 16
                .CellValue(lRow, 2) = 0.01
            Next
            .CellValue(1, 1) = "Length (ft)"
            .CellValue(2, 1) = "Mean Depth (ft)"
            .CellValue(3, 1) = "Mean Width (ft)"
            .CellValue(4, 1) = "Mannings Roughness Coefficient"
            .CellValue(5, 1) = "Longitudinal Slope"
            .CellValue(6, 1) = "Side Slope of Upper Flood Plain Left"
            .CellValue(7, 1) = "Side Slope of Lower Flood Plain Left"
            .CellValue(8, 1) = "Zero Slope Flood Plain Width Left (ft)"
            .CellValue(9, 1) = "Side Slope of Channel Left"
            .CellValue(10, 1) = "Side Slope of Channel Right"
            .CellValue(11, 1) = "Zero Slope Flood Plain Width Right (ft)"
            .CellValue(12, 1) = "Side Slope Lower Flood Plain Right"
            .CellValue(13, 1) = "Side Slope Upper Flood Plain Right"
            .CellValue(14, 1) = "Channel Depth (ft)"
            .CellValue(15, 1) = "Flood Side Slope Change at Depth (ft)"
            .CellValue(16, 1) = "Maximum Depth (ft)"

            For lCol = 0 To 1
                For lRow = 0 To .Rows - 1
                    .CellColor(lRow, lCol) = SystemColors.ControlLight

                Next
            Next
            .CellColor(0, 2) = SystemColors.ControlLight
        End With
        agdXSect.Refresh()

    End Sub

    Public Sub CurrentReach(ByVal lReach As Integer, ByVal lFtab As HspfFtable)
        pCurrentReachNum = lReach
        pCurrentFTab = lFtab
        agdXSectTitle.Text = "FTABLE " & CStr(pCurrentReachNum)
    End Sub

    Friend Sub Init(ByVal aHspfFTable As HspfFtable, ByVal aCtl As ctlEditFTables)
        Me.Icon = aCtl.ParentForm.Icon
        pHspfFTable = aHspfFTable
        Form_Load()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim pChannel As New atcSegmentation.Channel


        With agdXSect.Source
            pChannel.Length = .CellValue(1, 2)
            pChannel.DepthMean = .CellValue(2, 2)
            pChannel.WidthMean = .CellValue(3, 2)
            pChannel.ManningN = .CellValue(4, 2)
            pChannel.SlopeProfile = .CellValue(5, 2)
            pChannel.SlopeSideUpperFPLeft = .CellValue(6, 2)
            pChannel.SlopeSideLowerFPLeft = .CellValue(7, 2)
            pChannel.WidthZeroSlopeLeft = .CellValue(8, 2)
            pChannel.SlopeSideLeft = .CellValue(9, 2)
            pChannel.SlopeSideRight = .CellValue(10, 2)
            pChannel.WidthZeroSlopeRight = .CellValue(11, 2)
            pChannel.SlopeSideLowerFPRight = .CellValue(12, 2)
            pChannel.SlopeSideUpperFPRight = .CellValue(13, 2)
            pChannel.DepthChannel = .CellValue(14, 2)
            pChannel.DepthSlopeChange = .CellValue(15, 2)
            pChannel.DepthMax = .CellValue(16, 2)
            pHspfFTable.FTableFromCrossSect(pChannel)
        End With
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim ArrayVals!(16)
        Dim lFileStream As Stream
        Dim lStr As String
        Dim lFreeFile As Integer

        'CDFile.flags = &H8806&
        SaveFileDialog1.Filter = "BASINS Trapezoidal Files (*.ptf)|*.ptf"
        SaveFileDialog1.FileName = "*.ptf"
        SaveFileDialog1.Title = "Save Cross Section Specifications"
        'On Error GoTo 10
        'CDFile.CancelError = True
        'CDFile.Action = 2
        With agdXSect.Source
            ArrayVals(1) = .CellValue(1, 2)
            ArrayVals(2) = .CellValue(2, 2)
            ArrayVals(3) = .CellValue(3, 2)
            ArrayVals(4) = .CellValue(4, 2)
            ArrayVals(5) = .CellValue(5, 2)
            ArrayVals(6) = .CellValue(6, 2)
            ArrayVals(7) = .CellValue(7, 2)
            ArrayVals(8) = .CellValue(8, 2)
            ArrayVals(9) = .CellValue(9, 2)
            ArrayVals(10) = .CellValue(10, 2)
            ArrayVals(11) = .CellValue(11, 2)
            ArrayVals(12) = .CellValue(12, 2)
            ArrayVals(13) = .CellValue(13, 2)
            ArrayVals(14) = .CellValue(14, 2)
            ArrayVals(15) = .CellValue(15, 2)
            ArrayVals(16) = .CellValue(16, 2)
        End With

        '10:     'continue here on cancel

        lFileStream = SaveFileDialog1.OpenFile()
        lFreeFile = FreeFile()
        '.net conversion: NOTE!!!! VB6 version always called below code with chanid=1
        If Not (lFileStream Is Nothing) Then
            If Len(chanid) > 0 Then  'have a reach id
                lstr = "'Reach Number','Length(ft)','Mean Depth(ft)','Mean Width (ft)'," & _
                       "'Mannings Roughness Coeff.','Long. Slope','Type of x-section','Side slope of upper FP left'," & _
                       "'Side slope of lower FP left','Zero slope FP width left(ft)','Side slope of channel left'," & _
                       "'Side slope of channel right','Zero slope FP width right(ft)','Side slope lower FP right'," & _
                       "'Side slope upper FP right','Channel Depth(ft)','Flood side slope change at depth','Max. depth'," & _
                       "'No. of exits','Fraction of flow through exit 1','Fraction of flow through exit 2'," & _
                       "'Fraction of flow through exit 3','Fraction of flow through exit 4','Fraction of flow through exit 5'"
                Print(lFreeFile, lStr)  'header line
                lStr = "1" & " " & _
                      ArrayVals(1) & " " & _
                      ArrayVals(2) & " " & _
                      ArrayVals(3) & " " & _
                      ArrayVals(4) & " " & _
                      ArrayVals(5) & " " & _
                      "Trapezoidal" & " " & _
                      ArrayVals(13) & " " & _
                      ArrayVals(12) & " " & _
                      ArrayVals(11) & " " & _
                      ArrayVals(10) & " " & _
                      ArrayVals(9) & " " & _
                      ArrayVals(8) & " " & _
                      ArrayVals(7) & " " & _
                      ArrayVals(6) & " " & _
                      ArrayVals(14) & " " & _
                      ArrayVals(15) & " " & _
                      ArrayVals(16) & " " & _
                      "1 1 0 0 0 0"
                Print(lFreeFile, lStr)
                Exit Sub
            End If
            lFileStream.Close()
        End If

    End Sub
End Class