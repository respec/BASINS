VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "ATCPlugInManager"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Base 0
Option Explicit
'##PROJECT_TITLE ATCPlugInManager.dll
'##PROJECT_SUMMARY Manages PlugIn functionality
'##PROJECT_OVERVIEW_BEFORE_DIAGRAM
'##PROJECT_OVERVIEW_AFTER_DIAGRAM

'##MODULE_NAME ATCPlugInManager
'##MODULE_DATE 10/10/2003
'##MODULE_AUTHOR Mark Gray
'##MODULE_SUMMARY
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license
'##MODULE_SEEALSO Target="ATCPlugInManage~ATCTypes", Caption="Supporting Types"
'##MODULE_SEEALSO Target="ATCPlugInManage~ATCclsCatalog", Caption="Catalog"
'
' <><><><><><>< Private Variables ><><><><><><><>

Private pPlugInsAvail As Collection   'skeletons for each possible plugin
Private pPlugInsActive As Collection  'instances of plugins
Private pPlugInsActiveCurrent As Long 'index of "current" plugin
Private pSerialNumber As Long         'serial number of plugin creation
Private pErrorDescription As String   'Store description of error for later inspection by Get ErrorDescription

Public Property Get ErrorDescription() As String
'##SUMMARY ErrorDescription - Return a description of the most recent error
   ErrorDescription = pErrorDescription
   pErrorDescription = ""
End Property

Private Sub Class_Initialize()
'##SUMMARY Class_Initialize - Clear the private variables
  Clear
End Sub

Public Sub Clear()
'##SUMMARY Clear - Set all the private variables to default values
  pErrorDescription = ""
  Set pPlugInsAvail = Nothing
  Set pPlugInsAvail = New Collection
  
  Set pPlugInsActive = Nothing
  Set pPlugInsActive = New Collection
  pSerialNumber = 0
  pPlugInsActiveCurrent = 0
End Sub

Public Property Get Avail() As Collection
'##SUMMARY Get Avail - Return the collection of available plugins
  Set Avail = pPlugInsAvail
End Property

Public Property Get Active() As Collection
'##SUMMARY Get Avail - Return the collection of active plugins
  Set Active = pPlugInsActive
End Property

Public Property Get CurrentActive() As ATCPlugIn
'##SUMMARY Get Avail - Return the Current Active plugin
  CurrentActive = pPlugInsActive(pPlugInsActiveCurrent)
End Property

Public Property Let CurrentActiveIndex(ByVal newValue As Long)
'##SUMMARY Let CurrentActiveIndex - Set the Current Active plugin index
'##PARAM newValue - long (1..Active.Count)
  pPlugInsActiveCurrent = newValue
End Property

Public Property Get CurrentActiveIndex() As Long
'##SUMMARY Get CurrentActiveIndex - Return the Current Active plugin index (1..Active.Count)
  CurrentActiveIndex = pPlugInsActiveCurrent
End Property

Public Function ActiveIndexByName(PluginName As String) As Long
'##SUMMARY Get ActiveIndexByName - Return the active plugin index (1..Active.Count) of the named plugin _
   Returns 0 if PluginName is not found
'##PARAM PluginName - name of the desired plugin
  Dim p As Variant, i As Long, fnd As Boolean
  Dim searchStr$
  
  searchStr = UCase(PluginName)
  i = 0
  fnd = False
  For Each p In pPlugInsActive
    i = i + 1
    If UCase(p.Name) = searchStr Then
      ActiveIndexByName = i
      fnd = True
      Exit For
    End If
  Next p
  If Not (fnd) Then
    ActiveIndexByName = 0
    pErrorDescription = "PlugIn Name " & PluginName & " not found in PlugInsActive Collection"
  End If
End Function

Public Function AvailIndexByName(PluginName As String) As Long
'##SUMMARY Get AvailIndexByName - Return the available plugin index (1..Avail.Count) of the named plugin _
   Returns 0 if PluginName is not found
