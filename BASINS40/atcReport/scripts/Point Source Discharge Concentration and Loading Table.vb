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
Imports System.Collections.Specialized

Public Module PCSDischargeTable
  Public Sub ScriptMain(ByVal aAreaLayer As String, ByVal aIDField As String, ByVal aNameField As String, _
                        ByVal aSelectedAreaIndexes As Collection, ByVal aOutputPath As String, ByVal afrmOut As Object)

    'set area layer indexes
    Dim lAreaLayerIndex As Integer = GisUtil.LayerIndex(aAreaLayer)
    Dim lAreaIdFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aIDField)
    Dim lAreaNameFieldIndex As Integer = GisUtil.FieldIndex(lAreaLayerIndex, aNameField)

    'find PCS layer and needed fields
    Dim lPCSLayerIndex As Integer = GisUtil.LayerIndex("Permit Compliance System")
    Dim lNPDESFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "npdes")
    Dim lFacilityFieldIndex As Integer = GisUtil.FieldIndex(lPCSLayerIndex, "fac_name")
      
    'build grid source for results
    Dim lGridSource = New atcGridSource
    Dim ltitle1 As String
    Dim ltitle2 As String
    ltitle1 = "Watershed Characterization Report"
    ltitle2 = "Point Source Discharge Concentrations and Loadings Within " & aAreaLayer
    With lGridSource
      .Rows = 1
      .Columns = 9
      .FixedRows = 1
      .CellValue(0, 0) = "AreaID"
      .CellValue(0, 1) = "AreaName"
      .CellValue(0, 2) = "NPDES"
      .CellValue(0, 3) = "Facility Name"
      .CellValue(0, 4) = "Parameter"
      .CellValue(0, 5) = "Year"
      .CellValue(0, 6) = "Flow (MGD)"
      .CellValue(0, 7) = "Avg. Conc. (mg/l)"
      .CellValue(0, 8) = "Load (lbs/yr)"
    End With

    'open PCS loading dbfs
    Dim lpcspath As String = PathNameOnly(GisUtil.LayerFileName(lPCSLayerIndex)) & "\pcs\"
    Dim lallFiles As New NameValueCollection
    Dim lcNpdes As New Collection
    Dim lcParm As New Collection
    Dim lcYear As New Collection
    Dim lcFlow As New Collection
    Dim lcConc As New Collection
    Dim lcLoad As New Collection
    Dim ltmpDbf As IatcTable
    Dim i As Integer
    AddFilesInDir(lallFiles, lpcspath, False, "*.dbf")
    For Each lFilename As String In lallFiles
      'open dbf file
      ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lFilename)
      If ltmpDbf.NumFields > 5 Then
        For i = 1 To ltmpDbf.NumRecords
          ltmpDbf.CurrentRecord = i
          lcParm.Add(ltmpDbf.Value(1))
          lcNpdes.Add(ltmpDbf.Value(2))
          lcYear.Add(ltmpDbf.Value(3))
          lcConc.Add(ltmpDbf.Value(4))
          lcFlow.Add(ltmpDbf.Value(5))
          lcLoad.Add(ltmpDbf.Value(6))
        Next i
      End If
    Next

    'read in parm name table
    lpcspath = PathNameOnly(GisUtil.LayerFileName(lPCSLayerIndex)) & "\pcs3_prm.dbf"
    ltmpDbf = atcUtility.atcTableOpener.OpenAnyTable(lpcspath)
    Dim lcParmCode As New Collection
    Dim lcParmName As New Collection
    For i = 1 To ltmpDbf.NumRecords
      ltmpDbf.CurrentRecord = i
      lcParmCode.Add(ltmpDbf.Value(1))
      lcParmName.Add(ltmpDbf.Value(2))
    Next i

    Dim j As Integer
    Dim k As Integer
    Dim l As Integer
    Dim lPolygonIndex As Integer
    Dim lnpdes As String
    'loop through each selected polygon and pcs point looking for overlap
    For i = 1 To GisUtil.NumFeatures(lPCSLayerIndex)
      lPolygonIndex = GisUtil.PointInPolygon(lPCSLayerIndex, i, lAreaLayerIndex)
      If lPolygonIndex > -1 Then
        For j = 1 To aSelectedAreaIndexes.Count
          If aSelectedAreaIndexes(j) = lPolygonIndex Then
            'these overlap
            lnpdes = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lNPDESFieldIndex)
            For k = 1 To lcNpdes.Count
              If lnpdes = lcNpdes(k) Then
                'want to add this record
                lGridSource.rows = lGridSource.rows + 1
                lGridSource.CellValue(lGridSource.rows - 1, 0) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaIdFieldIndex)
                lGridSource.CellValue(lGridSource.rows - 1, 1) = GisUtil.FieldValue(lAreaLayerIndex, lPolygonIndex, lAreaNameFieldIndex)
                lGridSource.CellValue(lGridSource.rows - 1, 2) = lnpdes
                lGridSource.CellValue(lGridSource.rows - 1, 3) = GisUtil.FieldValue(lPCSLayerIndex, i - 1, lFacilityFieldIndex)
                lGridSource.CellValue(lGridSource.rows - 1, 4) = lcParm(k)
                'convert parm code to parm name
                For l = 1 To lcParmCode.Count
                  If Trim(lcParm(k)) = Trim(lcParmCode(l)) Then
                    lGridSource.CellValue(lGridSource.rows - 1, 4) = lcParmName(l)
                    Exit For
                  End If
                Next l
                lGridSource.CellValue(lGridSource.rows - 1, 5) = lcYear(k)
                lGridSource.CellValue(lGridSource.rows - 1, 6) = lcFlow(k)
                lGridSource.CellValue(lGridSource.rows - 1, 7) = lcConc(k)
                lGridSource.CellValue(lGridSource.rows - 1, 8) = lcLoad(k)
              End If
            Next k
            Exit For
          End If
        Next j
      End If
    Next i

    'write file
    SaveFileString(aOutputPath & "Point Source Discharge Concentration and Loading Table.out", _
       ltitle1 & vbCrLf & "  " & ltitle2 & vbCrLf & vbCrLf & lGridSource.ToString)

    'produce result grid
    If Not afrmOut Is Nothing Then
      afrmOut.InitializeResults(ltitle1, ltitle2, lGridSource)
      afrmOut.Show()
    End If

  End Sub

End Module

