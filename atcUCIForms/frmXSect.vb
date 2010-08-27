Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports System.IO

Public Class frmXSect

    Dim pFTableCtl As Object 'the control containing the ftable grid
    Dim pCurrentReachNum As Integer
    Dim pCurrentFTab As HspfFtable
    Dim pHspfFTable As HspfFtable
    Private FileName As String
    Dim chanid, ChanL, ChanYm, ChanWm, ChanN, ChanS, ChanM11, ChanM12, ChanYc, ChanM21, ChanM22, ChanYt1, ChanYt2 As New ArrayList
    Dim ChanM31, ChanM32, ChanW11, ChanW12 As New ArrayList
    Dim pPrevSelectedId As Integer

    Public Sub cboXFile_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboXFile.SelectedIndexChanged

        SaveDisplayedXSecParams(pPrevSelectedId)

        RefreshXFiles(cboXFile.SelectedIndex)

        pPrevSelectedId = cboXFile.SelectedIndex

    End Sub
    Private Sub RefreshXFiles(ByVal lSelectedXFileIndex As Integer)
        If cboXFile.Enabled Then  'have a reach id
            With agdXSect.Source
                .CellValue(1, 2) = ChanL(lSelectedXFileIndex)
                .CellValue(2, 2) = ChanYm(lSelectedXFileIndex)
                .CellValue(3, 2) = ChanWm(lSelectedXFileIndex)
                .CellValue(4, 2) = ChanN(lSelectedXFileIndex)
                .CellValue(5, 2) = ChanS(lSelectedXFileIndex)
                .CellValue(6, 2) = ChanM32(lSelectedXFileIndex)
                .CellValue(7, 2) = ChanM22(lSelectedXFileIndex)
                .CellValue(8, 2) = ChanW12(lSelectedXFileIndex)
                .CellValue(9, 2) = ChanM12(lSelectedXFileIndex)
                .CellValue(10, 2) = ChanM11(lSelectedXFileIndex)
                .CellValue(11, 2) = ChanW11(lSelectedXFileIndex)
                .CellValue(12, 2) = ChanM21(lSelectedXFileIndex)
                .CellValue(13, 2) = ChanM31(lSelectedXFileIndex)
                .CellValue(14, 2) = ChanYc(lSelectedXFileIndex)
                .CellValue(15, 2) = ChanYt1(lSelectedXFileIndex)
                .CellValue(16, 2) = ChanYt2(lSelectedXFileIndex)
            End With
            agdXSect.Refresh()
        End If
    End Sub

    Public Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
        Dim lLineString As String
        Dim lOper As Integer
        Dim lFileStream As Stream

        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        OpenFileDialog1.InitialDirectory = lBasinsFolder & "\modelout"
        OpenFileDialog1.Filter = "BASINS Trapezoidal Files | *.ptf"
        OpenFileDialog1.FileName = "*.ptf"
        OpenFileDialog1.Title = "Select BASINS Trapezoidal File"

        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                lFileStream = OpenFileDialog1.OpenFile()
                Using sr As StreamReader = New StreamReader(lFileStream)

                    lLineString = sr.ReadLine() 'Skip the big header line
                    lLineString = sr.ReadLine()
                    Do
                        Dim lSplitString As String() = lLineString.Split(New [Char]() {" "c, ","c})

                        chanid.Add(lSplitString(0)) 'reach id
                        ChanL.Add(lSplitString(1))  'length
                        ChanYm.Add(lSplitString(2)) 'mean depth
                        ChanWm.Add(lSplitString(3)) 'mean width
                        ChanN.Add(lSplitString(4))  'mann n
                        ChanS.Add(lSplitString(5))  'long slope
                        'If ChanS(ChanS.Count) < 0.0001 Then
                        '    ChanS.Remove(ChanS.Count)
                        '    ChanS.Add(0.0001)
                        'End If
                        ChanM31.Add(lSplitString(7))  'side slope upper left
                        ChanM21.Add(lSplitString(8))  'side slope lower left
                        ChanW11.Add(lSplitString(9))  'zero slope width left
                        ChanM11.Add(lSplitString(10))  'side slope chan left
                        ChanM12.Add(lSplitString(11))  'side slope chan right
                        ChanW12.Add(lSplitString(12))  'zero slope width right
                        ChanM22.Add(lSplitString(13))  'side slope lower right
                        ChanM32.Add(lSplitString(14))  'side slope upper right
                        ChanYc.Add(lSplitString(15))  'channel depth
                        ChanYt1.Add(lSplitString(16))  'depth at slope change
                        ChanYt2.Add(lSplitString(17))  'channel max depth

                        'Advance one line and read
                        lLineString = sr.ReadLine()
                    Loop Until lLineString Is Nothing
                    sr.Close()
                End Using

                cboXFile.Enabled = True
                cboXFile.Items.Clear()
                For lOper = 0 To chanid.Count - 1
                    cboXFile.Items.Add(chanid(lOper))
                Next
                RefreshXFiles(0)
                pPrevSelectedId = 0
                cboXFile.SelectedIndex = 0

            Catch Ex As Exception
                Logger.Msg("Error Opening File", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Open Cross Section FTABLE Error")
            End Try
        End If
    End Sub

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
                For lRow = 1 To .Rows - 1
                    .CellColor(lRow, lCol) = SystemColors.ControlLight
                Next
            Next

            For lRow = 1 To .Rows - 1
                .CellEditable(lRow, 2) = True
            Next

            agdXSect.Source.FixedRows = 1

        End With
        agdXSect.Refresh()
    End Sub

    Public Sub CurrentReach(ByVal lReach As Integer, ByVal lFtab As HspfFtable)
        pCurrentReachNum = lReach
        pCurrentFTab = lFtab
        agdXSectTitle.Text = "FTABLE " & pCurrentReachNum
    End Sub

    Friend Sub Init(ByVal aHspfFTable As HspfFtable, ByVal aCtl As ctlEditFTables)
        Me.Icon = aCtl.ParentForm.Icon
        Me.Text = "Import From Cross Section"
        pHspfFTable = aHspfFTable
        pFTableCtl = aCtl
        Form_Load()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lChannel As New atcSegmentation.Channel

        With agdXSect.Source
            lChannel.Length = .CellValue(1, 2)
            lChannel.DepthMean = .CellValue(2, 2)
            lChannel.WidthMean = .CellValue(3, 2)
            lChannel.ManningN = .CellValue(4, 2)
            lChannel.SlopeProfile = .CellValue(5, 2)
            lChannel.SlopeSideUpperFPLeft = .CellValue(6, 2)
            lChannel.SlopeSideLowerFPLeft = .CellValue(7, 2)
            lChannel.WidthZeroSlopeLeft = .CellValue(8, 2)
            lChannel.SlopeSideLeft = .CellValue(9, 2)
            lChannel.SlopeSideRight = .CellValue(10, 2)
            lChannel.WidthZeroSlopeRight = .CellValue(11, 2)
            lChannel.SlopeSideLowerFPRight = .CellValue(12, 2)
            lChannel.SlopeSideUpperFPRight = .CellValue(13, 2)
            lChannel.DepthChannel = .CellValue(14, 2)
            lChannel.DepthSlopeChange = .CellValue(15, 2)
            lChannel.DepthMax = .CellValue(16, 2)
            pCurrentFTab.FTableFromCrossSect(lChannel)
        End With
        pFTableCtl.UpdateFTablesFromXSect()
        Me.Close()
    End Sub

    Private Sub SaveDisplayedXSecParams(ByVal lSelectedXFileIndex As Integer)

        If cboXFile.Enabled Then
            With agdXSect.Source
                ChanL(lSelectedXFileIndex) = .CellValue(1, 2)
                ChanYm(lSelectedXFileIndex) = .CellValue(2, 2)
                ChanWm(lSelectedXFileIndex) = .CellValue(3, 2)
                ChanN(lSelectedXFileIndex) = .CellValue(4, 2)
                ChanS(lSelectedXFileIndex) = .CellValue(5, 2)
                ChanM32(lSelectedXFileIndex) = .CellValue(6, 2)
                ChanM22(lSelectedXFileIndex) = .CellValue(7, 2)
                ChanW12(lSelectedXFileIndex) = .CellValue(8, 2)
                ChanM12(lSelectedXFileIndex) = .CellValue(9, 2)
                ChanM11(lSelectedXFileIndex) = .CellValue(10, 2)
                ChanW11(lSelectedXFileIndex) = .CellValue(11, 2)
                ChanM21(lSelectedXFileIndex) = .CellValue(12, 2)
                ChanM31(lSelectedXFileIndex) = .CellValue(13, 2)
                ChanYc(lSelectedXFileIndex) = .CellValue(14, 2)
                ChanYt1(lSelectedXFileIndex) = .CellValue(15, 2)
                ChanYt2(lSelectedXFileIndex) = .CellValue(16, 2)
            End With
            agdXSect.Refresh()
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim lFileStream As Stream
        Dim lWriter As StreamWriter
        Dim lOper As Integer
        Dim lstr As String

        SaveFileDialog1.Filter = "BASINS Trapezoidal Files (*.ptf)|*.ptf"
        SaveFileDialog1.FileName = "*.ptf"
        SaveFileDialog1.Title = "Save Cross Section Specifications"

        SaveDisplayedXSecParams(cboXFile.SelectedIndex)

        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try
                lFileStream = SaveFileDialog1.OpenFile()

                lWriter = New StreamWriter(lFileStream)
                lstr = "'Reach Number','Length(ft)','Mean Depth(ft)','Mean Width (ft)'," & _
                           "'Mannings Roughness Coeff.','Long. Slope','Type of x-section','Side slope of upper FP left'," & _
                           "'Side slope of lower FP left','Zero slope FP width left(ft)','Side slope of channel left'," & _
                           "'Side slope of channel right','Zero slope FP width right(ft)','Side slope lower FP right'," & _
                           "'Side slope upper FP right','Channel Depth(ft)','Flood side slope change at depth','Max. depth'," & _
                           "'No. of exits','Fraction of flow through exit 1','Fraction of flow through exit 2'," & _
                           "'Fraction of flow through exit 3','Fraction of flow through exit 4','Fraction of flow through exit 5'"
                lWriter.WriteLine(lstr)

                If chanid.Count = 0 Then
                    lstr = "1 0.01 0.01 0.01 0.01 0.01 Trapezoidal 0.01 0.01 0.01 0.01 0.01 0.01 0.01 0.01 0.01 0.01 0.01 1 1 0 0 0 0"
                    lWriter.WriteLine(lstr)
                Else
                    For lOper = 0 To chanid.Count - 1
                        With agdXSect.Source
                            lstr = chanid(lOper) & " " & _
                               ChanL(lOper) & " " & _
                                       ChanYm(lOper) & " " & _
                                       ChanWm(lOper) & " " & _
                                       ChanN(lOper) & " " & _
                                       ChanS(lOper) & " " & _
                                       "Trapezoidal" & " " & _
                                       ChanM31(lOper) & " " & _
                                       ChanM21(lOper) & " " & _
                                       ChanW11(lOper) & " " & _
                                       ChanM11(lOper) & " " & _
                                       ChanM12(lOper) & " " & _
                                       ChanW12(lOper) & " " & _
                                       ChanM22(lOper) & " " & _
                                       ChanM32(lOper) & " " & _
                                       ChanYc(lOper) & " " & _
                                       ChanYt1(lOper) & " " & _
                                       ChanYt2(lOper) & " " & _
                                       "1 1 0 0 0 0"
                            lWriter.WriteLine(lstr)
                        End With
                    Next
                End If
                lWriter.Close()
            Catch Ex As Exception
                Logger.Msg("Error Saving File", Microsoft.VisualBasic.MsgBoxStyle.OkOnly, "Save Cross Section FTABLE Error")
            End Try
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click
        'TODO: Add this Code
    End Sub
End Class