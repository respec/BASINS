VERSION 5.00
Begin VB.Form frmGenActTrans 
   Caption         =   "GenScn Activate Modify Special Actions"
   ClientHeight    =   7008
   ClientLeft      =   348
   ClientTop       =   1380
   ClientWidth     =   8532
   BeginProperty Font 
      Name            =   "MS Sans Serif"
      Size            =   7.8
      Charset         =   0
      Weight          =   700
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   HelpContextID   =   65
   Icon            =   "GenTrans.frx":0000
   LinkTopic       =   "Form1"
   PaletteMode     =   1  'UseZOrder
   ScaleHeight     =   7008
   ScaleWidth      =   8532
   Begin MSComctlLib.ListView lstRemove 
      Height          =   492
      Left            =   4080
      TabIndex        =   44
      Top             =   240
      Visible         =   0   'False
      Width           =   2292
      _ExtentX        =   4043
      _ExtentY        =   868
      LabelWrap       =   -1  'True
      HideSelection   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      NumItems        =   0
   End
   Begin VB.CommandButton cmdFull 
      Caption         =   "Full &List"
      Height          =   252
      Left            =   3240
      TabIndex        =   4
      Top             =   3960
      Width           =   1092
   End
   Begin VB.Frame fraFilter 
      Caption         =   "Filter"
      Height          =   2532
      Left            =   120
      TabIndex        =   41
      Top             =   4320
      Width           =   8292
      Begin VB.ComboBox cmbField 
         Height          =   288
         Left            =   1440
         TabIndex        =   8
         Top             =   1440
         Width           =   2532
      End
      Begin VB.CommandButton cmdFiltOr 
         Caption         =   "Add To Available (OR)"
         Height          =   252
         Left            =   4320
         TabIndex        =   10
         Top             =   960
         Width           =   2532
      End
      Begin VB.TextBox txtFilter 
         Height          =   288
         Left            =   1440
         TabIndex        =   7
         Top             =   600
         Width           =   2532
      End
      Begin VB.CommandButton cmdFiltCancel 
         Caption         =   "Cancel"
         Height          =   252
         Left            =   3720
         TabIndex        =   11
         Top             =   2040
         Width           =   852
      End
      Begin VB.CommandButton cmdFiltOkay 
         Caption         =   "Select From Available"
         Height          =   252
         Left            =   4320
         TabIndex        =   9
         Top             =   600
         Width           =   2532
      End
      Begin VB.Label lblField 
         Caption         =   "Search Field:"
         Height          =   252
         Left            =   1440
         TabIndex        =   46
         Top             =   1200
         Width           =   1452
      End
      Begin VB.Label lblFilter 
         Caption         =   "Search Text:"
         Height          =   252
         Left            =   1440
         TabIndex        =   42
         Top             =   360
         Width           =   1932
      End
   End
   Begin VB.Frame fraEdit 
      Caption         =   "Edit Descriptions"
      Height          =   2532
      Left            =   120
      TabIndex        =   37
      Top             =   4320
      Visible         =   0   'False
      Width           =   8292
      Begin VB.CommandButton cmdEditOkay 
         Caption         =   "OK"
         Height          =   252
         Left            =   3240
         TabIndex        =   39
         Top             =   2040
         Width           =   852
      End
      Begin VB.CommandButton cmdEditCancel 
         Caption         =   "Cancel"
         Height          =   252
         Left            =   4200
         TabIndex        =   38
         Top             =   2040
         Width           =   852
      End
      Begin ATCoCtl.ATCoGrid ATCoGridEdit 
         Height          =   1692
         Left            =   120
         TabIndex        =   40
         Top             =   240
         Width           =   8052
         _ExtentX        =   14203
         _ExtentY        =   2985
         SelectionToggle =   0   'False
         AllowBigSelection=   0   'False
         AllowEditHeader =   0   'False
         AllowLoad       =   0   'False
         AllowSorting    =   -1  'True
         Rows            =   1
         Cols            =   2
         ColWidthMinimum =   300
         gridFontBold    =   0   'False
         gridFontItalic  =   0   'False
         gridFontName    =   "MS Sans Serif"
         gridFontSize    =   8
         gridFontUnderline=   0   'False
         gridFontWeight  =   400
         gridFontWidth   =   0
         Header          =   ""
         FixedRows       =   1
         FixedCols       =   1
         ScrollBars      =   3
         SelectionMode   =   0
         BackColor       =   -2147483643
         ForeColor       =   -2147483640
         BackColorBkg    =   -2147483632
         BackColorSel    =   -2147483634
         ForeColorSel    =   16777215
         BackColorFixed  =   -2147483633
         ForeColorFixed  =   -2147483630
         InsideLimitsBackground=   -2147483643
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         ComboCheckValidValues=   -1  'True
      End
   End
   Begin VB.CommandButton cmdFilter 
      Caption         =   "&Filter"
      Height          =   252
      Left            =   1920
      TabIndex        =   3
      Top             =   3960
      Width           =   1092
   End
   Begin VB.CommandButton cmdEdit 
      Caption         =   "Edit &Descriptions"
      Height          =   252
      Left            =   4920
      TabIndex        =   5
      Top             =   3960
      Width           =   1692
   End
   Begin VB.Frame fraModify 
      Caption         =   "Modify"
      Height          =   2532
      Left            =   120
      TabIndex        =   12
      Top             =   4320
      Visible         =   0   'False
      Width           =   8292
      Begin ATCoCtl.ATCoText ATCoText1 
         Height          =   252
         Left            =   1440
         TabIndex        =   43
         Top             =   1560
         Width           =   1212
         _ExtentX        =   2138
         _ExtentY        =   445
         InsideLimitsBackground=   16777215
         OutsideHardLimitBackground=   8421631
         OutsideSoftLimitBackground=   8454143
         HardMax         =   -999
         HardMin         =   -999
         SoftMax         =   -999
         SoftMin         =   -999
         MaxWidth        =   10
         Alignment       =   1
         DataType        =   1
         DefaultValue    =   ""
         Value           =   ""
         Enabled         =   -1  'True
      End
      Begin VB.ComboBox cmbAction 
         Height          =   288
         Left            =   120
         TabIndex        =   22
         Text            =   "Action"
         Top             =   1560
         Width           =   1212
      End
      Begin VB.OptionButton optDated 
         Caption         =   "Dated"
         Height          =   252
         Index           =   0
         Left            =   3000
         TabIndex        =   21
         Top             =   1320
         Width           =   1092
      End
      Begin VB.OptionButton optDated 
         Caption         =   "Undated"
         Height          =   252
         Index           =   1
         Left            =   3000
         TabIndex        =   20
         Top             =   1560
         Width           =   1092
      End
      Begin VB.TextBox txtYear 
         Height          =   288
         Left            =   4200
         TabIndex        =   19
         Text            =   "Year"
         Top             =   1560
         Width           =   852
      End
      Begin VB.TextBox txtMonth 
         Height          =   288
         Left            =   5160
         TabIndex        =   18
         Text            =   "Month"
         Top             =   1560
         Width           =   612
      End
      Begin VB.TextBox txtDay 
         Height          =   288
         Left            =   5880
         TabIndex        =   17
         Text            =   "Day"
         Top             =   1560
         Width           =   492
      End
      Begin VB.TextBox txtHour 
         Height          =   288
         Left            =   6480
         TabIndex        =   16
         Text            =   "Hour"
         Top             =   1560
         Width           =   492
      End
      Begin VB.TextBox txtMinute 
         Height          =   288
         Left            =   7080
         TabIndex        =   15
         Text            =   "Minute"
         Top             =   1560
         Width           =   612
      End
      Begin VB.CommandButton cmdModCancel 
         Caption         =   "Cancel"
         Height          =   252
         Left            =   4200
         TabIndex        =   14
         Top             =   2040
         Width           =   852
      End
      Begin VB.CommandButton cmdModOkay 
         Caption         =   "OK"
         Height          =   252
         Left            =   3240
         TabIndex        =   13
         Top             =   2040
         Width           =   852
      End
      Begin VB.Label lblDescText 
         Caption         =   "Description"
         Height          =   252
         Left            =   1320
         TabIndex        =   36
         Top             =   360
         Width           =   6372
      End
      Begin VB.Label lblText 
         Caption         =   "Conditional"
         Height          =   252
         Left            =   1320
         TabIndex        =   35
         Top             =   600
         Width           =   6372
      End
      Begin VB.Label lblCond 
         Caption         =   "Conditional:"
         Height          =   252
         Left            =   120
         TabIndex        =   34
         Top             =   600
         Width           =   1212
      End
      Begin VB.Label lblAction 
         Caption         =   "Action Code:"
         Height          =   252
         Left            =   120
         TabIndex        =   33
         Top             =   1320
         Width           =   1212
      End
      Begin VB.Label lblValue 
         Caption         =   "Value:"
         Height          =   252
         Left            =   1440
         TabIndex        =   32
         Top             =   1320
         Width           =   1212
      End
      Begin VB.Label lblYear 
         Caption         =   "Year:"
         Height          =   252
         Left            =   4200
         TabIndex        =   31
         Top             =   1320
         Width           =   852
      End
      Begin VB.Label lblMonth 
         Caption         =   "Month:"
         Height          =   252
         Left            =   5160
         TabIndex        =   30
         Top             =   1320
         Width           =   732
      End
      Begin VB.Label lblDay 
         Caption         =   "Day:"
         Height          =   252
         Left            =   5880
         TabIndex        =   29
         Top             =   1320
         Width           =   612
      End
      Begin VB.Label lblHour 
         Caption         =   "Hour:"
         Height          =   252
         Left            =   6480
         TabIndex        =   28
         Top             =   1320
         Width           =   612
      End
      Begin VB.Label lblMinute 
         Caption         =   "Minute:"
         Height          =   252
         Left            =   7080
         TabIndex        =   27
         Top             =   1320
         Width           =   612
      End
      Begin VB.Label lblMin 
         Caption         =   "Minimum:"
         Height          =   252
         Left            =   120
         TabIndex        =   26
         Top             =   960
         Width           =   1692
      End
      Begin VB.Label lblMax 
         Caption         =   "Maximum:"
         Height          =   252
         Left            =   2040
         TabIndex        =   25
         Top             =   960
         Width           =   1812
      End
      Begin VB.Label lblDef 
         Caption         =   "Default:"
         Height          =   252
         Left            =   4080
         TabIndex        =   24
         Top             =   960
         Width           =   1812
      End
      Begin VB.Label lblDesc 
         Caption         =   "Description:"
         Height          =   252
         Left            =   120
         TabIndex        =   23
         Top             =   360
         Width           =   1212
      End
   End
   Begin VB.CommandButton cmdModify 
      Caption         =   "&Modify"
      Height          =   252
      Left            =   120
      TabIndex        =   2
      Top             =   3960
      Width           =   972
   End
   Begin VB.CommandButton cmdClose 
      Caption         =   "&Close"
      Height          =   252
      Left            =   7440
      TabIndex        =   6
      Top             =   3960
      Width           =   972
   End
   Begin MSComctlLib.ListView lstRecord 
      Height          =   3372
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   8292
      _ExtentX        =   14626
      _ExtentY        =   5948
      View            =   3
      LabelWrap       =   -1  'True
      HideSelection   =   0   'False
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      NumItems        =   0
   End
   Begin VB.Label lblCount 
      Alignment       =   1  'Right Justify
      Height          =   252
      Left            =   6720
      TabIndex        =   45
      Top             =   120
      Width           =   1692
   End
   Begin VB.Label lblRecord 
      Caption         =   "Available User-Defined Variable Quantities:"
      Height          =   252
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   4932
   End
