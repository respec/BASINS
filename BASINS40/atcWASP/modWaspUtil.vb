Module modWaspUtil

    Public Function IntegerToAlphabet(ByVal aNumber As Integer) As String
        'given 1 returns 'A'
        'given 26 returns 'Z'
        'given 27 returns 'AA'
        'given 28 returns 'AB'
        Dim lResult As String = ""

        If (26 > aNumber) Then
            lResult = Chr(aNumber + 65)
        Else
            Dim lColumn As Integer
            Do
                lColumn = aNumber Mod 26
                aNumber = (aNumber \ 26) - 1
                lResult = Chr(lColumn + 65) + lResult
            Loop Until (aNumber < 0)
        End If

        Return lResult
    End Function

    Public Sub BreakLineIntoNthPart(ByVal aOrigX() As Double, ByVal aOrigY() As Double, ByVal aPieceIndex As Integer, ByVal aNumPieces As Integer, ByRef aNewX() As Double, ByRef aNewY() As Double)
        'break a line into specified number of segments, return the nth part 
        'given XY coords of vertices, return XY coords of new vertices 

        'first compute total length and distance between points
        Dim lTotalLength As Double = 0
        Dim lDistance(aOrigX.GetUpperBound(0)) As Double
        For lIndex As Integer = 1 To aOrigX.GetUpperBound(0)
            lDistance(lIndex) = System.Math.Sqrt(((aOrigX(lIndex) - aOrigX(lIndex - 1)) ^ 2) + ((aOrigY(lIndex) - aOrigY(lIndex - 1)) ^ 2))
            lTotalLength = lTotalLength + lDistance(lIndex)
        Next

        'find desired length
        Dim lDesiredLength As Double = lTotalLength / aNumPieces
        Dim lStartingDistance As Double = lTotalLength * (aPieceIndex - 1) / aNumPieces
        Dim lEndingDistance As Double = lStartingDistance + lDesiredLength

        'build arrays to store all points including new points
        Dim lXNew(aOrigX.GetUpperBound(0) + aNumPieces) As Double
        Dim lYNew(aOrigY.GetUpperBound(0) + aNumPieces) As Double

        Dim lCumDist As Double = 0.0
        Dim lPtCount As Integer = 0
        lXNew(0) = aOrigX(0)
        lYNew(0) = aOrigY(0)
        For lIndex As Integer = 1 To lDistance.GetUpperBound(0)
            lCumDist = lCumDist + lDistance(lIndex)
            If lCumDist < lStartingDistance Then
                'store this point in first position for future use
                lXNew(0) = aOrigX(lIndex - 1)
                lYNew(0) = aOrigY(lIndex - 1)
            ElseIf lCumDist > lStartingDistance And lCumDist < lEndingDistance Then
                'store this point and keep going
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex - 1)
                lYNew(lPtCount) = aOrigY(lIndex - 1)
            ElseIf lCumDist >= lEndingDistance Then
                'would be too much, store this point and stop
                lPtCount += 1
                lXNew(lPtCount) = aOrigX(lIndex - 1)
                lYNew(lPtCount) = aOrigY(lIndex - 1)
                Exit For
            End If
        Next

        'todo: pts 0 and lptcount need to be trimmed down to size

        'return an array of the proper size
        ReDim aNewX(lPtCount)
        ReDim aNewY(lPtCount)
        For lIndex As Integer = 0 To lPtCount
            aNewX(lIndex) = lXNew(lIndex)
            aNewY(lIndex) = lYNew(lIndex)
        Next

        '    If (lCumDist + lDistance(lIndex) > lDesiredLength) Then
        '        'would be too much, need to calculate end point for this piece
        '        aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 2) + ((aXOrig(lIndex - 1) - aXOrig(lIndex - 2)) * ((lDesiredLength - lCumDist) / lDistance(lIndex)))
        '        aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 2) + ((aYOrig(lIndex - 1) - aYOrig(lIndex - 2)) * ((lDesiredLength - lCumDist) / lDistance(lIndex)))
        '        aLineEndIndexes(lPiece + 1) = lIndex - 1 + lPiece  'save the index of this endpoint
        '        lPiece += 1
        '        lCumDist = lDistance(lIndex) * (1 - ((lDesiredLength - lCumDist) / lDistance(lIndex)))
        '        aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 1)
        '        aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 1)
        '    Else
        '        'not long enough yet, just add point
        '        aXNew(lIndex - 1 + lPiece) = aXOrig(lIndex - 1)
        '        aYNew(lIndex - 1 + lPiece) = aYOrig(lIndex - 1)
        '        lCumDist = lCumDist + lDistance(lIndex)
        '    End If

        'close out the last piece
        'aXNew(aXNew.GetUpperBound(0)) = aXOrig(aXOrig.GetUpperBound(0))
        'aYNew(aYNew.GetUpperBound(0)) = aYOrig(aYOrig.GetUpperBound(0))
        'aLineEndIndexes(aNumPieces) = aXNew.GetUpperBound(0)
    End Sub
End Module
