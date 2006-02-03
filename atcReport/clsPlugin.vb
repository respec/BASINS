Imports atcControls
Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Collections.Specialized

Public Class PlugIn
  Inherits atcData.atcDataDisplay

  Friend pMapWin As MapWindow.Interfaces.IMapWin
  Private pReportsDir As String
  Private pReports As Collection

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

    'default reports directory 
    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
    Dim lReportsDir As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\reports\"
    If Mid(lReportsDir, 3, 5) = "\dev\" Then
      'change loc if in devel envir
      lReportsDir = "C:\dev\BASINS40\atcReport\scripts"
    End If
    ReportsDir = lReportsDir
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

  Public Function BuildReport(ByVal aAreaLayerName As String, _
                              ByVal aAreaIDFieldName As String, _
                              ByVal aAreaNameFieldName As String, _
                              ByVal aReportIndex As Integer) As Object
    Dim i As Integer
    Dim lAreaLayerIndex As Integer
    Dim lProblem As String = ""

    Dim lArgs(3) As Object
    'set area layer indexes
    Try
      lAreaLayerIndex = GisUtil.LayerIndex(aAreaLayerName)
      lArgs(0) = lAreaLayerIndex
      If Not GisUtil.LayerType(lAreaLayerIndex) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
        lProblem = "Layer '" & aAreaLayerName & "' is not a polygon shapefile."
        Logger.Dbg(lProblem)
      End If
    Catch
      lProblem = "Layer '" & aAreaLayerName & "' Not Found"
      Logger.Dbg(lProblem)
    End Try

    If Len(lProblem) = 0 Then
      Try
        lArgs(1) = GisUtil.FieldIndex(lAreaLayerIndex, aAreaIDFieldName)
      Catch
        lProblem = "Field '" & aAreaIDFieldName & "' Not Found in Layer '" & aAreaLayerName & "'"
        Logger.Dbg(lProblem)
      End Try
    End If

    If Len(lProblem) = 0 Then
      Try
        lArgs(2) = GisUtil.FieldIndex(lAreaLayerIndex, aAreaNameFieldName)
      Catch
        lProblem = "Field '" & aAreaNameFieldName & "' Not Found in Layer '" & aAreaLayerName & "'"
        Logger.Dbg(lProblem)
      End Try
    End If

    If Len(lProblem) = 0 Then
      'are any areas selected?
      Dim cSelectedAreaIndexes As New Collection
      For i = 1 To GisUtil.NumSelectedFeatures(lAreaLayerIndex)
        'add selected areas to the collection
        cSelectedAreaIndexes.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(i - 1, lAreaLayerIndex))
      Next
      If cSelectedAreaIndexes.Count = 0 Then
        'no areas selected, act as if all are selected
        For i = 1 To GisUtil.NumFeatures(lAreaLayerIndex)
          cSelectedAreaIndexes.Add(i - 1)
        Next
      End If
      lArgs(3) = cSelectedAreaIndexes

      'now run script
      Dim lError As String
      'Return ListedSegmentsTable.ScriptMain(lArgs(0), lArgs(1), lArgs(2), lArgs(3))
      Return Scripting.Run("vb", "", Reports(aReportIndex), lError, False, pMapWin, lArgs)

    End If

  End Function

  Public ReadOnly Property Reports() As Collection
    Get
      If pReports Is Nothing Then 'fill in first time thru
        Dim lAllFiles = New NameValueCollection
        AddFilesInDir(lAllFiles, pReportsDir, True, "*.vb")
        pReports = New Collection
        For Each lReport As String In lAllFiles
          pReports.Add(lReport)
        Next lReport
      End If
      Return pReports
    End Get
  End Property

  Public Property ReportsDir() As String
    Get
      Return pReportsDir
    End Get
    Set(ByVal aReportsDir As String)
      pReportsDir = aReportsDir
      pReports = Nothing 'forces read next time thru
    End Set
  End Property
End Class