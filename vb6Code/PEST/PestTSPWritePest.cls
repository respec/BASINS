VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsPestTSPWritePest"
Attribute VB_GlobalNameSpace = True
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit
'Copyright 2003 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Private pComments As String           'comments for block
Private pContext As FastCollection    'of CONTEXT strings
Private pContextCom As FastCollection 'of CONTEXT comment strings
Private pTemplateFile As FastCollection    'mandatory
Private pTemplateFileCom As FastCollection
Private pModelInputFile As FastCollection  'must follow Template_File
Private pModelInputFileCom As FastCollection
Private pParmDataFile As String
Private pParmDataFileCom As String
Private pParmGroupFile As String
Private pParmGroupFileCom As String
Private pSeries As FastCollection       'of clsPestSeries
Private pControlFile As String          'mandatory
Private pControlFileCom As String
Private pInstructFile As String         'mandatory
Private pInstructFileCom As String
Private pModelCommandLine As String
Private pModelCommandLineCom As String
Private pAUI As Boolean
Private pAUICom As String

Public Property Get Comments() As String
  Comments = pComments
End Property
Public Property Let Comments(newvalue As String)
  pComments = newvalue
End Property

Public Property Get Context() As FastCollection
  Set Context = pContext
End Property
Public Property Let Context(ByVal newvalue As FastCollection)
  Set pContext = newvalue
End Property

Public Property Get ContextCom() As FastCollection
  Set ContextCom = pContextCom
End Property
Public Property Let ContextCom(ByVal newvalue As FastCollection)
  Set pContextCom = newvalue
End Property

Public Property Get TemplateFile() As FastCollection
  Set TemplateFile = pTemplateFile
End Property
Public Property Let TemplateFile(ByVal newvalue As FastCollection)
  Set pTemplateFile = newvalue
End Property

Public Property Get TemplateFileCom() As FastCollection
  Set TemplateFileCom = pTemplateFileCom
End Property
Public Property Let TemplateFileCom(ByVal newvalue As FastCollection)
  Set pTemplateFileCom = newvalue
End Property

Public Property Get ModelInputFile() As FastCollection
  Set ModelInputFile = pModelInputFile
End Property
Public Property Let ModelInputFile(ByVal newvalue As FastCollection)
  Set pModelInputFile = newvalue
End Property

Public Property Get ModelInputFileCom() As FastCollection
  Set ModelInputFileCom = pModelInputFileCom
End Property
Public Property Let ModelInputFileCom(ByVal newvalue As FastCollection)
  Set pModelInputFileCom = newvalue
End Property

Public Property Get ParmDataFile() As String
  ParmDataFile = pParmDataFile
End Property
Public Property Let ParmDataFile(newvalue As String)
  pParmDataFile = newvalue
End Property

Public Property Get ParmDataFileCom() As String
  ParmDataFileCom = pParmDataFileCom
End Property
Public Property Let ParmDataFileCom(newvalue As String)
  pParmDataFileCom = newvalue
End Property

Public Property Get ParmGroupFile() As String
  ParmGroupFile = pParmGroupFile
End Property
Public Property Let ParmGroupFile(newvalue As String)
  pParmGroupFile = newvalue
End Property

Public Property Get ParmGroupFileCom() As String
  ParmGroupFileCom = pParmGroupFileCom
End Property
Public Property Let ParmGroupFileCom(newvalue As String)
  pParmGroupFileCom = newvalue
End Property

Public Property Get Series() As FastCollection
  Set Series = pSeries
End Property
Public Property Let Series(ByVal newvalue As FastCollection)
  Set pSeries = newvalue
End Property

Public Property Get ControlFile() As String
  ControlFile = pControlFile
End Property
Public Property Let ControlFile(newvalue As String)
  pControlFile = newvalue
End Property

Public Property Get ControlFileCom() As String
  ControlFileCom = pControlFileCom
End Property
Public Property Let ControlFileCom(newvalue As String)
  pControlFileCom = newvalue
End Property

Public Property Get InstructFile() As String
  InstructFile = pInstructFile
