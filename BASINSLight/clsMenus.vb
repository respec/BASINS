'********************************************************************************************************
'File Name: clsMenus.vb
'Description: Public class used to access menu items through the plugin interface.
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
'1/31/2005 - Total overhaul to remove DotNetBar component. (dpa)
'2/4/2005  - Checks for existing menus, adds menus after menus, works with MapWindow default menus. 
'3/3/2005  - ParentMenu.Length = 0 means add top-level menu (jlk&mg)
'3/10/2005 - keys in m_MenuTable have "&" and " " removed (jlk&mg)
'3/15/2005 - modifications to work on menus that are dynamically added, keys should be distinct from names now. (dpa)
'3/18/2005 - removed duplicate code, now all versions of AddMenu use the same core (mgray)
'********************************************************************************************************
Public Class Menus
    Implements Interfaces.Menus
    Friend m_MenuTable As Hashtable     'This holds all menu objects keyed by the menu name.

    Friend m_PluginAddedMenus As New Hashtable

    Friend Function Contains(ByVal key As Object) As Boolean
        Return m_MenuTable.ContainsKey(MenuTableKey(CStr(key)))
    End Function
    Public Sub New()
        m_MenuTable = New Hashtable(107)
    End Sub
    Protected Overrides Sub Finalize()
        m_MenuTable = Nothing
    End Sub

    Private Sub MenuTrackerAdd(ByVal item As String, ByVal Plugin As String)
        If (m_PluginAddedMenus.Contains(item)) Then
            If Not CType(m_PluginAddedMenus.Item(item), ArrayList).Contains(Plugin) Then
                CType(m_PluginAddedMenus.Item(item), ArrayList).Add(Plugin)
            End If
        Else
            m_PluginAddedMenus.Add(item, New ArrayList())
            CType(m_PluginAddedMenus.Item(item), ArrayList).Add(Plugin)
        End If
    End Sub

    Private Sub MenuTrackerRemove(ByVal item As String, ByVal Plugin As String)
        If (m_PluginAddedMenus.Contains(item)) Then
            If CType(m_PluginAddedMenus.Item(item), ArrayList).Contains(Plugin) Then
                CType(m_PluginAddedMenus.Item(item), ArrayList).Remove(Plugin)
            End If

            If CType(m_PluginAddedMenus.Item(item), ArrayList).Count = 0 Then
                m_PluginAddedMenus.Remove(item)
            End If
        End If
    End Sub

    Public Sub MenuTrackerRemoveIfLastOwner(ByVal plugin As String)
        Dim dellist As New ArrayList
        Dim ienum As IDictionaryEnumerator = m_PluginAddedMenus.GetEnumerator()
        While ienum.MoveNext
            If CType(ienum.Value, ArrayList).Contains(plugin) And CType(ienum.Value, ArrayList).Count = 1 Then
                dellist.Add(ienum.Key.ToString())
            End If
        End While

        For i As Integer = 0 To dellist.Count - 1
            Try
                Remove(dellist(i))
            Catch
            End Try
            Try
                m_PluginAddedMenus.Remove(dellist(i))
            Catch
            End Try
        Next
    End Sub

    <CLSCompliant(False)> _
    Default Public ReadOnly Property Item(ByVal MenuName As String) As Interfaces.MenuItem Implements Interfaces.Menus.Item
        Get
            Dim Menu As Windows.Forms.ToolStripMenuItem = MenuTableItem(MenuName)
            If Menu Is Nothing Then
                Return Nothing
            Else
                Return New ToolStripMenuItem(Menu)
            End If
        End Get
    End Property

    Private Sub RemoveSubMenusFromMenuTable(ByVal ParentToolStripMenuItem As Windows.Forms.ToolStripMenuItem)
        If Not ParentToolStripMenuItem Is Nothing AndAlso Not ParentToolStripMenuItem.DropDownItems Is Nothing Then
            For Each SubMenu As Object In ParentToolStripMenuItem.DropDownItems
                If SubMenu.GetType.Name = "ToolStripMenuItem" Then
                    RemoveSubMenusFromMenuTable(SubMenu)
                End If
                For Each key As String In m_MenuTable.Keys 'Hashtable does not support Remove by value or index
                    If m_MenuTable.Item(key).Equals(SubMenu) Then
                        m_MenuTable.Remove(key)
                        Exit For
                    End If
                Next
            Next
        End If
    End Sub

    Public Function Remove(ByVal MenuName As String) As Boolean Implements Interfaces.Menus.Remove
        'Removes a custom menu from the main menu
        '1/31/2005 - dpa - Updated
        '3/16/2005 - dpa - Modified to remove menu items that have parents other than the main menu
        Dim m As Windows.Forms.ToolStripItem
        Dim key As String = MenuTableKey(MenuName)
        Dim ParentMenu As Windows.Forms.MenuStrip = Nothing
        Dim ParentStrip As Windows.Forms.ToolStrip = Nothing
        Try
            If m_MenuTable.ContainsKey(key) Then
                m = CType(m_MenuTable(key), Windows.Forms.ToolStripItem)
                If m.GetType.Name = "ToolStripMenuItem" Then
                    RemoveSubMenusFromMenuTable(m)
                End If
                m_MenuTable.Remove(key)
                Try
                    Dim o As Object = m.GetCurrentParent
                    If TypeOf (o) Is MenuStrip Then ParentMenu = CType(o, MenuStrip)
                    If TypeOf (o) Is ToolStrip Then ParentStrip = CType(o, ToolStrip)
                    If Not ParentMenu Is Nothing Then
                        ParentMenu.Items.Remove(m)
                    ElseIf Not ParentStrip Is Nothing Then
                        ParentStrip.Items.Remove(m)
                    Else
                        frmMain.MenuStrip1.Items.Remove(m)
                    End If
                Catch e As Exception
                    modMain.CustomExceptionHandler.OnThreadException(e)
                    Return False
                End Try
                Dim stackFrame As New Diagnostics.StackFrame(1)
                Dim pluginfile As String = stackFrame.GetMethod().Module.FullyQualifiedName
                If Not pluginfile.ToLower().EndsWith("mapwindow.exe") Then
                    Dim pluginkey As String = ""
                    Dim info As New PluginInfo
                    If info.Init(pluginfile, GetType(Interfaces.IPlugin).GUID) AndAlso Not info.Key = "" Then
                        pluginkey = info.Key
                    ElseIf info.Init(pluginfile, GetType(PluginInterfaces.IBasePlugin).GUID) AndAlso Not info.Key = "" Then
                        pluginkey = info.Key
                    End If
                    If Not pluginkey = "" Then MenuTrackerRemove(MenuName, pluginkey)
                End If
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MapWinUtility.Logger.Msg(ex.Message)
        End Try
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 1 Adds a menu item to the main menu based on a name only.  
        Return AddMenu(MenuName, "", Nothing, MenuName, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal Picture As Object) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 2 Adds a menu item to the main menu based on a name and a picture. 
        Return AddMenu(MenuName, "", Picture, MenuName, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal Picture As Object, ByVal Text As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 3 Adds a menu item to the main menu using name, text and picture. 
        Return AddMenu(MenuName, "", Picture, Text, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String, ByVal Picture As Object) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 5 Adds a menu item to the main menu using name, parent menu, and picture. 
        Return AddMenu(MenuName, ParentMenu, Picture, MenuName, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String, ByVal Picture As Object, ByVal [Text] As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 6 Adds a menu item to the main menu using name, parent menu, text and picture. 
        Return AddMenu(MenuName, ParentMenu, Picture, Text, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 7 Adds a menu item to the main menu using name, parent menu.
        Return AddMenu(MenuName, ParentMenu, Nothing, MenuName, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String, ByVal Text As String, ByVal Before As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 8 Adds a menu item to the main menu using name, parent menu, text, and what menu to add BEFORE.
        'If the third parameter is null and the fourth isn't, then overload 6 was really intended, so pass it that way (null image, nonnull text).
        'This is because passing "nothing" intending to fill the "picture" parameter won't work, VS will assume it's a string and call that overload.
        If (Text Is Nothing And Not Before Is Nothing) Then
            Return AddMenu(MenuName, ParentMenu, Nothing, Before, "")
        Else
            Return AddMenu(MenuName, ParentMenu, Nothing, Text, "", Before)
        End If
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String, ByVal Picture As Object, ByVal Text As String, ByVal After As String) As Interfaces.MenuItem Implements Interfaces.Menus.AddMenu
        'AddMenu Type 4
        'Adds a menu item to the main menu using name, parent menu, text, picture and "after"
        'Note that pictures have been disabled because they aren't
        'supported in the standard windows forms menu.  However there is sample code available on line for extending the
        'standard menu item, so pictures could be added back in later.
        'Updated 2/4/2005 (dpa)
        Return AddMenu(MenuName, ParentMenu, Picture, Text, After, "")
    End Function

    <CLSCompliant(False)> _
    Public Function AddMenu(ByVal MenuName As String, ByVal ParentMenu As String, ByVal Picture As Object, ByVal Text As String, ByVal After As String, ByVal Before As String) As Interfaces.MenuItem
        'The *real* addMenu function, called by the various overloads of AddMenu.
        'After and Before shouldn't both be set (at least one must be "")
        'This particular overload is internal, not exposed on the interface.
        Dim NewMenu As Windows.Forms.ToolStripItem
        Dim AfterMenu As Windows.Forms.ToolStripItem
        Dim BeforeMenu As Windows.Forms.ToolStripItem

        Try
            If MenuName = "" Then
                g_error = "Menu name not specified in function: AddMenu type 4."
                Return Nothing
            End If
            If Text = "" Then
                Text = MenuName
            End If
            'First try to get this menu item from the toolbar - in other words check if it already exists. 
            NewMenu = MenuTableItem(MenuName)
            If NewMenu Is Nothing Then
                'The menu item does not exist, so we have to add it.
                If ParentMenu.Length = 0 Then
                    'Find the correct position for the menu using AFter or Before
                    Dim InsertPosition As Long = frmMain.MenuStrip1.Items.Count
                    If After.Length > 0 Then
                        AfterMenu = MenuTableItem(After)
                        If Not AfterMenu Is Nothing Then
                            For i As Integer = 0 To frmMain.MenuStrip1.Items.Count - 1
                                If frmMain.MenuStrip1.Items(i).Name = AfterMenu.Name Then
                                    InsertPosition = i + 1
                                    Exit For
                                End If
                            Next
                        Else
                            g_error = "No menu item with name " & After & " exists."
                            ' We still want to return it, because the menu was added successfully.
                            ' It just might not be in the right place, hence setting g_error.
                            ' // Return Nothing
                        End If
                    ElseIf Before.Length > 0 Then
                        BeforeMenu = MenuTableItem(Before)
                        If Not BeforeMenu Is Nothing Then
                            For i As Integer = 0 To frmMain.MenuStrip1.Items.Count - 1
                                If frmMain.MenuStrip1.Items(i).Name = BeforeMenu.Name Then
                                    InsertPosition = i
                                    Exit For
                                End If
                            Next
                        Else
                            g_error = "No menu item with name " & Before & " exists."
                            ' We still want to return it, because the menu was added successfully.
                            ' It just might not be in the right place, hence setting g_error.
                            ' // Return Nothing
                        End If
                    End If

                    NewMenu = frmMain.MenuStrip1.Items.Add(Text, Picture, New EventHandler(AddressOf frmMain.CustomMenu_Click))
                    NewMenu.Name = MenuName
                    'No other way to get the event handler than the above; no useful overloads
                    'So now remove it and readd it where it's supposed to be.
                    frmMain.MenuStrip1.Items.Remove(NewMenu)
                    frmMain.MenuStrip1.Items.Insert(InsertPosition, NewMenu)
                Else
                    Dim Parent As Windows.Forms.ToolStripMenuItem = MenuTableItem(ParentMenu)
                    If Not Parent Is Nothing Then
                        'Find the correct position for the menu using AFter or Before
                        Dim InsertPosition As Long = Parent.DropDownItems.Count
                        If After.Length > 0 Then
                            AfterMenu = MenuTableItem(After)
                            If Not AfterMenu Is Nothing Then
                                For i As Integer = 0 To Parent.DropDownItems.Count - 1
                                    If Parent.DropDownItems(i).Name = AfterMenu.Name Then
                                        InsertPosition = i + 1
                                        Exit For
                                    End If
                                Next
                            Else
                                g_error = "No menu item with name " & After & " exists."
                                ' We still want to return it, because the menu was added successfully.
                                ' It just might not be in the right place, hence setting g_error.
                                ' // Return Nothing
                            End If
                        ElseIf Before.Length > 0 Then
                            BeforeMenu = MenuTableItem(Before)
                            If Not BeforeMenu Is Nothing Then
                                For i As Integer = 0 To Parent.DropDownItems.Count - 1
                                    If Parent.DropDownItems(i).Name = BeforeMenu.Name Then
                                        InsertPosition = i
                                        Exit For
                                    End If
                                Next
                            Else
                                g_error = "No menu item with name " & Before & " exists."
                                ' We still want to return it, because the menu was added successfully.
                                ' It just might not be in the right place, hence setting g_error.
                                ' // Return Nothing
                            End If
                        End If

                        NewMenu = Parent.DropDownItems.Add(Text, Picture, New EventHandler(AddressOf frmMain.CustomMenu_Click))
                        NewMenu.Name = MenuName
                        'No other way to get the event handler than the above; no useful overloads
                        'So now remove it and readd it where it's supposed to be.
                        Parent.DropDownItems.Remove(NewMenu)
                        Parent.DropDownItems.Insert(InsertPosition, NewMenu)
                    Else
                        'The parent object doesn't exist, this means that they gave a 
                        'bad key name for the parent.  
                        g_error = "Parent menu does not exist."
                        Return Nothing
                    End If
                End If
                MenuTableAdd(MenuName, NewMenu)
            End If
            EnsureHelpItemLast()
            Dim newItem As Interfaces.MenuItem
            If Not TypeOf (NewMenu) Is ToolStripSeparator Then
                newItem = New ToolStripMenuItem(MenuName, NewMenu)
            Else
                newItem = New ToolStripMenuItem(MenuName, True)
            End If

            Return newItem
        Catch ex As System.Exception
            g_error = ex.Message
            ShowError(ex)
            Return Nothing
        End Try
    End Function
    Friend Function MenuTableKey(ByVal Name As String) As String
        'Keys have ampersands and spaces removed
        Return Name.Replace("&", "").Replace(" ", "")
    End Function
    Private Sub MenuTableAdd(ByVal Name As String, ByVal NewMenu As Windows.Forms.ToolStripItem)
        Dim stackFrame As New Diagnostics.StackFrame(1)
        Dim pluginkey As String = FindPluginKey()
        If Not pluginkey = "" Then MenuTrackerAdd(Name, pluginkey)

        'Add a ToolStripMenuItem to m_MenuTable with a key formatted by MenuTableKey
        m_MenuTable.Add(MenuTableKey(Name), NewMenu)
    End Sub

    Private Function FindPluginKey() As String
        Try
            Dim i As Integer = 1
            While True
                Dim sf As New Diagnostics.StackFrame(i)
                If sf Is Nothing Then Return ""
                If sf.GetMethod() Is Nothing Then Return ""
                If sf.GetMethod().Module Is Nothing Then Return ""
                If sf.GetMethod().Module.FullyQualifiedName.ToLower().Contains("plugins") Then
                    Dim info As New PluginInfo
                    If info.Init(sf.GetMethod().Module.FullyQualifiedName, GetType(Interfaces.IPlugin).GUID) AndAlso Not info.Key = "" Then
                        Return info.Key
                    ElseIf info.Init(sf.GetMethod().Module.FullyQualifiedName, GetType(MapWindow.PluginInterfaces.IBasePlugin).GUID) AndAlso Not info.Key = "" Then
                        Return info.Key
                    End If
                End If
                i += 1
            End While
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try

        Return ""
    End Function

    Private Function MenuTableItem(ByVal Name As String) As Windows.Forms.ToolStripMenuItem
        'Retrieve a ToolStripMenuItem from m_MenuTable using a properly formatted key
        Dim key As String = MenuTableKey(Name)
        If m_MenuTable.ContainsKey(key) Then
            If Not TypeOf (m_MenuTable.Item(key)) Is ToolStripSeparator Then
                Return CType(m_MenuTable.Item(key), Windows.Forms.ToolStripMenuItem)
            Else
                Dim newItem As New System.Windows.Forms.ToolStripMenuItem("-")
                newItem.Name = Name
                Return newItem
            End If
        Else
            Return Nothing
        End If
    End Function
    'Chris Michaelis 1/2/2006
    Private Sub EnsureHelpItemLast()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MapWindowForm))
        For i As Integer = 0 To frmMain.MenuStrip1.Items.Count - 1
            If frmMain.MenuStrip1.Items(i).Text = resources.GetString("mnuHelp.Text") Then
                Dim o As Object = frmMain.MenuStrip1.Items(i)
                frmMain.MenuStrip1.Items.RemoveAt(i)
                frmMain.MenuStrip1.Items.Add(o)
                'MergeIndex does not work as advertised; but the above code accomplishes the same thing.
                'frmMain.MenuStrip1.Items(i).MergeIndex = Math.Max(0, frmMain.MenuStrip1.Items.Count - 1)
                Exit For
            End If
        Next
    End Sub
End Class
