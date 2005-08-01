Imports System.Windows.Forms
Imports atcData
Imports atcUtility

Friend Class atcDebugTimserForm
  Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

  Public Sub New(ByVal aDataManager As atcData.atcDataManager, _
        Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
    MyBase.New()
    pDataManager = aDataManager
    If aDataGroup Is Nothing Then
      pDataGroup = New atcDataGroup
    Else
      pDataGroup = aDataGroup
    End If
    InitializeComponent() 'required by Windows Form Designer

    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each ldisp As atcDataDisplay In DisplayPlugins
      mnuAnalysis.MenuItems.Add(ldisp.Name, New EventHandler(AddressOf mnuAnalysis_Click))
    Next

    If pDataGroup.Count = 0 Then 'ask user to specify some Data
      mnuFileAdd_Click(Nothing, Nothing)
    End If

    If pDataGroup.Count > 0 Then
      Me.Show()
      PopulateTree()
    Else 'use declined to specify Data
      Me.Close()
    End If
  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
  Friend WithEvents mnuAnalysis As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileAdd As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSave As System.Windows.Forms.MenuItem
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(atcDebugTimserForm))
    Me.MainMenu1 = New System.Windows.Forms.MainMenu
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileAdd = New System.Windows.Forms.MenuItem
    Me.mnuFileSave = New System.Windows.Forms.MenuItem
    Me.mnuAnalysis = New System.Windows.Forms.MenuItem
    '
    'MainMenu1
    '
    Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuAnalysis})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileAdd, Me.mnuFileSave})
    Me.mnuFile.Text = "File"
    '
    'mnuFileAdd
    '
    Me.mnuFileAdd.Index = 0
    Me.mnuFileAdd.Text = "Add Data"
    '
    'mnuFileSave
    '
    Me.mnuFileSave.Index = 1
    Me.mnuFileSave.Text = "Save"
    '
    'mnuAnalysis
    '
    Me.mnuAnalysis.Index = 1
    Me.mnuAnalysis.Text = "Analysis"
    '
    'atcDebugTimserForm
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(633, 628)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Menu = Me.MainMenu1
    Me.Name = "atcDebugTimserForm"
    Me.Text = "Data Debug"

  End Sub

