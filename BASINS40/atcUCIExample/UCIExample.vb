Imports MapWinUtility
Imports System.Collections.Specialized
Imports atcData
Imports atcUtility
Imports atcHspfBinOut
Imports atcWDM

Public Module UCIExample

    Public Sub Main()
        Dim lWorkingDir As String = "C:\hgl\test\"
        ChDir(lWorkingDir)

        Logger.StartToFile(lWorkingDir & "UCIExample.log")

        'hspfmsg.mdb is a database of hspf parameters and tables, 
        'you must do this before opening a UCI
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        Dim lUci As New atcUCI.HspfUci
        'this name is going to look somewhat bizarre, but it is the easiest way
        'to open a UCI for the purposes you'll need.
        lUci.FastReadUciForStarter(lMsg, "gee_v2.uci")

        'then you can start getting stuff out of the UCI like this
        Dim lStartYear As Integer = lUci.GlobalBlock.SDate(0)
        Dim lStartMonth As Integer = lUci.GlobalBlock.SDate(1)
        Dim lStartDay As Integer = lUci.GlobalBlock.SDate(2)
        Dim lStartHour As Integer = lUci.GlobalBlock.SDate(3)
        Dim lStartMin As Integer = lUci.GlobalBlock.SDate(4)
        Dim lEndYear As Integer = lUci.GlobalBlock.EDate(0)
        Dim lEndMonth As Integer = lUci.GlobalBlock.EDate(1)
        Dim lEndDay As Integer = lUci.GlobalBlock.EDate(2)
        Dim lEndHour As Integer = lUci.GlobalBlock.EDate(3)
        Dim lEndMin As Integer = lUci.GlobalBlock.EDate(4)

        'get the general run description
        Dim lRunDescription As String = lUci.GlobalBlock.RunInf.Value

        'loop thru each file in the files block
        Dim lFile As atcUCI.HspfFile
        Dim lWDMs As New Collection
        Dim lHBNs As New Collection
        For lIndex As Integer = 1 To lUci.FilesBlock.Count
            lFile = lUci.FilesBlock.Value(lIndex)
            If Left(lFile.Typ, 3) = "WDM" Then
                'make a collection of WDM files referenced
                lWDMs.Add(lFile.Name, lFile.Typ)   'make the type the key (WDM1, WDM2, etc); we'll need that later
            ElseIf lFile.Typ = "BINO" Then
                'make a collection of HBN files referenced
                lHBNs.Add(lFile.Name)
            End If
        Next lIndex

        'find all the operations in the UCI file
        For Each lOper As atcUCI.HspfOperation In lUci.OpnSeqBlock.Opns
            Dim lOperName As String = lOper.Name '"PERLND" for instance
            Dim lOperId As Integer = lOper.Id '101 for instance
            Dim lOperDesc As String = lOper.Description '"forest" for instance, or "little river"
        Next lOper

        For Each lOper As atcUCI.HspfOperation In lUci.OpnSeqBlock.Opns
            'for each operation you can see if it has any output written to WDM like this
            For Each lTarget As atcUCI.HspfConnection In lOper.Targets
                If Left(lTarget.Target.VolName, 3) = "WDM" Then
                    'found where this operation writes to WDM

                    'store the parameter name, for example "RCHRES6 HYDR:RO(1,1)"
                    Dim lParameter As String = lOper.Name & lOper.Id & " " & lTarget.Source.Group & ":" & lTarget.Source.Member & _
                                               "(" & lTarget.Source.MemSub1 & "," & lTarget.Source.MemSub2 & ")"
                    'store also the WDM file where this timeseries is written to
                    Dim lWDMName As String = lWDMs(lTarget.Target.VolName)
                    'and the WDM data set number 
                    Dim lWDMDsn As Integer = lTarget.Target.VolId
                    'and the units?

                End If
            Next lTarget

            'for each operation you can see what operations contribute to it
            If lOper.Name = "RCHRES" Then
                For Each lSource As atcUCI.HspfConnection In lOper.Sources
                    ' this is a source
                    If lSource.Source.VolName = "PERLND" Or lSource.Source.VolName = "IMPLND" Then
                        Dim NumAcres As Double = lSource.MFact
                    ElseIf lSource.Source.VolName = "RCHRES" Then
                        ' this is a rchres to rchres connection
                    End If
                Next lSource
            End If
        Next lOper


        'to generate the list of timeseries in the HBN file:
        Dim lHBNDataSource As New atcHspfBinOut.atcTimeseriesFileHspfBinOut
        If lHBNDataSource.Open("gee.hbn") Then
            Dim lDataSetCount As Integer = lHBNDataSource.DataSets.Count
            Dim lHBNDataSet As atcTimeseries = lHBNDataSource.DataSets(1)
            Dim lDataSetDescription As String = lHBNDataSet.ToString()
            Dim lParameter As String = lHBNDataSet.Attributes.GetValue("location") & " " & lHBNDataSet.Attributes.GetValue("constituent")
            'to get values out of the HBN data set
            Dim lNumValues As Integer = lHBNDataSet.numValues
            Dim lValue As Double = lHBNDataSet.Values(1)  'get the first value
            Dim lJulianDate As Double = lHBNDataSet.Dates.Values(1)  'date associated with the first value 
            Dim lUnits As String = lHBNDataSet.Attributes.GetDefinedValue("Units").Value
            Dim lDate(5) As Integer
            J2Date(lJulianDate, lDate)
            Dim lYear As Integer = lDate(0)
            Dim lMon As Integer = lDate(1)
            Dim lDay As Integer = lDate(2)
            Dim lHour As Integer = lDate(3)
            'clean up
            lHBNDataSet = Nothing
            lHBNDataSource = Nothing
        End If


        'later on when you want to get the data out of a WDM file, do this:
        Dim lWDMDataSource As New atcWDM.atcDataSourceWDM
        If lWDMDataSource.Open("calibration.wdm") Then    'use the WDM file name
            'successfully opened the wdm
            Dim lWDMDataSetIndex As Integer = lWDMDataSource.DataSets.IndexFromKey(102)  'use the WDM Dsn
            Dim lWDMDataSet As atcTimeseries = lWDMDataSource.DataSets.ItemByIndex(lWDMDataSetIndex)
            'get the values out of the WDM data set
            Dim lNumValues As Integer = lWDMDataSet.numValues
            Dim lValue As Double = lWDMDataSet.Values(1)  'get the first value
            Dim lJulianDate As Double = lWDMDataSet.Dates.Values(1)  'date associated with the first value 
            Dim lDate(5) As Integer
            J2Date(lJulianDate, lDate)
            Dim lYear As Integer = lDate(0)
            Dim lMon As Integer = lDate(1)
            Dim lDay As Integer = lDate(2)
            Dim lHour As Integer = lDate(3)
            'clean up
            lWDMDataSet = Nothing
            lWDMDataSource = Nothing
        End If

    End Sub
End Module
