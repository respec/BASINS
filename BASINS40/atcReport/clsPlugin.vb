Imports atcData
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Collections.Specialized

Public Class PlugIn
  Inherits atcData.atcDataDisplay

  Friend pMapWin As MapWindow.Interfaces.IMapWin
  Friend pReportsDir As String
  Friend pReportsColl As Collection

  'TODO: get these 3 from BASINS4 or plugInManager or Name?
  Private Const pReportsMenuName As String = "BasinsReports"
  Private Const pReportsMenuString As String = "Reports"

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Reports::Watershed Characterization Reports"
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get
      Return "This plug-in provides an interface for generating watershed characterization reports."
    End Get
  End Property

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    Dim lMenuItem As MapWindow.Interfaces.MenuItem

    pMapWin = aMapWin

    pMapWin.Menus.AddMenu(pReportsMenuName, "", Nothing, pReportsMenuString, "mnuFile")
    lMenuItem = pMapWin.Menus.AddMenu(pReportsMenuName & "_Watershed", pReportsMenuName, Nothing, "Watershed Characterization Reports")
    Dim lAllFiles = New NameValueCollection
    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
    pReportsDir = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\reports\"
    If Mid(pReportsDir, 3, 5) = "\dev\" Then
      'change loc if in devel envir
      pReportsDir = "C:\dev\BASINS40\atcReport\scripts"
    End If
    AddFilesInDir(lAllFiles, pReportsDir, True, "*.vb")
    Dim lReport As String
    pReportsColl = New Collection
    For Each lReport In lAllFiles
      pReportsColl.Add(lReport)
    Next lReport
  End Sub

  Public Overrides Sub Terminate()
    pMapWin.Menus.Remove(pReportsMenuName)
    pMapWin.Menus.Remove(pReportsMenuName & "_Watershed")
  End Sub

  Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
    If aItemName = pReportsMenuName & "_Watershed" Then
      GisUtil.MappingObject = pMapWin
      Dim lFrmReport As New frmReport
      lFrmReport.InitializeUI(Me)
      lFrmReport.Show()
      aHandled = True
    End If
  End Sub

  Public Sub BuildReport(ByVal aAreaLayerName As String, ByVal aAreaIDFieldName As String, _
                         ByVal aAreaNameFieldName As String, _
                         ByVal aOutputFolder As String, _
                         ByVal aReportIndex As Integer)
    Dim i As Integer
    Dim lError As String
    Dim lArgs(5) As Object
    lArgs(0) = aAreaLayerName
    lArgs(1) = aAreaIDFieldName
    lArgs(2) = aAreaNameFieldName

    Dim AreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayerName)
    'are any areas selected?
    Dim cSelectedAreaIndexes As New Collection
    For i = 1 To GisUtil.NumSelectedFeatures(AreaLayerIndex)
      'add selected areas to the collection
      cSelectedAreaIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, AreaLayerIndex))
    Next
    If cSelectedAreaIndexes.Count = 0 Then
      'no areas selected, act as if all are selected
      For i = 1 To GisUtil.NumFeatures(AreaLayerIndex)
        cSelectedAreaIndexes.Add(i - 1)
      Next
    End If
    lArgs(3) = cSelectedAreaIndexes

    'make sure output folder exists
    If Not FileExists(aOutputFolder, True, False) Then
      MkDirPath(aOutputFolder)
    End If
    lArgs(4) = aOutputFolder & "\"

    'first create form for output
    Dim lfrmResult As New frmResult
    lArgs(5) = lfrmResult
    'now run script
    Scripting.Run("vb", "", pReportsColl(aReportIndex), lError, False, pMapWin, lArgs)
    If Len(lError) > 0 Then
      Logger.Msg(lError)
    End If
  End Sub
End Class