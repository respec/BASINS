Imports System.Collections.Specialized
Imports System.Text
Imports MapWinUtility
Imports atcUtility

Module modCBP
    Private g_CbpBaseFolder As String = "D:\cbp_working\pp\param"
    Private g_ParmFileName As String = "parms\allParmDump.txt"
    Sub ParmSummary(Optional ByVal aDebug As Boolean = False)
        Dim lSubBasinCountyFile As New atcTableDelimited
        Dim lComIdUnique As New atcCollection
        With lSubBasinCountyFile
            .OpenFile("hyd\CountiesSubbasinsIntersection.csv")
            Logger.Dbg(.NumRecords)
            Dim lNumRecords As Integer = .NumRecords
            While .CurrentRecord <= lNumRecords
                Dim lComId As String = .Value(8).PadLeft(6, "0")
                Dim lComIdIndex As Integer = lComIdUnique.IndexFromKey(lComId)
                If lComIdIndex = -1 Then 'new
                    lComIdUnique.Add(lComId, New atcCollection)
                    lComIdIndex = lComIdUnique.Count
                End If
                Dim lComIdCountyCollection As atcCollection = lComIdUnique.ItemByKey(lComId)
                Dim lCountyFipsIndex As Integer = lComIdCountyCollection.IndexFromKey(.Value(3))
                If lCountyFipsIndex = -1 Then
                    lComIdCountyCollection.Add(.Value(3))
                End If
                .MoveNext()
            End While
            Logger.Dbg(" ComId: " & lComIdUnique.Count)
            lComIdUnique.Sort()
            For lComIdIndex As Integer = 0 To lComIdUnique.Count - 1
                Dim lComIdCountyCollection As atcCollection = lComIdUnique.ItemByIndex(lComIdIndex)
                Logger.Dbg(" ComId " & lComIdUnique.Keys(lComIdIndex).ToString.TrimStart("0") & " CountyCount " & lComIdCountyCollection.Count)
            Next
        End With

        Dim lBaseFolderOrig As String = My.Computer.FileSystem.CurrentDirectory
        My.Computer.FileSystem.CurrentDirectory = g_CbpBaseFolder

        Dim lFormat As String = "####0.0000"
        Dim lWidth As String = 10

        Dim lParmFiles As New NameValueCollection
        AddFilesInDir(lParmFiles, CurDir, True, "*.csv")

        Dim lSB As New StringBuilder
        Dim lLuOld As String = ""
        Dim lStartTime As Date = Now
        Dim lOutputFile As String = ""
        For Each lParmFile As String In lParmFiles
            Dim lCountSkipped As Integer = 0
            Dim lName() As String = lParmFile.Split("\")
            Dim lLu As String = lName(4) 'TODO: get landuse a better way
            If lLu <> lLuOld Then
                lOutputFile = lBaseFolderOrig & "\" & g_ParmFileName.Replace("all", lLu)
                Dim lOutputFileInfo As IO.FileSystemInfo = My.Computer.FileSystem.GetFileInfo(lOutputFile)
                If lOutputFileInfo.CreationTime < lStartTime Then
                    IO.File.Delete(lOutputFile)
                End If
                lSB = Nothing
                lSB = New StringBuilder
                lSB.AppendLine("LU" & vbTab & "Table".PadLeft(16) & vbTab & "Parm".PadLeft(12) & vbTab & _
                               "ComId" & vbTab & _
                               "Mean".PadLeft(lWidth) & vbTab & _
                               "Min".PadLeft(lWidth) & vbTab & _
                               "Max".PadLeft(lWidth) & vbTab & _
                               "Count".PadLeft(lWidth))
                lLuOld = lLu
            End If
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
                Dim lStartRecord As Integer = .CurrentRecord
                Logger.Dbg(lParmFile & ":" & .NumRecords)
                Dim lMin(.NumFields) As Double
                Dim lMax(.NumFields) As Double
                Dim lSum(.NumFields) As Double
                For Each lComId As String In lComIdUnique.Keys
                    Dim lComIdCountyCollection As atcCollection = lComIdUnique.ItemByKey(lComId)
                    If aDebug Then Logger.Dbg("ComId " & lComId.TrimStart("0"))
                    Array.Clear(lMin, 0, .NumFields)
                    Array.Clear(lMax, 0, .NumFields)
                    Array.Clear(lSum, 0, .NumFields)
                    Dim lFoundCount As Integer = 0
                    For Each lCountyFips As String In lComIdCountyCollection
                        Dim lFirst As Boolean = True
                        Dim lPrefixs() As String = {"A", "B", "C"}
                        For Each lPrefix As String In lPrefixs
                            .CurrentRecord = lStartRecord
                            If .FindFirst(1, lPrefix & lCountyFips) Then
                                Do
                                    If aDebug Then Logger.Dbg("Found " & lPrefix & lCountyFips & " at " & .CurrentRecord)
                                    lFoundCount += 1
                                    For lFieldIndex As Integer = 2 To .NumFields
                                        Dim lValue As Double = Double.NaN
                                        If IsNumeric(.Value(lFieldIndex)) Then
                                            lValue = .Value(lFieldIndex)
                                        End If
                                        lSum(lFieldIndex) += lValue
                                        If lValue > lMax(lFieldIndex) OrElse lFirst Then lMax(lFieldIndex) = lValue
                                        If lValue < lMax(lFieldIndex) OrElse lFirst Then lMin(lFieldIndex) = lValue
                                    Next
                                    lFirst = False
                                    .MoveNext()
                                Loop Until Not .FindFirst(1, lPrefix & lCountyFips, .CurrentRecord)
                            End If
                        Next
                    Next
                    If lFoundCount <> lComIdCountyCollection.Count Then
                        If aDebug Then Logger.Dbg("CountIssueForComId " & lComId & " " & lComIdCountyCollection.Count & " " & lFoundCount)
                    End If
                    For lFieldIndex As Integer = 2 To .NumFields
                        Dim lMean As Double = lSum(lFieldIndex) / lFoundCount
                        If Math.Abs(lMax(lFieldIndex) - lMin(lFieldIndex)) < 0.01 Then
                            lMean = lMax(lFieldIndex)
                        End If
                        If lTableName(lFieldIndex).Trim.Length > 0 Then
                            lSB.AppendLine(lLu & vbTab & _
                                           lTableName(lFieldIndex).PadLeft(16) & vbTab & lParmName(lFieldIndex).PadLeft(12) & vbTab & _
                                           lComId.TrimStart("0").PadLeft(6) & vbTab & _
                                           DoubleToString(lMean, , lFormat).PadLeft(lWidth) & vbTab & _
                                           DoubleToString(lMin(lFieldIndex), , lFormat).PadLeft(lWidth) & vbTab & _
                                           DoubleToString(lMax(lFieldIndex), , lFormat).PadLeft(lWidth) & vbTab & _
                                           (lFoundCount.ToString.PadLeft(lWidth)))
                        End If
                    Next
                Next
                AppendFileString(lOutputFile, lSB.ToString)
                lSB = Nothing
                lSB = New StringBuilder
            End With
        Next
        My.Computer.FileSystem.CurrentDirectory = lBaseFolderOrig
    End Sub

End Module
