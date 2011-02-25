Public Class UniformDistribution
    Private pSeed As Int32
    Public PreviousValue As Single
    Public Sub New(ByVal aSeed As Int32)
        pSeed = aSeed
        Generate() 'defines previous value 
    End Sub

    ''' <summary>
    ''' Generate a random numbers ranging from 0.0 to 1.0.
    ''' In the process of calculating the random number, the seed (x1) is set to a new value.
    ''' This function implements the prime-modulus generator xi = 16807 xi Mod(2**(31) - 1)
    ''' using code which ensures that no intermediate result uses more than 31 bits
    ''' </summary>
    ''' <returns>random number between 0.0 and 1.0</returns>
    ''' <remarks>
    ''' the theory behind the code is summarized in Bratley, P., B.L. Fox and L.E. Schrage. 1983. A Guide to Simulation.
    ''' Springer-Verlag, New York. (pages 199-202), based on code in SWAT2005
    ''' </remarks>
    Public Function Generate() As Single
        Dim lX As Integer = pSeed / 127773
        pSeed = 16807 * (pSeed - lX * 127773) - lX * 2836
        If (pSeed < 0) Then pSeed += 2147483647
        PreviousValue = pSeed * 0.0000000004656612875
        Return PreviousValue
    End Function
End Class

Public Class TriangularDistribution
    Private pUniform As UniformDistribution
    Public Sub New(ByVal aSeed As Int32)
        pUniform = New UniformDistribution(aSeed)
    End Sub

    ''' <summary>
    ''' Generate a random number from a triangular distribution
    ''' given X axis points at start, end, and peak Y value
    ''' </summary>
    ''' <param name="aT1">lower limit for distribution</param>
    ''' <param name="aT2">monthly mean for distribution</param>
    ''' <param name="aT3">upper limit for distribution</param>
    ''' <returns>daily value generated for distribution</returns>
    ''' <remarks></remarks>
    Public Function Generate(ByVal aT1 As Single, ByVal aT2 As Single, ByVal aT3 As Single) As Single
        Dim lU3 As Single = aT2 - aT1
        Dim lRn As Single = pUniform.PreviousValue
        pUniform.Generate()
        Dim lY As Single = 2.0 / (aT3 - aT1)
        Dim lB2 As Single = aT3 - aT2
        Dim lB1 As Single = lRn / lY
        Dim lX1 As Single = lY * lU3 / 2.0

        Dim lXx As Single
        Dim lYy As Single
        Dim lAtri As Single
        If (lRn <= lX1) Then
            lXx = 2.0 * lB1 * lU3
            If (lXx <= 0) Then
                lYy = 0
            Else
                lYy = Math.Sqrt(lXx)
            End If
            lAtri = lYy + aT1
        Else
            lXx = lB2 * lB2 - 2.0 * lB2 * (lB1 - 0.5 * lU3)
            If (lXx <= 0) Then
                lYy = 0
            Else
                lYy = Math.Sqrt(lXx)
            End If
            lAtri = aT3 - lYy
        End If

        Dim lAmn As Single = (aT3 + aT2 + aT1) / 3.0
        lAtri = lAtri * aT2 / lAmn

        If (lAtri >= 1.0) Then
            lAtri = 0.99
        ElseIf (lAtri <= 0.0) Then
            lAtri = 0.001
        End If
        Return lAtri
    End Function
End Class
