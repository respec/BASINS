Attribute VB_Name = "modPcsDMR2BasinsDBF"
Option Explicit
'Const pBaseDir = "c:\BasinsDownload Tutorial\"
'Const pBaseDirCore = pBaseDir & "BasinsCore\PCS3\"
'Const pBaseDirPCS = pBaseDir & "PCS Discharge\"

'Sub Main()
'
'  Set pLogger = New clsATCoLogger
'  With pLogger
'    .SetFileName pBaseDir & "PcsDMR2BasinsDBFLog.txt"
'    .Log2Debug = True
'    .Log "Main:OpenLogger"
'  End With
'
'  Dim lDbf As New clsDBF
'  lDbf.OpenDBF pBaseDirCore & "pcs\05010007.dbf"
'
'  Dim lCsv As New clsCSV
'  lCsv.OpenCSV pBaseDirPCS & "PA0004057.PCS_DMR_MEASUREMENT.CSV"
'
'  ImportDMR2DBF lCsv, lDbf, pLogger
'End Sub

Public Sub ImportDMR2DBF(DMRcsv As clsCSV, aDBF As clsDBF, Optional Logger As Object = Nothing)
  Dim newDBF As New clsDBF
  Dim KeyFields(1 To 3) As String

  Set newDBF = aDBF.Cousin
  DMR2BasinsDBF DMRcsv, newDBF, Logger

  Set aDBF.Logger = Logger
  KeyFields(1) = aDBF.FieldName(1) 'parm id
  KeyFields(2) = aDBF.FieldName(2) 'npdes
  KeyFields(3) = aDBF.FieldName(3) 'year
  aDBF.Merge newDBF, KeyFields, 1 'merge new DBF (from DMR csv) with existing DBF
  aDBF.WriteDBF aDBF.Filename 'write out new DBF

End Sub