End
Attribute VB_Name = "frmGenActTrans"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2000 by AQUA TERRA Consultants
Dim isel&, oldvalue$, cgroup$
Dim oldyear$, oldmonth$, oldday$, oldhour$, oldminute$
Dim itmx, itmy As ListItem
Dim initfg&, retcod&, gname$, gdesc$, intfg&
Dim uvquan$, uvname$, ac$, uvdesc$, idat&(5)
Dim RemainingString$(), fullflag As Boolean
Dim umin!, umax!, udef!, ucikey&, rval!, cond$

Private Sub cmdEdit_Click()
  Dim nrow&, istr$
    MousePointer = vbHourglass
    fullflag = True
    lstRecord.Enabled = False
    cmdModify.Enabled = False
    cmdClose.Enabled = False
    cmdEdit.Enabled = False
    cmdFilter.Enabled = False
    cmdFull.Enabled = False
    fraModify.Visible = False
    fraEdit.Visible = False
    fraFilter.Visible = False
    fraFilter.Visible = False
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + fraModify.Height + 1400
    ATCoGridEdit.clear
    ATCoGridEdit.Rows = 1
    ATCoGridEdit.ColTitle(0) = "Name"
    ATCoGridEdit.ColTitle(1) = "Description"
    ATCoGridEdit.colWidth(0) = 0.2 * ATCoGridEdit.Width
    ATCoGridEdit.colWidth(1) = 0.8 * ATCoGridEdit.Width
    ATCoGridEdit.ColEditable(1) = True
    ChDriveDir p.StatusFilePath 'does input file exist?
    On Error GoTo 20
      Open "uvquan.inp" For Input As #1
      'yes, it exists
      nrow = 1
      Line Input #1, istr
      ATCoGridEdit.TextMatrix(1, 0) = Left(istr, 6)
      ATCoGridEdit.TextMatrix(1, 1) = Mid(istr, 11, 70)
      ReDim Preserve RemainingString(nrow)
      RemainingString(nrow) = Mid(istr, 81)
