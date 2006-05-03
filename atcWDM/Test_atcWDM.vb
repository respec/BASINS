Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports atcUtility.modReflection
Imports atcData
Imports MapWinUtility
Imports atcUtility

Friend Module dbg
    Dim init As Boolean = False
    Public Sub Msg(ByVal aMsg As String)
        If Not init Then
            ChDriveDir("c:\test\atcWDM\data")
            SaveFileString("test.lis", "Begin atcWdm test" & vbCrLf)
            Try
                Kill("error.fil")
            Catch
            End Try
            HassLibs.F90_MSG("WRITE", 5)
            init = True
        End If
        AppendFileString("test.lis", aMsg & vbCrLf)
    End Sub
    Public Sub FileSetup()
        Msg("dbg.FileSetup start")
        Kill("shena.wdm")
        FileCopy("save\shena.wdm", "shena.wdm")
        Msg("dbg.FileSetup done")
    End Sub
End Module

<TestFixture()> Public Class Test_Builder
    Public Sub TestsAllPresent()
        Dim lTestBuildStatus As String
        lTestBuildStatus = BuildMissingTests("c:\test\")
        Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
    End Sub
End Class

<TestFixture()> Public Class Test_atcDataSourceWDM
    Dim pWdmName As String = "C:\test\atcWDM\data\shena.wdm"

    <TestFixtureSetUp()> Public Sub init()
        'MsgBox("Start test of atcDataSourceWDM")
        dbg.FileSetup()
    End Sub

    Public Sub TestOpen()
        Dim lWdm As New atcDataSourceWDM

        dbg.Msg("atcDataSourceWdm:TestOpen")
        Assert.IsTrue(lWdm.Open(pWdmName))
        dbg.Msg("                 Timeseries.Count:" & lWdm.DataSets.Count)
        dbg.Msg("                 Name:" & lWdm.Name)
        dbg.Msg("                 FileName:" & lWdm.Specification)
        lWdm = Nothing
    End Sub

    Public Sub TestAddTimeseries()
        Dim lWdm As New atcDataSourceWDM
        Dim lTimser As New atcTimeseries(lWdm)

        dbg.Msg("atcDataSourceWdm:TestAddTimeseries")
        lWdm.Open(pWdmName)
        dbg.Msg("                 TestAddTimeseries:Count:" & lWdm.DataSets.Count)
        Assert.IsTrue(lWdm.AddDataset(lTimser))
        lWdm = Nothing
    End Sub

    Public Sub TestSave()
        'Save()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestReadData()
        'ReadData()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_CanSave()
        'get_CanSave()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_CanOpen()
        'get_CanOpen()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Timeseries()
        'get_Timeseries()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testset_FileName()
        'set_FileName()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_FileName()
        'get_FileName()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_FileFilter()
        'get_FileFilter()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMessage()
        'Message()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestItemClicked()
        'ItemClicked()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestShapesSelected()
        'ShapesSelected()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLayersCleared()
        'LayersCleared()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLayerSelected()
        'LayerSelected()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLayerRemoved()
        'LayerRemoved()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLayersAdded()
        'LayersAdded()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMapDragFinished()
        'MapDragFinished()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMapMouseUp()
        'MapMouseUp()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMapMouseMove()
        'MapMouseMove()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMapMouseDown()
        'MapMouseDown()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestMapExtentsChanged()
        'MapExtentsChanged()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLegendMouseUp()
        'LegendMouseUp()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLegendMouseDown()
        'LegendMouseDown()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestLegendDoubleClick()
        'LegendDoubleClick()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestTerminate()
        'Terminate()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestInitialize()
        'Initialize()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestProjectSaving()
        'ProjectSaving()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestProjectLoading()
        'ProjectLoading()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Version()
        'get_Version()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_BuildDate()
        'get_BuildDate()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Name()
        'get_Name()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_SerialNumber()
        'get_SerialNumber()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Description()
        'get_Description()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Author()
        'get_Author()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestComputeTimeseries()
        'ComputeTimeseries()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestComputeStatistics()
        'ComputeStatistics()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_AvailableTimeseriesOperations()
        'get_AvailableTimeseriesOperations()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_AvailableStatistics()
        'get_AvailableStatistics()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetHashCode()
        'GetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        'Equals()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testrefresh()
        'refresh()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_atcMsgWDM

    Public Sub TestGetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        Assert.IsTrue(Equals(Me))
    End Sub

    Public Sub TestToString()
        Assert.AreEqual(ToString(), "atcWDM.Test_atcMsgWDM")
    End Sub

    Public Sub Testget_MsgUnit()
        'Dim lmsgwdm As New atcMsgWDM
        'Dim i As Long
        'dbg.msg("Testget_MsgUnit")
        'i = lmsgwdm.MsgUnit()
        'dbg.msg("  MsgUnit=" & i)
        'Assert.AreEqual(11, i)
        'lmsgwdm = Nothing
    End Sub

    Public Sub Testset_MsgUnit()
        'set_MsgUnit()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Attributes()
        'get_Attributes()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_HassLibs

    Public Sub TestF90_MSG()
        'F90_MSG()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_W99OPN()
        'F90_W99OPN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_W99CLO()
        'F90_W99CLO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDMOPN()
        'F90_WDMOPN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDMCLO()
        'F90_WDMCLO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_INQNAM()
        'F90_INQNAM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DATLST_XX()
        'F90_DATLST_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DECCHX_XX()
        'F90_DECCHX_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_ASRTRP()
        'F90_ASRTRP()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_CMPTIM()
        'F90_CMPTIM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DATNXT()
        'F90_DATNXT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DAYMON()
        'F90_DAYMON()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMADD()
        'F90_TIMADD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMDIF()
        'F90_TIMDIF()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMCNV()
        'F90_TIMCNV()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMCVT()
        'F90_TIMCVT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMBAK()
        'F90_TIMBAK()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TIMCHK()
        'F90_TIMCHK()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_JDMODY()
        'F90_JDMODY()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DTMCMN()
        'F90_DTMCMN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DECPRC()
        'F90_DECPRC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBFIN()
        'F90_WDBFIN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDFLCL()
        'F90_WDFLCL()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDDSNX()
        'F90_WDDSNX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDCKDT()
        'F90_WDCKDT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBOPN()
        'F90_WDBOPN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBOPNR()
        'F90_WDBOPNR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTX_XX()
        'F90_WMSGTX_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTW_XX()
        'F90_WMSGTW_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTT_XX()
        'F90_WMSGTT_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTH()
        'F90_WMSGTH()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDLBAX()
        'F90_WDLBAX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDDSDL()
        'F90_WDDSDL()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GETATT()
        'F90_GETATT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDTGET()
        'F90_WDTGET()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDTPUT()
        'F90_WDTPUT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WTFNDT()
        'F90_WTFNDT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSGC_XX()
        'F90_WDBSGC_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSGI()
        'F90_WDBSGI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSGR()
        'F90_WDBSGR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDLBAD()
        'F90_WDLBAD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDDSCL()
        'F90_WDDSCL()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDDSRN()
        'F90_WDDSRN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSAC()
        'F90_WDBSAC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSAI()
        'F90_WDBSAI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSAR()
        'F90_WDBSAR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDSAGY_XX()
        'F90_WDSAGY_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDLGET()
        'F90_WDLGET()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDLLSU()
        'F90_WDLLSU()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDIINI()
        'F90_WDIINI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WIDADD()
        'F90_WIDADD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WUA2ID()
        'F90_WUA2ID()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WID2UD()
        'F90_WID2UD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SCALIT()
        'F90_SCALIT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDRRE()
        'F90_TSDRRE()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSPC_XX()
        'F90_TSDSPC_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSIN()
        'F90_TSDSIN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSCT()
        'F90_TSDSCT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSUN_XX()
        'F90_TSDSUN_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSESPC()
        'F90_TSESPC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSM()
        'F90_TSDSM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSAPUT()
        'F90_TSAPUT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSGN()
        'F90_TSDSGN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DELSCN()
        'F90_DELSCN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_COPSCN()
        'F90_COPSCN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_NEWFIL()
        'F90_NEWFIL()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_NEWDSN()
        'F90_NEWDSN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_FILSET()
        'F90_FILSET()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCISAV()
        'F90_UCISAV()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UMAKPR()
        'F90_UMAKPR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UMAKDO()
        'F90_UMAKDO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGNRC()
        'F90_UCGNRC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGNCO()
        'F90_UCGNCO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGNLA()
        'F90_UCGNLA()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGNEX()
        'F90_UCGNEX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGNME()
        'F90_UCGNME()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGOUT()
        'F90_UCGOUT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGIRC_XX()
        'F90_UCGIRC_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WATINI()
        'F90_WATINI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WATHED_XX()
        'F90_WATHED_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WATINP()
        'F90_WATINP()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WATCLO()
        'F90_WATCLO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPGG_XX()
        'F90_STSPGG_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPGU_XX()
        'F90_STSPGU_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPFN_XX()
        'F90_STSPFN_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPPN()
        'F90_STSPPN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SIMNET()
        'F90_SIMNET()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DAANST()
        'F90_DAANST()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DAANWV()
        'F90_DAANWV()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_REPEXT()
        'F90_REPEXT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_ADDREPT()
        'F90_ADDREPT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DELREPT()
        'F90_DELREPT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_ADDBMP()
        'F90_ADDBMP()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SPIPH()
        'F90_SPIPH()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SCNDBG()
        'F90_SCNDBG()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_ACTSCN()
        'F90_ACTSCN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GLOBLK_XX()
        'F90_GLOBLK_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_PUTGLO()
        'F90_PUTGLO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SIMSCN()
        'F90_SIMSCN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GTNXKW_XX()
        'F90_GTNXKW_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTABLE_XX()
        'F90_XTABLE_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTABLEEX_XX()
        'F90_XTABLEEX_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XBLOCK_XX()
        'F90_XBLOCK_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XBLOCKEX_XX()
        'F90_XBLOCKEX_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTINFO_XX()
        'F90_XTINFO_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_REPUCI()
        'F90_REPUCI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DELUCI()
        'F90_DELUCI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_PUTUCI()
        'F90_PUTUCI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_HGETI()
        'F90_HGETI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_HGETC_XX()
        'F90_HGETC_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_HPUTC()
        'F90_HPUTC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GTINS_XX()
        'F90_GTINS_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_PBMPAR()
        'F90_PBMPAR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DELBMP()
        'F90_DELBMP()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GETOCR()
        'F90_GETOCR()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_FILSTA()
        'F90_FILSTA()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SGLABL_XX()
        'F90_SGLABL_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_FITLIN()
        'F90_FITLIN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_CMSTRM()
        'F90_CMSTRM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSFLAT()
        'F90_TSFLAT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_INFREE()
        'F90_INFREE()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TREE_BLD()
        'F90_TREE_BLD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TREE_SUM()
        'F90_TREE_SUM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TREE_SET_NAME()
        'F90_TREE_SET_NAME()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DISP_LIS()
        'F90_DISP_LIS()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TREE_END()
        'F90_TREE_END()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_FILT_MOD()
        'F90_FILT_MOD()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_FILT_LIS()
        'F90_FILT_LIS()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TREE_ROOT()
        'F90_TREE_ROOT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_BRAN_GET_PARM()
        'F90_BRAN_GET_PARM()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_PUTOLV()
        'F90_PUTOLV()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_MSGUNIT()
        'F90_MSGUNIT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSCBAT()
        'F90_TSCBAT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GNMEXE()
        'F90_GNMEXE()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GMANEX()
        'F90_GMANEX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GNTTRN()
        'F90_GNTTRN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GETUCI_XX()
        'F90_GETUCI_XX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SET_DRIVER()
        'F90_SET_DRIVER()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SENDSTRING()
        'F90_SENDSTRING()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GTNXKW()
        'F90_GTNXKW()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTABLE()
        'F90_XTABLE()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTABLEEX()
        'F90_XTABLEEX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XBLOCK()
        'F90_XBLOCK()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XBLOCKEX()
        'F90_XBLOCKEX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WATHED()
        'F90_WATHED()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_UCGIRC()
        'F90_UCGIRC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPGU()
        'F90_STSPGU()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPGG()
        'F90_STSPGG()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_STSPFN()
        'F90_STSPFN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GLOBLK()
        'F90_GLOBLK()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DATLST()
        'F90_DATLST()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_DECCHX()
        'F90_DECCHX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDBSGC()
        'F90_WDBSGC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSPC()
        'F90_TSDSPC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_TSDSUN()
        'F90_TSDSUN()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_SGLABL()
        'F90_SGLABL()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTX()
        'F90_WMSGTX()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTW()
        'F90_WMSGTW()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WMSGTT()
        'F90_WMSGTT()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_XTINFO()
        'F90_XTINFO()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_HGETC()
        'F90_HGETC()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GTINS()
        'F90_GTINS()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_WDSAGY()
        'F90_WDSAGY()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestF90_GETUCI()
        'F90_GETUCI()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetHashCode()
        'GetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        'Equals()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_WDMGlobal

    Public Sub TestUnitsAttributeDefinition()
        'UnitsAttributeDefinition()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetHashCode()
        'GetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        'Equals()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_atcWdmHandle

    Public Sub TestDispose()
        'Dispose()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetHashCode()
        'GetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        'Equals()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Unit()
        'get_Unit()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_dbg

    Public Sub TestMsg()
        'Msg()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestFileSetup()
        'FileSetup()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetHashCode()
        'GetHashCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestEquals()
        'Equals()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class
