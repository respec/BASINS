Module modCreateInputFile
    '******************************************************************************
    '   Application: GBMM
    '   Company:     Tetra Tech, Inc
    '   File Name:   ModuleCreateInputFile
    '   Purpose:     This module contains all functions and utilities to write the input
    '                text file.
    '
    '   Designer:    Ting Dai, Khalid Alvi, Jenny Zhen
    '   Developer:   Ragothaman Bhimarao, Sabu Paul
    '   History:     Created:
    '                Modified: 10/04/2005 - Ragothaman Bhimarao
    '
    '******************************************************************************


    'Define all private variables

    Private sw As IO.StreamWriter

    Public IfError As Boolean 'set to true when there is any error
    Public messWriteError As String 'Set the error message

    Public pCellSize As Double
    Public pCellArea As Double
    Public lakeCSstr As String
    Public LakeCount As Short
    Public LakeInfo As String
    Public PointSourceInfo As String

    Public Const Blank As String = ("c---------------------------------------------------------------------------------------------------------------------")

    '******************************************************************************
    'Subroutine: WriteInputTextFile
    'Author:     Ragothaman Bhimarao
    'Purpose:    Main subroutine to write the output file. Asks the user about
    '            the input file name. Calls subroutines to write each card.
    '******************************************************************************
    Public Function WriteInputTextFile() As Boolean
        IfError = False

        InitializeInputDataDictionary()

        Dim pInputFilePath As String = InputDataDictionary("DataFolderPath")

        If Not pInputFilePath.EndsWith("\") Then pInputFilePath &= "\"

        Dim pInputFileName As String = pInputFilePath & InputDataDictionary.Item("InputFileName") & ".inp"
        sw = New IO.StreamWriter(pInputFileName, False)

        'Call subroutines to write card data to input file


        WriteCardHeader()

        WriteCard100()
        WriteCard110()
        WriteCard120()
        WriteCard130()

        WriteCard140()

        WriteCard150()

        If IfError Then GoTo ShowError

        If InputDataDictionary("cbxSediment") Then
            WriteCard160()
            If IfError Then GoTo ShowError
        End If
        If InputDataDictionary("cbxMercury") Then
            WriteCard170()
            If IfError Then GoTo ShowError
        End If

        If InputDataDictionary("Lakes") = "" Then
            LakeCount = 0
        Else
            LakeInfo = GetLakeInfo()
        End If
        If IfError Then GoTo ShowError

        WriteCard200()
        If IfError Then GoTo ShowError

        WriteCard210()
        If IfError Then GoTo ShowError

        WriteCard220()
        If IfError Then GoTo ShowError

        WriteCard230()
        If IfError Then GoTo ShowError

        WriteCard240()
        If IfError Then GoTo ShowError

        If InputDataDictionary("chkTime") Then
            WriteCard250()
            If IfError Then GoTo ShowError
        End If
        If InputDataDictionary("PointSources") <> "" Then
            PointSourceInfo = ExtractPointSourceInfo()
            If PointSourceInfo <> "0" Then
                WriteCard260()
            End If
        End If
        If InputDataDictionary("Lakes") <> "" And LakeCount > 0 Then
            WriteCard270()
            If IfError Then GoTo ShowError
            WriteCard280()
            If IfError Then GoTo ShowError
        End If

        If IfError Then GoTo ShowError
        WriteCard290()
        If IfError Then GoTo ShowError

        WriteCard300()
        If IfError Then GoTo ShowError

        If InputDataDictionary("Lakes") <> "" And LakeCount > 0 Then
            WriteCard310()
            If IfError Then GoTo ShowError
        End If

        If InputDataDictionary("cbxSediment") Then
            WriteCard320()
            If IfError Then GoTo ShowError
        End If

        If InputDataDictionary("Lakes") <> "" And LakeCount > 0 And InputDataDictionary("cbxSediment") Then
            WriteCard330()
            If IfError Then GoTo ShowError
        End If

        If InputDataDictionary("cbxMercury") Then
            WriteCard340()
            If IfError Then GoTo ShowError

            WriteCard350()
            If IfError Then GoTo ShowError

            WriteCard360()

            If IfError Then GoTo ShowError

            If InputDataDictionary("Lakes") <> "" And LakeCount > 0 Then
                WriteCard370()
                If IfError Then GoTo ShowError

                WriteCard380()
                If IfError Then GoTo ShowError
            End If
        End If

        'Close the input file
        sw.Close()
        sw.Dispose()

        Return True
        Exit Function

ShowError:
        sw.Close()
        sw.Dispose()
        If (messWriteError <> "") Then
            ErrorMsg("Error Writing GBMM Input File: " & messWriteError)
        Else
            ErrorMsg("Error Writing GBMM Input File: " & Err.Description)
        End If
    End Function

    '******************************************************************************
    'Subroutine: WriteCardHeader
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write input Header
    '******************************************************************************
    Private Sub WriteCardHeader()
        sw.WriteLine((Blank))
        sw.WriteLine(("c   GBMM  - Grid Based Mercury Model, C++ Version 2.0"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c   Designed for:"))
        sw.WriteLine(("c      U.S. EPA, National Exposure Research Laboratory"))
        sw.WriteLine(("c      Ecosystems Research Division"))
        sw.WriteLine(("c      960 College Station Rd."))
        sw.WriteLine(("c      Athens, GA 30605"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c   Developed and maintained by:"))
        sw.WriteLine(("c      Tetra   Tech, Inc."))
        sw.WriteLine(("c      10306 Eaton Place, Suite 340"))
        sw.WriteLine(("c      Fairfax, VA 22030"))
        sw.WriteLine(("c      Phone: (703) 385 6000"))
        sw.WriteLine((Blank))
        sw.WriteLine(("c   GBMM INPUT FILE"))
        sw.WriteLine(("c   This input file was created at " & TimeOfDay & " on " & Today))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard100
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Simulation Control Form options
    '******************************************************************************
    Private Sub WriteCard100()
        sw.WriteLine(("c100 SIMULATION CONTROL"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    hydrofg      if = 1 run hydrology module"))
        sw.WriteLine(("c    sedfg        if = 1 run sediment module"))
        sw.WriteLine(("c    mercuryfg    if = 1 run mercury module"))
        sw.WriteLine(("c    waspfg       if = 1 run model with wasp linkage"))
        sw.WriteLine(("c    whaemfg      if = 1 run model with whaem linkage"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c  hydrofg sedfg   mercuryfg   waspfg  whaemfg"))
        sw.WriteLine((vbTab & InputDataDictionary("cbxHydro") & vbTab & InputDataDictionary("cbxSediment") & vbTab & InputDataDictionary("cbxMercury") & vbTab & InputDataDictionary("cbxWASP") & vbTab & InputDataDictionary("cbxWhAEM")))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard110
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Model Simulation time period
    '******************************************************************************
    Private Sub WriteCard110()
        sw.WriteLine(("c110 MODEL SIMULATION TIME PERIOD"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    simstart     model start date"))
        sw.WriteLine(("c    simend       model end date"))
        sw.WriteLine(("c    startmonth   growing season start month"))
        sw.WriteLine(("c    endmonth     growing season end month"))
        sw.WriteLine(("c    delt         time step in days (fixed = 1)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c  simstart    simend  startmonth  endmonth    delt"))
        sw.WriteLine((vbTab & InputDataDictionary("SimulationStartDate") & "  " & InputDataDictionary("SimulationEndDate") & vbTab & vbTab & InputDataDictionary("startMONTH") & vbTab & InputDataDictionary("endMONTH") & vbTab & 1))
        sw.WriteLine((Blank))
    End Sub


    '******************************************************************************
    'Subroutine: WriteCard120
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Model Data time period
    '******************************************************************************
    Private Sub WriteCard120()
        sw.WriteLine(("c120 DATA TIME PERIOD"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    ******data time period should be within simulation time period in card 110******"))
        sw.WriteLine(("c    ******data time period should be minimum one year if simulation time period is greater than or equal to one year******"))
        sw.WriteLine(("c    ******data time period should be equal to simulation time period if simulation time period is less than one year******"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c     dataindex   data index to distinguish the data type"))
        sw.WriteLine(("c                 if = 1 climate data"))
        sw.WriteLine(("c                 if = 2 mercury dry deposition data"))
        sw.WriteLine(("c                 if = 3 mercury wet deposition data"))
        sw.WriteLine(("c     datastart   data start date"))
        sw.WriteLine(("c     dataend     data end date"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c     dataindex   datastart   dataend"))
        sw.Write((GetDataTimePeriod()))
        sw.WriteLine((Blank))
    End Sub


    '******************************************************************************
    'Subroutine: WriteCard130
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write time series file path and name
    '******************************************************************************
    Private Sub WriteCard130()
        sw.WriteLine(("c130 TIME SERIES FILE PATH"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c     fileindex   file index to distinguish the data type"))
        sw.WriteLine(("c                 if = 1  climate file (month/day/year, station_ID, precipitation_cm, average temperature_C)"))
        sw.WriteLine(("c                 if = 2  dry deposition mercury file (month/day/year, station_ID, dry deposition rate_ug/m2)"))
        sw.WriteLine(("c                 if = 3  wet deposition mercury file (month/day/year, station_ID, wet deposition rate_ug/m2)"))
        sw.WriteLine(("c                 if = 4  point source file (month/day/year, station_ID, flow rate_m3/s, sediment load_kg, mercury load_ug)"))
        sw.WriteLine(("c     filepath    time series file path"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c     fileindex   filepath"))
        sw.Write((GetTimeSeriesFilePath()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard140
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write input / output file path
    '******************************************************************************
    Private Sub WriteCard140()
        sw.WriteLine(("c140 INPUT/OUTPUT FILE PATH"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c pathindex   path index"))
        sw.WriteLine(("c             if = 1 input folder path"))
        sw.WriteLine(("c             if = 2 output folder path"))
        sw.WriteLine(("c folderpath"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c pathindex   folderpath"))
        sw.WriteLine((GetFolderPath()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard150
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write hydrology input grids
    '******************************************************************************
    Private Sub WriteCard150()
        sw.WriteLine(("c150 HYDROLOGY INPUT GRIDS (ASCII FORMAT)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    gridindex    grid index to distinguish the grid type"))
        sw.WriteLine(("c                 if 101 thiessen grid for climate stations (INTEGER GRID)"))
        sw.WriteLine(("c                 if 102 landuse grid i.e. MRLC (INTEGER GRID)"))
        sw.WriteLine(("c                 if 103 soil properties grid (INTEGER GRID)"))
        sw.WriteLine(("c                 if 104 subwatershed grid (INTEGER GRID)"))
        sw.WriteLine(("c                 if 105 curve number pervious land grid2 (REAL GRID)"))
        sw.WriteLine(("c                 if 106 curve number impervious land grid (REAL GRID)"))
        sw.WriteLine(("c                 if 107 totaltime (REAL GRID)"))
        sw.WriteLine(("c                 if 108 streamtime (REAL GRID)"))
        sw.WriteLine(("c                 if 109 soil water grid, optional (REAL GRID)"))
        sw.WriteLine(("c                 if 110 overland length (REAL GRID)"))
        sw.WriteLine(("c                 if 111 average roughness (REAL GRID)"))
        sw.WriteLine(("c                 if 112 average slope (REAL GRID)"))
        sw.WriteLine(("c     gridtype    grid type (integer or real)"))
        sw.WriteLine(("c                     if = 1    integer grid"))
        sw.WriteLine(("c                     if = 2    real grid"))
        sw.WriteLine(("c     gridfile    grid file path"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c     gridindex   gridtype    gridfile"))
        sw.Write((GetHydrologyPath()))
        sw.WriteLine((Blank))
    End Sub


    '******************************************************************************
    'Subroutine: WriteCard160
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Sediment input grids
    '******************************************************************************
    Private Sub WriteCard160()
        sw.WriteLine(("c160 SEDIMENT INPUT GRIDS (ASCII FORMAT)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c gridindex   grid index to distinguish the grid type"))
        sw.WriteLine(("c             if = 201 LS grid for MUSLE equation(REAL GRID)"))
        sw.WriteLine(("c    gridtype    grid type (integer or real)"))
        sw.WriteLine(("c                if = 1    integer grid"))
        sw.WriteLine(("c                if = 2    real grid"))
        sw.WriteLine(("c    gridfile    grid file name"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    gridindex   gridtype   gridfile"))
        sw.Write((GetSedimentPath()))
        sw.WriteLine((Blank))
        System.Windows.Forms.Application.DoEvents()
    End Sub


    '******************************************************************************
    'Subroutine: WriteCard170
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Mercury input grids
    '******************************************************************************
    Private Sub WriteCard170()
        sw.WriteLine(("c170 MERCURY INPUT GRIDS (ASCII FORMAT)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c gridindex   grid index to distinguish the grid type"))
        sw.WriteLine(("c                if = 301  forest litter decomposition rate grid (REAL GRID)"))
        sw.WriteLine(("c                if = 302  thiessen grid for mercury stations, optional (INTEGER GRID)"))
        sw.WriteLine(("c                if = 303  mercury dry deposition grid, optional (REAL GRID)"))
        sw.WriteLine(("c                if = 304  mercury wet deposition grid, optional (REAL GRID)"))
        sw.WriteLine(("c                if = 305  soil mercury conc grid, optional (REAL GRID)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    gridtype    grid type (integer or real)"))
        sw.WriteLine(("c                if = 1    integer grid"))
        sw.WriteLine(("c                if = 2    real grid"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    gridfile    grid file name"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    gridindex   gridtype   gridfile"))
        sw.Write((GetMercuryPath()))
        sw.WriteLine((Blank))

    End Sub


    '******************************************************************************
    'Subroutine: WriteCard200
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write
    '******************************************************************************
    Private Sub WriteCard200()
        sw.WriteLine(("c200 WATERSHED CONTROLS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c nsws    number of subwatersheds in study area"))
        sw.WriteLine(("c nlus    number of land uses in study area"))
        sw.WriteLine(("c nsls    number of soil types in study area"))
        sw.WriteLine(("c ncls    number of climate data stations in study area"))
        sw.WriteLine(("c nhgs    number of mercury data stations in study area"))
        sw.WriteLine(("c npts    number of point sources in study area"))
        sw.WriteLine(("c nlks    number of lakes in study area"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c nsws    nlus    nsls    ncls    nhgs    npts    nlks"))
        sw.WriteLine((GetWatershedControls()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard210
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write
    '******************************************************************************
    Private Sub WriteCard210()
        sw.WriteLine(("c210 SUBWATERSHED INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c swsindex    subwatershed index(Serial number)"))
        sw.WriteLine(("c swsid       subwatershed id (grid value)"))
        sw.WriteLine(("c swsarea     subwatershed area (m2)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c swsindex    swsid    swsarea"))
        sw.WriteLine((GetSubWatershedInfo()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard220
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Landuse info
    '******************************************************************************
    Private Sub WriteCard220()
        sw.WriteLine(("c220 LANDUSE INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c luindex     landuse index (serial number)"))
        sw.WriteLine(("c luid        landuse id (grid value)"))
        sw.WriteLine(("c lutype      landuse type"))
        sw.WriteLine(("c             0 for water body"))
        sw.WriteLine(("c             1 for pervious land"))
        sw.WriteLine(("c             2 for impervious land"))
        sw.WriteLine(("c             3 for forest land"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c growvcf     growing season vegetation cover factor (0-1)"))
        sw.WriteLine(("c ngrowvcf    nongrowing season vegetation cover factor (0-1)"))
        sw.WriteLine(("c cfact       crop factor (0-1)"))
        sw.WriteLine(("c pfact       practice factor (0-1)"))
        sw.WriteLine(("c luname      landuse name"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c luindex luid    lutype  growvcf ngrowvcf    cfact   pfact   luname"))
        sw.WriteLine((GetLandUseInfo(InputDataDictionary("Landuse"), "TYPE", "GETCOVER", "NGETCOVER", "LUC", "LUP", "LUNAME")))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard230
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write Soil Properties info
    '******************************************************************************
    Private Sub WriteCard230()
        sw.WriteLine(("c230 SOIL PROPERTIES INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c slindex     soil properties index (serial number)"))
        sw.WriteLine(("c slid        soil properties id (grid value)"))
        sw.WriteLine(("c awc         plant available water content (cm/m)"))
        sw.WriteLine(("c bd          soil bulk density (g/cm3)"))
        sw.WriteLine(("c clayfr      fraction of clay content in soil"))
        sw.WriteLine(("c perm        soil permeability (cm/hr)"))
        sw.WriteLine(("c kfact       soil erodability factor (0-1)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c slindex slid  awc   bd  clayfr  perm    kfact"))
        'sw.WriteLine (GetSoilInfo(gApplicationPath & "\Data", InputDataDictionary("SoilMap")))
        'Get the data from temp\sub folder - Sabu Paul
        sw.WriteLine((GetSoilInfo(gMapTempFolder & "\Sub", InputDataDictionary("SoilMap"))))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard240
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write climate station info
    '******************************************************************************
    Private Sub WriteCard240()
        sw.WriteLine(("c240 CLIMATE STATION INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c clsindex    climate station index (serial number)"))
        sw.WriteLine(("c clsid       climate station id (grid value)"))
        sw.WriteLine(("c clsname     climate station name (station_id)"))
        sw.WriteLine(("c clslat      climate station latitude (degrees)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c clsindex clsid  clsname clslat"))
        sw.WriteLine((ExtractStationIDInfo()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard250
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write mercury station info
    '******************************************************************************
    Private Sub WriteCard250()
        sw.WriteLine(("c250 MERCURY STATION INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c hgsindex    mercury station index (serial number)"))
        sw.WriteLine(("c hgsid       mercury station id (grid value)"))
        sw.WriteLine(("c hgsname     mercury station name (station_id)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c hgsindex hgsid  hgsname"))
        sw.WriteLine((ExtractHgStationIDInfo()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard260
    'Author:     Ragothaman Bhimarao
    'Purpose:    Write point source info
    '******************************************************************************
    Private Sub WriteCard260()
        sw.WriteLine(("c260 POINT SOURCE INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    psindex  point source index (seriel number)"))
        sw.WriteLine(("c    swsid    subwatershed id (grid value)"))
        sw.WriteLine(("c    psname   point source name (permit)"))
        sw.WriteLine(("c    psttime  point source travel time to outlet (hr)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c    psindex  swsid   psname  psttime"))
        sw.WriteLine((PointSourceInfo))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard270
    'Author:     Ragothaman Bhimarao
    'Purpose:    Lake info
    '******************************************************************************
    Private Sub WriteCard270()
        sw.WriteLine(("c270 LAKE INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c lkindex     lake index (serial number)"))
        sw.WriteLine(("c lkid        lake id (grid value)"))
        sw.WriteLine(("c swsid       subwatershed id (grid value)"))
        sw.WriteLine(("c lkarea      lake surface area (m2)"))
        sw.WriteLine(("c lkdepth     lake bakfull depth (m)"))
        sw.WriteLine(("c lkidepth    lake initial water depth (m)"))
        sw.WriteLine(("c lkised      lake initial sediment concentration (mg/l)"))
        sw.WriteLine(("c lkihg       lake initial mercury concentration (ng/l)"))
        sw.WriteLine(("c lkihgb      lake initial benthic mercury concentration (ng/g)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c lkindex lkid    swsid   lkarea lkdepth lkidepth lkised   lkihg  lkihgb"))
        sw.WriteLine((LakeInfo))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard280
    'Author:     Ragothaman Bhimarao
    'Purpose:    Lake-ClimateStation info
    '******************************************************************************
    Private Sub WriteCard280()
        sw.WriteLine(("c280 LAKE-CLIMATE STATION INFORMATION"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c index   sequence number"))
        sw.WriteLine(("c lkid    lake id (grid value)"))
        sw.WriteLine(("c clsid   climate station id (grid value)"))
        sw.WriteLine(("c frac    fraction contribution of the climate station to the lake, (0-1)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c index   lkid    clsid   frac"))
        sw.WriteLine((lakeCSstr))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard290
    'Author:     Ragothaman Bhimarao
    'Purpose:    Routing Network info
    '******************************************************************************
    Private Sub WriteCard290()
        sw.WriteLine(("c290 ROUTING NETWORK"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c from        swsid (outlet)"))
        sw.WriteLine(("c to          swsid (outlet)"))
        sw.WriteLine(("c traveltime  travel time from outlet to outlet (hr)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c from    to  traveltime"))
        sw.WriteLine((GetRoutingInfo()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard300
    'Author:     Ragothaman Bhimarao
    'Purpose:    Watershed Hydrology Parameters
    '******************************************************************************
    Private Sub WriteCard300()
        sw.WriteLine(("c300 WATERSHED HYDROLOGY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c swfg    soil water flag"))
        sw.WriteLine(("c         if = 1 constant value"))
        sw.WriteLine(("c         if = 2 input grid"))
        sw.WriteLine(("c         if = 3 field capacity"))
        sw.WriteLine(("c swater  initial soil water if swfg=1 otherwise 0 (cm/m)"))
        sw.WriteLine(("c isnow   initial snow on land (cm on water)"))
        sw.WriteLine(("c grow_a  5day precipitation parameter a for growing season (cm)"))
        sw.WriteLine(("c grow_b  5day precipitation parameter b for growing season (cm)"))
        sw.WriteLine(("c ngrow_a 5day precipitation parameter a for non growing season (cm)"))
        sw.WriteLine(("c ngrow_b 5day precipitation parameter b for non growing season (cm)"))
        sw.WriteLine(("c usdepth unsaturated soil depth (m)"))
        sw.WriteLine(("c brdepth soil depth to bed rock (m)"))
        sw.WriteLine(("c gwater  initial shallow ground water (cm/m)"))
        sw.WriteLine(("c gr      groundwater recession coefficient (/day)"))
        sw.WriteLine(("c sr      groundwater seepage coefficient (/day)"))
        sw.WriteLine(("c gwrp    average groundwater recharge period (days)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c swfg    swater  isnow   grow_a  grow_b  ngrow_a ngrow_b usdepth brdepth gwater  gr  sr  gwrp"))
        sw.WriteLine((GetHydrologyParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard310
    'Author:     Ragothaman Bhimarao
    'Purpose:    Lake Hydrology Parameters
    '******************************************************************************
    Private Sub WriteCard310()
        sw.WriteLine(("c310 LAKE HYDROLOGY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c ks      lake infiltration rate (cm/hr)"))
        sw.WriteLine(("c evap_c  evaporation coefficient used to compute AET from lake (AET = evap_c * PET"))
        sw.WriteLine(("c orif_h  orifice depth (m)"))
        sw.WriteLine(("c orif_d  orifice diameter (m)"))
        sw.WriteLine(("c orif_c  orifice coefficient of discharge"))
        sw.WriteLine(("c weir_l  length of the weir crest (m)"))
        sw.WriteLine(("c weir_c  weir coefficient of discharge"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c ks  evap_c  orif_h  orif_d  orif_c  weir_l  weir_c"))
        sw.WriteLine((GetLakeParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard320
    'Author:     Ragothaman Bhimarao
    'Purpose:    Watershed sediment parameters
    '******************************************************************************
    Private Sub WriteCard320()
        sw.WriteLine(("c320 WATERSHED SEDIMENT PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c psed_init   initial sediment on pervious land (kg/ha)"))
        sw.WriteLine(("c ised_init   initial sediment on impervious land (kg/ha)"))
        sw.WriteLine(("c ised_acc    sediment accumulation rate on impervious land (kg/ha/day)"))
        sw.WriteLine(("c ised_depl   sediment depletion rate constant on impervious land (/day)"))
        sw.WriteLine(("c sed_cap     sedeiment yield capacity on land (kg/ha)"))
        sw.WriteLine(("c rain_alph   fraction of dialy rainfall that occurs during the time of concentration"))
        sw.WriteLine(("c sdr_alph    calibration coefficient for computing sediment delivery ratio"))
        sw.WriteLine(("c sdr_beta    routing coefficient for computing sediment delivery ratio"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c psed_init   ised_init   ised_acc    ised_depl   sed_cap rain_alph   sdr_alph    sdr_beta"))
        sw.WriteLine((GetWatershedParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard330
    'Author:     Ragothaman Bhimarao
    'Purpose:    Lake sediment parameters
    '******************************************************************************
    Private Sub WriteCard330()
        sw.WriteLine(("c330 LAKE SEDIMENT PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c tss_eq      equilibrium concentration of suspended solids in the water (g/m3)"))
        sw.WriteLine(("c sett_k      settling constant rate (/day)"))
        sw.WriteLine(("c tss_clay    fraction of clay in the inflow sediment"))
        sw.WriteLine(("c tss_silt    fraction of silt in the inflow sediment"))
        sw.WriteLine(("c tss_sand    fraction of sand in the inflow sediment"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c tss_eq  sett_k  tss_clay    tss_silt    tss_sand"))
        sw.WriteLine((GetLakeSedimentParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard340
    'Author:     Ragothaman Bhimarao
    'Purpose:    Air Deposition Mercury parameters
    '******************************************************************************
    Private Sub WriteCard340()
        sw.WriteLine(("c340 AIR DEPOSITION MERCURY PARAMETERS"))
        sw.WriteLine(("c adfg    air deposition mercury flag"))
        sw.WriteLine(("c         if = 1 constant value"))
        sw.WriteLine(("c         if = 2 input grid"))
        sw.WriteLine(("c         if = 3 time series"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c 'if adfg = 1'"))
        sw.WriteLine(("c adfg    air deposition mercury flag"))
        sw.WriteLine(("c dd_f    daily dry deposition mercury flux (µg/m2/day)"))
        sw.WriteLine(("c wdfg    wet deposition mercury flag"))
        sw.WriteLine(("c         if = 1 daily wet deposition mercury flux (µg/m2/day)"))
        sw.WriteLine(("c         if = 2 daily precipitation mercury concentration (ng/l)"))
        sw.WriteLine(("c wd_v    daily wet deposition mercury based on wdfg"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c adfg    dd_f    wdfg    wd_v"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c 'if adfg = 2'"))
        sw.WriteLine(("c adfg    air deposition mercury flag"))
        sw.WriteLine(("c dd_m    daily dry deposition mercury grid multiplier"))
        sw.WriteLine(("c wd_m    daily wet deposition mercury grid multiplier"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c adfg    dd_m    wd_m"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c 'if adfg = 3'"))
        sw.WriteLine(("c adfg    air deposition mercury flag"))
        sw.WriteLine(("c dd_d    dry deposition mercury time series data type"))
        sw.WriteLine(("c         if = 1 daily data"))
        sw.WriteLine(("c         if = 2 monthly data"))
        sw.WriteLine(("c wd_d    wet deposition mercury time series data type"))
        sw.WriteLine(("c         if = 1 daily data"))
        sw.WriteLine(("c         if = 2 monthly data"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c adfg    dd_d    wd_d"))
        sw.WriteLine((GetAirDepositionHgParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard350
    'Author:     Ragothaman Bhimarao
    'Purpose:    Watershed Mercury parameters
    '******************************************************************************
    Private Sub WriteCard350()
        sw.WriteLine(("c350 WATERSHED MERCURY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c shgfg       soil mercury flag"))
        sw.WriteLine(("c             if = 1 initial soil mercury constant concentration(ng/g)"))
        sw.WriteLine(("c             if = 2 initial soil mercury grid multiplier"))
        sw.WriteLine(("c smercury    value based on shgfg"))
        sw.WriteLine(("c gwmercury   initial groundwater mercury concentration(ng/l)"))
        sw.WriteLine(("c zd          watershed soil mixing depth (cm)"))
        sw.WriteLine(("c zr          soil reduction depth (cm)"))
        sw.WriteLine(("c kds         soil water partition coefficient (ml/g)"))
        sw.WriteLine(("c krs         soil base reduction rate (per day)"))
        sw.WriteLine(("c ef          pollutant enrichment factor"))
        sw.WriteLine(("c rd          bedrock density (g/cm3)"))
        sw.WriteLine(("c kcw         chemical weathering rate constant (µm/day"))
        sw.WriteLine(("c crock       concentration of mercury in bedrock (ng/g"))
        sw.WriteLine(("c kd          mercury decay rate in channel (per hour)"))
        sw.WriteLine(("c fmehg       fraction of methylmercury in total mercury"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c shgfg   smercury    gwmercury   zd  zr  kds krs ef  rd  kcw crock   kd  femhg"))
        sw.WriteLine((GetWatershedHgParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard360
    'Author:     Ragothaman Bhimarao
    'Purpose:    Forest Mercury parameters
    '******************************************************************************
    Private Sub WriteCard360()
        sw.WriteLine(("c360 FOREST MERCURY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c aaet    actual annual evapotranspiration (cm/year)"))
        sw.WriteLine(("c fint    interception fraction"))
        sw.WriteLine(("c fadh    adhering fraction"))
        sw.WriteLine(("c lit     initial amount of litter (g/m2)"))
        sw.WriteLine(("c bcf     air-plant bio-concentration factor"))
        sw.WriteLine(("c ca      air mercury concentration (ng/g)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c aaet    fint    fadh    lit bcf  ca"))
        sw.WriteLine((GetForestHgParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: WriteCard370
    'Author:     Ragothaman Bhimarao
    'Purpose:    Lake Mercury parameters
    '******************************************************************************
    Private Sub WriteCard370()
        sw.WriteLine(("c370 LAKE MERCURY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c alpha_w net reduction loss factor in the water column"))
        sw.WriteLine(("c beta_w  net methylation loss factor in the water column"))
        sw.WriteLine(("c kwr     water body mercury reduction rate constant (per day)"))
        sw.WriteLine(("c kwm     water body mercury methylation rate constant (per day)"))
        sw.WriteLine(("c vsb     biomass settling velocity in the water column (m/day)"))
        sw.WriteLine(("c vrs     sediment resuspension velocity (m/day)"))
        sw.WriteLine(("c kdsw    sediment/water partition coefficient in the water column"))
        sw.WriteLine(("c kbio    biomass/water partition coefficient in the water column"))
        sw.WriteLine(("c cbio    biomass concentration in the water column (m2/sec)"))
        sw.WriteLine(("c pd      soil particle density (cm)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c alpha_w beta_w  kwr kwm vsb vrs kdsw    kbio    cbio    pd"))
        sw.WriteLine((GetLakeHgParam()))
        sw.WriteLine((Blank))
    End Sub
    '******************************************************************************
    'Subroutine: WriteCard380
    'Author:     Ragothaman Bhimarao
    'Purpose:    Benthic Mercury parameters
    '******************************************************************************
    Private Sub WriteCard380()
        sw.WriteLine(("c380 BENTHIC MERCURY PARAMETERS"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c alpha_b net reduction loss factor in the benthic sediments"))
        sw.WriteLine(("c beta_b  net methylation loss factor in the benthic sediments"))
        sw.WriteLine(("c kbr     benthic sediment mercury reduction rate constant (per day)"))
        sw.WriteLine(("c kbm     benthic sediment mercury methylation rate constant (per day)"))
        sw.WriteLine(("c por_bs  porosity of the benthic sediment bed"))
        sw.WriteLine(("c vbur    burial velocity (m/day)"))
        sw.WriteLine(("c zb      depth of the benthic sediment bed (m)"))
        sw.WriteLine(("c kdbs    bed sediment/sediment pore water partition coefficient"))
        sw.WriteLine(("c cbs     solids concentration in the benthic sediments (g/m3)"))
        sw.WriteLine(("c esw     pore water diffusion coefficient (m2/sec)"))
        sw.WriteLine(("c"))
        sw.WriteLine(("c alpha_b beta_b  kbr kbm por_bs  vbur    zb  kdbs    cbs esw"))
        sw.WriteLine((GetBenthicHgParam()))
        sw.WriteLine((Blank))
    End Sub

    '******************************************************************************
    'Subroutine: GetDataTimePeriod - C120
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the simulation time periods
    '*****************************************************************************
    Private Function GetDataTimePeriod() As String
        Dim tmpstr As String
        tmpstr = vbTab & "1" & vbTab & InputDataDictionary("startDATE") & vbTab & InputDataDictionary("endDATE") & vbNewLine
        If InputDataDictionary("cbxMercury") = 1 And InputDataDictionary("HgStation") <> "" Then
            tmpstr = tmpstr & vbTab & "2" & vbTab & InputDataDictionary("DryHgStartDate") & vbTab & InputDataDictionary("DryHgEndDate") & vbNewLine
            tmpstr = tmpstr & vbTab & "3" & vbTab & InputDataDictionary("WetHgStartDate") & vbTab & InputDataDictionary("WetHgEndDate") & vbNewLine
        End If
        GetDataTimePeriod = tmpstr
    End Function

    '*****************************************************************************
    'Subroutine: GetTimeSeriesFilePath - C130A
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the count of point sources
    '*****************************************************************************
    Private Function GetPointSourcesCount() As Short
        If InputDataDictionary("PointSources") <> "" Then
            GetPointSourcesCount = ReturnRasterRowCount("pointsources", gMapTempFolder & "\SUB\")
        Else
            GetPointSourcesCount = 0
        End If
    End Function

    '*****************************************************************************
    'Subroutine: GetTimeSeriesFilePath - C130
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the simulation time periods
    '*****************************************************************************
    Private Function GetTimeSeriesFilePath() As String
        Dim tmpstr As String = ""

        For i As Integer = 1 To 4
            Dim tablename As String = Choose(i, "ClimateDataTextFile", "HgDryDepTimeSeries", "HgWetDepTimeSeries", "PSdataTable")
            If GetDataTable(tablename) IsNot Nothing Then
                If (i = 2 Or i = 3) And Not (InputDataDictionary("cbxMercury") And InputDataDictionary("chkTime")) Then
                Else
                    tmpstr &= StringFormat("\t{0}\t{1}\n", i, InputDataDictionary(tablename))
                End If
            Else
                ErrorMsg(tablename & " file missing")
                IfError = True
                Return ""
            End If
        Next
        Return tmpstr
    End Function

    '******************************************************************************
    'Subroutine: GetFolderPath - C140
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the folder paths
    '*****************************************************************************
    Private Function GetFolderPath() As String
        Return StringFormat("\t1\t{0}\n\t2\t{1}", gMapDataFolder, gMapOutputFolder)
    End Function

    '******************************************************************************
    'Subroutine: GetHydrologyPath   - C150
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the hydrology filepaths in the collection
    '*****************************************************************************
    Private Function GetHydrologyPath() As String
        Try
            Dim fileDict As New Generic.Dictionary(Of Integer, String)
            fileDict.Add(101, "thiessenwtr.asc")
            fileDict.Add(102, InputDataDictionary("Landuse") & ".asc")
            fileDict.Add(103, InputDataDictionary("SoilMap") & ".asc")
            fileDict.Add(104, "subwatershed.asc")
            fileDict.Add(105, "cn2.asc")
            fileDict.Add(106, "cnimp.asc")
            fileDict.Add(107, "totaltime.asc")
            fileDict.Add(108, "streamtime.asc")
            If InputDataDictionary("optInputSoilMoisture") And InputDataDictionary("InitialSoilMoisture") <> "" Then
                fileDict.Add(109, InputDataDictionary("InitialSoilMoisture") & ".asc")
            End If
            fileDict.Add(110, "ovlength.asc")
            fileDict.Add(111, "avroughness.asc")
            fileDict.Add(112, "avslope.asc")

            Return AllFilesExist(fileDict)
        Catch ex As Exception
            ErrorMsg(, ex)
            Return ""
        End Try
    End Function

    '******************************************************************************
    'Subroutine: GetSedimentPath    -C160
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the Sediment filepaths in the collection
    '*****************************************************************************
    Private Function GetSedimentPath() As String
        Dim fileDict As New Generic.Dictionary(Of Integer, String)
        fileDict.Add(201, "lsfactor.asc")
        Return AllFilesExist(fileDict)
    End Function

    '******************************************************************************
    'Subroutine: GetMercuryPath  -  C170
    'Author:     Ragothaman Bhimarao
    'Purpose:    Find the Sediment filepaths in the collection
    '*****************************************************************************
    Private Function GetMercuryPath() As String
        Dim fileDict As New Generic.Dictionary(Of Integer, String)
        fileDict.Add(301, "kdcomp.asc")
        If InputDataDictionary("chkTime") Then fileDict.Add(302, "thiessenhg.asc")
        If InputDataDictionary("chkGrid") Then
            fileDict.Add(303, InputDataDictionary("HgDryGrid") & ".asc")
            fileDict.Add(304, InputDataDictionary("HgWetGrid") & ".asc")
        End If
        If InputDataDictionary("optionSoilHgGrid") Then fileDict.Add(305, InputDataDictionary("InitialSoilHg") & ".asc")
        Return AllFilesExist(fileDict)
    End Function

    '******************************************************************************
    'Subroutine: GetWatershedControls  -  C200
    'Author:     Ragothaman Bhimarao
    'Purpose:   Watershed controls
    '*****************************************************************************
    Private Function GetWatershedControls() As String
        Dim nsws As Integer = ReturnRasterRowCount("SubWatershed", gMapTempFolder & "\SUB\")
        If nsws = 0 Then
            MsgBox("Missing Subwatershed raster...")
            messWriteError = "Missing Subwatershed raster..."
            IfError = True
            Return ""
        End If
        Dim nlus As Integer = ReturnRasterRowCount(InputDataDictionary("Landuse"), gMapTempFolder & "\SUB\")
        If nlus = 0 Then
            MsgBox("Missing Landuse raster...")
            messWriteError = "Missing Landuse raster..."
            IfError = True
            Return ""
        End If
        Dim nsls As Integer = ReturnRasterRowCount(InputDataDictionary("SoilMap"), gMapTempFolder & "\SUB\")
        If nsls = 0 Then
            MsgBox("Missing SoilMap raster ...")
            messWriteError = "Missing SoilMap raster..."
            IfError = True
            Return ""
        End If
        Dim ncls As Integer = ReturnRasterRowCount("Thiessenwtr", gMapTempFolder & "\SUB\")
        If ncls = 0 Then
            MsgBox("Missing Thiessenwtr raster...")
            messWriteError = "Missing ThiessenWtr raster..."
            IfError = True
            Exit Function
        End If
        Dim nhgs As Integer = 0
        If InputDataDictionary("chkTime") Then nhgs = ReturnRasterRowCount("Thiessenhg", gMapTempFolder & "\SUB\")
        Dim npts As Integer = 0
        If InputDataDictionary("PointSources") <> "" Then npts = ReturnRasterRowCount("pointsources", gMapTempFolder & "\SUB\")
        Dim nlks As Integer = 0
        If InputDataDictionary("Lakes") <> "" Then nlks = LakeCount
        Return StringFormat("\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", nsws, nlus, nsls, ncls, nhgs, npts, nlks)
    End Function

    '******************************************************************************
    'Subroutine: GetSubWatershedInfo  -  C210
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Subwatershed information
    '*****************************************************************************
    Private Function GetSubWatershedInfo() As String
        Try
            'i think this gets area for each subwatershed
            'in this new version, assume subwatershed is feature grid (shapefile), one shape for each subwatershed

            'THIS NEEDS SOME WORK!

            Dim sfIdx As Integer = GisUtil.LayerIndex(GetInputLayer("Subwatershed").Name)
            Dim tmpstr As String = ""
            For i As Integer = 0 To GisUtil.NumFeatures(sfIdx)
                tmpstr &= StringFormat("\t{0}\t{1}\t{2}\n", i + 1, GisUtil.FieldValue(sfIdx, i, GisUtil.FieldIndex(sfIdx, "ID")), GisUtil.FeatureArea(sfIdx, i))
            Next



            'GetCellArea()

            'Dim pRasterBnd As IRasterBand
            'Dim pBC As IRasterBandCollection
            'Dim pTmpRaster As IRaster
            'Dim pWSRasterLayer As IRasterLayer
            'pWSRasterLayer = GetInputRasterLayer("Subwatershed")
            'pTmpRaster = pWSRasterLayer.Raster
            'Dim i As Short
            'Dim tmpDict As Scripting.Dictionary
            'tmpDict = CreateObject("scripting.Dictionary")

            'Dim NumOfValues As Short
            'Dim pRow As iRow
            'Dim pTable As iTable
            'If Not (pTmpRaster Is Nothing) Then
            '    pBC = pTmpRaster
            '    pRasterBnd = pBC.Item(0)

            '    ' Attribute table
            '    pTable = pRasterBnd.AttributeTable
            '    NumOfValues = pTable.RowCount(Nothing)

            '    If Not pTable Is Nothing Then
            '        For i = 0 To NumOfValues - 1
            '            pRow = pTable.GetRow(i) 'Get a row from the table
            '            tmpDict.Add(pRow.Value(pTable.FindField("Value")), pRow.Value(pTable.FindField("Count")) * pCellArea)
            '        Next i
            '    End If
            'End If
            'Dim tmpstr As String
            'tmpstr = ""
            'For i = 0 To tmpDict.Count - 1
            '    tmpstr = tmpstr & vbTab & i + 1 & vbTab & tmpDict.keys(i) & vbTab & tmpDict.Item(tmpDict.keys(i)) & vbNewLine
            'Next
            Return tmpstr
        Catch ex As Exception
            ErrorMsg(, ex)
            IfError = True
        End Try

    End Function


    '******************************************************************************
    'Subroutine: GetLandUseInfo  -  C220
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Landuse information
    '*****************************************************************************

    Private Function GetLandUseInfo(ByRef strname As String, ByRef lutype As String, ByRef growvcf As String, ByRef ngrowvcf As String, ByRef crop As String, ByRef pracfact As String, ByRef luname As String) As String
        Dim pCursor As Object
        Dim pQueryFilter As Object

        On Error GoTo EH


        Dim pRasterBnd As IRasterBand
        Dim pBC As IRasterBandCollection
        Dim pTmpRaster As IRaster
        Dim rascoll As New Collection
        Dim linkcoll As New Collection
        Dim lutypeColl As New Collection
        Dim growColl As New Collection
        Dim ngrowColl As New Collection
        Dim cropColl As New Collection
        Dim pracColl As New Collection

        'Set ptmpRaster = OpenRasterDatasetFromDisk2(gApplicationPath & "\DATA", strname)
        pTmpRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", strname)
        Dim NumOfValues As Short
        Dim i As Short
        Dim ctr As Short
        Dim lutypeindx As Short
        Dim growindx As Short
        Dim ngrowindx As Short
        Dim lunameindx As Short
        Dim cropindx As Short
        Dim pracindx As Short
        Dim qrystr As String
        Dim tmpstr As String
        Dim pLUTable As iTable
        Dim pRow As iRow
        Dim pTable As iTable
        If Not (pTmpRaster Is Nothing) Then
            pBC = pTmpRaster
            pRasterBnd = pBC.Item(0)

            ' Attribute table
            pTable = pRasterBnd.AttributeTable
            NumOfValues = pTable.RowCount(Nothing)

            If Not pTable Is Nothing Then
                rascoll = New Collection
                For i = 0 To NumOfValues - 1
                    pRow = pTable.GetRow(i) 'Get a row from the table
                    rascoll.Add(pRow.Value(pTable.FindField("Value")))
                Next i
            End If
            pRow = Nothing
            If rascoll.Count() >= 1 Then
                pLUTable = GetDataTable("LuLookupTable")

                '            Dim pQueryFilter As esriGeoDatabase.IQueryFilter
                '            Dim pCursor As esriGeoDatabase.ICursor
                ' create a query filter
                '            Set pQueryFilter = New esriGeoDatabase.QueryFilter


                lutypeindx = pLUTable.FindField(lutype)
                growindx = pLUTable.FindField(growvcf)
                ngrowindx = pLUTable.FindField(ngrowvcf)
                cropindx = pLUTable.FindField(crop)
                pracindx = pLUTable.FindField(pracfact)
                lunameindx = pLUTable.FindField(luname)

                linkcoll = New Collection
                For ctr = 1 To rascoll.Count()
                    ' create the where statement
                    'qrystr = "'" & field1 & " = " & rascoll.Item(ctr) & "'"
                    qrystr = "LUcode = " & rascoll.Item(ctr)
                    pQueryFilter.WhereClause = qrystr

                    ' query the table passed into the function and use a cursor to hold the results
                    pCursor = pLUTable.Search(pQueryFilter, True)

                    pRow = pCursor.NextRow
                    linkcoll.Add(pRow.Value(lunameindx))
                    lutypeColl.Add(pRow.Value(lutypeindx))
                    growColl.Add(pRow.Value(growindx))
                    ngrowColl.Add(pRow.Value(ngrowindx))
                    cropColl.Add(pRow.Value(cropindx))
                    pracColl.Add(pRow.Value(pracindx))

                Next
                tmpstr = ""
                If rascoll.Count() = linkcoll.Count() Then
                    For ctr = 1 To rascoll.Count()
                        tmpstr = tmpstr & vbTab & ctr & vbTab & rascoll.Item(ctr) & vbTab & lutypeColl.Item(ctr) & vbTab & growColl.Item(ctr) & vbTab & ngrowColl.Item(ctr) & vbTab & cropColl.Item(ctr) & vbTab & pracColl.Item(ctr) & vbTab & linkcoll.Item(ctr) & vbNewLine
                    Next
                    GetLandUseInfo = tmpstr
                Else
                    GetLandUseInfo = "Review the Landuse raster and the lookup table. Numbers do not match"
                End If
            End If
        End If
        Exit Function

EH:
        MsgBox("Error in GetLanduseInfo " & Err.Description)
        IfError = True
    End Function



    '******************************************************************************
    'Subroutine: GetSoilInfo  -  C230
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Soil properties info
    '*****************************************************************************

    Public Function GetSoilInfo(ByRef path As String, ByRef soilStr As String) As String
        On Error GoTo EH
        Dim pRasterBnd As IRasterBand
        Dim pBC As IRasterBandCollection
        Dim pSoilRaster As IRaster
        Dim pFeatCls As IFeatureClass
        Dim rascoll As Collection
        Dim pSoilTable As iTable

        pSoilRaster = OpenRasterDatasetFromDisk2(path, soilStr)

        Dim NumOfValues As Short
        Dim i As Short
        Dim pRow As iRow
        Dim pTable As iTable
        If Not (pSoilRaster Is Nothing) Then
            pBC = pSoilRaster
            pRasterBnd = pBC.Item(0)

            ' Attribute table
            pTable = pRasterBnd.AttributeTable
            NumOfValues = pTable.RowCount(Nothing)

            rascoll = New Collection
            If Not pTable Is Nothing Then
                For i = 0 To NumOfValues - 1
                    pRow = pTable.GetRow(i) 'Get a row from the table
                    rascoll.Add(pRow.Value(pTable.FindField("Value")))
                Next i
            End If
        End If

        pSoilTable = GetDataTable("SoilData")

        Dim pds As iTable
        Dim pQueryFilter As IQueryFilter
        Dim QryString As String
        Dim ctr As Short

        pQueryFilter = New QueryFilter

        If rascoll.Count() = 1 Then
            QryString = "VALUE = " & rascoll.Item(1)
        Else
            QryString = "VALUE = " & rascoll.Item(1)
            For ctr = 2 To rascoll.Count()
                QryString = QryString & " or VALUE = " & rascoll.Item(ctr)
            Next
        End If

        ' Set the where clause
        pQueryFilter.WhereClause = QryString

        ' Execute the query filter
        Dim pCursor As ICursor
        Dim pqryRow As iRow
        Dim iRecCount As Short
        Dim awcColl As Collection
        Dim bdColl As Collection
        Dim clayfrColl As Collection
        Dim permColl As Collection
        Dim kfactColl As Collection

        '    Set pFeatureCursor = pFeatCls.Search(pQueryFilter, False)
        pCursor = pSoilTable.Search(pQueryFilter, False)
        awcColl = New Collection
        bdColl = New Collection
        clayfrColl = New Collection
        permColl = New Collection
        kfactColl = New Collection

        iRecCount = 0

        Dim tmpctr As Short
        Dim tmpstr As String
        If Not pSoilTable Is Nothing Then
            ' Attribute table
            For iRecCount = 1 To rascoll.Count()
                pqryRow = pCursor.NextRow
                If Not (pRow Is Nothing) Then
                    'iRecCount = iRecCount + 1
                    awcColl.Add(pqryRow.Value(pSoilTable.FindField("AWC")))
                    bdColl.Add(pqryRow.Value(pSoilTable.FindField("BD")))
                    clayfrColl.Add(pqryRow.Value(pSoilTable.FindField("Clayperc")))
                    permColl.Add(pqryRow.Value(pSoilTable.FindField("perm")))
                    kfactColl.Add(pqryRow.Value(pSoilTable.FindField("KFACT")))
                End If
            Next


            tmpctr = 0
            tmpstr = ""

            For tmpctr = 1 To rascoll.Count()
                tmpstr = tmpstr & vbTab & tmpctr & vbTab & rascoll.Item(tmpctr) & vbTab & awcColl.Item(tmpctr) & vbTab & bdColl.Item(tmpctr) & vbTab & clayfrColl.Item(tmpctr) & vbTab & permColl.Item(tmpctr) & vbTab & kfactColl.Item(tmpctr) & vbNewLine
            Next
            GetSoilInfo = tmpstr

            awcColl = Nothing
            bdColl = Nothing
            clayfrColl = Nothing
            permColl = Nothing
            rascoll = Nothing

        End If
        Exit Function

EH:
        MsgBox("Error in GetSoilInfo " & Err.Description)
        IfError = True
    End Function


    '******************************************************************************
    'Subroutine: ExtractStationIDInfo  -  C240
    'Author:     Ragothaman Bhimarao
    'Purpose:    Extract station name and LAT info from shapefile for the values in raster
    '*****************************************************************************

    Public Function ExtractStationIDInfo() As String
        On Error GoTo EH
        Dim pRasterBnd As IRasterBand
        Dim pBC As IRasterBandCollection
        Dim pThiessenRaster As IRaster
        Dim pFeatCls As IFeatureClass
        Dim rascoll As Collection

        'clip the thiessen to subwatershed extent first - Sabu Paul
        pThiessenRaster = OpenRasterDatasetFromDisk("ThiessSub")
        If (pThiessenRaster Is Nothing) Then
            Call ClipThiessenToSubwatershed(False)
            pThiessenRaster = OpenRasterDatasetFromDisk("ThiessSub")
        End If

        'Set pThiessenRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "ThiessenWtr")
        Dim NumOfValues As Short
        Dim i As Short
        Dim pRow As iRow
        Dim pTable As iTable
        If Not (pThiessenRaster Is Nothing) Then
            pBC = pThiessenRaster
            pRasterBnd = pBC.Item(0)

            ' Attribute table
            pTable = pRasterBnd.AttributeTable
            NumOfValues = pTable.RowCount(Nothing)

            rascoll = New Collection
            If Not pTable Is Nothing Then
                For i = 0 To NumOfValues - 1
                    pRow = pTable.GetRow(i) 'Get a row from the table
                    rascoll.Add(pRow.Value(pTable.FindField("Value")))
                Next i
            End If
        Else
            Err.Raise(vbObjectError + 5130, , "Thiessen Sub is missing")
        End If

        Dim pFeatureLayer As IFeatureLayer
        pFeatureLayer = GetInputFeatureLayer("ClimateStation")

        'Climate station - first check the map - Sabu Paul
        If (Not pFeatureLayer Is Nothing) Then
            pFeatCls = pFeatureLayer.FeatureClass
        Else
            pFeatCls = OpenShapeFile(gApplicationPath & "\Data", InputDataDictionary("ClimateStation"))
        End If

        Dim pds As iTable
        Dim pQueryFilter As IQueryFilter
        Dim QryString As String
        Dim ctr As Short

        pQueryFilter = New QueryFilter

        If rascoll.Count() = 1 Then
            QryString = "FID = " & rascoll.Item(1)
        Else
            QryString = "FID = " & rascoll.Item(1)
            For ctr = 2 To rascoll.Count()
                QryString = QryString & " or FID = " & rascoll.Item(ctr)
            Next
        End If

        ' Set the where clause
        pQueryFilter.WhereClause = QryString

        ' Execute the query filter
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim iRecCount As Short
        Dim stanameColl As Collection
        Dim latColl As Collection
        Dim FIDColl As Collection

        pFeatureCursor = pFeatCls.Search(pQueryFilter, False)
        FIDColl = New Collection
        stanameColl = New Collection
        latColl = New Collection
        iRecCount = 0

        If Not pFeatCls Is Nothing Then
            ' Attribute table
            Do
                pFeature = pFeatureCursor.NextFeature
                If Not (pFeature Is Nothing) Then
                    iRecCount = iRecCount + 1
                    FIDColl.Add(pFeature.Value(pFeatCls.FindField("FID")))
                    stanameColl.Add(pFeature.Value(pFeatCls.FindField("STA_ID")))
                    latColl.Add(pFeature.Value(pFeatCls.FindField("LAT")))
                End If
            Loop Until (pFeature Is Nothing)

            ExtractStationIDInfo = CreateStationInfo(FIDColl, stanameColl, latColl)

            FIDColl = Nothing
            stanameColl = Nothing
            latColl = Nothing

        End If

        Exit Function

EH:
        MsgBox("Error in ExtractStationIDInfo " & Err.Description)
        IfError = True
    End Function




    '******************************************************************************
    'Subroutine: ExtractStationIDInfo  -  C250
    'Author:     Ragothaman Bhimarao
    'Purpose:    Extract Hg Station info
    '*****************************************************************************
    Public Function ExtractHgStationIDInfo() As String
        On Error GoTo EH

        Dim pRasterBnd As IRasterBand
        Dim pBC As IRasterBandCollection
        Dim pThiessenRaster As IRaster
        Dim pFeatCls As IFeatureClass
        Dim rascoll As Collection

        'Set pThiessenRaster = OpenRasterDatasetFromDisk("ThiessenHg")
        pThiessenRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "ThiessenHg")

        Dim NumOfValues As Short
        Dim i As Short
        Dim pRow As iRow
        Dim pTable As iTable
        If Not (pThiessenRaster Is Nothing) Then
            pBC = pThiessenRaster
            pRasterBnd = pBC.Item(0)

            ' Attribute table
            pTable = pRasterBnd.AttributeTable
            NumOfValues = pTable.RowCount(Nothing)

            rascoll = New Collection
            If Not pTable Is Nothing Then
                For i = 0 To NumOfValues - 1
                    pRow = pTable.GetRow(i) 'Get a row from the table
                    rascoll.Add(pRow.Value(pTable.FindField("Value")))
                Next i
            End If
        End If

        Dim pFeatureLayer As IFeatureLayer
        pFeatureLayer = GetInputFeatureLayer("HgStation")
        If (Not pFeatureLayer Is Nothing) Then
            pFeatCls = pFeatureLayer.FeatureClass
        Else
            pFeatCls = OpenShapeFile(gApplicationPath & "\Data", InputDataDictionary("HgStation"))
        End If

        Dim pds As iTable
        Dim pQueryFilter As IQueryFilter
        Dim QryString As String
        Dim ctr As Short

        pQueryFilter = New QueryFilter

        If rascoll.Count() = 1 Then
            QryString = "FID = " & rascoll.Item(1)
        Else
            QryString = "FID = " & rascoll.Item(1)
            For ctr = 2 To rascoll.Count()
                QryString = QryString & " or FID = " & rascoll.Item(ctr)
            Next
        End If

        ' Set the where clause
        pQueryFilter.WhereClause = QryString

        ' Execute the query filter
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim iRecCount As Short
        Dim stanameColl As Collection
        Dim FIDColl As Collection

        pFeatureCursor = pFeatCls.Search(pQueryFilter, False)
        FIDColl = New Collection
        stanameColl = New Collection
        iRecCount = 0

        If Not pFeatCls Is Nothing Then
            ' Attribute table
            Do
                pFeature = pFeatureCursor.NextFeature
                If Not (pFeature Is Nothing) Then
                    iRecCount = iRecCount + 1
                    FIDColl.Add(pFeature.Value(pFeatCls.FindField("FID")))
                    stanameColl.Add(pFeature.Value(pFeatCls.FindField("STA_ID")))
                End If
            Loop Until (pFeature Is Nothing)

            ExtractHgStationIDInfo = CreateStationInfo(FIDColl, stanameColl, Nothing)

            FIDColl = Nothing
            stanameColl = Nothing
        End If

        Exit Function

EH:
        MsgBox("Error in ExtractHgStationIDInfo " & Err.Description)
        IfError = True
    End Function

    '******************************************************************************
    'Subroutine: ExtractPointSourceInfo  -  C260
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Point Sources information
    '*****************************************************************************
    Public Function ExtractPointSourceInfo() As Object
        Dim GetPixelNoDataMaskVal As Object
        Dim GetPixelValue As Object
        Dim pFeatureCenter As Object
        Dim pTable As Object ' As String
        On Error GoTo EH

        Dim pPointSourceRaster As IRaster
        pPointSourceRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "PointSources")

        Dim pSubWSRaster As IRaster
        pSubWSRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "Subwatershed")

        Dim pSWSTimeRaster As IRaster
        pSWSTimeRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "swstime")

        If pPointSourceRaster Is Nothing Or pSubWSRaster Is Nothing Or pSWSTimeRaster Is Nothing Then
            MsgBox("Missing PointSource/Subwatershed/swstime raster")
            IfError = True
            Exit Function
        End If

        Dim pBC As IRasterBandCollection
        pBC = pPointSourceRaster

        Dim pRasterBnd As IRasterBand
        pRasterBnd = pBC.Item(0)

        ' Attribute table
        '    Dim pTable As esriGeoDatabase.iTable
        pTable = pRasterBnd.AttributeTable

        Dim NumOfValues As Short
        NumOfValues = pTable.RowCount(Nothing)
        If NumOfValues < 1 Then
            ExtractPointSourceInfo = CStr(NumOfValues)
            'IfError = True
            Exit Function
        End If

        Dim i As Short
        Dim pRow As iRow

        Dim rascoll As Collection
        rascoll = New Collection
        If Not pTable Is Nothing Then
            For i = 0 To NumOfValues - 1
                pRow = pTable.GetRow(i) 'Get a row from the table
                rascoll.Add(pRow.Value(pTable.FindField("Value")))
            Next i
        End If

        Dim pFeatureLayer As IFeatureLayer
        pFeatureLayer = GetInputFeatureLayer("PointSources")

        Dim pFeatCls As IFeatureClass
        If Not pFeatureLayer Is Nothing Then
            pFeatCls = pFeatureLayer.FeatureClass
        Else
            MsgBox("Missing PointSource layer")
            IfError = True
            Exit Function
        End If

        Dim pds As iTable
        Dim pQueryFilter As IQueryFilter
        Dim QryString As String
        Dim ctr As Short

        pQueryFilter = New QueryFilter

        If rascoll.Count() = 1 Then
            QryString = "FID = " & rascoll.Item(1)
        Else
            QryString = "FID = " & rascoll.Item(1)
            For ctr = 2 To rascoll.Count()
                QryString = QryString & " or FID = " & rascoll.Item(ctr)
            Next
        End If

        ' Set the where clause
        pQueryFilter.WhereClause = QryString

        ' Execute the query filter
        Dim pFeatureCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim iRecCount As Short
        Dim stanameDict As Scripting.Dictionary

        pFeatureCursor = pFeatCls.Search(pQueryFilter, False)
        stanameDict = New Scripting.Dictionary
        iRecCount = 0

        '    Dim pFeatureCenter As esriGeometry.IPoint

        Dim SubWSDict As Scripting.Dictionary
        SubWSDict = New Scripting.Dictionary

        Dim SWSTimeDict As Scripting.Dictionary
        SWSTimeDict = New Scripting.Dictionary

        'Set cell size and cell area
        GetCellArea()

        Dim PSid As Double
        Dim SubWSid As Double
        Dim SWSTimeid As Double

        Dim pRasterPropPS As IRasterProps
        Dim pRasterPropSubWS As IRasterProps
        Dim pRasterPropSWStime As IRasterProps

        ' get raster properties
        pRasterPropPS = pPointSourceRaster
        pRasterPropSubWS = pSubWSRaster
        pRasterPropSWStime = pSWSTimeRaster

        If pRasterPropPS.Width <> pRasterPropSubWS.Width Or pRasterPropSubWS.Width <> pRasterPropSWStime.Width Or pRasterPropPS.Height <> pRasterPropSubWS.Height Or pRasterPropSubWS.Height <> pRasterPropSWStime.Height Then
            Err.Description = "Extent mismatch"
            GoTo EH
        End If

        Dim cnt As Short
        Dim tmpstr As String
        If Not pFeatCls Is Nothing Then
            ' Attribute table
            Do
                pFeature = pFeatureCursor.NextFeature
                If Not (pFeature Is Nothing) Then
                    pFeatureCenter = pFeature.Shape

                    iRecCount = iRecCount + 1
                    stanameDict.Add(pFeature.Value(pFeatCls.FindField("FID")), pFeature.Value(pFeatCls.FindField("STA_ID")))
                    'Only process the point sources within the subwatershed
                    If (GetPixelNoDataMaskVal(pFeatureCenter, pSubWSRaster) = 1) Then
                        SubWSid = GetPixelValue(pFeatureCenter, pSubWSRaster)

                        If (GetPixelNoDataMaskVal(pFeatureCenter, pPointSourceRaster) = 1) Then

                            PSid = GetPixelValue(pFeatureCenter, pPointSourceRaster)
                            If Not (SubWSDict.Exists(PSid)) Then SubWSDict.Add(PSid, SubWSid)

                            If (GetPixelNoDataMaskVal(pFeatureCenter, pSWSTimeRaster) = 1) Then
                                SWSTimeid = GetPixelValue(pFeatureCenter, pSWSTimeRaster)
                                If Not (SWSTimeDict.Exists(PSid)) Then SWSTimeDict.Add(PSid, SWSTimeid)
                            End If
                        End If
                    End If
                End If
            Loop Until (pFeature Is Nothing)


            ''        Dim pPixelBlockPS As IPixelBlock3
            ''        Dim pPixelBlockSubWS As IPixelBlock3
            ''        Dim pPixelBlockSWStime As IPixelBlock3
            ''
            ''        Dim vPixelDataPS As Variant
            ''        Dim vPixelDataSubWS As Variant
            ''        Dim vPixelDataSWStime As Variant
            ''
            ''        ' get vb supported pixel type
            ''        pRasterPropPS.PixelType = GetVBSupportedPixelType(pRasterPropPS.PixelType)
            ''        pRasterPropSubWS.PixelType = GetVBSupportedPixelType(pRasterPropSubWS.PixelType)
            ''        pRasterPropSWStime.PixelType = GetVBSupportedPixelType(pRasterPropSWStime.PixelType)
            ''
            ''
            ''
            ''        Dim lBlockSize As Long
            ''        Dim lTileNum As Long
            ''        Dim lTile As Long
            ''        Dim lStartRow As Long
            ''
            ''        lBlockSize = CLng(1048576 / pRasterPropSubWS.Width)
            ''        If lBlockSize < 1 Then lBlockSize = 1
            ''        lTileNum = CLng(pRasterPropSubWS.Height / lBlockSize)
            ''        If lTileNum * lBlockSize < pRasterPropSubWS.Height Then lTileNum = lTileNum + 1
            ''
            ''        Dim pSize As IPnt
            ''        Set pSize = New DblPnt
            ''        Dim pOrigin As IPnt
            ''        Set pOrigin = New DblPnt
            ''
            ''        For lTile = 0 To lTileNum - 1
            ''           lStartRow = lTile * lBlockSize
            ''           If lStartRow + lBlockSize > pRasterPropSubWS.Height Then lBlockSize = pRasterPropSubWS.Height - lStartRow
            ''           pOrigin.SetCoords 0, lStartRow
            ''           pSize.SetCoords pRasterPropSubWS.Width, lBlockSize
            ''
            ''            Set pPixelBlockPS = pPointSourceRaster.CreatePixelBlock(pSize)
            ''            pPointSourceRaster.Read pOrigin, pPixelBlockPS
            ''            vPixelDataPS = pPixelBlockPS.PixelData(0)
            ''
            ''            Set pPixelBlockSubWS = pSubWSRaster.CreatePixelBlock(pSize)
            ''            pSubWSRaster.Read pOrigin, pPixelBlockSubWS
            ''            vPixelDataSubWS = pPixelBlockSubWS.PixelData(0)
            ''
            ''            Set pPixelBlockSWStime = pSWSTimeRaster.CreatePixelBlock(pSize)
            ''            pSWSTimeRaster.Read pOrigin, pPixelBlockSWStime
            ''            vPixelDataSWStime = pPixelBlockSWStime.PixelData(0)
            ''
            ''
            ''            Dim lCol As Long
            ''            Dim lRow As Long
            ''
            ''
            ''
            ''
            ''            For lCol = 0 To pRasterPropPS.Width - 1
            ''                'For iRow = 0 To pRasterPropPS.Height - 1
            ''                For lRow = 0 To lBlockSize - 1
            ''                    If pPixelBlockPS.GetNoDataMaskVal(0, lCol, lRow) = 1 Then
            ''                        PSid = vPixelDataPS(lCol, lRow)
            ''                        'Get Subwatershed ID
            ''                        If pPixelBlockSubWS.GetNoDataMaskVal(0, lCol, lRow) = 1 Then
            ''                            SubWSid = vPixelDataSubWS(lCol, lRow)
            ''                            If Not (SubWSDict.Exists(PSid)) Then
            ''                                SubWSDict.Add PSid, SubWSid
            ''                            End If
            ''                        End If
            ''
            ''                        'Get swstime info
            ''                        If pPixelBlockSWStime.GetNoDataMaskVal(0, lCol, lRow) = 1 Then
            ''                            SWSTimeid = vPixelDataSWStime(lCol, lRow)
            ''                            If Not (SWSTimeDict.Exists(PSid)) Then
            ''                                SWSTimeDict.Add PSid, SWSTimeid
            ''                            End If
            ''                        End If
            ''                    End If
            ''                Next
            ''            Next
            ''       Next



            tmpstr = ""
            For cnt = 1 To rascoll.Count()
                tmpstr = tmpstr & vbTab & cnt & vbTab & SubWSDict.Item(rascoll.Item(cnt)) & vbTab & stanameDict.Item(rascoll.Item(cnt)) & vbTab & SWSTimeDict(rascoll.Item(cnt)) & vbNewLine
            Next

            'MsgBox tmpstr

            ExtractPointSourceInfo = tmpstr

        End If
        Exit Function

EH:
        MsgBox("Error in ExtractPointSourceInfo " & Err.Description)
        IfError = True
    End Function

    '******************************************************************************
    'Subroutine: GetLakeInfo  -  C270
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Lake information
    '*****************************************************************************
    Public Function GetLakeInfo() As String
        On Error GoTo ShowError
        Dim pLakeRaster As IRaster
        Dim pSubWSRaster As IRaster
        Dim pClimateRaster As IRaster

        pLakeRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "LakesGrid")
        pSubWSRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "Subwatershed")
        pClimateRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "Thiessenwtr")


        '    Set pLakeRaster = OpenRasterDatasetFromDisk("LakesGrid")
        '    Set pSubWSRaster = OpenRasterDatasetFromDisk("Subwatershed")
        '    Set pClimateRaster = OpenRasterDatasetFromDisk("Thiessenwtr")


        If pLakeRaster Is Nothing Or pSubWSRaster Is Nothing Or pClimateRaster Is Nothing Then
            MsgBox("Missing Lake/Subwatershed/Thiessenwtr raster")
            IfError = True
            Exit Function
        End If

        ' get raster properties
        Dim pRasterPropLake As IRasterProps
        pRasterPropLake = pLakeRaster
        ' get vb supported pixel type
        pRasterPropLake.PixelType = GetVBSupportedPixelType(pRasterPropLake.PixelType)

        Dim pRasterPropSubWS As IRasterProps
        pRasterPropSubWS = pSubWSRaster
        pRasterPropSubWS.PixelType = GetVBSupportedPixelType(pRasterPropSubWS.PixelType)

        Dim pRasterPropClimate As IRasterProps
        pRasterPropClimate = pClimateRaster
        pRasterPropClimate.PixelType = GetVBSupportedPixelType(pRasterPropClimate.PixelType)

        If pRasterPropLake.Width <> pRasterPropSubWS.Width Or pRasterPropSubWS.Width <> pRasterPropClimate.Width Or pRasterPropLake.Height <> pRasterPropSubWS.Height Or pRasterPropSubWS.Height <> pRasterPropClimate.Height Then
            Err.Description = "Extent mismatch"
            GoTo ShowError
        End If

        '    Dim noDataValueLake As Double
        '    Dim noDataValueSubWS As Double
        '    Dim noDataValueClimate As Integer

        GetCellArea()

        Dim iCol As Short
        Dim iRow As Short
        Dim lakeDict As Scripting.Dictionary
        lakeDict = New Scripting.Dictionary

        Dim tempDict As Scripting.Dictionary
        tempDict = New Scripting.Dictionary

        Dim climDict As Scripting.Dictionary
        climDict = New Scripting.Dictionary

        Dim lakeId As Double
        Dim SubWSid As Double

        Dim climateid As Short

        Dim pPixelBlockLake As IPixelBlock3
        Dim pPixelBlockSubWS As IPixelBlock3
        Dim pPixelBlockClimate As IPixelBlock3

        Dim vPixelDataLake As Object
        Dim vPixelDataSubWS As Object
        Dim vPixelDataClimate As Object

        Dim lBlockSize As Integer
        Dim lTileNum As Integer
        Dim lTile As Integer
        Dim lStartRow As Integer

        lBlockSize = CInt(1048576 / pRasterPropSubWS.Width)
        If lBlockSize < 1 Then lBlockSize = 1
        lTileNum = CInt(pRasterPropSubWS.Height / lBlockSize)
        If lTileNum * lBlockSize < pRasterPropSubWS.Height Then lTileNum = lTileNum + 1

        ' create a DblPnt to hold the PixelBlock size
        Dim pSize As IPnt
        pSize = New DblPnt

        Dim pOrigin As IPnt
        pOrigin = New DblPnt

        For lTile = 0 To lTileNum - 1
            lStartRow = lTile * lBlockSize
            If lStartRow + lBlockSize > pRasterPropSubWS.Height Then lBlockSize = pRasterPropSubWS.Height - lStartRow
            pOrigin.SetCoords(0, lStartRow)
            pSize.SetCoords(pRasterPropSubWS.Width, lBlockSize)

            pPixelBlockSubWS = pSubWSRaster.CreatePixelBlock(pSize)
            pSubWSRaster.Read(pOrigin, pPixelBlockSubWS)
            vPixelDataSubWS = pPixelBlockSubWS.PixelData(0)

            pPixelBlockLake = pLakeRaster.CreatePixelBlock(pSize)
            pLakeRaster.Read(pOrigin, pPixelBlockLake)
            vPixelDataLake = pPixelBlockLake.PixelData(0)

            pPixelBlockClimate = pClimateRaster.CreatePixelBlock(pSize)
            pClimateRaster.Read(pOrigin, pPixelBlockClimate)
            vPixelDataClimate = pPixelBlockClimate.PixelData(0)

            For iCol = 0 To pRasterPropLake.Width - 1
                For iRow = 0 To lBlockSize - 1
                    If pPixelBlockLake.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        lakeId = vPixelDataLake(iCol, iRow)
                        'Get Subwatershed ID -- Commented on January 11, 2005 - Lake subwatershed mapping is done based on AssessPoints
                        ''''                    If pPixelBlockSubWS.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        ''''                        SubWSid = vPixelDataSubWS(iCol, iRow)
                        ''''                        If Not (lakeDict.Exists(lakeId)) Then
                        ''''                            lakeDict.Add lakeId, SubWSid
                        ''''                        End If
                        ''''                    End If

                        'Get climate info
                        If pPixelBlockClimate.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                            climateid = vPixelDataClimate(iCol, iRow)
                            If (climDict.Exists(lakeId)) Then
                                tempDict = climDict.Item(lakeId)
                                If (tempDict.Exists(climateid)) Then
                                    tempDict.let_Item(climateid, tempDict.Item(climateid) + 1)
                                Else
                                    tempDict.Add(climateid, 1)
                                End If
                                climDict.Item(lakeId) = tempDict
                            Else
                                tempDict = New Scripting.Dictionary
                                tempDict.Add(climateid, 1)
                                climDict.Add(lakeId, tempDict)
                            End If
                        End If
                    End If
                Next
            Next
        Next

        lakeDict = GetLakeIDAssessIdDictionary
        LakeCount = lakeDict.Count

        'Get Lake Area
        Dim pLakeRasterBand As IRasterBand
        Dim pLakeRasterBC As IRasterBandCollection
        Dim lakeAreaDict As New Scripting.Dictionary

        Dim pLakeTable As iTable

        Dim NumVal As Short
        Dim cnt As Short
        Dim qrystr As String
        Dim pqryfilter As IQueryFilter
        Dim pCursor As ICursor
        Dim pRow As iRow
        If Not pLakeRaster Is Nothing Then
            pLakeRasterBC = pLakeRaster
            pLakeRasterBand = pLakeRasterBC.Item(0)
            pLakeTable = pLakeRasterBand.AttributeTable

            NumVal = pLakeTable.RowCount(Nothing)

            If Not pLakeTable Is Nothing Then

                qrystr = ""

                If climDict.Count = 1 Then
                    qrystr = "Value = " & climDict.keys(0)
                ElseIf climDict.Count > 1 Then
                    qrystr = "Value = " & climDict.keys(0)
                    For cnt = 1 To climDict.Count - 1
                        qrystr = qrystr & " or Value = " & climDict.keys(cnt)
                    Next
                ElseIf climDict.Count < 1 Then
                    Exit Function
                End If
                '            Set pqryfilter = New esriGeoDatabase.QueryFilter
                pqryfilter.WhereClause = qrystr

                pCursor = pLakeTable.Search(pqryfilter, False)

                'For cnt = 0 To lakeDict.Count - 1
                Do
                    pRow = pCursor.NextRow
                    If Not (pRow Is Nothing) Then
                        lakeAreaDict.Add(pRow.Value(pLakeTable.FindField("value")), pRow.Value(pLakeTable.FindField("count")) * pCellArea)
                    End If
                Loop Until pRow Is Nothing
                'Next

            End If
            qrystr = ""
            For cnt = 0 To lakeAreaDict.Count - 1
                qrystr = qrystr & lakeAreaDict.Item(lakeAreaDict.keys(cnt)) & vbNewLine
            Next

        End If

        Dim outStr As String
        Dim i As Short
        Dim j As Short
        Dim lakeKeys, tempKeys As Object
        Dim tmptotal As Double
        Dim fracdict As New Scripting.Dictionary
        Dim lakestr As String

        lakeKeys = lakeDict.keys
        outStr = ""
        lakeCSstr = ""

        Dim LakeInitSedi As String
        Dim LakeInitHg As String
        Dim LakeInitHgBenthic As String


        ''    If InputDataDictionary("LakeInitialSediment") = "" Then
        ''        LakeInitSedi = 0
        ''    Else
        ''        LakeInitSedi = InputDataDictionary("LakeInitialSediment")
        ''    End If
        ''
        ''    If InputDataDictionary("LakeHgWaterColumn") = "" Then
        ''        LakeInitHg = 0
        ''    Else
        ''        LakeInitHg = InputDataDictionary("LakeHgWaterColumn")
        ''    End If
        ''
        '    InputDataDictionary ("LakeHgBenthic")

        lakestr = InputDataDictionary("BankFullDepth") & vbTab & InputDataDictionary("InitWaterDepth") & vbTab & InputDataDictionary("LakeInitialSediment") & vbTab & InputDataDictionary("LakeHgWaterColumn") & vbTab & InputDataDictionary("LakeHgBenthic")

        Dim climIndex As Integer
        climIndex = 1

        For i = 0 To lakeDict.Count - 1
            tmptotal = 0
            outStr = outStr & vbTab & i + 1 & vbTab & lakeKeys(i) & vbTab
            outStr = outStr & lakeDict.Item(lakeKeys(i)) & vbTab
            outStr = outStr & lakeAreaDict.Item(lakeKeys(i)) & vbTab
            outStr = outStr & lakestr & vbNewLine

            tempDict = climDict.Item(lakeKeys(i)) 'lakeDict.keys(i)
            tempKeys = tempDict.keys


            For j = 0 To tempDict.Count - 1
                'outStr = outStr & vbTab & i + 1 & vbTab & lakeKeys(i) & vbTab
                ''            outStr = outStr & lakeDict.Item(lakeKeys(i)) & vbTab
                ''            outStr = outStr & lakeAreaDict.Item(lakeKeys(i)) & vbTab
                ''
                ''            outStr = outStr & tempDict.Item(tempKeys(j)) & vbTab
                ''            outStr = outStr & lakeAreaDict.Item(lakeKeys(i)) & vbTab & lakestr & vbNewLine
                '''            outStr = outStr & "  " & tempKeys(j) 'tempDict.key(tempDict.keys(j))
                '' '           outStr = outStr & "  " & tempDict.Item(tempKeys(j)) & vbNewLine
                tmptotal = tmptotal + tempDict.Item(tempDict.keys(j))
            Next

            For j = 0 To tempDict.Count - 1
                ''            lakeCSstr = lakeCSstr & vbTab & climIndex & vbTab & lakeKeys(i) & vbTab & tempDict.Item(tempKeys(j)) & vbTab & tempDict.Item(tempKeys(j)) / tmptotal & vbNewLine
                lakeCSstr = lakeCSstr & vbTab & climIndex & vbTab & lakeKeys(i) & vbTab & tempKeys(j) & vbTab & tempDict.Item(tempKeys(j)) / tmptotal & vbNewLine
                climIndex = climIndex + 1
            Next
        Next

        GetLakeInfo = outStr

        GoTo CleanUp

ShowError:
        MsgBox("Error GetLakeInfo: " & Err.Description, MsgBoxStyle.Exclamation)
        IfError = True
CleanUp:

    End Function



    '******************************************************************************
    'Subroutine: GetRoutingInfo  -  C290
    'Author:     Ragothaman Bhimarao
    'Purpose:    Routing Network information
    '*****************************************************************************
    Private Function GetRoutingInfo() As String
        On Error GoTo EH

        Dim fso As Scripting.FileSystemObject
        fso = CreateObject("Scripting.FileSystemObject")

        If Not (fso.FileExists(gMapInputFolder & "\Sequence.txt")) Then
            MsgBox("Delineate watershed before creating input")
            GetRoutingInfo = ""
            Exit Function
        End If

        'Create a dictionary for input data layer values
        InputDataDictionary = CreateObject("Scripting.Dictionary")
        FileOpen(200, gMapInputFolder & "\Sequence.txt", OpenMode.Input) ' Open file for input.

        Dim fromStr As String
        Dim toStr As String
        Dim distStr As String
        Dim typeStr As String
        Dim tmpstr As String
        tmpstr = ""
        Do While Not EOF(200)
            Input(200, fromStr)
            Input(200, toStr)
            Input(200, distStr)
            Input(200, typeStr)
            If fromStr <> "From:" Then
                tmpstr = tmpstr & vbTab & fromStr & vbTab & toStr & vbTab & distStr & vbNewLine
            End If
        Loop
        FileClose(200) ' Close file.
        fso = Nothing
        GetRoutingInfo = tmpstr

        GoTo CleanUp

EH:
        MsgBox("Error in GetRoutingInfo " & Err.Description)
        IfError = True
CleanUp:

    End Function

    '******************************************************************************
    'Subroutine: GetHydrologyParam  -  C300
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Hydrology Parameters information
    '*****************************************************************************
    Private Function GetHydrologyParam() As String
        Dim swfg As Short
        Dim swater As Object
        Dim gwrp As Object


        If InputDataDictionary("optConstSoilMoisture") Then
            swfg = 1
        ElseIf InputDataDictionary("optInputSoilMoisture") Then
            swfg = 2
        ElseIf InputDataDictionary("optFieldCapacity") Then
            swfg = 3
        End If

        If swfg = 1 Then
            swater = InputDataDictionary("InitialSoilMoistureConstant")
        ElseIf swfg = 2 Or swfg = 3 Then
            swater = 0
        End If

        If InputDataDictionary("cbxWhAEM") = 1 Then
            gwrp = InputDataDictionary("txtWhAEMDuration")
        Else
            gwrp = 0
        End If

        GetHydrologyParam = vbTab & swfg & vbTab & swater & vbTab & InputDataDictionary("InitialAccuSnowConstant") & vbTab & InputDataDictionary("aCNGrow") & vbTab & InputDataDictionary("bCNGrow") & vbTab & InputDataDictionary("aCNNonGrow") & vbTab & InputDataDictionary("bCNNonGrow") & vbTab & InputDataDictionary("HgLandUnsaturatedSoilDepth") & vbTab & InputDataDictionary("HgLandBedrockDepth") & vbTab & InputDataDictionary("InitialShallowGWConstant") & vbTab & InputDataDictionary("HydroGWRecessionCoeff") & vbTab & InputDataDictionary("HydroGWSeepageCoeff") & vbTab & gwrp & vbNewLine

    End Function


    '******************************************************************************
    'Subroutine: GetLakeParam  -  C310
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Lake Parameters information
    '*****************************************************************************
    Private Function GetLakeParam() As String
        GetLakeParam = vbTab & InputDataDictionary("HydroHydraulicCond") & vbTab & InputDataDictionary("HydroEvapoCoeff") & vbTab & InputDataDictionary("HydroOrificeDepth") & vbTab & InputDataDictionary("HydroOrificeDia") & vbTab & InputDataDictionary("HydroOrificeDischargeCoeff") & vbTab & InputDataDictionary("HydroWeirCrestLength") & vbTab & InputDataDictionary("HydroWeirDischargeCoeff") & vbNewLine
    End Function


    '******************************************************************************
    'Subroutine: GetWatershedParam  -  C320
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Watershed Parameters information
    '*****************************************************************************
    Private Function GetWatershedParam() As String
        GetWatershedParam = vbTab & InputDataDictionary("InitialSedimentPerviousLandConstant") & vbTab & InputDataDictionary("InitialSedimentImperviousLandConstant") & vbTab & InputDataDictionary("SedAccumulationRate") & vbTab & InputDataDictionary("SedDepletionRate") & vbTab & InputDataDictionary("SedYieldCapacity") & vbTab & InputDataDictionary("SedAlphaTc") & vbTab & InputDataDictionary("SedCalibCoeffAlpha") & vbTab & InputDataDictionary("SedRoutingCoeffBeta") & vbNewLine
    End Function


    '******************************************************************************
    'Subroutine: GetLakeSedimentParam  -  C330
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Lake Parameters information
    '*****************************************************************************
    Private Function GetLakeSedimentParam() As String
        GetLakeSedimentParam = vbTab & InputDataDictionary("SedEquilibriumConc") & vbTab & InputDataDictionary("SedDecayConstant") & vbTab & InputDataDictionary("SedPercentClay") & vbTab & InputDataDictionary("SedPercentSilt") & vbTab & InputDataDictionary("SedPercentSand") & vbNewLine
    End Function


    '******************************************************************************
    'Subroutine: GetAirDepositionHgParam  -  C340
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get air depostion mercury Parameters
    '*****************************************************************************
    Private Function GetAirDepositionHgParam() As String
        Dim adfg As Short
        Dim dd_f As Object
        Dim wdfg As Short
        Dim wd_v As Object
        Dim dd_m As Object
        Dim wd_m As Object
        Dim dd_d As Short
        Dim wd_d As Short
        If InputDataDictionary("chkConstant") = 1 Then
            adfg = 1

            dd_f = InputDataDictionary("HgDryConstant")

            If InputDataDictionary("optionHgWetConst") Then
                wdfg = 1
                wd_v = InputDataDictionary("HgWetConstant")
            ElseIf InputDataDictionary("optionHgWetPrcpConc") Then
                wdfg = 2
                wd_v = InputDataDictionary("HgWetPrcpConc")
            End If

            GetAirDepositionHgParam = vbTab & "adfg" & vbTab & "dd_f" & vbTab & "wdfg" & vbTab & "wd_v" & vbNewLine
            GetAirDepositionHgParam = GetAirDepositionHgParam & vbTab & adfg & vbTab & dd_f & vbTab & wdfg & vbTab & wd_v & vbNewLine

        ElseIf InputDataDictionary("chkGrid") = 1 Then
            adfg = 2

            dd_m = InputDataDictionary("HgDryMultiplier")
            wd_m = InputDataDictionary("HgWetMultiplier")

            GetAirDepositionHgParam = vbTab & "adfg" & vbTab & "dd_m" & vbTab & "wd_m" & vbNewLine
            GetAirDepositionHgParam = GetAirDepositionHgParam & vbTab & adfg & vbTab & dd_m & vbTab & wd_m & vbNewLine

        ElseIf InputDataDictionary("chkTime") = 1 Then
            adfg = 3

            If InputDataDictionary("cboDryDepTS") = "Daily" Then dd_d = 1
            If InputDataDictionary("cboDryDepTS") = "Monthly" Then dd_d = 2
            If InputDataDictionary("cboWetDepTS") = "Daily" Then wd_d = 1
            If InputDataDictionary("cboWetDepTS") = "Monthly" Then wd_d = 2


            GetAirDepositionHgParam = vbTab & "adfg" & vbTab & "dd_d" & vbTab & "wd_d" & vbNewLine
            GetAirDepositionHgParam = GetAirDepositionHgParam & vbTab & adfg & vbTab & dd_d & vbTab & wd_d & vbNewLine
        End If

    End Function

    '******************************************************************************
    'Subroutine: GetWatershedHgParam  -  C350
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get watershed mercury Parameters
    '*****************************************************************************
    Private Function GetWatershedHgParam() As String
        Dim shgfg As Short
        Dim smercury As Object

        If InputDataDictionary("optionInitialHgConstant") Then
            shgfg = 1
            smercury = InputDataDictionary("InitialConstantHg")
        ElseIf InputDataDictionary("optionSoilHgGrid") Then
            shgfg = 2
            smercury = InputDataDictionary("InitialSoilHgMultiplier")
        End If

        ''    GetWatershedHgParam = vbTab & "shgfg" & vbTab & "smercury" & vbTab & vbTab & "ssmercury" & vbTab & "zd" & vbTab & "zr" & vbTab & "pd" & vbTab & "kds" & vbTab & "krs" & vbTab & "ef" & vbTab & "bcf" & vbTab & "rd" & vbTab & "kcw" & vbTab & "crock" & vbNewLine
        GetWatershedHgParam = vbTab & shgfg & vbTab & smercury & vbTab & InputDataDictionary("InitialHgSaturatedSoilConstant") & vbTab & InputDataDictionary("HgLandSoilMixingDepth") & vbTab & InputDataDictionary("HgLandSoilReductionDepth") & vbTab & InputDataDictionary("HgLandSoilWaterPartitionCoeff") & vbTab & InputDataDictionary("HgLandSoilBaseReductionRate") & vbTab & InputDataDictionary("HgLandPollutantEnrichmentFactor") & vbTab & InputDataDictionary("HgLandBedrockDensity") & vbTab & InputDataDictionary("HgLandChemicalWeatheringRate") & vbTab & InputDataDictionary("HgLandBedrockHgConc") & vbTab & InputDataDictionary("HgWaterHgDecayInChannel") & vbTab & InputDataDictionary("HgMethylHgFraction") & vbNewLine

    End Function


    '******************************************************************************
    'Subroutine: GetForestHgParam  -  C360
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Forest mercury Parameters
    '*****************************************************************************
    Private Function GetForestHgParam() As String
        GetForestHgParam = vbTab & InputDataDictionary("HgLandAnnualEvapo") & vbTab & InputDataDictionary("HgLandLeafInterFraction") & vbTab & InputDataDictionary("HgLandLeafAdhereFraction") & vbTab & InputDataDictionary("InitialLeafLitterConstant") & vbTab & InputDataDictionary("HgLandAirPlantBioConc") & vbTab & InputDataDictionary("HgLandAirHgConc") & vbNewLine
    End Function



    '******************************************************************************
    'Subroutine: GetLakeHgParam  -  C370
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Lake mercury Parameters
    '*****************************************************************************
    Private Function GetLakeHgParam() As String
        GetLakeHgParam = vbTab & InputDataDictionary("HgWaterAlphaW") & vbTab & InputDataDictionary("HgWaterBetaW") & vbTab & InputDataDictionary("HgWaterKWR") & vbTab & InputDataDictionary("HgWaterKWM") & vbTab & InputDataDictionary("HgWaterVSB") & vbTab & InputDataDictionary("HgWaterVRS") & vbTab & InputDataDictionary("HgWaterKDsw") & vbTab & InputDataDictionary("HgWaterKbio") & vbTab & InputDataDictionary("HgWaterCbio") & vbTab & InputDataDictionary("HgLandSoilParticleDensity") & vbNewLine
    End Function


    '******************************************************************************
    'Subroutine: GetForestHgParam  -  C380
    'Author:     Ragothaman Bhimarao
    'Purpose:    Get Forest mercury Parameters
    '*****************************************************************************
    Private Function GetBenthicHgParam() As String
        GetBenthicHgParam = vbTab & InputDataDictionary("HgBenthicAlphaB") & vbTab & InputDataDictionary("HgBenthicBetaB") & vbTab & InputDataDictionary("HgBenthicKBR") & vbTab & InputDataDictionary("HgBenthicKBM") & vbTab & InputDataDictionary("HgWaterTheta_bs") & vbTab & InputDataDictionary("HgBenthicVbur") & vbTab & InputDataDictionary("HgBenthicSedimentDepth") & vbTab & InputDataDictionary("HgBenthicKDbs") & vbTab & InputDataDictionary("HgBenthicCbs") & vbTab & InputDataDictionary("HgBenthicPorewaterDiffusionCoeff") & vbNewLine
    End Function



    'Retrieves row count of input raster
    Public Function ReturnRasterRowCount(ByRef strname As String, Optional ByRef strpath As String = "") As Short
        Dim pRasterBnd As IRasterBand
        Dim pBC As IRasterBandCollection
        Dim pTmpRaster As IRaster

        Dim pWSRasterLayer As IRasterLayer
        If strpath = "" Then
            pTmpRaster = OpenRasterDatasetFromDisk(strname)
        ElseIf strpath = "Raster" Then
            pWSRasterLayer = GetInputRasterLayer("subwatershed")

            If pWSRasterLayer Is Nothing Then
                MsgBox("Not found Subwatershed layer.")
                Exit Function
            End If

            pTmpRaster = pWSRasterLayer.Raster
        Else
            pTmpRaster = OpenRasterDatasetFromDisk2(strpath, strname)
        End If

        Dim NumOfValues As Short
        Dim pTable As iTable
        If Not (pTmpRaster Is Nothing) Then
            pBC = pTmpRaster
            pRasterBnd = pBC.Item(0)

            ' Attribute table
            pTable = pRasterBnd.AttributeTable
            NumOfValues = pTable.RowCount(Nothing)
            ReturnRasterRowCount = NumOfValues
        Else
            ReturnRasterRowCount = 0
        End If

CleanUp:
        pTmpRaster = Nothing
        pBC = Nothing
        pRasterBnd = Nothing
    End Function

    'Retrieves row count of input shapefile
    Public Function ReturnShapeRowCount(ByRef strpath As String, ByRef strname As String) As Short
        Dim pFeatCls As IFeatureClass
        pFeatCls = OpenShapeFile(strpath, strname)

        Dim pTable As iTable
        pTable = pFeatCls

        Dim NumOfValues As Short
        NumOfValues = pTable.RowCount(Nothing)
    End Function

    'Create Climate file contents
    Public Function CreateStationInfo(ByRef valColl As Collection, ByRef SNColl As Collection, Optional ByRef LatiColl As Collection = Nothing) As String
        Dim i As Short
        Dim tmpstr As String
        tmpstr = ""
        If LatiColl Is Nothing Then
            For i = 1 To SNColl.Count()
                tmpstr = tmpstr & vbTab & i & vbTab & valColl.Item(i) & vbTab & SNColl.Item(i) & vbNewLine
            Next
        Else
            For i = 1 To SNColl.Count()
                tmpstr = tmpstr & vbTab & i & vbTab & valColl.Item(i) & vbTab & SNColl.Item(i) & vbTab & LatiColl.Item(i) & vbNewLine
            Next
        End If
        CreateStationInfo = tmpstr
    End Function


    'Check if all the files in the dictionary exist
    Public Function AllFilesExist(ByVal tmpDict As Generic.Dictionary(Of Integer, String)) As String
        Dim lst As Generic.List(Of Integer) = {101, 102, 103, 104, 302}
        Dim tmpstr As String = ""
        For Each kv As KeyValuePair(Of Integer, String) In tmpDict
            If Not My.Computer.FileSystem.FileExists(kv.Value) Then
                ErrorMsg(s & " does not exist.")
                IfError = True
                Return ""
            End If
            Dim filetype As Integer = 1
            If Not lst.Contains(kv.Key) Then filetype = 2
            tmpstr &= StringFormat("\t{0}\t{1}\t{2}\n", kv.Key, filetype, kv.Value)
        Next
    End Function

    Public Function GetCellArea() As Double
        Dim g As MapWinGIS.Grid
        Try
            g.Open(GetInputLayer("DEM"))
            Return g.Header.dX * g.Header.dY
        Catch ex As Exception
            ErrorMsg(, ex)
            Return 0
        Finally
            g.Close()
        End Try
    End Function

    Private Sub GetPointInfo()
        Dim GetPixelValue As Object
        Dim pFeatureCenter As Object
        Dim pFeatureLayer As IFeatureLayer
        Dim pRasterLayer As IRasterLayer

        pFeatureLayer = GetInputFeatureLayer(InputDataDictionary("PointSources"))
        pRasterLayer = GetInputRasterLayer("Subwatershed")

        Dim pRaster As IRaster
        pRaster = pRasterLayer.Raster

        If pRaster Is Nothing Then
            MsgBox("Error no raster found")
            Exit Sub
        End If

        Dim resultStr As String
        resultStr = ""

        Dim pFeatureClass As IFeatureClass
        pFeatureClass = pFeatureLayer.FeatureClass

        Dim pFeatureCursor As IFeatureCursor
        pFeatureCursor = pFeatureClass.Search(Nothing, False)

        Dim pFeature As IFeature
        '    Dim pFeatureCenter As esriGeometry.IPoint
        Dim cellValue As Double

        Do
            pFeature = pFeatureCursor.NextFeature

            If (Not pFeature Is Nothing) Then
                pFeatureCenter = pFeature.Shape
                cellValue = GetPixelValue(pFeatureCenter, pRaster)
                resultStr = resultStr & pFeature.Value(pFeatureClass.FindField("Station")) & ": " & cellValue & vbNewLine
            End If
        Loop Until pFeature Is Nothing

    End Sub

    Function MakePnt(ByRef dX As Double, ByRef dY As Double) As IPnt
        MakePnt = New DblPnt
        MakePnt.SetCoords(dX, dY)
    End Function

    'Function returns the dictionary of lake id and assesspointid
    Public Function GetLakeIDAssessIdDictionary() As Generic.Dictionary(Of String, String)
        Try
            'is this needed? don't have lakes grid!!!!


            'Dim GetPixelNoDataMaskVal As Object
            'Dim GetPixelValue As Object
            'Dim pPoint As Object

            'Dim LakeIDAssessIdDictionary As Scripting.Dictionary
            'LakeIDAssessIdDictionary = New Scripting.Dictionary

            'Dim pAssessFLayer As IFeatureLayer
            'pAssessFLayer = GetInputFeatureLayer("AssessPoints")

            'Dim pAssessFClass As IFeatureClass
            'pAssessFClass = pAssessFLayer.FeatureClass

            'Dim pAssessCursor As IFeatureCursor
            'pAssessCursor = pAssessFClass.Search(Nothing, False)

            'Dim lakeId As Integer
            'Dim assessId As Integer

            'Dim pFeature As IFeature
            'pFeature = pAssessCursor.NextFeature

            'Dim pLakeRaster As IRaster
            'pLakeRaster = OpenRasterDatasetFromDisk2(gMapTempFolder & "\SUB", "LakesGrid")

            'Do While Not pFeature Is Nothing
            '    pPoint = pFeature.Value(pAssessFClass.FindField("Shape"))
            '    assessId = CInt(pFeature.Value(pAssessFClass.FindField("ID")))
            '    If GetPixelNoDataMaskVal(pPoint, pLakeRaster) = 1 Then
            '        lakeId = CInt(GetPixelValue(pPoint, pLakeRaster))
            '        LakeIDAssessIdDictionary.let_Item(lakeId, assessId)
            '    End If
            '    pFeature = pAssessCursor.NextFeature
            'Loop
            'GetLakeIDAssessIdDictionary = LakeIDAssessIdDictionary
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function
End Module