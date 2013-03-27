Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBVariable
    Public Code As String
    Public LongName As String
    Public Description As String
    Public Units As String
    Public SpaceVarying As Boolean
    Public TimeVarying As Boolean
    Public Value As Double
    Public GridFileName As String = ""
    Public GridVariableName As String = ""
    Public GridXVarName As String = ""
    Public GridYVarName As String = ""
    Public GridTimeVarName As String = ""
    Public GridDataRangeMin As Double
    Public GridDataRangeMax As Double
    Public TimeFileName As String

    Public Overrides Function ToString() As String
        Return Description
    End Function

    Public Function ParameterString() As String
        Dim lVariableDescription As String = Code & ": " & LongName
        If Units.Trim.Length > 0 Then lVariableDescription = lVariableDescription & " (" & Units.Trim & ")"
        Return lVariableDescription & vbCrLf & DoubleToString(Value, , "###0.#######") & vbCrLf
    End Function

    Public Shared Function FromParameterString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        ParseVariableDescription(lUEBVar, aFileContents)
        lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, ""))
        Return lUEBVar
    End Function

    Public Function SiteVariableString(ByVal aPathName As String) As String
        Dim lVariableDescription As String = Code & ": " & LongName
        If Units.Trim.Length > 0 Then lVariableDescription = lVariableDescription & " (" & Units.Trim & ")"
        If SpaceVarying Then
            Return lVariableDescription & vbCrLf & "1" & vbCrLf & RelativeFilename(GridFileName, aPathName) & _
                   ";X:" & GridXVarName & ";Y:" & GridYVarName & ";D:" & GridVariableName
        Else
            Return lVariableDescription & vbCrLf & "0" & vbCrLf & Value & vbCrLf
        End If

    End Function

    Public Shared Function FromSiteVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        Dim lSpaceVaryingFlag As Integer

        ParseVariableDescription(lUEBVar, aFileContents)
        lSpaceVaryingFlag = Integer.Parse(StrSplit(aFileContents, vbCrLf, "'"))
        If lSpaceVaryingFlag = 0 Then 'constant value
            lUEBVar.SpaceVarying = False
            lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, "'"))
        Else
            lUEBVar.SpaceVarying = True
            Dim lRec As String = StrSplit(aFileContents, vbCrLf, "")
            Dim lKey As String
            lUEBVar.GridFileName = StrSplit(lRec, ";", "'")
            While lRec.Length > 0
                lKey = StrSplit(lRec, ":", "")
                Select Case lKey.ToUpper
                    Case "X" : lUEBVar.GridXVarName = StrSplit(lRec, ";", "")
                    Case "Y" : lUEBVar.GridYVarName = StrSplit(lRec, ";", "")
                    Case "D" : lUEBVar.GridVariableName = StrSplit(lRec, ";", "")
                End Select
            End While
        End If
        Return lUEBVar
    End Function

    Public Function InputVariableString(ByVal aPathName As String) As String
        Dim lVariableDescription As String = Code & ": " & LongName
        If Units.Trim.Length > 0 Then lVariableDescription = lVariableDescription & " (" & Units.Trim & ")"
        If SpaceVarying And TimeVarying Then
            Return lVariableDescription & vbCrLf & "1" & vbCrLf & RelativeFilename(GridFileName, aPathName) & _
                   ";X:" & GridXVarName & ";Y:" & GridYVarName & ";time:" & GridTimeVarName & ";D:" & GridVariableName
            If GridDataRangeMax > GridDataRangeMin Then
                lVariableDescription &= ";range:" & GridDataRangeMin & "," & GridDataRangeMax & vbCrLf
            Else
                lVariableDescription &= vbCrLf
            End If
        ElseIf TimeVarying Then
            Return lVariableDescription & vbCrLf & "0" & vbCrLf & RelativeFilename(TimeFileName, aPathName) & vbCrLf
        Else
            Return lVariableDescription & vbCrLf & "2" & vbCrLf & Value & vbCrLf
        End If

    End Function

    Public Shared Function FromInputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable
        Dim lVaryingFlag As Integer

        ParseVariableDescription(lUEBVar, aFileContents)
        lVaryingFlag = Integer.Parse(StrSplit(aFileContents, vbCrLf, "'"))
        If lVaryingFlag = 0 Then 'constant spatial timeseries
            lUEBVar.TimeVarying = True
            lUEBVar.SpaceVarying = False
            lUEBVar.TimeFileName = StrSplit(aFileContents, vbCrLf, "'")
        ElseIf lVaryingFlag = 1 Then 'varying in space/time
            lUEBVar.TimeVarying = True
            lUEBVar.SpaceVarying = True
            Dim lRec As String = StrSplit(aFileContents, vbCrLf, "")
            Dim lKey As String
            lUEBVar.GridFileName = StrSplit(lRec, ";", "'")
            While lRec.Length > 0
                lKey = StrSplit(lRec, ":", "")
                Select Case lKey.ToUpper
                    Case "X" : lUEBVar.GridXVarName = StrSplit(lRec, ";", "")
                    Case "Y" : lUEBVar.GridYVarName = StrSplit(lRec, ";", "")
                    Case "TIME" : lUEBVar.GridTimeVarName = StrSplit(lRec, ";", "")
                    Case "D" : lUEBVar.GridVariableName = StrSplit(lRec, ";", "")
                    Case "RANGE"
                        lUEBVar.GridDataRangeMin = Double.Parse(StrSplit(lRec, "&", ""))
                        lUEBVar.GridDataRangeMax = Double.Parse(StrSplit(lRec, ";", ""))
                End Select
            End While
        Else 'constant space/time
            lUEBVar.TimeVarying = False
            lUEBVar.SpaceVarying = False
            lUEBVar.Value = Double.Parse(StrSplit(aFileContents, vbCrLf, "'"))
        End If
        Return lUEBVar

    End Function

    Public Function OutputVariableString() As String
        Dim lVariableDescription As String = Code & ": " & LongName
        If Units.Trim.Length > 0 Then lVariableDescription = lVariableDescription & " (" & Units.Trim & ")"
        Return lVariableDescription & vbCrLf & GridFileName & vbCrLf
    End Function

    Public Shared Function FromOutputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable

        ParseVariableDescription(lUEBVar, aFileContents)
        lUEBVar.GridFileName = StrSplit(aFileContents, vbCrLf, "")
        Return lUEBVar
    End Function

    Public Function AggOutputVariableString() As String
        Dim lVariableDescription As String = Code & ": " & LongName
        If Units.Trim.Length > 0 Then lVariableDescription = lVariableDescription & " (" & Units.Trim & ")"
        Return lVariableDescription
    End Function

    Public Shared Function FromAggOutputVariableString(ByRef aFileContents As String) As clsUEBVariable
        Dim lUEBVar As New clsUEBVariable

        ParseVariableDescription(lUEBVar, aFileContents)
        Return lUEBVar
    End Function

    Public Shared Sub ParseVariableDescription(ByRef aUEBVar As clsUEBVariable, ByRef aFileContents As String)
        Dim lRec As String = StrSplit(aFileContents, vbCrLf, "")
        aUEBVar.Code = StrSplit(lRec, ":", "")
        aUEBVar.LongName = StrSplit(lRec, ":", "")
        aUEBVar.Description = lRec
        If aUEBVar.Description.EndsWith("""") Then 'parse units string at end of description
            Dim lPos As Integer = aUEBVar.Description.LastIndexOf(":")
            If lPos > 0 Then
                aUEBVar.Units = StrFindBlock(aUEBVar.Description, """", """", lPos)
                aUEBVar.Description = aUEBVar.Description.Substring(0, lPos)
            End If
        End If
    End Sub
End Class
