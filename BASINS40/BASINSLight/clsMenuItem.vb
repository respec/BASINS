'********************************************************************************************************
'File Name: clsToolStripMenuItem.vb
'Description: Public class used to access a single menu item through the plugin interface.
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
'1/31/2005 - Total overhaul to remove DotNetBar menu.
'********************************************************************************************************

Public Class ToolStripMenuItem
    Implements Interfaces.MenuItem

    Private m_Menu As Windows.Forms.ToolStripMenuItem

    Private m_BeginsGroup As Boolean
    Private m_Cursor As Cursor
    Private m_Category As String
    Private m_Description As String
    Private m_Displayed As Boolean
    Private m_Name As String
    Private m_Picture As Object
    Private m_Tooltip As String

    Private m_IsSeparator As Boolean = False

    Public Sub New(ByVal Menu As Windows.Forms.ToolStripMenuItem)
        m_Menu = Menu
        m_Name = Menu.Name
    End Sub

    Public Sub New(ByVal Name As String, ByVal Menu As Windows.Forms.ToolStripMenuItem)
        m_Menu = Menu
        m_Name = Name
    End Sub

    Public Sub New(ByVal Name As String, ByVal IsSeparator As Boolean)
        m_Name = Name
        m_IsSeparator = True
    End Sub

    Public ReadOnly Property IsFirstVisibleSubToolStripMenuItem() As Boolean Implements Interfaces.MenuItem.IsFirstVisibleSubmenuItem
        Get
            If m_IsSeparator Then
                If frmMain.m_Menu.m_MenuTable.ContainsKey(frmMain.m_Menu.MenuTableKey(m_Name)) AndAlso Not frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent() Is Nothing Then
                    For i As Integer = 0 To frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent().Items.Count - 1
                        If Not frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent() Is Nothing AndAlso Not frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent().Items(i) Is Nothing Then
                            If frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent().Items(i).Visible And Not frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).GetCurrentParent().Items(i).Name = frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).Name Then
                                Return False
                            End If
                        End If
                    Next
                End If
            Else
                If Not m_Menu.GetCurrentParent() Is Nothing Then
                    For i As Integer = 0 To m_Menu.GetCurrentParent().Items.Count - 1
                        If Not m_Menu.GetCurrentParent() Is Nothing AndAlso Not m_Menu.GetCurrentParent().Items(i) Is Nothing Then
                            If m_Menu.GetCurrentParent().Items(i).Visible And Not m_Menu.GetCurrentParent().Items(i).Name = m_Menu.Name Then
                                Return False
                            End If
                        End If
                    Next
                End If
            End If

            Return True
        End Get
    End Property

    Public Property BeginsGroup() As Boolean Implements Interfaces.MenuItem.BeginsGroup
        Get
            Return m_BeginsGroup
        End Get
        Set(ByVal Value As Boolean)
            m_BeginsGroup = Value
        End Set
    End Property

    Public Property Category() As String Implements Interfaces.MenuItem.Category
        Get
            Return m_Category
        End Get
        Set(ByVal Value As String)
            m_Category = Value
        End Set
    End Property

    Public Property Checked() As Boolean Implements Interfaces.MenuItem.Checked
        Get
            If m_IsSeparator Then Return False

            Return m_Menu.Checked
        End Get
        Set(ByVal Value As Boolean)
            If m_IsSeparator Then Return

            m_Menu.Checked = Value
        End Set
    End Property

    Public Property Cursor() As Cursor Implements Interfaces.MenuItem.Cursor
        Get
            Return m_Cursor
        End Get
        Set(ByVal Value As Cursor)
            m_Cursor = Value
        End Set
    End Property

    Public Property Description() As String Implements Interfaces.MenuItem.Description
        Get
            Return m_Description
        End Get
        Set(ByVal Value As String)
            m_Description = Value
        End Set
    End Property

    Public Property Displayed() As Boolean Implements Interfaces.MenuItem.Displayed
        Get
            Return m_Displayed
        End Get
        Set(ByVal Value As Boolean)
            m_Displayed = True
        End Set
    End Property

    Public Property Enabled() As Boolean Implements Interfaces.MenuItem.Enabled
        Get
            If m_IsSeparator Then Return True 'Disabled separator -- makes no sense

            Return m_Menu.Enabled
        End Get
        Set(ByVal Value As Boolean)
            If m_IsSeparator Then Return

            m_Menu.Enabled = Value
        End Set
    End Property

    Public ReadOnly Property Name() As String Implements Interfaces.MenuItem.Name
        Get
            Return m_Name
        End Get
    End Property

    Public ReadOnly Property NumSubItems() As Integer Implements Interfaces.MenuItem.NumSubItems
        Get
            If m_IsSeparator Then Return 0

            'm_Menu.DropDownItems.Count isn't always reliable,
            'because we don't want to count those whose text are "" (e.g., separators)
            Dim cnt As Integer = 0
            For i As Integer = 0 To m_Menu.DropDownItems.Count - 1
                If Not m_Menu.DropDownItems(i).Text.Trim() = "" Then cnt += 1
            Next

            Return cnt
        End Get
    End Property

    Public Property Picture() As Object Implements Interfaces.MenuItem.Picture
        'This doesn't do anything right now, but is for a future implementation of menus with pictures.
        '1/31/2005 (dpa)
        Get
            If m_IsSeparator Then Return Nothing

            Return m_Picture
        End Get
        Set(ByVal Value As Object)
            If m_IsSeparator Then Return

            m_Picture = Value
        End Set
    End Property

    <CLSCompliant(False)> _
    Public Function SubItem(ByVal MenuName As String) As Interfaces.MenuItem Implements Interfaces.MenuItem.SubItem
        If m_IsSeparator Then Return Nothing

        'Updated 1/31/2005 (dpa)
        Dim i As Integer
        For i = 0 To m_Menu.DropDownItems.Count - 1
            If m_Menu.DropDownItems(i).Text = MenuName OrElse m_Menu.DropDownItems(i).Name = MenuName Then
                Return New ToolStripMenuItem(CType(m_Menu.DropDownItems(i), Windows.Forms.ToolStripMenuItem))
            End If
        Next
        Return Nothing
    End Function

    <CLSCompliant(False)> _
    Public Function SubItem(ByVal Index As Integer) As Interfaces.MenuItem Implements Interfaces.MenuItem.SubItem
        Try
            If m_IsSeparator Then Return Nothing
            '7 Sept 2006 mgray Return a separator ToolStripMenuItem when appropriate instead of Nothing
            Dim lItem As Object = m_Menu.DropDownItems(Index)
            If lItem.GetType.Name.Equals("ToolStripSeparator") Then
                Return New ToolStripMenuItem(lItem.name, True)
            Else
                Return New ToolStripMenuItem(CType(lItem, Windows.Forms.ToolStripMenuItem))
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Property Text() As String Implements Interfaces.MenuItem.Text
        Get
            If m_IsSeparator Then Return "-"

            Return m_Menu.Text
        End Get
        Set(ByVal Value As String)
            If m_IsSeparator Then Exit Property

            m_Menu.Text = Value
        End Set
    End Property

    Public Property Tooltip() As String Implements Interfaces.MenuItem.Tooltip
        Get
            Return m_Tooltip
        End Get
        Set(ByVal Value As String)
            m_Tooltip = Value
        End Set
    End Property

    Public Property Visible() As Boolean Implements Interfaces.MenuItem.Visible
        Get
            If m_IsSeparator Then
                If frmMain.m_Menu.m_MenuTable.ContainsKey(frmMain.m_Menu.MenuTableKey(m_Name)) Then
                    If TypeOf (frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name))) Is ToolStripSeparator Then
                        Return frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).visible
                    End If
                End If
            Else
                Return m_Menu.Visible
            End If
        End Get
        Set(ByVal Value As Boolean)
            If m_IsSeparator Then
                If frmMain.m_Menu.m_MenuTable.ContainsKey(frmMain.m_Menu.MenuTableKey(m_Name)) Then
                    If TypeOf (frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name))) Is ToolStripSeparator Then
                        frmMain.m_Menu.m_MenuTable.Item(frmMain.m_Menu.MenuTableKey(m_Name)).visible = Value
                    End If
                End If
            Else
                m_Menu.Visible = Value
            End If
        End Set
    End Property
End Class
