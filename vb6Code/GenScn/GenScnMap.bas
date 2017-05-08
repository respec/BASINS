Attribute VB_Name = "GenScnMap"
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Type MapLine
  ShapeType As Integer '1=Point, 3=Line, 5=Polygon
  Name As String
  VertCnt As Long
  VertAr As Variant
End Type

Type MapShapeLine
  Filename As String
  Name As String
  Branch As String
  DownName As String
  DSInvert As Single
  linecnt As Long
  Length As Single
'  Point As Boolean
'  Polygon As Boolean
  RchMapLine() As MapLine
End Type
Global MyRch() As MapShapeLine
Dim cind&

Public Function GetDatabase$(f As String)
  If Len(f) >= Len(Dir(f)) + 1 Then
    GetDatabase = Left(f, Len(f) - (Len(Dir(f)) + 1))
  Else
    GetDatabase = CurDir
  End If
End Function

'Reads line and/or point layer(s) from comma-delimited filenames into into Rch
Public Sub ReadShapeLine(ByVal filenames$, Rch() As MapShapeLine, map As ATCoMap)
  Dim myRec As Recordset
  Dim i&, j&, l&, k&, FileLen&, version&, ShpTyp&, flen&, d() As Double
  Dim recnum&, RecLen&, RecShpTyp&, LinCnt&
  Dim NumParts&, OldNumParts&, NumPoints&, StartPoints&(), OrigStartPoints&()
  Dim lowX#, lowY#, uppX#, uppY#, tolrX#, tolrY#, tolr#
  Dim NPtsX&, CurStartInd&
  Dim dx#, dy#
  Dim r#, s$, b$, p$, M As Boolean, fu%, rlen#
  Dim ShpDB As Database
  Dim CommaPos&, Filename$
  Dim fldnum&, fldname$
  Dim fldSearchStartTime As Single
  'Static fldSearchTotalTime As Single
  Dim layer&, BranchName$, IDLocnName$, DownIDName$, LengthName$
  Dim outletIndex&, lind&
   
  ReDim Rch(0)
  LinCnt = -1
  
  While Len(filenames) > 0
    CommaPos = InStr(1, filenames, ",")
    If CommaPos = 0 Then
      Filename = filenames
      filenames = ""
    Else
      Filename = Left(filenames, CommaPos - 1)
      filenames = Mid(filenames, CommaPos + 1)
    End If
    
    For i = 0 To map.LayerCount - 1
      If LCase(FilenameOnly(map.LayerFilename(i))) = LCase(FilenameOnly(Filename)) Then
        layer = i
        Exit For
      End If
    Next i
    BranchName = map.LayerBranchField(layer)
    IDLocnName = map.LayerKeyField(layer)
    DownIDName = map.layerDownIDField(layer)
    LengthName = map.LayerLengthField(layer)
    
    s = GetDatabase(Filename)
    Set ShpDB = OpenDatabase(s, False, False, "DBASE IV")
    s = FilenameOnly(Filename) & ".dbf"
    Set myRec = ShpDB.OpenRecordset(s, dbOpenDynaset)
    myRec.MoveFirst
    
    fu = FreeFile(0)
    Open Filename For Binary Access Read As #fu
    If InStr(1, Filename, ":") = 0 And Left(Filename, 1) <> "\" Then
      Filename = CurDir & "\" & Filename
    End If
    ' check version
    i = ReadBigInt(fu)
    If i <> 9994 Then
      MsgBox ("'" & Filename & "' is not a shape file I know about")
      Exit Sub
    End If
    
    Seek #fu, 25           ' skip to good stuff 'w7
    flen = ReadBigInt(fu)  'Get file length
    Get #fu, , version     'Get the version    'b29:32(w8)
    Get #fu, , ShpTyp      'Get the shape type 'b33:36(w9)
    
    'Get bounding box
    Get #fu, , lowX
    Get #fu, , lowY
    Get #fu, , uppX
    Get #fu, , uppY
    tolr = 1000
    tolrY = Abs(uppY - lowY) / tolr
    tolrX = Abs(uppX - lowX) / tolr
  
    ' skip to entity stuff
    Seek #fu, 101 'w26
  
    Do
      recnum = ReadBigInt(fu)
      RecLen = ReadBigInt(fu)
  
      'Get the record shape type
      Get #fu, , RecShpTyp
      If RecShpTyp > 0 Then ' ok
        s = "": b = "Dummy": p = "": rlen = 0
        fldSearchStartTime = Timer
        For fldnum = 0 To myRec.Fields.Count - 1
          If Not IsNull(myRec(fldnum).value) Then
            fldname = myRec.Fields(fldnum).Name
            'Debug.Print fldnum, fldname
            If fldname = IDLocnName Then
              s = myRec(fldnum).value
              If RecShpTyp <> 3 Then fldnum = myRec.Fields.Count
            End If
            If RecShpTyp = 3 Then
              If fldname = BranchName Then b = myRec(fldnum).value
              If fldname = DownIDName Then p = myRec(fldnum).value
              If fldname = LengthName Then rlen = myRec(fldnum).value
            End If
          End If
        Next fldnum
        'fldSearchTotalTime = fldSearchTotalTime + (Timer - fldSearchStartTime)
        'Debug.Print "fldSearchTotalTime=" & fldSearchTotalTime
        M = False 'assume we dont need this line or poly
        If Len(s) > 0 Then ' have IDLOCN for this record
          l = 0
          Do While l <= LinCnt ' loop thru what we have
            If Rch(l).Name = s Then ' we have part of this one
              M = True
              Exit Do
            Else ' try next
              l = l + 1
            End If
          Loop
          If Not M Then 'didnt have this one, add it
            LinCnt = LinCnt + 1
            ReDim Preserve Rch(LinCnt)
            Rch(LinCnt).Name = s
            Rch(LinCnt).Filename = Filename
            l = LinCnt
            M = True
          End If
          If Len(b) > 0 Then Rch(l).Branch = b 'save (or update) branch
          If Len(p) > 0 Then
            Rch(l).DownName = p 'save (or update) downstream
            If IsNumeric(p) Then
              If p = 0 Then outletIndex = LinCnt
            End If
          Else 'null, no downid, assume outlet
            outletIndex = LinCnt
          End If
          Rch(l).Length = Rch(l).Length + rlen 'increment
        End If
        
        If RecShpTyp = 5 And M Then 'polygon, get rid of point/line of same locn
