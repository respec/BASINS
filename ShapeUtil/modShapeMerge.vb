Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic

Imports atcUtility
Imports MapWinUtility

Module modShapeMerge
	
	'keyField: the field name or number in the DBFs that is unique.
	'   Duplicate values in this field will cause DBF records and associated
	'   shapes that occur later in shpFileNames to be discarded.
	'   If keyField is 0, duplicate checking will use all fields
	'   If keyField is -1, no duplicate checking will occur
	'newBaseFilename: the path and filename without extension of the
	'   destination merged dbf, shp, and shx. If the file already exists, its
	'   current contents will be merged with the other file(s) in shpFileNames
	'shpFileNames: array of full path names of .shp files to merge
    Public Sub ShapeMerge(ByRef keyField As String, ByRef newBaseFilename As String, ByRef shpFileNames As ArrayList)
        Dim duplicate As Boolean
        Dim newBaseCopied As Boolean
        Dim successful As Boolean
        Dim curInput As Integer
        Dim dbfIn() As atcTableDBF
        Dim shpIn() As CShape_IO
        Dim dbfOut As atcTableDBF = Nothing
        Dim shpOut As CShape_IO = Nothing
        Dim baseFileName As String
        Dim fieldNum As Short
        Dim minFields As Short
        Dim keyFieldNum As Short
        Dim outRecsBeforeThisInput As Integer 'Skip searching for matches in records just added from the same file
        Dim outRecsBeforeAllInput As Integer 'Skip writing if we never added any records

        Dim FileLength As Integer
        Dim ShapeType As Integer
        Dim uppX, lowX, lowY, uppY As Double

        Dim NewFileLength As Integer
        Dim NewShapeType As Integer
        Dim NewHeader As T_MainFileHeader
        Dim shpInHeader As T_MainFileHeader
        Dim starttime As Date

        Dim j, i, mergeRecordCount As Integer
        Dim canCopyRecords As Boolean

        newBaseFilename = FilenameNoExt(newBaseFilename)

        ReDim dbfIn(shpFileNames.Count)
        ReDim shpIn(shpFileNames.Count)

        For curInput = 0 To shpFileNames.Count - 1
            'TODO check to see if sets of files are identical before any compares by record
            baseFileName = FilenameNoExt(shpFileNames(curInput))
            If Len(baseFileName) = 0 Then
                'skip blank entries in shpFileNames
            ElseIf LCase(baseFileName) = LCase(newBaseFilename) Then
                'skip the destination if it is named in shpFileNames
            ElseIf Not IO.File.Exists(baseFileName & ".dbf") Then
                LogMsg("Could not find " & baseFileName & ".dbf, so could not merge")
            ElseIf Not IO.File.Exists(baseFileName & ".shp") Then
                LogMsg("Could not find " & baseFileName & ".shp, so could not merge")

                'If we are merging an empty file into an existing file, might as well skip then empty file
            ElseIf FileLen(baseFileName & ".shp") <= 100 And IO.File.Exists(newBaseFilename & ".shp") Then
                LogDbg("Skipping empty shape file '" & baseFileName & ".shp'")

            ElseIf Not IO.File.Exists(newBaseFilename & ".shp") Then
                'If destination shape file does not exist, copy first shape file to merge
