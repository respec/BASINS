Imports MapWinUtility
Imports HTMLBuilder

Friend Class clsSoil
    Friend ID, Name, Group As String
    Sub New(ByVal _ID As String, ByVal _Name As String, ByVal _Group As String)
        ID = _ID
        Name = _Name
        Group = _Group
    End Sub
End Class

Friend Class clsLanduse
    Friend ID As String, Name As String, Area As Double
    Sub New(ByVal _ID As String, ByVal _Name As String, Optional ByVal _Area As Double = 0)
        ID = _ID
        Name = _Name
        Area = _Area
    End Sub
End Class

Friend Class clsProject
    Friend ProjectFolder, AppFolder, OutputFolder As String
    Friend DistFactor As Double = 1.0 'map units per meter
    Friend SubbasinLayer As String
    Friend SubbasinField As String

    Friend ReachLayer As String
    Friend ReachField As String
    Friend _303dLayer As String
    Friend WaterbodyField, ImpairmentField As String

    Friend Pop1Layer As String
    Friend Pop2Layer As String
    Friend PopNameField, PopPopField As String

    Friend SewerLayer As String
    Friend SewerNameField, SewerPopField, SewerHouseField, SewerPublicField, SewerSepticField, SewerOtherField As String

    Friend SoilLayer As String
    Friend SoilField As String
    Friend dictSoil As New Generic.SortedDictionary(Of String, clsSoil)

    Friend Enum enumLandUseType
        GIRAS
        NLCD_1992
        NLCD_2001
        UserShapefile
        UserGrid
    End Enum

    Friend LanduseType As enumLandUseType
    Friend LanduseLayer As String
    Friend LanduseField As String
    Friend LanduseIDShown As Boolean
    Friend dictLanduse(enumLandUseType.UserGrid) As Generic.SortedDictionary(Of String, clsLanduse)

    Friend PCSLayer As String
    Friend PCSNpdesField, PCSFacNameField, PCSSicField, PCSSicNameField, PCSCityField, PCSMajorField, PCSRecWaterField, PCSActiveField As String, PCSActiveOnly As Boolean

    Friend lstDatasets As New Generic.List(Of String)

    Public Sub New()
        ProjectFolder = IO.Path.GetDirectoryName(GisUtil.ProjectFileName) & "\WCS"
        If Not My.Computer.FileSystem.DirectoryExists(ProjectFolder) Then My.Computer.FileSystem.CreateDirectory(ProjectFolder)
        AppFolder = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location) & "\Plugins\WCS"
        Select Case GisUtil.MapUnits.ToUpper
            Case "METERS"
                DistFactor = 1.0
            Case "FEET"
                WarningMsg("The current project mapping units are set to 'feet', however BASINS is normally set up to user 'meters'.")
                DistFactor = 3.2808
            Case Else
                Logger.Message("The current BASINS project units are not compatible with this tool: " & GisUtil.MapUnits, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Select
        Dim Filename As String = AppFolder & "\SoilNames.txt"

        If Not My.Computer.FileSystem.FileExists(Filename) Then
            WarningMsg("Unable to find required file: {0}; replacing with default values.", Filename)
            IO.File.WriteAllText(Filename, My.Resources.SoilNames)
            Exit Sub
        End If

        Dim sr As New IO.StreamReader(Filename)
        Try
            sr.ReadLine()
            Do
                'file has MUID, Name, Hydrologic Group columns
                Dim ar() As String = sr.ReadLine.Split(vbTab)
                If ar.Length = 3 Then dictSoil.Add(ar(0), New clsSoil(ar(0), ar(1), ar(2)))
            Loop Until sr.EndOfStream
        Catch ex As Exception
            ErrorMsg("Unable to read soils data file: " & Filename, ex)
        Finally
            sr.Close()
            sr.Dispose()
        End Try

        LoadLanduses()

    End Sub

    Friend Sub LoadLanduses()
        Dim Filename As String = AppFolder & "\Landuses.txt"

        If Not My.Computer.FileSystem.FileExists(Filename) Then
            WarningMsg("Unable to find required file: {0}; replacing with default values.", Filename)
            IO.File.WriteAllText(Filename, My.Resources.LandUses)
        End If

        Dim sr As New IO.StreamReader(Filename)
        Try
            For t As enumLandUseType = enumLandUseType.GIRAS To enumLandUseType.UserGrid
                dictLanduse(t) = New Generic.SortedDictionary(Of String, clsLanduse)
                Dim ar() As String = sr.ReadLine.Split(vbTab)
                If ar.Length <> 2 Then Throw New ApplicationException("Invalid file format in " & Filename)
                Dim num As Integer = ar(1)
                For i As Integer = 0 To num - 1
                    ar = sr.ReadLine.Split(vbTab)
                    If ar.Length <> 2 Then Throw New ApplicationException("Invalid file format in " & Filename)
                    dictLanduse(t).Add(ar(0), New clsLanduse(ar(0), ar(1)))
                Next
            Next
        Catch ex As Exception
            ErrorMsg("Unable to read landuse data file: " & Filename, ex)
        Finally
            sr.Close()
            sr.Dispose()
        End Try
    End Sub

    Friend Sub SaveLanduses()
        Dim Filename As String = AppFolder & "\Landuses.txt"

        Dim sw As New IO.StreamWriter(Filename)
        Try
            For t As enumLandUseType = enumLandUseType.GIRAS To enumLandUseType.UserGrid
                sw.WriteLine(Choose(t + 1, "GIRAS", "NLCD-1992", "NLCD-2001", "USER-SHAPEFILE", "USER-GRIDFILE") & vbTab & dictLanduse(t).Count)
                For Each kv As Generic.KeyValuePair(Of String, clsLanduse) In dictLanduse(t)
                    sw.WriteLine(kv.Key & vbTab & kv.Value.Name)
                Next
            Next
        Catch ex As Exception
            ErrorMsg("Unable to write landuse data file: " & Filename, ex)
        Finally
            sw.Close()
            sw.Dispose()
        End Try
    End Sub

#Region "Project Load and Save"

    Friend Sub Load()
        Dim FileName As String = ProjectFolder & "\WCS.ini"
        Dim iniFile As New WRDB.IniFile.clsIniFile(FileName)
        OutputFolder = iniFile.GetKeyText("Folders", "OutputFolder", ProjectFolder & "\WCS Reports")
        SubbasinLayer = iniFile.GetKeyText("Layers", "SubbasinLayer", "Subbasins")
        SubbasinField = iniFile.GetKeyText("Fields", "SubbasinField", "NAME")
        ReachLayer = iniFile.GetKeyText("Layers", "ReachLayer", "Reach File*")
        ReachField = iniFile.GetKeyText("Fields", "ReachField", "PNAME")
        _303dLayer = iniFile.GetKeyText("Layers", "_303dLayer", "303d*")
        WaterbodyField = iniFile.GetKeyText("Fields", "WaterbodyField", "WBODY_NAME")
        ImpairmentField = iniFile.GetKeyText("Fields", "ImpairmentField", "EPA_IMPAIR")
        Pop1Layer = iniFile.GetKeyText("Layers", "Pop1Layer", "1990 County*")
        Pop2Layer = iniFile.GetKeyText("Layers", "Pop2Layer", "2000 County*")
        PopNameField = iniFile.GetKeyText("Fields", "PopNameField", "NAME")
        PopPopField = iniFile.GetKeyText("Fields", "PopPopField", "Population")
        SewerLayer = iniFile.GetKeyText("Layers", "SewerLayer", "1990 County*")
        SewerNameField = iniFile.GetKeyText("Fields", "SewerNameField", "NAME")
        SewerPopField = iniFile.GetKeyText("Fields", "SewerPopField", "Population")
        SewerHouseField = iniFile.GetKeyText("Fields", "SewerHouseField", "HouseUnits")
        SewerPublicField = iniFile.GetKeyText("Fields", "SewerPublicField", "SewrPublic")
        SewerSepticField = iniFile.GetKeyText("Fields", "SewerSepticField", "SewrSeptic")
        SewerOtherField = iniFile.GetKeyText("Fields", "SewerOtherField", "SewrOther")
        SoilLayer = iniFile.GetKeyText("Layers", "SoilLayer", "State Soil")
        SoilField = iniFile.GetKeyText("Fields", "SoilField", "MUID")
        LanduseType = iniFile.GetKeyText("Layers", "LanduseType", 0)
        LanduseLayer = iniFile.GetKeyText("Layers", "LanduseLayer", "Land*")
        LanduseField = iniFile.GetKeyText("Fields", "LanduseField", "LUCODE")
        LanduseIDShown = iniFile.GetKeyText("Fields", "LanduseIDShown", False)
        PCSLayer = iniFile.GetKeyText("Layers", "PCSLayer", "Permit*")
        PCSNpdesField = iniFile.GetKeyText("Fields", "PCSNpdesField", "NPDES")
        PCSFacNameField = iniFile.GetKeyText("Fields", "PCSFacNameField", "FAC_NAME")
        PCSSicField = iniFile.GetKeyText("Fields", "PCSSICField", "SIC2")
        PCSSicNameField = iniFile.GetKeyText("Fields", "PCSSicNameField", "SIC2D")
        PCSCityField = iniFile.GetKeyText("Fields", "PCSCityField", "CITY")
        PCSMajorField = iniFile.GetKeyText("Fields", "PCSMajorField", "MAJOR_ID")
        PCSRecWaterField = iniFile.GetKeyText("Fields", "PCSRecWaterField", "REC_WATER")
        PCSActiveField = iniFile.GetKeyText("Fields", "PCSActiveField", "ACTIVE")
        PCSActiveOnly = iniFile.GetKeyText("Fields", "PCSActiveOnly", "True")
    End Sub

    Friend Sub Save()
        Dim FileName As String = ProjectFolder & "\WCS.ini"
        Dim iniFile As New WRDB.IniFile.clsIniFile(FileName)
        iniFile.SetKeyText("Folders", "OutputFolder", OutputFolder)
        iniFile.SetKeyText("Layers", "SubbasinLayer", SubbasinLayer)
        iniFile.SetKeyText("Fields", "SubbasinField", SubbasinField)
        iniFile.SetKeyText("Layers", "ReachLayer", ReachLayer)
        iniFile.SetKeyText("Fields", "ReachField", ReachField)
        iniFile.SetKeyText("Layers", "_303dLayer", _303dLayer)
        iniFile.SetKeyText("Fields", "WaterbodyField", WaterbodyField)
        iniFile.SetKeyText("Fields", "ImpairmentField", ImpairmentField)
        iniFile.SetKeyText("Layers", "Pop1Layer", Pop1Layer)
        iniFile.SetKeyText("Layers", "Pop2Layer", Pop2Layer)
        iniFile.SetKeyText("Fields", "PopNameField", PopNameField)
        iniFile.SetKeyText("Fields", "PopPopField", PopPopField)
        iniFile.SetKeyText("Layers", "SewerLayer", SewerLayer)
        iniFile.SetKeyText("Fields", "SewerNameField", SewerNameField)
        iniFile.SetKeyText("Fields", "SewerPopField", SewerPopField)
        iniFile.SetKeyText("Fields", "SewerHouseField", SewerHouseField)
        iniFile.SetKeyText("Fields", "SewerPublicField", SewerPublicField)
        iniFile.SetKeyText("Fields", "SewerSepticField", SewerSepticField)
        iniFile.SetKeyText("Fields", "SewerOtherField", SewerOtherField)
        iniFile.SetKeyText("Layers", "SoilLayer", SoilLayer)
        iniFile.SetKeyText("Fields", "SoilField", SoilField)
        iniFile.SetKeyText("Layers", "LanduseType", LanduseType)
        iniFile.SetKeyText("Layers", "LanduseLayer", LanduseLayer)
        iniFile.SetKeyText("Fields", "LanduseField", LanduseField)
        iniFile.SetKeyText("Fields", "LanduseIDShown", LanduseIDShown)
        iniFile.SetKeyText("Layers", "PCSLayer", PCSLayer)
        iniFile.SetKeyText("Fields", "PCSNpdesField", PCSNpdesField)
        iniFile.SetKeyText("Fields", "PCSFacNameField", PCSFacNameField)
        iniFile.SetKeyText("Fields", "PCSSICField", PCSSicField)
        iniFile.SetKeyText("Fields", "PCSSicNameField", PCSSicNameField)
        iniFile.SetKeyText("Fields", "PCSCityField", PCSCityField)
        iniFile.SetKeyText("Fields", "PCSMajorField", PCSMajorField)
        iniFile.SetKeyText("Fields", "PCSRecWaterField", PCSRecWaterField)
        iniFile.SetKeyText("Fields", "PCSActiveField", PCSActiveField)
        iniFile.SetKeyText("Fields", "PCSActiveOnly", PCSActiveOnly)
        iniFile.Save()
    End Sub

#End Region

#Region "Reports"

    ''' <summary>
    ''' Created concatenated report consisting of selected subreports
    ''' </summary>
    ''' <param name="CheckedReports">List of checked report numbers</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ConcatReport(ByVal CheckedReports As CheckedListBox.CheckedIndexCollection) As String
        Dim hb As New clsHTMLBuilder
        hb.AppendHeading(clsHTMLBuilder.enumHeading.Level2, clsHTMLBuilder.enumAlign.Center, "BASINS Watershed Characterization System")

        hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent, clsHTMLBuilder.enumBorderStyle.none, , clsHTMLBuilder.enumDividerStyle.None)
        hb.AppendTableColumn("", , , 200, clsHTMLBuilder.enumWidthUnits.Pixels)
        hb.AppendTableColumn("")

        If CheckedReports.Contains(0) Then AppendWaterBodyTables(hb)
        If CheckedReports.Contains(1) Then AppendPopulationTable(hb)
        If CheckedReports.Contains(2) Then AppendSewerTable(hb)
        If CheckedReports.Contains(3) Then AppendSoilTable(hb)
        If CheckedReports.Contains(4) Then AppendLanduseTable(hb)
        If CheckedReports.Contains(5) Then AppendPCSTable(hb)
        If CheckedReports.Contains(6) Then AppendDataTable(hb)

        If Not My.Computer.FileSystem.DirectoryExists(OutputFolder) Then My.Computer.FileSystem.CreateDirectory(OutputFolder)
        'find highest report number
        Dim NextRptNum As Integer = 1
        For Each rpt As String In My.Computer.FileSystem.GetFiles(OutputFolder, FileIO.SearchOption.SearchTopLevelOnly, "Report*.htm")
            NextRptNum = Math.Max(NextRptNum, Val(IO.Path.GetFileNameWithoutExtension(rpt).Replace("Report", "")) + 1)
        Next
        Dim OutputFile As String = String.Format("{0}\Report{1}.htm", OutputFolder, NextRptNum)
        hb.Save(OutputFile)
        Return OutputFile
    End Function

    Private Class clsStream
        Friend Name As String
        Friend Length As Single
        Friend NumSegs As Integer
        Friend Impairment As String
        Sub New(ByVal _Name As String, ByVal _Impairment As String)
            Name = _Name
            Impairment = _Impairment
            Length = 0
            NumSegs = 0
        End Sub
    End Class

    ''' <summary>
    ''' Create HTML table containing summary of state waterbodies [RF1] and listed waterbodies [303d] by subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendWaterBodyTables(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            Dim TableName As String = "State Waterbodies"

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable()
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Waterbody Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("No. Segments", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Total Length (mi)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               String.IsNullOrEmpty(ReachLayer) Or _
               String.IsNullOrEmpty(ReachField) Or _
               String.IsNullOrEmpty(_303dLayer) Or _
               String.IsNullOrEmpty(WaterbodyField) Or _
               String.IsNullOrEmpty(ImpairmentField) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim fldS As Integer = GisUtil.FieldIndex(lyrS, SubbasinField)
            Dim lyrReach As Integer = GisUtil.LayerIndex(ReachLayer)
            Dim fldReach As Integer = GisUtil.FieldIndex(lyrReach, ReachField)
            Dim lyr303d As Integer = GisUtil.LayerIndex(_303dLayer)
            Dim fldWaterbody As Integer = GisUtil.FieldIndex(lyr303d, WaterbodyField)
            Dim fldImpairment As Integer = GisUtil.FieldIndex(lyr303d, ImpairmentField)
            Dim dictWaterbody As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, clsStream))

            For i As Integer = 0 To GisUtil.NumFeatures(lyrS) - 1
                Dim SubName As String = GisUtil.FieldValue(lyrS, i, fldS)
                If Not dictWaterbody.ContainsKey(SubName) Then dictWaterbody.Add(SubName, New Generic.SortedDictionary(Of String, clsStream))
                For j As Integer = 0 To GisUtil.NumFeatures(lyrReach) - 1
                    If GisUtil.LineInPolygon(lyrReach, j, lyrS, i) Then
                        Dim Name As String = GisUtil.FieldValue(lyrReach, j, fldReach)
                        If Name Is Nothing Then
                            WarningMsg("A null entry for the reach name was found in " & ReachLayer)
                            Exit Sub
                        End If
                        If Not dictWaterbody(SubName).ContainsKey(Name) Then dictWaterbody(SubName).Add(Name, New clsStream(Name, ""))
                        With dictWaterbody(SubName)(Name)
                            .NumSegs += 1
                            .Length += GisUtil.FeatureLength(lyrReach, j) / (DistFactor * 1609.3) 'convert to miles
                        End With
                    End If
                    If Not WCSForm.UpdateProgress("Tabulating waterbodies...", j, GisUtil.NumFeatures(lyrReach) - 1) Then Exit Sub
                Next
            Next

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsStream)) In dictWaterbody
                For Each kv2 As KeyValuePair(Of String, clsStream) In kv.Value
                    With kv2.Value
                        hb.AppendTableRow(kv.Key, .Name, .NumSegs, .Length.ToString("0.0"))
                    End With
                Next
            Next
            hb.AppendTableEnd()

            TableName = "Listed 303d Waterbodies"

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable()
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Waterbody Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Total Length (mi)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Year Listed", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Source", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Use", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Criteria Violated", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)

            dictWaterbody.Clear()

            For i As Integer = 0 To GisUtil.NumFeatures(lyrS) - 1
                Dim SubName As String = GisUtil.FieldValue(lyrS, i, fldS)
                If Not dictWaterbody.ContainsKey(SubName) Then dictWaterbody.Add(SubName, New Generic.SortedDictionary(Of String, clsStream))
                For j As Integer = 0 To GisUtil.NumFeatures(lyr303d) - 1
                    If GisUtil.LineInPolygon(lyr303d, j, lyrS, i) Then
                        Dim Name As String = GisUtil.FieldValue(lyr303d, j, fldWaterbody)
                        Dim Impair As String = GisUtil.FieldValue(lyr303d, j, fldImpairment)
                        If Not dictWaterbody(SubName).ContainsKey(Name & Impair) Then dictWaterbody(SubName).Add(Name & Impair, New clsStream(Name, Impair))
                        With dictWaterbody(SubName)(Name & Impair)
                            .NumSegs += 1
                            .Length += GisUtil.FeatureLength(lyr303d, j) / (DistFactor * 1609.3) 'convert to miles
                        End With
                    End If
                    If Not WCSForm.UpdateProgress("Tabulating waterbodies...", j, GisUtil.NumFeatures(lyr303d) - 1) Then Exit Sub
                Next
            Next

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsStream)) In dictWaterbody
                For Each kv2 As KeyValuePair(Of String, clsStream) In kv.Value
                    With kv2.Value
                        hb.AppendTableRow(kv.Key, .Name, .Length.ToString("0.0"), "", "", "", .Impairment)
                    End With
                Next
            Next
            hb.AppendTableEnd()
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Progress("", 1, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of census data by subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendPopulationTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            Dim TableName As String = "Subbasin Population Estimates"

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               String.IsNullOrEmpty(Pop1Layer) Or _
               String.IsNullOrEmpty(Pop2Layer) Or _
               String.IsNullOrEmpty(PopNameField) Or _
               String.IsNullOrEmpty(PopPopField) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable()
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Census Area Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Total Census Population - " & Val(Pop1Layer), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Total Census Population - " & Val(Pop2Layer), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Portion of Watershed (%)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Est. Watershed Population - " & Val(Pop1Layer), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Est. Watershed Population - " & Val(Pop2Layer), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Percent Change", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            Dim dictPop1, dictPop2 As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
            dictPop1 = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, Pop1Layer, PopNameField)
            If dictPop1 Is Nothing Then Exit Sub
            dictPop2 = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, Pop2Layer, PopNameField)
            If dictPop2 Is Nothing Then Exit Sub
            Dim lyrIndex1 As Integer = GisUtil.LayerIndex(Pop1Layer)
            Dim fldNameIndex1 As Integer = GisUtil.FieldIndex(lyrIndex1, PopNameField)
            Dim fldPopIndex1 As Integer = GisUtil.FieldIndex(lyrIndex1, PopPopField)
            Dim lyrIndex2 As Integer = GisUtil.LayerIndex(Pop2Layer)
            Dim fldNameIndex2 As Integer = GisUtil.FieldIndex(lyrIndex2, PopNameField)
            Dim fldPopIndex2 As Integer = GisUtil.FieldIndex(lyrIndex2, PopPopField)

            If dictPop1.Count <> dictPop2.Count Then Throw New ApplicationException("Unequal number of subbasins in two census layers.")

            For Each kv1 As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictPop1
                Dim SubName As String = kv1.Key
                Dim sumPop1 As Integer = 0
                Dim sumPop2 As Integer = 0
                If Not dictPop2.ContainsKey(SubName) Then Throw New ApplicationException("Missing subbasin name in second census layer: " & SubName)
                Dim ctr As Integer = 0
                For Each kv2 As KeyValuePair(Of String, Single) In kv1.Value
                    Dim CensusName As String = kv2.Key
                    Dim Area1 As Single = kv2.Value
                    Dim shpIndex1 As Integer = GisUtil.FindFeatureIndex(lyrIndex1, fldNameIndex1, CensusName)
                    Dim Pop1 As Integer = GisUtil.FieldValue(lyrIndex1, shpIndex1, fldPopIndex1)
                    Dim pct As Single = Area1 / GisUtil.FeatureArea(lyrIndex1, shpIndex1) 'should be same as for #2
                    If pct < 0.005 Then Continue For
                    If Not dictPop2(SubName).ContainsKey(CensusName) Then Throw New ApplicationException(String.Format("Missing census name in second census layer: {0}. Note that tracts may be subdivided or removed in consecutive censuses, so it may not be possible to prepare the population report by tract.", CensusName))
                    Dim Area2 As Single = dictPop2(SubName)(CensusName)
                    Dim shpIndex2 As Integer = GisUtil.FindFeatureIndex(lyrIndex2, fldNameIndex2, CensusName)
                    Dim Pop2 As Integer = GisUtil.FieldValue(lyrIndex2, shpIndex2, fldPopIndex2)
                    Dim pctChange As Single = (Pop2 - Pop1) * 100.0 / Pop1
                    hb.AppendTableRow(SubName, CensusName, Pop1, Pop2, (pct * 100.0).ToString("0.00"), CInt(Pop1 * pct), CInt(Pop2 * pct), pctChange.ToString("0.0"))
                    sumPop1 += CInt(Pop1 * pct)
                    sumPop2 += CInt(Pop2 * pct)
                    Logger.Progress("Tabulating population...", ctr, kv1.Value.Count - 1)
                Next
                Dim pctChg As Single = (sumPop2 - sumPop1) * 100.0 / sumPop1
                hb.AppendTableRow(SubName, "Totals", "", "", "", sumPop1, sumPop2, pctChg.ToString("0.0"))
            Next
            hb.AppendTableEnd()
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Progress("", 1, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of sewerage data by subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendSewerTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            Dim TableName As String = "Housing and Sewage Disposal Practices"

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               String.IsNullOrEmpty(Pop1Layer) Or _
               String.IsNullOrEmpty(SewerNameField) Or _
               String.IsNullOrEmpty(SewerPopField) Or _
               String.IsNullOrEmpty(SewerHouseField) Or _
               String.IsNullOrEmpty(SewerPublicField) Or _
               String.IsNullOrEmpty(SewerSepticField) Or _
               String.IsNullOrEmpty(SewerOtherField) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(70, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Census Area Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Total Census Population - " & Val(Pop1Layer), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Number of Housing Units", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Public Sewer", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Septic Tank", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Other", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim lyr As Integer = GisUtil.LayerIndex(SewerLayer)
            Dim fldName As Integer = GisUtil.FieldIndex(lyr, SewerNameField)
            Dim fldPop As Integer = GisUtil.FieldIndex(lyr, SewerPopField)
            Dim fldHouse As Integer = GisUtil.FieldIndex(lyr, SewerHouseField)
            Dim fldPublic As Integer = GisUtil.FieldIndex(lyr, SewerPublicField)
            Dim fldSeptic As Integer = GisUtil.FieldIndex(lyr, SewerSepticField)
            Dim fldOther As Integer = GisUtil.FieldIndex(lyr, SewerOtherField)

            Dim dictArea As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
            dictArea = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, SewerLayer, SewerNameField)
            If dictArea Is Nothing Then Exit Sub

            'get list of census tract (e.g., county) names so can list alphabetically
            Dim lstNames As New Generic.List(Of String)
            For j As Integer = 0 To GisUtil.NumFeatures(lyr) - 1
                lstNames.Add(GisUtil.FieldValue(lyr, j, fldName))
            Next
            lstNames.Sort()

            For Each Name As String In lstNames
                Logger.Progress(String.Format("Tabulating {0} summary...", TableName), lstNames.IndexOf(Name), lstNames.Count - 1)
                Dim j As Integer = GisUtil.FindFeatureIndex(lyr, fldName, Name)
                Dim InSub As Boolean = False
                For i As Integer = 0 To GisUtil.NumFeatures(lyrS) - 1
                    If GisUtil.OverlappingPolygons(lyrS, i, lyr, j) Then
                        Dim area As Single = GisUtil.AreaOverlappingPolygons(lyr, j, lyrS, i)
                        Dim pct As Single = area / GisUtil.FeatureArea(lyrS, i)
                        InSub = pct > 0.005 'may be tiny sliver
                    End If
                    If GisUtil.Cancel Then Exit Sub
                Next

                If InSub Then
                    Dim pop As Integer = GisUtil.FieldValue(lyr, j, fldPop)
                    Dim hse As Integer = GisUtil.FieldValue(lyr, j, fldHouse)
                    Dim pub As Integer = GisUtil.FieldValue(lyr, j, fldPublic)
                    Dim sep As Integer = GisUtil.FieldValue(lyr, j, fldSeptic)
                    Dim oth As Integer = GisUtil.FieldValue(lyr, j, fldOther)
                    hb.AppendTableRow(Name, pop, hse, pub, sep, oth)
                End If
            Next
            hb.AppendTableEnd()

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Status("")
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of soil types by subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendSoilTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)

        Try
            GisUtil.Cancel = False
            Dim TableName As String = "Soil Characteristics by STATSGO Soil Map Units"

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               String.IsNullOrEmpty(SoilLayer) Or _
               String.IsNullOrEmpty(SoilField) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(70, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Soil ID", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Soil Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Hyd. Soil Group", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Area (ac)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Portion of Watershed (%)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim fldS As Integer = GisUtil.FieldIndex(lyrS, SubbasinField)

            Dim dictArea As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
            dictArea = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, SoilLayer, SoilField)
            If dictArea Is Nothing Then Exit Sub

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictArea
                Dim SubName As String = kv.Key
                Dim SubIndex As Integer = GisUtil.FindFeatureIndex(lyrS, fldS, SubName)
                Dim SubArea As Single = GisUtil.FeatureArea(lyrS, SubIndex) / (DistFactor ^ 2 * 4046.86)
                For Each kv2 As KeyValuePair(Of String, Single) In kv.Value
                    Dim ID As String = kv2.Key
                    Dim Area As Single = kv2.Value / (DistFactor ^ 2 * 4046.86)
                    Dim Name As String = ""
                    Dim Group As String = ""
                    Dim soil As clsSoil = Nothing
                    If dictSoil.TryGetValue(ID, soil) Then
                        Name = soil.Name
                        Group = soil.Group
                    End If
                    If Area > 0 Then hb.AppendTableRow(SubName, ID, Name, Group, Area.ToString("0.0"), (Area * 100 / SubArea).ToString("0.00"))
                Next
                hb.AppendTableRow(SubName, "", "Totals", "", SubArea.ToString("0.0"), "100.00")
            Next
            hb.AppendTableEnd()

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Status("")
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of landuses used for specified subbasins
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendLanduseTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            'Dim TableName As String = "Land Use Distribution by Subbasin & Landuse"

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               (LanduseType <> enumLandUseType.GIRAS And String.IsNullOrEmpty(LanduseLayer)) Or _
               (LanduseType = enumLandUseType.UserShapefile And String.IsNullOrEmpty(LanduseField)) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            'hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            'hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent)
            'hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            'If LanduseIDShown Then hb.AppendTableColumn("Land Use ID", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            'hb.AppendTableColumn("Description", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            'hb.AppendTableColumn("Area (ac)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            'hb.AppendTableColumn("Portion of Watershed (%)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim fldS As Integer = GisUtil.FieldIndex(lyrS, SubbasinField)
            Dim LUField As String

            'create dictionary where key is subbasin name and each entry is dictionary of landuse code and area
            Dim dictAreaSum As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))

            Select Case LanduseType
                Case enumLandUseType.GIRAS, enumLandUseType.UserShapefile
                    Dim lstLyr As Generic.List(Of Integer)
                    If LanduseType = enumLandUseType.GIRAS Then
                        lstLyr = GIRASLayers(lyrS)
                        LUField = "LUCODE"
                    Else
                        lstLyr = New Generic.List(Of Integer)
                        lstLyr.Add(GisUtil.LayerIndex(LanduseLayer))
                        LUField = LanduseField
                    End If
                    For Each lyr As Integer In lstLyr
                        Dim dictArea As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
                        dictArea = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, GisUtil.LayerName(lyr), LUField)
                        If dictArea Is Nothing Then Exit Sub
                        For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictArea
                            Dim SubName As String = kv.Key
                            If Not dictAreaSum.ContainsKey(SubName) Then dictAreaSum.Add(SubName, New SortedDictionary(Of String, Single))
                            For Each kv2 As KeyValuePair(Of String, Single) In kv.Value
                                Dim LUCode As String = kv2.Key
                                Dim Area As Single = kv2.Value
                                If Not dictAreaSum(SubName).ContainsKey(LUCode) Then dictAreaSum(SubName).Add(LUCode, 0.0)
                                dictAreaSum(SubName)(LUCode) += Area
                            Next
                        Next
                    Next
                Case Else
                    dictAreaSum = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, LanduseLayer)
            End Select

            If dictAreaSum Is Nothing Then Exit Sub

            'create dictionary to hold data for two summary tables:

            Dim dictSubAreaLanduse As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, clsLanduse))

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictAreaSum
                Dim SubName As String = kv.Key
                'Dim SubIndex As Integer = GisUtil.FindFeatureIndex(lyrS, fldS, SubName)
                'Dim SubArea As Single = GisUtil.FeatureArea(lyrS, SubIndex) / (DistFactor ^ 2 * 4046.86)

                'areas are now tabulated for each subbasin by ID; want to report by name (in case user aggregated by using same name for more than one ID)
                'so form new sorted list by name and accumulate all codes with that name

                Dim dictLanduseName As New Generic.SortedDictionary(Of String, clsLanduse)

                For Each kv2 As KeyValuePair(Of String, Single) In kv.Value
                    Dim LUCode As String = kv2.Key.Replace("<", "(").Replace(">", ")")
                    Dim Area As Single = kv2.Value / (DistFactor ^ 2 * 4046.86)
                    Dim Name As String = StrConv(LUCode, VbStrConv.ProperCase)
                    Dim landuse As clsLanduse = Nothing
                    If dictLanduse(LanduseType).TryGetValue(LUCode, landuse) AndAlso Not String.IsNullOrEmpty(landuse.Name) Then Name = landuse.Name
                    If Area > 0 Then
                        If dictLanduseName.ContainsKey(Name) Then
                            dictLanduseName(Name).ID &= "; " & LUCode
                            dictLanduseName(Name).Area += Area
                        Else
                            dictLanduseName.Add(Name, New clsLanduse(LUCode, Name, Area))
                        End If
                    End If
                    If Not dictLanduse(LanduseType).ContainsKey(LUCode) Then
                        dictLanduse(LanduseType).Add(LUCode, New clsLanduse(LUCode, Name))
                    End If
                Next

                dictSubAreaLanduse.Add(SubName, dictLanduseName)

                'For Each kv3 As KeyValuePair(Of String, clsLanduse) In dictLanduseName
                '    If LanduseIDShown Then
                '        hb.AppendTableRow(SubName, kv3.Value.ID, kv3.Value.Name, kv3.Value.Area.ToString("0.0"), (kv3.Value.Area * 100 / SubArea).ToString("0.00"))
                '    Else
                '        hb.AppendTableRow(SubName, kv3.Value.Name, kv3.Value.Area.ToString("0.0"), (kv3.Value.Area * 100 / SubArea).ToString("0.00"))
                '    End If
                'Next

                'If LanduseIDShown Then
                '    hb.AppendTableRow(SubName, "", "Totals", SubArea.ToString("0.0"), "100.00")
                'Else
                '    hb.AppendTableRow(SubName, "Totals", SubArea.ToString("0.0"), "100.00")
                'End If
            Next
            'hb.AppendTableEnd()

            'create detailed list by subbasin and land use description with areas and percentages

            Dim TableName As String = "Land Use Distribution by Subbasin & Landuse"

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            If LanduseIDShown Then hb.AppendTableColumn("Land Use ID", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Description", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Area (ac)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Portion of Watershed (%)", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            For Each kv As Generic.KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsLanduse)) In dictSubAreaLanduse
                Dim SubName As String = kv.Key
                Dim SubIndex As Integer = GisUtil.FindFeatureIndex(lyrS, fldS, SubName)
                Dim SubArea As Single = GisUtil.FeatureArea(lyrS, SubIndex) / (DistFactor ^ 2 * 4046.86)
                Dim dictLanduseName As Generic.SortedDictionary(Of String, clsLanduse) = kv.Value
                For Each kv3 As KeyValuePair(Of String, clsLanduse) In dictLanduseName
                    If LanduseIDShown Then
                        hb.AppendTableRow(SubName, kv3.Value.ID, kv3.Value.Name, kv3.Value.Area.ToString("0.0"), (kv3.Value.Area * 100 / SubArea).ToString("0.00"))
                    Else
                        hb.AppendTableRow(SubName, kv3.Value.Name, kv3.Value.Area.ToString("0.0"), (kv3.Value.Area * 100 / SubArea).ToString("0.00"))
                    End If
                Next

                If LanduseIDShown Then
                    hb.AppendTableRow(SubName, "", "Totals", SubArea.ToString("0.0"), "100.00")
                Else
                    hb.AppendTableRow(SubName, "Totals", SubArea.ToString("0.0"), "100.00")
                End If
            Next

            hb.AppendTableEnd()

            'add second table in which each row is subbasin and each column is land use description (cells are area)

            TableName = "Land Use Area (ac) by Subbasin"

            'get list of unique land use descriptions for ALL subbasins
            Dim LUName As New Generic.List(Of String)
            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsLanduse)) In dictSubAreaLanduse
                For Each kv2 As KeyValuePair(Of String, clsLanduse) In kv.Value
                    Dim Name As String = kv2.Key
                    If Not LUName.Contains(Name) Then LUName.Add(Name)
                Next
            Next
            LUName.Sort()

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            For Each s As String In LUName
                hb.AppendTableColumn(s.Replace("/", "/ "), clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            Next
            hb.AppendTableColumn("Totals", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsLanduse)) In dictSubAreaLanduse
                Dim SubName As String = kv.Key
                Dim SubIndex As Integer = GisUtil.FindFeatureIndex(lyrS, fldS, SubName)
                Dim SubArea As Single = GisUtil.FeatureArea(lyrS, SubIndex) / (DistFactor ^ 2 * 4046.86)
                hb.AppendTableRow()
                hb.AppendTableCell(SubName)
                For Each s As String In LUName
                    If kv.Value.ContainsKey(s) Then
                        Dim Area As Single = kv.Value(s).Area
                        hb.AppendTableCell(Area.ToString("0.0"))
                    Else
                        hb.AppendTableCell("")
                    End If
                Next
                hb.AppendTableCell(SubArea.ToString("0.0"))
                hb.AppendTableRowEnd()
            Next

            hb.AppendTableEnd()

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Progress("", 1, 1)
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' Retrieve list of unique land use categories in all subbasins and store in dictionary
    ''' </summary>
    Friend Sub GetLanduses(ByVal LandUseType As enumLandUseType)
        Try
            GisUtil.Cancel = False

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               (LanduseType <> enumLandUseType.GIRAS And String.IsNullOrEmpty(LanduseLayer)) Or _
               (LanduseType = enumLandUseType.UserShapefile And String.IsNullOrEmpty(LanduseField)) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim fldS As Integer = GisUtil.FieldIndex(lyrS, SubbasinField)
            Dim LUField As String

            Dim dictAreaSum As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))

            Select Case LanduseType
                Case enumLandUseType.GIRAS, enumLandUseType.UserShapefile
                    Dim lstLyr As Generic.List(Of Integer)
                    If LanduseType = enumLandUseType.GIRAS Then
                        lstLyr = GIRASLayers(lyrS)
                        LUField = "LUCODE"
                    Else
                        lstLyr = New Generic.List(Of Integer)
                        lstLyr.Add(GisUtil.LayerIndex(LanduseLayer))
                        LUField = LanduseField
                    End If
                    For Each lyr As Integer In lstLyr
                        Dim dictArea As Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, Single))
                        dictArea = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, GisUtil.LayerName(lyr), LUField)
                        If dictArea Is Nothing Then Exit Sub
                        For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictArea
                            Dim SubName As String = kv.Key
                            If Not dictAreaSum.ContainsKey(SubName) Then dictAreaSum.Add(SubName, New SortedDictionary(Of String, Single))
                            For Each kv2 As KeyValuePair(Of String, Single) In kv.Value
                                Dim LUCode As String = kv2.Key
                                Dim Area As Single = kv2.Value
                                If Not dictAreaSum(SubName).ContainsKey(LUCode) Then dictAreaSum(SubName).Add(LUCode, 0.0)
                                dictAreaSum(SubName)(LUCode) += Area
                            Next
                        Next
                    Next
                Case Else
                    dictAreaSum = GisUtil.TabulateAreas(SubbasinLayer, SubbasinField, LanduseLayer)
            End Select

            If dictAreaSum Is Nothing Then Exit Sub

            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, Single)) In dictAreaSum
                Dim SubName As String = kv.Key
                Dim SubIndex As Integer = GisUtil.FindFeatureIndex(lyrS, fldS, SubName)
                Dim SubArea As Single = GisUtil.FeatureArea(lyrS, SubIndex) / (DistFactor ^ 2 * 4046.86)
                For Each kv2 As KeyValuePair(Of String, Single) In kv.Value
                    Dim LUCode As String = kv2.Key.Replace("<", "(").Replace(">", ")")
                    Dim Area As Single = kv2.Value / (DistFactor ^ 2 * 4046.86)
                    Dim Name As String = StrConv(LUCode, VbStrConv.ProperCase)
                    Dim landuse As clsLanduse = Nothing
                    If dictLanduse(LandUseType).TryGetValue(LUCode, landuse) AndAlso Not String.IsNullOrEmpty(landuse.Name) Then Name = landuse.Name
                    If Not dictLanduse(LandUseType).ContainsKey(LUCode) Then
                        dictLanduse(LandUseType).Add(LUCode, New clsLanduse(LUCode, Name))
                    End If
                Next
            Next

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Progress("", 1, 1)
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' GIRAS land use consists of multiple layers; these shapefiles are contained as field values in the "Land Use Index" layer
    ''' This routine will return the list of shapefiles contained in the GIRAS landuse coverages
    ''' Will return empty list if an error occurs
    ''' </summary>
    ''' <param name="BasinLayerIndex">Layer index associated with subbasins</param>
    Friend Function GIRASLayers(ByVal BasinLayerIndex As Integer) As Generic.List(Of Integer)
        Try
            Const LayerName As String = "Land Use Index", FieldName As String = "COVNAME"
            Dim ShapeFiles As New Generic.List(Of Integer)

            If GisUtil.IsLayer(LayerName) Then
                Dim LayerIndex As Integer = GisUtil.LayerIndex(LayerName)
                If GisUtil.IsField(LayerIndex, FieldName) Then
                    Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, FieldName)
                    For i As Integer = 0 To GisUtil.NumFeatures(LayerIndex) - 1
                        Dim LyrName As String = GisUtil.FieldValue(LayerIndex, i, FieldIndex)
                        Dim LayerFileName As String = IO.Path.GetDirectoryName(GisUtil.LayerFileName(LayerIndex)) & "\landuse\" & LyrName & ".shp"
                        For j As Integer = 0 To GisUtil.NumFeatures(BasinLayerIndex) - 1
                            If GisUtil.OverlappingPolygons(LayerIndex, i, BasinLayerIndex, j) Then
                                If GisUtil.IsLayer(LayerFileName) Then ShapeFiles.Add(GisUtil.LayerIndex(LayerFileName))
                                Exit For
                            End If
                        Next j
                    Next
                End If
            End If
            Return ShapeFiles
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function

    Private Class clsPCS
        Friend NPDES, FacName, Sic, SicName, City, Major, RecWater, Active As String, ActiveOnly As Boolean
        Sub New(ByVal _NPDES As String, ByVal _FacName As String, ByVal _Sic As String, ByVal _SicName As String, ByVal _City As String, ByVal _Major As String, ByVal _RecWater As String, ByVal _Active As String, ByVal _ActiveOnly As Boolean)
            NPDES = _NPDES
            FacName = _FacName
            Sic = _Sic
            SicName = _SicName
            City = _City
            Major = _Major
            RecWater = _RecWater
            Active = _Active
            ActiveOnly = _ActiveOnly
        End Sub
    End Class

    ''' <summary>
    ''' Create HTML table containing summary of NPDES permits by subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendPCSTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            Dim TableName As String = "Active Permitted Point Source Facilities"

            If String.IsNullOrEmpty(SubbasinLayer) Or _
               String.IsNullOrEmpty(SubbasinField) Or _
               String.IsNullOrEmpty(PCSLayer) Or _
               String.IsNullOrEmpty(PCSNpdesField) Or _
               String.IsNullOrEmpty(PCSFacNameField) Or _
               String.IsNullOrEmpty(PCSSicField) Or _
               String.IsNullOrEmpty(PCSSicNameField) Or _
               String.IsNullOrEmpty(PCSCityField) Or _
               String.IsNullOrEmpty(PCSMajorField) Or _
               String.IsNullOrEmpty(PCSRecWaterField) Or _
               String.IsNullOrEmpty(PCSActiveField) Then
                WarningMsg("One or more layers or fields have not been specified.")
                Exit Sub
            End If

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(70, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Subbasin Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("NPDES", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Facility Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("SIC", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("SIC Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("City", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Major/Minor", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Waterbody", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)

            Dim lyrS As Integer = GisUtil.LayerIndex(SubbasinLayer)
            Dim fldS As Integer = GisUtil.FieldIndex(lyrS, SubbasinField)
            Dim lyr As Integer = GisUtil.LayerIndex(PCSLayer)
            Dim fldNPDES As Integer = GisUtil.FieldIndex(lyr, PCSNpdesField)
            Dim fldFacName As Integer = GisUtil.FieldIndex(lyr, PCSFacNameField)
            Dim fldSic As Integer = GisUtil.FieldIndex(lyr, PCSSicField)
            Dim fldSicName As Integer = GisUtil.FieldIndex(lyr, PCSSicNameField)
            Dim fldCity As Integer = GisUtil.FieldIndex(lyr, PCSCityField)
            Dim fldMajor As Integer = GisUtil.FieldIndex(lyr, PCSMajorField)
            Dim fldRecWater As Integer = GisUtil.FieldIndex(lyr, PCSRecWaterField)
            Dim fldActive As Integer = GisUtil.FieldIndex(lyr, PCSActiveField)

            Dim dictPCS As New Generic.SortedDictionary(Of String, Generic.SortedDictionary(Of String, clsPCS))

            For i As Integer = 0 To GisUtil.NumFeatures(lyrS) - 1
                Dim SubName As String = GisUtil.FieldValue(lyrS, i, fldS)
                If Not dictPCS.ContainsKey(SubName) Then dictPCS.Add(SubName, New Generic.SortedDictionary(Of String, clsPCS))
                For j As Integer = 0 To GisUtil.NumFeatures(lyr) - 1
                    If GisUtil.PointInPolygon(lyr, j, lyrS) = i Then
                        Dim NPDES As String = GisUtil.FieldValue(lyr, j, fldNPDES)
                        If Not dictPCS(SubName).ContainsKey(NPDES) Then dictPCS(SubName).Add(NPDES, New clsPCS(NPDES, GisUtil.FieldValue(lyr, j, fldFacName), GisUtil.FieldValue(lyr, j, fldSic), GisUtil.FieldValue(lyr, j, fldSicName), GisUtil.FieldValue(lyr, j, fldCity), GisUtil.FieldValue(lyr, j, fldMajor), GisUtil.FieldValue(lyr, j, fldRecWater), GisUtil.FieldValue(lyr, j, fldActive), PCSActiveOnly))
                    End If
                Next
                If Not WCSForm.UpdateProgress("Tabulating NPDES permits...", i, GisUtil.NumFeatures(lyrS) - 1) Then Exit Sub
            Next
            For Each kv As KeyValuePair(Of String, Generic.SortedDictionary(Of String, clsPCS)) In dictPCS
                For Each kv2 As KeyValuePair(Of String, clsPCS) In kv.Value
                    With kv2.Value
                        Dim PrintIt As Boolean = True
                        If PCSActiveOnly AndAlso Not (.Active.ToUpper.StartsWith("A") Or .Active.ToUpper.StartsWith("Y")) Then PrintIt = False
                        If PrintIt Then hb.AppendTableRow(kv.Key, .NPDES, .FacName, .Sic, .SicName, .City, .Major, .RecWater)
                    End With
                Next
            Next
            hb.AppendTableEnd()

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Status("")
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

    Private Class clsData
        Friend ID As String, Name As String, Parm As String, NumObs As Integer, MinValue As Single, MaxValue As Single, MeanValue As Single, MinDate As Date, MaxDate As Date
        Sub New(ByVal _ID As String, ByVal _Name As String, ByVal _Parm As String)
            ID = _ID
            Name = _Name
            Parm = _Parm
            NumObs = 0
            MinDate = Date.MaxValue
            MaxDate = Date.MinValue
            MinValue = Single.MaxValue
            MaxValue = Single.MinValue
            MeanValue = 0
        End Sub
        Sub Update(ByVal _NumObs As Integer, ByVal _MinValue As Single, ByVal _MaxValue As Single, ByVal _MeanValue As Single, ByVal _MinDate As Date, ByVal _MaxDate As Date)
            NumObs += _NumObs
            If _MinDate < MinDate Then MinDate = _MinDate
            If _MaxDate > MaxDate Then MaxDate = _MaxDate
            MinValue = Math.Min(MinValue, _MinValue)
            MaxValue = Math.Max(MaxValue, _MaxValue)
            MeanValue += _MeanValue * _NumObs
        End Sub
    End Class

    ''' <summary>
    ''' Create HTML table containing summary of time series data contained in previously downloaded datasets
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    Private Sub AppendDataTable(ByVal hb As HTMLBuilder.clsHTMLBuilder)
        Try
            GisUtil.Cancel = False
            Dim TableName As String = "Time Series Data Summary"

            If lstDatasets.Count = 0 Then
                WarningMsg("You must select one or more datasets to produce a data report.")
                Exit Sub
            End If

            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendTableColumn("Station ID", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Station Name", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("Parameter", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Left)
            hb.AppendTableColumn("No. Obs", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Min Value", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Max Value", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("Mean Value", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Right)
            hb.AppendTableColumn("First Date", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)
            hb.AppendTableColumn("Last Date", clsHTMLBuilder.enumAlign.Center, clsHTMLBuilder.enumAlign.Center)

            Dim dictData As New Generic.SortedDictionary(Of String, clsData)

            For Each dsName As String In lstDatasets
                For Each ds As atcData.atcDataSource In atcData.atcDataManager.DataSources
                    Dim dataSource As String = ds.Name.Split(":")(2) & ": " & ds.Specification
                    If dsName = dataSource Then
                        For i As Integer = 0 To ds.DataSets.Count - 1
                            With ds.DataSets(i).Attributes
                                Dim StaID As String = .GetValue("Location")
                                Dim Name As String = .GetValue("MonitoringLocationName")
                                If String.IsNullOrEmpty(Name) Then
                                    Name = .GetValue("StaNam") 'NWIS
                                Else
                                    Name = Name.ToString.Replace("`", "")
                                End If
                                Dim PCode As String = .GetValue("Constituent")
                                If Not dictData.ContainsKey(StaID & PCode) Then dictData.Add(StaID & PCode, New clsData(StaID, Name, PCode))
                                Dim StartDate As Date = #1/1/1900#
                                Dim EndDate As Date = #1/1/1900#
                                Dim StartDateDbl As Double = .GetValue("Start Date")
                                Dim EndDateDbl As Double = .GetValue("End Date")
                                If Not Double.IsNaN(StartDateDbl) Then StartDate = Date.FromOADate(StartDateDbl)
                                If Not Double.IsNaN(EndDateDbl) Then EndDate = Date.FromOADate(EndDateDbl)
                                dictData(StaID & PCode).Update(.GetValue("Count"), .GetValue("Min"), .GetValue("Max"), .GetValue("Mean"), StartDate, EndDate)
                            End With
                            If Not WCSForm.UpdateProgress("Summarizing data...", i, ds.DataSets.Count - 1) Then Exit Sub
                        Next
                    End If
                Next
            Next

            Dim lastID As String = ""
            For Each kv As KeyValuePair(Of String, clsData) In dictData
                With kv.Value
                    hb.AppendTableRow(IIf(.ID = lastID, "", .ID), IIf(.ID = lastID, "", .Name), .Parm, .NumObs, .MinValue.ToString("0.0000"), .MaxValue.ToString("0.0000"), (.MeanValue / .NumObs).ToString("0.0000"), .MinDate.ToString("MM/dd/yyyy"), .MaxDate.ToString("MM/dd/yyyy"))
                    lastID = .ID
                End With
            Next
            hb.AppendTableEnd()

        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            Logger.Status("")
            WCSForm.tblProgress.Visible = False
        End Try
    End Sub

#End Region
End Class
