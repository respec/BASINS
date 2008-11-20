Imports atcUtility
Imports MapWinUtility

Public Class frmPlot

    Private pResults As atcControls.atcGridSource

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
    End Sub

    Public Sub LoadSetting()

        Dim lxaxis As Integer
        Dim lyaxis As Integer
        Dim lzaxis As Integer
        Dim lptlabels As Integer
        Dim lselectfield As Integer
        Dim ltitle As String

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

    End Sub

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

        If cboZAxis.Text = "None" Or cboZAxis.Text = "" Then ' 2d plot
            lstr.AppendLine("plot '" & lPlotDatFilename & "' u 1:2 with points t """ & cboYAxis.SelectedItem.ToString.Split("-")(1) & """")
            lstr.AppendLine("pause -1;")
        Else ' 3d plot
            lColZ = cboZAxis.SelectedItem.ToString.Split("-")(0)

            lstr.AppendLine("set grid;")
            'lstr.AppendLine("set dgrid3d 30,30;")
            'lstr.AppendLine("set hidden3d")

            Dim lblCol As Integer = 4
            If cboSelect.Text = "None" OrElse cboSelect.Text = "" Then
                lblCol = 4
            Else
                If rdoEmission.Checked Then
                    lblCol = 4
                ElseIf rdoModels.Checked Then
                    lblCol = 5
                ElseIf rdoModify.Checked Then
                    lblCol = 6
                Else
                    lblCol = 7
                End If
            End If
            If Not cboPointLabels.Text = "None" And Not cboPointLabels.Text = "" Then 'use labels
                lstr.AppendLine("set title ""Data points are labeled with " & cboPointLabels.SelectedItem.ToString.Split("-")(1) & """")
                lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
                lColB = cboPointLabels.SelectedItem.ToString.Split("-")(0)
            ElseIf Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
                'lstr.AppendLine("set title ""Data points are labeled with " & cboPointLabels.SelectedItem.ToString.Split("-")(1) & """")
                lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """, \" & vbCrLf & "'' u 1:2:3:" & lblCol & " with labels center offset 1.5,0.2 notitle; ")
                'lColB = cboPointLabels.SelectedItem.ToString.Split("-")(0)
            Else
                lstr.AppendLine("splot '" & lPlotDatFilename & "' u 1:2:3 with points t """ & cboZAxis.SelectedItem.ToString.Split("-")(1) & """")
            End If
            lstr.AppendLine("pause -1;")

        End If

        IO.File.WriteAllText(lPlotFilename, lstr.ToString)

        'Do parsing data first here
        If cboSelect.Text = "None" Or cboSelect.Text = "" Then
            parseCATData(lPlotDatFilename, "All")
        Else
            parseCATData(lPlotDatFilename, "Select")
        End If

        Process.Start(lGnuplotExe, """" & lPlotFilename & """") '"C:\mono_luChange\output\seddiff.plt")
    End Sub

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
            'Now get either 2d or 3d data to a temp dat file
            For lRow As Integer = pResults.FixedRows To pResults.Rows - 1
                For Each sem As String In lstboEmission.SelectedItems
                    If pResults.CellValue(lRow, lfilterCol).StartsWith(sem) Then
                        For Each smodel As String In lstboModels.SelectedItems
                            If pResults.CellValue(lRow, lfilterCol).Contains(smodel) Then
                                For Each smodify As String In lstboModifications.SelectedItems
                                    If pResults.CellValue(lRow, lfilterCol).Contains(smodify) Then

                                        If cboZAxis.Text = "None" OrElse cboZAxis.Text = "" Then ' 2d data
                                            lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & sem & " " & smodel & " " & smodify
                                        Else
                                            lline = pResults.CellValue(lRow, lColX - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColY - 1).Replace(",", "") & " " & pResults.CellValue(lRow, lColZ - 1).Replace(",", "") & " " & sem & " " & " " & smodel & " " & smodify
                                        End If

                                        If lptLabelCol > 0 Then
                                            lline &= " " & pResults.CellValue(lRow, lptLabelCol) & vbCrLf
                                        Else
                                            lline &= vbCrLf
                                        End If
                                        IO.File.AppendAllText(aFilename, lline)

                                        Exit For

                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            Next
        Catch ex As Exception
            Logger.Msg("Extrating CAT results Error: " & ex.Message)
            Return -99
        End Try
        Return 0
    End Function

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
        If Not cboSelect.Text = "None" And Not cboSelect.Text = "" Then
            pUpdatePlotSelection()
        Else
            'Clear the four filter listboxes?
            lstboEmission.Items.Clear()
            lstboModels.Items.Clear()
            lstboModifications.Items.Clear()
            lstboLanduse.Items.Clear()
        End If
    End Sub

    Private Sub pUpdatePlotSelection()

        'This routine populate the Landuse, weather models, and modifications data filter listboxes
        'based on the 'Selection Field' choice.
        'It is assumed that there is a field in the CAT result grid that has the scenario name that chains the 
        'component scenario factors into a string with underscore
        '
        Dim lfilterCol As Integer = cboSelect.Text.Split("-")(0) - 1

        Dim lu As String = ""
        Dim lemission As String = ""
        Dim lwmodels As String = ""
        Dim lmodify As String = ""


        Dim lEmissionCollection As atcCollection = New atcCollection
        Dim lwModelCollection As atcCollection = New atcCollection
        Dim lModificationCollection As atcCollection = New atcCollection


        'TODO: do this more dynamically
        lstboLanduse.Items.Clear()
        lstboLanduse.Items.Add("2030lua2")
        lstboLanduse.Items.Add("2030lub2")
        lstboLanduse.Items.Add("2090lua2")
        lstboLanduse.Items.Add("2090lub2")
        lstboLanduse.SelectedIndex = 0
        lstboLanduse.Enabled = False

        For lRow As Integer = pResults.FixedRows To pResults.Rows - 1

            'Here do the land use box dynamically
            'If Not lu = pResults.CellValue(lRow, lfilterCol).Split("_")(0) Then
            '    lstboLanduse.Items.Add(pResults.CellValue(lRow, lfilterCol).Split("_")(0))
            '    lu = pResults.CellValue(lRow, lfilterCol).Split("_")(0)
            'End If

            lemission = pResults.CellValue(lRow, lfilterCol).Split("_")(0)
            If pResults.CellValue(lRow, lfilterCol).Split("_").Length > 2 Then
                lwmodels = pResults.CellValue(lRow, lfilterCol).Split("_")(2)
                lmodify = pResults.CellValue(lRow, lfilterCol).Split("_")(3)
            Else
                lwmodels = pResults.CellValue(lRow, lfilterCol).Split("_")(0)
                lmodify = pResults.CellValue(lRow, lfilterCol).Split("_")(0)
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

        For i As Integer = 0 To lstboModels.Items.Count - 1
            lstboModels.SetSelected(i, True)
        Next

    End Sub

    Private Sub cboPointLabels_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPointLabels.SelectedIndexChanged

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
End Class