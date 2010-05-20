Imports System.IO
Imports atcData
Imports atcUtility

Module modArcSWAT

    Private Function CreateFlowLineFile(ByVal numRecords As Integer) As atcTableDBF

        Dim newDBF As New atcTableDBF()

        newDBF.NumFields = 6

        newDBF.FieldName(1) = "ARCID"
        newDBF.FieldType(1) = "N"
        newDBF.FieldLength(1) = 9
        newDBF.FieldDecimalCount(1) = 0

        newDBF.FieldName(2) = "GRID_CODE"
        newDBF.FieldType(2) = "N"
        newDBF.FieldLength(2) = 9
        newDBF.FieldDecimalCount(2) = 0

        newDBF.FieldName(3) = "FROM_NODE"
        newDBF.FieldType(3) = "N"
        newDBF.FieldLength(3) = 9
        newDBF.FieldDecimalCount(3) = 0

        newDBF.FieldName(4) = "TO_NODE"
        newDBF.FieldType(4) = "N"
        newDBF.FieldLength(4) = 9
        newDBF.FieldDecimalCount(4) = 0

        newDBF.FieldName(5) = "SUBBASIN"
        newDBF.FieldType(5) = "N"
        newDBF.FieldLength(5) = 9
        newDBF.FieldDecimalCount(5) = 0

        newDBF.FieldName(6) = "SUBBASINR"
        newDBF.FieldType(6) = "N"
        newDBF.FieldLength(6) = 9
        newDBF.FieldDecimalCount(6) = 0

        newDBF.NumRecords = numRecords
        newDBF.InitData()

        Return newDBF
    End Function

    Public Function CreateCatchmentFile(ByVal numRecords As Integer) As atcTableDBF


        Dim newDBF As New atcTableDBF()

        newDBF.NumFields = 2	        

        newDBF.FieldName(1) = "GRIDCODE"
        newDBF.FieldType(1) = "N"
        newDBF.FieldLength(1) = 9
        newDBF.FieldDecimalCount(1) = 0

        newDBF.FieldName(2) = "SUBBASIN"
        newDBF.FieldType(2) = "N"
        newDBF.FieldLength(2) = 9
        newDBF.FieldDecimalCount(2) = 0

        newDBF.NumRecords = numRecords
        newDBF.InitData()

        Return newDBF

    End Function


    Public Sub FillCatchmentFile(ByVal outFile As String, ByVal numRecs As Integer)

        Dim destFolder As String        
        destFolder = Path.GetDirectoryName(outFile)
        If (Directory.Exists(destFolder) = False) Then
            Directory.CreateDirectory(destFolder)
        End If

        If (File.Exists(outFile)) Then
            File.Delete(outFile)
        End If

        Dim newDbf As atcTableDBF
        newDbf = CreateCatchmentFile(numRecs)

        Dim val As Integer = 0
        newDbf.MoveFirst()

        For i As Integer = 0 To numRecs - 1
            If (i > 0) Then
                newDbf.MoveNext()
            End If
            val = i + 1
            newDbf.Value(1) = val.ToString()
            newDbf.Value(2) = val.ToString()
        Next

        newDbf.WriteFile(outFile)

    End Sub

    Public Function ConvertFlowLineFile(ByVal inFile As String, ByVal outFile As String) As Integer


        If (outFile = "") Then
            Throw New Exception("Must specify output file name.")
        End If

        If (File.Exists(inFile) = False) Then
            Throw New Exception("File does not exist: " + inFile)
        End If

        Dim atcDbf As New atcTableDBF()
        atcDbf.OpenFile(inFile)


        Dim numRecs As Integer = atcDbf.NumRecords


        Dim list As New List(Of Segment)

        Dim segment As Segment

        Dim val As String = ""
        Dim val2 As String = ""

        Dim dct As New Dictionary(Of String, Integer)

        atcDbf.MoveFirst()

        For i As Integer = 0 To numRecs - 1

            If (i > 0) Then
                atcDbf.MoveNext()
            End If

            val = atcDbf.Value(1)
            val2 = atcDbf.Value(14)

            dct.Add(val, i + 1)

            segment = New Segment()
            segment.Id = val
            segment.ToId = val2
            list.Add(segment)
        Next

        atcDbf = Nothing

        Dim newDbf As atcTableDBF = CreateFlowLineFile(numRecs)
        newDbf.MoveFirst()
        Dim id As Integer = 0
        Dim toId As Integer = 0
        For i As Integer = 0 To numRecs - 1

            If (i > 0) Then
                newDbf.MoveNext()
            End If

            id = dct(list(i).Id)

            If (dct.ContainsKey(list(i).ToId)) Then
                toId = dct(list(i).ToId)
            Else
                toId = 0
            End If

            newDbf.Value(1) = list(i).Id
            newDbf.Value(2) = id.ToString()
            newDbf.Value(3) = id.ToString()
            newDbf.Value(4) = toId.ToString()
            newDbf.Value(5) = id.ToString()
            newDbf.Value(6) = toId.ToString()

        Next

        newDbf.WriteFile(outFile)

        Return numRecs

    End Function

    Public Function GetFileName(ByVal file As String) As String

        Dim name As String = Path.GetFileName(file)

        Return name
    End Function

    'Copy shapefile and other associated files
    Public Sub CopyShapeFiles(ByVal srcFile As String, ByVal destFolder As String)

        If (File.Exists(srcFile) = False) Then
            Return
        End If

        If (Directory.Exists(destFolder) = False) Then
            Directory.CreateDirectory(destFolder)
        End If

        Dim srcPath As String = Path.GetDirectoryName(srcFile)
        'Dim baseName As String = Path.GetFileName(srcFile)
        Dim baseName As String = Path.GetFileNameWithoutExtension(srcFile)

        'File.Copy(srcFile, destFolder + "\" + baseName)
        Dim tmpFile As String

        'Copy files with these extensions - .dbf, .mwsr, .prj, .shp, .shp.xml, .shx
        Dim ext() As String = New String() {".dbf", ".mwsr", ".prj", ".shp", ".shp.xml", ".shx"}

        For i As Integer = 0 To ext.Length - 1
            tmpFile = baseName + ext(i)
            If (File.Exists(srcPath + "\" + tmpFile)) Then
                File.Copy(srcPath + "\" + tmpFile, destFolder + "\" + tmpFile)
            End If

        Next

    End Sub



    Private Class Segment

        Dim _Id As String = ""
        Dim _toId As String = ""




        Public Property Id() As String

            Get
                Return _Id
            End Get

            Set(ByVal value As String)
                _Id = value
            End Set

        End Property


        Public Property ToId() As String

            Get
                Return _toId
            End Get

            Set(ByVal value As String)
                _toId = value
            End Set

        End Property

    End Class

End Module
