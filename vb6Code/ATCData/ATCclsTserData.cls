VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCclsTserData"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'##MODULE_SUMMARY Class containing a timeseries and associated methods
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free _
 use permitted under open source license

Private pFile As ATCclsTserFile
Private pValues() As Single
Private pFlags() As Long
Private pHeader As ATTimSerDataHeader
Private pDates As ATCclsTserDate
Private pDtran As Long

Private pMin As Double
Private pMax As Double
Private pMean As Double
Private pGeometricMean As Double
Private pSum As Double
Private pStdDeviation As Double
Private pVariance As Double

Private pAttributes As Collection
Private pErrorDescription As String
Private pSerial As Long
Private pNeedSummaryRecalc As Boolean

Private pHeaderComplete As Boolean
Private pHeaderOnly As Boolean

'##SUMMARY Enumerated type for use in math operations.
'##MEMBER ATCAdd Add
'##MEMBER ATCSubtract Subtract
'##MEMBER ATCMultiply Multiply
'##MEMBER ATCDivide Divide
'##MEMBER ATCCummDiff Cummulative difference
'##MEMBER ATCAve Average
'##MEMBER ATCWeight Weight
'##MEMBER NONE no operator
'##MEMBER ATCMin Minimum
'##MEMBER ATCMax Maximum
'##MEMBER ATCAbs Absolute value
'##MEMBER ATCTSltC Constant C if value less than C, else value
'##MEMBER ATCTSgtC Constant C if value greater than C, else value
'##MEMBER ATCLn Natural logarithm
'##MEMBER ATCLog Base 10 logarithm
'##MEMBER ATCExp e to the power of value
'##MEMBER ATCTSPowTs value to the power in ts2
'##MEMBER ATCCPowTS constant to the power of value
'##MEMBER ATCTSPowC value to a constant power
Public Enum ATCOperatorType
  ATCAdd = 0
  ATCSubtract
  ATCMultiply
  ATCDivide
  ATCCummDiff
  ATCAve
  ATCWeight
  ATCTSPowC
  ATCCPowTS
  ATCTSPowTS
  ATCExp
  ATCLog
  ATCLn
  ATCAbs
  ATCTSltC
  ATCTSgtC
'  ATCTSleC
'  ATCTSgeC
  ATCMin
  ATCMax
  NONE = -999
End Enum

Public Sub Dump(Optional l& = 1)
'##SUMMARY Prints a summary of the timeseries to the immediate window _
 for debugging.&nbsp; Only works in the VB development environment.
  Debug.Print "File:" & pFile.filename
  Debug.Print "ISLC:" & pHeader.id & ":" & pHeader.Sen & ":" & pHeader.Loc & ":" & pHeader.Con
  If DataNotRead Then
    Debug.Print "Data Not Read"
  Else
    Debug.Print "Nval:" & pDates.Summary.NVALS
    Debug.Print "CInt:" & pDates.Summary.CIntvl
    DumpDate pDates.Summary.SJDay, "SDat"
    DumpDate pDates.Summary.EJDay, "Edat"
  End If
End Sub

Public Function AddRemoveDates(sjdate As Double, _
                               ejdate As Double, _
                               Optional NewValueOption As Long = 0, _
                               Optional newValue As Single = 0) As ATCclsTserData
'##SUMMARY Returns a new ATCclsTserData based on this one with values added and/or _
 removed to fit the specified start and end date
'##PARAMETER_DESCRIPTION sjdate Starting Julian Date
'##PARAMETER_DESCRIPTION ejdate Ending Julian Date
'##PARAMETER_DESCRIPTION NewValueOption _
 0:All new values are set to NewValue<br> _
 1:new values are set to closest value that exists already (NewValue is ignored)<br> _
 2:new values are interpolated from closest value that exists to NewValue<br> _
 3:new values are copied from existing values starting at _index_ NewValue
'##PARAMETER_DESCRIPTION newValue See explanation of NewValueOption
  
  Dim index As Long, indexOffset As Long, oldIndex As Long, lastOldIndex As Long
  Dim tsOut As ATCclsTserData
  Dim dsOut As ATCclsTserDate
  Dim dsS As ATTimSerDateSummary
  Dim curDate As Double
  Dim newval() As Single, NumNewVal As Long, NumPrependVal As Long, NumAppendVal As Long
  Dim valNew1 As Single, valNew2 As Single
  Dim cloneIndex As Long
  
  dsS = Me.Dates.Summary
  
  If sjdate >= dsS.SJDay And ejdate <= dsS.EJDay Then 'No added dates, just use SubSetByDate
    Set AddRemoveDates = Me.SubSetByDate(sjdate, ejdate)
    Exit Function
  End If
  
  If sjdate < dsS.SJDay Then
    If Me.Dates.Summary.CIntvl Then
      If dsS.SJDay > ejdate Then 'If current data is all after requested period, don't prepend too many
        NumPrependVal = timdifJ(sjdate, ejdate, dsS.Tu, dsS.ts)
      Else
        NumPrependVal = timdifJ(sjdate, dsS.SJDay, dsS.Tu, dsS.ts)
      End If
    Else
      MsgBox "Adding dates only works on constant interval time series", vbExclamation, "Cannot create time series"
      Exit Function
    End If
  ElseIf sjdate > dsS.SJDay Then
    NumPrependVal = 1 - Me.Dates.IndexAtOrBefore(sjdate)
  Else
    NumPrependVal = 0
  End If
  
  If ejdate > dsS.EJDay Then
    If Me.Dates.Summary.CIntvl Then
      If dsS.EJDay < sjdate Then 'If current data is all before requested period, don't append too many
        NumAppendVal = timdifJ(sjdate, ejdate, dsS.Tu, dsS.ts)
      Else
        NumAppendVal = timdifJ(dsS.EJDay, ejdate, dsS.Tu, dsS.ts)
      End If
    Else
      MsgBox "Adding dates only works on constant interval time series", vbExclamation, "Cannot create time series"
      Exit Function
    End If
  ElseIf ejdate < dsS.EJDay Then
    index = Me.Dates.IndexAtOrAfter(ejdate)
    NumAppendVal = index - dsS.NVALS
  Else
    NumAppendVal = 0
  End If
        
  NumNewVal = dsS.NVALS + NumPrependVal + NumAppendVal
  
  ReDim newval(NumNewVal)
  
  Select Case NewValueOption
    Case 0: valNew2 = newValue
    Case 1: valNew2 = Me.Value(1)
    Case 2: valNew1 = newValue
            valNew2 = Me.Value(1)
    Case 3: cloneIndex = newValue
  End Select
  
  'If we are adding values at the beginning, NumPrependVal >=1
  'so we go through this loop for each prepended value
  For index = 1 To NumPrependVal
    Select Case NewValueOption
      Case 0, 1: newval(index) = valNew2
      Case 2
        newval(index) = valNew1 + (valNew2 - valNew1) * (index - 1) / NumPrependVal
      Case 3
        oldIndex = cloneIndex + index - 1
        While oldIndex > Me.Dates.Summary.NVALS
          oldIndex = oldIndex - Me.Dates.Summary.NVALS + cloneIndex - 1
        Wend
        newval(index) = Me.Value(oldIndex)
    End Select
  Next
  
  'Next we copy desired existing values from old time series
  If NumPrependVal <= 0 Then
    oldIndex = 1 - NumPrependVal
    'indexOffset = 0
  Else
    oldIndex = 1
    'indexOffset = NumPrependVal
  End If
  
  If NumAppendVal <= 0 Then
    lastOldIndex = dsS.NVALS + NumAppendVal
  Else
    lastOldIndex = dsS.NVALS
  End If
  indexOffset = index - oldIndex
  While oldIndex <= lastOldIndex
    newval(oldIndex + indexOffset) = Me.Value(oldIndex)
    oldIndex = oldIndex + 1
  Wend
  indexOffset = lastOldIndex + indexOffset
  Select Case NewValueOption
    Case 1: valNew2 = Me.Value(dsS.NVALS)
    Case 2: valNew2 = valNew1
            valNew1 = Me.Value(dsS.NVALS)
  End Select
  'Finally we append new values to the end, if necessary
  For index = 1 To NumAppendVal
    Select Case NewValueOption
      Case 0, 1: newval(index + indexOffset) = valNew2
      Case 2:    newval(index + indexOffset) = valNew1 + (valNew2 - valNew1) * index / NumAppendVal
      Case 3:
        oldIndex = cloneIndex + index - 1
        While oldIndex > Me.Dates.Summary.NVALS
          oldIndex = oldIndex - Me.Dates.Summary.NVALS + cloneIndex - 1
        Wend
        newval(index + indexOffset) = Me.Value(oldIndex)
    End Select
  Next
  
  Set tsOut = New ATCclsTserData
  Set dsOut = New ATCclsTserDate
  Set tsOut.Dates = dsOut
  dsS.SJDay = sjdate
  dsS.EJDay = ejdate
  dsS.CIntvl = True
  dsS.Intvl = (ejdate - sjdate) / NumNewVal
  dsS.ts = Me.Dates.Summary.ts
  dsS.Tu = Me.Dates.Summary.Tu
  dsS.NVALS = NumNewVal
  dsOut.Summary = dsS
  
  tsOut.Values = newval
  Set AddRemoveDates = tsOut

