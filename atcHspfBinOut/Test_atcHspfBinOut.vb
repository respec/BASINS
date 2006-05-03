Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports ATCUtility.modReflection

<TestFixture()> Public Class Test_Builder
    Public Sub TestsAllPresent()
        Dim lTestBuildStatus As String
        lTestBuildStatus = BuildMissingTests("c:\test\")
        Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
    End Sub
End Class

<TestFixture()> Public Class Test_clsNetworkHspfOutput

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

<TestFixture()> Public Class Test_clsConnectionType

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

<TestFixture()> Public Class Test_clsHspfBinary

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

    Public Sub TestBinaryValue()
        'BinaryValue()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_clsUnfRec

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

<TestFixture()> Public Class Test_clsHspfBinId

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

<TestFixture()> Public Class Test_clsHspfBinData

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

<TestFixture()> Public Class Test_clsHspfBinHeader

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

<TestFixture()> Public Class Test_clsFtnUnfFile

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

<TestFixture()> Public Class Test_atcTimeseriesFileHspfBinOut
    Public Sub TestSave()
        'Save()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestReadData()
        'ReadData()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestOpen()
        Dim lFile As New atcTimeseriesFileHspfBinOut
        Dim lName As String = "\test\C:\test\atcHspfBinOut\data\test15p.hbn"
        lFile.Open(lName)
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

    Public Sub Testget_FileName()
        'get_FileName()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testset_FileName()
        'set_FileName()
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

    Public Sub TestAddTimSer()
        'AddTimSer()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_AvailableAttributes()
        'get_AvailableAttributes()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Data()
        'get_Data()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_DataCollection()
        'get_DataCollection()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_DataCount()
        'get_DataCount()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_ErrorDescription()
        'get_ErrorDescription()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_FileUnit()
        'get_FileUnit()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testset_HelpFilename()
        'set_HelpFilename()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testget_Label()
        'get_Label()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub Testset_Monitor()
        'set_Monitor()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestRemoveTimSer()
        'RemoveTimSer()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestRewriteTimSer()
        'RewriteTimSer()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestSaveAs()
        'SaveAs()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestShowFilterEdit()
        'ShowFilterEdit()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class

<TestFixture()> Public Class Test_ATCTimeUnit

    Public Sub TestToString()
        'ToString()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestGetTypeCode()
        'GetTypeCode()
        Assert.Ignore("Test not yet written")
    End Sub

    Public Sub TestCompareTo()
        'CompareTo()
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

    Public Sub TestGetType()
        'GetType()
        Assert.Ignore("Test not yet written")
    End Sub

End Class