10    Line Input #1, istr
        nrow = nrow + 1
        ATCoGridEdit.InsertRow (nrow - 1)
        ATCoGridEdit.TextMatrix(nrow, 0) = Left(istr, 6)
        ATCoGridEdit.TextMatrix(nrow, 1) = Mid(istr, 11, 70)
        ReDim Preserve RemainingString(nrow)
        RemainingString(nrow) = Mid(istr, 81)
      GoTo 10
20  'end of uvquan.inp
    Close #1
    fraEdit.Visible = True
    MousePointer = vbDefault
End Sub

Private Sub cmdEditCancel_Click()
  Dim i&
    fullflag = False
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    fraEdit.Visible = False
End Sub

Private Sub cmdEditOkay_Click()
  Dim i&, j&, istr$
    fullflag = False
    MousePointer = vbHourglass
    ChDriveDir p.StatusFilePath
    On Error GoTo 20
      Open "uvquan.inp" For Output As #1
      'yes, it exists
      For i = 1 To ATCoGridEdit.Rows
        istr = ATCoGridEdit.TextMatrix(i, 0) & "    " & _
               ATCoGridEdit.TextMatrix(i, 1)
        If Len(istr) < 80 Then
          For j = 1 To 80 - Len(istr)
            istr = istr & " "
          Next j
        ElseIf Len(istr) > 80 Then
          istr = Left(istr, 80)
        End If
        istr = istr & RemainingString(i)
        Print #1, istr
      Next i
