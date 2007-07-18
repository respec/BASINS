Imports atcData
Imports atcData.atcDataGroup
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptLineDump
    Private pTestPath As String = "D:\GisData\SERDP"
    Private Const pFormat As String = "#,##0.000"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        'change to the directory of the current project
        ChDriveDir(pTestPath)
        Logger.Dbg(" CurDir:" & CurDir())
        Dim lString As New Text.StringBuilder

        Dim lCrossSections As New MapWinGIS.Shapefile
        Dim lCrossSectionShapeName As String = "CrossSections"
        lCrossSections.Open(lCrossSectionShapeName & ".shp")
        Dim dX, dY, dH As Double

        For lXSectIndex As Integer = 0 To lCrossSections.NumShapes - 1
            lString.Append("XSection " & lXSectIndex & vbCrLf)
            Dim lShape As MapWinGIS.Shape = lCrossSections.Shape(lXSectIndex)
            dH = 0
            Dim lBasePoint As MapWinGIS.Point = lShape.Point(0)
            For lIndex As Integer = 0 To lShape.numPoints - 1
                Dim lPoint As MapWinGIS.Point = lShape.Point(lIndex)
                If lIndex > 0 Then
                    dX = lPoint.x - lBasePoint.x
                    dY = lPoint.y - lBasePoint.y
                    dH = Math.Sqrt((dX ^ 2) + (dY ^ 2)) 'meters
                End If
                lString.Append(DoubleToString(lPoint.x, 14, , , , 10) & vbTab & _
                               DoubleToString(lPoint.y, 14, , , , 10) & vbTab & _
                               DoubleToString(dH, , pFormat, , , 6) & vbTab & _
                               DoubleToString(lPoint.Z, , pFormat, , , 6) & vbCrLf)
            Next
        Next

        SaveFileString("XSectionDump.txt", lString.ToString)
    End Sub

    Sub ReportPoints(ByVal aX As Double, ByVal aY As Double, ByVal aPointCollection As atcCollection, ByRef aString As Text.StringBuilder, Optional ByVal aPos As Double = -1)
        If aPointCollection.Count > 1 Then
            Dim lRatio As Double = aPointCollection(0) / aPointCollection(1)
            If aPos > -1 Then
                aString.Append(DoubleToString(aPos, 5) & vbTab)
            End If
            aString.Append(DoubleToString(aX, 14, , , , 10) & vbTab & _
                           DoubleToString(aY, 14, , , , 10) & vbTab & _
                           DoubleToString(aPointCollection(0), , pFormat, , , 6) & vbTab & _
                           aPointCollection(1) & vbTab & _
                           DoubleToString(lRatio))
            For lIndex As Integer = 3 To aPointCollection.Count
                aString.Append(vbTab & DoubleToString(aPointCollection(lIndex - 1), , pFormat, , , 10))
            Next lIndex
            aString.AppendLine()
        End If
    End Sub
End Module