End Function

Public Sub AttribSet(AttrName As String, AttrValue As String, Optional AttrDef As ATCclsAttributeDefinition)
'##SUMMARY Set value of the named attribute to the specified value.
'##PARAM AttrDef Reference to attribute defintion for value checking.
'##PARAM AttrValue New value of attribute
'##PARAM AttrName Name of attribute being set.
  Dim a As Variant, fnd As Boolean, lAttr As ATTimSerAttribute, attrIndex As Long
  
  Select Case UCase(AttrName)
    Case "TYPE", "FILE", "START", "END", "NVAL", "LONG FILENAME", "PATH", "TUNIT", "TSTEP":
      pErrorDescription = "AttribSet cannot change attribute '" & AttrName & "' - Current value of '" & Attrib(AttrName) & "' was not changed."
    Case "DSN", "ID":             pHeader.id = AttrValue
    Case "SCENARIO", "IDSCEN":    pHeader.Sen = AttrValue
    Case "LOCATION", "IDLOCN":    pHeader.Loc = AttrValue
    Case "CONSTITUENT", "IDCONS": pHeader.Con = AttrValue
    Case "STATION NAME", "DESC":  pHeader.Desc = AttrValue
    Case "MAX":                   Max = AttrValue
    Case "MIN":                   Min = AttrValue
    Case "MEAN":                  Mean = AttrValue
    Case "GEOMETRIC MEAN":        GeometricMean = AttrValue
    Case "SUM":                   Sum = AttrValue
    Case "STDDEVIATION":          StdDeviation = AttrValue
    Case "VARIANCE":              Variance = AttrValue
    Case "HEADERCOMPLETE":        pHeaderComplete = AttrValue
    Case Else
      fnd = False
      For attrIndex = 1 To pAttributes.Count
        If pAttributes(attrIndex).Name = AttrName Then 'update existing
          lAttr = pAttributes(attrIndex)
          lAttr.Value = AttrValue
          pAttributes.Remove attrIndex
          If attrIndex < pAttributes.Count Then
            pAttributes.Add lAttr, AttrName, attrIndex + 1
          Else
            pAttributes.Add lAttr, AttrName
          End If
          fnd = True
          Exit For
        End If
      Next
      If Not (fnd) Then
        lAttr.Name = AttrName
        lAttr.Value = AttrValue
        If IsMissing(AttrDef) Then
          Set lAttr.Definition = New ATCclsAttributeDefinition
          lAttr.Definition.Name = AttrName
          lAttr.Definition.DataType = NONE
        ElseIf AttrDef Is Nothing Then
          Set lAttr.Definition = New ATCclsAttributeDefinition
          lAttr.Definition.Name = AttrName
          lAttr.Definition.DataType = NONE
        Else
          Set lAttr.Definition = AttrDef
        End If
        pAttributes.Add lAttr, AttrName
      End If
  End Select
End Sub

Public Property Get Attrib(AttrName As String, Optional AttrDefault As String = "") As String
'##SUMMARY Gets text value by name from Attribs collection or from several _
 other "attribute-like" values stored in different parts of the class.
'##PARAM AttrDefault Default value for attribute. Returned if _
 value is not set.
'##PARAM AttrName Name of attribute to get value for.
  Dim a As Variant, d(6) As Long
  
  Attrib = AttrDefault
  On Error GoTo ErrHand
  Select Case UCase(AttrName)
    Case "TYPE":                          Attrib = pFile.Label
    Case "FILE":                          Attrib = FilenameOnly(pFile.filename)
    Case "DSN", "ID":                     Attrib = CStr(pHeader.id)
    Case "SCENARIO", "IDSCEN", "SCEN":    Attrib = pHeader.Sen
    Case "LOCATION", "IDLOCN", "LOCN":    Attrib = pHeader.Loc
    Case "CONSTITUENT", "IDCONS", "CONS": Attrib = pHeader.Con
    Case "SJDAY": CompleteHeaderRead:     Attrib = pDates.Summary.SJDay
    Case "EJDAY": CompleteHeaderRead:     Attrib = pDates.Summary.EJDay
    Case "START":
                CompleteHeaderRead
                Call J2Date(pDates.Summary.SJDay, d)
                Attrib = CStr(d(0)) & "/" & CStr(d(1)) & "/" & CStr(d(2))
    Case "END":
                CompleteHeaderRead
                Call J2Date(pDates.Summary.EJDay, d)
                Call timcnv(d) 'convert to hour=24 for midnight
                Attrib = CStr(d(0)) & "/" & CStr(d(1)) & "/" & CStr(d(2))
    Case "NVAL": CompleteHeaderRead: Attrib = CStr(pDates.Summary.NVALS)
    Case "STATION NAME", "DESC":     Attrib = pHeader.Desc
    Case "LONG FILENAME", "PATH":    Attrib = pFile.filename
    Case "MAX":                      Attrib = Max
    Case "MIN":                      Attrib = Min
    Case "MEAN":                     Attrib = Mean
    Case "GEOMETRIC MEAN":           Attrib = GeometricMean
    Case "SUM":                      Attrib = Sum
    Case "STDDEVIATION":             Attrib = StdDeviation
    Case "VARIANCE":                 Attrib = Variance
    Case "TUNIT":                    Attrib = pDates.Summary.Tu
    Case "TSTEP":                    Attrib = pDates.Summary.ts
    Case "HEADERCOMPLETE":           Attrib = pHeaderComplete
    Case "HEADERONLY":               Attrib = pHeaderOnly
    Case Else
      'Room for improvement: use FastCollection and always set keys to Ucase(AttrName)
      'so we don't have to loop here, replace the For loop below with:
      'i = pAttributes.IndexFromKey(Ucase(AttrName))
      'If i > 0 then Attrib = pAttributes.ItemByIndex(i).Value
      For Each a In pAttributes
        If UCase(a.Name) = UCase(AttrName) Then
          Attrib = a.Value
          Exit For
        End If
      Next a
  End Select
  Exit Property
