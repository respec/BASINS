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
  Dim pDataType As String = "DemG", pAggr as Integer = 4, pScale as double = 1.0
  'Dim pDataType As String = "Ned", pAggr As Integer = 6, pScale As Double = 100
  Dim pStatusFile As String = "alldone.txt"
  Dim pStatus As String = "Start:" & Now & ":" & pDataType & ":" & pAggr & ":" & pScale & vbCrLf

  Dim pBaseDir As String = "D:\Basins\data\GeoTif\"
  Dim pBoundDir As String = pBaseDir & "boundary\"
  Dim pCheckDir As String = pBaseDir & pDataType & "Check\"

  Public Sub Main(ByVal aDataManager As atcDataManager, ByVal aBasinsPlugIn As PlugIn)
    ChDriveDir(pBaseDir)
    If FileExists(pStatusFile) Then Kill(pStatusFile)

    Dim lAllFiles As New NameValueCollection
    AddFilesInDir(lAllFiles, ".", True, "*" & pDataType & ".tif")
    Dim lHucStr As String
    Dim lWriteCnt As Integer = 0 'just build and write one map due to memory leak issues

    Try
      For lHuc2 As Integer = 1 To 22
        For lHuc4 As Integer = 0 To 40
          If pAggr = 6 Then
            For lHuc6 As Integer = 0 To 40
              lHucStr = Format(lHuc2, "00") & Format(lHuc4, "00") & Format(lHuc6, "00")
              If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
                lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
                If lWriteCnt > 0 Then
                  Exit For
                End If
              End If
            Next
          Else
            lHucStr = Format(lHuc2, "00") & Format(lHuc4, "00")
            If Not FileExists(pCheckDir & "huc" & lHucStr & ".log") Then
              lWriteCnt = ProcessMatchFiles(aBasinsPlugIn, lHucStr, lAllFiles)
            End If
          End If
          If lWriteCnt > 0 Then
            Exit For
          End If
        Next
        If lWriteCnt > 0 Then
          Exit For
        End If
      Next
      If lWriteCnt = 0 Then 'nothing to write
        AppendFileString(pStatusFile, pStatus & " Done:" & Now)
      End If
    Catch
    End Try

    Application.Exit() 
  End Sub

  Private Function ProcessMatchFiles(ByRef aBasinsPlugIn As PlugIn, ByVal aHucStr As String, ByVal aAllFiles As NameValueCollection) As Integer
    Dim lCurFiles As New NameValueCollection
    Dim lCnt As Integer = 0
    Dim lStr As String = ""
    Dim lWriteCnt As Integer = 0

    With aBasinsPlugIn.MapWin
      .Layers.Clear()
      For Each lFile As String In aAllFiles
        If FilenameNoPath(lFile).StartsWith(aHucStr) Then
          lStr &= FilenameNoPath(lFile).Substring(0, 8) & vbCrLf
          lCnt += 1
          .Layers.Add(lFile)
          .Layers.Item(.Layers.NumLayers - 1).UseTransparentColor = True
          .Layers.Item(.Layers.NumLayers - 1).ColoringScheme = pGridColorScheme(pscale)
          .View.ZoomToMaxExtents()
        End If
      Next

      If .Layers.NumLayers > 0 Then
        AddBoundaryLayers(aBasinsPlugIn, aHucStr.Length)
        .Project.Save(pCheckDir & "Huc" & aHucStr)
        .Reports.GetScreenPicture(.View.Extents).Save(pCheckDir & "Huc" & aHucStr & ".bmp")
        lStr = "HUC:" & aHucStr & " Count:" & lCnt & vbCrLf & lStr
        SaveFileString(pCheckDir & "Huc" & aHucStr & ".log", lStr)
        lWriteCnt += 1
      End If
    End With

    Return lWriteCnt
  End Function

  Private Sub AddBoundaryLayers(ByVal aBasinsPlugIn As PlugIn, ByVal aHucLen As Integer)
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
End Module