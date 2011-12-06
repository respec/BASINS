Imports MapWinUtility.Strings

Public Class clsUEBSiteFile

    Public Shared VariableName() As String = { _
            "Forest cover fraction", _
            "Drift factor", _
            "Atmospheric pressure, Pa", _
            "Ground heat flux, kJ/m^2/hr", _
            "Albedo extinction depth, m", _
            "Slope, degrees", _
            "Aspect, degrees from North", _
            "Latitude, degrees"}

    Public Shared NumVariables As Integer = VariableName.Length
    Public VariableValue(NumVariables - 1) As Double

    Public FileName As String

    Public Function WriteSiteFile() As Boolean

        Dim lStr As String = ""

        If FileName.Length > 0 Then
            Try
                For i As Integer = 0 To UBound(VariableValue)
                    lStr &= VariableValue(i) & " "
                    If ((i + 1) Mod 5) = 0 Then 'make a new line
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
