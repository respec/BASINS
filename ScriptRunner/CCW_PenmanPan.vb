Imports MapWindow.Interfaces
Imports atcUtility
Imports atcData
Imports atcMetCmp
Imports atcWDM
Imports atcData.atcDataManager
Imports MapWinUtility
Public Module CCW_PenmanPan
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'Calculate Penman Pan Evaporation

        'Open NLDAS WDM File
        Dim lWdmFileName As String = "C:\BASINS45\modelout\CCW\nldas.wdm"
        Dim lWdmDataSource As New atcDataSourceWDM
        If OpenDataSource(lWdmFileName) Then lWdmDataSource = DataSourceBySpecification(lWdmFileName)
        Dim GridLocations As String() = {"X161Y116", "X161Y117", "X161Y118", "X162Y113", "X162Y114", "X162Y115", "X162Y116",
                                                 "X162Y117", "X162Y118", "X163Y113", "X163Y114", "X163Y115", "X163Y116", "X163Y117",
                                                 "X163Y118", "X164Y113", "X164Y114", "X164Y115", "X164Y116"}

        Logger.Dbg("Starting the Penman Pan Calculations. Number of datasets in WDM file are " & lWdmDataSource.DataSets.Count)
        Dim PETDSNCounter As Integer = 0
        For Each GridLocation As String In GridLocations
            Dim lTemp As atcTimeseries = lWdmDataSource.DataSets.FindData("Location", GridLocation).FindData("Constituent", "ATEM")(0)
            'Logger.Dbg("Read the Temp Dataset. Number of Values = " & lTemp.numValues)
            Dim lTMinTSer As atcTimeseries = Aggregate(lTemp, atcTimeUnit.TUDay, 1, atcTran.TranMin)
            Dim lTMaxTSer As atcTimeseries = Aggregate(lTemp, atcTimeUnit.TUDay, 1, atcTran.TranMax)
            Dim lsRadTSer As atcTimeseries = lWdmDataSource.DataSets.FindData("Location", GridLocation).FindData("Constituent", "SOLR")(0)
            lsRadTSer = Aggregate(lsRadTSer, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            Dim lDewPTSer As atcTimeseries = lWdmDataSource.DataSets.FindData("Location", GridLocation).FindData("Constituent", "DEWP")(0)
            lDewPTSer = Aggregate(lDewPTSer, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            'I am guessing that Average Daily Dew Point Temperature is needed for this calculation.

            Dim lWindTSer As atcTimeseries = lWdmDataSource.DataSets.FindData("Location", GridLocation).FindData("Constituent", "WIND")(0)
            lWindTSer = Aggregate(lWindTSer, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
            Dim lMetCmpTS As atcTimeseries = PanEvaporationTimeseriesComputedByPenman(lTMinTSer, lTMaxTSer, lsRadTSer, lDewPTSer, lWindTSer, Nothing)
            Logger.Dbg("Annual Penman Pan PET at location " & GridLocation & " is " & lMetCmpTS.Attributes.GetDefinedValue("SumAnnual").Value)
            Dim lLatitude As Double = lTemp.Attributes.GetDefinedValue("Latitude").Value

            lMetCmpTS = DisSolPet(lMetCmpTS, Nothing, 2, lLatitude)
            lMetCmpTS.Attributes.SetValue("ID", 500 + PETDSNCounter)
            lMetCmpTS.Attributes.AddHistory("Calculated using Penman Pen")
            lMetCmpTS.Attributes.SetValue("Description", "Penman Pan")
            lWdmDataSource.AddDataset(lMetCmpTS, atcDataSource.EnumExistAction.ExistAskUser)
            Dim lCTS() As Double = {0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055,
                                    0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.0055, 0.055}

            Dim lHamonCmTS As atcTimeseries = PanEvaporationTimeseriesComputedByHamon(lTMinTSer, lTMaxTSer, Nothing, True, lLatitude, lCTS)
            Logger.Dbg("Annual Hamon PET at location " & GridLocation & " is " & lHamonCmTS.Attributes.GetDefinedValue("SumAnnual").Value)
            lHamonCmTS = DisSolPet(lHamonCmTS, Nothing, 2, lLatitude)
            lHamonCmTS.Attributes.SetValue("ID", 600 + PETDSNCounter)
            lHamonCmTS.Attributes.AddHistory("Calculated using Hamon")
            lHamonCmTS.Attributes.SetValue("Description", "Hamon")
            lWdmDataSource.AddDataset(lHamonCmTS, atcDataSource.EnumExistAction.ExistAskUser)
            PETDSNCounter += 1
        Next GridLocation


    End Sub
End Module
