Imports atcMwGisUtility
Imports atcModelSetup
Imports MapWinUtility
Imports atcUtility
Imports atcControls

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces

Public Module NLCDLanduseTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, _
                        ByVal aSelectedAreaIndexes As Collection, ByVal aOutputPath As String, ByVal afrmOut As Object)

    'set area layer indexes
    Dim lAreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim laIdFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aIDField)

    'find appropriate lu grid layer
    Dim i As Integer
    Dim lLanduseLayerIndex As Integer
    For i = 0 To GisUtil.NumLayers() - 1
      If GisUtil.LayerType(i) = 4 Then
        'this is a grid 
        If InStr(GisUtil.LayerFileName(i), "\nlcd\") > 0 Then
          'looks like an nlcd grid, lets use it
          lLanduseLayerIndex = i
        End If
      End If
    Next

    'tabulate the areas 
    Dim k As Integer = System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
    Dim laAreaLS(k, GisUtil.NumFeatures(lAreaLayerIndex)) As Double
    GisUtil.TabulateAreas(lLanduseLayerIndex, lAreaLayerIndex, laAreaLS)

    'build collection of selected area ids
    Dim lSelectedAreaIds As New Collection
    For i = 1 To aSelectedAreaIndexes.Count
      lSelectedAreaIds.Add(GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(i), laIdFieldIndex))
    Next i

    'if simple reclassifyfile exists, read it in
    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
    Dim lReclassifyFile As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "etc\"
    Dim ltmpDbf As IatcTable
    If FileExists(lReclassifyFile) Then
      lReclassifyFile = lReclassifyFile & "nlcd.dbf"
    Else
      lReclassifyFile = Mid((GisUtil.LayerFileName(lLanduseLayerIndex)), 1, 1) & ":\basins\etc\nlcd.dbf"
    End If
    Dim lcRcode As New Collection
    Dim lcRname As New Collection
    'open dbf file
    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lReclassifyFile)
    For i = 1 To ltmpDbf.NumRecords
      ltmpDbf.CurrentRecord = i
      lcRcode.Add(ltmpDbf.Value(1))
      lcRname.Add(ltmpDbf.Value(2))
    Next i

    'read landuse, subbasin (aoi), and area combinations for selected areas 
    Dim lcLugroup As New Collection
    Dim lcSubid As New Collection
    Dim lcArea As New Collection
    Dim lincollection As Boolean
    Dim lsubid As Integer
    Dim llugroup As String
    Dim j As Integer
    For k = 1 To aSelectedAreaIndexes.Count
      'loop thru each selected subbasin (or all if none selected)
      lsubid = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(k), laIdFieldIndex)
      For i = 1 To System.Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
        If laAreaLS(i, aSelectedAreaIndexes(k)) > 0 Then
          'find lugroup that corresponds to code i
          llugroup = ""
          For j = 1 To lcRcode.Count
            If i = lcRcode(j) Then
              llugroup = lcRname(j)
              Exit For
            End If
          Next j
          If Len(llugroup) > 0 Then
            lcLugroup.Add(llugroup)
          Else
            lcLugroup.Add("<No Data>")
          End If
          lcArea.Add(laAreaLS(i, aSelectedAreaIndexes(k)))
          lcSubid.Add(lsubid)
        End If
      Next i
    Next k

    'build collection of unique subbasin ids
    Dim lcUniqueSubids As New Collection
    For i = 1 To lcSubid.Count
      lincollection = False
      For j = 1 To lcUniqueSubids.Count
        If lcUniqueSubids(j) = lcSubid(i) Then
          lincollection = True
          Exit For
        End If
      Next j
      If Not lincollection Then
        lcUniqueSubids.Add(lcSubid(i))
      End If
    Next i

    'build collection of unique landuse groups
    Dim lcUniqueLugroups As New atcCollection
    For i = 1 To lcLugroup.Count
      lincollection = False
      For j = 0 To lcUniqueLugroups.Count - 1
        If lcUniqueLugroups(j) = lcLugroup(i) Then
          lincollection = True
          Exit For
        End If
      Next j
      If Not lincollection Then
        lcUniqueLugroups.Add(lcLugroup(i))
      End If
    Next i
    'sort landuse groups
    lcUniqueLugroups.Sort()

    'create summary array, area of each land use group in each subarea
    Dim lArea(lcUniqueSubids.Count, lcUniqueLugroups.Count) As Single
    Dim lspos As Integer
    Dim llpos As Integer
    'loop through each polygon (or grid subid/lucode combination)
    'and populate array with area values
    For i = 1 To lcSubid.Count
      'find subbasin position in the area array
      For j = 1 To lcUniqueSubids.Count
        If lcSubid(i) = lcUniqueSubids(j) Then
          lspos = j
          Exit For
        End If
      Next j
      'find lugroup that corresponds to this lucode
      For j = 1 To lcUniqueLugroups.Count
        If lcLugroup(i) = lcUniqueLugroups(j - 1) Then
          llpos = j
          Exit For
        End If
      Next j
      lArea(lspos, llpos) = lArea(lspos, llpos) + lcArea(i)
    Next i

    'build grid source for results
    Dim lGridSource = New atcGridSource
    Dim ltitle1 As String
    Dim ltitle2 As String
    ltitle1 = "Watershed Characterization Report"
    ltitle2 = "NLCD Landuse Distribution Within " & aAreaLayer & " (Area in Acres)"
    With lGridSource
      .Rows = 2
      .Columns = lcUniqueSubids.Count + 1
      .FixedRows = 1
      'write area ids
      For i = 1 To lcUniqueSubids.Count
        .CellValue(0, i) = lcUniqueSubids(i)
      Next i
      'write associated descriptions
      Dim laNameFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aNameField)
      For i = 1 To lcUniqueSubids.Count
        For j = 1 To aSelectedAreaIndexes.Count
          If lSelectedAreaIds(j) = lcUniqueSubids(i) Then
            .CellValue(1, i) = GisUtil.FieldValue(lAreaLayerIndex, aSelectedAreaIndexes(j), laNameFieldIndex)
            Exit For
          End If
        Next j
      Next i
      'now write data
      For j = 1 To lcUniqueLugroups.Count
        .rows = .rows + 1
        .CellValue(.rows - 1, 0) = lcUniqueLugroups(j - 1)
        For i = 1 To lcUniqueSubids.Count
          .CellValue(.rows - 1, i) = Format((lArea(i, j) / 4046.8564), "0.")
        Next i
      Next j
    End With

    'write file
    SaveFileString(aOutputPath & "NLCD Landuse Table.out", _
       ltitle1 & vbCrLf & "  " & ltitle2 & vbCrLf & vbCrLf & lGridSource.ToString)

    'produce result grid
    If Not afrmOut Is Nothing Then
      afrmOut.InitializeResults(ltitle1, ltitle2, lGridSource)
      afrmOut.Show()
    End If

  End Sub
End Module