'DMRcsv is the DMR file being ported to Basins 3 DBF file (newDbf)
'newDBF should have fields defined the same as Basins3 PCS DBF file
Private Sub DMR2BasinsDBF(DMRcsv As clsCSV, newDBF As clsDBF, Optional Logger As Object = Nothing)
  Dim I As Long, j As Long, index As Long
  Dim lKey As String
  Dim fname As String, NPDESId As String
  Dim DataValue As String, ParmID As String
  Dim Parms As New FastCollection
  Dim Dates As FastCollection
  Dim Flows As New FastCollection
  Dim iParm As Long, iDate As Long, iFlow As Long
  Dim thisYear As Long, curYear As Long
  Dim sumThisYear As Single, flowThisYear As Single
  Dim FlowByMonth As FastCollection, ConcByMonth As FastCollection
  Dim FlowWgtedConc As Single
  Dim nThisYear As Long
  Dim WeAreLoggin As Boolean
  Dim DisID As String

  newDBF.NumRecords = 0
  newDBF.InitData

  fname = FilenameNoPath(DMRcsv.Filename)
  I = InStr(fname, ".")
  NPDESId = Left(fname, I - 1)

  If Not Logger Is Nothing Then
    WeAreLoggin = True
    Logger.Log vbCrLf & "Start Process: Build DBF From DMR File: " & DMRcsv.Filename
  Else
    WeAreLoggin = False
  End If

  For I = 1 To DMRcsv.NumRecords
    DMRcsv.CurrentRecord = I
    DataValue = ""
    ParmID = DMRcsv.Value(12)
    If ParmID = "50050" Then 'flow value (others parm codes)
      DataValue = DMRcsv.Value(18)
      If DataValue = "" Then
        DataValue = DMRcsv.Value(3)
      End If
    ElseIf ValidPCSParm(ParmID) Then 'other parm value
      DataValue = DMRcsv.Value(3)
    End If
    If DataValue <> "" Then
      If IsNumeric(DataValue) Then
        iParm = Parms.IndexFromKey(ParmID)
        If Len(DMRcsv.Value(6)) > 0 Then
          DisID = "ID" & DMRcsv.Value(6)
        Else
          DisID = ""
        End If
        If ParmID = "50050" Then
          If Not Flows.KeyExists(DMRcsv.Value(10) & DisID) Then
            Flows.Add DataValue, DMRcsv.Value(10) & DisID
          Else
            Logger.Log DMRcsv.Value(10) & " - Multiple FLOW values for Discharge Num " & DisID & _
                        " Store first (" & Flows.ItemByKey(DMRcsv.Value(10) & DisID) & ") - ignore duplicate (" & DataValue & ")"
          End If
        Else 'all other parameters
          If iParm > 0 Then
            Set Dates = Parms.ItemByIndex(iParm)
          Else
            Set Dates = New FastCollection
            If Parms.Count = 0 Then 'add first
              Parms.Add Dates, ParmID
            ElseIf Parms.Key(Parms.Count) < ParmID Then 'add to end
              Parms.Add Dates, ParmID
            Else 'add to correct spot in list
              index = 1
              While index < Parms.Count
                If Parms.Key(index) > ParmID Then
                  Parms.Add Dates, ParmID, index
                  index = Parms.Count 'force exit
                Else 'try next
                  index = index + 1
                End If
              Wend
            End If
          End If
          'store data value with date as key
          lKey = DMRcsv.Value(10) & DisID
          If Not Dates.KeyExists(lKey) Then
            If Dates.Count = 0 Then 'add first
              Dates.Add DataValue, lKey
            ElseIf Dates.Key(Dates.Count) < lKey Then 'add to end
              Dates.Add DataValue, lKey
            Else
              index = 1
              While index < Dates.Count
                If Dates.Key(index) > lKey Then
                  Dates.Add DataValue, lKey, index
                  index = Dates.Count
                Else
                  index = index + 1
                End If
              Wend
            End If
          ElseIf WeAreLoggin Then
            Logger.Log DMRcsv.Value(10) & " - Multiple values for PARMID " & ParmID & " and Discharge Num " & DisID & _
                        " Store first (" & Dates.ItemByKey(DMRcsv.Value(10) & DisID) & ") - ignore duplicate (" & DataValue & ")"
          End If
        End If
      ElseIf WeAreLoggin Then
        Logger.Log DMRcsv.Value(10) & " - Skip non-numeric value (" & DataValue & ") for ParmID " & ParmID
      End If
    End If
  Next

  For iParm = 1 To Parms.Count
    ParmID = Parms.Key(iParm)
    Set Dates = Parms.ItemByIndex(iParm)
    While Dates.Count > 0
      I = InStr(Dates.Key(1), "ID")
      If I > 0 Then
        thisYear = CLng(Format(Left(Dates.Key(1), I - 1), "yyyy"))
      Else
        thisYear = CLng(Format(Dates.Key(1), "yyyy"))
      End If
      nThisYear = 0
      sumThisYear = 0
      flowThisYear = 0
      Set FlowByMonth = New FastCollection
      Set ConcByMonth = New FastCollection
      iDate = 1
      While iDate <= Dates.Count
        I = InStr(Dates.Key(iDate), "ID")
        If I > 0 Then
          curYear = CLng(Format(Left(Dates.Key(iDate), I - 1), "yyyy"))
        Else
          curYear = CLng(Format(Dates.Key(iDate), "yyyy"))
        End If
        If thisYear = curYear Then
          iFlow = Flows.IndexFromKey(Dates.Key(iDate))
          If iFlow > 0 Then 'flow value exists for this date
            nThisYear = nThisYear + 1
            FlowByMonth.Add Flows.ItemByIndex(iFlow)
            ConcByMonth.Add Dates.ItemByIndex(iDate)
            sumThisYear = sumThisYear + Dates.ItemByIndex(iDate)
            flowThisYear = flowThisYear + Flows.ItemByIndex(iFlow)
          End If
          Dates.Remove (iDate)
        Else
          iDate = iDate + 1
        End If
      Wend
      If nThisYear > 0 Then
        'calculate flow-weighted concentration
        FlowWgtedConc = 0
        For I = 1 To nThisYear
          FlowWgtedConc = FlowWgtedConc + (FlowByMonth.ItemByIndex(I) * ConcByMonth.ItemByIndex(I) / flowThisYear)
        Next I
        'Add a record to the DBF with
        newDBF.CurrentRecord = newDBF.NumRecords + 1
        newDBF.Value(1) = ParmID
        newDBF.Value(2) = NPDESId
        newDBF.Value(3) = CStr(thisYear)
        newDBF.Value(4) = CStr(sumThisYear / nThisYear) 'avg. annual conc.
        newDBF.Value(5) = CStr(flowThisYear / nThisYear) 'avg. annual flow
        'NOTE: the load calculation below assumes concentration units of mg/l
        '      with resulting units of lbs/year
        newDBF.Value(6) = CStr(flowThisYear / nThisYear * FlowWgtedConc * 8.333333333 * 365.25)
        newDBF.Value(7) = FlowWgtedConc
        'newdbf.Value(8)= error
        newDBF.Value(9) = CStr(nThisYear)
        ' aParmCode, thisYear, sumThisYear/nThisYear
      End If
    Wend
  Next

  If WeAreLoggin Then Logger.Log "End Process: Build DBF From DMR"

  fname = FilenameNoExt(DMRcsv.Filename) & ".dbf"
  newDBF.WriteDBF fname