End Property
Public Property Let InstructFile(newvalue As String)
  pInstructFile = newvalue
End Property

Public Property Get InstructFileCom() As String
  InstructFileCom = pInstructFileCom
End Property
Public Property Let InstructFileCom(newvalue As String)
  pInstructFileCom = newvalue
End Property

Public Property Get ModelCommandLine() As String
  ModelCommandLine = pModelCommandLine
End Property
Public Property Let ModelCommandLine(newvalue As String)
  pModelCommandLine = newvalue
End Property

Public Property Get ModelCommandLineCom() As String
  ModelCommandLineCom = pModelCommandLineCom
End Property
Public Property Let ModelCommandLineCom(newvalue As String)
  pModelCommandLineCom = newvalue
End Property

Public Property Get AUI() As Boolean
  AUI = pAUI
End Property
Public Property Let AUI(newvalue As Boolean)
  pAUI = newvalue
End Property

Public Property Get AUICom() As String
  AUICom = pAUICom
End Property
Public Property Let AUICom(newvalue As String)
  pAUICom = newvalue
End Property

Public Sub Populate(str As String)
  'str is contents of WRITE_PEST_FILES block (within header and END records)
  Dim rec As String, com As String, kwd As String, spec As String
  Dim curTemplate As String, lSeries As clsPestSeries

  Set lSeries = New clsPestSeries
  com = ""
  While Len(str) > 0
    rec = Trim(StrSplit(str, vbCrLf, ""))
    If Left(rec, 1) = "#" Then 'comment line
      If Len(com) > 0 Then
        com = com & vbCrLf & "  " & rec
      Else
        com = "  " & rec
      End If
    ElseIf Len(rec) > 0 Then
      kwd = Trim(StrRetRem(rec))
      spec = Trim(rec)
      Select Case kwd
        Case "CONTEXT"
          ProcessKwdColl spec, pContext, com, pContextCom
        Case "TEMPLATE_FILE"
          ProcessKwdColl spec, pTemplateFile, com, pTemplateFileCom
          curTemplate = spec 'save Template as key if needed for ModelInputFile collection
        Case "MODEL_INPUT_FILE"
          pModelInputFile.Add spec, curTemplate 'use current Template as key to match ModelInput and Template occurences
          If Len(com) > 0 Then
            pModelInputFileCom.Add com, curTemplate
          End If
        Case "PARAMETER_DATA_FILE"
          pParmDataFile = spec
          If Len(com) > 0 Then pParmDataFileCom = com
        Case "PARAMETER_GROUP_FILE"
          pParmGroupFile = spec
          If Len(com) > 0 Then pParmGroupFileCom = com
        Case "OBSERVATION_SERIES_NAME", "OBSERVATION_S_TABLE_NAME", _
             "OBSERVATION_V_TABLE_NAME", "OBSERVATION_E_TABLE_NAME"
          If Len(lSeries.SeriesType) > 0 Then 'old series exists, save it to collection
            pSeries.Add lSeries
            Set lSeries = New clsPestSeries
          End If
          Select Case kwd
            Case "OBSERVATION_SERIES_NAME": lSeries.SeriesType = "t"
            Case "OBSERVATION_S_TABLE_NAME": lSeries.SeriesType = "s"
            Case "OBSERVATION_V_TABLE_NAME": lSeries.SeriesType = "v"
            Case "OBSERVATION_E_TABLE_NAME": lSeries.SeriesType = "e"
          End Select
          lSeries.ObsSeries = spec
          If Len(com) > 0 Then lSeries.ObsSeriesCom = com
        Case "MODEL_SERIES_NAME", "MODEL_S_TABLE_NAME", _
             "MODEL_V_TABLE_NAME", "MODEL_E_TABLE_NAME"
          lSeries.ModelSeries = spec
          If Len(com) > 0 Then lSeries.ModelSeriesCom = com
        Case "SERIES_WEIGHTS_EQUATION", "S_TABLE_WEIGHTS_EQUATION", _
             "V_TABLE_WEIGHTS_EQUATION", "E_TABLE_WEIGHTS_EQUATION"
          lSeries.WeightEquation = spec
          If Len(com) > 0 Then lSeries.WeightEquationCom = com
        Case "SERIES_WEIGHTS_MIN_MAX", "S_TABLE_WEIGHTS_MIN_MAX", _
             "V_TABLE_WEIGHTS_MIN_MAX", "E_TABLE_WEIGHTS_MIN_MAX"
          lSeries.WeightMin = CSng(Trim(StrRetRem(spec)))
          lSeries.WeightMax = CSng(Trim(spec))
          If Len(com) > 0 Then lSeries.WeightMinMaxCom = com
        Case "NEW_PEST_CONTROL_FILE"
          pControlFile = spec
          If Len(com) > 0 Then pControlFileCom = com
        Case "NEW_INSTRUCTION_FILE"
          pInstructFile = spec
          If Len(com) > 0 Then pInstructFileCom = com
        Case "MODEL_COMMAND_LINE"
          pModelCommandLine = spec
          If Len(com) > 0 Then pModelCommandLineCom = com
        Case "AUTOMATIC_USER_INTERVENTION"
          If LCase(spec) = "yes" Then
            pAUI = True
          ElseIf LCase(spec) = "no" Then
            pAUI = False
          End If
          If Len(com) > 0 Then pAUICom = com
      End Select
      com = "" 'reset comment string after processing a keyword
    End If
  Wend
  If Len(lSeries.SeriesType) > 0 Then 'save existing series to collection
    pSeries.Add lSeries
  End If
