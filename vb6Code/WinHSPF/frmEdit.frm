VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Begin VB.Form frmEdit 
   Caption         =   "WinHSPF - Input Data Editor"
   ClientHeight    =   4860
   ClientLeft      =   60
   ClientTop       =   348
   ClientWidth     =   3732
   HelpContextID   =   40
   Icon            =   "frmEdit.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   4860
   ScaleWidth      =   3732
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdClose 
      Cancel          =   -1  'True
      Caption         =   "&Close"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   7.8
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1320
      TabIndex        =   0
      Top             =   4320
      Width           =   972
   End
   Begin MSComctlLib.TreeView treUci 
      Height          =   4095
      Left            =   60
      TabIndex        =   1
      Top             =   60
      Width           =   3615
      _ExtentX        =   6371
      _ExtentY        =   7218
      _Version        =   393217
      Indentation     =   176
      LineStyle       =   1
      Style           =   7
      Appearance      =   1
   End
End
Attribute VB_Name = "frmEdit"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
'Copyright 2002 AQUA TERRA Consultants - Royalty-free use permitted under open source license

Public tabname$
Private Sub cmdClose_Click()
  Unload Me
End Sub

Private Sub Form_Load()
  Dim vBlock As Variant, lBlock As HspfBlockDef
  Dim vSection As Variant, lSection As HspfSectionDef, kSection As String
  Dim vTable As Variant, lTable As HspfTableDef, kTable As String
  Dim vParm As Variant, lParm As HSPFParmDef, j&, i&, isop As Boolean
  Dim lOpnBlk As HspfOpnBlk
  
  If myUci.MetSegs.Count > 0 Then
    myUci.MetSeg2Source
  End If
  myUci.Point2Source

  For Each vBlock In myMsg.BlockDefs
    Set lBlock = vBlock
    treUci.Nodes.Add , , lBlock.Name, lBlock.Name
    For Each vSection In lBlock.SectionDefs
      Set lSection = vSection
      If lSection.Name = "<NONE>" Then
        kSection = lBlock.Name
      Else
        kSection = lBlock.Name & ":" & lSection.Name
        treUci.Nodes.Add lBlock.Name, tvwChild, kSection, lSection.Name
      End If
      For Each vTable In lSection.TableDefs
        Set lTable = vTable
        If lTable.Name = "<NONE>" Then
          kTable = kSection
        Else
          kTable = kSection & ":" & lTable.Name
          treUci.Nodes.Add kSection, tvwChild, kTable, lTable.Name
        End If
        'For Each vParm In lTable.ParmDefs
        '  'Set lParm = vParm 'used to have to do this
        '  On Error Resume Next
        '  treUci.Nodes.Add kTable, tvwChild, kTable & ":" & vParm.name, vParm.name
        'Next vParm
      Next vTable
    Next vSection
  Next vBlock
  
  'now bold the ones that are active
  Call BoldActive
  
End Sub

Private Sub Form_Resize()
  If Width > 200 And Height > 1000 Then
    treUci.Width = Width - 200
    cmdClose.Left = Width / 2 - cmdClose.Width / 2
    cmdClose.Top = Height - cmdClose.Height - 400
    treUci.Height = cmdClose.Top - 200
  End If
End Sub

Private Sub Form_Unload(Cancel As Integer)
  myUci.Source2MetSeg
  myUci.Source2Point
End Sub

Private Sub treUci_DblClick()
  Call TableSelected
End Sub

