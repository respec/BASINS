VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "RegistryHandler"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

'-------------------------------------------------------------------------------------------'
'   This Registry handler is developed by Ronald Kas (r.kas@kaycys.com)                     '
'   from Kaycys (http://www.kaycys.com).                                                    '
'                                                                                           '
'   You may use this Registry Handler for all purposes except from making profit with it.   '
'   Check our site regulary for updates.                                                    '
'-------------------------------------------------------------------------------------------'
'   excerpted and adapted by jlkittle on 4/29/03

Enum HKEYS
    HKEY_CLASSES_ROOT = &H80000000
    HKEY_CURRENT_USER = &H80000001
    HKEY_LOCAL_MACHINE = &H80000002
    HKEY_USERS = &H80000003
End Enum


Friend Function GetRegValue(RootKey As HKEYS, sKey As String, sValueName As String) As Variant
    Dim hKeyHandle As Long
    Dim vTemp As Variant
    Call RegOpenKeyEx(RootKey, sKey, 0&, lngKEY_ALL_ACCESS, hKeyHandle)
    ReadRegistryValue hKeyHandle, sValueName, vTemp
    Call RegCloseKey(hKeyHandle)
    GetRegValue = vTemp
End Function


Friend Function EnumKeys(RootKey As HKEYS, sKey As String) As Variant
    Dim hKeyHandle As Long
    Dim x As Variant
    Call RegOpenKeyEx(RootKey, sKey, 0&, lngKEY_ALL_ACCESS, hKeyHandle)
    If hKeyHandle = 0 Then
      'x = "Key '" & sKey & "' not found in registry"
    Else
      Call EnumerateRegistryKeys(hKeyHandle, x)
      Call RegCloseKey(hKeyHandle)
    End If
    EnumKeys = x
End Function


Friend Function EnumValues(RootKey As HKEYS, sKey As String) As Variant
    Dim vEnum As Variant
    Dim lHandle As Long
    Dim vRet As Variant
    Dim i As Integer
    
    Call OpenRegistryKey(RootKey, sKey, lHandle)
    If lHandle = 0 Then
      ReDim vRet(0)
    Else
      EnumerateRegistryValuesByHandle lHandle, vEnum
      Call CloseRegistryKey(lHandle)
      ReDim vRet(UBound(vEnum, 2))
      For i = 0 To UBound(vEnum, 2)
           vRet(i) = vEnum(1, i)
      Next i
    End If
    EnumValues = vRet
    
End Function

'------------------------------------------------------------------

Private Function CloseRegistryKey(ByVal vhKeyHandle As Long) As Boolean
    Dim lngReturn As Long
    
    lngReturn = RegCloseKey(vhKeyHandle)
    If lngReturn <> lngERROR_SUCCESS Then
        CloseRegistryKey = False
    Else
        CloseRegistryKey = True
    End If
End Function

Private Function EnumerateRegistryKeys(ByVal vhKeyHandle As Long, ByRef rvntKeys As Variant) As String
    Dim strValue As String, strClass As String, strMessage As String, strError As String
    Dim hKeyHandle As Long, lngDataLen As Long, lngValueLen As Long, lngReturn As Long, lngIndex As Long
    Dim lngClass As Long
    Dim strNodes() As String
    Dim typFileTime As FILE_TIME
    
    lngIndex = 0
    
    If vhKeyHandle = 0 Then
      ReDim strNodes(0)
    Else
      ' then loop through the nodes under the 'base node'...
      Do
        lngValueLen = 2000
        strValue = String(lngValueLen, 0)
        lngDataLen = 2000
      
        ' and read the names of all the nodes under it...
        lngReturn = RegEnumKeyEx(vhKeyHandle, lngIndex, strValue, lngValueLen, 0&, strClass, lngClass, typFileTime)
        strValue = Left(strValue, lngValueLen)
        ' checking for problems.
        If lngReturn <> lngERROR_SUCCESS And lngReturn <> lngNO_MORE_NODES Then
        End If
        
        ' Add each node into an array...
        ReDim Preserve strNodes(lngIndex)
        strNodes(lngIndex) = strValue
        lngIndex = lngIndex + 1
      
        ' and loop until the enumeration return fails.
      Loop While lngReturn <> lngNO_MORE_NODES
    End If
    rvntKeys = strNodes()
    Erase strNodes

End Function

Private Function EnumerateRegistryValuesByHandle(ByVal vhKeyHandle As Long, ByRef rvntValues As Variant) As String
    Dim strValue As String, strMessage As String, strError As String
    Dim lngData As String, lngDataLen As Long, lngValueLen As Long, lngReturn As Long, lngIndex As Long
    Dim lngValueType As Long
    Dim strNodes() As String
    Dim typFileTime As FILE_TIME
    
    If vhKeyHandle = 0 Then
      ReDim strNodes(0)
    Else
      ' loop through the nodes under the 'base node'...
      Do
        lngValueLen = 2000
        strValue = String(lngValueLen, 0)
        lngDataLen = 2000
      
        ' and read the names of all the nodes under it...
        lngReturn = RegEnumValue(vhKeyHandle, lngIndex, ByVal strValue, lngValueLen, 0&, lngValueType, _
                                 ByVal lngData, lngDataLen)
        strValue = Left(strValue, lngValueLen)
      
        ' checking for problems.
        If lngReturn <> lngERROR_SUCCESS And lngReturn <> lngNO_MORE_NODES Then
        End If
      
        ' Add each node into an array...
        ReDim Preserve strNodes(0 To 1, 0 To lngIndex)
        strNodes(0, lngIndex) = CStr(lngValueType)
        strNodes(1, lngIndex) = strValue
        lngIndex = lngIndex + 1
        
        ' and loop until the enumeration return fails.
      Loop While lngReturn <> lngNO_MORE_NODES
    End If
    rvntValues = strNodes()
    Erase strNodes

