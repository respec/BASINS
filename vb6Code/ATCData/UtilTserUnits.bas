Attribute VB_Name = "UtilTserUnits"
Option Explicit
'Copyright 2001 by AQUA TERRA Consultants

'Requres frmSpecifyUnits and UtilUnits

'Creates a new ATCclsTserData based on oldTs, but with units converted
Public Function ConvertTserUnits(oldTs As ATCclsTserData, NewUnits As String) As ATCclsTserData
  Dim NVALS As Long
  Dim i As Long
  Dim MyUnits As String
  Dim MyUnitCategory As String, NewUnitCategory As String
  Dim ConversionFactor As Double, degFPdegC As Double
  Dim datavals() As Single
  Dim newTS As ATCclsTserData
  Set newTS = oldTs.Copy
  NVALS = oldTs.dates.Summary.NVALS
  ReDim datavals(NVALS)
  MyUnits = oldTs.Attrib("Units")
  If Len(MyUnits) = 0 Or LCase(MyUnits) = "unknown" Then
    newTS.ErrorDescription = "Source data did not have units defined"
  Else
    MyUnitCategory = GetUnitCategory(MyUnits)
    NewUnitCategory = GetUnitCategory(NewUnits)
    If MyUnitCategory <> NewUnitCategory Then
      newTS.ErrorDescription = "Source data units '" & MyUnits & "' (" & MyUnitCategory & ") cannot be converted to '" & NewUnits & "' (" & NewUnitCategory & ")"
    ElseIf LCase(NewUnits) = LCase(MyUnits) Then
      GoTo SkipSettingValues
    ElseIf MyUnitCategory = "Temperature" Then
      newTS.AttribSet "Units", NewUnits
      degFPdegC = 9# / 5#
      Select Case MyUnits
        Case "degF"
          Select Case NewUnits
            Case "degC"
              For i = 1 To NVALS
                datavals(i) = (oldTs.value(i) - 32) / degFPdegC
              Next i
            Case "K"
              For i = 1 To NVALS
                datavals(i) = (oldTs.value(i) - 32) / degFPdegC + 273.15
              Next i
            Case "rankine"
              For i = 1 To NVALS
                datavals(i) = oldTs.value(i) + 459.67
              Next i
          End Select
        Case "degC"
          Select Case NewUnits
            Case "degF"
              For i = 1 To NVALS
                datavals(i) = oldTs.value(i) * degFPdegC + 32
              Next i
            Case "K"
              For i = 1 To NVALS
                datavals(i) = oldTs.value(i) + 273.15
              Next i
            Case "rankine"
              For i = 1 To NVALS
                datavals(i) = oldTs.value(i) * degFPdegC + 32 + 459.67
              Next i
          End Select
      End Select
    Else 'Non-temperature units all just require multiplication
      newTS.AttribSet "Units", NewUnits
      ConversionFactor = GetConversionFactor(MyUnits, NewUnits)
      If Abs(ConversionFactor - 1) < 0.000001 Then GoTo SkipSettingValues
      For i = 1 To NVALS
        datavals(i) = oldTs.value(i) * ConversionFactor
      Next i
    End If
  End If
  newTS.Values = datavals
  newTS.calcSummary 'compute correct values, replacing copied ones from oldTs
SkipSettingValues:
  Set ConvertTserUnits = newTS
End Function

'Converts a collection of ATCclsTserData to have compatible units
'If PreferredUnits has an entry for the type of units in use by each timeseries, those units are used
'In PreferredUnits key = type of units, such as "Flow", value = specific units, such as "cfs"
'If UnitsRequired is true, the user is asked which units each time series is in if it does not already have an attribute named "Units"
Public Function ConvertCompatibleUnits(oldTs As Collection, PreferredUnits As FastCollection, Optional UnitsRequired As Boolean = False) As Collection
  Dim retval As New Collection
  Dim vOldTser As Variant
  Dim vNewTser As Variant
  Dim OldTser As ATCclsTserData
  Dim OldUnits As String, OldUnitCategory As String
  Dim NewUnits As String, NewUnitCategory As String
  Dim CheckPreferred As Boolean, PreferredIndex As Long
  
  If Not PreferredUnits Is Nothing Then
    If PreferredUnits.Count > 0 Then CheckPreferred = True
  End If
  For Each vOldTser In oldTs
    Set OldTser = vOldTser
    OldUnits = OldTser.Attrib("Units")
    
    If Len(OldUnits) = 0 Then
      If UnitsRequired Then
        frmSpecifyUnits.Caption = "Specify Units for " & vOldTser.Header.con _
                                              & " at " & vOldTser.Header.loc _
                                              & " in " & vOldTser.Header.sen
        OldUnits = frmSpecifyUnits.GetUnitsFromUser
        'OldUnits = InputBox("Please enter units for " & OldTser.attrib("Scenario") & " " & OldTser.attrib("Location") & " " & OldTser.attrib("Constituent"))
        If Len(OldUnits) > 0 Then
          OldTser.AttribSet "Units", OldUnits
        End If
      End If
    End If
    
    If Len(OldUnits) > 0 And LCase(OldUnits) <> "unknown" And LCase(OldUnits) <> "<none>" Then
      OldUnitCategory = GetUnitCategory(OldUnits)
      If LCase(OldUnitCategory) <> "unknown" And LCase(OldUnitCategory) <> "<none>" Then
        If CheckPreferred Then
          PreferredIndex = PreferredUnits.IndexFromKey(OldUnitCategory)
          If PreferredIndex > 0 Then
            NewUnits = PreferredUnits.ItemByIndex(PreferredIndex)
            If Len(NewUnits) > 0 And LCase(NewUnits) <> LCase(OldUnits) Then
              retval.Add ConvertTserUnits(OldTser, NewUnits)
              GoTo AddedThisTser
            End If
          End If
        Else
          For Each vNewTser In retval
            NewUnits = vNewTser.Attrib("Units")
            If Len(NewUnits) > 0 And LCase(NewUnits) <> "unknown" And LCase(NewUnits) <> "<none>" Then
              NewUnitCategory = GetUnitCategory(NewUnits)
              If LCase(NewUnitCategory) <> "unknown" And LCase(NewUnitCategory) <> "<none>" Then
                If LCase(NewUnitCategory) = LCase(OldUnitCategory) And LCase(NewUnits) <> LCase(OldUnits) Then
                  retval.Add ConvertTserUnits(OldTser, NewUnits)
                  GoTo AddedThisTser
                End If
              End If
            End If
          Next
        End If
      End If
    End If
    retval.Add OldTser
AddedThisTser:
  Next
  Set ConvertCompatibleUnits = retval
End Function