ErrHand:
  Debug.Print "Attrib(" & AttrName & ") Error: " & Err.Description
End Property

Public Property Get AttribNumeric(AttrName As String, Optional AttrDefault As Long = 0) As Double
'##SUMMARY Gets&nbsp;numeric value by name from Attribs collection or _
 from several other "attribute-like" values stored in different _
 parts of the class.
'##PARAM AttrName Name of attribute to get value for.
'##PARAM AttrDefault Default value of attribute.  Returned if attribute not available.
  Dim str As String
  
  str = Attrib(AttrName, (AttrDefault))
  If IsNumeric(str) Then
    AttribNumeric = str
  Else
    AttribNumeric = AttrDefault
  End If
End Property

Public Property Get AttribNames() As Collection
'##SUMMARY Gets collection of attribute&nbsp;names available in the _
 timeseries
  Dim a As Variant
  Dim retval As Collection
  
  Set retval = Nothing
  Set retval = New Collection
  
  retval.Add "Type", "Type"
  retval.Add "File", "File"
  retval.Add "DSN", "DSN"
  retval.Add "Scenario", "IDSCEN"
  retval.Add "Location", "IDLOCN"
  retval.Add "Constituent", "IDCONS"
  retval.Add "Start", "Start"
  retval.Add "SJDay", "SJDay"
  retval.Add "End", "End"
  retval.Add "EJDay", "EJDay"
  retval.Add "Nval", "Nval"
  retval.Add "Station Name", "Station Name"
  retval.Add "Long Filename", "Long Filename"
  retval.Add "Max", "Max"
  retval.Add "Min", "Min"
  retval.Add "Mean", "Mean"
  retval.Add "Geometric Mean", "Geometric Mean"
  retval.Add "Sum", "Sum"
  retval.Add "StdDeviation", "StdDeviation"
  retval.Add "Variance", "Variance"
  retval.Add "TUnit", "TUnit"
  retval.Add "TStep", "TSTep"
  
  On Error Resume Next
  For Each a In pAttributes
    retval.Add a.Name, a.Name
  Next

  Set AttribNames = retval
End Property

'##SUMMARY Collection of available attributes in timeseries
Public Property Set Attribs(newV As Collection) ' of ATCData.ATTimSerAttribute
  Set pAttributes = newV
End Property
Public Property Get Attribs() As Collection ' of ATCData.ATTimSerAttribute
  Set Attribs = pAttributes
End Property

Public Sub calcSummary()
'##SUMMARY Calculate summary information about timeseries (mean, variance, _
 min, max, etc).
  Dim i&, x As Single, sumSquares As Double
  Dim tmpGeometricMean As Double
  Static RaisedError As Boolean
  
  On Error GoTo ErrHandler
  'On Error GoTo 0
  
  If DataNotRead Then Exit Sub
  pSum = 0
  sumSquares = 0
  tmpGeometricMean = 0
  
  pMean = 0
  pGeometricMean = 0
  pVariance = 0
  pStdDeviation = 0
  
  If UBound(pValues) = 0 Then
    pMax = 0
    pMin = 0
  Else
    pMax = -1E+30
    pMin = 1E+30
    For i = 1 To UBound(pValues)
      x = pValues(i)
      If x > pMax Then pMax = x
      If x < pMin Then pMin = x
      pSum = pSum + x
      sumSquares = sumSquares + x * x
      If pMin > 0 Then tmpGeometricMean = tmpGeometricMean + Log(x)
    Next i
      
    i = UBound(pValues)
    If i > 0 Then
      pMean = pSum / i
      
      If pMin > 0 Then pGeometricMean = Exp(tmpGeometricMean / i)
      
      If i > 1 Then
        pVariance = (sumSquares - (pSum * pSum) / i) / (i - 1)
        If pVariance > 0 Then pStdDeviation = Sqr(pVariance)
      End If
    End If
  End If
  pNeedSummaryRecalc = False
  Exit Sub

ErrHandler:
  If Not RaisedError Then
    RaisedError = True
    'MsgBox Err.Description, vbOKOnly, "Error calculating data summary"
  End If
End Sub

'##SUMMARY Dates present on timeseries.
Public Property Set Dates(newV As ATCData.ATCclsTserDate)
  Set pDates = newV
End Property
Public Property Get Dates() As ATCclsTserDate
  CompleteHeaderRead
  Set Dates = pDates
End Property

'##SUMMARY Text description of most recent error.
Public Property Let ErrorDescription(newError$)
  pErrorDescription = newError
End Property
Public Property Get ErrorDescription() As String
  ErrorDescription = pErrorDescription
  pErrorDescription = ""
End Property

'##SUMMARY Timeseries file object containing this timeseries.
Public Property Set File(newFile As ATCData.ATCclsTserFile)
  Set pFile = newFile
End Property
Public Property Get File() As ATCclsTserFile
  Set File = pFile
End Property

Public Property Get Flag(index As Long) As Long
'##SUMMARY Get value of a flag associated with a specific date/value _
 pair.
  
  If DataNotRead Then Call pFile.readData(Me)
  
  If index < 0 Or index > UBound(pFlags) Then
    Flag = 0
  Else
    Flag = pFlags(index)
  End If
End Property

'##SUMMARY Flags for this timeseries.
Public Property Let flags(newFlags() As Long)
  pFlags = newFlags
End Property
Public Property Get flags() As Long()
  If DataNotRead Then Call pFile.readData(Me)
  flags = pFlags
End Property

'##SUMMARY Header for this timesereies.
Public Property Set Header(newHeader As ATCData.ATTimSerDataHeader)
  Set pHeader = newHeader
