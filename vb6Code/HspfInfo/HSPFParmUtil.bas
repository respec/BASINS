Attribute VB_Name = "HSPFParmUtil"
Attribute VB_Ext_KEY = "RVB_UniqueId" ,"3607B89E03A3"
Option Explicit

Public fmsg As Long
Public fwdm As Long

Type OperDetails
    Name As String
    Count As Integer
    Ind As Integer
    SegID() As Long
    SegNum() As Long
End Type
Dim OpDet(3) As OperDetails

Type ParmDetail
    Name As String
    StartCol As Long
    Width As Long
    Def As String
    id As Long
End Type

Type Filter
    txt As String
    num As Long
    id() As Long
End Type

'Global myDB As Database
'Global myParmDefn As Recordset
'Global myParmTableDefn As Recordset
'Global myTableAliasDefn As Recordset
'Global myWat As Recordset
'Global myScen As Recordset
'Global mySeg As Recordset
'Global myTab As Recordset
'Global myParmData As Recordset
'Global myTabDef As TableDef
'Global myRec As Recordset
'Global mySQL As String
'Global myQuery As QueryDef
'Global myIndx As Index
'Global myRel As Relation

Global FiltInd&, Filt(3) As Filter

Public ExName As String
Public ExCmd As String
Public ExPath As String

Sub GetScenInfo(scnID&)
    Dim init&, id&, kwd$, kflg&, contfg&, retid&, Ind&, i&
    
    'look for perlnd, implnd, rchres operation types
    init = 1
    id = 0
    Do
      Call F90_GTNXKW(init, id, kwd, kflg, contfg, retid)
      If kflg > 0 And (retid > 120 And retid <= 123) Then ' just P,I,R
        'this operation type exists
        Ind = retid - 120
        OpDet(Ind).Name = kwd
        OpDet(Ind).Count = 0
        OpDet(Ind).Ind = Ind
        Call GetOperInfo(OpDet(Ind), scnID)
      End If
      init = 0
    Loop While contfg = 1
    
    For i = 1 To 3
        Call GetTableDetails(i)
    Next i
End Sub

Sub GetOperInfo(OpLoc As OperDetails, scnID&)  'from frmgenscnact (cousin)
    'find information about hspf operation type
    Dim cbuff$, omcode&, init&, retcod&, retkey&, i%, crit$
    Dim mySeg As Recordset

    Set mySeg = myDb.OpenRecordset("SegData", dbOpenDynaset)

    omcode = 3
    init = 1
    Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
    init = 0
    Do
      Call F90_XBLOCK(omcode, init, retkey, cbuff, retcod)
      If Mid(cbuff, 7, 6) = OpLoc.Name Then
        OpLoc.Count = OpLoc.Count + 1
        ReDim Preserve OpLoc.SegID(OpLoc.Count)
        ReDim Preserve OpLoc.SegNum(OpLoc.Count)

        i = Mid(cbuff, 18, 3)
        OpLoc.SegNum(OpLoc.Count) = i
        With mySeg
          .AddNew
          !Name = Mid(cbuff, 7, 14)
          !Description = "????"
          !ScenarioID = scnID
          !OpnTypID = OpLoc.Ind
          OpLoc.SegID(OpLoc.Count) = !id
          .Update
        End With
      End If
    Loop While retcod = 1 Or retcod = 2

    mySeg.Close

End Sub

