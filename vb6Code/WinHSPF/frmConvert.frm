VERSION 5.00
Begin VB.Form frmConvert 
   Caption         =   "NPSM to WinHSPF Conversion"
   ClientHeight    =   3816
   ClientLeft      =   48
   ClientTop       =   276
   ClientWidth     =   3876
   Icon            =   "frmConvert.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3816
   ScaleWidth      =   3876
   StartUpPosition =   3  'Windows Default
   Begin VB.Frame fraWDM 
      Caption         =   "WDM Files"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   972
      Left            =   120
      TabIndex        =   6
      Top             =   120
      Width           =   3612
      Begin VB.CommandButton cmdAdd 
         Caption         =   "&Add"
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
         Left            =   2760
         TabIndex        =   8
         ToolTipText     =   "Add a Project WDM"
         Top             =   360
         Width           =   732
      End
      Begin VB.Label lblConvert 
         Caption         =   "Info"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   7.8
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   612
         Left            =   120
         TabIndex        =   7
         Top             =   240
         Width           =   2652
      End
   End
   Begin VB.CheckBox chkRemove 
      Caption         =   "Remove PLTGEN Specs"
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
      Left            =   840
      TabIndex        =   5
      Top             =   2880
      Width           =   2652
   End
   Begin VB.CheckBox chkOutput 
      Caption         =   "Timeseries Output from PLTGEN to WDM Format"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   120
      TabIndex        =   4
      Top             =   2280
      Width           =   3612
   End
   Begin VB.CheckBox chkPoint 
      Caption         =   "Point Source Inputs from MUTSIN to WDM Format"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   492
      Left            =   120
      TabIndex        =   3
      Top             =   1680
      Width           =   3612
   End
   Begin VB.CheckBox chkNetwork 
      Caption         =   "Network to Schematic/Mass-Link Blocks"
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
      Left            =   120
      TabIndex        =   2
      Top             =   1200
      Width           =   3612
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "&Cancel"
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
      Left            =   2040
      TabIndex        =   1
      Top             =   3360
      Width           =   972
   End
   Begin MSComDlg.CommonDialog cdFile 
      Left            =   3360
      Top             =   3120
      _ExtentX        =   677
      _ExtentY        =   677
      _Version        =   393216
   End
   Begin VB.CommandButton cmdConvert 
      Caption         =   "&OK"
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
      Left            =   840
      TabIndex        =   0
      Top             =   3360
      Width           =   972
   End
End
Attribute VB_Name = "frmConvert"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Dim schemfound As Boolean, netfound As Boolean
Dim myFiles As hspffilesblk, wdmcount&, mutcnt&, pltcnt&
Dim newwdmname$, createwdmflag&

Private Sub chkOutput_Click()
  If chkOutput.Value = 1 Then
    chkRemove.Value = 1
    chkRemove.Enabled = True
  Else
    chkRemove.Value = 0
    chkRemove.Enabled = False
  End If
End Sub

Private Sub cmdAdd_Click()
  
    'add a project wdm
    createwdmflag = -1
    cdFile.flags = &H8806&
    cdFile.Filter = "WDM files (*.wdm)|*.wdm"
    cdFile.Filename = "*.wdm"
    cdFile.DialogTitle = "Select Project WDM File"
    On Error GoTo 40
    cdFile.CancelError = True
    cdFile.Action = 1
    'does wdm exist?
    On Error GoTo 20
    Open cdFile.Filename For Input As #1
    'yes, it exists
    Close #1
    createwdmflag = 0
    GoTo 30
20    'no, it does not exist, create it
    createwdmflag = 2
30  newwdmname = cdFile.Filename
40    'continue here on cancel
    DisplayWDMInfo
End Sub

