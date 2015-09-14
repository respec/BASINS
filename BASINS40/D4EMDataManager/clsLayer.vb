Imports atcUtility
Imports MapWinUtility
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

<CLSCompliant(False)> _
Public Class clsLayer
    Public FileName As String = ""
    Public Tag As String = ""
    Public IsRequired As Boolean = False
    Public IsShape As Boolean
    Public NeedsProjection As Boolean = True
    Public Grid As MapWinGIS.Grid
    Public ShapeFile As MapWinGIS.Shapefile
    Public IdFieldIndex As Integer = -1
    Public IdFieldName As String = ""
    Public NoData As String

    Private pRowCachedKeys As Integer = -1
    Private pCachedKeys() As String

    Public Sub New(ByVal aLayerFileName As String)
        Dim lLayerFileNamePartsSplitPipe() As String = aLayerFileName.Split("|")
        For Each lPipePart As String In lLayerFileNamePartsSplitPipe
            Dim lLayerFileNamePartsSplitEquals() As String = lPipePart.Split("=")

            Dim lSplitIndex As Integer = 0
            If lLayerFileNamePartsSplitEquals.Length = 2 Then
                Select Case lLayerFileNamePartsSplitEquals(0).ToUpper
                    Case "IDFIELD" : IdFieldIndex = CInt(lLayerFileNamePartsSplitEquals(1))
                    Case "IDNAME" : IdFieldName = lLayerFileNamePartsSplitEquals(1)
                    Case "REQUIRED" : IsRequired = CBool(lLayerFileNamePartsSplitEquals(1))
                    Case "TAG" : Tag = lLayerFileNamePartsSplitEquals(1)
                    Case Else
                        Tag = lLayerFileNamePartsSplitEquals(0)
                        FileName = lLayerFileNamePartsSplitEquals(1)
                End Select
            ElseIf IO.File.Exists(lLayerFileNamePartsSplitEquals(0)) Then
                FileName = lLayerFileNamePartsSplitEquals(0)
            End If
        Next
        Open()
        If IsShape Then
            If IdFieldIndex < 0 Then
                Throw New ApplicationException("Layer " & aLayerFileName & " missing IDFIELD")
            ElseIf IdFieldName.Length = 0 Then
                Throw New ApplicationException("Layer " & aLayerFileName & " missing IDNAME")
            ElseIf IdFieldName <> ShapeFile.Field(IdFieldIndex).Name Then
                Throw New Exception("Layer " & Tag & " FieldName " & IdFieldName & " and FieldIndex " & IdFieldIndex & " are incompatable")
            Else
                Logger.Dbg("ShapeLayer " & Tag & " opened, using ID field " & IdFieldName & " (" & IdFieldIndex & ")")
            End If
        Else
            Logger.Dbg("GridLayer " & Tag & " opened")
        End If
    End Sub

    Public Sub New(ByVal aTag As String, ByVal aGrid As MapWinGIS.Grid)
        Tag = aTag
        Grid = aGrid
        IsShape = False
    End Sub

    Public Sub New(ByVal aTag As String, ByVal aShapeFile As MapWinGIS.Shapefile)
        Tag = aTag
        ShapeFile = aShapeFile
        IsShape = True
    End Sub

    Public Function Key(ByVal aGrid As MapWinGIS.Grid, ByVal aRow As Integer, ByVal aCol As Integer) As String
        If IsShape Then
            If aRow <> Me.pRowCachedKeys Then
                'Here program will check the x, y coordinates and find the shape value
                Dim lX As Double
                Dim lY As Double
                Dim lLastShapeIndex As Integer = -1
                Dim lLastCol As Integer = aGrid.Header.NumberCols
                ReDim pCachedKeys(lLastCol)
                Me.ShapeFile.BeginPointInShapefile()
                For lCol As Integer = 0 To lLastCol
                    aGrid.CellToProj(lCol, aRow, lX, lY)
                    Dim lShapeIndex As Integer = -1
                    If lLastShapeIndex > -1 Then
                        If Me.ShapeFile.PointInShape(lLastShapeIndex, lX, lY) Then
                            lShapeIndex = lLastShapeIndex
                        End If
                    End If
                    If lShapeIndex = -1 Then
                        lShapeIndex = Me.ShapeFile.PointInShapefile(lX, lY)
                        lLastShapeIndex = lShapeIndex
                    End If
                    Dim lId As String
                    If lShapeIndex = -1 Then
                        lId = ""
                    Else
                        lId = Me.ShapeFile.CellValue(Me.IdFieldIndex, lShapeIndex)
                    End If
                    If lId Is Nothing Then
                        lId = ""
                    End If
                    pCachedKeys(lCol) = lId
                Next
                Me.ShapeFile.EndPointInShapefile()
                Me.pRowCachedKeys = aRow
            End If
            Return pCachedKeys(aCol)
        Else
            Dim lKey As String
            If NeedsProjection Then
                Dim lCol As Integer
                Dim lRow As Integer
                Dim lX As Double
                Dim lY As Double
                aGrid.CellToProj(aCol, aRow, lX, lY)
                Me.Grid.ProjToCell(lX, lY, lCol, lRow)
                lKey = Me.Grid.Value(lCol, lRow)
            Else
                lKey = Me.Grid.Value(aCol, aRow)
            End If
            If lKey = NoData Then
                lKey = ""
            End If
            Return lKey
        End If
    End Function

    Public Function MatchesGrid(ByVal aGrid As MapWinGIS.Grid) As Boolean
        If Not Me.IsShape Then
            Try
                Dim lLastCol As Integer = Me.Grid.Header.NumberCols
                Dim lLastRow As Integer = Me.Grid.Header.NumberRows
                If aGrid.Header.NumberCols = lLastCol AndAlso _
                    aGrid.Header.NumberRows = lLastRow Then
                    Dim lCol As Integer, lRow As Integer
                    Dim lX As Double, lY As Double
                    aGrid.CellToProj(0, 0, lX, lY)
                    Me.Grid.ProjToCell(lX, lY, lCol, lRow)
                    If lCol = 0 AndAlso lRow = 0 Then
                        aGrid.CellToProj(lLastCol, lLastRow, lX, lY)
                        Me.Grid.ProjToCell(lX, lY, lCol, lRow)
                        Return (lCol = lLastCol AndAlso lRow = lLastRow)
                    End If
                End If
            Catch e As Exception
                Logger.Dbg("MatchesGrid Exception: " & e.Message)
            End Try
        End If
        Return False
    End Function

    Private Sub Open()
        If Not FileExists(FileName) Then
            Throw New ApplicationException("Cannot open layer, file does not exist: '" & FileName & "'")
        End If
        Select Case IO.Path.GetExtension(FileName)
            Case ".shp"
                ShapeFile = New MapWinGIS.Shapefile
                If ShapeFile.Open(FileName) Then
                    Logger.Dbg("Opened shapefile " & Tag & " = " & ShapeFile.Filename)
                    IsShape = True
                Else
                    Logger.Dbg("Could not open " & FileName & vbCr & ShapeFile.ErrorMsg(ShapeFile.LastErrorCode))
                End If
            Case Else
                Grid = New MapWinGIS.Grid
                If Grid.Open(FileName) Then
                    Logger.Dbg("Opened grid " & Tag & " = " & Grid.Filename)
                    NoData = Grid.Header.NodataValue.ToString
                Else
                    Dim lStr As String = "Could not open " & Tag & " = " & FileName & vbCr & Grid.ErrorMsg(Grid.LastErrorCode)
                    Logger.Dbg(lStr)
                    Throw New ApplicationException(lStr)
                End If
                IsShape = False
        End Select
    End Sub

    ''' <summary>
    ''' Reopening layer can conserve memory, especially if there is a leak
    ''' </summary>
    Public Sub Reopen()
        Close()
        Open()
    End Sub

    Public Sub Close()
        If Me.IsShape Then
            Me.ShapeFile.Close()
            ReleaseCom(Me.ShapeFile, "Shape" & FileName)
            Logger.Dbg("Shape " & FileName & MemUsage()) : Logger.Flush()
            Me.ShapeFile = Nothing
        Else
            Me.Grid.Close()
            ReleaseCom(Me.Grid, "Grid" & FileName)
            Logger.Dbg("Grid " & FileName & MemUsage()) : Logger.Flush()
            Me.Grid = Nothing
        End If
    End Sub

    Public Function Reclassify(ByVal aReclassiflyScheme As atcCollection, _
                               ByVal aReclassifyGridName As String, _
                               Optional ByVal aNoKeyNoData As Boolean = False) As Boolean
        Dim lResult As Boolean = False
        If Not Me.IsShape Then
            Try
                Dim lReclassifyGridHeader As New MapWinGIS.GridHeader
                lReclassifyGridHeader.CopyFrom(Me.Grid.Header)
                Dim lReclassifyGrid As New MapWinGIS.Grid
                lReclassifyGrid.CreateNew(aReclassifyGridName, lReclassifyGridHeader, Me.Grid.DataType, 0, True, MapWinGIS.GridFileType.GeoTiff)
                With Me.Grid
                    For lRow As Integer = 0 To .Header.NumberRows - 1
                        For lCol As Integer = 0 To .Header.NumberCols - 1
                            Dim lGridValue As Double = .Value(lCol, lRow)
                            Dim lKeyIndex As Integer = aReclassiflyScheme.IndexFromKey(lGridValue)
                            If lKeyIndex = -1 Then
                                If aNoKeyNoData Then
                                    lReclassifyGrid.Value(lRow, lCol) = lReclassifyGrid.Header.NodataValue
                                Else
                                    lReclassifyGrid.Value(lCol, lRow) = lGridValue
                                End If
                            Else
                                lReclassifyGrid.Value(lCol, lRow) = aReclassiflyScheme.ItemByIndex(lKeyIndex)
                            End If
                        Next
                    Next
                End With
                lResult = lReclassifyGrid.Save(aReclassifyGridName, MapWinGIS.GridFileType.GeoTiff, Nothing)
                lReclassifyGrid.Close()
            Catch lEx As ApplicationException
                Throw New Exception("ReclassifyRangeExcepton " & lEx.Message)
            End Try
        End If
        Return lResult
    End Function

    Public Function ReclassifyRange(ByVal aReclassiflyScheme As ArrayList, _
                                    ByVal aReclassifyGridName As String) As Boolean
        Dim lResult As Boolean = False
        If Not Me.IsShape Then
            Try
                Dim lReclassifyGridHeader As New MapWinGIS.GridHeader
                lReclassifyGridHeader.CopyFrom(Me.Grid.Header)
                Dim lReclassifyGrid As New MapWinGIS.Grid
                lReclassifyGrid.CreateNew(aReclassifyGridName, lReclassifyGridHeader, MapWinGIS.GridDataType.ShortDataType, 0, True, MapWinGIS.GridFileType.GeoTiff)
                With Me.Grid
                    For lRow As Integer = 0 To .Header.NumberRows - 1
                        For lCol As Integer = 0 To .Header.NumberCols - 1
                            Dim lGridValue As Double = .Value(lCol, lRow)
                            For lIndex As Integer = 0 To aReclassiflyScheme.Count - 1
                                If lGridValue <= aReclassiflyScheme(lIndex) Then
                                    lReclassifyGrid.Value(lCol, lRow) = lIndex
                                    Exit For
                                End If
                            Next
                        Next
                    Next
                End With
                lResult = lReclassifyGrid.Save(aReclassifyGridName, MapWinGIS.GridFileType.GeoTiff, Nothing)
                lReclassifyGrid.Close()
            Catch lEx As ApplicationException
                Throw New Exception("ReclassifyRangeExcepton " & lEx.Message)
            End Try
        End If
        Return lResult
    End Function

    Private Shared Sub ReleaseCom(ByVal aObject As Object, Optional ByVal aName As String = "", Optional ByVal aDebug As Boolean = True)
        While System.Runtime.InteropServices.Marshal.ReleaseComObject(aObject) <> 0
            If aName.Length > 0 And aDebug Then
                Logger.Dbg(aName & " Release " & MemUsage()) : Logger.Flush()
            End If
        End While
        If aName.Length And aDebug > 0 Then
            Logger.Dbg(aName & " Release " & MemUsage()) : Logger.Flush()
        End If
    End Sub
End Class