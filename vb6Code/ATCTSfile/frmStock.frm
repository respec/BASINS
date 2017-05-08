VERSION 5.00
Begin VB.Form frmStock 
   Caption         =   "Stock"
   ClientHeight    =   7944
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   10320
   LinkTopic       =   "Form1"
   ScaleHeight     =   7944
   ScaleWidth      =   10320
   StartUpPosition =   3  'Windows Default
   Begin InetCtlsObjects.Inet net 
      Left            =   360
      Top             =   1440
      _ExtentX        =   804
      _ExtentY        =   804
      _Version        =   393216
   End
   Begin VB.TextBox txtResults 
      Height          =   5892
      Left            =   120
      MultiLine       =   -1  'True
      TabIndex        =   6
      Top             =   1920
      Width           =   10092
   End
   Begin VB.CommandButton cmdRetrieve 
      Caption         =   "Retrieve"
      Height          =   372
      Left            =   960
      TabIndex        =   5
      Top             =   1440
      Width           =   1212
   End
   Begin VB.TextBox txtSymbol 
      Height          =   288
      Left            =   960
      TabIndex        =   3
      Top             =   480
      Width           =   1092
   End
   Begin VB.TextBox txtURL 
      Height          =   288
      Left            =   960
      TabIndex        =   1
      Top             =   80
      Width           =   9252
   End
   Begin ATCoCtl.ATCoDate dat 
      Height          =   1356
      Left            =   2280
      TabIndex        =   0
      Top             =   480
      Width           =   5172
      _ExtentX        =   9123
      _ExtentY        =   2392
      TUnit           =   4
      TAggr           =   1
      TStep           =   1
      CurrE           =   35399
      CurrS           =   35399
      LimtE           =   73051
      LimtS           =   2
      DispL           =   4
      LabelCurrentRange=   "To Graph"
      LabelMaxRange   =   "Available"
      TstepVisible    =   -1  'True
   End
   Begin VB.Label Label2 
      Caption         =   "Symbol"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   4
      Top             =   480
      Width           =   732
   End
   Begin VB.Label Label1 
      Caption         =   "URL"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   120
      Width           =   732
   End
End
Attribute VB_Name = "frmStock"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants

Private curPage As String
Private pTSFile As ATCclsTserFile

Public Property Set TserFileToAddTo(newvalue As ATCclsTserFile)
  Set pTSFile = newvalue
End Property

Public Sub Retrieve()
  curPage = ""
  txtResults = "Loading..."
  txtResults.BackColor = vbButtonFace
  net.Execute txtURL.Text, "GET"
End Sub

Private Sub cmdRetrieve_Click()
  Retrieve
End Sub

Private Sub dat_Change()
  buildURL
End Sub

Private Sub Form_Load()
  dat.LimtE = Now
  dat.Curre = dat.LimtE
  dat.CurrS = dat.LimtE
End Sub

Private Sub net_StateChanged(ByVal State As Integer)
  Select Case State
    Case icNone
      Caption = ""
    Case icResolvingHost
      Caption = "HostResolvingHost"
    Case icHostResolved
      Caption = "HostResolved"
    Case icConnecting
      Caption = "Connecting"
    Case icConnected
      Caption = "Connected"
    Case icRequesting
      Caption = "Requesting"
    Case icRequestSent
      Caption = "RequestSent"
    Case icReceivingResponse
      Caption = "ReceivingResponse"
'      timLoading.Enabled = True
    Case icResponseReceived
      Caption = "ResponseReceived"
    Case icDisconnecting
      Caption = "Disconnecting"
    Case icDisconnected
      Caption = "Disconnected"
    Case icError
      Caption = "Error: " & net.ResponseInfo
    Case icResponseCompleted
'      timLoading.Enabled = False
      Caption = "ResponseCompleted"
      GetChunks
      If pTSFile Is Nothing Then
        MsgBox "TserFile not set"
      Else
        pTSFile.filename = txtURL
      End If
      txtResults.BackColor = vbWindowBackground
  End Select
End Sub

Public Property Let URL(newvalue As String)
  Dim beginPos&, endPos&
  txtURL.Text = newvalue
  If Left(newvalue, 7) = "http://" Then 'cmdRetrieve_Click
    beginPos = InStr(newvalue, "s=")
    If beginPos > 0 Then txtSymbol = URLpiece(newvalue, beginPos + 2)
  End If
End Property

Private Function URLpiece(str$, beginPos&) As String
  Dim endPos&, LenStr&
  endPos = beginPos
  LenStr = Len(str)
  While endPos < LenStr
    If Mid(str, endPos, 1) = "&" Then GoTo FoundEnd
    endPos = endPos + 1
  Wend
FoundEnd:
  URLpiece = Mid(str, beginPos, endPos - beginPos)
End Function

Private Sub buildURL()
  Dim tunit As String
  Select Case dat.tunit
    Case 7: tunit = "v"
    Case 5: tunit = "m"
    Case 3: tunit = "w"
    Case Else: tunit = "d"
  End Select
  txtURL.Text = "http://chart.yahoo.com/table.csv?" & _
  "s=" & txtSymbol & _
  "&a=" & Month(dat.CurrS) & _
  "&b=" & Day(dat.CurrS) & _
  "&c=" & Year(dat.CurrS) & _
  "&d=" & Month(dat.Curre) & _
  "&e=" & Day(dat.Curre) & _
  "&f=" & Year(dat.Curre) & _
  "&g=" & tunit & _
  "&q=q" & _
  "&y=0" & _
  "&z=" & txtSymbol & _
  "&x=.csv"

End Sub

Private Sub GetChunks()
  Static locked As Boolean
  Dim chunk$
  Dim done As Boolean
'  Dim restartTimer As Boolean
  If Not locked Then
    locked = True
'    restartTimer = timLoading.Enabled
'    timLoading.Enabled = False
    done = False
    chunk = ""
    While Not done
'      restartTimer = restartTimer And net.StillExecuting
      chunk = net.GetChunk(1024, icString)
      If Len(chunk) = 0 Then
        done = True
      Else
        curPage = curPage & chunk
      End If
      DoEvents
      'If ChangeWaiting Then GoTo Abort
    Wend
    txtResults = curPage
'    timLoading.Enabled = restartTimer
    locked = False
  End If
End Sub

Private Sub txtSymbol_KeyPress(KeyAscii As Integer)
  buildURL
End Sub