20  'end of uvquan.inp
    Close #1
    lstRecord.ListItems.clear
    Call FillListRecords
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    MousePointer = vbDefault
    fraEdit.Visible = False
End Sub

Private Sub cmdFiltCancel_Click()
  Dim i&
    fullflag = False
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    fraFilter.Visible = False
End Sub

Private Sub cmdFilter_Click()
    fullflag = True
    lstRecord.Enabled = False
    cmdModify.Enabled = False
    cmdClose.Enabled = False
    cmdEdit.Enabled = False
    cmdFilter.Enabled = False
    cmdFull.Enabled = False
    fraModify.Visible = False
    fraEdit.Visible = False
    fraFilter.Visible = False
    fraFilter.Visible = False
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + fraModify.Height + 1400
    txtFilter.text = ""
    cmbField.clear
    cmbField.AddItem "All"
    cmbField.AddItem "Name"
    cmbField.AddItem "Group"
    cmbField.AddItem "Description"
    cmbField.AddItem "Action"
    cmbField.AddItem "Value"
    cmbField.AddItem "Year"
    cmbField.AddItem "Month"
    cmbField.AddItem "Day"
    cmbField.AddItem "Hour"
    cmbField.AddItem "Minute"
    cmbField.AddItem "Conditional"
    cmbField.AddItem "Occurrance"
    cmbField.ListIndex = 0
    If lstRemove.ListItems.Count = 0 Then
      'cannot use 'or' condition
      cmdFiltOr.Visible = False
      cmdFiltOkay.Caption = "Select From Available"
    Else
      'or' condition is available
      cmdFiltOr.Visible = True
      cmdFiltOkay.Caption = "Select From Available (AND)"
    End If
    fraFilter.Visible = True
End Sub

Private Sub cmdFiltOkay_Click()
  Dim i&, j&, k&, removeit As Boolean
    fullflag = False
    MousePointer = vbHourglass
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    If Len(txtFilter.text) > 0 Then
      i = 1
10    'eliminate records from list without this text
        removeit = True
        Set itmx = lstRecord.ListItems(i)
        If cmbField.ListIndex = 0 Or cmbField.ListIndex = 1 Then
          'search name field
          If InStr(1, itmx, txtFilter.text) > 0 Then
            'found it
            removeit = False
          End If
        End If
        For j = 1 To 10
          If cmbField.ListIndex = 0 Or cmbField.ListIndex = j + 1 Then
          'search this field
            If InStr(1, itmx.SubItems(j), txtFilter.text) > 0 Then
              'found it
              removeit = False
            End If
          End If
        Next j
        If cmbField.ListIndex = 0 Or cmbField.ListIndex = 12 Then
          If InStr(1, itmx.SubItems(16), txtFilter.text) > 0 Then
            'found it
            removeit = False
          End If
        End If
        If removeit Then
          'save this record in removed list
          Set itmy = lstRemove.ListItems.Add(, , itmx)
          For k = 1 To 16
            itmy.SubItems(k) = itmx.SubItems(k)
          Next k
          'remove this record
          lstRecord.ListItems.Remove (i)
        Else
          i = i + 1
        End If
      If i <= lstRecord.ListItems.Count Then GoTo 10
    End If
    'now enable the list of records
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    lblCount.Caption = lstRecord.ListItems.Count & " of " & _
      lstRecord.ListItems.Count + lstRemove.ListItems.Count
    MousePointer = vbDefault
    fraFilter.Visible = False