Sub GetTableDetails(i&)
    Dim init&, kwd$, kflg&, contfg&, retid&, Ind&
    Dim tabno&, uunits&, initb&, retkey&, retcod&, cbuff$, Occur&
    Dim opF&, opL&, j&, k&, tmp$, s&
    Dim crit$
    Dim pd() As ParmDetail, pc&

    Set myParmData = myDb.OpenRecordset("ParmData", dbOpenDynaset)
    Set myParmDefn = myDb.OpenRecordset("ParmDefn", dbOpenDynaset)
    Set myParmTableDefn = myDb.OpenRecordset("ParmTableDefn", dbOpenDynaset)

    init = 1
    Ind = i + 120
    Do
      Call F90_GTNXKW(init, Ind, kwd, kflg, contfg, retid)
      If kflg <> 0 Then
        'frmMain.lblInfo.Caption = kwd
        DoEvents
        crit = "Name = '" & kwd & "' AND OpnTypID = " & i
        myParmTableDefn.FindFirst crit
        
        With myParmDefn
          crit = "ParmTableID = " & myParmTableDefn!id
          .FindFirst crit
          pc = 0
          Do
            pc = pc + 1
            ReDim Preserve pd(pc)
            pd(pc).Name = !Name
            pd(pc).StartCol = !StartCol
            pd(pc).Width = !Width
            If IsNull(!Def) Then
              pd(pc).Def = ""
            Else
              pd(pc).Def = !Def
            End If
            pd(pc).id = !id
            .FindNext crit
          Loop While Not (.NoMatch)
        End With
        initb = 1
        tabno = retid - (i * 1000)
        uunits = 1
        Occur = 1
        Do
          Call F90_XTABLE(Ind, tabno, uunits, initb, CLng(0), Occur, retkey, cbuff, retcod)
          initb = 0
          If retcod = 2 Then 'process me
            'Debug.Print cbuff
            opF = Left(cbuff, 5)
            tmp = Trim(Mid(cbuff, 6, 5))
            If Len(tmp) = 0 Then
              opL = opF
            Else
              opL = CLng(tmp)
            End If
            For j = 1 To OpDet(i).Count
               s = OpDet(i).SegNum(j)
               If (opF <= s) And (opL >= s) Then
                 For k = 1 To pc
                   With myParmData
                     .AddNew
                     !ParmID = pd(k).id
                     !SegID = OpDet(i).SegID(j)
                     tmp = Trim(Mid(cbuff, pd(k).StartCol, pd(k).Width))
                     If Len(tmp) = 0 Then
                       tmp = pd(k).Def
                       If Len(tmp) = 0 Then
                         tmp = " "
                       End If
                     End If
                     !Occur = Occur
                     !Value = tmp
                     .Update
                   End With
                   If pd(k).Name = "LSID" Or pd(k).Name = "RCHID" Then
                     crit = "ID = " & OpDet(i).SegID(j)
                     'MsgBox pd(k).Name & " is " & tmp & " find " & crit
                     Set mySeg = myDb.OpenRecordset("SegData", dbOpenDynaset)
                     With mySeg
                       .FindFirst crit
                       .Edit
                       !Description = tmp
                       .Update
                       .Close
                     End With
                   End If
                 Next k
               End If
            Next j
          End If
          If retcod = 10 Then 'look for more occurances of this table
            retcod = 1
            Occur = Occur + 1
            initb = 1
          End If
        Loop While retcod = 1 Or retcod = 2
      End If
      init = 0
    Loop While contfg = 1

    myParmData.Close
    myParmDefn.Close
    myParmTableDefn.Close

End Sub

Sub UpdateNumberSegs()
    Dim myOpnCnt As Recordset
    Dim crit$, ityp&, cnt&
  
    'update number of segs
    Set myOpnCnt = myDb.OpenRecordset("CountOpnTypes", dbOpenDynaset)
    Set mySeg = myDb.OpenRecordset("ScenarioData", dbOpenDynaset)
    
    While Not (mySeg.EOF)
      crit = "ID = " & mySeg(0).Value
      myOpnCnt.FindFirst crit
      mySeg.Edit
      While Not (myOpnCnt.NoMatch)
        ityp = myOpnCnt("OpnTypID")
        cnt = myOpnCnt("CountOfOpnTypID")
        If ityp = 3 Then 'rchres(3)
          mySeg("NumReaches") = cnt
        Else 'perlnd(1) or implnd(2)
          If IsNull(mySeg("NumSegments")) Then
            mySeg("NumSegments") = cnt
          Else
            mySeg("NumSegments") = cnt + mySeg("NumSegments")
          End If
        End If
        myOpnCnt.FindNext crit
      Wend
      mySeg.Update
      mySeg.MoveNext
    Wend
    
End Sub