End Function

Private Function OpenRegistryKey(ByVal vhRootKey As Long, ByVal vstrKeyName As String, ByRef rhKeyHandle As Long) As String
    Dim lngReturn As Long, hKeyHandle As Long
    
    ' then open the passed registry node (key) in the passed root key...
    lngReturn = RegOpenKeyEx(vhRootKey, vstrKeyName, 0&, lngKEY_ALL_ACCESS, hKeyHandle)
    If lngReturn <> lngERROR_SUCCESS Then
    End If
    
    rhKeyHandle = hKeyHandle
End Function

Private Function ReadRegistryValue(ByVal vhKeyHandle As Long, ByVal vstrValueName As String, _
                                  ByRef rvntValue As Variant) As String
  Dim strMessage As String, strError As String, strValueName As String, strData As String
  Dim lngReturn As Long, lngIndex As Long, lngValuesCount As Long, lngValueType As Long, lngValueLen As Long
  Dim lngValueMax As Long, lngData As Long, lngDataLen As Long
  Dim blnData As Boolean
  Dim vntValues As Variant
  Dim typFileTime As FILE_TIME
  
  ' Check that all required variables have been passed...
  If vhKeyHandle <= 0 Then Exit Function
  'If vstrValueName = "" Then Exit Function (empty string here gets default value, jlk 4/30/03
  
  ' and enumerate the keys to see what type of value is stored in the one to return. First get the number of values
  ' and the maximum name length of those stored in the passed key...
  lngReturn = RegQueryInfoKey(vhKeyHandle, "", 0&, 0&, 0&, 0&, 0&, lngValuesCount, lngValueMax, _
                              0&, 0&, typFileTime)
  If lngReturn = lngERROR_SUCCESS Then
    lngValueLen = Len(vstrValueName) + 1
    
    ' then loop through the values until the requested value name is found.
    Call EnumerateRegistryValuesByHandle(vhKeyHandle, vntValues)
    For lngIndex = 0 To UBound(vntValues, 2)
      lngReturn = lngERROR_FAILURE
      strValueName = vntValues(1, lngIndex)
        
      
      ' Check that the currently enumerated key is the one requested...
      If LCase(vstrValueName) = LCase(strValueName) Then
        lngValueType = vntValues(0, lngIndex)
        lngValueLen = Len(strValueName)
        
        ' and, depending on the value type, read and return the stored value...
        Select Case lngValueType
          Case lngREG_BINARY
      
            ' it's a binary value...
            lngDataLen = 1
            lngReturn = RegEnumValue(vhKeyHandle, lngIndex, strValueName, lngValueLen, 0&, lngValueType, _
                                     blnData, lngDataLen)
            rvntValue = blnData
            Exit For
          Case lngREG_DWORD
      
            ' it's a DWord...
            lngDataLen = 4
            lngReturn = RegEnumValue(vhKeyHandle, lngIndex, strValueName, lngValueLen, 0&, lngValueType, _
                                     lngData, lngDataLen)
            rvntValue = lngData
            Exit For
          Case lngREG_SZ
      
            ' it's a string value.
            lngDataLen = 2048
            strData = String(lngDataLen, 0)
            lngReturn = RegQueryValueEx(vhKeyHandle, strValueName, 0&, lngValueType, strData, lngDataLen)
            rvntValue = Left(strData, lngDataLen - 1)
            Exit For
        End Select
      End If
    Next
  End If

End Function

Private Function ReadValue(ByVal vhRootKey As Long, ByVal vstrKeyName As String, ByVal vstrValueName As String, ByRef rvntValue As Variant, Optional ByVal vvntDefault As Variant) As String
  Dim strReturn As String, strLanguageOffset As String, strMessage As String, strError As String
  Dim hKeyHandle As Long, lngReturn As Long, lngValueType As Long, hNewKeyHandle As Long
  
 
  ' Call each of the neccessary functions in turn starting with OpenRegistryKey...
  Do
    Call OpenRegistryKey(vhRootKey, vstrKeyName, hKeyHandle)
    ' then read the value...
    strReturn = ReadRegistryValue(hKeyHandle, vstrValueName, rvntValue)
    If strReturn <> "" Then
      If Not IsMissing(vvntDefault) And rvntValue = "" Or rvntValue = 0 Then
        'GoSub ValueWrite
      Else
        ReadValue = strReturn
        Exit Function
      End If
    Else
      Exit Do
    End If
  
    ' and close the opened key.
    Call CloseRegistryKey(hKeyHandle)
  Loop
  
  ' and close the opened key.
  Call CloseRegistryKey(hKeyHandle)

End Function
