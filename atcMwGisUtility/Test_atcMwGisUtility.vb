Option Strict Off
Option Explicit On
'Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Imports System.Collections.Specialized
Imports NUnit.Framework
Imports atcUtility
Imports atcUtility.modReflection

<TestFixture()> Public Class Test_Builder
  Public Sub TestsAllPresent()
    Dim lTestBuildStatus As String = BuildMissingTests("c:\test\")
    Assert.AreEqual("All tests present.", lTestBuildStatus, lTestBuildStatus)
  End Sub
End Class

<TestFixture()> Public Class Test_GISUtils

  Public Sub TestSetGisUtilsMappingObject()
    'SetGisUtilsMappingObject()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_OverlappingPolygons()
    'GisUtil_OverlappingPolygons()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_AssignContainingPolygons()
    'GisUtil_AssignContainingPolygons()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NumFieldsInLayer()
    'GisUtil_NumFieldsInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NthFieldNameInLayer()
    'GisUtil_NthFieldNameInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_FindLayerIndexByName()
    'GisUtil_FindLayerIndexByName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_FindFieldIndexByName()
    'GisUtil_FindFieldIndexByName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_AddField()
    'GisUtil_AddField()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_RemoveField()
    'GisUtil_RemoveField()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NumLayers()
    'GisUtil_NumLayers()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_LayerType()
    'GisUtil_LayerType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_LayerName()
    'GisUtil_LayerName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_LayerFileName()
    'GisUtil_LayerFileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NthFieldTypeInLayer()
    'GisUtil_NthFieldTypeInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_GridLayerMinimum()
    'GisUtil_GridLayerMinimum()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_GridLayerMaximum()
    'GisUtil_GridLayerMaximum()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NumFeaturesInLayer()
    'GisUtil_NumFeaturesInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_CellValueNthFeatureInLayer()
    'GisUtil_CellValueNthFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_AreaNthFeatureInLayer()
    'GisUtil_AreaNthFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_LengthNthFeatureInLayer()
    'GisUtil_LengthNthFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_EndPointsOfLine()
    'GisUtil_EndPointsOfLine()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_PointXY()
    'GisUtil_PointXY()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_RemoveFeatureFromLayer()
    'GisUtil_RemoveFeatureFromLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_SetValueNthFeatureInLayer()
    'GisUtil_SetValueNthFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_NumSelectedFeaturesInLayer()
    'GisUtil_NumSelectedFeaturesInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_IndexOfNthSelectedFeatureInLayer()
    'GisUtil_IndexOfNthSelectedFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_ProjectFileName()
    'GisUtil_ProjectFileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_PointInPolygon()
    'GisUtil_PointInPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_PointInPolygonXY()
    'GisUtil_PointInPolygonXY()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_AddLayerToMap()
    'GisUtil_AddLayerToMap()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_SetLayerVisible()
    'GisUtil_SetLayerVisible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_RemoveLayerFromMap()
    'GisUtil_RemoveLayerFromMap()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_TabulateAreas()
    'GisUtil_TabulateAreas()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_GridMinMaxInPolygon()
    'GisUtil_GridMinMaxInPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_GridValueAtPoint()
    'GisUtil_GridValueAtPoint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_Overlay()
    'GisUtil_Overlay()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_ClipShapesWithPolygon()
    'GisUtil_ClipShapesWithPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGisUtil_MergeFeaturesBasedOnAttribute()
    'GisUtil_MergeFeaturesBasedOnAttribute()
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

<TestFixture()> Public Class Test_GisUtil
  Private pTestDir As String = "c:\test\atcMwGisUtility\"
  Private pBaseDir As String = pTestDir & "temp\"
  Private pArchiveDir As String = pTestDir & "data\"
  Private pProjectName As String = pBaseDir & "patuxa.mwprj"

  Private pGisUtil As GisUtil

  <TestFixtureSetUp()> Public Sub init()
    LogDbg("Test_GisUtil:Init")
    Dim lFileNames As New NameValueCollection
    AddFilesInDir(lFileNames, pArchiveDir, False)
    For Each lFile As String In lFileNames
      FileCopy(lFile, pBaseDir & FilenameNoPath(lFile))
    Next
    LogDbg("Test_GisUtil:Init:Files:" & lFileNames.Count)
  End Sub

  Private Function LayerDescription(ByVal aLayerIndex As Integer) As String
    Dim lRet As String = "<" & aLayerIndex & ":"
    If aLayerIndex >= 0 And aLayerIndex < GisUtil.NumLayers Then
      lRet &= GisUtil.LayerName(aLayerIndex) & ":" & GisUtil.LayerType(aLayerIndex) & ">"
    Else
      lRet &= "<Layer Index Out Of Range>>"
    End If
    Return lRet
  End Function

  Public Sub Testget_MappingObject()
    'testing not applicable here
  End Sub

  Public Sub Testset_MappingObject()
    'testing not applicable here
  End Sub

  Public Sub TestLoadProject()
    GisUtil.LoadProject(pProjectName)
    LogDbg("TestLoadProject:" & GisUtil.ProjectFileName)
    For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
      LogDbg("  Layer:" & LayerDescription(lLayerIndex))
    Next
  End Sub

  Public Sub Testget_CurrentLayer()
    LogDbg("Testget_CurrentLayer:" & GisUtil.CurrentLayer)
  End Sub

  Public Sub Testset_CurrentLayer()
    GisUtil.CurrentLayer = 2
    LogDbg("Testset_CurrentLayer:" & GisUtil.CurrentLayer)
    Try
      GisUtil.CurrentLayer = GisUtil.NumLayers + 1
    Catch ex As Exception
      LogDbg("Testset_CurrentLayer:Exception:" & ex.ToString)
    End Try
  End Sub

  Public Sub TestOverlappingPolygons()
    Dim lLayerIndex1 As Integer = 1
    Dim lLayerIndex2 As Integer = 2
    Dim lFeatureIndex2 As Integer = 0
    Dim lResult As Boolean

    For lCurFeature As Integer = 0 To GisUtil.NumFeatures(lLayerIndex1) - 1
      lResult = GisUtil.OverlappingPolygons(lLayerIndex1, lCurFeature, lLayerIndex2, lFeatureIndex2)
      If lResult Then
        LogDbg("TestOverlappingPolygons:" & lLayerIndex1 & ":" & lLayerIndex2 & ":" & lCurFeature & ":" & lFeatureIndex2)
        LogDbg("TestOverlappingPolygons:" & _
          GisUtil.FieldValue(lLayerIndex1, lCurFeature, GisUtil.FieldIndex(lLayerIndex1, "CNTYNAME")) & ":" & _
          GisUtil.FieldValue(lLayerIndex2, lFeatureIndex2, GisUtil.FieldIndex(lLayerIndex2, "st")))
        Exit For
      End If
    Next
    LogDbg("TestOverlappingPolygons:exit")
  End Sub

  Public Sub TestAssignContainingPolygons()
    'AssignContainingPolygons()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNumFields()
    Dim lNumFields As Integer

    For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
      Try
        lNumFields = GisUtil.NumFields(lLayerIndex)
        LogDbg("TestNumFields:" & lNumFields & ":" & LayerDescription(lLayerIndex))
      Catch ex As Exception
        LogDbg("TestNumFields:Exception:" & ex.Message & ":" & LayerDescription(lLayerIndex))
      End Try
    Next

    Try
      lNumFields = GisUtil.NumFields  'current layer
      LogDbg("TestNumFields:CurrentLayer:" & lNumFields & ":" & LayerDescription(GisUtil.CurrentLayer))
    Catch ex As Exception
      LogDbg("TestNumFields:Exception:" & ex.Message & ":" & LayerDescription(GisUtil.CurrentLayer))
    End Try
  End Sub

  Public Sub TestFieldName()
    Dim lLayerIndex As Integer = 1
    Dim lFieldIndex As Integer = 2
    Dim lFieldName As String = GisUtil.FieldName(lFieldIndex, lLayerIndex)
    LogDbg("TestFieldName:" & lFieldName & ":" & lFieldIndex & ":" & LayerDescription(lLayerIndex))
  End Sub

  Public Sub TestLayerIndex()
    Dim lLayerNames() As String = {"st", "cnty", "huc"}
    For Each lLayerName As String In lLayerNames
      Try
        Dim lLayerIndex = GisUtil.LayerIndex(lLayerName)
        LogDbg("TestFindLayerIndex:" & LayerDescription(lLayerIndex))
      Catch ex As Exception
        LogDbg("TestFindLayerIndex:Exception:" & ex.Message)
      End Try
    Next
  End Sub

  Public Sub TestFieldIndex()
    Dim lLayerIndex As Integer = 1
    Dim lFieldName As String = "CNTYNAME"
    Dim lFieldIndex As String = GisUtil.FieldIndex(lLayerIndex, lFieldName)
    LogDbg("TestFieldIndex:" & lFieldName & ":" & lFieldIndex & ":" & LayerDescription(lLayerIndex))
  End Sub

  Public Sub TestFieldValue()
    Dim lLayerIndex As Integer = 1
    Dim lFeatureIndex As Integer = 1
    Dim lFieldIndex As Integer = 1
    Dim lFieldValue As String = GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lFieldIndex)
    LogDbg("TestFieldValue:" & lFieldValue & ":" & lFeatureIndex & ":" & lFieldIndex & ":" & lLayerIndex)
  End Sub

  Public Sub TestAddField()
    Dim lLayerIndex As Integer = 1
    Dim lFieldName As String = "dummy"
    Dim lFieldWidth As Integer = 5
    Dim lFieldType As Integer = 1
    Dim lResult = GisUtil.AddField(lLayerIndex, lFieldName, 1, 5)
    LogDbg("TestAddField:" & lResult & ":" & lFieldName & ":" & lFieldType & ":" & lFieldWidth & ":" & lLayerIndex)
  End Sub

  Public Sub TestRemoveField()
    Dim lLayerIndex As Integer = 1
    Dim lFieldName As String = "dummy"
    Dim lResult = GisUtil.RemoveField(lLayerIndex, lFieldName)
    LogDbg("TestRemoveField:" & lResult & ":" & lFieldName & ":" & lLayerIndex)
  End Sub

  Public Sub TestNumLayers()
    LogDbg("TestNumLayers:" & GisUtil.NumLayers)
  End Sub

  Public Sub TestLayerType()
    Dim lLayerIndex As Integer = 1
    Dim lLayerType As Integer = GisUtil.LayerType(lLayerIndex)
    LogDbg("TestLayerType:" & lLayerType & ":" & lLayerIndex)
  End Sub

  Public Sub TestLayerName()
    Dim lLayerIndex As Integer = 1
    Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
    LogDbg("TestLayerName:" & lLayerName & ":" & lLayerIndex)
  End Sub

  Public Sub TestLayerFileName()
    Dim lLayerIndex As Integer = 1
    Dim lLayerFileName As String = GisUtil.LayerFileName(lLayerIndex)
    LogDbg("TestLayerName:" & lLayerFileName & ":" & lLayerIndex)
  End Sub

  Public Sub TestFieldType()
    Dim lLayerIndex As Integer = 1
    Dim lFieldIndex As Integer = 2
    Dim lFieldType As String = GisUtil.FieldType(lFieldIndex, lLayerIndex)
    LogDbg("TestFieldType:" & lFieldType & ":" & lFieldIndex & ":" & lLayerIndex)
  End Sub

  Public Sub TestGridLayerMinimum()
    Dim lLayerIndex As Integer = 3
    LogDbg("TestGridLayerMinimum:" & GisUtil.GridLayerMinimum(lLayerIndex))
  End Sub

  Public Sub TestGridLayerMaximum()
    Dim lLayerIndex As Integer = 3
    LogDbg("TestGridLayerMaximum:" & GisUtil.GridLayerMaximum(lLayerIndex))
  End Sub

  Public Sub TestNumFeatures()
    For lLayerIndex As Integer = 0 To GisUtil.NumLayers
      Try
        LogDbg("TestNumFeatures:" & GisUtil.NumFeatures(lLayerIndex) & ":" & LayerDescription(lLayerIndex))
      Catch ex As Exception
        LogDbg("TestNumFeatures:Exception:" & ex.Message & ":" & LayerDescription(lLayerIndex))
      End Try
    Next
  End Sub

  Public Sub TestFeatureArea()
    Dim lFeatureIndex As Integer = 0
    For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
      Try
        LogDbg("TestLayerArea:" & GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) & ":" & LayerDescription(lLayerIndex))
      Catch ex As Exception
        LogDbg("TestLayerArea:Exception:" & ex.Message & ":" & LayerDescription(lLayerIndex))
      End Try
    Next
  End Sub

  Public Sub TestFeatureLength()
    Dim lFeaturnIndex As Integer = 0
    For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
      Try
        LogDbg("TestLayerLength:" & GisUtil.FeatureLength(lLayerIndex, lFeaturnIndex) & ":" & LayerDescription(lLayerIndex))
      Catch ex As Exception
        LogDbg("TestLayerLength:Exception:" & ex.Message & ":" & LayerDescription(lLayerIndex))
      End Try
    Next
  End Sub

  Public Sub TestEndPointsOfLine()
    'EndPointsOfLine()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointXY()
    'PointXY()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveFeatureFromLayer()
    'RemoveFeatureFromLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetValueNthFeatureInLayer()
    'SetValueNthFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNumSelectedFeaturesInLayer()
    'NumSelectedFeaturesInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestIndexOfNthSelectedFeatureInLayer()
    'IndexOfNthSelectedFeatureInLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_ProjectFileName()
    'get_ProjectFileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointInPolygon()
    'PointInPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestPointInPolygonXY()
    'PointInPolygonXY()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddLayerToMap()
    'AddLayerToMap()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_LayerVisible()
    'get_LayerVisible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testset_LayerVisible()
    'set_LayerVisible()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveLayerFromMap()
    'RemoveLayerFromMap()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestTabulateAreas()
    'TabulateAreas()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGridMinMaxInPolygon()
    'GridMinMaxInPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestGridValueAtPoint()
    'GridValueAtPoint()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestOverlay()
    'Overlay()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestClipShapesWithPolygon()
    'ClipShapesWithPolygon()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestMergeFeaturesBasedOnAttribute()
    'MergeFeaturesBasedOnAttribute()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumFields()
    'get_NumFields()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldName()
    'get_FieldName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumLayers()
    'get_NumLayers()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_LayerType()
    'get_LayerType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_LayerName()
    'get_LayerName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_LayerFileName()
    'get_LayerFileName()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_NumFeatures()
    'get_NumFeatures()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_FieldType()
    'get_FieldType()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_GridLayerMinimum()
    'get_GridLayerMinimum()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub Testget_GridLayerMaximum()
    'get_GridLayerMaximum()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveFeature()
    'RemoveFeature()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestSetFeatureValue()
    'SetFeatureValue()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestNumSelectedFeatures()
    'NumSelectedFeatures()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestExtentsOfLayer()
    'ExtentsOfLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestAddLayer()
    'AddLayer()
    Assert.Ignore("Test not yet written")
  End Sub

  Public Sub TestRemoveLayer()
    'RemoveLayer()
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

  Public Sub TestRunAllTests()
    'RunAllTests()
    Assert.Ignore("Test not yet written")
  End Sub

End Class
