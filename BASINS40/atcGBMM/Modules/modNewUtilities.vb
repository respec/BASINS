Module modNewUtilities
    Public NeedHgThiessen As Boolean
    '    Public pMin1 As New Scripting.Dictionary
    '    Public pMin2 As New Scripting.Dictionary
    '    Public pMin3 As New Scripting.Dictionary
    '    Public pMax1 As New Scripting.Dictionary
    '    Public pMax2 As New Scripting.Dictionary
    '    Public pMax3 As New Scripting.Dictionary

    '    Public gEnv As IEnvelope
    '    Public gActiveView As IActiveView
    '    Public gFeatureLayer As MapWindow.Interfaces.Layer
    '    Public gFeatureSelection As IFeatureSelection


    '    'Imports Text into Grid and defines DEM spatialreference
    '    Public Function ImportRaster(ByRef txtfileandpath As String, ByRef outpath As String, ByRef outname As String, ByRef pSR As ISpatialReference) As IRasterDataset

    '        On Error GoTo ErrorHandler

    '        Dim pWSF As IWorkspaceFactory
    '        pWSF = New RasterWorkspaceFactory
    '        Dim pOutWorkspace As IWorkspace
    '        pOutWorkspace = pWSF.OpenFromFile(outpath, 0)
    '        Dim pRasterImportOp As IRasterImportOp
    '        pRasterImportOp = New RasterConversionOp

    '        Dim pRasterDataSet As IRasterDataset
    '        pRasterDataSet = pRasterImportOp.ImportFromASCII(txtfileandpath, pOutWorkspace, outname, "GRID", False)

    '        Dim pGDSE, pds As IGeoDatasetSchemaEdit
    '        pGDSE = pRasterDataSet
    '        pds = pGDSE

    '        If pGDSE.CanAlterSpatialReference Then
    '            pGDSE.AlterSpatialReference(pSR)
    '        Else
    '            Err.Raise(vbObjectError + 5001, , "Cannot alter spatialref for : " & pds.name)
    '        End If

    '        GoTo CleanUp

    'ErrorHandler:
    '        bContinueWhAEMLink = False
    '        MsgBox("ImportRaster : " & Err.Description)
    'CleanUp:
    '        pWSF = Nothing
    '        pOutWorkspace = Nothing
    '        pRasterImportOp = Nothing
    '        pRasterDataSet = Nothing
    '        pGDSE = Nothing
    '        pds = Nothing
    '    End Function



    Public Sub CopyTextToTemp(ByRef strpath As String)

        Dim gridColl As Collection
        gridColl = New Collection

        gridColl.Add(InputDataDictionary("ClimateDataTextFile"))

        If InputDataDictionary("PSdataTable") <> "" Then
            gridColl.Add(InputDataDictionary("PSdataTable"))
        End If


        If InputDataDictionary("chkTime") Then
            gridColl.Add(InputDataDictionary("HgDryDepTimeSeries"))
            gridColl.Add(InputDataDictionary("HgWetDepTimeSeries"))
        End If

        Dim pTextFileName As String

        Dim counter As Short
        counter = 0
        For counter = 1 To gridColl.Count()
            pTextFileName = GetDataTableFileName(gridColl.Item(counter))
            If (pTextFileName = "") Then
                pTextFileName = gApplicationPath & "\data\" & gridColl.Item(counter)
            End If

            If My.Computer.FileSystem.FileExists(pTextFileName) Then
                My.Computer.FileSystem.CopyFile(pTextFileName, strpath & gridColl.Item(counter))
            End If
        Next
        gridColl = Nothing
    End Sub

    Public Sub Convertascii2(ByRef strpath As String)

        Dim gridColl As Collection
        gridColl = New Collection

        gridColl.Add(InputDataDictionary("SoilMap"))
        gridColl.Add(InputDataDictionary("Landuse"))

        Dim counter As Short
        counter = 0
        For counter = 1 To gridColl.Count()
            ExportGrid(gMapTempFolder & "\SUB\", gridColl.Item(counter), strpath & gridColl.Item(counter) & ".asc")
        Next
        gridColl = Nothing
    End Sub

    'Converts Rasters in the collection into Ascii/Float
    Public Sub ConvertAscii(ByRef strpath As String)
        Dim gridColl As Collection
        gridColl = New Collection

        gridColl.Add("thiessenwtr")
        gridColl.Add("subwatershed")
        gridColl.Add("cn2")
        gridColl.Add("cnimp")
        gridColl.Add("totaltime")
        gridColl.Add("streamtime")
        gridColl.Add("ovlength")
        gridColl.Add("avroughness")
        gridColl.Add("avslope")


        'soilwater

        If InputDataDictionary("PointSources") <> "" Then
            gridColl.Add("PointSources")
        End If

        If InputDataDictionary("optInputSoilMoisture") = True And InputDataDictionary("InitialSoilMoisture") <> "" Then
            gridColl.Add(InputDataDictionary("InitialSoilMoisture"))
        End If


        If InputDataDictionary("chkGrid") = 1 Then
            If InputDataDictionary("HgDryGrid") <> "" Then gridColl.Add(InputDataDictionary("HgDryGrid"))
            If InputDataDictionary("HgWetGrid") <> "" Then gridColl.Add(InputDataDictionary("HgWetGrid"))
        End If

        If InputDataDictionary("chkTime") = 1 Then
            gridColl.Add("thiessenhg")
        End If


        If InputDataDictionary("cbxSediment") = 1 Then
            gridColl.Add("lsfactor")
        End If

        If InputDataDictionary("cbxMercury") = 1 Then
            gridColl.Add("kdcomp")
        End If

        If InputDataDictionary("optionSoilHgGrid") = True And InputDataDictionary("InitialSoilHg") <> "" Then
            gridColl.Add(InputDataDictionary("InitialSoilHg"))
        End If

        Dim counter As Short
        counter = 0
        For counter = 1 To gridColl.Count()
            ExportGrid(gMapTempFolder & "\SUB\", gridColl.Item(counter), strpath & gridColl.Item(counter) & ".asc")
        Next

        gridColl = Nothing
    End Sub

    'Exports Grid into Float Text
    Public Sub ExportGrid(ByVal InFilePath As String, ByVal InFile As String, ByVal OutFileAndPath As String)
        Dim pRasterExportOp As IRasterExportOp
        pRasterExportOp = New RasterConversionOp
        'Get raster
        Dim pRas01 As IRaster
        pRas01 = OpenRasterDatasetFromDisk2(InFilePath, InFile)
        If Not pRas01 Is Nothing Then
            If My.Computer.FileSystem.FileExists(OutFileAndPath) Then My.Computer.FileSystem.DeleteFile(OutFileAndPath)
            ' call the export method
            pRasterExportOp.ExportToASCII(pRas01, OutFileAndPath)
        End If
    End Sub

    'Function to open raster dataset and read the raster from it
    Public Function OpenRasterDatasetFromDisk2(ByRef filepath As String, ByRef pRasterName As String) As MapWinGIS.Grid
        ' check if raster dataset exist
        If Not My.Computer.FileSystem.DirectoryExists(filepath & "\" & pRasterName) Then Return Nothing

        Dim g As New MapWinGIS.Grid
        g.Open(filepath & "\" & pRasterName, , True)
        Return g
    End Function

    'Delete all the rasterfiles in the collection
    Public Sub DeleteRasterFile(ByRef coll As Collection)
        Dim i As Short
        If Not coll Is Nothing Then

            For i = 1 To coll.Count()
                DeleteRasterDataset(coll.Item(i))
            Next

        End If
    End Sub

    '    Public Sub GetHgStation()
    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")
    '        'If input file is present, Create a dictionary for input data layer values
    '        FileOpen(200, gMapInputFolder & "\InputData.txt", OpenMode.Input) ' Open file for input.
    '        Dim ControlName As String
    '        Dim ControlValue As String
    '        Do While Not EOF(200)
    '            Input(200, ControlName)
    '            Input(200, ControlValue)
    '            If ControlName = "HgStation" Then
    '                MercuryStation = ControlValue
    '            End If
    '        Loop
    '        FileClose(200) ' Close file.
    '        fso = Nothing
    '    End Sub

    '    Public Sub CreateInputBackup()
    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")
    '        fso.CopyFile(gMapInputFolder & "\InputData.txt", gMapInputFolder & "\InputDataBkup.txt", True)
    '        fso = Nothing
    '    End Sub

    '    '*** Create InputData Dictionary
    '    Public Sub CreateInputDataDictionary(ByRef strfile As String, ByRef strdict As Scripting.Dictionary)
    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")
    '        If Not (fso.FileExists(gMapInputFolder & "\" & strfile)) Then
    '            Exit Sub
    '        End If

    '        'Create a dictionary for input data layer values
    '        ' Set strdict = CreateObject("Scripting.Dictionary")
    '        FileOpen(200, gMapInputFolder & "\" & strfile, OpenMode.Input) ' Open file for input.
    '        Dim keyDataType As String
    '        Dim ValueDataType As String
    '        Do While Not EOF(200)
    '            Input(200, keyDataType)
    '            Input(200, ValueDataType)
    '            strdict.Add(keyDataType, ValueDataType)
    '        Loop
    '        FileClose(200) ' Close file.
    '        fso = Nothing
    '    End Sub

    '    'Deletes the list of rasters when their corresponding field is modified in inputdata.txt
    '    Public Sub DeleteModified()
    '        Dim inputdict As Scripting.Dictionary
    '        Dim inputbkdict As Scripting.Dictionary
    '        Dim fielddict As Scripting.Dictionary

    '        inputdict = New Scripting.Dictionary
    '        inputbkdict = New Scripting.Dictionary
    '        fielddict = New Scripting.Dictionary

    '        ModuleNewUtilities.CreateInputDataDictionary("inputdata.txt", inputdict)
    '        ModuleNewUtilities.CreateInputDataDictionary("inputdatabkup.txt", inputbkdict)

    '        fielddict.Add("cbxStandard", "AWC")
    '        fielddict.Add("cbxSediment", "cn1")
    '        fielddict.Add("cbxMercury", "cn2")

    '        Dim ctr As Short

    '        For ctr = 0 To fielddict.Count - 1
    '            If inputdict.Item(fielddict.keys(ctr)) <> inputbkdict.Item(fielddict.keys(ctr)) Then
    '                DeleteRasterDataset((fielddict.Items(ctr)))
    '            End If
    '        Next
    '    End Sub


    '    'Extract subwatershed area
    '    Public Sub ExtractWatershedInfo()

    '        Dim pRasterBnd As IRasterBand
    '        Dim pBC As IRasterBandCollection
    '        Dim pWatershedRaster As IRaster

    '        Dim pFeatCls As IFeatureClass
    '        Dim rascoll As Collection
    '        Dim RasCountColl As Collection

    '        Dim pWSRasterLayer As IRasterLayer
    '        pWSRasterLayer = GetInputRasterLayer("Subwatershed")

    '        Dim fileAndpath As String
    '        Dim filename As String
    '        Dim pathname As String

    '        fileAndpath = pWSRasterLayer.filepath
    '        filename = Mid(pWSRasterLayer.filepath, InStrRev(pWSRasterLayer.filepath, "\") + 1, Len(pWSRasterLayer.filepath))
    '        pathname = Mid(pWSRasterLayer.filepath, 1, InStrRev(pWSRasterLayer.filepath, "\") - 1)


    '        pWatershedRaster = OpenRasterDatasetFromDisk2(pathname, filename)
    '        Dim NumOfValues As Short
    '        Dim i As Short
    '        Dim pRow As iRow
    '        Dim pTable As iTable
    '        If Not (pWatershedRaster Is Nothing) Then

    '            pBC = pWatershedRaster
    '            pRasterBnd = pBC.Item(0)

    '            ' Attribute table
    '            pTable = pRasterBnd.AttributeTable
    '            NumOfValues = pTable.RowCount(Nothing)

    '            rascoll = New Collection
    '            RasCountColl = New Collection

    '            If Not pTable Is Nothing Then
    '                For i = 0 To NumOfValues - 1
    '                    pRow = pTable.GetRow(i) 'Get a row from the table
    '                    rascoll.Add(pRow.Value(pTable.FindField("Value")))
    '                    RasCountColl.Add(pRow.Value(pTable.FindField("Count")))
    '                Next i
    '            End If
    '        End If

    '        '    Dim tm As Integer
    '        '    For tm = 1 To RasColl.Count
    '        '        MsgBox "Value " & RasColl.Count & "  : Count " & RasCountColl.Item(1)
    '        '    Next

    '        AppendClimateFile(rascoll, RasCountColl)

    '    End Sub

    '    Public Sub AppendClimateFile(ByRef rasColl1 As Collection, ByRef rasColl2 As Collection)
    '        FileOpen(1, gMapInputFolder & "\ClimateData.txt", OpenMode.Append) ' Open file for output.
    '        Dim i As Short
    '        PrintLine(1, "Number of Watersheds : " & rasColl1.Count())
    '        For i = 1 To rasColl1.Count()
    '            Print(1, "Area (in m2) : " & rasColl1.Item(i) & "," & rasColl2.Item(i) * gCellSize * gCellSize)
    '        Next
    '        FileClose(1) ' Close file.
    '    End Sub

    '    Public Function CreatePointRaster() As Object
    '        On Error GoTo ShowError
    '        Dim pRasterPoint As IRaster
    '        Dim pFeatureLayer As MapWindow.Interfaces.Layer
    '        'Set pFeatureLayer = GetInputLayer("ClimateStation")
    '        pFeatureLayer = GetInputLayer("assesspoint1")

    '        Dim pRasterPointSource As IRasterDataset
    '        Dim pFeatureClass As IFeatureClass
    '        If Not (pFeatureLayer Is Nothing) Then
    '            MsgBox("Creating point raster...")
    '            pFeatureClass = pFeatureLayer.FeatureClass


    '            pRasterPoint = pRasterPointSource.CreateDefaultRaster
    '            MsgBox("created point raster...")
    '        End If
    '        GoTo CleanUp

    'ShowError:
    '        MsgBox("CreatePointSource Raster: " & Err.Description & "  " & Err.Number)
    'CleanUp:
    '        pFeatureLayer = Nothing
    '        pFeatureClass = Nothing
    '        pRasterPointSource = Nothing
    '        pRasterPoint = Nothing

    '    End Function


    '    Public Sub ActivateSelectTool()
    '        ' Add some code to do some action on double-click.
    '        ' This example makes the builtin Select Graphics Tool the active tool.
    '        Dim pSelectTool As ICommandItem
    '        Dim pCommandBars As ICommandBars
    '        ' The identifier for the Select Graphics Tool
    '        Dim u As New UID
    '        u = "{C22579D1-BC17-11D0-8667-0000F8751720}"
    '        'Find the Select Graphics Tool
    '        pCommandBars = gApplication.Document.CommandBars
    '        pSelectTool = pCommandBars.Find(u)
    '        'Set the current tool of the application to be the Select Graphics Tool
    '        gApplication.CurrentTool = pSelectTool
    '    End Sub

    '    Private Function CopyDataRasters(ByRef tmpstr As String) As IRaster
    '        Dim ptmpRasLayer As IRasterLayer
    '        ptmpRasLayer = GetInputRasterLayer(tmpstr)
    '        Dim pTmpRaster As IRaster
    '        pTmpRaster = ptmpRasLayer.Raster

    '        Dim pSWSRaster As IRaster
    '        pSWSRaster = OpenRasterDatasetFromDisk("Subwatershed")

    '        If Not pSWSRaster Is Nothing Then
    '            If Not pTmpRaster Is Nothing Then
    '                gAlgebraOp.BindRaster(pSWSRaster, "SWS")
    '                gAlgebraOp.BindRaster(pTmpRaster, "TMP")

    '                pTmpRaster = gAlgebraOp.Execute("([SWS]-[SWS])+[TMP]")

    '                gAlgebraOp.UnbindRaster("SWS")
    '                gAlgebraOp.UnbindRaster("TMP")
    '            End If
    '        End If
    '        CopyDataRasters = pTmpRaster
    '        GoTo CleanUp

    'ShowError:
    '        MsgBox("Copying Data Rasters to Temp folder with modified extent: " & Err.Description & " " & Err.Number & " Filename : " & tmpstr)

    'CleanUp:
    '        ptmpRasLayer = Nothing
    '        pTmpRaster = Nothing
    '        pSWSRaster = Nothing

    '    End Function

    '    Public Sub CopyRastersToTemp()

    '        Dim pSWSRasLayer As IRasterLayer
    '        pSWSRasLayer = GetInputRasterLayer("Flowdir")

    '        Dim pRWSF As IWorkspaceFactory
    '        pRWSF = New RasterWorkspaceFactory
    '        Dim pRWS As IRasterWorkspace2
    '        pRWS = pRWSF.OpenFromFile(gApplicationPath & "\DATA", 0)
    '        Dim pRWSTemp As IRasterWorkspace2
    '        pRWSTemp = pRWSF.OpenFromFile(gApplicationPath & "\TEMP", 0)

    '        Dim pEnvelope As IEnvelope
    '        'Set pEnvelope = GetRasterValidExtent(pSWSRasLayer.Raster, pRWSTemp)

    '        '    Dim pWSRasterProps As IRasterProps
    '        '    Set pWSRasterProps = pSWSRasLayer.Raster
    '        '
    '        '    Set pEnvelope = pWSRasterProps.Extent






    '        '    CopyDataRasters (InputDataDictionary(""))
    '        '    CopyDataRasters (InputDataDictionary(""))
    '        '    CopyDataRasters (InputDataDictionary(""))
    '        '    CopyDataRasters (InputDataDictionary(""))
    '        '    CopyDataRasters (InputDataDictionary(""))

    '    End Sub

    '    'Write DryHg summary

    '    Public Sub HgSummary(ByRef strfile As String, ByRef outfile As String)
    '        Dim fso As New Scripting.FileSystemObject

    '        ''    If Not (fso.FileExists(gApplicationPath & "\Data\" & strfile)) Then
    '        ''        Exit Sub
    '        ''    End If

    '        Dim pTextFileName As String
    '        pTextFileName = GetDataTableFileName(strfile)

    '        If (pTextFileName = "") Then
    '            pTextFileName = gApplicationPath & "\Data\" & strfile
    '        End If

    '        If Not (fso.FileExists(pTextFileName)) Then
    '            Exit Sub
    '        End If

    '        'Open gApplicationPath & "\Data\" & strfile For Input As #200    ' Open file for input.
    '        FileOpen(200, pTextFileName, OpenMode.Input) ' Open file for input.

    '        Dim TextLine As String
    '        Dim tmp() As String
    '        Dim ctr As Short

    '        Dim pMin As New Scripting.Dictionary
    '        Dim pMax As New Scripting.Dictionary
    '        Dim tmpID As String
    '        Dim pMinDate As Date
    '        Dim pMaxDate As Date

    '        TextLine = LineInput(200)

    '        Do While Not EOF(200)
    '            TextLine = LineInput(200) ' Read line into variable.
    '            tmp = Split(TextLine, ",") ' read the line into arrays

    '            'For the first record
    '            If tmpID = "" Then
    '                pMinDate = CDate(tmp(0))
    '                pMaxDate = CDate(tmp(0))
    '                tmpID = tmp(1)
    '                GoTo NextStep
    '            End If

    '            If tmp(1) = tmpID Then
    '                If CDate(tmp(0)) < pMinDate Then
    '                    pMinDate = CDate(tmp(0))
    '                End If
    '                If pMaxDate < CDate(tmp(0)) Then
    '                    pMaxDate = CDate(tmp(0))
    '                End If
    '            Else
    '                pMin.Add(tmpID, pMinDate)
    '                pMax.Add(tmpID, pMaxDate)
    '                pMinDate = CDate(tmp(0))
    '                pMaxDate = CDate(tmp(0))
    '                tmpID = tmp(1)
    '            End If

    'NextStep:
    '        Loop

    '        pMin.Add(tmpID, pMinDate)
    '        pMax.Add(tmpID, pMaxDate)

    '        FileClose(200) ' Close file.

    '        FileOpen(1, gMapInputFolder & "\" & outfile, OpenMode.Output) ' Open file for output.
    '        WriteLine(1, "StationID", "MinDate", "MaxDate")

    '        Dim icnt As Short
    '        For icnt = 0 To pMin.Count - 1
    '            WriteLine(1, pMin.keys(icnt), CStr(pMin.Item(pMin.keys(icnt))), CStr(pMax.Item(pMax.keys(icnt))))
    '        Next
    '        FileClose(1)

    'CleanUp:
    '        fso = Nothing
    '        pMin = Nothing
    '        pMax = Nothing

    '    End Sub

    '    'Write Climate Summary
    '    Public Sub FindClimateMinMax(ByRef strfile As String)

    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")

    '        '    Dim pMapInputfolder As String
    '        '    pMapInputfolder = "D:\Projects\Lakes\alvitest\DATA"

    '        Dim pTextFileName As String
    '        pTextFileName = GetDataTableFileName(strfile)
    '        If (pTextFileName = "") Then
    '            pTextFileName = gApplicationPath & "\data\" & strfile
    '        End If

    '        ''    If Not (fso.FileExists(gApplicationPath & "\data\" & strfile)) Then
    '        ''        Exit Sub
    '        ''    End If
    '        If Not (fso.FileExists(pTextFileName)) Then Exit Sub

    '        FileOpen(200, pTextFileName, OpenMode.Input) ' Open file for input.

    '        Dim TextLine As String
    '        Dim tmp() As String
    '        Dim ctr As Short

    '        Dim pMin As New Scripting.Dictionary
    '        Dim pMax As New Scripting.Dictionary
    '        Dim tmpID As String
    '        Dim pMinDate As Date
    '        Dim pMaxDate As Date
    '        Dim tempDate As Date

    '        TextLine = LineInput(200)
    '        Do While Not EOF(200)
    '            TextLine = LineInput(200) ' Read line into variable.
    '            tmp = Split(TextLine, ",") ' read the line into arrays

    '            tempDate = CDate(tmp(0))

    '            'For the first record
    '            If tmpID = "" Then
    '                pMinDate = tempDate
    '                pMaxDate = tempDate
    '                tmpID = Replace(tmp(1), Chr(34), "") 'Remove quotes
    '                GoTo NextStep
    '            End If

    '            If Replace(tmp(1), Chr(34), "") = tmpID Then
    '                If tempDate < pMinDate Then
    '                    pMinDate = tempDate
    '                End If
    '                If pMaxDate < tempDate Then
    '                    pMaxDate = tempDate
    '                End If
    '            Else
    '                pMin.Add(tmpID, pMinDate)
    '                pMax.Add(tmpID, pMaxDate)
    '                pMinDate = tempDate
    '                pMaxDate = tempDate
    '                tmpID = Replace(tmp(1), Chr(34), "")
    '            End If

    'NextStep:
    '        Loop

    '        pMin.Add(tmpID, pMinDate)
    '        pMax.Add(tmpID, pMaxDate)

    '        FileClose(200) ' Close file.

    '        FileOpen(1, gMapInputFolder & "\ClimateSummary.txt", OpenMode.Output) ' Open file for output.

    '        WriteLine(1, "StationID", "MinDate", "MaxDate")

    '        Dim icnt As Short
    '        For icnt = 0 To pMin.Count - 1
    '            WriteLine(1, pMin.keys(icnt), CStr(pMin.Item(pMin.keys(icnt))), CStr(pMax.Item(pMax.keys(icnt))))
    '        Next
    '        FileClose(1)

    'CleanUp:
    '        fso = Nothing
    '        pMin = Nothing
    '        pMax = Nothing

    '    End Sub

    Public Function FindRange(ByVal tmpDict As Generic.Dictionary(Of String, String), ByVal filestr As String) As String

        If Not My.Computer.FileSystem.FileExists(gMapInputFolder & "\" & filestr) Then
            WarningMsg("{0}\{1} does not exist.", gMapInputFolder, filestr)
            Exit Function
        End If

        Dim sr As New IO.StreamReader(gMapInputFolder & "\" & filestr)

        'Dim ctr As Short
        'Dim tmpID As String
        'Dim pCurrMin As Date
        'Dim pCurrMax As Date
        'Dim StaID As String
        'Dim MinValue As String
        'Dim MaxValue As String
        'Dim minDict As New Scripting.Dictionary
        'Dim maxDict As New Scripting.Dictionary

        Dim minDict As New Generic.Dictionary(Of String, String)
        Dim maxDict As New Generic.Dictionary(Of String, String)

        'skip first line
        sr.ReadLine()
        While Not sr.EndOfStream
            Dim ar() As String = sr.ReadLine.Split(vbTab)
            Dim StaID As String = ar(0)
            Dim MinValue As String = ar(1)
            Dim MaxValue As String = ar(2)
            minDict.Add(StaID, MinValue)
            maxDict.Add(StaID, MaxValue)
        End While
        sr.Close()
        sr.Dispose()

        Dim MinDate As Date = Date.MaxValue
        Dim MaxDate As Date = Date.MinValue

        For Each s As String In tmpDict.Keys
            Dim d As Date
            If minDict.TryGetValue(s, d) Then
                If d.CompareTo(MinDate) < 0 Then MinDate = d
                If d.CompareTo(MaxDate) > 0 Then MaxDate = d
            Else
                WarningMsg("Invalid climate shapefile and/or textfile")
                MinDate = Now
                MaxDate = Now
                Exit For
            End If
        Next
        Return String.Format("{0:MM/dd/yyyy}-{1:MM/dd/yyyy}", MinDate, MaxDate)
    End Function

    ''' <summary>
    ''' Subroutine to read values from input file and load DataManagement form
    ''' </summary>
    Public Sub ReadDataManagementUserInputsFromFile2(ByVal frm As Form, Optional ByVal Selected As String = "")
        DefineApplicationPath()
        If TypeOf frm Is frmHydrology Then
            SetDefaultLayerOrTable(CType(frm, frmHydrology).cboInitial_Soil_Moisture)
        ElseIf TypeOf frm Is frmMercuryInput Then
            With CType(frm, frmMercuryInput)
                SetDefaultLayerOrTable(.cboInitialSoilHg)
                SetDefaultLayerOrTable(.cboHg_Dry_Deposition_Flux)
                SetDefaultLayerOrTable(.cboHg_Wet_Deposition_Flux)
                SetDefaultLayerOrTable(.cboHg_Stations)
                SetDefaultLayerOrTable(.cboHg_Dry_Deposition_Time_Series)
                SetDefaultLayerOrTable(.cboHg_Wet_Deposition_Time_Series)
            End With
        End If
        InitializeDataManagementForm2(frm, Selected)
    End Sub


    ''' <summary>
    ''' Set default
    ''' </summary>
    ''' <param name="pControl">Combobox control containing layers or tables</param>
    ''' <remarks></remarks>
    Public Sub SetDefaultLayerOrTable(ByRef pControl As System.Windows.Forms.ComboBox)
        'Read all the layer names and load default layers
        Dim pDataName As String = pControl.Text
        pControl.Text = ""

        For i As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerName(i) = pDataName Then
                pControl.Text = pDataName
                Exit Sub
            End If
        Next

        For i As Integer = 0 To gDataset.Tables.Count - 1
            If gDataset.Tables(i).TableName = pDataName Then
                pControl.Text = pDataName
                Exit Sub
            End If
        Next
    End Sub

    Public Sub InitializeDataManagementForm2(ByVal frm As Form, Optional ByVal Selected As String = "")

        Dim FeatureLayerNames As Generic.List(Of String) = Nothing
        Dim RasterLayerNames As Generic.List(Of String) = Nothing
        Dim TableNames As Generic.List(Of String) = Nothing
        GetNameLists(FeatureLayerNames, RasterLayerNames, TableNames)

        If TypeOf frm Is frmHydrology Then
            With CType(frm, frmHydrology)
                LoadDefaultLayerNames(.cboInitial_Soil_Moisture, "InitialSoilMoisture", RasterLayerNames.ToArray)
            End With
        ElseIf TypeOf frm Is frmMercuryInput Then
            With CType(frm, frmMercuryInput)
                LoadDefaultLayerNames(.cboInitialSoilHg, "InitialSoilHg", RasterLayerNames.ToArray)

                LoadDefaultLayerNames(.cboHg_Dry_Deposition_Flux, "HgDryGrid", RasterLayerNames.ToArray)
                LoadDefaultLayerNames(.cboHg_Wet_Deposition_Flux, "HgWetGrid", RasterLayerNames.ToArray)

                LoadDefaultLayerNames(.cboHg_Stations, "HgStation", FeatureLayerNames.ToArray)
                LoadDefaultLayerNames(.cboHg_Dry_Deposition_Time_Series, "HgDryDepTimeSeries", TableNames.ToArray)
                LoadDefaultLayerNames(.cboHg_Wet_Deposition_Time_Series, "HgWetDepTimeSeries", TableNames.ToArray)
            End With
        End If

    End Sub

    '*** Subroutine to load default names to list box controls
    Public Sub LoadDefaultLayerNames(ByRef pControl As System.Windows.Forms.ComboBox, ByRef datasetname As String, ByRef LayerNames() As String)
        Dim i As Short
        If LayerNames.Length = 0 Then
            pControl.Items.Clear()
            pControl.Text = ""
        Else
            For i = 0 To LayerNames.Length - 1
                pControl.Items.Add(LayerNames(i))
                If (Replace(UCase(LayerNames(i)), " ", "") = Replace(UCase(datasetname), " ", "")) Then
                    pControl.SelectedIndex = i - 1
                End If
            Next
        End If
    End Sub



    ''Selects Streams within the envelope
    'Public Function SelectStreamFeatures(ByRef streamEnvelope As IEnvelope, ByRef streamLayer As MapWindow.Interfaces.Layer, ByRef pActiveView As IActiveView) As Object

    '    Dim pSpatialFilter As ISpatialFilter
    '    pSpatialFilter = New SpatialFilter

    '    gFeatureSelection = streamLayer

    '    pSpatialFilter.Geometry = streamEnvelope
    '    '    pSpatialFilter.SpatialRel = esriSpatialRelIntersects

    '    'Merge Selected Features spatially
    '    '    gFeatureSelection.SelectFeatures pSpatialFilter, esriSelectionResultNew, False

    '    ' Refresh the selections
    '    '     gActiveView.PartialRefresh esriViewGeoSelection, Nothing, Nothing

    '    Dim pSelectionSet As ISelectionSet
    '    pSelectionSet = gFeatureSelection.SelectionSet
    '    Dim pFeatureCursor As IFeatureCursor
    '    pSelectionSet.Search(Nothing, True, pFeatureCursor)

    '    Dim pFeature As IFeature
    '    pFeature = pFeatureCursor.NextFeature

    '    Dim levelFld As Integer
    '    levelFld = pFeatureCursor.FindField("Level")

    '    Dim pLevel As Integer
    '    Dim levelColl As New Collection

    '    If FrmStreamLevel.cbxLevel.Items.Count > 0 Then Exit Function

    '    Do While Not pFeature Is Nothing
    '        pLevel = pFeature.Value(levelFld)
    '        If Not ModuleWASPOutput.ExistsInCollection(levelColl, pLevel, "No") Then
    '            levelColl.Add(pLevel)
    '            FrmStreamLevel.cbxLevel.Items.Add(CStr(pLevel))
    '        End If
    '        pFeature = pFeatureCursor.NextFeature
    '    Loop



    'End Function


    ''Cleanup Temp folders

    'Public Sub CleanTempRasters()

    '    Dim SafeColl As New Collection
    '    Dim FoldersColl As New Collection

    '    SafeColl.Add(gMapTempFolder & "\burn_dem")
    '    SafeColl.Add(gMapTempFolder & "\cn2")
    '    SafeColl.Add(gMapTempFolder & "\cnimp")
    '    SafeColl.Add(gMapTempFolder & "\csl")
    '    SafeColl.Add(gMapTempFolder & "\filldem")
    '    SafeColl.Add(gMapTempFolder & "\fillrawdem")
    '    SafeColl.Add(gMapTempFolder & "\flowaccu")
    '    SafeColl.Add(gMapTempFolder & "\flowdir")
    '    SafeColl.Add(gMapTempFolder & "\kdcomp")
    '    SafeColl.Add(gMapTempFolder & "\lsfactor")
    '    SafeColl.Add(gMapTempFolder & "\streamtime")
    '    SafeColl.Add(gMapTempFolder & "\totaltime")
    '    SafeColl.Add(gMapTempFolder & "\thiessenwtr")
    '    SafeColl.Add(gMapTempFolder & "\thiesssub")
    '    SafeColl.Add(gMapTempFolder & "\subwatershed")
    '    SafeColl.Add(gMapTempFolder & "\lakegrid")
    '    SafeColl.Add(gMapTempFolder & "\pointsources")
    '    SafeColl.Add(gMapTempFolder & "\thiessenhg")
    '    SafeColl.Add(gMapTempFolder & "\hydradius")
    '    SafeColl.Add(gMapTempFolder & "\swstime")
    '    SafeColl.Add(gMapTempFolder & "\info")
    '    SafeColl.Add(gMapTempFolder & "\slope")
    '    SafeColl.Add(gMapTempFolder & "\roughness")
    '    SafeColl.Add(gMapTempFolder & "\overlandtt")
    '    SafeColl.Add(gMapTempFolder & "\flowlength")
    '    SafeColl.Add(gMapTempFolder & "\focalflow")
    '    SafeColl.Add(gMapTempFolder & "\highpoint")
    '    SafeColl.Add(gMapTempFolder & "\avslope")
    '    SafeColl.Add(gMapTempFolder & "\ovlength")
    '    SafeColl.Add(gMapTempFolder & "\avroughness")


    '    Dim next_dir As Short
    '    Dim dir_name As String
    '    Dim sub_dir As String
    '    Dim i As Short

    '    next_dir = 1
    '    FoldersColl = Nothing
    '    FoldersColl.Add(gMapTempFolder) ' Start here.
    '    Do While next_dir <= FoldersColl.Count()
    '        ' Get the next directory to search.
    '        dir_name = FoldersColl.Item(next_dir)
    '        next_dir = next_dir + 1

    '        ' Read directories from dir_name.
    '        sub_dir = Dir(dir_name & "\*", FileAttribute.Directory)
    '        Do While sub_dir <> ""
    '            ' Add the name to the list if
    '            ' it is a directory.
    '            If UCase(sub_dir) <> "PAGEFILE.SYS" And sub_dir <> "." And sub_dir <> ".." Then
    '                sub_dir = dir_name & "\" & sub_dir
    '                On Error Resume Next
    '                If GetAttr(sub_dir) And FileAttribute.Directory Then FoldersColl.Add(sub_dir)
    '            End If
    '            sub_dir = Dir(, FileAttribute.Directory)
    '        Loop
    '    Loop

    '    Dim filename As String

    '    For i = 1 To FoldersColl.Count()
    '        If Len(FoldersColl.Item(i)) > Len(gMapTempFolder) Then
    '            filename = Mid(FoldersColl.Item(i), InStrRev(FoldersColl.Item(i), "\") + 1, Len(FoldersColl.Item(i)))
    '            If Not ModuleWASPOutput.ExistsInCollection(SafeColl, FoldersColl.Item(i), "No") And (UCase(filename) <> "SUB" Or UCase(filename) <> "INFO") Then
    '                Call DeleteRasterDataset(FoldersColl.Item(i), True)
    '            End If
    '        End If
    '    Next i

    'End Sub

    ''Clean temp folder shapefiles
    'Public Sub CleanTempShapes()
    '    Dim safeShpColl As New Collection

    '    safeShpColl.Add(gMapTempFolder & "\assesspoints")
    '    safeShpColl.Add(gMapTempFolder & "\branches")
    '    safeShpColl.Add(gMapTempFolder & "\snapassesspoints")
    '    safeShpColl.Add(gMapTempFolder & "\Subwatersheds")


    '    Dim next_dir As Short
    '    Dim dir_name As String
    '    Dim sub_dir As String
    '    Dim i As Short
    '    Dim FoldersColl As New Collection

    '    next_dir = 1
    '    FoldersColl = Nothing
    '    FoldersColl.Add(gMapTempFolder) ' Start here.
    '    Do While next_dir <= FoldersColl.Count()
    '        ' Get the next directory to search.
    '        dir_name = FoldersColl.Item(next_dir)
    '        next_dir = next_dir + 1

    '        ' Read directories from dir_name.
    '        sub_dir = Dir(dir_name & "\*.shp", FileAttribute.Normal)
    '        Do While sub_dir <> ""
    '            ' Add the name to the list if
    '            ' it is a directory.
    '            If UCase(sub_dir) <> "PAGEFILE.SYS" And sub_dir <> "." And sub_dir <> ".." Then
    '                sub_dir = dir_name & "\" & sub_dir
    '                On Error Resume Next
    '                If GetAttr(sub_dir) Then FoldersColl.Add(Replace(sub_dir, ".shp", ""))
    '            End If
    '            sub_dir = Dir(, FileAttribute.Directory)
    '        Loop
    '    Loop

    '    i = 1
    '    For i = 1 To FoldersColl.Count()
    '        If Len(FoldersColl.Item(i)) > Len(gMapTempFolder) Then
    '            If Not ModuleWASPOutput.ExistsInCollection(safeShpColl, FoldersColl.Item(i), "No") Then
    '                Call DeleteFeatureDataset(FoldersColl.Item(i), True)
    '            End If
    '        End If
    '    Next

    'End Sub
End Module