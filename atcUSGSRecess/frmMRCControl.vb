Imports System.IO
Imports atcGraph
Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph
Imports System.Text.RegularExpressions

Public Class frmMRCControl
    Private pFileInitialDir As String = ""
    Private pFileNameStations As String = "Station.txt"
    Private pFileNameRecSum As String = "recsum.txt"
    Private pFileStationFullName As String = ""
    Private pFileRecSumFullName As String = ""

    Public FirstMRC As clsMRC = Nothing
    Private WithEvents pZgc As ZedGraphControl
    Private WithEvents pZgcUA As ZedGraphControl
    Private pGrapher As clsGraphBase
    Private pGrapherUA As clsGraphBase

    Private pGraphDataGroup As atcTimeseriesGroup
    Private pGraphDataGroupUA As atcTimeseriesGroup

    Private pMRCGroup As atcCollection
    Private pMRCSelectedGroup As atcCollection

    Public Sub Initialize(ByVal aFileRecSum As String, Optional ByVal Args As atcDataAttributes = Nothing)
        Dim lFirstMRC As clsMRC = Nothing
        If Args IsNot Nothing Then
            With Args
                lFirstMRC = .GetValue("FirstMRC", Nothing)
                pFileInitialDir = .GetValue("WorkingDirectory", "")
            End With
        End If

        pMRCGroup = New atcCollection()
        pMRCSelectedGroup = New atcCollection()
        pGraphDataGroup = New atcTimeseriesGroup()
        pGraphDataGroupUA = New atcTimeseriesGroup()
        If lFirstMRC IsNot Nothing Then
            FirstMRC = lFirstMRC
        End If

        InitMasterPanes()
        lstEquations.Items.Clear()

        PopulateForm()
        Me.Show()

    End Sub

    Public Property Grapher() As clsGraphBase
        Get
            Return pGrapher
        End Get
        Set(ByVal newValue As clsGraphBase)
            pGrapher = newValue
            RefreshGraph()
        End Set
    End Property

    Public Property GrapherUA() As clsGraphBase
        Get
            Return pGrapherUA
        End Get
        Set(ByVal newValue As clsGraphBase)
            pGrapherUA = newValue
            RefreshGraphUA()
        End Set
    End Property

    Private Sub InitMasterPanes()
        If pZgc Is Nothing Then
            pZgc = CreateZgc()
            Me.Controls.Add(pZgc)
            panelGraph.Controls.Add(pZgc)
            With pZgc
                .Dock = System.Windows.Forms.DockStyle.Fill
                '.IsEnableHZoom = mnuViewHorizontalZoom.Checked
                '.IsEnableHPan = mnuViewHorizontalZoom.Checked
                '.IsEnableVZoom = mnuViewVerticalZoom.Checked
                '.IsEnableVPan = mnuViewVerticalZoom.Checked
                '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
            End With
        Else
            pZgc.GraphPane.CurveList.Clear()
        End If
        RefreshGraph()

        If pZgcUA Is Nothing Then
            pZgcUA = CreateZgc()
            Me.Controls.Add(pZgcUA)
            panelGraphUA.Controls.Add(pZgcUA)
            With pZgcUA
                .Dock = System.Windows.Forms.DockStyle.Fill
                '.IsEnableHZoom = mnuViewHorizontalZoom.Checked
                '.IsEnableHPan = mnuViewHorizontalZoom.Checked
                '.IsEnableVZoom = mnuViewVerticalZoom.Checked
                '.IsEnableVPan = mnuViewVerticalZoom.Checked
                '.IsZoomOnMouseCenter = mnuViewZoomMouse.Checked
            End With
        Else
            pZgcUA.GraphPane.CurveList.Clear()
        End If
        RefreshGraphUA()
    End Sub

    Public Sub RefreshGraph()
        pZgc.AxisChange()
        Invalidate()
        Refresh()
    End Sub

    Public Sub RefreshGraphUA()
        pZgcUA.AxisChange()
        Invalidate()
        Refresh()
    End Sub

    Private Sub RefreshGraphMRC(ByVal aDataGroup As atcTimeseriesGroup, ByVal aDataGroupUA As atcTimeseriesGroup)
        If pGrapher IsNot Nothing Then
            With pGrapher.ZedGraphCtrl.GraphPane
                .YAxis.Title.Text = ""
            End With
            pGrapher = Nothing
        End If
        pGrapher = New clsGraphTime(aDataGroup, pZgc)
        'pGrapher = New clsGraphOrdinal(aDataGroup, pZgc)
        With pGrapher.ZedGraphCtrl.GraphPane
            '.YAxis.Type = AxisType.Log
            .YAxis.Title.Text = "LOG OF FLOW"
            .XAxis.Title.Text = "TIME, IN DAYS"
            .AxisChange()
            '.CurveList.Item(0).Color = Drawing.Color.Red
            '.Legend.IsVisible = False
            '.CurveList.Item(1).Color = Drawing.Color.DarkBlue
            'CType(.CurveList.Item(1), LineItem).Line.Width = 2
        End With
        pZgc.Refresh()

        If pGrapherUA IsNot Nothing Then
            With pGrapherUA.ZedGraphCtrl.GraphPane
                .YAxis.Title.Text = ""
            End With
            pGrapherUA = Nothing
        End If
        pGrapherUA = New clsGraphTime(aDataGroupUA, pZgcUA)
        'pGrapherUA = New clsGraphOrdinal(aDataGroup, pZgcUA)
        With pGrapherUA.ZedGraphCtrl.GraphPane
            '.YAxis.Type = AxisType.Log
            .YAxis.Title.Text = "FLOW PER UNIT AREA"
            .XAxis.Title.Text = "TIME, IN DAYS"
            .AxisChange()
            '.CurveList.Item(0).Color = Drawing.Color.Red
            '.Legend.IsVisible = False
            '.CurveList.Item(1).Color = Drawing.Color.DarkBlue
            'CType(.CurveList.Item(1), LineItem).Line.Width = 2
            'For I As Integer = 0 To pGrapher.ZedGraphCtrl.GraphPane.CurveList.Count - 1
            '.CurveList.Item(I).Color = pGrapher.ZedGraphCtrl.GraphPane.CurveList.Item(I).Color
            'Next
        End With
        pZgcUA.Refresh()

    End Sub

    Private Sub PopulateForm()
        If Not String.IsNullOrEmpty(pFileInitialDir) AndAlso Directory.Exists(pFileInitialDir) Then
            pFileRecSumFullName = Path.Combine(pFileInitialDir, "recsum.txt")
        Else
            pFileRecSumFullName = ""
        End If
        'pFileStationFullName = GetSetting("atcUSGSRecess", "Defaults", "FileStation", "")
        'pFileRecSumFullName = GetSetting("atcUSGSRecess", "Defaults", "FileRecSum", "")
        'If Not File.Exists(pFileRecSumFullName) Then
        '    pFileRecSumFullName = FindFile("Find recsum.txt", "recsum.txt", "txt")
        '    SaveSetting("atcUSGSRecess", "Defaults", "FileRecSum", pFileRecSumFullName)
        'End If
        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        If FirstMRC IsNot Nothing Then
            ParseMRCParams(FirstMRC.RecSum, True)
            AddMRC(FirstMRC)
        End If

        If File.Exists(pFileRecSumFullName) Then
            PopulateRecSums()
        End If
        Me.Width += 175
    End Sub

    Private Sub AddMRC(ByVal aMRC As clsMRC)
        If aMRC.BuildMRC Then
            pMRCGroup.Add(aMRC)
        End If

        With aMRC
            Dim lFoundMatch As Boolean = False
            Dim lArr() As String = Regex.Split(.RecSum, "\s+")
            Dim lDA As Double
            If lArr.Length > 12 AndAlso Double.TryParse(lArr(12), lDA) Then
                Dim lYearArr() As String = lArr(2).Split("-")
                Dim lthisEquation As String = "" 'MRCToAdd(lArr(0), lArr(12), lArr(1), lArr(7), lArr(8), lArr(9), lArr(10), lArr(11))
                If lYearArr.Length = 2 Then
                    lthisEquation = MRCToAdd(lArr(0), lArr(12), lArr(1), lArr(7), lArr(8), lArr(9), lArr(10), lArr(11), lYearArr(0), lYearArr(1))
                Else
                    lthisEquation = MRCToAdd(lArr(0), lArr(12), lArr(1), lArr(7), lArr(8), lArr(9), lArr(10), lArr(11), "", "")
                End If
                'key to use is the RecSum string
                For Each lItem As String In lstEquations.Items
                    If lItem.ToLower = lthisEquation.ToLower Then
                        lFoundMatch = True
                        Exit For
                    End If
                Next
                If Not lFoundMatch Then
                    lstEquations.Items.Add(lthisEquation)
                End If
            End If
        End With

    End Sub

    Private Sub PopulateRecSums()
        If lstRecSum.Items.Count > 0 Then
            'lstRecSum.Items.Clear()
        Else
            Dim lTitleLine1 As String = "    File    S     P      #  Kmin  Kmed  Kmax  LogQmn  LogQmx     A         B         C       DA   Stnam"
            Dim lTitleLine2 As String = "-------------------------------------------------------------------------------------------------------------"
            '                            Indian.txt  s 1971-1972  3  11.8  22.9  25.9   0.057   0.709   5.8238  -25.3040   15.0121    22.9 HUNT_RIVER_NEAR_EAST_GREENWICH__RI
            lstRecSum.Items.Add(lTitleLine1)
            lstRecSum.Items.Add(lTitleLine2)
        End If

        Dim lSR As New StreamReader(pFileRecSumFullName)
        Dim lOneLine As String
        Dim lLinesRead As Integer = 1

        Dim lArr() As String = Nothing
        While Not lSR.EndOfStream
            lOneLine = lSR.ReadLine()
            If Not ParseRecSumRecord(lOneLine, lArr) Then
                If Not ParseRecSumRecord(lOneLine, lArr, False) Then
                    Continue While
                End If
            End If

            Dim lFoundMatch As Boolean = False
            'key to use is the RecSum string
            For Each lItem As String In lstRecSum.Items
                If lItem.ToLower = lOneLine.ToLower Then
                    lFoundMatch = True
                    Exit For
                End If
            Next
            If Not lFoundMatch Then
                lstRecSum.Items.Add(lOneLine)
            End If
        End While

        lSR.Close()
        lSR = Nothing
        'SaveSetting("atcUSGSRecess", "Defaults", "FileRecSum", pFileRecSumFullName)
    End Sub

    Private Function ParseRecSumRecord(ByVal aLine As String, ByRef Arr() As String, Optional ByVal aFixedWidth As Boolean = True) As Boolean
        Dim lRecordIsValid As Boolean = True
        ReDim Arr(0)
        '17 FORMAT (A12,A1,1X,1I4,'-',1I4,1I3,3F6.1,2F8.3,1F9.4,2F10.4, 1F8.1,Awhatever)
        If Not String.IsNullOrEmpty(aLine) AndAlso aLine.Length >= 89 Then
            Dim lColumns As Integer = 0
            Dim lDA As String = ""
            Dim lStartIndex As Integer = 2
            With aLine
                If aFixedWidth Then
                    Dim lStn As String = .Substring(clsRecess.RecSumColumn.c1Stn, clsRecess.RecSumColW.c1Stn)
                    Dim lSn As String = .Substring(clsRecess.RecSumColumn.c2Sn, clsRecess.RecSumColW.c2Sn)
                    Dim lYrS As String = .Substring(clsRecess.RecSumColumn.c3YrS, clsRecess.RecSumColW.c3YrS)
                    Dim lYrE As String = .Substring(clsRecess.RecSumColumn.c4YrE, clsRecess.RecSumColW.c4YrE)
                    Dim lSegCt As String = .Substring(clsRecess.RecSumColumn.c5SegCt, clsRecess.RecSumColW.c5SegCt)
                    Dim lKmin As String = .Substring(clsRecess.RecSumColumn.c6Kmin, clsRecess.RecSumColW.c6Kmin)
                    Dim lKmed As String = .Substring(clsRecess.RecSumColumn.c7Kmed, clsRecess.RecSumColW.c7Kmed)
                    Dim lKmax As String = .Substring(clsRecess.RecSumColumn.c8Kmax, clsRecess.RecSumColW.c8Kmax)
                    Dim lMinLogQC As String = .Substring(clsRecess.RecSumColumn.c9MinLogQC, clsRecess.RecSumColW.c9MinLogQC)
                    Dim lMaxLogQC As String = .Substring(clsRecess.RecSumColumn.c10MaxLogQC, clsRecess.RecSumColW.c10MaxLogQC)
                    Dim lCoA As String = .Substring(clsRecess.RecSumColumn.c11CoA, clsRecess.RecSumColW.c11CoA)
                    Dim lCoB As String = .Substring(clsRecess.RecSumColumn.c12CoB, clsRecess.RecSumColW.c12CoB)
                    Dim lCoC As String = .Substring(clsRecess.RecSumColumn.c13CoC, clsRecess.RecSumColW.c13CoC)
                    lColumns = 13
                    If aLine.Length >= 97 Then
                        lDA = .Substring(clsRecess.RecSumColumn.c14DA, clsRecess.RecSumColW.c14DA)
                        lColumns = 14
                    End If
                    ReDim Arr(lColumns - 1)
                    Arr(0) = lStn
                    Arr(1) = lSn
                    Arr(2) = lYrS
                    Arr(3) = lYrE
                    Arr(4) = lSegCt
                    Arr(5) = lKmin
                    Arr(6) = lKmed
                    Arr(7) = lKmax
                    Arr(8) = lMinLogQC
                    Arr(9) = lMaxLogQC
                    Arr(10) = lCoA
                    Arr(11) = lCoB
                    Arr(12) = lCoC
                Else
                    Arr = Regex.Split(aLine, "\s+")
                    Dim lpat As String = "^[1-9]\d{3}-[1-9]\d{3}$"
                    If Not Regex.IsMatch(Arr(2), lpat) Then
                        lRecordIsValid = False
                    End If
                    lStartIndex = 3
                    lColumns = 12
                End If

                If lRecordIsValid Then
                    If Arr.Length < 13 Then
                        lRecordIsValid = False
                    Else
                        'Eliminate lines by pattern, at least 13 values and the 3rd to 13th are numeric
                        For I As Integer = lStartIndex To 12
                            If Not IsNumeric(Arr(I)) Then
                                lRecordIsValid = False
                                Exit For
                            End If
                        Next
                    End If

                    If lColumns = 14 Then
                        Arr(13) = lDA
                    End If
                End If
            End With
        Else
            lRecordIsValid = False
        End If
        Return lRecordIsValid
    End Function

    Private Sub PopulateEquations()
        If lstRecSum.Items.Count < 3 Then
            Logger.Msg("No RECSUM records for constructing MRCs.", MsgBoxStyle.Information, "Auto Populate Equation Listing")
            Exit Sub
        End If

        Dim lArrRecSum() As String = Nothing
        Dim lMRCToAdd As String
        For CntRecSum As Integer = 2 To lstRecSum.Items.Count - 1
            If ParseRecSumRecord(lstRecSum.Items(CntRecSum), lArrRecSum) Then
                lMRCToAdd = MRCToAdd(lArrRecSum(clsRecess.RecSumFldIndex.c1Stn),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c14DA),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c2Sn),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c9MinLogQC),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c10MaxLogQC),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c11CoA),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c12CoB),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c13CoC),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c3YrS),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c4YrE))
                If Not lstEquations.Items.Contains(lMRCToAdd) Then
                    lstEquations.Items.Add(lMRCToAdd)
                End If
            ElseIf ParseRecSumRecord(lstRecSum.Items(CntRecSum), lArrRecSum, False) Then
                lMRCToAdd = MRCToAdd(lArrRecSum(clsRecess.RecSumFldIndex.c1Stn),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c14DA - 1),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c2Sn),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c9MinLogQC - 1),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c10MaxLogQC - 1),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c11CoA - 1),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c12CoB - 1),
                                     lArrRecSum(clsRecess.RecSumFldIndex.c13CoC - 1),
                                     lArrRecSum(2).Substring(0, 4),
                                     lArrRecSum(2).Substring(lArrRecSum(2).IndexOf("-") + 1))
                If Not lstEquations.Items.Contains(lMRCToAdd) Then
                    lstEquations.Items.Add(lMRCToAdd)
                End If
            End If
        Next
    End Sub

    Private Function MRCToAdd(ByVal aStation As String, ByVal aDA As String, ByVal aSeason As String,
                              ByVal aLogQMin As String, ByVal aLogQMax As String,
                              ByVal aCoeffA As String, ByVal aCoeffB As String, ByVal aCoeffC As String,
                              ByVal aYearStart As String,
                              ByVal aYearEnd As String) As String
        Dim lMRCToAdd As String = aStation & "," & aDA & ",(" & aSeason & "),"
        Dim lRange As String = "(" & aLogQMin & "~" & aLogQMax & "),"
        lMRCToAdd &= lRange
        Dim lEquation As String = "Coeff.A:" & aCoeffA & ",Coeff.B:" & aCoeffB & ",Coeff.C:" & aCoeffC
        lMRCToAdd &= lEquation
        If Not String.IsNullOrEmpty(aYearStart) AndAlso Not String.IsNullOrEmpty(aYearEnd) Then
            lMRCToAdd &= "," & aYearStart & "," & aYearEnd
        End If
        Return lMRCToAdd
    End Function

    Private Sub BuildMRCs()
        Dim lStation As String
        Dim lDA As Double
        Dim lMinLogQ As Double
        Dim lMaxLogQ As Double
        Dim lCoA As Double
        Dim lCoB As Double
        Dim lCoC As Double
        Dim lSeason As String
        Dim lYearStart As String
        Dim lYearEnd As String

        Dim lMRCOutputDir As String = GetSetting("atcUSGSRecess", "Defaults", "FileRecSumDir", "")
        If Not String.IsNullOrEmpty(pFileRecSumFullName) Then
            lMRCOutputDir = Path.GetDirectoryName(pFileRecSumFullName)
        End If
        If Not IO.Directory.Exists(lMRCOutputDir) Then
            Logger.Msg("Please specify an output directory for MRC curve text table. Suggested paths are:" & vbCrLf & _
                       "- Directory where 'recsum.txt' is located" & vbCrLf & _
                       "- Or any other directories" & vbCrLf & vbCrLf & _
                       "Please note that if no directory is specified, curve text table will not be saved.", MsgBoxStyle.Information, "Plot MRC")
            Dim FolderBrowserDialog1 As New System.Windows.Forms.FolderBrowserDialog
            With FolderBrowserDialog1
                ' Desktop is the root folder in the dialog.
                .RootFolder = Environment.SpecialFolder.Desktop
                ' Select directory on entry.
                .SelectedPath = "C:\"
                ' Prompt the user with a custom message.
                .Description = "Specify MRC curve text table output directory"
                If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    ' Display the selected folder if the user clicked on the OK button.
                    lMRCOutputDir = .SelectedPath
                    SaveSetting("atcUSGSRecess", "Defaults", "FileRecSumDir", lMRCOutputDir)
                End If
            End With
        End If
        Dim lCurvFileName As String = ""
        If IO.Directory.Exists(lMRCOutputDir) Then
            lCurvFileName = Path.Combine(lMRCOutputDir, "curvout.txt")
        End If
        txtMRCTable.Text = clsMRC.CurvOutHeader
        For Each lItem As String In lstEquations.CheckedItems

            Dim lArr() As String = lItem.Split(",")

            'get station
            lStation = lArr(0)

            'get DA
            'lDA = Double.Parse(lArr(1))
            If Not Double.TryParse(lArr(1), lDA) Then
                lDA = 0
            End If

            'get season
            lSeason = lArr(2).Replace("(", "").Replace(")", "")

            'get Min, max LogQ
            Dim lArr1() As String = lArr(3).Replace("(", "").Replace(")", "").Split("~")
            lMinLogQ = Double.Parse(lArr1(0))
            lMaxLogQ = Double.Parse(lArr1(1))

            'get Coeffs
            lCoA = Double.Parse(lArr(4).Substring("Coeff.A:".Length))
            lCoB = Double.Parse(lArr(5).Substring("Coeff.B:".Length))
            lCoC = Double.Parse(lArr(6).Substring("Coeff.C:".Length))

            'get years
            lYearStart = ""
            lYearEnd = ""
            If lArr.Length = 9 Then
                lYearStart = lArr(7)
                lYearEnd = lArr(8)
            End If

            Dim lMRC As New clsMRC
            With lMRC
                .Station = lStation
                .DrainageArea = lDA
                .CoeffA = lCoA
                .CoeffB = lCoB
                .CoeffC = lCoC
                .MaxLogQ = lMaxLogQ
                .MinLogQ = lMinLogQ
                .Season = lSeason
                .YearStart = lYearStart
                .YearEnd = lYearEnd
                If .BuildMRC() Then
                    .FileCurvOut = lCurvFileName
                    Try
                        txtMRCTable.Text &= .WriteCurvTable()
                    Catch ex As Exception
                        'Do Nothing
                    End Try
                    pGraphDataGroup.Add(.CurveData)
                    If .CurveDataUA IsNot Nothing Then
                        pGraphDataGroupUA.Add(.CurveDataUA)
                    End If
                End If
            End With
            pMRCGroup.Add(lMRC)
        Next
        txtMRCTable.SelectionLength = 0
    End Sub

    Private Sub lstRecSum_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstRecSum.SelectedIndexChanged
        Dim lSelectedIndex As Integer = lstRecSum.SelectedIndex
        If lSelectedIndex < 2 Then
            txtCoefA.Text = ""
            txtCoefB.Text = ""
            txtCoefC.Text = ""
            txtLogQMin.Text = ""
            txtLogQMax.Text = ""
            txtSeason.Text = ""
            txtYearStart.Text = ""
            txtYearEnd.Text = ""
            Exit Sub
        End If
        'ParseMRCParams(lstRecSum.SelectedItem)

        Dim lArr() As String = Nothing
        If ParseRecSumRecord(lstRecSum.SelectedItem, lArr) Then
            txtStation.Text = lArr(0)
            txtSeason.Text = lArr(1)
            txtLogQMin.Text = lArr(8)
            txtLogQMax.Text = lArr(9)
            txtCoefA.Text = lArr(10)
            txtCoefB.Text = lArr(11)
            txtCoefC.Text = lArr(12)
            txtYearStart.Text = lArr(2)
            txtYearEnd.Text = lArr(3)
            Dim lDA As Double
            If lArr.Length >= 14 Then
                If Not lArr(13).StartsWith("N/A") AndAlso Double.TryParse(lArr(13), lDA) Then
                    txtDA.Text = lArr(13)
                End If
            End If
        ElseIf ParseRecSumRecord(lstRecSum.SelectedItem, lArr, False) Then
            txtStation.Text = lArr(0)
            txtSeason.Text = lArr(1)
            txtLogQMin.Text = lArr(7)
            txtLogQMax.Text = lArr(8)
            txtCoefA.Text = lArr(9)
            txtCoefB.Text = lArr(10)
            txtCoefC.Text = lArr(11)
            txtYearStart.Text = lArr(2).Substring(0, 4)
            txtYearEnd.Text = lArr(2).Substring(lArr(2).IndexOf("-") + 1)
            Dim lDA As Double
            If lArr.Length >= 13 Then
                If Not lArr(12).StartsWith("N/A") AndAlso Double.TryParse(lArr(12), lDA) Then
                    txtDA.Text = lArr(12)
                End If
            End If
        End If
    End Sub

    Private Sub ParseMRCParams(ByVal aItem As String, Optional ByVal aComplete As Boolean = False)
        Dim lArr() As String = Regex.Split(aItem, "\s+")
        If aComplete Then
            txtStation.Text = lArr(0)
        End If
        txtSeason.Text = lArr(1)
        txtLogQMin.Text = lArr(7)
        txtLogQMax.Text = lArr(8)
        txtCoefA.Text = lArr(9)
        txtCoefB.Text = lArr(10)
        txtCoefC.Text = lArr(11)
        Dim lYearArr() As String = lArr(2).Split("-")
        If lYearArr.Length = 2 Then
            If IsNumeric(lYearArr(0)) AndAlso IsNumeric(lYearArr(1)) Then
                txtYearStart.Text = lYearArr(0)
                txtYearEnd.Text = lYearArr(1)
            End If
        End If
        Dim lDA As Double
        If lArr.Length > 12 Then
            If Not lArr(12).StartsWith("N/A") AndAlso Double.TryParse(lArr(12), lDA) Then
                txtDA.Text = lDA.ToString
            End If
        End If
    End Sub

    Private Sub btnMRCAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMRCAdd.Click
        If chkAddAllRecSumRecords.Checked Then
            PopulateEquations()
            Exit Sub
        End If
        Dim lMRCToAdd As String = ""
        If txtStation.Text.Trim() = "" OrElse _
           txtLogQMin.Text.Trim() = "" OrElse _
           txtLogQMax.Text.Trim() = "" OrElse _
           txtCoefA.Text.Trim() = "" OrElse _
           txtCoefB.Text.Trim() = "" OrElse _
           txtCoefC.Text.Trim() = "" OrElse _
           txtSeason.Text.Trim() = "" Then

            Logger.Msg("Must specify the following before adding to MRC list:" & vbCrLf & _
                       "- station" & vbCrLf & _
                       "- season" & vbCrLf & _
                       "- min, max LogQ" & vbCrLf & _
                       "- polynomial coefficients A, B, and C", MsgBoxStyle.Information, "Add MRC")
            Exit Sub
        ElseIf Not (IsNumeric(txtCoefA.Text.Trim()) AndAlso _
                    IsNumeric(txtCoefB.Text.Trim()) AndAlso _
                    IsNumeric(txtCoefC.Text.Trim()) AndAlso _
                    IsNumeric(txtLogQMin.Text.Trim()) AndAlso _
                    IsNumeric(txtLogQMax.Text.Trim())) Then
            Logger.Msg("LogQ, and coefficients are not all numeric values.", MsgBoxStyle.Information, "Add MRC")
            Exit Sub
        End If

        'lMRCToAdd = txtStation.Text.Trim() & "," & txtDA.Text.Trim() & ",(" & txtSeason.Text.Trim() & "),"
        'Dim lRange As String = "(" & txtLogQMin.Text.Trim() & "~" & txtLogQMax.Text.Trim() & "),"
        'lMRCToAdd &= lRange
        'Dim lEquation As String = "Coeff.A:" & txtCoefA.Text.Trim() & ",Coeff.B:" & txtCoefB.Text.Trim() & ",Coeff.C:" & txtCoefC.Text.Trim()
        'lMRCToAdd &= lEquation

        'lMRCToAdd = MRCToAdd(txtStation.Text.Trim(), txtDA.Text.Trim(), txtSeason.Text.Trim(), _
        '                     txtLogQMin.Text.Trim(), txtLogQMax.Text.Trim(), _
        '                     txtCoefA.Text.Trim(), txtCoefB.Text.Trim(), txtCoefC.Text.Trim())
        Dim lYearStart As Integer
        Dim lYearEnd As Integer
        Dim lYearStartText As String = ""
        Dim lYearEndText As String = ""
        If Integer.TryParse(txtYearStart.Text, lYearStart) AndAlso Integer.TryParse(txtYearEnd.Text, lYearEnd) Then
            lYearStartText = lYearStart.ToString()
            lYearEndText = lYearEnd.ToString()
        End If
        lMRCToAdd = MRCToAdd(txtStation.Text, txtDA.Text, txtSeason.Text,
                             txtLogQMin.Text, txtLogQMax.Text,
                             txtCoefA.Text, txtCoefB.Text, txtCoefC.Text, lYearStartText, lYearEndText)

        If Not lstEquations.Items.Contains(lMRCToAdd) Then
            lstEquations.Items.Add(lMRCToAdd)
        End If
    End Sub

    Private Sub btnMRCDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMRCDelete.Click
        Dim lSelectedIndex As Integer = lstEquations.SelectedIndex
        If lSelectedIndex = -1 Then Exit Sub
        lstEquations.Items.RemoveAt(lSelectedIndex)
    End Sub

    Private Sub btnMRCPlot_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMRCPlot.Click
        pGraphDataGroup.Clear()
        pGraphDataGroupUA.Clear()
        BuildMRCs()
        RefreshGraphMRC(pGraphDataGroup, pGraphDataGroupUA)
        tabMRCMain.SelectedIndex = 1
    End Sub

    Private Sub btnRecSum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecSum.Click
        Dim lNewFileRecSumFullName As String = ""
        Dim lInitialPath As String = ""
        Try
            lInitialPath = Path.GetDirectoryName(pFileRecSumFullName)
        Catch ex As Exception
            lInitialPath = ""
        End Try
        If lInitialPath = "" Then lInitialPath = "C:\"

        Dim lOpenFileDialog As New System.Windows.Forms.OpenFileDialog()
        With lOpenFileDialog
            '.AutoUpgradeEnabled = False
            .Title = "Browse For RecSum File"
            .InitialDirectory = lInitialPath '"c:\"
            .Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                lNewFileRecSumFullName = .FileName
            End If
            .Dispose()
        End With

        If File.Exists(lNewFileRecSumFullName) Then
            pFileRecSumFullName = lNewFileRecSumFullName
            PopulateRecSums()
        End If
    End Sub

    Private Sub btnMRCClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMRCClear.Click
        lstEquations.Items.Clear()
        txtStation.Text = ""
        txtDA.Text = ""
        txtSeason.Text = ""
        txtLogQMax.Text = ""
        txtLogQMin.Text = ""
        txtCoefA.Text = ""
        txtCoefB.Text = ""
        txtCoefC.Text = ""
        txtYearStart.Text = ""
        txtYearEnd.Text = ""
        RemoveHandler rbSelectAllEqns.CheckedChanged, AddressOf rbSelectAllEqns_CheckedChanged
        RemoveHandler rbSelectNoneEqns.CheckedChanged, AddressOf rbSelectAllEqns_CheckedChanged
        rbSelectAllEqns.Checked = False
        rbSelectNoneEqns.Checked = False
        AddHandler rbSelectAllEqns.CheckedChanged, AddressOf rbSelectAllEqns_CheckedChanged
        AddHandler rbSelectNoneEqns.CheckedChanged, AddressOf rbSelectAllEqns_CheckedChanged
    End Sub

    Private Sub rbSelectAllEqns_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSelectAllEqns.CheckedChanged, _
    rbSelectNoneEqns.CheckedChanged

        For I As Integer = 0 To lstEquations.Items.Count - 1
            If rbSelectAllEqns.Checked Then
                lstEquations.SetItemChecked(I, True)
            ElseIf rbSelectNoneEqns.Checked Then
                lstEquations.SetItemChecked(I, False)
            End If
        Next
    End Sub

    Private Sub btnClearRecSum_Click(sender As Object, e As EventArgs) Handles btnClearRecSum.Click
        If lstRecSum.Items.Count > 0 Then
            lstRecSum.Items.Clear()
        End If
    End Sub
End Class