#End Region

  Private pDataManager As atcDataManager

  'The tree control
  Private WithEvents atrMain As TreeView

  'The group of atcData displayed
  Private WithEvents pDataGroup As atcDataGroup

  Private Sub PopulateTree()
    Dim lAttributeName As String
    Dim lAttributeValue As String
    Dim lNumValues As Integer
    Dim lNumValuesShow As Integer = 8 'make number of values to display editable
    Dim lNumValuesNow As Integer
    Dim lValueStart As Integer

    If Not atrMain Is Nothing Then
      Me.Controls.Remove(atrMain)
    End If

    'memory leak if we don't clean out old tree?
    atrMain = New TreeView
    With atrMain
      .Location = New System.Drawing.Point(0, 0)
      .Name = "atrMain"
      .Size = Me.ClientSize
      .TabIndex = 14
      .Anchor = AnchorStyles.Top _
             Or AnchorStyles.Bottom _
             Or AnchorStyles.Left _
             Or AnchorStyles.Right
      Me.Controls.Add(atrMain)
      .Refresh()
      For Each lData As atcTimeseries In pDataGroup
        Dim lNode As New TreeNode
        lNode = .Nodes.Add(lData.ToString)

        Dim lAttributeNode As New TreeNode
        lAttributeNode = lNode.Nodes.Add("Attributes")
        Dim lAttributes As SortedList = lData.Attributes.GetAll
        For i As Integer = 0 To lAttributes.Count - 1
          lAttributeName = lAttributes.GetKey(i)
          If InStr(LCase(lAttributeName), "jday", CompareMethod.Text) Then
            lAttributeValue = DumpDate(lAttributes.GetByIndex(i))
          Else
            lAttributeValue = lAttributes.GetByIndex(i)
          End If
          lAttributeNode.Nodes.Add(lAttributeName & " : " & lAttributeValue)
        Next
        lAttributeNode.ExpandAll()
        lAttributeNode.EnsureVisible()

        Dim lInternalNode As New TreeNode
        lInternalNode = lNode.Nodes.Add("Internal")
        lNumValues = lData.numValues
        lInternalNode.Nodes.Add("NumValues :" & lNumValues)
        lInternalNode.ExpandAll()
        lInternalNode.EnsureVisible()

        lNode.Nodes.Add("Computed")
        'all computed attributes here?

        Dim lDataNode As New TreeNode
        lDataNode = lNode.Nodes.Add("Data")
        If lNumValues > lNumValuesShow Then
          lNumValuesNow = lNumValuesShow
        End If
        For j As Integer = 0 To lNumValuesNow - 1
          lDataNode.Nodes.Add(DumpDate(lData.Dates.Value(j)) & " : " & _
                              lData.Value(j))
        Next
        If lNumValues > lNumValuesShow Then  'some from end too
          If lNumValues - lNumValuesShow > lNumValuesShow Then
            lDataNode.Nodes.Add("  <" & lNumValues - (2 * lNumValuesShow) & " values skipped>")
            lValueStart = lNumValues - lNumValuesShow
          Else
            lValueStart = lNumValuesNow
          End If
          For j As Integer = lValueStart To lData.numValues
            lDataNode.Nodes.Add(DumpDate(lData.Dates.Value(j)) & " : " & _
                                lData.Value(j))
          Next
        End If
      Next
    End With
  End Sub

  Private Function GetIndex(ByVal aName As String) As Integer
    Return CInt(Mid(aName, InStr(aName, "#") + 1))
  End Function

  Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
    Dim newDisplay As atcDataDisplay
    Dim DisplayPlugins As ICollection = pDataManager.GetPlugins(GetType(atcDataDisplay))
    For Each ldisp As atcDataDisplay In DisplayPlugins
      If ldisp.Name = sender.Text Then
        Dim typ As System.Type = ldisp.GetType()
        Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(typ)
        newDisplay = asm.CreateInstance(typ.FullName)
        newDisplay.Show(pDataManager, pDataGroup)
        Exit Sub
      End If
    Next
  End Sub

  Private Sub mnuFileAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileAdd.Click
    pDataManager.UserSelectData(, pDataGroup)
  End Sub

  Private Sub pDataGroup_Added(ByVal aAdded As Collections.ArrayList) Handles pDataGroup.Added
    PopulateTree()
    'TODO: could efficiently insert newly added item(s)
  End Sub

  Private Sub pDataGroup_Removed(ByVal aRemoved As System.Collections.ArrayList) Handles pDataGroup.Removed
    PopulateTree()
    'TODO: could efficiently remove by serial number
  End Sub

  Private Sub mnuFileSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSave.Click
    Dim lFileName As String
    Dim cdlg As New Windows.Forms.SaveFileDialog
    With cdlg
      .Title = "Select File to Save Into"
      .Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
      .FilterIndex = 1
      .DefaultExt = "txt"
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        lFileName = AbsolutePath(.FileName, CurDir)
      Else 'Return empty string if user clicked Cancel
        lFileName = ""
      End If
    End With

    If Len(lFileName) > 0 Then
      Dim s As String
      For i As Integer = 0 To atrMain.GetNodeCount(False) - 1
        With atrMain.Nodes(i)
          s = s & .Text & vbCrLf
          For j As Integer = 0 To .GetNodeCount(False) - 1
            With .Nodes(j)
              s = s & vbTab & .Text & vbCrLf
              For k As Integer = 0 To .GetNodeCount(False) - 1
                s = s & vbTab & vbTab & .Nodes(k).Text & vbCrLf
              Next
            End With
          Next
        End With
      Next
      SaveFileString(lFileName, s)
    End If
  End Sub
End Class