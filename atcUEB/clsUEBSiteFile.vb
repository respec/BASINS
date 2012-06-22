Imports atcUtility
Imports MapWinUtility.Strings
Imports System.IO

Public Class clsUEBSiteFile

    'Public Shared VariableName() As String = { _
    '        "Uic: Energy content initial condition, kg m-3", _
    '        "Wic: Snow water equivalent initial condition, m", _
    '        "Tic: snow surface dimensionless age initial condition, m", _
    '        "df: Drift factor multiplier", _
    '        "apr: Average atmospheric pressure, Pa", _
    '        "Aep: Albedo extinction coefficient", _
    '        "cc: Canopy coverage fraction", _
    '        "hcan: Canopy height", _
    '        "lai: Leaf area index", _
    '        "Sbar: Maximum snow load held per unit branch area", _
    '        "yeage: Forest age flag for wind speed profile parameterization", _
    '        "slope: A 2-D grid that contains the slope at each grid point", _
    '        "aspect: A 2-D grid that contains the aspect at each grid point", _
    '        "latitude: A 2-D grid that contains the latitude at each grid point", _
    '        "longitude: A 2-D grid that contains the longitude at each grid point", _
    '        "subalb: Albedo (fraction 0-1) of the substrate beneath the snow (ground or glacier)", _
    '        "subtype: Type of beneath snow substrate (0-Ground, 1-Clean ice, 2-Debris ice, 3-Glacier/snow zone", _
    '        "gsurf: The fraction of surface melt that runs off (e.g. from a glacier)"}

    'Public Shared NumVariables As Integer = VariableName.Length
    Public Variables As Generic.List(Of clsUEBVariable)
    Public Header As String

    Public FileName As String

    Public Sub New(ByVal aFilename As String)

        FileName = aFilename
        Dim lFileContents As String
        If IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
        Else
            lFileContents = GetEmbeddedFileAsString("SiteInit.dat")
        End If
        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")
        Variables = New Generic.List(Of clsUEBVariable)
        While lFileContents.Length > 0
            Variables.Add(clsUEBVariable.FromSiteVariableString(lFileContents))
        End While
    End Sub

    Public Function WriteSiteFile() As Boolean

        Dim lStr As String = Header & vbCrLf

        If FileName.Length > 0 Then
            Try
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.SiteVariableString
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