Private Sub cmdConvert_Click()
  Dim fun&, wid$, k&, s$, fu&, i&, opname$
  Dim myFiles As hspffilesblk, vConn As Variant
  Dim vOper As Variant
  Dim loper As HspfOperation, lConn As HspfConnection
  Dim vCopyConn As Variant, lCopyConn As HspfConnection
  Dim cloc$, newdsn&, Id&, retc&
  Dim ConvertNetwork As Boolean, ConvertPoints As Boolean
  Dim ConvertOutput As Boolean, RemovePLTGEN As Boolean
  
  Me.MousePointer = vbHourglass
    If chkNetwork = 1 Then
      ConvertNetwork = True
    Else
      ConvertNetwork = False
    End If
    If chkPoint = 1 Then
      ConvertPoints = True
    Else
      ConvertPoints = False
    End If
    If chkOutput = 1 Then
      ConvertOutput = True
      If chkRemove = 1 Then
        RemovePLTGEN = True
      Else
        RemovePLTGEN = False
      End If
    Else
      ConvertOutput = False
      RemovePLTGEN = False
    End If
  
    If createwdmflag >= 0 Then
      'open the new file
      wid = "WDM" & CStr(myUci.wdmcount + 1)
      myUci.openWDM createwdmflag, newwdmname, fun, wid
      If fun < 1 Then
        'problem, cant do
        If createwdmflag = 2 Then
          Call MsgBox("Problem creating the project WDM file.", _
                        vbOKOnly, "Create WDM Problem")
        ElseIf createwdmflag = 0 Then
          Call MsgBox("Problem opening the project WDM file.", _
                        vbOKOnly, "Open WDM Problem")
        End If
      Else
        'add the project wdm to the filesblock as wdm1
        Set myFiles = myUci.filesblock
        For i = 1 To myFiles.Count
          If myFiles.Value(i).typ = "WDM1" Then
            myFiles.settyp i, wid
          End If
        Next i
        myFiles.AddFromSpecs newwdmname, "WDM1"
        'switch wdm1 and wdmx everywhere else in the uci
        For Each vConn In myUci.Connections
          Set lConn = vConn
          If lConn.Source.volname = "WDM1" Then
            lConn.Source.volname = wid
          End If
          If lConn.Target.volname = "WDM1" Then
            lConn.Target.volname = wid
          End If
        Next vConn
        'set files in new order in wdm objects array
        ChDriveDir PathNameOnly(myUci.Name)
        myUci.ClearWDM
        myUci.InitWDMArray
        myUci.SetWDMFiles
        myUci.Edited = True
      End If
    End If
    
    'convert network to schematic and mass-links
    If ConvertNetwork Then
      CheckSchematic schemfound, netfound
      If netfound And Not schemfound Then
        ConvertNetworkToSchematic
        myUci.Edited = True
        myUci.MaxAreaByLand2Stream = 0
      End If
    End If

    'convert point source inputs to wdm
    If ConvertPoints Then
      Set myFiles = myUci.filesblock
      k = 1
      Do While k <= myFiles.Count
        s = myFiles.Value(k).Name
        fu = myFiles.Value(k).Unit
        If UCase(Right(s, 3)) = "MUT" Then  'this is mutsin file
          Call ConvertMutsin(s, fu, 0, retc)
          If retc = 0 Then
            'now remove file
            myFiles.Remove (k)
            myUci.Edited = True
          Else
            k = k + 1
          End If
        Else
          k = k + 1
        End If
      Loop
    End If

    'convert outputs from pltgen to wdm
    If ConvertOutput Then
      For Each vOper In myUci.OpnBlks("PLTGEN").Ids
        Set loper = vOper
        If RemovePLTGEN Then
          fu = 0
          If loper.TableExists("PLOTINFO") Then
            fu = loper.tables("PLOTINFO").Parms("PLOTFL")
          End If
          If fu > 0 Then
            'remove from files block
            Set myFiles = myUci.filesblock
            k = 1
            Do While k <= myFiles.Count
              If fu = myFiles.Value(k).Unit Then
                'remove file
                myFiles.Remove (k)
                myUci.Edited = True
              Else
                k = k + 1
              End If
            Loop
          End If
        End If
        'build new outputs
        For Each vConn In loper.Sources
          Set lConn = vConn
          'this is a source to this pltgen
          If lConn.Source.volname = "COPY" Then
            'look for sources to this copy
            For Each vCopyConn In lConn.Source.Opn.Sources
              Set lCopyConn = vCopyConn
              'write out this source
              opname = lCopyConn.Source.volname
              Id = lCopyConn.Source.volid
              If opname = "RCHRES" Then
                cloc = "RCH" & CStr(Id)
              ElseIf opname = "BMPRAC" Then
                cloc = "BMP" & CStr(Id)
              Else
                cloc = Mid(opname, 1, 1) & CStr(Id)
              End If
              myUci.AddOutputWDMDataSet cloc, lCopyConn.Source.member, 1000, 1, newdsn
              myUci.AddExtTarget opname, Id, lCopyConn.Source.group, lCopyConn.Source.member, _
                   lCopyConn.Source.memsub1, lCopyConn.Source.memsub2, lCopyConn.MFact, _
                   lCopyConn.Tran, "WDM1", newdsn, lCopyConn.Source.member, 1, _
                   "ENGL", "AGGR", "REPL"
              myUci.Edited = True
            Next vCopyConn
            If RemovePLTGEN Then
              'remove the copy operation
              myUci.DeleteOperation lConn.Source.volname, lConn.Source.volid
            End If
          End If
        Next vConn
        If RemovePLTGEN Then
          'remove pltgen operation
          myUci.DeleteOperation loper.Name, loper.Id
        End If
      Next vOper
    End If
  
  Me.MousePointer = vbNormal
  Unload Me
