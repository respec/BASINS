VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "clsFileView"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"No"
Option Explicit

'True if this class can open and display this file
Public Function CanOpen(Filename As String) As Boolean
End Function

'Description of this view (e.g. "Text" or "Binary" or "ESRI Shape"
Public Function Desc() As String
End Function

'Save changes on current page back in original file
Public Sub SaveChanges()
End Sub

'Opens file in appropriate mode using myPar.Filename and myPar.FileHandle
'Saves myPar to access .txtMain and other controls during subsequent calls
Public Sub OpenFile(myPar As Form)
End Sub

'Populate myPar.txtMain from the open file starting at frmPar.FirstDispByte
'linePrefix = string inserted at start of each line
Public Sub DispPage(ByVal linePrefix As String)
End Sub

'Returns first byte position in file that should be displayed to scroll the specified amount
'If any parameter is zero, no scrolling means frmPar.FirstDispByte will be returned unchanged
'Direction: -1 = back toward beginning of file, 1 = moving toward end of file
'Units: 1=byte, 2=line, 3=page, 4=logical units of file (same as page if no logical units)
'Distance: number of units to scroll. If beyond end of file, scrolls to end of file
Public Function ScrollPos(ByVal Direction&, ByVal Units&, ByVal Distance&) As Long
End Function

'Find an occurrence of the SearchFor string in this page or in the file
'Calls DispPage if it is necessary to move to another page
'Direction: 0=first, 1=next, 2=previous, 3=last
Public Sub Find(SearchFor As String, Direction As Long)
End Sub
