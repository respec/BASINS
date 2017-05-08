VERSION 5.00
Begin VB.Form frmOptions 
   Caption         =   "Options"
   ClientHeight    =   4185
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4575
   LinkTopic       =   "Form1"
   ScaleHeight     =   4185
   ScaleWidth      =   4575
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdOk 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
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
      Index           =   1
      Left            =   600
      TabIndex        =   9
      Top             =   3600
      Width           =   1455
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "&Ok"
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
      Index           =   0
      Left            =   2520
      TabIndex        =   8
      Top             =   3600
      Width           =   1455
   End
   Begin VB.Frame Frame1 
      Caption         =   "File Types Displayed"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   3135
      Left            =   240
      TabIndex        =   0
      Top             =   240
      Width           =   4095
      Begin VB.CheckBox chkFileType 
         Caption         =   "&All files (*.*)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   0
         Left            =   360
         TabIndex        =   7
         Tag             =   "*"
         Top             =   360
         Width           =   3255
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "Relational &Database (*.rdb)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   6
         Left            =   360
         TabIndex        =   6
         Tag             =   "rdb"
         Top             =   2520
         Width           =   3255
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "&Rich Text files (*.rtf)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   5
         Left            =   360
         TabIndex        =   5
         Tag             =   "rtf"
         Top             =   2160
         Width           =   3255
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "&Text files (*.txt)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   4
         Left            =   360
         TabIndex        =   4
         Tag             =   "txt"
         Top             =   1800
         Width           =   3255
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "&Watershed Data Management (*.wdm)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   3
         Left            =   360
         TabIndex        =   3
         Tag             =   "wdm"
         Top             =   1440
         Value           =   1  'Checked
         Width           =   3615
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "HSPF &User Control Input (*.uci)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   2
         Left            =   360
         TabIndex        =   2
         Tag             =   "uci"
         Top             =   1080
         Value           =   1  'Checked
         Width           =   3255
      End
      Begin VB.CheckBox chkFileType 
         Caption         =   "ESRI &Shape files (*.shp)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   350
         Index           =   1
         Left            =   360
         TabIndex        =   1
         Tag             =   "shp"
         Top             =   720
         Value           =   1  'Checked
         Width           =   3255
      End
   End
End
Attribute VB_Name = "frmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const lastFileType = 6

Private Sub chkFileType_Click(Index As Integer)
  If Index = 0 Then
    Dim check As Boolean, typeIndex&
    If chkFileType(Index).Value = 1 Then check = True Else check = False
    For typeIndex = 1 To lastFileType
      If check Then chkFileType(typeIndex).Value = 1
      chkFileType(typeIndex).Enabled = Not check
    Next typeIndex
  End If
End Sub

Private Sub cmdOk_Click(Index As Integer)
  If Index = 0 Then
    If chkFileType(0).Value = 1 Then
      ReDim dispTypes(0)
      dispTypes(0) = "*"
    Else
      Dim typeIndex&, dispTypeIndex&
      dispTypeIndex = 0
      For typeIndex = 1 To lastFileType
        If chkFileType(typeIndex).Value = 1 Then dispTypeIndex = dispTypeIndex + 1
      Next typeIndex
      If dispTypeIndex = 0 Then
        MsgBox "No file types were selected for display." & vbCrLf & "Previous filter will be used."
      Else
        ReDim dispTypes(dispTypeIndex - 1)
        dispTypeIndex = 0
        For typeIndex = 1 To lastFileType
          If chkFileType(typeIndex).Value = 1 Then
            dispTypes(dispTypeIndex) = chkFileType(typeIndex).Tag
            dispTypeIndex = dispTypeIndex + 1
          End If
        Next typeIndex
      End If
    End If
    
    'Re-read directory with new settings. (Ugly non-OO hack)
    With frmMain.treeDirectory
      If .Nodes.count > 0 Then
        Call OpenDirectory(.Nodes(1).Text, frmMain.treeDirectory)
      Else
        frmMain.OpenDir
      End If
    End With
    
  End If
  Unload Me
End Sub

Private Sub Form_Load()
  Dim typeIndex&, dispTypeIndex&
  For typeIndex = 0 To lastFileType
    chkFileType(typeIndex).Value = 0
  Next typeIndex
  For dispTypeIndex = LBound(dispTypes) To UBound(dispTypes)
    For typeIndex = 0 To lastFileType
      If chkFileType(typeIndex).Tag = dispTypes(dispTypeIndex) Then
        chkFileType(typeIndex).Value = 1
      End If
    Next typeIndex
  Next dispTypeIndex
End Sub
