VERSION 5.00
Begin VB.Form frmNewPoint 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "WinHSPF - Add Point Source"
   ClientHeight    =   2064
   ClientLeft      =   36
   ClientTop       =   276
   ClientWidth     =   3744
   Icon            =   "frmNewPoint.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2064
   ScaleWidth      =   3744
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdNew 
      Caption         =   "Advanced Generation"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   3
      Left            =   120
      TabIndex        =   3
      Top             =   1560
      Width           =   3492
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "Convert All MUTSINs in Project"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   2
      Left            =   120
      TabIndex        =   2
      Top             =   1080
      Width           =   3492
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "Import MUTSIN Format"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   1
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   3492
   End
   Begin VB.CommandButton cmdNew 
      Caption         =   "Simple Create"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   372
      Index           =   0
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   3492
   End
End
Attribute VB_Name = "frmNewPoint"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim lts As Collection

Public Sub Init(tempts As Collection)
  Set lts = tempts
End Sub

Private Sub cmdNew_Click(Index As Integer)
  Dim myFiles As hspffilesblk, newTS As Collection
  Dim k&, s$, mutcnt&, fu&, i&, retc&
  Dim tempts As Collection, cfiles As Collection
  Dim sen$, loc$, con$, stanam$, longloc$, rchid&
  Dim ifound As Boolean, newdsn&, newwdmid$
  Dim loper As HspfOperation, Filename$
  Dim tsnew As frmTSnew
  Dim Desc As String
  
  Unload Me
  If Index = 0 Then 'create new
    frmAddPoint.Show vbModal
    'Call FilterList
  ElseIf Index = 1 Then 'import
    frmImportPoint.Show vbModal
    'Call FilterList
  ElseIf Index = 2 Then 'convert all mutsin
    Set myFiles = myUci.filesblock
    mutcnt = 0
    k = 1
    Do While k <= myFiles.Count
      s = myFiles.Value(k).Name
      fu = myFiles.Value(k).Unit
      If UCase(Right(s, 3)) = "MUT" Then  'this is mutsin file
        mutcnt = mutcnt + 1
        Call ConvertMutsin(s, fu, 1, retc)
        If retc = 0 Then
          'now remove file
          myFiles.Remove (k)
        Else
          k = k + 1
        End If
      Else
        k = k + 1
      End If
    Loop
    If mutcnt = 0 Then 'no mutsin files found
      MsgBox "No Mutsin files found in this project.", vbOKOnly, "Convert Point Sources Problem"
    End If
    'Call FilterList
  ElseIf Index = 3 Then  'gener
    Set tsnew = New frmTSnew
    Set tempts = New Collection
    For i = 1 To lts.Count
      If Mid(lts(i).Header.sen, 1, 3) = "PT-" Then
        tempts.Add lts(i)
      End If
    Next
    Set tsnew.AllTSer = tempts
    Set cfiles = New Collection
    Dim tfile As ATCclsTserFile
    For i = 1 To 4
      Set tfile = myUci.GetWDMObj(i)
      If Not tfile Is Nothing Then
        cfiles.Add tfile
      End If
    Next i
    Set tsnew.OpenFiles = cfiles
    
    tsnew.Show vbModal
    
    Call myUci.FindTimSer("", "", "", newTS)
    For i = lts.Count + 1 To newTS.Count
      'these must have been added
      sen = newTS(i).Header.sen
      stanam = newTS(i).Header.Desc
      loc = newTS(i).Header.loc
      con = newTS(i).Header.con
      newdsn = newTS(i).Header.Id
      rchid = CInt(Mid(loc, 4))
      Set loper = myUci.OpnBlks("RCHRES").operfromid(rchid)
      If Not loper Is Nothing Then
        Desc = loper.Description
      End If
      longloc = "RCHRES " & rchid & " - " & Desc
      Filename = newTS(i).File.Filename
      newwdmid = myUci.GetWDMIdFromName(Filename)
      frmPoint.UpdateListsForNewPointSource sen, stanam, loc, con, newwdmid, _
        newdsn, "RCHRES", rchid, longloc
    Next i
    
  End If
End Sub
