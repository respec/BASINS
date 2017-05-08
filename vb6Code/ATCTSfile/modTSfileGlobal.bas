Attribute VB_Name = "modTSfileGlobal"
Option Explicit

'For hspfmsg.wdm; used by clsTserWDM
Private pMsgUnit As Long
Private pMsg As ATCoMsgWDM

Private pUnitsDef As ATCclsAttributeDefinition
Private pUnitsDefEditable As ATCclsAttributeDefinition

Public Function gMsgUnit() As Long
  If pMsgUnit = 0 Then FindHSPFMsgWdm
  gMsgUnit = pMsgUnit
End Function

Public Sub SetMsgUnit(newValue As Long)
  If newValue > 0 Then
    pMsgUnit = newValue
    Set pMsg = New ATCoMsgWDM
    pMsg.MsgUnit = pMsgUnit
  End If
End Sub

Public Function gMsg() As ATCoMsgWDM
  If pMsgUnit = 0 Then FindHSPFMsgWdm
  Set gMsg = pMsg
End Function

Private Sub FindHSPFMsgWdm()
  Dim ff As New ATCoFindFile
  Dim hspfMsgFileName As String
  ff.SetDialogProperties "Please locate HSPF message file", "hspfmsg.wdm"
  ff.SetRegistryInfo "WinHSPF", "files", "hspfmsg.wdm"
  hspfMsgFileName = ff.GetName
'  If Not FileExists(hspfMsgFileName) Then
'SearchForFile:
'    With frmSelectScript.dlgOpenFile
'      .filename = "hspfmsg.wdm"
'      .DialogTitle = "Please locate HSPF message file"
'      .Filter = "WDM files (*.wdm)|*.wdm|All files (*.*)|*.*"
'      .FilterIndex = 1
'      .ShowOpen
'      hspfMsgFileName = .filename
'    End With
'    If FileExists(hspfMsgFileName) Then SaveSetting "HASS_ENT", "files", "HSPFMsgWDM", hspfMsgFileName
'  End If
'  If Not FileExists(hspfMsgFileName) Then GoTo SearchForFile
  If Not FileExists(hspfMsgFileName) Then
    MsgBox "Could not find hspfmsg.wdm - " & hspfMsgFileName, vbCritical, "ATCTSfile"
  Else
    pMsgUnit = F90_WDBOPN(CLng(1), hspfMsgFileName, Len(hspfMsgFileName))
    Set pMsg = New ATCoMsgWDM
    pMsg.MsgUnit = pMsgUnit
  End If
End Sub

' used by clsTserWDM and clsTSerSWATDbf (in clsTSerDBF.cls)
Public Function UnitsAttributeDefinition(Optional Editable As Boolean = False) As ATCclsAttributeDefinition
  Dim collUnits As FastCollection
  Dim UnitIndex As Long
  Dim retval As ATCclsAttributeDefinition
  Dim o As Object
    
  On Error GoTo ErrHand
  
  If pUnitsDef Is Nothing Then
  
    Set pUnitsDef = New ATCclsAttributeDefinition
    Set pUnitsDefEditable = New ATCclsAttributeDefinition
    
    pUnitsDef.DataType = ATCoTxt:         pUnitsDef.Name = "Units"
    pUnitsDefEditable.DataType = ATCoTxt: pUnitsDefEditable.Name = "Units"
    
    'Ignore error if we are working with an old version of atcdata without ATCclsAttributeDefinition.Editable
    On Error Resume Next
    Set o = pUnitsDef:         o.Editable = False
    Set o = pUnitsDefEditable: o.Editable = True
    On Error GoTo ErrHand
    
    Set collUnits = GetAllUnitsInCategory("all")
    For UnitIndex = 1 To collUnits.Count
      pUnitsDefEditable.ValidValues = pUnitsDefEditable.ValidValues & collUnits(UnitIndex)
      If UnitIndex < collUnits.Count Then
        pUnitsDefEditable.ValidValues = pUnitsDefEditable.ValidValues & ","
      End If
    Next
    'pUnitsDef.ValidValues = pUnitsDefEditable.ValidValues
  End If
  
  If Editable Then
    Set UnitsAttributeDefinition = pUnitsDefEditable
  Else
    Set UnitsAttributeDefinition = pUnitsDef
  End If
  Exit Function
ErrHand:
  MsgBox err.Description, vbOKOnly, "ATCTSFile UnitsAttributeDefinition"
End Function

