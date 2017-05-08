VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsVBtypes"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Public Enum VBitemType
  vbi_Misc = 0
  vbi_Group
  vbi_Project
  vbi_Reference
  vbi_Component
  vbi_Form
  vbi_UserControl
  vbi_Module
  vbi_Class
  vbi_FormObject
  vbi_Attribute
  vbi_Comment
  vbi_Enum
  vbi_Event
  vbi_Type
  vbi_Const
  vbi_Variable
  vbi_Sub
  vbi_PropertyGet
  vbi_PropertyLet
  vbi_PropertySet
  vbi_Function
  vbi_List
End Enum