'          Rch(l).Polygon = True
'          Rch(l).Point = False
          If Rch(l).linecnt > 0 Then
            ReDim Rch(l).RchMapLine(0)
            Rch(l).linecnt = 0
          End If
        ElseIf RecShpTyp <> 5 And Rch(l).linecnt > 0 Then 'if we already have a poly for this locn
          If Rch(l).RchMapLine(0).ShapeType = 5 Then      'ignore subsequent point/line
            M = False 'skip line
          End If
        End If

'        If RecShpTyp = 1 Then
'          Rch(l).Point = True
'          Rch(l).Polygon = False
'        ElseIf RecShpTyp = 3 Then
'          Rch(l).Point = False
'          Rch(l).Polygon = False
'        End If
        
        'Get the next record.
        If RecShpTyp = 1 Then 'point
          NumParts = 1
          NumPoints = 1
        Else                  'line or polygon, structure is the same
          Get #fu, , r ' lowX
          Get #fu, , r ' lowY
          Get #fu, , r ' uppX
          Get #fu, , r ' uppY
          
          Get #fu, , NumParts
          Get #fu, , NumPoints
        End If
        If M Then
          OldNumParts = Rch(l).linecnt
          Rch(l).linecnt = NumParts + Rch(l).linecnt
          If Rch(l).linecnt > 0 Then ReDim Preserve Rch(l).RchMapLine(Rch(l).linecnt - 1)
          For j = OldNumParts To Rch(l).linecnt - 1
            Rch(l).RchMapLine(j).ShapeType = RecShpTyp
            Rch(l).RchMapLine(j).Name = s
          Next j
        End If

      
        'Get the starting point of each line within the polyline
        ReDim StartPoints(NumParts) As Long
        ReDim OrigStartPoints(NumParts) As Long
        's = "NumParts: " & CStr(NumParts) & vbCrLf
        's = s & "NumPoint: " & CStr(NumPoints) & vbCrLf
        
        If RecShpTyp = 1 Then
          OrigStartPoints(0) = 0
        Else
          For j = 0 To NumParts - 1
            Get #fu, , OrigStartPoints(j)
          Next j
        End If
        OrigStartPoints(NumParts) = NumPoints
        On Error GoTo AllocError
        ReDim arcpoints(NumPoints, 2) As Double
        On Error GoTo 0
        'Get all the points at this time. This makes it easier to
        'handle multiple polylines in an arc.
        NPtsX = 0
        CurStartInd = 0
        StartPoints(0) = 0
        's = s & " Part: " & CStr(CurStartInd) & " OPos: " & CStr(OrigStartPoints(CurStartInd)) & " Pos: " & CStr(StartPoints(CurStartInd)) & vbCrLf

        For i = 0 To NumPoints - 1
          Get #fu, , r
          arcpoints(NPtsX, 0) = r
          Get #fu, , r
          arcpoints(NPtsX, 1) = r
          If i > OrigStartPoints(CurStartInd) And i < OrigStartPoints(CurStartInd + 1) - 1 Then
            dx = Abs(arcpoints(NPtsX - 1, 0) - arcpoints(NPtsX, 0))
            dy = Abs(arcpoints(NPtsX - 1, 1) - arcpoints(NPtsX, 1))
            If dx > tolrX Or dy > tolrY Then
              NPtsX = NPtsX + 1
            End If
          Else ' always save first and last pair
            NPtsX = NPtsX + 1
            If i = OrigStartPoints(CurStartInd + 1) - 1 Then
              CurStartInd = CurStartInd + 1
              StartPoints(CurStartInd) = NPtsX
              's = s & " Part: " & CStr(CurStartInd) & " OPos: " & CStr(OrigStartPoints(CurStartInd)) & " Pos: " & CStr(StartPoints(CurStartInd)) & vbCrLf
            End If
          End If
    
        Next i
    
        If NumParts > 1 Then
          'MsgBox s
        End If
    
        If M Then
          For i = 0 To NumParts - 1
            'Calculate the number of vertices in this Part.
            If NumParts = 1 Then
              j = NPtsX
            Else
              If i = NumParts - 1 Then
                j = NPtsX - StartPoints(i)
              Else
                j = StartPoints(i + 1) - StartPoints(i)
              End If
            End If
            Rch(l).RchMapLine(OldNumParts + i).VertCnt = j
            ReDim d(j, 2) As Double
            For k = 0 To j
              d(k, 0) = arcpoints(StartPoints(i) + k, 0)
              d(k, 1) = arcpoints(StartPoints(i) + k, 1)
            Next k
            Rch(l).RchMapLine(OldNumParts + i).VertAr = d
          Next i
        End If
      End If
      If Not (EOF(fu)) Then
        myRec.MoveNext
      End If
    Loop While Not EOF(fu)
    
    If Rch(outletIndex).Branch = "Dummy" Then
      cind = 1
      lind = 1
      i = 0
      Rch(outletIndex).Branch = "D" & cind
      Call findBranch(Rch, outletIndex, lind, i)
    End If
    
