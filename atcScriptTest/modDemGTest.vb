Imports atcData
Imports atcUtility
Imports Basins
Imports System
Imports System.Collections.Specialized
Imports System.Drawing
Imports System.Exception
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinGIS
Imports Microsoft.VisualBasic

Public Module modDemGTest
  'Dim pDataType As String = "DemG", pAggr As Integer = 4, pScale As Double = 1.0
  'Dim pDataType As String = "Ned", pAggr As Integer = 6, pScale As Double = 100
  Dim pDataType As String = "Ned", pAggr As Integer = 8, pScale As Double = 100

  Dim pStatusFile As String = "alldone.txt"
  Dim pStatus As String = "Start:" & pDataType & ":" & pAggr & ":" & pScale & vbCrLf
  Dim pDebugFile As String = "debug.txt"

  Dim pBaseDir As String = "F:\GeoTif\"
  Dim pDataDir As String = pBaseDir & pDataType & "\"
  Dim pBoundDir As String = pBaseDir & "boundary\"
  Dim pCheckDir As String = pBaseDir & pDataType & "Check\"

  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As atcBasinsPlugIn)
    ChDriveDir(pBaseDir)
    If FileExists(pStatusFile) Then Kill(pStatusFile)
    LogSetFileName(pDebugFile, True)
    LogDbg(pStatus)

    Dim lAllFiles As New NameValueCollection
    AddFilesInDir(lAllFiles, pDataDir, False, "*" & pDataType & ".tif")
    LogDbg("MatchFileCount " & lAllFiles.Count)
    Dim lHucStr As String
    Dim lPrevHuc As String = ""
    Dim lWriteCnt As Integer = 0 'just build and write one map due to memory leak issues

    Try
      Dim pDataDirLen As Integer = pDataDir.Length
      For Each lFile As String In lAllFiles
        lHucStr = lFile.Substring(pDataDirLen, pAggr)
        If Not lHucStr.Equals(lPrevHuc) Then
          If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
            lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
            If lWriteCnt > 0 Then
              Exit For
            End If
          End If
          lPrevHuc = lHucStr
        End If
      Next

      'For lHuc2 As Integer = 1 To 22
      '  LogDbg("Huc2 " & lHuc2)
      '  For lHuc4 As Integer = 0 To 40
      '    LogDbg("Huc4 " & lHuc2 & " " & lHuc4)
      '    If pAggr >= 6 Then
      '      For lHuc6 As Integer = 0 To 40
      '        LogDbg("Huc6 " & lHuc2 & " " & lHuc4 & " " & lHuc6)
      '        If pAggr = 6 Then
      '          lHucStr = Format(lHuc2, "00") & Format(lHuc4, "00") & Format(lHuc6, "00")
      '          If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
      '            lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
      '          End If
      '        Else
      '          For lHuc8 As Integer = 0 To 40
      '            lHucStr = Format(lHuc2, "00") & Format(lHuc4, "00") & Format(lHuc6, "00") & Format(lHuc8, "00")
      '            If FileExists(pDataDir & lHucStr & "ned.tif") Then
      '              If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
      '                LogDbg("Huc8 " & lHuc2 & " " & lHuc4 & " " & lHuc6 & " " & lHuc8)
      '                lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
      '              End If
      '            End If
      '            If lWriteCnt > 0 Then
      '              Exit For
      '            End If
      '          Next
      '        End If
      '        If lWriteCnt > 0 Then
      '          Exit For
      '        End If
      '      Next
      '    Else
      '      lHucStr = Format(lHuc2, "00") & Format(lHuc4, "00")
      '      If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
      '        lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
      '      End If
      '    End If
      '    If lWriteCnt > 0 Then
      '      Exit For
      '    End If
      '  Next
      '  If lWriteCnt > 0 Then
      '    Exit For
      '  End If
      'Next
      If lWriteCnt = 0 Then 'nothing to write
        AppendFileString(pStatusFile, pStatus & " Done:" & Now)
      End If
    Catch ex As Exception
      MsgBox("Error" & ex.ToString)
    End Try

    Application.Exit()
  End Sub

  Private Function ProcessMatchFiles(ByRef aBasinsPlugIn As atcBasinsPlugIn, ByVal aHucStr As String, ByVal aAllFiles As NameValueCollection) As Integer
    Dim lCurFiles As New NameValueCollection
    Dim lCnt As Integer = 0
    Dim lStr As String = ""
    Dim lWriteCnt As Integer = 0
    Dim lFile As String

    LogDbg("Processing " & aHucStr)

    With aBasinsPlugIn.MapWin
      .Layers.Clear()
      If aHucStr.Length = 8 Then
        lFile = PathNameOnly(aAllFiles.Item(0)) & "\" & aHucStr & "ned.tif"
        lCnt += 1
        Dim lGrid As New Grid
        LogDbg("Opening " & lFile)
        lGrid.Open(lFile)
        LogDbg("Adding")
        .Layers.Add(lGrid, pGridColorSchemeBlack(pScale))
        LogDbg("Added")
        .Layers.Item(.Layers.NumLayers - 1).UseTransparentColor = True
        '.Layers.Item(.Layers.NumLayers - 1).ColoringScheme = pGridColorSchemeBlack(pScale)
        LogDbg("RenderedBlack")
        .View.ZoomToMaxExtents()
        LogDbg("Zoomed")
        lGrid = Nothing
      Else
        For Each lFile In aAllFiles
          If FilenameNoPath(lFile).StartsWith(aHucStr) Then
            lStr &= FilenameNoPath(lFile).Substring(0, 8) & vbCrLf
            LogDbg("Match " & lStr)
            lCnt += 1
            Dim lGrid As New Grid
            LogDbg("Opening " & lFile)
            lGrid.Open(lFile)
            LogDbg("Adding")
            .Layers.Add(lGrid, pGridColorSchemeBlack(pScale))
            LogDbg("Added")
            .Layers.Item(.Layers.NumLayers - 1).UseTransparentColor = True
            '.Layers.Item(.Layers.NumLayers - 1).ColoringScheme = pGridColorSchemeBlack(pScale)
            LogDbg("RenderedBlack")
            lGrid = Nothing
          End If
        Next
      End If

      If .Layers.NumLayers > 0 Then
        'AddHucBoundaryLayers(aBasinsPlugIn, aHucStr.Length)
        Dim lHucLayerName As String = pBoundDir & "huc2-" & Left(aHucStr, 2) & ".shp"
        If Not FileExists(lHucLayerName) Then
          lHucLayerName = pBoundDir & "huc" & Len(aHucStr) & ".shp"
        End If
        LogDbg("Adding HUC layer " & lHucLayerName)
        aBasinsPlugIn.MapWin.Layers.Add(lHucLayerName)
        With .Layers.Item(.Layers.NumLayers - 1)
          .DrawFill = False
          .OutlineColor = Color.Red
          .LineOrPointSize = 1
        End With
        .View.ZoomToMaxExtents()
        LogDbg("Zoomed")

        .Reports.GetScreenPicture(.View.Extents).Save(pCheckDir & "Huc" & aHucStr & "B.bmp")
        SaveFileString(pCheckDir & "Huc" & aHucStr & ".log", lStr)
        LogDbg("SavedBlack")
        lWriteCnt += 1
        'For lCnt = 0 To .Layers.NumLayers - 1
        '  If .Layers(lCnt).LayerType = 4 Then 'eLayerType.Grid 
        '    .Layers.Item(lCnt).ColoringScheme = pGridColorScheme(pScale)
        '  End If
        'Next
        'LogDbg("RenderedColor")
        'AddStCoBoundaryLayers(aBasinsPlugIn)
        'LogDbg("AddedStCoBoundary")
        '.Reports.GetScreenPicture(.View.Extents).Save(pCheckDir & "Huc" & aHucStr & ".bmp")
        'LogDbg("SavedColor")
        'lStr = "HUC:" & aHucStr & " Count:" & lCnt & vbCrLf & lStr
        '.Project.Save(pCheckDir & "Huc" & aHucStr)
        LogDbg("SaveProject")
      End If
    End With

    Return lWriteCnt
  End Function

  Private Sub AddStCoBoundaryLayers(ByVal aBasinsPlugIn As atcBasinsPlugIn)
    With aBasinsPlugIn.MapWin
      .Layers.Add(pBoundDir & "st.shp")
      With .Layers.Item(.Layers.NumLayers - 1)
        .DrawFill = False
        .OutlineColor = Color.Yellow
        .LineOrPointSize = 2
      End With
      .Layers.Add(pBoundDir & "co.shp")
      With .Layers.Item(.Layers.NumLayers - 1)
        .DrawFill = False
        .OutlineColor = Color.Yellow
        .LineOrPointSize = 1
      End With
    End With
  End Sub

  Private Sub AddHucBoundaryLayers(ByVal aBasinsPlugIn As atcBasinsPlugIn, ByVal aHucLen As Integer)
    With aBasinsPlugIn.MapWin
      .Layers.Add(pBoundDir & "huc" & aHucLen & ".shp")
      With .Layers.Item(.Layers.NumLayers - 1)
        .DrawFill = False
        .OutlineColor = Color.Red
        .LineOrPointSize = 5
      End With
      .Layers.Add(pBoundDir & "huc8.shp")
      With .Layers.Item(.Layers.NumLayers - 1)
        .DrawFill = False
        .OutlineColor = Color.Magenta
        .LineOrPointSize = 1
      End With
    End With
  End Sub

  Private ReadOnly Property pGridColorScheme(Optional ByVal aScale As Double = 1) As GridColorScheme
    Get
      Static lGridColorScheme As GridColorScheme

      If lGridColorScheme Is Nothing Then 'make one
        lGridColorScheme = New GridColorScheme
        Dim lGridColorDummy As New GridColorScheme
        Dim lColorBreak As New MapWinGIS.GridColorBreak
        'order for mw is ?bgr
        With lGridColorScheme
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 128, 0, 0).ToArgb)
            .LowValue = 0 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 128, 0, 0).ToArgb)
            .HighValue = 0 * aScale
          End With
          .InsertBreak(lColorBreak)

          lColorBreak = New MapWinGIS.GridColorBreak
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 40, 85, 40).ToArgb)
            .LowValue = 1 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 80, 180, 80).ToArgb)
            .HighValue = 200 * aScale
          End With
          .InsertBreak(lColorBreak)

          lColorBreak = New MapWinGIS.GridColorBreak
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 80, 180, 80).ToArgb)
            .LowValue = 200 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 180, 210, 185).ToArgb)
            .HighValue = 500 * aScale
          End With
          .InsertBreak(lColorBreak)

          lColorBreak = New MapWinGIS.GridColorBreak
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 180, 210, 185).ToArgb)
            .LowValue = 500 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 180, 245, 245).ToArgb)
            .HighValue = 1000 * aScale
          End With
          .InsertBreak(lColorBreak)

          lColorBreak = New MapWinGIS.GridColorBreak
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 180, 245, 245).ToArgb)
            .LowValue = 1000 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 80, 140, 160).ToArgb)
            .HighValue = 2000 * aScale
          End With
          .InsertBreak(lColorBreak)

          lColorBreak = New MapWinGIS.GridColorBreak
          With lColorBreak
            .LowColor = Convert.ToUInt32(Color.FromArgb(0, 80, 140, 160).ToArgb)
            .LowValue = 2000 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(0, 10, 60, 100).ToArgb)
            .HighValue = 5000 * aScale
          End With
          .InsertBreak(lColorBreak)
        End With
      End If
      Return lGridColorScheme
    End Get
  End Property

  Private ReadOnly Property pGridColorSchemeBlack(Optional ByVal aScale As Double = 1) As GridColorScheme
    Get
      Static lGridColorSchemeBlack As GridColorScheme

      If lGridColorSchemeBlack Is Nothing Then 'make one
        lGridColorSchemeBlack = New GridColorScheme
        Dim lGridColorDummy As New GridColorScheme
        Dim lColorBreak As New MapWinGIS.GridColorBreak
        'order for mw is ?bgr
        With lGridColorSchemeBlack
          With lColorBreak
            .ColoringType = ColoringType.Gradient
            .LowColor = Convert.ToUInt32(Color.FromArgb(10, 10, 10, 10).ToArgb)
            .LowValue = -1.0E+20 * aScale
            .HighColor = Convert.ToUInt32(Color.FromArgb(10, 10, 10, 10).ToArgb)
            .HighValue = 1.0E+20 * aScale
          End With
          .InsertBreak(lColorBreak)
        End With
      End If
      Return lGridColorSchemeBlack
    End Get
  End Property
End Module