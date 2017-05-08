VERSION 5.00
Begin VB.Form frmGetDataFromWeb 
   Caption         =   "Get Data From Web"
   ClientHeight    =   1776
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   8736
   LinkTopic       =   "Form1"
   ScaleHeight     =   1776
   ScaleWidth      =   8736
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   372
      Left            =   2400
      TabIndex        =   5
      Top             =   1200
      Width           =   1212
   End
   Begin VB.CommandButton cmdOk 
      Caption         =   "&Ok"
      Height          =   372
      Left            =   960
      TabIndex        =   4
      Top             =   1200
      Width           =   1212
   End
   Begin VB.TextBox txtFile 
      Height          =   288
      Left            =   960
      TabIndex        =   3
      Text            =   "c:\temp\webdata.txt"
      Top             =   720
      Width           =   7452
   End
   Begin VB.TextBox txtURL 
      Height          =   288
      Left            =   960
      TabIndex        =   1
      Top             =   240
      Width           =   7452
   End
   Begin InetCtlsObjects.Inet TP 
      Left            =   0
      Top             =   120
      _ExtentX        =   804
      _ExtentY        =   804
      _Version        =   393216
   End
   Begin MSComDlg.CommonDialog CDFile 
      Left            =   0
      Top             =   600
      _ExtentX        =   699
      _ExtentY        =   699
      _Version        =   393216
      FontSize        =   4.09255e-38
   End
   Begin VB.Label lblStatus 
      Height          =   252
      Left            =   3720
      TabIndex        =   6
      Top             =   1200
      Width           =   4692
   End
   Begin VB.Label lblFile 
      Alignment       =   1  'Right Justify
      Caption         =   "Save &As"
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   720
      Width           =   732
   End
   Begin VB.Label lblURL 
      Alignment       =   1  'Right Justify
      Caption         =   "&URL"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   732
   End
End
Attribute VB_Name = "frmGetDataFromWeb"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Dim pFilename As String

Public Function GetFileName() As String
  pFilename = ""
  Me.Show vbModal
  While pFilename = ""
    DoEvents
  Wend
  GetFileName = pFilename
  'Me.Hide
  Unload Me
End Function

'Private Sub cmdBrowse_Click(Index As Integer)
'  Dim SitesFoundHTML As String
'  Select Case Index
'    Case 0
'      SitesFoundHTML = TP.OpenURL("http://water.usgs.gov/ga/nwis/discharge?huc_cd=03070103&format=station_list&sort_key=site_no&group_key=NONE&sitefile_output_format=html_table&column_name=agency_cd&column_name=site_no&column_name=station_nm&column_name=lat_va&column_name=long_va&column_name=state_cd&column_name=county_cd&column_name=alt_va&column_name=huc_cd&list_of_search_criteria=huc_cd")
'      MsgBox SitesFoundHTML, vbOKOnly, "Not yet implemented"
'    Case 1
'
'  End Select
'End Sub

Private Sub cmdCancel_Click()
  pFilename = "Error"
  DoEvents
  Unload Me
End Sub

Private Sub cmdOk_Click()
  Dim buf() As Byte
  On Error GoTo errHand
  Me.MousePointer = vbHourglass
  buf = TP.OpenURL(txtURL.text, icByteArray)
  SaveFileBytes txtFile.text, buf
  pFilename = txtFile.text
  Me.MousePointer = vbDefault
  Unload Me
  Exit Sub
errHand:
  MsgBox "Error getting data" & vbCr _
       & "URL: " & txtURL.text & vbCr _
       & "File: " & txtFile.text & vbCr _
       & err.Description, vbCritical, "Get Data From Web"
  pFilename = "Error"
End Sub

Private Sub TP_StateChanged(ByVal state As Integer)
  Select Case state
    Case icResolvingHost: lblStatus = "Resolving Host"
    Case icHostResolved:      lblStatus = "Host Resolved"
    Case icConnecting:        lblStatus = "Connecting"
    Case icConnected:         lblStatus = "Connected"
    Case icRequesting:        lblStatus = "Requesting"
    Case icRequestSent:       lblStatus = "Request Sent"
    Case icReceivingResponse: lblStatus = "Receiving Response"
    Case icResponseReceived:  lblStatus = "Response Received"
    Case icDisconnecting:     lblStatus = "Disconnecting"
    Case icDisconnected:      lblStatus = "Disconnected"
    Case icError: lblStatus = TP.ResponseCode & ":" & TP.ResponseInfo
    Case icResponseCompleted: lblStatus = "Response Completed"
  End Select
End Sub