Closefile:
    Close #fu
    ShpDB.Close
  Wend
  Exit Sub
AllocError:
  MsgBox "ReadShapeLine: Error allocating memory for shape file " & Filename
  GoTo Closefile
End Sub

Private Sub findBranch(Rch() As MapShapeLine, outletIndex&, lind&, MxCnt&)
  Dim i&, j&, s$, lcnt, ucnt&(), uind&(), lMxCnt&, X&
  
  s = Rch(outletIndex).Name
  MxCnt = 0
  lcnt = 0
  lMxCnt = 0
  ReDim ucnt(0)
  ReDim uind(0)
  
  'Debug.Print "in  ", Rch(outletIndex).Name, Rch(outletIndex).DownName, lind, cind, MxCnt
  For i = 0 To UBound(Rch)
    If i <> outletIndex Then
      If Rch(i).DownName = s Then
        uind(lcnt) = i
        Rch(i).Branch = "D" & lind 'assume in longest chain
        Call findBranch(Rch, i, lind, ucnt(lcnt)) 'any further up tree
        If ucnt(lcnt) > lMxCnt Then 'more up tree than any prev, renumb all prev
          lMxCnt = ucnt(lcnt)
          For j = 0 To lcnt - 1
            cind = cind + 1
            'Debug.Print " rp ", Rch(uind(j)).Name, Rch(uind(j)).DownName, lind, cind, ucnt(j), lMxCnt
            Rch(uind(j)).Branch = "D" & cind
            Call findBranch(Rch, uind(j), cind, X)
          Next j
        ElseIf lcnt > 0 Then 'renumb this one
          cind = cind + 1
          'Debug.Print " rx ", Rch(i).Name, Rch(i).DownName, lind, cind, ucnt(lcnt), lMxCnt
          Rch(i).Branch = "D" & cind
          Call findBranch(Rch, i, cind, X)
        End If
        lcnt = lcnt + 1
        ReDim Preserve ucnt(lcnt)
        ReDim Preserve uind(lcnt)
      End If
    End If
  Next i
  MxCnt = MxCnt + lMxCnt + 1
  'Debug.Print "  ex", s, Rch(outletIndex).DownName, lind, cind, MxCnt, lMxCnt
End Sub
