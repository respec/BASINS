Friend Class frmHydrology
    Inherits System.Windows.Forms.Form

    Private RegionArray() As String = {"Eastern US", "San Francisco Bay Region", "Upper Green River, Wyoming", "Upper Salmon River, Idaho", "User Defined"}
    Private AlphaDepthArray() As Double = {0.33, 0.378, 0.3094, 0.1593, 0}
    Private BetaDepthArray() As Double = {0.2964, 0.2582, 0.1923, 0.2625, 0}
    Private AlphaWidthArray() As Double = {2.933, 3.513, 1.579, 1.4968, 0}
    Private BetaWidthArray() As Double = {0.3916, 0.3692, 0.3866, 0.4562, 0}


    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        If Not ValidateSpatialReferenceOfInputLayers() Then
            Me.Close()
            Exit Sub
        End If

        If BlankCheck(Me) Then
            WritetoDict(Me)
            SaveDict()
            Close()
        End If
    End Sub

    Private Sub findInitialSoilMoistureGrid_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnInitial_Soil_Moisture.Click
        cboInitial_Soil_Moisture.Items.Add(SelectLayerOrTableData("Raster"))
        cboInitial_Soil_Moisture.SelectedIndex = cboInitial_Soil_Moisture.Items.Count - 1
    End Sub

    Private Sub frmHydrologyHydraulic_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        If optConstSoilMoisture.Checked = True Then
            InitialSoilMoistureConstant.Enabled = True
            cboInitial_Soil_Moisture.Enabled = False
            btnInitial_Soil_Moisture.Enabled = False
        ElseIf optFieldCapacity.Checked = True Then
            InitialSoilMoistureConstant.Enabled = False
            cboInitial_Soil_Moisture.Enabled = False
            btnInitial_Soil_Moisture.Enabled = False
        Else
            InitialSoilMoistureConstant.Enabled = False
            If optInputSoilMoisture.Checked = True Then
                cboInitial_Soil_Moisture.Enabled = True
                btnInitial_Soil_Moisture.Enabled = True
            End If
        End If
        listRegions.Items.AddRange(RegionArray)
        listRegions.SelectedIndex = 0
        ReadDataManagementUserInputsFromFile2(Me)
        Filer()
        LoadForm(Me)
    End Sub

    Private Sub listRegions_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles listRegions.SelectedIndexChanged
        Dim index As Short
        index = listRegions.SelectedIndex
        AlphaDepth.Text = AlphaDepthArray(index)
        BetaDepth.Text = BetaDepthArray(index)
        AlphaWidth.Text = AlphaWidthArray(index)
        BetaWidth.Text = BetaWidthArray(index)
        If (index = 4) Then
            AlphaDepth.ReadOnly = False
            BetaDepth.ReadOnly = False
            AlphaWidth.ReadOnly = False
            BetaWidth.ReadOnly = False
        Else
            AlphaDepth.ReadOnly = True
            BetaDepth.ReadOnly = True
            AlphaWidth.ReadOnly = True
            BetaWidth.ReadOnly = True
        End If
    End Sub

    Private Sub optConstSoilMoisture_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optConstSoilMoisture.CheckedChanged
        If eventSender.Checked Then
            optConstSoilMoisture.Checked = True
            optFieldCapacity.Checked = False
            optInputSoilMoisture.Checked = False
            InitialSoilMoistureConstant.Enabled = True
            cboInitial_Soil_Moisture.Enabled = False
            btnInitial_Soil_Moisture.Enabled = False
        End If
    End Sub

    Private Sub optFieldCapacity_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optFieldCapacity.CheckedChanged
        If eventSender.Checked Then
            optFieldCapacity.Checked = True
            optConstSoilMoisture.Checked = False
            optInputSoilMoisture.Checked = False
            InitialSoilMoistureConstant.Enabled = False
            cboInitial_Soil_Moisture.Enabled = False
            btnInitial_Soil_Moisture.Enabled = False
        End If
    End Sub

    Private Sub optInputSoilMoisture_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optInputSoilMoisture.CheckedChanged
        If eventSender.Checked Then
            optConstSoilMoisture.Checked = False
            optFieldCapacity.Checked = False
            optInputSoilMoisture.Checked = True
            InitialSoilMoistureConstant.Enabled = False
            cboInitial_Soil_Moisture.Enabled = True
            btnInitial_Soil_Moisture.Enabled = True
        End If
    End Sub

    Private Sub P2Map_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles P2Map.Click
        With New frmP2Map
            .ShowDialog()
            .Dispose()
        End With
    End Sub

    Private Sub btn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnInitial_Soil_Moisture.Click
        Dim btn As Button = eventSender
        Dim cbo As ComboBox = Controls(btn.Name.Replace("btn", "cbo"))
        Dim caption As String = btn.Name.Substring(3, btn.Name.Length - 3).Replace("_", " ")
        Dim selectedName As String = SelectLayerOrTableData(enumDataType.Feature, caption)
        Dim datatype As enumDataType
        Select Case btn.Tag
            Case "Feature" : datatype = enumDataType.Feature
            Case "Raster" : datatype = enumDataType.Raster
            Case "Table" : datatype = enumDataType.Table
        End Select
        Dim name As String = SelectLayerOrTableData(datatype, caption)
        If name <> "" Then cbo.Text = name
    End Sub

    ''' <summary>
    ''' Because items may be long, set tooltip whenever selected item changes
    ''' </summary>
    Private Sub cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInitial_Soil_Moisture.SelectedIndexChanged
        ToolTip1.SetToolTip(sender, CType(sender, ComboBox).Text)
    End Sub
End Class