CopyOverNew:
                LogDbg("Copying " & baseFileName & " to " & newBaseFilename)
                FileCopy(baseFileName & ".dbf", newBaseFilename & ".dbf")
                FileCopy(baseFileName & ".shp", newBaseFilename & ".shp")
                FileCopy(baseFileName & ".shx", newBaseFilename & ".shx")
                newBaseCopied = True
            Else
                If dbfOut Is Nothing Then
                    dbfOut = New atcTableDBF
                    dbfOut.OpenFile(newBaseFilename & ".dbf")
                    If dbfOut.NumRecords = 0 Then 'Destination layer is empty, delete it and copy over it
                        LogDbg("Destination " & newBaseFilename & " empty, deleting and copying over it")
                        dbfOut = Nothing
                        If IO.File.Exists(newBaseFilename & ".shp") Then Kill(newBaseFilename & ".shp")
                        If IO.File.Exists(newBaseFilename & ".shx") Then Kill(newBaseFilename & ".shx")
                        If IO.File.Exists(newBaseFilename & ".dbf") Then Kill(newBaseFilename & ".dbf")
                        GoTo CopyOverNew
                    ElseIf IsNumeric(keyField) Then
                        keyFieldNum = CShort(keyField)
                    Else
                        keyFieldNum = dbfOut.FieldNumber(keyField)
                    End If
                    dbfOut.CurrentRecord = dbfOut.NumRecords
                    outRecsBeforeAllInput = dbfOut.NumRecords
                    minFields = dbfOut.NumFields

                    shpOut = New CShape_IO
                    shpOut.ShapeFileOpen(newBaseFilename & ".shp", CShape_IO.READWRITEFLAG.Readwrite)
                    NewHeader = shpOut.getShapeHeader
                    NewShapeType = NewHeader.ShapeType
                End If

                dbfIn(curInput) = New atcTableDBF
                With dbfIn(curInput)
                    '.Logger = gLogger
                    .OpenFile(baseFileName & ".dbf")
                    'TODO check to make sure fields are exactly the same as dbfOut
                    If .NumFields <> dbfOut.NumFields Then
                        LogDbg("Different number of fields:" & vbCr & newBaseFilename & ".dbf = " & dbfOut.NumFields & vbCr & baseFileName & ".dbf = " & .NumFields & vbCr & vbCr & "Skipping last " & System.Math.Abs(.NumFields - dbfOut.NumFields) & " field(s)")
                        If .NumFields < minFields Then minFields = .NumFields
                        '          LogMsg "Different number of fields:" & vbCr _
                        ''                 & newBaseFilename & ".dbf = " & dbfOut.numFields & vbCr _
                        ''                 & baseFileName & ".dbf = " & .numFields & vbCr & vbCr _
                        ''                 & "Cannot merge shape files"
                        '          Exit Sub
                    End If
                End With

                shpIn(curInput) = New CShape_IO
                With shpIn(curInput)
                    .ShapeFileOpen(baseFileName & ".shp", CShape_IO.READWRITEFLAG.ReadOnlyMode)
                    If .getRecordCount <> dbfIn(curInput).NumRecords Then
                        LogMsg("Unequal number of records:" & vbCr & baseFileName & ".shp = " & .getRecordCount & vbCr & baseFileName & ".dbf = " & dbfIn(curInput).NumRecords & vbCr & vbCr & "Cannot merge shape files")
                        Exit Sub
                    End If
                    shpInHeader = .getShapeHeader
                    If shpInHeader.ShapeType <> NewShapeType Then
                        LogMsg("Different type of shapes in files: " & vbCr & newBaseFilename & ".shp = " & NewShapeType & vbCr & vbCr & baseFileName & ".shp = " & shpInHeader.ShapeType & vbCr & vbCr & "Cannot merge shape files")
                        Exit Sub
                    End If
                End With
            End If
        Next

        If Not dbfOut Is Nothing Then
            For curInput = 0 To shpFileNames.Count - 1
                If Not dbfIn(curInput) Is Nothing And Not shpIn(curInput) Is Nothing Then
                    starttime = Now
                    With dbfIn(curInput)
                        If .NumRecords < 1 Then
                            LogDbg("No records available in " & FilenameNoExt((dbfIn(curInput).FileName)))
                        ElseIf .NumRecords <> shpIn(curInput).getRecordCount Then
                            baseFileName = FilenameNoExt(shpFileNames(curInput))
                            LogMsg("Cannot merge - unequal number of records in " & vbCr & baseFileName & ".shp (" & shpIn(curInput).getRecordCount & ") and " & vbCr & baseFileName & ".dbf (" & .NumRecords & ")")
                        Else
                            mergeRecordCount = 0
                            LogDbg("Merging " & FilenameNoExt((dbfIn(curInput).FileName)) & " into " & newBaseFilename)
                            outRecsBeforeThisInput = dbfOut.NumRecords
                            If .NumFields = dbfOut.NumFields Then
                                canCopyRecords = True
                            Else
                                canCopyRecords = False
                            End If
                            For i = 1 To .NumRecords
                                .CurrentRecord = i
                                Select Case keyFieldNum
                                    Case -1 : duplicate = False
                                    Case 0 : duplicate = dbfOut.FindRecord(.RawRecord, 1, outRecsBeforeThisInput)
                                    Case Else : duplicate = dbfOut.FindFirst(keyFieldNum, .Value(keyFieldNum), 1, outRecsBeforeThisInput)
                                End Select
                                If duplicate Then
                                    'LogDbg "Merge:Skipping matching record in " & shpFileNames(curInput) & " - " & I
                                    'For j = 1 To .NumFields
                                    '  Debug.Print .FieldName(j), .Value(j), dbfOut.Value(j)
                                    'Next
                                Else
                                    dbfOut.CurrentRecord = dbfOut.NumRecords + 1
                                    If canCopyRecords Then
                                        dbfOut.RawRecord = .RawRecord 'Copy this record in the DBF
                                    Else
                                        For fieldNum = 1 To minFields
                                            dbfOut.Value(fieldNum) = .Value(fieldNum)
                                        Next
                                    End If
                                    'Append current shape from shpIn(curInput) to shpOut
                                    'Debug.Print ".";
                                    CopyShape(NewShapeType, .CurrentRecord, shpIn(curInput), shpOut)
                                    mergeRecordCount = mergeRecordCount + 1
                                End If
                                System.Windows.Forms.Application.DoEvents()
                            Next

                            LogDbg("Added " & mergeRecordCount & " records of " & .NumRecords & " to " & outRecsBeforeThisInput & " yielding " & dbfOut.NumRecords & " in " & Format((Now - starttime).TotalSeconds, "#.### sec"))
                        End If
                    End With
                End If
            Next
            If dbfOut.NumRecords > outRecsBeforeAllInput Then
                dbfOut.WriteFile(newBaseFilename & ".dbf")
            End If
        End If
        successful = True

        'GoSub CleanUp
        'Exit Sub

        'ErrHand: 
        '		GoSub CleanUp
        '		If Err.Source = "Merge" Then
        '			Err.Raise(Err.Number, "ShapeUtil Merge", Err.Description)
        '		Else
        '			Err.Raise(Err.Number, "ShapeUtil Merge", Err.Description & " (" & Err.Source & ")")
        '		End If
        '		Exit Sub

        'CleanUp: 
        On Error Resume Next 'If there is trouble killing files, just leave them
        If Not shpOut Is Nothing Then shpOut.FileShutDown()
        For curInput = 0 To shpFileNames.Count - 1
            If Not shpIn(curInput) Is Nothing Then shpIn(curInput).FileShutDown()
            shpIn(curInput) = Nothing
            dbfIn(curInput) = Nothing
            baseFileName = FilenameNoExt(shpFileNames(curInput))
            If IO.File.Exists(baseFileName & ".shp") Then Kill(baseFileName & ".shp")
            If IO.File.Exists(baseFileName & ".shx") Then Kill(baseFileName & ".shx")
            If IO.File.Exists(baseFileName & ".dbf") Then Kill(baseFileName & ".dbf")
        Next
        If Not successful And newBaseCopied Then
            Kill(newBaseFilename & ".dbf")
            Kill(newBaseFilename & ".shp")
            Kill(newBaseFilename & ".shx")
        End If

    End Sub
	
	Private Sub CopyShape(ByRef ShapeType As Integer, ByRef FromIndex As Integer, ByRef FromShp As CShape_IO, ByRef ToShp As CShape_IO)
		Select Case ShapeType 'ShapeType: 0=null, 1=Point, 3=PolyLine, 5=Polygon, 8=MultiPoint
            Case CShape_IO.FILETYPEENUM.typePoint
                ToShp.putXYPoint(0, FromShp.getXYPoint(FromIndex))
                'TODO: typePointZ, typePointM
			Case CShape_IO.FILETYPEENUM.typePolyline, CShape_IO.FILETYPEENUM.typePolygon, CShape_IO.FILETYPEENUM.typeMultipoint, CShape_IO.FILETYPEENUM.typePolyLineZ, CShape_IO.FILETYPEENUM.typePolygonZ, CShape_IO.FILETYPEENUM.typeMultiPointZ, CShape_IO.FILETYPEENUM.typePolyLineM, CShape_IO.FILETYPEENUM.typePolygonM, CShape_IO.FILETYPEENUM.typeMultiPointM, CShape_IO.FILETYPEENUM.typeMultiPatch
				ToShp.putPoly(0, FromShp.getPoly(FromIndex))
			Case Else
				LogDbg("CopyShape:Unsupported shape type " & ShapeType & " not copied")
		End Select
	End Sub
	
	Private Sub LogDbg(ByRef msg As String)
        Logger.Dbg(msg)
			End Sub
	
	Private Sub LogMsg(ByRef msg As String)
        'If gLogger Is Nothing Then
        '	MsgBox(msg, MsgBoxStyle.Critical, "Shape Merge")
        'Else
        Logger.Msg(msg, "Shape Merge")
        'End If
	End Sub
End Module