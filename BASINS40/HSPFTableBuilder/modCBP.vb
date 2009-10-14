Imports System.Collections.Specialized
Imports System.Text
Imports MapWinUtility
Imports atcUtility

Module modCBP
    Private g_CbpBaseFolder As String = "D:\cbp_working\pp\param"
    Sub ParmSummary()
        Dim lBaseFolderOrig As String = My.Computer.FileSystem.CurrentDirectory
        My.Computer.FileSystem.CurrentDirectory = g_CbpBaseFolder

        Dim lFormat As String = "####0.0000"
        Dim lWidth As String = 10

        Dim lSB As New StringBuilder
        lSB.AppendLine("LU" & vbTab & "Table".PadLeft(16) & vbTab & "Parm".PadLeft(12) & vbTab & _
                       "Mean".PadLeft(lWidth) & vbTab & _
                       "Min".PadLeft(lWidth) & vbTab & _
                       "Max".PadLeft(lWidth) & vbTab & _
                       "Count".PadLeft(lWidth))

        Dim lParmFiles As New NameValueCollection
        AddFilesInDir(lParmFiles, CurDir, True, "*.csv")
        For Each lParmFile As String In lParmFiles
            Dim lCountSkipped As Integer = 0
            Dim lName() As String = lParmFile.Split("\")
            Dim lLu As String = lName(4) 'TODO: get landuse a better way
            Dim lFileName As String = lName(6)
            Dim lParmTable As New atcTableDelimited
            With lParmTable
                .OpenFile(lParmFile)
                Dim lTableName(.NumFields) As String
                If .CurrentRecordAsDelimitedString.StartsWith("VARS") Then
                    For lFieldIndex As Integer = 2 To .NumFields
                        lTableName(lFieldIndex) = .FieldName(lFieldIndex)
                    Next
                Else
                    'add dummy field at start of record
                    lTableName = ("x," & .CurrentRecordAsDelimitedString).Split(",")
                    .MoveNext()
                    lCountSkipped += 1
                End If
                'add dummy field at start of record
                Dim lParmName() As String = ("x," & .CurrentRecordAsDelimitedString).Split(",")
                lCountSkipped += 1
                .MoveNext()
                Logger.Dbg(lParmFile & ":" & .NumRecords)
                Dim lMin(.NumFields) As Double
                Dim lMax(.NumFields) As Double
                Dim lSum(.NumFields) As Double
                Dim lFirst As Boolean = True
                While .CurrentRecord <= .NumRecords
                    If .Value(1).StartsWith("end") Then
                        lCountSkipped += 1
                        Exit While
                    Else
                        For lFieldIndex As Integer = 2 To .NumFields
                            Dim lValue As Double = Double.NaN
                            If IsNumeric(.Value(lFieldIndex)) Then
                                lValue = .Value(lFieldIndex)
                            End If
                            lSum(lFieldIndex) += lValue
                            If lValue > lMax(lFieldIndex) OrElse lFirst Then lMax(lFieldIndex) = lValue
                            If lValue < lMax(lFieldIndex) OrElse lFirst Then lMin(lFieldIndex) = lValue
                        Next
                    End If
                    .MoveNext()
                    lFirst = False
                End While
                For lFieldIndex As Integer = 2 To .NumFields
                    Dim lMean As Double = lSum(lFieldIndex) / (.NumRecords - lCountSkipped)
                    If Math.Abs(lMax(lFieldIndex) - lMin(lFieldIndex)) < 0.01 Then
                        lMean = lMax(lFieldIndex)
                    End If
                    If lTableName(lFieldIndex).Trim.Length > 0 Then
                        lSB.AppendLine(lLu & vbTab & _
                                       lTableName(lFieldIndex).PadLeft(16) & vbTab & lParmName(lFieldIndex).PadLeft(12) & vbTab & _
                                       DoubleToString(lMean, , lFormat).PadLeft(lWidth) & vbTab & _
                                       DoubleToString(lMin(lFieldIndex), , lFormat).PadLeft(lWidth) & vbTab & _
                                       DoubleToString(lMax(lFieldIndex), , lFormat).PadLeft(lWidth) & vbTab & _
                                       (.NumRecords - lCountSkipped).ToString.PadLeft(lWidth))
                    End If
                Next
            End With
        Next
        My.Computer.FileSystem.CurrentDirectory = lBaseFolderOrig
        SaveFileString("parms\AllParmDump.txt", lSB.ToString)
    End Sub
End Module
