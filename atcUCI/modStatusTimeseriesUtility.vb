Option Strict Off
Option Explicit On

Imports atcuci.HspfStatus.HspfStatusReqOptUnnEnum

Module modStatusTimeseriesUtility
    'Copyright 2006 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Public Sub UpdateInputTimeseriesDuranl(ByRef aOperation As HspfOperation, _
                                           ByRef aTimserStatus As HspfStatus)
        'only timser for duranl is the input timser
        aTimserStatus.Change("INPUT:TIMSER", 1, HspfStatusRequired)
    End Sub

    Public Sub UpdateOutputTimeseriesDuranl(ByRef aOperation As HspfOperation, _
                                            ByRef aTimserStatus As HspfStatus)
        'no output timsers for duranl
    End Sub

    Public Sub UpdateInputTimeseriesCopy(ByRef aOperation As HspfOperation, _
                                         ByRef aTimserStatus As HspfStatus)
        Dim lNMean, lNPoint As Integer

        If aOperation.TableExists("TIMESERIES") Then
            With aOperation.Tables.Item("TIMESERIES")
                lNPoint = .ParmValue("NPT")
                lNMean = .ParmValue("NMN")
            End With
        Else
            lNPoint = 0
            lNMean = 0
        End If
        For lPointIndex As Integer = 1 To lNPoint
            aTimserStatus.Change("INPUT:POINT", lPointIndex, HspfStatusOptional)
        Next lPointIndex
        For lMeanIndex As Integer = 1 To lNMean
            aTimserStatus.Change("INPUT:MEAN", lMeanIndex, HspfStatusOptional)
        Next lMeanIndex
    End Sub

    Public Sub UpdateOutputTimeseriesCopy(ByRef aOperation As HspfOperation, _
                                          ByRef aTimserStatus As HspfStatus)
        Dim lNMean, lNPoint As Integer

        If aOperation.TableExists("TIMESERIES") Then
            With aOperation.Tables.Item("TIMESERIES")
                lNPoint = .ParmValue("NPT")
                lNMean = .ParmValue("NMN")
            End With
        Else
            lNPoint = 0
            lNMean = 0
        End If
        For lPointIndex As Integer = 1 To lNPoint
            aTimserStatus.Change("OUTPUT:POINT", lPointIndex, HspfStatusOptional)
        Next lPointIndex
        For lMeanIndex As Integer = 1 To lNMean
            aTimserStatus.Change("OUTPUT:MEAN", lMeanIndex, HspfStatusOptional)
        Next lMeanIndex
    End Sub

    Public Sub UpdateInputTimeseriesPltgen(ByRef aOperation As HspfOperation, _
                                           ByRef aTimserStatus As HspfStatus)
        'only timser for pltgen is the input timser
        Dim lNMean, lNPoint As Integer

        If aOperation.TableExists("PLOTINFO") Then
            With aOperation.Tables.Item("PLOTINFO")
                lNPoint = .ParmValue("NPT")
                lNMean = .ParmValue("NMN")
            End With
        Else
            lNPoint = 0
            lNMean = 0
        End If
        For lPointIndex As Integer = 1 To lNPoint
            aTimserStatus.Change("INPUT:POINT", lPointIndex, HspfStatusRequired)
        Next lPointIndex
        For lMeanIndex As Integer = 1 To lNMean
            aTimserStatus.Change("INPUT:MEAN", lMeanIndex, HspfStatusRequired)
        Next lMeanIndex
    End Sub

    Public Sub UpdateOutputTimeseriesPltgen(ByRef aOperation As HspfOperation, _
                                            ByRef aTimserStatus As HspfStatus)
        'no output timsers for pltgen
    End Sub

    Public Sub UpdateInputTimeseriesDisply(ByRef aOperation As HspfOperation, _
                                           ByRef aTimserStatus As HspfStatus)
        'only timser for disply is the input timser
        aTimserStatus.Change("INPUT:TIMSER", 1, HspfStatusRequired)
    End Sub

    Public Sub UpdateOutputTimeseriesDisply(ByRef aOperation As HspfOperation, _
                                            ByRef aTimserStatus As HspfStatus)
        'no output timsers for disply
    End Sub

    Public Sub UpdateInputTimeseriesGener(ByRef aOperation As HspfOperation, _
                                          ByRef aTimserStatus As HspfStatus)
        'two possible input timsers for gener
        Dim lOpCode As Integer

        If aOperation.TableExists("OPCODE") Then
            lOpCode = aOperation.Tables.Item("OPCODE").ParmValue("OPCODE")
        Else
            lOpCode = 0
        End If
        If lOpCode > 0 And lOpCode <> 24 Then
            aTimserStatus.Change("INPUT:ONE", 1, HspfStatusRequired)
        End If
        If lOpCode > 15 And lOpCode < 24 Then
            aTimserStatus.Change("INPUT:TWO", 1, HspfStatusRequired)
        End If
    End Sub

    Public Sub UpdateOutputTimeseriesGener(ByRef aOperation As HspfOperation, _
                                           ByRef aTimserStatus As HspfStatus)
        'only one output timser for gener
        aTimserStatus.Change("OUTPUT:TIMSER", 1, HspfStatusOptional)
    End Sub

    Public Sub UpdateInputTimeseriesMutsin(ByRef aOperation As HspfOperation, _
                                           ByRef aTimserStatus As HspfStatus)
        'no input timsers for mutsin
    End Sub

    Public Sub UpdateOutputTimeseriesMutsin(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        'only timser for mutsin is the output timser
        Dim nmn, npt, i As Integer

        If O.TableExists("MUTSINFO") Then
            With O.Tables.Item("MUTSINFO")
                npt = .ParmValue("NPT")
                nmn = .ParmValue("NMN")
            End With
        Else
            npt = 0
            nmn = 0
        End If
        For i = 1 To npt
            TimserStatus.Change("OUTPUT:POINT", i, HspfStatusRequired)
        Next i
        For i = 1 To nmn
            TimserStatus.Change("OUTPUT:MEAN", i, HspfStatusRequired)
        Next i
    End Sub

    Public Sub UpdateInputTimeseriesReport(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        Dim ncon, i As Integer
        If O.TableExists("REPORT-FLAGS") Then
            ncon = O.Tables.Item("REPORT-FLAGS").ParmValue("NCON")
        Else
            ncon = 0
        End If
        For i = 1 To ncon
            TimserStatus.Change("INPUT:TIMSER", i, HspfStatusRequired)
        Next i
    End Sub

    Public Sub UpdateOutputTimeseriesReport(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        'no output timsers for report
    End Sub

    Public Sub UpdateInputTimeseriesBmprac(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        'input timsers for bmprac
        Dim nGqual, nCons, i As Integer

        If O.TableExists("GEN-INFO") Then
            With O.Tables.Item("GEN-INFO")
                nCons = .ParmValue("NCONS")
                nGqual = .ParmValue("NGQUAL")
            End With
        Else
            nCons = 0
            nGqual = 0
        End If

        'group inflow
        TimserStatus.Change("INFLOW:IVOL", 1, HspfStatusOptional)
        If Not O.Uci.CategoryBlock Is Nothing Then
            'have category block
            For i = 1 To O.Uci.CategoryBlock.Count
                TimserStatus.Change("INFLOW:CIVOL", i, HspfStatusOptional)
            Next i
        End If
        For i = 1 To nCons
            TimserStatus.Change("INFLOW:ICON", i, HspfStatusOptional)
        Next i
        TimserStatus.Change("INFLOW:IHEAT", 1, HspfStatusOptional)
        For i = 1 To 3
            TimserStatus.Change("INFLOW:ISED", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change("INFLOW:IDQAL", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change2("INFLOW:ISQAL", 1, i, HspfStatusOptional)
            TimserStatus.Change2("INFLOW:ISQAL", 2, i, HspfStatusOptional)
            TimserStatus.Change2("INFLOW:ISQAL", 3, i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("INFLOW:IOX", i, HspfStatusOptional)
        Next i
        For i = 1 To 4
            TimserStatus.Change("INFLOW:IDNUT", i, HspfStatusOptional)
        Next i
        For i = 1 To 3
            TimserStatus.Change2("INFLOW:ISNUT", i, 1, HspfStatusOptional)
            TimserStatus.Change2("INFLOW:ISNUT", i, 2, HspfStatusOptional)
        Next i
        For i = 1 To 5
            TimserStatus.Change("INFLOW:IPLK", i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("INFLOW:IPH", i, HspfStatusOptional)
        Next i

    End Sub

    Public Sub UpdateOutputTimeseriesBmprac(ByRef O As HspfOperation, ByRef TimserStatus As HspfStatus)
        'output timsers for bmprac
        Dim nGqual, nCons, i As Integer

        If O.TableExists("GEN-INFO") Then
            With O.Tables.Item("GEN-INFO")
                nCons = .ParmValue("NCONS")
                nGqual = .ParmValue("NGQUAL")
            End With
        Else
            nCons = 0
            nGqual = 0
        End If

        'group receiv
        TimserStatus.Change("RECEIV:IVOL", 1, HspfStatusOptional)
        If Not O.Uci.CategoryBlock Is Nothing Then
            'have category block
            For i = 1 To O.Uci.CategoryBlock.Count
                TimserStatus.Change("RECEIV:CIVOL", i, HspfStatusOptional)
            Next i
        End If
        For i = 1 To nCons
            TimserStatus.Change("RECEIV:ICON", 1, HspfStatusOptional)
        Next i
        TimserStatus.Change("RECEIV:IHEAT", 1, HspfStatusOptional)
        For i = 1 To 3
            TimserStatus.Change("RECEIV:ISED", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change("RECEIV:IDQAL", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change2("RECEIV:ISQAL", 1, i, HspfStatusOptional)
            TimserStatus.Change2("RECEIV:ISQAL", 2, i, HspfStatusOptional)
            TimserStatus.Change2("RECEIV:ISQAL", 3, i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("RECEIV:IOX", i, HspfStatusOptional)
        Next i
        For i = 1 To 4
            TimserStatus.Change("RECEIV:IDNUT", i, HspfStatusOptional)
        Next i
        For i = 1 To 3
            TimserStatus.Change2("RECEIV:ISNUT", i, 1, HspfStatusOptional)
            TimserStatus.Change2("RECEIV:ISNUT", i, 2, HspfStatusOptional)
        Next i
        For i = 1 To 5
            TimserStatus.Change("RECEIV:IPLK", i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("RECEIV:IPH", i, HspfStatusOptional)
        Next i

        'group roflow
        TimserStatus.Change("ROFLOW:ROVOL", 1, HspfStatusOptional)
        If Not O.Uci.CategoryBlock Is Nothing Then
            'have category block
            For i = 1 To O.Uci.CategoryBlock.Count
                TimserStatus.Change("ROFLOW:CROVOL", i, HspfStatusOptional)
            Next i
        End If
        For i = 1 To nCons
            TimserStatus.Change("ROFLOW:ROCON", 1, HspfStatusOptional)
        Next i
        TimserStatus.Change("ROFLOW:ROHEAT", 1, HspfStatusOptional)
        For i = 1 To 3
            TimserStatus.Change("ROFLOW:ROSED", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change("ROFLOW:RODQAL", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change2("ROFLOW:ROSQAL", 1, i, HspfStatusOptional)
            TimserStatus.Change2("ROFLOW:ROSQAL", 2, i, HspfStatusOptional)
            TimserStatus.Change2("ROFLOW:ROSQAL", 3, i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("ROFLOW:ROOX", i, HspfStatusOptional)
        Next i
        For i = 1 To 4
            TimserStatus.Change("ROFLOW:RODNUT", i, HspfStatusOptional)
        Next i
        For i = 1 To 3
            TimserStatus.Change2("ROFLOW:ROSNUT", i, 1, HspfStatusOptional)
            TimserStatus.Change2("ROFLOW:ROSNUT", i, 2, HspfStatusOptional)
        Next i
        For i = 1 To 5
            TimserStatus.Change("ROFLOW:ROPLK", i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("ROFLOW:ROPH", i, HspfStatusOptional)
        Next i

        'group remove
        TimserStatus.Change("REMOVE:RMVOL", 1, HspfStatusOptional)
        If Not O.Uci.CategoryBlock Is Nothing Then
            'have category block
            For i = 1 To O.Uci.CategoryBlock.Count
                TimserStatus.Change("REMOVE:CRMVOL", i, HspfStatusOptional)
            Next i
        End If
        For i = 1 To nCons
            TimserStatus.Change("REMOVE:RMCON", 1, HspfStatusOptional)
        Next i
        TimserStatus.Change("REMOVE:RMHEAT", 1, HspfStatusOptional)
        For i = 1 To 3
            TimserStatus.Change("REMOVE:RMSED", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change("REMOVE:RMDQAL", i, HspfStatusOptional)
        Next i
        For i = 1 To nGqual
            TimserStatus.Change2("REMOVE:RMSQAL", 1, i, HspfStatusOptional)
            TimserStatus.Change2("REMOVE:RMSQAL", 2, i, HspfStatusOptional)
            TimserStatus.Change2("REMOVE:RMSQAL", 3, i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("REMOVE:RMOX", i, HspfStatusOptional)
        Next i
        For i = 1 To 4
            TimserStatus.Change("REMOVE:RMDNUT", i, HspfStatusOptional)
        Next i
        For i = 1 To 3
            TimserStatus.Change2("REMOVE:RMSNUT", i, 1, HspfStatusOptional)
            TimserStatus.Change2("REMOVE:RMSNUT", i, 2, HspfStatusOptional)
        Next i
        For i = 1 To 5
            TimserStatus.Change("REMOVE:RMPLK", i, HspfStatusOptional)
        Next i
        For i = 1 To 2
            TimserStatus.Change("REMOVE:RMPH", i, HspfStatusOptional)
        Next i

    End Sub
End Module