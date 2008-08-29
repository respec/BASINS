Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcUtility
Imports atcControls
Imports System.Collections.ObjectModel



Public Class frmXSect
    Dim CurrentReach As Integer
    Dim lCurrentFTab As HspfTable
    Dim pUci As HspfUci

    '    Private Sub cboXFile_Click()
    '        Dim ArrayVals(16) As Single
    '        Call GetPTFData(cboXFile.Text, ArrayVals)
    '        With agdXSect
    '            .TextMatrix(1, 2) = ArrayVals(1)
    '            .TextMatrix(2, 2) = ArrayVals(2)
    '            .TextMatrix(3, 2) = ArrayVals(3)
    '            .TextMatrix(4, 2) = ArrayVals(4)
    '            .TextMatrix(5, 2) = ArrayVals(5)
    '            .TextMatrix(6, 2) = ArrayVals(6)
    '            .TextMatrix(7, 2) = ArrayVals(7)
    '            .TextMatrix(8, 2) = ArrayVals(8)
    '            .TextMatrix(9, 2) = ArrayVals(9)
    '            .TextMatrix(10, 2) = ArrayVals(10)
    '            .TextMatrix(11, 2) = ArrayVals(11)
    '            .TextMatrix(12, 2) = ArrayVals(12)
    '            .TextMatrix(13, 2) = ArrayVals(13)
    '            .TextMatrix(14, 2) = ArrayVals(14)
    '            .TextMatrix(15, 2) = ArrayVals(15)
    '            .TextMatrix(16, 2) = ArrayVals(16)
    '        End With
    '    End Sub

    '    Private Sub cmdOpen_Click()
    '        Dim ret&, ArrayIds$(), cnt&, i&

    '        ChDriveDir("\basins\modelout")
    '        CDFile.flags = &H8806&
    '        CDFile.Filter = "BASINS Trapezoidal Files (*.ptf)"
    '        CDFile.FileName = "*.ptf"
    '        CDFile.DialogTitle = "Select BASINS Trapezoidal File"
    '        On Error GoTo 50
    '        CDFile.CancelError = True
    '        CDFile.Action = 1
    '        'read file here
    '        Call ReadPTFFile(CDFile.FileName, ret)
    '        If ret = 0 Then
    '            Call GetPTFFileIds(cnt, ArrayIds)
    '            cboXFile.Clear()
    '            For i = 1 To cnt
    '                cboXFile.AddItem(ArrayIds(i))
    '            Next i
    '            cboXFile.ListIndex = 0
    '        End If
    '50:     'continue here on cancel
    '    End Sub

    '    Private Sub cmdSave_Click()
    '        Dim ArrayVals!(16)

    '        CDFile.flags = &H8806&
    '        CDFile.Filter = "BASINS Trapezoidal Files (*.ptf)|*.ptf"
    '        CDFile.FileName = "*.ptf"
    '        CDFile.DialogTitle = "Save Cross Section Specifications"
    '        On Error GoTo 10
    '        CDFile.CancelError = True
    '        CDFile.Action = 2
    '        With agdXSect
    '            ArrayVals(1) = .TextMatrix(1, 2)
    '            ArrayVals(2) = .TextMatrix(2, 2)
    '            ArrayVals(3) = .TextMatrix(3, 2)
    '            ArrayVals(4) = .TextMatrix(4, 2)
    '            ArrayVals(5) = .TextMatrix(5, 2)
    '            ArrayVals(6) = .TextMatrix(6, 2)
    '            ArrayVals(7) = .TextMatrix(7, 2)
    '            ArrayVals(8) = .TextMatrix(8, 2)
    '            ArrayVals(9) = .TextMatrix(9, 2)
    '            ArrayVals(10) = .TextMatrix(10, 2)
    '            ArrayVals(11) = .TextMatrix(11, 2)
    '            ArrayVals(12) = .TextMatrix(12, 2)
    '            ArrayVals(13) = .TextMatrix(13, 2)
    '            ArrayVals(14) = .TextMatrix(14, 2)
    '            ArrayVals(15) = .TextMatrix(15, 2)
    '            ArrayVals(16) = .TextMatrix(16, 2)
    '        End With
    '        Call WritePTFFile(CDFile.FileName, 1, ArrayVals)
    '10:     'continue here on cancel
    '    End Sub

    '    Private Sub cmdXSect_Click(ByVal Index As Integer)
    '        Dim l!, ym!, wm!, n!, s!, m32!, m22!, w12!
    '        Dim m12!, m11!, w11!, m21!, m31!, yc!, yt1!, yt2!

    '        If Index = 0 Then
    '            'okay
    '            With agdXSect
    '                l = .TextMatrix(1, 2)
    '                ym = .TextMatrix(2, 2)
    '                wm = .TextMatrix(3, 2)
    '                n = .TextMatrix(4, 2)
    '                s = .TextMatrix(5, 2)
    '                m32 = .TextMatrix(6, 2)
    '                m22 = .TextMatrix(7, 2)
    '                w12 = .TextMatrix(8, 2)
    '                m12 = .TextMatrix(9, 2)
    '                m11 = .TextMatrix(10, 2)
    '                w11 = .TextMatrix(11, 2)
    '                m21 = .TextMatrix(12, 2)
    '                m31 = .TextMatrix(13, 2)
    '                yc = .TextMatrix(14, 2)
    '                yt1 = .TextMatrix(15, 2)
    '                yt2 = .TextMatrix(16, 2)
    '                curftab.FTableFromCrossSect(l, ym, wm, n, s, m11, m12, yc, m21, _
    '                               m22, yt1, yt2, m31, m32, w11, w12)
    '            End With
    '            Unload(Me)
    '        ElseIf Index = 1 Then
    '            'cancel
    '            Unload(Me)
    '        Else
    '            Dim d As HH_AKLINK, h$
    '            d.pszKeywords = "Reach Editor"
    '            d.fReserved = vbFalse
    '            d.cbStruct = LenB(d)
    '            HtmlHelp(Me.hwnd, App.HelpFile, HH_KEYWORD_LOOKUP, d)
    '        End If
    '    End Sub

    '    Private Sub Form_Load()
    '        Dim i&
    '        With agdXSect
    '            .cols = 3
    '            .FixedCols = 2
    '            .TextMatrix(0, 0) = "Variable"
    '            .TextMatrix(0, 1) = "Description"
    '            .TextMatrix(0, 2) = "Value"
    '            .TextMatrix(1, 0) = "L"
    '            .TextMatrix(2, 0) = "Ym"
    '            .TextMatrix(3, 0) = "Wm"
    '            .TextMatrix(4, 0) = "n"
    '            .TextMatrix(5, 0) = "S"
    '            .TextMatrix(6, 0) = "m32"
    '            .TextMatrix(7, 0) = "m22"
    '            .TextMatrix(8, 0) = "W12"
    '            .TextMatrix(9, 0) = "m12"
    '            .TextMatrix(10, 0) = "m11"
    '            .TextMatrix(11, 0) = "W11"
    '            .TextMatrix(12, 0) = "m21"
    '            .TextMatrix(13, 0) = "m31"
    '            .TextMatrix(14, 0) = "Yc"
    '            .TextMatrix(15, 0) = "Yt1"
    '            .TextMatrix(16, 0) = "Yt2"
    '            For i = 1 To 16
    '                .TextMatrix(i, 2) = 0.01
    '                .ColType(2) = ATCoSng
    '                .ColMin(2) = 0.00001
    '            Next i
    '            .TextMatrix(1, 1) = "Length (ft)"
    '            .TextMatrix(2, 1) = "Mean Depth (ft)"
    '            .TextMatrix(3, 1) = "Mean Width (ft)"
    '            .TextMatrix(4, 1) = "Mannings Roughness Coefficient"
    '            .TextMatrix(5, 1) = "Longitudinal Slope"
    '            .TextMatrix(6, 1) = "Side Slope of Upper Flood Plain Left"
    '            .TextMatrix(7, 1) = "Side Slope of Lower Flood Plain Left"
    '            .TextMatrix(8, 1) = "Zero Slope Flood Plain Width Left (ft)"
    '            .TextMatrix(9, 1) = "Side Slope of Channel Left"
    '            .TextMatrix(10, 1) = "Side Slope of Channel Right"
    '            .TextMatrix(11, 1) = "Zero Slope Flood Plain Width Right (ft)"
    '            .TextMatrix(12, 1) = "Side Slope Lower Flood Plain Right"
    '            .TextMatrix(13, 1) = "Side Slope Upper Flood Plain Right"
    '            .TextMatrix(14, 1) = "Channel Depth (ft)"
    '            .TextMatrix(15, 1) = "Flood Side Slope Change at Depth (ft)"
    '            .TextMatrix(16, 1) = "Maximum Depth (ft)"
    '            .ColEditable(2) = True
    '            .ColsSizeByContents()
    '        End With
    '    End Sub

    '    Public Sub CurrentReach(ByVal r&, ByVal ftab As HspfFtable)
    '        currch = r
    '        curftab = ftab
    '        agdXSect.Header = "FTABLE " & CStr(currch)
    '    End Sub

    '    Private Sub Form_Resize()
    '        If Not (Me.WindowState = vbMinimized) Then
    '            If Width < 1500 Then Width = 1500
    '            If Height < 1500 Then Height = 1500
    '            agdXSect.Width = Width - 500
    '            fraXFile.Width = Width - 500
    '            cmdOpen.Left = 200
    '            cmdSave.Left = fraXFile.Width - cmdSave.Width - 200
    '            cboXFile.Left = (fraXFile.Width - cboXFile.Width) / 2
    '            cmdXSect(0).Left = agdXSect.Left + (agdXSect.Width / 2) - (1.5 * cmdXSect(0).Width) - 400
    '            cmdXSect(4).Left = agdXSect.Left + (agdXSect.Width / 2) + (0.5 * cmdXSect(0).Width) + 400
    '            cmdXSect(1).Left = agdXSect.Left + (agdXSect.Width / 2) - (0.5 * cmdXSect(1).Width)
    '            agdXSect.Height = Height - (4 * cmdXSect(1).Height) + 50
    '            cmdXSect(1).Top = Height - agdXSect.Top - cmdXSect(1).Height - 200 + fraXFile.Height
    '            cmdXSect(0).Top = Height - agdXSect.Top - cmdXSect(0).Height - 200 + fraXFile.Height
    '            cmdXSect(4).Top = Height - agdXSect.Top - cmdXSect(4).Height - 200 + fraXFile.Height
    '        End If
    '    End Sub
    Friend Sub Init(ByVal aCtl As ctlEditFTables)
        Me.Icon = aCtl.ParentForm.Icon
    End Sub
End Class