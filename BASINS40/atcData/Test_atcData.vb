Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports NUnit.Framework
Imports ATCUtility.modReflection

<TestFixture()> Public Class Test_Builder
  Public Sub TestsAllPresent()
    Dim lTestBuildStatus as String
    lTestBuildStatus = BuildMissingTests("c:\test\")
    Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
  End Sub
End Class

<TestFixture()> Public Class Test_atcDefinedValue

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcAttributeDefinition

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Calculated()
    'get_Calculated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Calculator()
    'get_Calculator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Calculator()
    'set_Calculator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Category()
    'set_Category()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Description()
    'get_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Description()
    'set_Description()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Help()
    'get_Help()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Help()
    'set_Help()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Editable()
    'get_Editable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Editable()
    'set_Editable()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ID()
    'get_ID()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ID()
    'set_ID()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_TypeString()
    'get_TypeString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_TypeString()
    'set_TypeString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValidList()
    'get_ValidList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValidList()
    'set_ValidList()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DefaultValue()
    'get_DefaultValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DefaultValue()
    'set_DefaultValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Min()
    'get_Min()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Min()
    'set_Min()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Max()
    'get_Max()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Max()
    'set_Max()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataAttributes

  Public Sub TestAddDefinition()
    'AddDefinition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetDefinition()
    'GetDefinition()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAllDefinitions()
    'AllDefinitions()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIsSimple()
    'IsSimple()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByIndex()
    'set_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByIndex()
    'get_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetEnumerator()
    'GetEnumerator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsSynchronized()
    'get_IsSynchronized()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SyncRoot()
    'get_SyncRoot()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Count()
    'get_Count()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyTo()
    'CopyTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveAt()
    'RemoveAt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemove()
    'Remove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsert()
    'Insert()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOf()
    'IndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsFixedSize()
    'get_IsFixedSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsReadOnly()
    'get_IsReadOnly()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAdd()
    'Add()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Item()
    'set_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Item()
    'get_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTrimToSize()
    'TrimToSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToArray()
    'ToArray()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSort()
    'Sort()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetRange()
    'GetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetRange()
    'SetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReverse()
    'Reverse()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveRange()
    'RemoveRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLastIndexOf()
    'LastIndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsertRange()
    'InsertRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBinarySearch()
    'BinarySearch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddRange()
    'AddRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capacity()
    'set_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capacity()
    'get_Capacity()
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

  Public Sub TestAddHistory()
    'AddHistory()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestValuesSortedByName()
    'ValuesSortedByName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestContainsAttribute()
    'ContainsAttribute()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetFormattedValue()
    'GetFormattedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetValue()
    'GetValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetValue()
    'SetValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCalculateAll()
    'CalculateAll()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDiscardCalculated()
    'DiscardCalculated()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetDefinedValue()
    'GetDefinedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Keys()
    'get_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Keys()
    'set_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByKey()
    'RemoveByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByKey()
    'get_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByKey()
    'set_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromKey()
    'IndexFromKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChangeTo()
    'ChangeTo()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataDisplay

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

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
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

End Class

<TestFixture()> Public Class Test_atcDataGroup

  Public Sub Testset_ItemByIndex()
    'set_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByIndex()
    'get_ItemByIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetEnumerator()
    'GetEnumerator()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsSynchronized()
    'get_IsSynchronized()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SyncRoot()
    'get_SyncRoot()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Count()
    'get_Count()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyTo()
    'CopyTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveAt()
    'RemoveAt()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemove()
    'Remove()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsert()
    'Insert()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOf()
    'IndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsFixedSize()
    'get_IsFixedSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_IsReadOnly()
    'get_IsReadOnly()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAdd()
    'Add()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Item()
    'set_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Item()
    'get_Item()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTrimToSize()
    'TrimToSize()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToArray()
    'ToArray()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSort()
    'Sort()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetRange()
    'GetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetRange()
    'SetRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReverse()
    'Reverse()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveRange()
    'RemoveRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestLastIndexOf()
    'LastIndexOf()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestInsertRange()
    'InsertRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestBinarySearch()
    'BinarySearch()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddRange()
    'AddRange()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Capacity()
    'set_Capacity()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Capacity()
    'get_Capacity()
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

  Public Sub Testadd_Added()
    'add_Added()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Removed()
    'remove_Removed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_Removed()
    'add_Removed()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_Added()
    'remove_Added()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ItemByKey()
    'get_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ItemByKey()
    'set_ItemByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestChangeTo()
    'ChangeTo()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOfSerial()
    'IndexOfSerial()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SelectedData()
    'get_SelectedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_SelectedData()
    'set_SelectedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Keys()
    'get_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Keys()
    'set_Keys()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveByKey()
    'RemoveByKey()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexFromKey()
    'IndexFromKey()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_AddedEventHandler

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
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

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_RemovedEventHandler

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
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

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataManager

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_OpenedData()
    'remove_OpenedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_OpenedData()
    'add_OpenedData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Basins()
    'get_Basins()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataSources()
    'get_DataSources()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestDataSets()
    'DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_SelectionAttributes()
    'get_SelectionAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DisplayAttributes()
    'get_DisplayAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetPlugins()
    'GetPlugins()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpenDataSource()
    'OpenDataSource()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserSelectDataSource()
    'UserSelectDataSource()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserSelectData()
    'UserSelectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestUserManage()
    'UserManage()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_XML()
    'get_XML()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_XML()
    'set_XML()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_OpenedDataEventHandler

  Public Sub TestGetObjectData()
    'GetObjectData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClone()
    'Clone()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGetInvocationList()
    'GetInvocationList()
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

  Public Sub TestDynamicInvoke()
    'DynamicInvoke()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Method()
    'get_Method()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Target()
    'get_Target()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataPlugin

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

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
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

End Class

<TestFixture()> Public Class Test_atcDataSet

  Public Sub TestClear()
    'Clear()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEquals()
    Dim lData1 As New atcDataSet
    Dim lData2 As New atcDataSet
    Assert.IsTrue(lData1.Equals(lData1))
    Assert.IsFalse(lData1.Equals(lData2))
  End Sub

  Public Sub TestToString()
    Dim lData1 As New atcDataSet
    Dim lDataToString As String = lData1.ToString
    Assert.IsTrue(lDataToString.StartsWith("atcDataSet #"))
    Assert.IsTrue(lDataToString.IndexOf("attributes") > 0)
  End Sub

  Public Sub Testget_Attributes()
    Dim lData1 As New atcDataSet
    lData1.Attributes.SetValue("testkey", "testvalue")
    Assert.IsTrue(lData1.Attributes.GetValue("testkey", "").Equals("testvalue"))
  End Sub

  Public Sub Testget_Serial()
    Dim lData1 As New atcDataSet
    Dim lData2 As New atcDataSet
    Assert.IsTrue(lData2.Serial > lData1.Serial)
  End Sub

End Class

<TestFixture()> Public Class Test_atcDataSource

  Public Sub TestAddDataSet()
    'AddDataSet()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Specification()
    'set_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Specification()
    'get_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSave()
    'Save()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadData()
    'ReadData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpen()
    'Open()
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

  Public Sub Testget_DataSets()
    'get_DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AvailableOperations()
    'get_AvailableOperations()
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

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
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

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataManager()
    'get_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DataManager()
    'set_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_EnumExistAction

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

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcSeasons

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsAMPM

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsHour

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfMonth

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfWeek

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsDayOfYear

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsMonth

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsYear

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsWaterYear

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsThresholdTS

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcSeasonsYearSubset

  Public Sub TestSeasonName()
    'SeasonName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSeasonIndex()
    'SeasonIndex()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSplit()
    'Split()
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

End Class

<TestFixture()> Public Class Test_atcTimeseries

  Public Sub TestClear()
    'Clear()
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

  Public Sub Testget_Value()
    'get_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Value()
    'set_Value()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Values()
    'get_Values()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Values()
    'set_Values()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValueAttributes()
    'get_ValueAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValueAttributes()
    'set_ValueAttributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Dates()
    'get_Dates()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Dates()
    'set_Dates()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_numValues()
    'get_numValues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_numValues()
    'set_numValues()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEnsureValuesRead()
    'EnsureValuesRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ValuesNeedToBeRead()
    'get_ValuesNeedToBeRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_ValuesNeedToBeRead()
    'set_ValuesNeedToBeRead()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Serial()
    'get_Serial()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_atcTimeseriesStatistics

  Public Sub TestAddDataSet()
    'AddDataSet()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Specification()
    'set_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Specification()
    'get_Specification()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSave()
    'Save()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestReadData()
    'ReadData()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOpen()
    'Open()
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

  Public Sub Testget_DataSets()
    'get_DataSets()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_AvailableOperations()
    'get_AvailableOperations()
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

  Public Sub TestNewOne()
    'NewOne()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Category()
    'get_Category()
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

  Public Sub Testget_Attributes()
    'get_Attributes()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_DataManager()
    'get_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_DataManager()
    'set_DataManager()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmDataSource

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAskUser()
    'AskUser()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmManager

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestEdit()
    'Edit()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmSeasonalAttributes

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_frmSelectData

  Public Sub TestEquals()
    'Equals()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestToString()
    'ToString()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAskUser()
    'AskUser()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_GridSource

  Public Sub Testset_Alignment()
    'set_Alignment()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Alignment()
    'get_Alignment()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Columns()
    'set_Columns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Columns()
    'get_Columns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_Rows()
    'set_Rows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_Rows()
    'get_Rows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_CellValue()
    'set_CellValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_CellValue()
    'get_CellValue()
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

  Public Sub Testadd_ChangedRows()
    'add_ChangedRows()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangedColumns()
    'add_ChangedColumns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedColumns()
    'remove_ChangedColumns()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedValue()
    'remove_ChangedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testadd_ChangedValue()
    'add_ChangedValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testremove_ChangedRows()
    'remove_ChangedRows()
    Assert.Ignore("Test not yet written")
  End Sub

End Class

<TestFixture()> Public Class Test_modTimeseriesMath

  Public Sub TestSubsetByDate()
    'SubsetByDate()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestCopyBaseAttributes()
    'CopyBaseAttributes()
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

End Class
