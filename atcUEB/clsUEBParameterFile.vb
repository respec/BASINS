Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBParameterFile

    Public Shared ParameterName() As String = { _
        "Temp above which all precip is rain, deg C", _
        "Temp below which all precip is snow, deg C", _
        "Emissivity of snow", _
        "Ground heat capacity, kJ/kg/deg C", _
        "Measurement height for Temp & Humidity, m", _
        "Surface aerodynamic roughness, m", _
        "Snow density, kg/m^3", _
        "Soil density, kg/m^3", _
        "Liquid holding capacity of snow", _
        "Snow saturated hydraulic conductivity, m/hr", _
        "Thermally active depth of soil, m", _
        "Bare ground albedo", _
        "Vsual new snow albedo", _
        "NIR new snow albedo", _
        "Thermal conductivity of fresh (dry) snow, kJ/m/k/hr", _
        "Thermal conductivity of soil, kJ/m/k/hr", _
        "Low frequency fluctuation in deep snow/soil layer, radian/hr", _
        "Amplitude correction coefficient of heat conduction", _
        "Stability correction control parameter", _
        "Reference temp of soil layer in ground heat calc input, deg C", _
        "Threshold depth of new snow, m", _
        "Fraction of surface melt that runs off"}

    Public Shared NumParameters As Integer = ParameterName.Length
    Public ParameterValue(NumParameters - 1) As Double

    Public FileName As String

    Public Sub ReadDataFile(ByVal aFilename As String)

        Dim lChrSep() As String = {" ", vbTab, vbCrLf}
        Dim lStr As String = WholeFileString(aFilename)
        lStr = ReplaceRepeats(lStr, " ") 'remove extra blanks
        Dim lStrArray() As String = lStr.Split(lChrSep, StringSplitOptions.None)
        ReDim ParameterValue(lStrArray.Length - 1)
        Dim j As Integer = 0
        For i As Integer = 0 To UBound(lStrArray)
            If IsNumeric(lStrArray(i)) Then
                ParameterValue(j) = CDbl(lStrArray(i))
                j += 1
            End If
        Next
        FileName = aFilename
    End Sub

    Public Function WriteParameterFile() As Boolean

        Dim lStr As String = ""

        If FileName.Length > 0 Then
            Try
                For i As Integer = 0 To UBound(ParameterValue)
                    lStr &= ParameterValue(i) & " "
                    If ((i + 1) Mod 8) = 0 Then 'make a new line
                        lStr &= vbCrLf
                    End If
                Next
                SaveFileString(FileName, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

End Class