End Sub

Public Function WriteBlock() As String
  Dim str As String
  Dim lv As Variant

  If Len(pComments) > 0 Then
    str = pComments & vbCrLf & "START WRITE_PEST_FILES"
  Else
    str = "START WRITE_PEST_FILES"
  End If
  For Each lv In pContext
    If pContextCom.Count > 0 Then
      If pContextCom.KeyExists(lv) Then str = str & vbCrLf & pContextCom(lv)
    End If
    str = str & vbCrLf & "  CONTEXT " & lv
  Next
  For Each lv In pTemplateFile
    If pTemplateFileCom.Count > 0 Then
      If pTemplateFileCom.KeyExists(lv) Then str = str & vbCrLf & pTemplateFileCom(lv)
    End If
    str = str & vbCrLf & "  TEMPLATE_FILE " & lv
    If pModelInputFileCom.Count > 0 Then
      If pModelInputFileCom.KeyExists(lv) Then str = str & vbCrLf & pModelInputFileCom(lv)
    End If
    If pModelInputFile.Count > 0 Then
      If pModelInputFile.KeyExists(lv) Then str = str & vbCrLf & "  MODEL_INPUT_FILE " & pModelInputFile(lv)
    End If
  Next
  If Len(pParmDataFileCom) > 0 Then str = str & vbCrLf & pParmDataFileCom
  If Len(pParmDataFile) > 0 Then str = str & vbCrLf & "  PARAMETER_DATA_FILE " & pParmDataFile
  If Len(pParmGroupFileCom) > 0 Then str = str & vbCrLf & pParmGroupFileCom
  If Len(pParmGroupFile) > 0 Then str = str & vbCrLf & "  PARAMETER_GROUP_FILE " & pParmGroupFile
  If Len(pControlFileCom) > 0 Then str = str & vbCrLf & pControlFileCom
  str = str & vbCrLf & "  NEW_PEST_CONTROL_FILE " & pControlFile
  If Len(pInstructFileCom) > 0 Then str = str & vbCrLf & pInstructFileCom
  str = str & vbCrLf & "  NEW_INSTRUCTION_FILE " & pInstructFile
  If Len(pModelCommandLineCom) > 0 Then str = str & vbCrLf & pModelCommandLineCom
  If Len(pModelCommandLine) > 0 Then str = str & vbCrLf & "  MODEL_COMMAND_LINE " & pModelCommandLine
  If Len(pAUICom) > 0 Then str = str & vbCrLf & pAUICom
  If pAUI Then
    str = str & vbCrLf & "  AUTOMATIC_USER_INTERVENTION yes"
  Else
    str = str & vbCrLf & "  AUTOMATIC_USER_INTERVENTION no"
  End If
  For Each lv In pSeries
    If Len(lv.ObsSeriesCom) > 0 Then str = str & vbCrLf & lv.ObsSeriesCom
    Select Case lv.SeriesType
      Case "t": str = str & vbCrLf & "  OBSERVATION_SERIES_NAME " & lv.ObsSeries
      Case "s": str = str & vbCrLf & "  OBSERVATION_S_TABLE_NAME " & lv.ObsSeries
      Case "v": str = str & vbCrLf & "  OBSERVATION_V_TABLE_NAME " & lv.ObsSeries
      Case "e": str = str & vbCrLf & "  OBSERVATION_E_TABLE_NAME " & lv.ObsSeries
    End Select
    If Len(lv.ModelSeriesCom) > 0 Then str = str & vbCrLf & lv.ModelSeriesCom
    Select Case lv.SeriesType
      Case "t": str = str & vbCrLf & "  MODEL_SERIES_NAME " & lv.ModelSeries
      Case "s": str = str & vbCrLf & "  MODEL_S_TABLE_NAME " & lv.ModelSeries
      Case "v": str = str & vbCrLf & "  MODEL_V_TABLE_NAME " & lv.ModelSeries
      Case "e": str = str & vbCrLf & "  MODEL_E_TABLE_NAME " & lv.ModelSeries
    End Select
    If Len(lv.WeightEquationCom) > 0 Then str = str & vbCrLf & lv.WeightEquationCom
    Select Case lv.SeriesType
      Case "t": str = str & vbCrLf & "  SERIES_WEIGHTS_EQUATION " & lv.WeightEquation
      Case "s": str = str & vbCrLf & "  S_TABLE_WEIGHTS_EQUATION " & lv.WeightEquation
      Case "v": str = str & vbCrLf & "  V_TABLE_WEIGHTS_EQUATION " & lv.WeightEquation
      Case "e": str = str & vbCrLf & "  E_TABLE_WEIGHTS_EQUATION " & lv.WeightEquation
    End Select
    If Len(lv.WeightMinMaxCom) > 0 Then str = str & vbCrLf & lv.WeightMinMaxCom
    If lv.WeightMin > 0 And lv.WeightMax > 0 Then
      Select Case lv.SeriesType
        Case "t": str = str & vbCrLf & "  SERIES_WEIGHTS_MIN_MAX " & lv.WeightMin & "  " & lv.WeightMax
        Case "s": str = str & vbCrLf & "  S_TABLE_WEIGHTS_MIN_MAX " & lv.WeightMin & "  " & lv.WeightMax
        Case "v": str = str & vbCrLf & "  V_TABLE_WEIGHTS_MIN_MAX " & lv.WeightMin & "  " & lv.WeightMax
        Case "e": str = str & vbCrLf & "  E_TABLE_WEIGHTS_MIN_MAX " & lv.WeightMin & "  " & lv.WeightMax
      End Select
    End If
  Next
  str = str & vbCrLf & "END WRITE_PEST_FILES"
  WriteBlock = str

End Function

Private Sub Class_Initialize()
  pComments = ""
  Set pContext = New FastCollection
  Set pContextCom = New FastCollection
  Set pTemplateFile = New FastCollection
  Set pTemplateFileCom = New FastCollection
  Set pModelInputFile = New FastCollection
  Set pModelInputFileCom = New FastCollection
  pParmDataFile = ""
  pParmDataFileCom = ""
  pParmGroupFile = ""
  pParmGroupFileCom = ""
  Set pSeries = New FastCollection
  pControlFile = ""
  pControlFileCom = ""
  pInstructFile = ""
  pInstructFileCom = ""
  pModelCommandLine = ""
  pModelCommandLineCom = ""
  pAUI = False
End Sub