End Property
Public Property Get Header() As ATCData.ATTimSerDataHeader
  Set Header = pHeader
End Property

'##SUMMARY Serial number assigned in order of creation. Guaranteed _
 to be unique at runtime but will be different from one run to another
Public Property Get Serial() As Long
  Serial = pSerial
End Property

'Public Property Let Summary(newV As ATCData.ATTimSerDataSummary)
'  pSummary = newV
'End Property
'Public Property Get Summary() As ATCData.ATTimSerDataSummary
'  If DataNotRead Then Call pFile.readData(Me)
'  Summary = pSummary
'End Property

'##SUMMARY Data transformation.
Public Property Let Dtran(newV As Long)
  pDtran = newV
End Property
Public Property Get Dtran() As Long
  If DataNotRead Then Call pFile.readData(Me)
  Dtran = pDtran
End Property

'##SUMMARY Mimimum value in timseries
Public Property Let Min(newV As Single)
  pMin = newV
End Property
Public Property Get Min() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  Min = pMin
End Property

'##SUMMARY Maximum value in timeseries
Public Property Let Max(newV As Single)
  pMax = newV
End Property
Public Property Get Max() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  Max = pMax
End Property

'##SUMMARY Mean value in timeseries
Public Property Let Mean(newV As Single)
  pMean = newV
End Property
Public Property Get Mean() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  Mean = pMean
End Property

'##SUMMARY Geometric mean value in timeseries
Public Property Let GeometricMean(newV As Single)
  pGeometricMean = newV
End Property
Public Property Get GeometricMean() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  GeometricMean = pGeometricMean
End Property

'##SUMMARY Summation of values in timeseries
Public Property Let Sum(newV As Single)
  pSum = newV
End Property
Public Property Get Sum() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  Sum = pSum
End Property

'##SUMMARY Variance of values in timeseries
Public Property Let Variance(newV As Single)
  pVariance = newV
End Property
Public Property Get Variance() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  Variance = pVariance
End Property

'##SUMMARY Standard deviation of values in timeseries
Public Property Let StdDeviation(newV As Single)
  pStdDeviation = newV
End Property
Public Property Get StdDeviation() As Single
  If DataNotRead Then Call pFile.readData(Me)
  If pNeedSummaryRecalc Then calcSummary
  StdDeviation = pStdDeviation
End Property

'##SUMMARY Data value in timeseries at specified index
Public Property Get Value(index As Long) As Single
  Static lastErrTime As Single
  
  If DataNotRead Then Call pFile.readData(Me)
  If index < 1 Then
    If Timer - lastErrTime > 1 Then
      MsgBox "index = '" & index & "' not valid. Values begin at index 1" & vbCr & "(ATCclsTserData Get Value)"
    End If
    lastErrTime = Timer
  ElseIf index > UBound(pValues) Then
    If Timer - lastErrTime > 1 Then
      MsgBox "Index '" & index & "' > maximum of '" & UBound(pValues) & "'" & vbCr & "(ATCclsTserData Get Value)"
    End If
    lastErrTime = Timer
  Else
    Value = pValues(index)
  End If
End Property
Public Property Let Value(index As Long, newValue As Single)
  If index < 1 Then
    MsgBox "index = '" & index & "' not valid. Values begin at index 1" & vbCr, vbOKOnly, "ATCclsTserData Let Value"
  ElseIf index > UBound(pValues) Then
    MsgBox "Index '" & index & "' > maximum of '" & UBound(pValues) & "'", vbOKOnly, "ATCclsTserData Let Value"
  Else
    'If Abs(pValues(index) = pMax) Then
      pNeedSummaryRecalc = True
    'ElseIf pValues(index) = pMin Then
    '  pNeedSummaryRecalc = True
    'End If
    pValues(index) = newValue
    'If newValue > pMax Then pMax = newValue
    'If newValue < pMin Then pMin = newValue
  End If
End Property

'##SUMMARY Array of values in timeseries
Public Property Let Values(newValues() As Single)
  pValues = newValues
  pNeedSummaryRecalc = True
End Property
Public Property Get Values() As Single()
  If DataNotRead Then Call pFile.readData(Me)
  Values = pValues
End Property

'##SUMMARY Returns a new ATCclsTserData containing data between specified julian dates
'##PARAMETER_DESCRIPTION s Starting Julian Date
'##PARAMETER_DESCRIPTION e Ending Julian Date
Public Function SubSetByDate(S As Double, e As Double) As ATCclsTserData
  Dim lTs As ATCclsTserData
  Dim i&, j&
  Dim spos&, cnt&
  Dim dataflg&(), dataval!(), dateflg&(), dateval#()
  Dim lAttribute As Variant
  
  CompleteHeaderRead
  
  Set lTs = New ATCclsTserData
  
  Set lTs.Header = pHeader.Copy
  Set lTs.File = pFile
  
  For Each lAttribute In pAttributes
    lTs.Attribs.Add lAttribute
  Next
  
  With pDates.Summary
    If DataNotRead And _
      ((.CIntvl And .SJDay = S And .EJDay = e) Or (Not .CIntvl)) Then
      Call pFile.readData(Me) 'Read whole dataset if that is what is requested
    End If
  End With
  
  Set lTs.Dates = pDates.SubSetByDate(S, e, spos)
  
  If DataNotRead Then
    Call pFile.readData(lTs)
  Else
    'spos = pDates.IndexAtOrAfter(s)
    'epos = pDates.IndexAtOrBefore(e)
    'cnt = epos - spos + 1
    cnt = lTs.Dates.Summary.NVALS
    If cnt > 0 Then
      ReDim dataval(cnt)
      CopyMemory dataval(1), pValues(spos), cnt * 4 'ByVal VarPtr(dataval(1)), ByVal VarPtr(pValues(spos)), cnt * 4
    End If
    lTs.Values = dataval
    If UBound(pFlags) = UBound(pValues) And cnt > 0 Then
      ReDim dataflg(cnt)
      CopyMemory dataflg(1), pFlags(spos), cnt * 4
      lTs.flags = dataflg
    End If
    lTs.calcSummary
  End If
  
  Set SubSetByDate = lTs
  
End Function

