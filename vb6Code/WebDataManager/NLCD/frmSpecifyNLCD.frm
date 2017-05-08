VERSION 5.00
Begin VB.Form frmSpecifyNLCD 
   Caption         =   "Specify NLCD to download"
   ClientHeight    =   7950
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5460
   Icon            =   "frmSpecifyNLCD.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   7950
   ScaleWidth      =   5460
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdWebSite 
      Caption         =   "Visit NLCD web site"
      Height          =   495
      Left            =   2880
      TabIndex        =   9
      Top             =   3240
      Width           =   2415
   End
   Begin VB.CheckBox chkFormat 
      Caption         =   "TIFF"
      Height          =   255
      Index           =   0
      Left            =   3120
      TabIndex        =   6
      Top             =   5400
      Value           =   1  'Checked
      Visible         =   0   'False
      Width           =   975
   End
   Begin VB.CheckBox chkFormat 
      Caption         =   "Binary"
      Height          =   255
      Index           =   1
      Left            =   3120
      TabIndex        =   5
      Top             =   5640
      Visible         =   0   'False
      Width           =   975
   End
   Begin VB.Frame fraButtons 
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      Height          =   375
      Left            =   2880
      TabIndex        =   2
      Top             =   7440
      Width           =   2415
      Begin VB.CommandButton cmdCancel 
         Cancel          =   -1  'True
         Caption         =   "Cancel"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   1320
         TabIndex        =   4
         Top             =   0
         Width           =   1095
      End
      Begin VB.CommandButton cmdOk 
         Caption         =   "&Ok"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   0
         TabIndex        =   3
         Top             =   0
         Width           =   1095
      End
   End
   Begin VB.ListBox lstStateName 
      Height          =   7260
      Left            =   120
      Style           =   1  'Checkbox
      TabIndex        =   0
      Top             =   600
      Width           =   2535
   End
   Begin VB.Label Label4 
      Caption         =   "For More Information"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   10
      Top             =   2760
      Width           =   2175
   End
   Begin VB.Label Label3 
      Caption         =   $"frmSpecifyNLCD.frx":08CA
      Height          =   1575
      Left            =   3000
      TabIndex        =   8
      Top             =   720
      Width           =   2295
   End
   Begin VB.Label Label2 
      Caption         =   "Description"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   2880
      TabIndex        =   7
      Top             =   240
      Width           =   2415
   End
   Begin VB.Label Label1 
      Caption         =   "Regions to download"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   240
      Width           =   2535
   End
End
Attribute VB_Name = "frmSpecifyNLCD"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim pManager As clsWebDataManager
Dim pQuery As ChilkatXml

Public Function AskUser(manager As clsWebDataManager) As ChilkatXml
  
  InitializeForm manager
  
  Me.Show
  While pQuery Is Nothing
    DoEvents
  Wend
  Set AskUser = pQuery
End Function

Public Function GetDefault(manager As clsWebDataManager) As ChilkatXml
  InitializeForm manager
  cmdOk_Click
  Set GetDefault = pQuery
End Function

Private Sub InitializeForm(manager As clsWebDataManager)
  Dim iNode As Long
  Dim NLCDname As String
  Dim lNode As ChilkatXml
  Dim lMatchNodeList As FastCollection
  Dim selectedStatesList As FastCollection
  Dim selectedStates As String
  
  Set pManager = manager
  
  Set selectedStatesList = FindNodeValues("state_abbrev")
  For iNode = 1 To selectedStatesList.Count
    selectedStates = selectedStates & LCase(Abbrev2Name(selectedStatesList.ItemByIndex(iNode).Content)) & ","
  Next
  selectedStates = ReplaceString(selectedStates, " ", "_")

  Set lMatchNodeList = manager.CurrentStatusGetList("state_nlcd")
  For iNode = 1 To lMatchNodeList.Count
    Set lNode = lMatchNodeList.ItemByIndex(iNode)
    NLCDname = lNode.Content
    lstStateName.AddItem NLCDname
    If InStr(selectedStates, NLCDname) > 0 Then
      lstStateName.Selected(iNode - 1) = True
    ElseIf Left(NLCDname, 10) = "california" Then
      If InStr(selectedStates, "california") > 0 Then lstStateName.Selected(iNode - 1) = True
    ElseIf NLCDname = "massachusettes" Then
      If InStr(selectedStates, "massachusetts") > 0 Then lstStateName.Selected(iNode - 1) = True
    ElseIf Left(NLCDname, 5) = "texas" Then
      If InStr(selectedStates, "texas") > 0 Then lstStateName.Selected(iNode - 1) = True
    End If
  Next
  Set pQuery = Nothing

End Sub

