VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "clsOption"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Attribute VB_Ext_KEY = "SavedWithClassBuilder" ,"Yes"
Attribute VB_Ext_KEY = "Top_Level" ,"Yes"
Option Explicit

'local variables to hold property values
Private mvarValue As Variant
Private mvarDefaultValue As Variant
Private mvarLabel As String
Private mvarControlType As Long
Private mvarVisible As Boolean
Private mvarValid As Boolean

Public Property Let DefaultValue(ByVal vData As Variant)
  mvarDefaultValue = vData
End Property

Public Property Get DefaultValue() As Variant
  DefaultValue = mvarDefaultValue
End Property

Public Property Let ControlType(ByVal vData As Long)
  mvarControlType = vData
End Property

Public Property Get ControlType() As Long
  ControlType = mvarControlType
End Property

Public Property Let Label(ByVal vData As String)
  mvarLabel = vData
End Property

Public Property Get Label() As String
  Label = mvarLabel
End Property

Public Property Let Value(ByVal vData As Variant)
  mvarValue = vData
End Property

Public Property Get Value() As Variant
Attribute Value.VB_UserMemId = 0
  Value = mvarValue
End Property

Public Property Let Visible(ByVal vData As Boolean)
  mvarVisible = vData
End Property

Public Property Get Visible() As Boolean
  Visible = mvarVisible
End Property

Public Property Let Valid(ByVal vData As Boolean)
  mvarValid = vData
End Property

Public Property Get Valid() As Boolean
  Valid = mvarValid
End Property

