Imports atcUtility
Imports MapWinUtility.Strings
Imports System.IO

Public Class clsUEBParameterFile

    'Public Shared ParameterName() As String = { _
    '    "irad: Radiation control flag (0=from ta, 1=input qsi, 2=input qsi, qli, 3=input qnet)", _
    '    "ireadalb: Albedo reading control flag (0=computed internally, 1=read in)", _
    '    "tr: Temp above which all precip is rain, deg C", _
    '    "ts: Temp below which all precip is snow, deg C", _
    '    "ems: Emissivity of snow", _
    '    "eg: Ground heat capacity, kJ/kg/deg C", _
    '    "z: Nominal measurement height for air temp & humidity, m", _
    '    "zo: Surface aerodynamic roughness, m", _
    '    "rho: Snow density, kg/m^3", _
    '    "rhog: Soil density, kg/m^3", _
    '    "lc: Liquid holding capacity of snow", _
    '    "ks: Snow saturated hydraulic conductivity, m/hr", _
    '    "de: Thermally active depth of soil, m", _
    '    "avo: Vsual new snow albedo", _
    '    "anir0: NIR new snow albedo", _
    '    "lans: Thermal conductivity of fresh (dry) snow, kJ/m/k/hr", _
    '    "lang: Thermal conductivity of soil, kJ/m/k/hr", _
    '    "wlf: Low frequency fluctuation in deep snow/soil layer, radian/hr", _
    '    "rdl: Amplitude correction coefficient of heat conduction", _
    '    "fstab: Stability correction control parameter", _
    '    "tref: Reference temp of soil layer in ground heat calc input, deg C", _
    '    "dnews: Threshold depth of new snow, m", _
    '    "emc: Emissivity of canopy", _
    '    "alpha: Scattering coefficient for solar radiation", _
    '    "alpha1: Scattering coefficient for long wave radiation", _
    '    "g: Leaf orientation with respect to zenith angle", _
    '    "uc: Unloading rate coefficient, 1/hr", _
    '    "as: Fraction of extraterestrial radiation on cloudy day", _
    '    "Bs: Fraction of extraterestrial radiation on clear day", _
    '    "lambda: Ratio of direct atm radiation to diffuse, worked out from Dingman", _
    '    "rimax: Maximum value of Richardson number for stability correction", _
    '    "wcoeff: Wind decay coefficient for the forest", _
    '    "a: A coefficient in Bristow-Campbell formula", _
    '    "c: C coefficient in Bristow-Campbell formula", _
    '    "stmflag: Model option for surface temperature approximation (1 - 4)", _
    '    "forwsflag: Wind speed in forest canopy flag (observed(1) or computed(2))"}

    'Public Shared NumParameters As Integer = ParameterName.Length
    Public Variables As Generic.List(Of clsUEBVariable)
    Public Header As String

    Public FileName As String

    Public Sub New(ByVal aFilename As String)

        FileName = aFilename
        Dim lFileContents As String
        If IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
        Else
            lFileContents = GetEmbeddedFileAsString("Param.dat")
        End If
        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")
        While lFileContents.Length > 0
            Variables.Add(clsUEBVariable.FromParameterString(lFileContents))
        End While
    End Sub

    Public Function WriteParameterFile() As Boolean

        Dim lStr As String = ""

        If FileName.Length > 0 Then
            Try
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.ParameterString
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
