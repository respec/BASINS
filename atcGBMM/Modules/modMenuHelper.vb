Module modMenuHelper
    Public EnableHydrology As Boolean
    Public EnableSediment As Boolean
    Public EnableMercury As Boolean

    Public Function EnableSetupModule() As Boolean

        If EnableHgMenu() = False Then Return False

        DefineApplicationPath()

        'Check if all input data file is present, is so read the file

        InitializeInputDataDictionary()

        'don't know why these must be true!

        'If My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt") Then
        '    LoadDict()
        '    Dim Value As String = ""
        '    If InputDataDictionary.TryGetValue("cbxMercury", Value) AndAlso Value <> 0 Then
        '        Return InputDataDictionary.ContainsKey("Mercury Input") And InputDataDictionary.ContainsKey("Sediment Input") And InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
        '    Else
        '        If InputDataDictionary.TryGetValue("cbxSediment", Value) AndAlso Value <> 0 Then
        '            Return InputDataDictionary.ContainsKey("Sediment Input") And InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
        '        Else
        '            Return InputDataDictionary.ContainsKey("Hydrology & Hydraulic Input")
        '        End If
        '    End If
        'End If

        Dim pLayers(8) As String
        pLayers(1) = "ClimateStation"
        pLayers(2) = "ClimateDataTextFile"
        pLayers(3) = "DEM"
        pLayers(4) = "Landuse"
        pLayers(5) = "LUcodeCNTable"
        pLayers(6) = "LuLookupTable"
        pLayers(7) = "NHD"
        pLayers(8) = "SoilMap"
        For i As Integer = 1 To 8
            Dim LayerName As String = ""
            If InputDataDictionary.TryGetValue(pLayers(i), LayerName) AndAlso Not CheckLayerFromFilePresentInMap(LayerName) Then Return False
        Next
        Return True
    End Function

    Public Function CheckLayerFromFilePresentInMap(ByVal pDataName As String) As Boolean
        Return GisUtil.IsLayer(pDataName)
    End Function

    ''' <summary>
    ''' Check that all the required temporary grids have been created in the Temp folder
    ''' </summary>
    ''' <remarks>Not sure if they have to be layers in drawing or just available for later loading; just assume these are TIF grids for now</remarks>
    Public Function EnableSimulationModule() As Boolean
        If Not EnableSetupModule() Then Return False
        Dim lstGrids As New Generic.List(Of String)
        lstGrids.AddRange(New String() {"cn2", "cnimp", "filldem", "flowaccu", "flowdir", "hydradius", "slope", "thiessenwtr", "totaltime"})

        If ComputeSedimentFlag Then
            lstGrids.Add("csl")
            lstGrids.Add("lsfactor")
        End If

        If ComputeMercuryFlag Then
            lstGrids.Add("kdcomp")
        End If

        'todo: grids can be in many formats--coordinate this with the creation of temporary grids
        For Each s As String In lstGrids
            If Not My.Computer.FileSystem.FileExists(String.Format("{0}\{1}.tif", gMapTempFolder, s)) Then Return False
        Next

        Return True
    End Function

    Public Function EnableWASPExportDrainFeatures() As Boolean
        If Not EnableSimulationModule() Then Return False

        InitializeInputDataDictionary()

        If InputDataDictionary("cbxWASP") Then
            Return GisUtil.IsLayer("Drain")
        Else
            Return False
        End If
    End Function

    'same as above--reuse
    'Public Function EnableWASPSelectDrainFeatures() As Boolean
    '    If Not EnableSimulationModule() Then Return False

    '    InitializeInputDataDictionary()
    '    If InputDataDictionary("cbxWASP") = 1 Then
    '        EnableWASPSelectDrainFeatures = True
    '        If GetInputLayer("Drain") Is Nothing Then EnableWASPSelectDrainFeatures = False
    '    Else
    '        EnableWASPSelectDrainFeatures = False
    '    End If

    'End Function

    Public Function EnableWASPInterfaceModule() As Boolean
        If Not EnableSimulationModule() Then Return False

        If GisUtil.IsLayer("Branches") Then
            Return False
        Else
            Return ComputeWASPFlag
        End If
    End Function

    Friend Enum enumModule
        Hydrology
        Sediment
        Mercury
    End Enum

    ''' <summary>
    ''' Enable Hydro, Sediment, Mercury modules
    ''' </summary>
    ''' <param name="moduletype">Type of module to enable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnableModule(ByVal moduletype As enumModule) As Boolean

        EnableHydrology = False
        EnableSediment = False
        EnableMercury = False

        InitializeInputDataDictionary()

        InputDataDictionary.TryGetValue("cbxHydro", EnableHydrology)
        InputDataDictionary.TryGetValue("cbxSediment", EnableSediment)
        InputDataDictionary.TryGetValue("cbxMercury", EnableMercury)

        Select Case moduletype
            Case enumModule.Hydrology : Return EnableHydrology
            Case enumModule.Sediment : Return EnableSediment
            Case enumModule.Mercury : Return EnableMercury
        End Select
    End Function

    Public Function EnableHgMenu() As Boolean
        If gMapWin.Project.FileName = "" Then
            MessageBox.Show("You must save your BASINS project before using GBMM.", "Warning", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If
        If IO.Path.GetFileNameWithoutExtension(gMapWin.Project.FileName).Length > 8 Then
            MessageBox.Show("The BASINS project file must be less than or equal to 8 characters in length.", "Warning", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    Public Function EnableWSDelineation() As Boolean
        If Not EnableHgMenu() Then Return False
        InitializeInputDataDictionary()
        Return GisUtil.IsLayer("AssessPoints")
    End Function

    Public Function EnableInputCreation() As Boolean
        If Not EnableHgMenu() Then Return False
        InitializeInputDataDictionary()
        Return GisUtil.IsLayer("Subwatershed")
    End Function

    Public Function EnableWASP() As Boolean
        Initialize()
        If InputDataDictionary Is Nothing Then InitializeInputDataDictionary()
        If InputDataDictionary("cbxWASP") Then Return True
        Return GisUtil.IsLayer("SubWatershed") And GisUtil.IsLayer("Branches")
    End Function

    Public Function EnableWhAEM() As Boolean
        If InputDataDictionary Is Nothing Then InitializeInputDataDictionary()
        Return InputDataDictionary("cbxWhAEM")
    End Function
End Module