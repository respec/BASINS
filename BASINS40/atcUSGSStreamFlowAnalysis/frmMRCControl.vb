Imports System.IO
Imports atcGraph
Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports ZedGraph
Imports System.Text.RegularExpressions

Public Class frmMRCControl
    Private pMaster As ZedGraph.MasterPane

    Private pFileInitialDir As String = ""
    Private pFileNameStations As String = "Station.txt"
    Private pFileNameRecSum As String = "recsum.txt"
    Private pFileStationFullName As String = ""
    Private pFileRecSumFullName As String = ""

    Public FirstMRC As clsMRC = Nothing
    Private WithEvents pZgc As ZedGraphControl
    Private pGrapher As clsGraphBase

    Private pGraphDataGroup As atcTimeseriesGroup

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
        If lFirstMRC IsNot Nothing Then
            FirstMRC = lFirstMRC
        End If

        InitMasterPane()
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

    Private Sub InitMasterPane()
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
                pMaster = .MasterPane

            End With
        Else
            pZgc.GraphPane.CurveList.Clear()
        End If
        RefreshGraph()
    End Sub

    Public Sub RefreshGraph()
        pZgc.AxisChange()
        Invalidate()
        Refresh()
    End Sub

    Private Sub RefreshGraphMRC(ByVal aDataGroup As atcTimeseriesGroup)
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
    End Sub

    Private Sub PopulateForm()
        pFileStationFullName = GetSetting("atcUSGSRecess", "Defaults", "FileStation", "")
        pFileRecSumFullName = GetSetting("atcUSGSRecess", "Defaults", "FileRecSum", "")

        If Not File.Exists(pFileStationFullName) Then
            pFileStationFullName = FindFile("Find station.txt", "station.txt", "txt")
            SaveSetting("atcUSGSRecess", "Defaults", "FileStation", pFileStationFullName)
        End If

        If Not File.Exists(pFileRecSumFullName) Then
            pFileRecSumFullName = FindFile("Find recsum.txt", "recsum.txt", "txt")
            SaveSetting("atcUSGSRecess", "Defaults", "FileRecSum", pFileRecSumFullName)
        End If
        RepopulateForm()
    End Sub

    Private Sub RepopulateForm()
        If FirstMRC IsNot Nothing Then AddMRC(FirstMRC)

        If File.Exists(pFileStationFullName) Then
            PopulateStations()
        End If

        If File.Exists(pFileRecSumFullName) Then
            PopulateRecSums()
        End If

    End Sub

    Private Sub AddMRC(ByVal aMRC As clsMRC)
        If aMRC.BuildMRC Then
            pMRCGroup.Add(aMRC)
        End If

        With aMRC
            Dim lFoundMatch As Boolean = False
            'key to use is the RecSum string
            For Each lItem As String In lstEquations.Items
                If lItem.ToLower = .RecSum.ToLower Then
                    lFoundMatch = True
                    Exit For
                End If
            Next
            If Not lFoundMatch Then
                lstEquations.Items.Add(.RecSum)
            End If
        End With

    End Sub

    Private Sub PopulateStations()

        If lstStations.Items.Count > 0 Then lstStations.Items.Clear()
        Dim lTitleLine1 As String = "Station      DA(sqmi)  Optional Info (Free form)"
        Dim lTitleLine2 As String = "---------------------- ------------------------------------------"

        'Station      DA(sqmi)  Optional Info (Free form)
        '---------------------- ------------------------------------------
        'Indian.txt      8.88   02371200  Indian Creek near Troy Alabama
        lstStations.Items.Add(lTitleLine1)
        lstStations.Items.Add(lTitleLine2)

        Dim lSR As New StreamReader(pFileStationFullName)
        Dim lOneLine As String
        Dim lTitleSectionLines As Integer = 10
        Dim lLinesRead As Integer = 1

        While Not lSR.EndOfStream
            lOneLine = lSR.ReadLine
            If lLinesRead < lTitleSectionLines Then
                lLinesRead += 1
                Continue While
            End If

            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Trim.Length < 2 Then
                    Continue While
                End If

                Dim lFoundMatch As Boolean = False
                'key to use is the RecSum string
                For Each lItem As String In lstStations.Items
                    If lItem.ToLower = lOneLine.ToLower Then
                        lFoundMatch = True
                        Exit For
                    End If
                Next
                If Not lFoundMatch Then
                    lstStations.Items.Add(lOneLine)
                End If
            End While
        End While
        lSR.Close()
        lSR = Nothing
        SaveSetting("atcUSGSRecess", "Defaults", "FileStation", pFileStationFullName)
    End Sub

    Private Sub PopulateRecSums()

        If lstRecSum.Items.Count > 0 Then lstRecSum.Items.Clear()
        Dim lTitleLine1 As String = "    File    S     P      #  Kmin  Kmed  Kmax  LogQmn  LogQmx     A         B         C"
        Dim lTitleLine2 As String = "-----------------------------------------------------------------------------------------"
        '                            Indian.txt  s 1971-1972  3  11.8  22.9  25.9   0.057   0.709   5.8238  -25.3040   15.0121
        lstRecSum.Items.Add(lTitleLine1)
        lstRecSum.Items.Add(lTitleLine2)

        Dim lSR As New StreamReader(pFileRecSumFullName)
        Dim lOneLine As String
        Dim lTitleSectionLines As Integer = 22
        Dim lLinesRead As Integer = 1

        While Not lSR.EndOfStream
            lOneLine = lSR.ReadLine
            If lLinesRead < lTitleSectionLines Then
                lLinesRead += 1
                Continue While
            End If
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Trim.Length < 2 Then
                    Continue While
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
        End While
        lSR.Close()
        lSR = Nothing
        SaveSetting("atcUSGSRecess", "Defaults", "FileRecSum", pFileRecSumFullName)
    End Sub

    Private Function RecSumStationMatched() As Boolean
        Dim lMatched As Boolean = True
        If txtStation.Text.Trim() = "" OrElse _
           txtDA.Text.Trim() = "" OrElse _
           txtLogQMin.Text.Trim() = "" OrElse _
           txtLogQMax.Text.Trim() = "" OrElse _
           txtCoefA.Text.Trim() = "" OrElse _
           txtCoefB.Text.Trim() = "" OrElse _
           txtCoefC.Text.Trim() = "" Then

            lMatched = False

        ElseIf lstRecSum.SelectedIndex < 2 OrElse lstStations.SelectedIndex < 2 Then
            lMatched = False
        Else
            Dim lArrRecSum() As String = Regex.Split(lstRecSum.SelectedItem, "\s+")
            Dim lArrStation() As String = Regex.Split(lstStations.SelectedItem, "\s+")
            If lArrRecSum(0).Trim().ToLower() <> lArrStation(0).Trim().ToLower() Then
                lMatched = False
            End If
        End If
        Return lMatched
    End Function

    Private Sub PopulateEquations()
        If lstRecSum.Items.Count < 3 OrElse lstStations.Items.Count < 3 Then
            Logger.Msg("No recsum or station listings.", MsgBoxStyle.Information, "Auto Populate Equation Listing")
            Exit Sub
        End If

        Dim lArrRecSum() As String
        Dim lArrStation() As String
        Dim lStationRecSum As String
        Dim lStationStation As String
        Dim lMRCToAdd As String
        Dim lEquation As String
        Dim lRange As String
        For CntRecSum As Integer = 2 To lstRecSum.Items.Count - 1
            lArrRecSum = Regex.Split(lstRecSum.Items(CntRecSum), "\s+")
            lStationRecSum = lArrRecSum(0).Trim.ToLower
            For CntStation As Integer = 2 To lstStations.Items.Count - 1
                lArrStation = Regex.Split(lstStations.Items(CntStation), "\s+")
                lStationStation = lArrStation(0).Trim.ToLower
                If lStationRecSum = lStationStation Then
                    If IsNumeric(lArrStation(1)) AndAlso _
                       IsNumeric(lArrRecSum(9)) AndAlso _
                       IsNumeric(lArrRecSum(10)) AndAlso _
                       IsNumeric(lArrRecSum(11)) AndAlso _
                       IsNumeric(lArrRecSum(7)) AndAlso _
                       IsNumeric(lArrRecSum(8)) Then

                        lMRCToAdd = lArrStation(0) & "," & lArrStation(1) & ",(" & lArrRecSum(1) & "),"
                        lRange = "(" & lArrRecSum(7) & "~" & lArrRecSum(8) & "),"
                        lMRCToAdd &= lRange
                        lEquation = "Coeff.A:" & lArrRecSum(9) & ",Coeff.B:" & lArrRecSum(10) & ",Coeff.C:" & lArrRecSum(11)
                        lMRCToAdd &= lEquation

                        If Not lstEquations.Items.Contains(lMRCToAdd) Then
                            lstEquations.Items.Add(lMRCToAdd)
                        End If
                    End If
                End If
            Next
        Next

    End Sub

    Private Sub BuildMRCs()
        Dim lStation As String
        Dim lDA As Double
        Dim lMinLogQ As Double
        Dim lMaxLogQ As Double
        Dim lCoA As Double
        Dim lCoB As Double
        Dim lCoC As Double
        Dim lSeason As String

        For Each lItem As String In lstEquations.CheckedItems

            Dim lArr() As String = lItem.Split(",")

            'get station
            lStation = lArr(0)

            'get DA
            lDA = Double.Parse(lArr(1))

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
                If .BuildMRC() Then
                    pGraphDataGroup.Add(.CurveData)
                End If
            End With
            pMRCGroup.Add(lMRC)
        Next
    End Sub

    Private Sub lstStations_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStations.SelectedIndexChanged
        Dim lSelectedIndex As Integer = lstStations.SelectedIndex
        If lSelectedIndex < 2 Then
            txtStation.Text = ""
            txtDA.Text = ""
            Exit Sub
        End If

        Dim lArr() As String = Regex.Split(lstStations.SelectedItem, "\s+")
        txtStation.Text = lArr(0)
        txtDA.Text = lArr(1)
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
            Exit Sub
        End If

        Dim lArr() As String = Regex.Split(lstRecSum.SelectedItem, "\s+")
        txtSeason.Text = lArr(1)
        txtLogQMin.Text = lArr(7)
        txtLogQMax.Text = lArr(8)
        txtCoefA.Text = lArr(9)
        txtCoefB.Text = lArr(10)
        txtCoefC.Text = lArr(11)
    End Sub

    Private Sub btnMRCAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMRCAdd.Click
        If chkAutoMatching.Checked Then
            PopulateEquations()
            Exit Sub
        End If
        Dim lMRCToAdd As String = ""
        If txtStation.Text.Trim() = "" OrElse _
           txtDA.Text.Trim() = "" OrElse _
           txtLogQMin.Text.Trim() = "" OrElse _
           txtLogQMax.Text.Trim() = "" OrElse _
           txtCoefA.Text.Trim() = "" OrElse _
           txtCoefB.Text.Trim() = "" OrElse _
           txtCoefC.Text.Trim() = "" OrElse _
           txtSeason.Text.Trim() = "" Then

            Logger.Msg("Must specify the following before adding to MRC list:" & vbCrLf & _
                       "- station" & vbCrLf & _
                       "- season" & vbCrLf & _
                       "- drainage area" & vbCrLf & _
                       "- min, max LogQ" & vbCrLf & _
                       "- polynomial coefficients A, B, and C", MsgBoxStyle.Information, "Add MRC")
            Exit Sub
        ElseIf Not (IsNumeric(txtDA.Text.Trim()) AndAlso _
                    IsNumeric(txtCoefA.Text.Trim()) AndAlso _
                    IsNumeric(txtCoefB.Text.Trim()) AndAlso _
                    IsNumeric(txtCoefC.Text.Trim()) AndAlso _
                    IsNumeric(txtLogQMin.Text.Trim()) AndAlso _
                    IsNumeric(txtLogQMax.Text.Trim())) Then
            Logger.Msg("DA, LogQ, and coefficients are not all numeric values.", MsgBoxStyle.Information, "Add MRC")
            Exit Sub
        End If

        lMRCToAdd = txtStation.Text.Trim() & "," & txtDA.Text.Trim() & ",(" & txtSeason.Text.Trim() & "),"
        Dim lRange As String = "(" & txtLogQMin.Text.Trim() & "~" & txtLogQMax.Text.Trim() & "),"
        lMRCToAdd &= lRange
        Dim lEquation As String = "Coeff.A:" & txtCoefA.Text.Trim() & ",Coeff.B:" & txtCoefB.Text.Trim() & ",Coeff.C:" & txtCoefC.Text.Trim()
        lMRCToAdd &= lEquation

        If RecSumStationMatched() AndAlso Not lstEquations.Items.Contains(lMRCToAdd) Then
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
        BuildMRCs()
        RefreshGraphMRC(pGraphDataGroup)
        tabMRCMain.SelectedIndex = 1
    End Sub

    Private Sub btnRecSum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecSum.Click
        Dim lNewFileRecSumFullName As String = ""
        Dim lInitialPath As String = Path.GetDirectoryName(pFileRecSumFullName)

        Dim lOpenFileDialog As New System.Windows.Forms.OpenFileDialog()
        With lOpenFileDialog
            .Title = "Browse For RecSum File"
            .InitialDirectory = lInitialPath '"c:\"
            .Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 2
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

    Private Sub btnStations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStations.Click
        Dim lNewFileStationFullName As String = ""
        Dim lInitialPath As String = Path.GetDirectoryName(pFileStationFullName)
        Dim lOpenFileDialog As New System.Windows.Forms.OpenFileDialog()
        With lOpenFileDialog
            .Title = "Browse For Station File"
            .InitialDirectory = lInitialPath '"c:\"
            .Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            .FilterIndex = 2
            .RestoreDirectory = True
            If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                lNewFileStationFullName = .FileName
            End If
            .Dispose()
        End With
        If File.Exists(lNewFileStationFullName) Then
            pFileStationFullName = lNewFileStationFullName
            PopulateStations()
        End If
    End Sub
End Class