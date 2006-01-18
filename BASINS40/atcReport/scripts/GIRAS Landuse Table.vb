Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module GIRASLanduseTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, ByVal cSelectedAreaIndexes As Collection, ByVal aOutputPath As String)

    Dim AreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    'set land use index layer
    Dim LanduseLayerIndex As Long = GisUtil.LayerIndex("Land Use Index")
    Dim LandUseFieldIndex As Long = GisUtil.FieldIndex(LanduseLayerIndex, "COVNAME")
    Dim PathName As String = PathNameOnly(GisUtil.LayerFileName(LanduseLayerIndex)) & "\landuse"

    'figure out which land use tiles to list
    Dim cluTiles As New Collection
    Dim NewFileName As String
    Dim j As Integer
    Dim i As Integer
    For i = 1 To GisUtil.NumFeatures(LanduseLayerIndex)
      'loop thru each shape of land use index shapefile
      NewFileName = GisUtil.FieldValue(LanduseLayerIndex, i - 1, LandUseFieldIndex)
      For j = 1 To cSelectedAreaIndexes.Count
        If GisUtil.OverlappingPolygons(LanduseLayerIndex, i - 1, AreaLayerIndex, cSelectedAreaIndexes(j)) Then
          'yes, add it
          cluTiles.Add(NewFileName)
          Exit For
        End If
      Next j
    Next i

    Dim LandUseFieldName As String
    Dim bfirst As Boolean = True
    For j = 1 To cluTiles.Count
      'loop thru each land use tile
      NewFileName = PathName & "\" & cluTiles(j) & ".shp"
      GisUtil.AddLayer(NewFileName, cluTiles(j))
      LandUseFieldName = "LUCODE"
      If aAreaLayer <> "<none>" Then
        'do overlay
        GisUtil.Overlay(cluTiles(j), LandUseFieldName, aAreaLayer, aIDField, _
                    PathName & "\overlay.shp", bfirst)
        bfirst = False
      End If
    Next j

    'build collection of selected area ids
    Dim cSelectedAreaIds As New Collection
    Dim aIdFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aIDField)
    For i = 1 To cSelectedAreaIndexes.Count
      cSelectedAreaIds.Add(GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(i), aIdFieldIndex))
    Next i

    'if simple reclassifyfile exists, read it in
    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
    Dim ReclassifyFile As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
    Dim tmpDbf As IatcTable
    If FileExists(ReclassifyFile) Then
      ReclassifyFile = ReclassifyFile & "giras.dbf"
    Else
      ReclassifyFile = Mid(PathName, 1, 1) & ":\basins\etc\giras.dbf"
    End If
    Dim cRcode As New Collection
    Dim cRname As New Collection
    'open dbf file
    tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(ReclassifyFile)
    For i = 1 To tmpDbf.NumRecords
      tmpDbf.CurrentRecord = i
      cRcode.Add(tmpDbf.Value(1))
      cRname.Add(tmpDbf.Value(2))
    Next i

    'read landuse, subbasin (aoi), and area combinations for selected areas from overlay.dbf
    Dim cLugroup As New Collection
    Dim cSubid As New Collection
    Dim cArea As New Collection
    Dim incollection As Boolean
    tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(PathName & "\overlay.dbf")
    For i = 1 To tmpDbf.NumRecords
      tmpDbf.CurrentRecord = i
      incollection = False
      For j = 1 To cSelectedAreaIds.Count
        If tmpDbf.Value(2) = cSelectedAreaIds(j) Then
          incollection = True
          Exit For
        End If
      Next j
      If incollection Then
        'find lugroup that corresponds to this lucode
        Dim lugroup As String = ""
        For j = 1 To cRcode.Count
          If tmpDbf.Value(1) = cRcode(j) Then
            lugroup = cRname(j)
            Exit For
          End If
        Next j
        If Len(lugroup) > 0 Then
          cLugroup.Add(lugroup)
        Else
          cLugroup.Add("<No Data>")
        End If
        cSubid.Add(tmpDbf.Value(2))
        cArea.Add(tmpDbf.Value(3))
      End If
    Next i

    'build collection of unique subbasin ids
    Dim cUniqueSubids As New Collection
    For i = 1 To cSubid.Count
      incollection = False
      For j = 1 To cUniqueSubids.Count
        If cUniqueSubids(j) = cSubid(i) Then
          incollection = True
          Exit For
        End If
      Next j
      If Not incollection Then
        cUniqueSubids.Add(cSubid(i))
      End If
    Next i

    'build collection of unique landuse groups
    Dim cUniqueLugroups As New atcCollection
    For i = 1 To cLugroup.Count
      incollection = False
      For j = 0 To cUniqueLugroups.Count - 1
        If cUniqueLugroups(j) = cLugroup(i) Then
          incollection = True
          Exit For
        End If
      Next j
      If Not incollection Then
        cUniqueLugroups.Add(cLugroup(i))
      End If
    Next i
    'sort landuse groups
    cUniqueLugroups.Sort()

    'create summary array, area of each land use group in each subarea
    Dim lArea(cUniqueSubids.Count, cUniqueLugroups.Count) As Single
    Dim spos As Integer
    Dim lpos As Integer
    'loop through each polygon (or grid subid/lucode combination)
    'and populate array with area values
    For i = 1 To cSubid.Count
      'find subbasin position in the area array
      For j = 1 To cUniqueSubids.Count
        If cSubid(i) = cUniqueSubids(j) Then
          spos = j
          Exit For
        End If
      Next j
      'find lugroup that corresponds to this lucode
      For j = 1 To cUniqueLugroups.Count
        If cLugroup(i) = cUniqueLugroups(j - 1) Then
          lpos = j
          Exit For
        End If
      Next j
      lArea(spos, lpos) = lArea(spos, lpos) + cArea(i)
    Next i

    'now write file
    Dim OutFile As Integer
    Dim ctxt As String
    OutFile = FreeFile()
    FileOpen(OutFile, aOutputPath & "GIRAS Landuse Table.out", OpenMode.Output)
    PrintLine(OutFile, "Watershed Characterization Report")
    PrintLine(OutFile, "  GIRAS Landuse Distribution Within " & aAreaLayer)
    PrintLine(OutFile, "  (Area in Acres)")
    PrintLine(OutFile, "")
    'write area ids
    ctxt = ""
    For i = 1 To cUniqueSubids.Count
      ctxt = ctxt & vbTab & cUniqueSubids(i)
    Next i
    PrintLine(OutFile, ctxt)
    'write associated descriptions
    Dim aNameFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aNameField)
    ctxt = ""
    For i = 1 To cUniqueSubids.Count
      For j = 1 To cSelectedAreaIndexes.Count
        If cSelectedAreaIds(j) = cUniqueSubids(i) Then
          ctxt = ctxt & vbTab & GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(j), aNameFieldIndex)
          Exit For
        End If
      Next j
    Next i
    PrintLine(OutFile, ctxt)
    'now write data
    For j = 1 To cUniqueLugroups.Count
      ctxt = cUniqueLugroups(j - 1)
      For i = 1 To cUniqueSubids.Count
        ctxt = ctxt & vbTab & Format((lArea(i, j) / 4046.8564), "0.")
      Next i
      PrintLine(OutFile, ctxt)
    Next j
    FileClose(OutFile)

  End Sub
End Module