Private Sub TableSelected()
  Dim lTable As HspfTable
  Dim lOpnBlk As HspfOpnBlk
  Dim S$, ilen&, iresp&
  Dim opname$
  Dim lOper As HspfOperation, vOper As Variant
  Dim M As String
  
  On Error GoTo notFound:
  
  If treUci.SelectedItem.Children > 0 Then
    'do nothing in this case -- this is not a table name
    Exit Sub
  Else
    'we have selected the table name
    S = treUci.SelectedItem.FullPath
    ilen = InStr(1, S, "\")
    If ilen = 0 Then
      opname = S
    Else
      opname = Mid(S, 1, ilen - 1)
    End If
    tabname = treUci.SelectedItem
  End If
  
  Me.Hide
  If tabname = "GLOBAL" Then
    myUci.GlobalBlock.Edit
  ElseIf tabname = "OPN SEQUENCE" Then
    myUci.OpnSeqBlock.Edit
  ElseIf tabname = "FILES" Then
    myUci.filesblock.Edit
  ElseIf tabname = "CATEGORY" And Not myUci.categoryblock Is Nothing Then
    myUci.categoryblock.Edit
  ElseIf tabname = "FTABLES" Then
    myUci.OpnBlks("RCHRES").Ids(1).Ftable.Edit
  ElseIf tabname = "MONTH-DATA" And Not myUci.MonthData Is Nothing Then
    myUci.MonthData.Edit
  ElseIf tabname = "EXT SOURCES" Then
    myUci.Connections(1).EditExtSrc
  ElseIf tabname = "NETWORK" Then
    myUci.Connections(1).EditNetwork
  ElseIf tabname = "SCHEMATIC" Then
    myUci.Connections(1).EditSchematic
  ElseIf tabname = "EXT TARGETS" Then
    myUci.Connections(1).EditExtTar
  ElseIf tabname = "MASS-LINK" Then
    myUci.MassLinks(1).Edit
  ElseIf tabname = "SPEC-ACTIONS" And Not myUci.SpecialActionBlk Is Nothing Then
    myUci.SpecialActionBlk.Edit
  Else
    'regular case
    Set lOpnBlk = myUci.OpnBlks(opname)
    If lOpnBlk.Count > 0 Then
      'check to see if this table exists
      If Not lOpnBlk.TableExists(tabname) Then
        iresp = myMsgBox.Show("Table " & tabname & " does not exist.  Do you want to add it?", "WinHSPF - Input Data Editor", "+&OK", "&Cancel")
        If iresp = 1 Then
          lOpnBlk.AddTableForAll tabname, opname
          setDefaultsForTable myUci, defUci, opname, tabname
          Call SetMissingValuesToDefaults(myUci, defUci)
          Call BoldActive
        End If
      End If
      If lOpnBlk.TableExists(tabname) Then
        Set lTable = lOpnBlk.tables(tabname)
        lTable.Edit
        'check for missing tables, add if needed
        CheckAndAddMissingTables opname
        CheckAndAddMassLinks
        Call SetMissingValuesToDefaults(myUci, defUci)
        Call BoldActive
      End If
    Else
      myMsgBox.Show "No Operations of this type available", "Edit Problem", "+&Ok"
    End If
  End If
  Me.Show vbModal
  Exit Sub

notFound:
  M = Err.Description
  Err.Clear
  On Error GoTo 0
  'Me.Hide
  If (tabname = "EXT SOURCES") Then
    'for debugging, add text to message
    myMsgBox.Show "Table/Block " & tabname & " not found." & vbCrLf & M, "Edit Problem", "+&Ok"
  Else
    myMsgBox.Show "Table/Block " & tabname & " not found.", "Edit Problem", "+&Ok"
  End If
  'Me.Show vbModal
End Sub

Private Sub treUci_KeyDown(KeyCode As Integer, Shift As Integer)
  If KeyCode = Asc(vbCr) Then
    Call TableSelected
  End If
End Sub

Private Sub BoldActive()
  Dim vBlock As Variant, lBlock As HspfBlockDef
  Dim vSection As Variant, lSection As HspfSectionDef, kSection As String
  Dim vTable As Variant, lTable As HspfTableDef, kTable As String
  Dim vParm As Variant, lParm As HSPFParmDef, j&, i&, isop As Boolean
  Dim lOpnBlk As HspfOpnBlk
  
  'bold the blocks/tables that are active
  For i = 1 To treUci.Nodes.Count
    If treUci.Nodes(i).Children = 0 And treUci.Nodes(i).Parent Is Nothing Then
      'this is a non-operation block
      If treUci.Nodes(i).Text = "PATHNAMES" Then
      'ElseIf treUci.Nodes(i).Text = "CATEGORY" Then
      ElseIf treUci.Nodes(i).Text = "FORMATS" Then
      Else
        treUci.Nodes(i).Bold = True
      End If
    ElseIf treUci.Nodes(i).Parent Is Nothing Then
      'this is an operation
      Set lOpnBlk = myUci.OpnBlks(treUci.Nodes(i).Text)
      If lOpnBlk.Count > 0 Then
        'this opn exists
        treUci.Nodes(i).Bold = True
      End If
    ElseIf treUci.Nodes(i).Children = 0 Then
      'this is a table
      If lOpnBlk.Count > 0 Then
        For j = 1 To lOpnBlk.tables.Count
          If lOpnBlk.tables(j).Name = treUci.Nodes(i).Text Then
            'this table exists
            treUci.Nodes(i).Bold = True
            treUci.Nodes(i).Parent.Bold = True
          End If
        Next j
      End If
    End If
  Next i
  
End Sub