'##SUMMARY Returns a new ATCclsTserData containing data generated from the table provided
'##PARAMETER_DESCRIPTION rangeTop contains an array of the top values in a range of _
values to be converted. Values must appear in ascending order -- rangeTop(i) < rangeTop(i+1)
'##PARAMETER_DESCRIPTION newValue contains the new values for data in the corresponding _
range.  Values larger than rangeTop(n) will remain unchanged in the new time series
'##PARAMETER_DESCRIPTION Interpolate interpolation flag<br> _
if False, for each range defined in rangeTop(i), _
all values will be set to newValue(i)<br> _
if True, values in each range will be interpolated between newValue(i-1) and newValue(i) _
values in the first range will be assigned to newValue(1) without interpolation
Public Function doTable(rangeTop() As Single, newValue() As Single, Interpolate As Boolean) As ATCclsTserData
  Dim datavals() As Single, dataIndex As Long
  Dim pValueIndex As Long, NVALS As Long
  Dim rangeIndex As Long, rangeMax As Long
  Dim retval As ATCclsTserData
  Dim NeedNewDates As Boolean
  Dim newDateValues() As Double
  
  If DataNotRead Then Call pFile.readData(Me)
  
  rangeMax = UBound(rangeTop)
  NVALS = Me.Dates.Summary.NVALS
  
  For rangeIndex = 1 To rangeMax
    If newValue(rangeIndex) = -911 Then
      NeedNewDates = True
      ReDim newDateValues(NVALS)
      Exit For
    End If
  Next
  
  ReDim datavals(NVALS)
  dataIndex = 1
  For pValueIndex = 1 To NVALS
    rangeIndex = 1
    While pValues(pValueIndex) > rangeTop(rangeIndex)
      rangeIndex = rangeIndex + 1
      If rangeIndex > rangeMax Then
        datavals(dataIndex) = pValues(pValueIndex)
        GoSub SetDate
        rangeIndex = 1
        dataIndex = dataIndex + 1
        pValueIndex = pValueIndex + 1
        If pValueIndex > NVALS Then Exit For
      End If
    Wend
    If newValue(rangeIndex) = -911 Then 'Delete
      
    ElseIf newValue(rangeIndex) = -912 Then 'Leave Unchanged
      datavals(dataIndex) = pValues(pValueIndex)
      GoSub SetDate
      dataIndex = dataIndex + 1
    ElseIf Not Interpolate Or rangeIndex = 1 Then
      datavals(dataIndex) = newValue(rangeIndex)
      GoSub SetDate
      dataIndex = dataIndex + 1
    Else
      datavals(dataIndex) = newValue(rangeIndex - 1) + _
                          (pValues(dataIndex) - rangeTop(rangeIndex - 1)) / _
                          (rangeTop(rangeIndex) - rangeTop(rangeIndex - 1))
      GoSub SetDate
      dataIndex = dataIndex + 1
    End If
  Next
  dataIndex = dataIndex - 1
  Set retval = New ATCclsTserData

  If NeedNewDates And dataIndex <> NVALS Then 'deleted some values
    Dim newDateSummary As ATTimSerDateSummary
    newDateSummary.CIntvl = False
    newDateSummary.EJDay = newDateValues(dataIndex)
    newDateSummary.NVALS = dataIndex
    newDateSummary.SJDay = newDateValues(1)
    ReDim Preserve datavals(dataIndex)
    ReDim Preserve newDateValues(dataIndex)
    Set retval.Dates = New ATCclsTserDate
    retval.Dates.Summary = newDateSummary
    retval.Dates.Values = newDateValues
  Else
    Set retval.Dates = pDates
  End If
  Set retval.File = pFile
  retval.Values = datavals
  retval.calcSummary
  Set doTable = retval
  Exit Function
  
SetDate:
  If NeedNewDates Then
    If pDates.Summary.CIntvl Then 'Shifting from constant to non-constant interval
      If pValueIndex = 1 Then
        newDateValues(dataIndex) = pDates.Summary.SJDay
      Else
        newDateValues(dataIndex) = pDates.Value(pValueIndex - 1)
      End If
    Else
      newDateValues(dataIndex) = pDates.Value(pValueIndex)
    End If
  End If
  Return

End Function