'##PARAM PluginName - name of the desired plugin
  Dim p As Variant
  Dim i As Long
  Dim fnd As Boolean
  Dim searchStr$
  
  searchStr = UCase(PluginName)
  i = 0
  fnd = False
  For Each p In pPlugInsAvail
    i = i + 1
    If UCase(p.ClassName) = searchStr Then
      AvailIndexByName = i
      fnd = True
      Exit For
    End If
  Next p
  If Not (fnd) Then
    AvailIndexByName = 0
    pErrorDescription = "PlugIn Class " & PluginName & " not found in PlugInsAvail Collection"
  End If
End Function

Public Sub Create(AvailIndex As Long)
'##SUMMARY Create - Create a new active plugin from the available plugin at the specified index _
   The newly created active plugin is available as CurrentActive
'##PARAM AvailIndex (1..Avail.Count) of the named plugin
  Dim t$
  Dim o As Object
  Dim plug As ATCPlugIn
  
  plug = pPlugInsAvail(AvailIndex)
  Set o = CreateObject(plug.PluginName & "." & plug.ClassName)
  Set plug.obj = o
  plug.ID = pSerialNumber
  pSerialNumber = pSerialNumber + 1
  
  pPlugInsActive.Add plug
  pPlugInsActiveCurrent = pPlugInsActive.Count
  
End Sub

Public Sub Delete(ActiveIndex As Long) 'get rid of an instance
'##SUMMARY Delete - Remove the active plugin at the specified index
'##PARAM ActiveIndex (1..Avail.Count) of the named plugin
  Dim p As ATCclsTserFile
  
  Set p = pPlugInsActive(ActiveIndex).obj
  p.Clear
  Set p = Nothing
  pPlugInsActive.Remove ActiveIndex
  If pPlugInsActiveCurrent >= ActiveIndex Then
    pPlugInsActiveCurrent = pPlugInsActive.Count
  End If
    
End Sub

'Finds a plugin registered in Windows and returns its type
'On error, returns "" and sets ErrorDescription
Public Function QueryType(PluginName$) As String
  Dim catalog As ATCclsCatalog
  Set catalog = CreateObject(PluginName & ".ATComponentCatalog")
  catalog
End Function

Public Function Load(PluginName As String) As Boolean 'from an ocx or dll
  Dim plug As ATCPlugIn
  Dim collItem As Variant
  Dim s$
  Dim catalog As ATCclsCatalog
  Dim o As Object

  Load = False 'will set to true after load is successful

  On Error GoTo CrCatErr
  Set catalog = CreateObject(PluginName & ".ATComponentCatalog")
  On Error GoTo 0
  plug.ClassName = PluginName
  
  For Each collItem In catalog.Plugins
    plug = collItem
    If Left(plug.ClassName, 1) = "@" Then ' catalog
      s = Mid(plug.ClassName, 2)
      If Not (Load(s)) Then
        pErrorDescription = ErrorDescription & " Load: Unable to Load Embedded Plug In: " & s & " (Referenced in " & PluginName & ")"
        Load = False
        Exit Function
      End If
    Else
      On Error GoTo CrErr
      plug.PluginName = PluginName
      s = PluginName & "." & plug.ClassName
      Set plug.obj = CreateObject(s) 'master copy of this plugin
      On Error GoTo 0
      plug.ID = pSerialNumber
      pSerialNumber = pSerialNumber + 1
      pPlugInsAvail.Add plug
    End If
  Next collItem
  Load = True
  Exit Function
CrCatErr:
  pErrorDescription = "Load: Unable to open ComponentCatalog for " & PluginName
  Exit Function
CrErr:
  pErrorDescription = "Load: Unable to open Component: " & s
  Exit Function
End Function

Private Sub DumpActive()
  Dim i As Long, lPlugin As ATCPlugIn, lTimser As ATCclsTserFile
  
  For i = 1 To pPlugInsActive.Count
    lPlugin = pPlugInsActive(i)
    If lPlugin.PluginType = "ATCclsTserFile" Then
      Set lTimser = lPlugin.obj
      Debug.Print lPlugin.ClassName, lTimser.Filename, lTimser.FileUnit
    End If
  Next

End Sub