Private Sub cmdCancel_Click()
  pManager.DownloadStatus = "Cancelled"
  pManager.State = -999
  Set pQuery = New ChilkatXml
  Unload Me
End Sub

Private Sub cmdOk_Click()
  If lstStateName.SelCount = 0 Then
    pManager.LogMsg "Select a region to download", "Specify NLCD"
  ElseIf chkFormat(0).Value = vbUnchecked And chkFormat(1).Value = vbUnchecked Then
    pManager.LogMsg "Select TIFF or Binary format", "Specify NLCD"
  Else
    Dim xml As String
    Dim iState As Long
    Dim lQuery As ChilkatXml
    Set lQuery = New ChilkatXml
    xml = "<clsNLCD><criteria>"
    For iState = 0 To lstStateName.ListCount - 1
      If lstStateName.Selected(iState) Then
        xml = xml & "<state_nlcd>" & lstStateName.List(iState) & "</state_nlcd>"
      End If
    Next
    If chkFormat(0).Value = vbChecked Then xml = xml & "<tif_nlcd>True</tif_nlcd>"
    If chkFormat(1).Value = vbChecked Then xml = xml & "<bin_nlcd>True</bin_nlcd>"
    xml = xml & "<save_dir>" & pManager.CurrentStatusGetString("save_dir") & "</save_dir>"
    xml = xml & "</criteria></clsNLCD>"
    lQuery.LoadXml xml
    Set pQuery = lQuery
    Me.Hide
  End If
End Sub

Private Sub cmdWebSite_Click()
  OpenFile pManager.CurrentStatusGetString("NLCDbaseURL", _
           "http://edcftp.cr.usgs.gov/pub/data/landcover/states/")
End Sub

Private Sub Form_Resize()
  fraButtons.Top = ScaleHeight - 510
  If fraButtons.Top > lstStateName.Top Then
    lstStateName.Height = fraButtons.Top + fraButtons.Height - lstStateName.Top
  End If
End Sub

Private Function FindNodeValues(nodeName As String) As FastCollection
  Dim lQuery As ChilkatXml
  Dim lResult As ChilkatXml
  Dim lStatus As Boolean
  Dim lProvider As clsWebData
  Dim providers As FastCollection
  Dim providerIndex As Long
  Dim lMatchNodeList As FastCollection
  Dim dbgStep As String
  
  On Error GoTo ErrHand
  
  dbgStep = "Set lMatchNodeList"
  Set lMatchNodeList = pManager.CurrentStatusGetList(nodeName)
  dbgStep = "Set FindNodeValues"
  Set FindNodeValues = lMatchNodeList
  dbgStep = "If lMatchNodeList.Length = 0"
  If lMatchNodeList.Count = 0 Then 'no current value, look for one
    dbgStep = "Set lResult"
    Set lResult = New ChilkatXml
    dbgStep = "Set providers"
    Set providers = pManager.Provides(nodeName)
    dbgStep = "For providerIndex"
    For providerIndex = 1 To providers.Count
      Debug.Print providers.ItemByIndex(providerIndex) & " could provide " & nodeName
      dbgStep = "Set lProvider"
      Set lProvider = pManager.DataTypeFromLabel(providers.ItemByIndex(providerIndex))
      dbgStep = "CurrentStatusUpdateString"
      pManager.CurrentStatusUpdateString nodeName, "", "requested from " & lProvider.Name
      dbgStep = "BuildQueryFromStatus"
      Set lQuery = New ChilkatXml
      dbgStep = "lQuery.loadXML"
      lQuery.LoadXml "<clsBasinsPrj><criteria><project_dir>" _
        & pManager.CurrentStatusGetString("project_dir") _
        & "</project_dir></criteria><requested><dbf_st><state_abbrev/></dbf_st></requested></clsBasinsPrj>"
      If lQuery Is Nothing Then GoTo NextProvider 'Not all required criteria specified
      
      dbgStep = "lStatus = lProvider.GetData"
      lStatus = lProvider.GetData(lQuery, lResult)
      If lStatus Then
        dbgStep = "CurrentStatusUpdateList"
        pManager.CurrentStatusUpdateList nodeName, _
                                         GetChildrenWithTag(lResult, nodeName), _
                                         "set by " & providers(providerIndex)
        dbgStep = "Set FindNodeValues"
        Set FindNodeValues = pManager.CurrentStatusGetList(nodeName)
        dbgStep = "FindNodeValues exit "
        Exit Function
      End If
NextProvider:
    Next
  End If
  dbgStep = "FindNodeValues end "
  Exit Function
ErrHand:
  pManager.LogMsg Err.Description & vbCr & " at step " & dbgStep, _
                         "Wizard FindNodeValues " & nodeName
End Function