'##SUMMARY Returns a new ATCclsTserData containing data generated using the supplied math function and parameters
Public Function doMath(oper As ATCOperatorType, ts2 As ATCclsTserData, Optional x As Double = 0, Optional X2 As Double = 0) As ATCclsTserData
  Dim dataflg&(), datavals() As Single, i&, d!
  Dim NVALS As Long
  Dim lTs As ATCclsTserData
  Dim lErrorDescription$
  
  Set lTs = New ATCclsTserData
  Set lTs.Dates = Me.Dates
  
  NVALS = Me.Dates.Summary.NVALS
  ReDim dataflg(NVALS)
  ReDim datavals(NVALS)
  If ts2.Header.id = 0 Then 'fill with ts1
    Set ts2.Dates = Me.Dates
    ts2.Values = datavals
  End If

  If doDateCheck(Me.Dates, ts2.Dates) Then
    Select Case oper
    Case ATCSubtract
      For i = 1 To NVALS
        datavals(i) = Me.Value(i) - ts2.Value(i) + x
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCCummDiff
      d = 0
      For i = 1 To NVALS
        d = Me.Value(i) - ts2.Value(i) + d
        datavals(i) = d
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCAdd
      For i = 1 To NVALS
        datavals(i) = Me.Value(i) + ts2.Value(i) + x
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCMultiply
      If ts2.Header.id = 0 Then 'no 2nd timeseries, just multiply by constant
        For i = 1 To NVALS
          datavals(i) = Me.Value(i) * x + X2
          'dataflg(i) = 0 'might want to update
        Next i
      Else
        For i = 1 To NVALS
          datavals(i) = (Me.Value(i) * ts2.Value(i)) + x
          'dataflg(i) = 0 'might want to update
        Next i
      End If
    Case ATCDivide
      For i = 1 To NVALS
        datavals(i) = (Me.Value(i) / ts2.Value(i)) + x
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCAve
      For i = 1 To NVALS
        datavals(i) = (Me.Value(i) + ts2.Value(i)) / 2
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCWeight
      For i = 1 To NVALS
        datavals(i) = (Me.Value(i) * x) + (ts2.Value(i) * X2)
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCTSPowC
      For i = 1 To NVALS
        datavals(i) = Me.Value(i) ^ x
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCCPowTS
      For i = 1 To NVALS
        datavals(i) = x ^ Me.Value(i)
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCTSPowTS
      For i = 1 To NVALS
        datavals(i) = Me.Value(i) ^ ts2.Value(i)
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCExp
      For i = 1 To NVALS
        datavals(i) = Exp(Me.Value(i))
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCLog
      For i = 1 To NVALS
        datavals(i) = Log(Me.Value(i)) / Log(10#)
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCLn
      For i = 1 To NVALS
        datavals(i) = Log(Me.Value(i))
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCAbs
      For i = 1 To NVALS
        datavals(i) = Abs(Me.Value(i))
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCTSltC
      For i = 1 To NVALS
        If Me.Value(i) < x Then
          datavals(i) = X2
        Else
          datavals(i) = Me.Value(i)
        End If
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCTSgtC
      For i = 1 To NVALS
        If Me.Value(i) > x Then
          datavals(i) = X2
        Else
          datavals(i) = Me.Value(i)
        End If
        'dataflg(i) = 0 'might want to update
      Next i
'    Case ATCTSleC
'      For i = 1 To NVALS
'        If Me.Value(i) <= x Then
'          datavals(i) = x2
'        Else
'          datavals(i) = Me.Value(i)
'        End If
'      Next i
'    Case ATCTSgeC
'      For i = 1 To NVALS
'        If Me.Value(i) >= x Then
'          datavals(i) = x2
'        Else
'          datavals(i) = Me.Value(i)
'        End If
'        'dataflg(i) = 0 'might want to update
'      Next i
    Case ATCMin
      For i = 1 To NVALS
        If ts2.Value(i) < Me.Value(i) Then
          datavals(i) = ts2.Value(i)
        Else
          datavals(i) = Me.Value(i)
        End If
        'dataflg(i) = 0 'might want to update
      Next i
    Case ATCMax
      For i = 1 To NVALS
        If ts2.Value(i) > Me.Value(i) Then
          datavals(i) = ts2.Value(i)
        Else
          datavals(i) = Me.Value(i)
        End If
        'dataflg(i) = 0 'might want to update
      Next i
    Case Else
      lErrorDescription = "Unknown operator: " & oper
    End Select
  Else
    lErrorDescription = "Dates of Timeseries do not match."
  End If
  
  If Len(lErrorDescription) = 0 Then
    lTs.Values = datavals
    lTs.flags = dataflg
    lTs.calcSummary
  Else
    lTs.ErrorDescription = lErrorDescription
  End If
  
  Set doMath = lTs
  
End Function

Public Function Copy() As ATCclsTserData
'##SUMMARY Returns a copy of this timeseries.
  Dim lTs As ATCclsTserData, lAttribute As Variant, lAttribs As Collection
  
  Set lTs = New ATCclsTserData
  Set lTs.Dates = Me.Dates
  Set lTs.File = pFile
  lTs.Values = Values
  lTs.Dtran = pDtran
  lTs.Min = pMin
  lTs.Max = pMax
  Set lAttribs = New Collection
  For Each lAttribute In pAttributes
    lAttribs.Add lAttribute
  Next
  Set lTs.Attribs = lAttribs
  Set lTs.Header = pHeader.Copy
  lTs.flags = flags
  Set Copy = lTs
End Function

Public Function doDateCheck(d1 As ATCclsTserDate, d2 As ATCclsTserDate) As Boolean
'##SUMMARY Checks to see if two instances of ATCclsTserDate are equivalent.
'##PARAM d2 second date class to check.
'##PARAM d1 first date class to check.
  Dim i&, ds1 As ATTimSerDateSummary, ds2 As ATTimSerDateSummary
  Dim d&(6)
  
  doDateCheck = True
  
  ds1 = d1.Summary
  ds2 = d2.Summary
  
  If ds1.CIntvl <> ds2.CIntvl Then
                                    doDateCheck = False
  ElseIf ds1.EJDay <> ds2.EJDay Then
                                    doDateCheck = False
  ElseIf ds1.Intvl <> ds2.Intvl Then
                                    doDateCheck = False
  ElseIf ds1.NVALS <> ds2.NVALS Then
                                    doDateCheck = False
  ElseIf ds1.SJDay <> ds2.SJDay Then
                                    doDateCheck = False
  ElseIf ds1.ts <> ds2.ts Then
                                    doDateCheck = False
  ElseIf ds1.Tu <> ds2.Tu Then
                                    doDateCheck = False
  Else
    For i = 1 To ds1.NVALS
      If d1.Value(i) <> d2.Value(i) Then
        doDateCheck = False
        Exit For
      End If
    Next i
  End If
End Function

'##SUMMARY Returns a new timeseries based on an existing _
timeseries object (ATCclsTserData containing "old" values and _
dates) and a "new" set of dates (ATCclsTserDate). All header and _
attribute information of the original timeseries is copied to the _
new timeseries.<p> _
For new dates occurring before the first old date, the first old _
value is used without interpolation. Similarly for new dates after _
the last old date, the last old value is used without interpolation. _
In between, each new value is calculated by linear interpolation _
between the closest old value before and the closest old value _
after each new date.
Public Function Interpolate(ds As ATCclsTserDate) As ATCclsTserData
  Dim dateOld1 As Double, dateOld2 As Double, dateNew As Double
  Dim oldIndex&, newIndex&, numOldVals&, numNewVals&
  Dim valOld1 As Single, valOld2 As Single
  Dim newVals() As Single
  Dim newTS As ATCclsTserData
  Set newTS = Nothing
  Set newTS = New ATCclsTserData
  
  If DataNotRead Then Call pFile.readData(Me)
  
  Dim Item As Variant
  For Each Item In pAttributes
    newTS.AttribSet Item.Name, Item.Value
  Next
  
  Set newTS.Dates = ds
  Set newTS.File = File
  newTS.flags = pFlags
  Set newTS.Header = pHeader.Copy
  oldIndex = 1
  dateOld2 = pDates.Value(oldIndex)
  dateOld1 = dateOld2
  valOld2 = pValues(oldIndex)
  valOld1 = valOld2
  numOldVals = pDates.Summary.NVALS
  numNewVals = ds.Summary.NVALS
  ReDim newVals(numNewVals)
  newIndex = 1
  While newIndex <= numNewVals
    dateNew = ds.Value(newIndex)
    While dateNew > dateOld2 And oldIndex < numOldVals
      'Debug.Print newIndex, oldIndex
      oldIndex = oldIndex + 1
      dateOld1 = dateOld2
      dateOld2 = pDates.Value(oldIndex)
      valOld1 = valOld2
      valOld2 = pValues(oldIndex)
    Wend
    If dateNew > dateOld2 Or oldIndex = 1 Then
      'First or last old value is used for new dates outside the range of old dates
      newVals(newIndex) = valOld2
    Else
      newVals(newIndex) = valOld1 + (valOld2 - valOld1) * (dateNew - dateOld1) / (dateOld2 - dateOld1)
    End If
    newIndex = newIndex + 1
  Wend
  newTS.Values = newVals
  newTS.calcSummary
  newTS.Header.Sen = "Interp:" & newTS.Header.Sen
  Set Interpolate = newTS
End Function

'##SUMMARY Returns a new ATCclsTserData aggregated based on the input _
 parameters.
Public Function Aggregate(ds As ATCclsTserDate, Tran As ATCTran) As ATCclsTserData
'##PARAM Tran Transformation function
'##PARAM ds Date class containing dates for aggregatoin
  Dim dateOld As Double, dateNew As Double, prevDateOld As Double, prevDateNew As Double
  Dim overlapStart As Double, overlapEnd As Double, ymd&(5)
  Dim oldIndex&, newIndex&, numOldVals&, numNewVals&, numAverageVals&
  Dim valOld As Single
  Dim fraction As Single
  Dim cumuFrac As Single 'Not sure if we really want this. Tries to make up for uneven end of interval, I think
  Dim newVals() As Single
  Dim newTS As ATCclsTserData
  
  Set newTS = Nothing
  Set newTS = New ATCclsTserData
  
  If DataNotRead Then Call pFile.readData(Me)
  
  Dim Item As Variant
  For Each Item In pAttributes
    newTS.AttribSet Item.Name, Item.Value
  Next
  
  Set newTS.Dates = ds
  Set newTS.File = File
  newTS.flags = pFlags
  Set newTS.Header = pHeader.Copy

'  If Not ds.Summary.CIntvl Then
'    pErrorDescription = "Cannot aggregate to a point (non-constant interval) dataset." & vbCr _
'           & "Interpolation is reccommended in this case."
'    Set Aggregate = newts
'    Exit Function '----------------------------------------------------------------------
'  End If
  
  numNewVals = ds.Summary.NVALS
  ReDim newVals(numNewVals)
  newIndex = 1
  dateNew = ds.Value(newIndex)
  prevDateNew = ds.Summary.SJDay ' dateNew - (ds.Value(newIndex + 1) - dateNew)
  
  oldIndex = pDates.IndexAtOrAfter(prevDateNew)
  dateOld = pDates.Value(oldIndex)
  numOldVals = pDates.Summary.NVALS
  If numOldVals > 0 Then valOld = pValues(oldIndex)
  
  If pDates.Summary.Tu = 5 Then
    prevDateOld = dateOld
    Call J2Date(dateOld - JulianSecond, ymd)
    prevDateOld = dateOld - daymon(ymd(0), ymd(1)) 'Subtract number of days in prev month
  Else
    prevDateOld = dateOld - (pDates.Value(oldIndex + 1) - dateOld) 'Dangerous assumption - bad when old dates are years
  End If
  Select Case Tran
    
    Case TranAverSame, TranSumDiv
      While newIndex <= numNewVals
        dateNew = ds.Value(newIndex)
        newVals(newIndex) = 0
        While prevDateOld < dateNew And oldIndex <= numOldVals
          If prevDateOld > prevDateNew Then overlapStart = prevDateOld Else overlapStart = prevDateNew
          If dateNew > dateOld Then overlapEnd = dateOld Else overlapEnd = dateNew
          fraction = (overlapEnd - overlapStart) / (dateNew - prevDateNew)
          cumuFrac = cumuFrac + fraction
          If Tran = TranSumDiv Then
            fraction = (overlapEnd - overlapStart) / (dateOld - prevDateOld)
          End If
          newVals(newIndex) = newVals(newIndex) + fraction * valOld
          If prevDateOld < dateNew Then
            If dateOld > dateNew Then 'use remaining part of this old interval on next new interval
              prevDateOld = dateNew
              If Tran = TranSumDiv Then valOld = valOld - valOld * fraction
            Else
              prevDateOld = dateOld
              oldIndex = oldIndex + 1
              If oldIndex <= numOldVals Then
                dateOld = pDates.Value(oldIndex)
                valOld = pValues(oldIndex)
              End If
              cumuFrac = 0
            End If
          End If
        Wend
        prevDateNew = dateNew
        If Tran = TranSumDiv And cumuFrac > 0.01 And cumuFrac < 0.999 Then
          newVals(newIndex) = newVals(newIndex) / cumuFrac
        End If
        newIndex = newIndex + 1
      Wend
      
    Case TranMax
      While newIndex <= numNewVals
        dateNew = ds.Value(newIndex)
        newVals(newIndex) = -1E+30
        While dateOld <= dateNew And oldIndex <= numOldVals
          If newVals(newIndex) < valOld Then newVals(newIndex) = valOld
          oldIndex = oldIndex + 1
          If oldIndex <= numOldVals Then
            dateOld = pDates.Value(oldIndex)
            valOld = pValues(oldIndex)
          End If
        Wend
        newIndex = newIndex + 1
      Wend
      
    Case TranMin
      While newIndex <= numNewVals
        dateNew = ds.Value(newIndex)
        newVals(newIndex) = 1E+30
        While dateOld <= dateNew And oldIndex <= numOldVals
          If newVals(newIndex) > valOld Then newVals(newIndex) = valOld
          oldIndex = oldIndex + 1
          If oldIndex <= numOldVals Then
            dateOld = pDates.Value(oldIndex)
            valOld = pValues(oldIndex)
          End If
        Wend
        newIndex = newIndex + 1
      Wend

  End Select
  newTS.Values = newVals
  newTS.calcSummary
  Set Aggregate = newTS
End Function

'##SUMMARY Fill values in timeseries with specified value.
Public Function FillValues(ByVal ts As Long, ByVal Tu As ATCTimeUnit, ByVal FillVal As Single, ByVal MissingVal As Single, AccumVal As Single) As ATCclsTserData
'##PARAM AccumVal Value indicating accumulated data.
'##PARAM MissingVal Value indicating missing data.
'##PARAM FillVal Value to Fill with.
'##PARAM Tu Time units.
'##PARAM ts Timestep in units of TU.</P>
  Dim dateOld As Double, dateNew As Double, Intvl As Double
  Dim newDateSummary As ATTimSerDateSummary
  Dim oldIndex&, newIndex&, numOldVals&, numNewVals&, VarLength&
  Dim valOld As Single, ymd&(5)
  Dim newVals() As Single
  Dim newTS As ATCclsTserData
  Set newTS = Nothing
  Set newTS = New ATCclsTserData
  
  If DataNotRead Then Call pFile.readData(Me)
  
  Dim Item As Variant
  For Each Item In pAttributes
    newTS.AttribSet Item.Name, Item.Value
  Next
  
  If Tu = 0 Then
    Tu = TUDay: MsgBox "TU value of 0 not specified in FillValues. Defaulting to days.", vbOKOnly, "ATCData:FillValues"
  End If
                    
  Select Case Tu
    Case TUSecond:  Intvl = ts / 86400#: VarLength = 0
    Case TUMinute:  Intvl = ts / 1440#: VarLength = 0
    Case TUHour:    Intvl = ts / 24#: VarLength = 0
    Case TUDay:     Intvl = ts: VarLength = 0
    Case TUMonth:   Intvl = ts * 30.44: VarLength = 1
    Case TUYear:    Intvl = ts * 365.25: VarLength = 2
    Case TUCentury: Intvl = ts * 36525: VarLength = 3
  End Select
  
  With newDateSummary
    .CIntvl = True
    If VarLength = 1 Then
      .SJDay = pDates.Value(1)
      Call J2Date(pDates.Value(pDates.Summary.NVALS), ymd)
      .EJDay = pDates.Value(pDates.Summary.NVALS) + daymon(ymd(0), ymd(1))
      .NVALS = (.EJDay - .SJDay) / Intvl
    Else
      .SJDay = pDates.Value(1) - Intvl
      .EJDay = pDates.Value(pDates.Summary.NVALS)
      .NVALS = (.EJDay - .SJDay) / Intvl
    End If
    .Intvl = Intvl
    .ts = ts
    .Tu = Tu
  End With
  Set newTS.Dates = New ATCclsTserDate
  newTS.Dates.Summary = newDateSummary
  Set newTS.File = File
  newTS.flags = pFlags
  Set newTS.Header = pHeader.Copy
  oldIndex = 1
  dateOld = pDates.Value(oldIndex)
  valOld = 0
  'valOld = pValues(oldIndex)
  numOldVals = pDates.Summary.NVALS
  numNewVals = newDateSummary.NVALS
  ReDim newVals(numNewVals)
  newIndex = 1
  dateNew = dateOld
  While newIndex <= numNewVals
    While dateNew < dateOld - JulianSecond 'Fill values not present in original data
      Select Case valOld
        Case MissingVal:
                         If pValues(oldIndex) = MissingVal Then
                           newVals(newIndex) = MissingVal
                         Else
                           newVals(newIndex) = FillVal
                         End If
        Case AccumVal:   newVals(newIndex) = AccumVal
        Case Else:       newVals(newIndex) = FillVal
      End Select
      newIndex = newIndex + 1
      If VarLength = 1 Then
        Call J2Date(dateNew, ymd)
        dateNew = dateNew + daymon(ymd(0), ymd(1))
      Else
        dateNew = dateNew + Intvl
      End If
    Wend
    valOld = pValues(oldIndex)
    newVals(newIndex) = valOld
    If VarLength = 1 Then
      Call J2Date(dateNew, ymd)
      dateNew = dateNew + daymon(ymd(0), ymd(1))
    Else
      dateNew = dateNew + Intvl
    End If
    newIndex = newIndex + 1
    oldIndex = oldIndex + 1
    If oldIndex <= numOldVals Then
      dateOld = pDates.Value(oldIndex)
    End If
  Wend
  newTS.Values = newVals
  newTS.calcSummary
  Set FillValues = newTS
End Function

'##SUMMARY FillMissing fills existing missing values in a time series.
Public Function FillMissing(FillMethod As String, MaxNMis As Long, MVal As Single) As ATCclsTserData
'##PARAM MVal Value already in data that indicates a missing value.
'##PARAM MaxNMis if more than this many values in a row are missing, _
 do not fill
'##PARAM FillMethod <P>valid values:<BR>INTERPOLATE - between valid _
 values around missing<BR>AVERAGE - between valid values around missing<BR>LAST _
 - fill with last valid value<BR>NEXT - fill with next valid value
  Dim i&, j&, lVal!, NMis&, InMissing As Boolean
  Dim dateOld1 As Double, dateOld2 As Double, dateDiff As Double
  Dim oldIndex&, newIndex&, numOldVals&, numNewVals&
  Dim valOld1 As Single, valOld2 As Single, valDiff As Single, valNew As Single
  Dim newVals() As Single
  Dim newTS As ATCclsTserData
  Set newTS = Nothing
  Set newTS = New ATCclsTserData
  
  If DataNotRead Then Call pFile.readData(Me)
  
  Dim Item As Variant
  For Each Item In pAttributes
    newTS.AttribSet Item.Name, Item.Value
  Next
  
  Set newTS.Dates = pDates
  Set newTS.File = File
  newTS.flags = pFlags
  Set newTS.Header = pHeader.Copy
  ReDim newVals(pDates.Summary.NVALS)
  NMis = 0
  lVal = MVal
  For i = 1 To pDates.Summary.NVALS
    newVals(i) = pValues(i)
    If pValues(i) = MVal Then
      NMis = NMis + 1
    Else
      If NMis > 0 Then
        If NMis <= MaxNMis Then 'fill missing period
          valOld1 = pValues(i - NMis - 1)
          valOld2 = pValues(i)
          valDiff = valOld2 - valOld1
          dateOld1 = pDates.Value(i - NMis - 1)
          dateOld2 = pDates.Value(i)
          dateDiff = dateOld2 - dateOld1
          Select Case UCase(FillMethod)
            Case "INTERPOLATE"
              If valOld1 <> MVal And valOld2 <> MVal Then 'valid values on both ends
                For j = i - NMis To i - 1
                  newVals(j) = valOld1 + valDiff * (pDates.Value(j) - dateOld1) / dateDiff
                Next j
              End If
            Case "AVERAGE"
              If valOld1 <> MVal And valOld2 <> MVal Then 'valid values on both ends
                valNew = (valOld1 + valOld2) / 2
                For j = i - NMis To i - 1
                  newVals(j) = valNew
                Next j
              End If
            Case "LAST"
              If valOld1 <> MVal Then
                For j = i - NMis To i - 1
                  newVals(j) = valOld1
                Next j
              End If
            Case "NEXT"
              If valOld2 <> MVal Then
                For j = i - NMis To i - 1
                  newVals(j) = valOld2
                Next j
              End If
          End Select

          'If FillMethod = "INTERPOLATE" Then
          '  If lval <> MVal Then 'valid values on both ends
          '    For j = 1 To NMis
          '      newVals(i - j) = pValues(i) - ((pValues(i) - pValues(i - NMis - 1)) * _
          '                      ((pDates.Value(i) - pDates.Value(i - j)) / (pDates.Value(i) - pDates.Value(i - NMis - 1))))
          '    Next j
          '  End If
          'End If
        End If
      End If
      NMis = 0
    End If
  Next i
  newTS.Values = newVals
  newTS.calcSummary
  newTS.Header.Sen = "FillMiss:" & newTS.Header.Sen
  Set FillMissing = newTS
End Function

'##SUMMARY Compare this timeseries to the specified one&nbsp; Returns _
 True if a match.
Public Function Compare(nTs As ATCclsTserData) As Boolean
'##PARAM nTs Timeseries to compare against.
  If pSerial = nTs.Serial Then
    Compare = True
  Else
    Compare = pHeader.Compare(nTs.Header)
    'check other stuff here?
  End If
End Function

Private Function DataNotRead() As Boolean
  On Error Resume Next 'if UBound(pValues) gives and error, DataNotRead = True
  If UBound(pValues) = 0 Then
    DataNotRead = True
  Else
    DataNotRead = False
  End If
End Function

Private Sub CompleteHeaderRead()
  Static CompletingHeader As Boolean
  If Not (pHeaderComplete) And Not CompletingHeader Then
    CompletingHeader = True
    pHeaderOnly = True 'just need the header
    pFile.readData Me 'read the header
    pHeaderComplete = True 'now complete
    pHeaderOnly = False 'read everything next time
    CompletingHeader = False
  End If
End Sub

Private Sub Class_Initialize()
  Set pAttributes = New Collection
  Set pHeader = New ATTimSerDataHeader
  Set pDates = Nothing
  Set pFile = Nothing
  ReDim pValues(0)
  ReDim pFlags(0) 'Set pFlags = Nothing
  pSerial = NextDataSerial
  pHeaderComplete = True 'set so that existing apps continue to work, false if wdm quick read
  pHeaderOnly = False
  NextDataSerial = NextDataSerial + 1
End Sub

Private Sub Class_Terminate()
  Set pAttributes = Nothing
  ReDim pValues(0) ' = Nothing
  ReDim pFlags(0) 'Set pFlags = Nothing
End Sub
