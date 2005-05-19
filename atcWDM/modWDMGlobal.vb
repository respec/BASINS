Option Strict Off
Option Explicit On 

Imports atcData
Imports ATCutility

Module WDMGlobal

  'VB date 0 is 30Dec1899, MJD date 0 is 17Nov1858
  '15018 julian days from WDM zero date to VB zero date
  Public Const MJDto1900 As Double = -15018

  Private pMsgUnit As Integer 'fortran unit number for hspfmsg.wdm; used by clsTserWDM
  Private pMsg As atcMsgWDM      'see clsMsgWDM
	
  Private pUnitsDef As atcAttributeDefinition
  Private pUnitsDefEditable As atcAttributeDefinition

  Friend Function gMsg() As atcMsgWDM
    FindHSPFMsgWdm()
    Return pMsg
  End Function

  Friend Function gMsgUnit() As Integer
    FindHSPFMsgWdm()
    gMsgUnit = pMsgUnit
  End Function

  Friend Sub SetMsgUnit(ByRef newValue As Integer)
    If newValue > 0 Then
      pMsgUnit = newValue
      pMsg = New atcMsgWDM
      pMsg.MsgUnit = pMsgUnit
    End If
  End Sub

  Private Sub FindHSPFMsgWdm()
    If pMsgUnit = 0 Then 'need to find, open and process HSPF message file
      Dim hspfMsgFileName As String = FindFile("Please locate HSPF message file", "hspfmsg.wdm")
      If Not FileExists(hspfMsgFileName) Then
        LogMsg("Could not find hspfmsg.wdm - " & hspfMsgFileName, "ATCTSfile")
      Else
        pMsgUnit = F90_WDBOPN(CInt(0), hspfMsgFileName, Len(hspfMsgFileName))
        pMsg = New atcMsgWDM
        pMsg.MsgUnit = pMsgUnit
      End If
    End If
  End Sub

  ' used by clsTserWDM and clsTSerSWATDbf (in clsTSerDBF.cls)
  Public Function UnitsAttributeDefinition(Optional ByRef Editable As Boolean = False) As atcAttributeDefinition
    If pUnitsDef Is Nothing Then 'Need to create pUnitsDef and pUnitsDefEditable
      pUnitsDef = New atcAttributeDefinition
      pUnitsDefEditable = New atcAttributeDefinition

      pUnitsDef.Name = "Units"
      pUnitsDefEditable.Name = "Units"

      pUnitsDef.TypeString = "String"
      pUnitsDefEditable.TypeString = "String"

      pUnitsDef.Editable = False
      pUnitsDefEditable.Editable = True

      pUnitsDefEditable.ValidList = GetAllUnitsInCategory("all")
      'pUnitsDef.ValidValues = pUnitsDefEditable.ValidValues
    End If

    If Editable Then
      Return pUnitsDefEditable
    Else
      Return pUnitsDef
    End If
  End Function

End Module