End Sub

Private Sub cmdFiltOr_Click()
  Dim i&, j&, k&, addit As Boolean
    fullflag = False
    MousePointer = vbHourglass
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    If Len(txtFilter.text) > 0 Then
      i = 1
10    'add records to active list with this text
        addit = False
        Set itmy = lstRemove.ListItems(i)
        If cmbField.ListIndex = 0 Or cmbField.ListIndex = 1 Then
          If InStr(1, itmy, txtFilter.text) > 0 Then
            'found it
            addit = True
          End If
        End If
        For j = 1 To 10
          If cmbField.ListIndex = 0 Or cmbField.ListIndex = j + 1 Then
            If InStr(1, itmy.SubItems(j), txtFilter.text) > 0 Then
              'found it
              addit = True
            End If
          End If
        Next j
        If cmbField.ListIndex = 0 Or cmbField.ListIndex = 12 Then
          If InStr(1, itmy.SubItems(16), txtFilter.text) > 0 Then
            'found it
            addit = True
          End If
        End If
        If addit Then
          'save this record in available list
          Set itmx = lstRecord.ListItems.Add(, , itmy)
          For k = 1 To 16
            itmx.SubItems(k) = itmy.SubItems(k)
          Next k
          'remove this record
          lstRemove.ListItems.Remove (i)
        Else
          i = i + 1
        End If
      If i <= lstRemove.ListItems.Count Then GoTo 10
    End If
    'enable the list of records
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    lblCount.Caption = lstRecord.ListItems.Count & " of " & _
      lstRecord.ListItems.Count + lstRemove.ListItems.Count
    MousePointer = vbDefault
    fraFilter.Visible = False
End Sub

Private Sub cmdFull_Click()
  Dim i&
    MousePointer = vbHourglass
    ChDriveDir p.StatusFilePath
    lstRecord.ListItems.clear
    Call FillListRecords
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    lstRecord.Enabled = True
    MousePointer = vbDefault
End Sub

Private Sub cmdModCancel_Click()
  Dim i&
    fullflag = False
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lstRecord.Enabled = True
    fraModify.Visible = False
End Sub

Private Sub cmdClose_Click()
    Unload frmGenActTrans
End Sub

Private Sub cmdModify_Click()
  fullflag = True
  Call ModifyUvquan
End Sub