End Sub

Private Sub cmdCancel_Click()
  Unload Me
End Sub

Private Sub Form_Load()
  Dim outputwdmname$, i&, k&
  Dim s As String
  '(assuming the uci file has been read)
  If Not myUci Is Nothing Then
  
    'show how many wdms exist
    createwdmflag = -1
    DisplayWDMInfo
    
    'check for schematic and network blocks
    CheckSchematic schemfound, netfound
    If schemfound Then
      chkNetwork.Value = 0
      chkNetwork.Enabled = False
    Else
      chkNetwork.Value = 1
    End If
    
    'check for mutsin point source inputs
    Set myFiles = myUci.filesblock
    k = 1
    mutcnt = 0
    Do While k <= myFiles.Count
      s = myFiles.Value(k).Name
      If UCase(Right(s, 3)) = "MUT" Then  'this is mutsin file
        mutcnt = mutcnt + 1
      End If
      k = k + 1
    Loop
    If mutcnt > 0 Then
      chkPoint.Value = 1
    Else
      chkPoint.Value = 0
      chkPoint.Enabled = False
    End If
      
    'check for pltgen outputs
    pltcnt = myUci.OpnBlks("PLTGEN").Count
    If pltcnt > 0 Then
      chkOutput.Value = 1
      chkRemove.Value = 1
    Else
      chkOutput.Value = 0
      chkOutput.Enabled = False
      chkRemove.Value = 0
      chkRemove.Enabled = False
    End If
    
  End If
End Sub

Private Sub DisplayWDMInfo()
  Dim outputwdmname$, wdmcount&, i&

  wdmcount = 0
  For i = 4 To 1 Step -1
    If Not myUci.GetWDMObj(i) Is Nothing Then
      If myUci.GetWDMObj(i).FileUnit > 0 Then
        'use this as the output wdm
        'wdmsfl = myUci.GetWDMObj(i).FileUnit
        'WDMId = i
        wdmcount = wdmcount + 1
        outputwdmname = FilenameOnly(myUci.GetWDMObj(i).Filename)
      End If
    End If
  Next i
  If wdmcount > 0 Then
    lblConvert.Caption = "Current Project WDM: '" & outputwdmname & "'"
  End If
  If createwdmflag > -1 Then
    lblConvert.Caption = lblConvert.Caption & vbCrLf & "Adding Project WDM: '" & FilenameOnly(newwdmname) & "'"
  End If
  lblConvert.Caption = lblConvert.Caption & vbCrLf & "(" & wdmcount & " Present)"
  If wdmcount > 3 Then
    cmdAdd.Enabled = False
  End If
End Sub

Private Sub Form_Resize()
  If width > 4000 Then
    fraWDM.width = width - 400
    lblConvert.width = fraWDM.width - 1000
    cmdAdd.Left = lblConvert.width
    chkNetwork.width = fraWDM.width
    chkPoint.width = fraWDM.width
    chkOutput.width = fraWDM.width
    cmdConvert.Left = width / 2 - cmdConvert.width - 100
    cmdCancel.Left = width / 2 + 100
  End If
End Sub
