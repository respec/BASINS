Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module NLCDLanduseTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, ByVal cSelectedAreaIndexes As Collection, ByVal aOutputPath As String)

    Dim AreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim aIdFieldIndex As Integer = GisUtil.FieldIndex(AreaLayerIndex, aIDField)

    'find appropriate lu grid layer
    Dim i As Integer
    Dim LanduseLayerIndex As Integer
    For i = 0 To GisUtil.NumLayers() - 1
      If GisUtil.LayerType(i) = 4 Then
        'this is a grid 
        If InStr(GisUtil.LayerFileName(i), "\nlcd\") > 0 Then
          'looks like an nlcd grid, lets use it
          LanduseLayerIndex = i
        End If
      End If
    Next

    'tabulate the areas 
    Dim k As Integer = System.Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseLayerIndex))
    Dim aAreaLS(k, GisUtil.NumFeatures(AreaLayerIndex)) As Double
    GisUtil.TabulateAreas(LanduseLayerIndex, AreaLayerIndex, aAreaLS)

    'build collection of selected area ids
    Dim cSelectedAreaIds As New Collection
    For i = 1 To cSelectedAreaIndexes.Count
      cSelectedAreaIds.Add(GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(i), aIdFieldIndex))
    Next i

    'if simple reclassifyfile exists, read it in
    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
    Dim ReclassifyFile As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
    Dim tmpDbf As IatcTable
    If FileExists(ReclassifyFile) Then
      ReclassifyFile = ReclassifyFile & "nlcd.dbf"
    Else
      ReclassifyFile = Mid((GisUtil.LayerFileName(LanduseLayerIndex)), 1, 1) & ":\basins\etc\nlcd.dbf"
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

    'read landuse, subbasin (aoi), and area combinations for selected areas 
    Dim cLugroup As New Collection
    Dim cSubid As New Collection
    Dim cArea As New Collection
    Dim incollection As Boolean
    Dim subid As Integer
    Dim lugroup As String
    Dim j As Integer
    For k = 1 To cSelectedAreaIndexes.Count
      'loop thru each selected subbasin (or all if none selected)
      subid = GisUtil.FieldValue(AreaLayerIndex, cSelectedAreaIndexes(k), aIdFieldIndex)
      For i = 1 To System.Convert.ToInt32(GisUtil.GridLayerMaximum(LanduseLayerIndex))
        If aAreaLS(i, cSelectedAreaIndexes(k)) > 0 Then
          'find lugroup that corresponds to code i
          lugroup = ""
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
          cArea.Add(aAreaLS(i, cSelectedAreaIndexes(k)))
          cSubid.Add(subid)
        End If
      Next i
    Next k

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
    FileOpen(OutFile, aOutputPath & "NLCD Landuse Table.out", OpenMode.Output)
    PrintLine(OutFile, "Watershed Characterization Report")
    PrintLine(OutFile, "  NLCD Landuse Distribution Within " & aAreaLayer)
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