Private Sub ModifyUvquan()
    Dim s&(5), e&(5), ou&, sp&, ru&, em&, inf$, clen&, i&
    lstRecord.Enabled = False
    cmdModify.Enabled = False
    cmdClose.Enabled = False
    cmdEdit.Enabled = False
    cmdFilter.Enabled = False
    cmdFull.Enabled = False
    fraEdit.Visible = False
    fraFilter.Visible = False
    fraModify.Visible = True
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + fraModify.Height + 1400
    Set itmx = lstRecord.ListItems(isel)
    fraModify.Caption = "Modify - " & itmx
    lblDescText.Caption = itmx.SubItems(2)
    clen = Len(itmx.SubItems(10))
    If clen > 0 Then
      lblText = itmx.SubItems(10)
    Else
      lblText = "<none>"
    End If
    cmbAction.clear
    cmbAction.AddItem "="
    cmbAction.AddItem "+="
    cmbAction.AddItem "-="
    cmbAction.AddItem "*="
    cmbAction.AddItem "/="
    cmbAction.AddItem "MIN"
    cmbAction.AddItem "MAX"
    cmbAction.AddItem "ABS"
    cmbAction.AddItem "INT"
    cmbAction.AddItem "^="
    cmbAction.AddItem "LN"
    cmbAction.AddItem "LOG"
    cmbAction.AddItem "MOD"
    For i = 0 To cmbAction.ListCount
      If cmbAction.List(i) = itmx.SubItems(3) Then
        cmbAction.ListIndex = i
      End If
    Next i
    ATCoText1.HardMin = itmx.SubItems(11)
    ATCoText1.HardMax = itmx.SubItems(12)
    ATCoText1.value = itmx.SubItems(4)
    If itmx.SubItems(11) = -999 Then
      lblMin.Caption = "Minimum: none"
    Else
      lblMin.Caption = "Minimum: " & itmx.SubItems(11)
    End If
    If itmx.SubItems(12) = -999 Then
      lblMax.Caption = "Maximum: none"
    Else
      lblMax.Caption = "Maximum: " & itmx.SubItems(12)
    End If
    If itmx.SubItems(13) = -999 Then
      lblDef.Caption = "Default: none"
    Else
      lblDef.Caption = "Default: " & itmx.SubItems(13)
    End If
    If itmx.SubItems(5) > 0 Then
      optDated(0) = True
      optDated(1) = False
      txtYear.text = itmx.SubItems(5)
      txtMonth.text = itmx.SubItems(6)
      txtDay.text = itmx.SubItems(7)
      txtHour.text = itmx.SubItems(8)
      txtMinute.text = itmx.SubItems(9)
      txtYear.Visible = True
      txtMonth.Visible = True
      txtDay.Visible = True
      txtHour.Visible = True
      txtMinute.Visible = True
      lblYear.Visible = True
      lblMonth.Visible = True
      lblDay.Visible = True
      lblHour.Visible = True
      lblMinute.Visible = True
    Else
      optDated(0) = False
      optDated(1) = True
      'get start date of run for defaults
      Call F90_GLOBLK(s(), e(), ou, sp, ru, em, inf)
      txtYear.text = s(0)
      txtMonth.text = s(1)
      txtDay.text = s(2)
      txtHour.text = s(3)
      txtMinute.text = s(4)
      txtYear.Visible = False
      txtMonth.Visible = False
      txtDay.Visible = False
      txtHour.Visible = False
      txtMinute.Visible = False
      lblYear.Visible = False
      lblMonth.Visible = False
      lblDay.Visible = False
      lblHour.Visible = False
      lblMinute.Visible = False
    End If
End Sub
Private Sub cmdModOkay_Click()
  Dim i&
    fullflag = False
    Dim idates&(5)
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    cmdModify.Enabled = True
    cmdClose.Enabled = True
    cmdEdit.Enabled = True
    cmdFilter.Enabled = True
    cmdFull.Enabled = True
    lblRecord.Enabled = True
    lstRecord.Enabled = True
    Set itmx = lstRecord.ListItems(isel)
    itmx.SubItems(3) = cmbAction.List(cmbAction.ListIndex)
    itmx.SubItems(4) = ATCoText1.value
    If optDated(0) = True Then
      itmx.SubItems(5) = txtYear.text
      itmx.SubItems(6) = txtMonth.text
      itmx.SubItems(7) = txtDay.text
      itmx.SubItems(8) = txtHour.text
      itmx.SubItems(9) = txtMinute.text
    ElseIf optDated(1) = True Then
      For i = 5 To 9
        itmx.SubItems(i) = 0
      Next i
    End If
    idates(0) = itmx.SubItems(5)
    idates(1) = itmx.SubItems(6)
    idates(2) = itmx.SubItems(7)
    idates(3) = itmx.SubItems(8)
    idates(4) = itmx.SubItems(9)
    Call F90_STSPPN(itmx.SubItems(14), itmx.SubItems(4), itmx.SubItems(15), idates(0), itmx.SubItems(3), Len(itmx.SubItems(3)))
    fraModify.Visible = False
End Sub