End Sub

'Determines if a parameter is one to be stored on the BASINS PCS database
'*** NOTE:  this should be done through a database, but is hard-coded for now
'*** NOTE:  assuming anything under 299 is not a concentration;
'***        anything above is a concentration in mg/l
'***        THIS IS THE BEST WE CAN DO FOR NOW!!!
Private Function ValidPCSParm(ParmID As String) As Boolean
  Dim isValid As Boolean
  Dim nID As Long

  isValid = False
  If IsNumeric(ParmID) Then
    nID = CLng(ParmID)
    'don't use flow(50050) or pH(400-409)
    If nID >= 299 And nID <> 50050 And (nID < 400 Or nID > 409) Then
      isValid = True
    End If
  End If
  ValidPCSParm = isValid
End Function

Public Function NewPCSdbf(Optional aNumRecords As Long = 0) As clsDBF
  Set NewPCSdbf = New clsDBF
  With NewPCSdbf
    .Year = CInt(Format(Now, "yyyy")) - 1900
    .Month = CByte(Format(Now, "mm"))
    .Day = CByte(Format(Now, "dd"))
    .NumFields = 10

    .FieldName(1) = "PARM"
    .FieldType(1) = "C"
    .FieldLength(1) = 10
    .FieldDecimalCount(1) = 0

    .FieldName(2) = "NPDES"
    .FieldType(2) = "C"
    .FieldLength(2) = 10
    .FieldDecimalCount(2) = 0

    .FieldName(3) = "YEAR"
    .FieldType(3) = "N"
    .FieldLength(3) = 6
    .FieldDecimalCount(3) = 0

    .FieldName(4) = "CONC"
    .FieldType(4) = "N"
    .FieldLength(4) = 20
    .FieldDecimalCount(4) = 5

    .FieldName(5) = "FLOW"
    .FieldType(5) = "N"
    .FieldLength(5) = 20
    .FieldDecimalCount(5) = 5

    .FieldName(6) = "LOAD"
    .FieldType(6) = "N"
    .FieldLength(6) = 20
    .FieldDecimalCount(6) = 5

    .FieldName(7) = "CONC_FW"
    .FieldType(7) = "N"
    .FieldLength(7) = 20
    .FieldDecimalCount(7) = 5

    .FieldName(8) = "ERROR"
    .FieldType(8) = "N"
    .FieldLength(8) = 20
    .FieldDecimalCount(8) = 5
  
    .FieldName(9) = "MONTH"
    .FieldType(9) = "N"
    .FieldLength(9) = 6
    .FieldDecimalCount(9) = 0
  
    .FieldName(10) = "N"
    .FieldType(10) = "N"
    .FieldLength(10) = 20
    .FieldDecimalCount(10) = 5

    .NumRecords = aNumRecords
    .InitData
  End With
End Function

