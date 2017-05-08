VERSION 5.00
Begin VB.Form frmUpdates 
   Caption         =   "Check for Software Updates"
   ClientHeight    =   2040
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8070
   Icon            =   "frmUpdates.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   2040
   ScaleWidth      =   8070
   StartUpPosition =   3  'Windows Default
   Begin VB.CheckBox chkAutoCheckForUpdates 
      Caption         =   "Check Automatically on first download each day"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   2280
      TabIndex        =   4
      Top             =   840
      Width           =   5655
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "Check Now"
      Default         =   -1  'True
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
      Left            =   240
      TabIndex        =   3
      Top             =   1440
      Width           =   1695
   End
   Begin VB.CommandButton cmdReset 
      Caption         =   "Reset to Default Server"
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
      Left            =   2280
      TabIndex        =   2
      Top             =   1440
      Width           =   2655
   End
   Begin VB.TextBox txtURL 
      Height          =   285
      Left            =   2280
      TabIndex        =   1
      Top             =   240
      Width           =   5655
   End
   Begin VB.Label lblLastCheck 
      Caption         =   "Last Checked: "
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   5160
      TabIndex        =   5
      Top             =   1500
      Width           =   2775
   End
   Begin VB.Label Label1 
      Caption         =   "Update Server URL:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   240
      TabIndex        =   0
      Top             =   260
      Width           =   1935
   End
End
Attribute VB_Name = "frmUpdates"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private pCheck As Boolean

'Dim pManager As clsWebDataManager

'Public Property Set Manager(newManager As clsWebDataManager)
'  Set pManager = newManager
'  pManager.State = 4 'state_update
'End Property

Private Sub chkAutoCheckForUpdates_Click()
  'Back door for clearing the "Update" "LastCheck" key
  'After clicking the checkbox 10 times, the automatic check for updates can run again today
  Static nChanges As Long
  nChanges = nChanges + 1
  If nChanges > 9 Then
    SaveSetting gAppName, "Update", "LastCheck", ""
    LogMsg "Automatic check for update can run again today", "Cleared " & gAppName & " Update LastCheck"
    nChanges = 0
  End If
End Sub

Private Sub cmdOk_Click()
  
  pCheck = True

  gUpdateURL = txtURL.Text
  'Save update URL if edited
  If gUpdateURL <> txtURL.Tag Then SaveSetting gAppName, "Update", "URL3", gUpdateURL
  Unload Me
End Sub

Private Sub cmdReset_Click()
  txtURL.Text = cDefaultServerURL
End Sub

Private Sub Form_Load()
  txtURL.Text = gUpdateURL 'GetSetting(gAppName, "Update", "URL3", cDefaultServerURL)
  txtURL.Tag = txtURL.Text
  
  lblLastCheck.Caption = "Last checked: " & GetSetting(gAppName, "Update", "LastCheck", "Never")
  'Get this user's preference for automatic checking for updates, default to yes
  chkAutoCheckForUpdates.Tag = GetSetting(gAppName, "Update", "Check", "yes")
  Select Case LCase(chkAutoCheckForUpdates.Tag)
    Case "yes", "true", "on", "1": chkAutoCheckForUpdates.Value = vbChecked
    Case Else: chkAutoCheckForUpdates = vbUnchecked
  End Select
End Sub

Private Sub Form_Resize()
  If Me.ScaleWidth > 2500 Then txtURL.Width = Me.ScaleWidth - 2415
End Sub

Private Sub Form_Unload(Cancel As Integer)
  
  'User cancelled by closing window, not clicking check for updates now
  If Not pCheck Then gCancelled = True
    
  Select Case LCase(chkAutoCheckForUpdates.Tag)
    Case "yes", "true" 'Setting when form was loaded said yes
      If chkAutoCheckForUpdates.Value = vbUnchecked Then
        SaveSetting gAppName, "Update", "Check", "no"
      End If
    Case Else 'Setting when form was loaded said no
      If chkAutoCheckForUpdates = vbChecked Then
        SaveSetting gAppName, "Update", "Check", "yes"
      End If
  End Select
End Sub