Private Sub Form_Load()
    MousePointer = vbHourglass
    fullflag = False
    Dim clmX As ColumnHeader
    'set column headers for record list
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Name", TextWidth("xxxxxx"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Group", TextWidth("xxxxxx"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Description", TextWidth("WWWWWWWWWWWWWWWWWWWWWWWWW"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Action", TextWidth("xxxxxx"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Value", TextWidth("0000000000"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Year", TextWidth("0000"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Mo", TextWidth("00"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Dy", TextWidth("00"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Hr", TextWidth("00"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Mi", TextWidth("00"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Conditional", TextWidth("WWWWWWWWWWWWWWWWWWWWWWWWWWWW"))
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Minimum", 0)
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Maximum", 0)
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Default", 0)
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Key", 0)
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Integer", 0)
    Set clmX = lstRecord.ColumnHeaders.Add(, , "Occurrance", TextWidth("0000000000"))
    'set column headers for removed list
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Name", TextWidth("xxxxxx"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Group", TextWidth("xxxxxx"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Description", TextWidth("WWWWWWWWWWWWWWWWWWWWWWWWW"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Action", TextWidth("xxxxxx"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Value", TextWidth("0000000000"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Year", TextWidth("0000"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Mo", TextWidth("00"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Dy", TextWidth("00"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Hr", TextWidth("00"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Mi", TextWidth("00"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Conditional", TextWidth("WWWWWWWWWWWWWWWWWWWWWWWWWWWW"))
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Minimum", 0)
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Maximum", 0)
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Default", 0)
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Key", 0)
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Integer", 0)
    Set clmX = lstRemove.ColumnHeaders.Add(, , "Occurrance", TextWidth("0000000000"))
    Call FillListRecords
    If Me.WindowState = vbNormal Then Me.Height = lstRecord.Height + 1300
    cmdModify.Enabled = False
    fraModify.Visible = False
    fraEdit.Visible = False
    fraFilter.Visible = False
    MousePointer = vbDefault
End Sub
Private Sub FillListRecords()
  Dim occurrance&
    'read each item of uvquan input file
    initfg = 1
    retcod = 0
    While retcod = 0
      Call F90_STSPGU(initfg, retcod, uvquan, cgroup, uvdesc, uvname, _
                      umin, umax, udef, intfg)
      If retcod = 2 Then
        'end of uvquan file
      ElseIf retcod = 3 Then
        'error reading uvquan file
        MsgBox "Error reading the system file 'uvquan.inp'.", _
          vbExclamation, "GenScn Activate Modify Special Actions"
      ElseIf retcod = 0 Then
        ucikey = -1
        occurrance = 0
        Do Until ucikey = 0
          Call F90_STSPFN(uvname, intfg, ucikey, _
                          idat(), rval, ac, cond)
          If ucikey > 0 Then
            occurrance = occurrance + 1
            Set itmx = lstRecord.ListItems.Add(, , uvquan)
            itmx.SubItems(1) = cgroup
            itmx.SubItems(2) = uvdesc
            itmx.SubItems(3) = ac
            itmx.SubItems(4) = rval
            itmx.SubItems(5) = idat(0)
            itmx.SubItems(6) = idat(1)
            itmx.SubItems(7) = idat(2)
            itmx.SubItems(8) = idat(3)
            itmx.SubItems(9) = idat(4)
            itmx.SubItems(10) = cond
            itmx.SubItems(11) = umin
            itmx.SubItems(12) = umax
            itmx.SubItems(13) = udef
            itmx.SubItems(14) = ucikey
            itmx.SubItems(15) = intfg
            itmx.SubItems(16) = occurrance
          End If
        Loop
      End If
      initfg = 0
    Wend
    lstRemove.ListItems.clear
    lblCount.Caption = lstRecord.ListItems.Count & " of " & lstRecord.ListItems.Count
End Sub

Private Sub lblName_Click()

End Sub

Private Sub Form_Resize()
  With frmGenActTrans
    If .Width >= 8100 Then
      lstRecord.Width = .Width - 350
      lblCount.Left = .Width - 250 - lblCount.Width
      cmdClose.Left = .Width - 250 - cmdClose.Width
      cmdEdit.Left = .Width * 0.6
      cmdFull.Left = .Width * 0.35
      cmdFilter.Left = .Width * 0.2
      fraFilter.Width = lstRecord.Width
      cmdFiltCancel.Left = (.Width * 0.5) - (cmdFiltCancel.Width * 0.5) - 400
      cmdFiltOkay.Left = (.Width * 0.5) + 300
      cmdFiltOr.Left = cmdFiltOkay.Left
      txtFilter.Left = (.Width * 0.5) - txtFilter.Width - 500
      cmbField.Left = txtFilter.Left
      lblFilter.Left = txtFilter.Left
      lblField.Left = txtFilter.Left
      fraModify.Width = lstRecord.Width
      cmdModOkay.Left = (.Width * 0.5) - cmdEditOkay.Width - 200
      cmdModCancel.Left = (.Width * 0.5)
      fraEdit.Width = lstRecord.Width
      ATCoGridEdit.Width = lstRecord.Width - 250
      cmdEditOkay.Left = (.Width * 0.5) - cmdEditOkay.Width - 200
      cmdEditCancel.Left = (.Width * 0.5)
    End If
    If (fullflag And .Height > 4600) Or _
      (fullflag = False And .Height > 2100) Then
      If fullflag Then
        lstRecord.Height = .Height - fraFilter.Height - 1400
      Else
        lstRecord.Height = .Height - 1200
      End If
      fraFilter.Top = 900 + lstRecord.Height
      fraModify.Top = fraFilter.Top
      fraEdit.Top = fraFilter.Top
      cmdModify.Top = 500 + lstRecord.Height
      cmdFilter.Top = cmdModify.Top
      cmdFull.Top = cmdModify.Top
      cmdEdit.Top = cmdModify.Top
      cmdClose.Top = cmdModify.Top
    End If
  End With
End Sub

Private Sub lstRecord_Click()
  Dim i&
    'is anything selected?
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      cmdModify.Enabled = True
    Else
      cmdModify.Enabled = False
    End If
End Sub


Private Sub lstRecord_ColumnClick(ByVal ColumnHeader As ColumnHeader)
    Dim k%

    k = ColumnHeader.SubItemIndex
    If k = lstRecord.SortKey Then
      lstRecord.SortOrder = (lstRecord.SortOrder + 1) Mod 2
    Else
      lstRecord.SortKey = k
      lstRecord.SortOrder = 0  ' ascending
    End If
    lstRecord.Sorted = True
End Sub


Private Sub lstRecord_DblClick()
  Dim i&
    'is anything selected?
    isel = 0
    For i = 1 To lstRecord.ListItems.Count
      If lstRecord.ListItems(i).Selected Then
        isel = i
      End If
    Next i
    If isel > 0 Then
      Set itmx = lstRecord.ListItems(isel)
      fullflag = True
      Call ModifyUvquan
    End If
End Sub

Private Sub optDated_Click(Index As Integer)
    If optDated(0) = True Then
      txtYear.Visible = True
      txtMonth.Visible = True
      txtDay.Visible = True
      txtHour.Visible = True
      txtMinute.Visible = True
      lblYear.Visible = True
      lblMonth.Visible = True
      lblDay.Visible = True
      lblHour.Visible = True
      lblMinute.Visible = True
    ElseIf optDated(1) = True Then
      txtYear.Visible = False
      txtMonth.Visible = False
      txtDay.Visible = False
      txtHour.Visible = False
      txtMinute.Visible = False
      lblYear.Visible = False
      lblMonth.Visible = False
      lblDay.Visible = False
      lblHour.Visible = False
      lblMinute.Visible = False
    End If
End Sub


Private Sub txtDay_GotFocus()
  oldday = txtDay.text
End Sub


Private Sub txtDay_LostFocus()
  Dim daynew$, chgflg&, iday&
  If Len(txtDay.text) > 0 Then
    daynew = txtDay.text
    Call ChkTxtI("Day", 1, 31, daynew, iday, chgflg)
    If chgflg <> 1 Then
      txtDay.text = oldday
    End If
  Else
    txtDay.text = oldday
  End If
End Sub


Private Sub txtHour_GotFocus()
  oldhour = txtHour.text
End Sub

Private Sub txtHour_LostFocus()
  Dim hournew$, chgflg&, ihour&
  If Len(txtHour.text) > 0 Then
    hournew = txtHour.text
    Call ChkTxtI("Hour", 0, 24, hournew, ihour, chgflg)
    If chgflg <> 1 Then
      txtHour.text = oldhour
    End If
  Else
    txtHour.text = oldhour
  End If
End Sub


Private Sub txtMinute_GotFocus()
    oldminute = txtMinute.text
End Sub


Private Sub txtMinute_LostFocus()
  Dim minutenew$, chgflg&, iminute&
  If Len(txtMinute.text) > 0 Then
    minutenew = txtMinute.text
    Call ChkTxtI("Minute", 0, 60, minutenew, iminute, chgflg)
    If chgflg <> 1 Then
      txtMinute.text = oldminute
    End If
  Else
    txtMinute.text = oldminute
  End If
End Sub


Private Sub txtMonth_GotFocus()
    oldmonth = txtMonth.text
End Sub


Private Sub txtMonth_LostFocus()
  Dim monthnew$, chgflg&, imonth&
  If Len(txtMonth.text) > 0 Then
    monthnew = txtMonth.text
    Call ChkTxtI("Month", 1, 12, monthnew, imonth, chgflg)
    If chgflg <> 1 Then
      txtMonth.text = oldmonth
    End If
  Else
    txtMonth.text = oldmonth
  End If
End Sub
Private Sub txtYear_GotFocus()
    oldyear = txtYear.text
End Sub


Private Sub txtYear_LostFocus()
  Dim yearnew$, chgflg&, iyear&
  If Len(txtYear.text) > 0 Then
    yearnew = txtYear.text
    Call ChkTxtI("Year", 1900, 2100, yearnew, iyear, chgflg)
    If chgflg <> 1 Then
      txtYear.text = oldyear
    End If
  Else
    txtYear.text = oldyear
  End If
End Sub
