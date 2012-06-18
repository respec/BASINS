Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBVariable
    Public Description As String
    Public SpaceVarying As Boolean
    Public TimeVarying As Boolean
    Public Value As Double
    Public GridFileName As String
    Public GridVariableName As String
    Public TimeFileName As String

    Public Overrides Function ToString() As String
        Return Description
    End Function

    Public Function ParameterString() As String
        Return Description & vbCrLf & DoubleToString(Value) & vbCrLf
    End Function

    Public Shared Function FromParameterString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        lUEBVar.Description = StrSplit(aFileContents, vbCrLf, "")
        lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, ""))
        Return lUEBVar
    End Function

    Public Function SiteVariableString() As String
        If SpaceVarying Then
            Return Description & vbCrLf & "1" & vbCrLf & GridFileName & vbCrLf & GridVariableName
        Else
            Return Description & vbCrLf & "0" & vbCrLf & Value
        End If

    End Function

    Public Shared Function FromSiteVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        Dim lSpaceVaryingFlag As Integer

        lUEBVar.Description = StrSplit(aFileContents, vbCrLf, "")
        lSpaceVaryingFlag = Integer.Parse(StrSplit(aFileContents, vbCrLf, ""))
        If lSpaceVaryingFlag = 0 Then 'constant value
            lUEBVar.SpaceVarying = False
            lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, ""))
        Else
            lUEBVar.SpaceVarying = True
            lUEBVar.GridFileName = StrSplit(aFileContents, vbCrLf, "")
            lUEBVar.GridVariableName = StrSplit(aFileContents, vbCrLf, "")
        End If
        Return lUEBVar
    End Function

    Public Function InputVariableString() As String
        If SpaceVarying And TimeVarying Then
            Return Description & vbCrLf & "1" & vbCrLf & GridFileName & vbCrLf & GridVariableName
        ElseIf TimeVarying Then
            Return Description & vbCrLf & "0" & vbCrLf & TimeFileName
        Else
            Return Description & vbCrLf & "2" & vbCrLf & Value
        End If

    End Function

    Public Shared Function FromInputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        Dim lVaryingFlag As Integer

        lUEBVar.Description = StrSplit(aFileContents, vbCrLf, "")
        lVaryingFlag = Integer.Parse(StrSplit(aFileContents, vbCrLf, ""))
        If lVaryingFlag = 0 Then 'constant spatial timeseries
            lUEBVar.TimeVarying = True
            lUEBVar.SpaceVarying = False
            lUEBVar.TimeFileName = StrSplit(aFileContents, vbCrLf, "")
        ElseIf lVaryingFlag = 1 Then 'varying in space/time
            lUEBVar.TimeVarying = True
            lUEBVar.SpaceVarying = True
            lUEBVar.GridFileName = StrSplit(aFileContents, vbCrLf, "")
            lUEBVar.GridVariableName = StrSplit(aFileContents, vbCrLf, "")
        Else 'constant space/time
            lUEBVar.TimeVarying = False
            lUEBVar.SpaceVarying = False
            lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, ""))
        End If
        Return lUEBVar

    End Function

    Public Function OutputVariableString() As String
        Return Description & vbCrLf & GridFileName
    End Function

    Public Shared Function FromOutputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable

        lUEBVar.Description = StrSplit(aFileContents, vbCrLf, "")
        lUEBVar.GridFileName = StrSplit(aFileContents, vbCrLf, "")
        Return lUEBVar
    End Function

    Public Shared Function FromAggOutputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable

        lUEBVar.Description = StrSplit(aFileContents, vbCrLf, "")
        Return lUEBVar
    End Function
End Class
