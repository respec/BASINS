'********************************************************************************************************
'File Name: modMain.vb
'Description: Entry point for MapWindow
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'7/29/2005 - Lailin Chen - Code added to sort the plugins in the plugin list
'7/29/2005 - Lailin Chen - Implemented the Refresh button function
'10/20/2005 - Paul Meems - Starting with translating resourcefile into Dutch.
'8/3/2006 - pm - Translation of new strings into Dutch
'********************************************************************************************************

Friend Class PluginsForm
    Inherits System.Windows.Forms.Form
#Region "Declarations"
    'PM
    Private resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PluginsForm))
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        LookupTable = New Hashtable
        cDlg.Filter = "Plugin Files (*.dll)|*.dll"
        cDlg.FilterIndex = 0
        cDlg.ValidateNames = True
        cDlg.CheckFileExists = True
        cDlg.CheckPathExists = True
        cDlg.DefaultExt = "dll"
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
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lstPlugins As System.Windows.Forms.CheckedListBox
    Friend WithEvents rtbPluginInfo As System.Windows.Forms.RichTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents chkRemember As System.Windows.Forms.CheckBox
    Friend WithEvents btnTurnAllOff As System.Windows.Forms.Button
    Friend WithEvents btnTurnAllOn As System.Windows.Forms.Button
    Friend WithEvents btnRefreshList As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PluginsForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnTurnAllOff = New System.Windows.Forms.Button
        Me.btnTurnAllOn = New System.Windows.Forms.Button
        Me.btnRefreshList = New System.Windows.Forms.Button
        Me.lstPlugins = New System.Windows.Forms.CheckedListBox
        Me.rtbPluginInfo = New System.Windows.Forms.RichTextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.chkRemember = New System.Windows.Forms.CheckBox
        Me.cDlg = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.AccessibleDescription = Nothing
        Me.GroupBox1.AccessibleName = Nothing
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.BackgroundImage = Nothing
        Me.GroupBox1.Controls.Add(Me.btnTurnAllOff)
        Me.GroupBox1.Controls.Add(Me.btnTurnAllOn)
        Me.GroupBox1.Controls.Add(Me.btnRefreshList)
        Me.GroupBox1.Controls.Add(Me.lstPlugins)
        Me.GroupBox1.Font = Nothing
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'btnTurnAllOff
        '
        Me.btnTurnAllOff.AccessibleDescription = Nothing
        Me.btnTurnAllOff.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTurnAllOff, "btnTurnAllOff")
        Me.btnTurnAllOff.BackgroundImage = Nothing
        Me.btnTurnAllOff.Font = Nothing
        Me.btnTurnAllOff.Name = "btnTurnAllOff"
        '
        'btnTurnAllOn
        '
        Me.btnTurnAllOn.AccessibleDescription = Nothing
        Me.btnTurnAllOn.AccessibleName = Nothing
        resources.ApplyResources(Me.btnTurnAllOn, "btnTurnAllOn")
        Me.btnTurnAllOn.BackgroundImage = Nothing
        Me.btnTurnAllOn.Font = Nothing
        Me.btnTurnAllOn.Name = "btnTurnAllOn"
        '
        'btnRefreshList
        '
        Me.btnRefreshList.AccessibleDescription = Nothing
        Me.btnRefreshList.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRefreshList, "btnRefreshList")
        Me.btnRefreshList.BackgroundImage = Nothing
        Me.btnRefreshList.Font = Nothing
        Me.btnRefreshList.Name = "btnRefreshList"
        '
        'lstPlugins
        '
        Me.lstPlugins.AccessibleDescription = Nothing
        Me.lstPlugins.AccessibleName = Nothing
        resources.ApplyResources(Me.lstPlugins, "lstPlugins")
        Me.lstPlugins.BackgroundImage = Nothing
        Me.lstPlugins.Font = Nothing
        Me.lstPlugins.Name = "lstPlugins"
        '
        'rtbPluginInfo
        '
        Me.rtbPluginInfo.AccessibleDescription = Nothing
        Me.rtbPluginInfo.AccessibleName = Nothing
        resources.ApplyResources(Me.rtbPluginInfo, "rtbPluginInfo")
        Me.rtbPluginInfo.BackgroundImage = Nothing
        Me.rtbPluginInfo.Font = Nothing
        Me.rtbPluginInfo.Name = "rtbPluginInfo"
        '
        'Label3
        '
        Me.Label3.AccessibleDescription = Nothing
        Me.Label3.AccessibleName = Nothing
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Font = Nothing
        Me.Label3.Name = "Label3"
        '
        'cmdApply
        '
        Me.cmdApply.AccessibleDescription = Nothing
        Me.cmdApply.AccessibleName = Nothing
        resources.ApplyResources(Me.cmdApply, "cmdApply")
        Me.cmdApply.BackgroundImage = Nothing
        Me.cmdApply.Font = Nothing
        Me.cmdApply.Name = "cmdApply"
        '
        'cmdCancel
        '
        Me.cmdCancel.AccessibleDescription = Nothing
        Me.cmdCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.cmdCancel, "cmdCancel")
        Me.cmdCancel.BackgroundImage = Nothing
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = Nothing
        Me.cmdCancel.Name = "cmdCancel"
        '
        'cmdOK
        '
        Me.cmdOK.AccessibleDescription = Nothing
        Me.cmdOK.AccessibleName = Nothing
        resources.ApplyResources(Me.cmdOK, "cmdOK")
        Me.cmdOK.BackgroundImage = Nothing
        Me.cmdOK.Font = Nothing
        Me.cmdOK.Name = "cmdOK"
        '
        'chkRemember
        '
        Me.chkRemember.AccessibleDescription = Nothing
        Me.chkRemember.AccessibleName = Nothing
        resources.ApplyResources(Me.chkRemember, "chkRemember")
        Me.chkRemember.BackgroundImage = Nothing
        Me.chkRemember.Font = Nothing
        Me.chkRemember.Name = "chkRemember"
        '
        'cDlg
        '
        resources.ApplyResources(Me.cDlg, "cDlg")
        '
        'PluginsForm
        '
        Me.AcceptButton = Me.cmdApply
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.cmdCancel
        Me.Controls.Add(Me.chkRemember)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.rtbPluginInfo)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PluginsForm"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Const RTF_HEADER As String = "{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}\viewkind4\uc1\pard\f0\fs17\fi-1150\li1150\tx1150"
    Private rtf As String
    Private LookupTable As Hashtable
    Private CheckedList() As Boolean

    Private Sub LoadListBox()
        Dim Plugin As PluginInfo
        Dim Checked As Boolean, Index As Integer
        Dim obj As DictionaryEntry
        Dim objStr As String

        Try
            lstPlugins.CreateControl()
            lstPlugins.Items.Clear()
            LookupTable.Clear()
            If frmMain.m_PluginManager.PluginsList.Count = 0 Then
                Exit Sub
            End If

            ReDim CheckedList(frmMain.m_PluginManager.PluginsList.Count - 1)
            'Add the following code to sort the plugins in the plugin list - 7/29/2005 - Lailin Chen 
            Dim sortedList As ArrayList
            Dim sortMap As Hashtable
            sortedList = New ArrayList
            sortMap = New Hashtable
            For Each obj In frmMain.m_PluginManager.PluginsList
                objStr = (CType(obj.Value, PluginInfo)).Name
                sortedList.Add(objStr)
                sortMap.Add(objStr, obj.Value)
            Next
            sortedList.Sort()
            For Each objStr In sortedList
                Plugin = CType(sortMap.Item(objStr.ToString), PluginInfo)
                Checked = frmMain.m_PluginManager.ContainsKey(frmMain.m_PluginManager.LoadedPlugins, Plugin.Key)
                Index = lstPlugins.Items.Add(Plugin.Name, Checked)
                LookupTable.Add(Index, Plugin.Key)
                CheckedList(Index) = Checked
            Next
        Catch ex As System.Exception
            g_error = ex.Message
            ShowError(ex)
        End Try
    End Sub

    Public Sub LoadForm()
        rtbPluginInfo.Text = ""
        LoadListBox()

        If lstPlugins.Items.Count > 0 Then
            lstPlugins.SelectedIndex = 0
            lstPlugins_SelectedIndexChanged(Me, New System.EventArgs)
        End If

        cmdApply.Enabled = False
    End Sub

    Private Sub DisplayPluginInfo(ByVal PluginIndex As Integer)
        Dim Plugin As PluginInfo

        Plugin = CType(frmMain.m_PluginManager.PluginsList(LookupTable(PluginIndex)), MapWindow.PluginInfo)

        rtbPluginInfo.Text = ""
        rtf = RTF_HEADER
        '10/20/2005 PM
        'DisplayPluginProperty("Name:  ", Plugin.Name)
        'DisplayPluginProperty("Version:  ", Plugin.Version)
        'DisplayPluginProperty("Date:  ", Plugin.BuildDate)
        'DisplayPluginProperty("Author:  ", Plugin.Author)
        'DisplayPluginProperty("Description:  ", Plugin.Description)
        DisplayPluginProperty(resources.GetString("PluginPropertyName.Text"), Plugin.Name)
        DisplayPluginProperty(resources.GetString("PluginPropertyVersion.Text"), Plugin.Version)
        DisplayPluginProperty(resources.GetString("PluginPropertyBuildDate.Text"), Plugin.BuildDate)
        DisplayPluginProperty(resources.GetString("PluginPropertyAuthor.Text"), Plugin.Author)
        DisplayPluginProperty(resources.GetString("PluginPropertyDescription.Text"), Plugin.Description)
        rtbPluginInfo.Rtf = rtf & "}"
        Plugin = Nothing
    End Sub

    Private Sub DisplayPluginProperty(ByVal PropertyName As String, ByVal PropertyValue As String)
        rtf = rtf & "\b " & PropertyName & "\b0\tab " & PropertyValue & "\par" & vbCrLf
    End Sub

    Private Overloads Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.DialogResult = DialogResult.OK
        If cmdApply.Enabled Then cmdApply_Click(sender, New System.EventArgs)
        frmMain.SynchPluginMenu()
        Me.Close()
    End Sub

    Private Overloads Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Overloads Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        Dim i As Integer
        Dim Key As String

        For i = 0 To lstPlugins.Items.Count - 1
            Key = CStr(LookupTable(i))
            If CheckedList(i) = True Then
                If frmMain.m_PluginManager.ContainsKey(frmMain.m_PluginManager.LoadedPlugins, Key) = False Then
                    ' Load the plugin
                    frmMain.m_PluginManager.StartPlugin(Key)
                End If
            Else
                If frmMain.m_PluginManager.ContainsKey(frmMain.m_PluginManager.LoadedPlugins, Key) = True Then
                    ' Unload the plugin
                    frmMain.m_PluginManager.StopPlugin(Key)
                End If
            End If
        Next i
        frmMain.SynchPluginMenu()
        cmdApply.Enabled = False

        'Bugzilla 380 -- Plugins are stored in the project, so a plugin change
        'should set the modified flag.
        frmMain.SetModified(True)
    End Sub

    Private Overloads Sub cmdBrowseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If cDlg.ShowDialog(Me) = DialogResult.OK Then
            Dim newKey As String = frmMain.m_PluginManager.AddFromFile(cDlg.FileName)
            If newKey = String.Empty Then
                Exit Sub
            End If

            Dim i As Integer
            LoadListBox()
            For i = 0 To CheckedList.Length - 1
                If CStr(LookupTable(i)) = newKey Then
                    CheckedList(i) = True
                    lstPlugins.SetSelected(i, True)
                    lstPlugins.SetItemChecked(i, True)
                    Exit For
                End If
            Next
        End If

    End Sub

    Private Sub lstPlugins_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPlugins.SelectedIndexChanged
        If frmMain.m_PluginManager.PluginsList.Count = 0 Then Exit Sub
        DisplayPluginInfo(lstPlugins.SelectedIndex)
        cmdApply.Enabled = True
        frmMain.SetModified(True)
    End Sub

    Private Sub lstPlugins_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lstPlugins.ItemCheck
        CheckedList(e.Index) = CBool(IIf(e.NewValue = CheckState.Checked, True, False))
    End Sub

    Private Sub PluginsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadListBox()
    End Sub

    Private Sub txtPluginFile_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
            e.Handled = True
            cmdBrowseFile_Click(Me, New System.EventArgs)
        End If
    End Sub

    Private Sub btnRefreshList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshList.Click
        frmMain.m_PluginManager.LoadPlugins()

        '7/29/2005 - Lailin Chen - Refresh the listbox
        'Clear everything first
        lstPlugins.Items.Clear()
        LookupTable.Clear()
        Dim Plugin As PluginInfo
        Dim Checked As Boolean, Index As Integer
        Dim obj As DictionaryEntry
        Dim objStr As String
        Dim sortedList As ArrayList
        Dim sortMap As Hashtable
        ReDim CheckedList(frmMain.m_PluginManager.PluginsList.Count - 1)
        'sort the plugins
        sortedList = New ArrayList
        sortMap = New Hashtable
        For Each obj In frmMain.m_PluginManager.PluginsList
            objStr = (CType(obj.Value, PluginInfo)).Name
            sortedList.Add(objStr)
            sortMap.Add(objStr, obj.Value)
        Next
        sortedList.Sort()
        For Each objStr In sortedList
            Plugin = CType(sortMap.Item(objStr.ToString), PluginInfo)
            Checked = frmMain.m_PluginManager.ContainsKey(frmMain.m_PluginManager.LoadedPlugins, Plugin.Key)
            Index = lstPlugins.Items.Add(Plugin.Name, Checked)
            LookupTable.Add(Index, Plugin.Key)
            CheckedList(Index) = Checked
        Next
    End Sub

    Private Sub btnTurnAllOn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTurnAllOn.Click
        For i As Integer = 0 To lstPlugins.Items.Count - 1
            lstPlugins.SetItemChecked(i, True)
            CheckedList(i) = True
        Next
    End Sub

    Private Sub btnTurnAllOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTurnAllOff.Click
        For i As Integer = 0 To lstPlugins.Items.Count - 1
            lstPlugins.SetItemChecked(i, False)
            CheckedList(i) = False
        Next
    End Sub
End Class
