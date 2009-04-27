Friend Class frmMercuryInput
    Inherits System.Windows.Forms.Form

    Private Sub chkConstant_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkConstant.CheckStateChanged
        If chkConstant.Checked Then
            chkGrid.Checked = False
            chkTime.Checked = False
            optionHgWetPrcpConc.Enabled = True
            optionHgWetConst.Enabled = True
            HgDryConstant.Enabled = True
            cboHg_Dry_Deposition_Flux.Enabled = False
            btnHg_Dry_Deposition_Flux.Enabled = False
            HgDryMultiplier.Enabled = False
            cboHg_Wet_Deposition_Flux.Enabled = False
            btnHg_Wet_Deposition_Flux.Enabled = False
            HgWetMultiplier.Enabled = False
            cboHg_Stations.Enabled = False
            btnHg_Stations.Enabled = False
            cboHg_Dry_Deposition_Time_Series.Enabled = False
            btnHg_Dry_Deposition_Time_Series.Enabled = False
            cboHg_Wet_Deposition_Time_Series.Enabled = False
            btnHg_Wet_Deposition_Time_Series.Enabled = False

            cboDryDepTS.Enabled = False
            cboWetDepTS.Enabled = False

            lblHgStation.Enabled = False
            lblDryHgDepTS.Enabled = False
            lblHgWetDepTS.Enabled = False

            lblHgDry.Enabled = False
            lblHgWet.Enabled = False
            lblHgDryMul.Enabled = False
            lblHgWetMul.Enabled = False

            If optionHgWetConst.Checked = True Then
                HgWetConstant.Enabled = True
                HgWetPrcpConc.Enabled = False
            ElseIf optionHgWetPrcpConc.Checked = True Then
                HgWetConstant.Enabled = False
                HgWetPrcpConc.Enabled = True
            End If
        ElseIf chkConstant.CheckState = 0 Then
            HgDryConstant.Enabled = False
            optionHgWetConst.Enabled = False
            HgWetConstant.Enabled = False
            optionHgWetPrcpConc.Enabled = False
            HgWetPrcpConc.Enabled = False
        End If
    End Sub

    Private Sub chkGrid_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkGrid.CheckStateChanged
        If chkGrid.CheckState = 1 Then
            chkConstant.CheckState = System.Windows.Forms.CheckState.Unchecked
            chkTime.CheckState = System.Windows.Forms.CheckState.Unchecked
            HgDryConstant.Enabled = False
            HgWetConstant.Enabled = False
            HgWetPrcpConc.Enabled = False
            cboHg_Dry_Deposition_Flux.Enabled = True
            btnHg_Dry_Deposition_Flux.Enabled = True
            HgDryMultiplier.Enabled = True
            cboHg_Wet_Deposition_Flux.Enabled = True
            btnHg_Wet_Deposition_Flux.Enabled = True
            HgWetMultiplier.Enabled = True
            cboHg_Stations.Enabled = False
            btnHg_Stations.Enabled = False
            cboHg_Dry_Deposition_Time_Series.Enabled = False
            btnHg_Dry_Deposition_Time_Series.Enabled = False
            cboHg_Wet_Deposition_Time_Series.Enabled = False
            btnHg_Wet_Deposition_Time_Series.Enabled = False
            cboDryDepTS.Enabled = False
            cboWetDepTS.Enabled = False
            lblHgStation.Enabled = False
            lblDryHgDepTS.Enabled = False
            lblHgWetDepTS.Enabled = False
            optionHgWetPrcpConc.Enabled = False
            optionHgWetConst.Enabled = False


            lblHgDry.Enabled = True
            lblHgWet.Enabled = True
            lblHgDryMul.Enabled = True
            lblHgWetMul.Enabled = True

        End If
    End Sub

    Private Sub chkTime_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkTime.CheckStateChanged
        If chkTime.Checked Then
            chkConstant.Checked = False
            chkGrid.Checked = False
            HgDryConstant.Enabled = False
            HgWetConstant.Enabled = False
            HgWetPrcpConc.Enabled = False
            cboHg_Dry_Deposition_Flux.Enabled = False
            btnHg_Dry_Deposition_Flux.Enabled = False
            HgDryMultiplier.Enabled = False
            cboHg_Wet_Deposition_Flux.Enabled = False
            btnHg_Wet_Deposition_Flux.Enabled = False
            HgWetMultiplier.Enabled = False
            cboHg_Stations.Enabled = True
            btnHg_Stations.Enabled = True
            cboHg_Dry_Deposition_Time_Series.Enabled = True
            btnHg_Dry_Deposition_Time_Series.Enabled = True
            cboHg_Wet_Deposition_Time_Series.Enabled = True
            btnHg_Wet_Deposition_Time_Series.Enabled = True
            cboDryDepTS.Enabled = True
            cboWetDepTS.Enabled = True
            lblHgStation.Enabled = True
            lblDryHgDepTS.Enabled = True
            lblHgWetDepTS.Enabled = True

            optionHgWetPrcpConc.Enabled = False
            optionHgWetConst.Enabled = False

            lblHgDry.Enabled = False
            lblHgWet.Enabled = False
            lblHgDryMul.Enabled = False
            lblHgWetMul.Enabled = False

        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        If Not ValidateSpatialReferenceOfInputLayers() Then
            Me.Close()
            Exit Sub
        End If

        If Not (chkConstant.Checked Or chkGrid.Checked Or chkTime.Checked) Then
            MsgBox("Choose one of the mercury deposition flux options")
            Exit Sub
        End If

        If cboHg_Stations.Text <> "" AndAlso Not CheckTableFields(cboHg_Stations.Text, "STA_ID") Then Exit Sub

        If BlankCheck(Me) Then
            WritetoDict(Me)
            SaveDict()
            NeedHgThiessen = chkTime.Checked
            Close()
        End If
    End Sub


    Private Sub btn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnHg_Dry_Deposition_Flux.Click, btnHg_Dry_Deposition_Time_Series.Click, btnHg_Stations.Click, btnHg_Wet_Deposition_Flux.Click, btnHg_Wet_Deposition_Time_Series.Click, btnInitialSoilHg.Click
        Dim btn As Button = eventSender
        Dim cbo As ComboBox = Controls(btn.Name.Replace("btn", "cbo"))
        Dim caption As String = btn.Name.Substring(3, btn.Name.Length - 3).Replace("_", " ")
        Dim datatype As enumDataType
        Select Case btn.Tag
            Case "Feature" : datatype = enumDataType.Feature
            Case "Raster" : datatype = enumDataType.Raster
            Case "Table" : datatype = enumDataType.Table
        End Select
        Dim name As String = SelectLayerOrTableData(datatype, caption)
        If name <> "" Then cbo.Text = name
    End Sub

    Private Sub frmMercuryInput_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        ''    ModuleFormInteract.CreateFile
        cboDryDepTS.Items.Clear()
        cboDryDepTS.Items.Add("Daily")
        cboDryDepTS.Items.Add("Monthly")
        cboDryDepTS.SelectedIndex = 0

        cboWetDepTS.Items.Clear()
        cboWetDepTS.Items.Add("Daily")
        cboWetDepTS.Items.Add("Monthly")
        cboWetDepTS.SelectedIndex = 0

        ReadDataManagementUserInputsFromFile2(Me)

        Filer()
        LoadForm(Me)

        InitializeInputDataDictionary()
        Dim chk As String = ""
        If Not InputDataDictionary.TryGetValue("chkConstant", chk) OrElse Not CBool(chk) Then
            HgDryConstant.Enabled = False
            optionHgWetConst.Enabled = False
            HgWetConstant.Enabled = False
            optionHgWetPrcpConc.Enabled = False
            HgWetPrcpConc.Enabled = False
        End If

    End Sub

    Private Sub optionHgWetConst_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionHgWetConst.CheckedChanged
        If eventSender.Checked Then
            HgWetConstant.Enabled = True
            HgWetPrcpConc.Enabled = False
        End If
    End Sub

    Private Sub optionHgWetPrcpConc_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionHgWetPrcpConc.CheckedChanged
        If eventSender.Checked Then
            HgWetConstant.Enabled = False
            HgWetPrcpConc.Enabled = True
        End If
    End Sub

    Private Sub optionInitialHgConstant_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionInitialHgConstant.CheckedChanged
        If eventSender.Checked Then
            InitialConstantHg.Enabled = True
            cboInitialSoilHg.Enabled = False
            btnInitialSoilHg.Enabled = False
            Label1.Enabled = False
            InitialSoilHgMultiplier.Enabled = False
        End If
    End Sub

    Private Sub optionSoilHgGrid_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optionSoilHgGrid.CheckedChanged
        If eventSender.Checked Then
            InitialConstantHg.Enabled = False
            cboInitialSoilHg.Enabled = True
            btnInitialSoilHg.Enabled = True
            Label1.Enabled = True
            InitialSoilHgMultiplier.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Because items may be long, set tooltip whenever selected item changes
    ''' </summary>
    Private Sub cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDryDepTS.SelectedIndexChanged, cboHg_Dry_Deposition_Flux.SelectedIndexChanged, cboHg_Dry_Deposition_Time_Series.SelectedIndexChanged, cboHg_Stations.SelectedIndexChanged, cboHg_Wet_Deposition_Flux.SelectedIndexChanged, cboHg_Wet_Deposition_Time_Series.SelectedIndexChanged, cboInitialSoilHg.SelectedIndexChanged, cboWetDepTS.SelectedIndexChanged, cboWetDepTS.SelectedIndexChanged
        ToolTip1.SetToolTip(sender, CType(sender, ComboBox).Text)
    End Sub
End Class