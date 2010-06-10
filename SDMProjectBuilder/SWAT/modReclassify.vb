Imports System.Collections.Generic
Imports MapWinUtility
Imports atcUtility

Friend Module modReclassify
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aSlopeInputGridName"></param>
    ''' <returns></returns>
    ''' <remarks>based on code from frmReclassifySlope in frmLUSoilsSlope in mwSWATPluginBASINS40</remarks>
    Friend Function ReclassifySlope(ByVal aSlopeInputGridName As String, _
                                    ByVal aReclassiflyScheme As Generic.List(Of Double), _
                                    ByVal aSlopeReclassifyGridName As String) As Boolean
        Dim lResult As Boolean = False
        Dim lGrid As New MapWinGIS.Grid
        If lGrid.Open(aSlopeInputGridName) Then
            Try
                Dim lGridHeader As New MapWinGIS.GridHeader
                lGridHeader.CopyFrom(lGrid.Header)
                Dim lReclassifyGrid As New MapWinGIS.Grid
                lReclassifyGrid.CreateNew(aSlopeReclassifyGridName, lGrid.Header, lGrid.DataType, 0, True, MapWinGIS.GridFileType.GeoTiff)
                For lRow As Integer = 0 To lGrid.Header.NumberRows - 1
                    For lCol As Integer = 0 To lGrid.Header.NumberCols - 1
                        Dim lGridValue As Double = lGrid.Value(lCol, lRow)
                        For lIndex As Integer = 0 To aReclassiflyScheme.Count - 1
                            If lGridValue <= aReclassiflyScheme(lIndex) Then
                                lReclassifyGrid.Value(lCol, lRow) = lIndex
                                Exit For
                            End If
                        Next
                    Next
                Next
                lResult = lReclassifyGrid.Save(aSlopeReclassifyGridName, MapWinGIS.GridFileType.GeoTiff, Nothing)
                lReclassifyGrid.Close()
            Catch lEx As Exception
                Logger.Msg(lEx.Message, MsgBoxStyle.Critical, "Reclassify Slope")
            End Try
        End If
        Return lResult
    End Function

End Module
