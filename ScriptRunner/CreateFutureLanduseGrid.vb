Imports atcUtility
Imports atcData
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized

Module CreateFutureLanduseGrid
    Private pCurrentCBPGridName As String = ""
    Private pCurrentICLUSGridName As String = ""
    Private pFutureCBPGridName As String = ""
    Private pFutureICLUSGridName As String = ""

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        'this script creates a future scenario CBP land use grid 
        'based on changes to the ICLUS housing density grids

        'initialize the random-number generator
        Rnd(-1.0)
        Randomize(1.0)

        GisUtil.MappingObject = aMapWin

        Dim lCurrentCBPGrid As New MapWinGIS.Grid
        lCurrentCBPGrid.Open(pCurrentCBPGridName)

        Dim lCurrentICLUSGrid As New MapWinGIS.Grid
        lCurrentICLUSGrid.Open(pCurrentICLUSGridName)

        Dim lFutureICLUSGrid As New MapWinGIS.Grid
        lFutureICLUSGrid.Open(pFutureICLUSGridName)

        FileCopy(pCurrentCBPGridName, pFutureCBPGridName)
        Dim lFutureCBPGrid As New MapWinGIS.Grid
        lFutureCBPGrid.Open(pFutureCBPGridName)

        'go through each cell of the current cbp grid
        Dim lX As Double, lY As Double
        Dim lIclusCol As Integer, lIclusRow As Integer
        Dim lFutureIclusCol As Integer, lFutureIclusRow As Integer
        Dim lCurrentCBPValue As Integer
        Dim lFutureCBPValue As Integer
        Dim lCurrentIclusValue As Integer
        Dim lFutureIclusValue As Integer
        For lRow As Integer = 1 To lCurrentCBPGrid.Header.NumberRows
            For lCol As Integer = 1 To lCurrentCBPGrid.Header.NumberCols
                lCurrentCBPValue = lCurrentCBPGrid.Value(lCol, lRow)
                'find corresponding cell of current iclus grid
                lCurrentCBPGrid.CellToProj(lCol, lRow, lX, lY)
                lCurrentICLUSGrid.ProjToCell(lX, lY, lIclusCol, lIclusRow)
                'get the current iclus value at that spot
                lCurrentIclusValue = lCurrentICLUSGrid.Value(lIclusCol, lIclusRow)
                'find corresponding cell of future iclus grid
                lFutureICLUSGrid.ProjToCell(lX, lY, lFutureIclusCol, lFutureIclusRow)
                'get the future iclus value at that spot
                lFutureIclusValue = lFutureICLUSGrid.Value(lFutureIclusCol, lFutureIclusRow)

                If lFutureIclusValue <> lCurrentIclusValue Then
                    'the iclus value has changed, figure out what to call this cell in the future CBP grid
                    lFutureCBPValue = FutureCBPValue(lCurrentCBPValue, lCurrentIclusValue, lFutureIclusValue)
                    lFutureCBPGrid.Value(lCol, lRow) = lFutureCBPValue
                End If

            Next
        Next

        'save the future cbp grid
        lFutureCBPGrid.Save()
        
    End Sub

    Private Function FutureCBPValue(ByVal aCurrentCBPValue As Integer, ByVal aCurrentIclusValue As Integer, ByVal aFutureIclusValue As Integer) As Integer
        'determine the future cbp value for a grid cell, based on the change of iclus value
        Dim lFutureCBPValue As Integer = aCurrentCBPValue

        If aFutureIclusValue <> aCurrentIclusValue Then
            'the iclus value changed

            'generate random value between 0 and 100
            Dim lRandomNumber As Single = 100 * Rnd()   'rnd returns number from 0.0 to 0.99999

            If aFutureIclusValue = 12 Then
                'iclus changed to 12
                If lRandomNumber < 23.6 Then
                    lFutureCBPValue = 2  'moderate intensity developed
                ElseIf lRandomNumber < 28.6 Then
                    lFutureCBPValue = 5  'deciduous forest
                ElseIf lRandomNumber < 31.9 Then
                    lFutureCBPValue = 8  'agriculture (resac)
                ElseIf lRandomNumber < 95.6 Then
                    lFutureCBPValue = 9  'high intensity developed
                ElseIf lRandomNumber < 97.6 Then
                    lFutureCBPValue = 10  'crops with manure
                Else
                    lFutureCBPValue = 15  'pasture
                End If
            ElseIf aCurrentIclusValue < 8 And aFutureIclusValue > 7 Then
                'iclus changed from 0-7 to 8-11
                If lRandomNumber < 78.2 Then
                    lFutureCBPValue = 2  'moderate intensity developed
                ElseIf lRandomNumber < 84.1 Then
                    lFutureCBPValue = 5  'deciduous forest
                ElseIf lRandomNumber < 87.5 Then
                    lFutureCBPValue = 8  'agriculture (resac)
                Else
                    lFutureCBPValue = 9  'high intensity developed
                End If
            ElseIf (aCurrentIclusValue > 0 And aCurrentIclusValue < 6) And (aFutureIclusValue = 0 Or aFutureIclusValue = 6 Or aFutureIclusValue = 7) Then
                'iclus changed from 1-5 to 0, 6, or 7
                If lRandomNumber < 26.3 Then
                    lFutureCBPValue = 2  'moderate intensity developed
                ElseIf lRandomNumber < 56.7 Then
                    lFutureCBPValue = 5  'deciduous forest
                ElseIf lRandomNumber < 59.8 Then
                    lFutureCBPValue = 6  'evergreen forest
                ElseIf lRandomNumber < 73.1 Then
                    lFutureCBPValue = 8  'agriculture (resac)
                ElseIf lRandomNumber < 79.2 Then
                    lFutureCBPValue = 9  'high intensity developed
                ElseIf lRandomNumber < 86.3 Then
                    lFutureCBPValue = 10  'crops with manure
                ElseIf lRandomNumber < 91.1 Then
                    lFutureCBPValue = 12  'hay
                Else
                    lFutureCBPValue = 15  'pasture
                End If
            End If

        End If

        FutureCBPValue = lFutureCBPValue
    End Function
End Module
