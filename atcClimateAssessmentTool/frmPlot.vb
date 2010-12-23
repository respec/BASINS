Imports atcUtility
Imports MapWinUtility

Public Class frmPlot

    Private pResults As atcControls.atcGridSource
    Private pDatablocks As Integer = 0
    Private pcatBlocks As atcCollection = New atcCollection
    Private pDataValueLbl As Integer = 0
    Private pOneDataBlock As Boolean = False

    'Private pTPLabel As Windows.Forms.VisualStyles.VisualStyleElement.ToolTip = New System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip(Me.components)

    Public Property Results() As atcControls.atcGridSource
        Get
            Return pResults
        End Get
        Set(ByVal newValue As atcControls.atcGridSource)
            pResults = newValue
            PopulateCombo(Me.cboXAxis)
            PopulateCombo(Me.cboYAxis)
            PopulateCombo(Me.cboZAxis)
            PopulateCombo(Me.cboPointLabels)
            PopulateCombo(Me.cboSelect)
        End Set
    End Property

    Private Sub PopulateCombo(ByVal aCombo As Windows.Forms.ComboBox)
        aCombo.Items.Clear()
        aCombo.Items.Add("None")
        For lColumn As Integer = 0 To pResults.Columns - 1
            Dim lColTitle As String = (lColumn + 1).ToString & "-"
            For lRow As Integer = 0 To pResults.FixedRows - 1
                lColTitle &= pResults.CellValue(lRow, lColumn) & " "
            Next
            aCombo.Items.Add(lColTitle)
        Next
        aCombo.SelectedIndex = 0
    End Sub

    Public Sub LoadSetting()

        Dim lxaxis As Integer
        Dim lyaxis As Integer
        Dim lzaxis As Integer
        Dim lptlabels As Integer
        Dim lselectfield As Integer
        Dim ltitle As String
        Dim ldatablock As Integer

        'For Debugging use only
        'SaveSetting("BasinsCATPlot", "Settings", "XAxis", "0")
        'SaveSetting("BasinsCATPlot", "Settings", "YAxis", "0")
        'SaveSetting("BasinsCATPlot", "Settings", "ZAxis", "0")
        'SaveSetting("BasinsCATPlot", "Settings", "PointLabels", "0")
        'SaveSetting("BasinsCATPlot", "Settings", "SelectField", "0")
        'SaveSetting("BasinsCATPlot", "Settings", "Title", "")

        lxaxis = GetSetting("BasinsCATPlot", "Settings", "XAxis", "0")
        lyaxis = GetSetting("BasinsCATPlot", "Settings", "YAxis", "0")
        lzaxis = GetSetting("BasinsCATPlot", "Settings", "ZAxis", "0")
        lptlabels = GetSetting("BasinsCATPlot", "Settings", "PointLabels", "0")
        lselectfield = GetSetting("BasinsCATPlot", "Settings", "SelectField", "0")
        ltitle = GetSetting("BasinsCATPlot", "Settings", "Title", "")
        ldatablock = GetSetting("BasinsCATPlot", "Settings", "DataBlock", "0")

        Dim ld As Integer = -99
        For i As Integer = 0 To cboXAxis.Items.Count - 1
            If Integer.TryParse(cboXAxis.Items(i).ToString.Split("-")(0), ld) Then
                If lxaxis = ld Then
                    cboXAxis.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        ld = -99
        For i As Integer = 0 To cboYAxis.Items.Count - 1
            If Integer.TryParse(cboYAxis.Items(i).ToString.Split("-")(0), ld) Then
                If lyaxis = ld Then
                    cboYAxis.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        ld = -99
        For i As Integer = 0 To cboZAxis.Items.Count - 1
            If Integer.TryParse(cboZAxis.Items(i).ToString.Split("-")(0), ld) Then
                If lzaxis = ld Then
                    cboZAxis.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        ld = -99
        For i As Integer = 0 To cboPointLabels.Items.Count - 1
            If Integer.TryParse(cboPointLabels.Items(i).ToString.Split("-")(0), ld) Then
                If lptlabels = ld Then
                    cboPointLabels.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        ld = -99
        For i As Integer = 0 To cboSelect.Items.Count - 1
            If Integer.TryParse(cboSelect.Items(i).ToString.Split("-")(0), ld) Then
                If lselectfield = ld Then
                    cboSelect.SelectedIndex = i
                    Exit For
                End If
            End If
        Next

        'cboDatablock.SelectedIndex = ldatablock
        If GetSetting("BasinsCATPlot", "Settings", "DBBMP", "No") = "True" Then
            chkboDBBMP.Checked = True
        Else
            chkboDBBMP.Checked = False
        End If

        If GetSetting("BasinsCATPlot", "Settings", "DBLanduse", "No") = "True" Then
            chkboDBLanduse.Checked = True
        Else
            chkboDBLanduse.Checked = False
        End If

        If GetSetting("BasinsCATPlot", "Settings", "DBEmission", "No") = "True" Then
            chkboDBEmission.Checked = True
        Else
            chkboDBEmission.Checked = False
        End If
        If GetSetting("BasinsCATPlot", "Settings", "DBModel", "No") = "True" Then
            chkboDBModel.Checked = True
        Else
            chkboDBModel.Checked = False
        End If
        If GetSetting("BasinsCATPlot", "Settings", "DBModify", "No") = "True" Then
            chkboDBModify.Checked = True
        Else
            chkboDBModify.Checked = False
        End If
        If GetSetting("BasinsCATPlot", "Settings", "DBOneBlock", "No") = "True" Then
            pOneDataBlock = True
        Else
            pOneDataBlock = False
        End If
    End Sub

    'Private Sub btnPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlot.Click
    '    Dim lGnuplotExe As String = FindFile("Please locate wgnuplot.exe", "wgnuplot.exe")
    '    Dim lPlotFilename As String = IO.Path.GetTempFileName()
    '    Dim lPlotDatFilename As String = IO.Path.GetTempFileName()

    '    Dim lColX As Integer
    '    Dim lColY As Integer
    '    Dim lColZ As Integer
    '    Dim lColB As Integer = 0

    '    Dim lstr As System.Text.StringBuilder = New System.Text.StringBuilder()

    '    lstr.AppendLine("reset;clear;reset;")
    '    'Differentiate if 3d plot
    '    If cboXAxis.SelectedItem Is Nothing OrElse cboYAxis.SelectedItem Is Nothing Then
    '        Logger.Msg("Please specify x- and y- axis category")
    '        Return
    '    End If

    '    If cboXAxis.SelectedItem.ToString = "None" Or cboXAxis.SelectedItem.ToString = "" Or _
    '       cboYAxis.SelectedItem.ToString = "None" Or cboYAxis.SelectedItem.ToString = "" Then
    '        Logger.Msg("Please specify x- and y- axis category")
    '        Return
    '    End If

    '    lstr.AppendLine("set title """ & txtTitle.Text & """;")
    '    lstr.AppendLine("set xlabel """ & cboXAxis.SelectedItem.ToString.Split("-")(1) & """;")
    '    lstr.AppendLine("set ylabel """ & cboYAxis.SelectedItem.ToString.Split("-")(1) & """;")

    '    lColX = cboXAxis.SelectedItem.ToString.Split("-")(0)
    '    lColY = cboYAxis.SelectedItem.ToString.Split("-")(0)

    '    If cboZAxis.Text = "None" Or cboZAxis.Text = "" Then ' 2d plot
    '        If cboDatablock.Text = "None" Then
    '            lstr.AppendLine("plot '" & lPlotDatFilename & "' u 1:2 with points t """ & cboYAxis.SelectedItem.ToString.Split("-")(1) & """")
    '            lstr.AppendLine("pause -1;")
    '        ElseIf pDatablocks > 0 Then
    '            lstr.Append("plot ")
    '            Dim i As Integer = 0
    '            For i = 0 To pDatablocks - 2
    '                lstr.AppendLine("'" & lPlotDatFilename & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """, \")
    '            Next
    '            lstr.AppendLine("'" & lPlotDatFilename & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """")
    '            lstr.AppendLine("pause -1;")
    '        End If
    '    Else ' 3d plot
    '        lColZ = cboZAxis.SelectedItem.ToString.Split("-")(0)

    '        lstr.AppendLine("set grid;")
    '        'lstr.AppendLine("set dgrid3d 30,30;")
    '        'lstr.AppendLine("set hidden3d")

    '        Dim lblCol As Integer = 4
    '        If cboSelect.Text = "None" OrElse cboSelect.Text = "" Then
    '            lblCol = 4
    '        Else
    '            If rdoBMP.Checked Then
    '                lblCol = 4
    '            ElseIf rdoLanduse.Checked Then
    '                lblCol = 5
    '            ElseIf rdoEmission.Checked Then
    '                lblCol = 6
    '            ElseIf rdoModels.Checked Then
    '                lblCol = 7
    '            ElseIf rdoModify.Checked Then
    '                lblCol = 8
    '            Else
    '                lblCol = 8
    '            End If
    '        End If
    '        If Not cboPointLabels.Text = "None" And Not cboPointLabels.Text = "" Then 'use labels
    '            lstr.AppendLine("set title ""Data points are labeled with " & cboPointLabels.SelectedItem.ToString.Split("-")(1) & """")
    '            lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
    '            lColB = cboPointLabels.SelectedItem.ToString.Split("-")(0)
    '        ElseIf Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
    '            'lstr.AppendLine("set title ""Data points are labeled with " & cboPointLabels.SelectedItem.ToString.Split("-")(1) & """")
    '            lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
    '            'lColB = cboPointLabels.SelectedItem.ToString.Split("-")(0)
    '        Else
    '            lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """")
    '        End If
    '        lstr.AppendLine("pause -1;")

    '    End If

    '    IO.File.WriteAllText(lPlotFilename, lstr.ToString)

    '    'Do parsing data first here
    '    Dim ldataReady As Integer = 0
    '    If cboSelect.Text = "None" Or cboSelect.Text = "" Then
    '        ldataReady = parseCATDataBlock(lPlotDatFilename, "All")
    '    Else
    '        ldataReady = parseCATDataBlock(lPlotDatFilename, "Select")
    '    End If

    '    If ldataReady = 0 Then
    '        Process.Start(lGnuplotExe, """" & lPlotFilename & """") '"C:\mono_luChange\output\seddiff.plt")
    '    Else
    '        Logger.Msg("Data parsing doesnot succeed, plotting not done")
    '    End If
    'End Sub

    Private Sub btnPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlot.Click
        Dim lGnuplotExe As String = FindFile("Please locate wgnuplot.exe", "wgnuplot.exe")
        Dim lPlotFilename As String = IO.Path.GetTempFileName()
        Dim lPlotDatFilename As String = IO.Path.GetTempFileName()

        Dim lColX As Integer
        Dim lColY As Integer
        Dim lColZ As Integer
        Dim lColB As Integer = 0

        Dim lstr As System.Text.StringBuilder = New System.Text.StringBuilder()

        lstr.AppendLine("reset;clear;reset;")
        'Differentiate if 3d plot
        If cboXAxis.SelectedItem Is Nothing OrElse cboYAxis.SelectedItem Is Nothing Then
            Logger.Msg("Please specify x- and y- axis category")
            Return
        End If

        If cboXAxis.SelectedItem.ToString = "None" Or cboXAxis.SelectedItem.ToString = "" Or _
           cboYAxis.SelectedItem.ToString = "None" Or cboYAxis.SelectedItem.ToString = "" Then
            Logger.Msg("Please specify x- and y- axis category")
            Return
        End If

        lstr.AppendLine("set title """ & txtTitle.Text & """;")
        lstr.AppendLine("set xlabel """ & cboXAxis.SelectedItem.ToString.Split("-")(1) & """;")
        lstr.AppendLine("set ylabel """ & cboYAxis.SelectedItem.ToString.Split("-")(1) & """;")

        lColX = cboXAxis.SelectedItem.ToString.Split("-")(0)
        lColY = cboYAxis.SelectedItem.ToString.Split("-")(0)

        'Do parsing data first here
        Dim ldataReady As Integer = 0
        If cboSelect.Text = "None" Or cboSelect.Text = "" Then
            ldataReady = parseCATDataBlock(lPlotDatFilename, "All")
        Else
            ldataReady = parseCATDataBlock(lPlotDatFilename, "Select")
        End If

        Dim lblCol As Integer = 4
        If (cboPointLabels.Text = "None" OrElse cboPointLabels.Text = "") And Not labelCATEGORY() Then
            lblCol = -99 ' Don't label the data points at all
        Else
            If cboSelect.Text = "None" OrElse cboSelect.Text = "" Then
                lblCol = 4
            Else
                If rdoBMP.Checked Then
                    lblCol = 4
                ElseIf rdoLanduse.Checked Then
                    lblCol = 5
                ElseIf rdoEmission.Checked Then
                    lblCol = 6
                ElseIf rdoModels.Checked Then
                    lblCol = 7
                ElseIf rdoModify.Checked Then
                    lblCol = 8
                Else
                    lblCol = 8
                End If
            End If
        End If

        If cboZAxis.Text = "None" Or cboZAxis.Text = "" Then ' 2d plot
            If pcatBlocks.Keys.Count > 1 Then
                'lstr.Append("plot ")
                'Dim i As Integer = 0
                'For i = 0 To pDatablocks - 2
                '    lstr.AppendLine("'" & lPlotDatFilename & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """, \")
                'Next
                'lstr.AppendLine("'" & lPlotDatFilename & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """")
                doGnuplotScript("2d", lstr, lPlotDatFilename, "", lblCol)
                lstr.AppendLine("pause -1;")
            ElseIf pcatBlocks.Keys.Count = 1 Then
                doGnuplotScript("2d", lstr, lPlotDatFilename, cboYAxis.SelectedItem.ToString.Split("-")(1), lblCol)
                'lstr.AppendLine("plot '" & lPlotDatFilename & "' u 1:2 with points t """ & cboYAxis.SelectedItem.ToString.Split("-")(1) & """")
                lstr.AppendLine("pause -1;")
            End If
        Else ' 3d plot
            lColZ = cboZAxis.SelectedItem.ToString.Split("-")(0)

            lstr.AppendLine("set grid;")
            lstr.AppendLine("set grid z;")
            'lstr.AppendLine("set dgrid3d 30,30;")
            'lstr.AppendLine("set hidden3d")

            If Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
                doGnuplotScript("3d", lstr, lPlotDatFilename, cboZAxis.SelectedItem.ToString.Split("-")(1), lblCol)
                'lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
            ElseIf Not cboPointLabels.Text = "None" And Not cboPointLabels.Text = "" Then 'use labels
                lstr.AppendLine("set title ""Data points are labeled with " & cboPointLabels.SelectedItem.ToString.Split("-")(1) & """")
                doGnuplotScript("3d", lstr, lPlotDatFilename, cboZAxis.SelectedItem.ToString.Split("-")(1), lblCol)
                'lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
                'lColB = cboPointLabels.SelectedItem.ToString.Split("-")(0)
            Else
                lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """")
            End If
            lstr.AppendLine("pause -1;")
        End If

        IO.File.WriteAllText(lPlotFilename, lstr.ToString)


        If ldataReady = 0 Then
            Process.Start(lGnuplotExe, """" & lPlotFilename & """") '"C:\mono_luChange\output\seddiff.plt")
        Else
            Logger.Msg("Data parsing doesnot succeed, plotting not done")
        End If
    End Sub

    Private Function doGnuplotScript(ByVal adim As String, ByRef aSB As System.Text.StringBuilder, ByVal afname As String, Optional ByVal aTitle As String = "", Optional ByVal albl As Integer = 0) As Integer
        'This function create the gnuplot script, with knowledge of the data block information via pcatBlocks and pDatablock
        Dim i As Integer

        If adim = "2d" Then
            aSB.Append("plot ")
            If pcatBlocks.Keys.Count > 1 Then
                For i = 0 To pcatBlocks.Keys.Count - 2
                    If albl > 0 Then
                        aSB.AppendLine("'" & afname & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """, \" & vbCrLf & "'' u 1:2:" & albl & " every :::" & i & "::" & i & " with labels center offset 1.5,0.2 notitle, \")
                    Else
                        aSB.AppendLine("'" & afname & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """, \")
                    End If
                Next
                If albl > 0 Then
                    aSB.AppendLine("'" & afname & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """, \" & vbCrLf & "'' u 1:2:" & albl & " every :::" & i & "::" & i & " with labels center offset 1.5,0.2 notitle;")
                Else
                    aSB.AppendLine("'" & afname & "' u 1:2 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString() & """;")
                End If
            Else
                If albl > 0 Then
                    aSB.AppendLine("'" & afname & "' u 1:2 with points t """ & aTitle & """, \" & vbCrLf & "'' u 1:2:" & albl & " with labels center offset 1.5,0.2 notitle;")
                Else
                    aSB.AppendLine("'" & afname & "' u 1:2 with points t """ & aTitle & """;")
                End If
            End If
        ElseIf adim = "3d" Then

            If chkboContour.Checked Then
                'Do a 3d contour map using x,y, and z and return
                aSB.AppendLine("set contour base;")
                aSB.AppendLine("set dgrid3d;")
                aSB.AppendLine("set autoscale fix;")
                aSB.AppendLine("set palette rgbformulae 22,13,-31")
                aSB.AppendLine("set isosample 30,30;")
                aSB.AppendLine("set clabel ""%16.0f"";")
                'aSB.AppendLine("set view map;")
                aSB.AppendLine("set view 180,0,1")
                For i = 0 To pcatBlocks.Keys.Count - 1
                    aSB.AppendLine("splot '" & afname & "' u 1:2:3 every :::" & i & "::" & i & " with pm3d t ""Color contour map for data block: " & pcatBlocks.Keys()(i).ToString & """;")
                    aSB.AppendLine("pause -1;")
                Next
                Return 0
            End If

            aSB.Append("splot ")
            If pcatBlocks.Keys.Count > 1 Then
                For i = 0 To pcatBlocks.Keys.Count - 2
                    If albl > 0 Then
                        aSB.Append("'" & afname & "'  u 1:2:3 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString & """, \" & vbCrLf & "'' u 1:2:3:" & albl & " every :::" & i & "::" & i & " with labels center offset 1.5,0.2 notitle, \" & vbCrLf)
                    Else
                        aSB.Append("'" & afname & "'  u 1:2:3 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString & """, \" & vbCrLf)
                    End If
                Next
                'for the last line in the splot command
                If albl > 0 Then
                    aSB.Append("'" & afname & "'  u 1:2:3 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString & """, \" & vbCrLf & "'' u 1:2:3:" & albl & " every :::" & i & "::" & i & " with labels center offset 1.5,0.2 notitle;" & vbCrLf)
                Else
                    aSB.Append("'" & afname & "'  u 1:2:3 every :::" & i & "::" & i & " with points t """ & pcatBlocks.Keys()(i).ToString & """;" & vbCrLf)
                End If
            Else ' only one data block
                If albl > 0 Then
                    aSB.Append("'" & afname & "'  u 1:2:3 with points t """ & aTitle & """, \" & vbCrLf & "'' u 1:2:3:" & albl & " with labels center offset 1.5,0.2 notitle;" & vbCrLf)
                Else
                    aSB.Append("'" & afname & "'  u 1:2:3 with points t """ & aTitle & """;" & vbCrLf)
                End If
            End If

        End If
        Return 0
    End Function

    Private Function labelCATEGORY() As Boolean
        If rdoBMP.Checked OrElse rdoLanduse.Checked OrElse rdoEmission.Checked OrElse rdoModels.Checked OrElse rdoModify.Checked Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function parseCATData(ByVal aFilename As String, ByVal aSelection As String) As Integer
        Dim lline As String = ""
        Dim lColX As Integer = cboXAxis.SelectedItem.ToString.Split("-")(0)
        Dim lColY As Integer = cboYAxis.SelectedItem.ToString.Split("-")(0)
        Dim lColZ As Integer = -99
        If Not cboZAxis.Text = "None" And Not cboZAxis.Text = "" Then
            lColZ = cboZAxis.SelectedItem.ToString.Split("-")(0) - 1
        End If

        Dim lfilterCol As Integer = -99
        If Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
            lfilterCol = cboSelect.Text.Split("-")(0) - 1
        End If

        Dim lptLabelCol As Integer = -99

        If Not cboPointLabels.Text = "None" And Not cboPointLabels.Text = "" Then
            lptLabelCol = cboPointLabels.SelectedItem.ToString.Split("-")(0) - 1
        End If

        If aSelection = "All" Then
            Try
                For lRow As Integer = pResults.FixedRows To pResults.Rows - 1

                    If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then ' 2d data
                        lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "")
                    Else
                        lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "")
                    End If

                    If lptLabelCol > 0 Then
                        lline &= " " & pResults.CellValue(lRow, lptLabelCol) & vbCrLf
                    Else
                        lline &= vbCrLf
                    End If
                    IO.File.AppendAllText(aFilename, lline)

                Next
            Catch ex As Exception
                Logger.Msg("Extracting CAT data error: " & ex.Message)
                Return -99
            End Try
            Return 0
        End If

        Try

            'give warning about selection if no selection is made to each of the five filters
            If lstboBMP.SelectedItems.Count = 0 OrElse _
               lstboEmission.SelectedItems.Count = 0 OrElse _
               lstboModifications.SelectedItems.Count = 0 OrElse _
               lstboLanduse.SelectedItems.Count = 0 OrElse _
               lstboModels.SelectedItems.Count = 0 Then
                If cboSelect.Text.Contains("Run") Then
                    Logger.Msg("Need to select at least one entry from each of the five filters.")
                    Return -99
                Else
                    Return 0
                End If
            End If

            'Now get either 2d or 3d data to a temp dat file
            Dim lcv As String = Nothing
            For lRow As Integer = pResults.FixedRows To pResults.Rows - 1

                lcv = pResults.CellValue(lRow, lfilterCol)
                For Each bmp As String In lstboBMP.SelectedItems
                    If lcv.EndsWith(bmp) Then

                        For Each lu As String In lstboLanduse.SelectedItems
                            If lcv.Contains(lu) Then

                                For Each sem As String In lstboEmission.SelectedItems
                                    If lcv.StartsWith(sem) Then

                                        For Each smodel As String In lstboModels.SelectedItems
                                            If lcv.Contains(smodel) Then

                                                For Each smodify As String In lstboModifications.SelectedItems
                                                    If lcv.Contains(smodify) Then

                                                        If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then ' 2d data
                                                            lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & bmp & " " & lu & " " & sem & " " & smodel & " " & smodify
                                                        Else
                                                            lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "") & " " & bmp & " " & lu & " " & sem & " " & " " & smodel & " " & smodify
                                                        End If

                                                        If lptLabelCol > 0 Then
                                                            lline &= " " & pResults.CellValue(lRow, lptLabelCol) & vbCrLf
                                                        Else
                                                            lline &= vbCrLf
                                                        End If
                                                        IO.File.AppendAllText(aFilename, lline)

                                                        Exit For

                                                    End If ' match modify
                                                Next ' foreach modify {F10, F30, M}

                                            End If ' match model
                                        Next ' foreach model {cccm, ..., ncar}

                                    End If ' match emission
                                Next ' foreach emission {a, b, base}

                            End If ' match landuse
                        Next ' foreach landuse {lu2030a2, ... mon10, mon70}

                    End If ' match bmp
                Next 'foreach bmp {y, n}
            Next ' foreach lRow

        Catch ex As Exception
            Logger.Msg("Extrating CAT results Error: " & ex.Message)
            Return -99
        End Try
        Return 0
    End Function

    Private Function parseCATDataBlock(ByVal aFilename As String, ByVal aSelection As String) As Integer
        Dim lline As String = ""
        Dim ltemp As String = ""

        'Reset the cat data block collection
        pcatBlocks.Clear()
        pDatablocks = 0

        If chkboDBBMP.Checked OrElse chkboDBLanduse.Checked OrElse chkboDBEmission.Checked OrElse _
        chkboDBModel.Checked OrElse chkboDBModify.Checked Then
            pOneDataBlock = False
        Else
            pOneDataBlock = True
        End If

        'Dim lblockIndex As Integer = 0
        'Select Case cboDatablock.Text
        '    Case "None"
        '        lblockIndex = 0
        '    Case "BMP"
        '        lblockIndex = 5 ' or 2 in the case of base_lu2030a2_No
        '    Case "Landuse"
        '        lblockIndex = 4 ' or 1 in the case of base_lu2030a2_No
        '    Case "Emission Scenarios"
        '        lblockIndex = 0 ' or 0 for base
        '    Case "Weather Models"
        '        lblockIndex = 2 ' or 0 for base
        '    Case "Modifications"
        '        lblockIndex = 3 ' or 0 for base
        '    Case Else
        '        lblockIndex = 0 ' or 0 for base
        'End Select

        Dim lColX As Integer = cboXAxis.SelectedItem.ToString.Split("-")(0)
        Dim lColY As Integer = cboYAxis.SelectedItem.ToString.Split("-")(0)
        Dim lColZ As Integer = -99
        If Not cboZAxis.Text = "None" And Not cboZAxis.Text = "" Then
            lColZ = cboZAxis.SelectedItem.ToString.Split("-")(0)
        End If

        Dim lfilterCol As Integer = -99
        If Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
            lfilterCol = cboSelect.Text.Split("-")(0)
        End If

        Dim lptLabelCol As Integer = -99

        If Not cboPointLabels.Text = "None" And Not cboPointLabels.Text = "" Then
            lptLabelCol = cboPointLabels.SelectedItem.ToString.Split("-")(0)
        End If
        Dim lKey As String = ""
        If aSelection = "All" Then
            Try
                For lRow As Integer = pResults.FixedRows To pResults.Rows - 1

                    lKey = ""
                    If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then ' 2d data
                        lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "")
                    Else
                        lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "")
                    End If

                    If lptLabelCol - 1 > 0 Then
                        lline &= " " & pResults.CellValue(lRow, lptLabelCol - 1) & vbCrLf
                    Else
                        lline &= vbCrLf
                    End If

                    If pOneDataBlock Then
                        IO.File.AppendAllText(aFilename, lline)
                    Else
                        ltemp = pResults.CellValue(lRow, 0)
                        '' adjust lblockIndex for base model name
                        'If ltemp.StartsWith("base") Then
                        '    If cboDatablock.Text = "Landuse" Then
                        '        lblockIndex = 1
                        '    ElseIf cboDatablock.Text = "BMP" Then
                        '        lblockIndex = 2
                        '    Else
                        '        lblockIndex = 0
                        '    End If
                        'End If

                        'Dim ltempKey As String = pResults.CellValue(lRow, 0).Split("_")(lblockIndex)

                        If isPartialKey("BMP") Then
                            If ltemp.StartsWith("base") Then
                                lKey &= ltemp.Split("_")(2) & "-"
                            Else
                                lKey &= ltemp.Split("_")(5) & "-"
                            End If
                        End If
                        If isPartialKey("Landuse") Then
                            If ltemp.StartsWith("base") Then
                                lKey &= ltemp.Split("_")(1) & "-"
                            Else
                                lKey &= ltemp.Split("_")(4) & "-"
                            End If
                        End If

                        If isPartialKey("Emission") Then
                            If ltemp.StartsWith("base") Then
                                lKey &= "base-"
                            Else
                                lKey &= ltemp.Split("_")(0) & "-"
                            End If
                        End If

                        If isPartialKey("Model") Then
                            If ltemp.StartsWith("base") Then
                                lKey &= "base-"
                            Else
                                lKey &= ltemp.Split("_")(2) & "-"
                            End If
                        End If

                        If isPartialKey("Modify") Then
                            If ltemp.StartsWith("base") Then
                                lKey &= "base-"
                            Else
                                lKey &= ltemp.Split("_")(3) & "-"
                            End If
                        End If

                        lKey = lKey.Trim("-")

                        If pcatBlocks.ItemByKey(lKey) = Nothing Then
                            pcatBlocks.Add(lKey, lline)
                        Else
                            ltemp = ""
                            ltemp = pcatBlocks.ItemByKey(lKey)
                            Dim lind As Integer = pcatBlocks.IndexFromKey(lKey)
                            ltemp &= lline
                            pcatBlocks.Insert(lind, lKey, ltemp)
                            pcatBlocks.RemoveAt(lind + 1)
                        End If
                    End If
                Next ' foreach lrow

                If pcatBlocks.Keys.Count > 1 Then
                    writeDataBlock(aFilename, pcatBlocks)
                End If
            Catch ex As Exception
                Logger.Msg("Extracting CAT data error: " & ex.Message)
                Return -99
            End Try
            Return 0
        End If

        Try

            'give warning about selection if no selection is made to each of the five filters
            If lstboBMP.SelectedItems.Count = 0 OrElse _
               lstboEmission.SelectedItems.Count = 0 OrElse _
               lstboModifications.SelectedItems.Count = 0 OrElse _
               lstboLanduse.SelectedItems.Count = 0 OrElse _
               lstboModels.SelectedItems.Count = 0 Then
                If cboSelect.Text.Contains("Run") Then
                    Logger.Msg("Need to select at least one entry from each of the five filters.")
                    Return -99
                Else
                    Return 0
                End If
            End If

            'Now get either 2d or 3d data to a temp dat file
            Dim lcv As String = Nothing
            Dim lfound As Boolean = False
            Dim lDataValLbl As String = ""

            For lRow As Integer = pResults.FixedRows To pResults.Rows - 1
                lKey = ""
                lfound = False
                lcv = pResults.CellValue(lRow, lfilterCol - 1)
                For Each bmp As String In lstboBMP.SelectedItems

                    If lcv.EndsWith(bmp) Then
                        If isPartialKey("BMP") Then lKey &= bmp & "-"
                        For Each lu As String In lstboLanduse.SelectedItems

                            If lcv.Contains(lu) Then
                                If isPartialKey("Landuse") Then lKey &= lu & "-"
                                For Each sem As String In lstboEmission.SelectedItems

                                    If lcv.StartsWith(sem) Then
                                        If isPartialKey("Emission") Then lKey &= sem & "-"
                                        For Each smodel As String In lstboModels.SelectedItems

                                            If lcv.Contains(smodel) Then
                                                If isPartialKey("Model") Then lKey &= smodel & "-"
                                                For Each smodify As String In lstboModifications.SelectedItems

                                                    If lcv.Contains(smodify) Then
                                                        If isPartialKey("Modify") Then lKey &= smodify & "-"
                                                        lKey = lKey.Trim("-")
                                                        If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then ' 2d data
                                                            If cboPointLabels.Text = "None" OrElse cboPointLabels.Text = "" Then
                                                                lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & bmp & " " & lu & " " & sem & " " & smodel & " " & smodify
                                                            Else
                                                                If lptLabelCol = lColY Then
                                                                    lDataValLbl = "->Z:" & pResults.CellValue(lRow, lptLabelCol - 1)
                                                                Else
                                                                    lDataValLbl = ":Label:" & pResults.CellValue(lRow, lptLabelCol - 1)
                                                                End If

                                                                lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & bmp & lDataValLbl & " " & lu & lDataValLbl & " " & sem & lDataValLbl & " " & smodel & lDataValLbl & " " & smodify & lDataValLbl
                                                            End If
                                                        Else ' 3d data
                                                            If cboPointLabels.Text = "None" OrElse cboPointLabels.Text = "" Then
                                                                lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "") & " " & bmp & " " & lu & " " & sem & " " & " " & smodel & " " & smodify
                                                            Else
                                                                If lptLabelCol = lColZ Then
                                                                    lDataValLbl = "->Z:" & pResults.CellValue(lRow, lptLabelCol - 1)
                                                                Else
                                                                    lDataValLbl = ":Label:" & pResults.CellValue(lRow, lptLabelCol - 1)
                                                                End If
                                                                lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "") & " " & bmp & lDataValLbl & " " & lu & lDataValLbl & " " & sem & lDataValLbl & " " & smodel & lDataValLbl & " " & smodify & lDataValLbl
                                                            End If
                                                        End If

                                                        If lptLabelCol - 1 > 0 Then
                                                            lline &= " " & pResults.CellValue(lRow, lptLabelCol - 1) & vbCrLf ' Don't have to worry about data value
                                                        Else
                                                            lline &= vbCrLf
                                                        End If

                                                        If pOneDataBlock Then
                                                            IO.File.AppendAllText(aFilename, lline)
                                                        Else
                                                            '' adjust lblockIndex for base model name
                                                            'Dim lblockIndex0 As Integer
                                                            'If lcv.StartsWith("base") Then
                                                            '    lblockIndex0 = lblockIndex 'save the default data block column number
                                                            '    If cboDatablock.Text = "Landuse" Then
                                                            '        lblockIndex = 1
                                                            '    ElseIf cboDatablock.Text = "BMP" Then
                                                            '        lblockIndex = 2
                                                            '    Else
                                                            '        lblockIndex = 0
                                                            '    End If
                                                            'End If

                                                            'Dim ltempKey As String = lcv.Split("_")(lblockIndex)
                                                            If pcatBlocks.ItemByKey(lKey) = Nothing Then
                                                                pcatBlocks.Add(lKey, lline)
                                                            Else
                                                                ltemp = ""
                                                                ltemp = pcatBlocks.ItemByKey(lKey)
                                                                Dim lind As Integer = pcatBlocks.IndexFromKey(lKey)
                                                                ltemp &= lline
                                                                'pcatBlocks.Insert(ltempKey, ltemp)
                                                                pcatBlocks.Insert(lind, lKey, ltemp)
                                                                pcatBlocks.RemoveAt(lind + 1)
                                                            End If
                                                            'If lcv.StartsWith("base") Then
                                                            '    lblockIndex = lblockIndex0 ' change it back to default data block number
                                                            'End If
                                                        End If

                                                        lfound = True
                                                        Exit For

                                                    End If ' match modify
                                                Next ' foreach modify {F10, F30, M}

                                            End If ' match model
                                            If lfound Then Exit For
                                        Next ' foreach model {cccm, ..., ncar}

                                    End If ' match emission
                                    If lfound Then Exit For
                                Next ' foreach emission {a, b, base}

                            End If ' match landuse
                            If lfound Then Exit For
                        Next ' foreach landuse {lu2030a2, ... mon10, mon70}

                    End If ' match bmp
                    If lfound Then Exit For
                Next 'foreach bmp {y, n}
            Next ' foreach lRow

            If pcatBlocks.Keys.Count > 1 Then
                writeDataBlock(aFilename, pcatBlocks)
            End If

        Catch ex As Exception
            Logger.Msg("Extrating CAT results Error: " & ex.Message)
            Return -99
        End Try
        Return 0
    End Function

    Private Function isPartialKey(ByVal aDF As String) As Boolean
        isPartialKey = False
        Select Case aDF
            Case "BMP"
                If chkboDBBMP.Checked Then isPartialKey = True
            Case "Landuse"
                If chkboDBLanduse.Checked Then isPartialKey = True
            Case "Emission"
                If chkboDBEmission.Checked Then isPartialKey = True
            Case "Model"
                If chkboDBModel.Checked Then isPartialKey = True
            Case "Modify"
                If chkboDBModify.Checked Then isPartialKey = True
            Case Else
                isPartialKey = False
        End Select
    End Function
    Private Sub writeDataBlock(ByVal aFilename As String, ByVal acatBlocks As atcCollection)
        'acatBlocks.Sort()
        For Each k As String In acatBlocks
            IO.File.AppendAllText(aFilename, k & vbCrLf)
        Next
        pDatablocks = acatBlocks.Keys.Count
    End Sub
    Private Sub cboXAxis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboXAxis.SelectedIndexChanged
        pUpdateTitle()
    End Sub

    Private Sub cboYAxis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYAxis.SelectedIndexChanged
        pUpdateTitle()
    End Sub

    Private Sub cboZAxis_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboZAxis.SelectedIndexChanged
        pUpdateTitle()
    End Sub

    Private Sub pUpdateTitle()
        Dim lstr As String = ""
        If cboXAxis.Text = "None" OrElse _
        cboYAxis.Text = "None" OrElse _
        cboXAxis.Text = "" OrElse _
        cboYAxis.Text = "" Then
            lstr = ""
        Else
            lstr = cboXAxis.Text.Split("-")(1) & "   X   " & cboYAxis.Text.Split("-")(1)
            If Not cboZAxis.Text = "None" AndAlso Not cboZAxis.Text = "" Then
                lstr &= " : " & cboZAxis.SelectedItem.ToString.Split("-")(1)
            End If
        End If
        txtTitle.Text = lstr
    End Sub

    Private Sub cboSelect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSelect.SelectedIndexChanged
        lstboBMP.Items.Clear()
        lstboLanduse.Items.Clear()
        lstboEmission.Items.Clear()
        lstboModels.Items.Clear()
        lstboModifications.Items.Clear()
        lstboLanduse.Items.Clear()

        If cboSelect.Text = "None" Then
            rdoBMP.Checked = False
            rdoLanduse.Checked = False
            rdoEmission.Checked = False
            rdoModels.Checked = False
            rdoModify.Checked = False
        ElseIf cboSelect.Text.Contains("Run") Then
            pUpdatePlotSelection()
        End If
    End Sub

    Private Sub pUpdatePlotSelection()

        'This routine populate the Landuse, weather models, and modifications data filter listboxes
        'based on the 'Selection Field' choice.
        'It is assumed that there is a field in the CAT result grid that has the model name that chains the 
        'component factors into a string with underscore

        If Not cboSelect.Text.Contains("Run") Then
            'Has to choose a column that has 'Run' in its title
            'else just return and do nothing
            Return
        End If

        Dim lfilterCol As Integer = cboSelect.Text.Split("-")(0) - 1

        Dim lu As String = ""
        Dim lemission As String = ""
        Dim lwmodels As String = ""
        Dim lmodify As String = ""
        Dim lbmp As String = ""


        Dim lEmissionCollection As atcCollection = New atcCollection
        Dim lwModelCollection As atcCollection = New atcCollection
        Dim lModificationCollection As atcCollection = New atcCollection

        Dim lBMPCollection As atcCollection = New atcCollection
        Dim lLUCollection As atcCollection = New atcCollection

        'TODO: do this more dynamically
        'lstboLanduse.Items.Clear()
        'lstboLanduse.Items.Add("2030lua2")
        'lstboLanduse.Items.Add("2030lub2")
        'lstboLanduse.Items.Add("2090lua2")
        'lstboLanduse.Items.Add("2090lub2")
        'lstboLanduse.SelectedIndex = 0
        'lstboLanduse.Enabled = False

        Dim lcv As String = Nothing
        For lRow As Integer = pResults.FixedRows To pResults.Rows - 1

            'Here do the land use box dynamically
            'If Not lu = pResults.CellValue(lRow, lfilterCol).Split("_")(0) Then
            '    lstboLanduse.Items.Add(pResults.CellValue(lRow, lfilterCol).Split("_")(0))
            '    lu = pResults.CellValue(lRow, lfilterCol).Split("_")(0)
            'End If

            lcv = pResults.CellValue(lRow, lfilterCol)
            lemission = lcv.Split("_")(0)
            If Not lcv.StartsWith("base") Then
                lwmodels = lcv.Split("_")(2)
                lmodify = lcv.Split("_")(3)
                lu = lcv.Split("_")(4)
                lbmp = lcv.Split("_")(5)
            Else
                lwmodels = lcv.Split("_")(0)
                lmodify = lcv.Split("_")(0)
                lu = lcv.Split("_")(1)
                lbmp = lcv.Split("_")(2)
            End If

            If Not lEmissionCollection.Contains(lemission) Then
                lEmissionCollection.Add(lemission)
            End If

            If Not lwModelCollection.Contains(lwmodels) Then
                lwModelCollection.Add(lwmodels)
            End If

            If Not lModificationCollection.Contains(lmodify) Then
                lModificationCollection.Add(lmodify)
            End If

            If Not lBMPCollection.Contains(lbmp) Then
                lBMPCollection.Add(lbmp)
            End If

            If Not lLUCollection.Contains(lu) Then
                lLUCollection.Add(lu)
            End If
        Next

        For Each s As String In lEmissionCollection
            lstboEmission.Items.Add(s)
        Next
        For Each s As String In lwModelCollection
            lstboModels.Items.Add(s)
        Next
        For Each s As String In lModificationCollection
            lstboModifications.Items.Add(s)
        Next

        For Each s As String In lBMPCollection
            lstboBMP.Items.Add(s)
        Next

        For Each s As String In lLUCollection
            lstboLanduse.Items.Add(s)
        Next

        For i As Integer = 0 To lstboModels.Items.Count - 1
            lstboModels.SetSelected(i, True)
        Next

    End Sub

    Private Sub cboPointLabels_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPointLabels.SelectedIndexChanged
        If cboPointLabels.Text = "None" OrElse cboPointLabels.Text = "" Then
            rdoBMP.Checked = False
            rdoLanduse.Checked = False
            rdoEmission.Checked = False
            rdoModels.Checked = False
            rdoModify.Checked = False
        End If
    End Sub

    Private Sub frmPlot_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub rdoLanduse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoLanduse.CheckedChanged

    End Sub

    Private Sub rdoModify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoModify.CheckedChanged

    End Sub

    Private Sub btnAllEmission_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllEmission.Click
        For i As Integer = 0 To lstboEmission.Items.Count - 1
            lstboEmission.SetSelected(i, True)
        Next
    End Sub

    Private Sub btnNoneEmission_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneEmission.Click
        For i As Integer = 0 To lstboEmission.Items.Count - 1
            lstboEmission.SetSelected(i, False)
        Next
    End Sub

    Private Sub btnAllModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllModels.Click
        For i As Integer = 0 To lstboModels.Items.Count - 1
            lstboModels.SetSelected(i, True)
        Next
    End Sub

    Private Sub btnNoneModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneModels.Click
        For i As Integer = 0 To lstboModels.Items.Count - 1
            lstboModels.SetSelected(i, False)
        Next
    End Sub

    Private Sub btnAllModifications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllModifications.Click
        For i As Integer = 0 To lstboModifications.Items.Count - 1
            lstboModifications.SetSelected(i, True)
        Next
    End Sub

    Private Sub btnNoneModifications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneModifications.Click
        For i As Integer = 0 To lstboModifications.Items.Count - 1
            lstboModifications.SetSelected(i, False)
        Next
    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click

        'TODO: Save settings here

        If cboXAxis.Text = "None" OrElse cboXAxis.Text = "" Then
            SaveSetting("BasinsCATPlot", "Settings", "XAxis", "0")
        Else
            SaveSetting("BasinsCATPlot", "Settings", "XAxis", cboXAxis.Text.Split("-")(0))
        End If

        If cboYAxis.Text = "None" OrElse cboYAxis.Text = "" Then
            SaveSetting("BasinsCATPlot", "Settings", "YAxis", "0")
        Else
            SaveSetting("BasinsCATPlot", "Settings", "YAxis", cboYAxis.Text.Split("-")(0))
        End If

        If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then
            SaveSetting("BasinsCATPlot", "Settings", "ZAxis", "0")
        Else
            SaveSetting("BasinsCATPlot", "Settings", "ZAxis", cboZAxis.Text.Split("-")(0))
        End If

        If cboPointLabels.Text = "None" OrElse cboPointLabels.Text = "" Then
            SaveSetting("BasinsCATPlot", "Settings", "PointLabels", "0")
        Else
            SaveSetting("BasinsCATPlot", "Settings", "PointLabels", cboPointLabels.Text.Split("-")(0))
        End If

        If cboSelect.Text = "None" OrElse cboSelect.Text = "" Then
            SaveSetting("BasinsCATPlot", "Settings", "SelectField", "0")
        Else
            SaveSetting("BasinsCATPlot", "Settings", "SelectField", cboSelect.Text.Split("-")(0))
        End If

        SaveSetting("BasinsCATPlot", "Settings", "DBBMP", chkboDBBMP.Checked.ToString)
        SaveSetting("BasinsCATPlot", "Settings", "DBLanduse", chkboDBLanduse.Checked.ToString)
        SaveSetting("BasinsCATPlot", "Settings", "DBEmission", chkboDBEmission.Checked.ToString)
        SaveSetting("BasinsCATPlot", "Settings", "DBModel", chkboDBModel.Checked.ToString)
        SaveSetting("BasinsCATPlot", "Settings", "DBModify", chkboDBModify.Checked.ToString)

        SaveSetting("BasinsCATPlot", "Settings", "DBOneBlock", pOneDataBlock.ToString)

        SaveSetting("BasinsCATPlot", "Settings", "Title", txtTitle.Text)
        Me.Close()

    End Sub

    Private Sub btnCancelPlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelPlot.Click
        Me.Close()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnAllLanduse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllLanduse.Click
        For i As Integer = 0 To lstboLanduse.Items.Count - 1
            lstboLanduse.SetSelected(i, True)
        Next
    End Sub

    Private Sub btnNoneLanduse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoneLanduse.Click
        For i As Integer = 0 To lstboLanduse.Items.Count - 1
            lstboLanduse.SetSelected(i, False)
        Next
    End Sub
End Class